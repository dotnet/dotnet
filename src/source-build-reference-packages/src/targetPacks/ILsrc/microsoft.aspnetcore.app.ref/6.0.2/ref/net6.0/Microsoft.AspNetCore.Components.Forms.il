


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
// Time-date stamp:            0xa7aa4c44
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002400
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00006346
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
// Size of image:                  0x0000c000
// Size of headers:                0x00000400
// Checksum:                       0x0000ae2c
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000062f4 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000534] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003200 [0x00002470] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000062d8 [0x0000001c] address [size] of Debug Directory:           
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
//              0x0000234c Virtual Size
//              0x00004000 Virtual Address
//              0x00002400 Size of Raw Data
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
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002a00 Pointer to Raw Data
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
//              0x00003000 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000348
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00006336 Import Name Table
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
// 0x00004054 [0x00002204] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00006258 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000ec8 Size
//              '#~' Name
//    Stream 2:
//              0x00000f34 Offset
//              0x00000b58 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001a8c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001a90 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001aa0 Offset
//              0x00000764 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000006090bb41757 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 22128
// PE header size       : 1024 (536 used)    ( 4.63%)
// PE additional info   : 10859             (49.07%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.33%)
// CLR meta-data size  : 8708               (39.35%)
// CLR additional info : 128                ( 0.58%)
// CLR method headers  : 91                 ( 0.41%)
// Managed code         : 182               ( 0.82%)
// Data                 : 2560              (11.57%)
// Unaccounted          : -1496             (-6.76%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 9216
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 8708
//   Module        -    1 (10 bytes)
//   TypeDef       -   19 (266 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   42 (252 bytes)
//   MethodDef     -   91 (1274 bytes)    0 abstract, 0 native, 91 bodies
//   FieldDef      -   11 (66 bytes)      2 constant
//   MemberRef     -   32 (192 bytes)
//   ParamDef      -   50 (300 bytes)
//   MethodImpl    -   22 (132 bytes)
//   CustomAttribute-  115 (690 bytes)
//   InterfaceImpl -   18 (72 bytes)
//   PropertyMap   -    9 (36 bytes)
//   Property      -   15 (90 bytes)
//   MethodSemantic-   23 (138 bytes)
//   TypeSpec      -    6 (12 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   NestedClass   -    4 (16 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    3 (18 bytes)
//   GenericParam  -    1 (8 bytes)
//   Strings       -  2902 bytes
//   Blobs         -  1892 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   216 bytes

// CLR additional info : 128

// CLR method headers : 91
//   Num.of method bodies  - 91
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 91

