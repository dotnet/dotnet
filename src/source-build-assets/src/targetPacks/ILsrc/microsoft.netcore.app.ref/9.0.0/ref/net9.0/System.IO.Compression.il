


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
// Time-date stamp:            0xca3710aa
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
// Addr. of entry point:           0x000042d2
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
// Checksum:                       0x0001381f
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00004280 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000484] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002e00 [0x00002918] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00004264 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000022d8 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x000002d4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000042c2 Import Name Table
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
// 0x00002248 [0x00001f9c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000041e4 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001310 Size
//              '#~' Name
//    Stream 2:
//              0x0000137c Offset
//              0x00000878 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001bf4 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001bf8 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001c08 Offset
//              0x00000394 Size
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

// File size            : 22296
// PE header size       : 512 (496 used)    ( 2.30%)
// PE additional info   : 11875             (53.26%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.32%)
// CLR meta-data size  : 8092               (36.29%)
// CLR additional info : 128                ( 0.57%)
// CLR method headers  : 137                ( 0.61%)
// Managed code         : 367               ( 1.65%)
// Data                 : 2048              ( 9.19%)
// Unaccounted          : -935              (-4.19%)

// Num.of PE sections   : 3
//   .text    - 9216
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 8092
//   Module        -    1 (10 bytes)
//   TypeDef       -   11 (154 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   41 (246 bytes)
//   MethodDef     -  137 (1918 bytes)    0 abstract, 0 native, 137 bodies
//   FieldDef      -   20 (120 bytes)     0 constant
//   MemberRef     -   23 (138 bytes)
//   ParamDef      -  193 (1158 bytes)
//   Constant      -   23 (138 bytes)
//   CustomAttribute-   65 (390 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    6 (24 bytes)
//   Property      -   33 (198 bytes)
//   MethodSemantic-   42 (252 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   Strings       -  2166 bytes
//   Blobs         -   916 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   198 bytes

// CLR additional info : 128

// CLR method headers : 137
//   Num.of method bodies  - 137
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 137

// Managed code : 367
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class CompressionLevel               (public) (auto) (ansi) (sealed) 
// Class CompressionMode                (public) (auto) (ansi) (sealed) 
// Class DeflateStream                  (public) (auto) (ansi) 
// Class GZipStream                     (public) (auto) (ansi) 
// Class ZipArchive                     (public) (auto) (ansi) 
// Class ZipArchiveEntry                (public) (auto) (ansi) 
// Class ZipArchiveMode                 (public) (auto) (ansi) (sealed) 
// Class ZLibCompressionOptions         (public) (auto) (ansi) (sealed) 
// Class ZLibCompressionStrategy        (public) (auto) (ansi) (sealed) 
// Class ZLibStream                     (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 15 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                        70 72 65 73 73 69 6F 6E 00 00 )                   // pression..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 15 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                       70 72 65 73 73 69 6F 6E 00 00 )                   // pression..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 15 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                 70 72 65 73 73 69 6F 6E 00 00 )                   // pression..
  .custom /*0C000014:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 9:0:0:0
}
.module System.IO.Compression.dll
// MVID: {7f553084-7f73-4884-8de0-ad969b5a6d80}
.custom /*0C000002:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed System.IO.Compression.CompressionLevel
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.IO.Compression.CompressionLevel/*02000002*/ Optimal = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.IO.Compression.CompressionLevel/*02000002*/ Fastest = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.IO.Compression.CompressionLevel/*02000002*/ NoCompression = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype System.IO.Compression.CompressionLevel/*02000002*/ SmallestSize = int32(0x00000003)
} // end of class System.IO.Compression.CompressionLevel

.class /*02000003*/ public auto ansi sealed System.IO.Compression.CompressionMode
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*04000006*/ public specialname rtspecialname int32 value__
  .field /*04000007*/ public static literal valuetype System.IO.Compression.CompressionMode/*02000003*/ Decompress = int32(0x00000000)
  .field /*04000008*/ public static literal valuetype System.IO.Compression.CompressionMode/*02000003*/ Compress = int32(0x00000001)
} // end of class System.IO.Compression.CompressionMode

