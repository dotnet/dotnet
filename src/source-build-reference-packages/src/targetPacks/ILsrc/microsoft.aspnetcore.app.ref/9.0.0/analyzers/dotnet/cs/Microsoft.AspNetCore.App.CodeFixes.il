
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
// Time-date stamp:            0xa062753e
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000a600
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000c47a
// Base of code:                   0x00002000
// Base of data:                   0x0000e000
// Image base:                     0x10000000
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
// Checksum:                       0x00010eac
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000c425 [0x0000004f] address [size] of Import Directory:          
// 0x0000e000 [0x0000051c] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000b000 [0x00002918] address [size] of Security Directory:        
// 0x00010000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000c320 [0x00000054] address [size] of Debug Directory:           
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
//              0x0000a480 Virtual Size
//              0x00002000 Virtual Address
//              0x0000a600 Size of Raw Data
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
//              0x0000051c Virtual Size
//              0x0000e000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000a800 Pointer to Raw Data
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
//              0x0000ae00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x0000047c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000c467 Import Name Table
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
// 0x000057e8 [0x00006ab8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000c2a0 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x0000252c Size
//              '#~' Name
//    Stream 2:
//              0x00002598 Offset
//              0x00002104 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000469c Offset
//              0x00000958 Size
//              '#US' Name
//    Stream 4:
//              0x00004ff4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00005004 Offset
//              0x00001ab4 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x00000e090ba21f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 55576
// PE header size       : 512 (496 used)    ( 0.92%)
// PE additional info   : 12083             (21.74%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.13%)
// CLR meta-data size  : 27320              (49.16%)
// CLR additional info : 128                ( 0.23%)
// CLR method headers  : 1010               ( 1.82%)
// Managed code         : 13167             (23.69%)
// Data                 : 2048              ( 3.69%)
// Unaccounted          : -764              (-1.37%)

// Num.of PE sections   : 3
//   .text    - 42496
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 27320
//   Module        -    1 (10 bytes)
//   TypeDef       -   44 (616 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -  149 (894 bytes)
//   MethodDef     -  143 (2002 bytes)    0 abstract, 0 native, 143 bodies
//   FieldDef      -  127 (762 bytes)     3 constant
//   MemberRef     -  242 (1452 bytes)
//   ParamDef      -  173 (1038 bytes)
//   MethodImpl    -   18 (108 bytes)
//   Constant      -    5 (30 bytes)
//   CustomAttribute-  222 (1332 bytes)
//   StandAloneSig -   46 (92 bytes)
//   InterfaceImpl -   10 (40 bytes)
//   PropertyMap   -    9 (36 bytes)
//   Property      -   11 (66 bytes)
//   MethodSemantic-   13 (78 bytes)
//   TypeSpec      -   38 (76 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    9 (180 bytes)
//   NestedClass   -   25 (100 bytes)
//   GenericParam  -   38 (304 bytes)
//   MethodSpec    -   42 (168 bytes)
//   Strings       -  8451 bytes
//   Blobs         -  6836 bytes
//   UserStrings   -  2392 bytes
//   Guids         -    16 bytes
//   Uncategorized -   219 bytes

// CLR additional info : 128

// CLR method headers : 1010
//   Num.of method bodies  - 143
//   Num.of fat headers    - 51
//   Num.of tiny headers   - 92
//   Num.of fat sections   - 8
//   Num.of small sections - 3

// Managed code : 13167
//   Ave method size - 92
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class IsReadOnlyAttribute            (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class ExtensionMethodsCache          (abstract) (auto) (ansi) (sealed) 
// Class HashCode                       (sequential) (ansi) (sealed) 
// Class PackageSourceAndNamespace      (sequential) (ansi) (sealed) 
// Class ThisAndExtensionMethod         (sequential) (ansi) (sealed) 
// Class WebApplicationBuilderFixer     (public) (auto) (ansi) (sealed) 
// Class HeaderDictionaryAddFixer       (public) (auto) (ansi) (sealed) 
// Class HeaderDictionaryIndexerFixer   (public) (auto) (ansi) 
// Class DetectMismatchedParameterOptionalityFixer (public) (auto) (ansi) 
// Class ExtensionMethodsCompletionProvider (public) (auto) (ansi) (sealed) 
// Class RouteParameterUnusedParameterFixer (public) (auto) (ansi) 
// Class AddPackageFixer                (public) (auto) (ansi) 
// Class AddAuthorizationBuilderFixer   (public) (auto) (ansi) (sealed) 
// Class SR                             (abstract) (auto) (ansi) (sealed) (nested private) 
// Class BitOperations                  (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_1           (auto) (ansi) (sealed) (nested private) 
// Class <RegisterCodeFixesAsync>d__4   (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_1           (auto) (ansi) (sealed) (nested private) 
// Class <RegisterCodeFixesAsync>d__4   (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_1           (auto) (ansi) (sealed) (nested private) 
// Class <FixHeaderDictionaryIndexer>d__5 (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_1           (auto) (ansi) (sealed) (nested private) 
// Class <FixMismatchedParameterOptionalityAsync>d__5 (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass0_0           (auto) (ansi) (sealed) (nested private) 
// Class <ProvideCompletionsAsync>d__0  (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass5_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass5_1           (auto) (ansi) (sealed) (nested private) 
// Class <RegisterCodeFixesAsync>d__5   (auto) (ansi) (sealed) (nested private) 
// Class <RegisterCodeFixesAsync>d__0   (auto) (ansi) (sealed) (nested private) 
// Class <TryCreateCodeActionAsync>d__5 (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_1           (auto) (ansi) (sealed) (nested private) 
// Class <RegisterCodeFixesAsync>d__4   (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ netstandard
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 2:0:0:0
}
.assembly extern /*23000002*/ Microsoft.AspNetCore.App.Analyzers
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 9:0:0:0
}
.assembly extern /*23000003*/ Microsoft.CodeAnalysis
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:11:0:0
}
.assembly extern /*23000004*/ Microsoft.CodeAnalysis.Workspaces
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:11:0:0
}
.assembly extern /*23000005*/ System.Composition.AttributedModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000006*/ System.Collections.Immutable
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000007*/ Microsoft.CodeAnalysis.CSharp
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:11:0:0
}
.assembly extern /*23000008*/ Microsoft.CodeAnalysis.Features
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:11:0:0
}
.assembly extern /*23000009*/ Microsoft.CodeAnalysis.ExternalAccess.AspNetCore
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:11:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.App.CodeFixes
{
  .custom /*0C000004:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000003*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [netstandard/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000004*/ instance void [netstandard/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 19 2E 4E 45 54 53 74 61 6E 64 61 72 64 2C   // ....NETStandard,
                                                                                                                                                                        56 65 72 73 69 6F 6E 3D 76 32 2E 30 01 00 54 0E   // Version=v2.0..T.
                                                                                                                                                                        14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                        79 4E 61 6D 65 11 2E 4E 45 54 20 53 74 61 6E 64   // yName..NET Stand
                                                                                                                                                                        61 72 64 20 32 2E 30 )                            // ard 2.0
  .custom /*0C000008:0A000005*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 81 73 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...sMicrosoft.As
                                                                                                                                                                                 70 4E 65 74 43 6F 72 65 2E 41 70 70 2E 41 6E 61   // pNetCore.App.Ana
                                                                                                                                                                                 6C 79 7A 65 72 73 2E 54 65 73 74 2C 20 50 75 62   // lyzers.Test, Pub
                                                                                                                                                                                 6C 69 63 4B 65 79 3D 30 30 32 34 30 30 30 30 30   // licKey=002400000
                                                                                                                                                                                 34 38 30 30 30 30 30 39 34 30 30 30 30 30 30 30   // 4800000940000000
                                                                                                                                                                                 36 30 32 30 30 30 30 30 30 32 34 30 30 30 30 35   // 6020000002400005
                                                                                                                                                                                 32 35 33 34 31 33 31 30 30 30 34 30 30 30 30 30   // 2534131000400000
                                                                                                                                                                                 31 30 30 30 31 30 30 66 33 33 61 32 39 30 34 34   // 1000100f33a29044
                                                                                                                                                                                 66 61 39 64 37 34 30 63 39 62 33 32 31 33 61 39   // fa9d740c9b3213a9
                                                                                                                                                                                 33 65 35 37 63 38 34 62 34 37 32 63 38 34 65 30   // 3e57c84b472c84e0
                                                                                                                                                                                 62 38 61 30 65 31 61 65 34 38 65 36 37 61 39 66   // b8a0e1ae48e67a9f
                                                                                                                                                                                 38 66 36 64 65 39 64 35 66 37 66 33 64 35 32 61   // 8f6de9d5f7f3d52a
                                                                                                                                                                                 63 32 33 65 34 38 61 63 35 31 38 30 31 66 31 64   // c23e48ac51801f1d
                                                                                                                                                                                 63 39 35 30 61 62 65 39 30 31 64 61 33 34 64 32   // c950abe901da34d2
                                                                                                                                                                                 61 39 65 33 62 61 61 64 62 31 34 31 61 31 37 63   // a9e3baadb141a17c
                                                                                                                                                                                 37 37 65 66 33 63 35 36 35 64 64 35 65 65 35 30   // 77ef3c565dd5ee50
                                                                                                                                                                                 35 34 62 39 31 63 66 36 33 62 62 33 63 36 61 62   // 54b91cf63bb3c6ab
                                                                                                                                                                                 38 33 66 37 32 61 62 33 61 61 66 65 39 33 64 30   // 83f72ab3aafe93d0
                                                                                                                                                                                 66 63 33 63 32 33 34 38 62 37 36 34 66 61 66 62   // fc3c2348b764fafb
                                                                                                                                                                                 30 62 31 63 30 37 33 33 64 65 35 31 34 35 39 61   // 0b1c0733de51459a
                                                                                                                                                                                 65 61 62 34 36 35 38 30 33 38 34 62 66 39 64 37   // eab46580384bf9d7
                                                                                                                                                                                 34 63 34 65 32 38 31 36 34 62 37 63 64 65 32 34   // 4c4e28164b7cde24
                                                                                                                                                                                 37 66 38 39 31 62 61 30 37 38 39 31 63 39 64 38   // 7f891ba07891c9d8
                                                                                                                                                                                 37 32 61 64 32 62 62 00 00 )                      // 72ad2bb..
  .custom /*0C000009:0A000006*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                      string) /* 0A000006 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 61 66   // ...CommitHash(af
                                                                                                                                                                 32 32 65 66 66 61 65 34 30 36 39 61 35 64 66 62   // 22effae4069a5dfb
                                                                                                                                                                 39 62 30 37 33 35 38 35 39 64 65 34 38 38 32 30   // 9b0735859de48820
                                                                                                                                                                 31 30 34 66 35 62 00 00 )                         // 104f5b..
  .custom /*0C00000A:0A000006*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                      string) /* 0A000006 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                 72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                 62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                 6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 61 66 32   // netcore/tree/af2
                                                                                                                                                                 32 65 66 66 61 65 34 30 36 39 61 35 64 66 62 39   // 2effae4069a5dfb9
                                                                                                                                                                 62 30 37 33 35 38 35 39 64 65 34 38 38 32 30 31   // b0735859de488201
                                                                                                                                                                 30 34 66 35 62 00 00 )                            // 04f5b..
  .custom /*0C00000B:0A000006*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                      string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                 72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A000008*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C00000E:0A000009*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                  43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                  20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                  2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000A*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 22 43 53 68 61 72 70 20 43 6F 64 65 46 69   // .."CSharp CodeFi
                                                                                                                                                                    78 65 73 20 66 6F 72 20 41 53 50 2E 4E 45 54 20   // xes for ASP.NET 
                                                                                                                                                                    43 6F 72 65 2E 00 00 )                            // Core...
  .custom /*0C000010:0A00000B*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 39 30 33 00   // ...9.0.24.52903.
                                                                                                                                                                    00 ) 
  .custom /*0C000011:0A00000C*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 3A 39 2E 30 2E 30 2D 72 74 6D 2E 32 34 35   // ..:9.0.0-rtm.245
                                                                                                                                                                             32 39 2E 33 2B 61 66 32 32 65 66 66 61 65 34 30   // 29.3+af22effae40
                                                                                                                                                                             36 39 61 35 64 66 62 39 62 30 37 33 35 38 35 39   // 69a5dfb9b0735859
                                                                                                                                                                             64 65 34 38 38 32 30 31 30 34 66 35 62 00 00 )    // de48820104f5b..
  .custom /*0C000012:0A00000D*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000013:0A00000E*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 22 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // .."Microsoft.Asp
                                                                                                                                                              4E 65 74 43 6F 72 65 2E 41 70 70 2E 43 6F 64 65   // NetCore.App.Code
                                                                                                                                                              46 69 78 65 73 00 00 )                            // Fixes..
  .custom /*0C000014:0A000006*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                      string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                 24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                 74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000015:0A00000F*/ instance void [netstandard/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                F3 3A 29 04 4F A9 D7 40 C9 B3 21 3A 93 E5 7C 84   // .:).O..@..!:..|.
                B4 72 C8 4E 0B 8A 0E 1A E4 8E 67 A9 F8 F6 DE 9D   // .r.N......g.....
                5F 7F 3D 52 AC 23 E4 8A C5 18 01 F1 DC 95 0A BE   // _.=R.#..........
                90 1D A3 4D 2A 9E 3B AA DB 14 1A 17 C7 7E F3 C5   // ...M*.;......~..
                65 DD 5E E5 05 4B 91 CF 63 BB 3C 6A B8 3F 72 AB   // e.^..K..c.<j.?r.
                3A AF E9 3D 0F C3 C2 34 8B 76 4F AF B0 B1 C0 73   // :..=...4.vO....s
                3D E5 14 59 AE AB 46 58 03 84 BF 9D 74 C4 E2 81   // =..Y..FX....t...
                64 B7 CD E2 47 F8 91 BA 07 89 1C 9D 87 2A D2 BB ) // d...G........*..
  .hash algorithm 0x00008004
  .ver 9:0:0:0
}
.module Microsoft.AspNetCore.App.CodeFixes.dll
// MVID: {18ed86e9-5593-4151-935b-448581b00703}
.custom /*0C000002:06000007*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000007*/::.ctor(int32) /* 06000007 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:06000006*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000006*/::.ctor(bool) /* 06000006 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F11995C8000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000017:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.IsReadOnlyAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001A:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method IsReadOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.IsReadOnlyAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001D:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000011*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000067       */ newarr     [netstandard/*23000001*/]System.Byte/*01000067*/
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
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000004*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000021:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000011*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000026:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000027:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:0A000011*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000006*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000007*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00002B:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00002C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002D:0A000011*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                    4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                    72 69 74 65 64 00 )                               // rited.
  .field /*04000004*/ public initonly int32 Version
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 A_1) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20a5
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000007*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000008*/ private abstract auto ansi sealed beforefieldinit ExtensionMethodsCache
       extends [netstandard/*23000001*/]System.Object/*01000015*/
{
  .method /*06000008*/ public hidebysig static 
          class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> 
          ConstructFromWellKnownTypes(class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ wellKnownTypes) cil managed
  // SIG: 00 01 15 12 59 02 11 2C 11 28 12 5D
  {
    .custom /*0C000030:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20b4
    // Code size       84 (0x54)
    .maxstack  5
    IL_0000:  /* 73   | (0A)00001D       */ newobj     instance void class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000002*/::.ctor() /* 0A00001D */
    IL_0005:  /* 25   |                  */ dup
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 1F   | 6A               */ ldc.i4.s   106
    IL_0009:  /* 6F   | (0A)00001E       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.INamedTypeSymbol/*01000068*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/::Get(valuetype [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypeData/*01000043*//WellKnownType/*01000044*/) /* 0A00001E */
    IL_000e:  /* 72   | (70)000001       */ ldstr      "AddOpenApi" /* 70000001 */
    IL_0013:  /* 73   | (06)00002C       */ newobj     instance void Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::.ctor(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/,
                                                                                                                                              string) /* 0600002C */
    IL_0018:  /* 72   | (70)000017       */ ldstr      "Microsoft.AspNetCore.OpenApi" /* 70000017 */
    IL_001d:  /* 72   | (70)000051       */ ldstr      "Microsoft.Extensions.DependencyInjection" /* 70000051 */
    IL_0022:  /* 73   | (06)00001F       */ newobj     instance void Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::.ctor(string,
                                                                                                                                                 string) /* 0600001F */
    IL_0027:  /* 6F   | (0A)00001F       */ callvirt   instance void class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000002*/::Add(!0,
                                                                                                                                                                                                                                                                                                                                     !1) /* 0A00001F */
    IL_002c:  /* 25   |                  */ dup
    IL_002d:  /* 02   |                  */ ldarg.0
    IL_002e:  /* 1F   | 26               */ ldc.i4.s   38
    IL_0030:  /* 6F   | (0A)00001E       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.INamedTypeSymbol/*01000068*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/::Get(valuetype [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypeData/*01000043*//WellKnownType/*01000044*/) /* 0A00001E */
    IL_0035:  /* 72   | (70)0000A3       */ ldstr      "MapOpenApi" /* 700000A3 */
    IL_003a:  /* 73   | (06)00002C       */ newobj     instance void Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::.ctor(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/,
                                                                                                                                              string) /* 0600002C */
    IL_003f:  /* 72   | (70)000017       */ ldstr      "Microsoft.AspNetCore.OpenApi" /* 70000017 */
    IL_0044:  /* 72   | (70)0000B9       */ ldstr      "Microsoft.AspNetCore.Builder" /* 700000B9 */
    IL_0049:  /* 73   | (06)00001F       */ newobj     instance void Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::.ctor(string,
                                                                                                                                                 string) /* 0600001F */
    IL_004e:  /* 6F   | (0A)00001F       */ callvirt   instance void class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000002*/::Add(!0,
                                                                                                                                                                                                                                                                                                                                     !1) /* 0A00001F */
    IL_0053:  /* 2A   |                  */ ret
  } // end of method ExtensionMethodsCache::ConstructFromWellKnownTypes

} // end of class ExtensionMethodsCache

.class /*02000009*/ private sequential ansi sealed beforefieldinit HashCode
       extends [netstandard/*23000001*/]System.ValueType/*01000018*/
{
  .custom /*0C000033:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000034:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .class /*02000014*/ abstract auto ansi sealed nested private beforefieldinit SR
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .field /*0400001F*/ public static string HashCode_HashCodeNotSupported
    .field /*04000020*/ public static string HashCode_EqualityNotSupported
    .method /*06000063*/ private hidebysig specialname rtspecialname static 
            void  .cctor() cil managed
    // SIG: 00 00 01
    {
      // Method begins at RVA 0x404d
      // Code size       21 (0x15)
      .maxstack  8
      IL_0000:  /* 72   | (70)00053F       */ ldstr      "HashCode is a mutable struct and should not be com"
      + "pared with other HashCodes. Use ToHashCode to retrieve the computed hash code." /* 7000053F */
      IL_0005:  /* 80   | (04)00001F       */ stsfld     string HashCode/*02000009*//SR/*02000014*/::HashCode_HashCodeNotSupported /* 0400001F */
      IL_000a:  /* 72   | (70)000642       */ ldstr      "HashCode is a mutable struct and should not be com"
      + "pared with other HashCodes." /* 70000642 */
      IL_000f:  /* 80   | (04)000020       */ stsfld     string HashCode/*02000009*//SR/*02000014*/::HashCode_EqualityNotSupported /* 04000020 */
      IL_0014:  /* 2A   |                  */ ret
    } // end of method SR::.cctor

  } // end of class SR

  .class /*02000015*/ abstract auto ansi sealed nested private beforefieldinit BitOperations
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .method /*06000064*/ public hidebysig static 
            uint32  RotateLeft(uint32 'value',
                               int32 offset) cil managed aggressiveinlining
    // SIG: 00 02 09 09 08
    {
      // Method begins at RVA 0x4063
      // Code size       17 (0x11)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 03   |                  */ ldarg.1
      IL_0002:  /* 1F   | 1F               */ ldc.i4.s   31
      IL_0004:  /* 5F   |                  */ and
      IL_0005:  /* 62   |                  */ shl
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 1F   | 20               */ ldc.i4.s   32
      IL_0009:  /* 03   |                  */ ldarg.1
      IL_000a:  /* 59   |                  */ sub
      IL_000b:  /* 1F   | 1F               */ ldc.i4.s   31
      IL_000d:  /* 5F   |                  */ and
      IL_000e:  /* 64   |                  */ shr.un
      IL_000f:  /* 60   |                  */ or
      IL_0010:  /* 2A   |                  */ ret
    } // end of method BitOperations::RotateLeft

    .method /*06000065*/ public hidebysig static 
            uint64  RotateLeft(uint64 'value',
                               int32 offset) cil managed aggressiveinlining
    // SIG: 00 02 0B 0B 08
    {
      // Method begins at RVA 0x4075
      // Code size       17 (0x11)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 03   |                  */ ldarg.1
      IL_0002:  /* 1F   | 3F               */ ldc.i4.s   63
      IL_0004:  /* 5F   |                  */ and
      IL_0005:  /* 62   |                  */ shl
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 1F   | 40               */ ldc.i4.s   64
      IL_0009:  /* 03   |                  */ ldarg.1
      IL_000a:  /* 59   |                  */ sub
      IL_000b:  /* 1F   | 3F               */ ldc.i4.s   63
      IL_000d:  /* 5F   |                  */ and
      IL_000e:  /* 64   |                  */ shr.un
      IL_000f:  /* 60   |                  */ or
      IL_0010:  /* 2A   |                  */ ret
    } // end of method BitOperations::RotateLeft

  } // end of class BitOperations

  .field /*04000005*/ private static initonly uint32 s_seed
  .field /*04000006*/ private static literal uint32 Prime1 = uint32(0x9E3779B1)
  .field /*04000007*/ private static literal uint32 Prime2 = uint32(0x85EBCA77)
  .field /*04000008*/ private static literal uint32 Prime3 = uint32(0xC2B2AE3D)
  .field /*04000009*/ private static literal uint32 Prime4 = uint32(0x27D4EB2F)
  .field /*0400000A*/ private static literal uint32 Prime5 = uint32(0x165667B1)
  .field /*0400000B*/ private uint32 _v1
  .field /*0400000C*/ private uint32 _v2
  .field /*0400000D*/ private uint32 _v3
  .field /*0400000E*/ private uint32 _v4
  .field /*0400000F*/ private uint32 _queue1
  .field /*04000010*/ private uint32 _queue2
  .field /*04000011*/ private uint32 _queue3
  .field /*04000012*/ private uint32 _length
  .method /*06000009*/ private hidebysig static 
          uint32  GenerateGlobalSeed() cil managed
  // SIG: 00 00 09
  {
    // Method begins at RVA 0x2114
    // Code size       42 (0x2a)
    .maxstack  2
    .locals /*11000001*/ init (class [netstandard/*23000001*/]System.Security.Cryptography.RandomNumberGenerator/*01000019*/ V_0,
             uint8[] V_1,
             uint32 V_2)
    IL_0000:  /* 28   | (0A)000020       */ call       class [netstandard/*23000001*/]System.Security.Cryptography.RandomNumberGenerator/*01000019*/ [netstandard/*23000001*/]System.Security.Cryptography.RandomNumberGenerator/*01000019*/::Create() /* 0A000020 */
    IL_0005:  /* 0A   |                  */ stloc.0
    .try
    {
      IL_0006:  /* 1A   |                  */ ldc.i4.4
      IL_0007:  /* 8D   | (01)000067       */ newarr     [netstandard/*23000001*/]System.Byte/*01000067*/
      IL_000c:  /* 0B   |                  */ stloc.1
      IL_000d:  /* 06   |                  */ ldloc.0
      IL_000e:  /* 07   |                  */ ldloc.1
      IL_000f:  /* 6F   | (0A)000021       */ callvirt   instance void [netstandard/*23000001*/]System.Security.Cryptography.RandomNumberGenerator/*01000019*/::GetBytes(uint8[]) /* 0A000021 */
      IL_0014:  /* 07   |                  */ ldloc.1
      IL_0015:  /* 16   |                  */ ldc.i4.0
      IL_0016:  /* 28   | (0A)000022       */ call       uint32 [netstandard/*23000001*/]System.BitConverter/*01000069*/::ToUInt32(uint8[],
                                                                                                                                   int32) /* 0A000022 */
      IL_001b:  /* 0C   |                  */ stloc.2
      IL_001c:  /* DE   | 0A               */ leave.s    IL_0028

    }  // end .try
    finally
    {
      IL_001e:  /* 06   |                  */ ldloc.0
      IL_001f:  /* 2C   | 06               */ brfalse.s  IL_0027

      IL_0021:  /* 06   |                  */ ldloc.0
      IL_0022:  /* 6F   | (0A)000023       */ callvirt   instance void [netstandard/*23000001*/]System.IDisposable/*0100006A*/::Dispose() /* 0A000023 */
      IL_0027:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 06 00 18 1E 00 0A 00 00 00 00
    IL_0028:  /* 08   |                  */ ldloc.2
    IL_0029:  /* 2A   |                  */ ret
  } // end of method HashCode::GenerateGlobalSeed

  .method /*0600000A*/ public hidebysig static 
          int32  Combine<T1>(!!T1 value1) cil managed
  // SIG: 10 01 01 08 1E 00
  {
    .param type T1 /*2A000001*/ 
      .custom /*0C000016:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x215c
    // Code size       69 (0x45)
    .maxstack  2
    .locals /*11000002*/ init (uint32 V_0,
             !!T1 V_1)
    IL_0000:  /* 0F   | 00               */ ldarga.s   value1
    IL_0002:  /* 12   | 01               */ ldloca.s   V_1
    IL_0004:  /* FE15 | (1B)000003       */ initobj    !!T1/*1B000003*/
    IL_000a:  /* 07   |                  */ ldloc.1
    IL_000b:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0010:  /* 2D   | 14               */ brtrue.s   IL_0026

    IL_0012:  /* 71   | (1B)000003       */ ldobj      !!T1/*1B000003*/
    IL_0017:  /* 0B   |                  */ stloc.1
    IL_0018:  /* 12   | 01               */ ldloca.s   V_1
    IL_001a:  /* 07   |                  */ ldloc.1
    IL_001b:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0020:  /* 2D   | 04               */ brtrue.s   IL_0026

    IL_0022:  /* 26   |                  */ pop
    IL_0023:  /* 16   |                  */ ldc.i4.0
    IL_0024:  /* 2B   | 0B               */ br.s       IL_0031

    IL_0026:  /* FE16 | (1B)000003       */ constrained. !!T1/*1B000003*/
    IL_002c:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0031:  /* 0A   |                  */ stloc.0
    IL_0032:  /* 28   | (06)000016       */ call       uint32 HashCode/*02000009*/::MixEmptyState() /* 06000016 */
    IL_0037:  /* 1A   |                  */ ldc.i4.4
    IL_0038:  /* 58   |                  */ add
    IL_0039:  /* 06   |                  */ ldloc.0
    IL_003a:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_003f:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_0044:  /* 2A   |                  */ ret
  } // end of method HashCode::Combine

  .method /*0600000B*/ public hidebysig static 
          int32  Combine<T1,T2>(!!T1 value1,
                                !!T2 value2) cil managed
  // SIG: 10 02 02 08 1E 00 1E 01
  {
    .param type T1 /*2A000002*/ 
      .custom /*0C000019:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A000003*/ 
      .custom /*0C00001C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b0
    // Code size       125 (0x7d)
    .maxstack  2
    .locals /*11000003*/ init (uint32 V_0,
             uint32 V_1,
             !!T1 V_2,
             !!T2 V_3)
    IL_0000:  /* 0F   | 00               */ ldarga.s   value1
    IL_0002:  /* 12   | 02               */ ldloca.s   V_2
    IL_0004:  /* FE15 | (1B)000003       */ initobj    !!T1/*1B000003*/
    IL_000a:  /* 08   |                  */ ldloc.2
    IL_000b:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0010:  /* 2D   | 14               */ brtrue.s   IL_0026

    IL_0012:  /* 71   | (1B)000003       */ ldobj      !!T1/*1B000003*/
    IL_0017:  /* 0C   |                  */ stloc.2
    IL_0018:  /* 12   | 02               */ ldloca.s   V_2
    IL_001a:  /* 08   |                  */ ldloc.2
    IL_001b:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0020:  /* 2D   | 04               */ brtrue.s   IL_0026

    IL_0022:  /* 26   |                  */ pop
    IL_0023:  /* 16   |                  */ ldc.i4.0
    IL_0024:  /* 2B   | 0B               */ br.s       IL_0031

    IL_0026:  /* FE16 | (1B)000003       */ constrained. !!T1/*1B000003*/
    IL_002c:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0031:  /* 0A   |                  */ stloc.0
    IL_0032:  /* 0F   | 01               */ ldarga.s   value2
    IL_0034:  /* 12   | 03               */ ldloca.s   V_3
    IL_0036:  /* FE15 | (1B)000004       */ initobj    !!T2/*1B000004*/
    IL_003c:  /* 09   |                  */ ldloc.3
    IL_003d:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0042:  /* 2D   | 14               */ brtrue.s   IL_0058

    IL_0044:  /* 71   | (1B)000004       */ ldobj      !!T2/*1B000004*/
    IL_0049:  /* 0D   |                  */ stloc.3
    IL_004a:  /* 12   | 03               */ ldloca.s   V_3
    IL_004c:  /* 09   |                  */ ldloc.3
    IL_004d:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0052:  /* 2D   | 04               */ brtrue.s   IL_0058

    IL_0054:  /* 26   |                  */ pop
    IL_0055:  /* 16   |                  */ ldc.i4.0
    IL_0056:  /* 2B   | 0B               */ br.s       IL_0063

    IL_0058:  /* FE16 | (1B)000004       */ constrained. !!T2/*1B000004*/
    IL_005e:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0063:  /* 0B   |                  */ stloc.1
    IL_0064:  /* 28   | (06)000016       */ call       uint32 HashCode/*02000009*/::MixEmptyState() /* 06000016 */
    IL_0069:  /* 1E   |                  */ ldc.i4.8
    IL_006a:  /* 58   |                  */ add
    IL_006b:  /* 06   |                  */ ldloc.0
    IL_006c:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_0071:  /* 07   |                  */ ldloc.1
    IL_0072:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_0077:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_007c:  /* 2A   |                  */ ret
  } // end of method HashCode::Combine

  .method /*0600000C*/ public hidebysig static 
          int32  Combine<T1,T2,T3>(!!T1 value1,
                                   !!T2 value2,
                                   !!T3 value3) cil managed
  // SIG: 10 03 03 08 1E 00 1E 01 1E 02
  {
    .param type T1 /*2A000004*/ 
      .custom /*0C000020:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A000005*/ 
      .custom /*0C000025:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T3 /*2A000006*/ 
      .custom /*0C00002A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223c
    // Code size       188 (0xbc)
    .maxstack  2
    .locals /*11000004*/ init (uint32 V_0,
             uint32 V_1,
             uint32 V_2,
             !!T1 V_3,
             !!T2 V_4,
             !!T3 V_5)
    IL_0000:  /* 0F   | 00               */ ldarga.s   value1
    IL_0002:  /* 12   | 03               */ ldloca.s   V_3
    IL_0004:  /* FE15 | (1B)000003       */ initobj    !!T1/*1B000003*/
    IL_000a:  /* 09   |                  */ ldloc.3
    IL_000b:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0010:  /* 2D   | 14               */ brtrue.s   IL_0026

    IL_0012:  /* 71   | (1B)000003       */ ldobj      !!T1/*1B000003*/
    IL_0017:  /* 0D   |                  */ stloc.3
    IL_0018:  /* 12   | 03               */ ldloca.s   V_3
    IL_001a:  /* 09   |                  */ ldloc.3
    IL_001b:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0020:  /* 2D   | 04               */ brtrue.s   IL_0026

    IL_0022:  /* 26   |                  */ pop
    IL_0023:  /* 16   |                  */ ldc.i4.0
    IL_0024:  /* 2B   | 0B               */ br.s       IL_0031

    IL_0026:  /* FE16 | (1B)000003       */ constrained. !!T1/*1B000003*/
    IL_002c:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0031:  /* 0A   |                  */ stloc.0
    IL_0032:  /* 0F   | 01               */ ldarga.s   value2
    IL_0034:  /* 12   | 04               */ ldloca.s   V_4
    IL_0036:  /* FE15 | (1B)000004       */ initobj    !!T2/*1B000004*/
    IL_003c:  /* 11   | 04               */ ldloc.s    V_4
    IL_003e:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0043:  /* 2D   | 16               */ brtrue.s   IL_005b

    IL_0045:  /* 71   | (1B)000004       */ ldobj      !!T2/*1B000004*/
    IL_004a:  /* 13   | 04               */ stloc.s    V_4
    IL_004c:  /* 12   | 04               */ ldloca.s   V_4
    IL_004e:  /* 11   | 04               */ ldloc.s    V_4
    IL_0050:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0055:  /* 2D   | 04               */ brtrue.s   IL_005b

    IL_0057:  /* 26   |                  */ pop
    IL_0058:  /* 16   |                  */ ldc.i4.0
    IL_0059:  /* 2B   | 0B               */ br.s       IL_0066

    IL_005b:  /* FE16 | (1B)000004       */ constrained. !!T2/*1B000004*/
    IL_0061:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0066:  /* 0B   |                  */ stloc.1
    IL_0067:  /* 0F   | 02               */ ldarga.s   value3
    IL_0069:  /* 12   | 05               */ ldloca.s   V_5
    IL_006b:  /* FE15 | (1B)000005       */ initobj    !!T3/*1B000005*/
    IL_0071:  /* 11   | 05               */ ldloc.s    V_5
    IL_0073:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_0078:  /* 2D   | 16               */ brtrue.s   IL_0090

    IL_007a:  /* 71   | (1B)000005       */ ldobj      !!T3/*1B000005*/
    IL_007f:  /* 13   | 05               */ stloc.s    V_5
    IL_0081:  /* 12   | 05               */ ldloca.s   V_5
    IL_0083:  /* 11   | 05               */ ldloc.s    V_5
    IL_0085:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_008a:  /* 2D   | 04               */ brtrue.s   IL_0090

    IL_008c:  /* 26   |                  */ pop
    IL_008d:  /* 16   |                  */ ldc.i4.0
    IL_008e:  /* 2B   | 0B               */ br.s       IL_009b

    IL_0090:  /* FE16 | (1B)000005       */ constrained. !!T3/*1B000005*/
    IL_0096:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_009b:  /* 0C   |                  */ stloc.2
    IL_009c:  /* 28   | (06)000016       */ call       uint32 HashCode/*02000009*/::MixEmptyState() /* 06000016 */
    IL_00a1:  /* 1F   | 0C               */ ldc.i4.s   12
    IL_00a3:  /* 58   |                  */ add
    IL_00a4:  /* 06   |                  */ ldloc.0
    IL_00a5:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_00aa:  /* 07   |                  */ ldloc.1
    IL_00ab:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_00b0:  /* 08   |                  */ ldloc.2
    IL_00b1:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_00b6:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_00bb:  /* 2A   |                  */ ret
  } // end of method HashCode::Combine

  .method /*0600000D*/ public hidebysig static 
          int32  Combine<T1,T2,T3,T4>(!!T1 value1,
                                      !!T2 value2,
                                      !!T3 value3,
                                      !!T4 value4) cil managed
  // SIG: 10 04 04 08 1E 00 1E 01 1E 02 1E 03
  {
    .param type T1 /*2A000007*/ 
      .custom /*0C00002F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A000008*/ 
      .custom /*0C000032:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T3 /*2A000009*/ 
      .custom /*0C000036:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T4 /*2A00000A*/ 
      .custom /*0C00003A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2304
    // Code size       287 (0x11f)
    .maxstack  4
    .locals /*11000005*/ init (uint32 V_0,
             uint32 V_1,
             uint32 V_2,
             uint32 V_3,
             uint32 V_4,
             uint32 V_5,
             uint32 V_6,
             uint32 V_7,
             !!T1 V_8,
             !!T2 V_9,
             !!T3 V_10,
             !!T4 V_11)
    IL_0000:  /* 0F   | 00               */ ldarga.s   value1
    IL_0002:  /* 12   | 08               */ ldloca.s   V_8
    IL_0004:  /* FE15 | (1B)000003       */ initobj    !!T1/*1B000003*/
    IL_000a:  /* 11   | 08               */ ldloc.s    V_8
    IL_000c:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0011:  /* 2D   | 16               */ brtrue.s   IL_0029

    IL_0013:  /* 71   | (1B)000003       */ ldobj      !!T1/*1B000003*/
    IL_0018:  /* 13   | 08               */ stloc.s    V_8
    IL_001a:  /* 12   | 08               */ ldloca.s   V_8
    IL_001c:  /* 11   | 08               */ ldloc.s    V_8
    IL_001e:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0023:  /* 2D   | 04               */ brtrue.s   IL_0029

    IL_0025:  /* 26   |                  */ pop
    IL_0026:  /* 16   |                  */ ldc.i4.0
    IL_0027:  /* 2B   | 0B               */ br.s       IL_0034

    IL_0029:  /* FE16 | (1B)000003       */ constrained. !!T1/*1B000003*/
    IL_002f:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0034:  /* 0A   |                  */ stloc.0
    IL_0035:  /* 0F   | 01               */ ldarga.s   value2
    IL_0037:  /* 12   | 09               */ ldloca.s   V_9
    IL_0039:  /* FE15 | (1B)000004       */ initobj    !!T2/*1B000004*/
    IL_003f:  /* 11   | 09               */ ldloc.s    V_9
    IL_0041:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0046:  /* 2D   | 16               */ brtrue.s   IL_005e

    IL_0048:  /* 71   | (1B)000004       */ ldobj      !!T2/*1B000004*/
    IL_004d:  /* 13   | 09               */ stloc.s    V_9
    IL_004f:  /* 12   | 09               */ ldloca.s   V_9
    IL_0051:  /* 11   | 09               */ ldloc.s    V_9
    IL_0053:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0058:  /* 2D   | 04               */ brtrue.s   IL_005e

    IL_005a:  /* 26   |                  */ pop
    IL_005b:  /* 16   |                  */ ldc.i4.0
    IL_005c:  /* 2B   | 0B               */ br.s       IL_0069

    IL_005e:  /* FE16 | (1B)000004       */ constrained. !!T2/*1B000004*/
    IL_0064:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0069:  /* 0B   |                  */ stloc.1
    IL_006a:  /* 0F   | 02               */ ldarga.s   value3
    IL_006c:  /* 12   | 0A               */ ldloca.s   V_10
    IL_006e:  /* FE15 | (1B)000005       */ initobj    !!T3/*1B000005*/
    IL_0074:  /* 11   | 0A               */ ldloc.s    V_10
    IL_0076:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_007b:  /* 2D   | 16               */ brtrue.s   IL_0093

    IL_007d:  /* 71   | (1B)000005       */ ldobj      !!T3/*1B000005*/
    IL_0082:  /* 13   | 0A               */ stloc.s    V_10
    IL_0084:  /* 12   | 0A               */ ldloca.s   V_10
    IL_0086:  /* 11   | 0A               */ ldloc.s    V_10
    IL_0088:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_008d:  /* 2D   | 04               */ brtrue.s   IL_0093

    IL_008f:  /* 26   |                  */ pop
    IL_0090:  /* 16   |                  */ ldc.i4.0
    IL_0091:  /* 2B   | 0B               */ br.s       IL_009e

    IL_0093:  /* FE16 | (1B)000005       */ constrained. !!T3/*1B000005*/
    IL_0099:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_009e:  /* 0C   |                  */ stloc.2
    IL_009f:  /* 0F   | 03               */ ldarga.s   value4
    IL_00a1:  /* 12   | 0B               */ ldloca.s   V_11
    IL_00a3:  /* FE15 | (1B)000006       */ initobj    !!T4/*1B000006*/
    IL_00a9:  /* 11   | 0B               */ ldloc.s    V_11
    IL_00ab:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00b0:  /* 2D   | 16               */ brtrue.s   IL_00c8

    IL_00b2:  /* 71   | (1B)000006       */ ldobj      !!T4/*1B000006*/
    IL_00b7:  /* 13   | 0B               */ stloc.s    V_11
    IL_00b9:  /* 12   | 0B               */ ldloca.s   V_11
    IL_00bb:  /* 11   | 0B               */ ldloc.s    V_11
    IL_00bd:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00c2:  /* 2D   | 04               */ brtrue.s   IL_00c8

    IL_00c4:  /* 26   |                  */ pop
    IL_00c5:  /* 16   |                  */ ldc.i4.0
    IL_00c6:  /* 2B   | 0B               */ br.s       IL_00d3

    IL_00c8:  /* FE16 | (1B)000006       */ constrained. !!T4/*1B000006*/
    IL_00ce:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_00d3:  /* 0D   |                  */ stloc.3
    IL_00d4:  /* 12   | 04               */ ldloca.s   V_4
    IL_00d6:  /* 12   | 05               */ ldloca.s   V_5
    IL_00d8:  /* 12   | 06               */ ldloca.s   V_6
    IL_00da:  /* 12   | 07               */ ldloca.s   V_7
    IL_00dc:  /* 28   | (06)000012       */ call       void HashCode/*02000009*/::Initialize(uint32&,
                                                                                             uint32&,
                                                                                             uint32&,
                                                                                             uint32&) /* 06000012 */
    IL_00e1:  /* 11   | 04               */ ldloc.s    V_4
    IL_00e3:  /* 06   |                  */ ldloc.0
    IL_00e4:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_00e9:  /* 13   | 04               */ stloc.s    V_4
    IL_00eb:  /* 11   | 05               */ ldloc.s    V_5
    IL_00ed:  /* 07   |                  */ ldloc.1
    IL_00ee:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_00f3:  /* 13   | 05               */ stloc.s    V_5
    IL_00f5:  /* 11   | 06               */ ldloc.s    V_6
    IL_00f7:  /* 08   |                  */ ldloc.2
    IL_00f8:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_00fd:  /* 13   | 06               */ stloc.s    V_6
    IL_00ff:  /* 11   | 07               */ ldloc.s    V_7
    IL_0101:  /* 09   |                  */ ldloc.3
    IL_0102:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0107:  /* 13   | 07               */ stloc.s    V_7
    IL_0109:  /* 11   | 04               */ ldloc.s    V_4
    IL_010b:  /* 11   | 05               */ ldloc.s    V_5
    IL_010d:  /* 11   | 06               */ ldloc.s    V_6
    IL_010f:  /* 11   | 07               */ ldloc.s    V_7
    IL_0111:  /* 28   | (06)000015       */ call       uint32 HashCode/*02000009*/::MixState(uint32,
                                                                                             uint32,
                                                                                             uint32,
                                                                                             uint32) /* 06000015 */
    IL_0116:  /* 1F   | 10               */ ldc.i4.s   16
    IL_0118:  /* 58   |                  */ add
    IL_0119:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_011e:  /* 2A   |                  */ ret
  } // end of method HashCode::Combine

  .method /*0600000E*/ public hidebysig static 
          int32  Combine<T1,T2,T3,T4,T5>(!!T1 value1,
                                         !!T2 value2,
                                         !!T3 value3,
                                         !!T4 value4,
                                         !!T5 value5) cil managed
  // SIG: 10 05 05 08 1E 00 1E 01 1E 02 1E 03 1E 04
  {
    .param type T1 /*2A00000B*/ 
      .custom /*0C00003F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A00000C*/ 
      .custom /*0C000044:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T3 /*2A00000D*/ 
      .custom /*0C000049:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T4 /*2A00000E*/ 
      .custom /*0C00004E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T5 /*2A00000F*/ 
      .custom /*0C000053:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2430
    // Code size       348 (0x15c)
    .maxstack  4
    .locals /*11000006*/ init (uint32 V_0,
             uint32 V_1,
             uint32 V_2,
             uint32 V_3,
             uint32 V_4,
             uint32 V_5,
             uint32 V_6,
             uint32 V_7,
             uint32 V_8,
             !!T1 V_9,
             !!T2 V_10,
             !!T3 V_11,
             !!T4 V_12,
             !!T5 V_13)
    IL_0000:  /* 0F   | 00               */ ldarga.s   value1
    IL_0002:  /* 12   | 09               */ ldloca.s   V_9
    IL_0004:  /* FE15 | (1B)000003       */ initobj    !!T1/*1B000003*/
    IL_000a:  /* 11   | 09               */ ldloc.s    V_9
    IL_000c:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0011:  /* 2D   | 16               */ brtrue.s   IL_0029

    IL_0013:  /* 71   | (1B)000003       */ ldobj      !!T1/*1B000003*/
    IL_0018:  /* 13   | 09               */ stloc.s    V_9
    IL_001a:  /* 12   | 09               */ ldloca.s   V_9
    IL_001c:  /* 11   | 09               */ ldloc.s    V_9
    IL_001e:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0023:  /* 2D   | 04               */ brtrue.s   IL_0029

    IL_0025:  /* 26   |                  */ pop
    IL_0026:  /* 16   |                  */ ldc.i4.0
    IL_0027:  /* 2B   | 0B               */ br.s       IL_0034

    IL_0029:  /* FE16 | (1B)000003       */ constrained. !!T1/*1B000003*/
    IL_002f:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0034:  /* 0A   |                  */ stloc.0
    IL_0035:  /* 0F   | 01               */ ldarga.s   value2
    IL_0037:  /* 12   | 0A               */ ldloca.s   V_10
    IL_0039:  /* FE15 | (1B)000004       */ initobj    !!T2/*1B000004*/
    IL_003f:  /* 11   | 0A               */ ldloc.s    V_10
    IL_0041:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0046:  /* 2D   | 16               */ brtrue.s   IL_005e

    IL_0048:  /* 71   | (1B)000004       */ ldobj      !!T2/*1B000004*/
    IL_004d:  /* 13   | 0A               */ stloc.s    V_10
    IL_004f:  /* 12   | 0A               */ ldloca.s   V_10
    IL_0051:  /* 11   | 0A               */ ldloc.s    V_10
    IL_0053:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0058:  /* 2D   | 04               */ brtrue.s   IL_005e

    IL_005a:  /* 26   |                  */ pop
    IL_005b:  /* 16   |                  */ ldc.i4.0
    IL_005c:  /* 2B   | 0B               */ br.s       IL_0069

    IL_005e:  /* FE16 | (1B)000004       */ constrained. !!T2/*1B000004*/
    IL_0064:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0069:  /* 0B   |                  */ stloc.1
    IL_006a:  /* 0F   | 02               */ ldarga.s   value3
    IL_006c:  /* 12   | 0B               */ ldloca.s   V_11
    IL_006e:  /* FE15 | (1B)000005       */ initobj    !!T3/*1B000005*/
    IL_0074:  /* 11   | 0B               */ ldloc.s    V_11
    IL_0076:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_007b:  /* 2D   | 16               */ brtrue.s   IL_0093

    IL_007d:  /* 71   | (1B)000005       */ ldobj      !!T3/*1B000005*/
    IL_0082:  /* 13   | 0B               */ stloc.s    V_11
    IL_0084:  /* 12   | 0B               */ ldloca.s   V_11
    IL_0086:  /* 11   | 0B               */ ldloc.s    V_11
    IL_0088:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_008d:  /* 2D   | 04               */ brtrue.s   IL_0093

    IL_008f:  /* 26   |                  */ pop
    IL_0090:  /* 16   |                  */ ldc.i4.0
    IL_0091:  /* 2B   | 0B               */ br.s       IL_009e

    IL_0093:  /* FE16 | (1B)000005       */ constrained. !!T3/*1B000005*/
    IL_0099:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_009e:  /* 0C   |                  */ stloc.2
    IL_009f:  /* 0F   | 03               */ ldarga.s   value4
    IL_00a1:  /* 12   | 0C               */ ldloca.s   V_12
    IL_00a3:  /* FE15 | (1B)000006       */ initobj    !!T4/*1B000006*/
    IL_00a9:  /* 11   | 0C               */ ldloc.s    V_12
    IL_00ab:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00b0:  /* 2D   | 16               */ brtrue.s   IL_00c8

    IL_00b2:  /* 71   | (1B)000006       */ ldobj      !!T4/*1B000006*/
    IL_00b7:  /* 13   | 0C               */ stloc.s    V_12
    IL_00b9:  /* 12   | 0C               */ ldloca.s   V_12
    IL_00bb:  /* 11   | 0C               */ ldloc.s    V_12
    IL_00bd:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00c2:  /* 2D   | 04               */ brtrue.s   IL_00c8

    IL_00c4:  /* 26   |                  */ pop
    IL_00c5:  /* 16   |                  */ ldc.i4.0
    IL_00c6:  /* 2B   | 0B               */ br.s       IL_00d3

    IL_00c8:  /* FE16 | (1B)000006       */ constrained. !!T4/*1B000006*/
    IL_00ce:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_00d3:  /* 0D   |                  */ stloc.3
    IL_00d4:  /* 0F   | 04               */ ldarga.s   value5
    IL_00d6:  /* 12   | 0D               */ ldloca.s   V_13
    IL_00d8:  /* FE15 | (1B)000007       */ initobj    !!T5/*1B000007*/
    IL_00de:  /* 11   | 0D               */ ldloc.s    V_13
    IL_00e0:  /* 8C   | (1B)000007       */ box        !!T5/*1B000007*/
    IL_00e5:  /* 2D   | 16               */ brtrue.s   IL_00fd

    IL_00e7:  /* 71   | (1B)000007       */ ldobj      !!T5/*1B000007*/
    IL_00ec:  /* 13   | 0D               */ stloc.s    V_13
    IL_00ee:  /* 12   | 0D               */ ldloca.s   V_13
    IL_00f0:  /* 11   | 0D               */ ldloc.s    V_13
    IL_00f2:  /* 8C   | (1B)000007       */ box        !!T5/*1B000007*/
    IL_00f7:  /* 2D   | 04               */ brtrue.s   IL_00fd

    IL_00f9:  /* 26   |                  */ pop
    IL_00fa:  /* 16   |                  */ ldc.i4.0
    IL_00fb:  /* 2B   | 0B               */ br.s       IL_0108

    IL_00fd:  /* FE16 | (1B)000007       */ constrained. !!T5/*1B000007*/
    IL_0103:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0108:  /* 13   | 04               */ stloc.s    V_4
    IL_010a:  /* 12   | 05               */ ldloca.s   V_5
    IL_010c:  /* 12   | 06               */ ldloca.s   V_6
    IL_010e:  /* 12   | 07               */ ldloca.s   V_7
    IL_0110:  /* 12   | 08               */ ldloca.s   V_8
    IL_0112:  /* 28   | (06)000012       */ call       void HashCode/*02000009*/::Initialize(uint32&,
                                                                                             uint32&,
                                                                                             uint32&,
                                                                                             uint32&) /* 06000012 */
    IL_0117:  /* 11   | 05               */ ldloc.s    V_5
    IL_0119:  /* 06   |                  */ ldloc.0
    IL_011a:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_011f:  /* 13   | 05               */ stloc.s    V_5
    IL_0121:  /* 11   | 06               */ ldloc.s    V_6
    IL_0123:  /* 07   |                  */ ldloc.1
    IL_0124:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0129:  /* 13   | 06               */ stloc.s    V_6
    IL_012b:  /* 11   | 07               */ ldloc.s    V_7
    IL_012d:  /* 08   |                  */ ldloc.2
    IL_012e:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0133:  /* 13   | 07               */ stloc.s    V_7
    IL_0135:  /* 11   | 08               */ ldloc.s    V_8
    IL_0137:  /* 09   |                  */ ldloc.3
    IL_0138:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_013d:  /* 13   | 08               */ stloc.s    V_8
    IL_013f:  /* 11   | 05               */ ldloc.s    V_5
    IL_0141:  /* 11   | 06               */ ldloc.s    V_6
    IL_0143:  /* 11   | 07               */ ldloc.s    V_7
    IL_0145:  /* 11   | 08               */ ldloc.s    V_8
    IL_0147:  /* 28   | (06)000015       */ call       uint32 HashCode/*02000009*/::MixState(uint32,
                                                                                             uint32,
                                                                                             uint32,
                                                                                             uint32) /* 06000015 */
    IL_014c:  /* 1F   | 14               */ ldc.i4.s   20
    IL_014e:  /* 58   |                  */ add
    IL_014f:  /* 11   | 04               */ ldloc.s    V_4
    IL_0151:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_0156:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_015b:  /* 2A   |                  */ ret
  } // end of method HashCode::Combine

  .method /*0600000F*/ public hidebysig static 
          int32  Combine<T1,T2,T3,T4,T5,T6>(!!T1 value1,
                                            !!T2 value2,
                                            !!T3 value3,
                                            !!T4 value4,
                                            !!T5 value5,
                                            !!T6 value6) cil managed
  // SIG: 10 06 06 08 1E 00 1E 01 1E 02 1E 03 1E 04 1E 05
  {
    .param type T1 /*2A000010*/ 
      .custom /*0C000056:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A000011*/ 
      .custom /*0C00005B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T3 /*2A000012*/ 
      .custom /*0C000060:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T4 /*2A000013*/ 
      .custom /*0C000066:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T5 /*2A000014*/ 
      .custom /*0C000068:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T6 /*2A000015*/ 
      .custom /*0C00006A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2598
    // Code size       409 (0x199)
    .maxstack  4
    .locals /*11000007*/ init (uint32 V_0,
             uint32 V_1,
             uint32 V_2,
             uint32 V_3,
             uint32 V_4,
             uint32 V_5,
             uint32 V_6,
             uint32 V_7,
             uint32 V_8,
             uint32 V_9,
             !!T1 V_10,
             !!T2 V_11,
             !!T3 V_12,
             !!T4 V_13,
             !!T5 V_14,
             !!T6 V_15)
    IL_0000:  /* 0F   | 00               */ ldarga.s   value1
    IL_0002:  /* 12   | 0A               */ ldloca.s   V_10
    IL_0004:  /* FE15 | (1B)000003       */ initobj    !!T1/*1B000003*/
    IL_000a:  /* 11   | 0A               */ ldloc.s    V_10
    IL_000c:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0011:  /* 2D   | 16               */ brtrue.s   IL_0029

    IL_0013:  /* 71   | (1B)000003       */ ldobj      !!T1/*1B000003*/
    IL_0018:  /* 13   | 0A               */ stloc.s    V_10
    IL_001a:  /* 12   | 0A               */ ldloca.s   V_10
    IL_001c:  /* 11   | 0A               */ ldloc.s    V_10
    IL_001e:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0023:  /* 2D   | 04               */ brtrue.s   IL_0029

    IL_0025:  /* 26   |                  */ pop
    IL_0026:  /* 16   |                  */ ldc.i4.0
    IL_0027:  /* 2B   | 0B               */ br.s       IL_0034

    IL_0029:  /* FE16 | (1B)000003       */ constrained. !!T1/*1B000003*/
    IL_002f:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0034:  /* 0A   |                  */ stloc.0
    IL_0035:  /* 0F   | 01               */ ldarga.s   value2
    IL_0037:  /* 12   | 0B               */ ldloca.s   V_11
    IL_0039:  /* FE15 | (1B)000004       */ initobj    !!T2/*1B000004*/
    IL_003f:  /* 11   | 0B               */ ldloc.s    V_11
    IL_0041:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0046:  /* 2D   | 16               */ brtrue.s   IL_005e

    IL_0048:  /* 71   | (1B)000004       */ ldobj      !!T2/*1B000004*/
    IL_004d:  /* 13   | 0B               */ stloc.s    V_11
    IL_004f:  /* 12   | 0B               */ ldloca.s   V_11
    IL_0051:  /* 11   | 0B               */ ldloc.s    V_11
    IL_0053:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0058:  /* 2D   | 04               */ brtrue.s   IL_005e

    IL_005a:  /* 26   |                  */ pop
    IL_005b:  /* 16   |                  */ ldc.i4.0
    IL_005c:  /* 2B   | 0B               */ br.s       IL_0069

    IL_005e:  /* FE16 | (1B)000004       */ constrained. !!T2/*1B000004*/
    IL_0064:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0069:  /* 0B   |                  */ stloc.1
    IL_006a:  /* 0F   | 02               */ ldarga.s   value3
    IL_006c:  /* 12   | 0C               */ ldloca.s   V_12
    IL_006e:  /* FE15 | (1B)000005       */ initobj    !!T3/*1B000005*/
    IL_0074:  /* 11   | 0C               */ ldloc.s    V_12
    IL_0076:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_007b:  /* 2D   | 16               */ brtrue.s   IL_0093

    IL_007d:  /* 71   | (1B)000005       */ ldobj      !!T3/*1B000005*/
    IL_0082:  /* 13   | 0C               */ stloc.s    V_12
    IL_0084:  /* 12   | 0C               */ ldloca.s   V_12
    IL_0086:  /* 11   | 0C               */ ldloc.s    V_12
    IL_0088:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_008d:  /* 2D   | 04               */ brtrue.s   IL_0093

    IL_008f:  /* 26   |                  */ pop
    IL_0090:  /* 16   |                  */ ldc.i4.0
    IL_0091:  /* 2B   | 0B               */ br.s       IL_009e

    IL_0093:  /* FE16 | (1B)000005       */ constrained. !!T3/*1B000005*/
    IL_0099:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_009e:  /* 0C   |                  */ stloc.2
    IL_009f:  /* 0F   | 03               */ ldarga.s   value4
    IL_00a1:  /* 12   | 0D               */ ldloca.s   V_13
    IL_00a3:  /* FE15 | (1B)000006       */ initobj    !!T4/*1B000006*/
    IL_00a9:  /* 11   | 0D               */ ldloc.s    V_13
    IL_00ab:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00b0:  /* 2D   | 16               */ brtrue.s   IL_00c8

    IL_00b2:  /* 71   | (1B)000006       */ ldobj      !!T4/*1B000006*/
    IL_00b7:  /* 13   | 0D               */ stloc.s    V_13
    IL_00b9:  /* 12   | 0D               */ ldloca.s   V_13
    IL_00bb:  /* 11   | 0D               */ ldloc.s    V_13
    IL_00bd:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00c2:  /* 2D   | 04               */ brtrue.s   IL_00c8

    IL_00c4:  /* 26   |                  */ pop
    IL_00c5:  /* 16   |                  */ ldc.i4.0
    IL_00c6:  /* 2B   | 0B               */ br.s       IL_00d3

    IL_00c8:  /* FE16 | (1B)000006       */ constrained. !!T4/*1B000006*/
    IL_00ce:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_00d3:  /* 0D   |                  */ stloc.3
    IL_00d4:  /* 0F   | 04               */ ldarga.s   value5
    IL_00d6:  /* 12   | 0E               */ ldloca.s   V_14
    IL_00d8:  /* FE15 | (1B)000007       */ initobj    !!T5/*1B000007*/
    IL_00de:  /* 11   | 0E               */ ldloc.s    V_14
    IL_00e0:  /* 8C   | (1B)000007       */ box        !!T5/*1B000007*/
    IL_00e5:  /* 2D   | 16               */ brtrue.s   IL_00fd

    IL_00e7:  /* 71   | (1B)000007       */ ldobj      !!T5/*1B000007*/
    IL_00ec:  /* 13   | 0E               */ stloc.s    V_14
    IL_00ee:  /* 12   | 0E               */ ldloca.s   V_14
    IL_00f0:  /* 11   | 0E               */ ldloc.s    V_14
    IL_00f2:  /* 8C   | (1B)000007       */ box        !!T5/*1B000007*/
    IL_00f7:  /* 2D   | 04               */ brtrue.s   IL_00fd

    IL_00f9:  /* 26   |                  */ pop
    IL_00fa:  /* 16   |                  */ ldc.i4.0
    IL_00fb:  /* 2B   | 0B               */ br.s       IL_0108

    IL_00fd:  /* FE16 | (1B)000007       */ constrained. !!T5/*1B000007*/
    IL_0103:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0108:  /* 13   | 04               */ stloc.s    V_4
    IL_010a:  /* 0F   | 05               */ ldarga.s   value6
    IL_010c:  /* 12   | 0F               */ ldloca.s   V_15
    IL_010e:  /* FE15 | (1B)000008       */ initobj    !!T6/*1B000008*/
    IL_0114:  /* 11   | 0F               */ ldloc.s    V_15
    IL_0116:  /* 8C   | (1B)000008       */ box        !!T6/*1B000008*/
    IL_011b:  /* 2D   | 16               */ brtrue.s   IL_0133

    IL_011d:  /* 71   | (1B)000008       */ ldobj      !!T6/*1B000008*/
    IL_0122:  /* 13   | 0F               */ stloc.s    V_15
    IL_0124:  /* 12   | 0F               */ ldloca.s   V_15
    IL_0126:  /* 11   | 0F               */ ldloc.s    V_15
    IL_0128:  /* 8C   | (1B)000008       */ box        !!T6/*1B000008*/
    IL_012d:  /* 2D   | 04               */ brtrue.s   IL_0133

    IL_012f:  /* 26   |                  */ pop
    IL_0130:  /* 16   |                  */ ldc.i4.0
    IL_0131:  /* 2B   | 0B               */ br.s       IL_013e

    IL_0133:  /* FE16 | (1B)000008       */ constrained. !!T6/*1B000008*/
    IL_0139:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_013e:  /* 13   | 05               */ stloc.s    V_5
    IL_0140:  /* 12   | 06               */ ldloca.s   V_6
    IL_0142:  /* 12   | 07               */ ldloca.s   V_7
    IL_0144:  /* 12   | 08               */ ldloca.s   V_8
    IL_0146:  /* 12   | 09               */ ldloca.s   V_9
    IL_0148:  /* 28   | (06)000012       */ call       void HashCode/*02000009*/::Initialize(uint32&,
                                                                                             uint32&,
                                                                                             uint32&,
                                                                                             uint32&) /* 06000012 */
    IL_014d:  /* 11   | 06               */ ldloc.s    V_6
    IL_014f:  /* 06   |                  */ ldloc.0
    IL_0150:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0155:  /* 13   | 06               */ stloc.s    V_6
    IL_0157:  /* 11   | 07               */ ldloc.s    V_7
    IL_0159:  /* 07   |                  */ ldloc.1
    IL_015a:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_015f:  /* 13   | 07               */ stloc.s    V_7
    IL_0161:  /* 11   | 08               */ ldloc.s    V_8
    IL_0163:  /* 08   |                  */ ldloc.2
    IL_0164:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0169:  /* 13   | 08               */ stloc.s    V_8
    IL_016b:  /* 11   | 09               */ ldloc.s    V_9
    IL_016d:  /* 09   |                  */ ldloc.3
    IL_016e:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0173:  /* 13   | 09               */ stloc.s    V_9
    IL_0175:  /* 11   | 06               */ ldloc.s    V_6
    IL_0177:  /* 11   | 07               */ ldloc.s    V_7
    IL_0179:  /* 11   | 08               */ ldloc.s    V_8
    IL_017b:  /* 11   | 09               */ ldloc.s    V_9
    IL_017d:  /* 28   | (06)000015       */ call       uint32 HashCode/*02000009*/::MixState(uint32,
                                                                                             uint32,
                                                                                             uint32,
                                                                                             uint32) /* 06000015 */
    IL_0182:  /* 1F   | 18               */ ldc.i4.s   24
    IL_0184:  /* 58   |                  */ add
    IL_0185:  /* 11   | 04               */ ldloc.s    V_4
    IL_0187:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_018c:  /* 11   | 05               */ ldloc.s    V_5
    IL_018e:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_0193:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_0198:  /* 2A   |                  */ ret
  } // end of method HashCode::Combine

  .method /*06000010*/ public hidebysig static 
          int32  Combine<T1,T2,T3,T4,T5,T6,T7>(!!T1 value1,
                                               !!T2 value2,
                                               !!T3 value3,
                                               !!T4 value4,
                                               !!T5 value5,
                                               !!T6 value6,
                                               !!T7 value7) cil managed
  // SIG: 10 07 07 08 1E 00 1E 01 1E 02 1E 03 1E 04 1E 05 1E 06
  {
    .param type T1 /*2A000016*/ 
      .custom /*0C00006D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A000017*/ 
      .custom /*0C000070:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T3 /*2A000018*/ 
      .custom /*0C000073:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T4 /*2A000019*/ 
      .custom /*0C000076:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T5 /*2A00001A*/ 
      .custom /*0C000079:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T6 /*2A00001B*/ 
      .custom /*0C00007B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T7 /*2A00001C*/ 
      .custom /*0C000080:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2740
    // Code size       470 (0x1d6)
    .maxstack  4
    .locals /*11000008*/ init (uint32 V_0,
             uint32 V_1,
             uint32 V_2,
             uint32 V_3,
             uint32 V_4,
             uint32 V_5,
             uint32 V_6,
             uint32 V_7,
             uint32 V_8,
             uint32 V_9,
             uint32 V_10,
             !!T1 V_11,
             !!T2 V_12,
             !!T3 V_13,
             !!T4 V_14,
             !!T5 V_15,
             !!T6 V_16,
             !!T7 V_17)
    IL_0000:  /* 0F   | 00               */ ldarga.s   value1
    IL_0002:  /* 12   | 0B               */ ldloca.s   V_11
    IL_0004:  /* FE15 | (1B)000003       */ initobj    !!T1/*1B000003*/
    IL_000a:  /* 11   | 0B               */ ldloc.s    V_11
    IL_000c:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0011:  /* 2D   | 16               */ brtrue.s   IL_0029

    IL_0013:  /* 71   | (1B)000003       */ ldobj      !!T1/*1B000003*/
    IL_0018:  /* 13   | 0B               */ stloc.s    V_11
    IL_001a:  /* 12   | 0B               */ ldloca.s   V_11
    IL_001c:  /* 11   | 0B               */ ldloc.s    V_11
    IL_001e:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0023:  /* 2D   | 04               */ brtrue.s   IL_0029

    IL_0025:  /* 26   |                  */ pop
    IL_0026:  /* 16   |                  */ ldc.i4.0
    IL_0027:  /* 2B   | 0B               */ br.s       IL_0034

    IL_0029:  /* FE16 | (1B)000003       */ constrained. !!T1/*1B000003*/
    IL_002f:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0034:  /* 0A   |                  */ stloc.0
    IL_0035:  /* 0F   | 01               */ ldarga.s   value2
    IL_0037:  /* 12   | 0C               */ ldloca.s   V_12
    IL_0039:  /* FE15 | (1B)000004       */ initobj    !!T2/*1B000004*/
    IL_003f:  /* 11   | 0C               */ ldloc.s    V_12
    IL_0041:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0046:  /* 2D   | 16               */ brtrue.s   IL_005e

    IL_0048:  /* 71   | (1B)000004       */ ldobj      !!T2/*1B000004*/
    IL_004d:  /* 13   | 0C               */ stloc.s    V_12
    IL_004f:  /* 12   | 0C               */ ldloca.s   V_12
    IL_0051:  /* 11   | 0C               */ ldloc.s    V_12
    IL_0053:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0058:  /* 2D   | 04               */ brtrue.s   IL_005e

    IL_005a:  /* 26   |                  */ pop
    IL_005b:  /* 16   |                  */ ldc.i4.0
    IL_005c:  /* 2B   | 0B               */ br.s       IL_0069

    IL_005e:  /* FE16 | (1B)000004       */ constrained. !!T2/*1B000004*/
    IL_0064:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0069:  /* 0B   |                  */ stloc.1
    IL_006a:  /* 0F   | 02               */ ldarga.s   value3
    IL_006c:  /* 12   | 0D               */ ldloca.s   V_13
    IL_006e:  /* FE15 | (1B)000005       */ initobj    !!T3/*1B000005*/
    IL_0074:  /* 11   | 0D               */ ldloc.s    V_13
    IL_0076:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_007b:  /* 2D   | 16               */ brtrue.s   IL_0093

    IL_007d:  /* 71   | (1B)000005       */ ldobj      !!T3/*1B000005*/
    IL_0082:  /* 13   | 0D               */ stloc.s    V_13
    IL_0084:  /* 12   | 0D               */ ldloca.s   V_13
    IL_0086:  /* 11   | 0D               */ ldloc.s    V_13
    IL_0088:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_008d:  /* 2D   | 04               */ brtrue.s   IL_0093

    IL_008f:  /* 26   |                  */ pop
    IL_0090:  /* 16   |                  */ ldc.i4.0
    IL_0091:  /* 2B   | 0B               */ br.s       IL_009e

    IL_0093:  /* FE16 | (1B)000005       */ constrained. !!T3/*1B000005*/
    IL_0099:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_009e:  /* 0C   |                  */ stloc.2
    IL_009f:  /* 0F   | 03               */ ldarga.s   value4
    IL_00a1:  /* 12   | 0E               */ ldloca.s   V_14
    IL_00a3:  /* FE15 | (1B)000006       */ initobj    !!T4/*1B000006*/
    IL_00a9:  /* 11   | 0E               */ ldloc.s    V_14
    IL_00ab:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00b0:  /* 2D   | 16               */ brtrue.s   IL_00c8

    IL_00b2:  /* 71   | (1B)000006       */ ldobj      !!T4/*1B000006*/
    IL_00b7:  /* 13   | 0E               */ stloc.s    V_14
    IL_00b9:  /* 12   | 0E               */ ldloca.s   V_14
    IL_00bb:  /* 11   | 0E               */ ldloc.s    V_14
    IL_00bd:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00c2:  /* 2D   | 04               */ brtrue.s   IL_00c8

    IL_00c4:  /* 26   |                  */ pop
    IL_00c5:  /* 16   |                  */ ldc.i4.0
    IL_00c6:  /* 2B   | 0B               */ br.s       IL_00d3

    IL_00c8:  /* FE16 | (1B)000006       */ constrained. !!T4/*1B000006*/
    IL_00ce:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_00d3:  /* 0D   |                  */ stloc.3
    IL_00d4:  /* 0F   | 04               */ ldarga.s   value5
    IL_00d6:  /* 12   | 0F               */ ldloca.s   V_15
    IL_00d8:  /* FE15 | (1B)000007       */ initobj    !!T5/*1B000007*/
    IL_00de:  /* 11   | 0F               */ ldloc.s    V_15
    IL_00e0:  /* 8C   | (1B)000007       */ box        !!T5/*1B000007*/
    IL_00e5:  /* 2D   | 16               */ brtrue.s   IL_00fd

    IL_00e7:  /* 71   | (1B)000007       */ ldobj      !!T5/*1B000007*/
    IL_00ec:  /* 13   | 0F               */ stloc.s    V_15
    IL_00ee:  /* 12   | 0F               */ ldloca.s   V_15
    IL_00f0:  /* 11   | 0F               */ ldloc.s    V_15
    IL_00f2:  /* 8C   | (1B)000007       */ box        !!T5/*1B000007*/
    IL_00f7:  /* 2D   | 04               */ brtrue.s   IL_00fd

    IL_00f9:  /* 26   |                  */ pop
    IL_00fa:  /* 16   |                  */ ldc.i4.0
    IL_00fb:  /* 2B   | 0B               */ br.s       IL_0108

    IL_00fd:  /* FE16 | (1B)000007       */ constrained. !!T5/*1B000007*/
    IL_0103:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0108:  /* 13   | 04               */ stloc.s    V_4
    IL_010a:  /* 0F   | 05               */ ldarga.s   value6
    IL_010c:  /* 12   | 10               */ ldloca.s   V_16
    IL_010e:  /* FE15 | (1B)000008       */ initobj    !!T6/*1B000008*/
    IL_0114:  /* 11   | 10               */ ldloc.s    V_16
    IL_0116:  /* 8C   | (1B)000008       */ box        !!T6/*1B000008*/
    IL_011b:  /* 2D   | 16               */ brtrue.s   IL_0133

    IL_011d:  /* 71   | (1B)000008       */ ldobj      !!T6/*1B000008*/
    IL_0122:  /* 13   | 10               */ stloc.s    V_16
    IL_0124:  /* 12   | 10               */ ldloca.s   V_16
    IL_0126:  /* 11   | 10               */ ldloc.s    V_16
    IL_0128:  /* 8C   | (1B)000008       */ box        !!T6/*1B000008*/
    IL_012d:  /* 2D   | 04               */ brtrue.s   IL_0133

    IL_012f:  /* 26   |                  */ pop
    IL_0130:  /* 16   |                  */ ldc.i4.0
    IL_0131:  /* 2B   | 0B               */ br.s       IL_013e

    IL_0133:  /* FE16 | (1B)000008       */ constrained. !!T6/*1B000008*/
    IL_0139:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_013e:  /* 13   | 05               */ stloc.s    V_5
    IL_0140:  /* 0F   | 06               */ ldarga.s   value7
    IL_0142:  /* 12   | 11               */ ldloca.s   V_17
    IL_0144:  /* FE15 | (1B)000009       */ initobj    !!T7/*1B000009*/
    IL_014a:  /* 11   | 11               */ ldloc.s    V_17
    IL_014c:  /* 8C   | (1B)000009       */ box        !!T7/*1B000009*/
    IL_0151:  /* 2D   | 16               */ brtrue.s   IL_0169

    IL_0153:  /* 71   | (1B)000009       */ ldobj      !!T7/*1B000009*/
    IL_0158:  /* 13   | 11               */ stloc.s    V_17
    IL_015a:  /* 12   | 11               */ ldloca.s   V_17
    IL_015c:  /* 11   | 11               */ ldloc.s    V_17
    IL_015e:  /* 8C   | (1B)000009       */ box        !!T7/*1B000009*/
    IL_0163:  /* 2D   | 04               */ brtrue.s   IL_0169

    IL_0165:  /* 26   |                  */ pop
    IL_0166:  /* 16   |                  */ ldc.i4.0
    IL_0167:  /* 2B   | 0B               */ br.s       IL_0174

    IL_0169:  /* FE16 | (1B)000009       */ constrained. !!T7/*1B000009*/
    IL_016f:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0174:  /* 13   | 06               */ stloc.s    V_6
    IL_0176:  /* 12   | 07               */ ldloca.s   V_7
    IL_0178:  /* 12   | 08               */ ldloca.s   V_8
    IL_017a:  /* 12   | 09               */ ldloca.s   V_9
    IL_017c:  /* 12   | 0A               */ ldloca.s   V_10
    IL_017e:  /* 28   | (06)000012       */ call       void HashCode/*02000009*/::Initialize(uint32&,
                                                                                             uint32&,
                                                                                             uint32&,
                                                                                             uint32&) /* 06000012 */
    IL_0183:  /* 11   | 07               */ ldloc.s    V_7
    IL_0185:  /* 06   |                  */ ldloc.0
    IL_0186:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_018b:  /* 13   | 07               */ stloc.s    V_7
    IL_018d:  /* 11   | 08               */ ldloc.s    V_8
    IL_018f:  /* 07   |                  */ ldloc.1
    IL_0190:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0195:  /* 13   | 08               */ stloc.s    V_8
    IL_0197:  /* 11   | 09               */ ldloc.s    V_9
    IL_0199:  /* 08   |                  */ ldloc.2
    IL_019a:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_019f:  /* 13   | 09               */ stloc.s    V_9
    IL_01a1:  /* 11   | 0A               */ ldloc.s    V_10
    IL_01a3:  /* 09   |                  */ ldloc.3
    IL_01a4:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_01a9:  /* 13   | 0A               */ stloc.s    V_10
    IL_01ab:  /* 11   | 07               */ ldloc.s    V_7
    IL_01ad:  /* 11   | 08               */ ldloc.s    V_8
    IL_01af:  /* 11   | 09               */ ldloc.s    V_9
    IL_01b1:  /* 11   | 0A               */ ldloc.s    V_10
    IL_01b3:  /* 28   | (06)000015       */ call       uint32 HashCode/*02000009*/::MixState(uint32,
                                                                                             uint32,
                                                                                             uint32,
                                                                                             uint32) /* 06000015 */
    IL_01b8:  /* 1F   | 1C               */ ldc.i4.s   28
    IL_01ba:  /* 58   |                  */ add
    IL_01bb:  /* 11   | 04               */ ldloc.s    V_4
    IL_01bd:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_01c2:  /* 11   | 05               */ ldloc.s    V_5
    IL_01c4:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_01c9:  /* 11   | 06               */ ldloc.s    V_6
    IL_01cb:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_01d0:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_01d5:  /* 2A   |                  */ ret
  } // end of method HashCode::Combine

  .method /*06000011*/ public hidebysig static 
          int32  Combine<T1,T2,T3,T4,T5,T6,T7,T8>(!!T1 value1,
                                                  !!T2 value2,
                                                  !!T3 value3,
                                                  !!T4 value4,
                                                  !!T5 value5,
                                                  !!T6 value6,
                                                  !!T7 value7,
                                                  !!T8 value8) cil managed
  // SIG: 10 08 08 08 1E 00 1E 01 1E 02 1E 03 1E 04 1E 05 1E 06 1E 07
  {
    .param type T1 /*2A00001D*/ 
      .custom /*0C000086:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A00001E*/ 
      .custom /*0C000089:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T3 /*2A00001F*/ 
      .custom /*0C00008B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T4 /*2A000020*/ 
      .custom /*0C00008F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T5 /*2A000021*/ 
      .custom /*0C000092:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T6 /*2A000022*/ 
      .custom /*0C000096:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T7 /*2A000023*/ 
      .custom /*0C000099:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param type T8 /*2A000024*/ 
      .custom /*0C00009E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2924
    // Code size       547 (0x223)
    .maxstack  4
    .locals /*11000009*/ init (uint32 V_0,
             uint32 V_1,
             uint32 V_2,
             uint32 V_3,
             uint32 V_4,
             uint32 V_5,
             uint32 V_6,
             uint32 V_7,
             uint32 V_8,
             uint32 V_9,
             uint32 V_10,
             uint32 V_11,
             !!T1 V_12,
             !!T2 V_13,
             !!T3 V_14,
             !!T4 V_15,
             !!T5 V_16,
             !!T6 V_17,
             !!T7 V_18,
             !!T8 V_19)
    IL_0000:  /* 0F   | 00               */ ldarga.s   value1
    IL_0002:  /* 12   | 0C               */ ldloca.s   V_12
    IL_0004:  /* FE15 | (1B)000003       */ initobj    !!T1/*1B000003*/
    IL_000a:  /* 11   | 0C               */ ldloc.s    V_12
    IL_000c:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0011:  /* 2D   | 16               */ brtrue.s   IL_0029

    IL_0013:  /* 71   | (1B)000003       */ ldobj      !!T1/*1B000003*/
    IL_0018:  /* 13   | 0C               */ stloc.s    V_12
    IL_001a:  /* 12   | 0C               */ ldloca.s   V_12
    IL_001c:  /* 11   | 0C               */ ldloc.s    V_12
    IL_001e:  /* 8C   | (1B)000003       */ box        !!T1/*1B000003*/
    IL_0023:  /* 2D   | 04               */ brtrue.s   IL_0029

    IL_0025:  /* 26   |                  */ pop
    IL_0026:  /* 16   |                  */ ldc.i4.0
    IL_0027:  /* 2B   | 0B               */ br.s       IL_0034

    IL_0029:  /* FE16 | (1B)000003       */ constrained. !!T1/*1B000003*/
    IL_002f:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0034:  /* 0A   |                  */ stloc.0
    IL_0035:  /* 0F   | 01               */ ldarga.s   value2
    IL_0037:  /* 12   | 0D               */ ldloca.s   V_13
    IL_0039:  /* FE15 | (1B)000004       */ initobj    !!T2/*1B000004*/
    IL_003f:  /* 11   | 0D               */ ldloc.s    V_13
    IL_0041:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0046:  /* 2D   | 16               */ brtrue.s   IL_005e

    IL_0048:  /* 71   | (1B)000004       */ ldobj      !!T2/*1B000004*/
    IL_004d:  /* 13   | 0D               */ stloc.s    V_13
    IL_004f:  /* 12   | 0D               */ ldloca.s   V_13
    IL_0051:  /* 11   | 0D               */ ldloc.s    V_13
    IL_0053:  /* 8C   | (1B)000004       */ box        !!T2/*1B000004*/
    IL_0058:  /* 2D   | 04               */ brtrue.s   IL_005e

    IL_005a:  /* 26   |                  */ pop
    IL_005b:  /* 16   |                  */ ldc.i4.0
    IL_005c:  /* 2B   | 0B               */ br.s       IL_0069

    IL_005e:  /* FE16 | (1B)000004       */ constrained. !!T2/*1B000004*/
    IL_0064:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0069:  /* 0B   |                  */ stloc.1
    IL_006a:  /* 0F   | 02               */ ldarga.s   value3
    IL_006c:  /* 12   | 0E               */ ldloca.s   V_14
    IL_006e:  /* FE15 | (1B)000005       */ initobj    !!T3/*1B000005*/
    IL_0074:  /* 11   | 0E               */ ldloc.s    V_14
    IL_0076:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_007b:  /* 2D   | 16               */ brtrue.s   IL_0093

    IL_007d:  /* 71   | (1B)000005       */ ldobj      !!T3/*1B000005*/
    IL_0082:  /* 13   | 0E               */ stloc.s    V_14
    IL_0084:  /* 12   | 0E               */ ldloca.s   V_14
    IL_0086:  /* 11   | 0E               */ ldloc.s    V_14
    IL_0088:  /* 8C   | (1B)000005       */ box        !!T3/*1B000005*/
    IL_008d:  /* 2D   | 04               */ brtrue.s   IL_0093

    IL_008f:  /* 26   |                  */ pop
    IL_0090:  /* 16   |                  */ ldc.i4.0
    IL_0091:  /* 2B   | 0B               */ br.s       IL_009e

    IL_0093:  /* FE16 | (1B)000005       */ constrained. !!T3/*1B000005*/
    IL_0099:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_009e:  /* 0C   |                  */ stloc.2
    IL_009f:  /* 0F   | 03               */ ldarga.s   value4
    IL_00a1:  /* 12   | 0F               */ ldloca.s   V_15
    IL_00a3:  /* FE15 | (1B)000006       */ initobj    !!T4/*1B000006*/
    IL_00a9:  /* 11   | 0F               */ ldloc.s    V_15
    IL_00ab:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00b0:  /* 2D   | 16               */ brtrue.s   IL_00c8

    IL_00b2:  /* 71   | (1B)000006       */ ldobj      !!T4/*1B000006*/
    IL_00b7:  /* 13   | 0F               */ stloc.s    V_15
    IL_00b9:  /* 12   | 0F               */ ldloca.s   V_15
    IL_00bb:  /* 11   | 0F               */ ldloc.s    V_15
    IL_00bd:  /* 8C   | (1B)000006       */ box        !!T4/*1B000006*/
    IL_00c2:  /* 2D   | 04               */ brtrue.s   IL_00c8

    IL_00c4:  /* 26   |                  */ pop
    IL_00c5:  /* 16   |                  */ ldc.i4.0
    IL_00c6:  /* 2B   | 0B               */ br.s       IL_00d3

    IL_00c8:  /* FE16 | (1B)000006       */ constrained. !!T4/*1B000006*/
    IL_00ce:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_00d3:  /* 0D   |                  */ stloc.3
    IL_00d4:  /* 0F   | 04               */ ldarga.s   value5
    IL_00d6:  /* 12   | 10               */ ldloca.s   V_16
    IL_00d8:  /* FE15 | (1B)000007       */ initobj    !!T5/*1B000007*/
    IL_00de:  /* 11   | 10               */ ldloc.s    V_16
    IL_00e0:  /* 8C   | (1B)000007       */ box        !!T5/*1B000007*/
    IL_00e5:  /* 2D   | 16               */ brtrue.s   IL_00fd

    IL_00e7:  /* 71   | (1B)000007       */ ldobj      !!T5/*1B000007*/
    IL_00ec:  /* 13   | 10               */ stloc.s    V_16
    IL_00ee:  /* 12   | 10               */ ldloca.s   V_16
    IL_00f0:  /* 11   | 10               */ ldloc.s    V_16
    IL_00f2:  /* 8C   | (1B)000007       */ box        !!T5/*1B000007*/
    IL_00f7:  /* 2D   | 04               */ brtrue.s   IL_00fd

    IL_00f9:  /* 26   |                  */ pop
    IL_00fa:  /* 16   |                  */ ldc.i4.0
    IL_00fb:  /* 2B   | 0B               */ br.s       IL_0108

    IL_00fd:  /* FE16 | (1B)000007       */ constrained. !!T5/*1B000007*/
    IL_0103:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0108:  /* 13   | 04               */ stloc.s    V_4
    IL_010a:  /* 0F   | 05               */ ldarga.s   value6
    IL_010c:  /* 12   | 11               */ ldloca.s   V_17
    IL_010e:  /* FE15 | (1B)000008       */ initobj    !!T6/*1B000008*/
    IL_0114:  /* 11   | 11               */ ldloc.s    V_17
    IL_0116:  /* 8C   | (1B)000008       */ box        !!T6/*1B000008*/
    IL_011b:  /* 2D   | 16               */ brtrue.s   IL_0133

    IL_011d:  /* 71   | (1B)000008       */ ldobj      !!T6/*1B000008*/
    IL_0122:  /* 13   | 11               */ stloc.s    V_17
    IL_0124:  /* 12   | 11               */ ldloca.s   V_17
    IL_0126:  /* 11   | 11               */ ldloc.s    V_17
    IL_0128:  /* 8C   | (1B)000008       */ box        !!T6/*1B000008*/
    IL_012d:  /* 2D   | 04               */ brtrue.s   IL_0133

    IL_012f:  /* 26   |                  */ pop
    IL_0130:  /* 16   |                  */ ldc.i4.0
    IL_0131:  /* 2B   | 0B               */ br.s       IL_013e

    IL_0133:  /* FE16 | (1B)000008       */ constrained. !!T6/*1B000008*/
    IL_0139:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_013e:  /* 13   | 05               */ stloc.s    V_5
    IL_0140:  /* 0F   | 06               */ ldarga.s   value7
    IL_0142:  /* 12   | 12               */ ldloca.s   V_18
    IL_0144:  /* FE15 | (1B)000009       */ initobj    !!T7/*1B000009*/
    IL_014a:  /* 11   | 12               */ ldloc.s    V_18
    IL_014c:  /* 8C   | (1B)000009       */ box        !!T7/*1B000009*/
    IL_0151:  /* 2D   | 16               */ brtrue.s   IL_0169

    IL_0153:  /* 71   | (1B)000009       */ ldobj      !!T7/*1B000009*/
    IL_0158:  /* 13   | 12               */ stloc.s    V_18
    IL_015a:  /* 12   | 12               */ ldloca.s   V_18
    IL_015c:  /* 11   | 12               */ ldloc.s    V_18
    IL_015e:  /* 8C   | (1B)000009       */ box        !!T7/*1B000009*/
    IL_0163:  /* 2D   | 04               */ brtrue.s   IL_0169

    IL_0165:  /* 26   |                  */ pop
    IL_0166:  /* 16   |                  */ ldc.i4.0
    IL_0167:  /* 2B   | 0B               */ br.s       IL_0174

    IL_0169:  /* FE16 | (1B)000009       */ constrained. !!T7/*1B000009*/
    IL_016f:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0174:  /* 13   | 06               */ stloc.s    V_6
    IL_0176:  /* 0F   | 07               */ ldarga.s   value8
    IL_0178:  /* 12   | 13               */ ldloca.s   V_19
    IL_017a:  /* FE15 | (1B)00000A       */ initobj    !!T8/*1B00000A*/
    IL_0180:  /* 11   | 13               */ ldloc.s    V_19
    IL_0182:  /* 8C   | (1B)00000A       */ box        !!T8/*1B00000A*/
    IL_0187:  /* 2D   | 16               */ brtrue.s   IL_019f

    IL_0189:  /* 71   | (1B)00000A       */ ldobj      !!T8/*1B00000A*/
    IL_018e:  /* 13   | 13               */ stloc.s    V_19
    IL_0190:  /* 12   | 13               */ ldloca.s   V_19
    IL_0192:  /* 11   | 13               */ ldloc.s    V_19
    IL_0194:  /* 8C   | (1B)00000A       */ box        !!T8/*1B00000A*/
    IL_0199:  /* 2D   | 04               */ brtrue.s   IL_019f

    IL_019b:  /* 26   |                  */ pop
    IL_019c:  /* 16   |                  */ ldc.i4.0
    IL_019d:  /* 2B   | 0B               */ br.s       IL_01aa

    IL_019f:  /* FE16 | (1B)00000A       */ constrained. !!T8/*1B00000A*/
    IL_01a5:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_01aa:  /* 13   | 07               */ stloc.s    V_7
    IL_01ac:  /* 12   | 08               */ ldloca.s   V_8
    IL_01ae:  /* 12   | 09               */ ldloca.s   V_9
    IL_01b0:  /* 12   | 0A               */ ldloca.s   V_10
    IL_01b2:  /* 12   | 0B               */ ldloca.s   V_11
    IL_01b4:  /* 28   | (06)000012       */ call       void HashCode/*02000009*/::Initialize(uint32&,
                                                                                             uint32&,
                                                                                             uint32&,
                                                                                             uint32&) /* 06000012 */
    IL_01b9:  /* 11   | 08               */ ldloc.s    V_8
    IL_01bb:  /* 06   |                  */ ldloc.0
    IL_01bc:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_01c1:  /* 13   | 08               */ stloc.s    V_8
    IL_01c3:  /* 11   | 09               */ ldloc.s    V_9
    IL_01c5:  /* 07   |                  */ ldloc.1
    IL_01c6:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_01cb:  /* 13   | 09               */ stloc.s    V_9
    IL_01cd:  /* 11   | 0A               */ ldloc.s    V_10
    IL_01cf:  /* 08   |                  */ ldloc.2
    IL_01d0:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_01d5:  /* 13   | 0A               */ stloc.s    V_10
    IL_01d7:  /* 11   | 0B               */ ldloc.s    V_11
    IL_01d9:  /* 09   |                  */ ldloc.3
    IL_01da:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_01df:  /* 13   | 0B               */ stloc.s    V_11
    IL_01e1:  /* 11   | 08               */ ldloc.s    V_8
    IL_01e3:  /* 11   | 04               */ ldloc.s    V_4
    IL_01e5:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_01ea:  /* 13   | 08               */ stloc.s    V_8
    IL_01ec:  /* 11   | 09               */ ldloc.s    V_9
    IL_01ee:  /* 11   | 05               */ ldloc.s    V_5
    IL_01f0:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_01f5:  /* 13   | 09               */ stloc.s    V_9
    IL_01f7:  /* 11   | 0A               */ ldloc.s    V_10
    IL_01f9:  /* 11   | 06               */ ldloc.s    V_6
    IL_01fb:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0200:  /* 13   | 0A               */ stloc.s    V_10
    IL_0202:  /* 11   | 0B               */ ldloc.s    V_11
    IL_0204:  /* 11   | 07               */ ldloc.s    V_7
    IL_0206:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_020b:  /* 13   | 0B               */ stloc.s    V_11
    IL_020d:  /* 11   | 08               */ ldloc.s    V_8
    IL_020f:  /* 11   | 09               */ ldloc.s    V_9
    IL_0211:  /* 11   | 0A               */ ldloc.s    V_10
    IL_0213:  /* 11   | 0B               */ ldloc.s    V_11
    IL_0215:  /* 28   | (06)000015       */ call       uint32 HashCode/*02000009*/::MixState(uint32,
                                                                                             uint32,
                                                                                             uint32,
                                                                                             uint32) /* 06000015 */
    IL_021a:  /* 1F   | 20               */ ldc.i4.s   32
    IL_021c:  /* 58   |                  */ add
    IL_021d:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_0222:  /* 2A   |                  */ ret
  } // end of method HashCode::Combine

  .method /*06000012*/ private hidebysig static 
          void  Initialize([out] uint32& v1,
                           [out] uint32& v2,
                           [out] uint32& v3,
                           [out] uint32& v4) cil managed aggressiveinlining
  // SIG: 00 04 01 10 09 10 09 10 09 10 09
  {
    // Method begins at RVA 0x2b53
    // Code size       53 (0x35)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (04)000005       */ ldsfld     uint32 HashCode/*02000009*/::s_seed /* 04000005 */
    IL_0006:  /* 20   | B179379E         */ ldc.i4     0x9e3779b1
    IL_000b:  /* 58   |                  */ add
    IL_000c:  /* 20   | 77CAEB85         */ ldc.i4     0x85ebca77
    IL_0011:  /* 58   |                  */ add
    IL_0012:  /* 54   |                  */ stind.i4
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 7E   | (04)000005       */ ldsfld     uint32 HashCode/*02000009*/::s_seed /* 04000005 */
    IL_0019:  /* 20   | 77CAEB85         */ ldc.i4     0x85ebca77
    IL_001e:  /* 58   |                  */ add
    IL_001f:  /* 54   |                  */ stind.i4
    IL_0020:  /* 04   |                  */ ldarg.2
    IL_0021:  /* 7E   | (04)000005       */ ldsfld     uint32 HashCode/*02000009*/::s_seed /* 04000005 */
    IL_0026:  /* 54   |                  */ stind.i4
    IL_0027:  /* 05   |                  */ ldarg.3
    IL_0028:  /* 7E   | (04)000005       */ ldsfld     uint32 HashCode/*02000009*/::s_seed /* 04000005 */
    IL_002d:  /* 20   | B179379E         */ ldc.i4     0x9e3779b1
    IL_0032:  /* 59   |                  */ sub
    IL_0033:  /* 54   |                  */ stind.i4
    IL_0034:  /* 2A   |                  */ ret
  } // end of method HashCode::Initialize

  .method /*06000013*/ private hidebysig static 
          uint32  Round(uint32 hash,
                        uint32 input) cil managed aggressiveinlining
  // SIG: 00 02 09 09 09
  {
    // Method begins at RVA 0x2b89
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 20   | 77CAEB85         */ ldc.i4     0x85ebca77
    IL_0007:  /* 5A   |                  */ mul
    IL_0008:  /* 58   |                  */ add
    IL_0009:  /* 1F   | 0D               */ ldc.i4.s   13
    IL_000b:  /* 28   | (06)000064       */ call       uint32 HashCode/*02000009*//BitOperations/*02000015*/::RotateLeft(uint32,
                                                                                                                         int32) /* 06000064 */
    IL_0010:  /* 20   | B179379E         */ ldc.i4     0x9e3779b1
    IL_0015:  /* 5A   |                  */ mul
    IL_0016:  /* 2A   |                  */ ret
  } // end of method HashCode::Round

  .method /*06000014*/ private hidebysig static 
          uint32  QueueRound(uint32 hash,
                             uint32 queuedValue) cil managed aggressiveinlining
  // SIG: 00 02 09 09 09
  {
    // Method begins at RVA 0x2ba1
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 20   | 3DAEB2C2         */ ldc.i4     0xc2b2ae3d
    IL_0007:  /* 5A   |                  */ mul
    IL_0008:  /* 58   |                  */ add
    IL_0009:  /* 1F   | 11               */ ldc.i4.s   17
    IL_000b:  /* 28   | (06)000064       */ call       uint32 HashCode/*02000009*//BitOperations/*02000015*/::RotateLeft(uint32,
                                                                                                                         int32) /* 06000064 */
    IL_0010:  /* 20   | 2FEBD427         */ ldc.i4     0x27d4eb2f
    IL_0015:  /* 5A   |                  */ mul
    IL_0016:  /* 2A   |                  */ ret
  } // end of method HashCode::QueueRound

  .method /*06000015*/ private hidebysig static 
          uint32  MixState(uint32 v1,
                           uint32 v2,
                           uint32 v3,
                           uint32 v4) cil managed aggressiveinlining
  // SIG: 00 04 09 09 09 09 09
  {
    // Method begins at RVA 0x2bb9
    // Code size       34 (0x22)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 17   |                  */ ldc.i4.1
    IL_0002:  /* 28   | (06)000064       */ call       uint32 HashCode/*02000009*//BitOperations/*02000015*/::RotateLeft(uint32,
                                                                                                                         int32) /* 06000064 */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 1D   |                  */ ldc.i4.7
    IL_0009:  /* 28   | (06)000064       */ call       uint32 HashCode/*02000009*//BitOperations/*02000015*/::RotateLeft(uint32,
                                                                                                                         int32) /* 06000064 */
    IL_000e:  /* 58   |                  */ add
    IL_000f:  /* 04   |                  */ ldarg.2
    IL_0010:  /* 1F   | 0C               */ ldc.i4.s   12
    IL_0012:  /* 28   | (06)000064       */ call       uint32 HashCode/*02000009*//BitOperations/*02000015*/::RotateLeft(uint32,
                                                                                                                         int32) /* 06000064 */
    IL_0017:  /* 58   |                  */ add
    IL_0018:  /* 05   |                  */ ldarg.3
    IL_0019:  /* 1F   | 12               */ ldc.i4.s   18
    IL_001b:  /* 28   | (06)000064       */ call       uint32 HashCode/*02000009*//BitOperations/*02000015*/::RotateLeft(uint32,
                                                                                                                         int32) /* 06000064 */
    IL_0020:  /* 58   |                  */ add
    IL_0021:  /* 2A   |                  */ ret
  } // end of method HashCode::MixState

  .method /*06000016*/ private hidebysig static 
          uint32  MixEmptyState() cil managed
  // SIG: 00 00 09
  {
    // Method begins at RVA 0x2bdc
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 7E   | (04)000005       */ ldsfld     uint32 HashCode/*02000009*/::s_seed /* 04000005 */
    IL_0005:  /* 20   | B1675616         */ ldc.i4     0x165667b1
    IL_000a:  /* 58   |                  */ add
    IL_000b:  /* 2A   |                  */ ret
  } // end of method HashCode::MixEmptyState

  .method /*06000017*/ private hidebysig static 
          uint32  MixFinal(uint32 hash) cil managed aggressiveinlining
  // SIG: 00 01 09 09
  {
    // Method begins at RVA 0x2be9
    // Code size       44 (0x2c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 1F   | 0F               */ ldc.i4.s   15
    IL_0004:  /* 64   |                  */ shr.un
    IL_0005:  /* 61   |                  */ xor
    IL_0006:  /* 10   | 00               */ starg.s    hash
    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 20   | 77CAEB85         */ ldc.i4     0x85ebca77
    IL_000e:  /* 5A   |                  */ mul
    IL_000f:  /* 10   | 00               */ starg.s    hash
    IL_0011:  /* 02   |                  */ ldarg.0
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* 1F   | 0D               */ ldc.i4.s   13
    IL_0015:  /* 64   |                  */ shr.un
    IL_0016:  /* 61   |                  */ xor
    IL_0017:  /* 10   | 00               */ starg.s    hash
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 20   | 3DAEB2C2         */ ldc.i4     0xc2b2ae3d
    IL_001f:  /* 5A   |                  */ mul
    IL_0020:  /* 10   | 00               */ starg.s    hash
    IL_0022:  /* 02   |                  */ ldarg.0
    IL_0023:  /* 02   |                  */ ldarg.0
    IL_0024:  /* 1F   | 10               */ ldc.i4.s   16
    IL_0026:  /* 64   |                  */ shr.un
    IL_0027:  /* 61   |                  */ xor
    IL_0028:  /* 10   | 00               */ starg.s    hash
    IL_002a:  /* 02   |                  */ ldarg.0
    IL_002b:  /* 2A   |                  */ ret
  } // end of method HashCode::MixFinal

  .method /*06000018*/ public hidebysig instance void 
          Add<T>(!!T 'value') cil managed
  // SIG: 30 01 01 01 1E 00
  {
    .param type T /*2A000025*/ 
      .custom /*0C0000A2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2c18
    // Code size       56 (0x38)
    .maxstack  3
    .locals /*1100000A*/ init (!!T V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 0F   | 01               */ ldarga.s   'value'
    IL_0003:  /* 12   | 00               */ ldloca.s   V_0
    IL_0005:  /* FE15 | (1B)000003       */ initobj    !!T/*1B000003*/
    IL_000b:  /* 06   |                  */ ldloc.0
    IL_000c:  /* 8C   | (1B)000003       */ box        !!T/*1B000003*/
    IL_0011:  /* 2D   | 14               */ brtrue.s   IL_0027

    IL_0013:  /* 71   | (1B)000003       */ ldobj      !!T/*1B000003*/
    IL_0018:  /* 0A   |                  */ stloc.0
    IL_0019:  /* 12   | 00               */ ldloca.s   V_0
    IL_001b:  /* 06   |                  */ ldloc.0
    IL_001c:  /* 8C   | (1B)000003       */ box        !!T/*1B000003*/
    IL_0021:  /* 2D   | 04               */ brtrue.s   IL_0027

    IL_0023:  /* 26   |                  */ pop
    IL_0024:  /* 16   |                  */ ldc.i4.0
    IL_0025:  /* 2B   | 0B               */ br.s       IL_0032

    IL_0027:  /* FE16 | (1B)000003       */ constrained. !!T/*1B000003*/
    IL_002d:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0032:  /* 28   | (06)00001A       */ call       instance void HashCode/*02000009*/::Add(int32) /* 0600001A */
    IL_0037:  /* 2A   |                  */ ret
  } // end of method HashCode::Add

  .method /*06000019*/ public hidebysig instance void 
          Add<T>(!!T 'value',
                 class [netstandard/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*0100001A*/<!!T> comparer) cil managed
  // SIG: 30 01 02 01 1E 00 15 12 69 01 1E 00
  {
    .param type T /*2A000026*/ 
      .custom /*0C0000A5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000035*/ 
    .custom /*0C0000BA:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2c5c
    // Code size       43 (0x2b)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 8C   | (1B)000003       */ box        !!T/*1B000003*/
    IL_0007:  /* 2C   | 1B               */ brfalse.s  IL_0024

    IL_0009:  /* 04   |                  */ ldarg.2
    IL_000a:  /* 2D   | 0F               */ brtrue.s   IL_001b

    IL_000c:  /* 0F   | 01               */ ldarga.s   'value'
    IL_000e:  /* FE16 | (1B)000003       */ constrained. !!T/*1B000003*/
    IL_0014:  /* 6F   | (0A)000024       */ callvirt   instance int32 [netstandard/*23000001*/]System.Object/*01000015*/::GetHashCode() /* 0A000024 */
    IL_0019:  /* 2B   | 0A               */ br.s       IL_0025

    IL_001b:  /* 04   |                  */ ldarg.2
    IL_001c:  /* 03   |                  */ ldarg.1
    IL_001d:  /* 6F   | (0A)000025       */ callvirt   instance int32 class [netstandard/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*0100001A*/<!!T>/*1B00000B*/::GetHashCode(!0) /* 0A000025 */
    IL_0022:  /* 2B   | 01               */ br.s       IL_0025

    IL_0024:  /* 16   |                  */ ldc.i4.0
    IL_0025:  /* 28   | (06)00001A       */ call       instance void HashCode/*02000009*/::Add(int32) /* 0600001A */
    IL_002a:  /* 2A   |                  */ ret
  } // end of method HashCode::Add

  .method /*0600001A*/ private hidebysig 
          instance void  Add(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2c88
    // Code size       180 (0xb4)
    .maxstack  4
    .locals /*1100000B*/ init (uint32 V_0,
             uint32 V_1,
             uint32 V_2,
             uint32 V_3)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 02   |                  */ ldarg.0
    IL_0004:  /* 7B   | (04)000012       */ ldfld      uint32 HashCode/*02000009*/::_length /* 04000012 */
    IL_0009:  /* 0D   |                  */ stloc.3
    IL_000a:  /* 09   |                  */ ldloc.3
    IL_000b:  /* 17   |                  */ ldc.i4.1
    IL_000c:  /* 58   |                  */ add
    IL_000d:  /* 7D   | (04)000012       */ stfld      uint32 HashCode/*02000009*/::_length /* 04000012 */
    IL_0012:  /* 09   |                  */ ldloc.3
    IL_0013:  /* 0B   |                  */ stloc.1
    IL_0014:  /* 07   |                  */ ldloc.1
    IL_0015:  /* 1A   |                  */ ldc.i4.4
    IL_0016:  /* 5E   |                  */ rem.un
    IL_0017:  /* 0C   |                  */ stloc.2
    IL_0018:  /* 08   |                  */ ldloc.2
    IL_0019:  /* 2D   | 08               */ brtrue.s   IL_0023

    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 06   |                  */ ldloc.0
    IL_001d:  /* 7D   | (04)00000F       */ stfld      uint32 HashCode/*02000009*/::_queue1 /* 0400000F */
    IL_0022:  /* 2A   |                  */ ret

    IL_0023:  /* 08   |                  */ ldloc.2
    IL_0024:  /* 17   |                  */ ldc.i4.1
    IL_0025:  /* 33   | 08               */ bne.un.s   IL_002f

    IL_0027:  /* 02   |                  */ ldarg.0
    IL_0028:  /* 06   |                  */ ldloc.0
    IL_0029:  /* 7D   | (04)000010       */ stfld      uint32 HashCode/*02000009*/::_queue2 /* 04000010 */
    IL_002e:  /* 2A   |                  */ ret

    IL_002f:  /* 08   |                  */ ldloc.2
    IL_0030:  /* 18   |                  */ ldc.i4.2
    IL_0031:  /* 33   | 08               */ bne.un.s   IL_003b

    IL_0033:  /* 02   |                  */ ldarg.0
    IL_0034:  /* 06   |                  */ ldloc.0
    IL_0035:  /* 7D   | (04)000011       */ stfld      uint32 HashCode/*02000009*/::_queue3 /* 04000011 */
    IL_003a:  /* 2A   |                  */ ret

    IL_003b:  /* 07   |                  */ ldloc.1
    IL_003c:  /* 19   |                  */ ldc.i4.3
    IL_003d:  /* 33   | 1D               */ bne.un.s   IL_005c

    IL_003f:  /* 02   |                  */ ldarg.0
    IL_0040:  /* 7C   | (04)00000B       */ ldflda     uint32 HashCode/*02000009*/::_v1 /* 0400000B */
    IL_0045:  /* 02   |                  */ ldarg.0
    IL_0046:  /* 7C   | (04)00000C       */ ldflda     uint32 HashCode/*02000009*/::_v2 /* 0400000C */
    IL_004b:  /* 02   |                  */ ldarg.0
    IL_004c:  /* 7C   | (04)00000D       */ ldflda     uint32 HashCode/*02000009*/::_v3 /* 0400000D */
    IL_0051:  /* 02   |                  */ ldarg.0
    IL_0052:  /* 7C   | (04)00000E       */ ldflda     uint32 HashCode/*02000009*/::_v4 /* 0400000E */
    IL_0057:  /* 28   | (06)000012       */ call       void HashCode/*02000009*/::Initialize(uint32&,
                                                                                             uint32&,
                                                                                             uint32&,
                                                                                             uint32&) /* 06000012 */
    IL_005c:  /* 02   |                  */ ldarg.0
    IL_005d:  /* 02   |                  */ ldarg.0
    IL_005e:  /* 7B   | (04)00000B       */ ldfld      uint32 HashCode/*02000009*/::_v1 /* 0400000B */
    IL_0063:  /* 02   |                  */ ldarg.0
    IL_0064:  /* 7B   | (04)00000F       */ ldfld      uint32 HashCode/*02000009*/::_queue1 /* 0400000F */
    IL_0069:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_006e:  /* 7D   | (04)00000B       */ stfld      uint32 HashCode/*02000009*/::_v1 /* 0400000B */
    IL_0073:  /* 02   |                  */ ldarg.0
    IL_0074:  /* 02   |                  */ ldarg.0
    IL_0075:  /* 7B   | (04)00000C       */ ldfld      uint32 HashCode/*02000009*/::_v2 /* 0400000C */
    IL_007a:  /* 02   |                  */ ldarg.0
    IL_007b:  /* 7B   | (04)000010       */ ldfld      uint32 HashCode/*02000009*/::_queue2 /* 04000010 */
    IL_0080:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_0085:  /* 7D   | (04)00000C       */ stfld      uint32 HashCode/*02000009*/::_v2 /* 0400000C */
    IL_008a:  /* 02   |                  */ ldarg.0
    IL_008b:  /* 02   |                  */ ldarg.0
    IL_008c:  /* 7B   | (04)00000D       */ ldfld      uint32 HashCode/*02000009*/::_v3 /* 0400000D */
    IL_0091:  /* 02   |                  */ ldarg.0
    IL_0092:  /* 7B   | (04)000011       */ ldfld      uint32 HashCode/*02000009*/::_queue3 /* 04000011 */
    IL_0097:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_009c:  /* 7D   | (04)00000D       */ stfld      uint32 HashCode/*02000009*/::_v3 /* 0400000D */
    IL_00a1:  /* 02   |                  */ ldarg.0
    IL_00a2:  /* 02   |                  */ ldarg.0
    IL_00a3:  /* 7B   | (04)00000E       */ ldfld      uint32 HashCode/*02000009*/::_v4 /* 0400000E */
    IL_00a8:  /* 06   |                  */ ldloc.0
    IL_00a9:  /* 28   | (06)000013       */ call       uint32 HashCode/*02000009*/::Round(uint32,
                                                                                          uint32) /* 06000013 */
    IL_00ae:  /* 7D   | (04)00000E       */ stfld      uint32 HashCode/*02000009*/::_v4 /* 0400000E */
    IL_00b3:  /* 2A   |                  */ ret
  } // end of method HashCode::Add

  .method /*0600001B*/ public hidebysig instance int32 
          ToHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2d48
    // Code size       118 (0x76)
    .maxstack  4
    .locals /*1100000C*/ init (uint32 V_0,
             uint32 V_1,
             uint32 V_2)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000012       */ ldfld      uint32 HashCode/*02000009*/::_length /* 04000012 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 06   |                  */ ldloc.0
    IL_0008:  /* 1A   |                  */ ldc.i4.4
    IL_0009:  /* 5E   |                  */ rem.un
    IL_000a:  /* 0B   |                  */ stloc.1
    IL_000b:  /* 06   |                  */ ldloc.0
    IL_000c:  /* 1A   |                  */ ldc.i4.4
    IL_000d:  /* 37   | 1F               */ blt.un.s   IL_002e

    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 7B   | (04)00000B       */ ldfld      uint32 HashCode/*02000009*/::_v1 /* 0400000B */
    IL_0015:  /* 02   |                  */ ldarg.0
    IL_0016:  /* 7B   | (04)00000C       */ ldfld      uint32 HashCode/*02000009*/::_v2 /* 0400000C */
    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 7B   | (04)00000D       */ ldfld      uint32 HashCode/*02000009*/::_v3 /* 0400000D */
    IL_0021:  /* 02   |                  */ ldarg.0
    IL_0022:  /* 7B   | (04)00000E       */ ldfld      uint32 HashCode/*02000009*/::_v4 /* 0400000E */
    IL_0027:  /* 28   | (06)000015       */ call       uint32 HashCode/*02000009*/::MixState(uint32,
                                                                                             uint32,
                                                                                             uint32,
                                                                                             uint32) /* 06000015 */
    IL_002c:  /* 2B   | 05               */ br.s       IL_0033

    IL_002e:  /* 28   | (06)000016       */ call       uint32 HashCode/*02000009*/::MixEmptyState() /* 06000016 */
    IL_0033:  /* 0C   |                  */ stloc.2
    IL_0034:  /* 08   |                  */ ldloc.2
    IL_0035:  /* 06   |                  */ ldloc.0
    IL_0036:  /* 1A   |                  */ ldc.i4.4
    IL_0037:  /* 5A   |                  */ mul
    IL_0038:  /* 58   |                  */ add
    IL_0039:  /* 0C   |                  */ stloc.2
    IL_003a:  /* 07   |                  */ ldloc.1
    IL_003b:  /* 16   |                  */ ldc.i4.0
    IL_003c:  /* 36   | 2F               */ ble.un.s   IL_006d

    IL_003e:  /* 08   |                  */ ldloc.2
    IL_003f:  /* 02   |                  */ ldarg.0
    IL_0040:  /* 7B   | (04)00000F       */ ldfld      uint32 HashCode/*02000009*/::_queue1 /* 0400000F */
    IL_0045:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_004a:  /* 0C   |                  */ stloc.2
    IL_004b:  /* 07   |                  */ ldloc.1
    IL_004c:  /* 17   |                  */ ldc.i4.1
    IL_004d:  /* 36   | 1E               */ ble.un.s   IL_006d

    IL_004f:  /* 08   |                  */ ldloc.2
    IL_0050:  /* 02   |                  */ ldarg.0
    IL_0051:  /* 7B   | (04)000010       */ ldfld      uint32 HashCode/*02000009*/::_queue2 /* 04000010 */
    IL_0056:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_005b:  /* 0C   |                  */ stloc.2
    IL_005c:  /* 07   |                  */ ldloc.1
    IL_005d:  /* 18   |                  */ ldc.i4.2
    IL_005e:  /* 36   | 0D               */ ble.un.s   IL_006d

    IL_0060:  /* 08   |                  */ ldloc.2
    IL_0061:  /* 02   |                  */ ldarg.0
    IL_0062:  /* 7B   | (04)000011       */ ldfld      uint32 HashCode/*02000009*/::_queue3 /* 04000011 */
    IL_0067:  /* 28   | (06)000014       */ call       uint32 HashCode/*02000009*/::QueueRound(uint32,
                                                                                               uint32) /* 06000014 */
    IL_006c:  /* 0C   |                  */ stloc.2
    IL_006d:  /* 08   |                  */ ldloc.2
    IL_006e:  /* 28   | (06)000017       */ call       uint32 HashCode/*02000009*/::MixFinal(uint32) /* 06000017 */
    IL_0073:  /* 0C   |                  */ stloc.2
    IL_0074:  /* 08   |                  */ ldloc.2
    IL_0075:  /* 2A   |                  */ ret
  } // end of method HashCode::ToHashCode

  .method /*0600001C*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00007C:0A000012*/ instance void [netstandard/*23000001*/]System.ObsoleteAttribute/*0100001B*/::.ctor(string,
                                                                                                                     bool) /* 0A000012 */ = ( 01 00 80 80 48 61 73 68 43 6F 64 65 20 69 73 20   // ....HashCode is 
                                                                                                                                              61 20 6D 75 74 61 62 6C 65 20 73 74 72 75 63 74   // a mutable struct
                                                                                                                                              20 61 6E 64 20 73 68 6F 75 6C 64 20 6E 6F 74 20   //  and should not 
                                                                                                                                              62 65 20 63 6F 6D 70 61 72 65 64 20 77 69 74 68   // be compared with
                                                                                                                                              20 6F 74 68 65 72 20 48 61 73 68 43 6F 64 65 73   //  other HashCodes
                                                                                                                                              2E 20 55 73 65 20 54 6F 48 61 73 68 43 6F 64 65   // . Use ToHashCode
                                                                                                                                              20 74 6F 20 72 65 74 72 69 65 76 65 20 74 68 65   //  to retrieve the
                                                                                                                                              20 63 6F 6D 70 75 74 65 64 20 68 61 73 68 20 63   //  computed hash c
                                                                                                                                              6F 64 65 2E 01 00 00 )                            // ode....
    .custom /*0C00007D:0A000013*/ instance void [netstandard/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001D*/::.ctor(valuetype [netstandard/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001C*/) /* 0A000013 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2dca
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 7E   | (04)00001F       */ ldsfld     string HashCode/*02000009*//SR/*02000014*/::HashCode_HashCodeNotSupported /* 0400001F */
    IL_0005:  /* 73   | (0A)000026       */ newobj     instance void [netstandard/*23000001*/]System.NotSupportedException/*0100006B*/::.ctor(string) /* 0A000026 */
    IL_000a:  /* 7A   |                  */ throw
  } // end of method HashCode::GetHashCode

  .method /*0600001D*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000081:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000082:0A000012*/ instance void [netstandard/*23000001*/]System.ObsoleteAttribute/*0100001B*/::.ctor(string,
                                                                                                                     bool) /* 0A000012 */ = ( 01 00 4D 48 61 73 68 43 6F 64 65 20 69 73 20 61   // ..MHashCode is a
                                                                                                                                              20 6D 75 74 61 62 6C 65 20 73 74 72 75 63 74 20   //  mutable struct 
                                                                                                                                              61 6E 64 20 73 68 6F 75 6C 64 20 6E 6F 74 20 62   // and should not b
                                                                                                                                              65 20 63 6F 6D 70 61 72 65 64 20 77 69 74 68 20   // e compared with 
                                                                                                                                              6F 74 68 65 72 20 48 61 73 68 43 6F 64 65 73 2E   // other HashCodes.
                                                                                                                                              01 00 00 ) 
    .custom /*0C000083:0A000013*/ instance void [netstandard/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001D*/::.ctor(valuetype [netstandard/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001C*/) /* 0A000013 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2dd6
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 7E   | (04)000020       */ ldsfld     string HashCode/*02000009*//SR/*02000014*/::HashCode_EqualityNotSupported /* 04000020 */
    IL_0005:  /* 73   | (0A)000026       */ newobj     instance void [netstandard/*23000001*/]System.NotSupportedException/*0100006B*/::.ctor(string) /* 0A000026 */
    IL_000a:  /* 7A   |                  */ throw
  } // end of method HashCode::Equals

  .method /*0600001E*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2de2
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 28   | (06)000009       */ call       uint32 HashCode/*02000009*/::GenerateGlobalSeed() /* 06000009 */
    IL_0005:  /* 80   | (04)000005       */ stsfld     uint32 HashCode/*02000009*/::s_seed /* 04000005 */
    IL_000a:  /* 2A   |                  */ ret
  } // end of method HashCode::.cctor

} // end of class HashCode

.class /*0200000A*/ private sequential ansi sealed beforefieldinit Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace
       extends [netstandard/*23000001*/]System.ValueType/*01000018*/
       implements class [netstandard/*23000001*/]System.IEquatable`1/*0100001E*/<valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000001*/
{
  .custom /*0C000037:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [netstandard/*23000001*/]System.IEquatable`1/*0100001E*/<valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000001*/
  .custom /*0C000001:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .field /*04000013*/ private string '<packageName>k__BackingField'
  .custom /*0C000061:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*04000014*/ private string '<namespaceName>k__BackingField'
  .custom /*0C000067:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*0600001F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string packageName,
                               string namespaceName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2dee
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000013       */ stfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<packageName>k__BackingField' /* 04000013 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 7D   | (04)000014       */ stfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<namespaceName>k__BackingField' /* 04000014 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::.ctor

  .method /*06000020*/ public hidebysig specialname 
          instance string  get_packageName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    .custom /*0C00008D:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2dfe
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000013       */ ldfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<packageName>k__BackingField' /* 04000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::get_packageName

  .method /*06000021*/ public hidebysig specialname 
          instance void  set_packageName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000090:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2e06
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000013       */ stfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<packageName>k__BackingField' /* 04000013 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::set_packageName

  .method /*06000022*/ public hidebysig specialname 
          instance string  get_namespaceName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000093:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    .custom /*0C000094:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2e0f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000014       */ ldfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<namespaceName>k__BackingField' /* 04000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::get_namespaceName

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_namespaceName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000097:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2e17
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000014       */ stfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<namespaceName>k__BackingField' /* 04000014 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::set_namespaceName

  .method /*06000024*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00009A:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    .custom /*0C00009B:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00009C:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2e20
    // Code size       64 (0x40)
    .maxstack  2
    .locals /*1100000D*/ init (class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ V_0)
    IL_0000:  /* 73   | (0A)000027       */ newobj     instance void [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::.ctor() /* 0A000027 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 06   |                  */ ldloc.0
    IL_0007:  /* 72   | (70)0000F3       */ ldstr      "PackageSourceAndNamespace" /* 700000F3 */
    IL_000c:  /* 6F   | (0A)000028       */ callvirt   instance class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::Append(string) /* 0A000028 */
    IL_0011:  /* 26   |                  */ pop
    IL_0012:  /* 06   |                  */ ldloc.0
    IL_0013:  /* 72   | (70)000127       */ ldstr      " { " /* 70000127 */
    IL_0018:  /* 6F   | (0A)000028       */ callvirt   instance class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::Append(string) /* 0A000028 */
    IL_001d:  /* 26   |                  */ pop
    IL_001e:  /* 02   |                  */ ldarg.0
    IL_001f:  /* 06   |                  */ ldloc.0
    IL_0020:  /* 28   | (06)000025       */ call       instance bool Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::PrintMembers(class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/) /* 06000025 */
    IL_0025:  /* 2C   | 09               */ brfalse.s  IL_0030

    IL_0027:  /* 06   |                  */ ldloc.0
    IL_0028:  /* 1F   | 20               */ ldc.i4.s   32
    IL_002a:  /* 6F   | (0A)000029       */ callvirt   instance class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::Append(char) /* 0A000029 */
    IL_002f:  /* 26   |                  */ pop
    IL_0030:  /* 06   |                  */ ldloc.0
    IL_0031:  /* 1F   | 7D               */ ldc.i4.s   125
    IL_0033:  /* 6F   | (0A)000029       */ callvirt   instance class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::Append(char) /* 0A000029 */
    IL_0038:  /* 26   |                  */ pop
    IL_0039:  /* 06   |                  */ ldloc.0
    IL_003a:  /* 6F   | (0A)00002A       */ callvirt   instance string [netstandard/*23000001*/]System.Object/*01000015*/::ToString() /* 0A00002A */
    IL_003f:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::ToString

  .method /*06000025*/ private hidebysig 
          instance bool  PrintMembers(class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ builder) cil managed
  // SIG: 20 01 02 12 7D
  {
    .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A0:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2e6c
    // Code size       52 (0x34)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 72   | (70)00012F       */ ldstr      "packageName = " /* 7000012F */
    IL_0006:  /* 6F   | (0A)000028       */ callvirt   instance class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::Append(string) /* 0A000028 */
    IL_000b:  /* 26   |                  */ pop
    IL_000c:  /* 03   |                  */ ldarg.1
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 28   | (06)000020       */ call       instance string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::get_packageName() /* 06000020 */
    IL_0013:  /* 6F   | (0A)00002B       */ callvirt   instance class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::Append(object) /* 0A00002B */
    IL_0018:  /* 26   |                  */ pop
    IL_0019:  /* 03   |                  */ ldarg.1
    IL_001a:  /* 72   | (70)00014D       */ ldstr      ", namespaceName = " /* 7000014D */
    IL_001f:  /* 6F   | (0A)000028       */ callvirt   instance class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::Append(string) /* 0A000028 */
    IL_0024:  /* 26   |                  */ pop
    IL_0025:  /* 03   |                  */ ldarg.1
    IL_0026:  /* 02   |                  */ ldarg.0
    IL_0027:  /* 28   | (06)000022       */ call       instance string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::get_namespaceName() /* 06000022 */
    IL_002c:  /* 6F   | (0A)00002B       */ callvirt   instance class [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/ [netstandard/*23000001*/]System.Text.StringBuilder/*0100001F*/::Append(object) /* 0A00002B */
    IL_0031:  /* 26   |                  */ pop
    IL_0032:  /* 17   |                  */ ldc.i4.1
    IL_0033:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::PrintMembers

  .method /*06000026*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/ left,
                              valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/ right) cil managed
  // SIG: 00 02 02 11 28 11 28
  {
    .custom /*0C0000A3:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2ea1
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)000027       */ call       bool Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::op_Equality(valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/,
                                                                                                                                              valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/) /* 06000027 */
    IL_0007:  /* 16   |                  */ ldc.i4.0
    IL_0008:  /* FE01 |                  */ ceq
    IL_000a:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::op_Inequality

  .method /*06000027*/ public hidebysig specialname static 
          bool  op_Equality(valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/ left,
                            valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/ right) cil managed
  // SIG: 00 02 02 11 28 11 28
  {
    .custom /*0C0000A6:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2ead
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 0F   | 00               */ ldarga.s   left
    IL_0002:  /* 03   |                  */ ldarg.1
    IL_0003:  /* 28   | (06)00002A       */ call       instance bool Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::Equals(valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/) /* 0600002A */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::op_Equality

  .method /*06000028*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A9:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2eb7
    // Code size       40 (0x28)
    .maxstack  8
    IL_0000:  /* 28   | (0A)00002C       */ call       class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<!0> class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<string>/*1B00000C*/::get_Default() /* 0A00002C */
    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 7B   | (04)000013       */ ldfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<packageName>k__BackingField' /* 04000013 */
    IL_000b:  /* 6F   | (0A)00002D       */ callvirt   instance int32 class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<string>/*1B00000C*/::GetHashCode(!0) /* 0A00002D */
    IL_0010:  /* 20   | 295555A5         */ ldc.i4     0xa5555529
    IL_0015:  /* 5A   |                  */ mul
    IL_0016:  /* 28   | (0A)00002C       */ call       class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<!0> class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<string>/*1B00000C*/::get_Default() /* 0A00002C */
    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 7B   | (04)000014       */ ldfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<namespaceName>k__BackingField' /* 04000014 */
    IL_0021:  /* 6F   | (0A)00002D       */ callvirt   instance int32 class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<string>/*1B00000C*/::GetHashCode(!0) /* 0A00002D */
    IL_0026:  /* 58   |                  */ add
    IL_0027:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::GetHashCode

  .method /*06000029*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000AB:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AC:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0000AD:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2ee0
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 75   | (02)00000A       */ isinst     Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/
    IL_0006:  /* 2C   | 0D               */ brfalse.s  IL_0015

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 03   |                  */ ldarg.1
    IL_000a:  /* A5   | (02)00000A       */ unbox.any  Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/
    IL_000f:  /* 28   | (06)00002A       */ call       instance bool Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::Equals(valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/) /* 0600002A */
    IL_0014:  /* 2A   |                  */ ret

    IL_0015:  /* 16   |                  */ ldc.i4.0
    IL_0016:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::Equals

  .method /*0600002A*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/ other) cil managed
  // SIG: 20 01 02 11 28
  {
    .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B0:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2ef8
    // Code size       49 (0x31)
    .maxstack  8
    IL_0000:  /* 28   | (0A)00002C       */ call       class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<!0> class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<string>/*1B00000C*/::get_Default() /* 0A00002C */
    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 7B   | (04)000013       */ ldfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<packageName>k__BackingField' /* 04000013 */
    IL_000b:  /* 03   |                  */ ldarg.1
    IL_000c:  /* 7B   | (04)000013       */ ldfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<packageName>k__BackingField' /* 04000013 */
    IL_0011:  /* 6F   | (0A)00002E       */ callvirt   instance bool class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<string>/*1B00000C*/::Equals(!0,
                                                                                                                                                                                          !0) /* 0A00002E */
    IL_0016:  /* 2C   | 17               */ brfalse.s  IL_002f

    IL_0018:  /* 28   | (0A)00002C       */ call       class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<!0> class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<string>/*1B00000C*/::get_Default() /* 0A00002C */
    IL_001d:  /* 02   |                  */ ldarg.0
    IL_001e:  /* 7B   | (04)000014       */ ldfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<namespaceName>k__BackingField' /* 04000014 */
    IL_0023:  /* 03   |                  */ ldarg.1
    IL_0024:  /* 7B   | (04)000014       */ ldfld      string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::'<namespaceName>k__BackingField' /* 04000014 */
    IL_0029:  /* 6F   | (0A)00002E       */ callvirt   instance bool class [netstandard/*23000001*/]System.Collections.Generic.EqualityComparer`1/*0100006C*/<string>/*1B00000C*/::Equals(!0,
                                                                                                                                                                                          !0) /* 0A00002E */
    IL_002e:  /* 2A   |                  */ ret

    IL_002f:  /* 16   |                  */ ldc.i4.0
    IL_0030:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::Equals

  .method /*0600002B*/ public hidebysig instance void 
          Deconstruct([out] string& packageName,
                      [out] string& namespaceName) cil managed
  // SIG: 20 02 01 10 0E 10 0E
  {
    .custom /*0C0000B2:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B3:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2f2a
    // Code size       17 (0x11)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (06)000020       */ call       instance string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::get_packageName() /* 06000020 */
    IL_0007:  /* 51   |                  */ stind.ref
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 02   |                  */ ldarg.0
    IL_000a:  /* 28   | (06)000022       */ call       instance string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::get_namespaceName() /* 06000022 */
    IL_000f:  /* 51   |                  */ stind.ref
    IL_0010:  /* 2A   |                  */ ret
  } // end of method PackageSourceAndNamespace::Deconstruct

  .property /*17000001*/ instance callconv(8) string
          packageName()
  {
    .get instance string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::get_packageName() /* 06000020 */
    .set instance void Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::set_packageName(string) /* 06000021 */
  } // end of property PackageSourceAndNamespace::packageName
  .property /*17000002*/ instance callconv(8) string
          namespaceName()
  {
    .get instance string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::get_namespaceName() /* 06000022 */
    .set instance void Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::set_namespaceName(string) /* 06000023 */
  } // end of property PackageSourceAndNamespace::namespaceName
} // end of class Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace

.class /*0200000B*/ private sequential ansi sealed beforefieldinit Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod
       extends [netstandard/*23000001*/]System.ValueType/*01000018*/
{
  .custom /*0C00003B:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.IsReadOnlyAttribute/*02000003*/::.ctor() /* 06000002 */ = ( 01 00 00 00 ) 
  .field /*04000015*/ private initonly class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ '<ThisType>k__BackingField'
  .custom /*0C000069:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*04000016*/ private initonly string '<ExtensionMethod>k__BackingField'
  .custom /*0C00006B:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*0600002C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ thisType,
                               string extensionMethod) cil managed
  // SIG: 20 02 01 12 80 81 0E
  {
    // Method begins at RVA 0x2f3c
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000015       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::'<ThisType>k__BackingField' /* 04000015 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 7D   | (04)000016       */ stfld      string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::'<ExtensionMethod>k__BackingField' /* 04000016 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method ThisAndExtensionMethod::.ctor

  .method /*0600002D*/ public hidebysig specialname 
          instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ 
          get_ThisType() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C0000B6:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2f4c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000015       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::'<ThisType>k__BackingField' /* 04000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThisAndExtensionMethod::get_ThisType

  .method /*0600002E*/ public hidebysig specialname 
          instance string  get_ExtensionMethod() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B7:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2f54
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000016       */ ldfld      string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::'<ExtensionMethod>k__BackingField' /* 04000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThisAndExtensionMethod::get_ExtensionMethod

  .method /*0600002F*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x2f5c
    // Code size       61 (0x3d)
    .maxstack  3
    .locals /*1100000E*/ init (valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/ V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 75   | (02)00000B       */ isinst     Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/
    IL_0006:  /* 2C   | 33               */ brfalse.s  IL_003b

    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* A5   | (02)00000B       */ unbox.any  Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/
    IL_000e:  /* 0A   |                  */ stloc.0
    IL_000f:  /* 7E   | (0A)00002F       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/::Default /* 0A00002F */
    IL_0014:  /* 02   |                  */ ldarg.0
    IL_0015:  /* 28   | (06)00002D       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ThisType() /* 0600002D */
    IL_001a:  /* 12   | 00               */ ldloca.s   V_0
    IL_001c:  /* 28   | (06)00002D       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ThisType() /* 0600002D */
    IL_0021:  /* 6F   | (0A)000030       */ callvirt   instance bool [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/::Equals(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/,
                                                                                                                                                                           class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/) /* 0A000030 */
    IL_0026:  /* 2C   | 13               */ brfalse.s  IL_003b

    IL_0028:  /* 02   |                  */ ldarg.0
    IL_0029:  /* 28   | (06)00002E       */ call       instance string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ExtensionMethod() /* 0600002E */
    IL_002e:  /* 12   | 00               */ ldloca.s   V_0
    IL_0030:  /* 28   | (06)00002E       */ call       instance string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ExtensionMethod() /* 0600002E */
    IL_0035:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_003a:  /* 2A   |                  */ ret

    IL_003b:  /* 16   |                  */ ldc.i4.0
    IL_003c:  /* 2A   |                  */ ret
  } // end of method ThisAndExtensionMethod::Equals

  .method /*06000030*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2fa5
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 7E   | (0A)00002F       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/::Default /* 0A00002F */
    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 28   | (06)00002D       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ThisType() /* 0600002D */
    IL_000b:  /* 6F   | (0A)000032       */ callvirt   instance int32 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/::GetHashCode(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/) /* 0A000032 */
    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 28   | (06)00002E       */ call       instance string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ExtensionMethod() /* 0600002E */
    IL_0016:  /* 28   | (2B)000001       */ call       int32 HashCode/*02000009*/::Combine<int32,string>(!!0,
                                                                                                         !!1) /* 2B000001 */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method ThisAndExtensionMethod::GetHashCode

  .property /*17000003*/ instance callconv(8) class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/
          ThisType()
  {
    .get instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ThisType() /* 0600002D */
  } // end of property ThisAndExtensionMethod::ThisType
  .property /*17000004*/ instance callconv(8) string
          ExtensionMethod()
  {
    .get instance string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ExtensionMethod() /* 0600002E */
  } // end of property ThisAndExtensionMethod::ExtensionMethod
} // end of class Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod

.class /*0200000C*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer
       extends [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/
{
  .custom /*0C000040:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000042:0A000014*/ instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.ExportCodeFixProviderAttribute/*01000021*/::.ctor(string,
                                                                                                                                                                                string[]) /* 0A000014 */ = ( 01 00 02 43 23 00 00 00 00 01 00 54 0E 04 4E 61   // ...C#......T..Na
                                                                                                                                                                                                             6D 65 1A 57 65 62 41 70 70 6C 69 63 61 74 69 6F   // me.WebApplicatio
                                                                                                                                                                                                             6E 42 75 69 6C 64 65 72 46 69 78 65 72 )          // nBuilderFixer
  .custom /*0C000043:0A000015*/ instance void [System.Composition.AttributedModel/*23000005*/]System.Composition.SharedAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*02000016*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C00006C:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000021*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root
    .field /*04000022*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .method /*06000066*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4087
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_0'::.ctor

  } // end of class '<>c__DisplayClass4_0'

  .class /*02000017*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_1'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C00006F:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000023*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic
    .field /*04000024*/ public class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ invocation
    .field /*04000025*/ public class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ 'CS$<>8__locals1'
    .method /*06000067*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x408f
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::.ctor

    .method /*06000068*/ assembly hidebysig 
            instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
            '<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
    // SIG: 20 01 15 12 80 A9 01 12 80 AD 11 80 ED
    {
      // Method begins at RVA 0x4097
      // Code size       45 (0x2d)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000023       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::diagnostic /* 04000023 */
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 7B   | (04)000025       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::'CS$<>8__locals1' /* 04000025 */
      IL_000c:  /* 7B   | (04)000021       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::root /* 04000021 */
      IL_0011:  /* 02   |                  */ ldarg.0
      IL_0012:  /* 7B   | (04)000025       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::'CS$<>8__locals1' /* 04000025 */
      IL_0017:  /* 7C   | (04)000022       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::context /* 04000022 */
      IL_001c:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
      IL_0021:  /* 02   |                  */ ldarg.0
      IL_0022:  /* 7B   | (04)000024       */ ldfld      class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::invocation /* 04000024 */
      IL_0027:  /* 28   | (06)000034       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*/::FixWebApplicationBuilderAsync(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                                                                                                                                                                                      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                                                                                                                                      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                                                      class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/) /* 06000034 */
      IL_002c:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::'<RegisterCodeFixesAsync>b__0'

  } // end of class '<>c__DisplayClass4_1'

  .class /*02000018*/ auto ansi sealed nested private beforefieldinit '<RegisterCodeFixesAsync>d__4'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000072:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000026*/ public int32 '<>1__state'
    .field /*04000027*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ '<>t__builder'
    .field /*04000028*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .field /*04000029*/ private class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ '<>8__1'
    .field /*0400002A*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> '<>u__1'
    .method /*06000069*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x40c8
      // Code size       560 (0x230)
      .maxstack  4
      .locals /*11000026*/ init (int32 V_0,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_1,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_2,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_3,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_4,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_5,
               class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/ V_6,
               string V_7,
               string V_8,
               string V_9,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_10)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000026       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>1__state' /* 04000026 */
      IL_0006:  /* 0A   |                  */ stloc.0
      .try
      {
        IL_0007:  /* 06   |                  */ ldloc.0
        IL_0008:  /* 2C   | 7C               */ brfalse.s  IL_0086

        IL_000a:  /* 02   |                  */ ldarg.0
        IL_000b:  /* 73   | (06)000066       */ newobj     instance void Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::.ctor() /* 06000066 */
        IL_0010:  /* 7D   | (04)000029       */ stfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_0015:  /* 02   |                  */ ldarg.0
        IL_0016:  /* 7B   | (04)000029       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_001b:  /* 02   |                  */ ldarg.0
        IL_001c:  /* 7B   | (04)000028       */ ldfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::context /* 04000028 */
        IL_0021:  /* 7D   | (04)000022       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::context /* 04000022 */
        IL_0026:  /* 02   |                  */ ldarg.0
        IL_0027:  /* 7B   | (04)000029       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_002c:  /* 7C   | (04)000022       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::context /* 04000022 */
        IL_0031:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_0036:  /* 02   |                  */ ldarg.0
        IL_0037:  /* 7B   | (04)000029       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_003c:  /* 7C   | (04)000022       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::context /* 04000022 */
        IL_0041:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_0046:  /* 6F   | (0A)0000BB       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSyntaxRootAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000BB */
        IL_004b:  /* 16   |                  */ ldc.i4.0
        IL_004c:  /* 6F   | (0A)0000BC       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001B*/::ConfigureAwait(bool) /* 0A0000BC */
        IL_0051:  /* 0D   |                  */ stloc.3
        IL_0052:  /* 12   | 03               */ ldloca.s   V_3
        IL_0054:  /* 28   | (0A)0000BD       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001C*/::GetAwaiter() /* 0A0000BD */
        IL_0059:  /* 0C   |                  */ stloc.2
        IL_005a:  /* 12   | 02               */ ldloca.s   V_2
        IL_005c:  /* 28   | (0A)0000BE       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::get_IsCompleted() /* 0A0000BE */
        IL_0061:  /* 2D   | 3F               */ brtrue.s   IL_00a2

        IL_0063:  /* 02   |                  */ ldarg.0
        IL_0064:  /* 16   |                  */ ldc.i4.0
        IL_0065:  /* 25   |                  */ dup
        IL_0066:  /* 0A   |                  */ stloc.0
        IL_0067:  /* 7D   | (04)000026       */ stfld      int32 Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>1__state' /* 04000026 */
        IL_006c:  /* 02   |                  */ ldarg.0
        IL_006d:  /* 08   |                  */ ldloc.2
        IL_006e:  /* 7D   | (04)00002A       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>u__1' /* 0400002A */
        IL_0073:  /* 02   |                  */ ldarg.0
        IL_0074:  /* 7C   | (04)000027       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>t__builder' /* 04000027 */
        IL_0079:  /* 12   | 02               */ ldloca.s   V_2
        IL_007b:  /* 02   |                  */ ldarg.0
        IL_007c:  /* 28   | (2B)00001B       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>,valuetype Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          !!1&) /* 2B00001B */
        IL_0081:  /* DD   | A9010000         */ leave      IL_022f

        IL_0086:  /* 02   |                  */ ldarg.0
        IL_0087:  /* 7B   | (04)00002A       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>u__1' /* 0400002A */
        IL_008c:  /* 0C   |                  */ stloc.2
        IL_008d:  /* 02   |                  */ ldarg.0
        IL_008e:  /* 7C   | (04)00002A       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>u__1' /* 0400002A */
        IL_0093:  /* FE15 | (1B)00001D       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/
        IL_0099:  /* 02   |                  */ ldarg.0
        IL_009a:  /* 15   |                  */ ldc.i4.m1
        IL_009b:  /* 25   |                  */ dup
        IL_009c:  /* 0A   |                  */ stloc.0
        IL_009d:  /* 7D   | (04)000026       */ stfld      int32 Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>1__state' /* 04000026 */
        IL_00a2:  /* 12   | 02               */ ldloca.s   V_2
        IL_00a4:  /* 28   | (0A)0000C0       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::GetResult() /* 0A0000C0 */
        IL_00a9:  /* 0B   |                  */ stloc.1
        IL_00aa:  /* 02   |                  */ ldarg.0
        IL_00ab:  /* 7B   | (04)000029       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_00b0:  /* 07   |                  */ ldloc.1
        IL_00b1:  /* 7D   | (04)000021       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::root /* 04000021 */
        IL_00b6:  /* 02   |                  */ ldarg.0
        IL_00b7:  /* 7B   | (04)000029       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_00bc:  /* 7B   | (04)000021       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::root /* 04000021 */
        IL_00c1:  /* 2D   | 05               */ brtrue.s   IL_00c8

        IL_00c3:  /* DD   | 4D010000         */ leave      IL_0215

        IL_00c8:  /* 02   |                  */ ldarg.0
        IL_00c9:  /* 7B   | (04)000029       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_00ce:  /* 7C   | (04)000022       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::context /* 04000022 */
        IL_00d3:  /* 28   | (0A)000068       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Diagnostics() /* 0A000068 */
        IL_00d8:  /* 13   | 05               */ stloc.s    V_5
        IL_00da:  /* 12   | 05               */ ldloca.s   V_5
        IL_00dc:  /* 28   | (0A)000069       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<!0> valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000010*/::GetEnumerator() /* 0A000069 */
        IL_00e1:  /* 13   | 04               */ stloc.s    V_4
        IL_00e3:  /* 38   | FF000000         */ br         IL_01e7

        IL_00e8:  /* 73   | (06)000067       */ newobj     instance void Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::.ctor() /* 06000067 */
        IL_00ed:  /* 13   | 06               */ stloc.s    V_6
        IL_00ef:  /* 11   | 06               */ ldloc.s    V_6
        IL_00f1:  /* 02   |                  */ ldarg.0
        IL_00f2:  /* 7B   | (04)000029       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_00f7:  /* 7D   | (04)000025       */ stfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::'CS$<>8__locals1' /* 04000025 */
        IL_00fc:  /* 11   | 06               */ ldloc.s    V_6
        IL_00fe:  /* 12   | 04               */ ldloca.s   V_4
        IL_0100:  /* 28   | (0A)00006A       */ call       instance !0 valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::get_Current() /* 0A00006A */
        IL_0105:  /* 7D   | (04)000023       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::diagnostic /* 04000023 */
        IL_010a:  /* 11   | 06               */ ldloc.s    V_6
        IL_010c:  /* 7B   | (04)000023       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::diagnostic /* 04000023 */
        IL_0111:  /* 6F   | (0A)0000C1       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Id() /* 0A0000C1 */
        IL_0116:  /* 13   | 07               */ stloc.s    V_7
        IL_0118:  /* 7E   | (0A)0000C2       */ ldsfld     string [netstandard/*23000001*/]System.String/*0100006E*/::Empty /* 0A0000C2 */
        IL_011d:  /* 13   | 08               */ stloc.s    V_8
        IL_011f:  /* 7E   | (0A)0000C2       */ ldsfld     string [netstandard/*23000001*/]System.String/*0100006E*/::Empty /* 0A0000C2 */
        IL_0124:  /* 13   | 09               */ stloc.s    V_9
        IL_0126:  /* 11   | 07               */ ldloc.s    V_7
        IL_0128:  /* 2C   | 67               */ brfalse.s  IL_0191

        IL_012a:  /* 11   | 07               */ ldloc.s    V_7
        IL_012c:  /* 7E   | (0A)000053       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DoNotUseHostConfigureLogging /* 0A000053 */
        IL_0131:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
        IL_0136:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                                string) /* 0A000031 */
        IL_013b:  /* 2C   | 10               */ brfalse.s  IL_014d

        IL_013d:  /* 72   | (70)0006DF       */ ldstr      "Fix references to Logging properties on WebApplica"
        + "tionBuilder" /* 700006DF */
        IL_0142:  /* 13   | 08               */ stloc.s    V_8
        IL_0144:  /* 72   | (70)00075B       */ ldstr      "Logging" /* 7000075B */
        IL_0149:  /* 13   | 09               */ stloc.s    V_9
        IL_014b:  /* 2B   | 44               */ br.s       IL_0191

        IL_014d:  /* 11   | 07               */ ldloc.s    V_7
        IL_014f:  /* 7E   | (0A)000055       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DoNotUseHostConfigureServices /* 0A000055 */
        IL_0154:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
        IL_0159:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                                string) /* 0A000031 */
        IL_015e:  /* 2C   | 10               */ brfalse.s  IL_0170

        IL_0160:  /* 72   | (70)00076B       */ ldstr      "Fix references to Services properties on WebApplic"
        + "ationBuilder" /* 7000076B */
        IL_0165:  /* 13   | 08               */ stloc.s    V_8
        IL_0167:  /* 72   | (70)0007E9       */ ldstr      "Services" /* 700007E9 */
        IL_016c:  /* 13   | 09               */ stloc.s    V_9
        IL_016e:  /* 2B   | 21               */ br.s       IL_0191

        IL_0170:  /* 11   | 07               */ ldloc.s    V_7
        IL_0172:  /* 7E   | (0A)000056       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DisallowConfigureAppConfigureHostBuilder /* 0A000056 */
        IL_0177:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
        IL_017c:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                                string) /* 0A000031 */
        IL_0181:  /* 2C   | 0E               */ brfalse.s  IL_0191

        IL_0183:  /* 72   | (70)0007FB       */ ldstr      "Fix references to Configuration properties on WebA"
        + "pplicationBuilder" /* 700007FB */
        IL_0188:  /* 13   | 08               */ stloc.s    V_8
        IL_018a:  /* 72   | (70)000884       */ ldstr      "Configuration" /* 70000884 */
        IL_018f:  /* 13   | 09               */ stloc.s    V_9
        IL_0191:  /* 11   | 06               */ ldloc.s    V_6
        IL_0193:  /* 7B   | (04)000023       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::diagnostic /* 04000023 */
        IL_0198:  /* 11   | 09               */ ldloc.s    V_9
        IL_019a:  /* 28   | (0A)0000B4       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax/*0100002F*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::IdentifierName(string) /* 0A0000B4 */
        IL_019f:  /* 11   | 06               */ ldloc.s    V_6
        IL_01a1:  /* 7B   | (04)000025       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::'CS$<>8__locals1' /* 04000025 */
        IL_01a6:  /* 7B   | (04)000021       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::root /* 04000021 */
        IL_01ab:  /* 11   | 06               */ ldloc.s    V_6
        IL_01ad:  /* 7C   | (04)000024       */ ldflda     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::invocation /* 04000024 */
        IL_01b2:  /* 28   | (06)000035       */ call       bool Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*/::CanFixWebApplicationBuilder(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax/*0100002F*/,
                                                                                                                                                                                                class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/&) /* 06000035 */
        IL_01b7:  /* 2C   | 2E               */ brfalse.s  IL_01e7

        IL_01b9:  /* 11   | 06               */ ldloc.s    V_6
        IL_01bb:  /* 7B   | (04)000025       */ ldfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::'CS$<>8__locals1' /* 04000025 */
        IL_01c0:  /* 7C   | (04)000022       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/::context /* 04000022 */
        IL_01c5:  /* 11   | 08               */ ldloc.s    V_8
        IL_01c7:  /* 11   | 06               */ ldloc.s    V_6
        IL_01c9:  /* FE06 | (06)000068       */ ldftn      instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::'<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 06000068 */
        IL_01cf:  /* 73   | (0A)00006E       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>/*1B000013*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                                                                   native int) /* 0A00006E */
        IL_01d4:  /* 11   | 07               */ ldloc.s    V_7
        IL_01d6:  /* 28   | (0A)00006F       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/::Create(string,
                                                                                                                                                                                                                                                                                           class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>,
                                                                                                                                                                                                                                                                                           string) /* 0A00006F */
        IL_01db:  /* 11   | 06               */ ldloc.s    V_6
        IL_01dd:  /* 7B   | (04)000023       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_1'/*02000017*/::diagnostic /* 04000023 */
        IL_01e2:  /* 28   | (0A)000070       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::RegisterCodeFix(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/,
                                                                                                                                                                                                     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/) /* 0A000070 */
        IL_01e7:  /* 12   | 04               */ ldloca.s   V_4
        IL_01e9:  /* 28   | (0A)000071       */ call       instance bool valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::MoveNext() /* 0A000071 */
        IL_01ee:  /* 3A   | F5FEFFFF         */ brtrue     IL_00e8

        IL_01f3:  /* DE   | 20               */ leave.s    IL_0215

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_01f5:  /* 13   | 0A               */ stloc.s    V_10
        IL_01f7:  /* 02   |                  */ ldarg.0
        IL_01f8:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_01fa:  /* 7D   | (04)000026       */ stfld      int32 Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>1__state' /* 04000026 */
        IL_01ff:  /* 02   |                  */ ldarg.0
        IL_0200:  /* 14   |                  */ ldnull
        IL_0201:  /* 7D   | (04)000029       */ stfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
        IL_0206:  /* 02   |                  */ ldarg.0
        IL_0207:  /* 7C   | (04)000027       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>t__builder' /* 04000027 */
        IL_020c:  /* 11   | 0A               */ ldloc.s    V_10
        IL_020e:  /* 28   | (0A)0000C3       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000C3 */
        IL_0213:  /* DE   | 1A               */ leave.s    IL_022f

      }  // end handler
      // HEX: 00 00 00 00 07 00 00 00 EE 01 00 00 F5 01 00 00 20 00 00 00 40 00 00 01
      IL_0215:  /* 02   |                  */ ldarg.0
      IL_0216:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_0218:  /* 7D   | (04)000026       */ stfld      int32 Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>1__state' /* 04000026 */
      IL_021d:  /* 02   |                  */ ldarg.0
      IL_021e:  /* 14   |                  */ ldnull
      IL_021f:  /* 7D   | (04)000029       */ stfld      class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<>c__DisplayClass4_0'/*02000016*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>8__1' /* 04000029 */
      IL_0224:  /* 02   |                  */ ldarg.0
      IL_0225:  /* 7C   | (04)000027       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>t__builder' /* 04000027 */
      IL_022a:  /* 28   | (0A)0000C4       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetResult() /* 0A0000C4 */
      IL_022f:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__4'::MoveNext

    .method /*0600006A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000D1:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x4320
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)000027       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>t__builder' /* 04000027 */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000C5       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000C5 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__4'::SetStateMachine

  } // end of class '<RegisterCodeFixesAsync>d__4'

  .field /*04000017*/ private initonly valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> '<FixableDiagnosticIds>k__BackingField'
  .custom /*0C00006E:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*06000031*/ public hidebysig specialname virtual 
          instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> 
          get_FixableDiagnosticIds() cil managed
  // SIG: 20 00 15 11 80 91 01 0E
  {
    .custom /*0C0000B8:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .param [0]/*08000048*/ 
    .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2fc2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000017       */ ldfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*/::'<FixableDiagnosticIds>k__BackingField' /* 04000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebApplicationBuilderFixer::get_FixableDiagnosticIds

  .method /*06000032*/ public hidebysig virtual final 
          instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ 
          GetFixAllProvider() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x2fca
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 28   | (0A)000033       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders/*0100006F*/::get_BatchFixer() /* 0A000033 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method WebApplicationBuilderFixer::GetFixAllProvider

  .method /*06000033*/ public hidebysig virtual final 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          RegisterCodeFixesAsync(valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context) cil managed
  // SIG: 20 01 12 80 A1 11 80 A5
  {
    .custom /*0C0000B9:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 73 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..sMicrosoft.Asp
                                                                                                                                                                                                                                  4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65 72   // NetCore.Analyzer
                                                                                                                                                                                                                                  73 2E 57 65 62 41 70 70 6C 69 63 61 74 69 6F 6E   // s.WebApplication
                                                                                                                                                                                                                                  42 75 69 6C 64 65 72 2E 46 69 78 65 72 73 2E 57   // Builder.Fixers.W
                                                                                                                                                                                                                                  65 62 41 70 70 6C 69 63 61 74 69 6F 6E 42 75 69   // ebApplicationBui
                                                                                                                                                                                                                                  6C 64 65 72 46 69 78 65 72 2B 3C 52 65 67 69 73   // lderFixer+<Regis
                                                                                                                                                                                                                                  74 65 72 43 6F 64 65 46 69 78 65 73 41 73 79 6E   // terCodeFixesAsyn
                                                                                                                                                                                                                                  63 3E 64 5F 5F 34 00 00 )                         // c>d__4..
    // Method begins at RVA 0x2fd4
    // Code size       55 (0x37)
    .maxstack  2
    .locals /*1100000F*/ init (valuetype Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)000034       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Create() /* 0A000034 */
    IL_0007:  /* 7D   | (04)000027       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>t__builder' /* 04000027 */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 7D   | (04)000028       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::context /* 04000028 */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 15   |                  */ ldc.i4.m1
    IL_0017:  /* 7D   | (04)000026       */ stfld      int32 Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>1__state' /* 04000026 */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 7C   | (04)000027       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>t__builder' /* 04000027 */
    IL_0023:  /* 12   | 00               */ ldloca.s   V_0
    IL_0025:  /* 28   | (2B)000002       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Start<valuetype Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/>(!!0&) /* 2B000002 */
    IL_002a:  /* 12   | 00               */ ldloca.s   V_0
    IL_002c:  /* 7C   | (04)000027       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*//'<RegisterCodeFixesAsync>d__4'/*02000018*/::'<>t__builder' /* 04000027 */
    IL_0031:  /* 28   | (0A)000036       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::get_Task() /* 0A000036 */
    IL_0036:  /* 2A   |                  */ ret
  } // end of method WebApplicationBuilderFixer::RegisterCodeFixesAsync

  .method /*06000034*/ private hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
          FixWebApplicationBuilderAsync(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                        class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                                        class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                                        class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ invocation) cil managed
  // SIG: 00 04 15 12 80 A9 01 12 80 AD 12 80 B1 12 80 B5 12 80 AD 12 80 B9
  {
    // Method begins at RVA 0x3018
    // Code size       40 (0x28)
    .maxstack  4
    .locals /*11000010*/ init (class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_0007:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_000c:  /* 16   |                  */ ldc.i4.0
    IL_000d:  /* 17   |                  */ ldc.i4.1
    IL_000e:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_0013:  /* 0A   |                  */ stloc.0
    IL_0014:  /* 04   |                  */ ldarg.2
    IL_0015:  /* 03   |                  */ ldarg.1
    IL_0016:  /* 06   |                  */ ldloc.0
    IL_0017:  /* 05   |                  */ ldarg.3
    IL_0018:  /* 28   | (2B)000003       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceNode<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>(!!0,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 2B000003 */
    IL_001d:  /* 6F   | (0A)00003B       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::WithSyntaxRoot(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 0A00003B */
    IL_0022:  /* 28   | (2B)000004       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!!0> [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/::FromResult<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>(!!0) /* 2B000004 */
    IL_0027:  /* 2A   |                  */ ret
  } // end of method WebApplicationBuilderFixer::FixWebApplicationBuilderAsync

  .method /*06000035*/ private hidebysig static 
          bool  CanFixWebApplicationBuilder(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                            class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax/*0100002F*/ identifierMethod,
                                            class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                                            [out] class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/& invocationName) cil managed
  // SIG: 00 04 02 12 80 B1 12 80 BD 12 80 B5 10 12 80 B9
  {
    .param [4]/*08000051*/ 
    .custom /*0C0000C6:0A000017*/ instance void [Microsoft.AspNetCore.App.Analyzers/*23000002*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x304c
    // Code size       403 (0x193)
    .maxstack  4
    .locals /*11000011*/ init (class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ V_0,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_1,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_2,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/ V_3,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/ V_4,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.LambdaExpressionSyntax/*01000034*/ V_5,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxList`1/*01000035*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax/*01000036*/> V_6,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxList`1/*01000035*//Enumerator/*01000037*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax/*01000036*/> V_7,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionStatementSyntax/*01000038*/ V_8,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ V_9,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ V_10,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_11,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ V_12,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ V_13,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ V_14,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_15,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ V_16)
    IL_0000:  /* 05   |                  */ ldarg.3
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 51   |                  */ stind.ref
    IL_0003:  /* 04   |                  */ ldarg.2
    IL_0004:  /* 2D   | 02               */ brtrue.s   IL_0008

    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 2A   |                  */ ret

    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 02   |                  */ ldarg.0
    IL_000a:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_000f:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_0014:  /* 16   |                  */ ldc.i4.0
    IL_0015:  /* 17   |                  */ ldc.i4.1
    IL_0016:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_001b:  /* 75   | (01)00002E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
    IL_0020:  /* 0A   |                  */ stloc.0
    IL_0021:  /* 06   |                  */ ldloc.0
    IL_0022:  /* 39   | 6A010000         */ brfalse    IL_0191

    IL_0027:  /* 06   |                  */ ldloc.0
    IL_0028:  /* 6F   | (0A)00003D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_Expression() /* 0A00003D */
    IL_002d:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
    IL_0032:  /* 0B   |                  */ stloc.1
    IL_0033:  /* 07   |                  */ ldloc.1
    IL_0034:  /* 2C   | 0F               */ brfalse.s  IL_0045

    IL_0036:  /* 07   |                  */ ldloc.1
    IL_0037:  /* 6F   | (0A)00003E       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Expression() /* 0A00003E */
    IL_003c:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
    IL_0041:  /* 0C   |                  */ stloc.2
    IL_0042:  /* 08   |                  */ ldloc.2
    IL_0043:  /* 2D   | 02               */ brtrue.s   IL_0047

    IL_0045:  /* 16   |                  */ ldc.i4.0
    IL_0046:  /* 2A   |                  */ ret

    IL_0047:  /* 08   |                  */ ldloc.2
    IL_0048:  /* 03   |                  */ ldarg.1
    IL_0049:  /* 6F   | (0A)00003F       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::WithName(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/) /* 0A00003F */
    IL_004e:  /* 0C   |                  */ stloc.2
    IL_004f:  /* 07   |                  */ ldloc.1
    IL_0050:  /* 6F   | (0A)000040       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode/*01000072*/::GetLeadingTrivia() /* 0A000040 */
    IL_0055:  /* 0D   |                  */ stloc.3
    IL_0056:  /* 07   |                  */ ldloc.1
    IL_0057:  /* 08   |                  */ ldloc.2
    IL_0058:  /* 6F   | (0A)000041       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::WithExpression(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/) /* 0A000041 */
    IL_005d:  /* 72   | (70)000173       */ ldstr      "    " /* 70000173 */
    IL_0062:  /* 72   | (70)00017D       */ ldstr      "\r\n" /* 7000017D */
    IL_0067:  /* 16   |                  */ ldc.i4.0
    IL_0068:  /* 28   | (2B)000005       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::NormalizeWhitespace<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/>(!!0,
                                                                                                                                                                                                                                                                                                            string,
                                                                                                                                                                                                                                                                                                            string,
                                                                                                                                                                                                                                                                                                            bool) /* 2B000005 */
    IL_006d:  /* 09   |                  */ ldloc.3
    IL_006e:  /* 28   | (2B)000006       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::WithLeadingTrivia<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/>(!!0,
                                                                                                                                                                                                                                                                                                          valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/) /* 2B000006 */
    IL_0073:  /* 0B   |                  */ stloc.1
    IL_0074:  /* 06   |                  */ ldloc.0
    IL_0075:  /* 6F   | (0A)000044       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_ArgumentList() /* 0A000044 */
    IL_007a:  /* 6F   | (0A)000045       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax/*01000073*/::get_Arguments() /* 0A000045 */
    IL_007f:  /* 8C   | (1B)00000D       */ box        valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>/*1B00000D*/
    IL_0084:  /* 28   | (2B)000007       */ call       !!0 [netstandard/*23000001*/]System.Linq.Enumerable/*01000074*/::SingleOrDefault<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<!!0>) /* 2B000007 */
    IL_0089:  /* 13   | 04               */ stloc.s    V_4
    IL_008b:  /* 11   | 04               */ ldloc.s    V_4
    IL_008d:  /* 2C   | 12               */ brfalse.s  IL_00a1

    IL_008f:  /* 11   | 04               */ ldloc.s    V_4
    IL_0091:  /* 6F   | (0A)000047       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/::get_Expression() /* 0A000047 */
    IL_0096:  /* 75   | (01)000034       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.LambdaExpressionSyntax/*01000034*/
    IL_009b:  /* 13   | 05               */ stloc.s    V_5
    IL_009d:  /* 11   | 05               */ ldloc.s    V_5
    IL_009f:  /* 2D   | 02               */ brtrue.s   IL_00a3

    IL_00a1:  /* 16   |                  */ ldc.i4.0
    IL_00a2:  /* 2A   |                  */ ret

    IL_00a3:  /* 11   | 05               */ ldloc.s    V_5
    IL_00a5:  /* 6F   | (0A)000048       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax/*01000064*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax/*01000075*/::get_Block() /* 0A000048 */
    IL_00aa:  /* 39   | 8B000000         */ brfalse    IL_013a

    IL_00af:  /* 11   | 05               */ ldloc.s    V_5
    IL_00b1:  /* 6F   | (0A)000048       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax/*01000064*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax/*01000075*/::get_Block() /* 0A000048 */
    IL_00b6:  /* 6F   | (0A)000049       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxList`1/*01000035*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax/*01000036*/> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax/*01000064*/::get_Statements() /* 0A000049 */
    IL_00bb:  /* 13   | 06               */ stloc.s    V_6
    IL_00bd:  /* 12   | 06               */ ldloca.s   V_6
    IL_00bf:  /* 28   | (0A)00004A       */ call       instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxList`1/*01000035*//Enumerator/*01000037*/<!0> valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxList`1/*01000035*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax/*01000036*/>/*1B00000E*/::GetEnumerator() /* 0A00004A */
    IL_00c4:  /* 13   | 07               */ stloc.s    V_7
    IL_00c6:  /* 2B   | 67               */ br.s       IL_012f

    IL_00c8:  /* 12   | 07               */ ldloca.s   V_7
    IL_00ca:  /* 28   | (0A)00004B       */ call       instance !0 valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxList`1/*01000035*//Enumerator/*01000037*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax/*01000036*/>/*1B00000F*/::get_Current() /* 0A00004B */
    IL_00cf:  /* 75   | (01)000038       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionStatementSyntax/*01000038*/
    IL_00d4:  /* 13   | 08               */ stloc.s    V_8
    IL_00d6:  /* 11   | 08               */ ldloc.s    V_8
    IL_00d8:  /* 2C   | 12               */ brfalse.s  IL_00ec

    IL_00da:  /* 11   | 08               */ ldloc.s    V_8
    IL_00dc:  /* 6F   | (0A)00004C       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionStatementSyntax/*01000038*/::get_Expression() /* 0A00004C */
    IL_00e1:  /* 75   | (01)00002E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
    IL_00e6:  /* 13   | 09               */ stloc.s    V_9
    IL_00e8:  /* 11   | 09               */ ldloc.s    V_9
    IL_00ea:  /* 2D   | 02               */ brtrue.s   IL_00ee

    IL_00ec:  /* 16   |                  */ ldc.i4.0
    IL_00ed:  /* 2A   |                  */ ret

    IL_00ee:  /* 11   | 09               */ ldloc.s    V_9
    IL_00f0:  /* 6F   | (0A)000044       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_ArgumentList() /* 0A000044 */
    IL_00f5:  /* 13   | 0A               */ stloc.s    V_10
    IL_00f7:  /* 11   | 09               */ ldloc.s    V_9
    IL_00f9:  /* 6F   | (0A)00003D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_Expression() /* 0A00003D */
    IL_00fe:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
    IL_0103:  /* 13   | 0B               */ stloc.s    V_11
    IL_0105:  /* 11   | 0B               */ ldloc.s    V_11
    IL_0107:  /* 2D   | 02               */ brtrue.s   IL_010b

    IL_0109:  /* 16   |                  */ ldc.i4.0
    IL_010a:  /* 2A   |                  */ ret

    IL_010b:  /* 11   | 0B               */ ldloc.s    V_11
    IL_010d:  /* 6F   | (0A)00004D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Name() /* 0A00004D */
    IL_0112:  /* 13   | 0C               */ stloc.s    V_12
    IL_0114:  /* 07   |                  */ ldloc.1
    IL_0115:  /* 11   | 0C               */ ldloc.s    V_12
    IL_0117:  /* 6F   | (0A)00003F       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::WithName(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/) /* 0A00003F */
    IL_011c:  /* 0B   |                  */ stloc.1
    IL_011d:  /* 06   |                  */ ldloc.0
    IL_011e:  /* 07   |                  */ ldloc.1
    IL_011f:  /* 11   | 0A               */ ldloc.s    V_10
    IL_0121:  /* 6F   | (0A)00004E       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::Update(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/,
                                                                                                                                                                                                                                                                                                                            class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/) /* 0A00004E */
    IL_0126:  /* 0A   |                  */ stloc.0
    IL_0127:  /* 07   |                  */ ldloc.1
    IL_0128:  /* 06   |                  */ ldloc.0
    IL_0129:  /* 6F   | (0A)000041       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::WithExpression(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/) /* 0A000041 */
    IL_012e:  /* 0B   |                  */ stloc.1
    IL_012f:  /* 12   | 07               */ ldloca.s   V_7
    IL_0131:  /* 28   | (0A)00004F       */ call       instance bool valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxList`1/*01000035*//Enumerator/*01000037*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax/*01000036*/>/*1B00000F*/::MoveNext() /* 0A00004F */
    IL_0136:  /* 2D   | 90               */ brtrue.s   IL_00c8

    IL_0138:  /* 2B   | 52               */ br.s       IL_018c

    IL_013a:  /* 11   | 05               */ ldloc.s    V_5
    IL_013c:  /* 6F   | (0A)000050       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax/*01000075*/::get_ExpressionBody() /* 0A000050 */
    IL_0141:  /* 75   | (01)00002E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
    IL_0146:  /* 13   | 0D               */ stloc.s    V_13
    IL_0148:  /* 11   | 0D               */ ldloc.s    V_13
    IL_014a:  /* 2D   | 02               */ brtrue.s   IL_014e

    IL_014c:  /* 16   |                  */ ldc.i4.0
    IL_014d:  /* 2A   |                  */ ret

    IL_014e:  /* 11   | 0D               */ ldloc.s    V_13
    IL_0150:  /* 6F   | (0A)000044       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_ArgumentList() /* 0A000044 */
    IL_0155:  /* 13   | 0E               */ stloc.s    V_14
    IL_0157:  /* 11   | 0D               */ ldloc.s    V_13
    IL_0159:  /* 6F   | (0A)00003D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_Expression() /* 0A00003D */
    IL_015e:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
    IL_0163:  /* 13   | 0F               */ stloc.s    V_15
    IL_0165:  /* 11   | 0F               */ ldloc.s    V_15
    IL_0167:  /* 2D   | 02               */ brtrue.s   IL_016b

    IL_0169:  /* 16   |                  */ ldc.i4.0
    IL_016a:  /* 2A   |                  */ ret

    IL_016b:  /* 11   | 0F               */ ldloc.s    V_15
    IL_016d:  /* 6F   | (0A)00004D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Name() /* 0A00004D */
    IL_0172:  /* 13   | 10               */ stloc.s    V_16
    IL_0174:  /* 07   |                  */ ldloc.1
    IL_0175:  /* 11   | 10               */ ldloc.s    V_16
    IL_0177:  /* 6F   | (0A)00003F       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::WithName(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/) /* 0A00003F */
    IL_017c:  /* 0B   |                  */ stloc.1
    IL_017d:  /* 06   |                  */ ldloc.0
    IL_017e:  /* 07   |                  */ ldloc.1
    IL_017f:  /* 6F   | (0A)000051       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::WithExpression(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/) /* 0A000051 */
    IL_0184:  /* 11   | 0E               */ ldloc.s    V_14
    IL_0186:  /* 6F   | (0A)000052       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::WithArgumentList(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/) /* 0A000052 */
    IL_018b:  /* 0A   |                  */ stloc.0
    IL_018c:  /* 05   |                  */ ldarg.3
    IL_018d:  /* 06   |                  */ ldloc.0
    IL_018e:  /* 51   |                  */ stind.ref
    IL_018f:  /* 17   |                  */ ldc.i4.1
    IL_0190:  /* 2A   |                  */ ret

    IL_0191:  /* 16   |                  */ ldc.i4.0
    IL_0192:  /* 2A   |                  */ ret
  } // end of method WebApplicationBuilderFixer::CanFixWebApplicationBuilder

  .method /*06000036*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x31eb
    // Code size       48 (0x30)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (0A)000053       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DoNotUseHostConfigureLogging /* 0A000053 */
    IL_0006:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_000b:  /* 7E   | (0A)000055       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DoNotUseHostConfigureServices /* 0A000055 */
    IL_0010:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_0015:  /* 7E   | (0A)000056       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DisallowConfigureAppConfigureHostBuilder /* 0A000056 */
    IL_001a:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_001f:  /* 28   | (2B)000008       */ call       valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<!!0> [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray/*01000078*/::Create<string>(!!0,
                                                                                                                                                                                                                                                                                            !!0,
                                                                                                                                                                                                                                                                                            !!0) /* 2B000008 */
    IL_0024:  /* 7D   | (04)000017       */ stfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*/::'<FixableDiagnosticIds>k__BackingField' /* 04000017 */
    IL_0029:  /* 02   |                  */ ldarg.0
    IL_002a:  /* 28   | (0A)000058       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/::.ctor() /* 0A000058 */
    IL_002f:  /* 2A   |                  */ ret
  } // end of method WebApplicationBuilderFixer::.ctor

  .property /*17000005*/ instance callconv(8) valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>
          FixableDiagnosticIds()
  {
    .custom /*0C000024:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer/*0200000C*/::get_FixableDiagnosticIds() /* 06000031 */
  } // end of property WebApplicationBuilderFixer::FixableDiagnosticIds
} // end of class Microsoft.AspNetCore.Analyzers.WebApplicationBuilder.Fixers.WebApplicationBuilderFixer

.class /*0200000D*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer
       extends [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/
{
  .custom /*0C000045:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000046:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000047:0A000014*/ instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.ExportCodeFixProviderAttribute/*01000021*/::.ctor(string,
                                                                                                                                                                                string[]) /* 0A000014 */ = ( 01 00 02 43 23 00 00 00 00 00 00 )                // ...C#......
  .custom /*0C000048:0A000015*/ instance void [System.Composition.AttributedModel/*23000005*/]System.Composition.SharedAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*02000019*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C000075:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*0400002B*/ public class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ wellKnownTypes
    .field /*0400002C*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root
    .field /*0400002D*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .method /*0600006B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x432e
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_0'::.ctor

  } // end of class '<>c__DisplayClass4_0'

  .class /*0200001A*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_1'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C000078:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*0400002E*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic
    .field /*0400002F*/ public class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ invocation
    .field /*04000030*/ public class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/ assignment
    .field /*04000031*/ public class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ 'CS$<>8__locals1'
    .method /*0600006C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4336
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::.ctor

    .method /*0600006D*/ assembly hidebysig 
            instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
            '<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
    // SIG: 20 01 15 12 80 A9 01 12 80 AD 11 80 ED
    {
      // Method begins at RVA 0x433e
      // Code size       56 (0x38)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)00002E       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::diagnostic /* 0400002E */
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
      IL_000c:  /* 7B   | (04)00002B       */ ldfld      class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::wellKnownTypes /* 0400002B */
      IL_0011:  /* 02   |                  */ ldarg.0
      IL_0012:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
      IL_0017:  /* 7B   | (04)00002C       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::root /* 0400002C */
      IL_001c:  /* 02   |                  */ ldarg.0
      IL_001d:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
      IL_0022:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
      IL_0027:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
      IL_002c:  /* 02   |                  */ ldarg.0
      IL_002d:  /* 7B   | (04)00002F       */ ldfld      class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::invocation /* 0400002F */
      IL_0032:  /* 28   | (06)00003A       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*/::ReplaceWithAppend(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                                                                                                                                                       class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/,
                                                                                                                                                                                                                                                                                                                                       class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                                                                                                       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/) /* 0600003A */
      IL_0037:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::'<RegisterCodeFixesAsync>b__0'

    .method /*0600006E*/ assembly hidebysig 
            instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
            '<RegisterCodeFixesAsync>b__1'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
    // SIG: 20 01 15 12 80 A9 01 12 80 AD 11 80 ED
    {
      // Method begins at RVA 0x4377
      // Code size       45 (0x2d)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)00002E       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::diagnostic /* 0400002E */
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
      IL_000c:  /* 7B   | (04)00002C       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::root /* 0400002C */
      IL_0011:  /* 02   |                  */ ldarg.0
      IL_0012:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
      IL_0017:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
      IL_001c:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
      IL_0021:  /* 02   |                  */ ldarg.0
      IL_0022:  /* 7B   | (04)000030       */ ldfld      class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::assignment /* 04000030 */
      IL_0027:  /* 28   | (06)00003C       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*/::ReplaceWithIndexer(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                                                                                                                                                        class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                                                                                                        class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                        class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/) /* 0600003C */
      IL_002c:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::'<RegisterCodeFixesAsync>b__1'

  } // end of class '<>c__DisplayClass4_1'

  .class /*0200001B*/ auto ansi sealed nested private beforefieldinit '<RegisterCodeFixesAsync>d__4'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C00007A:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000032*/ public int32 '<>1__state'
    .field /*04000033*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ '<>t__builder'
    .field /*04000034*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .field /*04000035*/ private class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ '<>8__1'
    .field /*04000036*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> '<>u__1'
    .field /*04000037*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> '<>u__2'
    .method /*0600006F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x43a8
      // Code size       688 (0x2b0)
      .maxstack  4
      .locals /*11000027*/ init (int32 V_0,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ V_1,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_2,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_3,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_4,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_5,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_6,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_7,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_8,
               class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/ V_9,
               string V_10,
               string V_11,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_12)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000032       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>1__state' /* 04000032 */
      IL_0006:  /* 0A   |                  */ stloc.0
      .try
      {
        IL_0007:  /* 06   |                  */ ldloc.0
        IL_0008:  /* 39   | 84000000         */ brfalse    IL_0091

        IL_000d:  /* 06   |                  */ ldloc.0
        IL_000e:  /* 17   |                  */ ldc.i4.1
        IL_000f:  /* 3B   | 22010000         */ beq        IL_0136

        IL_0014:  /* 02   |                  */ ldarg.0
        IL_0015:  /* 73   | (06)00006B       */ newobj     instance void Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::.ctor() /* 0600006B */
        IL_001a:  /* 7D   | (04)000035       */ stfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_001f:  /* 02   |                  */ ldarg.0
        IL_0020:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_0025:  /* 02   |                  */ ldarg.0
        IL_0026:  /* 7B   | (04)000034       */ ldfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::context /* 04000034 */
        IL_002b:  /* 7D   | (04)00002D       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
        IL_0030:  /* 02   |                  */ ldarg.0
        IL_0031:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_0036:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
        IL_003b:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_0040:  /* 02   |                  */ ldarg.0
        IL_0041:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_0046:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
        IL_004b:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_0050:  /* 6F   | (0A)0000BB       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSyntaxRootAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000BB */
        IL_0055:  /* 16   |                  */ ldc.i4.0
        IL_0056:  /* 6F   | (0A)0000BC       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001B*/::ConfigureAwait(bool) /* 0A0000BC */
        IL_005b:  /* 13   | 04               */ stloc.s    V_4
        IL_005d:  /* 12   | 04               */ ldloca.s   V_4
        IL_005f:  /* 28   | (0A)0000BD       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001C*/::GetAwaiter() /* 0A0000BD */
        IL_0064:  /* 0D   |                  */ stloc.3
        IL_0065:  /* 12   | 03               */ ldloca.s   V_3
        IL_0067:  /* 28   | (0A)0000BE       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::get_IsCompleted() /* 0A0000BE */
        IL_006c:  /* 2D   | 3F               */ brtrue.s   IL_00ad

        IL_006e:  /* 02   |                  */ ldarg.0
        IL_006f:  /* 16   |                  */ ldc.i4.0
        IL_0070:  /* 25   |                  */ dup
        IL_0071:  /* 0A   |                  */ stloc.0
        IL_0072:  /* 7D   | (04)000032       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>1__state' /* 04000032 */
        IL_0077:  /* 02   |                  */ ldarg.0
        IL_0078:  /* 09   |                  */ ldloc.3
        IL_0079:  /* 7D   | (04)000036       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>u__1' /* 04000036 */
        IL_007e:  /* 02   |                  */ ldarg.0
        IL_007f:  /* 7C   | (04)000033       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>t__builder' /* 04000033 */
        IL_0084:  /* 12   | 03               */ ldloca.s   V_3
        IL_0086:  /* 02   |                  */ ldarg.0
        IL_0087:  /* 28   | (2B)00001C       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>,valuetype Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       !!1&) /* 2B00001C */
        IL_008c:  /* DD   | 1E020000         */ leave      IL_02af

        IL_0091:  /* 02   |                  */ ldarg.0
        IL_0092:  /* 7B   | (04)000036       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>u__1' /* 04000036 */
        IL_0097:  /* 0D   |                  */ stloc.3
        IL_0098:  /* 02   |                  */ ldarg.0
        IL_0099:  /* 7C   | (04)000036       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>u__1' /* 04000036 */
        IL_009e:  /* FE15 | (1B)00001D       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/
        IL_00a4:  /* 02   |                  */ ldarg.0
        IL_00a5:  /* 15   |                  */ ldc.i4.m1
        IL_00a6:  /* 25   |                  */ dup
        IL_00a7:  /* 0A   |                  */ stloc.0
        IL_00a8:  /* 7D   | (04)000032       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>1__state' /* 04000032 */
        IL_00ad:  /* 12   | 03               */ ldloca.s   V_3
        IL_00af:  /* 28   | (0A)0000C0       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::GetResult() /* 0A0000C0 */
        IL_00b4:  /* 0C   |                  */ stloc.2
        IL_00b5:  /* 02   |                  */ ldarg.0
        IL_00b6:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_00bb:  /* 08   |                  */ ldloc.2
        IL_00bc:  /* 7D   | (04)00002C       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::root /* 0400002C */
        IL_00c1:  /* 02   |                  */ ldarg.0
        IL_00c2:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_00c7:  /* 7B   | (04)00002C       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::root /* 0400002C */
        IL_00cc:  /* 2D   | 05               */ brtrue.s   IL_00d3

        IL_00ce:  /* DD   | C2010000         */ leave      IL_0295

        IL_00d3:  /* 02   |                  */ ldarg.0
        IL_00d4:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_00d9:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
        IL_00de:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_00e3:  /* 02   |                  */ ldarg.0
        IL_00e4:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_00e9:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
        IL_00ee:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_00f3:  /* 6F   | (0A)0000C6       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSemanticModelAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000C6 */
        IL_00f8:  /* 16   |                  */ ldc.i4.0
        IL_00f9:  /* 6F   | (0A)0000C7       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001E*/::ConfigureAwait(bool) /* 0A0000C7 */
        IL_00fe:  /* 13   | 06               */ stloc.s    V_6
        IL_0100:  /* 12   | 06               */ ldloca.s   V_6
        IL_0102:  /* 28   | (0A)0000C8       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001F*/::GetAwaiter() /* 0A0000C8 */
        IL_0107:  /* 13   | 05               */ stloc.s    V_5
        IL_0109:  /* 12   | 05               */ ldloca.s   V_5
        IL_010b:  /* 28   | (0A)0000C9       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::get_IsCompleted() /* 0A0000C9 */
        IL_0110:  /* 2D   | 41               */ brtrue.s   IL_0153

        IL_0112:  /* 02   |                  */ ldarg.0
        IL_0113:  /* 17   |                  */ ldc.i4.1
        IL_0114:  /* 25   |                  */ dup
        IL_0115:  /* 0A   |                  */ stloc.0
        IL_0116:  /* 7D   | (04)000032       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>1__state' /* 04000032 */
        IL_011b:  /* 02   |                  */ ldarg.0
        IL_011c:  /* 11   | 05               */ ldloc.s    V_5
        IL_011e:  /* 7D   | (04)000037       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>u__2' /* 04000037 */
        IL_0123:  /* 02   |                  */ ldarg.0
        IL_0124:  /* 7C   | (04)000033       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>t__builder' /* 04000033 */
        IL_0129:  /* 12   | 05               */ ldloca.s   V_5
        IL_012b:  /* 02   |                  */ ldarg.0
        IL_012c:  /* 28   | (2B)00001D       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>,valuetype Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          !!1&) /* 2B00001D */
        IL_0131:  /* DD   | 79010000         */ leave      IL_02af

        IL_0136:  /* 02   |                  */ ldarg.0
        IL_0137:  /* 7B   | (04)000037       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>u__2' /* 04000037 */
        IL_013c:  /* 13   | 05               */ stloc.s    V_5
        IL_013e:  /* 02   |                  */ ldarg.0
        IL_013f:  /* 7C   | (04)000037       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>u__2' /* 04000037 */
        IL_0144:  /* FE15 | (1B)000020       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/
        IL_014a:  /* 02   |                  */ ldarg.0
        IL_014b:  /* 15   |                  */ ldc.i4.m1
        IL_014c:  /* 25   |                  */ dup
        IL_014d:  /* 0A   |                  */ stloc.0
        IL_014e:  /* 7D   | (04)000032       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>1__state' /* 04000032 */
        IL_0153:  /* 12   | 05               */ ldloca.s   V_5
        IL_0155:  /* 28   | (0A)0000CA       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::GetResult() /* 0A0000CA */
        IL_015a:  /* 0B   |                  */ stloc.1
        IL_015b:  /* 07   |                  */ ldloc.1
        IL_015c:  /* 2D   | 05               */ brtrue.s   IL_0163

        IL_015e:  /* DD   | 32010000         */ leave      IL_0295

        IL_0163:  /* 02   |                  */ ldarg.0
        IL_0164:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_0169:  /* 07   |                  */ ldloc.1
        IL_016a:  /* 6F   | (0A)0000CB       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Compilation/*0100008F*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/::get_Compilation() /* 0A0000CB */
        IL_016f:  /* 28   | (0A)0000CC       */ call       class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/::GetOrCreate(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Compilation/*0100008F*/) /* 0A0000CC */
        IL_0174:  /* 7D   | (04)00002B       */ stfld      class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::wellKnownTypes /* 0400002B */
        IL_0179:  /* 02   |                  */ ldarg.0
        IL_017a:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_017f:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
        IL_0184:  /* 28   | (0A)000068       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Diagnostics() /* 0A000068 */
        IL_0189:  /* 13   | 08               */ stloc.s    V_8
        IL_018b:  /* 12   | 08               */ ldloca.s   V_8
        IL_018d:  /* 28   | (0A)000069       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<!0> valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000010*/::GetEnumerator() /* 0A000069 */
        IL_0192:  /* 13   | 07               */ stloc.s    V_7
        IL_0194:  /* 38   | CE000000         */ br         IL_0267

        IL_0199:  /* 73   | (06)00006C       */ newobj     instance void Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::.ctor() /* 0600006C */
        IL_019e:  /* 13   | 09               */ stloc.s    V_9
        IL_01a0:  /* 11   | 09               */ ldloc.s    V_9
        IL_01a2:  /* 02   |                  */ ldarg.0
        IL_01a3:  /* 7B   | (04)000035       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_01a8:  /* 7D   | (04)000031       */ stfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
        IL_01ad:  /* 11   | 09               */ ldloc.s    V_9
        IL_01af:  /* 12   | 07               */ ldloca.s   V_7
        IL_01b1:  /* 28   | (0A)00006A       */ call       instance !0 valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::get_Current() /* 0A00006A */
        IL_01b6:  /* 7D   | (04)00002E       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::diagnostic /* 0400002E */
        IL_01bb:  /* 11   | 09               */ ldloc.s    V_9
        IL_01bd:  /* 7B   | (04)00002E       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::diagnostic /* 0400002E */
        IL_01c2:  /* 11   | 09               */ ldloc.s    V_9
        IL_01c4:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
        IL_01c9:  /* 7B   | (04)00002C       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::root /* 0400002C */
        IL_01ce:  /* 11   | 09               */ ldloc.s    V_9
        IL_01d0:  /* 7C   | (04)00002F       */ ldflda     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::invocation /* 0400002F */
        IL_01d5:  /* 28   | (06)00003B       */ call       bool Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*/::CanReplaceWithAppend(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                      class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/&) /* 0600003B */
        IL_01da:  /* 2C   | 35               */ brfalse.s  IL_0211

        IL_01dc:  /* 72   | (70)0008A0       */ ldstr      "Use 'IHeaderDictionary.Append'" /* 700008A0 */
        IL_01e1:  /* 13   | 0A               */ stloc.s    V_10
        IL_01e3:  /* 11   | 09               */ ldloc.s    V_9
        IL_01e5:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
        IL_01ea:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
        IL_01ef:  /* 11   | 0A               */ ldloc.s    V_10
        IL_01f1:  /* 11   | 09               */ ldloc.s    V_9
        IL_01f3:  /* FE06 | (06)00006D       */ ldftn      instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0600006D */
        IL_01f9:  /* 73   | (0A)00006E       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>/*1B000013*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                                                                   native int) /* 0A00006E */
        IL_01fe:  /* 11   | 0A               */ ldloc.s    V_10
        IL_0200:  /* 28   | (0A)00006F       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/::Create(string,
                                                                                                                                                                                                                                                                                           class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>,
                                                                                                                                                                                                                                                                                           string) /* 0A00006F */
        IL_0205:  /* 11   | 09               */ ldloc.s    V_9
        IL_0207:  /* 7B   | (04)00002E       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::diagnostic /* 0400002E */
        IL_020c:  /* 28   | (0A)000070       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::RegisterCodeFix(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/,
                                                                                                                                                                                                     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/) /* 0A000070 */
        IL_0211:  /* 11   | 09               */ ldloc.s    V_9
        IL_0213:  /* 7B   | (04)00002E       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::diagnostic /* 0400002E */
        IL_0218:  /* 11   | 09               */ ldloc.s    V_9
        IL_021a:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
        IL_021f:  /* 7B   | (04)00002C       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::root /* 0400002C */
        IL_0224:  /* 11   | 09               */ ldloc.s    V_9
        IL_0226:  /* 7C   | (04)000030       */ ldflda     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::assignment /* 04000030 */
        IL_022b:  /* 28   | (06)00003D       */ call       bool Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*/::CanReplaceWithIndexer(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                       class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/&) /* 0600003D */
        IL_0230:  /* 2C   | 35               */ brfalse.s  IL_0267

        IL_0232:  /* 72   | (70)0008DE       */ ldstr      "Use indexer" /* 700008DE */
        IL_0237:  /* 13   | 0B               */ stloc.s    V_11
        IL_0239:  /* 11   | 09               */ ldloc.s    V_9
        IL_023b:  /* 7B   | (04)000031       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'CS$<>8__locals1' /* 04000031 */
        IL_0240:  /* 7C   | (04)00002D       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/::context /* 0400002D */
        IL_0245:  /* 11   | 0B               */ ldloc.s    V_11
        IL_0247:  /* 11   | 09               */ ldloc.s    V_9
        IL_0249:  /* FE06 | (06)00006E       */ ldftn      instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::'<RegisterCodeFixesAsync>b__1'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0600006E */
        IL_024f:  /* 73   | (0A)00006E       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>/*1B000013*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                                                                   native int) /* 0A00006E */
        IL_0254:  /* 11   | 0B               */ ldloc.s    V_11
        IL_0256:  /* 28   | (0A)00006F       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/::Create(string,
                                                                                                                                                                                                                                                                                           class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>,
                                                                                                                                                                                                                                                                                           string) /* 0A00006F */
        IL_025b:  /* 11   | 09               */ ldloc.s    V_9
        IL_025d:  /* 7B   | (04)00002E       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_1'/*0200001A*/::diagnostic /* 0400002E */
        IL_0262:  /* 28   | (0A)000070       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::RegisterCodeFix(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/,
                                                                                                                                                                                                     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/) /* 0A000070 */
        IL_0267:  /* 12   | 07               */ ldloca.s   V_7
        IL_0269:  /* 28   | (0A)000071       */ call       instance bool valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::MoveNext() /* 0A000071 */
        IL_026e:  /* 3A   | 26FFFFFF         */ brtrue     IL_0199

        IL_0273:  /* DE   | 20               */ leave.s    IL_0295

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_0275:  /* 13   | 0C               */ stloc.s    V_12
        IL_0277:  /* 02   |                  */ ldarg.0
        IL_0278:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_027a:  /* 7D   | (04)000032       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>1__state' /* 04000032 */
        IL_027f:  /* 02   |                  */ ldarg.0
        IL_0280:  /* 14   |                  */ ldnull
        IL_0281:  /* 7D   | (04)000035       */ stfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
        IL_0286:  /* 02   |                  */ ldarg.0
        IL_0287:  /* 7C   | (04)000033       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>t__builder' /* 04000033 */
        IL_028c:  /* 11   | 0C               */ ldloc.s    V_12
        IL_028e:  /* 28   | (0A)0000C3       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000C3 */
        IL_0293:  /* DE   | 1A               */ leave.s    IL_02af

      }  // end handler
      // HEX: 00 00 00 00 07 00 00 00 6E 02 00 00 75 02 00 00 20 00 00 00 40 00 00 01
      IL_0295:  /* 02   |                  */ ldarg.0
      IL_0296:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_0298:  /* 7D   | (04)000032       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>1__state' /* 04000032 */
      IL_029d:  /* 02   |                  */ ldarg.0
      IL_029e:  /* 14   |                  */ ldnull
      IL_029f:  /* 7D   | (04)000035       */ stfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<>c__DisplayClass4_0'/*02000019*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>8__1' /* 04000035 */
      IL_02a4:  /* 02   |                  */ ldarg.0
      IL_02a5:  /* 7C   | (04)000033       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>t__builder' /* 04000033 */
      IL_02aa:  /* 28   | (0A)0000C4       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetResult() /* 0A0000C4 */
      IL_02af:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__4'::MoveNext

    .method /*06000070*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000D2:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x4680
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)000033       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>t__builder' /* 04000033 */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000C5       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000C5 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__4'::SetStateMachine

  } // end of class '<RegisterCodeFixesAsync>d__4'

  .field /*04000018*/ private initonly valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> '<FixableDiagnosticIds>k__BackingField'
  .custom /*0C000071:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*06000037*/ public hidebysig specialname virtual 
          instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> 
          get_FixableDiagnosticIds() cil managed
  // SIG: 20 00 15 11 80 91 01 0E
  {
    .custom /*0C0000BB:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .param [0]/*08000052*/ 
    .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x321c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000018       */ ldfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*/::'<FixableDiagnosticIds>k__BackingField' /* 04000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryAddFixer::get_FixableDiagnosticIds

  .method /*06000038*/ public hidebysig virtual final 
          instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ 
          GetFixAllProvider() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x3224
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 28   | (0A)000033       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders/*0100006F*/::get_BatchFixer() /* 0A000033 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryAddFixer::GetFixAllProvider

  .method /*06000039*/ public hidebysig virtual final 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          RegisterCodeFixesAsync(valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context) cil managed
  // SIG: 20 01 12 80 A1 11 80 A5
  {
    .custom /*0C0000BC:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 60 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..`Microsoft.Asp
                                                                                                                                                                                                                                  4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65 72   // NetCore.Analyzer
                                                                                                                                                                                                                                  73 2E 48 74 74 70 2E 46 69 78 65 72 73 2E 48 65   // s.Http.Fixers.He
                                                                                                                                                                                                                                  61 64 65 72 44 69 63 74 69 6F 6E 61 72 79 41 64   // aderDictionaryAd
                                                                                                                                                                                                                                  64 46 69 78 65 72 2B 3C 52 65 67 69 73 74 65 72   // dFixer+<Register
                                                                                                                                                                                                                                  43 6F 64 65 46 69 78 65 73 41 73 79 6E 63 3E 64   // CodeFixesAsync>d
                                                                                                                                                                                                                                  5F 5F 34 00 00 )                                  // __4..
    // Method begins at RVA 0x322c
    // Code size       55 (0x37)
    .maxstack  2
    .locals /*11000012*/ init (valuetype Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)000034       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Create() /* 0A000034 */
    IL_0007:  /* 7D   | (04)000033       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>t__builder' /* 04000033 */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 7D   | (04)000034       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::context /* 04000034 */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 15   |                  */ ldc.i4.m1
    IL_0017:  /* 7D   | (04)000032       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>1__state' /* 04000032 */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 7C   | (04)000033       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>t__builder' /* 04000033 */
    IL_0023:  /* 12   | 00               */ ldloca.s   V_0
    IL_0025:  /* 28   | (2B)000009       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Start<valuetype Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/>(!!0&) /* 2B000009 */
    IL_002a:  /* 12   | 00               */ ldloca.s   V_0
    IL_002c:  /* 7C   | (04)000033       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*//'<RegisterCodeFixesAsync>d__4'/*0200001B*/::'<>t__builder' /* 04000033 */
    IL_0031:  /* 28   | (0A)000036       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::get_Task() /* 0A000036 */
    IL_0036:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryAddFixer::RegisterCodeFixesAsync

  .method /*0600003A*/ private hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
          ReplaceWithAppend(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                            class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ wellKnownTypes,
                            class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                            class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                            class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ invocation) cil managed
  // SIG: 00 05 15 12 80 A9 01 12 80 AD 12 80 B1 12 5D 12 80 B5 12 80 AD 12 80 B9
  {
    // Method begins at RVA 0x3270
    // Code size       88 (0x58)
    .maxstack  8
    .locals /*11000013*/ init (class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_0,
             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxAnnotation/*01000045*/ V_1)
    IL_0000:  /* 04   |                  */ ldarg.2
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_0007:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_000c:  /* 16   |                  */ ldc.i4.0
    IL_000d:  /* 17   |                  */ ldc.i4.1
    IL_000e:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_0013:  /* 0A   |                  */ stloc.0
    IL_0014:  /* 72   | (70)000183       */ ldstr      "SymbolId" /* 70000183 */
    IL_0019:  /* 03   |                  */ ldarg.1
    IL_001a:  /* 1F   | 0A               */ ldc.i4.s   10
    IL_001c:  /* 6F   | (0A)00001E       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.INamedTypeSymbol/*01000068*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/::Get(valuetype [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypeData/*01000043*//WellKnownType/*01000044*/) /* 0A00001E */
    IL_0021:  /* 28   | (0A)000059       */ call       string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DocumentationCommentId/*01000079*/::CreateReferenceId(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/) /* 0A000059 */
    IL_0026:  /* 73   | (0A)00005A       */ newobj     instance void [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxAnnotation/*01000045*/::.ctor(string,
                                                                                                                                                                    string) /* 0A00005A */
    IL_002b:  /* 0B   |                  */ stloc.1
    IL_002c:  /* 05   |                  */ ldarg.3
    IL_002d:  /* 04   |                  */ ldarg.2
    IL_002e:  /* 06   |                  */ ldloc.0
    IL_002f:  /* 0E   | 04               */ ldarg.s    invocation
    IL_0031:  /* 18   |                  */ ldc.i4.2
    IL_0032:  /* 8D   | (01)000045       */ newarr     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxAnnotation/*01000045*/
    IL_0037:  /* 25   |                  */ dup
    IL_0038:  /* 16   |                  */ ldc.i4.0
    IL_0039:  /* 28   | (0A)00005B       */ call       class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxAnnotation/*01000045*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Simplification.Simplifier/*0100007A*/::get_AddImportsAnnotation() /* 0A00005B */
    IL_003e:  /* A2   |                  */ stelem.ref
    IL_003f:  /* 25   |                  */ dup
    IL_0040:  /* 17   |                  */ ldc.i4.1
    IL_0041:  /* 07   |                  */ ldloc.1
    IL_0042:  /* A2   |                  */ stelem.ref
    IL_0043:  /* 28   | (2B)00000A       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.AnnotationExtensions/*0100007B*/::WithAdditionalAnnotations<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/>(!!0,
                                                                                                                                                                                                                                                                                                                class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxAnnotation/*01000045*/[]) /* 2B00000A */
    IL_0048:  /* 28   | (2B)000003       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceNode<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>(!!0,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 2B000003 */
    IL_004d:  /* 6F   | (0A)00003B       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::WithSyntaxRoot(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 0A00003B */
    IL_0052:  /* 28   | (2B)000004       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!!0> [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/::FromResult<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>(!!0) /* 2B000004 */
    IL_0057:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryAddFixer::ReplaceWithAppend

  .method /*0600003B*/ private hidebysig static 
          bool  CanReplaceWithAppend(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                                     [out] class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/& invocation) cil managed
  // SIG: 00 03 02 12 80 B1 12 80 B5 10 12 80 B9
  {
    .param [3]/*0800005B*/ 
    .custom /*0C0000CB:0A000017*/ instance void [Microsoft.AspNetCore.App.Analyzers/*23000002*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x32d4
    // Code size       96 (0x60)
    .maxstack  4
    .locals /*11000014*/ init (class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ V_0,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ V_1,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_2,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ V_3)
    IL_0000:  /* 04   |                  */ ldarg.2
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 51   |                  */ stind.ref
    IL_0003:  /* 03   |                  */ ldarg.1
    IL_0004:  /* 75   | (01)00007C       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax/*0100007C*/
    IL_0009:  /* 2D   | 02               */ brtrue.s   IL_000d

    IL_000b:  /* 16   |                  */ ldc.i4.0
    IL_000c:  /* 2A   |                  */ ret

    IL_000d:  /* 03   |                  */ ldarg.1
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_0014:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_0019:  /* 16   |                  */ ldc.i4.0
    IL_001a:  /* 17   |                  */ ldc.i4.1
    IL_001b:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_0020:  /* 75   | (01)00002E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
    IL_0025:  /* 0A   |                  */ stloc.0
    IL_0026:  /* 06   |                  */ ldloc.0
    IL_0027:  /* 2C   | 35               */ brfalse.s  IL_005e

    IL_0029:  /* 06   |                  */ ldloc.0
    IL_002a:  /* 6F   | (0A)00003D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_Expression() /* 0A00003D */
    IL_002f:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
    IL_0034:  /* 0C   |                  */ stloc.2
    IL_0035:  /* 08   |                  */ ldloc.2
    IL_0036:  /* 2C   | 26               */ brfalse.s  IL_005e

    IL_0038:  /* 08   |                  */ ldloc.2
    IL_0039:  /* 6F   | (0A)00004D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Name() /* 0A00004D */
    IL_003e:  /* 0D   |                  */ stloc.3
    IL_003f:  /* 09   |                  */ ldloc.3
    IL_0040:  /* 2C   | 1C               */ brfalse.s  IL_005e

    IL_0042:  /* 09   |                  */ ldloc.3
    IL_0043:  /* 6F   | (0A)00005D       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/::get_Identifier() /* 0A00005D */
    IL_0048:  /* 0B   |                  */ stloc.1
    IL_0049:  /* 04   |                  */ ldarg.2
    IL_004a:  /* 06   |                  */ ldloc.0
    IL_004b:  /* 07   |                  */ ldloc.1
    IL_004c:  /* 72   | (70)000195       */ ldstr      "Append" /* 70000195 */
    IL_0051:  /* 28   | (0A)00005E       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::Identifier(string) /* 0A00005E */
    IL_0056:  /* 28   | (2B)00000B       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceToken<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/>(!!0,
                                                                                                                                                                                                                                                                                                   valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/,
                                                                                                                                                                                                                                                                                                   valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/) /* 2B00000B */
    IL_005b:  /* 51   |                  */ stind.ref
    IL_005c:  /* 17   |                  */ ldc.i4.1
    IL_005d:  /* 2A   |                  */ ret

    IL_005e:  /* 16   |                  */ ldc.i4.0
    IL_005f:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryAddFixer::CanReplaceWithAppend

  .method /*0600003C*/ private hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
          ReplaceWithIndexer(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                             class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/ assignment) cil managed
  // SIG: 00 04 15 12 80 A9 01 12 80 AD 12 80 B1 12 80 B5 12 80 AD 12 81 1D
  {
    // Method begins at RVA 0x3340
    // Code size       40 (0x28)
    .maxstack  4
    .locals /*11000010*/ init (class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_0007:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_000c:  /* 16   |                  */ ldc.i4.0
    IL_000d:  /* 17   |                  */ ldc.i4.1
    IL_000e:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_0013:  /* 0A   |                  */ stloc.0
    IL_0014:  /* 04   |                  */ ldarg.2
    IL_0015:  /* 03   |                  */ ldarg.1
    IL_0016:  /* 06   |                  */ ldloc.0
    IL_0017:  /* 05   |                  */ ldarg.3
    IL_0018:  /* 28   | (2B)000003       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceNode<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>(!!0,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 2B000003 */
    IL_001d:  /* 6F   | (0A)00003B       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::WithSyntaxRoot(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 0A00003B */
    IL_0022:  /* 28   | (2B)000004       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!!0> [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/::FromResult<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>(!!0) /* 2B000004 */
    IL_0027:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryAddFixer::ReplaceWithIndexer

  .method /*0600003D*/ private hidebysig static 
          bool  CanReplaceWithIndexer(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                                      [out] class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/& assignment) cil managed
  // SIG: 00 03 02 12 80 B1 12 80 B5 10 12 81 1D
  {
    .param [3]/*08000062*/ 
    .custom /*0C0000CE:0A000017*/ instance void [Microsoft.AspNetCore.App.Analyzers/*23000002*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x3374
    // Code size       145 (0x91)
    .maxstack  8
    .locals /*11000015*/ init (class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_0,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/> V_1,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ V_2,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ V_3)
    IL_0000:  /* 04   |                  */ ldarg.2
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 51   |                  */ stind.ref
    IL_0003:  /* 03   |                  */ ldarg.1
    IL_0004:  /* 2D   | 02               */ brtrue.s   IL_0008

    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 2A   |                  */ ret

    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 02   |                  */ ldarg.0
    IL_000a:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_000f:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_0014:  /* 16   |                  */ ldc.i4.0
    IL_0015:  /* 17   |                  */ ldc.i4.1
    IL_0016:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_001b:  /* 75   | (01)00002E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
    IL_0020:  /* 0C   |                  */ stloc.2
    IL_0021:  /* 08   |                  */ ldloc.2
    IL_0022:  /* 2C   | 6B               */ brfalse.s  IL_008f

    IL_0024:  /* 08   |                  */ ldloc.2
    IL_0025:  /* 6F   | (0A)00003D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_Expression() /* 0A00003D */
    IL_002a:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
    IL_002f:  /* 0A   |                  */ stloc.0
    IL_0030:  /* 06   |                  */ ldloc.0
    IL_0031:  /* 2C   | 5C               */ brfalse.s  IL_008f

    IL_0033:  /* 08   |                  */ ldloc.2
    IL_0034:  /* 6F   | (0A)000044       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_ArgumentList() /* 0A000044 */
    IL_0039:  /* 0D   |                  */ stloc.3
    IL_003a:  /* 09   |                  */ ldloc.3
    IL_003b:  /* 2C   | 52               */ brfalse.s  IL_008f

    IL_003d:  /* 09   |                  */ ldloc.3
    IL_003e:  /* 6F   | (0A)000045       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax/*01000073*/::get_Arguments() /* 0A000045 */
    IL_0043:  /* 0B   |                  */ stloc.1
    IL_0044:  /* 12   | 01               */ ldloca.s   V_1
    IL_0046:  /* 28   | (0A)000060       */ call       instance int32 valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>/*1B00000D*/::get_Count() /* 0A000060 */
    IL_004b:  /* 18   |                  */ ldc.i4.2
    IL_004c:  /* 33   | 41               */ bne.un.s   IL_008f

    IL_004e:  /* 04   |                  */ ldarg.2
    IL_004f:  /* 20   | 0A220000         */ ldc.i4     0x220a
    IL_0054:  /* 06   |                  */ ldloc.0
    IL_0055:  /* 6F   | (0A)00003E       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Expression() /* 0A00003E */
    IL_005a:  /* 17   |                  */ ldc.i4.1
    IL_005b:  /* 8D   | (01)000033       */ newarr     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/
    IL_0060:  /* 25   |                  */ dup
    IL_0061:  /* 16   |                  */ ldc.i4.0
    IL_0062:  /* 12   | 01               */ ldloca.s   V_1
    IL_0064:  /* 16   |                  */ ldc.i4.0
    IL_0065:  /* 28   | (0A)000061       */ call       instance !0 valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>/*1B00000D*/::get_Item(int32) /* 0A000061 */
    IL_006a:  /* A2   |                  */ stelem.ref
    IL_006b:  /* 28   | (2B)00000C       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<!!0> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::SeparatedList<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<!!0>) /* 2B00000C */
    IL_0070:  /* 28   | (0A)000063       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax/*0100007E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::BracketedArgumentList(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>) /* 0A000063 */
    IL_0075:  /* 28   | (0A)000064       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax/*0100004A*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::ElementAccessExpression(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/,
                                                                                                                                                                                                                                                                                                                   class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax/*0100007E*/) /* 0A000064 */
    IL_007a:  /* 12   | 01               */ ldloca.s   V_1
    IL_007c:  /* 17   |                  */ ldc.i4.1
    IL_007d:  /* 28   | (0A)000061       */ call       instance !0 valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>/*1B00000D*/::get_Item(int32) /* 0A000061 */
    IL_0082:  /* 6F   | (0A)000047       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/::get_Expression() /* 0A000047 */
    IL_0087:  /* 28   | (0A)000065       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::AssignmentExpression(valuetype [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxKind/*0100007F*/,
                                                                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/,
                                                                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/) /* 0A000065 */
    IL_008c:  /* 51   |                  */ stind.ref
    IL_008d:  /* 17   |                  */ ldc.i4.1
    IL_008e:  /* 2A   |                  */ ret

    IL_008f:  /* 16   |                  */ ldc.i4.0
    IL_0090:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryAddFixer::CanReplaceWithIndexer

  .method /*0600003E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3411
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (0A)000066       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DoNotUseIHeaderDictionaryAdd /* 0A000066 */
    IL_0006:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_000b:  /* 28   | (2B)00000D       */ call       valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<!!0> [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray/*01000078*/::Create<string>(!!0) /* 2B00000D */
    IL_0010:  /* 7D   | (04)000018       */ stfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*/::'<FixableDiagnosticIds>k__BackingField' /* 04000018 */
    IL_0015:  /* 02   |                  */ ldarg.0
    IL_0016:  /* 28   | (0A)000058       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/::.ctor() /* 0A000058 */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryAddFixer::.ctor

  .property /*17000006*/ instance callconv(8) valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>
          FixableDiagnosticIds()
  {
    .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer/*0200000D*/::get_FixableDiagnosticIds() /* 06000037 */
  } // end of property HeaderDictionaryAddFixer::FixableDiagnosticIds
} // end of class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryAddFixer

.class /*0200000E*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer
       extends [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/
{
  .custom /*0C00004A:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00004C:0A000014*/ instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.ExportCodeFixProviderAttribute/*01000021*/::.ctor(string,
                                                                                                                                                                                string[]) /* 0A000014 */ = ( 01 00 02 43 23 00 00 00 00 00 00 )                // ...C#......
  .custom /*0C00004D:0A000015*/ instance void [System.Composition.AttributedModel/*23000005*/]System.Composition.SharedAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*0200001C*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C00007F:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000038*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .method /*06000071*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x468e
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_0'::.ctor

  } // end of class '<>c__DisplayClass4_0'

  .class /*0200001D*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_1'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C000085:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000039*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic
    .field /*0400003A*/ public string resolvedPropertyName
    .field /*0400003B*/ public class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/ 'CS$<>8__locals1'
    .method /*06000072*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4696
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::.ctor

    .method /*06000073*/ assembly hidebysig 
            instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
            '<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
    // SIG: 20 01 15 12 80 A9 01 12 80 AD 11 80 ED
    {
      // Method begins at RVA 0x469e
      // Code size       35 (0x23)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000039       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::diagnostic /* 04000039 */
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 7B   | (04)00003B       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::'CS$<>8__locals1' /* 0400003B */
      IL_000c:  /* 7C   | (04)000038       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/::context /* 04000038 */
      IL_0011:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
      IL_0016:  /* 02   |                  */ ldarg.0
      IL_0017:  /* 7B   | (04)00003A       */ ldfld      string Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::resolvedPropertyName /* 0400003A */
      IL_001c:  /* 03   |                  */ ldarg.1
      IL_001d:  /* 28   | (06)000042       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*/::FixHeaderDictionaryIndexer(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                                                                                                                                                                    class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                                    string,
                                                                                                                                                                                                                                                                                                                                                    valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 06000042 */
      IL_0022:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::'<RegisterCodeFixesAsync>b__0'

  } // end of class '<>c__DisplayClass4_1'

  .class /*0200001E*/ auto ansi sealed nested private beforefieldinit '<FixHeaderDictionaryIndexer>d__5'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000088:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*0400003C*/ public int32 '<>1__state'
    .field /*0400003D*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> '<>t__builder'
    .field /*0400003E*/ public class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document
    .field /*0400003F*/ public valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken
    .field /*04000040*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic
    .field /*04000041*/ public string resolvedPropertyName
    .field /*04000042*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> '<>u__1'
    .method /*06000074*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x46c4
      // Code size       315 (0x13b)
      .maxstack  4
      .locals /*11000028*/ init (int32 V_0,
               class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ V_1,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_2,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_3,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/ V_4,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax/*0100004A*/ V_5,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ V_6,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_7,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_8,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_9,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_10)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)00003C       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>1__state' /* 0400003C */
      IL_0006:  /* 0A   |                  */ stloc.0
      .try
      {
        IL_0007:  /* 06   |                  */ ldloc.0
        IL_0008:  /* 2C   | 4F               */ brfalse.s  IL_0059

        IL_000a:  /* 02   |                  */ ldarg.0
        IL_000b:  /* 7B   | (04)00003E       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::document /* 0400003E */
        IL_0010:  /* 02   |                  */ ldarg.0
        IL_0011:  /* 7B   | (04)00003F       */ ldfld      valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::cancellationToken /* 0400003F */
        IL_0016:  /* 6F   | (0A)0000BB       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSyntaxRootAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000BB */
        IL_001b:  /* 16   |                  */ ldc.i4.0
        IL_001c:  /* 6F   | (0A)0000BC       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001B*/::ConfigureAwait(bool) /* 0A0000BC */
        IL_0021:  /* 13   | 08               */ stloc.s    V_8
        IL_0023:  /* 12   | 08               */ ldloca.s   V_8
        IL_0025:  /* 28   | (0A)0000BD       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001C*/::GetAwaiter() /* 0A0000BD */
        IL_002a:  /* 13   | 07               */ stloc.s    V_7
        IL_002c:  /* 12   | 07               */ ldloca.s   V_7
        IL_002e:  /* 28   | (0A)0000BE       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::get_IsCompleted() /* 0A0000BE */
        IL_0033:  /* 2D   | 41               */ brtrue.s   IL_0076

        IL_0035:  /* 02   |                  */ ldarg.0
        IL_0036:  /* 16   |                  */ ldc.i4.0
        IL_0037:  /* 25   |                  */ dup
        IL_0038:  /* 0A   |                  */ stloc.0
        IL_0039:  /* 7D   | (04)00003C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>1__state' /* 0400003C */
        IL_003e:  /* 02   |                  */ ldarg.0
        IL_003f:  /* 11   | 07               */ ldloc.s    V_7
        IL_0041:  /* 7D   | (04)000042       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>u__1' /* 04000042 */
        IL_0046:  /* 02   |                  */ ldarg.0
        IL_0047:  /* 7C   | (04)00003D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>t__builder' /* 0400003D */
        IL_004c:  /* 12   | 07               */ ldloca.s   V_7
        IL_004e:  /* 02   |                  */ ldarg.0
        IL_004f:  /* 28   | (2B)00001E       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>,valuetype Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         !!1&) /* 2B00001E */
        IL_0054:  /* DD   | E1000000         */ leave      IL_013a

        IL_0059:  /* 02   |                  */ ldarg.0
        IL_005a:  /* 7B   | (04)000042       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>u__1' /* 04000042 */
        IL_005f:  /* 13   | 07               */ stloc.s    V_7
        IL_0061:  /* 02   |                  */ ldarg.0
        IL_0062:  /* 7C   | (04)000042       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>u__1' /* 04000042 */
        IL_0067:  /* FE15 | (1B)00001D       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/
        IL_006d:  /* 02   |                  */ ldarg.0
        IL_006e:  /* 15   |                  */ ldc.i4.m1
        IL_006f:  /* 25   |                  */ dup
        IL_0070:  /* 0A   |                  */ stloc.0
        IL_0071:  /* 7D   | (04)00003C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>1__state' /* 0400003C */
        IL_0076:  /* 12   | 07               */ ldloca.s   V_7
        IL_0078:  /* 28   | (0A)0000C0       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::GetResult() /* 0A0000C0 */
        IL_007d:  /* 0C   |                  */ stloc.2
        IL_007e:  /* 08   |                  */ ldloc.2
        IL_007f:  /* 2D   | 0C               */ brtrue.s   IL_008d

        IL_0081:  /* 02   |                  */ ldarg.0
        IL_0082:  /* 7B   | (04)00003E       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::document /* 0400003E */
        IL_0087:  /* 0B   |                  */ stloc.1
        IL_0088:  /* DD   | 99000000         */ leave      IL_0126

        IL_008d:  /* 08   |                  */ ldloc.2
        IL_008e:  /* 02   |                  */ ldarg.0
        IL_008f:  /* 7B   | (04)000040       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::diagnostic /* 04000040 */
        IL_0094:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
        IL_0099:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
        IL_009e:  /* 16   |                  */ ldc.i4.0
        IL_009f:  /* 16   |                  */ ldc.i4.0
        IL_00a0:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                        bool,
                                                                                                                                                                                                                                                        bool) /* 0A000039 */
        IL_00a5:  /* 0D   |                  */ stloc.3
        IL_00a6:  /* 09   |                  */ ldloc.3
        IL_00a7:  /* 75   | (01)000033       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/
        IL_00ac:  /* 13   | 04               */ stloc.s    V_4
        IL_00ae:  /* 11   | 04               */ ldloc.s    V_4
        IL_00b0:  /* 2C   | 08               */ brfalse.s  IL_00ba

        IL_00b2:  /* 11   | 04               */ ldloc.s    V_4
        IL_00b4:  /* 6F   | (0A)000047       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/::get_Expression() /* 0A000047 */
        IL_00b9:  /* 0D   |                  */ stloc.3
        IL_00ba:  /* 09   |                  */ ldloc.3
        IL_00bb:  /* 75   | (01)00004A       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax/*0100004A*/
        IL_00c0:  /* 13   | 05               */ stloc.s    V_5
        IL_00c2:  /* 11   | 05               */ ldloc.s    V_5
        IL_00c4:  /* 2C   | 3E               */ brfalse.s  IL_0104

        IL_00c6:  /* 11   | 05               */ ldloc.s    V_5
        IL_00c8:  /* 6F   | (0A)0000CE       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax/*0100004A*/::get_Expression() /* 0A0000CE */
        IL_00cd:  /* 13   | 06               */ stloc.s    V_6
        IL_00cf:  /* 11   | 06               */ ldloc.s    V_6
        IL_00d1:  /* 2C   | 31               */ brfalse.s  IL_0104

        IL_00d3:  /* 20   | F1210000         */ ldc.i4     0x21f1
        IL_00d8:  /* 11   | 06               */ ldloc.s    V_6
        IL_00da:  /* 02   |                  */ ldarg.0
        IL_00db:  /* 7B   | (04)000041       */ ldfld      string Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::resolvedPropertyName /* 04000041 */
        IL_00e0:  /* 28   | (0A)0000B4       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax/*0100002F*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::IdentifierName(string) /* 0A0000B4 */
        IL_00e5:  /* 28   | (0A)0000B5       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::MemberAccessExpression(valuetype [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxKind/*0100007F*/,
                                                                                                                                                                                                                                                                                                                     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/,
                                                                                                                                                                                                                                                                                                                     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/) /* 0A0000B5 */
        IL_00ea:  /* 13   | 09               */ stloc.s    V_9
        IL_00ec:  /* 02   |                  */ ldarg.0
        IL_00ed:  /* 7B   | (04)00003E       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::document /* 0400003E */
        IL_00f2:  /* 08   |                  */ ldloc.2
        IL_00f3:  /* 11   | 05               */ ldloc.s    V_5
        IL_00f5:  /* 11   | 09               */ ldloc.s    V_9
        IL_00f7:  /* 28   | (2B)000003       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceNode<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>(!!0,
                                                                                                                                                                                                                                                                 class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                                 class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 2B000003 */
        IL_00fc:  /* 6F   | (0A)00003B       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::WithSyntaxRoot(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 0A00003B */
        IL_0101:  /* 0B   |                  */ stloc.1
        IL_0102:  /* DE   | 22               */ leave.s    IL_0126

        IL_0104:  /* 02   |                  */ ldarg.0
        IL_0105:  /* 7B   | (04)00003E       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::document /* 0400003E */
        IL_010a:  /* 0B   |                  */ stloc.1
        IL_010b:  /* DE   | 19               */ leave.s    IL_0126

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_010d:  /* 13   | 0A               */ stloc.s    V_10
        IL_010f:  /* 02   |                  */ ldarg.0
        IL_0110:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_0112:  /* 7D   | (04)00003C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>1__state' /* 0400003C */
        IL_0117:  /* 02   |                  */ ldarg.0
        IL_0118:  /* 7C   | (04)00003D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>t__builder' /* 0400003D */
        IL_011d:  /* 11   | 0A               */ ldloc.s    V_10
        IL_011f:  /* 28   | (0A)0000CF       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000CF */
        IL_0124:  /* DE   | 14               */ leave.s    IL_013a

      }  // end handler
      // HEX: 00 00 00 00 07 00 00 00 06 01 00 00 0D 01 00 00 19 00 00 00 40 00 00 01
      IL_0126:  /* 02   |                  */ ldarg.0
      IL_0127:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_0129:  /* 7D   | (04)00003C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>1__state' /* 0400003C */
      IL_012e:  /* 02   |                  */ ldarg.0
      IL_012f:  /* 7C   | (04)00003D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>t__builder' /* 0400003D */
      IL_0134:  /* 07   |                  */ ldloc.1
      IL_0135:  /* 28   | (0A)0000D0       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::SetResult(!0) /* 0A0000D0 */
      IL_013a:  /* 2A   |                  */ ret
    } // end of method '<FixHeaderDictionaryIndexer>d__5'::MoveNext

    .method /*06000075*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000D4:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x4828
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)00003D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>t__builder' /* 0400003D */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000D1       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000D1 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<FixHeaderDictionaryIndexer>d__5'::SetStateMachine

  } // end of class '<FixHeaderDictionaryIndexer>d__5'

  .field /*04000019*/ private initonly valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> '<FixableDiagnosticIds>k__BackingField'
  .custom /*0C000074:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*0600003F*/ public hidebysig specialname virtual 
          instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> 
          get_FixableDiagnosticIds() cil managed
  // SIG: 20 00 15 11 80 91 01 0E
  {
    .custom /*0C0000BD:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .param [0]/*08000063*/ 
    .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x342e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000019       */ ldfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*/::'<FixableDiagnosticIds>k__BackingField' /* 04000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryIndexerFixer::get_FixableDiagnosticIds

  .method /*06000040*/ public hidebysig virtual final 
          instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ 
          GetFixAllProvider() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x3436
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 28   | (0A)000033       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders/*0100006F*/::get_BatchFixer() /* 0A000033 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryIndexerFixer::GetFixAllProvider

  .method /*06000041*/ public hidebysig virtual final 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          RegisterCodeFixesAsync(valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context) cil managed
  // SIG: 20 01 12 80 A1 11 80 A5
  {
    // Method begins at RVA 0x3440
    // Code size       230 (0xe6)
    .maxstack  4
    .locals /*11000016*/ init (class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/ V_0,
             valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_1,
             valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_2,
             class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/ V_3,
             string V_4,
             string V_5)
    IL_0000:  /* 73   | (06)000071       */ newobj     instance void Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/::.ctor() /* 06000071 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 06   |                  */ ldloc.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000038       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/::context /* 04000038 */
    IL_000d:  /* 06   |                  */ ldloc.0
    IL_000e:  /* 7C   | (04)000038       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/::context /* 04000038 */
    IL_0013:  /* 28   | (0A)000068       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Diagnostics() /* 0A000068 */
    IL_0018:  /* 0C   |                  */ stloc.2
    IL_0019:  /* 12   | 02               */ ldloca.s   V_2
    IL_001b:  /* 28   | (0A)000069       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<!0> valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000010*/::GetEnumerator() /* 0A000069 */
    IL_0020:  /* 0B   |                  */ stloc.1
    IL_0021:  /* 38   | AE000000         */ br         IL_00d4

    IL_0026:  /* 73   | (06)000072       */ newobj     instance void Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::.ctor() /* 06000072 */
    IL_002b:  /* 0D   |                  */ stloc.3
    IL_002c:  /* 09   |                  */ ldloc.3
    IL_002d:  /* 06   |                  */ ldloc.0
    IL_002e:  /* 7D   | (04)00003B       */ stfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::'CS$<>8__locals1' /* 0400003B */
    IL_0033:  /* 09   |                  */ ldloc.3
    IL_0034:  /* 12   | 01               */ ldloca.s   V_1
    IL_0036:  /* 28   | (0A)00006A       */ call       instance !0 valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::get_Current() /* 0A00006A */
    IL_003b:  /* 7D   | (04)000039       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::diagnostic /* 04000039 */
    IL_0040:  /* 09   |                  */ ldloc.3
    IL_0041:  /* 7B   | (04)000039       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::diagnostic /* 04000039 */
    IL_0046:  /* 6F   | (0A)00006B       */ callvirt   instance class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string> [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Properties() /* 0A00006B */
    IL_004b:  /* 72   | (70)0001A3       */ ldstr      "HeaderName" /* 700001A3 */
    IL_0050:  /* 12   | 04               */ ldloca.s   V_4
    IL_0052:  /* 6F   | (0A)00006C       */ callvirt   instance bool class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string>/*1B000012*/::TryGetValue(!0,
                                                                                                                                                                                                                            !1&) /* 0A00006C */
    IL_0057:  /* 2C   | 7B               */ brfalse.s  IL_00d4

    IL_0059:  /* 09   |                  */ ldloc.3
    IL_005a:  /* 7B   | (04)000039       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::diagnostic /* 04000039 */
    IL_005f:  /* 6F   | (0A)00006B       */ callvirt   instance class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string> [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Properties() /* 0A00006B */
    IL_0064:  /* 72   | (70)0001B9       */ ldstr      "ResolvedPropertyName" /* 700001B9 */
    IL_0069:  /* 09   |                  */ ldloc.3
    IL_006a:  /* 7C   | (04)00003A       */ ldflda     string Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::resolvedPropertyName /* 0400003A */
    IL_006f:  /* 6F   | (0A)00006C       */ callvirt   instance bool class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string>/*1B000012*/::TryGetValue(!0,
                                                                                                                                                                                                                            !1&) /* 0A00006C */
    IL_0074:  /* 2C   | 5E               */ brfalse.s  IL_00d4

    IL_0076:  /* 1B   |                  */ ldc.i4.5
    IL_0077:  /* 8D   | (01)00006E       */ newarr     [netstandard/*23000001*/]System.String/*0100006E*/
    IL_007c:  /* 25   |                  */ dup
    IL_007d:  /* 16   |                  */ ldc.i4.0
    IL_007e:  /* 72   | (70)0001E3       */ ldstr      "Access header '" /* 700001E3 */
    IL_0083:  /* A2   |                  */ stelem.ref
    IL_0084:  /* 25   |                  */ dup
    IL_0085:  /* 17   |                  */ ldc.i4.1
    IL_0086:  /* 11   | 04               */ ldloc.s    V_4
    IL_0088:  /* A2   |                  */ stelem.ref
    IL_0089:  /* 25   |                  */ dup
    IL_008a:  /* 18   |                  */ ldc.i4.2
    IL_008b:  /* 72   | (70)000203       */ ldstr      "' with " /* 70000203 */
    IL_0090:  /* A2   |                  */ stelem.ref
    IL_0091:  /* 25   |                  */ dup
    IL_0092:  /* 19   |                  */ ldc.i4.3
    IL_0093:  /* 09   |                  */ ldloc.3
    IL_0094:  /* 7B   | (04)00003A       */ ldfld      string Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::resolvedPropertyName /* 0400003A */
    IL_0099:  /* A2   |                  */ stelem.ref
    IL_009a:  /* 25   |                  */ dup
    IL_009b:  /* 1A   |                  */ ldc.i4.4
    IL_009c:  /* 72   | (70)000213       */ ldstr      " property" /* 70000213 */
    IL_00a1:  /* A2   |                  */ stelem.ref
    IL_00a2:  /* 28   | (0A)00006D       */ call       string [netstandard/*23000001*/]System.String/*0100006E*/::Concat(string[]) /* 0A00006D */
    IL_00a7:  /* 13   | 05               */ stloc.s    V_5
    IL_00a9:  /* 09   |                  */ ldloc.3
    IL_00aa:  /* 7B   | (04)00003B       */ ldfld      class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::'CS$<>8__locals1' /* 0400003B */
    IL_00af:  /* 7C   | (04)000038       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_0'/*0200001C*/::context /* 04000038 */
    IL_00b4:  /* 11   | 05               */ ldloc.s    V_5
    IL_00b6:  /* 09   |                  */ ldloc.3
    IL_00b7:  /* FE06 | (06)000073       */ ldftn      instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::'<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 06000073 */
    IL_00bd:  /* 73   | (0A)00006E       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>/*1B000013*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                                                               native int) /* 0A00006E */
    IL_00c2:  /* 11   | 05               */ ldloc.s    V_5
    IL_00c4:  /* 28   | (0A)00006F       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/::Create(string,
                                                                                                                                                                                                                                                                                       class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>,
                                                                                                                                                                                                                                                                                       string) /* 0A00006F */
    IL_00c9:  /* 09   |                  */ ldloc.3
    IL_00ca:  /* 7B   | (04)000039       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<>c__DisplayClass4_1'/*0200001D*/::diagnostic /* 04000039 */
    IL_00cf:  /* 28   | (0A)000070       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::RegisterCodeFix(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/,
                                                                                                                                                                                                 class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/) /* 0A000070 */
    IL_00d4:  /* 12   | 01               */ ldloca.s   V_1
    IL_00d6:  /* 28   | (0A)000071       */ call       instance bool valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::MoveNext() /* 0A000071 */
    IL_00db:  /* 3A   | 46FFFFFF         */ brtrue     IL_0026

    IL_00e0:  /* 28   | (0A)000072       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/::get_CompletedTask() /* 0A000072 */
    IL_00e5:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryIndexerFixer::RegisterCodeFixesAsync

  .method /*06000042*/ private hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
          FixHeaderDictionaryIndexer(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                     class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                                     string resolvedPropertyName,
                                     valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 80 A9 01 12 80 AD 12 80 B1 12 80 AD 0E 11 80 ED
  {
    .custom /*0C0000BE:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 68 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..hMicrosoft.Asp
                                                                                                                                                                                                                                  4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65 72   // NetCore.Analyzer
                                                                                                                                                                                                                                  73 2E 48 74 74 70 2E 46 69 78 65 72 73 2E 48 65   // s.Http.Fixers.He
                                                                                                                                                                                                                                  61 64 65 72 44 69 63 74 69 6F 6E 61 72 79 49 6E   // aderDictionaryIn
                                                                                                                                                                                                                                  64 65 78 65 72 46 69 78 65 72 2B 3C 46 69 78 48   // dexerFixer+<FixH
                                                                                                                                                                                                                                  65 61 64 65 72 44 69 63 74 69 6F 6E 61 72 79 49   // eaderDictionaryI
                                                                                                                                                                                                                                  6E 64 65 78 65 72 3E 64 5F 5F 35 00 00 )          // ndexer>d__5..
    // Method begins at RVA 0x3534
    // Code size       79 (0x4f)
    .maxstack  2
    .locals /*11000017*/ init (valuetype Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)000073       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::Create() /* 0A000073 */
    IL_0007:  /* 7D   | (04)00003D       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>t__builder' /* 0400003D */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7D   | (04)000040       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::diagnostic /* 04000040 */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 03   |                  */ ldarg.1
    IL_0017:  /* 7D   | (04)00003E       */ stfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::document /* 0400003E */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 04   |                  */ ldarg.2
    IL_001f:  /* 7D   | (04)000041       */ stfld      string Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::resolvedPropertyName /* 04000041 */
    IL_0024:  /* 12   | 00               */ ldloca.s   V_0
    IL_0026:  /* 05   |                  */ ldarg.3
    IL_0027:  /* 7D   | (04)00003F       */ stfld      valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::cancellationToken /* 0400003F */
    IL_002c:  /* 12   | 00               */ ldloca.s   V_0
    IL_002e:  /* 15   |                  */ ldc.i4.m1
    IL_002f:  /* 7D   | (04)00003C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>1__state' /* 0400003C */
    IL_0034:  /* 12   | 00               */ ldloca.s   V_0
    IL_0036:  /* 7C   | (04)00003D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>t__builder' /* 0400003D */
    IL_003b:  /* 12   | 00               */ ldloca.s   V_0
    IL_003d:  /* 28   | (2B)00000E       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::Start<valuetype Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/>(!!0&) /* 2B00000E */
    IL_0042:  /* 12   | 00               */ ldloca.s   V_0
    IL_0044:  /* 7C   | (04)00003D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*//'<FixHeaderDictionaryIndexer>d__5'/*0200001E*/::'<>t__builder' /* 0400003D */
    IL_0049:  /* 28   | (0A)000075       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::get_Task() /* 0A000075 */
    IL_004e:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryIndexerFixer::FixHeaderDictionaryIndexer

  .method /*06000043*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x358f
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (0A)000076       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::UseHeaderDictionaryPropertiesInsteadOfIndexer /* 0A000076 */
    IL_0006:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_000b:  /* 28   | (2B)00000D       */ call       valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<!!0> [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray/*01000078*/::Create<string>(!!0) /* 2B00000D */
    IL_0010:  /* 7D   | (04)000019       */ stfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*/::'<FixableDiagnosticIds>k__BackingField' /* 04000019 */
    IL_0015:  /* 02   |                  */ ldarg.0
    IL_0016:  /* 28   | (0A)000058       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/::.ctor() /* 0A000058 */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method HeaderDictionaryIndexerFixer::.ctor

  .property /*17000007*/ instance callconv(8) valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>
          FixableDiagnosticIds()
  {
    .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer/*0200000E*/::get_FixableDiagnosticIds() /* 0600003F */
  } // end of property HeaderDictionaryIndexerFixer::FixableDiagnosticIds
} // end of class Microsoft.AspNetCore.Analyzers.Http.Fixers.HeaderDictionaryIndexerFixer

.class /*0200000F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer
       extends [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/
{
  .custom /*0C00004F:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000050:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000051:0A000014*/ instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.ExportCodeFixProviderAttribute/*01000021*/::.ctor(string,
                                                                                                                                                                                string[]) /* 0A000014 */ = ( 01 00 02 43 23 00 00 00 00 00 00 )                // ...C#......
  .custom /*0C000052:0A000015*/ instance void [System.Composition.AttributedModel/*23000005*/]System.Composition.SharedAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*0200001F*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C00008A:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000043*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .method /*06000076*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4836
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_0'::.ctor

  } // end of class '<>c__DisplayClass4_0'

  .class /*02000020*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_1'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C00008E:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000044*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic
    .field /*04000045*/ public class Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/ 'CS$<>8__locals1'
    .method /*06000077*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x483e
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::.ctor

    .method /*06000078*/ assembly hidebysig 
            instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
            '<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
    // SIG: 20 01 15 12 80 A9 01 12 80 AD 11 80 ED
    {
      // Method begins at RVA 0x4846
      // Code size       29 (0x1d)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000044       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/::diagnostic /* 04000044 */
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 7B   | (04)000045       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/::'CS$<>8__locals1' /* 04000045 */
      IL_000c:  /* 7C   | (04)000043       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/::context /* 04000043 */
      IL_0011:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
      IL_0016:  /* 03   |                  */ ldarg.1
      IL_0017:  /* 28   | (06)000047       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*/::FixMismatchedParameterOptionalityAsync(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                                                                                                                                                                                                      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                                                                      valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 06000047 */
      IL_001c:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::'<RegisterCodeFixesAsync>b__0'

  } // end of class '<>c__DisplayClass4_1'

  .class /*02000021*/ auto ansi sealed nested private beforefieldinit '<FixMismatchedParameterOptionalityAsync>d__5'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000091:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000046*/ public int32 '<>1__state'
    .field /*04000047*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> '<>t__builder'
    .field /*04000048*/ public class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document
    .field /*04000049*/ public valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken
    .field /*0400004A*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic
    .field /*0400004B*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> '<>u__1'
    .method /*06000079*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x4864
      // Code size       276 (0x114)
      .maxstack  4
      .locals /*11000029*/ init (int32 V_0,
               class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ V_1,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_2,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/ V_3,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ V_4,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_5,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_6,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/ V_7,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_8)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000046       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>1__state' /* 04000046 */
      IL_0006:  /* 0A   |                  */ stloc.0
      .try
      {
        IL_0007:  /* 06   |                  */ ldloc.0
        IL_0008:  /* 2C   | 4F               */ brfalse.s  IL_0059

        IL_000a:  /* 02   |                  */ ldarg.0
        IL_000b:  /* 7B   | (04)000048       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::document /* 04000048 */
        IL_0010:  /* 02   |                  */ ldarg.0
        IL_0011:  /* 7B   | (04)000049       */ ldfld      valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::cancellationToken /* 04000049 */
        IL_0016:  /* 6F   | (0A)0000BB       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSyntaxRootAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000BB */
        IL_001b:  /* 16   |                  */ ldc.i4.0
        IL_001c:  /* 6F   | (0A)0000BC       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001B*/::ConfigureAwait(bool) /* 0A0000BC */
        IL_0021:  /* 13   | 06               */ stloc.s    V_6
        IL_0023:  /* 12   | 06               */ ldloca.s   V_6
        IL_0025:  /* 28   | (0A)0000BD       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001C*/::GetAwaiter() /* 0A0000BD */
        IL_002a:  /* 13   | 05               */ stloc.s    V_5
        IL_002c:  /* 12   | 05               */ ldloca.s   V_5
        IL_002e:  /* 28   | (0A)0000BE       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::get_IsCompleted() /* 0A0000BE */
        IL_0033:  /* 2D   | 41               */ brtrue.s   IL_0076

        IL_0035:  /* 02   |                  */ ldarg.0
        IL_0036:  /* 16   |                  */ ldc.i4.0
        IL_0037:  /* 25   |                  */ dup
        IL_0038:  /* 0A   |                  */ stloc.0
        IL_0039:  /* 7D   | (04)000046       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>1__state' /* 04000046 */
        IL_003e:  /* 02   |                  */ ldarg.0
        IL_003f:  /* 11   | 05               */ ldloc.s    V_5
        IL_0041:  /* 7D   | (04)00004B       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>u__1' /* 0400004B */
        IL_0046:  /* 02   |                  */ ldarg.0
        IL_0047:  /* 7C   | (04)000047       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>t__builder' /* 04000047 */
        IL_004c:  /* 12   | 05               */ ldloca.s   V_5
        IL_004e:  /* 02   |                  */ ldarg.0
        IL_004f:  /* 28   | (2B)00001F       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>,valuetype Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           !!1&) /* 2B00001F */
        IL_0054:  /* DD   | BA000000         */ leave      IL_0113

        IL_0059:  /* 02   |                  */ ldarg.0
        IL_005a:  /* 7B   | (04)00004B       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>u__1' /* 0400004B */
        IL_005f:  /* 13   | 05               */ stloc.s    V_5
        IL_0061:  /* 02   |                  */ ldarg.0
        IL_0062:  /* 7C   | (04)00004B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>u__1' /* 0400004B */
        IL_0067:  /* FE15 | (1B)00001D       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/
        IL_006d:  /* 02   |                  */ ldarg.0
        IL_006e:  /* 15   |                  */ ldc.i4.m1
        IL_006f:  /* 25   |                  */ dup
        IL_0070:  /* 0A   |                  */ stloc.0
        IL_0071:  /* 7D   | (04)000046       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>1__state' /* 04000046 */
        IL_0076:  /* 12   | 05               */ ldloca.s   V_5
        IL_0078:  /* 28   | (0A)0000C0       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::GetResult() /* 0A0000C0 */
        IL_007d:  /* 0C   |                  */ stloc.2
        IL_007e:  /* 08   |                  */ ldloc.2
        IL_007f:  /* 2D   | 09               */ brtrue.s   IL_008a

        IL_0081:  /* 02   |                  */ ldarg.0
        IL_0082:  /* 7B   | (04)000048       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::document /* 04000048 */
        IL_0087:  /* 0B   |                  */ stloc.1
        IL_0088:  /* DE   | 75               */ leave.s    IL_00ff

        IL_008a:  /* 08   |                  */ ldloc.2
        IL_008b:  /* 02   |                  */ ldarg.0
        IL_008c:  /* 7B   | (04)00004A       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::diagnostic /* 0400004A */
        IL_0091:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
        IL_0096:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
        IL_009b:  /* 16   |                  */ ldc.i4.0
        IL_009c:  /* 16   |                  */ ldc.i4.0
        IL_009d:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                        bool,
                                                                                                                                                                                                                                                        bool) /* 0A000039 */
        IL_00a2:  /* 75   | (01)00004D       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/
        IL_00a7:  /* 0D   |                  */ stloc.3
        IL_00a8:  /* 09   |                  */ ldloc.3
        IL_00a9:  /* 2C   | 32               */ brfalse.s  IL_00dd

        IL_00ab:  /* 09   |                  */ ldloc.3
        IL_00ac:  /* 6F   | (0A)0000D2       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax/*01000090*/::get_Type() /* 0A0000D2 */
        IL_00b1:  /* 13   | 04               */ stloc.s    V_4
        IL_00b3:  /* 11   | 04               */ ldloc.s    V_4
        IL_00b5:  /* 2C   | 26               */ brfalse.s  IL_00dd

        IL_00b7:  /* 09   |                  */ ldloc.3
        IL_00b8:  /* 11   | 04               */ ldloc.s    V_4
        IL_00ba:  /* 28   | (0A)000087       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax/*01000085*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::NullableType(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/) /* 0A000087 */
        IL_00bf:  /* 6F   | (0A)00008A       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/::WithType(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/) /* 0A00008A */
        IL_00c4:  /* 13   | 07               */ stloc.s    V_7
        IL_00c6:  /* 02   |                  */ ldarg.0
        IL_00c7:  /* 7B   | (04)000048       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::document /* 04000048 */
        IL_00cc:  /* 08   |                  */ ldloc.2
        IL_00cd:  /* 09   |                  */ ldloc.3
        IL_00ce:  /* 11   | 07               */ ldloc.s    V_7
        IL_00d0:  /* 28   | (2B)000003       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceNode<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>(!!0,
                                                                                                                                                                                                                                                                 class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                                 class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 2B000003 */
        IL_00d5:  /* 6F   | (0A)00003B       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::WithSyntaxRoot(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 0A00003B */
        IL_00da:  /* 0B   |                  */ stloc.1
        IL_00db:  /* DE   | 22               */ leave.s    IL_00ff

        IL_00dd:  /* 02   |                  */ ldarg.0
        IL_00de:  /* 7B   | (04)000048       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::document /* 04000048 */
        IL_00e3:  /* 0B   |                  */ stloc.1
        IL_00e4:  /* DE   | 19               */ leave.s    IL_00ff

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_00e6:  /* 13   | 08               */ stloc.s    V_8
        IL_00e8:  /* 02   |                  */ ldarg.0
        IL_00e9:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_00eb:  /* 7D   | (04)000046       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>1__state' /* 04000046 */
        IL_00f0:  /* 02   |                  */ ldarg.0
        IL_00f1:  /* 7C   | (04)000047       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>t__builder' /* 04000047 */
        IL_00f6:  /* 11   | 08               */ ldloc.s    V_8
        IL_00f8:  /* 28   | (0A)0000CF       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000CF */
        IL_00fd:  /* DE   | 14               */ leave.s    IL_0113

      }  // end handler
      // HEX: 00 00 07 00 DF E6 00 19 40 00 00 01
      IL_00ff:  /* 02   |                  */ ldarg.0
      IL_0100:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_0102:  /* 7D   | (04)000046       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>1__state' /* 04000046 */
      IL_0107:  /* 02   |                  */ ldarg.0
      IL_0108:  /* 7C   | (04)000047       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>t__builder' /* 04000047 */
      IL_010d:  /* 07   |                  */ ldloc.1
      IL_010e:  /* 28   | (0A)0000D0       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::SetResult(!0) /* 0A0000D0 */
      IL_0113:  /* 2A   |                  */ ret
    } // end of method '<FixMismatchedParameterOptionalityAsync>d__5'::MoveNext

    .method /*0600007A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000D5:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x4994
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)000047       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>t__builder' /* 04000047 */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000D1       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000D1 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<FixMismatchedParameterOptionalityAsync>d__5'::SetStateMachine

  } // end of class '<FixMismatchedParameterOptionalityAsync>d__5'

  .field /*0400001A*/ private initonly valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> '<FixableDiagnosticIds>k__BackingField'
  .custom /*0C000077:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*06000044*/ public hidebysig specialname virtual 
          instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> 
          get_FixableDiagnosticIds() cil managed
  // SIG: 20 00 15 11 80 91 01 0E
  {
    .custom /*0C0000BF:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .param [0]/*08000069*/ 
    .custom /*0C0000D0:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x35ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00001A       */ ldfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*/::'<FixableDiagnosticIds>k__BackingField' /* 0400001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DetectMismatchedParameterOptionalityFixer::get_FixableDiagnosticIds

  .method /*06000045*/ public hidebysig virtual final 
          instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ 
          GetFixAllProvider() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x35b4
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 28   | (0A)000033       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders/*0100006F*/::get_BatchFixer() /* 0A000033 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method DetectMismatchedParameterOptionalityFixer::GetFixAllProvider

  .method /*06000046*/ public hidebysig virtual final 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          RegisterCodeFixesAsync(valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context) cil managed
  // SIG: 20 01 12 80 A1 11 80 A5
  {
    // Method begins at RVA 0x35bc
    // Code size       130 (0x82)
    .maxstack  4
    .locals /*11000018*/ init (class Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/ V_0,
             valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_1,
             valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_2,
             class Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/ V_3)
    IL_0000:  /* 73   | (06)000076       */ newobj     instance void Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/::.ctor() /* 06000076 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 06   |                  */ ldloc.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000043       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/::context /* 04000043 */
    IL_000d:  /* 06   |                  */ ldloc.0
    IL_000e:  /* 7C   | (04)000043       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/::context /* 04000043 */
    IL_0013:  /* 28   | (0A)000068       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Diagnostics() /* 0A000068 */
    IL_0018:  /* 0C   |                  */ stloc.2
    IL_0019:  /* 12   | 02               */ ldloca.s   V_2
    IL_001b:  /* 28   | (0A)000069       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<!0> valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000010*/::GetEnumerator() /* 0A000069 */
    IL_0020:  /* 0B   |                  */ stloc.1
    IL_0021:  /* 2B   | 50               */ br.s       IL_0073

    IL_0023:  /* 73   | (06)000077       */ newobj     instance void Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/::.ctor() /* 06000077 */
    IL_0028:  /* 0D   |                  */ stloc.3
    IL_0029:  /* 09   |                  */ ldloc.3
    IL_002a:  /* 06   |                  */ ldloc.0
    IL_002b:  /* 7D   | (04)000045       */ stfld      class Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/::'CS$<>8__locals1' /* 04000045 */
    IL_0030:  /* 09   |                  */ ldloc.3
    IL_0031:  /* 12   | 01               */ ldloca.s   V_1
    IL_0033:  /* 28   | (0A)00006A       */ call       instance !0 valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::get_Current() /* 0A00006A */
    IL_0038:  /* 7D   | (04)000044       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/::diagnostic /* 04000044 */
    IL_003d:  /* 09   |                  */ ldloc.3
    IL_003e:  /* 7B   | (04)000045       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/::'CS$<>8__locals1' /* 04000045 */
    IL_0043:  /* 7C   | (04)000043       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_0'/*0200001F*/::context /* 04000043 */
    IL_0048:  /* 72   | (70)000227       */ ldstr      "Fix mismatched route parameter and argument option"
    + "ality" /* 70000227 */
    IL_004d:  /* 09   |                  */ ldloc.3
    IL_004e:  /* FE06 | (06)000078       */ ldftn      instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/::'<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 06000078 */
    IL_0054:  /* 73   | (0A)00006E       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>/*1B000013*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                                                               native int) /* 0A00006E */
    IL_0059:  /* 7E   | (0A)000077       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DetectMismatchedParameterOptionality /* 0A000077 */
    IL_005e:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_0063:  /* 28   | (0A)00006F       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/::Create(string,
                                                                                                                                                                                                                                                                                       class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>,
                                                                                                                                                                                                                                                                                       string) /* 0A00006F */
    IL_0068:  /* 09   |                  */ ldloc.3
    IL_0069:  /* 7B   | (04)000044       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<>c__DisplayClass4_1'/*02000020*/::diagnostic /* 04000044 */
    IL_006e:  /* 28   | (0A)000070       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::RegisterCodeFix(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/,
                                                                                                                                                                                                 class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/) /* 0A000070 */
    IL_0073:  /* 12   | 01               */ ldloca.s   V_1
    IL_0075:  /* 28   | (0A)000071       */ call       instance bool valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::MoveNext() /* 0A000071 */
    IL_007a:  /* 2D   | A7               */ brtrue.s   IL_0023

    IL_007c:  /* 28   | (0A)000072       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/::get_CompletedTask() /* 0A000072 */
    IL_0081:  /* 2A   |                  */ ret
  } // end of method DetectMismatchedParameterOptionalityFixer::RegisterCodeFixesAsync

  .method /*06000047*/ private hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
          FixMismatchedParameterOptionalityAsync(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                                 class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                                                 valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
  // SIG: 00 03 15 12 80 A9 01 12 80 AD 12 80 B1 12 80 AD 11 80 ED
  {
    .custom /*0C0000C0:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 80 8A 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ....Microsoft.As
                                                                                                                                                                                                                                  70 4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65   // pNetCore.Analyze
                                                                                                                                                                                                                                  72 73 2E 52 6F 75 74 65 48 61 6E 64 6C 65 72 73   // rs.RouteHandlers
                                                                                                                                                                                                                                  2E 46 69 78 65 72 73 2E 44 65 74 65 63 74 4D 69   // .Fixers.DetectMi
                                                                                                                                                                                                                                  73 6D 61 74 63 68 65 64 50 61 72 61 6D 65 74 65   // smatchedParamete
                                                                                                                                                                                                                                  72 4F 70 74 69 6F 6E 61 6C 69 74 79 46 69 78 65   // rOptionalityFixe
                                                                                                                                                                                                                                  72 2B 3C 46 69 78 4D 69 73 6D 61 74 63 68 65 64   // r+<FixMismatched
                                                                                                                                                                                                                                  50 61 72 61 6D 65 74 65 72 4F 70 74 69 6F 6E 61   // ParameterOptiona
                                                                                                                                                                                                                                  6C 69 74 79 41 73 79 6E 63 3E 64 5F 5F 35 00 00 ) // lityAsync>d__5..
    // Method begins at RVA 0x364c
    // Code size       71 (0x47)
    .maxstack  2
    .locals /*11000019*/ init (valuetype Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)000073       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::Create() /* 0A000073 */
    IL_0007:  /* 7D   | (04)000047       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>t__builder' /* 04000047 */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7D   | (04)00004A       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::diagnostic /* 0400004A */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 03   |                  */ ldarg.1
    IL_0017:  /* 7D   | (04)000048       */ stfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::document /* 04000048 */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 04   |                  */ ldarg.2
    IL_001f:  /* 7D   | (04)000049       */ stfld      valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::cancellationToken /* 04000049 */
    IL_0024:  /* 12   | 00               */ ldloca.s   V_0
    IL_0026:  /* 15   |                  */ ldc.i4.m1
    IL_0027:  /* 7D   | (04)000046       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>1__state' /* 04000046 */
    IL_002c:  /* 12   | 00               */ ldloca.s   V_0
    IL_002e:  /* 7C   | (04)000047       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>t__builder' /* 04000047 */
    IL_0033:  /* 12   | 00               */ ldloca.s   V_0
    IL_0035:  /* 28   | (2B)00000F       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::Start<valuetype Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/>(!!0&) /* 2B00000F */
    IL_003a:  /* 12   | 00               */ ldloca.s   V_0
    IL_003c:  /* 7C   | (04)000047       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*//'<FixMismatchedParameterOptionalityAsync>d__5'/*02000021*/::'<>t__builder' /* 04000047 */
    IL_0041:  /* 28   | (0A)000075       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>/*1B000014*/::get_Task() /* 0A000075 */
    IL_0046:  /* 2A   |                  */ ret
  } // end of method DetectMismatchedParameterOptionalityFixer::FixMismatchedParameterOptionalityAsync

  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x369f
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (0A)000077       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::DetectMismatchedParameterOptionality /* 0A000077 */
    IL_0006:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_000b:  /* 28   | (2B)00000D       */ call       valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<!!0> [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray/*01000078*/::Create<string>(!!0) /* 2B00000D */
    IL_0010:  /* 7D   | (04)00001A       */ stfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*/::'<FixableDiagnosticIds>k__BackingField' /* 0400001A */
    IL_0015:  /* 02   |                  */ ldarg.0
    IL_0016:  /* 28   | (0A)000058       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/::.ctor() /* 0A000058 */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method DetectMismatchedParameterOptionalityFixer::.ctor

  .property /*17000008*/ instance callconv(8) valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>
          FixableDiagnosticIds()
  {
    .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer/*0200000F*/::get_FixableDiagnosticIds() /* 06000044 */
  } // end of property DetectMismatchedParameterOptionalityFixer::FixableDiagnosticIds
} // end of class Microsoft.AspNetCore.Analyzers.RouteHandlers.Fixers.DetectMismatchedParameterOptionalityFixer

.class /*02000010*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider
       extends [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionProvider/*01000050*/
{
  .custom /*0C000054:0A00001B*/ instance void [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.ExportCompletionProviderAttribute/*0100004F*/::.ctor(string,
                                                                                                                                                                                  string) /* 0A00001B */ = ( 01 00 22 45 78 74 65 6E 73 69 6F 6E 4D 65 74 68   // .."ExtensionMeth
                                                                                                                                                                                                             6F 64 73 43 6F 6D 70 6C 65 74 69 6F 6E 50 72 6F   // odsCompletionPro
                                                                                                                                                                                                             76 69 64 65 72 02 43 23 00 00 )                   // vider.C#..
  .custom /*0C000055:0A000015*/ instance void [System.Composition.AttributedModel/*23000005*/]System.Composition.SharedAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*02000022*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass0_0'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C000095:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*0400004C*/ public valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ token
    .field /*0400004D*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ symbolType
    .method /*0600007B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x49a2
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass0_0'::.ctor

    .method /*0600007C*/ assembly hidebysig 
            instance bool  '<ProvideCompletionsAsync>b__0'(valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> pair) cil managed
    // SIG: 20 01 02 15 11 81 49 02 11 2C 11 28
    {
      // Method begins at RVA 0x49aa
      // Code size       19 (0x13)
      .maxstack  8
      IL_0000:  /* 03   |                  */ ldarg.1
      IL_0001:  /* 02   |                  */ ldarg.0
      IL_0002:  /* 7B   | (04)00004D       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/::symbolType /* 0400004D */
      IL_0007:  /* 02   |                  */ ldarg.0
      IL_0008:  /* 7B   | (04)00004C       */ ldfld      valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/::token /* 0400004C */
      IL_000d:  /* 28   | (06)00004B       */ call       bool Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*/::IsMatchingExtensionMethod(valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>,
                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/,
                                                                                                                                                                                             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/) /* 0600004B */
      IL_0012:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass0_0'::'<ProvideCompletionsAsync>b__0'

  } // end of class '<>c__DisplayClass0_0'

  .class /*02000023*/ auto ansi sealed nested private beforefieldinit '<ProvideCompletionsAsync>d__0'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000098:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*0400004E*/ public int32 '<>1__state'
    .field /*0400004F*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ '<>t__builder'
    .field /*04000050*/ public class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ context
    .field /*04000051*/ private class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ '<>8__1'
    .field /*04000052*/ private valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ '<span>5__2'
    .field /*04000053*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> '<>u__1'
    .field /*04000054*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> '<>u__2'
    .method /*0600007D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x49c0
      // Code size       808 (0x328)
      .maxstack  11
      .locals /*1100002A*/ init (int32 V_0,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_1,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ V_2,
               class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> V_3,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_4,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_5,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_6,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_7,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_8,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_9,
               valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolInfo/*01000054*/ V_10,
               valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ V_11,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IMethodSymbol/*01000055*/ V_12,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IPropertySymbol/*01000056*/ V_13,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ILocalSymbol/*01000057*/ V_14,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ V_15,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/ V_16,
               class [netstandard/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>> V_17,
               valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> V_18,
               string V_19,
               valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/ V_20,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> V_21,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_22)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)00004E       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>1__state' /* 0400004E */
      IL_0006:  /* 0A   |                  */ stloc.0
      .try
      {
        IL_0007:  /* 06   |                  */ ldloc.0
        IL_0008:  /* 39   | 82000000         */ brfalse    IL_008f

        IL_000d:  /* 06   |                  */ ldloc.0
        IL_000e:  /* 17   |                  */ ldc.i4.1
        IL_000f:  /* 3B   | 46010000         */ beq        IL_015a

        IL_0014:  /* 02   |                  */ ldarg.0
        IL_0015:  /* 73   | (06)00007B       */ newobj     instance void Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/::.ctor() /* 0600007B */
        IL_001a:  /* 7D   | (04)000051       */ stfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>8__1' /* 04000051 */
        IL_001f:  /* 02   |                  */ ldarg.0
        IL_0020:  /* 7B   | (04)000050       */ ldfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
        IL_0025:  /* 6F   | (0A)0000D3       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/::get_Document() /* 0A0000D3 */
        IL_002a:  /* 6F   | (0A)0000D4       */ callvirt   instance bool [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::get_SupportsSemanticModel() /* 0A0000D4 */
        IL_002f:  /* 2D   | 05               */ brtrue.s   IL_0036

        IL_0031:  /* DD   | D7020000         */ leave      IL_030d

        IL_0036:  /* 02   |                  */ ldarg.0
        IL_0037:  /* 7B   | (04)000050       */ ldfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
        IL_003c:  /* 6F   | (0A)0000D3       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/::get_Document() /* 0A0000D3 */
        IL_0041:  /* 02   |                  */ ldarg.0
        IL_0042:  /* 7B   | (04)000050       */ ldfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
        IL_0047:  /* 6F   | (0A)0000D5       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/::get_CancellationToken() /* 0A0000D5 */
        IL_004c:  /* 6F   | (0A)0000BB       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSyntaxRootAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000BB */
        IL_0051:  /* 16   |                  */ ldc.i4.0
        IL_0052:  /* 6F   | (0A)0000BC       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001B*/::ConfigureAwait(bool) /* 0A0000BC */
        IL_0057:  /* 13   | 07               */ stloc.s    V_7
        IL_0059:  /* 12   | 07               */ ldloca.s   V_7
        IL_005b:  /* 28   | (0A)0000BD       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001C*/::GetAwaiter() /* 0A0000BD */
        IL_0060:  /* 13   | 06               */ stloc.s    V_6
        IL_0062:  /* 12   | 06               */ ldloca.s   V_6
        IL_0064:  /* 28   | (0A)0000BE       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::get_IsCompleted() /* 0A0000BE */
        IL_0069:  /* 2D   | 41               */ brtrue.s   IL_00ac

        IL_006b:  /* 02   |                  */ ldarg.0
        IL_006c:  /* 16   |                  */ ldc.i4.0
        IL_006d:  /* 25   |                  */ dup
        IL_006e:  /* 0A   |                  */ stloc.0
        IL_006f:  /* 7D   | (04)00004E       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>1__state' /* 0400004E */
        IL_0074:  /* 02   |                  */ ldarg.0
        IL_0075:  /* 11   | 06               */ ldloc.s    V_6
        IL_0077:  /* 7D   | (04)000053       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>u__1' /* 04000053 */
        IL_007c:  /* 02   |                  */ ldarg.0
        IL_007d:  /* 7C   | (04)00004F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>t__builder' /* 0400004F */
        IL_0082:  /* 12   | 06               */ ldloca.s   V_6
        IL_0084:  /* 02   |                  */ ldarg.0
        IL_0085:  /* 28   | (2B)000020       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>,valuetype Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            !!1&) /* 2B000020 */
        IL_008a:  /* DD   | 98020000         */ leave      IL_0327

        IL_008f:  /* 02   |                  */ ldarg.0
        IL_0090:  /* 7B   | (04)000053       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>u__1' /* 04000053 */
        IL_0095:  /* 13   | 06               */ stloc.s    V_6
        IL_0097:  /* 02   |                  */ ldarg.0
        IL_0098:  /* 7C   | (04)000053       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>u__1' /* 04000053 */
        IL_009d:  /* FE15 | (1B)00001D       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/
        IL_00a3:  /* 02   |                  */ ldarg.0
        IL_00a4:  /* 15   |                  */ ldc.i4.m1
        IL_00a5:  /* 25   |                  */ dup
        IL_00a6:  /* 0A   |                  */ stloc.0
        IL_00a7:  /* 7D   | (04)00004E       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>1__state' /* 0400004E */
        IL_00ac:  /* 12   | 06               */ ldloca.s   V_6
        IL_00ae:  /* 28   | (0A)0000C0       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::GetResult() /* 0A0000C0 */
        IL_00b3:  /* 0B   |                  */ stloc.1
        IL_00b4:  /* 07   |                  */ ldloc.1
        IL_00b5:  /* 2D   | 05               */ brtrue.s   IL_00bc

        IL_00b7:  /* DD   | 51020000         */ leave      IL_030d

        IL_00bc:  /* 02   |                  */ ldarg.0
        IL_00bd:  /* 02   |                  */ ldarg.0
        IL_00be:  /* 7B   | (04)000050       */ ldfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
        IL_00c3:  /* 6F   | (0A)0000D6       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/::get_CompletionListSpan() /* 0A0000D6 */
        IL_00c8:  /* 7D   | (04)000052       */ stfld      valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<span>5__2' /* 04000052 */
        IL_00cd:  /* 02   |                  */ ldarg.0
        IL_00ce:  /* 7B   | (04)000051       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>8__1' /* 04000051 */
        IL_00d3:  /* 07   |                  */ ldloc.1
        IL_00d4:  /* 02   |                  */ ldarg.0
        IL_00d5:  /* 7C   | (04)000052       */ ldflda     valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<span>5__2' /* 04000052 */
        IL_00da:  /* 28   | (0A)0000D7       */ call       instance int32 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/::get_Start() /* 0A0000D7 */
        IL_00df:  /* 16   |                  */ ldc.i4.0
        IL_00e0:  /* 6F   | (0A)0000D8       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindToken(int32,
                                                                                                                                                                                                                                                              bool) /* 0A0000D8 */
        IL_00e5:  /* 7D   | (04)00004C       */ stfld      valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/::token /* 0400004C */
        IL_00ea:  /* 02   |                  */ ldarg.0
        IL_00eb:  /* 7B   | (04)000051       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>8__1' /* 04000051 */
        IL_00f0:  /* 7C   | (04)00004C       */ ldflda     valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/::token /* 0400004C */
        IL_00f5:  /* 28   | (0A)0000D9       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/::get_Parent() /* 0A0000D9 */
        IL_00fa:  /* 2D   | 05               */ brtrue.s   IL_0101

        IL_00fc:  /* DD   | 0C020000         */ leave      IL_030d

        IL_0101:  /* 02   |                  */ ldarg.0
        IL_0102:  /* 7B   | (04)000050       */ ldfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
        IL_0107:  /* 6F   | (0A)0000D3       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/::get_Document() /* 0A0000D3 */
        IL_010c:  /* 02   |                  */ ldarg.0
        IL_010d:  /* 7B   | (04)000050       */ ldfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
        IL_0112:  /* 6F   | (0A)0000D5       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/::get_CancellationToken() /* 0A0000D5 */
        IL_0117:  /* 6F   | (0A)0000C6       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSemanticModelAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000C6 */
        IL_011c:  /* 16   |                  */ ldc.i4.0
        IL_011d:  /* 6F   | (0A)0000C7       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001E*/::ConfigureAwait(bool) /* 0A0000C7 */
        IL_0122:  /* 13   | 09               */ stloc.s    V_9
        IL_0124:  /* 12   | 09               */ ldloca.s   V_9
        IL_0126:  /* 28   | (0A)0000C8       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001F*/::GetAwaiter() /* 0A0000C8 */
        IL_012b:  /* 13   | 08               */ stloc.s    V_8
        IL_012d:  /* 12   | 08               */ ldloca.s   V_8
        IL_012f:  /* 28   | (0A)0000C9       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::get_IsCompleted() /* 0A0000C9 */
        IL_0134:  /* 2D   | 41               */ brtrue.s   IL_0177

        IL_0136:  /* 02   |                  */ ldarg.0
        IL_0137:  /* 17   |                  */ ldc.i4.1
        IL_0138:  /* 25   |                  */ dup
        IL_0139:  /* 0A   |                  */ stloc.0
        IL_013a:  /* 7D   | (04)00004E       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>1__state' /* 0400004E */
        IL_013f:  /* 02   |                  */ ldarg.0
        IL_0140:  /* 11   | 08               */ ldloc.s    V_8
        IL_0142:  /* 7D   | (04)000054       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>u__2' /* 04000054 */
        IL_0147:  /* 02   |                  */ ldarg.0
        IL_0148:  /* 7C   | (04)00004F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>t__builder' /* 0400004F */
        IL_014d:  /* 12   | 08               */ ldloca.s   V_8
        IL_014f:  /* 02   |                  */ ldarg.0
        IL_0150:  /* 28   | (2B)000021       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>,valuetype Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               !!1&) /* 2B000021 */
        IL_0155:  /* DD   | CD010000         */ leave      IL_0327

        IL_015a:  /* 02   |                  */ ldarg.0
        IL_015b:  /* 7B   | (04)000054       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>u__2' /* 04000054 */
        IL_0160:  /* 13   | 08               */ stloc.s    V_8
        IL_0162:  /* 02   |                  */ ldarg.0
        IL_0163:  /* 7C   | (04)000054       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>u__2' /* 04000054 */
        IL_0168:  /* FE15 | (1B)000020       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/
        IL_016e:  /* 02   |                  */ ldarg.0
        IL_016f:  /* 15   |                  */ ldc.i4.m1
        IL_0170:  /* 25   |                  */ dup
        IL_0171:  /* 0A   |                  */ stloc.0
        IL_0172:  /* 7D   | (04)00004E       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>1__state' /* 0400004E */
        IL_0177:  /* 12   | 08               */ ldloca.s   V_8
        IL_0179:  /* 28   | (0A)0000CA       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::GetResult() /* 0A0000CA */
        IL_017e:  /* 0C   |                  */ stloc.2
        IL_017f:  /* 08   |                  */ ldloc.2
        IL_0180:  /* 2D   | 05               */ brtrue.s   IL_0187

        IL_0182:  /* DD   | 86010000         */ leave      IL_030d

        IL_0187:  /* 08   |                  */ ldloc.2
        IL_0188:  /* 6F   | (0A)0000CB       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Compilation/*0100008F*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/::get_Compilation() /* 0A0000CB */
        IL_018d:  /* 28   | (0A)0000CC       */ call       class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/::GetOrCreate(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Compilation/*0100008F*/) /* 0A0000CC */
        IL_0192:  /* 28   | (06)000008       */ call       class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> ExtensionMethodsCache/*02000008*/::ConstructFromWellKnownTypes(class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/) /* 06000008 */
        IL_0197:  /* 0D   |                  */ stloc.3
        IL_0198:  /* 02   |                  */ ldarg.0
        IL_0199:  /* 7B   | (04)000051       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>8__1' /* 04000051 */
        IL_019e:  /* 7C   | (04)00004C       */ ldflda     valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/::token /* 0400004C */
        IL_01a3:  /* 28   | (0A)0000D9       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/::get_Parent() /* 0A0000D9 */
        IL_01a8:  /* 28   | (06)00004A       */ call       class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*/::FindNearestMemberAccessExpression(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 0600004A */
        IL_01ad:  /* 13   | 04               */ stloc.s    V_4
        IL_01af:  /* 11   | 04               */ ldloc.s    V_4
        IL_01b1:  /* 39   | 35010000         */ brfalse    IL_02eb

        IL_01b6:  /* 11   | 04               */ ldloc.s    V_4
        IL_01b8:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
        IL_01bd:  /* 13   | 05               */ stloc.s    V_5
        IL_01bf:  /* 11   | 05               */ ldloc.s    V_5
        IL_01c1:  /* 39   | 25010000         */ brfalse    IL_02eb

        IL_01c6:  /* 08   |                  */ ldloc.2
        IL_01c7:  /* 11   | 05               */ ldloc.s    V_5
        IL_01c9:  /* 6F   | (0A)00003E       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Expression() /* 0A00003E */
        IL_01ce:  /* 12   | 0B               */ ldloca.s   V_11
        IL_01d0:  /* FE15 | (01)00003B       */ initobj    [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/
        IL_01d6:  /* 11   | 0B               */ ldloc.s    V_11
        IL_01d8:  /* 28   | (0A)0000DA       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolInfo/*01000054*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.CSharpExtensions/*01000091*/::GetSymbolInfo(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/,
                                                                                                                                                                                                                                                                            class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/,
                                                                                                                                                                                                                                                                            valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000DA */
        IL_01dd:  /* 13   | 0A               */ stloc.s    V_10
        IL_01df:  /* 12   | 0A               */ ldloca.s   V_10
        IL_01e1:  /* 28   | (0A)0000DB       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolInfo/*01000054*/::get_Symbol() /* 0A0000DB */
        IL_01e6:  /* 13   | 10               */ stloc.s    V_16
        IL_01e8:  /* 11   | 10               */ ldloc.s    V_16
        IL_01ea:  /* 75   | (01)000055       */ isinst     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IMethodSymbol/*01000055*/
        IL_01ef:  /* 13   | 0C               */ stloc.s    V_12
        IL_01f1:  /* 11   | 0C               */ ldloc.s    V_12
        IL_01f3:  /* 2D   | 1C               */ brtrue.s   IL_0211

        IL_01f5:  /* 11   | 10               */ ldloc.s    V_16
        IL_01f7:  /* 75   | (01)000056       */ isinst     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IPropertySymbol/*01000056*/
        IL_01fc:  /* 13   | 0D               */ stloc.s    V_13
        IL_01fe:  /* 11   | 0D               */ ldloc.s    V_13
        IL_0200:  /* 2D   | 1A               */ brtrue.s   IL_021c

        IL_0202:  /* 11   | 10               */ ldloc.s    V_16
        IL_0204:  /* 75   | (01)000057       */ isinst     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ILocalSymbol/*01000057*/
        IL_0209:  /* 13   | 0E               */ stloc.s    V_14
        IL_020b:  /* 11   | 0E               */ ldloc.s    V_14
        IL_020d:  /* 2D   | 18               */ brtrue.s   IL_0227

        IL_020f:  /* 2B   | 21               */ br.s       IL_0232

        IL_0211:  /* 11   | 0C               */ ldloc.s    V_12
        IL_0213:  /* 6F   | (0A)0000DC       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IMethodSymbol/*01000055*/::get_ReturnType() /* 0A0000DC */
        IL_0218:  /* 13   | 0F               */ stloc.s    V_15
        IL_021a:  /* 2B   | 19               */ br.s       IL_0235

        IL_021c:  /* 11   | 0D               */ ldloc.s    V_13
        IL_021e:  /* 6F   | (0A)0000DD       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IPropertySymbol/*01000056*/::get_Type() /* 0A0000DD */
        IL_0223:  /* 13   | 0F               */ stloc.s    V_15
        IL_0225:  /* 2B   | 0E               */ br.s       IL_0235

        IL_0227:  /* 11   | 0E               */ ldloc.s    V_14
        IL_0229:  /* 6F   | (0A)0000DE       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ILocalSymbol/*01000057*/::get_Type() /* 0A0000DE */
        IL_022e:  /* 13   | 0F               */ stloc.s    V_15
        IL_0230:  /* 2B   | 03               */ br.s       IL_0235

        IL_0232:  /* 14   |                  */ ldnull
        IL_0233:  /* 13   | 0F               */ stloc.s    V_15
        IL_0235:  /* 02   |                  */ ldarg.0
        IL_0236:  /* 7B   | (04)000051       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>8__1' /* 04000051 */
        IL_023b:  /* 11   | 0F               */ ldloc.s    V_15
        IL_023d:  /* 7D   | (04)00004D       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/::symbolType /* 0400004D */
        IL_0242:  /* 09   |                  */ ldloc.3
        IL_0243:  /* 02   |                  */ ldarg.0
        IL_0244:  /* 7B   | (04)000051       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>8__1' /* 04000051 */
        IL_0249:  /* FE06 | (06)00007C       */ ldftn      instance bool Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/::'<ProvideCompletionsAsync>b__0'(valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>) /* 0600007C */
        IL_024f:  /* 73   | (0A)0000DF       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>,bool>/*1B000021*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                                                                native int) /* 0A0000DF */
        IL_0254:  /* 28   | (2B)000022       */ call       class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<!!0> [netstandard/*23000001*/]System.Linq.Enumerable/*01000074*/::Where<valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>>(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<!!0>,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                             class [netstandard/*23000001*/]System.Func`2/*01000066*/<!!0,bool>) /* 2B000022 */
        IL_0259:  /* 6F   | (0A)0000E0       */ callvirt   instance class [netstandard/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<!0> class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>>/*1B000022*/::GetEnumerator() /* 0A0000E0 */
        IL_025e:  /* 13   | 11               */ stloc.s    V_17
        .try
        {
          IL_0260:  /* 2B   | 6E               */ br.s       IL_02d0

          IL_0262:  /* 11   | 11               */ ldloc.s    V_17
          IL_0264:  /* 6F   | (0A)0000E1       */ callvirt   instance !0 class [netstandard/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>>/*1B000023*/::get_Current() /* 0A0000E1 */
          IL_0269:  /* 13   | 12               */ stloc.s    V_18
          IL_026b:  /* 02   |                  */ ldarg.0
          IL_026c:  /* 7B   | (04)000050       */ ldfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
          IL_0271:  /* 02   |                  */ ldarg.0
          IL_0272:  /* 7B   | (04)000052       */ ldfld      valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<span>5__2' /* 04000052 */
          IL_0277:  /* 6F   | (0A)0000E2       */ callvirt   instance void [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/::set_CompletionListSpan(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/) /* 0A0000E2 */
          IL_027c:  /* 02   |                  */ ldarg.0
          IL_027d:  /* 7B   | (04)000050       */ ldfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
          IL_0282:  /* 12   | 12               */ ldloca.s   V_18
          IL_0284:  /* 28   | (0A)00007B       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000015*/::get_Key() /* 0A00007B */
          IL_0289:  /* 13   | 14               */ stloc.s    V_20
          IL_028b:  /* 12   | 14               */ ldloca.s   V_20
          IL_028d:  /* 28   | (06)00002E       */ call       instance string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ExtensionMethod() /* 0600002E */
          IL_0292:  /* 12   | 12               */ ldloca.s   V_18
          IL_0294:  /* 28   | (0A)00007B       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000015*/::get_Key() /* 0A00007B */
          IL_0299:  /* 13   | 14               */ stloc.s    V_20
          IL_029b:  /* 12   | 14               */ ldloca.s   V_20
          IL_029d:  /* 28   | (06)00002E       */ call       instance string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ExtensionMethod() /* 0600002E */
          IL_02a2:  /* 13   | 13               */ stloc.s    V_19
          IL_02a4:  /* 12   | 12               */ ldloca.s   V_18
          IL_02a6:  /* 28   | (0A)00007B       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000015*/::get_Key() /* 0A00007B */
          IL_02ab:  /* 13   | 14               */ stloc.s    V_20
          IL_02ad:  /* 12   | 14               */ ldloca.s   V_20
          IL_02af:  /* 28   | (06)00002E       */ call       instance string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ExtensionMethod() /* 0600002E */
          IL_02b4:  /* 11   | 13               */ ldloc.s    V_19
          IL_02b6:  /* 14   |                  */ ldnull
          IL_02b7:  /* 12   | 15               */ ldloca.s   V_21
          IL_02b9:  /* FE15 | (1B)000024       */ initobj    valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>/*1B000024*/
          IL_02bf:  /* 11   | 15               */ ldloc.s    V_21
          IL_02c1:  /* 14   |                  */ ldnull
          IL_02c2:  /* 14   |                  */ ldnull
          IL_02c3:  /* 14   |                  */ ldnull
          IL_02c4:  /* 14   |                  */ ldnull
          IL_02c5:  /* 16   |                  */ ldc.i4.0
          IL_02c6:  /* 28   | (0A)0000E3       */ call       class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionItem/*01000092*/ [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionItem/*01000092*/::Create(string,
                                                                                                                                                                                                                                                                                               string,
                                                                                                                                                                                                                                                                                               string,
                                                                                                                                                                                                                                                                                               class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string>,
                                                                                                                                                                                                                                                                                               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>,
                                                                                                                                                                                                                                                                                               class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionItemRules/*01000093*/,
                                                                                                                                                                                                                                                                                               string,
                                                                                                                                                                                                                                                                                               string,
                                                                                                                                                                                                                                                                                               string,
                                                                                                                                                                                                                                                                                               bool) /* 0A0000E3 */
          IL_02cb:  /* 6F   | (0A)0000E4       */ callvirt   instance void [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/::AddItem(class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionItem/*01000092*/) /* 0A0000E4 */
          IL_02d0:  /* 11   | 11               */ ldloc.s    V_17
          IL_02d2:  /* 6F   | (0A)0000AB       */ callvirt   instance bool [netstandard/*23000001*/]System.Collections.IEnumerator/*0100008D*/::MoveNext() /* 0A0000AB */
          IL_02d7:  /* 2D   | 89               */ brtrue.s   IL_0262

          IL_02d9:  /* DE   | 10               */ leave.s    IL_02eb

        }  // end .try
        finally
        {
          IL_02db:  /* 06   |                  */ ldloc.0
          IL_02dc:  /* 16   |                  */ ldc.i4.0
          IL_02dd:  /* 2F   | 0B               */ bge.s      IL_02ea

          IL_02df:  /* 11   | 11               */ ldloc.s    V_17
          IL_02e1:  /* 2C   | 07               */ brfalse.s  IL_02ea

          IL_02e3:  /* 11   | 11               */ ldloc.s    V_17
          IL_02e5:  /* 6F   | (0A)000023       */ callvirt   instance void [netstandard/*23000001*/]System.IDisposable/*0100006A*/::Dispose() /* 0A000023 */
          IL_02ea:  /* DC   |                  */ endfinally
        }  // end handler
        // HEX: 02 00 00 00 60 02 00 00 7B 00 00 00 DB 02 00 00 10 00 00 00 00 00 00 00
        IL_02eb:  /* DE   | 20               */ leave.s    IL_030d

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_02ed:  /* 13   | 16               */ stloc.s    V_22
        IL_02ef:  /* 02   |                  */ ldarg.0
        IL_02f0:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_02f2:  /* 7D   | (04)00004E       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>1__state' /* 0400004E */
        IL_02f7:  /* 02   |                  */ ldarg.0
        IL_02f8:  /* 14   |                  */ ldnull
        IL_02f9:  /* 7D   | (04)000051       */ stfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>8__1' /* 04000051 */
        IL_02fe:  /* 02   |                  */ ldarg.0
        IL_02ff:  /* 7C   | (04)00004F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>t__builder' /* 0400004F */
        IL_0304:  /* 11   | 16               */ ldloc.s    V_22
        IL_0306:  /* 28   | (0A)0000C3       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000C3 */
        IL_030b:  /* DE   | 1A               */ leave.s    IL_0327

      }  // end handler
      // HEX: 00 00 00 00 07 00 00 00 E6 02 00 00 ED 02 00 00 20 00 00 00 40 00 00 01
      IL_030d:  /* 02   |                  */ ldarg.0
      IL_030e:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_0310:  /* 7D   | (04)00004E       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>1__state' /* 0400004E */
      IL_0315:  /* 02   |                  */ ldarg.0
      IL_0316:  /* 14   |                  */ ldnull
      IL_0317:  /* 7D   | (04)000051       */ stfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<>c__DisplayClass0_0'/*02000022*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>8__1' /* 04000051 */
      IL_031c:  /* 02   |                  */ ldarg.0
      IL_031d:  /* 7C   | (04)00004F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>t__builder' /* 0400004F */
      IL_0322:  /* 28   | (0A)0000C4       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetResult() /* 0A0000C4 */
      IL_0327:  /* 2A   |                  */ ret
    } // end of method '<ProvideCompletionsAsync>d__0'::MoveNext

    .method /*0600007E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000D6:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x4d28
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)00004F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>t__builder' /* 0400004F */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000C5       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000C5 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<ProvideCompletionsAsync>d__0'::SetStateMachine

  } // end of class '<ProvideCompletionsAsync>d__0'

  .method /*06000049*/ public hidebysig virtual 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          ProvideCompletionsAsync(class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ context) cil managed
  // SIG: 20 01 12 80 A1 12 81 45
  {
    .custom /*0C0000C2:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000C3:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 75 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..uMicrosoft.Asp
                                                                                                                                                                                                                                  4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65 72   // NetCore.Analyzer
                                                                                                                                                                                                                                  73 2E 52 6F 75 74 65 45 6D 62 65 64 64 65 64 4C   // s.RouteEmbeddedL
                                                                                                                                                                                                                                  61 6E 67 75 61 67 65 2E 45 78 74 65 6E 73 69 6F   // anguage.Extensio
                                                                                                                                                                                                                                  6E 4D 65 74 68 6F 64 73 43 6F 6D 70 6C 65 74 69   // nMethodsCompleti
                                                                                                                                                                                                                                  6F 6E 50 72 6F 76 69 64 65 72 2B 3C 50 72 6F 76   // onProvider+<Prov
                                                                                                                                                                                                                                  69 64 65 43 6F 6D 70 6C 65 74 69 6F 6E 73 41 73   // ideCompletionsAs
                                                                                                                                                                                                                                  79 6E 63 3E 64 5F 5F 30 00 00 )                   // ync>d__0..
    // Method begins at RVA 0x36bc
    // Code size       55 (0x37)
    .maxstack  2
    .locals /*1100001A*/ init (valuetype Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)000034       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Create() /* 0A000034 */
    IL_0007:  /* 7D   | (04)00004F       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>t__builder' /* 0400004F */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 7D   | (04)000050       */ stfld      class [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionContext/*01000051*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::context /* 04000050 */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 15   |                  */ ldc.i4.m1
    IL_0017:  /* 7D   | (04)00004E       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>1__state' /* 0400004E */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 7C   | (04)00004F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>t__builder' /* 0400004F */
    IL_0023:  /* 12   | 00               */ ldloca.s   V_0
    IL_0025:  /* 28   | (2B)000010       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Start<valuetype Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/>(!!0&) /* 2B000010 */
    IL_002a:  /* 12   | 00               */ ldloca.s   V_0
    IL_002c:  /* 7C   | (04)00004F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider/*02000010*//'<ProvideCompletionsAsync>d__0'/*02000023*/::'<>t__builder' /* 0400004F */
    IL_0031:  /* 28   | (0A)000036       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::get_Task() /* 0A000036 */
    IL_0036:  /* 2A   |                  */ ret
  } // end of method ExtensionMethodsCompletionProvider::ProvideCompletionsAsync

  .method /*0600004A*/ private hidebysig static 
          class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ 
          FindNearestMemberAccessExpression(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ node) cil managed
  // SIG: 00 01 12 80 B5 12 80 B5
  {
    // Method begins at RVA 0x3700
    // Code size       40 (0x28)
    .maxstack  2
    .locals /*11000010*/ init (class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 2B   | 1F               */ br.s       IL_0023

    IL_0004:  /* 06   |                  */ ldloc.0
    IL_0005:  /* 2C   | 0F               */ brfalse.s  IL_0016

    IL_0007:  /* 06   |                  */ ldloc.0
    IL_0008:  /* 20   | F1210000         */ ldc.i4     0x21f1
    IL_000d:  /* 28   | (0A)000078       */ call       bool [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharpExtensions/*01000081*/::IsKind(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                   valuetype [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxKind/*0100007F*/) /* 0A000078 */
    IL_0012:  /* 2C   | 02               */ brfalse.s  IL_0016

    IL_0014:  /* 06   |                  */ ldloc.0
    IL_0015:  /* 2A   |                  */ ret

    IL_0016:  /* 06   |                  */ ldloc.0
    IL_0017:  /* 2D   | 03               */ brtrue.s   IL_001c

    IL_0019:  /* 14   |                  */ ldnull
    IL_001a:  /* 2B   | 06               */ br.s       IL_0022

    IL_001c:  /* 06   |                  */ ldloc.0
    IL_001d:  /* 28   | (0A)000079       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::get_Parent() /* 0A000079 */
    IL_0022:  /* 0A   |                  */ stloc.0
    IL_0023:  /* 06   |                  */ ldloc.0
    IL_0024:  /* 2D   | DE               */ brtrue.s   IL_0004

    IL_0026:  /* 14   |                  */ ldnull
    IL_0027:  /* 2A   |                  */ ret
  } // end of method ExtensionMethodsCompletionProvider::FindNearestMemberAccessExpression

  .method /*0600004B*/ private hidebysig static 
          bool  IsMatchingExtensionMethod(valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> pair,
                                          class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/ symbolType,
                                          valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ token) cil managed
  // SIG: 00 03 02 15 11 81 49 02 11 2C 11 28 12 81 4D 11 81 19
  {
    // Method begins at RVA 0x3734
    // Code size       96 (0x60)
    .maxstack  3
    .locals /*1100001B*/ init (bool V_0,
             valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/ V_1)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2D   | 02               */ brtrue.s   IL_0005

    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 2A   |                  */ ret

    IL_0005:  /* 04   |                  */ ldarg.2
    IL_0006:  /* 20   | A8210000         */ ldc.i4     0x21a8
    IL_000b:  /* 28   | (0A)00007A       */ call       bool [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharpExtensions/*01000081*/::IsKind(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/,
                                                                                                                                                                   valuetype [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxKind/*0100007F*/) /* 0A00007A */
    IL_0010:  /* 2D   | 0D               */ brtrue.s   IL_001f

    IL_0012:  /* 04   |                  */ ldarg.2
    IL_0013:  /* 20   | 3C210000         */ ldc.i4     0x213c
    IL_0018:  /* 28   | (0A)00007A       */ call       bool [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharpExtensions/*01000081*/::IsKind(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/,
                                                                                                                                                                   valuetype [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxKind/*0100007F*/) /* 0A00007A */
    IL_001d:  /* 2B   | 01               */ br.s       IL_0020

    IL_001f:  /* 17   |                  */ ldc.i4.1
    IL_0020:  /* 0A   |                  */ stloc.0
    IL_0021:  /* 7E   | (0A)00002F       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/::Default /* 0A00002F */
    IL_0026:  /* 0F   | 00               */ ldarga.s   pair
    IL_0028:  /* 28   | (0A)00007B       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000015*/::get_Key() /* 0A00007B */
    IL_002d:  /* 0B   |                  */ stloc.1
    IL_002e:  /* 12   | 01               */ ldloca.s   V_1
    IL_0030:  /* 28   | (06)00002D       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ThisType() /* 0600002D */
    IL_0035:  /* 03   |                  */ ldarg.1
    IL_0036:  /* 6F   | (0A)000030       */ callvirt   instance bool [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolEqualityComparer/*0100006D*/::Equals(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/,
                                                                                                                                                                           class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/) /* 0A000030 */
    IL_003b:  /* 2C   | 21               */ brfalse.s  IL_005e

    IL_003d:  /* 06   |                  */ ldloc.0
    IL_003e:  /* 2C   | 1C               */ brfalse.s  IL_005c

    IL_0040:  /* 0F   | 00               */ ldarga.s   pair
    IL_0042:  /* 28   | (0A)00007B       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000052*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000015*/::get_Key() /* 0A00007B */
    IL_0047:  /* 0B   |                  */ stloc.1
    IL_0048:  /* 12   | 01               */ ldloca.s   V_1
    IL_004a:  /* 28   | (06)00002E       */ call       instance string Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::get_ExtensionMethod() /* 0600002E */
    IL_004f:  /* 0F   | 02               */ ldarga.s   token
    IL_0051:  /* 28   | (0A)00007C       */ call       instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/::get_ValueText() /* 0A00007C */
    IL_0056:  /* 6F   | (0A)00007D       */ callvirt   instance bool [netstandard/*23000001*/]System.String/*0100006E*/::Contains(string) /* 0A00007D */
    IL_005b:  /* 2A   |                  */ ret

    IL_005c:  /* 17   |                  */ ldc.i4.1
    IL_005d:  /* 2A   |                  */ ret

    IL_005e:  /* 16   |                  */ ldc.i4.0
    IL_005f:  /* 2A   |                  */ ret
  } // end of method ExtensionMethodsCompletionProvider::IsMatchingExtensionMethod

  .method /*0600004C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x37a0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00007E       */ call       instance void [Microsoft.CodeAnalysis.Features/*23000008*/]Microsoft.CodeAnalysis.Completion.CompletionProvider/*01000050*/::.ctor() /* 0A00007E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExtensionMethodsCompletionProvider::.ctor

} // end of class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.ExtensionMethodsCompletionProvider

.class /*02000011*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer
       extends [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/
{
  .custom /*0C000057:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000058:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000059:0A000014*/ instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.ExportCodeFixProviderAttribute/*01000021*/::.ctor(string,
                                                                                                                                                                                string[]) /* 0A000014 */ = ( 01 00 02 43 23 00 00 00 00 00 00 )                // ...C#......
  .custom /*0C00005A:0A000015*/ instance void [System.Composition.AttributedModel/*23000005*/]System.Composition.SharedAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*02000024*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass5_0'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C00009D:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000055*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root
    .field /*04000056*/ public class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageCache/*0100005A*/ routeUsageCache
    .field /*04000057*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .method /*0600007F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4d36
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass5_0'::.ctor

  } // end of class '<>c__DisplayClass5_0'

  .class /*02000025*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass5_1'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C0000A1:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000058*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic
    .field /*04000059*/ public class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ 'CS$<>8__locals1'
    .method /*06000080*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4d3e
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass5_1'::.ctor

    .method /*06000081*/ assembly hidebysig 
            instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
            '<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
    // SIG: 20 01 15 12 80 A9 01 12 80 AD 11 80 ED
    {
      // Method begins at RVA 0x4d46
      // Code size       51 (0x33)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000058       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::diagnostic /* 04000058 */
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 7B   | (04)000059       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::'CS$<>8__locals1' /* 04000059 */
      IL_000c:  /* 7B   | (04)000055       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::root /* 04000055 */
      IL_0011:  /* 02   |                  */ ldarg.0
      IL_0012:  /* 7B   | (04)000059       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::'CS$<>8__locals1' /* 04000059 */
      IL_0017:  /* 7B   | (04)000056       */ ldfld      class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageCache/*0100005A*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::routeUsageCache /* 04000056 */
      IL_001c:  /* 02   |                  */ ldarg.0
      IL_001d:  /* 7B   | (04)000059       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::'CS$<>8__locals1' /* 04000059 */
      IL_0022:  /* 7C   | (04)000057       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::context /* 04000057 */
      IL_0027:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
      IL_002c:  /* 03   |                  */ ldarg.1
      IL_002d:  /* 28   | (06)000050       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::AddRouteParameterAsync(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                                                                                                                                                                                       class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                                                                                                                                       class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageCache/*0100005A*/,
                                                                                                                                                                                                                                                                                                                                                                       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                                                       valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 06000050 */
      IL_0032:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass5_1'::'<RegisterCodeFixesAsync>b__0'

  } // end of class '<>c__DisplayClass5_1'

  .class /*02000026*/ auto ansi sealed nested private beforefieldinit '<RegisterCodeFixesAsync>d__5'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C0000A4:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*0400005A*/ public int32 '<>1__state'
    .field /*0400005B*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ '<>t__builder'
    .field /*0400005C*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .field /*0400005D*/ private class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ '<>8__1'
    .field /*0400005E*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> '<>u__1'
    .field /*0400005F*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> '<>u__2'
    .method /*06000082*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x4d7c
      // Code size       611 (0x263)
      .maxstack  4
      .locals /*1100002B*/ init (int32 V_0,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ V_1,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_2,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_3,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_4,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_5,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_6,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_7,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_8,
               class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/ V_9,
               string V_10,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_11)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)00005A       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>1__state' /* 0400005A */
      IL_0006:  /* 0A   |                  */ stloc.0
      .try
      {
        IL_0007:  /* 06   |                  */ ldloc.0
        IL_0008:  /* 39   | 84000000         */ brfalse    IL_0091

        IL_000d:  /* 06   |                  */ ldloc.0
        IL_000e:  /* 17   |                  */ ldc.i4.1
        IL_000f:  /* 3B   | 22010000         */ beq        IL_0136

        IL_0014:  /* 02   |                  */ ldarg.0
        IL_0015:  /* 73   | (06)00007F       */ newobj     instance void Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::.ctor() /* 0600007F */
        IL_001a:  /* 7D   | (04)00005D       */ stfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_001f:  /* 02   |                  */ ldarg.0
        IL_0020:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_0025:  /* 02   |                  */ ldarg.0
        IL_0026:  /* 7B   | (04)00005C       */ ldfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::context /* 0400005C */
        IL_002b:  /* 7D   | (04)000057       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::context /* 04000057 */
        IL_0030:  /* 02   |                  */ ldarg.0
        IL_0031:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_0036:  /* 7C   | (04)000057       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::context /* 04000057 */
        IL_003b:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_0040:  /* 02   |                  */ ldarg.0
        IL_0041:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_0046:  /* 7C   | (04)000057       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::context /* 04000057 */
        IL_004b:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_0050:  /* 6F   | (0A)0000BB       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSyntaxRootAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000BB */
        IL_0055:  /* 16   |                  */ ldc.i4.0
        IL_0056:  /* 6F   | (0A)0000BC       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001B*/::ConfigureAwait(bool) /* 0A0000BC */
        IL_005b:  /* 13   | 04               */ stloc.s    V_4
        IL_005d:  /* 12   | 04               */ ldloca.s   V_4
        IL_005f:  /* 28   | (0A)0000BD       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001C*/::GetAwaiter() /* 0A0000BD */
        IL_0064:  /* 0D   |                  */ stloc.3
        IL_0065:  /* 12   | 03               */ ldloca.s   V_3
        IL_0067:  /* 28   | (0A)0000BE       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::get_IsCompleted() /* 0A0000BE */
        IL_006c:  /* 2D   | 3F               */ brtrue.s   IL_00ad

        IL_006e:  /* 02   |                  */ ldarg.0
        IL_006f:  /* 16   |                  */ ldc.i4.0
        IL_0070:  /* 25   |                  */ dup
        IL_0071:  /* 0A   |                  */ stloc.0
        IL_0072:  /* 7D   | (04)00005A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>1__state' /* 0400005A */
        IL_0077:  /* 02   |                  */ ldarg.0
        IL_0078:  /* 09   |                  */ ldloc.3
        IL_0079:  /* 7D   | (04)00005E       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>u__1' /* 0400005E */
        IL_007e:  /* 02   |                  */ ldarg.0
        IL_007f:  /* 7C   | (04)00005B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>t__builder' /* 0400005B */
        IL_0084:  /* 12   | 03               */ ldloca.s   V_3
        IL_0086:  /* 02   |                  */ ldarg.0
        IL_0087:  /* 28   | (2B)000023       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>,valuetype Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  !!1&) /* 2B000023 */
        IL_008c:  /* DD   | D1010000         */ leave      IL_0262

        IL_0091:  /* 02   |                  */ ldarg.0
        IL_0092:  /* 7B   | (04)00005E       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>u__1' /* 0400005E */
        IL_0097:  /* 0D   |                  */ stloc.3
        IL_0098:  /* 02   |                  */ ldarg.0
        IL_0099:  /* 7C   | (04)00005E       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>u__1' /* 0400005E */
        IL_009e:  /* FE15 | (1B)00001D       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/
        IL_00a4:  /* 02   |                  */ ldarg.0
        IL_00a5:  /* 15   |                  */ ldc.i4.m1
        IL_00a6:  /* 25   |                  */ dup
        IL_00a7:  /* 0A   |                  */ stloc.0
        IL_00a8:  /* 7D   | (04)00005A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>1__state' /* 0400005A */
        IL_00ad:  /* 12   | 03               */ ldloca.s   V_3
        IL_00af:  /* 28   | (0A)0000C0       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::GetResult() /* 0A0000C0 */
        IL_00b4:  /* 0C   |                  */ stloc.2
        IL_00b5:  /* 02   |                  */ ldarg.0
        IL_00b6:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_00bb:  /* 08   |                  */ ldloc.2
        IL_00bc:  /* 7D   | (04)000055       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::root /* 04000055 */
        IL_00c1:  /* 02   |                  */ ldarg.0
        IL_00c2:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_00c7:  /* 7B   | (04)000055       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::root /* 04000055 */
        IL_00cc:  /* 2D   | 05               */ brtrue.s   IL_00d3

        IL_00ce:  /* DD   | 75010000         */ leave      IL_0248

        IL_00d3:  /* 02   |                  */ ldarg.0
        IL_00d4:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_00d9:  /* 7C   | (04)000057       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::context /* 04000057 */
        IL_00de:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_00e3:  /* 02   |                  */ ldarg.0
        IL_00e4:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_00e9:  /* 7C   | (04)000057       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::context /* 04000057 */
        IL_00ee:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_00f3:  /* 6F   | (0A)0000C6       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSemanticModelAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000C6 */
        IL_00f8:  /* 16   |                  */ ldc.i4.0
        IL_00f9:  /* 6F   | (0A)0000C7       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001E*/::ConfigureAwait(bool) /* 0A0000C7 */
        IL_00fe:  /* 13   | 06               */ stloc.s    V_6
        IL_0100:  /* 12   | 06               */ ldloca.s   V_6
        IL_0102:  /* 28   | (0A)0000C8       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001F*/::GetAwaiter() /* 0A0000C8 */
        IL_0107:  /* 13   | 05               */ stloc.s    V_5
        IL_0109:  /* 12   | 05               */ ldloca.s   V_5
        IL_010b:  /* 28   | (0A)0000C9       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::get_IsCompleted() /* 0A0000C9 */
        IL_0110:  /* 2D   | 41               */ brtrue.s   IL_0153

        IL_0112:  /* 02   |                  */ ldarg.0
        IL_0113:  /* 17   |                  */ ldc.i4.1
        IL_0114:  /* 25   |                  */ dup
        IL_0115:  /* 0A   |                  */ stloc.0
        IL_0116:  /* 7D   | (04)00005A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>1__state' /* 0400005A */
        IL_011b:  /* 02   |                  */ ldarg.0
        IL_011c:  /* 11   | 05               */ ldloc.s    V_5
        IL_011e:  /* 7D   | (04)00005F       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>u__2' /* 0400005F */
        IL_0123:  /* 02   |                  */ ldarg.0
        IL_0124:  /* 7C   | (04)00005B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>t__builder' /* 0400005B */
        IL_0129:  /* 12   | 05               */ ldloca.s   V_5
        IL_012b:  /* 02   |                  */ ldarg.0
        IL_012c:  /* 28   | (2B)000024       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>,valuetype Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     !!1&) /* 2B000024 */
        IL_0131:  /* DD   | 2C010000         */ leave      IL_0262

        IL_0136:  /* 02   |                  */ ldarg.0
        IL_0137:  /* 7B   | (04)00005F       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>u__2' /* 0400005F */
        IL_013c:  /* 13   | 05               */ stloc.s    V_5
        IL_013e:  /* 02   |                  */ ldarg.0
        IL_013f:  /* 7C   | (04)00005F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>u__2' /* 0400005F */
        IL_0144:  /* FE15 | (1B)000020       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/
        IL_014a:  /* 02   |                  */ ldarg.0
        IL_014b:  /* 15   |                  */ ldc.i4.m1
        IL_014c:  /* 25   |                  */ dup
        IL_014d:  /* 0A   |                  */ stloc.0
        IL_014e:  /* 7D   | (04)00005A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>1__state' /* 0400005A */
        IL_0153:  /* 12   | 05               */ ldloca.s   V_5
        IL_0155:  /* 28   | (0A)0000CA       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::GetResult() /* 0A0000CA */
        IL_015a:  /* 0B   |                  */ stloc.1
        IL_015b:  /* 07   |                  */ ldloc.1
        IL_015c:  /* 2D   | 05               */ brtrue.s   IL_0163

        IL_015e:  /* DD   | E5000000         */ leave      IL_0248

        IL_0163:  /* 02   |                  */ ldarg.0
        IL_0164:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_0169:  /* 07   |                  */ ldloc.1
        IL_016a:  /* 6F   | (0A)0000CB       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Compilation/*0100008F*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/::get_Compilation() /* 0A0000CB */
        IL_016f:  /* 28   | (0A)0000E5       */ call       class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageCache/*0100005A*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageCache/*0100005A*/::GetOrCreate(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Compilation/*0100008F*/) /* 0A0000E5 */
        IL_0174:  /* 7D   | (04)000056       */ stfld      class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageCache/*0100005A*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::routeUsageCache /* 04000056 */
        IL_0179:  /* 02   |                  */ ldarg.0
        IL_017a:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_017f:  /* 7C   | (04)000057       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::context /* 04000057 */
        IL_0184:  /* 28   | (0A)000068       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Diagnostics() /* 0A000068 */
        IL_0189:  /* 13   | 08               */ stloc.s    V_8
        IL_018b:  /* 12   | 08               */ ldloca.s   V_8
        IL_018d:  /* 28   | (0A)000069       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<!0> valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000010*/::GetEnumerator() /* 0A000069 */
        IL_0192:  /* 13   | 07               */ stloc.s    V_7
        IL_0194:  /* 38   | 81000000         */ br         IL_021a

        IL_0199:  /* 73   | (06)000080       */ newobj     instance void Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::.ctor() /* 06000080 */
        IL_019e:  /* 13   | 09               */ stloc.s    V_9
        IL_01a0:  /* 11   | 09               */ ldloc.s    V_9
        IL_01a2:  /* 02   |                  */ ldarg.0
        IL_01a3:  /* 7B   | (04)00005D       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_01a8:  /* 7D   | (04)000059       */ stfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::'CS$<>8__locals1' /* 04000059 */
        IL_01ad:  /* 11   | 09               */ ldloc.s    V_9
        IL_01af:  /* 12   | 07               */ ldloca.s   V_7
        IL_01b1:  /* 28   | (0A)00006A       */ call       instance !0 valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::get_Current() /* 0A00006A */
        IL_01b6:  /* 7D   | (04)000058       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::diagnostic /* 04000058 */
        IL_01bb:  /* 11   | 09               */ ldloc.s    V_9
        IL_01bd:  /* 7B   | (04)000058       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::diagnostic /* 04000058 */
        IL_01c2:  /* 6F   | (0A)00006B       */ callvirt   instance class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string> [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Properties() /* 0A00006B */
        IL_01c7:  /* 72   | (70)000297       */ ldstr      "RouteParameterName" /* 70000297 */
        IL_01cc:  /* 12   | 0A               */ ldloca.s   V_10
        IL_01ce:  /* 6F   | (0A)00006C       */ callvirt   instance bool class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string>/*1B000012*/::TryGetValue(!0,
                                                                                                                                                                                                                                !1&) /* 0A00006C */
        IL_01d3:  /* 2C   | 45               */ brfalse.s  IL_021a

        IL_01d5:  /* 11   | 09               */ ldloc.s    V_9
        IL_01d7:  /* 7B   | (04)000059       */ ldfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::'CS$<>8__locals1' /* 04000059 */
        IL_01dc:  /* 7C   | (04)000057       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/::context /* 04000057 */
        IL_01e1:  /* 72   | (70)0008F6       */ ldstr      "Add parameter '" /* 700008F6 */
        IL_01e6:  /* 11   | 0A               */ ldloc.s    V_10
        IL_01e8:  /* 72   | (70)000916       */ ldstr      "'" /* 70000916 */
        IL_01ed:  /* 28   | (0A)0000E6       */ call       string [netstandard/*23000001*/]System.String/*0100006E*/::Concat(string,
                                                                                                                             string,
                                                                                                                             string) /* 0A0000E6 */
        IL_01f2:  /* 11   | 09               */ ldloc.s    V_9
        IL_01f4:  /* FE06 | (06)000081       */ ldftn      instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::'<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 06000081 */
        IL_01fa:  /* 73   | (0A)00006E       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>/*1B000013*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                                                                   native int) /* 0A00006E */
        IL_01ff:  /* 7E   | (0A)00009B       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::RoutePatternUnusedParameter /* 0A00009B */
        IL_0204:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
        IL_0209:  /* 28   | (0A)00006F       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/::Create(string,
                                                                                                                                                                                                                                                                                           class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>,
                                                                                                                                                                                                                                                                                           string) /* 0A00006F */
        IL_020e:  /* 11   | 09               */ ldloc.s    V_9
        IL_0210:  /* 7B   | (04)000058       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_1'/*02000025*/::diagnostic /* 04000058 */
        IL_0215:  /* 28   | (0A)000070       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::RegisterCodeFix(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/,
                                                                                                                                                                                                     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/) /* 0A000070 */
        IL_021a:  /* 12   | 07               */ ldloca.s   V_7
        IL_021c:  /* 28   | (0A)000071       */ call       instance bool valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::MoveNext() /* 0A000071 */
        IL_0221:  /* 3A   | 73FFFFFF         */ brtrue     IL_0199

        IL_0226:  /* DE   | 20               */ leave.s    IL_0248

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_0228:  /* 13   | 0B               */ stloc.s    V_11
        IL_022a:  /* 02   |                  */ ldarg.0
        IL_022b:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_022d:  /* 7D   | (04)00005A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>1__state' /* 0400005A */
        IL_0232:  /* 02   |                  */ ldarg.0
        IL_0233:  /* 14   |                  */ ldnull
        IL_0234:  /* 7D   | (04)00005D       */ stfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
        IL_0239:  /* 02   |                  */ ldarg.0
        IL_023a:  /* 7C   | (04)00005B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>t__builder' /* 0400005B */
        IL_023f:  /* 11   | 0B               */ ldloc.s    V_11
        IL_0241:  /* 28   | (0A)0000C3       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000C3 */
        IL_0246:  /* DE   | 1A               */ leave.s    IL_0262

      }  // end handler
      // HEX: 00 00 00 00 07 00 00 00 21 02 00 00 28 02 00 00 20 00 00 00 40 00 00 01
      IL_0248:  /* 02   |                  */ ldarg.0
      IL_0249:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_024b:  /* 7D   | (04)00005A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>1__state' /* 0400005A */
      IL_0250:  /* 02   |                  */ ldarg.0
      IL_0251:  /* 14   |                  */ ldnull
      IL_0252:  /* 7D   | (04)00005D       */ stfld      class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<>c__DisplayClass5_0'/*02000024*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>8__1' /* 0400005D */
      IL_0257:  /* 02   |                  */ ldarg.0
      IL_0258:  /* 7C   | (04)00005B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>t__builder' /* 0400005B */
      IL_025d:  /* 28   | (0A)0000C4       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetResult() /* 0A0000C4 */
      IL_0262:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__5'::MoveNext

    .method /*06000083*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000D7:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x5008
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)00005B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>t__builder' /* 0400005B */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000C5       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000C5 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__5'::SetStateMachine

  } // end of class '<RegisterCodeFixesAsync>d__5'

  .field /*0400001B*/ private static initonly class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ DefaultType
  .field /*0400001C*/ private initonly valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> '<FixableDiagnosticIds>k__BackingField'
  .custom /*0C00007E:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*0600004D*/ public hidebysig specialname virtual 
          instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> 
          get_FixableDiagnosticIds() cil managed
  // SIG: 20 00 15 11 80 91 01 0E
  {
    .custom /*0C0000C4:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .param [0]/*08000073*/ 
    .custom /*0C0000D3:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x37a8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00001C       */ ldfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::'<FixableDiagnosticIds>k__BackingField' /* 0400001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::get_FixableDiagnosticIds

  .method /*0600004E*/ public hidebysig virtual final 
          instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ 
          GetFixAllProvider() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x37b0
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 28   | (0A)000033       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders/*0100006F*/::get_BatchFixer() /* 0A000033 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::GetFixAllProvider

  .method /*0600004F*/ public hidebysig virtual final 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          RegisterCodeFixesAsync(valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context) cil managed
  // SIG: 20 01 12 80 A1 11 80 A5
  {
    .custom /*0C0000C5:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 7B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..{Microsoft.Asp
                                                                                                                                                                                                                                  4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65 72   // NetCore.Analyzer
                                                                                                                                                                                                                                  73 2E 52 6F 75 74 65 45 6D 62 65 64 64 65 64 4C   // s.RouteEmbeddedL
                                                                                                                                                                                                                                  61 6E 67 75 61 67 65 2E 46 69 78 65 72 73 2E 52   // anguage.Fixers.R
                                                                                                                                                                                                                                  6F 75 74 65 50 61 72 61 6D 65 74 65 72 55 6E 75   // outeParameterUnu
                                                                                                                                                                                                                                  73 65 64 50 61 72 61 6D 65 74 65 72 46 69 78 65   // sedParameterFixe
                                                                                                                                                                                                                                  72 2B 3C 52 65 67 69 73 74 65 72 43 6F 64 65 46   // r+<RegisterCodeF
                                                                                                                                                                                                                                  69 78 65 73 41 73 79 6E 63 3E 64 5F 5F 35 00 00 ) // ixesAsync>d__5..
    // Method begins at RVA 0x37b8
    // Code size       55 (0x37)
    .maxstack  2
    .locals /*1100001C*/ init (valuetype Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)000034       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Create() /* 0A000034 */
    IL_0007:  /* 7D   | (04)00005B       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>t__builder' /* 0400005B */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 7D   | (04)00005C       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::context /* 0400005C */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 15   |                  */ ldc.i4.m1
    IL_0017:  /* 7D   | (04)00005A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>1__state' /* 0400005A */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 7C   | (04)00005B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>t__builder' /* 0400005B */
    IL_0023:  /* 12   | 00               */ ldloca.s   V_0
    IL_0025:  /* 28   | (2B)000011       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Start<valuetype Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/>(!!0&) /* 2B000011 */
    IL_002a:  /* 12   | 00               */ ldloca.s   V_0
    IL_002c:  /* 7C   | (04)00005B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*//'<RegisterCodeFixesAsync>d__5'/*02000026*/::'<>t__builder' /* 0400005B */
    IL_0031:  /* 28   | (0A)000036       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::get_Task() /* 0A000036 */
    IL_0036:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::RegisterCodeFixesAsync

  .method /*06000050*/ private hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
          AddRouteParameterAsync(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                 class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                                 class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageCache/*0100005A*/ routeUsageCache,
                                 class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                                 valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 80 A9 01 12 80 AD 12 80 B1 12 80 B5 12 81 69 12 80 AD 11 80 ED
  {
    // Method begins at RVA 0x37fc
    // Code size       96 (0x60)
    .maxstack  5
    .locals /*1100001D*/ init (valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ V_0,
             class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageModel/*0100005B*/ V_1,
             valuetype [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Infrastructure.RouteUsageContext/*0100005C*/ V_2)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_0007:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_000c:  /* 16   |                  */ ldc.i4.0
    IL_000d:  /* 16   |                  */ ldc.i4.0
    IL_000e:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_0013:  /* 16   |                  */ ldc.i4.0
    IL_0014:  /* 16   |                  */ ldc.i4.0
    IL_0015:  /* 16   |                  */ ldc.i4.0
    IL_0016:  /* 16   |                  */ ldc.i4.0
    IL_0017:  /* 6F   | (0A)00007F       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::GetFirstToken(bool,
                                                                                                                                                                                                                                                              bool,
                                                                                                                                                                                                                                                              bool,
                                                                                                                                                                                                                                                              bool) /* 0A00007F */
    IL_001c:  /* 0A   |                  */ stloc.0
    IL_001d:  /* 04   |                  */ ldarg.2
    IL_001e:  /* 06   |                  */ ldloc.0
    IL_001f:  /* 0E   | 04               */ ldarg.s    cancellationToken
    IL_0021:  /* 6F   | (0A)000080       */ callvirt   instance class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageModel/*0100005B*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageCache/*0100005A*/::Get(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/,
                                                                                                                                                                                                                                                                                                                                       valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A000080 */
    IL_0026:  /* 0B   |                  */ stloc.1
    IL_0027:  /* 07   |                  */ ldloc.1
    IL_0028:  /* 2D   | 03               */ brtrue.s   IL_002d

    IL_002a:  /* 14   |                  */ ldnull
    IL_002b:  /* 2B   | 0E               */ br.s       IL_003b

    IL_002d:  /* 07   |                  */ ldloc.1
    IL_002e:  /* 28   | (0A)000081       */ call       instance valuetype [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Infrastructure.RouteUsageContext/*0100005C*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageModel/*0100005B*/::get_UsageContext() /* 0A000081 */
    IL_0033:  /* 0C   |                  */ stloc.2
    IL_0034:  /* 12   | 02               */ ldloca.s   V_2
    IL_0036:  /* 28   | (0A)000082       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Infrastructure.RouteUsageContext/*0100005C*/::get_MethodSyntax() /* 0A000082 */
    IL_003b:  /* 2D   | 07               */ brtrue.s   IL_0044

    IL_003d:  /* 05   |                  */ ldarg.3
    IL_003e:  /* 28   | (2B)000004       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!!0> [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/::FromResult<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>(!!0) /* 2B000004 */
    IL_0043:  /* 2A   |                  */ ret

    IL_0044:  /* 02   |                  */ ldarg.0
    IL_0045:  /* 03   |                  */ ldarg.1
    IL_0046:  /* 05   |                  */ ldarg.3
    IL_0047:  /* 07   |                  */ ldloc.1
    IL_0048:  /* 6F   | (0A)000081       */ callvirt   instance valuetype [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Infrastructure.RouteUsageContext/*0100005C*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.RouteUsageModel/*0100005B*/::get_UsageContext() /* 0A000081 */
    IL_004d:  /* 0C   |                  */ stloc.2
    IL_004e:  /* 12   | 02               */ ldloca.s   V_2
    IL_0050:  /* 28   | (0A)000082       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Infrastructure.RouteUsageContext/*0100005C*/::get_MethodSyntax() /* 0A000082 */
    IL_0055:  /* 28   | (06)000051       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::UpdateDocument(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                                                                                                   class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                                                   class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                   class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 06000051 */
    IL_005a:  /* 28   | (2B)000004       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!!0> [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/::FromResult<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>(!!0) /* 2B000004 */
    IL_005f:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::AddRouteParameterAsync

  .method /*06000051*/ private hidebysig static 
          class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ 
          UpdateDocument(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                         class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                         class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                         class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ methodSyntax) cil managed
  // SIG: 00 04 12 80 AD 12 80 B1 12 80 B5 12 80 AD 12 80 B5
  {
    // Method begins at RVA 0x3868
    // Code size       255 (0xff)
    .maxstack  5
    .locals /*1100001E*/ init (string V_0,
             string V_1,
             int32 V_2,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ V_3,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ V_4,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/ V_5,
             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_6,
             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_7,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax/*0100005D*/ V_8,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax/*0100005E*/ V_9,
             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_10)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)00006B       */ callvirt   instance class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string> [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Properties() /* 0A00006B */
    IL_0006:  /* 72   | (70)000297       */ ldstr      "RouteParameterName" /* 70000297 */
    IL_000b:  /* 6F   | (0A)000083       */ callvirt   instance !1 class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string>/*1B000012*/::get_Item(!0) /* 0A000083 */
    IL_0010:  /* 0A   |                  */ stloc.0
    IL_0011:  /* 02   |                  */ ldarg.0
    IL_0012:  /* 6F   | (0A)00006B       */ callvirt   instance class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string> [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Properties() /* 0A00006B */
    IL_0017:  /* 72   | (70)0002BD       */ ldstr      "RouteParameterPolicy" /* 700002BD */
    IL_001c:  /* 6F   | (0A)000083       */ callvirt   instance !1 class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string>/*1B000012*/::get_Item(!0) /* 0A000083 */
    IL_0021:  /* 0B   |                  */ stloc.1
    IL_0022:  /* 02   |                  */ ldarg.0
    IL_0023:  /* 6F   | (0A)00006B       */ callvirt   instance class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string> [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Properties() /* 0A00006B */
    IL_0028:  /* 72   | (70)0002E7       */ ldstr      "RouteParameterIsOptional" /* 700002E7 */
    IL_002d:  /* 6F   | (0A)000083       */ callvirt   instance !1 class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string>/*1B000012*/::get_Item(!0) /* 0A000083 */
    IL_0032:  /* 28   | (0A)000084       */ call       class [netstandard/*23000001*/]System.Globalization.CultureInfo/*01000082*/ [netstandard/*23000001*/]System.Globalization.CultureInfo/*01000082*/::get_InvariantCulture() /* 0A000084 */
    IL_0037:  /* 28   | (0A)000085       */ call       bool [netstandard/*23000001*/]System.Convert/*01000083*/::ToBoolean(string,
                                                                                                                           class [netstandard/*23000001*/]System.IFormatProvider/*01000084*/) /* 0A000085 */
    IL_003c:  /* 02   |                  */ ldarg.0
    IL_003d:  /* 6F   | (0A)00006B       */ callvirt   instance class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string> [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Properties() /* 0A00006B */
    IL_0042:  /* 72   | (70)000319       */ ldstr      "RouteParameterInsertIndex" /* 70000319 */
    IL_0047:  /* 6F   | (0A)000083       */ callvirt   instance !1 class [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableDictionary`2/*01000080*/<string,string>/*1B000012*/::get_Item(!0) /* 0A000083 */
    IL_004c:  /* 28   | (0A)000084       */ call       class [netstandard/*23000001*/]System.Globalization.CultureInfo/*01000082*/ [netstandard/*23000001*/]System.Globalization.CultureInfo/*01000082*/::get_InvariantCulture() /* 0A000084 */
    IL_0051:  /* 28   | (0A)000086       */ call       int32 [netstandard/*23000001*/]System.Convert/*01000083*/::ToInt32(string,
                                                                                                                          class [netstandard/*23000001*/]System.IFormatProvider/*01000084*/) /* 0A000086 */
    IL_0056:  /* 0C   |                  */ stloc.2
    IL_0057:  /* 07   |                  */ ldloc.1
    IL_0058:  /* 28   | (06)000054       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::CalculateTypeFromPolicy(string) /* 06000054 */
    IL_005d:  /* 0D   |                  */ stloc.3
    IL_005e:  /* 2C   | 07               */ brfalse.s  IL_0067

    IL_0060:  /* 09   |                  */ ldloc.3
    IL_0061:  /* 28   | (0A)000087       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax/*01000085*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::NullableType(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/) /* 0A000087 */
    IL_0066:  /* 0D   |                  */ stloc.3
    IL_0067:  /* 09   |                  */ ldloc.3
    IL_0068:  /* 17   |                  */ ldc.i4.1
    IL_0069:  /* 8D   | (01)000045       */ newarr     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxAnnotation/*01000045*/
    IL_006e:  /* 25   |                  */ dup
    IL_006f:  /* 16   |                  */ ldc.i4.0
    IL_0070:  /* 72   | (70)00034D       */ ldstr      "CodeAction_Navigation" /* 7000034D */
    IL_0075:  /* 73   | (0A)000088       */ newobj     instance void [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxAnnotation/*01000045*/::.ctor(string) /* 0A000088 */
    IL_007a:  /* A2   |                  */ stelem.ref
    IL_007b:  /* 28   | (2B)000012       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.AnnotationExtensions/*0100007B*/::WithAdditionalAnnotations<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/>(!!0,
                                                                                                                                                                                                                                                                                                class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxAnnotation/*01000045*/[]) /* 2B000012 */
    IL_0080:  /* 13   | 04               */ stloc.s    V_4
    IL_0082:  /* 06   |                  */ ldloc.0
    IL_0083:  /* 28   | (0A)00005E       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::Identifier(string) /* 0A00005E */
    IL_0088:  /* 28   | (0A)000089       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::Parameter(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/) /* 0A000089 */
    IL_008d:  /* 11   | 04               */ ldloc.s    V_4
    IL_008f:  /* 6F   | (0A)00008A       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/::WithType(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/) /* 0A00008A */
    IL_0094:  /* 13   | 05               */ stloc.s    V_5
    IL_0096:  /* 05   |                  */ ldarg.3
    IL_0097:  /* 75   | (01)00005D       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax/*0100005D*/
    IL_009c:  /* 13   | 08               */ stloc.s    V_8
    IL_009e:  /* 11   | 08               */ ldloc.s    V_8
    IL_00a0:  /* 2D   | 0E               */ brtrue.s   IL_00b0

    IL_00a2:  /* 05   |                  */ ldarg.3
    IL_00a3:  /* 75   | (01)00005E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax/*0100005E*/
    IL_00a8:  /* 13   | 09               */ stloc.s    V_9
    IL_00aa:  /* 11   | 09               */ ldloc.s    V_9
    IL_00ac:  /* 2D   | 10               */ brtrue.s   IL_00be

    IL_00ae:  /* 2B   | 1C               */ br.s       IL_00cc

    IL_00b0:  /* 11   | 08               */ ldloc.s    V_8
    IL_00b2:  /* 11   | 05               */ ldloc.s    V_5
    IL_00b4:  /* 08   |                  */ ldloc.2
    IL_00b5:  /* 28   | (06)000052       */ call       class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::AddParameter(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax/*0100005D*/,
                                                                                                                                                                                                                                                                        class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/,
                                                                                                                                                                                                                                                                        int32) /* 06000052 */
    IL_00ba:  /* 13   | 0A               */ stloc.s    V_10
    IL_00bc:  /* 2B   | 29               */ br.s       IL_00e7

    IL_00be:  /* 11   | 09               */ ldloc.s    V_9
    IL_00c0:  /* 11   | 05               */ ldloc.s    V_5
    IL_00c2:  /* 08   |                  */ ldloc.2
    IL_00c3:  /* 28   | (06)000053       */ call       class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::AddParameter(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax/*0100005E*/,
                                                                                                                                                                                                                                                                        class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/,
                                                                                                                                                                                                                                                                        int32) /* 06000053 */
    IL_00c8:  /* 13   | 0A               */ stloc.s    V_10
    IL_00ca:  /* 2B   | 1B               */ br.s       IL_00e7

    IL_00cc:  /* 72   | (70)000379       */ ldstr      "Unexpected method syntax: " /* 70000379 */
    IL_00d1:  /* 05   |                  */ ldarg.3
    IL_00d2:  /* 6F   | (0A)00008B       */ callvirt   instance class [netstandard/*23000001*/]System.Type/*01000026*/ [netstandard/*23000001*/]System.Object/*01000015*/::GetType() /* 0A00008B */
    IL_00d7:  /* 6F   | (0A)00008C       */ callvirt   instance string [netstandard/*23000001*/]System.Type/*01000026*/::get_FullName() /* 0A00008C */
    IL_00dc:  /* 28   | (0A)00008D       */ call       string [netstandard/*23000001*/]System.String/*0100006E*/::Concat(string,
                                                                                                                         string) /* 0A00008D */
    IL_00e1:  /* 73   | (0A)00008E       */ newobj     instance void [netstandard/*23000001*/]System.InvalidOperationException/*01000086*/::.ctor(string) /* 0A00008E */
    IL_00e6:  /* 7A   |                  */ throw

    IL_00e7:  /* 11   | 0A               */ ldloc.s    V_10
    IL_00e9:  /* 13   | 06               */ stloc.s    V_6
    IL_00eb:  /* 03   |                  */ ldarg.1
    IL_00ec:  /* 05   |                  */ ldarg.3
    IL_00ed:  /* 11   | 06               */ ldloc.s    V_6
    IL_00ef:  /* 28   | (2B)000003       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceNode<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>(!!0,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 2B000003 */
    IL_00f4:  /* 13   | 07               */ stloc.s    V_7
    IL_00f6:  /* 04   |                  */ ldarg.2
    IL_00f7:  /* 11   | 07               */ ldloc.s    V_7
    IL_00f9:  /* 6F   | (0A)00003B       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::WithSyntaxRoot(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 0A00003B */
    IL_00fe:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::UpdateDocument

  .method /*06000052*/ private hidebysig static 
          class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ 
          AddParameter(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax/*0100005D*/ syntax,
                       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/ parameterSyntax,
                       int32 parameterIndex) cil managed
  // SIG: 00 03 12 80 B5 12 81 75 12 81 35 08
  {
    // Method begins at RVA 0x3974
    // Code size       48 (0x30)
    .maxstack  3
    .locals /*1100001F*/ init (valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/> V_0,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/> V_1)
    IL_0000:  /* 04   |                  */ ldarg.2
    IL_0001:  /* 15   |                  */ ldc.i4.m1
    IL_0002:  /* 33   | 03               */ bne.un.s   IL_0007

    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 10   | 02               */ starg.s    parameterIndex
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 6F   | (0A)00008F       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax/*0100005D*/::get_ParameterList() /* 0A00008F */
    IL_000d:  /* 6F   | (0A)000090       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax/*01000088*/::get_Parameters() /* 0A000090 */
    IL_0012:  /* 0B   |                  */ stloc.1
    IL_0013:  /* 12   | 01               */ ldloca.s   V_1
    IL_0015:  /* 04   |                  */ ldarg.2
    IL_0016:  /* 03   |                  */ ldarg.1
    IL_0017:  /* 28   | (0A)000091       */ call       instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<!0> valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/>/*1B000016*/::Insert(int32,
                                                                                                                                                                                                                                                                                                                                                                                                                         !0) /* 0A000091 */
    IL_001c:  /* 0A   |                  */ stloc.0
    IL_001d:  /* 02   |                  */ ldarg.0
    IL_001e:  /* 02   |                  */ ldarg.0
    IL_001f:  /* 6F   | (0A)00008F       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax/*0100005D*/::get_ParameterList() /* 0A00008F */
    IL_0024:  /* 06   |                  */ ldloc.0
    IL_0025:  /* 6F   | (0A)000092       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/::WithParameters(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/>) /* 0A000092 */
    IL_002a:  /* 6F   | (0A)000093       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax/*0100005D*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax/*0100005D*/::WithParameterList(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/) /* 0A000093 */
    IL_002f:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::AddParameter

  .method /*06000053*/ private hidebysig static 
          class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ 
          AddParameter(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax/*0100005E*/ syntax,
                       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/ parameterSyntax,
                       int32 parameterIndex) cil managed
  // SIG: 00 03 12 80 B5 12 81 79 12 81 35 08
  {
    // Method begins at RVA 0x39b0
    // Code size       48 (0x30)
    .maxstack  3
    .locals /*1100001F*/ init (valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/> V_0,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/> V_1)
    IL_0000:  /* 04   |                  */ ldarg.2
    IL_0001:  /* 15   |                  */ ldc.i4.m1
    IL_0002:  /* 33   | 03               */ bne.un.s   IL_0007

    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 10   | 02               */ starg.s    parameterIndex
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 6F   | (0A)000094       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax/*0100005E*/::get_ParameterList() /* 0A000094 */
    IL_000d:  /* 6F   | (0A)000090       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax/*01000088*/::get_Parameters() /* 0A000090 */
    IL_0012:  /* 0B   |                  */ stloc.1
    IL_0013:  /* 12   | 01               */ ldloca.s   V_1
    IL_0015:  /* 04   |                  */ ldarg.2
    IL_0016:  /* 03   |                  */ ldarg.1
    IL_0017:  /* 28   | (0A)000091       */ call       instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<!0> valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/>/*1B000016*/::Insert(int32,
                                                                                                                                                                                                                                                                                                                                                                                                                         !0) /* 0A000091 */
    IL_001c:  /* 0A   |                  */ stloc.0
    IL_001d:  /* 02   |                  */ ldarg.0
    IL_001e:  /* 02   |                  */ ldarg.0
    IL_001f:  /* 6F   | (0A)000094       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax/*0100005E*/::get_ParameterList() /* 0A000094 */
    IL_0024:  /* 06   |                  */ ldloc.0
    IL_0025:  /* 6F   | (0A)000092       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/::WithParameters(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax/*0100004D*/>) /* 0A000092 */
    IL_002a:  /* 6F   | (0A)000095       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax/*0100005E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax/*0100005E*/::WithParameterList(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax/*01000087*/) /* 0A000095 */
    IL_002f:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::AddParameter

  .method /*06000054*/ private hidebysig static 
          class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ 
          CalculateTypeFromPolicy(string routeParameterPolicy) cil managed
  // SIG: 00 01 12 81 39 0E
  {
    // Method begins at RVA 0x39ec
    // Code size       397 (0x18d)
    .maxstack  5
    .locals /*11000020*/ init (string V_0,
             string[] V_1,
             int32 V_2,
             string V_3,
             string V_4,
             string V_5,
             int32 V_6,
             char V_7)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 2D   | 06               */ brtrue.s   IL_0009

    IL_0003:  /* 7E   | (04)00001B       */ ldsfld     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::DefaultType /* 0400001B */
    IL_0008:  /* 2A   |                  */ ret

    IL_0009:  /* 14   |                  */ ldnull
    IL_000a:  /* 0A   |                  */ stloc.0
    IL_000b:  /* 02   |                  */ ldarg.0
    IL_000c:  /* 17   |                  */ ldc.i4.1
    IL_000d:  /* 8D   | (01)000089       */ newarr     [netstandard/*23000001*/]System.Char/*01000089*/
    IL_0012:  /* 25   |                  */ dup
    IL_0013:  /* 16   |                  */ ldc.i4.0
    IL_0014:  /* 1F   | 3A               */ ldc.i4.s   58
    IL_0016:  /* 9D   |                  */ stelem.i2
    IL_0017:  /* 17   |                  */ ldc.i4.1
    IL_0018:  /* 6F   | (0A)000096       */ callvirt   instance string[] [netstandard/*23000001*/]System.String/*0100006E*/::Split(char[],
                                                                                                                                   valuetype [netstandard/*23000001*/]System.StringSplitOptions/*0100008A*/) /* 0A000096 */
    IL_001d:  /* 0B   |                  */ stloc.1
    IL_001e:  /* 16   |                  */ ldc.i4.0
    IL_001f:  /* 0C   |                  */ stloc.2
    IL_0020:  /* 38   | 4C010000         */ br         IL_0171

    IL_0025:  /* 07   |                  */ ldloc.1
    IL_0026:  /* 08   |                  */ ldloc.2
    IL_0027:  /* 9A   |                  */ ldelem.ref
    IL_0028:  /* 0D   |                  */ stloc.3
    IL_0029:  /* 09   |                  */ ldloc.3
    IL_002a:  /* 39   | 1D010000         */ brfalse    IL_014c

    IL_002f:  /* 09   |                  */ ldloc.3
    IL_0030:  /* 28   | (0A)000097       */ call       instance int32 [netstandard/*23000001*/]System.String/*0100006E*/::get_Length() /* 0A000097 */
    IL_0035:  /* 13   | 06               */ stloc.s    V_6
    IL_0037:  /* 11   | 06               */ ldloc.s    V_6
    IL_0039:  /* 19   |                  */ ldc.i4.3
    IL_003a:  /* 59   |                  */ sub
    IL_003b:  /* 45   | 06000000         */ switch     ( 
              /*      | 25000000         */             IL_007d,
              /*      | 05000000         */             IL_005d,
              /*      | 9D000000         */             IL_00f5,
              /*      | 8E000000         */             IL_00e6,
              /*      | 7F000000         */             IL_00d7,
              /*      | 70000000         */             IL_00c8)
    IL_0058:  /* 38   | EF000000         */ br         IL_014c

    IL_005d:  /* 09   |                  */ ldloc.3
    IL_005e:  /* 16   |                  */ ldc.i4.0
    IL_005f:  /* 28   | (0A)000098       */ call       instance char [netstandard/*23000001*/]System.String/*0100006E*/::get_Chars(int32) /* 0A000098 */
    IL_0064:  /* 13   | 07               */ stloc.s    V_7
    IL_0066:  /* 11   | 07               */ ldloc.s    V_7
    IL_0068:  /* 1F   | 62               */ ldc.i4.s   98
    IL_006a:  /* 2E   | 35               */ beq.s      IL_00a1

    IL_006c:  /* 11   | 07               */ ldloc.s    V_7
    IL_006e:  /* 1F   | 67               */ ldc.i4.s   103
    IL_0070:  /* 2E   | 41               */ beq.s      IL_00b3

    IL_0072:  /* 11   | 07               */ ldloc.s    V_7
    IL_0074:  /* 1F   | 6C               */ ldc.i4.s   108
    IL_0076:  /* 2E   | 17               */ beq.s      IL_008f

    IL_0078:  /* 38   | CF000000         */ br         IL_014c

    IL_007d:  /* 09   |                  */ ldloc.3
    IL_007e:  /* 72   | (70)0003AF       */ ldstr      "int" /* 700003AF */
    IL_0083:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_0088:  /* 2D   | 7A               */ brtrue.s   IL_0104

    IL_008a:  /* 38   | BD000000         */ br         IL_014c

    IL_008f:  /* 09   |                  */ ldloc.3
    IL_0090:  /* 72   | (70)0003B7       */ ldstr      "long" /* 700003B7 */
    IL_0095:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_009a:  /* 2D   | 71               */ brtrue.s   IL_010d

    IL_009c:  /* 38   | AB000000         */ br         IL_014c

    IL_00a1:  /* 09   |                  */ ldloc.3
    IL_00a2:  /* 72   | (70)0003C1       */ ldstr      "bool" /* 700003C1 */
    IL_00a7:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_00ac:  /* 2D   | 68               */ brtrue.s   IL_0116

    IL_00ae:  /* 38   | 99000000         */ br         IL_014c

    IL_00b3:  /* 09   |                  */ ldloc.3
    IL_00b4:  /* 72   | (70)0003CB       */ ldstr      "guid" /* 700003CB */
    IL_00b9:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_00be:  /* 3A   | 80000000         */ brtrue     IL_0143

    IL_00c3:  /* 38   | 84000000         */ br         IL_014c

    IL_00c8:  /* 09   |                  */ ldloc.3
    IL_00c9:  /* 72   | (70)0003D5       */ ldstr      "datetime" /* 700003D5 */
    IL_00ce:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_00d3:  /* 2D   | 4A               */ brtrue.s   IL_011f

    IL_00d5:  /* 2B   | 75               */ br.s       IL_014c

    IL_00d7:  /* 09   |                  */ ldloc.3
    IL_00d8:  /* 72   | (70)0003E7       */ ldstr      "decimal" /* 700003E7 */
    IL_00dd:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_00e2:  /* 2D   | 44               */ brtrue.s   IL_0128

    IL_00e4:  /* 2B   | 66               */ br.s       IL_014c

    IL_00e6:  /* 09   |                  */ ldloc.3
    IL_00e7:  /* 72   | (70)0003F7       */ ldstr      "double" /* 700003F7 */
    IL_00ec:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_00f1:  /* 2D   | 3E               */ brtrue.s   IL_0131

    IL_00f3:  /* 2B   | 57               */ br.s       IL_014c

    IL_00f5:  /* 09   |                  */ ldloc.3
    IL_00f6:  /* 72   | (70)000405       */ ldstr      "float" /* 70000405 */
    IL_00fb:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                            string) /* 0A000031 */
    IL_0100:  /* 2D   | 38               */ brtrue.s   IL_013a

    IL_0102:  /* 2B   | 48               */ br.s       IL_014c

    IL_0104:  /* 72   | (70)0003AF       */ ldstr      "int" /* 700003AF */
    IL_0109:  /* 13   | 05               */ stloc.s    V_5
    IL_010b:  /* 2B   | 42               */ br.s       IL_014f

    IL_010d:  /* 72   | (70)0003B7       */ ldstr      "long" /* 700003B7 */
    IL_0112:  /* 13   | 05               */ stloc.s    V_5
    IL_0114:  /* 2B   | 39               */ br.s       IL_014f

    IL_0116:  /* 72   | (70)0003C1       */ ldstr      "bool" /* 700003C1 */
    IL_011b:  /* 13   | 05               */ stloc.s    V_5
    IL_011d:  /* 2B   | 30               */ br.s       IL_014f

    IL_011f:  /* 72   | (70)000411       */ ldstr      "System.DateTime" /* 70000411 */
    IL_0124:  /* 13   | 05               */ stloc.s    V_5
    IL_0126:  /* 2B   | 27               */ br.s       IL_014f

    IL_0128:  /* 72   | (70)000431       */ ldstr      "System.Decimal" /* 70000431 */
    IL_012d:  /* 13   | 05               */ stloc.s    V_5
    IL_012f:  /* 2B   | 1E               */ br.s       IL_014f

    IL_0131:  /* 72   | (70)0003F7       */ ldstr      "double" /* 700003F7 */
    IL_0136:  /* 13   | 05               */ stloc.s    V_5
    IL_0138:  /* 2B   | 15               */ br.s       IL_014f

    IL_013a:  /* 72   | (70)000405       */ ldstr      "float" /* 70000405 */
    IL_013f:  /* 13   | 05               */ stloc.s    V_5
    IL_0141:  /* 2B   | 0C               */ br.s       IL_014f

    IL_0143:  /* 72   | (70)00044F       */ ldstr      "System.Guid" /* 7000044F */
    IL_0148:  /* 13   | 05               */ stloc.s    V_5
    IL_014a:  /* 2B   | 03               */ br.s       IL_014f

    IL_014c:  /* 14   |                  */ ldnull
    IL_014d:  /* 13   | 05               */ stloc.s    V_5
    IL_014f:  /* 11   | 05               */ ldloc.s    V_5
    IL_0151:  /* 13   | 04               */ stloc.s    V_4
    IL_0153:  /* 11   | 04               */ ldloc.s    V_4
    IL_0155:  /* 2C   | 16               */ brfalse.s  IL_016d

    IL_0157:  /* 06   |                  */ ldloc.0
    IL_0158:  /* 2C   | 10               */ brfalse.s  IL_016a

    IL_015a:  /* 11   | 04               */ ldloc.s    V_4
    IL_015c:  /* 06   |                  */ ldloc.0
    IL_015d:  /* 28   | (0A)000099       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Inequality(string,
                                                                                                                              string) /* 0A000099 */
    IL_0162:  /* 2C   | 06               */ brfalse.s  IL_016a

    IL_0164:  /* 7E   | (04)00001B       */ ldsfld     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::DefaultType /* 0400001B */
    IL_0169:  /* 2A   |                  */ ret

    IL_016a:  /* 11   | 04               */ ldloc.s    V_4
    IL_016c:  /* 0A   |                  */ stloc.0
    IL_016d:  /* 08   |                  */ ldloc.2
    IL_016e:  /* 17   |                  */ ldc.i4.1
    IL_016f:  /* 58   |                  */ add
    IL_0170:  /* 0C   |                  */ stloc.2
    IL_0171:  /* 08   |                  */ ldloc.2
    IL_0172:  /* 07   |                  */ ldloc.1
    IL_0173:  /* 8E   |                  */ ldlen
    IL_0174:  /* 69   |                  */ conv.i4
    IL_0175:  /* 3F   | ABFEFFFF         */ blt        IL_0025

    IL_017a:  /* 06   |                  */ ldloc.0
    IL_017b:  /* 2D   | 06               */ brtrue.s   IL_0183

    IL_017d:  /* 7E   | (04)00001B       */ ldsfld     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::DefaultType /* 0400001B */
    IL_0182:  /* 2A   |                  */ ret

    IL_0183:  /* 06   |                  */ ldloc.0
    IL_0184:  /* 16   |                  */ ldc.i4.0
    IL_0185:  /* 14   |                  */ ldnull
    IL_0186:  /* 17   |                  */ ldc.i4.1
    IL_0187:  /* 28   | (0A)00009A       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::ParseTypeName(string,
                                                                                                                                                                                                                                                                                      int32,
                                                                                                                                                                                                                                                                                      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ParseOptions/*0100008B*/,
                                                                                                                                                                                                                                                                                      bool) /* 0A00009A */
    IL_018c:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::CalculateTypeFromPolicy

  .method /*06000055*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3b85
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (0A)00009B       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::RoutePatternUnusedParameter /* 0A00009B */
    IL_0006:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_000b:  /* 28   | (2B)00000D       */ call       valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<!!0> [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray/*01000078*/::Create<string>(!!0) /* 2B00000D */
    IL_0010:  /* 7D   | (04)00001C       */ stfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::'<FixableDiagnosticIds>k__BackingField' /* 0400001C */
    IL_0015:  /* 02   |                  */ ldarg.0
    IL_0016:  /* 28   | (0A)000058       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/::.ctor() /* 0A000058 */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::.ctor

  .method /*06000056*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x3ba2
    // Code size       19 (0x13)
    .maxstack  8
    IL_0000:  /* 72   | (70)000467       */ ldstr      "string" /* 70000467 */
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 28   | (0A)00009A       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::ParseTypeName(string,
                                                                                                                                                                                                                                                                                      int32,
                                                                                                                                                                                                                                                                                      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ParseOptions/*0100008B*/,
                                                                                                                                                                                                                                                                                      bool) /* 0A00009A */
    IL_000d:  /* 80   | (04)00001B       */ stsfld     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax/*0100004E*/ Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::DefaultType /* 0400001B */
    IL_0012:  /* 2A   |                  */ ret
  } // end of method RouteParameterUnusedParameterFixer::.cctor

  .property /*17000009*/ instance callconv(8) valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>
          FixableDiagnosticIds()
  {
    .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer/*02000011*/::get_FixableDiagnosticIds() /* 0600004D */
  } // end of property RouteParameterUnusedParameterFixer::FixableDiagnosticIds
} // end of class Microsoft.AspNetCore.Analyzers.RouteEmbeddedLanguage.Fixers.RouteParameterUnusedParameterFixer

.class /*02000012*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer
       extends [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/
{
  .custom /*0C00005C:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00005E:0A000014*/ instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.ExportCodeFixProviderAttribute/*01000021*/::.ctor(string,
                                                                                                                                                                                string[]) /* 0A000014 */ = ( 01 00 02 43 23 00 00 00 00 01 00 54 0E 04 4E 61   // ...C#......T..Na
                                                                                                                                                                                                             6D 65 0F 41 64 64 50 61 63 6B 61 67 65 46 69 78   // me.AddPackageFix
                                                                                                                                                                                                             65 72 )                                           // er
  .custom /*0C00005F:0A000015*/ instance void [System.Composition.AttributedModel/*23000005*/]System.Composition.SharedAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*02000027*/ auto ansi sealed nested private beforefieldinit '<RegisterCodeFixesAsync>d__0'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C0000A7:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000060*/ public int32 '<>1__state'
    .field /*04000061*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ '<>t__builder'
    .field /*04000062*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .field /*04000063*/ public class Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*/ '<>4__this'
    .field /*04000064*/ private class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ '<root>5__2'
    .field /*04000065*/ private class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ '<semanticModel>5__3'
    .field /*04000066*/ private class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> '<wellKnownExtensionMethodCache>5__4'
    .field /*04000067*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> '<>u__1'
    .field /*04000068*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> '<>u__2'
    .field /*04000069*/ private valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> '<>7__wrap4'
    .field /*0400006A*/ private class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ '<diagnostic>5__6'
    .field /*0400006B*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> '<>u__3'
    .method /*06000084*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x5018
      // Code size       998 (0x3e6)
      .maxstack  5
      .locals /*1100002C*/ init (int32 V_0,
               class Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*/ V_1,
               class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ V_2,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_3,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_4,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_5,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ V_6,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_7,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_8,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_9,
               valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ V_10,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_11,
               string V_12,
               class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax/*0100002F*/ V_13,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/ V_14,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ V_15,
               valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/ V_16,
               valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/ V_17,
               valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ V_18,
               valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ V_19,
               valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolInfo/*01000054*/ V_20,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IMethodSymbol/*01000055*/ V_21,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IPropertySymbol/*01000056*/ V_22,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ILocalSymbol/*01000057*/ V_23,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ V_24,
               int32 V_25,
               valuetype [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreInstallPackageData/*01000060*/ V_26,
               class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ V_27,
               valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ V_28,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> V_29,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_30)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000060       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
      IL_0006:  /* 0A   |                  */ stloc.0
      IL_0007:  /* 02   |                  */ ldarg.0
      IL_0008:  /* 7B   | (04)000063       */ ldfld      class Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>4__this' /* 04000063 */
      IL_000d:  /* 0B   |                  */ stloc.1
      .try
      {
        IL_000e:  /* 06   |                  */ ldloc.0
        IL_000f:  /* 45   | 03000000         */ switch     ( 
                  /*      | 59000000         */             IL_0079,
                  /*      | EB000000         */             IL_010b,
                  /*      | 0D030000         */             IL_032d)
        IL_0020:  /* 02   |                  */ ldarg.0
        IL_0021:  /* 7C   | (04)000062       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
        IL_0026:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_002b:  /* 02   |                  */ ldarg.0
        IL_002c:  /* 7C   | (04)000062       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
        IL_0031:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_0036:  /* 6F   | (0A)0000BB       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSyntaxRootAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000BB */
        IL_003b:  /* 16   |                  */ ldc.i4.0
        IL_003c:  /* 6F   | (0A)0000BC       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001B*/::ConfigureAwait(bool) /* 0A0000BC */
        IL_0041:  /* 13   | 05               */ stloc.s    V_5
        IL_0043:  /* 12   | 05               */ ldloca.s   V_5
        IL_0045:  /* 28   | (0A)0000BD       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001C*/::GetAwaiter() /* 0A0000BD */
        IL_004a:  /* 13   | 04               */ stloc.s    V_4
        IL_004c:  /* 12   | 04               */ ldloca.s   V_4
        IL_004e:  /* 28   | (0A)0000BE       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::get_IsCompleted() /* 0A0000BE */
        IL_0053:  /* 2D   | 41               */ brtrue.s   IL_0096

        IL_0055:  /* 02   |                  */ ldarg.0
        IL_0056:  /* 16   |                  */ ldc.i4.0
        IL_0057:  /* 25   |                  */ dup
        IL_0058:  /* 0A   |                  */ stloc.0
        IL_0059:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
        IL_005e:  /* 02   |                  */ ldarg.0
        IL_005f:  /* 11   | 04               */ ldloc.s    V_4
        IL_0061:  /* 7D   | (04)000067       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__1' /* 04000067 */
        IL_0066:  /* 02   |                  */ ldarg.0
        IL_0067:  /* 7C   | (04)000061       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
        IL_006c:  /* 12   | 04               */ ldloca.s   V_4
        IL_006e:  /* 02   |                  */ ldarg.0
        IL_006f:  /* 28   | (2B)000025       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>,valuetype Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               !!1&) /* 2B000025 */
        IL_0074:  /* DD   | 6C030000         */ leave      IL_03e5

        IL_0079:  /* 02   |                  */ ldarg.0
        IL_007a:  /* 7B   | (04)000067       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__1' /* 04000067 */
        IL_007f:  /* 13   | 04               */ stloc.s    V_4
        IL_0081:  /* 02   |                  */ ldarg.0
        IL_0082:  /* 7C   | (04)000067       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__1' /* 04000067 */
        IL_0087:  /* FE15 | (1B)00001D       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/
        IL_008d:  /* 02   |                  */ ldarg.0
        IL_008e:  /* 15   |                  */ ldc.i4.m1
        IL_008f:  /* 25   |                  */ dup
        IL_0090:  /* 0A   |                  */ stloc.0
        IL_0091:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
        IL_0096:  /* 12   | 04               */ ldloca.s   V_4
        IL_0098:  /* 28   | (0A)0000C0       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::GetResult() /* 0A0000C0 */
        IL_009d:  /* 0D   |                  */ stloc.3
        IL_009e:  /* 02   |                  */ ldarg.0
        IL_009f:  /* 09   |                  */ ldloc.3
        IL_00a0:  /* 7D   | (04)000064       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<root>5__2' /* 04000064 */
        IL_00a5:  /* 02   |                  */ ldarg.0
        IL_00a6:  /* 7B   | (04)000064       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<root>5__2' /* 04000064 */
        IL_00ab:  /* 2D   | 05               */ brtrue.s   IL_00b2

        IL_00ad:  /* DD   | 0B030000         */ leave      IL_03bd

        IL_00b2:  /* 02   |                  */ ldarg.0
        IL_00b3:  /* 7C   | (04)000062       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
        IL_00b8:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_00bd:  /* 02   |                  */ ldarg.0
        IL_00be:  /* 7C   | (04)000062       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
        IL_00c3:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_00c8:  /* 6F   | (0A)0000C6       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSemanticModelAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000C6 */
        IL_00cd:  /* 16   |                  */ ldc.i4.0
        IL_00ce:  /* 6F   | (0A)0000C7       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001E*/::ConfigureAwait(bool) /* 0A0000C7 */
        IL_00d3:  /* 13   | 08               */ stloc.s    V_8
        IL_00d5:  /* 12   | 08               */ ldloca.s   V_8
        IL_00d7:  /* 28   | (0A)0000C8       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001F*/::GetAwaiter() /* 0A0000C8 */
        IL_00dc:  /* 13   | 07               */ stloc.s    V_7
        IL_00de:  /* 12   | 07               */ ldloca.s   V_7
        IL_00e0:  /* 28   | (0A)0000C9       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::get_IsCompleted() /* 0A0000C9 */
        IL_00e5:  /* 2D   | 41               */ brtrue.s   IL_0128

        IL_00e7:  /* 02   |                  */ ldarg.0
        IL_00e8:  /* 17   |                  */ ldc.i4.1
        IL_00e9:  /* 25   |                  */ dup
        IL_00ea:  /* 0A   |                  */ stloc.0
        IL_00eb:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
        IL_00f0:  /* 02   |                  */ ldarg.0
        IL_00f1:  /* 11   | 07               */ ldloc.s    V_7
        IL_00f3:  /* 7D   | (04)000068       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__2' /* 04000068 */
        IL_00f8:  /* 02   |                  */ ldarg.0
        IL_00f9:  /* 7C   | (04)000061       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
        IL_00fe:  /* 12   | 07               */ ldloca.s   V_7
        IL_0100:  /* 02   |                  */ ldarg.0
        IL_0101:  /* 28   | (2B)000026       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>,valuetype Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  !!1&) /* 2B000026 */
        IL_0106:  /* DD   | DA020000         */ leave      IL_03e5

        IL_010b:  /* 02   |                  */ ldarg.0
        IL_010c:  /* 7B   | (04)000068       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__2' /* 04000068 */
        IL_0111:  /* 13   | 07               */ stloc.s    V_7
        IL_0113:  /* 02   |                  */ ldarg.0
        IL_0114:  /* 7C   | (04)000068       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__2' /* 04000068 */
        IL_0119:  /* FE15 | (1B)000020       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/
        IL_011f:  /* 02   |                  */ ldarg.0
        IL_0120:  /* 15   |                  */ ldc.i4.m1
        IL_0121:  /* 25   |                  */ dup
        IL_0122:  /* 0A   |                  */ stloc.0
        IL_0123:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
        IL_0128:  /* 12   | 07               */ ldloca.s   V_7
        IL_012a:  /* 28   | (0A)0000CA       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::GetResult() /* 0A0000CA */
        IL_012f:  /* 13   | 06               */ stloc.s    V_6
        IL_0131:  /* 02   |                  */ ldarg.0
        IL_0132:  /* 11   | 06               */ ldloc.s    V_6
        IL_0134:  /* 7D   | (04)000065       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<semanticModel>5__3' /* 04000065 */
        IL_0139:  /* 02   |                  */ ldarg.0
        IL_013a:  /* 7B   | (04)000065       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<semanticModel>5__3' /* 04000065 */
        IL_013f:  /* 2D   | 05               */ brtrue.s   IL_0146

        IL_0141:  /* DD   | 77020000         */ leave      IL_03bd

        IL_0146:  /* 02   |                  */ ldarg.0
        IL_0147:  /* 7B   | (04)000065       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<semanticModel>5__3' /* 04000065 */
        IL_014c:  /* 6F   | (0A)0000CB       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Compilation/*0100008F*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/::get_Compilation() /* 0A0000CB */
        IL_0151:  /* 28   | (0A)0000CC       */ call       class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/::GetOrCreate(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Compilation/*0100008F*/) /* 0A0000CC */
        IL_0156:  /* 0C   |                  */ stloc.2
        IL_0157:  /* 02   |                  */ ldarg.0
        IL_0158:  /* 08   |                  */ ldloc.2
        IL_0159:  /* 28   | (06)000008       */ call       class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> ExtensionMethodsCache/*02000008*/::ConstructFromWellKnownTypes(class [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.App.Analyzers.Infrastructure.WellKnownTypes/*01000017*/) /* 06000008 */
        IL_015e:  /* 7D   | (04)000066       */ stfld      class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<wellKnownExtensionMethodCache>5__4' /* 04000066 */
        IL_0163:  /* 02   |                  */ ldarg.0
        IL_0164:  /* 02   |                  */ ldarg.0
        IL_0165:  /* 7C   | (04)000062       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
        IL_016a:  /* 28   | (0A)000068       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Diagnostics() /* 0A000068 */
        IL_016f:  /* 13   | 09               */ stloc.s    V_9
        IL_0171:  /* 12   | 09               */ ldloca.s   V_9
        IL_0173:  /* 28   | (0A)000069       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<!0> valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000010*/::GetEnumerator() /* 0A000069 */
        IL_0178:  /* 7D   | (04)000069       */ stfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>7__wrap4' /* 04000069 */
        IL_017d:  /* 38   | EF010000         */ br         IL_0371

        IL_0182:  /* 02   |                  */ ldarg.0
        IL_0183:  /* 02   |                  */ ldarg.0
        IL_0184:  /* 7C   | (04)000069       */ ldflda     valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>7__wrap4' /* 04000069 */
        IL_0189:  /* 28   | (0A)00006A       */ call       instance !0 valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::get_Current() /* 0A00006A */
        IL_018e:  /* 7D   | (04)00006A       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<diagnostic>5__6' /* 0400006A */
        IL_0193:  /* 02   |                  */ ldarg.0
        IL_0194:  /* 7B   | (04)00006A       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<diagnostic>5__6' /* 0400006A */
        IL_0199:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
        IL_019e:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
        IL_01a3:  /* 13   | 0A               */ stloc.s    V_10
        IL_01a5:  /* 02   |                  */ ldarg.0
        IL_01a6:  /* 7B   | (04)000064       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<root>5__2' /* 04000064 */
        IL_01ab:  /* 11   | 0A               */ ldloc.s    V_10
        IL_01ad:  /* 16   |                  */ ldc.i4.0
        IL_01ae:  /* 16   |                  */ ldc.i4.0
        IL_01af:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                        bool,
                                                                                                                                                                                                                                                        bool) /* 0A000039 */
        IL_01b4:  /* 13   | 0B               */ stloc.s    V_11
        IL_01b6:  /* 11   | 0B               */ ldloc.s    V_11
        IL_01b8:  /* 2D   | 05               */ brtrue.s   IL_01bf

        IL_01ba:  /* DD   | FE010000         */ leave      IL_03bd

        IL_01bf:  /* 11   | 0B               */ ldloc.s    V_11
        IL_01c1:  /* 75   | (01)00002F       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax/*0100002F*/
        IL_01c6:  /* 13   | 0D               */ stloc.s    V_13
        IL_01c8:  /* 11   | 0D               */ ldloc.s    V_13
        IL_01ca:  /* 2D   | 03               */ brtrue.s   IL_01cf

        IL_01cc:  /* 14   |                  */ ldnull
        IL_01cd:  /* 2B   | 10               */ br.s       IL_01df

        IL_01cf:  /* 11   | 0D               */ ldloc.s    V_13
        IL_01d1:  /* 6F   | (0A)00005D       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/::get_Identifier() /* 0A00005D */
        IL_01d6:  /* 13   | 12               */ stloc.s    V_18
        IL_01d8:  /* 12   | 12               */ ldloca.s   V_18
        IL_01da:  /* 28   | (0A)0000A5       */ call       instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/::get_Text() /* 0A0000A5 */
        IL_01df:  /* 13   | 0C               */ stloc.s    V_12
        IL_01e1:  /* 11   | 0C               */ ldloc.s    V_12
        IL_01e3:  /* 2D   | 05               */ brtrue.s   IL_01ea

        IL_01e5:  /* DD   | D3010000         */ leave      IL_03bd

        IL_01ea:  /* 11   | 0B               */ ldloc.s    V_11
        IL_01ec:  /* 6F   | (0A)000079       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::get_Parent() /* 0A000079 */
        IL_01f1:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
        IL_01f6:  /* 2D   | 05               */ brtrue.s   IL_01fd

        IL_01f8:  /* DD   | C0010000         */ leave      IL_03bd

        IL_01fd:  /* 02   |                  */ ldarg.0
        IL_01fe:  /* 7B   | (04)000065       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<semanticModel>5__3' /* 04000065 */
        IL_0203:  /* 11   | 0B               */ ldloc.s    V_11
        IL_0205:  /* 6F   | (0A)000079       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::get_Parent() /* 0A000079 */
        IL_020a:  /* 74   | (01)000031       */ castclass  [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
        IL_020f:  /* 6F   | (0A)00003E       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Expression() /* 0A00003E */
        IL_0214:  /* 12   | 13               */ ldloca.s   V_19
        IL_0216:  /* FE15 | (01)00003B       */ initobj    [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/
        IL_021c:  /* 11   | 13               */ ldloc.s    V_19
        IL_021e:  /* 28   | (0A)0000E7       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolInfo/*01000054*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ModelExtensions/*01000094*/::GetSymbolInfo(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                             valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000E7 */
        IL_0223:  /* 13   | 14               */ stloc.s    V_20
        IL_0225:  /* 12   | 14               */ ldloca.s   V_20
        IL_0227:  /* 28   | (0A)0000DB       */ call       instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ISymbol/*01000053*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SymbolInfo/*01000054*/::get_Symbol() /* 0A0000DB */
        IL_022c:  /* 13   | 0E               */ stloc.s    V_14
        IL_022e:  /* 11   | 0E               */ ldloc.s    V_14
        IL_0230:  /* 75   | (01)000055       */ isinst     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IMethodSymbol/*01000055*/
        IL_0235:  /* 13   | 15               */ stloc.s    V_21
        IL_0237:  /* 11   | 15               */ ldloc.s    V_21
        IL_0239:  /* 2D   | 1C               */ brtrue.s   IL_0257

        IL_023b:  /* 11   | 0E               */ ldloc.s    V_14
        IL_023d:  /* 75   | (01)000056       */ isinst     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IPropertySymbol/*01000056*/
        IL_0242:  /* 13   | 16               */ stloc.s    V_22
        IL_0244:  /* 11   | 16               */ ldloc.s    V_22
        IL_0246:  /* 2D   | 1A               */ brtrue.s   IL_0262

        IL_0248:  /* 11   | 0E               */ ldloc.s    V_14
        IL_024a:  /* 75   | (01)000057       */ isinst     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ILocalSymbol/*01000057*/
        IL_024f:  /* 13   | 17               */ stloc.s    V_23
        IL_0251:  /* 11   | 17               */ ldloc.s    V_23
        IL_0253:  /* 2D   | 18               */ brtrue.s   IL_026d

        IL_0255:  /* 2B   | 21               */ br.s       IL_0278

        IL_0257:  /* 11   | 15               */ ldloc.s    V_21
        IL_0259:  /* 6F   | (0A)0000DC       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IMethodSymbol/*01000055*/::get_ReturnType() /* 0A0000DC */
        IL_025e:  /* 13   | 18               */ stloc.s    V_24
        IL_0260:  /* 2B   | 19               */ br.s       IL_027b

        IL_0262:  /* 11   | 16               */ ldloc.s    V_22
        IL_0264:  /* 6F   | (0A)0000DD       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.IPropertySymbol/*01000056*/::get_Type() /* 0A0000DD */
        IL_0269:  /* 13   | 18               */ stloc.s    V_24
        IL_026b:  /* 2B   | 0E               */ br.s       IL_027b

        IL_026d:  /* 11   | 17               */ ldloc.s    V_23
        IL_026f:  /* 6F   | (0A)0000DE       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ILocalSymbol/*01000057*/::get_Type() /* 0A0000DE */
        IL_0274:  /* 13   | 18               */ stloc.s    V_24
        IL_0276:  /* 2B   | 03               */ br.s       IL_027b

        IL_0278:  /* 14   |                  */ ldnull
        IL_0279:  /* 13   | 18               */ stloc.s    V_24
        IL_027b:  /* 11   | 18               */ ldloc.s    V_24
        IL_027d:  /* 13   | 0F               */ stloc.s    V_15
        IL_027f:  /* 11   | 0F               */ ldloc.s    V_15
        IL_0281:  /* 2D   | 05               */ brtrue.s   IL_0288

        IL_0283:  /* DD   | 35010000         */ leave      IL_03bd

        IL_0288:  /* 12   | 10               */ ldloca.s   V_16
        IL_028a:  /* 11   | 0F               */ ldloc.s    V_15
        IL_028c:  /* 11   | 0C               */ ldloc.s    V_12
        IL_028e:  /* 28   | (06)00002C       */ call       instance void Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/::.ctor(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.ITypeSymbol/*01000020*/,
                                                                                                                                                  string) /* 0600002C */
        IL_0293:  /* 02   |                  */ ldarg.0
        IL_0294:  /* 7B   | (04)000066       */ ldfld      class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<wellKnownExtensionMethodCache>5__4' /* 04000066 */
        IL_0299:  /* 11   | 10               */ ldloc.s    V_16
        IL_029b:  /* 12   | 11               */ ldloca.s   V_17
        IL_029d:  /* 6F   | (0A)0000E8       */ callvirt   instance bool class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/>/*1B000002*/::TryGetValue(!0,
                                                                                                                                                                                                                                                                                                                                                 !1&) /* 0A0000E8 */
        IL_02a2:  /* 39   | C3000000         */ brfalse    IL_036a

        IL_02a7:  /* 02   |                  */ ldarg.0
        IL_02a8:  /* 7B   | (04)00006A       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<diagnostic>5__6' /* 0400006A */
        IL_02ad:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
        IL_02b2:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
        IL_02b7:  /* 13   | 1C               */ stloc.s    V_28
        IL_02b9:  /* 12   | 1C               */ ldloca.s   V_28
        IL_02bb:  /* 28   | (0A)0000D7       */ call       instance int32 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/::get_Start() /* 0A0000D7 */
        IL_02c0:  /* 13   | 19               */ stloc.s    V_25
        IL_02c2:  /* 12   | 1A               */ ldloca.s   V_26
        IL_02c4:  /* 14   |                  */ ldnull
        IL_02c5:  /* 12   | 11               */ ldloca.s   V_17
        IL_02c7:  /* 28   | (06)000020       */ call       instance string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::get_packageName() /* 06000020 */
        IL_02cc:  /* 14   |                  */ ldnull
        IL_02cd:  /* 12   | 11               */ ldloca.s   V_17
        IL_02cf:  /* 28   | (06)000022       */ call       instance string Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/::get_namespaceName() /* 06000022 */
        IL_02d4:  /* 28   | (0A)0000E9       */ call       instance void [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreInstallPackageData/*01000060*/::.ctor(string,
                                                                                                                                                                                                                                                   string,
                                                                                                                                                                                                                                                   string,
                                                                                                                                                                                                                                                   string) /* 0A0000E9 */
        IL_02d9:  /* 07   |                  */ ldloc.1
        IL_02da:  /* 02   |                  */ ldarg.0
        IL_02db:  /* 7C   | (04)000062       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
        IL_02e0:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_02e5:  /* 11   | 19               */ ldloc.s    V_25
        IL_02e7:  /* 11   | 1A               */ ldloc.s    V_26
        IL_02e9:  /* 02   |                  */ ldarg.0
        IL_02ea:  /* 7C   | (04)000062       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
        IL_02ef:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_02f4:  /* 6F   | (06)00005A       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*/::TryCreateCodeActionAsync(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                                               int32,
                                                                                                                                                                                                                                                                                                                                                               valuetype [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreInstallPackageData/*01000060*/,
                                                                                                                                                                                                                                                                                                                                                               valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0600005A */
        IL_02f9:  /* 6F   | (0A)0000EA       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000025*/::GetAwaiter() /* 0A0000EA */
        IL_02fe:  /* 13   | 1D               */ stloc.s    V_29
        IL_0300:  /* 12   | 1D               */ ldloca.s   V_29
        IL_0302:  /* 28   | (0A)0000EB       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000026*/::get_IsCompleted() /* 0A0000EB */
        IL_0307:  /* 2D   | 41               */ brtrue.s   IL_034a

        IL_0309:  /* 02   |                  */ ldarg.0
        IL_030a:  /* 18   |                  */ ldc.i4.2
        IL_030b:  /* 25   |                  */ dup
        IL_030c:  /* 0A   |                  */ stloc.0
        IL_030d:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
        IL_0312:  /* 02   |                  */ ldarg.0
        IL_0313:  /* 11   | 1D               */ ldloc.s    V_29
        IL_0315:  /* 7D   | (04)00006B       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__3' /* 0400006B */
        IL_031a:  /* 02   |                  */ ldarg.0
        IL_031b:  /* 7C   | (04)000061       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
        IL_0320:  /* 12   | 1D               */ ldloca.s   V_29
        IL_0322:  /* 02   |                  */ ldarg.0
        IL_0323:  /* 28   | (2B)000027       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>,valuetype Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        !!1&) /* 2B000027 */
        IL_0328:  /* DD   | B8000000         */ leave      IL_03e5

        IL_032d:  /* 02   |                  */ ldarg.0
        IL_032e:  /* 7B   | (04)00006B       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__3' /* 0400006B */
        IL_0333:  /* 13   | 1D               */ stloc.s    V_29
        IL_0335:  /* 02   |                  */ ldarg.0
        IL_0336:  /* 7C   | (04)00006B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>u__3' /* 0400006B */
        IL_033b:  /* FE15 | (1B)000026       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000026*/
        IL_0341:  /* 02   |                  */ ldarg.0
        IL_0342:  /* 15   |                  */ ldc.i4.m1
        IL_0343:  /* 25   |                  */ dup
        IL_0344:  /* 0A   |                  */ stloc.0
        IL_0345:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
        IL_034a:  /* 12   | 1D               */ ldloca.s   V_29
        IL_034c:  /* 28   | (0A)0000EC       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000026*/::GetResult() /* 0A0000EC */
        IL_0351:  /* 13   | 1B               */ stloc.s    V_27
        IL_0353:  /* 11   | 1B               */ ldloc.s    V_27
        IL_0355:  /* 2C   | 13               */ brfalse.s  IL_036a

        IL_0357:  /* 02   |                  */ ldarg.0
        IL_0358:  /* 7C   | (04)000062       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
        IL_035d:  /* 11   | 1B               */ ldloc.s    V_27
        IL_035f:  /* 02   |                  */ ldarg.0
        IL_0360:  /* 7B   | (04)00006A       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<diagnostic>5__6' /* 0400006A */
        IL_0365:  /* 28   | (0A)000070       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::RegisterCodeFix(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/,
                                                                                                                                                                                                     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/) /* 0A000070 */
        IL_036a:  /* 02   |                  */ ldarg.0
        IL_036b:  /* 14   |                  */ ldnull
        IL_036c:  /* 7D   | (04)00006A       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<diagnostic>5__6' /* 0400006A */
        IL_0371:  /* 02   |                  */ ldarg.0
        IL_0372:  /* 7C   | (04)000069       */ ldflda     valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>7__wrap4' /* 04000069 */
        IL_0377:  /* 28   | (0A)000071       */ call       instance bool valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::MoveNext() /* 0A000071 */
        IL_037c:  /* 3A   | 01FEFFFF         */ brtrue     IL_0182

        IL_0381:  /* 02   |                  */ ldarg.0
        IL_0382:  /* 7C   | (04)000069       */ ldflda     valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>7__wrap4' /* 04000069 */
        IL_0387:  /* FE15 | (1B)000011       */ initobj    valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/
        IL_038d:  /* DE   | 2E               */ leave.s    IL_03bd

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_038f:  /* 13   | 1E               */ stloc.s    V_30
        IL_0391:  /* 02   |                  */ ldarg.0
        IL_0392:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_0394:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
        IL_0399:  /* 02   |                  */ ldarg.0
        IL_039a:  /* 14   |                  */ ldnull
        IL_039b:  /* 7D   | (04)000064       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<root>5__2' /* 04000064 */
        IL_03a0:  /* 02   |                  */ ldarg.0
        IL_03a1:  /* 14   |                  */ ldnull
        IL_03a2:  /* 7D   | (04)000065       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<semanticModel>5__3' /* 04000065 */
        IL_03a7:  /* 02   |                  */ ldarg.0
        IL_03a8:  /* 14   |                  */ ldnull
        IL_03a9:  /* 7D   | (04)000066       */ stfld      class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<wellKnownExtensionMethodCache>5__4' /* 04000066 */
        IL_03ae:  /* 02   |                  */ ldarg.0
        IL_03af:  /* 7C   | (04)000061       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
        IL_03b4:  /* 11   | 1E               */ ldloc.s    V_30
        IL_03b6:  /* 28   | (0A)0000C3       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000C3 */
        IL_03bb:  /* DE   | 28               */ leave.s    IL_03e5

      }  // end handler
      // HEX: 00 00 00 00 0E 00 00 00 81 03 00 00 8F 03 00 00 2E 00 00 00 40 00 00 01
      IL_03bd:  /* 02   |                  */ ldarg.0
      IL_03be:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_03c0:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
      IL_03c5:  /* 02   |                  */ ldarg.0
      IL_03c6:  /* 14   |                  */ ldnull
      IL_03c7:  /* 7D   | (04)000064       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<root>5__2' /* 04000064 */
      IL_03cc:  /* 02   |                  */ ldarg.0
      IL_03cd:  /* 14   |                  */ ldnull
      IL_03ce:  /* 7D   | (04)000065       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<semanticModel>5__3' /* 04000065 */
      IL_03d3:  /* 02   |                  */ ldarg.0
      IL_03d4:  /* 14   |                  */ ldnull
      IL_03d5:  /* 7D   | (04)000066       */ stfld      class [netstandard/*23000001*/]System.Collections.Generic.Dictionary`2/*01000016*/<valuetype Microsoft.AspNetCore.Analyzers.ThisAndExtensionMethod/*0200000B*/,valuetype Microsoft.AspNetCore.Analyzers.PackageSourceAndNamespace/*0200000A*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<wellKnownExtensionMethodCache>5__4' /* 04000066 */
      IL_03da:  /* 02   |                  */ ldarg.0
      IL_03db:  /* 7C   | (04)000061       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
      IL_03e0:  /* 28   | (0A)0000C4       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetResult() /* 0A0000C4 */
      IL_03e5:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__0'::MoveNext

    .method /*06000085*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000D8:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x5428
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)000061       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000C5       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000C5 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__0'::SetStateMachine

  } // end of class '<RegisterCodeFixesAsync>d__0'

  .class /*02000028*/ auto ansi sealed nested private beforefieldinit '<TryCreateCodeActionAsync>d__5'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C0000AA:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*0400006C*/ public int32 '<>1__state'
    .field /*0400006D*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> '<>t__builder'
    .field /*0400006E*/ public class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document
    .field /*0400006F*/ public int32 position
    .field /*04000070*/ public valuetype [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreInstallPackageData/*01000060*/ packageInstallData
    .field /*04000071*/ public valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken
    .field /*04000072*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> '<>u__1'
    .method /*06000086*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x5438
      // Code size       168 (0xa8)
      .maxstack  4
      .locals /*1100002D*/ init (int32 V_0,
               class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ V_1,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> V_2,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_3)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)00006C       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>1__state' /* 0400006C */
      IL_0006:  /* 0A   |                  */ stloc.0
      .try
      {
        IL_0007:  /* 06   |                  */ ldloc.0
        IL_0008:  /* 2C   | 4C               */ brfalse.s  IL_0056

        IL_000a:  /* 02   |                  */ ldarg.0
        IL_000b:  /* 7B   | (04)00006E       */ ldfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::document /* 0400006E */
        IL_0010:  /* 02   |                  */ ldarg.0
        IL_0011:  /* 7B   | (04)00006F       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::position /* 0400006F */
        IL_0016:  /* 02   |                  */ ldarg.0
        IL_0017:  /* 7B   | (04)000070       */ ldfld      valuetype [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreInstallPackageData/*01000060*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::packageInstallData /* 04000070 */
        IL_001c:  /* 02   |                  */ ldarg.0
        IL_001d:  /* 7B   | (04)000071       */ ldfld      valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::cancellationToken /* 04000071 */
        IL_0022:  /* 28   | (0A)0000ED       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreAddPackageCodeAction/*01000095*/::TryCreateCodeActionAsync(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                                                                                                                                   int32,
                                                                                                                                                                                                                                                                                                                                                                                                                                                   valuetype [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreInstallPackageData/*01000060*/,
                                                                                                                                                                                                                                                                                                                                                                                                                                                   valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000ED */
        IL_0027:  /* 6F   | (0A)0000EA       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000025*/::GetAwaiter() /* 0A0000EA */
        IL_002c:  /* 0C   |                  */ stloc.2
        IL_002d:  /* 12   | 02               */ ldloca.s   V_2
        IL_002f:  /* 28   | (0A)0000EB       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000026*/::get_IsCompleted() /* 0A0000EB */
        IL_0034:  /* 2D   | 3C               */ brtrue.s   IL_0072

        IL_0036:  /* 02   |                  */ ldarg.0
        IL_0037:  /* 16   |                  */ ldc.i4.0
        IL_0038:  /* 25   |                  */ dup
        IL_0039:  /* 0A   |                  */ stloc.0
        IL_003a:  /* 7D   | (04)00006C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>1__state' /* 0400006C */
        IL_003f:  /* 02   |                  */ ldarg.0
        IL_0040:  /* 08   |                  */ ldloc.2
        IL_0041:  /* 7D   | (04)000072       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>u__1' /* 04000072 */
        IL_0046:  /* 02   |                  */ ldarg.0
        IL_0047:  /* 7C   | (04)00006D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>t__builder' /* 0400006D */
        IL_004c:  /* 12   | 02               */ ldloca.s   V_2
        IL_004e:  /* 02   |                  */ ldarg.0
        IL_004f:  /* 28   | (2B)000028       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000017*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>,valuetype Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  !!1&) /* 2B000028 */
        IL_0054:  /* DE   | 51               */ leave.s    IL_00a7

        IL_0056:  /* 02   |                  */ ldarg.0
        IL_0057:  /* 7B   | (04)000072       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>u__1' /* 04000072 */
        IL_005c:  /* 0C   |                  */ stloc.2
        IL_005d:  /* 02   |                  */ ldarg.0
        IL_005e:  /* 7C   | (04)000072       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>u__1' /* 04000072 */
        IL_0063:  /* FE15 | (1B)000026       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000026*/
        IL_0069:  /* 02   |                  */ ldarg.0
        IL_006a:  /* 15   |                  */ ldc.i4.m1
        IL_006b:  /* 25   |                  */ dup
        IL_006c:  /* 0A   |                  */ stloc.0
        IL_006d:  /* 7D   | (04)00006C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>1__state' /* 0400006C */
        IL_0072:  /* 12   | 02               */ ldloca.s   V_2
        IL_0074:  /* 28   | (0A)0000EC       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000061*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000026*/::GetResult() /* 0A0000EC */
        IL_0079:  /* 0B   |                  */ stloc.1
        IL_007a:  /* DE   | 17               */ leave.s    IL_0093

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_007c:  /* 0D   |                  */ stloc.3
        IL_007d:  /* 02   |                  */ ldarg.0
        IL_007e:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_0080:  /* 7D   | (04)00006C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>1__state' /* 0400006C */
        IL_0085:  /* 02   |                  */ ldarg.0
        IL_0086:  /* 7C   | (04)00006D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>t__builder' /* 0400006D */
        IL_008b:  /* 09   |                  */ ldloc.3
        IL_008c:  /* 28   | (0A)0000EF       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000017*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000EF */
        IL_0091:  /* DE   | 14               */ leave.s    IL_00a7

      }  // end handler
      // HEX: 00 00 07 00 75 7C 00 17 40 00 00 01
      IL_0093:  /* 02   |                  */ ldarg.0
      IL_0094:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_0096:  /* 7D   | (04)00006C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>1__state' /* 0400006C */
      IL_009b:  /* 02   |                  */ ldarg.0
      IL_009c:  /* 7C   | (04)00006D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>t__builder' /* 0400006D */
      IL_00a1:  /* 07   |                  */ ldloc.1
      IL_00a2:  /* 28   | (0A)0000F0       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000017*/::SetResult(!0) /* 0A0000F0 */
      IL_00a7:  /* 2A   |                  */ ret
    } // end of method '<TryCreateCodeActionAsync>d__5'::MoveNext

    .method /*06000087*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000DA:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x54fc
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)00006D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>t__builder' /* 0400006D */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000F1       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000017*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000F1 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<TryCreateCodeActionAsync>d__5'::SetStateMachine

  } // end of class '<TryCreateCodeActionAsync>d__5'

  .field /*0400001D*/ private initonly valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> '<FixableDiagnosticIds>k__BackingField'
  .custom /*0C000084:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*06000057*/ public hidebysig virtual 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          RegisterCodeFixesAsync(valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context) cil managed
  // SIG: 20 01 12 80 A1 11 80 A5
  {
    .custom /*0C0000C8:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 58 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..XMicrosoft.Asp
                                                                                                                                                                                                                                  4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65 72   // NetCore.Analyzer
                                                                                                                                                                                                                                  73 2E 44 65 70 65 6E 64 65 6E 63 69 65 73 2E 41   // s.Dependencies.A
                                                                                                                                                                                                                                  64 64 50 61 63 6B 61 67 65 46 69 78 65 72 2B 3C   // ddPackageFixer+<
                                                                                                                                                                                                                                  52 65 67 69 73 74 65 72 43 6F 64 65 46 69 78 65   // RegisterCodeFixe
                                                                                                                                                                                                                                  73 41 73 79 6E 63 3E 64 5F 5F 30 00 00 )          // sAsync>d__0..
    // Method begins at RVA 0x3bb8
    // Code size       63 (0x3f)
    .maxstack  2
    .locals /*11000021*/ init (valuetype Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)000034       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Create() /* 0A000034 */
    IL_0007:  /* 7D   | (04)000061       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7D   | (04)000063       */ stfld      class Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>4__this' /* 04000063 */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 03   |                  */ ldarg.1
    IL_0017:  /* 7D   | (04)000062       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::context /* 04000062 */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 15   |                  */ ldc.i4.m1
    IL_001f:  /* 7D   | (04)000060       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>1__state' /* 04000060 */
    IL_0024:  /* 12   | 00               */ ldloca.s   V_0
    IL_0026:  /* 7C   | (04)000061       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
    IL_002b:  /* 12   | 00               */ ldloca.s   V_0
    IL_002d:  /* 28   | (2B)000013       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Start<valuetype Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/>(!!0&) /* 2B000013 */
    IL_0032:  /* 12   | 00               */ ldloca.s   V_0
    IL_0034:  /* 7C   | (04)000061       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<RegisterCodeFixesAsync>d__0'/*02000027*/::'<>t__builder' /* 04000061 */
    IL_0039:  /* 28   | (0A)000036       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::get_Task() /* 0A000036 */
    IL_003e:  /* 2A   |                  */ ret
  } // end of method AddPackageFixer::RegisterCodeFixesAsync

  .method /*06000058*/ public hidebysig virtual final 
          instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ 
          GetFixAllProvider() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x3c03
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 28   | (0A)000033       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders/*0100006F*/::get_BatchFixer() /* 0A000033 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method AddPackageFixer::GetFixAllProvider

  .method /*06000059*/ public hidebysig specialname virtual 
          instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> 
          get_FixableDiagnosticIds() cil managed
  // SIG: 20 00 15 11 80 91 01 0E
  {
    .custom /*0C0000C9:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .param [0]/*08000086*/ 
    .custom /*0C0000D9:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x3c0a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00001D       */ ldfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*/::'<FixableDiagnosticIds>k__BackingField' /* 0400001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AddPackageFixer::get_FixableDiagnosticIds

  .method /*0600005A*/ assembly hidebysig newslot strict virtual 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> 
          TryCreateCodeActionAsync(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                                   int32 position,
                                   valuetype [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreInstallPackageData/*01000060*/ packageInstallData,
                                   valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 A9 01 12 81 7D 12 80 AD 08 11 81 81 11 80 ED
  {
    .custom /*0C0000CA:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 5A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..ZMicrosoft.Asp
                                                                                                                                                                                                                                  4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65 72   // NetCore.Analyzer
                                                                                                                                                                                                                                  73 2E 44 65 70 65 6E 64 65 6E 63 69 65 73 2E 41   // s.Dependencies.A
                                                                                                                                                                                                                                  64 64 50 61 63 6B 61 67 65 46 69 78 65 72 2B 3C   // ddPackageFixer+<
                                                                                                                                                                                                                                  54 72 79 43 72 65 61 74 65 43 6F 64 65 41 63 74   // TryCreateCodeAct
                                                                                                                                                                                                                                  69 6F 6E 41 73 79 6E 63 3E 64 5F 5F 35 00 00 )    // ionAsync>d__5..
    .param [0]/*08000087*/ 
    .custom /*0C0000DB:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x3c14
    // Code size       80 (0x50)
    .maxstack  2
    .locals /*11000022*/ init (valuetype Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)00009C       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000017*/::Create() /* 0A00009C */
    IL_0007:  /* 7D   | (04)00006D       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>t__builder' /* 0400006D */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 7D   | (04)00006E       */ stfld      class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::document /* 0400006E */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 04   |                  */ ldarg.2
    IL_0017:  /* 7D   | (04)00006F       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::position /* 0400006F */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 05   |                  */ ldarg.3
    IL_001f:  /* 7D   | (04)000070       */ stfld      valuetype [Microsoft.CodeAnalysis.ExternalAccess.AspNetCore/*23000009*/]Microsoft.CodeAnalysis.ExternalAccess.AspNetCore.AddPackage.AspNetCoreInstallPackageData/*01000060*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::packageInstallData /* 04000070 */
    IL_0024:  /* 12   | 00               */ ldloca.s   V_0
    IL_0026:  /* 0E   | 04               */ ldarg.s    cancellationToken
    IL_0028:  /* 7D   | (04)000071       */ stfld      valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::cancellationToken /* 04000071 */
    IL_002d:  /* 12   | 00               */ ldloca.s   V_0
    IL_002f:  /* 15   |                  */ ldc.i4.m1
    IL_0030:  /* 7D   | (04)00006C       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>1__state' /* 0400006C */
    IL_0035:  /* 12   | 00               */ ldloca.s   V_0
    IL_0037:  /* 7C   | (04)00006D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>t__builder' /* 0400006D */
    IL_003c:  /* 12   | 00               */ ldloca.s   V_0
    IL_003e:  /* 28   | (2B)000014       */ call       instance void valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000017*/::Start<valuetype Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/>(!!0&) /* 2B000014 */
    IL_0043:  /* 12   | 00               */ ldloca.s   V_0
    IL_0045:  /* 7C   | (04)00006D       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*//'<TryCreateCodeActionAsync>d__5'/*02000028*/::'<>t__builder' /* 0400006D */
    IL_004a:  /* 28   | (0A)00009E       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004C*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/>/*1B000017*/::get_Task() /* 0A00009E */
    IL_004f:  /* 2A   |                  */ ret
  } // end of method AddPackageFixer::TryCreateCodeActionAsync

  .method /*0600005B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3c70
    // Code size       32 (0x20)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 17   |                  */ ldc.i4.1
    IL_0002:  /* 8D   | (01)00006E       */ newarr     [netstandard/*23000001*/]System.String/*0100006E*/
    IL_0007:  /* 25   |                  */ dup
    IL_0008:  /* 16   |                  */ ldc.i4.0
    IL_0009:  /* 72   | (70)000475       */ ldstr      "CS1061" /* 70000475 */
    IL_000e:  /* A2   |                  */ stelem.ref
    IL_000f:  /* 28   | (2B)000015       */ call       valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<!!0> [System.Collections.Immutable/*23000006*/]System.Runtime.InteropServices.ImmutableCollectionsMarshal/*0100008C*/::AsImmutableArray<string>(!!0[]) /* 2B000015 */
    IL_0014:  /* 7D   | (04)00001D       */ stfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*/::'<FixableDiagnosticIds>k__BackingField' /* 0400001D */
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 28   | (0A)000058       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/::.ctor() /* 0A000058 */
    IL_001f:  /* 2A   |                  */ ret
  } // end of method AddPackageFixer::.ctor

  .property /*1700000A*/ instance callconv(8) valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>
          FixableDiagnosticIds()
  {
    .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer/*02000012*/::get_FixableDiagnosticIds() /* 06000059 */
  } // end of property AddPackageFixer::FixableDiagnosticIds
} // end of class Microsoft.AspNetCore.Analyzers.Dependencies.AddPackageFixer

.class /*02000013*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer
       extends [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/
{
  .custom /*0C000062:06000005*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000005*/::.ctor(uint8) /* 06000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000063:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8) /* 06000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000064:0A000014*/ instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.ExportCodeFixProviderAttribute/*01000021*/::.ctor(string,
                                                                                                                                                                                string[]) /* 0A000014 */ = ( 01 00 02 43 23 00 00 00 00 00 00 )                // ...C#......
  .custom /*0C000065:0A000015*/ instance void [System.Composition.AttributedModel/*23000005*/]System.Composition.SharedAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*02000029*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C0000AE:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000073*/ public static initonly class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/ '<>9'
    .field /*04000074*/ public static class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/,bool> '<>9__6_0'
    .method /*06000088*/ private hidebysig specialname rtspecialname static 
            void  .cctor() cil managed
    // SIG: 00 00 01
    {
      // Method begins at RVA 0x550a
      // Code size       11 (0xb)
      .maxstack  8
      IL_0000:  /* 73   | (06)000089       */ newobj     instance void Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/::.ctor() /* 06000089 */
      IL_0005:  /* 80   | (04)000073       */ stsfld     class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/::'<>9' /* 04000073 */
      IL_000a:  /* 2A   |                  */ ret
    } // end of method '<>c'::.cctor

    .method /*06000089*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x5516
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c'::.ctor

    .method /*0600008A*/ assembly hidebysig 
            instance bool  '<ChainInvocation>b__6_0'(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/ trivia) cil managed
    // SIG: 20 01 02 11 81 95
    {
      // Method begins at RVA 0x551e
      // Code size       15 (0xf)
      .maxstack  8
      IL_0000:  /* 03   |                  */ ldarg.1
      IL_0001:  /* 20   | 5B210000         */ ldc.i4     0x215b
      IL_0006:  /* 28   | (0A)0000F2       */ call       bool [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharpExtensions/*01000081*/::IsKind(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/,
                                                                                                                                                                     valuetype [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxKind/*0100007F*/) /* 0A0000F2 */
      IL_000b:  /* 16   |                  */ ldc.i4.0
      IL_000c:  /* FE01 |                  */ ceq
      IL_000e:  /* 2A   |                  */ ret
    } // end of method '<>c'::'<ChainInvocation>b__6_0'

  } // end of class '<>c'

  .class /*0200002A*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C0000B1:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000075*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root
    .field /*04000076*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .method /*0600008B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x552e
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_0'::.ctor

  } // end of class '<>c__DisplayClass4_0'

  .class /*0200002B*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_1'
         extends [netstandard/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C0000B4:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000077*/ public class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic
    .field /*04000078*/ public class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ invocation
    .field /*04000079*/ public class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ 'CS$<>8__locals1'
    .method /*0600008C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x5536
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)0000B8       */ call       instance void [netstandard/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A0000B8 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::.ctor

    .method /*0600008D*/ assembly hidebysig 
            instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
            '<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ cancellationToken) cil managed
    // SIG: 20 01 15 12 80 A9 01 12 80 AD 11 80 ED
    {
      // Method begins at RVA 0x553e
      // Code size       45 (0x2d)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000077       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::diagnostic /* 04000077 */
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 7B   | (04)000079       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::'CS$<>8__locals1' /* 04000079 */
      IL_000c:  /* 7B   | (04)000075       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::root /* 04000075 */
      IL_0011:  /* 02   |                  */ ldarg.0
      IL_0012:  /* 7B   | (04)000079       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::'CS$<>8__locals1' /* 04000079 */
      IL_0017:  /* 7C   | (04)000076       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::context /* 04000076 */
      IL_001c:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
      IL_0021:  /* 02   |                  */ ldarg.0
      IL_0022:  /* 7B   | (04)000078       */ ldfld      class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::invocation /* 04000078 */
      IL_0027:  /* 28   | (06)000061       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*/::ReplaceWithAddAuthorizationBuilder(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                                                                                                                                                                                     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                                                                                                                                     class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/,
                                                                                                                                                                                                                                                                                                                                                                     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/) /* 06000061 */
      IL_002c:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass4_1'::'<RegisterCodeFixesAsync>b__0'

  } // end of class '<>c__DisplayClass4_1'

  .class /*0200002C*/ auto ansi sealed nested private beforefieldinit '<RegisterCodeFixesAsync>d__4'
         extends [netstandard/*23000001*/]System.ValueType/*01000018*/
         implements [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C0000B5:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*0400007A*/ public int32 '<>1__state'
    .field /*0400007B*/ public valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ '<>t__builder'
    .field /*0400007C*/ public valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context
    .field /*0400007D*/ private class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ '<>8__1'
    .field /*0400007E*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> '<>u__1'
    .field /*0400007F*/ private valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> '<>u__2'
    .method /*0600008E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000018*/ 
      // Method begins at RVA 0x556c
      // Code size       578 (0x242)
      .maxstack  4
      .locals /*1100002E*/ init (int32 V_0,
               class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_1,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_2,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_3,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_4,
               valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> V_5,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_6,
               valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> V_7,
               class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/ V_8,
               class [netstandard/*23000001*/]System.Exception/*01000040*/ V_9)
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)00007A       */ ldfld      int32 Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>1__state' /* 0400007A */
      IL_0006:  /* 0A   |                  */ stloc.0
      .try
      {
        IL_0007:  /* 06   |                  */ ldloc.0
        IL_0008:  /* 39   | 83000000         */ brfalse    IL_0090

        IL_000d:  /* 06   |                  */ ldloc.0
        IL_000e:  /* 17   |                  */ ldc.i4.1
        IL_000f:  /* 3B   | 21010000         */ beq        IL_0135

        IL_0014:  /* 02   |                  */ ldarg.0
        IL_0015:  /* 73   | (06)00008B       */ newobj     instance void Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::.ctor() /* 0600008B */
        IL_001a:  /* 7D   | (04)00007D       */ stfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_001f:  /* 02   |                  */ ldarg.0
        IL_0020:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_0025:  /* 02   |                  */ ldarg.0
        IL_0026:  /* 7B   | (04)00007C       */ ldfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::context /* 0400007C */
        IL_002b:  /* 7D   | (04)000076       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::context /* 04000076 */
        IL_0030:  /* 02   |                  */ ldarg.0
        IL_0031:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_0036:  /* 7C   | (04)000076       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::context /* 04000076 */
        IL_003b:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_0040:  /* 02   |                  */ ldarg.0
        IL_0041:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_0046:  /* 7C   | (04)000076       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::context /* 04000076 */
        IL_004b:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_0050:  /* 6F   | (0A)0000BB       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSyntaxRootAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000BB */
        IL_0055:  /* 16   |                  */ ldc.i4.0
        IL_0056:  /* 6F   | (0A)0000BC       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001B*/::ConfigureAwait(bool) /* 0A0000BC */
        IL_005b:  /* 0D   |                  */ stloc.3
        IL_005c:  /* 12   | 03               */ ldloca.s   V_3
        IL_005e:  /* 28   | (0A)0000BD       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001C*/::GetAwaiter() /* 0A0000BD */
        IL_0063:  /* 0C   |                  */ stloc.2
        IL_0064:  /* 12   | 02               */ ldloca.s   V_2
        IL_0066:  /* 28   | (0A)0000BE       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::get_IsCompleted() /* 0A0000BE */
        IL_006b:  /* 2D   | 3F               */ brtrue.s   IL_00ac

        IL_006d:  /* 02   |                  */ ldarg.0
        IL_006e:  /* 16   |                  */ ldc.i4.0
        IL_006f:  /* 25   |                  */ dup
        IL_0070:  /* 0A   |                  */ stloc.0
        IL_0071:  /* 7D   | (04)00007A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>1__state' /* 0400007A */
        IL_0076:  /* 02   |                  */ ldarg.0
        IL_0077:  /* 08   |                  */ ldloc.2
        IL_0078:  /* 7D   | (04)00007E       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>u__1' /* 0400007E */
        IL_007d:  /* 02   |                  */ ldarg.0
        IL_007e:  /* 7C   | (04)00007B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>t__builder' /* 0400007B */
        IL_0083:  /* 12   | 02               */ ldloca.s   V_2
        IL_0085:  /* 02   |                  */ ldarg.0
        IL_0086:  /* 28   | (2B)000029       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>,valuetype Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    !!1&) /* 2B000029 */
        IL_008b:  /* DD   | B1010000         */ leave      IL_0241

        IL_0090:  /* 02   |                  */ ldarg.0
        IL_0091:  /* 7B   | (04)00007E       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>u__1' /* 0400007E */
        IL_0096:  /* 0C   |                  */ stloc.2
        IL_0097:  /* 02   |                  */ ldarg.0
        IL_0098:  /* 7C   | (04)00007E       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>u__1' /* 0400007E */
        IL_009d:  /* FE15 | (1B)00001D       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/
        IL_00a3:  /* 02   |                  */ ldarg.0
        IL_00a4:  /* 15   |                  */ ldc.i4.m1
        IL_00a5:  /* 25   |                  */ dup
        IL_00a6:  /* 0A   |                  */ stloc.0
        IL_00a7:  /* 7D   | (04)00007A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>1__state' /* 0400007A */
        IL_00ac:  /* 12   | 02               */ ldloca.s   V_2
        IL_00ae:  /* 28   | (0A)0000C0       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B00001D*/::GetResult() /* 0A0000C0 */
        IL_00b3:  /* 0B   |                  */ stloc.1
        IL_00b4:  /* 02   |                  */ ldarg.0
        IL_00b5:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_00ba:  /* 07   |                  */ ldloc.1
        IL_00bb:  /* 7D   | (04)000075       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::root /* 04000075 */
        IL_00c0:  /* 02   |                  */ ldarg.0
        IL_00c1:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_00c6:  /* 7B   | (04)000075       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::root /* 04000075 */
        IL_00cb:  /* 2D   | 05               */ brtrue.s   IL_00d2

        IL_00cd:  /* DD   | 55010000         */ leave      IL_0227

        IL_00d2:  /* 02   |                  */ ldarg.0
        IL_00d3:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_00d8:  /* 7C   | (04)000076       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::context /* 04000076 */
        IL_00dd:  /* 28   | (0A)0000B9       */ call       instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Document() /* 0A0000B9 */
        IL_00e2:  /* 02   |                  */ ldarg.0
        IL_00e3:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_00e8:  /* 7C   | (04)000076       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::context /* 04000076 */
        IL_00ed:  /* 28   | (0A)0000BA       */ call       instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_CancellationToken() /* 0A0000BA */
        IL_00f2:  /* 6F   | (0A)0000C6       */ callvirt   instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::GetSemanticModelAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0A0000C6 */
        IL_00f7:  /* 16   |                  */ ldc.i4.0
        IL_00f8:  /* 6F   | (0A)0000C7       */ callvirt   instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<!0> class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001E*/::ConfigureAwait(bool) /* 0A0000C7 */
        IL_00fd:  /* 13   | 05               */ stloc.s    V_5
        IL_00ff:  /* 12   | 05               */ ldloca.s   V_5
        IL_0101:  /* 28   | (0A)0000C8       */ call       instance valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<!0> valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B00001F*/::GetAwaiter() /* 0A0000C8 */
        IL_0106:  /* 13   | 04               */ stloc.s    V_4
        IL_0108:  /* 12   | 04               */ ldloca.s   V_4
        IL_010a:  /* 28   | (0A)0000C9       */ call       instance bool valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::get_IsCompleted() /* 0A0000C9 */
        IL_010f:  /* 2D   | 41               */ brtrue.s   IL_0152

        IL_0111:  /* 02   |                  */ ldarg.0
        IL_0112:  /* 17   |                  */ ldc.i4.1
        IL_0113:  /* 25   |                  */ dup
        IL_0114:  /* 0A   |                  */ stloc.0
        IL_0115:  /* 7D   | (04)00007A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>1__state' /* 0400007A */
        IL_011a:  /* 02   |                  */ ldarg.0
        IL_011b:  /* 11   | 04               */ ldloc.s    V_4
        IL_011d:  /* 7D   | (04)00007F       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>u__2' /* 0400007F */
        IL_0122:  /* 02   |                  */ ldarg.0
        IL_0123:  /* 7C   | (04)00007B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>t__builder' /* 0400007B */
        IL_0128:  /* 12   | 04               */ ldloca.s   V_4
        IL_012a:  /* 02   |                  */ ldarg.0
        IL_012b:  /* 28   | (2B)00002A       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::AwaitUnsafeOnCompleted<valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>,valuetype Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/>(!!0&,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       !!1&) /* 2B00002A */
        IL_0130:  /* DD   | 0C010000         */ leave      IL_0241

        IL_0135:  /* 02   |                  */ ldarg.0
        IL_0136:  /* 7B   | (04)00007F       */ ldfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>u__2' /* 0400007F */
        IL_013b:  /* 13   | 04               */ stloc.s    V_4
        IL_013d:  /* 02   |                  */ ldarg.0
        IL_013e:  /* 7C   | (04)00007F       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>u__2' /* 0400007F */
        IL_0143:  /* FE15 | (1B)000020       */ initobj    valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/
        IL_0149:  /* 02   |                  */ ldarg.0
        IL_014a:  /* 15   |                  */ ldc.i4.m1
        IL_014b:  /* 25   |                  */ dup
        IL_014c:  /* 0A   |                  */ stloc.0
        IL_014d:  /* 7D   | (04)00007A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>1__state' /* 0400007A */
        IL_0152:  /* 12   | 04               */ ldloca.s   V_4
        IL_0154:  /* 28   | (0A)0000CA       */ call       instance !0 valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*0100003D*//ConfiguredTaskAwaiter/*0100003E*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SemanticModel/*01000049*/>/*1B000020*/::GetResult() /* 0A0000CA */
        IL_0159:  /* 2D   | 05               */ brtrue.s   IL_0160

        IL_015b:  /* DD   | C7000000         */ leave      IL_0227

        IL_0160:  /* 02   |                  */ ldarg.0
        IL_0161:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_0166:  /* 7C   | (04)000076       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::context /* 04000076 */
        IL_016b:  /* 28   | (0A)000068       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/> [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::get_Diagnostics() /* 0A000068 */
        IL_0170:  /* 13   | 07               */ stloc.s    V_7
        IL_0172:  /* 12   | 07               */ ldloca.s   V_7
        IL_0174:  /* 28   | (0A)000069       */ call       instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<!0> valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000010*/::GetEnumerator() /* 0A000069 */
        IL_0179:  /* 13   | 06               */ stloc.s    V_6
        IL_017b:  /* 2B   | 7C               */ br.s       IL_01f9

        IL_017d:  /* 73   | (06)00008C       */ newobj     instance void Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::.ctor() /* 0600008C */
        IL_0182:  /* 13   | 08               */ stloc.s    V_8
        IL_0184:  /* 11   | 08               */ ldloc.s    V_8
        IL_0186:  /* 02   |                  */ ldarg.0
        IL_0187:  /* 7B   | (04)00007D       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_018c:  /* 7D   | (04)000079       */ stfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::'CS$<>8__locals1' /* 04000079 */
        IL_0191:  /* 11   | 08               */ ldloc.s    V_8
        IL_0193:  /* 12   | 06               */ ldloca.s   V_6
        IL_0195:  /* 28   | (0A)00006A       */ call       instance !0 valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::get_Current() /* 0A00006A */
        IL_019a:  /* 7D   | (04)000077       */ stfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::diagnostic /* 04000077 */
        IL_019f:  /* 11   | 08               */ ldloc.s    V_8
        IL_01a1:  /* 7B   | (04)000077       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::diagnostic /* 04000077 */
        IL_01a6:  /* 11   | 08               */ ldloc.s    V_8
        IL_01a8:  /* 7B   | (04)000079       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::'CS$<>8__locals1' /* 04000079 */
        IL_01ad:  /* 7B   | (04)000075       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::root /* 04000075 */
        IL_01b2:  /* 11   | 08               */ ldloc.s    V_8
        IL_01b4:  /* 7C   | (04)000078       */ ldflda     class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::invocation /* 04000078 */
        IL_01b9:  /* 28   | (06)00005F       */ call       bool Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*/::CanReplaceWithAddAuthorizationBuilder(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/,
                                                                                                                                                                                                    class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                    class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/&) /* 0600005F */
        IL_01be:  /* 2C   | 39               */ brfalse.s  IL_01f9

        IL_01c0:  /* 11   | 08               */ ldloc.s    V_8
        IL_01c2:  /* 7B   | (04)000079       */ ldfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::'CS$<>8__locals1' /* 04000079 */
        IL_01c7:  /* 7C   | (04)000076       */ ldflda     valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/::context /* 04000076 */
        IL_01cc:  /* 72   | (70)00091A       */ ldstr      "Use 'AddAuthorizationBuilder'" /* 7000091A */
        IL_01d1:  /* 11   | 08               */ ldloc.s    V_8
        IL_01d3:  /* FE06 | (06)00008D       */ ldftn      instance class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::'<RegisterCodeFixesAsync>b__0'(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/) /* 0600008D */
        IL_01d9:  /* 73   | (0A)00006E       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>/*1B000013*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                                                                   native int) /* 0A00006E */
        IL_01de:  /* 7E   | (0A)0000B7       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::UseAddAuthorizationBuilder /* 0A0000B7 */
        IL_01e3:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
        IL_01e8:  /* 28   | (0A)00006F       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/::Create(string,
                                                                                                                                                                                                                                                                                           class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*0100003B*/,class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>>,
                                                                                                                                                                                                                                                                                           string) /* 0A00006F */
        IL_01ed:  /* 11   | 08               */ ldloc.s    V_8
        IL_01ef:  /* 7B   | (04)000077       */ ldfld      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_1'/*0200002B*/::diagnostic /* 04000077 */
        IL_01f4:  /* 28   | (0A)000070       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/::RegisterCodeFix(class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeActions.CodeAction/*0100005F*/,
                                                                                                                                                                                                     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/) /* 0A000070 */
        IL_01f9:  /* 12   | 06               */ ldloca.s   V_6
        IL_01fb:  /* 28   | (0A)000071       */ call       instance bool valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*//Enumerator/*0100003F*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/>/*1B000011*/::MoveNext() /* 0A000071 */
        IL_0200:  /* 3A   | 78FFFFFF         */ brtrue     IL_017d

        IL_0205:  /* DE   | 20               */ leave.s    IL_0227

      }  // end .try
      catch [netstandard/*23000001*/]System.Exception/*01000040*/ 
      {
        IL_0207:  /* 13   | 09               */ stloc.s    V_9
        IL_0209:  /* 02   |                  */ ldarg.0
        IL_020a:  /* 1F   | FE               */ ldc.i4.s   -2
        IL_020c:  /* 7D   | (04)00007A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>1__state' /* 0400007A */
        IL_0211:  /* 02   |                  */ ldarg.0
        IL_0212:  /* 14   |                  */ ldnull
        IL_0213:  /* 7D   | (04)00007D       */ stfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
        IL_0218:  /* 02   |                  */ ldarg.0
        IL_0219:  /* 7C   | (04)00007B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>t__builder' /* 0400007B */
        IL_021e:  /* 11   | 09               */ ldloc.s    V_9
        IL_0220:  /* 28   | (0A)0000C3       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetException(class [netstandard/*23000001*/]System.Exception/*01000040*/) /* 0A0000C3 */
        IL_0225:  /* DE   | 1A               */ leave.s    IL_0241

      }  // end handler
      // HEX: 00 00 00 00 07 00 00 00 00 02 00 00 07 02 00 00 20 00 00 00 40 00 00 01
      IL_0227:  /* 02   |                  */ ldarg.0
      IL_0228:  /* 1F   | FE               */ ldc.i4.s   -2
      IL_022a:  /* 7D   | (04)00007A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>1__state' /* 0400007A */
      IL_022f:  /* 02   |                  */ ldarg.0
      IL_0230:  /* 14   |                  */ ldnull
      IL_0231:  /* 7D   | (04)00007D       */ stfld      class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c__DisplayClass4_0'/*0200002A*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>8__1' /* 0400007D */
      IL_0236:  /* 02   |                  */ ldarg.0
      IL_0237:  /* 7C   | (04)00007B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>t__builder' /* 0400007B */
      IL_023c:  /* 28   | (0A)0000C4       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetResult() /* 0A0000C4 */
      IL_0241:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__4'::MoveNext

    .method /*0600008F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C0000DD:0A00001A*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000042*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000019*/ 
      // Method begins at RVA 0x57d8
      // Code size       13 (0xd)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7C   | (04)00007B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>t__builder' /* 0400007B */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 28   | (0A)0000C5       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::SetStateMachine(class [netstandard/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/) /* 0A0000C5 */
      IL_000c:  /* 2A   |                  */ ret
    } // end of method '<RegisterCodeFixesAsync>d__4'::SetStateMachine

  } // end of class '<RegisterCodeFixesAsync>d__4'

  .field /*0400001E*/ private initonly valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> '<FixableDiagnosticIds>k__BackingField'
  .custom /*0C000087:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*0600005C*/ public hidebysig specialname virtual 
          instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> 
          get_FixableDiagnosticIds() cil managed
  // SIG: 20 00 15 11 80 91 01 0E
  {
    .custom /*0C0000CC:0A000010*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .param [0]/*0800008C*/ 
    .custom /*0C0000DC:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x3c91
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00001E       */ ldfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*/::'<FixableDiagnosticIds>k__BackingField' /* 0400001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AddAuthorizationBuilderFixer::get_FixableDiagnosticIds

  .method /*0600005D*/ public hidebysig virtual final 
          instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ 
          GetFixAllProvider() cil managed
  // SIG: 20 00 12 80 95
  {
    // Method begins at RVA 0x3c99
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 28   | (0A)000033       */ call       class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.FixAllProvider/*01000025*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders/*0100006F*/::get_BatchFixer() /* 0A000033 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method AddAuthorizationBuilderFixer::GetFixAllProvider

  .method /*0600005E*/ public hidebysig virtual 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          RegisterCodeFixesAsync(valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ context) cil managed
  // SIG: 20 01 12 80 A1 11 80 A5
  {
    .custom /*0C0000CD:0A000016*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000027*/::.ctor(class [netstandard/*23000001*/]System.Type/*01000026*/) /* 0A000016 */ = ( 01 00 6D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..mMicrosoft.Asp
                                                                                                                                                                                                                                  4E 65 74 43 6F 72 65 2E 41 6E 61 6C 79 7A 65 72   // NetCore.Analyzer
                                                                                                                                                                                                                                  73 2E 41 75 74 68 6F 72 69 7A 61 74 69 6F 6E 2E   // s.Authorization.
                                                                                                                                                                                                                                  46 69 78 65 72 73 2E 41 64 64 41 75 74 68 6F 72   // Fixers.AddAuthor
                                                                                                                                                                                                                                  69 7A 61 74 69 6F 6E 42 75 69 6C 64 65 72 46 69   // izationBuilderFi
                                                                                                                                                                                                                                  78 65 72 2B 3C 52 65 67 69 73 74 65 72 43 6F 64   // xer+<RegisterCod
                                                                                                                                                                                                                                  65 46 69 78 65 73 41 73 79 6E 63 3E 64 5F 5F 34   // eFixesAsync>d__4
                                                                                                                                                                                                                                  00 00 ) 
    // Method begins at RVA 0x3ca0
    // Code size       55 (0x37)
    .maxstack  2
    .locals /*11000023*/ init (valuetype Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/ V_0)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* 28   | (0A)000034       */ call       valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Create() /* 0A000034 */
    IL_0007:  /* 7D   | (04)00007B       */ stfld      valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>t__builder' /* 0400007B */
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 7D   | (04)00007C       */ stfld      valuetype [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixContext/*01000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::context /* 0400007C */
    IL_0014:  /* 12   | 00               */ ldloca.s   V_0
    IL_0016:  /* 15   |                  */ ldc.i4.m1
    IL_0017:  /* 7D   | (04)00007A       */ stfld      int32 Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>1__state' /* 0400007A */
    IL_001c:  /* 12   | 00               */ ldloca.s   V_0
    IL_001e:  /* 7C   | (04)00007B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>t__builder' /* 0400007B */
    IL_0023:  /* 12   | 00               */ ldloca.s   V_0
    IL_0025:  /* 28   | (2B)000016       */ call       instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::Start<valuetype Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/>(!!0&) /* 2B000016 */
    IL_002a:  /* 12   | 00               */ ldloca.s   V_0
    IL_002c:  /* 7C   | (04)00007B       */ ldflda     valuetype [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<RegisterCodeFixesAsync>d__4'/*0200002C*/::'<>t__builder' /* 0400007B */
    IL_0031:  /* 28   | (0A)000036       */ call       instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/ [netstandard/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000041*/::get_Task() /* 0A000036 */
    IL_0036:  /* 2A   |                  */ ret
  } // end of method AddAuthorizationBuilderFixer::RegisterCodeFixesAsync

  .method /*0600005F*/ private hidebysig static 
          bool  CanReplaceWithAddAuthorizationBuilder(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                                      class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                                                      [out] class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/& invocation) cil managed
  // SIG: 00 03 02 12 80 B1 12 80 B5 10 12 80 B9
  {
    .param [3]/*08000090*/ 
    .custom /*0C0000DE:0A000017*/ instance void [Microsoft.AspNetCore.App.Analyzers/*23000002*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x3ce4
    // Code size       610 (0x262)
    .maxstack  4
    .locals /*11000024*/ init (valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/> V_0,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_1,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ V_2,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleLambdaExpressionSyntax/*01000062*/ V_3,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ V_4,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ V_5,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ V_6,
             class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_7,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax/*01000064*/ V_8,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_9,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ V_10,
             class [netstandard/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> V_11,
             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_12,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/> V_13,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ V_14,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ V_15,
             string V_16,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ V_17,
             bool V_18,
             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/ V_19,
             bool V_20)
    IL_0000:  /* 04   |                  */ ldarg.2
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 51   |                  */ stind.ref
    IL_0003:  /* 03   |                  */ ldarg.1
    IL_0004:  /* 02   |                  */ ldarg.0
    IL_0005:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_000a:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_000f:  /* 16   |                  */ ldc.i4.0
    IL_0010:  /* 17   |                  */ ldc.i4.1
    IL_0011:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_0016:  /* 75   | (01)00002E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
    IL_001b:  /* 13   | 04               */ stloc.s    V_4
    IL_001d:  /* 11   | 04               */ ldloc.s    V_4
    IL_001f:  /* 39   | 3C020000         */ brfalse    IL_0260

    IL_0024:  /* 11   | 04               */ ldloc.s    V_4
    IL_0026:  /* 6F   | (0A)000044       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_ArgumentList() /* 0A000044 */
    IL_002b:  /* 13   | 05               */ stloc.s    V_5
    IL_002d:  /* 11   | 05               */ ldloc.s    V_5
    IL_002f:  /* 39   | 2C020000         */ brfalse    IL_0260

    IL_0034:  /* 11   | 05               */ ldloc.s    V_5
    IL_0036:  /* 6F   | (0A)000045       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax/*01000073*/::get_Arguments() /* 0A000045 */
    IL_003b:  /* 0A   |                  */ stloc.0
    IL_003c:  /* 12   | 00               */ ldloca.s   V_0
    IL_003e:  /* 28   | (0A)000060       */ call       instance int32 valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>/*1B00000D*/::get_Count() /* 0A000060 */
    IL_0043:  /* 17   |                  */ ldc.i4.1
    IL_0044:  /* 40   | 17020000         */ bne.un     IL_0260

    IL_0049:  /* 11   | 04               */ ldloc.s    V_4
    IL_004b:  /* 6F   | (0A)00003D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_Expression() /* 0A00003D */
    IL_0050:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
    IL_0055:  /* 0B   |                  */ stloc.1
    IL_0056:  /* 07   |                  */ ldloc.1
    IL_0057:  /* 39   | 04020000         */ brfalse    IL_0260

    IL_005c:  /* 07   |                  */ ldloc.1
    IL_005d:  /* 6F   | (0A)00004D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Name() /* 0A00004D */
    IL_0062:  /* 13   | 06               */ stloc.s    V_6
    IL_0064:  /* 11   | 06               */ ldloc.s    V_6
    IL_0066:  /* 39   | F5010000         */ brfalse    IL_0260

    IL_006b:  /* 11   | 06               */ ldloc.s    V_6
    IL_006d:  /* 6F   | (0A)00005D       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/::get_Identifier() /* 0A00005D */
    IL_0072:  /* 0C   |                  */ stloc.2
    IL_0073:  /* 12   | 00               */ ldloca.s   V_0
    IL_0075:  /* 16   |                  */ ldc.i4.0
    IL_0076:  /* 28   | (0A)000061       */ call       instance !0 valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>/*1B00000D*/::get_Item(int32) /* 0A000061 */
    IL_007b:  /* 6F   | (0A)000047       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/::get_Expression() /* 0A000047 */
    IL_0080:  /* 75   | (01)000062       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleLambdaExpressionSyntax/*01000062*/
    IL_0085:  /* 0D   |                  */ stloc.3
    IL_0086:  /* 09   |                  */ ldloc.3
    IL_0087:  /* 39   | D4010000         */ brfalse    IL_0260

    IL_008c:  /* 09   |                  */ ldloc.3
    IL_008d:  /* 6F   | (0A)0000A0       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode/*01000072*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax/*01000075*/::get_Body() /* 0A0000A0 */
    IL_0092:  /* 75   | (01)000064       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax/*01000064*/
    IL_0097:  /* 13   | 08               */ stloc.s    V_8
    IL_0099:  /* 11   | 08               */ ldloc.s    V_8
    IL_009b:  /* 2C   | 0D               */ brfalse.s  IL_00aa

    IL_009d:  /* 11   | 08               */ ldloc.s    V_8
    IL_009f:  /* 14   |                  */ ldnull
    IL_00a0:  /* 16   |                  */ ldc.i4.0
    IL_00a1:  /* 6F   | (0A)0000A1       */ callvirt   instance class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/> [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::DescendantNodes(class [netstandard/*23000001*/]System.Func`2/*01000066*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,bool>,
                                                                                                                                                                                                                                                                                                                                                bool) /* 0A0000A1 */
    IL_00a6:  /* 13   | 07               */ stloc.s    V_7
    IL_00a8:  /* 2B   | 22               */ br.s       IL_00cc

    IL_00aa:  /* 09   |                  */ ldloc.3
    IL_00ab:  /* 6F   | (0A)0000A0       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode/*01000072*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax/*01000075*/::get_Body() /* 0A0000A0 */
    IL_00b0:  /* 75   | (01)00002E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
    IL_00b5:  /* 13   | 0A               */ stloc.s    V_10
    IL_00b7:  /* 11   | 0A               */ ldloc.s    V_10
    IL_00b9:  /* 2C   | 0F               */ brfalse.s  IL_00ca

    IL_00bb:  /* 17   |                  */ ldc.i4.1
    IL_00bc:  /* 8D   | (01)00002E       */ newarr     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
    IL_00c1:  /* 25   |                  */ dup
    IL_00c2:  /* 16   |                  */ ldc.i4.0
    IL_00c3:  /* 11   | 0A               */ ldloc.s    V_10
    IL_00c5:  /* A2   |                  */ stelem.ref
    IL_00c6:  /* 13   | 07               */ stloc.s    V_7
    IL_00c8:  /* 2B   | 02               */ br.s       IL_00cc

    IL_00ca:  /* 16   |                  */ ldc.i4.0
    IL_00cb:  /* 2A   |                  */ ret

    IL_00cc:  /* 07   |                  */ ldloc.1
    IL_00cd:  /* 08   |                  */ ldloc.2
    IL_00ce:  /* 72   | (70)000483       */ ldstr      "AddAuthorizationBuilder" /* 70000483 */
    IL_00d3:  /* 28   | (0A)00005E       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::Identifier(string) /* 0A00005E */
    IL_00d8:  /* 28   | (2B)000017       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceToken<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/>(!!0,
                                                                                                                                                                                                                                                                                                     valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/,
                                                                                                                                                                                                                                                                                                     valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/) /* 2B000017 */
    IL_00dd:  /* 13   | 09               */ stloc.s    V_9
    IL_00df:  /* 04   |                  */ ldarg.2
    IL_00e0:  /* 11   | 09               */ ldloc.s    V_9
    IL_00e2:  /* 28   | (0A)0000A2       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::InvocationExpression(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/) /* 0A0000A2 */
    IL_00e7:  /* 51   |                  */ stind.ref
    IL_00e8:  /* 11   | 07               */ ldloc.s    V_7
    IL_00ea:  /* 6F   | (0A)0000A3       */ callvirt   instance class [netstandard/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<!0> class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B000018*/::GetEnumerator() /* 0A0000A3 */
    IL_00ef:  /* 13   | 0B               */ stloc.s    V_11
    .try
    {
      IL_00f1:  /* 38   | 4E010000         */ br         IL_0244

      IL_00f6:  /* 11   | 0B               */ ldloc.s    V_11
      IL_00f8:  /* 6F   | (0A)0000A4       */ callvirt   instance !0 class [netstandard/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>/*1B000019*/::get_Current() /* 0A0000A4 */
      IL_00fd:  /* 13   | 0C               */ stloc.s    V_12
      IL_00ff:  /* 11   | 0C               */ ldloc.s    V_12
      IL_0101:  /* 75   | (01)00002E       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/
      IL_0106:  /* 13   | 04               */ stloc.s    V_4
      IL_0108:  /* 11   | 04               */ ldloc.s    V_4
      IL_010a:  /* 2C   | 7F               */ brfalse.s  IL_018b

      IL_010c:  /* 11   | 04               */ ldloc.s    V_4
      IL_010e:  /* 6F   | (0A)000044       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_ArgumentList() /* 0A000044 */
      IL_0113:  /* 13   | 05               */ stloc.s    V_5
      IL_0115:  /* 11   | 05               */ ldloc.s    V_5
      IL_0117:  /* 2C   | 72               */ brfalse.s  IL_018b

      IL_0119:  /* 11   | 05               */ ldloc.s    V_5
      IL_011b:  /* 6F   | (0A)000045       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax/*01000073*/::get_Arguments() /* 0A000045 */
      IL_0120:  /* 13   | 0D               */ stloc.s    V_13
      IL_0122:  /* 12   | 0D               */ ldloca.s   V_13
      IL_0124:  /* 28   | (0A)000060       */ call       instance int32 valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>/*1B00000D*/::get_Count() /* 0A000060 */
      IL_0129:  /* 18   |                  */ ldc.i4.2
      IL_012a:  /* 33   | 5F               */ bne.un.s   IL_018b

      IL_012c:  /* 11   | 04               */ ldloc.s    V_4
      IL_012e:  /* 6F   | (0A)00003D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/::get_Expression() /* 0A00003D */
      IL_0133:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
      IL_0138:  /* 13   | 0E               */ stloc.s    V_14
      IL_013a:  /* 11   | 0E               */ ldloc.s    V_14
      IL_013c:  /* 2C   | 4D               */ brfalse.s  IL_018b

      IL_013e:  /* 11   | 0E               */ ldloc.s    V_14
      IL_0140:  /* 6F   | (0A)00004D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Name() /* 0A00004D */
      IL_0145:  /* 13   | 06               */ stloc.s    V_6
      IL_0147:  /* 11   | 06               */ ldloc.s    V_6
      IL_0149:  /* 2C   | 40               */ brfalse.s  IL_018b

      IL_014b:  /* 11   | 06               */ ldloc.s    V_6
      IL_014d:  /* 6F   | (0A)00005D       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/::get_Identifier() /* 0A00005D */
      IL_0152:  /* 13   | 0F               */ stloc.s    V_15
      IL_0154:  /* 12   | 0F               */ ldloca.s   V_15
      IL_0156:  /* 28   | (0A)0000A5       */ call       instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/::get_Text() /* 0A0000A5 */
      IL_015b:  /* 72   | (70)0004B3       */ ldstr      "AddPolicy" /* 700004B3 */
      IL_0160:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                              string) /* 0A000031 */
      IL_0165:  /* 2C   | 24               */ brfalse.s  IL_018b

      IL_0167:  /* 04   |                  */ ldarg.2
      IL_0168:  /* 04   |                  */ ldarg.2
      IL_0169:  /* 50   |                  */ ldind.ref
      IL_016a:  /* 72   | (70)0004B3       */ ldstr      "AddPolicy" /* 700004B3 */
      IL_016f:  /* 11   | 0D               */ ldloc.s    V_13
      IL_0171:  /* 8C   | (1B)00000D       */ box        valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>/*1B00000D*/
      IL_0176:  /* 28   | (2B)00000C       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<!!0> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::SeparatedList<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<!!0>) /* 2B00000C */
      IL_017b:  /* 28   | (0A)0000A6       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::ArgumentList(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>) /* 0A0000A6 */
      IL_0180:  /* 28   | (06)000060       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*/::ChainInvocation(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/,
                                                                                                                                                                                                                                                                                                    string,
                                                                                                                                                                                                                                                                                                    class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/) /* 06000060 */
      IL_0185:  /* 51   |                  */ stind.ref
      IL_0186:  /* 38   | B9000000         */ br         IL_0244

      IL_018b:  /* 11   | 0C               */ ldloc.s    V_12
      IL_018d:  /* 75   | (01)000047       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/
      IL_0192:  /* 13   | 13               */ stloc.s    V_19
      IL_0194:  /* 11   | 13               */ ldloc.s    V_19
      IL_0196:  /* 2C   | 45               */ brfalse.s  IL_01dd

      IL_0198:  /* 11   | 13               */ ldloc.s    V_19
      IL_019a:  /* 6F   | (0A)0000A7       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/::get_Left() /* 0A0000A7 */
      IL_019f:  /* 75   | (01)000031       */ isinst     [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/
      IL_01a4:  /* 13   | 0E               */ stloc.s    V_14
      IL_01a6:  /* 11   | 0E               */ ldloc.s    V_14
      IL_01a8:  /* 2C   | 33               */ brfalse.s  IL_01dd

      IL_01aa:  /* 11   | 0E               */ ldloc.s    V_14
      IL_01ac:  /* 6F   | (0A)00004D       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/::get_Name() /* 0A00004D */
      IL_01b1:  /* 13   | 06               */ stloc.s    V_6
      IL_01b3:  /* 11   | 06               */ ldloc.s    V_6
      IL_01b5:  /* 2C   | 26               */ brfalse.s  IL_01dd

      IL_01b7:  /* 11   | 06               */ ldloc.s    V_6
      IL_01b9:  /* 6F   | (0A)00005D       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/::get_Identifier() /* 0A00005D */
      IL_01be:  /* 13   | 0F               */ stloc.s    V_15
      IL_01c0:  /* 12   | 0F               */ ldloca.s   V_15
      IL_01c2:  /* 28   | (0A)0000A5       */ call       instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxToken/*01000046*/::get_Text() /* 0A0000A5 */
      IL_01c7:  /* 13   | 10               */ stloc.s    V_16
      IL_01c9:  /* 11   | 10               */ ldloc.s    V_16
      IL_01cb:  /* 2C   | 10               */ brfalse.s  IL_01dd

      IL_01cd:  /* 11   | 13               */ ldloc.s    V_19
      IL_01cf:  /* 6F   | (0A)0000A8       */ callvirt   instance class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax/*01000047*/::get_Right() /* 0A0000A8 */
      IL_01d4:  /* 13   | 11               */ stloc.s    V_17
      IL_01d6:  /* 11   | 11               */ ldloc.s    V_17
      IL_01d8:  /* 14   |                  */ ldnull
      IL_01d9:  /* FE03 |                  */ cgt.un
      IL_01db:  /* 2B   | 01               */ br.s       IL_01de

      IL_01dd:  /* 16   |                  */ ldc.i4.0
      IL_01de:  /* 13   | 12               */ stloc.s    V_18
      IL_01e0:  /* 11   | 12               */ ldloc.s    V_18
      IL_01e2:  /* 2C   | 36               */ brfalse.s  IL_021a

      IL_01e4:  /* 11   | 10               */ ldloc.s    V_16
      IL_01e6:  /* 72   | (70)0004C7       */ ldstr      "DefaultPolicy" /* 700004C7 */
      IL_01eb:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                              string) /* 0A000031 */
      IL_01f0:  /* 2D   | 1C               */ brtrue.s   IL_020e

      IL_01f2:  /* 11   | 10               */ ldloc.s    V_16
      IL_01f4:  /* 72   | (70)0004E3       */ ldstr      "FallbackPolicy" /* 700004E3 */
      IL_01f9:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                              string) /* 0A000031 */
      IL_01fe:  /* 2D   | 0E               */ brtrue.s   IL_020e

      IL_0200:  /* 11   | 10               */ ldloc.s    V_16
      IL_0202:  /* 72   | (70)000501       */ ldstr      "InvokeHandlersAfterFailure" /* 70000501 */
      IL_0207:  /* 28   | (0A)000031       */ call       bool [netstandard/*23000001*/]System.String/*0100006E*/::op_Equality(string,
                                                                                                                              string) /* 0A000031 */
      IL_020c:  /* 2C   | 05               */ brfalse.s  IL_0213

      IL_020e:  /* 17   |                  */ ldc.i4.1
      IL_020f:  /* 13   | 14               */ stloc.s    V_20
      IL_0211:  /* 2B   | 03               */ br.s       IL_0216

      IL_0213:  /* 16   |                  */ ldc.i4.0
      IL_0214:  /* 13   | 14               */ stloc.s    V_20
      IL_0216:  /* 11   | 14               */ ldloc.s    V_20
      IL_0218:  /* 13   | 12               */ stloc.s    V_18
      IL_021a:  /* 11   | 12               */ ldloc.s    V_18
      IL_021c:  /* 2C   | 26               */ brfalse.s  IL_0244

      IL_021e:  /* 04   |                  */ ldarg.2
      IL_021f:  /* 04   |                  */ ldarg.2
      IL_0220:  /* 50   |                  */ ldind.ref
      IL_0221:  /* 72   | (70)000537       */ ldstr      "Set" /* 70000537 */
      IL_0226:  /* 11   | 10               */ ldloc.s    V_16
      IL_0228:  /* 28   | (0A)00008D       */ call       string [netstandard/*23000001*/]System.String/*0100006E*/::Concat(string,
                                                                                                                           string) /* 0A00008D */
      IL_022d:  /* 11   | 11               */ ldloc.s    V_17
      IL_022f:  /* 28   | (0A)0000A9       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::Argument(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/) /* 0A0000A9 */
      IL_0234:  /* 28   | (2B)000018       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<!!0> [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::SingletonSeparatedList<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>(!!0) /* 2B000018 */
      IL_0239:  /* 28   | (0A)0000A6       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::ArgumentList(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SeparatedSyntaxList`1/*01000048*/<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax/*01000033*/>) /* 0A0000A6 */
      IL_023e:  /* 28   | (06)000060       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*/::ChainInvocation(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/,
                                                                                                                                                                                                                                                                                                    string,
                                                                                                                                                                                                                                                                                                    class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/) /* 06000060 */
      IL_0243:  /* 51   |                  */ stind.ref
      IL_0244:  /* 11   | 0B               */ ldloc.s    V_11
      IL_0246:  /* 6F   | (0A)0000AB       */ callvirt   instance bool [netstandard/*23000001*/]System.Collections.IEnumerator/*0100008D*/::MoveNext() /* 0A0000AB */
      IL_024b:  /* 3A   | A6FEFFFF         */ brtrue     IL_00f6

      IL_0250:  /* DE   | 0C               */ leave.s    IL_025e

    }  // end .try
    finally
    {
      IL_0252:  /* 11   | 0B               */ ldloc.s    V_11
      IL_0254:  /* 2C   | 07               */ brfalse.s  IL_025d

      IL_0256:  /* 11   | 0B               */ ldloc.s    V_11
      IL_0258:  /* 6F   | (0A)000023       */ callvirt   instance void [netstandard/*23000001*/]System.IDisposable/*0100006A*/::Dispose() /* 0A000023 */
      IL_025d:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 F1 00 00 00 61 01 00 00 52 02 00 00 0C 00 00 00 00 00 00 00
    IL_025e:  /* 17   |                  */ ldc.i4.1
    IL_025f:  /* 2A   |                  */ ret

    IL_0260:  /* 16   |                  */ ldc.i4.0
    IL_0261:  /* 2A   |                  */ ret
  } // end of method AddAuthorizationBuilderFixer::CanReplaceWithAddAuthorizationBuilder

  .method /*06000060*/ private hidebysig static 
          class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ 
          ChainInvocation(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ invocation,
                          string invokedMemberName,
                          class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/ argumentList) cil managed
  // SIG: 00 03 12 80 B9 12 80 B9 0E 12 80 E5
  {
    // Method begins at RVA 0x3f70
    // Code size       128 (0x80)
    .maxstack  4
    .locals /*11000025*/ init (class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/> V_0,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/ V_1,
             valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/ V_2)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)000040       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode/*01000072*/::GetLeadingTrivia() /* 0A000040 */
    IL_0006:  /* 8C   | (01)000032       */ box        [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/
    IL_000b:  /* 7E   | (04)000074       */ ldsfld     class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/,bool> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/::'<>9__6_0' /* 04000074 */
    IL_0010:  /* 25   |                  */ dup
    IL_0011:  /* 2D   | 17               */ brtrue.s   IL_002a

    IL_0013:  /* 26   |                  */ pop
    IL_0014:  /* 7E   | (04)000073       */ ldsfld     class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/ Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/::'<>9' /* 04000073 */
    IL_0019:  /* FE06 | (06)00008A       */ ldftn      instance bool Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/::'<ChainInvocation>b__6_0'(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/) /* 0600008A */
    IL_001f:  /* 73   | (0A)0000AC       */ newobj     instance void class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/,bool>/*1B00001A*/::.ctor(object,
                                                                                                                                                                                                                                                     native int) /* 0A0000AC */
    IL_0024:  /* 25   |                  */ dup
    IL_0025:  /* 80   | (04)000074       */ stsfld     class [netstandard/*23000001*/]System.Func`2/*01000066*/<valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/,bool> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*//'<>c'/*02000029*/::'<>9__6_0' /* 04000074 */
    IL_002a:  /* 28   | (2B)000019       */ call       class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<!!0> [netstandard/*23000001*/]System.Linq.Enumerable/*01000074*/::Where<valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/>(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<!!0>,
                                                                                                                                                                                                                                                                                                                  class [netstandard/*23000001*/]System.Func`2/*01000066*/<!!0,bool>) /* 2B000019 */
    IL_002f:  /* 0A   |                  */ stloc.0
    IL_0030:  /* 18   |                  */ ldc.i4.2
    IL_0031:  /* 8D   | (01)000065       */ newarr     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/
    IL_0036:  /* 25   |                  */ dup
    IL_0037:  /* 16   |                  */ ldc.i4.0
    IL_0038:  /* 28   | (0A)0000AE       */ call       string [netstandard/*23000001*/]System.Environment/*0100008E*/::get_NewLine() /* 0A0000AE */
    IL_003d:  /* 28   | (0A)0000AF       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::EndOfLine(string) /* 0A0000AF */
    IL_0042:  /* A4   | (01)000065       */ stelem     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/
    IL_0047:  /* 25   |                  */ dup
    IL_0048:  /* 17   |                  */ ldc.i4.1
    IL_0049:  /* 28   | (0A)0000B0       */ call       valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::get_Tab() /* 0A0000B0 */
    IL_004e:  /* A4   | (01)000065       */ stelem     [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/
    IL_0053:  /* 73   | (0A)0000B1       */ newobj     instance void [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/::.ctor(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/[]) /* 0A0000B1 */
    IL_0058:  /* 0C   |                  */ stloc.2
    IL_0059:  /* 12   | 02               */ ldloca.s   V_2
    IL_005b:  /* 06   |                  */ ldloc.0
    IL_005c:  /* 28   | (0A)0000B2       */ call       instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/::AddRange(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000063*/<valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTrivia/*01000065*/>) /* 0A0000B2 */
    IL_0061:  /* 0B   |                  */ stloc.1
    IL_0062:  /* 20   | F1210000         */ ldc.i4     0x21f1
    IL_0067:  /* 02   |                  */ ldarg.0
    IL_0068:  /* 07   |                  */ ldloc.1
    IL_0069:  /* 28   | (2B)00001A       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::WithTrailingTrivia<class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/>(!!0,
                                                                                                                                                                                                                                                                                                         valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxTriviaList/*01000032*/) /* 2B00001A */
    IL_006e:  /* 03   |                  */ ldarg.1
    IL_006f:  /* 28   | (0A)0000B4       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax/*0100002F*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::IdentifierName(string) /* 0A0000B4 */
    IL_0074:  /* 28   | (0A)0000B5       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax/*01000031*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::MemberAccessExpression(valuetype [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxKind/*0100007F*/,
                                                                                                                                                                                                                                                                                                                 class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/,
                                                                                                                                                                                                                                                                                                                 class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax/*0100003A*/) /* 0A0000B5 */
    IL_0079:  /* 04   |                  */ ldarg.2
    IL_007a:  /* 28   | (0A)0000B6       */ call       class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.SyntaxFactory/*0100007D*/::InvocationExpression(class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax/*0100004B*/,
                                                                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax/*01000039*/) /* 0A0000B6 */
    IL_007f:  /* 2A   |                  */ ret
  } // end of method AddAuthorizationBuilderFixer::ChainInvocation

  .method /*06000061*/ private hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/> 
          ReplaceWithAddAuthorizationBuilder(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/ diagnostic,
                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ root,
                                             class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ document,
                                             class [Microsoft.CodeAnalysis.CSharp/*23000007*/]Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax/*0100002E*/ invocation) cil managed
  // SIG: 00 04 15 12 80 A9 01 12 80 AD 12 80 B1 12 80 B5 12 80 AD 12 80 B9
  {
    // Method begins at RVA 0x3ffc
    // Code size       40 (0x28)
    .maxstack  4
    .locals /*11000010*/ init (class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 6F   | (0A)000037       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Diagnostic/*0100002C*/::get_Location() /* 0A000037 */
    IL_0007:  /* 6F   | (0A)000038       */ callvirt   instance valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Location/*01000070*/::get_SourceSpan() /* 0A000038 */
    IL_000c:  /* 16   |                  */ ldc.i4.0
    IL_000d:  /* 17   |                  */ ldc.i4.1
    IL_000e:  /* 6F   | (0A)000039       */ callvirt   instance class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/ [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/::FindNode(valuetype [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.Text.TextSpan/*01000059*/,
                                                                                                                                                                                                                                                    bool,
                                                                                                                                                                                                                                                    bool) /* 0A000039 */
    IL_0013:  /* 0A   |                  */ stloc.0
    IL_0014:  /* 04   |                  */ ldarg.2
    IL_0015:  /* 03   |                  */ ldarg.1
    IL_0016:  /* 06   |                  */ ldloc.0
    IL_0017:  /* 05   |                  */ ldarg.3
    IL_0018:  /* 28   | (2B)000003       */ call       !!0 [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNodeExtensions/*01000071*/::ReplaceNode<class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/>(!!0,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/,
                                                                                                                                                                                                                                                             class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 2B000003 */
    IL_001d:  /* 6F   | (0A)00003B       */ callvirt   instance class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/ [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/::WithSyntaxRoot(class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.SyntaxNode/*0100002D*/) /* 0A00003B */
    IL_0022:  /* 28   | (2B)000004       */ call       class [netstandard/*23000001*/]System.Threading.Tasks.Task`1/*0100002A*/<!!0> [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000028*/::FromResult<class [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.Document/*0100002B*/>(!!0) /* 2B000004 */
    IL_0027:  /* 2A   |                  */ ret
  } // end of method AddAuthorizationBuilderFixer::ReplaceWithAddAuthorizationBuilder

  .method /*06000062*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4030
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (0A)0000B7       */ ldsfld     class [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/ [Microsoft.AspNetCore.App.Analyzers/*23000002*/]Microsoft.AspNetCore.Analyzers.DiagnosticDescriptors/*01000076*/::UseAddAuthorizationBuilder /* 0A0000B7 */
    IL_0006:  /* 6F   | (0A)000054       */ callvirt   instance string [Microsoft.CodeAnalysis/*23000003*/]Microsoft.CodeAnalysis.DiagnosticDescriptor/*01000077*/::get_Id() /* 0A000054 */
    IL_000b:  /* 28   | (2B)00000D       */ call       valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<!!0> [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray/*01000078*/::Create<string>(!!0) /* 2B00000D */
    IL_0010:  /* 7D   | (04)00001E       */ stfld      valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*/::'<FixableDiagnosticIds>k__BackingField' /* 0400001E */
    IL_0015:  /* 02   |                  */ ldarg.0
    IL_0016:  /* 28   | (0A)000058       */ call       instance void [Microsoft.CodeAnalysis.Workspaces/*23000004*/]Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider/*01000023*/::.ctor() /* 0A000058 */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method AddAuthorizationBuilderFixer::.ctor

  .property /*1700000B*/ instance callconv(8) valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string>
          FixableDiagnosticIds()
  {
    .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000004*/::.ctor(uint8[]) /* 06000004 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [System.Collections.Immutable/*23000006*/]System.Collections.Immutable.ImmutableArray`1/*01000024*/<string> Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer/*02000013*/::get_FixableDiagnosticIds() /* 0600005C */
  } // end of property AddAuthorizationBuilderFixer::FixableDiagnosticIds
} // end of class Microsoft.AspNetCore.Analyzers.Authorization.Fixers.AddAuthorizationBuilderFixer


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
