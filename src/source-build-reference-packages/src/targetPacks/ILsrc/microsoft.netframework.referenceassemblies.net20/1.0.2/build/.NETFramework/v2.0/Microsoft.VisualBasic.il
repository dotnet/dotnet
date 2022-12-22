
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
// Time-date stamp:            0x5c74324e
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x08
// Minor linker version:           0x00
// Size of code:                   0x0001a000
// Size of init.data:              0x00006000
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0001b4a6
// Base of code:                   0x00002000
// Base of data:                   0x0001c000
// Image base:                     0x5e430000
// Section alignment:              0x00002000
// File alignment:                 0x00001000
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00024000
// Size of headers:                0x00001000
// Checksum:                       0x00023513
// Subsystem:                      0x0003
// DLL characteristics:            0x0540
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0001b454 [0x0000004f] address [size] of Import Directory:          
// 0x0001c000 [0x00004358] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00021000 [0x00002390] address [size] of Security Directory:        
// 0x00022000 [0x0000000c] address [size] of Base Relocation Table:     
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
//              0x000194ac Virtual Size
//              0x00002000 Virtual Address
//              0x0001a000 Size of Raw Data
//              0x00001000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x00004358 Virtual Size
//              0x0001c000 Virtual Address
//              0x00005000 Size of Raw Data
//              0x0001b000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00022000 Virtual Address
//              0x00001000 Size of Raw Data
//              0x00020000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0001b000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000004a8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0001b496 Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 43 5E 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00002508 [0x0000ed14] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x0001121c [0x0000a1b8] address [size] of Resources Directory:       
// 0x0001b3d4 [0x00000080] address [size] of Strong Name Signature:     
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
//              'v2.0.50727' Version String
//    Storage Header:
//                    0x00 Flags
//                  0x0005 Number of Streams
//    Stream 1:
//              0x0000006c Offset
//              0x00008df8 Size
//              '#~' Name
//    Stream 2:
//              0x00008e64 Offset
//              0x00003db4 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000cc18 Offset
//              0x00000008 Size
//              '#US' Name
//    Stream 4:
//              0x0000cc20 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000cc30 Offset
//              0x000020e4 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000070903b45f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 144272
// PE header size       : 4096 (496 used)    ( 2.84%)
// PE additional info   : 26515             (18.38%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.05%)
// CLR meta-data size  : 60692              (42.07%)
// CLR additional info : 41528              (28.78%)
// CLR method headers  : 1190               ( 0.82%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 24576             (17.03%)
// Unaccounted          : -14397            (-9.98%)

// Num.of PE sections   : 3
//   .text    - 106496
//   .rsrc    - 20480
//   .reloc   - 4096

// CLR meta-data size  : 60692
//   Module        -    1 (10 bytes)
//   TypeDef       -  126 (1764 bytes)    1 interfaces, 0 explicit layout
//   TypeRef       -  100 (600 bytes)
//   MethodDef     - 1080 (15120 bytes)   2 abstract, 0 native, 1058 bodies
//   FieldDef      -  305 (1830 bytes)    0 constant
//   MemberRef     -   50 (300 bytes)
//   ParamDef      - 1574 (9444 bytes)
//   MethodImpl    -   19 (114 bytes)
//   Constant      -  396 (2376 bytes)
//   CustomAttribute-  233 (1398 bytes)
//   InterfaceImpl -    5 (20 bytes)
//   PropertyMap   -   37 (148 bytes)
//   Property      -  170 (1020 bytes)
//   MethodSemantic-  231 (1386 bytes)
//   Security      -   68 (408 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    8 (160 bytes)
//   ManifestResource-    3 (36 bytes)
//   NestedClass   -    3 (12 bytes)
//   EventMap      -    2 (8 bytes)
//   Event         -    6 (36 bytes)
//   GenericParam  -    2 (16 bytes)
//   Strings       - 15796 bytes
//   Blobs         -  8420 bytes
//   UserStrings   -     8 bytes
//   Guids         -    16 bytes
//   Uncategorized -   224 bytes

// CLR additional info : 41528
//   Resources         - 41400

// CLR method headers : 1190
//   Num.of method bodies  - 1058
//   Num.of fat headers    - 12
//   Num.of tiny headers   - 1046

// Managed code : 0
//   Ave method size - 0
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class ApplicationBase                (public) (auto) (ansi) 
// Class AssemblyInfo                   (public) (auto) (ansi) 
// Class ConsoleApplicationBase         (public) (auto) (ansi) 
// Class User                           (public) (auto) (ansi) 
// Class BuiltInRole                    (public) (auto) (ansi) (sealed) 
// Class BuiltInRoleConverter           (public) (auto) (ansi) 
// Class WebUser                        (public) (auto) (ansi) 
// Class AuthenticationMode             (public) (auto) (ansi) (sealed) 
// Class ShutdownMode                   (public) (auto) (ansi) (sealed) 
// Class UnhandledExceptionEventArgs    (public) (auto) (ansi) 
// Class StartupEventArgs               (public) (auto) (ansi) 
// Class StartupNextInstanceEventArgs   (public) (auto) (ansi) 
// Class StartupEventHandler            (public) (auto) (ansi) (sealed) 
// Class StartupNextInstanceEventHandler (public) (auto) (ansi) (sealed) 
// Class ShutdownEventHandler           (public) (auto) (ansi) (sealed) 
// Class UnhandledExceptionEventHandler (public) (auto) (ansi) (sealed) 
// Class NoStartupFormException         (public) (auto) (ansi) 
// Class CantStartSingleInstanceException (public) (auto) (ansi) 
// Class WindowsFormsApplicationBase    (public) (auto) (ansi) 
// Class Collection                     (public) (auto) (ansi) (sealed) 
// Class CollectionDebugView            (auto) (ansi) (sealed) (nested assembly) 
// Class Constants                      (public) (auto) (ansi) (sealed) 
// Class ControlChars                   (public) (auto) (ansi) (sealed) 
// Class Conversion                     (public) (auto) (ansi) (sealed) 
// Class DateAndTime                    (public) (auto) (ansi) (sealed) 
// Class AudioPlayMode                  (public) (auto) (ansi) (sealed) 
// Class Audio                          (public) (auto) (ansi) 
// Class Clock                          (public) (auto) (ansi) 
// Class Computer                       (public) (auto) (ansi) 
// Class ComputerInfo                   (public) (auto) (ansi) 
// Class ComputerInfoDebugView          (auto) (ansi) (sealed) (nested assembly) 
// Class Keyboard                       (public) (auto) (ansi) 
// Class Mouse                          (public) (auto) (ansi) 
// Class NetworkAvailableEventArgs      (public) (auto) (ansi) 
// Class NetworkAvailableEventHandler   (public) (auto) (ansi) (sealed) 
// Class Network                        (public) (auto) (ansi) 
// Class Ports                          (public) (auto) (ansi) 
// Class ServerComputer                 (public) (auto) (ansi) 
// Class ErrObject                      (public) (auto) (ansi) (sealed) 
// Class FileSystem                     (public) (auto) (ansi) 
// Class DeleteDirectoryOption          (public) (auto) (ansi) (sealed) 
// Class RecycleOption                  (public) (auto) (ansi) (sealed) 
// Class SearchOption                   (public) (auto) (ansi) (sealed) 
// Class UICancelOption                 (public) (auto) (ansi) (sealed) 
// Class UIOption                       (public) (auto) (ansi) (sealed) 
// Class MalformedLineException         (public) (auto) (ansi) 
// Class SpecialDirectories             (public) (auto) (ansi) 
// Class TextFieldParser                (public) (auto) (ansi) 
// Class FieldType                      (public) (auto) (ansi) (sealed) 
// Class FileSystem                     (public) (auto) (ansi) (sealed) 
// Class Financial                      (public) (auto) (ansi) (sealed) 
// Class VariantType                    (public) (auto) (ansi) (sealed) 
// Class AppWinStyle                    (public) (auto) (ansi) (sealed) 
// Class CallType                       (public) (auto) (ansi) (sealed) 
// Class CompareMethod                  (public) (auto) (ansi) (sealed) 
// Class DateFormat                     (public) (auto) (ansi) (sealed) 
// Class FirstDayOfWeek                 (public) (auto) (ansi) (sealed) 
// Class FileAttribute                  (public) (auto) (ansi) (sealed) 
// Class FirstWeekOfYear                (public) (auto) (ansi) (sealed) 
// Class VbStrConv                      (public) (auto) (ansi) (sealed) 
// Class TriState                       (public) (auto) (ansi) (sealed) 
// Class DateInterval                   (public) (auto) (ansi) (sealed) 
// Class DueDate                        (public) (auto) (ansi) (sealed) 
// Class OpenMode                       (public) (auto) (ansi) (sealed) 
// Class OpenAccess                     (public) (auto) (ansi) (sealed) 
// Class OpenShare                      (public) (auto) (ansi) (sealed) 
// Class TabInfo                        (public) (sequential) (ansi) (sealed) 
// Class SpcInfo                        (public) (sequential) (ansi) (sealed) 
// Class MsgBoxResult                   (public) (auto) (ansi) (sealed) 
// Class MsgBoxStyle                    (public) (auto) (ansi) (sealed) 
// Class VBFixedStringAttribute         (public) (auto) (ansi) (sealed) 
// Class VBFixedArrayAttribute          (public) (auto) (ansi) (sealed) 
// Class ComClassAttribute              (public) (auto) (ansi) (sealed) 
// Class MyGroupCollectionAttribute     (public) (auto) (ansi) (sealed) 
// Class HideModuleNameAttribute        (public) (auto) (ansi) (sealed) 
// Class Globals                        (public) (auto) (ansi) (sealed) 
// Class StandardModuleAttribute        (public) (auto) (ansi) (sealed) 
// Class OptionTextAttribute            (public) (auto) (ansi) (sealed) 
// Class OptionCompareAttribute         (public) (auto) (ansi) (sealed) 
// Class DesignerGeneratedAttribute     (public) (auto) (ansi) (sealed) 
// Class BooleanType                    (public) (auto) (ansi) (sealed) 
// Class ByteType                       (public) (auto) (ansi) (sealed) 
// Class CharArrayType                  (public) (auto) (ansi) (sealed) 
// Class CharType                       (public) (auto) (ansi) (sealed) 
// Class Conversions                    (public) (auto) (ansi) (sealed) 
// Class DateType                       (public) (auto) (ansi) (sealed) 
// Class DecimalType                    (public) (auto) (ansi) (sealed) 
// Class DoubleType                     (public) (auto) (ansi) (sealed) 
// Class ExceptionUtils                 (public) (auto) (ansi) (sealed) 
// Class InternalErrorException         (public) (auto) (ansi) (sealed) 
// Class FlowControl                    (public) (auto) (ansi) (sealed) 
// Class ObjectFlowControl              (public) (auto) (ansi) (sealed) 
// Class ForLoopControl                 (auto) (ansi) (sealed) (nested public) 
// Interface IVbHost                        (public) (abstract) (auto) (ansi) 
// Class HostServices                   (public) (auto) (ansi) (sealed) 
// Class IntegerType                    (public) (auto) (ansi) (sealed) 
// Class LateBinding                    (public) (auto) (ansi) (sealed) 
// Class LikeOperator                   (public) (auto) (ansi) (sealed) 
// Class LongType                       (public) (auto) (ansi) (sealed) 
// Class NewLateBinding                 (public) (auto) (ansi) (sealed) 
// Class ObjectType                     (public) (auto) (ansi) (sealed) 
// Class Operators                      (public) (auto) (ansi) (sealed) 
// Class ProjectData                    (public) (auto) (ansi) (sealed) 
// Class ShortType                      (public) (auto) (ansi) (sealed) 
// Class SingleType                     (public) (auto) (ansi) (sealed) 
// Class StaticLocalInitFlag            (public) (auto) (ansi) (sealed) 
// Class IncompleteInitialization       (public) (auto) (ansi) (sealed) 
// Class StringType                     (public) (auto) (ansi) (sealed) 
// Class Utils                          (public) (auto) (ansi) (sealed) 
// Class Versioned                      (public) (auto) (ansi) (sealed) 
// Class Information                    (public) (auto) (ansi) (sealed) 
// Class Interaction                    (public) (auto) (ansi) (sealed) 
// Class LogFileLocation                (public) (auto) (ansi) (sealed) 
// Class LogFileCreationScheduleOption  (public) (auto) (ansi) (sealed) 
// Class DiskSpaceExhaustedOption       (public) (auto) (ansi) (sealed) 
// Class FileLogTraceListener           (public) (auto) (ansi) 
// Class Log                            (public) (auto) (ansi) 
// Class AspLog                         (public) (auto) (ansi) 
// Class VBMath                         (public) (auto) (ansi) (sealed) 
// Class ClipboardProxy                 (public) (auto) (ansi) 
// Class FileSystemProxy                (public) (auto) (ansi) 
// Class ContextValue`1                 (public) (auto) (ansi) 
// Class RegistryProxy                  (public) (auto) (ansi) 
// Class SpecialDirectoriesProxy        (public) (auto) (ansi) 
// Class Strings                        (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v2.0.50727
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern /*23000002*/ System.Deployment
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 2:0:0:0
}
.assembly extern /*23000003*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern /*23000004*/ System.Windows.Forms
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern /*23000005*/ System.Runtime.Remoting
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern /*23000006*/ System.Management
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 2:0:0:0
}
.assembly extern /*23000007*/ System.Drawing
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 2:0:0:0
}
.assembly extern /*23000008*/ System.Web
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 2:0:0:0
}
.assembly /*20000001*/ Microsoft.VisualBasic
{
  .custom /*0C000001:0A000014*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000041*/::.ctor(string) /* 0A000014 */ = ( 01 00 19 4D 69 63 72 6F 73 6F 66 74 2E 56 69 73   // ...Microsoft.Vis
                                                                                                                                                           75 61 6C 42 61 73 69 63 2E 64 6C 6C 00 00 )       // ualBasic.dll..
  .custom /*0C000002:0A000015*/ instance void [mscorlib/*23000001*/]System.Security.AllowPartiallyTrustedCallersAttribute/*01000042*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .custom /*0C000003:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000004:0A000017*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*01000044*/::.ctor(string) /* 0A000017 */ = ( 01 00 19 4D 69 63 72 6F 73 6F 66 74 2E 56 69 73   // ...Microsoft.Vis
                                                                                                                                                                 75 61 6C 42 61 73 69 63 2E 64 6C 6C 00 00 )       // ualBasic.dll..
  .custom /*0C000005:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000045*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000019*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000046*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000046*//DebuggingModes/*01000047*/) /* 0A000019 */ = ( 01 00 03 00 00 00 00 00 ) 

  .custom /*0C000007:0A00001A*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000048*/::.ctor(int32) /* 0A00001A */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000008:0A00001B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000049*/::.ctor() /* 0A00001B */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C000009:0A00001C*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyKeyFileAttribute/*0100004A*/::.ctor(string) /* 0A00001C */ = ( 01 00 29 66 3A 5C 5C 64 64 5C 5C 54 6F 6F 6C 73   // ..)f:\\dd\\Tools
                                                                                                                                                             5C 5C 64 65 76 64 69 76 5C 5C 46 69 6E 61 6C 50   // \\devdiv\\FinalP
                                                                                                                                                             75 62 6C 69 63 4B 65 79 2E 73 6E 6B 00 00 )       // ublicKey.snk..
  .custom /*0C00000A:0A00001D*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDelaySignAttribute/*0100004B*/::.ctor(bool) /* 0A00001D */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000B:0A00001E*/ instance void [mscorlib/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*0100004C*/::.ctor(string) /* 0A00001E */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C00000C:0A00001F*/ instance void [mscorlib/*23000001*/]System.Resources.SatelliteContractVersionAttribute/*0100004D*/::.ctor(string) /* 0A00001F */ = ( 01 00 07 38 2E 30 2E 30 2E 30 00 00 )             // ...8.0.0.0..
  .custom /*0C00000D:0A000020*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100004E*/::.ctor(string) /* 0A000020 */ = ( 01 00 0E 38 2E 30 2E 35 30 37 32 37 2E 38 39 32   // ...8.0.50727.892
                                                                                                                                                                          32 00 00 )                                        // 2..
  .custom /*0C00000E:0A000021*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100004F*/::.ctor(string) /* 0A000021 */ = ( 01 00 0E 38 2E 30 2E 35 30 37 32 37 2E 38 39 32   // ...8.0.50727.892
                                                                                                                                                                 32 00 00 )                                        // 2..
  .custom /*0C00000F:0A000022*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000050*/::.ctor(string) /* 0A000022 */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                               43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                               20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                               2E 00 00 )                                        // ...
  .custom /*0C000010:0A000023*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000051*/::.ctor(string) /* 0A000023 */ = ( 01 00 24 4D 69 63 72 6F 73 6F 66 74 20 28 52 29   // ..$Microsoft (R)
                                                                                                                                                             20 56 69 73 75 61 6C 20 53 74 75 64 69 6F 20 28   //  Visual Studio (
                                                                                                                                                             52 29 20 32 30 30 35 00 00 )                      // R) 2005..
  .custom /*0C000011:0A000024*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000052*/::.ctor(string) /* 0A000024 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                             70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000012:0A000025*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000053*/::.ctor(string) /* 0A000025 */ = ( 01 00 19 4D 69 63 72 6F 73 6F 66 74 2E 56 69 73   // ...Microsoft.Vis
                                                                                                                                                                  75 61 6C 42 61 73 69 63 2E 64 6C 6C 00 00 )       // ualBasic.dll..
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
.module Microsoft.VisualBasic.DLL
// MVID: {ef293da8-f2e5-468d-ad40-816c38465e72}
.imagebase 0x5e430000
.file alignment 0x00001000
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F9255F68000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.ApplicationBase
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000001*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       0 (0x0)
  } // end of method ApplicationBase::.ctor

  .method /*06000002*/ public instance string 
          GetEnvironmentVariable(string name) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x2051
    // Code size       0 (0x0)
  } // end of method ApplicationBase::GetEnvironmentVariable

  .method /*06000003*/ public specialname 
          instance class Microsoft.VisualBasic.Logging.Log/*02000076*/ 
          get_Log() cil managed
  // SIG: 20 00 12 81 D8
  {
    // Method begins at RVA 0x2052
    // Code size       0 (0x0)
  } // end of method ApplicationBase::get_Log

  .method /*06000004*/ public specialname 
          instance class Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/ 
          get_Info() cil managed
  // SIG: 20 00 12 0C
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2053
    // Code size       0 (0x0)
  } // end of method ApplicationBase::get_Info

  .method /*06000005*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ 
          get_Culture() cil managed
  // SIG: 20 00 12 09
  {
    // Method begins at RVA 0x2054
    // Code size       0 (0x0)
  } // end of method ApplicationBase::get_Culture

  .method /*06000006*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ 
          get_UICulture() cil managed
  // SIG: 20 00 12 09
  {
    // Method begins at RVA 0x2055
    // Code size       0 (0x0)
  } // end of method ApplicationBase::get_UICulture

  .method /*06000007*/ public instance void 
          ChangeCulture(string cultureName) cil managed
  // SIG: 20 01 01 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2056
    // Code size       0 (0x0)
  } // end of method ApplicationBase::ChangeCulture

  .method /*06000008*/ public instance void 
          ChangeUICulture(string cultureName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2057
    // Code size       0 (0x0)
  } // end of method ApplicationBase::ChangeUICulture

  .property /*17000001*/ instance callconv(8) class Microsoft.VisualBasic.Logging.Log/*02000076*/
          Log()
  {
    .get instance class Microsoft.VisualBasic.Logging.Log/*02000076*/ Microsoft.VisualBasic.ApplicationServices.ApplicationBase/*02000002*/::get_Log() /* 06000003 */
  } // end of property ApplicationBase::Log
  .property /*17000002*/ instance callconv(8) class Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/
          Info()
  {
    .get instance class Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/ Microsoft.VisualBasic.ApplicationServices.ApplicationBase/*02000002*/::get_Info() /* 06000004 */
  } // end of property ApplicationBase::Info
  .property /*17000003*/ instance callconv(8) class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/
          Culture()
  {
    .get instance class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ Microsoft.VisualBasic.ApplicationServices.ApplicationBase/*02000002*/::get_Culture() /* 06000005 */
  } // end of property ApplicationBase::Culture
  .property /*17000004*/ instance callconv(8) class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/
          UICulture()
  {
    .get instance class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ Microsoft.VisualBasic.ApplicationServices.ApplicationBase/*02000002*/::get_UICulture() /* 06000006 */
  } // end of property ApplicationBase::UICulture
} // end of class Microsoft.VisualBasic.ApplicationServices.ApplicationBase

.class /*02000003*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.AssemblyInfo
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000009*/ public specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000003*/ currentAssembly) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x2058
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::.ctor

  .method /*0600000A*/ public specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2059
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_Description

  .method /*0600000B*/ public specialname 
          instance string  get_CompanyName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x205a
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_CompanyName

  .method /*0600000C*/ public specialname 
          instance string  get_Title() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x205b
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_Title

  .method /*0600000D*/ public specialname 
          instance string  get_Copyright() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x205c
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_Copyright

  .method /*0600000E*/ public specialname 
          instance string  get_Trademark() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x205d
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_Trademark

  .method /*0600000F*/ public specialname 
          instance string  get_ProductName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x205e
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_ProductName

  .method /*06000010*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Version/*01000004*/ 
          get_Version() cil managed
  // SIG: 20 00 12 11
  {
    // Method begins at RVA 0x205f
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_Version

  .method /*06000011*/ public specialname 
          instance string  get_AssemblyName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2060
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_AssemblyName

  .method /*06000012*/ public specialname 
          instance string  get_DirectoryPath() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2061
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_DirectoryPath

  .method /*06000013*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000003*/> 
          get_LoadedAssemblies() cil managed
  // SIG: 20 00 15 12 15 01 12 0D
  {
    // Method begins at RVA 0x2062
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_LoadedAssemblies

  .method /*06000014*/ public specialname 
          instance string  get_StackTrace() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2063
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_StackTrace

  .method /*06000015*/ public specialname 
          instance int64  get_WorkingSet() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2064
    // Code size       0 (0x0)
  } // end of method AssemblyInfo::get_WorkingSet

  .property /*17000005*/ instance callconv(8) string
          Description()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_Description() /* 0600000A */
  } // end of property AssemblyInfo::Description
  .property /*17000006*/ instance callconv(8) string
          CompanyName()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_CompanyName() /* 0600000B */
  } // end of property AssemblyInfo::CompanyName
  .property /*17000007*/ instance callconv(8) string
          Title()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_Title() /* 0600000C */
  } // end of property AssemblyInfo::Title
  .property /*17000008*/ instance callconv(8) string
          Copyright()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_Copyright() /* 0600000D */
  } // end of property AssemblyInfo::Copyright
  .property /*17000009*/ instance callconv(8) string
          Trademark()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_Trademark() /* 0600000E */
  } // end of property AssemblyInfo::Trademark
  .property /*1700000A*/ instance callconv(8) string
          ProductName()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_ProductName() /* 0600000F */
  } // end of property AssemblyInfo::ProductName
  .property /*1700000B*/ instance callconv(8) class [mscorlib/*23000001*/]System.Version/*01000004*/
          Version()
  {
    .get instance class [mscorlib/*23000001*/]System.Version/*01000004*/ Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_Version() /* 06000010 */
  } // end of property AssemblyInfo::Version
  .property /*1700000C*/ instance callconv(8) string
          AssemblyName()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_AssemblyName() /* 06000011 */
  } // end of property AssemblyInfo::AssemblyName
  .property /*1700000D*/ instance callconv(8) string
          DirectoryPath()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_DirectoryPath() /* 06000012 */
  } // end of property AssemblyInfo::DirectoryPath
  .property /*1700000E*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000003*/>
          LoadedAssemblies()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.Reflection.Assembly/*01000003*/> Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_LoadedAssemblies() /* 06000013 */
  } // end of property AssemblyInfo::LoadedAssemblies
  .property /*1700000F*/ instance callconv(8) string
          StackTrace()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_StackTrace() /* 06000014 */
  } // end of property AssemblyInfo::StackTrace
  .property /*17000010*/ instance callconv(8) int64
          WorkingSet()
  {
    .get instance int64 Microsoft.VisualBasic.ApplicationServices.AssemblyInfo/*02000003*/::get_WorkingSet() /* 06000015 */
  } // end of property AssemblyInfo::WorkingSet
} // end of class Microsoft.VisualBasic.ApplicationServices.AssemblyInfo

.class /*02000004*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase
       extends Microsoft.VisualBasic.ApplicationServices.ApplicationBase/*02000002*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000016*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2065
    // Code size       0 (0x0)
  } // end of method ConsoleApplicationBase::.ctor

  .method /*06000017*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          get_CommandLineArgs() cil managed
  // SIG: 20 00 15 12 15 01 0E
  {
    // Method begins at RVA 0x2066
    // Code size       0 (0x0)
  } // end of method ConsoleApplicationBase::get_CommandLineArgs

  .method /*06000018*/ public specialname 
          instance class [System.Deployment/*23000002*/]System.Deployment.Application.ApplicationDeployment/*01000006*/ 
          get_Deployment() cil managed
  // SIG: 20 00 12 19
  {
    // Method begins at RVA 0x2067
    // Code size       0 (0x0)
  } // end of method ConsoleApplicationBase::get_Deployment

  .method /*06000019*/ public specialname 
          instance bool  get_IsNetworkDeployed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2068
    // Code size       0 (0x0)
  } // end of method ConsoleApplicationBase::get_IsNetworkDeployed

  .method /*0600001A*/ family specialname 
          instance void  set_InternalCommandLine(class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 15 01 0E
  {
    // Method begins at RVA 0x2069
    // Code size       0 (0x0)
  } // end of method ConsoleApplicationBase::set_InternalCommandLine

  .property /*17000011*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string>
          CommandLineArgs()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase/*02000004*/::get_CommandLineArgs() /* 06000017 */
  } // end of property ConsoleApplicationBase::CommandLineArgs
  .property /*17000012*/ instance callconv(8) class [System.Deployment/*23000002*/]System.Deployment.Application.ApplicationDeployment/*01000006*/
          Deployment()
  {
    .get instance class [System.Deployment/*23000002*/]System.Deployment.Application.ApplicationDeployment/*01000006*/ Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase/*02000004*/::get_Deployment() /* 06000018 */
  } // end of property ConsoleApplicationBase::Deployment
  .property /*17000013*/ instance callconv(8) bool
          IsNetworkDeployed()
  {
    .get instance bool Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase/*02000004*/::get_IsNetworkDeployed() /* 06000019 */
  } // end of property ConsoleApplicationBase::IsNetworkDeployed
  .property /*17000014*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string>
          InternalCommandLine()
  {
    .custom /*0C00001F:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .set instance void Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase/*02000004*/::set_InternalCommandLine(class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string>) /* 0600001A */
  } // end of property ConsoleApplicationBase::InternalCommandLine
} // end of class Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase

.class /*02000005*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.User
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*0600001B*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206a
    // Code size       0 (0x0)
  } // end of method User::.ctor

  .method /*0600001C*/ public specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x206b
    // Code size       0 (0x0)
  } // end of method User::get_Name

  .method /*0600001D*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ 
          get_CurrentPrincipal() cil managed
  // SIG: 20 00 12 1D
  {
    // Method begins at RVA 0x206c
    // Code size       0 (0x0)
  } // end of method User::get_CurrentPrincipal

  .method /*0600001E*/ public specialname 
          instance void  set_CurrentPrincipal(class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1D
  {
    // Method begins at RVA 0x206d
    // Code size       0 (0x0)
  } // end of method User::set_CurrentPrincipal

  .method /*0600001F*/ public instance void 
          InitializeWithWindowsUser() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000027:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x206e
    // Code size       0 (0x0)
  } // end of method User::InitializeWithWindowsUser

  .method /*06000020*/ public specialname 
          instance bool  get_IsAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x206f
    // Code size       0 (0x0)
  } // end of method User::get_IsAuthenticated

  .method /*06000021*/ public instance bool 
          IsInRole(string role) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x2070
    // Code size       0 (0x0)
  } // end of method User::IsInRole

  .method /*06000022*/ public instance bool 
          IsInRole(valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ role) cil managed
  // SIG: 20 01 02 11 18
  {
    // Method begins at RVA 0x2071
    // Code size       0 (0x0)
  } // end of method User::IsInRole

  .method /*06000023*/ family newslot specialname strict virtual 
          instance class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ 
          get_InternalPrincipal() cil managed
  // SIG: 20 00 12 1D
  {
    // Method begins at RVA 0x2072
    // Code size       0 (0x0)
  } // end of method User::get_InternalPrincipal

  .method /*06000024*/ family newslot specialname strict virtual 
          instance void  set_InternalPrincipal(class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1D
  {
    // Method begins at RVA 0x2073
    // Code size       0 (0x0)
  } // end of method User::set_InternalPrincipal

  .property /*17000015*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.VisualBasic.ApplicationServices.User/*02000005*/::get_Name() /* 0600001C */
  } // end of property User::Name
  .property /*17000016*/ instance callconv(8) class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/
          CurrentPrincipal()
  {
    .custom /*0C000023:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get instance class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ Microsoft.VisualBasic.ApplicationServices.User/*02000005*/::get_CurrentPrincipal() /* 0600001D */
    .set instance void Microsoft.VisualBasic.ApplicationServices.User/*02000005*/::set_CurrentPrincipal(class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/) /* 0600001E */
  } // end of property User::CurrentPrincipal
  .property /*17000017*/ instance callconv(8) bool
          IsAuthenticated()
  {
    .get instance bool Microsoft.VisualBasic.ApplicationServices.User/*02000005*/::get_IsAuthenticated() /* 06000020 */
  } // end of property User::IsAuthenticated
  .property /*17000018*/ instance callconv(8) class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/
          InternalPrincipal()
  {
    .get instance class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ Microsoft.VisualBasic.ApplicationServices.User/*02000005*/::get_InternalPrincipal() /* 06000023 */
    .set instance void Microsoft.VisualBasic.ApplicationServices.User/*02000005*/::set_InternalPrincipal(class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/) /* 06000024 */
  } // end of property User::InternalPrincipal
} // end of class Microsoft.VisualBasic.ApplicationServices.User

.class /*02000006*/ public auto ansi sealed Microsoft.VisualBasic.ApplicationServices.BuiltInRole
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .custom /*0C000013:0A000027*/ instance void [System/*23000003*/]System.ComponentModel.TypeConverterAttribute/*01000056*/::.ctor(class [mscorlib/*23000001*/]System.Type/*0100000B*/) /* 0A000027 */ = ( 01 00 3E 4D 69 63 72 6F 73 6F 66 74 2E 56 69 73   // ..>Microsoft.Vis
                                                                                                                                                                                                          75 61 6C 42 61 73 69 63 2E 41 70 70 6C 69 63 61   // ualBasic.Applica
                                                                                                                                                                                                          74 69 6F 6E 53 65 72 76 69 63 65 73 2E 42 75 69   // tionServices.Bui
                                                                                                                                                                                                          6C 74 49 6E 52 6F 6C 65 43 6F 6E 76 65 72 74 65   // ltInRoleConverte
                                                                                                                                                                                                          72 00 00 )                                        // r..
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ AccountOperator = int32(0x00000224)
  .field /*04000003*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ Administrator = int32(0x00000220)
  .field /*04000004*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ BackupOperator = int32(0x00000227)
  .field /*04000005*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ Guest = int32(0x00000222)
  .field /*04000006*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ PowerUser = int32(0x00000223)
  .field /*04000007*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ PrintOperator = int32(0x00000226)
  .field /*04000008*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ Replicator = int32(0x00000228)
  .field /*04000009*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ SystemOperator = int32(0x00000225)
  .field /*0400000A*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.BuiltInRole/*02000006*/ User = int32(0x00000221)
} // end of class Microsoft.VisualBasic.ApplicationServices.BuiltInRole

.class /*02000007*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.BuiltInRoleConverter
       extends [System/*23000003*/]System.ComponentModel.TypeConverter/*01000009*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'SharedState' = bool(true)}}
  .method /*06000025*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2074
    // Code size       0 (0x0)
  } // end of method BuiltInRoleConverter::.ctor

  .method /*06000026*/ public strict virtual 
          instance bool  CanConvertTo(class [System/*23000003*/]System.ComponentModel.ITypeDescriptorContext/*0100000A*/ context,
                                      class [mscorlib/*23000001*/]System.Type/*0100000B*/ destinationType) cil managed
  // SIG: 20 02 02 12 29 12 2D
  {
    // Method begins at RVA 0x2075
    // Code size       0 (0x0)
  } // end of method BuiltInRoleConverter::CanConvertTo

  .method /*06000027*/ public strict virtual 
          instance object  ConvertTo(class [System/*23000003*/]System.ComponentModel.ITypeDescriptorContext/*0100000A*/ context,
                                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ culture,
                                     object 'value',
                                     class [mscorlib/*23000001*/]System.Type/*0100000B*/ destinationType) cil managed
  // SIG: 20 04 1C 12 29 12 09 1C 12 2D
  {
    // Method begins at RVA 0x2076
    // Code size       0 (0x0)
  } // end of method BuiltInRoleConverter::ConvertTo

} // end of class Microsoft.VisualBasic.ApplicationServices.BuiltInRoleConverter

.class /*02000008*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.WebUser
       extends Microsoft.VisualBasic.ApplicationServices.User/*02000005*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000028*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2077
    // Code size       0 (0x0)
  } // end of method WebUser::.ctor

  .method /*06000029*/ family specialname strict virtual 
          instance class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ 
          get_InternalPrincipal() cil managed
  // SIG: 20 00 12 1D
  {
    // Method begins at RVA 0x2078
    // Code size       0 (0x0)
  } // end of method WebUser::get_InternalPrincipal

  .method /*0600002A*/ family specialname strict virtual 
          instance void  set_InternalPrincipal(class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1D
  {
    // Method begins at RVA 0x2079
    // Code size       0 (0x0)
  } // end of method WebUser::set_InternalPrincipal

  .property /*17000019*/ instance callconv(8) class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/
          InternalPrincipal()
  {
    .set instance void Microsoft.VisualBasic.ApplicationServices.WebUser/*02000008*/::set_InternalPrincipal(class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/) /* 0600002A */
    .get instance class [mscorlib/*23000001*/]System.Security.Principal.IPrincipal/*01000007*/ Microsoft.VisualBasic.ApplicationServices.WebUser/*02000008*/::get_InternalPrincipal() /* 06000029 */
  } // end of property WebUser::InternalPrincipal
} // end of class Microsoft.VisualBasic.ApplicationServices.WebUser

.class /*02000009*/ public auto ansi sealed Microsoft.VisualBasic.ApplicationServices.AuthenticationMode
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*0400000B*/ public specialname rtspecialname int32 value__
  .field /*0400000C*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.AuthenticationMode/*02000009*/ Windows = int32(0x00000000)
  .field /*0400000D*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.AuthenticationMode/*02000009*/ ApplicationDefined = int32(0x00000001)
} // end of class Microsoft.VisualBasic.ApplicationServices.AuthenticationMode

.class /*0200000A*/ public auto ansi sealed Microsoft.VisualBasic.ApplicationServices.ShutdownMode
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*0400000E*/ public specialname rtspecialname int32 value__
  .field /*0400000F*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.ShutdownMode/*0200000A*/ AfterMainFormCloses = int32(0x00000000)
  .field /*04000010*/ public static literal valuetype Microsoft.VisualBasic.ApplicationServices.ShutdownMode/*0200000A*/ AfterAllFormsClose = int32(0x00000001)
} // end of class Microsoft.VisualBasic.ApplicationServices.ShutdownMode

.class /*0200000B*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs
       extends [System/*23000003*/]System.Threading.ThreadExceptionEventArgs/*0100000C*/
{
  .custom /*0C000014:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000045*/::.ctor(bool) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000015:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*0600002B*/ public specialname rtspecialname 
          instance void  .ctor(bool exitApplication,
                               class [mscorlib/*23000001*/]System.Exception/*0100000D*/ exception) cil managed
  // SIG: 20 02 01 02 12 35
  {
    // Method begins at RVA 0x207a
    // Code size       0 (0x0)
  } // end of method UnhandledExceptionEventArgs::.ctor

  .method /*0600002C*/ public specialname 
          instance bool  get_ExitApplication() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x207b
    // Code size       0 (0x0)
  } // end of method UnhandledExceptionEventArgs::get_ExitApplication

  .method /*0600002D*/ public specialname 
          instance void  set_ExitApplication(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x207c
    // Code size       0 (0x0)
  } // end of method UnhandledExceptionEventArgs::set_ExitApplication

  .property /*1700001A*/ instance callconv(8) bool
          ExitApplication()
  {
    .set instance void Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs/*0200000B*/::set_ExitApplication(bool) /* 0600002D */
    .get instance bool Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs/*0200000B*/::get_ExitApplication() /* 0600002C */
  } // end of property UnhandledExceptionEventArgs::ExitApplication
} // end of class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs

.class /*0200000C*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.StartupEventArgs
       extends [System/*23000003*/]System.ComponentModel.CancelEventArgs/*0100000E*/
{
  .custom /*0C000016:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000045*/::.ctor(bool) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000017:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*0600002E*/ public specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> args) cil managed
  // SIG: 20 01 01 15 12 15 01 0E
  {
    // Method begins at RVA 0x207d
    // Code size       0 (0x0)
  } // end of method StartupEventArgs::.ctor

  .method /*0600002F*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          get_CommandLine() cil managed
  // SIG: 20 00 15 12 15 01 0E
  {
    // Method begins at RVA 0x207e
    // Code size       0 (0x0)
  } // end of method StartupEventArgs::get_CommandLine

  .property /*1700001B*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string>
          CommandLine()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> Microsoft.VisualBasic.ApplicationServices.StartupEventArgs/*0200000C*/::get_CommandLine() /* 0600002F */
  } // end of property StartupEventArgs::CommandLine
} // end of class Microsoft.VisualBasic.ApplicationServices.StartupEventArgs

.class /*0200000D*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs
       extends [mscorlib/*23000001*/]System.EventArgs/*0100000F*/
{
  .custom /*0C000018:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*06000030*/ public specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> args,
                               bool bringToForegroundFlag) cil managed
  // SIG: 20 02 01 15 12 15 01 0E 02
  {
    // Method begins at RVA 0x207f
    // Code size       0 (0x0)
  } // end of method StartupNextInstanceEventArgs::.ctor

  .method /*06000031*/ public specialname 
          instance bool  get_BringToForeground() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2080
    // Code size       0 (0x0)
  } // end of method StartupNextInstanceEventArgs::get_BringToForeground

  .method /*06000032*/ public specialname 
          instance void  set_BringToForeground(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2081
    // Code size       0 (0x0)
  } // end of method StartupNextInstanceEventArgs::set_BringToForeground

  .method /*06000033*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          get_CommandLine() cil managed
  // SIG: 20 00 15 12 15 01 0E
  {
    // Method begins at RVA 0x2082
    // Code size       0 (0x0)
  } // end of method StartupNextInstanceEventArgs::get_CommandLine

  .property /*1700001C*/ instance callconv(8) bool
          BringToForeground()
  {
    .set instance void Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs/*0200000D*/::set_BringToForeground(bool) /* 06000032 */
    .get instance bool Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs/*0200000D*/::get_BringToForeground() /* 06000031 */
  } // end of property StartupNextInstanceEventArgs::BringToForeground
  .property /*1700001D*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string>
          CommandLine()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs/*0200000D*/::get_CommandLine() /* 06000033 */
  } // end of property StartupNextInstanceEventArgs::CommandLine
} // end of class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs

.class /*0200000E*/ public auto ansi sealed Microsoft.VisualBasic.ApplicationServices.StartupEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*01000010*/
{
  .custom /*0C000019:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*06000034*/ public specialname rtspecialname 
          instance void  .ctor(object TargetObject,
                               native int TargetMethod) runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method StartupEventHandler::.ctor

  .method /*06000035*/ public newslot strict virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ 
          BeginInvoke(object sender,
                      class Microsoft.VisualBasic.ApplicationServices.StartupEventArgs/*0200000C*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*01000012*/ DelegateCallback,
                      object DelegateAsyncState) runtime managed
  // SIG: 20 04 12 45 1C 12 30 12 49 1C
  {
  } // end of method StartupEventHandler::BeginInvoke

  .method /*06000036*/ public newslot strict virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ DelegateAsyncResult) runtime managed
  // SIG: 20 01 01 12 45
  {
  } // end of method StartupEventHandler::EndInvoke

  .method /*06000037*/ public newslot strict virtual 
          instance void  Invoke(object sender,
                                class Microsoft.VisualBasic.ApplicationServices.StartupEventArgs/*0200000C*/ e) runtime managed
  // SIG: 20 02 01 1C 12 30
  {
  } // end of method StartupEventHandler::Invoke

} // end of class Microsoft.VisualBasic.ApplicationServices.StartupEventHandler

.class /*0200000F*/ public auto ansi sealed Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*01000010*/
{
  .custom /*0C00001A:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*06000038*/ public specialname rtspecialname 
          instance void  .ctor(object TargetObject,
                               native int TargetMethod) runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method StartupNextInstanceEventHandler::.ctor

  .method /*06000039*/ public newslot strict virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ 
          BeginInvoke(object sender,
                      class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs/*0200000D*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*01000012*/ DelegateCallback,
                      object DelegateAsyncState) runtime managed
  // SIG: 20 04 12 45 1C 12 34 12 49 1C
  {
  } // end of method StartupNextInstanceEventHandler::BeginInvoke

  .method /*0600003A*/ public newslot strict virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ DelegateAsyncResult) runtime managed
  // SIG: 20 01 01 12 45
  {
  } // end of method StartupNextInstanceEventHandler::EndInvoke

  .method /*0600003B*/ public newslot strict virtual 
          instance void  Invoke(object sender,
                                class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs/*0200000D*/ e) runtime managed
  // SIG: 20 02 01 1C 12 34
  {
  } // end of method StartupNextInstanceEventHandler::Invoke

} // end of class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventHandler

.class /*02000010*/ public auto ansi sealed Microsoft.VisualBasic.ApplicationServices.ShutdownEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*01000010*/
{
  .custom /*0C00001B:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*0600003C*/ public specialname rtspecialname 
          instance void  .ctor(object TargetObject,
                               native int TargetMethod) runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ShutdownEventHandler::.ctor

  .method /*0600003D*/ public newslot strict virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ 
          BeginInvoke(object sender,
                      class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*01000012*/ DelegateCallback,
                      object DelegateAsyncState) runtime managed
  // SIG: 20 04 12 45 1C 12 3D 12 49 1C
  {
  } // end of method ShutdownEventHandler::BeginInvoke

  .method /*0600003E*/ public newslot strict virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ DelegateAsyncResult) runtime managed
  // SIG: 20 01 01 12 45
  {
  } // end of method ShutdownEventHandler::EndInvoke

  .method /*0600003F*/ public newslot strict virtual 
          instance void  Invoke(object sender,
                                class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) runtime managed
  // SIG: 20 02 01 1C 12 3D
  {
  } // end of method ShutdownEventHandler::Invoke

} // end of class Microsoft.VisualBasic.ApplicationServices.ShutdownEventHandler

.class /*02000011*/ public auto ansi sealed Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*01000010*/
{
  .custom /*0C00001C:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*06000040*/ public specialname rtspecialname 
          instance void  .ctor(object TargetObject,
                               native int TargetMethod) runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method UnhandledExceptionEventHandler::.ctor

  .method /*06000041*/ public newslot strict virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ 
          BeginInvoke(object sender,
                      class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs/*0200000B*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*01000012*/ DelegateCallback,
                      object DelegateAsyncState) runtime managed
  // SIG: 20 04 12 45 1C 12 2C 12 49 1C
  {
  } // end of method UnhandledExceptionEventHandler::BeginInvoke

  .method /*06000042*/ public newslot strict virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ DelegateAsyncResult) runtime managed
  // SIG: 20 01 01 12 45
  {
  } // end of method UnhandledExceptionEventHandler::EndInvoke

  .method /*06000043*/ public newslot strict virtual 
          instance void  Invoke(object sender,
                                class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs/*0200000B*/ e) runtime managed
  // SIG: 20 02 01 1C 12 2C
  {
  } // end of method UnhandledExceptionEventHandler::Invoke

} // end of class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventHandler

.class /*02000012*/ public auto ansi serializable Microsoft.VisualBasic.ApplicationServices.NoStartupFormException
       extends [mscorlib/*23000001*/]System.Exception/*0100000D*/
{
  .custom /*0C00001D:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000044*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2083
    // Code size       0 (0x0)
  } // end of method NoStartupFormException::.ctor

  .method /*06000045*/ public specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2084
    // Code size       0 (0x0)
  } // end of method NoStartupFormException::.ctor

  .method /*06000046*/ public specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*0100000D*/ inner) cil managed
  // SIG: 20 02 01 0E 12 35
  {
    // Method begins at RVA 0x2085
    // Code size       0 (0x0)
  } // end of method NoStartupFormException::.ctor

  .method /*06000047*/ family specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000013*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000014*/ context) cil managed
  // SIG: 20 02 01 12 4D 11 51
  {
    .custom /*0C000038:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x2086
    // Code size       0 (0x0)
  } // end of method NoStartupFormException::.ctor

} // end of class Microsoft.VisualBasic.ApplicationServices.NoStartupFormException

.class /*02000013*/ public auto ansi serializable Microsoft.VisualBasic.ApplicationServices.CantStartSingleInstanceException
       extends [mscorlib/*23000001*/]System.Exception/*0100000D*/
{
  .custom /*0C00001E:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000048*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2087
    // Code size       0 (0x0)
  } // end of method CantStartSingleInstanceException::.ctor

  .method /*06000049*/ public specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2088
    // Code size       0 (0x0)
  } // end of method CantStartSingleInstanceException::.ctor

  .method /*0600004A*/ public specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*0100000D*/ inner) cil managed
  // SIG: 20 02 01 0E 12 35
  {
    // Method begins at RVA 0x2089
    // Code size       0 (0x0)
  } // end of method CantStartSingleInstanceException::.ctor

  .method /*0600004B*/ family specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000013*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000014*/ context) cil managed
  // SIG: 20 02 01 12 4D 11 51
  {
    .custom /*0C00003D:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x208a
    // Code size       0 (0x0)
  } // end of method CantStartSingleInstanceException::.ctor

} // end of class Microsoft.VisualBasic.ApplicationServices.CantStartSingleInstanceException

.class /*02000014*/ public auto ansi Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
       extends Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase/*02000004*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*0600004C*/ public specialname 
          instance void  add_Startup(class Microsoft.VisualBasic.ApplicationServices.StartupEventHandler/*0200000E*/ obj) cil managed synchronized
  // SIG: 20 01 01 12 38
  {
    // Method begins at RVA 0x208b
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::add_Startup

  .method /*0600004D*/ public specialname 
          instance void  remove_Startup(class Microsoft.VisualBasic.ApplicationServices.StartupEventHandler/*0200000E*/ obj) cil managed synchronized
  // SIG: 20 01 01 12 38
  {
    // Method begins at RVA 0x208c
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::remove_Startup

  .method /*0600004E*/ public specialname 
          instance void  add_StartupNextInstance(class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventHandler/*0200000F*/ obj) cil managed synchronized
  // SIG: 20 01 01 12 3C
  {
    // Method begins at RVA 0x208d
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::add_StartupNextInstance

  .method /*0600004F*/ public specialname 
          instance void  remove_StartupNextInstance(class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventHandler/*0200000F*/ obj) cil managed synchronized
  // SIG: 20 01 01 12 3C
  {
    // Method begins at RVA 0x208e
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::remove_StartupNextInstance

  .method /*06000050*/ public specialname 
          instance void  add_Shutdown(class Microsoft.VisualBasic.ApplicationServices.ShutdownEventHandler/*02000010*/ obj) cil managed synchronized
  // SIG: 20 01 01 12 40
  {
    // Method begins at RVA 0x208f
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::add_Shutdown

  .method /*06000051*/ public specialname 
          instance void  remove_Shutdown(class Microsoft.VisualBasic.ApplicationServices.ShutdownEventHandler/*02000010*/ obj) cil managed synchronized
  // SIG: 20 01 01 12 40
  {
    // Method begins at RVA 0x2090
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::remove_Shutdown

  .method /*06000052*/ public specialname 
          instance void  add_NetworkAvailabilityChanged(class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x2091
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::add_NetworkAvailabilityChanged

  .method /*06000053*/ public specialname 
          instance void  remove_NetworkAvailabilityChanged(class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x2092
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::remove_NetworkAvailabilityChanged

  .method /*06000054*/ private specialname 
          instance void  raise_NetworkAvailabilityChanged(object sender,
                                                          class Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs/*02000023*/ e) cil managed
  // SIG: 20 02 01 1C 12 80 8C
  {
    // Method begins at RVA 0x2093
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::raise_NetworkAvailabilityChanged

  .method /*06000055*/ public specialname 
          instance void  add_UnhandledException(class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventHandler/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x2094
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::add_UnhandledException

  .method /*06000056*/ public specialname 
          instance void  remove_UnhandledException(class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventHandler/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x2095
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::remove_UnhandledException

  .method /*06000057*/ private specialname 
          instance void  raise_UnhandledException(object sender,
                                                  class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs/*0200000B*/ e) cil managed
  // SIG: 20 02 01 1C 12 2C
  {
    // Method begins at RVA 0x2096
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::raise_UnhandledException

  .method /*06000058*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2097
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::.ctor

  .method /*06000059*/ public specialname rtspecialname 
          instance void  .ctor(valuetype Microsoft.VisualBasic.ApplicationServices.AuthenticationMode/*02000009*/ authenticationMode) cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x2098
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::.ctor

  .method /*0600005A*/ public instance void 
          Run(string[] commandLine) cil managed
  // SIG: 20 01 01 1D 0E
  {
    // Method begins at RVA 0x2099
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::Run

  .method /*0600005B*/ public specialname 
          instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.FormCollection/*01000015*/ 
          get_OpenForms() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x209a
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_OpenForms

  .method /*0600005C*/ family specialname 
          instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/ 
          get_MainForm() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x209b
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_MainForm

  .method /*0600005D*/ family specialname 
          instance void  set_MainForm(class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/ 'value') cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x209c
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::set_MainForm

  .method /*0600005E*/ public specialname 
          instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/ 
          get_SplashScreen() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x209d
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_SplashScreen

  .method /*0600005F*/ public specialname 
          instance void  set_SplashScreen(class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/ 'value') cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x209e
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::set_SplashScreen

  .method /*06000060*/ public specialname 
          instance int32  get_MinimumSplashScreenDisplayTime() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x209f
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_MinimumSplashScreenDisplayTime

  .method /*06000061*/ public specialname 
          instance void  set_MinimumSplashScreenDisplayTime(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20a0
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::set_MinimumSplashScreenDisplayTime

  .method /*06000062*/ family specialname static 
          bool  get_UseCompatibleTextRendering() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x20a1
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_UseCompatibleTextRendering

  .method /*06000063*/ public specialname 
          instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.ApplicationContext/*01000017*/ 
          get_ApplicationContext() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x20a2
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_ApplicationContext

  .method /*06000064*/ public specialname 
          instance bool  get_SaveMySettingsOnExit() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20a3
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_SaveMySettingsOnExit

  .method /*06000065*/ public specialname 
          instance void  set_SaveMySettingsOnExit(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20a4
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::set_SaveMySettingsOnExit

  .method /*06000066*/ public instance void 
          DoEvents() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a5
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::DoEvents

  .method /*06000067*/ family newslot strict virtual 
          instance bool  OnInitialize(class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> commandLineArgs) cil managed
  // SIG: 20 01 02 15 12 15 01 0E
  {
    .custom /*0C000060:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .custom /*0C000061:0A000028*/ instance void [mscorlib/*23000001*/]System.STAThreadAttribute/*01000057*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a6
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::OnInitialize

  .method /*06000068*/ family newslot strict virtual 
          instance bool  OnStartup(class Microsoft.VisualBasic.ApplicationServices.StartupEventArgs/*0200000C*/ eventArgs) cil managed
  // SIG: 20 01 02 12 30
  {
    .custom /*0C000063:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20a7
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::OnStartup

  .method /*06000069*/ family newslot strict virtual 
          instance void  OnStartupNextInstance(class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs/*0200000D*/ eventArgs) cil managed
  // SIG: 20 01 01 12 34
  {
    .custom /*0C000065:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20a8
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::OnStartupNextInstance

  .method /*0600006A*/ family newslot strict virtual 
          instance void  OnRun() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000067:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20a9
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::OnRun

  .method /*0600006B*/ family newslot strict virtual 
          instance void  OnCreateSplashScreen() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000069:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20aa
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::OnCreateSplashScreen

  .method /*0600006C*/ family newslot strict virtual 
          instance void  OnCreateMainForm() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00006B:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20ab
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::OnCreateMainForm

  .method /*0600006D*/ family newslot strict virtual 
          instance void  OnShutdown() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00006D:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20ac
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::OnShutdown

  .method /*0600006E*/ family newslot strict virtual 
          instance bool  OnUnhandledException(class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs/*0200000B*/ e) cil managed
  // SIG: 20 01 02 12 2C
  {
    .custom /*0C00006F:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20ad
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::OnUnhandledException

  .method /*0600006F*/ family instance void 
          ShowSplashScreen() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000072:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20ae
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::ShowSplashScreen

  .method /*06000070*/ family instance void 
          HideSplashScreen() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000074:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x20af
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::HideSplashScreen

  .method /*06000071*/ famorassem specialname 
          instance valuetype Microsoft.VisualBasic.ApplicationServices.ShutdownMode/*0200000A*/ 
          get_ShutdownStyle() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x20b0
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_ShutdownStyle

  .method /*06000072*/ famorassem specialname 
          instance void  set_ShutdownStyle(valuetype Microsoft.VisualBasic.ApplicationServices.ShutdownMode/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x20b1
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::set_ShutdownStyle

  .method /*06000073*/ family specialname 
          instance bool  get_EnableVisualStyles() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b2
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_EnableVisualStyles

  .method /*06000074*/ family specialname 
          instance void  set_EnableVisualStyles(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20b3
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::set_EnableVisualStyles

  .method /*06000075*/ family specialname 
          instance bool  get_IsSingleInstance() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b4
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::get_IsSingleInstance

  .method /*06000076*/ family specialname 
          instance void  set_IsSingleInstance(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20b5
    // Code size       0 (0x0)
  } // end of method WindowsFormsApplicationBase::set_IsSingleInstance

  .event /*14000001*/ Microsoft.VisualBasic.ApplicationServices.StartupEventHandler/*0200000E*/ Startup
  {
    .removeon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::remove_Startup(class Microsoft.VisualBasic.ApplicationServices.StartupEventHandler/*0200000E*/) /* 0600004D */
    .addon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::add_Startup(class Microsoft.VisualBasic.ApplicationServices.StartupEventHandler/*0200000E*/) /* 0600004C */
  } // end of event WindowsFormsApplicationBase::Startup
  .event /*14000002*/ Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventHandler/*0200000F*/ StartupNextInstance
  {
    .addon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::add_StartupNextInstance(class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventHandler/*0200000F*/) /* 0600004E */
    .removeon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::remove_StartupNextInstance(class Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventHandler/*0200000F*/) /* 0600004F */
  } // end of event WindowsFormsApplicationBase::StartupNextInstance
  .event /*14000003*/ Microsoft.VisualBasic.ApplicationServices.ShutdownEventHandler/*02000010*/ Shutdown
  {
    .addon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::add_Shutdown(class Microsoft.VisualBasic.ApplicationServices.ShutdownEventHandler/*02000010*/) /* 06000050 */
    .removeon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::remove_Shutdown(class Microsoft.VisualBasic.ApplicationServices.ShutdownEventHandler/*02000010*/) /* 06000051 */
  } // end of event WindowsFormsApplicationBase::Shutdown
  .event /*14000004*/ Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/ NetworkAvailabilityChanged
  {
    .addon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::add_NetworkAvailabilityChanged(class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/) /* 06000052 */
    .removeon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::remove_NetworkAvailabilityChanged(class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/) /* 06000053 */
    .fire instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::raise_NetworkAvailabilityChanged(object,
                                                                                                                                            class Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs/*02000023*/) /* 06000054 */
  } // end of event WindowsFormsApplicationBase::NetworkAvailabilityChanged
  .event /*14000005*/ Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventHandler/*02000011*/ UnhandledException
  {
    .fire instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::raise_UnhandledException(object,
                                                                                                                                    class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs/*0200000B*/) /* 06000057 */
    .addon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::add_UnhandledException(class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventHandler/*02000011*/) /* 06000055 */
    .removeon instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::remove_UnhandledException(class Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventHandler/*02000011*/) /* 06000056 */
  } // end of event WindowsFormsApplicationBase::UnhandledException
  .property /*1700001E*/ instance callconv(8) class [System.Windows.Forms/*23000004*/]System.Windows.Forms.FormCollection/*01000015*/
          OpenForms()
  {
    .get instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.FormCollection/*01000015*/ Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_OpenForms() /* 0600005B */
  } // end of property WindowsFormsApplicationBase::OpenForms
  .property /*1700001F*/ instance callconv(8) class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/
          MainForm()
  {
    .get instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/ Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_MainForm() /* 0600005C */
    .set instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::set_MainForm(class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/) /* 0600005D */
  } // end of property WindowsFormsApplicationBase::MainForm
  .property /*17000020*/ instance callconv(8) class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/
          SplashScreen()
  {
    .get instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/ Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_SplashScreen() /* 0600005E */
    .set instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::set_SplashScreen(class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Form/*01000016*/) /* 0600005F */
  } // end of property WindowsFormsApplicationBase::SplashScreen
  .property /*17000021*/ instance callconv(8) int32
          MinimumSplashScreenDisplayTime()
  {
    .get instance int32 Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_MinimumSplashScreenDisplayTime() /* 06000060 */
    .set instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::set_MinimumSplashScreenDisplayTime(int32) /* 06000061 */
  } // end of property WindowsFormsApplicationBase::MinimumSplashScreenDisplayTime
  .property /*17000022*/ callconv(8) bool
          UseCompatibleTextRendering()
  {
    .custom /*0C000029:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get bool Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_UseCompatibleTextRendering() /* 06000062 */
  } // end of property WindowsFormsApplicationBase::UseCompatibleTextRendering
  .property /*17000023*/ instance callconv(8) class [System.Windows.Forms/*23000004*/]System.Windows.Forms.ApplicationContext/*01000017*/
          ApplicationContext()
  {
    .custom /*0C00002A:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.ApplicationContext/*01000017*/ Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_ApplicationContext() /* 06000063 */
  } // end of property WindowsFormsApplicationBase::ApplicationContext
  .property /*17000024*/ instance callconv(8) bool
          SaveMySettingsOnExit()
  {
    .get instance bool Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_SaveMySettingsOnExit() /* 06000064 */
    .set instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::set_SaveMySettingsOnExit(bool) /* 06000065 */
  } // end of property WindowsFormsApplicationBase::SaveMySettingsOnExit
  .property /*17000025*/ instance callconv(8) valuetype Microsoft.VisualBasic.ApplicationServices.ShutdownMode/*0200000A*/
          ShutdownStyle()
  {
    .set instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::set_ShutdownStyle(valuetype Microsoft.VisualBasic.ApplicationServices.ShutdownMode/*0200000A*/) /* 06000072 */
    .get instance valuetype Microsoft.VisualBasic.ApplicationServices.ShutdownMode/*0200000A*/ Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_ShutdownStyle() /* 06000071 */
  } // end of property WindowsFormsApplicationBase::ShutdownStyle
  .property /*17000026*/ instance callconv(8) bool
          EnableVisualStyles()
  {
    .get instance bool Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_EnableVisualStyles() /* 06000073 */
    .set instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::set_EnableVisualStyles(bool) /* 06000074 */
  } // end of property WindowsFormsApplicationBase::EnableVisualStyles
  .property /*17000027*/ instance callconv(8) bool
          IsSingleInstance()
  {
    .custom /*0C00002C:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .set instance void Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::set_IsSingleInstance(bool) /* 06000076 */
    .get instance bool Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase/*02000014*/::get_IsSingleInstance() /* 06000075 */
  } // end of property WindowsFormsApplicationBase::IsSingleInstance
} // end of class Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase

.class /*02000015*/ public auto ansi serializable sealed Microsoft.VisualBasic.Collection
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements [mscorlib/*23000001*/]System.Collections.ICollection/*0100001C*/,
                  [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/,
                  [mscorlib/*23000001*/]System.Runtime.Serialization.ISerializable/*01000018*/,
                  [mscorlib/*23000001*/]System.Runtime.Serialization.IDeserializationCallback/*01000019*/
{
  .custom /*0C000020:0A000029*/ instance void [mscorlib/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000058*/::.ctor(string) /* 0A000029 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .custom /*0C000021:0A00002A*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerDisplayAttribute/*01000059*/::.ctor(string) /* 0A00002A */ = ( 01 00 0F 43 6F 75 6E 74 20 3D 20 7B 43 6F 75 6E   // ...Count = {Coun
                                                                                                                                                              74 7D 00 00 )                                     // t}..
  .custom /*0C000022:0A00002B*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerTypeProxyAttribute/*0100005A*/::.ctor(class [mscorlib/*23000001*/]System.Type/*0100000B*/) /* 0A00002B */ = ( 01 00 34 4D 69 63 72 6F 73 6F 66 74 2E 56 69 73   // ..4Microsoft.Vis
                                                                                                                                                                                                             75 61 6C 42 61 73 69 63 2E 43 6F 6C 6C 65 63 74   // ualBasic.Collect
                                                                                                                                                                                                             69 6F 6E 2B 43 6F 6C 6C 65 63 74 69 6F 6E 44 65   // ion+CollectionDe
                                                                                                                                                                                                             62 75 67 56 69 65 77 00 00 )                      // bugView..
  .class /*02000016*/ auto ansi sealed nested assembly CollectionDebugView
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .method /*06000094*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x20d3
      // Code size       0 (0x0)
    } // end of method CollectionDebugView::.ctor

  } // end of class CollectionDebugView

  .method /*06000077*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b6
    // Code size       0 (0x0)
  } // end of method Collection::.ctor

  .method /*06000078*/ public instance void 
          Add(object Item,
              [opt] string Key,
              [opt] object Before,
              [opt] object After) cil managed
  // SIG: 20 04 01 1C 0E 1C 1C
  {
    .param [2]/*08000062*/  = nullref
    .param [3]/*08000063*/  = nullref
    .param [4]/*08000064*/  = nullref
    // Method begins at RVA 0x20b7
    // Code size       0 (0x0)
  } // end of method Collection::Add

  .method /*06000079*/ public instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b8
    // Code size       0 (0x0)
  } // end of method Collection::Clear

  .method /*0600007A*/ public instance bool 
          Contains(string Key) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x20b9
    // Code size       0 (0x0)
  } // end of method Collection::Contains

  .method /*0600007B*/ public hidebysig instance void 
          Remove(string Key) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20ba
    // Code size       0 (0x0)
  } // end of method Collection::Remove

  .method /*0600007C*/ public hidebysig instance void 
          Remove(int32 Index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20bb
    // Code size       0 (0x0)
  } // end of method Collection::Remove

  .method /*0600007D*/ public hidebysig specialname 
          instance object  get_Item(int32 Index) cil managed
  // SIG: 20 01 1C 08
  {
    // Method begins at RVA 0x20bc
    // Code size       0 (0x0)
  } // end of method Collection::get_Item

  .method /*0600007E*/ public hidebysig specialname 
          instance object  get_Item(string Key) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x20bd
    // Code size       0 (0x0)
  } // end of method Collection::get_Item

  .method /*0600007F*/ public hidebysig specialname 
          instance object  get_Item(object Index) cil managed
  // SIG: 20 01 1C 1C
  {
    // Method begins at RVA 0x20be
    // Code size       0 (0x0)
  } // end of method Collection::get_Item

  .method /*06000080*/ public specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20bf
    // Code size       0 (0x0)
  } // end of method Collection::get_Count

  .method /*06000081*/ public instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*0100001B*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x20c0
    // Code size       0 (0x0)
  } // end of method Collection::GetEnumerator

  .method /*06000082*/ private newslot strict virtual final 
          instance void  GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000013*/ info,
                                       valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000014*/ context) cil managed
  // SIG: 20 02 01 12 4D 11 51
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(128)}}
    .override [mscorlib/*23000001*/]System.Runtime.Serialization.ISerializable/*01000018*/::GetObjectData /*01000018::0A000001*/ 
    // Method begins at RVA 0x20c1
    // Code size       0 (0x0)
  } // end of method Collection::GetObjectData

  .method /*06000083*/ private newslot strict virtual final 
          instance void  OnDeserialization(object sender) cil managed
  // SIG: 20 01 01 1C
  {
    .override [mscorlib/*23000001*/]System.Runtime.Serialization.IDeserializationCallback/*01000019*/::OnDeserialization /*01000019::0A000002*/ 
    // Method begins at RVA 0x20c2
    // Code size       0 (0x0)
  } // end of method Collection::OnDeserialization

  .method /*06000084*/ private newslot strict virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*0100001B*/ 
          ICollectionGetEnumerator() cil managed
  // SIG: 20 00 12 6D
  {
    .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*0100001A*/::GetEnumerator /*0100001A::0A000003*/ 
    // Method begins at RVA 0x20c3
    // Code size       0 (0x0)
  } // end of method Collection::ICollectionGetEnumerator

  .method /*06000085*/ private newslot specialname strict virtual final 
          instance int32  get_ICollectionCount() cil managed
  // SIG: 20 00 08
  {
    .override [mscorlib/*23000001*/]System.Collections.ICollection/*0100001C*/::get_Count /*0100001C::0A000004*/ 
    // Method begins at RVA 0x20c4
    // Code size       0 (0x0)
  } // end of method Collection::get_ICollectionCount

  .method /*06000086*/ private newslot specialname strict virtual final 
          instance bool  get_ICollectionIsSynchronized() cil managed
  // SIG: 20 00 02
  {
    .override [mscorlib/*23000001*/]System.Collections.ICollection/*0100001C*/::get_IsSynchronized /*0100001C::0A000005*/ 
    // Method begins at RVA 0x20c5
    // Code size       0 (0x0)
  } // end of method Collection::get_ICollectionIsSynchronized

  .method /*06000087*/ private newslot specialname strict virtual final 
          instance object  get_ICollectionSyncRoot() cil managed
  // SIG: 20 00 1C
  {
    .override [mscorlib/*23000001*/]System.Collections.ICollection/*0100001C*/::get_SyncRoot /*0100001C::0A000006*/ 
    // Method begins at RVA 0x20c6
    // Code size       0 (0x0)
  } // end of method Collection::get_ICollectionSyncRoot

  .method /*06000088*/ private newslot specialname strict virtual final 
          instance bool  get_IListIsFixedSize() cil managed
  // SIG: 20 00 02
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::get_IsFixedSize /*0100001D::0A000007*/ 
    // Method begins at RVA 0x20c7
    // Code size       0 (0x0)
  } // end of method Collection::get_IListIsFixedSize

  .method /*06000089*/ private newslot specialname strict virtual final 
          instance bool  get_IListIsReadOnly() cil managed
  // SIG: 20 00 02
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::get_IsReadOnly /*0100001D::0A000008*/ 
    // Method begins at RVA 0x20c8
    // Code size       0 (0x0)
  } // end of method Collection::get_IListIsReadOnly

  .method /*0600008A*/ private newslot strict virtual final 
          instance void  ICollectionCopyTo(class [mscorlib/*23000001*/]System.Array/*0100001E*/ 'array',
                                           int32 index) cil managed
  // SIG: 20 02 01 12 79 08
  {
    .override [mscorlib/*23000001*/]System.Collections.ICollection/*0100001C*/::CopyTo /*0100001C::0A000009*/ 
    // Method begins at RVA 0x20c9
    // Code size       0 (0x0)
  } // end of method Collection::ICollectionCopyTo

  .method /*0600008B*/ private newslot strict virtual final 
          instance int32  IListAdd(object 'value') cil managed
  // SIG: 20 01 08 1C
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::Add /*0100001D::0A00000A*/ 
    // Method begins at RVA 0x20ca
    // Code size       0 (0x0)
  } // end of method Collection::IListAdd

  .method /*0600008C*/ private newslot strict virtual final 
          instance void  IListInsert(int32 index,
                                     object 'value') cil managed
  // SIG: 20 02 01 08 1C
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::Insert /*0100001D::0A00000B*/ 
    // Method begins at RVA 0x20cb
    // Code size       0 (0x0)
  } // end of method Collection::IListInsert

  .method /*0600008D*/ private newslot strict virtual final 
          instance void  IListRemoveAt(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::RemoveAt /*0100001D::0A00000C*/ 
    // Method begins at RVA 0x20cc
    // Code size       0 (0x0)
  } // end of method Collection::IListRemoveAt

  .method /*0600008E*/ private newslot strict virtual final 
          instance void  IListRemove(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::Remove /*0100001D::0A00000D*/ 
    // Method begins at RVA 0x20cd
    // Code size       0 (0x0)
  } // end of method Collection::IListRemove

  .method /*0600008F*/ private newslot strict virtual final 
          instance void  IListClear() cil managed
  // SIG: 20 00 01
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::Clear /*0100001D::0A00000E*/ 
    // Method begins at RVA 0x20ce
    // Code size       0 (0x0)
  } // end of method Collection::IListClear

  .method /*06000090*/ private newslot specialname strict virtual final 
          instance object  get_IListItem(int32 index) cil managed
  // SIG: 20 01 1C 08
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::get_Item /*0100001D::0A00000F*/ 
    // Method begins at RVA 0x20cf
    // Code size       0 (0x0)
  } // end of method Collection::get_IListItem

  .method /*06000091*/ private newslot specialname strict virtual final 
          instance void  set_IListItem(int32 index,
                                       object 'value') cil managed
  // SIG: 20 02 01 08 1C
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::set_Item /*0100001D::0A000010*/ 
    // Method begins at RVA 0x20d0
    // Code size       0 (0x0)
  } // end of method Collection::set_IListItem

  .method /*06000092*/ private newslot strict virtual final 
          instance bool  IListContains(object 'value') cil managed
  // SIG: 20 01 02 1C
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::Contains /*0100001D::0A000011*/ 
    // Method begins at RVA 0x20d1
    // Code size       0 (0x0)
  } // end of method Collection::IListContains

  .method /*06000093*/ private newslot strict virtual final 
          instance int32  IListIndexOf(object 'value') cil managed
  // SIG: 20 01 08 1C
  {
    .override [mscorlib/*23000001*/]System.Collections.IList/*0100001D*/::IndexOf /*0100001D::0A000012*/ 
    // Method begins at RVA 0x20d2
    // Code size       0 (0x0)
  } // end of method Collection::IListIndexOf

  .property /*17000028*/ instance callconv(8) object
          Item(int32)
  {
    .get instance object Microsoft.VisualBasic.Collection/*02000015*/::get_Item(int32) /* 0600007D */
  } // end of property Collection::Item
  .property /*17000029*/ instance callconv(8) object
          Item(string)
  {
    .get instance object Microsoft.VisualBasic.Collection/*02000015*/::get_Item(string) /* 0600007E */
  } // end of property Collection::Item
  .property /*1700002A*/ instance callconv(8) object
          Item(object)
  {
    .custom /*0C00002D:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get instance object Microsoft.VisualBasic.Collection/*02000015*/::get_Item(object) /* 0600007F */
  } // end of property Collection::Item
  .property /*1700002B*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.VisualBasic.Collection/*02000015*/::get_Count() /* 06000080 */
  } // end of property Collection::Count
  .property /*1700002C*/ instance callconv(8) int32
          ICollectionCount()
  {
    .get instance int32 Microsoft.VisualBasic.Collection/*02000015*/::get_ICollectionCount() /* 06000085 */
  } // end of property Collection::ICollectionCount
  .property /*1700002D*/ instance callconv(8) bool
          ICollectionIsSynchronized()
  {
    .get instance bool Microsoft.VisualBasic.Collection/*02000015*/::get_ICollectionIsSynchronized() /* 06000086 */
  } // end of property Collection::ICollectionIsSynchronized
  .property /*1700002E*/ instance callconv(8) object
          ICollectionSyncRoot()
  {
    .get instance object Microsoft.VisualBasic.Collection/*02000015*/::get_ICollectionSyncRoot() /* 06000087 */
  } // end of property Collection::ICollectionSyncRoot
  .property /*1700002F*/ instance callconv(8) bool
          IListIsFixedSize()
  {
    .get instance bool Microsoft.VisualBasic.Collection/*02000015*/::get_IListIsFixedSize() /* 06000088 */
  } // end of property Collection::IListIsFixedSize
  .property /*17000030*/ instance callconv(8) bool
          IListIsReadOnly()
  {
    .get instance bool Microsoft.VisualBasic.Collection/*02000015*/::get_IListIsReadOnly() /* 06000089 */
  } // end of property Collection::IListIsReadOnly
  .property /*17000031*/ instance callconv(8) object
          IListItem(int32)
  {
    .get instance object Microsoft.VisualBasic.Collection/*02000015*/::get_IListItem(int32) /* 06000090 */
    .set instance void Microsoft.VisualBasic.Collection/*02000015*/::set_IListItem(int32,
                                                                                   object) /* 06000091 */
  } // end of property Collection::IListItem
} // end of class Microsoft.VisualBasic.Collection

.class /*02000017*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.Constants
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000024:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .field /*04000011*/ public static literal int32 vbObjectError = int32(0x80040000)
  .field /*04000012*/ public static literal string vbCrLf = "\r\n"
  .field /*04000013*/ public static literal string vbNewLine = "\r\n"
  .field /*04000014*/ public static literal string vbCr = "\r"
  .field /*04000015*/ public static literal string vbLf = "\n"
  .field /*04000016*/ public static literal string vbBack = bytearray (08 00 ) 
  .field /*04000017*/ public static literal string vbFormFeed = bytearray (0C 00 ) 
  .field /*04000018*/ public static literal string vbTab = "\t"
  .field /*04000019*/ public static literal string vbVerticalTab = bytearray (0B 00 ) 
  .field /*0400001A*/ public static literal string vbNullChar = bytearray (00 00 ) 
  .field /*0400001B*/ public static literal string vbNullString = nullref
  .field /*0400001C*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ vbHide = int16(0x0000)
  .field /*0400001D*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ vbNormalFocus = int16(0x0001)
  .field /*0400001E*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ vbMinimizedFocus = int16(0x0002)
  .field /*0400001F*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ vbMaximizedFocus = int16(0x0003)
  .field /*04000020*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ vbNormalNoFocus = int16(0x0004)
  .field /*04000021*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ vbMinimizedNoFocus = int16(0x0006)
  .field /*04000022*/ public static literal valuetype Microsoft.VisualBasic.CallType/*02000037*/ vbMethod = int32(0x00000001)
  .field /*04000023*/ public static literal valuetype Microsoft.VisualBasic.CallType/*02000037*/ vbGet = int32(0x00000002)
  .field /*04000024*/ public static literal valuetype Microsoft.VisualBasic.CallType/*02000037*/ vbLet = int32(0x00000004)
  .field /*04000025*/ public static literal valuetype Microsoft.VisualBasic.CallType/*02000037*/ vbSet = int32(0x00000008)
  .field /*04000026*/ public static literal valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ vbBinaryCompare = int32(0x00000000)
  .field /*04000027*/ public static literal valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ vbTextCompare = int32(0x00000001)
  .field /*04000028*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ vbGeneralDate = int32(0x00000000)
  .field /*04000029*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ vbLongDate = int32(0x00000001)
  .field /*0400002A*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ vbShortDate = int32(0x00000002)
  .field /*0400002B*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ vbLongTime = int32(0x00000003)
  .field /*0400002C*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ vbShortTime = int32(0x00000004)
  .field /*0400002D*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ vbUseSystemDayOfWeek = int32(0x00000000)
  .field /*0400002E*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ vbSunday = int32(0x00000001)
  .field /*0400002F*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ vbMonday = int32(0x00000002)
  .field /*04000030*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ vbTuesday = int32(0x00000003)
  .field /*04000031*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ vbWednesday = int32(0x00000004)
  .field /*04000032*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ vbThursday = int32(0x00000005)
  .field /*04000033*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ vbFriday = int32(0x00000006)
  .field /*04000034*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ vbSaturday = int32(0x00000007)
  .field /*04000035*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ vbNormal = int32(0x00000000)
  .field /*04000036*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ vbReadOnly = int32(0x00000001)
  .field /*04000037*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ vbHidden = int32(0x00000002)
  .field /*04000038*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ vbSystem = int32(0x00000004)
  .field /*04000039*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ vbVolume = int32(0x00000008)
  .field /*0400003A*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ vbDirectory = int32(0x00000010)
  .field /*0400003B*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ vbArchive = int32(0x00000020)
  .field /*0400003C*/ public static literal valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ vbUseSystem = int32(0x00000000)
  .field /*0400003D*/ public static literal valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ vbFirstJan1 = int32(0x00000001)
  .field /*0400003E*/ public static literal valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ vbFirstFourDays = int32(0x00000002)
  .field /*0400003F*/ public static literal valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ vbFirstFullWeek = int32(0x00000003)
  .field /*04000040*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbUpperCase = int32(0x00000001)
  .field /*04000041*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbLowerCase = int32(0x00000002)
  .field /*04000042*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbProperCase = int32(0x00000003)
  .field /*04000043*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbWide = int32(0x00000004)
  .field /*04000044*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbNarrow = int32(0x00000008)
  .field /*04000045*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbKatakana = int32(0x00000010)
  .field /*04000046*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbHiragana = int32(0x00000020)
  .field /*04000047*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbSimplifiedChinese = int32(0x00000100)
  .field /*04000048*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbTraditionalChinese = int32(0x00000200)
  .field /*04000049*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ vbLinguisticCasing = int32(0x00000400)
  .field /*0400004A*/ public static literal valuetype Microsoft.VisualBasic.TriState/*0200003E*/ vbUseDefault = int32(0xFFFFFFFE)
  .field /*0400004B*/ public static literal valuetype Microsoft.VisualBasic.TriState/*0200003E*/ vbTrue = int32(0xFFFFFFFF)
  .field /*0400004C*/ public static literal valuetype Microsoft.VisualBasic.TriState/*0200003E*/ vbFalse = int32(0x00000000)
  .field /*0400004D*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbEmpty = int32(0x00000000)
  .field /*0400004E*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbNull = int32(0x00000001)
  .field /*0400004F*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbInteger = int32(0x00000003)
  .field /*04000050*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbLong = int32(0x00000014)
  .field /*04000051*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbSingle = int32(0x00000004)
  .field /*04000052*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbDouble = int32(0x00000005)
  .field /*04000053*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbCurrency = int32(0x00000006)
  .field /*04000054*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbDate = int32(0x00000007)
  .field /*04000055*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbString = int32(0x00000008)
  .field /*04000056*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbObject = int32(0x00000009)
  .field /*04000057*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbBoolean = int32(0x0000000B)
  .field /*04000058*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbVariant = int32(0x0000000C)
  .field /*04000059*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbDecimal = int32(0x0000000E)
  .field /*0400005A*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbByte = int32(0x00000011)
  .field /*0400005B*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbUserDefinedType = int32(0x00000024)
  .field /*0400005C*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ vbArray = int32(0x00002000)
  .field /*0400005D*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ vbOK = int32(0x00000001)
  .field /*0400005E*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ vbCancel = int32(0x00000002)
  .field /*0400005F*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ vbAbort = int32(0x00000003)
  .field /*04000060*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ vbRetry = int32(0x00000004)
  .field /*04000061*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ vbIgnore = int32(0x00000005)
  .field /*04000062*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ vbYes = int32(0x00000006)
  .field /*04000063*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ vbNo = int32(0x00000007)
  .field /*04000064*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbOKOnly = int32(0x00000000)
  .field /*04000065*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbOKCancel = int32(0x00000001)
  .field /*04000066*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbAbortRetryIgnore = int32(0x00000002)
  .field /*04000067*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbYesNoCancel = int32(0x00000003)
  .field /*04000068*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbYesNo = int32(0x00000004)
  .field /*04000069*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbRetryCancel = int32(0x00000005)
  .field /*0400006A*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbCritical = int32(0x00000010)
  .field /*0400006B*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbQuestion = int32(0x00000020)
  .field /*0400006C*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbExclamation = int32(0x00000030)
  .field /*0400006D*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbInformation = int32(0x00000040)
  .field /*0400006E*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbDefaultButton1 = int32(0x00000000)
  .field /*0400006F*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbDefaultButton2 = int32(0x00000100)
  .field /*04000070*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbDefaultButton3 = int32(0x00000200)
  .field /*04000071*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbApplicationModal = int32(0x00000000)
  .field /*04000072*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbSystemModal = int32(0x00001000)
  .field /*04000073*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbMsgBoxHelp = int32(0x00004000)
  .field /*04000074*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbMsgBoxRight = int32(0x00080000)
  .field /*04000075*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbMsgBoxRtlReading = int32(0x00100000)
  .field /*04000076*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ vbMsgBoxSetForeground = int32(0x00010000)
  .method /*06000095*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20d4
    // Code size       0 (0x0)
  } // end of method Constants::.cctor

  .method /*06000096*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d5
    // Code size       0 (0x0)
  } // end of method Constants::.ctor

} // end of class Microsoft.VisualBasic.Constants

.class /*02000018*/ public auto ansi sealed Microsoft.VisualBasic.ControlChars
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000077*/ public static literal string CrLf = "\r\n"
  .field /*04000078*/ public static literal string NewLine = "\r\n"
  .field /*04000079*/ public static literal char Cr = char(0x000D)
  .field /*0400007A*/ public static literal char Lf = char(0x000A)
  .field /*0400007B*/ public static literal char Back = char(0x0008)
  .field /*0400007C*/ public static literal char FormFeed = char(0x000C)
  .field /*0400007D*/ public static literal char Tab = char(0x0009)
  .field /*0400007E*/ public static literal char VerticalTab = char(0x000B)
  .field /*0400007F*/ public static literal char NullChar = char(0x0000)
  .field /*04000080*/ public static literal char Quote = char(0x0022)
  .method /*06000097*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d6
    // Code size       0 (0x0)
  } // end of method ControlChars::.ctor

} // end of class Microsoft.VisualBasic.ControlChars

.class /*02000019*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.Conversion
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000025:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .method /*06000098*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20d7
    // Code size       0 (0x0)
  } // end of method Conversion::.cctor

  .method /*06000099*/ public static string 
          ErrorToString() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x20d8
    // Code size       0 (0x0)
  } // end of method Conversion::ErrorToString

  .method /*0600009A*/ public static string 
          ErrorToString(int32 ErrorNumber) cil managed
  // SIG: 00 01 0E 08
  {
    // Method begins at RVA 0x20d9
    // Code size       0 (0x0)
  } // end of method Conversion::ErrorToString

  .method /*0600009B*/ public static int16 
          Fix(int16 Number) cil managed
  // SIG: 00 01 06 06
  {
    // Method begins at RVA 0x20da
    // Code size       0 (0x0)
  } // end of method Conversion::Fix

  .method /*0600009C*/ public static int32 
          Fix(int32 Number) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x20db
    // Code size       0 (0x0)
  } // end of method Conversion::Fix

  .method /*0600009D*/ public static int64 
          Fix(int64 Number) cil managed
  // SIG: 00 01 0A 0A
  {
    // Method begins at RVA 0x20dc
    // Code size       0 (0x0)
  } // end of method Conversion::Fix

  .method /*0600009E*/ public static float64 
          Fix(float64 Number) cil managed
  // SIG: 00 01 0D 0D
  {
    // Method begins at RVA 0x20dd
    // Code size       0 (0x0)
  } // end of method Conversion::Fix

  .method /*0600009F*/ public static float32 
          Fix(float32 Number) cil managed
  // SIG: 00 01 0C 0C
  {
    // Method begins at RVA 0x20de
    // Code size       0 (0x0)
  } // end of method Conversion::Fix

  .method /*060000A0*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          Fix(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ Number) cil managed
  // SIG: 00 01 11 7D 11 7D
  {
    // Method begins at RVA 0x20df
    // Code size       0 (0x0)
  } // end of method Conversion::Fix

  .method /*060000A1*/ public static object 
          Fix(object Number) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x20e0
    // Code size       0 (0x0)
  } // end of method Conversion::Fix

  .method /*060000A2*/ public static int16 
          Int(int16 Number) cil managed
  // SIG: 00 01 06 06
  {
    // Method begins at RVA 0x20e1
    // Code size       0 (0x0)
  } // end of method Conversion::Int

  .method /*060000A3*/ public static int32 
          Int(int32 Number) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x20e2
    // Code size       0 (0x0)
  } // end of method Conversion::Int

  .method /*060000A4*/ public static int64 
          Int(int64 Number) cil managed
  // SIG: 00 01 0A 0A
  {
    // Method begins at RVA 0x20e3
    // Code size       0 (0x0)
  } // end of method Conversion::Int

  .method /*060000A5*/ public static float64 
          Int(float64 Number) cil managed
  // SIG: 00 01 0D 0D
  {
    // Method begins at RVA 0x20e4
    // Code size       0 (0x0)
  } // end of method Conversion::Int

  .method /*060000A6*/ public static float32 
          Int(float32 Number) cil managed
  // SIG: 00 01 0C 0C
  {
    // Method begins at RVA 0x20e5
    // Code size       0 (0x0)
  } // end of method Conversion::Int

  .method /*060000A7*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          Int(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ Number) cil managed
  // SIG: 00 01 11 7D 11 7D
  {
    // Method begins at RVA 0x20e6
    // Code size       0 (0x0)
  } // end of method Conversion::Int

  .method /*060000A8*/ public static object 
          Int(object Number) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x20e7
    // Code size       0 (0x0)
  } // end of method Conversion::Int

  .method /*060000A9*/ public static string 
          Hex(int8 Number) cil managed
  // SIG: 00 01 0E 04
  {
    .custom /*0C000082:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20e8
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000AA*/ public static string 
          Hex(uint8 Number) cil managed
  // SIG: 00 01 0E 05
  {
    // Method begins at RVA 0x20e9
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000AB*/ public static string 
          Hex(int16 Number) cil managed
  // SIG: 00 01 0E 06
  {
    // Method begins at RVA 0x20ea
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000AC*/ public static string 
          Hex(uint16 Number) cil managed
  // SIG: 00 01 0E 07
  {
    .custom /*0C000083:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20eb
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000AD*/ public static string 
          Hex(int32 Number) cil managed
  // SIG: 00 01 0E 08
  {
    // Method begins at RVA 0x20ec
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000AE*/ public static string 
          Hex(uint32 Number) cil managed
  // SIG: 00 01 0E 09
  {
    .custom /*0C000084:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20ed
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000AF*/ public static string 
          Hex(int64 Number) cil managed
  // SIG: 00 01 0E 0A
  {
    // Method begins at RVA 0x20ee
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000B0*/ public static string 
          Hex(uint64 Number) cil managed
  // SIG: 00 01 0E 0B
  {
    .custom /*0C000085:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000B1*/ public static string 
          Hex(object Number) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x20f0
    // Code size       0 (0x0)
  } // end of method Conversion::Hex

  .method /*060000B2*/ public static string 
          Oct(int8 Number) cil managed
  // SIG: 00 01 0E 04
  {
    .custom /*0C000086:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f1
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000B3*/ public static string 
          Oct(uint8 Number) cil managed
  // SIG: 00 01 0E 05
  {
    // Method begins at RVA 0x20f2
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000B4*/ public static string 
          Oct(int16 Number) cil managed
  // SIG: 00 01 0E 06
  {
    // Method begins at RVA 0x20f3
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000B5*/ public static string 
          Oct(uint16 Number) cil managed
  // SIG: 00 01 0E 07
  {
    .custom /*0C000087:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f4
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000B6*/ public static string 
          Oct(int32 Number) cil managed
  // SIG: 00 01 0E 08
  {
    // Method begins at RVA 0x20f5
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000B7*/ public static string 
          Oct(uint32 Number) cil managed
  // SIG: 00 01 0E 09
  {
    .custom /*0C000088:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f6
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000B8*/ public static string 
          Oct(int64 Number) cil managed
  // SIG: 00 01 0E 0A
  {
    // Method begins at RVA 0x20f7
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000B9*/ public static string 
          Oct(uint64 Number) cil managed
  // SIG: 00 01 0E 0B
  {
    .custom /*0C000089:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000BA*/ public static string 
          Oct(object Number) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x20f9
    // Code size       0 (0x0)
  } // end of method Conversion::Oct

  .method /*060000BB*/ public static string 
          Str(object Number) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x20fa
    // Code size       0 (0x0)
  } // end of method Conversion::Str

  .method /*060000BC*/ public static float64 
          Val(string InputStr) cil managed
  // SIG: 00 01 0D 0E
  {
    // Method begins at RVA 0x20fb
    // Code size       0 (0x0)
  } // end of method Conversion::Val

  .method /*060000BD*/ public static int32 
          Val(char Expression) cil managed
  // SIG: 00 01 08 03
  {
    // Method begins at RVA 0x20fc
    // Code size       0 (0x0)
  } // end of method Conversion::Val

  .method /*060000BE*/ public static float64 
          Val(object Expression) cil managed
  // SIG: 00 01 0D 1C
  {
    // Method begins at RVA 0x20fd
    // Code size       0 (0x0)
  } // end of method Conversion::Val

  .method /*060000BF*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20fe
    // Code size       0 (0x0)
  } // end of method Conversion::.ctor

} // end of class Microsoft.VisualBasic.Conversion

.class /*0200001A*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.DateAndTime
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000026:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .method /*060000C0*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20ff
    // Code size       0 (0x0)
  } // end of method DateAndTime::.cctor

  .method /*060000C1*/ public specialname static 
          valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          get_Today() cil managed
  // SIG: 00 00 11 80 81
  {
    // Method begins at RVA 0x2100
    // Code size       0 (0x0)
  } // end of method DateAndTime::get_Today

  .method /*060000C2*/ public specialname static 
          void  set_Today(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Value) cil managed
  // SIG: 00 01 01 11 80 81
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2101
    // Code size       0 (0x0)
  } // end of method DateAndTime::set_Today

  .method /*060000C3*/ public specialname static 
          valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          get_Now() cil managed
  // SIG: 00 00 11 80 81
  {
    // Method begins at RVA 0x2102
    // Code size       0 (0x0)
  } // end of method DateAndTime::get_Now

  .method /*060000C4*/ public specialname static 
          valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          get_TimeOfDay() cil managed
  // SIG: 00 00 11 80 81
  {
    // Method begins at RVA 0x2103
    // Code size       0 (0x0)
  } // end of method DateAndTime::get_TimeOfDay

  .method /*060000C5*/ public specialname static 
          void  set_TimeOfDay(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Value) cil managed
  // SIG: 00 01 01 11 80 81
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2104
    // Code size       0 (0x0)
  } // end of method DateAndTime::set_TimeOfDay

  .method /*060000C6*/ public specialname static 
          string  get_TimeString() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2105
    // Code size       0 (0x0)
  } // end of method DateAndTime::get_TimeString

  .method /*060000C7*/ public specialname static 
          void  set_TimeString(string Value) cil managed
  // SIG: 00 01 01 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2106
    // Code size       0 (0x0)
  } // end of method DateAndTime::set_TimeString

  .method /*060000C8*/ public specialname static 
          string  get_DateString() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2107
    // Code size       0 (0x0)
  } // end of method DateAndTime::get_DateString

  .method /*060000C9*/ public specialname static 
          void  set_DateString(string Value) cil managed
  // SIG: 00 01 01 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2108
    // Code size       0 (0x0)
  } // end of method DateAndTime::set_DateString

  .method /*060000CA*/ public specialname static 
          float64  get_Timer() cil managed
  // SIG: 00 00 0D
  {
    // Method begins at RVA 0x2109
    // Code size       0 (0x0)
  } // end of method DateAndTime::get_Timer

  .method /*060000CB*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          DateAdd(valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Interval,
                  float64 Number,
                  valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ DateValue) cil managed
  // SIG: 00 03 11 80 81 11 80 FC 0D 11 80 81
  {
    // Method begins at RVA 0x210a
    // Code size       0 (0x0)
  } // end of method DateAndTime::DateAdd

  .method /*060000CC*/ public static int64 
          DateDiff(valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Interval,
                   valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Date1,
                   valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Date2,
                   [opt] valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ DayOfWeek,
                   [opt] valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ WeekOfYear) cil managed
  // SIG: 00 05 0A 11 80 FC 11 80 81 11 80 81 11 80 E8 11 80 F0
  {
    .param [4]/*080000A9*/  = int32(0x00000001)
    .param [5]/*080000AA*/  = int32(0x00000001)
    // Method begins at RVA 0x210b
    // Code size       0 (0x0)
  } // end of method DateAndTime::DateDiff

  .method /*060000CD*/ public static int32 
          DatePart(valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Interval,
                   valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ DateValue,
                   [opt] valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ FirstDayOfWeekValue,
                   [opt] valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ FirstWeekOfYearValue) cil managed
  // SIG: 00 04 08 11 80 FC 11 80 81 11 80 E8 11 80 F0
  {
    .param [3]/*080000AD*/  = int32(0x00000001)
    .param [4]/*080000AE*/  = int32(0x00000001)
    // Method begins at RVA 0x210c
    // Code size       0 (0x0)
  } // end of method DateAndTime::DatePart

  .method /*060000CE*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          DateAdd(string Interval,
                  float64 Number,
                  object DateValue) cil managed
  // SIG: 00 03 11 80 81 0E 0D 1C
  {
    // Method begins at RVA 0x210d
    // Code size       0 (0x0)
  } // end of method DateAndTime::DateAdd

  .method /*060000CF*/ public static int64 
          DateDiff(string Interval,
                   object Date1,
                   object Date2,
                   [opt] valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ DayOfWeek,
                   [opt] valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ WeekOfYear) cil managed
  // SIG: 00 05 0A 0E 1C 1C 11 80 E8 11 80 F0
  {
    .param [4]/*080000B5*/  = int32(0x00000001)
    .param [5]/*080000B6*/  = int32(0x00000001)
    // Method begins at RVA 0x210e
    // Code size       0 (0x0)
  } // end of method DateAndTime::DateDiff

  .method /*060000D0*/ public static int32 
          DatePart(string Interval,
                   object DateValue,
                   [opt] valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ DayOfWeek,
                   [opt] valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ WeekOfYear) cil managed
  // SIG: 00 04 08 0E 1C 11 80 E8 11 80 F0
  {
    .param [3]/*080000B9*/  = int32(0x00000001)
    .param [4]/*080000BA*/  = int32(0x00000001)
    // Method begins at RVA 0x210f
    // Code size       0 (0x0)
  } // end of method DateAndTime::DatePart

  .method /*060000D1*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          DateSerial(int32 Year,
                     int32 Month,
                     int32 Day) cil managed
  // SIG: 00 03 11 80 81 08 08 08
  {
    // Method begins at RVA 0x2110
    // Code size       0 (0x0)
  } // end of method DateAndTime::DateSerial

  .method /*060000D2*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          TimeSerial(int32 Hour,
                     int32 Minute,
                     int32 Second) cil managed
  // SIG: 00 03 11 80 81 08 08 08
  {
    // Method begins at RVA 0x2111
    // Code size       0 (0x0)
  } // end of method DateAndTime::TimeSerial

  .method /*060000D3*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          DateValue(string StringDate) cil managed
  // SIG: 00 01 11 80 81 0E
  {
    // Method begins at RVA 0x2112
    // Code size       0 (0x0)
  } // end of method DateAndTime::DateValue

  .method /*060000D4*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          TimeValue(string StringTime) cil managed
  // SIG: 00 01 11 80 81 0E
  {
    // Method begins at RVA 0x2113
    // Code size       0 (0x0)
  } // end of method DateAndTime::TimeValue

  .method /*060000D5*/ public static int32 
          Year(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ DateValue) cil managed
  // SIG: 00 01 08 11 80 81
  {
    // Method begins at RVA 0x2114
    // Code size       0 (0x0)
  } // end of method DateAndTime::Year

  .method /*060000D6*/ public static int32 
          Month(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ DateValue) cil managed
  // SIG: 00 01 08 11 80 81
  {
    // Method begins at RVA 0x2115
    // Code size       0 (0x0)
  } // end of method DateAndTime::Month

  .method /*060000D7*/ public static int32 
          Day(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ DateValue) cil managed
  // SIG: 00 01 08 11 80 81
  {
    // Method begins at RVA 0x2116
    // Code size       0 (0x0)
  } // end of method DateAndTime::Day

  .method /*060000D8*/ public static int32 
          Hour(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ TimeValue) cil managed
  // SIG: 00 01 08 11 80 81
  {
    // Method begins at RVA 0x2117
    // Code size       0 (0x0)
  } // end of method DateAndTime::Hour

  .method /*060000D9*/ public static int32 
          Minute(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ TimeValue) cil managed
  // SIG: 00 01 08 11 80 81
  {
    // Method begins at RVA 0x2118
    // Code size       0 (0x0)
  } // end of method DateAndTime::Minute

  .method /*060000DA*/ public static int32 
          Second(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ TimeValue) cil managed
  // SIG: 00 01 08 11 80 81
  {
    // Method begins at RVA 0x2119
    // Code size       0 (0x0)
  } // end of method DateAndTime::Second

  .method /*060000DB*/ public static int32 
          Weekday(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ DateValue,
                  [opt] valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ DayOfWeek) cil managed
  // SIG: 00 02 08 11 80 81 11 80 E8
  {
    .param [2]/*080000CA*/  = int32(0x00000001)
    // Method begins at RVA 0x211a
    // Code size       0 (0x0)
  } // end of method DateAndTime::Weekday

  .method /*060000DC*/ public static string 
          MonthName(int32 Month,
                    [opt] bool Abbreviate) cil managed
  // SIG: 00 02 0E 08 02
  {
    .param [2]/*080000CC*/  = bool(false)
    // Method begins at RVA 0x211b
    // Code size       0 (0x0)
  } // end of method DateAndTime::MonthName

  .method /*060000DD*/ public static string 
          WeekdayName(int32 Weekday,
                      [opt] bool Abbreviate,
                      [opt] valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ FirstDayOfWeekValue) cil managed
  // SIG: 00 03 0E 08 02 11 80 E8
  {
    .param [2]/*080000CE*/  = bool(false)
    .param [3]/*080000CF*/  = int32(0x00000000)
    // Method begins at RVA 0x211c
    // Code size       0 (0x0)
  } // end of method DateAndTime::WeekdayName

  .method /*060000DE*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x211d
    // Code size       0 (0x0)
  } // end of method DateAndTime::.ctor

  .property /*17000032*/ callconv(8) valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/
          Today()
  {
    .get valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Microsoft.VisualBasic.DateAndTime/*0200001A*/::get_Today() /* 060000C1 */
    .set void Microsoft.VisualBasic.DateAndTime/*0200001A*/::set_Today(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/) /* 060000C2 */
  } // end of property DateAndTime::Today
  .property /*17000033*/ callconv(8) valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/
          Now()
  {
    .get valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Microsoft.VisualBasic.DateAndTime/*0200001A*/::get_Now() /* 060000C3 */
  } // end of property DateAndTime::Now
  .property /*17000034*/ callconv(8) valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/
          TimeOfDay()
  {
    .set void Microsoft.VisualBasic.DateAndTime/*0200001A*/::set_TimeOfDay(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/) /* 060000C5 */
    .get valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Microsoft.VisualBasic.DateAndTime/*0200001A*/::get_TimeOfDay() /* 060000C4 */
  } // end of property DateAndTime::TimeOfDay
  .property /*17000035*/ callconv(8) string
          TimeString()
  {
    .set void Microsoft.VisualBasic.DateAndTime/*0200001A*/::set_TimeString(string) /* 060000C7 */
    .get string Microsoft.VisualBasic.DateAndTime/*0200001A*/::get_TimeString() /* 060000C6 */
  } // end of property DateAndTime::TimeString
  .property /*17000036*/ callconv(8) string
          DateString()
  {
    .set void Microsoft.VisualBasic.DateAndTime/*0200001A*/::set_DateString(string) /* 060000C9 */
    .get string Microsoft.VisualBasic.DateAndTime/*0200001A*/::get_DateString() /* 060000C8 */
  } // end of property DateAndTime::DateString
  .property /*17000037*/ callconv(8) float64
          Timer()
  {
    .get float64 Microsoft.VisualBasic.DateAndTime/*0200001A*/::get_Timer() /* 060000CA */
  } // end of property DateAndTime::Timer
} // end of class Microsoft.VisualBasic.DateAndTime

.class /*0200001B*/ public auto ansi sealed Microsoft.VisualBasic.AudioPlayMode
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000081*/ public specialname rtspecialname int32 value__
  .field /*04000082*/ public static literal valuetype Microsoft.VisualBasic.AudioPlayMode/*0200001B*/ WaitToComplete = int32(0x00000000)
  .field /*04000083*/ public static literal valuetype Microsoft.VisualBasic.AudioPlayMode/*0200001B*/ Background = int32(0x00000001)
  .field /*04000084*/ public static literal valuetype Microsoft.VisualBasic.AudioPlayMode/*0200001B*/ BackgroundLoop = int32(0x00000002)
} // end of class Microsoft.VisualBasic.AudioPlayMode

.class /*0200001C*/ public auto ansi Microsoft.VisualBasic.Devices.Audio
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*060000DF*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x211e
    // Code size       0 (0x0)
  } // end of method Audio::.ctor

  .method /*060000E0*/ public instance void 
          Play(string location) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x211f
    // Code size       0 (0x0)
  } // end of method Audio::Play

  .method /*060000E1*/ public instance void 
          Play(string location,
               valuetype Microsoft.VisualBasic.AudioPlayMode/*0200001B*/ playMode) cil managed
  // SIG: 20 02 01 0E 11 6C
  {
    // Method begins at RVA 0x2120
    // Code size       0 (0x0)
  } // end of method Audio::Play

  .method /*060000E2*/ public instance void 
          Play(uint8[] data,
               valuetype Microsoft.VisualBasic.AudioPlayMode/*0200001B*/ playMode) cil managed
  // SIG: 20 02 01 1D 05 11 6C
  {
    // Method begins at RVA 0x2121
    // Code size       0 (0x0)
  } // end of method Audio::Play

  .method /*060000E3*/ public instance void 
          Play(class [mscorlib/*23000001*/]System.IO.Stream/*01000021*/ 'stream',
               valuetype Microsoft.VisualBasic.AudioPlayMode/*0200001B*/ playMode) cil managed
  // SIG: 20 02 01 12 80 85 11 6C
  {
    // Method begins at RVA 0x2122
    // Code size       0 (0x0)
  } // end of method Audio::Play

  .method /*060000E4*/ public instance void 
          PlaySystemSound(class [System/*23000003*/]System.Media.SystemSound/*01000022*/ systemSound) cil managed
  // SIG: 20 01 01 12 80 89
  {
    // Method begins at RVA 0x2123
    // Code size       0 (0x0)
  } // end of method Audio::PlaySystemSound

  .method /*060000E5*/ public instance void 
          Stop() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2124
    // Code size       0 (0x0)
  } // end of method Audio::Stop

} // end of class Microsoft.VisualBasic.Devices.Audio

.class /*0200001D*/ public auto ansi Microsoft.VisualBasic.Devices.Clock
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*060000E6*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2125
    // Code size       0 (0x0)
  } // end of method Clock::.ctor

  .method /*060000E7*/ public specialname 
          instance valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          get_LocalTime() cil managed
  // SIG: 20 00 11 80 81
  {
    // Method begins at RVA 0x2126
    // Code size       0 (0x0)
  } // end of method Clock::get_LocalTime

  .method /*060000E8*/ public specialname 
          instance valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          get_GmtTime() cil managed
  // SIG: 20 00 11 80 81
  {
    // Method begins at RVA 0x2127
    // Code size       0 (0x0)
  } // end of method Clock::get_GmtTime

  .method /*060000E9*/ public specialname 
          instance int32  get_TickCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2128
    // Code size       0 (0x0)
  } // end of method Clock::get_TickCount

  .property /*17000038*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/
          LocalTime()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Microsoft.VisualBasic.Devices.Clock/*0200001D*/::get_LocalTime() /* 060000E7 */
  } // end of property Clock::LocalTime
  .property /*17000039*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/
          GmtTime()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Microsoft.VisualBasic.Devices.Clock/*0200001D*/::get_GmtTime() /* 060000E8 */
  } // end of property Clock::GmtTime
  .property /*1700003A*/ instance callconv(8) int32
          TickCount()
  {
    .get instance int32 Microsoft.VisualBasic.Devices.Clock/*0200001D*/::get_TickCount() /* 060000E9 */
  } // end of property Clock::TickCount
} // end of class Microsoft.VisualBasic.Devices.Clock

.class /*0200001E*/ public auto ansi Microsoft.VisualBasic.Devices.Computer
       extends Microsoft.VisualBasic.Devices.ServerComputer/*02000027*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*060000EA*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2129
    // Code size       0 (0x0)
  } // end of method Computer::.ctor

  .method /*060000EB*/ public specialname 
          instance class Microsoft.VisualBasic.Devices.Audio/*0200001C*/ 
          get_Audio() cil managed
  // SIG: 20 00 12 70
  {
    // Method begins at RVA 0x212a
    // Code size       0 (0x0)
  } // end of method Computer::get_Audio

  .method /*060000EC*/ public specialname 
          instance class Microsoft.VisualBasic.MyServices.ClipboardProxy/*02000079*/ 
          get_Clipboard() cil managed
  // SIG: 20 00 12 81 E4
  {
    // Method begins at RVA 0x212b
    // Code size       0 (0x0)
  } // end of method Computer::get_Clipboard

  .method /*060000ED*/ public specialname 
          instance class Microsoft.VisualBasic.Devices.Ports/*02000026*/ 
          get_Ports() cil managed
  // SIG: 20 00 12 80 98
  {
    // Method begins at RVA 0x212c
    // Code size       0 (0x0)
  } // end of method Computer::get_Ports

  .method /*060000EE*/ public specialname 
          instance class Microsoft.VisualBasic.Devices.Mouse/*02000022*/ 
          get_Mouse() cil managed
  // SIG: 20 00 12 80 88
  {
    // Method begins at RVA 0x212d
    // Code size       0 (0x0)
  } // end of method Computer::get_Mouse

  .method /*060000EF*/ public specialname 
          instance class Microsoft.VisualBasic.Devices.Keyboard/*02000021*/ 
          get_Keyboard() cil managed
  // SIG: 20 00 12 80 84
  {
    // Method begins at RVA 0x212e
    // Code size       0 (0x0)
  } // end of method Computer::get_Keyboard

  .method /*060000F0*/ public specialname 
          instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Screen/*01000023*/ 
          get_Screen() cil managed
  // SIG: 20 00 12 80 8D
  {
    // Method begins at RVA 0x212f
    // Code size       0 (0x0)
  } // end of method Computer::get_Screen

  .property /*1700003B*/ instance callconv(8) class Microsoft.VisualBasic.Devices.Audio/*0200001C*/
          Audio()
  {
    .get instance class Microsoft.VisualBasic.Devices.Audio/*0200001C*/ Microsoft.VisualBasic.Devices.Computer/*0200001E*/::get_Audio() /* 060000EB */
  } // end of property Computer::Audio
  .property /*1700003C*/ instance callconv(8) class Microsoft.VisualBasic.MyServices.ClipboardProxy/*02000079*/
          Clipboard()
  {
    .get instance class Microsoft.VisualBasic.MyServices.ClipboardProxy/*02000079*/ Microsoft.VisualBasic.Devices.Computer/*0200001E*/::get_Clipboard() /* 060000EC */
  } // end of property Computer::Clipboard
  .property /*1700003D*/ instance callconv(8) class Microsoft.VisualBasic.Devices.Ports/*02000026*/
          Ports()
  {
    .get instance class Microsoft.VisualBasic.Devices.Ports/*02000026*/ Microsoft.VisualBasic.Devices.Computer/*0200001E*/::get_Ports() /* 060000ED */
  } // end of property Computer::Ports
  .property /*1700003E*/ instance callconv(8) class Microsoft.VisualBasic.Devices.Mouse/*02000022*/
          Mouse()
  {
    .get instance class Microsoft.VisualBasic.Devices.Mouse/*02000022*/ Microsoft.VisualBasic.Devices.Computer/*0200001E*/::get_Mouse() /* 060000EE */
  } // end of property Computer::Mouse
  .property /*1700003F*/ instance callconv(8) class Microsoft.VisualBasic.Devices.Keyboard/*02000021*/
          Keyboard()
  {
    .get instance class Microsoft.VisualBasic.Devices.Keyboard/*02000021*/ Microsoft.VisualBasic.Devices.Computer/*0200001E*/::get_Keyboard() /* 060000EF */
  } // end of property Computer::Keyboard
  .property /*17000040*/ instance callconv(8) class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Screen/*01000023*/
          Screen()
  {
    .get instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.Screen/*01000023*/ Microsoft.VisualBasic.Devices.Computer/*0200001E*/::get_Screen() /* 060000F0 */
  } // end of property Computer::Screen
} // end of class Microsoft.VisualBasic.Devices.Computer

.class /*0200001F*/ public auto ansi Microsoft.VisualBasic.Devices.ComputerInfo
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000028:0A00002B*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerTypeProxyAttribute/*0100005A*/::.ctor(class [mscorlib/*23000001*/]System.Type/*0100000B*/) /* 0A00002B */ = ( 01 00 40 4D 69 63 72 6F 73 6F 66 74 2E 56 69 73   // ..@Microsoft.Vis
                                                                                                                                                                                                             75 61 6C 42 61 73 69 63 2E 44 65 76 69 63 65 73   // ualBasic.Devices
                                                                                                                                                                                                             2E 43 6F 6D 70 75 74 65 72 49 6E 66 6F 2B 43 6F   // .ComputerInfo+Co
                                                                                                                                                                                                             6D 70 75 74 65 72 49 6E 66 6F 44 65 62 75 67 56   // mputerInfoDebugV
                                                                                                                                                                                                             69 65 77 00 00 )                                  // iew..
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .class /*02000020*/ auto ansi sealed nested assembly ComputerInfoDebugView
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .method /*060000FA*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2139
      // Code size       0 (0x0)
    } // end of method ComputerInfoDebugView::.ctor

  } // end of class ComputerInfoDebugView

  .method /*060000F1*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2130
    // Code size       0 (0x0)
  } // end of method ComputerInfo::.ctor

  .method /*060000F2*/ public specialname 
          instance uint64  get_TotalPhysicalMemory() cil managed
  // SIG: 20 00 0B
  {
    // Method begins at RVA 0x2131
    // Code size       0 (0x0)
  } // end of method ComputerInfo::get_TotalPhysicalMemory

  .method /*060000F3*/ public specialname 
          instance uint64  get_AvailablePhysicalMemory() cil managed
  // SIG: 20 00 0B
  {
    // Method begins at RVA 0x2132
    // Code size       0 (0x0)
  } // end of method ComputerInfo::get_AvailablePhysicalMemory

  .method /*060000F4*/ public specialname 
          instance uint64  get_TotalVirtualMemory() cil managed
  // SIG: 20 00 0B
  {
    // Method begins at RVA 0x2133
    // Code size       0 (0x0)
  } // end of method ComputerInfo::get_TotalVirtualMemory

  .method /*060000F5*/ public specialname 
          instance uint64  get_AvailableVirtualMemory() cil managed
  // SIG: 20 00 0B
  {
    // Method begins at RVA 0x2134
    // Code size       0 (0x0)
  } // end of method ComputerInfo::get_AvailableVirtualMemory

  .method /*060000F6*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ 
          get_InstalledUICulture() cil managed
  // SIG: 20 00 12 09
  {
    // Method begins at RVA 0x2135
    // Code size       0 (0x0)
  } // end of method ComputerInfo::get_InstalledUICulture

  .method /*060000F7*/ public specialname 
          instance string  get_OSFullName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2136
    // Code size       0 (0x0)
  } // end of method ComputerInfo::get_OSFullName

  .method /*060000F8*/ public specialname 
          instance string  get_OSPlatform() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2137
    // Code size       0 (0x0)
  } // end of method ComputerInfo::get_OSPlatform

  .method /*060000F9*/ public specialname 
          instance string  get_OSVersion() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2138
    // Code size       0 (0x0)
  } // end of method ComputerInfo::get_OSVersion

  .property /*17000041*/ instance callconv(8) uint64
          TotalPhysicalMemory()
  {
    .custom /*0C000032:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance uint64 Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/::get_TotalPhysicalMemory() /* 060000F2 */
  } // end of property ComputerInfo::TotalPhysicalMemory
  .property /*17000042*/ instance callconv(8) uint64
          AvailablePhysicalMemory()
  {
    .custom /*0C000033:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance uint64 Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/::get_AvailablePhysicalMemory() /* 060000F3 */
  } // end of property ComputerInfo::AvailablePhysicalMemory
  .property /*17000043*/ instance callconv(8) uint64
          TotalVirtualMemory()
  {
    .custom /*0C000034:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance uint64 Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/::get_TotalVirtualMemory() /* 060000F4 */
  } // end of property ComputerInfo::TotalVirtualMemory
  .property /*17000044*/ instance callconv(8) uint64
          AvailableVirtualMemory()
  {
    .custom /*0C000036:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance uint64 Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/::get_AvailableVirtualMemory() /* 060000F5 */
  } // end of property ComputerInfo::AvailableVirtualMemory
  .property /*17000045*/ instance callconv(8) class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/
          InstalledUICulture()
  {
    .get instance class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/::get_InstalledUICulture() /* 060000F6 */
  } // end of property ComputerInfo::InstalledUICulture
  .property /*17000046*/ instance callconv(8) string
          OSFullName()
  {
    .get instance string Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/::get_OSFullName() /* 060000F7 */
  } // end of property ComputerInfo::OSFullName
  .property /*17000047*/ instance callconv(8) string
          OSPlatform()
  {
    .get instance string Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/::get_OSPlatform() /* 060000F8 */
  } // end of property ComputerInfo::OSPlatform
  .property /*17000048*/ instance callconv(8) string
          OSVersion()
  {
    .get instance string Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/::get_OSVersion() /* 060000F9 */
  } // end of property ComputerInfo::OSVersion
} // end of class Microsoft.VisualBasic.Devices.ComputerInfo

.class /*02000021*/ public auto ansi Microsoft.VisualBasic.Devices.Keyboard
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*060000FB*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213a
    // Code size       0 (0x0)
  } // end of method Keyboard::.ctor

  .method /*060000FC*/ public instance void 
          SendKeys(string keys) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x213b
    // Code size       0 (0x0)
  } // end of method Keyboard::SendKeys

  .method /*060000FD*/ public instance void 
          SendKeys(string keys,
                   bool wait) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x213c
    // Code size       0 (0x0)
  } // end of method Keyboard::SendKeys

  .method /*060000FE*/ public specialname 
          instance bool  get_ShiftKeyDown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x213d
    // Code size       0 (0x0)
  } // end of method Keyboard::get_ShiftKeyDown

  .method /*060000FF*/ public specialname 
          instance bool  get_AltKeyDown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x213e
    // Code size       0 (0x0)
  } // end of method Keyboard::get_AltKeyDown

  .method /*06000100*/ public specialname 
          instance bool  get_CtrlKeyDown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x213f
    // Code size       0 (0x0)
  } // end of method Keyboard::get_CtrlKeyDown

  .method /*06000101*/ public specialname 
          instance bool  get_CapsLock() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2140
    // Code size       0 (0x0)
  } // end of method Keyboard::get_CapsLock

  .method /*06000102*/ public specialname 
          instance bool  get_NumLock() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2141
    // Code size       0 (0x0)
  } // end of method Keyboard::get_NumLock

  .method /*06000103*/ public specialname 
          instance bool  get_ScrollLock() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2142
    // Code size       0 (0x0)
  } // end of method Keyboard::get_ScrollLock

  .property /*17000049*/ instance callconv(8) bool
          ShiftKeyDown()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Keyboard/*02000021*/::get_ShiftKeyDown() /* 060000FE */
  } // end of property Keyboard::ShiftKeyDown
  .property /*1700004A*/ instance callconv(8) bool
          AltKeyDown()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Keyboard/*02000021*/::get_AltKeyDown() /* 060000FF */
  } // end of property Keyboard::AltKeyDown
  .property /*1700004B*/ instance callconv(8) bool
          CtrlKeyDown()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Keyboard/*02000021*/::get_CtrlKeyDown() /* 06000100 */
  } // end of property Keyboard::CtrlKeyDown
  .property /*1700004C*/ instance callconv(8) bool
          CapsLock()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Keyboard/*02000021*/::get_CapsLock() /* 06000101 */
  } // end of property Keyboard::CapsLock
  .property /*1700004D*/ instance callconv(8) bool
          NumLock()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Keyboard/*02000021*/::get_NumLock() /* 06000102 */
  } // end of property Keyboard::NumLock
  .property /*1700004E*/ instance callconv(8) bool
          ScrollLock()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Keyboard/*02000021*/::get_ScrollLock() /* 06000103 */
  } // end of property Keyboard::ScrollLock
} // end of class Microsoft.VisualBasic.Devices.Keyboard

.class /*02000022*/ public auto ansi Microsoft.VisualBasic.Devices.Mouse
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000104*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2143
    // Code size       0 (0x0)
  } // end of method Mouse::.ctor

  .method /*06000105*/ public specialname 
          instance bool  get_ButtonsSwapped() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2144
    // Code size       0 (0x0)
  } // end of method Mouse::get_ButtonsSwapped

  .method /*06000106*/ public specialname 
          instance bool  get_WheelExists() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2145
    // Code size       0 (0x0)
  } // end of method Mouse::get_WheelExists

  .method /*06000107*/ public specialname 
          instance int32  get_WheelScrollLines() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2146
    // Code size       0 (0x0)
  } // end of method Mouse::get_WheelScrollLines

  .property /*1700004F*/ instance callconv(8) bool
          ButtonsSwapped()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Mouse/*02000022*/::get_ButtonsSwapped() /* 06000105 */
  } // end of property Mouse::ButtonsSwapped
  .property /*17000050*/ instance callconv(8) bool
          WheelExists()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Mouse/*02000022*/::get_WheelExists() /* 06000106 */
  } // end of property Mouse::WheelExists
  .property /*17000051*/ instance callconv(8) int32
          WheelScrollLines()
  {
    .get instance int32 Microsoft.VisualBasic.Devices.Mouse/*02000022*/::get_WheelScrollLines() /* 06000107 */
  } // end of property Mouse::WheelScrollLines
} // end of class Microsoft.VisualBasic.Devices.Mouse

.class /*02000023*/ public auto ansi Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs
       extends [mscorlib/*23000001*/]System.EventArgs/*0100000F*/
{
  .method /*06000108*/ public specialname rtspecialname 
          instance void  .ctor(bool networkAvailable) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2147
    // Code size       0 (0x0)
  } // end of method NetworkAvailableEventArgs::.ctor

  .method /*06000109*/ public specialname 
          instance bool  get_IsNetworkAvailable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2148
    // Code size       0 (0x0)
  } // end of method NetworkAvailableEventArgs::get_IsNetworkAvailable

  .property /*17000052*/ instance callconv(8) bool
          IsNetworkAvailable()
  {
    .get instance bool Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs/*02000023*/::get_IsNetworkAvailable() /* 06000109 */
  } // end of property NetworkAvailableEventArgs::IsNetworkAvailable
} // end of class Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs

.class /*02000024*/ public auto ansi sealed Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*01000010*/
{
  .custom /*0C00002B:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*0600010A*/ public specialname rtspecialname 
          instance void  .ctor(object TargetObject,
                               native int TargetMethod) runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method NetworkAvailableEventHandler::.ctor

  .method /*0600010B*/ public newslot strict virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ 
          BeginInvoke(object sender,
                      class Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs/*02000023*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*01000012*/ DelegateCallback,
                      object DelegateAsyncState) runtime managed
  // SIG: 20 04 12 45 1C 12 80 8C 12 49 1C
  {
  } // end of method NetworkAvailableEventHandler::BeginInvoke

  .method /*0600010C*/ public newslot strict virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*01000011*/ DelegateAsyncResult) runtime managed
  // SIG: 20 01 01 12 45
  {
  } // end of method NetworkAvailableEventHandler::EndInvoke

  .method /*0600010D*/ public newslot strict virtual 
          instance void  Invoke(object sender,
                                class Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs/*02000023*/ e) runtime managed
  // SIG: 20 02 01 1C 12 80 8C
  {
  } // end of method NetworkAvailableEventHandler::Invoke

} // end of class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler

.class /*02000025*/ public auto ansi Microsoft.VisualBasic.Devices.Network
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*0600010E*/ public specialname 
          instance void  add_NetworkAvailabilityChanged(class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/ 'handler') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x2149
    // Code size       0 (0x0)
  } // end of method Network::add_NetworkAvailabilityChanged

  .method /*0600010F*/ public specialname 
          instance void  remove_NetworkAvailabilityChanged(class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/ 'handler') cil managed
  // SIG: 20 01 01 12 80 90
  {
    // Method begins at RVA 0x214a
    // Code size       0 (0x0)
  } // end of method Network::remove_NetworkAvailabilityChanged

  .method /*06000110*/ private specialname 
          instance void  raise_NetworkAvailabilityChanged(object sender,
                                                          class Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs/*02000023*/ e) cil managed
  // SIG: 20 02 01 1C 12 80 8C
  {
    // Method begins at RVA 0x214b
    // Code size       0 (0x0)
  } // end of method Network::raise_NetworkAvailabilityChanged

  .method /*06000111*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214c
    // Code size       0 (0x0)
  } // end of method Network::.ctor

  .method /*06000112*/ public specialname 
          instance bool  get_IsAvailable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x214d
    // Code size       0 (0x0)
  } // end of method Network::get_IsAvailable

  .method /*06000113*/ public instance bool 
          Ping(string hostNameOrAddress) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x214e
    // Code size       0 (0x0)
  } // end of method Network::Ping

  .method /*06000114*/ public instance bool 
          Ping(class [System/*23000003*/]System.Uri/*01000024*/ address) cil managed
  // SIG: 20 01 02 12 80 91
  {
    // Method begins at RVA 0x214f
    // Code size       0 (0x0)
  } // end of method Network::Ping

  .method /*06000115*/ public instance bool 
          Ping(string hostNameOrAddress,
               int32 timeout) cil managed
  // SIG: 20 02 02 0E 08
  {
    // Method begins at RVA 0x2150
    // Code size       0 (0x0)
  } // end of method Network::Ping

  .method /*06000116*/ public instance bool 
          Ping(class [System/*23000003*/]System.Uri/*01000024*/ address,
               int32 timeout) cil managed
  // SIG: 20 02 02 12 80 91 08
  {
    // Method begins at RVA 0x2151
    // Code size       0 (0x0)
  } // end of method Network::Ping

  .method /*06000117*/ public instance void 
          DownloadFile(string address,
                       string destinationFileName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2152
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*06000118*/ public instance void 
          DownloadFile(class [System/*23000003*/]System.Uri/*01000024*/ address,
                       string destinationFileName) cil managed
  // SIG: 20 02 01 12 80 91 0E
  {
    // Method begins at RVA 0x2153
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*06000119*/ public instance void 
          DownloadFile(string address,
                       string destinationFileName,
                       string userName,
                       string password) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    // Method begins at RVA 0x2154
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*0600011A*/ public instance void 
          DownloadFile(class [System/*23000003*/]System.Uri/*01000024*/ address,
                       string destinationFileName,
                       string userName,
                       string password) cil managed
  // SIG: 20 04 01 12 80 91 0E 0E 0E
  {
    // Method begins at RVA 0x2155
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*0600011B*/ public instance void 
          DownloadFile(string address,
                       string destinationFileName,
                       string userName,
                       string password,
                       bool showUI,
                       int32 connectionTimeout,
                       bool overwrite) cil managed
  // SIG: 20 07 01 0E 0E 0E 0E 02 08 02
  {
    // Method begins at RVA 0x2158
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*0600011C*/ public instance void 
          DownloadFile(string address,
                       string destinationFileName,
                       string userName,
                       string password,
                       bool showUI,
                       int32 connectionTimeout,
                       bool overwrite,
                       valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 08 01 0E 0E 0E 0E 02 08 02 11 80 B4
  {
    // Method begins at RVA 0x2164
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*0600011D*/ public instance void 
          DownloadFile(class [System/*23000003*/]System.Uri/*01000024*/ address,
                       string destinationFileName,
                       string userName,
                       string password,
                       bool showUI,
                       int32 connectionTimeout,
                       bool overwrite) cil managed
  // SIG: 20 07 01 12 80 91 0E 0E 0E 02 08 02
  {
    // Method begins at RVA 0x2168
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*0600011E*/ public instance void 
          DownloadFile(class [System/*23000003*/]System.Uri/*01000024*/ address,
                       string destinationFileName,
                       string userName,
                       string password,
                       bool showUI,
                       int32 connectionTimeout,
                       bool overwrite,
                       valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 08 01 12 80 91 0E 0E 0E 02 08 02 11 80 B4
  {
    // Method begins at RVA 0x2174
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*0600011F*/ public instance void 
          DownloadFile(class [System/*23000003*/]System.Uri/*01000024*/ address,
                       string destinationFileName,
                       class [System/*23000003*/]System.Net.ICredentials/*01000025*/ networkCredentials,
                       bool showUI,
                       int32 connectionTimeout,
                       bool overwrite) cil managed
  // SIG: 20 06 01 12 80 91 0E 12 80 95 02 08 02
  {
    // Method begins at RVA 0x2175
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*06000120*/ public instance void 
          DownloadFile(class [System/*23000003*/]System.Uri/*01000024*/ address,
                       string destinationFileName,
                       class [System/*23000003*/]System.Net.ICredentials/*01000025*/ networkCredentials,
                       bool showUI,
                       int32 connectionTimeout,
                       bool overwrite,
                       valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 07 01 12 80 91 0E 12 80 95 02 08 02 11 80 B4
  {
    // Method begins at RVA 0x2176
    // Code size       0 (0x0)
  } // end of method Network::DownloadFile

  .method /*06000121*/ public instance void 
          UploadFile(string sourceFileName,
                     string address) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2177
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*06000122*/ public instance void 
          UploadFile(string sourceFileName,
                     class [System/*23000003*/]System.Uri/*01000024*/ address) cil managed
  // SIG: 20 02 01 0E 12 80 91
  {
    // Method begins at RVA 0x2178
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*06000123*/ public instance void 
          UploadFile(string sourceFileName,
                     string address,
                     string userName,
                     string password) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    // Method begins at RVA 0x2179
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*06000124*/ public instance void 
          UploadFile(string sourceFileName,
                     class [System/*23000003*/]System.Uri/*01000024*/ address,
                     string userName,
                     string password) cil managed
  // SIG: 20 04 01 0E 12 80 91 0E 0E
  {
    // Method begins at RVA 0x217a
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*06000125*/ public instance void 
          UploadFile(string sourceFileName,
                     string address,
                     string userName,
                     string password,
                     bool showUI,
                     int32 connectionTimeout) cil managed
  // SIG: 20 06 01 0E 0E 0E 0E 02 08
  {
    // Method begins at RVA 0x217b
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*06000126*/ public instance void 
          UploadFile(string sourceFileName,
                     string address,
                     string userName,
                     string password,
                     bool showUI,
                     int32 connectionTimeout,
                     valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 07 01 0E 0E 0E 0E 02 08 11 80 B4
  {
    // Method begins at RVA 0x217c
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*06000127*/ public instance void 
          UploadFile(string sourceFileName,
                     class [System/*23000003*/]System.Uri/*01000024*/ address,
                     string userName,
                     string password,
                     bool showUI,
                     int32 connectionTimeout) cil managed
  // SIG: 20 06 01 0E 12 80 91 0E 0E 02 08
  {
    // Method begins at RVA 0x217d
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*06000128*/ public instance void 
          UploadFile(string sourceFileName,
                     class [System/*23000003*/]System.Uri/*01000024*/ address,
                     string userName,
                     string password,
                     bool showUI,
                     int32 connectionTimeout,
                     valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 07 01 0E 12 80 91 0E 0E 02 08 11 80 B4
  {
    // Method begins at RVA 0x217e
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*06000129*/ public instance void 
          UploadFile(string sourceFileName,
                     class [System/*23000003*/]System.Uri/*01000024*/ address,
                     class [System/*23000003*/]System.Net.ICredentials/*01000025*/ networkCredentials,
                     bool showUI,
                     int32 connectionTimeout) cil managed
  // SIG: 20 05 01 0E 12 80 91 12 80 95 02 08
  {
    // Method begins at RVA 0x217f
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .method /*0600012A*/ public instance void 
          UploadFile(string sourceFileName,
                     class [System/*23000003*/]System.Uri/*01000024*/ address,
                     class [System/*23000003*/]System.Net.ICredentials/*01000025*/ networkCredentials,
                     bool showUI,
                     int32 connectionTimeout,
                     valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 06 01 0E 12 80 91 12 80 95 02 08 11 80 B4
  {
    // Method begins at RVA 0x2180
    // Code size       0 (0x0)
  } // end of method Network::UploadFile

  .event /*14000006*/ Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/ NetworkAvailabilityChanged
  {
    .removeon instance void Microsoft.VisualBasic.Devices.Network/*02000025*/::remove_NetworkAvailabilityChanged(class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/) /* 0600010F */
    .addon instance void Microsoft.VisualBasic.Devices.Network/*02000025*/::add_NetworkAvailabilityChanged(class Microsoft.VisualBasic.Devices.NetworkAvailableEventHandler/*02000024*/) /* 0600010E */
    .fire instance void Microsoft.VisualBasic.Devices.Network/*02000025*/::raise_NetworkAvailabilityChanged(object,
                                                                                                            class Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs/*02000023*/) /* 06000110 */
  } // end of event Network::NetworkAvailabilityChanged
  .property /*17000053*/ instance callconv(8) bool
          IsAvailable()
  {
    .get instance bool Microsoft.VisualBasic.Devices.Network/*02000025*/::get_IsAvailable() /* 06000112 */
  } // end of property Network::IsAvailable
} // end of class Microsoft.VisualBasic.Devices.Network

.class /*02000026*/ public auto ansi Microsoft.VisualBasic.Devices.Ports
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*0600012B*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2181
    // Code size       0 (0x0)
  } // end of method Ports::.ctor

  .method /*0600012C*/ public instance class [System/*23000003*/]System.IO.Ports.SerialPort/*01000026*/ 
          OpenSerialPort(string portName) cil managed
  // SIG: 20 01 12 80 99 0E
  {
    // Method begins at RVA 0x2182
    // Code size       0 (0x0)
  } // end of method Ports::OpenSerialPort

  .method /*0600012D*/ public instance class [System/*23000003*/]System.IO.Ports.SerialPort/*01000026*/ 
          OpenSerialPort(string portName,
                         int32 baudRate) cil managed
  // SIG: 20 02 12 80 99 0E 08
  {
    // Method begins at RVA 0x2183
    // Code size       0 (0x0)
  } // end of method Ports::OpenSerialPort

  .method /*0600012E*/ public instance class [System/*23000003*/]System.IO.Ports.SerialPort/*01000026*/ 
          OpenSerialPort(string portName,
                         int32 baudRate,
                         valuetype [System/*23000003*/]System.IO.Ports.Parity/*01000027*/ parity) cil managed
  // SIG: 20 03 12 80 99 0E 08 11 80 9D
  {
    // Method begins at RVA 0x2184
    // Code size       0 (0x0)
  } // end of method Ports::OpenSerialPort

  .method /*0600012F*/ public instance class [System/*23000003*/]System.IO.Ports.SerialPort/*01000026*/ 
          OpenSerialPort(string portName,
                         int32 baudRate,
                         valuetype [System/*23000003*/]System.IO.Ports.Parity/*01000027*/ parity,
                         int32 dataBits) cil managed
  // SIG: 20 04 12 80 99 0E 08 11 80 9D 08
  {
    // Method begins at RVA 0x2185
    // Code size       0 (0x0)
  } // end of method Ports::OpenSerialPort

  .method /*06000130*/ public instance class [System/*23000003*/]System.IO.Ports.SerialPort/*01000026*/ 
          OpenSerialPort(string portName,
                         int32 baudRate,
                         valuetype [System/*23000003*/]System.IO.Ports.Parity/*01000027*/ parity,
                         int32 dataBits,
                         valuetype [System/*23000003*/]System.IO.Ports.StopBits/*01000028*/ stopBits) cil managed
  // SIG: 20 05 12 80 99 0E 08 11 80 9D 08 11 80 A1
  {
    // Method begins at RVA 0x2186
    // Code size       0 (0x0)
  } // end of method Ports::OpenSerialPort

  .method /*06000131*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          get_SerialPortNames() cil managed
  // SIG: 20 00 15 12 15 01 0E
  {
    // Method begins at RVA 0x2187
    // Code size       0 (0x0)
  } // end of method Ports::get_SerialPortNames

  .property /*17000054*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string>
          SerialPortNames()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> Microsoft.VisualBasic.Devices.Ports/*02000026*/::get_SerialPortNames() /* 06000131 */
  } // end of property Ports::SerialPortNames
} // end of class Microsoft.VisualBasic.Devices.Ports

.class /*02000027*/ public auto ansi Microsoft.VisualBasic.Devices.ServerComputer
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000132*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2188
    // Code size       0 (0x0)
  } // end of method ServerComputer::.ctor

  .method /*06000133*/ public specialname 
          instance class Microsoft.VisualBasic.Devices.Clock/*0200001D*/ 
          get_Clock() cil managed
  // SIG: 20 00 12 74
  {
    // Method begins at RVA 0x2189
    // Code size       0 (0x0)
  } // end of method ServerComputer::get_Clock

  .method /*06000134*/ public specialname 
          instance class Microsoft.VisualBasic.MyServices.FileSystemProxy/*0200007A*/ 
          get_FileSystem() cil managed
  // SIG: 20 00 12 81 E8
  {
    // Method begins at RVA 0x218a
    // Code size       0 (0x0)
  } // end of method ServerComputer::get_FileSystem

  .method /*06000135*/ public specialname 
          instance class Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/ 
          get_Info() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x218b
    // Code size       0 (0x0)
  } // end of method ServerComputer::get_Info

  .method /*06000136*/ public specialname 
          instance class Microsoft.VisualBasic.Devices.Network/*02000025*/ 
          get_Network() cil managed
  // SIG: 20 00 12 80 94
  {
    // Method begins at RVA 0x218c
    // Code size       0 (0x0)
  } // end of method ServerComputer::get_Network

  .method /*06000137*/ public specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x218d
    // Code size       0 (0x0)
  } // end of method ServerComputer::get_Name

  .method /*06000138*/ public specialname 
          instance class Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/ 
          get_Registry() cil managed
  // SIG: 20 00 12 81 F0
  {
    // Method begins at RVA 0x218e
    // Code size       0 (0x0)
  } // end of method ServerComputer::get_Registry

  .property /*17000055*/ instance callconv(8) class Microsoft.VisualBasic.Devices.Clock/*0200001D*/
          Clock()
  {
    .get instance class Microsoft.VisualBasic.Devices.Clock/*0200001D*/ Microsoft.VisualBasic.Devices.ServerComputer/*02000027*/::get_Clock() /* 06000133 */
  } // end of property ServerComputer::Clock
  .property /*17000056*/ instance callconv(8) class Microsoft.VisualBasic.MyServices.FileSystemProxy/*0200007A*/
          FileSystem()
  {
    .get instance class Microsoft.VisualBasic.MyServices.FileSystemProxy/*0200007A*/ Microsoft.VisualBasic.Devices.ServerComputer/*02000027*/::get_FileSystem() /* 06000134 */
  } // end of property ServerComputer::FileSystem
  .property /*17000057*/ instance callconv(8) class Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/
          Info()
  {
    .get instance class Microsoft.VisualBasic.Devices.ComputerInfo/*0200001F*/ Microsoft.VisualBasic.Devices.ServerComputer/*02000027*/::get_Info() /* 06000135 */
  } // end of property ServerComputer::Info
  .property /*17000058*/ instance callconv(8) class Microsoft.VisualBasic.Devices.Network/*02000025*/
          Network()
  {
    .get instance class Microsoft.VisualBasic.Devices.Network/*02000025*/ Microsoft.VisualBasic.Devices.ServerComputer/*02000027*/::get_Network() /* 06000136 */
  } // end of property ServerComputer::Network
  .property /*17000059*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.VisualBasic.Devices.ServerComputer/*02000027*/::get_Name() /* 06000137 */
  } // end of property ServerComputer::Name
  .property /*1700005A*/ instance callconv(8) class Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/
          Registry()
  {
    .get instance class Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/ Microsoft.VisualBasic.Devices.ServerComputer/*02000027*/::get_Registry() /* 06000138 */
  } // end of property ServerComputer::Registry
} // end of class Microsoft.VisualBasic.Devices.ServerComputer

.class /*02000028*/ public auto ansi sealed Microsoft.VisualBasic.ErrObject
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000139*/ assembly specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x218f
    // Code size       0 (0x0)
  } // end of method ErrObject::.ctor

  .method /*0600013A*/ public specialname 
          instance int32  get_Erl() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2190
    // Code size       0 (0x0)
  } // end of method ErrObject::get_Erl

  .method /*0600013B*/ public specialname 
          instance int32  get_Number() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2191
    // Code size       0 (0x0)
  } // end of method ErrObject::get_Number

  .method /*0600013C*/ public specialname 
          instance void  set_Number(int32 Value) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2192
    // Code size       0 (0x0)
  } // end of method ErrObject::set_Number

  .method /*0600013D*/ public specialname 
          instance string  get_Source() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2193
    // Code size       0 (0x0)
  } // end of method ErrObject::get_Source

  .method /*0600013E*/ public specialname 
          instance void  set_Source(string Value) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2194
    // Code size       0 (0x0)
  } // end of method ErrObject::set_Source

  .method /*0600013F*/ public specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2195
    // Code size       0 (0x0)
  } // end of method ErrObject::get_Description

  .method /*06000140*/ public specialname 
          instance void  set_Description(string Value) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2196
    // Code size       0 (0x0)
  } // end of method ErrObject::set_Description

  .method /*06000141*/ public specialname 
          instance string  get_HelpFile() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2197
    // Code size       0 (0x0)
  } // end of method ErrObject::get_HelpFile

  .method /*06000142*/ public specialname 
          instance void  set_HelpFile(string Value) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2198
    // Code size       0 (0x0)
  } // end of method ErrObject::set_HelpFile

  .method /*06000143*/ public specialname 
          instance int32  get_HelpContext() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2199
    // Code size       0 (0x0)
  } // end of method ErrObject::get_HelpContext

  .method /*06000144*/ public specialname 
          instance void  set_HelpContext(int32 Value) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x219a
    // Code size       0 (0x0)
  } // end of method ErrObject::set_HelpContext

  .method /*06000145*/ public instance class [mscorlib/*23000001*/]System.Exception/*0100000D*/ 
          GetException() cil managed
  // SIG: 20 00 12 35
  {
    // Method begins at RVA 0x219b
    // Code size       0 (0x0)
  } // end of method ErrObject::GetException

  .method /*06000146*/ public instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00008A:0A00002C*/ instance void [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.ReliabilityContractAttribute/*0100005B*/::.ctor(valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Consistency/*0100005C*/,
                                                                                                                                                          valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Cer/*0100005D*/) /* 0A00002C */ = ( 01 00 03 00 00 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x219c
    // Code size       0 (0x0)
  } // end of method ErrObject::Clear

  .method /*06000147*/ public instance void 
          Raise(int32 Number,
                [opt] object Source,
                [opt] object Description,
                [opt] object HelpFile,
                [opt] object HelpContext) cil managed
  // SIG: 20 05 01 08 1C 1C 1C 1C
  {
    .param [2]/*0800016C*/  = nullref
    .param [3]/*0800016D*/  = nullref
    .param [4]/*0800016E*/  = nullref
    .param [5]/*0800016F*/  = nullref
    // Method begins at RVA 0x219d
    // Code size       0 (0x0)
  } // end of method ErrObject::Raise

  .method /*06000148*/ public specialname 
          instance int32  get_LastDllError() cil managed
  // SIG: 20 00 08
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(2)}}
    // Method begins at RVA 0x219e
    // Code size       0 (0x0)
  } // end of method ErrObject::get_LastDllError

  .property /*1700005B*/ instance callconv(8) int32
          Erl()
  {
    .get instance int32 Microsoft.VisualBasic.ErrObject/*02000028*/::get_Erl() /* 0600013A */
  } // end of property ErrObject::Erl
  .property /*1700005C*/ instance callconv(8) int32
          Number()
  {
    .get instance int32 Microsoft.VisualBasic.ErrObject/*02000028*/::get_Number() /* 0600013B */
    .set instance void Microsoft.VisualBasic.ErrObject/*02000028*/::set_Number(int32) /* 0600013C */
  } // end of property ErrObject::Number
  .property /*1700005D*/ instance callconv(8) string
          Source()
  {
    .set instance void Microsoft.VisualBasic.ErrObject/*02000028*/::set_Source(string) /* 0600013E */
    .get instance string Microsoft.VisualBasic.ErrObject/*02000028*/::get_Source() /* 0600013D */
  } // end of property ErrObject::Source
  .property /*1700005E*/ instance callconv(8) string
          Description()
  {
    .get instance string Microsoft.VisualBasic.ErrObject/*02000028*/::get_Description() /* 0600013F */
    .set instance void Microsoft.VisualBasic.ErrObject/*02000028*/::set_Description(string) /* 06000140 */
  } // end of property ErrObject::Description
  .property /*1700005F*/ instance callconv(8) string
          HelpFile()
  {
    .set instance void Microsoft.VisualBasic.ErrObject/*02000028*/::set_HelpFile(string) /* 06000142 */
    .get instance string Microsoft.VisualBasic.ErrObject/*02000028*/::get_HelpFile() /* 06000141 */
  } // end of property ErrObject::HelpFile
  .property /*17000060*/ instance callconv(8) int32
          HelpContext()
  {
    .set instance void Microsoft.VisualBasic.ErrObject/*02000028*/::set_HelpContext(int32) /* 06000144 */
    .get instance int32 Microsoft.VisualBasic.ErrObject/*02000028*/::get_HelpContext() /* 06000143 */
  } // end of property ErrObject::HelpContext
  .property /*17000061*/ instance callconv(8) int32
          LastDllError()
  {
    .get instance int32 Microsoft.VisualBasic.ErrObject/*02000028*/::get_LastDllError() /* 06000148 */
  } // end of property ErrObject::LastDllError
} // end of class Microsoft.VisualBasic.ErrObject

.class /*02000029*/ public auto ansi beforefieldinit Microsoft.VisualBasic.FileIO.FileSystem
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000149*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x219f
    // Code size       0 (0x0)
  } // end of method FileSystem::.cctor

  .method /*0600014A*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21a0
    // Code size       0 (0x0)
  } // end of method FileSystem::.ctor

  .method /*0600014B*/ public specialname static 
          class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.IO.DriveInfo/*01000029*/> 
          get_Drives() cil managed
  // SIG: 00 00 15 12 15 01 12 80 A5
  {
    // Method begins at RVA 0x21a1
    // Code size       0 (0x0)
  } // end of method FileSystem::get_Drives

  .method /*0600014C*/ public specialname static 
          string  get_CurrentDirectory() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21a2
    // Code size       0 (0x0)
  } // end of method FileSystem::get_CurrentDirectory

  .method /*0600014D*/ public specialname static 
          void  set_CurrentDirectory(string 'value') cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x21a3
    // Code size       0 (0x0)
  } // end of method FileSystem::set_CurrentDirectory

  .method /*0600014E*/ public static string 
          CombinePath(string baseDirectory,
                      string relativePath) cil managed
  // SIG: 00 02 0E 0E 0E
  {
    // Method begins at RVA 0x21a4
    // Code size       0 (0x0)
  } // end of method FileSystem::CombinePath

  .method /*0600014F*/ public static bool 
          DirectoryExists(string directory) cil managed
  // SIG: 00 01 02 0E
  {
    // Method begins at RVA 0x21a5
    // Code size       0 (0x0)
  } // end of method FileSystem::DirectoryExists

  .method /*06000150*/ public static bool 
          FileExists(string file) cil managed
  // SIG: 00 01 02 0E
  {
    // Method begins at RVA 0x21a6
    // Code size       0 (0x0)
  } // end of method FileSystem::FileExists

  .method /*06000151*/ public static class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          FindInFiles(string directory,
                      string containsText,
                      bool ignoreCase,
                      valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ searchType) cil managed
  // SIG: 00 04 15 12 15 01 0E 0E 0E 02 11 80 B0
  {
    // Method begins at RVA 0x21a7
    // Code size       0 (0x0)
  } // end of method FileSystem::FindInFiles

  .method /*06000152*/ public static class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          FindInFiles(string directory,
                      string containsText,
                      bool ignoreCase,
                      valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ searchType,
                      string[] fileWildcards) cil managed
  // SIG: 00 05 15 12 15 01 0E 0E 0E 02 11 80 B0 1D 0E
  {
    .param [5]/*0800017D*/ 
    .custom /*0C00008B:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a8
    // Code size       0 (0x0)
  } // end of method FileSystem::FindInFiles

  .method /*06000153*/ public static class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          GetDirectories(string directory) cil managed
  // SIG: 00 01 15 12 15 01 0E 0E
  {
    // Method begins at RVA 0x21a9
    // Code size       0 (0x0)
  } // end of method FileSystem::GetDirectories

  .method /*06000154*/ public static class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          GetDirectories(string directory,
                         valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ searchType,
                         string[] wildcards) cil managed
  // SIG: 00 03 15 12 15 01 0E 0E 11 80 B0 1D 0E
  {
    .param [3]/*08000181*/ 
    .custom /*0C00008C:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21aa
    // Code size       0 (0x0)
  } // end of method FileSystem::GetDirectories

  .method /*06000155*/ public static class [mscorlib/*23000001*/]System.IO.DirectoryInfo/*0100002A*/ 
          GetDirectoryInfo(string directory) cil managed
  // SIG: 00 01 12 80 A9 0E
  {
    // Method begins at RVA 0x21ab
    // Code size       0 (0x0)
  } // end of method FileSystem::GetDirectoryInfo

  .method /*06000156*/ public static class [mscorlib/*23000001*/]System.IO.DriveInfo/*01000029*/ 
          GetDriveInfo(string drive) cil managed
  // SIG: 00 01 12 80 A5 0E
  {
    // Method begins at RVA 0x21ac
    // Code size       0 (0x0)
  } // end of method FileSystem::GetDriveInfo

  .method /*06000157*/ public static class [mscorlib/*23000001*/]System.IO.FileInfo/*0100002B*/ 
          GetFileInfo(string file) cil managed
  // SIG: 00 01 12 80 AD 0E
  {
    // Method begins at RVA 0x21ad
    // Code size       0 (0x0)
  } // end of method FileSystem::GetFileInfo

  .method /*06000158*/ public static class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          GetFiles(string directory) cil managed
  // SIG: 00 01 15 12 15 01 0E 0E
  {
    // Method begins at RVA 0x21ae
    // Code size       0 (0x0)
  } // end of method FileSystem::GetFiles

  .method /*06000159*/ public static class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          GetFiles(string directory,
                   valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ searchType,
                   string[] wildcards) cil managed
  // SIG: 00 03 15 12 15 01 0E 0E 11 80 B0 1D 0E
  {
    .param [3]/*08000188*/ 
    .custom /*0C00008E:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21af
    // Code size       0 (0x0)
  } // end of method FileSystem::GetFiles

  .method /*0600015A*/ public static string 
          GetName(string path) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x21b0
    // Code size       0 (0x0)
  } // end of method FileSystem::GetName

  .method /*0600015B*/ public static string 
          GetParentPath(string path) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x21b1
    // Code size       0 (0x0)
  } // end of method FileSystem::GetParentPath

  .method /*0600015C*/ public static string 
          GetTempFileName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21b2
    // Code size       0 (0x0)
  } // end of method FileSystem::GetTempFileName

  .method /*0600015D*/ public static class Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/ 
          OpenTextFieldParser(string file) cil managed
  // SIG: 00 01 12 80 C4 0E
  {
    // Method begins at RVA 0x21b3
    // Code size       0 (0x0)
  } // end of method FileSystem::OpenTextFieldParser

  .method /*0600015E*/ public static class Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/ 
          OpenTextFieldParser(string file,
                              string[] delimiters) cil managed
  // SIG: 00 02 12 80 C4 0E 1D 0E
  {
    .param [2]/*0800018D*/ 
    .custom /*0C000090:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b4
    // Code size       0 (0x0)
  } // end of method FileSystem::OpenTextFieldParser

  .method /*0600015F*/ public static class Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/ 
          OpenTextFieldParser(string file,
                              int32[] fieldWidths) cil managed
  // SIG: 00 02 12 80 C4 0E 1D 08
  {
    .param [2]/*0800018F*/ 
    .custom /*0C000091:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       0 (0x0)
  } // end of method FileSystem::OpenTextFieldParser

  .method /*06000160*/ public static class [mscorlib/*23000001*/]System.IO.StreamReader/*0100002C*/ 
          OpenTextFileReader(string file) cil managed
  // SIG: 00 01 12 80 B1 0E
  {
    // Method begins at RVA 0x21b6
    // Code size       0 (0x0)
  } // end of method FileSystem::OpenTextFileReader

  .method /*06000161*/ public static class [mscorlib/*23000001*/]System.IO.StreamReader/*0100002C*/ 
          OpenTextFileReader(string file,
                             class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ encoding) cil managed
  // SIG: 00 02 12 80 B1 0E 12 80 B5
  {
    // Method begins at RVA 0x21b7
    // Code size       0 (0x0)
  } // end of method FileSystem::OpenTextFileReader

  .method /*06000162*/ public static class [mscorlib/*23000001*/]System.IO.StreamWriter/*0100002E*/ 
          OpenTextFileWriter(string file,
                             bool append) cil managed
  // SIG: 00 02 12 80 B9 0E 02
  {
    // Method begins at RVA 0x21b8
    // Code size       0 (0x0)
  } // end of method FileSystem::OpenTextFileWriter

  .method /*06000163*/ public static class [mscorlib/*23000001*/]System.IO.StreamWriter/*0100002E*/ 
          OpenTextFileWriter(string file,
                             bool append,
                             class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ encoding) cil managed
  // SIG: 00 03 12 80 B9 0E 02 12 80 B5
  {
    // Method begins at RVA 0x21b9
    // Code size       0 (0x0)
  } // end of method FileSystem::OpenTextFileWriter

  .method /*06000164*/ public static uint8[] 
          ReadAllBytes(string file) cil managed
  // SIG: 00 01 1D 05 0E
  {
    // Method begins at RVA 0x21ba
    // Code size       0 (0x0)
  } // end of method FileSystem::ReadAllBytes

  .method /*06000165*/ public static string 
          ReadAllText(string file) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x21bb
    // Code size       0 (0x0)
  } // end of method FileSystem::ReadAllText

  .method /*06000166*/ public static string 
          ReadAllText(string file,
                      class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ encoding) cil managed
  // SIG: 00 02 0E 0E 12 80 B5
  {
    // Method begins at RVA 0x21bc
    // Code size       0 (0x0)
  } // end of method FileSystem::ReadAllText

  .method /*06000167*/ public static void 
          CopyDirectory(string sourceDirectoryName,
                        string destinationDirectoryName) cil managed
  // SIG: 00 02 01 0E 0E
  {
    // Method begins at RVA 0x21bd
    // Code size       0 (0x0)
  } // end of method FileSystem::CopyDirectory

  .method /*06000168*/ public static void 
          CopyDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        bool overwrite) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x21be
    // Code size       0 (0x0)
  } // end of method FileSystem::CopyDirectory

  .method /*06000169*/ public static void 
          CopyDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI) cil managed
  // SIG: 00 03 01 0E 0E 11 80 B8
  {
    // Method begins at RVA 0x21bf
    // Code size       0 (0x0)
  } // end of method FileSystem::CopyDirectory

  .method /*0600016A*/ public static void 
          CopyDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                        valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 00 04 01 0E 0E 11 80 B8 11 80 B4
  {
    // Method begins at RVA 0x21c0
    // Code size       0 (0x0)
  } // end of method FileSystem::CopyDirectory

  .method /*0600016B*/ public static void 
          CopyFile(string sourceFileName,
                   string destinationFileName) cil managed
  // SIG: 00 02 01 0E 0E
  {
    // Method begins at RVA 0x21c1
    // Code size       0 (0x0)
  } // end of method FileSystem::CopyFile

  .method /*0600016C*/ public static void 
          CopyFile(string sourceFileName,
                   string destinationFileName,
                   bool overwrite) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x21c2
    // Code size       0 (0x0)
  } // end of method FileSystem::CopyFile

  .method /*0600016D*/ public static void 
          CopyFile(string sourceFileName,
                   string destinationFileName,
                   valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI) cil managed
  // SIG: 00 03 01 0E 0E 11 80 B8
  {
    // Method begins at RVA 0x21c3
    // Code size       0 (0x0)
  } // end of method FileSystem::CopyFile

  .method /*0600016E*/ public static void 
          CopyFile(string sourceFileName,
                   string destinationFileName,
                   valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                   valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 00 04 01 0E 0E 11 80 B8 11 80 B4
  {
    // Method begins at RVA 0x21c4
    // Code size       0 (0x0)
  } // end of method FileSystem::CopyFile

  .method /*0600016F*/ public static void 
          CreateDirectory(string directory) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x21c5
    // Code size       0 (0x0)
  } // end of method FileSystem::CreateDirectory

  .method /*06000170*/ public static void 
          DeleteDirectory(string directory,
                          valuetype Microsoft.VisualBasic.FileIO.DeleteDirectoryOption/*0200002A*/ onDirectoryNotEmpty) cil managed
  // SIG: 00 02 01 0E 11 80 A8
  {
    // Method begins at RVA 0x21c6
    // Code size       0 (0x0)
  } // end of method FileSystem::DeleteDirectory

  .method /*06000171*/ public static void 
          DeleteDirectory(string directory,
                          valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                          valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ recycle) cil managed
  // SIG: 00 03 01 0E 11 80 B8 11 80 AC
  {
    // Method begins at RVA 0x21c7
    // Code size       0 (0x0)
  } // end of method FileSystem::DeleteDirectory

  .method /*06000172*/ public static void 
          DeleteDirectory(string directory,
                          valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                          valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ recycle,
                          valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 00 04 01 0E 11 80 B8 11 80 AC 11 80 B4
  {
    // Method begins at RVA 0x21c8
    // Code size       0 (0x0)
  } // end of method FileSystem::DeleteDirectory

  .method /*06000173*/ public static void 
          DeleteFile(string file) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x21c9
    // Code size       0 (0x0)
  } // end of method FileSystem::DeleteFile

  .method /*06000174*/ public static void 
          DeleteFile(string file,
                     valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                     valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ recycle) cil managed
  // SIG: 00 03 01 0E 11 80 B8 11 80 AC
  {
    // Method begins at RVA 0x21ca
    // Code size       0 (0x0)
  } // end of method FileSystem::DeleteFile

  .method /*06000175*/ public static void 
          DeleteFile(string file,
                     valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                     valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ recycle,
                     valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 00 04 01 0E 11 80 B8 11 80 AC 11 80 B4
  {
    // Method begins at RVA 0x21cb
    // Code size       0 (0x0)
  } // end of method FileSystem::DeleteFile

  .method /*06000176*/ public static void 
          MoveDirectory(string sourceDirectoryName,
                        string destinationDirectoryName) cil managed
  // SIG: 00 02 01 0E 0E
  {
    // Method begins at RVA 0x21cc
    // Code size       0 (0x0)
  } // end of method FileSystem::MoveDirectory

  .method /*06000177*/ public static void 
          MoveDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        bool overwrite) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x21cd
    // Code size       0 (0x0)
  } // end of method FileSystem::MoveDirectory

  .method /*06000178*/ public static void 
          MoveDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI) cil managed
  // SIG: 00 03 01 0E 0E 11 80 B8
  {
    // Method begins at RVA 0x21ce
    // Code size       0 (0x0)
  } // end of method FileSystem::MoveDirectory

  .method /*06000179*/ public static void 
          MoveDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                        valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 00 04 01 0E 0E 11 80 B8 11 80 B4
  {
    // Method begins at RVA 0x21cf
    // Code size       0 (0x0)
  } // end of method FileSystem::MoveDirectory

  .method /*0600017A*/ public static void 
          MoveFile(string sourceFileName,
                   string destinationFileName) cil managed
  // SIG: 00 02 01 0E 0E
  {
    // Method begins at RVA 0x21d0
    // Code size       0 (0x0)
  } // end of method FileSystem::MoveFile

  .method /*0600017B*/ public static void 
          MoveFile(string sourceFileName,
                   string destinationFileName,
                   bool overwrite) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x21d1
    // Code size       0 (0x0)
  } // end of method FileSystem::MoveFile

  .method /*0600017C*/ public static void 
          MoveFile(string sourceFileName,
                   string destinationFileName,
                   valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI) cil managed
  // SIG: 00 03 01 0E 0E 11 80 B8
  {
    // Method begins at RVA 0x21d2
    // Code size       0 (0x0)
  } // end of method FileSystem::MoveFile

  .method /*0600017D*/ public static void 
          MoveFile(string sourceFileName,
                   string destinationFileName,
                   valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                   valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 00 04 01 0E 0E 11 80 B8 11 80 B4
  {
    // Method begins at RVA 0x21d3
    // Code size       0 (0x0)
  } // end of method FileSystem::MoveFile

  .method /*0600017E*/ public static void 
          RenameDirectory(string directory,
                          string newName) cil managed
  // SIG: 00 02 01 0E 0E
  {
    // Method begins at RVA 0x21d4
    // Code size       0 (0x0)
  } // end of method FileSystem::RenameDirectory

  .method /*0600017F*/ public static void 
          RenameFile(string file,
                     string newName) cil managed
  // SIG: 00 02 01 0E 0E
  {
    // Method begins at RVA 0x21d5
    // Code size       0 (0x0)
  } // end of method FileSystem::RenameFile

  .method /*06000180*/ public static void 
          WriteAllBytes(string file,
                        uint8[] data,
                        bool append) cil managed
  // SIG: 00 03 01 0E 1D 05 02
  {
    // Method begins at RVA 0x21d6
    // Code size       0 (0x0)
  } // end of method FileSystem::WriteAllBytes

  .method /*06000181*/ public static void 
          WriteAllText(string file,
                       string text,
                       bool append) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x21d7
    // Code size       0 (0x0)
  } // end of method FileSystem::WriteAllText

  .method /*06000182*/ public static void 
          WriteAllText(string file,
                       string text,
                       bool append,
                       class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ encoding) cil managed
  // SIG: 00 04 01 0E 0E 02 12 80 B5
  {
    // Method begins at RVA 0x21d8
    // Code size       0 (0x0)
  } // end of method FileSystem::WriteAllText

  .property /*17000062*/ callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.IO.DriveInfo/*01000029*/>
          Drives()
  {
    .get class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.IO.DriveInfo/*01000029*/> Microsoft.VisualBasic.FileIO.FileSystem/*02000029*/::get_Drives() /* 0600014B */
  } // end of property FileSystem::Drives
  .property /*17000063*/ callconv(8) string
          CurrentDirectory()
  {
    .set void Microsoft.VisualBasic.FileIO.FileSystem/*02000029*/::set_CurrentDirectory(string) /* 0600014D */
    .get string Microsoft.VisualBasic.FileIO.FileSystem/*02000029*/::get_CurrentDirectory() /* 0600014C */
  } // end of property FileSystem::CurrentDirectory
} // end of class Microsoft.VisualBasic.FileIO.FileSystem

.class /*0200002A*/ public auto ansi sealed Microsoft.VisualBasic.FileIO.DeleteDirectoryOption
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000085*/ public specialname rtspecialname int32 value__
  .field /*04000086*/ public static literal valuetype Microsoft.VisualBasic.FileIO.DeleteDirectoryOption/*0200002A*/ ThrowIfDirectoryNonEmpty = int32(0x00000004)
  .field /*04000087*/ public static literal valuetype Microsoft.VisualBasic.FileIO.DeleteDirectoryOption/*0200002A*/ DeleteAllContents = int32(0x00000005)
} // end of class Microsoft.VisualBasic.FileIO.DeleteDirectoryOption

.class /*0200002B*/ public auto ansi sealed Microsoft.VisualBasic.FileIO.RecycleOption
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000088*/ public specialname rtspecialname int32 value__
  .field /*04000089*/ public static literal valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ DeletePermanently = int32(0x00000002)
  .field /*0400008A*/ public static literal valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ SendToRecycleBin = int32(0x00000003)
} // end of class Microsoft.VisualBasic.FileIO.RecycleOption

.class /*0200002C*/ public auto ansi sealed Microsoft.VisualBasic.FileIO.SearchOption
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*0400008B*/ public specialname rtspecialname int32 value__
  .field /*0400008C*/ public static literal valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ SearchTopLevelOnly = int32(0x00000002)
  .field /*0400008D*/ public static literal valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ SearchAllSubDirectories = int32(0x00000003)
} // end of class Microsoft.VisualBasic.FileIO.SearchOption

.class /*0200002D*/ public auto ansi sealed Microsoft.VisualBasic.FileIO.UICancelOption
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*0400008E*/ public specialname rtspecialname int32 value__
  .field /*0400008F*/ public static literal valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ DoNothing = int32(0x00000002)
  .field /*04000090*/ public static literal valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ ThrowException = int32(0x00000003)
} // end of class Microsoft.VisualBasic.FileIO.UICancelOption

.class /*0200002E*/ public auto ansi sealed Microsoft.VisualBasic.FileIO.UIOption
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000091*/ public specialname rtspecialname int32 value__
  .field /*04000092*/ public static literal valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ OnlyErrorDialogs = int32(0x00000002)
  .field /*04000093*/ public static literal valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ AllDialogs = int32(0x00000003)
} // end of class Microsoft.VisualBasic.FileIO.UIOption

.class /*0200002F*/ public auto ansi serializable Microsoft.VisualBasic.FileIO.MalformedLineException
       extends [mscorlib/*23000001*/]System.Exception/*0100000D*/
{
  .method /*06000183*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21d9
    // Code size       0 (0x0)
  } // end of method MalformedLineException::.ctor

  .method /*06000184*/ public specialname rtspecialname 
          instance void  .ctor(string message,
                               int64 lineNumber) cil managed
  // SIG: 20 02 01 0E 0A
  {
    // Method begins at RVA 0x21da
    // Code size       0 (0x0)
  } // end of method MalformedLineException::.ctor

  .method /*06000185*/ public specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21db
    // Code size       0 (0x0)
  } // end of method MalformedLineException::.ctor

  .method /*06000186*/ public specialname rtspecialname 
          instance void  .ctor(string message,
                               int64 lineNumber,
                               class [mscorlib/*23000001*/]System.Exception/*0100000D*/ innerException) cil managed
  // SIG: 20 03 01 0E 0A 12 35
  {
    // Method begins at RVA 0x21dc
    // Code size       0 (0x0)
  } // end of method MalformedLineException::.ctor

  .method /*06000187*/ public specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*0100000D*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 35
  {
    // Method begins at RVA 0x21dd
    // Code size       0 (0x0)
  } // end of method MalformedLineException::.ctor

  .method /*06000188*/ family specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000013*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000014*/ context) cil managed
  // SIG: 20 02 01 12 4D 11 51
  {
    .custom /*0C00008D:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x21de
    // Code size       0 (0x0)
  } // end of method MalformedLineException::.ctor

  .method /*06000189*/ public specialname 
          instance int64  get_LineNumber() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21df
    // Code size       0 (0x0)
  } // end of method MalformedLineException::get_LineNumber

  .method /*0600018A*/ public specialname 
          instance void  set_LineNumber(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x21e0
    // Code size       0 (0x0)
  } // end of method MalformedLineException::set_LineNumber

  .method /*0600018B*/ public strict virtual 
          instance void  GetObjectData(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000013*/ info,
                                       valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000014*/ context) cil managed
  // SIG: 20 02 01 12 4D 11 51
  {
    .custom /*0C00008F:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .permissionset demand
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'SerializationFormatter' = bool(true)}}
    // Method begins at RVA 0x21e1
    // Code size       0 (0x0)
  } // end of method MalformedLineException::GetObjectData

  .method /*0600018C*/ public strict virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21e2
    // Code size       0 (0x0)
  } // end of method MalformedLineException::ToString

  .property /*17000064*/ instance callconv(8) int64
          LineNumber()
  {
    .custom /*0C00005D:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance int64 Microsoft.VisualBasic.FileIO.MalformedLineException/*0200002F*/::get_LineNumber() /* 06000189 */
    .set instance void Microsoft.VisualBasic.FileIO.MalformedLineException/*0200002F*/::set_LineNumber(int64) /* 0600018A */
  } // end of property MalformedLineException::LineNumber
} // end of class Microsoft.VisualBasic.FileIO.MalformedLineException

.class /*02000030*/ public auto ansi Microsoft.VisualBasic.FileIO.SpecialDirectories
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*0600018D*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e3
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::.ctor

  .method /*0600018E*/ public specialname static 
          string  get_MyDocuments() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21e4
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_MyDocuments

  .method /*0600018F*/ public specialname static 
          string  get_MyMusic() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21e5
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_MyMusic

  .method /*06000190*/ public specialname static 
          string  get_MyPictures() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21e6
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_MyPictures

  .method /*06000191*/ public specialname static 
          string  get_Desktop() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21e7
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_Desktop

  .method /*06000192*/ public specialname static 
          string  get_Programs() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21e8
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_Programs

  .method /*06000193*/ public specialname static 
          string  get_ProgramFiles() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21e9
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_ProgramFiles

  .method /*06000194*/ public specialname static 
          string  get_Temp() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21ea
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_Temp

  .method /*06000195*/ public specialname static 
          string  get_CurrentUserApplicationData() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21eb
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_CurrentUserApplicationData

  .method /*06000196*/ public specialname static 
          string  get_AllUsersApplicationData() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x21ec
    // Code size       0 (0x0)
  } // end of method SpecialDirectories::get_AllUsersApplicationData

  .property /*17000065*/ callconv(8) string
          MyDocuments()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_MyDocuments() /* 0600018E */
  } // end of property SpecialDirectories::MyDocuments
  .property /*17000066*/ callconv(8) string
          MyMusic()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_MyMusic() /* 0600018F */
  } // end of property SpecialDirectories::MyMusic
  .property /*17000067*/ callconv(8) string
          MyPictures()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_MyPictures() /* 06000190 */
  } // end of property SpecialDirectories::MyPictures
  .property /*17000068*/ callconv(8) string
          Desktop()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_Desktop() /* 06000191 */
  } // end of property SpecialDirectories::Desktop
  .property /*17000069*/ callconv(8) string
          Programs()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_Programs() /* 06000192 */
  } // end of property SpecialDirectories::Programs
  .property /*1700006A*/ callconv(8) string
          ProgramFiles()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_ProgramFiles() /* 06000193 */
  } // end of property SpecialDirectories::ProgramFiles
  .property /*1700006B*/ callconv(8) string
          Temp()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_Temp() /* 06000194 */
  } // end of property SpecialDirectories::Temp
  .property /*1700006C*/ callconv(8) string
          CurrentUserApplicationData()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_CurrentUserApplicationData() /* 06000195 */
  } // end of property SpecialDirectories::CurrentUserApplicationData
  .property /*1700006D*/ callconv(8) string
          AllUsersApplicationData()
  {
    .get string Microsoft.VisualBasic.FileIO.SpecialDirectories/*02000030*/::get_AllUsersApplicationData() /* 06000196 */
  } // end of property SpecialDirectories::AllUsersApplicationData
} // end of class Microsoft.VisualBasic.FileIO.SpecialDirectories

.class /*02000031*/ public auto ansi Microsoft.VisualBasic.FileIO.TextFieldParser
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements [mscorlib/*23000001*/]System.IDisposable/*0100002F*/
{
  .method /*06000197*/ public specialname rtspecialname 
          instance void  .ctor(string path) cil managed
  // SIG: 20 01 01 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x21ed
    // Code size       0 (0x0)
  } // end of method TextFieldParser::.ctor

  .method /*06000198*/ public specialname rtspecialname 
          instance void  .ctor(string path,
                               class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ defaultEncoding) cil managed
  // SIG: 20 02 01 0E 12 80 B5
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x21ee
    // Code size       0 (0x0)
  } // end of method TextFieldParser::.ctor

  .method /*06000199*/ public specialname rtspecialname 
          instance void  .ctor(string path,
                               class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ defaultEncoding,
                               bool detectEncoding) cil managed
  // SIG: 20 03 01 0E 12 80 B5 02
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x21ef
    // Code size       0 (0x0)
  } // end of method TextFieldParser::.ctor

  .method /*0600019A*/ public specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*01000021*/ 'stream') cil managed
  // SIG: 20 01 01 12 80 85
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x21f0
    // Code size       0 (0x0)
  } // end of method TextFieldParser::.ctor

  .method /*0600019B*/ public specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*01000021*/ 'stream',
                               class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ defaultEncoding) cil managed
  // SIG: 20 02 01 12 80 85 12 80 B5
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x21f1
    // Code size       0 (0x0)
  } // end of method TextFieldParser::.ctor

  .method /*0600019C*/ public specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*01000021*/ 'stream',
                               class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ defaultEncoding,
                               bool detectEncoding) cil managed
  // SIG: 20 03 01 12 80 85 12 80 B5 02
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x21f2
    // Code size       0 (0x0)
  } // end of method TextFieldParser::.ctor

  .method /*0600019D*/ public specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*01000021*/ 'stream',
                               class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ defaultEncoding,
                               bool detectEncoding,
                               bool leaveOpen) cil managed
  // SIG: 20 04 01 12 80 85 12 80 B5 02 02
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x21f3
    // Code size       0 (0x0)
  } // end of method TextFieldParser::.ctor

  .method /*0600019E*/ public specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.TextReader/*01000030*/ reader) cil managed
  // SIG: 20 01 01 12 80 C1
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x21f4
    // Code size       0 (0x0)
  } // end of method TextFieldParser::.ctor

  .method /*0600019F*/ public specialname 
          instance string[]  get_CommentTokens() cil managed
  // SIG: 20 00 1D 0E
  {
    // Method begins at RVA 0x21f5
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_CommentTokens

  .method /*060001A0*/ public specialname 
          instance void  set_CommentTokens(string[] 'value') cil managed
  // SIG: 20 01 01 1D 0E
  {
    // Method begins at RVA 0x21f6
    // Code size       0 (0x0)
  } // end of method TextFieldParser::set_CommentTokens

  .method /*060001A1*/ public specialname 
          instance bool  get_EndOfData() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f7
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_EndOfData

  .method /*060001A2*/ public specialname 
          instance int64  get_LineNumber() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21f8
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_LineNumber

  .method /*060001A3*/ public specialname 
          instance string  get_ErrorLine() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f9
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_ErrorLine

  .method /*060001A4*/ public specialname 
          instance int64  get_ErrorLineNumber() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21fa
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_ErrorLineNumber

  .method /*060001A5*/ public specialname 
          instance valuetype Microsoft.VisualBasic.FileIO.FieldType/*02000032*/ 
          get_TextFieldType() cil managed
  // SIG: 20 00 11 80 C8
  {
    // Method begins at RVA 0x21fb
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_TextFieldType

  .method /*060001A6*/ public specialname 
          instance void  set_TextFieldType(valuetype Microsoft.VisualBasic.FileIO.FieldType/*02000032*/ 'value') cil managed
  // SIG: 20 01 01 11 80 C8
  {
    // Method begins at RVA 0x21fc
    // Code size       0 (0x0)
  } // end of method TextFieldParser::set_TextFieldType

  .method /*060001A7*/ public specialname 
          instance int32[]  get_FieldWidths() cil managed
  // SIG: 20 00 1D 08
  {
    // Method begins at RVA 0x21fd
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_FieldWidths

  .method /*060001A8*/ public specialname 
          instance void  set_FieldWidths(int32[] 'value') cil managed
  // SIG: 20 01 01 1D 08
  {
    // Method begins at RVA 0x21fe
    // Code size       0 (0x0)
  } // end of method TextFieldParser::set_FieldWidths

  .method /*060001A9*/ public specialname 
          instance string[]  get_Delimiters() cil managed
  // SIG: 20 00 1D 0E
  {
    // Method begins at RVA 0x21ff
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_Delimiters

  .method /*060001AA*/ public specialname 
          instance void  set_Delimiters(string[] 'value') cil managed
  // SIG: 20 01 01 1D 0E
  {
    // Method begins at RVA 0x2200
    // Code size       0 (0x0)
  } // end of method TextFieldParser::set_Delimiters

  .method /*060001AB*/ public instance void 
          SetDelimiters(string[] delimiters) cil managed
  // SIG: 20 01 01 1D 0E
  {
    .param [1]/*0800020E*/ 
    .custom /*0C000095:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2201
    // Code size       0 (0x0)
  } // end of method TextFieldParser::SetDelimiters

  .method /*060001AC*/ public instance void 
          SetFieldWidths(int32[] fieldWidths) cil managed
  // SIG: 20 01 01 1D 08
  {
    .param [1]/*0800020F*/ 
    .custom /*0C000096:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2202
    // Code size       0 (0x0)
  } // end of method TextFieldParser::SetFieldWidths

  .method /*060001AD*/ public specialname 
          instance bool  get_TrimWhiteSpace() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2203
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_TrimWhiteSpace

  .method /*060001AE*/ public specialname 
          instance void  set_TrimWhiteSpace(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2204
    // Code size       0 (0x0)
  } // end of method TextFieldParser::set_TrimWhiteSpace

  .method /*060001AF*/ public instance string 
          ReadLine() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000092:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x2205
    // Code size       0 (0x0)
  } // end of method TextFieldParser::ReadLine

  .method /*060001B0*/ public instance string[] 
          ReadFields() cil managed
  // SIG: 20 00 1D 0E
  {
    // Method begins at RVA 0x2206
    // Code size       0 (0x0)
  } // end of method TextFieldParser::ReadFields

  .method /*060001B1*/ public instance string 
          PeekChars(int32 numberOfChars) cil managed
  // SIG: 20 01 0E 08
  {
    // Method begins at RVA 0x2207
    // Code size       0 (0x0)
  } // end of method TextFieldParser::PeekChars

  .method /*060001B2*/ public instance string 
          ReadToEnd() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000093:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x2208
    // Code size       0 (0x0)
  } // end of method TextFieldParser::ReadToEnd

  .method /*060001B3*/ public specialname 
          instance bool  get_HasFieldsEnclosedInQuotes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2209
    // Code size       0 (0x0)
  } // end of method TextFieldParser::get_HasFieldsEnclosedInQuotes

  .method /*060001B4*/ public specialname 
          instance void  set_HasFieldsEnclosedInQuotes(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x220a
    // Code size       0 (0x0)
  } // end of method TextFieldParser::set_HasFieldsEnclosedInQuotes

  .method /*060001B5*/ public instance void 
          Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x220b
    // Code size       0 (0x0)
  } // end of method TextFieldParser::Close

  .method /*060001B6*/ public newslot strict virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    .override [mscorlib/*23000001*/]System.IDisposable/*0100002F*/::Dispose /*0100002F::0A000013*/ 
    // Method begins at RVA 0x220c
    // Code size       0 (0x0)
  } // end of method TextFieldParser::Dispose

  .method /*060001B7*/ family newslot strict virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x220d
    // Code size       0 (0x0)
  } // end of method TextFieldParser::Dispose

  .method /*060001B8*/ family strict virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x220e
    // Code size       0 (0x0)
  } // end of method TextFieldParser::Finalize

  .property /*1700006E*/ instance callconv(8) string[]
          CommentTokens()
  {
    .custom /*0C000071:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get instance string[] Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_CommentTokens() /* 0600019F */
    .set instance void Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::set_CommentTokens(string[]) /* 060001A0 */
  } // end of property TextFieldParser::CommentTokens
  .property /*1700006F*/ instance callconv(8) bool
          EndOfData()
  {
    .get instance bool Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_EndOfData() /* 060001A1 */
  } // end of property TextFieldParser::EndOfData
  .property /*17000070*/ instance callconv(8) int64
          LineNumber()
  {
    .custom /*0C000076:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get instance int64 Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_LineNumber() /* 060001A2 */
  } // end of property TextFieldParser::LineNumber
  .property /*17000071*/ instance callconv(8) string
          ErrorLine()
  {
    .get instance string Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_ErrorLine() /* 060001A3 */
  } // end of property TextFieldParser::ErrorLine
  .property /*17000072*/ instance callconv(8) int64
          ErrorLineNumber()
  {
    .get instance int64 Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_ErrorLineNumber() /* 060001A4 */
  } // end of property TextFieldParser::ErrorLineNumber
  .property /*17000073*/ instance callconv(8) valuetype Microsoft.VisualBasic.FileIO.FieldType/*02000032*/
          TextFieldType()
  {
    .get instance valuetype Microsoft.VisualBasic.FileIO.FieldType/*02000032*/ Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_TextFieldType() /* 060001A5 */
    .set instance void Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::set_TextFieldType(valuetype Microsoft.VisualBasic.FileIO.FieldType/*02000032*/) /* 060001A6 */
  } // end of property TextFieldParser::TextFieldType
  .property /*17000074*/ instance callconv(8) int32[]
          FieldWidths()
  {
    .get instance int32[] Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_FieldWidths() /* 060001A7 */
    .set instance void Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::set_FieldWidths(int32[]) /* 060001A8 */
  } // end of property TextFieldParser::FieldWidths
  .property /*17000075*/ instance callconv(8) string[]
          Delimiters()
  {
    .get instance string[] Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_Delimiters() /* 060001A9 */
    .set instance void Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::set_Delimiters(string[]) /* 060001AA */
  } // end of property TextFieldParser::Delimiters
  .property /*17000076*/ instance callconv(8) bool
          TrimWhiteSpace()
  {
    .set instance void Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::set_TrimWhiteSpace(bool) /* 060001AE */
    .get instance bool Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_TrimWhiteSpace() /* 060001AD */
  } // end of property TextFieldParser::TrimWhiteSpace
  .property /*17000077*/ instance callconv(8) bool
          HasFieldsEnclosedInQuotes()
  {
    .custom /*0C000079:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get instance bool Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::get_HasFieldsEnclosedInQuotes() /* 060001B3 */
    .set instance void Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/::set_HasFieldsEnclosedInQuotes(bool) /* 060001B4 */
  } // end of property TextFieldParser::HasFieldsEnclosedInQuotes
} // end of class Microsoft.VisualBasic.FileIO.TextFieldParser

.class /*02000032*/ public auto ansi sealed Microsoft.VisualBasic.FileIO.FieldType
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000094*/ public specialname rtspecialname int32 value__
  .field /*04000095*/ public static literal valuetype Microsoft.VisualBasic.FileIO.FieldType/*02000032*/ Delimited = int32(0x00000000)
  .field /*04000096*/ public static literal valuetype Microsoft.VisualBasic.FileIO.FieldType/*02000032*/ FixedWidth = int32(0x00000001)
} // end of class Microsoft.VisualBasic.FileIO.FieldType

.class /*02000033*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.FileSystem
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00002E:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .permissionset linkcheck
             = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Unrestricted' = bool(true)},
                class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*060001B9*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x220f
    // Code size       0 (0x0)
  } // end of method FileSystem::.cctor

  .method /*060001BA*/ public static void 
          ChDir(string Path) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x2210
    // Code size       0 (0x0)
  } // end of method FileSystem::ChDir

  .method /*060001BB*/ public static void 
          ChDrive(char Drive) cil managed
  // SIG: 00 01 01 03
  {
    // Method begins at RVA 0x2211
    // Code size       0 (0x0)
  } // end of method FileSystem::ChDrive

  .method /*060001BC*/ public static void 
          ChDrive(string Drive) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x2212
    // Code size       0 (0x0)
  } // end of method FileSystem::ChDrive

  .method /*060001BD*/ public static string 
          CurDir() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2213
    // Code size       0 (0x0)
  } // end of method FileSystem::CurDir

  .method /*060001BE*/ public static string 
          CurDir(char Drive) cil managed
  // SIG: 00 01 0E 03
  {
    // Method begins at RVA 0x2214
    // Code size       0 (0x0)
  } // end of method FileSystem::CurDir

  .method /*060001BF*/ public static string 
          Dir() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2215
    // Code size       0 (0x0)
  } // end of method FileSystem::Dir

  .method /*060001C0*/ public static string 
          Dir(string PathName,
              [opt] valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ Attributes) cil managed
  // SIG: 00 02 0E 0E 11 80 EC
  {
    .param [2]/*08000219*/  = int32(0x00000000)
    // Method begins at RVA 0x2216
    // Code size       0 (0x0)
  } // end of method FileSystem::Dir

  .method /*060001C1*/ public static void 
          MkDir(string Path) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x2217
    // Code size       0 (0x0)
  } // end of method FileSystem::MkDir

  .method /*060001C2*/ public static void 
          RmDir(string Path) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x2218
    // Code size       0 (0x0)
  } // end of method FileSystem::RmDir

  .method /*060001C3*/ public static void 
          FileCopy(string Source,
                   string Destination) cil managed
  // SIG: 00 02 01 0E 0E
  {
    // Method begins at RVA 0x2219
    // Code size       0 (0x0)
  } // end of method FileSystem::FileCopy

  .method /*060001C4*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          FileDateTime(string PathName) cil managed
  // SIG: 00 01 11 80 81 0E
  {
    // Method begins at RVA 0x221a
    // Code size       0 (0x0)
  } // end of method FileSystem::FileDateTime

  .method /*060001C5*/ public static int64 
          FileLen(string PathName) cil managed
  // SIG: 00 01 0A 0E
  {
    // Method begins at RVA 0x221b
    // Code size       0 (0x0)
  } // end of method FileSystem::FileLen

  .method /*060001C6*/ public static valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ 
          GetAttr(string PathName) cil managed
  // SIG: 00 01 11 80 EC 0E
  {
    // Method begins at RVA 0x221c
    // Code size       0 (0x0)
  } // end of method FileSystem::GetAttr

  .method /*060001C7*/ public static void 
          Kill(string PathName) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x221d
    // Code size       0 (0x0)
  } // end of method FileSystem::Kill

  .method /*060001C8*/ public static void 
          SetAttr(string PathName,
                  valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ Attributes) cil managed
  // SIG: 00 02 01 0E 11 80 EC
  {
    // Method begins at RVA 0x221e
    // Code size       0 (0x0)
  } // end of method FileSystem::SetAttr

  .method /*060001C9*/ public static void 
          FileOpen(int32 FileNumber,
                   string FileName,
                   valuetype Microsoft.VisualBasic.OpenMode/*02000041*/ Mode,
                   [opt] valuetype Microsoft.VisualBasic.OpenAccess/*02000042*/ Access,
                   [opt] valuetype Microsoft.VisualBasic.OpenShare/*02000043*/ Share,
                   [opt] int32 RecordLength) cil managed
  // SIG: 00 06 01 08 0E 11 81 04 11 81 08 11 81 0C 08
  {
    .param [4]/*08000227*/  = int32(0xFFFFFFFF)
    .param [5]/*08000228*/  = int32(0xFFFFFFFF)
    .param [6]/*08000229*/  = int32(0xFFFFFFFF)
    // Method begins at RVA 0x221f
    // Code size       0 (0x0)
  } // end of method FileSystem::FileOpen

  .method /*060001CA*/ public static void 
          FileClose(int32[] FileNumbers) cil managed
  // SIG: 00 01 01 1D 08
  {
    .param [1]/*0800022A*/ 
    .custom /*0C000097:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2220
    // Code size       0 (0x0)
  } // end of method FileSystem::FileClose

  .method /*060001CB*/ public static void 
          FileGetObject(int32 FileNumber,
                        object& Value,
                        [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 1C 0A
  {
    .param [3]/*0800022D*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2221
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGetObject

  .method /*060001CC*/ public static void 
          FileGet(int32 FileNumber,
                  class [mscorlib/*23000001*/]System.ValueType/*01000031*/& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 12 80 C5 0A
  {
    .param [3]/*08000230*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2222
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001CD*/ public static void 
          FileGet(int32 FileNumber,
                  class [mscorlib/*23000001*/]System.Array/*0100001E*/& Value,
                  [opt] int64 RecordNumber,
                  [opt] bool ArrayIsDynamic,
                  [opt] bool StringIsFixedLength) cil managed
  // SIG: 00 05 01 08 10 12 79 0A 02 02
  {
    .param [3]/*08000233*/  = int64(0xFFFFFFFFFFFFFFFF)
    .param [4]/*08000234*/  = bool(false)
    .param [5]/*08000235*/  = bool(false)
    // Method begins at RVA 0x2223
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001CE*/ public static void 
          FileGet(int32 FileNumber,
                  bool& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 02 0A
  {
    .param [3]/*08000238*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2224
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001CF*/ public static void 
          FileGet(int32 FileNumber,
                  uint8& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 05 0A
  {
    .param [3]/*0800023B*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2225
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D0*/ public static void 
          FileGet(int32 FileNumber,
                  int16& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 06 0A
  {
    .param [3]/*0800023E*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2226
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D1*/ public static void 
          FileGet(int32 FileNumber,
                  int32& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 08 0A
  {
    .param [3]/*08000241*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2227
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D2*/ public static void 
          FileGet(int32 FileNumber,
                  int64& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 0A 0A
  {
    .param [3]/*08000244*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2228
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D3*/ public static void 
          FileGet(int32 FileNumber,
                  char& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 03 0A
  {
    .param [3]/*08000247*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2229
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D4*/ public static void 
          FileGet(int32 FileNumber,
                  float32& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 0C 0A
  {
    .param [3]/*0800024A*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x222a
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D5*/ public static void 
          FileGet(int32 FileNumber,
                  float64& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 0D 0A
  {
    .param [3]/*0800024D*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x222b
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D6*/ public static void 
          FileGet(int32 FileNumber,
                  valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 11 7D 0A
  {
    .param [3]/*08000250*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x222c
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D7*/ public static void 
          FileGet(int32 FileNumber,
                  string& Value,
                  [opt] int64 RecordNumber,
                  [opt] bool StringIsFixedLength) cil managed
  // SIG: 00 04 01 08 10 0E 0A 02
  {
    .param [3]/*08000253*/  = int64(0xFFFFFFFFFFFFFFFF)
    .param [4]/*08000254*/  = bool(false)
    // Method begins at RVA 0x222d
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D8*/ public static void 
          FileGet(int32 FileNumber,
                  valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/& Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 10 11 80 81 0A
  {
    .param [3]/*08000257*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x222e
    // Code size       0 (0x0)
  } // end of method FileSystem::FileGet

  .method /*060001D9*/ public static void 
          FilePutObject(int32 FileNumber,
                        object Value,
                        [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 1C 0A
  {
    .param [3]/*0800025A*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x222f
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePutObject

  .method /*060001DA*/ public static void 
          FilePut(object FileNumber,
                  object Value,
                  [opt] object RecordNumber) cil managed
  // SIG: 00 03 01 1C 1C 1C
  {
    .custom /*0C000094:0A00002E*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*0100005F*/::.ctor(string) /* 0A00002E */ = ( 01 00 80 CC 54 68 69 73 20 6D 65 6D 62 65 72 20   // ....This member 
                                                                                                                                             68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63 61   // has been depreca
                                                                                                                                             74 65 64 2E 20 50 6C 65 61 73 65 20 75 73 65 20   // ted. Please use 
                                                                                                                                             46 69 6C 65 50 75 74 4F 62 6A 65 63 74 20 74 6F   // FilePutObject to
                                                                                                                                             20 77 72 69 74 65 20 4F 62 6A 65 63 74 20 74 79   //  write Object ty
                                                                                                                                             70 65 73 2C 20 6F 72 20 63 6F 65 72 63 65 20 46   // pes, or coerce F
                                                                                                                                             69 6C 65 4E 75 6D 62 65 72 20 61 6E 64 20 52 65   // ileNumber and Re
                                                                                                                                             63 6F 72 64 4E 75 6D 62 65 72 20 74 6F 20 49 6E   // cordNumber to In
                                                                                                                                             74 65 67 65 72 20 66 6F 72 20 77 72 69 74 69 6E   // teger for writin
                                                                                                                                             67 20 6E 6F 6E 2D 4F 62 6A 65 63 74 20 74 79 70   // g non-Object typ
                                                                                                                                             65 73 2E 20 68 74 74 70 3A 2F 2F 67 6F 2E 6D 69   // es. http://go.mi
                                                                                                                                             63 72 6F 73 6F 66 74 2E 63 6F 6D 2F 66 77 6C 69   // crosoft.com/fwli
                                                                                                                                             6E 6B 2F 3F 6C 69 6E 6B 69 64 3D 31 34 32 30 32   // nk/?linkid=14202
                                                                                                                                             00 00 ) 
    .param [3]/*0800025D*/  = int32(0xFFFFFFFF)
    // Method begins at RVA 0x2230
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001DB*/ public static void 
          FilePut(int32 FileNumber,
                  class [mscorlib/*23000001*/]System.ValueType/*01000031*/ Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 12 80 C5 0A
  {
    .param [3]/*08000260*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2231
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001DC*/ public static void 
          FilePut(int32 FileNumber,
                  class [mscorlib/*23000001*/]System.Array/*0100001E*/ Value,
                  [opt] int64 RecordNumber,
                  [opt] bool ArrayIsDynamic,
                  [opt] bool StringIsFixedLength) cil managed
  // SIG: 00 05 01 08 12 79 0A 02 02
  {
    .param [3]/*08000263*/  = int64(0xFFFFFFFFFFFFFFFF)
    .param [4]/*08000264*/  = bool(false)
    .param [5]/*08000265*/  = bool(false)
    // Method begins at RVA 0x2232
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001DD*/ public static void 
          FilePut(int32 FileNumber,
                  bool Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 02 0A
  {
    .param [3]/*08000268*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2233
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001DE*/ public static void 
          FilePut(int32 FileNumber,
                  uint8 Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 05 0A
  {
    .param [3]/*0800026B*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2234
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001DF*/ public static void 
          FilePut(int32 FileNumber,
                  int16 Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 06 0A
  {
    .param [3]/*0800026E*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2235
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E0*/ public static void 
          FilePut(int32 FileNumber,
                  int32 Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 08 0A
  {
    .param [3]/*08000271*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2236
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E1*/ public static void 
          FilePut(int32 FileNumber,
                  int64 Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 0A 0A
  {
    .param [3]/*08000274*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2237
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E2*/ public static void 
          FilePut(int32 FileNumber,
                  char Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 03 0A
  {
    .param [3]/*08000277*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2238
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E3*/ public static void 
          FilePut(int32 FileNumber,
                  float32 Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 0C 0A
  {
    .param [3]/*0800027A*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x2239
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E4*/ public static void 
          FilePut(int32 FileNumber,
                  float64 Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 0D 0A
  {
    .param [3]/*0800027D*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x223a
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E5*/ public static void 
          FilePut(int32 FileNumber,
                  valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 11 7D 0A
  {
    .param [3]/*08000280*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x223b
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E6*/ public static void 
          FilePut(int32 FileNumber,
                  string Value,
                  [opt] int64 RecordNumber,
                  [opt] bool StringIsFixedLength) cil managed
  // SIG: 00 04 01 08 0E 0A 02
  {
    .param [3]/*08000283*/  = int64(0xFFFFFFFFFFFFFFFF)
    .param [4]/*08000284*/  = bool(false)
    // Method begins at RVA 0x223c
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E7*/ public static void 
          FilePut(int32 FileNumber,
                  valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Value,
                  [opt] int64 RecordNumber) cil managed
  // SIG: 00 03 01 08 11 80 81 0A
  {
    .param [3]/*08000287*/  = int64(0xFFFFFFFFFFFFFFFF)
    // Method begins at RVA 0x223d
    // Code size       0 (0x0)
  } // end of method FileSystem::FilePut

  .method /*060001E8*/ public static void 
          Print(int32 FileNumber,
                object[] Output) cil managed
  // SIG: 00 02 01 08 1D 1C
  {
    .param [2]/*08000289*/ 
    .custom /*0C0000A2:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x223e
    // Code size       0 (0x0)
  } // end of method FileSystem::Print

  .method /*060001E9*/ public static void 
          PrintLine(int32 FileNumber,
                    object[] Output) cil managed
  // SIG: 00 02 01 08 1D 1C
  {
    .param [2]/*0800028B*/ 
    .custom /*0C0000A3:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x223f
    // Code size       0 (0x0)
  } // end of method FileSystem::PrintLine

  .method /*060001EA*/ public static void 
          Input(int32 FileNumber,
                object& Value) cil managed
  // SIG: 00 02 01 08 10 1C
  {
    // Method begins at RVA 0x2240
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001EB*/ public static void 
          Input(int32 FileNumber,
                bool& Value) cil managed
  // SIG: 00 02 01 08 10 02
  {
    // Method begins at RVA 0x2241
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001EC*/ public static void 
          Input(int32 FileNumber,
                uint8& Value) cil managed
  // SIG: 00 02 01 08 10 05
  {
    // Method begins at RVA 0x2242
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001ED*/ public static void 
          Input(int32 FileNumber,
                int16& Value) cil managed
  // SIG: 00 02 01 08 10 06
  {
    // Method begins at RVA 0x2243
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001EE*/ public static void 
          Input(int32 FileNumber,
                int32& Value) cil managed
  // SIG: 00 02 01 08 10 08
  {
    // Method begins at RVA 0x2244
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001EF*/ public static void 
          Input(int32 FileNumber,
                int64& Value) cil managed
  // SIG: 00 02 01 08 10 0A
  {
    // Method begins at RVA 0x2245
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001F0*/ public static void 
          Input(int32 FileNumber,
                char& Value) cil managed
  // SIG: 00 02 01 08 10 03
  {
    // Method begins at RVA 0x2246
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001F1*/ public static void 
          Input(int32 FileNumber,
                float32& Value) cil managed
  // SIG: 00 02 01 08 10 0C
  {
    // Method begins at RVA 0x2247
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001F2*/ public static void 
          Input(int32 FileNumber,
                float64& Value) cil managed
  // SIG: 00 02 01 08 10 0D
  {
    // Method begins at RVA 0x2248
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001F3*/ public static void 
          Input(int32 FileNumber,
                valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/& Value) cil managed
  // SIG: 00 02 01 08 10 11 7D
  {
    // Method begins at RVA 0x2249
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001F4*/ public static void 
          Input(int32 FileNumber,
                string& Value) cil managed
  // SIG: 00 02 01 08 10 0E
  {
    // Method begins at RVA 0x224a
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001F5*/ public static void 
          Input(int32 FileNumber,
                valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/& Value) cil managed
  // SIG: 00 02 01 08 10 11 80 81
  {
    // Method begins at RVA 0x224b
    // Code size       0 (0x0)
  } // end of method FileSystem::Input

  .method /*060001F6*/ public static void 
          Write(int32 FileNumber,
                object[] Output) cil managed
  // SIG: 00 02 01 08 1D 1C
  {
    .param [2]/*080002A5*/ 
    .custom /*0C0000A7:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x224c
    // Code size       0 (0x0)
  } // end of method FileSystem::Write

  .method /*060001F7*/ public static void 
          WriteLine(int32 FileNumber,
                    object[] Output) cil managed
  // SIG: 00 02 01 08 1D 1C
  {
    .param [2]/*080002A7*/ 
    .custom /*0C0000A8:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x224d
    // Code size       0 (0x0)
  } // end of method FileSystem::WriteLine

  .method /*060001F8*/ public static string 
          InputString(int32 FileNumber,
                      int32 CharCount) cil managed
  // SIG: 00 02 0E 08 08
  {
    // Method begins at RVA 0x224e
    // Code size       0 (0x0)
  } // end of method FileSystem::InputString

  .method /*060001F9*/ public static string 
          LineInput(int32 FileNumber) cil managed
  // SIG: 00 01 0E 08
  {
    // Method begins at RVA 0x224f
    // Code size       0 (0x0)
  } // end of method FileSystem::LineInput

  .method /*060001FA*/ public static void 
          Lock(int32 FileNumber) cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2250
    // Code size       0 (0x0)
  } // end of method FileSystem::Lock

  .method /*060001FB*/ public static void 
          Lock(int32 FileNumber,
               int64 Record) cil managed
  // SIG: 00 02 01 08 0A
  {
    // Method begins at RVA 0x2251
    // Code size       0 (0x0)
  } // end of method FileSystem::Lock

  .method /*060001FC*/ public static void 
          Lock(int32 FileNumber,
               int64 FromRecord,
               int64 ToRecord) cil managed
  // SIG: 00 03 01 08 0A 0A
  {
    // Method begins at RVA 0x2252
    // Code size       0 (0x0)
  } // end of method FileSystem::Lock

  .method /*060001FD*/ public static void 
          Unlock(int32 FileNumber) cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2253
    // Code size       0 (0x0)
  } // end of method FileSystem::Unlock

  .method /*060001FE*/ public static void 
          Unlock(int32 FileNumber,
                 int64 Record) cil managed
  // SIG: 00 02 01 08 0A
  {
    // Method begins at RVA 0x2254
    // Code size       0 (0x0)
  } // end of method FileSystem::Unlock

  .method /*060001FF*/ public static void 
          Unlock(int32 FileNumber,
                 int64 FromRecord,
                 int64 ToRecord) cil managed
  // SIG: 00 03 01 08 0A 0A
  {
    // Method begins at RVA 0x2255
    // Code size       0 (0x0)
  } // end of method FileSystem::Unlock

  .method /*06000200*/ public static void 
          FileWidth(int32 FileNumber,
                    int32 RecordWidth) cil managed
  // SIG: 00 02 01 08 08
  {
    // Method begins at RVA 0x2256
    // Code size       0 (0x0)
  } // end of method FileSystem::FileWidth

  .method /*06000201*/ public static int32 
          FreeFile() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2257
    // Code size       0 (0x0)
  } // end of method FileSystem::FreeFile

  .method /*06000202*/ public static void 
          Seek(int32 FileNumber,
               int64 Position) cil managed
  // SIG: 00 02 01 08 0A
  {
    // Method begins at RVA 0x2258
    // Code size       0 (0x0)
  } // end of method FileSystem::Seek

  .method /*06000203*/ public static int64 
          Seek(int32 FileNumber) cil managed
  // SIG: 00 01 0A 08
  {
    // Method begins at RVA 0x2259
    // Code size       0 (0x0)
  } // end of method FileSystem::Seek

  .method /*06000204*/ public static bool 
          EOF(int32 FileNumber) cil managed
  // SIG: 00 01 02 08
  {
    // Method begins at RVA 0x225a
    // Code size       0 (0x0)
  } // end of method FileSystem::EOF

  .method /*06000205*/ public static int64 
          Loc(int32 FileNumber) cil managed
  // SIG: 00 01 0A 08
  {
    // Method begins at RVA 0x225b
    // Code size       0 (0x0)
  } // end of method FileSystem::Loc

  .method /*06000206*/ public static int64 
          LOF(int32 FileNumber) cil managed
  // SIG: 00 01 0A 08
  {
    // Method begins at RVA 0x225c
    // Code size       0 (0x0)
  } // end of method FileSystem::LOF

  .method /*06000207*/ public static valuetype Microsoft.VisualBasic.TabInfo/*02000044*/ 
          TAB() cil managed
  // SIG: 00 00 11 81 10
  {
    // Method begins at RVA 0x225d
    // Code size       0 (0x0)
  } // end of method FileSystem::TAB

  .method /*06000208*/ public static valuetype Microsoft.VisualBasic.TabInfo/*02000044*/ 
          TAB(int16 Column) cil managed
  // SIG: 00 01 11 81 10 06
  {
    // Method begins at RVA 0x225e
    // Code size       0 (0x0)
  } // end of method FileSystem::TAB

  .method /*06000209*/ public static valuetype Microsoft.VisualBasic.SpcInfo/*02000045*/ 
          SPC(int16 Count) cil managed
  // SIG: 00 01 11 81 14 06
  {
    // Method begins at RVA 0x225f
    // Code size       0 (0x0)
  } // end of method FileSystem::SPC

  .method /*0600020A*/ public static valuetype Microsoft.VisualBasic.OpenMode/*02000041*/ 
          FileAttr(int32 FileNumber) cil managed
  // SIG: 00 01 11 81 04 08
  {
    // Method begins at RVA 0x2260
    // Code size       0 (0x0)
  } // end of method FileSystem::FileAttr

  .method /*0600020B*/ public static void 
          Reset() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2261
    // Code size       0 (0x0)
  } // end of method FileSystem::Reset

  .method /*0600020C*/ public static void 
          Rename(string OldPath,
                 string NewPath) cil managed
  // SIG: 00 02 01 0E 0E
  {
    // Method begins at RVA 0x2262
    // Code size       0 (0x0)
  } // end of method FileSystem::Rename

  .method /*0600020D*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2263
    // Code size       0 (0x0)
  } // end of method FileSystem::.ctor

} // end of class Microsoft.VisualBasic.FileSystem

.class /*02000034*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.Financial
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00002F:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .method /*0600020E*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2264
    // Code size       0 (0x0)
  } // end of method Financial::.cctor

  .method /*0600020F*/ public static float64 
          DDB(float64 Cost,
              float64 Salvage,
              float64 Life,
              float64 Period,
              [opt] float64 Factor) cil managed
  // SIG: 00 05 0D 0D 0D 0D 0D 0D
  {
    .param [5]/*080002C8*/  = float64(2)
    // Method begins at RVA 0x2265
    // Code size       0 (0x0)
  } // end of method Financial::DDB

  .method /*06000210*/ public static float64 
          FV(float64 Rate,
             float64 NPer,
             float64 Pmt,
             [opt] float64 PV,
             [opt] valuetype Microsoft.VisualBasic.DueDate/*02000040*/ Due) cil managed
  // SIG: 00 05 0D 0D 0D 0D 0D 11 81 00
  {
    .param [4]/*080002CC*/  = float64(0)
    .param [5]/*080002CD*/  = int32(0x00000000)
    // Method begins at RVA 0x2266
    // Code size       0 (0x0)
  } // end of method Financial::FV

  .method /*06000211*/ public static float64 
          IPmt(float64 Rate,
               float64 Per,
               float64 NPer,
               float64 PV,
               [opt] float64 FV,
               [opt] valuetype Microsoft.VisualBasic.DueDate/*02000040*/ Due) cil managed
  // SIG: 00 06 0D 0D 0D 0D 0D 0D 11 81 00
  {
    .param [5]/*080002D2*/  = float64(0)
    .param [6]/*080002D3*/  = int32(0x00000000)
    // Method begins at RVA 0x2267
    // Code size       0 (0x0)
  } // end of method Financial::IPmt

  .method /*06000212*/ public static float64 
          IRR(float64[]& ValueArray,
              [opt] float64 Guess) cil managed
  // SIG: 00 02 0D 10 1D 0D 0D
  {
    .param [2]/*080002D5*/  = float64(0.10000000000000001)
    // Method begins at RVA 0x2268
    // Code size       0 (0x0)
  } // end of method Financial::IRR

  .method /*06000213*/ public static float64 
          MIRR(float64[]& ValueArray,
               float64 FinanceRate,
               float64 ReinvestRate) cil managed
  // SIG: 00 03 0D 10 1D 0D 0D 0D
  {
    // Method begins at RVA 0x2269
    // Code size       0 (0x0)
  } // end of method Financial::MIRR

  .method /*06000214*/ public static float64 
          NPer(float64 Rate,
               float64 Pmt,
               float64 PV,
               [opt] float64 FV,
               [opt] valuetype Microsoft.VisualBasic.DueDate/*02000040*/ Due) cil managed
  // SIG: 00 05 0D 0D 0D 0D 0D 11 81 00
  {
    .param [4]/*080002DC*/  = float64(0)
    .param [5]/*080002DD*/  = int32(0x00000000)
    // Method begins at RVA 0x226a
    // Code size       0 (0x0)
  } // end of method Financial::NPer

  .method /*06000215*/ public static float64 
          NPV(float64 Rate,
              float64[]& ValueArray) cil managed
  // SIG: 00 02 0D 0D 10 1D 0D
  {
    // Method begins at RVA 0x226b
    // Code size       0 (0x0)
  } // end of method Financial::NPV

  .method /*06000216*/ public static float64 
          Pmt(float64 Rate,
              float64 NPer,
              float64 PV,
              [opt] float64 FV,
              [opt] valuetype Microsoft.VisualBasic.DueDate/*02000040*/ Due) cil managed
  // SIG: 00 05 0D 0D 0D 0D 0D 11 81 00
  {
    .param [4]/*080002E3*/  = float64(0)
    .param [5]/*080002E4*/  = int32(0x00000000)
    // Method begins at RVA 0x226c
    // Code size       0 (0x0)
  } // end of method Financial::Pmt

  .method /*06000217*/ public static float64 
          PPmt(float64 Rate,
               float64 Per,
               float64 NPer,
               float64 PV,
               [opt] float64 FV,
               [opt] valuetype Microsoft.VisualBasic.DueDate/*02000040*/ Due) cil managed
  // SIG: 00 06 0D 0D 0D 0D 0D 0D 11 81 00
  {
    .param [5]/*080002E9*/  = float64(0)
    .param [6]/*080002EA*/  = int32(0x00000000)
    // Method begins at RVA 0x226d
    // Code size       0 (0x0)
  } // end of method Financial::PPmt

  .method /*06000218*/ public static float64 
          PV(float64 Rate,
             float64 NPer,
             float64 Pmt,
             [opt] float64 FV,
             [opt] valuetype Microsoft.VisualBasic.DueDate/*02000040*/ Due) cil managed
  // SIG: 00 05 0D 0D 0D 0D 0D 11 81 00
  {
    .param [4]/*080002EE*/  = float64(0)
    .param [5]/*080002EF*/  = int32(0x00000000)
    // Method begins at RVA 0x226e
    // Code size       0 (0x0)
  } // end of method Financial::PV

  .method /*06000219*/ public static float64 
          Rate(float64 NPer,
               float64 Pmt,
               float64 PV,
               [opt] float64 FV,
               [opt] valuetype Microsoft.VisualBasic.DueDate/*02000040*/ Due,
               [opt] float64 Guess) cil managed
  // SIG: 00 06 0D 0D 0D 0D 0D 11 81 00 0D
  {
    .param [4]/*080002F3*/  = float64(0)
    .param [5]/*080002F4*/  = int32(0x00000000)
    .param [6]/*080002F5*/  = float64(0.10000000000000001)
    // Method begins at RVA 0x226f
    // Code size       0 (0x0)
  } // end of method Financial::Rate

  .method /*0600021A*/ public static float64 
          SLN(float64 Cost,
              float64 Salvage,
              float64 Life) cil managed
  // SIG: 00 03 0D 0D 0D 0D
  {
    // Method begins at RVA 0x2270
    // Code size       0 (0x0)
  } // end of method Financial::SLN

  .method /*0600021B*/ public static float64 
          SYD(float64 Cost,
              float64 Salvage,
              float64 Life,
              float64 Period) cil managed
  // SIG: 00 04 0D 0D 0D 0D 0D
  {
    // Method begins at RVA 0x2271
    // Code size       0 (0x0)
  } // end of method Financial::SYD

  .method /*0600021C*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2272
    // Code size       0 (0x0)
  } // end of method Financial::.ctor

} // end of class Microsoft.VisualBasic.Financial

.class /*02000035*/ public auto ansi sealed Microsoft.VisualBasic.VariantType
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000097*/ public specialname rtspecialname int32 value__
  .field /*04000098*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Empty = int32(0x00000000)
  .field /*04000099*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Null = int32(0x00000001)
  .field /*0400009A*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Short = int32(0x00000002)
  .field /*0400009B*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Integer = int32(0x00000003)
  .field /*0400009C*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Single = int32(0x00000004)
  .field /*0400009D*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Double = int32(0x00000005)
  .field /*0400009E*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Currency = int32(0x00000006)
  .field /*0400009F*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Date = int32(0x00000007)
  .field /*040000A0*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ String = int32(0x00000008)
  .field /*040000A1*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Object = int32(0x00000009)
  .field /*040000A2*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Error = int32(0x0000000A)
  .field /*040000A3*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Boolean = int32(0x0000000B)
  .field /*040000A4*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Variant = int32(0x0000000C)
  .field /*040000A5*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ DataObject = int32(0x0000000D)
  .field /*040000A6*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Decimal = int32(0x0000000E)
  .field /*040000A7*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Byte = int32(0x00000011)
  .field /*040000A8*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Char = int32(0x00000012)
  .field /*040000A9*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Long = int32(0x00000014)
  .field /*040000AA*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ UserDefinedType = int32(0x00000024)
  .field /*040000AB*/ public static literal valuetype Microsoft.VisualBasic.VariantType/*02000035*/ Array = int32(0x00002000)
} // end of class Microsoft.VisualBasic.VariantType

.class /*02000036*/ public auto ansi sealed Microsoft.VisualBasic.AppWinStyle
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000AC*/ public specialname rtspecialname int16 value__
  .field /*040000AD*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ Hide = int16(0x0000)
  .field /*040000AE*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ NormalFocus = int16(0x0001)
  .field /*040000AF*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ MinimizedFocus = int16(0x0002)
  .field /*040000B0*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ MaximizedFocus = int16(0x0003)
  .field /*040000B1*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ NormalNoFocus = int16(0x0004)
  .field /*040000B2*/ public static literal valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ MinimizedNoFocus = int16(0x0006)
} // end of class Microsoft.VisualBasic.AppWinStyle

.class /*02000037*/ public auto ansi sealed Microsoft.VisualBasic.CallType
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000B3*/ public specialname rtspecialname int32 value__
  .field /*040000B4*/ public static literal valuetype Microsoft.VisualBasic.CallType/*02000037*/ Method = int32(0x00000001)
  .field /*040000B5*/ public static literal valuetype Microsoft.VisualBasic.CallType/*02000037*/ Get = int32(0x00000002)
  .field /*040000B6*/ public static literal valuetype Microsoft.VisualBasic.CallType/*02000037*/ Let = int32(0x00000004)
  .field /*040000B7*/ public static literal valuetype Microsoft.VisualBasic.CallType/*02000037*/ Set = int32(0x00000008)
} // end of class Microsoft.VisualBasic.CallType

.class /*02000038*/ public auto ansi sealed Microsoft.VisualBasic.CompareMethod
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000B8*/ public specialname rtspecialname int32 value__
  .field /*040000B9*/ public static literal valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Binary = int32(0x00000000)
  .field /*040000BA*/ public static literal valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Text = int32(0x00000001)
} // end of class Microsoft.VisualBasic.CompareMethod

.class /*02000039*/ public auto ansi sealed Microsoft.VisualBasic.DateFormat
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000BB*/ public specialname rtspecialname int32 value__
  .field /*040000BC*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ GeneralDate = int32(0x00000000)
  .field /*040000BD*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ LongDate = int32(0x00000001)
  .field /*040000BE*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ ShortDate = int32(0x00000002)
  .field /*040000BF*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ LongTime = int32(0x00000003)
  .field /*040000C0*/ public static literal valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ ShortTime = int32(0x00000004)
} // end of class Microsoft.VisualBasic.DateFormat

.class /*0200003A*/ public auto ansi sealed Microsoft.VisualBasic.FirstDayOfWeek
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000C1*/ public specialname rtspecialname int32 value__
  .field /*040000C2*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ System = int32(0x00000000)
  .field /*040000C3*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ Sunday = int32(0x00000001)
  .field /*040000C4*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ Monday = int32(0x00000002)
  .field /*040000C5*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ Tuesday = int32(0x00000003)
  .field /*040000C6*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ Wednesday = int32(0x00000004)
  .field /*040000C7*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ Thursday = int32(0x00000005)
  .field /*040000C8*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ Friday = int32(0x00000006)
  .field /*040000C9*/ public static literal valuetype Microsoft.VisualBasic.FirstDayOfWeek/*0200003A*/ Saturday = int32(0x00000007)
} // end of class Microsoft.VisualBasic.FirstDayOfWeek

.class /*0200003B*/ public auto ansi sealed Microsoft.VisualBasic.FileAttribute
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .custom /*0C000030:0A00002F*/ instance void [mscorlib/*23000001*/]System.FlagsAttribute/*01000060*/::.ctor() /* 0A00002F */ = ( 01 00 00 00 ) 
  .field /*040000CA*/ public specialname rtspecialname int32 value__
  .field /*040000CB*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ Normal = int32(0x00000000)
  .field /*040000CC*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ ReadOnly = int32(0x00000001)
  .field /*040000CD*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ Hidden = int32(0x00000002)
  .field /*040000CE*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ System = int32(0x00000004)
  .field /*040000CF*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ Volume = int32(0x00000008)
  .field /*040000D0*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ Directory = int32(0x00000010)
  .field /*040000D1*/ public static literal valuetype Microsoft.VisualBasic.FileAttribute/*0200003B*/ Archive = int32(0x00000020)
} // end of class Microsoft.VisualBasic.FileAttribute

.class /*0200003C*/ public auto ansi sealed Microsoft.VisualBasic.FirstWeekOfYear
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000D2*/ public specialname rtspecialname int32 value__
  .field /*040000D3*/ public static literal valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ System = int32(0x00000000)
  .field /*040000D4*/ public static literal valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ Jan1 = int32(0x00000001)
  .field /*040000D5*/ public static literal valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ FirstFourDays = int32(0x00000002)
  .field /*040000D6*/ public static literal valuetype Microsoft.VisualBasic.FirstWeekOfYear/*0200003C*/ FirstFullWeek = int32(0x00000003)
} // end of class Microsoft.VisualBasic.FirstWeekOfYear

.class /*0200003D*/ public auto ansi sealed Microsoft.VisualBasic.VbStrConv
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .custom /*0C000031:0A00002F*/ instance void [mscorlib/*23000001*/]System.FlagsAttribute/*01000060*/::.ctor() /* 0A00002F */ = ( 01 00 00 00 ) 
  .field /*040000D7*/ public specialname rtspecialname int32 value__
  .field /*040000D8*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ None = int32(0x00000000)
  .field /*040000D9*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ Uppercase = int32(0x00000001)
  .field /*040000DA*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ Lowercase = int32(0x00000002)
  .field /*040000DB*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ ProperCase = int32(0x00000003)
  .field /*040000DC*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ Wide = int32(0x00000004)
  .field /*040000DD*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ Narrow = int32(0x00000008)
  .field /*040000DE*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ Katakana = int32(0x00000010)
  .field /*040000DF*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ Hiragana = int32(0x00000020)
  .field /*040000E0*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ SimplifiedChinese = int32(0x00000100)
  .field /*040000E1*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ TraditionalChinese = int32(0x00000200)
  .field /*040000E2*/ public static literal valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ LinguisticCasing = int32(0x00000400)
} // end of class Microsoft.VisualBasic.VbStrConv

.class /*0200003E*/ public auto ansi sealed Microsoft.VisualBasic.TriState
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000E3*/ public specialname rtspecialname int32 value__
  .field /*040000E4*/ public static literal valuetype Microsoft.VisualBasic.TriState/*0200003E*/ False = int32(0x00000000)
  .field /*040000E5*/ public static literal valuetype Microsoft.VisualBasic.TriState/*0200003E*/ True = int32(0xFFFFFFFF)
  .field /*040000E6*/ public static literal valuetype Microsoft.VisualBasic.TriState/*0200003E*/ UseDefault = int32(0xFFFFFFFE)
} // end of class Microsoft.VisualBasic.TriState

.class /*0200003F*/ public auto ansi sealed Microsoft.VisualBasic.DateInterval
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000E7*/ public specialname rtspecialname int32 value__
  .field /*040000E8*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Year = int32(0x00000000)
  .field /*040000E9*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Quarter = int32(0x00000001)
  .field /*040000EA*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Month = int32(0x00000002)
  .field /*040000EB*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ DayOfYear = int32(0x00000003)
  .field /*040000EC*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Day = int32(0x00000004)
  .field /*040000ED*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ WeekOfYear = int32(0x00000005)
  .field /*040000EE*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Weekday = int32(0x00000006)
  .field /*040000EF*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Hour = int32(0x00000007)
  .field /*040000F0*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Minute = int32(0x00000008)
  .field /*040000F1*/ public static literal valuetype Microsoft.VisualBasic.DateInterval/*0200003F*/ Second = int32(0x00000009)
} // end of class Microsoft.VisualBasic.DateInterval

.class /*02000040*/ public auto ansi sealed Microsoft.VisualBasic.DueDate
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000F2*/ public specialname rtspecialname int32 value__
  .field /*040000F3*/ public static literal valuetype Microsoft.VisualBasic.DueDate/*02000040*/ EndOfPeriod = int32(0x00000000)
  .field /*040000F4*/ public static literal valuetype Microsoft.VisualBasic.DueDate/*02000040*/ BegOfPeriod = int32(0x00000001)
} // end of class Microsoft.VisualBasic.DueDate

.class /*02000041*/ public auto ansi sealed Microsoft.VisualBasic.OpenMode
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000F5*/ public specialname rtspecialname int32 value__
  .field /*040000F6*/ public static literal valuetype Microsoft.VisualBasic.OpenMode/*02000041*/ Input = int32(0x00000001)
  .field /*040000F7*/ public static literal valuetype Microsoft.VisualBasic.OpenMode/*02000041*/ Output = int32(0x00000002)
  .field /*040000F8*/ public static literal valuetype Microsoft.VisualBasic.OpenMode/*02000041*/ Random = int32(0x00000004)
  .field /*040000F9*/ public static literal valuetype Microsoft.VisualBasic.OpenMode/*02000041*/ Append = int32(0x00000008)
  .field /*040000FA*/ public static literal valuetype Microsoft.VisualBasic.OpenMode/*02000041*/ Binary = int32(0x00000020)
} // end of class Microsoft.VisualBasic.OpenMode

.class /*02000042*/ public auto ansi sealed Microsoft.VisualBasic.OpenAccess
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*040000FB*/ public specialname rtspecialname int32 value__
  .field /*040000FC*/ public static literal valuetype Microsoft.VisualBasic.OpenAccess/*02000042*/ Default = int32(0xFFFFFFFF)
  .field /*040000FD*/ public static literal valuetype Microsoft.VisualBasic.OpenAccess/*02000042*/ Read = int32(0x00000001)
  .field /*040000FE*/ public static literal valuetype Microsoft.VisualBasic.OpenAccess/*02000042*/ ReadWrite = int32(0x00000003)
  .field /*040000FF*/ public static literal valuetype Microsoft.VisualBasic.OpenAccess/*02000042*/ Write = int32(0x00000002)
} // end of class Microsoft.VisualBasic.OpenAccess

.class /*02000043*/ public auto ansi sealed Microsoft.VisualBasic.OpenShare
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000100*/ public specialname rtspecialname int32 value__
  .field /*04000101*/ public static literal valuetype Microsoft.VisualBasic.OpenShare/*02000043*/ Default = int32(0xFFFFFFFF)
  .field /*04000102*/ public static literal valuetype Microsoft.VisualBasic.OpenShare/*02000043*/ Shared = int32(0x00000003)
  .field /*04000103*/ public static literal valuetype Microsoft.VisualBasic.OpenShare/*02000043*/ LockRead = int32(0x00000002)
  .field /*04000104*/ public static literal valuetype Microsoft.VisualBasic.OpenShare/*02000043*/ LockReadWrite = int32(0x00000000)
  .field /*04000105*/ public static literal valuetype Microsoft.VisualBasic.OpenShare/*02000043*/ LockWrite = int32(0x00000001)
} // end of class Microsoft.VisualBasic.OpenShare

.class /*02000044*/ public sequential ansi sealed Microsoft.VisualBasic.TabInfo
       extends [mscorlib/*23000001*/]System.ValueType/*01000031*/
{
  .custom /*0C000035:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000106*/ public int16 Column
} // end of class Microsoft.VisualBasic.TabInfo

.class /*02000045*/ public sequential ansi sealed Microsoft.VisualBasic.SpcInfo
       extends [mscorlib/*23000001*/]System.ValueType/*01000031*/
{
  .custom /*0C000037:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000107*/ public int16 Count
} // end of class Microsoft.VisualBasic.SpcInfo

.class /*02000046*/ public auto ansi sealed Microsoft.VisualBasic.MsgBoxResult
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000108*/ public specialname rtspecialname int32 value__
  .field /*04000109*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ Ok = int32(0x00000001)
  .field /*0400010A*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ Cancel = int32(0x00000002)
  .field /*0400010B*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ Abort = int32(0x00000003)
  .field /*0400010C*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ Retry = int32(0x00000004)
  .field /*0400010D*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ Ignore = int32(0x00000005)
  .field /*0400010E*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ Yes = int32(0x00000006)
  .field /*0400010F*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ No = int32(0x00000007)
} // end of class Microsoft.VisualBasic.MsgBoxResult

.class /*02000047*/ public auto ansi sealed Microsoft.VisualBasic.MsgBoxStyle
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .custom /*0C000039:0A00002F*/ instance void [mscorlib/*23000001*/]System.FlagsAttribute/*01000060*/::.ctor() /* 0A00002F */ = ( 01 00 00 00 ) 
  .field /*04000110*/ public specialname rtspecialname int32 value__
  .field /*04000111*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ OkOnly = int32(0x00000000)
  .field /*04000112*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ OkCancel = int32(0x00000001)
  .field /*04000113*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ AbortRetryIgnore = int32(0x00000002)
  .field /*04000114*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ YesNoCancel = int32(0x00000003)
  .field /*04000115*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ YesNo = int32(0x00000004)
  .field /*04000116*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ RetryCancel = int32(0x00000005)
  .field /*04000117*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ Critical = int32(0x00000010)
  .field /*04000118*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ Question = int32(0x00000020)
  .field /*04000119*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ Exclamation = int32(0x00000030)
  .field /*0400011A*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ Information = int32(0x00000040)
  .field /*0400011B*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ DefaultButton1 = int32(0x00000000)
  .field /*0400011C*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ DefaultButton2 = int32(0x00000100)
  .field /*0400011D*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ DefaultButton3 = int32(0x00000200)
  .field /*0400011E*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ ApplicationModal = int32(0x00000000)
  .field /*0400011F*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ SystemModal = int32(0x00001000)
  .field /*04000120*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ MsgBoxHelp = int32(0x00004000)
  .field /*04000121*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ MsgBoxRight = int32(0x00080000)
  .field /*04000122*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ MsgBoxRtlReading = int32(0x00100000)
  .field /*04000123*/ public static literal valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ MsgBoxSetForeground = int32(0x00010000)
} // end of class Microsoft.VisualBasic.MsgBoxStyle

.class /*02000048*/ public auto ansi sealed Microsoft.VisualBasic.VBFixedStringAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C00003A:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 00 01 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 00 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                              74 69 70 6C 65 00 )                               // tiple.
  .method /*0600021D*/ public specialname 
          instance int32  get_Length() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2273
    // Code size       0 (0x0)
  } // end of method VBFixedStringAttribute::get_Length

  .method /*0600021E*/ public specialname rtspecialname 
          instance void  .ctor(int32 Length) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2274
    // Code size       0 (0x0)
  } // end of method VBFixedStringAttribute::.ctor

  .property /*17000078*/ instance callconv(8) int32
          Length()
  {
    .get instance int32 Microsoft.VisualBasic.VBFixedStringAttribute/*02000048*/::get_Length() /* 0600021D */
  } // end of property VBFixedStringAttribute::Length
} // end of class Microsoft.VisualBasic.VBFixedStringAttribute

.class /*02000049*/ public auto ansi sealed Microsoft.VisualBasic.VBFixedArrayAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C00003B:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 00 01 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 00 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                              74 69 70 6C 65 00 )                               // tiple.
  .method /*0600021F*/ public specialname 
          instance int32[]  get_Bounds() cil managed
  // SIG: 20 00 1D 08
  {
    // Method begins at RVA 0x2275
    // Code size       0 (0x0)
  } // end of method VBFixedArrayAttribute::get_Bounds

  .method /*06000220*/ public specialname 
          instance int32  get_Length() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2276
    // Code size       0 (0x0)
  } // end of method VBFixedArrayAttribute::get_Length

  .method /*06000221*/ public specialname rtspecialname 
          instance void  .ctor(int32 UpperBound1) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2277
    // Code size       0 (0x0)
  } // end of method VBFixedArrayAttribute::.ctor

  .method /*06000222*/ public specialname rtspecialname 
          instance void  .ctor(int32 UpperBound1,
                               int32 UpperBound2) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x2278
    // Code size       0 (0x0)
  } // end of method VBFixedArrayAttribute::.ctor

  .property /*17000079*/ instance callconv(8) int32[]
          Bounds()
  {
    .get instance int32[] Microsoft.VisualBasic.VBFixedArrayAttribute/*02000049*/::get_Bounds() /* 0600021F */
  } // end of property VBFixedArrayAttribute::Bounds
  .property /*1700007A*/ instance callconv(8) int32
          Length()
  {
    .get instance int32 Microsoft.VisualBasic.VBFixedArrayAttribute/*02000049*/::get_Length() /* 06000220 */
  } // end of property VBFixedArrayAttribute::Length
} // end of class Microsoft.VisualBasic.VBFixedArrayAttribute

.class /*0200004A*/ public auto ansi sealed Microsoft.VisualBasic.ComClassAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C00003C:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 04 00 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 00 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                              74 69 70 6C 65 00 )                               // tiple.
  .method /*06000223*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2279
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::.ctor

  .method /*06000224*/ public specialname rtspecialname 
          instance void  .ctor(string _ClassID) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x227a
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::.ctor

  .method /*06000225*/ public specialname rtspecialname 
          instance void  .ctor(string _ClassID,
                               string _InterfaceID) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x227b
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::.ctor

  .method /*06000226*/ public specialname rtspecialname 
          instance void  .ctor(string _ClassID,
                               string _InterfaceID,
                               string _EventId) cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    // Method begins at RVA 0x227c
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::.ctor

  .method /*06000227*/ public specialname 
          instance string  get_ClassID() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x227d
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::get_ClassID

  .method /*06000228*/ public specialname 
          instance string  get_InterfaceID() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x227e
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::get_InterfaceID

  .method /*06000229*/ public specialname 
          instance string  get_EventID() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x227f
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::get_EventID

  .method /*0600022A*/ public specialname 
          instance bool  get_InterfaceShadows() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2280
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::get_InterfaceShadows

  .method /*0600022B*/ public specialname 
          instance void  set_InterfaceShadows(bool Value) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2281
    // Code size       0 (0x0)
  } // end of method ComClassAttribute::set_InterfaceShadows

  .property /*1700007B*/ instance callconv(8) string
          ClassID()
  {
    .get instance string Microsoft.VisualBasic.ComClassAttribute/*0200004A*/::get_ClassID() /* 06000227 */
  } // end of property ComClassAttribute::ClassID
  .property /*1700007C*/ instance callconv(8) string
          InterfaceID()
  {
    .get instance string Microsoft.VisualBasic.ComClassAttribute/*0200004A*/::get_InterfaceID() /* 06000228 */
  } // end of property ComClassAttribute::InterfaceID
  .property /*1700007D*/ instance callconv(8) string
          EventID()
  {
    .get instance string Microsoft.VisualBasic.ComClassAttribute/*0200004A*/::get_EventID() /* 06000229 */
  } // end of property ComClassAttribute::EventID
  .property /*1700007E*/ instance callconv(8) bool
          InterfaceShadows()
  {
    .get instance bool Microsoft.VisualBasic.ComClassAttribute/*0200004A*/::get_InterfaceShadows() /* 0600022A */
    .set instance void Microsoft.VisualBasic.ComClassAttribute/*0200004A*/::set_InterfaceShadows(bool) /* 0600022B */
  } // end of property ComClassAttribute::InterfaceShadows
} // end of class Microsoft.VisualBasic.ComClassAttribute

.class /*0200004B*/ public auto ansi sealed Microsoft.VisualBasic.MyGroupCollectionAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C00003E:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 00 )                               // rited.
  .custom /*0C00003F:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
  .method /*0600022C*/ public specialname rtspecialname 
          instance void  .ctor(string typeToCollect,
                               string createInstanceMethodName,
                               string disposeInstanceMethodName,
                               string defaultInstanceAlias) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    // Method begins at RVA 0x2282
    // Code size       0 (0x0)
  } // end of method MyGroupCollectionAttribute::.ctor

  .method /*0600022D*/ public specialname 
          instance string  get_MyGroupName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2283
    // Code size       0 (0x0)
  } // end of method MyGroupCollectionAttribute::get_MyGroupName

  .method /*0600022E*/ public specialname 
          instance string  get_CreateMethod() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2284
    // Code size       0 (0x0)
  } // end of method MyGroupCollectionAttribute::get_CreateMethod

  .method /*0600022F*/ public specialname 
          instance string  get_DisposeMethod() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2285
    // Code size       0 (0x0)
  } // end of method MyGroupCollectionAttribute::get_DisposeMethod

  .method /*06000230*/ public specialname 
          instance string  get_DefaultInstanceAlias() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2286
    // Code size       0 (0x0)
  } // end of method MyGroupCollectionAttribute::get_DefaultInstanceAlias

  .property /*1700007F*/ instance callconv(8) string
          MyGroupName()
  {
    .get instance string Microsoft.VisualBasic.MyGroupCollectionAttribute/*0200004B*/::get_MyGroupName() /* 0600022D */
  } // end of property MyGroupCollectionAttribute::MyGroupName
  .property /*17000080*/ instance callconv(8) string
          CreateMethod()
  {
    .get instance string Microsoft.VisualBasic.MyGroupCollectionAttribute/*0200004B*/::get_CreateMethod() /* 0600022E */
  } // end of property MyGroupCollectionAttribute::CreateMethod
  .property /*17000081*/ instance callconv(8) string
          DisposeMethod()
  {
    .get instance string Microsoft.VisualBasic.MyGroupCollectionAttribute/*0200004B*/::get_DisposeMethod() /* 0600022F */
  } // end of property MyGroupCollectionAttribute::DisposeMethod
  .property /*17000082*/ instance callconv(8) string
          DefaultInstanceAlias()
  {
    .get instance string Microsoft.VisualBasic.MyGroupCollectionAttribute/*0200004B*/::get_DefaultInstanceAlias() /* 06000230 */
  } // end of property MyGroupCollectionAttribute::DefaultInstanceAlias
} // end of class Microsoft.VisualBasic.MyGroupCollectionAttribute

.class /*0200004C*/ public auto ansi sealed Microsoft.VisualBasic.HideModuleNameAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C000040:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 00 )                               // rited.
  .method /*06000231*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2287
    // Code size       0 (0x0)
  } // end of method HideModuleNameAttribute::.ctor

} // end of class Microsoft.VisualBasic.HideModuleNameAttribute

.class /*0200004D*/ public auto ansi sealed Microsoft.VisualBasic.Globals
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000041:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .method /*06000232*/ public specialname static 
          string  get_ScriptEngine() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2288
    // Code size       0 (0x0)
  } // end of method Globals::get_ScriptEngine

  .method /*06000233*/ public specialname static 
          int32  get_ScriptEngineMajorVersion() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2289
    // Code size       0 (0x0)
  } // end of method Globals::get_ScriptEngineMajorVersion

  .method /*06000234*/ public specialname static 
          int32  get_ScriptEngineMinorVersion() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x228a
    // Code size       0 (0x0)
  } // end of method Globals::get_ScriptEngineMinorVersion

  .method /*06000235*/ public specialname static 
          int32  get_ScriptEngineBuildVersion() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x228b
    // Code size       0 (0x0)
  } // end of method Globals::get_ScriptEngineBuildVersion

  .method /*06000236*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228c
    // Code size       0 (0x0)
  } // end of method Globals::.ctor

  .property /*17000083*/ callconv(8) string
          ScriptEngine()
  {
    .get string Microsoft.VisualBasic.Globals/*0200004D*/::get_ScriptEngine() /* 06000232 */
  } // end of property Globals::ScriptEngine
  .property /*17000084*/ callconv(8) int32
          ScriptEngineMajorVersion()
  {
    .get int32 Microsoft.VisualBasic.Globals/*0200004D*/::get_ScriptEngineMajorVersion() /* 06000233 */
  } // end of property Globals::ScriptEngineMajorVersion
  .property /*17000085*/ callconv(8) int32
          ScriptEngineMinorVersion()
  {
    .get int32 Microsoft.VisualBasic.Globals/*0200004D*/::get_ScriptEngineMinorVersion() /* 06000234 */
  } // end of property Globals::ScriptEngineMinorVersion
  .property /*17000086*/ callconv(8) int32
          ScriptEngineBuildVersion()
  {
    .get int32 Microsoft.VisualBasic.Globals/*0200004D*/::get_ScriptEngineBuildVersion() /* 06000235 */
  } // end of property Globals::ScriptEngineBuildVersion
} // end of class Microsoft.VisualBasic.Globals

.class /*0200004E*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C000042:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 04 00 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 00 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                              74 69 70 6C 65 00 )                               // tiple.
  .custom /*0C000043:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000237*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228d
    // Code size       0 (0x0)
  } // end of method StandardModuleAttribute::.ctor

} // end of class Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute

.class /*0200004F*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.OptionTextAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C000044:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000045:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 04 00 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 00 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                              74 69 70 6C 65 00 )                               // tiple.
  .method /*06000238*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228e
    // Code size       0 (0x0)
  } // end of method OptionTextAttribute::.ctor

} // end of class Microsoft.VisualBasic.CompilerServices.OptionTextAttribute

.class /*02000050*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C000046:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 00 08 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                              69 74 65 64 00 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                              74 69 70 6C 65 00 )                               // tiple.
  .custom /*0C000047:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000239*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228f
    // Code size       0 (0x0)
  } // end of method OptionCompareAttribute::.ctor

} // end of class Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute

.class /*02000051*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.DesignerGeneratedAttribute
       extends [mscorlib/*23000001*/]System.Attribute/*01000032*/
{
  .custom /*0C000048:0A000030*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*01000061*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*01000062*/) /* 0A000030 */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                              4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                              72 69 74 65 64 00 )                               // rited.
  .custom /*0C000049:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600023A*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2290
    // Code size       0 (0x0)
  } // end of method DesignerGeneratedAttribute::.ctor

} // end of class Microsoft.VisualBasic.CompilerServices.DesignerGeneratedAttribute

.class /*02000052*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.BooleanType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00004A:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600023B*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2291
    // Code size       0 (0x0)
  } // end of method BooleanType::.ctor

  .method /*0600023C*/ public static bool 
          FromString(string Value) cil managed
  // SIG: 00 01 02 0E
  {
    // Method begins at RVA 0x2292
    // Code size       0 (0x0)
  } // end of method BooleanType::FromString

  .method /*0600023D*/ public static bool 
          FromObject(object Value) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x2293
    // Code size       0 (0x0)
  } // end of method BooleanType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.BooleanType

.class /*02000053*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.ByteType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00004B:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600023E*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2294
    // Code size       0 (0x0)
  } // end of method ByteType::.ctor

  .method /*0600023F*/ public static uint8 
          FromString(string Value) cil managed
  // SIG: 00 01 05 0E
  {
    // Method begins at RVA 0x2295
    // Code size       0 (0x0)
  } // end of method ByteType::FromString

  .method /*06000240*/ public static uint8 
          FromObject(object Value) cil managed
  // SIG: 00 01 05 1C
  {
    // Method begins at RVA 0x2296
    // Code size       0 (0x0)
  } // end of method ByteType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.ByteType

.class /*02000054*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.CharArrayType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00004C:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000241*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2297
    // Code size       0 (0x0)
  } // end of method CharArrayType::.ctor

  .method /*06000242*/ public static char[] 
          FromString(string Value) cil managed
  // SIG: 00 01 1D 03 0E
  {
    // Method begins at RVA 0x2298
    // Code size       0 (0x0)
  } // end of method CharArrayType::FromString

  .method /*06000243*/ public static char[] 
          FromObject(object Value) cil managed
  // SIG: 00 01 1D 03 1C
  {
    // Method begins at RVA 0x2299
    // Code size       0 (0x0)
  } // end of method CharArrayType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.CharArrayType

.class /*02000055*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.CharType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00004D:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000244*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x229a
    // Code size       0 (0x0)
  } // end of method CharType::.ctor

  .method /*06000245*/ public static char 
          FromString(string Value) cil managed
  // SIG: 00 01 03 0E
  {
    // Method begins at RVA 0x229b
    // Code size       0 (0x0)
  } // end of method CharType::FromString

  .method /*06000246*/ public static char 
          FromObject(object Value) cil managed
  // SIG: 00 01 03 1C
  {
    // Method begins at RVA 0x229c
    // Code size       0 (0x0)
  } // end of method CharType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.CharType

.class /*02000056*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.Conversions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00004E:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000247*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x229d
    // Code size       0 (0x0)
  } // end of method Conversions::.ctor

  .method /*06000248*/ public static bool 
          ToBoolean(string Value) cil managed
  // SIG: 00 01 02 0E
  {
    // Method begins at RVA 0x229e
    // Code size       0 (0x0)
  } // end of method Conversions::ToBoolean

  .method /*06000249*/ public static bool 
          ToBoolean(object Value) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x229f
    // Code size       0 (0x0)
  } // end of method Conversions::ToBoolean

  .method /*0600024A*/ public static uint8 
          ToByte(string Value) cil managed
  // SIG: 00 01 05 0E
  {
    // Method begins at RVA 0x22a0
    // Code size       0 (0x0)
  } // end of method Conversions::ToByte

  .method /*0600024B*/ public static uint8 
          ToByte(object Value) cil managed
  // SIG: 00 01 05 1C
  {
    // Method begins at RVA 0x22a1
    // Code size       0 (0x0)
  } // end of method Conversions::ToByte

  .method /*0600024C*/ public static int8 
          ToSByte(string Value) cil managed
  // SIG: 00 01 04 0E
  {
    .custom /*0C000098:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a2
    // Code size       0 (0x0)
  } // end of method Conversions::ToSByte

  .method /*0600024D*/ public static int8 
          ToSByte(object Value) cil managed
  // SIG: 00 01 04 1C
  {
    .custom /*0C000099:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a3
    // Code size       0 (0x0)
  } // end of method Conversions::ToSByte

  .method /*0600024E*/ public static int16 
          ToShort(string Value) cil managed
  // SIG: 00 01 06 0E
  {
    // Method begins at RVA 0x22a4
    // Code size       0 (0x0)
  } // end of method Conversions::ToShort

  .method /*0600024F*/ public static int16 
          ToShort(object Value) cil managed
  // SIG: 00 01 06 1C
  {
    // Method begins at RVA 0x22a5
    // Code size       0 (0x0)
  } // end of method Conversions::ToShort

  .method /*06000250*/ public static uint16 
          ToUShort(string Value) cil managed
  // SIG: 00 01 07 0E
  {
    .custom /*0C00009A:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a6
    // Code size       0 (0x0)
  } // end of method Conversions::ToUShort

  .method /*06000251*/ public static uint16 
          ToUShort(object Value) cil managed
  // SIG: 00 01 07 1C
  {
    .custom /*0C00009B:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a7
    // Code size       0 (0x0)
  } // end of method Conversions::ToUShort

  .method /*06000252*/ public static int32 
          ToInteger(string Value) cil managed
  // SIG: 00 01 08 0E
  {
    // Method begins at RVA 0x22a8
    // Code size       0 (0x0)
  } // end of method Conversions::ToInteger

  .method /*06000253*/ public static int32 
          ToInteger(object Value) cil managed
  // SIG: 00 01 08 1C
  {
    // Method begins at RVA 0x22a9
    // Code size       0 (0x0)
  } // end of method Conversions::ToInteger

  .method /*06000254*/ public static uint32 
          ToUInteger(string Value) cil managed
  // SIG: 00 01 09 0E
  {
    .custom /*0C00009C:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22aa
    // Code size       0 (0x0)
  } // end of method Conversions::ToUInteger

  .method /*06000255*/ public static uint32 
          ToUInteger(object Value) cil managed
  // SIG: 00 01 09 1C
  {
    .custom /*0C00009D:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22ab
    // Code size       0 (0x0)
  } // end of method Conversions::ToUInteger

  .method /*06000256*/ public static int64 
          ToLong(string Value) cil managed
  // SIG: 00 01 0A 0E
  {
    // Method begins at RVA 0x22ac
    // Code size       0 (0x0)
  } // end of method Conversions::ToLong

  .method /*06000257*/ public static int64 
          ToLong(object Value) cil managed
  // SIG: 00 01 0A 1C
  {
    // Method begins at RVA 0x22ad
    // Code size       0 (0x0)
  } // end of method Conversions::ToLong

  .method /*06000258*/ public static uint64 
          ToULong(string Value) cil managed
  // SIG: 00 01 0B 0E
  {
    .custom /*0C00009E:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22ae
    // Code size       0 (0x0)
  } // end of method Conversions::ToULong

  .method /*06000259*/ public static uint64 
          ToULong(object Value) cil managed
  // SIG: 00 01 0B 1C
  {
    .custom /*0C00009F:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22af
    // Code size       0 (0x0)
  } // end of method Conversions::ToULong

  .method /*0600025A*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          ToDecimal(bool Value) cil managed
  // SIG: 00 01 11 7D 02
  {
    // Method begins at RVA 0x22b0
    // Code size       0 (0x0)
  } // end of method Conversions::ToDecimal

  .method /*0600025B*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          ToDecimal(string Value) cil managed
  // SIG: 00 01 11 7D 0E
  {
    // Method begins at RVA 0x22b1
    // Code size       0 (0x0)
  } // end of method Conversions::ToDecimal

  .method /*0600025C*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          ToDecimal(object Value) cil managed
  // SIG: 00 01 11 7D 1C
  {
    // Method begins at RVA 0x22b2
    // Code size       0 (0x0)
  } // end of method Conversions::ToDecimal

  .method /*0600025D*/ public static float32 
          ToSingle(string Value) cil managed
  // SIG: 00 01 0C 0E
  {
    // Method begins at RVA 0x22b3
    // Code size       0 (0x0)
  } // end of method Conversions::ToSingle

  .method /*0600025E*/ public static float32 
          ToSingle(object Value) cil managed
  // SIG: 00 01 0C 1C
  {
    // Method begins at RVA 0x22b4
    // Code size       0 (0x0)
  } // end of method Conversions::ToSingle

  .method /*0600025F*/ public static float64 
          ToDouble(string Value) cil managed
  // SIG: 00 01 0D 0E
  {
    // Method begins at RVA 0x22b5
    // Code size       0 (0x0)
  } // end of method Conversions::ToDouble

  .method /*06000260*/ public static float64 
          ToDouble(object Value) cil managed
  // SIG: 00 01 0D 1C
  {
    // Method begins at RVA 0x22b6
    // Code size       0 (0x0)
  } // end of method Conversions::ToDouble

  .method /*06000261*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          ToDate(string Value) cil managed
  // SIG: 00 01 11 80 81 0E
  {
    // Method begins at RVA 0x22b7
    // Code size       0 (0x0)
  } // end of method Conversions::ToDate

  .method /*06000262*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          ToDate(object Value) cil managed
  // SIG: 00 01 11 80 81 1C
  {
    // Method begins at RVA 0x22b8
    // Code size       0 (0x0)
  } // end of method Conversions::ToDate

  .method /*06000263*/ public static char 
          ToChar(string Value) cil managed
  // SIG: 00 01 03 0E
  {
    // Method begins at RVA 0x22b9
    // Code size       0 (0x0)
  } // end of method Conversions::ToChar

  .method /*06000264*/ public static char 
          ToChar(object Value) cil managed
  // SIG: 00 01 03 1C
  {
    // Method begins at RVA 0x22ba
    // Code size       0 (0x0)
  } // end of method Conversions::ToChar

  .method /*06000265*/ public static char[] 
          ToCharArrayRankOne(string Value) cil managed
  // SIG: 00 01 1D 03 0E
  {
    // Method begins at RVA 0x22bb
    // Code size       0 (0x0)
  } // end of method Conversions::ToCharArrayRankOne

  .method /*06000266*/ public static char[] 
          ToCharArrayRankOne(object Value) cil managed
  // SIG: 00 01 1D 03 1C
  {
    // Method begins at RVA 0x22bc
    // Code size       0 (0x0)
  } // end of method Conversions::ToCharArrayRankOne

  .method /*06000267*/ public static string 
          ToString(bool Value) cil managed
  // SIG: 00 01 0E 02
  {
    // Method begins at RVA 0x22bd
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000268*/ public static string 
          ToString(uint8 Value) cil managed
  // SIG: 00 01 0E 05
  {
    // Method begins at RVA 0x22be
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000269*/ public static string 
          ToString(char Value) cil managed
  // SIG: 00 01 0E 03
  {
    // Method begins at RVA 0x22bf
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*0600026A*/ public static string 
          FromCharArray(char[] Value) cil managed
  // SIG: 00 01 0E 1D 03
  {
    // Method begins at RVA 0x22c0
    // Code size       0 (0x0)
  } // end of method Conversions::FromCharArray

  .method /*0600026B*/ public static string 
          FromCharAndCount(char Value,
                           int32 Count) cil managed
  // SIG: 00 02 0E 03 08
  {
    // Method begins at RVA 0x22c1
    // Code size       0 (0x0)
  } // end of method Conversions::FromCharAndCount

  .method /*0600026C*/ public static string 
          FromCharArraySubset(char[] Value,
                              int32 StartIndex,
                              int32 Length) cil managed
  // SIG: 00 03 0E 1D 03 08 08
  {
    // Method begins at RVA 0x22c2
    // Code size       0 (0x0)
  } // end of method Conversions::FromCharArraySubset

  .method /*0600026D*/ public static string 
          ToString(int16 Value) cil managed
  // SIG: 00 01 0E 06
  {
    // Method begins at RVA 0x22c3
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*0600026E*/ public static string 
          ToString(int32 Value) cil managed
  // SIG: 00 01 0E 08
  {
    // Method begins at RVA 0x22c4
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*0600026F*/ public static string 
          ToString(uint32 Value) cil managed
  // SIG: 00 01 0E 09
  {
    .custom /*0C0000A0:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22c5
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000270*/ public static string 
          ToString(int64 Value) cil managed
  // SIG: 00 01 0E 0A
  {
    // Method begins at RVA 0x22c6
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000271*/ public static string 
          ToString(uint64 Value) cil managed
  // SIG: 00 01 0E 0B
  {
    .custom /*0C0000A1:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22c7
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000272*/ public static string 
          ToString(float32 Value) cil managed
  // SIG: 00 01 0E 0C
  {
    // Method begins at RVA 0x22c8
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000273*/ public static string 
          ToString(float64 Value) cil managed
  // SIG: 00 01 0E 0D
  {
    // Method begins at RVA 0x22c9
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000274*/ public static string 
          ToString(float32 Value,
                   class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0E 0C 12 80 CD
  {
    // Method begins at RVA 0x22ca
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000275*/ public static string 
          ToString(float64 Value,
                   class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0E 0D 12 80 CD
  {
    // Method begins at RVA 0x22cb
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000276*/ public static string 
          ToString(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Value) cil managed
  // SIG: 00 01 0E 11 80 81
  {
    // Method begins at RVA 0x22cc
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000277*/ public static string 
          ToString(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ Value) cil managed
  // SIG: 00 01 0E 11 7D
  {
    // Method begins at RVA 0x22cd
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000278*/ public static string 
          ToString(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ Value,
                   class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0E 11 7D 12 80 CD
  {
    // Method begins at RVA 0x22ce
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*06000279*/ public static string 
          ToString(object Value) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22cf
    // Code size       0 (0x0)
  } // end of method Conversions::ToString

  .method /*0600027A*/ public static !!T 
          ToGenericParameter<T>(object Value) cil managed
  // SIG: 10 01 01 1E 00 1C
  {
    // Method begins at RVA 0x22d0
    // Code size       0 (0x0)
  } // end of method Conversions::ToGenericParameter

  .method /*0600027B*/ public static object 
          ChangeType(object Expression,
                     class [mscorlib/*23000001*/]System.Type/*0100000B*/ TargetType) cil managed
  // SIG: 00 02 1C 1C 12 2D
  {
    // Method begins at RVA 0x22d1
    // Code size       0 (0x0)
  } // end of method Conversions::ChangeType

} // end of class Microsoft.VisualBasic.CompilerServices.Conversions

.class /*02000057*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.DateType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00004F:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600027C*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d2
    // Code size       0 (0x0)
  } // end of method DateType::.ctor

  .method /*0600027D*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          FromString(string Value) cil managed
  // SIG: 00 01 11 80 81 0E
  {
    // Method begins at RVA 0x22d3
    // Code size       0 (0x0)
  } // end of method DateType::FromString

  .method /*0600027E*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          FromString(string Value,
                     class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ culture) cil managed
  // SIG: 00 02 11 80 81 0E 12 09
  {
    // Method begins at RVA 0x22d4
    // Code size       0 (0x0)
  } // end of method DateType::FromString

  .method /*0600027F*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ 
          FromObject(object Value) cil managed
  // SIG: 00 01 11 80 81 1C
  {
    // Method begins at RVA 0x22d5
    // Code size       0 (0x0)
  } // end of method DateType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.DateType

.class /*02000058*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.DecimalType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000050:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000280*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d6
    // Code size       0 (0x0)
  } // end of method DecimalType::.ctor

  .method /*06000281*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          FromBoolean(bool Value) cil managed
  // SIG: 00 01 11 7D 02
  {
    // Method begins at RVA 0x22d7
    // Code size       0 (0x0)
  } // end of method DecimalType::FromBoolean

  .method /*06000282*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          FromString(string Value) cil managed
  // SIG: 00 01 11 7D 0E
  {
    // Method begins at RVA 0x22d8
    // Code size       0 (0x0)
  } // end of method DecimalType::FromString

  .method /*06000283*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          FromString(string Value,
                     class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 11 7D 0E 12 80 CD
  {
    // Method begins at RVA 0x22d9
    // Code size       0 (0x0)
  } // end of method DecimalType::FromString

  .method /*06000284*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          FromObject(object Value) cil managed
  // SIG: 00 01 11 7D 1C
  {
    // Method begins at RVA 0x22da
    // Code size       0 (0x0)
  } // end of method DecimalType::FromObject

  .method /*06000285*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          FromObject(object Value,
                     class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 11 7D 1C 12 80 CD
  {
    // Method begins at RVA 0x22db
    // Code size       0 (0x0)
  } // end of method DecimalType::FromObject

  .method /*06000286*/ public static valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ 
          Parse(string Value,
                class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 11 7D 0E 12 80 CD
  {
    // Method begins at RVA 0x22dc
    // Code size       0 (0x0)
  } // end of method DecimalType::Parse

} // end of class Microsoft.VisualBasic.CompilerServices.DecimalType

.class /*02000059*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.DoubleType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000051:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000287*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22dd
    // Code size       0 (0x0)
  } // end of method DoubleType::.ctor

  .method /*06000288*/ public static float64 
          FromString(string Value) cil managed
  // SIG: 00 01 0D 0E
  {
    // Method begins at RVA 0x22de
    // Code size       0 (0x0)
  } // end of method DoubleType::FromString

  .method /*06000289*/ public static float64 
          FromString(string Value,
                     class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0D 0E 12 80 CD
  {
    // Method begins at RVA 0x22df
    // Code size       0 (0x0)
  } // end of method DoubleType::FromString

  .method /*0600028A*/ public static float64 
          FromObject(object Value) cil managed
  // SIG: 00 01 0D 1C
  {
    // Method begins at RVA 0x22e0
    // Code size       0 (0x0)
  } // end of method DoubleType::FromObject

  .method /*0600028B*/ public static float64 
          FromObject(object Value,
                     class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0D 1C 12 80 CD
  {
    // Method begins at RVA 0x22e1
    // Code size       0 (0x0)
  } // end of method DoubleType::FromObject

  .method /*0600028C*/ public static float64 
          Parse(string Value) cil managed
  // SIG: 00 01 0D 0E
  {
    // Method begins at RVA 0x22e2
    // Code size       0 (0x0)
  } // end of method DoubleType::Parse

  .method /*0600028D*/ public static float64 
          Parse(string Value,
                class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0D 0E 12 80 CD
  {
    // Method begins at RVA 0x22e3
    // Code size       0 (0x0)
  } // end of method DoubleType::Parse

} // end of class Microsoft.VisualBasic.CompilerServices.DoubleType

.class /*0200005A*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.ExceptionUtils
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000052:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600028E*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e4
    // Code size       0 (0x0)
  } // end of method ExceptionUtils::.ctor

} // end of class Microsoft.VisualBasic.CompilerServices.ExceptionUtils

.class /*0200005B*/ public auto ansi serializable sealed Microsoft.VisualBasic.CompilerServices.InternalErrorException
       extends [mscorlib/*23000001*/]System.Exception/*0100000D*/
{
  .custom /*0C000053:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600028F*/ private specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000013*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000014*/ context) cil managed
  // SIG: 20 02 01 12 4D 11 51
  {
    .custom /*0C0000A4:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x22e5
    // Code size       0 (0x0)
  } // end of method InternalErrorException::.ctor

  .method /*06000290*/ public specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A5:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x22e6
    // Code size       0 (0x0)
  } // end of method InternalErrorException::.ctor

  .method /*06000291*/ public specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*0100000D*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 35
  {
    .custom /*0C0000A6:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x22e7
    // Code size       0 (0x0)
  } // end of method InternalErrorException::.ctor

  .method /*06000292*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e8
    // Code size       0 (0x0)
  } // end of method InternalErrorException::.ctor

} // end of class Microsoft.VisualBasic.CompilerServices.InternalErrorException

.class /*0200005C*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.FlowControl
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000054:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000293*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e9
    // Code size       0 (0x0)
  } // end of method FlowControl::.ctor

  .method /*06000294*/ public static bool 
          ForNextCheckR4(float32 count,
                         float32 limit,
                         float32 StepValue) cil managed
  // SIG: 00 03 02 0C 0C 0C
  {
    // Method begins at RVA 0x22ea
    // Code size       0 (0x0)
  } // end of method FlowControl::ForNextCheckR4

  .method /*06000295*/ public static bool 
          ForNextCheckR8(float64 count,
                         float64 limit,
                         float64 StepValue) cil managed
  // SIG: 00 03 02 0D 0D 0D
  {
    // Method begins at RVA 0x22eb
    // Code size       0 (0x0)
  } // end of method FlowControl::ForNextCheckR8

  .method /*06000296*/ public static bool 
          ForNextCheckDec(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ count,
                          valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ limit,
                          valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ StepValue) cil managed
  // SIG: 00 03 02 11 7D 11 7D 11 7D
  {
    // Method begins at RVA 0x22ec
    // Code size       0 (0x0)
  } // end of method FlowControl::ForNextCheckDec

  .method /*06000297*/ public static bool 
          ForLoopInitObj(object Counter,
                         object Start,
                         object Limit,
                         object StepValue,
                         object& LoopForResult,
                         object& CounterResult) cil managed
  // SIG: 00 06 02 1C 1C 1C 1C 10 1C 10 1C
  {
    // Method begins at RVA 0x22ed
    // Code size       0 (0x0)
  } // end of method FlowControl::ForLoopInitObj

  .method /*06000298*/ public static bool 
          ForNextCheckObj(object Counter,
                          object LoopObj,
                          object& CounterResult) cil managed
  // SIG: 00 03 02 1C 1C 10 1C
  {
    // Method begins at RVA 0x22ee
    // Code size       0 (0x0)
  } // end of method FlowControl::ForNextCheckObj

  .method /*06000299*/ public static class [mscorlib/*23000001*/]System.Collections.IEnumerator/*0100001B*/ 
          ForEachInArr(class [mscorlib/*23000001*/]System.Array/*0100001E*/ ary) cil managed
  // SIG: 00 01 12 6D 12 79
  {
    // Method begins at RVA 0x22ef
    // Code size       0 (0x0)
  } // end of method FlowControl::ForEachInArr

  .method /*0600029A*/ public static class [mscorlib/*23000001*/]System.Collections.IEnumerator/*0100001B*/ 
          ForEachInObj(object obj) cil managed
  // SIG: 00 01 12 6D 1C
  {
    // Method begins at RVA 0x22f0
    // Code size       0 (0x0)
  } // end of method FlowControl::ForEachInObj

  .method /*0600029B*/ public static bool 
          ForEachNextObj(object& obj,
                         class [mscorlib/*23000001*/]System.Collections.IEnumerator/*0100001B*/ enumerator) cil managed
  // SIG: 00 02 02 10 1C 12 6D
  {
    // Method begins at RVA 0x22f1
    // Code size       0 (0x0)
  } // end of method FlowControl::ForEachNextObj

  .method /*0600029C*/ public static void 
          CheckForSyncLockOnValueType(object obj) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x22f2
    // Code size       0 (0x0)
  } // end of method FlowControl::CheckForSyncLockOnValueType

} // end of class Microsoft.VisualBasic.CompilerServices.FlowControl

.class /*0200005D*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.ObjectFlowControl
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000055:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .class /*0200005E*/ auto ansi sealed nested public ForLoopControl
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .custom /*0C000056:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .method /*0600029F*/ private specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x22f5
      // Code size       0 (0x0)
    } // end of method ForLoopControl::.ctor

    .method /*060002A0*/ public static bool 
            ForLoopInitObj(object Counter,
                           object Start,
                           object Limit,
                           object StepValue,
                           object& LoopForResult,
                           object& CounterResult) cil managed
    // SIG: 00 06 02 1C 1C 1C 1C 10 1C 10 1C
    {
      // Method begins at RVA 0x22f6
      // Code size       0 (0x0)
    } // end of method ForLoopControl::ForLoopInitObj

    .method /*060002A1*/ public static bool 
            ForNextCheckObj(object Counter,
                            object LoopObj,
                            object& CounterResult) cil managed
    // SIG: 00 03 02 1C 1C 10 1C
    {
      // Method begins at RVA 0x22f7
      // Code size       0 (0x0)
    } // end of method ForLoopControl::ForNextCheckObj

    .method /*060002A2*/ public static bool 
            ForNextCheckR4(float32 count,
                           float32 limit,
                           float32 StepValue) cil managed
    // SIG: 00 03 02 0C 0C 0C
    {
      // Method begins at RVA 0x22f8
      // Code size       0 (0x0)
    } // end of method ForLoopControl::ForNextCheckR4

    .method /*060002A3*/ public static bool 
            ForNextCheckR8(float64 count,
                           float64 limit,
                           float64 StepValue) cil managed
    // SIG: 00 03 02 0D 0D 0D
    {
      // Method begins at RVA 0x22f9
      // Code size       0 (0x0)
    } // end of method ForLoopControl::ForNextCheckR8

    .method /*060002A4*/ public static bool 
            ForNextCheckDec(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ count,
                            valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ limit,
                            valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ StepValue) cil managed
    // SIG: 00 03 02 11 7D 11 7D 11 7D
    {
      // Method begins at RVA 0x22fa
      // Code size       0 (0x0)
    } // end of method ForLoopControl::ForNextCheckDec

  } // end of class ForLoopControl

  .method /*0600029D*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22f3
    // Code size       0 (0x0)
  } // end of method ObjectFlowControl::.ctor

  .method /*0600029E*/ public static void 
          CheckForSyncLockOnValueType(object Expression) cil managed
  // SIG: 00 01 01 1C
  {
    // Method begins at RVA 0x22f4
    // Code size       0 (0x0)
  } // end of method ObjectFlowControl::CheckForSyncLockOnValueType

} // end of class Microsoft.VisualBasic.CompilerServices.ObjectFlowControl

.class /*0200005F*/ interface public abstract auto ansi Microsoft.VisualBasic.CompilerServices.IVbHost
{
  .custom /*0C000057:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002A5*/ public newslot abstract strict virtual 
          instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.IWin32Window/*01000034*/ 
          GetParentWindow() cil managed
  // SIG: 20 00 12 80 D1
  {
    // Method begins at RVA 0x0
  } // end of method IVbHost::GetParentWindow

  .method /*060002A6*/ public newslot abstract strict virtual 
          instance string  GetWindowTitle() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IVbHost::GetWindowTitle

} // end of class Microsoft.VisualBasic.CompilerServices.IVbHost

.class /*02000060*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.HostServices
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000058:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(2)}}
  .method /*060002A7*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22fb
    // Code size       0 (0x0)
  } // end of method HostServices::.ctor

  .method /*060002A8*/ public specialname static 
          class Microsoft.VisualBasic.CompilerServices.IVbHost/*0200005F*/ 
          get_VBHost() cil managed
  // SIG: 00 00 12 81 7C
  {
    // Method begins at RVA 0x22fc
    // Code size       0 (0x0)
  } // end of method HostServices::get_VBHost

  .method /*060002A9*/ public specialname static 
          void  set_VBHost(class Microsoft.VisualBasic.CompilerServices.IVbHost/*0200005F*/ Value) cil managed
  // SIG: 00 01 01 12 81 7C
  {
    // Method begins at RVA 0x22fd
    // Code size       0 (0x0)
  } // end of method HostServices::set_VBHost

  .property /*17000087*/ callconv(8) class Microsoft.VisualBasic.CompilerServices.IVbHost/*0200005F*/
          VBHost()
  {
    .set void Microsoft.VisualBasic.CompilerServices.HostServices/*02000060*/::set_VBHost(class Microsoft.VisualBasic.CompilerServices.IVbHost/*0200005F*/) /* 060002A9 */
    .get class Microsoft.VisualBasic.CompilerServices.IVbHost/*0200005F*/ Microsoft.VisualBasic.CompilerServices.HostServices/*02000060*/::get_VBHost() /* 060002A8 */
  } // end of property HostServices::VBHost
} // end of class Microsoft.VisualBasic.CompilerServices.HostServices

.class /*02000061*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.IntegerType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000059:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002AA*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22fe
    // Code size       0 (0x0)
  } // end of method IntegerType::.ctor

  .method /*060002AB*/ public static int32 
          FromString(string Value) cil managed
  // SIG: 00 01 08 0E
  {
    // Method begins at RVA 0x22ff
    // Code size       0 (0x0)
  } // end of method IntegerType::FromString

  .method /*060002AC*/ public static int32 
          FromObject(object Value) cil managed
  // SIG: 00 01 08 1C
  {
    // Method begins at RVA 0x2300
    // Code size       0 (0x0)
  } // end of method IntegerType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.IntegerType

.class /*02000062*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.LateBinding
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00005A:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002AD*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2301
    // Code size       0 (0x0)
  } // end of method LateBinding::.ctor

  .method /*060002AE*/ public static object 
          LateGet(object o,
                  class [mscorlib/*23000001*/]System.Type/*0100000B*/ objType,
                  string name,
                  object[] args,
                  string[] paramnames,
                  bool[] CopyBack) cil managed
  // SIG: 00 06 1C 1C 12 2D 0E 1D 1C 1D 0E 1D 02
  {
    .custom /*0C0000A9:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AA:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2302
    // Code size       0 (0x0)
  } // end of method LateBinding::LateGet

  .method /*060002AF*/ public static void 
          LateSetComplex(object o,
                         class [mscorlib/*23000001*/]System.Type/*0100000B*/ objType,
                         string name,
                         object[] args,
                         string[] paramnames,
                         bool OptimisticSet,
                         bool RValueBase) cil managed
  // SIG: 00 07 01 1C 12 2D 0E 1D 1C 1D 0E 02 02
  {
    .custom /*0C0000AB:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AC:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2303
    // Code size       0 (0x0)
  } // end of method LateBinding::LateSetComplex

  .method /*060002B0*/ public static void 
          LateSet(object o,
                  class [mscorlib/*23000001*/]System.Type/*0100000B*/ objType,
                  string name,
                  object[] args,
                  string[] paramnames) cil managed
  // SIG: 00 05 01 1C 12 2D 0E 1D 1C 1D 0E
  {
    .custom /*0C0000AD:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AE:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2304
    // Code size       0 (0x0)
  } // end of method LateBinding::LateSet

  .method /*060002B1*/ public static object 
          LateIndexGet(object o,
                       object[] args,
                       string[] paramnames) cil managed
  // SIG: 00 03 1C 1C 1D 1C 1D 0E
  {
    .custom /*0C0000AF:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B0:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2305
    // Code size       0 (0x0)
  } // end of method LateBinding::LateIndexGet

  .method /*060002B2*/ public static void 
          LateIndexSetComplex(object o,
                              object[] args,
                              string[] paramnames,
                              bool OptimisticSet,
                              bool RValueBase) cil managed
  // SIG: 00 05 01 1C 1D 1C 1D 0E 02 02
  {
    .custom /*0C0000B1:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B2:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2306
    // Code size       0 (0x0)
  } // end of method LateBinding::LateIndexSetComplex

  .method /*060002B3*/ public static void 
          LateIndexSet(object o,
                       object[] args,
                       string[] paramnames) cil managed
  // SIG: 00 03 01 1C 1D 1C 1D 0E
  {
    .custom /*0C0000B3:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B4:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2307
    // Code size       0 (0x0)
  } // end of method LateBinding::LateIndexSet

  .method /*060002B4*/ public static void 
          LateCall(object o,
                   class [mscorlib/*23000001*/]System.Type/*0100000B*/ objType,
                   string name,
                   object[] args,
                   string[] paramnames,
                   bool[] CopyBack) cil managed
  // SIG: 00 06 01 1C 12 2D 0E 1D 1C 1D 0E 1D 02
  {
    .custom /*0C0000B5:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B6:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2308
    // Code size       0 (0x0)
  } // end of method LateBinding::LateCall

} // end of class Microsoft.VisualBasic.CompilerServices.LateBinding

.class /*02000063*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.LikeOperator
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00005B:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002B5*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2309
    // Code size       0 (0x0)
  } // end of method LikeOperator::.ctor

  .method /*060002B6*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x230a
    // Code size       0 (0x0)
  } // end of method LikeOperator::.cctor

  .method /*060002B7*/ public static object 
          LikeObject(object Source,
                     object Pattern,
                     valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ CompareOption) cil managed
  // SIG: 00 03 1C 1C 1C 11 80 E0
  {
    // Method begins at RVA 0x230b
    // Code size       0 (0x0)
  } // end of method LikeOperator::LikeObject

  .method /*060002B8*/ public static bool 
          LikeString(string Source,
                     string Pattern,
                     valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ CompareOption) cil managed
  // SIG: 00 03 02 0E 0E 11 80 E0
  {
    // Method begins at RVA 0x230c
    // Code size       0 (0x0)
  } // end of method LikeOperator::LikeString

} // end of class Microsoft.VisualBasic.CompilerServices.LikeOperator

.class /*02000064*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.LongType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00005C:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002B9*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2318
    // Code size       0 (0x0)
  } // end of method LongType::.ctor

  .method /*060002BA*/ public static int64 
          FromString(string Value) cil managed
  // SIG: 00 01 0A 0E
  {
    // Method begins at RVA 0x2319
    // Code size       0 (0x0)
  } // end of method LongType::FromString

  .method /*060002BB*/ public static int64 
          FromObject(object Value) cil managed
  // SIG: 00 01 0A 1C
  {
    // Method begins at RVA 0x231a
    // Code size       0 (0x0)
  } // end of method LongType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.LongType

.class /*02000065*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.NewLateBinding
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00005E:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002BC*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x231b
    // Code size       0 (0x0)
  } // end of method NewLateBinding::.ctor

  .method /*060002BD*/ public static bool 
          LateCanEvaluate(object 'instance',
                          class [mscorlib/*23000001*/]System.Type/*0100000B*/ 'type',
                          string memberName,
                          object[] arguments,
                          bool allowFunctionEvaluation,
                          bool allowPropertyEvaluation) cil managed
  // SIG: 00 06 02 1C 12 2D 0E 1D 1C 02 02
  {
    .custom /*0C0000B7:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B8:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x231c
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateCanEvaluate

  .method /*060002BE*/ public static object 
          LateCall(object Instance,
                   class [mscorlib/*23000001*/]System.Type/*0100000B*/ Type,
                   string MemberName,
                   object[] Arguments,
                   string[] ArgumentNames,
                   class [mscorlib/*23000001*/]System.Type/*0100000B*/[] TypeArguments,
                   bool[] CopyBack,
                   bool IgnoreReturn) cil managed
  // SIG: 00 08 1C 1C 12 2D 0E 1D 1C 1D 0E 1D 12 2D 1D 02 02
  {
    .custom /*0C0000B9:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BA:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2320
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateCall

  .method /*060002BF*/ public static object 
          LateIndexGet(object Instance,
                       object[] Arguments,
                       string[] ArgumentNames) cil managed
  // SIG: 00 03 1C 1C 1D 1C 1D 0E
  {
    .custom /*0C0000BB:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BC:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x232c
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateIndexGet

  .method /*060002C0*/ public static object 
          LateGet(object Instance,
                  class [mscorlib/*23000001*/]System.Type/*0100000B*/ Type,
                  string MemberName,
                  object[] Arguments,
                  string[] ArgumentNames,
                  class [mscorlib/*23000001*/]System.Type/*0100000B*/[] TypeArguments,
                  bool[] CopyBack) cil managed
  // SIG: 00 07 1C 1C 12 2D 0E 1D 1C 1D 0E 1D 12 2D 1D 02
  {
    .custom /*0C0000BD:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BE:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2330
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateGet

  .method /*060002C1*/ public static void 
          LateIndexSetComplex(object Instance,
                              object[] Arguments,
                              string[] ArgumentNames,
                              bool OptimisticSet,
                              bool RValueBase) cil managed
  // SIG: 00 05 01 1C 1D 1C 1D 0E 02 02
  {
    .custom /*0C0000BF:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C0:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x233c
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateIndexSetComplex

  .method /*060002C2*/ public static void 
          LateIndexSet(object Instance,
                       object[] Arguments,
                       string[] ArgumentNames) cil managed
  // SIG: 00 03 01 1C 1D 1C 1D 0E
  {
    .custom /*0C0000C1:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C2:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2348
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateIndexSet

  .method /*060002C3*/ public static void 
          LateSetComplex(object Instance,
                         class [mscorlib/*23000001*/]System.Type/*0100000B*/ Type,
                         string MemberName,
                         object[] Arguments,
                         string[] ArgumentNames,
                         class [mscorlib/*23000001*/]System.Type/*0100000B*/[] TypeArguments,
                         bool OptimisticSet,
                         bool RValueBase) cil managed
  // SIG: 00 08 01 1C 12 2D 0E 1D 1C 1D 0E 1D 12 2D 02 02
  {
    .custom /*0C0000C3:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C4:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x234c
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateSetComplex

  .method /*060002C4*/ public static void 
          LateSet(object Instance,
                  class [mscorlib/*23000001*/]System.Type/*0100000B*/ Type,
                  string MemberName,
                  object[] Arguments,
                  string[] ArgumentNames,
                  class [mscorlib/*23000001*/]System.Type/*0100000B*/[] TypeArguments) cil managed
  // SIG: 00 06 01 1C 12 2D 0E 1D 1C 1D 0E 1D 12 2D
  {
    .custom /*0C0000C5:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C6:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2358
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateSet

  .method /*060002C5*/ public static void 
          LateSet(object Instance,
                  class [mscorlib/*23000001*/]System.Type/*0100000B*/ Type,
                  string MemberName,
                  object[] Arguments,
                  string[] ArgumentNames,
                  class [mscorlib/*23000001*/]System.Type/*0100000B*/[] TypeArguments,
                  bool OptimisticSet,
                  bool RValueBase,
                  valuetype Microsoft.VisualBasic.CallType/*02000037*/ CallType) cil managed
  // SIG: 00 09 01 1C 12 2D 0E 1D 1C 1D 0E 1D 12 2D 02 02 11 80 DC
  {
    .custom /*0C0000C7:0A000032*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerStepThroughAttribute/*01000064*/::.ctor() /* 0A000032 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C8:0A000031*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000063*/::.ctor() /* 0A000031 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2364
    // Code size       0 (0x0)
  } // end of method NewLateBinding::LateSet

} // end of class Microsoft.VisualBasic.CompilerServices.NewLateBinding

.class /*02000066*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.CompilerServices.ObjectType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00005F:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002C6*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2370
    // Code size       0 (0x0)
  } // end of method ObjectType::.cctor

  .method /*060002C7*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2371
    // Code size       0 (0x0)
  } // end of method ObjectType::.ctor

  .method /*060002C8*/ public static int32 
          ObjTst(object o1,
                 object o2,
                 bool TextCompare) cil managed
  // SIG: 00 03 08 1C 1C 02
  {
    // Method begins at RVA 0x2372
    // Code size       0 (0x0)
  } // end of method ObjectType::ObjTst

  .method /*060002C9*/ public static object 
          PlusObj(object obj) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x2373
    // Code size       0 (0x0)
  } // end of method ObjectType::PlusObj

  .method /*060002CA*/ public static object 
          NegObj(object obj) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x2374
    // Code size       0 (0x0)
  } // end of method ObjectType::NegObj

  .method /*060002CB*/ public static object 
          NotObj(object obj) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x2375
    // Code size       0 (0x0)
  } // end of method ObjectType::NotObj

  .method /*060002CC*/ public static object 
          BitAndObj(object obj1,
                    object obj2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x2376
    // Code size       0 (0x0)
  } // end of method ObjectType::BitAndObj

  .method /*060002CD*/ public static object 
          BitOrObj(object obj1,
                   object obj2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x2377
    // Code size       0 (0x0)
  } // end of method ObjectType::BitOrObj

  .method /*060002CE*/ public static object 
          BitXorObj(object obj1,
                    object obj2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x2378
    // Code size       0 (0x0)
  } // end of method ObjectType::BitXorObj

  .method /*060002CF*/ public static object 
          AddObj(object o1,
                 object o2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x2379
    // Code size       0 (0x0)
  } // end of method ObjectType::AddObj

  .method /*060002D0*/ public static object 
          SubObj(object o1,
                 object o2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x237a
    // Code size       0 (0x0)
  } // end of method ObjectType::SubObj

  .method /*060002D1*/ public static object 
          MulObj(object o1,
                 object o2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x237b
    // Code size       0 (0x0)
  } // end of method ObjectType::MulObj

  .method /*060002D2*/ public static object 
          DivObj(object o1,
                 object o2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x237c
    // Code size       0 (0x0)
  } // end of method ObjectType::DivObj

  .method /*060002D3*/ public static object 
          PowObj(object obj1,
                 object obj2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x237d
    // Code size       0 (0x0)
  } // end of method ObjectType::PowObj

  .method /*060002D4*/ public static object 
          ModObj(object o1,
                 object o2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x237e
    // Code size       0 (0x0)
  } // end of method ObjectType::ModObj

  .method /*060002D5*/ public static object 
          IDivObj(object o1,
                  object o2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x237f
    // Code size       0 (0x0)
  } // end of method ObjectType::IDivObj

  .method /*060002D6*/ public static object 
          ShiftLeftObj(object o1,
                       int32 amount) cil managed
  // SIG: 00 02 1C 1C 08
  {
    // Method begins at RVA 0x2380
    // Code size       0 (0x0)
  } // end of method ObjectType::ShiftLeftObj

  .method /*060002D7*/ public static object 
          ShiftRightObj(object o1,
                        int32 amount) cil managed
  // SIG: 00 02 1C 1C 08
  {
    // Method begins at RVA 0x2381
    // Code size       0 (0x0)
  } // end of method ObjectType::ShiftRightObj

  .method /*060002D8*/ public static object 
          XorObj(object obj1,
                 object obj2) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x2382
    // Code size       0 (0x0)
  } // end of method ObjectType::XorObj

  .method /*060002D9*/ public static bool 
          LikeObj(object vLeft,
                  object vRight,
                  valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ CompareOption) cil managed
  // SIG: 00 03 02 1C 1C 11 80 E0
  {
    // Method begins at RVA 0x2383
    // Code size       0 (0x0)
  } // end of method ObjectType::LikeObj

  .method /*060002DA*/ public static object 
          StrCatObj(object vLeft,
                    object vRight) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x2384
    // Code size       0 (0x0)
  } // end of method ObjectType::StrCatObj

  .method /*060002DB*/ public static object 
          GetObjectValuePrimitive(object o) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x2385
    // Code size       0 (0x0)
  } // end of method ObjectType::GetObjectValuePrimitive

} // end of class Microsoft.VisualBasic.CompilerServices.ObjectType

.class /*02000067*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.CompilerServices.Operators
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000062:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002DC*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2386
    // Code size       0 (0x0)
  } // end of method Operators::.cctor

  .method /*060002DD*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2387
    // Code size       0 (0x0)
  } // end of method Operators::.ctor

  .method /*060002DE*/ public static object 
          CompareObjectEqual(object Left,
                             object Right,
                             bool TextCompare) cil managed
  // SIG: 00 03 1C 1C 1C 02
  {
    // Method begins at RVA 0x2388
    // Code size       0 (0x0)
  } // end of method Operators::CompareObjectEqual

  .method /*060002DF*/ public static bool 
          ConditionalCompareObjectEqual(object Left,
                                        object Right,
                                        bool TextCompare) cil managed
  // SIG: 00 03 02 1C 1C 02
  {
    // Method begins at RVA 0x2389
    // Code size       0 (0x0)
  } // end of method Operators::ConditionalCompareObjectEqual

  .method /*060002E0*/ public static object 
          CompareObjectNotEqual(object Left,
                                object Right,
                                bool TextCompare) cil managed
  // SIG: 00 03 1C 1C 1C 02
  {
    // Method begins at RVA 0x238a
    // Code size       0 (0x0)
  } // end of method Operators::CompareObjectNotEqual

  .method /*060002E1*/ public static bool 
          ConditionalCompareObjectNotEqual(object Left,
                                           object Right,
                                           bool TextCompare) cil managed
  // SIG: 00 03 02 1C 1C 02
  {
    // Method begins at RVA 0x238b
    // Code size       0 (0x0)
  } // end of method Operators::ConditionalCompareObjectNotEqual

  .method /*060002E2*/ public static object 
          CompareObjectLess(object Left,
                            object Right,
                            bool TextCompare) cil managed
  // SIG: 00 03 1C 1C 1C 02
  {
    // Method begins at RVA 0x238c
    // Code size       0 (0x0)
  } // end of method Operators::CompareObjectLess

  .method /*060002E3*/ public static bool 
          ConditionalCompareObjectLess(object Left,
                                       object Right,
                                       bool TextCompare) cil managed
  // SIG: 00 03 02 1C 1C 02
  {
    // Method begins at RVA 0x238d
    // Code size       0 (0x0)
  } // end of method Operators::ConditionalCompareObjectLess

  .method /*060002E4*/ public static object 
          CompareObjectLessEqual(object Left,
                                 object Right,
                                 bool TextCompare) cil managed
  // SIG: 00 03 1C 1C 1C 02
  {
    // Method begins at RVA 0x238e
    // Code size       0 (0x0)
  } // end of method Operators::CompareObjectLessEqual

  .method /*060002E5*/ public static bool 
          ConditionalCompareObjectLessEqual(object Left,
                                            object Right,
                                            bool TextCompare) cil managed
  // SIG: 00 03 02 1C 1C 02
  {
    // Method begins at RVA 0x238f
    // Code size       0 (0x0)
  } // end of method Operators::ConditionalCompareObjectLessEqual

  .method /*060002E6*/ public static object 
          CompareObjectGreaterEqual(object Left,
                                    object Right,
                                    bool TextCompare) cil managed
  // SIG: 00 03 1C 1C 1C 02
  {
    // Method begins at RVA 0x2390
    // Code size       0 (0x0)
  } // end of method Operators::CompareObjectGreaterEqual

  .method /*060002E7*/ public static bool 
          ConditionalCompareObjectGreaterEqual(object Left,
                                               object Right,
                                               bool TextCompare) cil managed
  // SIG: 00 03 02 1C 1C 02
  {
    // Method begins at RVA 0x2391
    // Code size       0 (0x0)
  } // end of method Operators::ConditionalCompareObjectGreaterEqual

  .method /*060002E8*/ public static object 
          CompareObjectGreater(object Left,
                               object Right,
                               bool TextCompare) cil managed
  // SIG: 00 03 1C 1C 1C 02
  {
    // Method begins at RVA 0x2392
    // Code size       0 (0x0)
  } // end of method Operators::CompareObjectGreater

  .method /*060002E9*/ public static bool 
          ConditionalCompareObjectGreater(object Left,
                                          object Right,
                                          bool TextCompare) cil managed
  // SIG: 00 03 02 1C 1C 02
  {
    // Method begins at RVA 0x2393
    // Code size       0 (0x0)
  } // end of method Operators::ConditionalCompareObjectGreater

  .method /*060002EA*/ public static int32 
          CompareObject(object Left,
                        object Right,
                        bool TextCompare) cil managed
  // SIG: 00 03 08 1C 1C 02
  {
    // Method begins at RVA 0x2394
    // Code size       0 (0x0)
  } // end of method Operators::CompareObject

  .method /*060002EB*/ public static int32 
          CompareString(string Left,
                        string Right,
                        bool TextCompare) cil managed
  // SIG: 00 03 08 0E 0E 02
  {
    // Method begins at RVA 0x2395
    // Code size       0 (0x0)
  } // end of method Operators::CompareString

  .method /*060002EC*/ public static object 
          PlusObject(object Operand) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x2396
    // Code size       0 (0x0)
  } // end of method Operators::PlusObject

  .method /*060002ED*/ public static object 
          NegateObject(object Operand) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x2397
    // Code size       0 (0x0)
  } // end of method Operators::NegateObject

  .method /*060002EE*/ public static object 
          NotObject(object Operand) cil managed
  // SIG: 00 01 1C 1C
  {
    // Method begins at RVA 0x2398
    // Code size       0 (0x0)
  } // end of method Operators::NotObject

  .method /*060002EF*/ public static object 
          AndObject(object Left,
                    object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x2399
    // Code size       0 (0x0)
  } // end of method Operators::AndObject

  .method /*060002F0*/ public static object 
          OrObject(object Left,
                   object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x239a
    // Code size       0 (0x0)
  } // end of method Operators::OrObject

  .method /*060002F1*/ public static object 
          XorObject(object Left,
                    object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x239b
    // Code size       0 (0x0)
  } // end of method Operators::XorObject

  .method /*060002F2*/ public static object 
          AddObject(object Left,
                    object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x239c
    // Code size       0 (0x0)
  } // end of method Operators::AddObject

  .method /*060002F3*/ public static object 
          SubtractObject(object Left,
                         object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x239d
    // Code size       0 (0x0)
  } // end of method Operators::SubtractObject

  .method /*060002F4*/ public static object 
          MultiplyObject(object Left,
                         object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x239e
    // Code size       0 (0x0)
  } // end of method Operators::MultiplyObject

  .method /*060002F5*/ public static object 
          DivideObject(object Left,
                       object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x239f
    // Code size       0 (0x0)
  } // end of method Operators::DivideObject

  .method /*060002F6*/ public static object 
          ExponentObject(object Left,
                         object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x23a0
    // Code size       0 (0x0)
  } // end of method Operators::ExponentObject

  .method /*060002F7*/ public static object 
          ModObject(object Left,
                    object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x23a1
    // Code size       0 (0x0)
  } // end of method Operators::ModObject

  .method /*060002F8*/ public static object 
          IntDivideObject(object Left,
                          object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x23a2
    // Code size       0 (0x0)
  } // end of method Operators::IntDivideObject

  .method /*060002F9*/ public static object 
          LeftShiftObject(object Operand,
                          object Amount) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x23a3
    // Code size       0 (0x0)
  } // end of method Operators::LeftShiftObject

  .method /*060002FA*/ public static object 
          RightShiftObject(object Operand,
                           object Amount) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x23a4
    // Code size       0 (0x0)
  } // end of method Operators::RightShiftObject

  .method /*060002FB*/ public static object 
          LikeObject(object Source,
                     object Pattern,
                     valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ CompareOption) cil managed
  // SIG: 00 03 1C 1C 1C 11 80 E0
  {
    // Method begins at RVA 0x23a5
    // Code size       0 (0x0)
  } // end of method Operators::LikeObject

  .method /*060002FC*/ public static bool 
          LikeString(string Source,
                     string Pattern,
                     valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ CompareOption) cil managed
  // SIG: 00 03 02 0E 0E 11 80 E0
  {
    // Method begins at RVA 0x23a6
    // Code size       0 (0x0)
  } // end of method Operators::LikeString

  .method /*060002FD*/ public static object 
          ConcatenateObject(object Left,
                            object Right) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    // Method begins at RVA 0x23a7
    // Code size       0 (0x0)
  } // end of method Operators::ConcatenateObject

} // end of class Microsoft.VisualBasic.CompilerServices.Operators

.class /*02000068*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.ProjectData
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000064:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060002FE*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23a8
    // Code size       0 (0x0)
  } // end of method ProjectData::.ctor

  .method /*060002FF*/ public static class [mscorlib/*23000001*/]System.Exception/*0100000D*/ 
          CreateProjectError(int32 hr) cil managed
  // SIG: 00 01 12 35 08
  {
    // Method begins at RVA 0x23a9
    // Code size       0 (0x0)
  } // end of method ProjectData::CreateProjectError

  .method /*06000300*/ public hidebysig static 
          void  SetProjectError(class [mscorlib/*23000001*/]System.Exception/*0100000D*/ ex) cil managed
  // SIG: 00 01 01 12 35
  {
    .custom /*0C0000C9:0A00002C*/ instance void [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.ReliabilityContractAttribute/*0100005B*/::.ctor(valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Consistency/*0100005C*/,
                                                                                                                                                          valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Cer/*0100005D*/) /* 0A00002C */ = ( 01 00 03 00 00 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x23aa
    // Code size       0 (0x0)
  } // end of method ProjectData::SetProjectError

  .method /*06000301*/ public hidebysig static 
          void  SetProjectError(class [mscorlib/*23000001*/]System.Exception/*0100000D*/ ex,
                                int32 lErl) cil managed
  // SIG: 00 02 01 12 35 08
  {
    .custom /*0C0000CA:0A00002C*/ instance void [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.ReliabilityContractAttribute/*0100005B*/::.ctor(valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Consistency/*0100005C*/,
                                                                                                                                                          valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Cer/*0100005D*/) /* 0A00002C */ = ( 01 00 03 00 00 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x23ab
    // Code size       0 (0x0)
  } // end of method ProjectData::SetProjectError

  .method /*06000302*/ public static void 
          ClearProjectError() cil managed
  // SIG: 00 00 01
  {
    .custom /*0C0000CB:0A00002C*/ instance void [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.ReliabilityContractAttribute/*0100005B*/::.ctor(valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Consistency/*0100005C*/,
                                                                                                                                                          valuetype [mscorlib/*23000001*/]System.Runtime.ConstrainedExecution.Cer/*0100005D*/) /* 0A00002C */ = ( 01 00 03 00 00 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x23ac
    // Code size       0 (0x0)
  } // end of method ProjectData::ClearProjectError

  .method /*06000303*/ public static void 
          EndApp() cil managed
  // SIG: 00 00 01
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(8)}}
    .permissionset demand
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(2)}}
    // Method begins at RVA 0x23ad
    // Code size       0 (0x0)
  } // end of method ProjectData::EndApp

} // end of class Microsoft.VisualBasic.CompilerServices.ProjectData

.class /*02000069*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.ShortType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000066:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000304*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23ae
    // Code size       0 (0x0)
  } // end of method ShortType::.ctor

  .method /*06000305*/ public static int16 
          FromString(string Value) cil managed
  // SIG: 00 01 06 0E
  {
    // Method begins at RVA 0x23af
    // Code size       0 (0x0)
  } // end of method ShortType::FromString

  .method /*06000306*/ public static int16 
          FromObject(object Value) cil managed
  // SIG: 00 01 06 1C
  {
    // Method begins at RVA 0x23b0
    // Code size       0 (0x0)
  } // end of method ShortType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.ShortType

.class /*0200006A*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.SingleType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000068:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000307*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23b1
    // Code size       0 (0x0)
  } // end of method SingleType::.ctor

  .method /*06000308*/ public static float32 
          FromString(string Value) cil managed
  // SIG: 00 01 0C 0E
  {
    // Method begins at RVA 0x23b2
    // Code size       0 (0x0)
  } // end of method SingleType::FromString

  .method /*06000309*/ public static float32 
          FromString(string Value,
                     class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0C 0E 12 80 CD
  {
    // Method begins at RVA 0x23b3
    // Code size       0 (0x0)
  } // end of method SingleType::FromString

  .method /*0600030A*/ public static float32 
          FromObject(object Value) cil managed
  // SIG: 00 01 0C 1C
  {
    // Method begins at RVA 0x23b4
    // Code size       0 (0x0)
  } // end of method SingleType::FromObject

  .method /*0600030B*/ public static float32 
          FromObject(object Value,
                     class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0C 1C 12 80 CD
  {
    // Method begins at RVA 0x23b5
    // Code size       0 (0x0)
  } // end of method SingleType::FromObject

} // end of class Microsoft.VisualBasic.CompilerServices.SingleType

.class /*0200006B*/ public auto ansi serializable sealed Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00006A:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000124*/ public int16 State
  .method /*0600030C*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23b6
    // Code size       0 (0x0)
  } // end of method StaticLocalInitFlag::.ctor

} // end of class Microsoft.VisualBasic.CompilerServices.StaticLocalInitFlag

.class /*0200006C*/ public auto ansi serializable sealed Microsoft.VisualBasic.CompilerServices.IncompleteInitialization
       extends [mscorlib/*23000001*/]System.Exception/*0100000D*/
{
  .custom /*0C00006C:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600030D*/ private specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000013*/ info,
                               valuetype [mscorlib/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000014*/ context) cil managed
  // SIG: 20 02 01 12 4D 11 51
  {
    .custom /*0C0000CC:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x23b7
    // Code size       0 (0x0)
  } // end of method IncompleteInitialization::.ctor

  .method /*0600030E*/ public specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000CD:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x23b8
    // Code size       0 (0x0)
  } // end of method IncompleteInitialization::.ctor

  .method /*0600030F*/ public specialname rtspecialname 
          instance void  .ctor(string message,
                               class [mscorlib/*23000001*/]System.Exception/*0100000D*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 35
  {
    .custom /*0C0000CE:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x23b9
    // Code size       0 (0x0)
  } // end of method IncompleteInitialization::.ctor

  .method /*06000310*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23ba
    // Code size       0 (0x0)
  } // end of method IncompleteInitialization::.ctor

} // end of class Microsoft.VisualBasic.CompilerServices.IncompleteInitialization

.class /*0200006D*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.StringType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00006E:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000311*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23bb
    // Code size       0 (0x0)
  } // end of method StringType::.ctor

  .method /*06000312*/ public static string 
          FromBoolean(bool Value) cil managed
  // SIG: 00 01 0E 02
  {
    // Method begins at RVA 0x23bc
    // Code size       0 (0x0)
  } // end of method StringType::FromBoolean

  .method /*06000313*/ public static string 
          FromByte(uint8 Value) cil managed
  // SIG: 00 01 0E 05
  {
    // Method begins at RVA 0x23bd
    // Code size       0 (0x0)
  } // end of method StringType::FromByte

  .method /*06000314*/ public static string 
          FromChar(char Value) cil managed
  // SIG: 00 01 0E 03
  {
    // Method begins at RVA 0x23be
    // Code size       0 (0x0)
  } // end of method StringType::FromChar

  .method /*06000315*/ public static string 
          FromShort(int16 Value) cil managed
  // SIG: 00 01 0E 06
  {
    // Method begins at RVA 0x23bf
    // Code size       0 (0x0)
  } // end of method StringType::FromShort

  .method /*06000316*/ public static string 
          FromInteger(int32 Value) cil managed
  // SIG: 00 01 0E 08
  {
    // Method begins at RVA 0x23c0
    // Code size       0 (0x0)
  } // end of method StringType::FromInteger

  .method /*06000317*/ public static string 
          FromLong(int64 Value) cil managed
  // SIG: 00 01 0E 0A
  {
    // Method begins at RVA 0x23c1
    // Code size       0 (0x0)
  } // end of method StringType::FromLong

  .method /*06000318*/ public static string 
          FromSingle(float32 Value) cil managed
  // SIG: 00 01 0E 0C
  {
    // Method begins at RVA 0x23c2
    // Code size       0 (0x0)
  } // end of method StringType::FromSingle

  .method /*06000319*/ public static string 
          FromDouble(float64 Value) cil managed
  // SIG: 00 01 0E 0D
  {
    // Method begins at RVA 0x23c3
    // Code size       0 (0x0)
  } // end of method StringType::FromDouble

  .method /*0600031A*/ public static string 
          FromSingle(float32 Value,
                     class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0E 0C 12 80 CD
  {
    // Method begins at RVA 0x23c4
    // Code size       0 (0x0)
  } // end of method StringType::FromSingle

  .method /*0600031B*/ public static string 
          FromDouble(float64 Value,
                     class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0E 0D 12 80 CD
  {
    // Method begins at RVA 0x23c5
    // Code size       0 (0x0)
  } // end of method StringType::FromDouble

  .method /*0600031C*/ public static string 
          FromDate(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Value) cil managed
  // SIG: 00 01 0E 11 80 81
  {
    // Method begins at RVA 0x23c6
    // Code size       0 (0x0)
  } // end of method StringType::FromDate

  .method /*0600031D*/ public static string 
          FromDecimal(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ Value) cil managed
  // SIG: 00 01 0E 11 7D
  {
    // Method begins at RVA 0x23c7
    // Code size       0 (0x0)
  } // end of method StringType::FromDecimal

  .method /*0600031E*/ public static string 
          FromDecimal(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ Value,
                      class [mscorlib/*23000001*/]System.Globalization.NumberFormatInfo/*01000033*/ NumberFormat) cil managed
  // SIG: 00 02 0E 11 7D 12 80 CD
  {
    // Method begins at RVA 0x23c8
    // Code size       0 (0x0)
  } // end of method StringType::FromDecimal

  .method /*0600031F*/ public static string 
          FromObject(object Value) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x23c9
    // Code size       0 (0x0)
  } // end of method StringType::FromObject

  .method /*06000320*/ public static int32 
          StrCmp(string sLeft,
                 string sRight,
                 bool TextCompare) cil managed
  // SIG: 00 03 08 0E 0E 02
  {
    // Method begins at RVA 0x23ca
    // Code size       0 (0x0)
  } // end of method StringType::StrCmp

  .method /*06000321*/ public static bool 
          StrLike(string Source,
                  string Pattern,
                  valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ CompareOption) cil managed
  // SIG: 00 03 02 0E 0E 11 80 E0
  {
    // Method begins at RVA 0x23cb
    // Code size       0 (0x0)
  } // end of method StringType::StrLike

  .method /*06000322*/ public static bool 
          StrLikeBinary(string Source,
                        string Pattern) cil managed
  // SIG: 00 02 02 0E 0E
  {
    // Method begins at RVA 0x23cc
    // Code size       0 (0x0)
  } // end of method StringType::StrLikeBinary

  .method /*06000323*/ public static bool 
          StrLikeText(string Source,
                      string Pattern) cil managed
  // SIG: 00 02 02 0E 0E
  {
    // Method begins at RVA 0x23cd
    // Code size       0 (0x0)
  } // end of method StringType::StrLikeText

  .method /*06000324*/ public static void 
          MidStmtStr(string& sDest,
                     int32 StartPosition,
                     int32 MaxInsertLength,
                     string sInsert) cil managed
  // SIG: 00 04 01 10 0E 08 08 0E
  {
    // Method begins at RVA 0x23ce
    // Code size       0 (0x0)
  } // end of method StringType::MidStmtStr

} // end of class Microsoft.VisualBasic.CompilerServices.StringType

.class /*0200006E*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.CompilerServices.Utils
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000070:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000325*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23cf
    // Code size       0 (0x0)
  } // end of method Utils::.cctor

  .method /*06000326*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d0
    // Code size       0 (0x0)
  } // end of method Utils::.ctor

  .method /*06000327*/ public static string 
          GetResourceString(string ResourceKey,
                            string[] Args) cil managed
  // SIG: 00 02 0E 0E 1D 0E
  {
    .param [2]/*0800049D*/ 
    .custom /*0C0000D6:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d1
    // Code size       0 (0x0)
  } // end of method Utils::GetResourceString

  .method /*06000328*/ public static void 
          ThrowException(int32 hr) cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x23d2
    // Code size       0 (0x0)
  } // end of method Utils::ThrowException

  .method /*06000329*/ public static object 
          SetCultureInfo(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000002*/ Culture) cil managed
  // SIG: 00 01 1C 12 09
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(32)}}
    // Method begins at RVA 0x23d3
    // Code size       0 (0x0)
  } // end of method Utils::SetCultureInfo

  .method /*0600032A*/ public static class [mscorlib/*23000001*/]System.Array/*0100001E*/ 
          CopyArray(class [mscorlib/*23000001*/]System.Array/*0100001E*/ arySrc,
                    class [mscorlib/*23000001*/]System.Array/*0100001E*/ aryDest) cil managed
  // SIG: 00 02 12 79 12 79 12 79
  {
    // Method begins at RVA 0x23d4
    // Code size       0 (0x0)
  } // end of method Utils::CopyArray

  .method /*0600032B*/ public static string 
          MethodToString(class [mscorlib/*23000001*/]System.Reflection.MethodBase/*01000035*/ Method) cil managed
  // SIG: 00 01 0E 12 80 D5
  {
    // Method begins at RVA 0x23d5
    // Code size       0 (0x0)
  } // end of method Utils::MethodToString

} // end of class Microsoft.VisualBasic.CompilerServices.Utils

.class /*0200006F*/ public auto ansi sealed Microsoft.VisualBasic.CompilerServices.Versioned
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000073:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600032C*/ private specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d6
    // Code size       0 (0x0)
  } // end of method Versioned::.ctor

  .method /*0600032D*/ public static object 
          CallByName(object Instance,
                     string MethodName,
                     valuetype Microsoft.VisualBasic.CallType/*02000037*/ UseCallType,
                     object[] Arguments) cil managed
  // SIG: 00 04 1C 1C 0E 11 80 DC 1D 1C
  {
    .param [4]/*080004A6*/ 
    .custom /*0C0000D7:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d8
    // Code size       0 (0x0)
  } // end of method Versioned::CallByName

  .method /*0600032E*/ public static bool 
          IsNumeric(object Expression) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x23e4
    // Code size       0 (0x0)
  } // end of method Versioned::IsNumeric

  .method /*0600032F*/ public static string 
          TypeName(object Expression) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x23e5
    // Code size       0 (0x0)
  } // end of method Versioned::TypeName

  .method /*06000330*/ public static string 
          SystemTypeName(string VbName) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x23e6
    // Code size       0 (0x0)
  } // end of method Versioned::SystemTypeName

  .method /*06000331*/ public static string 
          VbTypeName(string SystemName) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x23e7
    // Code size       0 (0x0)
  } // end of method Versioned::VbTypeName

} // end of class Microsoft.VisualBasic.CompilerServices.Versioned

.class /*02000070*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.Information
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000075:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .method /*06000332*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23e8
    // Code size       0 (0x0)
  } // end of method Information::.cctor

  .method /*06000333*/ public static class Microsoft.VisualBasic.ErrObject/*02000028*/ 
          Err() cil managed
  // SIG: 00 00 12 80 A0
  {
    // Method begins at RVA 0x23e9
    // Code size       0 (0x0)
  } // end of method Information::Err

  .method /*06000334*/ public static int32 
          Erl() cil managed
  // SIG: 00 00 08
  {
    .custom /*0C0000CF:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x23ea
    // Code size       0 (0x0)
  } // end of method Information::Erl

  .method /*06000335*/ public static bool 
          IsArray(object VarName) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x23eb
    // Code size       0 (0x0)
  } // end of method Information::IsArray

  .method /*06000336*/ public static bool 
          IsDate(object Expression) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x23ec
    // Code size       0 (0x0)
  } // end of method Information::IsDate

  .method /*06000337*/ public static bool 
          IsDBNull(object Expression) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x23ed
    // Code size       0 (0x0)
  } // end of method Information::IsDBNull

  .method /*06000338*/ public static bool 
          IsNothing(object Expression) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x23ee
    // Code size       0 (0x0)
  } // end of method Information::IsNothing

  .method /*06000339*/ public static bool 
          IsError(object Expression) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x23ef
    // Code size       0 (0x0)
  } // end of method Information::IsError

  .method /*0600033A*/ public static bool 
          IsReference(object Expression) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x23f0
    // Code size       0 (0x0)
  } // end of method Information::IsReference

  .method /*0600033B*/ public static int32 
          LBound(class [mscorlib/*23000001*/]System.Array/*0100001E*/ Array,
                 [opt] int32 Rank) cil managed
  // SIG: 00 02 08 12 79 08
  {
    .param [2]/*080004B2*/  = int32(0x00000001)
    // Method begins at RVA 0x23f1
    // Code size       0 (0x0)
  } // end of method Information::LBound

  .method /*0600033C*/ public static int32 
          UBound(class [mscorlib/*23000001*/]System.Array/*0100001E*/ Array,
                 [opt] int32 Rank) cil managed
  // SIG: 00 02 08 12 79 08
  {
    .param [2]/*080004B4*/  = int32(0x00000001)
    // Method begins at RVA 0x23f2
    // Code size       0 (0x0)
  } // end of method Information::UBound

  .method /*0600033D*/ public static int32 
          QBColor(int32 Color) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x23f3
    // Code size       0 (0x0)
  } // end of method Information::QBColor

  .method /*0600033E*/ public static int32 
          RGB(int32 Red,
              int32 Green,
              int32 Blue) cil managed
  // SIG: 00 03 08 08 08 08
  {
    // Method begins at RVA 0x23f4
    // Code size       0 (0x0)
  } // end of method Information::RGB

  .method /*0600033F*/ public static valuetype Microsoft.VisualBasic.VariantType/*02000035*/ 
          VarType(object VarName) cil managed
  // SIG: 00 01 11 80 D4 1C
  {
    // Method begins at RVA 0x23f5
    // Code size       0 (0x0)
  } // end of method Information::VarType

  .method /*06000340*/ public static bool 
          IsNumeric(object Expression) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x23f6
    // Code size       0 (0x0)
  } // end of method Information::IsNumeric

  .method /*06000341*/ public static string 
          TypeName(object VarName) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x23f7
    // Code size       0 (0x0)
  } // end of method Information::TypeName

  .method /*06000342*/ public static string 
          SystemTypeName(string VbName) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x23f8
    // Code size       0 (0x0)
  } // end of method Information::SystemTypeName

  .method /*06000343*/ public static string 
          VbTypeName(string UrtName) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x23f9
    // Code size       0 (0x0)
  } // end of method Information::VbTypeName

  .method /*06000344*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23fa
    // Code size       0 (0x0)
  } // end of method Information::.ctor

} // end of class Microsoft.VisualBasic.Information

.class /*02000071*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.Interaction
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000077:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .method /*06000345*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23fb
    // Code size       0 (0x0)
  } // end of method Interaction::.cctor

  .method /*06000346*/ public static int32 
          Shell(string PathName,
                [opt] valuetype Microsoft.VisualBasic.AppWinStyle/*02000036*/ Style,
                [opt] bool Wait,
                [opt] int32 Timeout) cil managed
  // SIG: 00 04 08 0E 11 80 D8 02 08
  {
    .param [2]/*080004BF*/  = int16(0x0002)
    .param [3]/*080004C0*/  = bool(false)
    .param [4]/*080004C1*/  = int32(0xFFFFFFFF)
    .permissionset demand
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(2)}}
    // Method begins at RVA 0x23fc
    // Code size       0 (0x0)
  } // end of method Interaction::Shell

  .method /*06000347*/ public static void 
          AppActivate(int32 ProcessId) cil managed
  // SIG: 00 01 01 08
  {
    .permissionset demand
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(2)}}
    // Method begins at RVA 0x2408
    // Code size       0 (0x0)
  } // end of method Interaction::AppActivate

  .method /*06000348*/ public static void 
          AppActivate(string Title) cil managed
  // SIG: 00 01 01 0E
  {
    .permissionset demand
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(2)}}
    // Method begins at RVA 0x2409
    // Code size       0 (0x0)
  } // end of method Interaction::AppActivate

  .method /*06000349*/ public static string 
          Command() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x240a
    // Code size       0 (0x0)
  } // end of method Interaction::Command

  .method /*0600034A*/ public static string 
          Environ(int32 Expression) cil managed
  // SIG: 00 01 0E 08
  {
    // Method begins at RVA 0x240b
    // Code size       0 (0x0)
  } // end of method Interaction::Environ

  .method /*0600034B*/ public static string 
          Environ(string Expression) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x240c
    // Code size       0 (0x0)
  } // end of method Interaction::Environ

  .method /*0600034C*/ public static void 
          Beep() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x240d
    // Code size       0 (0x0)
  } // end of method Interaction::Beep

  .method /*0600034D*/ public static string 
          InputBox(string Prompt,
                   [opt] string Title,
                   [opt] string DefaultResponse,
                   [opt] int32 XPos,
                   [opt] int32 YPos) cil managed
  // SIG: 00 05 0E 0E 0E 0E 08 08
  {
    .param [2]/*080004C7*/  = ""
    .param [3]/*080004C8*/  = ""
    .param [4]/*080004C9*/  = int32(0xFFFFFFFF)
    .param [5]/*080004CA*/  = int32(0xFFFFFFFF)
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(128)}}
    // Method begins at RVA 0x240e
    // Code size       0 (0x0)
  } // end of method Interaction::InputBox

  .method /*0600034E*/ public static valuetype Microsoft.VisualBasic.MsgBoxResult/*02000046*/ 
          MsgBox(object Prompt,
                 [opt] valuetype Microsoft.VisualBasic.MsgBoxStyle/*02000047*/ Buttons,
                 [opt] object Title) cil managed
  // SIG: 00 03 11 81 18 1C 11 81 1C 1C
  {
    .param [2]/*080004CC*/  = int32(0x00000000)
    .param [3]/*080004CD*/  = nullref
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(128)}}
    // Method begins at RVA 0x240f
    // Code size       0 (0x0)
  } // end of method Interaction::MsgBox

  .method /*0600034F*/ public static object 
          Choose(float64 Index,
                 object[] Choice) cil managed
  // SIG: 00 02 1C 0D 1D 1C
  {
    .param [2]/*080004CF*/ 
    .custom /*0C0000D8:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2410
    // Code size       0 (0x0)
  } // end of method Interaction::Choose

  .method /*06000350*/ public static object 
          IIf(bool Expression,
              object TruePart,
              object FalsePart) cil managed
  // SIG: 00 03 1C 02 1C 1C
  {
    // Method begins at RVA 0x2411
    // Code size       0 (0x0)
  } // end of method Interaction::IIf

  .method /*06000351*/ public static string 
          Partition(int64 Number,
                    int64 Start,
                    int64 Stop,
                    int64 Interval) cil managed
  // SIG: 00 04 0E 0A 0A 0A 0A
  {
    // Method begins at RVA 0x2412
    // Code size       0 (0x0)
  } // end of method Interaction::Partition

  .method /*06000352*/ public static object 
          Switch(object[] VarExpr) cil managed
  // SIG: 00 01 1C 1D 1C
  {
    .param [1]/*080004D7*/ 
    .custom /*0C0000D9:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2413
    // Code size       0 (0x0)
  } // end of method Interaction::Switch

  .method /*06000353*/ public static void 
          DeleteSetting(string AppName,
                        [opt] string Section,
                        [opt] string Key) cil managed
  // SIG: 00 03 01 0E 0E 0E
  {
    .param [2]/*080004D9*/  = nullref
    .param [3]/*080004DA*/  = nullref
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2414
    // Code size       0 (0x0)
  } // end of method Interaction::DeleteSetting

  .method /*06000354*/ public static string[0...,0...] 
          GetAllSettings(string AppName,
                         string Section) cil managed
  // SIG: 00 02 14 0E 02 00 02 00 00 0E 0E
  {
    // Method begins at RVA 0x2415
    // Code size       0 (0x0)
  } // end of method Interaction::GetAllSettings

  .method /*06000355*/ public static string 
          GetSetting(string AppName,
                     string Section,
                     string Key,
                     [opt] string Default) cil managed
  // SIG: 00 04 0E 0E 0E 0E 0E
  {
    .param [4]/*080004E0*/  = ""
    // Method begins at RVA 0x2416
    // Code size       0 (0x0)
  } // end of method Interaction::GetSetting

  .method /*06000356*/ public static void 
          SaveSetting(string AppName,
                      string Section,
                      string Key,
                      string Setting) cil managed
  // SIG: 00 04 01 0E 0E 0E 0E
  {
    // Method begins at RVA 0x2417
    // Code size       0 (0x0)
  } // end of method Interaction::SaveSetting

  .method /*06000357*/ public static object 
          CreateObject(string ProgId,
                       [opt] string ServerName) cil managed
  // SIG: 00 02 1C 0E 0E
  {
    .param [2]/*080004E6*/  = ""
    .permissionset demand
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(2)}}
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2418
    // Code size       0 (0x0)
  } // end of method Interaction::CreateObject

  .method /*06000358*/ public static object 
          GetObject([opt] string PathName,
                    [opt] string Class) cil managed
  // SIG: 00 02 1C 0E 0E
  {
    .param [1]/*080004E7*/  = nullref
    .param [2]/*080004E8*/  = nullref
    .permissionset demand
               = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.SecurityPermissionFlag, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Flags' = int32(2)}}
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x2419
    // Code size       0 (0x0)
  } // end of method Interaction::GetObject

  .method /*06000359*/ public static object 
          CallByName(object ObjectRef,
                     string ProcName,
                     valuetype Microsoft.VisualBasic.CallType/*02000037*/ UseCallType,
                     object[] Args) cil managed
  // SIG: 00 04 1C 1C 0E 11 80 DC 1D 1C
  {
    .param [4]/*080004EC*/ 
    .custom /*0C0000DA:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x241a
    // Code size       0 (0x0)
  } // end of method Interaction::CallByName

  .method /*0600035A*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x241b
    // Code size       0 (0x0)
  } // end of method Interaction::.ctor

} // end of class Microsoft.VisualBasic.Interaction

.class /*02000072*/ public auto ansi sealed Microsoft.VisualBasic.Logging.LogFileLocation
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*04000125*/ public specialname rtspecialname int32 value__
  .field /*04000126*/ public static literal valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/ TempDirectory = int32(0x00000000)
  .field /*04000127*/ public static literal valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/ LocalUserApplicationDirectory = int32(0x00000001)
  .field /*04000128*/ public static literal valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/ CommonApplicationDirectory = int32(0x00000002)
  .field /*04000129*/ public static literal valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/ ExecutableDirectory = int32(0x00000003)
  .field /*0400012A*/ public static literal valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/ Custom = int32(0x00000004)
} // end of class Microsoft.VisualBasic.Logging.LogFileLocation

.class /*02000073*/ public auto ansi sealed Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*0400012B*/ public specialname rtspecialname int32 value__
  .field /*0400012C*/ public static literal valuetype Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption/*02000073*/ None = int32(0x00000000)
  .field /*0400012D*/ public static literal valuetype Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption/*02000073*/ Daily = int32(0x00000001)
  .field /*0400012E*/ public static literal valuetype Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption/*02000073*/ Weekly = int32(0x00000002)
} // end of class Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption

.class /*02000074*/ public auto ansi sealed Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption
       extends [mscorlib/*23000001*/]System.Enum/*01000008*/
{
  .field /*0400012F*/ public specialname rtspecialname int32 value__
  .field /*04000130*/ public static literal valuetype Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption/*02000074*/ ThrowException = int32(0x00000000)
  .field /*04000131*/ public static literal valuetype Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption/*02000074*/ DiscardMessages = int32(0x00000001)
} // end of class Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption

.class /*02000075*/ public auto ansi beforefieldinit Microsoft.VisualBasic.Logging.FileLogTraceListener
       extends [System/*23000003*/]System.Diagnostics.TraceListener/*01000036*/
{
  .custom /*0C000078:0A000018*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000045*/::.ctor(bool) /* 0A000018 */ = ( 01 00 00 00 00 ) 
  .method /*0600035B*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x241c
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::.cctor

  .method /*0600035C*/ public specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x241d
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::.ctor

  .method /*0600035D*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
    // Method begins at RVA 0x241e
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::.ctor

  .method /*0600035E*/ public specialname 
          instance valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/ 
          get_Location() cil managed
  // SIG: 20 00 11 81 C8
  {
    // Method begins at RVA 0x241f
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_Location

  .method /*0600035F*/ public specialname 
          instance void  set_Location(valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/ 'value') cil managed
  // SIG: 20 01 01 11 81 C8
  {
    // Method begins at RVA 0x2420
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_Location

  .method /*06000360*/ public specialname 
          instance bool  get_AutoFlush() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2421
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_AutoFlush

  .method /*06000361*/ public specialname 
          instance void  set_AutoFlush(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2422
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_AutoFlush

  .method /*06000362*/ public specialname 
          instance bool  get_IncludeHostName() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2423
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_IncludeHostName

  .method /*06000363*/ public specialname 
          instance void  set_IncludeHostName(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2424
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_IncludeHostName

  .method /*06000364*/ public specialname 
          instance bool  get_Append() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2425
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_Append

  .method /*06000365*/ public specialname 
          instance void  set_Append(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2426
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_Append

  .method /*06000366*/ public specialname 
          instance valuetype Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption/*02000074*/ 
          get_DiskSpaceExhaustedBehavior() cil managed
  // SIG: 20 00 11 81 D0
  {
    // Method begins at RVA 0x2427
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_DiskSpaceExhaustedBehavior

  .method /*06000367*/ public specialname 
          instance void  set_DiskSpaceExhaustedBehavior(valuetype Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption/*02000074*/ 'value') cil managed
  // SIG: 20 01 01 11 81 D0
  {
    // Method begins at RVA 0x2428
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_DiskSpaceExhaustedBehavior

  .method /*06000368*/ public specialname 
          instance string  get_BaseFileName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2429
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_BaseFileName

  .method /*06000369*/ public specialname 
          instance void  set_BaseFileName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x242a
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_BaseFileName

  .method /*0600036A*/ public specialname 
          instance string  get_FullLogFileName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x242b
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_FullLogFileName

  .method /*0600036B*/ public specialname 
          instance valuetype Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption/*02000073*/ 
          get_LogFileCreationSchedule() cil managed
  // SIG: 20 00 11 81 CC
  {
    // Method begins at RVA 0x242c
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_LogFileCreationSchedule

  .method /*0600036C*/ public specialname 
          instance void  set_LogFileCreationSchedule(valuetype Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption/*02000073*/ 'value') cil managed
  // SIG: 20 01 01 11 81 CC
  {
    // Method begins at RVA 0x242d
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_LogFileCreationSchedule

  .method /*0600036D*/ public specialname 
          instance int64  get_MaxFileSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x242e
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_MaxFileSize

  .method /*0600036E*/ public specialname 
          instance void  set_MaxFileSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x242f
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_MaxFileSize

  .method /*0600036F*/ public specialname 
          instance int64  get_ReserveDiskSpace() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2430
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_ReserveDiskSpace

  .method /*06000370*/ public specialname 
          instance void  set_ReserveDiskSpace(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2431
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_ReserveDiskSpace

  .method /*06000371*/ public specialname 
          instance string  get_Delimiter() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2432
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_Delimiter

  .method /*06000372*/ public specialname 
          instance void  set_Delimiter(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2433
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_Delimiter

  .method /*06000373*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ 
          get_Encoding() cil managed
  // SIG: 20 00 12 80 B5
  {
    // Method begins at RVA 0x2434
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_Encoding

  .method /*06000374*/ public specialname 
          instance void  set_Encoding(class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B5
  {
    // Method begins at RVA 0x2435
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_Encoding

  .method /*06000375*/ public specialname 
          instance string  get_CustomLocation() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2436
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::get_CustomLocation

  .method /*06000376*/ public specialname 
          instance void  set_CustomLocation(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2437
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::set_CustomLocation

  .method /*06000377*/ public hidebysig strict virtual 
          instance void  Write(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x2438
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::Write

  .method /*06000378*/ public hidebysig strict virtual 
          instance void  WriteLine(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x2439
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::WriteLine

  .method /*06000379*/ public strict virtual 
          instance void  TraceEvent(class [System/*23000003*/]System.Diagnostics.TraceEventCache/*01000037*/ eventCache,
                                    string source,
                                    valuetype [System/*23000003*/]System.Diagnostics.TraceEventType/*01000038*/ eventType,
                                    int32 id,
                                    string message) cil managed
  // SIG: 20 05 01 12 80 DD 0E 11 80 E1 08 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x243c
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::TraceEvent

  .method /*0600037A*/ public strict virtual 
          instance void  TraceEvent(class [System/*23000003*/]System.Diagnostics.TraceEventCache/*01000037*/ eventCache,
                                    string source,
                                    valuetype [System/*23000003*/]System.Diagnostics.TraceEventType/*01000038*/ eventType,
                                    int32 id,
                                    string format,
                                    object[] args) cil managed
  // SIG: 20 06 01 12 80 DD 0E 11 80 E1 08 0E 1D 1C
  {
    .param [6]/*08000506*/ 
    .custom /*0C0000DB:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x2448
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::TraceEvent

  .method /*0600037B*/ public strict virtual 
          instance void  TraceData(class [System/*23000003*/]System.Diagnostics.TraceEventCache/*01000037*/ eventCache,
                                   string source,
                                   valuetype [System/*23000003*/]System.Diagnostics.TraceEventType/*01000038*/ eventType,
                                   int32 id,
                                   object data) cil managed
  // SIG: 20 05 01 12 80 DD 0E 11 80 E1 08 1C
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x2449
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::TraceData

  .method /*0600037C*/ public strict virtual 
          instance void  TraceData(class [System/*23000003*/]System.Diagnostics.TraceEventCache/*01000037*/ eventCache,
                                   string source,
                                   valuetype [System/*23000003*/]System.Diagnostics.TraceEventType/*01000038*/ eventType,
                                   int32 id,
                                   object[] data) cil managed
  // SIG: 20 05 01 12 80 DD 0E 11 80 E1 08 1D 1C
  {
    .param [5]/*08000510*/ 
    .custom /*0C0000DC:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x244a
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::TraceData

  .method /*0600037D*/ public strict virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x244b
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::Flush

  .method /*0600037E*/ public strict virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x244c
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::Close

  .method /*0600037F*/ family strict virtual 
          instance string[]  GetSupportedAttributes() cil managed
  // SIG: 20 00 1D 0E
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x244d
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::GetSupportedAttributes

  .method /*06000380*/ family strict virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    .permissionset linkcheck
               = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Synchronization' = bool(true)}}
    // Method begins at RVA 0x244e
    // Code size       0 (0x0)
  } // end of method FileLogTraceListener::Dispose

  .property /*17000088*/ instance callconv(8) valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/
          Location()
  {
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_Location(valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/) /* 0600035F */
    .get instance valuetype Microsoft.VisualBasic.Logging.LogFileLocation/*02000072*/ Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_Location() /* 0600035E */
  } // end of property FileLogTraceListener::Location
  .property /*17000089*/ instance callconv(8) bool
          AutoFlush()
  {
    .get instance bool Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_AutoFlush() /* 06000360 */
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_AutoFlush(bool) /* 06000361 */
  } // end of property FileLogTraceListener::AutoFlush
  .property /*1700008A*/ instance callconv(8) bool
          IncludeHostName()
  {
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_IncludeHostName(bool) /* 06000363 */
    .get instance bool Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_IncludeHostName() /* 06000362 */
  } // end of property FileLogTraceListener::IncludeHostName
  .property /*1700008B*/ instance callconv(8) bool
          Append()
  {
    .get instance bool Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_Append() /* 06000364 */
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_Append(bool) /* 06000365 */
  } // end of property FileLogTraceListener::Append
  .property /*1700008C*/ instance callconv(8) valuetype Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption/*02000074*/
          DiskSpaceExhaustedBehavior()
  {
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_DiskSpaceExhaustedBehavior(valuetype Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption/*02000074*/) /* 06000367 */
    .get instance valuetype Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption/*02000074*/ Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_DiskSpaceExhaustedBehavior() /* 06000366 */
  } // end of property FileLogTraceListener::DiskSpaceExhaustedBehavior
  .property /*1700008D*/ instance callconv(8) string
          BaseFileName()
  {
    .get instance string Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_BaseFileName() /* 06000368 */
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_BaseFileName(string) /* 06000369 */
  } // end of property FileLogTraceListener::BaseFileName
  .property /*1700008E*/ instance callconv(8) string
          FullLogFileName()
  {
    .get instance string Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_FullLogFileName() /* 0600036A */
  } // end of property FileLogTraceListener::FullLogFileName
  .property /*1700008F*/ instance callconv(8) valuetype Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption/*02000073*/
          LogFileCreationSchedule()
  {
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_LogFileCreationSchedule(valuetype Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption/*02000073*/) /* 0600036C */
    .get instance valuetype Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption/*02000073*/ Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_LogFileCreationSchedule() /* 0600036B */
  } // end of property FileLogTraceListener::LogFileCreationSchedule
  .property /*17000090*/ instance callconv(8) int64
          MaxFileSize()
  {
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_MaxFileSize(int64) /* 0600036E */
    .get instance int64 Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_MaxFileSize() /* 0600036D */
  } // end of property FileLogTraceListener::MaxFileSize
  .property /*17000091*/ instance callconv(8) int64
          ReserveDiskSpace()
  {
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_ReserveDiskSpace(int64) /* 06000370 */
    .get instance int64 Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_ReserveDiskSpace() /* 0600036F */
  } // end of property FileLogTraceListener::ReserveDiskSpace
  .property /*17000092*/ instance callconv(8) string
          Delimiter()
  {
    .get instance string Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_Delimiter() /* 06000371 */
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_Delimiter(string) /* 06000372 */
  } // end of property FileLogTraceListener::Delimiter
  .property /*17000093*/ instance callconv(8) class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/
          Encoding()
  {
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_Encoding(class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/) /* 06000374 */
    .get instance class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_Encoding() /* 06000373 */
  } // end of property FileLogTraceListener::Encoding
  .property /*17000094*/ instance callconv(8) string
          CustomLocation()
  {
    .set instance void Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::set_CustomLocation(string) /* 06000376 */
    .get instance string Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/::get_CustomLocation() /* 06000375 */
  } // end of property FileLogTraceListener::CustomLocation
} // end of class Microsoft.VisualBasic.Logging.FileLogTraceListener

.class /*02000076*/ public auto ansi beforefieldinit Microsoft.VisualBasic.Logging.Log
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000381*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x244f
    // Code size       0 (0x0)
  } // end of method Log::.cctor

  .method /*06000382*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2450
    // Code size       0 (0x0)
  } // end of method Log::.ctor

  .method /*06000383*/ public specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2451
    // Code size       0 (0x0)
  } // end of method Log::.ctor

  .method /*06000384*/ public instance void 
          WriteEntry(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2452
    // Code size       0 (0x0)
  } // end of method Log::WriteEntry

  .method /*06000385*/ public instance void 
          WriteEntry(string message,
                     valuetype [System/*23000003*/]System.Diagnostics.TraceEventType/*01000038*/ severity) cil managed
  // SIG: 20 02 01 0E 11 80 E1
  {
    // Method begins at RVA 0x2453
    // Code size       0 (0x0)
  } // end of method Log::WriteEntry

  .method /*06000386*/ public instance void 
          WriteEntry(string message,
                     valuetype [System/*23000003*/]System.Diagnostics.TraceEventType/*01000038*/ severity,
                     int32 id) cil managed
  // SIG: 20 03 01 0E 11 80 E1 08
  {
    // Method begins at RVA 0x2454
    // Code size       0 (0x0)
  } // end of method Log::WriteEntry

  .method /*06000387*/ public instance void 
          WriteException(class [mscorlib/*23000001*/]System.Exception/*0100000D*/ ex) cil managed
  // SIG: 20 01 01 12 35
  {
    // Method begins at RVA 0x2455
    // Code size       0 (0x0)
  } // end of method Log::WriteException

  .method /*06000388*/ public instance void 
          WriteException(class [mscorlib/*23000001*/]System.Exception/*0100000D*/ ex,
                         valuetype [System/*23000003*/]System.Diagnostics.TraceEventType/*01000038*/ severity,
                         string additionalInfo) cil managed
  // SIG: 20 03 01 12 35 11 80 E1 0E
  {
    // Method begins at RVA 0x2456
    // Code size       0 (0x0)
  } // end of method Log::WriteException

  .method /*06000389*/ public instance void 
          WriteException(class [mscorlib/*23000001*/]System.Exception/*0100000D*/ ex,
                         valuetype [System/*23000003*/]System.Diagnostics.TraceEventType/*01000038*/ severity,
                         string additionalInfo,
                         int32 id) cil managed
  // SIG: 20 04 01 12 35 11 80 E1 0E 08
  {
    // Method begins at RVA 0x2457
    // Code size       0 (0x0)
  } // end of method Log::WriteException

  .method /*0600038A*/ public specialname 
          instance class [System/*23000003*/]System.Diagnostics.TraceSource/*01000039*/ 
          get_TraceSource() cil managed
  // SIG: 20 00 12 80 E5
  {
    // Method begins at RVA 0x2458
    // Code size       0 (0x0)
  } // end of method Log::get_TraceSource

  .method /*0600038B*/ public specialname 
          instance class Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/ 
          get_DefaultFileLogWriter() cil managed
  // SIG: 20 00 12 81 D4
  {
    // Method begins at RVA 0x2459
    // Code size       0 (0x0)
  } // end of method Log::get_DefaultFileLogWriter

  .method /*0600038C*/ famorassem newslot strict virtual 
          instance void  InitializeWithDefaultsSinceNoConfigExists() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x245a
    // Code size       0 (0x0)
  } // end of method Log::InitializeWithDefaultsSinceNoConfigExists

  .property /*17000095*/ instance callconv(8) class [System/*23000003*/]System.Diagnostics.TraceSource/*01000039*/
          TraceSource()
  {
    .custom /*0C000081:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get instance class [System/*23000003*/]System.Diagnostics.TraceSource/*01000039*/ Microsoft.VisualBasic.Logging.Log/*02000076*/::get_TraceSource() /* 0600038A */
  } // end of property Log::TraceSource
  .property /*17000096*/ instance callconv(8) class Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/
          DefaultFileLogWriter()
  {
    .get instance class Microsoft.VisualBasic.Logging.FileLogTraceListener/*02000075*/ Microsoft.VisualBasic.Logging.Log/*02000076*/::get_DefaultFileLogWriter() /* 0600038B */
  } // end of property Log::DefaultFileLogWriter
} // end of class Microsoft.VisualBasic.Logging.Log

.class /*02000077*/ public auto ansi Microsoft.VisualBasic.Logging.AspLog
       extends Microsoft.VisualBasic.Logging.Log/*02000076*/
{
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*0600038D*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x245b
    // Code size       0 (0x0)
  } // end of method AspLog::.ctor

  .method /*0600038E*/ public specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x245c
    // Code size       0 (0x0)
  } // end of method AspLog::.ctor

  .method /*0600038F*/ famorassem strict virtual 
          instance void  InitializeWithDefaultsSinceNoConfigExists() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x245d
    // Code size       0 (0x0)
  } // end of method AspLog::InitializeWithDefaultsSinceNoConfigExists

} // end of class Microsoft.VisualBasic.Logging.AspLog

.class /*02000078*/ public auto ansi sealed Microsoft.VisualBasic.VBMath
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00007A:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .method /*06000390*/ public static float32 
          Rnd() cil managed
  // SIG: 00 00 0C
  {
    // Method begins at RVA 0x245e
    // Code size       0 (0x0)
  } // end of method VBMath::Rnd

  .method /*06000391*/ public static float32 
          Rnd(float32 Number) cil managed
  // SIG: 00 01 0C 0C
  {
    // Method begins at RVA 0x245f
    // Code size       0 (0x0)
  } // end of method VBMath::Rnd

  .method /*06000392*/ public static void 
          Randomize() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2460
    // Code size       0 (0x0)
  } // end of method VBMath::Randomize

  .method /*06000393*/ public static void 
          Randomize(float64 Number) cil managed
  // SIG: 00 01 01 0D
  {
    // Method begins at RVA 0x2461
    // Code size       0 (0x0)
  } // end of method VBMath::Randomize

  .method /*06000394*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2462
    // Code size       0 (0x0)
  } // end of method VBMath::.ctor

} // end of class Microsoft.VisualBasic.VBMath

.class /*02000079*/ public auto ansi Microsoft.VisualBasic.MyServices.ClipboardProxy
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00007B:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*06000395*/ assembly specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2463
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::.ctor

  .method /*06000396*/ public instance string 
          GetText() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2464
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::GetText

  .method /*06000397*/ public instance string 
          GetText(valuetype [System.Windows.Forms/*23000004*/]System.Windows.Forms.TextDataFormat/*0100003A*/ format) cil managed
  // SIG: 20 01 0E 11 80 E9
  {
    // Method begins at RVA 0x2465
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::GetText

  .method /*06000398*/ public instance bool 
          ContainsText() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2466
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::ContainsText

  .method /*06000399*/ public instance bool 
          ContainsText(valuetype [System.Windows.Forms/*23000004*/]System.Windows.Forms.TextDataFormat/*0100003A*/ format) cil managed
  // SIG: 20 01 02 11 80 E9
  {
    // Method begins at RVA 0x2467
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::ContainsText

  .method /*0600039A*/ public instance void 
          SetText(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2468
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::SetText

  .method /*0600039B*/ public instance void 
          SetText(string text,
                  valuetype [System.Windows.Forms/*23000004*/]System.Windows.Forms.TextDataFormat/*0100003A*/ format) cil managed
  // SIG: 20 02 01 0E 11 80 E9
  {
    // Method begins at RVA 0x2469
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::SetText

  .method /*0600039C*/ public instance class [System.Drawing/*23000007*/]System.Drawing.Image/*0100003B*/ 
          GetImage() cil managed
  // SIG: 20 00 12 80 ED
  {
    // Method begins at RVA 0x246a
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::GetImage

  .method /*0600039D*/ public instance bool 
          ContainsImage() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x246b
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::ContainsImage

  .method /*0600039E*/ public instance void 
          SetImage(class [System.Drawing/*23000007*/]System.Drawing.Image/*0100003B*/ image) cil managed
  // SIG: 20 01 01 12 80 ED
  {
    // Method begins at RVA 0x246c
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::SetImage

  .method /*0600039F*/ public instance class [mscorlib/*23000001*/]System.IO.Stream/*01000021*/ 
          GetAudioStream() cil managed
  // SIG: 20 00 12 80 85
  {
    // Method begins at RVA 0x246d
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::GetAudioStream

  .method /*060003A0*/ public instance bool 
          ContainsAudio() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x246e
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::ContainsAudio

  .method /*060003A1*/ public instance void 
          SetAudio(uint8[] audioBytes) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x246f
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::SetAudio

  .method /*060003A2*/ public instance void 
          SetAudio(class [mscorlib/*23000001*/]System.IO.Stream/*01000021*/ audioStream) cil managed
  // SIG: 20 01 01 12 80 85
  {
    // Method begins at RVA 0x2470
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::SetAudio

  .method /*060003A3*/ public instance class [System/*23000003*/]System.Collections.Specialized.StringCollection/*0100003C*/ 
          GetFileDropList() cil managed
  // SIG: 20 00 12 80 F1
  {
    // Method begins at RVA 0x2471
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::GetFileDropList

  .method /*060003A4*/ public instance bool 
          ContainsFileDropList() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2472
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::ContainsFileDropList

  .method /*060003A5*/ public instance void 
          SetFileDropList(class [System/*23000003*/]System.Collections.Specialized.StringCollection/*0100003C*/ filePaths) cil managed
  // SIG: 20 01 01 12 80 F1
  {
    // Method begins at RVA 0x2473
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::SetFileDropList

  .method /*060003A6*/ public instance object 
          GetData(string format) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x2474
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::GetData

  .method /*060003A7*/ public instance bool 
          ContainsData(string format) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x2475
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::ContainsData

  .method /*060003A8*/ public instance void 
          SetData(string format,
                  object data) cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x2476
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::SetData

  .method /*060003A9*/ public instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2477
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::Clear

  .method /*060003AA*/ public instance class [System.Windows.Forms/*23000004*/]System.Windows.Forms.IDataObject/*0100003D*/ 
          GetDataObject() cil managed
  // SIG: 20 00 12 80 F5
  {
    .custom /*0C0000D0:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x2478
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::GetDataObject

  .method /*060003AB*/ public instance void 
          SetDataObject(class [System.Windows.Forms/*23000004*/]System.Windows.Forms.DataObject/*0100003E*/ data) cil managed
  // SIG: 20 01 01 12 80 F9
  {
    .custom /*0C0000D1:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    // Method begins at RVA 0x2479
    // Code size       0 (0x0)
  } // end of method ClipboardProxy::SetDataObject

} // end of class Microsoft.VisualBasic.MyServices.ClipboardProxy

.class /*0200007A*/ public auto ansi Microsoft.VisualBasic.MyServices.FileSystemProxy
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00007C:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*060003AC*/ public specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.IO.DriveInfo/*01000029*/> 
          get_Drives() cil managed
  // SIG: 20 00 15 12 15 01 12 80 A5
  {
    // Method begins at RVA 0x247a
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::get_Drives

  .method /*060003AD*/ public specialname 
          instance class Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/ 
          get_SpecialDirectories() cil managed
  // SIG: 20 00 12 81 F4
  {
    // Method begins at RVA 0x247b
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::get_SpecialDirectories

  .method /*060003AE*/ public specialname 
          instance string  get_CurrentDirectory() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x247c
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::get_CurrentDirectory

  .method /*060003AF*/ public specialname 
          instance void  set_CurrentDirectory(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x247d
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::set_CurrentDirectory

  .method /*060003B0*/ public instance bool 
          DirectoryExists(string directory) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x247e
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::DirectoryExists

  .method /*060003B1*/ public instance bool 
          FileExists(string file) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x247f
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::FileExists

  .method /*060003B2*/ public instance void 
          CreateDirectory(string directory) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2480
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CreateDirectory

  .method /*060003B3*/ public instance class [mscorlib/*23000001*/]System.IO.DirectoryInfo/*0100002A*/ 
          GetDirectoryInfo(string directory) cil managed
  // SIG: 20 01 12 80 A9 0E
  {
    // Method begins at RVA 0x2481
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetDirectoryInfo

  .method /*060003B4*/ public instance class [mscorlib/*23000001*/]System.IO.FileInfo/*0100002B*/ 
          GetFileInfo(string file) cil managed
  // SIG: 20 01 12 80 AD 0E
  {
    // Method begins at RVA 0x2482
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetFileInfo

  .method /*060003B5*/ public instance class [mscorlib/*23000001*/]System.IO.DriveInfo/*01000029*/ 
          GetDriveInfo(string drive) cil managed
  // SIG: 20 01 12 80 A5 0E
  {
    // Method begins at RVA 0x2483
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetDriveInfo

  .method /*060003B6*/ public instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          GetFiles(string directory) cil managed
  // SIG: 20 01 15 12 15 01 0E 0E
  {
    // Method begins at RVA 0x2484
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetFiles

  .method /*060003B7*/ public instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          GetFiles(string directory,
                   valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ searchType,
                   string[] wildcards) cil managed
  // SIG: 20 03 15 12 15 01 0E 0E 11 80 B0 1D 0E
  {
    .param [3]/*0800053C*/ 
    .custom /*0C0000DD:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2485
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetFiles

  .method /*060003B8*/ public instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          GetDirectories(string directory) cil managed
  // SIG: 20 01 15 12 15 01 0E 0E
  {
    // Method begins at RVA 0x2486
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetDirectories

  .method /*060003B9*/ public instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          GetDirectories(string directory,
                         valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ searchType,
                         string[] wildcards) cil managed
  // SIG: 20 03 15 12 15 01 0E 0E 11 80 B0 1D 0E
  {
    .param [3]/*08000540*/ 
    .custom /*0C0000DE:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2487
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetDirectories

  .method /*060003BA*/ public instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          FindInFiles(string directory,
                      string containsText,
                      bool ignoreCase,
                      valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ searchType) cil managed
  // SIG: 20 04 15 12 15 01 0E 0E 0E 02 11 80 B0
  {
    // Method begins at RVA 0x2488
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::FindInFiles

  .method /*060003BB*/ public instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<string> 
          FindInFiles(string directory,
                      string containsText,
                      bool ignoreCase,
                      valuetype Microsoft.VisualBasic.FileIO.SearchOption/*0200002C*/ searchType,
                      string[] fileWildcards) cil managed
  // SIG: 20 05 15 12 15 01 0E 0E 0E 02 11 80 B0 1D 0E
  {
    .param [5]/*08000549*/ 
    .custom /*0C0000DF:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2489
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::FindInFiles

  .method /*060003BC*/ public instance string 
          GetParentPath(string path) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x248a
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetParentPath

  .method /*060003BD*/ public instance string 
          CombinePath(string baseDirectory,
                      string relativePath) cil managed
  // SIG: 20 02 0E 0E 0E
  {
    // Method begins at RVA 0x248b
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CombinePath

  .method /*060003BE*/ public instance string 
          GetName(string path) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x248c
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetName

  .method /*060003BF*/ public instance string 
          GetTempFileName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x248d
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::GetTempFileName

  .method /*060003C0*/ public instance string 
          ReadAllText(string file) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x248e
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::ReadAllText

  .method /*060003C1*/ public instance string 
          ReadAllText(string file,
                      class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ encoding) cil managed
  // SIG: 20 02 0E 0E 12 80 B5
  {
    // Method begins at RVA 0x248f
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::ReadAllText

  .method /*060003C2*/ public instance uint8[] 
          ReadAllBytes(string file) cil managed
  // SIG: 20 01 1D 05 0E
  {
    // Method begins at RVA 0x2490
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::ReadAllBytes

  .method /*060003C3*/ public instance void 
          WriteAllText(string file,
                       string text,
                       bool append) cil managed
  // SIG: 20 03 01 0E 0E 02
  {
    // Method begins at RVA 0x2491
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::WriteAllText

  .method /*060003C4*/ public instance void 
          WriteAllText(string file,
                       string text,
                       bool append,
                       class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ encoding) cil managed
  // SIG: 20 04 01 0E 0E 02 12 80 B5
  {
    // Method begins at RVA 0x2492
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::WriteAllText

  .method /*060003C5*/ public instance void 
          WriteAllBytes(string file,
                        uint8[] data,
                        bool append) cil managed
  // SIG: 20 03 01 0E 1D 05 02
  {
    // Method begins at RVA 0x2493
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::WriteAllBytes

  .method /*060003C6*/ public instance void 
          CopyFile(string sourceFileName,
                   string destinationFileName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2494
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CopyFile

  .method /*060003C7*/ public instance void 
          CopyFile(string sourceFileName,
                   string destinationFileName,
                   bool overwrite) cil managed
  // SIG: 20 03 01 0E 0E 02
  {
    // Method begins at RVA 0x2495
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CopyFile

  .method /*060003C8*/ public instance void 
          CopyFile(string sourceFileName,
                   string destinationFileName,
                   valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI) cil managed
  // SIG: 20 03 01 0E 0E 11 80 B8
  {
    // Method begins at RVA 0x2496
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CopyFile

  .method /*060003C9*/ public instance void 
          CopyFile(string sourceFileName,
                   string destinationFileName,
                   valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                   valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 04 01 0E 0E 11 80 B8 11 80 B4
  {
    // Method begins at RVA 0x2497
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CopyFile

  .method /*060003CA*/ public instance void 
          MoveFile(string sourceFileName,
                   string destinationFileName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2498
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::MoveFile

  .method /*060003CB*/ public instance void 
          MoveFile(string sourceFileName,
                   string destinationFileName,
                   bool overwrite) cil managed
  // SIG: 20 03 01 0E 0E 02
  {
    // Method begins at RVA 0x2499
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::MoveFile

  .method /*060003CC*/ public instance void 
          MoveFile(string sourceFileName,
                   string destinationFileName,
                   valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI) cil managed
  // SIG: 20 03 01 0E 0E 11 80 B8
  {
    // Method begins at RVA 0x249a
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::MoveFile

  .method /*060003CD*/ public instance void 
          MoveFile(string sourceFileName,
                   string destinationFileName,
                   valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                   valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 04 01 0E 0E 11 80 B8 11 80 B4
  {
    // Method begins at RVA 0x249b
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::MoveFile

  .method /*060003CE*/ public instance void 
          CopyDirectory(string sourceDirectoryName,
                        string destinationDirectoryName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x249c
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CopyDirectory

  .method /*060003CF*/ public instance void 
          CopyDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        bool overwrite) cil managed
  // SIG: 20 03 01 0E 0E 02
  {
    // Method begins at RVA 0x249d
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CopyDirectory

  .method /*060003D0*/ public instance void 
          CopyDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI) cil managed
  // SIG: 20 03 01 0E 0E 11 80 B8
  {
    // Method begins at RVA 0x249e
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CopyDirectory

  .method /*060003D1*/ public instance void 
          CopyDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                        valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 04 01 0E 0E 11 80 B8 11 80 B4
  {
    // Method begins at RVA 0x249f
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::CopyDirectory

  .method /*060003D2*/ public instance void 
          MoveDirectory(string sourceDirectoryName,
                        string destinationDirectoryName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x24a0
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::MoveDirectory

  .method /*060003D3*/ public instance void 
          MoveDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        bool overwrite) cil managed
  // SIG: 20 03 01 0E 0E 02
  {
    // Method begins at RVA 0x24a1
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::MoveDirectory

  .method /*060003D4*/ public instance void 
          MoveDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI) cil managed
  // SIG: 20 03 01 0E 0E 11 80 B8
  {
    // Method begins at RVA 0x24a2
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::MoveDirectory

  .method /*060003D5*/ public instance void 
          MoveDirectory(string sourceDirectoryName,
                        string destinationDirectoryName,
                        valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                        valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 04 01 0E 0E 11 80 B8 11 80 B4
  {
    // Method begins at RVA 0x24a3
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::MoveDirectory

  .method /*060003D6*/ public instance void 
          DeleteFile(string file) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x24a4
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::DeleteFile

  .method /*060003D7*/ public instance void 
          DeleteFile(string file,
                     valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                     valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ recycle) cil managed
  // SIG: 20 03 01 0E 11 80 B8 11 80 AC
  {
    // Method begins at RVA 0x24a5
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::DeleteFile

  .method /*060003D8*/ public instance void 
          DeleteFile(string file,
                     valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                     valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ recycle,
                     valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 04 01 0E 11 80 B8 11 80 AC 11 80 B4
  {
    // Method begins at RVA 0x24a6
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::DeleteFile

  .method /*060003D9*/ public instance void 
          DeleteDirectory(string directory,
                          valuetype Microsoft.VisualBasic.FileIO.DeleteDirectoryOption/*0200002A*/ onDirectoryNotEmpty) cil managed
  // SIG: 20 02 01 0E 11 80 A8
  {
    // Method begins at RVA 0x24a7
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::DeleteDirectory

  .method /*060003DA*/ public instance void 
          DeleteDirectory(string directory,
                          valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                          valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ recycle) cil managed
  // SIG: 20 03 01 0E 11 80 B8 11 80 AC
  {
    // Method begins at RVA 0x24a8
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::DeleteDirectory

  .method /*060003DB*/ public instance void 
          DeleteDirectory(string directory,
                          valuetype Microsoft.VisualBasic.FileIO.UIOption/*0200002E*/ showUI,
                          valuetype Microsoft.VisualBasic.FileIO.RecycleOption/*0200002B*/ recycle,
                          valuetype Microsoft.VisualBasic.FileIO.UICancelOption/*0200002D*/ onUserCancel) cil managed
  // SIG: 20 04 01 0E 11 80 B8 11 80 AC 11 80 B4
  {
    // Method begins at RVA 0x24a9
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::DeleteDirectory

  .method /*060003DC*/ public instance void 
          RenameFile(string file,
                     string newName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x24aa
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::RenameFile

  .method /*060003DD*/ public instance void 
          RenameDirectory(string directory,
                          string newName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x24ab
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::RenameDirectory

  .method /*060003DE*/ public instance class Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/ 
          OpenTextFieldParser(string file) cil managed
  // SIG: 20 01 12 80 C4 0E
  {
    // Method begins at RVA 0x24ac
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::OpenTextFieldParser

  .method /*060003DF*/ public instance class Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/ 
          OpenTextFieldParser(string file,
                              string[] delimiters) cil managed
  // SIG: 20 02 12 80 C4 0E 1D 0E
  {
    .param [2]/*080005A3*/ 
    .custom /*0C0000E0:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24ad
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::OpenTextFieldParser

  .method /*060003E0*/ public instance class Microsoft.VisualBasic.FileIO.TextFieldParser/*02000031*/ 
          OpenTextFieldParser(string file,
                              int32[] fieldWidths) cil managed
  // SIG: 20 02 12 80 C4 0E 1D 08
  {
    .param [2]/*080005A5*/ 
    .custom /*0C0000E1:0A00002D*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*0100005E*/::.ctor() /* 0A00002D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24ae
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::OpenTextFieldParser

  .method /*060003E1*/ public instance class [mscorlib/*23000001*/]System.IO.StreamReader/*0100002C*/ 
          OpenTextFileReader(string file) cil managed
  // SIG: 20 01 12 80 B1 0E
  {
    // Method begins at RVA 0x24af
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::OpenTextFileReader

  .method /*060003E2*/ public instance class [mscorlib/*23000001*/]System.IO.StreamReader/*0100002C*/ 
          OpenTextFileReader(string file,
                             class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ encoding) cil managed
  // SIG: 20 02 12 80 B1 0E 12 80 B5
  {
    // Method begins at RVA 0x24b0
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::OpenTextFileReader

  .method /*060003E3*/ public instance class [mscorlib/*23000001*/]System.IO.StreamWriter/*0100002E*/ 
          OpenTextFileWriter(string file,
                             bool append) cil managed
  // SIG: 20 02 12 80 B9 0E 02
  {
    // Method begins at RVA 0x24b1
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::OpenTextFileWriter

  .method /*060003E4*/ public instance class [mscorlib/*23000001*/]System.IO.StreamWriter/*0100002E*/ 
          OpenTextFileWriter(string file,
                             bool append,
                             class [mscorlib/*23000001*/]System.Text.Encoding/*0100002D*/ encoding) cil managed
  // SIG: 20 03 12 80 B9 0E 02 12 80 B5
  {
    // Method begins at RVA 0x24b2
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::OpenTextFileWriter

  .method /*060003E5*/ assembly specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24b3
    // Code size       0 (0x0)
  } // end of method FileSystemProxy::.ctor

  .property /*17000097*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.IO.DriveInfo/*01000029*/>
          Drives()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000005*/<class [mscorlib/*23000001*/]System.IO.DriveInfo/*01000029*/> Microsoft.VisualBasic.MyServices.FileSystemProxy/*0200007A*/::get_Drives() /* 060003AC */
  } // end of property FileSystemProxy::Drives
  .property /*17000098*/ instance callconv(8) class Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/
          SpecialDirectories()
  {
    .get instance class Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/ Microsoft.VisualBasic.MyServices.FileSystemProxy/*0200007A*/::get_SpecialDirectories() /* 060003AD */
  } // end of property FileSystemProxy::SpecialDirectories
  .property /*17000099*/ instance callconv(8) string
          CurrentDirectory()
  {
    .get instance string Microsoft.VisualBasic.MyServices.FileSystemProxy/*0200007A*/::get_CurrentDirectory() /* 060003AE */
    .set instance void Microsoft.VisualBasic.MyServices.FileSystemProxy/*0200007A*/::set_CurrentDirectory(string) /* 060003AF */
  } // end of property FileSystemProxy::CurrentDirectory
} // end of class Microsoft.VisualBasic.MyServices.FileSystemProxy

.class /*0200007B*/ public auto ansi Microsoft.VisualBasic.MyServices.Internal.ContextValue`1<T>
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00007D:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060003E6*/ public specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24b4
    // Code size       0 (0x0)
  } // end of method ContextValue`1::.ctor

  .method /*060003E7*/ public specialname 
          instance !T  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x24b5
    // Code size       0 (0x0)
  } // end of method ContextValue`1::get_Value

  .method /*060003E8*/ public specialname 
          instance void  set_Value(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x24b6
    // Code size       0 (0x0)
  } // end of method ContextValue`1::set_Value

  .property /*1700009A*/ instance callconv(8) !T
          Value()
  {
    .get instance !T Microsoft.VisualBasic.MyServices.Internal.ContextValue`1/*0200007B*/::get_Value() /* 060003E7 */
    .set instance void Microsoft.VisualBasic.MyServices.Internal.ContextValue`1/*0200007B*/::set_Value(!T) /* 060003E8 */
  } // end of property ContextValue`1::Value
} // end of class Microsoft.VisualBasic.MyServices.Internal.ContextValue`1

.class /*0200007C*/ public auto ansi Microsoft.VisualBasic.MyServices.RegistryProxy
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00007E:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*060003E9*/ public specialname 
          instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ 
          get_CurrentUser() cil managed
  // SIG: 20 00 12 80 FD
  {
    // Method begins at RVA 0x24b7
    // Code size       0 (0x0)
  } // end of method RegistryProxy::get_CurrentUser

  .method /*060003EA*/ public specialname 
          instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ 
          get_LocalMachine() cil managed
  // SIG: 20 00 12 80 FD
  {
    // Method begins at RVA 0x24b8
    // Code size       0 (0x0)
  } // end of method RegistryProxy::get_LocalMachine

  .method /*060003EB*/ public specialname 
          instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ 
          get_ClassesRoot() cil managed
  // SIG: 20 00 12 80 FD
  {
    // Method begins at RVA 0x24b9
    // Code size       0 (0x0)
  } // end of method RegistryProxy::get_ClassesRoot

  .method /*060003EC*/ public specialname 
          instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ 
          get_Users() cil managed
  // SIG: 20 00 12 80 FD
  {
    // Method begins at RVA 0x24ba
    // Code size       0 (0x0)
  } // end of method RegistryProxy::get_Users

  .method /*060003ED*/ public specialname 
          instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ 
          get_PerformanceData() cil managed
  // SIG: 20 00 12 80 FD
  {
    // Method begins at RVA 0x24bb
    // Code size       0 (0x0)
  } // end of method RegistryProxy::get_PerformanceData

  .method /*060003EE*/ public specialname 
          instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ 
          get_CurrentConfig() cil managed
  // SIG: 20 00 12 80 FD
  {
    // Method begins at RVA 0x24bc
    // Code size       0 (0x0)
  } // end of method RegistryProxy::get_CurrentConfig

  .method /*060003EF*/ public specialname 
          instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ 
          get_DynData() cil managed
  // SIG: 20 00 12 80 FD
  {
    // Method begins at RVA 0x24bd
    // Code size       0 (0x0)
  } // end of method RegistryProxy::get_DynData

  .method /*060003F0*/ public instance object 
          GetValue(string keyName,
                   string valueName,
                   object defaultValue) cil managed
  // SIG: 20 03 1C 0E 0E 1C
  {
    // Method begins at RVA 0x24be
    // Code size       0 (0x0)
  } // end of method RegistryProxy::GetValue

  .method /*060003F1*/ public instance void 
          SetValue(string keyName,
                   string valueName,
                   object 'value') cil managed
  // SIG: 20 03 01 0E 0E 1C
  {
    // Method begins at RVA 0x24bf
    // Code size       0 (0x0)
  } // end of method RegistryProxy::SetValue

  .method /*060003F2*/ public instance void 
          SetValue(string keyName,
                   string valueName,
                   object 'value',
                   valuetype [mscorlib/*23000001*/]Microsoft.Win32.RegistryValueKind/*01000040*/ valueKind) cil managed
  // SIG: 20 04 01 0E 0E 1C 11 81 01
  {
    // Method begins at RVA 0x24c0
    // Code size       0 (0x0)
  } // end of method RegistryProxy::SetValue

  .method /*060003F3*/ assembly specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24c1
    // Code size       0 (0x0)
  } // end of method RegistryProxy::.ctor

  .property /*1700009B*/ instance callconv(8) class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/
          CurrentUser()
  {
    .get instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/::get_CurrentUser() /* 060003E9 */
  } // end of property RegistryProxy::CurrentUser
  .property /*1700009C*/ instance callconv(8) class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/
          LocalMachine()
  {
    .get instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/::get_LocalMachine() /* 060003EA */
  } // end of property RegistryProxy::LocalMachine
  .property /*1700009D*/ instance callconv(8) class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/
          ClassesRoot()
  {
    .get instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/::get_ClassesRoot() /* 060003EB */
  } // end of property RegistryProxy::ClassesRoot
  .property /*1700009E*/ instance callconv(8) class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/
          Users()
  {
    .get instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/::get_Users() /* 060003EC */
  } // end of property RegistryProxy::Users
  .property /*1700009F*/ instance callconv(8) class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/
          PerformanceData()
  {
    .get instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/::get_PerformanceData() /* 060003ED */
  } // end of property RegistryProxy::PerformanceData
  .property /*170000A0*/ instance callconv(8) class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/
          CurrentConfig()
  {
    .get instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/::get_CurrentConfig() /* 060003EE */
  } // end of property RegistryProxy::CurrentConfig
  .property /*170000A1*/ instance callconv(8) class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/
          DynData()
  {
    .get instance class [mscorlib/*23000001*/]Microsoft.Win32.RegistryKey/*0100003F*/ Microsoft.VisualBasic.MyServices.RegistryProxy/*0200007C*/::get_DynData() /* 060003EF */
  } // end of property RegistryProxy::DynData
} // end of class Microsoft.VisualBasic.MyServices.RegistryProxy

.class /*0200007D*/ public auto ansi Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00007F:0A000026*/ instance void [System/*23000003*/]System.ComponentModel.EditorBrowsableAttribute/*01000054*/::.ctor(valuetype [System/*23000003*/]System.ComponentModel.EditorBrowsableState/*01000055*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .permissionset linkcheck
             = {class 'System.Security.Permissions.HostProtectionAttribute, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property enum class 'System.Security.Permissions.HostProtectionResource, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' 'Resources' = int32(4)}}
  .method /*060003F4*/ public specialname 
          instance string  get_MyDocuments() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c2
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_MyDocuments

  .method /*060003F5*/ public specialname 
          instance string  get_MyMusic() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c3
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_MyMusic

  .method /*060003F6*/ public specialname 
          instance string  get_MyPictures() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c4
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_MyPictures

  .method /*060003F7*/ public specialname 
          instance string  get_Desktop() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c5
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_Desktop

  .method /*060003F8*/ public specialname 
          instance string  get_Programs() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c6
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_Programs

  .method /*060003F9*/ public specialname 
          instance string  get_ProgramFiles() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c7
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_ProgramFiles

  .method /*060003FA*/ public specialname 
          instance string  get_Temp() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c8
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_Temp

  .method /*060003FB*/ public specialname 
          instance string  get_CurrentUserApplicationData() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c9
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_CurrentUserApplicationData

  .method /*060003FC*/ public specialname 
          instance string  get_AllUsersApplicationData() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24ca
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::get_AllUsersApplicationData

  .method /*060003FD*/ assembly specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24cb
    // Code size       0 (0x0)
  } // end of method SpecialDirectoriesProxy::.ctor

  .property /*170000A2*/ instance callconv(8) string
          MyDocuments()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_MyDocuments() /* 060003F4 */
  } // end of property SpecialDirectoriesProxy::MyDocuments
  .property /*170000A3*/ instance callconv(8) string
          MyMusic()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_MyMusic() /* 060003F5 */
  } // end of property SpecialDirectoriesProxy::MyMusic
  .property /*170000A4*/ instance callconv(8) string
          MyPictures()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_MyPictures() /* 060003F6 */
  } // end of property SpecialDirectoriesProxy::MyPictures
  .property /*170000A5*/ instance callconv(8) string
          Desktop()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_Desktop() /* 060003F7 */
  } // end of property SpecialDirectoriesProxy::Desktop
  .property /*170000A6*/ instance callconv(8) string
          Programs()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_Programs() /* 060003F8 */
  } // end of property SpecialDirectoriesProxy::Programs
  .property /*170000A7*/ instance callconv(8) string
          ProgramFiles()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_ProgramFiles() /* 060003F9 */
  } // end of property SpecialDirectoriesProxy::ProgramFiles
  .property /*170000A8*/ instance callconv(8) string
          Temp()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_Temp() /* 060003FA */
  } // end of property SpecialDirectoriesProxy::Temp
  .property /*170000A9*/ instance callconv(8) string
          CurrentUserApplicationData()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_CurrentUserApplicationData() /* 060003FB */
  } // end of property SpecialDirectoriesProxy::CurrentUserApplicationData
  .property /*170000AA*/ instance callconv(8) string
          AllUsersApplicationData()
  {
    .get instance string Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy/*0200007D*/::get_AllUsersApplicationData() /* 060003FC */
  } // end of property SpecialDirectoriesProxy::AllUsersApplicationData
} // end of class Microsoft.VisualBasic.MyServices.SpecialDirectoriesProxy

.class /*0200007E*/ public auto ansi sealed beforefieldinit Microsoft.VisualBasic.Strings
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000080:06000237*/ instance void Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute/*0200004E*/::.ctor() /* 06000237 */ = ( 01 00 00 00 ) 
  .method /*060003FE*/ private specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x24cc
    // Code size       0 (0x0)
  } // end of method Strings::.cctor

  .method /*060003FF*/ public static int32 
          Asc(char String) cil managed
  // SIG: 00 01 08 03
  {
    // Method begins at RVA 0x24cd
    // Code size       0 (0x0)
  } // end of method Strings::Asc

  .method /*06000400*/ public static int32 
          Asc(string String) cil managed
  // SIG: 00 01 08 0E
  {
    // Method begins at RVA 0x24ce
    // Code size       0 (0x0)
  } // end of method Strings::Asc

  .method /*06000401*/ public static int32 
          AscW(string String) cil managed
  // SIG: 00 01 08 0E
  {
    // Method begins at RVA 0x24cf
    // Code size       0 (0x0)
  } // end of method Strings::AscW

  .method /*06000402*/ public static int32 
          AscW(char String) cil managed
  // SIG: 00 01 08 03
  {
    // Method begins at RVA 0x24d0
    // Code size       0 (0x0)
  } // end of method Strings::AscW

  .method /*06000403*/ public static char 
          Chr(int32 CharCode) cil managed
  // SIG: 00 01 03 08
  {
    // Method begins at RVA 0x24d1
    // Code size       0 (0x0)
  } // end of method Strings::Chr

  .method /*06000404*/ public static char 
          ChrW(int32 CharCode) cil managed
  // SIG: 00 01 03 08
  {
    // Method begins at RVA 0x24d2
    // Code size       0 (0x0)
  } // end of method Strings::ChrW

  .method /*06000405*/ public static string[] 
          Filter(object[] Source,
                 string Match,
                 [opt] bool Include,
                 [opt] valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Compare) cil managed
  // SIG: 00 04 1D 0E 1D 1C 0E 02 11 80 E0
  {
    .param [3]/*080005C1*/  = bool(true)
    .param [4]/*080005C2*/  = int32(0x00000000)
    .custom /*0C0000E2:06000239*/ instance void Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute/*02000050*/::.ctor() /* 06000239 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24d3
    // Code size       0 (0x0)
  } // end of method Strings::Filter

  .method /*06000406*/ public static string[] 
          Filter(string[] Source,
                 string Match,
                 [opt] bool Include,
                 [opt] valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Compare) cil managed
  // SIG: 00 04 1D 0E 1D 0E 0E 02 11 80 E0
  {
    .param [3]/*080005C5*/  = bool(true)
    .param [4]/*080005C6*/  = int32(0x00000000)
    .custom /*0C0000E3:06000239*/ instance void Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute/*02000050*/::.ctor() /* 06000239 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24d4
    // Code size       0 (0x0)
  } // end of method Strings::Filter

  .method /*06000407*/ public static int32 
          InStr(string String1,
                string String2,
                [opt] valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Compare) cil managed
  // SIG: 00 03 08 0E 0E 11 80 E0
  {
    .param [3]/*080005C9*/  = int32(0x00000000)
    .custom /*0C0000E4:06000239*/ instance void Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute/*02000050*/::.ctor() /* 06000239 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24d5
    // Code size       0 (0x0)
  } // end of method Strings::InStr

  .method /*06000408*/ public static int32 
          InStr(int32 Start,
                string String1,
                string String2,
                [opt] valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Compare) cil managed
  // SIG: 00 04 08 08 0E 0E 11 80 E0
  {
    .param [4]/*080005CD*/  = int32(0x00000000)
    .custom /*0C0000E5:06000239*/ instance void Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute/*02000050*/::.ctor() /* 06000239 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24d6
    // Code size       0 (0x0)
  } // end of method Strings::InStr

  .method /*06000409*/ public static int32 
          InStrRev(string StringCheck,
                   string StringMatch,
                   [opt] int32 Start,
                   [opt] valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Compare) cil managed
  // SIG: 00 04 08 0E 0E 08 11 80 E0
  {
    .param [3]/*080005D0*/  = int32(0xFFFFFFFF)
    .param [4]/*080005D1*/  = int32(0x00000000)
    .custom /*0C0000E6:06000239*/ instance void Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute/*02000050*/::.ctor() /* 06000239 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24d7
    // Code size       0 (0x0)
  } // end of method Strings::InStrRev

  .method /*0600040A*/ public static string 
          Join(object[] SourceArray,
               [opt] string Delimiter) cil managed
  // SIG: 00 02 0E 1D 1C 0E
  {
    .param [2]/*080005D3*/  = " "
    // Method begins at RVA 0x24d8
    // Code size       0 (0x0)
  } // end of method Strings::Join

  .method /*0600040B*/ public static string 
          Join(string[] SourceArray,
               [opt] string Delimiter) cil managed
  // SIG: 00 02 0E 1D 0E 0E
  {
    .param [2]/*080005D5*/  = " "
    // Method begins at RVA 0x24d9
    // Code size       0 (0x0)
  } // end of method Strings::Join

  .method /*0600040C*/ public static string 
          LCase(string Value) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x24da
    // Code size       0 (0x0)
  } // end of method Strings::LCase

  .method /*0600040D*/ public static char 
          LCase(char Value) cil managed
  // SIG: 00 01 03 03
  {
    // Method begins at RVA 0x24db
    // Code size       0 (0x0)
  } // end of method Strings::LCase

  .method /*0600040E*/ public static int32 
          Len(bool Expression) cil managed
  // SIG: 00 01 08 02
  {
    // Method begins at RVA 0x24dc
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*0600040F*/ public static int32 
          Len(int8 Expression) cil managed
  // SIG: 00 01 08 04
  {
    .custom /*0C0000D2:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24dd
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000410*/ public static int32 
          Len(uint8 Expression) cil managed
  // SIG: 00 01 08 05
  {
    // Method begins at RVA 0x24de
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000411*/ public static int32 
          Len(int16 Expression) cil managed
  // SIG: 00 01 08 06
  {
    // Method begins at RVA 0x24df
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000412*/ public static int32 
          Len(uint16 Expression) cil managed
  // SIG: 00 01 08 07
  {
    .custom /*0C0000D3:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24e0
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000413*/ public static int32 
          Len(int32 Expression) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x24e1
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000414*/ public static int32 
          Len(uint32 Expression) cil managed
  // SIG: 00 01 08 09
  {
    .custom /*0C0000D4:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24e2
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000415*/ public static int32 
          Len(int64 Expression) cil managed
  // SIG: 00 01 08 0A
  {
    // Method begins at RVA 0x24e3
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000416*/ public static int32 
          Len(uint64 Expression) cil managed
  // SIG: 00 01 08 0B
  {
    .custom /*0C0000D5:0A000016*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000043*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24e4
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000417*/ public static int32 
          Len(valuetype [mscorlib/*23000001*/]System.Decimal/*0100001F*/ Expression) cil managed
  // SIG: 00 01 08 11 7D
  {
    // Method begins at RVA 0x24e5
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000418*/ public static int32 
          Len(float32 Expression) cil managed
  // SIG: 00 01 08 0C
  {
    // Method begins at RVA 0x24e6
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*06000419*/ public static int32 
          Len(float64 Expression) cil managed
  // SIG: 00 01 08 0D
  {
    // Method begins at RVA 0x24e7
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*0600041A*/ public static int32 
          Len(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Expression) cil managed
  // SIG: 00 01 08 11 80 81
  {
    // Method begins at RVA 0x24e8
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*0600041B*/ public static int32 
          Len(char Expression) cil managed
  // SIG: 00 01 08 03
  {
    // Method begins at RVA 0x24e9
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*0600041C*/ public static int32 
          Len(string Expression) cil managed
  // SIG: 00 01 08 0E
  {
    // Method begins at RVA 0x24ea
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*0600041D*/ public static int32 
          Len(object Expression) cil managed
  // SIG: 00 01 08 1C
  {
    // Method begins at RVA 0x24eb
    // Code size       0 (0x0)
  } // end of method Strings::Len

  .method /*0600041E*/ public static string 
          Replace(string Expression,
                  string Find,
                  string Replacement,
                  [opt] int32 Start,
                  [opt] int32 Count,
                  [opt] valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Compare) cil managed
  // SIG: 00 06 0E 0E 0E 0E 08 08 11 80 E0
  {
    .param [4]/*080005EB*/  = int32(0x00000001)
    .param [5]/*080005EC*/  = int32(0xFFFFFFFF)
    .param [6]/*080005ED*/  = int32(0x00000000)
    .custom /*0C0000E7:06000239*/ instance void Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute/*02000050*/::.ctor() /* 06000239 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24ec
    // Code size       0 (0x0)
  } // end of method Strings::Replace

  .method /*0600041F*/ public static string 
          Space(int32 Number) cil managed
  // SIG: 00 01 0E 08
  {
    // Method begins at RVA 0x24ed
    // Code size       0 (0x0)
  } // end of method Strings::Space

  .method /*06000420*/ public static string[] 
          Split(string Expression,
                [opt] string Delimiter,
                [opt] int32 Limit,
                [opt] valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Compare) cil managed
  // SIG: 00 04 1D 0E 0E 0E 08 11 80 E0
  {
    .param [2]/*080005F0*/  = " "
    .param [3]/*080005F1*/  = int32(0xFFFFFFFF)
    .param [4]/*080005F2*/  = int32(0x00000000)
    .custom /*0C0000E8:06000239*/ instance void Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute/*02000050*/::.ctor() /* 06000239 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24ee
    // Code size       0 (0x0)
  } // end of method Strings::Split

  .method /*06000421*/ public static string 
          LSet(string Source,
               int32 Length) cil managed
  // SIG: 00 02 0E 0E 08
  {
    // Method begins at RVA 0x24ef
    // Code size       0 (0x0)
  } // end of method Strings::LSet

  .method /*06000422*/ public static string 
          RSet(string Source,
               int32 Length) cil managed
  // SIG: 00 02 0E 0E 08
  {
    // Method begins at RVA 0x24f0
    // Code size       0 (0x0)
  } // end of method Strings::RSet

  .method /*06000423*/ public static object 
          StrDup(int32 Number,
                 object Character) cil managed
  // SIG: 00 02 1C 08 1C
  {
    // Method begins at RVA 0x24f1
    // Code size       0 (0x0)
  } // end of method Strings::StrDup

  .method /*06000424*/ public static string 
          StrDup(int32 Number,
                 char Character) cil managed
  // SIG: 00 02 0E 08 03
  {
    // Method begins at RVA 0x24f2
    // Code size       0 (0x0)
  } // end of method Strings::StrDup

  .method /*06000425*/ public static string 
          StrDup(int32 Number,
                 string Character) cil managed
  // SIG: 00 02 0E 08 0E
  {
    // Method begins at RVA 0x24f3
    // Code size       0 (0x0)
  } // end of method Strings::StrDup

  .method /*06000426*/ public static string 
          StrReverse(string Expression) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x24f4
    // Code size       0 (0x0)
  } // end of method Strings::StrReverse

  .method /*06000427*/ public static string 
          UCase(string Value) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x24f5
    // Code size       0 (0x0)
  } // end of method Strings::UCase

  .method /*06000428*/ public static char 
          UCase(char Value) cil managed
  // SIG: 00 01 03 03
  {
    // Method begins at RVA 0x24f6
    // Code size       0 (0x0)
  } // end of method Strings::UCase

  .method /*06000429*/ public static string 
          Format(object Expression,
                 [opt] string Style) cil managed
  // SIG: 00 02 0E 1C 0E
  {
    .param [2]/*08000601*/  = ""
    // Method begins at RVA 0x24f7
    // Code size       0 (0x0)
  } // end of method Strings::Format

  .method /*0600042A*/ public static string 
          FormatCurrency(object Expression,
                         [opt] int32 NumDigitsAfterDecimal,
                         [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ IncludeLeadingDigit,
                         [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ UseParensForNegativeNumbers,
                         [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ GroupDigits) cil managed
  // SIG: 00 05 0E 1C 08 11 80 F8 11 80 F8 11 80 F8
  {
    .param [2]/*08000603*/  = int32(0xFFFFFFFF)
    .param [3]/*08000604*/  = int32(0xFFFFFFFE)
    .param [4]/*08000605*/  = int32(0xFFFFFFFE)
    .param [5]/*08000606*/  = int32(0xFFFFFFFE)
    // Method begins at RVA 0x24f8
    // Code size       0 (0x0)
  } // end of method Strings::FormatCurrency

  .method /*0600042B*/ public static string 
          FormatDateTime(valuetype [mscorlib/*23000001*/]System.DateTime/*01000020*/ Expression,
                         [opt] valuetype Microsoft.VisualBasic.DateFormat/*02000039*/ NamedFormat) cil managed
  // SIG: 00 02 0E 11 80 81 11 80 E4
  {
    .param [2]/*08000608*/  = int32(0x00000000)
    // Method begins at RVA 0x24f9
    // Code size       0 (0x0)
  } // end of method Strings::FormatDateTime

  .method /*0600042C*/ public static string 
          FormatNumber(object Expression,
                       [opt] int32 NumDigitsAfterDecimal,
                       [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ IncludeLeadingDigit,
                       [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ UseParensForNegativeNumbers,
                       [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ GroupDigits) cil managed
  // SIG: 00 05 0E 1C 08 11 80 F8 11 80 F8 11 80 F8
  {
    .param [2]/*0800060A*/  = int32(0xFFFFFFFF)
    .param [3]/*0800060B*/  = int32(0xFFFFFFFE)
    .param [4]/*0800060C*/  = int32(0xFFFFFFFE)
    .param [5]/*0800060D*/  = int32(0xFFFFFFFE)
    // Method begins at RVA 0x24fa
    // Code size       0 (0x0)
  } // end of method Strings::FormatNumber

  .method /*0600042D*/ public static string 
          FormatPercent(object Expression,
                        [opt] int32 NumDigitsAfterDecimal,
                        [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ IncludeLeadingDigit,
                        [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ UseParensForNegativeNumbers,
                        [opt] valuetype Microsoft.VisualBasic.TriState/*0200003E*/ GroupDigits) cil managed
  // SIG: 00 05 0E 1C 08 11 80 F8 11 80 F8 11 80 F8
  {
    .param [2]/*0800060F*/  = int32(0xFFFFFFFF)
    .param [3]/*08000610*/  = int32(0xFFFFFFFE)
    .param [4]/*08000611*/  = int32(0xFFFFFFFE)
    .param [5]/*08000612*/  = int32(0xFFFFFFFE)
    // Method begins at RVA 0x24fb
    // Code size       0 (0x0)
  } // end of method Strings::FormatPercent

  .method /*0600042E*/ public static char 
          GetChar(string str,
                  int32 Index) cil managed
  // SIG: 00 02 03 0E 08
  {
    // Method begins at RVA 0x24fc
    // Code size       0 (0x0)
  } // end of method Strings::GetChar

  .method /*0600042F*/ public static string 
          Left(string str,
               int32 Length) cil managed
  // SIG: 00 02 0E 0E 08
  {
    // Method begins at RVA 0x24fd
    // Code size       0 (0x0)
  } // end of method Strings::Left

  .method /*06000430*/ public static string 
          LTrim(string str) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x24fe
    // Code size       0 (0x0)
  } // end of method Strings::LTrim

  .method /*06000431*/ public static string 
          Mid(string str,
              int32 Start) cil managed
  // SIG: 00 02 0E 0E 08
  {
    // Method begins at RVA 0x24ff
    // Code size       0 (0x0)
  } // end of method Strings::Mid

  .method /*06000432*/ public static string 
          Mid(string str,
              int32 Start,
              int32 Length) cil managed
  // SIG: 00 03 0E 0E 08 08
  {
    // Method begins at RVA 0x2500
    // Code size       0 (0x0)
  } // end of method Strings::Mid

  .method /*06000433*/ public static string 
          Right(string str,
                int32 Length) cil managed
  // SIG: 00 02 0E 0E 08
  {
    // Method begins at RVA 0x2501
    // Code size       0 (0x0)
  } // end of method Strings::Right

  .method /*06000434*/ public static string 
          RTrim(string str) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x2502
    // Code size       0 (0x0)
  } // end of method Strings::RTrim

  .method /*06000435*/ public static string 
          Trim(string str) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x2503
    // Code size       0 (0x0)
  } // end of method Strings::Trim

  .method /*06000436*/ public static int32 
          StrComp(string String1,
                  string String2,
                  [opt] valuetype Microsoft.VisualBasic.CompareMethod/*02000038*/ Compare) cil managed
  // SIG: 00 03 08 0E 0E 11 80 E0
  {
    .param [3]/*08000623*/  = int32(0x00000000)
    .custom /*0C0000E9:06000239*/ instance void Microsoft.VisualBasic.CompilerServices.OptionCompareAttribute/*02000050*/::.ctor() /* 06000239 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2504
    // Code size       0 (0x0)
  } // end of method Strings::StrComp

  .method /*06000437*/ public static string 
          StrConv(string str,
                  valuetype Microsoft.VisualBasic.VbStrConv/*0200003D*/ Conversion,
                  [opt] int32 LocaleID) cil managed
  // SIG: 00 03 0E 0E 11 80 F4 08
  {
    .param [3]/*08000626*/  = int32(0x00000000)
    // Method begins at RVA 0x2505
    // Code size       0 (0x0)
  } // end of method Strings::StrConv

  .method /*06000438*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2506
    // Code size       0 (0x0)
  } // end of method Strings::.ctor

} // end of class Microsoft.VisualBasic.Strings


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