.class /*02000004*/ public auto ansi beforefieldinit System.IO.Compression.DeflateStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
{
  .custom /*0C000015:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000016:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000002*/ compressionLevel) cil managed
  // SIG: 20 02 01 12 59 11 08
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000002*/ compressionLevel,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 08 02
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000003*/ mode) cil managed
  // SIG: 20 02 01 12 59 11 0C
  {
    // Method begins at RVA 0x2060
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000003*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 0C 02
  {
    // Method begins at RVA 0x2068
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               class System.IO.Compression.ZLibCompressionOptions/*02000009*/ compressionOptions,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 12 24 02
  {
    .param [3]/*0800000D*/  = bool(false)
    // Method begins at RVA 0x2070
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeflateStream::.ctor

  .method /*06000006*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 
          get_BaseStream() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2078
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_BaseStream

  .method /*06000007*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x207b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_CanRead

  .method /*06000008*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x207e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_CanSeek

  .method /*06000009*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2081
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_CanWrite

  .method /*0600000A*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2084
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_Length

  .method /*0600000B*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2087
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::get_Position

  .method /*0600000C*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x208a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::set_Position

  .method /*0600000D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*08000012*/ 
    .custom /*0C000021:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000013*/ 
    .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::BeginRead

  .method /*0600000E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*08000017*/ 
    .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000018*/ 
    .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::BeginWrite

  .method /*0600000F*/ public hidebysig virtual 
          instance void  CopyTo(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                                int32 bufferSize) cil managed
  // SIG: 20 02 01 12 59 08
  {
    // Method begins at RVA 0x2092
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::CopyTo

  .method /*06000010*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 03 12 69 12 59 08 11 6D
  {
    // Method begins at RVA 0x2094
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::CopyToAsync

  .method /*06000011*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2097
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::Dispose

  .method /*06000012*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 71
  {
    // Method begins at RVA 0x2099
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::DisposeAsync

  .method /*06000013*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 08 12 5D
  {
    // Method begins at RVA 0x209c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::EndRead

  .method /*06000014*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x209f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::EndWrite

  .method /*06000015*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::Flush

  .method /*06000016*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x20a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::FlushAsync

  .method /*06000017*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x20a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::Read

  .method /*06000018*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001D*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 75 01 05
  {
    .custom /*0C000025:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::Read

  .method /*06000019*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100001E*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 79 01 08 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::ReadAsync

  .method /*0600001A*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100001F*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000020*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 7D 01 08 15 11 80 81 01 05 11 6D
  {
    .custom /*0C000027:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800002B*/  = nullref
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::ReadAsync

  .method /*0600001B*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::ReadByte

  .method /*0600001C*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000021*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 85
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::Seek

  .method /*0600001D*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x20b8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::SetLength

  .method /*0600001E*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x20ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::Write

  .method /*0600001F*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000022*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 80 89 01 05
  {
    .custom /*0C000028:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::Write

  .method /*06000020*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 12 69 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::WriteAsync

  .method /*06000021*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000023*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 11 71 15 11 80 8D 01 05 11 6D
  {
    .custom /*0C000029:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000038*/  = nullref
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DeflateStream::WriteAsync

  .method /*06000022*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x20c4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeflateStream::WriteByte

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
          BaseStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ System.IO.Compression.DeflateStream/*02000004*/::get_BaseStream() /* 06000006 */
  } // end of property DeflateStream::BaseStream
  .property /*17000002*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Compression.DeflateStream/*02000004*/::get_CanRead() /* 06000007 */
  } // end of property DeflateStream::CanRead
  .property /*17000003*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Compression.DeflateStream/*02000004*/::get_CanSeek() /* 06000008 */
  } // end of property DeflateStream::CanSeek
  .property /*17000004*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Compression.DeflateStream/*02000004*/::get_CanWrite() /* 06000009 */
  } // end of property DeflateStream::CanWrite
  .property /*17000005*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.DeflateStream/*02000004*/::get_Length() /* 0600000A */
  } // end of property DeflateStream::Length
  .property /*17000006*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Compression.DeflateStream/*02000004*/::get_Position() /* 0600000B */
    .set instance void System.IO.Compression.DeflateStream/*02000004*/::set_Position(int64) /* 0600000C */
  } // end of property DeflateStream::Position
} // end of class System.IO.Compression.DeflateStream

