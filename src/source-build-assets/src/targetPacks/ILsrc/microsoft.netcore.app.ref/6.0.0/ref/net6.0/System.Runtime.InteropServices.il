


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
// Time-date stamp:            0x8626e658
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000da00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000f85a
// Base of code:                   0x00002000
// Base of data:                   0x00010000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00014000
// Size of headers:                0x00000200
// Checksum:                       0x00013898
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000f805 [0x0000004f] address [size] of Import Directory:          
// 0x00010000 [0x000004d4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000e400 [0x00002470] address [size] of Security Directory:        
// 0x00012000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000f70c [0x00000054] address [size] of Debug Directory:           
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
//              0x0000d860 Virtual Size
//              0x00002000 Virtual Address
//              0x0000da00 Size of Raw Data
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
//              0x000004d4 Virtual Size
//              0x00010000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000dc00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00012000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0000e200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000f000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x0000085c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000f847 Import Name Table
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
// 0x00002840 [0x0000ce4c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000f68c [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00006f28 Size
//              '#~' Name
//    Stream 2:
//              0x00006f94 Offset
//              0x000041b4 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000b148 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x0000b14c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000b15c Offset
//              0x00001cf0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016890ba3df57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 67696
// PE header size       : 512 (496 used)    ( 0.76%)
// PE additional info   : 10819             (15.98%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.11%)
// CLR meta-data size  : 52812              (78.01%)
// CLR additional info : 128                ( 0.19%)
// CLR method headers  : 517                ( 0.76%)
// Managed code         : 1510              ( 2.23%)
// Data                 : 2048              ( 3.03%)
// Unaccounted          : -722              (-1.07%)

// Num.of PE sections   : 3
//   .text    - 55808
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 52812
//   Module        -    1 (10 bytes)
//   TypeDef       -  168 (2352 bytes)    26 interfaces, 2 explicit layout
//   TypeRef       -   79 (474 bytes)
//   MethodDef     -  680 (9520 bytes)    195 abstract, 0 native, 477 bodies
//   FieldDef      -  471 (2826 bytes)    2 constant
//   MemberRef     -   39 (234 bytes)
//   ParamDef      -  993 (5958 bytes)
//   MethodImpl    -    1 (6 bytes)
//   Constant      -  298 (1788 bytes)
//   CustomAttribute-  600 (3600 bytes)
//   ClassLayout   -    3 (24 bytes)
//   FieldLayout   -    7 (42 bytes)
//   StandAloneSig -    1 (2 bytes)
//   InterfaceImpl -    8 (32 bytes)
//   PropertyMap   -   47 (188 bytes)
//   Property      -   68 (408 bytes)
//   MethodSemantic-   69 (414 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    3 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   ExportedType  -    8 (112 bytes)
//   NestedClass   -    6 (24 bytes)
//   GenericParam  -   28 (224 bytes)
//   GenericParamConstraint-    4 (16 bytes)
//   Strings       - 16820 bytes
//   Blobs         -  7408 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   236 bytes

// CLR additional info : 128

// CLR method headers : 517
//   Num.of method bodies  - 477
//   Num.of fat headers    - 2
//   Num.of tiny headers   - 475
//   Num.of fat sections   - 0
//   Num.of small sections - 1

