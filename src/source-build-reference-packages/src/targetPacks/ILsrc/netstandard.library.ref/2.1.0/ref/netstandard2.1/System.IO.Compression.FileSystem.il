


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
// Time-date stamp:            0xf87b9fac
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00000800
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000278a
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
// Checksum:                       0x00011096
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00002736 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004fc] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00001200 [0x00002378] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00002674 [0x00000038] address [size] of Debug Directory:           
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
//              0x00000790 Virtual Size
//              0x00002000 Virtual Address
//              0x00000800 Size of Raw Data
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
//              0x000004fc Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00000a00 Pointer to Raw Data
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
//              0x00001000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00002000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x0000078c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00002778 Import Name Table
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
// 0x00002050 [0x000005a4] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000025f4 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000188 Size
//              '#~' Name
//    Stream 2:
//              0x000001f4 Offset
//              0x0000024c Size
//              '#Strings' Name
//    Stream 3:
//              0x00000440 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000444 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000454 Offset
//              0x00000150 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000008900001407 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 13688
// PE header size       : 512 (496 used)    ( 3.74%)
// PE additional info   : 10583             (77.32%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.53%)
// CLR meta-data size  : 1444               (10.55%)
// CLR additional info : 128                ( 0.94%)
// CLR method headers  : 0                  ( 0.00%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              (14.96%)
// Unaccounted          : -1099             (-8.03%)

// Num.of PE sections   : 3
//   .text    - 2048
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 1444
//   Module        -    1 (10 bytes)
//   TypeRef       -   14 (84 bytes)
//   MemberRef     -   13 (78 bytes)
//   CustomAttribute-   13 (78 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   ExportedType  -    2 (28 bytes)
//   TypeDef       -    1 (14 bytes)
//   Strings       -   587 bytes
//   Blobs         -   336 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   167 bytes

// CLR additional info : 128

// CLR method headers : 0
//   Num.of method bodies  - 0
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 0

// Managed code : 0
// No classes defined in this module

// Metadata version: v4.0.30319
.assembly extern /*23000001*/ netstandard
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 2:1:0:0
}
.assembly /*20000001*/ noplatform System.IO.Compression.FileSystem
{
  .custom instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                    string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000001:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000002:0A000002*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000003:0A000003*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [netstandard/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000004:0A000004*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 20 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // .. System.IO.Com
                                                                                                                                                                     70 72 65 73 73 69 6F 6E 2E 46 69 6C 65 53 79 73   // pression.FileSys
                                                                                                                                                                     74 65 6D 00 00 )                                  // tem..
  .custom /*0C000005:0A000005*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000006:0A000006*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                  43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                  20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                  2E 00 00 )                                        // ...
  .custom /*0C000007:0A000007*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 20 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // .. System.IO.Com
                                                                                                                                                                    70 72 65 73 73 69 6F 6E 2E 46 69 6C 65 53 79 73   // pression.FileSys
                                                                                                                                                                    74 65 6D 00 00 )                                  // tem..
  .custom /*0C000008:0A000008*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 0E 34 2E 37 30 30 2E 31 39 2E 34 36 32 30   // ...4.700.19.4620
                                                                                                                                                                    35 00 00 )                                        // 5..
  .custom /*0C000009:0A000009*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 2E 34 2E 37 2E 30 2B 61 35 62 35 66 32 65   // ...4.7.0+a5b5f2e
                                                                                                                                                                             31 65 33 36 39 39 37 32 63 38 66 66 31 65 32 31   // 1e369972c8ff1e21
                                                                                                                                                                             38 33 39 37 39 66 61 62 36 30 39 39 66 35 32 65   // 83979fab6099f52e
                                                                                                                                                                             66 00 00 )                                        // f..
  .custom /*0C00000A:0A00000A*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C00000B:0A00000B*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 20 53 79 73 74 65 6D 2E 49 4F 2E 43 6F 6D   // .. System.IO.Com
                                                                                                                                                              70 72 65 73 73 69 6F 6E 2E 46 69 6C 65 53 79 73   // pression.FileSys
                                                                                                                                                              74 65 6D 00 00 )                                  // tem..
  .custom /*0C00000C:0A00000C*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*0100000D*/::.ctor() /* 0A00000C */ = ( 01 00 00 00 ) 
  .custom /*0C00000D:0A00000D*/ instance void [netstandard/*23000001*/]System.CLSCompliantAttribute/*0100000E*/::.ctor(bool) /* 0A00000D */ = ( 01 00 01 00 00 ) 
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.class extern /*27000001*/ forwarder System.IO.Compression.ZipFile
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000002*/ forwarder System.IO.Compression.ZipFileExtensions
{
  .assembly extern netstandard /*23000001*/ 
}
.module System.IO.Compression.FileSystem.dll
// MVID: {86d8fd3e-e713-4b78-821c-889d6c8610bf}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY

// *********** DISASSEMBLY COMPLETE ***********************
