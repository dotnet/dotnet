


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
// Time-date stamp:            0xe3a8314f
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001600
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003476
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
// Checksum:                       0x00014177
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003424 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004c4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002000 [0x00002930] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003408 [0x0000001c] address [size] of Debug Directory:           
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
//              0x0000147c Virtual Size
//              0x00002000 Virtual Address
//              0x00001600 Size of Raw Data
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
//              0x00001800 Pointer to Raw Data
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
//              0x00001e00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000478
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003466 Import Name Table
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
// 0x000020f8 [0x00001290] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003388 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000814 Size
//              '#~' Name
//    Stream 2:
//              0x00000880 Offset
//              0x00000698 Size
//              '#Strings' Name
//    Stream 3:
//              0x00000f18 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000f1c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000f2c Offset
//              0x00000364 Size
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

// File size            : 18736
// PE header size       : 512 (496 used)    ( 2.73%)
// PE additional info   : 11963             (63.85%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.38%)
// CLR meta-data size  : 4752               (25.36%)
// CLR additional info : 128                ( 0.68%)
// CLR method headers  : 45                 ( 0.24%)
// Managed code         : 121               ( 0.65%)
// Data                 : 2048              (10.93%)
// Unaccounted          : -905              (-4.83%)

// Num.of PE sections   : 3
//   .text    - 5632
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 4752
//   Module        -    1 (10 bytes)
//   TypeDef       -    5 (70 bytes)      0 interfaces, 0 explicit layout
//   TypeRef       -   41 (246 bytes)
//   MethodDef     -   45 (630 bytes)     0 abstract, 0 native, 45 bodies
//   FieldDef      -    5 (30 bytes)      0 constant
//   MemberRef     -   23 (138 bytes)
//   ParamDef      -   78 (468 bytes)
//   Constant      -    3 (18 bytes)
//   CustomAttribute-   33 (198 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    2 (8 bytes)
//   Property      -    7 (42 bytes)
//   MethodSemantic-    9 (54 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   Strings       -  1688 bytes
//   Blobs         -   868 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   194 bytes

// CLR additional info : 128

// CLR method headers : 45
//   Num.of method bodies  - 45
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 45

