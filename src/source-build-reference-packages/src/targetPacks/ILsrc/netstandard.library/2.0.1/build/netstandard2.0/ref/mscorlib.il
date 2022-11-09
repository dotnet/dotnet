
//  Microsoft (R) .NET IL Disassembler.  Version 5.0.0


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
// Time-date stamp:            0x59b9bd40
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00008c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000aaf2
// Base of code:                   0x00002000
// Base of data:                   0x0000c000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00010000
// Size of headers:                0x00000200
// Checksum:                       0x0001a106
// Subsystem:                      0x0003
// DLL characteristics:            0x8540
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000aaa0 [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x000004a0] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00009600 [0x00003e98] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000a968 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00008af8 Virtual Size
//              0x00002000 Virtual Address
//              0x00008c00 Size of Raw Data
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
//              0x000004a0 Virtual Size
//              0x0000c000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00008e00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x0000e000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00009400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000a000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000af4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000aae2 Import Name Table
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
// 0x00002050 [0x00008898] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000a8e8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00003c9c Size
//              '#~' Name
//    Stream 2:
//              0x00003d08 Offset
//              0x00004a04 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000870c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00008710 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00008720 Offset
//              0x00000178 Size
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

// File size            : 54424
// PE header size       : 512 (496 used)    ( 0.94%)
// PE additional info   : 17407             (31.98%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.13%)
// CLR meta-data size  : 34968              (64.25%)
// CLR additional info : 128                ( 0.24%)
// CLR method headers  : 0                  ( 0.00%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 3.76%)
// Unaccounted          : -711              (-1.31%)

