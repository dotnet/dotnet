


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
// Time-date stamp:            0x9b219869
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000c600
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000e5e2
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
// Checksum:                       0x00014868
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000e58f [0x0000004f] address [size] of Import Directory:          
// 0x00010000 [0x00000494] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000d000 [0x00002878] address [size] of Security Directory:        
// 0x00012000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000e4a4 [0x00000054] address [size] of Debug Directory:           
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
//              0x0000c5e8 Virtual Size
//              0x00002000 Virtual Address
//              0x0000c600 Size of Raw Data
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
//              0x00000494 Virtual Size
//              0x00010000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000c800 Pointer to Raw Data
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
//              0x0000ce00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000e000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000005e4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000e5d1 Import Name Table
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
// 0x00002ba0 [0x0000b884] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000e424 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000073fc Size
//              '#~' Name
//    Stream 2:
//              0x00007468 Offset
//              0x00002a04 Size
//              '#Strings' Name
//    Stream 3:
//              0x00009e6c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00009e70 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00009e80 Offset
//              0x00001a04 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000014090bb41f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 63608
// PE header size       : 512 (496 used)    ( 0.80%)
// PE additional info   : 11787             (18.53%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.11%)
// CLR meta-data size  : 47236              (74.26%)
// CLR additional info : 128                ( 0.20%)
// CLR method headers  : 830                ( 1.30%)
// Managed code         : 2064              ( 3.24%)
// Data                 : 2048              ( 3.22%)
// Unaccounted          : -1069             (-1.68%)

// Num.of PE sections   : 3
//   .text    - 50688
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 47236
//   Module        -    1 (10 bytes)
//   TypeDef       -   83 (1162 bytes)    10 interfaces, 0 explicit layout
//   TypeRef       -   57 (342 bytes)
//   MethodDef     -  863 (12082 bytes)   33 abstract, 0 native, 830 bodies
//   FieldDef      -  102 (612 bytes)     2 constant
//   MemberRef     -   59 (354 bytes)
//   ParamDef      - 1214 (7284 bytes)
//   MethodImpl    -   48 (288 bytes)
//   Constant      -   92 (552 bytes)
//   CustomAttribute-  612 (3672 bytes)
//   InterfaceImpl -   32 (128 bytes)
//   PropertyMap   -   61 (244 bytes)
//   Property      -  213 (1278 bytes)
//   MethodSemantic-  220 (1320 bytes)
//   TypeSpec      -    7 (14 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    1 (6 bytes)
//   GenericParam  -   20 (160 bytes)
//   GenericParamConstraint-    2 (8 bytes)
//   Strings       - 10755 bytes
//   Blobs         -  6660 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   219 bytes

// CLR additional info : 128

// CLR method headers : 830
//   Num.of method bodies  - 830
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 830

// Managed code : 2064
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class CallSite                       (public) (auto) (ansi) 
// Class CallSiteBinder                 (public) (abstract) (auto) (ansi) 
// Class CallSiteHelpers                (public) (abstract) (auto) (ansi) (sealed) 
// Class CallSite`1                     (public) (auto) (ansi) 
// Class DebugInfoGenerator             (public) (abstract) (auto) (ansi) 
// Class DynamicAttribute               (public) (auto) (ansi) (sealed) 
// Interface IRuntimeVariables              (public) (abstract) (auto) (ansi) 
// Class ReadOnlyCollectionBuilder`1    (public) (auto) (ansi) (sealed) 
// Class RuleCache`1                    (public) (auto) (ansi) 
// Interface IOrderedQueryable              (public) (abstract) (auto) (ansi) 
// Interface IOrderedQueryable`1            (public) (abstract) (auto) (ansi) 
// Interface IQueryable                     (public) (abstract) (auto) (ansi) 
// Interface IQueryable`1                   (public) (abstract) (auto) (ansi) 
// Interface IQueryProvider                 (public) (abstract) (auto) (ansi) 
// Class BinaryExpression               (public) (auto) (ansi) 
// Class BlockExpression                (public) (auto) (ansi) 
// Class CatchBlock                     (public) (auto) (ansi) (sealed) 
// Class ConditionalExpression          (public) (auto) (ansi) 
// Class ConstantExpression             (public) (auto) (ansi) 
// Class DebugInfoExpression            (public) (auto) (ansi) 
// Class DefaultExpression              (public) (auto) (ansi) (sealed) 
// Class DynamicExpression              (public) (auto) (ansi) 
// Class DynamicExpressionVisitor       (public) (abstract) (auto) (ansi) 
// Class ElementInit                    (public) (auto) (ansi) (sealed) 
// Class Expression                     (public) (abstract) (auto) (ansi) 
// Class ExpressionType                 (public) (auto) (ansi) (sealed) 
// Class ExpressionVisitor              (public) (abstract) (auto) (ansi) 
// Class Expression`1                   (public) (auto) (ansi) (sealed) 
// Class GotoExpression                 (public) (auto) (ansi) (sealed) 
// Class GotoExpressionKind             (public) (auto) (ansi) (sealed) 
// Interface IArgumentProvider              (public) (abstract) (auto) (ansi) 
// Interface IDynamicExpression             (public) (abstract) (auto) (ansi) 
// Class IndexExpression                (public) (auto) (ansi) (sealed) 
// Class InvocationExpression           (public) (auto) (ansi) (sealed) 
// Class LabelExpression                (public) (auto) (ansi) (sealed) 
// Class LabelTarget                    (public) (auto) (ansi) (sealed) 
// Class LambdaExpression               (public) (abstract) (auto) (ansi) 
// Class ListInitExpression             (public) (auto) (ansi) (sealed) 
// Class LoopExpression                 (public) (auto) (ansi) (sealed) 
// Class MemberAssignment               (public) (auto) (ansi) (sealed) 
// Class MemberBinding                  (public) (abstract) (auto) (ansi) 
// Class MemberBindingType              (public) (auto) (ansi) (sealed) 
// Class MemberExpression               (public) (auto) (ansi) 
// Class MemberInitExpression           (public) (auto) (ansi) (sealed) 
// Class MemberListBinding              (public) (auto) (ansi) (sealed) 
// Class MemberMemberBinding            (public) (auto) (ansi) (sealed) 
// Class MethodCallExpression           (public) (auto) (ansi) 
// Class NewArrayExpression             (public) (auto) (ansi) 
// Class NewExpression                  (public) (auto) (ansi) 
// Class ParameterExpression            (public) (auto) (ansi) 
// Class RuntimeVariablesExpression     (public) (auto) (ansi) (sealed) 
// Class SwitchCase                     (public) (auto) (ansi) (sealed) 
// Class SwitchExpression               (public) (auto) (ansi) (sealed) 
// Class SymbolDocumentInfo             (public) (auto) (ansi) 
// Class TryExpression                  (public) (auto) (ansi) (sealed) 
// Class TypeBinaryExpression           (public) (auto) (ansi) (sealed) 
// Class UnaryExpression                (public) (auto) (ansi) (sealed) 
// Class BinaryOperationBinder          (public) (abstract) (auto) (ansi) 
// Class BindingRestrictions            (public) (abstract) (auto) (ansi) 
// Class CallInfo                       (public) (auto) (ansi) (sealed) 
// Class ConvertBinder                  (public) (abstract) (auto) (ansi) 
// Class CreateInstanceBinder           (public) (abstract) (auto) (ansi) 
// Class DeleteIndexBinder              (public) (abstract) (auto) (ansi) 
// Class DeleteMemberBinder             (public) (abstract) (auto) (ansi) 
// Class DynamicMetaObject              (public) (auto) (ansi) 
// Class DynamicMetaObjectBinder        (public) (abstract) (auto) (ansi) 
// Class DynamicObject                  (public) (auto) (ansi) 
// Class ExpandoObject                  (public) (auto) (ansi) (sealed) 
// Class GetIndexBinder                 (public) (abstract) (auto) (ansi) 
// Class GetMemberBinder                (public) (abstract) (auto) (ansi) 
// Interface IDynamicMetaObjectProvider     (public) (abstract) (auto) (ansi) 
// Interface IInvokeOnGetBinder             (public) (abstract) (auto) (ansi) 
// Class InvokeBinder                   (public) (abstract) (auto) (ansi) 
// Class InvokeMemberBinder             (public) (abstract) (auto) (ansi) 
// Class SetIndexBinder                 (public) (abstract) (auto) (ansi) 
// Class SetMemberBinder                (public) (abstract) (auto) (ansi) 
// Class UnaryOperationBinder           (public) (abstract) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.ObjectModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Linq.Expressions
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 17 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 45   // ...System.Linq.E
                                                                                                                                                                        78 70 72 65 73 73 69 6F 6E 73 00 00 )             // xpressions..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 17 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 45   // ...System.Linq.E
                                                                                                                                                                       78 70 72 65 73 73 69 6F 6E 73 00 00 )             // xpressions..
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 17 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 45   // ...System.Linq.E
                                                                                                                                                                 78 70 72 65 73 73 69 6F 6E 73 00 00 )             // xpressions..
  .custom /*0C000016:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
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
  .ver 7:0:0:0
}
.module System.Linq.Expressions.dll
// MVID: {941f1ccf-11de-47ca-a92e-5d8224972745}
.custom /*0C000002:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003A       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00003A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001B:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)00003A       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00003A */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000039       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000039*/
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
    IL_0001:  /* 28   | (0A)00003A       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00003A */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000020:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)00003A       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00003A */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000024:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00003A       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00003A */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000029:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00002A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002B:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000004*/ public initonly int32 Version
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 A_1) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x209d
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003A       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00003A */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public auto ansi beforefieldinit System.Runtime.CompilerServices.CallSite
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CallSite::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ 
          get_Binder() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallSite::get_Binder

  .method /*06000009*/ public hidebysig static 
          class System.Runtime.CompilerServices.CallSite/*02000007*/ 
          Create(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                 class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder) cil managed
  // SIG: 00 02 12 1C 12 59 12 20
  {
    .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallSite::Create

  .property /*17000001*/ instance callconv(8) class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/
          Binder()
  {
    .get instance class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ System.Runtime.CompilerServices.CallSite/*02000007*/::get_Binder() /* 06000008 */
  } // end of property CallSite::Binder
} // end of class System.Runtime.CompilerServices.CallSite

.class /*02000008*/ public abstract auto ansi beforefieldinit System.Runtime.CompilerServices.CallSiteBinder
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600000A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ba
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CallSiteBinder::.ctor

  .method /*0600000B*/ public hidebysig specialname static 
          class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          get_UpdateLabel() cil managed
  // SIG: 00 00 12 80 A8
  {
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallSiteBinder::get_UpdateLabel

  .method /*0600000C*/ public hidebysig newslot abstract virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Bind(object[] args,
               class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters,
               class System.Linq.Expressions.LabelTarget/*0200002A*/ returnLabel) cil managed
  // SIG: 20 03 12 7C 1D 1C 15 12 5D 01 12 80 E0 12 80 A8
  {
    // Method begins at RVA 0x0
  } // end of method CallSiteBinder::Bind

  .method /*0600000D*/ public hidebysig newslot virtual 
          instance !!T  BindDelegate<class T>(class System.Runtime.CompilerServices.CallSite`1/*0200000A*/<!!T> site,
                                              object[] args) cil managed
  // SIG: 30 01 02 1E 00 15 12 28 01 1E 00 1D 1C
  {
    .param [0]/*08000006*/ 
    .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallSiteBinder::BindDelegate

  .method /*0600000E*/ family hidebysig instance void 
          CacheTarget<class T>(!!T target) cil managed
  // SIG: 30 01 01 01 1E 00
  {
    // Method begins at RVA 0x20c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CallSiteBinder::CacheTarget

  .property /*17000002*/ callconv(8) class System.Linq.Expressions.LabelTarget/*0200002A*/
          UpdateLabel()
  {
    .get class System.Linq.Expressions.LabelTarget/*0200002A*/ System.Runtime.CompilerServices.CallSiteBinder/*02000008*/::get_UpdateLabel() /* 0600000B */
  } // end of property CallSiteBinder::UpdateLabel
} // end of class System.Runtime.CompilerServices.CallSiteBinder

.class /*02000009*/ public abstract auto ansi sealed beforefieldinit System.Runtime.CompilerServices.CallSiteHelpers
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .method /*0600000F*/ public hidebysig static 
          bool  IsInternalFrame(class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000018*/ mb) cil managed
  // SIG: 00 01 02 12 61
  {
    .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallSiteHelpers::IsInternalFrame

} // end of class System.Runtime.CompilerServices.CallSiteHelpers

.class /*0200000A*/ public auto ansi beforefieldinit System.Runtime.CompilerServices.CallSite`1<class T>
       extends System.Runtime.CompilerServices.CallSite/*02000007*/
{
  .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000005*/ public !T Target
  .method /*06000010*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20cd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000007       */ call       instance void System.Runtime.CompilerServices.CallSite/*02000007*/::.ctor() /* 06000007 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CallSite`1::.ctor

  .method /*06000011*/ public hidebysig specialname 
          instance !T  get_Update() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallSite`1::get_Update

  .method /*06000012*/ public hidebysig static 
          class System.Runtime.CompilerServices.CallSite`1/*0200000A*/<!T> 
          Create(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder) cil managed
  // SIG: 00 01 15 12 28 01 13 00 12 20
  {
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallSite`1::Create

  .property /*17000003*/ instance callconv(8) !T
          Update()
  {
    .get instance !T System.Runtime.CompilerServices.CallSite`1/*0200000A*/::get_Update() /* 06000011 */
  } // end of property CallSite`1::Update
} // end of class System.Runtime.CompilerServices.CallSite`1

.class /*0200000B*/ public abstract auto ansi beforefieldinit System.Runtime.CompilerServices.DebugInfoGenerator
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000013*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20db
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DebugInfoGenerator::.ctor

  .method /*06000014*/ public hidebysig static 
          class System.Runtime.CompilerServices.DebugInfoGenerator/*0200000B*/ 
          CreatePdbGenerator() cil managed
  // SIG: 00 00 12 2C
  {
    .custom /*0C000057:0A000012*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000012 */ = ( 01 00 55 54 68 65 20 43 72 65 61 74 65 50 64 62   // ..UThe CreatePdb
                                                                                                                                                   47 65 6E 65 72 61 74 6F 72 20 41 50 49 20 69 73   // Generator API is
                                                                                                                                                   20 6E 6F 74 20 73 75 70 70 6F 72 74 65 64 20 61   //  not supported a
                                                                                                                                                   6E 64 20 74 68 72 6F 77 73 20 50 6C 61 74 66 6F   // nd throws Platfo
                                                                                                                                                   72 6D 4E 6F 74 53 75 70 70 6F 72 74 65 64 45 78   // rmNotSupportedEx
                                                                                                                                                   63 65 70 74 69 6F 6E 2E 02 00 54 0E 0C 44 69 61   // ception...T..Dia
                                                                                                                                                   67 6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42   // gnosticId.SYSLIB
                                                                                                                                                   30 30 30 38 54 0E 09 55 72 6C 46 6F 72 6D 61 74   // 0008T..UrlFormat
                                                                                                                                                   22 68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F   // "https://aka.ms/
                                                                                                                                                   64 6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F   // dotnet-warnings/
                                                                                                                                                   7B 30 7D )                                        // {0}
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoGenerator::CreatePdbGenerator

  .method /*06000015*/ public hidebysig newslot abstract virtual 
          instance void  MarkSequencePoint(class System.Linq.Expressions.LambdaExpression/*0200002B*/ 'method',
                                           int32 ilOffset,
                                           class System.Linq.Expressions.DebugInfoExpression/*0200001A*/ sequencePoint) cil managed
  // SIG: 20 03 01 12 80 AC 08 12 68
  {
    // Method begins at RVA 0x0
  } // end of method DebugInfoGenerator::MarkSequencePoint

} // end of class System.Runtime.CompilerServices.DebugInfoGenerator

.class /*0200000C*/ public auto ansi sealed beforefieldinit System.Runtime.CompilerServices.DynamicAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000040:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 8C 29 00 00 00 00 )                         // ...)....
  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003A       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00003A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicAttribute::.ctor

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool[] transformFlags) cil managed
  // SIG: 20 01 01 1D 02
  {
    // Method begins at RVA 0x20ee
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003A       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A00003A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicAttribute::.ctor

  .method /*06000018*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001A*/<bool> 
          get_TransformFlags() cil managed
  // SIG: 20 00 15 12 69 01 02
  {
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicAttribute::get_TransformFlags

  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001A*/<bool>
          TransformFlags()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001A*/<bool> System.Runtime.CompilerServices.DynamicAttribute/*0200000C*/::get_TransformFlags() /* 06000018 */
  } // end of property DynamicAttribute::TransformFlags
} // end of class System.Runtime.CompilerServices.DynamicAttribute

.class /*0200000D*/ interface public abstract auto ansi System.Runtime.CompilerServices.IRuntimeVariables
{
  .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000043:0A000013*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001B*/::.ctor(string) /* 0A000013 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*06000019*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IRuntimeVariables::get_Count

  .method /*0600001A*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_Item(int32 index) cil managed
  // SIG: 20 01 1C 08
  {
    // Method begins at RVA 0x0
  } // end of method IRuntimeVariables::get_Item

  .method /*0600001B*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Item(int32 index,
                                  object 'value') cil managed
  // SIG: 20 02 01 08 1C
  {
    // Method begins at RVA 0x0
  } // end of method IRuntimeVariables::set_Item

  .property /*17000005*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Runtime.CompilerServices.IRuntimeVariables/*0200000D*/::get_Count() /* 06000019 */
  } // end of property IRuntimeVariables::Count
  .property /*17000006*/ instance callconv(8) object
          Item(int32)
  {
    .get instance object System.Runtime.CompilerServices.IRuntimeVariables/*0200000D*/::get_Item(int32) /* 0600001A */
    .set instance void System.Runtime.CompilerServices.IRuntimeVariables/*0200000D*/::set_Item(int32,
                                                                                               object) /* 0600001B */
  } // end of property IRuntimeVariables::Item
} // end of class System.Runtime.CompilerServices.IRuntimeVariables

