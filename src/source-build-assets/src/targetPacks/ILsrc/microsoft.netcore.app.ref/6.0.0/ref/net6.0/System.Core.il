


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
// Time-date stamp:            0xed46dd69
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00004a92
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
// Checksum:                       0x0000a9cc
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00004a3f [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000434] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003600 [0x00002468] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00004970 [0x00000054] address [size] of Debug Directory:           
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
//              0x00002a98 Virtual Size
//              0x00002000 Virtual Address
//              0x00002c00 Size of Raw Data
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
//              0x00000434 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002e00 Pointer to Raw Data
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
//              0x00003400 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000a94
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00004a81 Import Name Table
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
// 0x00002050 [0x000028a0] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000048f0 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000010a8 Size
//              '#~' Name
//    Stream 2:
//              0x00001114 Offset
//              0x000014c8 Size
//              '#Strings' Name
//    Stream 3:
//              0x000025dc Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000025e0 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000025f0 Offset
//              0x000002b0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000008900005407 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 23144
// PE header size       : 512 (496 used)    ( 2.21%)
// PE additional info   : 10651             (46.02%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.31%)
// CLR meta-data size  : 10400              (44.94%)
// CLR additional info : 128                ( 0.55%)
// CLR method headers  : 0                  ( 0.00%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 8.85%)
// Unaccounted          : -667              (-2.88%)

