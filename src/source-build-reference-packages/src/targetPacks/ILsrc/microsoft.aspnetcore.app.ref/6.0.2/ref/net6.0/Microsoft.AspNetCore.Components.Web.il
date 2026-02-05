


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
// Number of sections:         0x0004
// Time-date stamp:            0xcc8c4725
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000cc00
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00010a96
// Base of code:                   0x00004000
// Base of data:                   0x00002000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00016000
// Size of headers:                0x00000400
// Checksum:                       0x00019078
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00010a44 [0x0000004f] address [size] of Import Directory:          
// 0x00012000 [0x00000534] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000da00 [0x00002468] address [size] of Security Directory:        
// 0x00014000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00010a28 [0x0000001c] address [size] of Debug Directory:           
// 0x00000000 [0x00000000] address [size] of Architecture Specific:     
// 0x00000000 [0x00000000] address [size] of Global Pointer:            
// 0x00000000 [0x00000000] address [size] of TLS Directory:             
// 0x00000000 [0x00000000] address [size] of Load Config Directory:     
// 0x00000000 [0x00000000] address [size] of Bound Import Directory:    
// 0x00004000 [0x00000008] address [size] of Import Address Table:      
// 0x00000000 [0x00000000] address [size] of Delay Load IAT:            
// 0x00004008 [0x00000048] address [size] of CLR Header:                


// Image sections:
//              .mvid
//              0x00000010 Virtual Size
//              0x00002000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00000400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

//              .text
//              0x0000ca9c Virtual Size
//              0x00004000 Virtual Address
//              0x0000cc00 Size of Raw Data
//              0x00000600 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x00000534 Virtual Size
//              0x00012000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000d200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00014000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0000d800 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00010000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000a98
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00010a86 Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 40 00 10 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00004054 [0x0000c954] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000109a8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00005f34 Size
//              '#~' Name
//    Stream 2:
//              0x00005fa0 Offset
//              0x00002a4c Size
//              '#Strings' Name
//    Stream 3:
//              0x000089ec Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000089f0 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00008a00 Offset
//              0x00003f54 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 65128
// PE header size       : 1024 (536 used)    ( 1.57%)
// PE additional info   : 10851             (16.66%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.11%)
// CLR meta-data size  : 51540              (79.14%)
// CLR additional info : 128                ( 0.20%)
// CLR method headers  : 562                ( 0.86%)
// Managed code         : 1124              ( 1.73%)
// Data                 : 2560              ( 3.93%)
// Unaccounted          : -2733             (-4.20%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 52224
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 51540
//   Module        -    1 (10 bytes)
//   TypeDef       -  112 (1568 bytes)    5 interfaces, 0 explicit layout
//   TypeRef       -  111 (666 bytes)
//   MethodDef     -  578 (8092 bytes)    12 abstract, 0 native, 562 bodies
//   FieldDef      -  120 (720 bytes)     2 constant
//   MemberRef     -   42 (252 bytes)
//   ParamDef      -  510 (3060 bytes)
//   MethodImpl    -   37 (222 bytes)
//   Constant      -   23 (138 bytes)
//   CustomAttribute-  950 (5700 bytes)
//   InterfaceImpl -   26 (104 bytes)
//   PropertyMap   -   51 (204 bytes)
//   Property      -  185 (1110 bytes)
//   MethodSemantic-  327 (1962 bytes)
//   TypeSpec      -    3 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    9 (180 bytes)
//   NestedClass   -   24 (96 bytes)
//   GenericParam  -   18 (144 bytes)
//   GenericParamConstraint-    2 (8 bytes)
//   Strings       - 10825 bytes
//   Blobs         - 16212 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   219 bytes

// CLR additional info : 128

// CLR method headers : 562
//   Num.of method bodies  - 562
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 562

// Managed code : 1124
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class BindInputElementAttribute      (public) (auto) (ansi) (sealed) 
// Class DomWrapperInterop              (abstract) (auto) (ansi) (sealed) 
// Class ElementReferenceExtensions     (public) (abstract) (auto) (ansi) (sealed) 
// Class WebElementReferenceContext     (public) (auto) (ansi) 
// Class WebEventDescriptor             (public) (auto) (ansi) (sealed) 
// Class WebRenderer                    (public) (abstract) (auto) (ansi) 
// Class FocusOnNavigate                (public) (auto) (ansi) 
// Class NavLink                        (public) (auto) (ansi) 
// Class NavLinkMatch                   (public) (auto) (ansi) (sealed) 
// Class HeadContent                    (public) (auto) (ansi) (sealed) 
// Class HeadOutlet                     (public) (auto) (ansi) (sealed) 
// Class PageTitle                      (public) (auto) (ansi) (sealed) 
// Interface IJSComponentConfiguration      (public) (abstract) (auto) (ansi) 
// Class JSComponentConfigurationExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class JSComponentConfigurationStore  (public) (auto) (ansi) (sealed) 
// Class ChangeEventArgsReader          (abstract) (auto) (ansi) (sealed) 
// Class ClipboardEventArgsReader       (abstract) (auto) (ansi) (sealed) 
// Class DragEventArgsReader            (abstract) (auto) (ansi) (sealed) 
// Class ErrorEventArgsReader           (abstract) (auto) (ansi) (sealed) 
// Class FocusEventArgsReader           (abstract) (auto) (ansi) (sealed) 
// Class KeyboardEventArgsReader        (abstract) (auto) (ansi) (sealed) 
// Class MouseEventArgsReader           (abstract) (auto) (ansi) (sealed) 
// Class PointerEventArgsReader         (abstract) (auto) (ansi) (sealed) 
// Class ProgressEventArgsReader        (abstract) (auto) (ansi) (sealed) 
// Class TouchEventArgsReader           (abstract) (auto) (ansi) (sealed) 
// Class WebEventData                   (auto) (ansi) 
// Class WebEventDescriptorReader       (abstract) (auto) (ansi) (sealed) 
// Class WheelEventArgsReader           (abstract) (auto) (ansi) (sealed) 
// Class BindAttributes                 (public) (abstract) (auto) (ansi) (sealed) 
// Class ClipboardEventArgs             (public) (auto) (ansi) 
// Class DataTransfer                   (public) (auto) (ansi) 
// Class DataTransferItem               (public) (auto) (ansi) 
// Class DragEventArgs                  (public) (auto) (ansi) 
// Class ErrorBoundary                  (public) (auto) (ansi) 
// Class ErrorEventArgs                 (public) (auto) (ansi) 
// Class EventHandlers                  (public) (abstract) (auto) (ansi) (sealed) 
// Class FocusEventArgs                 (public) (auto) (ansi) 
// Interface IErrorBoundaryLogger           (public) (abstract) (auto) (ansi) 
// Class KeyboardEventArgs              (public) (auto) (ansi) 
// Class MouseEventArgs                 (public) (auto) (ansi) 
// Class PointerEventArgs               (public) (auto) (ansi) 
// Class ProgressEventArgs              (public) (auto) (ansi) 
// Class TouchEventArgs                 (public) (auto) (ansi) 
// Class TouchPoint                     (public) (auto) (ansi) 
// Class WebEventCallbackFactoryEventArgsExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class WebRenderTreeBuilderExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class WheelEventArgs                 (public) (auto) (ansi) 
// Class ItemsProviderDelegate`1        (public) (auto) (ansi) (sealed) 
// Class ItemsProviderRequest           (public) (sequential) (ansi) (sealed) 
// Class ItemsProviderResult`1          (public) (sequential) (ansi) (sealed) 
// Interface IVirtualizeJsCallbacks         (abstract) (auto) (ansi) 
// Class PlaceholderContext             (public) (sequential) (ansi) (sealed) 
// Class Virtualize`1                   (public) (auto) (ansi) (sealed) 
// Class VirtualizeJsInterop            (auto) (ansi) 
// Class JSComponentInterop             (public) (auto) (ansi) 
// Class EditContextFieldClassExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class EditForm                       (public) (auto) (ansi) 
// Class FieldCssClassProvider          (public) (auto) (ansi) 
// Class InputBase`1                    (public) (abstract) (auto) (ansi) 
// Class InputCheckbox                  (public) (auto) (ansi) 
// Class InputDate`1                    (public) (auto) (ansi) 
// Class InputDateType                  (public) (auto) (ansi) (sealed) 
// Class InputExtensions                (abstract) (auto) (ansi) (sealed) 
// Class InputFile                      (public) (auto) (ansi) 
// Class BrowserFile                    (auto) (ansi) (sealed) 
// Class BrowserFileExtensions          (public) (abstract) (auto) (ansi) (sealed) 
// Class BrowserFileStream              (auto) (ansi) (sealed) 
// Interface IBrowserFile                   (public) (abstract) (auto) (ansi) 
// Interface IInputFileJsCallbacks          (abstract) (auto) (ansi) 
// Class InputFileChangeEventArgs       (public) (auto) (ansi) (sealed) 
// Class InputFileInterop               (abstract) (auto) (ansi) (sealed) 
// Class InputFileJsCallbacksRelay      (auto) (ansi) 
// Class RemoteBrowserFileStreamOptions (public) (auto) (ansi) 
// Class InputNumber`1                  (public) (auto) (ansi) 
// Class InputRadio`1                   (public) (auto) (ansi) 
// Class InputRadioContext              (auto) (ansi) 
// Class InputRadioGroup`1              (public) (auto) (ansi) 
// Class InputSelect`1                  (public) (auto) (ansi) 
// Class InputText                      (public) (auto) (ansi) 
// Class InputTextArea                  (public) (auto) (ansi) 
// Class ValidationMessage`1            (public) (auto) (ansi) 
// Class ValidationSummary              (public) (auto) (ansi) 
// Class <PrivateImplementationDetails> (auto) (ansi) (sealed) 
// Class WebRendererInteropMethods      (auto) (ansi) (sealed) (nested assembly) 
// Class NonMatchingType                (auto) (ansi) (nested private) 
// Class <OnAfterRenderAsync>d__15      (auto) (ansi) (sealed) (nested private) 
// Class <OnAfterRenderAsync>d__8       (auto) (ansi) (sealed) (nested private) 
// Class JSComponentParameter           (sequential) (ansi) (sealed) (nested assembly) 
// Class <OnErrorAsync>d__4             (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__60            (auto) (ansi) (sealed) (nested private) 
// Class <OnAfterRenderAsync>d__50      (auto) (ansi) (sealed) (nested private) 
// Class <RefreshDataAsync>d__48        (auto) (ansi) (sealed) (nested private) 
// Class <RefreshDataCoreAsync>d__57    (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__8             (auto) (ansi) (sealed) (nested private) 
// Class <InitializeAsync>d__5          (auto) (ansi) (sealed) (nested private) 
// Class ParameterTypeCache             (sequential) (ansi) (sealed) (nested assembly) 
// Class ParameterKind                  (auto) (ansi) (sealed) (nested assembly) 
// Class ParameterInfo                  (sequential) (ansi) (sealed) (nested assembly) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass18_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass19_0          (auto) (ansi) (sealed) (nested private) 
// Class <HandleSubmitAsync>d__33       (auto) (ansi) (sealed) (nested private) 
// Class <ConvertToImageFileAsync>d__22 (auto) (ansi) (sealed) (nested private) 
// Class <OnAfterRenderAsync>d__19      (auto) (ansi) (sealed) (nested private) 
// Class <CopyFileDataIntoBuffer>d__30  (auto) (ansi) (sealed) (nested private) 
// Class <OpenReadStreamAsync>d__29     (auto) (ansi) (sealed) (nested private) 
// Class <ReadAsync>d__28               (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.AspNetCore.Components
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.JSInterop
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ System.Text.Json
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ Microsoft.AspNetCore.Components.Forms
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ System.Linq.Expressions
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Components.Web
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

  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 78 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...xMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65   // pNetCore.Compone
                                                                                                                                                                                    6E 74 73 2E 53 65 72 76 65 72 2E 54 65 73 74 73   // nts.Server.Tests
                                                                                                                                                                                    2C 20 50 75 62 6C 69 63 4B 65 79 3D 30 30 32 34   // , PublicKey=0024
                                                                                                                                                                                    30 30 30 30 30 34 38 30 30 30 30 30 39 34 30 30   // 0000048000009400
                                                                                                                                                                                    30 30 30 30 30 36 30 32 30 30 30 30 30 30 32 34   // 0000060200000024
                                                                                                                                                                                    30 30 30 30 35 32 35 33 34 31 33 31 30 30 30 34   // 0000525341310004
                                                                                                                                                                                    30 30 30 30 30 31 30 30 30 31 30 30 66 33 33 61   // 000001000100f33a
                                                                                                                                                                                    32 39 30 34 34 66 61 39 64 37 34 30 63 39 62 33   // 29044fa9d740c9b3
                                                                                                                                                                                    32 31 33 61 39 33 65 35 37 63 38 34 62 34 37 32   // 213a93e57c84b472
                                                                                                                                                                                    63 38 34 65 30 62 38 61 30 65 31 61 65 34 38 65   // c84e0b8a0e1ae48e
                                                                                                                                                                                    36 37 61 39 66 38 66 36 64 65 39 64 35 66 37 66   // 67a9f8f6de9d5f7f
                                                                                                                                                                                    33 64 35 32 61 63 32 33 65 34 38 61 63 35 31 38   // 3d52ac23e48ac518
                                                                                                                                                                                    30 31 66 31 64 63 39 35 30 61 62 65 39 30 31 64   // 01f1dc950abe901d
                                                                                                                                                                                    61 33 34 64 32 61 39 65 33 62 61 61 64 62 31 34   // a34d2a9e3baadb14
                                                                                                                                                                                    31 61 31 37 63 37 37 65 66 33 63 35 36 35 64 64   // 1a17c77ef3c565dd
                                                                                                                                                                                    35 65 65 35 30 35 34 62 39 31 63 66 36 33 62 62   // 5ee5054b91cf63bb
                                                                                                                                                                                    33 63 36 61 62 38 33 66 37 32 61 62 33 61 61 66   // 3c6ab83f72ab3aaf
                                                                                                                                                                                    65 39 33 64 30 66 63 33 63 32 33 34 38 62 37 36   // e93d0fc3c2348b76
                                                                                                                                                                                    34 66 61 66 62 30 62 31 63 30 37 33 33 64 65 35   // 4fafb0b1c0733de5
                                                                                                                                                                                    31 34 35 39 61 65 61 62 34 36 35 38 30 33 38 34   // 1459aeab46580384
                                                                                                                                                                                    62 66 39 64 37 34 63 34 65 32 38 31 36 34 62 37   // bf9d74c4e28164b7
                                                                                                                                                                                    63 64 65 32 34 37 66 38 39 31 62 61 30 37 38 39   // cde247f891ba0789
                                                                                                                                                                                    31 63 39 64 38 37 32 61 64 32 62 62 00 00 )       // 1c9d872ad2bb..
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 75 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...uMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65   // pNetCore.Compone
                                                                                                                                                                                    6E 74 73 2E 57 65 62 2E 54 65 73 74 73 2C 20 50   // nts.Web.Tests, P
                                                                                                                                                                                    75 62 6C 69 63 4B 65 79 3D 30 30 32 34 30 30 30   // ublicKey=0024000
                                                                                                                                                                                    30 30 34 38 30 30 30 30 30 39 34 30 30 30 30 30   // 0048000009400000
                                                                                                                                                                                    30 30 36 30 32 30 30 30 30 30 30 32 34 30 30 30   // 0060200000024000
                                                                                                                                                                                    30 35 32 35 33 34 31 33 31 30 30 30 34 30 30 30   // 0525341310004000
                                                                                                                                                                                    30 30 31 30 30 30 31 30 30 66 33 33 61 32 39 30   // 001000100f33a290
                                                                                                                                                                                    34 34 66 61 39 64 37 34 30 63 39 62 33 32 31 33   // 44fa9d740c9b3213
                                                                                                                                                                                    61 39 33 65 35 37 63 38 34 62 34 37 32 63 38 34   // a93e57c84b472c84
                                                                                                                                                                                    65 30 62 38 61 30 65 31 61 65 34 38 65 36 37 61   // e0b8a0e1ae48e67a
                                                                                                                                                                                    39 66 38 66 36 64 65 39 64 35 66 37 66 33 64 35   // 9f8f6de9d5f7f3d5
                                                                                                                                                                                    32 61 63 32 33 65 34 38 61 63 35 31 38 30 31 66   // 2ac23e48ac51801f
                                                                                                                                                                                    31 64 63 39 35 30 61 62 65 39 30 31 64 61 33 34   // 1dc950abe901da34
                                                                                                                                                                                    64 32 61 39 65 33 62 61 61 64 62 31 34 31 61 31   // d2a9e3baadb141a1
                                                                                                                                                                                    37 63 37 37 65 66 33 63 35 36 35 64 64 35 65 65   // 7c77ef3c565dd5ee
                                                                                                                                                                                    35 30 35 34 62 39 31 63 66 36 33 62 62 33 63 36   // 5054b91cf63bb3c6
                                                                                                                                                                                    61 62 38 33 66 37 32 61 62 33 61 61 66 65 39 33   // ab83f72ab3aafe93
                                                                                                                                                                                    64 30 66 63 33 63 32 33 34 38 62 37 36 34 66 61   // d0fc3c2348b764fa
                                                                                                                                                                                    66 62 30 62 31 63 30 37 33 33 64 65 35 31 34 35   // fb0b1c0733de5145
                                                                                                                                                                                    39 61 65 61 62 34 36 35 38 30 33 38 34 62 66 39   // 9aeab46580384bf9
                                                                                                                                                                                    64 37 34 63 34 65 32 38 31 36 34 62 37 63 64 65   // d74c4e28164b7cde
                                                                                                                                                                                    32 34 37 66 38 39 31 62 61 30 37 38 39 31 63 39   // 247f891ba07891c9
                                                                                                                                                                                    64 38 37 32 61 64 32 62 62 00 00 )                // d872ad2bb..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 3B 53 75 70 70 6F 72 74 20 66 6F 72 20 72   // ..;Support for r
                                                                                                                                                                       65 6E 64 65 72 69 6E 67 20 41 53 50 2E 4E 45 54   // endering ASP.NET
                                                                                                                                                                       20 43 6F 72 65 20 63 6F 6D 70 6F 6E 65 6E 74 73   //  Core components
                                                                                                                                                                       20 66 6F 72 20 62 72 6F 77 73 65 72 73 2E 00 00 ) //  for browsers...
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 23 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..#Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                 74 73 2E 57 65 62 00 00 )                         // ts.Web..
  .custom /*0C000017:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
  .ver 6:0:0:0
}
.module Microsoft.AspNetCore.Components.Web.dll
// MVID: {c2cee190-1ec4-44d9-ae1a-3029eafd0834}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000001*/ public initonly uint8[] NullableFlags
  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullableAttribute::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] A_1) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000002*/ public initonly uint8 Flag
  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000027:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000003*/ public initonly bool IncludesInternals
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool A_1) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.BindInputElementAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002E:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'type',
                               string suffix,
                               string valueAttribute,
                               string changeAttribute,
                               bool isInvariantCulture,
                               string format) cil managed
  // SIG: 20 06 01 0E 0E 0E 0E 02 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindInputElementAttribute::.ctor

  .method /*06000007*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000032:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindInputElementAttribute::get_Type

  .method /*06000008*/ public hidebysig specialname 
          instance string  get_Suffix() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindInputElementAttribute::get_Suffix

  .method /*06000009*/ public hidebysig specialname 
          instance string  get_ValueAttribute() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00003D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindInputElementAttribute::get_ValueAttribute

  .method /*0600000A*/ public hidebysig specialname 
          instance string  get_ChangeAttribute() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000042:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindInputElementAttribute::get_ChangeAttribute

  .method /*0600000B*/ public hidebysig specialname 
          instance bool  get_IsInvariantCulture() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindInputElementAttribute::get_IsInvariantCulture

  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_Format() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00004D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindInputElementAttribute::get_Format

  .property /*17000001*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::get_Type() /* 06000007 */
  } // end of property BindInputElementAttribute::Type
  .property /*17000002*/ instance callconv(8) string
          Suffix()
  {
    .get instance string Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::get_Suffix() /* 06000008 */
  } // end of property BindInputElementAttribute::Suffix
  .property /*17000003*/ instance callconv(8) string
          ValueAttribute()
  {
    .get instance string Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::get_ValueAttribute() /* 06000009 */
  } // end of property BindInputElementAttribute::ValueAttribute
  .property /*17000004*/ instance callconv(8) string
          ChangeAttribute()
  {
    .get instance string Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::get_ChangeAttribute() /* 0600000A */
  } // end of property BindInputElementAttribute::ChangeAttribute
  .property /*17000005*/ instance callconv(8) bool
          IsInvariantCulture()
  {
    .get instance bool Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::get_IsInvariantCulture() /* 0600000B */
  } // end of property BindInputElementAttribute::IsInvariantCulture
  .property /*17000006*/ instance callconv(8) string
          Format()
  {
    .get instance string Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::get_Format() /* 0600000C */
  } // end of property BindInputElementAttribute::Format
} // end of class Microsoft.AspNetCore.Components.BindInputElementAttribute