.class /*0200000E*/ public auto ansi sealed beforefieldinit System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<!T>/*1B000001*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<!T>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001A*/<!T>/*1B000003*/,
                  [System.Runtime/*23000001*/]System.Collections.ICollection/*0100001D*/,
                  [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/
{
  .custom /*0C000046:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000048:0A000013*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001B*/::.ctor(string) /* 0A000013 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<!T>/*1B000001*/
  .custom /*0C000001:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<!T>/*1B000002*/
  .custom /*0C000019:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/
  .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001A*/<!T>/*1B000003*/
  .custom /*0C000023:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.ICollection/*0100001D*/
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type T /*2A000003*/ 
    .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::.ctor

  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<!T> collection) cil managed
  // SIG: 20 01 01 15 12 80 89 01 13 00
  {
    // Method begins at RVA 0x2101
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::.ctor

  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 capacity) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2109
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::.ctor

  .method /*0600001F*/ public hidebysig specialname 
          instance int32  get_Capacity() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2111
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::get_Capacity

  .method /*06000020*/ public hidebysig specialname 
          instance void  set_Capacity(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2114
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::set_Capacity

  .method /*06000021*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::get_Count

  .method /*06000022*/ public hidebysig newslot specialname virtual final 
          instance !T  get_Item(int32 index) cil managed
  // SIG: 20 01 13 00 08
  {
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::get_Item

  .method /*06000023*/ public hidebysig newslot specialname virtual final 
          instance void  set_Item(int32 index,
                                  !T 'value') cil managed
  // SIG: 20 02 01 08 13 00
  {
    // Method begins at RVA 0x211c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::set_Item

  .method /*06000024*/ private hidebysig newslot specialname virtual final 
          instance bool  'System.Collections.Generic.ICollection<T>.get_IsReadOnly'() cil managed
  // SIG: 20 00 02
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<!T>/*1B000001*/::get_IsReadOnly() /* 0A000014 */
    // Method begins at RVA 0x211e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::'System.Collections.Generic.ICollection<T>.get_IsReadOnly'

  .method /*06000025*/ private hidebysig newslot specialname virtual final 
          instance bool  System.Collections.ICollection.get_IsSynchronized() cil managed
  // SIG: 20 00 02
  {
    .override [System.Runtime/*23000001*/]System.Collections.ICollection/*0100001D*/::get_IsSynchronized /*0100001D::0A000015*/ 
    // Method begins at RVA 0x2121
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.ICollection.get_IsSynchronized

  .method /*06000026*/ private hidebysig newslot specialname virtual final 
          instance object  System.Collections.ICollection.get_SyncRoot() cil managed
  // SIG: 20 00 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.ICollection/*0100001D*/::get_SyncRoot /*0100001D::0A000016*/ 
    // Method begins at RVA 0x2124
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.ICollection.get_SyncRoot

  .method /*06000027*/ private hidebysig newslot specialname virtual final 
          instance bool  System.Collections.IList.get_IsFixedSize() cil managed
  // SIG: 20 00 02
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::get_IsFixedSize /*0100001E::0A000017*/ 
    // Method begins at RVA 0x2127
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.get_IsFixedSize

  .method /*06000028*/ private hidebysig newslot specialname virtual final 
          instance bool  System.Collections.IList.get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::get_IsReadOnly /*0100001E::0A000018*/ 
    // Method begins at RVA 0x212a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.get_IsReadOnly

  .method /*06000029*/ private hidebysig newslot specialname virtual final 
          instance object  System.Collections.IList.get_Item(int32 index) cil managed
  // SIG: 20 01 1C 08
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::get_Item /*0100001E::0A000019*/ 
    // Method begins at RVA 0x212d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.get_Item

  .method /*0600002A*/ private hidebysig newslot specialname virtual final 
          instance void  System.Collections.IList.set_Item(int32 index,
                                                           object 'value') cil managed
  // SIG: 20 02 01 08 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::set_Item /*0100001E::0A00001A*/ 
    // Method begins at RVA 0x2130
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.set_Item

  .method /*0600002B*/ public hidebysig newslot virtual final 
          instance void  Add(!T item) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2132
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::Add

  .method /*0600002C*/ public hidebysig newslot virtual final 
          instance void  Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2134
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::Clear

  .method /*0600002D*/ public hidebysig newslot virtual final 
          instance bool  Contains(!T item) cil managed
  // SIG: 20 01 02 13 00
  {
    // Method begins at RVA 0x2136
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::Contains

  .method /*0600002E*/ public hidebysig newslot virtual final 
          instance void  CopyTo(!T[] 'array',
                                int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 13 00 08
  {
    // Method begins at RVA 0x2139
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::CopyTo

  .method /*0600002F*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<!T> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 80 8D 01 13 00
  {
    // Method begins at RVA 0x213b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::GetEnumerator

  .method /*06000030*/ public hidebysig newslot virtual final 
          instance int32  IndexOf(!T item) cil managed
  // SIG: 20 01 08 13 00
  {
    // Method begins at RVA 0x213e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::IndexOf

  .method /*06000031*/ public hidebysig newslot virtual final 
          instance void  Insert(int32 index,
                                !T item) cil managed
  // SIG: 20 02 01 08 13 00
  {
    // Method begins at RVA 0x2141
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::Insert

  .method /*06000032*/ public hidebysig newslot virtual final 
          instance bool  Remove(!T item) cil managed
  // SIG: 20 01 02 13 00
  {
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::Remove

  .method /*06000033*/ public hidebysig newslot virtual final 
          instance void  RemoveAt(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2146
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::RemoveAt

  .method /*06000034*/ public hidebysig instance void 
          Reverse() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2148
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::Reverse

  .method /*06000035*/ public hidebysig instance void 
          Reverse(int32 index,
                  int32 count) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x214a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::Reverse

  .method /*06000036*/ private hidebysig newslot virtual final 
          instance void  System.Collections.ICollection.CopyTo(class [System.Runtime/*23000001*/]System.Array/*0100001F*/ 'array',
                                                               int32 index) cil managed
  // SIG: 20 02 01 12 7D 08
  {
    .override [System.Runtime/*23000001*/]System.Collections.ICollection/*0100001D*/::CopyTo /*0100001D::0A00001B*/ 
    // Method begins at RVA 0x214c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.ICollection.CopyTo

  .method /*06000037*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 85
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/::GetEnumerator /*01000020::0A00001C*/ 
    // Method begins at RVA 0x214e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IEnumerable.GetEnumerator

  .method /*06000038*/ private hidebysig newslot virtual final 
          instance int32  System.Collections.IList.Add(object 'value') cil managed
  // SIG: 20 01 08 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::Add /*0100001E::0A00001D*/ 
    // Method begins at RVA 0x2151
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.Add

  .method /*06000039*/ private hidebysig newslot virtual final 
          instance bool  System.Collections.IList.Contains(object 'value') cil managed
  // SIG: 20 01 02 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::Contains /*0100001E::0A00001E*/ 
    // Method begins at RVA 0x2154
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.Contains

  .method /*0600003A*/ private hidebysig newslot virtual final 
          instance int32  System.Collections.IList.IndexOf(object 'value') cil managed
  // SIG: 20 01 08 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::IndexOf /*0100001E::0A00001F*/ 
    // Method begins at RVA 0x2157
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.IndexOf

  .method /*0600003B*/ private hidebysig newslot virtual final 
          instance void  System.Collections.IList.Insert(int32 index,
                                                         object 'value') cil managed
  // SIG: 20 02 01 08 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::Insert /*0100001E::0A000020*/ 
    // Method begins at RVA 0x215a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.Insert

  .method /*0600003C*/ private hidebysig newslot virtual final 
          instance void  System.Collections.IList.Remove(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IList/*0100001E*/::Remove /*0100001E::0A000021*/ 
    // Method begins at RVA 0x215c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ReadOnlyCollectionBuilder`1::System.Collections.IList.Remove

  .method /*0600003D*/ public hidebysig instance !T[] 
          ToArray() cil managed
  // SIG: 20 00 1D 13 00
  {
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::ToArray

  .method /*0600003E*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<!T> 
          ToReadOnlyCollection() cil managed
  // SIG: 20 00 15 12 5D 01 13 00
  {
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyCollectionBuilder`1::ToReadOnlyCollection

  .property /*17000007*/ instance callconv(8) int32
          Capacity()
  {
    .get instance int32 System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::get_Capacity() /* 0600001F */
    .set instance void System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::set_Capacity(int32) /* 06000020 */
  } // end of property ReadOnlyCollectionBuilder`1::Capacity
  .property /*17000008*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::get_Count() /* 06000021 */
  } // end of property ReadOnlyCollectionBuilder`1::Count
  .property /*17000009*/ instance callconv(8) !T
          Item(int32)
  {
    .get instance !T System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::get_Item(int32) /* 06000022 */
    .set instance void System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::set_Item(int32,
                                                                                                         !T) /* 06000023 */
  } // end of property ReadOnlyCollectionBuilder`1::Item
  .property /*1700000A*/ instance callconv(8) bool
          'System.Collections.Generic.ICollection<T>.IsReadOnly'()
  {
    .get instance bool System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::'System.Collections.Generic.ICollection<T>.get_IsReadOnly'() /* 06000024 */
  } // end of property ReadOnlyCollectionBuilder`1::'System.Collections.Generic.ICollection<T>.IsReadOnly'
  .property /*1700000B*/ instance callconv(8) bool
          System.Collections.ICollection.IsSynchronized()
  {
    .get instance bool System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::System.Collections.ICollection.get_IsSynchronized() /* 06000025 */
  } // end of property ReadOnlyCollectionBuilder`1::System.Collections.ICollection.IsSynchronized
  .property /*1700000C*/ instance callconv(8) object
          System.Collections.ICollection.SyncRoot()
  {
    .get instance object System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::System.Collections.ICollection.get_SyncRoot() /* 06000026 */
  } // end of property ReadOnlyCollectionBuilder`1::System.Collections.ICollection.SyncRoot
  .property /*1700000D*/ instance callconv(8) bool
          System.Collections.IList.IsFixedSize()
  {
    .get instance bool System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::System.Collections.IList.get_IsFixedSize() /* 06000027 */
  } // end of property ReadOnlyCollectionBuilder`1::System.Collections.IList.IsFixedSize
  .property /*1700000E*/ instance callconv(8) bool
          System.Collections.IList.IsReadOnly()
  {
    .get instance bool System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::System.Collections.IList.get_IsReadOnly() /* 06000028 */
  } // end of property ReadOnlyCollectionBuilder`1::System.Collections.IList.IsReadOnly
  .property /*1700000F*/ instance callconv(8) object
          System.Collections.IList.Item(int32)
  {
    .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::System.Collections.IList.get_Item(int32) /* 06000029 */
    .set instance void System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1/*0200000E*/::System.Collections.IList.set_Item(int32,
                                                                                                                                  object) /* 0600002A */
  } // end of property ReadOnlyCollectionBuilder`1::System.Collections.IList.Item
} // end of class System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1

.class /*0200000F*/ public auto ansi beforefieldinit System.Runtime.CompilerServices.RuleCache`1<class T>
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00004C:0A000022*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000025*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000024*/) /* 0A000022 */ = ( 01 00 01 00 00 00 00 00 ) 
  .param type T /*2A000005*/ 
    .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*0600003F*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2164
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuleCache`1::.ctor

} // end of class System.Runtime.CompilerServices.RuleCache`1

.class /*02000010*/ interface public abstract auto ansi System.Linq.IOrderedQueryable
       implements [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/,
                  System.Linq.IQueryable/*02000012*/
{
} // end of class System.Linq.IOrderedQueryable

.class /*02000011*/ interface public abstract auto ansi System.Linq.IOrderedQueryable`1<+ T>
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<!T>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/,
                  System.Linq.IOrderedQueryable/*02000010*/,
                  System.Linq.IQueryable/*02000012*/,
                  class System.Linq.IQueryable`1/*02000013*/<!T>/*1B000004*/
{
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<!T>/*1B000002*/
  .custom /*0C000036:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Linq.IQueryable`1/*02000013*/<!T>/*1B000004*/
  .custom /*0C000044:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000006*/ 
    .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
} // end of class System.Linq.IOrderedQueryable`1

.class /*02000012*/ interface public abstract auto ansi System.Linq.IQueryable
       implements [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/
{
  .custom /*0C000052:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/
  .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000040*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ElementType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x0
  } // end of method IQueryable::get_ElementType

  .method /*06000041*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Expression() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x0
  } // end of method IQueryable::get_Expression

  .method /*06000042*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Linq.IQueryProvider/*02000014*/ 
          get_Provider() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x0
  } // end of method IQueryable::get_Provider

  .property /*17000010*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ElementType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.IQueryable/*02000012*/::get_ElementType() /* 06000040 */
  } // end of property IQueryable::ElementType
  .property /*17000011*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Expression()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.IQueryable/*02000012*/::get_Expression() /* 06000041 */
  } // end of property IQueryable::Expression
  .property /*17000012*/ instance callconv(8) class System.Linq.IQueryProvider/*02000014*/
          Provider()
  {
    .get instance class System.Linq.IQueryProvider/*02000014*/ System.Linq.IQueryable/*02000012*/::get_Provider() /* 06000042 */
  } // end of property IQueryable::Provider
} // end of class System.Linq.IQueryable

.class /*02000013*/ interface public abstract auto ansi System.Linq.IQueryable`1<+ T>
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<!T>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/,
                  System.Linq.IQueryable/*02000012*/
{
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<!T>/*1B000002*/
  .custom /*0C00004D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000007*/ 
    .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
} // end of class System.Linq.IQueryable`1

.class /*02000014*/ interface public abstract auto ansi System.Linq.IQueryProvider
{
  .custom /*0C000058:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000043*/ public hidebysig newslot abstract virtual 
          instance class System.Linq.IQueryable/*02000012*/ 
          CreateQuery(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 20 01 12 48 12 7C
  {
    // Method begins at RVA 0x0
  } // end of method IQueryProvider::CreateQuery

  .method /*06000044*/ public hidebysig newslot abstract virtual 
          instance class System.Linq.IQueryable`1/*02000013*/<!!TElement> 
          CreateQuery<TElement>(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 30 01 01 15 12 4C 01 1E 00 12 7C
  {
    .param type TElement /*2A000009*/ 
      .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IQueryProvider::CreateQuery

  .method /*06000045*/ public hidebysig newslot abstract virtual 
          instance object  Execute(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 20 01 1C 12 7C
  {
    .param [0]/*08000031*/ 
    .custom /*0C000099:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IQueryProvider::Execute

  .method /*06000046*/ public hidebysig newslot abstract virtual 
          instance !!TResult  Execute<TResult>(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 30 01 01 1E 00 12 7C
  {
    .param type TResult /*2A00000A*/ 
      .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IQueryProvider::Execute

} // end of class System.Linq.IQueryProvider

.class /*02000015*/ public auto ansi beforefieldinit System.Linq.Expressions.BinaryExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000047*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x216c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BinaryExpression::.ctor

  .method /*06000048*/ public hidebysig specialname virtual 
          instance bool  get_CanReduce() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2174
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::get_CanReduce

  .method /*06000049*/ public hidebysig specialname 
          instance class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          get_Conversion() cil managed
  // SIG: 20 00 12 80 AC
  {
    .custom /*0C0000CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2177
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::get_Conversion

  .method /*0600004A*/ public hidebysig specialname 
          instance bool  get_IsLifted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x217a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::get_IsLifted

  .method /*0600004B*/ public hidebysig specialname 
          instance bool  get_IsLiftedToNull() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x217d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::get_IsLiftedToNull

  .method /*0600004C*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Left() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x2180
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::get_Left

  .method /*0600004D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 
          get_Method() cil managed
  // SIG: 20 00 12 80 99
  {
    .custom /*0C0000D6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2183
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::get_Method

  .method /*0600004E*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Right() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::get_Right

  .method /*0600004F*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2189
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::Accept

  .method /*06000050*/ public hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Reduce() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x218c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::Reduce

  .method /*06000051*/ public hidebysig instance class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ left,
                 class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion,
                 class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 20 03 12 54 12 7C 12 80 AC 12 7C
  {
    .param [2]/*08000036*/ 
    .custom /*0C0000A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryExpression::Update

  .property /*17000013*/ instance callconv(8) bool
          CanReduce()
  {
    .get instance bool System.Linq.Expressions.BinaryExpression/*02000015*/::get_CanReduce() /* 06000048 */
  } // end of property BinaryExpression::CanReduce
  .property /*17000014*/ instance callconv(8) class System.Linq.Expressions.LambdaExpression/*0200002B*/
          Conversion()
  {
    .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.LambdaExpression/*0200002B*/ System.Linq.Expressions.BinaryExpression/*02000015*/::get_Conversion() /* 06000049 */
  } // end of property BinaryExpression::Conversion
  .property /*17000015*/ instance callconv(8) bool
          IsLifted()
  {
    .get instance bool System.Linq.Expressions.BinaryExpression/*02000015*/::get_IsLifted() /* 0600004A */
  } // end of property BinaryExpression::IsLifted
  .property /*17000016*/ instance callconv(8) bool
          IsLiftedToNull()
  {
    .get instance bool System.Linq.Expressions.BinaryExpression/*02000015*/::get_IsLiftedToNull() /* 0600004B */
  } // end of property BinaryExpression::IsLiftedToNull
  .property /*17000017*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Left()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.BinaryExpression/*02000015*/::get_Left() /* 0600004C */
  } // end of property BinaryExpression::Left
  .property /*17000018*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/
          Method()
  {
    .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ System.Linq.Expressions.BinaryExpression/*02000015*/::get_Method() /* 0600004D */
  } // end of property BinaryExpression::Method
  .property /*17000019*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Right()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.BinaryExpression/*02000015*/::get_Right() /* 0600004E */
  } // end of property BinaryExpression::Right
} // end of class System.Linq.Expressions.BinaryExpression

.class /*02000016*/ public auto ansi beforefieldinit System.Linq.Expressions.BlockExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C00005F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000060:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000052*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2192
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BlockExpression::.ctor

  .method /*06000053*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_Expressions() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BlockExpression::get_Expressions

  .method /*06000054*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BlockExpression::get_NodeType

  .method /*06000055*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Result() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BlockExpression::get_Result

  .method /*06000056*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BlockExpression::get_Type

  .method /*06000057*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> 
          get_Variables() cil managed
  // SIG: 20 00 15 12 5D 01 12 80 E0
  {
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BlockExpression::get_Variables

  .method /*06000058*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BlockExpression::Accept

  .method /*06000059*/ public hidebysig instance class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> variables,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> expressions) cil managed
  // SIG: 20 02 12 58 15 12 80 89 01 12 80 E0 15 12 80 89 01 12 7C
  {
    .param [1]/*08000039*/ 
    .custom /*0C0000AB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BlockExpression::Update

  .property /*1700001A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          Expressions()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.BlockExpression/*02000016*/::get_Expressions() /* 06000053 */
  } // end of property BlockExpression::Expressions
  .property /*1700001B*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.BlockExpression/*02000016*/::get_NodeType() /* 06000054 */
  } // end of property BlockExpression::NodeType
  .property /*1700001C*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Result()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.BlockExpression/*02000016*/::get_Result() /* 06000055 */
  } // end of property BlockExpression::Result
  .property /*1700001D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.BlockExpression/*02000016*/::get_Type() /* 06000056 */
  } // end of property BlockExpression::Type
  .property /*1700001E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/>
          Variables()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> System.Linq.Expressions.BlockExpression/*02000016*/::get_Variables() /* 06000057 */
  } // end of property BlockExpression::Variables
} // end of class System.Linq.Expressions.BlockExpression

.class /*02000017*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.CatchBlock
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000062:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21af
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CatchBlock::.ctor

  .method /*0600005B*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Body() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x21b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CatchBlock::get_Body

  .method /*0600005C*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Filter() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C0000E5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CatchBlock::get_Filter

  .method /*0600005D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Test() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CatchBlock::get_Test

  .method /*0600005E*/ public hidebysig specialname 
          instance class System.Linq.Expressions.ParameterExpression/*02000038*/ 
          get_Variable() cil managed
  // SIG: 20 00 12 80 E0
  {
    .custom /*0C0000E7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CatchBlock::get_Variable

  .method /*0600005F*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CatchBlock::ToString

  .method /*06000060*/ public hidebysig instance class System.Linq.Expressions.CatchBlock/*02000017*/ 
          Update(class System.Linq.Expressions.ParameterExpression/*02000038*/ variable,
                 class System.Linq.Expressions.Expression/*0200001F*/ 'filter',
                 class System.Linq.Expressions.Expression/*0200001F*/ body) cil managed
  // SIG: 20 03 12 5C 12 80 E0 12 7C 12 7C
  {
    .param [1]/*0800003B*/ 
    .custom /*0C0000B0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800003C*/ 
    .custom /*0C0000B3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CatchBlock::Update

  .property /*1700001F*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Body()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.CatchBlock/*02000017*/::get_Body() /* 0600005B */
  } // end of property CatchBlock::Body
  .property /*17000020*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Filter()
  {
    .custom /*0C00007A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.CatchBlock/*02000017*/::get_Filter() /* 0600005C */
  } // end of property CatchBlock::Filter
  .property /*17000021*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Test()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.CatchBlock/*02000017*/::get_Test() /* 0600005D */
  } // end of property CatchBlock::Test
  .property /*17000022*/ instance callconv(8) class System.Linq.Expressions.ParameterExpression/*02000038*/
          Variable()
  {
    .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.ParameterExpression/*02000038*/ System.Linq.Expressions.CatchBlock/*02000017*/::get_Variable() /* 0600005E */
  } // end of property CatchBlock::Variable
} // end of class System.Linq.Expressions.CatchBlock

.class /*02000018*/ public auto ansi beforefieldinit System.Linq.Expressions.ConditionalExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000061*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21c9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConditionalExpression::.ctor

  .method /*06000062*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_IfFalse() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x21d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConditionalExpression::get_IfFalse

  .method /*06000063*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_IfTrue() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x21d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConditionalExpression::get_IfTrue

  .method /*06000064*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x21d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConditionalExpression::get_NodeType

  .method /*06000065*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Test() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x21da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConditionalExpression::get_Test

  .method /*06000066*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x21dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConditionalExpression::get_Type

  .method /*06000067*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x21e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConditionalExpression::Accept

  .method /*06000068*/ public hidebysig instance class System.Linq.Expressions.ConditionalExpression/*02000018*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ test,
                 class System.Linq.Expressions.Expression/*0200001F*/ ifTrue,
                 class System.Linq.Expressions.Expression/*0200001F*/ ifFalse) cil managed
  // SIG: 20 03 12 60 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x21e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConditionalExpression::Update

  .property /*17000023*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          IfFalse()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.ConditionalExpression/*02000018*/::get_IfFalse() /* 06000062 */
  } // end of property ConditionalExpression::IfFalse
  .property /*17000024*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          IfTrue()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.ConditionalExpression/*02000018*/::get_IfTrue() /* 06000063 */
  } // end of property ConditionalExpression::IfTrue
  .property /*17000025*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.ConditionalExpression/*02000018*/::get_NodeType() /* 06000064 */
  } // end of property ConditionalExpression::NodeType
  .property /*17000026*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Test()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.ConditionalExpression/*02000018*/::get_Test() /* 06000065 */
  } // end of property ConditionalExpression::Test
  .property /*17000027*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.ConditionalExpression/*02000018*/::get_Type() /* 06000066 */
  } // end of property ConditionalExpression::Type
} // end of class System.Linq.Expressions.ConditionalExpression

.class /*02000019*/ public auto ansi beforefieldinit System.Linq.Expressions.ConstantExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000069*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConstantExpression::.ctor

  .method /*0600006A*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstantExpression::get_NodeType

  .method /*0600006B*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstantExpression::get_Type

  .method /*0600006C*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0000EB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstantExpression::get_Value

  .method /*0600006D*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstantExpression::Accept

  .property /*17000028*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.ConstantExpression/*02000019*/::get_NodeType() /* 0600006A */
  } // end of property ConstantExpression::NodeType
  .property /*17000029*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.ConstantExpression/*02000019*/::get_Type() /* 0600006B */
  } // end of property ConstantExpression::Type
  .property /*1700002A*/ instance callconv(8) object
          Value()
  {
    .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Linq.Expressions.ConstantExpression/*02000019*/::get_Value() /* 0600006C */
  } // end of property ConstantExpression::Value
} // end of class System.Linq.Expressions.ConstantExpression

.class /*0200001A*/ public auto ansi beforefieldinit System.Linq.Expressions.DebugInfoExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C00006C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006E*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21fa
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DebugInfoExpression::.ctor

  .method /*0600006F*/ public hidebysig specialname 
          instance class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/ 
          get_Document() cil managed
  // SIG: 20 00 12 80 F0
  {
    // Method begins at RVA 0x2202
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::get_Document

  .method /*06000070*/ public hidebysig newslot specialname virtual 
          instance int32  get_EndColumn() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2205
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::get_EndColumn

  .method /*06000071*/ public hidebysig newslot specialname virtual 
          instance int32  get_EndLine() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2208
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::get_EndLine

  .method /*06000072*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsClear() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x220b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::get_IsClear

  .method /*06000073*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x220e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::get_NodeType

  .method /*06000074*/ public hidebysig newslot specialname virtual 
          instance int32  get_StartColumn() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2211
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::get_StartColumn

  .method /*06000075*/ public hidebysig newslot specialname virtual 
          instance int32  get_StartLine() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::get_StartLine

  .method /*06000076*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::get_Type

  .method /*06000077*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DebugInfoExpression::Accept

  .property /*1700002B*/ instance callconv(8) class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/
          Document()
  {
    .get instance class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/ System.Linq.Expressions.DebugInfoExpression/*0200001A*/::get_Document() /* 0600006F */
  } // end of property DebugInfoExpression::Document
  .property /*1700002C*/ instance callconv(8) int32
          EndColumn()
  {
    .get instance int32 System.Linq.Expressions.DebugInfoExpression/*0200001A*/::get_EndColumn() /* 06000070 */
  } // end of property DebugInfoExpression::EndColumn
  .property /*1700002D*/ instance callconv(8) int32
          EndLine()
  {
    .get instance int32 System.Linq.Expressions.DebugInfoExpression/*0200001A*/::get_EndLine() /* 06000071 */
  } // end of property DebugInfoExpression::EndLine
  .property /*1700002E*/ instance callconv(8) bool
          IsClear()
  {
    .get instance bool System.Linq.Expressions.DebugInfoExpression/*0200001A*/::get_IsClear() /* 06000072 */
  } // end of property DebugInfoExpression::IsClear
  .property /*1700002F*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.DebugInfoExpression/*0200001A*/::get_NodeType() /* 06000073 */
  } // end of property DebugInfoExpression::NodeType
  .property /*17000030*/ instance callconv(8) int32
          StartColumn()
  {
    .get instance int32 System.Linq.Expressions.DebugInfoExpression/*0200001A*/::get_StartColumn() /* 06000074 */
  } // end of property DebugInfoExpression::StartColumn
  .property /*17000031*/ instance callconv(8) int32
          StartLine()
  {
    .get instance int32 System.Linq.Expressions.DebugInfoExpression/*0200001A*/::get_StartLine() /* 06000075 */
  } // end of property DebugInfoExpression::StartLine
  .property /*17000032*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.DebugInfoExpression/*0200001A*/::get_Type() /* 06000076 */
  } // end of property DebugInfoExpression::Type
} // end of class System.Linq.Expressions.DebugInfoExpression

.class /*0200001B*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.DefaultExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000078*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x221d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DefaultExpression::.ctor

  .method /*06000079*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2225
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultExpression::get_NodeType

  .method /*0600007A*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2228
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultExpression::get_Type

  .method /*0600007B*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x222b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultExpression::Accept

  .property /*17000033*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.DefaultExpression/*0200001B*/::get_NodeType() /* 06000079 */
  } // end of property DefaultExpression::NodeType
  .property /*17000034*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.DefaultExpression/*0200001B*/::get_Type() /* 0600007A */
  } // end of property DefaultExpression::Type
} // end of class System.Linq.Expressions.DefaultExpression

.class /*0200001C*/ public auto ansi beforefieldinit System.Linq.Expressions.DynamicExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
       implements System.Linq.Expressions.IArgumentProvider/*02000025*/,
                  System.Linq.Expressions.IDynamicExpression/*02000026*/
{
  .custom /*0C000072:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Linq.Expressions.IArgumentProvider/*02000025*/
  .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Linq.Expressions.IDynamicExpression/*02000026*/
  .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600007C*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x222e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicExpression::.ctor

  .method /*0600007D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_Arguments() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::get_Arguments

  .method /*0600007E*/ public hidebysig specialname 
          instance class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ 
          get_Binder() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::get_Binder

  .method /*0600007F*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_DelegateType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::get_DelegateType

  .method /*06000080*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::get_NodeType

  .method /*06000081*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() cil managed
  // SIG: 20 00 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::get_ArgumentCount /*02000025::06000213*/ 
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount

  .method /*06000082*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::get_Type

  .method /*06000083*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::Accept

  .method /*06000084*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 03 12 70 12 20 12 59 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::Dynamic

  .method /*06000085*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg0) cil managed
  // SIG: 00 03 12 70 12 20 12 59 12 7C
  {
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::Dynamic

  .method /*06000086*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg1) cil managed
  // SIG: 00 04 12 70 12 20 12 59 12 7C 12 7C
  {
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::Dynamic

  .method /*06000087*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg2) cil managed
  // SIG: 00 05 12 70 12 20 12 59 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2254
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::Dynamic

  .method /*06000088*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg2,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg3) cil managed
  // SIG: 00 06 12 70 12 20 12 59 12 7C 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::Dynamic

  .method /*06000089*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 03 12 70 12 20 12 59 1D 12 7C
  {
    .param [3]/*0800005D*/ 
    .custom /*0C0000E6:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::Dynamic

  .method /*0600008A*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 03 12 70 12 59 12 20 15 12 80 89 01 12 7C
  {
    .param [3]/*08000060*/ 
    .custom /*0C0000E8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::MakeDynamic

  .method /*0600008B*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg0) cil managed
  // SIG: 00 03 12 70 12 59 12 20 12 7C
  {
    // Method begins at RVA 0x2260
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::MakeDynamic

  .method /*0600008C*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg1) cil managed
  // SIG: 00 04 12 70 12 59 12 20 12 7C 12 7C
  {
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::MakeDynamic

  .method /*0600008D*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg2) cil managed
  // SIG: 00 05 12 70 12 59 12 20 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::MakeDynamic

  .method /*0600008E*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg2,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg3) cil managed
  // SIG: 00 06 12 70 12 59 12 20 12 7C 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2269
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::MakeDynamic

  .method /*0600008F*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 03 12 70 12 59 12 20 1D 12 7C
  {
    .param [3]/*08000075*/ 
    .custom /*0C0000ED:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C0000EE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::MakeDynamic

  .method /*06000090*/ private hidebysig newslot virtual final 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          System.Linq.Expressions.IArgumentProvider.GetArgument(int32 index) cil managed
  // SIG: 20 01 12 7C 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::GetArgument /*02000025::06000214*/ 
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::System.Linq.Expressions.IArgumentProvider.GetArgument

  .method /*06000091*/ private hidebysig newslot virtual final 
          instance object  System.Linq.Expressions.IDynamicExpression.CreateCallSite() cil managed
  // SIG: 20 00 1C
  {
    .override System.Linq.Expressions.IDynamicExpression/*02000026*/::CreateCallSite /*02000026::06000216*/ 
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::System.Linq.Expressions.IDynamicExpression.CreateCallSite

  .method /*06000092*/ private hidebysig newslot virtual final 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          System.Linq.Expressions.IDynamicExpression.Rewrite(class System.Linq.Expressions.Expression/*0200001F*/[] args) cil managed
  // SIG: 20 01 12 7C 1D 12 7C
  {
    .override System.Linq.Expressions.IDynamicExpression/*02000026*/::Rewrite /*02000026::06000217*/ 
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::System.Linq.Expressions.IDynamicExpression.Rewrite

  .method /*06000093*/ public hidebysig instance class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 20 01 12 70 15 12 80 89 01 12 7C
  {
    .param [1]/*08000078*/ 
    .custom /*0C0000EF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpression::Update

  .property /*17000035*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          Arguments()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.DynamicExpression/*0200001C*/::get_Arguments() /* 0600007D */
  } // end of property DynamicExpression::Arguments
  .property /*17000036*/ instance callconv(8) class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/
          Binder()
  {
    .get instance class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ System.Linq.Expressions.DynamicExpression/*0200001C*/::get_Binder() /* 0600007E */
  } // end of property DynamicExpression::Binder
  .property /*17000037*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          DelegateType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.DynamicExpression/*0200001C*/::get_DelegateType() /* 0600007F */
  } // end of property DynamicExpression::DelegateType
  .property /*17000038*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.DynamicExpression/*0200001C*/::get_NodeType() /* 06000080 */
  } // end of property DynamicExpression::NodeType
  .property /*17000039*/ instance callconv(8) int32
          System.Linq.Expressions.IArgumentProvider.ArgumentCount()
  {
    .get instance int32 System.Linq.Expressions.DynamicExpression/*0200001C*/::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() /* 06000081 */
  } // end of property DynamicExpression::System.Linq.Expressions.IArgumentProvider.ArgumentCount
  .property /*1700003A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.DynamicExpression/*0200001C*/::get_Type() /* 06000082 */
  } // end of property DynamicExpression::Type
} // end of class System.Linq.Expressions.DynamicExpression

.class /*0200001D*/ public abstract auto ansi beforefieldinit System.Linq.Expressions.DynamicExpressionVisitor
       extends System.Linq.Expressions.ExpressionVisitor/*02000021*/
{
  .method /*06000094*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x227b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0001DD       */ call       instance void System.Linq.Expressions.ExpressionVisitor/*02000021*/::.ctor() /* 060001DD */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicExpressionVisitor::.ctor

  .method /*06000095*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitDynamic(class System.Linq.Expressions.DynamicExpression/*0200001C*/ node) cil managed
  // SIG: 20 01 12 7C 12 70
  {
    .custom /*0C0000FC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2283
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicExpressionVisitor::VisitDynamic

} // end of class System.Linq.Expressions.DynamicExpressionVisitor

.class /*0200001E*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.ElementInit
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements System.Linq.Expressions.IArgumentProvider/*02000025*/
{
  .custom /*0C000075:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Linq.Expressions.IArgumentProvider/*02000025*/
  .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000096*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2286
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ElementInit::.ctor

  .method /*06000097*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 
          get_AddMethod() cil managed
  // SIG: 20 00 12 80 99
  {
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementInit::get_AddMethod

  .method /*06000098*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_Arguments() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x2291
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementInit::get_Arguments

  .method /*06000099*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() cil managed
  // SIG: 20 00 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::get_ArgumentCount /*02000025::06000213*/ 
    // Method begins at RVA 0x2294
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementInit::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount

  .method /*0600009A*/ private hidebysig newslot virtual final 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          System.Linq.Expressions.IArgumentProvider.GetArgument(int32 index) cil managed
  // SIG: 20 01 12 7C 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::GetArgument /*02000025::06000214*/ 
    // Method begins at RVA 0x2297
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementInit::System.Linq.Expressions.IArgumentProvider.GetArgument

  .method /*0600009B*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x229a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementInit::ToString

  .method /*0600009C*/ public hidebysig instance class System.Linq.Expressions.ElementInit/*0200001E*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 20 01 12 78 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x229d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ElementInit::Update

  .property /*1700003B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/
          AddMethod()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ System.Linq.Expressions.ElementInit/*0200001E*/::get_AddMethod() /* 06000097 */
  } // end of property ElementInit::AddMethod
  .property /*1700003C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          Arguments()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.ElementInit/*0200001E*/::get_Arguments() /* 06000098 */
  } // end of property ElementInit::Arguments
  .property /*1700003D*/ instance callconv(8) int32
          System.Linq.Expressions.IArgumentProvider.ArgumentCount()
  {
    .get instance int32 System.Linq.Expressions.ElementInit/*0200001E*/::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() /* 06000099 */
  } // end of property ElementInit::System.Linq.Expressions.IArgumentProvider.ArgumentCount
} // end of class System.Linq.Expressions.ElementInit

.class /*0200001F*/ public abstract auto ansi beforefieldinit System.Linq.Expressions.Expression
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000078:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600009D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22a0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Expression::.ctor

  .method /*0600009E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Linq.Expressions.ExpressionType/*02000020*/ nodeType,
                               class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 20 02 01 11 80 80 12 59
  {
    .custom /*0C000101:0A000012*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000012 */ = ( 01 00 80 D2 54 68 69 73 20 63 6F 6E 73 74 72 75   // ....This constru
                                                                                                                                                   63 74 6F 72 20 68 61 73 20 62 65 65 6E 20 64 65   // ctor has been de
                                                                                                                                                   70 72 65 63 61 74 65 64 2E 20 55 73 65 20 61 20   // precated. Use a 
                                                                                                                                                   64 69 66 66 65 72 65 6E 74 20 63 6F 6E 73 74 72   // different constr
                                                                                                                                                   75 63 74 6F 72 20 74 68 61 74 20 64 6F 65 73 20   // uctor that does 
                                                                                                                                                   6E 6F 74 20 74 61 6B 65 20 45 78 70 72 65 73 73   // not take Express
                                                                                                                                                   69 6F 6E 54 79 70 65 2E 20 54 68 65 6E 20 6F 76   // ionType. Then ov
                                                                                                                                                   65 72 72 69 64 65 20 4E 6F 64 65 54 79 70 65 20   // erride NodeType 
                                                                                                                                                   61 6E 64 20 54 79 70 65 20 70 72 6F 70 65 72 74   // and Type propert
                                                                                                                                                   69 65 73 20 74 6F 20 70 72 6F 76 69 64 65 20 74   // ies to provide t
                                                                                                                                                   68 65 20 76 61 6C 75 65 73 20 74 68 61 74 20 77   // he values that w
                                                                                                                                                   6F 75 6C 64 20 62 65 20 73 70 65 63 69 66 69 65   // ould be specifie
                                                                                                                                                   64 20 74 6F 20 74 68 69 73 20 63 6F 6E 73 74 72   // d to this constr
                                                                                                                                                   75 63 74 6F 72 2E 00 00 )                         // uctor...
    // Method begins at RVA 0x22a8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Expression::.ctor

  .method /*0600009F*/ public hidebysig newslot specialname virtual 
          instance bool  get_CanReduce() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::get_CanReduce

  .method /*060000A0*/ public hidebysig newslot specialname virtual 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x22b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::get_NodeType

  .method /*060000A1*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x22b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::get_Type

  .method /*060000A2*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x22b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Accept

  .method /*060000A3*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Add(class System.Linq.Expressions.Expression/*0200001F*/ left,
              class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x22bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Add

  .method /*060000A4*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Add(class System.Linq.Expressions.Expression/*0200001F*/ left,
              class System.Linq.Expressions.Expression/*0200001F*/ right,
              class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*08000083*/ 
    .custom /*0C0000F3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Add

  .method /*060000A5*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AddAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                    class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x22c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AddAssign

  .method /*060000A6*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AddAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                    class System.Linq.Expressions.Expression/*0200001F*/ right,
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*08000088*/ 
    .custom /*0C0000F5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AddAssign

  .method /*060000A7*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AddAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                    class System.Linq.Expressions.Expression/*0200001F*/ right,
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                    class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*0800008B*/ 
    .custom /*0C0000F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800008C*/ 
    .custom /*0C0000F7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AddAssign

  .method /*060000A8*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AddAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                           class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x22cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AddAssignChecked

  .method /*060000A9*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AddAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                           class System.Linq.Expressions.Expression/*0200001F*/ right,
                           class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*08000091*/ 
    .custom /*0C0000FA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AddAssignChecked

  .method /*060000AA*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AddAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                           class System.Linq.Expressions.Expression/*0200001F*/ right,
                           class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                           class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000094*/ 
    .custom /*0C0000FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000095*/ 
    .custom /*0C0000FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AddAssignChecked

  .method /*060000AB*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AddChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                     class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x22d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AddChecked

  .method /*060000AC*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AddChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                     class System.Linq.Expressions.Expression/*0200001F*/ right,
                     class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800009A*/ 
    .custom /*0C0000FF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AddChecked

  .method /*060000AD*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          And(class System.Linq.Expressions.Expression/*0200001F*/ left,
              class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x22da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::And

  .method /*060000AE*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          And(class System.Linq.Expressions.Expression/*0200001F*/ left,
              class System.Linq.Expressions.Expression/*0200001F*/ right,
              class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800009F*/ 
    .custom /*0C000102:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::And

  .method /*060000AF*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AndAlso(class System.Linq.Expressions.Expression/*0200001F*/ left,
                  class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x22e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AndAlso

  .method /*060000B0*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AndAlso(class System.Linq.Expressions.Expression/*0200001F*/ left,
                  class System.Linq.Expressions.Expression/*0200001F*/ right,
                  class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080000A4*/ 
    .custom /*0C000103:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AndAlso

  .method /*060000B1*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AndAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                    class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x22e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AndAssign

  .method /*060000B2*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AndAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                    class System.Linq.Expressions.Expression/*0200001F*/ right,
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080000A9*/ 
    .custom /*0C000105:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AndAssign

  .method /*060000B3*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          AndAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                    class System.Linq.Expressions.Expression/*0200001F*/ right,
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                    class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*080000AC*/ 
    .custom /*0C000106:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000AD*/ 
    .custom /*0C000107:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::AndAssign

  .method /*060000B4*/ public hidebysig static 
          class System.Linq.Expressions.IndexExpression/*02000027*/ 
          ArrayAccess(class System.Linq.Expressions.Expression/*0200001F*/ 'array',
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> indexes) cil managed
  // SIG: 00 02 12 80 9C 12 7C 15 12 80 89 01 12 7C
  {
    .param [2]/*080000AF*/ 
    .custom /*0C000108:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ArrayAccess

  .method /*060000B5*/ public hidebysig static 
          class System.Linq.Expressions.IndexExpression/*02000027*/ 
          ArrayAccess(class System.Linq.Expressions.Expression/*0200001F*/ 'array',
                      class System.Linq.Expressions.Expression/*0200001F*/[] indexes) cil managed
  // SIG: 00 02 12 80 9C 12 7C 1D 12 7C
  {
    .param [2]/*080000B1*/ 
    .custom /*0C000109:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00010A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ArrayAccess

  .method /*060000B6*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          ArrayIndex(class System.Linq.Expressions.Expression/*0200001F*/ 'array',
                     class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> indexes) cil managed
  // SIG: 00 02 12 80 D4 12 7C 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x22f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ArrayIndex

  .method /*060000B7*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ArrayIndex(class System.Linq.Expressions.Expression/*0200001F*/ 'array',
                     class System.Linq.Expressions.Expression/*0200001F*/ index) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x22f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ArrayIndex

  .method /*060000B8*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          ArrayIndex(class System.Linq.Expressions.Expression/*0200001F*/ 'array',
                     class System.Linq.Expressions.Expression/*0200001F*/[] indexes) cil managed
  // SIG: 00 02 12 80 D4 12 7C 1D 12 7C
  {
    .param [2]/*080000B7*/ 
    .custom /*0C00010B:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ArrayIndex

  .method /*060000B9*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          ArrayLength(class System.Linq.Expressions.Expression/*0200001F*/ 'array') cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x22fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ArrayLength

  .method /*060000BA*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Assign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                 class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2301
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Assign

  .method /*060000BB*/ public hidebysig static 
          class System.Linq.Expressions.MemberAssignment/*0200002E*/ 
          Bind(class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ member,
               class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 02 12 80 B8 12 80 A1 12 7C
  {
    // Method begins at RVA 0x2304
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Bind

  .method /*060000BC*/ public hidebysig static 
          class System.Linq.Expressions.MemberAssignment/*0200002E*/ 
          Bind(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ propertyAccessor,
               class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 02 12 80 B8 12 80 99 12 7C
  {
    .custom /*0C00010D:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 37 54 68 65 20 50 72 6F 70 65 72 74 79 20   // ..7The Property 
                                                                                                                                                                                            6D 65 74 61 64 61 74 61 20 6F 72 20 6F 74 68 65   // metadata or othe
                                                                                                                                                                                            72 20 61 63 63 65 73 73 6F 72 20 6D 61 79 20 62   // r accessor may b
                                                                                                                                                                                            65 20 74 72 69 6D 6D 65 64 2E 00 00 )             // e trimmed...
    // Method begins at RVA 0x2307
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Bind

  .method /*060000BD*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> expressions) cil managed
  // SIG: 00 01 12 58 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x230a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000BE*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> variables,
                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> expressions) cil managed
  // SIG: 00 02 12 58 15 12 80 89 01 12 80 E0 15 12 80 89 01 12 7C
  {
    .param [1]/*080000C0*/ 
    .custom /*0C00010F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x230d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000BF*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> variables,
                class System.Linq.Expressions.Expression/*0200001F*/[] expressions) cil managed
  // SIG: 00 02 12 58 15 12 80 89 01 12 80 E0 1D 12 7C
  {
    .param [1]/*080000C2*/ 
    .custom /*0C000112:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*080000C3*/ 
    .custom /*0C000114:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2310
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C0*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                class System.Linq.Expressions.Expression/*0200001F*/ arg1) cil managed
  // SIG: 00 02 12 58 12 7C 12 7C
  {
    // Method begins at RVA 0x2313
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C1*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                class System.Linq.Expressions.Expression/*0200001F*/ arg2) cil managed
  // SIG: 00 03 12 58 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2316
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C2*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                class System.Linq.Expressions.Expression/*0200001F*/ arg2,
                class System.Linq.Expressions.Expression/*0200001F*/ arg3) cil managed
  // SIG: 00 04 12 58 12 7C 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2319
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C3*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                class System.Linq.Expressions.Expression/*0200001F*/ arg2,
                class System.Linq.Expressions.Expression/*0200001F*/ arg3,
                class System.Linq.Expressions.Expression/*0200001F*/ arg4) cil managed
  // SIG: 00 05 12 58 12 7C 12 7C 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x231c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C4*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class System.Linq.Expressions.Expression/*0200001F*/[] expressions) cil managed
  // SIG: 00 01 12 58 1D 12 7C
  {
    .param [1]/*080000D2*/ 
    .custom /*0C000116:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x231f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C5*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> expressions) cil managed
  // SIG: 00 02 12 58 12 59 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x2322
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C6*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> variables,
                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> expressions) cil managed
  // SIG: 00 03 12 58 12 59 15 12 80 89 01 12 80 E0 15 12 80 89 01 12 7C
  {
    .param [2]/*080000D6*/ 
    .custom /*0C000117:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2325
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C7*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> variables,
                class System.Linq.Expressions.Expression/*0200001F*/[] expressions) cil managed
  // SIG: 00 03 12 58 12 59 15 12 80 89 01 12 80 E0 1D 12 7C
  {
    .param [2]/*080000D9*/ 
    .custom /*0C000118:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080000DA*/ 
    .custom /*0C00011A:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2328
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C8*/ public hidebysig static 
          class System.Linq.Expressions.BlockExpression/*02000016*/ 
          Block(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                class System.Linq.Expressions.Expression/*0200001F*/[] expressions) cil managed
  // SIG: 00 02 12 58 12 59 1D 12 7C
  {
    .param [2]/*080000DC*/ 
    .custom /*0C00011B:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x232b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Block

  .method /*060000C9*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Break(class System.Linq.Expressions.LabelTarget/*0200002A*/ target) cil managed
  // SIG: 00 01 12 80 8C 12 80 A8
  {
    // Method begins at RVA 0x232e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Break

  .method /*060000CA*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Break(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                class System.Linq.Expressions.Expression/*0200001F*/ 'value') cil managed
  // SIG: 00 02 12 80 8C 12 80 A8 12 7C
  {
    .param [2]/*080000DF*/ 
    .custom /*0C00011C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2331
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Break

  .method /*060000CB*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Break(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                class System.Linq.Expressions.Expression/*0200001F*/ 'value',
                class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 03 12 80 8C 12 80 A8 12 7C 12 59
  {
    .param [2]/*080000E1*/ 
    .custom /*0C00011D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2334
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Break

  .method /*060000CC*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Break(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 8C 12 80 A8 12 59
  {
    // Method begins at RVA 0x2337
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Break

  .method /*060000CD*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 D4 12 7C 12 80 99
  {
    .param [1]/*080000E5*/ 
    .custom /*0C00011E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x233a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000CE*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 03 12 80 D4 12 7C 12 80 99 15 12 80 89 01 12 7C
  {
    .param [1]/*080000E7*/ 
    .custom /*0C00011F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000E9*/ 
    .custom /*0C000120:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x233d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000CF*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/ arg0,
               class System.Linq.Expressions.Expression/*0200001F*/ arg1) cil managed
  // SIG: 00 04 12 80 D4 12 7C 12 80 99 12 7C 12 7C
  {
    .param [1]/*080000EA*/ 
    .custom /*0C000121:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2340
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D0*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/ arg0,
               class System.Linq.Expressions.Expression/*0200001F*/ arg1,
               class System.Linq.Expressions.Expression/*0200001F*/ arg2) cil managed
  // SIG: 00 05 12 80 D4 12 7C 12 80 99 12 7C 12 7C 12 7C
  {
    .param [1]/*080000EE*/ 
    .custom /*0C000122:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2343
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D1*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 03 12 80 D4 12 7C 12 80 99 1D 12 7C
  {
    .param [1]/*080000F3*/ 
    .custom /*0C000123:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000F5*/ 
    .custom /*0C000124:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000125:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2346
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D2*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
               string methodName,
               class [System.Runtime/*23000001*/]System.Type/*01000016*/[] typeArguments,
               class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 04 12 80 D4 12 7C 0E 1D 12 59 1D 12 7C
  {
    .custom /*0C000115:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    .param [3]/*080000F8*/ 
    .custom /*0C000126:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*080000F9*/ 
    .custom /*0C000127:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000128:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2349
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D3*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 02 12 80 D4 12 80 99 15 12 80 89 01 12 7C
  {
    .param [2]/*080000FB*/ 
    .custom /*0C000129:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x234c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D4*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/ arg0) cil managed
  // SIG: 00 02 12 80 D4 12 80 99 12 7C
  {
    // Method begins at RVA 0x234f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D5*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/ arg0,
               class System.Linq.Expressions.Expression/*0200001F*/ arg1) cil managed
  // SIG: 00 03 12 80 D4 12 80 99 12 7C 12 7C
  {
    // Method begins at RVA 0x2352
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D6*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/ arg0,
               class System.Linq.Expressions.Expression/*0200001F*/ arg1,
               class System.Linq.Expressions.Expression/*0200001F*/ arg2) cil managed
  // SIG: 00 04 12 80 D4 12 80 99 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2355
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D7*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/ arg0,
               class System.Linq.Expressions.Expression/*0200001F*/ arg1,
               class System.Linq.Expressions.Expression/*0200001F*/ arg2,
               class System.Linq.Expressions.Expression/*0200001F*/ arg3) cil managed
  // SIG: 00 05 12 80 D4 12 80 99 12 7C 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2358
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D8*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/ arg0,
               class System.Linq.Expressions.Expression/*0200001F*/ arg1,
               class System.Linq.Expressions.Expression/*0200001F*/ arg2,
               class System.Linq.Expressions.Expression/*0200001F*/ arg3,
               class System.Linq.Expressions.Expression/*0200001F*/ arg4) cil managed
  // SIG: 00 06 12 80 D4 12 80 99 12 7C 12 7C 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x235b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000D9*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
               class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 02 12 80 D4 12 80 99 1D 12 7C
  {
    .param [2]/*08000111*/ 
    .custom /*0C00012B:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00012C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x235e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000DA*/ public hidebysig static 
          class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Call(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
               string methodName,
               class [System.Runtime/*23000001*/]System.Type/*01000016*/[] typeArguments,
               class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 04 12 80 D4 12 59 0E 1D 12 59 1D 12 7C
  {
    .custom /*0C000119:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 80 C3 43 61 6C 6C 69 6E 67 20 61 20 67 65   // ....Calling a ge
                                                                                                                                                                                            6E 65 72 69 63 20 6D 65 74 68 6F 64 20 63 61 6E   // neric method can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 49 74 27 73   // y analyzed. It's
                                                                                                                                                                                            20 6E 6F 74 20 70 6F 73 73 69 62 6C 65 20 74 6F   //  not possible to
                                                                                                                                                                                            20 67 75 61 72 61 6E 74 65 65 20 74 68 65 20 61   //  guarantee the a
                                                                                                                                                                                            76 61 69 6C 61 62 69 6C 69 74 79 20 6F 66 20 72   // vailability of r
                                                                                                                                                                                            65 71 75 69 72 65 6D 65 6E 74 73 20 6F 66 20 74   // equirements of t
                                                                                                                                                                                            68 65 20 67 65 6E 65 72 69 63 20 6D 65 74 68 6F   // he generic metho
                                                                                                                                                                                            64 2E 20 54 68 69 73 20 63 61 6E 20 62 65 20 73   // d. This can be s
                                                                                                                                                                                            75 70 70 72 65 73 73 65 64 20 69 66 20 74 68 65   // uppressed if the
                                                                                                                                                                                            20 6D 65 74 68 6F 64 20 69 73 20 6E 6F 74 20 67   //  method is not g
                                                                                                                                                                                            65 6E 65 72 69 63 2E 00 00 )                      // eneric...
    .param [1]/*08000112*/ 
    .custom /*0C00012D:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002A*/) /* 0A000025 */ = ( 01 00 18 00 00 00 00 00 ) 
    .param [3]/*08000114*/ 
    .custom /*0C00012E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*08000115*/ 
    .custom /*0C00012F:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000130:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2361
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Call

  .method /*060000DB*/ public hidebysig static 
          class System.Linq.Expressions.CatchBlock/*02000017*/ 
          Catch(class System.Linq.Expressions.ParameterExpression/*02000038*/ variable,
                class System.Linq.Expressions.Expression/*0200001F*/ body) cil managed
  // SIG: 00 02 12 5C 12 80 E0 12 7C
  {
    // Method begins at RVA 0x2364
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Catch

  .method /*060000DC*/ public hidebysig static 
          class System.Linq.Expressions.CatchBlock/*02000017*/ 
          Catch(class System.Linq.Expressions.ParameterExpression/*02000038*/ variable,
                class System.Linq.Expressions.Expression/*0200001F*/ body,
                class System.Linq.Expressions.Expression/*0200001F*/ 'filter') cil managed
  // SIG: 00 03 12 5C 12 80 E0 12 7C 12 7C
  {
    .param [3]/*0800011A*/ 
    .custom /*0C000131:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2367
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Catch

  .method /*060000DD*/ public hidebysig static 
          class System.Linq.Expressions.CatchBlock/*02000017*/ 
          Catch(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                class System.Linq.Expressions.Expression/*0200001F*/ body) cil managed
  // SIG: 00 02 12 5C 12 59 12 7C
  {
    // Method begins at RVA 0x236a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Catch

  .method /*060000DE*/ public hidebysig static 
          class System.Linq.Expressions.CatchBlock/*02000017*/ 
          Catch(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                class System.Linq.Expressions.Expression/*0200001F*/ body,
                class System.Linq.Expressions.Expression/*0200001F*/ 'filter') cil managed
  // SIG: 00 03 12 5C 12 59 12 7C 12 7C
  {
    .param [3]/*0800011F*/ 
    .custom /*0C000132:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x236d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Catch

  .method /*060000DF*/ public hidebysig static 
          class System.Linq.Expressions.DebugInfoExpression/*0200001A*/ 
          ClearDebugInfo(class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/ document) cil managed
  // SIG: 00 01 12 68 12 80 F0
  {
    // Method begins at RVA 0x2370
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ClearDebugInfo

  .method /*060000E0*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Coalesce(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2373
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Coalesce

  .method /*060000E1*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Coalesce(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right,
                   class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 AC
  {
    .param [3]/*08000125*/ 
    .custom /*0C000133:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2376
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Coalesce

  .method /*060000E2*/ public hidebysig static 
          class System.Linq.Expressions.ConditionalExpression/*02000018*/ 
          Condition(class System.Linq.Expressions.Expression/*0200001F*/ test,
                    class System.Linq.Expressions.Expression/*0200001F*/ ifTrue,
                    class System.Linq.Expressions.Expression/*0200001F*/ ifFalse) cil managed
  // SIG: 00 03 12 60 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x2379
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Condition

  .method /*060000E3*/ public hidebysig static 
          class System.Linq.Expressions.ConditionalExpression/*02000018*/ 
          Condition(class System.Linq.Expressions.Expression/*0200001F*/ test,
                    class System.Linq.Expressions.Expression/*0200001F*/ ifTrue,
                    class System.Linq.Expressions.Expression/*0200001F*/ ifFalse,
                    class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 04 12 60 12 7C 12 7C 12 7C 12 59
  {
    // Method begins at RVA 0x237c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Condition

  .method /*060000E4*/ public hidebysig static 
          class System.Linq.Expressions.ConstantExpression/*02000019*/ 
          Constant(object 'value') cil managed
  // SIG: 00 01 12 64 1C
  {
    .param [1]/*0800012D*/ 
    .custom /*0C000134:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x237f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Constant

  .method /*060000E5*/ public hidebysig static 
          class System.Linq.Expressions.ConstantExpression/*02000019*/ 
          Constant(object 'value',
                   class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 64 1C 12 59
  {
    .param [1]/*0800012E*/ 
    .custom /*0C000135:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2382
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Constant

  .method /*060000E6*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Continue(class System.Linq.Expressions.LabelTarget/*0200002A*/ target) cil managed
  // SIG: 00 01 12 80 8C 12 80 A8
  {
    // Method begins at RVA 0x2385
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Continue

  .method /*060000E7*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Continue(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                   class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 8C 12 80 A8 12 59
  {
    // Method begins at RVA 0x2388
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Continue

  .method /*060000E8*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Convert(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 59
  {
    // Method begins at RVA 0x238b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Convert

  .method /*060000E9*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Convert(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                  class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 80 FC 12 7C 12 59 12 80 99
  {
    .param [3]/*08000137*/ 
    .custom /*0C000136:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x238e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Convert

  .method /*060000EA*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          ConvertChecked(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                         class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 59
  {
    // Method begins at RVA 0x2391
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ConvertChecked

  .method /*060000EB*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          ConvertChecked(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                         class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                         class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 80 FC 12 7C 12 59 12 80 99
  {
    .param [3]/*0800013C*/ 
    .custom /*0C000137:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2394
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ConvertChecked

  .method /*060000EC*/ public hidebysig static 
          class System.Linq.Expressions.DebugInfoExpression/*0200001A*/ 
          DebugInfo(class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/ document,
                    int32 startLine,
                    int32 startColumn,
                    int32 endLine,
                    int32 endColumn) cil managed
  // SIG: 00 05 12 68 12 80 F0 08 08 08 08
  {
    // Method begins at RVA 0x2397
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::DebugInfo

  .method /*060000ED*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Decrement(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x239a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Decrement

  .method /*060000EE*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Decrement(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*08000144*/ 
    .custom /*0C00013B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x239d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Decrement

  .method /*060000EF*/ public hidebysig static 
          class System.Linq.Expressions.DefaultExpression/*0200001B*/ 
          Default(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 01 12 6C 12 59
  {
    // Method begins at RVA 0x23a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Default

  .method /*060000F0*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Divide(class System.Linq.Expressions.Expression/*0200001F*/ left,
                 class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x23a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Divide

  .method /*060000F1*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Divide(class System.Linq.Expressions.Expression/*0200001F*/ left,
                 class System.Linq.Expressions.Expression/*0200001F*/ right,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800014A*/ 
    .custom /*0C00013F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Divide

  .method /*060000F2*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          DivideAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                       class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x23a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::DivideAssign

  .method /*060000F3*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          DivideAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                       class System.Linq.Expressions.Expression/*0200001F*/ right,
                       class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800014F*/ 
    .custom /*0C000140:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::DivideAssign

  .method /*060000F4*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          DivideAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                       class System.Linq.Expressions.Expression/*0200001F*/ right,
                       class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                       class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000152*/ 
    .custom /*0C000141:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000153*/ 
    .custom /*0C000142:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::DivideAssign

  .method /*060000F5*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 03 12 70 12 20 12 59 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x23b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Dynamic

  .method /*060000F6*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg0) cil managed
  // SIG: 00 03 12 70 12 20 12 59 12 7C
  {
    // Method begins at RVA 0x23b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Dynamic

  .method /*060000F7*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg1) cil managed
  // SIG: 00 04 12 70 12 20 12 59 12 7C 12 7C
  {
    // Method begins at RVA 0x23b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Dynamic

  .method /*060000F8*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg2) cil managed
  // SIG: 00 05 12 70 12 20 12 59 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x23bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Dynamic

  .method /*060000F9*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg2,
                  class System.Linq.Expressions.Expression/*0200001F*/ arg3) cil managed
  // SIG: 00 06 12 70 12 20 12 59 12 7C 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x23be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Dynamic

  .method /*060000FA*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          Dynamic(class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                  class [System.Runtime/*23000001*/]System.Type/*01000016*/ returnType,
                  class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 03 12 70 12 20 12 59 1D 12 7C
  {
    .param [3]/*0800016B*/ 
    .custom /*0C000146:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Dynamic

  .method /*060000FB*/ public hidebysig static 
          class System.Linq.Expressions.ElementInit/*0200001E*/ 
          ElementInit(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ addMethod,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 02 12 78 12 80 99 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ElementInit

  .method /*060000FC*/ public hidebysig static 
          class System.Linq.Expressions.ElementInit/*0200001E*/ 
          ElementInit(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ addMethod,
                      class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 02 12 78 12 80 99 1D 12 7C
  {
    .param [2]/*0800016F*/ 
    .custom /*0C000147:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ElementInit

  .method /*060000FD*/ public hidebysig static 
          class System.Linq.Expressions.DefaultExpression/*0200001B*/ 
          Empty() cil managed
  // SIG: 00 00 12 6C
  {
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Empty

  .method /*060000FE*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Equal(class System.Linq.Expressions.Expression/*0200001F*/ left,
                class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x23cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Equal

  .method /*060000FF*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Equal(class System.Linq.Expressions.Expression/*0200001F*/ left,
                class System.Linq.Expressions.Expression/*0200001F*/ right,
                bool liftToNull,
                class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 02 12 80 99
  {
    .param [4]/*08000175*/ 
    .custom /*0C000148:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Equal

  .method /*06000100*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ExclusiveOr(class System.Linq.Expressions.Expression/*0200001F*/ left,
                      class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x23d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ExclusiveOr

  .method /*06000101*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ExclusiveOr(class System.Linq.Expressions.Expression/*0200001F*/ left,
                      class System.Linq.Expressions.Expression/*0200001F*/ right,
                      class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800017A*/ 
    .custom /*0C00014B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ExclusiveOr

  .method /*06000102*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ExclusiveOrAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                            class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x23d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ExclusiveOrAssign

  .method /*06000103*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ExclusiveOrAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                            class System.Linq.Expressions.Expression/*0200001F*/ right,
                            class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800017F*/ 
    .custom /*0C00014C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ExclusiveOrAssign

  .method /*06000104*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ExclusiveOrAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                            class System.Linq.Expressions.Expression/*0200001F*/ right,
                            class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                            class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000182*/ 
    .custom /*0C00014D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000183*/ 
    .custom /*0C00014E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ExclusiveOrAssign

  .method /*06000105*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          Field(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100002C*/ 'field') cil managed
  // SIG: 00 02 12 80 C4 12 7C 12 80 B1
  {
    .param [1]/*08000184*/ 
    .custom /*0C00014F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Field

  .method /*06000106*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          Field(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                string fieldName) cil managed
  // SIG: 00 02 12 80 C4 12 7C 0E
  {
    .custom /*0C00012A:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    // Method begins at RVA 0x23e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Field

  .method /*06000107*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          Field(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                string fieldName) cil managed
  // SIG: 00 03 12 80 C4 12 7C 12 59 0E
  {
    .param [1]/*08000188*/ 
    .custom /*0C000150:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000189*/ 
    .custom /*0C000151:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002A*/) /* 0A000025 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x23e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Field

  .method /*06000108*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          GetActionType(class [System.Runtime/*23000001*/]System.Type/*01000016*/[] typeArgs) cil managed
  // SIG: 00 01 12 59 1D 12 59
  {
    .param [1]/*0800018B*/ 
    .custom /*0C000152:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000153:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::GetActionType

  .method /*06000109*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          GetDelegateType(class [System.Runtime/*23000001*/]System.Type/*01000016*/[] typeArgs) cil managed
  // SIG: 00 01 12 59 1D 12 59
  {
    .param [1]/*0800018C*/ 
    .custom /*0C000154:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::GetDelegateType

  .method /*0600010A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          GetFuncType(class [System.Runtime/*23000001*/]System.Type/*01000016*/[] typeArgs) cil managed
  // SIG: 00 01 12 59 1D 12 59
  {
    .param [1]/*0800018D*/ 
    .custom /*0C000155:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000156:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::GetFuncType

  .method /*0600010B*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Goto(class System.Linq.Expressions.LabelTarget/*0200002A*/ target) cil managed
  // SIG: 00 01 12 80 8C 12 80 A8
  {
    // Method begins at RVA 0x23f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Goto

  .method /*0600010C*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Goto(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
               class System.Linq.Expressions.Expression/*0200001F*/ 'value') cil managed
  // SIG: 00 02 12 80 8C 12 80 A8 12 7C
  {
    .param [2]/*08000190*/ 
    .custom /*0C000157:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Goto

  .method /*0600010D*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Goto(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
               class System.Linq.Expressions.Expression/*0200001F*/ 'value',
               class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 03 12 80 8C 12 80 A8 12 7C 12 59
  {
    .param [2]/*08000192*/ 
    .custom /*0C000158:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Goto

  .method /*0600010E*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Goto(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
               class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 8C 12 80 A8 12 59
  {
    // Method begins at RVA 0x23fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Goto

  .method /*0600010F*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          GreaterThan(class System.Linq.Expressions.Expression/*0200001F*/ left,
                      class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2400
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::GreaterThan

  .method /*06000110*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          GreaterThan(class System.Linq.Expressions.Expression/*0200001F*/ left,
                      class System.Linq.Expressions.Expression/*0200001F*/ right,
                      bool liftToNull,
                      class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 02 12 80 99
  {
    .param [4]/*0800019B*/ 
    .custom /*0C00015A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2403
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::GreaterThan

  .method /*06000111*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          GreaterThanOrEqual(class System.Linq.Expressions.Expression/*0200001F*/ left,
                             class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2406
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::GreaterThanOrEqual

  .method /*06000112*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          GreaterThanOrEqual(class System.Linq.Expressions.Expression/*0200001F*/ left,
                             class System.Linq.Expressions.Expression/*0200001F*/ right,
                             bool liftToNull,
                             class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 02 12 80 99
  {
    .param [4]/*080001A1*/ 
    .custom /*0C00015E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2409
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::GreaterThanOrEqual

  .method /*06000113*/ public hidebysig static 
          class System.Linq.Expressions.ConditionalExpression/*02000018*/ 
          IfThen(class System.Linq.Expressions.Expression/*0200001F*/ test,
                 class System.Linq.Expressions.Expression/*0200001F*/ ifTrue) cil managed
  // SIG: 00 02 12 60 12 7C 12 7C
  {
    // Method begins at RVA 0x240c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::IfThen

  .method /*06000114*/ public hidebysig static 
          class System.Linq.Expressions.ConditionalExpression/*02000018*/ 
          IfThenElse(class System.Linq.Expressions.Expression/*0200001F*/ test,
                     class System.Linq.Expressions.Expression/*0200001F*/ ifTrue,
                     class System.Linq.Expressions.Expression/*0200001F*/ ifFalse) cil managed
  // SIG: 00 03 12 60 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x240f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::IfThenElse

  .method /*06000115*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Increment(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2412
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Increment

  .method /*06000116*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Increment(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080001A9*/ 
    .custom /*0C00015F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2415
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Increment

  .method /*06000117*/ public hidebysig static 
          class System.Linq.Expressions.InvocationExpression/*02000028*/ 
          Invoke(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 02 12 80 A0 12 7C 15 12 80 89 01 12 7C
  {
    .param [2]/*080001AB*/ 
    .custom /*0C000160:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2418
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Invoke

  .method /*06000118*/ public hidebysig static 
          class System.Linq.Expressions.InvocationExpression/*02000028*/ 
          Invoke(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                 class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 02 12 80 A0 12 7C 1D 12 7C
  {
    .param [2]/*080001AD*/ 
    .custom /*0C000161:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000162:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x241b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Invoke

  .method /*06000119*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          IsFalse(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x241e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::IsFalse

  .method /*0600011A*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          IsFalse(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                  class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080001B0*/ 
    .custom /*0C000163:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2421
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::IsFalse

  .method /*0600011B*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          IsTrue(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2424
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::IsTrue

  .method /*0600011C*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          IsTrue(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080001B3*/ 
    .custom /*0C000164:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2427
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::IsTrue

  .method /*0600011D*/ public hidebysig static 
          class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          Label() cil managed
  // SIG: 00 00 12 80 A8
  {
    // Method begins at RVA 0x242a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Label

  .method /*0600011E*/ public hidebysig static 
          class System.Linq.Expressions.LabelExpression/*02000029*/ 
          Label(class System.Linq.Expressions.LabelTarget/*0200002A*/ target) cil managed
  // SIG: 00 01 12 80 A4 12 80 A8
  {
    // Method begins at RVA 0x242d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Label

  .method /*0600011F*/ public hidebysig static 
          class System.Linq.Expressions.LabelExpression/*02000029*/ 
          Label(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                class System.Linq.Expressions.Expression/*0200001F*/ defaultValue) cil managed
  // SIG: 00 02 12 80 A4 12 80 A8 12 7C
  {
    .param [2]/*080001B6*/ 
    .custom /*0C000165:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2430
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Label

  .method /*06000120*/ public hidebysig static 
          class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          Label(string name) cil managed
  // SIG: 00 01 12 80 A8 0E
  {
    .param [1]/*080001B7*/ 
    .custom /*0C000166:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2433
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Label

  .method /*06000121*/ public hidebysig static 
          class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          Label(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 01 12 80 A8 12 59
  {
    // Method begins at RVA 0x2436
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Label

  .method /*06000122*/ public hidebysig static 
          class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          Label(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                string name) cil managed
  // SIG: 00 02 12 80 A8 12 59 0E
  {
    .param [2]/*080001BA*/ 
    .custom /*0C000167:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2439
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Label

  .method /*06000123*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class System.Linq.Expressions.Expression/*0200001F*/ body,
                 bool tailCall,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 00 03 12 80 AC 12 7C 02 15 12 80 89 01 12 80 E0
  {
    .param [3]/*080001BD*/ 
    .custom /*0C000168:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x243c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000124*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class System.Linq.Expressions.Expression/*0200001F*/ body,
                 bool tailCall,
                 class System.Linq.Expressions.ParameterExpression/*02000038*/[] parameters) cil managed
  // SIG: 00 03 12 80 AC 12 7C 02 1D 12 80 E0
  {
    .param [3]/*080001C0*/ 
    .custom /*0C000169:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00016A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x243f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000125*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class System.Linq.Expressions.Expression/*0200001F*/ body,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 00 02 12 80 AC 12 7C 15 12 80 89 01 12 80 E0
  {
    .param [2]/*080001C2*/ 
    .custom /*0C00016B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2442
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000126*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class System.Linq.Expressions.Expression/*0200001F*/ body,
                 class System.Linq.Expressions.ParameterExpression/*02000038*/[] parameters) cil managed
  // SIG: 00 02 12 80 AC 12 7C 1D 12 80 E0
  {
    .param [2]/*080001C4*/ 
    .custom /*0C00016E:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00016F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2445
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000127*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class System.Linq.Expressions.Expression/*0200001F*/ body,
                 string name,
                 bool tailCall,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 00 04 12 80 AC 12 7C 0E 02 15 12 80 89 01 12 80 E0
  {
    .param [2]/*080001C6*/ 
    .custom /*0C000170:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080001C8*/ 
    .custom /*0C000171:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2448
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000128*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class System.Linq.Expressions.Expression/*0200001F*/ body,
                 string name,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 00 03 12 80 AC 12 7C 0E 15 12 80 89 01 12 80 E0
  {
    .param [2]/*080001CA*/ 
    .custom /*0C000172:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001CB*/ 
    .custom /*0C000173:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x244b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000129*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                 class System.Linq.Expressions.Expression/*0200001F*/ body,
                 bool tailCall,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 00 04 12 80 AC 12 59 12 7C 02 15 12 80 89 01 12 80 E0
  {
    .param [4]/*080001CF*/ 
    .custom /*0C000174:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x244e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*0600012A*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                 class System.Linq.Expressions.Expression/*0200001F*/ body,
                 bool tailCall,
                 class System.Linq.Expressions.ParameterExpression/*02000038*/[] parameters) cil managed
  // SIG: 00 04 12 80 AC 12 59 12 7C 02 1D 12 80 E0
  {
    .param [4]/*080001D3*/ 
    .custom /*0C000175:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000176:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2451
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*0600012B*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                 class System.Linq.Expressions.Expression/*0200001F*/ body,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 00 03 12 80 AC 12 59 12 7C 15 12 80 89 01 12 80 E0
  {
    .param [3]/*080001D6*/ 
    .custom /*0C000177:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2454
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*0600012C*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                 class System.Linq.Expressions.Expression/*0200001F*/ body,
                 class System.Linq.Expressions.ParameterExpression/*02000038*/[] parameters) cil managed
  // SIG: 00 03 12 80 AC 12 59 12 7C 1D 12 80 E0
  {
    .param [3]/*080001D9*/ 
    .custom /*0C000178:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000179:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2457
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*0600012D*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                 class System.Linq.Expressions.Expression/*0200001F*/ body,
                 string name,
                 bool tailCall,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 00 05 12 80 AC 12 59 12 7C 0E 02 15 12 80 89 01 12 80 E0
  {
    .param [3]/*080001DC*/ 
    .custom /*0C00017A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080001DE*/ 
    .custom /*0C00017B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x245a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*0600012E*/ public hidebysig static 
          class System.Linq.Expressions.LambdaExpression/*0200002B*/ 
          Lambda(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                 class System.Linq.Expressions.Expression/*0200001F*/ body,
                 string name,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 00 04 12 80 AC 12 59 12 7C 0E 15 12 80 89 01 12 80 E0
  {
    .param [3]/*080001E1*/ 
    .custom /*0C00017E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080001E2*/ 
    .custom /*0C00017F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x245d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*0600012F*/ public hidebysig static 
          class System.Linq.Expressions.Expression`1/*02000022*/<!!TDelegate> 
          Lambda<TDelegate>(class System.Linq.Expressions.Expression/*0200001F*/ body,
                            bool tailCall,
                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 10 01 03 15 12 80 88 01 1E 00 12 7C 02 15 12 80 89 01 12 80 E0
  {
    .param type TDelegate /*2A00000B*/ 
      .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001E5*/ 
    .custom /*0C000180:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2460
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000130*/ public hidebysig static 
          class System.Linq.Expressions.Expression`1/*02000022*/<!!TDelegate> 
          Lambda<TDelegate>(class System.Linq.Expressions.Expression/*0200001F*/ body,
                            bool tailCall,
                            class System.Linq.Expressions.ParameterExpression/*02000038*/[] parameters) cil managed
  // SIG: 10 01 03 15 12 80 88 01 1E 00 12 7C 02 1D 12 80 E0
  {
    .param type TDelegate /*2A00000C*/ 
      .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001E8*/ 
    .custom /*0C000181:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000182:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2463
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000131*/ public hidebysig static 
          class System.Linq.Expressions.Expression`1/*02000022*/<!!TDelegate> 
          Lambda<TDelegate>(class System.Linq.Expressions.Expression/*0200001F*/ body,
                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 10 01 02 15 12 80 88 01 1E 00 12 7C 15 12 80 89 01 12 80 E0
  {
    .param type TDelegate /*2A00000D*/ 
      .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001EA*/ 
    .custom /*0C000183:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2466
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000132*/ public hidebysig static 
          class System.Linq.Expressions.Expression`1/*02000022*/<!!TDelegate> 
          Lambda<TDelegate>(class System.Linq.Expressions.Expression/*0200001F*/ body,
                            class System.Linq.Expressions.ParameterExpression/*02000038*/[] parameters) cil managed
  // SIG: 10 01 02 15 12 80 88 01 1E 00 12 7C 1D 12 80 E0
  {
    .param type TDelegate /*2A00000E*/ 
      .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001EC*/ 
    .custom /*0C000184:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000185:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2469
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000133*/ public hidebysig static 
          class System.Linq.Expressions.Expression`1/*02000022*/<!!TDelegate> 
          Lambda<TDelegate>(class System.Linq.Expressions.Expression/*0200001F*/ body,
                            string name,
                            bool tailCall,
                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 10 01 04 15 12 80 88 01 1E 00 12 7C 0E 02 15 12 80 89 01 12 80 E0
  {
    .param type TDelegate /*2A00000F*/ 
      .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001EE*/ 
    .custom /*0C000186:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080001F0*/ 
    .custom /*0C000188:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x246c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000134*/ public hidebysig static 
          class System.Linq.Expressions.Expression`1/*02000022*/<!!TDelegate> 
          Lambda<TDelegate>(class System.Linq.Expressions.Expression/*0200001F*/ body,
                            string name,
                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 10 01 03 15 12 80 88 01 1E 00 12 7C 0E 15 12 80 89 01 12 80 E0
  {
    .param type TDelegate /*2A000010*/ 
      .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080001F2*/ 
    .custom /*0C000189:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001F3*/ 
    .custom /*0C00018A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x246f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Lambda

  .method /*06000135*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LeftShift(class System.Linq.Expressions.Expression/*0200001F*/ left,
                    class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LeftShift

  .method /*06000136*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LeftShift(class System.Linq.Expressions.Expression/*0200001F*/ left,
                    class System.Linq.Expressions.Expression/*0200001F*/ right,
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080001F8*/ 
    .custom /*0C00018B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2475
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LeftShift

  .method /*06000137*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LeftShiftAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2478
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LeftShiftAssign

  .method /*06000138*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LeftShiftAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right,
                          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080001FD*/ 
    .custom /*0C00018C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x247b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LeftShiftAssign

  .method /*06000139*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LeftShiftAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right,
                          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                          class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000200*/ 
    .custom /*0C00018D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000201*/ 
    .custom /*0C00018E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x247e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LeftShiftAssign

  .method /*0600013A*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LessThan(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2481
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LessThan

  .method /*0600013B*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LessThan(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right,
                   bool liftToNull,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 02 12 80 99
  {
    .param [4]/*08000207*/ 
    .custom /*0C00018F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2484
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LessThan

  .method /*0600013C*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LessThanOrEqual(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2487
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LessThanOrEqual

  .method /*0600013D*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          LessThanOrEqual(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right,
                          bool liftToNull,
                          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 02 12 80 99
  {
    .param [4]/*0800020D*/ 
    .custom /*0C000190:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x248a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::LessThanOrEqual

  .method /*0600013E*/ public hidebysig static 
          class System.Linq.Expressions.MemberListBinding/*02000033*/ 
          ListBind(class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ member,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ElementInit/*0200001E*/> initializers) cil managed
  // SIG: 00 02 12 80 CC 12 80 A1 15 12 80 89 01 12 78
  {
    // Method begins at RVA 0x248d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListBind

  .method /*0600013F*/ public hidebysig static 
          class System.Linq.Expressions.MemberListBinding/*02000033*/ 
          ListBind(class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ member,
                   class System.Linq.Expressions.ElementInit/*0200001E*/[] initializers) cil managed
  // SIG: 00 02 12 80 CC 12 80 A1 1D 12 78
  {
    .param [2]/*08000211*/ 
    .custom /*0C000192:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2490
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListBind

  .method /*06000140*/ public hidebysig static 
          class System.Linq.Expressions.MemberListBinding/*02000033*/ 
          ListBind(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ propertyAccessor,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ElementInit/*0200001E*/> initializers) cil managed
  // SIG: 00 02 12 80 CC 12 80 99 15 12 80 89 01 12 78
  {
    .custom /*0C000138:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 37 54 68 65 20 50 72 6F 70 65 72 74 79 20   // ..7The Property 
                                                                                                                                                                                            6D 65 74 61 64 61 74 61 20 6F 72 20 6F 74 68 65   // metadata or othe
                                                                                                                                                                                            72 20 61 63 63 65 73 73 6F 72 20 6D 61 79 20 62   // r accessor may b
                                                                                                                                                                                            65 20 74 72 69 6D 6D 65 64 2E 00 00 )             // e trimmed...
    // Method begins at RVA 0x2493
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListBind

  .method /*06000141*/ public hidebysig static 
          class System.Linq.Expressions.MemberListBinding/*02000033*/ 
          ListBind(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ propertyAccessor,
                   class System.Linq.Expressions.ElementInit/*0200001E*/[] initializers) cil managed
  // SIG: 00 02 12 80 CC 12 80 99 1D 12 78
  {
    .custom /*0C000139:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 37 54 68 65 20 50 72 6F 70 65 72 74 79 20   // ..7The Property 
                                                                                                                                                                                            6D 65 74 61 64 61 74 61 20 6F 72 20 6F 74 68 65   // metadata or othe
                                                                                                                                                                                            72 20 61 63 63 65 73 73 6F 72 20 6D 61 79 20 62   // r accessor may b
                                                                                                                                                                                            65 20 74 72 69 6D 6D 65 64 2E 00 00 )             // e trimmed...
    .param [2]/*08000215*/ 
    .custom /*0C000193:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2496
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListBind

  .method /*06000142*/ public hidebysig static 
          class System.Linq.Expressions.ListInitExpression/*0200002C*/ 
          ListInit(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ElementInit/*0200001E*/> initializers) cil managed
  // SIG: 00 02 12 80 B0 12 80 DC 15 12 80 89 01 12 78
  {
    // Method begins at RVA 0x2499
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListInit

  .method /*06000143*/ public hidebysig static 
          class System.Linq.Expressions.ListInitExpression/*0200002C*/ 
          ListInit(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> initializers) cil managed
  // SIG: 00 02 12 80 B0 12 80 DC 15 12 80 89 01 12 7C
  {
    .custom /*0C00013A:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    // Method begins at RVA 0x249c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListInit

  .method /*06000144*/ public hidebysig static 
          class System.Linq.Expressions.ListInitExpression/*0200002C*/ 
          ListInit(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                   class System.Linq.Expressions.ElementInit/*0200001E*/[] initializers) cil managed
  // SIG: 00 02 12 80 B0 12 80 DC 1D 12 78
  {
    .param [2]/*0800021B*/ 
    .custom /*0C000195:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x249f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListInit

  .method /*06000145*/ public hidebysig static 
          class System.Linq.Expressions.ListInitExpression/*0200002C*/ 
          ListInit(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                   class System.Linq.Expressions.Expression/*0200001F*/[] initializers) cil managed
  // SIG: 00 02 12 80 B0 12 80 DC 1D 12 7C
  {
    .custom /*0C00013C:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    .param [2]/*0800021D*/ 
    .custom /*0C000197:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListInit

  .method /*06000146*/ public hidebysig static 
          class System.Linq.Expressions.ListInitExpression/*0200002C*/ 
          ListInit(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ addMethod,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> initializers) cil managed
  // SIG: 00 03 12 80 B0 12 80 DC 12 80 99 15 12 80 89 01 12 7C
  {
    .custom /*0C00013D:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    .param [2]/*0800021F*/ 
    .custom /*0C000198:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListInit

  .method /*06000147*/ public hidebysig static 
          class System.Linq.Expressions.ListInitExpression/*0200002C*/ 
          ListInit(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ addMethod,
                   class System.Linq.Expressions.Expression/*0200001F*/[] initializers) cil managed
  // SIG: 00 03 12 80 B0 12 80 DC 12 80 99 1D 12 7C
  {
    .custom /*0C00013E:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    .param [2]/*08000222*/ 
    .custom /*0C000199:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000223*/ 
    .custom /*0C00019A:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ListInit

  .method /*06000148*/ public hidebysig static 
          class System.Linq.Expressions.LoopExpression/*0200002D*/ 
          Loop(class System.Linq.Expressions.Expression/*0200001F*/ body) cil managed
  // SIG: 00 01 12 80 B4 12 7C
  {
    // Method begins at RVA 0x24ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Loop

  .method /*06000149*/ public hidebysig static 
          class System.Linq.Expressions.LoopExpression/*0200002D*/ 
          Loop(class System.Linq.Expressions.Expression/*0200001F*/ body,
               class System.Linq.Expressions.LabelTarget/*0200002A*/ 'break') cil managed
  // SIG: 00 02 12 80 B4 12 7C 12 80 A8
  {
    .param [2]/*08000226*/ 
    .custom /*0C00019B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Loop

  .method /*0600014A*/ public hidebysig static 
          class System.Linq.Expressions.LoopExpression/*0200002D*/ 
          Loop(class System.Linq.Expressions.Expression/*0200001F*/ body,
               class System.Linq.Expressions.LabelTarget/*0200002A*/ 'break',
               class System.Linq.Expressions.LabelTarget/*0200002A*/ continue) cil managed
  // SIG: 00 03 12 80 B4 12 7C 12 80 A8 12 80 A8
  {
    .param [2]/*08000228*/ 
    .custom /*0C00019C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000229*/ 
    .custom /*0C00019D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Loop

  .method /*0600014B*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MakeBinary(valuetype System.Linq.Expressions.ExpressionType/*02000020*/ binaryType,
                     class System.Linq.Expressions.Expression/*0200001F*/ left,
                     class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 03 12 54 11 80 80 12 7C 12 7C
  {
    // Method begins at RVA 0x24b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeBinary

  .method /*0600014C*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MakeBinary(valuetype System.Linq.Expressions.ExpressionType/*02000020*/ binaryType,
                     class System.Linq.Expressions.Expression/*0200001F*/ left,
                     class System.Linq.Expressions.Expression/*0200001F*/ right,
                     bool liftToNull,
                     class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 05 12 54 11 80 80 12 7C 12 7C 02 12 80 99
  {
    .param [5]/*08000231*/ 
    .custom /*0C00019F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeBinary

  .method /*0600014D*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MakeBinary(valuetype System.Linq.Expressions.ExpressionType/*02000020*/ binaryType,
                     class System.Linq.Expressions.Expression/*0200001F*/ left,
                     class System.Linq.Expressions.Expression/*0200001F*/ right,
                     bool liftToNull,
                     class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                     class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 06 12 54 11 80 80 12 7C 12 7C 02 12 80 99 12 80 AC
  {
    .param [5]/*08000236*/ 
    .custom /*0C0001A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000237*/ 
    .custom /*0C0001A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeBinary

  .method /*0600014E*/ public hidebysig static 
          class System.Linq.Expressions.CatchBlock/*02000017*/ 
          MakeCatchBlock(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                         class System.Linq.Expressions.ParameterExpression/*02000038*/ variable,
                         class System.Linq.Expressions.Expression/*0200001F*/ body,
                         class System.Linq.Expressions.Expression/*0200001F*/ 'filter') cil managed
  // SIG: 00 04 12 5C 12 59 12 80 E0 12 7C 12 7C
  {
    .param [2]/*08000239*/ 
    .custom /*0C0001A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800023B*/ 
    .custom /*0C0001A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeCatchBlock

  .method /*0600014F*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 03 12 70 12 59 12 20 15 12 80 89 01 12 7C
  {
    .param [3]/*0800023E*/ 
    .custom /*0C0001A6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeDynamic

  .method /*06000150*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg0) cil managed
  // SIG: 00 03 12 70 12 59 12 20 12 7C
  {
    // Method begins at RVA 0x24c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeDynamic

  .method /*06000151*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg1) cil managed
  // SIG: 00 04 12 70 12 59 12 20 12 7C 12 7C
  {
    // Method begins at RVA 0x24c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeDynamic

  .method /*06000152*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg2) cil managed
  // SIG: 00 05 12 70 12 59 12 20 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x24c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeDynamic

  .method /*06000153*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg0,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg1,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg2,
                      class System.Linq.Expressions.Expression/*0200001F*/ arg3) cil managed
  // SIG: 00 06 12 70 12 59 12 20 12 7C 12 7C 12 7C 12 7C
  {
    // Method begins at RVA 0x24cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeDynamic

  .method /*06000154*/ public hidebysig static 
          class System.Linq.Expressions.DynamicExpression/*0200001C*/ 
          MakeDynamic(class [System.Runtime/*23000001*/]System.Type/*01000016*/ delegateType,
                      class System.Runtime.CompilerServices.CallSiteBinder/*02000008*/ binder,
                      class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 03 12 70 12 59 12 20 1D 12 7C
  {
    .param [3]/*08000253*/ 
    .custom /*0C0001A9:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C0001AA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeDynamic

  .method /*06000155*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          MakeGoto(valuetype System.Linq.Expressions.GotoExpressionKind/*02000024*/ kind,
                   class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                   class System.Linq.Expressions.Expression/*0200001F*/ 'value',
                   class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 04 12 80 8C 11 80 90 12 80 A8 12 7C 12 59
  {
    .param [3]/*08000256*/ 
    .custom /*0C0001AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeGoto

  .method /*06000156*/ public hidebysig static 
          class System.Linq.Expressions.IndexExpression/*02000027*/ 
          MakeIndex(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
                    class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100002D*/ indexer,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 03 12 80 9C 12 7C 12 80 B5 15 12 80 89 01 12 7C
  {
    .param [2]/*08000259*/ 
    .custom /*0C0001AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800025A*/ 
    .custom /*0C0001AF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeIndex

  .method /*06000157*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          MakeMemberAccess(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                           class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ member) cil managed
  // SIG: 00 02 12 80 C4 12 7C 12 80 A1
  {
    .param [1]/*0800025B*/ 
    .custom /*0C0001B0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeMemberAccess

  .method /*06000158*/ public hidebysig static 
          class System.Linq.Expressions.TryExpression/*0200003D*/ 
          MakeTry(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                  class System.Linq.Expressions.Expression/*0200001F*/ body,
                  class System.Linq.Expressions.Expression/*0200001F*/ 'finally',
                  class System.Linq.Expressions.Expression/*0200001F*/ 'fault',
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.CatchBlock/*02000017*/> handlers) cil managed
  // SIG: 00 05 12 80 F4 12 59 12 7C 12 7C 12 7C 15 12 80 89 01 12 5C
  {
    .custom /*0C000143:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800025D*/ 
    .custom /*0C0001B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800025F*/ 
    .custom /*0C0001B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000262*/ 
    .custom /*0C0001B3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeTry

  .method /*06000159*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          MakeUnary(valuetype System.Linq.Expressions.ExpressionType/*02000020*/ unaryType,
                    class System.Linq.Expressions.Expression/*0200001F*/ operand,
                    class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 03 12 80 FC 11 80 80 12 7C 12 59
  {
    // Method begins at RVA 0x24de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeUnary

  .method /*0600015A*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          MakeUnary(valuetype System.Linq.Expressions.ExpressionType/*02000020*/ unaryType,
                    class System.Linq.Expressions.Expression/*0200001F*/ operand,
                    class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 04 12 80 FC 11 80 80 12 7C 12 59 12 80 99
  {
    .param [4]/*08000269*/ 
    .custom /*0C0001B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MakeUnary

  .method /*0600015B*/ public hidebysig static 
          class System.Linq.Expressions.MemberMemberBinding/*02000034*/ 
          MemberBind(class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ member,
                     class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> bindings) cil managed
  // SIG: 00 02 12 80 D0 12 80 A1 15 12 80 89 01 12 80 BC
  {
    // Method begins at RVA 0x24e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MemberBind

  .method /*0600015C*/ public hidebysig static 
          class System.Linq.Expressions.MemberMemberBinding/*02000034*/ 
          MemberBind(class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ member,
                     class System.Linq.Expressions.MemberBinding/*0200002F*/[] bindings) cil managed
  // SIG: 00 02 12 80 D0 12 80 A1 1D 12 80 BC
  {
    .param [2]/*0800026D*/ 
    .custom /*0C0001B5:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MemberBind

  .method /*0600015D*/ public hidebysig static 
          class System.Linq.Expressions.MemberMemberBinding/*02000034*/ 
          MemberBind(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ propertyAccessor,
                     class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> bindings) cil managed
  // SIG: 00 02 12 80 D0 12 80 99 15 12 80 89 01 12 80 BC
  {
    .custom /*0C000144:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 37 54 68 65 20 50 72 6F 70 65 72 74 79 20   // ..7The Property 
                                                                                                                                                                                            6D 65 74 61 64 61 74 61 20 6F 72 20 6F 74 68 65   // metadata or othe
                                                                                                                                                                                            72 20 61 63 63 65 73 73 6F 72 20 6D 61 79 20 62   // r accessor may b
                                                                                                                                                                                            65 20 74 72 69 6D 6D 65 64 2E 00 00 )             // e trimmed...
    // Method begins at RVA 0x24ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MemberBind

  .method /*0600015E*/ public hidebysig static 
          class System.Linq.Expressions.MemberMemberBinding/*02000034*/ 
          MemberBind(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ propertyAccessor,
                     class System.Linq.Expressions.MemberBinding/*0200002F*/[] bindings) cil managed
  // SIG: 00 02 12 80 D0 12 80 99 1D 12 80 BC
  {
    .custom /*0C000145:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 37 54 68 65 20 50 72 6F 70 65 72 74 79 20   // ..7The Property 
                                                                                                                                                                                            6D 65 74 61 64 61 74 61 20 6F 72 20 6F 74 68 65   // metadata or othe
                                                                                                                                                                                            72 20 61 63 63 65 73 73 6F 72 20 6D 61 79 20 62   // r accessor may b
                                                                                                                                                                                            65 20 74 72 69 6D 6D 65 64 2E 00 00 )             // e trimmed...
    .param [2]/*08000271*/ 
    .custom /*0C0001B6:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MemberBind

  .method /*0600015F*/ public hidebysig static 
          class System.Linq.Expressions.MemberInitExpression/*02000032*/ 
          MemberInit(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                     class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> bindings) cil managed
  // SIG: 00 02 12 80 C8 12 80 DC 15 12 80 89 01 12 80 BC
  {
    // Method begins at RVA 0x24f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MemberInit

  .method /*06000160*/ public hidebysig static 
          class System.Linq.Expressions.MemberInitExpression/*02000032*/ 
          MemberInit(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                     class System.Linq.Expressions.MemberBinding/*0200002F*/[] bindings) cil managed
  // SIG: 00 02 12 80 C8 12 80 DC 1D 12 80 BC
  {
    .param [2]/*08000275*/ 
    .custom /*0C0001B8:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MemberInit

  .method /*06000161*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Modulo(class System.Linq.Expressions.Expression/*0200001F*/ left,
                 class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x24f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Modulo

  .method /*06000162*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Modulo(class System.Linq.Expressions.Expression/*0200001F*/ left,
                 class System.Linq.Expressions.Expression/*0200001F*/ right,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800027A*/ 
    .custom /*0C0001B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Modulo

  .method /*06000163*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ModuloAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                       class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x24fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ModuloAssign

  .method /*06000164*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ModuloAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                       class System.Linq.Expressions.Expression/*0200001F*/ right,
                       class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800027F*/ 
    .custom /*0C0001BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ModuloAssign

  .method /*06000165*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ModuloAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                       class System.Linq.Expressions.Expression/*0200001F*/ right,
                       class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                       class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000282*/ 
    .custom /*0C0001BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000283*/ 
    .custom /*0C0001BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2502
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ModuloAssign

  .method /*06000166*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Multiply(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2505
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Multiply

  .method /*06000167*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Multiply(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*08000288*/ 
    .custom /*0C0001BE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2508
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Multiply

  .method /*06000168*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MultiplyAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                         class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x250b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MultiplyAssign

  .method /*06000169*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MultiplyAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                         class System.Linq.Expressions.Expression/*0200001F*/ right,
                         class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800028D*/ 
    .custom /*0C0001C0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x250e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MultiplyAssign

  .method /*0600016A*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MultiplyAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                         class System.Linq.Expressions.Expression/*0200001F*/ right,
                         class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                         class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000290*/ 
    .custom /*0C0001C1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000291*/ 
    .custom /*0C0001C2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2511
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MultiplyAssign

  .method /*0600016B*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MultiplyAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                                class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2514
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MultiplyAssignChecked

  .method /*0600016C*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MultiplyAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                                class System.Linq.Expressions.Expression/*0200001F*/ right,
                                class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*08000296*/ 
    .custom /*0C0001C3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2517
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MultiplyAssignChecked

  .method /*0600016D*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MultiplyAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                                class System.Linq.Expressions.Expression/*0200001F*/ right,
                                class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                                class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000299*/ 
    .custom /*0C0001C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800029A*/ 
    .custom /*0C0001C6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x251a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MultiplyAssignChecked

  .method /*0600016E*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MultiplyChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x251d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MultiplyChecked

  .method /*0600016F*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          MultiplyChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right,
                          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800029F*/ 
    .custom /*0C0001C8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2520
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::MultiplyChecked

  .method /*06000170*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Negate(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2523
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Negate

  .method /*06000171*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Negate(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080002A2*/ 
    .custom /*0C0001C9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2526
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Negate

  .method /*06000172*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          NegateChecked(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2529
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::NegateChecked

  .method /*06000173*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          NegateChecked(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                        class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080002A5*/ 
    .custom /*0C0001CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x252c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::NegateChecked

  .method /*06000174*/ public hidebysig static 
          class System.Linq.Expressions.NewExpression/*02000037*/ 
          New(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100002E*/ constructor) cil managed
  // SIG: 00 01 12 80 DC 12 80 B9
  {
    // Method begins at RVA 0x252f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::New

  .method /*06000175*/ public hidebysig static 
          class System.Linq.Expressions.NewExpression/*02000037*/ 
          New(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100002E*/ constructor,
              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 02 12 80 DC 12 80 B9 15 12 80 89 01 12 7C
  {
    .param [2]/*080002A8*/ 
    .custom /*0C0001CC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2532
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::New

  .method /*06000176*/ public hidebysig static 
          class System.Linq.Expressions.NewExpression/*02000037*/ 
          New(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100002E*/ constructor,
              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments,
              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/> members) cil managed
  // SIG: 00 03 12 80 DC 12 80 B9 15 12 80 89 01 12 7C 15 12 80 89 01 12 80 A1
  {
    .custom /*0C000149:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 37 54 68 65 20 50 72 6F 70 65 72 74 79 20   // ..7The Property 
                                                                                                                                                                                            6D 65 74 61 64 61 74 61 20 6F 72 20 6F 74 68 65   // metadata or othe
                                                                                                                                                                                            72 20 61 63 63 65 73 73 6F 72 20 6D 61 79 20 62   // r accessor may b
                                                                                                                                                                                            65 20 74 72 69 6D 6D 65 64 2E 00 00 )             // e trimmed...
    .param [2]/*080002AA*/ 
    .custom /*0C0001CE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080002AB*/ 
    .custom /*0C0001CF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2535
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::New

  .method /*06000177*/ public hidebysig static 
          class System.Linq.Expressions.NewExpression/*02000037*/ 
          New(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100002E*/ constructor,
              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments,
              class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/[] members) cil managed
  // SIG: 00 03 12 80 DC 12 80 B9 15 12 80 89 01 12 7C 1D 12 80 A1
  {
    .custom /*0C00014A:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 37 54 68 65 20 50 72 6F 70 65 72 74 79 20   // ..7The Property 
                                                                                                                                                                                            6D 65 74 61 64 61 74 61 20 6F 72 20 6F 74 68 65   // metadata or othe
                                                                                                                                                                                            72 20 61 63 63 65 73 73 6F 72 20 6D 61 79 20 62   // r accessor may b
                                                                                                                                                                                            65 20 74 72 69 6D 6D 65 64 2E 00 00 )             // e trimmed...
    .param [2]/*080002AD*/ 
    .custom /*0C0001D0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080002AE*/ 
    .custom /*0C0001D1:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2538
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::New

  .method /*06000178*/ public hidebysig static 
          class System.Linq.Expressions.NewExpression/*02000037*/ 
          New(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100002E*/ constructor,
              class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 02 12 80 DC 12 80 B9 1D 12 7C
  {
    .param [2]/*080002B0*/ 
    .custom /*0C0001D3:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x253b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::New

  .method /*06000179*/ public hidebysig static 
          class System.Linq.Expressions.NewExpression/*02000037*/ 
          New(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 01 12 80 DC 12 59
  {
    .param [1]/*080002B1*/ 
    .custom /*0C0001D5:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002A*/) /* 0A000025 */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x253e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::New

  .method /*0600017A*/ public hidebysig static 
          class System.Linq.Expressions.NewArrayExpression/*02000036*/ 
          NewArrayBounds(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> bounds) cil managed
  // SIG: 00 02 12 80 D8 12 59 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x2541
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::NewArrayBounds

  .method /*0600017B*/ public hidebysig static 
          class System.Linq.Expressions.NewArrayExpression/*02000036*/ 
          NewArrayBounds(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                         class System.Linq.Expressions.Expression/*0200001F*/[] bounds) cil managed
  // SIG: 00 02 12 80 D8 12 59 1D 12 7C
  {
    .param [2]/*080002B5*/ 
    .custom /*0C0001D6:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2544
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::NewArrayBounds

  .method /*0600017C*/ public hidebysig static 
          class System.Linq.Expressions.NewArrayExpression/*02000036*/ 
          NewArrayInit(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> initializers) cil managed
  // SIG: 00 02 12 80 D8 12 59 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x2547
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::NewArrayInit

  .method /*0600017D*/ public hidebysig static 
          class System.Linq.Expressions.NewArrayExpression/*02000036*/ 
          NewArrayInit(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                       class System.Linq.Expressions.Expression/*0200001F*/[] initializers) cil managed
  // SIG: 00 02 12 80 D8 12 59 1D 12 7C
  {
    .param [2]/*080002B9*/ 
    .custom /*0C0001D7:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x254a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::NewArrayInit

  .method /*0600017E*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Not(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x254d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Not

  .method /*0600017F*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Not(class System.Linq.Expressions.Expression/*0200001F*/ expression,
              class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080002BC*/ 
    .custom /*0C0001D9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2550
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Not

  .method /*06000180*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          NotEqual(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2553
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::NotEqual

  .method /*06000181*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          NotEqual(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right,
                   bool liftToNull,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 02 12 80 99
  {
    .param [4]/*080002C2*/ 
    .custom /*0C0001DA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2556
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::NotEqual

  .method /*06000182*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          OnesComplement(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2559
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::OnesComplement

  .method /*06000183*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          OnesComplement(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                         class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080002C5*/ 
    .custom /*0C0001DB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x255c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::OnesComplement

  .method /*06000184*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Or(class System.Linq.Expressions.Expression/*0200001F*/ left,
             class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x255f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Or

  .method /*06000185*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Or(class System.Linq.Expressions.Expression/*0200001F*/ left,
             class System.Linq.Expressions.Expression/*0200001F*/ right,
             class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080002CA*/ 
    .custom /*0C0001DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2562
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Or

  .method /*06000186*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          OrAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2565
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::OrAssign

  .method /*06000187*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          OrAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080002CF*/ 
    .custom /*0C0001DD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2568
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::OrAssign

  .method /*06000188*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          OrAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                   class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*080002D2*/ 
    .custom /*0C0001DF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080002D3*/ 
    .custom /*0C0001E0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x256b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::OrAssign

  .method /*06000189*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          OrElse(class System.Linq.Expressions.Expression/*0200001F*/ left,
                 class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x256e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::OrElse

  .method /*0600018A*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          OrElse(class System.Linq.Expressions.Expression/*0200001F*/ left,
                 class System.Linq.Expressions.Expression/*0200001F*/ right,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080002D8*/ 
    .custom /*0C0001E1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2571
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::OrElse

  .method /*0600018B*/ public hidebysig static 
          class System.Linq.Expressions.ParameterExpression/*02000038*/ 
          Parameter(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 01 12 80 E0 12 59
  {
    // Method begins at RVA 0x2574
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Parameter

  .method /*0600018C*/ public hidebysig static 
          class System.Linq.Expressions.ParameterExpression/*02000038*/ 
          Parameter(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                    string name) cil managed
  // SIG: 00 02 12 80 E0 12 59 0E
  {
    .param [2]/*080002DB*/ 
    .custom /*0C0001E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2577
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Parameter

  .method /*0600018D*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          PostDecrementAssign(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x257a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PostDecrementAssign

  .method /*0600018E*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          PostDecrementAssign(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                              class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080002DE*/ 
    .custom /*0C0001E3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x257d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PostDecrementAssign

  .method /*0600018F*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          PostIncrementAssign(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2580
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PostIncrementAssign

  .method /*06000190*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          PostIncrementAssign(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                              class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080002E1*/ 
    .custom /*0C0001E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2583
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PostIncrementAssign

  .method /*06000191*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Power(class System.Linq.Expressions.Expression/*0200001F*/ left,
                class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2586
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Power

  .method /*06000192*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Power(class System.Linq.Expressions.Expression/*0200001F*/ left,
                class System.Linq.Expressions.Expression/*0200001F*/ right,
                class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080002E6*/ 
    .custom /*0C0001E5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2589
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Power

  .method /*06000193*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          PowerAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                      class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x258c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PowerAssign

  .method /*06000194*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          PowerAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                      class System.Linq.Expressions.Expression/*0200001F*/ right,
                      class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*080002EB*/ 
    .custom /*0C0001E6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x258f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PowerAssign

  .method /*06000195*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          PowerAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                      class System.Linq.Expressions.Expression/*0200001F*/ right,
                      class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                      class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*080002EE*/ 
    .custom /*0C0001E7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080002EF*/ 
    .custom /*0C0001E8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2592
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PowerAssign

  .method /*06000196*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          PreDecrementAssign(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2595
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PreDecrementAssign

  .method /*06000197*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          PreDecrementAssign(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                             class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080002F2*/ 
    .custom /*0C0001E9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2598
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PreDecrementAssign

  .method /*06000198*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          PreIncrementAssign(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x259b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PreIncrementAssign

  .method /*06000199*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          PreIncrementAssign(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                             class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*080002F5*/ 
    .custom /*0C0001EC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x259e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PreIncrementAssign

  .method /*0600019A*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          Property(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ propertyAccessor) cil managed
  // SIG: 00 02 12 80 C4 12 7C 12 80 99
  {
    .custom /*0C000159:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 37 54 68 65 20 50 72 6F 70 65 72 74 79 20   // ..7The Property 
                                                                                                                                                                                            6D 65 74 61 64 61 74 61 20 6F 72 20 6F 74 68 65   // metadata or othe
                                                                                                                                                                                            72 20 61 63 63 65 73 73 6F 72 20 6D 61 79 20 62   // r accessor may b
                                                                                                                                                                                            65 20 74 72 69 6D 6D 65 64 2E 00 00 )             // e trimmed...
    .param [1]/*080002F6*/ 
    .custom /*0C0001ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Property

  .method /*0600019B*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          Property(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                   class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100002D*/ 'property') cil managed
  // SIG: 00 02 12 80 C4 12 7C 12 80 B5
  {
    .param [1]/*080002F8*/ 
    .custom /*0C0001EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Property

  .method /*0600019C*/ public hidebysig static 
          class System.Linq.Expressions.IndexExpression/*02000027*/ 
          Property(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
                   class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100002D*/ indexer,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 00 03 12 80 9C 12 7C 12 80 B5 15 12 80 89 01 12 7C
  {
    .param [1]/*080002FA*/ 
    .custom /*0C0001EF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080002FC*/ 
    .custom /*0C0001F0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x25a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Property

  .method /*0600019D*/ public hidebysig static 
          class System.Linq.Expressions.IndexExpression/*02000027*/ 
          Property(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
                   class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100002D*/ indexer,
                   class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 03 12 80 9C 12 7C 12 80 B5 1D 12 7C
  {
    .param [1]/*080002FD*/ 
    .custom /*0C0001F1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080002FF*/ 
    .custom /*0C0001F2:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C0001F3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x25aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Property

  .method /*0600019E*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          Property(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                   string propertyName) cil managed
  // SIG: 00 02 12 80 C4 12 7C 0E
  {
    .custom /*0C00015B:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    // Method begins at RVA 0x25ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Property

  .method /*0600019F*/ public hidebysig static 
          class System.Linq.Expressions.IndexExpression/*02000027*/ 
          Property(class System.Linq.Expressions.Expression/*0200001F*/ 'instance',
                   string propertyName,
                   class System.Linq.Expressions.Expression/*0200001F*/[] arguments) cil managed
  // SIG: 00 03 12 80 9C 12 7C 0E 1D 12 7C
  {
    .custom /*0C00015C:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    .param [3]/*08000304*/ 
    .custom /*0C0001F4:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C0001F5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x25b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Property

  .method /*060001A0*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          Property(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                   class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                   string propertyName) cil managed
  // SIG: 00 03 12 80 C4 12 7C 12 59 0E
  {
    .param [1]/*08000305*/ 
    .custom /*0C0001F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000306*/ 
    .custom /*0C0001F7:0A000025*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002A*/) /* 0A000025 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x25b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Property

  .method /*060001A1*/ public hidebysig static 
          class System.Linq.Expressions.MemberExpression/*02000031*/ 
          PropertyOrField(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                          string propertyOrFieldName) cil managed
  // SIG: 00 02 12 80 C4 12 7C 0E
  {
    .custom /*0C00015D:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000029*/::.ctor(string) /* 0A000024 */ = ( 01 00 76 43 72 65 61 74 69 6E 67 20 45 78 70 72   // ..vCreating Expr
                                                                                                                                                                                            65 73 73 69 6F 6E 73 20 72 65 71 75 69 72 65 73   // essions requires
                                                                                                                                                                                            20 75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F   //  unreferenced co
                                                                                                                                                                                            64 65 20 62 65 63 61 75 73 65 20 74 68 65 20 6D   // de because the m
                                                                                                                                                                                            65 6D 62 65 72 73 20 62 65 69 6E 67 20 72 65 66   // embers being ref
                                                                                                                                                                                            65 72 65 6E 63 65 64 20 62 79 20 74 68 65 20 45   // erenced by the E
                                                                                                                                                                                            78 70 72 65 73 73 69 6F 6E 20 6D 61 79 20 62 65   // xpression may be
                                                                                                                                                                                            20 74 72 69 6D 6D 65 64 2E 00 00 )                //  trimmed...
    // Method begins at RVA 0x25b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::PropertyOrField

  .method /*060001A2*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Quote(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x25b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Quote

  .method /*060001A3*/ public hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Reduce() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x25bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Reduce

  .method /*060001A4*/ public hidebysig instance class System.Linq.Expressions.Expression/*0200001F*/ 
          ReduceAndCheck() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x25bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ReduceAndCheck

  .method /*060001A5*/ public hidebysig instance class System.Linq.Expressions.Expression/*0200001F*/ 
          ReduceExtensions() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x25c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ReduceExtensions

  .method /*060001A6*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ReferenceEqual(class System.Linq.Expressions.Expression/*0200001F*/ left,
                         class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x25c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ReferenceEqual

  .method /*060001A7*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          ReferenceNotEqual(class System.Linq.Expressions.Expression/*0200001F*/ left,
                            class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x25c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ReferenceNotEqual

  .method /*060001A8*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Rethrow() cil managed
  // SIG: 00 00 12 80 FC
  {
    // Method begins at RVA 0x25cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Rethrow

  .method /*060001A9*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Rethrow(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 01 12 80 FC 12 59
  {
    // Method begins at RVA 0x25ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Rethrow

  .method /*060001AA*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Return(class System.Linq.Expressions.LabelTarget/*0200002A*/ target) cil managed
  // SIG: 00 01 12 80 8C 12 80 A8
  {
    // Method begins at RVA 0x25d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Return

  .method /*060001AB*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Return(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                 class System.Linq.Expressions.Expression/*0200001F*/ 'value') cil managed
  // SIG: 00 02 12 80 8C 12 80 A8 12 7C
  {
    .param [2]/*08000312*/ 
    .custom /*0C0001F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Return

  .method /*060001AC*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Return(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                 class System.Linq.Expressions.Expression/*0200001F*/ 'value',
                 class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 03 12 80 8C 12 80 A8 12 7C 12 59
  {
    .param [2]/*08000314*/ 
    .custom /*0C0001FA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Return

  .method /*060001AD*/ public hidebysig static 
          class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Return(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                 class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 8C 12 80 A8 12 59
  {
    // Method begins at RVA 0x25da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Return

  .method /*060001AE*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          RightShift(class System.Linq.Expressions.Expression/*0200001F*/ left,
                     class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x25dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::RightShift

  .method /*060001AF*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          RightShift(class System.Linq.Expressions.Expression/*0200001F*/ left,
                     class System.Linq.Expressions.Expression/*0200001F*/ right,
                     class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800031C*/ 
    .custom /*0C0001FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::RightShift

  .method /*060001B0*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          RightShiftAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                           class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x25e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::RightShiftAssign

  .method /*060001B1*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          RightShiftAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                           class System.Linq.Expressions.Expression/*0200001F*/ right,
                           class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*08000321*/ 
    .custom /*0C0001FE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::RightShiftAssign

  .method /*060001B2*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          RightShiftAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                           class System.Linq.Expressions.Expression/*0200001F*/ right,
                           class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                           class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000324*/ 
    .custom /*0C0001FF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000325*/ 
    .custom /*0C000200:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::RightShiftAssign

  .method /*060001B3*/ public hidebysig static 
          class System.Linq.Expressions.RuntimeVariablesExpression/*02000039*/ 
          RuntimeVariables(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> variables) cil managed
  // SIG: 00 01 12 80 E4 15 12 80 89 01 12 80 E0
  {
    // Method begins at RVA 0x25ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::RuntimeVariables

  .method /*060001B4*/ public hidebysig static 
          class System.Linq.Expressions.RuntimeVariablesExpression/*02000039*/ 
          RuntimeVariables(class System.Linq.Expressions.ParameterExpression/*02000038*/[] variables) cil managed
  // SIG: 00 01 12 80 E4 1D 12 80 E0
  {
    .param [1]/*08000327*/ 
    .custom /*0C000201:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::RuntimeVariables

  .method /*060001B5*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Subtract(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x25f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Subtract

  .method /*060001B6*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          Subtract(class System.Linq.Expressions.Expression/*0200001F*/ left,
                   class System.Linq.Expressions.Expression/*0200001F*/ right,
                   class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800032C*/ 
    .custom /*0C000202:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Subtract

  .method /*060001B7*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          SubtractAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                         class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x25f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SubtractAssign

  .method /*060001B8*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          SubtractAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                         class System.Linq.Expressions.Expression/*0200001F*/ right,
                         class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*08000331*/ 
    .custom /*0C000203:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SubtractAssign

  .method /*060001B9*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          SubtractAssign(class System.Linq.Expressions.Expression/*0200001F*/ left,
                         class System.Linq.Expressions.Expression/*0200001F*/ right,
                         class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                         class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*08000334*/ 
    .custom /*0C000204:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000335*/ 
    .custom /*0C000205:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SubtractAssign

  .method /*060001BA*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          SubtractAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                                class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x2601
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SubtractAssignChecked

  .method /*060001BB*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          SubtractAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                                class System.Linq.Expressions.Expression/*0200001F*/ right,
                                class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*0800033A*/ 
    .custom /*0C000206:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2604
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SubtractAssignChecked

  .method /*060001BC*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          SubtractAssignChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                                class System.Linq.Expressions.Expression/*0200001F*/ right,
                                class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method',
                                class System.Linq.Expressions.LambdaExpression/*0200002B*/ conversion) cil managed
  // SIG: 00 04 12 54 12 7C 12 7C 12 80 99 12 80 AC
  {
    .param [3]/*0800033D*/ 
    .custom /*0C000207:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800033E*/ 
    .custom /*0C000208:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2607
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SubtractAssignChecked

  .method /*060001BD*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          SubtractChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right) cil managed
  // SIG: 00 02 12 54 12 7C 12 7C
  {
    // Method begins at RVA 0x260a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SubtractChecked

  .method /*060001BE*/ public hidebysig static 
          class System.Linq.Expressions.BinaryExpression/*02000015*/ 
          SubtractChecked(class System.Linq.Expressions.Expression/*0200001F*/ left,
                          class System.Linq.Expressions.Expression/*0200001F*/ right,
                          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 03 12 54 12 7C 12 7C 12 80 99
  {
    .param [3]/*08000343*/ 
    .custom /*0C000209:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x260d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SubtractChecked

  .method /*060001BF*/ public hidebysig static 
          class System.Linq.Expressions.SwitchExpression/*0200003B*/ 
          Switch(class System.Linq.Expressions.Expression/*0200001F*/ switchValue,
                 class System.Linq.Expressions.Expression/*0200001F*/ defaultBody,
                 class System.Linq.Expressions.SwitchCase/*0200003A*/[] cases) cil managed
  // SIG: 00 03 12 80 EC 12 7C 12 7C 1D 12 80 E8
  {
    .param [2]/*08000345*/ 
    .custom /*0C00020A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000346*/ 
    .custom /*0C00020B:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00020C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2610
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Switch

  .method /*060001C0*/ public hidebysig static 
          class System.Linq.Expressions.SwitchExpression/*0200003B*/ 
          Switch(class System.Linq.Expressions.Expression/*0200001F*/ switchValue,
                 class System.Linq.Expressions.Expression/*0200001F*/ defaultBody,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ comparison,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.SwitchCase/*0200003A*/> cases) cil managed
  // SIG: 00 04 12 80 EC 12 7C 12 7C 12 80 99 15 12 80 89 01 12 80 E8
  {
    .param [2]/*08000348*/ 
    .custom /*0C00020D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000349*/ 
    .custom /*0C00020E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800034A*/ 
    .custom /*0C00020F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2613
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Switch

  .method /*060001C1*/ public hidebysig static 
          class System.Linq.Expressions.SwitchExpression/*0200003B*/ 
          Switch(class System.Linq.Expressions.Expression/*0200001F*/ switchValue,
                 class System.Linq.Expressions.Expression/*0200001F*/ defaultBody,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ comparison,
                 class System.Linq.Expressions.SwitchCase/*0200003A*/[] cases) cil managed
  // SIG: 00 04 12 80 EC 12 7C 12 7C 12 80 99 1D 12 80 E8
  {
    .param [2]/*0800034C*/ 
    .custom /*0C000210:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800034D*/ 
    .custom /*0C000211:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800034E*/ 
    .custom /*0C000212:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000213:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2616
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Switch

  .method /*060001C2*/ public hidebysig static 
          class System.Linq.Expressions.SwitchExpression/*0200003B*/ 
          Switch(class System.Linq.Expressions.Expression/*0200001F*/ switchValue,
                 class System.Linq.Expressions.SwitchCase/*0200003A*/[] cases) cil managed
  // SIG: 00 02 12 80 EC 12 7C 1D 12 80 E8
  {
    .param [2]/*08000350*/ 
    .custom /*0C000214:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000215:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2619
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Switch

  .method /*060001C3*/ public hidebysig static 
          class System.Linq.Expressions.SwitchExpression/*0200003B*/ 
          Switch(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                 class System.Linq.Expressions.Expression/*0200001F*/ switchValue,
                 class System.Linq.Expressions.Expression/*0200001F*/ defaultBody,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ comparison,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.SwitchCase/*0200003A*/> cases) cil managed
  // SIG: 00 05 12 80 EC 12 59 12 7C 12 7C 12 80 99 15 12 80 89 01 12 80 E8
  {
    .custom /*0C00016C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000351*/ 
    .custom /*0C000216:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000353*/ 
    .custom /*0C000217:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000356*/ 
    .custom /*0C000218:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x261c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Switch

  .method /*060001C4*/ public hidebysig static 
          class System.Linq.Expressions.SwitchExpression/*0200003B*/ 
          Switch(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                 class System.Linq.Expressions.Expression/*0200001F*/ switchValue,
                 class System.Linq.Expressions.Expression/*0200001F*/ defaultBody,
                 class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ comparison,
                 class System.Linq.Expressions.SwitchCase/*0200003A*/[] cases) cil managed
  // SIG: 00 05 12 80 EC 12 59 12 7C 12 7C 12 80 99 1D 12 80 E8
  {
    .custom /*0C00016D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000357*/ 
    .custom /*0C000219:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000359*/ 
    .custom /*0C00021A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800035C*/ 
    .custom /*0C00021B:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00021C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x261f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Switch

  .method /*060001C5*/ public hidebysig static 
          class System.Linq.Expressions.SwitchCase/*0200003A*/ 
          SwitchCase(class System.Linq.Expressions.Expression/*0200001F*/ body,
                     class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> testValues) cil managed
  // SIG: 00 02 12 80 E8 12 7C 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x2622
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SwitchCase

  .method /*060001C6*/ public hidebysig static 
          class System.Linq.Expressions.SwitchCase/*0200003A*/ 
          SwitchCase(class System.Linq.Expressions.Expression/*0200001F*/ body,
                     class System.Linq.Expressions.Expression/*0200001F*/[] testValues) cil managed
  // SIG: 00 02 12 80 E8 12 7C 1D 12 7C
  {
    .param [2]/*08000360*/ 
    .custom /*0C00021D:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2625
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SwitchCase

  .method /*060001C7*/ public hidebysig static 
          class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/ 
          SymbolDocument(string fileName) cil managed
  // SIG: 00 01 12 80 F0 0E
  {
    // Method begins at RVA 0x2628
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SymbolDocument

  .method /*060001C8*/ public hidebysig static 
          class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/ 
          SymbolDocument(string fileName,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ language) cil managed
  // SIG: 00 02 12 80 F0 0E 11 80 BD
  {
    // Method begins at RVA 0x262b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SymbolDocument

  .method /*060001C9*/ public hidebysig static 
          class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/ 
          SymbolDocument(string fileName,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ language,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ languageVendor) cil managed
  // SIG: 00 03 12 80 F0 0E 11 80 BD 11 80 BD
  {
    // Method begins at RVA 0x262e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SymbolDocument

  .method /*060001CA*/ public hidebysig static 
          class System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/ 
          SymbolDocument(string fileName,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ language,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ languageVendor,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ documentType) cil managed
  // SIG: 00 04 12 80 F0 0E 11 80 BD 11 80 BD 11 80 BD
  {
    // Method begins at RVA 0x2631
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::SymbolDocument

  .method /*060001CB*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Throw(class System.Linq.Expressions.Expression/*0200001F*/ 'value') cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    .param [1]/*0800036B*/ 
    .custom /*0C00021E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2634
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Throw

  .method /*060001CC*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Throw(class System.Linq.Expressions.Expression/*0200001F*/ 'value',
                class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 59
  {
    .param [1]/*0800036C*/ 
    .custom /*0C00021F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2637
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Throw

  .method /*060001CD*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x263a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::ToString

  .method /*060001CE*/ public hidebysig static 
          class System.Linq.Expressions.TryExpression/*0200003D*/ 
          TryCatch(class System.Linq.Expressions.Expression/*0200001F*/ body,
                   class System.Linq.Expressions.CatchBlock/*02000017*/[] handlers) cil managed
  // SIG: 00 02 12 80 F4 12 7C 1D 12 5C
  {
    .param [2]/*0800036F*/ 
    .custom /*0C000220:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000221:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x263d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TryCatch

  .method /*060001CF*/ public hidebysig static 
          class System.Linq.Expressions.TryExpression/*0200003D*/ 
          TryCatchFinally(class System.Linq.Expressions.Expression/*0200001F*/ body,
                          class System.Linq.Expressions.Expression/*0200001F*/ 'finally',
                          class System.Linq.Expressions.CatchBlock/*02000017*/[] handlers) cil managed
  // SIG: 00 03 12 80 F4 12 7C 12 7C 1D 12 5C
  {
    .param [2]/*08000371*/ 
    .custom /*0C000222:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000372*/ 
    .custom /*0C000223:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000224:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2640
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TryCatchFinally

  .method /*060001D0*/ public hidebysig static 
          class System.Linq.Expressions.TryExpression/*0200003D*/ 
          TryFault(class System.Linq.Expressions.Expression/*0200001F*/ body,
                   class System.Linq.Expressions.Expression/*0200001F*/ 'fault') cil managed
  // SIG: 00 02 12 80 F4 12 7C 12 7C
  {
    .param [2]/*08000374*/ 
    .custom /*0C000225:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2643
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TryFault

  .method /*060001D1*/ public hidebysig static 
          class System.Linq.Expressions.TryExpression/*0200003D*/ 
          TryFinally(class System.Linq.Expressions.Expression/*0200001F*/ body,
                     class System.Linq.Expressions.Expression/*0200001F*/ 'finally') cil managed
  // SIG: 00 02 12 80 F4 12 7C 12 7C
  {
    .param [2]/*08000376*/ 
    .custom /*0C000226:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2646
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TryFinally

  .method /*060001D2*/ public hidebysig static 
          bool  TryGetActionType(class [System.Runtime/*23000001*/]System.Type/*01000016*/[] typeArgs,
                                 [out] class [System.Runtime/*23000001*/]System.Type/*01000016*/& actionType) cil managed
  // SIG: 00 02 02 1D 12 59 10 12 59
  {
    .param [2]/*08000378*/ 
    .custom /*0C000227:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000228:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000026 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2649
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TryGetActionType

  .method /*060001D3*/ public hidebysig static 
          bool  TryGetFuncType(class [System.Runtime/*23000001*/]System.Type/*01000016*/[] typeArgs,
                               [out] class [System.Runtime/*23000001*/]System.Type/*01000016*/& funcType) cil managed
  // SIG: 00 02 02 1D 12 59 10 12 59
  {
    .param [2]/*0800037A*/ 
    .custom /*0C000229:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00022A:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000026 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x264c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TryGetFuncType

  .method /*060001D4*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          TypeAs(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                 class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 59
  {
    // Method begins at RVA 0x264f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TypeAs

  .method /*060001D5*/ public hidebysig static 
          class System.Linq.Expressions.TypeBinaryExpression/*0200003E*/ 
          TypeEqual(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                    class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 F8 12 7C 12 59
  {
    // Method begins at RVA 0x2652
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TypeEqual

  .method /*060001D6*/ public hidebysig static 
          class System.Linq.Expressions.TypeBinaryExpression/*0200003E*/ 
          TypeIs(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                 class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 F8 12 7C 12 59
  {
    // Method begins at RVA 0x2655
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::TypeIs

  .method /*060001D7*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          UnaryPlus(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2658
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::UnaryPlus

  .method /*060001D8*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          UnaryPlus(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 'method') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 80 99
  {
    .param [2]/*08000383*/ 
    .custom /*0C00022B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x265b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::UnaryPlus

  .method /*060001D9*/ public hidebysig static 
          class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Unbox(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 80 FC 12 7C 12 59
  {
    // Method begins at RVA 0x265e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Unbox

  .method /*060001DA*/ public hidebysig static 
          class System.Linq.Expressions.ParameterExpression/*02000038*/ 
          Variable(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 01 12 80 E0 12 59
  {
    // Method begins at RVA 0x2661
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Variable

  .method /*060001DB*/ public hidebysig static 
          class System.Linq.Expressions.ParameterExpression/*02000038*/ 
          Variable(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                   string name) cil managed
  // SIG: 00 02 12 80 E0 12 59 0E
  {
    .param [2]/*08000388*/ 
    .custom /*0C00022C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2664
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::Variable

  .method /*060001DC*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitChildren(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2667
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression::VisitChildren

  .property /*1700003E*/ instance callconv(8) bool
          CanReduce()
  {
    .get instance bool System.Linq.Expressions.Expression/*0200001F*/::get_CanReduce() /* 0600009F */
  } // end of property Expression::CanReduce
  .property /*1700003F*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.Expression/*0200001F*/::get_NodeType() /* 060000A0 */
  } // end of property Expression::NodeType
  .property /*17000040*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.Expression/*0200001F*/::get_Type() /* 060000A1 */
  } // end of property Expression::Type
} // end of class System.Linq.Expressions.Expression

.class /*02000020*/ public auto ansi sealed System.Linq.Expressions.ExpressionType
       extends [System.Runtime/*23000001*/]System.Enum/*01000031*/
{
  .field /*04000006*/ public specialname rtspecialname int32 value__
  .field /*04000007*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Add = int32(0x00000000)
  .field /*04000008*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ AddChecked = int32(0x00000001)
  .field /*04000009*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ And = int32(0x00000002)
  .field /*0400000A*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ AndAlso = int32(0x00000003)
  .field /*0400000B*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ ArrayLength = int32(0x00000004)
  .field /*0400000C*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ ArrayIndex = int32(0x00000005)
  .field /*0400000D*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Call = int32(0x00000006)
  .field /*0400000E*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Coalesce = int32(0x00000007)
  .field /*0400000F*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Conditional = int32(0x00000008)
  .field /*04000010*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Constant = int32(0x00000009)
  .field /*04000011*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Convert = int32(0x0000000A)
  .field /*04000012*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ ConvertChecked = int32(0x0000000B)
  .field /*04000013*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Divide = int32(0x0000000C)
  .field /*04000014*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Equal = int32(0x0000000D)
  .field /*04000015*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ ExclusiveOr = int32(0x0000000E)
  .field /*04000016*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ GreaterThan = int32(0x0000000F)
  .field /*04000017*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ GreaterThanOrEqual = int32(0x00000010)
  .field /*04000018*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Invoke = int32(0x00000011)
  .field /*04000019*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Lambda = int32(0x00000012)
  .field /*0400001A*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ LeftShift = int32(0x00000013)
  .field /*0400001B*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ LessThan = int32(0x00000014)
  .field /*0400001C*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ LessThanOrEqual = int32(0x00000015)
  .field /*0400001D*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ ListInit = int32(0x00000016)
  .field /*0400001E*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ MemberAccess = int32(0x00000017)
  .field /*0400001F*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ MemberInit = int32(0x00000018)
  .field /*04000020*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Modulo = int32(0x00000019)
  .field /*04000021*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Multiply = int32(0x0000001A)
  .field /*04000022*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ MultiplyChecked = int32(0x0000001B)
  .field /*04000023*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Negate = int32(0x0000001C)
  .field /*04000024*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ UnaryPlus = int32(0x0000001D)
  .field /*04000025*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ NegateChecked = int32(0x0000001E)
  .field /*04000026*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ New = int32(0x0000001F)
  .field /*04000027*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ NewArrayInit = int32(0x00000020)
  .field /*04000028*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ NewArrayBounds = int32(0x00000021)
  .field /*04000029*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Not = int32(0x00000022)
  .field /*0400002A*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ NotEqual = int32(0x00000023)
  .field /*0400002B*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Or = int32(0x00000024)
  .field /*0400002C*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ OrElse = int32(0x00000025)
  .field /*0400002D*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Parameter = int32(0x00000026)
  .field /*0400002E*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Power = int32(0x00000027)
  .field /*0400002F*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Quote = int32(0x00000028)
  .field /*04000030*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ RightShift = int32(0x00000029)
  .field /*04000031*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Subtract = int32(0x0000002A)
  .field /*04000032*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ SubtractChecked = int32(0x0000002B)
  .field /*04000033*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ TypeAs = int32(0x0000002C)
  .field /*04000034*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ TypeIs = int32(0x0000002D)
  .field /*04000035*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Assign = int32(0x0000002E)
  .field /*04000036*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Block = int32(0x0000002F)
  .field /*04000037*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ DebugInfo = int32(0x00000030)
  .field /*04000038*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Decrement = int32(0x00000031)
  .field /*04000039*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Dynamic = int32(0x00000032)
  .field /*0400003A*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Default = int32(0x00000033)
  .field /*0400003B*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Extension = int32(0x00000034)
  .field /*0400003C*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Goto = int32(0x00000035)
  .field /*0400003D*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Increment = int32(0x00000036)
  .field /*0400003E*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Index = int32(0x00000037)
  .field /*0400003F*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Label = int32(0x00000038)
  .field /*04000040*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ RuntimeVariables = int32(0x00000039)
  .field /*04000041*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Loop = int32(0x0000003A)
  .field /*04000042*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Switch = int32(0x0000003B)
  .field /*04000043*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Throw = int32(0x0000003C)
  .field /*04000044*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Try = int32(0x0000003D)
  .field /*04000045*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ Unbox = int32(0x0000003E)
  .field /*04000046*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ AddAssign = int32(0x0000003F)
  .field /*04000047*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ AndAssign = int32(0x00000040)
  .field /*04000048*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ DivideAssign = int32(0x00000041)
  .field /*04000049*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ ExclusiveOrAssign = int32(0x00000042)
  .field /*0400004A*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ LeftShiftAssign = int32(0x00000043)
  .field /*0400004B*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ ModuloAssign = int32(0x00000044)
  .field /*0400004C*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ MultiplyAssign = int32(0x00000045)
  .field /*0400004D*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ OrAssign = int32(0x00000046)
  .field /*0400004E*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ PowerAssign = int32(0x00000047)
  .field /*0400004F*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ RightShiftAssign = int32(0x00000048)
  .field /*04000050*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ SubtractAssign = int32(0x00000049)
  .field /*04000051*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ AddAssignChecked = int32(0x0000004A)
  .field /*04000052*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ MultiplyAssignChecked = int32(0x0000004B)
  .field /*04000053*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ SubtractAssignChecked = int32(0x0000004C)
  .field /*04000054*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ PreIncrementAssign = int32(0x0000004D)
  .field /*04000055*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ PreDecrementAssign = int32(0x0000004E)
  .field /*04000056*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ PostIncrementAssign = int32(0x0000004F)
  .field /*04000057*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ PostDecrementAssign = int32(0x00000050)
  .field /*04000058*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ TypeEqual = int32(0x00000051)
  .field /*04000059*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ OnesComplement = int32(0x00000052)
  .field /*0400005A*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ IsTrue = int32(0x00000053)
  .field /*0400005B*/ public static literal valuetype System.Linq.Expressions.ExpressionType/*02000020*/ IsFalse = int32(0x00000054)
} // end of class System.Linq.Expressions.ExpressionType

.class /*02000021*/ public abstract auto ansi beforefieldinit System.Linq.Expressions.ExpressionVisitor
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00007B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060001DD*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x266a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExpressionVisitor::.ctor

  .method /*060001DE*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          Visit(class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> nodes) cil managed
  // SIG: 20 01 15 12 5D 01 12 7C 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x2672
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::Visit

  .method /*060001DF*/ public hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Visit(class System.Linq.Expressions.Expression/*0200001F*/ node) cil managed
  // SIG: 20 01 12 7C 12 7C
  {
    .custom /*0C00017C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800038B*/ 
    .custom /*0C00022D:0A000027*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000032*/::.ctor(string) /* 0A000027 */ = ( 01 00 04 6E 6F 64 65 00 00 )                      // ...node..
    // Method begins at RVA 0x2675
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::Visit

  .method /*060001E0*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<!!T> 
          VisitAndConvert<(System.Linq.Expressions.Expression/*0200001F*/) T>(class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<!!T> nodes,
                                                                              string callerName) cil managed
  // SIG: 30 01 02 15 12 5D 01 1E 00 15 12 5D 01 1E 00 0E
  {
    .param type T /*2A000011*/ 
      .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800038E*/ 
    .custom /*0C00022E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2678
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitAndConvert

  .method /*060001E1*/ public hidebysig instance !!T 
          VisitAndConvert<(System.Linq.Expressions.Expression/*0200001F*/) T>(!!T node,
                                                                              string callerName) cil managed
  // SIG: 30 01 02 1E 00 1E 00 0E
  {
    .custom /*0C00017D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param type T /*2A000012*/ 
      .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000012*/ , System.Linq.Expressions.Expression/*0200001F*/
      .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800038F*/ 
    .custom /*0C00022F:0A000027*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000032*/::.ctor(string) /* 0A000027 */ = ( 01 00 04 6E 6F 64 65 00 00 )                      // ...node..
    // Method begins at RVA 0x267b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitAndConvert

  .method /*060001E2*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitBinary(class System.Linq.Expressions.BinaryExpression/*02000015*/ node) cil managed
  // SIG: 20 01 12 7C 12 54
  {
    // Method begins at RVA 0x267e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitBinary

  .method /*060001E3*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitBlock(class System.Linq.Expressions.BlockExpression/*02000016*/ node) cil managed
  // SIG: 20 01 12 7C 12 58
  {
    // Method begins at RVA 0x2681
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitBlock

  .method /*060001E4*/ family hidebysig newslot virtual 
          instance class System.Linq.Expressions.CatchBlock/*02000017*/ 
          VisitCatchBlock(class System.Linq.Expressions.CatchBlock/*02000017*/ node) cil managed
  // SIG: 20 01 12 5C 12 5C
  {
    // Method begins at RVA 0x2684
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitCatchBlock

  .method /*060001E5*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitConditional(class System.Linq.Expressions.ConditionalExpression/*02000018*/ node) cil managed
  // SIG: 20 01 12 7C 12 60
  {
    // Method begins at RVA 0x2687
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitConditional

  .method /*060001E6*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitConstant(class System.Linq.Expressions.ConstantExpression/*02000019*/ node) cil managed
  // SIG: 20 01 12 7C 12 64
  {
    // Method begins at RVA 0x268a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitConstant

  .method /*060001E7*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitDebugInfo(class System.Linq.Expressions.DebugInfoExpression/*0200001A*/ node) cil managed
  // SIG: 20 01 12 7C 12 68
  {
    // Method begins at RVA 0x268d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitDebugInfo

  .method /*060001E8*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitDefault(class System.Linq.Expressions.DefaultExpression/*0200001B*/ node) cil managed
  // SIG: 20 01 12 7C 12 6C
  {
    // Method begins at RVA 0x2690
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitDefault

  .method /*060001E9*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitDynamic(class System.Linq.Expressions.DynamicExpression/*0200001C*/ node) cil managed
  // SIG: 20 01 12 7C 12 70
  {
    // Method begins at RVA 0x2693
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitDynamic

  .method /*060001EA*/ family hidebysig newslot virtual 
          instance class System.Linq.Expressions.ElementInit/*0200001E*/ 
          VisitElementInit(class System.Linq.Expressions.ElementInit/*0200001E*/ node) cil managed
  // SIG: 20 01 12 78 12 78
  {
    // Method begins at RVA 0x2696
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitElementInit

  .method /*060001EB*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitExtension(class System.Linq.Expressions.Expression/*0200001F*/ node) cil managed
  // SIG: 20 01 12 7C 12 7C
  {
    // Method begins at RVA 0x2699
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitExtension

  .method /*060001EC*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitGoto(class System.Linq.Expressions.GotoExpression/*02000023*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 8C
  {
    // Method begins at RVA 0x269c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitGoto

  .method /*060001ED*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitIndex(class System.Linq.Expressions.IndexExpression/*02000027*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 9C
  {
    // Method begins at RVA 0x269f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitIndex

  .method /*060001EE*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitInvocation(class System.Linq.Expressions.InvocationExpression/*02000028*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 A0
  {
    // Method begins at RVA 0x26a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitInvocation

  .method /*060001EF*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitLabel(class System.Linq.Expressions.LabelExpression/*02000029*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 A4
  {
    // Method begins at RVA 0x26a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitLabel

  .method /*060001F0*/ family hidebysig newslot virtual 
          instance class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          VisitLabelTarget(class System.Linq.Expressions.LabelTarget/*0200002A*/ node) cil managed
  // SIG: 20 01 12 80 A8 12 80 A8
  {
    .custom /*0C000187:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080003A0*/ 
    .custom /*0C000230:0A000027*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000032*/::.ctor(string) /* 0A000027 */ = ( 01 00 04 6E 6F 64 65 00 00 )                      // ...node..
    // Method begins at RVA 0x26a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitLabelTarget

  .method /*060001F1*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitLambda<T>(class System.Linq.Expressions.Expression`1/*02000022*/<!!T> node) cil managed
  // SIG: 30 01 01 12 7C 15 12 80 88 01 1E 00
  {
    .param type T /*2A000013*/ 
      .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitLambda

  .method /*060001F2*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitListInit(class System.Linq.Expressions.ListInitExpression/*0200002C*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 B0
  {
    // Method begins at RVA 0x26ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitListInit

  .method /*060001F3*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitLoop(class System.Linq.Expressions.LoopExpression/*0200002D*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 B4
  {
    // Method begins at RVA 0x26b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitLoop

  .method /*060001F4*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitMember(class System.Linq.Expressions.MemberExpression/*02000031*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 C4
  {
    // Method begins at RVA 0x26b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitMember

  .method /*060001F5*/ family hidebysig newslot virtual 
          instance class System.Linq.Expressions.MemberAssignment/*0200002E*/ 
          VisitMemberAssignment(class System.Linq.Expressions.MemberAssignment/*0200002E*/ node) cil managed
  // SIG: 20 01 12 80 B8 12 80 B8
  {
    // Method begins at RVA 0x26b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitMemberAssignment

  .method /*060001F6*/ family hidebysig newslot virtual 
          instance class System.Linq.Expressions.MemberBinding/*0200002F*/ 
          VisitMemberBinding(class System.Linq.Expressions.MemberBinding/*0200002F*/ node) cil managed
  // SIG: 20 01 12 80 BC 12 80 BC
  {
    // Method begins at RVA 0x26ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitMemberBinding

  .method /*060001F7*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitMemberInit(class System.Linq.Expressions.MemberInitExpression/*02000032*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 C8
  {
    // Method begins at RVA 0x26bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitMemberInit

  .method /*060001F8*/ family hidebysig newslot virtual 
          instance class System.Linq.Expressions.MemberListBinding/*02000033*/ 
          VisitMemberListBinding(class System.Linq.Expressions.MemberListBinding/*02000033*/ node) cil managed
  // SIG: 20 01 12 80 CC 12 80 CC
  {
    // Method begins at RVA 0x26c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitMemberListBinding

  .method /*060001F9*/ family hidebysig newslot virtual 
          instance class System.Linq.Expressions.MemberMemberBinding/*02000034*/ 
          VisitMemberMemberBinding(class System.Linq.Expressions.MemberMemberBinding/*02000034*/ node) cil managed
  // SIG: 20 01 12 80 D0 12 80 D0
  {
    // Method begins at RVA 0x26c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitMemberMemberBinding

  .method /*060001FA*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitMethodCall(class System.Linq.Expressions.MethodCallExpression/*02000035*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 D4
  {
    // Method begins at RVA 0x26c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitMethodCall

  .method /*060001FB*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitNew(class System.Linq.Expressions.NewExpression/*02000037*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 DC
  {
    // Method begins at RVA 0x26c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitNew

  .method /*060001FC*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitNewArray(class System.Linq.Expressions.NewArrayExpression/*02000036*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 D8
  {
    // Method begins at RVA 0x26cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitNewArray

  .method /*060001FD*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitParameter(class System.Linq.Expressions.ParameterExpression/*02000038*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 E0
  {
    // Method begins at RVA 0x26cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitParameter

  .method /*060001FE*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitRuntimeVariables(class System.Linq.Expressions.RuntimeVariablesExpression/*02000039*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 E4
  {
    // Method begins at RVA 0x26d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitRuntimeVariables

  .method /*060001FF*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitSwitch(class System.Linq.Expressions.SwitchExpression/*0200003B*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 EC
  {
    // Method begins at RVA 0x26d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitSwitch

  .method /*06000200*/ family hidebysig newslot virtual 
          instance class System.Linq.Expressions.SwitchCase/*0200003A*/ 
          VisitSwitchCase(class System.Linq.Expressions.SwitchCase/*0200003A*/ node) cil managed
  // SIG: 20 01 12 80 E8 12 80 E8
  {
    // Method begins at RVA 0x26d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitSwitchCase

  .method /*06000201*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitTry(class System.Linq.Expressions.TryExpression/*0200003D*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 F4
  {
    // Method begins at RVA 0x26db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitTry

  .method /*06000202*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitTypeBinary(class System.Linq.Expressions.TypeBinaryExpression/*0200003E*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 F8
  {
    // Method begins at RVA 0x26de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitTypeBinary

  .method /*06000203*/ famorassem hidebysig newslot virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          VisitUnary(class System.Linq.Expressions.UnaryExpression/*0200003F*/ node) cil managed
  // SIG: 20 01 12 7C 12 80 FC
  {
    // Method begins at RVA 0x26e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::VisitUnary

  .method /*06000204*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<!!T> 
          Visit<T>(class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<!!T> nodes,
                   class [System.Runtime/*23000001*/]System.Func`2/*01000033*/<!!T,!!T> elementVisitor) cil managed
  // SIG: 10 01 02 15 12 5D 01 1E 00 15 12 5D 01 1E 00 15 12 80 CD 02 1E 00 1E 00
  {
    .param type T /*2A000014*/ 
      .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpressionVisitor::Visit

} // end of class System.Linq.Expressions.ExpressionVisitor

.class /*02000022*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.Expression`1<TDelegate>
       extends System.Linq.Expressions.LambdaExpression/*0200002B*/
{
  .custom /*0C00007D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type TDelegate /*2A000008*/ 
    .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000205*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26e7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000236       */ call       instance void System.Linq.Expressions.LambdaExpression/*0200002B*/::.ctor() /* 06000236 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Expression`1::.ctor

  .method /*06000206*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x26ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression`1::Accept

  .method /*06000207*/ public hidebysig instance !TDelegate 
          Compile() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x26f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression`1::Compile

  .method /*06000208*/ public hidebysig instance !TDelegate 
          Compile(bool preferInterpretation) cil managed
  // SIG: 20 01 13 00 02
  {
    // Method begins at RVA 0x26f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression`1::Compile

  .method /*06000209*/ public hidebysig instance !TDelegate 
          Compile(class System.Runtime.CompilerServices.DebugInfoGenerator/*0200000B*/ debugInfoGenerator) cil managed
  // SIG: 20 01 13 00 12 2C
  {
    // Method begins at RVA 0x26f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression`1::Compile

  .method /*0600020A*/ public hidebysig instance class System.Linq.Expressions.Expression`1/*02000022*/<!TDelegate> 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ body,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters) cil managed
  // SIG: 20 02 15 12 80 88 01 13 00 12 7C 15 12 80 89 01 12 80 E0
  {
    .param [2]/*080003BB*/ 
    .custom /*0C000231:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x26fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Expression`1::Update

} // end of class System.Linq.Expressions.Expression`1

.class /*02000023*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.GotoExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C000080:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600020B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26fe
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GotoExpression::.ctor

  .method /*0600020C*/ public hidebysig specialname 
          instance valuetype System.Linq.Expressions.GotoExpressionKind/*02000024*/ 
          get_Kind() cil managed
  // SIG: 20 00 11 80 90
  {
    // Method begins at RVA 0x2706
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GotoExpression::get_Kind

  .method /*0600020D*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2709
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GotoExpression::get_NodeType

  .method /*0600020E*/ public hidebysig specialname 
          instance class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          get_Target() cil managed
  // SIG: 20 00 12 80 A8
  {
    // Method begins at RVA 0x270c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GotoExpression::get_Target

  .method /*0600020F*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x270f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GotoExpression::get_Type

  .method /*06000210*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Value() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C000191:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2712
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GotoExpression::get_Value

  .method /*06000211*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2715
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GotoExpression::Accept

  .method /*06000212*/ public hidebysig instance class System.Linq.Expressions.GotoExpression/*02000023*/ 
          Update(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                 class System.Linq.Expressions.Expression/*0200001F*/ 'value') cil managed
  // SIG: 20 02 12 80 8C 12 80 A8 12 7C
  {
    .param [2]/*080003BE*/ 
    .custom /*0C000232:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2718
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GotoExpression::Update

  .property /*17000041*/ instance callconv(8) valuetype System.Linq.Expressions.GotoExpressionKind/*02000024*/
          Kind()
  {
    .get instance valuetype System.Linq.Expressions.GotoExpressionKind/*02000024*/ System.Linq.Expressions.GotoExpression/*02000023*/::get_Kind() /* 0600020C */
  } // end of property GotoExpression::Kind
  .property /*17000042*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.GotoExpression/*02000023*/::get_NodeType() /* 0600020D */
  } // end of property GotoExpression::NodeType
  .property /*17000043*/ instance callconv(8) class System.Linq.Expressions.LabelTarget/*0200002A*/
          Target()
  {
    .get instance class System.Linq.Expressions.LabelTarget/*0200002A*/ System.Linq.Expressions.GotoExpression/*02000023*/::get_Target() /* 0600020E */
  } // end of property GotoExpression::Target
  .property /*17000044*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.GotoExpression/*02000023*/::get_Type() /* 0600020F */
  } // end of property GotoExpression::Type
  .property /*17000045*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Value()
  {
    .custom /*0C0000C6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.GotoExpression/*02000023*/::get_Value() /* 06000210 */
  } // end of property GotoExpression::Value
} // end of class System.Linq.Expressions.GotoExpression

.class /*02000024*/ public auto ansi sealed System.Linq.Expressions.GotoExpressionKind
       extends [System.Runtime/*23000001*/]System.Enum/*01000031*/
{
  .field /*0400005C*/ public specialname rtspecialname int32 value__
  .field /*0400005D*/ public static literal valuetype System.Linq.Expressions.GotoExpressionKind/*02000024*/ Goto = int32(0x00000000)
  .field /*0400005E*/ public static literal valuetype System.Linq.Expressions.GotoExpressionKind/*02000024*/ Return = int32(0x00000001)
  .field /*0400005F*/ public static literal valuetype System.Linq.Expressions.GotoExpressionKind/*02000024*/ Break = int32(0x00000002)
  .field /*04000060*/ public static literal valuetype System.Linq.Expressions.GotoExpressionKind/*02000024*/ Continue = int32(0x00000003)
} // end of class System.Linq.Expressions.GotoExpressionKind

.class /*02000025*/ interface public abstract auto ansi System.Linq.Expressions.IArgumentProvider
{
  .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000213*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_ArgumentCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IArgumentProvider::get_ArgumentCount

  .method /*06000214*/ public hidebysig newslot abstract virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          GetArgument(int32 index) cil managed
  // SIG: 20 01 12 7C 08
  {
    // Method begins at RVA 0x0
  } // end of method IArgumentProvider::GetArgument

  .property /*17000046*/ instance callconv(8) int32
          ArgumentCount()
  {
    .get instance int32 System.Linq.Expressions.IArgumentProvider/*02000025*/::get_ArgumentCount() /* 06000213 */
  } // end of property IArgumentProvider::ArgumentCount
} // end of class System.Linq.Expressions.IArgumentProvider

.class /*02000026*/ interface public abstract auto ansi System.Linq.Expressions.IDynamicExpression
       implements System.Linq.Expressions.IArgumentProvider/*02000025*/
{
  .custom /*0C000083:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type System.Linq.Expressions.IArgumentProvider/*02000025*/
  .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000215*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_DelegateType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x0
  } // end of method IDynamicExpression::get_DelegateType

  .method /*06000216*/ public hidebysig newslot abstract virtual 
          instance object  CreateCallSite() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method IDynamicExpression::CreateCallSite

  .method /*06000217*/ public hidebysig newslot abstract virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Rewrite(class System.Linq.Expressions.Expression/*0200001F*/[] args) cil managed
  // SIG: 20 01 12 7C 1D 12 7C
  {
    // Method begins at RVA 0x0
  } // end of method IDynamicExpression::Rewrite

  .property /*17000047*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          DelegateType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.IDynamicExpression/*02000026*/::get_DelegateType() /* 06000215 */
  } // end of property IDynamicExpression::DelegateType
} // end of class System.Linq.Expressions.IDynamicExpression

.class /*02000027*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.IndexExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
       implements System.Linq.Expressions.IArgumentProvider/*02000025*/
{
  .custom /*0C000084:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000085:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Linq.Expressions.IArgumentProvider/*02000025*/
  .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000218*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x271b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method IndexExpression::.ctor

  .method /*06000219*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_Arguments() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x2723
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::get_Arguments

  .method /*0600021A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100002D*/ 
          get_Indexer() cil managed
  // SIG: 20 00 12 80 B5
  {
    .custom /*0C000194:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2726
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::get_Indexer

  .method /*0600021B*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2729
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::get_NodeType

  .method /*0600021C*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Object() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C000196:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x272c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::get_Object

  .method /*0600021D*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() cil managed
  // SIG: 20 00 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::get_ArgumentCount /*02000025::06000213*/ 
    // Method begins at RVA 0x272f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount

  .method /*0600021E*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2732
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::get_Type

  .method /*0600021F*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2735
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::Accept

  .method /*06000220*/ private hidebysig newslot virtual final 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          System.Linq.Expressions.IArgumentProvider.GetArgument(int32 index) cil managed
  // SIG: 20 01 12 7C 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::GetArgument /*02000025::06000214*/ 
    // Method begins at RVA 0x2738
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::System.Linq.Expressions.IArgumentProvider.GetArgument

  .method /*06000221*/ public hidebysig instance class System.Linq.Expressions.IndexExpression/*02000027*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ 'object',
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 20 02 12 80 9C 12 7C 15 12 80 89 01 12 7C
  {
    .param [2]/*080003C4*/ 
    .custom /*0C000233:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x273b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IndexExpression::Update

  .property /*17000048*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          Arguments()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.IndexExpression/*02000027*/::get_Arguments() /* 06000219 */
  } // end of property IndexExpression::Arguments
  .property /*17000049*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100002D*/
          Indexer()
  {
    .custom /*0C0000D0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100002D*/ System.Linq.Expressions.IndexExpression/*02000027*/::get_Indexer() /* 0600021A */
  } // end of property IndexExpression::Indexer
  .property /*1700004A*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.IndexExpression/*02000027*/::get_NodeType() /* 0600021B */
  } // end of property IndexExpression::NodeType
  .property /*1700004B*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Object()
  {
    .custom /*0C0000D3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.IndexExpression/*02000027*/::get_Object() /* 0600021C */
  } // end of property IndexExpression::Object
  .property /*1700004C*/ instance callconv(8) int32
          System.Linq.Expressions.IArgumentProvider.ArgumentCount()
  {
    .get instance int32 System.Linq.Expressions.IndexExpression/*02000027*/::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() /* 0600021D */
  } // end of property IndexExpression::System.Linq.Expressions.IArgumentProvider.ArgumentCount
  .property /*1700004D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.IndexExpression/*02000027*/::get_Type() /* 0600021E */
  } // end of property IndexExpression::Type
} // end of class System.Linq.Expressions.IndexExpression

.class /*02000028*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.InvocationExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
       implements System.Linq.Expressions.IArgumentProvider/*02000025*/
{
  .custom /*0C000086:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000087:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Linq.Expressions.IArgumentProvider/*02000025*/
  .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000222*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x273e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvocationExpression::.ctor

  .method /*06000223*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_Arguments() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x2746
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvocationExpression::get_Arguments

  .method /*06000224*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Expression() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x2749
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvocationExpression::get_Expression

  .method /*06000225*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x274c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvocationExpression::get_NodeType

  .method /*06000226*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() cil managed
  // SIG: 20 00 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::get_ArgumentCount /*02000025::06000213*/ 
    // Method begins at RVA 0x274f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvocationExpression::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount

  .method /*06000227*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2752
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvocationExpression::get_Type

  .method /*06000228*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2755
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvocationExpression::Accept

  .method /*06000229*/ private hidebysig newslot virtual final 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          System.Linq.Expressions.IArgumentProvider.GetArgument(int32 index) cil managed
  // SIG: 20 01 12 7C 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::GetArgument /*02000025::06000214*/ 
    // Method begins at RVA 0x2758
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvocationExpression::System.Linq.Expressions.IArgumentProvider.GetArgument

  .method /*0600022A*/ public hidebysig instance class System.Linq.Expressions.InvocationExpression/*02000028*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 20 02 12 80 A0 12 7C 15 12 80 89 01 12 7C
  {
    .param [2]/*080003C8*/ 
    .custom /*0C000234:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x275b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvocationExpression::Update

  .property /*1700004E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          Arguments()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.InvocationExpression/*02000028*/::get_Arguments() /* 06000223 */
  } // end of property InvocationExpression::Arguments
  .property /*1700004F*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Expression()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.InvocationExpression/*02000028*/::get_Expression() /* 06000224 */
  } // end of property InvocationExpression::Expression
  .property /*17000050*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.InvocationExpression/*02000028*/::get_NodeType() /* 06000225 */
  } // end of property InvocationExpression::NodeType
  .property /*17000051*/ instance callconv(8) int32
          System.Linq.Expressions.IArgumentProvider.ArgumentCount()
  {
    .get instance int32 System.Linq.Expressions.InvocationExpression/*02000028*/::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() /* 06000226 */
  } // end of property InvocationExpression::System.Linq.Expressions.IArgumentProvider.ArgumentCount
  .property /*17000052*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.InvocationExpression/*02000028*/::get_Type() /* 06000227 */
  } // end of property InvocationExpression::Type
} // end of class System.Linq.Expressions.InvocationExpression

.class /*02000029*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.LabelExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C000088:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600022B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x275e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LabelExpression::.ctor

  .method /*0600022C*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_DefaultValue() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C00019E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2766
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelExpression::get_DefaultValue

  .method /*0600022D*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2769
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelExpression::get_NodeType

  .method /*0600022E*/ public hidebysig specialname 
          instance class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          get_Target() cil managed
  // SIG: 20 00 12 80 A8
  {
    // Method begins at RVA 0x276c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelExpression::get_Target

  .method /*0600022F*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x276f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelExpression::get_Type

  .method /*06000230*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2772
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelExpression::Accept

  .method /*06000231*/ public hidebysig instance class System.Linq.Expressions.LabelExpression/*02000029*/ 
          Update(class System.Linq.Expressions.LabelTarget/*0200002A*/ target,
                 class System.Linq.Expressions.Expression/*0200001F*/ defaultValue) cil managed
  // SIG: 20 02 12 80 A4 12 80 A8 12 7C
  {
    .param [2]/*080003CB*/ 
    .custom /*0C000235:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2775
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelExpression::Update

  .property /*17000053*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          DefaultValue()
  {
    .custom /*0C0000E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.LabelExpression/*02000029*/::get_DefaultValue() /* 0600022C */
  } // end of property LabelExpression::DefaultValue
  .property /*17000054*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.LabelExpression/*02000029*/::get_NodeType() /* 0600022D */
  } // end of property LabelExpression::NodeType
  .property /*17000055*/ instance callconv(8) class System.Linq.Expressions.LabelTarget/*0200002A*/
          Target()
  {
    .get instance class System.Linq.Expressions.LabelTarget/*0200002A*/ System.Linq.Expressions.LabelExpression/*02000029*/::get_Target() /* 0600022E */
  } // end of property LabelExpression::Target
  .property /*17000056*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.LabelExpression/*02000029*/::get_Type() /* 0600022F */
  } // end of property LabelExpression::Type
} // end of class System.Linq.Expressions.LabelExpression

.class /*0200002A*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.LabelTarget
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000232*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2778
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LabelTarget::.ctor

  .method /*06000233*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2780
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelTarget::get_Name

  .method /*06000234*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2783
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelTarget::get_Type

  .method /*06000235*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2786
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LabelTarget::ToString

  .property /*17000057*/ instance callconv(8) string
          Name()
  {
    .custom /*0C0000E3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Linq.Expressions.LabelTarget/*0200002A*/::get_Name() /* 06000233 */
  } // end of property LabelTarget::Name
  .property /*17000058*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.LabelTarget/*0200002A*/::get_Type() /* 06000234 */
  } // end of property LabelTarget::Type
} // end of class System.Linq.Expressions.LabelTarget

.class /*0200002B*/ public abstract auto ansi beforefieldinit System.Linq.Expressions.LambdaExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C00008D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000236*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2789
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LambdaExpression::.ctor

  .method /*06000237*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Body() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x2791
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::get_Body

  .method /*06000238*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001A3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2794
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::get_Name

  .method /*06000239*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2797
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::get_NodeType

  .method /*0600023A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> 
          get_Parameters() cil managed
  // SIG: 20 00 15 12 5D 01 12 80 E0
  {
    // Method begins at RVA 0x279a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::get_Parameters

  .method /*0600023B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x279d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::get_ReturnType

  .method /*0600023C*/ public hidebysig specialname 
          instance bool  get_TailCall() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x27a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::get_TailCall

  .method /*0600023D*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x27a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::get_Type

  .method /*0600023E*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Delegate/*01000034*/ 
          Compile() cil managed
  // SIG: 20 00 12 80 D1
  {
    // Method begins at RVA 0x27a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::Compile

  .method /*0600023F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Delegate/*01000034*/ 
          Compile(bool preferInterpretation) cil managed
  // SIG: 20 01 12 80 D1 02
  {
    // Method begins at RVA 0x27a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::Compile

  .method /*06000240*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Delegate/*01000034*/ 
          Compile(class System.Runtime.CompilerServices.DebugInfoGenerator/*0200000B*/ debugInfoGenerator) cil managed
  // SIG: 20 01 12 80 D1 12 2C
  {
    // Method begins at RVA 0x27ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LambdaExpression::Compile

  .property /*17000059*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Body()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.LambdaExpression/*0200002B*/::get_Body() /* 06000237 */
  } // end of property LambdaExpression::Body
  .property /*1700005A*/ instance callconv(8) string
          Name()
  {
    .custom /*0C0000E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Linq.Expressions.LambdaExpression/*0200002B*/::get_Name() /* 06000238 */
  } // end of property LambdaExpression::Name
  .property /*1700005B*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.LambdaExpression/*0200002B*/::get_NodeType() /* 06000239 */
  } // end of property LambdaExpression::NodeType
  .property /*1700005C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/>
          Parameters()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> System.Linq.Expressions.LambdaExpression/*0200002B*/::get_Parameters() /* 0600023A */
  } // end of property LambdaExpression::Parameters
  .property /*1700005D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.LambdaExpression/*0200002B*/::get_ReturnType() /* 0600023B */
  } // end of property LambdaExpression::ReturnType
  .property /*1700005E*/ instance callconv(8) bool
          TailCall()
  {
    .get instance bool System.Linq.Expressions.LambdaExpression/*0200002B*/::get_TailCall() /* 0600023C */
  } // end of property LambdaExpression::TailCall
  .property /*1700005F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.LambdaExpression/*0200002B*/::get_Type() /* 0600023D */
  } // end of property LambdaExpression::Type
} // end of class System.Linq.Expressions.LambdaExpression

.class /*0200002C*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.ListInitExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C00008F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000090:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000241*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27af
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ListInitExpression::.ctor

  .method /*06000242*/ public hidebysig specialname virtual 
          instance bool  get_CanReduce() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x27b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ListInitExpression::get_CanReduce

  .method /*06000243*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ElementInit/*0200001E*/> 
          get_Initializers() cil managed
  // SIG: 20 00 15 12 5D 01 12 78
  {
    // Method begins at RVA 0x27ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ListInitExpression::get_Initializers

  .method /*06000244*/ public hidebysig specialname 
          instance class System.Linq.Expressions.NewExpression/*02000037*/ 
          get_NewExpression() cil managed
  // SIG: 20 00 12 80 DC
  {
    // Method begins at RVA 0x27bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ListInitExpression::get_NewExpression

  .method /*06000245*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x27c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ListInitExpression::get_NodeType

  .method /*06000246*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x27c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ListInitExpression::get_Type

  .method /*06000247*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x27c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ListInitExpression::Accept

  .method /*06000248*/ public hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Reduce() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x27c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ListInitExpression::Reduce

  .method /*06000249*/ public hidebysig instance class System.Linq.Expressions.ListInitExpression/*0200002C*/ 
          Update(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ElementInit/*0200001E*/> initializers) cil managed
  // SIG: 20 02 12 80 B0 12 80 DC 15 12 80 89 01 12 78
  {
    // Method begins at RVA 0x27cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ListInitExpression::Update

  .property /*17000060*/ instance callconv(8) bool
          CanReduce()
  {
    .get instance bool System.Linq.Expressions.ListInitExpression/*0200002C*/::get_CanReduce() /* 06000242 */
  } // end of property ListInitExpression::CanReduce
  .property /*17000061*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ElementInit/*0200001E*/>
          Initializers()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ElementInit/*0200001E*/> System.Linq.Expressions.ListInitExpression/*0200002C*/::get_Initializers() /* 06000243 */
  } // end of property ListInitExpression::Initializers
  .property /*17000062*/ instance callconv(8) class System.Linq.Expressions.NewExpression/*02000037*/
          NewExpression()
  {
    .get instance class System.Linq.Expressions.NewExpression/*02000037*/ System.Linq.Expressions.ListInitExpression/*0200002C*/::get_NewExpression() /* 06000244 */
  } // end of property ListInitExpression::NewExpression
  .property /*17000063*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.ListInitExpression/*0200002C*/::get_NodeType() /* 06000245 */
  } // end of property ListInitExpression::NodeType
  .property /*17000064*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.ListInitExpression/*0200002C*/::get_Type() /* 06000246 */
  } // end of property ListInitExpression::Type
} // end of class System.Linq.Expressions.ListInitExpression

.class /*0200002D*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.LoopExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C000091:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600024A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27cf
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoopExpression::.ctor

  .method /*0600024B*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Body() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x27d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoopExpression::get_Body

  .method /*0600024C*/ public hidebysig specialname 
          instance class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          get_BreakLabel() cil managed
  // SIG: 20 00 12 80 A8
  {
    .custom /*0C0001A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoopExpression::get_BreakLabel

  .method /*0600024D*/ public hidebysig specialname 
          instance class System.Linq.Expressions.LabelTarget/*0200002A*/ 
          get_ContinueLabel() cil managed
  // SIG: 20 00 12 80 A8
  {
    .custom /*0C0001A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoopExpression::get_ContinueLabel

  .method /*0600024E*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x27e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoopExpression::get_NodeType

  .method /*0600024F*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x27e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoopExpression::get_Type

  .method /*06000250*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x27e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoopExpression::Accept

  .method /*06000251*/ public hidebysig instance class System.Linq.Expressions.LoopExpression/*0200002D*/ 
          Update(class System.Linq.Expressions.LabelTarget/*0200002A*/ breakLabel,
                 class System.Linq.Expressions.LabelTarget/*0200002A*/ continueLabel,
                 class System.Linq.Expressions.Expression/*0200001F*/ body) cil managed
  // SIG: 20 03 12 80 B4 12 80 A8 12 80 A8 12 7C
  {
    .param [1]/*080003D2*/ 
    .custom /*0C000236:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080003D3*/ 
    .custom /*0C000237:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoopExpression::Update

  .property /*17000065*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Body()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.LoopExpression/*0200002D*/::get_Body() /* 0600024B */
  } // end of property LoopExpression::Body
  .property /*17000066*/ instance callconv(8) class System.Linq.Expressions.LabelTarget/*0200002A*/
          BreakLabel()
  {
    .custom /*0C0000E9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.LabelTarget/*0200002A*/ System.Linq.Expressions.LoopExpression/*0200002D*/::get_BreakLabel() /* 0600024C */
  } // end of property LoopExpression::BreakLabel
  .property /*17000067*/ instance callconv(8) class System.Linq.Expressions.LabelTarget/*0200002A*/
          ContinueLabel()
  {
    .custom /*0C0000EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.LabelTarget/*0200002A*/ System.Linq.Expressions.LoopExpression/*0200002D*/::get_ContinueLabel() /* 0600024D */
  } // end of property LoopExpression::ContinueLabel
  .property /*17000068*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.LoopExpression/*0200002D*/::get_NodeType() /* 0600024E */
  } // end of property LoopExpression::NodeType
  .property /*17000069*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.LoopExpression/*0200002D*/::get_Type() /* 0600024F */
  } // end of property LoopExpression::Type
} // end of class System.Linq.Expressions.LoopExpression

.class /*0200002E*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.MemberAssignment
       extends System.Linq.Expressions.MemberBinding/*0200002F*/
{
  .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000252*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27ec
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (06)000255       */ call       instance void System.Linq.Expressions.MemberBinding/*0200002F*/::.ctor(valuetype System.Linq.Expressions.MemberBindingType/*02000030*/,
                                                                                                                              class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/) /* 06000255 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method MemberAssignment::.ctor

  .method /*06000253*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Expression() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x27f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberAssignment::get_Expression

  .method /*06000254*/ public hidebysig instance class System.Linq.Expressions.MemberAssignment/*0200002E*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 20 01 12 80 B8 12 7C
  {
    // Method begins at RVA 0x27f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberAssignment::Update

  .property /*1700006A*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Expression()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.MemberAssignment/*0200002E*/::get_Expression() /* 06000253 */
  } // end of property MemberAssignment::Expression
} // end of class System.Linq.Expressions.MemberAssignment

.class /*0200002F*/ public abstract auto ansi beforefieldinit System.Linq.Expressions.MemberBinding
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000096:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000255*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Linq.Expressions.MemberBindingType/*02000030*/ 'type',
                               class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ member) cil managed
  // SIG: 20 02 01 11 80 C0 12 80 A1
  {
    .custom /*0C0001AB:0A000012*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000012 */ = ( 01 00 3A 54 68 69 73 20 63 6F 6E 73 74 72 75 63   // ..:This construc
                                                                                                                                                   74 6F 72 20 68 61 73 20 62 65 65 6E 20 64 65 70   // tor has been dep
                                                                                                                                                   72 65 63 61 74 65 64 20 61 6E 64 20 69 73 20 6E   // recated and is n
                                                                                                                                                   6F 74 20 73 75 70 70 6F 72 74 65 64 2E 00 00 )    // ot supported...
    // Method begins at RVA 0x27fc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MemberBinding::.ctor

  .method /*06000256*/ public hidebysig specialname 
          instance valuetype System.Linq.Expressions.MemberBindingType/*02000030*/ 
          get_BindingType() cil managed
  // SIG: 20 00 11 80 C0
  {
    // Method begins at RVA 0x2804
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberBinding::get_BindingType

  .method /*06000257*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ 
          get_Member() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x2807
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberBinding::get_Member

  .method /*06000258*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x280a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberBinding::ToString

  .property /*1700006B*/ instance callconv(8) valuetype System.Linq.Expressions.MemberBindingType/*02000030*/
          BindingType()
  {
    .get instance valuetype System.Linq.Expressions.MemberBindingType/*02000030*/ System.Linq.Expressions.MemberBinding/*0200002F*/::get_BindingType() /* 06000256 */
  } // end of property MemberBinding::BindingType
  .property /*1700006C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/
          Member()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ System.Linq.Expressions.MemberBinding/*0200002F*/::get_Member() /* 06000257 */
  } // end of property MemberBinding::Member
} // end of class System.Linq.Expressions.MemberBinding

.class /*02000030*/ public auto ansi sealed System.Linq.Expressions.MemberBindingType
       extends [System.Runtime/*23000001*/]System.Enum/*01000031*/
{
  .field /*04000061*/ public specialname rtspecialname int32 value__
  .field /*04000062*/ public static literal valuetype System.Linq.Expressions.MemberBindingType/*02000030*/ Assignment = int32(0x00000000)
  .field /*04000063*/ public static literal valuetype System.Linq.Expressions.MemberBindingType/*02000030*/ MemberBinding = int32(0x00000001)
  .field /*04000064*/ public static literal valuetype System.Linq.Expressions.MemberBindingType/*02000030*/ ListBinding = int32(0x00000002)
} // end of class System.Linq.Expressions.MemberBindingType

.class /*02000031*/ public auto ansi beforefieldinit System.Linq.Expressions.MemberExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C000097:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000098:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000259*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x280d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MemberExpression::.ctor

  .method /*0600025A*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Expression() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C0001AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2815
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberExpression::get_Expression

  .method /*0600025B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ 
          get_Member() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x2818
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberExpression::get_Member

  .method /*0600025C*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x281b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberExpression::get_NodeType

  .method /*0600025D*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x281e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberExpression::Accept

  .method /*0600025E*/ public hidebysig instance class System.Linq.Expressions.MemberExpression/*02000031*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 20 01 12 80 C4 12 7C
  {
    .param [1]/*080003D9*/ 
    .custom /*0C000238:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2821
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberExpression::Update

  .property /*1700006D*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Expression()
  {
    .custom /*0C0000EC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.MemberExpression/*02000031*/::get_Expression() /* 0600025A */
  } // end of property MemberExpression::Expression
  .property /*1700006E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/
          Member()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/ System.Linq.Expressions.MemberExpression/*02000031*/::get_Member() /* 0600025B */
  } // end of property MemberExpression::Member
  .property /*1700006F*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.MemberExpression/*02000031*/::get_NodeType() /* 0600025C */
  } // end of property MemberExpression::NodeType
} // end of class System.Linq.Expressions.MemberExpression

