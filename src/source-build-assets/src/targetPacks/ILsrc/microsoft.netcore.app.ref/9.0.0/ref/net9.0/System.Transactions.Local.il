


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
// Time-date stamp:            0x956b08a7
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00003400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000052a6
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
// Checksum:                       0x00011655
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00005254 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x000004a4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003e00 [0x00002930] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00005238 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000032ac Virtual Size
//              0x00002000 Virtual Address
//              0x00003400 Size of Raw Data
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
//              0x000004a4 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00003600 Pointer to Raw Data
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
//              0x00003c00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000002a8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00005296 Import Name Table
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
// 0x00002290 [0x00002f28] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000051b8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000015e8 Size
//              '#~' Name
//    Stream 2:
//              0x00001654 Offset
//              0x000011a4 Size
//              '#Strings' Name
//    Stream 3:
//              0x000027f8 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000027fc Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000280c Offset
//              0x0000071c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000000090bb41f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 26416
// PE header size       : 512 (496 used)    ( 1.94%)
// PE additional info   : 11931             (45.17%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.27%)
// CLR meta-data size  : 12072              (45.70%)
// CLR additional info : 128                ( 0.48%)
// CLR method headers  : 124                ( 0.47%)
// Managed code         : 452               ( 1.71%)
// Data                 : 2048              ( 7.75%)
// Unaccounted          : -923              (-3.49%)

// Num.of PE sections   : 3
//   .text    - 13312
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 12072
//   Module        -    1 (10 bytes)
//   TypeDef       -   35 (490 bytes)     6 interfaces, 0 explicit layout
//   TypeRef       -   50 (300 bytes)
//   MethodDef     -  149 (2086 bytes)    13 abstract, 0 native, 124 bodies
//   FieldDef      -   33 (198 bytes)     1 constant
//   MemberRef     -   37 (222 bytes)
//   ParamDef      -  158 (948 bytes)
//   MethodImpl    -    5 (30 bytes)
//   Constant      -   23 (138 bytes)
//   CustomAttribute-  108 (648 bytes)
//   InterfaceImpl -    8 (32 bytes)
//   PropertyMap   -    6 (24 bytes)
//   Property      -   19 (114 bytes)
//   MethodSemantic-   30 (180 bytes)
//   TypeSpec      -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   EventMap      -    2 (8 bytes)
//   Event         -    2 (12 bytes)
//   Strings       -  4513 bytes
//   Blobs         -  1820 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   215 bytes

// CLR additional info : 128

// CLR method headers : 124
//   Num.of method bodies  - 124
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 124