// Managed code : 1510
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class NativeIntegerAttribute         (auto) (ansi) (sealed) 
// Class DataMisalignedException        (public) (auto) (ansi) (sealed) 
// Class DllNotFoundException           (public) (auto) (ansi) 
// Class SecureString                   (public) (auto) (ansi) (sealed) 
// Class SecureStringMarshal            (public) (abstract) (auto) (ansi) (sealed) 
// Class AllowReversePInvokeCallsAttribute (public) (auto) (ansi) (sealed) 
// Class ArrayWithOffset                (public) (sequential) (ansi) (sealed) 
// Class AutomationProxyAttribute       (public) (auto) (ansi) (sealed) 
// Class BestFitMappingAttribute        (public) (auto) (ansi) (sealed) 
// Class BStrWrapper                    (public) (auto) (ansi) (sealed) 
// Class CallingConvention              (public) (auto) (ansi) (sealed) 
// Class ClassInterfaceAttribute        (public) (auto) (ansi) (sealed) 
// Class ClassInterfaceType             (public) (auto) (ansi) (sealed) 
// Class CLong                          (public) (sequential) (ansi) (sealed) 
// Class CoClassAttribute               (public) (auto) (ansi) (sealed) 
// Class CollectionsMarshal             (public) (abstract) (auto) (ansi) (sealed) 
// Class ComAliasNameAttribute          (public) (auto) (ansi) (sealed) 
// Class ComAwareEventInfo              (public) (auto) (ansi) 
// Class ComCompatibleVersionAttribute  (public) (auto) (ansi) (sealed) 
// Class ComConversionLossAttribute     (public) (auto) (ansi) (sealed) 
// Class ComDefaultInterfaceAttribute   (public) (auto) (ansi) (sealed) 
// Class ComEventInterfaceAttribute     (public) (auto) (ansi) (sealed) 
// Class ComEventsHelper                (public) (abstract) (auto) (ansi) (sealed) 
// Class COMException                   (public) (auto) (ansi) 
// Class ComImportAttribute             (public) (auto) (ansi) (sealed) 
// Class ComInterfaceType               (public) (auto) (ansi) (sealed) 
// Class ComMemberType                  (public) (auto) (ansi) (sealed) 
// Class ComRegisterFunctionAttribute   (public) (auto) (ansi) (sealed) 
// Class ComSourceInterfacesAttribute   (public) (auto) (ansi) (sealed) 
// Class ComUnregisterFunctionAttribute (public) (auto) (ansi) (sealed) 
// Class CULong                         (public) (sequential) (ansi) (sealed) 
// Class CurrencyWrapper                (public) (auto) (ansi) (sealed) 
// Class CustomQueryInterfaceMode       (public) (auto) (ansi) (sealed) 
// Class CustomQueryInterfaceResult     (public) (auto) (ansi) (sealed) 
// Class DefaultCharSetAttribute        (public) (auto) (ansi) (sealed) 
// Class DefaultDllImportSearchPathsAttribute (public) (auto) (ansi) (sealed) 
// Class DefaultParameterValueAttribute (public) (auto) (ansi) (sealed) 
// Class DispatchWrapper                (public) (auto) (ansi) (sealed) 
// Class DispIdAttribute                (public) (auto) (ansi) (sealed) 
// Class DllImportAttribute             (public) (auto) (ansi) (sealed) 
// Class DllImportResolver              (public) (auto) (ansi) (sealed) 
// Class DllImportSearchPath            (public) (auto) (ansi) (sealed) 
// Class DynamicInterfaceCastableImplementationAttribute (public) (auto) (ansi) (sealed) 
// Class ErrorWrapper                   (public) (auto) (ansi) (sealed) 
// Class GuidAttribute                  (public) (auto) (ansi) (sealed) 
// Class HandleCollector                (public) (auto) (ansi) (sealed) 
// Class HandleRef                      (public) (sequential) (ansi) (sealed) 
// Interface ICustomAdapter                 (public) (abstract) (auto) (ansi) 
// Interface ICustomFactory                 (public) (abstract) (auto) (ansi) 
// Interface ICustomMarshaler               (public) (abstract) (auto) (ansi) 
// Interface ICustomQueryInterface          (public) (abstract) (auto) (ansi) 
// Interface IDynamicInterfaceCastable      (public) (abstract) (auto) (ansi) 
// Class ImportedFromTypeLibAttribute   (public) (auto) (ansi) (sealed) 
// Class InterfaceTypeAttribute         (public) (auto) (ansi) (sealed) 
// Class InvalidComObjectException      (public) (auto) (ansi) 
// Class InvalidOleVariantTypeException (public) (auto) (ansi) 
// Class LCIDConversionAttribute        (public) (auto) (ansi) (sealed) 
// Class ManagedToNativeComInteropStubAttribute (public) (auto) (ansi) (sealed) 
// Class Marshal                        (public) (abstract) (auto) (ansi) (sealed) 
// Class MarshalAsAttribute             (public) (auto) (ansi) (sealed) 
// Class MarshalDirectiveException      (public) (auto) (ansi) 
// Class NativeLibrary                  (public) (abstract) (auto) (ansi) (sealed) 
// Class NativeMemory                   (public) (abstract) (auto) (ansi) (sealed) 
// Class NFloat                         (public) (sequential) (ansi) (sealed) 
// Class OptionalAttribute              (public) (auto) (ansi) (sealed) 
// Class PosixSignal                    (public) (auto) (ansi) (sealed) 
// Class PosixSignalContext             (public) (auto) (ansi) (sealed) 
// Class PosixSignalRegistration        (public) (auto) (ansi) (sealed) 
// Class PreserveSigAttribute           (public) (auto) (ansi) (sealed) 
// Class PrimaryInteropAssemblyAttribute (public) (auto) (ansi) (sealed) 
// Class ProgIdAttribute                (public) (auto) (ansi) (sealed) 
// Class RuntimeEnvironment             (public) (abstract) (auto) (ansi) (sealed) 
// Class SafeArrayRankMismatchException (public) (auto) (ansi) 
// Class SafeArrayTypeMismatchException (public) (auto) (ansi) 
// Class SEHException                   (public) (auto) (ansi) 
// Class StandardOleMarshalObject       (public) (auto) (ansi) 
// Class TypeIdentifierAttribute        (public) (auto) (ansi) (sealed) 
// Class TypeLibFuncAttribute           (public) (auto) (ansi) (sealed) 
// Class TypeLibFuncFlags               (public) (auto) (ansi) (sealed) 
// Class TypeLibImportClassAttribute    (public) (auto) (ansi) (sealed) 
// Class TypeLibTypeAttribute           (public) (auto) (ansi) (sealed) 
// Class TypeLibTypeFlags               (public) (auto) (ansi) (sealed) 
// Class TypeLibVarAttribute            (public) (auto) (ansi) (sealed) 
// Class TypeLibVarFlags                (public) (auto) (ansi) (sealed) 
// Class TypeLibVersionAttribute        (public) (auto) (ansi) (sealed) 
// Class UnknownWrapper                 (public) (auto) (ansi) (sealed) 
// Class UnmanagedFunctionPointerAttribute (public) (auto) (ansi) (sealed) 
// Class UnmanagedType                  (public) (auto) (ansi) (sealed) 
// Class VarEnum                        (public) (auto) (ansi) (sealed) 
// Class VariantWrapper                 (public) (auto) (ansi) (sealed) 
// Class CreateComInterfaceFlags        (public) (auto) (ansi) (sealed) 
// Class CreateObjectFlags              (public) (auto) (ansi) (sealed) 
// Class ComWrappers                    (public) (abstract) (auto) (ansi) 
// Class UnmanagedCallConvAttribute     (public) (auto) (ansi) (sealed) 
// Class UnmanagedCallersOnlyAttribute  (public) (auto) (ansi) (sealed) 
// Class ObjectiveCTrackedTypeAttribute (public) (auto) (ansi) (sealed) 
// Class ObjectiveCMarshal              (public) (abstract) (auto) (ansi) (sealed) 
// Class ADVF                           (public) (auto) (ansi) (sealed) 
// Class BINDPTR                        (public) (explicit) (ansi) (sealed) 
// Class BIND_OPTS                      (public) (sequential) (ansi) (sealed) 
// Class CALLCONV                       (public) (auto) (ansi) (sealed) 
// Class CONNECTDATA                    (public) (sequential) (ansi) (sealed) 
// Class DATADIR                        (public) (auto) (ansi) (sealed) 
// Class DESCKIND                       (public) (auto) (ansi) (sealed) 
// Class DISPPARAMS                     (public) (sequential) (ansi) (sealed) 
// Class DVASPECT                       (public) (auto) (ansi) (sealed) 
// Class ELEMDESC                       (public) (sequential) (ansi) (sealed) 
// Class EXCEPINFO                      (public) (sequential) (ansi) (sealed) 
// Class FILETIME                       (public) (sequential) (ansi) (sealed) 
// Class FORMATETC                      (public) (sequential) (ansi) (sealed) 
// Class FUNCDESC                       (public) (sequential) (ansi) (sealed) 
// Class FUNCFLAGS                      (public) (auto) (ansi) (sealed) 
// Class FUNCKIND                       (public) (auto) (ansi) (sealed) 
// Interface IAdviseSink                    (public) (abstract) (auto) (ansi) 
// Interface IBindCtx                       (public) (abstract) (auto) (ansi) 
// Interface IConnectionPoint               (public) (abstract) (auto) (ansi) 
// Interface IConnectionPointContainer      (public) (abstract) (auto) (ansi) 
// Interface IDataObject                    (public) (abstract) (auto) (ansi) 
// Class IDLDESC                        (public) (sequential) (ansi) (sealed) 
// Class IDLFLAG                        (public) (auto) (ansi) (sealed) 
// Interface IEnumConnectionPoints          (public) (abstract) (auto) (ansi) 
// Interface IEnumConnections               (public) (abstract) (auto) (ansi) 
// Interface IEnumFORMATETC                 (public) (abstract) (auto) (ansi) 
// Interface IEnumMoniker                   (public) (abstract) (auto) (ansi) 
// Interface IEnumSTATDATA                  (public) (abstract) (auto) (ansi) 
// Interface IEnumString                    (public) (abstract) (auto) (ansi) 
// Interface IEnumVARIANT                   (public) (abstract) (auto) (ansi) 
// Interface IMoniker                       (public) (abstract) (auto) (ansi) 
// Class IMPLTYPEFLAGS                  (public) (auto) (ansi) (sealed) 
// Class INVOKEKIND                     (public) (auto) (ansi) (sealed) 
// Interface IPersistFile                   (public) (abstract) (auto) (ansi) 
// Interface IRunningObjectTable            (public) (abstract) (auto) (ansi) 
// Interface IStream                        (public) (abstract) (auto) (ansi) 
// Interface ITypeComp                      (public) (abstract) (auto) (ansi) 
// Interface ITypeInfo                      (public) (abstract) (auto) (ansi) 
// Interface ITypeInfo2                     (public) (abstract) (auto) (ansi) 
// Interface ITypeLib                       (public) (abstract) (auto) (ansi) 
// Interface ITypeLib2                      (public) (abstract) (auto) (ansi) 
// Class LIBFLAGS                       (public) (auto) (ansi) (sealed) 
// Class PARAMDESC                      (public) (sequential) (ansi) (sealed) 
// Class PARAMFLAG                      (public) (auto) (ansi) (sealed) 
// Class STATDATA                       (public) (sequential) (ansi) (sealed) 
// Class STATSTG                        (public) (sequential) (ansi) (sealed) 
// Class STGMEDIUM                      (public) (sequential) (ansi) (sealed) 
// Class SYSKIND                        (public) (auto) (ansi) (sealed) 
// Class TYMED                          (public) (auto) (ansi) (sealed) 
// Class TYPEATTR                       (public) (sequential) (ansi) (sealed) 
// Class TYPEDESC                       (public) (sequential) (ansi) (sealed) 
// Class TYPEFLAGS                      (public) (auto) (ansi) (sealed) 
// Class TYPEKIND                       (public) (auto) (ansi) (sealed) 
// Class TYPELIBATTR                    (public) (sequential) (ansi) (sealed) 
// Class VARDESC                        (public) (sequential) (ansi) (sealed) 
// Class VARFLAGS                       (public) (auto) (ansi) (sealed) 
// Class VARKIND                        (public) (auto) (ansi) (sealed) 
// Class IDispatchConstantAttribute     (public) (auto) (ansi) (sealed) 
// Class IUnknownConstantAttribute      (public) (auto) (ansi) (sealed) 
// Class UnmanagedMemoryAccessor        (public) (auto) (ansi) 
// Class ComInterfaceEntry              (sequential) (ansi) (sealed) (nested public) 
// Class ComInterfaceDispatch           (sequential) (ansi) (sealed) (nested public) 
// Class UnhandledExceptionPropagationHandler (auto) (ansi) (sealed) (nested public) 
// Class MessageSendFunction            (auto) (ansi) (sealed) (nested public) 
// Class DESCUNION                      (explicit) (ansi) (sealed) (nested public) 
// Class DESCUNION                      (explicit) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Runtime.InteropServices
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 1E 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                        65 2E 49 6E 74 65 72 6F 70 53 65 72 76 69 63 65   // e.InteropService
                                                                                                                                                                        73 00 00 )                                        // s..
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 1E 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                       65 2E 49 6E 74 65 72 6F 70 53 65 72 76 69 63 65   // e.InteropService
                                                                                                                                                                       73 00 00 )                                        // s..
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 1E 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                 65 2E 49 6E 74 65 72 6F 70 53 65 72 76 69 63 65   // e.InteropService
                                                                                                                                                                 73 00 00 )                                        // s..
  .custom /*0C000015:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .permissionset reqmin
             = {[System.Runtime/*23000001*/]System.Security.Permissions.SecurityPermissionAttribute/*01000012*/ = {property bool 'SkipVerification' = bool(true)}}
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
  .ver 6:0:0:0
}
.class extern /*27000001*/ forwarder System.IO.UnmanagedMemoryStream
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000002*/ forwarder System.Reflection.Missing
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000003*/ forwarder System.Runtime.InteropServices.CriticalHandle
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000004*/ forwarder System.Runtime.InteropServices.GCHandle
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000005*/ forwarder System.Runtime.InteropServices.GCHandleType
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000006*/ forwarder System.Runtime.InteropServices.InAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000007*/ forwarder System.Runtime.InteropServices.SafeBuffer
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000008*/ forwarder System.Runtime.InteropServices.SafeHandle
{
  .assembly extern System.Runtime /*23000001*/ 
}
.module System.Runtime.InteropServices.dll
// MVID: {fac8cca1-7e53-4853-a0eb-12ad9676406e}
.custom /*0C000001:0A000011*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000013*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000016:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000018:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000001*/ public initonly uint8[] NullableFlags
  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2058
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00004E       */ newarr     [System.Runtime/*23000001*/]System.Byte/*0100004E*/
    IL_000d:  /* 25   |                  */ dup
    IL_000e:  /* 16   |                  */ ldc.i4.0
    IL_000f:  /* 03   |                  */ ldarg.1
    IL_0010:  /* 9C   |                  */ stelem.i1
    IL_0011:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_0016:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] A_1) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2070
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00001C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000002*/ public initonly uint8 Flag
  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x207f
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00001F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000003*/ public initonly bool IncludesInternals
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool A_1) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x208e
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NativeIntegerAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000004*/ public initonly bool[] TransformFlags
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209d
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00004F       */ newarr     [System.Runtime/*23000001*/]System.Boolean/*0100004F*/
    IL_000d:  /* 25   |                  */ dup
    IL_000e:  /* 16   |                  */ ldc.i4.0
    IL_000f:  /* 17   |                  */ ldc.i4.1
    IL_0010:  /* 9C   |                  */ stelem.i1
    IL_0011:  /* 7D   | (04)000004       */ stfld      bool[] System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::TransformFlags /* 04000004 */
    IL_0016:  /* 2A   |                  */ ret
  } // end of method NativeIntegerAttribute::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool[] A_1) cil managed
  // SIG: 20 01 01 1D 02
  {
    // Method begins at RVA 0x20b5
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      bool[] System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::TransformFlags /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NativeIntegerAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NativeIntegerAttribute

.class /*02000007*/ public auto ansi sealed beforefieldinit System.DataMisalignedException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000018*/
{
  .custom /*0C000028:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataMisalignedException::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataMisalignedException::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x20d4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataMisalignedException::.ctor

} // end of class System.DataMisalignedException

.class /*02000008*/ public auto ansi beforefieldinit System.DllNotFoundException
       extends [System.Runtime/*23000001*/]System.TypeLoadException/*0100001A*/
{
  .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20dc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.TypeLoadException/*0100001A*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DllNotFoundException::.ctor

  .method /*0600000C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.TypeLoadException/*0100001A*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DllNotFoundException::.ctor

  .method /*0600000D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20ec
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.TypeLoadException/*0100001A*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DllNotFoundException::.ctor

  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ inner) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x20f4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.TypeLoadException/*0100001A*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DllNotFoundException::.ctor

} // end of class System.DllNotFoundException

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Security.SecureString
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001E*/
{
  .method /*0600000F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20fc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SecureString::.ctor

  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(char* 'value',
                               int32 length) cil managed
  // SIG: 20 02 01 0F 03 08
  {
    .custom /*0C00003D:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2104
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SecureString::.ctor

  .method /*06000011*/ public hidebysig specialname 
          instance int32  get_Length() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecureString::get_Length

  .method /*06000012*/ public hidebysig instance void 
          AppendChar(char c) cil managed
  // SIG: 20 01 01 03
  {
    // Method begins at RVA 0x210f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SecureString::AppendChar

  .method /*06000013*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2111
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SecureString::Clear

  .method /*06000014*/ public hidebysig instance class System.Security.SecureString/*02000009*/ 
          Copy() cil managed
  // SIG: 20 00 12 24
  {
    .custom /*0C000049:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecureString::Copy

  .method /*06000015*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2116
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SecureString::Dispose

  .method /*06000016*/ public hidebysig instance void 
          InsertAt(int32 index,
                   char c) cil managed
  // SIG: 20 02 01 08 03
  {
    // Method begins at RVA 0x2118
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SecureString::InsertAt

  .method /*06000017*/ public hidebysig instance bool 
          IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecureString::IsReadOnly

  .method /*06000018*/ public hidebysig instance void 
          MakeReadOnly() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x211d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SecureString::MakeReadOnly

  .method /*06000019*/ public hidebysig instance void 
          RemoveAt(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x211f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SecureString::RemoveAt

  .method /*0600001A*/ public hidebysig instance void 
          SetAt(int32 index,
                char c) cil managed
  // SIG: 20 02 01 08 03
  {
    // Method begins at RVA 0x2121
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SecureString::SetAt

  .property /*17000001*/ instance callconv(8) int32
          Length()
  {
    .get instance int32 System.Security.SecureString/*02000009*/::get_Length() /* 06000011 */
  } // end of property SecureString::Length
} // end of class System.Security.SecureString

.class /*0200000A*/ public abstract auto ansi sealed beforefieldinit System.Security.SecureStringMarshal
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001B*/ public hidebysig static 
          native int  SecureStringToCoTaskMemAnsi(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x2123
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecureStringMarshal::SecureStringToCoTaskMemAnsi

  .method /*0600001C*/ public hidebysig static 
          native int  SecureStringToCoTaskMemUnicode(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x2126
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecureStringMarshal::SecureStringToCoTaskMemUnicode

  .method /*0600001D*/ public hidebysig static 
          native int  SecureStringToGlobalAllocAnsi(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x2129
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecureStringMarshal::SecureStringToGlobalAllocAnsi

  .method /*0600001E*/ public hidebysig static 
          native int  SecureStringToGlobalAllocUnicode(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecureStringMarshal::SecureStringToGlobalAllocUnicode

} // end of class System.Security.SecureStringMarshal

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.AllowReversePInvokeCallsAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000030:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 40 43 6F 64 65 20 41 63 63 65 73 73 20 53   // ..@Code Access S
                                                                                                                                                 65 63 75 72 69 74 79 20 69 73 20 6E 6F 74 20 73   // ecurity is not s
                                                                                                                                                 75 70 70 6F 72 74 65 64 20 6F 72 20 68 6F 6E 6F   // upported or hono
                                                                                                                                                 72 65 64 20 62 79 20 74 68 65 20 72 75 6E 74 69   // red by the runti
                                                                                                                                                 6D 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F 73 74   // me...T..Diagnost
                                                                                                                                                 69 63 49 64 0A 53 59 53 4C 49 42 30 30 30 33 54   // icId.SYSLIB0003T
                                                                                                                                                 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74 74 70   // ..UrlFormat"http
                                                                                                                                                 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65   // s://aka.ms/dotne
                                                                                                                                                 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D )       // t-warnings/{0}
  .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..@.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .method /*0600001F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x212f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AllowReversePInvokeCallsAttribute::.ctor

} // end of class System.Runtime.InteropServices.AllowReversePInvokeCallsAttribute

.class /*0200000C*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ArrayWithOffset
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000035:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000020*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .field /*04000005*/ private initonly object _dummy
  .field /*04000006*/ private initonly int32 _dummyPrimitive
  .method /*06000020*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'array',
                               int32 offset) cil managed
  // SIG: 20 02 01 1C 08
  {
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayWithOffset::.ctor

  .method /*06000021*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .param [1]/*08000017*/ 
    .custom /*0C000059:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000022*/::.ctor(bool) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayWithOffset::Equals

  .method /*06000022*/ public hidebysig instance bool 
          Equals(valuetype System.Runtime.InteropServices.ArrayWithOffset/*0200000C*/ obj) cil managed
  // SIG: 20 01 02 11 30
  {
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayWithOffset::Equals

  .method /*06000023*/ public hidebysig instance object 
          GetArray() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayWithOffset::GetArray

  .method /*06000024*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayWithOffset::GetHashCode

  .method /*06000025*/ public hidebysig instance int32 
          GetOffset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayWithOffset::GetOffset

  .method /*06000026*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Runtime.InteropServices.ArrayWithOffset/*0200000C*/ a,
                            valuetype System.Runtime.InteropServices.ArrayWithOffset/*0200000C*/ b) cil managed
  // SIG: 00 02 02 11 30 11 30
  {
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayWithOffset::op_Equality

  .method /*06000027*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Runtime.InteropServices.ArrayWithOffset/*0200000C*/ a,
                              valuetype System.Runtime.InteropServices.ArrayWithOffset/*0200000C*/ b) cil managed
  // SIG: 00 02 02 11 30 11 30
  {
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayWithOffset::op_Inequality

} // end of class System.Runtime.InteropServices.ArrayWithOffset

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.AutomationProxyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000037:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 05 04 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool val) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x214f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AutomationProxyAttribute::.ctor

  .method /*06000029*/ public hidebysig specialname 
          instance bool  get_Value() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2157
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AutomationProxyAttribute::get_Value

  .property /*17000002*/ instance callconv(8) bool
          Value()
  {
    .get instance bool System.Runtime.InteropServices.AutomationProxyAttribute/*0200000D*/::get_Value() /* 06000029 */
  } // end of property AutomationProxyAttribute::Value
} // end of class System.Runtime.InteropServices.AutomationProxyAttribute

.class /*0200000E*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.BestFitMappingAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000038:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 0D 04 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .field /*04000007*/ public bool ThrowOnUnmappableChar
  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool BestFitMapping) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x215a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BestFitMappingAttribute::.ctor

  .method /*0600002B*/ public hidebysig specialname 
          instance bool  get_BestFitMapping() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2162
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BestFitMappingAttribute::get_BestFitMapping

  .property /*17000003*/ instance callconv(8) bool
          BestFitMapping()
  {
    .get instance bool System.Runtime.InteropServices.BestFitMappingAttribute/*0200000E*/::get_BestFitMapping() /* 0600002B */
  } // end of property BestFitMappingAttribute::BestFitMapping
} // end of class System.Runtime.InteropServices.BestFitMappingAttribute

.class /*0200000F*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.BStrWrapper
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C00003A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003C:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600002C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2165
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BStrWrapper::.ctor

  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x216d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BStrWrapper::.ctor

  .method /*0600002E*/ public hidebysig specialname 
          instance string  get_WrappedObject() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2175
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BStrWrapper::get_WrappedObject

  .property /*17000004*/ instance callconv(8) string
          WrappedObject()
  {
    .get instance string System.Runtime.InteropServices.BStrWrapper/*0200000F*/::get_WrappedObject() /* 0600002E */
  } // end of property BStrWrapper::WrappedObject
} // end of class System.Runtime.InteropServices.BStrWrapper

.class /*02000010*/ public auto ansi sealed System.Runtime.InteropServices.CallingConvention
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*04000008*/ public specialname rtspecialname int32 value__
  .field /*04000009*/ public static literal valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ Winapi = int32(0x00000001)
  .field /*0400000A*/ public static literal valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ Cdecl = int32(0x00000002)
  .field /*0400000B*/ public static literal valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ StdCall = int32(0x00000003)
  .field /*0400000C*/ public static literal valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ ThisCall = int32(0x00000004)
  .field /*0400000D*/ public static literal valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ FastCall = int32(0x00000005)
} // end of class System.Runtime.InteropServices.CallingConvention

.class /*02000011*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ClassInterfaceAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000043:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 05 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600002F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int16 classInterfaceType) cil managed
  // SIG: 20 01 01 06
  {
    // Method begins at RVA 0x2178
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ClassInterfaceAttribute::.ctor

  .method /*06000030*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.ClassInterfaceType/*02000012*/ classInterfaceType) cil managed
  // SIG: 20 01 01 11 48
  {
    // Method begins at RVA 0x2180
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ClassInterfaceAttribute::.ctor

  .method /*06000031*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.ClassInterfaceType/*02000012*/ 
          get_Value() cil managed
  // SIG: 20 00 11 48
  {
    // Method begins at RVA 0x2188
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClassInterfaceAttribute::get_Value

  .property /*17000005*/ instance callconv(8) valuetype System.Runtime.InteropServices.ClassInterfaceType/*02000012*/
          Value()
  {
    .get instance valuetype System.Runtime.InteropServices.ClassInterfaceType/*02000012*/ System.Runtime.InteropServices.ClassInterfaceAttribute/*02000011*/::get_Value() /* 06000031 */
  } // end of property ClassInterfaceAttribute::Value
} // end of class System.Runtime.InteropServices.ClassInterfaceAttribute

.class /*02000012*/ public auto ansi sealed System.Runtime.InteropServices.ClassInterfaceType
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*0400000E*/ public specialname rtspecialname int32 value__
  .field /*0400000F*/ public static literal valuetype System.Runtime.InteropServices.ClassInterfaceType/*02000012*/ None = int32(0x00000000)
  .field /*04000010*/ public static literal valuetype System.Runtime.InteropServices.ClassInterfaceType/*02000012*/ AutoDispatch = int32(0x00000001)
  .custom /*0C00003E:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00003F:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3C 53 75 70 70 6F 72 74 20 66 6F 72 20 49   // ..<Support for I
                                                                                                                                                 44 69 73 70 61 74 63 68 20 6D 61 79 20 62 65 20   // Dispatch may be 
                                                                                                                                                 75 6E 61 76 61 69 6C 61 62 6C 65 20 69 6E 20 66   // unavailable in f
                                                                                                                                                 75 74 75 72 65 20 72 65 6C 65 61 73 65 73 2E 00   // uture releases..
                                                                                                                                                 00 ) 
  .field /*04000011*/ public static literal valuetype System.Runtime.InteropServices.ClassInterfaceType/*02000012*/ AutoDual = int32(0x00000002)
  .custom /*0C000041:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000042:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3C 53 75 70 70 6F 72 74 20 66 6F 72 20 49   // ..<Support for I
                                                                                                                                                 44 69 73 70 61 74 63 68 20 6D 61 79 20 62 65 20   // Dispatch may be 
                                                                                                                                                 75 6E 61 76 61 69 6C 61 62 6C 65 20 69 6E 20 66   // unavailable in f
                                                                                                                                                 75 74 75 72 65 20 72 65 6C 65 61 73 65 73 2E 00   // uture releases..
                                                                                                                                                 00 ) 
} // end of class System.Runtime.InteropServices.ClassInterfaceType

.class /*02000013*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.CLong
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000026*/<valuetype System.Runtime.InteropServices.CLong/*02000013*/>/*1B000001*/
{
  .pack 0
  .size 1
  .custom /*0C000046:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000020*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .custom /*0C000047:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
  .method /*06000032*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x218b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CLong::.ctor

  .method /*06000033*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(native int 'value') cil managed
  // SIG: 20 01 01 18
  {
    .param [1]/*08000024*/ 
    .custom /*0C000074:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CLong::.ctor

  .method /*06000034*/ public hidebysig specialname 
          instance native int  get_Value() cil managed
  // SIG: 20 00 18
  {
    .param [0]/*08000025*/ 
    .custom /*0C000078:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CLong::get_Value

  .method /*06000035*/ public hidebysig virtual 
          instance bool  Equals(object o) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00009B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000026*/ 
    .custom /*0C00007A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000022*/::.ctor(bool) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2192
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CLong::Equals

  .method /*06000036*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Runtime.InteropServices.CLong/*02000013*/ other) cil managed
  // SIG: 20 01 02 11 4C
  {
    // Method begins at RVA 0x2195
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CLong::Equals

  .method /*06000037*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2198
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CLong::GetHashCode

  .method /*06000038*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x219b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CLong::ToString

  .property /*17000006*/ instance callconv(8) native int
          Value()
  {
    .custom /*0C000027:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    .get instance native int System.Runtime.InteropServices.CLong/*02000013*/::get_Value() /* 06000034 */
  } // end of property CLong::Value
} // end of class System.Runtime.InteropServices.CLong

.class /*02000014*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.CoClassAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00004A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00004C:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 04 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000039*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ coClass) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x219e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CoClassAttribute::.ctor

  .method /*0600003A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ 
          get_CoClass() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CoClassAttribute::get_CoClass

  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000027*/
          CoClass()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ System.Runtime.InteropServices.CoClassAttribute/*02000014*/::get_CoClass() /* 0600003A */
  } // end of property CoClassAttribute::CoClass
} // end of class System.Runtime.InteropServices.CoClassAttribute

.class /*02000015*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.CollectionsMarshal
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C00004E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600003B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Span`1/*01000028*/<!!T> 
          AsSpan<T>(class [System.Collections/*23000002*/]System.Collections.Generic.List`1/*01000029*/<!!T> list) cil managed
  // SIG: 10 01 01 15 11 80 A1 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0000A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000029*/ 
    .custom /*0C00007E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800002A*/ 
    .custom /*0C000082:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CollectionsMarshal::AsSpan

  .method /*0600003C*/ public hidebysig static 
          !!TValue&  GetValueRefOrNullRef<TKey,TValue>(class [System.Collections/*23000002*/]System.Collections.Generic.Dictionary`2/*0100002A*/<!!TKey,!!TValue> dictionary,
                                                       !!TKey key) cil managed
  // SIG: 10 02 02 10 1E 01 15 12 80 A9 02 1E 00 1E 01 1E 00
  {
    .param type TValue /*2A000003*/ 
      .custom /*0C00001B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CollectionsMarshal::GetValueRefOrNullRef

  .method /*0600003D*/ public hidebysig static 
          !!TValue&  GetValueRefOrAddDefault<TKey,TValue>(class [System.Collections/*23000002*/]System.Collections.Generic.Dictionary`2/*0100002A*/<!!TKey,!!TValue> dictionary,
                                                          !!TKey key,
                                                          [out] bool& exists) cil managed
  // SIG: 10 02 03 10 1E 01 15 12 80 A9 02 1E 00 1E 01 1E 00 10 02
  {
    .param type TValue /*2A000005*/ 
      .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800002D*/ 
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CollectionsMarshal::GetValueRefOrAddDefault

} // end of class System.Runtime.InteropServices.CollectionsMarshal

.class /*02000016*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComAliasNameAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000052:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000054:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 80 29 00 00 01 00 54 02 09 49 6E 68 65 72   // ...)....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600003E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string alias) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21b2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComAliasNameAttribute::.ctor

  .method /*0600003F*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAliasNameAttribute::get_Value

  .property /*17000008*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Runtime.InteropServices.ComAliasNameAttribute/*02000016*/::get_Value() /* 0600003F */
  } // end of property ComAliasNameAttribute::Value
} // end of class System.Runtime.InteropServices.ComAliasNameAttribute

.class /*02000017*/ public auto ansi beforefieldinit System.Runtime.InteropServices.ComAwareEventInfo
       extends [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100002B*/
{
  .custom /*0C000056:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000058:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000040*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ 'type',
                               string eventName) cil managed
  // SIG: 20 02 01 12 80 9D 0E
  {
    .param [1]/*08000032*/ 
    .custom /*0C000094:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A000018 */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x21bd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000024       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100002B*/::.ctor() /* 0A000024 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComAwareEventInfo::.ctor

  .method /*06000041*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.EventAttributes/*0100002E*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 80 B9
  {
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::get_Attributes

  .method /*06000042*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 80 9D
  {
    .custom /*0C0000B6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::get_DeclaringType

  .method /*06000043*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::get_MetadataToken

  .method /*06000044*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*0100002F*/ 
          get_Module() cil managed
  // SIG: 20 00 12 80 BD
  {
    // Method begins at RVA 0x21ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::get_Module

  .method /*06000045*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::get_Name

  .method /*06000046*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 80 9D
  {
    .custom /*0C0000C0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::get_ReflectedType

  .method /*06000047*/ public hidebysig virtual 
          instance void  AddEventHandler(object target,
                                         class [System.Runtime/*23000001*/]System.Delegate/*01000031*/ 'handler') cil managed
  // SIG: 20 02 01 1C 12 80 C5
  {
    .custom /*0C0000C3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x21d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ComAwareEventInfo::AddEventHandler

  .method /*06000048*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000032*/ 
          GetAddMethod(bool nonPublic) cil managed
  // SIG: 20 01 12 80 C9 02
  {
    .custom /*0C0000C5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::GetAddMethod

  .method /*06000049*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::GetCustomAttributes

  .method /*0600004A*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*01000027*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 80 9D 02
  {
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::GetCustomAttributes

  .method /*0600004B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000033*/<class [System.Runtime/*23000001*/]System.Reflection.CustomAttributeData/*01000034*/> 
          GetCustomAttributesData() cil managed
  // SIG: 20 00 15 12 80 CD 01 12 80 D1
  {
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::GetCustomAttributesData

  .method /*0600004C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000032*/[] 
          GetOtherMethods(bool nonPublic) cil managed
  // SIG: 20 01 1D 12 80 C9 02
  {
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::GetOtherMethods

  .method /*0600004D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000032*/ 
          GetRaiseMethod(bool nonPublic) cil managed
  // SIG: 20 01 12 80 C9 02
  {
    .custom /*0C0000CC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::GetRaiseMethod

  .method /*0600004E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000032*/ 
          GetRemoveMethod(bool nonPublic) cil managed
  // SIG: 20 01 12 80 C9 02
  {
    .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::GetRemoveMethod

  .method /*0600004F*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*01000027*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 80 9D 02
  {
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComAwareEventInfo::IsDefined

  .method /*06000050*/ public hidebysig virtual 
          instance void  RemoveEventHandler(object target,
                                            class [System.Runtime/*23000001*/]System.Delegate/*01000031*/ 'handler') cil managed
  // SIG: 20 02 01 1C 12 80 C5
  {
    .custom /*0C0000D4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x21f1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ComAwareEventInfo::RemoveEventHandler

  .property /*17000009*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.EventAttributes/*0100002E*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.EventAttributes/*0100002E*/ System.Runtime.InteropServices.ComAwareEventInfo/*02000017*/::get_Attributes() /* 06000041 */
  } // end of property ComAwareEventInfo::Attributes
  .property /*1700000A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000027*/
          DeclaringType()
  {
    .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ System.Runtime.InteropServices.ComAwareEventInfo/*02000017*/::get_DeclaringType() /* 06000042 */
  } // end of property ComAwareEventInfo::DeclaringType
  .property /*1700000B*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Runtime.InteropServices.ComAwareEventInfo/*02000017*/::get_MetadataToken() /* 06000043 */
  } // end of property ComAwareEventInfo::MetadataToken
  .property /*1700000C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*0100002F*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*0100002F*/ System.Runtime.InteropServices.ComAwareEventInfo/*02000017*/::get_Module() /* 06000044 */
  } // end of property ComAwareEventInfo::Module
  .property /*1700000D*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Runtime.InteropServices.ComAwareEventInfo/*02000017*/::get_Name() /* 06000045 */
  } // end of property ComAwareEventInfo::Name
  .property /*1700000E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000027*/
          ReflectedType()
  {
    .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ System.Runtime.InteropServices.ComAwareEventInfo/*02000017*/::get_ReflectedType() /* 06000046 */
  } // end of property ComAwareEventInfo::ReflectedType
} // end of class System.Runtime.InteropServices.ComAwareEventInfo

.class /*02000018*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComCompatibleVersionAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00005C:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 01 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 major,
                               int32 minor,
                               int32 build,
                               int32 revision) cil managed
  // SIG: 20 04 01 08 08 08 08
  {
    // Method begins at RVA 0x21f3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComCompatibleVersionAttribute::.ctor

  .method /*06000052*/ public hidebysig specialname 
          instance int32  get_BuildNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComCompatibleVersionAttribute::get_BuildNumber

  .method /*06000053*/ public hidebysig specialname 
          instance int32  get_MajorVersion() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComCompatibleVersionAttribute::get_MajorVersion

  .method /*06000054*/ public hidebysig specialname 
          instance int32  get_MinorVersion() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2201
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComCompatibleVersionAttribute::get_MinorVersion

  .method /*06000055*/ public hidebysig specialname 
          instance int32  get_RevisionNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2204
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComCompatibleVersionAttribute::get_RevisionNumber

  .property /*1700000F*/ instance callconv(8) int32
          BuildNumber()
  {
    .get instance int32 System.Runtime.InteropServices.ComCompatibleVersionAttribute/*02000018*/::get_BuildNumber() /* 06000052 */
  } // end of property ComCompatibleVersionAttribute::BuildNumber
  .property /*17000010*/ instance callconv(8) int32
          MajorVersion()
  {
    .get instance int32 System.Runtime.InteropServices.ComCompatibleVersionAttribute/*02000018*/::get_MajorVersion() /* 06000053 */
  } // end of property ComCompatibleVersionAttribute::MajorVersion
  .property /*17000011*/ instance callconv(8) int32
          MinorVersion()
  {
    .get instance int32 System.Runtime.InteropServices.ComCompatibleVersionAttribute/*02000018*/::get_MinorVersion() /* 06000054 */
  } // end of property ComCompatibleVersionAttribute::MinorVersion
  .property /*17000012*/ instance callconv(8) int32
          RevisionNumber()
  {
    .get instance int32 System.Runtime.InteropServices.ComCompatibleVersionAttribute/*02000018*/::get_RevisionNumber() /* 06000055 */
  } // end of property ComCompatibleVersionAttribute::RevisionNumber
} // end of class System.Runtime.InteropServices.ComCompatibleVersionAttribute

.class /*02000019*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComConversionLossAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00005D:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 FF 7F 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000056*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2207
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComConversionLossAttribute::.ctor

} // end of class System.Runtime.InteropServices.ComConversionLossAttribute

.class /*0200001A*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComDefaultInterfaceAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000060:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 04 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000057*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ defaultInterface) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x220f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComDefaultInterfaceAttribute::.ctor

  .method /*06000058*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ 
          get_Value() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComDefaultInterfaceAttribute::get_Value

  .property /*17000013*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000027*/
          Value()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ System.Runtime.InteropServices.ComDefaultInterfaceAttribute/*0200001A*/::get_Value() /* 06000058 */
  } // end of property ComDefaultInterfaceAttribute::Value
} // end of class System.Runtime.InteropServices.ComDefaultInterfaceAttribute

.class /*0200001B*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComEventInterfaceAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000061:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000063:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 04 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .custom /*0C000064:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000059*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ SourceInterface,
                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ EventProvider) cil managed
  // SIG: 20 02 01 12 80 9D 12 80 9D
  {
    .param [1]/*08000046*/ 
    .custom /*0C0000C2:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A000018 */ = ( 01 00 08 00 00 00 00 00 ) 
    .param [2]/*08000047*/ 
    .custom /*0C0000C4:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A000018 */ = ( 01 00 2F 0A 00 00 00 00 )                         // ../.....
    // Method begins at RVA 0x221a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComEventInterfaceAttribute::.ctor

  .method /*0600005A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ 
          get_EventProvider() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x2222
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComEventInterfaceAttribute::get_EventProvider

  .method /*0600005B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ 
          get_SourceInterface() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x2225
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComEventInterfaceAttribute::get_SourceInterface

  .property /*17000014*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000027*/
          EventProvider()
  {
    .custom /*0C00004D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A000018 */ = ( 01 00 2F 0A 00 00 00 00 )                         // ../.....
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ System.Runtime.InteropServices.ComEventInterfaceAttribute/*0200001B*/::get_EventProvider() /* 0600005A */
  } // end of property ComEventInterfaceAttribute::EventProvider
  .property /*17000015*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000027*/
          SourceInterface()
  {
    .custom /*0C000050:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A000018 */ = ( 01 00 08 00 00 00 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ System.Runtime.InteropServices.ComEventInterfaceAttribute/*0200001B*/::get_SourceInterface() /* 0600005B */
  } // end of property ComEventInterfaceAttribute::SourceInterface
} // end of class System.Runtime.InteropServices.ComEventInterfaceAttribute

.class /*0200001C*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComEventsHelper
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000067:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .custom /*0C000068:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600005C*/ public hidebysig static 
          void  Combine(object rcw,
                        valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ iid,
                        int32 dispid,
                        class [System.Runtime/*23000001*/]System.Delegate/*01000031*/ d) cil managed
  // SIG: 00 04 01 1C 11 80 D5 08 12 80 C5
  {
    // Method begins at RVA 0x2228
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ComEventsHelper::Combine

  .method /*0600005D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Delegate/*01000031*/ 
          Remove(object rcw,
                 valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ iid,
                 int32 dispid,
                 class [System.Runtime/*23000001*/]System.Delegate/*01000031*/ d) cil managed
  // SIG: 00 04 12 80 C5 1C 11 80 D5 08 12 80 C5
  {
    .param [0]/*0800004C*/ 
    .custom /*0C0000CB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComEventsHelper::Remove

} // end of class System.Runtime.InteropServices.ComEventsHelper

.class /*0200001D*/ public auto ansi beforefieldinit System.Runtime.InteropServices.COMException
       extends [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/
{
  .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x222d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method COMException::.ctor

  .method /*0600005F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C0000E9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2235
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method COMException::.ctor

  .method /*06000060*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x223d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method COMException::.ctor

  .method /*06000061*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ inner) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x2245
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method COMException::.ctor

  .method /*06000062*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               int32 errorCode) cil managed
  // SIG: 20 02 01 0E 08
  {
    // Method begins at RVA 0x224d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method COMException::.ctor

  .method /*06000063*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000F6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2255
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method COMException::ToString

} // end of class System.Runtime.InteropServices.COMException

.class /*0200001E*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComImportAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00006B:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 04 04 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2258
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComImportAttribute::.ctor

} // end of class System.Runtime.InteropServices.ComImportAttribute

.class /*0200001F*/ public auto ansi sealed System.Runtime.InteropServices.ComInterfaceType
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*04000012*/ public specialname rtspecialname int32 value__
  .field /*04000013*/ public static literal valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/ InterfaceIsDual = int32(0x00000000)
  .field /*04000014*/ public static literal valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/ InterfaceIsIUnknown = int32(0x00000001)
  .field /*04000015*/ public static literal valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/ InterfaceIsIDispatch = int32(0x00000002)
  .field /*04000016*/ public static literal valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/ InterfaceIsIInspectable = int32(0x00000003)
} // end of class System.Runtime.InteropServices.ComInterfaceType

.class /*02000020*/ public auto ansi sealed System.Runtime.InteropServices.ComMemberType
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*04000017*/ public specialname rtspecialname int32 value__
  .field /*04000018*/ public static literal valuetype System.Runtime.InteropServices.ComMemberType/*02000020*/ Method = int32(0x00000000)
  .field /*04000019*/ public static literal valuetype System.Runtime.InteropServices.ComMemberType/*02000020*/ PropGet = int32(0x00000001)
  .field /*0400001A*/ public static literal valuetype System.Runtime.InteropServices.ComMemberType/*02000020*/ PropSet = int32(0x00000002)
} // end of class System.Runtime.InteropServices.ComMemberType

.class /*02000021*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComRegisterFunctionAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00006C:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ..@.....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000065*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2260
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComRegisterFunctionAttribute::.ctor

} // end of class System.Runtime.InteropServices.ComRegisterFunctionAttribute

.class /*02000022*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComSourceInterfacesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00006F:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 04 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 01 )                                  // ited.
  .custom /*0C000070:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000066*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string sourceInterfaces) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2268
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComSourceInterfacesAttribute::.ctor

  .method /*06000067*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2270
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComSourceInterfacesAttribute::.ctor

  .method /*06000068*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface1,
                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface2) cil managed
  // SIG: 20 02 01 12 80 9D 12 80 9D
  {
    // Method begins at RVA 0x2278
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComSourceInterfacesAttribute::.ctor

  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface1,
                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface2,
                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface3) cil managed
  // SIG: 20 03 01 12 80 9D 12 80 9D 12 80 9D
  {
    // Method begins at RVA 0x2280
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComSourceInterfacesAttribute::.ctor

  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface1,
                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface2,
                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface3,
                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ sourceInterface4) cil managed
  // SIG: 20 04 01 12 80 9D 12 80 9D 12 80 9D 12 80 9D
  {
    // Method begins at RVA 0x2288
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComSourceInterfacesAttribute::.ctor

  .method /*0600006B*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2290
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComSourceInterfacesAttribute::get_Value

  .property /*17000016*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Runtime.InteropServices.ComSourceInterfacesAttribute/*02000022*/::get_Value() /* 0600006B */
  } // end of property ComSourceInterfacesAttribute::Value
} // end of class System.Runtime.InteropServices.ComSourceInterfacesAttribute

.class /*02000023*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ComUnregisterFunctionAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000071:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ..@.....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600006C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2293
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComUnregisterFunctionAttribute::.ctor

} // end of class System.Runtime.InteropServices.ComUnregisterFunctionAttribute

.class /*02000024*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.CULong
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000026*/<valuetype System.Runtime.InteropServices.CULong/*02000024*/>/*1B000002*/
{
  .pack 0
  .size 1
  .custom /*0C000072:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000020*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .custom /*0C000073:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
  .method /*0600006D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint32 'value') cil managed
  // SIG: 20 01 01 09
  {
    // Method begins at RVA 0x229b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CULong::.ctor

  .method /*0600006E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(native uint 'value') cil managed
  // SIG: 20 01 01 19
  {
    .param [1]/*08000064*/ 
    .custom /*0C0000F9:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CULong::.ctor

  .method /*0600006F*/ public hidebysig specialname 
          instance native uint  get_Value() cil managed
  // SIG: 20 00 19
  {
    .param [0]/*08000065*/ 
    .custom /*0C0000FC:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CULong::get_Value

  .method /*06000070*/ public hidebysig virtual 
          instance bool  Equals(object o) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00010B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000066*/ 
    .custom /*0C0000FF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000022*/::.ctor(bool) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CULong::Equals

  .method /*06000071*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Runtime.InteropServices.CULong/*02000024*/ other) cil managed
  // SIG: 20 01 02 11 80 90
  {
    // Method begins at RVA 0x22a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CULong::Equals

  .method /*06000072*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CULong::GetHashCode

  .method /*06000073*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CULong::ToString

  .property /*17000017*/ instance callconv(8) native uint
          Value()
  {
    .custom /*0C00005A:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    .get instance native uint System.Runtime.InteropServices.CULong/*02000024*/::get_Value() /* 0600006F */
  } // end of property CULong::Value
} // end of class System.Runtime.InteropServices.CULong

.class /*02000025*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.CurrencyWrapper
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000076:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000077:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 66 43 75 72 72 65 6E 63 79 57 72 61 70 70   // ..fCurrencyWrapp
                                                                                                                                                 65 72 20 61 6E 64 20 73 75 70 70 6F 72 74 20 66   // er and support f
                                                                                                                                                 6F 72 20 6D 61 72 73 68 61 6C 6C 69 6E 67 20 74   // or marshalling t
                                                                                                                                                 6F 20 74 68 65 20 56 41 52 49 41 4E 54 20 74 79   // o the VARIANT ty
                                                                                                                                                 70 65 20 6D 61 79 20 62 65 20 75 6E 61 76 61 69   // pe may be unavai
                                                                                                                                                 6C 61 62 6C 65 20 69 6E 20 66 75 74 75 72 65 20   // lable in future 
                                                                                                                                                 72 65 6C 65 61 73 65 73 2E 00 00 )                // releases...
  .method /*06000074*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Decimal/*01000037*/ obj) cil managed
  // SIG: 20 01 01 11 80 DD
  {
    // Method begins at RVA 0x22ae
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CurrencyWrapper::.ctor

  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object obj) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000114:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22b6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CurrencyWrapper::.ctor

  .method /*06000076*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Decimal/*01000037*/ 
          get_WrappedObject() cil managed
  // SIG: 20 00 11 80 DD
  {
    // Method begins at RVA 0x22be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CurrencyWrapper::get_WrappedObject

  .property /*17000018*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Decimal/*01000037*/
          WrappedObject()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Decimal/*01000037*/ System.Runtime.InteropServices.CurrencyWrapper/*02000025*/::get_WrappedObject() /* 06000076 */
  } // end of property CurrencyWrapper::WrappedObject
} // end of class System.Runtime.InteropServices.CurrencyWrapper

.class /*02000026*/ public auto ansi sealed System.Runtime.InteropServices.CustomQueryInterfaceMode
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000079:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400001B*/ public specialname rtspecialname int32 value__
  .field /*0400001C*/ public static literal valuetype System.Runtime.InteropServices.CustomQueryInterfaceMode/*02000026*/ Ignore = int32(0x00000000)
  .field /*0400001D*/ public static literal valuetype System.Runtime.InteropServices.CustomQueryInterfaceMode/*02000026*/ Allow = int32(0x00000001)
} // end of class System.Runtime.InteropServices.CustomQueryInterfaceMode

.class /*02000027*/ public auto ansi sealed System.Runtime.InteropServices.CustomQueryInterfaceResult
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C00007B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400001E*/ public specialname rtspecialname int32 value__
  .field /*0400001F*/ public static literal valuetype System.Runtime.InteropServices.CustomQueryInterfaceResult/*02000027*/ Handled = int32(0x00000000)
  .field /*04000020*/ public static literal valuetype System.Runtime.InteropServices.CustomQueryInterfaceResult/*02000027*/ NotHandled = int32(0x00000001)
  .field /*04000021*/ public static literal valuetype System.Runtime.InteropServices.CustomQueryInterfaceResult/*02000027*/ Failed = int32(0x00000002)
} // end of class System.Runtime.InteropServices.CustomQueryInterfaceResult

.class /*02000028*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.DefaultCharSetAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00007C:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 02 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000038*/ charSet) cil managed
  // SIG: 20 01 01 11 80 E1
  {
    // Method begins at RVA 0x22c1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DefaultCharSetAttribute::.ctor

  .method /*06000078*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000038*/ 
          get_CharSet() cil managed
  // SIG: 20 00 11 80 E1
  {
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultCharSetAttribute::get_CharSet

  .property /*17000019*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000038*/
          CharSet()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000038*/ System.Runtime.InteropServices.DefaultCharSetAttribute/*02000028*/::get_CharSet() /* 06000078 */
  } // end of property DefaultCharSetAttribute::CharSet
} // end of class System.Runtime.InteropServices.DefaultCharSetAttribute

.class /*02000029*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.DefaultDllImportSearchPathsAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00007D:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 41 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ..A.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 )                      // Multiple.
  .method /*06000079*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ paths) cil managed
  // SIG: 20 01 01 11 80 BC
  {
    // Method begins at RVA 0x22cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DefaultDllImportSearchPathsAttribute::.ctor

  .method /*0600007A*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ 
          get_Paths() cil managed
  // SIG: 20 00 11 80 BC
  {
    // Method begins at RVA 0x22d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultDllImportSearchPathsAttribute::get_Paths

  .property /*1700001A*/ instance callconv(8) valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/
          Paths()
  {
    .get instance valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ System.Runtime.InteropServices.DefaultDllImportSearchPathsAttribute/*02000029*/::get_Paths() /* 0600007A */
  } // end of property DefaultDllImportSearchPathsAttribute::Paths
} // end of class System.Runtime.InteropServices.DefaultDllImportSearchPathsAttribute

.class /*0200002A*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.DefaultParameterValueAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00007F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000080:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000081:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 08 00 00 00 00 ) 
  .method /*0600007B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x22d7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DefaultParameterValueAttribute::.ctor

  .method /*0600007C*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x22df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultParameterValueAttribute::get_Value

  .property /*1700001B*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Runtime.InteropServices.DefaultParameterValueAttribute/*0200002A*/::get_Value() /* 0600007C */
  } // end of property DefaultParameterValueAttribute::Value
} // end of class System.Runtime.InteropServices.DefaultParameterValueAttribute

.class /*0200002B*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.DispatchWrapper
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000083:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000085:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .custom /*0C000086:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600007D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object obj) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x22e2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DispatchWrapper::.ctor

  .method /*0600007E*/ public hidebysig specialname 
          instance object  get_WrappedObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x22ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DispatchWrapper::get_WrappedObject

  .property /*1700001C*/ instance callconv(8) object
          WrappedObject()
  {
    .get instance object System.Runtime.InteropServices.DispatchWrapper/*0200002B*/::get_WrappedObject() /* 0600007E */
  } // end of property DispatchWrapper::WrappedObject
} // end of class System.Runtime.InteropServices.DispatchWrapper

.class /*0200002C*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.DispIdAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000087:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 C0 03 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600007F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dispId) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22ed
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DispIdAttribute::.ctor

  .method /*06000080*/ public hidebysig specialname 
          instance int32  get_Value() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DispIdAttribute::get_Value

  .property /*1700001D*/ instance callconv(8) int32
          Value()
  {
    .get instance int32 System.Runtime.InteropServices.DispIdAttribute/*0200002C*/::get_Value() /* 06000080 */
  } // end of property DispIdAttribute::Value
} // end of class System.Runtime.InteropServices.DispIdAttribute

.class /*0200002D*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.DllImportAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000088:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00008A:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ..@.....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .field /*04000022*/ public bool BestFitMapping
  .field /*04000023*/ public valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ CallingConvention
  .field /*04000024*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000038*/ CharSet
  .field /*04000025*/ public string EntryPoint
  .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .field /*04000026*/ public bool ExactSpelling
  .field /*04000027*/ public bool PreserveSig
  .field /*04000028*/ public bool SetLastError
  .field /*04000029*/ public bool ThrowOnUnmappableChar
  .method /*06000081*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string dllName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22f8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DllImportAttribute::.ctor

  .method /*06000082*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2300
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DllImportAttribute::get_Value

  .property /*1700001E*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Runtime.InteropServices.DllImportAttribute/*0200002D*/::get_Value() /* 06000082 */
  } // end of property DllImportAttribute::Value
} // end of class System.Runtime.InteropServices.DllImportAttribute

.class /*0200002E*/ public auto ansi sealed System.Runtime.InteropServices.DllImportResolver
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000039*/
{
  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method DllImportResolver::.ctor

  .method /*06000084*/ public hidebysig newslot virtual 
          instance native int  Invoke(string libraryName,
                                      class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003A*/ 'assembly',
                                      valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003B*/<valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/> searchPath) runtime managed
  // SIG: 20 03 18 0E 12 80 E9 15 11 80 ED 01 11 80 BC
  {
    .custom /*0C00013C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  } // end of method DllImportResolver::Invoke

  .method /*06000085*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003C*/ 
          BeginInvoke(string libraryName,
                      class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003A*/ 'assembly',
                      valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003B*/<valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/> searchPath,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100003D*/ callback,
                      object 'object') runtime managed
  // SIG: 20 05 12 80 F1 0E 12 80 E9 15 11 80 ED 01 11 80 BC 12 80 F5 1C
  {
    .param [1]/*08000075*/ 
    .custom /*0C000117:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000076*/ 
    .custom /*0C000119:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method DllImportResolver::BeginInvoke

  .method /*06000086*/ public hidebysig newslot virtual 
          instance native int  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003C*/ result) runtime managed
  // SIG: 20 01 18 12 80 F1
  {
  } // end of method DllImportResolver::EndInvoke

} // end of class System.Runtime.InteropServices.DllImportResolver

.class /*0200002F*/ public auto ansi sealed System.Runtime.InteropServices.DllImportSearchPath
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C00008C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*0400002A*/ public specialname rtspecialname int32 value__
  .field /*0400002B*/ public static literal valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ LegacyBehavior = int32(0x00000000)
  .field /*0400002C*/ public static literal valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ AssemblyDirectory = int32(0x00000002)
  .field /*0400002D*/ public static literal valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ UseDllDirectoryForDependencies = int32(0x00000100)
  .field /*0400002E*/ public static literal valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ ApplicationDirectory = int32(0x00000200)
  .field /*0400002F*/ public static literal valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ UserDirectories = int32(0x00000400)
  .field /*04000030*/ public static literal valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ System32 = int32(0x00000800)
  .field /*04000031*/ public static literal valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/ SafeDirectories = int32(0x00001000)
} // end of class System.Runtime.InteropServices.DllImportSearchPath

.class /*02000030*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.DynamicInterfaceCastableImplementationAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00008D:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 04 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .method /*06000087*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2303
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicInterfaceCastableImplementationAttribute::.ctor

} // end of class System.Runtime.InteropServices.DynamicInterfaceCastableImplementationAttribute

.class /*02000031*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ErrorWrapper
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000090:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000088*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ e) cil managed
  // SIG: 20 01 01 12 65
  {
    // Method begins at RVA 0x230b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ErrorWrapper::.ctor

  .method /*06000089*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 errorCode) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2313
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ErrorWrapper::.ctor

  .method /*0600008A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object errorCode) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x231b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ErrorWrapper::.ctor

  .method /*0600008B*/ public hidebysig specialname 
          instance int32  get_ErrorCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2323
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorWrapper::get_ErrorCode

  .property /*1700001F*/ instance callconv(8) int32
          ErrorCode()
  {
    .get instance int32 System.Runtime.InteropServices.ErrorWrapper/*02000031*/::get_ErrorCode() /* 0600008B */
  } // end of property ErrorWrapper::ErrorCode
} // end of class System.Runtime.InteropServices.ErrorWrapper

.class /*02000032*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.GuidAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000091:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000093:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 1D 14 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600008C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string guid) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2326
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GuidAttribute::.ctor

  .method /*0600008D*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x232e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GuidAttribute::get_Value

  .property /*17000020*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Runtime.InteropServices.GuidAttribute/*02000032*/::get_Value() /* 0600008D */
  } // end of property GuidAttribute::Value
} // end of class System.Runtime.InteropServices.GuidAttribute

.class /*02000033*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.HandleCollector
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000096:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600008E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               int32 initialThreshold) cil managed
  // SIG: 20 02 01 0E 08
  {
    .custom /*0C00015D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2331
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HandleCollector::.ctor

  .method /*0600008F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               int32 initialThreshold,
                               int32 maximumThreshold) cil managed
  // SIG: 20 03 01 0E 08 08
  {
    .custom /*0C000161:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2339
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HandleCollector::.ctor

  .method /*06000090*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2341
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleCollector::get_Count

  .method /*06000091*/ public hidebysig specialname 
          instance int32  get_InitialThreshold() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2344
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleCollector::get_InitialThreshold

  .method /*06000092*/ public hidebysig specialname 
          instance int32  get_MaximumThreshold() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2347
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleCollector::get_MaximumThreshold

  .method /*06000093*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x234a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleCollector::get_Name

  .method /*06000094*/ public hidebysig instance void 
          Add() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x234d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HandleCollector::Add

  .method /*06000095*/ public hidebysig instance void 
          Remove() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x234f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HandleCollector::Remove

  .property /*17000021*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Runtime.InteropServices.HandleCollector/*02000033*/::get_Count() /* 06000090 */
  } // end of property HandleCollector::Count
  .property /*17000022*/ instance callconv(8) int32
          InitialThreshold()
  {
    .get instance int32 System.Runtime.InteropServices.HandleCollector/*02000033*/::get_InitialThreshold() /* 06000091 */
  } // end of property HandleCollector::InitialThreshold
  .property /*17000023*/ instance callconv(8) int32
          MaximumThreshold()
  {
    .get instance int32 System.Runtime.InteropServices.HandleCollector/*02000033*/::get_MaximumThreshold() /* 06000092 */
  } // end of property HandleCollector::MaximumThreshold
  .property /*17000024*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Runtime.InteropServices.HandleCollector/*02000033*/::get_Name() /* 06000093 */
  } // end of property HandleCollector::Name
} // end of class System.Runtime.InteropServices.HandleCollector

.class /*02000034*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.HandleRef
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000098:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000099:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00009A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000020*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .field /*04000032*/ private initonly object _dummy
  .field /*04000033*/ private initonly int32 _dummyPrimitive
  .method /*06000096*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object wrapper,
                               native int handle) cil managed
  // SIG: 20 02 01 1C 18
  {
    // Method begins at RVA 0x2351
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleRef::.ctor

  .method /*06000097*/ public hidebysig specialname 
          instance native int  get_Handle() cil managed
  // SIG: 20 00 18
  {
    // Method begins at RVA 0x2354
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleRef::get_Handle

  .method /*06000098*/ public hidebysig specialname 
          instance object  get_Wrapper() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2357
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleRef::get_Wrapper

  .method /*06000099*/ public hidebysig specialname static 
          native int  op_Explicit(valuetype System.Runtime.InteropServices.HandleRef/*02000034*/ 'value') cil managed
  // SIG: 00 01 18 11 80 D0
  {
    // Method begins at RVA 0x235a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleRef::op_Explicit

  .method /*0600009A*/ public hidebysig static 
          native int  ToIntPtr(valuetype System.Runtime.InteropServices.HandleRef/*02000034*/ 'value') cil managed
  // SIG: 00 01 18 11 80 D0
  {
    // Method begins at RVA 0x235d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandleRef::ToIntPtr

  .property /*17000025*/ instance callconv(8) native int
          Handle()
  {
    .get instance native int System.Runtime.InteropServices.HandleRef/*02000034*/::get_Handle() /* 06000097 */
  } // end of property HandleRef::Handle
  .property /*17000026*/ instance callconv(8) object
          Wrapper()
  {
    .get instance object System.Runtime.InteropServices.HandleRef/*02000034*/::get_Wrapper() /* 06000098 */
  } // end of property HandleRef::Wrapper
} // end of class System.Runtime.InteropServices.HandleRef

.class /*02000035*/ interface public abstract auto ansi System.Runtime.InteropServices.ICustomAdapter
{
  .custom /*0C00009C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009D:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600009B*/ public hidebysig newslot abstract virtual 
          instance object  GetUnderlyingObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method ICustomAdapter::GetUnderlyingObject

} // end of class System.Runtime.InteropServices.ICustomAdapter

.class /*02000036*/ interface public abstract auto ansi System.Runtime.InteropServices.ICustomFactory
{
  .custom /*0C00009E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600009C*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003F*/ 
          CreateInstance(class [System.Runtime/*23000001*/]System.Type/*01000027*/ serverType) cil managed
  // SIG: 20 01 12 80 FD 12 80 9D
  {
    // Method begins at RVA 0x0
  } // end of method ICustomFactory::CreateInstance

} // end of class System.Runtime.InteropServices.ICustomFactory

.class /*02000037*/ interface public abstract auto ansi System.Runtime.InteropServices.ICustomMarshaler
{
  .custom /*0C00009F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600009D*/ public hidebysig newslot abstract virtual 
          instance void  CleanUpManagedData(object ManagedObj) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x0
  } // end of method ICustomMarshaler::CleanUpManagedData

  .method /*0600009E*/ public hidebysig newslot abstract virtual 
          instance void  CleanUpNativeData(native int pNativeData) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ICustomMarshaler::CleanUpNativeData

  .method /*0600009F*/ public hidebysig newslot abstract virtual 
          instance int32  GetNativeDataSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ICustomMarshaler::GetNativeDataSize

  .method /*060000A0*/ public hidebysig newslot abstract virtual 
          instance native int  MarshalManagedToNative(object ManagedObj) cil managed
  // SIG: 20 01 18 1C
  {
    // Method begins at RVA 0x0
  } // end of method ICustomMarshaler::MarshalManagedToNative

  .method /*060000A1*/ public hidebysig newslot abstract virtual 
          instance object  MarshalNativeToManaged(native int pNativeData) cil managed
  // SIG: 20 01 1C 18
  {
    // Method begins at RVA 0x0
  } // end of method ICustomMarshaler::MarshalNativeToManaged

} // end of class System.Runtime.InteropServices.ICustomMarshaler

.class /*02000038*/ interface public abstract auto ansi System.Runtime.InteropServices.ICustomQueryInterface
{
  .custom /*0C0000A1:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060000A2*/ public hidebysig newslot abstract virtual 
          instance valuetype System.Runtime.InteropServices.CustomQueryInterfaceResult/*02000027*/ 
          GetInterface(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& iid,
                       [out] native int& ppv) cil managed
  // SIG: 20 02 11 80 9C 10 11 80 D5 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ICustomQueryInterface::GetInterface

} // end of class System.Runtime.InteropServices.ICustomQueryInterface

.class /*02000039*/ interface public abstract auto ansi System.Runtime.InteropServices.IDynamicInterfaceCastable
{
  .method /*060000A3*/ public hidebysig newslot abstract virtual 
          instance bool  IsInterfaceImplemented(valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000040*/ interfaceType,
                                                bool throwIfNotImplemented) cil managed
  // SIG: 20 02 02 11 81 01 02
  {
    // Method begins at RVA 0x0
  } // end of method IDynamicInterfaceCastable::IsInterfaceImplemented

  .method /*060000A4*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000040*/ 
          GetInterfaceImplementation(valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000040*/ interfaceType) cil managed
  // SIG: 20 01 11 81 01 11 81 01
  {
    // Method begins at RVA 0x0
  } // end of method IDynamicInterfaceCastable::GetInterfaceImplementation

} // end of class System.Runtime.InteropServices.IDynamicInterfaceCastable

.class /*0200003A*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ImportedFromTypeLibAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000A4:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 01 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*060000A5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string tlbFile) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2360
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ImportedFromTypeLibAttribute::.ctor

  .method /*060000A6*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2368
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ImportedFromTypeLibAttribute::get_Value

  .property /*17000027*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Runtime.InteropServices.ImportedFromTypeLibAttribute/*0200003A*/::get_Value() /* 060000A6 */
  } // end of property ImportedFromTypeLibAttribute::Value
} // end of class System.Runtime.InteropServices.ImportedFromTypeLibAttribute

.class /*0200003B*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.InterfaceTypeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000A6:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 04 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*060000A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int16 interfaceType) cil managed
  // SIG: 20 01 01 06
  {
    // Method begins at RVA 0x236b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InterfaceTypeAttribute::.ctor

  .method /*060000A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/ interfaceType) cil managed
  // SIG: 20 01 01 11 7C
  {
    // Method begins at RVA 0x2373
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InterfaceTypeAttribute::.ctor

  .method /*060000A9*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/ 
          get_Value() cil managed
  // SIG: 20 00 11 7C
  {
    // Method begins at RVA 0x237b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InterfaceTypeAttribute::get_Value

  .property /*17000028*/ instance callconv(8) valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/
          Value()
  {
    .get instance valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/ System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::get_Value() /* 060000A9 */
  } // end of property InterfaceTypeAttribute::Value
} // end of class System.Runtime.InteropServices.InterfaceTypeAttribute

.class /*0200003C*/ public auto ansi beforefieldinit System.Runtime.InteropServices.InvalidComObjectException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000018*/
{
  .custom /*0C0000A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x237e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidComObjectException::.ctor

  .method /*060000AB*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C00018D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2386
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidComObjectException::.ctor

  .method /*060000AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x238e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidComObjectException::.ctor

  .method /*060000AD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ inner) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x2396
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidComObjectException::.ctor

} // end of class System.Runtime.InteropServices.InvalidComObjectException

.class /*0200003D*/ public auto ansi beforefieldinit System.Runtime.InteropServices.InvalidOleVariantTypeException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000018*/
{
  .custom /*0C0000A9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000AE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x239e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidOleVariantTypeException::.ctor

  .method /*060000AF*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C00018E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23a6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidOleVariantTypeException::.ctor

  .method /*060000B0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23ae
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidOleVariantTypeException::.ctor

  .method /*060000B1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ inner) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x23b6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidOleVariantTypeException::.ctor

} // end of class System.Runtime.InteropServices.InvalidOleVariantTypeException

.class /*0200003E*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.LCIDConversionAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000AB:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ..@.....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*060000B2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 lcid) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x23be
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LCIDConversionAttribute::.ctor

  .method /*060000B3*/ public hidebysig specialname 
          instance int32  get_Value() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LCIDConversionAttribute::get_Value

  .property /*17000029*/ instance callconv(8) int32
          Value()
  {
    .get instance int32 System.Runtime.InteropServices.LCIDConversionAttribute/*0200003E*/::get_Value() /* 060000B3 */
  } // end of property LCIDConversionAttribute::Value
} // end of class System.Runtime.InteropServices.LCIDConversionAttribute

.class /*0200003F*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ManagedToNativeComInteropStubAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000AE:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 02 00 54 02 09 49 6E 68 65 72   // ..@.....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                                          74 69 70 6C 65 00 )                               // tiple.
  .method /*060000B4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ classType,
                               string methodName) cil managed
  // SIG: 20 02 01 12 80 9D 0E
  {
    // Method begins at RVA 0x23c9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ManagedToNativeComInteropStubAttribute::.ctor

  .method /*060000B5*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ 
          get_ClassType() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x23d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManagedToNativeComInteropStubAttribute::get_ClassType

  .method /*060000B6*/ public hidebysig specialname 
          instance string  get_MethodName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManagedToNativeComInteropStubAttribute::get_MethodName

  .property /*1700002A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000027*/
          ClassType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000027*/ System.Runtime.InteropServices.ManagedToNativeComInteropStubAttribute/*0200003F*/::get_ClassType() /* 060000B5 */
  } // end of property ManagedToNativeComInteropStubAttribute::ClassType
  .property /*1700002B*/ instance callconv(8) string
          MethodName()
  {
    .get instance string System.Runtime.InteropServices.ManagedToNativeComInteropStubAttribute/*0200003F*/::get_MethodName() /* 060000B6 */
  } // end of property ManagedToNativeComInteropStubAttribute::MethodName
} // end of class System.Runtime.InteropServices.ManagedToNativeComInteropStubAttribute

.class /*02000040*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.Marshal
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000034*/ public static initonly int32 SystemDefaultCharSize
  .field /*04000035*/ public static initonly int32 SystemMaxDBCSCharSize
  .method /*060000B7*/ public hidebysig static 
          int32  AddRef(native int pUnk) cil managed
  // SIG: 00 01 08 18
  {
    // Method begins at RVA 0x23d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::AddRef

  .method /*060000B8*/ public hidebysig static 
          native int  AllocCoTaskMem(int32 cb) cil managed
  // SIG: 00 01 18 08
  {
    // Method begins at RVA 0x23da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::AllocCoTaskMem

  .method /*060000B9*/ public hidebysig static 
          native int  AllocHGlobal(int32 cb) cil managed
  // SIG: 00 01 18 08
  {
    // Method begins at RVA 0x23dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::AllocHGlobal

  .method /*060000BA*/ public hidebysig static 
          native int  AllocHGlobal(native int cb) cil managed
  // SIG: 00 01 18 18
  {
    // Method begins at RVA 0x23e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::AllocHGlobal

  .method /*060000BB*/ public hidebysig static 
          bool  AreComObjectsAvailableForCleanup() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x23e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::AreComObjectsAvailableForCleanup

  .method /*060000BC*/ public hidebysig static 
          object  BindToMoniker(string monikerName) cil managed
  // SIG: 00 01 1C 0E
  {
    .custom /*0C00018F:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000041*/::.ctor(string) /* 0A00001B */ = ( 01 00 2B 42 75 69 6C 74 2D 69 6E 20 43 4F 4D 20   // ..+Built-in COM 
                                                                                                                                                                                            73 75 70 70 6F 72 74 20 69 73 20 6E 6F 74 20 74   // support is not t
                                                                                                                                                                                            72 69 6D 20 63 6F 6D 70 61 74 69 62 6C 65 01 00   // rim compatible..
                                                                                                                                                                                            54 0E 03 55 72 6C 20 68 74 74 70 73 3A 2F 2F 61   // T..Url https://a
                                                                                                                                                                                            6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 69 6C 6C   // ka.ms/dotnet-ill
                                                                                                                                                                                            69 6E 6B 2F 63 6F 6D )                            // ink/com
    .custom /*0C000190:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x23e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::BindToMoniker

  .method /*060000BD*/ public hidebysig static 
          void  ChangeWrapperHandleStrength(object otp,
                                            bool fIsWeak) cil managed
  // SIG: 00 02 01 1C 02
  {
    .custom /*0C000191:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x23e9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ChangeWrapperHandleStrength

  .method /*060000BE*/ public hidebysig static 
          void  CleanupUnusedObjectsInCurrentContext() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x23eb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::CleanupUnusedObjectsInCurrentContext

  .method /*060000BF*/ public hidebysig static 
          void  Copy(uint8[] source,
                     int32 startIndex,
                     native int destination,
                     int32 length) cil managed
  // SIG: 00 04 01 1D 05 08 18 08
  {
    // Method begins at RVA 0x23ed
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C0*/ public hidebysig static 
          void  Copy(char[] source,
                     int32 startIndex,
                     native int destination,
                     int32 length) cil managed
  // SIG: 00 04 01 1D 03 08 18 08
  {
    // Method begins at RVA 0x23ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C1*/ public hidebysig static 
          void  Copy(float64[] source,
                     int32 startIndex,
                     native int destination,
                     int32 length) cil managed
  // SIG: 00 04 01 1D 0D 08 18 08
  {
    // Method begins at RVA 0x23f1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C2*/ public hidebysig static 
          void  Copy(int16[] source,
                     int32 startIndex,
                     native int destination,
                     int32 length) cil managed
  // SIG: 00 04 01 1D 06 08 18 08
  {
    // Method begins at RVA 0x23f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C3*/ public hidebysig static 
          void  Copy(int32[] source,
                     int32 startIndex,
                     native int destination,
                     int32 length) cil managed
  // SIG: 00 04 01 1D 08 08 18 08
  {
    // Method begins at RVA 0x23f5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C4*/ public hidebysig static 
          void  Copy(int64[] source,
                     int32 startIndex,
                     native int destination,
                     int32 length) cil managed
  // SIG: 00 04 01 1D 0A 08 18 08
  {
    // Method begins at RVA 0x23f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C5*/ public hidebysig static 
          void  Copy(native int source,
                     uint8[] destination,
                     int32 startIndex,
                     int32 length) cil managed
  // SIG: 00 04 01 18 1D 05 08 08
  {
    // Method begins at RVA 0x23f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C6*/ public hidebysig static 
          void  Copy(native int source,
                     char[] destination,
                     int32 startIndex,
                     int32 length) cil managed
  // SIG: 00 04 01 18 1D 03 08 08
  {
    // Method begins at RVA 0x23fb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C7*/ public hidebysig static 
          void  Copy(native int source,
                     float64[] destination,
                     int32 startIndex,
                     int32 length) cil managed
  // SIG: 00 04 01 18 1D 0D 08 08
  {
    // Method begins at RVA 0x23fd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C8*/ public hidebysig static 
          void  Copy(native int source,
                     int16[] destination,
                     int32 startIndex,
                     int32 length) cil managed
  // SIG: 00 04 01 18 1D 06 08 08
  {
    // Method begins at RVA 0x23ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000C9*/ public hidebysig static 
          void  Copy(native int source,
                     int32[] destination,
                     int32 startIndex,
                     int32 length) cil managed
  // SIG: 00 04 01 18 1D 08 08 08
  {
    // Method begins at RVA 0x2401
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000CA*/ public hidebysig static 
          void  Copy(native int source,
                     int64[] destination,
                     int32 startIndex,
                     int32 length) cil managed
  // SIG: 00 04 01 18 1D 0A 08 08
  {
    // Method begins at RVA 0x2403
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000CB*/ public hidebysig static 
          void  Copy(native int source,
                     native int[] destination,
                     int32 startIndex,
                     int32 length) cil managed
  // SIG: 00 04 01 18 1D 18 08 08
  {
    // Method begins at RVA 0x2405
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000CC*/ public hidebysig static 
          void  Copy(native int source,
                     float32[] destination,
                     int32 startIndex,
                     int32 length) cil managed
  // SIG: 00 04 01 18 1D 0C 08 08
  {
    // Method begins at RVA 0x2407
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000CD*/ public hidebysig static 
          void  Copy(native int[] source,
                     int32 startIndex,
                     native int destination,
                     int32 length) cil managed
  // SIG: 00 04 01 1D 18 08 18 08
  {
    // Method begins at RVA 0x2409
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000CE*/ public hidebysig static 
          void  Copy(float32[] source,
                     int32 startIndex,
                     native int destination,
                     int32 length) cil managed
  // SIG: 00 04 01 1D 0C 08 18 08
  {
    // Method begins at RVA 0x240b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Copy

  .method /*060000CF*/ public hidebysig static 
          native int  CreateAggregatedObject(native int pOuter,
                                             object o) cil managed
  // SIG: 00 02 18 18 1C
  {
    .custom /*0C000192:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C000193:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x240d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::CreateAggregatedObject

  .method /*060000D0*/ public hidebysig static 
          native int  CreateAggregatedObject<T>(native int pOuter,
                                                !!T o) cil managed
  // SIG: 10 01 02 18 18 1E 00
  {
    .custom /*0C000194:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2410
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::CreateAggregatedObject

  .method /*060000D1*/ public hidebysig static 
          object  CreateWrapperOfType(object o,
                                      class [System.Runtime/*23000001*/]System.Type/*01000027*/ t) cil managed
  // SIG: 00 02 1C 1C 12 80 9D
  {
    .custom /*0C000195:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000196:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C000197:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*080000ED*/ 
    .custom /*0C0001B1:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000042*/::.ctor(string) /* 0A00001C */ = ( 01 00 01 6F 00 00 )                               // ...o..
    .param [2]/*080000EF*/ 
    .custom /*0C0001B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2413
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::CreateWrapperOfType

  .method /*060000D2*/ public hidebysig static 
          !!TWrapper  CreateWrapperOfType<T,TWrapper>(!!T o) cil managed
  // SIG: 10 02 01 1E 01 1E 00
  {
    .custom /*0C000198:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000199:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .param [0]/*080000F0*/ 
    .custom /*0C0001B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2416
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::CreateWrapperOfType

  .method /*060000D3*/ public hidebysig static 
          void  DestroyStructure(native int ptr,
                                 class [System.Runtime/*23000001*/]System.Type/*01000027*/ structuretype) cil managed
  // SIG: 00 02 01 18 12 80 9D
  {
    .custom /*0C00019A:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2419
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::DestroyStructure

  .method /*060000D4*/ public hidebysig static 
          void  DestroyStructure<T>(native int ptr) cil managed
  // SIG: 10 01 01 01 18
  {
    .custom /*0C00019B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x241b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::DestroyStructure

  .method /*060000D5*/ public hidebysig static 
          int32  FinalReleaseComObject(object o) cil managed
  // SIG: 00 01 08 1C
  {
    .custom /*0C00019C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x241d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::FinalReleaseComObject

  .method /*060000D6*/ public hidebysig static 
          void  FreeBSTR(native int ptr) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x2420
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::FreeBSTR

  .method /*060000D7*/ public hidebysig static 
          void  FreeCoTaskMem(native int ptr) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x2422
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::FreeCoTaskMem

  .method /*060000D8*/ public hidebysig static 
          void  FreeHGlobal(native int hglobal) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x2424
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::FreeHGlobal

  .method /*060000D9*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 
          GenerateGuidForType(class [System.Runtime/*23000001*/]System.Type/*01000027*/ 'type') cil managed
  // SIG: 00 01 11 80 D5 12 80 9D
  {
    // Method begins at RVA 0x2426
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GenerateGuidForType

  .method /*060000DA*/ public hidebysig static 
          string  GenerateProgIdForType(class [System.Runtime/*23000001*/]System.Type/*01000027*/ 'type') cil managed
  // SIG: 00 01 0E 12 80 9D
  {
    .param [0]/*080000FA*/ 
    .custom /*0C0001CB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2429
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GenerateProgIdForType

  .method /*060000DB*/ public hidebysig static 
          native int  GetComInterfaceForObject(object o,
                                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ T) cil managed
  // SIG: 00 02 18 1C 12 80 9D
  {
    .custom /*0C0001A0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C0001A1:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x242c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetComInterfaceForObject

  .method /*060000DC*/ public hidebysig static 
          native int  GetComInterfaceForObject(object o,
                                               class [System.Runtime/*23000001*/]System.Type/*01000027*/ T,
                                               valuetype System.Runtime.InteropServices.CustomQueryInterfaceMode/*02000026*/ mode) cil managed
  // SIG: 00 03 18 1C 12 80 9D 11 80 98
  {
    .custom /*0C0001A2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C0001A3:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x242f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetComInterfaceForObject

  .method /*060000DD*/ public hidebysig static 
          native int  GetComInterfaceForObject<T,TInterface>(!!T o) cil managed
  // SIG: 10 02 01 18 1E 00
  {
    .custom /*0C0001A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001A5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .param [1]/*08000101*/ 
    .custom /*0C0001CE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001CF:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000043*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2432
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetComInterfaceForObject

  .method /*060000DE*/ public hidebysig static 
          object  GetComObjectData(object obj,
                                   object key) cil managed
  // SIG: 00 02 1C 1C 1C
  {
    .custom /*0C0001A6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .param [0]/*08000102*/ 
    .custom /*0C0001D0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2435
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetComObjectData

  .method /*060000DF*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Delegate/*01000031*/ 
          GetDelegateForFunctionPointer(native int ptr,
                                        class [System.Runtime/*23000001*/]System.Type/*01000027*/ t) cil managed
  // SIG: 00 02 12 80 C5 18 12 80 9D
  {
    .custom /*0C0001A7:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2438
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetDelegateForFunctionPointer

  .method /*060000E0*/ public hidebysig static 
          !!TDelegate  GetDelegateForFunctionPointer<TDelegate>(native int ptr) cil managed
  // SIG: 10 01 01 1E 00 18
  {
    .param type TDelegate /*2A00000C*/ 
      .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x243b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetDelegateForFunctionPointer

  .method /*060000E1*/ public hidebysig static 
          int32  GetEndComSlot(class [System.Runtime/*23000001*/]System.Type/*01000027*/ t) cil managed
  // SIG: 00 01 08 12 80 9D
  {
    .custom /*0C0001A8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x243e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetEndComSlot

  .method /*060000E2*/ public hidebysig static 
          int32  GetExceptionCode() cil managed
  // SIG: 00 00 08
  {
    .custom /*0C0001A9:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001AA:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 39 47 65 74 45 78 63 65 70 74 69 6F 6E 43   // ..9GetExceptionC
                                                                                                                                                   6F 64 65 28 29 20 6D 61 79 20 62 65 20 75 6E 61   // ode() may be una
                                                                                                                                                   76 61 69 6C 61 62 6C 65 20 69 6E 20 66 75 74 75   // vailable in futu
                                                                                                                                                   72 65 20 72 65 6C 65 61 73 65 73 2E 00 00 )       // re releases...
    // Method begins at RVA 0x2441
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetExceptionCode

  .method /*060000E3*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Exception/*01000019*/ 
          GetExceptionForHR(int32 errorCode) cil managed
  // SIG: 00 01 12 65 08
  {
    .custom /*0C0001AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2444
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetExceptionForHR

  .method /*060000E4*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Exception/*01000019*/ 
          GetExceptionForHR(int32 errorCode,
                            native int errorInfo) cil managed
  // SIG: 00 02 12 65 08 18
  {
    .custom /*0C0001AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2447
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetExceptionForHR

  .method /*060000E5*/ public hidebysig static 
          native int  GetExceptionPointers() cil managed
  // SIG: 00 00 18
  {
    // Method begins at RVA 0x244a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetExceptionPointers

  .method /*060000E6*/ public hidebysig static 
          native int  GetFunctionPointerForDelegate(class [System.Runtime/*23000001*/]System.Delegate/*01000031*/ d) cil managed
  // SIG: 00 01 18 12 80 C5
  {
    .custom /*0C0001AD:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x244d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetFunctionPointerForDelegate

  .method /*060000E7*/ public hidebysig static 
          native int  GetFunctionPointerForDelegate<TDelegate>(!!TDelegate d) cil managed
  // SIG: 10 01 01 18 1E 00
  {
    // Method begins at RVA 0x2450
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetFunctionPointerForDelegate

  .method /*060000E8*/ public hidebysig static 
          native int  GetHINSTANCE(class [System.Runtime/*23000001*/]System.Reflection.Module/*0100002F*/ m) cil managed
  // SIG: 00 01 18 12 80 BD
  {
    // Method begins at RVA 0x2453
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetHINSTANCE

  .method /*060000E9*/ public hidebysig static 
          int32  GetHRForException(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ e) cil managed
  // SIG: 00 01 08 12 65
  {
    .custom /*0C0001AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2456
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetHRForException

  .method /*060000EA*/ public hidebysig static 
          int32  GetHRForLastWin32Error() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2459
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetHRForLastWin32Error

  .method /*060000EB*/ public hidebysig static 
          native int  GetIDispatchForObject(object o) cil managed
  // SIG: 00 01 18 1C
  {
    .custom /*0C0001AF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x245c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetIDispatchForObject

  .method /*060000EC*/ public hidebysig static 
          native int  GetIUnknownForObject(object o) cil managed
  // SIG: 00 01 18 1C
  {
    .custom /*0C0001B0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x245f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetIUnknownForObject

  .method /*060000ED*/ public hidebysig static 
          int32  GetLastPInvokeError() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2462
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetLastPInvokeError

  .method /*060000EE*/ public hidebysig static 
          int32  GetLastSystemError() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2465
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetLastSystemError

  .method /*060000EF*/ public hidebysig static 
          int32  GetLastWin32Error() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2468
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetLastWin32Error

  .method /*060000F0*/ public hidebysig static 
          void  GetNativeVariantForObject(object obj,
                                          native int pDstNativeVariant) cil managed
  // SIG: 00 02 01 1C 18
  {
    .custom /*0C0001B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001B4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C0001B5:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x246b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::GetNativeVariantForObject

  .method /*060000F1*/ public hidebysig static 
          void  GetNativeVariantForObject<T>(!!T obj,
                                             native int pDstNativeVariant) cil managed
  // SIG: 10 01 02 01 1E 00 18
  {
    .custom /*0C0001B7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001B8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C0001B9:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x246d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::GetNativeVariantForObject

  .method /*060000F2*/ public hidebysig static 
          object  GetObjectForIUnknown(native int pUnk) cil managed
  // SIG: 00 01 1C 18
  {
    .custom /*0C0001BA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x246f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetObjectForIUnknown

  .method /*060000F3*/ public hidebysig static 
          object  GetObjectForNativeVariant(native int pSrcNativeVariant) cil managed
  // SIG: 00 01 1C 18
  {
    .custom /*0C0001BB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001BC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C0001BD:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetObjectForNativeVariant

  .method /*060000F4*/ public hidebysig static 
          !!T  GetObjectForNativeVariant<T>(native int pSrcNativeVariant) cil managed
  // SIG: 10 01 01 1E 00 18
  {
    .custom /*0C0001BE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001BF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C0001C0:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2475
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetObjectForNativeVariant

  .method /*060000F5*/ public hidebysig static 
          object[]  GetObjectsForNativeVariants(native int aSrcNativeVariant,
                                                int32 cVars) cil managed
  // SIG: 00 02 1D 1C 18 08
  {
    .custom /*0C0001C1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C0001C2:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000119*/ 
    .custom /*0C0001DF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2478
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetObjectsForNativeVariants

  .method /*060000F6*/ public hidebysig static 
          !!T[]  GetObjectsForNativeVariants<T>(native int aSrcNativeVariant,
                                                int32 cVars) cil managed
  // SIG: 10 01 02 1D 1E 00 18 08
  {
    .custom /*0C0001C4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .custom /*0C0001C5:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param type T /*2A000010*/ 
      .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x247b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetObjectsForNativeVariants

  .method /*060000F7*/ public hidebysig static 
          int32  GetStartComSlot(class [System.Runtime/*23000001*/]System.Type/*01000027*/ t) cil managed
  // SIG: 00 01 08 12 80 9D
  {
    .custom /*0C0001C6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x247e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetStartComSlot

  .method /*060000F8*/ public hidebysig static 
          object  GetTypedObjectForIUnknown(native int pUnk,
                                            class [System.Runtime/*23000001*/]System.Type/*01000027*/ t) cil managed
  // SIG: 00 02 1C 18 12 80 9D
  {
    .custom /*0C0001C7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2481
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetTypedObjectForIUnknown

  .method /*060000F9*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*01000027*/ 
          GetTypeFromCLSID(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 'clsid') cil managed
  // SIG: 00 01 12 80 9D 11 80 D5
  {
    .custom /*0C0001C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001C9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2484
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetTypeFromCLSID

  .method /*060000FA*/ public hidebysig static 
          string  GetTypeInfoName(class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/ typeInfo) cil managed
  // SIG: 00 01 0E 12 82 30
  {
    .custom /*0C0001CA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2487
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetTypeInfoName

  .method /*060000FB*/ public hidebysig static 
          object  GetUniqueObjectForIUnknown(native int unknown) cil managed
  // SIG: 00 01 1C 18
  {
    .custom /*0C0001CC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x248a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::GetUniqueObjectForIUnknown

  .method /*060000FC*/ public hidebysig static 
          void  InitHandle(class [System.Runtime/*23000001*/]System.Runtime.InteropServices.SafeHandle/*01000044*/ safeHandle,
                           native int handle) cil managed
  // SIG: 00 02 01 12 81 11 18
  {
    // Method begins at RVA 0x248d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::InitHandle

  .method /*060000FD*/ public hidebysig static 
          bool  IsComObject(object o) cil managed
  // SIG: 00 01 02 1C
  {
    // Method begins at RVA 0x248f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::IsComObject

  .method /*060000FE*/ public hidebysig static 
          bool  IsTypeVisibleFromCom(class [System.Runtime/*23000001*/]System.Type/*01000027*/ t) cil managed
  // SIG: 00 01 02 12 80 9D
  {
    // Method begins at RVA 0x2492
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::IsTypeVisibleFromCom

  .method /*060000FF*/ public hidebysig static 
          native int  OffsetOf(class [System.Runtime/*23000001*/]System.Type/*01000027*/ t,
                               string fieldName) cil managed
  // SIG: 00 02 18 12 80 9D 0E
  {
    .custom /*0C0001CD:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2495
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::OffsetOf

  .method /*06000100*/ public hidebysig static 
          native int  OffsetOf<T>(string fieldName) cil managed
  // SIG: 10 01 01 18 0E
  {
    .param type T /*2A000011*/ 
      .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2498
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::OffsetOf

  .method /*06000101*/ public hidebysig static 
          void  Prelink(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000032*/ m) cil managed
  // SIG: 00 01 01 12 80 C9
  {
    // Method begins at RVA 0x249b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::Prelink

  .method /*06000102*/ public hidebysig static 
          void  PrelinkAll(class [System.Runtime/*23000001*/]System.Type/*01000027*/ c) cil managed
  // SIG: 00 01 01 12 80 9D
  {
    // Method begins at RVA 0x249d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::PrelinkAll

  .method /*06000103*/ public hidebysig static 
          string  PtrToStringAnsi(native int ptr) cil managed
  // SIG: 00 01 0E 18
  {
    .custom /*0C0001D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x249f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringAnsi

  .method /*06000104*/ public hidebysig static 
          string  PtrToStringAnsi(native int ptr,
                                  int32 len) cil managed
  // SIG: 00 02 0E 18 08
  {
    // Method begins at RVA 0x24a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringAnsi

  .method /*06000105*/ public hidebysig static 
          string  PtrToStringAuto(native int ptr) cil managed
  // SIG: 00 01 0E 18
  {
    .custom /*0C0001D2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringAuto

  .method /*06000106*/ public hidebysig static 
          string  PtrToStringAuto(native int ptr,
                                  int32 len) cil managed
  // SIG: 00 02 0E 18 08
  {
    .custom /*0C0001D3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringAuto

  .method /*06000107*/ public hidebysig static 
          string  PtrToStringBSTR(native int ptr) cil managed
  // SIG: 00 01 0E 18
  {
    // Method begins at RVA 0x24ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringBSTR

  .method /*06000108*/ public hidebysig static 
          string  PtrToStringUni(native int ptr) cil managed
  // SIG: 00 01 0E 18
  {
    .custom /*0C0001D4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringUni

  .method /*06000109*/ public hidebysig static 
          string  PtrToStringUni(native int ptr,
                                 int32 len) cil managed
  // SIG: 00 02 0E 18 08
  {
    // Method begins at RVA 0x24b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringUni

  .method /*0600010A*/ public hidebysig static 
          string  PtrToStringUTF8(native int ptr) cil managed
  // SIG: 00 01 0E 18
  {
    .custom /*0C0001D5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringUTF8

  .method /*0600010B*/ public hidebysig static 
          string  PtrToStringUTF8(native int ptr,
                                  int32 byteLen) cil managed
  // SIG: 00 02 0E 18 08
  {
    // Method begins at RVA 0x24b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStringUTF8

  .method /*0600010C*/ public hidebysig static 
          void  PtrToStructure(native int ptr,
                               object structure) cil managed
  // SIG: 00 02 01 18 1C
  {
    .custom /*0C0001D6:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x24ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::PtrToStructure

  .method /*0600010D*/ public hidebysig static 
          object  PtrToStructure(native int ptr,
                                 class [System.Runtime/*23000001*/]System.Type/*01000027*/ structureType) cil managed
  // SIG: 00 02 1C 18 12 80 9D
  {
    .custom /*0C0001D7:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800013C*/ 
    .custom /*0C0001F3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800013E*/ 
    .custom /*0C0001F4:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A000018 */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x24bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStructure

  .method /*0600010E*/ public hidebysig static 
          !!T  PtrToStructure<T>(native int ptr) cil managed
  // SIG: 10 01 01 1E 00 18
  {
    .custom /*0C0001D8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param type T /*2A000012*/ 
      .custom /*0C000045:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A000018 */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x24bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::PtrToStructure

  .method /*0600010F*/ public hidebysig static 
          void  PtrToStructure<T>(native int ptr,
                                  !!T structure) cil managed
  // SIG: 10 01 02 01 18 1E 00
  {
    .param type T /*2A000013*/ 
      .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000141*/ 
    .custom /*0C0001F7:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000043*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24c2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::PtrToStructure

  .method /*06000110*/ public hidebysig static 
          int32  QueryInterface(native int pUnk,
                                valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& iid,
                                [out] native int& ppv) cil managed
  // SIG: 00 03 08 18 10 11 80 D5 10 18
  {
    // Method begins at RVA 0x24c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::QueryInterface

  .method /*06000111*/ public hidebysig static 
          uint8  ReadByte(native int ptr) cil managed
  // SIG: 00 01 05 18
  {
    // Method begins at RVA 0x24c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadByte

  .method /*06000112*/ public hidebysig static 
          uint8  ReadByte(native int ptr,
                          int32 ofs) cil managed
  // SIG: 00 02 05 18 08
  {
    // Method begins at RVA 0x24ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadByte

  .method /*06000113*/ public hidebysig static 
          uint8  ReadByte(object ptr,
                          int32 ofs) cil managed
  // SIG: 00 02 05 1C 08
  {
    .custom /*0C0001D9:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001DA:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3E 52 65 61 64 42 79 74 65 28 4F 62 6A 65   // ..>ReadByte(Obje
                                                                                                                                                   63 74 2C 20 49 6E 74 33 32 29 20 6D 61 79 20 62   // ct, Int32) may b
                                                                                                                                                   65 20 75 6E 61 76 61 69 6C 61 62 6C 65 20 69 6E   // e unavailable in
                                                                                                                                                   20 66 75 74 75 72 65 20 72 65 6C 65 61 73 65 73   //  future releases
                                                                                                                                                   2E 00 00 )                                        // ...
    // Method begins at RVA 0x24cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadByte

  .method /*06000114*/ public hidebysig static 
          int16  ReadInt16(native int ptr) cil managed
  // SIG: 00 01 06 18
  {
    // Method begins at RVA 0x24d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt16

  .method /*06000115*/ public hidebysig static 
          int16  ReadInt16(native int ptr,
                           int32 ofs) cil managed
  // SIG: 00 02 06 18 08
  {
    // Method begins at RVA 0x24d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt16

  .method /*06000116*/ public hidebysig static 
          int16  ReadInt16(object ptr,
                           int32 ofs) cil managed
  // SIG: 00 02 06 1C 08
  {
    .custom /*0C0001DB:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001DC:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3F 52 65 61 64 49 6E 74 31 36 28 4F 62 6A   // ..?ReadInt16(Obj
                                                                                                                                                   65 63 74 2C 20 49 6E 74 33 32 29 20 6D 61 79 20   // ect, Int32) may 
                                                                                                                                                   62 65 20 75 6E 61 76 61 69 6C 61 62 6C 65 20 69   // be unavailable i
                                                                                                                                                   6E 20 66 75 74 75 72 65 20 72 65 6C 65 61 73 65   // n future release
                                                                                                                                                   73 2E 00 00 )                                     // s...
    // Method begins at RVA 0x24d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt16

  .method /*06000117*/ public hidebysig static 
          int32  ReadInt32(native int ptr) cil managed
  // SIG: 00 01 08 18
  {
    // Method begins at RVA 0x24d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt32

  .method /*06000118*/ public hidebysig static 
          int32  ReadInt32(native int ptr,
                           int32 ofs) cil managed
  // SIG: 00 02 08 18 08
  {
    // Method begins at RVA 0x24dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt32

  .method /*06000119*/ public hidebysig static 
          int32  ReadInt32(object ptr,
                           int32 ofs) cil managed
  // SIG: 00 02 08 1C 08
  {
    .custom /*0C0001DD:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001DE:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3F 52 65 61 64 49 6E 74 33 32 28 4F 62 6A   // ..?ReadInt32(Obj
                                                                                                                                                   65 63 74 2C 20 49 6E 74 33 32 29 20 6D 61 79 20   // ect, Int32) may 
                                                                                                                                                   62 65 20 75 6E 61 76 61 69 6C 61 62 6C 65 20 69   // be unavailable i
                                                                                                                                                   6E 20 66 75 74 75 72 65 20 72 65 6C 65 61 73 65   // n future release
                                                                                                                                                   73 2E 00 00 )                                     // s...
    // Method begins at RVA 0x24df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt32

  .method /*0600011A*/ public hidebysig static 
          int64  ReadInt64(native int ptr) cil managed
  // SIG: 00 01 0A 18
  {
    // Method begins at RVA 0x24e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt64

  .method /*0600011B*/ public hidebysig static 
          int64  ReadInt64(native int ptr,
                           int32 ofs) cil managed
  // SIG: 00 02 0A 18 08
  {
    // Method begins at RVA 0x24e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt64

  .method /*0600011C*/ public hidebysig static 
          int64  ReadInt64(object ptr,
                           int32 ofs) cil managed
  // SIG: 00 02 0A 1C 08
  {
    .custom /*0C0001E0:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001E1:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3F 52 65 61 64 49 6E 74 36 34 28 4F 62 6A   // ..?ReadInt64(Obj
                                                                                                                                                   65 63 74 2C 20 49 6E 74 33 32 29 20 6D 61 79 20   // ect, Int32) may 
                                                                                                                                                   62 65 20 75 6E 61 76 61 69 6C 61 62 6C 65 20 69   // be unavailable i
                                                                                                                                                   6E 20 66 75 74 75 72 65 20 72 65 6C 65 61 73 65   // n future release
                                                                                                                                                   73 2E 00 00 )                                     // s...
    // Method begins at RVA 0x24e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadInt64

  .method /*0600011D*/ public hidebysig static 
          native int  ReadIntPtr(native int ptr) cil managed
  // SIG: 00 01 18 18
  {
    // Method begins at RVA 0x24eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadIntPtr

  .method /*0600011E*/ public hidebysig static 
          native int  ReadIntPtr(native int ptr,
                                 int32 ofs) cil managed
  // SIG: 00 02 18 18 08
  {
    // Method begins at RVA 0x24ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadIntPtr

  .method /*0600011F*/ public hidebysig static 
          native int  ReadIntPtr(object ptr,
                                 int32 ofs) cil managed
  // SIG: 00 02 18 1C 08
  {
    .custom /*0C0001E2:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001E3:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 40 52 65 61 64 49 6E 74 50 74 72 28 4F 62   // ..@ReadIntPtr(Ob
                                                                                                                                                   6A 65 63 74 2C 20 49 6E 74 33 32 29 20 6D 61 79   // ject, Int32) may
                                                                                                                                                   20 62 65 20 75 6E 61 76 61 69 6C 61 62 6C 65 20   //  be unavailable 
                                                                                                                                                   69 6E 20 66 75 74 75 72 65 20 72 65 6C 65 61 73   // in future releas
                                                                                                                                                   65 73 2E 00 00 )                                  // es...
    // Method begins at RVA 0x24f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReadIntPtr

  .method /*06000120*/ public hidebysig static 
          native int  ReAllocCoTaskMem(native int pv,
                                       int32 cb) cil managed
  // SIG: 00 02 18 18 08
  {
    // Method begins at RVA 0x24f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReAllocCoTaskMem

  .method /*06000121*/ public hidebysig static 
          native int  ReAllocHGlobal(native int pv,
                                     native int cb) cil managed
  // SIG: 00 02 18 18 18
  {
    // Method begins at RVA 0x24f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReAllocHGlobal

  .method /*06000122*/ public hidebysig static 
          int32  Release(native int pUnk) cil managed
  // SIG: 00 01 08 18
  {
    // Method begins at RVA 0x24fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::Release

  .method /*06000123*/ public hidebysig static 
          int32  ReleaseComObject(object o) cil managed
  // SIG: 00 01 08 1C
  {
    .custom /*0C0001E4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x24fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::ReleaseComObject

  .method /*06000124*/ public hidebysig static 
          native int  SecureStringToBSTR(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x2500
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SecureStringToBSTR

  .method /*06000125*/ public hidebysig static 
          native int  SecureStringToCoTaskMemAnsi(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x2503
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SecureStringToCoTaskMemAnsi

  .method /*06000126*/ public hidebysig static 
          native int  SecureStringToCoTaskMemUnicode(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x2506
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SecureStringToCoTaskMemUnicode

  .method /*06000127*/ public hidebysig static 
          native int  SecureStringToGlobalAllocAnsi(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x2509
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SecureStringToGlobalAllocAnsi

  .method /*06000128*/ public hidebysig static 
          native int  SecureStringToGlobalAllocUnicode(class System.Security.SecureString/*02000009*/ s) cil managed
  // SIG: 00 01 18 12 24
  {
    // Method begins at RVA 0x250c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SecureStringToGlobalAllocUnicode

  .method /*06000129*/ public hidebysig static 
          bool  SetComObjectData(object obj,
                                 object key,
                                 object data) cil managed
  // SIG: 00 03 02 1C 1C 1C
  {
    .custom /*0C0001E5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .param [3]/*0800016B*/ 
    .custom /*0C00020C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x250f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SetComObjectData

  .method /*0600012A*/ public hidebysig static 
          void  SetLastPInvokeError(int32 'error') cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2512
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SetLastPInvokeError

  .method /*0600012B*/ public hidebysig static 
          void  SetLastSystemError(int32 'error') cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2515
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SetLastSystemError

  .method /*0600012C*/ public hidebysig static 
          int32  SizeOf(object structure) cil managed
  // SIG: 00 01 08 1C
  {
    .custom /*0C0001E6:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2518
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SizeOf

  .method /*0600012D*/ public hidebysig static 
          int32  SizeOf(class [System.Runtime/*23000001*/]System.Type/*01000027*/ t) cil managed
  // SIG: 00 01 08 12 80 9D
  {
    .custom /*0C0001E7:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x251b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SizeOf

  .method /*0600012E*/ public hidebysig static 
          int32  SizeOf<T>() cil managed
  // SIG: 10 01 00 08
  {
    .custom /*0C0001E8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x251e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SizeOf

  .method /*0600012F*/ public hidebysig static 
          int32  SizeOf<T>(!!T structure) cil managed
  // SIG: 10 01 01 08 1E 00
  {
    .param type T /*2A000015*/ 
      .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2521
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::SizeOf

  .method /*06000130*/ public hidebysig static 
          native int  StringToBSTR(string s) cil managed
  // SIG: 00 01 18 0E
  {
    .custom /*0C0001E9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2524
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::StringToBSTR

  .method /*06000131*/ public hidebysig static 
          native int  StringToCoTaskMemAnsi(string s) cil managed
  // SIG: 00 01 18 0E
  {
    .custom /*0C0001EA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2527
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::StringToCoTaskMemAnsi

  .method /*06000132*/ public hidebysig static 
          native int  StringToCoTaskMemAuto(string s) cil managed
  // SIG: 00 01 18 0E
  {
    .custom /*0C0001EB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x252a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::StringToCoTaskMemAuto

  .method /*06000133*/ public hidebysig static 
          native int  StringToCoTaskMemUni(string s) cil managed
  // SIG: 00 01 18 0E
  {
    .custom /*0C0001EC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x252d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::StringToCoTaskMemUni

  .method /*06000134*/ public hidebysig static 
          native int  StringToCoTaskMemUTF8(string s) cil managed
  // SIG: 00 01 18 0E
  {
    .custom /*0C0001ED:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2530
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::StringToCoTaskMemUTF8

  .method /*06000135*/ public hidebysig static 
          native int  StringToHGlobalAnsi(string s) cil managed
  // SIG: 00 01 18 0E
  {
    .custom /*0C0001EE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2533
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::StringToHGlobalAnsi

  .method /*06000136*/ public hidebysig static 
          native int  StringToHGlobalAuto(string s) cil managed
  // SIG: 00 01 18 0E
  {
    .custom /*0C0001EF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2536
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::StringToHGlobalAuto

  .method /*06000137*/ public hidebysig static 
          native int  StringToHGlobalUni(string s) cil managed
  // SIG: 00 01 18 0E
  {
    .custom /*0C0001F0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2539
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::StringToHGlobalUni

  .method /*06000138*/ public hidebysig static 
          void  StructureToPtr(object structure,
                               native int ptr,
                               bool fDeleteOld) cil managed
  // SIG: 00 03 01 1C 18 02
  {
    .custom /*0C0001F1:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x253c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::StructureToPtr

  .method /*06000139*/ public hidebysig static 
          void  StructureToPtr<T>(!!T structure,
                                  native int ptr,
                                  bool fDeleteOld) cil managed
  // SIG: 10 01 03 01 1E 00 18 02
  {
    .param type T /*2A000016*/ 
      .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800017C*/ 
    .custom /*0C000217:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000043*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x253e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::StructureToPtr

  .method /*0600013A*/ public hidebysig static 
          void  ThrowExceptionForHR(int32 errorCode) cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2540
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ThrowExceptionForHR

  .method /*0600013B*/ public hidebysig static 
          void  ThrowExceptionForHR(int32 errorCode,
                                    native int errorInfo) cil managed
  // SIG: 00 02 01 08 18
  {
    // Method begins at RVA 0x2542
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ThrowExceptionForHR

  .method /*0600013C*/ public hidebysig static 
          native int  UnsafeAddrOfPinnedArrayElement(class [System.Runtime/*23000001*/]System.Array/*01000045*/ arr,
                                                     int32 index) cil managed
  // SIG: 00 02 18 12 81 15 08
  {
    .custom /*0C0001F2:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2544
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::UnsafeAddrOfPinnedArrayElement

  .method /*0600013D*/ public hidebysig static 
          native int  UnsafeAddrOfPinnedArrayElement<T>(!!T[] arr,
                                                        int32 index) cil managed
  // SIG: 10 01 02 18 1D 1E 00 08
  {
    .param type T /*2A000017*/ 
      .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2547
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Marshal::UnsafeAddrOfPinnedArrayElement

  .method /*0600013E*/ public hidebysig static 
          void  WriteByte(native int ptr,
                          uint8 val) cil managed
  // SIG: 00 02 01 18 05
  {
    // Method begins at RVA 0x254a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteByte

  .method /*0600013F*/ public hidebysig static 
          void  WriteByte(native int ptr,
                          int32 ofs,
                          uint8 val) cil managed
  // SIG: 00 03 01 18 08 05
  {
    // Method begins at RVA 0x254c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteByte

  .method /*06000140*/ public hidebysig static 
          void  WriteByte(object ptr,
                          int32 ofs,
                          uint8 val) cil managed
  // SIG: 00 03 01 1C 08 05
  {
    .custom /*0C0001F5:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001F6:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 45 57 72 69 74 65 42 79 74 65 28 4F 62 6A   // ..EWriteByte(Obj
                                                                                                                                                   65 63 74 2C 20 49 6E 74 33 32 2C 20 42 79 74 65   // ect, Int32, Byte
                                                                                                                                                   29 20 6D 61 79 20 62 65 20 75 6E 61 76 61 69 6C   // ) may be unavail
                                                                                                                                                   61 62 6C 65 20 69 6E 20 66 75 74 75 72 65 20 72   // able in future r
                                                                                                                                                   65 6C 65 61 73 65 73 2E 00 00 )                   // eleases...
    // Method begins at RVA 0x254e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteByte

  .method /*06000141*/ public hidebysig static 
          void  WriteInt16(native int ptr,
                           char val) cil managed
  // SIG: 00 02 01 18 03
  {
    // Method begins at RVA 0x2550
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt16

  .method /*06000142*/ public hidebysig static 
          void  WriteInt16(native int ptr,
                           int16 val) cil managed
  // SIG: 00 02 01 18 06
  {
    // Method begins at RVA 0x2552
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt16

  .method /*06000143*/ public hidebysig static 
          void  WriteInt16(native int ptr,
                           int32 ofs,
                           char val) cil managed
  // SIG: 00 03 01 18 08 03
  {
    // Method begins at RVA 0x2554
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt16

  .method /*06000144*/ public hidebysig static 
          void  WriteInt16(native int ptr,
                           int32 ofs,
                           int16 val) cil managed
  // SIG: 00 03 01 18 08 06
  {
    // Method begins at RVA 0x2556
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt16

  .method /*06000145*/ public hidebysig static 
          void  WriteInt16(object ptr,
                           int32 ofs,
                           char val) cil managed
  // SIG: 00 03 01 1C 08 03
  {
    .custom /*0C0001F8:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001F9:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 46 57 72 69 74 65 49 6E 74 31 36 28 4F 62   // ..FWriteInt16(Ob
                                                                                                                                                   6A 65 63 74 2C 20 49 6E 74 33 32 2C 20 43 68 61   // ject, Int32, Cha
                                                                                                                                                   72 29 20 6D 61 79 20 62 65 20 75 6E 61 76 61 69   // r) may be unavai
                                                                                                                                                   6C 61 62 6C 65 20 69 6E 20 66 75 74 75 72 65 20   // lable in future 
                                                                                                                                                   72 65 6C 65 61 73 65 73 2E 00 00 )                // releases...
    // Method begins at RVA 0x2558
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt16

  .method /*06000146*/ public hidebysig static 
          void  WriteInt16(object ptr,
                           int32 ofs,
                           int16 val) cil managed
  // SIG: 00 03 01 1C 08 06
  {
    .custom /*0C0001FA:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001FB:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 47 57 72 69 74 65 49 6E 74 31 36 28 4F 62   // ..GWriteInt16(Ob
                                                                                                                                                   6A 65 63 74 2C 20 49 6E 74 33 32 2C 20 49 6E 74   // ject, Int32, Int
                                                                                                                                                   31 36 29 20 6D 61 79 20 62 65 20 75 6E 61 76 61   // 16) may be unava
                                                                                                                                                   69 6C 61 62 6C 65 20 69 6E 20 66 75 74 75 72 65   // ilable in future
                                                                                                                                                   20 72 65 6C 65 61 73 65 73 2E 00 00 )             //  releases...
    // Method begins at RVA 0x255a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt16

  .method /*06000147*/ public hidebysig static 
          void  WriteInt32(native int ptr,
                           int32 val) cil managed
  // SIG: 00 02 01 18 08
  {
    // Method begins at RVA 0x255c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt32

  .method /*06000148*/ public hidebysig static 
          void  WriteInt32(native int ptr,
                           int32 ofs,
                           int32 val) cil managed
  // SIG: 00 03 01 18 08 08
  {
    // Method begins at RVA 0x255e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt32

  .method /*06000149*/ public hidebysig static 
          void  WriteInt32(object ptr,
                           int32 ofs,
                           int32 val) cil managed
  // SIG: 00 03 01 1C 08 08
  {
    .custom /*0C0001FC:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001FD:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 47 57 72 69 74 65 49 6E 74 33 32 28 4F 62   // ..GWriteInt32(Ob
                                                                                                                                                   6A 65 63 74 2C 20 49 6E 74 33 32 2C 20 49 6E 74   // ject, Int32, Int
                                                                                                                                                   33 32 29 20 6D 61 79 20 62 65 20 75 6E 61 76 61   // 32) may be unava
                                                                                                                                                   69 6C 61 62 6C 65 20 69 6E 20 66 75 74 75 72 65   // ilable in future
                                                                                                                                                   20 72 65 6C 65 61 73 65 73 2E 00 00 )             //  releases...
    // Method begins at RVA 0x2560
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt32

  .method /*0600014A*/ public hidebysig static 
          void  WriteInt64(native int ptr,
                           int32 ofs,
                           int64 val) cil managed
  // SIG: 00 03 01 18 08 0A
  {
    // Method begins at RVA 0x2562
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt64

  .method /*0600014B*/ public hidebysig static 
          void  WriteInt64(native int ptr,
                           int64 val) cil managed
  // SIG: 00 02 01 18 0A
  {
    // Method begins at RVA 0x2564
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt64

  .method /*0600014C*/ public hidebysig static 
          void  WriteInt64(object ptr,
                           int32 ofs,
                           int64 val) cil managed
  // SIG: 00 03 01 1C 08 0A
  {
    .custom /*0C0001FE:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C0001FF:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 47 57 72 69 74 65 49 6E 74 36 34 28 4F 62   // ..GWriteInt64(Ob
                                                                                                                                                   6A 65 63 74 2C 20 49 6E 74 33 32 2C 20 49 6E 74   // ject, Int32, Int
                                                                                                                                                   36 34 29 20 6D 61 79 20 62 65 20 75 6E 61 76 61   // 64) may be unava
                                                                                                                                                   69 6C 61 62 6C 65 20 69 6E 20 66 75 74 75 72 65   // ilable in future
                                                                                                                                                   20 72 65 6C 65 61 73 65 73 2E 00 00 )             //  releases...
    // Method begins at RVA 0x2566
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteInt64

  .method /*0600014D*/ public hidebysig static 
          void  WriteIntPtr(native int ptr,
                            int32 ofs,
                            native int val) cil managed
  // SIG: 00 03 01 18 08 18
  {
    // Method begins at RVA 0x2568
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteIntPtr

  .method /*0600014E*/ public hidebysig static 
          void  WriteIntPtr(native int ptr,
                            native int val) cil managed
  // SIG: 00 02 01 18 18
  {
    // Method begins at RVA 0x256a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteIntPtr

  .method /*0600014F*/ public hidebysig static 
          void  WriteIntPtr(object ptr,
                            int32 ofs,
                            native int val) cil managed
  // SIG: 00 03 01 1C 08 18
  {
    .custom /*0C000200:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000201:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 49 57 72 69 74 65 49 6E 74 50 74 72 28 4F   // ..IWriteIntPtr(O
                                                                                                                                                   62 6A 65 63 74 2C 20 49 6E 74 33 32 2C 20 49 6E   // bject, Int32, In
                                                                                                                                                   74 50 74 72 29 20 6D 61 79 20 62 65 20 75 6E 61   // tPtr) may be una
                                                                                                                                                   76 61 69 6C 61 62 6C 65 20 69 6E 20 66 75 74 75   // vailable in futu
                                                                                                                                                   72 65 20 72 65 6C 65 61 73 65 73 2E 00 00 )       // re releases...
    // Method begins at RVA 0x256c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::WriteIntPtr

  .method /*06000150*/ public hidebysig static 
          void  ZeroFreeBSTR(native int s) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x256e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ZeroFreeBSTR

  .method /*06000151*/ public hidebysig static 
          void  ZeroFreeCoTaskMemAnsi(native int s) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x2570
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ZeroFreeCoTaskMemAnsi

  .method /*06000152*/ public hidebysig static 
          void  ZeroFreeCoTaskMemUnicode(native int s) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x2572
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ZeroFreeCoTaskMemUnicode

  .method /*06000153*/ public hidebysig static 
          void  ZeroFreeCoTaskMemUTF8(native int s) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x2574
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ZeroFreeCoTaskMemUTF8

  .method /*06000154*/ public hidebysig static 
          void  ZeroFreeGlobalAllocAnsi(native int s) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x2576
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ZeroFreeGlobalAllocAnsi

  .method /*06000155*/ public hidebysig static 
          void  ZeroFreeGlobalAllocUnicode(native int s) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x2578
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Marshal::ZeroFreeGlobalAllocUnicode

} // end of class System.Runtime.InteropServices.Marshal

.class /*02000041*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.MarshalAsAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B5:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 29 00 00 01 00 54 02 09 49 6E 68 65 72   // ...)....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .field /*04000036*/ public valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ ArraySubType
  .field /*04000037*/ public int32 IidParameterIndex
  .field /*04000038*/ public string MarshalCookie
  .field /*04000039*/ public string MarshalType
  .field /*0400003A*/ public class [System.Runtime/*23000001*/]System.Type/*01000027*/ MarshalTypeRef
  .field /*0400003B*/ public valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ SafeArraySubType
  .field /*0400003C*/ public class [System.Runtime/*23000001*/]System.Type/*01000027*/ SafeArrayUserDefinedSubType
  .field /*0400003D*/ public int32 SizeConst
  .field /*0400003E*/ public int16 SizeParamIndex
  .method /*06000156*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int16 unmanagedType) cil managed
  // SIG: 20 01 01 06
  {
    // Method begins at RVA 0x257a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MarshalAsAttribute::.ctor

  .method /*06000157*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ unmanagedType) cil managed
  // SIG: 20 01 01 11 81 74
  {
    // Method begins at RVA 0x2582
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MarshalAsAttribute::.ctor

  .method /*06000158*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ 
          get_Value() cil managed
  // SIG: 20 00 11 81 74
  {
    // Method begins at RVA 0x258a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MarshalAsAttribute::get_Value

  .property /*1700002C*/ instance callconv(8) valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/
          Value()
  {
    .get instance valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ System.Runtime.InteropServices.MarshalAsAttribute/*02000041*/::get_Value() /* 06000158 */
  } // end of property MarshalAsAttribute::Value
} // end of class System.Runtime.InteropServices.MarshalAsAttribute

.class /*02000042*/ public auto ansi beforefieldinit System.Runtime.InteropServices.MarshalDirectiveException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000018*/
{
  .custom /*0C0000B8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000159*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x258d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MarshalDirectiveException::.ctor

  .method /*0600015A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C000202:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2595
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MarshalDirectiveException::.ctor

  .method /*0600015B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x259d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MarshalDirectiveException::.ctor

  .method /*0600015C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ inner) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x25a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MarshalDirectiveException::.ctor

} // end of class System.Runtime.InteropServices.MarshalDirectiveException

.class /*02000043*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.NativeLibrary
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C0000BB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600015D*/ public hidebysig static 
          void  Free(native int handle) cil managed
  // SIG: 00 01 01 18
  {
    // Method begins at RVA 0x25ad
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NativeLibrary::Free

  .method /*0600015E*/ public hidebysig static 
          native int  GetExport(native int handle,
                                string name) cil managed
  // SIG: 00 02 18 18 0E
  {
    // Method begins at RVA 0x25af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeLibrary::GetExport

  .method /*0600015F*/ public hidebysig static 
          native int  Load(string libraryPath) cil managed
  // SIG: 00 01 18 0E
  {
    // Method begins at RVA 0x25b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeLibrary::Load

  .method /*06000160*/ public hidebysig static 
          native int  Load(string libraryName,
                           class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003A*/ 'assembly',
                           valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003B*/<valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/> searchPath) cil managed
  // SIG: 00 03 18 0E 12 80 E9 15 11 80 ED 01 11 80 BC
  {
    // Method begins at RVA 0x25b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeLibrary::Load

  .method /*06000161*/ public hidebysig static 
          void  SetDllImportResolver(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003A*/ 'assembly',
                                     class System.Runtime.InteropServices.DllImportResolver/*0200002E*/ resolver) cil managed
  // SIG: 00 02 01 12 80 E9 12 80 B8
  {
    // Method begins at RVA 0x25b8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NativeLibrary::SetDllImportResolver

  .method /*06000162*/ public hidebysig static 
          bool  TryGetExport(native int handle,
                             string name,
                             [out] native int& address) cil managed
  // SIG: 00 03 02 18 0E 10 18
  {
    // Method begins at RVA 0x25ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeLibrary::TryGetExport

  .method /*06000163*/ public hidebysig static 
          bool  TryLoad(string libraryPath,
                        [out] native int& handle) cil managed
  // SIG: 00 02 02 0E 10 18
  {
    // Method begins at RVA 0x25bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeLibrary::TryLoad

  .method /*06000164*/ public hidebysig static 
          bool  TryLoad(string libraryName,
                        class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003A*/ 'assembly',
                        valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100003B*/<valuetype System.Runtime.InteropServices.DllImportSearchPath/*0200002F*/> searchPath,
                        [out] native int& handle) cil managed
  // SIG: 00 04 02 0E 12 80 E9 15 11 80 ED 01 11 80 BC 10 18
  {
    // Method begins at RVA 0x25c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeLibrary::TryLoad

} // end of class System.Runtime.InteropServices.NativeLibrary

.class /*02000044*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.NativeMemory
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .method /*06000165*/ public hidebysig static 
          void*  AlignedAlloc(native uint byteCount,
                              native uint 'alignment') cil managed
  // SIG: 00 02 0F 01 19 19
  {
    .custom /*0C000204:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080001D5*/ 
    .custom /*0C000224:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    .param [2]/*080001D6*/ 
    .custom /*0C000225:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeMemory::AlignedAlloc

  .method /*06000166*/ public hidebysig static 
          void  AlignedFree(void* ptr) cil managed
  // SIG: 00 01 01 0F 01
  {
    .custom /*0C000205:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NativeMemory::AlignedFree

  .method /*06000167*/ public hidebysig static 
          void*  AlignedRealloc(void* ptr,
                                native uint byteCount,
                                native uint 'alignment') cil managed
  // SIG: 00 03 0F 01 0F 01 19 19
  {
    .custom /*0C000206:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080001D9*/ 
    .custom /*0C000227:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    .param [3]/*080001DA*/ 
    .custom /*0C000229:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeMemory::AlignedRealloc

  .method /*06000168*/ public hidebysig static 
          void*  Alloc(native uint byteCount) cil managed
  // SIG: 00 01 0F 01 19
  {
    .custom /*0C000207:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080001DB*/ 
    .custom /*0C00022A:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeMemory::Alloc

  .method /*06000169*/ public hidebysig static 
          void*  Alloc(native uint elementCount,
                       native uint elementSize) cil managed
  // SIG: 00 02 0F 01 19 19
  {
    .custom /*0C000208:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080001DC*/ 
    .custom /*0C00022B:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    .param [2]/*080001DD*/ 
    .custom /*0C00022C:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeMemory::Alloc

  .method /*0600016A*/ public hidebysig static 
          void*  AllocZeroed(native uint byteCount) cil managed
  // SIG: 00 01 0F 01 19
  {
    .custom /*0C00020A:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080001DE*/ 
    .custom /*0C00022D:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeMemory::AllocZeroed

  .method /*0600016B*/ public hidebysig static 
          void*  AllocZeroed(native uint elementCount,
                             native uint elementSize) cil managed
  // SIG: 00 02 0F 01 19 19
  {
    .custom /*0C00020B:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080001DF*/ 
    .custom /*0C00022E:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    .param [2]/*080001E0*/ 
    .custom /*0C00022F:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeMemory::AllocZeroed

  .method /*0600016C*/ public hidebysig static 
          void  Free(void* ptr) cil managed
  // SIG: 00 01 01 0F 01
  {
    .custom /*0C00020D:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NativeMemory::Free

  .method /*0600016D*/ public hidebysig static 
          void*  Realloc(void* ptr,
                         native uint byteCount) cil managed
  // SIG: 00 02 0F 01 0F 01 19
  {
    .custom /*0C00020F:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080001E3*/ 
    .custom /*0C000230:06000006*/ instance void System.Runtime.CompilerServices.NativeIntegerAttribute/*02000006*/::.ctor() /* 06000006 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NativeMemory::Realloc

} // end of class System.Runtime.InteropServices.NativeMemory

.class /*02000045*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.NFloat
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000026*/<valuetype System.Runtime.InteropServices.NFloat/*02000045*/>/*1B000003*/
{
  .pack 0
  .size 1
  .custom /*0C0000BF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000020*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .method /*0600016E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x25dc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NFloat::.ctor

  .method /*0600016F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float64 'value') cil managed
  // SIG: 20 01 01 0D
  {
    // Method begins at RVA 0x25de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NFloat::.ctor

  .method /*06000170*/ public hidebysig specialname 
          instance float64  get_Value() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x25e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NFloat::get_Value

  .method /*06000171*/ public hidebysig virtual 
          instance bool  Equals(object o) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000210:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001E6*/ 
    .custom /*0C000231:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000022*/::.ctor(bool) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NFloat::Equals

  .method /*06000172*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Runtime.InteropServices.NFloat/*02000045*/ other) cil managed
  // SIG: 20 01 02 11 81 14
  {
    // Method begins at RVA 0x25e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NFloat::Equals

  .method /*06000173*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x25e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NFloat::GetHashCode

  .method /*06000174*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000211:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NFloat::ToString

  .property /*1700002D*/ instance callconv(8) float64
          Value()
  {
    .get instance float64 System.Runtime.InteropServices.NFloat/*02000045*/::get_Value() /* 06000170 */
  } // end of property NFloat::Value
} // end of class System.Runtime.InteropServices.NFloat

.class /*02000046*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.OptionalAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000C1:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 08 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000175*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x25ef
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method OptionalAttribute::.ctor

} // end of class System.Runtime.InteropServices.OptionalAttribute

.class /*02000047*/ public auto ansi sealed System.Runtime.InteropServices.PosixSignal
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*0400003F*/ public specialname rtspecialname int32 value__
  .field /*04000040*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGTSTP = int32(0xFFFFFFF6)
  .custom /*0C0000AF:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .field /*04000041*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGTTOU = int32(0xFFFFFFF7)
  .custom /*0C0000B2:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .field /*04000042*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGTTIN = int32(0xFFFFFFF8)
  .custom /*0C0000B7:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .field /*04000043*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGWINCH = int32(0xFFFFFFF9)
  .custom /*0C0000BA:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .field /*04000044*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGCONT = int32(0xFFFFFFFA)
  .custom /*0C0000BD:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .field /*04000045*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGCHLD = int32(0xFFFFFFFB)
  .custom /*0C0000BE:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .field /*04000046*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGTERM = int32(0xFFFFFFFC)
  .field /*04000047*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGQUIT = int32(0xFFFFFFFD)
  .field /*04000048*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGINT = int32(0xFFFFFFFE)
  .field /*04000049*/ public static literal valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ SIGHUP = int32(0xFFFFFFFF)
} // end of class System.Runtime.InteropServices.PosixSignal

.class /*02000048*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.PosixSignalContext
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .method /*06000176*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ signal) cil managed
  // SIG: 20 01 01 11 81 1C
  {
    // Method begins at RVA 0x25f7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PosixSignalContext::.ctor

  .method /*06000177*/ public hidebysig specialname 
          instance bool  get_Cancel() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixSignalContext::get_Cancel

  .method /*06000178*/ public hidebysig specialname 
          instance void  set_Cancel(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2602
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixSignalContext::set_Cancel

  .method /*06000179*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ 
          get_Signal() cil managed
  // SIG: 20 00 11 81 1C
  {
    // Method begins at RVA 0x2604
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixSignalContext::get_Signal

  .property /*1700002E*/ instance callconv(8) bool
          Cancel()
  {
    .get instance bool System.Runtime.InteropServices.PosixSignalContext/*02000048*/::get_Cancel() /* 06000177 */
    .set instance void System.Runtime.InteropServices.PosixSignalContext/*02000048*/::set_Cancel(bool) /* 06000178 */
  } // end of property PosixSignalContext::Cancel
  .property /*1700002F*/ instance callconv(8) valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/
          Signal()
  {
    .get instance valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ System.Runtime.InteropServices.PosixSignalContext/*02000048*/::get_Signal() /* 06000179 */
  } // end of property PosixSignalContext::Signal
} // end of class System.Runtime.InteropServices.PosixSignalContext

.class /*02000049*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.PosixSignalRegistration
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001E*/
{
  .method /*0600017A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2607
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PosixSignalRegistration::.ctor

  .method /*0600017B*/ public hidebysig static 
          class System.Runtime.InteropServices.PosixSignalRegistration/*02000049*/ 
          Create(valuetype System.Runtime.InteropServices.PosixSignal/*02000047*/ signal,
                 class [System.Runtime/*23000001*/]System.Action`1/*01000047*/<class System.Runtime.InteropServices.PosixSignalContext/*02000048*/> 'handler') cil managed
  // SIG: 00 02 12 81 24 11 81 1C 15 12 81 1D 01 12 81 20
  {
    .custom /*0C000212:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000213:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
    .custom /*0C000214:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .custom /*0C000215:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C000216:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    // Method begins at RVA 0x260f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixSignalRegistration::Create

  .method /*0600017C*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2612
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixSignalRegistration::Dispose

  .method /*0600017D*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*0100001D*/::Finalize /*0100001D::0A00001F*/ 
    // Method begins at RVA 0x2614
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::Finalize() /* 0A00001F */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method PosixSignalRegistration::Finalize

} // end of class System.Runtime.InteropServices.PosixSignalRegistration

.class /*0200004A*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.PreserveSigAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000C6:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ..@.....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600017E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x263c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PreserveSigAttribute::.ctor

} // end of class System.Runtime.InteropServices.PreserveSigAttribute

.class /*0200004B*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.PrimaryInteropAssemblyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000C7:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 01 00 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                                          74 69 70 6C 65 01 )                               // tiple.
  .method /*0600017F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 major,
                               int32 minor) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x2644
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PrimaryInteropAssemblyAttribute::.ctor

  .method /*06000180*/ public hidebysig specialname 
          instance int32  get_MajorVersion() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x264c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PrimaryInteropAssemblyAttribute::get_MajorVersion

  .method /*06000181*/ public hidebysig specialname 
          instance int32  get_MinorVersion() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x264f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PrimaryInteropAssemblyAttribute::get_MinorVersion

  .property /*17000030*/ instance callconv(8) int32
          MajorVersion()
  {
    .get instance int32 System.Runtime.InteropServices.PrimaryInteropAssemblyAttribute/*0200004B*/::get_MajorVersion() /* 06000180 */
  } // end of property PrimaryInteropAssemblyAttribute::MajorVersion
  .property /*17000031*/ instance callconv(8) int32
          MinorVersion()
  {
    .get instance int32 System.Runtime.InteropServices.PrimaryInteropAssemblyAttribute/*0200004B*/::get_MinorVersion() /* 06000181 */
  } // end of property PrimaryInteropAssemblyAttribute::MinorVersion
} // end of class System.Runtime.InteropServices.PrimaryInteropAssemblyAttribute

.class /*0200004C*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ProgIdAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000CA:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 04 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000182*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string progId) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2652
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProgIdAttribute::.ctor

  .method /*06000183*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x265a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProgIdAttribute::get_Value

  .property /*17000032*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Runtime.InteropServices.ProgIdAttribute/*0200004C*/::get_Value() /* 06000183 */
  } // end of property ProgIdAttribute::Value
} // end of class System.Runtime.InteropServices.ProgIdAttribute

.class /*0200004D*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.RuntimeEnvironment
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C0000CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000184*/ public hidebysig specialname static 
          string  get_SystemConfigurationFile() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x265d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeEnvironment::get_SystemConfigurationFile

  .method /*06000185*/ public hidebysig static 
          bool  FromGlobalAccessCache(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003A*/ a) cil managed
  // SIG: 00 01 02 12 80 E9
  {
    // Method begins at RVA 0x2660
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeEnvironment::FromGlobalAccessCache

  .method /*06000186*/ public hidebysig static 
          string  GetRuntimeDirectory() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2663
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeEnvironment::GetRuntimeDirectory

  .method /*06000187*/ public hidebysig static 
          native int  GetRuntimeInterfaceAsIntPtr(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 'clsid',
                                                  valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ riid) cil managed
  // SIG: 00 02 18 11 80 D5 11 80 D5
  {
    .custom /*0C000218:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 AB 52 75 6E 74 69 6D 65 45 6E 76 69 72   // ....RuntimeEnvir
                                                                                                                                                   6F 6E 6D 65 6E 74 20 6D 65 6D 62 65 72 73 20 53   // onment members S
                                                                                                                                                   79 73 74 65 6D 43 6F 6E 66 69 67 75 72 61 74 69   // ystemConfigurati
                                                                                                                                                   6F 6E 46 69 6C 65 2C 20 47 65 74 52 75 6E 74 69   // onFile, GetRunti
                                                                                                                                                   6D 65 49 6E 74 65 72 66 61 63 65 41 73 49 6E 74   // meInterfaceAsInt
                                                                                                                                                   50 74 72 2C 20 61 6E 64 20 47 65 74 52 75 6E 74   // Ptr, and GetRunt
                                                                                                                                                   69 6D 65 49 6E 74 65 72 66 61 63 65 41 73 4F 62   // imeInterfaceAsOb
                                                                                                                                                   6A 65 63 74 20 61 72 65 20 6E 6F 74 20 73 75 70   // ject are not sup
                                                                                                                                                   70 6F 72 74 65 64 20 61 6E 64 20 74 68 72 6F 77   // ported and throw
                                                                                                                                                   20 50 6C 61 74 66 6F 72 6D 4E 6F 74 53 75 70 70   //  PlatformNotSupp
                                                                                                                                                   6F 72 74 65 64 45 78 63 65 70 74 69 6F 6E 2E 02   // ortedException..
                                                                                                                                                   00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49 64   // .T..DiagnosticId
                                                                                                                                                   0A 53 59 53 4C 49 42 30 30 31 39 54 0E 09 55 72   // .SYSLIB0019T..Ur
                                                                                                                                                   6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F   // lFormat"https://
                                                                                                                                                   61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61   // aka.ms/dotnet-wa
                                                                                                                                                   72 6E 69 6E 67 73 2F 7B 30 7D )                   // rnings/{0}
    // Method begins at RVA 0x2666
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeEnvironment::GetRuntimeInterfaceAsIntPtr

  .method /*06000188*/ public hidebysig static 
          object  GetRuntimeInterfaceAsObject(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 'clsid',
                                              valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ riid) cil managed
  // SIG: 00 02 1C 11 80 D5 11 80 D5
  {
    .custom /*0C000219:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 AB 52 75 6E 74 69 6D 65 45 6E 76 69 72   // ....RuntimeEnvir
                                                                                                                                                   6F 6E 6D 65 6E 74 20 6D 65 6D 62 65 72 73 20 53   // onment members S
                                                                                                                                                   79 73 74 65 6D 43 6F 6E 66 69 67 75 72 61 74 69   // ystemConfigurati
                                                                                                                                                   6F 6E 46 69 6C 65 2C 20 47 65 74 52 75 6E 74 69   // onFile, GetRunti
                                                                                                                                                   6D 65 49 6E 74 65 72 66 61 63 65 41 73 49 6E 74   // meInterfaceAsInt
                                                                                                                                                   50 74 72 2C 20 61 6E 64 20 47 65 74 52 75 6E 74   // Ptr, and GetRunt
                                                                                                                                                   69 6D 65 49 6E 74 65 72 66 61 63 65 41 73 4F 62   // imeInterfaceAsOb
                                                                                                                                                   6A 65 63 74 20 61 72 65 20 6E 6F 74 20 73 75 70   // ject are not sup
                                                                                                                                                   70 6F 72 74 65 64 20 61 6E 64 20 74 68 72 6F 77   // ported and throw
                                                                                                                                                   20 50 6C 61 74 66 6F 72 6D 4E 6F 74 53 75 70 70   //  PlatformNotSupp
                                                                                                                                                   6F 72 74 65 64 45 78 63 65 70 74 69 6F 6E 2E 02   // ortedException..
                                                                                                                                                   00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49 64   // .T..DiagnosticId
                                                                                                                                                   0A 53 59 53 4C 49 42 30 30 31 39 54 0E 09 55 72   // .SYSLIB0019T..Ur
                                                                                                                                                   6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F   // lFormat"https://
                                                                                                                                                   61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61   // aka.ms/dotnet-wa
                                                                                                                                                   72 6E 69 6E 67 73 2F 7B 30 7D )                   // rnings/{0}
    // Method begins at RVA 0x2669
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeEnvironment::GetRuntimeInterfaceAsObject

  .method /*06000189*/ public hidebysig static 
          string  GetSystemVersion() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x266c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeEnvironment::GetSystemVersion

  .property /*17000033*/ callconv(8) string
          SystemConfigurationFile()
  {
    .custom /*0C000097:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 AB 52 75 6E 74 69 6D 65 45 6E 76 69 72   // ....RuntimeEnvir
                                                                                                                                                   6F 6E 6D 65 6E 74 20 6D 65 6D 62 65 72 73 20 53   // onment members S
                                                                                                                                                   79 73 74 65 6D 43 6F 6E 66 69 67 75 72 61 74 69   // ystemConfigurati
                                                                                                                                                   6F 6E 46 69 6C 65 2C 20 47 65 74 52 75 6E 74 69   // onFile, GetRunti
                                                                                                                                                   6D 65 49 6E 74 65 72 66 61 63 65 41 73 49 6E 74   // meInterfaceAsInt
                                                                                                                                                   50 74 72 2C 20 61 6E 64 20 47 65 74 52 75 6E 74   // Ptr, and GetRunt
                                                                                                                                                   69 6D 65 49 6E 74 65 72 66 61 63 65 41 73 4F 62   // imeInterfaceAsOb
                                                                                                                                                   6A 65 63 74 20 61 72 65 20 6E 6F 74 20 73 75 70   // ject are not sup
                                                                                                                                                   70 6F 72 74 65 64 20 61 6E 64 20 74 68 72 6F 77   // ported and throw
                                                                                                                                                   20 50 6C 61 74 66 6F 72 6D 4E 6F 74 53 75 70 70   //  PlatformNotSupp
                                                                                                                                                   6F 72 74 65 64 45 78 63 65 70 74 69 6F 6E 2E 02   // ortedException..
                                                                                                                                                   00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49 64   // .T..DiagnosticId
                                                                                                                                                   0A 53 59 53 4C 49 42 30 30 31 39 54 0E 09 55 72   // .SYSLIB0019T..Ur
                                                                                                                                                   6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F   // lFormat"https://
                                                                                                                                                   61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61   // aka.ms/dotnet-wa
                                                                                                                                                   72 6E 69 6E 67 73 2F 7B 30 7D )                   // rnings/{0}
    .get string System.Runtime.InteropServices.RuntimeEnvironment/*0200004D*/::get_SystemConfigurationFile() /* 06000184 */
  } // end of property RuntimeEnvironment::SystemConfigurationFile
} // end of class System.Runtime.InteropServices.RuntimeEnvironment

.class /*0200004E*/ public auto ansi beforefieldinit System.Runtime.InteropServices.SafeArrayRankMismatchException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000018*/
{
  .custom /*0C0000D0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000D1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600018A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x266f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SafeArrayRankMismatchException::.ctor

  .method /*0600018B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C00021A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2677
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SafeArrayRankMismatchException::.ctor

  .method /*0600018C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x267f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SafeArrayRankMismatchException::.ctor

  .method /*0600018D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ inner) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x2687
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SafeArrayRankMismatchException::.ctor

} // end of class System.Runtime.InteropServices.SafeArrayRankMismatchException

.class /*0200004F*/ public auto ansi beforefieldinit System.Runtime.InteropServices.SafeArrayTypeMismatchException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000018*/
{
  .custom /*0C0000D2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000D3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600018E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x268f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SafeArrayTypeMismatchException::.ctor

  .method /*0600018F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C00021B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2697
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SafeArrayTypeMismatchException::.ctor

  .method /*06000190*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x269f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SafeArrayTypeMismatchException::.ctor

  .method /*06000191*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ inner) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x26a7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000018*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SafeArrayTypeMismatchException::.ctor

} // end of class System.Runtime.InteropServices.SafeArrayTypeMismatchException

.class /*02000050*/ public auto ansi beforefieldinit System.Runtime.InteropServices.SEHException
       extends [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/
{
  .custom /*0C0000D5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000D6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000192*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26af
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SEHException::.ctor

  .method /*06000193*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C00021C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26b7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SEHException::.ctor

  .method /*06000194*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x26bf
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SEHException::.ctor

  .method /*06000195*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000019*/ inner) cil managed
  // SIG: 20 02 01 0E 12 65
  {
    // Method begins at RVA 0x26c7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.InteropServices.ExternalException/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SEHException::.ctor

  .method /*06000196*/ public hidebysig newslot virtual 
          instance bool  CanResume() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x26cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SEHException::CanResume

} // end of class System.Runtime.InteropServices.SEHException

.class /*02000051*/ public auto ansi beforefieldinit System.Runtime.InteropServices.StandardOleMarshalObject
       extends [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003F*/
{
  .method /*06000197*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26d2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003F*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StandardOleMarshalObject::.ctor

} // end of class System.Runtime.InteropServices.StandardOleMarshalObject

.class /*02000052*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.TypeIdentifierAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000D9:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 18 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .method /*06000198*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26da
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeIdentifierAttribute::.ctor

  .method /*06000199*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string scope,
                               string identifier) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x26e2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeIdentifierAttribute::.ctor

  .method /*0600019A*/ public hidebysig specialname 
          instance string  get_Identifier() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x26ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeIdentifierAttribute::get_Identifier

  .method /*0600019B*/ public hidebysig specialname 
          instance string  get_Scope() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x26ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeIdentifierAttribute::get_Scope

  .property /*17000034*/ instance callconv(8) string
          Identifier()
  {
    .get instance string System.Runtime.InteropServices.TypeIdentifierAttribute/*02000052*/::get_Identifier() /* 0600019A */
  } // end of property TypeIdentifierAttribute::Identifier
  .property /*17000035*/ instance callconv(8) string
          Scope()
  {
    .get instance string System.Runtime.InteropServices.TypeIdentifierAttribute/*02000052*/::get_Scope() /* 0600019B */
  } // end of property TypeIdentifierAttribute::Scope
} // end of class System.Runtime.InteropServices.TypeIdentifierAttribute

.class /*02000053*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.TypeLibFuncAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000DA:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ..@.....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600019C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int16 'flags') cil managed
  // SIG: 20 01 01 06
  {
    // Method begins at RVA 0x26f0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeLibFuncAttribute::.ctor

  .method /*0600019D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ 'flags') cil managed
  // SIG: 20 01 01 11 81 50
  {
    // Method begins at RVA 0x26f8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeLibFuncAttribute::.ctor

  .method /*0600019E*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ 
          get_Value() cil managed
  // SIG: 20 00 11 81 50
  {
    // Method begins at RVA 0x2700
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeLibFuncAttribute::get_Value

  .property /*17000036*/ instance callconv(8) valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/
          Value()
  {
    .get instance valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ System.Runtime.InteropServices.TypeLibFuncAttribute/*02000053*/::get_Value() /* 0600019E */
  } // end of property TypeLibFuncAttribute::Value
} // end of class System.Runtime.InteropServices.TypeLibFuncAttribute

.class /*02000054*/ public auto ansi sealed System.Runtime.InteropServices.TypeLibFuncFlags
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C0000DB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*0400004A*/ public specialname rtspecialname int32 value__
  .field /*0400004B*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FRestricted = int32(0x00000001)
  .field /*0400004C*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FSource = int32(0x00000002)
  .field /*0400004D*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FBindable = int32(0x00000004)
  .field /*0400004E*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FRequestEdit = int32(0x00000008)
  .field /*0400004F*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FDisplayBind = int32(0x00000010)
  .field /*04000050*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FDefaultBind = int32(0x00000020)
  .field /*04000051*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FHidden = int32(0x00000040)
  .field /*04000052*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FUsesGetLastError = int32(0x00000080)
  .field /*04000053*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FDefaultCollelem = int32(0x00000100)
  .field /*04000054*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FUiDefault = int32(0x00000200)
  .field /*04000055*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FNonBrowsable = int32(0x00000400)
  .field /*04000056*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FReplaceable = int32(0x00000800)
  .field /*04000057*/ public static literal valuetype System.Runtime.InteropServices.TypeLibFuncFlags/*02000054*/ FImmediateBind = int32(0x00001000)
} // end of class System.Runtime.InteropServices.TypeLibFuncFlags

.class /*02000055*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.TypeLibImportClassAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000DC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000DD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000DE:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 04 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600019F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000027*/ importClass) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2703
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeLibImportClassAttribute::.ctor

  .method /*060001A0*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x270b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeLibImportClassAttribute::get_Value

  .property /*17000037*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Runtime.InteropServices.TypeLibImportClassAttribute/*02000055*/::get_Value() /* 060001A0 */
  } // end of property TypeLibImportClassAttribute::Value
} // end of class System.Runtime.InteropServices.TypeLibImportClassAttribute

.class /*02000056*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.TypeLibTypeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000DF:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 1C 04 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*060001A1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int16 'flags') cil managed
  // SIG: 20 01 01 06
  {
    // Method begins at RVA 0x270e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeLibTypeAttribute::.ctor

  .method /*060001A2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ 'flags') cil managed
  // SIG: 20 01 01 11 81 5C
  {
    // Method begins at RVA 0x2716
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeLibTypeAttribute::.ctor

  .method /*060001A3*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ 
          get_Value() cil managed
  // SIG: 20 00 11 81 5C
  {
    // Method begins at RVA 0x271e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeLibTypeAttribute::get_Value

  .property /*17000038*/ instance callconv(8) valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/
          Value()
  {
    .get instance valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ System.Runtime.InteropServices.TypeLibTypeAttribute/*02000056*/::get_Value() /* 060001A3 */
  } // end of property TypeLibTypeAttribute::Value
} // end of class System.Runtime.InteropServices.TypeLibTypeAttribute

.class /*02000057*/ public auto ansi sealed System.Runtime.InteropServices.TypeLibTypeFlags
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C0000E0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000058*/ public specialname rtspecialname int32 value__
  .field /*04000059*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FAppObject = int32(0x00000001)
  .field /*0400005A*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FCanCreate = int32(0x00000002)
  .field /*0400005B*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FLicensed = int32(0x00000004)
  .field /*0400005C*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FPreDeclId = int32(0x00000008)
  .field /*0400005D*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FHidden = int32(0x00000010)
  .field /*0400005E*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FControl = int32(0x00000020)
  .field /*0400005F*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FDual = int32(0x00000040)
  .field /*04000060*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FNonExtensible = int32(0x00000080)
  .field /*04000061*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FOleAutomation = int32(0x00000100)
  .field /*04000062*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FRestricted = int32(0x00000200)
  .field /*04000063*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FAggregatable = int32(0x00000400)
  .field /*04000064*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FReplaceable = int32(0x00000800)
  .field /*04000065*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FDispatchable = int32(0x00001000)
  .field /*04000066*/ public static literal valuetype System.Runtime.InteropServices.TypeLibTypeFlags/*02000057*/ FReverseBind = int32(0x00002000)
} // end of class System.Runtime.InteropServices.TypeLibTypeFlags