.class /*02000007*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.DomWrapperInterop
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ public static literal string Focus = "Blazor._internal.domWrapper.focus"
  .field /*04000005*/ public static literal string FocusBySelector = "Blazor._internal.domWrapper.focusBySelector"
} // end of class Microsoft.AspNetCore.Components.DomWrapperInterop

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.ElementReferenceExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000039:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600000D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000018*/ 
          FocusAsync(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/ elementReference) cil managed
  // SIG: 00 01 11 61 11 65
  {
    .custom /*0C000053:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementReferenceExtensions::FocusAsync

  .method /*0600000E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000018*/ 
          FocusAsync(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/ elementReference,
                     bool preventScroll) cil managed
  // SIG: 00 02 11 61 11 65 02
  {
    .custom /*0C000059:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementReferenceExtensions::FocusAsync

  .method /*0600000F*/ assembly hidebysig static 
          class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ 
          GetJSRuntime(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/ elementReference) cil managed
  // SIG: 00 01 12 69 11 65
  {
    .custom /*0C00005D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00005E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementReferenceExtensions::GetJSRuntime

} // end of class Microsoft.AspNetCore.Components.ElementReferenceExtensions

.class /*02000009*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.WebElementReferenceContext
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReferenceContext/*0100001B*/
{
  .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000010*/ assembly hidebysig specialname 
          instance class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ 
          get_JSRuntime() cil managed
  // SIG: 20 00 12 69
  {
    .custom /*0C000065:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebElementReferenceContext::get_JSRuntime

  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ jsRuntime) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebElementReferenceContext::.ctor

  .property /*17000007*/ instance callconv(8) class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/
          JSRuntime()
  {
    .get instance class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ Microsoft.AspNetCore.Components.WebElementReferenceContext/*02000009*/::get_JSRuntime() /* 06000010 */
  } // end of property WebElementReferenceContext::JSRuntime
} // end of class Microsoft.AspNetCore.Components.WebElementReferenceContext

.class /*0200000A*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000043:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000012*/ public hidebysig specialname 
          instance uint64  get_EventHandlerId() cil managed
  // SIG: 20 00 0B
  {
    .custom /*0C00006F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventDescriptor::get_EventHandlerId

  .method /*06000013*/ public hidebysig specialname 
          instance void  set_EventHandlerId(uint64 'value') cil managed
  // SIG: 20 01 01 0B
  {
    .custom /*0C000073:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventDescriptor::set_EventHandlerId

  .method /*06000014*/ public hidebysig specialname 
          instance string  get_EventName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000078:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventDescriptor::get_EventName

  .method /*06000015*/ public hidebysig specialname 
          instance void  set_EventName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventDescriptor::set_EventName

  .method /*06000016*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.EventFieldInfo/*0100001C*/ 
          get_EventFieldInfo() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00007F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventDescriptor::get_EventFieldInfo

  .method /*06000017*/ public hidebysig specialname 
          instance void  set_EventFieldInfo(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.EventFieldInfo/*0100001C*/ 'value') cil managed
  // SIG: 20 01 01 12 71
  {
    .custom /*0C000080:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000081:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventDescriptor::set_EventFieldInfo

  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventDescriptor::.ctor

  .property /*17000008*/ instance callconv(8) uint64
          EventHandlerId()
  {
    .get instance uint64 Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor/*0200000A*/::get_EventHandlerId() /* 06000012 */
    .set instance void Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor/*0200000A*/::set_EventHandlerId(uint64) /* 06000013 */
  } // end of property WebEventDescriptor::EventHandlerId
  .property /*17000009*/ instance callconv(8) string
          EventName()
  {
    .get instance string Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor/*0200000A*/::get_EventName() /* 06000014 */
    .set instance void Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor/*0200000A*/::set_EventName(string) /* 06000015 */
  } // end of property WebEventDescriptor::EventName
  .property /*1700000A*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.EventFieldInfo/*0100001C*/
          EventFieldInfo()
  {
    .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.EventFieldInfo/*0100001C*/ Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor/*0200000A*/::get_EventFieldInfo() /* 06000016 */
    .set instance void Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor/*0200000A*/::set_EventFieldInfo(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.EventFieldInfo/*0100001C*/) /* 06000017 */
  } // end of property WebEventDescriptor::EventFieldInfo
} // end of class Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor

.class /*0200000B*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Components.RenderTree.WebRenderer
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.Renderer/*0100001D*/
{
  .custom /*0C000049:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000059*/ auto ansi sealed nested assembly beforefieldinit WebRendererInteropMethods
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000186:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*06000211*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class Microsoft.AspNetCore.Components.RenderTree.WebRenderer/*0200000B*/ renderer,
                                 class [System.Text.Json/*23000006*/]System.Text.Json.JsonSerializerOptions/*01000020*/ jsonOptions,
                                 class Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*/ jsComponentInterop) cil managed
    // SIG: 20 03 01 12 2C 12 80 81 12 80 F0
    {
      .custom /*0C00039F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(string) /* 0A000015 */ = ( 01 00 12 44 69 73 70 61 74 63 68 45 76 65 6E 74   // ...DispatchEvent
                                                                                                                                                                                       41 73 79 6E 63 00 00 )                            // Async..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method WebRendererInteropMethods::.ctor

    .method /*06000212*/ public hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
            DispatchEventAsync(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ eventDescriptor,
                               valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ eventArgs) cil managed
    // SIG: 20 02 12 80 9D 11 80 A1 11 80 A1
    {
      .custom /*0C0003A0:0A000016*/ instance void [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.JSInvokableAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method WebRendererInteropMethods::DispatchEventAsync

    .method /*06000213*/ public hidebysig 
            instance int32  AddRootComponent(string identifier,
                                             string domElementSelector) cil managed
    // SIG: 20 02 08 0E 0E
    {
      .custom /*0C0003A1:0A000016*/ instance void [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.JSInvokableAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method WebRendererInteropMethods::AddRootComponent

    .method /*06000214*/ public hidebysig 
            instance void  SetRootComponentParameters(int32 componentId,
                                                      int32 parameterCount,
                                                      valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ parametersJson) cil managed
    // SIG: 20 03 01 08 08 11 80 A1
    {
      .custom /*0C0003A2:0A000016*/ instance void [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.JSInvokableAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method WebRendererInteropMethods::SetRootComponentParameters

    .method /*06000215*/ public hidebysig 
            instance void  RemoveRootComponent(int32 componentId) cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C0003A3:0A000016*/ instance void [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.JSInvokableAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method WebRendererInteropMethods::RemoveRootComponent

  } // end of class WebRendererInteropMethods

  .method /*06000019*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ComponentModel/*23000004*/]System.IServiceProvider/*0100001E*/ serviceProvider,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100001F*/ loggerFactory,
                               class [System.Text.Json/*23000006*/]System.Text.Json.JsonSerializerOptions/*01000020*/ jsonOptions,
                               class Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*/ jsComponentInterop) cil managed
  // SIG: 20 04 01 12 79 12 7D 12 80 81 12 80 F0
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRenderer::.ctor

  .method /*0600001A*/ family hidebysig specialname 
          instance int32  get_RendererId() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000082:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRenderer::get_RendererId

  .method /*0600001B*/ family hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000021*/) 
          set_RendererId(int32 'value') cil managed
  // SIG: 20 01 1F 80 85 01 08
  {
    .custom /*0C000084:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRenderer::set_RendererId

  .method /*0600001C*/ famorassem hidebysig 
          instance int32  AddRootComponent(class [System.Runtime/*23000001*/]System.Type/*01000024*/ componentType,
                                           string domElementSelector) cil managed
  // SIG: 20 02 08 12 80 91 0E
  {
    .param [1]/*08000014*/ 
    .custom /*0C00007B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRenderer::AddRootComponent

  .method /*0600001D*/ family hidebysig newslot abstract virtual 
          instance void  AttachRootComponentToBrowser(int32 componentId,
                                                      string domElementSelector) cil managed
  // SIG: 20 02 01 08 0E
  {
    // Method begins at RVA 0x0
  } // end of method WebRenderer::AttachRootComponentToBrowser

  .method /*0600001E*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRenderer::Dispose

  .property /*1700000B*/ instance callconv(8) int32
          RendererId()
  {
    .get instance int32 Microsoft.AspNetCore.Components.RenderTree.WebRenderer/*0200000B*/::get_RendererId() /* 0600001A */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000021*/) Microsoft.AspNetCore.Components.RenderTree.WebRenderer/*0200000B*/::set_RendererId(int32) /* 0600001B */
  } // end of property WebRenderer::RendererId
} // end of class Microsoft.AspNetCore.Components.RenderTree.WebRenderer

.class /*0200000C*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Routing.FocusOnNavigate
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
{
  .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200005A*/ auto ansi nested private beforefieldinit NonMatchingType
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .method /*06000216*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NonMatchingType::.ctor

  } // end of class NonMatchingType

  .class /*0200005B*/ auto ansi sealed nested private beforefieldinit '<OnAfterRenderAsync>d__15'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C000189:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001B*/ public int32 '<>1__state'
    .field /*0400001C*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*0400001D*/ public class Microsoft.AspNetCore.Components.Routing.FocusOnNavigate/*0200000C*/ '<>4__this'
    .field /*0400001E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*0100002F*/ '<>u__1'
    .method /*06000217*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnAfterRenderAsync>d__15'::MoveNext

    .method /*06000218*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003A4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnAfterRenderAsync>d__15'::SetStateMachine

  } // end of class '<OnAfterRenderAsync>d__15'

  .method /*0600001F*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RouteData/*0100002A*/ 
          get_RouteData() cil managed
  // SIG: 20 00 12 80 A9
  {
    .custom /*0C00008A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusOnNavigate::get_RouteData

  .method /*06000020*/ public hidebysig specialname 
          instance void  set_RouteData(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RouteData/*0100002A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .custom /*0C00008D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusOnNavigate::set_RouteData

  .method /*06000021*/ public hidebysig specialname 
          instance string  get_Selector() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00008E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusOnNavigate::get_Selector

  .method /*06000022*/ public hidebysig specialname 
          instance void  set_Selector(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00008F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusOnNavigate::set_Selector

  .method /*06000023*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusOnNavigate::OnParametersSet

  .method /*06000024*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          OnAfterRenderAsync(bool firstRender) cil managed
  // SIG: 20 01 12 80 9D 02
  {
    .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000A3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 51 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..QMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 52 6F 75 74 69 6E 67 2E 46 6F 63 75 73   // ts.Routing.Focus
                                                                                                                                                                                                                                        4F 6E 4E 61 76 69 67 61 74 65 2B 3C 4F 6E 41 66   // OnNavigate+<OnAf
                                                                                                                                                                                                                                        74 65 72 52 65 6E 64 65 72 41 73 79 6E 63 3E 64   // terRenderAsync>d
                                                                                                                                                                                                                                        5F 5F 31 35 00 00 )                               // __15..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusOnNavigate::OnAfterRenderAsync

  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusOnNavigate::.ctor

  .property /*1700000C*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RouteData/*0100002A*/
          RouteData()
  {
    .custom /*0C000051:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RouteData/*0100002A*/ Microsoft.AspNetCore.Components.Routing.FocusOnNavigate/*0200000C*/::get_RouteData() /* 0600001F */
    .set instance void Microsoft.AspNetCore.Components.Routing.FocusOnNavigate/*0200000C*/::set_RouteData(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RouteData/*0100002A*/) /* 06000020 */
  } // end of property FocusOnNavigate::RouteData
  .property /*1700000D*/ instance callconv(8) string
          Selector()
  {
    .custom /*0C000057:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Routing.FocusOnNavigate/*0200000C*/::get_Selector() /* 06000021 */
    .set instance void Microsoft.AspNetCore.Components.Routing.FocusOnNavigate/*0200000C*/::set_Selector(string) /* 06000022 */
  } // end of property FocusOnNavigate::Selector
} // end of class Microsoft.AspNetCore.Components.Routing.FocusOnNavigate

.class /*0200000D*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Routing.NavLink
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
{
  .custom /*0C000055:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000026*/ public hidebysig specialname 
          instance string  get_ActiveClass() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000AA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::get_ActiveClass

  .method /*06000027*/ public hidebysig specialname 
          instance void  set_ActiveClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000AD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::set_ActiveClass

  .method /*06000028*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 
          get_AdditionalAttributes() cil managed
  // SIG: 20 00 15 12 80 CD 02 0E 1C
  {
    .custom /*0C0000B0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800001D*/ 
    .custom /*0C000087:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::get_AdditionalAttributes

  .method /*06000029*/ public hidebysig specialname 
          instance void  set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 CD 02 0E 1C
  {
    .custom /*0C0000B3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800001E*/ 
    .custom /*0C000088:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::set_AdditionalAttributes

  .method /*0600002A*/ family hidebysig specialname 
          instance string  get_CssClass() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000110:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::get_CssClass

  .method /*0600002B*/ family hidebysig specialname 
          instance void  set_CssClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000113:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::set_CssClass

  .method /*0600002C*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 
          get_ChildContent() cil managed
  // SIG: 20 00 12 80 D1
  {
    .custom /*0C000115:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::get_ChildContent

  .method /*0600002D*/ public hidebysig specialname 
          instance void  set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D1
  {
    .custom /*0C000118:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::set_ChildContent

  .method /*0600002E*/ public hidebysig specialname 
          instance valuetype Microsoft.AspNetCore.Components.Routing.NavLinkMatch/*0200000E*/ 
          get_Match() cil managed
  // SIG: 20 00 11 38
  {
    .custom /*0C00011B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::get_Match

  .method /*0600002F*/ public hidebysig specialname 
          instance void  set_Match(valuetype Microsoft.AspNetCore.Components.Routing.NavLinkMatch/*0200000E*/ 'value') cil managed
  // SIG: 20 01 01 11 38
  {
    .custom /*0C00011E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::set_Match

  .method /*06000030*/ family hidebysig virtual 
          instance void  OnInitialized() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::OnInitialized

  .method /*06000031*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::OnParametersSet

  .method /*06000032*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::Dispose

  .method /*06000033*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C000128:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::BuildRenderTree

  .method /*06000034*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NavLink::.ctor

  .property /*1700000E*/ instance callconv(8) string
          ActiveClass()
  {
    .custom /*0C00005B:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::get_ActiveClass() /* 06000026 */
    .set instance void Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::set_ActiveClass(string) /* 06000027 */
  } // end of property NavLink::ActiveClass
  .property /*1700000F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>
          AdditionalAttributes()
  {
    .custom /*0C000062:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C000063:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 01 00 54 02 16 43 61 70 74 75 72 65 55 6E   // ....T..CaptureUn
                                                                                                                                                                                        6D 61 74 63 68 65 64 56 61 6C 75 65 73 01 )       // matchedValues.
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::get_AdditionalAttributes() /* 06000028 */
    .set instance void Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>) /* 06000029 */
  } // end of property NavLink::AdditionalAttributes
  .property /*17000010*/ instance callconv(8) string
          CssClass()
  {
    .get instance string Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::get_CssClass() /* 0600002A */
    .set instance void Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::set_CssClass(string) /* 0600002B */
  } // end of property NavLink::CssClass
  .property /*17000011*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/
          ChildContent()
  {
    .custom /*0C00006D:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::get_ChildContent() /* 0600002C */
    .set instance void Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/) /* 0600002D */
  } // end of property NavLink::ChildContent
  .property /*17000012*/ instance callconv(8) valuetype Microsoft.AspNetCore.Components.Routing.NavLinkMatch/*0200000E*/
          Match()
  {
    .custom /*0C000071:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance valuetype Microsoft.AspNetCore.Components.Routing.NavLinkMatch/*0200000E*/ Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::get_Match() /* 0600002E */
    .set instance void Microsoft.AspNetCore.Components.Routing.NavLink/*0200000D*/::set_Match(valuetype Microsoft.AspNetCore.Components.Routing.NavLinkMatch/*0200000E*/) /* 0600002F */
  } // end of property NavLink::Match
} // end of class Microsoft.AspNetCore.Components.Routing.NavLink

.class /*0200000E*/ public auto ansi sealed Microsoft.AspNetCore.Components.Routing.NavLinkMatch
       extends [System.Runtime/*23000001*/]System.Enum/*01000036*/
{
  .field /*04000006*/ public specialname rtspecialname int32 value__
  .field /*04000007*/ public static literal valuetype Microsoft.AspNetCore.Components.Routing.NavLinkMatch/*0200000E*/ Prefix = int32(0x00000000)
  .field /*04000008*/ public static literal valuetype Microsoft.AspNetCore.Components.Routing.NavLinkMatch/*0200000E*/ All = int32(0x00000001)
} // end of class Microsoft.AspNetCore.Components.Routing.NavLinkMatch

.class /*0200000F*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.HeadContent
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
{
  .custom /*0C000060:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000035*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 
          get_ChildContent() cil managed
  // SIG: 20 00 12 80 D1
  {
    .custom /*0C00012C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeadContent::get_ChildContent

  .method /*06000036*/ public hidebysig specialname 
          instance void  set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D1
  {
    .custom /*0C00012D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeadContent::set_ChildContent

  .method /*06000037*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C00012F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeadContent::BuildRenderTree

  .method /*06000038*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeadContent::.ctor

  .property /*17000013*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/
          ChildContent()
  {
    .custom /*0C000077:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ Microsoft.AspNetCore.Components.Web.HeadContent/*0200000F*/::get_ChildContent() /* 06000035 */
    .set instance void Microsoft.AspNetCore.Components.Web.HeadContent/*0200000F*/::set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/) /* 06000036 */
  } // end of property HeadContent::ChildContent
} // end of class Microsoft.AspNetCore.Components.Web.HeadContent

.class /*02000010*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.HeadOutlet
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
{
  .custom /*0C000067:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200005C*/ auto ansi sealed nested private beforefieldinit '<OnAfterRenderAsync>d__8'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C00018B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001F*/ public int32 '<>1__state'
    .field /*04000020*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000021*/ public bool firstRender
    .field /*04000022*/ public class Microsoft.AspNetCore.Components.Web.HeadOutlet/*02000010*/ '<>4__this'
    .field /*04000023*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000037*/<string> '<>u__1'
    .method /*06000219*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnAfterRenderAsync>d__8'::MoveNext

    .method /*0600021A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003A5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnAfterRenderAsync>d__8'::SetStateMachine

  } // end of class '<OnAfterRenderAsync>d__8'

  .field /*04000009*/ static assembly literal string HeadSectionOutletName = "head"
  .field /*0400000A*/ static assembly literal string TitleSectionOutletName = "title"
  .method /*06000039*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          OnAfterRenderAsync(bool firstRender) cil managed
  // SIG: 20 01 12 80 9D 02
  {
    .custom /*0C000133:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 47 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..GMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 57 65 62 2E 48 65 61 64 4F 75 74 6C 65   // ts.Web.HeadOutle
                                                                                                                                                                                                                                        74 2B 3C 4F 6E 41 66 74 65 72 52 65 6E 64 65 72   // t+<OnAfterRender
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 38 00 00 )             // Async>d__8..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeadOutlet::OnAfterRenderAsync

  .method /*0600003A*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeadOutlet::BuildRenderTree

  .method /*0600003B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HeadOutlet::.ctor

} // end of class Microsoft.AspNetCore.Components.Web.HeadOutlet

.class /*02000011*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.PageTitle
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
{
  .custom /*0C00006B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600003C*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 
          get_ChildContent() cil managed
  // SIG: 20 00 12 80 D1
  {
    .custom /*0C000137:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PageTitle::get_ChildContent

  .method /*0600003D*/ public hidebysig specialname 
          instance void  set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D1
  {
    .custom /*0C00013B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PageTitle::set_ChildContent

  .method /*0600003E*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C00013F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PageTitle::BuildRenderTree

  .method /*0600003F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PageTitle::.ctor

  .property /*17000014*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/
          ChildContent()
  {
    .custom /*0C00007C:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ Microsoft.AspNetCore.Components.Web.PageTitle/*02000011*/::get_ChildContent() /* 0600003C */
    .set instance void Microsoft.AspNetCore.Components.Web.PageTitle/*02000011*/::set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/) /* 0600003D */
  } // end of property PageTitle::ChildContent
} // end of class Microsoft.AspNetCore.Components.Web.PageTitle

.class /*02000012*/ interface public abstract auto ansi Microsoft.AspNetCore.Components.Web.IJSComponentConfiguration
{
  .custom /*0C000070:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000040*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/ 
          get_JSComponents() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x0
  } // end of method IJSComponentConfiguration::get_JSComponents

  .property /*17000015*/ instance callconv(8) class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/
          JSComponents()
  {
    .get instance class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/ Microsoft.AspNetCore.Components.Web.IJSComponentConfiguration/*02000012*/::get_JSComponents() /* 06000040 */
  } // end of property IJSComponentConfiguration::JSComponents
} // end of class Microsoft.AspNetCore.Components.Web.IJSComponentConfiguration

.class /*02000013*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.JSComponentConfigurationExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000074:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000076:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000041*/ public hidebysig static 
          void  RegisterForJavaScript<([Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.IComponent/*01000038*/) TComponent>(class Microsoft.AspNetCore.Components.Web.IJSComponentConfiguration/*02000012*/ configuration,
                                                                                                                                                        string identifier) cil managed
  // SIG: 10 01 02 01 12 48 0E
  {
    .custom /*0C000146:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TComponent /*2A000005*/ 
      .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
      .custom /*0C00002B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationExtensions::RegisterForJavaScript

  .method /*06000042*/ public hidebysig static 
          void  RegisterForJavaScript<([Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.IComponent/*01000038*/) TComponent>(class Microsoft.AspNetCore.Components.Web.IJSComponentConfiguration/*02000012*/ configuration,
                                                                                                                                                        string identifier,
                                                                                                                                                        string javaScriptInitializer) cil managed
  // SIG: 10 01 03 01 12 48 0E 0E
  {
    .custom /*0C000147:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TComponent /*2A000007*/ 
      .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
      .custom /*0C000037:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationExtensions::RegisterForJavaScript

  .method /*06000043*/ public hidebysig static 
          void  RegisterForJavaScript(class Microsoft.AspNetCore.Components.Web.IJSComponentConfiguration/*02000012*/ configuration,
                                      class [System.Runtime/*23000001*/]System.Type/*01000024*/ componentType,
                                      string identifier) cil managed
  // SIG: 00 03 01 12 48 12 80 91 0E
  {
    .custom /*0C00014A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00014B:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/,
                                                                                                                                                          class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A00001C */ = ( 01 00 08 00 00 00 45 4D 69 63 72 6F 73 6F 66 74   // ......EMicrosoft
                                                                                                                                                                                                                                        2E 41 73 70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70   // .AspNetCore.Comp
                                                                                                                                                                                                                                        6F 6E 65 6E 74 73 2E 57 65 62 2E 49 6E 66 72 61   // onents.Web.Infra
                                                                                                                                                                                                                                        73 74 72 75 63 74 75 72 65 2E 4A 53 43 6F 6D 70   // structure.JSComp
                                                                                                                                                                                                                                        6F 6E 65 6E 74 49 6E 74 65 72 6F 70 00 00 )       // onentInterop..
    .param [2]/*0800002F*/ 
    .custom /*0C000121:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationExtensions::RegisterForJavaScript

  .method /*06000044*/ public hidebysig static 
          void  RegisterForJavaScript(class Microsoft.AspNetCore.Components.Web.IJSComponentConfiguration/*02000012*/ configuration,
                                      class [System.Runtime/*23000001*/]System.Type/*01000024*/ componentType,
                                      string identifier,
                                      string javaScriptInitializer) cil managed
  // SIG: 00 04 01 12 48 12 80 91 0E 0E
  {
    .custom /*0C00014C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00014D:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/,
                                                                                                                                                          class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A00001C */ = ( 01 00 08 00 00 00 45 4D 69 63 72 6F 73 6F 66 74   // ......EMicrosoft
                                                                                                                                                                                                                                        2E 41 73 70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70   // .AspNetCore.Comp
                                                                                                                                                                                                                                        6F 6E 65 6E 74 73 2E 57 65 62 2E 49 6E 66 72 61   // onents.Web.Infra
                                                                                                                                                                                                                                        73 74 72 75 63 74 75 72 65 2E 4A 53 43 6F 6D 70   // structure.JSComp
                                                                                                                                                                                                                                        6F 6E 65 6E 74 49 6E 74 65 72 6F 70 00 00 )       // onentInterop..
    .param [2]/*08000032*/ 
    .custom /*0C000127:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationExtensions::RegisterForJavaScript

} // end of class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationExtensions

.class /*02000014*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000079:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200005D*/ sequential ansi sealed nested assembly beforefieldinit JSComponentParameter
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
  {
    .custom /*0C00018D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00018E:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003B*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    .field /*04000024*/ private initonly string '<Name>k__BackingField'
    .custom /*0C0000A4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000025*/ private initonly string '<Type>k__BackingField'
    .custom /*0C0000A7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .method /*0600021B*/ public hidebysig specialname 
            instance string  get_Name() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C0003A6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method JSComponentParameter::get_Name

    .method /*0600021C*/ public hidebysig specialname 
            instance string  get_Type() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C0003A7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method JSComponentParameter::get_Type

    .method /*0600021D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(string name,
                                 class [System.Runtime/*23000001*/]System.Type/*01000024*/ dotNetType) cil managed
    // SIG: 20 02 01 0E 12 80 91
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method JSComponentParameter::.ctor

    .property /*170000B6*/ instance callconv(8) string
            Name()
    {
      .get instance string Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*//JSComponentParameter/*0200005D*/::get_Name() /* 0600021B */
    } // end of property JSComponentParameter::Name
    .property /*170000B7*/ instance callconv(8) string
            Type()
    {
      .get instance string Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*//JSComponentParameter/*0200005D*/::get_Type() /* 0600021C */
    } // end of property JSComponentParameter::Type
  } // end of class JSComponentParameter

  .method /*06000045*/ assembly hidebysig specialname 
          instance class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,class [System.Runtime/*23000001*/]System.Type/*01000024*/> 
          get_JSComponentTypesByIdentifier() cil managed
  // SIG: 20 00 15 12 80 E5 02 0E 12 80 91
  {
    .custom /*0C00014F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationStore::get_JSComponentTypesByIdentifier

  .method /*06000046*/ assembly hidebysig specialname 
          instance class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,valuetype Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*//JSComponentParameter/*0200005D*/[]> 
          get_JSComponentParametersByIdentifier() cil managed
  // SIG: 20 00 15 12 80 E5 02 0E 1D 11 81 74
  {
    .custom /*0C000152:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationStore::get_JSComponentParametersByIdentifier

  .method /*06000047*/ assembly hidebysig specialname 
          instance class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,class [System.Collections/*23000007*/]System.Collections.Generic.List`1/*0100003A*/<string>> 
          get_JSComponentIdentifiersByInitializer() cil managed
  // SIG: 20 00 15 12 80 E5 02 0E 15 12 80 E9 01 0E
  {
    .custom /*0C000155:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationStore::get_JSComponentIdentifiersByInitializer

  .method /*06000048*/ assembly hidebysig 
          instance void  Add(class [System.Runtime/*23000001*/]System.Type/*01000024*/ componentType,
                             string identifier) cil managed
  // SIG: 20 02 01 12 80 91 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationStore::Add

  .method /*06000049*/ assembly hidebysig 
          instance void  Add(class [System.Runtime/*23000001*/]System.Type/*01000024*/ componentType,
                             string identifier,
                             string javaScriptInitializer) cil managed
  // SIG: 20 03 01 12 80 91 0E 0E
  {
    .custom /*0C000159:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/,
                                                                                                                                                          class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A00001C */ = ( 01 00 00 02 00 00 56 4D 69 63 72 6F 73 6F 66 74   // ......VMicrosoft
                                                                                                                                                                                                                                        2E 41 73 70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70   // .AspNetCore.Comp
                                                                                                                                                                                                                                        6F 6E 65 6E 74 73 2E 57 65 62 2E 4A 53 43 6F 6D   // onents.Web.JSCom
                                                                                                                                                                                                                                        70 6F 6E 65 6E 74 43 6F 6E 66 69 67 75 72 61 74   // ponentConfigurat
                                                                                                                                                                                                                                        69 6F 6E 53 74 6F 72 65 2B 4A 53 43 6F 6D 70 6F   // ionStore+JSCompo
                                                                                                                                                                                                                                        6E 65 6E 74 50 61 72 61 6D 65 74 65 72 00 00 )    // nentParameter..
    .custom /*0C00015A:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(string,
                                                                                                                                                          class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A00001D */ = ( 01 00 10 41 64 64 52 6F 6F 74 43 6F 6D 70 6F 6E   // ...AddRootCompon
                                                                                                                                                                                                                                        65 6E 74 50 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // entPMicrosoft.As
                                                                                                                                                                                                                                        70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65   // pNetCore.Compone
                                                                                                                                                                                                                                        6E 74 73 2E 52 65 6E 64 65 72 54 72 65 65 2E 57   // nts.RenderTree.W
                                                                                                                                                                                                                                        65 62 52 65 6E 64 65 72 65 72 2B 57 65 62 52 65   // ebRenderer+WebRe
                                                                                                                                                                                                                                        6E 64 65 72 65 72 49 6E 74 65 72 6F 70 4D 65 74   // ndererInteropMet
                                                                                                                                                                                                                                        68 6F 64 73 00 00 )                               // hods..
    .custom /*0C00015B:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(string,
                                                                                                                                                          class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A00001D */ = ( 01 00 1A 53 65 74 52 6F 6F 74 43 6F 6D 70 6F 6E   // ...SetRootCompon
                                                                                                                                                                                                                                        65 6E 74 50 61 72 61 6D 65 74 65 72 73 50 4D 69   // entParametersPMi
                                                                                                                                                                                                                                        63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                                                                        72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 52 65   // re.Components.Re
                                                                                                                                                                                                                                        6E 64 65 72 54 72 65 65 2E 57 65 62 52 65 6E 64   // nderTree.WebRend
                                                                                                                                                                                                                                        65 72 65 72 2B 57 65 62 52 65 6E 64 65 72 65 72   // erer+WebRenderer
                                                                                                                                                                                                                                        49 6E 74 65 72 6F 70 4D 65 74 68 6F 64 73 00 00 ) // InteropMethods..
    .custom /*0C00015C:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(string,
                                                                                                                                                          class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A00001D */ = ( 01 00 13 52 65 6D 6F 76 65 52 6F 6F 74 43 6F 6D   // ...RemoveRootCom
                                                                                                                                                                                                                                        70 6F 6E 65 6E 74 50 4D 69 63 72 6F 73 6F 66 74   // ponentPMicrosoft
                                                                                                                                                                                                                                        2E 41 73 70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70   // .AspNetCore.Comp
                                                                                                                                                                                                                                        6F 6E 65 6E 74 73 2E 52 65 6E 64 65 72 54 72 65   // onents.RenderTre
                                                                                                                                                                                                                                        65 2E 57 65 62 52 65 6E 64 65 72 65 72 2B 57 65   // e.WebRenderer+We
                                                                                                                                                                                                                                        62 52 65 6E 64 65 72 65 72 49 6E 74 65 72 6F 70   // bRendererInterop
                                                                                                                                                                                                                                        4D 65 74 68 6F 64 73 00 00 )                      // Methods..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationStore::Add

  .method /*0600004A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentConfigurationStore::.ctor

  .property /*17000016*/ instance callconv(8) class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,class [System.Runtime/*23000001*/]System.Type/*01000024*/>
          JSComponentTypesByIdentifier()
  {
    .get instance class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,class [System.Runtime/*23000001*/]System.Type/*01000024*/> Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/::get_JSComponentTypesByIdentifier() /* 06000045 */
  } // end of property JSComponentConfigurationStore::JSComponentTypesByIdentifier
  .property /*17000017*/ instance callconv(8) class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,valuetype Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*//JSComponentParameter/*0200005D*/[]>
          JSComponentParametersByIdentifier()
  {
    .get instance class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,valuetype Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*//JSComponentParameter/*0200005D*/[]> Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/::get_JSComponentParametersByIdentifier() /* 06000046 */
  } // end of property JSComponentConfigurationStore::JSComponentParametersByIdentifier
  .property /*17000018*/ instance callconv(8) class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,class [System.Collections/*23000007*/]System.Collections.Generic.List`1/*0100003A*/<string>>
          JSComponentIdentifiersByInitializer()
  {
    .get instance class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,class [System.Collections/*23000007*/]System.Collections.Generic.List`1/*0100003A*/<string>> Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/::get_JSComponentIdentifiersByInitializer() /* 06000047 */
  } // end of property JSComponentConfigurationStore::JSComponentIdentifiersByInitializer
} // end of class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore

.class /*02000015*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.ChangeEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600004B*/ assembly hidebysig static 
          class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ChangeEventArgs/*0100003C*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 F1 11 80 A1
  {
    .custom /*0C00015F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChangeEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.ChangeEventArgsReader

.class /*02000016*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.ClipboardEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600004C*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.ClipboardEventArgs/*02000023*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 8C 11 80 A1
  {
    .custom /*0C000162:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClipboardEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.ClipboardEventArgsReader

.class /*02000017*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.DragEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600004D*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.DragEventArgs/*02000026*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 98 11 80 A1
  {
    .custom /*0C000165:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DragEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.DragEventArgsReader

.class /*02000018*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.ErrorEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600004E*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 A0 11 80 A1
  {
    .custom /*0C000168:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.ErrorEventArgsReader

.class /*02000019*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.FocusEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600004F*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.FocusEventArgs/*0200002A*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 A8 11 80 A1
  {
    .custom /*0C00016C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.FocusEventArgsReader

.class /*0200001A*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.KeyboardEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000050*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 B0 11 80 A1
  {
    .custom /*0C000170:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.KeyboardEventArgsReader

.class /*0200001B*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.MouseEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000085:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000086:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000051*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 B4 11 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgsReader::Read

  .method /*06000052*/ assembly hidebysig static 
          void  ReadProperty(class Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/ eventArgs,
                             valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonProperty/*0100003D*/ 'property') cil managed
  // SIG: 00 02 01 12 80 B4 11 80 F5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgsReader::ReadProperty

} // end of class Microsoft.AspNetCore.Components.Web.MouseEventArgsReader

.class /*0200001C*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.PointerEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000053*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 B8 11 80 A1
  {
    .custom /*0C000177:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.PointerEventArgsReader

.class /*0200001D*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.ProgressEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000054*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 BC 11 80 A1
  {
    .custom /*0C00017A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.ProgressEventArgsReader

.class /*0200001E*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.TouchEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000055*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 C0 11 80 A1
  {
    .custom /*0C00017D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.TouchEventArgsReader

.class /*0200001F*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.WebEventData
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000056*/ public hidebysig static 
          class Microsoft.AspNetCore.Components.Web.WebEventData/*0200001F*/ 
          Parse(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.Renderer/*0100001D*/ renderer,
                class [System.Text.Json/*23000006*/]System.Text.Json.JsonSerializerOptions/*01000020*/ jsonSerializerOptions,
                valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ eventDescriptorJson,
                valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ eventArgsJson) cil managed
  // SIG: 00 04 12 7C 12 75 12 80 81 11 80 A1 11 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventData::Parse

  .method /*06000057*/ public hidebysig static 
          class Microsoft.AspNetCore.Components.Web.WebEventData/*0200001F*/ 
          Parse(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.Renderer/*0100001D*/ renderer,
                class [System.Text.Json/*23000006*/]System.Text.Json.JsonSerializerOptions/*01000020*/ jsonSerializerOptions,
                class Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor/*0200000A*/ eventDescriptor,
                valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ eventArgsJson) cil managed
  // SIG: 00 04 12 7C 12 75 12 80 81 12 28 11 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventData::Parse

  .method /*06000058*/ public hidebysig specialname 
          instance uint64  get_EventHandlerId() cil managed
  // SIG: 20 00 0B
  {
    .custom /*0C000182:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventData::get_EventHandlerId

  .method /*06000059*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.EventFieldInfo/*0100001C*/ 
          get_EventFieldInfo() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000184:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000185:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventData::get_EventFieldInfo

  .method /*0600005A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/ 
          get_EventArgs() cil managed
  // SIG: 20 00 12 80 F9
  {
    .custom /*0C000187:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventData::get_EventArgs

  .property /*17000019*/ instance callconv(8) uint64
          EventHandlerId()
  {
    .get instance uint64 Microsoft.AspNetCore.Components.Web.WebEventData/*0200001F*/::get_EventHandlerId() /* 06000058 */
  } // end of property WebEventData::EventHandlerId
  .property /*1700001A*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.EventFieldInfo/*0100001C*/
          EventFieldInfo()
  {
    .custom /*0C000083:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderTree.EventFieldInfo/*0100001C*/ Microsoft.AspNetCore.Components.Web.WebEventData/*0200001F*/::get_EventFieldInfo() /* 06000059 */
  } // end of property WebEventData::EventFieldInfo
  .property /*1700001B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
          EventArgs()
  {
    .get instance class [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/ Microsoft.AspNetCore.Components.Web.WebEventData/*0200001F*/::get_EventArgs() /* 0600005A */
  } // end of property WebEventData::EventArgs
} // end of class Microsoft.AspNetCore.Components.Web.WebEventData

.class /*02000020*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.WebEventDescriptorReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600005B*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.RenderTree.WebEventDescriptor/*0200000A*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 28 11 80 A1
  {
    .custom /*0C000188:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventDescriptorReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.WebEventDescriptorReader

.class /*02000021*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.WheelEventArgsReader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600005C*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/ 
          Read(valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ jsonElement) cil managed
  // SIG: 00 01 12 80 D0 11 80 A1
  {
    .custom /*0C00018A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgsReader::Read

} // end of class Microsoft.AspNetCore.Components.Web.WheelEventArgsReader

.class /*02000022*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.BindAttributes
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000090:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 FF FF 05 76 61 6C 75 65 08 6F 6E 63 68 61   // .....value.oncha
                                                                                                                                                      6E 67 65 00 FF 00 00 )                            // nge....
  .custom /*0C000091:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 FF 05 76 61 6C 75 65 05 76 61 6C 75 65 08   // ....value.value.
                                                                                                                                                      6F 6E 63 68 61 6E 67 65 00 FF 00 00 )             // onchange....
  .custom /*0C000092:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 08 63 68 65 63 6B 62 6F 78 FF 07 63 68 65   // ...checkbox..che
                                                                                                                                                      63 6B 65 64 08 6F 6E 63 68 61 6E 67 65 00 FF 00   // cked.onchange...
                                                                                                                                                      00 ) 
  .custom /*0C000093:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 04 74 65 78 74 FF 05 76 61 6C 75 65 08 6F   // ...text..value.o
                                                                                                                                                      6E 63 68 61 6E 67 65 00 FF 00 00 )                // nchange....
  .custom /*0C000094:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 06 6E 75 6D 62 65 72 FF 05 76 61 6C 75 65   // ...number..value
                                                                                                                                                      08 6F 6E 63 68 61 6E 67 65 01 FF 00 00 )          // .onchange....
  .custom /*0C000095:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 06 6E 75 6D 62 65 72 05 76 61 6C 75 65 05   // ...number.value.
                                                                                                                                                      76 61 6C 75 65 08 6F 6E 63 68 61 6E 67 65 01 FF   // value.onchange..
                                                                                                                                                      00 00 ) 
  .custom /*0C000096:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 04 64 61 74 65 FF 05 76 61 6C 75 65 08 6F   // ...date..value.o
                                                                                                                                                      6E 63 68 61 6E 67 65 01 0A 79 79 79 79 2D 4D 4D   // nchange..yyyy-MM
                                                                                                                                                      2D 64 64 00 00 )                                  // -dd..
  .custom /*0C000097:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 04 64 61 74 65 05 76 61 6C 75 65 05 76 61   // ...date.value.va
                                                                                                                                                      6C 75 65 08 6F 6E 63 68 61 6E 67 65 01 0A 79 79   // lue.onchange..yy
                                                                                                                                                      79 79 2D 4D 4D 2D 64 64 00 00 )                   // yy-MM-dd..
  .custom /*0C000098:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 0E 64 61 74 65 74 69 6D 65 2D 6C 6F 63 61   // ...datetime-loca
                                                                                                                                                      6C FF 05 76 61 6C 75 65 08 6F 6E 63 68 61 6E 67   // l..value.onchang
                                                                                                                                                      65 01 13 79 79 79 79 2D 4D 4D 2D 64 64 54 48 48   // e..yyyy-MM-ddTHH
                                                                                                                                                      3A 6D 6D 3A 73 73 00 00 )                         // :mm:ss..
  .custom /*0C000099:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 0E 64 61 74 65 74 69 6D 65 2D 6C 6F 63 61   // ...datetime-loca
                                                                                                                                                      6C 05 76 61 6C 75 65 05 76 61 6C 75 65 08 6F 6E   // l.value.value.on
                                                                                                                                                      63 68 61 6E 67 65 01 13 79 79 79 79 2D 4D 4D 2D   // change..yyyy-MM-
                                                                                                                                                      64 64 54 48 48 3A 6D 6D 3A 73 73 00 00 )          // ddTHH:mm:ss..
  .custom /*0C00009A:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 05 6D 6F 6E 74 68 FF 05 76 61 6C 75 65 08   // ...month..value.
                                                                                                                                                      6F 6E 63 68 61 6E 67 65 01 07 79 79 79 79 2D 4D   // onchange..yyyy-M
                                                                                                                                                      4D 00 00 )                                        // M..
  .custom /*0C00009B:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 05 6D 6F 6E 74 68 05 76 61 6C 75 65 05 76   // ...month.value.v
                                                                                                                                                      61 6C 75 65 08 6F 6E 63 68 61 6E 67 65 01 07 79   // alue.onchange..y
                                                                                                                                                      79 79 79 2D 4D 4D 00 00 )                         // yyy-MM..
  .custom /*0C00009C:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 04 74 69 6D 65 FF 05 76 61 6C 75 65 08 6F   // ...time..value.o
                                                                                                                                                      6E 63 68 61 6E 67 65 01 08 48 48 3A 6D 6D 3A 73   // nchange..HH:mm:s
                                                                                                                                                      73 00 00 )                                        // s..
  .custom /*0C00009D:06000006*/ instance void Microsoft.AspNetCore.Components.BindInputElementAttribute/*02000006*/::.ctor(string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           string,
                                                                                                                           bool,
                                                                                                                           string) /* 06000006 */ = ( 01 00 04 74 69 6D 65 05 76 61 6C 75 65 05 76 61   // ...time.value.va
                                                                                                                                                      6C 75 65 08 6F 6E 63 68 61 6E 67 65 01 08 48 48   // lue.onchange..HH
                                                                                                                                                      3A 6D 6D 3A 73 73 00 00 )                         // :mm:ss..
  .custom /*0C00009E:0A00001F*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.BindElementAttribute/*0100003F*/::.ctor(string,
                                                                                                                                                                   string,
                                                                                                                                                                   string,
                                                                                                                                                                   string) /* 0A00001F */ = ( 01 00 06 73 65 6C 65 63 74 FF 05 76 61 6C 75 65   // ...select..value
                                                                                                                                                                                              08 6F 6E 63 68 61 6E 67 65 00 00 )                // .onchange..
  .custom /*0C00009F:0A00001F*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.BindElementAttribute/*0100003F*/::.ctor(string,
                                                                                                                                                                   string,
                                                                                                                                                                   string,
                                                                                                                                                                   string) /* 0A00001F */ = ( 01 00 08 74 65 78 74 61 72 65 61 FF 05 76 61 6C   // ...textarea..val
                                                                                                                                                                                              75 65 08 6F 6E 63 68 61 6E 67 65 00 00 )          // ue.onchange..
} // end of class Microsoft.AspNetCore.Components.Web.BindAttributes

.class /*02000023*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.ClipboardEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
{
  .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005D*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00018C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClipboardEventArgs::get_Type

  .method /*0600005E*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00018F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClipboardEventArgs::set_Type

  .method /*0600005F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClipboardEventArgs::.ctor

  .property /*1700001C*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.ClipboardEventArgs/*02000023*/::get_Type() /* 0600005D */
    .set instance void Microsoft.AspNetCore.Components.Web.ClipboardEventArgs/*02000023*/::set_Type(string) /* 0600005E */
  } // end of property ClipboardEventArgs::Type
} // end of class Microsoft.AspNetCore.Components.Web.ClipboardEventArgs

.class /*02000024*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.DataTransfer
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000060*/ public hidebysig specialname 
          instance string  get_DropEffect() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000192:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::get_DropEffect

  .method /*06000061*/ public hidebysig specialname 
          instance void  set_DropEffect(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000194:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::set_DropEffect

  .method /*06000062*/ public hidebysig specialname 
          instance string  get_EffectAllowed() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000196:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000197:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::get_EffectAllowed

  .method /*06000063*/ public hidebysig specialname 
          instance void  set_EffectAllowed(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000199:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00019A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::set_EffectAllowed

  .method /*06000064*/ public hidebysig specialname 
          instance string[]  get_Files() cil managed
  // SIG: 20 00 1D 0E
  {
    .custom /*0C00019C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::get_Files

  .method /*06000065*/ public hidebysig specialname 
          instance void  set_Files(string[] 'value') cil managed
  // SIG: 20 01 01 1D 0E
  {
    .custom /*0C00019E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::set_Files

  .method /*06000066*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/[] 
          get_Items() cil managed
  // SIG: 20 00 1D 12 80 94
  {
    .custom /*0C0001A1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::get_Items

  .method /*06000067*/ public hidebysig specialname 
          instance void  set_Items(class Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 80 94
  {
    .custom /*0C0001A3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::set_Items

  .method /*06000068*/ public hidebysig specialname 
          instance string[]  get_Types() cil managed
  // SIG: 20 00 1D 0E
  {
    .custom /*0C0001A6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::get_Types

  .method /*06000069*/ public hidebysig specialname 
          instance void  set_Types(string[] 'value') cil managed
  // SIG: 20 01 01 1D 0E
  {
    .custom /*0C0001A8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::set_Types

  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransfer::.ctor

  .property /*1700001D*/ instance callconv(8) string
          DropEffect()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::get_DropEffect() /* 06000060 */
    .set instance void Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::set_DropEffect(string) /* 06000061 */
  } // end of property DataTransfer::DropEffect
  .property /*1700001E*/ instance callconv(8) string
          EffectAllowed()
  {
    .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::get_EffectAllowed() /* 06000062 */
    .set instance void Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::set_EffectAllowed(string) /* 06000063 */
  } // end of property DataTransfer::EffectAllowed
  .property /*1700001F*/ instance callconv(8) string[]
          Files()
  {
    .get instance string[] Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::get_Files() /* 06000064 */
    .set instance void Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::set_Files(string[]) /* 06000065 */
  } // end of property DataTransfer::Files
  .property /*17000020*/ instance callconv(8) class Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/[]
          Items()
  {
    .get instance class Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/[] Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::get_Items() /* 06000066 */
    .set instance void Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::set_Items(class Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/[]) /* 06000067 */
  } // end of property DataTransfer::Items
  .property /*17000021*/ instance callconv(8) string[]
          Types()
  {
    .get instance string[] Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::get_Types() /* 06000068 */
    .set instance void Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/::set_Types(string[]) /* 06000069 */
  } // end of property DataTransfer::Types
} // end of class Microsoft.AspNetCore.Components.Web.DataTransfer

.class /*02000025*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.DataTransferItem
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006B*/ public hidebysig specialname 
          instance string  get_Kind() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001AB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransferItem::get_Kind

  .method /*0600006C*/ public hidebysig specialname 
          instance void  set_Kind(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001AF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransferItem::set_Kind

  .method /*0600006D*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001B3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransferItem::get_Type

  .method /*0600006E*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001B7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransferItem::set_Type

  .method /*0600006F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataTransferItem::.ctor

  .property /*17000022*/ instance callconv(8) string
          Kind()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/::get_Kind() /* 0600006B */
    .set instance void Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/::set_Kind(string) /* 0600006C */
  } // end of property DataTransferItem::Kind
  .property /*17000023*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/::get_Type() /* 0600006D */
    .set instance void Microsoft.AspNetCore.Components.Web.DataTransferItem/*02000025*/::set_Type(string) /* 0600006E */
  } // end of property DataTransferItem::Type
} // end of class Microsoft.AspNetCore.Components.Web.DataTransferItem

.class /*02000026*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.DragEventArgs
       extends Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/
{
  .custom /*0C0000AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000070*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/ 
          get_DataTransfer() cil managed
  // SIG: 20 00 12 80 90
  {
    .custom /*0C0001BD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DragEventArgs::get_DataTransfer

  .method /*06000071*/ public hidebysig specialname 
          instance void  set_DataTransfer(class Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 90
  {
    .custom /*0C0001C1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DragEventArgs::set_DataTransfer

  .method /*06000072*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DragEventArgs::.ctor

  .property /*17000024*/ instance callconv(8) class Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/
          DataTransfer()
  {
    .get instance class Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/ Microsoft.AspNetCore.Components.Web.DragEventArgs/*02000026*/::get_DataTransfer() /* 06000070 */
    .set instance void Microsoft.AspNetCore.Components.Web.DragEventArgs/*02000026*/::set_DataTransfer(class Microsoft.AspNetCore.Components.Web.DataTransfer/*02000024*/) /* 06000071 */
  } // end of property DragEventArgs::DataTransfer
} // end of class Microsoft.AspNetCore.Components.Web.DragEventArgs

.class /*02000027*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.ErrorBoundary
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ErrorBoundaryBase/*01000040*/
{
  .custom /*0C0000AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200005E*/ auto ansi sealed nested private beforefieldinit '<OnErrorAsync>d__4'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C000190:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000026*/ public int32 '<>1__state'
    .field /*04000027*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000028*/ public class Microsoft.AspNetCore.Components.Web.ErrorBoundary/*02000027*/ '<>4__this'
    .field /*04000029*/ public class [System.Runtime/*23000001*/]System.Exception/*01000041*/ exception
    .field /*0400002A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*0100002F*/ '<>u__1'
    .method /*0600021E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnErrorAsync>d__4'::MoveNext

    .method /*0600021F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003A8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnErrorAsync>d__4'::SetStateMachine

  } // end of class '<OnErrorAsync>d__4'

  .method /*06000073*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          OnErrorAsync(class [System.Runtime/*23000001*/]System.Exception/*01000041*/ exception) cil managed
  // SIG: 20 01 12 80 9D 12 81 05
  {
    .custom /*0C0001C3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 44 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..DMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 57 65 62 2E 45 72 72 6F 72 42 6F 75 6E   // ts.Web.ErrorBoun
                                                                                                                                                                                                                                        64 61 72 79 2B 3C 4F 6E 45 72 72 6F 72 41 73 79   // dary+<OnErrorAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 34 00 00 )                      // nc>d__4..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorBoundary::OnErrorAsync

  .method /*06000074*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorBoundary::BuildRenderTree

  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorBoundary::.ctor

} // end of class Microsoft.AspNetCore.Components.Web.ErrorBoundary

.class /*02000028*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.ErrorEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
{
  .custom /*0C0000B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000076*/ public hidebysig specialname 
          instance string  get_Message() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001C8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::get_Message

  .method /*06000077*/ public hidebysig specialname 
          instance void  set_Message(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001CB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::set_Message

  .method /*06000078*/ public hidebysig specialname 
          instance string  get_Filename() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001CE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::get_Filename

  .method /*06000079*/ public hidebysig specialname 
          instance void  set_Filename(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001D1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::set_Filename

  .method /*0600007A*/ public hidebysig specialname 
          instance int32  get_Lineno() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0001D4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::get_Lineno

  .method /*0600007B*/ public hidebysig specialname 
          instance void  set_Lineno(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0001D7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::set_Lineno

  .method /*0600007C*/ public hidebysig specialname 
          instance int32  get_Colno() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0001D9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::get_Colno

  .method /*0600007D*/ public hidebysig specialname 
          instance void  set_Colno(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0001DC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::set_Colno

  .method /*0600007E*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001DF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::get_Type

  .method /*0600007F*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001E1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::set_Type

  .method /*06000080*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorEventArgs::.ctor

  .property /*17000025*/ instance callconv(8) string
          Message()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::get_Message() /* 06000076 */
    .set instance void Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::set_Message(string) /* 06000077 */
  } // end of property ErrorEventArgs::Message
  .property /*17000026*/ instance callconv(8) string
          Filename()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::get_Filename() /* 06000078 */
    .set instance void Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::set_Filename(string) /* 06000079 */
  } // end of property ErrorEventArgs::Filename
  .property /*17000027*/ instance callconv(8) int32
          Lineno()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::get_Lineno() /* 0600007A */
    .set instance void Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::set_Lineno(int32) /* 0600007B */
  } // end of property ErrorEventArgs::Lineno
  .property /*17000028*/ instance callconv(8) int32
          Colno()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::get_Colno() /* 0600007C */
    .set instance void Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::set_Colno(int32) /* 0600007D */
  } // end of property ErrorEventArgs::Colno
  .property /*17000029*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::get_Type() /* 0600007E */
    .set instance void Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/::set_Type(string) /* 0600007F */
  } // end of property ErrorEventArgs::Type
} // end of class Microsoft.AspNetCore.Components.Web.ErrorEventArgs

.class /*02000029*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.EventHandlers
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000B4:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 66 6F 63 75 73 32 4D 69 63 72 6F   // ...onfocus2Micro
                                                                                                                                                                                             73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E   // soft.AspNetCore.
                                                                                                                                                                                             43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 46   // Components.Web.F
                                                                                                                                                                                             6F 63 75 73 45 76 65 6E 74 41 72 67 73 01 01 00   // ocusEventArgs...
                                                                                                                                                                                             00 ) 
  .custom /*0C0000B5:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 06 6F 6E 62 6C 75 72 32 4D 69 63 72 6F 73   // ...onblur2Micros
                                                                                                                                                                                             6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E 43   // oft.AspNetCore.C
                                                                                                                                                                                             6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 46 6F   // omponents.Web.Fo
                                                                                                                                                                                             63 75 73 45 76 65 6E 74 41 72 67 73 01 01 00 00 ) // cusEventArgs....
  .custom /*0C0000B6:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 66 6F 63 75 73 69 6E 32 4D 69 63   // ...onfocusin2Mic
                                                                                                                                                                                             72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72   // rosoft.AspNetCor
                                                                                                                                                                                             65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62   // e.Components.Web
                                                                                                                                                                                             2E 46 6F 63 75 73 45 76 65 6E 74 41 72 67 73 01   // .FocusEventArgs.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000B7:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0A 6F 6E 66 6F 63 75 73 6F 75 74 32 4D 69   // ...onfocusout2Mi
                                                                                                                                                                                             63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                             72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65   // re.Components.We
                                                                                                                                                                                             62 2E 46 6F 63 75 73 45 76 65 6E 74 41 72 67 73   // b.FocusEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000B8:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 6D 6F 75 73 65 6F 76 65 72 32 4D   // ...onmouseover2M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 4D 6F 75 73 65 45 76 65 6E 74 41 72 67   // eb.MouseEventArg
                                                                                                                                                                                             73 01 01 00 00 )                                  // s....
  .custom /*0C0000B9:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0A 6F 6E 6D 6F 75 73 65 6F 75 74 32 4D 69   // ...onmouseout2Mi
                                                                                                                                                                                             63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                             72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65   // re.Components.We
                                                                                                                                                                                             62 2E 4D 6F 75 73 65 45 76 65 6E 74 41 72 67 73   // b.MouseEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000BA:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 6D 6F 75 73 65 6D 6F 76 65 32 4D   // ...onmousemove2M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 4D 6F 75 73 65 45 76 65 6E 74 41 72 67   // eb.MouseEventArg
                                                                                                                                                                                             73 01 01 00 00 )                                  // s....
  .custom /*0C0000BB:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 6D 6F 75 73 65 64 6F 77 6E 32 4D   // ...onmousedown2M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 4D 6F 75 73 65 45 76 65 6E 74 41 72 67   // eb.MouseEventArg
                                                                                                                                                                                             73 01 01 00 00 )                                  // s....
  .custom /*0C0000BC:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 6D 6F 75 73 65 75 70 32 4D 69 63   // ...onmouseup2Mic
                                                                                                                                                                                             72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72   // rosoft.AspNetCor
                                                                                                                                                                                             65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62   // e.Components.Web
                                                                                                                                                                                             2E 4D 6F 75 73 65 45 76 65 6E 74 41 72 67 73 01   // .MouseEventArgs.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000BD:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 63 6C 69 63 6B 32 4D 69 63 72 6F   // ...onclick2Micro
                                                                                                                                                                                             73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E   // soft.AspNetCore.
                                                                                                                                                                                             43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 4D   // Components.Web.M
                                                                                                                                                                                             6F 75 73 65 45 76 65 6E 74 41 72 67 73 01 01 00   // ouseEventArgs...
                                                                                                                                                                                             00 ) 
  .custom /*0C0000BE:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0A 6F 6E 64 62 6C 63 6C 69 63 6B 32 4D 69   // ...ondblclick2Mi
                                                                                                                                                                                             63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                             72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65   // re.Components.We
                                                                                                                                                                                             62 2E 4D 6F 75 73 65 45 76 65 6E 74 41 72 67 73   // b.MouseEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000BF:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 77 68 65 65 6C 32 4D 69 63 72 6F   // ...onwheel2Micro
                                                                                                                                                                                             73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E   // soft.AspNetCore.
                                                                                                                                                                                             43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 57   // Components.Web.W
                                                                                                                                                                                             68 65 65 6C 45 76 65 6E 74 41 72 67 73 01 01 00   // heelEventArgs...
                                                                                                                                                                                             00 ) 
  .custom /*0C0000C0:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 6D 6F 75 73 65 77 68 65 65 6C 32   // ...onmousewheel2
                                                                                                                                                                                             4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74   // Microsoft.AspNet
                                                                                                                                                                                             43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E   // Core.Components.
                                                                                                                                                                                             57 65 62 2E 57 68 65 65 6C 45 76 65 6E 74 41 72   // Web.WheelEventAr
                                                                                                                                                                                             67 73 01 01 00 00 )                               // gs....
  .custom /*0C0000C1:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0D 6F 6E 63 6F 6E 74 65 78 74 6D 65 6E 75   // ...oncontextmenu
                                                                                                                                                                                             32 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65   // 2Microsoft.AspNe
                                                                                                                                                                                             74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73   // tCore.Components
                                                                                                                                                                                             2E 57 65 62 2E 4D 6F 75 73 65 45 76 65 6E 74 41   // .Web.MouseEventA
                                                                                                                                                                                             72 67 73 01 01 00 00 )                            // rgs....
  .custom /*0C0000C2:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 06 6F 6E 64 72 61 67 31 4D 69 63 72 6F 73   // ...ondrag1Micros
                                                                                                                                                                                             6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E 43   // oft.AspNetCore.C
                                                                                                                                                                                             6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 44 72   // omponents.Web.Dr
                                                                                                                                                                                             61 67 45 76 65 6E 74 41 72 67 73 01 01 00 00 )    // agEventArgs....
  .custom /*0C0000C3:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 64 72 61 67 65 6E 64 31 4D 69 63   // ...ondragend1Mic
                                                                                                                                                                                             72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72   // rosoft.AspNetCor
                                                                                                                                                                                             65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62   // e.Components.Web
                                                                                                                                                                                             2E 44 72 61 67 45 76 65 6E 74 41 72 67 73 01 01   // .DragEventArgs..
                                                                                                                                                                                             00 00 ) 
  .custom /*0C0000C4:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 64 72 61 67 65 6E 74 65 72 31 4D   // ...ondragenter1M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 44 72 61 67 45 76 65 6E 74 41 72 67 73   // eb.DragEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000C5:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 64 72 61 67 6C 65 61 76 65 31 4D   // ...ondragleave1M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 44 72 61 67 45 76 65 6E 74 41 72 67 73   // eb.DragEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000C6:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0A 6F 6E 64 72 61 67 6F 76 65 72 31 4D 69   // ...ondragover1Mi
                                                                                                                                                                                             63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                             72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65   // re.Components.We
                                                                                                                                                                                             62 2E 44 72 61 67 45 76 65 6E 74 41 72 67 73 01   // b.DragEventArgs.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000C7:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 64 72 61 67 73 74 61 72 74 31 4D   // ...ondragstart1M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 44 72 61 67 45 76 65 6E 74 41 72 67 73   // eb.DragEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000C8:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 06 6F 6E 64 72 6F 70 31 4D 69 63 72 6F 73   // ...ondrop1Micros
                                                                                                                                                                                             6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E 43   // oft.AspNetCore.C
                                                                                                                                                                                             6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 44 72   // omponents.Web.Dr
                                                                                                                                                                                             61 67 45 76 65 6E 74 41 72 67 73 01 01 00 00 )    // agEventArgs....
  .custom /*0C0000C9:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 6B 65 79 64 6F 77 6E 35 4D 69 63   // ...onkeydown5Mic
                                                                                                                                                                                             72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72   // rosoft.AspNetCor
                                                                                                                                                                                             65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62   // e.Components.Web
                                                                                                                                                                                             2E 4B 65 79 62 6F 61 72 64 45 76 65 6E 74 41 72   // .KeyboardEventAr
                                                                                                                                                                                             67 73 01 01 00 00 )                               // gs....
  .custom /*0C0000CA:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 6B 65 79 75 70 35 4D 69 63 72 6F   // ...onkeyup5Micro
                                                                                                                                                                                             73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E   // soft.AspNetCore.
                                                                                                                                                                                             43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 4B   // Components.Web.K
                                                                                                                                                                                             65 79 62 6F 61 72 64 45 76 65 6E 74 41 72 67 73   // eyboardEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000CB:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0A 6F 6E 6B 65 79 70 72 65 73 73 35 4D 69   // ...onkeypress5Mi
                                                                                                                                                                                             63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                             72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65   // re.Components.We
                                                                                                                                                                                             62 2E 4B 65 79 62 6F 61 72 64 45 76 65 6E 74 41   // b.KeyboardEventA
                                                                                                                                                                                             72 67 73 01 01 00 00 )                            // rgs....
  .custom /*0C0000CC:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 08 6F 6E 63 68 61 6E 67 65 80 93 4D 69 63   // ...onchange..Mic
                                                                                                                                                                                             72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72   // rosoft.AspNetCor
                                                                                                                                                                                             65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 43 68 61   // e.Components.Cha
                                                                                                                                                                                             6E 67 65 45 76 65 6E 74 41 72 67 73 2C 20 4D 69   // ngeEventArgs, Mi
                                                                                                                                                                                             63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                             72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2C 20 56   // re.Components, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             61 64 62 39 37 39 33 38 32 39 64 64 61 65 36 30   // adb9793829ddae60
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000CD:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 69 6E 70 75 74 80 93 4D 69 63 72   // ...oninput..Micr
                                                                                                                                                                                             6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65   // osoft.AspNetCore
                                                                                                                                                                                             2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 43 68 61 6E   // .Components.Chan
                                                                                                                                                                                             67 65 45 76 65 6E 74 41 72 67 73 2C 20 4D 69 63   // geEventArgs, Mic
                                                                                                                                                                                             72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72   // rosoft.AspNetCor
                                                                                                                                                                                             65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2C 20 56 65   // e.Components, Ve
                                                                                                                                                                                             72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43   // rsion=6.0.0.0, C
                                                                                                                                                                                             75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20   // ulture=neutral, 
                                                                                                                                                                                             50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 61   // PublicKeyToken=a
                                                                                                                                                                                             64 62 39 37 39 33 38 32 39 64 64 61 65 36 30 01   // db9793829ddae60.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000CE:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 69 6E 76 61 6C 69 64 63 53 79 73   // ...oninvalidcSys
                                                                                                                                                                                             74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53   // tem.EventArgs, S
                                                                                                                                                                                             79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56   // ystem.Runtime, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000CF:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 72 65 73 65 74 63 53 79 73 74 65   // ...onresetcSyste
                                                                                                                                                                                             6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79 73   // m.EventArgs, Sys
                                                                                                                                                                                             74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65 72   // tem.Runtime, Ver
                                                                                                                                                                                             73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43 75   // sion=6.0.0.0, Cu
                                                                                                                                                                                             6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50   // lture=neutral, P
                                                                                                                                                                                             75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30   // ublicKeyToken=b0
                                                                                                                                                                                             33 66 35 66 37 66 31 31 64 35 30 61 33 61 01 01   // 3f5f7f11d50a3a..
                                                                                                                                                                                             00 00 ) 
  .custom /*0C0000D0:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 08 6F 6E 73 65 6C 65 63 74 63 53 79 73 74   // ...onselectcSyst
                                                                                                                                                                                             65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79   // em.EventArgs, Sy
                                                                                                                                                                                             73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65   // stem.Runtime, Ve
                                                                                                                                                                                             72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43   // rsion=6.0.0.0, C
                                                                                                                                                                                             75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20   // ulture=neutral, 
                                                                                                                                                                                             50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62   // PublicKeyToken=b
                                                                                                                                                                                             30 33 66 35 66 37 66 31 31 64 35 30 61 33 61 01   // 03f5f7f11d50a3a.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000D1:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0D 6F 6E 73 65 6C 65 63 74 73 74 61 72 74   // ...onselectstart
                                                                                                                                                                                             63 53 79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67   // cSystem.EventArg
                                                                                                                                                                                             73 2C 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // s, System.Runtim
                                                                                                                                                                                             65 2C 20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30   // e, Version=6.0.0
                                                                                                                                                                                             2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74   // .0, Culture=neut
                                                                                                                                                                                             72 61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F   // ral, PublicKeyTo
                                                                                                                                                                                             6B 65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35   // ken=b03f5f7f11d5
                                                                                                                                                                                             30 61 33 61 01 01 00 00 )                         // 0a3a....
  .custom /*0C0000D2:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 11 6F 6E 73 65 6C 65 63 74 69 6F 6E 63 68   // ...onselectionch
                                                                                                                                                                                             61 6E 67 65 63 53 79 73 74 65 6D 2E 45 76 65 6E   // angecSystem.Even
                                                                                                                                                                                             74 41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52 75   // tArgs, System.Ru
                                                                                                                                                                                             6E 74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D 36   // ntime, Version=6
                                                                                                                                                                                             2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D   // .0.0.0, Culture=
                                                                                                                                                                                             6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B   // neutral, PublicK
                                                                                                                                                                                             65 79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37 66   // eyToken=b03f5f7f
                                                                                                                                                                                             31 31 64 35 30 61 33 61 01 01 00 00 )             // 11d50a3a....
  .custom /*0C0000D3:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 08 6F 6E 73 75 62 6D 69 74 63 53 79 73 74   // ...onsubmitcSyst
                                                                                                                                                                                             65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79   // em.EventArgs, Sy
                                                                                                                                                                                             73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65   // stem.Runtime, Ve
                                                                                                                                                                                             72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43   // rsion=6.0.0.0, C
                                                                                                                                                                                             75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20   // ulture=neutral, 
                                                                                                                                                                                             50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62   // PublicKeyToken=b
                                                                                                                                                                                             30 33 66 35 66 37 66 31 31 64 35 30 61 33 61 01   // 03f5f7f11d50a3a.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000D4:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 62 65 66 6F 72 65 63 6F 70 79 63   // ...onbeforecopyc
                                                                                                                                                                                             53 79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67 73   // System.EventArgs
                                                                                                                                                                                             2C 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D 65   // , System.Runtime
                                                                                                                                                                                             2C 20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E   // , Version=6.0.0.
                                                                                                                                                                                             30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72   // 0, Culture=neutr
                                                                                                                                                                                             61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B   // al, PublicKeyTok
                                                                                                                                                                                             65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30   // en=b03f5f7f11d50
                                                                                                                                                                                             61 33 61 01 01 00 00 )                            // a3a....
  .custom /*0C0000D5:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 62 65 66 6F 72 65 63 75 74 63 53   // ...onbeforecutcS
                                                                                                                                                                                             79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C   // ystem.EventArgs,
                                                                                                                                                                                             20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C   //  System.Runtime,
                                                                                                                                                                                             20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30   //  Version=6.0.0.0
                                                                                                                                                                                             2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61   // , Culture=neutra
                                                                                                                                                                                             6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65   // l, PublicKeyToke
                                                                                                                                                                                             6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30 61   // n=b03f5f7f11d50a
                                                                                                                                                                                             33 61 01 01 00 00 )                               // 3a....
  .custom /*0C0000D6:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0D 6F 6E 62 65 66 6F 72 65 70 61 73 74 65   // ...onbeforepaste
                                                                                                                                                                                             63 53 79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67   // cSystem.EventArg
                                                                                                                                                                                             73 2C 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // s, System.Runtim
                                                                                                                                                                                             65 2C 20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30   // e, Version=6.0.0
                                                                                                                                                                                             2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74   // .0, Culture=neut
                                                                                                                                                                                             72 61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F   // ral, PublicKeyTo
                                                                                                                                                                                             6B 65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35   // ken=b03f5f7f11d5
                                                                                                                                                                                             30 61 33 61 01 01 00 00 )                         // 0a3a....
  .custom /*0C0000D7:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 06 6F 6E 63 6F 70 79 36 4D 69 63 72 6F 73   // ...oncopy6Micros
                                                                                                                                                                                             6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E 43   // oft.AspNetCore.C
                                                                                                                                                                                             6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 43 6C   // omponents.Web.Cl
                                                                                                                                                                                             69 70 62 6F 61 72 64 45 76 65 6E 74 41 72 67 73   // ipboardEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000D8:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 05 6F 6E 63 75 74 36 4D 69 63 72 6F 73 6F   // ...oncut6Microso
                                                                                                                                                                                             66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E 43 6F   // ft.AspNetCore.Co
                                                                                                                                                                                             6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 43 6C 69   // mponents.Web.Cli
                                                                                                                                                                                             70 62 6F 61 72 64 45 76 65 6E 74 41 72 67 73 01   // pboardEventArgs.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000D9:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 70 61 73 74 65 36 4D 69 63 72 6F   // ...onpaste6Micro
                                                                                                                                                                                             73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E   // soft.AspNetCore.
                                                                                                                                                                                             43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 43   // Components.Web.C
                                                                                                                                                                                             6C 69 70 62 6F 61 72 64 45 76 65 6E 74 41 72 67   // lipboardEventArg
                                                                                                                                                                                             73 01 01 00 00 )                                  // s....
  .custom /*0C0000DA:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0D 6F 6E 74 6F 75 63 68 63 61 6E 63 65 6C   // ...ontouchcancel
                                                                                                                                                                                             32 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65   // 2Microsoft.AspNe
                                                                                                                                                                                             74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73   // tCore.Components
                                                                                                                                                                                             2E 57 65 62 2E 54 6F 75 63 68 45 76 65 6E 74 41   // .Web.TouchEventA
                                                                                                                                                                                             72 67 73 01 01 00 00 )                            // rgs....
  .custom /*0C0000DB:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0A 6F 6E 74 6F 75 63 68 65 6E 64 32 4D 69   // ...ontouchend2Mi
                                                                                                                                                                                             63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                             72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65   // re.Components.We
                                                                                                                                                                                             62 2E 54 6F 75 63 68 45 76 65 6E 74 41 72 67 73   // b.TouchEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000DC:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 74 6F 75 63 68 6D 6F 76 65 32 4D   // ...ontouchmove2M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 54 6F 75 63 68 45 76 65 6E 74 41 72 67   // eb.TouchEventArg
                                                                                                                                                                                             73 01 01 00 00 )                                  // s....
  .custom /*0C0000DD:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 74 6F 75 63 68 73 74 61 72 74 32   // ...ontouchstart2
                                                                                                                                                                                             4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74   // Microsoft.AspNet
                                                                                                                                                                                             43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E   // Core.Components.
                                                                                                                                                                                             57 65 62 2E 54 6F 75 63 68 45 76 65 6E 74 41 72   // Web.TouchEventAr
                                                                                                                                                                                             67 73 01 01 00 00 )                               // gs....
  .custom /*0C0000DE:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 74 6F 75 63 68 65 6E 74 65 72 32   // ...ontouchenter2
                                                                                                                                                                                             4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74   // Microsoft.AspNet
                                                                                                                                                                                             43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E   // Core.Components.
                                                                                                                                                                                             57 65 62 2E 54 6F 75 63 68 45 76 65 6E 74 41 72   // Web.TouchEventAr
                                                                                                                                                                                             67 73 01 01 00 00 )                               // gs....
  .custom /*0C0000DF:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 74 6F 75 63 68 6C 65 61 76 65 32   // ...ontouchleave2
                                                                                                                                                                                             4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74   // Microsoft.AspNet
                                                                                                                                                                                             43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E   // Core.Components.
                                                                                                                                                                                             57 65 62 2E 54 6F 75 63 68 45 76 65 6E 74 41 72   // Web.TouchEventAr
                                                                                                                                                                                             67 73 01 01 00 00 )                               // gs....
  .custom /*0C0000E0:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 13 6F 6E 67 6F 74 70 6F 69 6E 74 65 72 63   // ...ongotpointerc
                                                                                                                                                                                             61 70 74 75 72 65 34 4D 69 63 72 6F 73 6F 66 74   // apture4Microsoft
                                                                                                                                                                                             2E 41 73 70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70   // .AspNetCore.Comp
                                                                                                                                                                                             6F 6E 65 6E 74 73 2E 57 65 62 2E 50 6F 69 6E 74   // onents.Web.Point
                                                                                                                                                                                             65 72 45 76 65 6E 74 41 72 67 73 01 01 00 00 )    // erEventArgs....
  .custom /*0C0000E1:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 14 6F 6E 6C 6F 73 74 70 6F 69 6E 74 65 72   // ...onlostpointer
                                                                                                                                                                                             63 61 70 74 75 72 65 34 4D 69 63 72 6F 73 6F 66   // capture4Microsof
                                                                                                                                                                                             74 2E 41 73 70 4E 65 74 43 6F 72 65 2E 43 6F 6D   // t.AspNetCore.Com
                                                                                                                                                                                             70 6F 6E 65 6E 74 73 2E 57 65 62 2E 50 6F 69 6E   // ponents.Web.Poin
                                                                                                                                                                                             74 65 72 45 76 65 6E 74 41 72 67 73 01 01 00 00 ) // terEventArgs....
  .custom /*0C0000E2:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0F 6F 6E 70 6F 69 6E 74 65 72 63 61 6E 63   // ...onpointercanc
                                                                                                                                                                                             65 6C 34 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // el4Microsoft.Asp
                                                                                                                                                                                             4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                             74 73 2E 57 65 62 2E 50 6F 69 6E 74 65 72 45 76   // ts.Web.PointerEv
                                                                                                                                                                                             65 6E 74 41 72 67 73 01 01 00 00 )                // entArgs....
  .custom /*0C0000E3:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0D 6F 6E 70 6F 69 6E 74 65 72 64 6F 77 6E   // ...onpointerdown
                                                                                                                                                                                             34 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65   // 4Microsoft.AspNe
                                                                                                                                                                                             74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73   // tCore.Components
                                                                                                                                                                                             2E 57 65 62 2E 50 6F 69 6E 74 65 72 45 76 65 6E   // .Web.PointerEven
                                                                                                                                                                                             74 41 72 67 73 01 01 00 00 )                      // tArgs....
  .custom /*0C0000E4:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0E 6F 6E 70 6F 69 6E 74 65 72 65 6E 74 65   // ...onpointerente
                                                                                                                                                                                             72 34 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E   // r4Microsoft.AspN
                                                                                                                                                                                             65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74   // etCore.Component
                                                                                                                                                                                             73 2E 57 65 62 2E 50 6F 69 6E 74 65 72 45 76 65   // s.Web.PointerEve
                                                                                                                                                                                             6E 74 41 72 67 73 01 01 00 00 )                   // ntArgs....
  .custom /*0C0000E5:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0E 6F 6E 70 6F 69 6E 74 65 72 6C 65 61 76   // ...onpointerleav
                                                                                                                                                                                             65 34 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E   // e4Microsoft.AspN
                                                                                                                                                                                             65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74   // etCore.Component
                                                                                                                                                                                             73 2E 57 65 62 2E 50 6F 69 6E 74 65 72 45 76 65   // s.Web.PointerEve
                                                                                                                                                                                             6E 74 41 72 67 73 01 01 00 00 )                   // ntArgs....
  .custom /*0C0000E6:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0D 6F 6E 70 6F 69 6E 74 65 72 6D 6F 76 65   // ...onpointermove
                                                                                                                                                                                             34 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65   // 4Microsoft.AspNe
                                                                                                                                                                                             74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73   // tCore.Components
                                                                                                                                                                                             2E 57 65 62 2E 50 6F 69 6E 74 65 72 45 76 65 6E   // .Web.PointerEven
                                                                                                                                                                                             74 41 72 67 73 01 01 00 00 )                      // tArgs....
  .custom /*0C0000E7:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 70 6F 69 6E 74 65 72 6F 75 74 34   // ...onpointerout4
                                                                                                                                                                                             4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74   // Microsoft.AspNet
                                                                                                                                                                                             43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E   // Core.Components.
                                                                                                                                                                                             57 65 62 2E 50 6F 69 6E 74 65 72 45 76 65 6E 74   // Web.PointerEvent
                                                                                                                                                                                             41 72 67 73 01 01 00 00 )                         // Args....
  .custom /*0C0000E8:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0D 6F 6E 70 6F 69 6E 74 65 72 6F 76 65 72   // ...onpointerover
                                                                                                                                                                                             34 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65   // 4Microsoft.AspNe
                                                                                                                                                                                             74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73   // tCore.Components
                                                                                                                                                                                             2E 57 65 62 2E 50 6F 69 6E 74 65 72 45 76 65 6E   // .Web.PointerEven
                                                                                                                                                                                             74 41 72 67 73 01 01 00 00 )                      // tArgs....
  .custom /*0C0000E9:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 70 6F 69 6E 74 65 72 75 70 34 4D   // ...onpointerup4M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 50 6F 69 6E 74 65 72 45 76 65 6E 74 41   // eb.PointerEventA
                                                                                                                                                                                             72 67 73 01 01 00 00 )                            // rgs....
  .custom /*0C0000EA:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 63 61 6E 70 6C 61 79 63 53 79 73   // ...oncanplaycSys
                                                                                                                                                                                             74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53   // tem.EventArgs, S
                                                                                                                                                                                             79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56   // ystem.Runtime, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000EB:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 10 6F 6E 63 61 6E 70 6C 61 79 74 68 72 6F   // ...oncanplaythro
                                                                                                                                                                                             75 67 68 63 53 79 73 74 65 6D 2E 45 76 65 6E 74   // ughcSystem.Event
                                                                                                                                                                                             41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52 75 6E   // Args, System.Run
                                                                                                                                                                                             74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D 36 2E   // time, Version=6.
                                                                                                                                                                                             30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E   // 0.0.0, Culture=n
                                                                                                                                                                                             65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65   // eutral, PublicKe
                                                                                                                                                                                             79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37 66 31   // yToken=b03f5f7f1
                                                                                                                                                                                             31 64 35 30 61 33 61 01 01 00 00 )                // 1d50a3a....
  .custom /*0C0000EC:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 63 75 65 63 68 61 6E 67 65 63 53   // ...oncuechangecS
                                                                                                                                                                                             79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C   // ystem.EventArgs,
                                                                                                                                                                                             20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C   //  System.Runtime,
                                                                                                                                                                                             20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30   //  Version=6.0.0.0
                                                                                                                                                                                             2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61   // , Culture=neutra
                                                                                                                                                                                             6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65   // l, PublicKeyToke
                                                                                                                                                                                             6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30 61   // n=b03f5f7f11d50a
                                                                                                                                                                                             33 61 01 01 00 00 )                               // 3a....
  .custom /*0C0000ED:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 10 6F 6E 64 75 72 61 74 69 6F 6E 63 68 61   // ...ondurationcha
                                                                                                                                                                                             6E 67 65 63 53 79 73 74 65 6D 2E 45 76 65 6E 74   // ngecSystem.Event
                                                                                                                                                                                             41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52 75 6E   // Args, System.Run
                                                                                                                                                                                             74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D 36 2E   // time, Version=6.
                                                                                                                                                                                             30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E   // 0.0.0, Culture=n
                                                                                                                                                                                             65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65   // eutral, PublicKe
                                                                                                                                                                                             79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37 66 31   // yToken=b03f5f7f1
                                                                                                                                                                                             31 64 35 30 61 33 61 01 01 00 00 )                // 1d50a3a....
  .custom /*0C0000EE:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 65 6D 70 74 69 65 64 63 53 79 73   // ...onemptiedcSys
                                                                                                                                                                                             74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53   // tem.EventArgs, S
                                                                                                                                                                                             79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56   // ystem.Runtime, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000EF:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 70 61 75 73 65 63 53 79 73 74 65   // ...onpausecSyste
                                                                                                                                                                                             6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79 73   // m.EventArgs, Sys
                                                                                                                                                                                             74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65 72   // tem.Runtime, Ver
                                                                                                                                                                                             73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43 75   // sion=6.0.0.0, Cu
                                                                                                                                                                                             6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50   // lture=neutral, P
                                                                                                                                                                                             75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30   // ublicKeyToken=b0
                                                                                                                                                                                             33 66 35 66 37 66 31 31 64 35 30 61 33 61 01 01   // 3f5f7f11d50a3a..
                                                                                                                                                                                             00 00 ) 
  .custom /*0C0000F0:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 06 6F 6E 70 6C 61 79 63 53 79 73 74 65 6D   // ...onplaycSystem
                                                                                                                                                                                             2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79 73 74   // .EventArgs, Syst
                                                                                                                                                                                             65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65 72 73   // em.Runtime, Vers
                                                                                                                                                                                             69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=6.0.0.0, Cul
                                                                                                                                                                                             74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                             62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30 33   // blicKeyToken=b03
                                                                                                                                                                                             66 35 66 37 66 31 31 64 35 30 61 33 61 01 01 00   // f5f7f11d50a3a...
                                                                                                                                                                                             00 ) 
  .custom /*0C0000F1:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 70 6C 61 79 69 6E 67 63 53 79 73   // ...onplayingcSys
                                                                                                                                                                                             74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53   // tem.EventArgs, S
                                                                                                                                                                                             79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56   // ystem.Runtime, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000F2:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 72 61 74 65 63 68 61 6E 67 65 63   // ...onratechangec
                                                                                                                                                                                             53 79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67 73   // System.EventArgs
                                                                                                                                                                                             2C 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D 65   // , System.Runtime
                                                                                                                                                                                             2C 20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E   // , Version=6.0.0.
                                                                                                                                                                                             30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72   // 0, Culture=neutr
                                                                                                                                                                                             61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B   // al, PublicKeyTok
                                                                                                                                                                                             65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30   // en=b03f5f7f11d50
                                                                                                                                                                                             61 33 61 01 01 00 00 )                            // a3a....
  .custom /*0C0000F3:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 08 6F 6E 73 65 65 6B 65 64 63 53 79 73 74   // ...onseekedcSyst
                                                                                                                                                                                             65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79   // em.EventArgs, Sy
                                                                                                                                                                                             73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65   // stem.Runtime, Ve
                                                                                                                                                                                             72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43   // rsion=6.0.0.0, C
                                                                                                                                                                                             75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20   // ulture=neutral, 
                                                                                                                                                                                             50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62   // PublicKeyToken=b
                                                                                                                                                                                             30 33 66 35 66 37 66 31 31 64 35 30 61 33 61 01   // 03f5f7f11d50a3a.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000F4:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 73 65 65 6B 69 6E 67 63 53 79 73   // ...onseekingcSys
                                                                                                                                                                                             74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53   // tem.EventArgs, S
                                                                                                                                                                                             79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56   // ystem.Runtime, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000F5:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 73 74 61 6C 6C 65 64 63 53 79 73   // ...onstalledcSys
                                                                                                                                                                                             74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53   // tem.EventArgs, S
                                                                                                                                                                                             79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56   // ystem.Runtime, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000F6:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 06 6F 6E 73 74 6F 70 63 53 79 73 74 65 6D   // ...onstopcSystem
                                                                                                                                                                                             2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79 73 74   // .EventArgs, Syst
                                                                                                                                                                                             65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65 72 73   // em.Runtime, Vers
                                                                                                                                                                                             69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43 75 6C   // ion=6.0.0.0, Cul
                                                                                                                                                                                             74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75   // ture=neutral, Pu
                                                                                                                                                                                             62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30 33   // blicKeyToken=b03
                                                                                                                                                                                             66 35 66 37 66 31 31 64 35 30 61 33 61 01 01 00   // f5f7f11d50a3a...
                                                                                                                                                                                             00 ) 
  .custom /*0C0000F7:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 73 75 73 70 65 6E 64 63 53 79 73   // ...onsuspendcSys
                                                                                                                                                                                             74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53   // tem.EventArgs, S
                                                                                                                                                                                             79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56   // ystem.Runtime, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000F8:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 74 69 6D 65 75 70 64 61 74 65 63   // ...ontimeupdatec
                                                                                                                                                                                             53 79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67 73   // System.EventArgs
                                                                                                                                                                                             2C 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D 65   // , System.Runtime
                                                                                                                                                                                             2C 20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E   // , Version=6.0.0.
                                                                                                                                                                                             30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72   // 0, Culture=neutr
                                                                                                                                                                                             61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B   // al, PublicKeyTok
                                                                                                                                                                                             65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30   // en=b03f5f7f11d50
                                                                                                                                                                                             61 33 61 01 01 00 00 )                            // a3a....
  .custom /*0C0000F9:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0E 6F 6E 76 6F 6C 75 6D 65 63 68 61 6E 67   // ...onvolumechang
                                                                                                                                                                                             65 63 53 79 73 74 65 6D 2E 45 76 65 6E 74 41 72   // ecSystem.EventAr
                                                                                                                                                                                             67 73 2C 20 53 79 73 74 65 6D 2E 52 75 6E 74 69   // gs, System.Runti
                                                                                                                                                                                             6D 65 2C 20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E   // me, Version=6.0.
                                                                                                                                                                                             30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75   // 0.0, Culture=neu
                                                                                                                                                                                             74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54   // tral, PublicKeyT
                                                                                                                                                                                             6F 6B 65 6E 3D 62 30 33 66 35 66 37 66 31 31 64   // oken=b03f5f7f11d
                                                                                                                                                                                             35 30 61 33 61 01 01 00 00 )                      // 50a3a....
  .custom /*0C0000FA:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 77 61 69 74 69 6E 67 63 53 79 73   // ...onwaitingcSys
                                                                                                                                                                                             74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53   // tem.EventArgs, S
                                                                                                                                                                                             79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56   // ystem.Runtime, V
                                                                                                                                                                                             65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                                             43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                             20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                             62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000FB:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0B 6F 6E 6C 6F 61 64 73 74 61 72 74 35 4D   // ...onloadstart5M
                                                                                                                                                                                             69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                                             6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57   // ore.Components.W
                                                                                                                                                                                             65 62 2E 50 72 6F 67 72 65 73 73 45 76 65 6E 74   // eb.ProgressEvent
                                                                                                                                                                                             41 72 67 73 01 01 00 00 )                         // Args....
  .custom /*0C0000FC:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 74 69 6D 65 6F 75 74 35 4D 69 63   // ...ontimeout5Mic
                                                                                                                                                                                             72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72   // rosoft.AspNetCor
                                                                                                                                                                                             65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62   // e.Components.Web
                                                                                                                                                                                             2E 50 72 6F 67 72 65 73 73 45 76 65 6E 74 41 72   // .ProgressEventAr
                                                                                                                                                                                             67 73 01 01 00 00 )                               // gs....
  .custom /*0C0000FD:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 61 62 6F 72 74 35 4D 69 63 72 6F   // ...onabort5Micro
                                                                                                                                                                                             73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E   // soft.AspNetCore.
                                                                                                                                                                                             43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 50   // Components.Web.P
                                                                                                                                                                                             72 6F 67 72 65 73 73 45 76 65 6E 74 41 72 67 73   // rogressEventArgs
                                                                                                                                                                                             01 01 00 00 ) 
  .custom /*0C0000FE:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 06 6F 6E 6C 6F 61 64 35 4D 69 63 72 6F 73   // ...onload5Micros
                                                                                                                                                                                             6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E 43   // oft.AspNetCore.C
                                                                                                                                                                                             6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 50 72   // omponents.Web.Pr
                                                                                                                                                                                             6F 67 72 65 73 73 45 76 65 6E 74 41 72 67 73 01   // ogressEventArgs.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C0000FF:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 09 6F 6E 6C 6F 61 64 65 6E 64 35 4D 69 63   // ...onloadend5Mic
                                                                                                                                                                                             72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72   // rosoft.AspNetCor
                                                                                                                                                                                             65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62   // e.Components.Web
                                                                                                                                                                                             2E 50 72 6F 67 72 65 73 73 45 76 65 6E 74 41 72   // .ProgressEventAr
                                                                                                                                                                                             67 73 01 01 00 00 )                               // gs....
  .custom /*0C000100:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0A 6F 6E 70 72 6F 67 72 65 73 73 35 4D 69   // ...onprogress5Mi
                                                                                                                                                                                             63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43 6F   // crosoft.AspNetCo
                                                                                                                                                                                             72 65 2E 43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65   // re.Components.We
                                                                                                                                                                                             62 2E 50 72 6F 67 72 65 73 73 45 76 65 6E 74 41   // b.ProgressEventA
                                                                                                                                                                                             72 67 73 01 01 00 00 )                            // rgs....
  .custom /*0C000101:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 65 72 72 6F 72 32 4D 69 63 72 6F   // ...onerror2Micro
                                                                                                                                                                                             73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E   // soft.AspNetCore.
                                                                                                                                                                                             43 6F 6D 70 6F 6E 65 6E 74 73 2E 57 65 62 2E 45   // Components.Web.E
                                                                                                                                                                                             72 72 6F 72 45 76 65 6E 74 41 72 67 73 01 01 00   // rrorEventArgs...
                                                                                                                                                                                             00 ) 
  .custom /*0C000102:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0A 6F 6E 61 63 74 69 76 61 74 65 63 53 79   // ...onactivatecSy
                                                                                                                                                                                             73 74 65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20   // stem.EventArgs, 
                                                                                                                                                                                             53 79 73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20   // System.Runtime, 
                                                                                                                                                                                             56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C   // Version=6.0.0.0,
                                                                                                                                                                                             20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                                                                             2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                                                                             3D 62 30 33 66 35 66 37 66 31 31 64 35 30 61 33   // =b03f5f7f11d50a3
                                                                                                                                                                                             61 01 01 00 00 )                                  // a....
  .custom /*0C000103:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 10 6F 6E 62 65 66 6F 72 65 61 63 74 69 76   // ...onbeforeactiv
                                                                                                                                                                                             61 74 65 63 53 79 73 74 65 6D 2E 45 76 65 6E 74   // atecSystem.Event
                                                                                                                                                                                             41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52 75 6E   // Args, System.Run
                                                                                                                                                                                             74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D 36 2E   // time, Version=6.
                                                                                                                                                                                             30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E   // 0.0.0, Culture=n
                                                                                                                                                                                             65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65   // eutral, PublicKe
                                                                                                                                                                                             79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37 66 31   // yToken=b03f5f7f1
                                                                                                                                                                                             31 64 35 30 61 33 61 01 01 00 00 )                // 1d50a3a....
  .custom /*0C000104:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 12 6F 6E 62 65 66 6F 72 65 64 65 61 63 74   // ...onbeforedeact
                                                                                                                                                                                             69 76 61 74 65 63 53 79 73 74 65 6D 2E 45 76 65   // ivatecSystem.Eve
                                                                                                                                                                                             6E 74 41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52   // ntArgs, System.R
                                                                                                                                                                                             75 6E 74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D   // untime, Version=
                                                                                                                                                                                             36 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65   // 6.0.0.0, Culture
                                                                                                                                                                                             3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63   // =neutral, Public
                                                                                                                                                                                             4B 65 79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37   // KeyToken=b03f5f7
                                                                                                                                                                                             66 31 31 64 35 30 61 33 61 01 01 00 00 )          // f11d50a3a....
  .custom /*0C000105:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 64 65 61 63 74 69 76 61 74 65 63   // ...ondeactivatec
                                                                                                                                                                                             53 79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67 73   // System.EventArgs
                                                                                                                                                                                             2C 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D 65   // , System.Runtime
                                                                                                                                                                                             2C 20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E   // , Version=6.0.0.
                                                                                                                                                                                             30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72   // 0, Culture=neutr
                                                                                                                                                                                             61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B   // al, PublicKeyTok
                                                                                                                                                                                             65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30   // en=b03f5f7f11d50
                                                                                                                                                                                             61 33 61 01 01 00 00 )                            // a3a....
  .custom /*0C000106:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 07 6F 6E 65 6E 64 65 64 63 53 79 73 74 65   // ...onendedcSyste
                                                                                                                                                                                             6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79 73   // m.EventArgs, Sys
                                                                                                                                                                                             74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65 72   // tem.Runtime, Ver
                                                                                                                                                                                             73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43 75   // sion=6.0.0.0, Cu
                                                                                                                                                                                             6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50   // lture=neutral, P
                                                                                                                                                                                             75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30   // ublicKeyToken=b0
                                                                                                                                                                                             33 66 35 66 37 66 31 31 64 35 30 61 33 61 01 01   // 3f5f7f11d50a3a..
                                                                                                                                                                                             00 00 ) 
  .custom /*0C000107:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 12 6F 6E 66 75 6C 6C 73 63 72 65 65 6E 63   // ...onfullscreenc
                                                                                                                                                                                             68 61 6E 67 65 63 53 79 73 74 65 6D 2E 45 76 65   // hangecSystem.Eve
                                                                                                                                                                                             6E 74 41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52   // ntArgs, System.R
                                                                                                                                                                                             75 6E 74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D   // untime, Version=
                                                                                                                                                                                             36 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65   // 6.0.0.0, Culture
                                                                                                                                                                                             3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63   // =neutral, Public
                                                                                                                                                                                             4B 65 79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37   // KeyToken=b03f5f7
                                                                                                                                                                                             66 31 31 64 35 30 61 33 61 01 01 00 00 )          // f11d50a3a....
  .custom /*0C000108:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 11 6F 6E 66 75 6C 6C 73 63 72 65 65 6E 65   // ...onfullscreene
                                                                                                                                                                                             72 72 6F 72 63 53 79 73 74 65 6D 2E 45 76 65 6E   // rrorcSystem.Even
                                                                                                                                                                                             74 41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52 75   // tArgs, System.Ru
                                                                                                                                                                                             6E 74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D 36   // ntime, Version=6
                                                                                                                                                                                             2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D   // .0.0.0, Culture=
                                                                                                                                                                                             6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B   // neutral, PublicK
                                                                                                                                                                                             65 79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37 66   // eyToken=b03f5f7f
                                                                                                                                                                                             31 31 64 35 30 61 33 61 01 01 00 00 )             // 11d50a3a....
  .custom /*0C000109:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 0C 6F 6E 6C 6F 61 64 65 64 64 61 74 61 63   // ...onloadeddatac
                                                                                                                                                                                             53 79 73 74 65 6D 2E 45 76 65 6E 74 41 72 67 73   // System.EventArgs
                                                                                                                                                                                             2C 20 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D 65   // , System.Runtime
                                                                                                                                                                                             2C 20 56 65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E   // , Version=6.0.0.
                                                                                                                                                                                             30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72   // 0, Culture=neutr
                                                                                                                                                                                             61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B   // al, PublicKeyTok
                                                                                                                                                                                             65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30   // en=b03f5f7f11d50
                                                                                                                                                                                             61 33 61 01 01 00 00 )                            // a3a....
  .custom /*0C00010A:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 10 6F 6E 6C 6F 61 64 65 64 6D 65 74 61 64   // ...onloadedmetad
                                                                                                                                                                                             61 74 61 63 53 79 73 74 65 6D 2E 45 76 65 6E 74   // atacSystem.Event
                                                                                                                                                                                             41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52 75 6E   // Args, System.Run
                                                                                                                                                                                             74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D 36 2E   // time, Version=6.
                                                                                                                                                                                             30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E   // 0.0.0, Culture=n
                                                                                                                                                                                             65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65   // eutral, PublicKe
                                                                                                                                                                                             79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37 66 31   // yToken=b03f5f7f1
                                                                                                                                                                                             31 64 35 30 61 33 61 01 01 00 00 )                // 1d50a3a....
  .custom /*0C00010B:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 13 6F 6E 70 6F 69 6E 74 65 72 6C 6F 63 6B   // ...onpointerlock
                                                                                                                                                                                             63 68 61 6E 67 65 63 53 79 73 74 65 6D 2E 45 76   // changecSystem.Ev
                                                                                                                                                                                             65 6E 74 41 72 67 73 2C 20 53 79 73 74 65 6D 2E   // entArgs, System.
                                                                                                                                                                                             52 75 6E 74 69 6D 65 2C 20 56 65 72 73 69 6F 6E   // Runtime, Version
                                                                                                                                                                                             3D 36 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72   // =6.0.0.0, Cultur
                                                                                                                                                                                             65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69   // e=neutral, Publi
                                                                                                                                                                                             63 4B 65 79 54 6F 6B 65 6E 3D 62 30 33 66 35 66   // cKeyToken=b03f5f
                                                                                                                                                                                             37 66 31 31 64 35 30 61 33 61 01 01 00 00 )       // 7f11d50a3a....
  .custom /*0C00010C:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 12 6F 6E 70 6F 69 6E 74 65 72 6C 6F 63 6B   // ...onpointerlock
                                                                                                                                                                                             65 72 72 6F 72 63 53 79 73 74 65 6D 2E 45 76 65   // errorcSystem.Eve
                                                                                                                                                                                             6E 74 41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52   // ntArgs, System.R
                                                                                                                                                                                             75 6E 74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D   // untime, Version=
                                                                                                                                                                                             36 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65   // 6.0.0.0, Culture
                                                                                                                                                                                             3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63   // =neutral, Public
                                                                                                                                                                                             4B 65 79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37   // KeyToken=b03f5f7
                                                                                                                                                                                             66 31 31 64 35 30 61 33 61 01 01 00 00 )          // f11d50a3a....
  .custom /*0C00010D:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 12 6F 6E 72 65 61 64 79 73 74 61 74 65 63   // ...onreadystatec
                                                                                                                                                                                             68 61 6E 67 65 63 53 79 73 74 65 6D 2E 45 76 65   // hangecSystem.Eve
                                                                                                                                                                                             6E 74 41 72 67 73 2C 20 53 79 73 74 65 6D 2E 52   // ntArgs, System.R
                                                                                                                                                                                             75 6E 74 69 6D 65 2C 20 56 65 72 73 69 6F 6E 3D   // untime, Version=
                                                                                                                                                                                             36 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65   // 6.0.0.0, Culture
                                                                                                                                                                                             3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63   // =neutral, Public
                                                                                                                                                                                             4B 65 79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37   // KeyToken=b03f5f7
                                                                                                                                                                                             66 31 31 64 35 30 61 33 61 01 01 00 00 )          // f11d50a3a....
  .custom /*0C00010E:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 08 6F 6E 73 63 72 6F 6C 6C 63 53 79 73 74   // ...onscrollcSyst
                                                                                                                                                                                             65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79   // em.EventArgs, Sy
                                                                                                                                                                                             73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65   // stem.Runtime, Ve
                                                                                                                                                                                             72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43   // rsion=6.0.0.0, C
                                                                                                                                                                                             75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20   // ulture=neutral, 
                                                                                                                                                                                             50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62   // PublicKeyToken=b
                                                                                                                                                                                             30 33 66 35 66 37 66 31 31 64 35 30 61 33 61 01   // 03f5f7f11d50a3a.
                                                                                                                                                                                             01 00 00 ) 
  .custom /*0C00010F:0A000020*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventHandlerAttribute/*01000042*/::.ctor(string,
                                                                                                                                                                    class [System.Runtime/*23000001*/]System.Type/*01000024*/,
                                                                                                                                                                    bool,
                                                                                                                                                                    bool) /* 0A000020 */ = ( 01 00 08 6F 6E 74 6F 67 67 6C 65 63 53 79 73 74   // ...ontogglecSyst
                                                                                                                                                                                             65 6D 2E 45 76 65 6E 74 41 72 67 73 2C 20 53 79   // em.EventArgs, Sy
                                                                                                                                                                                             73 74 65 6D 2E 52 75 6E 74 69 6D 65 2C 20 56 65   // stem.Runtime, Ve
                                                                                                                                                                                             72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20 43   // rsion=6.0.0.0, C
                                                                                                                                                                                             75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20   // ulture=neutral, 
                                                                                                                                                                                             50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62   // PublicKeyToken=b
                                                                                                                                                                                             30 33 66 35 66 37 66 31 31 64 35 30 61 33 61 01   // 03f5f7f11d50a3a.
                                                                                                                                                                                             01 00 00 ) 
} // end of class Microsoft.AspNetCore.Components.Web.EventHandlers

.class /*0200002A*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.FocusEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
{
  .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000112:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000081*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001E3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusEventArgs::get_Type

  .method /*06000082*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001E4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusEventArgs::set_Type

  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FocusEventArgs::.ctor

  .property /*1700002A*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.FocusEventArgs/*0200002A*/::get_Type() /* 06000081 */
    .set instance void Microsoft.AspNetCore.Components.Web.FocusEventArgs/*0200002A*/::set_Type(string) /* 06000082 */
  } // end of property FocusEventArgs::Type
} // end of class Microsoft.AspNetCore.Components.Web.FocusEventArgs

.class /*0200002B*/ interface public abstract auto ansi Microsoft.AspNetCore.Components.Web.IErrorBoundaryLogger
{
  .custom /*0C000114:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000084*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000018*/ 
          LogErrorAsync(class [System.Runtime/*23000001*/]System.Exception/*01000041*/ exception) cil managed
  // SIG: 20 01 11 61 12 81 05
  {
    // Method begins at RVA 0x0
  } // end of method IErrorBoundaryLogger::LogErrorAsync

} // end of class Microsoft.AspNetCore.Components.Web.IErrorBoundaryLogger

.class /*0200002C*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.KeyboardEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
{
  .custom /*0C000116:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000117:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000085*/ public hidebysig specialname 
          instance string  get_Key() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001E6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_Key

  .method /*06000086*/ public hidebysig specialname 
          instance void  set_Key(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001E8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_Key

  .method /*06000087*/ public hidebysig specialname 
          instance string  get_Code() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001EA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_Code

  .method /*06000088*/ public hidebysig specialname 
          instance void  set_Code(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001EB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_Code

  .method /*06000089*/ public hidebysig specialname 
          instance float32  get_Location() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C0001ED:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_Location

  .method /*0600008A*/ public hidebysig specialname 
          instance void  set_Location(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    .custom /*0C0001F0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_Location

  .method /*0600008B*/ public hidebysig specialname 
          instance bool  get_Repeat() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001F3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_Repeat

  .method /*0600008C*/ public hidebysig specialname 
          instance void  set_Repeat(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001F4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_Repeat

  .method /*0600008D*/ public hidebysig specialname 
          instance bool  get_CtrlKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001F5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_CtrlKey

  .method /*0600008E*/ public hidebysig specialname 
          instance void  set_CtrlKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001F6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_CtrlKey

  .method /*0600008F*/ public hidebysig specialname 
          instance bool  get_ShiftKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001F7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_ShiftKey

  .method /*06000090*/ public hidebysig specialname 
          instance void  set_ShiftKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001F8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_ShiftKey

  .method /*06000091*/ public hidebysig specialname 
          instance bool  get_AltKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001F9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_AltKey

  .method /*06000092*/ public hidebysig specialname 
          instance void  set_AltKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001FA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_AltKey

  .method /*06000093*/ public hidebysig specialname 
          instance bool  get_MetaKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001FC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_MetaKey

  .method /*06000094*/ public hidebysig specialname 
          instance void  set_MetaKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001FD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_MetaKey

  .method /*06000095*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001FE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::get_Type

  .method /*06000096*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001FF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::set_Type

  .method /*06000097*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyboardEventArgs::.ctor

  .property /*1700002B*/ instance callconv(8) string
          Key()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_Key() /* 06000085 */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_Key(string) /* 06000086 */
  } // end of property KeyboardEventArgs::Key
  .property /*1700002C*/ instance callconv(8) string
          Code()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_Code() /* 06000087 */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_Code(string) /* 06000088 */
  } // end of property KeyboardEventArgs::Code
  .property /*1700002D*/ instance callconv(8) float32
          Location()
  {
    .get instance float32 Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_Location() /* 06000089 */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_Location(float32) /* 0600008A */
  } // end of property KeyboardEventArgs::Location
  .property /*1700002E*/ instance callconv(8) bool
          Repeat()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_Repeat() /* 0600008B */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_Repeat(bool) /* 0600008C */
  } // end of property KeyboardEventArgs::Repeat
  .property /*1700002F*/ instance callconv(8) bool
          CtrlKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_CtrlKey() /* 0600008D */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_CtrlKey(bool) /* 0600008E */
  } // end of property KeyboardEventArgs::CtrlKey
  .property /*17000030*/ instance callconv(8) bool
          ShiftKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_ShiftKey() /* 0600008F */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_ShiftKey(bool) /* 06000090 */
  } // end of property KeyboardEventArgs::ShiftKey
  .property /*17000031*/ instance callconv(8) bool
          AltKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_AltKey() /* 06000091 */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_AltKey(bool) /* 06000092 */
  } // end of property KeyboardEventArgs::AltKey
  .property /*17000032*/ instance callconv(8) bool
          MetaKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_MetaKey() /* 06000093 */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_MetaKey(bool) /* 06000094 */
  } // end of property KeyboardEventArgs::MetaKey
  .property /*17000033*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::get_Type() /* 06000095 */
    .set instance void Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/::set_Type(string) /* 06000096 */
  } // end of property KeyboardEventArgs::Type
} // end of class Microsoft.AspNetCore.Components.Web.KeyboardEventArgs

.class /*0200002D*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.MouseEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
{
  .custom /*0C000119:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00011A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000098*/ public hidebysig specialname 
          instance int64  get_Detail() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000201:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_Detail

  .method /*06000099*/ public hidebysig specialname 
          instance void  set_Detail(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C000202:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_Detail

  .method /*0600009A*/ public hidebysig specialname 
          instance float64  get_ScreenX() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000203:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_ScreenX

  .method /*0600009B*/ public hidebysig specialname 
          instance void  set_ScreenX(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000204:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_ScreenX

  .method /*0600009C*/ public hidebysig specialname 
          instance float64  get_ScreenY() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000206:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_ScreenY

  .method /*0600009D*/ public hidebysig specialname 
          instance void  set_ScreenY(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000207:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_ScreenY

  .method /*0600009E*/ public hidebysig specialname 
          instance float64  get_ClientX() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000208:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_ClientX

  .method /*0600009F*/ public hidebysig specialname 
          instance void  set_ClientX(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000209:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_ClientX

  .method /*060000A0*/ public hidebysig specialname 
          instance float64  get_ClientY() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C00020B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_ClientY

  .method /*060000A1*/ public hidebysig specialname 
          instance void  set_ClientY(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C00020C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_ClientY

  .method /*060000A2*/ public hidebysig specialname 
          instance float64  get_OffsetX() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C00020D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_OffsetX

  .method /*060000A3*/ public hidebysig specialname 
          instance void  set_OffsetX(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C00020F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_OffsetX

  .method /*060000A4*/ public hidebysig specialname 
          instance float64  get_OffsetY() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000211:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_OffsetY

  .method /*060000A5*/ public hidebysig specialname 
          instance void  set_OffsetY(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000212:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_OffsetY

  .method /*060000A6*/ public hidebysig specialname 
          instance float64  get_PageX() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000215:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_PageX

  .method /*060000A7*/ public hidebysig specialname 
          instance void  set_PageX(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000217:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_PageX

  .method /*060000A8*/ public hidebysig specialname 
          instance float64  get_PageY() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C00021A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_PageY

  .method /*060000A9*/ public hidebysig specialname 
          instance void  set_PageY(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C00021B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_PageY

  .method /*060000AA*/ public hidebysig specialname 
          instance int64  get_Button() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C00021D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_Button

  .method /*060000AB*/ public hidebysig specialname 
          instance void  set_Button(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C00021E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_Button

  .method /*060000AC*/ public hidebysig specialname 
          instance int64  get_Buttons() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000221:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_Buttons

  .method /*060000AD*/ public hidebysig specialname 
          instance void  set_Buttons(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C000223:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_Buttons

  .method /*060000AE*/ public hidebysig specialname 
          instance bool  get_CtrlKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000225:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_CtrlKey

  .method /*060000AF*/ public hidebysig specialname 
          instance void  set_CtrlKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000227:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_CtrlKey

  .method /*060000B0*/ public hidebysig specialname 
          instance bool  get_ShiftKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000229:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_ShiftKey

  .method /*060000B1*/ public hidebysig specialname 
          instance void  set_ShiftKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00022A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_ShiftKey

  .method /*060000B2*/ public hidebysig specialname 
          instance bool  get_AltKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00022B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_AltKey

  .method /*060000B3*/ public hidebysig specialname 
          instance void  set_AltKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00022E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_AltKey

  .method /*060000B4*/ public hidebysig specialname 
          instance bool  get_MetaKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000232:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_MetaKey

  .method /*060000B5*/ public hidebysig specialname 
          instance void  set_MetaKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000234:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_MetaKey

  .method /*060000B6*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000237:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::get_Type

  .method /*060000B7*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000238:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::set_Type

  .method /*060000B8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MouseEventArgs::.ctor

  .property /*17000034*/ instance callconv(8) int64
          Detail()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_Detail() /* 06000098 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_Detail(int64) /* 06000099 */
  } // end of property MouseEventArgs::Detail
  .property /*17000035*/ instance callconv(8) float64
          ScreenX()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_ScreenX() /* 0600009A */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_ScreenX(float64) /* 0600009B */
  } // end of property MouseEventArgs::ScreenX
  .property /*17000036*/ instance callconv(8) float64
          ScreenY()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_ScreenY() /* 0600009C */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_ScreenY(float64) /* 0600009D */
  } // end of property MouseEventArgs::ScreenY
  .property /*17000037*/ instance callconv(8) float64
          ClientX()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_ClientX() /* 0600009E */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_ClientX(float64) /* 0600009F */
  } // end of property MouseEventArgs::ClientX
  .property /*17000038*/ instance callconv(8) float64
          ClientY()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_ClientY() /* 060000A0 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_ClientY(float64) /* 060000A1 */
  } // end of property MouseEventArgs::ClientY
  .property /*17000039*/ instance callconv(8) float64
          OffsetX()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_OffsetX() /* 060000A2 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_OffsetX(float64) /* 060000A3 */
  } // end of property MouseEventArgs::OffsetX
  .property /*1700003A*/ instance callconv(8) float64
          OffsetY()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_OffsetY() /* 060000A4 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_OffsetY(float64) /* 060000A5 */
  } // end of property MouseEventArgs::OffsetY
  .property /*1700003B*/ instance callconv(8) float64
          PageX()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_PageX() /* 060000A6 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_PageX(float64) /* 060000A7 */
  } // end of property MouseEventArgs::PageX
  .property /*1700003C*/ instance callconv(8) float64
          PageY()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_PageY() /* 060000A8 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_PageY(float64) /* 060000A9 */
  } // end of property MouseEventArgs::PageY
  .property /*1700003D*/ instance callconv(8) int64
          Button()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_Button() /* 060000AA */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_Button(int64) /* 060000AB */
  } // end of property MouseEventArgs::Button
  .property /*1700003E*/ instance callconv(8) int64
          Buttons()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_Buttons() /* 060000AC */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_Buttons(int64) /* 060000AD */
  } // end of property MouseEventArgs::Buttons
  .property /*1700003F*/ instance callconv(8) bool
          CtrlKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_CtrlKey() /* 060000AE */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_CtrlKey(bool) /* 060000AF */
  } // end of property MouseEventArgs::CtrlKey
  .property /*17000040*/ instance callconv(8) bool
          ShiftKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_ShiftKey() /* 060000B0 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_ShiftKey(bool) /* 060000B1 */
  } // end of property MouseEventArgs::ShiftKey
  .property /*17000041*/ instance callconv(8) bool
          AltKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_AltKey() /* 060000B2 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_AltKey(bool) /* 060000B3 */
  } // end of property MouseEventArgs::AltKey
  .property /*17000042*/ instance callconv(8) bool
          MetaKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_MetaKey() /* 060000B4 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_MetaKey(bool) /* 060000B5 */
  } // end of property MouseEventArgs::MetaKey
  .property /*17000043*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::get_Type() /* 060000B6 */
    .set instance void Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/::set_Type(string) /* 060000B7 */
  } // end of property MouseEventArgs::Type
} // end of class Microsoft.AspNetCore.Components.Web.MouseEventArgs

.class /*0200002E*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.PointerEventArgs
       extends Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/
{
  .custom /*0C00011C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00011D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000B9*/ public hidebysig specialname 
          instance int64  get_PointerId() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C00023A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::get_PointerId

  .method /*060000BA*/ public hidebysig specialname 
          instance void  set_PointerId(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C00023B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::set_PointerId

  .method /*060000BB*/ public hidebysig specialname 
          instance float32  get_Width() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C00023C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::get_Width

  .method /*060000BC*/ public hidebysig specialname 
          instance void  set_Width(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    .custom /*0C00023E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::set_Width

  .method /*060000BD*/ public hidebysig specialname 
          instance float32  get_Height() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C00023F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::get_Height

  .method /*060000BE*/ public hidebysig specialname 
          instance void  set_Height(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    .custom /*0C000240:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::set_Height

  .method /*060000BF*/ public hidebysig specialname 
          instance float32  get_Pressure() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000241:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::get_Pressure

  .method /*060000C0*/ public hidebysig specialname 
          instance void  set_Pressure(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    .custom /*0C000243:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::set_Pressure

  .method /*060000C1*/ public hidebysig specialname 
          instance float32  get_TiltX() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000244:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::get_TiltX

  .method /*060000C2*/ public hidebysig specialname 
          instance void  set_TiltX(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    .custom /*0C000245:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::set_TiltX

  .method /*060000C3*/ public hidebysig specialname 
          instance float32  get_TiltY() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000246:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::get_TiltY

  .method /*060000C4*/ public hidebysig specialname 
          instance void  set_TiltY(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    .custom /*0C000248:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::set_TiltY

  .method /*060000C5*/ public hidebysig specialname 
          instance string  get_PointerType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000249:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::get_PointerType

  .method /*060000C6*/ public hidebysig specialname 
          instance void  set_PointerType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00024A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::set_PointerType

  .method /*060000C7*/ public hidebysig specialname 
          instance bool  get_IsPrimary() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00024B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::get_IsPrimary

  .method /*060000C8*/ public hidebysig specialname 
          instance void  set_IsPrimary(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00024D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::set_IsPrimary

  .method /*060000C9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointerEventArgs::.ctor

  .property /*17000044*/ instance callconv(8) int64
          PointerId()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::get_PointerId() /* 060000B9 */
    .set instance void Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::set_PointerId(int64) /* 060000BA */
  } // end of property PointerEventArgs::PointerId
  .property /*17000045*/ instance callconv(8) float32
          Width()
  {
    .get instance float32 Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::get_Width() /* 060000BB */
    .set instance void Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::set_Width(float32) /* 060000BC */
  } // end of property PointerEventArgs::Width
  .property /*17000046*/ instance callconv(8) float32
          Height()
  {
    .get instance float32 Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::get_Height() /* 060000BD */
    .set instance void Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::set_Height(float32) /* 060000BE */
  } // end of property PointerEventArgs::Height
  .property /*17000047*/ instance callconv(8) float32
          Pressure()
  {
    .get instance float32 Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::get_Pressure() /* 060000BF */
    .set instance void Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::set_Pressure(float32) /* 060000C0 */
  } // end of property PointerEventArgs::Pressure
  .property /*17000048*/ instance callconv(8) float32
          TiltX()
  {
    .get instance float32 Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::get_TiltX() /* 060000C1 */
    .set instance void Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::set_TiltX(float32) /* 060000C2 */
  } // end of property PointerEventArgs::TiltX
  .property /*17000049*/ instance callconv(8) float32
          TiltY()
  {
    .get instance float32 Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::get_TiltY() /* 060000C3 */
    .set instance void Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::set_TiltY(float32) /* 060000C4 */
  } // end of property PointerEventArgs::TiltY
  .property /*1700004A*/ instance callconv(8) string
          PointerType()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::get_PointerType() /* 060000C5 */
    .set instance void Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::set_PointerType(string) /* 060000C6 */
  } // end of property PointerEventArgs::PointerType
  .property /*1700004B*/ instance callconv(8) bool
          IsPrimary()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::get_IsPrimary() /* 060000C7 */
    .set instance void Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/::set_IsPrimary(bool) /* 060000C8 */
  } // end of property PointerEventArgs::IsPrimary
} // end of class Microsoft.AspNetCore.Components.Web.PointerEventArgs

.class /*0200002F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.ProgressEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
{
  .custom /*0C00011F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000120:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000CA*/ public hidebysig specialname 
          instance bool  get_LengthComputable() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00024E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::get_LengthComputable

  .method /*060000CB*/ public hidebysig specialname 
          instance void  set_LengthComputable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00024F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::set_LengthComputable

  .method /*060000CC*/ public hidebysig specialname 
          instance int64  get_Loaded() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000251:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::get_Loaded

  .method /*060000CD*/ public hidebysig specialname 
          instance void  set_Loaded(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C000252:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::set_Loaded

  .method /*060000CE*/ public hidebysig specialname 
          instance int64  get_Total() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000253:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::get_Total

  .method /*060000CF*/ public hidebysig specialname 
          instance void  set_Total(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C000254:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::set_Total

  .method /*060000D0*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000256:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::get_Type

  .method /*060000D1*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000257:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::set_Type

  .method /*060000D2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgressEventArgs::.ctor

  .property /*1700004C*/ instance callconv(8) bool
          LengthComputable()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/::get_LengthComputable() /* 060000CA */
    .set instance void Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/::set_LengthComputable(bool) /* 060000CB */
  } // end of property ProgressEventArgs::LengthComputable
  .property /*1700004D*/ instance callconv(8) int64
          Loaded()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/::get_Loaded() /* 060000CC */
    .set instance void Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/::set_Loaded(int64) /* 060000CD */
  } // end of property ProgressEventArgs::Loaded
  .property /*1700004E*/ instance callconv(8) int64
          Total()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/::get_Total() /* 060000CE */
    .set instance void Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/::set_Total(int64) /* 060000CF */
  } // end of property ProgressEventArgs::Total
  .property /*1700004F*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/::get_Type() /* 060000D0 */
    .set instance void Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/::set_Type(string) /* 060000D1 */
  } // end of property ProgressEventArgs::Type
} // end of class Microsoft.AspNetCore.Components.Web.ProgressEventArgs

.class /*02000030*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.TouchEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
{
  .custom /*0C000122:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000123:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000D3*/ public hidebysig specialname 
          instance int64  get_Detail() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000258:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_Detail

  .method /*060000D4*/ public hidebysig specialname 
          instance void  set_Detail(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C00025A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_Detail

  .method /*060000D5*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] 
          get_Touches() cil managed
  // SIG: 20 00 1D 12 80 C4
  {
    .custom /*0C00025B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_Touches

  .method /*060000D6*/ public hidebysig specialname 
          instance void  set_Touches(class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 80 C4
  {
    .custom /*0C00025C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_Touches

  .method /*060000D7*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] 
          get_TargetTouches() cil managed
  // SIG: 20 00 1D 12 80 C4
  {
    .custom /*0C00025D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_TargetTouches

  .method /*060000D8*/ public hidebysig specialname 
          instance void  set_TargetTouches(class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 80 C4
  {
    .custom /*0C00025F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_TargetTouches

  .method /*060000D9*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] 
          get_ChangedTouches() cil managed
  // SIG: 20 00 1D 12 80 C4
  {
    .custom /*0C000260:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_ChangedTouches

  .method /*060000DA*/ public hidebysig specialname 
          instance void  set_ChangedTouches(class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 80 C4
  {
    .custom /*0C000261:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_ChangedTouches

  .method /*060000DB*/ public hidebysig specialname 
          instance bool  get_CtrlKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000262:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_CtrlKey

  .method /*060000DC*/ public hidebysig specialname 
          instance void  set_CtrlKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000264:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_CtrlKey

  .method /*060000DD*/ public hidebysig specialname 
          instance bool  get_ShiftKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000265:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_ShiftKey

  .method /*060000DE*/ public hidebysig specialname 
          instance void  set_ShiftKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000266:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_ShiftKey

  .method /*060000DF*/ public hidebysig specialname 
          instance bool  get_AltKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000267:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_AltKey

  .method /*060000E0*/ public hidebysig specialname 
          instance void  set_AltKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000269:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_AltKey

  .method /*060000E1*/ public hidebysig specialname 
          instance bool  get_MetaKey() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00026A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_MetaKey

  .method /*060000E2*/ public hidebysig specialname 
          instance void  set_MetaKey(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00026B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_MetaKey

  .method /*060000E3*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00026C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::get_Type

  .method /*060000E4*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00026E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::set_Type

  .method /*060000E5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchEventArgs::.ctor

  .property /*17000050*/ instance callconv(8) int64
          Detail()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_Detail() /* 060000D3 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_Detail(int64) /* 060000D4 */
  } // end of property TouchEventArgs::Detail
  .property /*17000051*/ instance callconv(8) class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[]
          Touches()
  {
    .get instance class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_Touches() /* 060000D5 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_Touches(class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[]) /* 060000D6 */
  } // end of property TouchEventArgs::Touches
  .property /*17000052*/ instance callconv(8) class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[]
          TargetTouches()
  {
    .get instance class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_TargetTouches() /* 060000D7 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_TargetTouches(class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[]) /* 060000D8 */
  } // end of property TouchEventArgs::TargetTouches
  .property /*17000053*/ instance callconv(8) class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[]
          ChangedTouches()
  {
    .get instance class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[] Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_ChangedTouches() /* 060000D9 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_ChangedTouches(class Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/[]) /* 060000DA */
  } // end of property TouchEventArgs::ChangedTouches
  .property /*17000054*/ instance callconv(8) bool
          CtrlKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_CtrlKey() /* 060000DB */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_CtrlKey(bool) /* 060000DC */
  } // end of property TouchEventArgs::CtrlKey
  .property /*17000055*/ instance callconv(8) bool
          ShiftKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_ShiftKey() /* 060000DD */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_ShiftKey(bool) /* 060000DE */
  } // end of property TouchEventArgs::ShiftKey
  .property /*17000056*/ instance callconv(8) bool
          AltKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_AltKey() /* 060000DF */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_AltKey(bool) /* 060000E0 */
  } // end of property TouchEventArgs::AltKey
  .property /*17000057*/ instance callconv(8) bool
          MetaKey()
  {
    .get instance bool Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_MetaKey() /* 060000E1 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_MetaKey(bool) /* 060000E2 */
  } // end of property TouchEventArgs::MetaKey
  .property /*17000058*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::get_Type() /* 060000E3 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/::set_Type(string) /* 060000E4 */
  } // end of property TouchEventArgs::Type
} // end of class Microsoft.AspNetCore.Components.Web.TouchEventArgs

.class /*02000031*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.TouchPoint
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*060000E6*/ public hidebysig specialname 
          instance int64  get_Identifier() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C00026F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::get_Identifier

  .method /*060000E7*/ public hidebysig specialname 
          instance void  set_Identifier(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C000270:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::set_Identifier

  .method /*060000E8*/ public hidebysig specialname 
          instance float64  get_ScreenX() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000271:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::get_ScreenX

  .method /*060000E9*/ public hidebysig specialname 
          instance void  set_ScreenX(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000272:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::set_ScreenX

  .method /*060000EA*/ public hidebysig specialname 
          instance float64  get_ScreenY() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000273:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::get_ScreenY

  .method /*060000EB*/ public hidebysig specialname 
          instance void  set_ScreenY(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000274:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::set_ScreenY

  .method /*060000EC*/ public hidebysig specialname 
          instance float64  get_ClientX() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000275:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::get_ClientX

  .method /*060000ED*/ public hidebysig specialname 
          instance void  set_ClientX(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000276:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::set_ClientX

  .method /*060000EE*/ public hidebysig specialname 
          instance float64  get_ClientY() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000277:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::get_ClientY

  .method /*060000EF*/ public hidebysig specialname 
          instance void  set_ClientY(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C000278:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::set_ClientY

  .method /*060000F0*/ public hidebysig specialname 
          instance float64  get_PageX() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C000279:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::get_PageX

  .method /*060000F1*/ public hidebysig specialname 
          instance void  set_PageX(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C00027A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::set_PageX

  .method /*060000F2*/ public hidebysig specialname 
          instance float64  get_PageY() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C00027B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::get_PageY

  .method /*060000F3*/ public hidebysig specialname 
          instance void  set_PageY(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C00027C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::set_PageY

  .method /*060000F4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TouchPoint::.ctor

  .property /*17000059*/ instance callconv(8) int64
          Identifier()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::get_Identifier() /* 060000E6 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::set_Identifier(int64) /* 060000E7 */
  } // end of property TouchPoint::Identifier
  .property /*1700005A*/ instance callconv(8) float64
          ScreenX()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::get_ScreenX() /* 060000E8 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::set_ScreenX(float64) /* 060000E9 */
  } // end of property TouchPoint::ScreenX
  .property /*1700005B*/ instance callconv(8) float64
          ScreenY()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::get_ScreenY() /* 060000EA */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::set_ScreenY(float64) /* 060000EB */
  } // end of property TouchPoint::ScreenY
  .property /*1700005C*/ instance callconv(8) float64
          ClientX()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::get_ClientX() /* 060000EC */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::set_ClientX(float64) /* 060000ED */
  } // end of property TouchPoint::ClientX
  .property /*1700005D*/ instance callconv(8) float64
          ClientY()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::get_ClientY() /* 060000EE */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::set_ClientY(float64) /* 060000EF */
  } // end of property TouchPoint::ClientY
  .property /*1700005E*/ instance callconv(8) float64
          PageX()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::get_PageX() /* 060000F0 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::set_PageX(float64) /* 060000F1 */
  } // end of property TouchPoint::PageX
  .property /*1700005F*/ instance callconv(8) float64
          PageY()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::get_PageY() /* 060000F2 */
    .set instance void Microsoft.AspNetCore.Components.Web.TouchPoint/*02000031*/::set_PageY(float64) /* 060000F3 */
  } // end of property TouchPoint::PageY
} // end of class Microsoft.AspNetCore.Components.Web.TouchPoint

.class /*02000032*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.WebEventCallbackFactoryEventArgsExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000124:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000125:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000126:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000F5*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.ClipboardEventArgs/*02000023*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.ClipboardEventArgs/*02000023*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 8C 12 81 15 1C 15 12 81 19 01 12 80 8C
  {
    .custom /*0C00027D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00027E:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*08000097*/ 
    .custom /*0C000200:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000F6*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.ClipboardEventArgs/*02000023*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.ClipboardEventArgs/*02000023*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 8C 12 81 15 1C 15 12 81 1D 02 12 80 8C 12 80 9D
  {
    .custom /*0C000280:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000281:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*0800009B*/ 
    .custom /*0C000205:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000F7*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.DragEventArgs/*02000026*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.DragEventArgs/*02000026*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 98 12 81 15 1C 15 12 81 19 01 12 80 98
  {
    .custom /*0C000282:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000283:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*0800009F*/ 
    .custom /*0C00020A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000F8*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.DragEventArgs/*02000026*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.DragEventArgs/*02000026*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 98 12 81 15 1C 15 12 81 1D 02 12 80 98 12 80 9D
  {
    .custom /*0C000284:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000285:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000A3*/ 
    .custom /*0C000210:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000F9*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 A0 12 81 15 1C 15 12 81 19 01 12 80 A0
  {
    .custom /*0C000286:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000287:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000A7*/ 
    .custom /*0C000218:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000FA*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.ErrorEventArgs/*02000028*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 A0 12 81 15 1C 15 12 81 1D 02 12 80 A0 12 80 9D
  {
    .custom /*0C000288:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000289:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000AB*/ 
    .custom /*0C00021F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000FB*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.FocusEventArgs/*0200002A*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.FocusEventArgs/*0200002A*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 A8 12 81 15 1C 15 12 81 19 01 12 80 A8
  {
    .custom /*0C00028B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00028C:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000AF*/ 
    .custom /*0C000228:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000FC*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.FocusEventArgs/*0200002A*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.FocusEventArgs/*0200002A*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 A8 12 81 15 1C 15 12 81 1D 02 12 80 A8 12 80 9D
  {
    .custom /*0C00028D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00028E:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000B3*/ 
    .custom /*0C00022F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000FD*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 B0 12 81 15 1C 15 12 81 19 01 12 80 B0
  {
    .custom /*0C00028F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000290:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000B7*/ 
    .custom /*0C000239:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000FE*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.KeyboardEventArgs/*0200002C*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 B0 12 81 15 1C 15 12 81 1D 02 12 80 B0 12 80 9D
  {
    .custom /*0C000291:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000292:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000BB*/ 
    .custom /*0C00023D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*060000FF*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 B4 12 81 15 1C 15 12 81 19 01 12 80 B4
  {
    .custom /*0C000293:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000294:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000BF*/ 
    .custom /*0C000242:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000100*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 B4 12 81 15 1C 15 12 81 1D 02 12 80 B4 12 80 9D
  {
    .custom /*0C000295:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000296:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000C3*/ 
    .custom /*0C000247:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000101*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 B8 12 81 15 1C 15 12 81 19 01 12 80 B8
  {
    .custom /*0C000297:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000298:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000C7*/ 
    .custom /*0C00024C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000102*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.PointerEventArgs/*0200002E*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 B8 12 81 15 1C 15 12 81 1D 02 12 80 B8 12 80 9D
  {
    .custom /*0C000299:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00029A:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000CB*/ 
    .custom /*0C000250:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000103*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 BC 12 81 15 1C 15 12 81 19 01 12 80 BC
  {
    .custom /*0C00029B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00029C:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000CF*/ 
    .custom /*0C000255:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000104*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.ProgressEventArgs/*0200002F*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 BC 12 81 15 1C 15 12 81 1D 02 12 80 BC 12 80 9D
  {
    .custom /*0C00029D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00029E:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000D3*/ 
    .custom /*0C000259:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000105*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 C0 12 81 15 1C 15 12 81 19 01 12 80 C0
  {
    .custom /*0C00029F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0002A0:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000D7*/ 
    .custom /*0C00025E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000106*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.TouchEventArgs/*02000030*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 C0 12 81 15 1C 15 12 81 1D 02 12 80 C0 12 80 9D
  {
    .custom /*0C0002A1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0002A2:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000DB*/ 
    .custom /*0C000263:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000107*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<class Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 D0 12 81 15 1C 15 12 81 19 01 12 80 D0
  {
    .custom /*0C0002A3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0002A4:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000DF*/ 
    .custom /*0C000268:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

  .method /*06000108*/ public hidebysig static 
          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/> 
          Create(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallbackFactory/*01000045*/ factory,
                 object receiver,
                 class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/> callback) cil managed
  // SIG: 00 03 15 11 81 11 01 12 80 D0 12 81 15 1C 15 12 81 1D 02 12 80 D0 12 80 9D
  {
    .custom /*0C0002A5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0002A6:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 6C 54 68 69 73 20 65 78 74 65 6E 73 69 6F   // ..lThis extensio
                                                                                                                                                   6E 20 6D 65 74 68 6F 64 20 69 73 20 6F 62 73 6F   // n method is obso
                                                                                                                                                   6C 65 74 65 20 61 6E 64 20 77 69 6C 6C 20 62 65   // lete and will be
                                                                                                                                                   20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20 66 75   //  removed in a fu
                                                                                                                                                   74 75 72 65 20 76 65 72 73 69 6F 6E 2E 20 55 73   // ture version. Us
                                                                                                                                                   65 20 74 68 65 20 67 65 6E 65 72 69 63 20 6F 76   // e the generic ov
                                                                                                                                                   65 72 6C 6F 61 64 20 69 6E 73 74 65 61 64 2E 00   // erload instead..
                                                                                                                                                   00 ) 
    .param [0]/*080000E3*/ 
    .custom /*0C00026D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEventCallbackFactoryEventArgsExtensions::Create

} // end of class Microsoft.AspNetCore.Components.Web.WebEventCallbackFactoryEventArgsExtensions

.class /*02000033*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.WebRenderTreeBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000129:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00012A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00012B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000109*/ public hidebysig static 
          void  AddEventPreventDefaultAttribute(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder,
                                                int32 sequence,
                                                string eventName,
                                                bool 'value') cil managed
  // SIG: 00 04 01 12 80 D5 08 0E 02
  {
    .custom /*0C0002A7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRenderTreeBuilderExtensions::AddEventPreventDefaultAttribute

  .method /*0600010A*/ public hidebysig static 
          void  AddEventStopPropagationAttribute(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder,
                                                 int32 sequence,
                                                 string eventName,
                                                 bool 'value') cil managed
  // SIG: 00 04 01 12 80 D5 08 0E 02
  {
    .custom /*0C0002A9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRenderTreeBuilderExtensions::AddEventStopPropagationAttribute

} // end of class Microsoft.AspNetCore.Components.Web.WebRenderTreeBuilderExtensions

.class /*02000034*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.WheelEventArgs
       extends Microsoft.AspNetCore.Components.Web.MouseEventArgs/*0200002D*/
{
  .method /*0600010B*/ public hidebysig specialname 
          instance float64  get_DeltaX() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C0002AB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::get_DeltaX

  .method /*0600010C*/ public hidebysig specialname 
          instance void  set_DeltaX(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C0002AD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::set_DeltaX

  .method /*0600010D*/ public hidebysig specialname 
          instance float64  get_DeltaY() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C0002AF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::get_DeltaY

  .method /*0600010E*/ public hidebysig specialname 
          instance void  set_DeltaY(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C0002B0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::set_DeltaY

  .method /*0600010F*/ public hidebysig specialname 
          instance float64  get_DeltaZ() cil managed
  // SIG: 20 00 0D
  {
    .custom /*0C0002B1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::get_DeltaZ

  .method /*06000110*/ public hidebysig specialname 
          instance void  set_DeltaZ(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    .custom /*0C0002B3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::set_DeltaZ

  .method /*06000111*/ public hidebysig specialname 
          instance int64  get_DeltaMode() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C0002B5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::get_DeltaMode

  .method /*06000112*/ public hidebysig specialname 
          instance void  set_DeltaMode(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C0002B7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::set_DeltaMode

  .method /*06000113*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WheelEventArgs::.ctor

  .property /*17000060*/ instance callconv(8) float64
          DeltaX()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/::get_DeltaX() /* 0600010B */
    .set instance void Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/::set_DeltaX(float64) /* 0600010C */
  } // end of property WheelEventArgs::DeltaX
  .property /*17000061*/ instance callconv(8) float64
          DeltaY()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/::get_DeltaY() /* 0600010D */
    .set instance void Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/::set_DeltaY(float64) /* 0600010E */
  } // end of property WheelEventArgs::DeltaY
  .property /*17000062*/ instance callconv(8) float64
          DeltaZ()
  {
    .get instance float64 Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/::get_DeltaZ() /* 0600010F */
    .set instance void Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/::set_DeltaZ(float64) /* 06000110 */
  } // end of property WheelEventArgs::DeltaZ
  .property /*17000063*/ instance callconv(8) int64
          DeltaMode()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/::get_DeltaMode() /* 06000111 */
    .set instance void Microsoft.AspNetCore.Components.Web.WheelEventArgs/*02000034*/::set_DeltaMode(int64) /* 06000112 */
  } // end of property WheelEventArgs::DeltaMode
} // end of class Microsoft.AspNetCore.Components.Web.WheelEventArgs

.class /*02000035*/ public auto ansi sealed Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderDelegate`1<TItem>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000048*/
{
  .param type TItem /*2A000001*/ 
    .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000114*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ItemsProviderDelegate`1::.ctor

  .method /*06000115*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000049*/<valuetype Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult`1/*02000037*/<!TItem>> 
          Invoke(valuetype Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest/*02000036*/ 'request') runtime managed
  // SIG: 20 01 15 11 81 25 01 15 11 80 DC 01 13 00 11 80 D8
  {
    .param [0]/*080000F5*/ 
    .custom /*0C00027F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 00 01 00 00 ) 
  } // end of method ItemsProviderDelegate`1::Invoke

  .method /*06000116*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100004A*/ 
          BeginInvoke(valuetype Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest/*02000036*/ 'request',
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100004B*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 81 29 11 80 D8 12 81 2D 1C
  {
  } // end of method ItemsProviderDelegate`1::BeginInvoke

  .method /*06000117*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000049*/<valuetype Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult`1/*02000037*/<!TItem>> 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100004A*/ result) runtime managed
  // SIG: 20 01 15 11 81 25 01 15 11 80 DC 01 13 00 12 81 29
  {
    .param [0]/*080000FA*/ 
    .custom /*0C00028A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 00 01 00 00 ) 
  } // end of method ItemsProviderDelegate`1::EndInvoke

} // end of class Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderDelegate`1

.class /*02000036*/ public sequential ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
{
  .custom /*0C00012E:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003B*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
  .field /*0400000B*/ private initonly int32 '<StartIndex>k__BackingField'
  .custom /*0C000048:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000C*/ private initonly int32 '<Count>k__BackingField'
  .custom /*0C00004E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000D*/ private initonly valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ '<CancellationToken>k__BackingField'
  .custom /*0C000054:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000118*/ public hidebysig specialname 
          instance int32  get_StartIndex() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0002B9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ItemsProviderRequest::get_StartIndex

  .method /*06000119*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0002BA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ItemsProviderRequest::get_Count

  .method /*0600011A*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ 
          get_CancellationToken() cil managed
  // SIG: 20 00 11 81 31
  {
    .custom /*0C0002BB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ItemsProviderRequest::get_CancellationToken

  .method /*0600011B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 startIndex,
                               int32 count,
                               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken) cil managed
  // SIG: 20 03 01 08 08 11 81 31
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ItemsProviderRequest::.ctor

  .property /*17000064*/ instance callconv(8) int32
          StartIndex()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest/*02000036*/::get_StartIndex() /* 06000118 */
  } // end of property ItemsProviderRequest::StartIndex
  .property /*17000065*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest/*02000036*/::get_Count() /* 06000119 */
  } // end of property ItemsProviderRequest::Count
  .property /*17000066*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/
          CancellationToken()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest/*02000036*/::get_CancellationToken() /* 0600011A */
  } // end of property ItemsProviderRequest::CancellationToken
} // end of class Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest

.class /*02000037*/ public sequential ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult`1<TItem>
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
{
  .custom /*0C000130:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000131:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000132:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003B*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
  .param type TItem /*2A000002*/ 
    .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .field /*0400000E*/ private initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004D*/<!TItem> '<Items>k__BackingField'
  .custom /*0C00005A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000F*/ private initonly int32 '<TotalItemCount>k__BackingField'
  .custom /*0C00005F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*0600011C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004D*/<!TItem> 
          get_Items() cil managed
  // SIG: 20 00 15 12 81 35 01 13 00
  {
    .custom /*0C0002BC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ItemsProviderResult`1::get_Items

  .method /*0600011D*/ public hidebysig specialname 
          instance int32  get_TotalItemCount() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0002BD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ItemsProviderResult`1::get_TotalItemCount

  .method /*0600011E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004D*/<!TItem> items,
                               int32 totalItemCount) cil managed
  // SIG: 20 02 01 15 12 81 35 01 13 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ItemsProviderResult`1::.ctor

  .property /*17000067*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004D*/<!TItem>
          Items()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004D*/<!TItem> Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult`1/*02000037*/::get_Items() /* 0600011C */
  } // end of property ItemsProviderResult`1::Items
  .property /*17000068*/ instance callconv(8) int32
          TotalItemCount()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult`1/*02000037*/::get_TotalItemCount() /* 0600011D */
  } // end of property ItemsProviderResult`1::TotalItemCount
} // end of class Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult`1

.class /*02000038*/ interface private abstract auto ansi Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks
{
  .method /*0600011F*/ public hidebysig newslot abstract virtual 
          instance void  OnBeforeSpacerVisible(float32 spacerSize,
                                               float32 spacerSeparation,
                                               float32 containerSize) cil managed
  // SIG: 20 03 01 0C 0C 0C
  {
    // Method begins at RVA 0x0
  } // end of method IVirtualizeJsCallbacks::OnBeforeSpacerVisible

  .method /*06000120*/ public hidebysig newslot abstract virtual 
          instance void  OnAfterSpacerVisible(float32 spacerSize,
                                              float32 spacerSeparation,
                                              float32 containerSize) cil managed
  // SIG: 20 03 01 0C 0C 0C
  {
    // Method begins at RVA 0x0
  } // end of method IVirtualizeJsCallbacks::OnAfterSpacerVisible

} // end of class Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks

.class /*02000039*/ public sequential ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
{
  .custom /*0C000134:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003B*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
  .field /*04000010*/ private initonly int32 '<Index>k__BackingField'
  .custom /*0C000066:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000011*/ private initonly float32 '<Size>k__BackingField'
  .custom /*0C00006A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000121*/ public hidebysig specialname 
          instance int32  get_Index() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0002BE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PlaceholderContext::get_Index

  .method /*06000122*/ public hidebysig specialname 
          instance float32  get_Size() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C0002BF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PlaceholderContext::get_Size

  .method /*06000123*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 index,
                               [opt] float32 size) cil managed
  // SIG: 20 02 01 08 0C
  {
    .param [2]/*08000108*/  = float32(0.0000000)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PlaceholderContext::.ctor

  .property /*17000069*/ instance callconv(8) int32
          Index()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext/*02000039*/::get_Index() /* 06000121 */
  } // end of property PlaceholderContext::Index
  .property /*1700006A*/ instance callconv(8) float32
          Size()
  {
    .get instance float32 Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext/*02000039*/::get_Size() /* 06000122 */
  } // end of property PlaceholderContext::Size
} // end of class Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext

.class /*0200003A*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1<TItem>
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
       implements Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks/*02000038*/,
                  [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100004E*/
{
  .custom /*0C000135:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000136:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks/*02000038*/
  .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100004E*/
  .custom /*0C000021:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type TItem /*2A000003*/ 
    .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .class /*0200005F*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__60'<TItem>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C000191:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type TItem /*2A00000D*/ 
      .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .field /*0400002B*/ public int32 '<>1__state'
    .field /*0400002C*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000051*/ '<>t__builder'
    .field /*0400002D*/ public class Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/<!TItem> '<>4__this'
    .field /*0400002E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*0100002F*/ '<>u__1'
    .method /*06000220*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__60'::MoveNext

    .method /*06000221*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003A9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__60'::SetStateMachine

  } // end of class '<DisposeAsync>d__60'

  .class /*02000060*/ auto ansi sealed nested private beforefieldinit '<OnAfterRenderAsync>d__50'<TItem>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C000193:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type TItem /*2A00000E*/ 
      .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .field /*0400002F*/ public int32 '<>1__state'
    .field /*04000030*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000031*/ public bool firstRender
    .field /*04000032*/ public class Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/<!TItem> '<>4__this'
    .field /*04000033*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*0100002F*/ '<>u__1'
    .method /*06000222*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnAfterRenderAsync>d__50'::MoveNext

    .method /*06000223*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003AA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnAfterRenderAsync>d__50'::SetStateMachine

  } // end of class '<OnAfterRenderAsync>d__50'

  .class /*02000061*/ auto ansi sealed nested private beforefieldinit '<RefreshDataAsync>d__48'<TItem>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C000195:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type TItem /*2A00000F*/ 
      .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .field /*04000034*/ public int32 '<>1__state'
    .field /*04000035*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000036*/ public class Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/<!TItem> '<>4__this'
    .field /*04000037*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*0100002F*/ '<>u__1'
    .method /*06000224*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RefreshDataAsync>d__48'::MoveNext

    .method /*06000225*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003AB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RefreshDataAsync>d__48'::SetStateMachine

  } // end of class '<RefreshDataAsync>d__48'

  .class /*02000062*/ auto ansi sealed nested private beforefieldinit '<RefreshDataCoreAsync>d__57'<TItem>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C000198:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type TItem /*2A000010*/ 
      .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .field /*04000038*/ public int32 '<>1__state'
    .field /*04000039*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000051*/ '<>t__builder'
    .field /*0400003A*/ public class Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/<!TItem> '<>4__this'
    .field /*0400003B*/ public bool renderOnSuccess
    .field /*0400003C*/ private valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ '<cancellationToken>5__2'
    .field /*0400003D*/ private valuetype Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest/*02000036*/ '<request>5__3'
    .field /*0400003E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000037*/<valuetype Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult`1/*02000037*/<!TItem>> '<>u__1'
    .method /*06000226*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RefreshDataCoreAsync>d__57'::MoveNext

    .method /*06000227*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003AC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RefreshDataCoreAsync>d__57'::SetStateMachine

  } // end of class '<RefreshDataCoreAsync>d__57'

  .method /*06000124*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem> 
          get_ChildContent() cil managed
  // SIG: 20 00 15 12 81 3D 01 13 00
  {
    .custom /*0C0002C0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000109*/ 
    .custom /*0C0002A8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::get_ChildContent

  .method /*06000125*/ public hidebysig specialname 
          instance void  set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem> 'value') cil managed
  // SIG: 20 01 01 15 12 81 3D 01 13 00
  {
    .custom /*0C0002C1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800010A*/ 
    .custom /*0C0002AA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::set_ChildContent

  .method /*06000126*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem> 
          get_ItemContent() cil managed
  // SIG: 20 00 15 12 81 3D 01 13 00
  {
    .custom /*0C0002C2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800010B*/ 
    .custom /*0C0002AC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::get_ItemContent

  .method /*06000127*/ public hidebysig specialname 
          instance void  set_ItemContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem> 'value') cil managed
  // SIG: 20 01 01 15 12 81 3D 01 13 00
  {
    .custom /*0C0002C3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800010C*/ 
    .custom /*0C0002AE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::set_ItemContent

  .method /*06000128*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<valuetype Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext/*02000039*/> 
          get_Placeholder() cil managed
  // SIG: 20 00 15 12 81 3D 01 11 80 E4
  {
    .custom /*0C0002C4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0002C5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::get_Placeholder

  .method /*06000129*/ public hidebysig specialname 
          instance void  set_Placeholder(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<valuetype Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext/*02000039*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 3D 01 11 80 E4
  {
    .custom /*0C0002C6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0002C7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::set_Placeholder

  .method /*0600012A*/ public hidebysig specialname 
          instance float32  get_ItemSize() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C0002C8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::get_ItemSize

  .method /*0600012B*/ public hidebysig specialname 
          instance void  set_ItemSize(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    .custom /*0C0002C9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::set_ItemSize

  .method /*0600012C*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderDelegate`1/*02000035*/<!TItem> 
          get_ItemsProvider() cil managed
  // SIG: 20 00 15 12 80 D4 01 13 00
  {
    .custom /*0C0002CA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800010F*/ 
    .custom /*0C0002B2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::get_ItemsProvider

  .method /*0600012D*/ public hidebysig specialname 
          instance void  set_ItemsProvider(class Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderDelegate`1/*02000035*/<!TItem> 'value') cil managed
  // SIG: 20 01 01 15 12 80 D4 01 13 00
  {
    .custom /*0C0002CB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000110*/ 
    .custom /*0C0002B4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::set_ItemsProvider

  .method /*0600012E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000050*/<!TItem> 
          get_Items() cil managed
  // SIG: 20 00 15 12 81 41 01 13 00
  {
    .custom /*0C0002CC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000111*/ 
    .custom /*0C0002B6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::get_Items

  .method /*0600012F*/ public hidebysig specialname 
          instance void  set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000050*/<!TItem> 'value') cil managed
  // SIG: 20 01 01 15 12 81 41 01 13 00
  {
    .custom /*0C0002CD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000112*/ 
    .custom /*0C0002B8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::set_Items

  .method /*06000130*/ public hidebysig specialname 
          instance int32  get_OverscanCount() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0002CE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::get_OverscanCount

  .method /*06000131*/ public hidebysig specialname 
          instance void  set_OverscanCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0002CF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::set_OverscanCount

  .method /*06000132*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          RefreshDataAsync() cil managed
  // SIG: 20 00 12 80 9D
  {
    .custom /*0C0002D0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 57 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..WMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 57 65 62 2E 56 69 72 74 75 61 6C 69 7A   // ts.Web.Virtualiz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 56 69 72 74 75 61 6C 69 7A 65   // ation.Virtualize
                                                                                                                                                                                                                                        60 31 2B 3C 52 65 66 72 65 73 68 44 61 74 61 41   // `1+<RefreshDataA
                                                                                                                                                                                                                                        73 79 6E 63 3E 64 5F 5F 34 38 00 00 )             // sync>d__48..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::RefreshDataAsync

  .method /*06000133*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::OnParametersSet

  .method /*06000134*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          OnAfterRenderAsync(bool firstRender) cil managed
  // SIG: 20 01 12 80 9D 02
  {
    .custom /*0C0002D1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 59 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..YMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 57 65 62 2E 56 69 72 74 75 61 6C 69 7A   // ts.Web.Virtualiz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 56 69 72 74 75 61 6C 69 7A 65   // ation.Virtualize
                                                                                                                                                                                                                                        60 31 2B 3C 4F 6E 41 66 74 65 72 52 65 6E 64 65   // `1+<OnAfterRende
                                                                                                                                                                                                                                        72 41 73 79 6E 63 3E 64 5F 5F 35 30 00 00 )       // rAsync>d__50..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::OnAfterRenderAsync

  .method /*06000135*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::BuildRenderTree

  .method /*06000136*/ private hidebysig newslot virtual final 
          instance void  Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks.OnBeforeSpacerVisible(float32 spacerSize,
                                                                                                                         float32 spacerSeparation,
                                                                                                                         float32 containerSize) cil managed
  // SIG: 20 03 01 0C 0C 0C
  {
    .override Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks/*02000038*/::OnBeforeSpacerVisible /*02000038::0600011F*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks.OnBeforeSpacerVisible

  .method /*06000137*/ private hidebysig newslot virtual final 
          instance void  Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks.OnAfterSpacerVisible(float32 spacerSize,
                                                                                                                        float32 spacerSeparation,
                                                                                                                        float32 containerSize) cil managed
  // SIG: 20 03 01 0C 0C 0C
  {
    .override Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks/*02000038*/::OnAfterSpacerVisible /*02000038::06000120*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks.OnAfterSpacerVisible

  .method /*06000138*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000018*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 61
  {
    .custom /*0C0002D4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 53 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..SMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 57 65 62 2E 56 69 72 74 75 61 6C 69 7A   // ts.Web.Virtualiz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 56 69 72 74 75 61 6C 69 7A 65   // ation.Virtualize
                                                                                                                                                                                                                                        60 31 2B 3C 44 69 73 70 6F 73 65 41 73 79 6E 63   // `1+<DisposeAsync
                                                                                                                                                                                                                                        3E 64 5F 5F 36 30 00 00 )                         // >d__60..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::DisposeAsync

  .method /*06000139*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Virtualize`1::.ctor

  .property /*1700006B*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem>
          ChildContent()
  {
    .custom /*0C0001AD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0001AE:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem> Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::get_ChildContent() /* 06000124 */
    .set instance void Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem>) /* 06000125 */
  } // end of property Virtualize`1::ChildContent
  .property /*1700006C*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem>
          ItemContent()
  {
    .custom /*0C0001B1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0001B2:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem> Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::get_ItemContent() /* 06000126 */
    .set instance void Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::set_ItemContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<!TItem>) /* 06000127 */
  } // end of property Virtualize`1::ItemContent
  .property /*1700006D*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<valuetype Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext/*02000039*/>
          Placeholder()
  {
    .custom /*0C0001B5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001B6:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<valuetype Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext/*02000039*/> Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::get_Placeholder() /* 06000128 */
    .set instance void Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::set_Placeholder(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<valuetype Microsoft.AspNetCore.Components.Web.Virtualization.PlaceholderContext/*02000039*/>) /* 06000129 */
  } // end of property Virtualize`1::Placeholder
  .property /*1700006E*/ instance callconv(8) float32
          ItemSize()
  {
    .custom /*0C0001B9:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance float32 Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::get_ItemSize() /* 0600012A */
    .set instance void Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::set_ItemSize(float32) /* 0600012B */
  } // end of property Virtualize`1::ItemSize
  .property /*1700006F*/ instance callconv(8) class Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderDelegate`1/*02000035*/<!TItem>
          ItemsProvider()
  {
    .custom /*0C0001BB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0001BC:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderDelegate`1/*02000035*/<!TItem> Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::get_ItemsProvider() /* 0600012C */
    .set instance void Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::set_ItemsProvider(class Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderDelegate`1/*02000035*/<!TItem>) /* 0600012D */
  } // end of property Virtualize`1::ItemsProvider
  .property /*17000070*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000050*/<!TItem>
          Items()
  {
    .custom /*0C0001BF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0001C0:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000050*/<!TItem> Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::get_Items() /* 0600012E */
    .set instance void Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000050*/<!TItem>) /* 0600012F */
  } // end of property Virtualize`1::Items
  .property /*17000071*/ instance callconv(8) int32
          OverscanCount()
  {
    .custom /*0C0001C2:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance int32 Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::get_OverscanCount() /* 06000130 */
    .set instance void Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1/*0200003A*/::set_OverscanCount(int32) /* 06000131 */
  } // end of property Virtualize`1::OverscanCount
} // end of class Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize`1

.class /*0200003B*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Components.Web.Virtualization.VirtualizeJsInterop
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100004E*/
{
  .class /*02000063*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__8'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C00019B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400003F*/ public int32 '<>1__state'
    .field /*04000040*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000051*/ '<>t__builder'
    .field /*04000041*/ public class Microsoft.AspNetCore.Components.Web.Virtualization.VirtualizeJsInterop/*0200003B*/ '<>4__this'
    .field /*04000042*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*0100002F*/ '<>u__1'
    .method /*06000228*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__8'::MoveNext

    .method /*06000229*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003AD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__8'::SetStateMachine

  } // end of class '<DisposeAsync>d__8'

  .class /*02000064*/ auto ansi sealed nested private beforefieldinit '<InitializeAsync>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C00019D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000043*/ public int32 '<>1__state'
    .field /*04000044*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000051*/ '<>t__builder'
    .field /*04000045*/ public class Microsoft.AspNetCore.Components.Web.Virtualization.VirtualizeJsInterop/*0200003B*/ '<>4__this'
    .field /*04000046*/ public valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/ spacerBefore
    .field /*04000047*/ public valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/ spacerAfter
    .field /*04000048*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*0100002F*/ '<>u__1'
    .method /*0600022A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InitializeAsync>d__5'::MoveNext

    .method /*0600022B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003AE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InitializeAsync>d__5'::SetStateMachine

  } // end of class '<InitializeAsync>d__5'

  .method /*0600013A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Components.Web.Virtualization.IVirtualizeJsCallbacks/*02000038*/ owner,
                               class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ jsRuntime) cil managed
  // SIG: 20 02 01 12 80 E0 12 69
  {
    .custom /*0C0002D6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0002D7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(string) /* 0A000015 */ = ( 01 00 15 4F 6E 53 70 61 63 65 72 42 65 66 6F 72   // ...OnSpacerBefor
                                                                                                                                                                                     65 56 69 73 69 62 6C 65 00 00 )                   // eVisible..
    .custom /*0C0002D8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(string) /* 0A000015 */ = ( 01 00 14 4F 6E 53 70 61 63 65 72 41 66 74 65 72   // ...OnSpacerAfter
                                                                                                                                                                                     56 69 73 69 62 6C 65 00 00 )                      // Visible..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualizeJsInterop::.ctor

  .method /*0600013B*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000018*/ 
          InitializeAsync(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/ spacerBefore,
                          valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/ spacerAfter) cil managed
  // SIG: 20 02 11 61 11 65 11 65
  {
    .custom /*0C0002D9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 5C 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..\Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 57 65 62 2E 56 69 72 74 75 61 6C 69 7A   // ts.Web.Virtualiz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 56 69 72 74 75 61 6C 69 7A 65   // ation.Virtualize
                                                                                                                                                                                                                                        4A 73 49 6E 74 65 72 6F 70 2B 3C 49 6E 69 74 69   // JsInterop+<Initi
                                                                                                                                                                                                                                        61 6C 69 7A 65 41 73 79 6E 63 3E 64 5F 5F 35 00   // alizeAsync>d__5.
                                                                                                                                                                                                                                        00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualizeJsInterop::InitializeAsync

  .method /*0600013C*/ public hidebysig instance void 
          OnSpacerBeforeVisible(float32 spacerSize,
                                float32 spacerSeparation,
                                float32 containerSize) cil managed
  // SIG: 20 03 01 0C 0C 0C
  {
    .custom /*0C0002DB:0A000016*/ instance void [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.JSInvokableAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualizeJsInterop::OnSpacerBeforeVisible

  .method /*0600013D*/ public hidebysig instance void 
          OnSpacerAfterVisible(float32 spacerSize,
                               float32 spacerSeparation,
                               float32 containerSize) cil managed
  // SIG: 20 03 01 0C 0C 0C
  {
    .custom /*0C0002DD:0A000016*/ instance void [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.JSInvokableAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualizeJsInterop::OnSpacerAfterVisible

  .method /*0600013E*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000018*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 61
  {
    .custom /*0C0002DF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 59 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..YMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 57 65 62 2E 56 69 72 74 75 61 6C 69 7A   // ts.Web.Virtualiz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 56 69 72 74 75 61 6C 69 7A 65   // ation.Virtualize
                                                                                                                                                                                                                                        4A 73 49 6E 74 65 72 6F 70 2B 3C 44 69 73 70 6F   // JsInterop+<Dispo
                                                                                                                                                                                                                                        73 65 41 73 79 6E 63 3E 64 5F 5F 38 00 00 )       // seAsync>d__8..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualizeJsInterop::DisposeAsync

} // end of class Microsoft.AspNetCore.Components.Web.Virtualization.VirtualizeJsInterop

.class /*0200003C*/ public auto ansi Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000138:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000139:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00013A:0A000022*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000053*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000052*/) /* 0A000022 */ = ( 01 00 01 00 00 00 00 00 ) 
  .class /*02000065*/ sequential ansi sealed nested assembly beforefieldinit ParameterTypeCache
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
  {
    .custom /*0C00019F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0001A0:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003B*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    .field /*04000049*/ public initonly class [System.Collections/*23000007*/]System.Collections.Generic.Dictionary`2/*01000039*/<string,valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterInfo/*02000067*/> ParameterInfoByName
    .method /*0600022C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/ componentType) cil managed
    // SIG: 20 01 01 12 80 91
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ParameterTypeCache::.ctor

  } // end of class ParameterTypeCache

  .class /*02000066*/ auto ansi sealed nested assembly ParameterKind
         extends [System.Runtime/*23000001*/]System.Enum/*01000036*/
  {
    .custom /*0C0001A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .field /*0400004A*/ public specialname rtspecialname int32 value__
    .field /*0400004B*/ public static literal valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterKind/*02000066*/ Value = int32(0x00000000)
    .field /*0400004C*/ public static literal valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterKind/*02000066*/ EventCallbackWithNoParameters = int32(0x00000001)
    .field /*0400004D*/ public static literal valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterKind/*02000066*/ EventCallbackWithSingleParameter = int32(0x00000002)
  } // end of class ParameterKind

  .class /*02000067*/ sequential ansi sealed nested assembly beforefieldinit ParameterInfo
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
  {
    .custom /*0C0001A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0001A5:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003B*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    .field /*0400004E*/ private initonly class [System.Runtime/*23000001*/]System.Type/*01000024*/ '<Type>k__BackingField'
    .custom /*0C000169:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400004F*/ private initonly valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterKind/*02000066*/ '<Kind>k__BackingField'
    .custom /*0C00016D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .method /*0600022D*/ public hidebysig specialname 
            instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ 
            get_Type() cil managed
    // SIG: 20 00 12 80 91
    {
      .custom /*0C0003AF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ParameterInfo::get_Type

    .method /*0600022E*/ public hidebysig specialname 
            instance valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterKind/*02000066*/ 
            get_Kind() cil managed
    // SIG: 20 00 11 81 98
    {
      .custom /*0C0003B0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ParameterInfo::get_Kind

    .method /*0600022F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/ parameterType) cil managed
    // SIG: 20 01 01 12 80 91
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ParameterInfo::.ctor

    .property /*170000B8*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000024*/
            Type()
    {
      .get instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterInfo/*02000067*/::get_Type() /* 0600022D */
    } // end of property ParameterInfo::Type
    .property /*170000B9*/ instance callconv(8) valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterKind/*02000066*/
            Kind()
    {
      .get instance valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterKind/*02000066*/ Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterInfo/*02000067*/::get_Kind() /* 0600022E */
    } // end of property ParameterInfo::Kind
  } // end of class ParameterInfo

  .class /*02000068*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001A7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000050*/ public static initonly class Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//'<>c'/*02000068*/ '<>9'
    .field /*04000051*/ public static class [System.Runtime/*23000001*/]System.Func`2/*01000047*/<class [System.Runtime/*23000001*/]System.Type/*01000024*/,valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterTypeCache/*02000065*/> '<>9__15_0'
    .method /*06000230*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000231*/ assembly hidebysig 
            instance void  '<.cctor>b__2_0'() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__2_0'

    .method /*06000232*/ assembly hidebysig 
            instance valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterTypeCache/*02000065*/ 
            '<GetComponentParameters>b__15_0'(class [System.Runtime/*23000001*/]System.Type/*01000024*/ 'type') cil managed
    // SIG: 20 01 11 81 94 12 80 91
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetComponentParameters>b__15_0'

  } // end of class '<>c'

  .class /*02000069*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass18_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001A9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000052*/ public class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSObjectReference/*01000055*/ jsObjectReference
    .method /*06000233*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass18_0'::.ctor

    .method /*06000234*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
            '<CreateEventCallbackWithNoParameters>b__0'() cil managed
    // SIG: 20 00 12 80 9D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass18_0'::'<CreateEventCallbackWithNoParameters>b__0'

  } // end of class '<>c__DisplayClass18_0'

  .class /*0200006A*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass19_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001AA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000053*/ public class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSObjectReference/*01000055*/ jsObjectReference
    .method /*06000235*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass19_0'::.ctor

    .method /*06000236*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
            '<CreateEventCallbackWithSingleParameter>b__0'(object 'value') cil managed
    // SIG: 20 01 12 80 9D 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass19_0'::'<CreateEventCallbackWithSingleParameter>b__0'

  } // end of class '<>c__DisplayClass19_0'

  .method /*0600013F*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/ 
          get_Configuration() cil managed
  // SIG: 20 00 12 50
  {
    .custom /*0C0002E1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentInterop::get_Configuration

  .method /*06000140*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/ configuration) cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentInterop::.ctor

  .method /*06000141*/ assembly hidebysig 
          instance void  AttachToRenderer(class Microsoft.AspNetCore.Components.RenderTree.WebRenderer/*0200000B*/ renderer) cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentInterop::AttachToRenderer

  .method /*06000142*/ famorassem hidebysig newslot virtual 
          instance int32  AddRootComponent(string identifier,
                                           string domElementSelector) cil managed
  // SIG: 20 02 08 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentInterop::AddRootComponent

  .method /*06000143*/ famorassem hidebysig 
          instance void  SetRootComponentParameters(int32 componentId,
                                                    int32 parameterCount,
                                                    valuetype [System.Text.Json/*23000006*/]System.Text.Json.JsonElement/*01000028*/ parametersJson,
                                                    class [System.Text.Json/*23000006*/]System.Text.Json.JsonSerializerOptions/*01000020*/ jsonOptions) cil managed
  // SIG: 20 04 01 08 08 11 80 A1 12 80 81
  {
    .custom /*0C0002E6:0A000023*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessageAttribute/*01000054*/::.ctor(string,
                                                                                                                                                                     string) /* 0A000023 */ = ( 01 00 12 52 65 66 6C 65 63 74 69 6F 6E 41 6E 61   // ...ReflectionAna
                                                                                                                                                                                                6C 79 73 69 73 1F 49 4C 32 30 32 36 3A 52 65 71   // lysis.IL2026:Req
                                                                                                                                                                                                75 69 72 65 73 55 6E 72 65 66 65 72 65 6E 63 65   // uiresUnreference
                                                                                                                                                                                                64 43 6F 64 65 01 00 54 0E 0D 4A 75 73 74 69 66   // dCode..T..Justif
                                                                                                                                                                                                69 63 61 74 69 6F 6E 35 4F 70 65 6E 43 6F 6D 70   // ication5OpenComp
                                                                                                                                                                                                6F 6E 65 6E 74 20 61 6C 72 65 61 64 79 20 68 61   // onent already ha
                                                                                                                                                                                                73 20 74 68 65 20 72 69 67 68 74 20 73 65 74 20   // s the right set 
                                                                                                                                                                                                6F 66 20 61 74 74 72 69 62 75 74 65 73 )          // of attributes
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentInterop::SetRootComponentParameters

  .method /*06000144*/ famorassem hidebysig newslot virtual 
          instance void  RemoveRootComponent(int32 componentId) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentInterop::RemoveRootComponent

  .method /*06000145*/ assembly hidebysig static 
          valuetype Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*//ParameterTypeCache/*02000065*/ 
          GetComponentParameters(class [System.Runtime/*23000001*/]System.Type/*01000024*/ componentType) cil managed
  // SIG: 00 01 11 81 94 12 80 91
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentInterop::GetComponentParameters

  .method /*06000146*/ assembly hidebysig static 
          bool  IsEventCallbackType(class [System.Runtime/*23000001*/]System.Type/*01000024*/ 'type') cil managed
  // SIG: 00 01 02 12 80 91
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSComponentInterop::IsEventCallbackType

  .property /*17000072*/ instance callconv(8) class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/
          Configuration()
  {
    .get instance class Microsoft.AspNetCore.Components.Web.JSComponentConfigurationStore/*02000014*/ Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop/*0200003C*/::get_Configuration() /* 0600013F */
  } // end of property JSComponentInterop::Configuration
} // end of class Microsoft.AspNetCore.Components.Web.Infrastructure.JSComponentInterop

.class /*0200003D*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.EditContextFieldClassExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00013C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00013D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00013E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000147*/ public hidebysig static 
          string  FieldCssClass<TField>(class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ editContext,
                                        class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!!TField>> accessor) cil managed
  // SIG: 10 01 02 0E 12 81 59 15 12 81 5D 01 15 12 81 61 01 1E 00
  {
    .custom /*0C0002E9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TField /*2A000011*/ 
      .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextFieldClassExtensions::FieldCssClass

  .method /*06000148*/ public hidebysig static 
          string  FieldCssClass(class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ editContext,
                                [in] valuetype [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*01000059*/& fieldIdentifier) cil managed
  // SIG: 00 02 0E 12 81 59 10 11 81 65
  {
    .custom /*0C0002EB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000134*/ 
    .custom /*0C0002D2:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003B*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextFieldClassExtensions::FieldCssClass

  .method /*06000149*/ public hidebysig static 
          void  SetFieldCssClassProvider(class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ editContext,
                                         class Microsoft.AspNetCore.Components.Forms.FieldCssClassProvider/*0200003F*/ fieldCssClassProvider) cil managed
  // SIG: 00 02 01 12 81 59 12 80 FC
  {
    .custom /*0C0002EC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextFieldClassExtensions::SetFieldCssClassProvider

} // end of class Microsoft.AspNetCore.Components.Forms.EditContextFieldClassExtensions

.class /*0200003E*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.EditForm
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
{
  .custom /*0C000140:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000141:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200006B*/ auto ansi sealed nested private beforefieldinit '<HandleSubmitAsync>d__33'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0001AC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000054*/ public int32 '<>1__state'
    .field /*04000055*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000056*/ public class Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/ '<>4__this'
    .field /*04000057*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100005A*/ '<>u__1'
    .field /*04000058*/ private bool '<isValid>5__2'
    .method /*06000237*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleSubmitAsync>d__33'::MoveNext

    .method /*06000238*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003B1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleSubmitAsync>d__33'::SetStateMachine

  } // end of class '<HandleSubmitAsync>d__33'

  .method /*0600014A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::.ctor

  .method /*0600014B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 
          get_AdditionalAttributes() cil managed
  // SIG: 20 00 15 12 80 CD 02 0E 1C
  {
    .custom /*0C0002EF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000137*/ 
    .custom /*0C0002D3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::get_AdditionalAttributes

  .method /*0600014C*/ public hidebysig specialname 
          instance void  set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 CD 02 0E 1C
  {
    .custom /*0C0002F1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000138*/ 
    .custom /*0C0002D5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::set_AdditionalAttributes

  .method /*0600014D*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ 
          get_EditContext() cil managed
  // SIG: 20 00 12 81 59
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::get_EditContext

  .method /*0600014E*/ public hidebysig specialname 
          instance void  set_EditContext(class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ 'value') cil managed
  // SIG: 20 01 01 12 81 59
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::set_EditContext

  .method /*0600014F*/ public hidebysig specialname 
          instance object  get_Model() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0002F3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::get_Model

  .method /*06000150*/ public hidebysig specialname 
          instance void  set_Model(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0002F4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::set_Model

  .method /*06000151*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> 
          get_ChildContent() cil managed
  // SIG: 20 00 15 12 81 3D 01 12 81 59
  {
    .custom /*0C0002F5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800013B*/ 
    .custom /*0C0002DA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::get_ChildContent

  .method /*06000152*/ public hidebysig specialname 
          instance void  set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 3D 01 12 81 59
  {
    .custom /*0C0002F6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800013C*/ 
    .custom /*0C0002DC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::set_ChildContent

  .method /*06000153*/ public hidebysig specialname 
          instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> 
          get_OnSubmit() cil managed
  // SIG: 20 00 15 11 81 11 01 12 81 59
  {
    .custom /*0C0002F7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800013D*/ 
    .custom /*0C0002DE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::get_OnSubmit

  .method /*06000154*/ public hidebysig specialname 
          instance void  set_OnSubmit(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 11 01 12 81 59
  {
    .custom /*0C0002F8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800013E*/ 
    .custom /*0C0002E0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::set_OnSubmit

  .method /*06000155*/ public hidebysig specialname 
          instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> 
          get_OnValidSubmit() cil managed
  // SIG: 20 00 15 11 81 11 01 12 81 59
  {
    .custom /*0C0002FB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800013F*/ 
    .custom /*0C0002E2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::get_OnValidSubmit

  .method /*06000156*/ public hidebysig specialname 
          instance void  set_OnValidSubmit(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 11 01 12 81 59
  {
    .custom /*0C0002FD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000140*/ 
    .custom /*0C0002E3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::set_OnValidSubmit

  .method /*06000157*/ public hidebysig specialname 
          instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> 
          get_OnInvalidSubmit() cil managed
  // SIG: 20 00 15 11 81 11 01 12 81 59
  {
    .custom /*0C0002FE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000141*/ 
    .custom /*0C0002E4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::get_OnInvalidSubmit

  .method /*06000158*/ public hidebysig specialname 
          instance void  set_OnInvalidSubmit(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 11 01 12 81 59
  {
    .custom /*0C0002FF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000142*/ 
    .custom /*0C0002E5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::set_OnInvalidSubmit

  .method /*06000159*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::OnParametersSet

  .method /*0600015A*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C000301:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditForm::BuildRenderTree

  .property /*17000073*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>
          AdditionalAttributes()
  {
    .custom /*0C0001C4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C0001C5:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 01 00 54 02 16 43 61 70 74 75 72 65 55 6E   // ....T..CaptureUn
                                                                                                                                                                                        6D 61 74 63 68 65 64 56 61 6C 75 65 73 01 )       // matchedValues.
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::get_AdditionalAttributes() /* 0600014B */
    .set instance void Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>) /* 0600014C */
  } // end of property EditForm::AdditionalAttributes
  .property /*17000074*/ instance callconv(8) class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/
          EditContext()
  {
    .custom /*0C0001C6:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::get_EditContext() /* 0600014D */
    .set instance void Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::set_EditContext(class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/) /* 0600014E */
  } // end of property EditForm::EditContext
  .property /*17000075*/ instance callconv(8) object
          Model()
  {
    .custom /*0C0001C7:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance object Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::get_Model() /* 0600014F */
    .set instance void Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::set_Model(object) /* 06000150 */
  } // end of property EditForm::Model
  .property /*17000076*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/>
          ChildContent()
  {
    .custom /*0C0001C9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0001CA:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::get_ChildContent() /* 06000151 */
    .set instance void Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment`1/*0100004F*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/>) /* 06000152 */
  } // end of property EditForm::ChildContent
  .property /*17000077*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/>
          OnSubmit()
  {
    .custom /*0C0001CC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .custom /*0C0001CD:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::get_OnSubmit() /* 06000153 */
    .set instance void Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::set_OnSubmit(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/>) /* 06000154 */
  } // end of property EditForm::OnSubmit
  .property /*17000078*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/>
          OnValidSubmit()
  {
    .custom /*0C0001CF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .custom /*0C0001D0:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::get_OnValidSubmit() /* 06000155 */
    .set instance void Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::set_OnValidSubmit(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/>) /* 06000156 */
  } // end of property EditForm::OnValidSubmit
  .property /*17000079*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/>
          OnInvalidSubmit()
  {
    .custom /*0C0001D2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .custom /*0C0001D3:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/> Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::get_OnInvalidSubmit() /* 06000157 */
    .set instance void Microsoft.AspNetCore.Components.Forms.EditForm/*0200003E*/::set_OnInvalidSubmit(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/>) /* 06000158 */
  } // end of property EditForm::OnInvalidSubmit
} // end of class Microsoft.AspNetCore.Components.Forms.EditForm

.class /*0200003F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.FieldCssClassProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000142:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000143:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000012*/ static assembly initonly class Microsoft.AspNetCore.Components.Forms.FieldCssClassProvider/*0200003F*/ Instance
  .method /*0600015B*/ public hidebysig newslot virtual 
          instance string  GetFieldCssClass(class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ editContext,
                                            [in] valuetype [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*01000059*/& modreq([System.Runtime/*23000001*/]System.Runtime.InteropServices.InAttribute/*0100005B*/) fieldIdentifier) cil managed
  // SIG: 20 02 0E 12 81 59 1F 81 6D 10 11 81 65
  {
    .param [2]/*08000145*/ 
    .custom /*0C0002E7:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003B*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldCssClassProvider::GetFieldCssClass

  .method /*0600015C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldCssClassProvider::.ctor

} // end of class Microsoft.AspNetCore.Components.Forms.FieldCssClassProvider

.class /*02000040*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputBase`1<TValue>
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
{
  .custom /*0C000144:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000145:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
  .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600015D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 
          get_AdditionalAttributes() cil managed
  // SIG: 20 00 15 12 80 CD 02 0E 1C
  {
    .custom /*0C000303:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000146*/ 
    .custom /*0C0002E8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_AdditionalAttributes

  .method /*0600015E*/ public hidebysig specialname 
          instance void  set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 CD 02 0E 1C
  {
    .custom /*0C000304:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000147*/ 
    .custom /*0C0002EA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_AdditionalAttributes

  .method /*0600015F*/ public hidebysig specialname 
          instance !TValue  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    .custom /*0C000305:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_Value

  .method /*06000160*/ public hidebysig specialname 
          instance void  set_Value(!TValue 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    .custom /*0C000307:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_Value

  .method /*06000161*/ public hidebysig specialname 
          instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<!TValue> 
          get_ValueChanged() cil managed
  // SIG: 20 00 15 11 81 11 01 13 00
  {
    .custom /*0C000308:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000149*/ 
    .custom /*0C0002ED:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_ValueChanged

  .method /*06000162*/ public hidebysig specialname 
          instance void  set_ValueChanged(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<!TValue> 'value') cil managed
  // SIG: 20 01 01 15 11 81 11 01 13 00
  {
    .custom /*0C00030A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800014A*/ 
    .custom /*0C0002EE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_ValueChanged

  .method /*06000163*/ public hidebysig specialname 
          instance class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>> 
          get_ValueExpression() cil managed
  // SIG: 20 00 15 12 81 5D 01 15 12 81 61 01 13 00
  {
    .custom /*0C00030B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800014B*/ 
    .custom /*0C0002F0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_ValueExpression

  .method /*06000164*/ public hidebysig specialname 
          instance void  set_ValueExpression(class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>> 'value') cil managed
  // SIG: 20 01 01 15 12 81 5D 01 15 12 81 61 01 13 00
  {
    .custom /*0C00030E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800014C*/ 
    .custom /*0C0002F2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_ValueExpression

  .method /*06000165*/ public hidebysig specialname 
          instance string  get_DisplayName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000310:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_DisplayName

  .method /*06000166*/ public hidebysig specialname 
          instance void  set_DisplayName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000312:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_DisplayName

  .method /*06000167*/ family hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ 
          get_EditContext() cil managed
  // SIG: 20 00 12 81 59
  {
    .custom /*0C000313:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000314:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_EditContext

  .method /*06000168*/ family hidebysig specialname 
          instance void  set_EditContext(class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ 'value') cil managed
  // SIG: 20 01 01 12 81 59
  {
    .custom /*0C000317:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000318:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_EditContext

  .method /*06000169*/ famorassem hidebysig specialname 
          instance valuetype [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*01000059*/ 
          get_FieldIdentifier() cil managed
  // SIG: 20 00 11 81 65
  {
    .custom /*0C00031A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_FieldIdentifier

  .method /*0600016A*/ famorassem hidebysig specialname 
          instance void  set_FieldIdentifier(valuetype [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*01000059*/ 'value') cil managed
  // SIG: 20 01 01 11 81 65
  {
    .custom /*0C00031B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_FieldIdentifier

  .method /*0600016B*/ family hidebysig specialname 
          instance !TValue  get_CurrentValue() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_CurrentValue

  .method /*0600016C*/ family hidebysig specialname 
          instance void  set_CurrentValue(!TValue 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_CurrentValue

  .method /*0600016D*/ family hidebysig specialname 
          instance string  get_CurrentValueAsString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_CurrentValueAsString

  .method /*0600016E*/ family hidebysig specialname 
          instance void  set_CurrentValueAsString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::set_CurrentValueAsString

  .method /*0600016F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::.ctor

  .method /*06000170*/ family hidebysig newslot virtual 
          instance string  FormatValueAsString(!TValue 'value') cil managed
  // SIG: 20 01 0E 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::FormatValueAsString

  .method /*06000171*/ family hidebysig newslot abstract virtual 
          instance bool  TryParseValueFromString(string 'value',
                                                 [out] !TValue& result,
                                                 [out] string& validationErrorMessage) cil managed
  // SIG: 20 03 02 0E 10 13 00 10 0E
  {
    .param [2]/*08000154*/ 
    .custom /*0C0002F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0002FA:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*0100005C*/::.ctor(bool) /* 0A000025 */ = ( 01 00 00 00 00 ) 
    .param [3]/*08000155*/ 
    .custom /*0C0002FC:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method InputBase`1::TryParseValueFromString

  .method /*06000172*/ family hidebysig specialname 
          instance string  get_CssClass() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000321:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::get_CssClass

  .method /*06000173*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          SetParametersAsync(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterView/*0100005F*/ parameters) cil managed
  // SIG: 20 01 12 80 9D 11 81 7D
  {
    .custom /*0C000322:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000323:0A000027*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MemberNotNullAttribute/*0100005E*/::.ctor(string[]) /* 0A000027 */ = ( 01 00 02 00 00 00 0B 45 64 69 74 43 6F 6E 74 65   // .......EditConte
                                                                                                                                                                                   78 74 13 43 61 73 63 61 64 65 64 45 64 69 74 43   // xt.CascadedEditC
                                                                                                                                                                                   6F 6E 74 65 78 74 00 00 )                         // ontext..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::SetParametersAsync

  .method /*06000174*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::Dispose

  .method /*06000175*/ private hidebysig newslot virtual final 
          instance void  System.IDisposable.Dispose() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.IDisposable/*01000032*/::Dispose /*01000032::0A000024*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputBase`1::System.IDisposable.Dispose

  .property /*1700007A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>
          AdditionalAttributes()
  {
    .custom /*0C0001D5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C0001D6:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 01 00 54 02 16 43 61 70 74 75 72 65 55 6E   // ....T..CaptureUn
                                                                                                                                                                                        6D 61 74 63 68 65 64 56 61 6C 75 65 73 01 )       // matchedValues.
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_AdditionalAttributes() /* 0600015D */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>) /* 0600015E */
  } // end of property InputBase`1::AdditionalAttributes
  .property /*1700007B*/ instance callconv(8) !TValue
          Value()
  {
    .custom /*0C0001D8:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance !TValue Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_Value() /* 0600015F */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_Value(!TValue) /* 06000160 */
  } // end of property InputBase`1::Value
  .property /*1700007C*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<!TValue>
          ValueChanged()
  {
    .custom /*0C0001DA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .custom /*0C0001DB:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<!TValue> Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_ValueChanged() /* 06000161 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_ValueChanged(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<!TValue>) /* 06000162 */
  } // end of property InputBase`1::ValueChanged
  .property /*1700007D*/ instance callconv(8) class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>>
          ValueExpression()
  {
    .custom /*0C0001DD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C0001DE:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>> Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_ValueExpression() /* 06000163 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_ValueExpression(class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>>) /* 06000164 */
  } // end of property InputBase`1::ValueExpression
  .property /*1700007E*/ instance callconv(8) string
          DisplayName()
  {
    .custom /*0C0001E0:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_DisplayName() /* 06000165 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_DisplayName(string) /* 06000166 */
  } // end of property InputBase`1::DisplayName
  .property /*1700007F*/ instance callconv(8) class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/
          EditContext()
  {
    .custom /*0C0001E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/ Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_EditContext() /* 06000167 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_EditContext(class [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.EditContext/*01000056*/) /* 06000168 */
  } // end of property InputBase`1::EditContext
  .property /*17000080*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*01000059*/
          FieldIdentifier()
  {
    .get instance valuetype [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*01000059*/ Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_FieldIdentifier() /* 06000169 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_FieldIdentifier(valuetype [Microsoft.AspNetCore.Components.Forms/*23000008*/]Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*01000059*/) /* 0600016A */
  } // end of property InputBase`1::FieldIdentifier
  .property /*17000081*/ instance callconv(8) !TValue
          CurrentValue()
  {
    .get instance !TValue Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_CurrentValue() /* 0600016B */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_CurrentValue(!TValue) /* 0600016C */
  } // end of property InputBase`1::CurrentValue
  .property /*17000082*/ instance callconv(8) string
          CurrentValueAsString()
  {
    .get instance string Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_CurrentValueAsString() /* 0600016D */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::set_CurrentValueAsString(string) /* 0600016E */
  } // end of property InputBase`1::CurrentValueAsString
  .property /*17000083*/ instance callconv(8) string
          CssClass()
  {
    .custom /*0C0001E5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/::get_CssClass() /* 06000172 */
  } // end of property InputBase`1::CssClass
} // end of class Microsoft.AspNetCore.Components.Forms.InputBase`1

.class /*02000041*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputCheckbox
       extends class Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/<bool>/*1B000001*/
{
  .method /*06000176*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 
          get_Element() cil managed
  // SIG: 20 00 15 11 81 81 01 11 65
  {
    .custom /*0C000325:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputCheckbox::get_Element

  .method /*06000177*/ family hidebysig specialname 
          instance void  set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 81 01 11 65
  {
    .custom /*0C000326:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000158*/ 
    .custom /*0C000300:0A000028*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000061*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputCheckbox::set_Element

  .method /*06000178*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C000327:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputCheckbox::BuildRenderTree

  .method /*06000179*/ family hidebysig virtual 
          instance bool  TryParseValueFromString(string 'value',
                                                 [out] bool& result,
                                                 [out] string& validationErrorMessage) cil managed
  // SIG: 20 03 02 0E 10 02 10 0E
  {
    .custom /*0C000328:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800015C*/ 
    .custom /*0C000302:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputCheckbox::TryParseValueFromString

  .method /*0600017A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputCheckbox::.ctor

  .property /*17000084*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>
          Element()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> Microsoft.AspNetCore.Components.Forms.InputCheckbox/*02000041*/::get_Element() /* 06000176 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputCheckbox/*02000041*/::set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>) /* 06000177 */
  } // end of property InputCheckbox::Element
} // end of class Microsoft.AspNetCore.Components.Forms.InputCheckbox

.class /*02000042*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputDate`1<TValue>
       extends class Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/<!TValue>/*1B000002*/
{
  .custom /*0C000148:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000149:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TValue /*2A000006*/ 
    .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000031:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
  .method /*0600017B*/ public hidebysig specialname 
          instance valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/ 
          get_Type() cil managed
  // SIG: 20 00 11 81 0C
  {
    .custom /*0C000329:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::get_Type

  .method /*0600017C*/ public hidebysig specialname 
          instance void  set_Type(valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/ 'value') cil managed
  // SIG: 20 01 01 11 81 0C
  {
    .custom /*0C00032A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::set_Type

  .method /*0600017D*/ public hidebysig specialname 
          instance string  get_ParsingErrorMessage() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00032B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::get_ParsingErrorMessage

  .method /*0600017E*/ public hidebysig specialname 
          instance void  set_ParsingErrorMessage(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00032C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::set_ParsingErrorMessage

  .method /*0600017F*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 
          get_Element() cil managed
  // SIG: 20 00 15 11 81 81 01 11 65
  {
    .custom /*0C00032D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::get_Element

  .method /*06000180*/ family hidebysig specialname 
          instance void  set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 81 01 11 65
  {
    .custom /*0C00032E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800015F*/ 
    .custom /*0C000306:0A000028*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000061*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::set_Element

  .method /*06000181*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::.ctor

  .method /*06000182*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::OnParametersSet

  .method /*06000183*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::BuildRenderTree

  .method /*06000184*/ family hidebysig virtual 
          instance string  FormatValueAsString(!TValue 'value') cil managed
  // SIG: 20 01 0E 13 00
  {
    .param [1]/*08000161*/ 
    .custom /*0C000309:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::FormatValueAsString

  .method /*06000185*/ family hidebysig virtual 
          instance bool  TryParseValueFromString(string 'value',
                                                 [out] !TValue& result,
                                                 [out] string& validationErrorMessage) cil managed
  // SIG: 20 03 02 0E 10 13 00 10 0E
  {
    .custom /*0C000330:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000163*/ 
    .custom /*0C00030C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00030D:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*0100005C*/::.ctor(bool) /* 0A000025 */ = ( 01 00 00 00 00 ) 
    .param [3]/*08000164*/ 
    .custom /*0C00030F:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputDate`1::TryParseValueFromString

  .property /*17000085*/ instance callconv(8) valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/
          Type()
  {
    .custom /*0C0001E7:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/ Microsoft.AspNetCore.Components.Forms.InputDate`1/*02000042*/::get_Type() /* 0600017B */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputDate`1/*02000042*/::set_Type(valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/) /* 0600017C */
  } // end of property InputDate`1::Type
  .property /*17000086*/ instance callconv(8) string
          ParsingErrorMessage()
  {
    .custom /*0C0001E9:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Forms.InputDate`1/*02000042*/::get_ParsingErrorMessage() /* 0600017D */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputDate`1/*02000042*/::set_ParsingErrorMessage(string) /* 0600017E */
  } // end of property InputDate`1::ParsingErrorMessage
  .property /*17000087*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>
          Element()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> Microsoft.AspNetCore.Components.Forms.InputDate`1/*02000042*/::get_Element() /* 0600017F */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputDate`1/*02000042*/::set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>) /* 06000180 */
  } // end of property InputDate`1::Element
} // end of class Microsoft.AspNetCore.Components.Forms.InputDate`1

.class /*02000043*/ public auto ansi sealed Microsoft.AspNetCore.Components.Forms.InputDateType
       extends [System.Runtime/*23000001*/]System.Enum/*01000036*/
{
  .field /*04000013*/ public specialname rtspecialname int32 value__
  .field /*04000014*/ public static literal valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/ Date = int32(0x00000000)
  .field /*04000015*/ public static literal valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/ DateTimeLocal = int32(0x00000001)
  .field /*04000016*/ public static literal valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/ Month = int32(0x00000002)
  .field /*04000017*/ public static literal valuetype Microsoft.AspNetCore.Components.Forms.InputDateType/*02000043*/ Time = int32(0x00000003)
} // end of class Microsoft.AspNetCore.Components.Forms.InputDateType

.class /*02000044*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.InputExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00014E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000186*/ public hidebysig static 
          bool  TryParseSelectableValueFromString<TValue>(class Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/<!!TValue> input,
                                                          string 'value',
                                                          [out] !!TValue& result,
                                                          [out] string& validationErrorMessage) cil managed
  // SIG: 10 01 04 02 15 12 81 00 01 1E 00 0E 10 1E 00 10 0E
  {
    .custom /*0C000331:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000332:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000012*/ 
      .custom /*0C000072:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [1]/*08000165*/ 
    .custom /*0C000311:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000167*/ 
    .custom /*0C000315:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000316:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*0100005C*/::.ctor(bool) /* 0A000025 */ = ( 01 00 00 00 00 ) 
    .param [4]/*08000168*/ 
    .custom /*0C000319:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputExtensions::TryParseSelectableValueFromString

} // end of class Microsoft.AspNetCore.Components.Forms.InputExtensions

.class /*02000045*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputFile
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
       implements Microsoft.AspNetCore.Components.Forms.IInputFileJsCallbacks/*0200004A*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
{
  .custom /*0C000150:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000151:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Components.Forms.IInputFileJsCallbacks/*0200004A*/
  .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200006C*/ auto ansi sealed nested private beforefieldinit '<ConvertToImageFileAsync>d__22'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0001B0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000059*/ public int32 '<>1__state'
    .field /*0400005A*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*01000064*/<class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/> '<>t__builder'
    .field /*0400005B*/ public class Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/ '<>4__this'
    .field /*0400005C*/ public class Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/ file
    .field /*0400005D*/ public string format
    .field /*0400005E*/ public int32 maxWidth
    .field /*0400005F*/ public int32 maxHeight
    .field /*04000060*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000037*/<class Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/> '<>u__1'
    .method /*06000239*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ConvertToImageFileAsync>d__22'::MoveNext

    .method /*0600023A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003B2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ConvertToImageFileAsync>d__22'::SetStateMachine

  } // end of class '<ConvertToImageFileAsync>d__22'

  .class /*0200006D*/ auto ansi sealed nested private beforefieldinit '<OnAfterRenderAsync>d__19'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0001B4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000061*/ public int32 '<>1__state'
    .field /*04000062*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000063*/ public bool firstRender
    .field /*04000064*/ public class Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/ '<>4__this'
    .field /*04000065*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*0100002F*/ '<>u__1'
    .method /*0600023B*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnAfterRenderAsync>d__19'::MoveNext

    .method /*0600023C*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003B3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnAfterRenderAsync>d__19'::SetStateMachine

  } // end of class '<OnAfterRenderAsync>d__19'

  .method /*06000187*/ assembly hidebysig specialname 
          instance class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ 
          get_JSRuntime() cil managed
  // SIG: 20 00 12 69
  {
    .custom /*0C000333:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::get_JSRuntime

  .method /*06000188*/ assembly hidebysig specialname 
          instance void  set_JSRuntime(class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ 'value') cil managed
  // SIG: 20 01 01 12 69
  {
    .custom /*0C000334:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::set_JSRuntime

  .method /*06000189*/ public hidebysig specialname 
          instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs/*0200004B*/> 
          get_OnChange() cil managed
  // SIG: 20 00 15 11 81 11 01 12 81 2C
  {
    .custom /*0C000335:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800016A*/ 
    .custom /*0C00031C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::get_OnChange

  .method /*0600018A*/ public hidebysig specialname 
          instance void  set_OnChange(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs/*0200004B*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 11 01 12 81 2C
  {
    .custom /*0C000336:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800016B*/ 
    .custom /*0C00031D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::set_OnChange

  .method /*0600018B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000062*/<string,object> 
          get_AdditionalAttributes() cil managed
  // SIG: 20 00 15 12 81 89 02 0E 1C
  {
    .custom /*0C000337:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*0800016C*/ 
    .custom /*0C00031E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::get_AdditionalAttributes

  .method /*0600018C*/ public hidebysig specialname 
          instance void  set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000062*/<string,object> 'value') cil managed
  // SIG: 20 01 01 15 12 81 89 02 0E 1C
  {
    .custom /*0C000338:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800016D*/ 
    .custom /*0C00031F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::set_AdditionalAttributes

  .method /*0600018D*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 
          get_Element() cil managed
  // SIG: 20 00 15 11 81 81 01 11 65
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::get_Element

  .method /*0600018E*/ family hidebysig specialname 
          instance void  set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 81 01 11 65
  {
    .param [1]/*0800016E*/ 
    .custom /*0C000320:0A000028*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000061*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::set_Element

  .method /*0600018F*/ family hidebysig virtual 
          instance void  OnInitialized() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::OnInitialized

  .method /*06000190*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          OnAfterRenderAsync(bool firstRender) cil managed
  // SIG: 20 01 12 80 9D 02
  {
    .custom /*0C000339:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 49 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..IMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 46 6F 72 6D 73 2E 49 6E 70 75 74 46 69   // ts.Forms.InputFi
                                                                                                                                                                                                                                        6C 65 2B 3C 4F 6E 41 66 74 65 72 52 65 6E 64 65   // le+<OnAfterRende
                                                                                                                                                                                                                                        72 41 73 79 6E 63 3E 64 5F 5F 31 39 00 00 )       // rAsync>d__19..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::OnAfterRenderAsync

  .method /*06000191*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::BuildRenderTree

  .method /*06000192*/ assembly hidebysig 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000063*/ 
          OpenReadStream(class Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/ file,
                         int64 maxAllowedSize,
                         valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken) cil managed
  // SIG: 20 03 12 81 8D 12 81 18 0A 11 81 31
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::OpenReadStream

  .method /*06000193*/ assembly hidebysig 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000049*/<class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/> 
          ConvertToImageFileAsync(class Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/ file,
                                  string format,
                                  int32 maxWidth,
                                  int32 maxHeight) cil managed
  // SIG: 20 04 15 11 81 25 01 12 81 24 12 81 18 0E 08 08
  {
    .custom /*0C00033A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 46 6F 72 6D 73 2E 49 6E 70 75 74 46 69   // ts.Forms.InputFi
                                                                                                                                                                                                                                        6C 65 2B 3C 43 6F 6E 76 65 72 74 54 6F 49 6D 61   // le+<ConvertToIma
                                                                                                                                                                                                                                        67 65 46 69 6C 65 41 73 79 6E 63 3E 64 5F 5F 32   // geFileAsync>d__2
                                                                                                                                                                                                                                        32 00 00 )                                        // 2..
    .param [0]/*08000174*/ 
    .custom /*0C000324:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::ConvertToImageFileAsync

  .method /*06000194*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          Microsoft.AspNetCore.Components.Forms.IInputFileJsCallbacks.NotifyChange(class Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/[] files) cil managed
  // SIG: 20 01 12 80 9D 1D 12 81 18
  {
    .override Microsoft.AspNetCore.Components.Forms.IInputFileJsCallbacks/*0200004A*/::NotifyChange /*0200004A::060001BC*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::Microsoft.AspNetCore.Components.Forms.IInputFileJsCallbacks.NotifyChange

  .method /*06000195*/ private hidebysig newslot virtual final 
          instance void  System.IDisposable.Dispose() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.IDisposable/*01000032*/::Dispose /*01000032::0A000024*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::System.IDisposable.Dispose

  .method /*06000196*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFile::.ctor

  .property /*17000088*/ instance callconv(8) class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/
          JSRuntime()
  {
    .custom /*0C0001EC:0A000029*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.InjectAttribute/*01000065*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/::get_JSRuntime() /* 06000187 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/::set_JSRuntime(class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/) /* 06000188 */
  } // end of property InputFile::JSRuntime
  .property /*17000089*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs/*0200004B*/>
          OnChange()
  {
    .custom /*0C0001EE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .custom /*0C0001EF:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs/*0200004B*/> Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/::get_OnChange() /* 06000189 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/::set_OnChange(valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs/*0200004B*/>) /* 0600018A */
  } // end of property InputFile::OnChange
  .property /*1700008A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000062*/<string,object>
          AdditionalAttributes()
  {
    .custom /*0C0001F1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C0001F2:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 01 00 54 02 16 43 61 70 74 75 72 65 55 6E   // ....T..CaptureUn
                                                                                                                                                                                        6D 61 74 63 68 65 64 56 61 6C 75 65 73 01 )       // matchedValues.
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000062*/<string,object> Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/::get_AdditionalAttributes() /* 0600018B */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/::set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000062*/<string,object>) /* 0600018C */
  } // end of property InputFile::AdditionalAttributes
  .property /*1700008B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>
          Element()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/::get_Element() /* 0600018D */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/::set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>) /* 0600018E */
  } // end of property InputFile::Element
} // end of class Microsoft.AspNetCore.Components.Forms.InputFile

.class /*02000046*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.BrowserFile
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/
{
  .custom /*0C000153:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000154:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000197*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/ 
          get_Owner() cil managed
  // SIG: 20 00 12 81 14
  {
    .custom /*0C00033B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::get_Owner

  .method /*06000198*/ assembly hidebysig specialname 
          instance void  set_Owner(class Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/ 'value') cil managed
  // SIG: 20 01 01 12 81 14
  {
    .custom /*0C00033C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::set_Owner

  .method /*06000199*/ public hidebysig specialname 
          instance int32  get_Id() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00033D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::get_Id

  .method /*0600019A*/ public hidebysig specialname 
          instance void  set_Id(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C00033E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::set_Id

  .method /*0600019B*/ public hidebysig newslot specialname virtual final 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00033F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::get_Name

  .method /*0600019C*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000340:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::set_Name

  .method /*0600019D*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/ 
          get_LastModified() cil managed
  // SIG: 20 00 11 81 99
  {
    .custom /*0C000341:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::get_LastModified

  .method /*0600019E*/ public hidebysig specialname 
          instance void  set_LastModified(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/ 'value') cil managed
  // SIG: 20 01 01 11 81 99
  {
    .custom /*0C000342:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::set_LastModified

  .method /*0600019F*/ public hidebysig newslot specialname virtual final 
          instance int64  get_Size() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::get_Size

  .method /*060001A0*/ public hidebysig specialname 
          instance void  set_Size(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::set_Size

  .method /*060001A1*/ public hidebysig newslot specialname virtual final 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000343:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::get_ContentType

  .method /*060001A2*/ public hidebysig specialname 
          instance void  set_ContentType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000344:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::set_ContentType

  .method /*060001A3*/ public hidebysig specialname 
          instance string  get_RelativePath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000345:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000346:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::get_RelativePath

  .method /*060001A4*/ public hidebysig specialname 
          instance void  set_RelativePath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000347:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000348:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::set_RelativePath

  .method /*060001A5*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000063*/ 
          OpenReadStream([opt] int64 maxAllowedSize,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken) cil managed
  // SIG: 20 02 12 81 8D 0A 11 81 31
  {
    .param [1]/*08000181*/  = int64(0x7D000)
    .param [2]/*08000182*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::OpenReadStream

  .method /*060001A6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFile::.ctor

  .property /*1700008C*/ instance callconv(8) class Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/
          Owner()
  {
    .get instance class Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/ Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::get_Owner() /* 06000197 */
    .set instance void Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::set_Owner(class Microsoft.AspNetCore.Components.Forms.InputFile/*02000045*/) /* 06000198 */
  } // end of property BrowserFile::Owner
  .property /*1700008D*/ instance callconv(8) int32
          Id()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::get_Id() /* 06000199 */
    .set instance void Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::set_Id(int32) /* 0600019A */
  } // end of property BrowserFile::Id
  .property /*1700008E*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::get_Name() /* 0600019B */
    .set instance void Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::set_Name(string) /* 0600019C */
  } // end of property BrowserFile::Name
  .property /*1700008F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/
          LastModified()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/ Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::get_LastModified() /* 0600019D */
    .set instance void Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::set_LastModified(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/) /* 0600019E */
  } // end of property BrowserFile::LastModified
  .property /*17000090*/ instance callconv(8) int64
          Size()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::get_Size() /* 0600019F */
    .set instance void Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::set_Size(int64) /* 060001A0 */
  } // end of property BrowserFile::Size
  .property /*17000091*/ instance callconv(8) string
          ContentType()
  {
    .get instance string Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::get_ContentType() /* 060001A1 */
    .set instance void Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::set_ContentType(string) /* 060001A2 */
  } // end of property BrowserFile::ContentType
  .property /*17000092*/ instance callconv(8) string
          RelativePath()
  {
    .custom /*0C0001FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::get_RelativePath() /* 060001A3 */
    .set instance void Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/::set_RelativePath(string) /* 060001A4 */
  } // end of property BrowserFile::RelativePath
} // end of class Microsoft.AspNetCore.Components.Forms.BrowserFile

.class /*02000047*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.BrowserFileExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000156:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060001A7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000049*/<class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/> 
          RequestImageFileAsync(class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/ browserFile,
                                string format,
                                int32 maxWidth,
                                int32 maxHeight) cil managed
  // SIG: 00 04 15 11 81 25 01 12 81 24 12 81 24 0E 08 08
  {
    .custom /*0C000349:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00034A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000183*/ 
    .custom /*0C00032F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileExtensions::RequestImageFileAsync

} // end of class Microsoft.AspNetCore.Components.Forms.BrowserFileExtensions

.class /*02000048*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.BrowserFileStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000063*/
{
  .custom /*0C000157:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000158:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200006E*/ auto ansi sealed nested private beforefieldinit '<CopyFileDataIntoBuffer>d__30'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0001B8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000066*/ public int32 '<>1__state'
    .field /*04000067*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*01000064*/<int32> '<>t__builder'
    .field /*04000068*/ public class Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/ '<>4__this'
    .field /*04000069*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken
    .field /*0400006A*/ public valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000069*/<uint8> destination
    .field /*0400006B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100006A*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000063*/> '<>u__1'
    .field /*0400006C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000037*/<int32> '<>u__2'
    .method /*0600023D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CopyFileDataIntoBuffer>d__30'::MoveNext

    .method /*0600023E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003B4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CopyFileDataIntoBuffer>d__30'::SetStateMachine

  } // end of class '<CopyFileDataIntoBuffer>d__30'

  .class /*0200006F*/ auto ansi sealed nested private beforefieldinit '<OpenReadStreamAsync>d__29'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0001BA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400006D*/ public int32 '<>1__state'
    .field /*0400006E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100006B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000063*/> '<>t__builder'
    .field /*0400006F*/ public class Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/ '<>4__this'
    .field /*04000070*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken
    .field /*04000071*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000037*/<class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSStreamReference/*0100006C*/> '<>u__1'
    .field /*04000072*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000037*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000063*/> '<>u__2'
    .method /*0600023F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OpenReadStreamAsync>d__29'::MoveNext

    .method /*06000240*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003B5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OpenReadStreamAsync>d__29'::SetStateMachine

  } // end of class '<OpenReadStreamAsync>d__29'

  .class /*02000070*/ auto ansi sealed nested private beforefieldinit '<ReadAsync>d__28'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0001BE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000073*/ public int32 '<>1__state'
    .field /*04000074*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*01000064*/<int32> '<>t__builder'
    .field /*04000075*/ public class Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/ '<>4__this'
    .field /*04000076*/ public valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000069*/<uint8> buffer
    .field /*04000077*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken
    .field /*04000078*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000037*/<int32> '<>u__1'
    .method /*06000241*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__28'::MoveNext

    .method /*06000242*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C0003B6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__28'::SetStateMachine

  } // end of class '<ReadAsync>d__28'

  .method /*060001A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.IJSRuntime/*0100001A*/ jsRuntime,
                               valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/ inputFileElement,
                               class Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/ file,
                               int64 maxAllowedSize,
                               valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken) cil managed
  // SIG: 20 05 01 12 69 11 65 12 81 18 0A 11 81 31
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::.ctor

  .method /*060001A9*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::get_CanRead

  .method /*060001AA*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::get_CanSeek

  .method /*060001AB*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::get_CanWrite

  .method /*060001AC*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::get_Length

  .method /*060001AD*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::get_Position

  .method /*060001AE*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::set_Position

  .method /*060001AF*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::Flush

  .method /*060001B0*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::Read

  .method /*060001B1*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000067*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 81 9D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::Seek

  .method /*060001B2*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::SetLength

  .method /*060001B3*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::Write

  .method /*060001B4*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000068*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 81 A1 01 08 1D 05 08 08 11 81 31
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::ReadAsync

  .method /*060001B5*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000049*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000069*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 81 25 01 08 15 11 81 A5 01 05 11 81 31
  {
    .custom /*0C000352:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000353:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002B*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000024*/) /* 0A000017 */ = ( 01 00 48 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..HMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                        74 73 2E 46 6F 72 6D 73 2E 42 72 6F 77 73 65 72   // ts.Forms.Browser
                                                                                                                                                                                                                                        46 69 6C 65 53 74 72 65 61 6D 2B 3C 52 65 61 64   // FileStream+<Read
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 32 38 00 00 )          // Async>d__28..
    .param [2]/*0800019C*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::ReadAsync

  .method /*060001B6*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowserFileStream::Dispose

  .property /*17000093*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/::get_CanRead() /* 060001A9 */
  } // end of property BrowserFileStream::CanRead
  .property /*17000094*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/::get_CanSeek() /* 060001AA */
  } // end of property BrowserFileStream::CanSeek
  .property /*17000095*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/::get_CanWrite() /* 060001AB */
  } // end of property BrowserFileStream::CanWrite
  .property /*17000096*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/::get_Length() /* 060001AC */
  } // end of property BrowserFileStream::Length
  .property /*17000097*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/::get_Position() /* 060001AD */
    .set instance void Microsoft.AspNetCore.Components.Forms.BrowserFileStream/*02000048*/::set_Position(int64) /* 060001AE */
  } // end of property BrowserFileStream::Position
} // end of class Microsoft.AspNetCore.Components.Forms.BrowserFileStream

.class /*02000049*/ interface public abstract auto ansi Microsoft.AspNetCore.Components.Forms.IBrowserFile
{
  .custom /*0C00015D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*060001B7*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IBrowserFile::get_Name

  .method /*060001B8*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/ 
          get_LastModified() cil managed
  // SIG: 20 00 11 81 99
  {
    // Method begins at RVA 0x0
  } // end of method IBrowserFile::get_LastModified

  .method /*060001B9*/ public hidebysig newslot specialname abstract virtual 
          instance int64  get_Size() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x0
  } // end of method IBrowserFile::get_Size

  .method /*060001BA*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IBrowserFile::get_ContentType

  .method /*060001BB*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000063*/ 
          OpenReadStream([opt] int64 maxAllowedSize,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100004C*/ cancellationToken) cil managed
  // SIG: 20 02 12 81 8D 0A 11 81 31
  {
    .param [1]/*0800019E*/  = int64(0x7D000)
    .param [2]/*0800019F*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IBrowserFile::OpenReadStream

  .property /*17000098*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/::get_Name() /* 060001B7 */
  } // end of property IBrowserFile::Name
  .property /*17000099*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/
          LastModified()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/ Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/::get_LastModified() /* 060001B8 */
  } // end of property IBrowserFile::LastModified
  .property /*1700009A*/ instance callconv(8) int64
          Size()
  {
    .get instance int64 Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/::get_Size() /* 060001B9 */
  } // end of property IBrowserFile::Size
  .property /*1700009B*/ instance callconv(8) string
          ContentType()
  {
    .get instance string Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/::get_ContentType() /* 060001BA */
  } // end of property IBrowserFile::ContentType
} // end of class Microsoft.AspNetCore.Components.Forms.IBrowserFile

.class /*0200004A*/ interface private abstract auto ansi Microsoft.AspNetCore.Components.Forms.IInputFileJsCallbacks
{
  .custom /*0C00015E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*060001BC*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          NotifyChange(class Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/[] files) cil managed
  // SIG: 20 01 12 80 9D 1D 12 81 18
  {
    // Method begins at RVA 0x0
  } // end of method IInputFileJsCallbacks::NotifyChange

} // end of class Microsoft.AspNetCore.Components.Forms.IInputFileJsCallbacks

.class /*0200004B*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100003E*/
{
  .custom /*0C000160:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000161:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060001BD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100006D*/<class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/> files) cil managed
  // SIG: 20 01 01 15 12 81 B5 01 12 81 24
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFileChangeEventArgs::.ctor

  .method /*060001BE*/ public hidebysig specialname 
          instance int32  get_FileCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFileChangeEventArgs::get_FileCount

  .method /*060001BF*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/ 
          get_File() cil managed
  // SIG: 20 00 12 81 24
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFileChangeEventArgs::get_File

  .method /*060001C0*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100006D*/<class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/> 
          GetMultipleFiles([opt] int32 maximumFileCount) cil managed
  // SIG: 20 01 15 12 81 B5 01 12 81 24 08
  {
    .param [1]/*080001A2*/  = int32(0x0000000A)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFileChangeEventArgs::GetMultipleFiles

  .property /*1700009C*/ instance callconv(8) int32
          FileCount()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs/*0200004B*/::get_FileCount() /* 060001BE */
  } // end of property InputFileChangeEventArgs::FileCount
  .property /*1700009D*/ instance callconv(8) class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/
          File()
  {
    .get instance class Microsoft.AspNetCore.Components.Forms.IBrowserFile/*02000049*/ Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs/*0200004B*/::get_File() /* 060001BF */
  } // end of property InputFileChangeEventArgs::File
} // end of class Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs

.class /*0200004C*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.InputFileInterop
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000163:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000164:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000018*/ public static literal string Init = "Blazor._internal.InputFile.init"
  .field /*04000019*/ public static literal string ReadFileData = "Blazor._internal.InputFile.readFileData"
  .field /*0400001A*/ public static literal string ToImageFile = "Blazor._internal.InputFile.toImageFile"
} // end of class Microsoft.AspNetCore.Components.Forms.InputFileInterop

.class /*0200004D*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputFileJsCallbacksRelay
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
{
  .custom /*0C000166:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000167:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060001C1*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000032*/ 
          get_DotNetReference() cil managed
  // SIG: 20 00 12 80 C9
  {
    .custom /*0C00035A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFileJsCallbacksRelay::get_DotNetReference

  .method /*060001C2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Components.Forms.IInputFileJsCallbacks/*0200004A*/ callbacks) cil managed
  // SIG: 20 01 01 12 81 28
  {
    .custom /*0C00035C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute/*01000025*/::.ctor(string) /* 0A000015 */ = ( 01 00 0C 4E 6F 74 69 66 79 43 68 61 6E 67 65 00   // ...NotifyChange.
                                                                                                                                                                                     00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFileJsCallbacksRelay::.ctor

  .method /*060001C3*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          NotifyChange(class Microsoft.AspNetCore.Components.Forms.BrowserFile/*02000046*/[] files) cil managed
  // SIG: 20 01 12 80 9D 1D 12 81 18
  {
    .custom /*0C00035D:0A000016*/ instance void [Microsoft.JSInterop/*23000003*/]Microsoft.JSInterop.JSInvokableAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFileJsCallbacksRelay::NotifyChange

  .method /*060001C4*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputFileJsCallbacksRelay::Dispose

  .property /*1700009E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
          DotNetReference()
  {
    .get instance class [System.Runtime/*23000001*/]System.IDisposable/*01000032*/ Microsoft.AspNetCore.Components.Forms.InputFileJsCallbacksRelay/*0200004D*/::get_DotNetReference() /* 060001C1 */
  } // end of property InputFileJsCallbacksRelay::DotNetReference
} // end of class Microsoft.AspNetCore.Components.Forms.InputFileJsCallbacksRelay

.class /*0200004E*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.RemoteBrowserFileStreamOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00016A:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*0100006E*/::.ctor(string) /* 0A00002A */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00016B:0A000021*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000043*/::.ctor(string) /* 0A000021 */ = ( 01 00 45 52 65 6D 6F 74 65 4A 53 44 61 74 61 53   // ..ERemoteJSDataS
                                                                                                                                                 74 72 65 61 6D 20 64 65 66 61 75 6C 74 73 20 61   // tream defaults a
                                                                                                                                                 72 65 20 75 74 69 6C 69 7A 65 64 20 69 6E 73 74   // re utilized inst
                                                                                                                                                 65 61 64 20 6F 66 20 74 68 65 20 6F 70 74 69 6F   // ead of the optio
                                                                                                                                                 6E 73 20 68 65 72 65 2E 00 00 )                   // ns here...
  .method /*060001C5*/ public hidebysig specialname 
          instance int32  get_MaxSegmentSize() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00035F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RemoteBrowserFileStreamOptions::get_MaxSegmentSize

  .method /*060001C6*/ public hidebysig specialname 
          instance void  set_MaxSegmentSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C000360:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RemoteBrowserFileStreamOptions::set_MaxSegmentSize

  .method /*060001C7*/ public hidebysig specialname 
          instance int32  get_MaxBufferSize() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000363:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RemoteBrowserFileStreamOptions::get_MaxBufferSize

  .method /*060001C8*/ public hidebysig specialname 
          instance void  set_MaxBufferSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C000365:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RemoteBrowserFileStreamOptions::set_MaxBufferSize

  .method /*060001C9*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100006F*/ 
          get_SegmentFetchTimeout() cil managed
  // SIG: 20 00 11 81 BD
  {
    .custom /*0C000366:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RemoteBrowserFileStreamOptions::get_SegmentFetchTimeout

  .method /*060001CA*/ public hidebysig specialname 
          instance void  set_SegmentFetchTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100006F*/ 'value') cil managed
  // SIG: 20 01 01 11 81 BD
  {
    .custom /*0C000368:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RemoteBrowserFileStreamOptions::set_SegmentFetchTimeout

  .method /*060001CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RemoteBrowserFileStreamOptions::.ctor

  .property /*1700009F*/ instance callconv(8) int32
          MaxSegmentSize()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Forms.RemoteBrowserFileStreamOptions/*0200004E*/::get_MaxSegmentSize() /* 060001C5 */
    .set instance void Microsoft.AspNetCore.Components.Forms.RemoteBrowserFileStreamOptions/*0200004E*/::set_MaxSegmentSize(int32) /* 060001C6 */
  } // end of property RemoteBrowserFileStreamOptions::MaxSegmentSize
  .property /*170000A0*/ instance callconv(8) int32
          MaxBufferSize()
  {
    .get instance int32 Microsoft.AspNetCore.Components.Forms.RemoteBrowserFileStreamOptions/*0200004E*/::get_MaxBufferSize() /* 060001C7 */
    .set instance void Microsoft.AspNetCore.Components.Forms.RemoteBrowserFileStreamOptions/*0200004E*/::set_MaxBufferSize(int32) /* 060001C8 */
  } // end of property RemoteBrowserFileStreamOptions::MaxBufferSize
  .property /*170000A1*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100006F*/
          SegmentFetchTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100006F*/ Microsoft.AspNetCore.Components.Forms.RemoteBrowserFileStreamOptions/*0200004E*/::get_SegmentFetchTimeout() /* 060001C9 */
    .set instance void Microsoft.AspNetCore.Components.Forms.RemoteBrowserFileStreamOptions/*0200004E*/::set_SegmentFetchTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100006F*/) /* 060001CA */
  } // end of property RemoteBrowserFileStreamOptions::SegmentFetchTimeout
} // end of class Microsoft.AspNetCore.Components.Forms.RemoteBrowserFileStreamOptions

.class /*0200004F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputNumber`1<TValue>
       extends class Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/<!TValue>/*1B000002*/
{
  .custom /*0C00016E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00016F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TValue /*2A000008*/ 
    .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
  .method /*060001CC*/ public hidebysig specialname 
          instance string  get_ParsingErrorMessage() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000369:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputNumber`1::get_ParsingErrorMessage

  .method /*060001CD*/ public hidebysig specialname 
          instance void  set_ParsingErrorMessage(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00036A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputNumber`1::set_ParsingErrorMessage

  .method /*060001CE*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 
          get_Element() cil managed
  // SIG: 20 00 15 11 81 81 01 11 65
  {
    .custom /*0C00036C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputNumber`1::get_Element

  .method /*060001CF*/ family hidebysig specialname 
          instance void  set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 81 01 11 65
  {
    .custom /*0C00036E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080001A9*/ 
    .custom /*0C00034B:0A000028*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000061*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputNumber`1::set_Element

  .method /*060001D0*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputNumber`1::BuildRenderTree

  .method /*060001D1*/ family hidebysig virtual 
          instance bool  TryParseValueFromString(string 'value',
                                                 [out] !TValue& result,
                                                 [out] string& validationErrorMessage) cil managed
  // SIG: 20 03 02 0E 10 13 00 10 0E
  {
    .custom /*0C00036F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001AC*/ 
    .custom /*0C00034C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00034D:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*0100005C*/::.ctor(bool) /* 0A000025 */ = ( 01 00 00 00 00 ) 
    .param [3]/*080001AD*/ 
    .custom /*0C00034E:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputNumber`1::TryParseValueFromString

  .method /*060001D2*/ family hidebysig virtual 
          instance string  FormatValueAsString(!TValue 'value') cil managed
  // SIG: 20 01 0E 13 00
  {
    .custom /*0C000370:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputNumber`1::FormatValueAsString

  .method /*060001D3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputNumber`1::.ctor

  .property /*170000A2*/ instance callconv(8) string
          ParsingErrorMessage()
  {
    .custom /*0C00020E:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Forms.InputNumber`1/*0200004F*/::get_ParsingErrorMessage() /* 060001CC */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputNumber`1/*0200004F*/::set_ParsingErrorMessage(string) /* 060001CD */
  } // end of property InputNumber`1::ParsingErrorMessage
  .property /*170000A3*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>
          Element()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> Microsoft.AspNetCore.Components.Forms.InputNumber`1/*0200004F*/::get_Element() /* 060001CE */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputNumber`1/*0200004F*/::set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>) /* 060001CF */
  } // end of property InputNumber`1::Element
} // end of class Microsoft.AspNetCore.Components.Forms.InputNumber`1

.class /*02000050*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputRadio`1<TValue>
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
{
  .custom /*0C000171:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000172:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type TValue /*2A000009*/ 
    .custom /*0C000041:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
  .method /*060001D4*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/ 
          get_Context() cil managed
  // SIG: 20 00 12 81 44
  {
    .custom /*0C000373:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::get_Context

  .method /*060001D5*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 
          get_AdditionalAttributes() cil managed
  // SIG: 20 00 15 12 80 CD 02 0E 1C
  {
    .custom /*0C000375:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*080001AF*/ 
    .custom /*0C00034F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::get_AdditionalAttributes

  .method /*060001D6*/ public hidebysig specialname 
          instance void  set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 CD 02 0E 1C
  {
    .custom /*0C000377:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080001B0*/ 
    .custom /*0C000350:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::set_AdditionalAttributes

  .method /*060001D7*/ public hidebysig specialname 
          instance !TValue  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    .custom /*0C000379:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::get_Value

  .method /*060001D8*/ public hidebysig specialname 
          instance void  set_Value(!TValue 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    .custom /*0C00037A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::set_Value

  .method /*060001D9*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00037B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::get_Name

  .method /*060001DA*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00037C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::set_Name

  .method /*060001DB*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::OnParametersSet

  .method /*060001DC*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C00037F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::BuildRenderTree

  .method /*060001DD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadio`1::.ctor

  .property /*170000A4*/ instance callconv(8) class Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/
          Context()
  {
    .get instance class Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/ Microsoft.AspNetCore.Components.Forms.InputRadio`1/*02000050*/::get_Context() /* 060001D4 */
  } // end of property InputRadio`1::Context
  .property /*170000A5*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>
          AdditionalAttributes()
  {
    .custom /*0C000213:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C000214:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 01 00 54 02 16 43 61 70 74 75 72 65 55 6E   // ....T..CaptureUn
                                                                                                                                                                                        6D 61 74 63 68 65 64 56 61 6C 75 65 73 01 )       // matchedValues.
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> Microsoft.AspNetCore.Components.Forms.InputRadio`1/*02000050*/::get_AdditionalAttributes() /* 060001D5 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputRadio`1/*02000050*/::set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>) /* 060001D6 */
  } // end of property InputRadio`1::AdditionalAttributes
  .property /*170000A6*/ instance callconv(8) !TValue
          Value()
  {
    .custom /*0C000216:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance !TValue Microsoft.AspNetCore.Components.Forms.InputRadio`1/*02000050*/::get_Value() /* 060001D7 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputRadio`1/*02000050*/::set_Value(!TValue) /* 060001D8 */
  } // end of property InputRadio`1::Value
  .property /*170000A7*/ instance callconv(8) string
          Name()
  {
    .custom /*0C000219:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Forms.InputRadio`1/*02000050*/::get_Name() /* 060001D9 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputRadio`1/*02000050*/::set_Name(string) /* 060001DA */
  } // end of property InputRadio`1::Name
} // end of class Microsoft.AspNetCore.Components.Forms.InputRadio`1

.class /*02000051*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputRadioContext
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000173:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000174:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060001DE*/ public hidebysig specialname 
          instance string  get_GroupName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000380:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioContext::get_GroupName

  .method /*060001DF*/ public hidebysig specialname 
          instance object  get_CurrentValue() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C000381:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000382:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioContext::get_CurrentValue

  .method /*060001E0*/ public hidebysig specialname 
          instance string  get_FieldClass() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000383:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioContext::get_FieldClass

  .method /*060001E1*/ public hidebysig specialname 
          instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ChangeEventArgs/*0100003C*/> 
          get_ChangeEventCallback() cil managed
  // SIG: 20 00 15 11 81 11 01 12 80 F1
  {
    .custom /*0C000384:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*080001B4*/ 
    .custom /*0C000351:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioContext::get_ChangeEventCallback

  .method /*060001E2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/ parentContext,
                               string groupName,
                               object currentValue,
                               string fieldClass,
                               valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ChangeEventArgs/*0100003C*/> changeEventCallback) cil managed
  // SIG: 20 05 01 12 81 44 0E 1C 0E 15 11 81 11 01 12 80 F1
  {
    .param [1]/*080001B5*/ 
    .custom /*0C000354:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001B7*/ 
    .custom /*0C000355:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080001B9*/ 
    .custom /*0C000356:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioContext::.ctor

  .method /*060001E3*/ public hidebysig instance class Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/ 
          FindContextInAncestors(string groupName) cil managed
  // SIG: 20 01 12 81 44 0E
  {
    .param [0]/*080001BA*/ 
    .custom /*0C000357:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioContext::FindContextInAncestors

  .property /*170000A8*/ instance callconv(8) string
          GroupName()
  {
    .get instance string Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/::get_GroupName() /* 060001DE */
  } // end of property InputRadioContext::GroupName
  .property /*170000A9*/ instance callconv(8) object
          CurrentValue()
  {
    .custom /*0C00021C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/::get_CurrentValue() /* 060001DF */
  } // end of property InputRadioContext::CurrentValue
  .property /*170000AA*/ instance callconv(8) string
          FieldClass()
  {
    .get instance string Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/::get_FieldClass() /* 060001E0 */
  } // end of property InputRadioContext::FieldClass
  .property /*170000AB*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ChangeEventArgs/*0100003C*/>
          ChangeEventCallback()
  {
    .custom /*0C000220:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get instance valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.EventCallback`1/*01000044*/<class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ChangeEventArgs/*0100003C*/> Microsoft.AspNetCore.Components.Forms.InputRadioContext/*02000051*/::get_ChangeEventCallback() /* 060001E1 */
  } // end of property InputRadioContext::ChangeEventCallback
} // end of class Microsoft.AspNetCore.Components.Forms.InputRadioContext

.class /*02000052*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputRadioGroup`1<TValue>
       extends class Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/<!TValue>/*1B000002*/
{
  .custom /*0C000175:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000176:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TValue /*2A00000A*/ 
    .custom /*0C000046:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
  .method /*060001E4*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 
          get_ChildContent() cil managed
  // SIG: 20 00 12 80 D1
  {
    .custom /*0C000385:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioGroup`1::get_ChildContent

  .method /*060001E5*/ public hidebysig specialname 
          instance void  set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D1
  {
    .custom /*0C000386:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioGroup`1::set_ChildContent

  .method /*060001E6*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000387:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioGroup`1::get_Name

  .method /*060001E7*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000388:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioGroup`1::set_Name

  .method /*060001E8*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioGroup`1::OnParametersSet

  .method /*060001E9*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C000389:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioGroup`1::BuildRenderTree

  .method /*060001EA*/ family hidebysig virtual 
          instance bool  TryParseValueFromString(string 'value',
                                                 [out] !TValue& result,
                                                 [out] string& validationErrorMessage) cil managed
  // SIG: 20 03 02 0E 10 13 00 10 0E
  {
    .param [2]/*080001C0*/ 
    .custom /*0C000358:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000359:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*0100005C*/::.ctor(bool) /* 0A000025 */ = ( 01 00 00 00 00 ) 
    .param [3]/*080001C1*/ 
    .custom /*0C00035B:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioGroup`1::TryParseValueFromString

  .method /*060001EB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputRadioGroup`1::.ctor

  .property /*170000AC*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/
          ChildContent()
  {
    .custom /*0C000222:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ Microsoft.AspNetCore.Components.Forms.InputRadioGroup`1/*02000052*/::get_ChildContent() /* 060001E4 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputRadioGroup`1/*02000052*/::set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/) /* 060001E5 */
  } // end of property InputRadioGroup`1::ChildContent
  .property /*170000AD*/ instance callconv(8) string
          Name()
  {
    .custom /*0C000224:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance string Microsoft.AspNetCore.Components.Forms.InputRadioGroup`1/*02000052*/::get_Name() /* 060001E6 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputRadioGroup`1/*02000052*/::set_Name(string) /* 060001E7 */
  } // end of property InputRadioGroup`1::Name
} // end of class Microsoft.AspNetCore.Components.Forms.InputRadioGroup`1

.class /*02000053*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputSelect`1<TValue>
       extends class Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/<!TValue>/*1B000002*/
{
  .custom /*0C000178:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000179:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TValue /*2A00000B*/ 
    .custom /*0C00004C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000022*/) /* 0A000014 */ = ( 01 00 FF FF FF FF 00 00 ) 
  .method /*060001EC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputSelect`1::.ctor

  .method /*060001ED*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 
          get_ChildContent() cil managed
  // SIG: 20 00 12 80 D1
  {
    .custom /*0C00038A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputSelect`1::get_ChildContent

  .method /*060001EE*/ public hidebysig specialname 
          instance void  set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D1
  {
    .custom /*0C00038B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputSelect`1::set_ChildContent

  .method /*060001EF*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 
          get_Element() cil managed
  // SIG: 20 00 15 11 81 81 01 11 65
  {
    .custom /*0C00038C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputSelect`1::get_Element

  .method /*060001F0*/ family hidebysig specialname 
          instance void  set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 81 01 11 65
  {
    .custom /*0C00038D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080001C3*/ 
    .custom /*0C00035E:0A000028*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000061*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputSelect`1::set_Element

  .method /*060001F1*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C00038E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputSelect`1::BuildRenderTree

  .method /*060001F2*/ family hidebysig virtual 
          instance bool  TryParseValueFromString(string 'value',
                                                 [out] !TValue& result,
                                                 [out] string& validationErrorMessage) cil managed
  // SIG: 20 03 02 0E 10 13 00 10 0E
  {
    .param [2]/*080001C6*/ 
    .custom /*0C000361:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000362:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*0100005C*/::.ctor(bool) /* 0A000025 */ = ( 01 00 00 00 00 ) 
    .param [3]/*080001C7*/ 
    .custom /*0C000364:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputSelect`1::TryParseValueFromString

  .method /*060001F3*/ family hidebysig virtual 
          instance string  FormatValueAsString(!TValue 'value') cil managed
  // SIG: 20 01 0E 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputSelect`1::FormatValueAsString

  .property /*170000AE*/ instance callconv(8) class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/
          ChildContent()
  {
    .custom /*0C000226:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/ Microsoft.AspNetCore.Components.Forms.InputSelect`1/*02000053*/::get_ChildContent() /* 060001ED */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputSelect`1/*02000053*/::set_ChildContent(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.RenderFragment/*01000034*/) /* 060001EE */
  } // end of property InputSelect`1::ChildContent
  .property /*170000AF*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>
          Element()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> Microsoft.AspNetCore.Components.Forms.InputSelect`1/*02000053*/::get_Element() /* 060001EF */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputSelect`1/*02000053*/::set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>) /* 060001F0 */
  } // end of property InputSelect`1::Element
} // end of class Microsoft.AspNetCore.Components.Forms.InputSelect`1

.class /*02000054*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputText
       extends class Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/<string>/*1B000003*/
{
  .custom /*0C00017B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00017C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
  .method /*060001F4*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 
          get_Element() cil managed
  // SIG: 20 00 15 11 81 81 01 11 65
  {
    .custom /*0C00038F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputText::get_Element

  .method /*060001F5*/ family hidebysig specialname 
          instance void  set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 81 01 11 65
  {
    .custom /*0C000390:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080001C9*/ 
    .custom /*0C000367:0A000028*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000061*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputText::set_Element

  .method /*060001F6*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputText::BuildRenderTree

  .method /*060001F7*/ family hidebysig virtual 
          instance bool  TryParseValueFromString(string 'value',
                                                 [out] string& result,
                                                 [out] string& validationErrorMessage) cil managed
  // SIG: 20 03 02 0E 10 0E 10 0E
  {
    .custom /*0C000391:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001CD*/ 
    .custom /*0C00036B:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputText::TryParseValueFromString

  .method /*060001F8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputText::.ctor

  .property /*170000B0*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>
          Element()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> Microsoft.AspNetCore.Components.Forms.InputText/*02000054*/::get_Element() /* 060001F4 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputText/*02000054*/::set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>) /* 060001F5 */
  } // end of property InputText::Element
} // end of class Microsoft.AspNetCore.Components.Forms.InputText

.class /*02000055*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.InputTextArea
       extends class Microsoft.AspNetCore.Components.Forms.InputBase`1/*02000040*/<string>/*1B000003*/
{
  .custom /*0C00017E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00017F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
  .method /*060001F9*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 
          get_Element() cil managed
  // SIG: 20 00 15 11 81 81 01 11 65
  {
    .custom /*0C000392:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputTextArea::get_Element

  .method /*060001FA*/ family hidebysig specialname 
          instance void  set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 81 01 11 65
  {
    .custom /*0C000393:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080001CE*/ 
    .custom /*0C00036D:0A000028*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000061*/::.ctor() /* 0A000028 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputTextArea::set_Element

  .method /*060001FB*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputTextArea::BuildRenderTree

  .method /*060001FC*/ family hidebysig virtual 
          instance bool  TryParseValueFromString(string 'value',
                                                 [out] string& result,
                                                 [out] string& validationErrorMessage) cil managed
  // SIG: 20 03 02 0E 10 0E 10 0E
  {
    .custom /*0C000394:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001D2*/ 
    .custom /*0C000371:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005D*/::.ctor(bool) /* 0A000026 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputTextArea::TryParseValueFromString

  .method /*060001FD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InputTextArea::.ctor

  .property /*170000B1*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>
          Element()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/> Microsoft.AspNetCore.Components.Forms.InputTextArea/*02000055*/::get_Element() /* 060001F9 */
    .set instance void Microsoft.AspNetCore.Components.Forms.InputTextArea/*02000055*/::set_Element(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000060*/<valuetype [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ElementReference/*01000019*/>) /* 060001FA */
  } // end of property InputTextArea::Element
} // end of class Microsoft.AspNetCore.Components.Forms.InputTextArea

.class /*02000056*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.ValidationMessage`1<TValue>
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
{
  .param type TValue /*2A00000C*/ 
    .custom /*0C000052:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060001FE*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 
          get_AdditionalAttributes() cil managed
  // SIG: 20 00 15 12 80 CD 02 0E 1C
  {
    .custom /*0C000395:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*080001D3*/ 
    .custom /*0C000372:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::get_AdditionalAttributes

  .method /*060001FF*/ public hidebysig specialname 
          instance void  set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 CD 02 0E 1C
  {
    .custom /*0C000396:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080001D4*/ 
    .custom /*0C000374:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::set_AdditionalAttributes

  .method /*06000200*/ public hidebysig specialname 
          instance class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>> 
          get_For() cil managed
  // SIG: 20 00 15 12 81 5D 01 15 12 81 61 01 13 00
  {
    .custom /*0C000397:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*080001D5*/ 
    .custom /*0C000376:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::get_For

  .method /*06000201*/ public hidebysig specialname 
          instance void  set_For(class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>> 'value') cil managed
  // SIG: 20 01 01 15 12 81 5D 01 15 12 81 61 01 13 00
  {
    .custom /*0C000398:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080001D6*/ 
    .custom /*0C000378:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::set_For

  .method /*06000202*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::.ctor

  .method /*06000203*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::OnParametersSet

  .method /*06000204*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C000399:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::BuildRenderTree

  .method /*06000205*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::Dispose

  .method /*06000206*/ private hidebysig newslot virtual final 
          instance void  System.IDisposable.Dispose() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.IDisposable/*01000032*/::Dispose /*01000032::0A000024*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessage`1::System.IDisposable.Dispose

  .property /*170000B2*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>
          AdditionalAttributes()
  {
    .custom /*0C00022C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C00022D:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 01 00 54 02 16 43 61 70 74 75 72 65 55 6E   // ....T..CaptureUn
                                                                                                                                                                                        6D 61 74 63 68 65 64 56 61 6C 75 65 73 01 )       // matchedValues.
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> Microsoft.AspNetCore.Components.Forms.ValidationMessage`1/*02000056*/::get_AdditionalAttributes() /* 060001FE */
    .set instance void Microsoft.AspNetCore.Components.Forms.ValidationMessage`1/*02000056*/::set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>) /* 060001FF */
  } // end of property ValidationMessage`1::AdditionalAttributes
  .property /*170000B3*/ instance callconv(8) class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>>
          For()
  {
    .custom /*0C000230:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C000231:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>> Microsoft.AspNetCore.Components.Forms.ValidationMessage`1/*02000056*/::get_For() /* 06000200 */
    .set instance void Microsoft.AspNetCore.Components.Forms.ValidationMessage`1/*02000056*/::set_For(class [System.Linq.Expressions/*23000009*/]System.Linq.Expressions.Expression`1/*01000057*/<class [System.Runtime/*23000001*/]System.Func`1/*01000058*/<!TValue>>) /* 06000201 */
  } // end of property ValidationMessage`1::For
} // end of class Microsoft.AspNetCore.Components.Forms.ValidationMessage`1

.class /*02000057*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.ValidationSummary
       extends [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ComponentBase/*01000029*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
{
  .custom /*0C000180:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000181:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000032*/
  .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000207*/ public hidebysig specialname 
          instance object  get_Model() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00039A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::get_Model

  .method /*06000208*/ public hidebysig specialname 
          instance void  set_Model(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00039B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::set_Model

  .method /*06000209*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 
          get_AdditionalAttributes() cil managed
  // SIG: 20 00 15 12 80 CD 02 0E 1C
  {
    .custom /*0C00039C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*080001DA*/ 
    .custom /*0C00037D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::get_AdditionalAttributes

  .method /*0600020A*/ public hidebysig specialname 
          instance void  set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 CD 02 0E 1C
  {
    .custom /*0C00039D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080001DB*/ 
    .custom /*0C00037E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::set_AdditionalAttributes

  .method /*0600020B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::.ctor

  .method /*0600020C*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::OnParametersSet

  .method /*0600020D*/ family hidebysig virtual 
          instance void  BuildRenderTree(class [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder/*01000035*/ builder) cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C00039E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::BuildRenderTree

  .method /*0600020E*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::Dispose

  .method /*0600020F*/ private hidebysig newslot virtual final 
          instance void  System.IDisposable.Dispose() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.IDisposable/*01000032*/::Dispose /*01000032::0A000024*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationSummary::System.IDisposable.Dispose

  .property /*170000B4*/ instance callconv(8) object
          Model()
  {
    .custom /*0C000233:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .get instance object Microsoft.AspNetCore.Components.Forms.ValidationSummary/*02000057*/::get_Model() /* 06000207 */
    .set instance void Microsoft.AspNetCore.Components.Forms.ValidationSummary/*02000057*/::set_Model(object) /* 06000208 */
  } // end of property ValidationSummary::Model
  .property /*170000B5*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>
          AdditionalAttributes()
  {
    .custom /*0C000235:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C000236:0A00001B*/ instance void [Microsoft.AspNetCore.Components/*23000002*/]Microsoft.AspNetCore.Components.ParameterAttribute/*01000031*/::.ctor() /* 0A00001B */ = ( 01 00 01 00 54 02 16 43 61 70 74 75 72 65 55 6E   // ....T..CaptureUn
                                                                                                                                                                                        6D 61 74 63 68 65 64 56 61 6C 75 65 73 01 )       // matchedValues.
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object> Microsoft.AspNetCore.Components.Forms.ValidationSummary/*02000057*/::get_AdditionalAttributes() /* 06000209 */
    .set instance void Microsoft.AspNetCore.Components.Forms.ValidationSummary/*02000057*/::set_AdditionalAttributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000033*/<string,object>) /* 0600020A */
  } // end of property ValidationSummary::AdditionalAttributes
} // end of class Microsoft.AspNetCore.Components.Forms.ValidationSummary

.class /*02000058*/ private auto ansi sealed '<PrivateImplementationDetails>'
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000183:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000210*/ assembly hidebysig static 
          uint32  ComputeStringHash(string s) cil managed
  // SIG: 00 01 09 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method '<PrivateImplementationDetails>'::ComputeStringHash

} // end of class '<PrivateImplementationDetails>'


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