.class /*02000032*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.MemberInitExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600025F*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2824
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MemberInitExpression::.ctor

  .method /*06000260*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> 
          get_Bindings() cil managed
  // SIG: 20 00 15 12 5D 01 12 80 BC
  {
    // Method begins at RVA 0x282c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInitExpression::get_Bindings

  .method /*06000261*/ public hidebysig specialname virtual 
          instance bool  get_CanReduce() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x282f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInitExpression::get_CanReduce

  .method /*06000262*/ public hidebysig specialname 
          instance class System.Linq.Expressions.NewExpression/*02000037*/ 
          get_NewExpression() cil managed
  // SIG: 20 00 12 80 DC
  {
    // Method begins at RVA 0x2832
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInitExpression::get_NewExpression

  .method /*06000263*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2835
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInitExpression::get_NodeType

  .method /*06000264*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2838
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInitExpression::get_Type

  .method /*06000265*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x283b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInitExpression::Accept

  .method /*06000266*/ public hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Reduce() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x283e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInitExpression::Reduce

  .method /*06000267*/ public hidebysig instance class System.Linq.Expressions.MemberInitExpression/*02000032*/ 
          Update(class System.Linq.Expressions.NewExpression/*02000037*/ newExpression,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> bindings) cil managed
  // SIG: 20 02 12 80 C8 12 80 DC 15 12 80 89 01 12 80 BC
  {
    // Method begins at RVA 0x2841
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInitExpression::Update

  .property /*17000070*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.MemberBinding/*0200002F*/>
          Bindings()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> System.Linq.Expressions.MemberInitExpression/*02000032*/::get_Bindings() /* 06000260 */
  } // end of property MemberInitExpression::Bindings
  .property /*17000071*/ instance callconv(8) bool
          CanReduce()
  {
    .get instance bool System.Linq.Expressions.MemberInitExpression/*02000032*/::get_CanReduce() /* 06000261 */
  } // end of property MemberInitExpression::CanReduce
  .property /*17000072*/ instance callconv(8) class System.Linq.Expressions.NewExpression/*02000037*/
          NewExpression()
  {
    .get instance class System.Linq.Expressions.NewExpression/*02000037*/ System.Linq.Expressions.MemberInitExpression/*02000032*/::get_NewExpression() /* 06000262 */
  } // end of property MemberInitExpression::NewExpression
  .property /*17000073*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.MemberInitExpression/*02000032*/::get_NodeType() /* 06000263 */
  } // end of property MemberInitExpression::NodeType
  .property /*17000074*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.MemberInitExpression/*02000032*/::get_Type() /* 06000264 */
  } // end of property MemberInitExpression::Type
} // end of class System.Linq.Expressions.MemberInitExpression