.class /*02000058*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.TypeLibVarAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000E1:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 01 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*060001A4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int16 'flags') cil managed
  // SIG: 20 01 01 06
  {
    // Method begins at RVA 0x2721
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeLibVarAttribute::.ctor

  .method /*060001A5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ 'flags') cil managed
  // SIG: 20 01 01 11 81 64
  {
    // Method begins at RVA 0x2729
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeLibVarAttribute::.ctor

  .method /*060001A6*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ 
          get_Value() cil managed
  // SIG: 20 00 11 81 64
  {
    // Method begins at RVA 0x2731
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeLibVarAttribute::get_Value

  .property /*17000039*/ instance callconv(8) valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/
          Value()
  {
    .get instance valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ System.Runtime.InteropServices.TypeLibVarAttribute/*02000058*/::get_Value() /* 060001A6 */
  } // end of property TypeLibVarAttribute::Value
} // end of class System.Runtime.InteropServices.TypeLibVarAttribute

.class /*02000059*/ public auto ansi sealed System.Runtime.InteropServices.TypeLibVarFlags
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C0000E2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000067*/ public specialname rtspecialname int32 value__
  .field /*04000068*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FReadOnly = int32(0x00000001)
  .field /*04000069*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FSource = int32(0x00000002)
  .field /*0400006A*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FBindable = int32(0x00000004)
  .field /*0400006B*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FRequestEdit = int32(0x00000008)
  .field /*0400006C*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FDisplayBind = int32(0x00000010)
  .field /*0400006D*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FDefaultBind = int32(0x00000020)
  .field /*0400006E*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FHidden = int32(0x00000040)
  .field /*0400006F*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FRestricted = int32(0x00000080)
  .field /*04000070*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FDefaultCollelem = int32(0x00000100)
  .field /*04000071*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FUiDefault = int32(0x00000200)
  .field /*04000072*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FNonBrowsable = int32(0x00000400)
  .field /*04000073*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FReplaceable = int32(0x00000800)
  .field /*04000074*/ public static literal valuetype System.Runtime.InteropServices.TypeLibVarFlags/*02000059*/ FImmediateBind = int32(0x00001000)
} // end of class System.Runtime.InteropServices.TypeLibVarFlags

