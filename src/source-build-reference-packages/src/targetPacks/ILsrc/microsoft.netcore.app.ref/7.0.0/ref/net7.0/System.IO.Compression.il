


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
// Time-date stamp:            0x914e2105
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000433a
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
// Checksum:                       0x0000a5b3
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000042e7 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000484] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002e00 [0x00002880] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00004200 [0x00000054] address [size] of Debug Directory:           
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
//              0x00002340 Virtual Size
//              0x00002000 Virtual Address
//              0x00002400 Size of Raw Data
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
//              0x00000484 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002600 Pointer to Raw Data
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
//              0x00002c00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00004000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x0000033c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00004329 Import Name Table
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
// 0x00002264 [0x00001f1c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00004180 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001294 Size
//              '#~' Name
//    Stream 2:
//              0x00001300 Offset
//              0x000007f0 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001af0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001af4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001b04 Offset
//              0x00000418 Size
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

// File size            : 22144
// PE header size       : 512 (496 used)    ( 2.31%)
// PE additional info   : 11779             (53.19%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.33%)
// CLR meta-data size  : 7964               (35.96%)
// CLR additional info : 128                ( 0.58%)
// CLR method headers  : 135                ( 0.61%)
// Managed code         : 395               ( 1.78%)
// Data                 : 2048              ( 9.25%)
// Unaccounted          : -889              (-4.01%)

// Num.of PE sections   : 3
//   .text    - 9216
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 7964
//   Module        -    1 (10 bytes)
//   TypeDef       -   14 (196 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   41 (246 bytes)
//   MethodDef     -  135 (1890 bytes)    0 abstract, 0 native, 135 bodies
//   FieldDef      -   16 (96 bytes)      0 constant
//   MemberRef     -   21 (126 bytes)
//   ParamDef      -  182 (1092 bytes)
//   Constant      -   15 (90 bytes)
//   CustomAttribute-   74 (444 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    5 (20 bytes)
//   Property      -   31 (186 bytes)
//   MethodSemantic-   38 (228 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   Strings       -  2030 bytes
//   Blobs         -  1048 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   196 bytes

// CLR additional info : 128

// CLR method headers : 135
//   Num.of method bodies  - 135
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 135

// Managed code : 395
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class CompressionLevel               (public) (auto) (ansi) (sealed) 
// Class CompressionMode                (public) (auto) (ansi) (sealed) 
// Class DeflateStream                  (public) (auto) (ansi) 
// Class GZipStream                     (public) (auto) (ansi) 
// Class ZipArchive                     (public) (auto) (ansi) 
// Class ZipArchiveEntry                (public) (auto) (ansi) 
// Class ZipArchiveMode                 (public) (auto) (ansi) (sealed) 
// Class ZLibStream                     (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.IO.Compression
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000004:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 15 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                        70 72 65 73 73 69 6F 6E 00 00 )                   // pression..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 15 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                       70 72 65 73 73 69 6F 6E 00 00 )                   // pression..
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 15 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                 70 72 65 73 73 69 6F 6E 00 00 )                   // pression..
  .custom /*0C000015:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 7:0:0:0
}
.module System.IO.Compression.dll
// MVID: {350659d8-6802-4cd5-b993-d0b21a9e789e}
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
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000018:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001B:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001E:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000021:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public auto ansi sealed System.IO.Compression.CompressionLevel
       extends [System.Runtime/*23000001*/]System.Enum/*01000015*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.IO.Compression.CompressionLevel/*02000007*/ Optimal = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.IO.Compression.CompressionLevel/*02000007*/ Fastest = int32(0x00000001)
  .field /*04000008*/ public static literal valuetype System.IO.Compression.CompressionLevel/*02000007*/ NoCompression = int32(0x00000002)
  .field /*04000009*/ public static literal valuetype System.IO.Compression.CompressionLevel/*02000007*/ SmallestSize = int32(0x00000003)
} // end of class System.IO.Compression.CompressionLevel

