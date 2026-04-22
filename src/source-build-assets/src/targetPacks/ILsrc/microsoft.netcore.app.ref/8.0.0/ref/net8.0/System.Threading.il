


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
// Time-date stamp:            0xc0193b7b
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00004c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00006b42
// Base of code:                   0x00002000
// Base of data:                   0x00008000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x0000c000
// Size of headers:                0x00000200
// Checksum:                       0x0000b5ea
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00006af0 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000464] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00005600 [0x00002910] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00006ad4 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00004b48 Virtual Size
//              0x00002000 Virtual Address
//              0x00004c00 Size of Raw Data
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
//              0x00000464 Virtual Size
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00004e00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x0000a000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00005400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00006000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000b44
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00006b32 Import Name Table
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
// 0x00002564 [0x000044f0] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00006a54 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00002a9c Size
//              '#~' Name
//    Stream 2:
//              0x00002b08 Offset
//              0x0000116c Size
//              '#Strings' Name
//    Stream 3:
//              0x00003c74 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00003c78 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00003c88 Offset
//              0x00000868 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000004090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 32528
// PE header size       : 512 (496 used)    ( 1.57%)
// PE additional info   : 11835             (36.38%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.22%)
// CLR meta-data size  : 17648              (54.25%)
// CLR additional info : 128                ( 0.39%)
// CLR method headers  : 375                ( 1.15%)
// Managed code         : 922               ( 2.83%)
// Data                 : 2048              ( 6.30%)
// Unaccounted          : -1012             (-3.11%)

// Num.of PE sections   : 3
//   .text    - 19456
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 17648
//   Module        -    1 (10 bytes)
//   TypeDef       -   37 (518 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   57 (342 bytes)
//   MethodDef     -  354 (4956 bytes)    0 abstract, 0 native, 346 bodies
//   FieldDef      -   14 (84 bytes)      0 constant
//   MemberRef     -   39 (234 bytes)
//   ParamDef      -  395 (2370 bytes)
//   MethodImpl    -    1 (6 bytes)
//   Constant      -    4 (24 bytes)
//   CustomAttribute-  247 (1482 bytes)
//   InterfaceImpl -   12 (48 bytes)
//   PropertyMap   -   15 (60 bytes)
//   Property      -   43 (258 bytes)
//   MethodSemantic-   46 (276 bytes)
//   TypeSpec      -    2 (4 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   GenericParam  -   12 (96 bytes)
//   Strings       -  4459 bytes
//   Blobs         -  2152 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   207 bytes

// CLR additional info : 128

// CLR method headers : 375
//   Num.of method bodies  - 346
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 345
//   Num.of fat sections   - 0
//   Num.of small sections - 1