.class /*02000005*/ public auto ansi beforefieldinit System.IO.Compression.GZipStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
{
  .custom /*0C000017:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000018:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000023*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000002*/ compressionLevel) cil managed
  // SIG: 20 02 01 12 59 11 08
  {
    // Method begins at RVA 0x20c6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*06000024*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000002*/ compressionLevel,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 08 02
  {
    // Method begins at RVA 0x20ce
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000003*/ mode) cil managed
  // SIG: 20 02 01 12 59 11 0C
  {
    // Method begins at RVA 0x20d6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*06000026*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000003*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 0C 02
  {
    // Method begins at RVA 0x20de
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               class System.IO.Compression.ZLibCompressionOptions/*02000009*/ compressionOptions,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 12 24 02
  {
    .param [3]/*08000046*/  = bool(false)
    // Method begins at RVA 0x20e6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GZipStream::.ctor

  .method /*06000028*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 
          get_BaseStream() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_BaseStream

  .method /*06000029*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_CanRead

  .method /*0600002A*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_CanSeek

  .method /*0600002B*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_CanWrite

  .method /*0600002C*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_Length

  .method /*0600002D*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::get_Position

  .method /*0600002E*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2100
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::set_Position

  .method /*0600002F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*0800004B*/ 
    .custom /*0C00002E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800004C*/ 
    .custom /*0C00002F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::BeginRead

  .method /*06000030*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*08000050*/ 
    .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000051*/ 
    .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::BeginWrite

  .method /*06000031*/ public hidebysig virtual 
          instance void  CopyTo(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                                int32 bufferSize) cil managed
  // SIG: 20 02 01 12 59 08
  {
    // Method begins at RVA 0x2108
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::CopyTo

  .method /*06000032*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 03 12 69 12 59 08 11 6D
  {
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::CopyToAsync

  .method /*06000033*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x210d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::Dispose

  .method /*06000034*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 71
  {
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::DisposeAsync

  .method /*06000035*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 08 12 5D
  {
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::EndRead

  .method /*06000036*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x2115
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::EndWrite

  .method /*06000037*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2117
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::Flush

  .method /*06000038*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::FlushAsync

  .method /*06000039*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::Read

  .method /*0600003A*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001D*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 75 01 05
  {
    .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::Read

  .method /*0600003B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100001E*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 79 01 08 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::ReadAsync

  .method /*0600003C*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100001F*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000020*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 7D 01 08 15 11 80 81 01 05 11 6D
  {
    .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000064*/  = nullref
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::ReadAsync

  .method /*0600003D*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::ReadByte

  .method /*0600003E*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000021*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 85
  {
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::Seek

  .method /*0600003F*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x212e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::SetLength

  .method /*06000040*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2130
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::Write

  .method /*06000041*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000022*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 80 89 01 05
  {
    .custom /*0C00002C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2132
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::Write

  .method /*06000042*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 12 69 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::WriteAsync

  .method /*06000043*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000023*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 11 71 15 11 80 8D 01 05 11 6D
  {
    .custom /*0C00002D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000071*/  = nullref
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GZipStream::WriteAsync

  .method /*06000044*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x213a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GZipStream::WriteByte

  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
          BaseStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ System.IO.Compression.GZipStream/*02000005*/::get_BaseStream() /* 06000028 */
  } // end of property GZipStream::BaseStream
  .property /*17000008*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Compression.GZipStream/*02000005*/::get_CanRead() /* 06000029 */
  } // end of property GZipStream::CanRead
  .property /*17000009*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Compression.GZipStream/*02000005*/::get_CanSeek() /* 0600002A */
  } // end of property GZipStream::CanSeek
  .property /*1700000A*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Compression.GZipStream/*02000005*/::get_CanWrite() /* 0600002B */
  } // end of property GZipStream::CanWrite
  .property /*1700000B*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.GZipStream/*02000005*/::get_Length() /* 0600002C */
  } // end of property GZipStream::Length
  .property /*1700000C*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Compression.GZipStream/*02000005*/::get_Position() /* 0600002D */
    .set instance void System.IO.Compression.GZipStream/*02000005*/::set_Position(int64) /* 0600002E */
  } // end of property GZipStream::Position
} // end of class System.IO.Compression.GZipStream