.class /*02000033*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.MemberListBinding
       extends System.Linq.Expressions.MemberBinding/*0200002F*/
{
  .custom /*0C00009C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000268*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2844
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (06)000255       */ call       instance void System.Linq.Expressions.MemberBinding/*0200002F*/::.ctor(valuetype System.Linq.Expressions.MemberBindingType/*02000030*/,
                                                                                                                              class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/) /* 06000255 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method MemberListBinding::.ctor

  .method /*06000269*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ElementInit/*0200001E*/> 
          get_Initializers() cil managed
  // SIG: 20 00 15 12 5D 01 12 78
  {
    // Method begins at RVA 0x284e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberListBinding::get_Initializers

  .method /*0600026A*/ public hidebysig instance class System.Linq.Expressions.MemberListBinding/*02000033*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ElementInit/*0200001E*/> initializers) cil managed
  // SIG: 20 01 12 80 CC 15 12 80 89 01 12 78
  {
    // Method begins at RVA 0x2851
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberListBinding::Update

  .property /*17000075*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ElementInit/*0200001E*/>
          Initializers()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ElementInit/*0200001E*/> System.Linq.Expressions.MemberListBinding/*02000033*/::get_Initializers() /* 06000269 */
  } // end of property MemberListBinding::Initializers
} // end of class System.Linq.Expressions.MemberListBinding

