


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
// Time-date stamp:            0xc5dea329
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001800
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000036b2
// Base of code:                   0x00002000
// Base of data:                   0x00004000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00008000
// Size of headers:                0x00000200
// Checksum:                       0x00005335
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000365d [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004c4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002200 [0x00002880] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003568 [0x00000054] address [size] of Debug Directory:           
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
//              0x000016b8 Virtual Size
//              0x00002000 Virtual Address
//              0x00001800 Size of Raw Data
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
//              0x000004c4 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00001a00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00006000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00002000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00003000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000006b4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000369f Import Name Table
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
// 0x00002134 [0x000013b4] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000034e8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x0000087c Size
//              '#~' Name
//    Stream 2:
//              0x000008e8 Offset
//              0x000006bc Size
//              '#Strings' Name
//    Stream 3:
//              0x00000fa4 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000fa8 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000fb8 Offset
//              0x000003fc Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 19072
// PE header size       : 512 (496 used)    ( 2.68%)
// PE additional info   : 11843             (62.10%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.38%)
// CLR meta-data size  : 5044               (26.45%)
// CLR additional info : 128                ( 0.67%)
// CLR method headers  : 47                 ( 0.25%)
// Managed code         : 178               ( 0.93%)
// Data                 : 2048              (10.74%)
// Unaccounted          : -800              (-4.19%)

// Num.of PE sections   : 3
//   .text    - 6144
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 5044
//   Module        -    1 (10 bytes)
//   TypeDef       -    9 (126 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   41 (246 bytes)
//   MethodDef     -   47 (658 bytes)     0 abstract, 0 native, 47 bodies
//   FieldDef      -    8 (48 bytes)      0 constant
//   MemberRef     -   20 (120 bytes)
//   ParamDef      -   74 (444 bytes)
//   Constant      -    2 (12 bytes)
//   CustomAttribute-   45 (270 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    6 (36 bytes)
//   MethodSemantic-    7 (42 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   Strings       -  1722 bytes
//   Blobs         -  1020 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   196 bytes

// CLR additional info : 128

// CLR method headers : 47
//   Num.of method bodies  - 47
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 47

// Managed code : 178
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class BrotliDecoder                  (public) (sequential) (ansi) (sealed) 
// Class BrotliEncoder                  (public) (sequential) (ansi) (sealed) 
// Class BrotliStream                   (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.IO.Compression
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.IO.Compression.Brotli
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 1C 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                        70 72 65 73 73 69 6F 6E 2E 42 72 6F 74 6C 69 00   // pression.Brotli.
                                                                                                                                                                        00 ) 
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 1C 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                       70 72 65 73 73 69 6F 6E 2E 42 72 6F 74 6C 69 00   // pression.Brotli.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 1C 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                 70 72 65 73 73 69 6F 6E 2E 42 72 6F 74 6C 69 00   // pression.Brotli.
                                                                                                                                                                 00 ) 
  .custom /*0C000015:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 7:0:0:0
}
.module System.IO.Compression.Brotli.dll
// MVID: {ab85bf60-6709-4a66-aad7-decc0ae90824}
.custom /*0C000001:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000016:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000029       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000029*/
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000021:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public sequential ansi sealed beforefieldinit System.IO.Compression.BrotliDecoder
       extends [System.Runtime/*23000001*/]System.ValueType/*01000016*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000017*/
{
  .field /*04000005*/ private object _dummy
  .field /*04000006*/ private int32 _dummyPrimitive
  .method /*06000007*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Buffers.OperationStatus/*01000019*/ 
          Decompress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001B*/<uint8> destination,
                     [out] int32& bytesConsumed,
                     [out] int32& bytesWritten) cil managed
  // SIG: 20 04 11 65 15 11 69 01 05 15 11 6D 01 05 10 08 10 08
  {
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliDecoder::Decompress

  .method /*06000008*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliDecoder::Dispose

  .method /*06000009*/ public hidebysig static 
          bool  TryDecompress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                              valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001B*/<uint8> destination,
                              [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 69 01 05 15 11 6D 01 05 10 08
  {
    // Method begins at RVA 0x20b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliDecoder::TryDecompress

} // end of class System.IO.Compression.BrotliDecoder

.class /*02000008*/ public sequential ansi sealed beforefieldinit System.IO.Compression.BrotliEncoder
       extends [System.Runtime/*23000001*/]System.ValueType/*01000016*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000017*/
{
  .field /*04000007*/ private object _dummy
  .field /*04000008*/ private int32 _dummyPrimitive
  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 quality,
                               int32 window) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::.ctor

  .method /*0600000B*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Buffers.OperationStatus/*01000019*/ 
          Compress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                   valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001B*/<uint8> destination,
                   [out] int32& bytesConsumed,
                   [out] int32& bytesWritten,
                   bool isFinalBlock) cil managed
  // SIG: 20 05 11 65 15 11 69 01 05 15 11 6D 01 05 10 08 10 08 02
  {
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::Compress

  .method /*0600000C*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliEncoder::Dispose

  .method /*0600000D*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Buffers.OperationStatus/*01000019*/ 
          Flush(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001B*/<uint8> destination,
                [out] int32& bytesWritten) cil managed
  // SIG: 20 02 11 65 15 11 6D 01 05 10 08
  {
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::Flush

  .method /*0600000E*/ public hidebysig static 
          int32  GetMaxCompressedLength(int32 inputSize) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::GetMaxCompressedLength

  .method /*0600000F*/ public hidebysig static 
          bool  TryCompress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001B*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 69 01 05 15 11 6D 01 05 10 08
  {
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::TryCompress

  .method /*06000010*/ public hidebysig static 
          bool  TryCompress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001B*/<uint8> destination,
                            [out] int32& bytesWritten,
                            int32 quality,
                            int32 window) cil managed
  // SIG: 00 05 02 15 11 69 01 05 15 11 6D 01 05 10 08 08 08
  {
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::TryCompress

} // end of class System.IO.Compression.BrotliEncoder

.class /*02000009*/ public auto ansi sealed beforefieldinit System.IO.Compression.BrotliStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/
{
  .custom /*0C000024:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 'stream',
                               valuetype [System.IO.Compression/*23000002*/]System.IO.Compression.CompressionLevel/*0100001D*/ compressionLevel) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    // Method begins at RVA 0x20c8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*06000012*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 'stream',
                               valuetype [System.IO.Compression/*23000002*/]System.IO.Compression.CompressionLevel/*0100001D*/ compressionLevel,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 71 11 75 02
  {
    // Method begins at RVA 0x20d0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*06000013*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 'stream',
                               valuetype [System.IO.Compression/*23000002*/]System.IO.Compression.CompressionMode/*0100001E*/ mode) cil managed
  // SIG: 20 02 01 12 71 11 79
  {
    // Method begins at RVA 0x20d8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 'stream',
                               valuetype [System.IO.Compression/*23000002*/]System.IO.Compression.CompressionMode/*0100001E*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 71 11 79 02
  {
    // Method begins at RVA 0x20e0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*06000015*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          get_BaseStream() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_BaseStream

  .method /*06000016*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_CanRead

  .method /*06000017*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_CanSeek

  .method /*06000018*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_CanWrite

  .method /*06000019*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_Length

  .method /*0600001A*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_Position

  .method /*0600001B*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x20fa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::set_Position

  .method /*0600001C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*01000020*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 7D 1D 05 08 08 12 80 81 1C
  {
    .param [4]/*08000028*/ 
    .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000029*/ 
    .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::BeginRead

  .method /*0600001D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*01000020*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 7D 1D 05 08 08 12 80 81 1C
  {
    .param [4]/*0800002D*/ 
    .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800002E*/ 
    .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::BeginWrite

  .method /*0600001E*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2102
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::Dispose

  .method /*0600001F*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000021*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::DisposeAsync

  .method /*06000020*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ asyncResult) cil managed
  // SIG: 20 01 08 12 7D
  {
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::EndRead

  .method /*06000021*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ asyncResult) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x210a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::EndWrite

  .method /*06000022*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::Flush

  .method /*06000023*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000022*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 89 11 80 8D
  {
    // Method begins at RVA 0x210e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::FlushAsync

  .method /*06000024*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x2111
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::Read

  .method /*06000025*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001B*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 6D 01 05
  {
    .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::Read

  .method /*06000026*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 91 01 08 1D 05 08 08 11 80 8D
  {
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::ReadAsync

  .method /*06000027*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000025*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000026*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 95 01 08 15 11 80 99 01 05 11 80 8D
  {
    .custom /*0C000027:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800003C*/  = nullref
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::ReadAsync

  .method /*06000028*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::ReadByte

  .method /*06000029*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000027*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 9D
  {
    // Method begins at RVA 0x2120
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::Seek

  .method /*0600002A*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2123
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::SetLength

  .method /*0600002B*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2125
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::Write

  .method /*0600002C*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2127
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::Write

  .method /*0600002D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000022*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 89 1D 05 08 08 11 80 8D
  {
    // Method begins at RVA 0x2129
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::WriteAsync

  .method /*0600002E*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000021*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000028*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 02 11 80 85 15 11 80 A1 01 05 11 80 8D
  {
    .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000049*/  = nullref
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::WriteAsync

  .method /*0600002F*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x212f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::WriteByte

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/
          BaseStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ System.IO.Compression.BrotliStream/*02000009*/::get_BaseStream() /* 06000015 */
  } // end of property BrotliStream::BaseStream
  .property /*17000002*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Compression.BrotliStream/*02000009*/::get_CanRead() /* 06000016 */
  } // end of property BrotliStream::CanRead
  .property /*17000003*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Compression.BrotliStream/*02000009*/::get_CanSeek() /* 06000017 */
  } // end of property BrotliStream::CanSeek
  .property /*17000004*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Compression.BrotliStream/*02000009*/::get_CanWrite() /* 06000018 */
  } // end of property BrotliStream::CanWrite
  .property /*17000005*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.BrotliStream/*02000009*/::get_Length() /* 06000019 */
  } // end of property BrotliStream::Length
  .property /*17000006*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Compression.BrotliStream/*02000009*/::get_Position() /* 0600001A */
    .set instance void System.IO.Compression.BrotliStream/*02000009*/::set_Position(int64) /* 0600001B */
  } // end of property BrotliStream::Position
} // end of class System.IO.Compression.BrotliStream


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
