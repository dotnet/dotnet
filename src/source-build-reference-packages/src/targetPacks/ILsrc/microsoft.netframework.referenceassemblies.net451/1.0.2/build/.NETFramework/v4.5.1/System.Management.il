
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
// Time-date stamp:            0x523110ce
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x0b
// Minor linker version:           0x00
// Size of code:                   0x0000a800
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000c616
// Base of code:                   0x00002000
// Base of data:                   0x0000e000
// Image base:                     0x63c00000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0006
// Minor subsystem version:        0x0000
// Size of image:                  0x00012000
// Size of headers:                0x00000200
// Checksum:                       0x00011429
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000c5c4 [0x0000004f] address [size] of Import Directory:          
// 0x0000e000 [0x00000460] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000b200 [0x00003e70] address [size] of Security Directory:        
// 0x00010000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00000000 [0x00000000] address [size] of Debug Directory:           
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
//              0x0000a61c Virtual Size
//              0x00002000 Virtual Address
//              0x0000a800 Size of Raw Data
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
//              0x00000460 Virtual Size
//              0x0000e000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000aa00 Pointer to Raw Data
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
//              0x0000b000 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000618
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000c606 Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 C0 63 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x000022e4 [0x00007f60] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x0000a244 [0x00002280] address [size] of Resources Directory:       
// 0x0000c4c4 [0x00000100] address [size] of Strong Name Signature:     
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
//              0x000047e0 Size
//              '#~' Name
//    Stream 2:
//              0x0000484c Offset
//              0x000025dc Size
//              '#Strings' Name
//    Stream 3:
//              0x00006e28 Offset
//              0x00000008 Size
//              '#US' Name
//    Stream 4:
//              0x00006e30 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00006e40 Offset
//              0x00001120 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000030903b45f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 61552
// PE header size       : 512 (496 used)    ( 0.83%)
// PE additional info   : 17275             (28.07%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.12%)
// CLR meta-data size  : 32608              (52.98%)
// CLR additional info : 9088               (14.76%)
// CLR method headers  : 645                ( 1.05%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 3.33%)
// Unaccounted          : -696              (-1.13%)

// Num.of PE sections   : 3
//   .text    - 43008
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 32608
//   Module        -    1 (10 bytes)
//   TypeDef       -   75 (1050 bytes)    2 interfaces, 0 explicit layout
//   TypeRef       -   61 (366 bytes)
//   MethodDef     -  586 (8204 bytes)    5 abstract, 0 native, 557 bodies
//   FieldDef      -  170 (1020 bytes)    1 constant
//   MemberRef     -   30 (180 bytes)
//   ParamDef      -  570 (3420 bytes)
//   MethodImpl    -   11 (66 bytes)
//   Constant      -  160 (960 bytes)
//   CustomAttribute-   45 (270 bytes)
//   InterfaceImpl -   23 (92 bytes)
//   PropertyMap   -   45 (180 bytes)
//   Property      -  146 (876 bytes)
//   MethodSemantic-  234 (1404 bytes)
//   Security      -    3 (18 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   ManifestResource-    1 (12 bytes)
//   NestedClass   -    4 (16 bytes)
//   EventMap      -    2 (8 bytes)
//   Event         -    6 (36 bytes)
//   Strings       -  9691 bytes
//   Blobs         -  4384 bytes
//   UserStrings   -     8 bytes
//   Guids         -    16 bytes
//   Uncategorized -   219 bytes

// CLR additional info : 9088
//   Resources         - 8832

// CLR method headers : 645
//   Num.of method bodies  - 557
//   Num.of fat headers    - 8
//   Num.of tiny headers   - 549

// Managed code : 0
//   Ave method size - 0
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class TextFormat                     (public) (auto) (ansi) (sealed) 
// Class CimType                        (public) (auto) (ansi) (sealed) 
// Class ComparisonSettings             (public) (auto) (ansi) (sealed) 
// Class ManagementBaseObject           (public) (auto) (ansi) 
// Class ManagementObject               (public) (auto) (ansi) 
// Class ManagementClass                (public) (auto) (ansi) 
// Class ManagementDateTimeConverter    (public) (auto) (ansi) (sealed) 
// Class ManagementEventArgs            (public) (abstract) (auto) (ansi) 
// Class ObjectReadyEventArgs           (public) (auto) (ansi) 
// Class CompletedEventArgs             (public) (auto) (ansi) 
// Class ObjectPutEventArgs             (public) (auto) (ansi) 
// Class ProgressEventArgs              (public) (auto) (ansi) 
// Class EventArrivedEventArgs          (public) (auto) (ansi) 
// Class StoppedEventArgs               (public) (auto) (ansi) 
// Class EventArrivedEventHandler       (public) (auto) (ansi) (sealed) 
// Class StoppedEventHandler            (public) (auto) (ansi) (sealed) 
// Class ManagementEventWatcher         (public) (auto) (ansi) 
// Class ManagementStatus               (public) (auto) (ansi) (sealed) 
// Class ManagementException            (public) (auto) (ansi) 
// Class ManagementNamedValueCollection (public) (auto) (ansi) 
// Class ManagementObjectCollection     (public) (auto) (ansi) 
// Class ManagementObjectEnumerator     (auto) (ansi) (nested public) 
// Class ManagementObjectSearcher       (public) (auto) (ansi) 
// Class ObjectReadyEventHandler        (public) (auto) (ansi) (sealed) 
// Class CompletedEventHandler          (public) (auto) (ansi) (sealed) 
// Class ProgressEventHandler           (public) (auto) (ansi) (sealed) 
// Class ObjectPutEventHandler          (public) (auto) (ansi) (sealed) 
// Class ManagementOperationObserver    (public) (auto) (ansi) 
// Class AuthenticationLevel            (public) (auto) (ansi) (sealed) 
// Class ImpersonationLevel             (public) (auto) (ansi) (sealed) 
// Class PutType                        (public) (auto) (ansi) (sealed) 
// Class ManagementOptions              (public) (abstract) (auto) (ansi) 
// Class EnumerationOptions             (public) (auto) (ansi) 
// Class EventWatcherOptions            (public) (auto) (ansi) 
// Class ObjectGetOptions               (public) (auto) (ansi) 
// Class PutOptions                     (public) (auto) (ansi) 
// Class DeleteOptions                  (public) (auto) (ansi) 
// Class InvokeMethodOptions            (public) (auto) (ansi) 
// Class ConnectionOptions              (public) (auto) (ansi) 
// Class ManagementPath                 (public) (auto) (ansi) 
// Class ManagementPathConverter        (auto) (ansi) 
// Class ManagementQuery                (public) (abstract) (auto) (ansi) 
// Class ObjectQuery                    (public) (auto) (ansi) 
// Class EventQuery                     (public) (auto) (ansi) 
// Class WqlObjectQuery                 (public) (auto) (ansi) 
// Class SelectQuery                    (public) (auto) (ansi) 
// Class RelatedObjectQuery             (public) (auto) (ansi) 
// Class RelationshipQuery              (public) (auto) (ansi) 
// Class WqlEventQuery                  (public) (auto) (ansi) 
// Class ManagementQueryConverter       (auto) (ansi) 
// Class ManagementScope                (public) (auto) (ansi) 
// Class ManagementScopeConverter       (auto) (ansi) 
// Class PropertyData                   (public) (auto) (ansi) 
// Class PropertyDataCollection         (public) (auto) (ansi) 
// Class PropertyDataEnumerator         (auto) (ansi) (nested public) 
// Class QualifierData                  (public) (auto) (ansi) 
// Class QualifierDataCollection        (public) (auto) (ansi) 
// Class QualifierDataEnumerator        (auto) (ansi) (nested public) 
// Class MethodData                     (public) (auto) (ansi) 
// Class MethodDataCollection           (public) (auto) (ansi) 
// Class MethodDataEnumerator           (auto) (ansi) (nested public) 
// Class CodeLanguage                   (public) (auto) (ansi) (sealed) 
// Class InstrumentedAttribute          (public) (auto) (ansi) 
// Class InstrumentationType            (public) (auto) (ansi) (sealed) 
// Class InstrumentationClassAttribute  (public) (auto) (ansi) 
// Class ManagedNameAttribute           (public) (auto) (ansi) 
// Class IgnoreMemberAttribute          (public) (auto) (ansi) 
// Class Instrumentation                (public) (auto) (ansi) 
// Interface IEvent                         (public) (abstract) (auto) (ansi) 
// Class BaseEvent                      (public) (abstract) (auto) (ansi) 
// Interface IInstance                      (public) (abstract) (auto) (ansi) 
// Class Instance                       (public) (abstract) (auto) (ansi) 
// Class ManagementInstaller            (public) (auto) (ansi) 
// Class DefaultManagementProjectInstaller (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000003*/ System.Configuration.Install
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly extern /*23000004*/ Microsoft.JScript
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.Management
{
  .custom /*0C000002:0A000005*/ instance void [mscorlib/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000021*/::.ctor(string) /* 0A000005 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C000003:0A000006*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyKeyFileAttribute/*01000022*/::.ctor(string) /* 0A000006 */ = ( 01 00 25 66 3A 5C 64 64 5C 74 6F 6F 6C 73 5C 64   // ..%f:\dd\tools\d
                                                                                                                                                             65 76 64 69 76 5C 46 69 6E 61 6C 50 75 62 6C 69   // evdiv\FinalPubli
                                                                                                                                                             63 4B 65 79 2E 73 6E 6B 00 00 )                   // cKey.snk..
  .custom /*0C000004:0A000007*/ instance void [mscorlib/*23000001*/]System.Resources.SatelliteContractVersionAttribute/*01000023*/::.ctor(string) /* 0A000007 */ = ( 01 00 07 34 2E 30 2E 30 2E 30 00 00 )             // ...4.0.0.0..
  .custom /*0C000005:0A000008*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDelaySignAttribute/*01000024*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000006:0A000009*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblySignatureKeyAttribute/*01000025*/::.ctor(string,
                                                                                                                                       string) /* 0A000009 */ = ( 01 00 82 40 30 30 32 34 30 30 30 30 30 63 38 30   // ...@002400000c80
                                                                                                                                                                  30 30 30 30 31 34 30 31 30 30 30 30 30 36 30 32   // 0000140100000602
                                                                                                                                                                  30 30 30 30 30 30 32 34 30 30 30 30 35 32 35 33   // 0000002400005253
                                                                                                                                                                  34 31 33 31 30 30 30 38 30 30 30 30 30 31 30 30   // 4131000800000100
                                                                                                                                                                  30 31 30 30 36 31 33 33 39 39 61 66 66 31 38 65   // 0100613399aff18e
                                                                                                                                                                  66 31 61 32 63 32 35 31 34 61 32 37 33 61 34 32   // f1a2c2514a273a42
                                                                                                                                                                  64 39 30 34 32 62 37 32 33 32 31 66 31 37 35 37   // d9042b72321f1757
                                                                                                                                                                  31 30 32 64 66 39 65 62 61 64 61 36 39 39 32 33   // 102df9ebada69923
                                                                                                                                                                  65 32 37 33 38 34 30 36 63 32 31 65 35 62 38 30   // e2738406c21e5b80
                                                                                                                                                                  31 35 35 32 61 62 38 64 32 30 30 61 36 35 61 32   // 1552ab8d200a65a2
                                                                                                                                                                  33 35 65 30 30 31 61 63 39 61 64 63 32 35 66 32   // 35e001ac9adc25f2
                                                                                                                                                                  64 38 31 31 65 62 30 39 34 39 36 61 34 63 36 61   // d811eb09496a4c6a
                                                                                                                                                                  35 39 64 34 36 31 39 35 38 39 63 36 39 66 35 62   // 59d4619589c69f5b
                                                                                                                                                                  61 66 30 63 34 31 37 39 61 34 37 33 31 31 64 39   // af0c4179a47311d9
                                                                                                                                                                  32 35 35 35 63 64 30 30 36 61 63 63 38 62 35 39   // 2555cd006acc8b59
                                                                                                                                                                  35 39 66 32 62 64 36 65 31 30 65 33 36 30 63 33   // 59f2bd6e10e360c3
                                                                                                                                                                  34 35 33 37 61 31 64 32 36 36 64 61 38 30 38 35   // 4537a1d266da8085
                                                                                                                                                                  38 35 36 35 38 33 63 38 35 64 38 31 64 61 37 66   // 856583c85d81da7f
                                                                                                                                                                  33 65 63 30 31 65 64 39 35 36 34 63 35 38 64 39   // 3ec01ed9564c58d9
                                                                                                                                                                  33 64 37 31 33 63 64 30 31 37 32 63 38 65 32 33   // 3d713cd0172c8e23
                                                                                                                                                                  61 31 30 66 30 32 33 39 62 38 30 63 39 36 62 30   // a10f0239b80c96b0
                                                                                                                                                                  37 37 33 36 66 35 64 38 62 30 32 32 35 34 32 61   // 7736f5d8b022542a
                                                                                                                                                                  34 65 37 34 32 35 31 61 35 66 34 33 32 38 32 34   // 4e74251a5f432824
                                                                                                                                                                  33 31 38 62 33 35 33 39 61 35 61 30 38 37 66 38   // 318b3539a5a087f8
                                                                                                                                                                  65 35 33 64 32 66 31 33 35 66 39 63 61 34 37 66   // e53d2f135f9ca47f
                                                                                                                                                                  33 62 62 32 65 31 30 61 66 66 30 61 66 30 38 34   // 3bb2e10aff0af084
                                                                                                                                                                  39 35 30 34 66 62 37 63 65 61 33 66 66 31 39 32   // 9504fb7cea3ff192
                                                                                                                                                                  64 63 38 64 65 30 65 64 61 64 36 34 63 36 38 65   // dc8de0edad64c68e
                                                                                                                                                                  66 64 65 33 34 63 35 36 64 33 30 32 61 64 35 35   // fde34c56d302ad55
                                                                                                                                                                  66 64 36 65 38 30 66 33 30 32 64 35 65 66 63 64   // fd6e80f302d5efcd
                                                                                                                                                                  65 61 65 39 35 33 36 35 38 64 33 34 35 32 35 36   // eae953658d345256
                                                                                                                                                                  31 62 35 66 33 36 63 35 34 32 65 66 64 62 64 64   // 1b5f36c542efdbdd
                                                                                                                                                                  39 66 38 38 38 35 33 38 64 33 37 34 63 65 66 31   // 9f888538d374cef1
                                                                                                                                                                  30 36 61 63 66 37 64 39 33 61 34 34 34 35 63 33   // 06acf7d93a4445c3
                                                                                                                                                                  63 37 33 63 64 39 31 31 66 30 35 37 31 61 61 66   // c73cd911f0571aaf
                                                                                                                                                                  33 64 35 34 64 61 31 32 62 31 31 64 64 65 63 33   // 3d54da12b11ddec3
                                                                                                                                                                  37 35 62 33 81 00 61 35 61 38 36 36 65 31 65 65   // 75b3..a5a866e1ee
                                                                                                                                                                  31 38 36 66 38 30 37 36 36 38 32 30 39 66 33 62   // 186f807668209f3b
                                                                                                                                                                  31 31 32 33 36 61 63 65 35 65 32 31 66 31 31 37   // 11236ace5e21f117
                                                                                                                                                                  38 30 33 61 33 31 34 33 61 62 62 31 32 36 64 64   // 803a3143abb126dd
                                                                                                                                                                  30 33 35 64 37 64 32 66 38 37 36 62 36 39 33 38   // 035d7d2f876b6938
                                                                                                                                                                  61 61 66 32 65 65 33 34 31 34 64 35 34 32 30 64   // aaf2ee3414d5420d
                                                                                                                                                                  37 35 33 36 32 31 34 30 30 64 62 34 34 61 34 39   // 753621400db44a49
                                                                                                                                                                  63 34 38 36 63 65 31 33 34 33 30 30 61 32 31 30   // c486ce134300a210
                                                                                                                                                                  36 61 64 62 36 62 64 62 34 33 33 35 39 30 66 65   // 6adb6bdb433590fe
                                                                                                                                                                  66 38 61 64 35 63 34 33 63 62 61 38 32 32 39 30   // f8ad5c43cba82290
                                                                                                                                                                  64 63 34 39 35 33 30 65 66 66 64 38 36 35 32 33   // dc49530effd86523
                                                                                                                                                                  64 39 34 38 33 63 30 30 66 34 35 38 61 66 34 36   // d9483c00f458af46
                                                                                                                                                                  38 39 30 30 33 36 62 30 65 32 63 36 31 64 30 37   // 890036b0e2c61d07
                                                                                                                                                                  37 64 37 66 62 61 63 34 36 37 61 35 30 36 65 62   // 7d7fbac467a506eb
                                                                                                                                                                  61 32 39 65 34 36 37 61 38 37 31 39 38 62 30 35   // a29e467a87198b05
                                                                                                                                                                  33 63 37 34 39 61 61 32 61 34 64 32 38 34 30 63   // 3c749aa2a4d2840c
                                                                                                                                                                  37 38 34 65 36 64 00 00 )                         // 784e6d..
  .custom /*0C000007:0A00000A*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000026*/::.ctor() /* 0A00000A */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000008:0A00000B*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000027*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000027*//DebuggingModes/*01000028*/) /* 0A00000B */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000009:0A00000C*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000029*/::.ctor(int32) /* 0A00000C */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C00000A:0A00000D*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*0100002A*/::.ctor(string) /* 0A00000D */ = ( 01 00 15 53 79 73 74 65 6D 2E 4D 61 6E 61 67 65   // ...System.Manage
                                                                                                                                                                  6D 65 6E 74 2E 64 6C 6C 00 00 )                   // ment.dll..
  .custom /*0C00000B:0A00000E*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100002B*/::.ctor(string) /* 0A00000E */ = ( 01 00 0F 34 2E 30 2E 33 30 33 31 39 2E 31 38 34   // ...4.0.30319.184
                                                                                                                                                                          30 38 00 00 )                                     // 08..
  .custom /*0C00000C:0A00000F*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*0100002C*/::.ctor(bool) /* 0A00000F */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000010*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100002D*/::.ctor(string) /* 0A000010 */ = ( 01 00 15 53 79 73 74 65 6D 2E 4D 61 6E 61 67 65   // ...System.Manage
                                                                                                                                                           6D 65 6E 74 2E 64 6C 6C 00 00 )                   // ment.dll..
  .custom /*0C00000E:0A000011*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100002E*/::.ctor(string) /* 0A000011 */ = ( 01 00 15 53 79 73 74 65 6D 2E 4D 61 6E 61 67 65   // ...System.Manage
                                                                                                                                                                 6D 65 6E 74 2E 64 6C 6C 00 00 )                   // ment.dll..
  .custom /*0C00000F:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*0100002F*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000010:0A000013*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000030*/::.ctor(string) /* 0A000013 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                             70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A000014*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000031*/::.ctor(string) /* 0A000014 */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                             4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C000012:0A000015*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000032*/::.ctor(string) /* 0A000015 */ = ( 01 00 2F C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ../.. Microsoft 
                                                                                                                                                               43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 20 41 6C   // Corporation.  Al
                                                                                                                                                               6C 20 72 69 67 68 74 73 20 72 65 73 65 72 76 65   // l rights reserve
                                                                                                                                                               64 2E 00 00 )                                     // d...
  .custom /*0C000013:0A000016*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000033*/::.ctor(string) /* 0A000016 */ = ( 01 00 0F 34 2E 30 2E 33 30 33 31 39 2E 31 38 34   // ...4.0.30319.184
                                                                                                                                                                 30 38 00 00 )                                     // 08..
  .custom /*0C000014:0A000017*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000034*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .permissionset reqmin
             = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'UnmanagedCode' = bool(true)},
                class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'SkipVerification' = bool(true)}}
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
  .ver 4:0:0:0
}
.module System.Management.dll
// MVID: {14186a7d-b922-4d81-bf24-710e9a32e629}
.custom /*0C000001:0A000018*/ instance void [mscorlib/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000035*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
.imagebase 0x63c00000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007FB583647000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed System.Management.TextFormat
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Management.TextFormat/*02000002*/ Mof = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Management.TextFormat/*02000002*/ CimDtd20 = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Management.TextFormat/*02000002*/ WmiDtd20 = int32(0x00000002)
} // end of class System.Management.TextFormat

.class /*02000003*/ public auto ansi sealed System.Management.CimType
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Management.CimType/*02000003*/ None = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Management.CimType/*02000003*/ SInt8 = int32(0x00000010)
  .field /*04000008*/ public static literal valuetype System.Management.CimType/*02000003*/ UInt8 = int32(0x00000011)
  .field /*04000009*/ public static literal valuetype System.Management.CimType/*02000003*/ SInt16 = int32(0x00000002)
  .field /*0400000A*/ public static literal valuetype System.Management.CimType/*02000003*/ UInt16 = int32(0x00000012)
  .field /*0400000B*/ public static literal valuetype System.Management.CimType/*02000003*/ SInt32 = int32(0x00000003)
  .field /*0400000C*/ public static literal valuetype System.Management.CimType/*02000003*/ UInt32 = int32(0x00000013)
  .field /*0400000D*/ public static literal valuetype System.Management.CimType/*02000003*/ SInt64 = int32(0x00000014)
  .field /*0400000E*/ public static literal valuetype System.Management.CimType/*02000003*/ UInt64 = int32(0x00000015)
  .field /*0400000F*/ public static literal valuetype System.Management.CimType/*02000003*/ Real32 = int32(0x00000004)
  .field /*04000010*/ public static literal valuetype System.Management.CimType/*02000003*/ Real64 = int32(0x00000005)
  .field /*04000011*/ public static literal valuetype System.Management.CimType/*02000003*/ Boolean = int32(0x0000000B)
  .field /*04000012*/ public static literal valuetype System.Management.CimType/*02000003*/ String = int32(0x00000008)
  .field /*04000013*/ public static literal valuetype System.Management.CimType/*02000003*/ DateTime = int32(0x00000065)
  .field /*04000014*/ public static literal valuetype System.Management.CimType/*02000003*/ Reference = int32(0x00000066)
  .field /*04000015*/ public static literal valuetype System.Management.CimType/*02000003*/ Char16 = int32(0x00000067)
  .field /*04000016*/ public static literal valuetype System.Management.CimType/*02000003*/ Object = int32(0x0000000D)
} // end of class System.Management.CimType

.class /*02000004*/ public auto ansi sealed System.Management.ComparisonSettings
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .custom /*0C000015:0A000019*/ instance void [mscorlib/*23000001*/]System.FlagsAttribute/*01000036*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
  .field /*04000017*/ public specialname rtspecialname int32 value__
  .field /*04000018*/ public static literal valuetype System.Management.ComparisonSettings/*02000004*/ IncludeAll = int32(0x00000000)
  .field /*04000019*/ public static literal valuetype System.Management.ComparisonSettings/*02000004*/ IgnoreQualifiers = int32(0x00000001)
  .field /*0400001A*/ public static literal valuetype System.Management.ComparisonSettings/*02000004*/ IgnoreObjectSource = int32(0x00000002)
  .field /*0400001B*/ public static literal valuetype System.Management.ComparisonSettings/*02000004*/ IgnoreDefaultValues = int32(0x00000004)
  .field /*0400001C*/ public static literal valuetype System.Management.ComparisonSettings/*02000004*/ IgnoreClass = int32(0x00000008)
  .field /*0400001D*/ public static literal valuetype System.Management.ComparisonSettings/*02000004*/ IgnoreCase = int32(0x00000010)
  .field /*0400001E*/ public static literal valuetype System.Management.ComparisonSettings/*02000004*/ IgnoreFlavor = int32(0x00000020)
} // end of class System.Management.ComparisonSettings