.class /*02000034*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.MemberMemberBinding
       extends System.Linq.Expressions.MemberBinding/*0200002F*/
{
  .custom /*0C00009E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600026B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2854
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (06)000255       */ call       instance void System.Linq.Expressions.MemberBinding/*0200002F*/::.ctor(valuetype System.Linq.Expressions.MemberBindingType/*02000030*/,
                                                                                                                              class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/) /* 06000255 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method MemberMemberBinding::.ctor

  .method /*0600026C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> 
          get_Bindings() cil managed
  // SIG: 20 00 15 12 5D 01 12 80 BC
  {
    // Method begins at RVA 0x285e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberMemberBinding::get_Bindings

  .method /*0600026D*/ public hidebysig instance class System.Linq.Expressions.MemberMemberBinding/*02000034*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> bindings) cil managed
  // SIG: 20 01 12 80 D0 15 12 80 89 01 12 80 BC
  {
    // Method begins at RVA 0x2861
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberMemberBinding::Update

  .property /*17000076*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.MemberBinding/*0200002F*/>
          Bindings()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.MemberBinding/*0200002F*/> System.Linq.Expressions.MemberMemberBinding/*02000034*/::get_Bindings() /* 0600026C */
  } // end of property MemberMemberBinding::Bindings
} // end of class System.Linq.Expressions.MemberMemberBinding