// Managed code : 121
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class BrotliCompressionOptions       (public) (auto) (ansi) (sealed) 
// Class BrotliDecoder                  (public) (sequential) (ansi) (sealed) 
// Class BrotliEncoder                  (public) (sequential) (ansi) (sealed) 
// Class BrotliStream                   (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly extern /*23000002*/ System.IO.Compression
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 9:0:0:0
}
.assembly /*20000001*/ noplatform System.IO.Compression.Brotli
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 1C 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                        70 72 65 73 73 69 6F 6E 2E 42 72 6F 74 6C 69 00   // pression.Brotli.
                                                                                                                                                                        00 ) 
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 1C 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                       70 72 65 73 73 69 6F 6E 2E 42 72 6F 74 6C 69 00   // pression.Brotli.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 1C 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // ...System.IO.Com
                                                                                                                                                                 70 72 65 73 73 69 6F 6E 2E 42 72 6F 74 6C 69 00   // pression.Brotli.
                                                                                                                                                                 00 ) 
  .custom /*0C000016:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 9:0:0:0
}
.module System.IO.Compression.Brotli.dll
// MVID: {769e46ac-202e-4eb7-895e-b0d5f794fee0}
.custom /*0C000003:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000004:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit System.IO.Compression.BrotliCompressionOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000014*/
{
  .field /*04000001*/ private int32 '<Quality>k__BackingField'
  .custom /*0C000002:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname 
          instance int32  get_Quality() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000001:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000001       */ ldfld      int32 System.IO.Compression.BrotliCompressionOptions/*02000002*/::'<Quality>k__BackingField' /* 04000001 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliCompressionOptions::get_Quality

  .method /*06000002*/ public hidebysig specialname 
          instance void  set_Quality(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C000017:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2058
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000001       */ stfld      int32 System.IO.Compression.BrotliCompressionOptions/*02000002*/::'<Quality>k__BackingField' /* 04000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method BrotliCompressionOptions::set_Quality

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2061
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliCompressionOptions::.ctor

  .property /*17000001*/ instance callconv(8) int32
          Quality()
  {
    .get instance int32 System.IO.Compression.BrotliCompressionOptions/*02000002*/::get_Quality() /* 06000001 */
    .set instance void System.IO.Compression.BrotliCompressionOptions/*02000002*/::set_Quality(int32) /* 06000002 */
  } // end of property BrotliCompressionOptions::Quality
} // end of class System.IO.Compression.BrotliCompressionOptions

.class /*02000003*/ public sequential ansi sealed beforefieldinit System.IO.Compression.BrotliDecoder
       extends [System.Runtime/*23000001*/]System.ValueType/*01000016*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000017*/
{
  .field /*04000002*/ private object _dummy
  .field /*04000003*/ private int32 _dummyPrimitive
  .method /*06000004*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Buffers.OperationStatus/*01000018*/ 
          Decompress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000019*/<uint8> source,
                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001A*/<uint8> destination,
                     [out] int32& bytesConsumed,
                     [out] int32& bytesWritten) cil managed
  // SIG: 20 04 11 61 15 11 65 01 05 15 11 69 01 05 10 08 10 08
  {
    // Method begins at RVA 0x2069
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliDecoder::Decompress

  .method /*06000005*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliDecoder::Dispose

  .method /*06000006*/ public hidebysig static 
          bool  TryDecompress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000019*/<uint8> source,
                              valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001A*/<uint8> destination,
                              [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 65 01 05 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliDecoder::TryDecompress

} // end of class System.IO.Compression.BrotliDecoder

.class /*02000004*/ public sequential ansi sealed beforefieldinit System.IO.Compression.BrotliEncoder
       extends [System.Runtime/*23000001*/]System.ValueType/*01000016*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000017*/
{
  .field /*04000004*/ private object _dummy
  .field /*04000005*/ private int32 _dummyPrimitive
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 quality,
                               int32 window) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x2071
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::.ctor

  .method /*06000008*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Buffers.OperationStatus/*01000018*/ 
          Compress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000019*/<uint8> source,
                   valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001A*/<uint8> destination,
                   [out] int32& bytesConsumed,
                   [out] int32& bytesWritten,
                   bool isFinalBlock) cil managed
  // SIG: 20 05 11 61 15 11 65 01 05 15 11 69 01 05 10 08 10 08 02
  {
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::Compress

  .method /*06000009*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2077
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliEncoder::Dispose

  .method /*0600000A*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Buffers.OperationStatus/*01000018*/ 
          Flush(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001A*/<uint8> destination,
                [out] int32& bytesWritten) cil managed
  // SIG: 20 02 11 61 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x2079
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::Flush

  .method /*0600000B*/ public hidebysig static 
          int32  GetMaxCompressedLength(int32 inputSize) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x207c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::GetMaxCompressedLength

  .method /*0600000C*/ public hidebysig static 
          bool  TryCompress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000019*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001A*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 65 01 05 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x207f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::TryCompress

  .method /*0600000D*/ public hidebysig static 
          bool  TryCompress(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000019*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001A*/<uint8> destination,
                            [out] int32& bytesWritten,
                            int32 quality,
                            int32 window) cil managed
  // SIG: 00 05 02 15 11 65 01 05 15 11 69 01 05 10 08 08 08
  {
    // Method begins at RVA 0x2082
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliEncoder::TryCompress

} // end of class System.IO.Compression.BrotliEncoder

.class /*02000005*/ public auto ansi sealed beforefieldinit System.IO.Compression.BrotliStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/
{
  .custom /*0C000018:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000019:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001C*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/ 'stream',
                               class System.IO.Compression.BrotliCompressionOptions/*02000002*/ compressionOptions,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 03 01 12 75 12 08 02
  {
    .param [3]/*0800001D*/  = bool(false)
    // Method begins at RVA 0x2085
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*0600000F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/ 'stream',
                               valuetype [System.IO.Compression/*23000002*/]System.IO.Compression.CompressionLevel/*0100001E*/ compressionLevel) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    // Method begins at RVA 0x208d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/ 'stream',
                               valuetype [System.IO.Compression/*23000002*/]System.IO.Compression.CompressionLevel/*0100001E*/ compressionLevel,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 75 11 79 02
  {
    // Method begins at RVA 0x2095
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/ 'stream',
                               valuetype [System.IO.Compression/*23000002*/]System.IO.Compression.CompressionMode/*0100001F*/ mode) cil managed
  // SIG: 20 02 01 12 75 11 7D
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*06000012*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/ 'stream',
                               valuetype [System.IO.Compression/*23000002*/]System.IO.Compression.CompressionMode/*0100001F*/ mode,
                               bool leaveOpen) cil managed
  // SIG: 20 03 01 12 75 11 7D 02
  {
    // Method begins at RVA 0x20a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrotliStream::.ctor

  .method /*06000013*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/ 
          get_BaseStream() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_BaseStream

  .method /*06000014*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_CanRead

  .method /*06000015*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_CanSeek

  .method /*06000016*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_CanWrite

  .method /*06000017*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_Length

  .method /*06000018*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::get_Position

  .method /*06000019*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x20bf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::set_Position

  .method /*0600001A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000020*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*01000021*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 80 81 1D 05 08 08 12 80 85 1C
  {
    .param [4]/*0800002C*/ 
    .custom /*0C00001E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001C*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800002D*/ 
    .custom /*0C00001F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001C*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::BeginRead

  .method /*0600001B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000020*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*01000021*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 80 81 1D 05 08 08 12 80 85 1C
  {
    .param [4]/*08000031*/ 
    .custom /*0C000020:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001C*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000032*/ 
    .custom /*0C000021:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001C*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::BeginWrite

  .method /*0600001C*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::Dispose

  .method /*0600001D*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000022*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 89
  {
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::DisposeAsync

  .method /*0600001E*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000020*/ asyncResult) cil managed
  // SIG: 20 01 08 12 80 81
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::EndRead

  .method /*0600001F*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000020*/ asyncResult) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x20cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::EndWrite

  .method /*06000020*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::Flush

  .method /*06000021*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000023*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000024*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 8D 11 80 91
  {
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::FlushAsync

  .method /*06000022*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x20d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::Read

  .method /*06000023*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001A*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 69 01 05
  {
    .custom /*0C00001A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::Read

  .method /*06000024*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000025*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000024*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 95 01 08 1D 05 08 08 11 80 91
  {
    // Method begins at RVA 0x20dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::ReadAsync

  .method /*06000025*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000026*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000027*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000024*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 99 01 08 15 11 80 9D 01 05 11 80 91
  {
    .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000040*/  = nullref
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::ReadAsync

  .method /*06000026*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::ReadByte

  .method /*06000027*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000028*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 A1
  {
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::Seek

  .method /*06000028*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x20e8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::SetLength

  .method /*06000029*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x20ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::Write

  .method /*0600002A*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000019*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 65 01 05
  {
    .custom /*0C00001C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::Write

  .method /*0600002B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000023*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000024*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 8D 1D 05 08 08 11 80 91
  {
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::WriteAsync

  .method /*0600002C*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000022*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000029*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000024*/ cancellationToken) cil managed
  // SIG: 20 02 11 80 89 15 11 80 A5 01 05 11 80 91
  {
    .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800004D*/  = nullref
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrotliStream::WriteAsync

  .method /*0600002D*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x20f4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BrotliStream::WriteByte

  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/
          BaseStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001D*/ System.IO.Compression.BrotliStream/*02000005*/::get_BaseStream() /* 06000013 */
  } // end of property BrotliStream::BaseStream
  .property /*17000003*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Compression.BrotliStream/*02000005*/::get_CanRead() /* 06000014 */
  } // end of property BrotliStream::CanRead
  .property /*17000004*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Compression.BrotliStream/*02000005*/::get_CanSeek() /* 06000015 */
  } // end of property BrotliStream::CanSeek
  .property /*17000005*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Compression.BrotliStream/*02000005*/::get_CanWrite() /* 06000016 */
  } // end of property BrotliStream::CanWrite
  .property /*17000006*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Compression.BrotliStream/*02000005*/::get_Length() /* 06000017 */
  } // end of property BrotliStream::Length
  .property /*17000007*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Compression.BrotliStream/*02000005*/::get_Position() /* 06000018 */
    .set instance void System.IO.Compression.BrotliStream/*02000005*/::set_Position(int64) /* 06000019 */
  } // end of property BrotliStream::Position
} // end of class System.IO.Compression.BrotliStream


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