.class /*02000005*/ public auto ansi serializable beforefieldinit System.Management.ManagementBaseObject
       extends [System/*23000002*/]System.ComponentModel.Component/*01000002*/
       implements [mscorlib/*23000001*/]System.ICloneable/*01000006*/,
                  [mscorlib/*23000001*/]System.Runtime.Serialization.ISerializable/*01000003*/
{
  .custom /*0C000016:0A00001A*/ instance void [mscorlib/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000037*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .custom /*0C000017:0A00001B*/ instance void [System/*23000002*/]System.ComponentModel.ToolboxItemAttribute/*01000038*/::.ctor(bool) /* 0A00001B */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    // Method begins at RVA 0x2050
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::.ctor

  .method /*06000002*/ public hidebysig instance void 
          Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2051
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::Dispose

  .method /*06000003*/ public hidebysig specialname static 
          native int  op_Explicit(class System.Management.ManagementBaseObject/*02000005*/ managementObject) cil managed
  // SIG: 00 01 18 12 14
  {
    // Method begins at RVA 0x2052
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::op_Explicit

  .method /*06000004*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                                                                                  valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'SerializationFormatter' = bool(true)}}
    .override [mscorlib/*23000001*/]System.Runtime.Serialization.ISerializable/*01000003*/::GetObjectData /*01000003::0A000001*/ 
    // Method begins at RVA 0x2053
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::System.Runtime.Serialization.ISerializable.GetObjectData

  .method /*06000005*/ family hidebysig newslot virtual 
          instance void  GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                                       valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    // Method begins at RVA 0x2054
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::GetObjectData

  .method /*06000006*/ public hidebysig newslot virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2055
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::Clone

  .method /*06000007*/ public hidebysig newslot specialname virtual 
          instance class System.Management.PropertyDataCollection/*02000037*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 80 DC
  {
    // Method begins at RVA 0x2056
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::get_Properties

  .method /*06000008*/ public hidebysig newslot specialname virtual 
          instance class System.Management.PropertyDataCollection/*02000037*/ 
          get_SystemProperties() cil managed
  // SIG: 20 00 12 80 DC
  {
    // Method begins at RVA 0x2057
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::get_SystemProperties

  .method /*06000009*/ public hidebysig newslot specialname virtual 
          instance class System.Management.QualifierDataCollection/*0200003A*/ 
          get_Qualifiers() cil managed
  // SIG: 20 00 12 80 E8
  {
    // Method begins at RVA 0x2058
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::get_Qualifiers

  .method /*0600000A*/ public hidebysig newslot specialname virtual 
          instance class System.Management.ManagementPath/*02000029*/ 
          get_ClassPath() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x2059
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::get_ClassPath

  .method /*0600000B*/ public hidebysig specialname 
          instance object  get_Item(string propertyName) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x205a
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::get_Item

  .method /*0600000C*/ public hidebysig specialname 
          instance void  set_Item(string propertyName,
                                  object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x205b
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::set_Item

  .method /*0600000D*/ public hidebysig instance object 
          GetPropertyValue(string propertyName) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x205c
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::GetPropertyValue

  .method /*0600000E*/ public hidebysig instance object 
          GetQualifierValue(string qualifierName) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x205d
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::GetQualifierValue

  .method /*0600000F*/ public hidebysig instance void 
          SetQualifierValue(string qualifierName,
                            object qualifierValue) cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x205e
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::SetQualifierValue

  .method /*06000010*/ public hidebysig instance object 
          GetPropertyQualifierValue(string propertyName,
                                    string qualifierName) cil managed
  // SIG: 20 02 1C 0E 0E
  {
    // Method begins at RVA 0x205f
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::GetPropertyQualifierValue

  .method /*06000011*/ public hidebysig instance void 
          SetPropertyQualifierValue(string propertyName,
                                    string qualifierName,
                                    object qualifierValue) cil managed
  // SIG: 20 03 01 0E 0E 1C
  {
    // Method begins at RVA 0x2060
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::SetPropertyQualifierValue

  .method /*06000012*/ public hidebysig instance string 
          GetText(valuetype System.Management.TextFormat/*02000002*/ format) cil managed
  // SIG: 20 01 0E 11 08
  {
    // Method begins at RVA 0x2061
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::GetText

  .method /*06000013*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x2062
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::Equals

  .method /*06000014*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2063
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::GetHashCode

  .method /*06000015*/ public hidebysig instance bool 
          CompareTo(class System.Management.ManagementBaseObject/*02000005*/ otherObject,
                    valuetype System.Management.ComparisonSettings/*02000004*/ settings) cil managed
  // SIG: 20 02 02 12 14 11 10
  {
    // Method begins at RVA 0x2064
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::CompareTo

  .method /*06000016*/ public hidebysig instance void 
          SetPropertyValue(string propertyName,
                           object propertyValue) cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x2065
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::SetPropertyValue

  .method /*06000017*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2066
    // Code size       0 (0x0)
  } // end of method ManagementBaseObject::.cctor

  .property /*17000001*/ instance callconv(8) class System.Management.PropertyDataCollection/*02000037*/
          Properties()
  {
    .get instance class System.Management.PropertyDataCollection/*02000037*/ System.Management.ManagementBaseObject/*02000005*/::get_Properties() /* 06000007 */
  } // end of property ManagementBaseObject::Properties
  .property /*17000002*/ instance callconv(8) class System.Management.PropertyDataCollection/*02000037*/
          SystemProperties()
  {
    .get instance class System.Management.PropertyDataCollection/*02000037*/ System.Management.ManagementBaseObject/*02000005*/::get_SystemProperties() /* 06000008 */
  } // end of property ManagementBaseObject::SystemProperties
  .property /*17000003*/ instance callconv(8) class System.Management.QualifierDataCollection/*0200003A*/
          Qualifiers()
  {
    .get instance class System.Management.QualifierDataCollection/*0200003A*/ System.Management.ManagementBaseObject/*02000005*/::get_Qualifiers() /* 06000009 */
  } // end of property ManagementBaseObject::Qualifiers
  .property /*17000004*/ instance callconv(8) class System.Management.ManagementPath/*02000029*/
          ClassPath()
  {
    .get instance class System.Management.ManagementPath/*02000029*/ System.Management.ManagementBaseObject/*02000005*/::get_ClassPath() /* 0600000A */
  } // end of property ManagementBaseObject::ClassPath
  .property /*17000005*/ instance callconv(8) object
          Item(string)
  {
    .set instance void System.Management.ManagementBaseObject/*02000005*/::set_Item(string,
                                                                                    object) /* 0600000C */
    .get instance object System.Management.ManagementBaseObject/*02000005*/::get_Item(string) /* 0600000B */
  } // end of property ManagementBaseObject::Item
} // end of class System.Management.ManagementBaseObject

.class /*02000006*/ public auto ansi serializable beforefieldinit System.Management.ManagementObject
       extends System.Management.ManagementBaseObject/*02000005*/
       implements [mscorlib/*23000001*/]System.ICloneable/*01000006*/
{
  .method /*06000018*/ family hidebysig virtual 
          instance void  GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                                       valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    // Method begins at RVA 0x2067
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetObjectData

  .method /*06000019*/ public hidebysig instance void 
          Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2068
    // Code size       0 (0x0)
  } // end of method ManagementObject::Dispose

  .method /*0600001A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2069
    // Code size       0 (0x0)
  } // end of method ManagementObject::.ctor

  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementPath/*02000029*/ path) cil managed
  // SIG: 20 01 01 12 80 A4
  {
    // Method begins at RVA 0x206a
    // Code size       0 (0x0)
  } // end of method ManagementObject::.ctor

  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string path) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x206b
    // Code size       0 (0x0)
  } // end of method ManagementObject::.ctor

  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementPath/*02000029*/ path,
                               class System.Management.ObjectGetOptions/*02000024*/ options) cil managed
  // SIG: 20 02 01 12 80 A4 12 80 90
  {
    // Method begins at RVA 0x206c
    // Code size       0 (0x0)
  } // end of method ManagementObject::.ctor

  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string path,
                               class System.Management.ObjectGetOptions/*02000024*/ options) cil managed
  // SIG: 20 02 01 0E 12 80 90
  {
    // Method begins at RVA 0x206d
    // Code size       0 (0x0)
  } // end of method ManagementObject::.ctor

  .method /*0600001F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementScope/*02000034*/ scope,
                               class System.Management.ManagementPath/*02000029*/ path,
                               class System.Management.ObjectGetOptions/*02000024*/ options) cil managed
  // SIG: 20 03 01 12 80 D0 12 80 A4 12 80 90
  {
    // Method begins at RVA 0x206e
    // Code size       0 (0x0)
  } // end of method ManagementObject::.ctor

  .method /*06000020*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scopeString,
                               string pathString,
                               class System.Management.ObjectGetOptions/*02000024*/ options) cil managed
  // SIG: 20 03 01 0E 0E 12 80 90
  {
    // Method begins at RVA 0x206f
    // Code size       0 (0x0)
  } // end of method ManagementObject::.ctor

  .method /*06000021*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    // Method begins at RVA 0x2070
    // Code size       0 (0x0)
  } // end of method ManagementObject::.ctor

  .method /*06000022*/ public hidebysig specialname 
          instance class System.Management.ManagementScope/*02000034*/ 
          get_Scope() cil managed
  // SIG: 20 00 12 80 D0
  {
    // Method begins at RVA 0x2071
    // Code size       0 (0x0)
  } // end of method ManagementObject::get_Scope

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_Scope(class System.Management.ManagementScope/*02000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D0
  {
    // Method begins at RVA 0x2072
    // Code size       0 (0x0)
  } // end of method ManagementObject::set_Scope

  .method /*06000024*/ public hidebysig newslot specialname virtual 
          instance class System.Management.ManagementPath/*02000029*/ 
          get_Path() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x2073
    // Code size       0 (0x0)
  } // end of method ManagementObject::get_Path

  .method /*06000025*/ public hidebysig newslot specialname virtual 
          instance void  set_Path(class System.Management.ManagementPath/*02000029*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A4
  {
    // Method begins at RVA 0x2074
    // Code size       0 (0x0)
  } // end of method ManagementObject::set_Path

  .method /*06000026*/ public hidebysig specialname 
          instance class System.Management.ObjectGetOptions/*02000024*/ 
          get_Options() cil managed
  // SIG: 20 00 12 80 90
  {
    // Method begins at RVA 0x2075
    // Code size       0 (0x0)
  } // end of method ManagementObject::get_Options

  .method /*06000027*/ public hidebysig specialname 
          instance void  set_Options(class System.Management.ObjectGetOptions/*02000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x2076
    // Code size       0 (0x0)
  } // end of method ManagementObject::set_Options

  .method /*06000028*/ public hidebysig specialname virtual 
          instance class System.Management.ManagementPath/*02000029*/ 
          get_ClassPath() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x2077
    // Code size       0 (0x0)
  } // end of method ManagementObject::get_ClassPath

  .method /*06000029*/ public hidebysig instance void 
          Get() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2078
    // Code size       0 (0x0)
  } // end of method ManagementObject::Get

  .method /*0600002A*/ public hidebysig instance void 
          Get(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x2079
    // Code size       0 (0x0)
  } // end of method ManagementObject::Get

  .method /*0600002B*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelated() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x207a
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelated

  .method /*0600002C*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelated(string relatedClass) cil managed
  // SIG: 20 01 12 58 0E
  {
    // Method begins at RVA 0x207c
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelated

  .method /*0600002D*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelated(string relatedClass,
                     string relationshipClass,
                     string relationshipQualifier,
                     string relatedQualifier,
                     string relatedRole,
                     string thisRole,
                     bool classDefinitionsOnly,
                     class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 08 12 58 0E 0E 0E 0E 0E 0E 02 12 80 88
  {
    // Method begins at RVA 0x2088
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelated

  .method /*0600002E*/ public hidebysig instance void 
          GetRelated(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x2089
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelated

  .method /*0600002F*/ public hidebysig instance void 
          GetRelated(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                     string relatedClass) cil managed
  // SIG: 20 02 01 12 74 0E
  {
    // Method begins at RVA 0x208c
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelated

  .method /*06000030*/ public hidebysig instance void 
          GetRelated(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                     string relatedClass,
                     string relationshipClass,
                     string relationshipQualifier,
                     string relatedQualifier,
                     string relatedRole,
                     string thisRole,
                     bool classDefinitionsOnly,
                     class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 09 01 12 74 0E 0E 0E 0E 0E 0E 02 12 80 88
  {
    // Method begins at RVA 0x2098
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelated

  .method /*06000031*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelationships() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x2099
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelationships

  .method /*06000032*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelationships(string relationshipClass) cil managed
  // SIG: 20 01 12 58 0E
  {
    // Method begins at RVA 0x209a
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelationships

  .method /*06000033*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelationships(string relationshipClass,
                           string relationshipQualifier,
                           string thisRole,
                           bool classDefinitionsOnly,
                           class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 05 12 58 0E 0E 0E 02 12 80 88
  {
    // Method begins at RVA 0x209b
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelationships

  .method /*06000034*/ public hidebysig instance void 
          GetRelationships(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x209c
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelationships

  .method /*06000035*/ public hidebysig instance void 
          GetRelationships(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                           string relationshipClass) cil managed
  // SIG: 20 02 01 12 74 0E
  {
    // Method begins at RVA 0x209d
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelationships

  .method /*06000036*/ public hidebysig instance void 
          GetRelationships(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                           string relationshipClass,
                           string relationshipQualifier,
                           string thisRole,
                           bool classDefinitionsOnly,
                           class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 06 01 12 74 0E 0E 0E 02 12 80 88
  {
    // Method begins at RVA 0x209e
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetRelationships

  .method /*06000037*/ public hidebysig instance class System.Management.ManagementPath/*02000029*/ 
          Put() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x209f
    // Code size       0 (0x0)
  } // end of method ManagementObject::Put

  .method /*06000038*/ public hidebysig instance class System.Management.ManagementPath/*02000029*/ 
          Put(class System.Management.PutOptions/*02000025*/ options) cil managed
  // SIG: 20 01 12 80 A4 12 80 94
  {
    // Method begins at RVA 0x20a0
    // Code size       0 (0x0)
  } // end of method ManagementObject::Put

  .method /*06000039*/ public hidebysig instance void 
          Put(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x20a1
    // Code size       0 (0x0)
  } // end of method ManagementObject::Put

  .method /*0600003A*/ public hidebysig instance void 
          Put(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
              class System.Management.PutOptions/*02000025*/ options) cil managed
  // SIG: 20 02 01 12 74 12 80 94
  {
    // Method begins at RVA 0x20a2
    // Code size       0 (0x0)
  } // end of method ManagementObject::Put

  .method /*0600003B*/ public hidebysig instance class System.Management.ManagementPath/*02000029*/ 
          CopyTo(class System.Management.ManagementPath/*02000029*/ path) cil managed
  // SIG: 20 01 12 80 A4 12 80 A4
  {
    // Method begins at RVA 0x20a3
    // Code size       0 (0x0)
  } // end of method ManagementObject::CopyTo

  .method /*0600003C*/ public hidebysig instance class System.Management.ManagementPath/*02000029*/ 
          CopyTo(string path) cil managed
  // SIG: 20 01 12 80 A4 0E
  {
    // Method begins at RVA 0x20a4
    // Code size       0 (0x0)
  } // end of method ManagementObject::CopyTo

  .method /*0600003D*/ public hidebysig instance class System.Management.ManagementPath/*02000029*/ 
          CopyTo(string path,
                 class System.Management.PutOptions/*02000025*/ options) cil managed
  // SIG: 20 02 12 80 A4 0E 12 80 94
  {
    // Method begins at RVA 0x20a5
    // Code size       0 (0x0)
  } // end of method ManagementObject::CopyTo

  .method /*0600003E*/ public hidebysig instance class System.Management.ManagementPath/*02000029*/ 
          CopyTo(class System.Management.ManagementPath/*02000029*/ path,
                 class System.Management.PutOptions/*02000025*/ options) cil managed
  // SIG: 20 02 12 80 A4 12 80 A4 12 80 94
  {
    // Method begins at RVA 0x20a6
    // Code size       0 (0x0)
  } // end of method ManagementObject::CopyTo

  .method /*0600003F*/ public hidebysig instance void 
          CopyTo(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                 class System.Management.ManagementPath/*02000029*/ path) cil managed
  // SIG: 20 02 01 12 74 12 80 A4
  {
    // Method begins at RVA 0x20a7
    // Code size       0 (0x0)
  } // end of method ManagementObject::CopyTo

  .method /*06000040*/ public hidebysig instance void 
          CopyTo(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                 string path) cil managed
  // SIG: 20 02 01 12 74 0E
  {
    // Method begins at RVA 0x20a8
    // Code size       0 (0x0)
  } // end of method ManagementObject::CopyTo

  .method /*06000041*/ public hidebysig instance void 
          CopyTo(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                 string path,
                 class System.Management.PutOptions/*02000025*/ options) cil managed
  // SIG: 20 03 01 12 74 0E 12 80 94
  {
    // Method begins at RVA 0x20a9
    // Code size       0 (0x0)
  } // end of method ManagementObject::CopyTo

  .method /*06000042*/ public hidebysig instance void 
          CopyTo(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                 class System.Management.ManagementPath/*02000029*/ path,
                 class System.Management.PutOptions/*02000025*/ options) cil managed
  // SIG: 20 03 01 12 74 12 80 A4 12 80 94
  {
    // Method begins at RVA 0x20aa
    // Code size       0 (0x0)
  } // end of method ManagementObject::CopyTo

  .method /*06000043*/ public hidebysig instance void 
          Delete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ab
    // Code size       0 (0x0)
  } // end of method ManagementObject::Delete

  .method /*06000044*/ public hidebysig instance void 
          Delete(class System.Management.DeleteOptions/*02000026*/ options) cil managed
  // SIG: 20 01 01 12 80 98
  {
    // Method begins at RVA 0x20ac
    // Code size       0 (0x0)
  } // end of method ManagementObject::Delete

  .method /*06000045*/ public hidebysig instance void 
          Delete(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x20ad
    // Code size       0 (0x0)
  } // end of method ManagementObject::Delete

  .method /*06000046*/ public hidebysig instance void 
          Delete(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                 class System.Management.DeleteOptions/*02000026*/ options) cil managed
  // SIG: 20 02 01 12 74 12 80 98
  {
    // Method begins at RVA 0x20ae
    // Code size       0 (0x0)
  } // end of method ManagementObject::Delete

  .method /*06000047*/ public hidebysig instance object 
          InvokeMethod(string methodName,
                       object[] args) cil managed
  // SIG: 20 02 1C 0E 1D 1C
  {
    // Method begins at RVA 0x20af
    // Code size       0 (0x0)
  } // end of method ManagementObject::InvokeMethod

  .method /*06000048*/ public hidebysig instance void 
          InvokeMethod(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                       string methodName,
                       object[] args) cil managed
  // SIG: 20 03 01 12 74 0E 1D 1C
  {
    // Method begins at RVA 0x20b0
    // Code size       0 (0x0)
  } // end of method ManagementObject::InvokeMethod

  .method /*06000049*/ public hidebysig instance class System.Management.ManagementBaseObject/*02000005*/ 
          InvokeMethod(string methodName,
                       class System.Management.ManagementBaseObject/*02000005*/ inParameters,
                       class System.Management.InvokeMethodOptions/*02000027*/ options) cil managed
  // SIG: 20 03 12 14 0E 12 14 12 80 9C
  {
    // Method begins at RVA 0x20b1
    // Code size       0 (0x0)
  } // end of method ManagementObject::InvokeMethod

  .method /*0600004A*/ public hidebysig instance void 
          InvokeMethod(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                       string methodName,
                       class System.Management.ManagementBaseObject/*02000005*/ inParameters,
                       class System.Management.InvokeMethodOptions/*02000027*/ options) cil managed
  // SIG: 20 04 01 12 74 0E 12 14 12 80 9C
  {
    // Method begins at RVA 0x20b2
    // Code size       0 (0x0)
  } // end of method ManagementObject::InvokeMethod

  .method /*0600004B*/ public hidebysig instance class System.Management.ManagementBaseObject/*02000005*/ 
          GetMethodParameters(string methodName) cil managed
  // SIG: 20 01 12 14 0E
  {
    // Method begins at RVA 0x20b3
    // Code size       0 (0x0)
  } // end of method ManagementObject::GetMethodParameters

  .method /*0600004C*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20b4
    // Code size       0 (0x0)
  } // end of method ManagementObject::Clone

  .method /*0600004D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20b5
    // Code size       0 (0x0)
  } // end of method ManagementObject::ToString

  .property /*17000006*/ instance callconv(8) class System.Management.ManagementScope/*02000034*/
          Scope()
  {
    .set instance void System.Management.ManagementObject/*02000006*/::set_Scope(class System.Management.ManagementScope/*02000034*/) /* 06000023 */
    .get instance class System.Management.ManagementScope/*02000034*/ System.Management.ManagementObject/*02000006*/::get_Scope() /* 06000022 */
  } // end of property ManagementObject::Scope
  .property /*17000007*/ instance callconv(8) class System.Management.ManagementPath/*02000029*/
          Path()
  {
    .set instance void System.Management.ManagementObject/*02000006*/::set_Path(class System.Management.ManagementPath/*02000029*/) /* 06000025 */
    .get instance class System.Management.ManagementPath/*02000029*/ System.Management.ManagementObject/*02000006*/::get_Path() /* 06000024 */
  } // end of property ManagementObject::Path
  .property /*17000008*/ instance callconv(8) class System.Management.ObjectGetOptions/*02000024*/
          Options()
  {
    .set instance void System.Management.ManagementObject/*02000006*/::set_Options(class System.Management.ObjectGetOptions/*02000024*/) /* 06000027 */
    .get instance class System.Management.ObjectGetOptions/*02000024*/ System.Management.ManagementObject/*02000006*/::get_Options() /* 06000026 */
  } // end of property ManagementObject::Options
  .property /*17000009*/ instance callconv(8) class System.Management.ManagementPath/*02000029*/
          ClassPath()
  {
    .get instance class System.Management.ManagementPath/*02000029*/ System.Management.ManagementObject/*02000006*/::get_ClassPath() /* 06000028 */
  } // end of property ManagementObject::ClassPath
} // end of class System.Management.ManagementObject

.class /*02000007*/ public auto ansi serializable beforefieldinit System.Management.ManagementClass
       extends System.Management.ManagementObject/*02000006*/
{
  .method /*0600004E*/ family hidebysig virtual 
          instance void  GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                                       valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    // Method begins at RVA 0x20b6
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetObjectData

  .method /*0600004F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b7
    // Code size       0 (0x0)
  } // end of method ManagementClass::.ctor

  .method /*06000050*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementPath/*02000029*/ path) cil managed
  // SIG: 20 01 01 12 80 A4
  {
    // Method begins at RVA 0x20b8
    // Code size       0 (0x0)
  } // end of method ManagementClass::.ctor

  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string path) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20b9
    // Code size       0 (0x0)
  } // end of method ManagementClass::.ctor

  .method /*06000052*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementPath/*02000029*/ path,
                               class System.Management.ObjectGetOptions/*02000024*/ options) cil managed
  // SIG: 20 02 01 12 80 A4 12 80 90
  {
    // Method begins at RVA 0x20ba
    // Code size       0 (0x0)
  } // end of method ManagementClass::.ctor

  .method /*06000053*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string path,
                               class System.Management.ObjectGetOptions/*02000024*/ options) cil managed
  // SIG: 20 02 01 0E 12 80 90
  {
    // Method begins at RVA 0x20bb
    // Code size       0 (0x0)
  } // end of method ManagementClass::.ctor

  .method /*06000054*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementScope/*02000034*/ scope,
                               class System.Management.ManagementPath/*02000029*/ path,
                               class System.Management.ObjectGetOptions/*02000024*/ options) cil managed
  // SIG: 20 03 01 12 80 D0 12 80 A4 12 80 90
  {
    // Method begins at RVA 0x20bc
    // Code size       0 (0x0)
  } // end of method ManagementClass::.ctor

  .method /*06000055*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scope,
                               string path,
                               class System.Management.ObjectGetOptions/*02000024*/ options) cil managed
  // SIG: 20 03 01 0E 0E 12 80 90
  {
    // Method begins at RVA 0x20bd
    // Code size       0 (0x0)
  } // end of method ManagementClass::.ctor

  .method /*06000056*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    // Method begins at RVA 0x20be
    // Code size       0 (0x0)
  } // end of method ManagementClass::.ctor

  .method /*06000057*/ public hidebysig specialname virtual 
          instance class System.Management.ManagementPath/*02000029*/ 
          get_Path() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x20bf
    // Code size       0 (0x0)
  } // end of method ManagementClass::get_Path

  .method /*06000058*/ public hidebysig specialname virtual 
          instance void  set_Path(class System.Management.ManagementPath/*02000029*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A4
  {
    // Method begins at RVA 0x20c0
    // Code size       0 (0x0)
  } // end of method ManagementClass::set_Path

  .method /*06000059*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/ 
          get_Derivation() cil managed
  // SIG: 20 00 12 21
  {
    // Method begins at RVA 0x20c1
    // Code size       0 (0x0)
  } // end of method ManagementClass::get_Derivation

  .method /*0600005A*/ public hidebysig specialname 
          instance class System.Management.MethodDataCollection/*0200003D*/ 
          get_Methods() cil managed
  // SIG: 20 00 12 80 F4
  {
    // Method begins at RVA 0x20c2
    // Code size       0 (0x0)
  } // end of method ManagementClass::get_Methods

  .method /*0600005B*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetInstances() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x20c3
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetInstances

  .method /*0600005C*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetInstances(class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 01 12 58 12 80 88
  {
    // Method begins at RVA 0x20c4
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetInstances

  .method /*0600005D*/ public hidebysig instance void 
          GetInstances(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x20c5
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetInstances

  .method /*0600005E*/ public hidebysig instance void 
          GetInstances(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                       class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 02 01 12 74 12 80 88
  {
    // Method begins at RVA 0x20c6
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetInstances

  .method /*0600005F*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetSubclasses() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x20c7
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetSubclasses

  .method /*06000060*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetSubclasses(class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 01 12 58 12 80 88
  {
    // Method begins at RVA 0x20c8
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetSubclasses

  .method /*06000061*/ public hidebysig instance void 
          GetSubclasses(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x20c9
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetSubclasses

  .method /*06000062*/ public hidebysig instance void 
          GetSubclasses(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                        class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 02 01 12 74 12 80 88
  {
    // Method begins at RVA 0x20ca
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetSubclasses

  .method /*06000063*/ public hidebysig instance class System.Management.ManagementClass/*02000007*/ 
          Derive(string newClassName) cil managed
  // SIG: 20 01 12 1C 0E
  {
    // Method begins at RVA 0x20cb
    // Code size       0 (0x0)
  } // end of method ManagementClass::Derive

  .method /*06000064*/ public hidebysig instance class System.Management.ManagementObject/*02000006*/ 
          CreateInstance() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x20cc
    // Code size       0 (0x0)
  } // end of method ManagementClass::CreateInstance

  .method /*06000065*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20cd
    // Code size       0 (0x0)
  } // end of method ManagementClass::Clone

  .method /*06000066*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelatedClasses() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x20ce
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelatedClasses

  .method /*06000067*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelatedClasses(string relatedClass) cil managed
  // SIG: 20 01 12 58 0E
  {
    // Method begins at RVA 0x20cf
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelatedClasses

  .method /*06000068*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelatedClasses(string relatedClass,
                            string relationshipClass,
                            string relationshipQualifier,
                            string relatedQualifier,
                            string relatedRole,
                            string thisRole,
                            class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 07 12 58 0E 0E 0E 0E 0E 0E 12 80 88
  {
    // Method begins at RVA 0x20d0
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelatedClasses

  .method /*06000069*/ public hidebysig instance void 
          GetRelatedClasses(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x20d1
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelatedClasses

  .method /*0600006A*/ public hidebysig instance void 
          GetRelatedClasses(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                            string relatedClass) cil managed
  // SIG: 20 02 01 12 74 0E
  {
    // Method begins at RVA 0x20d4
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelatedClasses

  .method /*0600006B*/ public hidebysig instance void 
          GetRelatedClasses(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                            string relatedClass,
                            string relationshipClass,
                            string relationshipQualifier,
                            string relatedQualifier,
                            string relatedRole,
                            string thisRole,
                            class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 08 01 12 74 0E 0E 0E 0E 0E 0E 12 80 88
  {
    // Method begins at RVA 0x20e0
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelatedClasses

  .method /*0600006C*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelationshipClasses() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x20e1
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelationshipClasses

  .method /*0600006D*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelationshipClasses(string relationshipClass) cil managed
  // SIG: 20 01 12 58 0E
  {
    // Method begins at RVA 0x20e2
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelationshipClasses

  .method /*0600006E*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          GetRelationshipClasses(string relationshipClass,
                                 string relationshipQualifier,
                                 string thisRole,
                                 class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 04 12 58 0E 0E 0E 12 80 88
  {
    // Method begins at RVA 0x20e3
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelationshipClasses

  .method /*0600006F*/ public hidebysig instance void 
          GetRelationshipClasses(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x20e4
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelationshipClasses

  .method /*06000070*/ public hidebysig instance void 
          GetRelationshipClasses(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                                 string relationshipClass) cil managed
  // SIG: 20 02 01 12 74 0E
  {
    // Method begins at RVA 0x20e5
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelationshipClasses

  .method /*06000071*/ public hidebysig instance void 
          GetRelationshipClasses(class System.Management.ManagementOperationObserver/*0200001D*/ watcher,
                                 string relationshipClass,
                                 string relationshipQualifier,
                                 string thisRole,
                                 class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 05 01 12 74 0E 0E 0E 12 80 88
  {
    // Method begins at RVA 0x20e6
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetRelationshipClasses

  .method /*06000072*/ public hidebysig instance class [System/*23000002*/]System.CodeDom.CodeTypeDeclaration/*01000009*/ 
          GetStronglyTypedClassCode(bool includeSystemClassInClassDef,
                                    bool systemPropertyClass) cil managed
  // SIG: 20 02 12 25 02 02
  {
    // Method begins at RVA 0x20e7
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetStronglyTypedClassCode

  .method /*06000073*/ public hidebysig instance bool 
          GetStronglyTypedClassCode(valuetype System.Management.CodeLanguage/*0200003F*/ lang,
                                    string filePath,
                                    string classNamespace) cil managed
  // SIG: 20 03 02 11 80 FC 0E 0E
  {
    // Method begins at RVA 0x20e8
    // Code size       0 (0x0)
  } // end of method ManagementClass::GetStronglyTypedClassCode

  .property /*1700000A*/ instance callconv(8) class System.Management.ManagementPath/*02000029*/
          Path()
  {
    .set instance void System.Management.ManagementClass/*02000007*/::set_Path(class System.Management.ManagementPath/*02000029*/) /* 06000058 */
    .get instance class System.Management.ManagementPath/*02000029*/ System.Management.ManagementClass/*02000007*/::get_Path() /* 06000057 */
  } // end of property ManagementClass::Path
  .property /*1700000B*/ instance callconv(8) class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/
          Derivation()
  {
    .get instance class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/ System.Management.ManagementClass/*02000007*/::get_Derivation() /* 06000059 */
  } // end of property ManagementClass::Derivation
  .property /*1700000C*/ instance callconv(8) class System.Management.MethodDataCollection/*0200003D*/
          Methods()
  {
    .get instance class System.Management.MethodDataCollection/*0200003D*/ System.Management.ManagementClass/*02000007*/::get_Methods() /* 0600005A */
  } // end of property ManagementClass::Methods
} // end of class System.Management.ManagementClass

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Management.ManagementDateTimeConverter
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
{
  .method /*06000074*/ private hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e9
    // Code size       0 (0x0)
  } // end of method ManagementDateTimeConverter::.ctor

  .method /*06000075*/ public hidebysig static 
          valuetype [mscorlib/*23000001*/]System.DateTime/*0100000A*/ 
          ToDateTime(string dmtfDate) cil managed
  // SIG: 00 01 11 29 0E
  {
    // Method begins at RVA 0x20ea
    // Code size       0 (0x0)
  } // end of method ManagementDateTimeConverter::ToDateTime

  .method /*06000076*/ public hidebysig static 
          string  ToDmtfDateTime(valuetype [mscorlib/*23000001*/]System.DateTime/*0100000A*/ 'date') cil managed
  // SIG: 00 01 0E 11 29
  {
    // Method begins at RVA 0x20eb
    // Code size       0 (0x0)
  } // end of method ManagementDateTimeConverter::ToDmtfDateTime

  .method /*06000077*/ public hidebysig static 
          valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ 
          ToTimeSpan(string dmtfTimespan) cil managed
  // SIG: 00 01 11 2D 0E
  {
    // Method begins at RVA 0x20ec
    // Code size       0 (0x0)
  } // end of method ManagementDateTimeConverter::ToTimeSpan

  .method /*06000078*/ public hidebysig static 
          string  ToDmtfTimeInterval(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timespan) cil managed
  // SIG: 00 01 0E 11 2D
  {
    // Method begins at RVA 0x20ed
    // Code size       0 (0x0)
  } // end of method ManagementDateTimeConverter::ToDmtfTimeInterval

} // end of class System.Management.ManagementDateTimeConverter

.class /*02000009*/ public abstract auto ansi beforefieldinit System.Management.ManagementEventArgs
       extends [mscorlib/*23000001*/]System.EventArgs/*0100000C*/
{
  .method /*06000079*/ public hidebysig specialname 
          instance object  get_Context() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20ee
    // Code size       0 (0x0)
  } // end of method ManagementEventArgs::get_Context

  .method /*0600007A*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ef
    // Code size       0 (0x0)
  } // end of method ManagementEventArgs::.ctor

  .property /*1700000D*/ instance callconv(8) object
          Context()
  {
    .get instance object System.Management.ManagementEventArgs/*02000009*/::get_Context() /* 06000079 */
  } // end of property ManagementEventArgs::Context
} // end of class System.Management.ManagementEventArgs

.class /*0200000A*/ public auto ansi beforefieldinit System.Management.ObjectReadyEventArgs
       extends System.Management.ManagementEventArgs/*02000009*/
{
  .method /*0600007B*/ public hidebysig specialname 
          instance class System.Management.ManagementBaseObject/*02000005*/ 
          get_NewObject() cil managed
  // SIG: 20 00 12 14
  {
    // Method begins at RVA 0x20f0
    // Code size       0 (0x0)
  } // end of method ObjectReadyEventArgs::get_NewObject

  .method /*0600007C*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f1
    // Code size       0 (0x0)
  } // end of method ObjectReadyEventArgs::.ctor

  .property /*1700000E*/ instance callconv(8) class System.Management.ManagementBaseObject/*02000005*/
          NewObject()
  {
    .get instance class System.Management.ManagementBaseObject/*02000005*/ System.Management.ObjectReadyEventArgs/*0200000A*/::get_NewObject() /* 0600007B */
  } // end of property ObjectReadyEventArgs::NewObject
} // end of class System.Management.ObjectReadyEventArgs

.class /*0200000B*/ public auto ansi beforefieldinit System.Management.CompletedEventArgs
       extends System.Management.ManagementEventArgs/*02000009*/
{
  .method /*0600007D*/ public hidebysig specialname 
          instance class System.Management.ManagementBaseObject/*02000005*/ 
          get_StatusObject() cil managed
  // SIG: 20 00 12 14
  {
    // Method begins at RVA 0x20f2
    // Code size       0 (0x0)
  } // end of method CompletedEventArgs::get_StatusObject

  .method /*0600007E*/ public hidebysig specialname 
          instance valuetype System.Management.ManagementStatus/*02000013*/ 
          get_Status() cil managed
  // SIG: 20 00 11 4C
  {
    // Method begins at RVA 0x20f3
    // Code size       0 (0x0)
  } // end of method CompletedEventArgs::get_Status

  .method /*0600007F*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f4
    // Code size       0 (0x0)
  } // end of method CompletedEventArgs::.ctor

  .property /*1700000F*/ instance callconv(8) class System.Management.ManagementBaseObject/*02000005*/
          StatusObject()
  {
    .get instance class System.Management.ManagementBaseObject/*02000005*/ System.Management.CompletedEventArgs/*0200000B*/::get_StatusObject() /* 0600007D */
  } // end of property CompletedEventArgs::StatusObject
  .property /*17000010*/ instance callconv(8) valuetype System.Management.ManagementStatus/*02000013*/
          Status()
  {
    .get instance valuetype System.Management.ManagementStatus/*02000013*/ System.Management.CompletedEventArgs/*0200000B*/::get_Status() /* 0600007E */
  } // end of property CompletedEventArgs::Status
} // end of class System.Management.CompletedEventArgs

.class /*0200000C*/ public auto ansi beforefieldinit System.Management.ObjectPutEventArgs
       extends System.Management.ManagementEventArgs/*02000009*/
{
  .method /*06000080*/ public hidebysig specialname 
          instance class System.Management.ManagementPath/*02000029*/ 
          get_Path() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x20f5
    // Code size       0 (0x0)
  } // end of method ObjectPutEventArgs::get_Path

  .method /*06000081*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f6
    // Code size       0 (0x0)
  } // end of method ObjectPutEventArgs::.ctor

  .property /*17000011*/ instance callconv(8) class System.Management.ManagementPath/*02000029*/
          Path()
  {
    .get instance class System.Management.ManagementPath/*02000029*/ System.Management.ObjectPutEventArgs/*0200000C*/::get_Path() /* 06000080 */
  } // end of property ObjectPutEventArgs::Path
} // end of class System.Management.ObjectPutEventArgs

.class /*0200000D*/ public auto ansi beforefieldinit System.Management.ProgressEventArgs
       extends System.Management.ManagementEventArgs/*02000009*/
{
  .method /*06000082*/ public hidebysig specialname 
          instance int32  get_UpperBound() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20f7
    // Code size       0 (0x0)
  } // end of method ProgressEventArgs::get_UpperBound

  .method /*06000083*/ public hidebysig specialname 
          instance int32  get_Current() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20f8
    // Code size       0 (0x0)
  } // end of method ProgressEventArgs::get_Current

  .method /*06000084*/ public hidebysig specialname 
          instance string  get_Message() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20f9
    // Code size       0 (0x0)
  } // end of method ProgressEventArgs::get_Message

  .method /*06000085*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20fa
    // Code size       0 (0x0)
  } // end of method ProgressEventArgs::.ctor

  .property /*17000012*/ instance callconv(8) int32
          UpperBound()
  {
    .get instance int32 System.Management.ProgressEventArgs/*0200000D*/::get_UpperBound() /* 06000082 */
  } // end of property ProgressEventArgs::UpperBound
  .property /*17000013*/ instance callconv(8) int32
          Current()
  {
    .get instance int32 System.Management.ProgressEventArgs/*0200000D*/::get_Current() /* 06000083 */
  } // end of property ProgressEventArgs::Current
  .property /*17000014*/ instance callconv(8) string
          Message()
  {
    .get instance string System.Management.ProgressEventArgs/*0200000D*/::get_Message() /* 06000084 */
  } // end of property ProgressEventArgs::Message
} // end of class System.Management.ProgressEventArgs

.class /*0200000E*/ public auto ansi beforefieldinit System.Management.EventArrivedEventArgs
       extends System.Management.ManagementEventArgs/*02000009*/
{
  .method /*06000086*/ public hidebysig specialname 
          instance class System.Management.ManagementBaseObject/*02000005*/ 
          get_NewEvent() cil managed
  // SIG: 20 00 12 14
  {
    // Method begins at RVA 0x20fb
    // Code size       0 (0x0)
  } // end of method EventArrivedEventArgs::get_NewEvent

  .method /*06000087*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20fc
    // Code size       0 (0x0)
  } // end of method EventArrivedEventArgs::.ctor

  .property /*17000015*/ instance callconv(8) class System.Management.ManagementBaseObject/*02000005*/
          NewEvent()
  {
    .get instance class System.Management.ManagementBaseObject/*02000005*/ System.Management.EventArrivedEventArgs/*0200000E*/::get_NewEvent() /* 06000086 */
  } // end of property EventArrivedEventArgs::NewEvent
} // end of class System.Management.EventArrivedEventArgs

.class /*0200000F*/ public auto ansi beforefieldinit System.Management.StoppedEventArgs
       extends System.Management.ManagementEventArgs/*02000009*/
{
  .method /*06000088*/ public hidebysig specialname 
          instance valuetype System.Management.ManagementStatus/*02000013*/ 
          get_Status() cil managed
  // SIG: 20 00 11 4C
  {
    // Method begins at RVA 0x20fd
    // Code size       0 (0x0)
  } // end of method StoppedEventArgs::get_Status

  .method /*06000089*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20fe
    // Code size       0 (0x0)
  } // end of method StoppedEventArgs::.ctor

  .property /*17000016*/ instance callconv(8) valuetype System.Management.ManagementStatus/*02000013*/
          Status()
  {
    .get instance valuetype System.Management.ManagementStatus/*02000013*/ System.Management.StoppedEventArgs/*0200000F*/::get_Status() /* 06000088 */
  } // end of property StoppedEventArgs::Status
} // end of class System.Management.StoppedEventArgs

.class /*02000010*/ public auto ansi sealed System.Management.EventArrivedEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*0100000D*/
{
  .method /*0600008A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method EventArrivedEventHandler::.ctor

  .method /*0600008B*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Management.EventArrivedEventArgs/*0200000E*/ e) runtime managed
  // SIG: 20 02 01 1C 12 38
  {
  } // end of method EventArrivedEventHandler::Invoke

  .method /*0600008C*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ 
          BeginInvoke(object sender,
                      class System.Management.EventArrivedEventArgs/*0200000E*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100000F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 39 1C 12 38 12 3D 1C
  {
  } // end of method EventArrivedEventHandler::BeginInvoke

  .method /*0600008D*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ result) runtime managed
  // SIG: 20 01 01 12 39
  {
  } // end of method EventArrivedEventHandler::EndInvoke

} // end of class System.Management.EventArrivedEventHandler

.class /*02000011*/ public auto ansi sealed System.Management.StoppedEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*0100000D*/
{
  .method /*0600008E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method StoppedEventHandler::.ctor

  .method /*0600008F*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Management.StoppedEventArgs/*0200000F*/ e) runtime managed
  // SIG: 20 02 01 1C 12 3C
  {
  } // end of method StoppedEventHandler::Invoke

  .method /*06000090*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ 
          BeginInvoke(object sender,
                      class System.Management.StoppedEventArgs/*0200000F*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100000F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 39 1C 12 3C 12 3D 1C
  {
  } // end of method StoppedEventHandler::BeginInvoke

  .method /*06000091*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ result) runtime managed
  // SIG: 20 01 01 12 39
  {
  } // end of method StoppedEventHandler::EndInvoke

} // end of class System.Management.StoppedEventHandler

.class /*02000012*/ public auto ansi beforefieldinit System.Management.ManagementEventWatcher
       extends [System/*23000002*/]System.ComponentModel.Component/*01000002*/
{
  .custom /*0C000018:0A00001B*/ instance void [System/*23000002*/]System.ComponentModel.ToolboxItemAttribute/*01000038*/::.ctor(bool) /* 0A00001B */ = ( 01 00 00 00 00 ) 
  .method /*06000092*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ff
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::.ctor

  .method /*06000093*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.EventQuery/*0200002D*/ query) cil managed
  // SIG: 20 01 01 12 80 B4
  {
    // Method begins at RVA 0x2100
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::.ctor

  .method /*06000094*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2101
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::.ctor

  .method /*06000095*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementScope/*02000034*/ scope,
                               class System.Management.EventQuery/*0200002D*/ query) cil managed
  // SIG: 20 02 01 12 80 D0 12 80 B4
  {
    // Method begins at RVA 0x2102
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::.ctor

  .method /*06000096*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scope,
                               string query) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2103
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::.ctor

  .method /*06000097*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scope,
                               string query,
                               class System.Management.EventWatcherOptions/*02000023*/ options) cil managed
  // SIG: 20 03 01 0E 0E 12 80 8C
  {
    // Method begins at RVA 0x2104
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::.ctor

  .method /*06000098*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementScope/*02000034*/ scope,
                               class System.Management.EventQuery/*0200002D*/ query,
                               class System.Management.EventWatcherOptions/*02000023*/ options) cil managed
  // SIG: 20 03 01 12 80 D0 12 80 B4 12 80 8C
  {
    // Method begins at RVA 0x2105
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::.ctor

  .method /*06000099*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2106
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::Finalize

  .method /*0600009A*/ public hidebysig specialname 
          instance void  add_EventArrived(class System.Management.EventArrivedEventHandler/*02000010*/ 'value') cil managed
  // SIG: 20 01 01 12 40
  {
    // Method begins at RVA 0x2107
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::add_EventArrived

  .method /*0600009B*/ public hidebysig specialname 
          instance void  remove_EventArrived(class System.Management.EventArrivedEventHandler/*02000010*/ 'value') cil managed
  // SIG: 20 01 01 12 40
  {
    // Method begins at RVA 0x2108
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::remove_EventArrived

  .method /*0600009C*/ public hidebysig specialname 
          instance void  add_Stopped(class System.Management.StoppedEventHandler/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x2109
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::add_Stopped

  .method /*0600009D*/ public hidebysig specialname 
          instance void  remove_Stopped(class System.Management.StoppedEventHandler/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x210a
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::remove_Stopped

  .method /*0600009E*/ public hidebysig specialname 
          instance class System.Management.ManagementScope/*02000034*/ 
          get_Scope() cil managed
  // SIG: 20 00 12 80 D0
  {
    // Method begins at RVA 0x210b
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::get_Scope

  .method /*0600009F*/ public hidebysig specialname 
          instance void  set_Scope(class System.Management.ManagementScope/*02000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D0
  {
    // Method begins at RVA 0x210c
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::set_Scope

  .method /*060000A0*/ public hidebysig specialname 
          instance class System.Management.EventQuery/*0200002D*/ 
          get_Query() cil managed
  // SIG: 20 00 12 80 B4
  {
    // Method begins at RVA 0x210d
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::get_Query

  .method /*060000A1*/ public hidebysig specialname 
          instance void  set_Query(class System.Management.EventQuery/*0200002D*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B4
  {
    // Method begins at RVA 0x210e
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::set_Query

  .method /*060000A2*/ public hidebysig specialname 
          instance class System.Management.EventWatcherOptions/*02000023*/ 
          get_Options() cil managed
  // SIG: 20 00 12 80 8C
  {
    // Method begins at RVA 0x210f
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::get_Options

  .method /*060000A3*/ public hidebysig specialname 
          instance void  set_Options(class System.Management.EventWatcherOptions/*02000023*/ 'value') cil managed
  // SIG: 20 01 01 12 80 8C
  {
    // Method begins at RVA 0x2110
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::set_Options

  .method /*060000A4*/ public hidebysig instance class System.Management.ManagementBaseObject/*02000005*/ 
          WaitForNextEvent() cil managed
  // SIG: 20 00 12 14
  {
    // Method begins at RVA 0x2111
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::WaitForNextEvent

  .method /*060000A5*/ public hidebysig instance void 
          Start() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2112
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::Start

  .method /*060000A6*/ public hidebysig instance void 
          Stop() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2113
    // Code size       0 (0x0)
  } // end of method ManagementEventWatcher::Stop

  .event /*14000001*/ System.Management.EventArrivedEventHandler/*02000010*/ EventArrived
  {
    .addon instance void System.Management.ManagementEventWatcher/*02000012*/::add_EventArrived(class System.Management.EventArrivedEventHandler/*02000010*/) /* 0600009A */
    .removeon instance void System.Management.ManagementEventWatcher/*02000012*/::remove_EventArrived(class System.Management.EventArrivedEventHandler/*02000010*/) /* 0600009B */
  } // end of event ManagementEventWatcher::EventArrived
  .event /*14000002*/ System.Management.StoppedEventHandler/*02000011*/ Stopped
  {
    .addon instance void System.Management.ManagementEventWatcher/*02000012*/::add_Stopped(class System.Management.StoppedEventHandler/*02000011*/) /* 0600009C */
    .removeon instance void System.Management.ManagementEventWatcher/*02000012*/::remove_Stopped(class System.Management.StoppedEventHandler/*02000011*/) /* 0600009D */
  } // end of event ManagementEventWatcher::Stopped
  .property /*17000017*/ instance callconv(8) class System.Management.ManagementScope/*02000034*/
          Scope()
  {
    .set instance void System.Management.ManagementEventWatcher/*02000012*/::set_Scope(class System.Management.ManagementScope/*02000034*/) /* 0600009F */
    .get instance class System.Management.ManagementScope/*02000034*/ System.Management.ManagementEventWatcher/*02000012*/::get_Scope() /* 0600009E */
  } // end of property ManagementEventWatcher::Scope
  .property /*17000018*/ instance callconv(8) class System.Management.EventQuery/*0200002D*/
          Query()
  {
    .set instance void System.Management.ManagementEventWatcher/*02000012*/::set_Query(class System.Management.EventQuery/*0200002D*/) /* 060000A1 */
    .get instance class System.Management.EventQuery/*0200002D*/ System.Management.ManagementEventWatcher/*02000012*/::get_Query() /* 060000A0 */
  } // end of property ManagementEventWatcher::Query
  .property /*17000019*/ instance callconv(8) class System.Management.EventWatcherOptions/*02000023*/
          Options()
  {
    .set instance void System.Management.ManagementEventWatcher/*02000012*/::set_Options(class System.Management.EventWatcherOptions/*02000023*/) /* 060000A3 */
    .get instance class System.Management.EventWatcherOptions/*02000023*/ System.Management.ManagementEventWatcher/*02000012*/::get_Options() /* 060000A2 */
  } // end of property ManagementEventWatcher::Options
} // end of class System.Management.ManagementEventWatcher

.class /*02000013*/ public auto ansi sealed System.Management.ManagementStatus
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .field /*0400001F*/ public specialname rtspecialname int32 value__
  .field /*04000020*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ NoError = int32(0x00000000)
  .field /*04000021*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ False = int32(0x00000001)
  .field /*04000022*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ResetToDefault = int32(0x00040002)
  .field /*04000023*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ Different = int32(0x00040003)
  .field /*04000024*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ Timedout = int32(0x00040004)
  .field /*04000025*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ NoMoreData = int32(0x00040005)
  .field /*04000026*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ OperationCanceled = int32(0x00040006)
  .field /*04000027*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ Pending = int32(0x00040007)
  .field /*04000028*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ DuplicateObjects = int32(0x00040008)
  .field /*04000029*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ PartialResults = int32(0x00040010)
  .field /*0400002A*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ Failed = int32(0x80041001)
  .field /*0400002B*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ NotFound = int32(0x80041002)
  .field /*0400002C*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ AccessDenied = int32(0x80041003)
  .field /*0400002D*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ProviderFailure = int32(0x80041004)
  .field /*0400002E*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ TypeMismatch = int32(0x80041005)
  .field /*0400002F*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ OutOfMemory = int32(0x80041006)
  .field /*04000030*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidContext = int32(0x80041007)
  .field /*04000031*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidParameter = int32(0x80041008)
  .field /*04000032*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ NotAvailable = int32(0x80041009)
  .field /*04000033*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ CriticalError = int32(0x8004100A)
  .field /*04000034*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidStream = int32(0x8004100B)
  .field /*04000035*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ NotSupported = int32(0x8004100C)
  .field /*04000036*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidSuperclass = int32(0x8004100D)
  .field /*04000037*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidNamespace = int32(0x8004100E)
  .field /*04000038*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidObject = int32(0x8004100F)
  .field /*04000039*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidClass = int32(0x80041010)
  .field /*0400003A*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ProviderNotFound = int32(0x80041011)
  .field /*0400003B*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidProviderRegistration = int32(0x80041012)
  .field /*0400003C*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ProviderLoadFailure = int32(0x80041013)
  .field /*0400003D*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InitializationFailure = int32(0x80041014)
  .field /*0400003E*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ TransportFailure = int32(0x80041015)
  .field /*0400003F*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidOperation = int32(0x80041016)
  .field /*04000040*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidQuery = int32(0x80041017)
  .field /*04000041*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidQueryType = int32(0x80041018)
  .field /*04000042*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ AlreadyExists = int32(0x80041019)
  .field /*04000043*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ OverrideNotAllowed = int32(0x8004101A)
  .field /*04000044*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ PropagatedQualifier = int32(0x8004101B)
  .field /*04000045*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ PropagatedProperty = int32(0x8004101C)
  .field /*04000046*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ Unexpected = int32(0x8004101D)
  .field /*04000047*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ IllegalOperation = int32(0x8004101E)
  .field /*04000048*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ CannotBeKey = int32(0x8004101F)
  .field /*04000049*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ IncompleteClass = int32(0x80041020)
  .field /*0400004A*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidSyntax = int32(0x80041021)
  .field /*0400004B*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ NondecoratedObject = int32(0x80041022)
  .field /*0400004C*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ReadOnly = int32(0x80041023)
  .field /*0400004D*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ProviderNotCapable = int32(0x80041024)
  .field /*0400004E*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ClassHasChildren = int32(0x80041025)
  .field /*0400004F*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ClassHasInstances = int32(0x80041026)
  .field /*04000050*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ QueryNotImplemented = int32(0x80041027)
  .field /*04000051*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ IllegalNull = int32(0x80041028)
  .field /*04000052*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidQualifierType = int32(0x80041029)
  .field /*04000053*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidPropertyType = int32(0x8004102A)
  .field /*04000054*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ValueOutOfRange = int32(0x8004102B)
  .field /*04000055*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ CannotBeSingleton = int32(0x8004102C)
  .field /*04000056*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidCimType = int32(0x8004102D)
  .field /*04000057*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidMethod = int32(0x8004102E)
  .field /*04000058*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidMethodParameters = int32(0x8004102F)
  .field /*04000059*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ SystemProperty = int32(0x80041030)
  .field /*0400005A*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidProperty = int32(0x80041031)
  .field /*0400005B*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ CallCanceled = int32(0x80041032)
  .field /*0400005C*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ShuttingDown = int32(0x80041033)
  .field /*0400005D*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ PropagatedMethod = int32(0x80041034)
  .field /*0400005E*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UnsupportedParameter = int32(0x80041035)
  .field /*0400005F*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ MissingParameterID = int32(0x80041036)
  .field /*04000060*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidParameterID = int32(0x80041037)
  .field /*04000061*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ NonconsecutiveParameterIDs = int32(0x80041038)
  .field /*04000062*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ParameterIDOnRetval = int32(0x80041039)
  .field /*04000063*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidObjectPath = int32(0x8004103A)
  .field /*04000064*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ OutOfDiskSpace = int32(0x8004103B)
  .field /*04000065*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ BufferTooSmall = int32(0x8004103C)
  .field /*04000066*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UnsupportedPutExtension = int32(0x8004103D)
  .field /*04000067*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UnknownObjectType = int32(0x8004103E)
  .field /*04000068*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UnknownPacketType = int32(0x8004103F)
  .field /*04000069*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ MarshalVersionMismatch = int32(0x80041040)
  .field /*0400006A*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ MarshalInvalidSignature = int32(0x80041041)
  .field /*0400006B*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidQualifier = int32(0x80041042)
  .field /*0400006C*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidDuplicateParameter = int32(0x80041043)
  .field /*0400006D*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ TooMuchData = int32(0x80041044)
  .field /*0400006E*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ServerTooBusy = int32(0x80041045)
  .field /*0400006F*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidFlavor = int32(0x80041046)
  .field /*04000070*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ CircularReference = int32(0x80041047)
  .field /*04000071*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UnsupportedClassUpdate = int32(0x80041048)
  .field /*04000072*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ CannotChangeKeyInheritance = int32(0x80041049)
  .field /*04000073*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ CannotChangeIndexInheritance = int32(0x80041050)
  .field /*04000074*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ TooManyProperties = int32(0x80041051)
  .field /*04000075*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UpdateTypeMismatch = int32(0x80041052)
  .field /*04000076*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UpdateOverrideNotAllowed = int32(0x80041053)
  .field /*04000077*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UpdatePropagatedMethod = int32(0x80041054)
  .field /*04000078*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ MethodNotImplemented = int32(0x80041055)
  .field /*04000079*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ MethodDisabled = int32(0x80041056)
  .field /*0400007A*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ RefresherBusy = int32(0x80041057)
  .field /*0400007B*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UnparsableQuery = int32(0x80041058)
  .field /*0400007C*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ NotEventClass = int32(0x80041059)
  .field /*0400007D*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ MissingGroupWithin = int32(0x8004105A)
  .field /*0400007E*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ MissingAggregationList = int32(0x8004105B)
  .field /*0400007F*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ PropertyNotAnObject = int32(0x8004105C)
  .field /*04000080*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ AggregatingByObject = int32(0x8004105D)
  .field /*04000081*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ UninterpretableProviderQuery = int32(0x8004105F)
  .field /*04000082*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ BackupRestoreWinmgmtRunning = int32(0x80041060)
  .field /*04000083*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ QueueOverflow = int32(0x80041061)
  .field /*04000084*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ PrivilegeNotHeld = int32(0x80041062)
  .field /*04000085*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ InvalidOperator = int32(0x80041063)
  .field /*04000086*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ LocalCredentials = int32(0x80041064)
  .field /*04000087*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ CannotBeAbstract = int32(0x80041065)
  .field /*04000088*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ AmendedObject = int32(0x80041066)
  .field /*04000089*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ ClientTooSlow = int32(0x80041067)
  .field /*0400008A*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ RegistrationTooBroad = int32(0x80042001)
  .field /*0400008B*/ public static literal valuetype System.Management.ManagementStatus/*02000013*/ RegistrationTooPrecise = int32(0x80042002)
} // end of class System.Management.ManagementStatus

.class /*02000014*/ public auto ansi serializable beforefieldinit System.Management.ManagementException
       extends [mscorlib/*23000001*/]System.SystemException/*01000010*/
{
  .method /*060000A7*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    // Method begins at RVA 0x2114
    // Code size       0 (0x0)
  } // end of method ManagementException::.ctor

  .method /*060000A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2115
    // Code size       0 (0x0)
  } // end of method ManagementException::.ctor

  .method /*060000A9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2116
    // Code size       0 (0x0)
  } // end of method ManagementException::.ctor

  .method /*060000AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*01000011*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 45
  {
    // Method begins at RVA 0x2117
    // Code size       0 (0x0)
  } // end of method ManagementException::.ctor

  .method /*060000AB*/ public hidebysig virtual 
          instance void  GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                                       valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    .permissionset demand
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'SerializationFormatter' = bool(true)}}
    // Method begins at RVA 0x2118
    // Code size       0 (0x0)
  } // end of method ManagementException::GetObjectData

  .method /*060000AC*/ public hidebysig specialname 
          instance class System.Management.ManagementBaseObject/*02000005*/ 
          get_ErrorInformation() cil managed
  // SIG: 20 00 12 14
  {
    // Method begins at RVA 0x2119
    // Code size       0 (0x0)
  } // end of method ManagementException::get_ErrorInformation

  .method /*060000AD*/ public hidebysig specialname 
          instance valuetype System.Management.ManagementStatus/*02000013*/ 
          get_ErrorCode() cil managed
  // SIG: 20 00 11 4C
  {
    // Method begins at RVA 0x211a
    // Code size       0 (0x0)
  } // end of method ManagementException::get_ErrorCode

  .property /*1700001A*/ instance callconv(8) class System.Management.ManagementBaseObject/*02000005*/
          ErrorInformation()
  {
    .get instance class System.Management.ManagementBaseObject/*02000005*/ System.Management.ManagementException/*02000014*/::get_ErrorInformation() /* 060000AC */
  } // end of property ManagementException::ErrorInformation
  .property /*1700001B*/ instance callconv(8) valuetype System.Management.ManagementStatus/*02000013*/
          ErrorCode()
  {
    .get instance valuetype System.Management.ManagementStatus/*02000013*/ System.Management.ManagementException/*02000014*/::get_ErrorCode() /* 060000AD */
  } // end of property ManagementException::ErrorCode
} // end of class System.Management.ManagementException

.class /*02000015*/ public auto ansi beforefieldinit System.Management.ManagementNamedValueCollection
       extends [System/*23000002*/]System.Collections.Specialized.NameObjectCollectionBase/*01000012*/
{
  .custom /*0C000019:0A00001A*/ instance void [mscorlib/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000037*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*060000AE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x211b
    // Code size       0 (0x0)
  } // end of method ManagementNamedValueCollection::.ctor

  .method /*060000AF*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000004*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000005*/ context) cil managed
  // SIG: 20 02 01 12 11 11 15
  {
    // Method begins at RVA 0x211c
    // Code size       0 (0x0)
  } // end of method ManagementNamedValueCollection::.ctor

  .method /*060000B0*/ public hidebysig instance void 
          Add(string name,
              object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x211d
    // Code size       0 (0x0)
  } // end of method ManagementNamedValueCollection::Add

  .method /*060000B1*/ public hidebysig instance void 
          Remove(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x211e
    // Code size       0 (0x0)
  } // end of method ManagementNamedValueCollection::Remove

  .method /*060000B2*/ public hidebysig instance void 
          RemoveAll() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x211f
    // Code size       0 (0x0)
  } // end of method ManagementNamedValueCollection::RemoveAll

  .method /*060000B3*/ public hidebysig instance class System.Management.ManagementNamedValueCollection/*02000015*/ 
          Clone() cil managed
  // SIG: 20 00 12 54
  {
    // Method begins at RVA 0x2120
    // Code size       0 (0x0)
  } // end of method ManagementNamedValueCollection::Clone

  .method /*060000B4*/ public hidebysig specialname 
          instance object  get_Item(string name) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x2121
    // Code size       0 (0x0)
  } // end of method ManagementNamedValueCollection::get_Item

  .property /*1700001C*/ instance callconv(8) object
          Item(string)
  {
    .get instance object System.Management.ManagementNamedValueCollection/*02000015*/::get_Item(string) /* 060000B4 */
  } // end of property ManagementNamedValueCollection::Item
} // end of class System.Management.ManagementNamedValueCollection

.class /*02000016*/ public auto ansi beforefieldinit System.Management.ManagementObjectCollection
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements [mscorlib/*23000001*/]System.Collections.ICollection/*01000015*/,
                  [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000013*/,
                  [mscorlib/*23000001*/]System.IDisposable/*01000016*/
{
  .class /*02000017*/ auto ansi nested public beforefieldinit ManagementObjectEnumerator
         extends [mscorlib/*23000001*/]System.Object/*01000007*/
         implements [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/,
                    [mscorlib/*23000001*/]System.IDisposable/*01000016*/
  {
    .method /*060000C0*/ family hidebysig virtual 
            instance void  Finalize() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x212d
      // Code size       0 (0x0)
    } // end of method ManagementObjectEnumerator::Finalize

    .method /*060000C1*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x212e
      // Code size       0 (0x0)
    } // end of method ManagementObjectEnumerator::Dispose

    .method /*060000C2*/ public hidebysig specialname 
            instance class System.Management.ManagementBaseObject/*02000005*/ 
            get_Current() cil managed
    // SIG: 20 00 12 14
    {
      // Method begins at RVA 0x212f
      // Code size       0 (0x0)
    } // end of method ManagementObjectEnumerator::get_Current

    .method /*060000C3*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/::get_Current /*01000014::0A000003*/ 
      // Method begins at RVA 0x2130
      // Code size       0 (0x0)
    } // end of method ManagementObjectEnumerator::System.Collections.IEnumerator.get_Current

    .method /*060000C4*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x2131
      // Code size       0 (0x0)
    } // end of method ManagementObjectEnumerator::MoveNext

    .method /*060000C5*/ public hidebysig newslot virtual final 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2132
      // Code size       0 (0x0)
    } // end of method ManagementObjectEnumerator::Reset

    .method /*060000C6*/ private hidebysig specialname rtspecialname static 
            void  .cctor() cil managed
    // SIG: 00 00 01
    {
      // Method begins at RVA 0x2133
      // Code size       0 (0x0)
    } // end of method ManagementObjectEnumerator::.cctor

    .method /*060000C7*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2134
      // Code size       0 (0x0)
    } // end of method ManagementObjectEnumerator::.ctor

    .property /*17000020*/ instance callconv(8) class System.Management.ManagementBaseObject/*02000005*/
            Current()
    {
      .get instance class System.Management.ManagementBaseObject/*02000005*/ System.Management.ManagementObjectCollection/*02000016*//ManagementObjectEnumerator/*02000017*/::get_Current() /* 060000C2 */
    } // end of property ManagementObjectEnumerator::Current
    .property /*17000021*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object System.Management.ManagementObjectCollection/*02000016*//ManagementObjectEnumerator/*02000017*/::System.Collections.IEnumerator.get_Current() /* 060000C3 */
    } // end of property ManagementObjectEnumerator::System.Collections.IEnumerator.Current
  } // end of class ManagementObjectEnumerator

  .method /*060000B5*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2122
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::Finalize

  .method /*060000B6*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2123
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::Dispose

  .method /*060000B7*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2124
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::get_Count

  .method /*060000B8*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsSynchronized() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2125
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::get_IsSynchronized

  .method /*060000B9*/ public hidebysig newslot specialname virtual final 
          instance object  get_SyncRoot() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2126
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::get_SyncRoot

  .method /*060000BA*/ public hidebysig newslot virtual final 
          instance void  CopyTo(class [mscorlib/*23000001*/]System.Array/*01000017*/ 'array',
                                int32 index) cil managed
  // SIG: 20 02 01 12 5D 08
  {
    // Method begins at RVA 0x2127
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::CopyTo

  .method /*060000BB*/ public hidebysig instance void 
          CopyTo(class System.Management.ManagementBaseObject/*02000005*/[] objectCollection,
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 14 08
  {
    // Method begins at RVA 0x2128
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::CopyTo

  .method /*060000BC*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*//ManagementObjectEnumerator/*02000017*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 12 5C
  {
    // Method begins at RVA 0x2129
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::GetEnumerator

  .method /*060000BD*/ private hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 51
  {
    .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000013*/::GetEnumerator /*01000013::0A000002*/ 
    // Method begins at RVA 0x212a
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::System.Collections.IEnumerable.GetEnumerator

  .method /*060000BE*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x212b
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::.cctor

  .method /*060000BF*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x212c
    // Code size       0 (0x0)
  } // end of method ManagementObjectCollection::.ctor

  .property /*1700001D*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Management.ManagementObjectCollection/*02000016*/::get_Count() /* 060000B7 */
  } // end of property ManagementObjectCollection::Count
  .property /*1700001E*/ instance callconv(8) bool
          IsSynchronized()
  {
    .get instance bool System.Management.ManagementObjectCollection/*02000016*/::get_IsSynchronized() /* 060000B8 */
  } // end of property ManagementObjectCollection::IsSynchronized
  .property /*1700001F*/ instance callconv(8) object
          SyncRoot()
  {
    .get instance object System.Management.ManagementObjectCollection/*02000016*/::get_SyncRoot() /* 060000B9 */
  } // end of property ManagementObjectCollection::SyncRoot
} // end of class System.Management.ManagementObjectCollection

.class /*02000018*/ public auto ansi beforefieldinit System.Management.ManagementObjectSearcher
       extends [System/*23000002*/]System.ComponentModel.Component/*01000002*/
{
  .custom /*0C00001A:0A00001B*/ instance void [System/*23000002*/]System.ComponentModel.ToolboxItemAttribute/*01000038*/::.ctor(bool) /* 0A00001B */ = ( 01 00 00 00 00 ) 
  .method /*060000C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2135
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::.ctor

  .method /*060000C9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string queryString) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2136
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::.ctor

  .method /*060000CA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ObjectQuery/*0200002C*/ query) cil managed
  // SIG: 20 01 01 12 80 B0
  {
    // Method begins at RVA 0x2137
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::.ctor

  .method /*060000CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scope,
                               string queryString) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2138
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::.ctor

  .method /*060000CC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementScope/*02000034*/ scope,
                               class System.Management.ObjectQuery/*0200002C*/ query) cil managed
  // SIG: 20 02 01 12 80 D0 12 80 B0
  {
    // Method begins at RVA 0x2139
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::.ctor

  .method /*060000CD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scope,
                               string queryString,
                               class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 03 01 0E 0E 12 80 88
  {
    // Method begins at RVA 0x213a
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::.ctor

  .method /*060000CE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementScope/*02000034*/ scope,
                               class System.Management.ObjectQuery/*0200002C*/ query,
                               class System.Management.EnumerationOptions/*02000022*/ options) cil managed
  // SIG: 20 03 01 12 80 D0 12 80 B0 12 80 88
  {
    // Method begins at RVA 0x213b
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::.ctor

  .method /*060000CF*/ public hidebysig specialname 
          instance class System.Management.ManagementScope/*02000034*/ 
          get_Scope() cil managed
  // SIG: 20 00 12 80 D0
  {
    // Method begins at RVA 0x213c
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::get_Scope

  .method /*060000D0*/ public hidebysig specialname 
          instance void  set_Scope(class System.Management.ManagementScope/*02000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D0
  {
    // Method begins at RVA 0x213d
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::set_Scope

  .method /*060000D1*/ public hidebysig specialname 
          instance class System.Management.ObjectQuery/*0200002C*/ 
          get_Query() cil managed
  // SIG: 20 00 12 80 B0
  {
    // Method begins at RVA 0x213e
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::get_Query

  .method /*060000D2*/ public hidebysig specialname 
          instance void  set_Query(class System.Management.ObjectQuery/*0200002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B0
  {
    // Method begins at RVA 0x213f
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::set_Query

  .method /*060000D3*/ public hidebysig specialname 
          instance class System.Management.EnumerationOptions/*02000022*/ 
          get_Options() cil managed
  // SIG: 20 00 12 80 88
  {
    // Method begins at RVA 0x2140
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::get_Options

  .method /*060000D4*/ public hidebysig specialname 
          instance void  set_Options(class System.Management.EnumerationOptions/*02000022*/ 'value') cil managed
  // SIG: 20 01 01 12 80 88
  {
    // Method begins at RVA 0x2141
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::set_Options

  .method /*060000D5*/ public hidebysig instance class System.Management.ManagementObjectCollection/*02000016*/ 
          Get() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x2142
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::Get

  .method /*060000D6*/ public hidebysig instance void 
          Get(class System.Management.ManagementOperationObserver/*0200001D*/ watcher) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x2143
    // Code size       0 (0x0)
  } // end of method ManagementObjectSearcher::Get

  .property /*17000022*/ instance callconv(8) class System.Management.ManagementScope/*02000034*/
          Scope()
  {
    .set instance void System.Management.ManagementObjectSearcher/*02000018*/::set_Scope(class System.Management.ManagementScope/*02000034*/) /* 060000D0 */
    .get instance class System.Management.ManagementScope/*02000034*/ System.Management.ManagementObjectSearcher/*02000018*/::get_Scope() /* 060000CF */
  } // end of property ManagementObjectSearcher::Scope
  .property /*17000023*/ instance callconv(8) class System.Management.ObjectQuery/*0200002C*/
          Query()
  {
    .get instance class System.Management.ObjectQuery/*0200002C*/ System.Management.ManagementObjectSearcher/*02000018*/::get_Query() /* 060000D1 */
    .set instance void System.Management.ManagementObjectSearcher/*02000018*/::set_Query(class System.Management.ObjectQuery/*0200002C*/) /* 060000D2 */
  } // end of property ManagementObjectSearcher::Query
  .property /*17000024*/ instance callconv(8) class System.Management.EnumerationOptions/*02000022*/
          Options()
  {
    .get instance class System.Management.EnumerationOptions/*02000022*/ System.Management.ManagementObjectSearcher/*02000018*/::get_Options() /* 060000D3 */
    .set instance void System.Management.ManagementObjectSearcher/*02000018*/::set_Options(class System.Management.EnumerationOptions/*02000022*/) /* 060000D4 */
  } // end of property ManagementObjectSearcher::Options
} // end of class System.Management.ManagementObjectSearcher

.class /*02000019*/ public auto ansi sealed System.Management.ObjectReadyEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*0100000D*/
{
  .method /*060000D7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ObjectReadyEventHandler::.ctor

  .method /*060000D8*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Management.ObjectReadyEventArgs/*0200000A*/ e) runtime managed
  // SIG: 20 02 01 1C 12 28
  {
  } // end of method ObjectReadyEventHandler::Invoke

  .method /*060000D9*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ 
          BeginInvoke(object sender,
                      class System.Management.ObjectReadyEventArgs/*0200000A*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100000F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 39 1C 12 28 12 3D 1C
  {
  } // end of method ObjectReadyEventHandler::BeginInvoke

  .method /*060000DA*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ result) runtime managed
  // SIG: 20 01 01 12 39
  {
  } // end of method ObjectReadyEventHandler::EndInvoke

} // end of class System.Management.ObjectReadyEventHandler

.class /*0200001A*/ public auto ansi sealed System.Management.CompletedEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*0100000D*/
{
  .method /*060000DB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method CompletedEventHandler::.ctor

  .method /*060000DC*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Management.CompletedEventArgs/*0200000B*/ e) runtime managed
  // SIG: 20 02 01 1C 12 2C
  {
  } // end of method CompletedEventHandler::Invoke

  .method /*060000DD*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ 
          BeginInvoke(object sender,
                      class System.Management.CompletedEventArgs/*0200000B*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100000F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 39 1C 12 2C 12 3D 1C
  {
  } // end of method CompletedEventHandler::BeginInvoke

  .method /*060000DE*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ result) runtime managed
  // SIG: 20 01 01 12 39
  {
  } // end of method CompletedEventHandler::EndInvoke

} // end of class System.Management.CompletedEventHandler

.class /*0200001B*/ public auto ansi sealed System.Management.ProgressEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*0100000D*/
{
  .method /*060000DF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ProgressEventHandler::.ctor

  .method /*060000E0*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Management.ProgressEventArgs/*0200000D*/ e) runtime managed
  // SIG: 20 02 01 1C 12 34
  {
  } // end of method ProgressEventHandler::Invoke

  .method /*060000E1*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ 
          BeginInvoke(object sender,
                      class System.Management.ProgressEventArgs/*0200000D*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100000F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 39 1C 12 34 12 3D 1C
  {
  } // end of method ProgressEventHandler::BeginInvoke

  .method /*060000E2*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ result) runtime managed
  // SIG: 20 01 01 12 39
  {
  } // end of method ProgressEventHandler::EndInvoke

} // end of class System.Management.ProgressEventHandler

.class /*0200001C*/ public auto ansi sealed System.Management.ObjectPutEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*0100000D*/
{
  .method /*060000E3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ObjectPutEventHandler::.ctor

  .method /*060000E4*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Management.ObjectPutEventArgs/*0200000C*/ e) runtime managed
  // SIG: 20 02 01 1C 12 30
  {
  } // end of method ObjectPutEventHandler::Invoke

  .method /*060000E5*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ 
          BeginInvoke(object sender,
                      class System.Management.ObjectPutEventArgs/*0200000C*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100000F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 39 1C 12 30 12 3D 1C
  {
  } // end of method ObjectPutEventHandler::BeginInvoke

  .method /*060000E6*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*0100000E*/ result) runtime managed
  // SIG: 20 01 01 12 39
  {
  } // end of method ObjectPutEventHandler::EndInvoke

} // end of class System.Management.ObjectPutEventHandler

.class /*0200001D*/ public auto ansi beforefieldinit System.Management.ManagementOperationObserver
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
{
  .method /*060000E7*/ public hidebysig specialname 
          instance void  add_ObjectReady(class System.Management.ObjectReadyEventHandler/*02000019*/ 'value') cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x2144
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::add_ObjectReady

  .method /*060000E8*/ public hidebysig specialname 
          instance void  remove_ObjectReady(class System.Management.ObjectReadyEventHandler/*02000019*/ 'value') cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x2145
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::remove_ObjectReady

  .method /*060000E9*/ public hidebysig specialname 
          instance void  add_Completed(class System.Management.CompletedEventHandler/*0200001A*/ 'value') cil managed
  // SIG: 20 01 01 12 68
  {
    // Method begins at RVA 0x2146
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::add_Completed

  .method /*060000EA*/ public hidebysig specialname 
          instance void  remove_Completed(class System.Management.CompletedEventHandler/*0200001A*/ 'value') cil managed
  // SIG: 20 01 01 12 68
  {
    // Method begins at RVA 0x2147
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::remove_Completed

  .method /*060000EB*/ public hidebysig specialname 
          instance void  add_Progress(class System.Management.ProgressEventHandler/*0200001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6C
  {
    // Method begins at RVA 0x2148
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::add_Progress

  .method /*060000EC*/ public hidebysig specialname 
          instance void  remove_Progress(class System.Management.ProgressEventHandler/*0200001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6C
  {
    // Method begins at RVA 0x2149
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::remove_Progress

  .method /*060000ED*/ public hidebysig specialname 
          instance void  add_ObjectPut(class System.Management.ObjectPutEventHandler/*0200001C*/ 'value') cil managed
  // SIG: 20 01 01 12 70
  {
    // Method begins at RVA 0x214a
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::add_ObjectPut

  .method /*060000EE*/ public hidebysig specialname 
          instance void  remove_ObjectPut(class System.Management.ObjectPutEventHandler/*0200001C*/ 'value') cil managed
  // SIG: 20 01 01 12 70
  {
    // Method begins at RVA 0x214b
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::remove_ObjectPut

  .method /*060000EF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214c
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::.ctor

  .method /*060000F0*/ public hidebysig instance void 
          Cancel() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214d
    // Code size       0 (0x0)
  } // end of method ManagementOperationObserver::Cancel

  .event /*14000003*/ System.Management.ObjectReadyEventHandler/*02000019*/ ObjectReady
  {
    .addon instance void System.Management.ManagementOperationObserver/*0200001D*/::add_ObjectReady(class System.Management.ObjectReadyEventHandler/*02000019*/) /* 060000E7 */
    .removeon instance void System.Management.ManagementOperationObserver/*0200001D*/::remove_ObjectReady(class System.Management.ObjectReadyEventHandler/*02000019*/) /* 060000E8 */
  } // end of event ManagementOperationObserver::ObjectReady
  .event /*14000004*/ System.Management.CompletedEventHandler/*0200001A*/ Completed
  {
    .removeon instance void System.Management.ManagementOperationObserver/*0200001D*/::remove_Completed(class System.Management.CompletedEventHandler/*0200001A*/) /* 060000EA */
    .addon instance void System.Management.ManagementOperationObserver/*0200001D*/::add_Completed(class System.Management.CompletedEventHandler/*0200001A*/) /* 060000E9 */
  } // end of event ManagementOperationObserver::Completed
  .event /*14000005*/ System.Management.ProgressEventHandler/*0200001B*/ Progress
  {
    .removeon instance void System.Management.ManagementOperationObserver/*0200001D*/::remove_Progress(class System.Management.ProgressEventHandler/*0200001B*/) /* 060000EC */
    .addon instance void System.Management.ManagementOperationObserver/*0200001D*/::add_Progress(class System.Management.ProgressEventHandler/*0200001B*/) /* 060000EB */
  } // end of event ManagementOperationObserver::Progress
  .event /*14000006*/ System.Management.ObjectPutEventHandler/*0200001C*/ ObjectPut
  {
    .addon instance void System.Management.ManagementOperationObserver/*0200001D*/::add_ObjectPut(class System.Management.ObjectPutEventHandler/*0200001C*/) /* 060000ED */
    .removeon instance void System.Management.ManagementOperationObserver/*0200001D*/::remove_ObjectPut(class System.Management.ObjectPutEventHandler/*0200001C*/) /* 060000EE */
  } // end of event ManagementOperationObserver::ObjectPut
} // end of class System.Management.ManagementOperationObserver

.class /*0200001E*/ public auto ansi sealed System.Management.AuthenticationLevel
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .field /*0400008C*/ public specialname rtspecialname int32 value__
  .field /*0400008D*/ public static literal valuetype System.Management.AuthenticationLevel/*0200001E*/ Default = int32(0x00000000)
  .field /*0400008E*/ public static literal valuetype System.Management.AuthenticationLevel/*0200001E*/ None = int32(0x00000001)
  .field /*0400008F*/ public static literal valuetype System.Management.AuthenticationLevel/*0200001E*/ Connect = int32(0x00000002)
  .field /*04000090*/ public static literal valuetype System.Management.AuthenticationLevel/*0200001E*/ Call = int32(0x00000003)
  .field /*04000091*/ public static literal valuetype System.Management.AuthenticationLevel/*0200001E*/ Packet = int32(0x00000004)
  .field /*04000092*/ public static literal valuetype System.Management.AuthenticationLevel/*0200001E*/ PacketIntegrity = int32(0x00000005)
  .field /*04000093*/ public static literal valuetype System.Management.AuthenticationLevel/*0200001E*/ PacketPrivacy = int32(0x00000006)
  .field /*04000094*/ public static literal valuetype System.Management.AuthenticationLevel/*0200001E*/ Unchanged = int32(0xFFFFFFFF)
} // end of class System.Management.AuthenticationLevel

.class /*0200001F*/ public auto ansi sealed System.Management.ImpersonationLevel
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .field /*04000095*/ public specialname rtspecialname int32 value__
  .field /*04000096*/ public static literal valuetype System.Management.ImpersonationLevel/*0200001F*/ Default = int32(0x00000000)
  .field /*04000097*/ public static literal valuetype System.Management.ImpersonationLevel/*0200001F*/ Anonymous = int32(0x00000001)
  .field /*04000098*/ public static literal valuetype System.Management.ImpersonationLevel/*0200001F*/ Identify = int32(0x00000002)
  .field /*04000099*/ public static literal valuetype System.Management.ImpersonationLevel/*0200001F*/ Impersonate = int32(0x00000003)
  .field /*0400009A*/ public static literal valuetype System.Management.ImpersonationLevel/*0200001F*/ Delegate = int32(0x00000004)
} // end of class System.Management.ImpersonationLevel

.class /*02000020*/ public auto ansi sealed System.Management.PutType
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .field /*0400009B*/ public specialname rtspecialname int32 value__
  .field /*0400009C*/ public static literal valuetype System.Management.PutType/*02000020*/ None = int32(0x00000000)
  .field /*0400009D*/ public static literal valuetype System.Management.PutType/*02000020*/ UpdateOnly = int32(0x00000001)
  .field /*0400009E*/ public static literal valuetype System.Management.PutType/*02000020*/ CreateOnly = int32(0x00000002)
  .field /*0400009F*/ public static literal valuetype System.Management.PutType/*02000020*/ UpdateOrCreate = int32(0x00000003)
} // end of class System.Management.PutType

.class /*02000021*/ public abstract auto ansi beforefieldinit System.Management.ManagementOptions
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements [mscorlib/*23000001*/]System.ICloneable/*01000006*/
{
  .custom /*0C00001B:0A00001C*/ instance void [System/*23000002*/]System.ComponentModel.TypeConverterAttribute/*01000039*/::.ctor(class [mscorlib/*23000001*/]System.Type/*0100001B*/) /* 0A00001C */ = ( 01 00 7A 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // ..zSystem.Compon
                                                                                                                                                                                                          65 6E 74 4D 6F 64 65 6C 2E 45 78 70 61 6E 64 61   // entModel.Expanda
                                                                                                                                                                                                          62 6C 65 4F 62 6A 65 63 74 43 6F 6E 76 65 72 74   // bleObjectConvert
                                                                                                                                                                                                          65 72 2C 20 53 79 73 74 65 6D 2C 20 56 65 72 73   // er, System, Vers
                                                                                                                                                                                                          69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=4.0.0.0, Cul
                                                                                                                                                                                                          74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                                          62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 37 37   // blicKeyToken=b77
                                                                                                                                                                                                          61 35 63 35 36 31 39 33 34 65 30 38 39 00 00 )    // a5c561934e089..
  .field /*040000A0*/ public static initonly valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ InfiniteTimeout
  .method /*060000F1*/ public hidebysig specialname 
          instance class System.Management.ManagementNamedValueCollection/*02000015*/ 
          get_Context() cil managed
  // SIG: 20 00 12 54
  {
    // Method begins at RVA 0x214e
    // Code size       0 (0x0)
  } // end of method ManagementOptions::get_Context

  .method /*060000F2*/ public hidebysig specialname 
          instance void  set_Context(class System.Management.ManagementNamedValueCollection/*02000015*/ 'value') cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x214f
    // Code size       0 (0x0)
  } // end of method ManagementOptions::set_Context

  .method /*060000F3*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ 
          get_Timeout() cil managed
  // SIG: 20 00 11 2D
  {
    // Method begins at RVA 0x2150
    // Code size       0 (0x0)
  } // end of method ManagementOptions::get_Timeout

  .method /*060000F4*/ public hidebysig specialname 
          instance void  set_Timeout(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 11 2D
  {
    // Method begins at RVA 0x2151
    // Code size       0 (0x0)
  } // end of method ManagementOptions::set_Timeout

  .method /*060000F5*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2152
    // Code size       0 (0x0)
  } // end of method ManagementOptions::.ctor

  .method /*060000F6*/ public hidebysig newslot abstract virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method ManagementOptions::Clone

  .method /*060000F7*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2153
    // Code size       0 (0x0)
  } // end of method ManagementOptions::.cctor

  .property /*17000025*/ instance callconv(8) class System.Management.ManagementNamedValueCollection/*02000015*/
          Context()
  {
    .set instance void System.Management.ManagementOptions/*02000021*/::set_Context(class System.Management.ManagementNamedValueCollection/*02000015*/) /* 060000F2 */
    .get instance class System.Management.ManagementNamedValueCollection/*02000015*/ System.Management.ManagementOptions/*02000021*/::get_Context() /* 060000F1 */
  } // end of property ManagementOptions::Context
  .property /*17000026*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/
          Timeout()
  {
    .set instance void System.Management.ManagementOptions/*02000021*/::set_Timeout(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/) /* 060000F4 */
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ System.Management.ManagementOptions/*02000021*/::get_Timeout() /* 060000F3 */
  } // end of property ManagementOptions::Timeout
} // end of class System.Management.ManagementOptions

.class /*02000022*/ public auto ansi beforefieldinit System.Management.EnumerationOptions
       extends System.Management.ManagementOptions/*02000021*/
{
  .method /*060000F8*/ public hidebysig specialname 
          instance bool  get_ReturnImmediately() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2154
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::get_ReturnImmediately

  .method /*060000F9*/ public hidebysig specialname 
          instance void  set_ReturnImmediately(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2155
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::set_ReturnImmediately

  .method /*060000FA*/ public hidebysig specialname 
          instance int32  get_BlockSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2156
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::get_BlockSize

  .method /*060000FB*/ public hidebysig specialname 
          instance void  set_BlockSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2157
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::set_BlockSize

  .method /*060000FC*/ public hidebysig specialname 
          instance bool  get_Rewindable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2158
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::get_Rewindable

  .method /*060000FD*/ public hidebysig specialname 
          instance void  set_Rewindable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2159
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::set_Rewindable

  .method /*060000FE*/ public hidebysig specialname 
          instance bool  get_UseAmendedQualifiers() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x215a
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::get_UseAmendedQualifiers

  .method /*060000FF*/ public hidebysig specialname 
          instance void  set_UseAmendedQualifiers(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x215b
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::set_UseAmendedQualifiers

  .method /*06000100*/ public hidebysig specialname 
          instance bool  get_EnsureLocatable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x215c
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::get_EnsureLocatable

  .method /*06000101*/ public hidebysig specialname 
          instance void  set_EnsureLocatable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x215d
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::set_EnsureLocatable

  .method /*06000102*/ public hidebysig specialname 
          instance bool  get_PrototypeOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x215e
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::get_PrototypeOnly

  .method /*06000103*/ public hidebysig specialname 
          instance void  set_PrototypeOnly(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x215f
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::set_PrototypeOnly

  .method /*06000104*/ public hidebysig specialname 
          instance bool  get_DirectRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2160
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::get_DirectRead

  .method /*06000105*/ public hidebysig specialname 
          instance void  set_DirectRead(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2161
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::set_DirectRead

  .method /*06000106*/ public hidebysig specialname 
          instance bool  get_EnumerateDeep() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2162
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::get_EnumerateDeep

  .method /*06000107*/ public hidebysig specialname 
          instance void  set_EnumerateDeep(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2163
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::set_EnumerateDeep

  .method /*06000108*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2164
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::.ctor

  .method /*06000109*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementNamedValueCollection/*02000015*/ context,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timeout,
                               int32 blockSize,
                               bool rewindable,
                               bool returnImmediatley,
                               bool useAmendedQualifiers,
                               bool ensureLocatable,
                               bool prototypeOnly,
                               bool directRead,
                               bool enumerateDeep) cil managed
  // SIG: 20 0A 01 12 54 11 2D 08 02 02 02 02 02 02 02
  {
    // Method begins at RVA 0x2170
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::.ctor

  .method /*0600010A*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2174
    // Code size       0 (0x0)
  } // end of method EnumerationOptions::Clone

  .property /*17000027*/ instance callconv(8) bool
          ReturnImmediately()
  {
    .get instance bool System.Management.EnumerationOptions/*02000022*/::get_ReturnImmediately() /* 060000F8 */
    .set instance void System.Management.EnumerationOptions/*02000022*/::set_ReturnImmediately(bool) /* 060000F9 */
  } // end of property EnumerationOptions::ReturnImmediately
  .property /*17000028*/ instance callconv(8) int32
          BlockSize()
  {
    .set instance void System.Management.EnumerationOptions/*02000022*/::set_BlockSize(int32) /* 060000FB */
    .get instance int32 System.Management.EnumerationOptions/*02000022*/::get_BlockSize() /* 060000FA */
  } // end of property EnumerationOptions::BlockSize
  .property /*17000029*/ instance callconv(8) bool
          Rewindable()
  {
    .get instance bool System.Management.EnumerationOptions/*02000022*/::get_Rewindable() /* 060000FC */
    .set instance void System.Management.EnumerationOptions/*02000022*/::set_Rewindable(bool) /* 060000FD */
  } // end of property EnumerationOptions::Rewindable
  .property /*1700002A*/ instance callconv(8) bool
          UseAmendedQualifiers()
  {
    .set instance void System.Management.EnumerationOptions/*02000022*/::set_UseAmendedQualifiers(bool) /* 060000FF */
    .get instance bool System.Management.EnumerationOptions/*02000022*/::get_UseAmendedQualifiers() /* 060000FE */
  } // end of property EnumerationOptions::UseAmendedQualifiers
  .property /*1700002B*/ instance callconv(8) bool
          EnsureLocatable()
  {
    .get instance bool System.Management.EnumerationOptions/*02000022*/::get_EnsureLocatable() /* 06000100 */
    .set instance void System.Management.EnumerationOptions/*02000022*/::set_EnsureLocatable(bool) /* 06000101 */
  } // end of property EnumerationOptions::EnsureLocatable
  .property /*1700002C*/ instance callconv(8) bool
          PrototypeOnly()
  {
    .get instance bool System.Management.EnumerationOptions/*02000022*/::get_PrototypeOnly() /* 06000102 */
    .set instance void System.Management.EnumerationOptions/*02000022*/::set_PrototypeOnly(bool) /* 06000103 */
  } // end of property EnumerationOptions::PrototypeOnly
  .property /*1700002D*/ instance callconv(8) bool
          DirectRead()
  {
    .get instance bool System.Management.EnumerationOptions/*02000022*/::get_DirectRead() /* 06000104 */
    .set instance void System.Management.EnumerationOptions/*02000022*/::set_DirectRead(bool) /* 06000105 */
  } // end of property EnumerationOptions::DirectRead
  .property /*1700002E*/ instance callconv(8) bool
          EnumerateDeep()
  {
    .get instance bool System.Management.EnumerationOptions/*02000022*/::get_EnumerateDeep() /* 06000106 */
    .set instance void System.Management.EnumerationOptions/*02000022*/::set_EnumerateDeep(bool) /* 06000107 */
  } // end of property EnumerationOptions::EnumerateDeep
} // end of class System.Management.EnumerationOptions

.class /*02000023*/ public auto ansi beforefieldinit System.Management.EventWatcherOptions
       extends System.Management.ManagementOptions/*02000021*/
{
  .method /*0600010B*/ public hidebysig specialname 
          instance int32  get_BlockSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2180
    // Code size       0 (0x0)
  } // end of method EventWatcherOptions::get_BlockSize

  .method /*0600010C*/ public hidebysig specialname 
          instance void  set_BlockSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2181
    // Code size       0 (0x0)
  } // end of method EventWatcherOptions::set_BlockSize

  .method /*0600010D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2182
    // Code size       0 (0x0)
  } // end of method EventWatcherOptions::.ctor

  .method /*0600010E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementNamedValueCollection/*02000015*/ context,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timeout,
                               int32 blockSize) cil managed
  // SIG: 20 03 01 12 54 11 2D 08
  {
    // Method begins at RVA 0x2183
    // Code size       0 (0x0)
  } // end of method EventWatcherOptions::.ctor

  .method /*0600010F*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2184
    // Code size       0 (0x0)
  } // end of method EventWatcherOptions::Clone

  .property /*1700002F*/ instance callconv(8) int32
          BlockSize()
  {
    .set instance void System.Management.EventWatcherOptions/*02000023*/::set_BlockSize(int32) /* 0600010C */
    .get instance int32 System.Management.EventWatcherOptions/*02000023*/::get_BlockSize() /* 0600010B */
  } // end of property EventWatcherOptions::BlockSize
} // end of class System.Management.EventWatcherOptions

.class /*02000024*/ public auto ansi beforefieldinit System.Management.ObjectGetOptions
       extends System.Management.ManagementOptions/*02000021*/
{
  .method /*06000110*/ public hidebysig specialname 
          instance bool  get_UseAmendedQualifiers() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2185
    // Code size       0 (0x0)
  } // end of method ObjectGetOptions::get_UseAmendedQualifiers

  .method /*06000111*/ public hidebysig specialname 
          instance void  set_UseAmendedQualifiers(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2186
    // Code size       0 (0x0)
  } // end of method ObjectGetOptions::set_UseAmendedQualifiers

  .method /*06000112*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2187
    // Code size       0 (0x0)
  } // end of method ObjectGetOptions::.ctor

  .method /*06000113*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementNamedValueCollection/*02000015*/ context) cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x2188
    // Code size       0 (0x0)
  } // end of method ObjectGetOptions::.ctor

  .method /*06000114*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementNamedValueCollection/*02000015*/ context,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timeout,
                               bool useAmendedQualifiers) cil managed
  // SIG: 20 03 01 12 54 11 2D 02
  {
    // Method begins at RVA 0x2189
    // Code size       0 (0x0)
  } // end of method ObjectGetOptions::.ctor

  .method /*06000115*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x218a
    // Code size       0 (0x0)
  } // end of method ObjectGetOptions::Clone

  .property /*17000030*/ instance callconv(8) bool
          UseAmendedQualifiers()
  {
    .set instance void System.Management.ObjectGetOptions/*02000024*/::set_UseAmendedQualifiers(bool) /* 06000111 */
    .get instance bool System.Management.ObjectGetOptions/*02000024*/::get_UseAmendedQualifiers() /* 06000110 */
  } // end of property ObjectGetOptions::UseAmendedQualifiers
} // end of class System.Management.ObjectGetOptions

.class /*02000025*/ public auto ansi beforefieldinit System.Management.PutOptions
       extends System.Management.ManagementOptions/*02000021*/
{
  .method /*06000116*/ public hidebysig specialname 
          instance bool  get_UseAmendedQualifiers() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x218b
    // Code size       0 (0x0)
  } // end of method PutOptions::get_UseAmendedQualifiers

  .method /*06000117*/ public hidebysig specialname 
          instance void  set_UseAmendedQualifiers(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x218c
    // Code size       0 (0x0)
  } // end of method PutOptions::set_UseAmendedQualifiers

  .method /*06000118*/ public hidebysig specialname 
          instance valuetype System.Management.PutType/*02000020*/ 
          get_Type() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x218d
    // Code size       0 (0x0)
  } // end of method PutOptions::get_Type

  .method /*06000119*/ public hidebysig specialname 
          instance void  set_Type(valuetype System.Management.PutType/*02000020*/ 'value') cil managed
  // SIG: 20 01 01 11 80 80
  {
    // Method begins at RVA 0x218e
    // Code size       0 (0x0)
  } // end of method PutOptions::set_Type

  .method /*0600011A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x218f
    // Code size       0 (0x0)
  } // end of method PutOptions::.ctor

  .method /*0600011B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementNamedValueCollection/*02000015*/ context) cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x2190
    // Code size       0 (0x0)
  } // end of method PutOptions::.ctor

  .method /*0600011C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementNamedValueCollection/*02000015*/ context,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timeout,
                               bool useAmendedQualifiers,
                               valuetype System.Management.PutType/*02000020*/ putType) cil managed
  // SIG: 20 04 01 12 54 11 2D 02 11 80 80
  {
    // Method begins at RVA 0x2191
    // Code size       0 (0x0)
  } // end of method PutOptions::.ctor

  .method /*0600011D*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2192
    // Code size       0 (0x0)
  } // end of method PutOptions::Clone

  .property /*17000031*/ instance callconv(8) bool
          UseAmendedQualifiers()
  {
    .set instance void System.Management.PutOptions/*02000025*/::set_UseAmendedQualifiers(bool) /* 06000117 */
    .get instance bool System.Management.PutOptions/*02000025*/::get_UseAmendedQualifiers() /* 06000116 */
  } // end of property PutOptions::UseAmendedQualifiers
  .property /*17000032*/ instance callconv(8) valuetype System.Management.PutType/*02000020*/
          Type()
  {
    .set instance void System.Management.PutOptions/*02000025*/::set_Type(valuetype System.Management.PutType/*02000020*/) /* 06000119 */
    .get instance valuetype System.Management.PutType/*02000020*/ System.Management.PutOptions/*02000025*/::get_Type() /* 06000118 */
  } // end of property PutOptions::Type
} // end of class System.Management.PutOptions

.class /*02000026*/ public auto ansi beforefieldinit System.Management.DeleteOptions
       extends System.Management.ManagementOptions/*02000021*/
{
  .method /*0600011E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2193
    // Code size       0 (0x0)
  } // end of method DeleteOptions::.ctor

  .method /*0600011F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementNamedValueCollection/*02000015*/ context,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timeout) cil managed
  // SIG: 20 02 01 12 54 11 2D
  {
    // Method begins at RVA 0x2194
    // Code size       0 (0x0)
  } // end of method DeleteOptions::.ctor

  .method /*06000120*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2195
    // Code size       0 (0x0)
  } // end of method DeleteOptions::Clone

} // end of class System.Management.DeleteOptions

.class /*02000027*/ public auto ansi beforefieldinit System.Management.InvokeMethodOptions
       extends System.Management.ManagementOptions/*02000021*/
{
  .method /*06000121*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2196
    // Code size       0 (0x0)
  } // end of method InvokeMethodOptions::.ctor

  .method /*06000122*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementNamedValueCollection/*02000015*/ context,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timeout) cil managed
  // SIG: 20 02 01 12 54 11 2D
  {
    // Method begins at RVA 0x2197
    // Code size       0 (0x0)
  } // end of method InvokeMethodOptions::.ctor

  .method /*06000123*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2198
    // Code size       0 (0x0)
  } // end of method InvokeMethodOptions::Clone

} // end of class System.Management.InvokeMethodOptions

.class /*02000028*/ public auto ansi beforefieldinit System.Management.ConnectionOptions
       extends System.Management.ManagementOptions/*02000021*/
{
  .method /*06000124*/ public hidebysig specialname 
          instance string  get_Locale() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2199
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::get_Locale

  .method /*06000125*/ public hidebysig specialname 
          instance void  set_Locale(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x219a
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::set_Locale

  .method /*06000126*/ public hidebysig specialname 
          instance string  get_Username() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x219b
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::get_Username

  .method /*06000127*/ public hidebysig specialname 
          instance void  set_Username(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x219c
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::set_Username

  .method /*06000128*/ public hidebysig specialname 
          instance void  set_Password(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x219d
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::set_Password

  .method /*06000129*/ public hidebysig specialname 
          instance void  set_SecurePassword(class [mscorlib/*23000001*/]System.Security.SecureString/*01000018*/ 'value') cil managed
  // SIG: 20 01 01 12 61
  {
    // Method begins at RVA 0x219e
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::set_SecurePassword

  .method /*0600012A*/ public hidebysig specialname 
          instance string  get_Authority() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x219f
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::get_Authority

  .method /*0600012B*/ public hidebysig specialname 
          instance void  set_Authority(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21a0
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::set_Authority

  .method /*0600012C*/ public hidebysig specialname 
          instance valuetype System.Management.ImpersonationLevel/*0200001F*/ 
          get_Impersonation() cil managed
  // SIG: 20 00 11 7C
  {
    // Method begins at RVA 0x21a1
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::get_Impersonation

  .method /*0600012D*/ public hidebysig specialname 
          instance void  set_Impersonation(valuetype System.Management.ImpersonationLevel/*0200001F*/ 'value') cil managed
  // SIG: 20 01 01 11 7C
  {
    // Method begins at RVA 0x21a2
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::set_Impersonation

  .method /*0600012E*/ public hidebysig specialname 
          instance valuetype System.Management.AuthenticationLevel/*0200001E*/ 
          get_Authentication() cil managed
  // SIG: 20 00 11 78
  {
    // Method begins at RVA 0x21a3
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::get_Authentication

  .method /*0600012F*/ public hidebysig specialname 
          instance void  set_Authentication(valuetype System.Management.AuthenticationLevel/*0200001E*/ 'value') cil managed
  // SIG: 20 01 01 11 78
  {
    // Method begins at RVA 0x21a4
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::set_Authentication

  .method /*06000130*/ public hidebysig specialname 
          instance bool  get_EnablePrivileges() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a5
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::get_EnablePrivileges

  .method /*06000131*/ public hidebysig specialname 
          instance void  set_EnablePrivileges(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21a6
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::set_EnablePrivileges

  .method /*06000132*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21a8
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::.ctor

  .method /*06000133*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string locale,
                               string username,
                               string password,
                               string authority,
                               valuetype System.Management.ImpersonationLevel/*0200001F*/ impersonation,
                               valuetype System.Management.AuthenticationLevel/*0200001E*/ authentication,
                               bool enablePrivileges,
                               class System.Management.ManagementNamedValueCollection/*02000015*/ context,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timeout) cil managed
  // SIG: 20 09 01 0E 0E 0E 0E 11 7C 11 78 02 12 54 11 2D
  {
    // Method begins at RVA 0x21b4
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::.ctor

  .method /*06000134*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string locale,
                               string username,
                               class [mscorlib/*23000001*/]System.Security.SecureString/*01000018*/ password,
                               string authority,
                               valuetype System.Management.ImpersonationLevel/*0200001F*/ impersonation,
                               valuetype System.Management.AuthenticationLevel/*0200001E*/ authentication,
                               bool enablePrivileges,
                               class System.Management.ManagementNamedValueCollection/*02000015*/ context,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ timeout) cil managed
  // SIG: 20 09 01 0E 0E 12 61 0E 11 7C 11 78 02 12 54 11 2D
  {
    // Method begins at RVA 0x21b5
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::.ctor

  .method /*06000135*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x21b8
    // Code size       0 (0x0)
  } // end of method ConnectionOptions::Clone

  .property /*17000033*/ instance callconv(8) string
          Locale()
  {
    .set instance void System.Management.ConnectionOptions/*02000028*/::set_Locale(string) /* 06000125 */
    .get instance string System.Management.ConnectionOptions/*02000028*/::get_Locale() /* 06000124 */
  } // end of property ConnectionOptions::Locale
  .property /*17000034*/ instance callconv(8) string
          Username()
  {
    .set instance void System.Management.ConnectionOptions/*02000028*/::set_Username(string) /* 06000127 */
    .get instance string System.Management.ConnectionOptions/*02000028*/::get_Username() /* 06000126 */
  } // end of property ConnectionOptions::Username
  .property /*17000035*/ instance callconv(8) string
          Password()
  {
    .set instance void System.Management.ConnectionOptions/*02000028*/::set_Password(string) /* 06000128 */
  } // end of property ConnectionOptions::Password
  .property /*17000036*/ instance callconv(8) class [mscorlib/*23000001*/]System.Security.SecureString/*01000018*/
          SecurePassword()
  {
    .set instance void System.Management.ConnectionOptions/*02000028*/::set_SecurePassword(class [mscorlib/*23000001*/]System.Security.SecureString/*01000018*/) /* 06000129 */
  } // end of property ConnectionOptions::SecurePassword
  .property /*17000037*/ instance callconv(8) string
          Authority()
  {
    .get instance string System.Management.ConnectionOptions/*02000028*/::get_Authority() /* 0600012A */
    .set instance void System.Management.ConnectionOptions/*02000028*/::set_Authority(string) /* 0600012B */
  } // end of property ConnectionOptions::Authority
  .property /*17000038*/ instance callconv(8) valuetype System.Management.ImpersonationLevel/*0200001F*/
          Impersonation()
  {
    .get instance valuetype System.Management.ImpersonationLevel/*0200001F*/ System.Management.ConnectionOptions/*02000028*/::get_Impersonation() /* 0600012C */
    .set instance void System.Management.ConnectionOptions/*02000028*/::set_Impersonation(valuetype System.Management.ImpersonationLevel/*0200001F*/) /* 0600012D */
  } // end of property ConnectionOptions::Impersonation
  .property /*17000039*/ instance callconv(8) valuetype System.Management.AuthenticationLevel/*0200001E*/
          Authentication()
  {
    .set instance void System.Management.ConnectionOptions/*02000028*/::set_Authentication(valuetype System.Management.AuthenticationLevel/*0200001E*/) /* 0600012F */
    .get instance valuetype System.Management.AuthenticationLevel/*0200001E*/ System.Management.ConnectionOptions/*02000028*/::get_Authentication() /* 0600012E */
  } // end of property ConnectionOptions::Authentication
  .property /*1700003A*/ instance callconv(8) bool
          EnablePrivileges()
  {
    .set instance void System.Management.ConnectionOptions/*02000028*/::set_EnablePrivileges(bool) /* 06000131 */
    .get instance bool System.Management.ConnectionOptions/*02000028*/::get_EnablePrivileges() /* 06000130 */
  } // end of property ConnectionOptions::EnablePrivileges
} // end of class System.Management.ConnectionOptions

.class /*02000029*/ public auto ansi beforefieldinit System.Management.ManagementPath
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements [mscorlib/*23000001*/]System.ICloneable/*01000006*/
{
  .custom /*0C00001C:0A00001C*/ instance void [System/*23000002*/]System.ComponentModel.TypeConverterAttribute/*01000039*/::.ctor(class [mscorlib/*23000001*/]System.Type/*0100001B*/) /* 0A00001C */ = ( 01 00 29 53 79 73 74 65 6D 2E 4D 61 6E 61 67 65   // ..)System.Manage
                                                                                                                                                                                                          6D 65 6E 74 2E 4D 61 6E 61 67 65 6D 65 6E 74 50   // ment.ManagementP
                                                                                                                                                                                                          61 74 68 43 6F 6E 76 65 72 74 65 72 00 00 )       // athConverter..
  .method /*06000136*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21c4
    // Code size       0 (0x0)
  } // end of method ManagementPath::.ctor

  .method /*06000137*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string path) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21c5
    // Code size       0 (0x0)
  } // end of method ManagementPath::.ctor

  .method /*06000138*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21c6
    // Code size       0 (0x0)
  } // end of method ManagementPath::ToString

  .method /*06000139*/ public hidebysig instance class System.Management.ManagementPath/*02000029*/ 
          Clone() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x21c7
    // Code size       0 (0x0)
  } // end of method ManagementPath::Clone

  .method /*0600013A*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [mscorlib/*23000001*/]System.ICloneable/*01000006*/::Clone /*01000006::0A000004*/ 
    // Method begins at RVA 0x21c8
    // Code size       0 (0x0)
  } // end of method ManagementPath::System.ICloneable.Clone

  .method /*0600013B*/ public hidebysig specialname static 
          class System.Management.ManagementPath/*02000029*/ 
          get_DefaultPath() cil managed
  // SIG: 00 00 12 80 A4
  {
    // Method begins at RVA 0x21c9
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_DefaultPath

  .method /*0600013C*/ public hidebysig specialname static 
          void  set_DefaultPath(class System.Management.ManagementPath/*02000029*/ 'value') cil managed
  // SIG: 00 01 01 12 80 A4
  {
    // Method begins at RVA 0x21ca
    // Code size       0 (0x0)
  } // end of method ManagementPath::set_DefaultPath

  .method /*0600013D*/ public hidebysig instance void 
          SetAsClass() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21cb
    // Code size       0 (0x0)
  } // end of method ManagementPath::SetAsClass

  .method /*0600013E*/ public hidebysig instance void 
          SetAsSingleton() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21cc
    // Code size       0 (0x0)
  } // end of method ManagementPath::SetAsSingleton

  .method /*0600013F*/ public hidebysig specialname 
          instance string  get_Path() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21cd
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_Path

  .method /*06000140*/ public hidebysig specialname 
          instance void  set_Path(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21ce
    // Code size       0 (0x0)
  } // end of method ManagementPath::set_Path

  .method /*06000141*/ public hidebysig specialname 
          instance string  get_RelativePath() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21cf
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_RelativePath

  .method /*06000142*/ public hidebysig specialname 
          instance void  set_RelativePath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21d0
    // Code size       0 (0x0)
  } // end of method ManagementPath::set_RelativePath

  .method /*06000143*/ public hidebysig specialname 
          instance string  get_Server() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21d1
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_Server

  .method /*06000144*/ public hidebysig specialname 
          instance void  set_Server(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21d2
    // Code size       0 (0x0)
  } // end of method ManagementPath::set_Server

  .method /*06000145*/ public hidebysig specialname 
          instance string  get_NamespacePath() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21d3
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_NamespacePath

  .method /*06000146*/ public hidebysig specialname 
          instance void  set_NamespacePath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21d4
    // Code size       0 (0x0)
  } // end of method ManagementPath::set_NamespacePath

  .method /*06000147*/ public hidebysig specialname 
          instance string  get_ClassName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21d5
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_ClassName

  .method /*06000148*/ public hidebysig specialname 
          instance void  set_ClassName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21d6
    // Code size       0 (0x0)
  } // end of method ManagementPath::set_ClassName

  .method /*06000149*/ public hidebysig specialname 
          instance bool  get_IsClass() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21d7
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_IsClass

  .method /*0600014A*/ public hidebysig specialname 
          instance bool  get_IsInstance() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21d8
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_IsInstance

  .method /*0600014B*/ public hidebysig specialname 
          instance bool  get_IsSingleton() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21d9
    // Code size       0 (0x0)
  } // end of method ManagementPath::get_IsSingleton

  .method /*0600014C*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x21da
    // Code size       0 (0x0)
  } // end of method ManagementPath::.cctor

  .property /*1700003B*/ callconv(8) class System.Management.ManagementPath/*02000029*/
          DefaultPath()
  {
    .set void System.Management.ManagementPath/*02000029*/::set_DefaultPath(class System.Management.ManagementPath/*02000029*/) /* 0600013C */
    .get class System.Management.ManagementPath/*02000029*/ System.Management.ManagementPath/*02000029*/::get_DefaultPath() /* 0600013B */
  } // end of property ManagementPath::DefaultPath
  .property /*1700003C*/ instance callconv(8) string
          Path()
  {
    .custom /*0C000021:0A00001D*/ instance void [System/*23000002*/]System.ComponentModel.RefreshPropertiesAttribute/*0100003A*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.RefreshProperties/*0100003B*/) /* 0A00001D */ = ( 01 00 01 00 00 00 00 00 ) 
    .set instance void System.Management.ManagementPath/*02000029*/::set_Path(string) /* 06000140 */
    .get instance string System.Management.ManagementPath/*02000029*/::get_Path() /* 0600013F */
  } // end of property ManagementPath::Path
  .property /*1700003D*/ instance callconv(8) string
          RelativePath()
  {
    .custom /*0C000023:0A00001D*/ instance void [System/*23000002*/]System.ComponentModel.RefreshPropertiesAttribute/*0100003A*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.RefreshProperties/*0100003B*/) /* 0A00001D */ = ( 01 00 01 00 00 00 00 00 ) 
    .get instance string System.Management.ManagementPath/*02000029*/::get_RelativePath() /* 06000141 */
    .set instance void System.Management.ManagementPath/*02000029*/::set_RelativePath(string) /* 06000142 */
  } // end of property ManagementPath::RelativePath
  .property /*1700003E*/ instance callconv(8) string
          Server()
  {
    .custom /*0C000024:0A00001D*/ instance void [System/*23000002*/]System.ComponentModel.RefreshPropertiesAttribute/*0100003A*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.RefreshProperties/*0100003B*/) /* 0A00001D */ = ( 01 00 01 00 00 00 00 00 ) 
    .set instance void System.Management.ManagementPath/*02000029*/::set_Server(string) /* 06000144 */
    .get instance string System.Management.ManagementPath/*02000029*/::get_Server() /* 06000143 */
  } // end of property ManagementPath::Server
  .property /*1700003F*/ instance callconv(8) string
          NamespacePath()
  {
    .custom /*0C000025:0A00001D*/ instance void [System/*23000002*/]System.ComponentModel.RefreshPropertiesAttribute/*0100003A*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.RefreshProperties/*0100003B*/) /* 0A00001D */ = ( 01 00 01 00 00 00 00 00 ) 
    .get instance string System.Management.ManagementPath/*02000029*/::get_NamespacePath() /* 06000145 */
    .set instance void System.Management.ManagementPath/*02000029*/::set_NamespacePath(string) /* 06000146 */
  } // end of property ManagementPath::NamespacePath
  .property /*17000040*/ instance callconv(8) string
          ClassName()
  {
    .custom /*0C000027:0A00001D*/ instance void [System/*23000002*/]System.ComponentModel.RefreshPropertiesAttribute/*0100003A*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.RefreshProperties/*0100003B*/) /* 0A00001D */ = ( 01 00 01 00 00 00 00 00 ) 
    .set instance void System.Management.ManagementPath/*02000029*/::set_ClassName(string) /* 06000148 */
    .get instance string System.Management.ManagementPath/*02000029*/::get_ClassName() /* 06000147 */
  } // end of property ManagementPath::ClassName
  .property /*17000041*/ instance callconv(8) bool
          IsClass()
  {
    .get instance bool System.Management.ManagementPath/*02000029*/::get_IsClass() /* 06000149 */
  } // end of property ManagementPath::IsClass
  .property /*17000042*/ instance callconv(8) bool
          IsInstance()
  {
    .get instance bool System.Management.ManagementPath/*02000029*/::get_IsInstance() /* 0600014A */
  } // end of property ManagementPath::IsInstance
  .property /*17000043*/ instance callconv(8) bool
          IsSingleton()
  {
    .get instance bool System.Management.ManagementPath/*02000029*/::get_IsSingleton() /* 0600014B */
  } // end of property ManagementPath::IsSingleton
} // end of class System.Management.ManagementPath

.class /*0200002A*/ private auto ansi beforefieldinit System.Management.ManagementPathConverter
       extends [System/*23000002*/]System.ComponentModel.ExpandableObjectConverter/*01000019*/
{
  .method /*0600014D*/ public hidebysig virtual 
          instance bool  CanConvertFrom(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                        class [mscorlib/*23000001*/]System.Type/*0100001B*/ sourceType) cil managed
  // SIG: 20 02 02 12 69 12 6D
  {
    // Method begins at RVA 0x21db
    // Code size       0 (0x0)
  } // end of method ManagementPathConverter::CanConvertFrom

  .method /*0600014E*/ public hidebysig virtual 
          instance bool  CanConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                      class [mscorlib/*23000001*/]System.Type/*0100001B*/ destinationType) cil managed
  // SIG: 20 02 02 12 69 12 6D
  {
    // Method begins at RVA 0x21dc
    // Code size       0 (0x0)
  } // end of method ManagementPathConverter::CanConvertTo

  .method /*0600014F*/ public hidebysig virtual 
          instance object  ConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100001C*/ culture,
                                     object 'value',
                                     class [mscorlib/*23000001*/]System.Type/*0100001B*/ destinationType) cil managed
  // SIG: 20 04 1C 12 69 12 71 1C 12 6D
  {
    // Method begins at RVA 0x21dd
    // Code size       0 (0x0)
  } // end of method ManagementPathConverter::ConvertTo

  .method /*06000150*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21de
    // Code size       0 (0x0)
  } // end of method ManagementPathConverter::.ctor

} // end of class System.Management.ManagementPathConverter

.class /*0200002B*/ public abstract auto ansi beforefieldinit System.Management.ManagementQuery
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements [mscorlib/*23000001*/]System.ICloneable/*01000006*/
{
  .custom /*0C00001D:0A00001C*/ instance void [System/*23000002*/]System.ComponentModel.TypeConverterAttribute/*01000039*/::.ctor(class [mscorlib/*23000001*/]System.Type/*0100001B*/) /* 0A00001C */ = ( 01 00 2A 53 79 73 74 65 6D 2E 4D 61 6E 61 67 65   // ..*System.Manage
                                                                                                                                                                                                          6D 65 6E 74 2E 4D 61 6E 61 67 65 6D 65 6E 74 51   // ment.ManagementQ
                                                                                                                                                                                                          75 65 72 79 43 6F 6E 76 65 72 74 65 72 00 00 )    // ueryConverter..
  .method /*06000151*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21df
    // Code size       0 (0x0)
  } // end of method ManagementQuery::.ctor

  .method /*06000152*/ famorassem hidebysig newslot virtual 
          instance void  ParseQuery(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21e0
    // Code size       0 (0x0)
  } // end of method ManagementQuery::ParseQuery

  .method /*06000153*/ public hidebysig newslot specialname virtual 
          instance string  get_QueryString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21e1
    // Code size       0 (0x0)
  } // end of method ManagementQuery::get_QueryString

  .method /*06000154*/ public hidebysig newslot specialname virtual 
          instance void  set_QueryString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21e2
    // Code size       0 (0x0)
  } // end of method ManagementQuery::set_QueryString

  .method /*06000155*/ public hidebysig newslot specialname virtual 
          instance string  get_QueryLanguage() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21e3
    // Code size       0 (0x0)
  } // end of method ManagementQuery::get_QueryLanguage

  .method /*06000156*/ public hidebysig newslot specialname virtual 
          instance void  set_QueryLanguage(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21e4
    // Code size       0 (0x0)
  } // end of method ManagementQuery::set_QueryLanguage

  .method /*06000157*/ public hidebysig newslot abstract virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method ManagementQuery::Clone

  .method /*06000158*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x21e5
    // Code size       0 (0x0)
  } // end of method ManagementQuery::.cctor

  .property /*17000044*/ instance callconv(8) string
          QueryString()
  {
    .get instance string System.Management.ManagementQuery/*0200002B*/::get_QueryString() /* 06000153 */
    .set instance void System.Management.ManagementQuery/*0200002B*/::set_QueryString(string) /* 06000154 */
  } // end of property ManagementQuery::QueryString
  .property /*17000045*/ instance callconv(8) string
          QueryLanguage()
  {
    .get instance string System.Management.ManagementQuery/*0200002B*/::get_QueryLanguage() /* 06000155 */
    .set instance void System.Management.ManagementQuery/*0200002B*/::set_QueryLanguage(string) /* 06000156 */
  } // end of property ManagementQuery::QueryLanguage
} // end of class System.Management.ManagementQuery

.class /*0200002C*/ public auto ansi beforefieldinit System.Management.ObjectQuery
       extends System.Management.ManagementQuery/*0200002B*/
{
  .method /*06000159*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e6
    // Code size       0 (0x0)
  } // end of method ObjectQuery::.ctor

  .method /*0600015A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21e7
    // Code size       0 (0x0)
  } // end of method ObjectQuery::.ctor

  .method /*0600015B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string language,
                               string query) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x21e8
    // Code size       0 (0x0)
  } // end of method ObjectQuery::.ctor

  .method /*0600015C*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x21e9
    // Code size       0 (0x0)
  } // end of method ObjectQuery::Clone

} // end of class System.Management.ObjectQuery

.class /*0200002D*/ public auto ansi beforefieldinit System.Management.EventQuery
       extends System.Management.ManagementQuery/*0200002B*/
{
  .method /*0600015D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ea
    // Code size       0 (0x0)
  } // end of method EventQuery::.ctor

  .method /*0600015E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21eb
    // Code size       0 (0x0)
  } // end of method EventQuery::.ctor

  .method /*0600015F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string language,
                               string query) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x21ec
    // Code size       0 (0x0)
  } // end of method EventQuery::.ctor

  .method /*06000160*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x21ed
    // Code size       0 (0x0)
  } // end of method EventQuery::Clone

} // end of class System.Management.EventQuery

.class /*0200002E*/ public auto ansi beforefieldinit System.Management.WqlObjectQuery
       extends System.Management.ObjectQuery/*0200002C*/
{
  .method /*06000161*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ee
    // Code size       0 (0x0)
  } // end of method WqlObjectQuery::.ctor

  .method /*06000162*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21ef
    // Code size       0 (0x0)
  } // end of method WqlObjectQuery::.ctor

  .method /*06000163*/ public hidebysig specialname virtual 
          instance string  get_QueryLanguage() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f0
    // Code size       0 (0x0)
  } // end of method WqlObjectQuery::get_QueryLanguage

  .method /*06000164*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x21f1
    // Code size       0 (0x0)
  } // end of method WqlObjectQuery::Clone

  .property /*17000046*/ instance callconv(8) string
          QueryLanguage()
  {
    .get instance string System.Management.WqlObjectQuery/*0200002E*/::get_QueryLanguage() /* 06000163 */
  } // end of property WqlObjectQuery::QueryLanguage
} // end of class System.Management.WqlObjectQuery

.class /*0200002F*/ public auto ansi beforefieldinit System.Management.SelectQuery
       extends System.Management.WqlObjectQuery/*0200002E*/
{
  .method /*06000165*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21f2
    // Code size       0 (0x0)
  } // end of method SelectQuery::.ctor

  .method /*06000166*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string queryOrClassName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21f3
    // Code size       0 (0x0)
  } // end of method SelectQuery::.ctor

  .method /*06000167*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string className,
                               string condition) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x21f4
    // Code size       0 (0x0)
  } // end of method SelectQuery::.ctor

  .method /*06000168*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string className,
                               string condition,
                               string[] selectedProperties) cil managed
  // SIG: 20 03 01 0E 0E 1D 0E
  {
    // Method begins at RVA 0x21f5
    // Code size       0 (0x0)
  } // end of method SelectQuery::.ctor

  .method /*06000169*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool isSchemaQuery,
                               string condition) cil managed
  // SIG: 20 02 01 02 0E
  {
    // Method begins at RVA 0x21f6
    // Code size       0 (0x0)
  } // end of method SelectQuery::.ctor

  .method /*0600016A*/ public hidebysig specialname virtual 
          instance string  get_QueryString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f7
    // Code size       0 (0x0)
  } // end of method SelectQuery::get_QueryString

  .method /*0600016B*/ public hidebysig specialname virtual 
          instance void  set_QueryString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21f8
    // Code size       0 (0x0)
  } // end of method SelectQuery::set_QueryString

  .method /*0600016C*/ public hidebysig specialname 
          instance bool  get_IsSchemaQuery() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f9
    // Code size       0 (0x0)
  } // end of method SelectQuery::get_IsSchemaQuery

  .method /*0600016D*/ public hidebysig specialname 
          instance void  set_IsSchemaQuery(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21fa
    // Code size       0 (0x0)
  } // end of method SelectQuery::set_IsSchemaQuery

  .method /*0600016E*/ public hidebysig specialname 
          instance string  get_ClassName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21fb
    // Code size       0 (0x0)
  } // end of method SelectQuery::get_ClassName

  .method /*0600016F*/ public hidebysig specialname 
          instance void  set_ClassName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21fc
    // Code size       0 (0x0)
  } // end of method SelectQuery::set_ClassName

  .method /*06000170*/ public hidebysig specialname 
          instance string  get_Condition() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21fd
    // Code size       0 (0x0)
  } // end of method SelectQuery::get_Condition

  .method /*06000171*/ public hidebysig specialname 
          instance void  set_Condition(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21fe
    // Code size       0 (0x0)
  } // end of method SelectQuery::set_Condition

  .method /*06000172*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/ 
          get_SelectedProperties() cil managed
  // SIG: 20 00 12 21
  {
    // Method begins at RVA 0x21ff
    // Code size       0 (0x0)
  } // end of method SelectQuery::get_SelectedProperties

  .method /*06000173*/ public hidebysig specialname 
          instance void  set_SelectedProperties(class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/ 'value') cil managed
  // SIG: 20 01 01 12 21
  {
    // Method begins at RVA 0x2200
    // Code size       0 (0x0)
  } // end of method SelectQuery::set_SelectedProperties

  .method /*06000174*/ famorassem hidebysig 
          instance void  BuildQuery() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2201
    // Code size       0 (0x0)
  } // end of method SelectQuery::BuildQuery

  .method /*06000175*/ famorassem hidebysig virtual 
          instance void  ParseQuery(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2202
    // Code size       0 (0x0)
  } // end of method SelectQuery::ParseQuery

  .method /*06000176*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2203
    // Code size       0 (0x0)
  } // end of method SelectQuery::Clone

  .property /*17000047*/ instance callconv(8) string
          QueryString()
  {
    .get instance string System.Management.SelectQuery/*0200002F*/::get_QueryString() /* 0600016A */
    .set instance void System.Management.SelectQuery/*0200002F*/::set_QueryString(string) /* 0600016B */
  } // end of property SelectQuery::QueryString
  .property /*17000048*/ instance callconv(8) bool
          IsSchemaQuery()
  {
    .set instance void System.Management.SelectQuery/*0200002F*/::set_IsSchemaQuery(bool) /* 0600016D */
    .get instance bool System.Management.SelectQuery/*0200002F*/::get_IsSchemaQuery() /* 0600016C */
  } // end of property SelectQuery::IsSchemaQuery
  .property /*17000049*/ instance callconv(8) string
          ClassName()
  {
    .get instance string System.Management.SelectQuery/*0200002F*/::get_ClassName() /* 0600016E */
    .set instance void System.Management.SelectQuery/*0200002F*/::set_ClassName(string) /* 0600016F */
  } // end of property SelectQuery::ClassName
  .property /*1700004A*/ instance callconv(8) string
          Condition()
  {
    .get instance string System.Management.SelectQuery/*0200002F*/::get_Condition() /* 06000170 */
    .set instance void System.Management.SelectQuery/*0200002F*/::set_Condition(string) /* 06000171 */
  } // end of property SelectQuery::Condition
  .property /*1700004B*/ instance callconv(8) class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/
          SelectedProperties()
  {
    .get instance class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/ System.Management.SelectQuery/*0200002F*/::get_SelectedProperties() /* 06000172 */
    .set instance void System.Management.SelectQuery/*0200002F*/::set_SelectedProperties(class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/) /* 06000173 */
  } // end of property SelectQuery::SelectedProperties
} // end of class System.Management.SelectQuery