.class /*02000035*/ public auto ansi beforefieldinit System.Linq.Expressions.MethodCallExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
       implements System.Linq.Expressions.IArgumentProvider/*02000025*/
{
  .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Linq.Expressions.IArgumentProvider/*02000025*/
  .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600026E*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2864
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MethodCallExpression::.ctor

  .method /*0600026F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_Arguments() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x286c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::get_Arguments

  .method /*06000270*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 
          get_Method() cil managed
  // SIG: 20 00 12 80 99
  {
    // Method begins at RVA 0x286f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::get_Method

  .method /*06000271*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2872
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::get_NodeType

  .method /*06000272*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Object() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C0001B7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2875
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::get_Object

  .method /*06000273*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() cil managed
  // SIG: 20 00 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::get_ArgumentCount /*02000025::06000213*/ 
    // Method begins at RVA 0x2878
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount

  .method /*06000274*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x287b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::get_Type

  .method /*06000275*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x287e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::Accept

  .method /*06000276*/ private hidebysig newslot virtual final 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          System.Linq.Expressions.IArgumentProvider.GetArgument(int32 index) cil managed
  // SIG: 20 01 12 7C 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::GetArgument /*02000025::06000214*/ 
    // Method begins at RVA 0x2881
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::System.Linq.Expressions.IArgumentProvider.GetArgument

  .method /*06000277*/ public hidebysig instance class System.Linq.Expressions.MethodCallExpression/*02000035*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ 'object',
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 20 02 12 80 D4 12 7C 15 12 80 89 01 12 7C
  {
    .param [1]/*080003E1*/ 
    .custom /*0C000239:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080003E2*/ 
    .custom /*0C00023A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2884
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodCallExpression::Update

  .property /*17000077*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          Arguments()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.MethodCallExpression/*02000035*/::get_Arguments() /* 0600026F */
  } // end of property MethodCallExpression::Arguments
  .property /*17000078*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/
          Method()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ System.Linq.Expressions.MethodCallExpression/*02000035*/::get_Method() /* 06000270 */
  } // end of property MethodCallExpression::Method
  .property /*17000079*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.MethodCallExpression/*02000035*/::get_NodeType() /* 06000271 */
  } // end of property MethodCallExpression::NodeType
  .property /*1700007A*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Object()
  {
    .custom /*0C0000F0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.MethodCallExpression/*02000035*/::get_Object() /* 06000272 */
  } // end of property MethodCallExpression::Object
  .property /*1700007B*/ instance callconv(8) int32
          System.Linq.Expressions.IArgumentProvider.ArgumentCount()
  {
    .get instance int32 System.Linq.Expressions.MethodCallExpression/*02000035*/::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() /* 06000273 */
  } // end of property MethodCallExpression::System.Linq.Expressions.IArgumentProvider.ArgumentCount
  .property /*1700007C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.MethodCallExpression/*02000035*/::get_Type() /* 06000274 */
  } // end of property MethodCallExpression::Type
} // end of class System.Linq.Expressions.MethodCallExpression

.class /*02000036*/ public auto ansi beforefieldinit System.Linq.Expressions.NewArrayExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000278*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2887
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NewArrayExpression::.ctor

  .method /*06000279*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_Expressions() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x288f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewArrayExpression::get_Expressions

  .method /*0600027A*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2892
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewArrayExpression::get_Type

  .method /*0600027B*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2895
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewArrayExpression::Accept

  .method /*0600027C*/ public hidebysig instance class System.Linq.Expressions.NewArrayExpression/*02000036*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> expressions) cil managed
  // SIG: 20 01 12 80 D8 15 12 80 89 01 12 7C
  {
    // Method begins at RVA 0x2898
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewArrayExpression::Update

  .property /*1700007D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          Expressions()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.NewArrayExpression/*02000036*/::get_Expressions() /* 06000279 */
  } // end of property NewArrayExpression::Expressions
  .property /*1700007E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.NewArrayExpression/*02000036*/::get_Type() /* 0600027A */
  } // end of property NewArrayExpression::Type
} // end of class System.Linq.Expressions.NewArrayExpression

.class /*02000037*/ public auto ansi beforefieldinit System.Linq.Expressions.NewExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
       implements System.Linq.Expressions.IArgumentProvider/*02000025*/
{
  .custom /*0C0000A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Linq.Expressions.IArgumentProvider/*02000025*/
  .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600027D*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x289b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NewExpression::.ctor

  .method /*0600027E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_Arguments() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x28a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::get_Arguments

  .method /*0600027F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100002E*/ 
          get_Constructor() cil managed
  // SIG: 20 00 12 80 B9
  {
    .custom /*0C0001BA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x28a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::get_Constructor

  .method /*06000280*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/> 
          get_Members() cil managed
  // SIG: 20 00 15 12 5D 01 12 80 A1
  {
    .param [0]/*080003E5*/ 
    .custom /*0C00023B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x28a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::get_Members

  .method /*06000281*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x28ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::get_NodeType

  .method /*06000282*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() cil managed
  // SIG: 20 00 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::get_ArgumentCount /*02000025::06000213*/ 
    // Method begins at RVA 0x28af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount

  .method /*06000283*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x28b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::get_Type

  .method /*06000284*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x28b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::Accept

  .method /*06000285*/ private hidebysig newslot virtual final 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          System.Linq.Expressions.IArgumentProvider.GetArgument(int32 index) cil managed
  // SIG: 20 01 12 7C 08
  {
    .override System.Linq.Expressions.IArgumentProvider/*02000025*/::GetArgument /*02000025::06000214*/ 
    // Method begins at RVA 0x28b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::System.Linq.Expressions.IArgumentProvider.GetArgument

  .method /*06000286*/ public hidebysig instance class System.Linq.Expressions.NewExpression/*02000037*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> arguments) cil managed
  // SIG: 20 01 12 80 DC 15 12 80 89 01 12 7C
  {
    .param [1]/*080003E8*/ 
    .custom /*0C00023C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x28bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NewExpression::Update

  .property /*1700007F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          Arguments()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.NewExpression/*02000037*/::get_Arguments() /* 0600027E */
  } // end of property NewExpression::Arguments
  .property /*17000080*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100002E*/
          Constructor()
  {
    .custom /*0C0000F1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100002E*/ System.Linq.Expressions.NewExpression/*02000037*/::get_Constructor() /* 0600027F */
  } // end of property NewExpression::Constructor
  .property /*17000081*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/>
          Members()
  {
    .custom /*0C0000F2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000028*/> System.Linq.Expressions.NewExpression/*02000037*/::get_Members() /* 06000280 */
  } // end of property NewExpression::Members
  .property /*17000082*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.NewExpression/*02000037*/::get_NodeType() /* 06000281 */
  } // end of property NewExpression::NodeType
  .property /*17000083*/ instance callconv(8) int32
          System.Linq.Expressions.IArgumentProvider.ArgumentCount()
  {
    .get instance int32 System.Linq.Expressions.NewExpression/*02000037*/::System.Linq.Expressions.IArgumentProvider.get_ArgumentCount() /* 06000282 */
  } // end of property NewExpression::System.Linq.Expressions.IArgumentProvider.ArgumentCount
  .property /*17000084*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.NewExpression/*02000037*/::get_Type() /* 06000283 */
  } // end of property NewExpression::Type
} // end of class System.Linq.Expressions.NewExpression

