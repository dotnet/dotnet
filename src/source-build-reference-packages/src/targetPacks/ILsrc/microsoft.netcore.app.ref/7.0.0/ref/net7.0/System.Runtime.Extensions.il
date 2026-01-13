


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
// Time-date stamp:            0xa8d5f3f9
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003262
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
// Checksum:                       0x000078b6
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000320f [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004a4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00001e00 [0x00002880] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003120 [0x00000054] address [size] of Debug Directory:           
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
//              0x00001268 Virtual Size
//              0x00002000 Virtual Address
//              0x00001400 Size of Raw Data
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
//              0x000004a4 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00001600 Pointer to Raw Data
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
//              0x00001c00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000264
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003251 Import Name Table
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
// 0x00002050 [0x00001050] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000030a0 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000005a0 Size
//              '#~' Name
//    Stream 2:
//              0x0000060c Offset
//              0x000007a4 Size
//              '#Strings' Name
//    Stream 3:
//              0x00000db0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000db4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000dc4 Offset
//              0x0000028c Size
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

// File size            : 18048
// PE header size       : 512 (496 used)    ( 2.84%)
// PE additional info   : 11811             (65.44%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.40%)
// CLR meta-data size  : 4176               (23.14%)
// CLR additional info : 128                ( 0.71%)
// CLR method headers  : 0                  ( 0.00%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              (11.35%)
// Unaccounted          : -699              (-3.87%)

// Num.of PE sections   : 3
//   .text    - 5120
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 4176
//   Module        -    1 (10 bytes)
//   TypeRef       -   16 (96 bytes)
//   MemberRef     -   15 (90 bytes)
//   CustomAttribute-   18 (108 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   ExportedType  -   73 (1022 bytes)
//   TypeDef       -    1 (14 bytes)
//   Strings       -  1955 bytes
//   Blobs         -   652 bytes
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
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Runtime.Extensions
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000001:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000002:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000003:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000004:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000005:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000006:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C000007:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 19 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                        65 2E 45 78 74 65 6E 73 69 6F 6E 73 00 00 )       // e.Extensions..
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000B:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000C:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000D:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 19 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                       65 2E 45 78 74 65 6E 73 69 6F 6E 73 00 00 )       // e.Extensions..
  .custom /*0C00000E:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C00000F:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000010:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000011:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 19 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                 65 2E 45 78 74 65 6E 73 69 6F 6E 73 00 00 )       // e.Extensions..
  .custom /*0C000012:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
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
.class extern /*27000001*/ forwarder System.AppDomain
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000002*/ forwarder System.AppDomainSetup
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000003*/ forwarder System.AppDomainUnloadedException
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000004*/ forwarder System.ApplicationId
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000005*/ forwarder System.AssemblyLoadEventArgs
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000006*/ forwarder System.AssemblyLoadEventHandler
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000007*/ forwarder System.Base64FormattingOptions
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000008*/ forwarder System.BitConverter
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000009*/ forwarder System.CannotUnloadAppDomainException
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000A*/ forwarder System.CodeDom.Compiler.IndentedTextWriter
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000B*/ forwarder System.Collections.ArrayList
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000C*/ forwarder System.Collections.Comparer
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000D*/ forwarder System.Collections.Hashtable
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000E*/ forwarder System.Collections.IHashCodeProvider
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000F*/ forwarder System.ContextBoundObject
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000010*/ forwarder System.ContextMarshalException
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000011*/ forwarder System.ContextStaticAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000012*/ forwarder System.Convert
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000013*/ forwarder System.Diagnostics.Stopwatch
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000014*/ forwarder System.Environment
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000015*/ SpecialFolder
{
  .class extern System.Environment /*27000014*/ 
}
.class extern /*27000016*/ SpecialFolderOption
{
  .class extern System.Environment /*27000014*/ 
}
.class extern /*27000017*/ forwarder System.EnvironmentVariableTarget
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000018*/ forwarder System.Globalization.GlobalizationExtensions
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000019*/ forwarder System.IO.BinaryReader
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700001A*/ forwarder System.IO.BinaryWriter
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700001B*/ forwarder System.IO.BufferedStream
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700001C*/ forwarder System.IO.EndOfStreamException
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700001D*/ forwarder System.IO.InvalidDataException
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700001E*/ forwarder System.IO.MemoryStream
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700001F*/ forwarder System.IO.Path
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000020*/ forwarder System.IO.StreamReader
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000021*/ forwarder System.IO.StreamWriter
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000022*/ forwarder System.IO.StringReader
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000023*/ forwarder System.IO.StringWriter
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000024*/ forwarder System.IO.TextReader
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000025*/ forwarder System.IO.TextWriter
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000026*/ forwarder System.LoaderOptimization
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000027*/ forwarder System.LoaderOptimizationAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000028*/ forwarder System.Math
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000029*/ forwarder System.MathF
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700002A*/ forwarder System.MidpointRounding
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700002B*/ forwarder System.Net.WebUtility
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700002C*/ forwarder System.Numerics.BitOperations
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700002D*/ forwarder System.OperatingSystem
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700002E*/ forwarder System.PlatformID
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700002F*/ forwarder System.Progress`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000030*/ forwarder System.Random
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000031*/ forwarder System.Reflection.AssemblyNameProxy
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000032*/ forwarder System.ResolveEventHandler
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000033*/ forwarder System.Runtime.CompilerServices.SwitchExpressionException
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000034*/ forwarder System.Runtime.ProfileOptimization
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000035*/ forwarder System.Runtime.Versioning.ComponentGuaranteesAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000036*/ forwarder System.Runtime.Versioning.ComponentGuaranteesOptions
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000037*/ forwarder System.Runtime.Versioning.FrameworkName
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000038*/ forwarder System.Runtime.Versioning.ResourceConsumptionAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000039*/ forwarder System.Runtime.Versioning.ResourceExposureAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700003A*/ forwarder System.Runtime.Versioning.ResourceScope
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700003B*/ forwarder System.Runtime.Versioning.VersioningHelper
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700003C*/ forwarder System.Security.IPermission
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700003D*/ forwarder System.Security.ISecurityEncodable
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700003E*/ forwarder System.Security.IStackWalk
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700003F*/ forwarder System.Security.Permissions.CodeAccessSecurityAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000040*/ forwarder System.Security.Permissions.PermissionState
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000041*/ forwarder System.Security.Permissions.SecurityAction
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000042*/ forwarder System.Security.Permissions.SecurityAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000043*/ forwarder System.Security.Permissions.SecurityPermissionAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000044*/ forwarder System.Security.Permissions.SecurityPermissionFlag
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000045*/ forwarder System.Security.PermissionSet
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000046*/ forwarder System.Security.SecurityElement
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000047*/ forwarder System.StringComparer
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000048*/ forwarder System.StringNormalizationExtensions
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000049*/ forwarder System.UriBuilder
{
  .assembly extern System.Runtime /*23000001*/ 
}
.module System.Runtime.Extensions.dll
// MVID: {5d7c8a9e-e018-440f-9d69-c764cec10a0e}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY

// *********** DISASSEMBLY COMPLETE ***********************