// Managed code : 922
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class Barrier                        (public) (auto) (ansi) 
// Class BarrierPostPhaseException      (public) (auto) (ansi) 
// Class CountdownEvent                 (public) (auto) (ansi) 
// Class HostExecutionContext           (public) (auto) (ansi) 
// Class HostExecutionContextManager    (public) (auto) (ansi) 
// Class LockCookie                     (public) (sequential) (ansi) (sealed) 
// Class ReaderWriterLock               (public) (auto) (ansi) (sealed) 
// Class AbandonedMutexException        (public) (auto) (ansi) 
// Class AsyncFlowControl               (public) (sequential) (ansi) (sealed) 
// Class AsyncLocalValueChangedArgs`1   (public) (sequential) (ansi) (sealed) 
// Class AsyncLocal`1                   (public) (auto) (ansi) (sealed) 
// Class AutoResetEvent                 (public) (auto) (ansi) (sealed) 
// Class ContextCallback                (public) (auto) (ansi) (sealed) 
// Class EventResetMode                 (public) (auto) (ansi) (sealed) 
// Class EventWaitHandle                (public) (auto) (ansi) 
// Class ExecutionContext               (public) (auto) (ansi) (sealed) 
// Class Interlocked                    (public) (abstract) (auto) (ansi) (sealed) 
// Class LazyInitializer                (public) (abstract) (auto) (ansi) (sealed) 
// Class LockRecursionException         (public) (auto) (ansi) 
// Class LockRecursionPolicy            (public) (auto) (ansi) (sealed) 
// Class ManualResetEvent               (public) (auto) (ansi) (sealed) 
// Class ManualResetEventSlim           (public) (auto) (ansi) 
// Class Monitor                        (public) (abstract) (auto) (ansi) (sealed) 
// Class Mutex                          (public) (auto) (ansi) (sealed) 
// Class ReaderWriterLockSlim           (public) (auto) (ansi) 
// Class Semaphore                      (public) (auto) (ansi) (sealed) 
// Class SemaphoreFullException         (public) (auto) (ansi) 
// Class SemaphoreSlim                  (public) (auto) (ansi) 
// Class SendOrPostCallback             (public) (auto) (ansi) (sealed) 
// Class SpinLock                       (public) (sequential) (ansi) (sealed) 
// Class SpinWait                       (public) (sequential) (ansi) (sealed) 
// Class SynchronizationContext         (public) (auto) (ansi) 
// Class SynchronizationLockException   (public) (auto) (ansi) 
// Class ThreadLocal`1                  (public) (auto) (ansi) 
// Class Volatile                       (public) (abstract) (auto) (ansi) (sealed) 
// Class WaitHandleCannotBeOpenedException (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly /*20000001*/ noplatform System.Threading
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
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 10 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                        69 6E 67 00 00 )                                  // ing..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000E:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 10 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                       69 6E 67 00 00 )                                  // ing..
  .custom /*0C00000F:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 38 2E 30 2E 32 33 2E 35 33 31 30 33 00   // ...8.0.23.53103.
                                                                                                                                                                       00 ) 
  .custom /*0C000010:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 38 2E 30 2E 30 2B 35 35 33 35 65 33 31   // ...8.0.0+5535e31
                                                                                                                                                                                61 37 31 32 33 34 33 61 36 33 66 35 64 37 64 37   // a712343a63f5d7d7
                                                                                                                                                                                39 36 63 64 38 37 34 65 35 36 33 65 35 61 63 31   // 96cd874e563e5ac1
                                                                                                                                                                                34 00 00 )                                        // 4..
  .custom /*0C000011:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000012:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                 69 6E 67 00 00 )                                  // ing..
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
  .ver 8:0:0:0
}
.module System.Threading.dll
// MVID: {105ff245-42b5-4364-89af-60c454a882bc}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.Threading.Barrier
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 participantCount) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Barrier::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 participantCount,
                               class [System.Runtime/*23000001*/]System.Action`1/*01000016*/<class System.Threading.Barrier/*02000002*/> postPhaseAction) cil managed
  // SIG: 20 02 01 08 15 12 59 01 12 08
  {
    .param [2]/*08000003*/ 
    .custom /*0C000017:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000012 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Barrier::.ctor

  .method /*06000003*/ public hidebysig specialname 
          instance int64  get_CurrentPhaseNumber() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2060
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::get_CurrentPhaseNumber

  .method /*06000004*/ public hidebysig specialname 
          instance int32  get_ParticipantCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2063
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::get_ParticipantCount

  .method /*06000005*/ public hidebysig specialname 
          instance int32  get_ParticipantsRemaining() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2066
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::get_ParticipantsRemaining

  .method /*06000006*/ public hidebysig instance int64 
          AddParticipant() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C00001E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2069
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::AddParticipant

  .method /*06000007*/ public hidebysig instance int64 
          AddParticipants(int32 participantCount) cil managed
  // SIG: 20 01 0A 08
  {
    .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x206c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::AddParticipants

  .method /*06000008*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Barrier::Dispose

  .method /*06000009*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2071
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Barrier::Dispose

  .method /*0600000A*/ public hidebysig instance void 
          RemoveParticipant() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2073
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Barrier::RemoveParticipant

  .method /*0600000B*/ public hidebysig instance void 
          RemoveParticipants(int32 participantCount) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2075
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Barrier::RemoveParticipants

  .method /*0600000C*/ public hidebysig instance void 
          SignalAndWait() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2077
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Barrier::SignalAndWait

  .method /*0600000D*/ public hidebysig instance bool 
          SignalAndWait(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 02 08
  {
    .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2079
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::SignalAndWait

  .method /*0600000E*/ public hidebysig instance bool 
          SignalAndWait(int32 millisecondsTimeout,
                        valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 02 08 11 61
  {
    .custom /*0C000036:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x207c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::SignalAndWait

  .method /*0600000F*/ public hidebysig instance void 
          SignalAndWait(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 01 01 11 61
  {
    .custom /*0C000037:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x207f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Barrier::SignalAndWait

  .method /*06000010*/ public hidebysig instance bool 
          SignalAndWait(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 02 11 65
  {
    .custom /*0C000038:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2081
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::SignalAndWait

  .method /*06000011*/ public hidebysig instance bool 
          SignalAndWait(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
                        valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 02 11 65 11 61
  {
    .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2084
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Barrier::SignalAndWait

  .property /*17000001*/ instance callconv(8) int64
          CurrentPhaseNumber()
  {
    .get instance int64 System.Threading.Barrier/*02000002*/::get_CurrentPhaseNumber() /* 06000003 */
  } // end of property Barrier::CurrentPhaseNumber
  .property /*17000002*/ instance callconv(8) int32
          ParticipantCount()
  {
    .get instance int32 System.Threading.Barrier/*02000002*/::get_ParticipantCount() /* 06000004 */
  } // end of property Barrier::ParticipantCount
  .property /*17000003*/ instance callconv(8) int32
          ParticipantsRemaining()
  {
    .get instance int32 System.Threading.Barrier/*02000002*/::get_ParticipantsRemaining() /* 06000005 */
  } // end of property Barrier::ParticipantsRemaining
} // end of class System.Threading.Barrier

.class /*02000003*/ public auto ansi beforefieldinit System.Threading.BarrierPostPhaseException
       extends [System.Runtime/*23000001*/]System.Exception/*0100001B*/
{
  .custom /*0C000015:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000016:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000012*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2087
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BarrierPostPhaseException::.ctor

  .method /*06000013*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ innerException) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x208f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BarrierPostPhaseException::.ctor

  .method /*06000014*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001F*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000020*/ context) cil managed
  // SIG: 20 02 01 12 7D 11 80 81
  {
    .custom /*0C000042:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000043:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C000044:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2097
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BarrierPostPhaseException::.ctor

  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x209f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BarrierPostPhaseException::.ctor

  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 6D
  {
    // Method begins at RVA 0x20a7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BarrierPostPhaseException::.ctor

} // end of class System.Threading.BarrierPostPhaseException

.class /*02000004*/ public auto ansi beforefieldinit System.Threading.CountdownEvent
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCount) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20af
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::.ctor

  .method /*06000018*/ public hidebysig specialname 
          instance int32  get_CurrentCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::get_CurrentCount

  .method /*06000019*/ public hidebysig specialname 
          instance int32  get_InitialCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::get_InitialCount

  .method /*0600001A*/ public hidebysig specialname 
          instance bool  get_IsSet() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::get_IsSet

  .method /*0600001B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ 
          get_WaitHandle() cil managed
  // SIG: 20 00 12 80 85
  {
    .custom /*0C00004C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::get_WaitHandle

  .method /*0600001C*/ public hidebysig instance void 
          AddCount() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::AddCount

  .method /*0600001D*/ public hidebysig instance void 
          AddCount(int32 signalCount) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::AddCount

  .method /*0600001E*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::Dispose

  .method /*0600001F*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20c9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::Dispose

  .method /*06000020*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20cb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::Reset

  .method /*06000021*/ public hidebysig instance void 
          Reset(int32 count) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::Reset

  .method /*06000022*/ public hidebysig instance bool 
          Signal() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::Signal

  .method /*06000023*/ public hidebysig instance bool 
          Signal(int32 signalCount) cil managed
  // SIG: 20 01 02 08
  {
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::Signal

  .method /*06000024*/ public hidebysig instance bool 
          TryAddCount() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::TryAddCount

  .method /*06000025*/ public hidebysig instance bool 
          TryAddCount(int32 signalCount) cil managed
  // SIG: 20 01 02 08
  {
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::TryAddCount

  .method /*06000026*/ public hidebysig instance void 
          Wait() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00005F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x20db
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::Wait

  .method /*06000027*/ public hidebysig instance bool 
          Wait(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 02 08
  {
    .custom /*0C000061:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::Wait

  .method /*06000028*/ public hidebysig instance bool 
          Wait(int32 millisecondsTimeout,
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 02 08 11 61
  {
    .custom /*0C000062:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::Wait

  .method /*06000029*/ public hidebysig instance void 
          Wait(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 01 01 11 61
  {
    .custom /*0C000064:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x20e3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CountdownEvent::Wait

  .method /*0600002A*/ public hidebysig instance bool 
          Wait(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 02 11 65
  {
    .custom /*0C000065:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::Wait

  .method /*0600002B*/ public hidebysig instance bool 
          Wait(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 02 11 65 11 61
  {
    .custom /*0C000066:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CountdownEvent::Wait

  .property /*17000004*/ instance callconv(8) int32
          CurrentCount()
  {
    .get instance int32 System.Threading.CountdownEvent/*02000004*/::get_CurrentCount() /* 06000018 */
  } // end of property CountdownEvent::CurrentCount
  .property /*17000005*/ instance callconv(8) int32
          InitialCount()
  {
    .get instance int32 System.Threading.CountdownEvent/*02000004*/::get_InitialCount() /* 06000019 */
  } // end of property CountdownEvent::InitialCount
  .property /*17000006*/ instance callconv(8) bool
          IsSet()
  {
    .get instance bool System.Threading.CountdownEvent/*02000004*/::get_IsSet() /* 0600001A */
  } // end of property CountdownEvent::IsSet
  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/
          WaitHandle()
  {
    .custom /*0C000024:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ System.Threading.CountdownEvent/*02000004*/::get_WaitHandle() /* 0600001B */
  } // end of property CountdownEvent::WaitHandle
} // end of class System.Threading.CountdownEvent

.class /*02000005*/ public auto ansi beforefieldinit System.Threading.HostExecutionContext
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00001C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000018:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*0600002C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20eb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HostExecutionContext::.ctor

  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object state) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20f3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HostExecutionContext::.ctor

  .method /*0600002E*/ famorassem hidebysig specialname 
          instance object  get_State() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostExecutionContext::get_State

  .method /*0600002F*/ famorassem hidebysig specialname 
          instance void  set_State(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20fe
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostExecutionContext::set_State

  .method /*06000030*/ public hidebysig newslot virtual 
          instance class System.Threading.HostExecutionContext/*02000005*/ 
          CreateCopy() cil managed
  // SIG: 20 00 12 14
  {
    .custom /*0C000067:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostExecutionContext::CreateCopy

  .method /*06000031*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2103
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostExecutionContext::Dispose

  .method /*06000032*/ public hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2105
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostExecutionContext::Dispose

  .property /*17000008*/ instance callconv(8) object
          State()
  {
    .get instance object System.Threading.HostExecutionContext/*02000005*/::get_State() /* 0600002E */
    .set instance void System.Threading.HostExecutionContext/*02000005*/::set_State(object) /* 0600002F */
  } // end of property HostExecutionContext::State
} // end of class System.Threading.HostExecutionContext

.class /*02000006*/ public auto ansi beforefieldinit System.Threading.HostExecutionContextManager
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000020:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000033*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2107
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HostExecutionContextManager::.ctor

  .method /*06000034*/ public hidebysig newslot virtual 
          instance class System.Threading.HostExecutionContext/*02000005*/ 
          Capture() cil managed
  // SIG: 20 00 12 14
  {
    .custom /*0C000068:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostExecutionContextManager::Capture

  .method /*06000035*/ public hidebysig newslot virtual 
          instance void  Revert(object previousState) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2112
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostExecutionContextManager::Revert

  .method /*06000036*/ public hidebysig newslot virtual 
          instance object  SetHostExecutionContext(class System.Threading.HostExecutionContext/*02000005*/ hostExecutionContext) cil managed
  // SIG: 20 01 1C 12 14
  {
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostExecutionContextManager::SetHostExecutionContext

} // end of class System.Threading.HostExecutionContextManager

.class /*02000007*/ public sequential ansi sealed beforefieldinit System.Threading.LockCookie
       extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000023*/<valuetype System.Threading.LockCookie/*02000007*/>/*1B000001*/
{
  .field /*04000001*/ private int32 _dummyPrimitive
  .method /*06000037*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000069:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000026*/ 
    .custom /*0C000060:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000024*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LockCookie::Equals

  .method /*06000038*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Threading.LockCookie/*02000007*/ obj) cil managed
  // SIG: 20 01 02 11 1C
  {
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LockCookie::Equals

  .method /*06000039*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LockCookie::GetHashCode

  .method /*0600003A*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Threading.LockCookie/*02000007*/ a,
                            valuetype System.Threading.LockCookie/*02000007*/ b) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x2120
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LockCookie::op_Equality

  .method /*0600003B*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Threading.LockCookie/*02000007*/ a,
                              valuetype System.Threading.LockCookie/*02000007*/ b) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x2123
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LockCookie::op_Inequality

} // end of class System.Threading.LockCookie

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Threading.ReaderWriterLock
       extends [System.Runtime/*23000001*/]System.Runtime.ConstrainedExecution.CriticalFinalizerObject/*01000025*/
{
  .method /*0600003C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2126
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000024       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.ConstrainedExecution.CriticalFinalizerObject/*01000025*/::.ctor() /* 0A000024 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::.ctor

  .method /*0600003D*/ public hidebysig specialname 
          instance bool  get_IsReaderLockHeld() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLock::get_IsReaderLockHeld

  .method /*0600003E*/ public hidebysig specialname 
          instance bool  get_IsWriterLockHeld() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLock::get_IsWriterLockHeld

  .method /*0600003F*/ public hidebysig specialname 
          instance int32  get_WriterSeqNum() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLock::get_WriterSeqNum

  .method /*06000040*/ public hidebysig instance void 
          AcquireReaderLock(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C00006A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2137
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::AcquireReaderLock

  .method /*06000041*/ public hidebysig instance void 
          AcquireReaderLock(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 01 11 65
  {
    .custom /*0C00006B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2139
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::AcquireReaderLock

  .method /*06000042*/ public hidebysig instance void 
          AcquireWriterLock(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x213b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::AcquireWriterLock

  .method /*06000043*/ public hidebysig instance void 
          AcquireWriterLock(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 01 11 65
  {
    // Method begins at RVA 0x213d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::AcquireWriterLock

  .method /*06000044*/ public hidebysig instance bool 
          AnyWritersSince(int32 seqNum) cil managed
  // SIG: 20 01 02 08
  {
    // Method begins at RVA 0x213f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLock::AnyWritersSince

  .method /*06000045*/ public hidebysig instance void 
          DowngradeFromWriterLock(valuetype System.Threading.LockCookie/*02000007*/& lockCookie) cil managed
  // SIG: 20 01 01 10 11 1C
  {
    // Method begins at RVA 0x2142
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::DowngradeFromWriterLock

  .method /*06000046*/ public hidebysig instance valuetype System.Threading.LockCookie/*02000007*/ 
          ReleaseLock() cil managed
  // SIG: 20 00 11 1C
  {
    // Method begins at RVA 0x2144
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLock::ReleaseLock

  .method /*06000047*/ public hidebysig instance void 
          ReleaseReaderLock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2147
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::ReleaseReaderLock

  .method /*06000048*/ public hidebysig instance void 
          ReleaseWriterLock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2149
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::ReleaseWriterLock

  .method /*06000049*/ public hidebysig instance void 
          RestoreLock(valuetype System.Threading.LockCookie/*02000007*/& lockCookie) cil managed
  // SIG: 20 01 01 10 11 1C
  {
    .custom /*0C00006D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x214b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLock::RestoreLock

  .method /*0600004A*/ public hidebysig instance valuetype System.Threading.LockCookie/*02000007*/ 
          UpgradeToWriterLock(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 11 1C 08
  {
    .custom /*0C00006F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLock::UpgradeToWriterLock

  .method /*0600004B*/ public hidebysig instance valuetype System.Threading.LockCookie/*02000007*/ 
          UpgradeToWriterLock(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 11 1C 11 65
  {
    .custom /*0C000071:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLock::UpgradeToWriterLock

  .property /*17000009*/ instance callconv(8) bool
          IsReaderLockHeld()
  {
    .get instance bool System.Threading.ReaderWriterLock/*02000008*/::get_IsReaderLockHeld() /* 0600003D */
  } // end of property ReaderWriterLock::IsReaderLockHeld
  .property /*1700000A*/ instance callconv(8) bool
          IsWriterLockHeld()
  {
    .get instance bool System.Threading.ReaderWriterLock/*02000008*/::get_IsWriterLockHeld() /* 0600003E */
  } // end of property ReaderWriterLock::IsWriterLockHeld
  .property /*1700000B*/ instance callconv(8) int32
          WriterSeqNum()
  {
    .get instance int32 System.Threading.ReaderWriterLock/*02000008*/::get_WriterSeqNum() /* 0600003F */
  } // end of property ReaderWriterLock::WriterSeqNum
} // end of class System.Threading.ReaderWriterLock

.class /*02000009*/ public auto ansi beforefieldinit System.Threading.AbandonedMutexException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000026*/
{
  .custom /*0C000029:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*0600004C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2153
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AbandonedMutexException::.ctor

  .method /*0600004D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 location,
                               class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ handle) cil managed
  // SIG: 20 02 01 08 12 80 85
  {
    // Method begins at RVA 0x215b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AbandonedMutexException::.ctor

  .method /*0600004E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001F*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000020*/ context) cil managed
  // SIG: 20 02 01 12 7D 11 80 81
  {
    .custom /*0C000072:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000073:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C000074:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2163
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AbandonedMutexException::.ctor

  .method /*0600004F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x216b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AbandonedMutexException::.ctor

  .method /*06000050*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ inner) cil managed
  // SIG: 20 02 01 0E 12 6D
  {
    // Method begins at RVA 0x2173
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AbandonedMutexException::.ctor

  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ inner,
                               int32 location,
                               class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ handle) cil managed
  // SIG: 20 04 01 0E 12 6D 08 12 80 85
  {
    // Method begins at RVA 0x217b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AbandonedMutexException::.ctor

  .method /*06000052*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               int32 location,
                               class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ handle) cil managed
  // SIG: 20 03 01 0E 08 12 80 85
  {
    // Method begins at RVA 0x2183
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AbandonedMutexException::.ctor

  .method /*06000053*/ public hidebysig specialname 
          instance class System.Threading.Mutex/*02000019*/ 
          get_Mutex() cil managed
  // SIG: 20 00 12 64
  {
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AbandonedMutexException::get_Mutex

  .method /*06000054*/ public hidebysig specialname 
          instance int32  get_MutexIndex() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x218e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AbandonedMutexException::get_MutexIndex

  .property /*1700000C*/ instance callconv(8) class System.Threading.Mutex/*02000019*/
          Mutex()
  {
    .get instance class System.Threading.Mutex/*02000019*/ System.Threading.AbandonedMutexException/*02000009*/::get_Mutex() /* 06000053 */
  } // end of property AbandonedMutexException::Mutex
  .property /*1700000D*/ instance callconv(8) int32
          MutexIndex()
  {
    .get instance int32 System.Threading.AbandonedMutexException/*02000009*/::get_MutexIndex() /* 06000054 */
  } // end of property AbandonedMutexException::MutexIndex
} // end of class System.Threading.AbandonedMutexException

.class /*0200000A*/ public sequential ansi sealed beforefieldinit System.Threading.AsyncFlowControl
       extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000023*/<valuetype System.Threading.AsyncFlowControl/*0200000A*/>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .field /*04000002*/ private object _dummy
  .field /*04000003*/ private int32 _dummyPrimitive
  .method /*06000055*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2191
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsyncFlowControl::Dispose

  .method /*06000056*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000076:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000043*/ 
    .custom /*0C00006C:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000024*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2193
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncFlowControl::Equals

  .method /*06000057*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Threading.AsyncFlowControl/*0200000A*/ obj) cil managed
  // SIG: 20 01 02 11 28
  {
    // Method begins at RVA 0x2196
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncFlowControl::Equals

  .method /*06000058*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2199
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncFlowControl::GetHashCode

  .method /*06000059*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Threading.AsyncFlowControl/*0200000A*/ a,
                            valuetype System.Threading.AsyncFlowControl/*0200000A*/ b) cil managed
  // SIG: 00 02 02 11 28 11 28
  {
    // Method begins at RVA 0x219c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncFlowControl::op_Equality

  .method /*0600005A*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Threading.AsyncFlowControl/*0200000A*/ a,
                              valuetype System.Threading.AsyncFlowControl/*0200000A*/ b) cil managed
  // SIG: 00 02 02 11 28 11 28
  {
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncFlowControl::op_Inequality

  .method /*0600005B*/ public hidebysig instance void 
          Undo() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21a2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsyncFlowControl::Undo

} // end of class System.Threading.AsyncFlowControl

.class /*0200000B*/ public sequential ansi sealed beforefieldinit System.Threading.AsyncLocalValueChangedArgs`1<T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
{
  .custom /*0C00002B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000027*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
  .field /*04000004*/ private initonly !T _PreviousValue_k__BackingField
  .field /*04000005*/ private initonly !T _CurrentValue_k__BackingField
  .field /*04000006*/ private initonly int32 _dummyPrimitive
  .method /*0600005C*/ public hidebysig specialname 
          instance !T  get_CurrentValue() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x21a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncLocalValueChangedArgs`1::get_CurrentValue

  .method /*0600005D*/ public hidebysig specialname 
          instance !T  get_PreviousValue() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x21a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncLocalValueChangedArgs`1::get_PreviousValue

  .method /*0600005E*/ public hidebysig specialname 
          instance bool  get_ThreadContextChanged() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncLocalValueChangedArgs`1::get_ThreadContextChanged

  .property /*1700000E*/ instance callconv(8) !T
          CurrentValue()
  {
    .get instance !T System.Threading.AsyncLocalValueChangedArgs`1/*0200000B*/::get_CurrentValue() /* 0600005C */
  } // end of property AsyncLocalValueChangedArgs`1::CurrentValue
  .property /*1700000F*/ instance callconv(8) !T
          PreviousValue()
  {
    .get instance !T System.Threading.AsyncLocalValueChangedArgs`1/*0200000B*/::get_PreviousValue() /* 0600005D */
  } // end of property AsyncLocalValueChangedArgs`1::PreviousValue
  .property /*17000010*/ instance callconv(8) bool
          ThreadContextChanged()
  {
    .get instance bool System.Threading.AsyncLocalValueChangedArgs`1/*0200000B*/::get_ThreadContextChanged() /* 0600005E */
  } // end of property AsyncLocalValueChangedArgs`1::ThreadContextChanged
} // end of class System.Threading.AsyncLocalValueChangedArgs`1

.class /*0200000C*/ public auto ansi sealed beforefieldinit System.Threading.AsyncLocal`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C000031:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .param type T /*2A000002*/ 
    .custom /*0C000014:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
  .method /*0600005F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsyncLocal`1::.ctor

  .method /*06000060*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Action`1/*01000016*/<valuetype System.Threading.AsyncLocalValueChangedArgs`1/*0200000B*/<!T>> valueChangedHandler) cil managed
  // SIG: 20 01 01 15 12 59 01 15 11 2C 01 13 00
  {
    .param [1]/*08000049*/ 
    .custom /*0C00006E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000012 */ = ( 01 00 03 00 00 00 02 00 01 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsyncLocal`1::.ctor

  .method /*06000061*/ public hidebysig specialname 
          instance !T  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    .param [0]/*0800004A*/ 
    .custom /*0C000070:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullAttribute/*01000028*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncLocal`1::get_Value

  .method /*06000062*/ public hidebysig specialname 
          instance void  set_Value(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x21c0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsyncLocal`1::set_Value

  .property /*17000011*/ instance callconv(8) !T
          Value()
  {
    .get instance !T System.Threading.AsyncLocal`1/*0200000C*/::get_Value() /* 06000061 */
    .set instance void System.Threading.AsyncLocal`1/*0200000C*/::set_Value(!T) /* 06000062 */
  } // end of property AsyncLocal`1::Value
} // end of class System.Threading.AsyncLocal`1

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.Threading.AutoResetEvent
       extends System.Threading.EventWaitHandle/*02000010*/
{
  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initialState) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21c2
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000068       */ call       instance void System.Threading.EventWaitHandle/*02000010*/::.ctor(bool,
                                                                                                                         valuetype System.Threading.EventResetMode/*0200000F*/) /* 06000068 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AutoResetEvent::.ctor

} // end of class System.Threading.AutoResetEvent

.class /*0200000E*/ public auto ansi sealed System.Threading.ContextCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000029*/
{
  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ContextCallback::.ctor

  .method /*06000065*/ public hidebysig newslot virtual 
          instance void  Invoke(object state) runtime managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000079:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method ContextCallback::Invoke

  .method /*06000066*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginInvoke(object state,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 80 A9 1C 12 80 AD 1C
  {
    .param [1]/*08000050*/ 
    .custom /*0C000075:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
  } // end of method ContextCallback::BeginInvoke

  .method /*06000067*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ result) runtime managed
  // SIG: 20 01 01 12 80 A9
  {
  } // end of method ContextCallback::EndInvoke

} // end of class System.Threading.ContextCallback

.class /*0200000F*/ public auto ansi sealed System.Threading.EventResetMode
       extends [System.Runtime/*23000001*/]System.Enum/*0100002C*/
{
  .field /*04000007*/ public specialname rtspecialname int32 value__
  .field /*04000008*/ public static literal valuetype System.Threading.EventResetMode/*0200000F*/ AutoReset = int32(0x00000000)
  .field /*04000009*/ public static literal valuetype System.Threading.EventResetMode/*0200000F*/ ManualReset = int32(0x00000001)
} // end of class System.Threading.EventResetMode

.class /*02000010*/ public auto ansi beforefieldinit System.Threading.EventWaitHandle
       extends [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/
{
  .custom /*0C000039:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000068*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initialState,
                               valuetype System.Threading.EventResetMode/*0200000F*/ mode) cil managed
  // SIG: 20 02 01 02 11 3C
  {
    // Method begins at RVA 0x21cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EventWaitHandle::.ctor

  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initialState,
                               valuetype System.Threading.EventResetMode/*0200000F*/ mode,
                               string name) cil managed
  // SIG: 20 03 01 02 11 3C 0E
  {
    .custom /*0C00007B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EventWaitHandle::.ctor

  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initialState,
                               valuetype System.Threading.EventResetMode/*0200000F*/ mode,
                               string name,
                               [out] bool& createdNew) cil managed
  // SIG: 20 04 01 02 11 3C 0E 10 02
  {
    .custom /*0C00007C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21dc
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method EventWaitHandle::.ctor

  .method /*0600006B*/ public hidebysig static 
          class System.Threading.EventWaitHandle/*02000010*/ 
          OpenExisting(string name) cil managed
  // SIG: 00 01 12 40 0E
  {
    .custom /*0C00007D:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100002D*/::.ctor(string) /* 0A00001B */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventWaitHandle::OpenExisting

  .method /*0600006C*/ public hidebysig instance bool 
          Reset() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventWaitHandle::Reset

  .method /*0600006D*/ public hidebysig instance bool 
          Set() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventWaitHandle::Set

  .method /*0600006E*/ public hidebysig static 
          bool  TryOpenExisting(string name,
                                [out] class System.Threading.EventWaitHandle/*02000010*/& result) cil managed
  // SIG: 00 02 02 0E 10 12 40
  {
    .custom /*0C00007E:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100002D*/::.ctor(string) /* 0A00001B */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .param [2]/*0800005F*/ 
    .custom /*0C000077:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000078:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000024*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventWaitHandle::TryOpenExisting

} // end of class System.Threading.EventWaitHandle

.class /*02000011*/ public auto ansi sealed beforefieldinit System.Threading.ExecutionContext
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/,
                  [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002E*/
{
  .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000023:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002E*/
  .custom /*0C000027:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*0600006F*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21f1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExecutionContext::.ctor

  .method /*06000070*/ public hidebysig static 
          class System.Threading.ExecutionContext/*02000011*/ 
          Capture() cil managed
  // SIG: 00 00 12 44
  {
    .custom /*0C00007F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExecutionContext::Capture

  .method /*06000071*/ public hidebysig instance class System.Threading.ExecutionContext/*02000011*/ 
          CreateCopy() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x21fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExecutionContext::CreateCopy

  .method /*06000072*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExecutionContext::Dispose

  .method /*06000073*/ public hidebysig newslot virtual final 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001F*/ info,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000020*/ context) cil managed
  // SIG: 20 02 01 12 7D 11 80 81
  {
    .custom /*0C000080:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C000081:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2201
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExecutionContext::GetObjectData

  .method /*06000074*/ public hidebysig static 
          bool  IsFlowSuppressed() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExecutionContext::IsFlowSuppressed

  .method /*06000075*/ public hidebysig static 
          void  Restore(class System.Threading.ExecutionContext/*02000011*/ executionContext) cil managed
  // SIG: 00 01 01 12 44
  {
    // Method begins at RVA 0x2206
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExecutionContext::Restore

  .method /*06000076*/ public hidebysig static 
          void  RestoreFlow() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2208
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExecutionContext::RestoreFlow

  .method /*06000077*/ public hidebysig static 
          void  Run(class System.Threading.ExecutionContext/*02000011*/ executionContext,
                    class System.Threading.ContextCallback/*0200000E*/ callback,
                    object state) cil managed
  // SIG: 00 03 01 12 44 12 38 1C
  {
    .param [3]/*08000065*/ 
    .custom /*0C00007A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExecutionContext::Run

  .method /*06000078*/ public hidebysig static 
          valuetype System.Threading.AsyncFlowControl/*0200000A*/ 
          SuppressFlow() cil managed
  // SIG: 00 00 11 28
  {
    // Method begins at RVA 0x220c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExecutionContext::SuppressFlow

} // end of class System.Threading.ExecutionContext

.class /*02000012*/ public abstract auto ansi sealed beforefieldinit System.Threading.Interlocked
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C00003E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000079*/ public hidebysig static 
          int32  Add(int32& location1,
                     int32 'value') cil managed
  // SIG: 00 02 08 10 08 08
  {
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Add

  .method /*0600007A*/ public hidebysig static 
          int64  Add(int64& location1,
                     int64 'value') cil managed
  // SIG: 00 02 0A 10 0A 0A
  {
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Add

  .method /*0600007B*/ public hidebysig static 
          uint32  Add(uint32& location1,
                      uint32 'value') cil managed
  // SIG: 00 02 09 10 09 09
  {
    .custom /*0C000082:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2215
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Add

  .method /*0600007C*/ public hidebysig static 
          uint64  Add(uint64& location1,
                      uint64 'value') cil managed
  // SIG: 00 02 0B 10 0B 0B
  {
    .custom /*0C000083:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Add

  .method /*0600007D*/ public hidebysig static 
          int32  And(int32& location1,
                     int32 'value') cil managed
  // SIG: 00 02 08 10 08 08
  {
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::And

  .method /*0600007E*/ public hidebysig static 
          int64  And(int64& location1,
                     int64 'value') cil managed
  // SIG: 00 02 0A 10 0A 0A
  {
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::And

  .method /*0600007F*/ public hidebysig static 
          uint32  And(uint32& location1,
                      uint32 'value') cil managed
  // SIG: 00 02 09 10 09 09
  {
    .custom /*0C000084:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::And

  .method /*06000080*/ public hidebysig static 
          uint64  And(uint64& location1,
                      uint64 'value') cil managed
  // SIG: 00 02 0B 10 0B 0B
  {
    .custom /*0C000085:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::And

  .method /*06000081*/ public hidebysig static 
          float64  CompareExchange(float64& location1,
                                   float64 'value',
                                   float64 comparand) cil managed
  // SIG: 00 03 0D 10 0D 0D 0D
  {
    // Method begins at RVA 0x2227
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*06000082*/ public hidebysig static 
          int32  CompareExchange(int32& location1,
                                 int32 'value',
                                 int32 comparand) cil managed
  // SIG: 00 03 08 10 08 08 08
  {
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*06000083*/ public hidebysig static 
          int64  CompareExchange(int64& location1,
                                 int64 'value',
                                 int64 comparand) cil managed
  // SIG: 00 03 0A 10 0A 0A 0A
  {
    // Method begins at RVA 0x222d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*06000084*/ public hidebysig static 
          native int  CompareExchange(native int& location1,
                                      native int 'value',
                                      native int comparand) cil managed
  // SIG: 00 03 18 10 18 18 18
  {
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*06000085*/ public hidebysig static 
          native uint  CompareExchange(native uint& location1,
                                       native uint 'value',
                                       native uint comparand) cil managed
  // SIG: 00 03 19 10 19 19 19
  {
    .custom /*0C000086:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2233
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*06000086*/ public hidebysig static 
          object  CompareExchange(object& location1,
                                  object 'value',
                                  object comparand) cil managed
  // SIG: 00 03 1C 10 1C 1C 1C
  {
    .custom /*0C000088:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000085*/ 
    .custom /*0C000087:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 09 6C 6F 63 61 74 69 6F 6E 31 00 00 )       // ...location1..
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*06000087*/ public hidebysig static 
          float32  CompareExchange(float32& location1,
                                   float32 'value',
                                   float32 comparand) cil managed
  // SIG: 00 03 0C 10 0C 0C 0C
  {
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*06000088*/ public hidebysig static 
          uint32  CompareExchange(uint32& location1,
                                  uint32 'value',
                                  uint32 comparand) cil managed
  // SIG: 00 03 09 10 09 09 09
  {
    .custom /*0C000089:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*06000089*/ public hidebysig static 
          uint64  CompareExchange(uint64& location1,
                                  uint64 'value',
                                  uint64 comparand) cil managed
  // SIG: 00 03 0B 10 0B 0B 0B
  {
    .custom /*0C00008A:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*0600008A*/ public hidebysig static 
          !!T  CompareExchange<class T>(!!T& location1,
                                        !!T 'value',
                                        !!T comparand) cil managed
  // SIG: 10 01 03 1E 00 10 1E 00 1E 00 1E 00
  {
    .param type T /*2A000004*/ 
      .custom /*0C00001A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000092*/ 
    .custom /*0C00008D:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 09 6C 6F 63 61 74 69 6F 6E 31 00 00 )       // ...location1..
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::CompareExchange

  .method /*0600008B*/ public hidebysig static 
          int32  Decrement(int32& location) cil managed
  // SIG: 00 01 08 10 08
  {
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Decrement

  .method /*0600008C*/ public hidebysig static 
          int64  Decrement(int64& location) cil managed
  // SIG: 00 01 0A 10 0A
  {
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Decrement

  .method /*0600008D*/ public hidebysig static 
          uint32  Decrement(uint32& location) cil managed
  // SIG: 00 01 09 10 09
  {
    .custom /*0C00008B:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Decrement

  .method /*0600008E*/ public hidebysig static 
          uint64  Decrement(uint64& location) cil managed
  // SIG: 00 01 0B 10 0B
  {
    .custom /*0C00008C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Decrement

  .method /*0600008F*/ public hidebysig static 
          float64  Exchange(float64& location1,
                            float64 'value') cil managed
  // SIG: 00 02 0D 10 0D 0D
  {
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000090*/ public hidebysig static 
          int32  Exchange(int32& location1,
                          int32 'value') cil managed
  // SIG: 00 02 08 10 08 08
  {
    // Method begins at RVA 0x2254
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000091*/ public hidebysig static 
          int64  Exchange(int64& location1,
                          int64 'value') cil managed
  // SIG: 00 02 0A 10 0A 0A
  {
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000092*/ public hidebysig static 
          native int  Exchange(native int& location1,
                               native int 'value') cil managed
  // SIG: 00 02 18 10 18 18
  {
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000093*/ public hidebysig static 
          native uint  Exchange(native uint& location1,
                                native uint 'value') cil managed
  // SIG: 00 02 19 10 19 19
  {
    .custom /*0C00008E:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000094*/ public hidebysig static 
          object  Exchange(object& location1,
                           object 'value') cil managed
  // SIG: 00 02 1C 10 1C 1C
  {
    .custom /*0C00008F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000A4*/ 
    .custom /*0C000097:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 09 6C 6F 63 61 74 69 6F 6E 31 00 00 )       // ...location1..
    .param [1]/*080000A5*/ 
    .custom /*0C000098:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x2260
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000095*/ public hidebysig static 
          float32  Exchange(float32& location1,
                            float32 'value') cil managed
  // SIG: 00 02 0C 10 0C 0C
  {
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000096*/ public hidebysig static 
          uint32  Exchange(uint32& location1,
                           uint32 'value') cil managed
  // SIG: 00 02 09 10 09 09
  {
    .custom /*0C000090:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000097*/ public hidebysig static 
          uint64  Exchange(uint64& location1,
                           uint64 'value') cil managed
  // SIG: 00 02 0B 10 0B 0B
  {
    .custom /*0C000091:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000098*/ public hidebysig static 
          !!T  Exchange<class T>(!!T& location1,
                                 !!T 'value') cil managed
  // SIG: 10 01 02 1E 00 10 1E 00 1E 00
  {
    .param type T /*2A000005*/ 
      .custom /*0C00001D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000AD*/ 
    .custom /*0C00009C:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 09 6C 6F 63 61 74 69 6F 6E 31 00 00 )       // ...location1..
    .param [1]/*080000AE*/ 
    .custom /*0C00009D:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Exchange

  .method /*06000099*/ public hidebysig static 
          int32  Increment(int32& location) cil managed
  // SIG: 00 01 08 10 08
  {
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Increment

  .method /*0600009A*/ public hidebysig static 
          int64  Increment(int64& location) cil managed
  // SIG: 00 01 0A 10 0A
  {
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Increment

  .method /*0600009B*/ public hidebysig static 
          uint32  Increment(uint32& location) cil managed
  // SIG: 00 01 09 10 09
  {
    .custom /*0C000092:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Increment

  .method /*0600009C*/ public hidebysig static 
          uint64  Increment(uint64& location) cil managed
  // SIG: 00 01 0B 10 0B
  {
    .custom /*0C000093:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Increment

  .method /*0600009D*/ public hidebysig static 
          void  MemoryBarrier() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x227b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Interlocked::MemoryBarrier

  .method /*0600009E*/ public hidebysig static 
          void  MemoryBarrierProcessWide() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x227d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Interlocked::MemoryBarrierProcessWide

  .method /*0600009F*/ public hidebysig static 
          int32  Or(int32& location1,
                    int32 'value') cil managed
  // SIG: 00 02 08 10 08 08
  {
    // Method begins at RVA 0x227f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Or

  .method /*060000A0*/ public hidebysig static 
          int64  Or(int64& location1,
                    int64 'value') cil managed
  // SIG: 00 02 0A 10 0A 0A
  {
    // Method begins at RVA 0x2282
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Or

  .method /*060000A1*/ public hidebysig static 
          uint32  Or(uint32& location1,
                     uint32 'value') cil managed
  // SIG: 00 02 09 10 09 09
  {
    .custom /*0C000094:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2285
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Or

  .method /*060000A2*/ public hidebysig static 
          uint64  Or(uint64& location1,
                     uint64 'value') cil managed
  // SIG: 00 02 0B 10 0B 0B
  {
    .custom /*0C000095:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2288
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Or

  .method /*060000A3*/ public hidebysig static 
          int64  Read([in] int64& location) cil managed
  // SIG: 00 01 0A 10 0A
  {
    .param [1]/*080000BC*/ 
    .custom /*0C0000A3:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x228b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Read

  .method /*060000A4*/ public hidebysig static 
          uint64  Read([in] uint64& location) cil managed
  // SIG: 00 01 0B 10 0B
  {
    .custom /*0C000096:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000BD*/ 
    .custom /*0C0000A5:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Interlocked::Read

} // end of class System.Threading.Interlocked

.class /*02000013*/ public abstract auto ansi sealed beforefieldinit System.Threading.LazyInitializer
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C000040:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*060000A5*/ public hidebysig static 
          !!T  EnsureInitialized<class T>(!!T& target) cil managed
  // SIG: 10 01 01 1E 00 10 1E 00
  {
    .param type T /*2A000006*/ 
      .custom /*0C000021:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000032*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000031*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*080000BE*/ 
    .custom /*0C0000A7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000A8:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullAttribute/*01000033*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2291
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LazyInitializer::EnsureInitialized

  .method /*060000A6*/ public hidebysig static 
          !!T  EnsureInitialized<T>(!!T& target,
                                    bool& initialized,
                                    object& syncLock) cil managed
  // SIG: 10 01 03 1E 00 10 1E 00 10 02 10 1C
  {
    .param type T /*2A000007*/ 
      .custom /*0C000025:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
      .custom /*0C000026:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000032*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000031*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*080000BF*/ 
    .custom /*0C0000A9:0A000020*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000034*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [3]/*080000C1*/ 
    .custom /*0C0000AA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000AB:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 08 73 79 6E 63 4C 6F 63 6B 00 00 )          // ...syncLock..
    // Method begins at RVA 0x2294
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LazyInitializer::EnsureInitialized

  .method /*060000A7*/ public hidebysig static 
          !!T  EnsureInitialized<T>(!!T& target,
                                    bool& initialized,
                                    object& syncLock,
                                    class [System.Runtime/*23000001*/]System.Func`1/*01000035*/<!!T> valueFactory) cil managed
  // SIG: 10 01 04 1E 00 10 1E 00 10 02 10 1C 15 12 80 D5 01 1E 00
  {
    .param type T /*2A000008*/ 
      .custom /*0C000028:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000C2*/ 
    .custom /*0C0000AC:0A000020*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000034*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [3]/*080000C4*/ 
    .custom /*0C0000AD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000AE:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 08 73 79 6E 63 4C 6F 63 6B 00 00 )          // ...syncLock..
    // Method begins at RVA 0x2297
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LazyInitializer::EnsureInitialized

  .method /*060000A8*/ public hidebysig static 
          !!T  EnsureInitialized<class T>(!!T& target,
                                          class [System.Runtime/*23000001*/]System.Func`1/*01000035*/<!!T> valueFactory) cil managed
  // SIG: 10 01 02 1E 00 10 1E 00 15 12 80 D5 01 1E 00
  {
    .param [1]/*080000C6*/ 
    .custom /*0C0000AF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B0:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullAttribute/*01000033*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LazyInitializer::EnsureInitialized

  .method /*060000A9*/ public hidebysig static 
          !!T  EnsureInitialized<class T>(!!T& target,
                                          object& syncLock,
                                          class [System.Runtime/*23000001*/]System.Func`1/*01000035*/<!!T> valueFactory) cil managed
  // SIG: 10 01 03 1E 00 10 1E 00 10 1C 15 12 80 D5 01 1E 00
  {
    .param [1]/*080000C8*/ 
    .custom /*0C0000B1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B2:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullAttribute/*01000033*/::.ctor() /* 0A00001F */ = ( 01 00 00 00 ) 
    .param [2]/*080000C9*/ 
    .custom /*0C0000B3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B4:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 08 73 79 6E 63 4C 6F 63 6B 00 00 )          // ...syncLock..
    // Method begins at RVA 0x229d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LazyInitializer::EnsureInitialized

} // end of class System.Threading.LazyInitializer

.class /*02000014*/ public auto ansi beforefieldinit System.Threading.LockRecursionException
       extends [System.Runtime/*23000001*/]System.Exception/*0100001B*/
{
  .custom /*0C000045:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000046:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*060000AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22a0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LockRecursionException::.ctor

  .method /*060000AB*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001F*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000020*/ context) cil managed
  // SIG: 20 02 01 12 7D 11 80 81
  {
    .custom /*0C000099:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00009A:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C00009B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x22a8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LockRecursionException::.ctor

  .method /*060000AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22b0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LockRecursionException::.ctor

  .method /*060000AD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 6D
  {
    // Method begins at RVA 0x22b8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001B*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LockRecursionException::.ctor

} // end of class System.Threading.LockRecursionException

.class /*02000015*/ public auto ansi sealed System.Threading.LockRecursionPolicy
       extends [System.Runtime/*23000001*/]System.Enum/*0100002C*/
{
  .field /*0400000A*/ public specialname rtspecialname int32 value__
  .field /*0400000B*/ public static literal valuetype System.Threading.LockRecursionPolicy/*02000015*/ NoRecursion = int32(0x00000000)
  .field /*0400000C*/ public static literal valuetype System.Threading.LockRecursionPolicy/*02000015*/ SupportsRecursion = int32(0x00000001)
} // end of class System.Threading.LockRecursionPolicy

.class /*02000016*/ public auto ansi sealed beforefieldinit System.Threading.ManualResetEvent
       extends System.Threading.EventWaitHandle/*02000010*/
{
  .method /*060000AE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initialState) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22c0
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000068       */ call       instance void System.Threading.EventWaitHandle/*02000010*/::.ctor(bool,
                                                                                                                         valuetype System.Threading.EventResetMode/*0200000F*/) /* 06000068 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method ManualResetEvent::.ctor

} // end of class System.Threading.ManualResetEvent

.class /*02000017*/ public auto ansi beforefieldinit System.Threading.ManualResetEventSlim
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .method /*060000AF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22ca
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::.ctor

  .method /*060000B0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initialState) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22d2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::.ctor

  .method /*060000B1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initialState,
                               int32 spinCount) cil managed
  // SIG: 20 02 01 02 08
  {
    // Method begins at RVA 0x22da
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::.ctor

  .method /*060000B2*/ public hidebysig specialname 
          instance bool  get_IsSet() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManualResetEventSlim::get_IsSet

  .method /*060000B3*/ public hidebysig specialname 
          instance int32  get_SpinCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManualResetEventSlim::get_SpinCount

  .method /*060000B4*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ 
          get_WaitHandle() cil managed
  // SIG: 20 00 12 80 85
  {
    .custom /*0C00009E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManualResetEventSlim::get_WaitHandle

  .method /*060000B5*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22eb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::Dispose

  .method /*060000B6*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22ed
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::Dispose

  .method /*060000B7*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::Reset

  .method /*060000B8*/ public hidebysig instance void 
          Set() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22f1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::Set

  .method /*060000B9*/ public hidebysig instance void 
          Wait() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00009F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x22f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::Wait

  .method /*060000BA*/ public hidebysig instance bool 
          Wait(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 02 08
  {
    .custom /*0C0000A0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x22f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManualResetEventSlim::Wait

  .method /*060000BB*/ public hidebysig instance bool 
          Wait(int32 millisecondsTimeout,
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 02 08 11 61
  {
    .custom /*0C0000A1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x22f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManualResetEventSlim::Wait

  .method /*060000BC*/ public hidebysig instance void 
          Wait(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 01 01 11 61
  {
    .custom /*0C0000A2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x22fb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ManualResetEventSlim::Wait

  .method /*060000BD*/ public hidebysig instance bool 
          Wait(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 02 11 65
  {
    .custom /*0C0000A4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x22fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManualResetEventSlim::Wait

  .method /*060000BE*/ public hidebysig instance bool 
          Wait(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 02 11 65 11 61
  {
    .custom /*0C0000A6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2300
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManualResetEventSlim::Wait

  .property /*17000012*/ instance callconv(8) bool
          IsSet()
  {
    .get instance bool System.Threading.ManualResetEventSlim/*02000017*/::get_IsSet() /* 060000B2 */
  } // end of property ManualResetEventSlim::IsSet
  .property /*17000013*/ instance callconv(8) int32
          SpinCount()
  {
    .get instance int32 System.Threading.ManualResetEventSlim/*02000017*/::get_SpinCount() /* 060000B3 */
  } // end of property ManualResetEventSlim::SpinCount
  .property /*17000014*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/
          WaitHandle()
  {
    .custom /*0C000047:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ System.Threading.ManualResetEventSlim/*02000017*/::get_WaitHandle() /* 060000B4 */
  } // end of property ManualResetEventSlim::WaitHandle
} // end of class System.Threading.ManualResetEventSlim

.class /*02000018*/ public abstract auto ansi sealed beforefieldinit System.Threading.Monitor
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C000048:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000049:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*060000BF*/ public hidebysig specialname static 
          int64  get_LockContentionCount() cil managed
  // SIG: 00 00 0A
  {
    // Method begins at RVA 0x2303
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::get_LockContentionCount

  .method /*060000C0*/ public hidebysig static 
          void  Enter(object obj) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x2306
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Monitor::Enter

  .method /*060000C1*/ public hidebysig static 
          void  Enter(object obj,
                      bool& lockTaken) cil managed
  // SIG: 00 02 01 1C 10 02
  {
    // Method begins at RVA 0x2308
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Monitor::Enter

  .method /*060000C2*/ public hidebysig static 
          void  Exit(object obj) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x230a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Monitor::Exit

  .method /*060000C3*/ public hidebysig static 
          bool  IsEntered(object obj) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::IsEntered

  .method /*060000C4*/ public hidebysig static 
          void  Pulse(object obj) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x230f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Monitor::Pulse

  .method /*060000C5*/ public hidebysig static 
          void  PulseAll(object obj) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x2311
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Monitor::PulseAll

  .method /*060000C6*/ public hidebysig static 
          bool  TryEnter(object obj) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x2313
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::TryEnter

  .method /*060000C7*/ public hidebysig static 
          void  TryEnter(object obj,
                         bool& lockTaken) cil managed
  // SIG: 00 02 01 1C 10 02
  {
    // Method begins at RVA 0x2316
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Monitor::TryEnter

  .method /*060000C8*/ public hidebysig static 
          bool  TryEnter(object obj,
                         int32 millisecondsTimeout) cil managed
  // SIG: 00 02 02 1C 08
  {
    // Method begins at RVA 0x2318
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::TryEnter

  .method /*060000C9*/ public hidebysig static 
          void  TryEnter(object obj,
                         int32 millisecondsTimeout,
                         bool& lockTaken) cil managed
  // SIG: 00 03 01 1C 08 10 02
  {
    // Method begins at RVA 0x231b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Monitor::TryEnter

  .method /*060000CA*/ public hidebysig static 
          bool  TryEnter(object obj,
                         valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 00 02 02 1C 11 65
  {
    // Method begins at RVA 0x231d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::TryEnter

  .method /*060000CB*/ public hidebysig static 
          void  TryEnter(object obj,
                         valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
                         bool& lockTaken) cil managed
  // SIG: 00 03 01 1C 11 65 10 02
  {
    // Method begins at RVA 0x2320
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Monitor::TryEnter

  .method /*060000CC*/ public hidebysig static 
          bool  Wait(object obj) cil managed
  // SIG: 00 01 02 1C
  {
    .custom /*0C0000B5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2322
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::Wait

  .method /*060000CD*/ public hidebysig static 
          bool  Wait(object obj,
                     int32 millisecondsTimeout) cil managed
  // SIG: 00 02 02 1C 08
  {
    .custom /*0C0000B6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2325
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::Wait

  .method /*060000CE*/ public hidebysig static 
          bool  Wait(object obj,
                     int32 millisecondsTimeout,
                     bool exitContext) cil managed
  // SIG: 00 03 02 1C 08 02
  {
    .custom /*0C0000B7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2328
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::Wait

  .method /*060000CF*/ public hidebysig static 
          bool  Wait(object obj,
                     valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 00 02 02 1C 11 65
  {
    .custom /*0C0000B8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x232b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::Wait

  .method /*060000D0*/ public hidebysig static 
          bool  Wait(object obj,
                     valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
                     bool exitContext) cil managed
  // SIG: 00 03 02 1C 11 65 02
  {
    .custom /*0C0000B9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x232e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Monitor::Wait

  .property /*17000015*/ callconv(8) int64
          LockContentionCount()
  {
    .get int64 System.Threading.Monitor/*02000018*/::get_LockContentionCount() /* 060000BF */
  } // end of property Monitor::LockContentionCount
} // end of class System.Threading.Monitor

.class /*02000019*/ public auto ansi sealed beforefieldinit System.Threading.Mutex
       extends [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/
{
  .custom /*0C00004A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*060000D1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2331
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Mutex::.ctor

  .method /*060000D2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initiallyOwned) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2339
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Mutex::.ctor

  .method /*060000D3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initiallyOwned,
                               string name) cil managed
  // SIG: 20 02 01 02 0E
  {
    .custom /*0C0000BA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2341
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Mutex::.ctor

  .method /*060000D4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool initiallyOwned,
                               string name,
                               [out] bool& createdNew) cil managed
  // SIG: 20 03 01 02 0E 10 02
  {
    .custom /*0C0000BB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2349
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Mutex::.ctor

  .method /*060000D5*/ public hidebysig static 
          class System.Threading.Mutex/*02000019*/ 
          OpenExisting(string name) cil managed
  // SIG: 00 01 12 64 0E
  {
    // Method begins at RVA 0x2352
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Mutex::OpenExisting

  .method /*060000D6*/ public hidebysig instance void 
          ReleaseMutex() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2355
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Mutex::ReleaseMutex

  .method /*060000D7*/ public hidebysig static 
          bool  TryOpenExisting(string name,
                                [out] class System.Threading.Mutex/*02000019*/& result) cil managed
  // SIG: 00 02 02 0E 10 12 64
  {
    .param [2]/*08000103*/ 
    .custom /*0C0000C3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000C4:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000024*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2357
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Mutex::TryOpenExisting

} // end of class System.Threading.Mutex

.class /*0200001A*/ public auto ansi beforefieldinit System.Threading.ReaderWriterLockSlim
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .method /*060000D8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x235a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::.ctor

  .method /*060000D9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Threading.LockRecursionPolicy/*02000015*/ recursionPolicy) cil managed
  // SIG: 20 01 01 11 54
  {
    // Method begins at RVA 0x2362
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::.ctor

  .method /*060000DA*/ public hidebysig specialname 
          instance int32  get_CurrentReadCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x236a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_CurrentReadCount

  .method /*060000DB*/ public hidebysig specialname 
          instance bool  get_IsReadLockHeld() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x236d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_IsReadLockHeld

  .method /*060000DC*/ public hidebysig specialname 
          instance bool  get_IsUpgradeableReadLockHeld() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2370
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_IsUpgradeableReadLockHeld

  .method /*060000DD*/ public hidebysig specialname 
          instance bool  get_IsWriteLockHeld() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2373
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_IsWriteLockHeld

  .method /*060000DE*/ public hidebysig specialname 
          instance valuetype System.Threading.LockRecursionPolicy/*02000015*/ 
          get_RecursionPolicy() cil managed
  // SIG: 20 00 11 54
  {
    // Method begins at RVA 0x2376
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_RecursionPolicy

  .method /*060000DF*/ public hidebysig specialname 
          instance int32  get_RecursiveReadCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2379
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_RecursiveReadCount

  .method /*060000E0*/ public hidebysig specialname 
          instance int32  get_RecursiveUpgradeCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x237c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_RecursiveUpgradeCount

  .method /*060000E1*/ public hidebysig specialname 
          instance int32  get_RecursiveWriteCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x237f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_RecursiveWriteCount

  .method /*060000E2*/ public hidebysig specialname 
          instance int32  get_WaitingReadCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2382
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_WaitingReadCount

  .method /*060000E3*/ public hidebysig specialname 
          instance int32  get_WaitingUpgradeCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2385
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_WaitingUpgradeCount

  .method /*060000E4*/ public hidebysig specialname 
          instance int32  get_WaitingWriteCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2388
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::get_WaitingWriteCount

  .method /*060000E5*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x238b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::Dispose

  .method /*060000E6*/ public hidebysig instance void 
          EnterReadLock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x238d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::EnterReadLock

  .method /*060000E7*/ public hidebysig instance void 
          EnterUpgradeableReadLock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x238f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::EnterUpgradeableReadLock

  .method /*060000E8*/ public hidebysig instance void 
          EnterWriteLock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2391
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::EnterWriteLock

  .method /*060000E9*/ public hidebysig instance void 
          ExitReadLock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2393
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::ExitReadLock

  .method /*060000EA*/ public hidebysig instance void 
          ExitUpgradeableReadLock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2395
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::ExitUpgradeableReadLock

  .method /*060000EB*/ public hidebysig instance void 
          ExitWriteLock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2397
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReaderWriterLockSlim::ExitWriteLock

  .method /*060000EC*/ public hidebysig instance bool 
          TryEnterReadLock(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 02 08
  {
    // Method begins at RVA 0x2399
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::TryEnterReadLock

  .method /*060000ED*/ public hidebysig instance bool 
          TryEnterReadLock(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 02 11 65
  {
    // Method begins at RVA 0x239c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::TryEnterReadLock

  .method /*060000EE*/ public hidebysig instance bool 
          TryEnterUpgradeableReadLock(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 02 08
  {
    // Method begins at RVA 0x239f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::TryEnterUpgradeableReadLock

  .method /*060000EF*/ public hidebysig instance bool 
          TryEnterUpgradeableReadLock(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 02 11 65
  {
    // Method begins at RVA 0x23a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::TryEnterUpgradeableReadLock

  .method /*060000F0*/ public hidebysig instance bool 
          TryEnterWriteLock(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 02 08
  {
    // Method begins at RVA 0x23a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::TryEnterWriteLock

  .method /*060000F1*/ public hidebysig instance bool 
          TryEnterWriteLock(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 02 11 65
  {
    // Method begins at RVA 0x23a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReaderWriterLockSlim::TryEnterWriteLock

  .property /*17000016*/ instance callconv(8) int32
          CurrentReadCount()
  {
    .get instance int32 System.Threading.ReaderWriterLockSlim/*0200001A*/::get_CurrentReadCount() /* 060000DA */
  } // end of property ReaderWriterLockSlim::CurrentReadCount
  .property /*17000017*/ instance callconv(8) bool
          IsReadLockHeld()
  {
    .get instance bool System.Threading.ReaderWriterLockSlim/*0200001A*/::get_IsReadLockHeld() /* 060000DB */
  } // end of property ReaderWriterLockSlim::IsReadLockHeld
  .property /*17000018*/ instance callconv(8) bool
          IsUpgradeableReadLockHeld()
  {
    .get instance bool System.Threading.ReaderWriterLockSlim/*0200001A*/::get_IsUpgradeableReadLockHeld() /* 060000DC */
  } // end of property ReaderWriterLockSlim::IsUpgradeableReadLockHeld
  .property /*17000019*/ instance callconv(8) bool
          IsWriteLockHeld()
  {
    .get instance bool System.Threading.ReaderWriterLockSlim/*0200001A*/::get_IsWriteLockHeld() /* 060000DD */
  } // end of property ReaderWriterLockSlim::IsWriteLockHeld
  .property /*1700001A*/ instance callconv(8) valuetype System.Threading.LockRecursionPolicy/*02000015*/
          RecursionPolicy()
  {
    .get instance valuetype System.Threading.LockRecursionPolicy/*02000015*/ System.Threading.ReaderWriterLockSlim/*0200001A*/::get_RecursionPolicy() /* 060000DE */
  } // end of property ReaderWriterLockSlim::RecursionPolicy
  .property /*1700001B*/ instance callconv(8) int32
          RecursiveReadCount()
  {
    .get instance int32 System.Threading.ReaderWriterLockSlim/*0200001A*/::get_RecursiveReadCount() /* 060000DF */
  } // end of property ReaderWriterLockSlim::RecursiveReadCount
  .property /*1700001C*/ instance callconv(8) int32
          RecursiveUpgradeCount()
  {
    .get instance int32 System.Threading.ReaderWriterLockSlim/*0200001A*/::get_RecursiveUpgradeCount() /* 060000E0 */
  } // end of property ReaderWriterLockSlim::RecursiveUpgradeCount
  .property /*1700001D*/ instance callconv(8) int32
          RecursiveWriteCount()
  {
    .get instance int32 System.Threading.ReaderWriterLockSlim/*0200001A*/::get_RecursiveWriteCount() /* 060000E1 */
  } // end of property ReaderWriterLockSlim::RecursiveWriteCount
  .property /*1700001E*/ instance callconv(8) int32
          WaitingReadCount()
  {
    .get instance int32 System.Threading.ReaderWriterLockSlim/*0200001A*/::get_WaitingReadCount() /* 060000E2 */
  } // end of property ReaderWriterLockSlim::WaitingReadCount
  .property /*1700001F*/ instance callconv(8) int32
          WaitingUpgradeCount()
  {
    .get instance int32 System.Threading.ReaderWriterLockSlim/*0200001A*/::get_WaitingUpgradeCount() /* 060000E3 */
  } // end of property ReaderWriterLockSlim::WaitingUpgradeCount
  .property /*17000020*/ instance callconv(8) int32
          WaitingWriteCount()
  {
    .get instance int32 System.Threading.ReaderWriterLockSlim/*0200001A*/::get_WaitingWriteCount() /* 060000E4 */
  } // end of property ReaderWriterLockSlim::WaitingWriteCount
} // end of class System.Threading.ReaderWriterLockSlim

.class /*0200001B*/ public auto ansi sealed beforefieldinit System.Threading.Semaphore
       extends [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/
{
  .custom /*0C00004D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*060000F2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCount,
                               int32 maximumCount) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x23ab
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Semaphore::.ctor

  .method /*060000F3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCount,
                               int32 maximumCount,
                               string name) cil managed
  // SIG: 20 03 01 08 08 0E
  {
    .custom /*0C0000BC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23b3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Semaphore::.ctor

  .method /*060000F4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCount,
                               int32 maximumCount,
                               string name,
                               [out] bool& createdNew) cil managed
  // SIG: 20 04 01 08 08 0E 10 02
  {
    .custom /*0C0000BD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23bb
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Semaphore::.ctor

  .method /*060000F5*/ public hidebysig static 
          class System.Threading.Semaphore/*0200001B*/ 
          OpenExisting(string name) cil managed
  // SIG: 00 01 12 6C 0E
  {
    .custom /*0C0000BE:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100002D*/::.ctor(string) /* 0A00001B */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Semaphore::OpenExisting

  .method /*060000F6*/ public hidebysig instance int32 
          Release() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Semaphore::Release

  .method /*060000F7*/ public hidebysig instance int32 
          Release(int32 releaseCount) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Semaphore::Release

  .method /*060000F8*/ public hidebysig static 
          bool  TryOpenExisting(string name,
                                [out] class System.Threading.Semaphore/*0200001B*/& result) cil managed
  // SIG: 00 02 02 0E 10 12 6C
  {
    .custom /*0C0000BF:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100002D*/::.ctor(string) /* 0A00001B */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .param [2]/*08000117*/ 
    .custom /*0C0000CC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000024*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Semaphore::TryOpenExisting

} // end of class System.Threading.Semaphore

.class /*0200001C*/ public auto ansi beforefieldinit System.Threading.SemaphoreFullException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000026*/
{
  .custom /*0C00004F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000050:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*060000F9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SemaphoreFullException::.ctor

  .method /*060000FA*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001F*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000020*/ context) cil managed
  // SIG: 20 02 01 12 7D 11 80 81
  {
    .custom /*0C0000C0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000C1:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C0000C2:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x23d8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SemaphoreFullException::.ctor

  .method /*060000FB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23e0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SemaphoreFullException::.ctor

  .method /*060000FC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 6D
  {
    // Method begins at RVA 0x23e8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SemaphoreFullException::.ctor

} // end of class System.Threading.SemaphoreFullException

.class /*0200001D*/ public auto ansi beforefieldinit System.Threading.SemaphoreSlim
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C000051:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000052:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C00002E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*060000FD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCount) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x23f0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SemaphoreSlim::.ctor

  .method /*060000FE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCount,
                               int32 maxCount) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x23f8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SemaphoreSlim::.ctor

  .method /*060000FF*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ 
          get_AvailableWaitHandle() cil managed
  // SIG: 20 00 12 80 85
  {
    // Method begins at RVA 0x2400
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::get_AvailableWaitHandle

  .method /*06000100*/ public hidebysig specialname 
          instance int32  get_CurrentCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2403
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::get_CurrentCount

  .method /*06000101*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2406
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SemaphoreSlim::Dispose

  .method /*06000102*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2408
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SemaphoreSlim::Dispose

  .method /*06000103*/ public hidebysig instance int32 
          Release() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x240a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::Release

  .method /*06000104*/ public hidebysig instance int32 
          Release(int32 releaseCount) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x240d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::Release

  .method /*06000105*/ public hidebysig instance void 
          Wait() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000C5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2410
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SemaphoreSlim::Wait

  .method /*06000106*/ public hidebysig instance bool 
          Wait(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 02 08
  {
    .custom /*0C0000C6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2412
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::Wait

  .method /*06000107*/ public hidebysig instance bool 
          Wait(int32 millisecondsTimeout,
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 02 08 11 61
  {
    .custom /*0C0000C7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2415
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::Wait

  .method /*06000108*/ public hidebysig instance void 
          Wait(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 01 01 11 61
  {
    .custom /*0C0000C8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2418
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SemaphoreSlim::Wait

  .method /*06000109*/ public hidebysig instance bool 
          Wait(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 02 11 65
  {
    .custom /*0C0000C9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x241a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::Wait

  .method /*0600010A*/ public hidebysig instance bool 
          Wait(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 02 11 65 11 61
  {
    .custom /*0C0000CA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x241d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::Wait

  .method /*0600010B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000036*/ 
          WaitAsync() cil managed
  // SIG: 20 00 12 80 D9
  {
    // Method begins at RVA 0x2420
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::WaitAsync

  .method /*0600010C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000037*/<bool> 
          WaitAsync(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 15 12 80 DD 01 02 08
  {
    // Method begins at RVA 0x2423
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::WaitAsync

  .method /*0600010D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000037*/<bool> 
          WaitAsync(int32 millisecondsTimeout,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 80 DD 01 02 08 11 61
  {
    // Method begins at RVA 0x2426
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::WaitAsync

  .method /*0600010E*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000036*/ 
          WaitAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 D9 11 61
  {
    // Method begins at RVA 0x2429
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::WaitAsync

  .method /*0600010F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000037*/<bool> 
          WaitAsync(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 20 01 15 12 80 DD 01 02 11 65
  {
    // Method begins at RVA 0x242c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::WaitAsync

  .method /*06000110*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000037*/<bool> 
          WaitAsync(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000018*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 80 DD 01 02 11 65 11 61
  {
    // Method begins at RVA 0x242f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlim::WaitAsync

  .property /*17000021*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/
          AvailableWaitHandle()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*01000021*/ System.Threading.SemaphoreSlim/*0200001D*/::get_AvailableWaitHandle() /* 060000FF */
  } // end of property SemaphoreSlim::AvailableWaitHandle
  .property /*17000022*/ instance callconv(8) int32
          CurrentCount()
  {
    .get instance int32 System.Threading.SemaphoreSlim/*0200001D*/::get_CurrentCount() /* 06000100 */
  } // end of property SemaphoreSlim::CurrentCount
} // end of class System.Threading.SemaphoreSlim

.class /*0200001E*/ public auto ansi sealed System.Threading.SendOrPostCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000029*/
{
  .method /*06000111*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method SendOrPostCallback::.ctor

  .method /*06000112*/ public hidebysig newslot virtual 
          instance void  Invoke(object state) runtime managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000CB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method SendOrPostCallback::Invoke

  .method /*06000113*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginInvoke(object state,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 80 A9 1C 12 80 AD 1C
  {
    .param [1]/*08000133*/ 
    .custom /*0C0000D2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
  } // end of method SendOrPostCallback::BeginInvoke

  .method /*06000114*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ result) runtime managed
  // SIG: 20 01 01 12 80 A9
  {
  } // end of method SendOrPostCallback::EndInvoke

} // end of class System.Threading.SendOrPostCallback

.class /*0200001F*/ public sequential ansi sealed beforefieldinit System.Threading.SpinLock
       extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
{
  .field /*0400000D*/ private int32 _dummyPrimitive
  .method /*06000115*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool enableThreadOwnerTracking) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2432
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpinLock::.ctor

  .method /*06000116*/ public hidebysig specialname 
          instance bool  get_IsHeld() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2435
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpinLock::get_IsHeld

  .method /*06000117*/ public hidebysig specialname 
          instance bool  get_IsHeldByCurrentThread() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2438
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpinLock::get_IsHeldByCurrentThread

  .method /*06000118*/ public hidebysig specialname 
          instance bool  get_IsThreadOwnerTrackingEnabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x243b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpinLock::get_IsThreadOwnerTrackingEnabled

  .method /*06000119*/ public hidebysig instance void 
          Enter(bool& lockTaken) cil managed
  // SIG: 20 01 01 10 02
  {
    // Method begins at RVA 0x243e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinLock::Enter

  .method /*0600011A*/ public hidebysig instance void 
          Exit() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2440
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinLock::Exit

  .method /*0600011B*/ public hidebysig instance void 
          Exit(bool useMemoryBarrier) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2442
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinLock::Exit

  .method /*0600011C*/ public hidebysig instance void 
          TryEnter(bool& lockTaken) cil managed
  // SIG: 20 01 01 10 02
  {
    // Method begins at RVA 0x2444
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinLock::TryEnter

  .method /*0600011D*/ public hidebysig instance void 
          TryEnter(int32 millisecondsTimeout,
                   bool& lockTaken) cil managed
  // SIG: 20 02 01 08 10 02
  {
    // Method begins at RVA 0x2446
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinLock::TryEnter

  .method /*0600011E*/ public hidebysig instance void 
          TryEnter(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout,
                   bool& lockTaken) cil managed
  // SIG: 20 02 01 11 65 10 02
  {
    // Method begins at RVA 0x2448
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinLock::TryEnter

  .property /*17000023*/ instance callconv(8) bool
          IsHeld()
  {
    .get instance bool System.Threading.SpinLock/*0200001F*/::get_IsHeld() /* 06000116 */
  } // end of property SpinLock::IsHeld
  .property /*17000024*/ instance callconv(8) bool
          IsHeldByCurrentThread()
  {
    .get instance bool System.Threading.SpinLock/*0200001F*/::get_IsHeldByCurrentThread() /* 06000117 */
  } // end of property SpinLock::IsHeldByCurrentThread
  .property /*17000025*/ instance callconv(8) bool
          IsThreadOwnerTrackingEnabled()
  {
    .get instance bool System.Threading.SpinLock/*0200001F*/::get_IsThreadOwnerTrackingEnabled() /* 06000118 */
  } // end of property SpinLock::IsThreadOwnerTrackingEnabled
} // end of class System.Threading.SpinLock

.class /*02000020*/ public sequential ansi sealed beforefieldinit System.Threading.SpinWait
       extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
{
  .custom /*0C000053:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000054:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .field /*0400000E*/ private int32 _dummyPrimitive
  .method /*0600011F*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x244a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpinWait::get_Count

  .method /*06000120*/ public hidebysig specialname 
          instance bool  get_NextSpinWillYield() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x244d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpinWait::get_NextSpinWillYield

  .method /*06000121*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2450
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinWait::Reset

  .method /*06000122*/ public hidebysig instance void 
          SpinOnce() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2452
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinWait::SpinOnce

  .method /*06000123*/ public hidebysig instance void 
          SpinOnce(int32 sleep1Threshold) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2454
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinWait::SpinOnce

  .method /*06000124*/ public hidebysig static 
          void  SpinUntil(class [System.Runtime/*23000001*/]System.Func`1/*01000035*/<bool> condition) cil managed
  // SIG: 00 01 01 15 12 80 D5 01 02
  {
    // Method begins at RVA 0x2456
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SpinWait::SpinUntil

  .method /*06000125*/ public hidebysig static 
          bool  SpinUntil(class [System.Runtime/*23000001*/]System.Func`1/*01000035*/<bool> condition,
                          int32 millisecondsTimeout) cil managed
  // SIG: 00 02 02 15 12 80 D5 01 02 08
  {
    // Method begins at RVA 0x2458
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpinWait::SpinUntil

  .method /*06000126*/ public hidebysig static 
          bool  SpinUntil(class [System.Runtime/*23000001*/]System.Func`1/*01000035*/<bool> condition,
                          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000019*/ timeout) cil managed
  // SIG: 00 02 02 15 12 80 D5 01 02 11 65
  {
    // Method begins at RVA 0x245b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SpinWait::SpinUntil

  .property /*17000026*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Threading.SpinWait/*02000020*/::get_Count() /* 0600011F */
  } // end of property SpinWait::Count
  .property /*17000027*/ instance callconv(8) bool
          NextSpinWillYield()
  {
    .get instance bool System.Threading.SpinWait/*02000020*/::get_NextSpinWillYield() /* 06000120 */
  } // end of property SpinWait::NextSpinWillYield
} // end of class System.Threading.SpinWait

.class /*02000021*/ public auto ansi beforefieldinit System.Threading.SynchronizationContext
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C000055:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000056:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000127*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x245e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SynchronizationContext::.ctor

  .method /*06000128*/ public hidebysig specialname static 
          class System.Threading.SynchronizationContext/*02000021*/ 
          get_Current() cil managed
  // SIG: 00 00 12 80 84
  {
    .custom /*0C0000CE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2466
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SynchronizationContext::get_Current

  .method /*06000129*/ public hidebysig newslot virtual 
          instance class System.Threading.SynchronizationContext/*02000021*/ 
          CreateCopy() cil managed
  // SIG: 20 00 12 80 84
  {
    // Method begins at RVA 0x2469
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SynchronizationContext::CreateCopy

  .method /*0600012A*/ public hidebysig instance bool 
          IsWaitNotificationRequired() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x246c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SynchronizationContext::IsWaitNotificationRequired

  .method /*0600012B*/ public hidebysig newslot virtual 
          instance void  OperationCompleted() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x246f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SynchronizationContext::OperationCompleted

  .method /*0600012C*/ public hidebysig newslot virtual 
          instance void  OperationStarted() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2471
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SynchronizationContext::OperationStarted

  .method /*0600012D*/ public hidebysig newslot virtual 
          instance void  Post(class System.Threading.SendOrPostCallback/*0200001E*/ d,
                              object state) cil managed
  // SIG: 20 02 01 12 78 1C
  {
    .param [2]/*08000146*/ 
    .custom /*0C0000D7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2473
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SynchronizationContext::Post

  .method /*0600012E*/ public hidebysig newslot virtual 
          instance void  Send(class System.Threading.SendOrPostCallback/*0200001E*/ d,
                              object state) cil managed
  // SIG: 20 02 01 12 78 1C
  {
    .param [2]/*08000148*/ 
    .custom /*0C0000D8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2475
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SynchronizationContext::Send

  .method /*0600012F*/ public hidebysig static 
          void  SetSynchronizationContext(class System.Threading.SynchronizationContext/*02000021*/ syncContext) cil managed
  // SIG: 00 01 01 12 80 84
  {
    .custom /*0C0000CF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2477
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SynchronizationContext::SetSynchronizationContext

  .method /*06000130*/ family hidebysig instance void 
          SetWaitNotificationRequired() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2479
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SynchronizationContext::SetWaitNotificationRequired

  .method /*06000131*/ public hidebysig newslot virtual 
          instance int32  Wait(native int[] waitHandles,
                               bool waitAll,
                               int32 millisecondsTimeout) cil managed
  // SIG: 20 03 08 1D 18 02 08
  {
    .custom /*0C0000D0:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x247b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SynchronizationContext::Wait

  .method /*06000132*/ family hidebysig static 
          int32  WaitHelper(native int[] waitHandles,
                            bool waitAll,
                            int32 millisecondsTimeout) cil managed
  // SIG: 00 03 08 1D 18 02 08
  {
    .custom /*0C0000D1:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x247e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SynchronizationContext::WaitHelper

  .property /*17000028*/ callconv(8) class System.Threading.SynchronizationContext/*02000021*/
          Current()
  {
    .custom /*0C000063:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .get class System.Threading.SynchronizationContext/*02000021*/ System.Threading.SynchronizationContext/*02000021*/::get_Current() /* 06000128 */
  } // end of property SynchronizationContext::Current
} // end of class System.Threading.SynchronizationContext

.class /*02000022*/ public auto ansi beforefieldinit System.Threading.SynchronizationLockException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000026*/
{
  .custom /*0C000057:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000058:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000133*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2481
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SynchronizationLockException::.ctor

  .method /*06000134*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001F*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000020*/ context) cil managed
  // SIG: 20 02 01 12 7D 11 80 81
  {
    .custom /*0C0000D3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D4:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C0000D5:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2489
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SynchronizationLockException::.ctor

  .method /*06000135*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2491
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SynchronizationLockException::.ctor

  .method /*06000136*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 6D
  {
    // Method begins at RVA 0x2499
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000026*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SynchronizationLockException::.ctor

} // end of class System.Threading.SynchronizationLockException

.class /*02000023*/ public auto ansi beforefieldinit System.Threading.ThreadLocal`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C000059:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000033:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .param type T /*2A000003*/ 
    .custom /*0C000019:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
  .method /*06000137*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24a1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadLocal`1::.ctor

  .method /*06000138*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool trackAllValues) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24a9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadLocal`1::.ctor

  .method /*06000139*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`1/*01000035*/<!T> valueFactory) cil managed
  // SIG: 20 01 01 15 12 80 D5 01 13 00
  {
    // Method begins at RVA 0x24b1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadLocal`1::.ctor

  .method /*0600013A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`1/*01000035*/<!T> valueFactory,
                               bool trackAllValues) cil managed
  // SIG: 20 02 01 15 12 80 D5 01 13 00 02
  {
    // Method begins at RVA 0x24b9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadLocal`1::.ctor

  .method /*0600013B*/ public hidebysig specialname 
          instance bool  get_IsValueCreated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ThreadLocal`1::get_IsValueCreated

  .method /*0600013C*/ public hidebysig specialname 
          instance !T  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    .param [0]/*08000159*/ 
    .custom /*0C0000DF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullAttribute/*01000028*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ThreadLocal`1::get_Value

  .method /*0600013D*/ public hidebysig specialname 
          instance void  set_Value(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x24c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ThreadLocal`1::set_Value

  .method /*0600013E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000038*/<!T> 
          get_Values() cil managed
  // SIG: 20 00 15 12 80 E1 01 13 00
  {
    // Method begins at RVA 0x24c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ThreadLocal`1::get_Values

  .method /*0600013F*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24cc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ThreadLocal`1::Dispose

  .method /*06000140*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24ce
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ThreadLocal`1::Dispose

  .method /*06000141*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000013*/::Finalize /*01000013::0A000021*/ 
    // Method begins at RVA 0x24d0
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::Finalize() /* 0A000021 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method ThreadLocal`1::Finalize

  .method /*06000142*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000D6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ThreadLocal`1::ToString

  .property /*17000029*/ instance callconv(8) bool
          IsValueCreated()
  {
    .get instance bool System.Threading.ThreadLocal`1/*02000023*/::get_IsValueCreated() /* 0600013B */
  } // end of property ThreadLocal`1::IsValueCreated
  .property /*1700002A*/ instance callconv(8) !T
          Value()
  {
    .get instance !T System.Threading.ThreadLocal`1/*02000023*/::get_Value() /* 0600013C */
    .set instance void System.Threading.ThreadLocal`1/*02000023*/::set_Value(!T) /* 0600013D */
  } // end of property ThreadLocal`1::Value
  .property /*1700002B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000038*/<!T>
          Values()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000038*/<!T> System.Threading.ThreadLocal`1/*02000023*/::get_Values() /* 0600013E */
  } // end of property ThreadLocal`1::Values
} // end of class System.Threading.ThreadLocal`1

.class /*02000024*/ public abstract auto ansi sealed beforefieldinit System.Threading.Volatile
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C00005B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000143*/ public hidebysig static 
          bool  Read([in] bool& location) cil managed
  // SIG: 00 01 02 10 02
  {
    .param [1]/*0800015C*/ 
    .custom /*0C0000E3:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*06000144*/ public hidebysig static 
          uint8  Read([in] uint8& location) cil managed
  // SIG: 00 01 05 10 05
  {
    .param [1]/*0800015D*/ 
    .custom /*0C0000E5:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*06000145*/ public hidebysig static 
          float64  Read([in] float64& location) cil managed
  // SIG: 00 01 0D 10 0D
  {
    .param [1]/*0800015E*/ 
    .custom /*0C0000E6:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2501
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*06000146*/ public hidebysig static 
          int16  Read([in] int16& location) cil managed
  // SIG: 00 01 06 10 06
  {
    .param [1]/*0800015F*/ 
    .custom /*0C0000E7:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2504
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*06000147*/ public hidebysig static 
          int32  Read([in] int32& location) cil managed
  // SIG: 00 01 08 10 08
  {
    .param [1]/*08000160*/ 
    .custom /*0C0000EB:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2507
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*06000148*/ public hidebysig static 
          int64  Read([in] int64& location) cil managed
  // SIG: 00 01 0A 10 0A
  {
    .param [1]/*08000161*/ 
    .custom /*0C0000EC:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x250a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*06000149*/ public hidebysig static 
          native int  Read([in] native int& location) cil managed
  // SIG: 00 01 18 10 18
  {
    .param [1]/*08000162*/ 
    .custom /*0C0000ED:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x250d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*0600014A*/ public hidebysig static 
          int8  Read([in] int8& location) cil managed
  // SIG: 00 01 04 10 04
  {
    .custom /*0C0000D9:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000163*/ 
    .custom /*0C0000EE:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2510
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*0600014B*/ public hidebysig static 
          float32  Read([in] float32& location) cil managed
  // SIG: 00 01 0C 10 0C
  {
    .param [1]/*08000164*/ 
    .custom /*0C0000EF:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2513
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*0600014C*/ public hidebysig static 
          uint16  Read([in] uint16& location) cil managed
  // SIG: 00 01 07 10 07
  {
    .custom /*0C0000DA:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000165*/ 
    .custom /*0C0000F0:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2516
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*0600014D*/ public hidebysig static 
          uint32  Read([in] uint32& location) cil managed
  // SIG: 00 01 09 10 09
  {
    .custom /*0C0000DB:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000166*/ 
    .custom /*0C0000F1:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2519
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*0600014E*/ public hidebysig static 
          uint64  Read([in] uint64& location) cil managed
  // SIG: 00 01 0B 10 0B
  {
    .custom /*0C0000DC:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000167*/ 
    .custom /*0C0000F2:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x251c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*0600014F*/ public hidebysig static 
          native uint  Read([in] native uint& location) cil managed
  // SIG: 00 01 19 10 19
  {
    .custom /*0C0000DD:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000168*/ 
    .custom /*0C0000F3:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x251f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*06000150*/ public hidebysig static 
          !!T  Read<class T>([in] !!T& location) cil managed
  // SIG: 10 01 01 1E 00 10 1E 00
  {
    .param type T /*2A00000B*/ 
      .custom /*0C00002F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000169*/ 
    .custom /*0C0000F4:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 08 6C 6F 63 61 74 69 6F 6E 00 00 )          // ...location..
    .param [1]/*0800016A*/ 
    .custom /*0C0000F5:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000030*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    .custom /*0C0000F6:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 08 6C 6F 63 61 74 69 6F 6E 00 00 )          // ...location..
    // Method begins at RVA 0x2522
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Volatile::Read

  .method /*06000151*/ public hidebysig static 
          void  Write(bool& location,
                      bool 'value') cil managed
  // SIG: 00 02 01 10 02 02
  {
    // Method begins at RVA 0x2525
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*06000152*/ public hidebysig static 
          void  Write(uint8& location,
                      uint8 'value') cil managed
  // SIG: 00 02 01 10 05 05
  {
    // Method begins at RVA 0x2527
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*06000153*/ public hidebysig static 
          void  Write(float64& location,
                      float64 'value') cil managed
  // SIG: 00 02 01 10 0D 0D
  {
    // Method begins at RVA 0x2529
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*06000154*/ public hidebysig static 
          void  Write(int16& location,
                      int16 'value') cil managed
  // SIG: 00 02 01 10 06 06
  {
    // Method begins at RVA 0x252b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*06000155*/ public hidebysig static 
          void  Write(int32& location,
                      int32 'value') cil managed
  // SIG: 00 02 01 10 08 08
  {
    // Method begins at RVA 0x252d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*06000156*/ public hidebysig static 
          void  Write(int64& location,
                      int64 'value') cil managed
  // SIG: 00 02 01 10 0A 0A
  {
    // Method begins at RVA 0x252f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*06000157*/ public hidebysig static 
          void  Write(native int& location,
                      native int 'value') cil managed
  // SIG: 00 02 01 10 18 18
  {
    // Method begins at RVA 0x2531
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*06000158*/ public hidebysig static 
          void  Write(int8& location,
                      int8 'value') cil managed
  // SIG: 00 02 01 10 04 04
  {
    .custom /*0C0000DE:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2533
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*06000159*/ public hidebysig static 
          void  Write(float32& location,
                      float32 'value') cil managed
  // SIG: 00 02 01 10 0C 0C
  {
    // Method begins at RVA 0x2535
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*0600015A*/ public hidebysig static 
          void  Write(uint16& location,
                      uint16 'value') cil managed
  // SIG: 00 02 01 10 07 07
  {
    .custom /*0C0000E0:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2537
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*0600015B*/ public hidebysig static 
          void  Write(uint32& location,
                      uint32 'value') cil managed
  // SIG: 00 02 01 10 09 09
  {
    .custom /*0C0000E1:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2539
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*0600015C*/ public hidebysig static 
          void  Write(uint64& location,
                      uint64 'value') cil managed
  // SIG: 00 02 01 10 0B 0B
  {
    .custom /*0C0000E2:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x253b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*0600015D*/ public hidebysig static 
          void  Write(native uint& location,
                      native uint 'value') cil managed
  // SIG: 00 02 01 10 19 19
  {
    .custom /*0C0000E4:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x253d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

  .method /*0600015E*/ public hidebysig static 
          void  Write<class T>(!!T& location,
                               !!T 'value') cil managed
  // SIG: 10 01 02 01 10 1E 00 1E 00
  {
    .param type T /*2A00000C*/ 
      .custom /*0C000034:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000185*/ 
    .custom /*0C0000F7:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100002F*/::.ctor(string) /* 0A00001C */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x253f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Volatile::Write

} // end of class System.Threading.Volatile

.class /*02000025*/ public auto ansi beforefieldinit System.Threading.WaitHandleCannotBeOpenedException
       extends [System.Runtime/*23000001*/]System.ApplicationException/*01000039*/
{
  .custom /*0C00005D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*0600015F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2541
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.ApplicationException/*01000039*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WaitHandleCannotBeOpenedException::.ctor

  .method /*06000160*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001F*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000020*/ context) cil managed
  // SIG: 20 02 01 12 7D 11 80 81
  {
    .custom /*0C0000E8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001A*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000E9:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
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
    .custom /*0C0000EA:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001D*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2549
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.ApplicationException/*01000039*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WaitHandleCannotBeOpenedException::.ctor

  .method /*06000161*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2551
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.ApplicationException/*01000039*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WaitHandleCannotBeOpenedException::.ctor

  .method /*06000162*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001B*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 6D
  {
    // Method begins at RVA 0x2559
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.ApplicationException/*01000039*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WaitHandleCannotBeOpenedException::.ctor

} // end of class System.Threading.WaitHandleCannotBeOpenedException


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