.class /*02000030*/ public auto ansi beforefieldinit System.Management.RelatedObjectQuery
       extends System.Management.WqlObjectQuery/*0200002E*/
{
  .method /*06000177*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2204
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::.ctor

  .method /*06000178*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string queryOrSourceObject) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2205
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::.ctor

  .method /*06000179*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string sourceObject,
                               string relatedClass) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2208
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::.ctor

  .method /*0600017A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string sourceObject,
                               string relatedClass,
                               string relationshipClass,
                               string relatedQualifier,
                               string relationshipQualifier,
                               string relatedRole,
                               string thisRole,
                               bool classDefinitionsOnly) cil managed
  // SIG: 20 08 01 0E 0E 0E 0E 0E 0E 0E 02
  {
    // Method begins at RVA 0x2214
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::.ctor

  .method /*0600017B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool isSchemaQuery,
                               string sourceObject,
                               string relatedClass,
                               string relationshipClass,
                               string relatedQualifier,
                               string relationshipQualifier,
                               string relatedRole,
                               string thisRole) cil managed
  // SIG: 20 08 01 02 0E 0E 0E 0E 0E 0E 0E
  {
    // Method begins at RVA 0x2215
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::.ctor

  .method /*0600017C*/ public hidebysig specialname 
          instance bool  get_IsSchemaQuery() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2216
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_IsSchemaQuery

  .method /*0600017D*/ public hidebysig specialname 
          instance void  set_IsSchemaQuery(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2217
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_IsSchemaQuery

  .method /*0600017E*/ public hidebysig specialname 
          instance string  get_SourceObject() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2218
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_SourceObject

  .method /*0600017F*/ public hidebysig specialname 
          instance void  set_SourceObject(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2219
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_SourceObject

  .method /*06000180*/ public hidebysig specialname 
          instance string  get_RelatedClass() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x221a
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_RelatedClass

  .method /*06000181*/ public hidebysig specialname 
          instance void  set_RelatedClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x221b
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_RelatedClass

  .method /*06000182*/ public hidebysig specialname 
          instance string  get_RelationshipClass() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x221c
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_RelationshipClass

  .method /*06000183*/ public hidebysig specialname 
          instance void  set_RelationshipClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x221d
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_RelationshipClass

  .method /*06000184*/ public hidebysig specialname 
          instance string  get_RelatedQualifier() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x221e
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_RelatedQualifier

  .method /*06000185*/ public hidebysig specialname 
          instance void  set_RelatedQualifier(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x221f
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_RelatedQualifier

  .method /*06000186*/ public hidebysig specialname 
          instance string  get_RelationshipQualifier() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2220
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_RelationshipQualifier

  .method /*06000187*/ public hidebysig specialname 
          instance void  set_RelationshipQualifier(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2221
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_RelationshipQualifier

  .method /*06000188*/ public hidebysig specialname 
          instance string  get_RelatedRole() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2222
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_RelatedRole

  .method /*06000189*/ public hidebysig specialname 
          instance void  set_RelatedRole(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2223
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_RelatedRole

  .method /*0600018A*/ public hidebysig specialname 
          instance string  get_ThisRole() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2224
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_ThisRole

  .method /*0600018B*/ public hidebysig specialname 
          instance void  set_ThisRole(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2225
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_ThisRole

  .method /*0600018C*/ public hidebysig specialname 
          instance bool  get_ClassDefinitionsOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2226
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::get_ClassDefinitionsOnly

  .method /*0600018D*/ public hidebysig specialname 
          instance void  set_ClassDefinitionsOnly(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2227
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::set_ClassDefinitionsOnly

  .method /*0600018E*/ famorassem hidebysig 
          instance void  BuildQuery() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2228
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::BuildQuery

  .method /*0600018F*/ famorassem hidebysig virtual 
          instance void  ParseQuery(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2229
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::ParseQuery

  .method /*06000190*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x222a
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::Clone

  .method /*06000191*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x222b
    // Code size       0 (0x0)
  } // end of method RelatedObjectQuery::.cctor

  .property /*1700004C*/ instance callconv(8) bool
          IsSchemaQuery()
  {
    .get instance bool System.Management.RelatedObjectQuery/*02000030*/::get_IsSchemaQuery() /* 0600017C */
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_IsSchemaQuery(bool) /* 0600017D */
  } // end of property RelatedObjectQuery::IsSchemaQuery
  .property /*1700004D*/ instance callconv(8) string
          SourceObject()
  {
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_SourceObject(string) /* 0600017F */
    .get instance string System.Management.RelatedObjectQuery/*02000030*/::get_SourceObject() /* 0600017E */
  } // end of property RelatedObjectQuery::SourceObject
  .property /*1700004E*/ instance callconv(8) string
          RelatedClass()
  {
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_RelatedClass(string) /* 06000181 */
    .get instance string System.Management.RelatedObjectQuery/*02000030*/::get_RelatedClass() /* 06000180 */
  } // end of property RelatedObjectQuery::RelatedClass
  .property /*1700004F*/ instance callconv(8) string
          RelationshipClass()
  {
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_RelationshipClass(string) /* 06000183 */
    .get instance string System.Management.RelatedObjectQuery/*02000030*/::get_RelationshipClass() /* 06000182 */
  } // end of property RelatedObjectQuery::RelationshipClass
  .property /*17000050*/ instance callconv(8) string
          RelatedQualifier()
  {
    .get instance string System.Management.RelatedObjectQuery/*02000030*/::get_RelatedQualifier() /* 06000184 */
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_RelatedQualifier(string) /* 06000185 */
  } // end of property RelatedObjectQuery::RelatedQualifier
  .property /*17000051*/ instance callconv(8) string
          RelationshipQualifier()
  {
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_RelationshipQualifier(string) /* 06000187 */
    .get instance string System.Management.RelatedObjectQuery/*02000030*/::get_RelationshipQualifier() /* 06000186 */
  } // end of property RelatedObjectQuery::RelationshipQualifier
  .property /*17000052*/ instance callconv(8) string
          RelatedRole()
  {
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_RelatedRole(string) /* 06000189 */
    .get instance string System.Management.RelatedObjectQuery/*02000030*/::get_RelatedRole() /* 06000188 */
  } // end of property RelatedObjectQuery::RelatedRole
  .property /*17000053*/ instance callconv(8) string
          ThisRole()
  {
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_ThisRole(string) /* 0600018B */
    .get instance string System.Management.RelatedObjectQuery/*02000030*/::get_ThisRole() /* 0600018A */
  } // end of property RelatedObjectQuery::ThisRole
  .property /*17000054*/ instance callconv(8) bool
          ClassDefinitionsOnly()
  {
    .get instance bool System.Management.RelatedObjectQuery/*02000030*/::get_ClassDefinitionsOnly() /* 0600018C */
    .set instance void System.Management.RelatedObjectQuery/*02000030*/::set_ClassDefinitionsOnly(bool) /* 0600018D */
  } // end of property RelatedObjectQuery::ClassDefinitionsOnly
} // end of class System.Management.RelatedObjectQuery

.class /*02000031*/ public auto ansi beforefieldinit System.Management.RelationshipQuery
       extends System.Management.WqlObjectQuery/*0200002E*/
{
  .method /*06000192*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x222c
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::.ctor

  .method /*06000193*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string queryOrSourceObject) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x222d
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::.ctor

  .method /*06000194*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string sourceObject,
                               string relationshipClass) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x222e
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::.ctor

  .method /*06000195*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string sourceObject,
                               string relationshipClass,
                               string relationshipQualifier,
                               string thisRole,
                               bool classDefinitionsOnly) cil managed
  // SIG: 20 05 01 0E 0E 0E 0E 02
  {
    // Method begins at RVA 0x222f
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::.ctor

  .method /*06000196*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool isSchemaQuery,
                               string sourceObject,
                               string relationshipClass,
                               string relationshipQualifier,
                               string thisRole) cil managed
  // SIG: 20 05 01 02 0E 0E 0E 0E
  {
    // Method begins at RVA 0x2230
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::.ctor

  .method /*06000197*/ public hidebysig specialname 
          instance bool  get_IsSchemaQuery() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2231
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::get_IsSchemaQuery

  .method /*06000198*/ public hidebysig specialname 
          instance void  set_IsSchemaQuery(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2232
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::set_IsSchemaQuery

  .method /*06000199*/ public hidebysig specialname 
          instance string  get_SourceObject() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2233
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::get_SourceObject

  .method /*0600019A*/ public hidebysig specialname 
          instance void  set_SourceObject(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2234
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::set_SourceObject

  .method /*0600019B*/ public hidebysig specialname 
          instance string  get_RelationshipClass() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2235
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::get_RelationshipClass

  .method /*0600019C*/ public hidebysig specialname 
          instance void  set_RelationshipClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2236
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::set_RelationshipClass

  .method /*0600019D*/ public hidebysig specialname 
          instance string  get_RelationshipQualifier() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2237
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::get_RelationshipQualifier

  .method /*0600019E*/ public hidebysig specialname 
          instance void  set_RelationshipQualifier(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2238
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::set_RelationshipQualifier

  .method /*0600019F*/ public hidebysig specialname 
          instance string  get_ThisRole() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2239
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::get_ThisRole

  .method /*060001A0*/ public hidebysig specialname 
          instance void  set_ThisRole(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x223a
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::set_ThisRole

  .method /*060001A1*/ public hidebysig specialname 
          instance bool  get_ClassDefinitionsOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x223b
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::get_ClassDefinitionsOnly

  .method /*060001A2*/ public hidebysig specialname 
          instance void  set_ClassDefinitionsOnly(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x223c
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::set_ClassDefinitionsOnly

  .method /*060001A3*/ famorassem hidebysig 
          instance void  BuildQuery() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x223d
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::BuildQuery

  .method /*060001A4*/ famorassem hidebysig virtual 
          instance void  ParseQuery(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x223e
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::ParseQuery

  .method /*060001A5*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x223f
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::Clone

  .method /*060001A6*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2240
    // Code size       0 (0x0)
  } // end of method RelationshipQuery::.cctor

  .property /*17000055*/ instance callconv(8) bool
          IsSchemaQuery()
  {
    .get instance bool System.Management.RelationshipQuery/*02000031*/::get_IsSchemaQuery() /* 06000197 */
    .set instance void System.Management.RelationshipQuery/*02000031*/::set_IsSchemaQuery(bool) /* 06000198 */
  } // end of property RelationshipQuery::IsSchemaQuery
  .property /*17000056*/ instance callconv(8) string
          SourceObject()
  {
    .get instance string System.Management.RelationshipQuery/*02000031*/::get_SourceObject() /* 06000199 */
    .set instance void System.Management.RelationshipQuery/*02000031*/::set_SourceObject(string) /* 0600019A */
  } // end of property RelationshipQuery::SourceObject
  .property /*17000057*/ instance callconv(8) string
          RelationshipClass()
  {
    .set instance void System.Management.RelationshipQuery/*02000031*/::set_RelationshipClass(string) /* 0600019C */
    .get instance string System.Management.RelationshipQuery/*02000031*/::get_RelationshipClass() /* 0600019B */
  } // end of property RelationshipQuery::RelationshipClass
  .property /*17000058*/ instance callconv(8) string
          RelationshipQualifier()
  {
    .get instance string System.Management.RelationshipQuery/*02000031*/::get_RelationshipQualifier() /* 0600019D */
    .set instance void System.Management.RelationshipQuery/*02000031*/::set_RelationshipQualifier(string) /* 0600019E */
  } // end of property RelationshipQuery::RelationshipQualifier
  .property /*17000059*/ instance callconv(8) string
          ThisRole()
  {
    .get instance string System.Management.RelationshipQuery/*02000031*/::get_ThisRole() /* 0600019F */
    .set instance void System.Management.RelationshipQuery/*02000031*/::set_ThisRole(string) /* 060001A0 */
  } // end of property RelationshipQuery::ThisRole
  .property /*1700005A*/ instance callconv(8) bool
          ClassDefinitionsOnly()
  {
    .get instance bool System.Management.RelationshipQuery/*02000031*/::get_ClassDefinitionsOnly() /* 060001A1 */
    .set instance void System.Management.RelationshipQuery/*02000031*/::set_ClassDefinitionsOnly(bool) /* 060001A2 */
  } // end of property RelationshipQuery::ClassDefinitionsOnly
} // end of class System.Management.RelationshipQuery

.class /*02000032*/ public auto ansi beforefieldinit System.Management.WqlEventQuery
       extends System.Management.EventQuery/*0200002D*/
{
  .method /*060001A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2241
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.ctor

  .method /*060001A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string queryOrEventClassName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2242
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.ctor

  .method /*060001A9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string eventClassName,
                               string condition) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2243
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.ctor

  .method /*060001AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string eventClassName,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ withinInterval) cil managed
  // SIG: 20 02 01 0E 11 2D
  {
    // Method begins at RVA 0x2244
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.ctor

  .method /*060001AB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string eventClassName,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ withinInterval,
                               string condition) cil managed
  // SIG: 20 03 01 0E 11 2D 0E
  {
    // Method begins at RVA 0x2245
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.ctor

  .method /*060001AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string eventClassName,
                               string condition,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ groupWithinInterval) cil managed
  // SIG: 20 03 01 0E 0E 11 2D
  {
    // Method begins at RVA 0x2246
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.ctor

  .method /*060001AD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string eventClassName,
                               string condition,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ groupWithinInterval,
                               string[] groupByPropertyList) cil managed
  // SIG: 20 04 01 0E 0E 11 2D 1D 0E
  {
    // Method begins at RVA 0x2247
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.ctor

  .method /*060001AE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string eventClassName,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ withinInterval,
                               string condition,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ groupWithinInterval,
                               string[] groupByPropertyList,
                               string havingCondition) cil managed
  // SIG: 20 06 01 0E 11 2D 0E 11 2D 1D 0E 0E
  {
    // Method begins at RVA 0x2248
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.ctor

  .method /*060001AF*/ public hidebysig specialname virtual 
          instance string  get_QueryLanguage() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2249
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::get_QueryLanguage

  .method /*060001B0*/ public hidebysig specialname virtual 
          instance string  get_QueryString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x224a
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::get_QueryString

  .method /*060001B1*/ public hidebysig specialname virtual 
          instance void  set_QueryString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x224b
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::set_QueryString

  .method /*060001B2*/ public hidebysig specialname 
          instance string  get_EventClassName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x224c
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::get_EventClassName

  .method /*060001B3*/ public hidebysig specialname 
          instance void  set_EventClassName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x224d
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::set_EventClassName

  .method /*060001B4*/ public hidebysig specialname 
          instance string  get_Condition() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x224e
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::get_Condition

  .method /*060001B5*/ public hidebysig specialname 
          instance void  set_Condition(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x224f
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::set_Condition

  .method /*060001B6*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ 
          get_WithinInterval() cil managed
  // SIG: 20 00 11 2D
  {
    // Method begins at RVA 0x2250
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::get_WithinInterval

  .method /*060001B7*/ public hidebysig specialname 
          instance void  set_WithinInterval(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 11 2D
  {
    // Method begins at RVA 0x2251
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::set_WithinInterval

  .method /*060001B8*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ 
          get_GroupWithinInterval() cil managed
  // SIG: 20 00 11 2D
  {
    // Method begins at RVA 0x2252
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::get_GroupWithinInterval

  .method /*060001B9*/ public hidebysig specialname 
          instance void  set_GroupWithinInterval(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 11 2D
  {
    // Method begins at RVA 0x2253
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::set_GroupWithinInterval

  .method /*060001BA*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/ 
          get_GroupByPropertyList() cil managed
  // SIG: 20 00 12 21
  {
    // Method begins at RVA 0x2254
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::get_GroupByPropertyList

  .method /*060001BB*/ public hidebysig specialname 
          instance void  set_GroupByPropertyList(class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/ 'value') cil managed
  // SIG: 20 01 01 12 21
  {
    // Method begins at RVA 0x2255
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::set_GroupByPropertyList

  .method /*060001BC*/ public hidebysig specialname 
          instance string  get_HavingCondition() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2256
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::get_HavingCondition

  .method /*060001BD*/ public hidebysig specialname 
          instance void  set_HavingCondition(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2257
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::set_HavingCondition

  .method /*060001BE*/ famorassem hidebysig 
          instance void  BuildQuery() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2258
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::BuildQuery

  .method /*060001BF*/ famorassem hidebysig virtual 
          instance void  ParseQuery(string query) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2259
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::ParseQuery

  .method /*060001C0*/ public hidebysig virtual 
          instance object  Clone() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x225a
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::Clone

  .method /*060001C1*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x225b
    // Code size       0 (0x0)
  } // end of method WqlEventQuery::.cctor

  .property /*1700005B*/ instance callconv(8) string
          QueryLanguage()
  {
    .get instance string System.Management.WqlEventQuery/*02000032*/::get_QueryLanguage() /* 060001AF */
  } // end of property WqlEventQuery::QueryLanguage
  .property /*1700005C*/ instance callconv(8) string
          QueryString()
  {
    .set instance void System.Management.WqlEventQuery/*02000032*/::set_QueryString(string) /* 060001B1 */
    .get instance string System.Management.WqlEventQuery/*02000032*/::get_QueryString() /* 060001B0 */
  } // end of property WqlEventQuery::QueryString
  .property /*1700005D*/ instance callconv(8) string
          EventClassName()
  {
    .get instance string System.Management.WqlEventQuery/*02000032*/::get_EventClassName() /* 060001B2 */
    .set instance void System.Management.WqlEventQuery/*02000032*/::set_EventClassName(string) /* 060001B3 */
  } // end of property WqlEventQuery::EventClassName
  .property /*1700005E*/ instance callconv(8) string
          Condition()
  {
    .get instance string System.Management.WqlEventQuery/*02000032*/::get_Condition() /* 060001B4 */
    .set instance void System.Management.WqlEventQuery/*02000032*/::set_Condition(string) /* 060001B5 */
  } // end of property WqlEventQuery::Condition
  .property /*1700005F*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/
          WithinInterval()
  {
    .set instance void System.Management.WqlEventQuery/*02000032*/::set_WithinInterval(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/) /* 060001B7 */
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ System.Management.WqlEventQuery/*02000032*/::get_WithinInterval() /* 060001B6 */
  } // end of property WqlEventQuery::WithinInterval
  .property /*17000060*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/
          GroupWithinInterval()
  {
    .set instance void System.Management.WqlEventQuery/*02000032*/::set_GroupWithinInterval(valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/) /* 060001B9 */
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*0100000B*/ System.Management.WqlEventQuery/*02000032*/::get_GroupWithinInterval() /* 060001B8 */
  } // end of property WqlEventQuery::GroupWithinInterval
  .property /*17000061*/ instance callconv(8) class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/
          GroupByPropertyList()
  {
    .set instance void System.Management.WqlEventQuery/*02000032*/::set_GroupByPropertyList(class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/) /* 060001BB */
    .get instance class [System/*23000002*/]System.Collections.Specialized.StringCollection/*01000008*/ System.Management.WqlEventQuery/*02000032*/::get_GroupByPropertyList() /* 060001BA */
  } // end of property WqlEventQuery::GroupByPropertyList
  .property /*17000062*/ instance callconv(8) string
          HavingCondition()
  {
    .get instance string System.Management.WqlEventQuery/*02000032*/::get_HavingCondition() /* 060001BC */
    .set instance void System.Management.WqlEventQuery/*02000032*/::set_HavingCondition(string) /* 060001BD */
  } // end of property WqlEventQuery::HavingCondition
} // end of class System.Management.WqlEventQuery

.class /*02000033*/ private auto ansi beforefieldinit System.Management.ManagementQueryConverter
       extends [System/*23000002*/]System.ComponentModel.ExpandableObjectConverter/*01000019*/
{
  .method /*060001C2*/ public hidebysig virtual 
          instance bool  CanConvertFrom(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                        class [mscorlib/*23000001*/]System.Type/*0100001B*/ sourceType) cil managed
  // SIG: 20 02 02 12 69 12 6D
  {
    // Method begins at RVA 0x225c
    // Code size       0 (0x0)
  } // end of method ManagementQueryConverter::CanConvertFrom

  .method /*060001C3*/ public hidebysig virtual 
          instance bool  CanConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                      class [mscorlib/*23000001*/]System.Type/*0100001B*/ destinationType) cil managed
  // SIG: 20 02 02 12 69 12 6D
  {
    // Method begins at RVA 0x225d
    // Code size       0 (0x0)
  } // end of method ManagementQueryConverter::CanConvertTo

  .method /*060001C4*/ public hidebysig virtual 
          instance object  ConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100001C*/ culture,
                                     object 'value',
                                     class [mscorlib/*23000001*/]System.Type/*0100001B*/ destinationType) cil managed
  // SIG: 20 04 1C 12 69 12 71 1C 12 6D
  {
    // Method begins at RVA 0x225e
    // Code size       0 (0x0)
  } // end of method ManagementQueryConverter::ConvertTo

  .method /*060001C5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225f
    // Code size       0 (0x0)
  } // end of method ManagementQueryConverter::.ctor

} // end of class System.Management.ManagementQueryConverter

.class /*02000034*/ public auto ansi beforefieldinit System.Management.ManagementScope
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements [mscorlib/*23000001*/]System.ICloneable/*01000006*/
{
  .custom /*0C00001E:0A00001C*/ instance void [System/*23000002*/]System.ComponentModel.TypeConverterAttribute/*01000039*/::.ctor(class [mscorlib/*23000001*/]System.Type/*0100001B*/) /* 0A00001C */ = ( 01 00 2A 53 79 73 74 65 6D 2E 4D 61 6E 61 67 65   // ..*System.Manage
                                                                                                                                                                                                          6D 65 6E 74 2E 4D 61 6E 61 67 65 6D 65 6E 74 53   // ment.ManagementS
                                                                                                                                                                                                          63 6F 70 65 43 6F 6E 76 65 72 74 65 72 00 00 )    // copeConverter..
  .method /*060001C6*/ public hidebysig specialname 
          instance bool  get_IsConnected() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2260
    // Code size       0 (0x0)
  } // end of method ManagementScope::get_IsConnected

  .method /*060001C7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2261
    // Code size       0 (0x0)
  } // end of method ManagementScope::.ctor

  .method /*060001C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementPath/*02000029*/ path) cil managed
  // SIG: 20 01 01 12 80 A4
  {
    // Method begins at RVA 0x2262
    // Code size       0 (0x0)
  } // end of method ManagementScope::.ctor

  .method /*060001C9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string path) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2263
    // Code size       0 (0x0)
  } // end of method ManagementScope::.ctor

  .method /*060001CA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string path,
                               class System.Management.ConnectionOptions/*02000028*/ options) cil managed
  // SIG: 20 02 01 0E 12 80 A0
  {
    // Method begins at RVA 0x2264
    // Code size       0 (0x0)
  } // end of method ManagementScope::.ctor

  .method /*060001CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Management.ManagementPath/*02000029*/ path,
                               class System.Management.ConnectionOptions/*02000028*/ options) cil managed
  // SIG: 20 02 01 12 80 A4 12 80 A0
  {
    // Method begins at RVA 0x2265
    // Code size       0 (0x0)
  } // end of method ManagementScope::.ctor

  .method /*060001CC*/ public hidebysig specialname 
          instance class System.Management.ConnectionOptions/*02000028*/ 
          get_Options() cil managed
  // SIG: 20 00 12 80 A0
  {
    // Method begins at RVA 0x2266
    // Code size       0 (0x0)
  } // end of method ManagementScope::get_Options

  .method /*060001CD*/ public hidebysig specialname 
          instance void  set_Options(class System.Management.ConnectionOptions/*02000028*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A0
  {
    // Method begins at RVA 0x2267
    // Code size       0 (0x0)
  } // end of method ManagementScope::set_Options

  .method /*060001CE*/ public hidebysig specialname 
          instance class System.Management.ManagementPath/*02000029*/ 
          get_Path() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x2268
    // Code size       0 (0x0)
  } // end of method ManagementScope::get_Path

  .method /*060001CF*/ public hidebysig specialname 
          instance void  set_Path(class System.Management.ManagementPath/*02000029*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A4
  {
    // Method begins at RVA 0x2269
    // Code size       0 (0x0)
  } // end of method ManagementScope::set_Path

  .method /*060001D0*/ public hidebysig instance class System.Management.ManagementScope/*02000034*/ 
          Clone() cil managed
  // SIG: 20 00 12 80 D0
  {
    // Method begins at RVA 0x226a
    // Code size       0 (0x0)
  } // end of method ManagementScope::Clone

  .method /*060001D1*/ private hidebysig newslot virtual final 
          instance object  System.ICloneable.Clone() cil managed
  // SIG: 20 00 1C
  {
    .override [mscorlib/*23000001*/]System.ICloneable/*01000006*/::Clone /*01000006::0A000004*/ 
    // Method begins at RVA 0x226b
    // Code size       0 (0x0)
  } // end of method ManagementScope::System.ICloneable.Clone

  .method /*060001D2*/ public hidebysig instance void 
          Connect() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x226c
    // Code size       0 (0x0)
  } // end of method ManagementScope::Connect

  .property /*17000063*/ instance callconv(8) bool
          IsConnected()
  {
    .get instance bool System.Management.ManagementScope/*02000034*/::get_IsConnected() /* 060001C6 */
  } // end of property ManagementScope::IsConnected
  .property /*17000064*/ instance callconv(8) class System.Management.ConnectionOptions/*02000028*/
          Options()
  {
    .get instance class System.Management.ConnectionOptions/*02000028*/ System.Management.ManagementScope/*02000034*/::get_Options() /* 060001CC */
    .set instance void System.Management.ManagementScope/*02000034*/::set_Options(class System.Management.ConnectionOptions/*02000028*/) /* 060001CD */
  } // end of property ManagementScope::Options
  .property /*17000065*/ instance callconv(8) class System.Management.ManagementPath/*02000029*/
          Path()
  {
    .get instance class System.Management.ManagementPath/*02000029*/ System.Management.ManagementScope/*02000034*/::get_Path() /* 060001CE */
    .set instance void System.Management.ManagementScope/*02000034*/::set_Path(class System.Management.ManagementPath/*02000029*/) /* 060001CF */
  } // end of property ManagementScope::Path
} // end of class System.Management.ManagementScope

.class /*02000035*/ private auto ansi beforefieldinit System.Management.ManagementScopeConverter
       extends [System/*23000002*/]System.ComponentModel.ExpandableObjectConverter/*01000019*/
{
  .method /*060001D3*/ public hidebysig virtual 
          instance bool  CanConvertFrom(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                        class [mscorlib/*23000001*/]System.Type/*0100001B*/ sourceType) cil managed
  // SIG: 20 02 02 12 69 12 6D
  {
    // Method begins at RVA 0x226d
    // Code size       0 (0x0)
  } // end of method ManagementScopeConverter::CanConvertFrom

  .method /*060001D4*/ public hidebysig virtual 
          instance bool  CanConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                      class [mscorlib/*23000001*/]System.Type/*0100001B*/ destinationType) cil managed
  // SIG: 20 02 02 12 69 12 6D
  {
    // Method begins at RVA 0x226e
    // Code size       0 (0x0)
  } // end of method ManagementScopeConverter::CanConvertTo

  .method /*060001D5*/ public hidebysig virtual 
          instance object  ConvertTo(class [System/*23000002*/]System.ComponentModel.ITypeDescriptorContext/*0100001A*/ context,
                                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100001C*/ culture,
                                     object 'value',
                                     class [mscorlib/*23000001*/]System.Type/*0100001B*/ destinationType) cil managed
  // SIG: 20 04 1C 12 69 12 71 1C 12 6D
  {
    // Method begins at RVA 0x226f
    // Code size       0 (0x0)
  } // end of method ManagementScopeConverter::ConvertTo

  .method /*060001D6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2270
    // Code size       0 (0x0)
  } // end of method ManagementScopeConverter::.ctor

} // end of class System.Management.ManagementScopeConverter

.class /*02000036*/ public auto ansi beforefieldinit System.Management.PropertyData
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
{
  .method /*060001D7*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2271
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Name

  .method /*060001D8*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2272
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Value

  .method /*060001D9*/ public hidebysig specialname 
          instance void  set_Value(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2273
    // Code size       0 (0x0)
  } // end of method PropertyData::set_Value

  .method /*060001DA*/ public hidebysig specialname 
          instance valuetype System.Management.CimType/*02000003*/ 
          get_Type() cil managed
  // SIG: 20 00 11 0C
  {
    // Method begins at RVA 0x2274
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Type

  .method /*060001DB*/ public hidebysig specialname 
          instance bool  get_IsLocal() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2275
    // Code size       0 (0x0)
  } // end of method PropertyData::get_IsLocal

  .method /*060001DC*/ public hidebysig specialname 
          instance bool  get_IsArray() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2276
    // Code size       0 (0x0)
  } // end of method PropertyData::get_IsArray

  .method /*060001DD*/ public hidebysig specialname 
          instance string  get_Origin() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2277
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Origin

  .method /*060001DE*/ public hidebysig specialname 
          instance class System.Management.QualifierDataCollection/*0200003A*/ 
          get_Qualifiers() cil managed
  // SIG: 20 00 12 80 E8
  {
    // Method begins at RVA 0x2278
    // Code size       0 (0x0)
  } // end of method PropertyData::get_Qualifiers

  .method /*060001DF*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2279
    // Code size       0 (0x0)
  } // end of method PropertyData::.ctor

  .property /*17000066*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Management.PropertyData/*02000036*/::get_Name() /* 060001D7 */
  } // end of property PropertyData::Name
  .property /*17000067*/ instance callconv(8) object
          Value()
  {
    .set instance void System.Management.PropertyData/*02000036*/::set_Value(object) /* 060001D9 */
    .get instance object System.Management.PropertyData/*02000036*/::get_Value() /* 060001D8 */
  } // end of property PropertyData::Value
  .property /*17000068*/ instance callconv(8) valuetype System.Management.CimType/*02000003*/
          Type()
  {
    .get instance valuetype System.Management.CimType/*02000003*/ System.Management.PropertyData/*02000036*/::get_Type() /* 060001DA */
  } // end of property PropertyData::Type
  .property /*17000069*/ instance callconv(8) bool
          IsLocal()
  {
    .get instance bool System.Management.PropertyData/*02000036*/::get_IsLocal() /* 060001DB */
  } // end of property PropertyData::IsLocal
  .property /*1700006A*/ instance callconv(8) bool
          IsArray()
  {
    .get instance bool System.Management.PropertyData/*02000036*/::get_IsArray() /* 060001DC */
  } // end of property PropertyData::IsArray
  .property /*1700006B*/ instance callconv(8) string
          Origin()
  {
    .get instance string System.Management.PropertyData/*02000036*/::get_Origin() /* 060001DD */
  } // end of property PropertyData::Origin
  .property /*1700006C*/ instance callconv(8) class System.Management.QualifierDataCollection/*0200003A*/
          Qualifiers()
  {
    .get instance class System.Management.QualifierDataCollection/*0200003A*/ System.Management.PropertyData/*02000036*/::get_Qualifiers() /* 060001DE */
  } // end of property PropertyData::Qualifiers
} // end of class System.Management.PropertyData

.class /*02000037*/ public auto ansi beforefieldinit System.Management.PropertyDataCollection
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements [mscorlib/*23000001*/]System.Collections.ICollection/*01000015*/,
                  [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000013*/
{
  .custom /*0C00001F:0A00001A*/ instance void [mscorlib/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000037*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .class /*02000038*/ auto ansi nested public beforefieldinit PropertyDataEnumerator
         extends [mscorlib/*23000001*/]System.Object/*01000007*/
         implements [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/
  {
    .method /*060001ED*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/::get_Current /*01000014::0A000003*/ 
      // Method begins at RVA 0x2287
      // Code size       0 (0x0)
    } // end of method PropertyDataEnumerator::System.Collections.IEnumerator.get_Current

    .method /*060001EE*/ public hidebysig specialname 
            instance class System.Management.PropertyData/*02000036*/ 
            get_Current() cil managed
    // SIG: 20 00 12 80 D8
    {
      // Method begins at RVA 0x2288
      // Code size       0 (0x0)
    } // end of method PropertyDataEnumerator::get_Current

    .method /*060001EF*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x2289
      // Code size       0 (0x0)
    } // end of method PropertyDataEnumerator::MoveNext

    .method /*060001F0*/ public hidebysig newslot virtual final 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x228a
      // Code size       0 (0x0)
    } // end of method PropertyDataEnumerator::Reset

    .method /*060001F1*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x228b
      // Code size       0 (0x0)
    } // end of method PropertyDataEnumerator::.ctor

    .property /*17000071*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object System.Management.PropertyDataCollection/*02000037*//PropertyDataEnumerator/*02000038*/::System.Collections.IEnumerator.get_Current() /* 060001ED */
    } // end of property PropertyDataEnumerator::System.Collections.IEnumerator.Current
    .property /*17000072*/ instance callconv(8) class System.Management.PropertyData/*02000036*/
            Current()
    {
      .get instance class System.Management.PropertyData/*02000036*/ System.Management.PropertyDataCollection/*02000037*//PropertyDataEnumerator/*02000038*/::get_Current() /* 060001EE */
    } // end of property PropertyDataEnumerator::Current
  } // end of class PropertyDataEnumerator

  .method /*060001E0*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x227a
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::get_Count

  .method /*060001E1*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsSynchronized() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x227b
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::get_IsSynchronized

  .method /*060001E2*/ public hidebysig newslot specialname virtual final 
          instance object  get_SyncRoot() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x227c
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::get_SyncRoot

  .method /*060001E3*/ public hidebysig newslot virtual final 
          instance void  CopyTo(class [mscorlib/*23000001*/]System.Array/*01000017*/ 'array',
                                int32 index) cil managed
  // SIG: 20 02 01 12 5D 08
  {
    // Method begins at RVA 0x227d
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::CopyTo

  .method /*060001E4*/ public hidebysig instance void 
          CopyTo(class System.Management.PropertyData/*02000036*/[] propertyArray,
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 80 D8 08
  {
    // Method begins at RVA 0x227e
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::CopyTo

  .method /*060001E5*/ private hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 51
  {
    .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000013*/::GetEnumerator /*01000013::0A000002*/ 
    // Method begins at RVA 0x227f
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::System.Collections.IEnumerable.GetEnumerator

  .method /*060001E6*/ public hidebysig instance class System.Management.PropertyDataCollection/*02000037*//PropertyDataEnumerator/*02000038*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 12 80 E0
  {
    // Method begins at RVA 0x2280
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::GetEnumerator

  .method /*060001E7*/ public hidebysig newslot specialname virtual 
          instance class System.Management.PropertyData/*02000036*/ 
          get_Item(string propertyName) cil managed
  // SIG: 20 01 12 80 D8 0E
  {
    // Method begins at RVA 0x2281
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::get_Item

  .method /*060001E8*/ public hidebysig newslot virtual 
          instance void  Remove(string propertyName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2282
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::Remove

  .method /*060001E9*/ public hidebysig newslot virtual 
          instance void  Add(string propertyName,
                             object propertyValue) cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x2283
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::Add

  .method /*060001EA*/ public hidebysig instance void 
          Add(string propertyName,
              object propertyValue,
              valuetype System.Management.CimType/*02000003*/ propertyType) cil managed
  // SIG: 20 03 01 0E 1C 11 0C
  {
    // Method begins at RVA 0x2284
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::Add

  .method /*060001EB*/ public hidebysig instance void 
          Add(string propertyName,
              valuetype System.Management.CimType/*02000003*/ propertyType,
              bool isArray) cil managed
  // SIG: 20 03 01 0E 11 0C 02
  {
    // Method begins at RVA 0x2285
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::Add

  .method /*060001EC*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2286
    // Code size       0 (0x0)
  } // end of method PropertyDataCollection::.ctor

  .property /*1700006D*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Management.PropertyDataCollection/*02000037*/::get_Count() /* 060001E0 */
  } // end of property PropertyDataCollection::Count
  .property /*1700006E*/ instance callconv(8) bool
          IsSynchronized()
  {
    .get instance bool System.Management.PropertyDataCollection/*02000037*/::get_IsSynchronized() /* 060001E1 */
  } // end of property PropertyDataCollection::IsSynchronized
  .property /*1700006F*/ instance callconv(8) object
          SyncRoot()
  {
    .get instance object System.Management.PropertyDataCollection/*02000037*/::get_SyncRoot() /* 060001E2 */
  } // end of property PropertyDataCollection::SyncRoot
  .property /*17000070*/ instance callconv(8) class System.Management.PropertyData/*02000036*/
          Item(string)
  {
    .get instance class System.Management.PropertyData/*02000036*/ System.Management.PropertyDataCollection/*02000037*/::get_Item(string) /* 060001E7 */
  } // end of property PropertyDataCollection::Item
} // end of class System.Management.PropertyDataCollection

.class /*02000039*/ public auto ansi beforefieldinit System.Management.QualifierData
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
{
  .method /*060001F2*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x228c
    // Code size       0 (0x0)
  } // end of method QualifierData::get_Name

  .method /*060001F3*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x228d
    // Code size       0 (0x0)
  } // end of method QualifierData::get_Value

  .method /*060001F4*/ public hidebysig specialname 
          instance void  set_Value(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x228e
    // Code size       0 (0x0)
  } // end of method QualifierData::set_Value

  .method /*060001F5*/ public hidebysig specialname 
          instance bool  get_IsAmended() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x228f
    // Code size       0 (0x0)
  } // end of method QualifierData::get_IsAmended

  .method /*060001F6*/ public hidebysig specialname 
          instance void  set_IsAmended(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2290
    // Code size       0 (0x0)
  } // end of method QualifierData::set_IsAmended

  .method /*060001F7*/ public hidebysig specialname 
          instance bool  get_IsLocal() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2291
    // Code size       0 (0x0)
  } // end of method QualifierData::get_IsLocal

  .method /*060001F8*/ public hidebysig specialname 
          instance bool  get_PropagatesToInstance() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2292
    // Code size       0 (0x0)
  } // end of method QualifierData::get_PropagatesToInstance

  .method /*060001F9*/ public hidebysig specialname 
          instance void  set_PropagatesToInstance(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2293
    // Code size       0 (0x0)
  } // end of method QualifierData::set_PropagatesToInstance

  .method /*060001FA*/ public hidebysig specialname 
          instance bool  get_PropagatesToSubclass() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2294
    // Code size       0 (0x0)
  } // end of method QualifierData::get_PropagatesToSubclass

  .method /*060001FB*/ public hidebysig specialname 
          instance void  set_PropagatesToSubclass(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2295
    // Code size       0 (0x0)
  } // end of method QualifierData::set_PropagatesToSubclass

  .method /*060001FC*/ public hidebysig specialname 
          instance bool  get_IsOverridable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2296
    // Code size       0 (0x0)
  } // end of method QualifierData::get_IsOverridable

  .method /*060001FD*/ public hidebysig specialname 
          instance void  set_IsOverridable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2297
    // Code size       0 (0x0)
  } // end of method QualifierData::set_IsOverridable

  .method /*060001FE*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2298
    // Code size       0 (0x0)
  } // end of method QualifierData::.ctor

  .property /*17000073*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Management.QualifierData/*02000039*/::get_Name() /* 060001F2 */
  } // end of property QualifierData::Name
  .property /*17000074*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Management.QualifierData/*02000039*/::get_Value() /* 060001F3 */
    .set instance void System.Management.QualifierData/*02000039*/::set_Value(object) /* 060001F4 */
  } // end of property QualifierData::Value
  .property /*17000075*/ instance callconv(8) bool
          IsAmended()
  {
    .get instance bool System.Management.QualifierData/*02000039*/::get_IsAmended() /* 060001F5 */
    .set instance void System.Management.QualifierData/*02000039*/::set_IsAmended(bool) /* 060001F6 */
  } // end of property QualifierData::IsAmended
  .property /*17000076*/ instance callconv(8) bool
          IsLocal()
  {
    .get instance bool System.Management.QualifierData/*02000039*/::get_IsLocal() /* 060001F7 */
  } // end of property QualifierData::IsLocal
  .property /*17000077*/ instance callconv(8) bool
          PropagatesToInstance()
  {
    .set instance void System.Management.QualifierData/*02000039*/::set_PropagatesToInstance(bool) /* 060001F9 */
    .get instance bool System.Management.QualifierData/*02000039*/::get_PropagatesToInstance() /* 060001F8 */
  } // end of property QualifierData::PropagatesToInstance
  .property /*17000078*/ instance callconv(8) bool
          PropagatesToSubclass()
  {
    .set instance void System.Management.QualifierData/*02000039*/::set_PropagatesToSubclass(bool) /* 060001FB */
    .get instance bool System.Management.QualifierData/*02000039*/::get_PropagatesToSubclass() /* 060001FA */
  } // end of property QualifierData::PropagatesToSubclass
  .property /*17000079*/ instance callconv(8) bool
          IsOverridable()
  {
    .set instance void System.Management.QualifierData/*02000039*/::set_IsOverridable(bool) /* 060001FD */
    .get instance bool System.Management.QualifierData/*02000039*/::get_IsOverridable() /* 060001FC */
  } // end of property QualifierData::IsOverridable
} // end of class System.Management.QualifierData

.class /*0200003A*/ public auto ansi beforefieldinit System.Management.QualifierDataCollection
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements [mscorlib/*23000001*/]System.Collections.ICollection/*01000015*/,
                  [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000013*/
{
  .custom /*0C000020:0A00001A*/ instance void [mscorlib/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000037*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .class /*0200003B*/ auto ansi nested public beforefieldinit QualifierDataEnumerator
         extends [mscorlib/*23000001*/]System.Object/*01000007*/
         implements [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/
  {
    .method /*0600020B*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/::get_Current /*01000014::0A000003*/ 
      // Method begins at RVA 0x22a5
      // Code size       0 (0x0)
    } // end of method QualifierDataEnumerator::System.Collections.IEnumerator.get_Current

    .method /*0600020C*/ public hidebysig specialname 
            instance class System.Management.QualifierData/*02000039*/ 
            get_Current() cil managed
    // SIG: 20 00 12 80 E4
    {
      // Method begins at RVA 0x22a6
      // Code size       0 (0x0)
    } // end of method QualifierDataEnumerator::get_Current

    .method /*0600020D*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x22a7
      // Code size       0 (0x0)
    } // end of method QualifierDataEnumerator::MoveNext

    .method /*0600020E*/ public hidebysig newslot virtual final 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x22a8
      // Code size       0 (0x0)
    } // end of method QualifierDataEnumerator::Reset

    .method /*0600020F*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x22a9
      // Code size       0 (0x0)
    } // end of method QualifierDataEnumerator::.ctor

    .property /*1700007E*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object System.Management.QualifierDataCollection/*0200003A*//QualifierDataEnumerator/*0200003B*/::System.Collections.IEnumerator.get_Current() /* 0600020B */
    } // end of property QualifierDataEnumerator::System.Collections.IEnumerator.Current
    .property /*1700007F*/ instance callconv(8) class System.Management.QualifierData/*02000039*/
            Current()
    {
      .get instance class System.Management.QualifierData/*02000039*/ System.Management.QualifierDataCollection/*0200003A*//QualifierDataEnumerator/*0200003B*/::get_Current() /* 0600020C */
    } // end of property QualifierDataEnumerator::Current
  } // end of class QualifierDataEnumerator

  .method /*060001FF*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2299
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::get_Count

  .method /*06000200*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsSynchronized() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x229a
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::get_IsSynchronized

  .method /*06000201*/ public hidebysig newslot specialname virtual final 
          instance object  get_SyncRoot() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x229b
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::get_SyncRoot

  .method /*06000202*/ public hidebysig newslot virtual final 
          instance void  CopyTo(class [mscorlib/*23000001*/]System.Array/*01000017*/ 'array',
                                int32 index) cil managed
  // SIG: 20 02 01 12 5D 08
  {
    // Method begins at RVA 0x229c
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::CopyTo

  .method /*06000203*/ public hidebysig instance void 
          CopyTo(class System.Management.QualifierData/*02000039*/[] qualifierArray,
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 80 E4 08
  {
    // Method begins at RVA 0x229d
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::CopyTo

  .method /*06000204*/ private hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 51
  {
    .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000013*/::GetEnumerator /*01000013::0A000002*/ 
    // Method begins at RVA 0x229e
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::System.Collections.IEnumerable.GetEnumerator

  .method /*06000205*/ public hidebysig instance class System.Management.QualifierDataCollection/*0200003A*//QualifierDataEnumerator/*0200003B*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 12 80 EC
  {
    // Method begins at RVA 0x229f
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::GetEnumerator

  .method /*06000206*/ public hidebysig newslot specialname virtual 
          instance class System.Management.QualifierData/*02000039*/ 
          get_Item(string qualifierName) cil managed
  // SIG: 20 01 12 80 E4 0E
  {
    // Method begins at RVA 0x22a0
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::get_Item

  .method /*06000207*/ public hidebysig newslot virtual 
          instance void  Remove(string qualifierName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22a1
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::Remove

  .method /*06000208*/ public hidebysig newslot virtual 
          instance void  Add(string qualifierName,
                             object qualifierValue) cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x22a2
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::Add

  .method /*06000209*/ public hidebysig newslot virtual 
          instance void  Add(string qualifierName,
                             object qualifierValue,
                             bool isAmended,
                             bool propagatesToInstance,
                             bool propagatesToSubclass,
                             bool isOverridable) cil managed
  // SIG: 20 06 01 0E 1C 02 02 02 02
  {
    // Method begins at RVA 0x22a3
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::Add

  .method /*0600020A*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22a4
    // Code size       0 (0x0)
  } // end of method QualifierDataCollection::.ctor

  .property /*1700007A*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Management.QualifierDataCollection/*0200003A*/::get_Count() /* 060001FF */
  } // end of property QualifierDataCollection::Count
  .property /*1700007B*/ instance callconv(8) bool
          IsSynchronized()
  {
    .get instance bool System.Management.QualifierDataCollection/*0200003A*/::get_IsSynchronized() /* 06000200 */
  } // end of property QualifierDataCollection::IsSynchronized
  .property /*1700007C*/ instance callconv(8) object
          SyncRoot()
  {
    .get instance object System.Management.QualifierDataCollection/*0200003A*/::get_SyncRoot() /* 06000201 */
  } // end of property QualifierDataCollection::SyncRoot
  .property /*1700007D*/ instance callconv(8) class System.Management.QualifierData/*02000039*/
          Item(string)
  {
    .get instance class System.Management.QualifierData/*02000039*/ System.Management.QualifierDataCollection/*0200003A*/::get_Item(string) /* 06000206 */
  } // end of property QualifierDataCollection::Item
} // end of class System.Management.QualifierDataCollection

.class /*0200003C*/ public auto ansi beforefieldinit System.Management.MethodData
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
{
  .method /*06000210*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22aa
    // Code size       0 (0x0)
  } // end of method MethodData::get_Name

  .method /*06000211*/ public hidebysig specialname 
          instance class System.Management.ManagementBaseObject/*02000005*/ 
          get_InParameters() cil managed
  // SIG: 20 00 12 14
  {
    // Method begins at RVA 0x22ab
    // Code size       0 (0x0)
  } // end of method MethodData::get_InParameters

  .method /*06000212*/ public hidebysig specialname 
          instance class System.Management.ManagementBaseObject/*02000005*/ 
          get_OutParameters() cil managed
  // SIG: 20 00 12 14
  {
    // Method begins at RVA 0x22ac
    // Code size       0 (0x0)
  } // end of method MethodData::get_OutParameters

  .method /*06000213*/ public hidebysig specialname 
          instance string  get_Origin() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22ad
    // Code size       0 (0x0)
  } // end of method MethodData::get_Origin

  .method /*06000214*/ public hidebysig specialname 
          instance class System.Management.QualifierDataCollection/*0200003A*/ 
          get_Qualifiers() cil managed
  // SIG: 20 00 12 80 E8
  {
    // Method begins at RVA 0x22ae
    // Code size       0 (0x0)
  } // end of method MethodData::get_Qualifiers

  .method /*06000215*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22af
    // Code size       0 (0x0)
  } // end of method MethodData::.ctor

  .property /*17000080*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Management.MethodData/*0200003C*/::get_Name() /* 06000210 */
  } // end of property MethodData::Name
  .property /*17000081*/ instance callconv(8) class System.Management.ManagementBaseObject/*02000005*/
          InParameters()
  {
    .get instance class System.Management.ManagementBaseObject/*02000005*/ System.Management.MethodData/*0200003C*/::get_InParameters() /* 06000211 */
  } // end of property MethodData::InParameters
  .property /*17000082*/ instance callconv(8) class System.Management.ManagementBaseObject/*02000005*/
          OutParameters()
  {
    .get instance class System.Management.ManagementBaseObject/*02000005*/ System.Management.MethodData/*0200003C*/::get_OutParameters() /* 06000212 */
  } // end of property MethodData::OutParameters
  .property /*17000083*/ instance callconv(8) string
          Origin()
  {
    .get instance string System.Management.MethodData/*0200003C*/::get_Origin() /* 06000213 */
  } // end of property MethodData::Origin
  .property /*17000084*/ instance callconv(8) class System.Management.QualifierDataCollection/*0200003A*/
          Qualifiers()
  {
    .get instance class System.Management.QualifierDataCollection/*0200003A*/ System.Management.MethodData/*0200003C*/::get_Qualifiers() /* 06000214 */
  } // end of property MethodData::Qualifiers
} // end of class System.Management.MethodData

.class /*0200003D*/ public auto ansi beforefieldinit System.Management.MethodDataCollection
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements [mscorlib/*23000001*/]System.Collections.ICollection/*01000015*/,
                  [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000013*/
{
  .custom /*0C000022:0A00001A*/ instance void [mscorlib/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000037*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .class /*0200003E*/ auto ansi nested public beforefieldinit MethodDataEnumerator
         extends [mscorlib/*23000001*/]System.Object/*01000007*/
         implements [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/
  {
    .method /*06000222*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/::get_Current /*01000014::0A000003*/ 
      // Method begins at RVA 0x22bc
      // Code size       0 (0x0)
    } // end of method MethodDataEnumerator::System.Collections.IEnumerator.get_Current

    .method /*06000223*/ public hidebysig specialname 
            instance class System.Management.MethodData/*0200003C*/ 
            get_Current() cil managed
    // SIG: 20 00 12 80 F0
    {
      // Method begins at RVA 0x22bd
      // Code size       0 (0x0)
    } // end of method MethodDataEnumerator::get_Current

    .method /*06000224*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x22be
      // Code size       0 (0x0)
    } // end of method MethodDataEnumerator::MoveNext

    .method /*06000225*/ public hidebysig newslot virtual final 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x22bf
      // Code size       0 (0x0)
    } // end of method MethodDataEnumerator::Reset

    .method /*06000226*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x22c0
      // Code size       0 (0x0)
    } // end of method MethodDataEnumerator::.ctor

    .property /*17000089*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object System.Management.MethodDataCollection/*0200003D*//MethodDataEnumerator/*0200003E*/::System.Collections.IEnumerator.get_Current() /* 06000222 */
    } // end of property MethodDataEnumerator::System.Collections.IEnumerator.Current
    .property /*1700008A*/ instance callconv(8) class System.Management.MethodData/*0200003C*/
            Current()
    {
      .get instance class System.Management.MethodData/*0200003C*/ System.Management.MethodDataCollection/*0200003D*//MethodDataEnumerator/*0200003E*/::get_Current() /* 06000223 */
    } // end of property MethodDataEnumerator::Current
  } // end of class MethodDataEnumerator

  .method /*06000216*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22b0
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::get_Count

  .method /*06000217*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsSynchronized() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22b1
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::get_IsSynchronized

  .method /*06000218*/ public hidebysig newslot specialname virtual final 
          instance object  get_SyncRoot() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x22b2
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::get_SyncRoot

  .method /*06000219*/ public hidebysig newslot virtual final 
          instance void  CopyTo(class [mscorlib/*23000001*/]System.Array/*01000017*/ 'array',
                                int32 index) cil managed
  // SIG: 20 02 01 12 5D 08
  {
    // Method begins at RVA 0x22b3
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::CopyTo

  .method /*0600021A*/ public hidebysig instance void 
          CopyTo(class System.Management.MethodData/*0200003C*/[] methodArray,
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 80 F0 08
  {
    // Method begins at RVA 0x22b4
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::CopyTo

  .method /*0600021B*/ private hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000014*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 51
  {
    .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000013*/::GetEnumerator /*01000013::0A000002*/ 
    // Method begins at RVA 0x22b5
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::System.Collections.IEnumerable.GetEnumerator

  .method /*0600021C*/ public hidebysig instance class System.Management.MethodDataCollection/*0200003D*//MethodDataEnumerator/*0200003E*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 12 80 F8
  {
    // Method begins at RVA 0x22b6
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::GetEnumerator

  .method /*0600021D*/ public hidebysig newslot specialname virtual 
          instance class System.Management.MethodData/*0200003C*/ 
          get_Item(string methodName) cil managed
  // SIG: 20 01 12 80 F0 0E
  {
    // Method begins at RVA 0x22b7
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::get_Item

  .method /*0600021E*/ public hidebysig newslot virtual 
          instance void  Remove(string methodName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22b8
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::Remove

  .method /*0600021F*/ public hidebysig newslot virtual 
          instance void  Add(string methodName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22b9
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::Add

  .method /*06000220*/ public hidebysig newslot virtual 
          instance void  Add(string methodName,
                             class System.Management.ManagementBaseObject/*02000005*/ inParameters,
                             class System.Management.ManagementBaseObject/*02000005*/ outParameters) cil managed
  // SIG: 20 03 01 0E 12 14 12 14
  {
    // Method begins at RVA 0x22ba
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::Add

  .method /*06000221*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22bb
    // Code size       0 (0x0)
  } // end of method MethodDataCollection::.ctor

  .property /*17000085*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Management.MethodDataCollection/*0200003D*/::get_Count() /* 06000216 */
  } // end of property MethodDataCollection::Count
  .property /*17000086*/ instance callconv(8) bool
          IsSynchronized()
  {
    .get instance bool System.Management.MethodDataCollection/*0200003D*/::get_IsSynchronized() /* 06000217 */
  } // end of property MethodDataCollection::IsSynchronized
  .property /*17000087*/ instance callconv(8) object
          SyncRoot()
  {
    .get instance object System.Management.MethodDataCollection/*0200003D*/::get_SyncRoot() /* 06000218 */
  } // end of property MethodDataCollection::SyncRoot
  .property /*17000088*/ instance callconv(8) class System.Management.MethodData/*0200003C*/
          Item(string)
  {
    .get instance class System.Management.MethodData/*0200003C*/ System.Management.MethodDataCollection/*0200003D*/::get_Item(string) /* 0600021D */
  } // end of property MethodDataCollection::Item
} // end of class System.Management.MethodDataCollection

.class /*0200003F*/ public auto ansi sealed System.Management.CodeLanguage
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .field /*040000A1*/ public specialname rtspecialname int32 value__
  .field /*040000A2*/ public static literal valuetype System.Management.CodeLanguage/*0200003F*/ CSharp = int32(0x00000000)
  .field /*040000A3*/ public static literal valuetype System.Management.CodeLanguage/*0200003F*/ JScript = int32(0x00000001)
  .field /*040000A4*/ public static literal valuetype System.Management.CodeLanguage/*0200003F*/ VB = int32(0x00000002)
  .field /*040000A5*/ public static literal valuetype System.Management.CodeLanguage/*0200003F*/ VJSharp = int32(0x00000003)
  .field /*040000A6*/ public static literal valuetype System.Management.CodeLanguage/*0200003F*/ Mcpp = int32(0x00000004)
} // end of class System.Management.CodeLanguage

.class /*02000040*/ public auto ansi beforefieldinit System.Management.Instrumentation.InstrumentedAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*0100001D*/
{
  .custom /*0C000026:0A00001E*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100003C*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100003D*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000227*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22c1
    // Code size       0 (0x0)
  } // end of method InstrumentedAttribute::.ctor

  .method /*06000228*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string namespaceName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22c2
    // Code size       0 (0x0)
  } // end of method InstrumentedAttribute::.ctor

  .method /*06000229*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string namespaceName,
                               string securityDescriptor) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x22c3
    // Code size       0 (0x0)
  } // end of method InstrumentedAttribute::.ctor

  .method /*0600022A*/ public hidebysig specialname 
          instance string  get_NamespaceName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22c4
    // Code size       0 (0x0)
  } // end of method InstrumentedAttribute::get_NamespaceName

  .method /*0600022B*/ public hidebysig specialname 
          instance string  get_SecurityDescriptor() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22c5
    // Code size       0 (0x0)
  } // end of method InstrumentedAttribute::get_SecurityDescriptor

  .property /*1700008B*/ instance callconv(8) string
          NamespaceName()
  {
    .get instance string System.Management.Instrumentation.InstrumentedAttribute/*02000040*/::get_NamespaceName() /* 0600022A */
  } // end of property InstrumentedAttribute::NamespaceName
  .property /*1700008C*/ instance callconv(8) string
          SecurityDescriptor()
  {
    .get instance string System.Management.Instrumentation.InstrumentedAttribute/*02000040*/::get_SecurityDescriptor() /* 0600022B */
  } // end of property InstrumentedAttribute::SecurityDescriptor
} // end of class System.Management.Instrumentation.InstrumentedAttribute

.class /*02000041*/ public auto ansi sealed System.Management.Instrumentation.InstrumentationType
       extends [mscorlib/*23000001*/]System.Enum/*01000001*/
{
  .field /*040000A7*/ public specialname rtspecialname int32 value__
  .field /*040000A8*/ public static literal valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/ Instance = int32(0x00000000)
  .field /*040000A9*/ public static literal valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/ Event = int32(0x00000001)
  .field /*040000AA*/ public static literal valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/ Abstract = int32(0x00000002)
} // end of class System.Management.Instrumentation.InstrumentationType

.class /*02000042*/ public auto ansi beforefieldinit System.Management.Instrumentation.InstrumentationClassAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*0100001D*/
{
  .custom /*0C000028:0A00001E*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100003C*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100003D*/) /* 0A00001E */ = ( 01 00 0C 00 00 00 00 00 ) 
  .method /*0600022C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/ instrumentationType) cil managed
  // SIG: 20 01 01 11 81 04
  {
    // Method begins at RVA 0x22c6
    // Code size       0 (0x0)
  } // end of method InstrumentationClassAttribute::.ctor

  .method /*0600022D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/ instrumentationType,
                               string managedBaseClassName) cil managed
  // SIG: 20 02 01 11 81 04 0E
  {
    // Method begins at RVA 0x22c7
    // Code size       0 (0x0)
  } // end of method InstrumentationClassAttribute::.ctor

  .method /*0600022E*/ public hidebysig specialname 
          instance valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/ 
          get_InstrumentationType() cil managed
  // SIG: 20 00 11 81 04
  {
    // Method begins at RVA 0x22c8
    // Code size       0 (0x0)
  } // end of method InstrumentationClassAttribute::get_InstrumentationType

  .method /*0600022F*/ public hidebysig specialname 
          instance string  get_ManagedBaseClassName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22c9
    // Code size       0 (0x0)
  } // end of method InstrumentationClassAttribute::get_ManagedBaseClassName

  .property /*1700008D*/ instance callconv(8) valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/
          InstrumentationType()
  {
    .get instance valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/ System.Management.Instrumentation.InstrumentationClassAttribute/*02000042*/::get_InstrumentationType() /* 0600022E */
  } // end of property InstrumentationClassAttribute::InstrumentationType
  .property /*1700008E*/ instance callconv(8) string
          ManagedBaseClassName()
  {
    .get instance string System.Management.Instrumentation.InstrumentationClassAttribute/*02000042*/::get_ManagedBaseClassName() /* 0600022F */
  } // end of property InstrumentationClassAttribute::ManagedBaseClassName
} // end of class System.Management.Instrumentation.InstrumentationClassAttribute

.class /*02000043*/ public auto ansi beforefieldinit System.Management.Instrumentation.ManagedNameAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*0100001D*/
{
  .custom /*0C000029:0A00001E*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100003C*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100003D*/) /* 0A00001E */ = ( 01 00 CC 01 00 00 00 00 ) 
  .method /*06000230*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22ca
    // Code size       0 (0x0)
  } // end of method ManagedNameAttribute::get_Name

  .method /*06000231*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22cb
    // Code size       0 (0x0)
  } // end of method ManagedNameAttribute::.ctor

  .property /*1700008F*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Management.Instrumentation.ManagedNameAttribute/*02000043*/::get_Name() /* 06000230 */
  } // end of property ManagedNameAttribute::Name
} // end of class System.Management.Instrumentation.ManagedNameAttribute

.class /*02000044*/ public auto ansi beforefieldinit System.Management.Instrumentation.IgnoreMemberAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*0100001D*/
{
  .custom /*0C00002A:0A00001E*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100003C*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100003D*/) /* 0A00001E */ = ( 01 00 C0 01 00 00 00 00 ) 
  .method /*06000232*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22cc
    // Code size       0 (0x0)
  } // end of method IgnoreMemberAttribute::.ctor

} // end of class System.Management.Instrumentation.IgnoreMemberAttribute

.class /*02000045*/ public auto ansi beforefieldinit System.Management.Instrumentation.Instrumentation
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
{
  .method /*06000233*/ public hidebysig static 
          void  RegisterAssembly(class [mscorlib/*23000001*/]System.Reflection.Assembly/*0100001E*/ assemblyToRegister) cil managed
  // SIG: 00 01 01 12 79
  {
    // Method begins at RVA 0x22cd
    // Code size       0 (0x0)
  } // end of method Instrumentation::RegisterAssembly

  .method /*06000234*/ public hidebysig static 
          bool  IsAssemblyRegistered(class [mscorlib/*23000001*/]System.Reflection.Assembly/*0100001E*/ assemblyToRegister) cil managed
  // SIG: 00 01 02 12 79
  {
    // Method begins at RVA 0x22ce
    // Code size       0 (0x0)
  } // end of method Instrumentation::IsAssemblyRegistered

  .method /*06000235*/ public hidebysig static 
          void  Fire(object eventData) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x22cf
    // Code size       0 (0x0)
  } // end of method Instrumentation::Fire

  .method /*06000236*/ public hidebysig static 
          void  Publish(object instanceData) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x22d0
    // Code size       0 (0x0)
  } // end of method Instrumentation::Publish

  .method /*06000237*/ public hidebysig static 
          void  Revoke(object instanceData) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x22d1
    // Code size       0 (0x0)
  } // end of method Instrumentation::Revoke

  .method /*06000238*/ public hidebysig static 
          void  SetBatchSize(class [mscorlib/*23000001*/]System.Type/*0100001B*/ instrumentationClass,
                             int32 batchSize) cil managed
  // SIG: 00 02 01 12 6D 08
  {
    // Method begins at RVA 0x22d2
    // Code size       0 (0x0)
  } // end of method Instrumentation::SetBatchSize

  .method /*06000239*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d3
    // Code size       0 (0x0)
  } // end of method Instrumentation::.ctor

  .method /*0600023A*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x22d4
    // Code size       0 (0x0)
  } // end of method Instrumentation::.cctor

} // end of class System.Management.Instrumentation.Instrumentation

.class /*02000046*/ interface public abstract auto ansi System.Management.Instrumentation.IEvent
{
  .method /*0600023B*/ public hidebysig newslot abstract virtual 
          instance void  Fire() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IEvent::Fire

} // end of class System.Management.Instrumentation.IEvent

.class /*02000047*/ public abstract auto ansi beforefieldinit System.Management.Instrumentation.BaseEvent
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements System.Management.Instrumentation.IEvent/*02000046*/
{
  .custom /*0C00002B:0600022C*/ instance void System.Management.Instrumentation.InstrumentationClassAttribute/*02000042*/::.ctor(valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/) /* 0600022C */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600023C*/ public hidebysig newslot virtual final 
          instance void  Fire() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d5
    // Code size       0 (0x0)
  } // end of method BaseEvent::Fire

  .method /*0600023D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d6
    // Code size       0 (0x0)
  } // end of method BaseEvent::.ctor

} // end of class System.Management.Instrumentation.BaseEvent

.class /*02000048*/ interface public abstract auto ansi System.Management.Instrumentation.IInstance
{
  .method /*0600023E*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_Published() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IInstance::get_Published

  .method /*0600023F*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Published(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method IInstance::set_Published

  .property /*17000090*/ instance callconv(8) bool
          Published()
  {
    .get instance bool System.Management.Instrumentation.IInstance/*02000048*/::get_Published() /* 0600023E */
    .set instance void System.Management.Instrumentation.IInstance/*02000048*/::set_Published(bool) /* 0600023F */
  } // end of property IInstance::Published
} // end of class System.Management.Instrumentation.IInstance

.class /*02000049*/ public abstract auto ansi beforefieldinit System.Management.Instrumentation.Instance
       extends [mscorlib/*23000001*/]System.Object/*01000007*/
       implements System.Management.Instrumentation.IInstance/*02000048*/
{
  .custom /*0C00002C:0600022C*/ instance void System.Management.Instrumentation.InstrumentationClassAttribute/*02000042*/::.ctor(valuetype System.Management.Instrumentation.InstrumentationType/*02000041*/) /* 0600022C */ = ( 01 00 00 00 00 00 00 00 ) 
  .method /*06000240*/ public hidebysig newslot specialname virtual final 
          instance bool  get_Published() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22d7
    // Code size       0 (0x0)
  } // end of method Instance::get_Published

  .method /*06000241*/ public hidebysig newslot specialname virtual final 
          instance void  set_Published(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22d8
    // Code size       0 (0x0)
  } // end of method Instance::set_Published

  .method /*06000242*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d9
    // Code size       0 (0x0)
  } // end of method Instance::.ctor

  .property /*17000091*/ instance callconv(8) bool
          Published()
  {
    .custom /*0C00002D:06000232*/ instance void System.Management.Instrumentation.IgnoreMemberAttribute/*02000044*/::.ctor() /* 06000232 */ = ( 01 00 00 00 ) 
    .set instance void System.Management.Instrumentation.Instance/*02000049*/::set_Published(bool) /* 06000241 */
    .get instance bool System.Management.Instrumentation.Instance/*02000049*/::get_Published() /* 06000240 */
  } // end of property Instance::Published
} // end of class System.Management.Instrumentation.Instance

.class /*0200004A*/ public auto ansi beforefieldinit System.Management.Instrumentation.ManagementInstaller
       extends [System.Configuration.Install/*23000003*/]System.Configuration.Install.Installer/*0100001F*/
{
  .method /*06000243*/ public hidebysig specialname virtual 
          instance string  get_HelpText() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22da
    // Code size       0 (0x0)
  } // end of method ManagementInstaller::get_HelpText

  .method /*06000244*/ public hidebysig virtual 
          instance void  Install(class [mscorlib/*23000001*/]System.Collections.IDictionary/*01000020*/ savedState) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x22db
    // Code size       0 (0x0)
  } // end of method ManagementInstaller::Install

  .method /*06000245*/ public hidebysig virtual 
          instance void  Commit(class [mscorlib/*23000001*/]System.Collections.IDictionary/*01000020*/ savedState) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x22dc
    // Code size       0 (0x0)
  } // end of method ManagementInstaller::Commit

  .method /*06000246*/ public hidebysig virtual 
          instance void  Rollback(class [mscorlib/*23000001*/]System.Collections.IDictionary/*01000020*/ savedState) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x22dd
    // Code size       0 (0x0)
  } // end of method ManagementInstaller::Rollback

  .method /*06000247*/ public hidebysig virtual 
          instance void  Uninstall(class [mscorlib/*23000001*/]System.Collections.IDictionary/*01000020*/ savedState) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x22de
    // Code size       0 (0x0)
  } // end of method ManagementInstaller::Uninstall

  .method /*06000248*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22df
    // Code size       0 (0x0)
  } // end of method ManagementInstaller::.ctor

  .method /*06000249*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x22e0
    // Code size       0 (0x0)
  } // end of method ManagementInstaller::.cctor

  .property /*17000092*/ instance callconv(8) string
          HelpText()
  {
    .get instance string System.Management.Instrumentation.ManagementInstaller/*0200004A*/::get_HelpText() /* 06000243 */
  } // end of property ManagementInstaller::HelpText
} // end of class System.Management.Instrumentation.ManagementInstaller

.class /*0200004B*/ public auto ansi beforefieldinit System.Management.Instrumentation.DefaultManagementProjectInstaller
       extends [System.Configuration.Install/*23000003*/]System.Configuration.Install.Installer/*0100001F*/
{
  .method /*0600024A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e1
    // Code size       0 (0x0)
  } // end of method DefaultManagementProjectInstaller::.ctor

} // end of class System.Management.Instrumentation.DefaultManagementProjectInstaller


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