.class /*0200005A*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.TypeLibVersionAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000E3:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 01 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*060001A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 major,
                               int32 minor) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x2734
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeLibVersionAttribute::.ctor

  .method /*060001A8*/ public hidebysig specialname 
          instance int32  get_MajorVersion() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x273c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeLibVersionAttribute::get_MajorVersion

  .method /*060001A9*/ public hidebysig specialname 
          instance int32  get_MinorVersion() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x273f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeLibVersionAttribute::get_MinorVersion

  .property /*1700003A*/ instance callconv(8) int32
          MajorVersion()
  {
    .get instance int32 System.Runtime.InteropServices.TypeLibVersionAttribute/*0200005A*/::get_MajorVersion() /* 060001A8 */
  } // end of property TypeLibVersionAttribute::MajorVersion
  .property /*1700003B*/ instance callconv(8) int32
          MinorVersion()
  {
    .get instance int32 System.Runtime.InteropServices.TypeLibVersionAttribute/*0200005A*/::get_MinorVersion() /* 060001A9 */
  } // end of property TypeLibVersionAttribute::MinorVersion
} // end of class System.Runtime.InteropServices.TypeLibVersionAttribute

.class /*0200005B*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.UnknownWrapper
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C0000E4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000E5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000E6:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object obj) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2742
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnknownWrapper::.ctor

  .method /*060001AB*/ public hidebysig specialname 
          instance object  get_WrappedObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x274a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnknownWrapper::get_WrappedObject

  .property /*1700003C*/ instance callconv(8) object
          WrappedObject()
  {
    .get instance object System.Runtime.InteropServices.UnknownWrapper/*0200005B*/::get_WrappedObject() /* 060001AB */
  } // end of property UnknownWrapper::WrappedObject
} // end of class System.Runtime.InteropServices.UnknownWrapper