// Num.of PE sections   : 3
//   .text    - 11264
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 10400
//   Module        -    1 (10 bytes)
//   TypeRef       -   19 (114 bytes)
//   MemberRef     -   17 (102 bytes)
//   CustomAttribute-   20 (120 bytes)
//   Security      -    1 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   17 (340 bytes)
//   ExportedType  -  248 (3472 bytes)
//   TypeDef       -    1 (14 bytes)
//   Strings       -  5318 bytes
//   Blobs         -   688 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   174 bytes

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
  .ver 0:0:0:0
}
.assembly extern /*23000002*/ System.IO.MemoryMappedFiles
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000003*/ System.Security.Cryptography.Cng
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000004*/ System.IO.Pipes
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000005*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000006*/ System.Diagnostics.EventLog
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 0:0:0:0
}
.assembly extern /*23000007*/ System.Diagnostics.PerformanceCounter
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 0:0:0:0
}
.assembly extern /*23000008*/ System.Linq.Expressions
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000009*/ System.IO.Pipes.AccessControl
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*2300000A*/ System.Linq
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*2300000B*/ System.Linq.Queryable
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*2300000C*/ System.Linq.Parallel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*2300000D*/ System.Runtime.InteropServices
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*2300000E*/ System.Security.Cryptography.Algorithms
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*2300000F*/ System.Security.Cryptography.Csp
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000010*/ System.Security.Cryptography.X509Certificates
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly extern /*23000011*/ System.Threading
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 0:0:0:0
}
.assembly /*20000001*/ System.Core
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
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000006*/::.ctor(bool) /* 0A000005 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 0B 53 79 73 74 65 6D 2E 43 6F 72 65 00 00 ) // ...System.Core..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 0B 53 79 73 74 65 6D 2E 43 6F 72 65 00 00 ) // ...System.Core..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 0B 53 79 73 74 65 6D 2E 43 6F 72 65 00 00 ) // ...System.Core..
  .custom /*0C000014:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .permissionset reqmin
             = {[System.Runtime/*23000001*/]System.Security.Permissions.SecurityPermissionAttribute/*01000011*/ = {property bool 'SkipVerification' = bool(true)}}
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.class extern /*27000001*/ forwarder Microsoft.Win32.SafeHandles.SafeMemoryMappedFileHandle
{
  .assembly extern System.IO.MemoryMappedFiles /*23000002*/ 
}
.class extern /*27000002*/ forwarder Microsoft.Win32.SafeHandles.SafeMemoryMappedViewHandle
{
  .assembly extern System.IO.MemoryMappedFiles /*23000002*/ 
}
.class extern /*27000003*/ forwarder Microsoft.Win32.SafeHandles.SafeNCryptHandle
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*27000004*/ forwarder Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*27000005*/ forwarder Microsoft.Win32.SafeHandles.SafeNCryptProviderHandle
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*27000006*/ forwarder Microsoft.Win32.SafeHandles.SafeNCryptSecretHandle
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*27000007*/ forwarder Microsoft.Win32.SafeHandles.SafePipeHandle
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*27000008*/ forwarder System.Action
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000009*/ forwarder System.Action`10
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000A*/ forwarder System.Action`11
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000B*/ forwarder System.Action`12
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000C*/ forwarder System.Action`13
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000D*/ forwarder System.Action`14
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000E*/ forwarder System.Action`15
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700000F*/ forwarder System.Action`16
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000010*/ forwarder System.Action`2
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000011*/ forwarder System.Action`3
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000012*/ forwarder System.Action`4
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000013*/ forwarder System.Action`9
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000014*/ forwarder System.Collections.Generic.HashSet`1
{
  .assembly extern System.Collections /*23000005*/ 
}
.class extern /*27000015*/ Enumerator
{
  .class extern System.Collections.Generic.HashSet`1 /*27000014*/ 
}
.class extern /*27000016*/ forwarder System.Diagnostics.Eventing.Reader.EventBookmark
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000017*/ forwarder System.Diagnostics.Eventing.Reader.EventKeyword
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000018*/ forwarder System.Diagnostics.Eventing.Reader.EventLevel
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000019*/ forwarder System.Diagnostics.Eventing.Reader.EventLogConfiguration
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700001A*/ forwarder System.Diagnostics.Eventing.Reader.EventLogException
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700001B*/ forwarder System.Diagnostics.Eventing.Reader.EventLogInformation
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700001C*/ forwarder System.Diagnostics.Eventing.Reader.EventLogInvalidDataException
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700001D*/ forwarder System.Diagnostics.Eventing.Reader.EventLogIsolation
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700001E*/ forwarder System.Diagnostics.Eventing.Reader.EventLogLink
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700001F*/ forwarder System.Diagnostics.Eventing.Reader.EventLogMode
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000020*/ forwarder System.Diagnostics.Eventing.Reader.EventLogNotFoundException
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000021*/ forwarder System.Diagnostics.Eventing.Reader.EventLogPropertySelector
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000022*/ forwarder System.Diagnostics.Eventing.Reader.EventLogProviderDisabledException
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000023*/ forwarder System.Diagnostics.Eventing.Reader.EventLogQuery
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000024*/ forwarder System.Diagnostics.Eventing.Reader.EventLogReader
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000025*/ forwarder System.Diagnostics.Eventing.Reader.EventLogReadingException
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000026*/ forwarder System.Diagnostics.Eventing.Reader.EventLogRecord
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000027*/ forwarder System.Diagnostics.Eventing.Reader.EventLogSession
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000028*/ forwarder System.Diagnostics.Eventing.Reader.EventLogStatus
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000029*/ forwarder System.Diagnostics.Eventing.Reader.EventLogType
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700002A*/ forwarder System.Diagnostics.Eventing.Reader.EventLogWatcher
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700002B*/ forwarder System.Diagnostics.Eventing.Reader.EventMetadata
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700002C*/ forwarder System.Diagnostics.Eventing.Reader.EventOpcode
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700002D*/ forwarder System.Diagnostics.Eventing.Reader.EventProperty
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700002E*/ forwarder System.Diagnostics.Eventing.Reader.EventRecord
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*2700002F*/ forwarder System.Diagnostics.Eventing.Reader.EventRecordWrittenEventArgs
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000030*/ forwarder System.Diagnostics.Eventing.Reader.EventTask
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000031*/ forwarder System.Diagnostics.Eventing.Reader.PathType
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000032*/ forwarder System.Diagnostics.Eventing.Reader.ProviderMetadata
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000033*/ forwarder System.Diagnostics.Eventing.Reader.SessionAuthentication
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000034*/ forwarder System.Diagnostics.Eventing.Reader.StandardEventKeywords
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000035*/ forwarder System.Diagnostics.Eventing.Reader.StandardEventLevel
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000036*/ forwarder System.Diagnostics.Eventing.Reader.StandardEventOpcode
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000037*/ forwarder System.Diagnostics.Eventing.Reader.StandardEventTask
{
  .assembly extern System.Diagnostics.EventLog /*23000006*/ 
}
.class extern /*27000038*/ forwarder System.Diagnostics.PerformanceData.CounterData
{
  .assembly extern System.Diagnostics.PerformanceCounter /*23000007*/ 
}
.class extern /*27000039*/ forwarder System.Diagnostics.PerformanceData.CounterSet
{
  .assembly extern System.Diagnostics.PerformanceCounter /*23000007*/ 
}
.class extern /*2700003A*/ forwarder System.Diagnostics.PerformanceData.CounterSetInstance
{
  .assembly extern System.Diagnostics.PerformanceCounter /*23000007*/ 
}
.class extern /*2700003B*/ forwarder System.Diagnostics.PerformanceData.CounterSetInstanceCounterDataSet
{
  .assembly extern System.Diagnostics.PerformanceCounter /*23000007*/ 
}
.class extern /*2700003C*/ forwarder System.Diagnostics.PerformanceData.CounterSetInstanceType
{
  .assembly extern System.Diagnostics.PerformanceCounter /*23000007*/ 
}
.class extern /*2700003D*/ forwarder System.Diagnostics.PerformanceData.CounterType
{
  .assembly extern System.Diagnostics.PerformanceCounter /*23000007*/ 
}
.class extern /*2700003E*/ forwarder System.Dynamic.BinaryOperationBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700003F*/ forwarder System.Dynamic.BindingRestrictions
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000040*/ forwarder System.Dynamic.CallInfo
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000041*/ forwarder System.Dynamic.ConvertBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000042*/ forwarder System.Dynamic.CreateInstanceBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000043*/ forwarder System.Dynamic.DeleteIndexBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000044*/ forwarder System.Dynamic.DeleteMemberBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000045*/ forwarder System.Dynamic.DynamicMetaObject
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000046*/ forwarder System.Dynamic.DynamicMetaObjectBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000047*/ forwarder System.Dynamic.DynamicObject
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000048*/ forwarder System.Dynamic.ExpandoObject
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000049*/ forwarder System.Dynamic.GetIndexBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700004A*/ forwarder System.Dynamic.GetMemberBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700004B*/ forwarder System.Dynamic.IDynamicMetaObjectProvider
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700004C*/ forwarder System.Dynamic.IInvokeOnGetBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700004D*/ forwarder System.Dynamic.InvokeBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700004E*/ forwarder System.Dynamic.InvokeMemberBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700004F*/ forwarder System.Dynamic.SetIndexBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000050*/ forwarder System.Dynamic.SetMemberBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000051*/ forwarder System.Dynamic.UnaryOperationBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000052*/ forwarder System.Func`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000053*/ forwarder System.Func`10
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000054*/ forwarder System.Func`11
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000055*/ forwarder System.Func`12
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000056*/ forwarder System.Func`13
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000057*/ forwarder System.Func`14
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000058*/ forwarder System.Func`15
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000059*/ forwarder System.Func`16
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700005A*/ forwarder System.Func`17
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700005B*/ forwarder System.Func`2
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700005C*/ forwarder System.Func`3
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700005D*/ forwarder System.Func`4
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700005E*/ forwarder System.Func`5
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*2700005F*/ forwarder System.InvalidTimeZoneException
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000060*/ forwarder System.IO.HandleInheritability
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000061*/ forwarder System.IO.MemoryMappedFiles.MemoryMappedFile
{
  .assembly extern System.IO.MemoryMappedFiles /*23000002*/ 
}
.class extern /*27000062*/ forwarder System.IO.MemoryMappedFiles.MemoryMappedFileAccess
{
  .assembly extern System.IO.MemoryMappedFiles /*23000002*/ 
}
.class extern /*27000063*/ forwarder System.IO.MemoryMappedFiles.MemoryMappedFileOptions
{
  .assembly extern System.IO.MemoryMappedFiles /*23000002*/ 
}
.class extern /*27000064*/ forwarder System.IO.MemoryMappedFiles.MemoryMappedFileRights
{
  .assembly extern System.IO.MemoryMappedFiles /*23000002*/ 
}
.class extern /*27000065*/ forwarder System.IO.MemoryMappedFiles.MemoryMappedViewAccessor
{
  .assembly extern System.IO.MemoryMappedFiles /*23000002*/ 
}
.class extern /*27000066*/ forwarder System.IO.MemoryMappedFiles.MemoryMappedViewStream
{
  .assembly extern System.IO.MemoryMappedFiles /*23000002*/ 
}
.class extern /*27000067*/ forwarder System.IO.Pipes.AnonymousPipeClientStream
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*27000068*/ forwarder System.IO.Pipes.AnonymousPipeServerStream
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*27000069*/ forwarder System.IO.Pipes.NamedPipeClientStream
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*2700006A*/ forwarder System.IO.Pipes.NamedPipeServerStream
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*2700006B*/ forwarder System.IO.Pipes.PipeAccessRights
{
  .assembly extern System.IO.Pipes.AccessControl /*23000009*/ 
}
.class extern /*2700006C*/ forwarder System.IO.Pipes.PipeAccessRule
{
  .assembly extern System.IO.Pipes.AccessControl /*23000009*/ 
}
.class extern /*2700006D*/ forwarder System.IO.Pipes.PipeAuditRule
{
  .assembly extern System.IO.Pipes.AccessControl /*23000009*/ 
}
.class extern /*2700006E*/ forwarder System.IO.Pipes.PipeDirection
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*2700006F*/ forwarder System.IO.Pipes.PipeOptions
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*27000070*/ forwarder System.IO.Pipes.PipeSecurity
{
  .assembly extern System.IO.Pipes.AccessControl /*23000009*/ 
}
.class extern /*27000071*/ forwarder System.IO.Pipes.PipeStream
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*27000072*/ forwarder System.IO.Pipes.PipeStreamImpersonationWorker
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*27000073*/ forwarder System.IO.Pipes.PipeTransmissionMode
{
  .assembly extern System.IO.Pipes /*23000004*/ 
}
.class extern /*27000074*/ forwarder System.Lazy`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000075*/ forwarder System.Linq.Enumerable
{
  .assembly extern System.Linq /*2300000A*/ 
}
.class extern /*27000076*/ forwarder System.Linq.EnumerableExecutor
{
  .assembly extern System.Linq.Queryable /*2300000B*/ 
}
.class extern /*27000077*/ forwarder System.Linq.EnumerableExecutor`1
{
  .assembly extern System.Linq.Queryable /*2300000B*/ 
}
.class extern /*27000078*/ forwarder System.Linq.EnumerableQuery
{
  .assembly extern System.Linq.Queryable /*2300000B*/ 
}
.class extern /*27000079*/ forwarder System.Linq.EnumerableQuery`1
{
  .assembly extern System.Linq.Queryable /*2300000B*/ 
}
.class extern /*2700007A*/ forwarder System.Linq.Expressions.BinaryExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700007B*/ forwarder System.Linq.Expressions.BlockExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700007C*/ forwarder System.Linq.Expressions.CatchBlock
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700007D*/ forwarder System.Linq.Expressions.ConditionalExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700007E*/ forwarder System.Linq.Expressions.ConstantExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700007F*/ forwarder System.Linq.Expressions.DebugInfoExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000080*/ forwarder System.Linq.Expressions.DefaultExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000081*/ forwarder System.Linq.Expressions.DynamicExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000082*/ forwarder System.Linq.Expressions.DynamicExpressionVisitor
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000083*/ forwarder System.Linq.Expressions.ElementInit
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000084*/ forwarder System.Linq.Expressions.Expression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000085*/ forwarder System.Linq.Expressions.Expression`1
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000086*/ forwarder System.Linq.Expressions.ExpressionType
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000087*/ forwarder System.Linq.Expressions.ExpressionVisitor
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000088*/ forwarder System.Linq.Expressions.GotoExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000089*/ forwarder System.Linq.Expressions.GotoExpressionKind
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700008A*/ forwarder System.Linq.Expressions.IArgumentProvider
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700008B*/ forwarder System.Linq.Expressions.IDynamicExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700008C*/ forwarder System.Linq.Expressions.IndexExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700008D*/ forwarder System.Linq.Expressions.InvocationExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700008E*/ forwarder System.Linq.Expressions.LabelExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700008F*/ forwarder System.Linq.Expressions.LabelTarget
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000090*/ forwarder System.Linq.Expressions.LambdaExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000091*/ forwarder System.Linq.Expressions.ListInitExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000092*/ forwarder System.Linq.Expressions.LoopExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000093*/ forwarder System.Linq.Expressions.MemberAssignment
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000094*/ forwarder System.Linq.Expressions.MemberBinding
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000095*/ forwarder System.Linq.Expressions.MemberBindingType
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000096*/ forwarder System.Linq.Expressions.MemberExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000097*/ forwarder System.Linq.Expressions.MemberInitExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000098*/ forwarder System.Linq.Expressions.MemberListBinding
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*27000099*/ forwarder System.Linq.Expressions.MemberMemberBinding
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700009A*/ forwarder System.Linq.Expressions.MethodCallExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700009B*/ forwarder System.Linq.Expressions.NewArrayExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700009C*/ forwarder System.Linq.Expressions.NewExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700009D*/ forwarder System.Linq.Expressions.ParameterExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700009E*/ forwarder System.Linq.Expressions.RuntimeVariablesExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*2700009F*/ forwarder System.Linq.Expressions.SwitchCase
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000A0*/ forwarder System.Linq.Expressions.SwitchExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000A1*/ forwarder System.Linq.Expressions.SymbolDocumentInfo
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000A2*/ forwarder System.Linq.Expressions.TryExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000A3*/ forwarder System.Linq.Expressions.TypeBinaryExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000A4*/ forwarder System.Linq.Expressions.UnaryExpression
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000A5*/ forwarder System.Linq.IGrouping`2
{
  .assembly extern System.Linq /*2300000A*/ 
}
.class extern /*270000A6*/ forwarder System.Linq.ILookup`2
{
  .assembly extern System.Linq /*2300000A*/ 
}
.class extern /*270000A7*/ forwarder System.Linq.IOrderedEnumerable`1
{
  .assembly extern System.Linq /*2300000A*/ 
}
.class extern /*270000A8*/ forwarder System.Linq.IOrderedQueryable
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000A9*/ forwarder System.Linq.IOrderedQueryable`1
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000AA*/ forwarder System.Linq.IQueryable
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000AB*/ forwarder System.Linq.IQueryable`1
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000AC*/ forwarder System.Linq.IQueryProvider
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000AD*/ forwarder System.Linq.Lookup`2
{
  .assembly extern System.Linq /*2300000A*/ 
}
.class extern /*270000AE*/ forwarder System.Linq.OrderedParallelQuery`1
{
  .assembly extern System.Linq.Parallel /*2300000C*/ 
}
.class extern /*270000AF*/ forwarder System.Linq.ParallelEnumerable
{
  .assembly extern System.Linq.Parallel /*2300000C*/ 
}
.class extern /*270000B0*/ forwarder System.Linq.ParallelExecutionMode
{
  .assembly extern System.Linq.Parallel /*2300000C*/ 
}
.class extern /*270000B1*/ forwarder System.Linq.ParallelMergeOptions
{
  .assembly extern System.Linq.Parallel /*2300000C*/ 
}
.class extern /*270000B2*/ forwarder System.Linq.ParallelQuery
{
  .assembly extern System.Linq.Parallel /*2300000C*/ 
}
.class extern /*270000B3*/ forwarder System.Linq.ParallelQuery`1
{
  .assembly extern System.Linq.Parallel /*2300000C*/ 
}
.class extern /*270000B4*/ forwarder System.Linq.Queryable
{
  .assembly extern System.Linq.Queryable /*2300000B*/ 
}
.class extern /*270000B5*/ forwarder System.Runtime.CompilerServices.CallSite
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000B6*/ forwarder System.Runtime.CompilerServices.CallSite`1
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000B7*/ forwarder System.Runtime.CompilerServices.CallSiteBinder
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000B8*/ forwarder System.Runtime.CompilerServices.CallSiteHelpers
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000B9*/ forwarder System.Runtime.CompilerServices.DebugInfoGenerator
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000BA*/ forwarder System.Runtime.CompilerServices.DynamicAttribute
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000BB*/ forwarder System.Runtime.CompilerServices.ExtensionAttribute
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*270000BC*/ forwarder System.Runtime.CompilerServices.IRuntimeVariables
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000BD*/ forwarder System.Runtime.CompilerServices.IStrongBox
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*270000BE*/ forwarder System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000BF*/ forwarder System.Runtime.CompilerServices.RuleCache`1
{
  .assembly extern System.Linq.Expressions /*23000008*/ 
}
.class extern /*270000C0*/ forwarder System.Runtime.CompilerServices.StrongBox`1
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*270000C1*/ forwarder System.Runtime.InteropServices.ComAwareEventInfo
{
  .assembly extern System.Runtime.InteropServices /*2300000D*/ 
}
.class extern /*270000C2*/ forwarder System.Security.Cryptography.Aes
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000C3*/ forwarder System.Security.Cryptography.AesCng
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000C4*/ forwarder System.Security.Cryptography.AesCryptoServiceProvider
{
  .assembly extern System.Security.Cryptography.Csp /*2300000F*/ 
}
.class extern /*270000C5*/ forwarder System.Security.Cryptography.AesManaged
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000C6*/ forwarder System.Security.Cryptography.CngAlgorithm
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000C7*/ forwarder System.Security.Cryptography.CngAlgorithmGroup
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000C8*/ forwarder System.Security.Cryptography.CngExportPolicies
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000C9*/ forwarder System.Security.Cryptography.CngKey
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000CA*/ forwarder System.Security.Cryptography.CngKeyBlobFormat
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000CB*/ forwarder System.Security.Cryptography.CngKeyCreationOptions
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000CC*/ forwarder System.Security.Cryptography.CngKeyCreationParameters
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000CD*/ forwarder System.Security.Cryptography.CngKeyHandleOpenOptions
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000CE*/ forwarder System.Security.Cryptography.CngKeyOpenOptions
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000CF*/ forwarder System.Security.Cryptography.CngKeyUsages
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000D0*/ forwarder System.Security.Cryptography.CngProperty
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000D1*/ forwarder System.Security.Cryptography.CngPropertyCollection
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000D2*/ forwarder System.Security.Cryptography.CngPropertyOptions
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000D3*/ forwarder System.Security.Cryptography.CngProvider
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000D4*/ forwarder System.Security.Cryptography.CngUIPolicy
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000D5*/ forwarder System.Security.Cryptography.CngUIProtectionLevels
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000D6*/ forwarder System.Security.Cryptography.DSACng
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000D7*/ forwarder System.Security.Cryptography.ECCurve
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000D8*/ ECCurveType
{
  .class extern System.Security.Cryptography.ECCurve /*270000D7*/ 
}
.class extern /*270000D9*/ NamedCurves
{
  .class extern System.Security.Cryptography.ECCurve /*270000D7*/ 
}
.class extern /*270000DA*/ forwarder System.Security.Cryptography.ECDiffieHellman
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000DB*/ forwarder System.Security.Cryptography.ECDiffieHellmanCng
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000DC*/ forwarder System.Security.Cryptography.ECDiffieHellmanCngPublicKey
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000DD*/ forwarder System.Security.Cryptography.ECDiffieHellmanKeyDerivationFunction
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000DE*/ forwarder System.Security.Cryptography.ECDiffieHellmanPublicKey
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000DF*/ forwarder System.Security.Cryptography.ECDsa
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000E0*/ forwarder System.Security.Cryptography.ECDsaCng
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000E1*/ forwarder System.Security.Cryptography.ECKeyXmlFormat
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000E2*/ forwarder System.Security.Cryptography.ECParameters
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000E3*/ forwarder System.Security.Cryptography.ECPoint
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000E4*/ forwarder System.Security.Cryptography.IncrementalHash
{
  .assembly extern System.Security.Cryptography.Algorithms /*2300000E*/ 
}
.class extern /*270000E5*/ forwarder System.Security.Cryptography.RSACng
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000E6*/ forwarder System.Security.Cryptography.SHA256CryptoServiceProvider
{
  .assembly extern System.Security.Cryptography.Csp /*2300000F*/ 
}
.class extern /*270000E7*/ forwarder System.Security.Cryptography.SHA384CryptoServiceProvider
{
  .assembly extern System.Security.Cryptography.Csp /*2300000F*/ 
}
.class extern /*270000E8*/ forwarder System.Security.Cryptography.SHA512CryptoServiceProvider
{
  .assembly extern System.Security.Cryptography.Csp /*2300000F*/ 
}
.class extern /*270000E9*/ forwarder System.Security.Cryptography.TripleDESCng
{
  .assembly extern System.Security.Cryptography.Cng /*23000003*/ 
}
.class extern /*270000EA*/ forwarder System.Security.Cryptography.X509Certificates.CertificateRequest
{
  .assembly extern System.Security.Cryptography.X509Certificates /*23000010*/ 
}
.class extern /*270000EB*/ forwarder System.Security.Cryptography.X509Certificates.DSACertificateExtensions
{
  .assembly extern System.Security.Cryptography.X509Certificates /*23000010*/ 
}
.class extern /*270000EC*/ forwarder System.Security.Cryptography.X509Certificates.ECDsaCertificateExtensions
{
  .assembly extern System.Security.Cryptography.X509Certificates /*23000010*/ 
}
.class extern /*270000ED*/ forwarder System.Security.Cryptography.X509Certificates.RSACertificateExtensions
{
  .assembly extern System.Security.Cryptography.X509Certificates /*23000010*/ 
}
.class extern /*270000EE*/ forwarder System.Security.Cryptography.X509Certificates.SubjectAlternativeNameBuilder
{
  .assembly extern System.Security.Cryptography.X509Certificates /*23000010*/ 
}
.class extern /*270000EF*/ forwarder System.Security.Cryptography.X509Certificates.X509SignatureGenerator
{
  .assembly extern System.Security.Cryptography.X509Certificates /*23000010*/ 
}
.class extern /*270000F0*/ forwarder System.Threading.LazyThreadSafetyMode
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*270000F1*/ forwarder System.Threading.LockRecursionException
{
  .assembly extern System.Threading /*23000011*/ 
}
.class extern /*270000F2*/ forwarder System.Threading.LockRecursionPolicy
{
  .assembly extern System.Threading /*23000011*/ 
}
.class extern /*270000F3*/ forwarder System.Threading.ReaderWriterLockSlim
{
  .assembly extern System.Threading /*23000011*/ 
}
.class extern /*270000F4*/ forwarder System.Threading.Tasks.TaskExtensions
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*270000F5*/ forwarder System.TimeZoneInfo
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*270000F6*/ AdjustmentRule
{
  .class extern System.TimeZoneInfo /*270000F5*/ 
}
.class extern /*270000F7*/ TransitionTime
{
  .class extern System.TimeZoneInfo /*270000F5*/ 
}
.class extern /*270000F8*/ forwarder System.TimeZoneNotFoundException
{
  .assembly extern System.Runtime /*23000001*/ 
}
.module System.Core.dll
// MVID: {c7e04b0e-25cf-4b8c-a276-ed62c47c42b7}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000012*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.SkipLocalsInitAttribute/*01000013*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY

// *********** DISASSEMBLY COMPLETE ***********************
