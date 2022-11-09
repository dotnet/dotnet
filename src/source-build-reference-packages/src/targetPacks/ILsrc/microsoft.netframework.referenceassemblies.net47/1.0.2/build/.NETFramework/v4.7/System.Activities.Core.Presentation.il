
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
// Time-date stamp:            0x58fa71c6
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0005a200
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0005c1fa
// Base of code:                   0x00002000
// Base of data:                   0x0005e000
// Image base:                     0x50190000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0006
// Minor subsystem version:        0x0000
// Size of image:                  0x00062000
// Size of headers:                0x00000200
// Checksum:                       0x0005fa7f
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0005c1a8 [0x0000004f] address [size] of Import Directory:          
// 0x0005e000 [0x000004c0] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0005ac00 [0x00003fb0] address [size] of Security Directory:        
// 0x00060000 [0x0000000c] address [size] of Base Relocation Table:     
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
//              0x0005a200 Virtual Size
//              0x00002000 Virtual Address
//              0x0005a200 Size of Raw Data
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
//              0x000004c0 Virtual Size
//              0x0005e000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0005a400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00060000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0005aa00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0005c000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000001fc
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0005c1ea Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 19 50 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00002594 [0x0001146c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00013a00 [0x000486a8] address [size] of Resources Directory:       
// 0x0005c0a8 [0x00000100] address [size] of Strong Name Signature:     
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
//              0x00009394 Size
//              '#~' Name
//    Stream 2:
//              0x00009400 Offset
//              0x000067b8 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000fbb8 Offset
//              0x00000008 Size
//              '#US' Name
//    Stream 4:
//              0x0000fbc0 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000fbd0 Offset
//              0x0000189c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000017090bb41f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 388016
// PE header size       : 512 (496 used)    ( 0.13%)
// PE additional info   : 17691             ( 4.56%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.02%)
// CLR meta-data size  : 70764              (18.24%)
// CLR additional info : 296872             (76.51%)
// CLR method headers  : 1328               ( 0.34%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 0.53%)
// Unaccounted          : -1271             (-0.33%)

// Num.of PE sections   : 3
//   .text    - 369152
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 70764
//   Module        -    1 (10 bytes)
//   TypeDef       -  166 (2324 bytes)    3 interfaces, 0 explicit layout
//   TypeRef       -  174 (1044 bytes)
//   MethodDef     - 1190 (16660 bytes)   19 abstract, 0 native, 1163 bodies
//   FieldDef      -  267 (1602 bytes)    141 constant
//   MemberRef     -   44 (264 bytes)
//   ParamDef      -  942 (5652 bytes)
//   MethodImpl    -  105 (630 bytes)
//   Constant      -   51 (306 bytes)
//   CustomAttribute-  493 (2958 bytes)
//   InterfaceImpl -  140 (560 bytes)
//   PropertyMap   -   72 (288 bytes)
//   Property      -  313 (1878 bytes)
//   MethodSemantic-  475 (2850 bytes)
//   TypeSpec      -    5 (10 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   17 (340 bytes)
//   ManifestResource-    2 (24 bytes)
//   NestedClass   -    8 (32 bytes)
//   EventMap      -    8 (32 bytes)
//   Event         -   10 (60 bytes)
//   GenericParam  -   14 (112 bytes)
//   GenericParamConstraint-    1 (4 bytes)
//   Strings       - 26551 bytes
//   Blobs         -  6300 bytes
//   UserStrings   -     8 bytes
//   Guids         -    16 bytes
//   Uncategorized -   227 bytes

// CLR additional info : 296872
//   Resources         - 296616

// CLR method headers : 1328
//   Num.of method bodies  - 1163
//   Num.of fat headers    - 15
//   Num.of tiny headers   - 1148

// Managed code : 0
//   Ave method size - 0
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class GeneratedInternalTypeHelper    (public) (auto) (ansi) (sealed) 
// Class ServiceDesigner                (auto) (ansi) 
// Class BindingEditor                  (auto) (ansi) 
// Class TypeToTreeConverter            (auto) (ansi) (sealed) 
// Class ContentButtonTitleConverter    (auto) (ansi) (sealed) 
// Class ActivityXRefConverter          (auto) (ansi) (sealed) 
// Class BindingPropertyValueEditor     (auto) (ansi) (sealed) 
// Class ContentCorrelationTypeExpander (auto) (ansi) 
// Class TypeEntryContainer             (auto) (ansi) (sealed) 
// Class ExpanderTypeEntry              (auto) (ansi) (sealed) 
// Class ServiceContractImporter        (public) (abstract) (auto) (ansi) (sealed) 
// Class EndpointDesigner               (auto) (ansi) (sealed) 
// Class ReceiveDesigner                (auto) (ansi) 
// Class ReceiveReplyDesigner           (auto) (ansi) 
// Class SendDesigner                   (auto) (ansi) 
// Class SendReplyDesigner              (auto) (ansi) 
// Class CorrelatesOnValueEditor        (auto) (ansi) (sealed) 
// Class MessageQueryEditor             (auto) (ansi) 
// Class MessageQuerySetDesigner        (auto) (ansi) 
// Class CorrelationDataDesigner        (auto) (ansi) 
// Class CorrelationDataWrapper         (auto) (ansi) (sealed) 
// Class CorrelationInitializerDesigner (auto) (ansi) 
// Class CorrelationInitializerValueEditor (auto) (ansi) (sealed) 
// Class InitializeCorrelationDesigner  (auto) (ansi) 
// Class ContentDialogViewModel`2       (auto) (ansi) 
// Class MessagingContentPropertyEditorResources (auto) (ansi) 
// Class ReceiveContentDialog           (auto) (ansi) 
// Class ReceiveContentPropertyEditor   (auto) (ansi) (sealed) 
// Class ReceiveReplyValidationFeature  (auto) (ansi) 
// Class SendContentDialog              (auto) (ansi) 
// Class SendContentPropertyEditor      (auto) (ansi) (sealed) 
// Class SendReplyValidationFeature     (auto) (ansi) 
// Class TransactedReceiveScopeDesigner (auto) (ansi) 
// Class CorrelationScopeDesigner       (auto) (ansi) 
// Class SendMessageContentSearchableStringConverter (auto) (ansi) 
// Class SendParametersContentSearchableStringConverter (auto) (ansi) 
// Class ReceiveMessageContentSearchableStringConverter (auto) (ansi) 
// Class ReceiveParametersContentSearchableStringConverter (auto) (ansi) 
// Class XPathMessageQuerySearchableStringConverter (auto) (ansi) 
// Class ReceiveAndSendReplyFactory     (public) (auto) (ansi) (sealed) 
// Class SendAndReceiveReplyFactory     (public) (auto) (ansi) (sealed) 
// Class DynamicArgumentDesignerOptions (public) (auto) (ansi) 
// Class DynamicArgumentDialog          (public) (auto) (ansi) (sealed) 
// Class ActivityXRefPropertyEditor     (auto) (ansi) (sealed) 
// Class TypeCollectionDesigner         (auto) (ansi) 
// Class DynamicArgumentDesigner        (auto) (ansi) (sealed) 
// Class DynamicArgumentWrapperObject   (auto) (ansi) (sealed) 
// Class TypeCollectionPropertyEditor   (auto) (ansi) (sealed) 
// Class ActivityDesignerHelper         (abstract) (auto) (ansi) (sealed) 
// Class ArgumentCollectionPropertyEditor (auto) (ansi) (sealed) 
// Class FlowSwitchLinkCasePropertyEditor (auto) (ansi) (sealed) 
// Class FlowchartDesigner              (auto) (ansi) 
// Class ParallelSeparator              (auto) (ansi) 
// Class WhileDesigner                  (auto) (ansi) 
// Class DoWhileDesigner                (auto) (ansi) 
// Class ForEachDesigner                (auto) (ansi) 
// Class IfElseDesigner                 (auto) (ansi) 
// Class FlowDecisionDesigner           (auto) (ansi) 
// Class FlowSwitchDesigner             (auto) (ansi) 
// Class InteropDesigner                (auto) (ansi) 
// Class FlowchartResizeGrip            (auto) (ansi) 
// Class PropertyNames                  (abstract) (auto) (ansi) (sealed) 
// Class CaseLabelVisibilityConverter   (auto) (ansi) 
// Class FlowchartExpressionAutomationPeer (auto) (ansi) 
// Class FlowchartSizeFeature           (auto) (ansi) 
// Class SR                             (auto) (ansi) 
// Class AssignDesigner                 (auto) (ansi) 
// Class ConnectionPointType            (public) (auto) (ansi) (sealed) 
// Class LocationChangedEventArgs       (public) (auto) (ansi) 
// Class ParallelDesigner               (auto) (ansi) 
// Class PickDesigner                   (auto) (ansi) 
// Class PickBranchDesigner             (auto) (ansi) 
// Class PickValidationErrorSourceLocator (auto) (ansi) 
// Class PickValidationErrorSourceLocatorFeature (auto) (ansi) 
// Class TryCatchDesigner               (auto) (ansi) 
// Class CatchDesigner                  (auto) (ansi) 
// Class DesignerMetadata               (public) (auto) (ansi) 
// Class RegisterMetadataDelayedWorker  (auto) (ansi) 
// Class SequenceDesigner               (auto) (ansi) 
// Class VerticalConnector              (auto) (ansi) 
// Class CaseDesigner                   (auto) (ansi) 
// Class SwitchDesigner                 (auto) (ansi) 
// Class CancellationScopeDesigner      (auto) (ansi) 
// Class CompensableActivityDesigner    (auto) (ansi) 
// Class StartSymbol                    (auto) (ansi) 
// Class InvokeMethodDesigner           (auto) (ansi) 
// Class TransactionScopeDesigner       (auto) (ansi) 
// Class CaseKeyBox                     (auto) (ansi) 
// Class CaseKeyBoxViewModel            (auto) (ansi) 
// Class CaseKeyValidationCallbackDelegate (auto) (ansi) (sealed) 
// Interface ICaseKeyBoxView                (abstract) (auto) (ansi) 
// Class ComboBoxHelper                 (abstract) (auto) (ansi) (sealed) 
// Class ExpressionToExpressionTextConverter (auto) (ansi) 
// Class FakeRoot                       (auto) (ansi) 
// Class FlowchartDesignerCommands      (public) (abstract) (auto) (ansi) (sealed) 
// Class FlowSwitchCaseEditorDialog     (auto) (ansi) (sealed) 
// Class FlowSwitchLink`1               (abstract) (auto) (ansi) 
// Class FlowSwitchDefaultLink`1        (auto) (ansi) 
// Class FlowSwitchCaseLink`1           (auto) (ansi) 
// Class FlowSwitchLinkMultiValueConverter (auto) (ansi) (sealed) 
// Class GenericFlowSwitchHelper        (abstract) (auto) (ansi) (sealed) 
// Class FlowchartExpressionAdorner     (auto) (ansi) 
// Class FlowDecisionLabelFeature       (auto) (ansi) 
// Class FlowSwitchLabelFeature         (auto) (ansi) 
// Class StartNode                      (auto) (ansi) 
// Interface IFlowSwitchLink                (abstract) (auto) (ansi) 
// Interface IFlowSwitchDefaultLink         (abstract) (auto) (ansi) 
// Class GenericTypeArgumentConverter   (public) (auto) (ansi) (sealed) 
// Class MaxValueConverter              (auto) (ansi) 
// Class MorphHelpers                   (abstract) (auto) (ansi) (sealed) 
// Class NotConverter                   (auto) (ansi) (sealed) 
// Class CaseKeyBoxIsEnabledConverter   (auto) (ansi) 
// Class SwitchTryCatchDesignerHelper   (abstract) (auto) (ansi) (sealed) 
// Class HintTextConverter              (auto) (ansi) (sealed) 
// Class HintTextMaxWidthConverter      (auto) (ansi) (sealed) 
// Class WriteLineDesigner              (auto) (ansi) 
// Class DelegateArgumentsValueEditor   (auto) (ansi) 
// Class DynamicActivityPropertyChooser (auto) (ansi) 
// Class DynamicActivityPropertyChooserViewModel (auto) (ansi) 
// Class DynamicActivityPropertyToTooltipConverter (auto) (ansi) (sealed) 
// Class FlowSwitchDefaultLinkFeature   (auto) (ansi) 
// Class FreeFormPanelUtilities         (abstract) (auto) (ansi) (sealed) 
// Class InvokeDelegateDesigner         (auto) (ansi) (sealed) 
// Class InvokeDelegateValidationFeature (auto) (ansi) 
// Class FlowchartConnectionPointsAdorner (auto) (ansi) 
// Class FlowchartFreeFormPanel         (auto) (ansi) (sealed) 
// Class TransitionReorderChange        (auto) (ansi) 
// Class NoPersistScopeDesigner         (auto) (ansi) 
// Class SelectedPropertyNameChangedEventHandler (auto) (ansi) (sealed) 
// Class SelectedPropertyNameChangedEventArgs (auto) (ansi) (sealed) 
// Class ReorderableListEditor          (auto) (ansi) 
// Class ArgumentIdentifierTrimConverter (auto) (ansi) (sealed) 
// Class ExpandableItemShowExpandedMultiValueConverter (auto) (ansi) (sealed) 
// Class ExpandableItemWrapper          (auto) (ansi) 
// Class DisplayNameConverter           (auto) (ansi) (sealed) 
// Class IsLastSelectedItemConverter    (auto) (ansi) 
// Class IsStateRootDesignerConverter   (auto) (ansi) (sealed) 
// Class StateConnectionPointToolTip    (auto) (ansi) 
// Class StateLinkButtonToolTipConverter (auto) (ansi) (sealed) 
// Class StateContainerResizeGrip       (auto) (ansi) 
// Class FinalState                     (public) (auto) (ansi) (sealed) 
// Class StateContainerEditor           (auto) (ansi) 
// Class StateDesigner                  (auto) (ansi) 
// Class StateMachineDesigner           (auto) (ansi) 
// Class TransitionDesigner             (auto) (ansi) 
// Class StateMachineValidationErrorSourceLocator (auto) (ansi) 
// Class StateMachineValidationErrorSourceLocatorFeature (auto) (ansi) 
// Class StateMachineConnectionPointsAdorner (auto) (ansi) 
// Class StateMachineFreeFormPanel      (auto) (ansi) (sealed) 
// Class FxTrace                        (abstract) (auto) (ansi) (sealed) 
// Class ForEachWithBodyFactory`1       (public) (auto) (ansi) (sealed) 
// Class ParallelForEachWithBodyFactory`1 (public) (auto) (ansi) (sealed) 
// Class PickWithTwoBranchesFactory     (public) (auto) (ansi) (sealed) 
// Class StateMachineWithInitialStateFactory (public) (auto) (ansi) (sealed) 
// Class DesignerStylesDictionary       (public) (auto) (ansi) (sealed) 
// Class EditorCategoryTemplateDictionary (auto) (ansi) (sealed) 
// Class StringResourceDictionary       (auto) (ansi) (sealed) 
// Class <GenerateActivityTemplates>d__9 (auto) (ansi) (sealed) (nested private) 
// Class MessageQueryEntry              (auto) (ansi) (sealed) (nested assembly) 
// Class CorrelationInitializerEntry    (auto) (ansi) (sealed) (nested assembly) 
// Class CorrelationDataValueEditor     (auto) (ansi) (sealed) (nested assembly) 
// Class <get_AttachedProperties>d__5   (auto) (ansi) (sealed) (nested private) 
// Class <get_AttachedProperties>d__1   (auto) (ansi) (sealed) (nested private) 
// Class <get_AttachedProperties>d__3   (auto) (ansi) (sealed) (nested private) 
// Class ConnectorCreationResult        (auto) (ansi) (sealed) (nested assembly) 
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
.assembly extern /*23000003*/ WindowsBase
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*23000004*/ System.Activities.Presentation
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*23000005*/ System.Xaml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000006*/ PresentationFramework
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*23000007*/ System.Configuration
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly extern /*23000008*/ System.ServiceModel
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000009*/ PresentationCore
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*2300000A*/ System.Xml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*2300000B*/ System.Activities
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*2300000C*/ System.ServiceModel.Activities
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*2300000D*/ System.Xml.Linq
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*2300000E*/ System.Core
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*2300000F*/ System.ServiceModel.Internals
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*23000010*/ System.Runtime.Serialization
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000011*/ System.Windows.Presentation
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly /*20000001*/ noplatform System.Activities.Core.Presentation
{
  .custom /*0C000001:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100008D*/::.ctor() /* 0A00000E */ = ( 01 00 00 00 ) 
  .custom /*0C000002:0A00000F*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*0100008E*/::.ctor(int32) /* 0A00000F */ = ( 01 00 08 00 00 00 00 00 ) 

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000003:0A000010*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*0100008F*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*0100008F*//DebuggingModes/*01000090*/) /* 0A000010 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000004:0A000011*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000091*/::.ctor(string) /* 0A000011 */ = ( 01 00 81 6D 43 49 54 2E 53 79 73 74 65 6D 2E 41   // ...mCIT.System.A
                                                                                                                                                                              63 74 69 76 69 74 69 65 73 2E 43 6F 72 65 2E 44   // ctivities.Core.D
                                                                                                                                                                              65 73 69 67 6E 2C 20 50 75 62 6C 69 63 4B 65 79   // esign, PublicKey
                                                                                                                                                                              3D 30 30 32 34 30 30 30 30 30 34 38 30 30 30 30   // =002400000480000
                                                                                                                                                                              30 39 34 30 30 30 30 30 30 30 36 30 32 30 30 30   // 0940000000602000
                                                                                                                                                                              30 30 30 32 34 30 30 30 30 35 32 35 33 34 31 33   // 0002400005253413
                                                                                                                                                                              31 30 30 30 34 30 30 30 30 30 31 30 30 30 31 30   // 1000400000100010
                                                                                                                                                                              30 62 35 66 63 39 30 65 37 30 32 37 66 36 37 38   // 0b5fc90e7027f678
                                                                                                                                                                              37 31 65 37 37 33 61 38 66 64 65 38 39 33 38 63   // 71e773a8fde8938c
                                                                                                                                                                              38 31 64 64 34 30 32 62 61 36 35 62 39 32 30 31   // 81dd402ba65b9201
                                                                                                                                                                              64 36 30 35 39 33 65 39 36 63 34 39 32 36 35 31   // d60593e96c492651
                                                                                                                                                                              65 38 38 39 63 63 31 33 66 31 34 31 35 65 62 62   // e889cc13f1415ebb
                                                                                                                                                                              35 33 66 61 63 31 31 33 31 61 65 30 62 64 33 33   // 53fac1131ae0bd33
                                                                                                                                                                              33 63 35 65 65 36 30 32 31 36 37 32 64 39 37 31   // 3c5ee6021672d971
                                                                                                                                                                              38 65 61 33 31 61 38 61 65 62 64 30 64 61 30 30   // 8ea31a8aebd0da00
                                                                                                                                                                              37 32 66 32 35 64 38 37 64 62 61 36 66 63 39 30   // 72f25d87dba6fc90
                                                                                                                                                                              66 66 64 35 39 38 65 64 34 64 61 33 35 65 34 34   // ffd598ed4da35e44
                                                                                                                                                                              63 33 39 38 63 34 35 34 33 30 37 65 38 65 33 33   // c398c454307e8e33
                                                                                                                                                                              62 38 34 32 36 31 34 33 64 61 65 63 39 66 35 39   // b8426143daec9f59
                                                                                                                                                                              36 38 33 36 66 39 37 63 38 66 37 34 37 35 30 65   // 6836f97c8f74750e
                                                                                                                                                                              35 39 37 35 63 36 34 65 32 31 38 39 66 34 35 64   // 5975c64e2189f45d
                                                                                                                                                                              65 66 34 36 62 32 61 32 62 31 32 34 37 61 64 63   // ef46b2a2b1247adc
                                                                                                                                                                              33 36 35 32 62 66 35 63 33 30 38 30 35 35 64 61   // 3652bf5c308055da
                                                                                                                                                                              39 00 00 )                                        // 9..
  .custom /*0C000005:0A000012*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000092*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000006:0A000013*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*01000093*/::.ctor(bool) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000007:0A000014*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000094*/::.ctor(string) /* 0A000014 */ = ( 01 00 27 53 79 73 74 65 6D 2E 41 63 74 69 76 69   // ..'System.Activi
                                                                                                                                                           74 69 65 73 2E 43 6F 72 65 2E 50 72 65 73 65 6E   // ties.Core.Presen
                                                                                                                                                           74 61 74 69 6F 6E 2E 64 6C 6C 00 00 )             // tation.dll..
  .custom /*0C000008:0A000015*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*01000095*/::.ctor(string) /* 0A000015 */ = ( 01 00 27 53 79 73 74 65 6D 2E 41 63 74 69 76 69   // ..'System.Activi
                                                                                                                                                                 74 69 65 73 2E 43 6F 72 65 2E 50 72 65 73 65 6E   // ties.Core.Presen
                                                                                                                                                                 74 61 74 69 6F 6E 2E 64 6C 6C 00 00 )             // tation.dll..
  .custom /*0C000009:0A000016*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000096*/::.ctor(string) /* 0A000016 */ = ( 01 00 27 53 79 73 74 65 6D 2E 41 63 74 69 76 69   // ..'System.Activi
                                                                                                                                                                  74 69 65 73 2E 43 6F 72 65 2E 50 72 65 73 65 6E   // ties.Core.Presen
                                                                                                                                                                  74 61 74 69 6F 6E 2E 64 6C 6C 00 00 )             // tation.dll..
  .custom /*0C00000A:0A000017*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000097*/::.ctor(string) /* 0A000017 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                             70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000B:0A000018*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000098*/::.ctor(string) /* 0A000018 */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                             4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C00000C:0A000019*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000099*/::.ctor(string) /* 0A000019 */ = ( 01 00 2F C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ../.. Microsoft 
                                                                                                                                                               43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 20 41 6C   // Corporation.  Al
                                                                                                                                                               6C 20 72 69 67 68 74 73 20 72 65 73 65 72 76 65   // l rights reserve
                                                                                                                                                               64 2E 00 00 )                                     // d...
  .custom /*0C00000D:0A00001A*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100009A*/::.ctor(string) /* 0A00001A */ = ( 01 00 0A 34 2E 37 2E 32 30 35 33 2E 30 00 00 )    // ...4.7.2053.0..
  .custom /*0C00000E:0A00001B*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100009B*/::.ctor(string) /* 0A00001B */ = ( 01 00 0A 34 2E 37 2E 32 30 35 33 2E 30 00 00 )    // ...4.7.2053.0..
  .custom /*0C00000F:0A00001C*/ instance void [mscorlib/*23000001*/]System.Resources.SatelliteContractVersionAttribute/*0100009C*/::.ctor(string) /* 0A00001C */ = ( 01 00 07 34 2E 30 2E 30 2E 30 00 00 )             // ...4.0.0.0..
  .custom /*0C000010:0A00001D*/ instance void [mscorlib/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*0100009D*/::.ctor(string) /* 0A00001D */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C000011:0A00001E*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDelaySignAttribute/*0100009E*/::.ctor(bool) /* 0A00001E */ = ( 01 00 01 00 00 ) 
  .custom /*0C000012:0A00001F*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyKeyFileAttribute/*0100009F*/::.ctor(string) /* 0A00001F */ = ( 01 00 28 66 3A 5C 64 64 5C 74 6F 6F 6C 73 5C 64   // ..(f:\dd\tools\d
                                                                                                                                                             65 76 64 69 76 5C 33 35 4D 53 53 68 61 72 65 64   // evdiv\35MSShared
                                                                                                                                                             4C 69 62 31 30 32 34 2E 73 6E 6B 00 00 )          // Lib1024.snk..
  .custom /*0C000013:0A000020*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblySignatureKeyAttribute/*010000A0*/::.ctor(string,
                                                                                                                                       string) /* 0A000020 */ = ( 01 00 82 40 30 30 32 34 30 30 30 30 30 63 38 30   // ...@002400000c80
                                                                                                                                                                  30 30 30 30 31 34 30 31 30 30 30 30 30 36 30 32   // 0000140100000602
                                                                                                                                                                  30 30 30 30 30 30 32 34 30 30 30 30 35 32 35 33   // 0000002400005253
                                                                                                                                                                  34 31 33 31 30 30 30 38 30 30 30 30 30 31 30 30   // 4131000800000100
                                                                                                                                                                  30 31 30 30 38 35 61 61 64 30 62 65 66 30 36 38   // 010085aad0bef068
                                                                                                                                                                  38 64 31 62 39 39 34 61 30 64 37 38 65 31 66 64   // 8d1b994a0d78e1fd
                                                                                                                                                                  32 39 66 63 32 34 61 63 33 34 65 64 33 64 33 61   // 29fc24ac34ed3d3a
                                                                                                                                                                  63 33 66 62 39 62 33 64 30 63 34 38 33 38 36 62   // c3fb9b3d0c48386b
                                                                                                                                                                  61 38 33 34 61 61 38 38 30 30 33 35 30 36 30 61   // a834aa880035060a
                                                                                                                                                                  38 38 34 38 62 32 64 38 61 64 66 35 38 65 36 37   // 8848b2d8adf58e67
                                                                                                                                                                  30 65 64 32 30 39 31 34 62 65 33 36 38 31 61 38   // 0ed20914be3681a8
                                                                                                                                                                  39 31 63 39 63 38 63 30 31 65 65 66 32 61 62 32   // 91c9c8c01eef2ab2
                                                                                                                                                                  32 38 37 32 35 34 37 63 33 39 62 65 30 30 61 66   // 2872547c39be00af
                                                                                                                                                                  30 65 36 63 37 32 34 38 35 64 37 63 66 64 31 61   // 0e6c72485d7cfd1a
                                                                                                                                                                  35 31 64 66 38 39 34 37 64 33 36 63 65 62 61 39   // 51df8947d36ceba9
                                                                                                                                                                  39 38 39 31 30 36 62 35 38 61 62 65 37 39 65 36   // 989106b58abe79e6
                                                                                                                                                                  61 33 65 37 31 61 30 31 65 64 36 62 64 63 38 36   // a3e71a01ed6bdc86
                                                                                                                                                                  37 30 31 32 38 38 33 65 30 62 31 61 34 64 33 35   // 7012883e0b1a4d35
                                                                                                                                                                  62 31 62 35 65 65 65 64 36 64 66 32 31 65 34 30   // b1b5eeed6df21e40
                                                                                                                                                                  31 62 62 30 63 32 32 66 32 32 34 36 63 63 62 36   // 1bb0c22f2246ccb6
                                                                                                                                                                  39 39 37 39 64 63 39 65 36 31 65 65 66 32 36 32   // 9979dc9e61eef262
                                                                                                                                                                  38 33 32 65 64 30 66 32 30 36 34 38 35 33 37 32   // 832ed0f206485372
                                                                                                                                                                  35 61 37 35 34 38 35 66 61 38 61 33 65 66 62 37   // 5a75485fa8a3efb7
                                                                                                                                                                  65 30 32 37 33 31 39 63 38 36 64 65 63 30 33 64   // e027319c86dec03d
                                                                                                                                                                  63 33 62 31 62 63 61 32 62 35 30 38 31 62 61 62   // c3b1bca2b5081bab
                                                                                                                                                                  35 32 61 36 32 37 62 39 39 31 37 34 35 30 64 66   // 52a627b9917450df
                                                                                                                                                                  61 64 35 33 34 37 39 39 65 31 63 37 61 66 35 38   // ad534799e1c7af58
                                                                                                                                                                  36 38 33 62 64 66 61 31 33 35 66 31 35 31 38 66   // 683bdfa135f1518f
                                                                                                                                                                  66 31 65 61 36 30 65 39 30 64 37 62 39 39 33 61   // f1ea60e90d7b993a
                                                                                                                                                                  36 63 38 37 66 64 33 64 64 39 33 34 30 38 65 33   // 6c87fd3dd93408e3
                                                                                                                                                                  35 64 31 32 39 36 66 39 61 37 66 39 61 39 37 63   // 5d1296f9a7f9a97c
                                                                                                                                                                  35 64 62 35 36 63 30 66 33 63 63 32 35 61 64 31   // 5db56c0f3cc25ad1
                                                                                                                                                                  31 65 39 37 37 37 66 39 34 64 31 33 38 62 33 63   // 1e9777f94d138b3c
                                                                                                                                                                  65 61 35 33 62 39 61 38 33 33 31 63 32 65 36 64   // ea53b9a8331c2e6d
                                                                                                                                                                  63 62 38 64 32 65 61 39 34 65 31 38 62 66 31 31   // cb8d2ea94e18bf11
                                                                                                                                                                  36 33 66 66 31 31 32 61 32 32 64 62 64 39 32 64   // 63ff112a22dbd92d
                                                                                                                                                                  34 32 39 61 81 00 38 39 31 33 65 66 38 36 39 36   // 429a..8913ef8696
                                                                                                                                                                  34 36 64 31 34 39 37 31 64 66 32 32 32 63 32 31   // 46d14971df222c21
                                                                                                                                                                  30 30 31 38 61 62 33 39 34 63 66 65 36 33 66 38   // 0018ab394cfe63f8
                                                                                                                                                                  65 62 39 62 34 64 38 39 34 63 30 64 64 61 37 33   // eb9b4d894c0dda73
                                                                                                                                                                  36 38 63 66 62 36 39 64 66 31 35 64 30 34 39 62   // 68cfb69df15d049b
                                                                                                                                                                  33 34 37 66 38 61 38 62 39 32 30 35 63 64 63 63   // 347f8a8b9205cdcc
                                                                                                                                                                  33 64 36 62 64 36 36 39 30 66 30 62 64 32 34 62   // 3d6bd6690f0bd24b
                                                                                                                                                                  33 64 61 33 31 37 39 66 65 62 38 63 30 33 66 34   // 3da3179feb8c03f4
                                                                                                                                                                  31 30 37 30 33 30 32 37 63 37 38 34 34 66 66 36   // 10703027c7844ff6
                                                                                                                                                                  35 34 39 39 37 65 33 38 30 31 35 64 66 63 39 37   // 54997e38015dfc97
                                                                                                                                                                  32 32 32 63 31 35 30 36 31 61 66 32 61 31 64 33   // 222c15061af2a1d3
                                                                                                                                                                  66 65 39 31 37 37 35 62 39 64 64 34 62 38 65 64   // fe91775b9dd4b8ed
                                                                                                                                                                  65 36 34 64 33 64 35 39 38 31 36 32 37 30 61 39   // e64d3d59816270a9
                                                                                                                                                                  35 32 30 65 33 39 33 63 38 65 36 30 36 31 39 62   // 520e393c8e60619b
                                                                                                                                                                  38 30 64 34 30 66 63 31 64 63 34 66 31 32 62 35   // 80d40fc1dc4f12b5
                                                                                                                                                                  61 61 30 63 32 64 66 32 30 64 30 32 65 61 33 32   // aa0c2df20d02ea32
                                                                                                                                                                  39 36 30 38 35 31 00 00 )                         // 960851..
  .custom /*0C000014:0A000021*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*010000A1*/::.ctor() /* 0A000021 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C000015:0A000022*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.DefaultDllImportSearchPathsAttribute/*010000A2*/::.ctor(valuetype [mscorlib/*23000001*/]System.Runtime.InteropServices.DllImportSearchPath/*010000A3*/) /* 0A000022 */ = ( 01 00 02 08 00 00 00 00 ) 
  .custom /*0C000016:0A000023*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*010000A4*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                B5 FC 90 E7 02 7F 67 87 1E 77 3A 8F DE 89 38 C8   // ......g..w:...8.
                1D D4 02 BA 65 B9 20 1D 60 59 3E 96 C4 92 65 1E   // ....e. .`Y>...e.
                88 9C C1 3F 14 15 EB B5 3F AC 11 31 AE 0B D3 33   // ...?....?..1...3
                C5 EE 60 21 67 2D 97 18 EA 31 A8 AE BD 0D A0 07   // ..`!g-...1......
                2F 25 D8 7D BA 6F C9 0F FD 59 8E D4 DA 35 E4 4C   // /%.}.o...Y...5.L
                39 8C 45 43 07 E8 E3 3B 84 26 14 3D AE C9 F5 96   // 9.EC...;.&.=....
                83 6F 97 C8 F7 47 50 E5 97 5C 64 E2 18 9F 45 DE   // .o...GP..\d...E.
                F4 6B 2A 2B 12 47 AD C3 65 2B F5 C3 08 05 5D A9 ) // .k*+.G..e+....].
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.module System.Activities.Core.Presentation.dll
// MVID: {b667f513-72da-4f5d-934c-ada343ad4f35}
.imagebase 0x50190000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007FC2E1FE7000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit XamlGeneratedNamespace.GeneratedInternalTypeHelper
       extends [WindowsBase/*23000003*/]System.Windows.Markup.InternalTypeHelper/*01000001*/
{
  .custom /*0C000017:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                    string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                               75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                               30 00 00 )                                        // 0..
  .custom /*0C000019:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000001*/ family hidebysig virtual 
          instance object  CreateInstance(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type',
                                          class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 02 1C 12 0D 12 11
  {
    // Method begins at RVA 0x2050
    // Code size       0 (0x0)
  } // end of method GeneratedInternalTypeHelper::CreateInstance

  .method /*06000002*/ family hidebysig virtual 
          instance object  GetPropertyValue(class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000005*/ propertyInfo,
                                            object target,
                                            class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 03 1C 12 15 1C 12 11
  {
    // Method begins at RVA 0x2051
    // Code size       0 (0x0)
  } // end of method GeneratedInternalTypeHelper::GetPropertyValue

  .method /*06000003*/ family hidebysig virtual 
          instance void  SetPropertyValue(class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000005*/ propertyInfo,
                                          object target,
                                          object 'value',
                                          class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 01 12 15 1C 1C 12 11
  {
    // Method begins at RVA 0x2052
    // Code size       0 (0x0)
  } // end of method GeneratedInternalTypeHelper::SetPropertyValue

  .method /*06000004*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Delegate/*01000006*/ 
          CreateDelegate(class [mscorlib/*23000001*/]System.Type/*01000003*/ delegateType,
                         object target,
                         string 'handler') cil managed
  // SIG: 20 03 12 19 12 0D 1C 0E
  {
    // Method begins at RVA 0x2054
    // Code size       0 (0x0)
  } // end of method GeneratedInternalTypeHelper::CreateDelegate

  .method /*06000005*/ family hidebysig virtual 
          instance void  AddEventHandler(class [mscorlib/*23000001*/]System.Reflection.EventInfo/*01000007*/ eventInfo,
                                         object target,
                                         class [mscorlib/*23000001*/]System.Delegate/*01000006*/ 'handler') cil managed
  // SIG: 20 03 01 12 1D 1C 12 19
  {
    // Method begins at RVA 0x2060
    // Code size       0 (0x0)
  } // end of method GeneratedInternalTypeHelper::AddEventHandler

  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2061
    // Code size       0 (0x0)
  } // end of method GeneratedInternalTypeHelper::.ctor

} // end of class XamlGeneratedNamespace.GeneratedInternalTypeHelper

.class /*02000003*/ private auto ansi beforefieldinit System.ServiceModel.Presentation.ServiceDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IExpandChild/*0100000A*/,
                  [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2062
    // Code size       0 (0x0)
  } // end of method ServiceDesigner::.ctor

  .method /*06000008*/ public hidebysig newslot specialname virtual final 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_ExpandedChild() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x2063
    // Code size       0 (0x0)
  } // end of method ServiceDesigner::get_ExpandedChild

  .method /*06000009*/ assembly hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2064
    // Code size       0 (0x0)
  } // end of method ServiceDesigner::RegisterMetadata

  .method /*0600000A*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00001A:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00001B:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2070
    // Code size       0 (0x0)
  } // end of method ServiceDesigner::InitializeComponent

  .method /*0600000B*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00001C:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00001D:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00001E:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2071
    // Code size       0 (0x0)
  } // end of method ServiceDesigner::System.Windows.Markup.IComponentConnector.Connect

  .property /*17000001*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          ExpandedChild()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Presentation.ServiceDesigner/*02000003*/::get_ExpandedChild() /* 06000008 */
  } // end of property ServiceDesigner::ExpandedChild
} // end of class System.ServiceModel.Presentation.ServiceDesigner

.class /*02000004*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.BindingEditor
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.ComboBox/*0100000D*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000001*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ BindingProperty
  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2072
    // Code size       0 (0x0)
  } // end of method BindingEditor::.ctor

  .method /*0600000D*/ public hidebysig specialname 
          instance object  get_Binding() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2073
    // Code size       0 (0x0)
  } // end of method BindingEditor::get_Binding

  .method /*0600000E*/ public hidebysig specialname 
          instance void  set_Binding(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2074
    // Code size       0 (0x0)
  } // end of method BindingEditor::set_Binding

  .method /*0600000F*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x2075
    // Code size       0 (0x0)
  } // end of method BindingEditor::OnInitialized

  .method /*06000010*/ family hidebysig virtual 
          instance void  OnSelectionChanged(class [PresentationFramework/*23000006*/]System.Windows.Controls.SelectionChangedEventArgs/*01000010*/ e) cil managed
  // SIG: 20 01 01 12 41
  {
    // Method begins at RVA 0x2076
    // Code size       0 (0x0)
  } // end of method BindingEditor::OnSelectionChanged

  .method /*06000011*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00001F:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000020:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2077
    // Code size       0 (0x0)
  } // end of method BindingEditor::InitializeComponent

  .method /*06000012*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000021:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000022:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000023:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2078
    // Code size       0 (0x0)
  } // end of method BindingEditor::System.Windows.Markup.IComponentConnector.Connect

  .method /*06000013*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2079
    // Code size       0 (0x0)
  } // end of method BindingEditor::.cctor

  .property /*17000002*/ instance callconv(8) object
          Binding()
  {
    .get instance object System.ServiceModel.Activities.Presentation.BindingEditor/*02000004*/::get_Binding() /* 0600000D */
    .set instance void System.ServiceModel.Activities.Presentation.BindingEditor/*02000004*/::set_Binding(object) /* 0600000E */
  } // end of property BindingEditor::Binding
} // end of class System.ServiceModel.Activities.Presentation.BindingEditor

.class /*02000005*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.TypeToTreeConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*06000014*/ private hidebysig newslot virtual final 
          instance object  System.Windows.Data.IValueConverter.Convert(object 'value',
                                                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                                                       object parameter,
                                                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    .override [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/::Convert /*01000011::0A000002*/ 
    // Method begins at RVA 0x207a
    // Code size       0 (0x0)
  } // end of method TypeToTreeConverter::System.Windows.Data.IValueConverter.Convert

  .method /*06000015*/ private hidebysig newslot virtual final 
          instance object  System.Windows.Data.IValueConverter.ConvertBack(object 'value',
                                                                           class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                                                           object parameter,
                                                                           class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    .override [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/::ConvertBack /*01000011::0A000003*/ 
    // Method begins at RVA 0x207b
    // Code size       0 (0x0)
  } // end of method TypeToTreeConverter::System.Windows.Data.IValueConverter.ConvertBack

  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x207c
    // Code size       0 (0x0)
  } // end of method TypeToTreeConverter::.ctor

} // end of class System.ServiceModel.Activities.Presentation.TypeToTreeConverter

.class /*02000006*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.ContentButtonTitleConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*06000017*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x207d
    // Code size       0 (0x0)
  } // end of method ContentButtonTitleConverter::Convert

  .method /*06000018*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x207e
    // Code size       0 (0x0)
  } // end of method ContentButtonTitleConverter::ConvertBack

  .method /*06000019*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x207f
    // Code size       0 (0x0)
  } // end of method ContentButtonTitleConverter::.ctor

} // end of class System.ServiceModel.Activities.Presentation.ContentButtonTitleConverter

.class /*02000007*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.ActivityXRefConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*0600001A*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2080
    // Code size       0 (0x0)
  } // end of method ActivityXRefConverter::Convert

  .method /*0600001B*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2081
    // Code size       0 (0x0)
  } // end of method ActivityXRefConverter::ConvertBack

  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2082
    // Code size       0 (0x0)
  } // end of method ActivityXRefConverter::.ctor

} // end of class System.ServiceModel.Activities.Presentation.ActivityXRefConverter

.class /*02000008*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.BindingPropertyValueEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValueEditor/*01000012*/
{
  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2083
    // Code size       0 (0x0)
  } // end of method BindingPropertyValueEditor::.ctor

} // end of class System.ServiceModel.Activities.Presentation.BindingPropertyValueEditor

.class /*02000009*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.ContentControl/*01000013*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*04000002*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ IsSelectionValidProperty
  .field /*04000003*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.RoutedEvent/*01000016*/ IsSelectionValidChangedEvent
  .field /*04000004*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.RoutedEvent/*01000016*/ SelectionChangedEvent
  .field /*04000005*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TypesToExpandProperty
  .field /*04000006*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ SelectedTypeEntryProperty
  .field /*04000007*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.TreeView/*01000017*/ typeExpander
  .field /*04000008*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.TextBlock/*01000018*/ emptyContent
  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2084
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::.ctor

  .method /*0600001F*/ public hidebysig specialname 
          instance void  add_IsSelectionValidChanged(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x2085
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::add_IsSelectionValidChanged

  .method /*06000020*/ public hidebysig specialname 
          instance void  remove_IsSelectionValidChanged(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x2086
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::remove_IsSelectionValidChanged

  .method /*06000021*/ public hidebysig specialname 
          instance void  add_SelectionChanged(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x2087
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::add_SelectionChanged

  .method /*06000022*/ public hidebysig specialname 
          instance void  remove_SelectionChanged(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x2088
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::remove_SelectionChanged

  .method /*06000023*/ public hidebysig specialname 
          instance bool  get_IsSelectionValid() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2089
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::get_IsSelectionValid

  .method /*06000024*/ private hidebysig specialname 
          instance void  set_IsSelectionValid(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x208a
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::set_IsSelectionValid

  .method /*06000025*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/> 
          get_TypesToExpand() cil managed
  // SIG: 20 00 15 12 65 01 12 2C
  {
    // Method begins at RVA 0x208b
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::get_TypesToExpand

  .method /*06000026*/ public hidebysig specialname 
          instance void  set_TypesToExpand(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/> 'value') cil managed
  // SIG: 20 01 01 15 12 65 01 12 2C
  {
    // Method begins at RVA 0x208c
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::set_TypesToExpand

  .method /*06000027*/ public hidebysig specialname 
          instance class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/ 
          get_SelectedTypeEntry() cil managed
  // SIG: 20 00 12 2C
  {
    // Method begins at RVA 0x208d
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::get_SelectedTypeEntry

  .method /*06000028*/ private hidebysig specialname 
          instance void  set_SelectedTypeEntry(class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x208e
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::set_SelectedTypeEntry

  .method /*06000029*/ public hidebysig instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          GetSelectedType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x208f
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::GetSelectedType

  .method /*0600002A*/ public hidebysig instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100001A*/[] 
          GetMemberPath() cil managed
  // SIG: 20 00 1D 12 69
  {
    // Method begins at RVA 0x2090
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::GetMemberPath

  .method /*0600002B*/ assembly hidebysig static 
          bool  IsPrimitiveTypeInXPath(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 00 01 02 12 0D
  {
    // Method begins at RVA 0x2091
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::IsPrimitiveTypeInXPath

  .method /*0600002C*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000024:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000025:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2092
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::InitializeComponent

  .method /*0600002D*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000026:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000027:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000028:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2093
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600002E*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000029:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00002A:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00002B:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x2094
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::System.Windows.Markup.IStyleConnector.Connect

  .method /*0600002F*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2095
    // Code size       0 (0x0)
  } // end of method ContentCorrelationTypeExpander::.cctor

  .event /*14000001*/ [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ IsSelectionValidChanged
  {
    .addon instance void System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::add_IsSelectionValidChanged(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 0600001F */
    .removeon instance void System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::remove_IsSelectionValidChanged(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 06000020 */
  } // end of event ContentCorrelationTypeExpander::IsSelectionValidChanged
  .event /*14000002*/ [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ SelectionChanged
  {
    .addon instance void System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::add_SelectionChanged(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 06000021 */
    .removeon instance void System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::remove_SelectionChanged(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 06000022 */
  } // end of event ContentCorrelationTypeExpander::SelectionChanged
  .property /*17000003*/ instance callconv(8) bool
          IsSelectionValid()
  {
    .get instance bool System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::get_IsSelectionValid() /* 06000023 */
    .set instance void System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::set_IsSelectionValid(bool) /* 06000024 */
  } // end of property ContentCorrelationTypeExpander::IsSelectionValid
  .property /*17000004*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/>
          TypesToExpand()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/> System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::get_TypesToExpand() /* 06000025 */
    .set instance void System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::set_TypesToExpand(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/>) /* 06000026 */
  } // end of property ContentCorrelationTypeExpander::TypesToExpand
  .property /*17000005*/ instance callconv(8) class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/
          SelectedTypeEntry()
  {
    .get instance class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/ System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::get_SelectedTypeEntry() /* 06000027 */
    .set instance void System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander/*02000009*/::set_SelectedTypeEntry(class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/) /* 06000028 */
  } // end of property ContentCorrelationTypeExpander::SelectedTypeEntry
} // end of class System.ServiceModel.Activities.Presentation.ContentCorrelationTypeExpander

.class /*0200000A*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.TypeEntryContainer
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*06000030*/ public hidebysig specialname 
          instance string  get_DisplayText() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00002C:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2096
    // Code size       0 (0x0)
  } // end of method TypeEntryContainer::get_DisplayText

  .method /*06000031*/ public hidebysig specialname 
          instance void  set_DisplayText(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00002D:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2097
    // Code size       0 (0x0)
  } // end of method TypeEntryContainer::set_DisplayText

  .method /*06000032*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/> 
          get_Items() cil managed
  // SIG: 20 00 15 12 65 01 12 2C
  {
    .custom /*0C00002E:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2098
    // Code size       0 (0x0)
  } // end of method TypeEntryContainer::get_Items

  .method /*06000033*/ public hidebysig specialname 
          instance void  set_Items(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/> 'value') cil managed
  // SIG: 20 01 01 15 12 65 01 12 2C
  {
    .custom /*0C000030:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2099
    // Code size       0 (0x0)
  } // end of method TypeEntryContainer::set_Items

  .method /*06000034*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x209a
    // Code size       0 (0x0)
  } // end of method TypeEntryContainer::ToString

  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209b
    // Code size       0 (0x0)
  } // end of method TypeEntryContainer::.ctor

  .property /*17000006*/ instance callconv(8) string
          DisplayText()
  {
    .get instance string System.ServiceModel.Activities.Presentation.TypeEntryContainer/*0200000A*/::get_DisplayText() /* 06000030 */
    .set instance void System.ServiceModel.Activities.Presentation.TypeEntryContainer/*0200000A*/::set_DisplayText(string) /* 06000031 */
  } // end of property TypeEntryContainer::DisplayText
  .property /*17000007*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/>
          Items()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/> System.ServiceModel.Activities.Presentation.TypeEntryContainer/*0200000A*/::get_Items() /* 06000032 */
    .set instance void System.ServiceModel.Activities.Presentation.TypeEntryContainer/*0200000A*/::set_Items(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/>) /* 06000033 */
  } // end of property TypeEntryContainer::Items
} // end of class System.ServiceModel.Activities.Presentation.TypeEntryContainer

.class /*0200000B*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.ExpanderTypeEntry
       extends [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/
{
  .field /*04000009*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ NameProperty
  .field /*0400000A*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TypeToExpandProperty
  .field /*0400000B*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TagProperty
  .method /*06000036*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x209c
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::get_Name

  .method /*06000037*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x209d
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::set_Name

  .method /*06000038*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_TypeToExpand() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x209e
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::get_TypeToExpand

  .method /*06000039*/ public hidebysig specialname 
          instance void  set_TypeToExpand(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x209f
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::set_TypeToExpand

  .method /*0600003A*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/[] 
          get_TypeToExpandSource() cil managed
  // SIG: 20 00 1D 12 0D
  {
    // Method begins at RVA 0x20a0
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::get_TypeToExpandSource

  .method /*0600003B*/ public hidebysig specialname 
          instance object  get_Tag() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x20a1
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::get_Tag

  .method /*0600003C*/ public hidebysig specialname 
          instance void  set_Tag(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20a2
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::set_Tag

  .method /*0600003D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20a3
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::ToString

  .method /*0600003E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a4
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::.ctor

  .method /*0600003F*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20a5
    // Code size       0 (0x0)
  } // end of method ExpanderTypeEntry::.cctor

  .property /*17000008*/ instance callconv(8) string
          Name()
  {
    .get instance string System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/::get_Name() /* 06000036 */
    .set instance void System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/::set_Name(string) /* 06000037 */
  } // end of property ExpanderTypeEntry::Name
  .property /*17000009*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          TypeToExpand()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/::get_TypeToExpand() /* 06000038 */
    .set instance void System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/::set_TypeToExpand(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000039 */
  } // end of property ExpanderTypeEntry::TypeToExpand
  .property /*1700000A*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/[]
          TypeToExpandSource()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/[] System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/::get_TypeToExpandSource() /* 0600003A */
  } // end of property ExpanderTypeEntry::TypeToExpandSource
  .property /*1700000B*/ instance callconv(8) object
          Tag()
  {
    .get instance object System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/::get_Tag() /* 0600003B */
    .set instance void System.ServiceModel.Activities.Presentation.ExpanderTypeEntry/*0200000B*/::set_Tag(object) /* 0600003C */
  } // end of property ExpanderTypeEntry::Tag
} // end of class System.ServiceModel.Activities.Presentation.ExpanderTypeEntry

.class /*0200000C*/ public abstract auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.ServiceContractImporter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .class /*0200009F*/ auto ansi sealed nested private beforefieldinit '<GenerateActivityTemplates>d__9'
         extends [mscorlib/*23000001*/]System.Object/*01000002*/
         implements class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/>/*1B000002*/,
                    [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/,
                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/>/*1B000001*/,
                    [mscorlib/*23000001*/]System.IDisposable/*01000023*/,
                    [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/
  {
    .custom /*0C000070:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    .method /*06000476*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0001D6:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.IDisposable/*01000023*/::Dispose /*01000023::0A000005*/ 
      // Method begins at RVA 0x2562
      // Code size       0 (0x0)
    } // end of method '<GenerateActivityTemplates>d__9'::System.IDisposable.Dispose

    .method /*06000477*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::MoveNext /*01000024::0A000006*/ 
      // Method begins at RVA 0x2563
      // Code size       0 (0x0)
    } // end of method '<GenerateActivityTemplates>d__9'::MoveNext

    .method /*06000478*/ private hidebysig newslot specialname virtual final 
            instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/ 
            'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder>.get_Current'() cil managed
    // SIG: 20 00 12 80 81
    {
      .custom /*0C0001D7:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/>/*1B000001*/::get_Current() /* 0A000007 */
      // Method begins at RVA 0x2564
      // Code size       0 (0x0)
    } // end of method '<GenerateActivityTemplates>d__9'::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder>.get_Current'

    .method /*06000479*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0001D8:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::Reset /*01000024::0A000008*/ 
      // Method begins at RVA 0x2565
      // Code size       0 (0x0)
    } // end of method '<GenerateActivityTemplates>d__9'::System.Collections.IEnumerator.Reset

    .method /*0600047A*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C0001D9:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::get_Current /*01000024::0A000009*/ 
      // Method begins at RVA 0x2566
      // Code size       0 (0x0)
    } // end of method '<GenerateActivityTemplates>d__9'::System.Collections.IEnumerator.get_Current

    .method /*0600047B*/ private hidebysig newslot virtual final 
            instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/> 
            'System.Collections.Generic.IEnumerable<System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 95 01 12 80 81
    {
      .custom /*0C0001DA:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override  method instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<!0> class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/>/*1B000002*/::GetEnumerator() /* 0A00000A */
      // Method begins at RVA 0x2567
      // Code size       0 (0x0)
    } // end of method '<GenerateActivityTemplates>d__9'::'System.Collections.Generic.IEnumerable<System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder>.GetEnumerator'

    .method /*0600047C*/ private hidebysig newslot virtual final 
            instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 91
    {
      .custom /*0C0001DB:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/::GetEnumerator /*01000026::0A00000B*/ 
      // Method begins at RVA 0x2568
      // Code size       0 (0x0)
    } // end of method '<GenerateActivityTemplates>d__9'::System.Collections.IEnumerable.GetEnumerator

    .method /*0600047D*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2569
      // Code size       0 (0x0)
    } // end of method '<GenerateActivityTemplates>d__9'::.ctor

    .property /*17000130*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/
            'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder>.Current'()
    {
      .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/ System.ServiceModel.Activities.Presentation.ServiceContractImporter/*0200000C*//'<GenerateActivityTemplates>d__9'/*0200009F*/::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder>.get_Current'() /* 06000478 */
    } // end of property '<GenerateActivityTemplates>d__9'::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder>.Current'
    .property /*17000131*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object System.ServiceModel.Activities.Presentation.ServiceContractImporter/*0200000C*//'<GenerateActivityTemplates>d__9'/*0200009F*/::System.Collections.IEnumerator.get_Current() /* 0600047A */
    } // end of property '<GenerateActivityTemplates>d__9'::System.Collections.IEnumerator.Current
  } // end of class '<GenerateActivityTemplates>d__9'

  .field /*0400000C*/ public static literal string ContractTypeViewStateKey = "contractType"
  .method /*06000040*/ assembly hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> 
          get_FilterFunction() cil managed
  // SIG: 00 00 15 12 71 02 12 0D 02
  {
    // Method begins at RVA 0x20a6
    // Code size       0 (0x0)
  } // end of method ServiceContractImporter::get_FilterFunction

  .method /*06000041*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          SelectContractType(class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*0100001D*/ localAssemblyName,
                             class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [mscorlib/*23000001*/]System.Reflection.AssemblyName/*0100001D*/> referencedAssemblies,
                             class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ editingContext) cil managed
  // SIG: 00 03 12 0D 12 75 15 12 65 01 12 75 12 79
  {
    // Method begins at RVA 0x20a7
    // Code size       0 (0x0)
  } // end of method ServiceContractImporter::SelectContractType

  .method /*06000042*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/> 
          GenerateActivityTemplates(class [mscorlib/*23000001*/]System.Type/*01000003*/ contractType) cil managed
  // SIG: 00 01 15 12 7D 01 12 80 81 12 0D
  {
    .custom /*0C000032:0A000028*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*010000AA*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000028 */ = ( 01 00 63 53 79 73 74 65 6D 2E 53 65 72 76 69 63   // ..cSystem.Servic
                                                                                                                                                                                                                               65 4D 6F 64 65 6C 2E 41 63 74 69 76 69 74 69 65   // eModel.Activitie
                                                                                                                                                                                                                               73 2E 50 72 65 73 65 6E 74 61 74 69 6F 6E 2E 53   // s.Presentation.S
                                                                                                                                                                                                                               65 72 76 69 63 65 43 6F 6E 74 72 61 63 74 49 6D   // erviceContractIm
                                                                                                                                                                                                                               70 6F 72 74 65 72 2B 3C 47 65 6E 65 72 61 74 65   // porter+<Generate
                                                                                                                                                                                                                               41 63 74 69 76 69 74 79 54 65 6D 70 6C 61 74 65   // ActivityTemplate
                                                                                                                                                                                                                               73 3E 64 5F 5F 39 00 00 )                         // s>d__9..
    // Method begins at RVA 0x20a8
    // Code size       0 (0x0)
  } // end of method ServiceContractImporter::GenerateActivityTemplates

  .method /*06000043*/ public hidebysig static 
          string  SaveActivityTemplate(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Toolbox.ActivityTemplateFactoryBuilder/*01000020*/ activityTemplate) cil managed
  // SIG: 00 01 0E 12 80 81
  {
    // Method begins at RVA 0x20a9
    // Code size       0 (0x0)
  } // end of method ServiceContractImporter::SaveActivityTemplate

  .method /*06000044*/ assembly hidebysig static 
          class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ 
          GenerateActivity(class [System.ServiceModel/*23000008*/]System.ServiceModel.Description.OperationDescription/*01000022*/ operation) cil managed
  // SIG: 00 01 12 80 85 12 80 89
  {
    // Method begins at RVA 0x20aa
    // Code size       0 (0x0)
  } // end of method ServiceContractImporter::GenerateActivity

  .property /*1700000C*/ callconv(8) class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>
          FilterFunction()
  {
    .get class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> System.ServiceModel.Activities.Presentation.ServiceContractImporter/*0200000C*/::get_FilterFunction() /* 06000040 */
  } // end of property ServiceContractImporter::FilterFunction
} // end of class System.ServiceModel.Activities.Presentation.ServiceContractImporter

.class /*0200000D*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.EndpointDesigner
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*06000045*/ assembly hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x20ab
    // Code size       0 (0x0)
  } // end of method EndpointDesigner::RegisterMetadata

  .method /*06000046*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       0 (0x0)
  } // end of method EndpointDesigner::.ctor

} // end of class System.ServiceModel.Activities.Presentation.EndpointDesigner

.class /*0200000E*/ private auto ansi System.ServiceModel.Activities.Presentation.ReceiveDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*0400000D*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ CreateSendReplyCommand
  .field /*0400000E*/ assembly class System.ServiceModel.Activities.Presentation.ReceiveDesigner/*0200000E*/ receiveDesigner
  .field /*0400000F*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ contentGrid
  .field /*04000010*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.TextBox/*0100002A*/ txtOperationName
  .method /*06000047*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20b0
    // Code size       0 (0x0)
  } // end of method ReceiveDesigner::.cctor

  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20bc
    // Code size       0 (0x0)
  } // end of method ReceiveDesigner::.ctor

  .method /*06000049*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20bd
    // Code size       0 (0x0)
  } // end of method ReceiveDesigner::OnModelItemChanged

  .method /*0600004A*/ family hidebysig virtual 
          instance void  OnReadOnlyChanged(bool isReadOnly) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20be
    // Code size       0 (0x0)
  } // end of method ReceiveDesigner::OnReadOnlyChanged

  .method /*0600004B*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000036:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000037:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x20bf
    // Code size       0 (0x0)
  } // end of method ReceiveDesigner::InitializeComponent

  .method /*0600004C*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000038:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000039:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00003A:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x20c0
    // Code size       0 (0x0)
  } // end of method ReceiveDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.ServiceModel.Activities.Presentation.ReceiveDesigner

.class /*0200000F*/ private auto ansi System.ServiceModel.Activities.Presentation.ReceiveReplyDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000011*/ assembly class System.ServiceModel.Activities.Presentation.ReceiveReplyDesigner/*0200000F*/ receiveReplyDesigner
  .field /*04000012*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ contentGrid
  .method /*0600004D*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20c1
    // Code size       0 (0x0)
  } // end of method ReceiveReplyDesigner::.cctor

  .method /*0600004E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c2
    // Code size       0 (0x0)
  } // end of method ReceiveReplyDesigner::.ctor

  .method /*0600004F*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20c3
    // Code size       0 (0x0)
  } // end of method ReceiveReplyDesigner::OnModelItemChanged

  .method /*06000050*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00003B:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00003C:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x20c4
    // Code size       0 (0x0)
  } // end of method ReceiveReplyDesigner::InitializeComponent

  .method /*06000051*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00003D:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00003E:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00003F:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x20c5
    // Code size       0 (0x0)
  } // end of method ReceiveReplyDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.ServiceModel.Activities.Presentation.ReceiveReplyDesigner

.class /*02000010*/ private auto ansi System.ServiceModel.Activities.Presentation.SendDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000013*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ CreateReceiveReplyCommand
  .field /*04000014*/ assembly class System.ServiceModel.Activities.Presentation.SendDesigner/*02000010*/ sendDesigner
  .field /*04000015*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ contentGrid
  .field /*04000016*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.TextBox/*0100002A*/ txtOperationName
  .method /*06000052*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20c8
    // Code size       0 (0x0)
  } // end of method SendDesigner::.cctor

  .method /*06000053*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d4
    // Code size       0 (0x0)
  } // end of method SendDesigner::.ctor

  .method /*06000054*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20d5
    // Code size       0 (0x0)
  } // end of method SendDesigner::OnModelItemChanged

  .method /*06000055*/ family hidebysig virtual 
          instance void  OnReadOnlyChanged(bool isReadOnly) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20d6
    // Code size       0 (0x0)
  } // end of method SendDesigner::OnReadOnlyChanged

  .method /*06000056*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000040:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000041:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x20d7
    // Code size       0 (0x0)
  } // end of method SendDesigner::InitializeComponent

  .method /*06000057*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000042:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000043:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000044:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x20d8
    // Code size       0 (0x0)
  } // end of method SendDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.ServiceModel.Activities.Presentation.SendDesigner

.class /*02000011*/ private auto ansi System.ServiceModel.Activities.Presentation.SendReplyDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000017*/ assembly class System.ServiceModel.Activities.Presentation.SendReplyDesigner/*02000011*/ sendReplyDesigner
  .field /*04000018*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ contentGrid
  .method /*06000058*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20d9
    // Code size       0 (0x0)
  } // end of method SendReplyDesigner::.cctor

  .method /*06000059*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20da
    // Code size       0 (0x0)
  } // end of method SendReplyDesigner::.ctor

  .method /*0600005A*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20db
    // Code size       0 (0x0)
  } // end of method SendReplyDesigner::OnModelItemChanged

  .method /*0600005B*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000045:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000046:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x20dc
    // Code size       0 (0x0)
  } // end of method SendReplyDesigner::InitializeComponent

  .method /*0600005C*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000047:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000048:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000049:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x20dd
    // Code size       0 (0x0)
  } // end of method SendReplyDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.ServiceModel.Activities.Presentation.SendReplyDesigner

.class /*02000012*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.CorrelatesOnValueEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor/*0100002B*/
{
  .method /*0600005D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20de
    // Code size       0 (0x0)
  } // end of method CorrelatesOnValueEditor::.ctor

  .method /*0600005E*/ public hidebysig virtual 
          instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValue/*0100002C*/ propertyValue,
                                    class [PresentationCore/*23000009*/]System.Windows.IInputElement/*0100002D*/ commandSource) cil managed
  // SIG: 20 02 01 12 80 B1 12 80 B5
  {
    // Method begins at RVA 0x20df
    // Code size       0 (0x0)
  } // end of method CorrelatesOnValueEditor::ShowDialog

  .method /*0600005F*/ public hidebysig instance void 
          ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ activity,
                     class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 2D 12 79
  {
    // Method begins at RVA 0x20e0
    // Code size       0 (0x0)
  } // end of method CorrelatesOnValueEditor::ShowDialog

} // end of class System.ServiceModel.Activities.Presentation.CorrelatesOnValueEditor

.class /*02000013*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.MessageQueryEditor
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.ComboBox/*0100000D*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000019*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TypeCollectionProperty
  .field /*0400001A*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ QueryProperty
  .field /*0400001B*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.RoutedEvent/*01000016*/ XPathCreatedEvent
  .method /*06000060*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e1
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::.ctor

  .method /*06000061*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<string,class [mscorlib/*23000001*/]System.Type/*01000003*/>> 
          get_TypeCollection() cil managed
  // SIG: 20 00 15 12 65 01 15 11 80 B9 02 0E 12 0D
  {
    // Method begins at RVA 0x20e2
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::get_TypeCollection

  .method /*06000062*/ public hidebysig specialname 
          instance void  set_TypeCollection(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<string,class [mscorlib/*23000001*/]System.Type/*01000003*/>> 'value') cil managed
  // SIG: 20 01 01 15 12 65 01 15 11 80 B9 02 0E 12 0D
  {
    // Method begins at RVA 0x20e3
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::set_TypeCollection

  .method /*06000063*/ public hidebysig specialname 
          instance class [System.ServiceModel/*23000008*/]System.ServiceModel.XPathMessageQuery/*0100002F*/ 
          get_Query() cil managed
  // SIG: 20 00 12 80 BD
  {
    // Method begins at RVA 0x20e4
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::get_Query

  .method /*06000064*/ private hidebysig specialname 
          instance void  set_Query(class [System.ServiceModel/*23000008*/]System.ServiceModel.XPathMessageQuery/*0100002F*/ 'value') cil managed
  // SIG: 20 01 01 12 80 BD
  {
    // Method begins at RVA 0x20e5
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::set_Query

  .method /*06000065*/ assembly hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_Activity() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x20e6
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::get_Activity

  .method /*06000066*/ assembly hidebysig specialname 
          instance void  set_Activity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x20e7
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::set_Activity

  .method /*06000067*/ public hidebysig specialname 
          instance void  add_XPathCreated(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x20e8
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::add_XPathCreated

  .method /*06000068*/ public hidebysig specialname 
          instance void  remove_XPathCreated(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x20e9
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::remove_XPathCreated

  .method /*06000069*/ family hidebysig virtual 
          instance class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ 
          GetContainerForItemOverride() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x20ea
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::GetContainerForItemOverride

  .method /*0600006A*/ family hidebysig virtual 
          instance void  OnKeyDown(class [PresentationCore/*23000009*/]System.Windows.Input.KeyEventArgs/*01000030*/ e) cil managed
  // SIG: 20 01 01 12 80 C1
  {
    // Method begins at RVA 0x20eb
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::OnKeyDown

  .method /*0600006B*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00004C:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00004D:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x20ec
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::InitializeComponent

  .method /*0600006C*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00004E:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00004F:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000050:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x20ed
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600006D*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20ee
    // Code size       0 (0x0)
  } // end of method MessageQueryEditor::.cctor

  .event /*14000003*/ [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ XPathCreated
  {
    .addon instance void System.ServiceModel.Activities.Presentation.MessageQueryEditor/*02000013*/::add_XPathCreated(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 06000067 */
    .removeon instance void System.ServiceModel.Activities.Presentation.MessageQueryEditor/*02000013*/::remove_XPathCreated(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 06000068 */
  } // end of event MessageQueryEditor::XPathCreated
  .property /*1700000D*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<string,class [mscorlib/*23000001*/]System.Type/*01000003*/>>
          TypeCollection()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<string,class [mscorlib/*23000001*/]System.Type/*01000003*/>> System.ServiceModel.Activities.Presentation.MessageQueryEditor/*02000013*/::get_TypeCollection() /* 06000061 */
    .set instance void System.ServiceModel.Activities.Presentation.MessageQueryEditor/*02000013*/::set_TypeCollection(class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<string,class [mscorlib/*23000001*/]System.Type/*01000003*/>>) /* 06000062 */
  } // end of property MessageQueryEditor::TypeCollection
  .property /*1700000E*/ instance callconv(8) class [System.ServiceModel/*23000008*/]System.ServiceModel.XPathMessageQuery/*0100002F*/
          Query()
  {
    .get instance class [System.ServiceModel/*23000008*/]System.ServiceModel.XPathMessageQuery/*0100002F*/ System.ServiceModel.Activities.Presentation.MessageQueryEditor/*02000013*/::get_Query() /* 06000063 */
    .set instance void System.ServiceModel.Activities.Presentation.MessageQueryEditor/*02000013*/::set_Query(class [System.ServiceModel/*23000008*/]System.ServiceModel.XPathMessageQuery/*0100002F*/) /* 06000064 */
  } // end of property MessageQueryEditor::Query
  .property /*1700000F*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          Activity()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.MessageQueryEditor/*02000013*/::get_Activity() /* 06000065 */
    .set instance void System.ServiceModel.Activities.Presentation.MessageQueryEditor/*02000013*/::set_Activity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 06000066 */
  } // end of property MessageQueryEditor::Activity
} // end of class System.ServiceModel.Activities.Presentation.MessageQueryEditor

.class /*02000014*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.ContentControl/*01000013*/
       implements [System/*23000002*/]System.ComponentModel.INotifyPropertyChanged/*01000031*/,
                  [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .class /*020000A0*/ auto ansi sealed nested assembly beforefieldinit MessageQueryEntry
         extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.DesignObjectWrapper/*01000034*/
  {
    .field /*04000100*/ static assembly string KeyProperty
    .field /*04000101*/ static assembly string ValueProperty
    .method /*0600047E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x256a
      // Code size       0 (0x0)
    } // end of method MessageQueryEntry::.ctor

    .method /*0600047F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/,class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> entry) cil managed
    // SIG: 20 01 01 15 11 80 B9 02 12 2D 12 2D
    {
      // Method begins at RVA 0x256b
      // Code size       0 (0x0)
    } // end of method MessageQueryEntry::.ctor

    .method /*06000480*/ public hidebysig static 
            class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyDescriptorData/*01000035*/[] 
            InitializeTypeProperties() cil managed
    // SIG: 00 00 1D 12 80 D5
    {
      // Method begins at RVA 0x256c
      // Code size       0 (0x0)
    } // end of method MessageQueryEntry::InitializeTypeProperties

    .method /*06000481*/ assembly hidebysig 
            instance string  GetKey() cil managed
    // SIG: 20 00 0E
    {
      // Method begins at RVA 0x256d
      // Code size       0 (0x0)
    } // end of method MessageQueryEntry::GetKey

    .method /*06000482*/ family hidebysig specialname virtual 
            instance string  get_AutomationId() cil managed
    // SIG: 20 00 0E
    {
      // Method begins at RVA 0x256e
      // Code size       0 (0x0)
    } // end of method MessageQueryEntry::get_AutomationId

    .method /*06000483*/ private hidebysig specialname rtspecialname static 
            void  .cctor() cil managed
    // SIG: 00 00 01
    {
      // Method begins at RVA 0x256f
      // Code size       0 (0x0)
    } // end of method MessageQueryEntry::.cctor

    .property /*17000132*/ instance callconv(8) string
            AutomationId()
    {
      .get instance string System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*//MessageQueryEntry/*020000A0*/::get_AutomationId() /* 06000482 */
    } // end of property MessageQueryEntry::AutomationId
  } // end of class MessageQueryEntry

  .field /*0400001C*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ActivityProperty
  .field /*0400001D*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ MessageQuerySetContainerProperty
  .field /*0400001E*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ IsTypeExpanderEnabledProperty
  .field /*0400001F*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGrid/*01000033*/ messageQueries
  .method /*0600006E*/ public hidebysig newslot specialname virtual final 
          instance void  add_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    .custom /*0C000051:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::add_PropertyChanged

  .method /*0600006F*/ public hidebysig newslot specialname virtual final 
          instance void  remove_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    .custom /*0C000052:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f0
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::remove_PropertyChanged

  .method /*06000070*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f1
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::.ctor

  .method /*06000071*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_Activity() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x20f2
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::get_Activity

  .method /*06000072*/ public hidebysig specialname 
          instance void  set_Activity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x20f3
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::set_Activity

  .method /*06000073*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_MessageQuerySetContainer() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x20f4
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::get_MessageQuerySetContainer

  .method /*06000074*/ public hidebysig specialname 
          instance void  set_MessageQuerySetContainer(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x20f5
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::set_MessageQuerySetContainer

  .method /*06000075*/ public hidebysig specialname 
          instance bool  get_IsTypeExpanderEnabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f6
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::get_IsTypeExpanderEnabled

  .method /*06000076*/ private hidebysig specialname 
          instance void  set_IsTypeExpanderEnabled(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20f7
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::set_IsTypeExpanderEnabled

  .method /*06000077*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<string,class [mscorlib/*23000001*/]System.Type/*01000003*/>> 
          get_ActivityParameters() cil managed
  // SIG: 20 00 15 12 65 01 15 11 80 B9 02 0E 12 0D
  {
    // Method begins at RVA 0x20f8
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::get_ActivityParameters

  .method /*06000078*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x20f9
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::OnInitialized

  .method /*06000079*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000053:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000054:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x20fa
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::InitializeComponent

  .method /*0600007A*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000055:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000056:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000057:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x20fb
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600007B*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000058:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000059:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00005A:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x20fc
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*0600007C*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20fd
    // Code size       0 (0x0)
  } // end of method MessageQuerySetDesigner::.cctor

  .event /*14000004*/ [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ PropertyChanged
  {
    .addon instance void System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::add_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/) /* 0600006E */
    .removeon instance void System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::remove_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/) /* 0600006F */
  } // end of event MessageQuerySetDesigner::PropertyChanged
  .property /*17000010*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          Activity()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::get_Activity() /* 06000071 */
    .set instance void System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::set_Activity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 06000072 */
  } // end of property MessageQuerySetDesigner::Activity
  .property /*17000011*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          MessageQuerySetContainer()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::get_MessageQuerySetContainer() /* 06000073 */
    .set instance void System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::set_MessageQuerySetContainer(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 06000074 */
  } // end of property MessageQuerySetDesigner::MessageQuerySetContainer
  .property /*17000012*/ instance callconv(8) bool
          IsTypeExpanderEnabled()
  {
    .get instance bool System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::get_IsTypeExpanderEnabled() /* 06000075 */
    .set instance void System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::set_IsTypeExpanderEnabled(bool) /* 06000076 */
  } // end of property MessageQuerySetDesigner::IsTypeExpanderEnabled
  .property /*17000013*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<string,class [mscorlib/*23000001*/]System.Type/*01000003*/>>
          ActivityParameters()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<string,class [mscorlib/*23000001*/]System.Type/*01000003*/>> System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner/*02000014*/::get_ActivityParameters() /* 06000077 */
  } // end of property MessageQuerySetDesigner::ActivityParameters
} // end of class System.ServiceModel.Activities.Presentation.MessageQuerySetDesigner

.class /*02000015*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.CorrelationDataDesigner
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.ContentControl/*01000013*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*04000020*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ActivityProperty
  .field /*04000021*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ CorrelationInitializeDataProperty
  .field /*04000022*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ CorrelationHandleProperty
  .field /*04000023*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ AddNewDataCommand
  .field /*04000024*/ assembly class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.ExpressionTextBox/*01000036*/ correlationHandleETB
  .field /*04000025*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGrid/*01000033*/ correlationInitializers
  .method /*0600007D*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_Activity() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x20fe
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::get_Activity

  .method /*0600007E*/ public hidebysig specialname 
          instance void  set_Activity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x20ff
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::set_Activity

  .method /*0600007F*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/> 
          get_CorrelationInitializeData() cil managed
  // SIG: 20 00 15 12 80 DD 01 12 58
  {
    // Method begins at RVA 0x2100
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::get_CorrelationInitializeData

  .method /*06000080*/ public hidebysig specialname 
          instance void  set_CorrelationInitializeData(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 DD 01 12 58
  {
    // Method begins at RVA 0x2101
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::set_CorrelationInitializeData

  .method /*06000081*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_CorrelationHandle() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x2102
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::get_CorrelationHandle

  .method /*06000082*/ public hidebysig specialname 
          instance void  set_CorrelationHandle(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x2103
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::set_CorrelationHandle

  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2104
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::.ctor

  .method /*06000084*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x2105
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::OnInitialized

  .method /*06000085*/ assembly hidebysig 
          instance void  CommitEdit() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2106
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::CommitEdit

  .method /*06000086*/ assembly hidebysig 
          instance void  ValidateKey(class System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/ wrapper,
                                     string oldKey) cil managed
  // SIG: 20 02 01 12 58 0E
  {
    // Method begins at RVA 0x2107
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::ValidateKey

  .method /*06000087*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00005B:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00005C:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2108
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::InitializeComponent

  .method /*06000088*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00005D:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00005E:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00005F:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2109
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*06000089*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000060:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000061:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000062:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x210a
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*0600008A*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x210b
    // Code size       0 (0x0)
  } // end of method CorrelationDataDesigner::.cctor

  .property /*17000014*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          Activity()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/::get_Activity() /* 0600007D */
    .set instance void System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/::set_Activity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 0600007E */
  } // end of property CorrelationDataDesigner::Activity
  .property /*17000015*/ instance callconv(8) class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/>
          CorrelationInitializeData()
  {
    .get instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/> System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/::get_CorrelationInitializeData() /* 0600007F */
    .set instance void System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/::set_CorrelationInitializeData(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/>) /* 06000080 */
  } // end of property CorrelationDataDesigner::CorrelationInitializeData
  .property /*17000016*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          CorrelationHandle()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/::get_CorrelationHandle() /* 06000081 */
    .set instance void System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/::set_CorrelationHandle(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 06000082 */
  } // end of property CorrelationDataDesigner::CorrelationHandle
} // end of class System.ServiceModel.Activities.Presentation.CorrelationDataDesigner

.class /*02000016*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.CorrelationDataWrapper
       extends [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/
{
  .field /*04000026*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ KeyProperty
  .field /*04000027*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ValueProperty
  .method /*0600008B*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_Value() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x210c
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::get_Value

  .method /*0600008C*/ public hidebysig specialname 
          instance void  set_Value(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x210d
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::set_Value

  .method /*0600008D*/ public hidebysig specialname 
          instance string  get_Key() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x210e
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::get_Key

  .method /*0600008E*/ public hidebysig specialname 
          instance void  set_Key(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x210f
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::set_Key

  .method /*0600008F*/ public hidebysig specialname static 
          class System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/ 
          get_Editor() cil managed
  // SIG: 00 00 12 54
  {
    .custom /*0C000063:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::get_Editor

  .method /*06000090*/ public hidebysig specialname static 
          void  set_Editor(class System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/ 'value') cil managed
  // SIG: 00 01 01 12 54
  {
    .custom /*0C000064:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2111
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::set_Editor

  .method /*06000091*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2112
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::.ctor

  .method /*06000092*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(string key,
                               class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 02 01 0E 12 2D
  {
    // Method begins at RVA 0x2113
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::.ctor

  .method /*06000093*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2114
    // Code size       0 (0x0)
  } // end of method CorrelationDataWrapper::.cctor

  .property /*17000017*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          Value()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/::get_Value() /* 0600008B */
    .set instance void System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/::set_Value(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 0600008C */
  } // end of property CorrelationDataWrapper::Value
  .property /*17000018*/ instance callconv(8) string
          Key()
  {
    .get instance string System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/::get_Key() /* 0600008D */
    .set instance void System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/::set_Key(string) /* 0600008E */
  } // end of property CorrelationDataWrapper::Key
  .property /*17000019*/ callconv(8) class System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/
          Editor()
  {
    .get class System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/ System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/::get_Editor() /* 0600008F */
    .set void System.ServiceModel.Activities.Presentation.CorrelationDataWrapper/*02000016*/::set_Editor(class System.ServiceModel.Activities.Presentation.CorrelationDataDesigner/*02000015*/) /* 06000090 */
  } // end of property CorrelationDataWrapper::Editor
} // end of class System.ServiceModel.Activities.Presentation.CorrelationDataWrapper

.class /*02000017*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.CorrelationInitializerDesigner
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.ContentControl/*01000013*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .class /*020000A1*/ auto ansi sealed nested assembly beforefieldinit CorrelationInitializerEntry
         extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.DesignObjectWrapper/*01000034*/
  {
    .field /*04000102*/ static assembly initonly string HandleProperty
    .field /*04000103*/ static assembly initonly string CorrelationTypeProperty
    .field /*04000104*/ static assembly initonly string MessageQuerySetModelPropertyProperty
    .method /*06000484*/ public hidebysig static 
            class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyDescriptorData/*01000035*/[] 
            InitializeTypeProperties() cil managed
    // SIG: 00 00 1D 12 80 D5
    {
      // Method begins at RVA 0x2570
      // Code size       0 (0x0)
    } // end of method CorrelationInitializerEntry::InitializeTypeProperties

    .method /*06000485*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2571
      // Code size       0 (0x0)
    } // end of method CorrelationInitializerEntry::.ctor

    .method /*06000486*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ initializer) cil managed
    // SIG: 20 01 01 12 2D
    {
      // Method begins at RVA 0x2572
      // Code size       0 (0x0)
    } // end of method CorrelationInitializerEntry::.ctor

    .method /*06000487*/ family hidebysig specialname virtual 
            instance string  get_AutomationId() cil managed
    // SIG: 20 00 0E
    {
      // Method begins at RVA 0x2573
      // Code size       0 (0x0)
    } // end of method CorrelationInitializerEntry::get_AutomationId

    .method /*06000488*/ assembly hidebysig 
            instance object  GetHandle() cil managed
    // SIG: 20 00 1C
    {
      // Method begins at RVA 0x2574
      // Code size       0 (0x0)
    } // end of method CorrelationInitializerEntry::GetHandle

    .method /*06000489*/ assembly hidebysig 
            instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
            GetCorrelationType() cil managed
    // SIG: 20 00 12 0D
    {
      // Method begins at RVA 0x2575
      // Code size       0 (0x0)
    } // end of method CorrelationInitializerEntry::GetCorrelationType

    .method /*0600048A*/ assembly hidebysig 
            instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ 
            GetMessageQuerySetModelProperty() cil managed
    // SIG: 20 00 12 80 E1
    {
      // Method begins at RVA 0x2576
      // Code size       0 (0x0)
    } // end of method CorrelationInitializerEntry::GetMessageQuerySetModelProperty

    .method /*0600048B*/ private hidebysig specialname rtspecialname static 
            void  .cctor() cil managed
    // SIG: 00 00 01
    {
      // Method begins at RVA 0x2577
      // Code size       0 (0x0)
    } // end of method CorrelationInitializerEntry::.cctor

    .property /*17000133*/ instance callconv(8) string
            AutomationId()
    {
      .get instance string System.ServiceModel.Activities.Presentation.CorrelationInitializerDesigner/*02000017*//CorrelationInitializerEntry/*020000A1*/::get_AutomationId() /* 06000487 */
    } // end of property CorrelationInitializerEntry::AutomationId
  } // end of class CorrelationInitializerEntry

  .field /*04000028*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ActivityProperty
  .field /*04000029*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGrid/*01000033*/ correlationInitializers
  .field /*0400002A*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.ComboBox/*0100000D*/ typeCombo
  .method /*06000094*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2115
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::.ctor

  .method /*06000095*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_Activity() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x2116
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::get_Activity

  .method /*06000096*/ public hidebysig specialname 
          instance void  set_Activity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x2117
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::set_Activity

  .method /*06000097*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ args) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x2118
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::OnInitialized

  .method /*06000098*/ assembly hidebysig 
          instance void  CleanupObjectMap() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2119
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::CleanupObjectMap

  .method /*06000099*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000066:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000067:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x211a
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::InitializeComponent

  .method /*0600009A*/ assembly hidebysig 
          instance class [mscorlib/*23000001*/]System.Delegate/*01000006*/ 
          _CreateDelegate(class [mscorlib/*23000001*/]System.Type/*01000003*/ delegateType,
                          string 'handler') cil managed
  // SIG: 20 02 12 19 12 0D 0E
  {
    .custom /*0C000068:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000069:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x211b
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::_CreateDelegate

  .method /*0600009B*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00006A:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00006B:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00006C:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x211c
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600009C*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00006D:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00006E:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00006F:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x211d
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*0600009D*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x211e
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerDesigner::.cctor

  .property /*1700001A*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          Activity()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.CorrelationInitializerDesigner/*02000017*/::get_Activity() /* 06000095 */
    .set instance void System.ServiceModel.Activities.Presentation.CorrelationInitializerDesigner/*02000017*/::set_Activity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 06000096 */
  } // end of property CorrelationInitializerDesigner::Activity
} // end of class System.ServiceModel.Activities.Presentation.CorrelationInitializerDesigner

.class /*02000018*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.CorrelationInitializerValueEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor/*0100002B*/
{
  .method /*0600009E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x211f
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerValueEditor::.ctor

  .method /*0600009F*/ public hidebysig virtual 
          instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValue/*0100002C*/ propertyValue,
                                    class [PresentationCore/*23000009*/]System.Windows.IInputElement/*0100002D*/ commandSource) cil managed
  // SIG: 20 02 01 12 80 B1 12 80 B5
  {
    // Method begins at RVA 0x2120
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerValueEditor::ShowDialog

  .method /*060000A0*/ public hidebysig instance void 
          ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem,
                     class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 2D 12 79
  {
    // Method begins at RVA 0x2121
    // Code size       0 (0x0)
  } // end of method CorrelationInitializerValueEditor::ShowDialog

} // end of class System.ServiceModel.Activities.Presentation.CorrelationInitializerValueEditor

.class /*02000019*/ private auto ansi System.ServiceModel.Activities.Presentation.InitializeCorrelationDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .class /*020000A2*/ auto ansi sealed nested assembly beforefieldinit CorrelationDataValueEditor
         extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor/*0100002B*/
  {
    .method /*0600048C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2578
      // Code size       0 (0x0)
    } // end of method CorrelationDataValueEditor::.ctor

    .method /*0600048D*/ public hidebysig virtual 
            instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValue/*0100002C*/ propertyValue,
                                      class [PresentationCore/*23000009*/]System.Windows.IInputElement/*0100002D*/ commandSource) cil managed
    // SIG: 20 02 01 12 80 B1 12 80 B5
    {
      // Method begins at RVA 0x2579
      // Code size       0 (0x0)
    } // end of method CorrelationDataValueEditor::ShowDialog

    .method /*0600048E*/ public hidebysig 
            instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem,
                                      class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context) cil managed
    // SIG: 20 02 01 12 2D 12 79
    {
      // Method begins at RVA 0x257a
      // Code size       0 (0x0)
    } // end of method CorrelationDataValueEditor::ShowDialog

  } // end of class CorrelationDataValueEditor

  .field /*0400002B*/ public static literal string CorrelationPropertyName = "Correlation"
  .field /*0400002C*/ public static literal string CorrelationDataPropertyName = "CorrelationData"
  .field /*0400002D*/ assembly class System.ServiceModel.Activities.Presentation.InitializeCorrelationDesigner/*02000019*/ initializeCorrelationDesigner
  .field /*0400002E*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Button/*01000039*/ btnCorrelationData
  .method /*060000A1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2122
    // Code size       0 (0x0)
  } // end of method InitializeCorrelationDesigner::.ctor

  .method /*060000A2*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2123
    // Code size       0 (0x0)
  } // end of method InitializeCorrelationDesigner::.cctor

  .method /*060000A3*/ family hidebysig virtual 
          instance void  OnReadOnlyChanged(bool isReadOnly) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2124
    // Code size       0 (0x0)
  } // end of method InitializeCorrelationDesigner::OnReadOnlyChanged

  .method /*060000A4*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2125
    // Code size       0 (0x0)
  } // end of method InitializeCorrelationDesigner::OnModelItemChanged

  .method /*060000A5*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000073:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000074:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2126
    // Code size       0 (0x0)
  } // end of method InitializeCorrelationDesigner::InitializeComponent

  .method /*060000A6*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000076:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000077:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000078:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2127
    // Code size       0 (0x0)
  } // end of method InitializeCorrelationDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.ServiceModel.Activities.Presentation.InitializeCorrelationDesigner

.class /*0200001A*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2<.ctor TMessage,.ctor TParameter>
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System/*23000002*/]System.ComponentModel.INotifyPropertyChanged/*01000031*/
{
  .method /*060000A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem) cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x2128
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::.ctor

  .method /*060000A8*/ public hidebysig newslot specialname virtual final 
          instance void  add_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    .custom /*0C000079:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2129
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::add_PropertyChanged

  .method /*060000A9*/ public hidebysig newslot specialname virtual final 
          instance void  remove_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    .custom /*0C00007A:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x212a
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::remove_PropertyChanged

  .method /*060000AA*/ public hidebysig specialname 
          instance bool  get_IsEditingEnabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x212b
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_IsEditingEnabled

  .method /*060000AB*/ public hidebysig specialname 
          instance bool  get_IsMessageChecked() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x212c
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_IsMessageChecked

  .method /*060000AC*/ public hidebysig specialname 
          instance void  set_IsMessageChecked(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x212d
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::set_IsMessageChecked

  .method /*060000AD*/ public hidebysig specialname 
          instance bool  get_IsParameterChecked() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x212e
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_IsParameterChecked

  .method /*060000AE*/ public hidebysig specialname 
          instance void  set_IsParameterChecked(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x212f
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::set_IsParameterChecked

  .method /*060000AF*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_ModelItem() cil managed
  // SIG: 20 00 12 2D
  {
    .custom /*0C00007B:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2130
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_ModelItem

  .method /*060000B0*/ private hidebysig specialname 
          instance void  set_ModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    .custom /*0C00007C:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2131
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::set_ModelItem

  .method /*060000B1*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 
          get_Context() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x2132
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_Context

  .method /*060000B2*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_MessageExpression() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x2133
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_MessageExpression

  .method /*060000B3*/ public hidebysig specialname 
          instance void  set_MessageExpression(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x2134
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::set_MessageExpression

  .method /*060000B4*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_DeclaredMessageType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x2135
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_DeclaredMessageType

  .method /*060000B5*/ public hidebysig specialname 
          instance void  set_DeclaredMessageType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x2136
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::set_DeclaredMessageType

  .method /*060000B6*/ public hidebysig specialname 
          instance bool  get_IsDictionary() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00007D:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_IsDictionary

  .method /*060000B7*/ private hidebysig specialname 
          instance void  set_IsDictionary(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00007E:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2138
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::set_IsDictionary

  .method /*060000B8*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_UnderlyingArgumentType() cil managed
  // SIG: 20 00 12 0D
  {
    .custom /*0C00007F:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2139
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_UnderlyingArgumentType

  .method /*060000B9*/ private hidebysig specialname 
          instance void  set_UnderlyingArgumentType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    .custom /*0C000080:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::set_UnderlyingArgumentType

  .method /*060000BA*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/> 
          get_DynamicArguments() cil managed
  // SIG: 20 00 15 12 80 DD 01 12 80 C0
  {
    .custom /*0C000081:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x213b
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::get_DynamicArguments

  .method /*060000BB*/ public hidebysig specialname 
          instance void  set_DynamicArguments(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 DD 01 12 80 C0
  {
    .custom /*0C000082:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x213c
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::set_DynamicArguments

  .method /*060000BC*/ assembly hidebysig 
          instance bool  OnOk() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x213d
    // Code size       0 (0x0)
  } // end of method ContentDialogViewModel`2::OnOk

  .event /*14000005*/ [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ PropertyChanged
  {
    .addon instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::add_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/) /* 060000A8 */
    .removeon instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::remove_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/) /* 060000A9 */
  } // end of event ContentDialogViewModel`2::PropertyChanged
  .property /*1700001B*/ instance callconv(8) bool
          IsEditingEnabled()
  {
    .get instance bool System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_IsEditingEnabled() /* 060000AA */
  } // end of property ContentDialogViewModel`2::IsEditingEnabled
  .property /*1700001C*/ instance callconv(8) bool
          IsMessageChecked()
  {
    .get instance bool System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_IsMessageChecked() /* 060000AB */
    .set instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::set_IsMessageChecked(bool) /* 060000AC */
  } // end of property ContentDialogViewModel`2::IsMessageChecked
  .property /*1700001D*/ instance callconv(8) bool
          IsParameterChecked()
  {
    .get instance bool System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_IsParameterChecked() /* 060000AD */
    .set instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::set_IsParameterChecked(bool) /* 060000AE */
  } // end of property ContentDialogViewModel`2::IsParameterChecked
  .property /*1700001E*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          ModelItem()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_ModelItem() /* 060000AF */
    .set instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::set_ModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 060000B0 */
  } // end of property ContentDialogViewModel`2::ModelItem
  .property /*1700001F*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/
          Context()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_Context() /* 060000B1 */
  } // end of property ContentDialogViewModel`2::Context
  .property /*17000020*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          MessageExpression()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_MessageExpression() /* 060000B2 */
    .set instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::set_MessageExpression(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 060000B3 */
  } // end of property ContentDialogViewModel`2::MessageExpression
  .property /*17000021*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          DeclaredMessageType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_DeclaredMessageType() /* 060000B4 */
    .set instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::set_DeclaredMessageType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 060000B5 */
  } // end of property ContentDialogViewModel`2::DeclaredMessageType
  .property /*17000022*/ instance callconv(8) bool
          IsDictionary()
  {
    .get instance bool System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_IsDictionary() /* 060000B6 */
    .set instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::set_IsDictionary(bool) /* 060000B7 */
  } // end of property ContentDialogViewModel`2::IsDictionary
  .property /*17000023*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          UnderlyingArgumentType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_UnderlyingArgumentType() /* 060000B8 */
    .set instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::set_UnderlyingArgumentType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 060000B9 */
  } // end of property ContentDialogViewModel`2::UnderlyingArgumentType
  .property /*17000024*/ instance callconv(8) class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/>
          DynamicArguments()
  {
    .get instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/> System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::get_DynamicArguments() /* 060000BA */
    .set instance void System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/::set_DynamicArguments(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/>) /* 060000BB */
  } // end of property ContentDialogViewModel`2::DynamicArguments
} // end of class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2

.class /*0200001B*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.MessagingContentPropertyEditorResources
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*060000BD*/ assembly hidebysig static 
          class [PresentationFramework/*23000006*/]System.Windows.ResourceDictionary/*0100003A*/ 
          GetResources() cil managed
  // SIG: 00 00 12 80 E9
  {
    // Method begins at RVA 0x213e
    // Code size       0 (0x0)
  } // end of method MessagingContentPropertyEditorResources::GetResources

  .method /*060000BE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213f
    // Code size       0 (0x0)
  } // end of method MessagingContentPropertyEditorResources::.ctor

} // end of class System.ServiceModel.Activities.Presentation.MessagingContentPropertyEditorResources

.class /*0200001C*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.ReceiveContentDialog
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*0400002F*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ViewModelProperty
  .method /*060000BF*/ private hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2140
    // Code size       0 (0x0)
  } // end of method ReceiveContentDialog::.ctor

  .method /*060000C0*/ public hidebysig specialname 
          instance class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveMessageContent/*0100003C*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveParametersContent/*0100003D*/> 
          get_ViewModel() cil managed
  // SIG: 20 00 15 12 68 02 12 80 F1 12 80 F5
  {
    // Method begins at RVA 0x2141
    // Code size       0 (0x0)
  } // end of method ReceiveContentDialog::get_ViewModel

  .method /*060000C1*/ public hidebysig specialname 
          instance void  set_ViewModel(class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveMessageContent/*0100003C*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveParametersContent/*0100003D*/> 'value') cil managed
  // SIG: 20 01 01 15 12 68 02 12 80 F1 12 80 F5
  {
    // Method begins at RVA 0x2142
    // Code size       0 (0x0)
  } // end of method ReceiveContentDialog::set_ViewModel

  .method /*060000C2*/ public hidebysig static 
          bool  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ activity,
                           class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context,
                           class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ owner) cil managed
  // SIG: 00 03 02 12 2D 12 79 12 6D
  {
    // Method begins at RVA 0x2143
    // Code size       0 (0x0)
  } // end of method ReceiveContentDialog::ShowDialog

  .method /*060000C3*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000083:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000084:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2144
    // Code size       0 (0x0)
  } // end of method ReceiveContentDialog::InitializeComponent

  .method /*060000C4*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000085:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000086:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000087:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2145
    // Code size       0 (0x0)
  } // end of method ReceiveContentDialog::System.Windows.Markup.IComponentConnector.Connect

  .method /*060000C5*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000088:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000089:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00008A:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x2146
    // Code size       0 (0x0)
  } // end of method ReceiveContentDialog::System.Windows.Markup.IStyleConnector.Connect

  .method /*060000C6*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2147
    // Code size       0 (0x0)
  } // end of method ReceiveContentDialog::.cctor

  .property /*17000025*/ instance callconv(8) class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveMessageContent/*0100003C*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveParametersContent/*0100003D*/>
          ViewModel()
  {
    .get instance class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveMessageContent/*0100003C*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveParametersContent/*0100003D*/> System.ServiceModel.Activities.Presentation.ReceiveContentDialog/*0200001C*/::get_ViewModel() /* 060000C0 */
    .set instance void System.ServiceModel.Activities.Presentation.ReceiveContentDialog/*0200001C*/::set_ViewModel(class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveMessageContent/*0100003C*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.ReceiveParametersContent/*0100003D*/>) /* 060000C1 */
  } // end of property ReceiveContentDialog::ViewModel
} // end of class System.ServiceModel.Activities.Presentation.ReceiveContentDialog

.class /*0200001D*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.ReceiveContentPropertyEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor/*0100002B*/
{
  .method /*060000C7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2148
    // Code size       0 (0x0)
  } // end of method ReceiveContentPropertyEditor::.ctor

  .method /*060000C8*/ public hidebysig virtual 
          instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValue/*0100002C*/ propertyValue,
                                    class [PresentationCore/*23000009*/]System.Windows.IInputElement/*0100002D*/ commandSource) cil managed
  // SIG: 20 02 01 12 80 B1 12 80 B5
  {
    // Method begins at RVA 0x2149
    // Code size       0 (0x0)
  } // end of method ReceiveContentPropertyEditor::ShowDialog

} // end of class System.ServiceModel.Activities.Presentation.ReceiveContentPropertyEditor

.class /*0200001E*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.ReceiveReplyValidationFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.DesignTimeValidationFeature/*0100003E*/
{
  .method /*060000C9*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ApplyTo() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x214a
    // Code size       0 (0x0)
  } // end of method ReceiveReplyValidationFeature::get_ApplyTo

  .method /*060000CA*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/> 
          get_DesignTimeConstraints() cil managed
  // SIG: 20 00 15 12 65 01 12 80 FD
  {
    // Method begins at RVA 0x214b
    // Code size       0 (0x0)
  } // end of method ReceiveReplyValidationFeature::get_DesignTimeConstraints

  .method /*060000CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214c
    // Code size       0 (0x0)
  } // end of method ReceiveReplyValidationFeature::.ctor

  .property /*17000026*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ApplyTo()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.ServiceModel.Activities.Presentation.ReceiveReplyValidationFeature/*0200001E*/::get_ApplyTo() /* 060000C9 */
  } // end of property ReceiveReplyValidationFeature::ApplyTo
  .property /*17000027*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/>
          DesignTimeConstraints()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/> System.ServiceModel.Activities.Presentation.ReceiveReplyValidationFeature/*0200001E*/::get_DesignTimeConstraints() /* 060000CA */
  } // end of property ReceiveReplyValidationFeature::DesignTimeConstraints
} // end of class System.ServiceModel.Activities.Presentation.ReceiveReplyValidationFeature

.class /*0200001F*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.SendContentDialog
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*04000030*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ViewModelProperty
  .method /*060000CC*/ private hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214d
    // Code size       0 (0x0)
  } // end of method SendContentDialog::.ctor

  .method /*060000CD*/ public hidebysig specialname 
          instance class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendMessageContent/*01000040*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendParametersContent/*01000041*/> 
          get_ViewModel() cil managed
  // SIG: 20 00 15 12 68 02 12 81 01 12 81 05
  {
    // Method begins at RVA 0x214e
    // Code size       0 (0x0)
  } // end of method SendContentDialog::get_ViewModel

  .method /*060000CE*/ public hidebysig specialname 
          instance void  set_ViewModel(class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendMessageContent/*01000040*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendParametersContent/*01000041*/> 'value') cil managed
  // SIG: 20 01 01 15 12 68 02 12 81 01 12 81 05
  {
    // Method begins at RVA 0x214f
    // Code size       0 (0x0)
  } // end of method SendContentDialog::set_ViewModel

  .method /*060000CF*/ public hidebysig static 
          bool  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ activity,
                           class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context,
                           class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ owner) cil managed
  // SIG: 00 03 02 12 2D 12 79 12 6D
  {
    // Method begins at RVA 0x2150
    // Code size       0 (0x0)
  } // end of method SendContentDialog::ShowDialog

  .method /*060000D0*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00008B:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00008C:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2151
    // Code size       0 (0x0)
  } // end of method SendContentDialog::InitializeComponent

  .method /*060000D1*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00008D:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00008E:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00008F:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2152
    // Code size       0 (0x0)
  } // end of method SendContentDialog::System.Windows.Markup.IComponentConnector.Connect

  .method /*060000D2*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000090:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000091:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000092:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x2153
    // Code size       0 (0x0)
  } // end of method SendContentDialog::System.Windows.Markup.IStyleConnector.Connect

  .method /*060000D3*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2154
    // Code size       0 (0x0)
  } // end of method SendContentDialog::.cctor

  .property /*17000028*/ instance callconv(8) class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendMessageContent/*01000040*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendParametersContent/*01000041*/>
          ViewModel()
  {
    .get instance class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendMessageContent/*01000040*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendParametersContent/*01000041*/> System.ServiceModel.Activities.Presentation.SendContentDialog/*0200001F*/::get_ViewModel() /* 060000CD */
    .set instance void System.ServiceModel.Activities.Presentation.SendContentDialog/*0200001F*/::set_ViewModel(class System.ServiceModel.Activities.Presentation.ContentDialogViewModel`2/*0200001A*/<class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendMessageContent/*01000040*/,class [System.ServiceModel.Activities/*2300000C*/]System.ServiceModel.Activities.SendParametersContent/*01000041*/>) /* 060000CE */
  } // end of property SendContentDialog::ViewModel
} // end of class System.ServiceModel.Activities.Presentation.SendContentDialog

.class /*02000020*/ private auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.SendContentPropertyEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor/*0100002B*/
{
  .method /*060000D4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2155
    // Code size       0 (0x0)
  } // end of method SendContentPropertyEditor::.ctor

  .method /*060000D5*/ public hidebysig virtual 
          instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValue/*0100002C*/ propertyValue,
                                    class [PresentationCore/*23000009*/]System.Windows.IInputElement/*0100002D*/ commandSource) cil managed
  // SIG: 20 02 01 12 80 B1 12 80 B5
  {
    // Method begins at RVA 0x2156
    // Code size       0 (0x0)
  } // end of method SendContentPropertyEditor::ShowDialog

} // end of class System.ServiceModel.Activities.Presentation.SendContentPropertyEditor

.class /*02000021*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.SendReplyValidationFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.DesignTimeValidationFeature/*0100003E*/
{
  .method /*060000D6*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ApplyTo() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x2157
    // Code size       0 (0x0)
  } // end of method SendReplyValidationFeature::get_ApplyTo

  .method /*060000D7*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/> 
          get_DesignTimeConstraints() cil managed
  // SIG: 20 00 15 12 65 01 12 80 FD
  {
    // Method begins at RVA 0x2158
    // Code size       0 (0x0)
  } // end of method SendReplyValidationFeature::get_DesignTimeConstraints

  .method /*060000D8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2159
    // Code size       0 (0x0)
  } // end of method SendReplyValidationFeature::.ctor

  .property /*17000029*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ApplyTo()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.ServiceModel.Activities.Presentation.SendReplyValidationFeature/*02000021*/::get_ApplyTo() /* 060000D6 */
  } // end of property SendReplyValidationFeature::ApplyTo
  .property /*1700002A*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/>
          DesignTimeConstraints()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/> System.ServiceModel.Activities.Presentation.SendReplyValidationFeature/*02000021*/::get_DesignTimeConstraints() /* 060000D7 */
  } // end of property SendReplyValidationFeature::DesignTimeConstraints
} // end of class System.ServiceModel.Activities.Presentation.SendReplyValidationFeature

.class /*02000022*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.TransactedReceiveScopeDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*060000D9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x215a
    // Code size       0 (0x0)
  } // end of method TransactedReceiveScopeDesigner::.ctor

  .method /*060000DA*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x215b
    // Code size       0 (0x0)
  } // end of method TransactedReceiveScopeDesigner::RegisterMetadata

  .method /*060000DB*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000093:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000094:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x215c
    // Code size       0 (0x0)
  } // end of method TransactedReceiveScopeDesigner::InitializeComponent

  .method /*060000DC*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000095:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000096:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000097:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x215d
    // Code size       0 (0x0)
  } // end of method TransactedReceiveScopeDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.ServiceModel.Activities.Presentation.TransactedReceiveScopeDesigner

.class /*02000023*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.CorrelationScopeDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000031*/ assembly class System.ServiceModel.Activities.Presentation.CorrelationScopeDesigner/*02000023*/ correlationScopeDesigner
  .method /*060000DD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x215e
    // Code size       0 (0x0)
  } // end of method CorrelationScopeDesigner::.ctor

  .method /*060000DE*/ assembly hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x215f
    // Code size       0 (0x0)
  } // end of method CorrelationScopeDesigner::RegisterMetadata

  .method /*060000DF*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000098:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000099:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2160
    // Code size       0 (0x0)
  } // end of method CorrelationScopeDesigner::InitializeComponent

  .method /*060000E0*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00009A:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00009B:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00009C:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2161
    // Code size       0 (0x0)
  } // end of method CorrelationScopeDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.ServiceModel.Activities.Presentation.CorrelationScopeDesigner

.class /*02000024*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.Converters.SendMessageContentSearchableStringConverter
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Converters.SearchableStringConverter/*01000042*/
{
  .method /*060000E1*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<string> 
          Convert(object 'value') cil managed
  // SIG: 20 01 15 12 65 01 0E 1C
  {
    // Method begins at RVA 0x2162
    // Code size       0 (0x0)
  } // end of method SendMessageContentSearchableStringConverter::Convert

  .method /*060000E2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2163
    // Code size       0 (0x0)
  } // end of method SendMessageContentSearchableStringConverter::.ctor

} // end of class System.ServiceModel.Activities.Presentation.Converters.SendMessageContentSearchableStringConverter

.class /*02000025*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.Converters.SendParametersContentSearchableStringConverter
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Converters.SearchableStringConverter/*01000042*/
{
  .method /*060000E3*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<string> 
          Convert(object 'value') cil managed
  // SIG: 20 01 15 12 65 01 0E 1C
  {
    // Method begins at RVA 0x2164
    // Code size       0 (0x0)
  } // end of method SendParametersContentSearchableStringConverter::Convert

  .method /*060000E4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2165
    // Code size       0 (0x0)
  } // end of method SendParametersContentSearchableStringConverter::.ctor

} // end of class System.ServiceModel.Activities.Presentation.Converters.SendParametersContentSearchableStringConverter

.class /*02000026*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.Converters.ReceiveMessageContentSearchableStringConverter
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Converters.SearchableStringConverter/*01000042*/
{
  .method /*060000E5*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<string> 
          Convert(object 'value') cil managed
  // SIG: 20 01 15 12 65 01 0E 1C
  {
    // Method begins at RVA 0x2166
    // Code size       0 (0x0)
  } // end of method ReceiveMessageContentSearchableStringConverter::Convert

  .method /*060000E6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2167
    // Code size       0 (0x0)
  } // end of method ReceiveMessageContentSearchableStringConverter::.ctor

} // end of class System.ServiceModel.Activities.Presentation.Converters.ReceiveMessageContentSearchableStringConverter

.class /*02000027*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.Converters.ReceiveParametersContentSearchableStringConverter
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Converters.SearchableStringConverter/*01000042*/
{
  .method /*060000E7*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<string> 
          Convert(object 'value') cil managed
  // SIG: 20 01 15 12 65 01 0E 1C
  {
    // Method begins at RVA 0x2168
    // Code size       0 (0x0)
  } // end of method ReceiveParametersContentSearchableStringConverter::Convert

  .method /*060000E8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2169
    // Code size       0 (0x0)
  } // end of method ReceiveParametersContentSearchableStringConverter::.ctor

} // end of class System.ServiceModel.Activities.Presentation.Converters.ReceiveParametersContentSearchableStringConverter

.class /*02000028*/ private auto ansi beforefieldinit System.ServiceModel.Activities.Presentation.Converters.XPathMessageQuerySearchableStringConverter
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Converters.SearchableStringConverter/*01000042*/
{
  .method /*060000E9*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<string> 
          Convert(object 'value') cil managed
  // SIG: 20 01 15 12 65 01 0E 1C
  {
    // Method begins at RVA 0x216a
    // Code size       0 (0x0)
  } // end of method XPathMessageQuerySearchableStringConverter::Convert

  .method /*060000EA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x216b
    // Code size       0 (0x0)
  } // end of method XPathMessageQuerySearchableStringConverter::.ctor

} // end of class System.ServiceModel.Activities.Presentation.Converters.XPathMessageQuerySearchableStringConverter

.class /*02000029*/ public auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.Factories.ReceiveAndSendReplyFactory
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IActivityTemplateFactory/*01000043*/
{
  .method /*060000EB*/ public hidebysig newslot virtual final 
          instance class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ 
          Create(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ target) cil managed
  // SIG: 20 01 12 80 85 12 6D
  {
    // Method begins at RVA 0x216c
    // Code size       0 (0x0)
  } // end of method ReceiveAndSendReplyFactory::Create

  .method /*060000EC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x216d
    // Code size       0 (0x0)
  } // end of method ReceiveAndSendReplyFactory::.ctor

  .method /*060000ED*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x216e
    // Code size       0 (0x0)
  } // end of method ReceiveAndSendReplyFactory::.cctor

} // end of class System.ServiceModel.Activities.Presentation.Factories.ReceiveAndSendReplyFactory

.class /*0200002A*/ public auto ansi sealed beforefieldinit System.ServiceModel.Activities.Presentation.Factories.SendAndReceiveReplyFactory
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IActivityTemplateFactory/*01000043*/
{
  .method /*060000EE*/ public hidebysig newslot virtual final 
          instance class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ 
          Create(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ target) cil managed
  // SIG: 20 01 12 80 85 12 6D
  {
    // Method begins at RVA 0x216f
    // Code size       0 (0x0)
  } // end of method SendAndReceiveReplyFactory::Create

  .method /*060000EF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2170
    // Code size       0 (0x0)
  } // end of method SendAndReceiveReplyFactory::.ctor

  .method /*060000F0*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2171
    // Code size       0 (0x0)
  } // end of method SendAndReceiveReplyFactory::.cctor

} // end of class System.ServiceModel.Activities.Presentation.Factories.SendAndReceiveReplyFactory

.class /*0200002B*/ public auto ansi beforefieldinit System.Activities.Presentation.DynamicArgumentDesignerOptions
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*060000F1*/ public hidebysig specialname 
          instance string  get_Title() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A6:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2172
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::get_Title

  .method /*060000F2*/ public hidebysig specialname 
          instance void  set_Title(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A7:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2173
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::set_Title

  .method /*060000F3*/ public hidebysig specialname 
          instance string  get_ArgumentPrefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2174
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::get_ArgumentPrefix

  .method /*060000F4*/ public hidebysig specialname 
          instance void  set_ArgumentPrefix(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2175
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::set_ArgumentPrefix

  .method /*060000F5*/ assembly hidebysig specialname 
          instance bool  get_HideDirection() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000A8:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2176
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::get_HideDirection

  .method /*060000F6*/ assembly hidebysig specialname 
          instance void  set_HideDirection(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000A9:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2177
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::set_HideDirection

  .method /*060000F7*/ assembly hidebysig specialname 
          instance string  get_HintText() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2178
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::get_HintText

  .method /*060000F8*/ assembly hidebysig specialname 
          instance void  set_HintText(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2179
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::set_HintText

  .method /*060000F9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x217a
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesignerOptions::.ctor

  .property /*1700002B*/ instance callconv(8) string
          Title()
  {
    .get instance string System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/::get_Title() /* 060000F1 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/::set_Title(string) /* 060000F2 */
  } // end of property DynamicArgumentDesignerOptions::Title
  .property /*1700002C*/ instance callconv(8) string
          ArgumentPrefix()
  {
    .get instance string System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/::get_ArgumentPrefix() /* 060000F3 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/::set_ArgumentPrefix(string) /* 060000F4 */
  } // end of property DynamicArgumentDesignerOptions::ArgumentPrefix
  .property /*1700002D*/ instance callconv(8) bool
          HideDirection()
  {
    .get instance bool System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/::get_HideDirection() /* 060000F5 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/::set_HideDirection(bool) /* 060000F6 */
  } // end of property DynamicArgumentDesignerOptions::HideDirection
  .property /*1700002E*/ instance callconv(8) string
          HintText()
  {
    .get instance string System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/::get_HintText() /* 060000F7 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/::set_HintText(string) /* 060000F8 */
  } // end of property DynamicArgumentDesignerOptions::HintText
} // end of class System.Activities.Presentation.DynamicArgumentDesignerOptions

.class /*0200002C*/ public auto ansi sealed beforefieldinit System.Activities.Presentation.DynamicArgumentDialog
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/
{
  .method /*060000FA*/ public hidebysig static 
          bool  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ activity,
                           class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ data,
                           class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context,
                           class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ owner,
                           class System.Activities.Presentation.DynamicArgumentDesignerOptions/*0200002B*/ options) cil managed
  // SIG: 00 05 02 12 2D 12 2D 12 79 12 6D 12 80 AC
  {
    // Method begins at RVA 0x217b
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDialog::ShowDialog

  .method /*060000FB*/ family hidebysig virtual 
          instance void  OnWorkflowElementDialogClosed(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000044*/<bool> dialogResult) cil managed
  // SIG: 20 01 01 15 11 81 11 01 02
  {
    // Method begins at RVA 0x217c
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDialog::OnWorkflowElementDialogClosed

  .method /*060000FC*/ private specialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x217d
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDialog::.ctor

} // end of class System.Activities.Presentation.DynamicArgumentDialog

.class /*0200002D*/ private auto ansi sealed beforefieldinit System.Activities.Presentation.ActivityXRefPropertyEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValueEditor/*01000012*/
{
  .method /*060000FD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x217e
    // Code size       0 (0x0)
  } // end of method ActivityXRefPropertyEditor::.ctor

} // end of class System.Activities.Presentation.ActivityXRefPropertyEditor

.class /*0200002E*/ private auto ansi beforefieldinit System.Activities.Presentation.TypeCollectionDesigner
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.ContentControl/*01000013*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*04000032*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ContextProperty
  .field /*04000033*/ public static initonly class [System/*23000002*/]System.Windows.Input.ICommand/*01000045*/ AddNewTypeCommand
  .field /*04000034*/ public static initonly class [System/*23000002*/]System.Windows.Input.ICommand/*01000045*/ DeleteTypeCommand
  .field /*04000035*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ Grid_1
  .field /*04000036*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.StackPanel/*01000046*/ HeadBar
  .field /*04000037*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Button/*01000039*/ ButtonDelete
  .field /*04000038*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGrid/*01000033*/ typesDataGrid
  .method /*060000FE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x217f
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::.ctor

  .method /*060000FF*/ assembly hidebysig specialname 
          instance void  set_InitialTypeCollection(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [mscorlib/*23000001*/]System.Type/*01000003*/> 'value') cil managed
  // SIG: 20 01 01 15 12 7D 01 12 0D
  {
    // Method begins at RVA 0x2180
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::set_InitialTypeCollection

  .method /*06000100*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [mscorlib/*23000001*/]System.Type/*01000003*/> 
          get_UpdatedTypeCollection() cil managed
  // SIG: 20 00 15 12 7D 01 12 0D
  {
    // Method begins at RVA 0x2181
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::get_UpdatedTypeCollection

  .method /*06000101*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 
          get_Context() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x2182
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::get_Context

  .method /*06000102*/ public hidebysig specialname 
          instance void  set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2183
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::set_Context

  .method /*06000103*/ public hidebysig specialname 
          instance bool  get_AllowDuplicate() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000AA:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2184
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::get_AllowDuplicate

  .method /*06000104*/ public hidebysig specialname 
          instance void  set_AllowDuplicate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000AB:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2185
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::set_AllowDuplicate

  .method /*06000105*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> 
          get_Filter() cil managed
  // SIG: 20 00 15 12 71 02 12 0D 02
  {
    .custom /*0C0000AC:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2186
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::get_Filter

  .method /*06000106*/ public hidebysig specialname 
          instance void  set_Filter(class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> 'value') cil managed
  // SIG: 20 01 01 15 12 71 02 12 0D 02
  {
    .custom /*0C0000AD:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2187
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::set_Filter

  .method /*06000107*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_DefaultType() cil managed
  // SIG: 20 00 12 0D
  {
    .custom /*0C0000AE:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2188
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::get_DefaultType

  .method /*06000108*/ public hidebysig specialname 
          instance void  set_DefaultType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    .custom /*0C0000AF:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2189
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::set_DefaultType

  .method /*06000109*/ assembly hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/ 
          get_ParentDialog() cil managed
  // SIG: 20 00 12 80 ED
  {
    .custom /*0C0000B0:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218a
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::get_ParentDialog

  .method /*0600010A*/ assembly hidebysig specialname 
          instance void  set_ParentDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 ED
  {
    .custom /*0C0000B1:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218b
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::set_ParentDialog

  .method /*0600010B*/ assembly hidebysig 
          instance bool  OnOK() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x218c
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::OnOK

  .method /*0600010C*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x218d
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::OnInitialized

  .method /*0600010D*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000B2:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B3:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x218e
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::InitializeComponent

  .method /*0600010E*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000B4:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B5:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000B6:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x218f
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600010F*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000B7:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B8:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000B9:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x2190
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*06000110*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2191
    // Code size       0 (0x0)
  } // end of method TypeCollectionDesigner::.cctor

  .property /*1700002F*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [mscorlib/*23000001*/]System.Type/*01000003*/>
          InitialTypeCollection()
  {
    .set instance void System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::set_InitialTypeCollection(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [mscorlib/*23000001*/]System.Type/*01000003*/>) /* 060000FF */
  } // end of property TypeCollectionDesigner::InitialTypeCollection
  .property /*17000030*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [mscorlib/*23000001*/]System.Type/*01000003*/>
          UpdatedTypeCollection()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [mscorlib/*23000001*/]System.Type/*01000003*/> System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::get_UpdatedTypeCollection() /* 06000100 */
  } // end of property TypeCollectionDesigner::UpdatedTypeCollection
  .property /*17000031*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/
          Context()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::get_Context() /* 06000101 */
    .set instance void System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/) /* 06000102 */
  } // end of property TypeCollectionDesigner::Context
  .property /*17000032*/ instance callconv(8) bool
          AllowDuplicate()
  {
    .get instance bool System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::get_AllowDuplicate() /* 06000103 */
    .set instance void System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::set_AllowDuplicate(bool) /* 06000104 */
  } // end of property TypeCollectionDesigner::AllowDuplicate
  .property /*17000033*/ instance callconv(8) class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>
          Filter()
  {
    .get instance class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::get_Filter() /* 06000105 */
    .set instance void System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::set_Filter(class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>) /* 06000106 */
  } // end of property TypeCollectionDesigner::Filter
  .property /*17000034*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          DefaultType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::get_DefaultType() /* 06000107 */
    .set instance void System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::set_DefaultType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000108 */
  } // end of property TypeCollectionDesigner::DefaultType
  .property /*17000035*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/
          ParentDialog()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/ System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::get_ParentDialog() /* 06000109 */
    .set instance void System.Activities.Presentation.TypeCollectionDesigner/*0200002E*/::set_ParentDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/) /* 0600010A */
  } // end of property TypeCollectionDesigner::ParentDialog
} // end of class System.Activities.Presentation.TypeCollectionDesigner

.class /*0200002F*/ private auto ansi sealed beforefieldinit System.Activities.Presentation.DynamicArgumentDesigner
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.UserControl/*01000047*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*04000039*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ContextProperty
  .field /*0400003A*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ OwnerActivityProperty
  .field /*0400003B*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ IsDirectionReadOnlyProperty
  .field /*0400003C*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ DynamicArgumentsProperty
  .field /*0400003D*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ IsDictionaryProperty
  .field /*0400003E*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ UnderlyingArgumentTypeProperty
  .field /*0400003F*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ CreateDynamicArgumentCommand
  .field /*04000040*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ MoveUpArgumentCommand
  .field /*04000041*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ MoveDownArgumentCommand
  .field /*04000042*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ DeleteArgumentCommand
  .field /*04000043*/ public static literal string DefaultArgumentPrefix = "Argument"
  .field /*04000044*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ Grid_1
  .field /*04000045*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.StackPanel/*01000046*/ StackPane_1
  .field /*04000046*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Button/*01000039*/ ButtonDelete
  .field /*04000047*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Button/*01000039*/ ButtonMovUp
  .field /*04000048*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Button/*01000039*/ ButtonMovDown
  .field /*04000049*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGrid/*01000033*/ WPF_DataGrid
  .method /*06000111*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2192
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::.ctor

  .method /*06000112*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x2193
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::OnInitialized

  .method /*06000113*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/> 
          get_DynamicArguments() cil managed
  // SIG: 20 00 15 12 80 DD 01 12 80 C0
  {
    // Method begins at RVA 0x2194
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_DynamicArguments

  .method /*06000114*/ public hidebysig specialname 
          instance void  set_DynamicArguments(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 DD 01 12 80 C0
  {
    // Method begins at RVA 0x2195
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_DynamicArguments

  .method /*06000115*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_OwnerActivity() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x2196
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_OwnerActivity

  .method /*06000116*/ public hidebysig specialname 
          instance void  set_OwnerActivity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x2197
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_OwnerActivity

  .method /*06000117*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 
          get_Context() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x2198
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_Context

  .method /*06000118*/ public hidebysig specialname 
          instance void  set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2199
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_Context

  .method /*06000119*/ public hidebysig specialname 
          instance bool  get_IsDirectionReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x219a
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_IsDirectionReadOnly

  .method /*0600011A*/ public hidebysig specialname 
          instance void  set_IsDirectionReadOnly(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x219b
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_IsDirectionReadOnly

  .method /*0600011B*/ public hidebysig specialname 
          instance bool  get_HideDirection() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x219c
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_HideDirection

  .method /*0600011C*/ public hidebysig specialname 
          instance void  set_HideDirection(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x219d
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_HideDirection

  .method /*0600011D*/ public hidebysig specialname 
          instance string  get_ArgumentPrefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x219e
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_ArgumentPrefix

  .method /*0600011E*/ public hidebysig specialname 
          instance void  set_ArgumentPrefix(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x219f
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_ArgumentPrefix

  .method /*0600011F*/ public hidebysig specialname 
          instance string  get_HintText() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21a0
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_HintText

  .method /*06000120*/ public hidebysig specialname 
          instance void  set_HintText(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21a1
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_HintText

  .method /*06000121*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/ 
          get_ParentDialog() cil managed
  // SIG: 20 00 12 80 ED
  {
    .custom /*0C0000BA:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a2
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_ParentDialog

  .method /*06000122*/ public hidebysig specialname 
          instance void  set_ParentDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 ED
  {
    .custom /*0C0000BB:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a3
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_ParentDialog

  .method /*06000123*/ assembly hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_UnderlyingArgumentType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x21a4
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_UnderlyingArgumentType

  .method /*06000124*/ assembly hidebysig specialname 
          instance void  set_UnderlyingArgumentType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x21a5
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_UnderlyingArgumentType

  .method /*06000125*/ assembly hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000044*/<bool> 
          get_IsDictionary() cil managed
  // SIG: 20 00 15 11 81 11 01 02
  {
    // Method begins at RVA 0x21a6
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::get_IsDictionary

  .method /*06000126*/ assembly hidebysig specialname 
          instance void  set_IsDictionary(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000044*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 81 11 01 02
  {
    // Method begins at RVA 0x21a7
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::set_IsDictionary

  .method /*06000127*/ assembly hidebysig static 
          class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/> 
          ModelItemToWrapperCollection(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ model,
                                       [out] bool& isDictionary,
                                       [out] class [mscorlib/*23000001*/]System.Type/*01000003*/& underlyingArgumentType) cil managed
  // SIG: 00 03 15 12 80 DD 01 12 80 C0 12 2D 10 02 10 12 0D
  {
    // Method begins at RVA 0x21a8
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::ModelItemToWrapperCollection

  .method /*06000128*/ assembly hidebysig static 
          void  WrapperCollectionToModelItem(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/> wrappers,
                                             class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ data,
                                             bool isDictionary,
                                             class [mscorlib/*23000001*/]System.Type/*01000003*/ underlyingArgumentType) cil managed
  // SIG: 00 04 01 15 12 80 DD 01 12 80 C0 12 2D 02 12 0D
  {
    // Method begins at RVA 0x21a9
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::WrapperCollectionToModelItem

  .method /*06000129*/ assembly hidebysig 
          instance void  ValidateEntry(class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/ entry,
                                       valuetype [WindowsBase/*23000003*/]System.Windows.DependencyPropertyChangedEventArgs/*01000048*/ e) cil managed
  // SIG: 20 02 01 12 80 C0 11 81 21
  {
    // Method begins at RVA 0x21aa
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::ValidateEntry

  .method /*0600012A*/ assembly hidebysig 
          instance string  GetDefaultName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21ab
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::GetDefaultName

  .method /*0600012B*/ assembly hidebysig 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          GetDefaultType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x21ac
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::GetDefaultType

  .method /*0600012C*/ assembly hidebysig 
          instance valuetype [System.Activities/*2300000B*/]System.Activities.ArgumentDirection/*01000049*/ 
          GetDefaultDirection() cil managed
  // SIG: 20 00 11 81 25
  {
    // Method begins at RVA 0x21ad
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::GetDefaultDirection

  .method /*0600012D*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000BC:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BD:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x21ae
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::InitializeComponent

  .method /*0600012E*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000BE:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BF:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000C0:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x21af
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600012F*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000C1:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C2:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000C3:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x21b0
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*06000130*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x21b1
    // Code size       0 (0x0)
  } // end of method DynamicArgumentDesigner::.cctor

  .property /*17000036*/ instance callconv(8) class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/>
          DynamicArguments()
  {
    .get instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/> System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_DynamicArguments() /* 06000113 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_DynamicArguments(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/>) /* 06000114 */
  } // end of property DynamicArgumentDesigner::DynamicArguments
  .property /*17000037*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          OwnerActivity()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_OwnerActivity() /* 06000115 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_OwnerActivity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 06000116 */
  } // end of property DynamicArgumentDesigner::OwnerActivity
  .property /*17000038*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/
          Context()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_Context() /* 06000117 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/) /* 06000118 */
  } // end of property DynamicArgumentDesigner::Context
  .property /*17000039*/ instance callconv(8) bool
          IsDirectionReadOnly()
  {
    .get instance bool System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_IsDirectionReadOnly() /* 06000119 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_IsDirectionReadOnly(bool) /* 0600011A */
  } // end of property DynamicArgumentDesigner::IsDirectionReadOnly
  .property /*1700003A*/ instance callconv(8) bool
          HideDirection()
  {
    .get instance bool System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_HideDirection() /* 0600011B */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_HideDirection(bool) /* 0600011C */
  } // end of property DynamicArgumentDesigner::HideDirection
  .property /*1700003B*/ instance callconv(8) string
          ArgumentPrefix()
  {
    .get instance string System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_ArgumentPrefix() /* 0600011D */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_ArgumentPrefix(string) /* 0600011E */
  } // end of property DynamicArgumentDesigner::ArgumentPrefix
  .property /*1700003C*/ instance callconv(8) string
          HintText()
  {
    .get instance string System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_HintText() /* 0600011F */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_HintText(string) /* 06000120 */
  } // end of property DynamicArgumentDesigner::HintText
  .property /*1700003D*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/
          ParentDialog()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/ System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_ParentDialog() /* 06000121 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_ParentDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/) /* 06000122 */
  } // end of property DynamicArgumentDesigner::ParentDialog
  .property /*1700003E*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          UnderlyingArgumentType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_UnderlyingArgumentType() /* 06000123 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_UnderlyingArgumentType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000124 */
  } // end of property DynamicArgumentDesigner::UnderlyingArgumentType
  .property /*1700003F*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000044*/<bool>
          IsDictionary()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000044*/<bool> System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::get_IsDictionary() /* 06000125 */
    .set instance void System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/::set_IsDictionary(valuetype [mscorlib/*23000001*/]System.Nullable`1/*01000044*/<bool>) /* 06000126 */
  } // end of property DynamicArgumentDesigner::IsDictionary
} // end of class System.Activities.Presentation.DynamicArgumentDesigner

.class /*02000030*/ private auto ansi sealed beforefieldinit System.Activities.Presentation.DynamicArgumentWrapperObject
       extends [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/
{
  .field /*0400004A*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ModelItemProperty
  .field /*0400004B*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ NameProperty
  .field /*0400004C*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ArgumentTypeProperty
  .field /*0400004D*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ DirectionProperty
  .field /*0400004E*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ExpressionProperty
  .field /*0400004F*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ UseLocationExpressionProperty
  .method /*06000131*/ public hidebysig specialname 
          instance void  add_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    .custom /*0C0000C4:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b2
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::add_PropertyChanged

  .method /*06000132*/ public hidebysig specialname 
          instance void  remove_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    .custom /*0C0000C5:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b3
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::remove_PropertyChanged

  .method /*06000133*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21b4
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::get_Name

  .method /*06000134*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21b5
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::set_Name

  .method /*06000135*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_Type() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x21b6
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::get_Type

  .method /*06000136*/ public hidebysig specialname 
          instance void  set_Type(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x21b7
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::set_Type

  .method /*06000137*/ public hidebysig specialname 
          instance valuetype [System.Activities/*2300000B*/]System.Activities.ArgumentDirection/*01000049*/ 
          get_Direction() cil managed
  // SIG: 20 00 11 81 25
  {
    // Method begins at RVA 0x21b8
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::get_Direction

  .method /*06000138*/ public hidebysig specialname 
          instance void  set_Direction(valuetype [System.Activities/*2300000B*/]System.Activities.ArgumentDirection/*01000049*/ 'value') cil managed
  // SIG: 20 01 01 11 81 25
  {
    // Method begins at RVA 0x21b9
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::set_Direction

  .method /*06000139*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_Expression() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x21ba
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::get_Expression

  .method /*0600013A*/ public hidebysig specialname 
          instance void  set_Expression(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x21bb
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::set_Expression

  .method /*0600013B*/ public hidebysig specialname 
          instance bool  get_UseLocationExpression() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21bc
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::get_UseLocationExpression

  .method /*0600013C*/ public hidebysig specialname 
          instance void  set_UseLocationExpression(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21bd
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::set_UseLocationExpression

  .method /*0600013D*/ assembly hidebysig specialname 
          instance bool  get_IsValidating() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000C6:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21be
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::get_IsValidating

  .method /*0600013E*/ assembly hidebysig specialname 
          instance void  set_IsValidating(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000C7:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21bf
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::set_IsValidating

  .method /*0600013F*/ assembly hidebysig specialname 
          instance class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGridRow/*0100004A*/ 
          get_Row() cil managed
  // SIG: 20 00 12 81 29
  {
    .custom /*0C0000C8:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c0
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::get_Row

  .method /*06000140*/ assembly hidebysig specialname 
          instance void  set_Row(class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGridRow/*0100004A*/ 'value') cil managed
  // SIG: 20 01 01 12 81 29
  {
    .custom /*0C0000C9:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c1
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::set_Row

  .method /*06000141*/ public hidebysig specialname static 
          class System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/ 
          get_Editor() cil managed
  // SIG: 00 00 12 80 BC
  {
    .custom /*0C0000CA:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c2
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::get_Editor

  .method /*06000142*/ public hidebysig specialname static 
          void  set_Editor(class System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/ 'value') cil managed
  // SIG: 00 01 01 12 80 BC
  {
    .custom /*0C0000CB:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c3
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::set_Editor

  .method /*06000143*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21c4
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::.ctor

  .method /*06000144*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string argumentName,
                               class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ argumentItem) cil managed
  // SIG: 20 02 01 0E 12 2D
  {
    // Method begins at RVA 0x21c5
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::.ctor

  .method /*06000145*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21c6
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::ToString

  .method /*06000146*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x21c7
    // Code size       0 (0x0)
  } // end of method DynamicArgumentWrapperObject::.cctor

  .event /*14000006*/ [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ PropertyChanged
  {
    .addon instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::add_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/) /* 06000131 */
    .removeon instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::remove_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/) /* 06000132 */
  } // end of event DynamicArgumentWrapperObject::PropertyChanged
  .property /*17000040*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::get_Name() /* 06000133 */
    .set instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::set_Name(string) /* 06000134 */
  } // end of property DynamicArgumentWrapperObject::Name
  .property /*17000041*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          Type()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::get_Type() /* 06000135 */
    .set instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::set_Type(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000136 */
  } // end of property DynamicArgumentWrapperObject::Type
  .property /*17000042*/ instance callconv(8) valuetype [System.Activities/*2300000B*/]System.Activities.ArgumentDirection/*01000049*/
          Direction()
  {
    .get instance valuetype [System.Activities/*2300000B*/]System.Activities.ArgumentDirection/*01000049*/ System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::get_Direction() /* 06000137 */
    .set instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::set_Direction(valuetype [System.Activities/*2300000B*/]System.Activities.ArgumentDirection/*01000049*/) /* 06000138 */
  } // end of property DynamicArgumentWrapperObject::Direction
  .property /*17000043*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          Expression()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::get_Expression() /* 06000139 */
    .set instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::set_Expression(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 0600013A */
  } // end of property DynamicArgumentWrapperObject::Expression
  .property /*17000044*/ instance callconv(8) bool
          UseLocationExpression()
  {
    .get instance bool System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::get_UseLocationExpression() /* 0600013B */
    .set instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::set_UseLocationExpression(bool) /* 0600013C */
  } // end of property DynamicArgumentWrapperObject::UseLocationExpression
  .property /*17000045*/ instance callconv(8) bool
          IsValidating()
  {
    .get instance bool System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::get_IsValidating() /* 0600013D */
    .set instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::set_IsValidating(bool) /* 0600013E */
  } // end of property DynamicArgumentWrapperObject::IsValidating
  .property /*17000046*/ instance callconv(8) class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGridRow/*0100004A*/
          Row()
  {
    .get instance class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGridRow/*0100004A*/ System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::get_Row() /* 0600013F */
    .set instance void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::set_Row(class [PresentationFramework/*23000006*/]System.Windows.Controls.DataGridRow/*0100004A*/) /* 06000140 */
  } // end of property DynamicArgumentWrapperObject::Row
  .property /*17000047*/ callconv(8) class System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/
          Editor()
  {
    .get class System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/ System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::get_Editor() /* 06000141 */
    .set void System.Activities.Presentation.DynamicArgumentWrapperObject/*02000030*/::set_Editor(class System.Activities.Presentation.DynamicArgumentDesigner/*0200002F*/) /* 06000142 */
  } // end of property DynamicArgumentWrapperObject::Editor
} // end of class System.Activities.Presentation.DynamicArgumentWrapperObject

.class /*02000031*/ private auto ansi sealed beforefieldinit System.Activities.Presentation.TypeCollectionPropertyEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor/*0100002B*/
{
  .field /*04000050*/ public static literal string AllowDuplicate = "AllowDuplicate"
  .field /*04000051*/ public static literal string Filter = "Filter"
  .field /*04000052*/ public static literal string DefaultType = "DefaultType"
  .method /*06000147*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21c8
    // Code size       0 (0x0)
  } // end of method TypeCollectionPropertyEditor::.ctor

  .method /*06000148*/ assembly hidebysig static 
          !!T  GetOptionValueOrUseDefault<T>(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/ attributes,
                                             string optionName,
                                             !!T defaultValue) cil managed
  // SIG: 10 01 03 1E 00 12 80 99 0E 1E 00
  {
    // Method begins at RVA 0x21c9
    // Code size       0 (0x0)
  } // end of method TypeCollectionPropertyEditor::GetOptionValueOrUseDefault

  .method /*06000149*/ public hidebysig virtual 
          instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValue/*0100002C*/ propertyValue,
                                    class [PresentationCore/*23000009*/]System.Windows.IInputElement/*0100002D*/ commandSource) cil managed
  // SIG: 20 02 01 12 80 B1 12 80 B5
  {
    // Method begins at RVA 0x21ca
    // Code size       0 (0x0)
  } // end of method TypeCollectionPropertyEditor::ShowDialog

} // end of class System.Activities.Presentation.TypeCollectionPropertyEditor

.class /*02000032*/ private abstract auto ansi sealed beforefieldinit System.Activities.Presentation.ActivityDesignerHelper
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .custom /*0C00002F:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100008D*/::.ctor() /* 0A00000E */ = ( 01 00 00 00 ) 
  .field /*04000053*/ public static literal string ChannelBasedCorrelationKey = "ChannelBasedCorrelation"
  .method /*0600014A*/ public hidebysig static 
          bool  IsItemInSequence(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ item,
                                 [out] class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/& sequence) cil managed
  // SIG: 00 02 02 12 2D 10 12 2D
  {
    .custom /*0C0000CC:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100008D*/::.ctor() /* 0A00000E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21cb
    // Code size       0 (0x0)
  } // end of method ActivityDesignerHelper::IsItemInSequence

  .method /*0600014B*/ public hidebysig static 
          bool  IsItemInFlowchart(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ item,
                                  [out] class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/& flowchart,
                                  [out] class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/& flowStep) cil managed
  // SIG: 00 03 02 12 2D 10 12 2D 10 12 2D
  {
    .custom /*0C0000CD:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100008D*/::.ctor() /* 0A00000E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21cc
    // Code size       0 (0x0)
  } // end of method ActivityDesignerHelper::IsItemInFlowchart

  .method /*0600014C*/ public hidebysig static 
          bool  IsMessagingActivity(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ item) cil managed
  // SIG: 00 01 02 12 2D
  {
    .custom /*0C0000CE:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100008D*/::.ctor() /* 0A00000E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21cd
    // Code size       0 (0x0)
  } // end of method ActivityDesignerHelper::IsMessagingActivity

  .method /*0600014D*/ public hidebysig static 
          string  GenerateUniqueVariableNameForContext(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ context,
                                                       string prefix) cil managed
  // SIG: 00 02 0E 12 6D 0E
  {
    // Method begins at RVA 0x21ce
    // Code size       0 (0x0)
  } // end of method ActivityDesignerHelper::GenerateUniqueVariableNameForContext

} // end of class System.Activities.Presentation.ActivityDesignerHelper

.class /*02000033*/ private auto ansi sealed beforefieldinit System.Activities.Presentation.ArgumentCollectionPropertyEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor/*0100002B*/
{
  .method /*0600014E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21cf
    // Code size       0 (0x0)
  } // end of method ArgumentCollectionPropertyEditor::.ctor

  .method /*0600014F*/ public hidebysig virtual 
          instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValue/*0100002C*/ propertyValue,
                                    class [PresentationCore/*23000009*/]System.Windows.IInputElement/*0100002D*/ commandSource) cil managed
  // SIG: 20 02 01 12 80 B1 12 80 B5
  {
    // Method begins at RVA 0x21d0
    // Code size       0 (0x0)
  } // end of method ArgumentCollectionPropertyEditor::ShowDialog

} // end of class System.Activities.Presentation.ArgumentCollectionPropertyEditor

.class /*02000034*/ private auto ansi sealed beforefieldinit System.Activities.Presentation.FlowSwitchLinkCasePropertyEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValueEditor/*01000012*/
{
  .method /*06000150*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21d1
    // Code size       0 (0x0)
  } // end of method FlowSwitchLinkCasePropertyEditor::.ctor

} // end of class System.Activities.Presentation.FlowSwitchLinkCasePropertyEditor

.class /*02000035*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowchartDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IMultipleDragEnabledCompositeView/*0100004B*/,
                  [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ICompositeView/*0100004C*/,
                  [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.IAutoConnectContainer/*0100004D*/,
                  [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.IAutoSplitContainer/*0100004E*/,
                  [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .custom /*0C000031:0A00002A*/ instance void [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesignerOptionsAttribute/*010000AC*/::.ctor() /* 0A00002A */ = ( 01 00 01 00 54 02 16 41 6C 77 61 79 73 43 6F 6C   // ....T..AlwaysCol
                                                                                                                                                                                                  6C 61 70 73 65 43 68 69 6C 64 72 65 6E 01 )       // lapseChildren.
  .field /*04000054*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ DroppingTypeResolvingOptionsProperty
  .field /*04000055*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ConnectionPointsProperty
  .field /*04000056*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ LinkModelItemProperty
  .field /*04000057*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ FlowElementModelItemProperty
  .field /*04000058*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ FlowchartWidthProperty
  .field /*04000059*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ FlowchartHeightProperty
  .field /*0400005A*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TrueConnectionPointProperty
  .field /*0400005B*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ FalseConnectionPointProperty
  .field /*0400005C*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ShowAllConditionsProperty
  .field /*0400005D*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ SetAsStartNodeCommand
  .field /*0400005E*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ ShowAllConditionsCommand
  .field /*0400005F*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ HideAllConditionsCommand
  .field /*04000060*/ assembly class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/,class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/> modelElement
  .field /*04000061*/ assembly class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.FreeFormPanel/*01000051*/ panel
  .field /*04000062*/ assembly class System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/ flowchartDesigner
  .field /*04000063*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.ContentPresenter/*01000052*/ flowchartContentPresenter
  .method /*06000151*/ public hidebysig newslot specialname virtual final 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/ 
          get_DroppingTypeResolvingOptions() cil managed
  // SIG: 20 00 12 81 4D
  {
    // Method begins at RVA 0x21d2
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_DroppingTypeResolvingOptions

  .method /*06000152*/ public hidebysig specialname 
          instance void  set_DroppingTypeResolvingOptions(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/ 'value') cil managed
  // SIG: 20 01 01 12 81 4D
  {
    // Method begins at RVA 0x21d3
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::set_DroppingTypeResolvingOptions

  .method /*06000153*/ public hidebysig newslot virtual final 
          instance void  OnItemMoved(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem) cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x21d4
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnItemMoved

  .method /*06000154*/ public hidebysig newslot virtual final 
          instance object  OnItemsCopied(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> itemsToCopy) cil managed
  // SIG: 20 01 1C 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x21d5
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnItemsCopied

  .method /*06000155*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> 
          SortSelectedItems(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> selectedItems) cil managed
  // SIG: 20 01 15 12 81 51 01 12 2D 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x21d6
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::SortSelectedItems

  .method /*06000156*/ public hidebysig newslot virtual final 
          instance void  OnItemsMoved(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> movedItems) cil managed
  // SIG: 20 01 01 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x21d7
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnItemsMoved

  .method /*06000157*/ public hidebysig newslot virtual final 
          instance object  OnItemsCut(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> itemsToCut) cil managed
  // SIG: 20 01 1C 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x21d8
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnItemsCut

  .method /*06000158*/ public hidebysig newslot virtual final 
          instance void  OnItemsDelete(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> itemsToDelete) cil managed
  // SIG: 20 01 01 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x21d9
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnItemsDelete

  .method /*06000159*/ public hidebysig instance void 
          OnItemsDelete(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> itemsToDelete,
                        class [System.Core/*2300000E*/]System.Collections.Generic.HashSet`1/*01000055*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> updatedItems) cil managed
  // SIG: 20 02 01 15 12 81 51 01 12 2D 15 12 81 55 01 12 2D
  {
    // Method begins at RVA 0x21da
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnItemsDelete

  .method /*0600015A*/ public hidebysig newslot virtual final 
          instance bool  CanPasteItems(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> itemsToPaste) cil managed
  // SIG: 20 01 02 15 12 81 51 01 1C
  {
    // Method begins at RVA 0x21db
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::CanPasteItems

  .method /*0600015B*/ public hidebysig newslot virtual final 
          instance void  OnItemsPasted(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> itemsToPaste,
                                       class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> metaData,
                                       valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/ pastePoint,
                                       class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/ pastePointReference) cil managed
  // SIG: 20 04 01 15 12 81 51 01 1C 15 12 81 51 01 1C 11 81 59 12 21
  {
    // Method begins at RVA 0x21dc
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnItemsPasted

  .method /*0600015C*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsDefaultContainer() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21dd
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_IsDefaultContainer

  .method /*0600015D*/ assembly hidebysig 
          instance void  FindBestMatchConnectionPointPair(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> srcConnPoints,
                                                          class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> destConnPoints,
                                                          [out] class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/& srcConnPoint,
                                                          [out] class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/& destConnPoint) cil managed
  // SIG: 20 04 01 15 12 81 51 01 12 81 5D 15 12 81 51 01 12 81 5D 10 12 81 5D 10 12 81 5D
  {
    // Method begins at RVA 0x21de
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::FindBestMatchConnectionPointPair

  .method /*0600015E*/ assembly hidebysig 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ 
          FindClosestConnectionPoint(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ srcConnPoint,
                                     class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> destConnPoints,
                                     [out] float64& minDist) cil managed
  // SIG: 20 03 12 81 5D 12 81 5D 15 12 81 51 01 12 81 5D 10 0D
  {
    // Method begins at RVA 0x21df
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::FindClosestConnectionPoint

  .method /*0600015F*/ assembly hidebysig 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ 
          FindClosestConnectionPoint(valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/ srcConnPointLocation,
                                     class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> destConnPoints,
                                     [out] float64& minDist) cil managed
  // SIG: 20 03 12 81 5D 11 81 59 15 12 81 51 01 12 81 5D 10 0D
  {
    // Method begins at RVA 0x21e0
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::FindClosestConnectionPoint

  .method /*06000160*/ assembly hidebysig 
          instance int32  NumberOfIncomingLinks(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ designer) cil managed
  // SIG: 20 01 08 12 81 41
  {
    // Method begins at RVA 0x21e1
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::NumberOfIncomingLinks

  .method /*06000161*/ public hidebysig static 
          bool  Pack<class T>(!!T[] toPack,
                              class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<!!T,bool> isPacked) cil managed
  // SIG: 10 01 02 02 1D 1E 00 15 12 71 02 1E 00 02
  {
    // Method begins at RVA 0x21e2
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::Pack

  .method /*06000162*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x21e3
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnModelItemChanged

  .method /*06000163*/ assembly hidebysig 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/ 
          GetLinkOnCanvas(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ srcFlowElementModelItem,
                          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ destflowElementModelItem,
                          string propertyName) cil managed
  // SIG: 20 03 12 81 61 12 2D 12 2D 0E
  {
    // Method begins at RVA 0x21e4
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetLinkOnCanvas

  .method /*06000164*/ assembly hidebysig 
          instance class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ 
          DeleteLink(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/ link,
                     [opt] bool isMoveOrAutoSplit,
                     [opt] class [System.Core/*2300000E*/]System.Collections.Generic.HashSet`1/*01000055*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> referenceUpdatedModelItems) cil managed
  // SIG: 20 03 12 81 AC 12 81 61 02 15 12 81 55 01 12 2D
  {
    .param [2]/*0800011F*/  = bool(false)
    .param [3]/*08000120*/  = nullref
    // Method begins at RVA 0x21e5
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::DeleteLink

  .method /*06000165*/ assembly hidebysig 
          instance bool  CreateLinkGesture(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ sourceConnectionPoint,
                                           class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ destConnectionPoint,
                                           [out] string& errorMessage,
                                           class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ caseKey) cil managed
  // SIG: 20 04 02 12 81 5D 12 81 5D 10 0E 12 81 AC
  {
    // Method begins at RVA 0x21e6
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::CreateLinkGesture

  .method /*06000166*/ assembly hidebysig 
          instance bool  CreateLinkGesture(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ sourceConnectionPoint,
                                           class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ destConnectionPoint,
                                           [out] string& errorMessage,
                                           bool isLinkValidDueToLinkMove,
                                           class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ caseKey) cil managed
  // SIG: 20 05 02 12 81 5D 12 81 5D 10 0E 02 12 81 AC
  {
    // Method begins at RVA 0x21e7
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::CreateLinkGesture

  .method /*06000167*/ assembly hidebysig 
          instance bool  CreateLinkGesture(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ sourceConnectionPoint,
                                           class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ dest,
                                           valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/ mouseLocation,
                                           [out] string& errorMessage,
                                           bool isLinkValidDueToLinkMove,
                                           class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ caseKey) cil managed
  // SIG: 20 06 02 12 81 5D 12 81 41 11 81 59 10 0E 02 12 81 AC
  {
    // Method begins at RVA 0x21e8
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::CreateLinkGesture

  .method /*06000168*/ assembly hidebysig 
          instance bool  CreateLinkGesture(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ sourceConnectionPoint,
                                           class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ dest,
                                           [out] string& errorMessage,
                                           bool isLinkValidDueToLinkMove,
                                           class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ caseKey) cil managed
  // SIG: 20 05 02 12 81 5D 12 81 41 10 0E 02 12 81 AC
  {
    // Method begins at RVA 0x21e9
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::CreateLinkGesture

  .method /*06000169*/ assembly hidebysig 
          instance bool  CreateLinkGesture(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ source,
                                           class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ destConnectionPoint,
                                           valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/ mouseLocation,
                                           [out] string& errorMessage,
                                           bool isLinkValidDueToLinkMove,
                                           class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ caseKey) cil managed
  // SIG: 20 06 02 12 81 41 12 81 5D 11 81 59 10 0E 02 12 81 AC
  {
    // Method begins at RVA 0x21ea
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::CreateLinkGesture

  .method /*0600016A*/ assembly hidebysig 
          instance bool  CreateLinkGesture(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ source,
                                           class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ dest,
                                           [out] string& errorMessage,
                                           class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ caseKey) cil managed
  // SIG: 20 04 02 12 81 41 12 81 41 10 0E 12 81 AC
  {
    // Method begins at RVA 0x21eb
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::CreateLinkGesture

  .method /*0600016B*/ assembly hidebysig specialname 
          instance bool  get_IsResizing() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000CF:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ec
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_IsResizing

  .method /*0600016C*/ assembly hidebysig specialname 
          instance void  set_IsResizing(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000D0:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ed
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::set_IsResizing

  .method /*0600016D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ee
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::.ctor

  .method /*0600016E*/ public hidebysig specialname static 
          float64  get_FlowNodeCaptionFontSize() cil managed
  // SIG: 00 00 0D
  {
    // Method begins at RVA 0x21ef
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_FlowNodeCaptionFontSize

  .method /*0600016F*/ public hidebysig specialname static 
          class [PresentationCore/*23000009*/]System.Windows.Media.FontFamily/*01000059*/ 
          get_FlowNodeCaptionFontFamily() cil managed
  // SIG: 00 00 12 81 65
  {
    // Method begins at RVA 0x21f0
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_FlowNodeCaptionFontFamily

  .method /*06000170*/ public hidebysig specialname static 
          valuetype [PresentationCore/*23000009*/]System.Windows.FontStyle/*0100005A*/ 
          get_FlowNodeCaptionFontStyle() cil managed
  // SIG: 00 00 11 81 69
  {
    // Method begins at RVA 0x21f1
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_FlowNodeCaptionFontStyle

  .method /*06000171*/ public hidebysig specialname static 
          class [PresentationCore/*23000009*/]System.Windows.Media.Typeface/*0100005B*/ 
          get_FlowElementCaptionTypeface() cil managed
  // SIG: 00 00 12 81 6D
  {
    // Method begins at RVA 0x21f2
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_FlowElementCaptionTypeface

  .method /*06000172*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x21f3
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::RegisterMetadata

  .method /*06000173*/ public hidebysig specialname 
          instance class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ 
          get_StartSymbol() cil managed
  // SIG: 20 00 12 81 41
  {
    .custom /*0C0000D1:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21f4
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_StartSymbol

  .method /*06000174*/ public hidebysig specialname 
          instance void  set_StartSymbol(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ 'value') cil managed
  // SIG: 20 01 01 12 81 41
  {
    .custom /*0C0000D2:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21f5
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::set_StartSymbol

  .method /*06000175*/ assembly hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> 
          GetConnectionPoints(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ obj) cil managed
  // SIG: 00 01 15 12 81 51 01 12 81 5D 12 6D
  {
    // Method begins at RVA 0x21f6
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetConnectionPoints

  .method /*06000176*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ 
          GetFalseConnectionPoint(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ obj) cil managed
  // SIG: 00 01 12 81 5D 12 6D
  {
    // Method begins at RVA 0x21f7
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetFalseConnectionPoint

  .method /*06000177*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          GetLinkModelItem(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ obj) cil managed
  // SIG: 00 01 12 2D 12 6D
  {
    // Method begins at RVA 0x21f8
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetLinkModelItem

  .method /*06000178*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          GetFlowElementModelItem(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ obj) cil managed
  // SIG: 00 01 12 2D 12 6D
  {
    // Method begins at RVA 0x21f9
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetFlowElementModelItem

  .method /*06000179*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ 
          GetTrueConnectionPoint(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ obj) cil managed
  // SIG: 00 01 12 81 5D 12 6D
  {
    // Method begins at RVA 0x21fa
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetTrueConnectionPoint

  .method /*0600017A*/ public hidebysig specialname 
          instance float64  get_FlowchartWidth() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x21fb
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_FlowchartWidth

  .method /*0600017B*/ public hidebysig specialname 
          instance void  set_FlowchartWidth(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    // Method begins at RVA 0x21fc
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::set_FlowchartWidth

  .method /*0600017C*/ public hidebysig specialname 
          instance float64  get_FlowchartHeight() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x21fd
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_FlowchartHeight

  .method /*0600017D*/ public hidebysig specialname 
          instance void  set_FlowchartHeight(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    // Method begins at RVA 0x21fe
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::set_FlowchartHeight

  .method /*0600017E*/ public hidebysig specialname 
          instance bool  get_ShowAllConditions() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21ff
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_ShowAllConditions

  .method /*0600017F*/ public hidebysig specialname 
          instance void  set_ShowAllConditions(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2200
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::set_ShowAllConditions

  .method /*06000180*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x2201
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::OnInitialized

  .method /*06000181*/ private hidebysig specialname 
          instance bool  get_IsRightToLeft() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2202
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::get_IsRightToLeft

  .method /*06000182*/ assembly hidebysig static 
          void  DropActivityBelow(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.ViewStateService/*0100005C*/ viewStateService,
                                  class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem,
                                  class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ activity,
                                  float64 interval) cil managed
  // SIG: 00 04 01 12 81 71 12 2D 12 80 85 0D
  {
    // Method begins at RVA 0x2203
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::DropActivityBelow

  .method /*06000183*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ 
          GetDestinationConnectionPointForAutoConnect(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ dest,
                                                      class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ srcConnPoint) cil managed
  // SIG: 00 02 12 81 5D 12 81 41 12 81 5D
  {
    // Method begins at RVA 0x2204
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetDestinationConnectionPointForAutoConnect

  .method /*06000184*/ public hidebysig newslot virtual final 
          instance void  DoAutoConnect(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e,
                                       class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ targetElement,
                                       valuetype [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.AutoConnectDirections/*0100005E*/ direction) cil managed
  // SIG: 20 03 01 12 81 75 12 81 41 11 81 79
  {
    // Method begins at RVA 0x2205
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::DoAutoConnect

  .method /*06000185*/ public hidebysig newslot virtual final 
          instance valuetype [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.AutoConnectDirections/*0100005E*/ 
          GetDirectionsAllowed(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e,
                               class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ targetElement) cil managed
  // SIG: 20 02 11 81 79 12 81 75 12 81 41
  {
    // Method begins at RVA 0x2206
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetDirectionsAllowed

  .method /*06000186*/ public hidebysig newslot virtual final 
          instance bool  CanAutoSplit(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e) cil managed
  // SIG: 20 01 02 12 81 75
  {
    // Method begins at RVA 0x2207
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::CanAutoSplit

  .method /*06000187*/ assembly hidebysig static 
          int32  GetConnectionPointIndex(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ element,
                                         class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ pnt) cil managed
  // SIG: 00 02 08 12 81 41 12 81 5D
  {
    // Method begins at RVA 0x2208
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetConnectionPointIndex

  .method /*06000188*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ 
          GetConnectionPointFromIndex(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ element,
                                      int32 index) cil managed
  // SIG: 00 02 12 81 5D 12 81 41 08
  {
    // Method begins at RVA 0x2209
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetConnectionPointFromIndex

  .method /*06000189*/ assembly hidebysig 
          instance class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ 
          GetView(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ item) cil managed
  // SIG: 20 01 12 81 41 12 2D
  {
    // Method begins at RVA 0x220a
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::GetView

  .method /*0600018A*/ public hidebysig newslot virtual final 
          instance void  DoAutoSplit(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e,
                                     class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/ connector) cil managed
  // SIG: 20 02 01 12 81 75 12 81 61
  {
    // Method begins at RVA 0x220b
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::DoAutoSplit

  .method /*0600018B*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000D3:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D4:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x220c
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::InitializeComponent

  .method /*0600018C*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000D5:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D6:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000D7:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x220d
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600018D*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000D8:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D9:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000DA:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x220e
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*0600018E*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x220f
    // Code size       0 (0x0)
  } // end of method FlowchartDesigner::.cctor

  .property /*17000048*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/
          DroppingTypeResolvingOptions()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/ System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_DroppingTypeResolvingOptions() /* 06000151 */
    .set instance void System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::set_DroppingTypeResolvingOptions(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/) /* 06000152 */
  } // end of property FlowchartDesigner::DroppingTypeResolvingOptions
  .property /*17000049*/ instance callconv(8) bool
          IsDefaultContainer()
  {
    .get instance bool System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_IsDefaultContainer() /* 0600015C */
  } // end of property FlowchartDesigner::IsDefaultContainer
  .property /*1700004A*/ instance callconv(8) bool
          IsResizing()
  {
    .get instance bool System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_IsResizing() /* 0600016B */
    .set instance void System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::set_IsResizing(bool) /* 0600016C */
  } // end of property FlowchartDesigner::IsResizing
  .property /*1700004B*/ callconv(8) float64
          FlowNodeCaptionFontSize()
  {
    .get float64 System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_FlowNodeCaptionFontSize() /* 0600016E */
  } // end of property FlowchartDesigner::FlowNodeCaptionFontSize
  .property /*1700004C*/ callconv(8) class [PresentationCore/*23000009*/]System.Windows.Media.FontFamily/*01000059*/
          FlowNodeCaptionFontFamily()
  {
    .get class [PresentationCore/*23000009*/]System.Windows.Media.FontFamily/*01000059*/ System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_FlowNodeCaptionFontFamily() /* 0600016F */
  } // end of property FlowchartDesigner::FlowNodeCaptionFontFamily
  .property /*1700004D*/ callconv(8) valuetype [PresentationCore/*23000009*/]System.Windows.FontStyle/*0100005A*/
          FlowNodeCaptionFontStyle()
  {
    .get valuetype [PresentationCore/*23000009*/]System.Windows.FontStyle/*0100005A*/ System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_FlowNodeCaptionFontStyle() /* 06000170 */
  } // end of property FlowchartDesigner::FlowNodeCaptionFontStyle
  .property /*1700004E*/ callconv(8) class [PresentationCore/*23000009*/]System.Windows.Media.Typeface/*0100005B*/
          FlowElementCaptionTypeface()
  {
    .get class [PresentationCore/*23000009*/]System.Windows.Media.Typeface/*0100005B*/ System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_FlowElementCaptionTypeface() /* 06000171 */
  } // end of property FlowchartDesigner::FlowElementCaptionTypeface
  .property /*1700004F*/ instance callconv(8) class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/
          StartSymbol()
  {
    .get instance class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_StartSymbol() /* 06000173 */
    .set instance void System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::set_StartSymbol(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/) /* 06000174 */
  } // end of property FlowchartDesigner::StartSymbol
  .property /*17000050*/ instance callconv(8) float64
          FlowchartWidth()
  {
    .get instance float64 System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_FlowchartWidth() /* 0600017A */
    .set instance void System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::set_FlowchartWidth(float64) /* 0600017B */
  } // end of property FlowchartDesigner::FlowchartWidth
  .property /*17000051*/ instance callconv(8) float64
          FlowchartHeight()
  {
    .get instance float64 System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_FlowchartHeight() /* 0600017C */
    .set instance void System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::set_FlowchartHeight(float64) /* 0600017D */
  } // end of property FlowchartDesigner::FlowchartHeight
  .property /*17000052*/ instance callconv(8) bool
          ShowAllConditions()
  {
    .get instance bool System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_ShowAllConditions() /* 0600017E */
    .set instance void System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::set_ShowAllConditions(bool) /* 0600017F */
  } // end of property FlowchartDesigner::ShowAllConditions
  .property /*17000053*/ instance callconv(8) bool
          IsRightToLeft()
  {
    .get instance bool System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/::get_IsRightToLeft() /* 06000181 */
  } // end of property FlowchartDesigner::IsRightToLeft
} // end of class System.Activities.Core.Presentation.FlowchartDesigner

.class /*02000036*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.ParallelSeparator
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.UserControl/*01000047*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000064*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ AllowedItemTypeProperty
  .field /*04000065*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ContextProperty
  .field /*04000066*/ assembly class System.Activities.Core.Presentation.ParallelSeparator/*02000036*/ ParallelSeparatorControl
  .field /*04000067*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Media.Animation.BeginStoryboard/*0100005F*/ Collapse_BeginStoryboardOnDrop
  .field /*04000068*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ grid
  .field /*04000069*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Shapes.Rectangle/*01000060*/ dropTarget
  .method /*0600018F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2210
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::.ctor

  .method /*06000190*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_AllowedItemType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x2211
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::get_AllowedItemType

  .method /*06000191*/ public hidebysig specialname 
          instance void  set_AllowedItemType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x2212
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::set_AllowedItemType

  .method /*06000192*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 
          get_Context() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x2213
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::get_Context

  .method /*06000193*/ public hidebysig specialname 
          instance void  set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2214
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::set_Context

  .method /*06000194*/ family hidebysig virtual 
          instance void  OnDragEnter(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e) cil managed
  // SIG: 20 01 01 12 81 75
  {
    // Method begins at RVA 0x2215
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::OnDragEnter

  .method /*06000195*/ family hidebysig virtual 
          instance void  OnDragLeave(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e) cil managed
  // SIG: 20 01 01 12 81 75
  {
    // Method begins at RVA 0x2216
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::OnDragLeave

  .method /*06000196*/ family hidebysig virtual 
          instance void  OnDrop(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e) cil managed
  // SIG: 20 01 01 12 81 75
  {
    // Method begins at RVA 0x2217
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::OnDrop

  .method /*06000197*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000DB:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000DC:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2218
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::InitializeComponent

  .method /*06000198*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000DD:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000DE:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000DF:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2219
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::System.Windows.Markup.IComponentConnector.Connect

  .method /*06000199*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x221a
    // Code size       0 (0x0)
  } // end of method ParallelSeparator::.cctor

  .property /*17000054*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          AllowedItemType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Core.Presentation.ParallelSeparator/*02000036*/::get_AllowedItemType() /* 06000190 */
    .set instance void System.Activities.Core.Presentation.ParallelSeparator/*02000036*/::set_AllowedItemType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000191 */
  } // end of property ParallelSeparator::AllowedItemType
  .property /*17000055*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/
          Context()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ System.Activities.Core.Presentation.ParallelSeparator/*02000036*/::get_Context() /* 06000192 */
    .set instance void System.Activities.Core.Presentation.ParallelSeparator/*02000036*/::set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/) /* 06000193 */
  } // end of property ParallelSeparator::Context
} // end of class System.Activities.Core.Presentation.ParallelSeparator

.class /*02000037*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.WhileDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*0600019A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x221b
    // Code size       0 (0x0)
  } // end of method WhileDesigner::.ctor

  .method /*0600019B*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x221c
    // Code size       0 (0x0)
  } // end of method WhileDesigner::RegisterMetadata

  .method /*0600019C*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000E0:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E1:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x221d
    // Code size       0 (0x0)
  } // end of method WhileDesigner::InitializeComponent

  .method /*0600019D*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000E2:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E3:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000E4:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x221e
    // Code size       0 (0x0)
  } // end of method WhileDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.WhileDesigner

.class /*02000038*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.DoWhileDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*0600019E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x221f
    // Code size       0 (0x0)
  } // end of method DoWhileDesigner::.ctor

  .method /*0600019F*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2220
    // Code size       0 (0x0)
  } // end of method DoWhileDesigner::RegisterMetadata

  .method /*060001A0*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000E5:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E6:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2221
    // Code size       0 (0x0)
  } // end of method DoWhileDesigner::InitializeComponent

  .method /*060001A1*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000E7:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E8:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000E9:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2222
    // Code size       0 (0x0)
  } // end of method DoWhileDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.DoWhileDesigner

.class /*02000039*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.ForEachDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .method /*060001A2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2223
    // Code size       0 (0x0)
  } // end of method ForEachDesigner::.ctor

  .method /*060001A3*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2224
    // Code size       0 (0x0)
  } // end of method ForEachDesigner::RegisterMetadata

  .method /*060001A4*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000EA:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000EB:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2225
    // Code size       0 (0x0)
  } // end of method ForEachDesigner::InitializeComponent

  .method /*060001A5*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000EC:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000ED:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000EE:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2226
    // Code size       0 (0x0)
  } // end of method ForEachDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*060001A6*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000EF:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F0:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000F1:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x2227
    // Code size       0 (0x0)
  } // end of method ForEachDesigner::System.Windows.Markup.IStyleConnector.Connect

} // end of class System.Activities.Core.Presentation.ForEachDesigner

.class /*0200003A*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.IfElseDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .method /*060001A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2228
    // Code size       0 (0x0)
  } // end of method IfElseDesigner::.ctor

  .method /*060001A8*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2229
    // Code size       0 (0x0)
  } // end of method IfElseDesigner::RegisterMetadata

  .method /*060001A9*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000F2:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F3:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x222a
    // Code size       0 (0x0)
  } // end of method IfElseDesigner::InitializeComponent

  .method /*060001AA*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000F4:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F5:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000F6:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x222b
    // Code size       0 (0x0)
  } // end of method IfElseDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*060001AB*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000F7:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F8:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000F9:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x222c
    // Code size       0 (0x0)
  } // end of method IfElseDesigner::System.Windows.Markup.IStyleConnector.Connect

} // end of class System.Activities.Core.Presentation.IfElseDesigner

.class /*0200003B*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowDecisionDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*0400006A*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ExpressionButtonVisibilityProperty
  .field /*0400006B*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ExpressionButtonColorProperty
  .field /*0400006C*/ assembly class System.Activities.Core.Presentation.FlowDecisionDesigner/*0200003B*/ controlRoot
  .field /*0400006D*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ rootGrid
  .field /*0400006E*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.TextBox/*0100002A*/ displayNameTextBox
  .field /*0400006F*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Button/*01000039*/ defaultAnnotationIndicator
  .method /*060001AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x222d
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::.ctor

  .method /*060001AD*/ public hidebysig specialname 
          instance valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ 
          get_ExpressionButtonVisibility() cil managed
  // SIG: 20 00 11 81 85
  {
    // Method begins at RVA 0x222e
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::get_ExpressionButtonVisibility

  .method /*060001AE*/ public hidebysig specialname 
          instance void  set_ExpressionButtonVisibility(valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ 'value') cil managed
  // SIG: 20 01 01 11 81 85
  {
    // Method begins at RVA 0x222f
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::set_ExpressionButtonVisibility

  .method /*060001AF*/ public hidebysig specialname 
          instance class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/ 
          get_ExpressionButtonColor() cil managed
  // SIG: 20 00 12 81 89
  {
    // Method begins at RVA 0x2230
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::get_ExpressionButtonColor

  .method /*060001B0*/ public hidebysig specialname 
          instance void  set_ExpressionButtonColor(class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/ 'value') cil managed
  // SIG: 20 01 01 12 81 89
  {
    // Method begins at RVA 0x2231
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::set_ExpressionButtonColor

  .method /*060001B1*/ public hidebysig specialname 
          instance bool  get_ExpressionShown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2232
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::get_ExpressionShown

  .method /*060001B2*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2234
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::RegisterMetadata

  .method /*060001B3*/ family hidebysig virtual 
          instance class [PresentationCore/*23000009*/]System.Windows.Automation.Peers.AutomationPeer/*01000063*/ 
          OnCreateAutomationPeer() cil managed
  // SIG: 20 00 12 81 8D
  {
    // Method begins at RVA 0x2240
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::OnCreateAutomationPeer

  .method /*060001B4*/ famorassem hidebysig virtual 
          instance void  OnEditAnnotation() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2241
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::OnEditAnnotation

  .method /*060001B5*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000FA:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000FB:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2242
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::InitializeComponent

  .method /*060001B6*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0000FC:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0000FD:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0000FE:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2243
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*060001B7*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2244
    // Code size       0 (0x0)
  } // end of method FlowDecisionDesigner::.cctor

  .property /*17000056*/ instance callconv(8) valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/
          ExpressionButtonVisibility()
  {
    .get instance valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ System.Activities.Core.Presentation.FlowDecisionDesigner/*0200003B*/::get_ExpressionButtonVisibility() /* 060001AD */
    .set instance void System.Activities.Core.Presentation.FlowDecisionDesigner/*0200003B*/::set_ExpressionButtonVisibility(valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/) /* 060001AE */
  } // end of property FlowDecisionDesigner::ExpressionButtonVisibility
  .property /*17000057*/ instance callconv(8) class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/
          ExpressionButtonColor()
  {
    .get instance class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/ System.Activities.Core.Presentation.FlowDecisionDesigner/*0200003B*/::get_ExpressionButtonColor() /* 060001AF */
    .set instance void System.Activities.Core.Presentation.FlowDecisionDesigner/*0200003B*/::set_ExpressionButtonColor(class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/) /* 060001B0 */
  } // end of property FlowDecisionDesigner::ExpressionButtonColor
  .property /*17000058*/ instance callconv(8) bool
          ExpressionShown()
  {
    .get instance bool System.Activities.Core.Presentation.FlowDecisionDesigner/*0200003B*/::get_ExpressionShown() /* 060001B1 */
  } // end of property FlowDecisionDesigner::ExpressionShown
} // end of class System.Activities.Core.Presentation.FlowDecisionDesigner

.class /*0200003C*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowSwitchDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000070*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ExpressionButtonVisibilityProperty
  .field /*04000071*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ExpressionButtonColorProperty
  .field /*04000072*/ assembly class System.Activities.Core.Presentation.FlowSwitchDesigner/*0200003C*/ controlRoot
  .field /*04000073*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ rootGrid
  .field /*04000074*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.TextBox/*0100002A*/ displayNameTextBox
  .field /*04000075*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Button/*01000039*/ defaultAnnotationIndicator
  .method /*060001B8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2245
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::.ctor

  .method /*060001B9*/ public hidebysig specialname 
          instance valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ 
          get_ExpressionButtonVisibility() cil managed
  // SIG: 20 00 11 81 85
  {
    // Method begins at RVA 0x2246
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::get_ExpressionButtonVisibility

  .method /*060001BA*/ public hidebysig specialname 
          instance void  set_ExpressionButtonVisibility(valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ 'value') cil managed
  // SIG: 20 01 01 11 81 85
  {
    // Method begins at RVA 0x2247
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::set_ExpressionButtonVisibility

  .method /*060001BB*/ public hidebysig specialname 
          instance class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/ 
          get_ExpressionButtonColor() cil managed
  // SIG: 20 00 12 81 89
  {
    // Method begins at RVA 0x2248
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::get_ExpressionButtonColor

  .method /*060001BC*/ public hidebysig specialname 
          instance void  set_ExpressionButtonColor(class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/ 'value') cil managed
  // SIG: 20 01 01 12 81 89
  {
    // Method begins at RVA 0x2249
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::set_ExpressionButtonColor

  .method /*060001BD*/ public hidebysig specialname 
          instance bool  get_ExpressionShown() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x224a
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::get_ExpressionShown

  .method /*060001BE*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x224c
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::RegisterMetadata

  .method /*060001BF*/ family hidebysig virtual 
          instance class [PresentationCore/*23000009*/]System.Windows.Automation.Peers.AutomationPeer/*01000063*/ 
          OnCreateAutomationPeer() cil managed
  // SIG: 20 00 12 81 8D
  {
    // Method begins at RVA 0x2258
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::OnCreateAutomationPeer

  .method /*060001C0*/ famorassem hidebysig virtual 
          instance void  OnEditAnnotation() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2259
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::OnEditAnnotation

  .method /*060001C1*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000FF:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000100:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x225a
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::InitializeComponent

  .method /*060001C2*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000101:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000102:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000103:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x225b
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*060001C3*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x225c
    // Code size       0 (0x0)
  } // end of method FlowSwitchDesigner::.cctor

  .property /*17000059*/ instance callconv(8) valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/
          ExpressionButtonVisibility()
  {
    .get instance valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ System.Activities.Core.Presentation.FlowSwitchDesigner/*0200003C*/::get_ExpressionButtonVisibility() /* 060001B9 */
    .set instance void System.Activities.Core.Presentation.FlowSwitchDesigner/*0200003C*/::set_ExpressionButtonVisibility(valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/) /* 060001BA */
  } // end of property FlowSwitchDesigner::ExpressionButtonVisibility
  .property /*1700005A*/ instance callconv(8) class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/
          ExpressionButtonColor()
  {
    .get instance class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/ System.Activities.Core.Presentation.FlowSwitchDesigner/*0200003C*/::get_ExpressionButtonColor() /* 060001BB */
    .set instance void System.Activities.Core.Presentation.FlowSwitchDesigner/*0200003C*/::set_ExpressionButtonColor(class [PresentationCore/*23000009*/]System.Windows.Media.Brush/*01000062*/) /* 060001BC */
  } // end of property FlowSwitchDesigner::ExpressionButtonColor
  .property /*1700005B*/ instance callconv(8) bool
          ExpressionShown()
  {
    .get instance bool System.Activities.Core.Presentation.FlowSwitchDesigner/*0200003C*/::get_ExpressionShown() /* 060001BD */
  } // end of property FlowSwitchDesigner::ExpressionShown
} // end of class System.Activities.Core.Presentation.FlowSwitchDesigner

.class /*0200003D*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.InteropDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*060001C4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225d
    // Code size       0 (0x0)
  } // end of method InteropDesigner::.ctor

  .method /*060001C5*/ public hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> 
          get_Filter() cil managed
  // SIG: 00 00 15 12 71 02 12 0D 02
  {
    // Method begins at RVA 0x225e
    // Code size       0 (0x0)
  } // end of method InteropDesigner::get_Filter

  .method /*060001C6*/ public hidebysig specialname static 
          string  get_InteropTypeAssemblyQualifiedName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x225f
    // Code size       0 (0x0)
  } // end of method InteropDesigner::get_InteropTypeAssemblyQualifiedName

  .method /*060001C7*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2260
    // Code size       0 (0x0)
  } // end of method InteropDesigner::OnModelItemChanged

  .method /*060001C8*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2264
    // Code size       0 (0x0)
  } // end of method InteropDesigner::RegisterMetadata

  .method /*060001C9*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000104:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000105:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2270
    // Code size       0 (0x0)
  } // end of method InteropDesigner::InitializeComponent

  .method /*060001CA*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000106:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000107:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000108:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2271
    // Code size       0 (0x0)
  } // end of method InteropDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*060001CB*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2272
    // Code size       0 (0x0)
  } // end of method InteropDesigner::.cctor

  .property /*1700005C*/ callconv(8) class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>
          Filter()
  {
    .get class [mscorlib/*23000001*/]System.Func`2/*0100001C*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> System.Activities.Core.Presentation.InteropDesigner/*0200003D*/::get_Filter() /* 060001C5 */
  } // end of property InteropDesigner::Filter
  .property /*1700005D*/ callconv(8) string
          InteropTypeAssemblyQualifiedName()
  {
    .get string System.Activities.Core.Presentation.InteropDesigner/*0200003D*/::get_InteropTypeAssemblyQualifiedName() /* 060001C6 */
  } // end of property InteropDesigner::InteropTypeAssemblyQualifiedName
} // end of class System.Activities.Core.Presentation.InteropDesigner

.class /*0200003E*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowchartResizeGrip
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.Control/*01000064*/
{
  .field /*04000076*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ IconProperty
  .field /*04000077*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ParentFlowchartDesignerProperty
  .field /*04000078*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ParentGridProperty
  .field /*04000079*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ DisabledProperty
  .method /*060001CC*/ public hidebysig specialname 
          instance class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/ 
          get_Icon() cil managed
  // SIG: 20 00 12 81 95
  {
    // Method begins at RVA 0x2273
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::get_Icon

  .method /*060001CD*/ public hidebysig specialname 
          instance void  set_Icon(class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/ 'value') cil managed
  // SIG: 20 01 01 12 81 95
  {
    // Method begins at RVA 0x2274
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::set_Icon

  .method /*060001CE*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/ 
          get_ParentFlowchartDesigner() cil managed
  // SIG: 20 00 12 80 D4
  {
    // Method begins at RVA 0x2275
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::get_ParentFlowchartDesigner

  .method /*060001CF*/ public hidebysig specialname 
          instance void  set_ParentFlowchartDesigner(class System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D4
  {
    // Method begins at RVA 0x2276
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::set_ParentFlowchartDesigner

  .method /*060001D0*/ public hidebysig specialname 
          instance class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ 
          get_ParentGrid() cil managed
  // SIG: 20 00 12 80 A5
  {
    // Method begins at RVA 0x2277
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::get_ParentGrid

  .method /*060001D1*/ public hidebysig specialname 
          instance void  set_ParentGrid(class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A5
  {
    // Method begins at RVA 0x2278
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::set_ParentGrid

  .method /*060001D2*/ public hidebysig specialname 
          instance bool  get_Disabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2279
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::get_Disabled

  .method /*060001D3*/ public hidebysig specialname 
          instance void  set_Disabled(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x227a
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::set_Disabled

  .method /*060001D4*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x227b
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::OnInitialized

  .method /*060001D5*/ family hidebysig virtual 
          instance void  OnPreviewMouseLeftButtonDown(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x227c
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::OnPreviewMouseLeftButtonDown

  .method /*060001D6*/ family hidebysig virtual 
          instance void  OnMouseMove(class [PresentationCore/*23000009*/]System.Windows.Input.MouseEventArgs/*01000067*/ args) cil managed
  // SIG: 20 01 01 12 81 9D
  {
    // Method begins at RVA 0x227d
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::OnMouseMove

  .method /*060001D7*/ family hidebysig virtual 
          instance void  OnPreviewMouseLeftButtonUp(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x227e
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::OnPreviewMouseLeftButtonUp

  .method /*060001D8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x227f
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::.ctor

  .method /*060001D9*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2280
    // Code size       0 (0x0)
  } // end of method FlowchartResizeGrip::.cctor

  .property /*1700005E*/ instance callconv(8) class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/
          Icon()
  {
    .get instance class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/ System.Activities.Core.Presentation.FlowchartResizeGrip/*0200003E*/::get_Icon() /* 060001CC */
    .set instance void System.Activities.Core.Presentation.FlowchartResizeGrip/*0200003E*/::set_Icon(class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/) /* 060001CD */
  } // end of property FlowchartResizeGrip::Icon
  .property /*1700005F*/ instance callconv(8) class System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/
          ParentFlowchartDesigner()
  {
    .get instance class System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/ System.Activities.Core.Presentation.FlowchartResizeGrip/*0200003E*/::get_ParentFlowchartDesigner() /* 060001CE */
    .set instance void System.Activities.Core.Presentation.FlowchartResizeGrip/*0200003E*/::set_ParentFlowchartDesigner(class System.Activities.Core.Presentation.FlowchartDesigner/*02000035*/) /* 060001CF */
  } // end of property FlowchartResizeGrip::ParentFlowchartDesigner
  .property /*17000060*/ instance callconv(8) class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/
          ParentGrid()
  {
    .get instance class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ System.Activities.Core.Presentation.FlowchartResizeGrip/*0200003E*/::get_ParentGrid() /* 060001D0 */
    .set instance void System.Activities.Core.Presentation.FlowchartResizeGrip/*0200003E*/::set_ParentGrid(class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/) /* 060001D1 */
  } // end of property FlowchartResizeGrip::ParentGrid
  .property /*17000061*/ instance callconv(8) bool
          Disabled()
  {
    .get instance bool System.Activities.Core.Presentation.FlowchartResizeGrip/*0200003E*/::get_Disabled() /* 060001D2 */
    .set instance void System.Activities.Core.Presentation.FlowchartResizeGrip/*0200003E*/::set_Disabled(bool) /* 060001D3 */
  } // end of property FlowchartResizeGrip::Disabled
} // end of class System.Activities.Core.Presentation.FlowchartResizeGrip

.class /*0200003F*/ private abstract auto ansi sealed beforefieldinit System.Activities.Core.Presentation.PropertyNames
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .field /*0400007A*/ public static initonly string ObjectInitializationAction
  .field /*0400007B*/ public static initonly string Action
  .field /*0400007C*/ public static initonly string ActionArgument
  .field /*0400007D*/ public static initonly string ActionArgument1
  .field /*0400007E*/ public static initonly string ActionArgument2
  .field /*0400007F*/ public static initonly string ActionHandler
  .field /*04000080*/ public static initonly string AssignSequence
  .field /*04000081*/ public static initonly string VarDataReader
  .field /*04000082*/ public static initonly string VarRecord
  .field /*04000083*/ public static initonly string NameProperty
  .field /*04000084*/ public static initonly string AssignTo
  .field /*04000085*/ public static initonly string AssignValue
  .field /*04000086*/ public static initonly string DisplayName
  .field /*04000087*/ public static initonly string Parameters
  .field /*04000088*/ public static initonly string ArgumentName
  .field /*04000089*/ public static initonly string Expression
  .field /*0400008A*/ public static initonly string VBExpressionText
  .field /*0400008B*/ public static initonly string ConnectionContext
  .field /*0400008C*/ public static initonly string ResultsCollection
  .field /*0400008D*/ public static initonly string RowsAffected
  .field /*0400008E*/ public static initonly string ParameterDirection
  .field /*0400008F*/ public static initonly string ArgumentType
  .field /*04000090*/ public static initonly string Body
  .field /*04000091*/ public static initonly string ExceptionType
  .field /*04000092*/ public static initonly string ResultProperty
  .method /*060001DA*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2281
    // Code size       0 (0x0)
  } // end of method PropertyNames::.cctor

} // end of class System.Activities.Core.Presentation.PropertyNames

.class /*02000040*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.CaseLabelVisibilityConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*060001DB*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2282
    // Code size       0 (0x0)
  } // end of method CaseLabelVisibilityConverter::Convert

  .method /*060001DC*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2283
    // Code size       0 (0x0)
  } // end of method CaseLabelVisibilityConverter::ConvertBack

  .method /*060001DD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2284
    // Code size       0 (0x0)
  } // end of method CaseLabelVisibilityConverter::.ctor

} // end of class System.Activities.Core.Presentation.CaseLabelVisibilityConverter

.class /*02000041*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowchartExpressionAutomationPeer
       extends [PresentationCore/*23000009*/]System.Windows.Automation.Peers.UIElementAutomationPeer/*01000068*/
{
  .method /*060001DE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [PresentationFramework/*23000006*/]System.Windows.FrameworkElement/*01000069*/ owner,
                               class [PresentationCore/*23000009*/]System.Windows.Automation.Peers.AutomationPeer/*01000063*/ wrappedAutomationPeer) cil managed
  // SIG: 20 02 01 12 81 A5 12 81 8D
  {
    // Method begins at RVA 0x2285
    // Code size       0 (0x0)
  } // end of method FlowchartExpressionAutomationPeer::.ctor

  .method /*060001DF*/ family hidebysig virtual 
          instance string  GetItemStatusCore() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2286
    // Code size       0 (0x0)
  } // end of method FlowchartExpressionAutomationPeer::GetItemStatusCore

  .method /*060001E0*/ family hidebysig virtual 
          instance string  GetClassNameCore() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2287
    // Code size       0 (0x0)
  } // end of method FlowchartExpressionAutomationPeer::GetClassNameCore

  .method /*060001E1*/ family hidebysig virtual 
          instance string  GetNameCore() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2288
    // Code size       0 (0x0)
  } // end of method FlowchartExpressionAutomationPeer::GetNameCore

  .method /*060001E2*/ family hidebysig virtual 
          instance string  GetAutomationIdCore() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2289
    // Code size       0 (0x0)
  } // end of method FlowchartExpressionAutomationPeer::GetAutomationIdCore

} // end of class System.Activities.Core.Presentation.FlowchartExpressionAutomationPeer

.class /*02000042*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowchartSizeFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ViewStateAttachedPropertyFeature/*0100006A*/
{
  .class /*020000A3*/ auto ansi sealed nested private beforefieldinit '<get_AttachedProperties>d__5'
         extends [mscorlib/*23000001*/]System.Object/*01000002*/
         implements class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000004*/,
                    [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/,
                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000003*/,
                    [mscorlib/*23000001*/]System.IDisposable/*01000023*/,
                    [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/
  {
    .custom /*0C000071:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    .method /*0600048F*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0001DC:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.IDisposable/*01000023*/::Dispose /*01000023::0A000005*/ 
      // Method begins at RVA 0x257b
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__5'::System.IDisposable.Dispose

    .method /*06000490*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::MoveNext /*01000024::0A000006*/ 
      // Method begins at RVA 0x257c
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__5'::MoveNext

    .method /*06000491*/ private hidebysig newslot specialname virtual final 
            instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/ 
            'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'() cil managed
    // SIG: 20 00 12 81 AD
    {
      .custom /*0C0001DD:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000003*/::get_Current() /* 0A00000C */
      // Method begins at RVA 0x257d
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__5'::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'

    .method /*06000492*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0001DE:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::Reset /*01000024::0A000008*/ 
      // Method begins at RVA 0x257e
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__5'::System.Collections.IEnumerator.Reset

    .method /*06000493*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C0001DF:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::get_Current /*01000024::0A000009*/ 
      // Method begins at RVA 0x257f
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__5'::System.Collections.IEnumerator.get_Current

    .method /*06000494*/ private hidebysig newslot virtual final 
            instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> 
            'System.Collections.Generic.IEnumerable<System.Activities.Presentation.Model.AttachedPropertyInfo>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 95 01 12 81 AD
    {
      .custom /*0C0001E0:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override  method instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<!0> class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000004*/::GetEnumerator() /* 0A00000D */
      // Method begins at RVA 0x2580
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__5'::'System.Collections.Generic.IEnumerable<System.Activities.Presentation.Model.AttachedPropertyInfo>.GetEnumerator'

    .method /*06000495*/ private hidebysig newslot virtual final 
            instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 91
    {
      .custom /*0C0001E1:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/::GetEnumerator /*01000026::0A00000B*/ 
      // Method begins at RVA 0x2581
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__5'::System.Collections.IEnumerable.GetEnumerator

    .method /*06000496*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2582
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__5'::.ctor

    .property /*17000134*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/
            'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.Current'()
    {
      .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/ System.Activities.Core.Presentation.FlowchartSizeFeature/*02000042*//'<get_AttachedProperties>d__5'/*020000A3*/::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'() /* 06000491 */
    } // end of property '<get_AttachedProperties>d__5'::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.Current'
    .property /*17000135*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object System.Activities.Core.Presentation.FlowchartSizeFeature/*02000042*//'<get_AttachedProperties>d__5'/*020000A3*/::System.Collections.IEnumerator.get_Current() /* 06000493 */
    } // end of property '<get_AttachedProperties>d__5'::System.Collections.IEnumerator.Current
  } // end of class '<get_AttachedProperties>d__5'

  .field /*04000093*/ public static literal string WidthPropertyName = "Width"
  .field /*04000094*/ public static literal string HeightPropertyName = "Height"
  .field /*04000095*/ public static literal float64 DefaultWidth = float64(600)
  .field /*04000096*/ public static literal float64 DefaultHeight = float64(600)
  .method /*060001E3*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> 
          get_AttachedProperties() cil managed
  // SIG: 20 00 15 12 7D 01 12 81 AD
  {
    .custom /*0C000109:0A000028*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*010000AA*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000028 */ = ( 01 00 55 53 79 73 74 65 6D 2E 41 63 74 69 76 69   // ..USystem.Activi
                                                                                                                                                                                                                               74 69 65 73 2E 43 6F 72 65 2E 50 72 65 73 65 6E   // ties.Core.Presen
                                                                                                                                                                                                                               74 61 74 69 6F 6E 2E 46 6C 6F 77 63 68 61 72 74   // tation.Flowchart
                                                                                                                                                                                                                               53 69 7A 65 46 65 61 74 75 72 65 2B 3C 67 65 74   // SizeFeature+<get
                                                                                                                                                                                                                               5F 41 74 74 61 63 68 65 64 50 72 6F 70 65 72 74   // _AttachedPropert
                                                                                                                                                                                                                               69 65 73 3E 64 5F 5F 35 00 00 )                   // ies>d__5..
    // Method begins at RVA 0x228a
    // Code size       0 (0x0)
  } // end of method FlowchartSizeFeature::get_AttachedProperties

  .method /*060001E4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228b
    // Code size       0 (0x0)
  } // end of method FlowchartSizeFeature::.ctor

  .property /*17000062*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>
          AttachedProperties()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> System.Activities.Core.Presentation.FlowchartSizeFeature/*02000042*/::get_AttachedProperties() /* 060001E3 */
  } // end of property FlowchartSizeFeature::AttachedProperties
} // end of class System.Activities.Core.Presentation.FlowchartSizeFeature

.class /*02000043*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.SR
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .custom /*0C000033:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                    string) /* 0A000025 */ = ( 01 00 33 53 79 73 74 65 6D 2E 52 65 73 6F 75 72   // ..3System.Resour
                                                                                                                                                               63 65 73 2E 54 6F 6F 6C 73 2E 53 74 72 6F 6E 67   // ces.Tools.Strong
                                                                                                                                                               6C 79 54 79 70 65 64 52 65 73 6F 75 72 63 65 42   // lyTypedResourceB
                                                                                                                                                               75 69 6C 64 65 72 07 34 2E 30 2E 30 2E 30 00 00 ) // uilder.4.0.0.0..
  .custom /*0C000034:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
  .custom /*0C000035:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
  .method /*060001E5*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228c
    // Code size       0 (0x0)
  } // end of method SR::.ctor

  .method /*060001E6*/ assembly hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Resources.ResourceManager/*0100006C*/ 
          get_ResourceManager() cil managed
  // SIG: 00 00 12 81 B1
  {
    // Method begins at RVA 0x228d
    // Code size       0 (0x0)
  } // end of method SR::get_ResourceManager

  .method /*060001E7*/ assembly hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 11
  {
    // Method begins at RVA 0x228e
    // Code size       0 (0x0)
  } // end of method SR::get_Culture

  .method /*060001E8*/ assembly hidebysig specialname static 
          void  set_Culture(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ 'value') cil managed
  // SIG: 00 01 01 12 11
  {
    // Method begins at RVA 0x228f
    // Code size       0 (0x0)
  } // end of method SR::set_Culture

  .method /*060001E9*/ assembly hidebysig specialname static 
          string  get_AddActivityHintText() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2290
    // Code size       0 (0x0)
  } // end of method SR::get_AddActivityHintText

  .method /*060001EA*/ assembly hidebysig specialname static 
          string  get_AddNewCase() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2291
    // Code size       0 (0x0)
  } // end of method SR::get_AddNewCase

  .method /*060001EB*/ assembly hidebysig specialname static 
          string  get_AutoConnect() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2292
    // Code size       0 (0x0)
  } // end of method SR::get_AutoConnect

  .method /*060001EC*/ assembly hidebysig specialname static 
          string  get_AutoSplit() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2293
    // Code size       0 (0x0)
  } // end of method SR::get_AutoSplit

  .method /*060001ED*/ assembly hidebysig specialname static 
          string  get_CannotConstructConnectionSegment() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2294
    // Code size       0 (0x0)
  } // end of method SR::get_CannotConstructConnectionSegment

  .method /*060001EE*/ assembly hidebysig specialname static 
          string  get_CannotCreateLink() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2295
    // Code size       0 (0x0)
  } // end of method SR::get_CannotCreateLink

  .method /*060001EF*/ assembly hidebysig specialname static 
          string  get_CannotCreateTransitionFromAncestorToDescendant() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2296
    // Code size       0 (0x0)
  } // end of method SR::get_CannotCreateTransitionFromAncestorToDescendant

  .method /*060001F0*/ assembly hidebysig specialname static 
          string  get_CannotCreateTransitionToCompositeState() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2297
    // Code size       0 (0x0)
  } // end of method SR::get_CannotCreateTransitionToCompositeState

  .method /*060001F1*/ assembly hidebysig specialname static 
          string  get_CannotPasteSendReplyOrReceiveReply() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2298
    // Code size       0 (0x0)
  } // end of method SR::get_CannotPasteSendReplyOrReceiveReply

  .method /*060001F2*/ assembly hidebysig specialname static 
          string  get_CannotSetCompositeStateAsInitialState() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2299
    // Code size       0 (0x0)
  } // end of method SR::get_CannotSetCompositeStateAsInitialState

  .method /*060001F3*/ assembly hidebysig specialname static 
          string  get_CannotSetFinalStateAsInitialState() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x229a
    // Code size       0 (0x0)
  } // end of method SR::get_CannotSetFinalStateAsInitialState

  .method /*060001F4*/ assembly hidebysig specialname static 
          string  get_CaseFormat() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x229b
    // Code size       0 (0x0)
  } // end of method SR::get_CaseFormat

  .method /*060001F5*/ assembly hidebysig specialname static 
          string  get_ChangeCaseValue() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x229c
    // Code size       0 (0x0)
  } // end of method SR::get_ChangeCaseValue

  .method /*060001F6*/ assembly hidebysig specialname static 
          string  get_ChangeTypeCollectionEditingScopeDesc() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x229d
    // Code size       0 (0x0)
  } // end of method SR::get_ChangeTypeCollectionEditingScopeDesc

  .method /*060001F7*/ assembly hidebysig specialname static 
          string  get_ClearInitialState() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x229e
    // Code size       0 (0x0)
  } // end of method SR::get_ClearInitialState

  .method /*060001F8*/ assembly hidebysig specialname static 
          string  get_ConnectionAutomationPropertiesName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x229f
    // Code size       0 (0x0)
  } // end of method SR::get_ConnectionAutomationPropertiesName

  .method /*060001F9*/ assembly hidebysig specialname static 
          string  get_ConnectionNotFoundInConfig() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a0
    // Code size       0 (0x0)
  } // end of method SR::get_ConnectionNotFoundInConfig

  .method /*060001FA*/ assembly hidebysig specialname static 
          string  get_CopyTransitionToolTip() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a1
    // Code size       0 (0x0)
  } // end of method SR::get_CopyTransitionToolTip

  .method /*060001FB*/ assembly hidebysig specialname static 
          string  get_CorrelationInitializerNotSupported() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a2
    // Code size       0 (0x0)
  } // end of method SR::get_CorrelationInitializerNotSupported

  .method /*060001FC*/ assembly hidebysig specialname static 
          string  get_CreateParameter() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a3
    // Code size       0 (0x0)
  } // end of method SR::get_CreateParameter

  .method /*060001FD*/ assembly hidebysig specialname static 
          string  get_CreateTransition() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a4
    // Code size       0 (0x0)
  } // end of method SR::get_CreateTransition

  .method /*060001FE*/ assembly hidebysig specialname static 
          string  get_CustomConnectionStringLabel() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a5
    // Code size       0 (0x0)
  } // end of method SR::get_CustomConnectionStringLabel

  .method /*060001FF*/ assembly hidebysig specialname static 
          string  get_DefaultCaseExists() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a6
    // Code size       0 (0x0)
  } // end of method SR::get_DefaultCaseExists

  .method /*06000200*/ assembly hidebysig specialname static 
          string  get_DefineContent() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a7
    // Code size       0 (0x0)
  } // end of method SR::get_DefineContent

  .method /*06000201*/ assembly hidebysig specialname static 
          string  get_DelegateArgumentMissing() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a8
    // Code size       0 (0x0)
  } // end of method SR::get_DelegateArgumentMissing

  .method /*06000202*/ assembly hidebysig specialname static 
          string  get_DelegateArgumentsDirectionalityMismatch() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22a9
    // Code size       0 (0x0)
  } // end of method SR::get_DelegateArgumentsDirectionalityMismatch

  .method /*06000203*/ assembly hidebysig specialname static 
          string  get_DelegateInArgumentTypeMismatch() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22aa
    // Code size       0 (0x0)
  } // end of method SR::get_DelegateInArgumentTypeMismatch

  .method /*06000204*/ assembly hidebysig specialname static 
          string  get_DelegateOutArgumentTypeMismatch() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22ab
    // Code size       0 (0x0)
  } // end of method SR::get_DelegateOutArgumentTypeMismatch

  .method /*06000205*/ assembly hidebysig specialname static 
          string  get_DuplicateCaseKey() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22ac
    // Code size       0 (0x0)
  } // end of method SR::get_DuplicateCaseKey

  .method /*06000206*/ assembly hidebysig specialname static 
          string  get_DuplicateKeyName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22ad
    // Code size       0 (0x0)
  } // end of method SR::get_DuplicateKeyName

  .method /*06000207*/ assembly hidebysig specialname static 
          string  get_EditingCommand() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22ae
    // Code size       0 (0x0)
  } // end of method SR::get_EditingCommand

  .method /*06000208*/ assembly hidebysig specialname static 
          string  get_EditingConnectionSettings() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22af
    // Code size       0 (0x0)
  } // end of method SR::get_EditingConnectionSettings

  .method /*06000209*/ assembly hidebysig specialname static 
          string  get_EditingParameters() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b0
    // Code size       0 (0x0)
  } // end of method SR::get_EditingParameters

  .method /*0600020A*/ assembly hidebysig specialname static 
          string  get_EditParametersUndoUnitName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b1
    // Code size       0 (0x0)
  } // end of method SR::get_EditParametersUndoUnitName

  .method /*0600020B*/ assembly hidebysig specialname static 
          string  get_EditStateToolTip() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b2
    // Code size       0 (0x0)
  } // end of method SR::get_EditStateToolTip

  .method /*0600020C*/ assembly hidebysig specialname static 
          string  get_EditTransitionTooltip() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b3
    // Code size       0 (0x0)
  } // end of method SR::get_EditTransitionTooltip

  .method /*0600020D*/ assembly hidebysig specialname static 
          string  get_EmptyName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b4
    // Code size       0 (0x0)
  } // end of method SR::get_EmptyName

  .method /*0600020E*/ assembly hidebysig specialname static 
          string  get_EqualityError() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b5
    // Code size       0 (0x0)
  } // end of method SR::get_EqualityError

  .method /*0600020F*/ assembly hidebysig specialname static 
          string  get_ErrorMessageBoxTitle() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b6
    // Code size       0 (0x0)
  } // end of method SR::get_ErrorMessageBoxTitle

  .method /*06000210*/ assembly hidebysig specialname static 
          string  get_ExpressionDefaultText() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b7
    // Code size       0 (0x0)
  } // end of method SR::get_ExpressionDefaultText

  .method /*06000211*/ assembly hidebysig specialname static 
          string  get_FCCreateLink() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b8
    // Code size       0 (0x0)
  } // end of method SR::get_FCCreateLink

  .method /*06000212*/ assembly hidebysig specialname static 
          string  get_FCDeleteLink() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22b9
    // Code size       0 (0x0)
  } // end of method SR::get_FCDeleteLink

  .method /*06000213*/ assembly hidebysig specialname static 
          string  get_FCDeleteShape() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22ba
    // Code size       0 (0x0)
  } // end of method SR::get_FCDeleteShape

  .method /*06000214*/ assembly hidebysig specialname static 
          string  get_FCFalseBranchExists() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22bb
    // Code size       0 (0x0)
  } // end of method SR::get_FCFalseBranchExists

  .method /*06000215*/ assembly hidebysig specialname static 
          string  get_FCFlowConditionLinkError() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22bc
    // Code size       0 (0x0)
  } // end of method SR::get_FCFlowConditionLinkError

  .method /*06000216*/ assembly hidebysig specialname static 
          string  get_FCFlowConditionLinksExist() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22bd
    // Code size       0 (0x0)
  } // end of method SR::get_FCFlowConditionLinksExist

  .method /*06000217*/ assembly hidebysig specialname static 
          string  get_FCFlowDecisionFalseMarker() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22be
    // Code size       0 (0x0)
  } // end of method SR::get_FCFlowDecisionFalseMarker

  .method /*06000218*/ assembly hidebysig specialname static 
          string  get_FCFlowDecisionTrueMarker() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22bf
    // Code size       0 (0x0)
  } // end of method SR::get_FCFlowDecisionTrueMarker

  .method /*06000219*/ assembly hidebysig specialname static 
          string  get_FCInvalidLink() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c0
    // Code size       0 (0x0)
  } // end of method SR::get_FCInvalidLink

  .method /*0600021A*/ assembly hidebysig specialname static 
          string  get_FCLinkMove() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c1
    // Code size       0 (0x0)
  } // end of method SR::get_FCLinkMove

  .method /*0600021B*/ assembly hidebysig specialname static 
          string  get_FCNextLinkDefined() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c2
    // Code size       0 (0x0)
  } // end of method SR::get_FCNextLinkDefined

  .method /*0600021C*/ assembly hidebysig specialname static 
          string  get_FCResizeUndoUnitName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c3
    // Code size       0 (0x0)
  } // end of method SR::get_FCResizeUndoUnitName

  .method /*0600021D*/ assembly hidebysig specialname static 
          string  get_FCTrueBranchExists() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c4
    // Code size       0 (0x0)
  } // end of method SR::get_FCTrueBranchExists

  .method /*0600021E*/ assembly hidebysig specialname static 
          string  get_FillDelegateArguments() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c5
    // Code size       0 (0x0)
  } // end of method SR::get_FillDelegateArguments

  .method /*0600021F*/ assembly hidebysig specialname static 
          string  get_FlowSwitchCaseRenameEditingScopeDesc() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c6
    // Code size       0 (0x0)
  } // end of method SR::get_FlowSwitchCaseRenameEditingScopeDesc

  .method /*06000220*/ assembly hidebysig specialname static 
          string  get_FlowSwitchDefaultCaseDisplayNameEditingScopeDesc() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c7
    // Code size       0 (0x0)
  } // end of method SR::get_FlowSwitchDefaultCaseDisplayNameEditingScopeDesc

  .method /*06000221*/ assembly hidebysig specialname static 
          string  get_InitialStateConnectionPointTooltip() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c8
    // Code size       0 (0x0)
  } // end of method SR::get_InitialStateConnectionPointTooltip

  .method /*06000222*/ assembly hidebysig specialname static 
          string  get_InvalidFlowSwitchCaseMessage() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22c9
    // Code size       0 (0x0)
  } // end of method SR::get_InvalidFlowSwitchCaseMessage

  .method /*06000223*/ assembly hidebysig specialname static 
          string  get_ItemMove() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22ca
    // Code size       0 (0x0)
  } // end of method SR::get_ItemMove

  .method /*06000224*/ assembly hidebysig specialname static 
          string  get_MoveLink() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22cb
    // Code size       0 (0x0)
  } // end of method SR::get_MoveLink

  .method /*06000225*/ assembly hidebysig specialname static 
          string  get_NewCaseHint() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22cc
    // Code size       0 (0x0)
  } // end of method SR::get_NewCaseHint

  .method /*06000226*/ assembly hidebysig specialname static 
          string  get_NotSupportedCaseKeyStringConversion() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22cd
    // Code size       0 (0x0)
  } // end of method SR::get_NotSupportedCaseKeyStringConversion

  .method /*06000227*/ assembly hidebysig specialname static 
          string  get_NotSupportedEncoding() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22ce
    // Code size       0 (0x0)
  } // end of method SR::get_NotSupportedEncoding

  .method /*06000228*/ assembly hidebysig specialname static 
          string  get_NullName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22cf
    // Code size       0 (0x0)
  } // end of method SR::get_NullName

  .method /*06000229*/ assembly hidebysig specialname static 
          string  get_NullOrEmptyKeyName() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d0
    // Code size       0 (0x0)
  } // end of method SR::get_NullOrEmptyKeyName

  .method /*0600022A*/ assembly hidebysig specialname static 
          string  get_ParameterDialogTitle() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d1
    // Code size       0 (0x0)
  } // end of method SR::get_ParameterDialogTitle

  .method /*0600022B*/ assembly hidebysig specialname static 
          string  get_PasteTransitionOnFinalState() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d2
    // Code size       0 (0x0)
  } // end of method SR::get_PasteTransitionOnFinalState

  .method /*0600022C*/ assembly hidebysig specialname static 
          string  get_PasteTransitionOnNonStateItem() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d3
    // Code size       0 (0x0)
  } // end of method SR::get_PasteTransitionOnNonStateItem

  .method /*0600022D*/ assembly hidebysig specialname static 
          string  get_PasteTransitionToolTip() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d4
    // Code size       0 (0x0)
  } // end of method SR::get_PasteTransitionToolTip

  .method /*0600022E*/ assembly hidebysig specialname static 
          string  get_PasteTransitionWithoutAvailableConnectionPoints() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d5
    // Code size       0 (0x0)
  } // end of method SR::get_PasteTransitionWithoutAvailableConnectionPoints

  .method /*0600022F*/ assembly hidebysig specialname static 
          string  get_PasteTransitionWithoutDestinationState() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d6
    // Code size       0 (0x0)
  } // end of method SR::get_PasteTransitionWithoutDestinationState

  .method /*06000230*/ assembly hidebysig specialname static 
          string  get_PropertyIsNotAConcreteActivityDelegate() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d7
    // Code size       0 (0x0)
  } // end of method SR::get_PropertyIsNotAConcreteActivityDelegate

  .method /*06000231*/ assembly hidebysig specialname static 
          string  get_PropertyIsNotAnActivityDelegate() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d8
    // Code size       0 (0x0)
  } // end of method SR::get_PropertyIsNotAnActivityDelegate

  .method /*06000232*/ assembly hidebysig specialname static 
          string  get_PropertyReferenceChange() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22d9
    // Code size       0 (0x0)
  } // end of method SR::get_PropertyReferenceChange

  .method /*06000233*/ assembly hidebysig specialname static 
          string  get_PropertyReferenceNotResolved() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22da
    // Code size       0 (0x0)
  } // end of method SR::get_PropertyReferenceNotResolved

  .method /*06000234*/ assembly hidebysig specialname static 
          string  get_ReorderItems() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22db
    // Code size       0 (0x0)
  } // end of method SR::get_ReorderItems

  .method /*06000235*/ assembly hidebysig specialname static 
          string  get_SetInitialState() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22dc
    // Code size       0 (0x0)
  } // end of method SR::get_SetInitialState

  .method /*06000236*/ assembly hidebysig specialname static 
          string  get_SharedTriggerToolTip() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22dd
    // Code size       0 (0x0)
  } // end of method SR::get_SharedTriggerToolTip

  .method /*06000237*/ assembly hidebysig specialname static 
          string  get_StateLinkButtonToolTip() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22de
    // Code size       0 (0x0)
  } // end of method SR::get_StateLinkButtonToolTip

  .method /*06000238*/ assembly hidebysig specialname static 
          string  get_StateMachineResize() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22df
    // Code size       0 (0x0)
  } // end of method SR::get_StateMachineResize

  .method /*06000239*/ assembly hidebysig specialname static 
          string  get_StateResize() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22e0
    // Code size       0 (0x0)
  } // end of method SR::get_StateResize

  .method /*0600023A*/ assembly hidebysig specialname static 
          string  get_TransitionConnectionPointTooltip() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22e1
    // Code size       0 (0x0)
  } // end of method SR::get_TransitionConnectionPointTooltip

  .method /*0600023B*/ assembly hidebysig specialname static 
          string  get_UnrootedRequestInReceiveReply() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22e2
    // Code size       0 (0x0)
  } // end of method SR::get_UnrootedRequestInReceiveReply

  .method /*0600023C*/ assembly hidebysig specialname static 
          string  get_UnrootedRequestInSendReply() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22e3
    // Code size       0 (0x0)
  } // end of method SR::get_UnrootedRequestInSendReply

  .method /*0600023D*/ assembly hidebysig specialname static 
          string  get_ViewMessageContent() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22e4
    // Code size       0 (0x0)
  } // end of method SR::get_ViewMessageContent

  .method /*0600023E*/ assembly hidebysig specialname static 
          string  get_ViewParameterContent() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22e5
    // Code size       0 (0x0)
  } // end of method SR::get_ViewParameterContent

  .method /*0600023F*/ assembly hidebysig specialname static 
          string  get_WrongNumberOfArgumentsForActivityDelegate() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x22e6
    // Code size       0 (0x0)
  } // end of method SR::get_WrongNumberOfArgumentsForActivityDelegate

  .property /*17000063*/ callconv(8) class [mscorlib/*23000001*/]System.Resources.ResourceManager/*0100006C*/
          ResourceManager()
  {
    .custom /*0C00004A:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get class [mscorlib/*23000001*/]System.Resources.ResourceManager/*0100006C*/ System.Activities.Core.Presentation.SR/*02000043*/::get_ResourceManager() /* 060001E6 */
  } // end of property SR::ResourceManager
  .property /*17000064*/ callconv(8) class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/
          Culture()
  {
    .custom /*0C00004B:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 02 00 00 00 00 00 ) 
    .get class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ System.Activities.Core.Presentation.SR/*02000043*/::get_Culture() /* 060001E7 */
    .set void System.Activities.Core.Presentation.SR/*02000043*/::set_Culture(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/) /* 060001E8 */
  } // end of property SR::Culture
  .property /*17000065*/ callconv(8) string
          AddActivityHintText()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_AddActivityHintText() /* 060001E9 */
  } // end of property SR::AddActivityHintText
  .property /*17000066*/ callconv(8) string
          AddNewCase()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_AddNewCase() /* 060001EA */
  } // end of property SR::AddNewCase
  .property /*17000067*/ callconv(8) string
          AutoConnect()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_AutoConnect() /* 060001EB */
  } // end of property SR::AutoConnect
  .property /*17000068*/ callconv(8) string
          AutoSplit()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_AutoSplit() /* 060001EC */
  } // end of property SR::AutoSplit
  .property /*17000069*/ callconv(8) string
          CannotConstructConnectionSegment()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CannotConstructConnectionSegment() /* 060001ED */
  } // end of property SR::CannotConstructConnectionSegment
  .property /*1700006A*/ callconv(8) string
          CannotCreateLink()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CannotCreateLink() /* 060001EE */
  } // end of property SR::CannotCreateLink
  .property /*1700006B*/ callconv(8) string
          CannotCreateTransitionFromAncestorToDescendant()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CannotCreateTransitionFromAncestorToDescendant() /* 060001EF */
  } // end of property SR::CannotCreateTransitionFromAncestorToDescendant
  .property /*1700006C*/ callconv(8) string
          CannotCreateTransitionToCompositeState()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CannotCreateTransitionToCompositeState() /* 060001F0 */
  } // end of property SR::CannotCreateTransitionToCompositeState
  .property /*1700006D*/ callconv(8) string
          CannotPasteSendReplyOrReceiveReply()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CannotPasteSendReplyOrReceiveReply() /* 060001F1 */
  } // end of property SR::CannotPasteSendReplyOrReceiveReply
  .property /*1700006E*/ callconv(8) string
          CannotSetCompositeStateAsInitialState()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CannotSetCompositeStateAsInitialState() /* 060001F2 */
  } // end of property SR::CannotSetCompositeStateAsInitialState
  .property /*1700006F*/ callconv(8) string
          CannotSetFinalStateAsInitialState()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CannotSetFinalStateAsInitialState() /* 060001F3 */
  } // end of property SR::CannotSetFinalStateAsInitialState
  .property /*17000070*/ callconv(8) string
          CaseFormat()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CaseFormat() /* 060001F4 */
  } // end of property SR::CaseFormat
  .property /*17000071*/ callconv(8) string
          ChangeCaseValue()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ChangeCaseValue() /* 060001F5 */
  } // end of property SR::ChangeCaseValue
  .property /*17000072*/ callconv(8) string
          ChangeTypeCollectionEditingScopeDesc()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ChangeTypeCollectionEditingScopeDesc() /* 060001F6 */
  } // end of property SR::ChangeTypeCollectionEditingScopeDesc
  .property /*17000073*/ callconv(8) string
          ClearInitialState()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ClearInitialState() /* 060001F7 */
  } // end of property SR::ClearInitialState
  .property /*17000074*/ callconv(8) string
          ConnectionAutomationPropertiesName()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ConnectionAutomationPropertiesName() /* 060001F8 */
  } // end of property SR::ConnectionAutomationPropertiesName
  .property /*17000075*/ callconv(8) string
          ConnectionNotFoundInConfig()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ConnectionNotFoundInConfig() /* 060001F9 */
  } // end of property SR::ConnectionNotFoundInConfig
  .property /*17000076*/ callconv(8) string
          CopyTransitionToolTip()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CopyTransitionToolTip() /* 060001FA */
  } // end of property SR::CopyTransitionToolTip
  .property /*17000077*/ callconv(8) string
          CorrelationInitializerNotSupported()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CorrelationInitializerNotSupported() /* 060001FB */
  } // end of property SR::CorrelationInitializerNotSupported
  .property /*17000078*/ callconv(8) string
          CreateParameter()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CreateParameter() /* 060001FC */
  } // end of property SR::CreateParameter
  .property /*17000079*/ callconv(8) string
          CreateTransition()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CreateTransition() /* 060001FD */
  } // end of property SR::CreateTransition
  .property /*1700007A*/ callconv(8) string
          CustomConnectionStringLabel()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_CustomConnectionStringLabel() /* 060001FE */
  } // end of property SR::CustomConnectionStringLabel
  .property /*1700007B*/ callconv(8) string
          DefaultCaseExists()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_DefaultCaseExists() /* 060001FF */
  } // end of property SR::DefaultCaseExists
  .property /*1700007C*/ callconv(8) string
          DefineContent()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_DefineContent() /* 06000200 */
  } // end of property SR::DefineContent
  .property /*1700007D*/ callconv(8) string
          DelegateArgumentMissing()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_DelegateArgumentMissing() /* 06000201 */
  } // end of property SR::DelegateArgumentMissing
  .property /*1700007E*/ callconv(8) string
          DelegateArgumentsDirectionalityMismatch()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_DelegateArgumentsDirectionalityMismatch() /* 06000202 */
  } // end of property SR::DelegateArgumentsDirectionalityMismatch
  .property /*1700007F*/ callconv(8) string
          DelegateInArgumentTypeMismatch()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_DelegateInArgumentTypeMismatch() /* 06000203 */
  } // end of property SR::DelegateInArgumentTypeMismatch
  .property /*17000080*/ callconv(8) string
          DelegateOutArgumentTypeMismatch()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_DelegateOutArgumentTypeMismatch() /* 06000204 */
  } // end of property SR::DelegateOutArgumentTypeMismatch
  .property /*17000081*/ callconv(8) string
          DuplicateCaseKey()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_DuplicateCaseKey() /* 06000205 */
  } // end of property SR::DuplicateCaseKey
  .property /*17000082*/ callconv(8) string
          DuplicateKeyName()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_DuplicateKeyName() /* 06000206 */
  } // end of property SR::DuplicateKeyName
  .property /*17000083*/ callconv(8) string
          EditingCommand()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_EditingCommand() /* 06000207 */
  } // end of property SR::EditingCommand
  .property /*17000084*/ callconv(8) string
          EditingConnectionSettings()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_EditingConnectionSettings() /* 06000208 */
  } // end of property SR::EditingConnectionSettings
  .property /*17000085*/ callconv(8) string
          EditingParameters()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_EditingParameters() /* 06000209 */
  } // end of property SR::EditingParameters
  .property /*17000086*/ callconv(8) string
          EditParametersUndoUnitName()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_EditParametersUndoUnitName() /* 0600020A */
  } // end of property SR::EditParametersUndoUnitName
  .property /*17000087*/ callconv(8) string
          EditStateToolTip()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_EditStateToolTip() /* 0600020B */
  } // end of property SR::EditStateToolTip
  .property /*17000088*/ callconv(8) string
          EditTransitionTooltip()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_EditTransitionTooltip() /* 0600020C */
  } // end of property SR::EditTransitionTooltip
  .property /*17000089*/ callconv(8) string
          EmptyName()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_EmptyName() /* 0600020D */
  } // end of property SR::EmptyName
  .property /*1700008A*/ callconv(8) string
          EqualityError()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_EqualityError() /* 0600020E */
  } // end of property SR::EqualityError
  .property /*1700008B*/ callconv(8) string
          ErrorMessageBoxTitle()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ErrorMessageBoxTitle() /* 0600020F */
  } // end of property SR::ErrorMessageBoxTitle
  .property /*1700008C*/ callconv(8) string
          ExpressionDefaultText()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ExpressionDefaultText() /* 06000210 */
  } // end of property SR::ExpressionDefaultText
  .property /*1700008D*/ callconv(8) string
          FCCreateLink()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCCreateLink() /* 06000211 */
  } // end of property SR::FCCreateLink
  .property /*1700008E*/ callconv(8) string
          FCDeleteLink()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCDeleteLink() /* 06000212 */
  } // end of property SR::FCDeleteLink
  .property /*1700008F*/ callconv(8) string
          FCDeleteShape()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCDeleteShape() /* 06000213 */
  } // end of property SR::FCDeleteShape
  .property /*17000090*/ callconv(8) string
          FCFalseBranchExists()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCFalseBranchExists() /* 06000214 */
  } // end of property SR::FCFalseBranchExists
  .property /*17000091*/ callconv(8) string
          FCFlowConditionLinkError()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCFlowConditionLinkError() /* 06000215 */
  } // end of property SR::FCFlowConditionLinkError
  .property /*17000092*/ callconv(8) string
          FCFlowConditionLinksExist()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCFlowConditionLinksExist() /* 06000216 */
  } // end of property SR::FCFlowConditionLinksExist
  .property /*17000093*/ callconv(8) string
          FCFlowDecisionFalseMarker()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCFlowDecisionFalseMarker() /* 06000217 */
  } // end of property SR::FCFlowDecisionFalseMarker
  .property /*17000094*/ callconv(8) string
          FCFlowDecisionTrueMarker()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCFlowDecisionTrueMarker() /* 06000218 */
  } // end of property SR::FCFlowDecisionTrueMarker
  .property /*17000095*/ callconv(8) string
          FCInvalidLink()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCInvalidLink() /* 06000219 */
  } // end of property SR::FCInvalidLink
  .property /*17000096*/ callconv(8) string
          FCLinkMove()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCLinkMove() /* 0600021A */
  } // end of property SR::FCLinkMove
  .property /*17000097*/ callconv(8) string
          FCNextLinkDefined()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCNextLinkDefined() /* 0600021B */
  } // end of property SR::FCNextLinkDefined
  .property /*17000098*/ callconv(8) string
          FCResizeUndoUnitName()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCResizeUndoUnitName() /* 0600021C */
  } // end of property SR::FCResizeUndoUnitName
  .property /*17000099*/ callconv(8) string
          FCTrueBranchExists()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FCTrueBranchExists() /* 0600021D */
  } // end of property SR::FCTrueBranchExists
  .property /*1700009A*/ callconv(8) string
          FillDelegateArguments()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FillDelegateArguments() /* 0600021E */
  } // end of property SR::FillDelegateArguments
  .property /*1700009B*/ callconv(8) string
          FlowSwitchCaseRenameEditingScopeDesc()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FlowSwitchCaseRenameEditingScopeDesc() /* 0600021F */
  } // end of property SR::FlowSwitchCaseRenameEditingScopeDesc
  .property /*1700009C*/ callconv(8) string
          FlowSwitchDefaultCaseDisplayNameEditingScopeDesc()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_FlowSwitchDefaultCaseDisplayNameEditingScopeDesc() /* 06000220 */
  } // end of property SR::FlowSwitchDefaultCaseDisplayNameEditingScopeDesc
  .property /*1700009D*/ callconv(8) string
          InitialStateConnectionPointTooltip()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_InitialStateConnectionPointTooltip() /* 06000221 */
  } // end of property SR::InitialStateConnectionPointTooltip
  .property /*1700009E*/ callconv(8) string
          InvalidFlowSwitchCaseMessage()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_InvalidFlowSwitchCaseMessage() /* 06000222 */
  } // end of property SR::InvalidFlowSwitchCaseMessage
  .property /*1700009F*/ callconv(8) string
          ItemMove()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ItemMove() /* 06000223 */
  } // end of property SR::ItemMove
  .property /*170000A0*/ callconv(8) string
          MoveLink()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_MoveLink() /* 06000224 */
  } // end of property SR::MoveLink
  .property /*170000A1*/ callconv(8) string
          NewCaseHint()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_NewCaseHint() /* 06000225 */
  } // end of property SR::NewCaseHint
  .property /*170000A2*/ callconv(8) string
          NotSupportedCaseKeyStringConversion()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_NotSupportedCaseKeyStringConversion() /* 06000226 */
  } // end of property SR::NotSupportedCaseKeyStringConversion
  .property /*170000A3*/ callconv(8) string
          NotSupportedEncoding()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_NotSupportedEncoding() /* 06000227 */
  } // end of property SR::NotSupportedEncoding
  .property /*170000A4*/ callconv(8) string
          NullName()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_NullName() /* 06000228 */
  } // end of property SR::NullName
  .property /*170000A5*/ callconv(8) string
          NullOrEmptyKeyName()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_NullOrEmptyKeyName() /* 06000229 */
  } // end of property SR::NullOrEmptyKeyName
  .property /*170000A6*/ callconv(8) string
          ParameterDialogTitle()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ParameterDialogTitle() /* 0600022A */
  } // end of property SR::ParameterDialogTitle
  .property /*170000A7*/ callconv(8) string
          PasteTransitionOnFinalState()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PasteTransitionOnFinalState() /* 0600022B */
  } // end of property SR::PasteTransitionOnFinalState
  .property /*170000A8*/ callconv(8) string
          PasteTransitionOnNonStateItem()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PasteTransitionOnNonStateItem() /* 0600022C */
  } // end of property SR::PasteTransitionOnNonStateItem
  .property /*170000A9*/ callconv(8) string
          PasteTransitionToolTip()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PasteTransitionToolTip() /* 0600022D */
  } // end of property SR::PasteTransitionToolTip
  .property /*170000AA*/ callconv(8) string
          PasteTransitionWithoutAvailableConnectionPoints()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PasteTransitionWithoutAvailableConnectionPoints() /* 0600022E */
  } // end of property SR::PasteTransitionWithoutAvailableConnectionPoints
  .property /*170000AB*/ callconv(8) string
          PasteTransitionWithoutDestinationState()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PasteTransitionWithoutDestinationState() /* 0600022F */
  } // end of property SR::PasteTransitionWithoutDestinationState
  .property /*170000AC*/ callconv(8) string
          PropertyIsNotAConcreteActivityDelegate()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PropertyIsNotAConcreteActivityDelegate() /* 06000230 */
  } // end of property SR::PropertyIsNotAConcreteActivityDelegate
  .property /*170000AD*/ callconv(8) string
          PropertyIsNotAnActivityDelegate()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PropertyIsNotAnActivityDelegate() /* 06000231 */
  } // end of property SR::PropertyIsNotAnActivityDelegate
  .property /*170000AE*/ callconv(8) string
          PropertyReferenceChange()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PropertyReferenceChange() /* 06000232 */
  } // end of property SR::PropertyReferenceChange
  .property /*170000AF*/ callconv(8) string
          PropertyReferenceNotResolved()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_PropertyReferenceNotResolved() /* 06000233 */
  } // end of property SR::PropertyReferenceNotResolved
  .property /*170000B0*/ callconv(8) string
          ReorderItems()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ReorderItems() /* 06000234 */
  } // end of property SR::ReorderItems
  .property /*170000B1*/ callconv(8) string
          SetInitialState()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_SetInitialState() /* 06000235 */
  } // end of property SR::SetInitialState
  .property /*170000B2*/ callconv(8) string
          SharedTriggerToolTip()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_SharedTriggerToolTip() /* 06000236 */
  } // end of property SR::SharedTriggerToolTip
  .property /*170000B3*/ callconv(8) string
          StateLinkButtonToolTip()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_StateLinkButtonToolTip() /* 06000237 */
  } // end of property SR::StateLinkButtonToolTip
  .property /*170000B4*/ callconv(8) string
          StateMachineResize()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_StateMachineResize() /* 06000238 */
  } // end of property SR::StateMachineResize
  .property /*170000B5*/ callconv(8) string
          StateResize()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_StateResize() /* 06000239 */
  } // end of property SR::StateResize
  .property /*170000B6*/ callconv(8) string
          TransitionConnectionPointTooltip()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_TransitionConnectionPointTooltip() /* 0600023A */
  } // end of property SR::TransitionConnectionPointTooltip
  .property /*170000B7*/ callconv(8) string
          UnrootedRequestInReceiveReply()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_UnrootedRequestInReceiveReply() /* 0600023B */
  } // end of property SR::UnrootedRequestInReceiveReply
  .property /*170000B8*/ callconv(8) string
          UnrootedRequestInSendReply()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_UnrootedRequestInSendReply() /* 0600023C */
  } // end of property SR::UnrootedRequestInSendReply
  .property /*170000B9*/ callconv(8) string
          ViewMessageContent()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ViewMessageContent() /* 0600023D */
  } // end of property SR::ViewMessageContent
  .property /*170000BA*/ callconv(8) string
          ViewParameterContent()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_ViewParameterContent() /* 0600023E */
  } // end of property SR::ViewParameterContent
  .property /*170000BB*/ callconv(8) string
          WrongNumberOfArgumentsForActivityDelegate()
  {
    .get string System.Activities.Core.Presentation.SR/*02000043*/::get_WrongNumberOfArgumentsForActivityDelegate() /* 0600023F */
  } // end of property SR::WrongNumberOfArgumentsForActivityDelegate
} // end of class System.Activities.Core.Presentation.SR

.class /*02000044*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.AssignDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*04000097*/ assembly class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.ExpressionTextBox/*01000036*/ assignToExpression
  .field /*04000098*/ assembly class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.ExpressionTextBox/*01000036*/ assignValueExpression
  .method /*06000240*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e7
    // Code size       0 (0x0)
  } // end of method AssignDesigner::.ctor

  .method /*06000241*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x22e8
    // Code size       0 (0x0)
  } // end of method AssignDesigner::OnInitialized

  .method /*06000242*/ assembly hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x22e9
    // Code size       0 (0x0)
  } // end of method AssignDesigner::RegisterMetadata

  .method /*06000243*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x22ea
    // Code size       0 (0x0)
  } // end of method AssignDesigner::OnModelItemChanged

  .method /*06000244*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00010A:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00010B:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x22eb
    // Code size       0 (0x0)
  } // end of method AssignDesigner::InitializeComponent

  .method /*06000245*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00010C:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00010D:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00010E:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x22ec
    // Code size       0 (0x0)
  } // end of method AssignDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.AssignDesigner

.class /*02000045*/ public auto ansi sealed System.Activities.Core.Presentation.ConnectionPointType
       extends [mscorlib/*23000001*/]System.Enum/*0100006D*/
{
  .field /*04000099*/ public specialname rtspecialname int32 value__
  .field /*0400009A*/ public static literal valuetype System.Activities.Core.Presentation.ConnectionPointType/*02000045*/ Default = int32(0x00000000)
  .field /*0400009B*/ public static literal valuetype System.Activities.Core.Presentation.ConnectionPointType/*02000045*/ Incoming = int32(0x00000001)
  .field /*0400009C*/ public static literal valuetype System.Activities.Core.Presentation.ConnectionPointType/*02000045*/ Outgoing = int32(0x00000002)
} // end of class System.Activities.Core.Presentation.ConnectionPointType

.class /*02000046*/ public auto ansi beforefieldinit System.Activities.Core.Presentation.LocationChangedEventArgs
       extends [mscorlib/*23000001*/]System.EventArgs/*0100000F*/
{
  .method /*06000246*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/ newLocation) cil managed
  // SIG: 20 01 01 11 81 59
  {
    // Method begins at RVA 0x22ed
    // Code size       0 (0x0)
  } // end of method LocationChangedEventArgs::.ctor

  .method /*06000247*/ public hidebysig specialname 
          instance valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/ 
          get_NewLocation() cil managed
  // SIG: 20 00 11 81 59
  {
    // Method begins at RVA 0x22ee
    // Code size       0 (0x0)
  } // end of method LocationChangedEventArgs::get_NewLocation

  .property /*170000BC*/ instance callconv(8) valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/
          NewLocation()
  {
    .get instance valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/ System.Activities.Core.Presentation.LocationChangedEventArgs/*02000046*/::get_NewLocation() /* 06000247 */
  } // end of property LocationChangedEventArgs::NewLocation
} // end of class System.Activities.Core.Presentation.LocationChangedEventArgs

.class /*02000047*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.ParallelDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*06000248*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22ef
    // Code size       0 (0x0)
  } // end of method ParallelDesigner::.ctor

  .method /*06000249*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x22f0
    // Code size       0 (0x0)
  } // end of method ParallelDesigner::RegisterMetadata

  .method /*0600024A*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00010F:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000110:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x22fc
    // Code size       0 (0x0)
  } // end of method ParallelDesigner::InitializeComponent

  .method /*0600024B*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000111:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000112:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000113:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x22fd
    // Code size       0 (0x0)
  } // end of method ParallelDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.ParallelDesigner

.class /*02000048*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.PickDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*0400009D*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ CreateBranchCommand
  .method /*0600024C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22fe
    // Code size       0 (0x0)
  } // end of method PickDesigner::.ctor

  .method /*0600024D*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2300
    // Code size       0 (0x0)
  } // end of method PickDesigner::RegisterMetadata

  .method /*0600024E*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000114:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000115:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x230c
    // Code size       0 (0x0)
  } // end of method PickDesigner::InitializeComponent

  .method /*0600024F*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000116:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000117:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000118:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x230d
    // Code size       0 (0x0)
  } // end of method PickDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*06000250*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000119:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00011A:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00011B:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x230e
    // Code size       0 (0x0)
  } // end of method PickDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*06000251*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x230f
    // Code size       0 (0x0)
  } // end of method PickDesigner::.cctor

} // end of class System.Activities.Core.Presentation.PickDesigner

.class /*02000049*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.PickBranchDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*06000252*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2310
    // Code size       0 (0x0)
  } // end of method PickBranchDesigner::.ctor

  .method /*06000253*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2311
    // Code size       0 (0x0)
  } // end of method PickBranchDesigner::RegisterMetadata

  .method /*06000254*/ family hidebysig virtual 
          instance string  GetAutomationIdMemberName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2312
    // Code size       0 (0x0)
  } // end of method PickBranchDesigner::GetAutomationIdMemberName

  .method /*06000255*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00011C:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00011D:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2313
    // Code size       0 (0x0)
  } // end of method PickBranchDesigner::InitializeComponent

  .method /*06000256*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00011E:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00011F:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000120:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2314
    // Code size       0 (0x0)
  } // end of method PickBranchDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.PickBranchDesigner

.class /*0200004A*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.PickValidationErrorSourceLocator
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Validation.IValidationErrorSourceLocator/*0100006E*/
{
  .method /*06000257*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> 
          FindSourceDetailFromActivity(class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ errorSource,
                                       object errorSourceDetail) cil managed
  // SIG: 20 02 15 12 81 51 01 1C 12 80 85 1C
  {
    // Method begins at RVA 0x2315
    // Code size       0 (0x0)
  } // end of method PickValidationErrorSourceLocator::FindSourceDetailFromActivity

  .method /*06000258*/ public hidebysig newslot virtual final 
          instance void  ReplaceParentChainWithSource(class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ parentActivity,
                                                      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> parentChain) cil managed
  // SIG: 20 02 01 12 80 85 15 12 81 51 01 1C
  {
    // Method begins at RVA 0x2316
    // Code size       0 (0x0)
  } // end of method PickValidationErrorSourceLocator::ReplaceParentChainWithSource

  .method /*06000259*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2317
    // Code size       0 (0x0)
  } // end of method PickValidationErrorSourceLocator::.ctor

} // end of class System.Activities.Core.Presentation.PickValidationErrorSourceLocator

.class /*0200004B*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.PickValidationErrorSourceLocatorFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ValidationErrorSourceLocatorFeature/*0100006F*/
{
  .method /*0600025A*/ family hidebysig specialname virtual 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Validation.IValidationErrorSourceLocator/*0100006E*/ 
          get_ValidationErrorSourceLocator() cil managed
  // SIG: 20 00 12 81 B9
  {
    // Method begins at RVA 0x2318
    // Code size       0 (0x0)
  } // end of method PickValidationErrorSourceLocatorFeature::get_ValidationErrorSourceLocator

  .method /*0600025B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2319
    // Code size       0 (0x0)
  } // end of method PickValidationErrorSourceLocatorFeature::.ctor

  .property /*170000BD*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Validation.IValidationErrorSourceLocator/*0100006E*/
          ValidationErrorSourceLocator()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Validation.IValidationErrorSourceLocator/*0100006E*/ System.Activities.Core.Presentation.PickValidationErrorSourceLocatorFeature/*0200004B*/::get_ValidationErrorSourceLocator() /* 0600025A */
  } // end of property PickValidationErrorSourceLocatorFeature::ValidationErrorSourceLocator
} // end of class System.Activities.Core.Presentation.PickValidationErrorSourceLocatorFeature

.class /*0200004C*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.TryCatchDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*0400009E*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ShowTryExpandedProperty
  .field /*0400009F*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ShowFinallyExpandedProperty
  .field /*040000A0*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ShowTypePresenterExpandedProperty
  .field /*040000A1*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ SelectedCatchProperty
  .field /*040000A2*/ assembly class System.Activities.Core.Presentation.TryCatchDesigner/*0200004C*/ controlRoot
  .method /*0600025C*/ public hidebysig specialname 
          instance bool  get_ShowTryExpanded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x231a
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::get_ShowTryExpanded

  .method /*0600025D*/ public hidebysig specialname 
          instance void  set_ShowTryExpanded(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x231b
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::set_ShowTryExpanded

  .method /*0600025E*/ public hidebysig specialname 
          instance bool  get_ShowFinallyExpanded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x231c
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::get_ShowFinallyExpanded

  .method /*0600025F*/ public hidebysig specialname 
          instance void  set_ShowFinallyExpanded(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x231d
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::set_ShowFinallyExpanded

  .method /*06000260*/ public hidebysig specialname 
          instance bool  get_ShowTypePresenterExpanded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x231e
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::get_ShowTypePresenterExpanded

  .method /*06000261*/ public hidebysig specialname 
          instance void  set_ShowTypePresenterExpanded(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x231f
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::set_ShowTypePresenterExpanded

  .method /*06000262*/ assembly hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2320
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::RegisterMetadata

  .method /*06000263*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x232c
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::.ctor

  .method /*06000264*/ assembly hidebysig static 
          bool  IsDescendantOfCatch(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ catchObject,
                                    class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ descendant) cil managed
  // SIG: 00 02 02 12 2D 12 2D
  {
    // Method begins at RVA 0x232d
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::IsDescendantOfCatch

  .method /*06000265*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000121:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000122:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x232e
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::InitializeComponent

  .method /*06000266*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000123:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000124:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000125:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x232f
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*06000267*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000126:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000127:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000128:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x2330
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*06000268*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2331
    // Code size       0 (0x0)
  } // end of method TryCatchDesigner::.cctor

  .property /*170000BE*/ instance callconv(8) bool
          ShowTryExpanded()
  {
    .get instance bool System.Activities.Core.Presentation.TryCatchDesigner/*0200004C*/::get_ShowTryExpanded() /* 0600025C */
    .set instance void System.Activities.Core.Presentation.TryCatchDesigner/*0200004C*/::set_ShowTryExpanded(bool) /* 0600025D */
  } // end of property TryCatchDesigner::ShowTryExpanded
  .property /*170000BF*/ instance callconv(8) bool
          ShowFinallyExpanded()
  {
    .get instance bool System.Activities.Core.Presentation.TryCatchDesigner/*0200004C*/::get_ShowFinallyExpanded() /* 0600025E */
    .set instance void System.Activities.Core.Presentation.TryCatchDesigner/*0200004C*/::set_ShowFinallyExpanded(bool) /* 0600025F */
  } // end of property TryCatchDesigner::ShowFinallyExpanded
  .property /*170000C0*/ instance callconv(8) bool
          ShowTypePresenterExpanded()
  {
    .get instance bool System.Activities.Core.Presentation.TryCatchDesigner/*0200004C*/::get_ShowTypePresenterExpanded() /* 06000260 */
    .set instance void System.Activities.Core.Presentation.TryCatchDesigner/*0200004C*/::set_ShowTypePresenterExpanded(bool) /* 06000261 */
  } // end of property TryCatchDesigner::ShowTypePresenterExpanded
} // end of class System.Activities.Core.Presentation.TryCatchDesigner

.class /*0200004D*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.CatchDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*040000A3*/ assembly class System.Activities.Core.Presentation.CatchDesigner/*0200004D*/ controlRoot
  .field /*040000A4*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.TextBlock/*01000018*/ exceptionTypeTextBlock
  .field /*040000A5*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.TextBox/*0100002A*/ exceptionArgumentNameBox
  .field /*040000A6*/ assembly class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowItemPresenter/*01000070*/ actionPresenter
  .method /*06000269*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2332
    // Code size       0 (0x0)
  } // end of method CatchDesigner::.ctor

  .method /*0600026A*/ assembly hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2333
    // Code size       0 (0x0)
  } // end of method CatchDesigner::RegisterMetadata

  .method /*0600026B*/ public hidebysig specialname 
          instance string  get_ExceptionTypeShortName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2334
    // Code size       0 (0x0)
  } // end of method CatchDesigner::get_ExceptionTypeShortName

  .method /*0600026C*/ public hidebysig specialname 
          instance string  get_ExceptionTypeFullName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2335
    // Code size       0 (0x0)
  } // end of method CatchDesigner::get_ExceptionTypeFullName

  .method /*0600026D*/ family hidebysig virtual 
          instance void  OnMouseDown(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x2336
    // Code size       0 (0x0)
  } // end of method CatchDesigner::OnMouseDown

  .method /*0600026E*/ family hidebysig virtual 
          instance void  OnMouseUp(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x2337
    // Code size       0 (0x0)
  } // end of method CatchDesigner::OnMouseUp

  .method /*0600026F*/ family hidebysig virtual 
          instance string  GetAutomationIdMemberName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2338
    // Code size       0 (0x0)
  } // end of method CatchDesigner::GetAutomationIdMemberName

  .method /*06000270*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000129:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00012A:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2339
    // Code size       0 (0x0)
  } // end of method CatchDesigner::InitializeComponent

  .method /*06000271*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00012B:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00012C:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00012D:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x233a
    // Code size       0 (0x0)
  } // end of method CatchDesigner::System.Windows.Markup.IComponentConnector.Connect

  .property /*170000C1*/ instance callconv(8) string
          ExceptionTypeShortName()
  {
    .get instance string System.Activities.Core.Presentation.CatchDesigner/*0200004D*/::get_ExceptionTypeShortName() /* 0600026B */
  } // end of property CatchDesigner::ExceptionTypeShortName
  .property /*170000C2*/ instance callconv(8) string
          ExceptionTypeFullName()
  {
    .get instance string System.Activities.Core.Presentation.CatchDesigner/*0200004D*/::get_ExceptionTypeFullName() /* 0600026C */
  } // end of property CatchDesigner::ExceptionTypeFullName
} // end of class System.Activities.Core.Presentation.CatchDesigner

.class /*0200004E*/ public auto ansi beforefieldinit System.Activities.Core.Presentation.DesignerMetadata
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.IRegisterMetadata/*01000071*/
{
  .method /*06000272*/ public hidebysig newslot virtual final 
          instance void  Register() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x233b
    // Code size       0 (0x0)
  } // end of method DesignerMetadata::Register

  .method /*06000273*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x233c
    // Code size       0 (0x0)
  } // end of method DesignerMetadata::.ctor

} // end of class System.Activities.Core.Presentation.DesignerMetadata

.class /*0200004F*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.RegisterMetadataDelayedWorker
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*06000274*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<string,class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/>>> 
          get_DelayedWorkItems() cil managed
  // SIG: 20 00 15 12 81 3D 02 0E 15 12 81 51 01 15 12 81 C9 01 12 31
  {
    // Method begins at RVA 0x233d
    // Code size       0 (0x0)
  } // end of method RegisterMetadataDelayedWorker::get_DelayedWorkItems

  .method /*06000275*/ public hidebysig instance void 
          RegisterMetadataDelayed(string assemblyName,
                                  class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/> delayedWork) cil managed
  // SIG: 20 02 01 0E 15 12 81 C9 01 12 31
  {
    // Method begins at RVA 0x233e
    // Code size       0 (0x0)
  } // end of method RegisterMetadataDelayedWorker::RegisterMetadataDelayed

  .method /*06000276*/ public hidebysig instance void 
          WorkNowIfApplicable() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x233f
    // Code size       0 (0x0)
  } // end of method RegisterMetadataDelayedWorker::WorkNowIfApplicable

  .method /*06000277*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2340
    // Code size       0 (0x0)
  } // end of method RegisterMetadataDelayedWorker::.ctor

  .property /*170000C3*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<string,class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/>>>
          DelayedWorkItems()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<string,class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/>>> System.Activities.Core.Presentation.RegisterMetadataDelayedWorker/*0200004F*/::get_DelayedWorkItems() /* 06000274 */
  } // end of property RegisterMetadataDelayedWorker::DelayedWorkItems
} // end of class System.Activities.Core.Presentation.RegisterMetadataDelayedWorker

.class /*02000050*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.SequenceDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*06000278*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2341
    // Code size       0 (0x0)
  } // end of method SequenceDesigner::.ctor

  .method /*06000279*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2342
    // Code size       0 (0x0)
  } // end of method SequenceDesigner::OnModelItemChanged

  .method /*0600027A*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2344
    // Code size       0 (0x0)
  } // end of method SequenceDesigner::RegisterMetadata

  .method /*0600027B*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00012E:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00012F:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2350
    // Code size       0 (0x0)
  } // end of method SequenceDesigner::InitializeComponent

  .method /*0600027C*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000130:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000131:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000132:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2351
    // Code size       0 (0x0)
  } // end of method SequenceDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.SequenceDesigner

.class /*02000051*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.VerticalConnector
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.UserControl/*01000047*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*040000A7*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ AllowedItemTypeProperty
  .field /*040000A8*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ContextProperty
  .field /*040000A9*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Media.Animation.BeginStoryboard/*0100005F*/ Collapse_BeginStoryboardOnDrop
  .field /*040000AA*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Shapes.Rectangle/*01000060*/ dropTarget
  .method /*0600027D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2352
    // Code size       0 (0x0)
  } // end of method VerticalConnector::.ctor

  .method /*0600027E*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_AllowedItemType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x2353
    // Code size       0 (0x0)
  } // end of method VerticalConnector::get_AllowedItemType

  .method /*0600027F*/ public hidebysig specialname 
          instance void  set_AllowedItemType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x2354
    // Code size       0 (0x0)
  } // end of method VerticalConnector::set_AllowedItemType

  .method /*06000280*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 
          get_Context() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x2355
    // Code size       0 (0x0)
  } // end of method VerticalConnector::get_Context

  .method /*06000281*/ public hidebysig specialname 
          instance void  set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2356
    // Code size       0 (0x0)
  } // end of method VerticalConnector::set_Context

  .method /*06000282*/ family hidebysig virtual 
          instance void  OnDragEnter(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e) cil managed
  // SIG: 20 01 01 12 81 75
  {
    // Method begins at RVA 0x2357
    // Code size       0 (0x0)
  } // end of method VerticalConnector::OnDragEnter

  .method /*06000283*/ family hidebysig virtual 
          instance void  OnDragLeave(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e) cil managed
  // SIG: 20 01 01 12 81 75
  {
    // Method begins at RVA 0x2358
    // Code size       0 (0x0)
  } // end of method VerticalConnector::OnDragLeave

  .method /*06000284*/ family hidebysig virtual 
          instance void  OnDrop(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e) cil managed
  // SIG: 20 01 01 12 81 75
  {
    // Method begins at RVA 0x2359
    // Code size       0 (0x0)
  } // end of method VerticalConnector::OnDrop

  .method /*06000285*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000133:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000134:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x235a
    // Code size       0 (0x0)
  } // end of method VerticalConnector::InitializeComponent

  .method /*06000286*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000135:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000136:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000137:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x235b
    // Code size       0 (0x0)
  } // end of method VerticalConnector::System.Windows.Markup.IComponentConnector.Connect

  .method /*06000287*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x235c
    // Code size       0 (0x0)
  } // end of method VerticalConnector::.cctor

  .property /*170000C4*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          AllowedItemType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Core.Presentation.VerticalConnector/*02000051*/::get_AllowedItemType() /* 0600027E */
    .set instance void System.Activities.Core.Presentation.VerticalConnector/*02000051*/::set_AllowedItemType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0600027F */
  } // end of property VerticalConnector::AllowedItemType
  .property /*170000C5*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/
          Context()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ System.Activities.Core.Presentation.VerticalConnector/*02000051*/::get_Context() /* 06000280 */
    .set instance void System.Activities.Core.Presentation.VerticalConnector/*02000051*/::set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/) /* 06000281 */
  } // end of property VerticalConnector::Context
} // end of class System.Activities.Core.Presentation.VerticalConnector

.class /*02000052*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.CaseDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*040000AB*/ assembly class System.Activities.Core.Presentation.CaseDesigner/*02000052*/ controlRoot
  .field /*040000AC*/ assembly class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/ caseKey
  .method /*06000288*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x235d
    // Code size       0 (0x0)
  } // end of method CaseDesigner::.ctor

  .method /*06000289*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/> 
          get_FocusSelf() cil managed
  // SIG: 20 00 15 12 81 C9 01 12 81 64
  {
    // Method begins at RVA 0x235e
    // Code size       0 (0x0)
  } // end of method CaseDesigner::get_FocusSelf

  .method /*0600028A*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x235f
    // Code size       0 (0x0)
  } // end of method CaseDesigner::RegisterMetadata

  .method /*0600028B*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2360
    // Code size       0 (0x0)
  } // end of method CaseDesigner::OnModelItemChanged

  .method /*0600028C*/ family hidebysig virtual 
          instance void  OnMouseDown(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x2361
    // Code size       0 (0x0)
  } // end of method CaseDesigner::OnMouseDown

  .method /*0600028D*/ family hidebysig virtual 
          instance void  OnMouseUp(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x2362
    // Code size       0 (0x0)
  } // end of method CaseDesigner::OnMouseUp

  .method /*0600028E*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000138:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000139:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2363
    // Code size       0 (0x0)
  } // end of method CaseDesigner::InitializeComponent

  .method /*0600028F*/ assembly hidebysig 
          instance class [mscorlib/*23000001*/]System.Delegate/*01000006*/ 
          _CreateDelegate(class [mscorlib/*23000001*/]System.Type/*01000003*/ delegateType,
                          string 'handler') cil managed
  // SIG: 20 02 12 19 12 0D 0E
  {
    .custom /*0C00013A:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00013B:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2364
    // Code size       0 (0x0)
  } // end of method CaseDesigner::_CreateDelegate

  .method /*06000290*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00013C:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00013D:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00013E:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2365
    // Code size       0 (0x0)
  } // end of method CaseDesigner::System.Windows.Markup.IComponentConnector.Connect

  .property /*170000C6*/ instance callconv(8) class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/>
          FocusSelf()
  {
    .get instance class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/> System.Activities.Core.Presentation.CaseDesigner/*02000052*/::get_FocusSelf() /* 06000289 */
  } // end of property CaseDesigner::FocusSelf
} // end of class System.Activities.Core.Presentation.CaseDesigner

.class /*02000053*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.SwitchDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*040000AD*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ CaseTypeProperty
  .field /*040000AE*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ SelectedCaseProperty
  .field /*040000AF*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ShowDefaultCaseExpandedProperty
  .field /*040000B0*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ NewKeyProperty
  .field /*040000B1*/ assembly class System.Activities.Core.Presentation.SwitchDesigner/*02000053*/ controlRoot
  .method /*06000291*/ public hidebysig specialname 
          instance bool  get_ShowDefaultCaseExpanded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2366
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::get_ShowDefaultCaseExpanded

  .method /*06000292*/ public hidebysig specialname 
          instance void  set_ShowDefaultCaseExpanded(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2367
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::set_ShowDefaultCaseExpanded

  .method /*06000293*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ 
          get_CheckDuplicateCaseKey() cil managed
  // SIG: 20 00 12 81 6C
  {
    // Method begins at RVA 0x2368
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::get_CheckDuplicateCaseKey

  .method /*06000294*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2369
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::.ctor

  .method /*06000295*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x236a
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::OnModelItemChanged

  .method /*06000296*/ assembly hidebysig static 
          bool  IsDescendantOfCase(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ caseObject,
                                   class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ descendant) cil managed
  // SIG: 00 02 02 12 2D 12 2D
  {
    // Method begins at RVA 0x236b
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::IsDescendantOfCase

  .method /*06000297*/ assembly hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x236c
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::RegisterMetadata

  .method /*06000298*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00013F:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000140:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2378
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::InitializeComponent

  .method /*06000299*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000141:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000142:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000143:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2379
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600029A*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000144:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000145:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000146:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x237a
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*0600029B*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x237b
    // Code size       0 (0x0)
  } // end of method SwitchDesigner::.cctor

  .property /*170000C7*/ instance callconv(8) bool
          ShowDefaultCaseExpanded()
  {
    .get instance bool System.Activities.Core.Presentation.SwitchDesigner/*02000053*/::get_ShowDefaultCaseExpanded() /* 06000291 */
    .set instance void System.Activities.Core.Presentation.SwitchDesigner/*02000053*/::set_ShowDefaultCaseExpanded(bool) /* 06000292 */
  } // end of property SwitchDesigner::ShowDefaultCaseExpanded
  .property /*170000C8*/ instance callconv(8) class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/
          CheckDuplicateCaseKey()
  {
    .get instance class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ System.Activities.Core.Presentation.SwitchDesigner/*02000053*/::get_CheckDuplicateCaseKey() /* 06000293 */
  } // end of property SwitchDesigner::CheckDuplicateCaseKey
} // end of class System.Activities.Core.Presentation.SwitchDesigner

.class /*02000054*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.CancellationScopeDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*0600029C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x237c
    // Code size       0 (0x0)
  } // end of method CancellationScopeDesigner::.ctor

  .method /*0600029D*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x237d
    // Code size       0 (0x0)
  } // end of method CancellationScopeDesigner::RegisterMetadata

  .method /*0600029E*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000147:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000148:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x237e
    // Code size       0 (0x0)
  } // end of method CancellationScopeDesigner::InitializeComponent

  .method /*0600029F*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000149:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00014A:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00014B:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x237f
    // Code size       0 (0x0)
  } // end of method CancellationScopeDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.CancellationScopeDesigner

.class /*02000055*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.CompensableActivityDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*060002A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2380
    // Code size       0 (0x0)
  } // end of method CompensableActivityDesigner::.ctor

  .method /*060002A1*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2381
    // Code size       0 (0x0)
  } // end of method CompensableActivityDesigner::RegisterMetadata

  .method /*060002A2*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00014C:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00014D:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2382
    // Code size       0 (0x0)
  } // end of method CompensableActivityDesigner::InitializeComponent

  .method /*060002A3*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00014E:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00014F:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000150:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2383
    // Code size       0 (0x0)
  } // end of method CompensableActivityDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.CompensableActivityDesigner

.class /*02000056*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StartSymbol
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*040000B2*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TextProperty
  .field /*040000B3*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.ContentControl/*01000013*/ startIcon
  .method /*060002A4*/ public hidebysig specialname 
          instance string  get_Text() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2384
    // Code size       0 (0x0)
  } // end of method StartSymbol::get_Text

  .method /*060002A5*/ public hidebysig specialname 
          instance void  set_Text(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2385
    // Code size       0 (0x0)
  } // end of method StartSymbol::set_Text

  .method /*060002A6*/ public hidebysig static 
          class System.Activities.Core.Presentation.StartSymbol/*02000056*/ 
          CreateStartSymbol(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context) cil managed
  // SIG: 00 01 12 81 58 12 79
  {
    // Method begins at RVA 0x2386
    // Code size       0 (0x0)
  } // end of method StartSymbol::CreateStartSymbol

  .method /*060002A7*/ private hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2387
    // Code size       0 (0x0)
  } // end of method StartSymbol::.ctor

  .method /*060002A8*/ family hidebysig virtual 
          instance void  OnContextMenuOpening(class [PresentationFramework/*23000006*/]System.Windows.Controls.ContextMenuEventArgs/*01000073*/ e) cil managed
  // SIG: 20 01 01 12 81 CD
  {
    // Method begins at RVA 0x2388
    // Code size       0 (0x0)
  } // end of method StartSymbol::OnContextMenuOpening

  .method /*060002A9*/ family hidebysig virtual 
          instance void  OnPreviewKeyDown(class [PresentationCore/*23000009*/]System.Windows.Input.KeyEventArgs/*01000030*/ e) cil managed
  // SIG: 20 01 01 12 80 C1
  {
    // Method begins at RVA 0x2389
    // Code size       0 (0x0)
  } // end of method StartSymbol::OnPreviewKeyDown

  .method /*060002AA*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000151:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000152:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x238a
    // Code size       0 (0x0)
  } // end of method StartSymbol::InitializeComponent

  .method /*060002AB*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000153:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000154:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000155:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x238b
    // Code size       0 (0x0)
  } // end of method StartSymbol::System.Windows.Markup.IComponentConnector.Connect

  .method /*060002AC*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x238c
    // Code size       0 (0x0)
  } // end of method StartSymbol::.cctor

  .property /*170000C9*/ instance callconv(8) string
          Text()
  {
    .get instance string System.Activities.Core.Presentation.StartSymbol/*02000056*/::get_Text() /* 060002A4 */
    .set instance void System.Activities.Core.Presentation.StartSymbol/*02000056*/::set_Text(string) /* 060002A5 */
  } // end of property StartSymbol::Text
} // end of class System.Activities.Core.Presentation.StartSymbol

.class /*02000057*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.InvokeMethodDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*060002AD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x238d
    // Code size       0 (0x0)
  } // end of method InvokeMethodDesigner::.ctor

  .method /*060002AE*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2390
    // Code size       0 (0x0)
  } // end of method InvokeMethodDesigner::RegisterMetadata

  .method /*060002AF*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000156:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000157:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x239c
    // Code size       0 (0x0)
  } // end of method InvokeMethodDesigner::InitializeComponent

  .method /*060002B0*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000158:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000159:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00015A:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x239d
    // Code size       0 (0x0)
  } // end of method InvokeMethodDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.InvokeMethodDesigner

.class /*02000058*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.TransactionScopeDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*060002B1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x239e
    // Code size       0 (0x0)
  } // end of method TransactionScopeDesigner::.ctor

  .method /*060002B2*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x239f
    // Code size       0 (0x0)
  } // end of method TransactionScopeDesigner::RegisterMetadata

  .method /*060002B3*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00015B:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00015C:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x23a0
    // Code size       0 (0x0)
  } // end of method TransactionScopeDesigner::InitializeComponent

  .method /*060002B4*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00015D:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00015E:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00015F:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x23a1
    // Code size       0 (0x0)
  } // end of method TransactionScopeDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.TransactionScopeDesigner

.class /*02000059*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.CaseKeyBox
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.UserControl/*01000047*/
       implements System.Activities.Core.Presentation.ICaseKeyBoxView/*0200005C*/,
                  [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*040000B4*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ DisplayHintTextProperty
  .field /*040000B5*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ LabelTextProperty
  .field /*040000B6*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ValueProperty
  .field /*040000B7*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ValueTypeProperty
  .field /*040000B8*/ public static class [PresentationCore/*23000009*/]System.Windows.RoutedEvent/*01000016*/ ValueCommittedEvent
  .field /*040000B9*/ public static class [PresentationCore/*23000009*/]System.Windows.RoutedEvent/*01000016*/ EditCancelledEvent
  .field /*040000BA*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ CaseKeyValidationCallbackProperty
  .field /*040000BB*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ErrorCallbackProperty
  .field /*040000BC*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ CommitExplicitlyProperty
  .field /*040000BD*/ assembly class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/ controlRoot
  .field /*040000BE*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.ContentPresenter/*01000052*/ contentPresenter
  .method /*060002B5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23a2
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::.ctor

  .method /*060002B6*/ public hidebysig specialname 
          instance void  add_ValueCommitted(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x23a3
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::add_ValueCommitted

  .method /*060002B7*/ public hidebysig specialname 
          instance void  remove_ValueCommitted(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x23a4
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::remove_ValueCommitted

  .method /*060002B8*/ public hidebysig newslot virtual 
          instance void  OnValueCommitted() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23a5
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::OnValueCommitted

  .method /*060002B9*/ public hidebysig specialname 
          instance void  add_EditCancelled(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x23a6
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::add_EditCancelled

  .method /*060002BA*/ public hidebysig specialname 
          instance void  remove_EditCancelled(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 12 55
  {
    // Method begins at RVA 0x23a7
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::remove_EditCancelled

  .method /*060002BB*/ public hidebysig newslot virtual 
          instance void  OnEditCancelled() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23a8
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::OnEditCancelled

  .method /*060002BC*/ public hidebysig newslot specialname virtual final 
          instance class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ 
          get_CaseKeyValidationCallback() cil managed
  // SIG: 20 00 12 81 6C
  {
    // Method begins at RVA 0x23a9
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::get_CaseKeyValidationCallback

  .method /*060002BD*/ public hidebysig specialname 
          instance void  set_CaseKeyValidationCallback(class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ 'value') cil managed
  // SIG: 20 01 01 12 81 6C
  {
    // Method begins at RVA 0x23aa
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::set_CaseKeyValidationCallback

  .method /*060002BE*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/> 
          get_ErrorCallback() cil managed
  // SIG: 20 00 15 12 81 C9 01 12 81 64
  {
    // Method begins at RVA 0x23ab
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::get_ErrorCallback

  .method /*060002BF*/ public hidebysig specialname 
          instance void  set_ErrorCallback(class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 C9 01 12 81 64
  {
    // Method begins at RVA 0x23ac
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::set_ErrorCallback

  .method /*060002C0*/ public hidebysig specialname 
          instance bool  get_CommitExplicitly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23ad
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::get_CommitExplicitly

  .method /*060002C1*/ public hidebysig specialname 
          instance void  set_CommitExplicitly(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23ae
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::set_CommitExplicitly

  .method /*060002C2*/ public hidebysig specialname 
          instance string  get_LabelText() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23af
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::get_LabelText

  .method /*060002C3*/ public hidebysig specialname 
          instance void  set_LabelText(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23b0
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::set_LabelText

  .method /*060002C4*/ public hidebysig newslot specialname virtual final 
          instance bool  get_DisplayHintText() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23b1
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::get_DisplayHintText

  .method /*060002C5*/ public hidebysig specialname 
          instance void  set_DisplayHintText(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23b2
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::set_DisplayHintText

  .method /*060002C6*/ public hidebysig newslot specialname virtual final 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x23b3
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::get_Value

  .method /*060002C7*/ public hidebysig newslot specialname virtual final 
          instance void  set_Value(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x23b4
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::set_Value

  .method /*060002C8*/ public hidebysig newslot specialname virtual final 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ValueType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x23b5
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::get_ValueType

  .method /*060002C9*/ public hidebysig specialname 
          instance void  set_ValueType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x23b6
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::set_ValueType

  .method /*060002CA*/ public hidebysig newslot virtual final 
          instance void  RegainFocus() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23b7
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::RegainFocus

  .method /*060002CB*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/ 
          get_ViewModel() cil managed
  // SIG: 20 00 12 81 68
  {
    .custom /*0C000160:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23b8
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::get_ViewModel

  .method /*060002CC*/ public hidebysig specialname 
          instance void  set_ViewModel(class System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/ 'value') cil managed
  // SIG: 20 01 01 12 81 68
  {
    .custom /*0C000161:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23b9
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::set_ViewModel

  .method /*060002CD*/ public hidebysig instance bool 
          CommitChanges() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23ba
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::CommitChanges

  .method /*060002CE*/ public hidebysig instance void 
          CancelChanges() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23bb
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::CancelChanges

  .method /*060002CF*/ public hidebysig instance void 
          ResetText() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23bc
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::ResetText

  .method /*060002D0*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000162:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000163:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x23bd
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::InitializeComponent

  .method /*060002D1*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000164:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000165:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000166:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x23be
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::System.Windows.Markup.IComponentConnector.Connect

  .method /*060002D2*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000167:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000168:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000169:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x23bf
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::System.Windows.Markup.IStyleConnector.Connect

  .method /*060002D3*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23c0
    // Code size       0 (0x0)
  } // end of method CaseKeyBox::.cctor

  .event /*14000007*/ [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ ValueCommitted
  {
    .addon instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::add_ValueCommitted(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 060002B6 */
    .removeon instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::remove_ValueCommitted(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 060002B7 */
  } // end of event CaseKeyBox::ValueCommitted
  .event /*14000008*/ [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/ EditCancelled
  {
    .addon instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::add_EditCancelled(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 060002B9 */
    .removeon instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::remove_EditCancelled(class [PresentationCore/*23000009*/]System.Windows.RoutedEventHandler/*01000015*/) /* 060002BA */
  } // end of event CaseKeyBox::EditCancelled
  .property /*170000CA*/ instance callconv(8) class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/
          CaseKeyValidationCallback()
  {
    .get instance class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::get_CaseKeyValidationCallback() /* 060002BC */
    .set instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::set_CaseKeyValidationCallback(class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/) /* 060002BD */
  } // end of property CaseKeyBox::CaseKeyValidationCallback
  .property /*170000CB*/ instance callconv(8) class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/>
          ErrorCallback()
  {
    .get instance class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/> System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::get_ErrorCallback() /* 060002BE */
    .set instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::set_ErrorCallback(class [mscorlib/*23000001*/]System.Action`1/*01000072*/<class System.Activities.Core.Presentation.CaseKeyBox/*02000059*/>) /* 060002BF */
  } // end of property CaseKeyBox::ErrorCallback
  .property /*170000CC*/ instance callconv(8) bool
          CommitExplicitly()
  {
    .get instance bool System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::get_CommitExplicitly() /* 060002C0 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::set_CommitExplicitly(bool) /* 060002C1 */
  } // end of property CaseKeyBox::CommitExplicitly
  .property /*170000CD*/ instance callconv(8) string
          LabelText()
  {
    .get instance string System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::get_LabelText() /* 060002C2 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::set_LabelText(string) /* 060002C3 */
  } // end of property CaseKeyBox::LabelText
  .property /*170000CE*/ instance callconv(8) bool
          DisplayHintText()
  {
    .get instance bool System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::get_DisplayHintText() /* 060002C4 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::set_DisplayHintText(bool) /* 060002C5 */
  } // end of property CaseKeyBox::DisplayHintText
  .property /*170000CF*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::get_Value() /* 060002C6 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::set_Value(object) /* 060002C7 */
  } // end of property CaseKeyBox::Value
  .property /*170000D0*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ValueType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::get_ValueType() /* 060002C8 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::set_ValueType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 060002C9 */
  } // end of property CaseKeyBox::ValueType
  .property /*170000D1*/ instance callconv(8) class System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/
          ViewModel()
  {
    .get instance class System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/ System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::get_ViewModel() /* 060002CB */
    .set instance void System.Activities.Core.Presentation.CaseKeyBox/*02000059*/::set_ViewModel(class System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/) /* 060002CC */
  } // end of property CaseKeyBox::ViewModel
} // end of class System.Activities.Core.Presentation.CaseKeyBox

.class /*0200005A*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.CaseKeyBoxViewModel
       extends [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/
{
  .field /*040000BF*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ComboBoxIsEditableProperty
  .field /*040000C0*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ComboBoxVisibilityProperty
  .field /*040000C1*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ComboBoxItemsProperty
  .field /*040000C2*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ DataTemplateNameProperty
  .field /*040000C3*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TextProperty
  .field /*040000C4*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TextBoxVisibilityProperty
  .field /*040000C5*/ public static literal string BoxesTemplate = "Boxes"
  .field /*040000C6*/ public static literal string LabelTemplate = "Label"
  .method /*060002D4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Activities.Core.Presentation.ICaseKeyBoxView/*0200005C*/ view) cil managed
  // SIG: 20 01 01 12 81 70
  {
    // Method begins at RVA 0x23c1
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::.ctor

  .method /*060002D5*/ public hidebysig specialname 
          instance bool  get_ComboBoxIsEditable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23c2
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::get_ComboBoxIsEditable

  .method /*060002D6*/ public hidebysig specialname 
          instance void  set_ComboBoxIsEditable(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23c3
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::set_ComboBoxIsEditable

  .method /*060002D7*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<string> 
          get_ComboBoxItems() cil managed
  // SIG: 20 00 15 12 80 DD 01 0E
  {
    // Method begins at RVA 0x23c4
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::get_ComboBoxItems

  .method /*060002D8*/ public hidebysig specialname 
          instance void  set_ComboBoxItems(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 80 DD 01 0E
  {
    // Method begins at RVA 0x23c5
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::set_ComboBoxItems

  .method /*060002D9*/ public hidebysig specialname 
          instance valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ 
          get_ComboBoxVisibility() cil managed
  // SIG: 20 00 11 81 85
  {
    // Method begins at RVA 0x23c6
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::get_ComboBoxVisibility

  .method /*060002DA*/ public hidebysig specialname 
          instance void  set_ComboBoxVisibility(valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ 'value') cil managed
  // SIG: 20 01 01 11 81 85
  {
    // Method begins at RVA 0x23c7
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::set_ComboBoxVisibility

  .method /*060002DB*/ public hidebysig specialname 
          instance string  get_DataTemplateName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23c8
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::get_DataTemplateName

  .method /*060002DC*/ public hidebysig specialname 
          instance void  set_DataTemplateName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23c9
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::set_DataTemplateName

  .method /*060002DD*/ public hidebysig specialname 
          instance string  get_Text() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23ca
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::get_Text

  .method /*060002DE*/ public hidebysig specialname 
          instance void  set_Text(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23cb
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::set_Text

  .method /*060002DF*/ public hidebysig specialname 
          instance valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ 
          get_TextBoxVisibility() cil managed
  // SIG: 20 00 11 81 85
  {
    // Method begins at RVA 0x23cc
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::get_TextBoxVisibility

  .method /*060002E0*/ public hidebysig specialname 
          instance void  set_TextBoxVisibility(valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ 'value') cil managed
  // SIG: 20 01 01 11 81 85
  {
    // Method begins at RVA 0x23cd
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::set_TextBoxVisibility

  .method /*060002E1*/ public hidebysig specialname 
          instance bool  get_IsBoxOnly() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00016A:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23ce
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::get_IsBoxOnly

  .method /*060002E2*/ public hidebysig specialname 
          instance void  set_IsBoxOnly(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00016B:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23cf
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::set_IsBoxOnly

  .method /*060002E3*/ public hidebysig instance bool 
          OnEnterPressed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23d0
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::OnEnterPressed

  .method /*060002E4*/ public hidebysig instance void 
          OnEscapePressed() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d1
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::OnEscapePressed

  .method /*060002E5*/ public hidebysig instance void 
          OnLabelGotFocus() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d2
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::OnLabelGotFocus

  .method /*060002E6*/ public hidebysig instance bool 
          OnLostFocus() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23d3
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::OnLostFocus

  .method /*060002E7*/ public hidebysig instance void 
          OnValueChanged() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d4
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::OnValueChanged

  .method /*060002E8*/ public hidebysig instance void 
          OnValueTypeChanged() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d5
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::OnValueTypeChanged

  .method /*060002E9*/ public hidebysig instance void 
          SaveOldText() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d6
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::SaveOldText

  .method /*060002EA*/ public hidebysig instance bool 
          CanResolveInputText([out] string& reason) cil managed
  // SIG: 20 01 02 10 0E
  {
    // Method begins at RVA 0x23d7
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::CanResolveInputText

  .method /*060002EB*/ public hidebysig instance bool 
          TextHasBeenChanged() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23d8
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::TextHasBeenChanged

  .method /*060002EC*/ public hidebysig instance void 
          ResetText() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d9
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::ResetText

  .method /*060002ED*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23da
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxViewModel::.cctor

  .property /*170000D2*/ instance callconv(8) bool
          ComboBoxIsEditable()
  {
    .get instance bool System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::get_ComboBoxIsEditable() /* 060002D5 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::set_ComboBoxIsEditable(bool) /* 060002D6 */
  } // end of property CaseKeyBoxViewModel::ComboBoxIsEditable
  .property /*170000D3*/ instance callconv(8) class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<string>
          ComboBoxItems()
  {
    .get instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<string> System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::get_ComboBoxItems() /* 060002D7 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::set_ComboBoxItems(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<string>) /* 060002D8 */
  } // end of property CaseKeyBoxViewModel::ComboBoxItems
  .property /*170000D4*/ instance callconv(8) valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/
          ComboBoxVisibility()
  {
    .get instance valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::get_ComboBoxVisibility() /* 060002D9 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::set_ComboBoxVisibility(valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/) /* 060002DA */
  } // end of property CaseKeyBoxViewModel::ComboBoxVisibility
  .property /*170000D5*/ instance callconv(8) string
          DataTemplateName()
  {
    .get instance string System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::get_DataTemplateName() /* 060002DB */
    .set instance void System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::set_DataTemplateName(string) /* 060002DC */
  } // end of property CaseKeyBoxViewModel::DataTemplateName
  .property /*170000D6*/ instance callconv(8) string
          Text()
  {
    .get instance string System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::get_Text() /* 060002DD */
    .set instance void System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::set_Text(string) /* 060002DE */
  } // end of property CaseKeyBoxViewModel::Text
  .property /*170000D7*/ instance callconv(8) valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/
          TextBoxVisibility()
  {
    .get instance valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/ System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::get_TextBoxVisibility() /* 060002DF */
    .set instance void System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::set_TextBoxVisibility(valuetype [PresentationCore/*23000009*/]System.Windows.Visibility/*01000061*/) /* 060002E0 */
  } // end of property CaseKeyBoxViewModel::TextBoxVisibility
  .property /*170000D8*/ instance callconv(8) bool
          IsBoxOnly()
  {
    .get instance bool System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::get_IsBoxOnly() /* 060002E1 */
    .set instance void System.Activities.Core.Presentation.CaseKeyBoxViewModel/*0200005A*/::set_IsBoxOnly(bool) /* 060002E2 */
  } // end of property CaseKeyBoxViewModel::IsBoxOnly
} // end of class System.Activities.Core.Presentation.CaseKeyBoxViewModel

.class /*0200005B*/ private auto ansi sealed System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*01000074*/
{
  .method /*060002EE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method CaseKeyValidationCallbackDelegate::.ctor

  .method /*060002EF*/ public hidebysig newslot virtual 
          instance bool  Invoke(object obj,
                                [out] string& reason) runtime managed
  // SIG: 20 02 02 1C 10 0E
  {
  } // end of method CaseKeyValidationCallbackDelegate::Invoke

  .method /*060002F0*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*01000075*/ 
          BeginInvoke(object obj,
                      [out] string& reason,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*01000076*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 81 D5 1C 10 0E 12 81 D9 1C
  {
  } // end of method CaseKeyValidationCallbackDelegate::BeginInvoke

  .method /*060002F1*/ public hidebysig newslot virtual 
          instance bool  EndInvoke([out] string& reason,
                                   class [mscorlib/*23000001*/]System.IAsyncResult/*01000075*/ result) runtime managed
  // SIG: 20 02 02 10 0E 12 81 D5
  {
  } // end of method CaseKeyValidationCallbackDelegate::EndInvoke

} // end of class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate

.class /*0200005C*/ interface private abstract auto ansi System.Activities.Core.Presentation.ICaseKeyBoxView
{
  .method /*060002F2*/ public hidebysig newslot abstract virtual 
          instance void  RegainFocus() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ICaseKeyBoxView::RegainFocus

  .method /*060002F3*/ public hidebysig newslot abstract virtual 
          instance void  OnValueCommitted() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ICaseKeyBoxView::OnValueCommitted

  .method /*060002F4*/ public hidebysig newslot abstract virtual 
          instance void  OnEditCancelled() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ICaseKeyBoxView::OnEditCancelled

  .method /*060002F5*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_DisplayHintText() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method ICaseKeyBoxView::get_DisplayHintText

  .method /*060002F6*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method ICaseKeyBoxView::get_Value

  .method /*060002F7*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Value(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x0
  } // end of method ICaseKeyBoxView::set_Value

  .method /*060002F8*/ public hidebysig newslot specialname abstract virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ValueType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x0
  } // end of method ICaseKeyBoxView::get_ValueType

  .method /*060002F9*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ 
          get_CaseKeyValidationCallback() cil managed
  // SIG: 20 00 12 81 6C
  {
    // Method begins at RVA 0x0
  } // end of method ICaseKeyBoxView::get_CaseKeyValidationCallback

  .property /*170000D9*/ instance callconv(8) bool
          DisplayHintText()
  {
    .get instance bool System.Activities.Core.Presentation.ICaseKeyBoxView/*0200005C*/::get_DisplayHintText() /* 060002F5 */
  } // end of property ICaseKeyBoxView::DisplayHintText
  .property /*170000DA*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Activities.Core.Presentation.ICaseKeyBoxView/*0200005C*/::get_Value() /* 060002F6 */
    .set instance void System.Activities.Core.Presentation.ICaseKeyBoxView/*0200005C*/::set_Value(object) /* 060002F7 */
  } // end of property ICaseKeyBoxView::Value
  .property /*170000DB*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ValueType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Core.Presentation.ICaseKeyBoxView/*0200005C*/::get_ValueType() /* 060002F8 */
  } // end of property ICaseKeyBoxView::ValueType
  .property /*170000DC*/ instance callconv(8) class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/
          CaseKeyValidationCallback()
  {
    .get instance class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ System.Activities.Core.Presentation.ICaseKeyBoxView/*0200005C*/::get_CaseKeyValidationCallback() /* 060002F9 */
  } // end of property ICaseKeyBoxView::CaseKeyValidationCallback
} // end of class System.Activities.Core.Presentation.ICaseKeyBoxView

.class /*0200005D*/ private abstract auto ansi sealed beforefieldinit System.Activities.Core.Presentation.ComboBoxHelper
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*060002FA*/ public hidebysig static 
          bool  ShouldFilterUnnecessaryComboBoxEvent(class [PresentationFramework/*23000006*/]System.Windows.Controls.ComboBox/*0100000D*/ comboBox) cil managed
  // SIG: 00 01 02 12 35
  {
    // Method begins at RVA 0x23db
    // Code size       0 (0x0)
  } // end of method ComboBoxHelper::ShouldFilterUnnecessaryComboBoxEvent

  .method /*060002FB*/ public hidebysig static 
          void  SynchronizeComboBoxSelection(class [PresentationFramework/*23000006*/]System.Windows.Controls.ComboBox/*0100000D*/ comboBox,
                                             string 'value') cil managed
  // SIG: 00 02 01 12 35 0E
  {
    // Method begins at RVA 0x23dc
    // Code size       0 (0x0)
  } // end of method ComboBoxHelper::SynchronizeComboBoxSelection

} // end of class System.Activities.Core.Presentation.ComboBoxHelper

.class /*0200005E*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.ExpressionToExpressionTextConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*060002FC*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x23dd
    // Code size       0 (0x0)
  } // end of method ExpressionToExpressionTextConverter::Convert

  .method /*060002FD*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x23de
    // Code size       0 (0x0)
  } // end of method ExpressionToExpressionTextConverter::ConvertBack

  .method /*060002FE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23df
    // Code size       0 (0x0)
  } // end of method ExpressionToExpressionTextConverter::.ctor

} // end of class System.Activities.Core.Presentation.ExpressionToExpressionTextConverter

.class /*0200005F*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FakeRoot
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*060002FF*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.StartNode/*0200006A*/ 
          get_StartNode() cil managed
  // SIG: 20 00 12 81 A8
  {
    .custom /*0C00016C:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e0
    // Code size       0 (0x0)
  } // end of method FakeRoot::get_StartNode

  .method /*06000300*/ public hidebysig specialname 
          instance void  set_StartNode(class System.Activities.Core.Presentation.StartNode/*0200006A*/ 'value') cil managed
  // SIG: 20 01 01 12 81 A8
  {
    .custom /*0C00016D:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e1
    // Code size       0 (0x0)
  } // end of method FakeRoot::set_StartNode

  .method /*06000301*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23e2
    // Code size       0 (0x0)
  } // end of method FakeRoot::.ctor

  .property /*170000DD*/ instance callconv(8) class System.Activities.Core.Presentation.StartNode/*0200006A*/
          StartNode()
  {
    .get instance class System.Activities.Core.Presentation.StartNode/*0200006A*/ System.Activities.Core.Presentation.FakeRoot/*0200005F*/::get_StartNode() /* 060002FF */
    .set instance void System.Activities.Core.Presentation.FakeRoot/*0200005F*/::set_StartNode(class System.Activities.Core.Presentation.StartNode/*0200006A*/) /* 06000300 */
  } // end of property FakeRoot::StartNode
} // end of class System.Activities.Core.Presentation.FakeRoot

.class /*02000060*/ public abstract auto ansi sealed beforefieldinit System.Activities.Core.Presentation.FlowchartDesignerCommands
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .field /*040000C7*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ ConnectNodesCommand
  .method /*06000302*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23e3
    // Code size       0 (0x0)
  } // end of method FlowchartDesignerCommands::.cctor

} // end of class System.Activities.Core.Presentation.FlowchartDesignerCommands

.class /*02000061*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.FlowSwitchCaseEditorDialog
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowElementDialog/*0100003B*/
{
  .method /*06000303*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ activity,
                               class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context,
                               class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ owner,
                               string title,
                               class [mscorlib/*23000001*/]System.Type/*01000003*/ genericType) cil managed
  // SIG: 20 05 01 12 2D 12 79 12 6D 0E 12 0D
  {
    // Method begins at RVA 0x23e4
    // Code size       0 (0x0)
  } // end of method FlowSwitchCaseEditorDialog::.ctor

  .method /*06000304*/ family hidebysig virtual 
          instance void  OnKeyDown(class [PresentationCore/*23000009*/]System.Windows.Input.KeyEventArgs/*01000030*/ e) cil managed
  // SIG: 20 01 01 12 80 C1
  {
    // Method begins at RVA 0x23e5
    // Code size       0 (0x0)
  } // end of method FlowSwitchCaseEditorDialog::OnKeyDown

  .method /*06000305*/ public hidebysig specialname 
          instance string  get_CaseName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23e6
    // Code size       0 (0x0)
  } // end of method FlowSwitchCaseEditorDialog::get_CaseName

  .method /*06000306*/ public hidebysig specialname 
          instance object  get_Case() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x23e7
    // Code size       0 (0x0)
  } // end of method FlowSwitchCaseEditorDialog::get_Case

  .method /*06000307*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23e8
    // Code size       0 (0x0)
  } // end of method FlowSwitchCaseEditorDialog::.cctor

  .property /*170000DE*/ instance callconv(8) string
          CaseName()
  {
    .get instance string System.Activities.Core.Presentation.FlowSwitchCaseEditorDialog/*02000061*/::get_CaseName() /* 06000305 */
  } // end of property FlowSwitchCaseEditorDialog::CaseName
  .property /*170000DF*/ instance callconv(8) object
          Case()
  {
    .get instance object System.Activities.Core.Presentation.FlowSwitchCaseEditorDialog/*02000061*/::get_Case() /* 06000306 */
  } // end of property FlowSwitchCaseEditorDialog::Case
} // end of class System.Activities.Core.Presentation.FlowSwitchCaseEditorDialog

.class /*02000062*/ private abstract auto ansi beforefieldinit System.Activities.Core.Presentation.FlowSwitchLink`1<T>
       extends [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/
       implements System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/
{
  .field /*040000C8*/ family bool internalChange
  .field /*040000C9*/ family class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ flowSwitchModelItem
  .method /*06000308*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ flowSwitchMI,
                               !T caseValue,
                               bool isDefault) cil managed
  // SIG: 20 03 01 12 2D 13 00 02
  {
    // Method begins at RVA 0x23e9
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::.ctor

  .method /*06000309*/ public hidebysig newslot specialname virtual final 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_ModelItem() cil managed
  // SIG: 20 00 12 2D
  {
    .custom /*0C00016E:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23ea
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::get_ModelItem

  .method /*0600030A*/ public hidebysig newslot specialname virtual final 
          instance void  set_ModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    .custom /*0C00016F:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23eb
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::set_ModelItem

  .method /*0600030B*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ 
          get_ValidateCaseKey() cil managed
  // SIG: 20 00 12 81 6C
  {
    // Method begins at RVA 0x23ec
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::get_ValidateCaseKey

  .method /*0600030C*/ public hidebysig newslot specialname virtual final 
          instance class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ 
          get_ParentFlowSwitch() cil managed
  // SIG: 20 00 12 81 DD
  {
    // Method begins at RVA 0x23ed
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::get_ParentFlowSwitch

  .method /*0600030D*/ public hidebysig newslot specialname virtual final 
          instance void  set_ParentFlowSwitch(class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ 'value') cil managed
  // SIG: 20 01 01 12 81 DD
  {
    // Method begins at RVA 0x23ee
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::set_ParentFlowSwitch

  .method /*0600030E*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsDefaultCase() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23ef
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::get_IsDefaultCase

  .method /*0600030F*/ public hidebysig newslot specialname virtual final 
          instance void  set_IsDefaultCase(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23f0
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::set_IsDefaultCase

  .method /*06000310*/ public hidebysig newslot specialname virtual final 
          instance string  get_CaseName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23f1
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::get_CaseName

  .method /*06000311*/ public hidebysig newslot specialname virtual final 
          instance object  get_CaseObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x23f2
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::get_CaseObject

  .method /*06000312*/ public hidebysig specialname 
          instance void  set_CaseObject(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x23f3
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::set_CaseObject

  .method /*06000313*/ public hidebysig specialname 
          instance !T  get_Case() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x23f4
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::get_Case

  .method /*06000314*/ public hidebysig specialname 
          instance void  set_Case(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x23f5
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::set_Case

  .method /*06000315*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_GenericType() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x23f6
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::get_GenericType

  .method /*06000316*/ family hidebysig instance bool 
          IsUndoRedoInProgress() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23f7
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::IsUndoRedoInProgress

  .method /*06000317*/ public hidebysig newslot virtual 
          instance class [PresentationFramework/*23000006*/]System.Windows.Data.MultiBinding/*01000078*/ 
          CreateConnectorLabelTextBinding() cil managed
  // SIG: 20 00 12 81 E1
  {
    // Method begins at RVA 0x23f8
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::CreateConnectorLabelTextBinding

  .method /*06000318*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23f9
    // Code size       0 (0x0)
  } // end of method FlowSwitchLink`1::.cctor

  .property /*170000E0*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          ModelItem()
  {
    .custom /*0C00009D:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::get_ModelItem() /* 06000309 */
    .set instance void System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::set_ModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 0600030A */
  } // end of property FlowSwitchLink`1::ModelItem
  .property /*170000E1*/ instance callconv(8) class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/
          ValidateCaseKey()
  {
    .get instance class System.Activities.Core.Presentation.CaseKeyValidationCallbackDelegate/*0200005B*/ System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::get_ValidateCaseKey() /* 0600030B */
  } // end of property FlowSwitchLink`1::ValidateCaseKey
  .property /*170000E2*/ instance callconv(8) class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/
          ParentFlowSwitch()
  {
    .custom /*0C00009E:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::get_ParentFlowSwitch() /* 0600030C */
    .set instance void System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::set_ParentFlowSwitch(class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/) /* 0600030D */
  } // end of property FlowSwitchLink`1::ParentFlowSwitch
  .property /*170000E3*/ instance callconv(8) bool
          IsDefaultCase()
  {
    .get instance bool System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::get_IsDefaultCase() /* 0600030E */
    .set instance void System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::set_IsDefaultCase(bool) /* 0600030F */
  } // end of property FlowSwitchLink`1::IsDefaultCase
  .property /*170000E4*/ instance callconv(8) string
          CaseName()
  {
    .custom /*0C00009F:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 00 00 00 ) 
    .get instance string System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::get_CaseName() /* 06000310 */
  } // end of property FlowSwitchLink`1::CaseName
  .property /*170000E5*/ instance callconv(8) object
          CaseObject()
  {
    .custom /*0C0000A0:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 00 00 00 ) 
    .get instance object System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::get_CaseObject() /* 06000311 */
    .set instance void System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::set_CaseObject(object) /* 06000312 */
  } // end of property FlowSwitchLink`1::CaseObject
  .property /*170000E6*/ instance callconv(8) !T
          Case()
  {
    .custom /*0C0000A1:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 00 00 00 ) 
    .get instance !T System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::get_Case() /* 06000313 */
    .set instance void System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::set_Case(!T) /* 06000314 */
  } // end of property FlowSwitchLink`1::Case
  .property /*170000E7*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          GenericType()
  {
    .custom /*0C0000A2:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 00 00 00 ) 
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/::get_GenericType() /* 06000315 */
  } // end of property FlowSwitchLink`1::GenericType
} // end of class System.Activities.Core.Presentation.FlowSwitchLink`1

.class /*02000063*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowSwitchDefaultLink`1<T>
       extends class System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/<!T>/*1B000005*/
       implements System.Activities.Core.Presentation.IFlowSwitchDefaultLink/*0200006C*/,
                  System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/
{
  .method /*06000319*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ flowSwitchMI,
                               !T caseValue,
                               bool isDefault) cil managed
  // SIG: 20 03 01 12 2D 13 00 02
  {
    // Method begins at RVA 0x23fa
    // Code size       0 (0x0)
  } // end of method FlowSwitchDefaultLink`1::.ctor

  .method /*0600031A*/ public hidebysig newslot specialname virtual final 
          instance string  get_DefaultCaseDisplayName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23fb
    // Code size       0 (0x0)
  } // end of method FlowSwitchDefaultLink`1::get_DefaultCaseDisplayName

  .method /*0600031B*/ public hidebysig newslot specialname virtual final 
          instance void  set_DefaultCaseDisplayName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23fc
    // Code size       0 (0x0)
  } // end of method FlowSwitchDefaultLink`1::set_DefaultCaseDisplayName

  .method /*0600031C*/ public hidebysig virtual 
          instance class [PresentationFramework/*23000006*/]System.Windows.Data.MultiBinding/*01000078*/ 
          CreateConnectorLabelTextBinding() cil managed
  // SIG: 20 00 12 81 E1
  {
    // Method begins at RVA 0x23fd
    // Code size       0 (0x0)
  } // end of method FlowSwitchDefaultLink`1::CreateConnectorLabelTextBinding

  .method /*0600031D*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23fe
    // Code size       0 (0x0)
  } // end of method FlowSwitchDefaultLink`1::.cctor

  .property /*170000E8*/ instance callconv(8) string
          DefaultCaseDisplayName()
  {
    .get instance string System.Activities.Core.Presentation.FlowSwitchDefaultLink`1/*02000063*/::get_DefaultCaseDisplayName() /* 0600031A */
    .set instance void System.Activities.Core.Presentation.FlowSwitchDefaultLink`1/*02000063*/::set_DefaultCaseDisplayName(string) /* 0600031B */
  } // end of property FlowSwitchDefaultLink`1::DefaultCaseDisplayName
} // end of class System.Activities.Core.Presentation.FlowSwitchDefaultLink`1

.class /*02000064*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowSwitchCaseLink`1<T>
       extends class System.Activities.Core.Presentation.FlowSwitchLink`1/*02000062*/<!T>/*1B000005*/
{
  .method /*0600031E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ flowSwitchMI,
                               !T caseValue,
                               bool isDefault) cil managed
  // SIG: 20 03 01 12 2D 13 00 02
  {
    // Method begins at RVA 0x23ff
    // Code size       0 (0x0)
  } // end of method FlowSwitchCaseLink`1::.ctor

  .method /*0600031F*/ public hidebysig specialname 
          instance !T  get_Case() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x2400
    // Code size       0 (0x0)
  } // end of method FlowSwitchCaseLink`1::get_Case

  .method /*06000320*/ public hidebysig specialname 
          instance void  set_Case(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2401
    // Code size       0 (0x0)
  } // end of method FlowSwitchCaseLink`1::set_Case

  .property /*170000E9*/ instance callconv(8) !T
          Case()
  {
    .custom /*0C0000A3:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 01 00 00 ) 
    .get instance !T System.Activities.Core.Presentation.FlowSwitchCaseLink`1/*02000064*/::get_Case() /* 0600031F */
    .set instance void System.Activities.Core.Presentation.FlowSwitchCaseLink`1/*02000064*/::set_Case(!T) /* 06000320 */
  } // end of property FlowSwitchCaseLink`1::Case
} // end of class System.Activities.Core.Presentation.FlowSwitchCaseLink`1

.class /*02000065*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.FlowSwitchLinkMultiValueConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IMultiValueConverter/*01000079*/
{
  .method /*06000321*/ public hidebysig newslot virtual final 
          instance object  Convert(object[] values,
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1D 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2402
    // Code size       0 (0x0)
  } // end of method FlowSwitchLinkMultiValueConverter::Convert

  .method /*06000322*/ public hidebysig newslot virtual final 
          instance object[]  ConvertBack(object 'value',
                                         class [mscorlib/*23000001*/]System.Type/*01000003*/[] targetTypes,
                                         object parameter,
                                         class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1D 1C 1C 1D 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2403
    // Code size       0 (0x0)
  } // end of method FlowSwitchLinkMultiValueConverter::ConvertBack

  .method /*06000323*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2404
    // Code size       0 (0x0)
  } // end of method FlowSwitchLinkMultiValueConverter::.ctor

} // end of class System.Activities.Core.Presentation.FlowSwitchLinkMultiValueConverter

.class /*02000066*/ private abstract auto ansi sealed beforefieldinit System.Activities.Core.Presentation.GenericFlowSwitchHelper
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*06000324*/ public hidebysig specialname static 
          string  get_FlowSwitchCasesKeyIdentifier() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2405
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::get_FlowSwitchCasesKeyIdentifier

  .method /*06000325*/ public hidebysig specialname static 
          string  get_FlowSwitchNullCaseKeyIdentifier() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2406
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::get_FlowSwitchNullCaseKeyIdentifier

  .method /*06000326*/ public hidebysig specialname static 
          string  get_FlowSwitchEmptyCaseKeyIdentifier() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2407
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::get_FlowSwitchEmptyCaseKeyIdentifier

  .method /*06000327*/ public hidebysig static 
          void  Copy(class [mscorlib/*23000001*/]System.Type/*01000003*/ genericType,
                     class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ currentFlowElement,
                     class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/,class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/> clonedFlowElements) cil managed
  // SIG: 00 03 01 12 0D 12 81 DD 15 12 81 3D 02 12 81 DD 12 81 DD
  {
    // Method begins at RVA 0x2408
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::Copy

  .method /*06000328*/ public hidebysig static 
          void  GenericCopy<T>(class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ currentFlowElement,
                               class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/,class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/> clonedFlowElements) cil managed
  // SIG: 10 01 02 01 12 81 DD 15 12 81 3D 02 12 81 DD 12 81 DD
  {
    // Method begins at RVA 0x2409
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GenericCopy

  .method /*06000329*/ public hidebysig static 
          void  ReferenceCopy(class [mscorlib/*23000001*/]System.Type/*01000003*/ genericType,
                              class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ currentFlowElement,
                              class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/,class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> modelItems,
                              class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/,class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/> clonedFlowElements) cil managed
  // SIG: 00 04 01 12 0D 12 81 DD 15 12 81 3D 02 12 81 DD 12 2D 15 12 81 3D 02 12 81 DD 12 81 DD
  {
    // Method begins at RVA 0x240a
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::ReferenceCopy

  .method /*0600032A*/ public hidebysig static 
          void  GenericRemapFlowSwitch<T>(class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ currentFlowElement,
                                          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem,
                                          class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*0100004F*/<class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/,class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/> oldNewFlowNodeMap) cil managed
  // SIG: 10 01 03 01 12 81 DD 12 2D 15 12 81 3D 02 12 81 DD 12 81 DD
  {
    // Method begins at RVA 0x240b
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GenericRemapFlowSwitch

  .method /*0600032B*/ public hidebysig static 
          bool  IsGenericFlowSwitch(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 00 01 02 12 0D
  {
    // Method begins at RVA 0x240c
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::IsGenericFlowSwitch

  .method /*0600032C*/ public hidebysig static 
          class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ 
          CreateFlowSwitchLink(class [mscorlib/*23000001*/]System.Type/*01000003*/ flowSwitchType,
                               class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ currentMI,
                               object caseValue,
                               bool isDefault) cil managed
  // SIG: 00 04 12 81 AC 12 0D 12 2D 1C 02
  {
    // Method begins at RVA 0x240d
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::CreateFlowSwitchLink

  .method /*0600032D*/ public hidebysig static 
          class System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/ 
          CreateGenericFlowSwitchLink<T>(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ currentMI,
                                         !!T caseValue,
                                         bool isDefault) cil managed
  // SIG: 10 01 03 12 81 AC 12 2D 1E 00 02
  {
    // Method begins at RVA 0x240e
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::CreateGenericFlowSwitchLink

  .method /*0600032E*/ public hidebysig static 
          string  GetCaseName(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ casesProperties,
                              class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type',
                              [out] string& errorMessage) cil managed
  // SIG: 00 03 0E 12 80 E1 12 0D 10 0E
  {
    // Method begins at RVA 0x240f
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GetCaseName

  .method /*0600032F*/ public hidebysig static 
          string  GenericGetCaseName<T>(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/ collection,
                                        [out] string& errorMessage) cil managed
  // SIG: 10 01 02 0E 12 81 E9 10 0E
  {
    // Method begins at RVA 0x2410
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GenericGetCaseName

  .method /*06000330*/ public hidebysig static 
          bool  IsIntegralType(class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 00 01 02 12 0D
  {
    // Method begins at RVA 0x2411
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::IsIntegralType

  .method /*06000331*/ public hidebysig static 
          string  GetString(object key,
                            class [mscorlib/*23000001*/]System.Type/*01000003*/ 'type') cil managed
  // SIG: 00 02 0E 1C 12 0D
  {
    // Method begins at RVA 0x2412
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GetString

  .method /*06000332*/ public hidebysig static 
          object  GetObject(string caseString,
                            class [mscorlib/*23000001*/]System.Type/*01000003*/ genericType) cil managed
  // SIG: 00 02 1C 0E 12 0D
  {
    // Method begins at RVA 0x2413
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GetObject

  .method /*06000333*/ public hidebysig static 
          bool  ContainsCaseKey(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ casesProp,
                                object key) cil managed
  // SIG: 00 02 02 12 80 E1 1C
  {
    // Method begins at RVA 0x2414
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::ContainsCaseKey

  .method /*06000334*/ public hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          GetCaseModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ casesProp,
                           object key) cil managed
  // SIG: 00 02 12 2D 12 80 E1 1C
  {
    // Method begins at RVA 0x2415
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GetCaseModelItem

  .method /*06000335*/ public hidebysig static 
          object  GetCase(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/ itemsCollection,
                          object key) cil managed
  // SIG: 00 02 1C 12 81 E9 1C
  {
    // Method begins at RVA 0x2416
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GetCase

  .method /*06000336*/ public hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/[] 
          GetCaseKeys(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ casesProp) cil managed
  // SIG: 00 01 1D 12 2D 12 80 E1
  {
    // Method begins at RVA 0x2417
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::GetCaseKeys

  .method /*06000337*/ public hidebysig static 
          void  RemoveCase(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ casesProp,
                           object key) cil managed
  // SIG: 00 02 01 12 80 E1 1C
  {
    // Method begins at RVA 0x2418
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::RemoveCase

  .method /*06000338*/ public hidebysig static 
          void  AddCase(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ casesPropperties,
                        object newKey,
                        object newCase) cil managed
  // SIG: 00 03 01 12 80 E1 1C 1C
  {
    // Method begins at RVA 0x2419
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::AddCase

  .method /*06000339*/ public hidebysig static 
          bool  CanBeGeneratedUniquely(class [mscorlib/*23000001*/]System.Type/*01000003*/ typeArgument) cil managed
  // SIG: 00 01 02 12 0D
  {
    // Method begins at RVA 0x241a
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::CanBeGeneratedUniquely

  .method /*0600033A*/ public hidebysig static 
          bool  CheckEquality(object 'value',
                              class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType) cil managed
  // SIG: 00 02 02 1C 12 0D
  {
    // Method begins at RVA 0x241b
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::CheckEquality

  .method /*0600033B*/ public hidebysig static 
          bool  ValidateCaseKey(object obj,
                                class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ casesProp,
                                class [mscorlib/*23000001*/]System.Type/*01000003*/ genericType,
                                [out] string& reason) cil managed
  // SIG: 00 04 02 1C 12 80 E1 12 0D 10 0E
  {
    // Method begins at RVA 0x241c
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::ValidateCaseKey

  .method /*0600033C*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x241d
    // Code size       0 (0x0)
  } // end of method GenericFlowSwitchHelper::.cctor

  .property /*170000EA*/ callconv(8) string
          FlowSwitchCasesKeyIdentifier()
  {
    .get string System.Activities.Core.Presentation.GenericFlowSwitchHelper/*02000066*/::get_FlowSwitchCasesKeyIdentifier() /* 06000324 */
  } // end of property GenericFlowSwitchHelper::FlowSwitchCasesKeyIdentifier
  .property /*170000EB*/ callconv(8) string
          FlowSwitchNullCaseKeyIdentifier()
  {
    .get string System.Activities.Core.Presentation.GenericFlowSwitchHelper/*02000066*/::get_FlowSwitchNullCaseKeyIdentifier() /* 06000325 */
  } // end of property GenericFlowSwitchHelper::FlowSwitchNullCaseKeyIdentifier
  .property /*170000EC*/ callconv(8) string
          FlowSwitchEmptyCaseKeyIdentifier()
  {
    .get string System.Activities.Core.Presentation.GenericFlowSwitchHelper/*02000066*/::get_FlowSwitchEmptyCaseKeyIdentifier() /* 06000326 */
  } // end of property GenericFlowSwitchHelper::FlowSwitchEmptyCaseKeyIdentifier
} // end of class System.Activities.Core.Presentation.GenericFlowSwitchHelper

.class /*02000067*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowchartExpressionAdorner
       extends [PresentationFramework/*23000006*/]System.Windows.Documents.Adorner/*0100007B*/
{
  .method /*0600033D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ adornedElement) cil managed
  // SIG: 20 01 01 12 81 41
  {
    // Method begins at RVA 0x241e
    // Code size       0 (0x0)
  } // end of method FlowchartExpressionAdorner::.ctor

  .method /*0600033E*/ family hidebysig virtual 
          instance void  OnRender(class [PresentationCore/*23000009*/]System.Windows.Media.DrawingContext/*0100007C*/ drawingContext) cil managed
  // SIG: 20 01 01 12 81 F1
  {
    // Method begins at RVA 0x241f
    // Code size       0 (0x0)
  } // end of method FlowchartExpressionAdorner::OnRender

  .method /*0600033F*/ public hidebysig static 
          string  GetExpressionString(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ adornedElement) cil managed
  // SIG: 00 01 0E 12 81 41
  {
    // Method begins at RVA 0x2420
    // Code size       0 (0x0)
  } // end of method FlowchartExpressionAdorner::GetExpressionString

} // end of class System.Activities.Core.Presentation.FlowchartExpressionAdorner

.class /*02000068*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowDecisionLabelFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ViewStateAttachedPropertyFeature/*0100006A*/
{
  .class /*020000A4*/ auto ansi sealed nested private beforefieldinit '<get_AttachedProperties>d__1'
         extends [mscorlib/*23000001*/]System.Object/*01000002*/
         implements class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000004*/,
                    [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/,
                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000003*/,
                    [mscorlib/*23000001*/]System.IDisposable/*01000023*/,
                    [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/
  {
    .custom /*0C000072:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    .method /*06000497*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0001E2:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.IDisposable/*01000023*/::Dispose /*01000023::0A000005*/ 
      // Method begins at RVA 0x2583
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__1'::System.IDisposable.Dispose

    .method /*06000498*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::MoveNext /*01000024::0A000006*/ 
      // Method begins at RVA 0x2584
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__1'::MoveNext

    .method /*06000499*/ private hidebysig newslot specialname virtual final 
            instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/ 
            'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'() cil managed
    // SIG: 20 00 12 81 AD
    {
      .custom /*0C0001E3:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000003*/::get_Current() /* 0A00000C */
      // Method begins at RVA 0x2585
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__1'::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'

    .method /*0600049A*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0001E4:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::Reset /*01000024::0A000008*/ 
      // Method begins at RVA 0x2586
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__1'::System.Collections.IEnumerator.Reset

    .method /*0600049B*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C0001E5:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::get_Current /*01000024::0A000009*/ 
      // Method begins at RVA 0x2587
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__1'::System.Collections.IEnumerator.get_Current

    .method /*0600049C*/ private hidebysig newslot virtual final 
            instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> 
            'System.Collections.Generic.IEnumerable<System.Activities.Presentation.Model.AttachedPropertyInfo>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 95 01 12 81 AD
    {
      .custom /*0C0001E6:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override  method instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<!0> class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000004*/::GetEnumerator() /* 0A00000D */
      // Method begins at RVA 0x2588
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__1'::'System.Collections.Generic.IEnumerable<System.Activities.Presentation.Model.AttachedPropertyInfo>.GetEnumerator'

    .method /*0600049D*/ private hidebysig newslot virtual final 
            instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 91
    {
      .custom /*0C0001E7:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/::GetEnumerator /*01000026::0A00000B*/ 
      // Method begins at RVA 0x2589
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__1'::System.Collections.IEnumerable.GetEnumerator

    .method /*0600049E*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x258a
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__1'::.ctor

    .property /*17000136*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/
            'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.Current'()
    {
      .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/ System.Activities.Core.Presentation.FlowDecisionLabelFeature/*02000068*//'<get_AttachedProperties>d__1'/*020000A4*/::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'() /* 06000499 */
    } // end of property '<get_AttachedProperties>d__1'::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.Current'
    .property /*17000137*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object System.Activities.Core.Presentation.FlowDecisionLabelFeature/*02000068*//'<get_AttachedProperties>d__1'/*020000A4*/::System.Collections.IEnumerator.get_Current() /* 0600049B */
    } // end of property '<get_AttachedProperties>d__1'::System.Collections.IEnumerator.Current
  } // end of class '<get_AttachedProperties>d__1'

  .method /*06000340*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> 
          get_AttachedProperties() cil managed
  // SIG: 20 00 15 12 7D 01 12 81 AD
  {
    .custom /*0C000171:0A000028*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*010000AA*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000028 */ = ( 01 00 59 53 79 73 74 65 6D 2E 41 63 74 69 76 69   // ..YSystem.Activi
                                                                                                                                                                                                                               74 69 65 73 2E 43 6F 72 65 2E 50 72 65 73 65 6E   // ties.Core.Presen
                                                                                                                                                                                                                               74 61 74 69 6F 6E 2E 46 6C 6F 77 44 65 63 69 73   // tation.FlowDecis
                                                                                                                                                                                                                               69 6F 6E 4C 61 62 65 6C 46 65 61 74 75 72 65 2B   // ionLabelFeature+
                                                                                                                                                                                                                               3C 67 65 74 5F 41 74 74 61 63 68 65 64 50 72 6F   // <get_AttachedPro
                                                                                                                                                                                                                               70 65 72 74 69 65 73 3E 64 5F 5F 31 00 00 )       // perties>d__1..
    // Method begins at RVA 0x2421
    // Code size       0 (0x0)
  } // end of method FlowDecisionLabelFeature::get_AttachedProperties

  .method /*06000341*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2422
    // Code size       0 (0x0)
  } // end of method FlowDecisionLabelFeature::.ctor

  .property /*170000ED*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>
          AttachedProperties()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> System.Activities.Core.Presentation.FlowDecisionLabelFeature/*02000068*/::get_AttachedProperties() /* 06000340 */
  } // end of property FlowDecisionLabelFeature::AttachedProperties
} // end of class System.Activities.Core.Presentation.FlowDecisionLabelFeature

.class /*02000069*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowSwitchLabelFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ViewStateAttachedPropertyFeature/*0100006A*/
{
  .class /*020000A5*/ auto ansi sealed nested private beforefieldinit '<get_AttachedProperties>d__3'
         extends [mscorlib/*23000001*/]System.Object/*01000002*/
         implements class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000004*/,
                    [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/,
                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000003*/,
                    [mscorlib/*23000001*/]System.IDisposable/*01000023*/,
                    [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/
  {
    .custom /*0C000075:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    .method /*0600049F*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0001E8:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.IDisposable/*01000023*/::Dispose /*01000023::0A000005*/ 
      // Method begins at RVA 0x258b
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__3'::System.IDisposable.Dispose

    .method /*060004A0*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::MoveNext /*01000024::0A000006*/ 
      // Method begins at RVA 0x258c
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__3'::MoveNext

    .method /*060004A1*/ private hidebysig newslot specialname virtual final 
            instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/ 
            'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'() cil managed
    // SIG: 20 00 12 81 AD
    {
      .custom /*0C0001E9:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000003*/::get_Current() /* 0A00000C */
      // Method begins at RVA 0x258d
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__3'::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'

    .method /*060004A2*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0001EA:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::Reset /*01000024::0A000008*/ 
      // Method begins at RVA 0x258e
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__3'::System.Collections.IEnumerator.Reset

    .method /*060004A3*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C0001EB:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/::get_Current /*01000024::0A000009*/ 
      // Method begins at RVA 0x258f
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__3'::System.Collections.IEnumerator.get_Current

    .method /*060004A4*/ private hidebysig newslot virtual final 
            instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> 
            'System.Collections.Generic.IEnumerable<System.Activities.Presentation.Model.AttachedPropertyInfo>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 95 01 12 81 AD
    {
      .custom /*0C0001EC:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override  method instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<!0> class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>/*1B000004*/::GetEnumerator() /* 0A00000D */
      // Method begins at RVA 0x2590
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__3'::'System.Collections.Generic.IEnumerable<System.Activities.Presentation.Model.AttachedPropertyInfo>.GetEnumerator'

    .method /*060004A5*/ private hidebysig newslot virtual final 
            instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000024*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 91
    {
      .custom /*0C0001ED:0A000029*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*010000AB*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
      .override [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000026*/::GetEnumerator /*01000026::0A00000B*/ 
      // Method begins at RVA 0x2591
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__3'::System.Collections.IEnumerable.GetEnumerator

    .method /*060004A6*/ private specialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2592
      // Code size       0 (0x0)
    } // end of method '<get_AttachedProperties>d__3'::.ctor

    .property /*17000138*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/
            'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.Current'()
    {
      .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/ System.Activities.Core.Presentation.FlowSwitchLabelFeature/*02000069*//'<get_AttachedProperties>d__3'/*020000A5*/::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.get_Current'() /* 060004A1 */
    } // end of property '<get_AttachedProperties>d__3'::'System.Collections.Generic.IEnumerator<System.Activities.Presentation.Model.AttachedPropertyInfo>.Current'
    .property /*17000139*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object System.Activities.Core.Presentation.FlowSwitchLabelFeature/*02000069*//'<get_AttachedProperties>d__3'/*020000A5*/::System.Collections.IEnumerator.get_Current() /* 060004A3 */
    } // end of property '<get_AttachedProperties>d__3'::System.Collections.IEnumerator.Current
  } // end of class '<get_AttachedProperties>d__3'

  .field /*040000CA*/ public static literal string DefaultCaseDisplayNamePropertyName = "DefaultCaseDisplayName"
  .field /*040000CB*/ public static literal string DefaultCaseDisplayNameDefaultValue = "Default"
  .method /*06000342*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> 
          get_AttachedProperties() cil managed
  // SIG: 20 00 15 12 7D 01 12 81 AD
  {
    .custom /*0C000172:0A000028*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*010000AA*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000028 */ = ( 01 00 57 53 79 73 74 65 6D 2E 41 63 74 69 76 69   // ..WSystem.Activi
                                                                                                                                                                                                                               74 69 65 73 2E 43 6F 72 65 2E 50 72 65 73 65 6E   // ties.Core.Presen
                                                                                                                                                                                                                               74 61 74 69 6F 6E 2E 46 6C 6F 77 53 77 69 74 63   // tation.FlowSwitc
                                                                                                                                                                                                                               68 4C 61 62 65 6C 46 65 61 74 75 72 65 2B 3C 67   // hLabelFeature+<g
                                                                                                                                                                                                                               65 74 5F 41 74 74 61 63 68 65 64 50 72 6F 70 65   // et_AttachedPrope
                                                                                                                                                                                                                               72 74 69 65 73 3E 64 5F 5F 33 00 00 )             // rties>d__3..
    // Method begins at RVA 0x2423
    // Code size       0 (0x0)
  } // end of method FlowSwitchLabelFeature::get_AttachedProperties

  .method /*06000343*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2424
    // Code size       0 (0x0)
  } // end of method FlowSwitchLabelFeature::.ctor

  .property /*170000EE*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/>
          AttachedProperties()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.AttachedPropertyInfo/*0100006B*/> System.Activities.Core.Presentation.FlowSwitchLabelFeature/*02000069*/::get_AttachedProperties() /* 06000342 */
  } // end of property FlowSwitchLabelFeature::AttachedProperties
} // end of class System.Activities.Core.Presentation.FlowSwitchLabelFeature

.class /*0200006A*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StartNode
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*06000344*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2425
    // Code size       0 (0x0)
  } // end of method StartNode::.ctor

} // end of class System.Activities.Core.Presentation.StartNode

.class /*0200006B*/ interface private abstract auto ansi System.Activities.Core.Presentation.IFlowSwitchLink
{
  .method /*06000345*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_ModelItem() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::get_ModelItem

  .method /*06000346*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_ModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::set_ModelItem

  .method /*06000347*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ 
          get_ParentFlowSwitch() cil managed
  // SIG: 20 00 12 81 DD
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::get_ParentFlowSwitch

  .method /*06000348*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_ParentFlowSwitch(class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ 'value') cil managed
  // SIG: 20 01 01 12 81 DD
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::set_ParentFlowSwitch

  .method /*06000349*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_IsDefaultCase() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::get_IsDefaultCase

  .method /*0600034A*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_IsDefaultCase(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::set_IsDefaultCase

  .method /*0600034B*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_CaseName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::get_CaseName

  .method /*0600034C*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_CaseObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::get_CaseObject

  .method /*0600034D*/ public hidebysig newslot abstract virtual 
          instance class [PresentationFramework/*23000006*/]System.Windows.Data.MultiBinding/*01000078*/ 
          CreateConnectorLabelTextBinding() cil managed
  // SIG: 20 00 12 81 E1
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchLink::CreateConnectorLabelTextBinding

  .property /*170000EF*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          ModelItem()
  {
    .custom /*0C0000A4:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/::get_ModelItem() /* 06000345 */
    .set instance void System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/::set_ModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 06000346 */
  } // end of property IFlowSwitchLink::ModelItem
  .property /*170000F0*/ instance callconv(8) class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/
          ParentFlowSwitch()
  {
    .custom /*0C0000A5:0A00002B*/ instance void [System/*23000002*/]System.ComponentModel.BrowsableAttribute/*010000AD*/::.ctor(bool) /* 0A00002B */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/ System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/::get_ParentFlowSwitch() /* 06000347 */
    .set instance void System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/::set_ParentFlowSwitch(class [System.Activities/*2300000B*/]System.Activities.Statements.FlowNode/*01000077*/) /* 06000348 */
  } // end of property IFlowSwitchLink::ParentFlowSwitch
  .property /*170000F1*/ instance callconv(8) bool
          IsDefaultCase()
  {
    .get instance bool System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/::get_IsDefaultCase() /* 06000349 */
    .set instance void System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/::set_IsDefaultCase(bool) /* 0600034A */
  } // end of property IFlowSwitchLink::IsDefaultCase
  .property /*170000F2*/ instance callconv(8) string
          CaseName()
  {
    .get instance string System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/::get_CaseName() /* 0600034B */
  } // end of property IFlowSwitchLink::CaseName
  .property /*170000F3*/ instance callconv(8) object
          CaseObject()
  {
    .get instance object System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/::get_CaseObject() /* 0600034C */
  } // end of property IFlowSwitchLink::CaseObject
} // end of class System.Activities.Core.Presentation.IFlowSwitchLink

.class /*0200006C*/ interface private abstract auto ansi System.Activities.Core.Presentation.IFlowSwitchDefaultLink
       implements System.Activities.Core.Presentation.IFlowSwitchLink/*0200006B*/
{
  .method /*0600034E*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_DefaultCaseDisplayName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchDefaultLink::get_DefaultCaseDisplayName

  .method /*0600034F*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_DefaultCaseDisplayName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IFlowSwitchDefaultLink::set_DefaultCaseDisplayName

  .property /*170000F4*/ instance callconv(8) string
          DefaultCaseDisplayName()
  {
    .get instance string System.Activities.Core.Presentation.IFlowSwitchDefaultLink/*0200006C*/::get_DefaultCaseDisplayName() /* 0600034E */
    .set instance void System.Activities.Core.Presentation.IFlowSwitchDefaultLink/*0200006C*/::set_DefaultCaseDisplayName(string) /* 0600034F */
  } // end of property IFlowSwitchDefaultLink::DefaultCaseDisplayName
} // end of class System.Activities.Core.Presentation.IFlowSwitchDefaultLink

.class /*0200006D*/ public auto ansi sealed beforefieldinit System.Activities.Core.Presentation.GenericTypeArgumentConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*06000350*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2426
    // Code size       0 (0x0)
  } // end of method GenericTypeArgumentConverter::Convert

  .method /*06000351*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2427
    // Code size       0 (0x0)
  } // end of method GenericTypeArgumentConverter::ConvertBack

  .method /*06000352*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2428
    // Code size       0 (0x0)
  } // end of method GenericTypeArgumentConverter::.ctor

} // end of class System.Activities.Core.Presentation.GenericTypeArgumentConverter

.class /*0200006E*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.MaxValueConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IMultiValueConverter/*01000079*/
{
  .method /*06000353*/ public hidebysig newslot virtual final 
          instance object  Convert(object[] values,
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1D 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2429
    // Code size       0 (0x0)
  } // end of method MaxValueConverter::Convert

  .method /*06000354*/ public hidebysig newslot virtual final 
          instance object[]  ConvertBack(object 'value',
                                         class [mscorlib/*23000001*/]System.Type/*01000003*/[] targetTypes,
                                         object parameter,
                                         class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1D 1C 1C 1D 12 0D 1C 12 11
  {
    // Method begins at RVA 0x242a
    // Code size       0 (0x0)
  } // end of method MaxValueConverter::ConvertBack

  .method /*06000355*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x242b
    // Code size       0 (0x0)
  } // end of method MaxValueConverter::.ctor

} // end of class System.Activities.Core.Presentation.MaxValueConverter

.class /*0200006F*/ private abstract auto ansi sealed beforefieldinit System.Activities.Core.Presentation.MorphHelpers
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*06000356*/ public hidebysig static 
          object  ArgumentMorphHelper(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ originalValue,
                                      class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ newModelProperty) cil managed
  // SIG: 00 02 1C 12 2D 12 80 E1
  {
    // Method begins at RVA 0x242c
    // Code size       0 (0x0)
  } // end of method MorphHelpers::ArgumentMorphHelper

  .method /*06000357*/ assembly hidebysig static 
          class [System.Activities/*2300000B*/]System.Activities.Argument/*0100007D*/ 
          MorphArgument(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ originalValue,
                        class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType) cil managed
  // SIG: 00 02 12 81 F5 12 2D 12 0D
  {
    // Method begins at RVA 0x242d
    // Code size       0 (0x0)
  } // end of method MorphHelpers::MorphArgument

  .method /*06000358*/ public hidebysig static 
          object  ActivityActionMorphHelper(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ originalValue,
                                            class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ newModelProperty) cil managed
  // SIG: 00 02 1C 12 2D 12 80 E1
  {
    // Method begins at RVA 0x242e
    // Code size       0 (0x0)
  } // end of method MorphHelpers::ActivityActionMorphHelper

  .method /*06000359*/ public hidebysig static 
          object  ActivityFuncMorphHelper(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ originalValue,
                                          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelProperty/*01000038*/ newModelProperty) cil managed
  // SIG: 00 02 1C 12 2D 12 80 E1
  {
    // Method begins at RVA 0x242f
    // Code size       0 (0x0)
  } // end of method MorphHelpers::ActivityFuncMorphHelper

} // end of class System.Activities.Core.Presentation.MorphHelpers

.class /*02000070*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.NotConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*0600035A*/ public hidebysig newslot virtual final 
          instance object  Convert(object o,
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2430
    // Code size       0 (0x0)
  } // end of method NotConverter::Convert

  .method /*0600035B*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object o,
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2431
    // Code size       0 (0x0)
  } // end of method NotConverter::ConvertBack

  .method /*0600035C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2432
    // Code size       0 (0x0)
  } // end of method NotConverter::.ctor

} // end of class System.Activities.Core.Presentation.NotConverter

.class /*02000071*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.CaseKeyBoxIsEnabledConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IMultiValueConverter/*01000079*/
{
  .method /*0600035D*/ public hidebysig newslot virtual final 
          instance object  Convert(object[] values,
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1D 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2433
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxIsEnabledConverter::Convert

  .method /*0600035E*/ public hidebysig newslot virtual final 
          instance object[]  ConvertBack(object 'value',
                                         class [mscorlib/*23000001*/]System.Type/*01000003*/[] targetTypes,
                                         object parameter,
                                         class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1D 1C 1C 1D 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2434
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxIsEnabledConverter::ConvertBack

  .method /*0600035F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2435
    // Code size       0 (0x0)
  } // end of method CaseKeyBoxIsEnabledConverter::.ctor

} // end of class System.Activities.Core.Presentation.CaseKeyBoxIsEnabledConverter

.class /*02000072*/ private abstract auto ansi sealed beforefieldinit System.Activities.Core.Presentation.SwitchTryCatchDesignerHelper
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*06000360*/ public hidebysig static 
          void  MakeRootDesigner(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/ wve) cil managed
  // SIG: 00 01 01 12 21
  {
    // Method begins at RVA 0x2436
    // Code size       0 (0x0)
  } // end of method SwitchTryCatchDesignerHelper::MakeRootDesigner

  .method /*06000361*/ public hidebysig static 
          void  MakeParentRootDesigner<([System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/) TParentType>(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/ wve) cil managed
  // SIG: 10 01 01 01 12 21
  {
    // Method begins at RVA 0x2437
    // Code size       0 (0x0)
  } // end of method SwitchTryCatchDesignerHelper::MakeParentRootDesigner

} // end of class System.Activities.Core.Presentation.SwitchTryCatchDesignerHelper

.class /*02000073*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.HintTextConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IMultiValueConverter/*01000079*/
{
  .method /*06000362*/ public hidebysig newslot virtual final 
          instance object  Convert(object[] values,
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1D 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2438
    // Code size       0 (0x0)
  } // end of method HintTextConverter::Convert

  .method /*06000363*/ public hidebysig newslot virtual final 
          instance object[]  ConvertBack(object 'value',
                                         class [mscorlib/*23000001*/]System.Type/*01000003*/[] targetTypes,
                                         object parameter,
                                         class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1D 1C 1C 1D 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2439
    // Code size       0 (0x0)
  } // end of method HintTextConverter::ConvertBack

  .method /*06000364*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x243a
    // Code size       0 (0x0)
  } // end of method HintTextConverter::.ctor

} // end of class System.Activities.Core.Presentation.HintTextConverter

.class /*02000074*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.HintTextMaxWidthConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*06000365*/ public hidebysig newslot virtual final 
          instance object  Convert(object o,
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x243b
    // Code size       0 (0x0)
  } // end of method HintTextMaxWidthConverter::Convert

  .method /*06000366*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object o,
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x243c
    // Code size       0 (0x0)
  } // end of method HintTextMaxWidthConverter::ConvertBack

  .method /*06000367*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x243d
    // Code size       0 (0x0)
  } // end of method HintTextMaxWidthConverter::.ctor

} // end of class System.Activities.Core.Presentation.HintTextMaxWidthConverter

.class /*02000075*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.WriteLineDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*040000CC*/ assembly class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.ExpressionTextBox/*01000036*/ writeLineTextExpression
  .method /*06000368*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x243e
    // Code size       0 (0x0)
  } // end of method WriteLineDesigner::.ctor

  .method /*06000369*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x243f
    // Code size       0 (0x0)
  } // end of method WriteLineDesigner::RegisterMetadata

  .method /*0600036A*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000173:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000174:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2440
    // Code size       0 (0x0)
  } // end of method WriteLineDesigner::InitializeComponent

  .method /*0600036B*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000175:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000176:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000177:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2441
    // Code size       0 (0x0)
  } // end of method WriteLineDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.WriteLineDesigner

.class /*02000076*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.DelegateArgumentsValueEditor
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor/*0100002B*/
{
  .method /*0600036C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2442
    // Code size       0 (0x0)
  } // end of method DelegateArgumentsValueEditor::.ctor

  .method /*0600036D*/ public hidebysig virtual 
          instance void  ShowDialog(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.PropertyEditing.PropertyValue/*0100002C*/ propertyValue,
                                    class [PresentationCore/*23000009*/]System.Windows.IInputElement/*0100002D*/ commandSource) cil managed
  // SIG: 20 02 01 12 80 B1 12 80 B5
  {
    // Method begins at RVA 0x2443
    // Code size       0 (0x0)
  } // end of method DelegateArgumentsValueEditor::ShowDialog

} // end of class System.Activities.Core.Presentation.DelegateArgumentsValueEditor

.class /*02000077*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.DynamicActivityPropertyChooser
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.UserControl/*01000047*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*040000CD*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.ComboBox/*0100000D*/ comboBox
  .method /*0600036E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2444
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::.ctor

  .method /*0600036F*/ public hidebysig specialname 
          instance void  add_SelectedPropertyNameChanged(class System.Activities.Core.Presentation.SelectedPropertyNameChangedEventHandler/*02000082*/ 'value') cil managed
  // SIG: 20 01 01 12 82 08
  {
    .custom /*0C000178:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2445
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::add_SelectedPropertyNameChanged

  .method /*06000370*/ public hidebysig specialname 
          instance void  remove_SelectedPropertyNameChanged(class System.Activities.Core.Presentation.SelectedPropertyNameChangedEventHandler/*02000082*/ 'value') cil managed
  // SIG: 20 01 01 12 82 08
  {
    .custom /*0C000179:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2446
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::remove_SelectedPropertyNameChanged

  .method /*06000371*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/ 
          get_Model() cil managed
  // SIG: 20 00 12 81 E0
  {
    // Method begins at RVA 0x2447
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::get_Model

  .method /*06000372*/ public hidebysig specialname 
          instance void  set_Model(class System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/ 'value') cil managed
  // SIG: 20 01 01 12 81 E0
  {
    // Method begins at RVA 0x2448
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::set_Model

  .method /*06000373*/ public hidebysig specialname 
          instance void  set_Properties(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/ 'value') cil managed
  // SIG: 20 01 01 12 81 E9
  {
    // Method begins at RVA 0x2449
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::set_Properties

  .method /*06000374*/ public hidebysig specialname 
          instance void  set_Filter(class [mscorlib/*23000001*/]System.Predicate`1/*0100007E*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 F9 01 12 81 FD
  {
    // Method begins at RVA 0x244a
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::set_Filter

  .method /*06000375*/ public hidebysig specialname 
          instance string  get_SelectedPropertyName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x244b
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::get_SelectedPropertyName

  .method /*06000376*/ public hidebysig specialname 
          instance void  set_SelectedPropertyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x244c
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::set_SelectedPropertyName

  .method /*06000377*/ public hidebysig specialname 
          instance bool  get_IsUpdatingDropDownItems() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x244d
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::get_IsUpdatingDropDownItems

  .method /*06000378*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x244e
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::OnInitialized

  .method /*06000379*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00017A:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00017B:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x244f
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::InitializeComponent

  .method /*0600037A*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00017C:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00017D:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00017E:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2450
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooser::System.Windows.Markup.IComponentConnector.Connect

  .event /*14000009*/ System.Activities.Core.Presentation.SelectedPropertyNameChangedEventHandler/*02000082*/ SelectedPropertyNameChanged
  {
    .addon instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::add_SelectedPropertyNameChanged(class System.Activities.Core.Presentation.SelectedPropertyNameChangedEventHandler/*02000082*/) /* 0600036F */
    .removeon instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::remove_SelectedPropertyNameChanged(class System.Activities.Core.Presentation.SelectedPropertyNameChangedEventHandler/*02000082*/) /* 06000370 */
  } // end of event DynamicActivityPropertyChooser::SelectedPropertyNameChanged
  .property /*170000F5*/ instance callconv(8) class System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/
          Model()
  {
    .get instance class System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/ System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::get_Model() /* 06000371 */
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::set_Model(class System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/) /* 06000372 */
  } // end of property DynamicActivityPropertyChooser::Model
  .property /*170000F6*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/
          Properties()
  {
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::set_Properties(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/) /* 06000373 */
  } // end of property DynamicActivityPropertyChooser::Properties
  .property /*170000F7*/ instance callconv(8) class [mscorlib/*23000001*/]System.Predicate`1/*0100007E*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/>
          Filter()
  {
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::set_Filter(class [mscorlib/*23000001*/]System.Predicate`1/*0100007E*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/>) /* 06000374 */
  } // end of property DynamicActivityPropertyChooser::Filter
  .property /*170000F8*/ instance callconv(8) string
          SelectedPropertyName()
  {
    .get instance string System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::get_SelectedPropertyName() /* 06000375 */
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::set_SelectedPropertyName(string) /* 06000376 */
  } // end of property DynamicActivityPropertyChooser::SelectedPropertyName
  .property /*170000F9*/ instance callconv(8) bool
          IsUpdatingDropDownItems()
  {
    .get instance bool System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/::get_IsUpdatingDropDownItems() /* 06000377 */
  } // end of property DynamicActivityPropertyChooser::IsUpdatingDropDownItems
} // end of class System.Activities.Core.Presentation.DynamicActivityPropertyChooser

.class /*02000078*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ViewModel/*01000080*/
{
  .method /*0600037B*/ private hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 81 E9
  {
    // Method begins at RVA 0x2451
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::get_Properties

  .method /*0600037C*/ public hidebysig specialname 
          instance void  set_Properties(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/ 'value') cil managed
  // SIG: 20 01 01 12 81 E9
  {
    // Method begins at RVA 0x2452
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::set_Properties

  .method /*0600037D*/ private hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Predicate`1/*0100007E*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/> 
          get_Filter() cil managed
  // SIG: 20 00 15 12 81 F9 01 12 81 FD
  {
    // Method begins at RVA 0x2453
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::get_Filter

  .method /*0600037E*/ public hidebysig specialname 
          instance void  set_Filter(class [mscorlib/*23000001*/]System.Predicate`1/*0100007E*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 F9 01 12 81 FD
  {
    // Method begins at RVA 0x2454
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::set_Filter

  .method /*0600037F*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000081*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/> 
          get_DropDownItems() cil managed
  // SIG: 20 00 15 12 82 05 01 12 81 FD
  {
    // Method begins at RVA 0x2455
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::get_DropDownItems

  .method /*06000380*/ private hidebysig specialname 
          instance void  set_DropDownItems(class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000081*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/> 'value') cil managed
  // SIG: 20 01 01 15 12 82 05 01 12 81 FD
  {
    // Method begins at RVA 0x2456
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::set_DropDownItems

  .method /*06000381*/ public hidebysig specialname 
          instance bool  get_IsUpdatingDropDownItems() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00017F:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2457
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::get_IsUpdatingDropDownItems

  .method /*06000382*/ private hidebysig specialname 
          instance void  set_IsUpdatingDropDownItems(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000180:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2458
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::set_IsUpdatingDropDownItems

  .method /*06000383*/ public hidebysig specialname 
          instance string  get_SelectedPropertyName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2459
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::get_SelectedPropertyName

  .method /*06000384*/ public hidebysig specialname 
          instance void  set_SelectedPropertyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x245a
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::set_SelectedPropertyName

  .method /*06000385*/ public hidebysig instance void 
          UpdateDropDownItems() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x245b
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::UpdateDropDownItems

  .method /*06000386*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x245c
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyChooserViewModel::.ctor

  .property /*170000FA*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/
          Properties()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/ System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::get_Properties() /* 0600037B */
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::set_Properties(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItemCollection/*0100007A*/) /* 0600037C */
  } // end of property DynamicActivityPropertyChooserViewModel::Properties
  .property /*170000FB*/ instance callconv(8) class [mscorlib/*23000001*/]System.Predicate`1/*0100007E*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/>
          Filter()
  {
    .get instance class [mscorlib/*23000001*/]System.Predicate`1/*0100007E*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/> System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::get_Filter() /* 0600037D */
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::set_Filter(class [mscorlib/*23000001*/]System.Predicate`1/*0100007E*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/>) /* 0600037E */
  } // end of property DynamicActivityPropertyChooserViewModel::Filter
  .property /*170000FC*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000081*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/>
          DropDownItems()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000081*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/> System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::get_DropDownItems() /* 0600037F */
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::set_DropDownItems(class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000081*/<class [System.Activities/*2300000B*/]System.Activities.DynamicActivityProperty/*0100007F*/>) /* 06000380 */
  } // end of property DynamicActivityPropertyChooserViewModel::DropDownItems
  .property /*170000FD*/ instance callconv(8) bool
          IsUpdatingDropDownItems()
  {
    .get instance bool System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::get_IsUpdatingDropDownItems() /* 06000381 */
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::set_IsUpdatingDropDownItems(bool) /* 06000382 */
  } // end of property DynamicActivityPropertyChooserViewModel::IsUpdatingDropDownItems
  .property /*170000FE*/ instance callconv(8) string
          SelectedPropertyName()
  {
    .get instance string System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::get_SelectedPropertyName() /* 06000383 */
    .set instance void System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel/*02000078*/::set_SelectedPropertyName(string) /* 06000384 */
  } // end of property DynamicActivityPropertyChooserViewModel::SelectedPropertyName
} // end of class System.Activities.Core.Presentation.DynamicActivityPropertyChooserViewModel

.class /*02000079*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.DynamicActivityPropertyToTooltipConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*06000387*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x245d
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyToTooltipConverter::Convert

  .method /*06000388*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x245e
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyToTooltipConverter::ConvertBack

  .method /*06000389*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x245f
    // Code size       0 (0x0)
  } // end of method DynamicActivityPropertyToTooltipConverter::.ctor

} // end of class System.Activities.Core.Presentation.DynamicActivityPropertyToTooltipConverter

.class /*0200007A*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowSwitchDefaultLinkFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Feature/*01000082*/
{
  .method /*0600038A*/ public hidebysig virtual 
          instance void  Initialize(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context,
                                    class [mscorlib/*23000001*/]System.Type/*01000003*/ modelType) cil managed
  // SIG: 20 02 01 12 79 12 0D
  {
    // Method begins at RVA 0x2460
    // Code size       0 (0x0)
  } // end of method FlowSwitchDefaultLinkFeature::Initialize

  .method /*0600038B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2461
    // Code size       0 (0x0)
  } // end of method FlowSwitchDefaultLinkFeature::.ctor

} // end of class System.Activities.Core.Presentation.FlowSwitchDefaultLinkFeature

.class /*0200007B*/ private abstract auto ansi sealed beforefieldinit System.Activities.Core.Presentation.FreeFormPanelUtilities
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*0600038C*/ assembly hidebysig static 
          bool  IsRightToLeft(class [PresentationCore/*23000009*/]System.Windows.Media.Visual/*01000083*/ visual) cil managed
  // SIG: 00 01 02 12 82 0D
  {
    // Method begins at RVA 0x2462
    // Code size       0 (0x0)
  } // end of method FreeFormPanelUtilities::IsRightToLeft

} // end of class System.Activities.Core.Presentation.FreeFormPanelUtilities

.class /*0200007C*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.InvokeDelegateDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*040000CE*/ assembly class System.Activities.Core.Presentation.DynamicActivityPropertyChooser/*02000077*/ chooser
  .method /*0600038D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2463
    // Code size       0 (0x0)
  } // end of method InvokeDelegateDesigner::.ctor

  .method /*0600038E*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2464
    // Code size       0 (0x0)
  } // end of method InvokeDelegateDesigner::RegisterMetadata

  .method /*0600038F*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x2465
    // Code size       0 (0x0)
  } // end of method InvokeDelegateDesigner::OnInitialized

  .method /*06000390*/ family hidebysig virtual 
          instance void  OnReadOnlyChanged(bool isReadOnly) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2466
    // Code size       0 (0x0)
  } // end of method InvokeDelegateDesigner::OnReadOnlyChanged

  .method /*06000391*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000181:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000182:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2467
    // Code size       0 (0x0)
  } // end of method InvokeDelegateDesigner::InitializeComponent

  .method /*06000392*/ assembly hidebysig 
          instance class [mscorlib/*23000001*/]System.Delegate/*01000006*/ 
          _CreateDelegate(class [mscorlib/*23000001*/]System.Type/*01000003*/ delegateType,
                          string 'handler') cil managed
  // SIG: 20 02 12 19 12 0D 0E
  {
    .custom /*0C000183:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000184:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2468
    // Code size       0 (0x0)
  } // end of method InvokeDelegateDesigner::_CreateDelegate

  .method /*06000393*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000185:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000186:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000187:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2469
    // Code size       0 (0x0)
  } // end of method InvokeDelegateDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.InvokeDelegateDesigner

.class /*0200007D*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.InvokeDelegateValidationFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.DesignTimeValidationFeature/*0100003E*/
{
  .method /*06000394*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          get_ApplyTo() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x246a
    // Code size       0 (0x0)
  } // end of method InvokeDelegateValidationFeature::get_ApplyTo

  .method /*06000395*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/> 
          get_DesignTimeConstraints() cil managed
  // SIG: 20 00 15 12 65 01 12 80 FD
  {
    // Method begins at RVA 0x246b
    // Code size       0 (0x0)
  } // end of method InvokeDelegateValidationFeature::get_DesignTimeConstraints

  .method /*06000396*/ public hidebysig virtual 
          instance void  Initialize(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ context,
                                    class [mscorlib/*23000001*/]System.Type/*01000003*/ modelType) cil managed
  // SIG: 20 02 01 12 79 12 0D
  {
    // Method begins at RVA 0x246c
    // Code size       0 (0x0)
  } // end of method InvokeDelegateValidationFeature::Initialize

  .method /*06000397*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x246d
    // Code size       0 (0x0)
  } // end of method InvokeDelegateValidationFeature::.ctor

  .property /*170000FF*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000003*/
          ApplyTo()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000003*/ System.Activities.Core.Presentation.InvokeDelegateValidationFeature/*0200007D*/::get_ApplyTo() /* 06000394 */
  } // end of property InvokeDelegateValidationFeature::ApplyTo
  .property /*17000100*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/>
          DesignTimeConstraints()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000019*/<class [System.Activities/*2300000B*/]System.Activities.Validation.Constraint/*0100003F*/> System.Activities.Core.Presentation.InvokeDelegateValidationFeature/*0200007D*/::get_DesignTimeConstraints() /* 06000395 */
  } // end of property InvokeDelegateValidationFeature::DesignTimeConstraints
} // end of class System.Activities.Core.Presentation.InvokeDelegateValidationFeature

.class /*0200007E*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.FlowchartConnectionPointsAdorner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPointsAdorner/*01000084*/
{
  .method /*06000398*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ adornedElement,
                               class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> connectionPointsToShow,
                               bool isParentShapeSelected,
                               bool isTextRightToLeft) cil managed
  // SIG: 20 04 01 12 81 41 15 12 81 51 01 12 81 5D 02 02
  {
    // Method begins at RVA 0x246e
    // Code size       0 (0x0)
  } // end of method FlowchartConnectionPointsAdorner::.ctor

  .method /*06000399*/ family hidebysig virtual 
          instance void  OnRender(class [PresentationCore/*23000009*/]System.Windows.Media.DrawingContext/*0100007C*/ drawingContext) cil managed
  // SIG: 20 01 01 12 81 F1
  {
    // Method begins at RVA 0x246f
    // Code size       0 (0x0)
  } // end of method FlowchartConnectionPointsAdorner::OnRender

} // end of class System.Activities.Core.Presentation.FlowchartConnectionPointsAdorner

.class /*0200007F*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.FlowchartFreeFormPanel
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.FreeFormPanel/*01000051*/
{
  .method /*0600039A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2470
    // Code size       0 (0x0)
  } // end of method FlowchartFreeFormPanel::.ctor

} // end of class System.Activities.Core.Presentation.FlowchartFreeFormPanel

.class /*02000080*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.TransitionReorderChange
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.Change/*01000085*/
{
  .method /*0600039B*/ public hidebysig specialname virtual 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2471
    // Code size       0 (0x0)
  } // end of method TransitionReorderChange::get_Description

  .method /*0600039C*/ public hidebysig virtual 
          instance bool  Apply() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2472
    // Code size       0 (0x0)
  } // end of method TransitionReorderChange::Apply

  .method /*0600039D*/ public hidebysig virtual 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.Change/*01000085*/ 
          GetInverse() cil managed
  // SIG: 20 00 12 82 15
  {
    // Method begins at RVA 0x2473
    // Code size       0 (0x0)
  } // end of method TransitionReorderChange::GetInverse

  .method /*0600039E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2474
    // Code size       0 (0x0)
  } // end of method TransitionReorderChange::.ctor

  .property /*17000101*/ instance callconv(8) string
          Description()
  {
    .get instance string System.Activities.Core.Presentation.TransitionReorderChange/*02000080*/::get_Description() /* 0600039B */
  } // end of property TransitionReorderChange::Description
} // end of class System.Activities.Core.Presentation.TransitionReorderChange

.class /*02000081*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.NoPersistScopeDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .field /*040000CF*/ public static literal string BodyPropertyName = "Body"
  .method /*0600039F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2475
    // Code size       0 (0x0)
  } // end of method NoPersistScopeDesigner::.ctor

  .method /*060003A0*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2476
    // Code size       0 (0x0)
  } // end of method NoPersistScopeDesigner::RegisterMetadata

  .method /*060003A1*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000188:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000189:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2477
    // Code size       0 (0x0)
  } // end of method NoPersistScopeDesigner::InitializeComponent

  .method /*060003A2*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00018A:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00018B:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00018C:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2478
    // Code size       0 (0x0)
  } // end of method NoPersistScopeDesigner::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.NoPersistScopeDesigner

.class /*02000082*/ private auto ansi sealed System.Activities.Core.Presentation.SelectedPropertyNameChangedEventHandler
       extends [mscorlib/*23000001*/]System.MulticastDelegate/*01000074*/
{
  .method /*060003A3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method SelectedPropertyNameChangedEventHandler::.ctor

  .method /*060003A4*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Activities.Core.Presentation.SelectedPropertyNameChangedEventArgs/*02000083*/ e) runtime managed
  // SIG: 20 02 01 1C 12 82 0C
  {
  } // end of method SelectedPropertyNameChangedEventHandler::Invoke

  .method /*060003A5*/ public hidebysig newslot virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*01000075*/ 
          BeginInvoke(object sender,
                      class System.Activities.Core.Presentation.SelectedPropertyNameChangedEventArgs/*02000083*/ e,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*01000076*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 81 D5 1C 12 82 0C 12 81 D9 1C
  {
  } // end of method SelectedPropertyNameChangedEventHandler::BeginInvoke

  .method /*060003A6*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [mscorlib/*23000001*/]System.IAsyncResult/*01000075*/ result) runtime managed
  // SIG: 20 01 01 12 81 D5
  {
  } // end of method SelectedPropertyNameChangedEventHandler::EndInvoke

} // end of class System.Activities.Core.Presentation.SelectedPropertyNameChangedEventHandler

.class /*02000083*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.SelectedPropertyNameChangedEventArgs
       extends [mscorlib/*23000001*/]System.EventArgs/*0100000F*/
{
  .method /*060003A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string propertyName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2479
    // Code size       0 (0x0)
  } // end of method SelectedPropertyNameChangedEventArgs::.ctor

  .method /*060003A8*/ public hidebysig specialname 
          instance string  get_PropertyName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00018D:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x247a
    // Code size       0 (0x0)
  } // end of method SelectedPropertyNameChangedEventArgs::get_PropertyName

  .method /*060003A9*/ private hidebysig specialname 
          instance void  set_PropertyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00018E:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x247b
    // Code size       0 (0x0)
  } // end of method SelectedPropertyNameChangedEventArgs::set_PropertyName

  .property /*17000102*/ instance callconv(8) string
          PropertyName()
  {
    .get instance string System.Activities.Core.Presentation.SelectedPropertyNameChangedEventArgs/*02000083*/::get_PropertyName() /* 060003A8 */
    .set instance void System.Activities.Core.Presentation.SelectedPropertyNameChangedEventArgs/*02000083*/::set_PropertyName(string) /* 060003A9 */
  } // end of property SelectedPropertyNameChangedEventArgs::PropertyName
} // end of class System.Activities.Core.Presentation.SelectedPropertyNameChangedEventArgs

.class /*02000084*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.ReorderableListEditor
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.UserControl/*01000047*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*040000D0*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ItemTemplateProperty
  .field /*040000D1*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ListProperty
  .field /*040000D2*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ SelectedListItemProperty
  .field /*040000D3*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.ListBox/*01000086*/ listBox
  .method /*060003AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x247c
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::.ctor

  .method /*060003AB*/ public hidebysig specialname 
          instance class [PresentationFramework/*23000006*/]System.Windows.DataTemplate/*01000087*/ 
          get_ItemTemplate() cil managed
  // SIG: 20 00 12 82 1D
  {
    // Method begins at RVA 0x247d
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::get_ItemTemplate

  .method /*060003AC*/ public hidebysig specialname 
          instance void  set_ItemTemplate(class [PresentationFramework/*23000006*/]System.Windows.DataTemplate/*01000087*/ 'value') cil managed
  // SIG: 20 01 01 12 82 1D
  {
    // Method begins at RVA 0x247e
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::set_ItemTemplate

  .method /*060003AD*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/> 
          get_List() cil managed
  // SIG: 20 00 15 12 80 DD 01 12 82 1C
  {
    // Method begins at RVA 0x247f
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::get_List

  .method /*060003AE*/ public hidebysig specialname 
          instance void  set_List(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 DD 01 12 82 1C
  {
    // Method begins at RVA 0x2480
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::set_List

  .method /*060003AF*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/ 
          get_SelectedListItem() cil managed
  // SIG: 20 00 12 82 1C
  {
    // Method begins at RVA 0x2481
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::get_SelectedListItem

  .method /*060003B0*/ public hidebysig specialname 
          instance void  set_SelectedListItem(class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/ 'value') cil managed
  // SIG: 20 01 01 12 82 1C
  {
    // Method begins at RVA 0x2482
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::set_SelectedListItem

  .method /*060003B1*/ family hidebysig virtual 
          instance void  OnIsKeyboardFocusWithinChanged(valuetype [WindowsBase/*23000003*/]System.Windows.DependencyPropertyChangedEventArgs/*01000048*/ e) cil managed
  // SIG: 20 01 01 11 81 21
  {
    // Method begins at RVA 0x2483
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::OnIsKeyboardFocusWithinChanged

  .method /*060003B2*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00018F:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000190:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2484
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::InitializeComponent

  .method /*060003B3*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000191:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000192:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000193:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2485
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::System.Windows.Markup.IComponentConnector.Connect

  .method /*060003B4*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C000194:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000195:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C000196:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x2486
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::System.Windows.Markup.IStyleConnector.Connect

  .method /*060003B5*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2487
    // Code size       0 (0x0)
  } // end of method ReorderableListEditor::.cctor

  .property /*17000103*/ instance callconv(8) class [PresentationFramework/*23000006*/]System.Windows.DataTemplate/*01000087*/
          ItemTemplate()
  {
    .get instance class [PresentationFramework/*23000006*/]System.Windows.DataTemplate/*01000087*/ System.Activities.Core.Presentation.ReorderableListEditor/*02000084*/::get_ItemTemplate() /* 060003AB */
    .set instance void System.Activities.Core.Presentation.ReorderableListEditor/*02000084*/::set_ItemTemplate(class [PresentationFramework/*23000006*/]System.Windows.DataTemplate/*01000087*/) /* 060003AC */
  } // end of property ReorderableListEditor::ItemTemplate
  .property /*17000104*/ instance callconv(8) class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/>
          List()
  {
    .get instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/> System.Activities.Core.Presentation.ReorderableListEditor/*02000084*/::get_List() /* 060003AD */
    .set instance void System.Activities.Core.Presentation.ReorderableListEditor/*02000084*/::set_List(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/>) /* 060003AE */
  } // end of property ReorderableListEditor::List
  .property /*17000105*/ instance callconv(8) class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/
          SelectedListItem()
  {
    .get instance class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/ System.Activities.Core.Presentation.ReorderableListEditor/*02000084*/::get_SelectedListItem() /* 060003AF */
    .set instance void System.Activities.Core.Presentation.ReorderableListEditor/*02000084*/::set_SelectedListItem(class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/) /* 060003B0 */
  } // end of property ReorderableListEditor::SelectedListItem
} // end of class System.Activities.Core.Presentation.ReorderableListEditor

.class /*02000085*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.ArgumentIdentifierTrimConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*060003B6*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2488
    // Code size       0 (0x0)
  } // end of method ArgumentIdentifierTrimConverter::Convert

  .method /*060003B7*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2489
    // Code size       0 (0x0)
  } // end of method ArgumentIdentifierTrimConverter::ConvertBack

  .method /*060003B8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x248a
    // Code size       0 (0x0)
  } // end of method ArgumentIdentifierTrimConverter::.ctor

} // end of class System.Activities.Core.Presentation.ArgumentIdentifierTrimConverter

.class /*02000086*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.ExpandableItemShowExpandedMultiValueConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IMultiValueConverter/*01000079*/
{
  .method /*060003B9*/ public hidebysig newslot virtual final 
          instance object  Convert(object[] values,
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1D 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x248b
    // Code size       0 (0x0)
  } // end of method ExpandableItemShowExpandedMultiValueConverter::Convert

  .method /*060003BA*/ public hidebysig newslot virtual final 
          instance object[]  ConvertBack(object 'value',
                                         class [mscorlib/*23000001*/]System.Type/*01000003*/[] targetTypes,
                                         object parameter,
                                         class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1D 1C 1C 1D 12 0D 1C 12 11
  {
    // Method begins at RVA 0x248c
    // Code size       0 (0x0)
  } // end of method ExpandableItemShowExpandedMultiValueConverter::ConvertBack

  .method /*060003BB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x248d
    // Code size       0 (0x0)
  } // end of method ExpandableItemShowExpandedMultiValueConverter::.ctor

} // end of class System.Activities.Core.Presentation.ExpandableItemShowExpandedMultiValueConverter

.class /*02000087*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.ExpandableItemWrapper
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System/*23000002*/]System.ComponentModel.INotifyPropertyChanged/*01000031*/
{
  .method /*060003BC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x248e
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::.ctor

  .method /*060003BD*/ public hidebysig newslot specialname virtual final 
          instance void  add_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    .custom /*0C000197:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x248f
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::add_PropertyChanged

  .method /*060003BE*/ public hidebysig newslot specialname virtual final 
          instance void  remove_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    .custom /*0C000198:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2490
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::remove_PropertyChanged

  .method /*060003BF*/ public hidebysig specialname 
          instance bool  get_IsExpanded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2491
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::get_IsExpanded

  .method /*060003C0*/ public hidebysig specialname 
          instance void  set_IsExpanded(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2492
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::set_IsExpanded

  .method /*060003C1*/ public hidebysig specialname 
          instance bool  get_IsPinned() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2493
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::get_IsPinned

  .method /*060003C2*/ public hidebysig specialname 
          instance void  set_IsPinned(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2494
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::set_IsPinned

  .method /*060003C3*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_Item() cil managed
  // SIG: 20 00 12 2D
  {
    .custom /*0C000199:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2495
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::get_Item

  .method /*060003C4*/ public hidebysig specialname 
          instance void  set_Item(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    .custom /*0C00019A:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2496
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::set_Item

  .method /*060003C5*/ assembly hidebysig 
          instance void  SetPinState(bool isPinned) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2497
    // Code size       0 (0x0)
  } // end of method ExpandableItemWrapper::SetPinState

  .event /*1400000A*/ [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/ PropertyChanged
  {
    .addon instance void System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/::add_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/) /* 060003BD */
    .removeon instance void System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/::remove_PropertyChanged(class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000032*/) /* 060003BE */
  } // end of event ExpandableItemWrapper::PropertyChanged
  .property /*17000106*/ instance callconv(8) bool
          IsExpanded()
  {
    .get instance bool System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/::get_IsExpanded() /* 060003BF */
    .set instance void System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/::set_IsExpanded(bool) /* 060003C0 */
  } // end of property ExpandableItemWrapper::IsExpanded
  .property /*17000107*/ instance callconv(8) bool
          IsPinned()
  {
    .get instance bool System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/::get_IsPinned() /* 060003C1 */
    .set instance void System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/::set_IsPinned(bool) /* 060003C2 */
  } // end of property ExpandableItemWrapper::IsPinned
  .property /*17000108*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          Item()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/::get_Item() /* 060003C3 */
    .set instance void System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/::set_Item(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 060003C4 */
  } // end of property ExpandableItemWrapper::Item
} // end of class System.Activities.Core.Presentation.ExpandableItemWrapper

.class /*02000088*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.DisplayNameConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*060003C6*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2498
    // Code size       0 (0x0)
  } // end of method DisplayNameConverter::Convert

  .method /*060003C7*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x2499
    // Code size       0 (0x0)
  } // end of method DisplayNameConverter::ConvertBack

  .method /*060003C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x249a
    // Code size       0 (0x0)
  } // end of method DisplayNameConverter::.ctor

} // end of class System.Activities.Core.Presentation.DisplayNameConverter

.class /*02000089*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.IsLastSelectedItemConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IMultiValueConverter/*01000079*/
{
  .method /*060003C9*/ public hidebysig newslot virtual final 
          instance object  Convert(object[] values,
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1D 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x249b
    // Code size       0 (0x0)
  } // end of method IsLastSelectedItemConverter::Convert

  .method /*060003CA*/ public hidebysig newslot virtual final 
          instance object[]  ConvertBack(object 'value',
                                         class [mscorlib/*23000001*/]System.Type/*01000003*/[] targetTypes,
                                         object parameter,
                                         class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1D 1C 1C 1D 12 0D 1C 12 11
  {
    // Method begins at RVA 0x249c
    // Code size       0 (0x0)
  } // end of method IsLastSelectedItemConverter::ConvertBack

  .method /*060003CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x249d
    // Code size       0 (0x0)
  } // end of method IsLastSelectedItemConverter::.ctor

} // end of class System.Activities.Core.Presentation.IsLastSelectedItemConverter

.class /*0200008A*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.IsStateRootDesignerConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*060003CC*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x249e
    // Code size       0 (0x0)
  } // end of method IsStateRootDesignerConverter::Convert

  .method /*060003CD*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x249f
    // Code size       0 (0x0)
  } // end of method IsStateRootDesignerConverter::ConvertBack

  .method /*060003CE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24a0
    // Code size       0 (0x0)
  } // end of method IsStateRootDesignerConverter::.ctor

} // end of class System.Activities.Core.Presentation.IsStateRootDesignerConverter

.class /*0200008B*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StateConnectionPointToolTip
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.UserControl/*01000047*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*060003CF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24a1
    // Code size       0 (0x0)
  } // end of method StateConnectionPointToolTip::.ctor

  .method /*060003D0*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00019B:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00019C:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x24a2
    // Code size       0 (0x0)
  } // end of method StateConnectionPointToolTip::InitializeComponent

  .method /*060003D1*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C00019D:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00019E:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C00019F:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x24a3
    // Code size       0 (0x0)
  } // end of method StateConnectionPointToolTip::System.Windows.Markup.IComponentConnector.Connect

} // end of class System.Activities.Core.Presentation.StateConnectionPointToolTip

.class /*0200008C*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.StateLinkButtonToolTipConverter
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [PresentationFramework/*23000006*/]System.Windows.Data.IValueConverter/*01000011*/
{
  .method /*060003D2*/ public hidebysig newslot virtual final 
          instance object  Convert(object 'value',
                                   class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                   object parameter,
                                   class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x24a4
    // Code size       0 (0x0)
  } // end of method StateLinkButtonToolTipConverter::Convert

  .method /*060003D3*/ public hidebysig newslot virtual final 
          instance object  ConvertBack(object 'value',
                                       class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType,
                                       object parameter,
                                       class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*01000004*/ culture) cil managed
  // SIG: 20 04 1C 1C 12 0D 1C 12 11
  {
    // Method begins at RVA 0x24a5
    // Code size       0 (0x0)
  } // end of method StateLinkButtonToolTipConverter::ConvertBack

  .method /*060003D4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24a6
    // Code size       0 (0x0)
  } // end of method StateLinkButtonToolTipConverter::.ctor

} // end of class System.Activities.Core.Presentation.StateLinkButtonToolTipConverter

.class /*0200008D*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StateContainerResizeGrip
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.Control/*01000064*/
{
  .field /*040000D4*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ IconProperty
  .field /*040000D5*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ParentStateContainerEditorProperty
  .field /*040000D6*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ DisabledProperty
  .method /*060003D5*/ public hidebysig specialname 
          instance class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/ 
          get_Icon() cil managed
  // SIG: 20 00 12 81 95
  {
    // Method begins at RVA 0x24a7
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::get_Icon

  .method /*060003D6*/ public hidebysig specialname 
          instance void  set_Icon(class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/ 'value') cil managed
  // SIG: 20 01 01 12 81 95
  {
    // Method begins at RVA 0x24a8
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::set_Icon

  .method /*060003D7*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/ 
          get_ParentStateContainerEditor() cil managed
  // SIG: 20 00 12 82 3C
  {
    // Method begins at RVA 0x24a9
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::get_ParentStateContainerEditor

  .method /*060003D8*/ public hidebysig specialname 
          instance void  set_ParentStateContainerEditor(class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/ 'value') cil managed
  // SIG: 20 01 01 12 82 3C
  {
    // Method begins at RVA 0x24aa
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::set_ParentStateContainerEditor

  .method /*060003D9*/ public hidebysig specialname 
          instance bool  get_Disabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24ab
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::get_Disabled

  .method /*060003DA*/ public hidebysig specialname 
          instance void  set_Disabled(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24ac
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::set_Disabled

  .method /*060003DB*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x24ad
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::OnInitialized

  .method /*060003DC*/ family hidebysig virtual 
          instance void  OnGotMouseCapture(class [PresentationCore/*23000009*/]System.Windows.Input.MouseEventArgs/*01000067*/ e) cil managed
  // SIG: 20 01 01 12 81 9D
  {
    // Method begins at RVA 0x24ae
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::OnGotMouseCapture

  .method /*060003DD*/ family hidebysig virtual 
          instance void  OnLostMouseCapture(class [PresentationCore/*23000009*/]System.Windows.Input.MouseEventArgs/*01000067*/ e) cil managed
  // SIG: 20 01 01 12 81 9D
  {
    // Method begins at RVA 0x24af
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::OnLostMouseCapture

  .method /*060003DE*/ family hidebysig virtual 
          instance void  OnPreviewMouseLeftButtonDown(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x24b0
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::OnPreviewMouseLeftButtonDown

  .method /*060003DF*/ family hidebysig virtual 
          instance void  OnMouseMove(class [PresentationCore/*23000009*/]System.Windows.Input.MouseEventArgs/*01000067*/ args) cil managed
  // SIG: 20 01 01 12 81 9D
  {
    // Method begins at RVA 0x24b1
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::OnMouseMove

  .method /*060003E0*/ family hidebysig virtual 
          instance void  OnPreviewMouseLeftButtonUp(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x24b2
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::OnPreviewMouseLeftButtonUp

  .method /*060003E1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24b3
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::.ctor

  .method /*060003E2*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x24b4
    // Code size       0 (0x0)
  } // end of method StateContainerResizeGrip::.cctor

  .property /*17000109*/ instance callconv(8) class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/
          Icon()
  {
    .get instance class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/ System.Activities.Core.Presentation.StateContainerResizeGrip/*0200008D*/::get_Icon() /* 060003D5 */
    .set instance void System.Activities.Core.Presentation.StateContainerResizeGrip/*0200008D*/::set_Icon(class [PresentationCore/*23000009*/]System.Windows.Media.DrawingBrush/*01000065*/) /* 060003D6 */
  } // end of property StateContainerResizeGrip::Icon
  .property /*1700010A*/ instance callconv(8) class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/
          ParentStateContainerEditor()
  {
    .get instance class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/ System.Activities.Core.Presentation.StateContainerResizeGrip/*0200008D*/::get_ParentStateContainerEditor() /* 060003D7 */
    .set instance void System.Activities.Core.Presentation.StateContainerResizeGrip/*0200008D*/::set_ParentStateContainerEditor(class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/) /* 060003D8 */
  } // end of property StateContainerResizeGrip::ParentStateContainerEditor
  .property /*1700010B*/ instance callconv(8) bool
          Disabled()
  {
    .get instance bool System.Activities.Core.Presentation.StateContainerResizeGrip/*0200008D*/::get_Disabled() /* 060003D9 */
    .set instance void System.Activities.Core.Presentation.StateContainerResizeGrip/*0200008D*/::set_Disabled(bool) /* 060003DA */
  } // end of property StateContainerResizeGrip::Disabled
} // end of class System.Activities.Core.Presentation.StateContainerResizeGrip

.class /*0200008E*/ public auto ansi sealed beforefieldinit System.Activities.Core.Presentation.FinalState
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*060003E3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24b5
    // Code size       0 (0x0)
  } // end of method FinalState::.ctor

} // end of class System.Activities.Core.Presentation.FinalState

.class /*0200008F*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StateContainerEditor
       extends [PresentationFramework/*23000006*/]System.Windows.Controls.UserControl/*01000047*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IMultipleDragEnabledCompositeView/*0100004B*/,
                  [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ICompositeView/*0100004C*/,
                  [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.IAutoConnectContainer/*0100004D*/,
                  [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.IAutoSplitContainer/*0100004E*/,
                  [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .class /*020000A6*/ auto ansi sealed nested assembly ConnectorCreationResult
         extends [mscorlib/*23000001*/]System.Enum/*0100006D*/
  {
    .field /*04000105*/ public specialname rtspecialname int32 value__
    .field /*04000106*/ public static literal valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ Success = int32(0x00000000)
    .field /*04000107*/ public static literal valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ CannotCreateTransitionToCompositeState = int32(0x00000001)
    .field /*04000108*/ public static literal valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ CannotCreateTransitionFromAncestorToDescendant = int32(0x00000002)
    .field /*04000109*/ public static literal valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ CannotSetCompositeStateAsInitialState = int32(0x00000003)
    .field /*0400010A*/ public static literal valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ CannotSetFinalStateAsInitialState = int32(0x00000004)
    .field /*0400010B*/ public static literal valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ OtherFailure = int32(0x00000005)
  } // end of class ConnectorCreationResult

  .field /*040000D7*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ DroppingTypeResolvingOptionsProperty
  .field /*040000D8*/ static assembly literal string ConnectorLocationViewStateKey = "ConnectorLocation"
  .field /*040000D9*/ static assembly literal string SrcConnectionPointIndexStateKey = "SrcConnectionPointIndex"
  .field /*040000DA*/ static assembly literal string DestConnectionPointIndexStateKey = "DestConnectionPointIndex"
  .field /*040000DB*/ static assembly literal string StateContainerWidthViewStateKey = "StateContainerWidth"
  .field /*040000DC*/ static assembly literal string StateContainerHeightViewStateKey = "StateContainerHeight"
  .field /*040000DD*/ static assembly literal string DefaultStateDisplayName = "State"
  .field /*040000DE*/ static assembly literal string DefaultFinalStateDisplayName = "FinalState"
  .field /*040000DF*/ static assembly literal int32 ConnectionPointNum = int32(0x00000013)
  .field /*040000E0*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ StateContainerWidthProperty
  .field /*040000E1*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ StateContainerHeightProperty
  .field /*040000E2*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ PanelMinWidthProperty
  .field /*040000E3*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ PanelMinHeightProperty
  .field /*040000E4*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ConnectorModelItemProperty
  .field /*040000E5*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ConnectionPointsProperty
  .field /*040000E6*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ ModelItemProperty
  .field /*040000E7*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ IsReadOnlyProperty
  .field /*040000E8*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ IsStateMachineContainerProperty
  .field /*040000E9*/ assembly class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/ stateContainerEditor
  .field /*040000EA*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.Grid/*01000029*/ stateContainerGrid
  .field /*040000EB*/ assembly class System.Activities.Core.Presentation.StateMachineFreeFormPanel/*02000096*/ panel
  .method /*060003E4*/ public hidebysig newslot specialname virtual final 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/ 
          get_DroppingTypeResolvingOptions() cil managed
  // SIG: 20 00 12 81 4D
  {
    // Method begins at RVA 0x24b6
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_DroppingTypeResolvingOptions

  .method /*060003E5*/ public hidebysig specialname 
          instance void  set_DroppingTypeResolvingOptions(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/ 'value') cil managed
  // SIG: 20 01 01 12 81 4D
  {
    // Method begins at RVA 0x24b7
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_DroppingTypeResolvingOptions

  .method /*060003E6*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsDefaultContainer() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24b8
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_IsDefaultContainer

  .method /*060003E7*/ public hidebysig newslot virtual final 
          instance void  OnItemMoved(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem) cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x24b9
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::OnItemMoved

  .method /*060003E8*/ public hidebysig newslot virtual final 
          instance object  OnItemsCopied(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> itemsToCopy) cil managed
  // SIG: 20 01 1C 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x24ba
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::OnItemsCopied

  .method /*060003E9*/ public hidebysig newslot virtual final 
          instance object  OnItemsCut(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> itemsToCut) cil managed
  // SIG: 20 01 1C 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x24bb
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::OnItemsCut

  .method /*060003EA*/ public hidebysig newslot virtual final 
          instance void  OnItemsDelete(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> itemsToDelete) cil managed
  // SIG: 20 01 01 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x24bc
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::OnItemsDelete

  .method /*060003EB*/ public hidebysig newslot virtual final 
          instance bool  CanPasteItems(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> itemsToPaste) cil managed
  // SIG: 20 01 02 15 12 81 51 01 1C
  {
    // Method begins at RVA 0x24bd
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::CanPasteItems

  .method /*060003EC*/ assembly hidebysig 
          instance bool  CanPasteTransition(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ destinationStateItem,
                                            [out] string& errorMessage,
                                            class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/[] sourceStateItems) cil managed
  // SIG: 20 03 02 12 2D 10 0E 1D 12 2D
  {
    .param [3]/*08000339*/ 
    .custom /*0C000170:0A00002C*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*010000AE*/::.ctor() /* 0A00002C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24be
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::CanPasteTransition

  .method /*060003ED*/ public hidebysig newslot virtual final 
          instance void  OnItemsPasted(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> itemsToPaste,
                                       class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> metaData,
                                       valuetype [WindowsBase/*23000003*/]System.Windows.Point/*01000056*/ pastePoint,
                                       class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.WorkflowViewElement/*01000008*/ pastePointReference) cil managed
  // SIG: 20 04 01 15 12 81 51 01 1C 15 12 81 51 01 1C 11 81 59 12 21
  {
    // Method begins at RVA 0x24bf
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::OnItemsPasted

  .method /*060003EE*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> 
          SortSelectedItems(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> selectedItems) cil managed
  // SIG: 20 01 15 12 81 51 01 12 2D 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x24c0
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::SortSelectedItems

  .method /*060003EF*/ public hidebysig newslot virtual final 
          instance void  OnItemsMoved(class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/> movedItems) cil managed
  // SIG: 20 01 01 15 12 81 51 01 12 2D
  {
    // Method begins at RVA 0x24c1
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::OnItemsMoved

  .method /*060003F0*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          GetConnectorModelItem(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ obj) cil managed
  // SIG: 00 01 12 2D 12 6D
  {
    // Method begins at RVA 0x24c2
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetConnectorModelItem

  .method /*060003F1*/ assembly hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> 
          GetConnectionPoints(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ obj) cil managed
  // SIG: 00 01 15 12 81 51 01 12 81 5D 12 6D
  {
    // Method begins at RVA 0x24c3
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetConnectionPoints

  .method /*060003F2*/ assembly hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/> 
          GetAttachedConnectors(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ shape) cil managed
  // SIG: 00 01 15 12 81 51 01 12 81 61 12 81 41
  {
    // Method begins at RVA 0x24c4
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetAttachedConnectors

  .method /*060003F3*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          GetStateMachineModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem) cil managed
  // SIG: 00 01 12 2D 12 2D
  {
    // Method begins at RVA 0x24c5
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetStateMachineModelItem

  .method /*060003F4*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          GetParentStateModelItemForTransition(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ transitionModelItem) cil managed
  // SIG: 00 01 12 2D 12 2D
  {
    // Method begins at RVA 0x24c6
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetParentStateModelItemForTransition

  .method /*060003F5*/ assembly hidebysig static 
          class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ 
          GetStateView(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ stateModelItem) cil managed
  // SIG: 00 01 12 81 41 12 2D
  {
    // Method begins at RVA 0x24c7
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetStateView

  .method /*060003F6*/ assembly hidebysig static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ 
          GetClosestConnectionPoint(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ srcConnPoint,
                                    class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> destConnPoints,
                                    [out] float64& minDist) cil managed
  // SIG: 00 03 12 81 5D 12 81 5D 15 12 81 51 01 12 81 5D 10 0D
  {
    // Method begins at RVA 0x24c8
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetClosestConnectionPoint

  .method /*060003F7*/ assembly hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> 
          GetEmptyConnectionPoints(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ designer) cil managed
  // SIG: 00 01 15 12 81 51 01 12 81 5D 12 81 41
  {
    // Method begins at RVA 0x24c9
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetEmptyConnectionPoints

  .method /*060003F8*/ assembly hidebysig static 
          bool  IsFinalState(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem) cil managed
  // SIG: 00 01 02 12 2D
  {
    // Method begins at RVA 0x24ca
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::IsFinalState

  .method /*060003F9*/ assembly hidebysig static 
          string  GenerateTransitionName(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ stateMachineModelItem) cil managed
  // SIG: 00 01 0E 12 2D
  {
    // Method begins at RVA 0x24cb
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GenerateTransitionName

  .method /*060003FA*/ assembly hidebysig 
          instance int32  DeleteConnectorModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/ connector,
                                                   [opt] bool rerouting) cil managed
  // SIG: 20 02 08 12 81 61 02
  {
    .param [2]/*0800034D*/  = bool(false)
    // Method begins at RVA 0x24cc
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::DeleteConnectorModelItem

  .method /*060003FB*/ assembly hidebysig 
          instance valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ 
          CreateConnectorGesture(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ sourceConnectionPoint,
                                 class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ destConnectionPoint,
                                 class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/ referenceConnector,
                                 bool isConnectorStartMoved) cil managed
  // SIG: 20 04 11 82 98 12 81 5D 12 81 5D 12 81 61 02
  {
    // Method begins at RVA 0x24cd
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::CreateConnectorGesture

  .method /*060003FC*/ assembly hidebysig 
          instance valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ 
          CreateConnectorGesture(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ sourceConnectionPoint,
                                 class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ dest,
                                 class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/ referenceConnector,
                                 bool isConnectorStartMoved) cil managed
  // SIG: 20 04 11 82 98 12 81 5D 12 81 41 12 81 61 02
  {
    // Method begins at RVA 0x24ce
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::CreateConnectorGesture

  .method /*060003FD*/ assembly hidebysig 
          instance valuetype System.Activities.Core.Presentation.StateContainerEditor/*0200008F*//ConnectorCreationResult/*020000A6*/ 
          CreateConnectorGesture(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ source,
                                 class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/ destConnectionPoint,
                                 class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/ referenceConnector,
                                 bool isConnectorStartMoved) cil managed
  // SIG: 20 04 11 82 98 12 81 41 12 81 5D 12 81 61 02
  {
    // Method begins at RVA 0x24cf
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::CreateConnectorGesture

  .method /*060003FE*/ assembly hidebysig 
          instance void  StoreShapeSizeWithUndoRecursively(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ modelItem) cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x24d0
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::StoreShapeSizeWithUndoRecursively

  .method /*060003FF*/ assembly hidebysig specialname static 
          class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_CopiedTransitionDestinationState() cil managed
  // SIG: 00 00 12 2D
  {
    .custom /*0C0001A0:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24d1
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_CopiedTransitionDestinationState

  .method /*06000400*/ assembly hidebysig specialname static 
          void  set_CopiedTransitionDestinationState(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 00 01 01 12 2D
  {
    .custom /*0C0001A1:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24d2
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_CopiedTransitionDestinationState

  .method /*06000401*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24d3
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::.ctor

  .method /*06000402*/ assembly hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.FreeFormPanel/*01000051*/ 
          get_Panel() cil managed
  // SIG: 20 00 12 81 45
  {
    // Method begins at RVA 0x24d4
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_Panel

  .method /*06000403*/ public hidebysig specialname 
          instance float64  get_StateContainerWidth() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x24d5
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_StateContainerWidth

  .method /*06000404*/ public hidebysig specialname 
          instance void  set_StateContainerWidth(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    // Method begins at RVA 0x24d6
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_StateContainerWidth

  .method /*06000405*/ public hidebysig specialname 
          instance float64  get_StateContainerHeight() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x24d7
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_StateContainerHeight

  .method /*06000406*/ public hidebysig specialname 
          instance void  set_StateContainerHeight(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    // Method begins at RVA 0x24d8
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_StateContainerHeight

  .method /*06000407*/ public hidebysig specialname 
          instance float64  get_PanelMinWidth() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x24d9
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_PanelMinWidth

  .method /*06000408*/ public hidebysig specialname 
          instance void  set_PanelMinWidth(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    // Method begins at RVA 0x24da
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_PanelMinWidth

  .method /*06000409*/ public hidebysig specialname 
          instance float64  get_PanelMinHeight() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x24db
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_PanelMinHeight

  .method /*0600040A*/ public hidebysig specialname 
          instance void  set_PanelMinHeight(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    // Method begins at RVA 0x24dc
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_PanelMinHeight

  .method /*0600040B*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 
          get_ModelItem() cil managed
  // SIG: 20 00 12 2D
  {
    // Method begins at RVA 0x24dd
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_ModelItem

  .method /*0600040C*/ public hidebysig specialname 
          instance void  set_ModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2D
  {
    // Method begins at RVA 0x24de
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_ModelItem

  .method /*0600040D*/ family hidebysig specialname 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24df
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_IsReadOnly

  .method /*0600040E*/ private hidebysig specialname 
          instance void  set_IsReadOnly(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24e0
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_IsReadOnly

  .method /*0600040F*/ family hidebysig specialname 
          instance bool  get_IsStateMachineContainer() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24e1
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_IsStateMachineContainer

  .method /*06000410*/ private hidebysig specialname 
          instance void  set_IsStateMachineContainer(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24e2
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_IsStateMachineContainer

  .method /*06000411*/ public hidebysig specialname 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 
          get_Context() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C0001A2:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24e3
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_Context

  .method /*06000412*/ public hidebysig specialname 
          instance void  set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C0001A3:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24e4
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::set_Context

  .method /*06000413*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x24e5
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::OnInitialized

  .method /*06000414*/ private hidebysig specialname 
          instance bool  get_IsRightToLeft() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24e6
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::get_IsRightToLeft

  .method /*06000415*/ assembly hidebysig 
          instance class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/ 
          GetStateMachineContainerEditor() cil managed
  // SIG: 20 00 12 82 3C
  {
    // Method begins at RVA 0x24e7
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetStateMachineContainerEditor

  .method /*06000416*/ public hidebysig instance void 
          StoreAttachedConnectorViewStates(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ element) cil managed
  // SIG: 20 01 01 12 81 41
  {
    // Method begins at RVA 0x24e8
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::StoreAttachedConnectorViewStates

  .method /*06000417*/ public hidebysig newslot virtual final 
          instance void  DoAutoConnect(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e,
                                       class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ targetElement,
                                       valuetype [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.AutoConnectDirections/*0100005E*/ direction) cil managed
  // SIG: 20 03 01 12 81 75 12 81 41 11 81 79
  {
    // Method begins at RVA 0x24e9
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::DoAutoConnect

  .method /*06000418*/ public hidebysig newslot virtual final 
          instance valuetype [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.AutoConnectDirections/*0100005E*/ 
          GetDirectionsAllowed(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e,
                               class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ targetElement) cil managed
  // SIG: 20 02 11 81 79 12 81 75 12 81 41
  {
    // Method begins at RVA 0x24ea
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::GetDirectionsAllowed

  .method /*06000419*/ public hidebysig newslot virtual final 
          instance bool  CanAutoSplit(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e) cil managed
  // SIG: 20 01 02 12 81 75
  {
    // Method begins at RVA 0x24eb
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::CanAutoSplit

  .method /*0600041A*/ public hidebysig newslot virtual final 
          instance void  DoAutoSplit(class [PresentationCore/*23000009*/]System.Windows.DragEventArgs/*0100005D*/ e,
                                     class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.Connector/*01000058*/ connector) cil managed
  // SIG: 20 02 01 12 81 75 12 81 61
  {
    // Method begins at RVA 0x24ec
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::DoAutoSplit

  .method /*0600041B*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001A4:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001A5:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x24ed
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::InitializeComponent

  .method /*0600041C*/ assembly hidebysig 
          instance class [mscorlib/*23000001*/]System.Delegate/*01000006*/ 
          _CreateDelegate(class [mscorlib/*23000001*/]System.Type/*01000003*/ delegateType,
                          string 'handler') cil managed
  // SIG: 20 02 12 19 12 0D 0E
  {
    .custom /*0C0001A6:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001A7:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x24ee
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::_CreateDelegate

  .method /*0600041D*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001A8:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001A9:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001AA:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x24ef
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600041E*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x24f0
    // Code size       0 (0x0)
  } // end of method StateContainerEditor::.cctor

  .property /*1700010C*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/
          DroppingTypeResolvingOptions()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/ System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_DroppingTypeResolvingOptions() /* 060003E4 */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_DroppingTypeResolvingOptions(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.View.TypeResolvingOptions/*01000053*/) /* 060003E5 */
  } // end of property StateContainerEditor::DroppingTypeResolvingOptions
  .property /*1700010D*/ instance callconv(8) bool
          IsDefaultContainer()
  {
    .get instance bool System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_IsDefaultContainer() /* 060003E6 */
  } // end of property StateContainerEditor::IsDefaultContainer
  .property /*1700010E*/ callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          CopiedTransitionDestinationState()
  {
    .get class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_CopiedTransitionDestinationState() /* 060003FF */
    .set void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_CopiedTransitionDestinationState(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 06000400 */
  } // end of property StateContainerEditor::CopiedTransitionDestinationState
  .property /*1700010F*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.FreeFormPanel/*01000051*/
          Panel()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.FreeFormPanel/*01000051*/ System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_Panel() /* 06000402 */
  } // end of property StateContainerEditor::Panel
  .property /*17000110*/ instance callconv(8) float64
          StateContainerWidth()
  {
    .get instance float64 System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_StateContainerWidth() /* 06000403 */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_StateContainerWidth(float64) /* 06000404 */
  } // end of property StateContainerEditor::StateContainerWidth
  .property /*17000111*/ instance callconv(8) float64
          StateContainerHeight()
  {
    .get instance float64 System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_StateContainerHeight() /* 06000405 */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_StateContainerHeight(float64) /* 06000406 */
  } // end of property StateContainerEditor::StateContainerHeight
  .property /*17000112*/ instance callconv(8) float64
          PanelMinWidth()
  {
    .get instance float64 System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_PanelMinWidth() /* 06000407 */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_PanelMinWidth(float64) /* 06000408 */
  } // end of property StateContainerEditor::PanelMinWidth
  .property /*17000113*/ instance callconv(8) float64
          PanelMinHeight()
  {
    .get instance float64 System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_PanelMinHeight() /* 06000409 */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_PanelMinHeight(float64) /* 0600040A */
  } // end of property StateContainerEditor::PanelMinHeight
  .property /*17000114*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/
          ModelItem()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/ System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_ModelItem() /* 0600040B */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_ModelItem(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Model.ModelItem/*0100000B*/) /* 0600040C */
  } // end of property StateContainerEditor::ModelItem
  .property /*17000115*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_IsReadOnly() /* 0600040D */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_IsReadOnly(bool) /* 0600040E */
  } // end of property StateContainerEditor::IsReadOnly
  .property /*17000116*/ instance callconv(8) bool
          IsStateMachineContainer()
  {
    .get instance bool System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_IsStateMachineContainer() /* 0600040F */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_IsStateMachineContainer(bool) /* 06000410 */
  } // end of property StateContainerEditor::IsStateMachineContainer
  .property /*17000117*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/
          Context()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/ System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_Context() /* 06000411 */
    .set instance void System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::set_Context(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.EditingContext/*0100001E*/) /* 06000412 */
  } // end of property StateContainerEditor::Context
  .property /*17000118*/ instance callconv(8) bool
          IsRightToLeft()
  {
    .get instance bool System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/::get_IsRightToLeft() /* 06000414 */
  } // end of property StateContainerEditor::IsRightToLeft
} // end of class System.Activities.Core.Presentation.StateContainerEditor

.class /*02000090*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StateDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*040000EC*/ static assembly literal string EntryPropertyName = "Entry"
  .field /*040000ED*/ static assembly literal string ExitPropertyName = "Exit"
  .field /*040000EE*/ static assembly literal string IsFinalPropertyName = "IsFinal"
  .field /*040000EF*/ static assembly literal string DisplayNamePropertyName = "DisplayName"
  .field /*040000F0*/ static assembly literal string TransitionsPropertyName = "Transitions"
  .field /*040000F1*/ static assembly literal string ChildStatesPropertyName = "States"
  .field /*040000F2*/ static assembly literal string VariablesPropertyName = "Variables"
  .field /*040000F3*/ public static initonly class [PresentationCore/*23000009*/]System.Windows.Input.RoutedCommand/*01000028*/ SetAsInitialCommand
  .field /*040000F4*/ assembly class System.Activities.Core.Presentation.StateDesigner/*02000090*/ stateDesigner
  .field /*040000F5*/ assembly class [PresentationFramework/*23000006*/]System.Windows.Controls.ContentPresenter/*01000052*/ stateContentPresenter
  .method /*0600041F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24f1
    // Code size       0 (0x0)
  } // end of method StateDesigner::.ctor

  .method /*06000420*/ family hidebysig virtual 
          instance void  OnInitialized(class [mscorlib/*23000001*/]System.EventArgs/*0100000F*/ e) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x24f2
    // Code size       0 (0x0)
  } // end of method StateDesigner::OnInitialized

  .method /*06000421*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x24f4
    // Code size       0 (0x0)
  } // end of method StateDesigner::RegisterMetadata

  .method /*06000422*/ family hidebysig virtual 
          instance void  OnShowExpandedChanged(bool newValue) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2500
    // Code size       0 (0x0)
  } // end of method StateDesigner::OnShowExpandedChanged

  .method /*06000423*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2501
    // Code size       0 (0x0)
  } // end of method StateDesigner::OnModelItemChanged

  .method /*06000424*/ famorassem hidebysig virtual 
          instance string  GetAutomationItemStatus() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2502
    // Code size       0 (0x0)
  } // end of method StateDesigner::GetAutomationItemStatus

  .method /*06000425*/ assembly hidebysig 
          instance bool  IsFinalState() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2503
    // Code size       0 (0x0)
  } // end of method StateDesigner::IsFinalState

  .method /*06000426*/ public hidebysig instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.FreeFormPanel/*01000051*/ 
          GetStateMachineFreeFormPanel() cil managed
  // SIG: 20 00 12 81 45
  {
    // Method begins at RVA 0x2504
    // Code size       0 (0x0)
  } // end of method StateDesigner::GetStateMachineFreeFormPanel

  .method /*06000427*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001AB:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001AC:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2505
    // Code size       0 (0x0)
  } // end of method StateDesigner::InitializeComponent

  .method /*06000428*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001AD:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001AE:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001AF:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2506
    // Code size       0 (0x0)
  } // end of method StateDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*06000429*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001B0:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001B1:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001B2:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x2507
    // Code size       0 (0x0)
  } // end of method StateDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*0600042A*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2508
    // Code size       0 (0x0)
  } // end of method StateDesigner::.cctor

} // end of class System.Activities.Core.Presentation.StateDesigner

.class /*02000091*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StateMachineDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .custom /*0C000065:0A00002A*/ instance void [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesignerOptionsAttribute/*010000AC*/::.ctor() /* 0A00002A */ = ( 01 00 01 00 54 02 16 41 6C 77 61 79 73 43 6F 6C   // ....T..AlwaysCol
                                                                                                                                                                                                  6C 61 70 73 65 43 68 69 6C 64 72 65 6E 01 )       // lapseChildren.
  .field /*040000F6*/ static assembly literal string InitialStatePropertyName = "InitialState"
  .field /*040000F7*/ static assembly literal string VariablesPropertyName = "Variables"
  .field /*040000F8*/ static assembly literal string StatesPropertyName = "States"
  .method /*0600042B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2509
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::.ctor

  .method /*0600042C*/ assembly hidebysig specialname 
          instance bool  get_IsResizing() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001B3:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x250a
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::get_IsResizing

  .method /*0600042D*/ assembly hidebysig specialname 
          instance void  set_IsResizing(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001B4:0A000027*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*010000A9*/::.ctor() /* 0A000027 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x250b
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::set_IsResizing

  .method /*0600042E*/ assembly hidebysig specialname 
          instance class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/ 
          get_StateContainerEditor() cil managed
  // SIG: 20 00 12 82 3C
  {
    // Method begins at RVA 0x250c
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::get_StateContainerEditor

  .method /*0600042F*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2510
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::RegisterMetadata

  .method /*06000430*/ family hidebysig virtual 
          instance void  OnModelItemChanged(object newItem) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x251c
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::OnModelItemChanged

  .method /*06000431*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001B5:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001B6:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x251d
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::InitializeComponent

  .method /*06000432*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001B7:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001B8:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001B9:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x251e
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*06000433*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001BA:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001BB:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001BC:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x251f
    // Code size       0 (0x0)
  } // end of method StateMachineDesigner::System.Windows.Markup.IStyleConnector.Connect

  .property /*17000119*/ instance callconv(8) bool
          IsResizing()
  {
    .get instance bool System.Activities.Core.Presentation.StateMachineDesigner/*02000091*/::get_IsResizing() /* 0600042C */
    .set instance void System.Activities.Core.Presentation.StateMachineDesigner/*02000091*/::set_IsResizing(bool) /* 0600042D */
  } // end of property StateMachineDesigner::IsResizing
  .property /*1700011A*/ instance callconv(8) class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/
          StateContainerEditor()
  {
    .get instance class System.Activities.Core.Presentation.StateContainerEditor/*0200008F*/ System.Activities.Core.Presentation.StateMachineDesigner/*02000091*/::get_StateContainerEditor() /* 0600042E */
  } // end of property StateMachineDesigner::StateContainerEditor
} // end of class System.Activities.Core.Presentation.StateMachineDesigner

.class /*02000092*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.TransitionDesigner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ActivityDesigner/*01000027*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/,
                  [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/
{
  .field /*040000F9*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ NewTransitionProperty
  .field /*040000FA*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ TransitionsSharingTriggerProperty
  .field /*040000FB*/ public static initonly class [WindowsBase/*23000003*/]System.Windows.DependencyProperty/*0100000E*/ SelectedTransitionProperty
  .field /*040000FC*/ static assembly literal string TriggerPropertyName = "Trigger"
  .field /*040000FD*/ static assembly literal string ActionPropertyName = "Action"
  .field /*040000FE*/ static assembly literal string ToPropertyName = "To"
  .field /*040000FF*/ static assembly literal string ConditionPropertyName = "Condition"
  .method /*06000434*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2520
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::.ctor

  .method /*06000435*/ public hidebysig specialname 
          instance object  get_NewTransition() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2521
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::get_NewTransition

  .method /*06000436*/ public hidebysig specialname 
          instance void  set_NewTransition(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2522
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::set_NewTransition

  .method /*06000437*/ public hidebysig specialname 
          instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/> 
          get_TransitionsSharingTrigger() cil managed
  // SIG: 20 00 15 12 80 DD 01 12 82 1C
  {
    // Method begins at RVA 0x2523
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::get_TransitionsSharingTrigger

  .method /*06000438*/ public hidebysig specialname 
          instance void  set_TransitionsSharingTrigger(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 DD 01 12 82 1C
  {
    // Method begins at RVA 0x2524
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::set_TransitionsSharingTrigger

  .method /*06000439*/ public hidebysig specialname 
          instance class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/ 
          get_SelectedTransition() cil managed
  // SIG: 20 00 12 82 1C
  {
    // Method begins at RVA 0x2525
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::get_SelectedTransition

  .method /*0600043A*/ public hidebysig specialname 
          instance void  set_SelectedTransition(class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/ 'value') cil managed
  // SIG: 20 01 01 12 82 1C
  {
    // Method begins at RVA 0x2526
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::set_SelectedTransition

  .method /*0600043B*/ public hidebysig static 
          void  RegisterMetadata(class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Metadata.AttributeTableBuilder/*0100000C*/ builder) cil managed
  // SIG: 00 01 01 12 31
  {
    // Method begins at RVA 0x2527
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::RegisterMetadata

  .method /*0600043C*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001BD:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001BE:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2528
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::InitializeComponent

  .method /*0600043D*/ assembly hidebysig 
          instance class [mscorlib/*23000001*/]System.Delegate/*01000006*/ 
          _CreateDelegate(class [mscorlib/*23000001*/]System.Type/*01000003*/ delegateType,
                          string 'handler') cil managed
  // SIG: 20 02 12 19 12 0D 0E
  {
    .custom /*0C0001BF:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001C0:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2529
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::_CreateDelegate

  .method /*0600043E*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001C1:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001C2:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001C3:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x252a
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::System.Windows.Markup.IComponentConnector.Connect

  .method /*0600043F*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IStyleConnector.Connect(int32 connectionId,
                                                                       object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001C4:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001C5:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001C6:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [PresentationFramework/*23000006*/]System.Windows.Markup.IStyleConnector/*01000014*/::Connect /*01000014::0A000004*/ 
    // Method begins at RVA 0x252b
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::System.Windows.Markup.IStyleConnector.Connect

  .method /*06000440*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x252c
    // Code size       0 (0x0)
  } // end of method TransitionDesigner::.cctor

  .property /*1700011B*/ instance callconv(8) object
          NewTransition()
  {
    .get instance object System.Activities.Core.Presentation.TransitionDesigner/*02000092*/::get_NewTransition() /* 06000435 */
    .set instance void System.Activities.Core.Presentation.TransitionDesigner/*02000092*/::set_NewTransition(object) /* 06000436 */
  } // end of property TransitionDesigner::NewTransition
  .property /*1700011C*/ instance callconv(8) class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/>
          TransitionsSharingTrigger()
  {
    .get instance class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/> System.Activities.Core.Presentation.TransitionDesigner/*02000092*/::get_TransitionsSharingTrigger() /* 06000437 */
    .set instance void System.Activities.Core.Presentation.TransitionDesigner/*02000092*/::set_TransitionsSharingTrigger(class [System/*23000002*/]System.Collections.ObjectModel.ObservableCollection`1/*01000037*/<class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/>) /* 06000438 */
  } // end of property TransitionDesigner::TransitionsSharingTrigger
  .property /*1700011D*/ instance callconv(8) class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/
          SelectedTransition()
  {
    .get instance class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/ System.Activities.Core.Presentation.TransitionDesigner/*02000092*/::get_SelectedTransition() /* 06000439 */
    .set instance void System.Activities.Core.Presentation.TransitionDesigner/*02000092*/::set_SelectedTransition(class System.Activities.Core.Presentation.ExpandableItemWrapper/*02000087*/) /* 0600043A */
  } // end of property TransitionDesigner::SelectedTransition
} // end of class System.Activities.Core.Presentation.TransitionDesigner

.class /*02000093*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StateMachineValidationErrorSourceLocator
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Validation.IValidationErrorSourceLocator/*0100006E*/
{
  .method /*06000441*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> 
          FindSourceDetailFromActivity(class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ errorSource,
                                       object errorSourceDetail) cil managed
  // SIG: 20 02 15 12 81 51 01 1C 12 80 85 1C
  {
    // Method begins at RVA 0x252d
    // Code size       0 (0x0)
  } // end of method StateMachineValidationErrorSourceLocator::FindSourceDetailFromActivity

  .method /*06000442*/ public hidebysig newslot virtual final 
          instance void  ReplaceParentChainWithSource(class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ parentActivity,
                                                      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<object> parentChain) cil managed
  // SIG: 20 02 01 12 80 85 15 12 81 51 01 1C
  {
    // Method begins at RVA 0x252e
    // Code size       0 (0x0)
  } // end of method StateMachineValidationErrorSourceLocator::ReplaceParentChainWithSource

  .method /*06000443*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x252f
    // Code size       0 (0x0)
  } // end of method StateMachineValidationErrorSourceLocator::.ctor

} // end of class System.Activities.Core.Presentation.StateMachineValidationErrorSourceLocator

.class /*02000094*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StateMachineValidationErrorSourceLocatorFeature
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.ValidationErrorSourceLocatorFeature/*0100006F*/
{
  .method /*06000444*/ family hidebysig specialname virtual 
          instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Validation.IValidationErrorSourceLocator/*0100006E*/ 
          get_ValidationErrorSourceLocator() cil managed
  // SIG: 20 00 12 81 B9
  {
    // Method begins at RVA 0x2530
    // Code size       0 (0x0)
  } // end of method StateMachineValidationErrorSourceLocatorFeature::get_ValidationErrorSourceLocator

  .method /*06000445*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2531
    // Code size       0 (0x0)
  } // end of method StateMachineValidationErrorSourceLocatorFeature::.ctor

  .property /*1700011E*/ instance callconv(8) class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Validation.IValidationErrorSourceLocator/*0100006E*/
          ValidationErrorSourceLocator()
  {
    .get instance class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.Validation.IValidationErrorSourceLocator/*0100006E*/ System.Activities.Core.Presentation.StateMachineValidationErrorSourceLocatorFeature/*02000094*/::get_ValidationErrorSourceLocator() /* 06000444 */
  } // end of property StateMachineValidationErrorSourceLocatorFeature::ValidationErrorSourceLocator
} // end of class System.Activities.Core.Presentation.StateMachineValidationErrorSourceLocatorFeature

.class /*02000095*/ private auto ansi beforefieldinit System.Activities.Core.Presentation.StateMachineConnectionPointsAdorner
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPointsAdorner/*01000084*/
{
  .method /*06000446*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [PresentationCore/*23000009*/]System.Windows.UIElement/*01000050*/ adornedElement,
                               class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000054*/<class [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.ConnectionPoint/*01000057*/> connectionPointsToShow,
                               bool isParentShapeSelected) cil managed
  // SIG: 20 03 01 12 81 41 15 12 81 51 01 12 81 5D 02
  {
    // Method begins at RVA 0x2532
    // Code size       0 (0x0)
  } // end of method StateMachineConnectionPointsAdorner::.ctor

  .method /*06000447*/ family hidebysig virtual 
          instance void  OnRender(class [PresentationCore/*23000009*/]System.Windows.Media.DrawingContext/*0100007C*/ drawingContext) cil managed
  // SIG: 20 01 01 12 81 F1
  {
    // Method begins at RVA 0x2533
    // Code size       0 (0x0)
  } // end of method StateMachineConnectionPointsAdorner::OnRender

  .method /*06000448*/ family hidebysig virtual 
          instance void  OnMouseDown(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x2534
    // Code size       0 (0x0)
  } // end of method StateMachineConnectionPointsAdorner::OnMouseDown

  .method /*06000449*/ family hidebysig virtual 
          instance void  OnMouseUp(class [PresentationCore/*23000009*/]System.Windows.Input.MouseButtonEventArgs/*01000066*/ e) cil managed
  // SIG: 20 01 01 12 81 99
  {
    // Method begins at RVA 0x2535
    // Code size       0 (0x0)
  } // end of method StateMachineConnectionPointsAdorner::OnMouseUp

  .method /*0600044A*/ family hidebysig virtual 
          instance void  OnContextMenuOpening(class [PresentationFramework/*23000006*/]System.Windows.Controls.ContextMenuEventArgs/*01000073*/ e) cil managed
  // SIG: 20 01 01 12 81 CD
  {
    // Method begins at RVA 0x2536
    // Code size       0 (0x0)
  } // end of method StateMachineConnectionPointsAdorner::OnContextMenuOpening

} // end of class System.Activities.Core.Presentation.StateMachineConnectionPointsAdorner

.class /*02000096*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.StateMachineFreeFormPanel
       extends [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.FreeFormEditing.FreeFormPanel/*01000051*/
{
  .method /*0600044B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2537
    // Code size       0 (0x0)
  } // end of method StateMachineFreeFormPanel::.ctor

} // end of class System.Activities.Core.Presentation.StateMachineFreeFormPanel

.class /*02000097*/ private abstract auto ansi sealed beforefieldinit System.Activities.Core.Presentation.FxTrace
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
{
  .method /*0600044C*/ public hidebysig static 
          void  UpdateEventDefinitions(valuetype [System.ServiceModel.Internals/*2300000F*/]System.Runtime.Diagnostics.EventDescriptor/*01000088*/[] eventDescriptors,
                                       uint16[] end2EndEvents) cil managed
  // SIG: 00 02 01 1D 11 82 21 1D 07
  {
    // Method begins at RVA 0x2538
    // Code size       0 (0x0)
  } // end of method FxTrace::UpdateEventDefinitions

  .method /*0600044D*/ public hidebysig static 
          bool  IsEventEnabled(int32 index) cil managed
  // SIG: 00 01 02 08
  {
    // Method begins at RVA 0x2539
    // Code size       0 (0x0)
  } // end of method FxTrace::IsEventEnabled

  .method /*0600044E*/ public hidebysig specialname static 
          bool  get_ShouldTraceCritical() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x253a
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceCritical

  .method /*0600044F*/ public hidebysig specialname static 
          bool  get_TracingEnabled() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x253b
    // Code size       0 (0x0)
  } // end of method FxTrace::get_TracingEnabled

  .method /*06000450*/ public hidebysig specialname static 
          bool  get_ShouldTraceError() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x253c
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceError

  .method /*06000451*/ public hidebysig specialname static 
          bool  get_ShouldTraceInformation() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x253d
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceInformation

  .method /*06000452*/ public hidebysig specialname static 
          bool  get_ShouldTraceVerbose() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x253e
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceVerbose

  .method /*06000453*/ public hidebysig specialname static 
          bool  get_ShouldTraceWarning() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x253f
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceWarning

  .method /*06000454*/ public hidebysig specialname static 
          bool  get_ShouldTraceCriticalToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2540
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceCriticalToTraceSource

  .method /*06000455*/ public hidebysig specialname static 
          bool  get_ShouldTraceErrorToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2541
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceErrorToTraceSource

  .method /*06000456*/ public hidebysig specialname static 
          bool  get_ShouldTraceInformationToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2542
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceInformationToTraceSource

  .method /*06000457*/ public hidebysig specialname static 
          bool  get_ShouldTraceVerboseToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2543
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceVerboseToTraceSource

  .method /*06000458*/ public hidebysig specialname static 
          bool  get_ShouldTraceWarningToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2544
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceWarningToTraceSource

  .method /*06000459*/ public hidebysig specialname static 
          class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.ExceptionTrace/*01000089*/ 
          get_Exception() cil managed
  // SIG: 00 00 12 82 25
  {
    // Method begins at RVA 0x2545
    // Code size       0 (0x0)
  } // end of method FxTrace::get_Exception

  .method /*0600045A*/ public hidebysig specialname static 
          class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.Diagnostics.EtwDiagnosticTrace/*0100008A*/ 
          get_Trace() cil managed
  // SIG: 00 00 12 82 29
  {
    // Method begins at RVA 0x2546
    // Code size       0 (0x0)
  } // end of method FxTrace::get_Trace

  .method /*0600045B*/ public hidebysig specialname static 
          class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.Diagnostics.EventLogger/*0100008B*/ 
          get_EventLog() cil managed
  // SIG: 00 00 12 82 2D
  {
    // Method begins at RVA 0x2547
    // Code size       0 (0x0)
  } // end of method FxTrace::get_EventLog

  .method /*0600045C*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2548
    // Code size       0 (0x0)
  } // end of method FxTrace::.cctor

  .property /*1700011F*/ callconv(8) bool
          ShouldTraceCritical()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceCritical() /* 0600044E */
  } // end of property FxTrace::ShouldTraceCritical
  .property /*17000120*/ callconv(8) bool
          TracingEnabled()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_TracingEnabled() /* 0600044F */
  } // end of property FxTrace::TracingEnabled
  .property /*17000121*/ callconv(8) bool
          ShouldTraceError()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceError() /* 06000450 */
  } // end of property FxTrace::ShouldTraceError
  .property /*17000122*/ callconv(8) bool
          ShouldTraceInformation()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceInformation() /* 06000451 */
  } // end of property FxTrace::ShouldTraceInformation
  .property /*17000123*/ callconv(8) bool
          ShouldTraceVerbose()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceVerbose() /* 06000452 */
  } // end of property FxTrace::ShouldTraceVerbose
  .property /*17000124*/ callconv(8) bool
          ShouldTraceWarning()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceWarning() /* 06000453 */
  } // end of property FxTrace::ShouldTraceWarning
  .property /*17000125*/ callconv(8) bool
          ShouldTraceCriticalToTraceSource()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceCriticalToTraceSource() /* 06000454 */
  } // end of property FxTrace::ShouldTraceCriticalToTraceSource
  .property /*17000126*/ callconv(8) bool
          ShouldTraceErrorToTraceSource()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceErrorToTraceSource() /* 06000455 */
  } // end of property FxTrace::ShouldTraceErrorToTraceSource
  .property /*17000127*/ callconv(8) bool
          ShouldTraceInformationToTraceSource()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceInformationToTraceSource() /* 06000456 */
  } // end of property FxTrace::ShouldTraceInformationToTraceSource
  .property /*17000128*/ callconv(8) bool
          ShouldTraceVerboseToTraceSource()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceVerboseToTraceSource() /* 06000457 */
  } // end of property FxTrace::ShouldTraceVerboseToTraceSource
  .property /*17000129*/ callconv(8) bool
          ShouldTraceWarningToTraceSource()
  {
    .get bool System.Activities.Core.Presentation.FxTrace/*02000097*/::get_ShouldTraceWarningToTraceSource() /* 06000458 */
  } // end of property FxTrace::ShouldTraceWarningToTraceSource
  .property /*1700012A*/ callconv(8) class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.ExceptionTrace/*01000089*/
          Exception()
  {
    .get class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.ExceptionTrace/*01000089*/ System.Activities.Core.Presentation.FxTrace/*02000097*/::get_Exception() /* 06000459 */
  } // end of property FxTrace::Exception
  .property /*1700012B*/ callconv(8) class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.Diagnostics.EtwDiagnosticTrace/*0100008A*/
          Trace()
  {
    .get class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.Diagnostics.EtwDiagnosticTrace/*0100008A*/ System.Activities.Core.Presentation.FxTrace/*02000097*/::get_Trace() /* 0600045A */
  } // end of property FxTrace::Trace
  .property /*1700012C*/ callconv(8) class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.Diagnostics.EventLogger/*0100008B*/
          EventLog()
  {
    .get class [System.ServiceModel.Internals/*2300000F*/]System.Runtime.Diagnostics.EventLogger/*0100008B*/ System.Activities.Core.Presentation.FxTrace/*02000097*/::get_EventLog() /* 0600045B */
  } // end of property FxTrace::EventLog
} // end of class System.Activities.Core.Presentation.FxTrace

.class /*02000098*/ public auto ansi sealed beforefieldinit System.Activities.Core.Presentation.Factories.ForEachWithBodyFactory`1<T>
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IActivityTemplateFactory/*01000043*/
{
  .method /*0600045D*/ public hidebysig newslot virtual final 
          instance class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ 
          Create(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ target) cil managed
  // SIG: 20 01 12 80 85 12 6D
  {
    // Method begins at RVA 0x2549
    // Code size       0 (0x0)
  } // end of method ForEachWithBodyFactory`1::Create

  .method /*0600045E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x254a
    // Code size       0 (0x0)
  } // end of method ForEachWithBodyFactory`1::.ctor

} // end of class System.Activities.Core.Presentation.Factories.ForEachWithBodyFactory`1

.class /*02000099*/ public auto ansi sealed beforefieldinit System.Activities.Core.Presentation.Factories.ParallelForEachWithBodyFactory`1<T>
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IActivityTemplateFactory/*01000043*/
{
  .method /*0600045F*/ public hidebysig newslot virtual final 
          instance class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ 
          Create(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ target) cil managed
  // SIG: 20 01 12 80 85 12 6D
  {
    // Method begins at RVA 0x254b
    // Code size       0 (0x0)
  } // end of method ParallelForEachWithBodyFactory`1::Create

  .method /*06000460*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x254c
    // Code size       0 (0x0)
  } // end of method ParallelForEachWithBodyFactory`1::.ctor

} // end of class System.Activities.Core.Presentation.Factories.ParallelForEachWithBodyFactory`1

.class /*0200009A*/ public auto ansi sealed beforefieldinit System.Activities.Core.Presentation.Factories.PickWithTwoBranchesFactory
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IActivityTemplateFactory/*01000043*/
{
  .method /*06000461*/ public hidebysig newslot virtual final 
          instance class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ 
          Create(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ target) cil managed
  // SIG: 20 01 12 80 85 12 6D
  {
    // Method begins at RVA 0x254d
    // Code size       0 (0x0)
  } // end of method PickWithTwoBranchesFactory::Create

  .method /*06000462*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x254e
    // Code size       0 (0x0)
  } // end of method PickWithTwoBranchesFactory::.ctor

} // end of class System.Activities.Core.Presentation.Factories.PickWithTwoBranchesFactory

.class /*0200009B*/ public auto ansi sealed beforefieldinit System.Activities.Core.Presentation.Factories.StateMachineWithInitialStateFactory
       extends [mscorlib/*23000001*/]System.Object/*01000002*/
       implements [System.Activities.Presentation/*23000004*/]System.Activities.Presentation.IActivityTemplateFactory/*01000043*/
{
  .method /*06000463*/ public hidebysig newslot virtual final 
          instance class [System.Activities/*2300000B*/]System.Activities.Activity/*01000021*/ 
          Create(class [WindowsBase/*23000003*/]System.Windows.DependencyObject/*0100001B*/ target) cil managed
  // SIG: 20 01 12 80 85 12 6D
  {
    // Method begins at RVA 0x254f
    // Code size       0 (0x0)
  } // end of method StateMachineWithInitialStateFactory::Create

  .method /*06000464*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2550
    // Code size       0 (0x0)
  } // end of method StateMachineWithInitialStateFactory::.ctor

} // end of class System.Activities.Core.Presentation.Factories.StateMachineWithInitialStateFactory

.class /*0200009C*/ public auto ansi sealed beforefieldinit System.Activities.Core.Presentation.Themes.DesignerStylesDictionary
       extends [PresentationFramework/*23000006*/]System.Windows.ResourceDictionary/*0100003A*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*06000465*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2551
    // Code size       0 (0x0)
  } // end of method DesignerStylesDictionary::.ctor

  .method /*06000466*/ public hidebysig specialname static 
          class [PresentationFramework/*23000006*/]System.Windows.Style/*0100008C*/ 
          get_SequenceStyle() cil managed
  // SIG: 00 00 12 82 31
  {
    // Method begins at RVA 0x2552
    // Code size       0 (0x0)
  } // end of method DesignerStylesDictionary::get_SequenceStyle

  .method /*06000467*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001C7:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001C8:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2553
    // Code size       0 (0x0)
  } // end of method DesignerStylesDictionary::InitializeComponent

  .method /*06000468*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001C9:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001CA:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001CB:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2554
    // Code size       0 (0x0)
  } // end of method DesignerStylesDictionary::System.Windows.Markup.IComponentConnector.Connect

  .property /*1700012D*/ callconv(8) class [PresentationFramework/*23000006*/]System.Windows.Style/*0100008C*/
          SequenceStyle()
  {
    .get class [PresentationFramework/*23000006*/]System.Windows.Style/*0100008C*/ System.Activities.Core.Presentation.Themes.DesignerStylesDictionary/*0200009C*/::get_SequenceStyle() /* 06000466 */
  } // end of property DesignerStylesDictionary::SequenceStyle
} // end of class System.Activities.Core.Presentation.Themes.DesignerStylesDictionary

.class /*0200009D*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.Themes.EditorCategoryTemplateDictionary
       extends [PresentationFramework/*23000006*/]System.Windows.ResourceDictionary/*0100003A*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*06000469*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2555
    // Code size       0 (0x0)
  } // end of method EditorCategoryTemplateDictionary::.ctor

  .method /*0600046A*/ public hidebysig specialname static 
          class System.Activities.Core.Presentation.Themes.EditorCategoryTemplateDictionary/*0200009D*/ 
          get_Instance() cil managed
  // SIG: 00 00 12 82 74
  {
    // Method begins at RVA 0x2556
    // Code size       0 (0x0)
  } // end of method EditorCategoryTemplateDictionary::get_Instance

  .method /*0600046B*/ public hidebysig instance class [PresentationFramework/*23000006*/]System.Windows.DataTemplate/*01000087*/ 
          GetCategoryTemplate(string templateName) cil managed
  // SIG: 20 01 12 82 1D 0E
  {
    // Method begins at RVA 0x2557
    // Code size       0 (0x0)
  } // end of method EditorCategoryTemplateDictionary::GetCategoryTemplate

  .method /*0600046C*/ public hidebysig instance string 
          GetCategoryTitle(string categoryName) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x2558
    // Code size       0 (0x0)
  } // end of method EditorCategoryTemplateDictionary::GetCategoryTitle

  .method /*0600046D*/ public hidebysig instance object 
          GetCategoryImage(string imageName) cil managed
  // SIG: 20 01 1C 0E
  {
    // Method begins at RVA 0x2559
    // Code size       0 (0x0)
  } // end of method EditorCategoryTemplateDictionary::GetCategoryImage

  .method /*0600046E*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001CC:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001CD:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x255a
    // Code size       0 (0x0)
  } // end of method EditorCategoryTemplateDictionary::InitializeComponent

  .method /*0600046F*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001CE:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001CF:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001D0:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x255b
    // Code size       0 (0x0)
  } // end of method EditorCategoryTemplateDictionary::System.Windows.Markup.IComponentConnector.Connect

  .property /*1700012E*/ callconv(8) class System.Activities.Core.Presentation.Themes.EditorCategoryTemplateDictionary/*0200009D*/
          Instance()
  {
    .get class System.Activities.Core.Presentation.Themes.EditorCategoryTemplateDictionary/*0200009D*/ System.Activities.Core.Presentation.Themes.EditorCategoryTemplateDictionary/*0200009D*/::get_Instance() /* 0600046A */
  } // end of property EditorCategoryTemplateDictionary::Instance
} // end of class System.Activities.Core.Presentation.Themes.EditorCategoryTemplateDictionary

.class /*0200009E*/ private auto ansi sealed beforefieldinit System.Activities.Core.Presentation.Themes.StringResourceDictionary
       extends [PresentationFramework/*23000006*/]System.Windows.ResourceDictionary/*0100003A*/
       implements [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/
{
  .method /*06000470*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x255c
    // Code size       0 (0x0)
  } // end of method StringResourceDictionary::.ctor

  .method /*06000471*/ public hidebysig specialname static 
          class System.Activities.Core.Presentation.Themes.StringResourceDictionary/*0200009E*/ 
          get_Instance() cil managed
  // SIG: 00 00 12 82 78
  {
    // Method begins at RVA 0x255d
    // Code size       0 (0x0)
  } // end of method StringResourceDictionary::get_Instance

  .method /*06000472*/ public hidebysig instance string 
          GetString(string key) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x255e
    // Code size       0 (0x0)
  } // end of method StringResourceDictionary::GetString

  .method /*06000473*/ public hidebysig instance string 
          GetString(string key,
                    string defaultValue) cil managed
  // SIG: 20 02 0E 0E 0E
  {
    // Method begins at RVA 0x255f
    // Code size       0 (0x0)
  } // end of method StringResourceDictionary::GetString

  .method /*06000474*/ public hidebysig newslot virtual final 
          instance void  InitializeComponent() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001D1:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D2:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    // Method begins at RVA 0x2560
    // Code size       0 (0x0)
  } // end of method StringResourceDictionary::InitializeComponent

  .method /*06000475*/ private hidebysig newslot virtual final 
          instance void  System.Windows.Markup.IComponentConnector.Connect(int32 connectionId,
                                                                           object target) cil managed
  // SIG: 20 02 01 08 1C
  {
    .custom /*0C0001D3:0A000024*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerNonUserCodeAttribute/*010000A5*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D4:0A000025*/ instance void [System/*23000002*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*010000A6*/::.ctor(string,
                                                                                                                                      string) /* 0A000025 */ = ( 01 00 16 50 72 65 73 65 6E 74 61 74 69 6F 6E 42   // ...PresentationB
                                                                                                                                                                 75 69 6C 64 54 61 73 6B 73 07 34 2E 30 2E 30 2E   // uildTasks.4.0.0.
                                                                                                                                                                 30 00 00 )                                        // 0..
    .custom /*0C0001D5:0A000026*/ instance void [System/*23000002*/]System.ComponentModel.EditorBrowsableAttribute/*010000A7*/::.ctor(valuetype [System/*23000002*/]System.ComponentModel.EditorBrowsableState/*010000A8*/) /* 0A000026 */ = ( 01 00 01 00 00 00 00 00 ) 
    .override [System.Xaml/*23000005*/]System.Windows.Markup.IComponentConnector/*01000009*/::Connect /*01000009::0A000001*/ 
    // Method begins at RVA 0x2561
    // Code size       0 (0x0)
  } // end of method StringResourceDictionary::System.Windows.Markup.IComponentConnector.Connect

  .property /*1700012F*/ callconv(8) class System.Activities.Core.Presentation.Themes.StringResourceDictionary/*0200009E*/
          Instance()
  {
    .get class System.Activities.Core.Presentation.Themes.StringResourceDictionary/*0200009E*/ System.Activities.Core.Presentation.Themes.StringResourceDictionary/*0200009E*/::get_Instance() /* 06000471 */
  } // end of property StringResourceDictionary::Instance
} // end of class System.Activities.Core.Presentation.Themes.StringResourceDictionary


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