.class /*02000008*/ public auto ansi sealed System.IO.Compression.CompressionMode
       extends [System.Runtime/*23000001*/]System.Enum/*01000015*/
{
  .field /*0400000A*/ public specialname rtspecialname int32 value__
  .field /*0400000B*/ public static literal valuetype System.IO.Compression.CompressionMode/*02000008*/ Decompress = int32(0x00000000)
  .field /*0400000C*/ public static literal valuetype System.IO.Compression.CompressionMode/*02000008*/ Compress = int32(0x00000001)
} // end of class System.IO.Compression.CompressionMode

.class /*02000009*/ public auto ansi beforefieldinit System.IO.Compression.DeflateStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
{
  .custom /*0C000024:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000007*/ compressionLevel) cil managed
  // SIG: 20 02 01 12 59 11 1C
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000007*/ compressionLevel,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 1C 02
  {
    // Method begins at RVA 0x20b4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000008*/ mode) cil managed
  // SIG: 20 02 01 12 59 11 20
  {
    // Method begins at RVA 0x20bc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000008*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 20 02
  {
    // Method begins at RVA 0x20c4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*0600000B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 
          get_BaseStream() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_BaseStream

  .method /*0600000C*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_CanRead

  .method /*0600000D*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_CanSeek

  .method /*0600000E*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_CanWrite

  .method /*0600000F*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_Length

  .method /*06000010*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_Position

  .method /*06000011*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x20de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::set_Position

  .method /*06000012*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*0800000F*/ 
    .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000010*/ 
    .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::BeginRead

  .method /*06000013*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*08000014*/ 
    .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000015*/ 
    .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::BeginWrite

  .method /*06000014*/ public hidebysig virtual 
          instance void  CopyTo(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                                int32 bufferSize) cil managed
  // SIG: 20 02 01 12 59 08
  {
    // Method begins at RVA 0x20e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::CopyTo

  .method /*06000015*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 03 12 69 12 59 08 11 6D
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::CopyToAsync

  .method /*06000016*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20eb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::Dispose

  .method /*06000017*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 71
  {
    // Method begins at RVA 0x20ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::DisposeAsync

  .method /*06000018*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 08 12 5D
  {
    // Method begins at RVA 0x20f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::EndRead

  .method /*06000019*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x20f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::EndWrite

  .method /*0600001A*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::Flush

  .method /*0600001B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::FlushAsync

  .method /*0600001C*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::Read

  .method /*0600001D*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001D*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 75 01 05
  {
    .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::Read

  .method /*0600001E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100001E*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 79 01 08 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::ReadAsync

  .method /*0600001F*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100001F*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000020*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 7D 01 08 15 11 80 81 01 05 11 6D
  {
    .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000028*/  = nullref
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::ReadAsync

  .method /*06000020*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::ReadByte

  .method /*06000021*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000021*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 85
  {
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::Seek

  .method /*06000022*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x210c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::SetLength

  .method /*06000023*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x210e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::Write

  .method /*06000024*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000022*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 80 89 01 05
  {
    .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::Write

  .method /*06000025*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 12 69 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::WriteAsync

  .method /*06000026*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000023*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 11 71 15 11 80 8D 01 05 11 6D
  {
    .custom /*0C000036:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000035*/  = nullref
    // Method begins at RVA 0x2115
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::WriteAsync

  .method /*06000027*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2118
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::WriteByte

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
          BaseStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ System.IO.Compression.DeflateStream/*02000009*/::get_BaseStream() /* 0600000B */
  } // end of property DeflateStream::BaseStream
  .property /*17000002*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Compression.DeflateStream/*02000009*/::get_CanRead() /* 0600000C */
  } // end of property DeflateStream::CanRead
  .property /*17000003*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Compression.DeflateStream/*02000009*/::get_CanSeek() /* 0600000D */
  } // end of property DeflateStream::CanSeek
  .property /*17000004*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Compression.DeflateStream/*02000009*/::get_CanWrite() /* 0600000E */
  } // end of property DeflateStream::CanWrite
  .property /*17000005*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.DeflateStream/*02000009*/::get_Length() /* 0600000F */
  } // end of property DeflateStream::Length
  .property /*17000006*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Compression.DeflateStream/*02000009*/::get_Position() /* 06000010 */
    .set instance void System.IO.Compression.DeflateStream/*02000009*/::set_Position(int64) /* 06000011 */
  } // end of property DeflateStream::Position
} // end of class System.IO.Compression.DeflateStream