// Managed code : 182
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class DataAnnotationsValidator       (public) (auto) (ansi) 
// Class EditContext                    (public) (auto) (ansi) (sealed) 
// Class EditContextDataAnnotationsExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class EditContextProperties          (public) (auto) (ansi) (sealed) 
// Class FieldChangedEventArgs          (public) (auto) (ansi) (sealed) 
// Class FieldIdentifier                (public) (sequential) (ansi) (sealed) 
// Class FieldState                     (auto) (ansi) 
// Class ValidationMessageStore         (public) (auto) (ansi) (sealed) 
// Class ValidationRequestedEventArgs   (public) (auto) (ansi) (sealed) 
// Class ValidationStateChangedEventArgs (public) (auto) (ansi) (sealed) 
// Class <GetValidationMessages>d__23   (auto) (ansi) (sealed) (nested private) 
// Class <GetValidationMessages>d__24   (auto) (ansi) (sealed) (nested private) 
// Class DataAnnotationsEventSubscriptions (auto) (ansi) (sealed) (nested private) 
// Class <GetValidationMessages>d__7    (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Runtime.Loader
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.AspNetCore.Components
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Linq.Expressions
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Components.Forms
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
  .custom /*0C000008:0A000006*/ instance void [System.Runtime.Loader/*23000002*/]System.Reflection.Metadata.MetadataUpdateHandlerAttribute/*01000008*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000006 */ = ( 01 00 4A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..JMicrosoft.Asp
                                                                                                                                                                                                                                            4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                            74 73 2E 46 6F 72 6D 73 2E 45 64 69 74 43 6F 6E   // ts.Forms.EditCon
                                                                                                                                                                                                                                            74 65 78 74 44 61 74 61 41 6E 6E 6F 74 61 74 69   // textDataAnnotati
                                                                                                                                                                                                                                            6F 6E 73 45 78 74 65 6E 73 69 6F 6E 73 00 00 )    // onsExtensions..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000009*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000C*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000D*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000E*/::.ctor(string) /* 0A00000C */ = ( 01 00 35 46 6F 72 6D 73 20 61 6E 64 20 76 61 6C   // ..5Forms and val
                                                                                                                                                                       69 64 61 74 69 6F 6E 20 73 75 70 70 6F 72 74 20   // idation support 
                                                                                                                                                                       66 6F 72 20 42 6C 61 7A 6F 72 20 61 70 70 6C 69   // for Blazor appli
                                                                                                                                                                       63 61 74 69 6F 6E 73 2E 00 00 )                   // cations...
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000010*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000011*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000012*/::.ctor(string) /* 0A000010 */ = ( 01 00 25 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..%Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                 74 73 2E 46 6F 72 6D 73 00 00 )                   // ts.Forms..
  .custom /*0C000016:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000017:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000013*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.module Microsoft.AspNetCore.Components.Forms.dll
// MVID: {501a7e28-8963-4503-86f9-a57dbd8676b2}
.custom /*0C000001:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000015*/
{
  .custom /*0C000028:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000029:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002A:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000016*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.DataAnnotationsValidator
       extends [Microsoft.AspNetCore.Components/*23000003*/]Microsoft.AspNetCore.Components.ComponentBase/*01000018*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
{
  .method /*06000006*/ family hidebysig virtual 
          instance void  OnInitialized() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataAnnotationsValidator::OnInitialized

  .method /*06000007*/ family hidebysig virtual 
          instance void  OnParametersSet() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataAnnotationsValidator::OnParametersSet

  .method /*06000008*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataAnnotationsValidator::Dispose

  .method /*06000009*/ private hidebysig newslot virtual final 
          instance void  System.IDisposable.Dispose() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.IDisposable/*01000019*/::Dispose /*01000019::0A000014*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataAnnotationsValidator::System.IDisposable.Dispose

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataAnnotationsValidator::.ctor

} // end of class Microsoft.AspNetCore.Components.Forms.DataAnnotationsValidator

.class /*02000007*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.EditContext
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000010*/ auto ansi sealed nested private beforefieldinit '<GetValidationMessages>d__23'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>/*1B000005*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string>/*1B000004*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
  {
    .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000008*/ public class Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/ '<>4__this'
    .method /*06000042*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C00005F:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__23'::.ctor

    .method /*06000043*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000060:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000019*/::Dispose /*01000019::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__23'::System.IDisposable.Dispose

    .method /*06000044*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::MoveNext /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__23'::MoveNext

    .method /*06000045*/ private hidebysig newslot specialname virtual final 
            instance string  'System.Collections.Generic.IEnumerator<System.String>.get_Current'() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C000061:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string>/*1B000004*/::get_Current() /* 0A000018 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__23'::'System.Collections.Generic.IEnumerator<System.String>.get_Current'

    .method /*06000046*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000062:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::Reset /*01000021::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__23'::System.Collections.IEnumerator.Reset

    .method /*06000047*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C000063:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::get_Current /*01000021::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__23'::System.Collections.IEnumerator.get_Current

    .method /*06000048*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string> 
            'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 89 01 0E
    {
      .custom /*0C000064:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>/*1B000005*/::GetEnumerator() /* 0A00001B */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__23'::'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'

    .method /*06000049*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 85
    {
      .custom /*0C000065:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/::GetEnumerator /*01000023::0A00001C*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__23'::System.Collections.IEnumerable.GetEnumerator

    .property /*1700000A*/ instance callconv(8) string
            'System.Collections.Generic.IEnumerator<System.String>.Current'()
    {
      .get instance string Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*//'<GetValidationMessages>d__23'/*02000010*/::'System.Collections.Generic.IEnumerator<System.String>.get_Current'() /* 06000045 */
    } // end of property '<GetValidationMessages>d__23'::'System.Collections.Generic.IEnumerator<System.String>.Current'
    .property /*1700000B*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*//'<GetValidationMessages>d__23'/*02000010*/::System.Collections.IEnumerator.get_Current() /* 06000047 */
    } // end of property '<GetValidationMessages>d__23'::System.Collections.IEnumerator.Current
  } // end of class '<GetValidationMessages>d__23'

  .class /*02000011*/ auto ansi sealed nested private beforefieldinit '<GetValidationMessages>d__24'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>/*1B000005*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string>/*1B000004*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
  {
    .custom /*0C000049:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000009*/ public class Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/ '<>4__this'
    .field /*0400000A*/ public valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ '<>3__fieldIdentifier'
    .method /*0600004A*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C000066:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__24'::.ctor

    .method /*0600004B*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000067:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000019*/::Dispose /*01000019::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__24'::System.IDisposable.Dispose

    .method /*0600004C*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::MoveNext /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__24'::MoveNext

    .method /*0600004D*/ private hidebysig newslot specialname virtual final 
            instance string  'System.Collections.Generic.IEnumerator<System.String>.get_Current'() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C000068:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string>/*1B000004*/::get_Current() /* 0A000018 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__24'::'System.Collections.Generic.IEnumerator<System.String>.get_Current'

    .method /*0600004E*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000069:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::Reset /*01000021::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__24'::System.Collections.IEnumerator.Reset

    .method /*0600004F*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C00006A:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::get_Current /*01000021::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__24'::System.Collections.IEnumerator.get_Current

    .method /*06000050*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string> 
            'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 89 01 0E
    {
      .custom /*0C00006B:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>/*1B000005*/::GetEnumerator() /* 0A00001B */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__24'::'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'

    .method /*06000051*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 85
    {
      .custom /*0C00006C:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/::GetEnumerator /*01000023::0A00001C*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__24'::System.Collections.IEnumerable.GetEnumerator

    .property /*1700000C*/ instance callconv(8) string
            'System.Collections.Generic.IEnumerator<System.String>.Current'()
    {
      .get instance string Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*//'<GetValidationMessages>d__24'/*02000011*/::'System.Collections.Generic.IEnumerator<System.String>.get_Current'() /* 0600004D */
    } // end of property '<GetValidationMessages>d__24'::'System.Collections.Generic.IEnumerator<System.String>.Current'
    .property /*1700000D*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*//'<GetValidationMessages>d__24'/*02000011*/::System.Collections.IEnumerator.get_Current() /* 0600004F */
    } // end of property '<GetValidationMessages>d__24'::System.Collections.IEnumerator.Current
  } // end of class '<GetValidationMessages>d__24'

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object model) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::.ctor

  .method /*0600000C*/ public hidebysig specialname 
          instance void  add_OnFieldChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.FieldChangedEventArgs/*0200000A*/> 'value') cil managed
  // SIG: 20 01 01 15 12 6D 01 12 28
  {
    .custom /*0C00003E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000003*/ 
    .custom /*0C000020:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::add_OnFieldChanged

  .method /*0600000D*/ public hidebysig specialname 
          instance void  remove_OnFieldChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.FieldChangedEventArgs/*0200000A*/> 'value') cil managed
  // SIG: 20 01 01 15 12 6D 01 12 28
  {
    .custom /*0C00003F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000004*/ 
    .custom /*0C000026:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::remove_OnFieldChanged

  .method /*0600000E*/ public hidebysig specialname 
          instance void  add_OnValidationRequested(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationRequestedEventArgs/*0200000E*/> 'value') cil managed
  // SIG: 20 01 01 15 12 6D 01 12 38
  {
    .custom /*0C000043:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000005*/ 
    .custom /*0C00002B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::add_OnValidationRequested

  .method /*0600000F*/ public hidebysig specialname 
          instance void  remove_OnValidationRequested(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationRequestedEventArgs/*0200000E*/> 'value') cil managed
  // SIG: 20 01 01 15 12 6D 01 12 38
  {
    .custom /*0C000045:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000006*/ 
    .custom /*0C00002D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::remove_OnValidationRequested

  .method /*06000010*/ public hidebysig specialname 
          instance void  add_OnValidationStateChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationStateChangedEventArgs/*0200000F*/> 'value') cil managed
  // SIG: 20 01 01 15 12 6D 01 12 3C
  {
    .custom /*0C000046:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000007*/ 
    .custom /*0C000031:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::add_OnValidationStateChanged

  .method /*06000011*/ public hidebysig specialname 
          instance void  remove_OnValidationStateChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationStateChangedEventArgs/*0200000F*/> 'value') cil managed
  // SIG: 20 01 01 15 12 6D 01 12 3C
  {
    .custom /*0C000048:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000008*/ 
    .custom /*0C000035:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::remove_OnValidationStateChanged

  .method /*06000012*/ public hidebysig instance valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ 
          Field(string fieldName) cil managed
  // SIG: 20 01 11 2C 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::Field

  .method /*06000013*/ public hidebysig specialname 
          instance object  get_Model() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00004A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::get_Model

  .method /*06000014*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Components.Forms.EditContextProperties/*02000009*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 24
  {
    .custom /*0C00004C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::get_Properties

  .method /*06000015*/ public hidebysig instance void 
          NotifyFieldChanged([in] valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/& fieldIdentifier) cil managed
  // SIG: 20 01 01 10 11 2C
  {
    .param [1]/*0800000A*/ 
    .custom /*0C000039:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001C*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::NotifyFieldChanged

  .method /*06000016*/ public hidebysig instance void 
          NotifyValidationStateChanged() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::NotifyValidationStateChanged

  .method /*06000017*/ public hidebysig instance void 
          MarkAsUnmodified([in] valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/& fieldIdentifier) cil managed
  // SIG: 20 01 01 10 11 2C
  {
    .param [1]/*0800000B*/ 
    .custom /*0C00003D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001C*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::MarkAsUnmodified

  .method /*06000018*/ public hidebysig instance void 
          MarkAsUnmodified() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::MarkAsUnmodified

  .method /*06000019*/ public hidebysig instance bool 
          IsModified() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::IsModified

  .method /*0600001A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> 
          GetValidationMessages() cil managed
  // SIG: 20 00 15 12 79 01 0E
  {
    .custom /*0C000050:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*0100001D*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000016 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                           4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                           74 73 2E 46 6F 72 6D 73 2E 45 64 69 74 43 6F 6E   // ts.Forms.EditCon
                                                                                                                                                                                                                                           74 65 78 74 2B 3C 47 65 74 56 61 6C 69 64 61 74   // text+<GetValidat
                                                                                                                                                                                                                                           69 6F 6E 4D 65 73 73 61 67 65 73 3E 64 5F 5F 32   // ionMessages>d__2
                                                                                                                                                                                                                                           33 00 00 )                                        // 3..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::GetValidationMessages

  .method /*0600001B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> 
          GetValidationMessages(valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ fieldIdentifier) cil managed
  // SIG: 20 01 15 12 79 01 0E 11 2C
  {
    .custom /*0C000051:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*0100001D*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000016 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                           4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                           74 73 2E 46 6F 72 6D 73 2E 45 64 69 74 43 6F 6E   // ts.Forms.EditCon
                                                                                                                                                                                                                                           74 65 78 74 2B 3C 47 65 74 56 61 6C 69 64 61 74   // text+<GetValidat
                                                                                                                                                                                                                                           69 6F 6E 4D 65 73 73 61 67 65 73 3E 64 5F 5F 32   // ionMessages>d__2
                                                                                                                                                                                                                                           34 00 00 )                                        // 4..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::GetValidationMessages

  .method /*0600001C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> 
          GetValidationMessages(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<object>> accessor) cil managed
  // SIG: 20 01 15 12 79 01 0E 15 12 7D 01 15 12 80 81 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::GetValidationMessages

  .method /*0600001D*/ public hidebysig instance bool 
          IsModified([in] valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/& fieldIdentifier) cil managed
  // SIG: 20 01 02 10 11 2C
  {
    .param [1]/*0800000E*/ 
    .custom /*0C000044:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001C*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::IsModified

  .method /*0600001E*/ public hidebysig instance bool 
          IsModified(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<object>> accessor) cil managed
  // SIG: 20 01 02 15 12 7D 01 15 12 80 81 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::IsModified

  .method /*0600001F*/ public hidebysig instance bool 
          Validate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContext::Validate

  .event /*14000001*/ class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.FieldChangedEventArgs/*0200000A*/>/*1B000001*/ OnFieldChanged
  {
    .custom /*0C000002:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .addon instance void Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/::add_OnFieldChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.FieldChangedEventArgs/*0200000A*/>) /* 0600000C */
    .removeon instance void Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/::remove_OnFieldChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.FieldChangedEventArgs/*0200000A*/>) /* 0600000D */
  } // end of event EditContext::OnFieldChanged
  .event /*14000002*/ class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationRequestedEventArgs/*0200000E*/>/*1B000002*/ OnValidationRequested
  {
    .custom /*0C00001C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .addon instance void Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/::add_OnValidationRequested(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationRequestedEventArgs/*0200000E*/>) /* 0600000E */
    .removeon instance void Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/::remove_OnValidationRequested(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationRequestedEventArgs/*0200000E*/>) /* 0600000F */
  } // end of event EditContext::OnValidationRequested
  .event /*14000003*/ class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationStateChangedEventArgs/*0200000F*/>/*1B000003*/ OnValidationStateChanged
  {
    .custom /*0C000021:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .addon instance void Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/::add_OnValidationStateChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationStateChangedEventArgs/*0200000F*/>) /* 06000010 */
    .removeon instance void Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/::remove_OnValidationStateChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001B*/<class Microsoft.AspNetCore.Components.Forms.ValidationStateChangedEventArgs/*0200000F*/>) /* 06000011 */
  } // end of event EditContext::OnValidationStateChanged
  .property /*17000001*/ instance callconv(8) object
          Model()
  {
    .get instance object Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/::get_Model() /* 06000013 */
  } // end of property EditContext::Model
  .property /*17000002*/ instance callconv(8) class Microsoft.AspNetCore.Components.Forms.EditContextProperties/*02000009*/
          Properties()
  {
    .get instance class Microsoft.AspNetCore.Components.Forms.EditContextProperties/*02000009*/ Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/::get_Properties() /* 06000014 */
  } // end of property EditContext::Properties
} // end of class Microsoft.AspNetCore.Components.Forms.EditContext

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.EditContextDataAnnotationsExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000034:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000012*/ auto ansi sealed nested private beforefieldinit DataAnnotationsEventSubscriptions
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
         implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
  {
    .method /*06000052*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/ editContext) cil managed
    // SIG: 20 01 01 12 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DataAnnotationsEventSubscriptions::.ctor

    .method /*06000053*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DataAnnotationsEventSubscriptions::Dispose

  } // end of class DataAnnotationsEventSubscriptions

  .method /*06000020*/ public hidebysig static 
          class Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/ 
          AddDataAnnotationsValidation(class Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/ editContext) cil managed
  // SIG: 00 01 12 1C 12 1C
  {
    .custom /*0C000052:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000053:0A00001E*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000025*/::.ctor(string) /* 0A00001E */ = ( 01 00 2C 55 73 65 20 45 6E 61 62 6C 65 44 61 74   // ..,Use EnableDat
                                                                                                                                                   61 41 6E 6E 6F 74 61 74 69 6F 6E 73 56 61 6C 69   // aAnnotationsVali
                                                                                                                                                   64 61 74 69 6F 6E 20 69 6E 73 74 65 61 64 2E 00   // dation instead..
                                                                                                                                                   00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextDataAnnotationsExtensions::AddDataAnnotationsValidation

  .method /*06000021*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IDisposable/*01000019*/ 
          EnableDataAnnotationsValidation(class Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/ editContext) cil managed
  // SIG: 00 01 12 65 12 1C
  {
    .custom /*0C000054:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextDataAnnotationsExtensions::EnableDataAnnotationsValidation

} // end of class Microsoft.AspNetCore.Components.Forms.EditContextDataAnnotationsExtensions

.class /*02000009*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.EditContextProperties
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000036:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000038:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000026*/::.ctor(string) /* 0A00001F */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*06000022*/ public hidebysig specialname 
          instance object  get_Item(object key) cil managed
  // SIG: 20 01 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextProperties::get_Item

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_Item(object key,
                                  object 'value') cil managed
  // SIG: 20 02 01 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextProperties::set_Item

  .method /*06000024*/ public hidebysig instance bool 
          TryGetValue(object key,
                      [out] object& 'value') cil managed
  // SIG: 20 02 02 1C 10 1C
  {
    .param [2]/*08000016*/ 
    .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00004E:0A000020*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000027*/::.ctor(bool) /* 0A000020 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextProperties::TryGetValue

  .method /*06000025*/ public hidebysig instance bool 
          Remove(object key) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextProperties::Remove

  .method /*06000026*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditContextProperties::.ctor

  .property /*17000003*/ instance callconv(8) object
          Item(object)
  {
    .get instance object Microsoft.AspNetCore.Components.Forms.EditContextProperties/*02000009*/::get_Item(object) /* 06000022 */
    .set instance void Microsoft.AspNetCore.Components.Forms.EditContextProperties/*02000009*/::set_Item(object,
                                                                                                         object) /* 06000023 */
  } // end of property EditContextProperties::Item
} // end of class Microsoft.AspNetCore.Components.Forms.EditContextProperties

.class /*0200000A*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.FieldChangedEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000028*/
{
  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor([in] valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/& fieldIdentifier) cil managed
  // SIG: 20 01 01 10 11 2C
  {
    .param [1]/*08000018*/ 
    .custom /*0C00004F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001C*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldChangedEventArgs::.ctor

  .method /*06000028*/ public hidebysig specialname 
          instance valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ 
          get_FieldIdentifier() cil managed
  // SIG: 20 00 11 2C
  {
    .custom /*0C000057:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldChangedEventArgs::get_FieldIdentifier

  .property /*17000004*/ instance callconv(8) valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/
          FieldIdentifier()
  {
    .get instance valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ Microsoft.AspNetCore.Components.Forms.FieldChangedEventArgs/*0200000A*/::get_FieldIdentifier() /* 06000028 */
  } // end of property FieldChangedEventArgs::FieldIdentifier
} // end of class Microsoft.AspNetCore.Components.Forms.FieldChangedEventArgs

.class /*0200000B*/ public sequential ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.FieldIdentifier
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002A*/<valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/>/*1B000006*/
{
  .custom /*0C00003A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001C*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002A*/<valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/>/*1B000006*/
  .custom /*0C00001B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ private initonly object '<Model>k__BackingField'
  .custom /*0C000022:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000005*/ private initonly string '<FieldName>k__BackingField'
  .custom /*0C000027:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000029*/ public hidebysig static 
          valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ 
          Create<TField>(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<!!TField>> accessor) cil managed
  // SIG: 10 01 01 11 2C 15 12 7D 01 15 12 80 81 01 1E 00
  {
    .param type TField /*2A000001*/ 
      .custom /*0C000018:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldIdentifier::Create

  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object model,
                               string fieldName) cil managed
  // SIG: 20 02 01 1C 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldIdentifier::.ctor

  .method /*0600002B*/ public hidebysig specialname 
          instance object  get_Model() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C000058:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldIdentifier::get_Model

  .method /*0600002C*/ public hidebysig specialname 
          instance string  get_FieldName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000059:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldIdentifier::get_FieldName

  .method /*0600002D*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldIdentifier::GetHashCode

  .method /*0600002E*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldIdentifier::Equals

  .method /*0600002F*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ otherIdentifier) cil managed
  // SIG: 20 01 02 11 2C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldIdentifier::Equals

  .property /*17000005*/ instance callconv(8) object
          Model()
  {
    .get instance object Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/::get_Model() /* 0600002B */
  } // end of property FieldIdentifier::Model
  .property /*17000006*/ instance callconv(8) string
          FieldName()
  {
    .get instance string Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/::get_FieldName() /* 0600002C */
  } // end of property FieldIdentifier::FieldName
} // end of class Microsoft.AspNetCore.Components.Forms.FieldIdentifier

.class /*0200000C*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Components.Forms.FieldState
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .class /*02000013*/ auto ansi sealed nested private beforefieldinit '<GetValidationMessages>d__7'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>/*1B000005*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string>/*1B000004*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
  {
    .custom /*0C00004B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400000B*/ public class Microsoft.AspNetCore.Components.Forms.FieldState/*0200000C*/ '<>4__this'
    .method /*06000054*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C00006D:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__7'::.ctor

    .method /*06000055*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00006E:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000019*/::Dispose /*01000019::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__7'::System.IDisposable.Dispose

    .method /*06000056*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::MoveNext /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__7'::MoveNext

    .method /*06000057*/ private hidebysig newslot specialname virtual final 
            instance string  'System.Collections.Generic.IEnumerator<System.String>.get_Current'() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C00006F:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string>/*1B000004*/::get_Current() /* 0A000018 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__7'::'System.Collections.Generic.IEnumerator<System.String>.get_Current'

    .method /*06000058*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000070:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::Reset /*01000021::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__7'::System.Collections.IEnumerator.Reset

    .method /*06000059*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C000071:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/::get_Current /*01000021::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__7'::System.Collections.IEnumerator.get_Current

    .method /*0600005A*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<string> 
            'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 89 01 0E
    {
      .custom /*0C000072:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>/*1B000005*/::GetEnumerator() /* 0A00001B */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__7'::'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'

    .method /*0600005B*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000021*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 85
    {
      .custom /*0C000073:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000024*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/::GetEnumerator /*01000023::0A00001C*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetValidationMessages>d__7'::System.Collections.IEnumerable.GetEnumerator

    .property /*1700000E*/ instance callconv(8) string
            'System.Collections.Generic.IEnumerator<System.String>.Current'()
    {
      .get instance string Microsoft.AspNetCore.Components.Forms.FieldState/*0200000C*//'<GetValidationMessages>d__7'/*02000013*/::'System.Collections.Generic.IEnumerator<System.String>.get_Current'() /* 06000057 */
    } // end of property '<GetValidationMessages>d__7'::'System.Collections.Generic.IEnumerator<System.String>.Current'
    .property /*1700000F*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.Components.Forms.FieldState/*0200000C*//'<GetValidationMessages>d__7'/*02000013*/::System.Collections.IEnumerator.get_Current() /* 06000059 */
    } // end of property '<GetValidationMessages>d__7'::System.Collections.IEnumerator.Current
  } // end of class '<GetValidationMessages>d__7'

  .method /*06000030*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ fieldIdentifier) cil managed
  // SIG: 20 01 01 11 2C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldState::.ctor

  .method /*06000031*/ public hidebysig specialname 
          instance bool  get_IsModified() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00005C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldState::get_IsModified

  .method /*06000032*/ public hidebysig specialname 
          instance void  set_IsModified(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00005D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldState::set_IsModified

  .method /*06000033*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> 
          GetValidationMessages() cil managed
  // SIG: 20 00 15 12 79 01 0E
  {
    .custom /*0C00005E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*0100001D*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000016 */ = ( 01 00 4C 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..LMicrosoft.Asp
                                                                                                                                                                                                                                           4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65 6E   // NetCore.Componen
                                                                                                                                                                                                                                           74 73 2E 46 6F 72 6D 73 2E 46 69 65 6C 64 53 74   // ts.Forms.FieldSt
                                                                                                                                                                                                                                           61 74 65 2B 3C 47 65 74 56 61 6C 69 64 61 74 69   // ate+<GetValidati
                                                                                                                                                                                                                                           6F 6E 4D 65 73 73 61 67 65 73 3E 64 5F 5F 37 00   // onMessages>d__7.
                                                                                                                                                                                                                                           00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldState::GetValidationMessages

  .method /*06000034*/ public hidebysig instance void 
          AssociateWithValidationMessageStore(class Microsoft.AspNetCore.Components.Forms.ValidationMessageStore/*0200000D*/ validationMessageStore) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldState::AssociateWithValidationMessageStore

  .method /*06000035*/ public hidebysig instance void 
          DissociateFromValidationMessageStore(class Microsoft.AspNetCore.Components.Forms.ValidationMessageStore/*0200000D*/ validationMessageStore) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldState::DissociateFromValidationMessageStore

  .property /*17000007*/ instance callconv(8) bool
          IsModified()
  {
    .get instance bool Microsoft.AspNetCore.Components.Forms.FieldState/*0200000C*/::get_IsModified() /* 06000031 */
    .set instance void Microsoft.AspNetCore.Components.Forms.FieldState/*0200000C*/::set_IsModified(bool) /* 06000032 */
  } // end of property FieldState::IsModified
} // end of class Microsoft.AspNetCore.Components.Forms.FieldState

.class /*0200000D*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.ValidationMessageStore
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000042:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000026*/::.ctor(string) /* 0A00001F */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*06000036*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Components.Forms.EditContext/*02000007*/ editContext) cil managed
  // SIG: 20 01 01 12 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::.ctor

  .method /*06000037*/ public hidebysig instance void 
          Add([in] valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/& fieldIdentifier,
              string message) cil managed
  // SIG: 20 02 01 10 11 2C 0E
  {
    .param [1]/*08000023*/ 
    .custom /*0C000055:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001C*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::Add

  .method /*06000038*/ public hidebysig instance void 
          Add(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<object>> accessor,
              string message) cil managed
  // SIG: 20 02 01 15 12 7D 01 15 12 80 81 01 1C 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::Add

  .method /*06000039*/ public hidebysig instance void 
          Add([in] valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/& fieldIdentifier,
              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> messages) cil managed
  // SIG: 20 02 01 10 11 2C 15 12 79 01 0E
  {
    .param [1]/*08000027*/ 
    .custom /*0C000056:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001C*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::Add

  .method /*0600003A*/ public hidebysig instance void 
          Add(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<object>> accessor,
              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> messages) cil managed
  // SIG: 20 02 01 15 12 7D 01 15 12 80 81 01 1C 15 12 79 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::Add

  .method /*0600003B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> 
          get_Item(valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/ fieldIdentifier) cil managed
  // SIG: 20 01 15 12 79 01 0E 11 2C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::get_Item

  .method /*0600003C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> 
          get_Item(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<object>> accessor) cil managed
  // SIG: 20 01 15 12 79 01 0E 15 12 7D 01 15 12 80 81 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::get_Item

  .method /*0600003D*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::Clear

  .method /*0600003E*/ public hidebysig instance void 
          Clear(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<object>> accessor) cil managed
  // SIG: 20 01 01 15 12 7D 01 15 12 80 81 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::Clear

  .method /*0600003F*/ public hidebysig instance void 
          Clear([in] valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/& fieldIdentifier) cil managed
  // SIG: 20 01 01 10 11 2C
  {
    .param [1]/*0800002E*/ 
    .custom /*0C00005B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001C*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationMessageStore::Clear

  .property /*17000008*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>
          Item(valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/)
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> Microsoft.AspNetCore.Components.Forms.ValidationMessageStore/*0200000D*/::get_Item(valuetype Microsoft.AspNetCore.Components.Forms.FieldIdentifier/*0200000B*/) /* 0600003B */
  } // end of property ValidationMessageStore::Item
  .property /*17000009*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>
          Item(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<object>>)
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> Microsoft.AspNetCore.Components.Forms.ValidationMessageStore/*0200000D*/::get_Item(class [System.Linq.Expressions/*23000004*/]System.Linq.Expressions.Expression`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<object>>) /* 0600003C */
  } // end of property ValidationMessageStore::Item
} // end of class Microsoft.AspNetCore.Components.Forms.ValidationMessageStore

.class /*0200000E*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.ValidationRequestedEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000028*/
{
  .field /*04000006*/ public static initonly class Microsoft.AspNetCore.Components.Forms.ValidationRequestedEventArgs/*0200000E*/ Empty
  .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*06000040*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationRequestedEventArgs::.ctor

} // end of class Microsoft.AspNetCore.Components.Forms.ValidationRequestedEventArgs

.class /*0200000F*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Components.Forms.ValidationStateChangedEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000028*/
{
  .field /*04000007*/ public static initonly class Microsoft.AspNetCore.Components.Forms.ValidationStateChangedEventArgs/*0200000F*/ Empty
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*06000041*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValidationStateChangedEventArgs::.ctor

} // end of class Microsoft.AspNetCore.Components.Forms.ValidationStateChangedEventArgs


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