.class /*0200005C*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000E7:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 10 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000075*/ public bool BestFitMapping
  .field /*04000076*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000038*/ CharSet
  .field /*04000077*/ public bool SetLastError
  .field /*04000078*/ public bool ThrowOnUnmappableChar
  .method /*060001AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ callingConvention) cil managed
  // SIG: 20 01 01 11 40
  {
    // Method begins at RVA 0x274d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnmanagedFunctionPointerAttribute::.ctor

  .method /*060001AD*/ public hidebysig specialname 
          instance valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ 
          get_CallingConvention() cil managed
  // SIG: 20 00 11 40
  {
    // Method begins at RVA 0x2755
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedFunctionPointerAttribute::get_CallingConvention

  .property /*1700003D*/ instance callconv(8) valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/
          CallingConvention()
  {
    .get instance valuetype System.Runtime.InteropServices.CallingConvention/*02000010*/ System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute/*0200005C*/::get_CallingConvention() /* 060001AD */
  } // end of property UnmanagedFunctionPointerAttribute::CallingConvention
} // end of class System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute

.class /*0200005D*/ public auto ansi sealed System.Runtime.InteropServices.UnmanagedType
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*04000079*/ public specialname rtspecialname int32 value__
  .field /*0400007A*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ Bool = int32(0x00000002)
  .field /*0400007B*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ I1 = int32(0x00000003)
  .field /*0400007C*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ U1 = int32(0x00000004)
  .field /*0400007D*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ I2 = int32(0x00000005)
  .field /*0400007E*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ U2 = int32(0x00000006)
  .field /*0400007F*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ I4 = int32(0x00000007)
  .field /*04000080*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ U4 = int32(0x00000008)
  .field /*04000081*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ I8 = int32(0x00000009)
  .field /*04000082*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ U8 = int32(0x0000000A)
  .field /*04000083*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ R4 = int32(0x0000000B)
  .field /*04000084*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ R8 = int32(0x0000000C)
  .field /*04000085*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ Currency = int32(0x0000000F)
  .custom /*0C000140:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000141:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3E 4D 61 72 73 68 61 6C 6C 69 6E 67 20 61   // ..>Marshalling a
                                                                                                                                                 73 20 43 75 72 72 65 6E 63 79 20 6D 61 79 20 62   // s Currency may b
                                                                                                                                                 65 20 75 6E 61 76 61 69 6C 61 62 6C 65 20 69 6E   // e unavailable in
                                                                                                                                                 20 66 75 74 75 72 65 20 72 65 6C 65 61 73 65 73   //  future releases
                                                                                                                                                 2E 00 00 )                                        // ...
  .field /*04000086*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ BStr = int32(0x00000013)
  .field /*04000087*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ LPStr = int32(0x00000014)
  .field /*04000088*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ LPWStr = int32(0x00000015)
  .field /*04000089*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ LPTStr = int32(0x00000016)
  .field /*0400008A*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ ByValTStr = int32(0x00000017)
  .field /*0400008B*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ IUnknown = int32(0x00000019)
  .field /*0400008C*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ IDispatch = int32(0x0000001A)
  .custom /*0C000155:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400008D*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ Struct = int32(0x0000001B)
  .custom /*0C000159:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400008E*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ Interface = int32(0x0000001C)
  .field /*0400008F*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ SafeArray = int32(0x0000001D)
  .custom /*0C000162:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000090*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ ByValArray = int32(0x0000001E)
  .field /*04000091*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ SysInt = int32(0x0000001F)
  .field /*04000092*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ SysUInt = int32(0x00000020)
  .field /*04000093*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ VBByRefStr = int32(0x00000022)
  .custom /*0C00016B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00016C:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 43 4D 61 72 73 68 61 6C 6C 69 6E 67 20 61   // ..CMarshalling a
                                                                                                                                                 73 20 56 42 42 79 52 65 66 53 74 72 69 6E 67 20   // s VBByRefString 
                                                                                                                                                 6D 61 79 20 62 65 20 75 6E 61 76 61 69 6C 61 62   // may be unavailab
                                                                                                                                                 6C 65 20 69 6E 20 66 75 74 75 72 65 20 72 65 6C   // le in future rel
                                                                                                                                                 65 61 73 65 73 2E 00 00 )                         // eases...
  .field /*04000094*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ AnsiBStr = int32(0x00000023)
  .custom /*0C00016E:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00016F:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3E 4D 61 72 73 68 61 6C 6C 69 6E 67 20 61   // ..>Marshalling a
                                                                                                                                                 73 20 41 6E 73 69 42 53 74 72 20 6D 61 79 20 62   // s AnsiBStr may b
                                                                                                                                                 65 20 75 6E 61 76 61 69 6C 61 62 6C 65 20 69 6E   // e unavailable in
                                                                                                                                                 20 66 75 74 75 72 65 20 72 65 6C 65 61 73 65 73   //  future releases
                                                                                                                                                 2E 00 00 )                                        // ...
  .field /*04000095*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ TBStr = int32(0x00000024)
  .custom /*0C000171:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000172:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 3B 4D 61 72 73 68 61 6C 6C 69 6E 67 20 61   // ..;Marshalling a
                                                                                                                                                 73 20 54 42 73 74 72 20 6D 61 79 20 62 65 20 75   // s TBstr may be u
                                                                                                                                                 6E 61 76 61 69 6C 61 62 6C 65 20 69 6E 20 66 75   // navailable in fu
                                                                                                                                                 74 75 72 65 20 72 65 6C 65 61 73 65 73 2E 00 00 ) // ture releases...
  .field /*04000096*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ VariantBool = int32(0x00000025)
  .custom /*0C000174:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000097*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ FunctionPtr = int32(0x00000026)
  .field /*04000098*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ AsAny = int32(0x00000028)
  .custom /*0C000178:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000179:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001F*/::.ctor(string) /* 0A000014 */ = ( 01 00 6B 4D 61 72 73 68 61 6C 6C 69 6E 67 20 61   // ..kMarshalling a
                                                                                                                                                 72 62 69 74 72 61 72 79 20 74 79 70 65 73 20 6D   // rbitrary types m
                                                                                                                                                 61 79 20 62 65 20 75 6E 61 76 61 69 6C 61 62 6C   // ay be unavailabl
                                                                                                                                                 65 20 69 6E 20 66 75 74 75 72 65 20 72 65 6C 65   // e in future rele
                                                                                                                                                 61 73 65 73 2E 20 53 70 65 63 69 66 79 20 74 68   // ases. Specify th
                                                                                                                                                 65 20 74 79 70 65 20 79 6F 75 20 77 69 73 68 20   // e type you wish 
                                                                                                                                                 74 6F 20 6D 61 72 73 68 61 6C 20 61 73 2E 00 00 ) // to marshal as...
  .field /*04000099*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ LPArray = int32(0x0000002A)
  .field /*0400009A*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ LPStruct = int32(0x0000002B)
  .field /*0400009B*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ CustomMarshaler = int32(0x0000002C)
  .field /*0400009C*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ Error = int32(0x0000002D)
  .field /*0400009D*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ IInspectable = int32(0x0000002E)
  .field /*0400009E*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ HString = int32(0x0000002F)
  .field /*0400009F*/ public static literal valuetype System.Runtime.InteropServices.UnmanagedType/*0200005D*/ LPUTF8Str = int32(0x00000030)
} // end of class System.Runtime.InteropServices.UnmanagedType

.class /*0200005E*/ public auto ansi sealed System.Runtime.InteropServices.VarEnum
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C0000E8:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040000A0*/ public specialname rtspecialname int32 value__
  .field /*040000A1*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_EMPTY = int32(0x00000000)
  .field /*040000A2*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_NULL = int32(0x00000001)
  .field /*040000A3*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_I2 = int32(0x00000002)
  .field /*040000A4*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_I4 = int32(0x00000003)
  .field /*040000A5*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_R4 = int32(0x00000004)
  .field /*040000A6*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_R8 = int32(0x00000005)
  .field /*040000A7*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_CY = int32(0x00000006)
  .field /*040000A8*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_DATE = int32(0x00000007)
  .field /*040000A9*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_BSTR = int32(0x00000008)
  .field /*040000AA*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_DISPATCH = int32(0x00000009)
  .field /*040000AB*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_ERROR = int32(0x0000000A)
  .field /*040000AC*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_BOOL = int32(0x0000000B)
  .field /*040000AD*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_VARIANT = int32(0x0000000C)
  .field /*040000AE*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_UNKNOWN = int32(0x0000000D)
  .field /*040000AF*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_DECIMAL = int32(0x0000000E)
  .field /*040000B0*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_I1 = int32(0x00000010)
  .field /*040000B1*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_UI1 = int32(0x00000011)
  .field /*040000B2*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_UI2 = int32(0x00000012)
  .field /*040000B3*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_UI4 = int32(0x00000013)
  .field /*040000B4*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_I8 = int32(0x00000014)
  .field /*040000B5*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_UI8 = int32(0x00000015)
  .field /*040000B6*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_INT = int32(0x00000016)
  .field /*040000B7*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_UINT = int32(0x00000017)
  .field /*040000B8*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_VOID = int32(0x00000018)
  .field /*040000B9*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_HRESULT = int32(0x00000019)
  .field /*040000BA*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_PTR = int32(0x0000001A)
  .field /*040000BB*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_SAFEARRAY = int32(0x0000001B)
  .field /*040000BC*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_CARRAY = int32(0x0000001C)
  .field /*040000BD*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_USERDEFINED = int32(0x0000001D)
  .field /*040000BE*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_LPSTR = int32(0x0000001E)
  .field /*040000BF*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_LPWSTR = int32(0x0000001F)
  .field /*040000C0*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_RECORD = int32(0x00000024)
  .field /*040000C1*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_FILETIME = int32(0x00000040)
  .field /*040000C2*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_BLOB = int32(0x00000041)
  .field /*040000C3*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_STREAM = int32(0x00000042)
  .field /*040000C4*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_STORAGE = int32(0x00000043)
  .field /*040000C5*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_STREAMED_OBJECT = int32(0x00000044)
  .field /*040000C6*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_STORED_OBJECT = int32(0x00000045)
  .field /*040000C7*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_BLOB_OBJECT = int32(0x00000046)
  .field /*040000C8*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_CF = int32(0x00000047)
  .field /*040000C9*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_CLSID = int32(0x00000048)
  .field /*040000CA*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_VECTOR = int32(0x00001000)
  .field /*040000CB*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_ARRAY = int32(0x00002000)
  .field /*040000CC*/ public static literal valuetype System.Runtime.InteropServices.VarEnum/*0200005E*/ VT_BYREF = int32(0x00004000)
} // end of class System.Runtime.InteropServices.VarEnum

.class /*0200005F*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.VariantWrapper
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C0000EA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000EC:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001AE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object obj) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2758
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method VariantWrapper::.ctor

  .method /*060001AF*/ public hidebysig specialname 
          instance object  get_WrappedObject() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2760
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VariantWrapper::get_WrappedObject

  .property /*1700003E*/ instance callconv(8) object
          WrappedObject()
  {
    .get instance object System.Runtime.InteropServices.VariantWrapper/*0200005F*/::get_WrappedObject() /* 060001AF */
  } // end of property VariantWrapper::WrappedObject
} // end of class System.Runtime.InteropServices.VariantWrapper

.class /*02000060*/ public auto ansi sealed System.Runtime.InteropServices.CreateComInterfaceFlags
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C0000ED:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*040000CD*/ public specialname rtspecialname int32 value__
  .field /*040000CE*/ public static literal valuetype System.Runtime.InteropServices.CreateComInterfaceFlags/*02000060*/ None = int32(0x00000000)
  .field /*040000CF*/ public static literal valuetype System.Runtime.InteropServices.CreateComInterfaceFlags/*02000060*/ CallerDefinedIUnknown = int32(0x00000001)
  .field /*040000D0*/ public static literal valuetype System.Runtime.InteropServices.CreateComInterfaceFlags/*02000060*/ TrackerSupport = int32(0x00000002)
} // end of class System.Runtime.InteropServices.CreateComInterfaceFlags

.class /*02000061*/ public auto ansi sealed System.Runtime.InteropServices.CreateObjectFlags
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C0000EE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*040000D1*/ public specialname rtspecialname int32 value__
  .field /*040000D2*/ public static literal valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ None = int32(0x00000000)
  .field /*040000D3*/ public static literal valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ TrackerObject = int32(0x00000001)
  .field /*040000D4*/ public static literal valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ UniqueInstance = int32(0x00000002)
  .field /*040000D5*/ public static literal valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ Aggregation = int32(0x00000004)
  .field /*040000D6*/ public static literal valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ Unwrap = int32(0x00000008)
} // end of class System.Runtime.InteropServices.CreateObjectFlags