// Num.of PE sections   : 3
//   .text    - 35840
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 34968
//   Module        -    1 (10 bytes)
//   TypeRef       -   14 (84 bytes)
//   MemberRef     -   13 (78 bytes)
//   CustomAttribute-   13 (78 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   ExportedType  - 1078 (15092 bytes)
//   TypeDef       -    1 (14 bytes)
//   Strings       - 18948 bytes
//   Blobs         -   376 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   166 bytes

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
  .ver 2:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Win32.Registry
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:1:0:0
}
.assembly extern /*23000003*/ System.Security.Principal.Windows
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:1:0:0
}
.assembly extern /*23000004*/ System.Security.AccessControl
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:1:0:0
}
.assembly /*20000001*/ mscorlib
{
  .custom /*0C000001:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000002:0A000002*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000003:0A000003*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [netstandard/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 07 01 00 00 00 00 ) 

  .custom /*0C000004:0A000004*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 08 6D 73 63 6F 72 6C 69 62 00 00 )          // ...mscorlib..
  .custom /*0C000005:0A000005*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 08 6D 73 63 6F 72 6C 69 62 00 00 )          // ...mscorlib..
  .custom /*0C000006:0A000006*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 08 6D 73 63 6F 72 6C 69 62 00 00 )          // ...mscorlib..
  .custom /*0C000007:0A000007*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000008:0A000008*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C000009:0A000009*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 2F C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ../.. Microsoft 
                                                                                                                                                                  43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 20 41 6C   // Corporation.  Al
                                                                                                                                                                  6C 20 72 69 67 68 74 73 20 72 65 73 65 72 76 65   // l rights reserve
                                                                                                                                                                  64 2E 00 00 )                                     // d...
  .custom /*0C00000A:0A00000A*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 0C 34 2E 36 2E 32 35 37 31 34 2E 30 31 00   // ...4.6.25714.01.
                                                                                                                                                                    00 ) 
  .custom /*0C00000B:0A00000B*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 61 34 2E 36 2E 32 35 37 31 34 2E 30 31 20   // ..a4.6.25714.01 
                                                                                                                                                                             62 75 69 6C 74 20 62 79 3A 20 64 6C 61 62 2D 44   // built by: dlab-D
                                                                                                                                                                             44 56 53 4F 57 49 4E 41 47 45 30 33 32 2E 20 43   // DVSOWINAGE032. C
                                                                                                                                                                             6F 6D 6D 69 74 20 48 61 73 68 3A 20 62 37 66 31   // ommit Hash: b7f1
                                                                                                                                                                             38 32 34 31 35 39 32 37 64 33 62 39 38 34 34 35   // 82415927d3b98445
                                                                                                                                                                             64 30 34 33 65 31 36 38 30 63 35 36 62 39 64 31   // d043e1680c56b9d1
                                                                                                                                                                             66 31 37 63 00 00 )                               // f17c..
  .custom /*0C00000C:0A00000C*/ instance void [netstandard/*23000001*/]System.CLSCompliantAttribute/*0100000D*/::.ctor(bool) /* 0A00000C */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A00000D*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000E*/::.ctor(string,
                                                                                                                                      string) /* 0A00000D */ = ( 01 00 00 00 00 00 ) 
  .publickey = (00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.class extern /*27000001*/ forwarder Microsoft.Win32.Registry
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000002*/ forwarder Microsoft.Win32.RegistryHive
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000003*/ forwarder Microsoft.Win32.RegistryKey
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000004*/ forwarder Microsoft.Win32.RegistryKeyPermissionCheck
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000005*/ forwarder Microsoft.Win32.RegistryOptions
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000006*/ forwarder Microsoft.Win32.RegistryValueKind
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000007*/ forwarder Microsoft.Win32.RegistryValueOptions
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000008*/ forwarder Microsoft.Win32.RegistryView
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000009*/ forwarder Microsoft.Win32.SafeHandles.CriticalHandleMinusOneIsInvalid
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700000A*/ forwarder Microsoft.Win32.SafeHandles.CriticalHandleZeroOrMinusOneIsInvalid
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700000B*/ forwarder Microsoft.Win32.SafeHandles.SafeAccessTokenHandle
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*2700000C*/ forwarder Microsoft.Win32.SafeHandles.SafeFileHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700000D*/ forwarder Microsoft.Win32.SafeHandles.SafeHandleMinusOneIsInvalid
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700000E*/ forwarder Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700000F*/ forwarder Microsoft.Win32.SafeHandles.SafeRegistryHandle
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000010*/ forwarder Microsoft.Win32.SafeHandles.SafeWaitHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000011*/ forwarder System.AccessViolationException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000012*/ forwarder System.Action
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000013*/ forwarder System.Action`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000014*/ forwarder System.Action`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000015*/ forwarder System.Action`3
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000016*/ forwarder System.Action`4
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000017*/ forwarder System.Action`5
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000018*/ forwarder System.Action`6
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000019*/ forwarder System.Action`7
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700001A*/ forwarder System.Action`8
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700001B*/ forwarder System.Activator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700001C*/ forwarder System.AggregateException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700001D*/ forwarder System.AppContext
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700001E*/ forwarder System.AppDomain
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700001F*/ forwarder System.AppDomainUnloadedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000020*/ forwarder System.ApplicationException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000021*/ forwarder System.ApplicationId
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000022*/ forwarder System.ArgumentException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000023*/ forwarder System.ArgumentNullException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000024*/ forwarder System.ArgumentOutOfRangeException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000025*/ forwarder System.ArithmeticException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000026*/ forwarder System.Array
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000027*/ forwarder System.ArraySegment`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000028*/ forwarder System.ArrayTypeMismatchException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000029*/ forwarder System.AssemblyLoadEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700002A*/ forwarder System.AssemblyLoadEventHandler
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700002B*/ forwarder System.AsyncCallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700002C*/ forwarder System.Attribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700002D*/ forwarder System.AttributeTargets
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700002E*/ forwarder System.AttributeUsageAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700002F*/ forwarder System.BadImageFormatException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000030*/ forwarder System.Base64FormattingOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000031*/ forwarder System.BitConverter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000032*/ forwarder System.Boolean
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000033*/ forwarder System.Buffer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000034*/ forwarder System.Byte
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000035*/ forwarder System.CannotUnloadAppDomainException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000036*/ forwarder System.Char
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000037*/ forwarder System.CharEnumerator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000038*/ forwarder System.CLSCompliantAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000039*/ forwarder System.Comparison`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700003A*/ forwarder System.Console
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700003B*/ forwarder System.ConsoleCancelEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700003C*/ forwarder System.ConsoleCancelEventHandler
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700003D*/ forwarder System.ConsoleColor
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700003E*/ forwarder System.ConsoleKey
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700003F*/ forwarder System.ConsoleKeyInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000040*/ forwarder System.ConsoleModifiers
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000041*/ forwarder System.ConsoleSpecialKey
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000042*/ forwarder System.ContextBoundObject
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000043*/ forwarder System.ContextMarshalException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000044*/ forwarder System.ContextStaticAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000045*/ forwarder System.Convert
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000046*/ forwarder System.Converter`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000047*/ forwarder System.DataMisalignedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000048*/ forwarder System.DateTime
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000049*/ forwarder System.DateTimeKind
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700004A*/ forwarder System.DateTimeOffset
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700004B*/ forwarder System.DayOfWeek
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700004C*/ forwarder System.DBNull
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700004D*/ forwarder System.Decimal
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700004E*/ forwarder System.Delegate
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700004F*/ forwarder System.DivideByZeroException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000050*/ forwarder System.DllNotFoundException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000051*/ forwarder System.Double
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000052*/ forwarder System.DuplicateWaitObjectException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000053*/ forwarder System.EntryPointNotFoundException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000054*/ forwarder System.Enum
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000055*/ forwarder System.Environment
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000056*/ SpecialFolder
{
  .class extern System.Environment /*27000055*/ 
}
.class extern /*27000057*/ SpecialFolderOption
{
  .class extern System.Environment /*27000055*/ 
}
.class extern /*27000058*/ forwarder System.EnvironmentVariableTarget
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000059*/ forwarder System.EventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700005A*/ forwarder System.EventHandler
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700005B*/ forwarder System.EventHandler`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700005C*/ forwarder System.Exception
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700005D*/ forwarder System.ExecutionEngineException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700005E*/ forwarder System.FieldAccessException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700005F*/ forwarder System.FlagsAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000060*/ forwarder System.FormatException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000061*/ forwarder System.FormattableString
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000062*/ forwarder System.Func`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000063*/ forwarder System.Func`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000064*/ forwarder System.Func`3
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000065*/ forwarder System.Func`4
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000066*/ forwarder System.Func`5
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000067*/ forwarder System.Func`6
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000068*/ forwarder System.Func`7
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000069*/ forwarder System.Func`8
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700006A*/ forwarder System.Func`9
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700006B*/ forwarder System.GC
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700006C*/ forwarder System.GCCollectionMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700006D*/ forwarder System.GCNotificationStatus
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700006E*/ forwarder System.Guid
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700006F*/ forwarder System.IAsyncResult
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000070*/ forwarder System.ICloneable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000071*/ forwarder System.IComparable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000072*/ forwarder System.IComparable`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000073*/ forwarder System.IConvertible
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000074*/ forwarder System.ICustomFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000075*/ forwarder System.IDisposable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000076*/ forwarder System.IEquatable`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000077*/ forwarder System.IFormatProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000078*/ forwarder System.IFormattable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000079*/ forwarder System.IndexOutOfRangeException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700007A*/ forwarder System.InsufficientExecutionStackException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700007B*/ forwarder System.InsufficientMemoryException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700007C*/ forwarder System.Int16
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700007D*/ forwarder System.Int32
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700007E*/ forwarder System.Int64
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700007F*/ forwarder System.IntPtr
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000080*/ forwarder System.InvalidCastException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000081*/ forwarder System.InvalidOperationException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000082*/ forwarder System.InvalidProgramException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000083*/ forwarder System.InvalidTimeZoneException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000084*/ forwarder System.IObservable`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000085*/ forwarder System.IObserver`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000086*/ forwarder System.IProgress`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000087*/ forwarder System.IServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000088*/ forwarder System.Lazy`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000089*/ forwarder System.LoaderOptimization
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700008A*/ forwarder System.LoaderOptimizationAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700008B*/ forwarder System.LocalDataStoreSlot
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700008C*/ forwarder System.MarshalByRefObject
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700008D*/ forwarder System.Math
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700008E*/ forwarder System.MemberAccessException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700008F*/ forwarder System.MethodAccessException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000090*/ forwarder System.MidpointRounding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000091*/ forwarder System.MissingFieldException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000092*/ forwarder System.MissingMemberException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000093*/ forwarder System.MissingMethodException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000094*/ forwarder System.ModuleHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000095*/ forwarder System.MTAThreadAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000096*/ forwarder System.MulticastDelegate
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000097*/ forwarder System.MulticastNotSupportedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000098*/ forwarder System.NonSerializedAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000099*/ forwarder System.NotFiniteNumberException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700009A*/ forwarder System.NotImplementedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700009B*/ forwarder System.NotSupportedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700009C*/ forwarder System.Nullable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700009D*/ forwarder System.Nullable`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700009E*/ forwarder System.NullReferenceException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700009F*/ forwarder System.Object
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A0*/ forwarder System.ObjectDisposedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A1*/ forwarder System.ObsoleteAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A2*/ forwarder System.OperatingSystem
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A3*/ forwarder System.OperationCanceledException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A4*/ forwarder System.OutOfMemoryException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A5*/ forwarder System.OverflowException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A6*/ forwarder System.ParamArrayAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A7*/ forwarder System.PlatformID
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A8*/ forwarder System.PlatformNotSupportedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000A9*/ forwarder System.Predicate`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000AA*/ forwarder System.Progress`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000AB*/ forwarder System.Random
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000AC*/ forwarder System.RankException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000AD*/ forwarder System.ResolveEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000AE*/ forwarder System.ResolveEventHandler
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000AF*/ forwarder System.RuntimeArgumentHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B0*/ forwarder System.RuntimeFieldHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B1*/ forwarder System.RuntimeMethodHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B2*/ forwarder System.RuntimeTypeHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B3*/ forwarder System.SByte
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B4*/ forwarder System.SerializableAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B5*/ forwarder System.Single
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B6*/ forwarder System.StackOverflowException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B7*/ forwarder System.STAThreadAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B8*/ forwarder System.String
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000B9*/ forwarder System.StringComparer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000BA*/ forwarder System.StringComparison
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000BB*/ forwarder System.StringSplitOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000BC*/ forwarder System.SystemException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000BD*/ forwarder System.ThreadStaticAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000BE*/ forwarder System.TimeoutException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000BF*/ forwarder System.TimeSpan
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000C0*/ forwarder System.TimeZone
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000C1*/ forwarder System.TimeZoneInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000C2*/ AdjustmentRule
{
  .class extern System.TimeZoneInfo /*270000C1*/ 
}
.class extern /*270000C3*/ TransitionTime
{
  .class extern System.TimeZoneInfo /*270000C1*/ 
}
.class extern /*270000C4*/ forwarder System.TimeZoneNotFoundException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000C5*/ forwarder System.Tuple
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000C6*/ forwarder System.TupleExtensions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000C7*/ forwarder System.Tuple`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000C8*/ forwarder System.Tuple`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000C9*/ forwarder System.Tuple`3
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000CA*/ forwarder System.Tuple`4
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000CB*/ forwarder System.Tuple`5
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000CC*/ forwarder System.Tuple`6
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000CD*/ forwarder System.Tuple`7
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000CE*/ forwarder System.Tuple`8
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000CF*/ forwarder System.Type
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D0*/ forwarder System.TypeAccessException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D1*/ forwarder System.TypeCode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D2*/ forwarder System.TypedReference
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D3*/ forwarder System.TypeInitializationException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D4*/ forwarder System.TypeLoadException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D5*/ forwarder System.TypeUnloadedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D6*/ forwarder System.UInt16
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D7*/ forwarder System.UInt32
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D8*/ forwarder System.UInt64
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000D9*/ forwarder System.UIntPtr
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000DA*/ forwarder System.UnauthorizedAccessException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000DB*/ forwarder System.UnhandledExceptionEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000DC*/ forwarder System.UnhandledExceptionEventHandler
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000DD*/ forwarder System.ValueTuple
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000DE*/ forwarder System.ValueTuple`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000DF*/ forwarder System.ValueTuple`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E0*/ forwarder System.ValueTuple`3
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E1*/ forwarder System.ValueTuple`4
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E2*/ forwarder System.ValueTuple`5
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E3*/ forwarder System.ValueTuple`6
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E4*/ forwarder System.ValueTuple`7
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E5*/ forwarder System.ValueTuple`8
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E6*/ forwarder System.ValueType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E7*/ forwarder System.Version
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E8*/ forwarder System.Void
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000E9*/ forwarder System.WeakReference
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000EA*/ forwarder System.WeakReference`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000EB*/ forwarder System.Collections.ArrayList
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000EC*/ forwarder System.Collections.BitArray
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000ED*/ forwarder System.Collections.CaseInsensitiveComparer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000EE*/ forwarder System.Collections.CaseInsensitiveHashCodeProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000EF*/ forwarder System.Collections.CollectionBase
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F0*/ forwarder System.Collections.Comparer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F1*/ forwarder System.Collections.DictionaryBase
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F2*/ forwarder System.Collections.DictionaryEntry
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F3*/ forwarder System.Collections.Hashtable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F4*/ forwarder System.Collections.ICollection
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F5*/ forwarder System.Collections.IComparer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F6*/ forwarder System.Collections.IDictionary
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F7*/ forwarder System.Collections.IDictionaryEnumerator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F8*/ forwarder System.Collections.IEnumerable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000F9*/ forwarder System.Collections.IEnumerator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000FA*/ forwarder System.Collections.IEqualityComparer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000FB*/ forwarder System.Collections.IHashCodeProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000FC*/ forwarder System.Collections.IList
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000FD*/ forwarder System.Collections.IStructuralComparable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000FE*/ forwarder System.Collections.IStructuralEquatable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270000FF*/ forwarder System.Collections.Queue
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000100*/ forwarder System.Collections.ReadOnlyCollectionBase
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000101*/ forwarder System.Collections.SortedList
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000102*/ forwarder System.Collections.Stack
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000103*/ forwarder System.Collections.StructuralComparisons
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000104*/ forwarder System.Collections.Concurrent.ConcurrentDictionary`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000105*/ forwarder System.Collections.Concurrent.ConcurrentQueue`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000106*/ forwarder System.Collections.Concurrent.ConcurrentStack`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000107*/ forwarder System.Collections.Concurrent.EnumerablePartitionerOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000108*/ forwarder System.Collections.Concurrent.IProducerConsumerCollection`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000109*/ forwarder System.Collections.Concurrent.OrderablePartitioner`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700010A*/ forwarder System.Collections.Concurrent.Partitioner
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700010B*/ forwarder System.Collections.Concurrent.Partitioner`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700010C*/ forwarder System.Collections.Generic.Comparer`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700010D*/ forwarder System.Collections.Generic.Dictionary`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700010E*/ Enumerator
{
  .class extern System.Collections.Generic.Dictionary`2 /*2700010D*/ 
}
.class extern /*2700010F*/ KeyCollection
{
  .class extern System.Collections.Generic.Dictionary`2 /*2700010D*/ 
}
.class extern /*27000110*/ Enumerator
{
  .class extern System.Collections.Generic.Dictionary`2/KeyCollection /*2700010F*/ 
}
.class extern /*27000111*/ ValueCollection
{
  .class extern System.Collections.Generic.Dictionary`2 /*2700010D*/ 
}
.class extern /*27000112*/ Enumerator
{
  .class extern System.Collections.Generic.Dictionary`2/ValueCollection /*27000111*/ 
}
.class extern /*27000113*/ forwarder System.Collections.Generic.EqualityComparer`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000114*/ forwarder System.Collections.Generic.ICollection`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000115*/ forwarder System.Collections.Generic.IComparer`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000116*/ forwarder System.Collections.Generic.IDictionary`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000117*/ forwarder System.Collections.Generic.IEnumerable`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000118*/ forwarder System.Collections.Generic.IEnumerator`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000119*/ forwarder System.Collections.Generic.IEqualityComparer`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700011A*/ forwarder System.Collections.Generic.IList`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700011B*/ forwarder System.Collections.Generic.IReadOnlyCollection`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700011C*/ forwarder System.Collections.Generic.IReadOnlyDictionary`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700011D*/ forwarder System.Collections.Generic.IReadOnlyList`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700011E*/ forwarder System.Collections.Generic.KeyNotFoundException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700011F*/ forwarder System.Collections.Generic.KeyValuePair`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000120*/ forwarder System.Collections.Generic.List`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000121*/ Enumerator
{
  .class extern System.Collections.Generic.List`1 /*27000120*/ 
}
.class extern /*27000122*/ forwarder System.Collections.ObjectModel.Collection`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000123*/ forwarder System.Collections.ObjectModel.KeyedCollection`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000124*/ forwarder System.Collections.ObjectModel.ReadOnlyCollection`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000125*/ forwarder System.Collections.ObjectModel.ReadOnlyDictionary`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000126*/ KeyCollection
{
  .class extern System.Collections.ObjectModel.ReadOnlyDictionary`2 /*27000125*/ 
}
.class extern /*27000127*/ ValueCollection
{
  .class extern System.Collections.ObjectModel.ReadOnlyDictionary`2 /*27000125*/ 
}
.class extern /*27000128*/ forwarder System.Configuration.Assemblies.AssemblyHashAlgorithm
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000129*/ forwarder System.Configuration.Assemblies.AssemblyVersionCompatibility
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700012A*/ forwarder System.Diagnostics.ConditionalAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700012B*/ forwarder System.Diagnostics.DebuggableAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700012C*/ DebuggingModes
{
  .class extern System.Diagnostics.DebuggableAttribute /*2700012B*/ 
}
.class extern /*2700012D*/ forwarder System.Diagnostics.Debugger
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700012E*/ forwarder System.Diagnostics.DebuggerBrowsableAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700012F*/ forwarder System.Diagnostics.DebuggerBrowsableState
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000130*/ forwarder System.Diagnostics.DebuggerDisplayAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000131*/ forwarder System.Diagnostics.DebuggerHiddenAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000132*/ forwarder System.Diagnostics.DebuggerNonUserCodeAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000133*/ forwarder System.Diagnostics.DebuggerStepperBoundaryAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000134*/ forwarder System.Diagnostics.DebuggerStepThroughAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000135*/ forwarder System.Diagnostics.DebuggerTypeProxyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000136*/ forwarder System.Diagnostics.DebuggerVisualizerAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000137*/ forwarder System.Diagnostics.StackFrame
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000138*/ forwarder System.Diagnostics.StackTrace
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000139*/ forwarder System.Diagnostics.CodeAnalysis.SuppressMessageAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700013A*/ forwarder System.Diagnostics.Contracts.Contract
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700013B*/ forwarder System.Diagnostics.Contracts.ContractAbbreviatorAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700013C*/ forwarder System.Diagnostics.Contracts.ContractArgumentValidatorAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700013D*/ forwarder System.Diagnostics.Contracts.ContractClassAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700013E*/ forwarder System.Diagnostics.Contracts.ContractClassForAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700013F*/ forwarder System.Diagnostics.Contracts.ContractFailedEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000140*/ forwarder System.Diagnostics.Contracts.ContractFailureKind
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000141*/ forwarder System.Diagnostics.Contracts.ContractInvariantMethodAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000142*/ forwarder System.Diagnostics.Contracts.ContractOptionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000143*/ forwarder System.Diagnostics.Contracts.ContractPublicPropertyNameAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000144*/ forwarder System.Diagnostics.Contracts.ContractReferenceAssemblyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000145*/ forwarder System.Diagnostics.Contracts.ContractRuntimeIgnoredAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000146*/ forwarder System.Diagnostics.Contracts.ContractVerificationAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000147*/ forwarder System.Diagnostics.Contracts.PureAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000148*/ forwarder System.Diagnostics.SymbolStore.ISymbolBinder
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000149*/ forwarder System.Diagnostics.SymbolStore.ISymbolBinder1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700014A*/ forwarder System.Diagnostics.SymbolStore.ISymbolDocument
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700014B*/ forwarder System.Diagnostics.SymbolStore.ISymbolDocumentWriter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700014C*/ forwarder System.Diagnostics.SymbolStore.ISymbolMethod
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700014D*/ forwarder System.Diagnostics.SymbolStore.ISymbolNamespace
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700014E*/ forwarder System.Diagnostics.SymbolStore.ISymbolReader
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700014F*/ forwarder System.Diagnostics.SymbolStore.ISymbolScope
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000150*/ forwarder System.Diagnostics.SymbolStore.ISymbolVariable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000151*/ forwarder System.Diagnostics.SymbolStore.ISymbolWriter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000152*/ forwarder System.Diagnostics.SymbolStore.SymAddressKind
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000153*/ forwarder System.Diagnostics.SymbolStore.SymbolToken
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000154*/ forwarder System.Diagnostics.SymbolStore.SymDocumentType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000155*/ forwarder System.Diagnostics.SymbolStore.SymLanguageType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000156*/ forwarder System.Diagnostics.SymbolStore.SymLanguageVendor
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000157*/ forwarder System.Diagnostics.Tracing.EventActivityOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000158*/ forwarder System.Diagnostics.Tracing.EventAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000159*/ forwarder System.Diagnostics.Tracing.EventChannel
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700015A*/ forwarder System.Diagnostics.Tracing.EventCommand
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700015B*/ forwarder System.Diagnostics.Tracing.EventCommandEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700015C*/ forwarder System.Diagnostics.Tracing.EventDataAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700015D*/ forwarder System.Diagnostics.Tracing.EventFieldAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700015E*/ forwarder System.Diagnostics.Tracing.EventFieldFormat
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700015F*/ forwarder System.Diagnostics.Tracing.EventFieldTags
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000160*/ forwarder System.Diagnostics.Tracing.EventIgnoreAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000161*/ forwarder System.Diagnostics.Tracing.EventKeywords
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000162*/ forwarder System.Diagnostics.Tracing.EventLevel
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000163*/ forwarder System.Diagnostics.Tracing.EventListener
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000164*/ forwarder System.Diagnostics.Tracing.EventManifestOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000165*/ forwarder System.Diagnostics.Tracing.EventOpcode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000166*/ forwarder System.Diagnostics.Tracing.EventSource
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000167*/ EventData
{
  .class extern System.Diagnostics.Tracing.EventSource /*27000166*/ 
}
.class extern /*27000168*/ forwarder System.Diagnostics.Tracing.EventSourceAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000169*/ forwarder System.Diagnostics.Tracing.EventSourceException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700016A*/ forwarder System.Diagnostics.Tracing.EventSourceOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700016B*/ forwarder System.Diagnostics.Tracing.EventSourceSettings
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700016C*/ forwarder System.Diagnostics.Tracing.EventTags
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700016D*/ forwarder System.Diagnostics.Tracing.EventTask
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700016E*/ forwarder System.Diagnostics.Tracing.EventWrittenEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700016F*/ forwarder System.Diagnostics.Tracing.NonEventAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000170*/ forwarder System.Globalization.Calendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000171*/ forwarder System.Globalization.CalendarAlgorithmType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000172*/ forwarder System.Globalization.CalendarWeekRule
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000173*/ forwarder System.Globalization.CharUnicodeInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000174*/ forwarder System.Globalization.ChineseLunisolarCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000175*/ forwarder System.Globalization.CompareInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000176*/ forwarder System.Globalization.CompareOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000177*/ forwarder System.Globalization.CultureInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000178*/ forwarder System.Globalization.CultureNotFoundException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000179*/ forwarder System.Globalization.CultureTypes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700017A*/ forwarder System.Globalization.DateTimeFormatInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700017B*/ forwarder System.Globalization.DateTimeStyles
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700017C*/ forwarder System.Globalization.DaylightTime
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700017D*/ forwarder System.Globalization.DigitShapes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700017E*/ forwarder System.Globalization.EastAsianLunisolarCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700017F*/ forwarder System.Globalization.GregorianCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000180*/ forwarder System.Globalization.GregorianCalendarTypes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000181*/ forwarder System.Globalization.HebrewCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000182*/ forwarder System.Globalization.HijriCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000183*/ forwarder System.Globalization.IdnMapping
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000184*/ forwarder System.Globalization.JapaneseCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000185*/ forwarder System.Globalization.JapaneseLunisolarCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000186*/ forwarder System.Globalization.JulianCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000187*/ forwarder System.Globalization.KoreanCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000188*/ forwarder System.Globalization.KoreanLunisolarCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000189*/ forwarder System.Globalization.NumberFormatInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700018A*/ forwarder System.Globalization.NumberStyles
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700018B*/ forwarder System.Globalization.PersianCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700018C*/ forwarder System.Globalization.RegionInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700018D*/ forwarder System.Globalization.SortKey
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700018E*/ forwarder System.Globalization.SortVersion
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700018F*/ forwarder System.Globalization.StringInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000190*/ forwarder System.Globalization.TaiwanCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000191*/ forwarder System.Globalization.TaiwanLunisolarCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000192*/ forwarder System.Globalization.TextElementEnumerator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000193*/ forwarder System.Globalization.TextInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000194*/ forwarder System.Globalization.ThaiBuddhistCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000195*/ forwarder System.Globalization.TimeSpanStyles
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000196*/ forwarder System.Globalization.UmAlQuraCalendar
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000197*/ forwarder System.Globalization.UnicodeCategory
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000198*/ forwarder System.IO.BinaryReader
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000199*/ forwarder System.IO.BinaryWriter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700019A*/ forwarder System.IO.BufferedStream
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700019B*/ forwarder System.IO.Directory
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700019C*/ forwarder System.IO.DirectoryInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700019D*/ forwarder System.IO.DirectoryNotFoundException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700019E*/ forwarder System.IO.DriveInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700019F*/ forwarder System.IO.DriveNotFoundException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A0*/ forwarder System.IO.DriveType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A1*/ forwarder System.IO.EndOfStreamException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A2*/ forwarder System.IO.File
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A3*/ forwarder System.IO.FileAccess
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A4*/ forwarder System.IO.FileAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A5*/ forwarder System.IO.FileInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A6*/ forwarder System.IO.FileLoadException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A7*/ forwarder System.IO.FileMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A8*/ forwarder System.IO.FileNotFoundException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001A9*/ forwarder System.IO.FileOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001AA*/ forwarder System.IO.FileShare
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001AB*/ forwarder System.IO.FileStream
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001AC*/ forwarder System.IO.FileSystemInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001AD*/ forwarder System.IO.IOException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001AE*/ forwarder System.IO.MemoryStream
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001AF*/ forwarder System.IO.Path
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B0*/ forwarder System.IO.PathTooLongException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B1*/ forwarder System.IO.SearchOption
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B2*/ forwarder System.IO.SeekOrigin
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B3*/ forwarder System.IO.Stream
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B4*/ forwarder System.IO.StreamReader
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B5*/ forwarder System.IO.StreamWriter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B6*/ forwarder System.IO.StringReader
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B7*/ forwarder System.IO.StringWriter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B8*/ forwarder System.IO.TextReader
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001B9*/ forwarder System.IO.TextWriter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001BA*/ forwarder System.IO.UnmanagedMemoryAccessor
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001BB*/ forwarder System.IO.UnmanagedMemoryStream
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001BC*/ forwarder System.IO.IsolatedStorage.INormalizeForIsolatedStorage
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001BD*/ forwarder System.IO.IsolatedStorage.IsolatedStorage
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001BE*/ forwarder System.IO.IsolatedStorage.IsolatedStorageException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001BF*/ forwarder System.IO.IsolatedStorage.IsolatedStorageFile
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C0*/ forwarder System.IO.IsolatedStorage.IsolatedStorageFileStream
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C1*/ forwarder System.IO.IsolatedStorage.IsolatedStorageScope
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C2*/ forwarder System.Reflection.AmbiguousMatchException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C3*/ forwarder System.Reflection.Assembly
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C4*/ forwarder System.Reflection.AssemblyAlgorithmIdAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C5*/ forwarder System.Reflection.AssemblyCompanyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C6*/ forwarder System.Reflection.AssemblyConfigurationAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C7*/ forwarder System.Reflection.AssemblyContentType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C8*/ forwarder System.Reflection.AssemblyCopyrightAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001C9*/ forwarder System.Reflection.AssemblyCultureAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001CA*/ forwarder System.Reflection.AssemblyDefaultAliasAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001CB*/ forwarder System.Reflection.AssemblyDelaySignAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001CC*/ forwarder System.Reflection.AssemblyDescriptionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001CD*/ forwarder System.Reflection.AssemblyFileVersionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001CE*/ forwarder System.Reflection.AssemblyFlagsAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001CF*/ forwarder System.Reflection.AssemblyInformationalVersionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D0*/ forwarder System.Reflection.AssemblyKeyFileAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D1*/ forwarder System.Reflection.AssemblyKeyNameAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D2*/ forwarder System.Reflection.AssemblyMetadataAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D3*/ forwarder System.Reflection.AssemblyName
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D4*/ forwarder System.Reflection.AssemblyNameFlags
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D5*/ forwarder System.Reflection.AssemblyNameProxy
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D6*/ forwarder System.Reflection.AssemblyProductAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D7*/ forwarder System.Reflection.AssemblySignatureKeyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D8*/ forwarder System.Reflection.AssemblyTitleAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001D9*/ forwarder System.Reflection.AssemblyTrademarkAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001DA*/ forwarder System.Reflection.AssemblyVersionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001DB*/ forwarder System.Reflection.Binder
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001DC*/ forwarder System.Reflection.BindingFlags
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001DD*/ forwarder System.Reflection.CallingConventions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001DE*/ forwarder System.Reflection.ConstructorInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001DF*/ forwarder System.Reflection.CustomAttributeData
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E0*/ forwarder System.Reflection.CustomAttributeExtensions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E1*/ forwarder System.Reflection.CustomAttributeFormatException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E2*/ forwarder System.Reflection.CustomAttributeNamedArgument
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E3*/ forwarder System.Reflection.CustomAttributeTypedArgument
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E4*/ forwarder System.Reflection.DefaultMemberAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E5*/ forwarder System.Reflection.EventAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E6*/ forwarder System.Reflection.EventInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E7*/ forwarder System.Reflection.ExceptionHandlingClause
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E8*/ forwarder System.Reflection.ExceptionHandlingClauseOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001E9*/ forwarder System.Reflection.FieldAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001EA*/ forwarder System.Reflection.FieldInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001EB*/ forwarder System.Reflection.GenericParameterAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001EC*/ forwarder System.Reflection.ICustomAttributeProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001ED*/ forwarder System.Reflection.ImageFileMachine
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001EE*/ forwarder System.Reflection.InterfaceMapping
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001EF*/ forwarder System.Reflection.IntrospectionExtensions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F0*/ forwarder System.Reflection.InvalidFilterCriteriaException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F1*/ forwarder System.Reflection.IReflect
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F2*/ forwarder System.Reflection.IReflectableType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F3*/ forwarder System.Reflection.LocalVariableInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F4*/ forwarder System.Reflection.ManifestResourceInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F5*/ forwarder System.Reflection.MemberFilter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F6*/ forwarder System.Reflection.MemberInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F7*/ forwarder System.Reflection.MemberTypes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F8*/ forwarder System.Reflection.MethodAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001F9*/ forwarder System.Reflection.MethodBase
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001FA*/ forwarder System.Reflection.MethodBody
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001FB*/ forwarder System.Reflection.MethodImplAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001FC*/ forwarder System.Reflection.MethodInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001FD*/ forwarder System.Reflection.Missing
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001FE*/ forwarder System.Reflection.Module
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270001FF*/ forwarder System.Reflection.ModuleResolveEventHandler
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000200*/ forwarder System.Reflection.ObfuscateAssemblyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000201*/ forwarder System.Reflection.ObfuscationAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000202*/ forwarder System.Reflection.ParameterAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000203*/ forwarder System.Reflection.ParameterInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000204*/ forwarder System.Reflection.ParameterModifier
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000205*/ forwarder System.Reflection.Pointer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000206*/ forwarder System.Reflection.PortableExecutableKinds
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000207*/ forwarder System.Reflection.ProcessorArchitecture
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000208*/ forwarder System.Reflection.PropertyAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000209*/ forwarder System.Reflection.PropertyInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700020A*/ forwarder System.Reflection.ReflectionContext
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700020B*/ forwarder System.Reflection.ReflectionTypeLoadException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700020C*/ forwarder System.Reflection.ResourceAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700020D*/ forwarder System.Reflection.ResourceLocation
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700020E*/ forwarder System.Reflection.RuntimeReflectionExtensions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700020F*/ forwarder System.Reflection.StrongNameKeyPair
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000210*/ forwarder System.Reflection.TargetException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000211*/ forwarder System.Reflection.TargetInvocationException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000212*/ forwarder System.Reflection.TargetParameterCountException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000213*/ forwarder System.Reflection.TypeAttributes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000214*/ forwarder System.Reflection.TypeDelegator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000215*/ forwarder System.Reflection.TypeFilter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000216*/ forwarder System.Reflection.TypeInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000217*/ forwarder System.Reflection.Emit.FlowControl
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000218*/ forwarder System.Reflection.Emit.OpCode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000219*/ forwarder System.Reflection.Emit.OpCodes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700021A*/ forwarder System.Reflection.Emit.OpCodeType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700021B*/ forwarder System.Reflection.Emit.OperandType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700021C*/ forwarder System.Reflection.Emit.PackingSize
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700021D*/ forwarder System.Reflection.Emit.StackBehaviour
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700021E*/ forwarder System.Resources.IResourceReader
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700021F*/ forwarder System.Resources.IResourceWriter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000220*/ forwarder System.Resources.MissingManifestResourceException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000221*/ forwarder System.Resources.MissingSatelliteAssemblyException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000222*/ forwarder System.Resources.NeutralResourcesLanguageAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000223*/ forwarder System.Resources.ResourceManager
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000224*/ forwarder System.Resources.ResourceReader
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000225*/ forwarder System.Resources.ResourceSet
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000226*/ forwarder System.Resources.ResourceWriter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000227*/ forwarder System.Resources.SatelliteContractVersionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000228*/ forwarder System.Resources.UltimateResourceFallbackLocation
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000229*/ forwarder System.Runtime.AssemblyTargetedPatchBandAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700022A*/ forwarder System.Runtime.GCLargeObjectHeapCompactionMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700022B*/ forwarder System.Runtime.GCLatencyMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700022C*/ forwarder System.Runtime.GCSettings
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700022D*/ forwarder System.Runtime.MemoryFailPoint
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700022E*/ forwarder System.Runtime.TargetedPatchingOptOutAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700022F*/ forwarder System.Runtime.CompilerServices.AccessedThroughPropertyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000230*/ forwarder System.Runtime.CompilerServices.AsyncStateMachineAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000231*/ forwarder System.Runtime.CompilerServices.AsyncTaskMethodBuilder
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000232*/ forwarder System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000233*/ forwarder System.Runtime.CompilerServices.AsyncVoidMethodBuilder
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000234*/ forwarder System.Runtime.CompilerServices.CallConvCdecl
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000235*/ forwarder System.Runtime.CompilerServices.CallConvFastcall
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000236*/ forwarder System.Runtime.CompilerServices.CallConvStdcall
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000237*/ forwarder System.Runtime.CompilerServices.CallConvThiscall
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000238*/ forwarder System.Runtime.CompilerServices.CallerFilePathAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000239*/ forwarder System.Runtime.CompilerServices.CallerLineNumberAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700023A*/ forwarder System.Runtime.CompilerServices.CallerMemberNameAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700023B*/ forwarder System.Runtime.CompilerServices.CompilationRelaxations
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700023C*/ forwarder System.Runtime.CompilerServices.CompilationRelaxationsAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700023D*/ forwarder System.Runtime.CompilerServices.CompilerGeneratedAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700023E*/ forwarder System.Runtime.CompilerServices.CompilerGlobalScopeAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700023F*/ forwarder System.Runtime.CompilerServices.CompilerMarshalOverride
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000240*/ forwarder System.Runtime.CompilerServices.ConditionalWeakTable`2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000241*/ CreateValueCallback
{
  .class extern System.Runtime.CompilerServices.ConditionalWeakTable`2 /*27000240*/ 
}
.class extern /*27000242*/ forwarder System.Runtime.CompilerServices.ConfiguredTaskAwaitable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000243*/ ConfiguredTaskAwaiter
{
  .class extern System.Runtime.CompilerServices.ConfiguredTaskAwaitable /*27000242*/ 
}
.class extern /*27000244*/ forwarder System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000245*/ ConfiguredTaskAwaiter
{
  .class extern System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1 /*27000244*/ 
}
.class extern /*27000246*/ forwarder System.Runtime.CompilerServices.ContractHelper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000247*/ forwarder System.Runtime.CompilerServices.CustomConstantAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000248*/ forwarder System.Runtime.CompilerServices.DateTimeConstantAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000249*/ forwarder System.Runtime.CompilerServices.DecimalConstantAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700024A*/ forwarder System.Runtime.CompilerServices.DefaultDependencyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700024B*/ forwarder System.Runtime.CompilerServices.DependencyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700024C*/ forwarder System.Runtime.CompilerServices.DisablePrivateReflectionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700024D*/ forwarder System.Runtime.CompilerServices.DiscardableAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700024E*/ forwarder System.Runtime.CompilerServices.ExtensionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700024F*/ forwarder System.Runtime.CompilerServices.FixedAddressValueTypeAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000250*/ forwarder System.Runtime.CompilerServices.FixedBufferAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000251*/ forwarder System.Runtime.CompilerServices.FormattableStringFactory
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000252*/ forwarder System.Runtime.CompilerServices.HasCopySemanticsAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000253*/ forwarder System.Runtime.CompilerServices.IAsyncStateMachine
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000254*/ forwarder System.Runtime.CompilerServices.ICriticalNotifyCompletion
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000255*/ forwarder System.Runtime.CompilerServices.IndexerNameAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000256*/ forwarder System.Runtime.CompilerServices.INotifyCompletion
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000257*/ forwarder System.Runtime.CompilerServices.InternalsVisibleToAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000258*/ forwarder System.Runtime.CompilerServices.IsBoxed
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000259*/ forwarder System.Runtime.CompilerServices.IsByValue
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700025A*/ forwarder System.Runtime.CompilerServices.IsConst
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700025B*/ forwarder System.Runtime.CompilerServices.IsCopyConstructed
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700025C*/ forwarder System.Runtime.CompilerServices.IsExplicitlyDereferenced
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700025D*/ forwarder System.Runtime.CompilerServices.IsImplicitlyDereferenced
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700025E*/ forwarder System.Runtime.CompilerServices.IsJitIntrinsic
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700025F*/ forwarder System.Runtime.CompilerServices.IsLong
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000260*/ forwarder System.Runtime.CompilerServices.IsPinned
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000261*/ forwarder System.Runtime.CompilerServices.IsSignUnspecifiedByte
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000262*/ forwarder System.Runtime.CompilerServices.IsUdtReturn
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000263*/ forwarder System.Runtime.CompilerServices.IsVolatile
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000264*/ forwarder System.Runtime.CompilerServices.IteratorStateMachineAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000265*/ forwarder System.Runtime.CompilerServices.IUnknownConstantAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000266*/ forwarder System.Runtime.CompilerServices.LoadHint
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000267*/ forwarder System.Runtime.CompilerServices.MethodCodeType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000268*/ forwarder System.Runtime.CompilerServices.MethodImplAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000269*/ forwarder System.Runtime.CompilerServices.MethodImplOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700026A*/ forwarder System.Runtime.CompilerServices.NativeCppClassAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700026B*/ forwarder System.Runtime.CompilerServices.ReferenceAssemblyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700026C*/ forwarder System.Runtime.CompilerServices.RequiredAttributeAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700026D*/ forwarder System.Runtime.CompilerServices.RuntimeCompatibilityAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700026E*/ forwarder System.Runtime.CompilerServices.RuntimeHelpers
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700026F*/ CleanupCode
{
  .class extern System.Runtime.CompilerServices.RuntimeHelpers /*2700026E*/ 
}
.class extern /*27000270*/ TryCode
{
  .class extern System.Runtime.CompilerServices.RuntimeHelpers /*2700026E*/ 
}
.class extern /*27000271*/ forwarder System.Runtime.CompilerServices.RuntimeWrappedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000272*/ forwarder System.Runtime.CompilerServices.ScopelessEnumAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000273*/ forwarder System.Runtime.CompilerServices.SpecialNameAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000274*/ forwarder System.Runtime.CompilerServices.StateMachineAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000275*/ forwarder System.Runtime.CompilerServices.StringFreezingAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000276*/ forwarder System.Runtime.CompilerServices.SuppressIldasmAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000277*/ forwarder System.Runtime.CompilerServices.TaskAwaiter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000278*/ forwarder System.Runtime.CompilerServices.TaskAwaiter`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000279*/ forwarder System.Runtime.CompilerServices.TupleElementNamesAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700027A*/ forwarder System.Runtime.CompilerServices.TypeForwardedFromAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700027B*/ forwarder System.Runtime.CompilerServices.TypeForwardedToAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700027C*/ forwarder System.Runtime.CompilerServices.UnsafeValueTypeAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700027D*/ forwarder System.Runtime.CompilerServices.YieldAwaitable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700027E*/ YieldAwaiter
{
  .class extern System.Runtime.CompilerServices.YieldAwaitable /*2700027D*/ 
}
.class extern /*2700027F*/ forwarder System.Runtime.ConstrainedExecution.Cer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000280*/ forwarder System.Runtime.ConstrainedExecution.Consistency
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000281*/ forwarder System.Runtime.ConstrainedExecution.CriticalFinalizerObject
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000282*/ forwarder System.Runtime.ConstrainedExecution.PrePrepareMethodAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000283*/ forwarder System.Runtime.ConstrainedExecution.ReliabilityContractAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000284*/ forwarder System.Runtime.ExceptionServices.ExceptionDispatchInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000285*/ forwarder System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000286*/ forwarder System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000287*/ forwarder System.Runtime.InteropServices.AllowReversePInvokeCallsAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000288*/ forwarder System.Runtime.InteropServices.ArrayWithOffset
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000289*/ forwarder System.Runtime.InteropServices.BestFitMappingAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700028A*/ forwarder System.Runtime.InteropServices.BStrWrapper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700028B*/ forwarder System.Runtime.InteropServices.CallingConvention
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700028C*/ forwarder System.Runtime.InteropServices.CharSet
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700028D*/ forwarder System.Runtime.InteropServices.ClassInterfaceAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700028E*/ forwarder System.Runtime.InteropServices.ClassInterfaceType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700028F*/ forwarder System.Runtime.InteropServices.CoClassAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000290*/ forwarder System.Runtime.InteropServices.ComAliasNameAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000291*/ forwarder System.Runtime.InteropServices.ComCompatibleVersionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000292*/ forwarder System.Runtime.InteropServices.ComConversionLossAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000293*/ forwarder System.Runtime.InteropServices.ComDefaultInterfaceAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000294*/ forwarder System.Runtime.InteropServices.ComEventInterfaceAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000295*/ forwarder System.Runtime.InteropServices.ComEventsHelper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000296*/ forwarder System.Runtime.InteropServices.COMException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000297*/ forwarder System.Runtime.InteropServices.ComImportAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000298*/ forwarder System.Runtime.InteropServices.ComInterfaceType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000299*/ forwarder System.Runtime.InteropServices.ComMemberType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700029A*/ forwarder System.Runtime.InteropServices.ComRegisterFunctionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700029B*/ forwarder System.Runtime.InteropServices.ComSourceInterfacesAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700029C*/ forwarder System.Runtime.InteropServices.ComUnregisterFunctionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700029D*/ forwarder System.Runtime.InteropServices.ComVisibleAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700029E*/ forwarder System.Runtime.InteropServices.CriticalHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700029F*/ forwarder System.Runtime.InteropServices.CurrencyWrapper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A0*/ forwarder System.Runtime.InteropServices.CustomQueryInterfaceMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A1*/ forwarder System.Runtime.InteropServices.CustomQueryInterfaceResult
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A2*/ forwarder System.Runtime.InteropServices.DefaultCharSetAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A3*/ forwarder System.Runtime.InteropServices.DefaultDllImportSearchPathsAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A4*/ forwarder System.Runtime.InteropServices.DispatchWrapper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A5*/ forwarder System.Runtime.InteropServices.DispIdAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A6*/ forwarder System.Runtime.InteropServices.DllImportAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A7*/ forwarder System.Runtime.InteropServices.DllImportSearchPath
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A8*/ forwarder System.Runtime.InteropServices.ErrorWrapper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002A9*/ forwarder System.Runtime.InteropServices.ExternalException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002AA*/ forwarder System.Runtime.InteropServices.FieldOffsetAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002AB*/ forwarder System.Runtime.InteropServices.GCHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002AC*/ forwarder System.Runtime.InteropServices.GCHandleType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002AD*/ forwarder System.Runtime.InteropServices.GuidAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002AE*/ forwarder System.Runtime.InteropServices.HandleRef
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002AF*/ forwarder System.Runtime.InteropServices.ICustomAdapter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B0*/ forwarder System.Runtime.InteropServices.ICustomFactory
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B1*/ forwarder System.Runtime.InteropServices.ICustomMarshaler
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B2*/ forwarder System.Runtime.InteropServices.ICustomQueryInterface
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B3*/ forwarder System.Runtime.InteropServices.InAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B4*/ forwarder System.Runtime.InteropServices.InterfaceTypeAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B5*/ forwarder System.Runtime.InteropServices.InvalidComObjectException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B6*/ forwarder System.Runtime.InteropServices.InvalidOleVariantTypeException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B7*/ forwarder System.Runtime.InteropServices.LayoutKind
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B8*/ forwarder System.Runtime.InteropServices.LCIDConversionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002B9*/ forwarder System.Runtime.InteropServices.Marshal
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002BA*/ forwarder System.Runtime.InteropServices.MarshalAsAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002BB*/ forwarder System.Runtime.InteropServices.MarshalDirectiveException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002BC*/ forwarder System.Runtime.InteropServices.OptionalAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002BD*/ forwarder System.Runtime.InteropServices.OutAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002BE*/ forwarder System.Runtime.InteropServices.PreserveSigAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002BF*/ forwarder System.Runtime.InteropServices.PrimaryInteropAssemblyAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C0*/ forwarder System.Runtime.InteropServices.ProgIdAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C1*/ forwarder System.Runtime.InteropServices.RuntimeEnvironment
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C2*/ forwarder System.Runtime.InteropServices.SafeArrayRankMismatchException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C3*/ forwarder System.Runtime.InteropServices.SafeArrayTypeMismatchException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C4*/ forwarder System.Runtime.InteropServices.SafeBuffer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C5*/ forwarder System.Runtime.InteropServices.SafeHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C6*/ forwarder System.Runtime.InteropServices.SEHException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C7*/ forwarder System.Runtime.InteropServices.StructLayoutAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C8*/ forwarder System.Runtime.InteropServices.TypeIdentifierAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002C9*/ forwarder System.Runtime.InteropServices.UnknownWrapper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002CA*/ forwarder System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002CB*/ forwarder System.Runtime.InteropServices.UnmanagedType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002CC*/ forwarder System.Runtime.InteropServices.VarEnum
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002CD*/ forwarder System.Runtime.InteropServices.VariantWrapper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002CE*/ forwarder System.Runtime.InteropServices.ComTypes.BINDPTR
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002CF*/ forwarder System.Runtime.InteropServices.ComTypes.BIND_OPTS
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D0*/ forwarder System.Runtime.InteropServices.ComTypes.CALLCONV
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D1*/ forwarder System.Runtime.InteropServices.ComTypes.CONNECTDATA
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D2*/ forwarder System.Runtime.InteropServices.ComTypes.DESCKIND
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D3*/ forwarder System.Runtime.InteropServices.ComTypes.DISPPARAMS
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D4*/ forwarder System.Runtime.InteropServices.ComTypes.ELEMDESC
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D5*/ DESCUNION
{
  .class extern System.Runtime.InteropServices.ComTypes.ELEMDESC /*270002D4*/ 
}
.class extern /*270002D6*/ forwarder System.Runtime.InteropServices.ComTypes.EXCEPINFO
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D7*/ forwarder System.Runtime.InteropServices.ComTypes.FILETIME
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D8*/ forwarder System.Runtime.InteropServices.ComTypes.FUNCDESC
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002D9*/ forwarder System.Runtime.InteropServices.ComTypes.FUNCFLAGS
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002DA*/ forwarder System.Runtime.InteropServices.ComTypes.FUNCKIND
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002DB*/ forwarder System.Runtime.InteropServices.ComTypes.IBindCtx
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002DC*/ forwarder System.Runtime.InteropServices.ComTypes.IConnectionPoint
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002DD*/ forwarder System.Runtime.InteropServices.ComTypes.IConnectionPointContainer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002DE*/ forwarder System.Runtime.InteropServices.ComTypes.IDLDESC
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002DF*/ forwarder System.Runtime.InteropServices.ComTypes.IDLFLAG
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E0*/ forwarder System.Runtime.InteropServices.ComTypes.IEnumConnectionPoints
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E1*/ forwarder System.Runtime.InteropServices.ComTypes.IEnumConnections
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E2*/ forwarder System.Runtime.InteropServices.ComTypes.IEnumMoniker
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E3*/ forwarder System.Runtime.InteropServices.ComTypes.IEnumString
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E4*/ forwarder System.Runtime.InteropServices.ComTypes.IEnumVARIANT
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E5*/ forwarder System.Runtime.InteropServices.ComTypes.IMoniker
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E6*/ forwarder System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E7*/ forwarder System.Runtime.InteropServices.ComTypes.INVOKEKIND
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E8*/ forwarder System.Runtime.InteropServices.ComTypes.IPersistFile
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002E9*/ forwarder System.Runtime.InteropServices.ComTypes.IRunningObjectTable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002EA*/ forwarder System.Runtime.InteropServices.ComTypes.IStream
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002EB*/ forwarder System.Runtime.InteropServices.ComTypes.ITypeComp
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002EC*/ forwarder System.Runtime.InteropServices.ComTypes.ITypeInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002ED*/ forwarder System.Runtime.InteropServices.ComTypes.ITypeInfo2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002EE*/ forwarder System.Runtime.InteropServices.ComTypes.ITypeLib
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002EF*/ forwarder System.Runtime.InteropServices.ComTypes.ITypeLib2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F0*/ forwarder System.Runtime.InteropServices.ComTypes.LIBFLAGS
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F1*/ forwarder System.Runtime.InteropServices.ComTypes.PARAMDESC
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F2*/ forwarder System.Runtime.InteropServices.ComTypes.PARAMFLAG
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F3*/ forwarder System.Runtime.InteropServices.ComTypes.STATSTG
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F4*/ forwarder System.Runtime.InteropServices.ComTypes.SYSKIND
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F5*/ forwarder System.Runtime.InteropServices.ComTypes.TYPEATTR
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F6*/ forwarder System.Runtime.InteropServices.ComTypes.TYPEDESC
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F7*/ forwarder System.Runtime.InteropServices.ComTypes.TYPEFLAGS
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F8*/ forwarder System.Runtime.InteropServices.ComTypes.TYPEKIND
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002F9*/ forwarder System.Runtime.InteropServices.ComTypes.TYPELIBATTR
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002FA*/ forwarder System.Runtime.InteropServices.ComTypes.VARDESC
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002FB*/ DESCUNION
{
  .class extern System.Runtime.InteropServices.ComTypes.VARDESC /*270002FA*/ 
}
.class extern /*270002FC*/ forwarder System.Runtime.InteropServices.ComTypes.VARFLAGS
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002FD*/ forwarder System.Runtime.InteropServices.ComTypes.VARKIND
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002FE*/ forwarder System.Runtime.Serialization.Formatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270002FF*/ forwarder System.Runtime.Serialization.FormatterConverter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000300*/ forwarder System.Runtime.Serialization.FormatterServices
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000301*/ forwarder System.Runtime.Serialization.IDeserializationCallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000302*/ forwarder System.Runtime.Serialization.IFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000303*/ forwarder System.Runtime.Serialization.IFormatterConverter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000304*/ forwarder System.Runtime.Serialization.IObjectReference
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000305*/ forwarder System.Runtime.Serialization.ISafeSerializationData
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000306*/ forwarder System.Runtime.Serialization.ISerializable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000307*/ forwarder System.Runtime.Serialization.ISerializationSurrogate
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000308*/ forwarder System.Runtime.Serialization.ISurrogateSelector
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000309*/ forwarder System.Runtime.Serialization.ObjectIDGenerator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700030A*/ forwarder System.Runtime.Serialization.ObjectManager
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700030B*/ forwarder System.Runtime.Serialization.OnDeserializedAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700030C*/ forwarder System.Runtime.Serialization.OnDeserializingAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700030D*/ forwarder System.Runtime.Serialization.OnSerializedAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700030E*/ forwarder System.Runtime.Serialization.OnSerializingAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700030F*/ forwarder System.Runtime.Serialization.OptionalFieldAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000310*/ forwarder System.Runtime.Serialization.SafeSerializationEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000311*/ forwarder System.Runtime.Serialization.SerializationBinder
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000312*/ forwarder System.Runtime.Serialization.SerializationEntry
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000313*/ forwarder System.Runtime.Serialization.SerializationException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000314*/ forwarder System.Runtime.Serialization.SerializationInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000315*/ forwarder System.Runtime.Serialization.SerializationInfoEnumerator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000316*/ forwarder System.Runtime.Serialization.SerializationObjectManager
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000317*/ forwarder System.Runtime.Serialization.StreamingContext
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000318*/ forwarder System.Runtime.Serialization.StreamingContextStates
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000319*/ forwarder System.Runtime.Serialization.SurrogateSelector
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700031A*/ forwarder System.Runtime.Serialization.Formatters.FormatterAssemblyStyle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700031B*/ forwarder System.Runtime.Serialization.Formatters.FormatterTypeStyle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700031C*/ forwarder System.Runtime.Serialization.Formatters.TypeFilterLevel
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700031D*/ forwarder System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700031E*/ forwarder System.Runtime.Versioning.ComponentGuaranteesAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700031F*/ forwarder System.Runtime.Versioning.ComponentGuaranteesOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000320*/ forwarder System.Runtime.Versioning.ResourceConsumptionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000321*/ forwarder System.Runtime.Versioning.ResourceExposureAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000322*/ forwarder System.Runtime.Versioning.ResourceScope
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000323*/ forwarder System.Runtime.Versioning.TargetFrameworkAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000324*/ forwarder System.Runtime.Versioning.VersioningHelper
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000325*/ forwarder System.Security.AllowPartiallyTrustedCallersAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000326*/ forwarder System.Security.IPermission
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000327*/ forwarder System.Security.ISecurityEncodable
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000328*/ forwarder System.Security.PartialTrustVisibilityLevel
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000329*/ forwarder System.Security.SecureString
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700032A*/ forwarder System.Security.SecurityCriticalAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700032B*/ forwarder System.Security.SecurityCriticalScope
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700032C*/ forwarder System.Security.SecurityElement
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700032D*/ forwarder System.Security.SecurityException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700032E*/ forwarder System.Security.SecurityRulesAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700032F*/ forwarder System.Security.SecurityRuleSet
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000330*/ forwarder System.Security.SecuritySafeCriticalAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000331*/ forwarder System.Security.SecurityTransparentAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000332*/ forwarder System.Security.SecurityTreatAsSafeAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000333*/ forwarder System.Security.SuppressUnmanagedCodeSecurityAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000334*/ forwarder System.Security.UnverifiableCodeAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000335*/ forwarder System.Security.VerificationException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000336*/ forwarder System.Security.AccessControl.AccessControlActions
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000337*/ forwarder System.Security.AccessControl.AccessControlModification
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000338*/ forwarder System.Security.AccessControl.AccessControlSections
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000339*/ forwarder System.Security.AccessControl.AccessControlType
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700033A*/ forwarder System.Security.AccessControl.AccessRule
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700033B*/ forwarder System.Security.AccessControl.AccessRule`1
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700033C*/ forwarder System.Security.AccessControl.AceEnumerator
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700033D*/ forwarder System.Security.AccessControl.AceFlags
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700033E*/ forwarder System.Security.AccessControl.AceQualifier
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700033F*/ forwarder System.Security.AccessControl.AceType
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000340*/ forwarder System.Security.AccessControl.AuditFlags
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000341*/ forwarder System.Security.AccessControl.AuditRule
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000342*/ forwarder System.Security.AccessControl.AuditRule`1
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000343*/ forwarder System.Security.AccessControl.AuthorizationRule
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000344*/ forwarder System.Security.AccessControl.AuthorizationRuleCollection
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000345*/ forwarder System.Security.AccessControl.CommonAce
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000346*/ forwarder System.Security.AccessControl.CommonAcl
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000347*/ forwarder System.Security.AccessControl.CommonObjectSecurity
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000348*/ forwarder System.Security.AccessControl.CommonSecurityDescriptor
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000349*/ forwarder System.Security.AccessControl.CompoundAce
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700034A*/ forwarder System.Security.AccessControl.CompoundAceType
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700034B*/ forwarder System.Security.AccessControl.ControlFlags
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700034C*/ forwarder System.Security.AccessControl.CustomAce
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700034D*/ forwarder System.Security.AccessControl.DiscretionaryAcl
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700034E*/ forwarder System.Security.AccessControl.GenericAce
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700034F*/ forwarder System.Security.AccessControl.GenericAcl
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000350*/ forwarder System.Security.AccessControl.GenericSecurityDescriptor
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000351*/ forwarder System.Security.AccessControl.InheritanceFlags
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000352*/ forwarder System.Security.AccessControl.KnownAce
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000353*/ forwarder System.Security.AccessControl.NativeObjectSecurity
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000354*/ ExceptionFromErrorCode
{
  .class extern System.Security.AccessControl.NativeObjectSecurity /*27000353*/ 
}
.class extern /*27000355*/ forwarder System.Security.AccessControl.ObjectAccessRule
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000356*/ forwarder System.Security.AccessControl.ObjectAce
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000357*/ forwarder System.Security.AccessControl.ObjectAceFlags
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000358*/ forwarder System.Security.AccessControl.ObjectAuditRule
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000359*/ forwarder System.Security.AccessControl.ObjectSecurity
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700035A*/ forwarder System.Security.AccessControl.ObjectSecurity`1
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700035B*/ forwarder System.Security.AccessControl.PrivilegeNotHeldException
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700035C*/ forwarder System.Security.AccessControl.PropagationFlags
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700035D*/ forwarder System.Security.AccessControl.QualifiedAce
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700035E*/ forwarder System.Security.AccessControl.RawAcl
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*2700035F*/ forwarder System.Security.AccessControl.RawSecurityDescriptor
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000360*/ forwarder System.Security.AccessControl.RegistryAccessRule
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000361*/ forwarder System.Security.AccessControl.RegistryAuditRule
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000362*/ forwarder System.Security.AccessControl.RegistryRights
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000363*/ forwarder System.Security.AccessControl.RegistrySecurity
{
  .assembly extern Microsoft.Win32.Registry /*23000002*/ 
}
.class extern /*27000364*/ forwarder System.Security.AccessControl.ResourceType
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000365*/ forwarder System.Security.AccessControl.SecurityInfos
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000366*/ forwarder System.Security.AccessControl.SystemAcl
{
  .assembly extern System.Security.AccessControl /*23000004*/ 
}
.class extern /*27000367*/ forwarder System.Security.Claims.Claim
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000368*/ forwarder System.Security.Claims.ClaimsIdentity
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000369*/ forwarder System.Security.Claims.ClaimsPrincipal
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700036A*/ forwarder System.Security.Claims.ClaimTypes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700036B*/ forwarder System.Security.Claims.ClaimValueTypes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700036C*/ forwarder System.Security.Cryptography.Aes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700036D*/ forwarder System.Security.Cryptography.AsymmetricAlgorithm
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700036E*/ forwarder System.Security.Cryptography.AsymmetricKeyExchangeDeformatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700036F*/ forwarder System.Security.Cryptography.AsymmetricKeyExchangeFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000370*/ forwarder System.Security.Cryptography.AsymmetricSignatureDeformatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000371*/ forwarder System.Security.Cryptography.AsymmetricSignatureFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000372*/ forwarder System.Security.Cryptography.CipherMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000373*/ forwarder System.Security.Cryptography.CryptoConfig
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000374*/ forwarder System.Security.Cryptography.CryptographicException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000375*/ forwarder System.Security.Cryptography.CryptographicUnexpectedOperationException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000376*/ forwarder System.Security.Cryptography.CryptoStream
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000377*/ forwarder System.Security.Cryptography.CryptoStreamMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000378*/ forwarder System.Security.Cryptography.CspKeyContainerInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000379*/ forwarder System.Security.Cryptography.CspParameters
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700037A*/ forwarder System.Security.Cryptography.CspProviderFlags
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700037B*/ forwarder System.Security.Cryptography.DeriveBytes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700037C*/ forwarder System.Security.Cryptography.DES
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700037D*/ forwarder System.Security.Cryptography.DESCryptoServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700037E*/ forwarder System.Security.Cryptography.DSA
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700037F*/ forwarder System.Security.Cryptography.DSACryptoServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000380*/ forwarder System.Security.Cryptography.DSAParameters
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000381*/ forwarder System.Security.Cryptography.DSASignatureDeformatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000382*/ forwarder System.Security.Cryptography.DSASignatureFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000383*/ forwarder System.Security.Cryptography.FromBase64Transform
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000384*/ forwarder System.Security.Cryptography.FromBase64TransformMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000385*/ forwarder System.Security.Cryptography.HashAlgorithm
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000386*/ forwarder System.Security.Cryptography.HashAlgorithmName
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000387*/ forwarder System.Security.Cryptography.HMAC
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000388*/ forwarder System.Security.Cryptography.HMACMD5
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000389*/ forwarder System.Security.Cryptography.HMACSHA1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700038A*/ forwarder System.Security.Cryptography.HMACSHA256
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700038B*/ forwarder System.Security.Cryptography.HMACSHA384
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700038C*/ forwarder System.Security.Cryptography.HMACSHA512
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700038D*/ forwarder System.Security.Cryptography.ICryptoTransform
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700038E*/ forwarder System.Security.Cryptography.ICspAsymmetricAlgorithm
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700038F*/ forwarder System.Security.Cryptography.KeyedHashAlgorithm
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000390*/ forwarder System.Security.Cryptography.KeyNumber
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000391*/ forwarder System.Security.Cryptography.KeySizes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000392*/ forwarder System.Security.Cryptography.MaskGenerationMethod
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000393*/ forwarder System.Security.Cryptography.MD5
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000394*/ forwarder System.Security.Cryptography.MD5CryptoServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000395*/ forwarder System.Security.Cryptography.PaddingMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000396*/ forwarder System.Security.Cryptography.PasswordDeriveBytes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000397*/ forwarder System.Security.Cryptography.PKCS1MaskGenerationMethod
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000398*/ forwarder System.Security.Cryptography.RandomNumberGenerator
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000399*/ forwarder System.Security.Cryptography.RC2
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700039A*/ forwarder System.Security.Cryptography.RC2CryptoServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700039B*/ forwarder System.Security.Cryptography.Rfc2898DeriveBytes
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700039C*/ forwarder System.Security.Cryptography.Rijndael
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700039D*/ forwarder System.Security.Cryptography.RijndaelManaged
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700039E*/ forwarder System.Security.Cryptography.RNGCryptoServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700039F*/ forwarder System.Security.Cryptography.RSA
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A0*/ forwarder System.Security.Cryptography.RSACryptoServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A1*/ forwarder System.Security.Cryptography.RSAEncryptionPadding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A2*/ forwarder System.Security.Cryptography.RSAEncryptionPaddingMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A3*/ forwarder System.Security.Cryptography.RSAOAEPKeyExchangeDeformatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A4*/ forwarder System.Security.Cryptography.RSAOAEPKeyExchangeFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A5*/ forwarder System.Security.Cryptography.RSAParameters
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A6*/ forwarder System.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A7*/ forwarder System.Security.Cryptography.RSAPKCS1KeyExchangeFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A8*/ forwarder System.Security.Cryptography.RSAPKCS1SignatureDeformatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003A9*/ forwarder System.Security.Cryptography.RSAPKCS1SignatureFormatter
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003AA*/ forwarder System.Security.Cryptography.RSASignaturePadding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003AB*/ forwarder System.Security.Cryptography.RSASignaturePaddingMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003AC*/ forwarder System.Security.Cryptography.SHA1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003AD*/ forwarder System.Security.Cryptography.SHA1CryptoServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003AE*/ forwarder System.Security.Cryptography.SHA1Managed
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003AF*/ forwarder System.Security.Cryptography.SHA256
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B0*/ forwarder System.Security.Cryptography.SHA256Managed
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B1*/ forwarder System.Security.Cryptography.SHA384
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B2*/ forwarder System.Security.Cryptography.SHA384Managed
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B3*/ forwarder System.Security.Cryptography.SHA512
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B4*/ forwarder System.Security.Cryptography.SHA512Managed
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B5*/ forwarder System.Security.Cryptography.SignatureDescription
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B6*/ forwarder System.Security.Cryptography.SymmetricAlgorithm
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B7*/ forwarder System.Security.Cryptography.ToBase64Transform
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B8*/ forwarder System.Security.Cryptography.TripleDES
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003B9*/ forwarder System.Security.Cryptography.TripleDESCryptoServiceProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003BA*/ forwarder System.Security.Cryptography.X509Certificates.X509Certificate
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003BB*/ forwarder System.Security.Cryptography.X509Certificates.X509ContentType
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003BC*/ forwarder System.Security.Cryptography.X509Certificates.X509KeyStorageFlags
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003BD*/ forwarder System.Security.Permissions.CodeAccessSecurityAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003BE*/ forwarder System.Security.Permissions.SecurityAction
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003BF*/ forwarder System.Security.Permissions.SecurityAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003C0*/ forwarder System.Security.Permissions.SecurityPermissionAttribute
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003C1*/ forwarder System.Security.Permissions.SecurityPermissionFlag
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003C2*/ forwarder System.Security.Principal.GenericIdentity
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003C3*/ forwarder System.Security.Principal.GenericPrincipal
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003C4*/ forwarder System.Security.Principal.IdentityNotMappedException
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003C5*/ forwarder System.Security.Principal.IdentityReference
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003C6*/ forwarder System.Security.Principal.IdentityReferenceCollection
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003C7*/ forwarder System.Security.Principal.IIdentity
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003C8*/ forwarder System.Security.Principal.IPrincipal
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003C9*/ forwarder System.Security.Principal.NTAccount
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003CA*/ forwarder System.Security.Principal.PrincipalPolicy
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003CB*/ forwarder System.Security.Principal.SecurityIdentifier
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003CC*/ forwarder System.Security.Principal.TokenAccessLevels
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003CD*/ forwarder System.Security.Principal.TokenImpersonationLevel
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003CE*/ forwarder System.Security.Principal.WellKnownSidType
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003CF*/ forwarder System.Security.Principal.WindowsBuiltInRole
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003D0*/ forwarder System.Security.Principal.WindowsIdentity
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003D1*/ forwarder System.Security.Principal.WindowsPrincipal
{
  .assembly extern System.Security.Principal.Windows /*23000003*/ 
}
.class extern /*270003D2*/ forwarder System.Text.ASCIIEncoding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003D3*/ forwarder System.Text.Decoder
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003D4*/ forwarder System.Text.DecoderExceptionFallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003D5*/ forwarder System.Text.DecoderExceptionFallbackBuffer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003D6*/ forwarder System.Text.DecoderFallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003D7*/ forwarder System.Text.DecoderFallbackBuffer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003D8*/ forwarder System.Text.DecoderFallbackException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003D9*/ forwarder System.Text.DecoderReplacementFallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003DA*/ forwarder System.Text.DecoderReplacementFallbackBuffer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003DB*/ forwarder System.Text.Encoder
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003DC*/ forwarder System.Text.EncoderExceptionFallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003DD*/ forwarder System.Text.EncoderExceptionFallbackBuffer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003DE*/ forwarder System.Text.EncoderFallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003DF*/ forwarder System.Text.EncoderFallbackBuffer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E0*/ forwarder System.Text.EncoderFallbackException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E1*/ forwarder System.Text.EncoderReplacementFallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E2*/ forwarder System.Text.EncoderReplacementFallbackBuffer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E3*/ forwarder System.Text.Encoding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E4*/ forwarder System.Text.EncodingInfo
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E5*/ forwarder System.Text.EncodingProvider
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E6*/ forwarder System.Text.NormalizationForm
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E7*/ forwarder System.Text.StringBuilder
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E8*/ forwarder System.Text.UnicodeEncoding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003E9*/ forwarder System.Text.UTF32Encoding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003EA*/ forwarder System.Text.UTF7Encoding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003EB*/ forwarder System.Text.UTF8Encoding
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003EC*/ forwarder System.Threading.AbandonedMutexException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003ED*/ forwarder System.Threading.ApartmentState
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003EE*/ forwarder System.Threading.AsyncFlowControl
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003EF*/ forwarder System.Threading.AsyncLocalValueChangedArgs`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F0*/ forwarder System.Threading.AsyncLocal`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F1*/ forwarder System.Threading.AutoResetEvent
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F2*/ forwarder System.Threading.CancellationToken
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F3*/ forwarder System.Threading.CancellationTokenRegistration
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F4*/ forwarder System.Threading.CancellationTokenSource
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F5*/ forwarder System.Threading.CompressedStack
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F6*/ forwarder System.Threading.ContextCallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F7*/ forwarder System.Threading.CountdownEvent
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F8*/ forwarder System.Threading.EventResetMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003F9*/ forwarder System.Threading.EventWaitHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003FA*/ forwarder System.Threading.ExecutionContext
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003FB*/ forwarder System.Threading.HostExecutionContext
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003FC*/ forwarder System.Threading.HostExecutionContextManager
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003FD*/ forwarder System.Threading.Interlocked
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003FE*/ forwarder System.Threading.IOCompletionCallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*270003FF*/ forwarder System.Threading.LazyInitializer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000400*/ forwarder System.Threading.LazyThreadSafetyMode
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000401*/ forwarder System.Threading.LockCookie
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000402*/ forwarder System.Threading.LockRecursionException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000403*/ forwarder System.Threading.ManualResetEvent
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000404*/ forwarder System.Threading.ManualResetEventSlim
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000405*/ forwarder System.Threading.Monitor
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000406*/ forwarder System.Threading.Mutex
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000407*/ forwarder System.Threading.NativeOverlapped
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000408*/ forwarder System.Threading.Overlapped
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000409*/ forwarder System.Threading.ParameterizedThreadStart
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700040A*/ forwarder System.Threading.ReaderWriterLock
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700040B*/ forwarder System.Threading.RegisteredWaitHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700040C*/ forwarder System.Threading.SemaphoreFullException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700040D*/ forwarder System.Threading.SemaphoreSlim
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700040E*/ forwarder System.Threading.SendOrPostCallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700040F*/ forwarder System.Threading.SpinLock
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000410*/ forwarder System.Threading.SpinWait
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000411*/ forwarder System.Threading.SynchronizationContext
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000412*/ forwarder System.Threading.SynchronizationLockException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000413*/ forwarder System.Threading.Thread
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000414*/ forwarder System.Threading.ThreadAbortException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000415*/ forwarder System.Threading.ThreadInterruptedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000416*/ forwarder System.Threading.ThreadLocal`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000417*/ forwarder System.Threading.ThreadPool
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000418*/ forwarder System.Threading.ThreadPriority
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000419*/ forwarder System.Threading.ThreadStart
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700041A*/ forwarder System.Threading.ThreadStartException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700041B*/ forwarder System.Threading.ThreadState
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700041C*/ forwarder System.Threading.ThreadStateException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700041D*/ forwarder System.Threading.Timeout
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700041E*/ forwarder System.Threading.Timer
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700041F*/ forwarder System.Threading.TimerCallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000420*/ forwarder System.Threading.Volatile
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000421*/ forwarder System.Threading.WaitCallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000422*/ forwarder System.Threading.WaitHandle
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000423*/ forwarder System.Threading.WaitHandleCannotBeOpenedException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000424*/ forwarder System.Threading.WaitHandleExtensions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000425*/ forwarder System.Threading.WaitOrTimerCallback
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000426*/ forwarder System.Threading.Tasks.ConcurrentExclusiveSchedulerPair
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000427*/ forwarder System.Threading.Tasks.Parallel
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000428*/ forwarder System.Threading.Tasks.ParallelLoopResult
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000429*/ forwarder System.Threading.Tasks.ParallelLoopState
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700042A*/ forwarder System.Threading.Tasks.ParallelOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700042B*/ forwarder System.Threading.Tasks.Task
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700042C*/ forwarder System.Threading.Tasks.TaskCanceledException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700042D*/ forwarder System.Threading.Tasks.TaskCompletionSource`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700042E*/ forwarder System.Threading.Tasks.TaskContinuationOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*2700042F*/ forwarder System.Threading.Tasks.TaskCreationOptions
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000430*/ forwarder System.Threading.Tasks.TaskFactory
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000431*/ forwarder System.Threading.Tasks.TaskFactory`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000432*/ forwarder System.Threading.Tasks.TaskScheduler
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000433*/ forwarder System.Threading.Tasks.TaskSchedulerException
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000434*/ forwarder System.Threading.Tasks.TaskStatus
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000435*/ forwarder System.Threading.Tasks.Task`1
{
  .assembly extern netstandard /*23000001*/ 
}
.class extern /*27000436*/ forwarder System.Threading.Tasks.UnobservedTaskExceptionEventArgs
{
  .assembly extern netstandard /*23000001*/ 
}
.module mscorlib.dll
// MVID: {a9d33822-1d27-4e72-b121-f7083d8427f4}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007FFAFE7A6000

// *********** DISASSEMBLY COMPLETE ***********************