.class /*0200000A*/ public auto ansi beforefieldinit System.IO.Compression.GZipStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
{
  .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000007*/ compressionLevel) cil managed
  // SIG: 20 02 01 12 59 11 1C
  {
    // Method begins at RVA 0x211a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000007*/ compressionLevel,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 1C 02
  {
    // Method begins at RVA 0x2122
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000008*/ mode) cil managed
  // SIG: 20 02 01 12 59 11 20
  {
    // Method begins at RVA 0x212a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*0600002B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000008*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 20 02
  {
    // Method begins at RVA 0x2132
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*0600002C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 
          get_BaseStream() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_BaseStream

  .method /*0600002D*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_CanRead

  .method /*0600002E*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_CanSeek

  .method /*0600002F*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_CanWrite

  .method /*06000030*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_Length

  .method /*06000031*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_Position

  .method /*06000032*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x214c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::set_Position

  .method /*06000033*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*08000045*/ 
    .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000046*/ 
    .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::BeginRead

  .method /*06000034*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*0800004A*/ 
    .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800004B*/ 
    .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2151
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::BeginWrite

  .method /*06000035*/ public hidebysig virtual 
          instance void  CopyTo(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                                int32 bufferSize) cil managed
  // SIG: 20 02 01 12 59 08
  {
    // Method begins at RVA 0x2154
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::CopyTo

  .method /*06000036*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 03 12 69 12 59 08 11 6D
  {
    // Method begins at RVA 0x2156
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::CopyToAsync

  .method /*06000037*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2159
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::Dispose

  .method /*06000038*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 71
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::DisposeAsync

  .method /*06000039*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 08 12 5D
  {
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::EndRead

  .method /*0600003A*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x2161
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::EndWrite

  .method /*0600003B*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2163
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::Flush

  .method /*0600003C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x2165
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::FlushAsync

  .method /*0600003D*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x2168
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::Read

  .method /*0600003E*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001D*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 75 01 05
  {
    .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x216b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::Read

  .method /*0600003F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100001E*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 79 01 08 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x216e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::ReadAsync

  .method /*06000040*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100001F*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000020*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 7D 01 08 15 11 80 81 01 05 11 6D
  {
    .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800005E*/  = nullref
    // Method begins at RVA 0x2171
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::ReadAsync

  .method /*06000041*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2174
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::ReadByte

  .method /*06000042*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000021*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 85
  {
    // Method begins at RVA 0x2177
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::Seek

  .method /*06000043*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x217a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::SetLength

  .method /*06000044*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x217c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::Write

  .method /*06000045*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000022*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 80 89 01 05
  {
    .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x217e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::Write

  .method /*06000046*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 12 69 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2180
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::WriteAsync

  .method /*06000047*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000023*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 11 71 15 11 80 8D 01 05 11 6D
  {
    .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800006B*/  = nullref
    // Method begins at RVA 0x2183
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::WriteAsync

  .method /*06000048*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2186
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::WriteByte

  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
          BaseStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ System.IO.Compression.GZipStream/*0200000A*/::get_BaseStream() /* 0600002C */
  } // end of property GZipStream::BaseStream
  .property /*17000008*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Compression.GZipStream/*0200000A*/::get_CanRead() /* 0600002D */
  } // end of property GZipStream::CanRead
  .property /*17000009*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Compression.GZipStream/*0200000A*/::get_CanSeek() /* 0600002E */
  } // end of property GZipStream::CanSeek
  .property /*1700000A*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Compression.GZipStream/*0200000A*/::get_CanWrite() /* 0600002F */
  } // end of property GZipStream::CanWrite
  .property /*1700000B*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.GZipStream/*0200000A*/::get_Length() /* 06000030 */
  } // end of property GZipStream::Length
  .property /*1700000C*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Compression.GZipStream/*0200000A*/::get_Position() /* 06000031 */
    .set instance void System.IO.Compression.GZipStream/*0200000A*/::set_Position(int64) /* 06000032 */
  } // end of property GZipStream::Position
} // end of class System.IO.Compression.GZipStream