.class /*02000038*/ public auto ansi beforefieldinit System.Linq.Expressions.ParameterExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C0000A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000287*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x28be
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ParameterExpression::.ctor

  .method /*06000288*/ public hidebysig specialname 
          instance bool  get_IsByRef() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x28c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterExpression::get_IsByRef

  .method /*06000289*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001BF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x28c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterExpression::get_Name

  .method /*0600028A*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x28cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterExpression::get_NodeType

  .method /*0600028B*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x28cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterExpression::get_Type

  .method /*0600028C*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x28d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterExpression::Accept

  .property /*17000085*/ instance callconv(8) bool
          IsByRef()
  {
    .get instance bool System.Linq.Expressions.ParameterExpression/*02000038*/::get_IsByRef() /* 06000288 */
  } // end of property ParameterExpression::IsByRef
  .property /*17000086*/ instance callconv(8) string
          Name()
  {
    .custom /*0C0000F4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Linq.Expressions.ParameterExpression/*02000038*/::get_Name() /* 06000289 */
  } // end of property ParameterExpression::Name
  .property /*17000087*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.ParameterExpression/*02000038*/::get_NodeType() /* 0600028A */
  } // end of property ParameterExpression::NodeType
  .property /*17000088*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.ParameterExpression/*02000038*/::get_Type() /* 0600028B */
  } // end of property ParameterExpression::Type
} // end of class System.Linq.Expressions.ParameterExpression

.class /*02000039*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.RuntimeVariablesExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C0000A9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600028D*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x28d5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RuntimeVariablesExpression::.ctor

  .method /*0600028E*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x28dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeVariablesExpression::get_NodeType

  .method /*0600028F*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x28e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeVariablesExpression::get_Type

  .method /*06000290*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> 
          get_Variables() cil managed
  // SIG: 20 00 15 12 5D 01 12 80 E0
  {
    // Method begins at RVA 0x28e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeVariablesExpression::get_Variables

  .method /*06000291*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x28e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeVariablesExpression::Accept

  .method /*06000292*/ public hidebysig instance class System.Linq.Expressions.RuntimeVariablesExpression/*02000039*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> variables) cil managed
  // SIG: 20 01 12 80 E4 15 12 80 89 01 12 80 E0
  {
    // Method begins at RVA 0x28e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RuntimeVariablesExpression::Update

  .property /*17000089*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.RuntimeVariablesExpression/*02000039*/::get_NodeType() /* 0600028E */
  } // end of property RuntimeVariablesExpression::NodeType
  .property /*1700008A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.RuntimeVariablesExpression/*02000039*/::get_Type() /* 0600028F */
  } // end of property RuntimeVariablesExpression::Type
  .property /*1700008B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/>
          Variables()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> System.Linq.Expressions.RuntimeVariablesExpression/*02000039*/::get_Variables() /* 06000290 */
  } // end of property RuntimeVariablesExpression::Variables
} // end of class System.Linq.Expressions.RuntimeVariablesExpression

.class /*0200003A*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.SwitchCase
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C0000AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000293*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x28ec
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SwitchCase::.ctor

  .method /*06000294*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Body() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x28f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchCase::get_Body

  .method /*06000295*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> 
          get_TestValues() cil managed
  // SIG: 20 00 15 12 5D 01 12 7C
  {
    // Method begins at RVA 0x28f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchCase::get_TestValues

  .method /*06000296*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x28fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchCase::ToString

  .method /*06000297*/ public hidebysig instance class System.Linq.Expressions.SwitchCase/*0200003A*/ 
          Update(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.Expression/*0200001F*/> testValues,
                 class System.Linq.Expressions.Expression/*0200001F*/ body) cil managed
  // SIG: 20 02 12 80 E8 15 12 80 89 01 12 7C 12 7C
  {
    // Method begins at RVA 0x28fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchCase::Update

  .property /*1700008C*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Body()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.SwitchCase/*0200003A*/::get_Body() /* 06000294 */
  } // end of property SwitchCase::Body
  .property /*1700008D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/>
          TestValues()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.Expression/*0200001F*/> System.Linq.Expressions.SwitchCase/*0200003A*/::get_TestValues() /* 06000295 */
  } // end of property SwitchCase::TestValues
} // end of class System.Linq.Expressions.SwitchCase

.class /*0200003B*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.SwitchExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C0000AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000298*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2900
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SwitchExpression::.ctor

  .method /*06000299*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.SwitchCase/*0200003A*/> 
          get_Cases() cil managed
  // SIG: 20 00 15 12 5D 01 12 80 E8
  {
    // Method begins at RVA 0x2908
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchExpression::get_Cases

  .method /*0600029A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 
          get_Comparison() cil managed
  // SIG: 20 00 12 80 99
  {
    .custom /*0C0001C5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x290b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchExpression::get_Comparison

  .method /*0600029B*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_DefaultBody() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C0001C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x290e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchExpression::get_DefaultBody

  .method /*0600029C*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2911
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchExpression::get_NodeType

  .method /*0600029D*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_SwitchValue() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x2914
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchExpression::get_SwitchValue

  .method /*0600029E*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2917
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchExpression::get_Type

  .method /*0600029F*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x291a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchExpression::Accept

  .method /*060002A0*/ public hidebysig instance class System.Linq.Expressions.SwitchExpression/*0200003B*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ switchValue,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.SwitchCase/*0200003A*/> cases,
                 class System.Linq.Expressions.Expression/*0200001F*/ defaultBody) cil managed
  // SIG: 20 03 12 80 EC 12 7C 15 12 80 89 01 12 80 E8 12 7C
  {
    .param [2]/*080003F0*/ 
    .custom /*0C00023D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080003F1*/ 
    .custom /*0C00023E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x291d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SwitchExpression::Update

  .property /*1700008E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.SwitchCase/*0200003A*/>
          Cases()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.SwitchCase/*0200003A*/> System.Linq.Expressions.SwitchExpression/*0200003B*/::get_Cases() /* 06000299 */
  } // end of property SwitchExpression::Cases
  .property /*1700008F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/
          Comparison()
  {
    .custom /*0C0000F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ System.Linq.Expressions.SwitchExpression/*0200003B*/::get_Comparison() /* 0600029A */
  } // end of property SwitchExpression::Comparison
  .property /*17000090*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          DefaultBody()
  {
    .custom /*0C0000F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.SwitchExpression/*0200003B*/::get_DefaultBody() /* 0600029B */
  } // end of property SwitchExpression::DefaultBody
  .property /*17000091*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.SwitchExpression/*0200003B*/::get_NodeType() /* 0600029C */
  } // end of property SwitchExpression::NodeType
  .property /*17000092*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          SwitchValue()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.SwitchExpression/*0200003B*/::get_SwitchValue() /* 0600029D */
  } // end of property SwitchExpression::SwitchValue
  .property /*17000093*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.SwitchExpression/*0200003B*/::get_Type() /* 0600029E */
  } // end of property SwitchExpression::Type
} // end of class System.Linq.Expressions.SwitchExpression

.class /*0200003C*/ public auto ansi beforefieldinit System.Linq.Expressions.SymbolDocumentInfo
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C0000B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002A1*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2920
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SymbolDocumentInfo::.ctor

  .method /*060002A2*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ 
          get_DocumentType() cil managed
  // SIG: 20 00 11 80 BD
  {
    // Method begins at RVA 0x2928
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolDocumentInfo::get_DocumentType

  .method /*060002A3*/ public hidebysig specialname 
          instance string  get_FileName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x292b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolDocumentInfo::get_FileName

  .method /*060002A4*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ 
          get_Language() cil managed
  // SIG: 20 00 11 80 BD
  {
    // Method begins at RVA 0x292e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolDocumentInfo::get_Language

  .method /*060002A5*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ 
          get_LanguageVendor() cil managed
  // SIG: 20 00 11 80 BD
  {
    // Method begins at RVA 0x2931
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolDocumentInfo::get_LanguageVendor

  .property /*17000094*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/
          DocumentType()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/::get_DocumentType() /* 060002A2 */
  } // end of property SymbolDocumentInfo::DocumentType
  .property /*17000095*/ instance callconv(8) string
          FileName()
  {
    .get instance string System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/::get_FileName() /* 060002A3 */
  } // end of property SymbolDocumentInfo::FileName
  .property /*17000096*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/
          Language()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/::get_Language() /* 060002A4 */
  } // end of property SymbolDocumentInfo::Language
  .property /*17000097*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/
          LanguageVendor()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100002F*/ System.Linq.Expressions.SymbolDocumentInfo/*0200003C*/::get_LanguageVendor() /* 060002A5 */
  } // end of property SymbolDocumentInfo::LanguageVendor
} // end of class System.Linq.Expressions.SymbolDocumentInfo

.class /*0200003D*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.TryExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C0000B4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002A6*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2934
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TryExpression::.ctor

  .method /*060002A7*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Body() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x293c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TryExpression::get_Body

  .method /*060002A8*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Fault() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C0001CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x293f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TryExpression::get_Fault

  .method /*060002A9*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Finally() cil managed
  // SIG: 20 00 12 7C
  {
    .custom /*0C0001CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2942
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TryExpression::get_Finally

  .method /*060002AA*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.CatchBlock/*02000017*/> 
          get_Handlers() cil managed
  // SIG: 20 00 15 12 5D 01 12 5C
  {
    // Method begins at RVA 0x2945
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TryExpression::get_Handlers

  .method /*060002AB*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2948
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TryExpression::get_NodeType

  .method /*060002AC*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x294b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TryExpression::get_Type

  .method /*060002AD*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x294e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TryExpression::Accept

  .method /*060002AE*/ public hidebysig instance class System.Linq.Expressions.TryExpression/*0200003D*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ body,
                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<class System.Linq.Expressions.CatchBlock/*02000017*/> handlers,
                 class System.Linq.Expressions.Expression/*0200001F*/ 'finally',
                 class System.Linq.Expressions.Expression/*0200001F*/ 'fault') cil managed
  // SIG: 20 04 12 80 F4 12 7C 15 12 80 89 01 12 5C 12 7C 12 7C
  {
    .param [2]/*080003F4*/ 
    .custom /*0C00023F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080003F5*/ 
    .custom /*0C000240:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080003F6*/ 
    .custom /*0C000241:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2951
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TryExpression::Update

  .property /*17000098*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Body()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.TryExpression/*0200003D*/::get_Body() /* 060002A7 */
  } // end of property TryExpression::Body
  .property /*17000099*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Fault()
  {
    .custom /*0C0000FE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.TryExpression/*0200003D*/::get_Fault() /* 060002A8 */
  } // end of property TryExpression::Fault
  .property /*1700009A*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Finally()
  {
    .custom /*0C000100:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.TryExpression/*0200003D*/::get_Finally() /* 060002A9 */
  } // end of property TryExpression::Finally
  .property /*1700009B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.CatchBlock/*02000017*/>
          Handlers()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.CatchBlock/*02000017*/> System.Linq.Expressions.TryExpression/*0200003D*/::get_Handlers() /* 060002AA */
  } // end of property TryExpression::Handlers
  .property /*1700009C*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.TryExpression/*0200003D*/::get_NodeType() /* 060002AB */
  } // end of property TryExpression::NodeType
  .property /*1700009D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.TryExpression/*0200003D*/::get_Type() /* 060002AC */
  } // end of property TryExpression::Type
} // end of class System.Linq.Expressions.TryExpression

.class /*0200003E*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.TypeBinaryExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C0000B6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002AF*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2954
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeBinaryExpression::.ctor

  .method /*060002B0*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Expression() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x295c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBinaryExpression::get_Expression

  .method /*060002B1*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x295f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBinaryExpression::get_NodeType

  .method /*060002B2*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2962
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBinaryExpression::get_Type

  .method /*060002B3*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_TypeOperand() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2965
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBinaryExpression::get_TypeOperand

  .method /*060002B4*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x2968
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBinaryExpression::Accept

  .method /*060002B5*/ public hidebysig instance class System.Linq.Expressions.TypeBinaryExpression/*0200003E*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 20 01 12 80 F8 12 7C
  {
    // Method begins at RVA 0x296b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBinaryExpression::Update

  .property /*1700009E*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Expression()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.TypeBinaryExpression/*0200003E*/::get_Expression() /* 060002B0 */
  } // end of property TypeBinaryExpression::Expression
  .property /*1700009F*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.TypeBinaryExpression/*0200003E*/::get_NodeType() /* 060002B1 */
  } // end of property TypeBinaryExpression::NodeType
  .property /*170000A0*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.TypeBinaryExpression/*0200003E*/::get_Type() /* 060002B2 */
  } // end of property TypeBinaryExpression::Type
  .property /*170000A1*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          TypeOperand()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.TypeBinaryExpression/*0200003E*/::get_TypeOperand() /* 060002B3 */
  } // end of property TypeBinaryExpression::TypeOperand
} // end of class System.Linq.Expressions.TypeBinaryExpression

.class /*0200003F*/ public auto ansi sealed beforefieldinit System.Linq.Expressions.UnaryExpression
       extends System.Linq.Expressions.Expression/*0200001F*/
{
  .custom /*0C0000B8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002B6*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x296e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00009D       */ call       instance void System.Linq.Expressions.Expression/*0200001F*/::.ctor() /* 0600009D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnaryExpression::.ctor

  .method /*060002B7*/ public hidebysig specialname virtual 
          instance bool  get_CanReduce() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2976
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::get_CanReduce

  .method /*060002B8*/ public hidebysig specialname 
          instance bool  get_IsLifted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2979
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::get_IsLifted

  .method /*060002B9*/ public hidebysig specialname 
          instance bool  get_IsLiftedToNull() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x297c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::get_IsLiftedToNull

  .method /*060002BA*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ 
          get_Method() cil managed
  // SIG: 20 00 12 80 99
  {
    .custom /*0C0001D8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x297f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::get_Method

  .method /*060002BB*/ public hidebysig specialname virtual final 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2982
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::get_NodeType

  .method /*060002BC*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Operand() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x2985
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::get_Operand

  .method /*060002BD*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2988
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::get_Type

  .method /*060002BE*/ famorassem hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Accept(class System.Linq.Expressions.ExpressionVisitor/*02000021*/ visitor) cil managed
  // SIG: 20 01 12 7C 12 80 84
  {
    // Method begins at RVA 0x298b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::Accept

  .method /*060002BF*/ public hidebysig virtual 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Reduce() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x298e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::Reduce

  .method /*060002C0*/ public hidebysig instance class System.Linq.Expressions.UnaryExpression/*0200003F*/ 
          Update(class System.Linq.Expressions.Expression/*0200001F*/ operand) cil managed
  // SIG: 20 01 12 80 FC 12 7C
  {
    // Method begins at RVA 0x2991
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryExpression::Update

  .property /*170000A2*/ instance callconv(8) bool
          CanReduce()
  {
    .get instance bool System.Linq.Expressions.UnaryExpression/*0200003F*/::get_CanReduce() /* 060002B7 */
  } // end of property UnaryExpression::CanReduce
  .property /*170000A3*/ instance callconv(8) bool
          IsLifted()
  {
    .get instance bool System.Linq.Expressions.UnaryExpression/*0200003F*/::get_IsLifted() /* 060002B8 */
  } // end of property UnaryExpression::IsLifted
  .property /*170000A4*/ instance callconv(8) bool
          IsLiftedToNull()
  {
    .get instance bool System.Linq.Expressions.UnaryExpression/*0200003F*/::get_IsLiftedToNull() /* 060002B9 */
  } // end of property UnaryExpression::IsLiftedToNull
  .property /*170000A5*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/
          Method()
  {
    .custom /*0C000104:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000026*/ System.Linq.Expressions.UnaryExpression/*0200003F*/::get_Method() /* 060002BA */
  } // end of property UnaryExpression::Method
  .property /*170000A6*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          NodeType()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Linq.Expressions.UnaryExpression/*0200003F*/::get_NodeType() /* 060002BB */
  } // end of property UnaryExpression::NodeType
  .property /*170000A7*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Operand()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Linq.Expressions.UnaryExpression/*0200003F*/::get_Operand() /* 060002BC */
  } // end of property UnaryExpression::Operand
  .property /*170000A8*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Linq.Expressions.UnaryExpression/*0200003F*/::get_Type() /* 060002BD */
  } // end of property UnaryExpression::Type
} // end of class System.Linq.Expressions.UnaryExpression

.class /*02000040*/ public abstract auto ansi beforefieldinit System.Dynamic.BinaryOperationBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000BA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002C1*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Linq.Expressions.ExpressionType/*02000020*/ operation) cil managed
  // SIG: 20 01 01 11 80 80
  {
    // Method begins at RVA 0x2994
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BinaryOperationBinder::.ctor

  .method /*060002C2*/ public hidebysig specialname 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_Operation() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x299c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryOperationBinder::get_Operation

  .method /*060002C3*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x299f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryOperationBinder::get_ReturnType

  .method /*060002C4*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x29a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryOperationBinder::Bind

  .method /*060002C5*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackBinaryOperation(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                                  class System.Dynamic.DynamicMetaObject/*02000047*/ arg) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 12 81 1C
  {
    // Method begins at RVA 0x29a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryOperationBinder::FallbackBinaryOperation

  .method /*060002C6*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackBinaryOperation(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                                  class System.Dynamic.DynamicMetaObject/*02000047*/ arg,
                                  class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 12 81 1C 12 81 1C
  {
    .param [3]/*08000402*/ 
    .custom /*0C000242:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method BinaryOperationBinder::FallbackBinaryOperation

  .property /*170000A9*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          Operation()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Dynamic.BinaryOperationBinder/*02000040*/::get_Operation() /* 060002C2 */
  } // end of property BinaryOperationBinder::Operation
  .property /*170000AA*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.BinaryOperationBinder/*02000040*/::get_ReturnType() /* 060002C3 */
  } // end of property BinaryOperationBinder::ReturnType
} // end of class System.Dynamic.BinaryOperationBinder

.class /*02000041*/ public abstract auto ansi beforefieldinit System.Dynamic.BindingRestrictions
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C0000BC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000065*/ public static initonly class System.Dynamic.BindingRestrictions/*02000041*/ Empty
  .method /*060002C7*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x29a8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BindingRestrictions::.ctor

  .method /*060002C8*/ public hidebysig static 
          class System.Dynamic.BindingRestrictions/*02000041*/ 
          Combine(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001A*/<class System.Dynamic.DynamicMetaObject/*02000047*/> contributingObjects) cil managed
  // SIG: 00 01 12 81 04 15 12 69 01 12 81 1C
  {
    .param [1]/*08000403*/ 
    .custom /*0C000243:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x29b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindingRestrictions::Combine

  .method /*060002C9*/ public hidebysig static 
          class System.Dynamic.BindingRestrictions/*02000041*/ 
          GetExpressionRestriction(class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 01 12 81 04 12 7C
  {
    // Method begins at RVA 0x29b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindingRestrictions::GetExpressionRestriction

  .method /*060002CA*/ public hidebysig static 
          class System.Dynamic.BindingRestrictions/*02000041*/ 
          GetInstanceRestriction(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                                 object 'instance') cil managed
  // SIG: 00 02 12 81 04 12 7C 1C
  {
    .param [2]/*08000406*/ 
    .custom /*0C000244:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x29b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindingRestrictions::GetInstanceRestriction

  .method /*060002CB*/ public hidebysig static 
          class System.Dynamic.BindingRestrictions/*02000041*/ 
          GetTypeRestriction(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                             class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 00 02 12 81 04 12 7C 12 59
  {
    // Method begins at RVA 0x29b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindingRestrictions::GetTypeRestriction

  .method /*060002CC*/ public hidebysig instance class System.Dynamic.BindingRestrictions/*02000041*/ 
          Merge(class System.Dynamic.BindingRestrictions/*02000041*/ restrictions) cil managed
  // SIG: 20 01 12 81 04 12 81 04
  {
    // Method begins at RVA 0x29bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindingRestrictions::Merge

  .method /*060002CD*/ public hidebysig instance class System.Linq.Expressions.Expression/*0200001F*/ 
          ToExpression() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x29bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BindingRestrictions::ToExpression

} // end of class System.Dynamic.BindingRestrictions

.class /*02000042*/ public auto ansi sealed beforefieldinit System.Dynamic.CallInfo
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C0000BE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002CE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 argCount,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<string> argNames) cil managed
  // SIG: 20 02 01 08 15 12 80 89 01 0E
  {
    // Method begins at RVA 0x29c2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CallInfo::.ctor

  .method /*060002CF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 argCount,
                               string[] argNames) cil managed
  // SIG: 20 02 01 08 1D 0E
  {
    .param [2]/*0800040D*/ 
    .custom /*0C000245:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x29ca
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CallInfo::.ctor

  .method /*060002D0*/ public hidebysig specialname 
          instance int32  get_ArgumentCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x29d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallInfo::get_ArgumentCount

  .method /*060002D1*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<string> 
          get_ArgumentNames() cil managed
  // SIG: 20 00 15 12 5D 01 0E
  {
    // Method begins at RVA 0x29d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallInfo::get_ArgumentNames

  .method /*060002D2*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0001DE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800040E*/ 
    .custom /*0C000246:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000026 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallInfo::Equals

  .method /*060002D3*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x29db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CallInfo::GetHashCode

  .property /*170000AB*/ instance callconv(8) int32
          ArgumentCount()
  {
    .get instance int32 System.Dynamic.CallInfo/*02000042*/::get_ArgumentCount() /* 060002D0 */
  } // end of property CallInfo::ArgumentCount
  .property /*170000AC*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<string>
          ArgumentNames()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<string> System.Dynamic.CallInfo/*02000042*/::get_ArgumentNames() /* 060002D1 */
  } // end of property CallInfo::ArgumentNames
} // end of class System.Dynamic.CallInfo

.class /*02000043*/ public abstract auto ansi beforefieldinit System.Dynamic.ConvertBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000C0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002D4*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type',
                               bool 'explicit') cil managed
  // SIG: 20 02 01 12 59 02
  {
    // Method begins at RVA 0x29de
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConvertBinder::.ctor

  .method /*060002D5*/ public hidebysig specialname 
          instance bool  get_Explicit() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x29e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConvertBinder::get_Explicit

  .method /*060002D6*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x29e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConvertBinder::get_ReturnType

  .method /*060002D7*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_Type() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x29ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConvertBinder::get_Type

  .method /*060002D8*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    .param [2]/*08000412*/ 
    .custom /*0C000247:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x29ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConvertBinder::Bind

  .method /*060002D9*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackConvert(class System.Dynamic.DynamicMetaObject/*02000047*/ target) cil managed
  // SIG: 20 01 12 81 1C 12 81 1C
  {
    // Method begins at RVA 0x29f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConvertBinder::FallbackConvert

  .method /*060002DA*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackConvert(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                          class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 12 81 1C
  {
    .param [2]/*08000415*/ 
    .custom /*0C000248:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ConvertBinder::FallbackConvert

  .property /*170000AD*/ instance callconv(8) bool
          Explicit()
  {
    .get instance bool System.Dynamic.ConvertBinder/*02000043*/::get_Explicit() /* 060002D5 */
  } // end of property ConvertBinder::Explicit
  .property /*170000AE*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.ConvertBinder/*02000043*/::get_ReturnType() /* 060002D6 */
  } // end of property ConvertBinder::ReturnType
  .property /*170000AF*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          Type()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.ConvertBinder/*02000043*/::get_Type() /* 060002D7 */
  } // end of property ConvertBinder::Type
} // end of class System.Dynamic.ConvertBinder

.class /*02000044*/ public abstract auto ansi beforefieldinit System.Dynamic.CreateInstanceBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000C2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002DB*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Dynamic.CallInfo/*02000042*/ callInfo) cil managed
  // SIG: 20 01 01 12 81 08
  {
    // Method begins at RVA 0x29f5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CreateInstanceBinder::.ctor

  .method /*060002DC*/ public hidebysig specialname 
          instance class System.Dynamic.CallInfo/*02000042*/ 
          get_CallInfo() cil managed
  // SIG: 20 00 12 81 08
  {
    // Method begins at RVA 0x29fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreateInstanceBinder::get_CallInfo

  .method /*060002DD*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2a00
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreateInstanceBinder::get_ReturnType

  .method /*060002DE*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2a03
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreateInstanceBinder::Bind

  .method /*060002DF*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackCreateInstance(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                                 class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2a06
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreateInstanceBinder::FallbackCreateInstance

  .method /*060002E0*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackCreateInstance(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                                 class System.Dynamic.DynamicMetaObject/*02000047*/[] args,
                                 class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 1D 12 81 1C 12 81 1C
  {
    .param [3]/*0800041D*/ 
    .custom /*0C000249:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method CreateInstanceBinder::FallbackCreateInstance

  .property /*170000B0*/ instance callconv(8) class System.Dynamic.CallInfo/*02000042*/
          CallInfo()
  {
    .get instance class System.Dynamic.CallInfo/*02000042*/ System.Dynamic.CreateInstanceBinder/*02000044*/::get_CallInfo() /* 060002DC */
  } // end of property CreateInstanceBinder::CallInfo
  .property /*170000B1*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.CreateInstanceBinder/*02000044*/::get_ReturnType() /* 060002DD */
  } // end of property CreateInstanceBinder::ReturnType
} // end of class System.Dynamic.CreateInstanceBinder

.class /*02000045*/ public abstract auto ansi beforefieldinit System.Dynamic.DeleteIndexBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000C4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002E1*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Dynamic.CallInfo/*02000042*/ callInfo) cil managed
  // SIG: 20 01 01 12 81 08
  {
    // Method begins at RVA 0x2a09
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeleteIndexBinder::.ctor

  .method /*060002E2*/ public hidebysig specialname 
          instance class System.Dynamic.CallInfo/*02000042*/ 
          get_CallInfo() cil managed
  // SIG: 20 00 12 81 08
  {
    // Method begins at RVA 0x2a11
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteIndexBinder::get_CallInfo

  .method /*060002E3*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2a14
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteIndexBinder::get_ReturnType

  .method /*060002E4*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2a17
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteIndexBinder::Bind

  .method /*060002E5*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackDeleteIndex(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                              class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2a1a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteIndexBinder::FallbackDeleteIndex

  .method /*060002E6*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackDeleteIndex(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                              class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes,
                              class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 1D 12 81 1C 12 81 1C
  {
    .param [3]/*08000425*/ 
    .custom /*0C00024A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DeleteIndexBinder::FallbackDeleteIndex

  .property /*170000B2*/ instance callconv(8) class System.Dynamic.CallInfo/*02000042*/
          CallInfo()
  {
    .get instance class System.Dynamic.CallInfo/*02000042*/ System.Dynamic.DeleteIndexBinder/*02000045*/::get_CallInfo() /* 060002E2 */
  } // end of property DeleteIndexBinder::CallInfo
  .property /*170000B3*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.DeleteIndexBinder/*02000045*/::get_ReturnType() /* 060002E3 */
  } // end of property DeleteIndexBinder::ReturnType
} // end of class System.Dynamic.DeleteIndexBinder

.class /*02000046*/ public abstract auto ansi beforefieldinit System.Dynamic.DeleteMemberBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060002E7*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               bool ignoreCase) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x2a1d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeleteMemberBinder::.ctor

  .method /*060002E8*/ public hidebysig specialname 
          instance bool  get_IgnoreCase() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2a25
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteMemberBinder::get_IgnoreCase

  .method /*060002E9*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2a28
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteMemberBinder::get_Name

  .method /*060002EA*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2a2b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteMemberBinder::get_ReturnType

  .method /*060002EB*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    .param [2]/*08000429*/ 
    .custom /*0C00024B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2a2e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteMemberBinder::Bind

  .method /*060002EC*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackDeleteMember(class System.Dynamic.DynamicMetaObject/*02000047*/ target) cil managed
  // SIG: 20 01 12 81 1C 12 81 1C
  {
    // Method begins at RVA 0x2a31
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeleteMemberBinder::FallbackDeleteMember

  .method /*060002ED*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackDeleteMember(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                               class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 12 81 1C
  {
    .param [2]/*0800042C*/ 
    .custom /*0C00024C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DeleteMemberBinder::FallbackDeleteMember

  .property /*170000B4*/ instance callconv(8) bool
          IgnoreCase()
  {
    .get instance bool System.Dynamic.DeleteMemberBinder/*02000046*/::get_IgnoreCase() /* 060002E8 */
  } // end of property DeleteMemberBinder::IgnoreCase
  .property /*170000B5*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Dynamic.DeleteMemberBinder/*02000046*/::get_Name() /* 060002E9 */
  } // end of property DeleteMemberBinder::Name
  .property /*170000B6*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.DeleteMemberBinder/*02000046*/::get_ReturnType() /* 060002EA */
  } // end of property DeleteMemberBinder::ReturnType
} // end of class System.Dynamic.DeleteMemberBinder