.class /*02000062*/ public abstract auto ansi beforefieldinit System.Runtime.InteropServices.ComWrappers
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C0000EF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000F0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000F1:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
  .custom /*0C0000F2:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C0000F3:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
  .custom /*0C0000F4:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000046*/::.ctor(string) /* 0A00001E */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
  .custom /*0C0000F5:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
  .class /*020000A3*/ sequential ansi sealed nested public beforefieldinit ComInterfaceEntry
         extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
  {
    .custom /*0C00018B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .field /*040001CB*/ public valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ IID
    .field /*040001CC*/ public native int Vtable
  } // end of class ComInterfaceEntry

  .class /*020000A4*/ sequential ansi sealed nested public beforefieldinit ComInterfaceDispatch
         extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
  {
    .custom /*0C00018C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .field /*040001CD*/ public native int Vtable
    .method /*060002A4*/ public hidebysig static 
            !!T  GetInstance<class T>(valuetype System.Runtime.InteropServices.ComWrappers/*02000062*//ComInterfaceDispatch/*020000A4*/* dispatchPtr) cil managed
    // SIG: 10 01 01 1E 00 0F 11 82 90
    {
      .custom /*0C000250:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
      .param [1]/*080003D5*/ 
      .custom /*0C000256:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
      // Method begins at RVA 0x283b
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ComInterfaceDispatch::GetInstance

  } // end of class ComInterfaceDispatch

  .method /*060001B0*/ public hidebysig instance native int 
          GetOrCreateComInterfaceForObject(object 'instance',
                                           valuetype System.Runtime.InteropServices.CreateComInterfaceFlags/*02000060*/ 'flags') cil managed
  // SIG: 20 02 18 1C 11 81 80
  {
    // Method begins at RVA 0x2763
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComWrappers::GetOrCreateComInterfaceForObject

  .method /*060001B1*/ family hidebysig newslot abstract virtual 
          instance valuetype System.Runtime.InteropServices.ComWrappers/*02000062*//ComInterfaceEntry/*020000A3*/* 
          ComputeVtables(object obj,
                         valuetype System.Runtime.InteropServices.CreateComInterfaceFlags/*02000060*/ 'flags',
                         [out] int32& count) cil managed
  // SIG: 20 03 0F 11 82 8C 1C 11 81 80 10 08
  {
    .custom /*0C00021D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000213*/ 
    .custom /*0C000235:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ComWrappers::ComputeVtables

  .method /*060001B2*/ public hidebysig instance object 
          GetOrCreateObjectForComInstance(native int externalComObject,
                                          valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ 'flags') cil managed
  // SIG: 20 02 1C 18 11 81 84
  {
    // Method begins at RVA 0x2766
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComWrappers::GetOrCreateObjectForComInstance

  .method /*060001B3*/ family hidebysig newslot abstract virtual 
          instance object  CreateObject(native int externalComObject,
                                        valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ 'flags') cil managed
  // SIG: 20 02 1C 18 11 81 84
  {
    .custom /*0C00021E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ComWrappers::CreateObject

  .method /*060001B4*/ public hidebysig instance object 
          GetOrRegisterObjectForComInstance(native int externalComObject,
                                            valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ 'flags',
                                            object wrapper) cil managed
  // SIG: 20 03 1C 18 11 81 84 1C
  {
    // Method begins at RVA 0x2769
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComWrappers::GetOrRegisterObjectForComInstance

  .method /*060001B5*/ public hidebysig instance object 
          GetOrRegisterObjectForComInstance(native int externalComObject,
                                            valuetype System.Runtime.InteropServices.CreateObjectFlags/*02000061*/ 'flags',
                                            object wrapper,
                                            native int inner) cil managed
  // SIG: 20 04 1C 18 11 81 84 1C 18
  {
    // Method begins at RVA 0x276c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComWrappers::GetOrRegisterObjectForComInstance

  .method /*060001B6*/ family hidebysig newslot abstract virtual 
          instance void  ReleaseObjects(class [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000048*/ objects) cil managed
  // SIG: 20 01 01 12 81 21
  {
    // Method begins at RVA 0x0
  } // end of method ComWrappers::ReleaseObjects

  .method /*060001B7*/ public hidebysig static 
          void  RegisterForTrackerSupport(class System.Runtime.InteropServices.ComWrappers/*02000062*/ 'instance') cil managed
  // SIG: 00 01 01 12 81 88
  {
    // Method begins at RVA 0x276f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ComWrappers::RegisterForTrackerSupport

  .method /*060001B8*/ public hidebysig static 
          void  RegisterForMarshalling(class System.Runtime.InteropServices.ComWrappers/*02000062*/ 'instance') cil managed
  // SIG: 00 01 01 12 81 88
  {
    .custom /*0C000220:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2771
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ComWrappers::RegisterForMarshalling

  .method /*060001B9*/ family hidebysig static 
          void  GetIUnknownImpl([out] native int& fpQueryInterface,
                                [out] native int& fpAddRef,
                                [out] native int& fpRelease) cil managed
  // SIG: 00 03 01 10 18 10 18 10 18
  {
    // Method begins at RVA 0x2773
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComWrappers::GetIUnknownImpl

  .method /*060001BA*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2776
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComWrappers::.ctor

} // end of class System.Runtime.InteropServices.ComWrappers

.class /*02000063*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.UnmanagedCallConvAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000F7:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..@.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*040000D7*/ public class [System.Runtime/*23000001*/]System.Type/*01000027*/[] CallConvs
  .custom /*0C00019D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
  .method /*060001BB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x277e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnmanagedCallConvAttribute::.ctor

} // end of class System.Runtime.InteropServices.UnmanagedCallConvAttribute

.class /*02000064*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000F8:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 40 00 00 00 01 00 54 02 09 49 6E 68 65 72   // ..@.....T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .field /*040000D8*/ public class [System.Runtime/*23000001*/]System.Type/*01000027*/[] CallConvs
  .custom /*0C00019E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
  .field /*040000D9*/ public string EntryPoint
  .custom /*0C00019F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060001BC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2786
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnmanagedCallersOnlyAttribute::.ctor

} // end of class System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute

.class /*02000065*/ public auto ansi sealed beforefieldinit System.Runtime.InteropServices.ObjectiveC.ObjectiveCTrackedTypeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C0000FA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 05 6D 61 63 6F 73 00 00 )                   // ...macos..
  .custom /*0C0000FB:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 04 00 00 00 02 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 01 54 02 0D 41 6C 6C 6F 77 4D 75 6C   // ited.T..AllowMul
                                                                                                                                                                                                                          74 69 70 6C 65 00 )                               // tiple.
  .method /*060001BD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x278e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000015*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ObjectiveCTrackedTypeAttribute::.ctor

} // end of class System.Runtime.InteropServices.ObjectiveC.ObjectiveCTrackedTypeAttribute

.class /*02000066*/ public abstract auto ansi sealed beforefieldinit System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C0000FD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 05 6D 61 63 6F 73 00 00 )                   // ...macos..
  .custom /*0C0000FE:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
  .class /*020000A5*/ auto ansi sealed nested public UnhandledExceptionPropagationHandler
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000039*/
  {
    .method /*060002A5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method UnhandledExceptionPropagationHandler::.ctor

    .method /*060002A6*/ public hidebysig newslot virtual 
            instance method unmanaged void *(native int) 
            Invoke(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ exception,
                   valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100004A*/ lastMethod,
                   [out] native int& context) runtime managed
    // SIG: 20 03 1B 09 01 01 18 12 65 11 81 29 10 18
    {
      .param [1]/*080003D8*/ 
      .custom /*0C000257:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    } // end of method UnhandledExceptionPropagationHandler::Invoke

    .method /*060002A7*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003C*/ 
            BeginInvoke(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ exception,
                        valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100004A*/ lastMethod,
                        [out] native int& context,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*0100003D*/ callback,
                        object 'object') runtime managed
    // SIG: 20 05 12 80 F1 12 65 11 81 29 10 18 12 80 F5 1C
    {
      .param [1]/*080003DB*/ 
      .custom /*0C000258:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    } // end of method UnhandledExceptionPropagationHandler::BeginInvoke

    .method /*060002A8*/ public hidebysig newslot virtual 
            instance method unmanaged void *(native int) 
            EndInvoke([out] native int& context,
                      class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003C*/ result) runtime managed
    // SIG: 20 02 1B 09 01 01 18 10 18 12 80 F1
    {
    } // end of method UnhandledExceptionPropagationHandler::EndInvoke

  } // end of class UnhandledExceptionPropagationHandler

  .class /*020000A6*/ auto ansi sealed nested public MessageSendFunction
         extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
  {
    .field /*040001CE*/ public specialname rtspecialname int32 value__
    .field /*040001CF*/ public static literal valuetype System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal/*02000066*//MessageSendFunction/*020000A6*/ MsgSend = int32(0x00000000)
    .field /*040001D0*/ public static literal valuetype System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal/*02000066*//MessageSendFunction/*020000A6*/ MsgSendFpret = int32(0x00000001)
    .field /*040001D1*/ public static literal valuetype System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal/*02000066*//MessageSendFunction/*020000A6*/ MsgSendStret = int32(0x00000002)
    .field /*040001D2*/ public static literal valuetype System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal/*02000066*//MessageSendFunction/*020000A6*/ MsgSendSuper = int32(0x00000003)
    .field /*040001D3*/ public static literal valuetype System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal/*02000066*//MessageSendFunction/*020000A6*/ MsgSendSuperStret = int32(0x00000004)
  } // end of class MessageSendFunction

  .method /*060001BE*/ public hidebysig static 
          void  Initialize(method unmanaged void *() beginEndCallback,
                           method unmanaged int32 *(native int) isReferencedCallback,
                           method unmanaged void *(native int) trackedObjectEnteredFinalization,
                           class System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal/*02000066*//UnhandledExceptionPropagationHandler/*020000A5*/ unhandledExceptionPropagationHandler) cil managed
  // SIG: 00 04 01 1B 09 00 01 1B 09 01 08 18 1B 09 01 01 18 12 82 94
  {
    .param [4]/*0800022A*/ 
    .custom /*0C000237:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2796
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectiveCMarshal::Initialize

  .method /*060001BF*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.GCHandle/*01000049*/ 
          CreateReferenceTrackingHandle(object obj,
                                        [out] valuetype [System.Runtime/*23000001*/]System.Span`1/*01000028*/<native int>& taggedMemory) cil managed
  // SIG: 00 02 11 81 25 1C 10 15 11 80 A1 01 18
  {
    .param [1]/*0800022B*/ 
    .custom /*0C000238:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x279c
    // Code size       2 (0x2)
    .maxstack  1
    .locals /*11000001*/ init (valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.GCHandle/*01000049*/ V_0)
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectiveCMarshal::CreateReferenceTrackingHandle

  .method /*060001C0*/ public hidebysig static 
          void  SetMessageSendCallback(valuetype System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal/*02000066*//MessageSendFunction/*020000A6*/ msgSendFunction,
                                       native int func) cil managed
  // SIG: 00 02 01 11 82 98 18
  {
    // Method begins at RVA 0x27aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectiveCMarshal::SetMessageSendCallback

  .method /*060001C1*/ public hidebysig static 
          void  SetMessageSendPendingException(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ exception) cil managed
  // SIG: 00 01 01 12 65
  {
    .custom /*0C000221:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectiveCMarshal::SetMessageSendPendingException

} // end of class System.Runtime.InteropServices.ObjectiveC.ObjectiveCMarshal

.class /*02000067*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.ADVF
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000100:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000101:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*040000DA*/ public specialname rtspecialname int32 value__
  .field /*040000DB*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ ADVF_NODATA = int32(0x00000001)
  .field /*040000DC*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ ADVF_PRIMEFIRST = int32(0x00000002)
  .field /*040000DD*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ ADVF_ONLYONCE = int32(0x00000004)
  .field /*040000DE*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ ADVFCACHE_NOHANDLER = int32(0x00000008)
  .field /*040000DF*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ ADVFCACHE_FORCEBUILTIN = int32(0x00000010)
  .field /*040000E0*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ ADVFCACHE_ONSAVE = int32(0x00000020)
  .field /*040000E1*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ ADVF_DATAONSTOP = int32(0x00000040)
} // end of class System.Runtime.InteropServices.ComTypes.ADVF

.class /*02000068*/ public explicit ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.BINDPTR
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000102:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040000E2*/ [0] public native int lpfuncdesc
  .field /*040000E3*/ [0] public native int lptcomp
  .field /*040000E4*/ [0] public native int lpvardesc
} // end of class System.Runtime.InteropServices.ComTypes.BINDPTR

.class /*02000069*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.BIND_OPTS
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000103:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040000E5*/ public int32 cbStruct
  .field /*040000E6*/ public int32 dwTickCountDeadline
  .field /*040000E7*/ public int32 grfFlags
  .field /*040000E8*/ public int32 grfMode
} // end of class System.Runtime.InteropServices.ComTypes.BIND_OPTS

.class /*0200006A*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.CALLCONV
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000104:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040000E9*/ public specialname rtspecialname int32 value__
  .field /*040000EA*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_CDECL = int32(0x00000001)
  .field /*040000EB*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_MSCPASCAL = int32(0x00000002)
  .field /*040000EC*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_PASCAL = int32(0x00000002)
  .field /*040000ED*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_MACPASCAL = int32(0x00000003)
  .field /*040000EE*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_STDCALL = int32(0x00000004)
  .field /*040000EF*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_RESERVED = int32(0x00000005)
  .field /*040000F0*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_SYSCALL = int32(0x00000006)
  .field /*040000F1*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_MPWCDECL = int32(0x00000007)
  .field /*040000F2*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_MPWPASCAL = int32(0x00000008)
  .field /*040000F3*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ CC_MAX = int32(0x00000009)
} // end of class System.Runtime.InteropServices.ComTypes.CALLCONV

.class /*0200006B*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.CONNECTDATA
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000105:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040000F4*/ public int32 dwCookie
  .field /*040000F5*/ public object pUnk
  .custom /*0C0001C3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
} // end of class System.Runtime.InteropServices.ComTypes.CONNECTDATA

.class /*0200006C*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.DATADIR
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000106:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040000F6*/ public specialname rtspecialname int32 value__
  .field /*040000F7*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DATADIR/*0200006C*/ DATADIR_GET = int32(0x00000001)
  .field /*040000F8*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DATADIR/*0200006C*/ DATADIR_SET = int32(0x00000002)
} // end of class System.Runtime.InteropServices.ComTypes.DATADIR

.class /*0200006D*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.DESCKIND
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000107:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040000F9*/ public specialname rtspecialname int32 value__
  .field /*040000FA*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DESCKIND/*0200006D*/ DESCKIND_NONE = int32(0x00000000)
  .field /*040000FB*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DESCKIND/*0200006D*/ DESCKIND_FUNCDESC = int32(0x00000001)
  .field /*040000FC*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DESCKIND/*0200006D*/ DESCKIND_VARDESC = int32(0x00000002)
  .field /*040000FD*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DESCKIND/*0200006D*/ DESCKIND_TYPECOMP = int32(0x00000003)
  .field /*040000FE*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DESCKIND/*0200006D*/ DESCKIND_IMPLICITAPPOBJ = int32(0x00000004)
  .field /*040000FF*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DESCKIND/*0200006D*/ DESCKIND_MAX = int32(0x00000005)
} // end of class System.Runtime.InteropServices.ComTypes.DESCKIND

.class /*0200006E*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.DISPPARAMS
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000108:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000100*/ public int32 cArgs
  .field /*04000101*/ public int32 cNamedArgs
  .field /*04000102*/ public native int rgdispidNamedArgs
  .field /*04000103*/ public native int rgvarg
} // end of class System.Runtime.InteropServices.ComTypes.DISPPARAMS

.class /*0200006F*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.DVASPECT
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000109:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00010A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000104*/ public specialname rtspecialname int32 value__
  .field /*04000105*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DVASPECT/*0200006F*/ DVASPECT_CONTENT = int32(0x00000001)
  .field /*04000106*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DVASPECT/*0200006F*/ DVASPECT_THUMBNAIL = int32(0x00000002)
  .field /*04000107*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DVASPECT/*0200006F*/ DVASPECT_ICON = int32(0x00000004)
  .field /*04000108*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.DVASPECT/*0200006F*/ DVASPECT_DOCPRINT = int32(0x00000008)
} // end of class System.Runtime.InteropServices.ComTypes.DVASPECT

.class /*02000070*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.ELEMDESC
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C00010C:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .class /*020000A7*/ explicit ansi sealed nested public beforefieldinit DESCUNION
         extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
  {
    .field /*040001D4*/ [0] public valuetype System.Runtime.InteropServices.ComTypes.IDLDESC/*0200007C*/ idldesc
    .field /*040001D5*/ [0] public valuetype System.Runtime.InteropServices.ComTypes.PARAMDESC/*02000091*/ paramdesc
  } // end of class DESCUNION

  .field /*04000109*/ public valuetype System.Runtime.InteropServices.ComTypes.ELEMDESC/*02000070*//DESCUNION/*020000A7*/ desc
  .field /*0400010A*/ public valuetype System.Runtime.InteropServices.ComTypes.TYPEDESC/*02000099*/ tdesc
} // end of class System.Runtime.InteropServices.ComTypes.ELEMDESC

.class /*02000071*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.EXCEPINFO
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C00010D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00010E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00010F:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400010B*/ public string bstrDescription
  .field /*0400010C*/ public string bstrHelpFile
  .field /*0400010D*/ public string bstrSource
  .field /*0400010E*/ public int32 dwHelpContext
  .field /*0400010F*/ public native int pfnDeferredFillIn
  .field /*04000110*/ public native int pvReserved
  .field /*04000111*/ public int32 scode
  .field /*04000112*/ public int16 wCode
  .field /*04000113*/ public int16 wReserved
} // end of class System.Runtime.InteropServices.ComTypes.EXCEPINFO

.class /*02000072*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.FILETIME
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000110:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000114*/ public int32 dwHighDateTime
  .field /*04000115*/ public int32 dwLowDateTime
} // end of class System.Runtime.InteropServices.ComTypes.FILETIME

.class /*02000073*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.FORMATETC
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000112:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000116*/ public int16 cfFormat
  .field /*04000117*/ public valuetype System.Runtime.InteropServices.ComTypes.DVASPECT/*0200006F*/ dwAspect
  .field /*04000118*/ public int32 lindex
  .field /*04000119*/ public native int ptd
  .field /*0400011A*/ public valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ tymed
} // end of class System.Runtime.InteropServices.ComTypes.FORMATETC

.class /*02000074*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.FUNCDESC
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000113:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400011B*/ public valuetype System.Runtime.InteropServices.ComTypes.CALLCONV/*0200006A*/ 'callconv'
  .field /*0400011C*/ public int16 cParams
  .field /*0400011D*/ public int16 cParamsOpt
  .field /*0400011E*/ public int16 cScodes
  .field /*0400011F*/ public valuetype System.Runtime.InteropServices.ComTypes.ELEMDESC/*02000070*/ elemdescFunc
  .field /*04000120*/ public valuetype System.Runtime.InteropServices.ComTypes.FUNCKIND/*02000076*/ funckind
  .field /*04000121*/ public valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ invkind
  .field /*04000122*/ public native int lprgelemdescParam
  .field /*04000123*/ public native int lprgscode
  .field /*04000124*/ public int32 memid
  .field /*04000125*/ public int16 oVft
  .field /*04000126*/ public int16 wFuncFlags
} // end of class System.Runtime.InteropServices.ComTypes.FUNCDESC

.class /*02000075*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.FUNCFLAGS
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000115:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000116:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000127*/ public specialname rtspecialname int16 value__
  .field /*04000128*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FRESTRICTED = int16(0x0001)
  .field /*04000129*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FSOURCE = int16(0x0002)
  .field /*0400012A*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FBINDABLE = int16(0x0004)
  .field /*0400012B*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FREQUESTEDIT = int16(0x0008)
  .field /*0400012C*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FDISPLAYBIND = int16(0x0010)
  .field /*0400012D*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FDEFAULTBIND = int16(0x0020)
  .field /*0400012E*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FHIDDEN = int16(0x0040)
  .field /*0400012F*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FUSESGETLASTERROR = int16(0x0080)
  .field /*04000130*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FDEFAULTCOLLELEM = int16(0x0100)
  .field /*04000131*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FUIDEFAULT = int16(0x0200)
  .field /*04000132*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FNONBROWSABLE = int16(0x0400)
  .field /*04000133*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FREPLACEABLE = int16(0x0800)
  .field /*04000134*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCFLAGS/*02000075*/ FUNCFLAG_FIMMEDIATEBIND = int16(0x1000)
} // end of class System.Runtime.InteropServices.ComTypes.FUNCFLAGS

.class /*02000076*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.FUNCKIND
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000118:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000135*/ public specialname rtspecialname int32 value__
  .field /*04000136*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCKIND/*02000076*/ FUNC_VIRTUAL = int32(0x00000000)
  .field /*04000137*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCKIND/*02000076*/ FUNC_PUREVIRTUAL = int32(0x00000001)
  .field /*04000138*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCKIND/*02000076*/ FUNC_NONVIRTUAL = int32(0x00000002)
  .field /*04000139*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCKIND/*02000076*/ FUNC_STATIC = int32(0x00000003)
  .field /*0400013A*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.FUNCKIND/*02000076*/ FUNC_DISPATCH = int32(0x00000004)
} // end of class System.Runtime.InteropServices.ComTypes.FUNCKIND

.class /*02000077*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IAdviseSink
{
  .custom /*0C00011A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00011B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00011C:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001C2*/ public hidebysig newslot abstract virtual 
          instance void  OnClose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IAdviseSink::OnClose

  .method /*060001C3*/ public hidebysig newslot abstract virtual 
          instance void  OnDataChange(valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/& format,
                                      valuetype System.Runtime.InteropServices.ComTypes.STGMEDIUM/*02000095*/& stgmedium) cil managed
  // SIG: 20 02 01 10 11 81 CC 10 11 82 54
  {
    // Method begins at RVA 0x0
  } // end of method IAdviseSink::OnDataChange

  .method /*060001C4*/ public hidebysig newslot abstract virtual 
          instance void  OnRename(class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ moniker) cil managed
  // SIG: 20 01 01 12 82 14
  {
    // Method begins at RVA 0x0
  } // end of method IAdviseSink::OnRename

  .method /*060001C5*/ public hidebysig newslot abstract virtual 
          instance void  OnSave() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IAdviseSink::OnSave

  .method /*060001C6*/ public hidebysig newslot abstract virtual 
          instance void  OnViewChange(int32 aspect,
                                      int32 index) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IAdviseSink::OnViewChange

} // end of class System.Runtime.InteropServices.ComTypes.IAdviseSink

.class /*02000078*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IBindCtx
{
  .custom /*0C00011D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00011E:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00011F:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001C7*/ public hidebysig newslot abstract virtual 
          instance void  EnumObjectParam([out] class System.Runtime.InteropServices.ComTypes.IEnumString/*02000083*/& ppenum) cil managed
  // SIG: 20 01 01 10 12 82 0C
  {
    .custom /*0C000222:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IBindCtx::EnumObjectParam

  .method /*060001C8*/ public hidebysig newslot abstract virtual 
          instance void  GetBindOptions(valuetype System.Runtime.InteropServices.ComTypes.BIND_OPTS/*02000069*/& pbindopts) cil managed
  // SIG: 20 01 01 10 11 81 A4
  {
    // Method begins at RVA 0x0
  } // end of method IBindCtx::GetBindOptions

  .method /*060001C9*/ public hidebysig newslot abstract virtual 
          instance void  GetObjectParam(string pszKey,
                                        [out] object& ppunk) cil managed
  // SIG: 20 02 01 0E 10 1C
  {
    .param [2]/*08000238*/ 
    .custom /*0C00023A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IBindCtx::GetObjectParam

  .method /*060001CA*/ public hidebysig newslot abstract virtual 
          instance void  GetRunningObjectTable([out] class System.Runtime.InteropServices.ComTypes.IRunningObjectTable/*02000089*/& pprot) cil managed
  // SIG: 20 01 01 10 12 82 24
  {
    .custom /*0C000223:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IBindCtx::GetRunningObjectTable

  .method /*060001CB*/ public hidebysig newslot abstract virtual 
          instance void  RegisterObjectBound(object punk) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x0
  } // end of method IBindCtx::RegisterObjectBound

  .method /*060001CC*/ public hidebysig newslot abstract virtual 
          instance void  RegisterObjectParam(string pszKey,
                                             object punk) cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x0
  } // end of method IBindCtx::RegisterObjectParam

  .method /*060001CD*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseBoundObjects() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IBindCtx::ReleaseBoundObjects

  .method /*060001CE*/ public hidebysig newslot abstract virtual 
          instance void  RevokeObjectBound(object punk) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x0
  } // end of method IBindCtx::RevokeObjectBound

  .method /*060001CF*/ public hidebysig newslot abstract virtual 
          instance int32  RevokeObjectParam(string pszKey) cil managed
  // SIG: 20 01 08 0E
  {
    // Method begins at RVA 0x0
  } // end of method IBindCtx::RevokeObjectParam

  .method /*060001D0*/ public hidebysig newslot abstract virtual 
          instance void  SetBindOptions(valuetype System.Runtime.InteropServices.ComTypes.BIND_OPTS/*02000069*/& pbindopts) cil managed
  // SIG: 20 01 01 10 11 81 A4
  {
    // Method begins at RVA 0x0
  } // end of method IBindCtx::SetBindOptions

} // end of class System.Runtime.InteropServices.ComTypes.IBindCtx

.class /*02000079*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IConnectionPoint
{
  .custom /*0C000120:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000121:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000122:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001D1*/ public hidebysig newslot abstract virtual 
          instance void  Advise(object pUnkSink,
                                [out] int32& pdwCookie) cil managed
  // SIG: 20 02 01 1C 10 08
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionPoint::Advise

  .method /*060001D2*/ public hidebysig newslot abstract virtual 
          instance void  EnumConnections([out] class System.Runtime.InteropServices.ComTypes.IEnumConnections/*0200007F*/& ppEnum) cil managed
  // SIG: 20 01 01 10 12 81 FC
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionPoint::EnumConnections

  .method /*060001D3*/ public hidebysig newslot abstract virtual 
          instance void  GetConnectionInterface([out] valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& pIID) cil managed
  // SIG: 20 01 01 10 11 80 D5
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionPoint::GetConnectionInterface

  .method /*060001D4*/ public hidebysig newslot abstract virtual 
          instance void  GetConnectionPointContainer([out] class System.Runtime.InteropServices.ComTypes.IConnectionPointContainer/*0200007A*/& ppCPC) cil managed
  // SIG: 20 01 01 10 12 81 E8
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionPoint::GetConnectionPointContainer

  .method /*060001D5*/ public hidebysig newslot abstract virtual 
          instance void  Unadvise(int32 dwCookie) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionPoint::Unadvise

} // end of class System.Runtime.InteropServices.ComTypes.IConnectionPoint

.class /*0200007A*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IConnectionPointContainer
{
  .custom /*0C000123:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000124:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000125:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001D6*/ public hidebysig newslot abstract virtual 
          instance void  EnumConnectionPoints([out] class System.Runtime.InteropServices.ComTypes.IEnumConnectionPoints/*0200007E*/& ppEnum) cil managed
  // SIG: 20 01 01 10 12 81 F8
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionPointContainer::EnumConnectionPoints

  .method /*060001D7*/ public hidebysig newslot abstract virtual 
          instance void  FindConnectionPoint(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& riid,
                                             [out] class System.Runtime.InteropServices.ComTypes.IConnectionPoint/*02000079*/& ppCP) cil managed
  // SIG: 20 02 01 10 11 80 D5 10 12 81 E4
  {
    .custom /*0C000226:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IConnectionPointContainer::FindConnectionPoint

} // end of class System.Runtime.InteropServices.ComTypes.IConnectionPointContainer

.class /*0200007B*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IDataObject
{
  .custom /*0C000126:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000127:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001D8*/ public hidebysig newslot abstract virtual 
          instance int32  DAdvise(valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/& pFormatetc,
                                  valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ advf,
                                  class System.Runtime.InteropServices.ComTypes.IAdviseSink/*02000077*/ adviseSink,
                                  [out] int32& connection) cil managed
  // SIG: 20 04 08 10 11 81 CC 11 81 9C 12 81 DC 10 08
  {
    // Method begins at RVA 0x0
  } // end of method IDataObject::DAdvise

  .method /*060001D9*/ public hidebysig newslot abstract virtual 
          instance void  DUnadvise(int32 connection) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method IDataObject::DUnadvise

  .method /*060001DA*/ public hidebysig newslot abstract virtual 
          instance int32  EnumDAdvise([out] class System.Runtime.InteropServices.ComTypes.IEnumSTATDATA/*02000082*/& enumAdvise) cil managed
  // SIG: 20 01 08 10 12 82 08
  {
    .custom /*0C000228:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IDataObject::EnumDAdvise

  .method /*060001DB*/ public hidebysig newslot abstract virtual 
          instance class System.Runtime.InteropServices.ComTypes.IEnumFORMATETC/*02000080*/ 
          EnumFormatEtc(valuetype System.Runtime.InteropServices.ComTypes.DATADIR/*0200006C*/ direction) cil managed
  // SIG: 20 01 12 82 00 11 81 B0
  {
    // Method begins at RVA 0x0
  } // end of method IDataObject::EnumFormatEtc

  .method /*060001DC*/ public hidebysig newslot abstract virtual 
          instance int32  GetCanonicalFormatEtc(valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/& formatIn,
                                                [out] valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/& formatOut) cil managed
  // SIG: 20 02 08 10 11 81 CC 10 11 81 CC
  {
    // Method begins at RVA 0x0
  } // end of method IDataObject::GetCanonicalFormatEtc

  .method /*060001DD*/ public hidebysig newslot abstract virtual 
          instance void  GetData(valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/& format,
                                 [out] valuetype System.Runtime.InteropServices.ComTypes.STGMEDIUM/*02000095*/& medium) cil managed
  // SIG: 20 02 01 10 11 81 CC 10 11 82 54
  {
    // Method begins at RVA 0x0
  } // end of method IDataObject::GetData

  .method /*060001DE*/ public hidebysig newslot abstract virtual 
          instance void  GetDataHere(valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/& format,
                                     valuetype System.Runtime.InteropServices.ComTypes.STGMEDIUM/*02000095*/& medium) cil managed
  // SIG: 20 02 01 10 11 81 CC 10 11 82 54
  {
    // Method begins at RVA 0x0
  } // end of method IDataObject::GetDataHere

  .method /*060001DF*/ public hidebysig newslot abstract virtual 
          instance int32  QueryGetData(valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/& format) cil managed
  // SIG: 20 01 08 10 11 81 CC
  {
    // Method begins at RVA 0x0
  } // end of method IDataObject::QueryGetData

  .method /*060001E0*/ public hidebysig newslot abstract virtual 
          instance void  SetData(valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/& formatIn,
                                 valuetype System.Runtime.InteropServices.ComTypes.STGMEDIUM/*02000095*/& medium,
                                 bool release) cil managed
  // SIG: 20 03 01 10 11 81 CC 10 11 82 54 02
  {
    // Method begins at RVA 0x0
  } // end of method IDataObject::SetData

} // end of class System.Runtime.InteropServices.ComTypes.IDataObject

.class /*0200007C*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.IDLDESC
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000128:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400013B*/ public native int dwReserved
  .field /*0400013C*/ public valuetype System.Runtime.InteropServices.ComTypes.IDLFLAG/*0200007D*/ wIDLFlags
} // end of class System.Runtime.InteropServices.ComTypes.IDLDESC

.class /*0200007D*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.IDLFLAG
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000129:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00012A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*0400013D*/ public specialname rtspecialname int16 value__
  .field /*0400013E*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IDLFLAG/*0200007D*/ IDLFLAG_NONE = int16(0x0000)
  .field /*0400013F*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IDLFLAG/*0200007D*/ IDLFLAG_FIN = int16(0x0001)
  .field /*04000140*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IDLFLAG/*0200007D*/ IDLFLAG_FOUT = int16(0x0002)
  .field /*04000141*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IDLFLAG/*0200007D*/ IDLFLAG_FLCID = int16(0x0004)
  .field /*04000142*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IDLFLAG/*0200007D*/ IDLFLAG_FRETVAL = int16(0x0008)
} // end of class System.Runtime.InteropServices.ComTypes.IDLFLAG

.class /*0200007E*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IEnumConnectionPoints
{
  .custom /*0C00012B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00012C:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00012D:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001E1*/ public hidebysig newslot abstract virtual 
          instance void  Clone([out] class System.Runtime.InteropServices.ComTypes.IEnumConnectionPoints/*0200007E*/& ppenum) cil managed
  // SIG: 20 01 01 10 12 81 F8
  {
    // Method begins at RVA 0x0
  } // end of method IEnumConnectionPoints::Clone

  .method /*060001E2*/ public hidebysig newslot abstract virtual 
          instance int32  Next(int32 celt,
                               class System.Runtime.InteropServices.ComTypes.IConnectionPoint/*02000079*/[] rgelt,
                               native int pceltFetched) cil managed
  // SIG: 20 03 08 08 1D 12 81 E4 18
  {
    // Method begins at RVA 0x0
  } // end of method IEnumConnectionPoints::Next

  .method /*060001E3*/ public hidebysig newslot abstract virtual 
          instance void  Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IEnumConnectionPoints::Reset

  .method /*060001E4*/ public hidebysig newslot abstract virtual 
          instance int32  Skip(int32 celt) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumConnectionPoints::Skip

} // end of class System.Runtime.InteropServices.ComTypes.IEnumConnectionPoints

.class /*0200007F*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IEnumConnections
{
  .custom /*0C00012E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00012F:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000130:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001E5*/ public hidebysig newslot abstract virtual 
          instance void  Clone([out] class System.Runtime.InteropServices.ComTypes.IEnumConnections/*0200007F*/& ppenum) cil managed
  // SIG: 20 01 01 10 12 81 FC
  {
    // Method begins at RVA 0x0
  } // end of method IEnumConnections::Clone

  .method /*060001E6*/ public hidebysig newslot abstract virtual 
          instance int32  Next(int32 celt,
                               valuetype System.Runtime.InteropServices.ComTypes.CONNECTDATA/*0200006B*/[] rgelt,
                               native int pceltFetched) cil managed
  // SIG: 20 03 08 08 1D 11 81 AC 18
  {
    // Method begins at RVA 0x0
  } // end of method IEnumConnections::Next

  .method /*060001E7*/ public hidebysig newslot abstract virtual 
          instance void  Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IEnumConnections::Reset

  .method /*060001E8*/ public hidebysig newslot abstract virtual 
          instance int32  Skip(int32 celt) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumConnections::Skip

} // end of class System.Runtime.InteropServices.ComTypes.IEnumConnections

.class /*02000080*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IEnumFORMATETC
{
  .custom /*0C000131:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000132:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000133:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001E9*/ public hidebysig newslot abstract virtual 
          instance void  Clone([out] class System.Runtime.InteropServices.ComTypes.IEnumFORMATETC/*02000080*/& newEnum) cil managed
  // SIG: 20 01 01 10 12 82 00
  {
    // Method begins at RVA 0x0
  } // end of method IEnumFORMATETC::Clone

  .method /*060001EA*/ public hidebysig newslot abstract virtual 
          instance int32  Next(int32 celt,
                               valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/[] rgelt,
                               int32[] pceltFetched) cil managed
  // SIG: 20 03 08 08 1D 11 81 CC 1D 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumFORMATETC::Next

  .method /*060001EB*/ public hidebysig newslot abstract virtual 
          instance int32  Reset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumFORMATETC::Reset

  .method /*060001EC*/ public hidebysig newslot abstract virtual 
          instance int32  Skip(int32 celt) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumFORMATETC::Skip

} // end of class System.Runtime.InteropServices.ComTypes.IEnumFORMATETC

.class /*02000081*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IEnumMoniker
{
  .custom /*0C000134:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000135:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000136:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001ED*/ public hidebysig newslot abstract virtual 
          instance void  Clone([out] class System.Runtime.InteropServices.ComTypes.IEnumMoniker/*02000081*/& ppenum) cil managed
  // SIG: 20 01 01 10 12 82 04
  {
    // Method begins at RVA 0x0
  } // end of method IEnumMoniker::Clone

  .method /*060001EE*/ public hidebysig newslot abstract virtual 
          instance int32  Next(int32 celt,
                               class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/[] rgelt,
                               native int pceltFetched) cil managed
  // SIG: 20 03 08 08 1D 12 82 14 18
  {
    // Method begins at RVA 0x0
  } // end of method IEnumMoniker::Next

  .method /*060001EF*/ public hidebysig newslot abstract virtual 
          instance void  Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IEnumMoniker::Reset

  .method /*060001F0*/ public hidebysig newslot abstract virtual 
          instance int32  Skip(int32 celt) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumMoniker::Skip

} // end of class System.Runtime.InteropServices.ComTypes.IEnumMoniker

.class /*02000082*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IEnumSTATDATA
{
  .custom /*0C000137:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000138:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001F1*/ public hidebysig newslot abstract virtual 
          instance void  Clone([out] class System.Runtime.InteropServices.ComTypes.IEnumSTATDATA/*02000082*/& newEnum) cil managed
  // SIG: 20 01 01 10 12 82 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumSTATDATA::Clone

  .method /*060001F2*/ public hidebysig newslot abstract virtual 
          instance int32  Next(int32 celt,
                               valuetype System.Runtime.InteropServices.ComTypes.STATDATA/*02000093*/[] rgelt,
                               int32[] pceltFetched) cil managed
  // SIG: 20 03 08 08 1D 11 82 4C 1D 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumSTATDATA::Next

  .method /*060001F3*/ public hidebysig newslot abstract virtual 
          instance int32  Reset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumSTATDATA::Reset

  .method /*060001F4*/ public hidebysig newslot abstract virtual 
          instance int32  Skip(int32 celt) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumSTATDATA::Skip

} // end of class System.Runtime.InteropServices.ComTypes.IEnumSTATDATA

.class /*02000083*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IEnumString
{
  .custom /*0C000139:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00013A:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00013B:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001F5*/ public hidebysig newslot abstract virtual 
          instance void  Clone([out] class System.Runtime.InteropServices.ComTypes.IEnumString/*02000083*/& ppenum) cil managed
  // SIG: 20 01 01 10 12 82 0C
  {
    // Method begins at RVA 0x0
  } // end of method IEnumString::Clone

  .method /*060001F6*/ public hidebysig newslot abstract virtual 
          instance int32  Next(int32 celt,
                               string[] rgelt,
                               native int pceltFetched) cil managed
  // SIG: 20 03 08 08 1D 0E 18
  {
    // Method begins at RVA 0x0
  } // end of method IEnumString::Next

  .method /*060001F7*/ public hidebysig newslot abstract virtual 
          instance void  Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IEnumString::Reset

  .method /*060001F8*/ public hidebysig newslot abstract virtual 
          instance int32  Skip(int32 celt) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumString::Skip

} // end of class System.Runtime.InteropServices.ComTypes.IEnumString

.class /*02000084*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IEnumVARIANT
{
  .custom /*0C00013D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00013E:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00013F:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001F9*/ public hidebysig newslot abstract virtual 
          instance class System.Runtime.InteropServices.ComTypes.IEnumVARIANT/*02000084*/ 
          Clone() cil managed
  // SIG: 20 00 12 82 10
  {
    // Method begins at RVA 0x0
  } // end of method IEnumVARIANT::Clone

  .method /*060001FA*/ public hidebysig newslot abstract virtual 
          instance int32  Next(int32 celt,
                               object[] rgVar,
                               native int pceltFetched) cil managed
  // SIG: 20 03 08 08 1D 1C 18
  {
    .param [2]/*08000279*/ 
    .custom /*0C00023C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IEnumVARIANT::Next

  .method /*060001FB*/ public hidebysig newslot abstract virtual 
          instance int32  Reset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumVARIANT::Reset

  .method /*060001FC*/ public hidebysig newslot abstract virtual 
          instance int32  Skip(int32 celt) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method IEnumVARIANT::Skip

} // end of class System.Runtime.InteropServices.ComTypes.IEnumVARIANT

.class /*02000085*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IMoniker
{
  .custom /*0C000142:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000143:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000144:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060001FD*/ public hidebysig newslot abstract virtual 
          instance void  BindToObject(class System.Runtime.InteropServices.ComTypes.IBindCtx/*02000078*/ pbc,
                                      class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkToLeft,
                                      valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& riidResult,
                                      [out] object& ppvResult) cil managed
  // SIG: 20 04 01 12 81 E0 12 82 14 10 11 80 D5 10 1C
  {
    .param [2]/*0800027D*/ 
    .custom /*0C00023E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::BindToObject

  .method /*060001FE*/ public hidebysig newslot abstract virtual 
          instance void  BindToStorage(class System.Runtime.InteropServices.ComTypes.IBindCtx/*02000078*/ pbc,
                                       class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkToLeft,
                                       valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& riid,
                                       [out] object& ppvObj) cil managed
  // SIG: 20 04 01 12 81 E0 12 82 14 10 11 80 D5 10 1C
  {
    .param [2]/*08000281*/ 
    .custom /*0C000240:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::BindToStorage

  .method /*060001FF*/ public hidebysig newslot abstract virtual 
          instance void  CommonPrefixWith(class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkOther,
                                          [out] class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/& ppmkPrefix) cil managed
  // SIG: 20 02 01 12 82 14 10 12 82 14
  {
    .param [2]/*08000285*/ 
    .custom /*0C000242:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::CommonPrefixWith

  .method /*06000200*/ public hidebysig newslot abstract virtual 
          instance void  ComposeWith(class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkRight,
                                     bool fOnlyIfNotGeneric,
                                     [out] class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/& ppmkComposite) cil managed
  // SIG: 20 03 01 12 82 14 02 10 12 82 14
  {
    .param [3]/*08000288*/ 
    .custom /*0C000243:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::ComposeWith

  .method /*06000201*/ public hidebysig newslot abstract virtual 
          instance void  Enum(bool fForward,
                              [out] class System.Runtime.InteropServices.ComTypes.IEnumMoniker/*02000081*/& ppenumMoniker) cil managed
  // SIG: 20 02 01 02 10 12 82 04
  {
    .custom /*0C000232:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::Enum

  .method /*06000202*/ public hidebysig newslot abstract virtual 
          instance void  GetClassID([out] valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& pClassID) cil managed
  // SIG: 20 01 01 10 11 80 D5
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::GetClassID

  .method /*06000203*/ public hidebysig newslot abstract virtual 
          instance void  GetDisplayName(class System.Runtime.InteropServices.ComTypes.IBindCtx/*02000078*/ pbc,
                                        class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkToLeft,
                                        [out] string& ppszDisplayName) cil managed
  // SIG: 20 03 01 12 81 E0 12 82 14 10 0E
  {
    .param [2]/*0800028D*/ 
    .custom /*0C000244:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::GetDisplayName

  .method /*06000204*/ public hidebysig newslot abstract virtual 
          instance void  GetSizeMax([out] int64& pcbSize) cil managed
  // SIG: 20 01 01 10 0A
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::GetSizeMax

  .method /*06000205*/ public hidebysig newslot abstract virtual 
          instance void  GetTimeOfLastChange(class System.Runtime.InteropServices.ComTypes.IBindCtx/*02000078*/ pbc,
                                             class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkToLeft,
                                             [out] valuetype System.Runtime.InteropServices.ComTypes.FILETIME/*02000072*/& pFileTime) cil managed
  // SIG: 20 03 01 12 81 E0 12 82 14 10 11 81 C8
  {
    .param [2]/*08000291*/ 
    .custom /*0C000247:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::GetTimeOfLastChange

  .method /*06000206*/ public hidebysig newslot abstract virtual 
          instance void  Hash([out] int32& pdwHash) cil managed
  // SIG: 20 01 01 10 08
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::Hash

  .method /*06000207*/ public hidebysig newslot abstract virtual 
          instance void  Inverse([out] class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/& ppmk) cil managed
  // SIG: 20 01 01 10 12 82 14
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::Inverse

  .method /*06000208*/ public hidebysig newslot abstract virtual 
          instance int32  IsDirty() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::IsDirty

  .method /*06000209*/ public hidebysig newslot abstract virtual 
          instance int32  IsEqual(class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkOtherMoniker) cil managed
  // SIG: 20 01 08 12 82 14
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::IsEqual

  .method /*0600020A*/ public hidebysig newslot abstract virtual 
          instance int32  IsRunning(class System.Runtime.InteropServices.ComTypes.IBindCtx/*02000078*/ pbc,
                                    class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkToLeft,
                                    class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkNewlyRunning) cil managed
  // SIG: 20 03 08 12 81 E0 12 82 14 12 82 14
  {
    .custom /*0C000233:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000296*/ 
    .custom /*0C00024A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::IsRunning

  .method /*0600020B*/ public hidebysig newslot abstract virtual 
          instance int32  IsSystemMoniker([out] int32& pdwMksys) cil managed
  // SIG: 20 01 08 10 08
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::IsSystemMoniker

  .method /*0600020C*/ public hidebysig newslot abstract virtual 
          instance void  Load(class System.Runtime.InteropServices.ComTypes.IStream/*0200008A*/ pStm) cil managed
  // SIG: 20 01 01 12 82 28
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::Load

  .method /*0600020D*/ public hidebysig newslot abstract virtual 
          instance void  ParseDisplayName(class System.Runtime.InteropServices.ComTypes.IBindCtx/*02000078*/ pbc,
                                          class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkToLeft,
                                          string pszDisplayName,
                                          [out] int32& pchEaten,
                                          [out] class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/& ppmkOut) cil managed
  // SIG: 20 05 01 12 81 E0 12 82 14 0E 10 08 10 12 82 14
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::ParseDisplayName

  .method /*0600020E*/ public hidebysig newslot abstract virtual 
          instance void  Reduce(class System.Runtime.InteropServices.ComTypes.IBindCtx/*02000078*/ pbc,
                                int32 dwReduceHowFar,
                                class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/& ppmkToLeft,
                                [out] class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/& ppmkReduced) cil managed
  // SIG: 20 04 01 12 81 E0 08 10 12 82 14 10 12 82 14
  {
    .custom /*0C000234:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002A0*/ 
    .custom /*0C00024E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::Reduce

  .method /*0600020F*/ public hidebysig newslot abstract virtual 
          instance void  RelativePathTo(class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkOther,
                                        [out] class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/& ppmkRelPath) cil managed
  // SIG: 20 02 01 12 82 14 10 12 82 14
  {
    .param [2]/*080002A5*/ 
    .custom /*0C000251:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMoniker::RelativePathTo

  .method /*06000210*/ public hidebysig newslot abstract virtual 
          instance void  Save(class System.Runtime.InteropServices.ComTypes.IStream/*0200008A*/ pStm,
                              bool fClearDirty) cil managed
  // SIG: 20 02 01 12 82 28 02
  {
    // Method begins at RVA 0x0
  } // end of method IMoniker::Save

} // end of class System.Runtime.InteropServices.ComTypes.IMoniker

.class /*02000086*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000145:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000146:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000143*/ public specialname rtspecialname int32 value__
  .field /*04000144*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS/*02000086*/ IMPLTYPEFLAG_FDEFAULT = int32(0x00000001)
  .field /*04000145*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS/*02000086*/ IMPLTYPEFLAG_FSOURCE = int32(0x00000002)
  .field /*04000146*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS/*02000086*/ IMPLTYPEFLAG_FRESTRICTED = int32(0x00000004)
  .field /*04000147*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS/*02000086*/ IMPLTYPEFLAG_FDEFAULTVTABLE = int32(0x00000008)
} // end of class System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS

.class /*02000087*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.INVOKEKIND
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000147:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000148:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000148*/ public specialname rtspecialname int32 value__
  .field /*04000149*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ INVOKE_FUNC = int32(0x00000001)
  .field /*0400014A*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ INVOKE_PROPERTYGET = int32(0x00000002)
  .field /*0400014B*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ INVOKE_PROPERTYPUT = int32(0x00000004)
  .field /*0400014C*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ INVOKE_PROPERTYPUTREF = int32(0x00000008)
} // end of class System.Runtime.InteropServices.ComTypes.INVOKEKIND

.class /*02000088*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IPersistFile
{
  .custom /*0C000149:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00014A:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00014B:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000211*/ public hidebysig newslot abstract virtual 
          instance void  GetClassID([out] valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& pClassID) cil managed
  // SIG: 20 01 01 10 11 80 D5
  {
    // Method begins at RVA 0x0
  } // end of method IPersistFile::GetClassID

  .method /*06000212*/ public hidebysig newslot abstract virtual 
          instance void  GetCurFile([out] string& ppszFileName) cil managed
  // SIG: 20 01 01 10 0E
  {
    // Method begins at RVA 0x0
  } // end of method IPersistFile::GetCurFile

  .method /*06000213*/ public hidebysig newslot abstract virtual 
          instance int32  IsDirty() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IPersistFile::IsDirty

  .method /*06000214*/ public hidebysig newslot abstract virtual 
          instance void  Load(string pszFileName,
                              int32 dwMode) cil managed
  // SIG: 20 02 01 0E 08
  {
    // Method begins at RVA 0x0
  } // end of method IPersistFile::Load

  .method /*06000215*/ public hidebysig newslot abstract virtual 
          instance void  Save(string pszFileName,
                              bool fRemember) cil managed
  // SIG: 20 02 01 0E 02
  {
    .custom /*0C000236:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IPersistFile::Save

  .method /*06000216*/ public hidebysig newslot abstract virtual 
          instance void  SaveCompleted(string pszFileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IPersistFile::SaveCompleted

} // end of class System.Runtime.InteropServices.ComTypes.IPersistFile

.class /*02000089*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IRunningObjectTable
{
  .custom /*0C00014C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00014D:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00014E:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000217*/ public hidebysig newslot abstract virtual 
          instance void  EnumRunning([out] class System.Runtime.InteropServices.ComTypes.IEnumMoniker/*02000081*/& ppenumMoniker) cil managed
  // SIG: 20 01 01 10 12 82 04
  {
    // Method begins at RVA 0x0
  } // end of method IRunningObjectTable::EnumRunning

  .method /*06000218*/ public hidebysig newslot abstract virtual 
          instance int32  GetObject(class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkObjectName,
                                    [out] object& ppunkObject) cil managed
  // SIG: 20 02 08 12 82 14 10 1C
  {
    // Method begins at RVA 0x0
  } // end of method IRunningObjectTable::GetObject

  .method /*06000219*/ public hidebysig newslot abstract virtual 
          instance int32  GetTimeOfLastChange(class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkObjectName,
                                              [out] valuetype System.Runtime.InteropServices.ComTypes.FILETIME/*02000072*/& pfiletime) cil managed
  // SIG: 20 02 08 12 82 14 10 11 81 C8
  {
    // Method begins at RVA 0x0
  } // end of method IRunningObjectTable::GetTimeOfLastChange

  .method /*0600021A*/ public hidebysig newslot abstract virtual 
          instance int32  IsRunning(class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkObjectName) cil managed
  // SIG: 20 01 08 12 82 14
  {
    // Method begins at RVA 0x0
  } // end of method IRunningObjectTable::IsRunning

  .method /*0600021B*/ public hidebysig newslot abstract virtual 
          instance void  NoteChangeTime(int32 dwRegister,
                                        valuetype System.Runtime.InteropServices.ComTypes.FILETIME/*02000072*/& pfiletime) cil managed
  // SIG: 20 02 01 08 10 11 81 C8
  {
    // Method begins at RVA 0x0
  } // end of method IRunningObjectTable::NoteChangeTime

  .method /*0600021C*/ public hidebysig newslot abstract virtual 
          instance int32  Register(int32 grfFlags,
                                   object punkObject,
                                   class System.Runtime.InteropServices.ComTypes.IMoniker/*02000085*/ pmkObjectName) cil managed
  // SIG: 20 03 08 08 1C 12 82 14
  {
    // Method begins at RVA 0x0
  } // end of method IRunningObjectTable::Register

  .method /*0600021D*/ public hidebysig newslot abstract virtual 
          instance void  Revoke(int32 dwRegister) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method IRunningObjectTable::Revoke

} // end of class System.Runtime.InteropServices.ComTypes.IRunningObjectTable

.class /*0200008A*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.IStream
{
  .custom /*0C00014F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000150:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000151:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600021E*/ public hidebysig newslot abstract virtual 
          instance void  Clone([out] class System.Runtime.InteropServices.ComTypes.IStream/*0200008A*/& ppstm) cil managed
  // SIG: 20 01 01 10 12 82 28
  {
    // Method begins at RVA 0x0
  } // end of method IStream::Clone

  .method /*0600021F*/ public hidebysig newslot abstract virtual 
          instance void  Commit(int32 grfCommitFlags) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method IStream::Commit

  .method /*06000220*/ public hidebysig newslot abstract virtual 
          instance void  CopyTo(class System.Runtime.InteropServices.ComTypes.IStream/*0200008A*/ pstm,
                                int64 cb,
                                native int pcbRead,
                                native int pcbWritten) cil managed
  // SIG: 20 04 01 12 82 28 0A 18 18
  {
    // Method begins at RVA 0x0
  } // end of method IStream::CopyTo

  .method /*06000221*/ public hidebysig newslot abstract virtual 
          instance void  LockRegion(int64 libOffset,
                                    int64 cb,
                                    int32 dwLockType) cil managed
  // SIG: 20 03 01 0A 0A 08
  {
    // Method begins at RVA 0x0
  } // end of method IStream::LockRegion

  .method /*06000222*/ public hidebysig newslot abstract virtual 
          instance void  Read(uint8[] pv,
                              int32 cb,
                              native int pcbRead) cil managed
  // SIG: 20 03 01 1D 05 08 18
  {
    // Method begins at RVA 0x0
  } // end of method IStream::Read

  .method /*06000223*/ public hidebysig newslot abstract virtual 
          instance void  Revert() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IStream::Revert

  .method /*06000224*/ public hidebysig newslot abstract virtual 
          instance void  Seek(int64 dlibMove,
                              int32 dwOrigin,
                              native int plibNewPosition) cil managed
  // SIG: 20 03 01 0A 08 18
  {
    // Method begins at RVA 0x0
  } // end of method IStream::Seek

  .method /*06000225*/ public hidebysig newslot abstract virtual 
          instance void  SetSize(int64 libNewSize) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x0
  } // end of method IStream::SetSize

  .method /*06000226*/ public hidebysig newslot abstract virtual 
          instance void  Stat([out] valuetype System.Runtime.InteropServices.ComTypes.STATSTG/*02000094*/& pstatstg,
                              int32 grfStatFlag) cil managed
  // SIG: 20 02 01 10 11 82 50 08
  {
    // Method begins at RVA 0x0
  } // end of method IStream::Stat

  .method /*06000227*/ public hidebysig newslot abstract virtual 
          instance void  UnlockRegion(int64 libOffset,
                                      int64 cb,
                                      int32 dwLockType) cil managed
  // SIG: 20 03 01 0A 0A 08
  {
    // Method begins at RVA 0x0
  } // end of method IStream::UnlockRegion

  .method /*06000228*/ public hidebysig newslot abstract virtual 
          instance void  Write(uint8[] pv,
                               int32 cb,
                               native int pcbWritten) cil managed
  // SIG: 20 03 01 1D 05 08 18
  {
    // Method begins at RVA 0x0
  } // end of method IStream::Write

} // end of class System.Runtime.InteropServices.ComTypes.IStream

.class /*0200008B*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.ITypeComp
{
  .custom /*0C000152:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000153:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000154:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000229*/ public hidebysig newslot abstract virtual 
          instance void  Bind(string szName,
                              int32 lHashVal,
                              int16 wFlags,
                              [out] class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/& ppTInfo,
                              [out] valuetype System.Runtime.InteropServices.ComTypes.DESCKIND/*0200006D*/& pDescKind,
                              [out] valuetype System.Runtime.InteropServices.ComTypes.BINDPTR/*02000068*/& pBindPtr) cil managed
  // SIG: 20 06 01 0E 08 06 10 12 82 30 10 11 81 B4 10 11 81 A0
  {
    // Method begins at RVA 0x0
  } // end of method ITypeComp::Bind

  .method /*0600022A*/ public hidebysig newslot abstract virtual 
          instance void  BindType(string szName,
                                  int32 lHashVal,
                                  [out] class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/& ppTInfo,
                                  [out] class System.Runtime.InteropServices.ComTypes.ITypeComp/*0200008B*/& ppTComp) cil managed
  // SIG: 20 04 01 0E 08 10 12 82 30 10 12 82 2C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeComp::BindType

} // end of class System.Runtime.InteropServices.ComTypes.ITypeComp

.class /*0200008C*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.ITypeInfo
{
  .custom /*0C000156:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000157:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000158:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600022B*/ public hidebysig newslot abstract virtual 
          instance void  AddressOfMember(int32 memid,
                                         valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ invKind,
                                         [out] native int& ppv) cil managed
  // SIG: 20 03 01 08 11 82 1C 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::AddressOfMember

  .method /*0600022C*/ public hidebysig newslot abstract virtual 
          instance void  CreateInstance(object pUnkOuter,
                                        valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& riid,
                                        [out] object& ppvObj) cil managed
  // SIG: 20 03 01 1C 10 11 80 D5 10 1C
  {
    .param [1]/*080002E0*/ 
    .custom /*0C000252:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::CreateInstance

  .method /*0600022D*/ public hidebysig newslot abstract virtual 
          instance void  GetContainingTypeLib([out] class System.Runtime.InteropServices.ComTypes.ITypeLib/*0200008E*/& ppTLB,
                                              [out] int32& pIndex) cil managed
  // SIG: 20 02 01 10 12 82 38 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetContainingTypeLib

  .method /*0600022E*/ public hidebysig newslot abstract virtual 
          instance void  GetDllEntry(int32 memid,
                                     valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ invKind,
                                     native int pBstrDllName,
                                     native int pBstrName,
                                     native int pwOrdinal) cil managed
  // SIG: 20 05 01 08 11 82 1C 18 18 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetDllEntry

  .method /*0600022F*/ public hidebysig newslot abstract virtual 
          instance void  GetDocumentation(int32 index,
                                          [out] string& strName,
                                          [out] string& strDocString,
                                          [out] int32& dwHelpContext,
                                          [out] string& strHelpFile) cil managed
  // SIG: 20 05 01 08 10 0E 10 0E 10 08 10 0E
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetDocumentation

  .method /*06000230*/ public hidebysig newslot abstract virtual 
          instance void  GetFuncDesc(int32 index,
                                     [out] native int& ppFuncDesc) cil managed
  // SIG: 20 02 01 08 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetFuncDesc

  .method /*06000231*/ public hidebysig newslot abstract virtual 
          instance void  GetIDsOfNames(string[] rgszNames,
                                       int32 cNames,
                                       int32[] pMemId) cil managed
  // SIG: 20 03 01 1D 0E 08 1D 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetIDsOfNames

  .method /*06000232*/ public hidebysig newslot abstract virtual 
          instance void  GetImplTypeFlags(int32 index,
                                          [out] valuetype System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS/*02000086*/& pImplTypeFlags) cil managed
  // SIG: 20 02 01 08 10 11 82 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetImplTypeFlags

  .method /*06000233*/ public hidebysig newslot abstract virtual 
          instance void  GetMops(int32 memid,
                                 [out] string& pBstrMops) cil managed
  // SIG: 20 02 01 08 10 0E
  {
    .custom /*0C000239:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetMops

  .method /*06000234*/ public hidebysig newslot abstract virtual 
          instance void  GetNames(int32 memid,
                                  string[] rgBstrNames,
                                  int32 cMaxNames,
                                  [out] int32& pcNames) cil managed
  // SIG: 20 04 01 08 1D 0E 08 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetNames

  .method /*06000235*/ public hidebysig newslot abstract virtual 
          instance void  GetRefTypeInfo(int32 hRef,
                                        [out] class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/& ppTI) cil managed
  // SIG: 20 02 01 08 10 12 82 30
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetRefTypeInfo

  .method /*06000236*/ public hidebysig newslot abstract virtual 
          instance void  GetRefTypeOfImplType(int32 index,
                                              [out] int32& href) cil managed
  // SIG: 20 02 01 08 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetRefTypeOfImplType

  .method /*06000237*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeAttr([out] native int& ppTypeAttr) cil managed
  // SIG: 20 01 01 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetTypeAttr

  .method /*06000238*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeComp([out] class System.Runtime.InteropServices.ComTypes.ITypeComp/*0200008B*/& ppTComp) cil managed
  // SIG: 20 01 01 10 12 82 2C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetTypeComp

  .method /*06000239*/ public hidebysig newslot abstract virtual 
          instance void  GetVarDesc(int32 index,
                                    [out] native int& ppVarDesc) cil managed
  // SIG: 20 02 01 08 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::GetVarDesc

  .method /*0600023A*/ public hidebysig newslot abstract virtual 
          instance void  Invoke(object pvInstance,
                                int32 memid,
                                int16 wFlags,
                                valuetype System.Runtime.InteropServices.ComTypes.DISPPARAMS/*0200006E*/& pDispParams,
                                native int pVarResult,
                                native int pExcepInfo,
                                [out] int32& puArgErr) cil managed
  // SIG: 20 07 01 1C 08 06 10 11 81 B8 18 18 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::Invoke

  .method /*0600023B*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseFuncDesc(native int pFuncDesc) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::ReleaseFuncDesc

  .method /*0600023C*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseTypeAttr(native int pTypeAttr) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::ReleaseTypeAttr

  .method /*0600023D*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseVarDesc(native int pVarDesc) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo::ReleaseVarDesc

} // end of class System.Runtime.InteropServices.ComTypes.ITypeInfo

.class /*0200008D*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.ITypeInfo2
       implements System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/
{
  .custom /*0C00015A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00015B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00015C:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .interfaceimpl type System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/
  .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600023E*/ public hidebysig newslot abstract virtual 
          instance void  AddressOfMember(int32 memid,
                                         valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ invKind,
                                         [out] native int& ppv) cil managed
  // SIG: 20 03 01 08 11 82 1C 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::AddressOfMember

  .method /*0600023F*/ public hidebysig newslot abstract virtual 
          instance void  CreateInstance(object pUnkOuter,
                                        valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& riid,
                                        [out] object& ppvObj) cil managed
  // SIG: 20 03 01 1C 10 11 80 D5 10 1C
  {
    .param [1]/*08000311*/ 
    .custom /*0C000253:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::CreateInstance

  .method /*06000240*/ public hidebysig newslot abstract virtual 
          instance void  GetAllCustData(native int pCustData) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetAllCustData

  .method /*06000241*/ public hidebysig newslot abstract virtual 
          instance void  GetAllFuncCustData(int32 index,
                                            native int pCustData) cil managed
  // SIG: 20 02 01 08 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetAllFuncCustData

  .method /*06000242*/ public hidebysig newslot abstract virtual 
          instance void  GetAllImplTypeCustData(int32 index,
                                                native int pCustData) cil managed
  // SIG: 20 02 01 08 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetAllImplTypeCustData

  .method /*06000243*/ public hidebysig newslot abstract virtual 
          instance void  GetAllParamCustData(int32 indexFunc,
                                             int32 indexParam,
                                             native int pCustData) cil managed
  // SIG: 20 03 01 08 08 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetAllParamCustData

  .method /*06000244*/ public hidebysig newslot abstract virtual 
          instance void  GetAllVarCustData(int32 index,
                                           native int pCustData) cil managed
  // SIG: 20 02 01 08 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetAllVarCustData

  .method /*06000245*/ public hidebysig newslot abstract virtual 
          instance void  GetContainingTypeLib([out] class System.Runtime.InteropServices.ComTypes.ITypeLib/*0200008E*/& ppTLB,
                                              [out] int32& pIndex) cil managed
  // SIG: 20 02 01 10 12 82 38 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetContainingTypeLib

  .method /*06000246*/ public hidebysig newslot abstract virtual 
          instance void  GetCustData(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& guid,
                                     [out] object& pVarVal) cil managed
  // SIG: 20 02 01 10 11 80 D5 10 1C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetCustData

  .method /*06000247*/ public hidebysig newslot abstract virtual 
          instance void  GetDllEntry(int32 memid,
                                     valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ invKind,
                                     native int pBstrDllName,
                                     native int pBstrName,
                                     native int pwOrdinal) cil managed
  // SIG: 20 05 01 08 11 82 1C 18 18 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetDllEntry

  .method /*06000248*/ public hidebysig newslot abstract virtual 
          instance void  GetDocumentation(int32 index,
                                          [out] string& strName,
                                          [out] string& strDocString,
                                          [out] int32& dwHelpContext,
                                          [out] string& strHelpFile) cil managed
  // SIG: 20 05 01 08 10 0E 10 0E 10 08 10 0E
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetDocumentation

  .method /*06000249*/ public hidebysig newslot abstract virtual 
          instance void  GetDocumentation2(int32 memid,
                                           [out] string& pbstrHelpString,
                                           [out] int32& pdwHelpStringContext,
                                           [out] string& pbstrHelpStringDll) cil managed
  // SIG: 20 04 01 08 10 0E 10 08 10 0E
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetDocumentation2

  .method /*0600024A*/ public hidebysig newslot abstract virtual 
          instance void  GetFuncCustData(int32 index,
                                         valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& guid,
                                         [out] object& pVarVal) cil managed
  // SIG: 20 03 01 08 10 11 80 D5 10 1C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetFuncCustData

  .method /*0600024B*/ public hidebysig newslot abstract virtual 
          instance void  GetFuncDesc(int32 index,
                                     [out] native int& ppFuncDesc) cil managed
  // SIG: 20 02 01 08 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetFuncDesc

  .method /*0600024C*/ public hidebysig newslot abstract virtual 
          instance void  GetFuncIndexOfMemId(int32 memid,
                                             valuetype System.Runtime.InteropServices.ComTypes.INVOKEKIND/*02000087*/ invKind,
                                             [out] int32& pFuncIndex) cil managed
  // SIG: 20 03 01 08 11 82 1C 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetFuncIndexOfMemId

  .method /*0600024D*/ public hidebysig newslot abstract virtual 
          instance void  GetIDsOfNames(string[] rgszNames,
                                       int32 cNames,
                                       int32[] pMemId) cil managed
  // SIG: 20 03 01 1D 0E 08 1D 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetIDsOfNames

  .method /*0600024E*/ public hidebysig newslot abstract virtual 
          instance void  GetImplTypeCustData(int32 index,
                                             valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& guid,
                                             [out] object& pVarVal) cil managed
  // SIG: 20 03 01 08 10 11 80 D5 10 1C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetImplTypeCustData

  .method /*0600024F*/ public hidebysig newslot abstract virtual 
          instance void  GetImplTypeFlags(int32 index,
                                          [out] valuetype System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS/*02000086*/& pImplTypeFlags) cil managed
  // SIG: 20 02 01 08 10 11 82 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetImplTypeFlags

  .method /*06000250*/ public hidebysig newslot abstract virtual 
          instance void  GetMops(int32 memid,
                                 [out] string& pBstrMops) cil managed
  // SIG: 20 02 01 08 10 0E
  {
    .custom /*0C00023B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetMops

  .method /*06000251*/ public hidebysig newslot abstract virtual 
          instance void  GetNames(int32 memid,
                                  string[] rgBstrNames,
                                  int32 cMaxNames,
                                  [out] int32& pcNames) cil managed
  // SIG: 20 04 01 08 1D 0E 08 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetNames

  .method /*06000252*/ public hidebysig newslot abstract virtual 
          instance void  GetParamCustData(int32 indexFunc,
                                          int32 indexParam,
                                          valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& guid,
                                          [out] object& pVarVal) cil managed
  // SIG: 20 04 01 08 08 10 11 80 D5 10 1C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetParamCustData

  .method /*06000253*/ public hidebysig newslot abstract virtual 
          instance void  GetRefTypeInfo(int32 hRef,
                                        [out] class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/& ppTI) cil managed
  // SIG: 20 02 01 08 10 12 82 30
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetRefTypeInfo

  .method /*06000254*/ public hidebysig newslot abstract virtual 
          instance void  GetRefTypeOfImplType(int32 index,
                                              [out] int32& href) cil managed
  // SIG: 20 02 01 08 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetRefTypeOfImplType

  .method /*06000255*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeAttr([out] native int& ppTypeAttr) cil managed
  // SIG: 20 01 01 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetTypeAttr

  .method /*06000256*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeComp([out] class System.Runtime.InteropServices.ComTypes.ITypeComp/*0200008B*/& ppTComp) cil managed
  // SIG: 20 01 01 10 12 82 2C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetTypeComp

  .method /*06000257*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeFlags([out] int32& pTypeFlags) cil managed
  // SIG: 20 01 01 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetTypeFlags

  .method /*06000258*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeKind([out] valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/& pTypeKind) cil managed
  // SIG: 20 01 01 10 11 82 6C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetTypeKind

  .method /*06000259*/ public hidebysig newslot abstract virtual 
          instance void  GetVarCustData(int32 index,
                                        valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& guid,
                                        [out] object& pVarVal) cil managed
  // SIG: 20 03 01 08 10 11 80 D5 10 1C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetVarCustData

  .method /*0600025A*/ public hidebysig newslot abstract virtual 
          instance void  GetVarDesc(int32 index,
                                    [out] native int& ppVarDesc) cil managed
  // SIG: 20 02 01 08 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetVarDesc

  .method /*0600025B*/ public hidebysig newslot abstract virtual 
          instance void  GetVarIndexOfMemId(int32 memid,
                                            [out] int32& pVarIndex) cil managed
  // SIG: 20 02 01 08 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::GetVarIndexOfMemId

  .method /*0600025C*/ public hidebysig newslot abstract virtual 
          instance void  Invoke(object pvInstance,
                                int32 memid,
                                int16 wFlags,
                                valuetype System.Runtime.InteropServices.ComTypes.DISPPARAMS/*0200006E*/& pDispParams,
                                native int pVarResult,
                                native int pExcepInfo,
                                [out] int32& puArgErr) cil managed
  // SIG: 20 07 01 1C 08 06 10 11 81 B8 18 18 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::Invoke

  .method /*0600025D*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseFuncDesc(native int pFuncDesc) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::ReleaseFuncDesc

  .method /*0600025E*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseTypeAttr(native int pTypeAttr) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::ReleaseTypeAttr

  .method /*0600025F*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseVarDesc(native int pVarDesc) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeInfo2::ReleaseVarDesc

} // end of class System.Runtime.InteropServices.ComTypes.ITypeInfo2

.class /*0200008E*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.ITypeLib
{
  .custom /*0C00015E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00015F:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000160:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000260*/ public hidebysig newslot abstract virtual 
          instance void  FindName(string szNameBuf,
                                  int32 lHashVal,
                                  class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/[] ppTInfo,
                                  int32[] rgMemId,
                                  int16& pcFound) cil managed
  // SIG: 20 05 01 0E 08 1D 12 82 30 1D 08 10 06
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::FindName

  .method /*06000261*/ public hidebysig newslot abstract virtual 
          instance void  GetDocumentation(int32 index,
                                          [out] string& strName,
                                          [out] string& strDocString,
                                          [out] int32& dwHelpContext,
                                          [out] string& strHelpFile) cil managed
  // SIG: 20 05 01 08 10 0E 10 0E 10 08 10 0E
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::GetDocumentation

  .method /*06000262*/ public hidebysig newslot abstract virtual 
          instance void  GetLibAttr([out] native int& ppTLibAttr) cil managed
  // SIG: 20 01 01 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::GetLibAttr

  .method /*06000263*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeComp([out] class System.Runtime.InteropServices.ComTypes.ITypeComp/*0200008B*/& ppTComp) cil managed
  // SIG: 20 01 01 10 12 82 2C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::GetTypeComp

  .method /*06000264*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeInfo(int32 index,
                                     [out] class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/& ppTI) cil managed
  // SIG: 20 02 01 08 10 12 82 30
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::GetTypeInfo

  .method /*06000265*/ public hidebysig newslot abstract virtual 
          instance int32  GetTypeInfoCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::GetTypeInfoCount

  .method /*06000266*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeInfoOfGuid(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& guid,
                                           [out] class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/& ppTInfo) cil managed
  // SIG: 20 02 01 10 11 80 D5 10 12 82 30
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::GetTypeInfoOfGuid

  .method /*06000267*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeInfoType(int32 index,
                                         [out] valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/& pTKind) cil managed
  // SIG: 20 02 01 08 10 11 82 6C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::GetTypeInfoType

  .method /*06000268*/ public hidebysig newslot abstract virtual 
          instance bool  IsName(string szNameBuf,
                                int32 lHashVal) cil managed
  // SIG: 20 02 02 0E 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::IsName

  .method /*06000269*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseTLibAttr(native int pTLibAttr) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib::ReleaseTLibAttr

} // end of class System.Runtime.InteropServices.ComTypes.ITypeLib

.class /*0200008F*/ interface public abstract auto ansi System.Runtime.InteropServices.ComTypes.ITypeLib2
       implements System.Runtime.InteropServices.ComTypes.ITypeLib/*0200008E*/
{
  .custom /*0C000163:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000164:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000165:060000A8*/ instance void System.Runtime.InteropServices.InterfaceTypeAttribute/*0200003B*/::.ctor(valuetype System.Runtime.InteropServices.ComInterfaceType/*0200001F*/) /* 060000A8 */ = ( 01 00 01 00 00 00 00 00 ) 
  .interfaceimpl type System.Runtime.InteropServices.ComTypes.ITypeLib/*0200008E*/
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600026A*/ public hidebysig newslot abstract virtual 
          instance void  FindName(string szNameBuf,
                                  int32 lHashVal,
                                  class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/[] ppTInfo,
                                  int32[] rgMemId,
                                  int16& pcFound) cil managed
  // SIG: 20 05 01 0E 08 1D 12 82 30 1D 08 10 06
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::FindName

  .method /*0600026B*/ public hidebysig newslot abstract virtual 
          instance void  GetAllCustData(native int pCustData) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetAllCustData

  .method /*0600026C*/ public hidebysig newslot abstract virtual 
          instance void  GetCustData(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& guid,
                                     [out] object& pVarVal) cil managed
  // SIG: 20 02 01 10 11 80 D5 10 1C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetCustData

  .method /*0600026D*/ public hidebysig newslot abstract virtual 
          instance void  GetDocumentation(int32 index,
                                          [out] string& strName,
                                          [out] string& strDocString,
                                          [out] int32& dwHelpContext,
                                          [out] string& strHelpFile) cil managed
  // SIG: 20 05 01 08 10 0E 10 0E 10 08 10 0E
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetDocumentation

  .method /*0600026E*/ public hidebysig newslot abstract virtual 
          instance void  GetDocumentation2(int32 index,
                                           [out] string& pbstrHelpString,
                                           [out] int32& pdwHelpStringContext,
                                           [out] string& pbstrHelpStringDll) cil managed
  // SIG: 20 04 01 08 10 0E 10 08 10 0E
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetDocumentation2

  .method /*0600026F*/ public hidebysig newslot abstract virtual 
          instance void  GetLibAttr([out] native int& ppTLibAttr) cil managed
  // SIG: 20 01 01 10 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetLibAttr

  .method /*06000270*/ public hidebysig newslot abstract virtual 
          instance void  GetLibStatistics(native int pcUniqueNames,
                                          [out] int32& pcchUniqueNames) cil managed
  // SIG: 20 02 01 18 10 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetLibStatistics

  .method /*06000271*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeComp([out] class System.Runtime.InteropServices.ComTypes.ITypeComp/*0200008B*/& ppTComp) cil managed
  // SIG: 20 01 01 10 12 82 2C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetTypeComp

  .method /*06000272*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeInfo(int32 index,
                                     [out] class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/& ppTI) cil managed
  // SIG: 20 02 01 08 10 12 82 30
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetTypeInfo

  .method /*06000273*/ public hidebysig newslot abstract virtual 
          instance int32  GetTypeInfoCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetTypeInfoCount

  .method /*06000274*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeInfoOfGuid(valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/& guid,
                                           [out] class System.Runtime.InteropServices.ComTypes.ITypeInfo/*0200008C*/& ppTInfo) cil managed
  // SIG: 20 02 01 10 11 80 D5 10 12 82 30
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetTypeInfoOfGuid

  .method /*06000275*/ public hidebysig newslot abstract virtual 
          instance void  GetTypeInfoType(int32 index,
                                         [out] valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/& pTKind) cil managed
  // SIG: 20 02 01 08 10 11 82 6C
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::GetTypeInfoType

  .method /*06000276*/ public hidebysig newslot abstract virtual 
          instance bool  IsName(string szNameBuf,
                                int32 lHashVal) cil managed
  // SIG: 20 02 02 0E 08
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::IsName

  .method /*06000277*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseTLibAttr(native int pTLibAttr) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ITypeLib2::ReleaseTLibAttr

} // end of class System.Runtime.InteropServices.ComTypes.ITypeLib2

.class /*02000090*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.LIBFLAGS
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000166:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000167:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*0400014D*/ public specialname rtspecialname int16 value__
  .field /*0400014E*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.LIBFLAGS/*02000090*/ LIBFLAG_FRESTRICTED = int16(0x0001)
  .field /*0400014F*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.LIBFLAGS/*02000090*/ LIBFLAG_FCONTROL = int16(0x0002)
  .field /*04000150*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.LIBFLAGS/*02000090*/ LIBFLAG_FHIDDEN = int16(0x0004)
  .field /*04000151*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.LIBFLAGS/*02000090*/ LIBFLAG_FHASDISKIMAGE = int16(0x0008)
} // end of class System.Runtime.InteropServices.ComTypes.LIBFLAGS

.class /*02000091*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.PARAMDESC
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000168:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000152*/ public native int lpVarValue
  .field /*04000153*/ public valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ wParamFlags
} // end of class System.Runtime.InteropServices.ComTypes.PARAMDESC

.class /*02000092*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.PARAMFLAG
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000169:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00016A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000154*/ public specialname rtspecialname int16 value__
  .field /*04000155*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ PARAMFLAG_NONE = int16(0x0000)
  .field /*04000156*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ PARAMFLAG_FIN = int16(0x0001)
  .field /*04000157*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ PARAMFLAG_FOUT = int16(0x0002)
  .field /*04000158*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ PARAMFLAG_FLCID = int16(0x0004)
  .field /*04000159*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ PARAMFLAG_FRETVAL = int16(0x0008)
  .field /*0400015A*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ PARAMFLAG_FOPT = int16(0x0010)
  .field /*0400015B*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ PARAMFLAG_FHASDEFAULT = int16(0x0020)
  .field /*0400015C*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.PARAMFLAG/*02000092*/ PARAMFLAG_FHASCUSTDATA = int16(0x0040)
} // end of class System.Runtime.InteropServices.ComTypes.PARAMFLAG

.class /*02000093*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.STATDATA
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C00016D:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400015D*/ public valuetype System.Runtime.InteropServices.ComTypes.ADVF/*02000067*/ advf
  .field /*0400015E*/ public class System.Runtime.InteropServices.ComTypes.IAdviseSink/*02000077*/ advSink
  .custom /*0C000203:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .field /*0400015F*/ public int32 connection
  .field /*04000160*/ public valuetype System.Runtime.InteropServices.ComTypes.FORMATETC/*02000073*/ formatetc
} // end of class System.Runtime.InteropServices.ComTypes.STATDATA

.class /*02000094*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.STATSTG
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000170:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000161*/ public valuetype System.Runtime.InteropServices.ComTypes.FILETIME/*02000072*/ atime
  .field /*04000162*/ public int64 cbSize
  .field /*04000163*/ public valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 'clsid'
  .field /*04000164*/ public valuetype System.Runtime.InteropServices.ComTypes.FILETIME/*02000072*/ ctime
  .field /*04000165*/ public int32 grfLocksSupported
  .field /*04000166*/ public int32 grfMode
  .field /*04000167*/ public int32 grfStateBits
  .field /*04000168*/ public valuetype System.Runtime.InteropServices.ComTypes.FILETIME/*02000072*/ mtime
  .field /*04000169*/ public string pwcsName
  .custom /*0C000209:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .field /*0400016A*/ public int32 reserved
  .field /*0400016B*/ public int32 'type'
} // end of class System.Runtime.InteropServices.ComTypes.STATSTG

.class /*02000095*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.STGMEDIUM
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000173:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400016C*/ public object pUnkForRelease
  .custom /*0C00020E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .field /*0400016D*/ public valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ tymed
  .field /*0400016E*/ public native int unionmember
} // end of class System.Runtime.InteropServices.ComTypes.STGMEDIUM

.class /*02000096*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.SYSKIND
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000175:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400016F*/ public specialname rtspecialname int32 value__
  .field /*04000170*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.SYSKIND/*02000096*/ SYS_WIN16 = int32(0x00000000)
  .field /*04000171*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.SYSKIND/*02000096*/ SYS_WIN32 = int32(0x00000001)
  .field /*04000172*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.SYSKIND/*02000096*/ SYS_MAC = int32(0x00000002)
  .field /*04000173*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.SYSKIND/*02000096*/ SYS_WIN64 = int32(0x00000003)
} // end of class System.Runtime.InteropServices.ComTypes.SYSKIND

.class /*02000097*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.TYMED
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000176:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000177:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000174*/ public specialname rtspecialname int32 value__
  .field /*04000175*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ TYMED_NULL = int32(0x00000000)
  .field /*04000176*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ TYMED_HGLOBAL = int32(0x00000001)
  .field /*04000177*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ TYMED_FILE = int32(0x00000002)
  .field /*04000178*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ TYMED_ISTREAM = int32(0x00000004)
  .field /*04000179*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ TYMED_ISTORAGE = int32(0x00000008)
  .field /*0400017A*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ TYMED_GDI = int32(0x00000010)
  .field /*0400017B*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ TYMED_MFPICT = int32(0x00000020)
  .field /*0400017C*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYMED/*02000097*/ TYMED_ENHMF = int32(0x00000040)
} // end of class System.Runtime.InteropServices.ComTypes.TYMED

.class /*02000098*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.TYPEATTR
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C00017A:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*0400017D*/ public int16 cbAlignment
  .field /*0400017E*/ public int32 cbSizeInstance
  .field /*0400017F*/ public int16 cbSizeVft
  .field /*04000180*/ public int16 cFuncs
  .field /*04000181*/ public int16 cImplTypes
  .field /*04000182*/ public int16 cVars
  .field /*04000183*/ public int32 dwReserved
  .field /*04000184*/ public valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ guid
  .field /*04000185*/ public valuetype System.Runtime.InteropServices.ComTypes.IDLDESC/*0200007C*/ idldescType
  .field /*04000186*/ public int32 lcid
  .field /*04000187*/ public native int lpstrSchema
  .field /*04000188*/ public static literal int32 MEMBER_ID_NIL = int32(0xFFFFFFFF)
  .field /*04000189*/ public int32 memidConstructor
  .field /*0400018A*/ public int32 memidDestructor
  .field /*0400018B*/ public valuetype System.Runtime.InteropServices.ComTypes.TYPEDESC/*02000099*/ tdescAlias
  .field /*0400018C*/ public valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ typekind
  .field /*0400018D*/ public int16 wMajorVerNum
  .field /*0400018E*/ public int16 wMinorVerNum
  .field /*0400018F*/ public valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ wTypeFlags
} // end of class System.Runtime.InteropServices.ComTypes.TYPEATTR

.class /*02000099*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.TYPEDESC
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C00017B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*04000190*/ public native int lpValue
  .field /*04000191*/ public int16 vt
} // end of class System.Runtime.InteropServices.ComTypes.TYPEDESC

.class /*0200009A*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.TYPEFLAGS
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C00017C:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00017D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000192*/ public specialname rtspecialname int16 value__
  .field /*04000193*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FAPPOBJECT = int16(0x0001)
  .field /*04000194*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FCANCREATE = int16(0x0002)
  .field /*04000195*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FLICENSED = int16(0x0004)
  .field /*04000196*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FPREDECLID = int16(0x0008)
  .field /*04000197*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FHIDDEN = int16(0x0010)
  .field /*04000198*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FCONTROL = int16(0x0020)
  .field /*04000199*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FDUAL = int16(0x0040)
  .field /*0400019A*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FNONEXTENSIBLE = int16(0x0080)
  .field /*0400019B*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FOLEAUTOMATION = int16(0x0100)
  .field /*0400019C*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FRESTRICTED = int16(0x0200)
  .field /*0400019D*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FAGGREGATABLE = int16(0x0400)
  .field /*0400019E*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FREPLACEABLE = int16(0x0800)
  .field /*0400019F*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FDISPATCHABLE = int16(0x1000)
  .field /*040001A0*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FREVERSEBIND = int16(0x2000)
  .field /*040001A1*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEFLAGS/*0200009A*/ TYPEFLAG_FPROXY = int16(0x4000)
} // end of class System.Runtime.InteropServices.ComTypes.TYPEFLAGS

.class /*0200009B*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.TYPEKIND
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C00017E:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040001A2*/ public specialname rtspecialname int32 value__
  .field /*040001A3*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_ENUM = int32(0x00000000)
  .field /*040001A4*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_RECORD = int32(0x00000001)
  .field /*040001A5*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_MODULE = int32(0x00000002)
  .field /*040001A6*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_INTERFACE = int32(0x00000003)
  .field /*040001A7*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_DISPATCH = int32(0x00000004)
  .field /*040001A8*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_COCLASS = int32(0x00000005)
  .field /*040001A9*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_ALIAS = int32(0x00000006)
  .field /*040001AA*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_UNION = int32(0x00000007)
  .field /*040001AB*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.TYPEKIND/*0200009B*/ TKIND_MAX = int32(0x00000008)
} // end of class System.Runtime.InteropServices.ComTypes.TYPEKIND

.class /*0200009C*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.TYPELIBATTR
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C00017F:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040001AC*/ public valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ guid
  .field /*040001AD*/ public int32 lcid
  .field /*040001AE*/ public valuetype System.Runtime.InteropServices.ComTypes.SYSKIND/*02000096*/ syskind
  .field /*040001AF*/ public valuetype System.Runtime.InteropServices.ComTypes.LIBFLAGS/*02000090*/ wLibFlags
  .field /*040001B0*/ public int16 wMajorVerNum
  .field /*040001B1*/ public int16 wMinorVerNum
} // end of class System.Runtime.InteropServices.ComTypes.TYPELIBATTR

.class /*0200009D*/ public sequential ansi sealed beforefieldinit System.Runtime.InteropServices.ComTypes.VARDESC
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C000180:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .class /*020000A8*/ explicit ansi sealed nested public beforefieldinit DESCUNION
         extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
  {
    .field /*040001D6*/ [0] public native int lpvarValue
    .field /*040001D7*/ [0] public int32 oInst
  } // end of class DESCUNION

  .field /*040001B2*/ public valuetype System.Runtime.InteropServices.ComTypes.VARDESC/*0200009D*//DESCUNION/*020000A8*/ desc
  .field /*040001B3*/ public valuetype System.Runtime.InteropServices.ComTypes.ELEMDESC/*02000070*/ elemdescVar
  .field /*040001B4*/ public string lpstrSchema
  .custom /*0C00021F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .field /*040001B5*/ public int32 memid
  .field /*040001B6*/ public valuetype System.Runtime.InteropServices.ComTypes.VARKIND/*0200009F*/ varkind
  .field /*040001B7*/ public int16 wVarFlags
} // end of class System.Runtime.InteropServices.ComTypes.VARDESC

.class /*0200009E*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.VARFLAGS
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000181:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000182:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003E*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*040001B8*/ public specialname rtspecialname int16 value__
  .field /*040001B9*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FREADONLY = int16(0x0001)
  .field /*040001BA*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FSOURCE = int16(0x0002)
  .field /*040001BB*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FBINDABLE = int16(0x0004)
  .field /*040001BC*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FREQUESTEDIT = int16(0x0008)
  .field /*040001BD*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FDISPLAYBIND = int16(0x0010)
  .field /*040001BE*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FDEFAULTBIND = int16(0x0020)
  .field /*040001BF*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FHIDDEN = int16(0x0040)
  .field /*040001C0*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FRESTRICTED = int16(0x0080)
  .field /*040001C1*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FDEFAULTCOLLELEM = int16(0x0100)
  .field /*040001C2*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FUIDEFAULT = int16(0x0200)
  .field /*040001C3*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FNONBROWSABLE = int16(0x0400)
  .field /*040001C4*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FREPLACEABLE = int16(0x0800)
  .field /*040001C5*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARFLAGS/*0200009E*/ VARFLAG_FIMMEDIATEBIND = int16(0x1000)
} // end of class System.Runtime.InteropServices.ComTypes.VARFLAGS

.class /*0200009F*/ public auto ansi sealed System.Runtime.InteropServices.ComTypes.VARKIND
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .custom /*0C000183:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000023*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
  .field /*040001C6*/ public specialname rtspecialname int32 value__
  .field /*040001C7*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARKIND/*0200009F*/ VAR_PERINSTANCE = int32(0x00000000)
  .field /*040001C8*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARKIND/*0200009F*/ VAR_STATIC = int32(0x00000001)
  .field /*040001C9*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARKIND/*0200009F*/ VAR_CONST = int32(0x00000002)
  .field /*040001CA*/ public static literal valuetype System.Runtime.InteropServices.ComTypes.VARKIND/*0200009F*/ VAR_DISPATCH = int32(0x00000003)
} // end of class System.Runtime.InteropServices.ComTypes.VARKIND

.class /*020000A0*/ public auto ansi sealed beforefieldinit System.Runtime.CompilerServices.IDispatchConstantAttribute
       extends [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CustomConstantAttribute/*0100004B*/
{
  .custom /*0C000184:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000185:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000186:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000030*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .custom /*0C000187:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 09 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*06000278*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27b0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CustomConstantAttribute/*0100004B*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method IDispatchConstantAttribute::.ctor

  .method /*06000279*/ public hidebysig specialname virtual 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x27b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IDispatchConstantAttribute::get_Value

  .property /*1700003F*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Runtime.CompilerServices.IDispatchConstantAttribute/*020000A0*/::get_Value() /* 06000279 */
  } // end of property IDispatchConstantAttribute::Value
} // end of class System.Runtime.CompilerServices.IDispatchConstantAttribute

.class /*020000A1*/ public auto ansi sealed beforefieldinit System.Runtime.CompilerServices.IUnknownConstantAttribute
       extends [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CustomConstantAttribute/*0100004B*/
{
  .custom /*0C000188:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000189:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00018A:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 00 09 00 00 01 00 54 02 09 49 6E 68 65 72   // ........T..Inher
                                                                                                                                                                                                                          69 74 65 64 00 )                                  // ited.
  .method /*0600027A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27bb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CustomConstantAttribute/*0100004B*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method IUnknownConstantAttribute::.ctor

  .method /*0600027B*/ public hidebysig specialname virtual 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x27c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IUnknownConstantAttribute::get_Value

  .property /*17000040*/ instance callconv(8) object
          Value()
  {
    .get instance object System.Runtime.CompilerServices.IUnknownConstantAttribute/*020000A1*/::get_Value() /* 0600027B */
  } // end of property IUnknownConstantAttribute::Value
} // end of class System.Runtime.CompilerServices.IUnknownConstantAttribute

.class /*020000A2*/ public auto ansi beforefieldinit System.IO.UnmanagedMemoryAccessor
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001E*/
{
  .method /*0600027C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27c6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::.ctor

  .method /*0600027D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.InteropServices.SafeBuffer/*0100004C*/ buffer,
                               int64 offset,
                               int64 capacity) cil managed
  // SIG: 20 03 01 12 81 31 0A 0A
  {
    .custom /*0C00023D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27ce
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::.ctor

  .method /*0600027E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.InteropServices.SafeBuffer/*0100004C*/ buffer,
                               int64 offset,
                               int64 capacity,
                               valuetype [System.Runtime/*23000001*/]System.IO.FileAccess/*0100004D*/ access) cil managed
  // SIG: 20 04 01 12 81 31 0A 0A 11 81 35
  {
    .custom /*0C00023F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27d6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::.ctor

  .method /*0600027F*/ public hidebysig specialname 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x27de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::get_CanRead

  .method /*06000280*/ public hidebysig specialname 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x27e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::get_CanWrite

  .method /*06000281*/ public hidebysig specialname 
          instance int64  get_Capacity() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x27e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::get_Capacity

  .method /*06000282*/ family hidebysig specialname 
          instance bool  get_IsOpen() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x27e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::get_IsOpen

  .method /*06000283*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Dispose

  .method /*06000284*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x27ec
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Dispose

  .method /*06000285*/ family hidebysig instance void 
          Initialize(class [System.Runtime/*23000001*/]System.Runtime.InteropServices.SafeBuffer/*0100004C*/ buffer,
                     int64 offset,
                     int64 capacity,
                     valuetype [System.Runtime/*23000001*/]System.IO.FileAccess/*0100004D*/ access) cil managed
  // SIG: 20 04 01 12 81 31 0A 0A 11 81 35
  {
    .custom /*0C000241:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Initialize

  .method /*06000286*/ public hidebysig instance int32 
          ReadArray<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000021*/) T>(int64 position,
                                                                                                  !!T[] 'array',
                                                                                                  int32 offset,
                                                                                                  int32 count) cil managed
  // SIG: 30 01 04 08 0A 1D 1E 00 08 08
  {
    .param [2]/*080003A3*/ 
    .custom /*0C000254:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    // Method begins at RVA 0x27f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadArray

  .method /*06000287*/ public hidebysig instance bool 
          ReadBoolean(int64 position) cil managed
  // SIG: 20 01 02 0A
  {
    // Method begins at RVA 0x27f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadBoolean

  .method /*06000288*/ public hidebysig instance uint8 
          ReadByte(int64 position) cil managed
  // SIG: 20 01 05 0A
  {
    // Method begins at RVA 0x27f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadByte

  .method /*06000289*/ public hidebysig instance char 
          ReadChar(int64 position) cil managed
  // SIG: 20 01 03 0A
  {
    // Method begins at RVA 0x27f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadChar

  .method /*0600028A*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Decimal/*01000037*/ 
          ReadDecimal(int64 position) cil managed
  // SIG: 20 01 11 80 DD 0A
  {
    // Method begins at RVA 0x27fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadDecimal

  .method /*0600028B*/ public hidebysig instance float64 
          ReadDouble(int64 position) cil managed
  // SIG: 20 01 0D 0A
  {
    // Method begins at RVA 0x27ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadDouble

  .method /*0600028C*/ public hidebysig instance int16 
          ReadInt16(int64 position) cil managed
  // SIG: 20 01 06 0A
  {
    // Method begins at RVA 0x2802
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadInt16

  .method /*0600028D*/ public hidebysig instance int32 
          ReadInt32(int64 position) cil managed
  // SIG: 20 01 08 0A
  {
    // Method begins at RVA 0x2805
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadInt32

  .method /*0600028E*/ public hidebysig instance int64 
          ReadInt64(int64 position) cil managed
  // SIG: 20 01 0A 0A
  {
    // Method begins at RVA 0x2808
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadInt64

  .method /*0600028F*/ public hidebysig instance int8 
          ReadSByte(int64 position) cil managed
  // SIG: 20 01 04 0A
  {
    .custom /*0C000245:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x280b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadSByte

  .method /*06000290*/ public hidebysig instance float32 
          ReadSingle(int64 position) cil managed
  // SIG: 20 01 0C 0A
  {
    // Method begins at RVA 0x280e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadSingle

  .method /*06000291*/ public hidebysig instance uint16 
          ReadUInt16(int64 position) cil managed
  // SIG: 20 01 07 0A
  {
    .custom /*0C000246:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2811
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadUInt16

  .method /*06000292*/ public hidebysig instance uint32 
          ReadUInt32(int64 position) cil managed
  // SIG: 20 01 09 0A
  {
    .custom /*0C000248:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2814
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadUInt32

  .method /*06000293*/ public hidebysig instance uint64 
          ReadUInt64(int64 position) cil managed
  // SIG: 20 01 0B 0A
  {
    .custom /*0C000249:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2817
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::ReadUInt64

  .method /*06000294*/ public hidebysig instance void 
          Read<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000021*/) T>(int64 position,
                                                                                             [out] !!T& structure) cil managed
  // SIG: 30 01 02 01 0A 10 1E 00
  {
    // Method begins at RVA 0x281a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnmanagedMemoryAccessor::Read

  .method /*06000295*/ public hidebysig instance void 
          Write(int64 position,
                bool 'value') cil managed
  // SIG: 20 02 01 0A 02
  {
    // Method begins at RVA 0x281d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*06000296*/ public hidebysig instance void 
          Write(int64 position,
                uint8 'value') cil managed
  // SIG: 20 02 01 0A 05
  {
    // Method begins at RVA 0x281f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*06000297*/ public hidebysig instance void 
          Write(int64 position,
                char 'value') cil managed
  // SIG: 20 02 01 0A 03
  {
    // Method begins at RVA 0x2821
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*06000298*/ public hidebysig instance void 
          Write(int64 position,
                valuetype [System.Runtime/*23000001*/]System.Decimal/*01000037*/ 'value') cil managed
  // SIG: 20 02 01 0A 11 80 DD
  {
    // Method begins at RVA 0x2823
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*06000299*/ public hidebysig instance void 
          Write(int64 position,
                float64 'value') cil managed
  // SIG: 20 02 01 0A 0D
  {
    // Method begins at RVA 0x2825
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*0600029A*/ public hidebysig instance void 
          Write(int64 position,
                int16 'value') cil managed
  // SIG: 20 02 01 0A 06
  {
    // Method begins at RVA 0x2827
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*0600029B*/ public hidebysig instance void 
          Write(int64 position,
                int32 'value') cil managed
  // SIG: 20 02 01 0A 08
  {
    // Method begins at RVA 0x2829
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*0600029C*/ public hidebysig instance void 
          Write(int64 position,
                int64 'value') cil managed
  // SIG: 20 02 01 0A 0A
  {
    // Method begins at RVA 0x282b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*0600029D*/ public hidebysig instance void 
          Write(int64 position,
                int8 'value') cil managed
  // SIG: 20 02 01 0A 04
  {
    .custom /*0C00024B:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x282d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*0600029E*/ public hidebysig instance void 
          Write(int64 position,
                float32 'value') cil managed
  // SIG: 20 02 01 0A 0C
  {
    // Method begins at RVA 0x282f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*0600029F*/ public hidebysig instance void 
          Write(int64 position,
                uint16 'value') cil managed
  // SIG: 20 02 01 0A 07
  {
    .custom /*0C00024C:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2831
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*060002A0*/ public hidebysig instance void 
          Write(int64 position,
                uint32 'value') cil managed
  // SIG: 20 02 01 0A 09
  {
    .custom /*0C00024D:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2833
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*060002A1*/ public hidebysig instance void 
          Write(int64 position,
                uint64 'value') cil managed
  // SIG: 20 02 01 0A 0B
  {
    .custom /*0C00024F:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2835
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .method /*060002A2*/ public hidebysig instance void 
          WriteArray<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000021*/) T>(int64 position,
                                                                                                   !!T[] 'array',
                                                                                                   int32 offset,
                                                                                                   int32 count) cil managed
  // SIG: 30 01 04 01 0A 1D 1E 00 08 08
  {
    .param [2]/*080003D0*/ 
    .custom /*0C000255:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    // Method begins at RVA 0x2837
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::WriteArray

  .method /*060002A3*/ public hidebysig instance void 
          Write<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000021*/) T>(int64 position,
                                                                                              !!T& structure) cil managed
  // SIG: 30 01 02 01 0A 10 1E 00
  {
    // Method begins at RVA 0x2839
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method UnmanagedMemoryAccessor::Write

  .property /*17000041*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.UnmanagedMemoryAccessor/*020000A2*/::get_CanRead() /* 0600027F */
  } // end of property UnmanagedMemoryAccessor::CanRead
  .property /*17000042*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.UnmanagedMemoryAccessor/*020000A2*/::get_CanWrite() /* 06000280 */
  } // end of property UnmanagedMemoryAccessor::CanWrite
  .property /*17000043*/ instance callconv(8) int64
          Capacity()
  {
    .get instance int64 System.IO.UnmanagedMemoryAccessor/*020000A2*/::get_Capacity() /* 06000281 */
  } // end of property UnmanagedMemoryAccessor::Capacity
  .property /*17000044*/ instance callconv(8) bool
          IsOpen()
  {
    .get instance bool System.IO.UnmanagedMemoryAccessor/*020000A2*/::get_IsOpen() /* 06000282 */
  } // end of property UnmanagedMemoryAccessor::IsOpen
} // end of class System.IO.UnmanagedMemoryAccessor


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