.class /*0200000B*/ public auto ansi beforefieldinit System.IO.Compression.ZipArchive
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000024*/
{
  .custom /*0C000028:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000024*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000049*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream') cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x2188
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchive::.ctor

  .method /*0600004A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/ mode) cil managed
  // SIG: 20 02 01 12 59 11 34
  {
    // Method begins at RVA 0x2190
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchive::.ctor

  .method /*0600004B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 34 02
  {
    // Method begins at RVA 0x2198
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchive::.ctor

  .method /*0600004C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/ mode,
                               bool leaveOpen,
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*01000025*/ entryNameEncoding) cil managed
  // SIG: 20 04 01 12 59 11 34 02 12 80 95
  {
    .param [4]/*08000076*/ 
    .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchive::.ctor

  .method /*0600004D*/ public hidebysig specialname 
          instance string  get_Comment() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::get_Comment

  .method /*0600004E*/ public hidebysig specialname 
          instance void  set_Comment(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*08000077*/ 
    .custom /*0C000040:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000026*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ab
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchive::set_Comment

  .method /*0600004F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000027*/<class System.IO.Compression.ZipArchiveEntry/*0200000C*/> 
          get_Entries() cil managed
  // SIG: 20 00 15 12 80 9D 01 12 30
  {
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::get_Entries

  .method /*06000050*/ public hidebysig specialname 
          instance valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 34
  {
    // Method begins at RVA 0x21b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::get_Mode

  .method /*06000051*/ public hidebysig instance class System.IO.Compression.ZipArchiveEntry/*0200000C*/ 
          CreateEntry(string entryName) cil managed
  // SIG: 20 01 12 30 0E
  {
    // Method begins at RVA 0x21b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::CreateEntry

  .method /*06000052*/ public hidebysig instance class System.IO.Compression.ZipArchiveEntry/*0200000C*/ 
          CreateEntry(string entryName,
                      valuetype System.IO.Compression.CompressionLevel/*02000007*/ compressionLevel) cil managed
  // SIG: 20 02 12 30 0E 11 1C
  {
    // Method begins at RVA 0x21b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::CreateEntry

  .method /*06000053*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchive::Dispose

  .method /*06000054*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21bb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchive::Dispose

  .method /*06000055*/ public hidebysig instance class System.IO.Compression.ZipArchiveEntry/*0200000C*/ 
          GetEntry(string entryName) cil managed
  // SIG: 20 01 12 30 0E
  {
    .param [0]/*0800007C*/ 
    .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::GetEntry

  .property /*1700000D*/ instance callconv(8) string
          Comment()
  {
    .get instance string System.IO.Compression.ZipArchive/*0200000B*/::get_Comment() /* 0600004D */
    .set instance void System.IO.Compression.ZipArchive/*0200000B*/::set_Comment(string) /* 0600004E */
  } // end of property ZipArchive::Comment
  .property /*1700000E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000027*/<class System.IO.Compression.ZipArchiveEntry/*0200000C*/>
          Entries()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000027*/<class System.IO.Compression.ZipArchiveEntry/*0200000C*/> System.IO.Compression.ZipArchive/*0200000B*/::get_Entries() /* 0600004F */
  } // end of property ZipArchive::Entries
  .property /*1700000F*/ instance callconv(8) valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/
          Mode()
  {
    .get instance valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/ System.IO.Compression.ZipArchive/*0200000B*/::get_Mode() /* 06000050 */
  } // end of property ZipArchive::Mode
} // end of class System.IO.Compression.ZipArchive

.class /*0200000C*/ public auto ansi beforefieldinit System.IO.Compression.ZipArchiveEntry
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000056*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21c0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::.ctor

  .method /*06000057*/ public hidebysig specialname 
          instance class System.IO.Compression.ZipArchive/*0200000B*/ 
          get_Archive() cil managed
  // SIG: 20 00 12 2C
  {
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Archive

  .method /*06000058*/ public hidebysig specialname 
          instance string  get_Comment() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Comment

  .method /*06000059*/ public hidebysig specialname 
          instance void  set_Comment(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800007E*/ 
    .custom /*0C000043:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000026*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ce
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::set_Comment

  .method /*0600005A*/ public hidebysig specialname 
          instance int64  get_CompressedLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_CompressedLength

  .method /*0600005B*/ public hidebysig specialname 
          instance uint32  get_Crc32() cil managed
  // SIG: 20 00 09
  {
    // Method begins at RVA 0x21d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Crc32

  .method /*0600005C*/ public hidebysig specialname 
          instance int32  get_ExternalAttributes() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_ExternalAttributes

  .method /*0600005D*/ public hidebysig specialname 
          instance void  set_ExternalAttributes(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::set_ExternalAttributes

  .method /*0600005E*/ public hidebysig specialname 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_FullName

  .method /*0600005F*/ public hidebysig specialname 
          instance bool  get_IsEncrypted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_IsEncrypted

  .method /*06000060*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/ 
          get_LastWriteTime() cil managed
  // SIG: 20 00 11 80 A1
  {
    // Method begins at RVA 0x21e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_LastWriteTime

  .method /*06000061*/ public hidebysig specialname 
          instance void  set_LastWriteTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 11 80 A1
  {
    // Method begins at RVA 0x21e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::set_LastWriteTime

  .method /*06000062*/ public hidebysig specialname 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Length

  .method /*06000063*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Name

  .method /*06000064*/ public hidebysig instance void 
          Delete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ec
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::Delete

  .method /*06000065*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 
          Open() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::Open

  .method /*06000066*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::ToString

  .property /*17000010*/ instance callconv(8) class System.IO.Compression.ZipArchive/*0200000B*/
          Archive()
  {
    .get instance class System.IO.Compression.ZipArchive/*0200000B*/ System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_Archive() /* 06000057 */
  } // end of property ZipArchiveEntry::Archive
  .property /*17000011*/ instance callconv(8) string
          Comment()
  {
    .get instance string System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_Comment() /* 06000058 */
    .set instance void System.IO.Compression.ZipArchiveEntry/*0200000C*/::set_Comment(string) /* 06000059 */
  } // end of property ZipArchiveEntry::Comment
  .property /*17000012*/ instance callconv(8) int64
          CompressedLength()
  {
    .get instance int64 System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_CompressedLength() /* 0600005A */
  } // end of property ZipArchiveEntry::CompressedLength
  .property /*17000013*/ instance callconv(8) uint32
          Crc32()
  {
    .custom /*0C000030:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .get instance uint32 System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_Crc32() /* 0600005B */
  } // end of property ZipArchiveEntry::Crc32
  .property /*17000014*/ instance callconv(8) int32
          ExternalAttributes()
  {
    .get instance int32 System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_ExternalAttributes() /* 0600005C */
    .set instance void System.IO.Compression.ZipArchiveEntry/*0200000C*/::set_ExternalAttributes(int32) /* 0600005D */
  } // end of property ZipArchiveEntry::ExternalAttributes
  .property /*17000015*/ instance callconv(8) string
          FullName()
  {
    .get instance string System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_FullName() /* 0600005E */
  } // end of property ZipArchiveEntry::FullName
  .property /*17000016*/ instance callconv(8) bool
          IsEncrypted()
  {
    .get instance bool System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_IsEncrypted() /* 0600005F */
  } // end of property ZipArchiveEntry::IsEncrypted
  .property /*17000017*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/
          LastWriteTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/ System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_LastWriteTime() /* 06000060 */
    .set instance void System.IO.Compression.ZipArchiveEntry/*0200000C*/::set_LastWriteTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/) /* 06000061 */
  } // end of property ZipArchiveEntry::LastWriteTime
  .property /*17000018*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_Length() /* 06000062 */
  } // end of property ZipArchiveEntry::Length
  .property /*17000019*/ instance callconv(8) string
          Name()
  {
    .get instance string System.IO.Compression.ZipArchiveEntry/*0200000C*/::get_Name() /* 06000063 */
  } // end of property ZipArchiveEntry::Name
} // end of class System.IO.Compression.ZipArchiveEntry

.class /*0200000D*/ public auto ansi sealed System.IO.Compression.ZipArchiveMode
       extends [System.Runtime/*23000001*/]System.Enum/*01000015*/
{
  .field /*0400000D*/ public specialname rtspecialname int32 value__
  .field /*0400000E*/ public static literal valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/ Read = int32(0x00000000)
  .field /*0400000F*/ public static literal valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/ Create = int32(0x00000001)
  .field /*04000010*/ public static literal valuetype System.IO.Compression.ZipArchiveMode/*0200000D*/ Update = int32(0x00000002)
} // end of class System.IO.Compression.ZipArchiveMode

.class /*0200000E*/ public auto ansi sealed beforefieldinit System.IO.Compression.ZLibStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000067*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000007*/ compressionLevel) cil managed
  // SIG: 20 02 01 12 59 11 1C
  {
    // Method begins at RVA 0x21f4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*06000068*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000007*/ compressionLevel,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 1C 02
  {
    // Method begins at RVA 0x21fc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000008*/ mode) cil managed
  // SIG: 20 02 01 12 59 11 20
  {
    // Method begins at RVA 0x2204
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000008*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 20 02
  {
    // Method begins at RVA 0x220c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*0600006B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 
          get_BaseStream() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_BaseStream

  .method /*0600006C*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_CanRead

  .method /*0600006D*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_CanSeek

  .method /*0600006E*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x221d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_CanWrite

  .method /*0600006F*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2220
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_Length

  .method /*06000070*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_Position

  .method /*06000071*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2226
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::set_Position

  .method /*06000072*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*0800008F*/ 
    .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000090*/ 
    .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2228
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::BeginRead

  .method /*06000073*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*08000094*/ 
    .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000095*/ 
    .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::BeginWrite

  .method /*06000074*/ public hidebysig virtual 
          instance void  CopyTo(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                                int32 bufferSize) cil managed
  // SIG: 20 02 01 12 59 08
  {
    // Method begins at RVA 0x222e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::CopyTo

  .method /*06000075*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 03 12 69 12 59 08 11 6D
  {
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::CopyToAsync

  .method /*06000076*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2233
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::Dispose

  .method /*06000077*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 71
  {
    // Method begins at RVA 0x2235
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::DisposeAsync

  .method /*06000078*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 08 12 5D
  {
    // Method begins at RVA 0x2238
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::EndRead

  .method /*06000079*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x223b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::EndWrite

  .method /*0600007A*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x223d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::Flush

  .method /*0600007B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::FlushAsync

  .method /*0600007C*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::Read

  .method /*0600007D*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001D*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 75 01 05
  {
    .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::Read

  .method /*0600007E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100001E*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 79 01 08 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::ReadAsync

  .method /*0600007F*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100001F*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000020*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 7D 01 08 15 11 80 81 01 05 11 6D
  {
    .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080000A8*/  = nullref
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::ReadAsync

  .method /*06000080*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::ReadByte

  .method /*06000081*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000021*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 85
  {
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::Seek

  .method /*06000082*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2254
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::SetLength

  .method /*06000083*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2256
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::Write

  .method /*06000084*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000022*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 80 89 01 05
  {
    .custom /*0C000045:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2258
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::Write

  .method /*06000085*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 12 69 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::WriteAsync

  .method /*06000086*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000023*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 11 71 15 11 80 8D 01 05 11 6D
  {
    .custom /*0C000046:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080000B5*/  = nullref
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::WriteAsync

  .method /*06000087*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2260
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::WriteByte

  .property /*1700001A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
          BaseStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ System.IO.Compression.ZLibStream/*0200000E*/::get_BaseStream() /* 0600006B */
  } // end of property ZLibStream::BaseStream
  .property /*1700001B*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Compression.ZLibStream/*0200000E*/::get_CanRead() /* 0600006C */
  } // end of property ZLibStream::CanRead
  .property /*1700001C*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Compression.ZLibStream/*0200000E*/::get_CanSeek() /* 0600006D */
  } // end of property ZLibStream::CanSeek
  .property /*1700001D*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Compression.ZLibStream/*0200000E*/::get_CanWrite() /* 0600006E */
  } // end of property ZLibStream::CanWrite
  .property /*1700001E*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.ZLibStream/*0200000E*/::get_Length() /* 0600006F */
  } // end of property ZLibStream::Length
  .property /*1700001F*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Compression.ZLibStream/*0200000E*/::get_Position() /* 06000070 */
    .set instance void System.IO.Compression.ZLibStream/*0200000E*/::set_Position(int64) /* 06000071 */
  } // end of property ZLibStream::Position
} // end of class System.IO.Compression.ZLibStream


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