.class /*02000006*/ public auto ansi beforefieldinit System.IO.Compression.ZipArchive
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000024*/
{
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000024*/
  .custom /*0C000001:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000045*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream') cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x213c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchive::.ctor

  .method /*06000046*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.ZipArchiveMode/*02000008*/ mode) cil managed
  // SIG: 20 02 01 12 59 11 20
  {
    // Method begins at RVA 0x2144
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchive::.ctor

  .method /*06000047*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.ZipArchiveMode/*02000008*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 20 02
  {
    // Method begins at RVA 0x214c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchive::.ctor

  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.ZipArchiveMode/*02000008*/ mode,
                               bool leaveOpen,
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*01000025*/ entryNameEncoding) cil managed
  // SIG: 20 04 01 12 59 11 20 02 12 80 95
  {
    .param [4]/*0800007C*/ 
    .custom /*0C000036:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2154
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchive::.ctor

  .method /*06000049*/ public hidebysig specialname 
          instance string  get_Comment() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x215c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::get_Comment

  .method /*0600004A*/ public hidebysig specialname 
          instance void  set_Comment(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800007D*/ 
    .custom /*0C000037:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000026*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchive::set_Comment

  .method /*0600004B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000027*/<class System.IO.Compression.ZipArchiveEntry/*02000007*/> 
          get_Entries() cil managed
  // SIG: 20 00 15 12 80 9D 01 12 1C
  {
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::get_Entries

  .method /*0600004C*/ public hidebysig specialname 
          instance valuetype System.IO.Compression.ZipArchiveMode/*02000008*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 20
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::get_Mode

  .method /*0600004D*/ public hidebysig instance class System.IO.Compression.ZipArchiveEntry/*02000007*/ 
          CreateEntry(string entryName) cil managed
  // SIG: 20 01 12 1C 0E
  {
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::CreateEntry

  .method /*0600004E*/ public hidebysig instance class System.IO.Compression.ZipArchiveEntry/*02000007*/ 
          CreateEntry(string entryName,
                      valuetype System.IO.Compression.CompressionLevel/*02000002*/ compressionLevel) cil managed
  // SIG: 20 02 12 1C 0E 11 08
  {
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::CreateEntry

  .method /*0600004F*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x216d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchive::Dispose

  .method /*06000050*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x216f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchive::Dispose

  .method /*06000051*/ public hidebysig instance class System.IO.Compression.ZipArchiveEntry/*02000007*/ 
          GetEntry(string entryName) cil managed
  // SIG: 20 01 12 1C 0E
  {
    .param [0]/*08000082*/ 
    .custom /*0C00003A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2171
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchive::GetEntry

  .property /*1700000D*/ instance callconv(8) string
          Comment()
  {
    .get instance string System.IO.Compression.ZipArchive/*02000006*/::get_Comment() /* 06000049 */
    .set instance void System.IO.Compression.ZipArchive/*02000006*/::set_Comment(string) /* 0600004A */
  } // end of property ZipArchive::Comment
  .property /*1700000E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000027*/<class System.IO.Compression.ZipArchiveEntry/*02000007*/>
          Entries()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000027*/<class System.IO.Compression.ZipArchiveEntry/*02000007*/> System.IO.Compression.ZipArchive/*02000006*/::get_Entries() /* 0600004B */
  } // end of property ZipArchive::Entries
  .property /*1700000F*/ instance callconv(8) valuetype System.IO.Compression.ZipArchiveMode/*02000008*/
          Mode()
  {
    .get instance valuetype System.IO.Compression.ZipArchiveMode/*02000008*/ System.IO.Compression.ZipArchive/*02000006*/::get_Mode() /* 0600004C */
  } // end of property ZipArchive::Mode
} // end of class System.IO.Compression.ZipArchive

.class /*02000007*/ public auto ansi beforefieldinit System.IO.Compression.ZipArchiveEntry
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000052*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2174
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::.ctor

  .method /*06000053*/ public hidebysig specialname 
          instance class System.IO.Compression.ZipArchive/*02000006*/ 
          get_Archive() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Archive

  .method /*06000054*/ public hidebysig specialname 
          instance string  get_Comment() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Comment

  .method /*06000055*/ public hidebysig specialname 
          instance void  set_Comment(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*08000084*/ 
    .custom /*0C00003B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000026*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::set_Comment

  .method /*06000056*/ public hidebysig specialname 
          instance int64  get_CompressedLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2184
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_CompressedLength

  .method /*06000057*/ public hidebysig specialname 
          instance uint32  get_Crc32() cil managed
  // SIG: 20 00 09
  {
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Crc32

  .method /*06000058*/ public hidebysig specialname 
          instance int32  get_ExternalAttributes() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_ExternalAttributes

  .method /*06000059*/ public hidebysig specialname 
          instance void  set_ExternalAttributes(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x218d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::set_ExternalAttributes

  .method /*0600005A*/ public hidebysig specialname 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_FullName

  .method /*0600005B*/ public hidebysig specialname 
          instance bool  get_IsEncrypted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2192
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_IsEncrypted

  .method /*0600005C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/ 
          get_LastWriteTime() cil managed
  // SIG: 20 00 11 80 A1
  {
    // Method begins at RVA 0x2195
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_LastWriteTime

  .method /*0600005D*/ public hidebysig specialname 
          instance void  set_LastWriteTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 11 80 A1
  {
    // Method begins at RVA 0x2198
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::set_LastWriteTime

  .method /*0600005E*/ public hidebysig specialname 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Length

  .method /*0600005F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::get_Name

  .method /*06000060*/ public hidebysig instance void 
          Delete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZipArchiveEntry::Delete

  .method /*06000061*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 
          Open() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x21a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::Open

  .method /*06000062*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZipArchiveEntry::ToString

  .property /*17000010*/ instance callconv(8) class System.IO.Compression.ZipArchive/*02000006*/
          Archive()
  {
    .get instance class System.IO.Compression.ZipArchive/*02000006*/ System.IO.Compression.ZipArchiveEntry/*02000007*/::get_Archive() /* 06000053 */
  } // end of property ZipArchiveEntry::Archive
  .property /*17000011*/ instance callconv(8) string
          Comment()
  {
    .get instance string System.IO.Compression.ZipArchiveEntry/*02000007*/::get_Comment() /* 06000054 */
    .set instance void System.IO.Compression.ZipArchiveEntry/*02000007*/::set_Comment(string) /* 06000055 */
  } // end of property ZipArchiveEntry::Comment
  .property /*17000012*/ instance callconv(8) int64
          CompressedLength()
  {
    .get instance int64 System.IO.Compression.ZipArchiveEntry/*02000007*/::get_CompressedLength() /* 06000056 */
  } // end of property ZipArchiveEntry::CompressedLength
  .property /*17000013*/ instance callconv(8) uint32
          Crc32()
  {
    .custom /*0C000023:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .get instance uint32 System.IO.Compression.ZipArchiveEntry/*02000007*/::get_Crc32() /* 06000057 */
  } // end of property ZipArchiveEntry::Crc32
  .property /*17000014*/ instance callconv(8) int32
          ExternalAttributes()
  {
    .get instance int32 System.IO.Compression.ZipArchiveEntry/*02000007*/::get_ExternalAttributes() /* 06000058 */
    .set instance void System.IO.Compression.ZipArchiveEntry/*02000007*/::set_ExternalAttributes(int32) /* 06000059 */
  } // end of property ZipArchiveEntry::ExternalAttributes
  .property /*17000015*/ instance callconv(8) string
          FullName()
  {
    .get instance string System.IO.Compression.ZipArchiveEntry/*02000007*/::get_FullName() /* 0600005A */
  } // end of property ZipArchiveEntry::FullName
  .property /*17000016*/ instance callconv(8) bool
          IsEncrypted()
  {
    .get instance bool System.IO.Compression.ZipArchiveEntry/*02000007*/::get_IsEncrypted() /* 0600005B */
  } // end of property ZipArchiveEntry::IsEncrypted
  .property /*17000017*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/
          LastWriteTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/ System.IO.Compression.ZipArchiveEntry/*02000007*/::get_LastWriteTime() /* 0600005C */
    .set instance void System.IO.Compression.ZipArchiveEntry/*02000007*/::set_LastWriteTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000028*/) /* 0600005D */
  } // end of property ZipArchiveEntry::LastWriteTime
  .property /*17000018*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.ZipArchiveEntry/*02000007*/::get_Length() /* 0600005E */
  } // end of property ZipArchiveEntry::Length
  .property /*17000019*/ instance callconv(8) string
          Name()
  {
    .get instance string System.IO.Compression.ZipArchiveEntry/*02000007*/::get_Name() /* 0600005F */
  } // end of property ZipArchiveEntry::Name
} // end of class System.IO.Compression.ZipArchiveEntry

.class /*02000008*/ public auto ansi sealed System.IO.Compression.ZipArchiveMode
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*04000009*/ public specialname rtspecialname int32 value__
  .field /*0400000A*/ public static literal valuetype System.IO.Compression.ZipArchiveMode/*02000008*/ Read = int32(0x00000000)
  .field /*0400000B*/ public static literal valuetype System.IO.Compression.ZipArchiveMode/*02000008*/ Create = int32(0x00000001)
  .field /*0400000C*/ public static literal valuetype System.IO.Compression.ZipArchiveMode/*02000008*/ Update = int32(0x00000002)
} // end of class System.IO.Compression.ZipArchiveMode

.class /*02000009*/ public auto ansi sealed beforefieldinit System.IO.Compression.ZLibCompressionOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .field /*0400000D*/ private int32 '<CompressionLevel>k__BackingField'
  .custom /*0C00001F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000029*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .field /*0400000E*/ private valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ '<CompressionStrategy>k__BackingField'
  .custom /*0C000020:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000029*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .method /*06000063*/ public hidebysig specialname 
          instance int32  get_CompressionLevel() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000032:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000029*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00000D       */ ldfld      int32 System.IO.Compression.ZLibCompressionOptions/*02000009*/::'<CompressionLevel>k__BackingField' /* 0400000D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibCompressionOptions::get_CompressionLevel

  .method /*06000064*/ public hidebysig specialname 
          instance void  set_CompressionLevel(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C000033:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000029*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b0
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)00000D       */ stfld      int32 System.IO.Compression.ZLibCompressionOptions/*02000009*/::'<CompressionLevel>k__BackingField' /* 0400000D */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ZLibCompressionOptions::set_CompressionLevel

  .method /*06000065*/ public hidebysig specialname 
          instance valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ 
          get_CompressionStrategy() cil managed
  // SIG: 20 00 11 28
  {
    .custom /*0C000034:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000029*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00000E       */ ldfld      valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ System.IO.Compression.ZLibCompressionOptions/*02000009*/::'<CompressionStrategy>k__BackingField' /* 0400000E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibCompressionOptions::get_CompressionStrategy

  .method /*06000066*/ public hidebysig specialname 
          instance void  set_CompressionStrategy(valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    .custom /*0C000035:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000029*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c1
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)00000E       */ stfld      valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ System.IO.Compression.ZLibCompressionOptions/*02000009*/::'<CompressionStrategy>k__BackingField' /* 0400000E */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ZLibCompressionOptions::set_CompressionStrategy

  .method /*06000067*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ca
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibCompressionOptions::.ctor

  .property /*1700001A*/ instance callconv(8) int32
          CompressionLevel()
  {
    .get instance int32 System.IO.Compression.ZLibCompressionOptions/*02000009*/::get_CompressionLevel() /* 06000063 */
    .set instance void System.IO.Compression.ZLibCompressionOptions/*02000009*/::set_CompressionLevel(int32) /* 06000064 */
  } // end of property ZLibCompressionOptions::CompressionLevel
  .property /*1700001B*/ instance callconv(8) valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/
          CompressionStrategy()
  {
    .get instance valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ System.IO.Compression.ZLibCompressionOptions/*02000009*/::get_CompressionStrategy() /* 06000065 */
    .set instance void System.IO.Compression.ZLibCompressionOptions/*02000009*/::set_CompressionStrategy(valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/) /* 06000066 */
  } // end of property ZLibCompressionOptions::CompressionStrategy
} // end of class System.IO.Compression.ZLibCompressionOptions

.class /*0200000A*/ public auto ansi sealed System.IO.Compression.ZLibCompressionStrategy
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*0400000F*/ public specialname rtspecialname int32 value__
  .field /*04000010*/ public static literal valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ Default = int32(0x00000000)
  .field /*04000011*/ public static literal valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ Filtered = int32(0x00000001)
  .field /*04000012*/ public static literal valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ HuffmanOnly = int32(0x00000002)
  .field /*04000013*/ public static literal valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ RunLengthEncoding = int32(0x00000003)
  .field /*04000014*/ public static literal valuetype System.IO.Compression.ZLibCompressionStrategy/*0200000A*/ Fixed = int32(0x00000004)
} // end of class System.IO.Compression.ZLibCompressionStrategy

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.IO.Compression.ZLibStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
{
  .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000068*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000002*/ compressionLevel) cil managed
  // SIG: 20 02 01 12 59 11 08
  {
    // Method begins at RVA 0x21d2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionLevel/*02000002*/ compressionLevel,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 08 02
  {
    // Method begins at RVA 0x21da
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000003*/ mode) cil managed
  // SIG: 20 02 01 12 59 11 0C
  {
    // Method begins at RVA 0x21e2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*0600006B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               valuetype System.IO.Compression.CompressionMode/*02000003*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 11 0C 02
  {
    // Method begins at RVA 0x21ea
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*0600006C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 'stream',
                               class System.IO.Compression.ZLibCompressionOptions/*02000009*/ compressionOptions,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 03 01 12 59 12 24 02
  {
    .param [3]/*08000095*/  = bool(false)
    // Method begins at RVA 0x21f2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ZLibStream::.ctor

  .method /*0600006D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ 
          get_BaseStream() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_BaseStream

  .method /*0600006E*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_CanRead

  .method /*0600006F*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2200
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_CanSeek

  .method /*06000070*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_CanWrite

  .method /*06000071*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2206
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_Length

  .method /*06000072*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::get_Position

  .method /*06000073*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x220c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::set_Position

  .method /*06000074*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*0800009A*/ 
    .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800009B*/ 
    .custom /*0C00003F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::BeginRead

  .method /*06000075*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*01000018*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 5D 1D 05 08 08 12 61 1C
  {
    .param [4]/*0800009F*/ 
    .custom /*0C000040:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000A0*/ 
    .custom /*0C000041:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2211
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::BeginWrite

  .method /*06000076*/ public hidebysig virtual 
          instance void  CopyTo(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                                int32 bufferSize) cil managed
  // SIG: 20 02 01 12 59 08
  {
    // Method begins at RVA 0x2214
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::CopyTo

  .method /*06000077*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ destination,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 03 12 69 12 59 08 11 6D
  {
    // Method begins at RVA 0x2216
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::CopyToAsync

  .method /*06000078*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2219
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::Dispose

  .method /*06000079*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 71
  {
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::DisposeAsync

  .method /*0600007A*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 08 12 5D
  {
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::EndRead

  .method /*0600007B*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000017*/ asyncResult) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x2221
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::EndWrite

  .method /*0600007C*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2223
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::Flush

  .method /*0600007D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 01 12 69 11 6D
  {
    // Method begins at RVA 0x2225
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::FlushAsync

  .method /*0600007E*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x2228
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::Read

  .method /*0600007F*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001D*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 75 01 05
  {
    .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x222b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::Read

  .method /*06000080*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100001E*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 79 01 08 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x222e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::ReadAsync

  .method /*06000081*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100001F*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000020*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 7D 01 08 15 11 80 81 01 05 11 6D
  {
    .custom /*0C000039:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080000B3*/  = nullref
    // Method begins at RVA 0x2231
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::ReadAsync

  .method /*06000082*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2234
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::ReadByte

  .method /*06000083*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000021*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 85
  {
    // Method begins at RVA 0x2237
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::Seek

  .method /*06000084*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x223a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::SetLength

  .method /*06000085*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x223c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::Write

  .method /*06000086*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000022*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 80 89 01 05
  {
    .custom /*0C00003C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x223e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::Write

  .method /*06000087*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 04 12 69 1D 05 08 08 11 6D
  {
    // Method begins at RVA 0x2240
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::WriteAsync

  .method /*06000088*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001C*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000023*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001B*/ cancellationToken) cil managed
  // SIG: 20 02 11 71 15 11 80 8D 01 05 11 6D
  {
    .custom /*0C00003D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080000C0*/  = nullref
    // Method begins at RVA 0x2243
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ZLibStream::WriteAsync

  .method /*06000089*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2246
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ZLibStream::WriteByte

  .property /*1700001C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/
          BaseStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000016*/ System.IO.Compression.ZLibStream/*0200000B*/::get_BaseStream() /* 0600006D */
  } // end of property ZLibStream::BaseStream
  .property /*1700001D*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Compression.ZLibStream/*0200000B*/::get_CanRead() /* 0600006E */
  } // end of property ZLibStream::CanRead
  .property /*1700001E*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Compression.ZLibStream/*0200000B*/::get_CanSeek() /* 0600006F */
  } // end of property ZLibStream::CanSeek
  .property /*1700001F*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Compression.ZLibStream/*0200000B*/::get_CanWrite() /* 06000070 */
  } // end of property ZLibStream::CanWrite
  .property /*17000020*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.ZLibStream/*0200000B*/::get_Length() /* 06000071 */
  } // end of property ZLibStream::Length
  .property /*17000021*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Compression.ZLibStream/*0200000B*/::get_Position() /* 06000072 */
    .set instance void System.IO.Compression.ZLibStream/*0200000B*/::set_Position(int64) /* 06000073 */
  } // end of property ZLibStream::Position
} // end of class System.IO.Compression.ZLibStream


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