// Managed code : 452
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class CommittableTransaction         (public) (auto) (ansi) (sealed) 
// Class DependentCloneOption           (public) (auto) (ansi) (sealed) 
// Class DependentTransaction           (public) (auto) (ansi) (sealed) 
// Class Enlistment                     (public) (auto) (ansi) 
// Class EnlistmentOptions              (public) (auto) (ansi) (sealed) 
// Class EnterpriseServicesInteropOption (public) (auto) (ansi) (sealed) 
// Class HostCurrentTransactionCallback (public) (auto) (ansi) (sealed) 
// Interface IDtcTransaction                (public) (abstract) (auto) (ansi) 
// Interface IEnlistmentNotification        (public) (abstract) (auto) (ansi) 
// Interface IPromotableSinglePhaseNotification (public) (abstract) (auto) (ansi) 
// Interface ISimpleTransactionSuperior     (public) (abstract) (auto) (ansi) 
// Interface ISinglePhaseNotification       (public) (abstract) (auto) (ansi) 
// Class IsolationLevel                 (public) (auto) (ansi) (sealed) 
// Interface ITransactionPromoter           (public) (abstract) (auto) (ansi) 
// Class PreparingEnlistment            (public) (auto) (ansi) 
// Class SinglePhaseEnlistment          (public) (auto) (ansi) 
// Class SubordinateTransaction         (public) (auto) (ansi) (sealed) 
// Class Transaction                    (public) (auto) (ansi) 
// Class TransactionAbortedException    (public) (auto) (ansi) 
// Class TransactionCompletedEventHandler (public) (auto) (ansi) (sealed) 
// Class TransactionEventArgs           (public) (auto) (ansi) 
// Class TransactionException           (public) (auto) (ansi) 
// Class TransactionInDoubtException    (public) (auto) (ansi) 
// Class TransactionInformation         (public) (auto) (ansi) 
// Class TransactionInterop             (public) (abstract) (auto) (ansi) (sealed) 
// Class TransactionManager             (public) (abstract) (auto) (ansi) (sealed) 
// Class TransactionManagerCommunicationException (public) (auto) (ansi) 
// Class TransactionOptions             (public) (sequential) (ansi) (sealed) 
// Class TransactionPromotionException  (public) (auto) (ansi) 
// Class TransactionScope               (public) (auto) (ansi) (sealed) 
// Class TransactionScopeAsyncFlowOption (public) (auto) (ansi) (sealed) 
// Class TransactionScopeOption         (public) (auto) (ansi) (sealed) 
// Class TransactionStartedEventHandler (public) (auto) (ansi) (sealed) 
// Class TransactionStatus              (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000002*/ System.Runtime.InteropServices
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly /*20000001*/ noplatform System.Transactions.Local
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

  .custom /*0C000008:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 19 53 79 73 74 65 6D 2E 54 72 61 6E 73 61   // ...System.Transa
                                                                                                                                                                        63 74 69 6F 6E 73 2E 4C 6F 63 61 6C 00 00 )       // ctions.Local..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 19 53 79 73 74 65 6D 2E 54 72 61 6E 73 61   // ...System.Transa
                                                                                                                                                                       63 74 69 6F 6E 73 2E 4C 6F 63 61 6C 00 00 )       // ctions.Local..
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 19 53 79 73 74 65 6D 2E 54 72 61 6E 73 61   // ...System.Transa
                                                                                                                                                                 63 74 69 6F 6E 73 2E 4C 6F 63 61 6C 00 00 )       // ctions.Local..
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
  .ver 9:0:0:0
}
.module System.Transactions.Local.dll
// MVID: {92190889-e4f4-46f6-9703-89b1978a8664}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit System.Transactions.CommittableTransaction
       extends System.Transactions.Transaction/*02000013*/
       implements [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/
{
  .custom /*0C000016:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000013*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00002C       */ call       instance void System.Transactions.Transaction/*02000013*/::.ctor() /* 0600002C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CommittableTransaction::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ timeout) cil managed
  // SIG: 20 01 01 11 5D
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00002C       */ call       instance void System.Transactions.Transaction/*02000013*/::.ctor() /* 0600002C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CommittableTransaction::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionOptions/*0200001D*/ options) cil managed
  // SIG: 20 01 01 11 74
  {
    // Method begins at RVA 0x2060
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00002C       */ call       instance void System.Transactions.Transaction/*02000013*/::.ctor() /* 0600002C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CommittableTransaction::.ctor

  .method /*06000004*/ private hidebysig newslot specialname virtual final 
          instance object  System.IAsyncResult.get_AsyncState() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/::get_AsyncState /*01000014::0A000013*/ 
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CommittableTransaction::System.IAsyncResult.get_AsyncState

  .method /*06000005*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000016*/ 
          System.IAsyncResult.get_AsyncWaitHandle() cil managed
  // SIG: 20 00 12 59
  {
    .override [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/::get_AsyncWaitHandle /*01000014::0A000014*/ 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CommittableTransaction::System.IAsyncResult.get_AsyncWaitHandle

  .method /*06000006*/ private hidebysig newslot specialname virtual final 
          instance bool  System.IAsyncResult.get_CompletedSynchronously() cil managed
  // SIG: 20 00 02
  {
    .override [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/::get_CompletedSynchronously /*01000014::0A000015*/ 
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CommittableTransaction::System.IAsyncResult.get_CompletedSynchronously

  .method /*06000007*/ private hidebysig newslot specialname virtual final 
          instance bool  System.IAsyncResult.get_IsCompleted() cil managed
  // SIG: 20 00 02
  {
    .override [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/::get_IsCompleted /*01000014::0A000016*/ 
    // Method begins at RVA 0x2071
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CommittableTransaction::System.IAsyncResult.get_IsCompleted

  .method /*06000008*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/ 
          BeginCommit(class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ asyncCallback,
                      object asyncState) cil managed
  // SIG: 20 02 12 51 12 69 1C
  {
    .custom /*0C00001E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000003*/ 
    .custom /*0C000019:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CommittableTransaction::BeginCommit

  .method /*06000009*/ public hidebysig instance void 
          Commit() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2077
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CommittableTransaction::Commit

  .method /*0600000A*/ public hidebysig instance void 
          EndCommit(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/ asyncResult) cil managed
  // SIG: 20 01 01 12 51
  {
    .custom /*0C000021:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2079
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CommittableTransaction::EndCommit

  .property /*17000001*/ instance callconv(8) object
          System.IAsyncResult.AsyncState()
  {
    .custom /*0C000003:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Transactions.CommittableTransaction/*02000002*/::System.IAsyncResult.get_AsyncState() /* 06000004 */
  } // end of property CommittableTransaction::System.IAsyncResult.AsyncState
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000016*/
          System.IAsyncResult.AsyncWaitHandle()
  {
    .custom /*0C000018:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000016*/ System.Transactions.CommittableTransaction/*02000002*/::System.IAsyncResult.get_AsyncWaitHandle() /* 06000005 */
  } // end of property CommittableTransaction::System.IAsyncResult.AsyncWaitHandle
  .property /*17000003*/ instance callconv(8) bool
          System.IAsyncResult.CompletedSynchronously()
  {
    .get instance bool System.Transactions.CommittableTransaction/*02000002*/::System.IAsyncResult.get_CompletedSynchronously() /* 06000006 */
  } // end of property CommittableTransaction::System.IAsyncResult.CompletedSynchronously
  .property /*17000004*/ instance callconv(8) bool
          System.IAsyncResult.IsCompleted()
  {
    .get instance bool System.Transactions.CommittableTransaction/*02000002*/::System.IAsyncResult.get_IsCompleted() /* 06000007 */
  } // end of property CommittableTransaction::System.IAsyncResult.IsCompleted
} // end of class System.Transactions.CommittableTransaction

.class /*02000003*/ public auto ansi sealed System.Transactions.DependentCloneOption
       extends [System.Runtime/*23000001*/]System.Enum/*0100001B*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Transactions.DependentCloneOption/*02000003*/ BlockCommitUntilComplete = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Transactions.DependentCloneOption/*02000003*/ RollbackIfNotComplete = int32(0x00000001)
} // end of class System.Transactions.DependentCloneOption

.class /*02000004*/ public auto ansi sealed beforefieldinit System.Transactions.DependentTransaction
       extends System.Transactions.Transaction/*02000013*/
{
  .method /*0600000B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x207b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00002C       */ call       instance void System.Transactions.Transaction/*02000013*/::.ctor() /* 0600002C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DependentTransaction::.ctor

  .method /*0600000C*/ public hidebysig instance void 
          Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2083
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DependentTransaction::Complete

} // end of class System.Transactions.DependentTransaction

.class /*02000005*/ public auto ansi beforefieldinit System.Transactions.Enlistment
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .method /*0600000D*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2085
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Enlistment::.ctor

  .method /*0600000E*/ public hidebysig instance void 
          Done() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x208d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Enlistment::Done

} // end of class System.Transactions.Enlistment

.class /*02000006*/ public auto ansi sealed System.Transactions.EnlistmentOptions
       extends [System.Runtime/*23000001*/]System.Enum/*0100001B*/
{
  .custom /*0C00001C:0A000019*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100001C*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype System.Transactions.EnlistmentOptions/*02000006*/ None = int32(0x00000000)
  .field /*04000006*/ public static literal valuetype System.Transactions.EnlistmentOptions/*02000006*/ EnlistDuringPrepareRequired = int32(0x00000001)
} // end of class System.Transactions.EnlistmentOptions

.class /*02000007*/ public auto ansi sealed System.Transactions.EnterpriseServicesInteropOption
       extends [System.Runtime/*23000001*/]System.Enum/*0100001B*/
{
  .field /*04000007*/ public specialname rtspecialname int32 value__
  .field /*04000008*/ public static literal valuetype System.Transactions.EnterpriseServicesInteropOption/*02000007*/ None = int32(0x00000000)
  .field /*04000009*/ public static literal valuetype System.Transactions.EnterpriseServicesInteropOption/*02000007*/ Automatic = int32(0x00000001)
  .field /*0400000A*/ public static literal valuetype System.Transactions.EnterpriseServicesInteropOption/*02000007*/ Full = int32(0x00000002)
} // end of class System.Transactions.EnterpriseServicesInteropOption

.class /*02000008*/ public auto ansi sealed System.Transactions.HostCurrentTransactionCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001D*/
{
  .method /*0600000F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method HostCurrentTransactionCallback::.ctor

  .method /*06000010*/ public hidebysig newslot virtual 
          instance class System.Transactions.Transaction/*02000013*/ 
          Invoke() runtime managed
  // SIG: 20 00 12 4C
  {
    .custom /*0C000026:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  } // end of method HostCurrentTransactionCallback::Invoke

  .method /*06000011*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/ 
          BeginInvoke(class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 02 12 51 12 69 1C
  {
  } // end of method HostCurrentTransactionCallback::BeginInvoke

  .method /*06000012*/ public hidebysig newslot virtual 
          instance class System.Transactions.Transaction/*02000013*/ 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/ result) runtime managed
  // SIG: 20 01 12 4C 12 51
  {
    .param [0]/*0800000B*/ 
    .custom /*0C000024:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 02 00 00 ) 
  } // end of method HostCurrentTransactionCallback::EndInvoke

} // end of class System.Transactions.HostCurrentTransactionCallback

.class /*02000009*/ interface public abstract auto ansi beforefieldinit System.Transactions.IDtcTransaction
{
  .custom /*0C000020:0A00001A*/ instance void [System.Runtime.InteropServices/*23000002*/]System.Runtime.InteropServices.InterfaceTypeAttribute/*0100001F*/::.ctor(valuetype [System.Runtime.InteropServices/*23000002*/]System.Runtime.InteropServices.ComInterfaceType/*0100001E*/) /* 0A00001A */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000013*/ public hidebysig newslot abstract virtual 
          instance void  Abort(native int reason,
                               int32 retaining,
                               int32 'async') cil managed
  // SIG: 20 03 01 18 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IDtcTransaction::Abort

  .method /*06000014*/ public hidebysig newslot abstract virtual 
          instance void  Commit(int32 retaining,
                                int32 commitType,
                                int32 reserved) cil managed
  // SIG: 20 03 01 08 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IDtcTransaction::Commit

  .method /*06000015*/ public hidebysig newslot abstract virtual 
          instance void  GetTransactionInfo(native int transactionInformation) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method IDtcTransaction::GetTransactionInfo

} // end of class System.Transactions.IDtcTransaction

.class /*0200000A*/ interface public abstract auto ansi beforefieldinit System.Transactions.IEnlistmentNotification
{
  .custom /*0C000022:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
  .method /*06000016*/ public hidebysig newslot abstract virtual 
          instance void  Commit(class System.Transactions.Enlistment/*02000005*/ enlistment) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x0
  } // end of method IEnlistmentNotification::Commit

  .method /*06000017*/ public hidebysig newslot abstract virtual 
          instance void  InDoubt(class System.Transactions.Enlistment/*02000005*/ enlistment) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x0
  } // end of method IEnlistmentNotification::InDoubt

  .method /*06000018*/ public hidebysig newslot abstract virtual 
          instance void  Prepare(class System.Transactions.PreparingEnlistment/*02000010*/ preparingEnlistment) cil managed
  // SIG: 20 01 01 12 40
  {
    // Method begins at RVA 0x0
  } // end of method IEnlistmentNotification::Prepare

  .method /*06000019*/ public hidebysig newslot abstract virtual 
          instance void  Rollback(class System.Transactions.Enlistment/*02000005*/ enlistment) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x0
  } // end of method IEnlistmentNotification::Rollback

} // end of class System.Transactions.IEnlistmentNotification

.class /*0200000B*/ interface public abstract auto ansi beforefieldinit System.Transactions.IPromotableSinglePhaseNotification
       implements System.Transactions.ITransactionPromoter/*0200000F*/
{
  .custom /*0C000023:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type System.Transactions.ITransactionPromoter/*0200000F*/
  .custom /*0C000017:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*0600001A*/ public hidebysig newslot abstract virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IPromotableSinglePhaseNotification::Initialize

  .method /*0600001B*/ public hidebysig newslot abstract virtual 
          instance void  Rollback(class System.Transactions.SinglePhaseEnlistment/*02000011*/ singlePhaseEnlistment) cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IPromotableSinglePhaseNotification::Rollback

  .method /*0600001C*/ public hidebysig newslot abstract virtual 
          instance void  SinglePhaseCommit(class System.Transactions.SinglePhaseEnlistment/*02000011*/ singlePhaseEnlistment) cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IPromotableSinglePhaseNotification::SinglePhaseCommit

} // end of class System.Transactions.IPromotableSinglePhaseNotification

.class /*0200000C*/ interface public abstract auto ansi beforefieldinit System.Transactions.ISimpleTransactionSuperior
       implements System.Transactions.ITransactionPromoter/*0200000F*/
{
  .method /*0600001D*/ public hidebysig newslot abstract virtual 
          instance void  Rollback() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ISimpleTransactionSuperior::Rollback

} // end of class System.Transactions.ISimpleTransactionSuperior

.class /*0200000D*/ interface public abstract auto ansi beforefieldinit System.Transactions.ISinglePhaseNotification
       implements System.Transactions.IEnlistmentNotification/*0200000A*/
{
  .method /*0600001E*/ public hidebysig newslot abstract virtual 
          instance void  SinglePhaseCommit(class System.Transactions.SinglePhaseEnlistment/*02000011*/ singlePhaseEnlistment) cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C00003C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISinglePhaseNotification::SinglePhaseCommit

} // end of class System.Transactions.ISinglePhaseNotification

.class /*0200000E*/ public auto ansi sealed System.Transactions.IsolationLevel
       extends [System.Runtime/*23000001*/]System.Enum/*0100001B*/
{
  .field /*0400000B*/ public specialname rtspecialname int32 value__
  .field /*0400000C*/ public static literal valuetype System.Transactions.IsolationLevel/*0200000E*/ Serializable = int32(0x00000000)
  .field /*0400000D*/ public static literal valuetype System.Transactions.IsolationLevel/*0200000E*/ RepeatableRead = int32(0x00000001)
  .field /*0400000E*/ public static literal valuetype System.Transactions.IsolationLevel/*0200000E*/ ReadCommitted = int32(0x00000002)
  .field /*0400000F*/ public static literal valuetype System.Transactions.IsolationLevel/*0200000E*/ ReadUncommitted = int32(0x00000003)
  .field /*04000010*/ public static literal valuetype System.Transactions.IsolationLevel/*0200000E*/ Snapshot = int32(0x00000004)
  .field /*04000011*/ public static literal valuetype System.Transactions.IsolationLevel/*0200000E*/ Chaos = int32(0x00000005)
  .field /*04000012*/ public static literal valuetype System.Transactions.IsolationLevel/*0200000E*/ Unspecified = int32(0x00000006)
} // end of class System.Transactions.IsolationLevel

.class /*0200000F*/ interface public abstract auto ansi beforefieldinit System.Transactions.ITransactionPromoter
{
  .custom /*0C000025:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .method /*0600001F*/ public hidebysig newslot abstract virtual 
          instance uint8[]  Promote() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method ITransactionPromoter::Promote

} // end of class System.Transactions.ITransactionPromoter

.class /*02000010*/ public auto ansi beforefieldinit System.Transactions.PreparingEnlistment
       extends System.Transactions.Enlistment/*02000005*/
{
  .method /*06000020*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x208f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Transactions.Enlistment/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PreparingEnlistment::.ctor

  .method /*06000021*/ public hidebysig instance void 
          ForceRollback() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2097
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PreparingEnlistment::ForceRollback

  .method /*06000022*/ public hidebysig instance void 
          ForceRollback(class [System.Runtime/*23000001*/]System.Exception/*01000020*/ e) cil managed
  // SIG: 20 01 01 12 80 81
  {
    .custom /*0C000042:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2099
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PreparingEnlistment::ForceRollback

  .method /*06000023*/ public hidebysig instance void 
          Prepared() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PreparingEnlistment::Prepared

  .method /*06000024*/ public hidebysig instance uint8[] 
          RecoveryInformation() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C000043:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x209d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PreparingEnlistment::RecoveryInformation

} // end of class System.Transactions.PreparingEnlistment

.class /*02000011*/ public auto ansi beforefieldinit System.Transactions.SinglePhaseEnlistment
       extends System.Transactions.Enlistment/*02000005*/
{
  .custom /*0C000027:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000028:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*06000025*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000D       */ call       instance void System.Transactions.Enlistment/*02000005*/::.ctor() /* 0600000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SinglePhaseEnlistment::.ctor

  .method /*06000026*/ public hidebysig instance void 
          Aborted() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SinglePhaseEnlistment::Aborted

  .method /*06000027*/ public hidebysig instance void 
          Aborted(class [System.Runtime/*23000001*/]System.Exception/*01000020*/ e) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x20aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SinglePhaseEnlistment::Aborted

  .method /*06000028*/ public hidebysig instance void 
          Committed() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SinglePhaseEnlistment::Committed

  .method /*06000029*/ public hidebysig instance void 
          InDoubt() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ae
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SinglePhaseEnlistment::InDoubt

  .method /*0600002A*/ public hidebysig instance void 
          InDoubt(class [System.Runtime/*23000001*/]System.Exception/*01000020*/ e) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x20b0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SinglePhaseEnlistment::InDoubt

} // end of class System.Transactions.SinglePhaseEnlistment

.class /*02000012*/ public auto ansi sealed beforefieldinit System.Transactions.SubordinateTransaction
       extends System.Transactions.Transaction/*02000013*/
{
  .method /*0600002B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.IsolationLevel/*0200000E*/ isoLevel,
                               class System.Transactions.ISimpleTransactionSuperior/*0200000C*/ superior) cil managed
  // SIG: 20 02 01 11 38 12 30
  {
    .custom /*0C000044:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20b2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00002C       */ call       instance void System.Transactions.Transaction/*02000013*/::.ctor() /* 0600002C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SubordinateTransaction::.ctor

} // end of class System.Transactions.SubordinateTransaction

.class /*02000013*/ public auto ansi beforefieldinit System.Transactions.Transaction
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000024*/,
                  [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000021*/
{
  .custom /*0C000029:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000024*/
  .custom /*0C00001A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000021*/
  .custom /*0C00001D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*0600002C*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ba
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Transaction::.ctor

  .method /*0600002D*/ public hidebysig specialname static 
          class System.Transactions.Transaction/*02000013*/ 
          get_Current() cil managed
  // SIG: 00 00 12 4C
  {
    .custom /*0C000045:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::get_Current

  .method /*0600002E*/ public hidebysig specialname static 
          void  set_Current(class System.Transactions.Transaction/*02000013*/ 'value') cil managed
  // SIG: 00 01 01 12 4C
  {
    .custom /*0C000046:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Transaction::set_Current

  .method /*0600002F*/ public hidebysig specialname 
          instance valuetype System.Transactions.IsolationLevel/*0200000E*/ 
          get_IsolationLevel() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::get_IsolationLevel

  .method /*06000030*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ 
          get_PromoterType() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::get_PromoterType

  .method /*06000031*/ public hidebysig specialname 
          instance class System.Transactions.TransactionInformation/*02000019*/ 
          get_TransactionInformation() cil managed
  // SIG: 20 00 12 64
  {
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::get_TransactionInformation

  .method /*06000032*/ public hidebysig specialname 
          instance void  add_TransactionCompleted(class System.Transactions.TransactionCompletedEventHandler/*02000015*/ 'value') cil managed
  // SIG: 20 01 01 12 54
  {
    .custom /*0C000048:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Transaction::add_TransactionCompleted

  .method /*06000033*/ public hidebysig specialname 
          instance void  remove_TransactionCompleted(class System.Transactions.TransactionCompletedEventHandler/*02000015*/ 'value') cil managed
  // SIG: 20 01 01 12 54
  {
    .custom /*0C000049:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Transaction::remove_TransactionCompleted

  .method /*06000034*/ public hidebysig instance class System.Transactions.Transaction/*02000013*/ 
          Clone() cil managed
  // SIG: 20 00 12 4C
  {
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::Clone

  .method /*06000035*/ public hidebysig instance class System.Transactions.DependentTransaction/*02000004*/ 
          DependentClone(valuetype System.Transactions.DependentCloneOption/*02000003*/ cloneOption) cil managed
  // SIG: 20 01 12 10 11 0C
  {
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::DependentClone

  .method /*06000036*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Transaction::Dispose

  .method /*06000037*/ public hidebysig instance class System.Transactions.Enlistment/*02000005*/ 
          EnlistDurable(valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ resourceManagerIdentifier,
                        class System.Transactions.IEnlistmentNotification/*0200000A*/ enlistmentNotification,
                        valuetype System.Transactions.EnlistmentOptions/*02000006*/ enlistmentOptions) cil managed
  // SIG: 20 03 12 14 11 80 95 12 28 11 18
  {
    // Method begins at RVA 0x20dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::EnlistDurable

  .method /*06000038*/ public hidebysig instance class System.Transactions.Enlistment/*02000005*/ 
          EnlistDurable(valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ resourceManagerIdentifier,
                        class System.Transactions.ISinglePhaseNotification/*0200000D*/ singlePhaseNotification,
                        valuetype System.Transactions.EnlistmentOptions/*02000006*/ enlistmentOptions) cil managed
  // SIG: 20 03 12 14 11 80 95 12 34 11 18
  {
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::EnlistDurable

  .method /*06000039*/ public hidebysig instance bool 
          EnlistPromotableSinglePhase(class System.Transactions.IPromotableSinglePhaseNotification/*0200000B*/ promotableSinglePhaseNotification) cil managed
  // SIG: 20 01 02 12 2C
  {
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::EnlistPromotableSinglePhase

  .method /*0600003A*/ public hidebysig instance bool 
          EnlistPromotableSinglePhase(class System.Transactions.IPromotableSinglePhaseNotification/*0200000B*/ promotableSinglePhaseNotification,
                                      valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ promoterType) cil managed
  // SIG: 20 02 02 12 2C 11 80 95
  {
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::EnlistPromotableSinglePhase

  .method /*0600003B*/ public hidebysig instance class System.Transactions.Enlistment/*02000005*/ 
          EnlistVolatile(class System.Transactions.IEnlistmentNotification/*0200000A*/ enlistmentNotification,
                         valuetype System.Transactions.EnlistmentOptions/*02000006*/ enlistmentOptions) cil managed
  // SIG: 20 02 12 14 12 28 11 18
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::EnlistVolatile

  .method /*0600003C*/ public hidebysig instance class System.Transactions.Enlistment/*02000005*/ 
          EnlistVolatile(class System.Transactions.ISinglePhaseNotification/*0200000D*/ singlePhaseNotification,
                         valuetype System.Transactions.EnlistmentOptions/*02000006*/ enlistmentOptions) cil managed
  // SIG: 20 02 12 14 12 34 11 18
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::EnlistVolatile

  .method /*0600003D*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00004A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000031*/ 
    .custom /*0C000047:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000026*/::.ctor(bool) /* 0A00001C */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::Equals

  .method /*0600003E*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::GetHashCode

  .method /*0600003F*/ public hidebysig instance uint8[] 
          GetPromotedToken() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::GetPromotedToken

  .method /*06000040*/ public hidebysig specialname static 
          bool  op_Equality(class System.Transactions.Transaction/*02000013*/ x,
                            class System.Transactions.Transaction/*02000013*/ y) cil managed
  // SIG: 00 02 02 12 4C 12 4C
  {
    .custom /*0C00004B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::op_Equality

  .method /*06000041*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Transactions.Transaction/*02000013*/ x,
                              class System.Transactions.Transaction/*02000013*/ y) cil managed
  // SIG: 00 02 02 12 4C 12 4C
  {
    .custom /*0C00004C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::op_Inequality

  .method /*06000042*/ public hidebysig instance class System.Transactions.Enlistment/*02000005*/ 
          PromoteAndEnlistDurable(valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ resourceManagerIdentifier,
                                  class System.Transactions.IPromotableSinglePhaseNotification/*0200000B*/ promotableNotification,
                                  class System.Transactions.ISinglePhaseNotification/*0200000D*/ enlistmentNotification,
                                  valuetype System.Transactions.EnlistmentOptions/*02000006*/ enlistmentOptions) cil managed
  // SIG: 20 04 12 14 11 80 95 12 2C 12 34 11 18
  {
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Transaction::PromoteAndEnlistDurable

  .method /*06000043*/ public hidebysig instance void 
          Rollback() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2100
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Transaction::Rollback

  .method /*06000044*/ public hidebysig instance void 
          Rollback(class [System.Runtime/*23000001*/]System.Exception/*01000020*/ e) cil managed
  // SIG: 20 01 01 12 80 81
  {
    .custom /*0C00004D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Transaction::Rollback

  .method /*06000045*/ public hidebysig instance void 
          SetDistributedTransactionIdentifier(class System.Transactions.IPromotableSinglePhaseNotification/*0200000B*/ promotableNotification,
                                              valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ distributedTransactionIdentifier) cil managed
  // SIG: 20 02 01 12 2C 11 80 95
  {
    // Method begins at RVA 0x2104
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Transaction::SetDistributedTransactionIdentifier

  .method /*06000046*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000022*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000023*/ context) cil managed
  // SIG: 20 02 01 12 80 89 11 80 8D
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000021*/::GetObjectData /*01000021::0A00001B*/ 
    // Method begins at RVA 0x2106
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Transaction::System.Runtime.Serialization.ISerializable.GetObjectData

  .event /*14000001*/ System.Transactions.TransactionCompletedEventHandler/*02000015*/ TransactionCompleted
  {
    .custom /*0C000004:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Transactions.Transaction/*02000013*/::add_TransactionCompleted(class System.Transactions.TransactionCompletedEventHandler/*02000015*/) /* 06000032 */
    .removeon instance void System.Transactions.Transaction/*02000013*/::remove_TransactionCompleted(class System.Transactions.TransactionCompletedEventHandler/*02000015*/) /* 06000033 */
  } // end of event Transaction::TransactionCompleted
  .property /*17000005*/ callconv(8) class System.Transactions.Transaction/*02000013*/
          Current()
  {
    .custom /*0C00001B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 02 00 00 ) 
    .get class System.Transactions.Transaction/*02000013*/ System.Transactions.Transaction/*02000013*/::get_Current() /* 0600002D */
    .set void System.Transactions.Transaction/*02000013*/::set_Current(class System.Transactions.Transaction/*02000013*/) /* 0600002E */
  } // end of property Transaction::Current
  .property /*17000006*/ instance callconv(8) valuetype System.Transactions.IsolationLevel/*0200000E*/
          IsolationLevel()
  {
    .get instance valuetype System.Transactions.IsolationLevel/*0200000E*/ System.Transactions.Transaction/*02000013*/::get_IsolationLevel() /* 0600002F */
  } // end of property Transaction::IsolationLevel
  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/
          PromoterType()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ System.Transactions.Transaction/*02000013*/::get_PromoterType() /* 06000030 */
  } // end of property Transaction::PromoterType
  .property /*17000008*/ instance callconv(8) class System.Transactions.TransactionInformation/*02000019*/
          TransactionInformation()
  {
    .get instance class System.Transactions.TransactionInformation/*02000019*/ System.Transactions.Transaction/*02000013*/::get_TransactionInformation() /* 06000031 */
  } // end of property Transaction::TransactionInformation
} // end of class System.Transactions.Transaction

.class /*02000014*/ public auto ansi beforefieldinit System.Transactions.TransactionAbortedException
       extends System.Transactions.TransactionException/*02000017*/
{
  .custom /*0C00002C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*06000047*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2108
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionAbortedException::.ctor

  .method /*06000048*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000022*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000023*/ context) cil managed
  // SIG: 20 02 01 12 80 89 11 80 8D
  {
    .custom /*0C00004F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000050:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000027*/::.ctor(string) /* 0A00001D */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C000051:0A00001E*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000029*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000028*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionAbortedException::.ctor

  .method /*06000049*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2118
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionAbortedException::.ctor

  .method /*0600004A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000020*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 81
  {
    // Method begins at RVA 0x2120
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionAbortedException::.ctor

} // end of class System.Transactions.TransactionAbortedException

.class /*02000015*/ public auto ansi sealed System.Transactions.TransactionCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001D*/
{
  .method /*0600004B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method TransactionCompletedEventHandler::.ctor

  .method /*0600004C*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Transactions.TransactionEventArgs/*02000016*/ e) runtime managed
  // SIG: 20 02 01 1C 12 58
  {
    .custom /*0C000054:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000046*/ 
    .custom /*0C00004E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 02 00 00 ) 
  } // end of method TransactionCompletedEventHandler::Invoke

  .method /*0600004D*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/ 
          BeginInvoke(object sender,
                      class System.Transactions.TransactionEventArgs/*02000016*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 51 1C 12 58 12 69 1C
  {
    .param [1]/*08000048*/ 
    .custom /*0C000052:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000049*/ 
    .custom /*0C000053:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 01 00 00 ) 
  } // end of method TransactionCompletedEventHandler::BeginInvoke

  .method /*0600004E*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/ result) runtime managed
  // SIG: 20 01 01 12 51
  {
  } // end of method TransactionCompletedEventHandler::EndInvoke

} // end of class System.Transactions.TransactionCompletedEventHandler

.class /*02000016*/ public auto ansi beforefieldinit System.Transactions.TransactionEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100002A*/
{
  .custom /*0C00002E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*0600004F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2128
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000024       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*0100002A*/::.ctor() /* 0A000024 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionEventArgs::.ctor

  .method /*06000050*/ public hidebysig specialname 
          instance class System.Transactions.Transaction/*02000013*/ 
          get_Transaction() cil managed
  // SIG: 20 00 12 4C
  {
    // Method begins at RVA 0x2130
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionEventArgs::get_Transaction

  .property /*17000009*/ instance callconv(8) class System.Transactions.Transaction/*02000013*/
          Transaction()
  {
    .get instance class System.Transactions.Transaction/*02000013*/ System.Transactions.TransactionEventArgs/*02000016*/::get_Transaction() /* 06000050 */
  } // end of property TransactionEventArgs::Transaction
} // end of class System.Transactions.TransactionEventArgs

.class /*02000017*/ public auto ansi beforefieldinit System.Transactions.TransactionException
       extends [System.Runtime/*23000001*/]System.SystemException/*0100002B*/
{
  .custom /*0C000030:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000031:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2133
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002B*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionException::.ctor

  .method /*06000052*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000022*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000023*/ context) cil managed
  // SIG: 20 02 01 12 80 89 11 80 8D
  {
    .custom /*0C000055:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000056:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000027*/::.ctor(string) /* 0A00001D */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C000057:0A00001E*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000029*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000028*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x213b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002B*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionException::.ctor

  .method /*06000053*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2143
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002B*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionException::.ctor

  .method /*06000054*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000020*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 81
  {
    // Method begins at RVA 0x214b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002B*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionException::.ctor

} // end of class System.Transactions.TransactionException

.class /*02000018*/ public auto ansi beforefieldinit System.Transactions.TransactionInDoubtException
       extends System.Transactions.TransactionException/*02000017*/
{
  .custom /*0C000032:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000033:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*06000055*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2153
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionInDoubtException::.ctor

  .method /*06000056*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000022*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000023*/ context) cil managed
  // SIG: 20 02 01 12 80 89 11 80 8D
  {
    .custom /*0C000058:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000059:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000027*/::.ctor(string) /* 0A00001D */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C00005A:0A00001E*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000029*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000028*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x215b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionInDoubtException::.ctor

  .method /*06000057*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2163
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionInDoubtException::.ctor

  .method /*06000058*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000020*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 81
  {
    // Method begins at RVA 0x216b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionInDoubtException::.ctor

} // end of class System.Transactions.TransactionInDoubtException

.class /*02000019*/ public auto ansi beforefieldinit System.Transactions.TransactionInformation
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000034:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*06000059*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2173
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionInformation::.ctor

  .method /*0600005A*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002C*/ 
          get_CreationTime() cil managed
  // SIG: 20 00 11 80 B1
  {
    // Method begins at RVA 0x217b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInformation::get_CreationTime

  .method /*0600005B*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ 
          get_DistributedIdentifier() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x217e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInformation::get_DistributedIdentifier

  .method /*0600005C*/ public hidebysig specialname 
          instance string  get_LocalIdentifier() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2181
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInformation::get_LocalIdentifier

  .method /*0600005D*/ public hidebysig specialname 
          instance valuetype System.Transactions.TransactionStatus/*02000023*/ 
          get_Status() cil managed
  // SIG: 20 00 11 80 8C
  {
    // Method begins at RVA 0x2184
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInformation::get_Status

  .property /*1700000A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002C*/
          CreationTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002C*/ System.Transactions.TransactionInformation/*02000019*/::get_CreationTime() /* 0600005A */
  } // end of property TransactionInformation::CreationTime
  .property /*1700000B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/
          DistributedIdentifier()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ System.Transactions.TransactionInformation/*02000019*/::get_DistributedIdentifier() /* 0600005B */
  } // end of property TransactionInformation::DistributedIdentifier
  .property /*1700000C*/ instance callconv(8) string
          LocalIdentifier()
  {
    .get instance string System.Transactions.TransactionInformation/*02000019*/::get_LocalIdentifier() /* 0600005C */
  } // end of property TransactionInformation::LocalIdentifier
  .property /*1700000D*/ instance callconv(8) valuetype System.Transactions.TransactionStatus/*02000023*/
          Status()
  {
    .get instance valuetype System.Transactions.TransactionStatus/*02000023*/ System.Transactions.TransactionInformation/*02000019*/::get_Status() /* 0600005D */
  } // end of property TransactionInformation::Status
} // end of class System.Transactions.TransactionInformation

.class /*0200001A*/ public abstract auto ansi sealed beforefieldinit System.Transactions.TransactionInterop
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000036:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000037:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .field /*04000013*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ PromoterTypeDtc
  .method /*0600005E*/ public hidebysig static 
          class System.Transactions.IDtcTransaction/*02000009*/ 
          GetDtcTransaction(class System.Transactions.Transaction/*02000013*/ transaction) cil managed
  // SIG: 00 01 12 24 12 4C
  {
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInterop::GetDtcTransaction

  .method /*0600005F*/ public hidebysig static 
          uint8[]  GetExportCookie(class System.Transactions.Transaction/*02000013*/ transaction,
                                   uint8[] whereabouts) cil managed
  // SIG: 00 02 1D 05 12 4C 1D 05
  {
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInterop::GetExportCookie

  .method /*06000060*/ public hidebysig static 
          class System.Transactions.Transaction/*02000013*/ 
          GetTransactionFromDtcTransaction(class System.Transactions.IDtcTransaction/*02000009*/ transactionNative) cil managed
  // SIG: 00 01 12 4C 12 24
  {
    // Method begins at RVA 0x218d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInterop::GetTransactionFromDtcTransaction

  .method /*06000061*/ public hidebysig static 
          class System.Transactions.Transaction/*02000013*/ 
          GetTransactionFromExportCookie(uint8[] cookie) cil managed
  // SIG: 00 01 12 4C 1D 05
  {
    // Method begins at RVA 0x2190
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInterop::GetTransactionFromExportCookie

  .method /*06000062*/ public hidebysig static 
          class System.Transactions.Transaction/*02000013*/ 
          GetTransactionFromTransmitterPropagationToken(uint8[] propagationToken) cil managed
  // SIG: 00 01 12 4C 1D 05
  {
    // Method begins at RVA 0x2193
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInterop::GetTransactionFromTransmitterPropagationToken

  .method /*06000063*/ public hidebysig static 
          uint8[]  GetTransmitterPropagationToken(class System.Transactions.Transaction/*02000013*/ transaction) cil managed
  // SIG: 00 01 1D 05 12 4C
  {
    // Method begins at RVA 0x2196
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInterop::GetTransmitterPropagationToken

  .method /*06000064*/ public hidebysig static 
          uint8[]  GetWhereabouts() cil managed
  // SIG: 00 00 1D 05
  {
    // Method begins at RVA 0x2199
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionInterop::GetWhereabouts

} // end of class System.Transactions.TransactionInterop

.class /*0200001B*/ public abstract auto ansi sealed beforefieldinit System.Transactions.TransactionManager
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000038:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000039:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .field /*04000014*/ private static bool '<ImplicitDistributedTransactions>k__BackingField'
  .custom /*0C00002B:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100002D*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
  .method /*06000065*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ 
          get_DefaultTimeout() cil managed
  // SIG: 00 00 11 5D
  {
    // Method begins at RVA 0x219c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionManager::get_DefaultTimeout

  .method /*06000066*/ public hidebysig specialname static 
          void  set_DefaultTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ 'value') cil managed
  // SIG: 00 01 01 11 5D
  {
    // Method begins at RVA 0x219f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionManager::set_DefaultTimeout

  .method /*06000067*/ public hidebysig specialname static 
          class System.Transactions.HostCurrentTransactionCallback/*02000008*/ 
          get_HostCurrentCallback() cil managed
  // SIG: 00 00 12 20
  {
    // Method begins at RVA 0x21a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionManager::get_HostCurrentCallback

  .method /*06000068*/ public hidebysig specialname static 
          void  set_HostCurrentCallback(class System.Transactions.HostCurrentTransactionCallback/*02000008*/ 'value') cil managed
  // SIG: 00 01 01 12 20
  {
    .param [1]/*0800005F*/ 
    .custom /*0C00005B:0A000020*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*0100002E*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionManager::set_HostCurrentCallback

  .method /*06000069*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ 
          get_MaximumTimeout() cil managed
  // SIG: 00 00 11 5D
  {
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionManager::get_MaximumTimeout

  .method /*0600006A*/ public hidebysig specialname static 
          void  set_MaximumTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ 'value') cil managed
  // SIG: 00 01 01 11 5D
  {
    // Method begins at RVA 0x21a9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionManager::set_MaximumTimeout

  .method /*0600006B*/ public hidebysig specialname static 
          bool  get_ImplicitDistributedTransactions() cil managed
  // SIG: 00 00 02
  {
    .custom /*0C00005C:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100002D*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ab
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 7E   | (04)000014       */ ldsfld     bool System.Transactions.TransactionManager/*0200001B*/::'<ImplicitDistributedTransactions>k__BackingField' /* 04000014 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method TransactionManager::get_ImplicitDistributedTransactions

  .method /*0600006C*/ public hidebysig specialname static 
          void  set_ImplicitDistributedTransactions(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    .custom /*0C00005D:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100002D*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    .custom /*0C00005E:0A000021*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100002F*/::.ctor(string) /* 0A000021 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C00005F:0A000022*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000030*/::.ctor(string) /* 0A000022 */ = ( 01 00 80 D8 44 69 73 74 72 69 62 75 74 65 64 20   // ....Distributed 
                                                                                                                                                                                            74 72 61 6E 73 61 63 74 69 6F 6E 73 20 73 75 70   // transactions sup
                                                                                                                                                                                            70 6F 72 74 20 6D 61 79 20 6E 6F 74 20 62 65 20   // port may not be 
                                                                                                                                                                                            63 6F 6D 70 61 74 69 62 6C 65 20 77 69 74 68 20   // compatible with 
                                                                                                                                                                                            74 72 69 6D 6D 69 6E 67 2E 20 49 66 20 79 6F 75   // trimming. If you
                                                                                                                                                                                            72 20 70 72 6F 67 72 61 6D 20 63 72 65 61 74 65   // r program create
                                                                                                                                                                                            73 20 61 20 64 69 73 74 72 69 62 75 74 65 64 20   // s a distributed 
                                                                                                                                                                                            74 72 61 6E 73 61 63 74 69 6F 6E 20 76 69 61 20   // transaction via 
                                                                                                                                                                                            53 79 73 74 65 6D 2E 54 72 61 6E 73 61 63 74 69   // System.Transacti
                                                                                                                                                                                            6F 6E 73 2C 20 74 68 65 20 63 6F 72 72 65 63 74   // ons, the correct
                                                                                                                                                                                            6E 65 73 73 20 6F 66 20 74 68 65 20 61 70 70 6C   // ness of the appl
                                                                                                                                                                                            69 63 61 74 69 6F 6E 20 63 61 6E 6E 6F 74 20 62   // ication cannot b
                                                                                                                                                                                            65 20 67 75 61 72 61 6E 74 65 65 64 20 61 66 74   // e guaranteed aft
                                                                                                                                                                                            65 72 20 74 72 69 6D 6D 69 6E 67 2E 00 00 )       // er trimming...
    // Method begins at RVA 0x21b2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 80   | (04)000014       */ stsfld     bool System.Transactions.TransactionManager/*0200001B*/::'<ImplicitDistributedTransactions>k__BackingField' /* 04000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionManager::set_ImplicitDistributedTransactions

  .method /*0600006D*/ public hidebysig specialname static 
          void  add_DistributedTransactionStarted(class System.Transactions.TransactionStartedEventHandler/*02000022*/ 'value') cil managed
  // SIG: 00 01 01 12 80 88
  {
    // Method begins at RVA 0x21ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionManager::add_DistributedTransactionStarted

  .method /*0600006E*/ public hidebysig specialname static 
          void  remove_DistributedTransactionStarted(class System.Transactions.TransactionStartedEventHandler/*02000022*/ 'value') cil managed
  // SIG: 00 01 01 12 80 88
  {
    // Method begins at RVA 0x21bc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionManager::remove_DistributedTransactionStarted

  .method /*0600006F*/ public hidebysig static 
          void  RecoveryComplete(valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ resourceManagerIdentifier) cil managed
  // SIG: 00 01 01 11 80 95
  {
    // Method begins at RVA 0x21be
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionManager::RecoveryComplete

  .method /*06000070*/ public hidebysig static 
          class System.Transactions.Enlistment/*02000005*/ 
          Reenlist(valuetype [System.Runtime/*23000001*/]System.Guid/*01000025*/ resourceManagerIdentifier,
                   uint8[] recoveryInformation,
                   class System.Transactions.IEnlistmentNotification/*0200000A*/ enlistmentNotification) cil managed
  // SIG: 00 03 12 14 11 80 95 1D 05 12 28
  {
    .custom /*0C000061:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionManager::Reenlist

  .event /*14000002*/ System.Transactions.TransactionStartedEventHandler/*02000022*/ DistributedTransactionStarted
  {
    .addon void System.Transactions.TransactionManager/*0200001B*/::add_DistributedTransactionStarted(class System.Transactions.TransactionStartedEventHandler/*02000022*/) /* 0600006D */
    .removeon void System.Transactions.TransactionManager/*0200001B*/::remove_DistributedTransactionStarted(class System.Transactions.TransactionStartedEventHandler/*02000022*/) /* 0600006E */
  } // end of event TransactionManager::DistributedTransactionStarted
  .property /*1700000E*/ callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/
          DefaultTimeout()
  {
    .get valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ System.Transactions.TransactionManager/*0200001B*/::get_DefaultTimeout() /* 06000065 */
    .set void System.Transactions.TransactionManager/*0200001B*/::set_DefaultTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/) /* 06000066 */
  } // end of property TransactionManager::DefaultTimeout
  .property /*1700000F*/ callconv(8) class System.Transactions.HostCurrentTransactionCallback/*02000008*/
          HostCurrentCallback()
  {
    .get class System.Transactions.HostCurrentTransactionCallback/*02000008*/ System.Transactions.TransactionManager/*0200001B*/::get_HostCurrentCallback() /* 06000067 */
    .set void System.Transactions.TransactionManager/*0200001B*/::set_HostCurrentCallback(class System.Transactions.HostCurrentTransactionCallback/*02000008*/) /* 06000068 */
  } // end of property TransactionManager::HostCurrentCallback
  .property /*17000010*/ callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/
          MaximumTimeout()
  {
    .get valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ System.Transactions.TransactionManager/*0200001B*/::get_MaximumTimeout() /* 06000069 */
    .set void System.Transactions.TransactionManager/*0200001B*/::set_MaximumTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/) /* 0600006A */
  } // end of property TransactionManager::MaximumTimeout
  .property /*17000011*/ callconv(8) bool
          ImplicitDistributedTransactions()
  {
    .get bool System.Transactions.TransactionManager/*0200001B*/::get_ImplicitDistributedTransactions() /* 0600006B */
    .set void System.Transactions.TransactionManager/*0200001B*/::set_ImplicitDistributedTransactions(bool) /* 0600006C */
  } // end of property TransactionManager::ImplicitDistributedTransactions
} // end of class System.Transactions.TransactionManager

.class /*0200001C*/ public auto ansi beforefieldinit System.Transactions.TransactionManagerCommunicationException
       extends System.Transactions.TransactionException/*02000017*/
{
  .custom /*0C00003A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*06000071*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21c3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionManagerCommunicationException::.ctor

  .method /*06000072*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000022*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000023*/ context) cil managed
  // SIG: 20 02 01 12 80 89 11 80 8D
  {
    .custom /*0C000062:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000063:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000027*/::.ctor(string) /* 0A00001D */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C000064:0A00001E*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000029*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000028*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x21cb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionManagerCommunicationException::.ctor

  .method /*06000073*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21d3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionManagerCommunicationException::.ctor

  .method /*06000074*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000020*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 81
  {
    // Method begins at RVA 0x21db
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionManagerCommunicationException::.ctor

} // end of class System.Transactions.TransactionManagerCommunicationException

.class /*0200001D*/ public sequential ansi sealed beforefieldinit System.Transactions.TransactionOptions
       extends [System.Runtime/*23000001*/]System.ValueType/*01000031*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000032*/<valuetype System.Transactions.TransactionOptions/*0200001D*/>/*1B000001*/
{
  .field /*04000015*/ private initonly int32 _dummyPrimitive
  .method /*06000075*/ public hidebysig specialname 
          instance valuetype System.Transactions.IsolationLevel/*0200000E*/ 
          get_IsolationLevel() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x21e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionOptions::get_IsolationLevel

  .method /*06000076*/ public hidebysig specialname 
          instance void  set_IsolationLevel(valuetype System.Transactions.IsolationLevel/*0200000E*/ 'value') cil managed
  // SIG: 20 01 01 11 38
  {
    // Method begins at RVA 0x21e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionOptions::set_IsolationLevel

  .method /*06000077*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ 
          get_Timeout() cil managed
  // SIG: 20 00 11 5D
  {
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionOptions::get_Timeout

  .method /*06000078*/ public hidebysig specialname 
          instance void  set_Timeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ 'value') cil managed
  // SIG: 20 01 01 11 5D
  {
    // Method begins at RVA 0x21eb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionOptions::set_Timeout

  .method /*06000079*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000065:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800006F*/ 
    .custom /*0C000060:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000026*/::.ctor(bool) /* 0A00001C */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionOptions::Equals

  .method /*0600007A*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Transactions.TransactionOptions/*0200001D*/ other) cil managed
  // SIG: 20 01 02 11 74
  {
    // Method begins at RVA 0x21f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionOptions::Equals

  .method /*0600007B*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionOptions::GetHashCode

  .method /*0600007C*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Transactions.TransactionOptions/*0200001D*/ x,
                            valuetype System.Transactions.TransactionOptions/*0200001D*/ y) cil managed
  // SIG: 00 02 02 11 74 11 74
  {
    // Method begins at RVA 0x21f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionOptions::op_Equality

  .method /*0600007D*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Transactions.TransactionOptions/*0200001D*/ x,
                              valuetype System.Transactions.TransactionOptions/*0200001D*/ y) cil managed
  // SIG: 00 02 02 11 74 11 74
  {
    // Method begins at RVA 0x21f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransactionOptions::op_Inequality

  .property /*17000012*/ instance callconv(8) valuetype System.Transactions.IsolationLevel/*0200000E*/
          IsolationLevel()
  {
    .get instance valuetype System.Transactions.IsolationLevel/*0200000E*/ System.Transactions.TransactionOptions/*0200001D*/::get_IsolationLevel() /* 06000075 */
    .set instance void System.Transactions.TransactionOptions/*0200001D*/::set_IsolationLevel(valuetype System.Transactions.IsolationLevel/*0200000E*/) /* 06000076 */
  } // end of property TransactionOptions::IsolationLevel
  .property /*17000013*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/
          Timeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ System.Transactions.TransactionOptions/*0200001D*/::get_Timeout() /* 06000077 */
    .set instance void System.Transactions.TransactionOptions/*0200001D*/::set_Timeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/) /* 06000078 */
  } // end of property TransactionOptions::Timeout
} // end of class System.Transactions.TransactionOptions

.class /*0200001E*/ public auto ansi beforefieldinit System.Transactions.TransactionPromotionException
       extends System.Transactions.TransactionException/*02000017*/
{
  .custom /*0C00003D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*0600007E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21fc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionPromotionException::.ctor

  .method /*0600007F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000022*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000023*/ context) cil managed
  // SIG: 20 02 01 12 80 89 11 80 8D
  {
    .custom /*0C000066:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000067:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000027*/::.ctor(string) /* 0A00001D */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C000068:0A00001E*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000029*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000028*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2204
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionPromotionException::.ctor

  .method /*06000080*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x220c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionPromotionException::.ctor

  .method /*06000081*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000020*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 81
  {
    // Method begins at RVA 0x2214
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000051       */ call       instance void System.Transactions.TransactionException/*02000017*/::.ctor() /* 06000051 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionPromotionException::.ctor

} // end of class System.Transactions.TransactionPromotionException

.class /*0200001F*/ public auto ansi sealed beforefieldinit System.Transactions.TransactionScope
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000024*/
{
  .custom /*0C00003F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000040:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000041:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000013*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000024*/
  .custom /*0C00001F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*06000082*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x221c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Transactions.Transaction/*02000013*/ transactionToUse) cil managed
  // SIG: 20 01 01 12 4C
  {
    // Method begins at RVA 0x2224
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*06000084*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Transactions.Transaction/*02000013*/ transactionToUse,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ scopeTimeout) cil managed
  // SIG: 20 02 01 12 4C 11 5D
  {
    // Method begins at RVA 0x222c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*06000085*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Transactions.Transaction/*02000013*/ transactionToUse,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ scopeTimeout,
                               valuetype System.Transactions.EnterpriseServicesInteropOption/*02000007*/ interopOption) cil managed
  // SIG: 20 03 01 12 4C 11 5D 11 1C
  {
    // Method begins at RVA 0x2234
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*06000086*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Transactions.Transaction/*02000013*/ transactionToUse,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ scopeTimeout,
                               valuetype System.Transactions.TransactionScopeAsyncFlowOption/*02000020*/ asyncFlowOption) cil managed
  // SIG: 20 03 01 12 4C 11 5D 11 80 80
  {
    // Method begins at RVA 0x223c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*06000087*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Transactions.Transaction/*02000013*/ transactionToUse,
                               valuetype System.Transactions.TransactionScopeAsyncFlowOption/*02000020*/ asyncFlowOption) cil managed
  // SIG: 20 02 01 12 4C 11 80 80
  {
    // Method begins at RVA 0x2244
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*06000088*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionScopeAsyncFlowOption/*02000020*/ asyncFlowOption) cil managed
  // SIG: 20 01 01 11 80 80
  {
    // Method begins at RVA 0x224c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*06000089*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionScopeOption/*02000021*/ scopeOption) cil managed
  // SIG: 20 01 01 11 80 84
  {
    // Method begins at RVA 0x2254
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*0600008A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionScopeOption/*02000021*/ scopeOption,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ scopeTimeout) cil managed
  // SIG: 20 02 01 11 80 84 11 5D
  {
    // Method begins at RVA 0x225c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*0600008B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionScopeOption/*02000021*/ scopeOption,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000017*/ scopeTimeout,
                               valuetype System.Transactions.TransactionScopeAsyncFlowOption/*02000020*/ asyncFlowOption) cil managed
  // SIG: 20 03 01 11 80 84 11 5D 11 80 80
  {
    // Method begins at RVA 0x2264
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*0600008C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionScopeOption/*02000021*/ scopeOption,
                               valuetype System.Transactions.TransactionOptions/*0200001D*/ transactionOptions) cil managed
  // SIG: 20 02 01 11 80 84 11 74
  {
    // Method begins at RVA 0x226c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*0600008D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionScopeOption/*02000021*/ scopeOption,
                               valuetype System.Transactions.TransactionOptions/*0200001D*/ transactionOptions,
                               valuetype System.Transactions.EnterpriseServicesInteropOption/*02000007*/ interopOption) cil managed
  // SIG: 20 03 01 11 80 84 11 74 11 1C
  {
    // Method begins at RVA 0x2274
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*0600008E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionScopeOption/*02000021*/ scopeOption,
                               valuetype System.Transactions.TransactionOptions/*0200001D*/ transactionOptions,
                               valuetype System.Transactions.TransactionScopeAsyncFlowOption/*02000020*/ asyncFlowOption) cil managed
  // SIG: 20 03 01 11 80 84 11 74 11 80 80
  {
    // Method begins at RVA 0x227c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*0600008F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Transactions.TransactionScopeOption/*02000021*/ scopeOption,
                               valuetype System.Transactions.TransactionScopeAsyncFlowOption/*02000020*/ asyncFlowOption) cil managed
  // SIG: 20 02 01 11 80 84 11 80 80
  {
    // Method begins at RVA 0x2284
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransactionScope::.ctor

  .method /*06000090*/ public hidebysig instance void 
          Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionScope::Complete

  .method /*06000091*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransactionScope::Dispose

} // end of class System.Transactions.TransactionScope

.class /*02000020*/ public auto ansi sealed System.Transactions.TransactionScopeAsyncFlowOption
       extends [System.Runtime/*23000001*/]System.Enum/*0100001B*/
{
  .field /*04000016*/ public specialname rtspecialname int32 value__
  .field /*04000017*/ public static literal valuetype System.Transactions.TransactionScopeAsyncFlowOption/*02000020*/ Suppress = int32(0x00000000)
  .field /*04000018*/ public static literal valuetype System.Transactions.TransactionScopeAsyncFlowOption/*02000020*/ Enabled = int32(0x00000001)
} // end of class System.Transactions.TransactionScopeAsyncFlowOption

.class /*02000021*/ public auto ansi sealed System.Transactions.TransactionScopeOption
       extends [System.Runtime/*23000001*/]System.Enum/*0100001B*/
{
  .field /*04000019*/ public specialname rtspecialname int32 value__
  .field /*0400001A*/ public static literal valuetype System.Transactions.TransactionScopeOption/*02000021*/ Required = int32(0x00000000)
  .field /*0400001B*/ public static literal valuetype System.Transactions.TransactionScopeOption/*02000021*/ RequiresNew = int32(0x00000001)
  .field /*0400001C*/ public static literal valuetype System.Transactions.TransactionScopeOption/*02000021*/ Suppress = int32(0x00000002)
} // end of class System.Transactions.TransactionScopeOption

.class /*02000022*/ public auto ansi sealed System.Transactions.TransactionStartedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001D*/
{
  .method /*06000092*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method TransactionStartedEventHandler::.ctor

  .method /*06000093*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Transactions.TransactionEventArgs/*02000016*/ e) runtime managed
  // SIG: 20 02 01 1C 12 58
  {
    .custom /*0C000069:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000018*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000098*/ 
    .custom /*0C00006A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 02 00 00 ) 
  } // end of method TransactionStartedEventHandler::Invoke

  .method /*06000094*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/ 
          BeginInvoke(object sender,
                      class System.Transactions.TransactionEventArgs/*02000016*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001A*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 51 1C 12 58 12 69 1C
  {
    .param [1]/*0800009A*/ 
    .custom /*0C00006B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800009B*/ 
    .custom /*0C00006C:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000019*/::.ctor(uint8) /* 0A000018 */ = ( 01 00 01 00 00 ) 
  } // end of method TransactionStartedEventHandler::BeginInvoke

  .method /*06000095*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000014*/ result) runtime managed
  // SIG: 20 01 01 12 51
  {
  } // end of method TransactionStartedEventHandler::EndInvoke

} // end of class System.Transactions.TransactionStartedEventHandler

.class /*02000023*/ public auto ansi sealed System.Transactions.TransactionStatus
       extends [System.Runtime/*23000001*/]System.Enum/*0100001B*/
{
  .field /*0400001D*/ public specialname rtspecialname int32 value__
  .field /*0400001E*/ public static literal valuetype System.Transactions.TransactionStatus/*02000023*/ Active = int32(0x00000000)
  .field /*0400001F*/ public static literal valuetype System.Transactions.TransactionStatus/*02000023*/ Committed = int32(0x00000001)
  .field /*04000020*/ public static literal valuetype System.Transactions.TransactionStatus/*02000023*/ Aborted = int32(0x00000002)
  .field /*04000021*/ public static literal valuetype System.Transactions.TransactionStatus/*02000023*/ InDoubt = int32(0x00000003)
} // end of class System.Transactions.TransactionStatus


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