.class /*02000047*/ public auto ansi beforefieldinit System.Dynamic.DynamicMetaObject
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C0000C9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000066*/ public static initonly class System.Dynamic.DynamicMetaObject/*02000047*/[] EmptyMetaObjects
  .method /*060002EE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                               class System.Dynamic.BindingRestrictions/*02000041*/ restrictions) cil managed
  // SIG: 20 02 01 12 7C 12 81 04
  {
    // Method begins at RVA 0x2a34
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicMetaObject::.ctor

  .method /*060002EF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Linq.Expressions.Expression/*0200001F*/ expression,
                               class System.Dynamic.BindingRestrictions/*02000041*/ restrictions,
                               object 'value') cil managed
  // SIG: 20 03 01 12 7C 12 81 04 1C
  {
    // Method begins at RVA 0x2a3c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicMetaObject::.ctor

  .method /*060002F0*/ public hidebysig specialname 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          get_Expression() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x2a44
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::get_Expression

  .method /*060002F1*/ public hidebysig specialname 
          instance bool  get_HasValue() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2a47
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::get_HasValue

  .method /*060002F2*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_LimitType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2a4a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::get_LimitType

  .method /*060002F3*/ public hidebysig specialname 
          instance class System.Dynamic.BindingRestrictions/*02000041*/ 
          get_Restrictions() cil managed
  // SIG: 20 00 12 81 04
  {
    // Method begins at RVA 0x2a4d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::get_Restrictions

  .method /*060002F4*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_RuntimeType() cil managed
  // SIG: 20 00 12 59
  {
    .custom /*0C0001EA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2a50
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::get_RuntimeType

  .method /*060002F5*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0001EB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2a53
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::get_Value

  .method /*060002F6*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindBinaryOperation(class System.Dynamic.BinaryOperationBinder/*02000040*/ binder,
                              class System.Dynamic.DynamicMetaObject/*02000047*/ arg) cil managed
  // SIG: 20 02 12 81 1C 12 81 00 12 81 1C
  {
    // Method begins at RVA 0x2a56
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindBinaryOperation

  .method /*060002F7*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindConvert(class System.Dynamic.ConvertBinder/*02000043*/ binder) cil managed
  // SIG: 20 01 12 81 1C 12 81 0C
  {
    // Method begins at RVA 0x2a59
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindConvert

  .method /*060002F8*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindCreateInstance(class System.Dynamic.CreateInstanceBinder/*02000044*/ binder,
                             class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 10 1D 12 81 1C
  {
    // Method begins at RVA 0x2a5c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindCreateInstance

  .method /*060002F9*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindDeleteIndex(class System.Dynamic.DeleteIndexBinder/*02000045*/ binder,
                          class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes) cil managed
  // SIG: 20 02 12 81 1C 12 81 14 1D 12 81 1C
  {
    // Method begins at RVA 0x2a5f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindDeleteIndex

  .method /*060002FA*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindDeleteMember(class System.Dynamic.DeleteMemberBinder/*02000046*/ binder) cil managed
  // SIG: 20 01 12 81 1C 12 81 18
  {
    // Method begins at RVA 0x2a62
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindDeleteMember

  .method /*060002FB*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindGetIndex(class System.Dynamic.GetIndexBinder/*0200004B*/ binder,
                       class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes) cil managed
  // SIG: 20 02 12 81 1C 12 81 2C 1D 12 81 1C
  {
    // Method begins at RVA 0x2a65
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindGetIndex

  .method /*060002FC*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindGetMember(class System.Dynamic.GetMemberBinder/*0200004C*/ binder) cil managed
  // SIG: 20 01 12 81 1C 12 81 30
  {
    // Method begins at RVA 0x2a68
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindGetMember

  .method /*060002FD*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindInvoke(class System.Dynamic.InvokeBinder/*0200004F*/ binder,
                     class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 3C 1D 12 81 1C
  {
    // Method begins at RVA 0x2a6b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindInvoke

  .method /*060002FE*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindInvokeMember(class System.Dynamic.InvokeMemberBinder/*02000050*/ binder,
                           class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 40 1D 12 81 1C
  {
    // Method begins at RVA 0x2a6e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindInvokeMember

  .method /*060002FF*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindSetIndex(class System.Dynamic.SetIndexBinder/*02000051*/ binder,
                       class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes,
                       class System.Dynamic.DynamicMetaObject/*02000047*/ 'value') cil managed
  // SIG: 20 03 12 81 1C 12 81 44 1D 12 81 1C 12 81 1C
  {
    // Method begins at RVA 0x2a71
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindSetIndex

  .method /*06000300*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindSetMember(class System.Dynamic.SetMemberBinder/*02000052*/ binder,
                        class System.Dynamic.DynamicMetaObject/*02000047*/ 'value') cil managed
  // SIG: 20 02 12 81 1C 12 81 48 12 81 1C
  {
    // Method begins at RVA 0x2a74
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindSetMember

  .method /*06000301*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          BindUnaryOperation(class System.Dynamic.UnaryOperationBinder/*02000053*/ binder) cil managed
  // SIG: 20 01 12 81 1C 12 81 4C
  {
    // Method begins at RVA 0x2a77
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::BindUnaryOperation

  .method /*06000302*/ public hidebysig static 
          class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Create(object 'value',
                 class System.Linq.Expressions.Expression/*0200001F*/ expression) cil managed
  // SIG: 00 02 12 81 1C 1C 12 7C
  {
    // Method begins at RVA 0x2a7a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::Create

  .method /*06000303*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<string> 
          GetDynamicMemberNames() cil managed
  // SIG: 20 00 15 12 80 89 01 0E
  {
    // Method begins at RVA 0x2a7d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObject::GetDynamicMemberNames

  .property /*170000B7*/ instance callconv(8) class System.Linq.Expressions.Expression/*0200001F*/
          Expression()
  {
    .get instance class System.Linq.Expressions.Expression/*0200001F*/ System.Dynamic.DynamicMetaObject/*02000047*/::get_Expression() /* 060002F0 */
  } // end of property DynamicMetaObject::Expression
  .property /*170000B8*/ instance callconv(8) bool
          HasValue()
  {
    .get instance bool System.Dynamic.DynamicMetaObject/*02000047*/::get_HasValue() /* 060002F1 */
  } // end of property DynamicMetaObject::HasValue
  .property /*170000B9*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          LimitType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.DynamicMetaObject/*02000047*/::get_LimitType() /* 060002F2 */
  } // end of property DynamicMetaObject::LimitType
  .property /*170000BA*/ instance callconv(8) class System.Dynamic.BindingRestrictions/*02000041*/
          Restrictions()
  {
    .get instance class System.Dynamic.BindingRestrictions/*02000041*/ System.Dynamic.DynamicMetaObject/*02000047*/::get_Restrictions() /* 060002F3 */
  } // end of property DynamicMetaObject::Restrictions
  .property /*170000BB*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          RuntimeType()
  {
    .custom /*0C00010C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.DynamicMetaObject/*02000047*/::get_RuntimeType() /* 060002F4 */
  } // end of property DynamicMetaObject::RuntimeType
  .property /*170000BC*/ instance callconv(8) object
          Value()
  {
    .custom /*0C00010E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Dynamic.DynamicMetaObject/*02000047*/::get_Value() /* 060002F5 */
  } // end of property DynamicMetaObject::Value
} // end of class System.Dynamic.DynamicMetaObject

.class /*02000048*/ public abstract auto ansi beforefieldinit System.Dynamic.DynamicMetaObjectBinder
       extends System.Runtime.CompilerServices.CallSiteBinder/*02000008*/
{
  .custom /*0C0000CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000304*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2a80
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000A       */ call       instance void System.Runtime.CompilerServices.CallSiteBinder/*02000008*/::.ctor() /* 0600000A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicMetaObjectBinder::.ctor

  .method /*06000305*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2a88
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObjectBinder::get_ReturnType

  .method /*06000306*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x0
  } // end of method DynamicMetaObjectBinder::Bind

  .method /*06000307*/ public hidebysig virtual final 
          instance class System.Linq.Expressions.Expression/*0200001F*/ 
          Bind(object[] args,
               class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000017*/<class System.Linq.Expressions.ParameterExpression/*02000038*/> parameters,
               class System.Linq.Expressions.LabelTarget/*0200002A*/ returnLabel) cil managed
  // SIG: 20 03 12 7C 1D 1C 15 12 5D 01 12 80 E0 12 80 A8
  {
    // Method begins at RVA 0x2a8b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObjectBinder::Bind

  .method /*06000308*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Defer(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    .param [2]/*0800044F*/ 
    .custom /*0C00024D:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00024E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2a8e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObjectBinder::Defer

  .method /*06000309*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Defer(class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 01 12 81 1C 1D 12 81 1C
  {
    .param [1]/*08000450*/ 
    .custom /*0C00024F:0A000023*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000027*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2a91
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObjectBinder::Defer

  .method /*0600030A*/ public hidebysig instance class System.Linq.Expressions.Expression/*0200001F*/ 
          GetUpdateExpression(class [System.Runtime/*23000001*/]System.Type/*01000016*/ 'type') cil managed
  // SIG: 20 01 12 7C 12 59
  {
    // Method begins at RVA 0x2a94
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicMetaObjectBinder::GetUpdateExpression

  .property /*170000BD*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.DynamicMetaObjectBinder/*02000048*/::get_ReturnType() /* 06000305 */
  } // end of property DynamicMetaObjectBinder::ReturnType
} // end of class System.Dynamic.DynamicMetaObjectBinder

.class /*02000049*/ public auto ansi beforefieldinit System.Dynamic.DynamicObject
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements System.Dynamic.IDynamicMetaObjectProvider/*0200004D*/
{
  .custom /*0C0000CE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Dynamic.IDynamicMetaObjectProvider/*0200004D*/
  .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600030B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2a97
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicObject::.ctor

  .method /*0600030C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<string> 
          GetDynamicMemberNames() cil managed
  // SIG: 20 00 15 12 80 89 01 0E
  {
    // Method begins at RVA 0x2a9f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::GetDynamicMemberNames

  .method /*0600030D*/ public hidebysig newslot virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          GetMetaObject(class System.Linq.Expressions.Expression/*0200001F*/ parameter) cil managed
  // SIG: 20 01 12 81 1C 12 7C
  {
    // Method begins at RVA 0x2aa2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::GetMetaObject

  .method /*0600030E*/ public hidebysig newslot virtual 
          instance bool  TryBinaryOperation(class System.Dynamic.BinaryOperationBinder/*02000040*/ binder,
                                            object arg,
                                            [out] object& result) cil managed
  // SIG: 20 03 02 12 81 00 1C 10 1C
  {
    .param [3]/*08000455*/ 
    .custom /*0C000250:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2aa5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryBinaryOperation

  .method /*0600030F*/ public hidebysig newslot virtual 
          instance bool  TryConvert(class System.Dynamic.ConvertBinder/*02000043*/ binder,
                                    [out] object& result) cil managed
  // SIG: 20 02 02 12 81 0C 10 1C
  {
    .param [2]/*08000457*/ 
    .custom /*0C000251:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2aa8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryConvert

  .method /*06000310*/ public hidebysig newslot virtual 
          instance bool  TryCreateInstance(class System.Dynamic.CreateInstanceBinder/*02000044*/ binder,
                                           object[] args,
                                           [out] object& result) cil managed
  // SIG: 20 03 02 12 81 10 1D 1C 10 1C
  {
    .custom /*0C0001F8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000458*/ 
    .custom /*0C000252:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800045A*/ 
    .custom /*0C000253:0A000026*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000026 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2aab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryCreateInstance

  .method /*06000311*/ public hidebysig newslot virtual 
          instance bool  TryDeleteIndex(class System.Dynamic.DeleteIndexBinder/*02000045*/ binder,
                                        object[] indexes) cil managed
  // SIG: 20 02 02 12 81 14 1D 1C
  {
    // Method begins at RVA 0x2aae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryDeleteIndex

  .method /*06000312*/ public hidebysig newslot virtual 
          instance bool  TryDeleteMember(class System.Dynamic.DeleteMemberBinder/*02000046*/ binder) cil managed
  // SIG: 20 01 02 12 81 18
  {
    // Method begins at RVA 0x2ab1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryDeleteMember

  .method /*06000313*/ public hidebysig newslot virtual 
          instance bool  TryGetIndex(class System.Dynamic.GetIndexBinder/*0200004B*/ binder,
                                     object[] indexes,
                                     [out] object& result) cil managed
  // SIG: 20 03 02 12 81 2C 1D 1C 10 1C
  {
    .param [3]/*08000460*/ 
    .custom /*0C000254:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2ab4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryGetIndex

  .method /*06000314*/ public hidebysig newslot virtual 
          instance bool  TryGetMember(class System.Dynamic.GetMemberBinder/*0200004C*/ binder,
                                      [out] object& result) cil managed
  // SIG: 20 02 02 12 81 30 10 1C
  {
    .param [2]/*08000462*/ 
    .custom /*0C000255:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2ab7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryGetMember

  .method /*06000315*/ public hidebysig newslot virtual 
          instance bool  TryInvoke(class System.Dynamic.InvokeBinder/*0200004F*/ binder,
                                   object[] args,
                                   [out] object& result) cil managed
  // SIG: 20 03 02 12 81 3C 1D 1C 10 1C
  {
    .custom /*0C0001FB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000463*/ 
    .custom /*0C000256:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2aba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryInvoke

  .method /*06000316*/ public hidebysig newslot virtual 
          instance bool  TryInvokeMember(class System.Dynamic.InvokeMemberBinder/*02000050*/ binder,
                                         object[] args,
                                         [out] object& result) cil managed
  // SIG: 20 03 02 12 81 40 1D 1C 10 1C
  {
    .custom /*0C0001FC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000466*/ 
    .custom /*0C000257:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2abd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryInvokeMember

  .method /*06000317*/ public hidebysig newslot virtual 
          instance bool  TrySetIndex(class System.Dynamic.SetIndexBinder/*02000051*/ binder,
                                     object[] indexes,
                                     object 'value') cil managed
  // SIG: 20 03 02 12 81 44 1D 1C 1C
  {
    .param [3]/*0800046B*/ 
    .custom /*0C000258:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2ac0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TrySetIndex

  .method /*06000318*/ public hidebysig newslot virtual 
          instance bool  TrySetMember(class System.Dynamic.SetMemberBinder/*02000052*/ binder,
                                      object 'value') cil managed
  // SIG: 20 02 02 12 81 48 1C
  {
    .param [2]/*0800046D*/ 
    .custom /*0C000259:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2ac3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TrySetMember

  .method /*06000319*/ public hidebysig newslot virtual 
          instance bool  TryUnaryOperation(class System.Dynamic.UnaryOperationBinder/*02000053*/ binder,
                                           [out] object& result) cil managed
  // SIG: 20 02 02 12 81 4C 10 1C
  {
    .param [2]/*0800046F*/ 
    .custom /*0C00025A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2ac6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicObject::TryUnaryOperation

} // end of class System.Dynamic.DynamicObject

.class /*0200004A*/ public auto ansi sealed beforefieldinit System.Dynamic.ExpandoObject
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000007*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/,
                  [System.ObjectModel/*23000002*/]System.ComponentModel.INotifyPropertyChanged/*01000037*/,
                  System.Dynamic.IDynamicMetaObjectProvider/*0200004D*/
{
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/
  .custom /*0C000071:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000007*/
  .custom /*0C000074:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/
  .custom /*0C000077:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
  .method /*0600031A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2ac9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00003B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00003B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExpandoObject::.ctor

  .method /*0600031B*/ private hidebysig newslot specialname virtual final 
          instance int32  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'() cil managed
  // SIG: 20 00 08
  {
    .override  method instance int32 class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/::get_Count() /* 0A000028 */
    // Method begins at RVA 0x2ad1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'

  .method /*0600031C*/ private hidebysig newslot specialname virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_IsReadOnly'() cil managed
  // SIG: 20 00 02
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/::get_IsReadOnly() /* 0A000029 */
    // Method begins at RVA 0x2ad4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_IsReadOnly'

  .method /*0600031D*/ private hidebysig newslot specialname virtual final 
          instance object  'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Item'(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .override  method instance !1 class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/::get_Item(!0) /* 0A00002A */
    // Method begins at RVA 0x2ad7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Item'

  .method /*0600031E*/ private hidebysig newslot specialname virtual final 
          instance void  'System.Collections.Generic.IDictionary<System.String,System.Object>.set_Item'(string key,
                                                                                                        object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/::set_Item(!0,
                                                                                                                                                                !1) /* 0A00002B */
    // Method begins at RVA 0x2ada
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.set_Item'

  .method /*0600031F*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<string> 
          'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Keys'() cil managed
  // SIG: 20 00 15 12 71 01 0E
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/::get_Keys() /* 0A00002C */
    // Method begins at RVA 0x2adc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Keys'

  .method /*06000320*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<object> 
          'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Values'() cil managed
  // SIG: 20 00 15 12 71 01 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<!1> class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/::get_Values() /* 0A00002D */
    // Method begins at RVA 0x2adf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Values'

  .method /*06000321*/ private hidebysig newslot specialname virtual final 
          instance void  System.ComponentModel.INotifyPropertyChanged.add_PropertyChanged(class [System.ObjectModel/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000038*/ 'value') cil managed
  // SIG: 20 01 01 12 80 E1
  {
    .override [System.ObjectModel/*23000002*/]System.ComponentModel.INotifyPropertyChanged/*01000037*/::add_PropertyChanged /*01000037::0A00002E*/ 
    // Method begins at RVA 0x2ae2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExpandoObject::System.ComponentModel.INotifyPropertyChanged.add_PropertyChanged

  .method /*06000322*/ private hidebysig newslot specialname virtual final 
          instance void  System.ComponentModel.INotifyPropertyChanged.remove_PropertyChanged(class [System.ObjectModel/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000038*/ 'value') cil managed
  // SIG: 20 01 01 12 80 E1
  {
    .override [System.ObjectModel/*23000002*/]System.ComponentModel.INotifyPropertyChanged/*01000037*/::remove_PropertyChanged /*01000037::0A00002F*/ 
    // Method begins at RVA 0x2ae4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExpandoObject::System.ComponentModel.INotifyPropertyChanged.remove_PropertyChanged

  .method /*06000323*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object> item) cil managed
  // SIG: 20 01 01 15 11 80 D5 02 0E 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/::Add(!0) /* 0A000030 */
    // Method begins at RVA 0x2ae6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add'

  .method /*06000324*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Clear'() cil managed
  // SIG: 20 00 01
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/::Clear() /* 0A000031 */
    // Method begins at RVA 0x2ae8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Clear'

  .method /*06000325*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 80 D5 02 0E 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/::Contains(!0) /* 0A000032 */
    // Method begins at RVA 0x2aea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains'

  .method /*06000326*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>[] 'array',
                                                                                                                                               int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 80 D5 02 0E 1C 08
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/::CopyTo(!0[],
                                                                                                                                                                                                                                                           int32) /* 0A000033 */
    // Method begins at RVA 0x2aed
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo'

  .method /*06000327*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 80 D5 02 0E 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000005*/::Remove(!0) /* 0A000034 */
    // Method begins at RVA 0x2aef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove'

  .method /*06000328*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.IDictionary<System.String,System.Object>.Add'(string key,
                                                                                                   object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/::Add(!0,
                                                                                                                                                           !1) /* 0A000035 */
    // Method begins at RVA 0x2af2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.Add'

  .method /*06000329*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.String,System.Object>.ContainsKey'(string key) cil managed
  // SIG: 20 01 02 0E
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/::ContainsKey(!0) /* 0A000036 */
    // Method begins at RVA 0x2af4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.ContainsKey'

  .method /*0600032A*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.String,System.Object>.Remove'(string key) cil managed
  // SIG: 20 01 02 0E
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/::Remove(!0) /* 0A000037 */
    // Method begins at RVA 0x2af7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.Remove'

  .method /*0600032B*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue'(string key,
                                                                                                           [out] object& 'value') cil managed
  // SIG: 20 02 02 0E 10 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000036*/<string,object>/*1B000006*/::TryGetValue(!0,
                                                                                                                                                                   !1&) /* 0A000038 */
    // Method begins at RVA 0x2afa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue'

  .method /*0600032C*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 80 8D 01 15 11 80 D5 02 0E 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>>/*1B000007*/::GetEnumerator() /* 0A000039 */
    // Method begins at RVA 0x2afd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'

  .method /*0600032D*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 85
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000020*/::GetEnumerator /*01000020::0A00001C*/ 
    // Method begins at RVA 0x2b00
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::System.Collections.IEnumerable.GetEnumerator

  .method /*0600032E*/ private hidebysig newslot virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          System.Dynamic.IDynamicMetaObjectProvider.GetMetaObject(class System.Linq.Expressions.Expression/*0200001F*/ parameter) cil managed
  // SIG: 20 01 12 81 1C 12 7C
  {
    .override System.Dynamic.IDynamicMetaObjectProvider/*0200004D*/::GetMetaObject /*0200004D::0600033C*/ 
    // Method begins at RVA 0x2b03
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExpandoObject::System.Dynamic.IDynamicMetaObjectProvider.GetMetaObject

  .event /*14000001*/ [System.ObjectModel/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000038*/ System.ComponentModel.INotifyPropertyChanged.PropertyChanged
  {
    .custom /*0C000004:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .addon instance void System.Dynamic.ExpandoObject/*0200004A*/::System.ComponentModel.INotifyPropertyChanged.add_PropertyChanged(class [System.ObjectModel/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000038*/) /* 06000321 */
    .removeon instance void System.Dynamic.ExpandoObject/*0200004A*/::System.ComponentModel.INotifyPropertyChanged.remove_PropertyChanged(class [System.ObjectModel/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000038*/) /* 06000322 */
  } // end of event ExpandoObject::System.ComponentModel.INotifyPropertyChanged.PropertyChanged
  .property /*170000BE*/ instance callconv(8) int32
          'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Count'()
  {
    .get instance int32 System.Dynamic.ExpandoObject/*0200004A*/::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Count'() /* 0600031B */
  } // end of property ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Count'
  .property /*170000BF*/ instance callconv(8) bool
          'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.IsReadOnly'()
  {
    .get instance bool System.Dynamic.ExpandoObject/*0200004A*/::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_IsReadOnly'() /* 0600031C */
  } // end of property ExpandoObject::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.IsReadOnly'
  .property /*170000C0*/ instance callconv(8) object
          'System.Collections.Generic.IDictionary<System.String,System.Object>.Item'(string)
  {
    .custom /*0C000110:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Dynamic.ExpandoObject/*0200004A*/::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Item'(string) /* 0600031D */
    .set instance void System.Dynamic.ExpandoObject/*0200004A*/::'System.Collections.Generic.IDictionary<System.String,System.Object>.set_Item'(string,
                                                                                                                                                object) /* 0600031E */
  } // end of property ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.Item'
  .property /*170000C1*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<string>
          'System.Collections.Generic.IDictionary<System.String,System.Object>.Keys'()
  {
    .custom /*0C000111:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<string> System.Dynamic.ExpandoObject/*0200004A*/::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Keys'() /* 0600031F */
  } // end of property ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.Keys'
  .property /*170000C2*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<object>
          'System.Collections.Generic.IDictionary<System.String,System.Object>.Values'()
  {
    .custom /*0C000113:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100001C*/<object> System.Dynamic.ExpandoObject/*0200004A*/::'System.Collections.Generic.IDictionary<System.String,System.Object>.get_Values'() /* 06000320 */
  } // end of property ExpandoObject::'System.Collections.Generic.IDictionary<System.String,System.Object>.Values'
} // end of class System.Dynamic.ExpandoObject

.class /*0200004B*/ public abstract auto ansi beforefieldinit System.Dynamic.GetIndexBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000D2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600032F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Dynamic.CallInfo/*02000042*/ callInfo) cil managed
  // SIG: 20 01 01 12 81 08
  {
    // Method begins at RVA 0x2b06
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GetIndexBinder::.ctor

  .method /*06000330*/ public hidebysig specialname 
          instance class System.Dynamic.CallInfo/*02000042*/ 
          get_CallInfo() cil managed
  // SIG: 20 00 12 81 08
  {
    // Method begins at RVA 0x2b0e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetIndexBinder::get_CallInfo

  .method /*06000331*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2b11
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetIndexBinder::get_ReturnType

  .method /*06000332*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2b14
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetIndexBinder::Bind

  .method /*06000333*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackGetIndex(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                           class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2b17
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetIndexBinder::FallbackGetIndex

  .method /*06000334*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackGetIndex(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                           class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes,
                           class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 1D 12 81 1C 12 81 1C
  {
    .param [3]/*08000488*/ 
    .custom /*0C00025B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method GetIndexBinder::FallbackGetIndex

  .property /*170000C3*/ instance callconv(8) class System.Dynamic.CallInfo/*02000042*/
          CallInfo()
  {
    .get instance class System.Dynamic.CallInfo/*02000042*/ System.Dynamic.GetIndexBinder/*0200004B*/::get_CallInfo() /* 06000330 */
  } // end of property GetIndexBinder::CallInfo
  .property /*170000C4*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.GetIndexBinder/*0200004B*/::get_ReturnType() /* 06000331 */
  } // end of property GetIndexBinder::ReturnType
} // end of class System.Dynamic.GetIndexBinder

.class /*0200004C*/ public abstract auto ansi beforefieldinit System.Dynamic.GetMemberBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000D4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000D5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000335*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               bool ignoreCase) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x2b1a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GetMemberBinder::.ctor

  .method /*06000336*/ public hidebysig specialname 
          instance bool  get_IgnoreCase() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2b22
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetMemberBinder::get_IgnoreCase

  .method /*06000337*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2b25
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetMemberBinder::get_Name

  .method /*06000338*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2b28
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetMemberBinder::get_ReturnType

  .method /*06000339*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    .param [2]/*0800048C*/ 
    .custom /*0C00025C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2b2b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetMemberBinder::Bind

  .method /*0600033A*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackGetMember(class System.Dynamic.DynamicMetaObject/*02000047*/ target) cil managed
  // SIG: 20 01 12 81 1C 12 81 1C
  {
    // Method begins at RVA 0x2b2e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GetMemberBinder::FallbackGetMember

  .method /*0600033B*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackGetMember(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                            class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 12 81 1C
  {
    .param [2]/*0800048F*/ 
    .custom /*0C00025D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method GetMemberBinder::FallbackGetMember

  .property /*170000C5*/ instance callconv(8) bool
          IgnoreCase()
  {
    .get instance bool System.Dynamic.GetMemberBinder/*0200004C*/::get_IgnoreCase() /* 06000336 */
  } // end of property GetMemberBinder::IgnoreCase
  .property /*170000C6*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Dynamic.GetMemberBinder/*0200004C*/::get_Name() /* 06000337 */
  } // end of property GetMemberBinder::Name
  .property /*170000C7*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.GetMemberBinder/*0200004C*/::get_ReturnType() /* 06000338 */
  } // end of property GetMemberBinder::ReturnType
} // end of class System.Dynamic.GetMemberBinder

.class /*0200004D*/ interface public abstract auto ansi System.Dynamic.IDynamicMetaObjectProvider
{
  .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600033C*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          GetMetaObject(class System.Linq.Expressions.Expression/*0200001F*/ parameter) cil managed
  // SIG: 20 01 12 81 1C 12 7C
  {
    // Method begins at RVA 0x0
  } // end of method IDynamicMetaObjectProvider::GetMetaObject

} // end of class System.Dynamic.IDynamicMetaObjectProvider

.class /*0200004E*/ interface public abstract auto ansi System.Dynamic.IInvokeOnGetBinder
{
  .method /*0600033D*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_InvokeOnGet() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IInvokeOnGetBinder::get_InvokeOnGet

  .property /*170000C8*/ instance callconv(8) bool
          InvokeOnGet()
  {
    .get instance bool System.Dynamic.IInvokeOnGetBinder/*0200004E*/::get_InvokeOnGet() /* 0600033D */
  } // end of property IInvokeOnGetBinder::InvokeOnGet
} // end of class System.Dynamic.IInvokeOnGetBinder

.class /*0200004F*/ public abstract auto ansi beforefieldinit System.Dynamic.InvokeBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000D8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000D9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600033E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Dynamic.CallInfo/*02000042*/ callInfo) cil managed
  // SIG: 20 01 01 12 81 08
  {
    // Method begins at RVA 0x2b31
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvokeBinder::.ctor

  .method /*0600033F*/ public hidebysig specialname 
          instance class System.Dynamic.CallInfo/*02000042*/ 
          get_CallInfo() cil managed
  // SIG: 20 00 12 81 08
  {
    // Method begins at RVA 0x2b39
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeBinder::get_CallInfo

  .method /*06000340*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2b3c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeBinder::get_ReturnType

  .method /*06000341*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2b3f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeBinder::Bind

  .method /*06000342*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackInvoke(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                         class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2b42
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeBinder::FallbackInvoke

  .method /*06000343*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackInvoke(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                         class System.Dynamic.DynamicMetaObject/*02000047*/[] args,
                         class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 1D 12 81 1C 12 81 1C
  {
    .param [3]/*08000498*/ 
    .custom /*0C00025E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method InvokeBinder::FallbackInvoke

  .property /*170000C9*/ instance callconv(8) class System.Dynamic.CallInfo/*02000042*/
          CallInfo()
  {
    .get instance class System.Dynamic.CallInfo/*02000042*/ System.Dynamic.InvokeBinder/*0200004F*/::get_CallInfo() /* 0600033F */
  } // end of property InvokeBinder::CallInfo
  .property /*170000CA*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.InvokeBinder/*0200004F*/::get_ReturnType() /* 06000340 */
  } // end of property InvokeBinder::ReturnType
} // end of class System.Dynamic.InvokeBinder

.class /*02000050*/ public abstract auto ansi beforefieldinit System.Dynamic.InvokeMemberBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000DA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000DB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000344*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               bool ignoreCase,
                               class System.Dynamic.CallInfo/*02000042*/ callInfo) cil managed
  // SIG: 20 03 01 0E 02 12 81 08
  {
    // Method begins at RVA 0x2b45
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvokeMemberBinder::.ctor

  .method /*06000345*/ public hidebysig specialname 
          instance class System.Dynamic.CallInfo/*02000042*/ 
          get_CallInfo() cil managed
  // SIG: 20 00 12 81 08
  {
    // Method begins at RVA 0x2b4d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeMemberBinder::get_CallInfo

  .method /*06000346*/ public hidebysig specialname 
          instance bool  get_IgnoreCase() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2b50
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeMemberBinder::get_IgnoreCase

  .method /*06000347*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2b53
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeMemberBinder::get_Name

  .method /*06000348*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2b56
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeMemberBinder::get_ReturnType

  .method /*06000349*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2b59
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeMemberBinder::Bind

  .method /*0600034A*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackInvoke(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                         class System.Dynamic.DynamicMetaObject/*02000047*/[] args,
                         class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 1D 12 81 1C 12 81 1C
  {
    .param [3]/*080004A0*/ 
    .custom /*0C00025F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method InvokeMemberBinder::FallbackInvoke

  .method /*0600034B*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackInvokeMember(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2b5c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InvokeMemberBinder::FallbackInvokeMember

  .method /*0600034C*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackInvokeMember(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                               class System.Dynamic.DynamicMetaObject/*02000047*/[] args,
                               class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 1D 12 81 1C 12 81 1C
  {
    .param [3]/*080004A5*/ 
    .custom /*0C000260:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method InvokeMemberBinder::FallbackInvokeMember

  .property /*170000CB*/ instance callconv(8) class System.Dynamic.CallInfo/*02000042*/
          CallInfo()
  {
    .get instance class System.Dynamic.CallInfo/*02000042*/ System.Dynamic.InvokeMemberBinder/*02000050*/::get_CallInfo() /* 06000345 */
  } // end of property InvokeMemberBinder::CallInfo
  .property /*170000CC*/ instance callconv(8) bool
          IgnoreCase()
  {
    .get instance bool System.Dynamic.InvokeMemberBinder/*02000050*/::get_IgnoreCase() /* 06000346 */
  } // end of property InvokeMemberBinder::IgnoreCase
  .property /*170000CD*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Dynamic.InvokeMemberBinder/*02000050*/::get_Name() /* 06000347 */
  } // end of property InvokeMemberBinder::Name
  .property /*170000CE*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.InvokeMemberBinder/*02000050*/::get_ReturnType() /* 06000348 */
  } // end of property InvokeMemberBinder::ReturnType
} // end of class System.Dynamic.InvokeMemberBinder

.class /*02000051*/ public abstract auto ansi beforefieldinit System.Dynamic.SetIndexBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000DC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000DD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600034D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Dynamic.CallInfo/*02000042*/ callInfo) cil managed
  // SIG: 20 01 01 12 81 08
  {
    // Method begins at RVA 0x2b5f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SetIndexBinder::.ctor

  .method /*0600034E*/ public hidebysig specialname 
          instance class System.Dynamic.CallInfo/*02000042*/ 
          get_CallInfo() cil managed
  // SIG: 20 00 12 81 08
  {
    // Method begins at RVA 0x2b67
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetIndexBinder::get_CallInfo

  .method /*0600034F*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2b6a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetIndexBinder::get_ReturnType

  .method /*06000350*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2b6d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetIndexBinder::Bind

  .method /*06000351*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackSetIndex(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                           class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes,
                           class System.Dynamic.DynamicMetaObject/*02000047*/ 'value') cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 1D 12 81 1C 12 81 1C
  {
    // Method begins at RVA 0x2b70
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetIndexBinder::FallbackSetIndex

  .method /*06000352*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackSetIndex(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                           class System.Dynamic.DynamicMetaObject/*02000047*/[] indexes,
                           class System.Dynamic.DynamicMetaObject/*02000047*/ 'value',
                           class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 04 12 81 1C 12 81 1C 1D 12 81 1C 12 81 1C 12 81 1C
  {
    .param [4]/*080004AF*/ 
    .custom /*0C000261:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method SetIndexBinder::FallbackSetIndex

  .property /*170000CF*/ instance callconv(8) class System.Dynamic.CallInfo/*02000042*/
          CallInfo()
  {
    .get instance class System.Dynamic.CallInfo/*02000042*/ System.Dynamic.SetIndexBinder/*02000051*/::get_CallInfo() /* 0600034E */
  } // end of property SetIndexBinder::CallInfo
  .property /*170000D0*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.SetIndexBinder/*02000051*/::get_ReturnType() /* 0600034F */
  } // end of property SetIndexBinder::ReturnType
} // end of class System.Dynamic.SetIndexBinder

.class /*02000052*/ public abstract auto ansi beforefieldinit System.Dynamic.SetMemberBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000DE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000DF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000353*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               bool ignoreCase) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x2b73
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SetMemberBinder::.ctor

  .method /*06000354*/ public hidebysig specialname 
          instance bool  get_IgnoreCase() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2b7b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetMemberBinder::get_IgnoreCase

  .method /*06000355*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2b7e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetMemberBinder::get_Name

  .method /*06000356*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2b81
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetMemberBinder::get_ReturnType

  .method /*06000357*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    // Method begins at RVA 0x2b84
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetMemberBinder::Bind

  .method /*06000358*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackSetMember(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                            class System.Dynamic.DynamicMetaObject/*02000047*/ 'value') cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 12 81 1C
  {
    // Method begins at RVA 0x2b87
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SetMemberBinder::FallbackSetMember

  .method /*06000359*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackSetMember(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                            class System.Dynamic.DynamicMetaObject/*02000047*/ 'value',
                            class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 03 12 81 1C 12 81 1C 12 81 1C 12 81 1C
  {
    .param [3]/*080004B8*/ 
    .custom /*0C000262:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method SetMemberBinder::FallbackSetMember

  .property /*170000D1*/ instance callconv(8) bool
          IgnoreCase()
  {
    .get instance bool System.Dynamic.SetMemberBinder/*02000052*/::get_IgnoreCase() /* 06000354 */
  } // end of property SetMemberBinder::IgnoreCase
  .property /*170000D2*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Dynamic.SetMemberBinder/*02000052*/::get_Name() /* 06000355 */
  } // end of property SetMemberBinder::Name
  .property /*170000D3*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.SetMemberBinder/*02000052*/::get_ReturnType() /* 06000356 */
  } // end of property SetMemberBinder::ReturnType
} // end of class System.Dynamic.SetMemberBinder

.class /*02000053*/ public abstract auto ansi beforefieldinit System.Dynamic.UnaryOperationBinder
       extends System.Dynamic.DynamicMetaObjectBinder/*02000048*/
{
  .custom /*0C0000E0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000E1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600035A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Linq.Expressions.ExpressionType/*02000020*/ operation) cil managed
  // SIG: 20 01 01 11 80 80
  {
    // Method begins at RVA 0x2b8a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000304       */ call       instance void System.Dynamic.DynamicMetaObjectBinder/*02000048*/::.ctor() /* 06000304 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UnaryOperationBinder::.ctor

  .method /*0600035B*/ public hidebysig specialname 
          instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ 
          get_Operation() cil managed
  // SIG: 20 00 11 80 80
  {
    // Method begins at RVA 0x2b92
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryOperationBinder::get_Operation

  .method /*0600035C*/ public hidebysig specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2b95
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryOperationBinder::get_ReturnType

  .method /*0600035D*/ public hidebysig virtual final 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          Bind(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
               class System.Dynamic.DynamicMetaObject/*02000047*/[] args) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 1D 12 81 1C
  {
    .param [2]/*080004BB*/ 
    .custom /*0C000263:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2b98
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryOperationBinder::Bind

  .method /*0600035E*/ public hidebysig instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackUnaryOperation(class System.Dynamic.DynamicMetaObject/*02000047*/ target) cil managed
  // SIG: 20 01 12 81 1C 12 81 1C
  {
    // Method begins at RVA 0x2b9b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnaryOperationBinder::FallbackUnaryOperation

  .method /*0600035F*/ public hidebysig newslot abstract virtual 
          instance class System.Dynamic.DynamicMetaObject/*02000047*/ 
          FallbackUnaryOperation(class System.Dynamic.DynamicMetaObject/*02000047*/ target,
                                 class System.Dynamic.DynamicMetaObject/*02000047*/ errorSuggestion) cil managed
  // SIG: 20 02 12 81 1C 12 81 1C 12 81 1C
  {
    .param [2]/*080004BE*/ 
    .custom /*0C000264:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method UnaryOperationBinder::FallbackUnaryOperation

  .property /*170000D4*/ instance callconv(8) valuetype System.Linq.Expressions.ExpressionType/*02000020*/
          Operation()
  {
    .get instance valuetype System.Linq.Expressions.ExpressionType/*02000020*/ System.Dynamic.UnaryOperationBinder/*02000053*/::get_Operation() /* 0600035B */
  } // end of property UnaryOperationBinder::Operation
  .property /*170000D5*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000016*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000016*/ System.Dynamic.UnaryOperationBinder/*02000053*/::get_ReturnType() /* 0600035C */
  } // end of property UnaryOperationBinder::ReturnType
} // end of class System.Dynamic.UnaryOperationBinder


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
