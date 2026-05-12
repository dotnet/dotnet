


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
// Time-date stamp:            0xa387da8a
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
// Addr. of entry point:           0x00004a2a
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
// Checksum:                       0x0001422a
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000049d8 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000498] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003600 [0x00002908] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000049bc [0x0000001c] address [size] of Debug Directory:           
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
//              0x00002a30 Virtual Size
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
//              0x00000498 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000a2c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00004a1a Import Name Table
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
// 0x00002210 [0x0000272c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000493c [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000011a8 Size
//              '#~' Name
//    Stream 2:
//              0x00001214 Offset
//              0x00000a9c Size
//              '#Strings' Name
//    Stream 3:
//              0x00001cb0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001cb4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001cc4 Offset
//              0x00000a68 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000903a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 24328
// PE header size       : 512 (496 used)    ( 2.10%)
// PE additional info   : 11879             (48.83%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.30%)
// CLR meta-data size  : 10028              (41.22%)
// CLR additional info : 128                ( 0.53%)
// CLR method headers  : 168                ( 0.69%)
// Managed code         : 275               ( 1.13%)
// Data                 : 2048              ( 8.42%)
// Unaccounted          : -782              (-3.21%)

// Num.of PE sections   : 3
//   .text    - 11264
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 10028
//   Module        -    1 (10 bytes)
//   TypeDef       -   15 (210 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   45 (270 bytes)
//   MethodDef     -  122 (1708 bytes)    0 abstract, 0 native, 110 bodies
//   FieldDef      -   21 (126 bytes)     0 constant
//   MemberRef     -   31 (186 bytes)
//   ParamDef      -  108 (648 bytes)
//   MethodImpl    -    2 (12 bytes)
//   Constant      -   18 (108 bytes)
//   CustomAttribute-  142 (852 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    3 (12 bytes)
//   Property      -   15 (90 bytes)
//   MethodSemantic-   22 (132 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   Strings       -  2716 bytes
//   Blobs         -  2664 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   198 bytes

// CLR additional info : 128

// CLR method headers : 168
//   Num.of method bodies  - 110
//   Num.of fat headers    - 2
//   Num.of tiny headers   - 108
//   Num.of fat sections   - 0
//   Num.of small sections - 2

// Managed code : 275
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class LocalDataStoreSlot             (public) (auto) (ansi) (sealed) 
// Class ApartmentState                 (public) (auto) (ansi) (sealed) 
// Class CompressedStack                (public) (auto) (ansi) (sealed) 
// Class ParameterizedThreadStart       (public) (auto) (ansi) (sealed) 
// Class Thread                         (public) (auto) (ansi) (sealed) 
// Class ThreadAbortException           (public) (auto) (ansi) (sealed) 
// Class ThreadExceptionEventArgs       (public) (auto) (ansi) 
// Class ThreadExceptionEventHandler    (public) (auto) (ansi) (sealed) 
// Class ThreadInterruptedException     (public) (auto) (ansi) 
// Class ThreadPriority                 (public) (auto) (ansi) (sealed) 
// Class ThreadStart                    (public) (auto) (ansi) (sealed) 
// Class ThreadStartException           (public) (auto) (ansi) (sealed) 
// Class ThreadState                    (public) (auto) (ansi) (sealed) 
// Class ThreadStateException           (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000002*/ System.Threading
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.Threading.Thread
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

  .custom /*0C000008:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 17 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                        69 6E 67 2E 54 68 72 65 61 64 00 00 )             // ing.Thread..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C000010:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 17 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                       69 6E 67 2E 54 68 72 65 61 64 00 00 )             // ing.Thread..
  .custom /*0C000013:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000014:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000015:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000016:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 17 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                 69 6E 67 2E 54 68 72 65 61 64 00 00 )             // ing.Thread..
  .custom /*0C000017:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 64 6F 74 6E 65   // com/dotnet/dotne
                                                                                                                                                                    74 00 00 )                                        // t..
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
  .ver 10:0:0:0
}
.module System.Threading.Thread.dll
// MVID: {cd04a219-8317-4566-ac2c-150cb2e3e4b4}
.custom /*0C000002:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit System.LocalDataStoreSlot
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*06000001*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LocalDataStoreSlot::.ctor

  .method /*06000002*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000013*/::Finalize /*01000013::0A000012*/ 
    // Method begins at RVA 0x2058
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)000012       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::Finalize() /* 0A000012 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method LocalDataStoreSlot::Finalize

} // end of class System.LocalDataStoreSlot

.class /*02000003*/ public auto ansi sealed System.Threading.ApartmentState
       extends [System.Runtime/*23000001*/]System.Enum/*01000014*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Threading.ApartmentState/*02000003*/ STA = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Threading.ApartmentState/*02000003*/ MTA = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Threading.ApartmentState/*02000003*/ Unknown = int32(0x00000002)
} // end of class System.Threading.ApartmentState

.class /*02000004*/ public auto ansi sealed beforefieldinit System.Threading.CompressedStack
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000017*/
{
  .custom /*0C000019:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000017*/
  .custom /*0C000001:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000003*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2080
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CompressedStack::.ctor

  .method /*06000004*/ public hidebysig static 
          class System.Threading.CompressedStack/*02000004*/ 
          Capture() cil managed
  // SIG: 00 00 12 10
  {
    // Method begins at RVA 0x2088
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompressedStack::Capture

  .method /*06000005*/ public hidebysig instance class System.Threading.CompressedStack/*02000004*/ 
          CreateCopy() cil managed
  // SIG: 20 00 12 10
  {
    // Method begins at RVA 0x208b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompressedStack::CreateCopy

  .method /*06000006*/ public hidebysig static 
          class System.Threading.CompressedStack/*02000004*/ 
          GetCompressedStack() cil managed
  // SIG: 00 00 12 10
  {
    // Method begins at RVA 0x208e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompressedStack::GetCompressedStack

  .method /*06000007*/ public hidebysig newslot virtual final 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C00001F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    .custom /*0C000020:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2091
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CompressedStack::GetObjectData

  .method /*06000008*/ public hidebysig static 
          void  Run(class System.Threading.CompressedStack/*02000004*/ compressedStack,
                    class [System.Threading/*23000002*/]System.Threading.ContextCallback/*0100001D*/ callback,
                    object state) cil managed
  // SIG: 00 03 01 12 10 12 75 1C
  {
    .param [3]/*08000005*/ 
    .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2093
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CompressedStack::Run

} // end of class System.Threading.CompressedStack

.class /*02000005*/ public auto ansi sealed System.Threading.ParameterizedThreadStart
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001E*/
{
  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ParameterizedThreadStart::.ctor

  .method /*0600000A*/ public hidebysig newslot virtual 
          instance void  Invoke(object obj) runtime managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  } // end of method ParameterizedThreadStart::Invoke

  .method /*0600000B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ 
          BeginInvoke(object obj,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000020*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 7D 1C 12 80 81 1C
  {
    .param [1]/*08000009*/ 
    .custom /*0C000025:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method ParameterizedThreadStart::BeginInvoke

  .method /*0600000C*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ result) runtime managed
  // SIG: 20 01 01 12 7D
  {
  } // end of method ParameterizedThreadStart::EndInvoke

} // end of class System.Threading.ParameterizedThreadStart

.class /*02000006*/ public auto ansi sealed beforefieldinit System.Threading.Thread
       extends [System.Runtime/*23000001*/]System.Runtime.ConstrainedExecution.CriticalFinalizerObject/*01000021*/
{
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600000D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Threading.ParameterizedThreadStart/*02000005*/ start) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x2095
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.ConstrainedExecution.CriticalFinalizerObject/*01000021*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Thread::.ctor

  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Threading.ParameterizedThreadStart/*02000005*/ start,
                               int32 maxStackSize) cil managed
  // SIG: 20 02 01 12 14 08
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.ConstrainedExecution.CriticalFinalizerObject/*01000021*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Thread::.ctor

  .method /*0600000F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Threading.ThreadStart/*0200000C*/ start) cil managed
  // SIG: 20 01 01 12 30
  {
    // Method begins at RVA 0x20a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.ConstrainedExecution.CriticalFinalizerObject/*01000021*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Thread::.ctor

  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Threading.ThreadStart/*0200000C*/ start,
                               int32 maxStackSize) cil managed
  // SIG: 20 02 01 12 30 08
  {
    // Method begins at RVA 0x20ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.ConstrainedExecution.CriticalFinalizerObject/*01000021*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Thread::.ctor

  .method /*06000011*/ public hidebysig specialname 
          instance valuetype System.Threading.ApartmentState/*02000003*/ 
          get_ApartmentState() cil managed
  // SIG: 20 00 11 0C
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_ApartmentState

  .method /*06000012*/ public hidebysig specialname 
          instance void  set_ApartmentState(valuetype System.Threading.ApartmentState/*02000003*/ 'value') cil managed
  // SIG: 20 01 01 11 0C
  {
    // Method begins at RVA 0x20b8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::set_ApartmentState

  .method /*06000013*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/ 
          get_CurrentCulture() cil managed
  // SIG: 20 00 12 80 89
  {
    // Method begins at RVA 0x20ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_CurrentCulture

  .method /*06000014*/ public hidebysig specialname 
          instance void  set_CurrentCulture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/ 'value') cil managed
  // SIG: 20 01 01 12 80 89
  {
    // Method begins at RVA 0x20bd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::set_CurrentCulture

  .method /*06000015*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Security.Principal.IPrincipal/*01000023*/ 
          get_CurrentPrincipal() cil managed
  // SIG: 00 00 12 80 8D
  {
    .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_CurrentPrincipal

  .method /*06000016*/ public hidebysig specialname static 
          void  set_CurrentPrincipal(class [System.Runtime/*23000001*/]System.Security.Principal.IPrincipal/*01000023*/ 'value') cil managed
  // SIG: 00 01 01 12 80 8D
  {
    .custom /*0C00002E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::set_CurrentPrincipal

  .method /*06000017*/ public hidebysig specialname static 
          class System.Threading.Thread/*02000006*/ 
          get_CurrentThread() cil managed
  // SIG: 00 00 12 18
  {
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_CurrentThread

  .method /*06000018*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/ 
          get_CurrentUICulture() cil managed
  // SIG: 20 00 12 80 89
  {
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_CurrentUICulture

  .method /*06000019*/ public hidebysig specialname 
          instance void  set_CurrentUICulture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/ 'value') cil managed
  // SIG: 20 01 01 12 80 89
  {
    // Method begins at RVA 0x20ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::set_CurrentUICulture

  .method /*0600001A*/ public hidebysig specialname 
          instance class [System.Threading/*23000002*/]System.Threading.ExecutionContext/*01000024*/ 
          get_ExecutionContext() cil managed
  // SIG: 20 00 12 80 91
  {
    .custom /*0C00002F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_ExecutionContext

  .method /*0600001B*/ public hidebysig specialname 
          instance bool  get_IsAlive() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_IsAlive

  .method /*0600001C*/ public hidebysig specialname 
          instance bool  get_IsBackground() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_IsBackground

  .method /*0600001D*/ public hidebysig specialname 
          instance void  set_IsBackground(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::set_IsBackground

  .method /*0600001E*/ public hidebysig specialname 
          instance bool  get_IsThreadPoolThread() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_IsThreadPoolThread

  .method /*0600001F*/ public hidebysig specialname 
          instance int32  get_ManagedThreadId() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_ManagedThreadId

  .method /*06000020*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_Name

  .method /*06000021*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::set_Name

  .method /*06000022*/ public hidebysig specialname 
          instance valuetype System.Threading.ThreadPriority/*0200000B*/ 
          get_Priority() cil managed
  // SIG: 20 00 11 2C
  {
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_Priority

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_Priority(valuetype System.Threading.ThreadPriority/*0200000B*/ 'value') cil managed
  // SIG: 20 01 01 11 2C
  {
    // Method begins at RVA 0x20e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::set_Priority

  .method /*06000024*/ public hidebysig specialname 
          instance valuetype System.Threading.ThreadState/*0200000E*/ 
          get_ThreadState() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::get_ThreadState

  .method /*06000025*/ public hidebysig instance void 
          Abort() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000033:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 47 54 68 72 65 61 64 2E 41 62 6F 72 74 20   // ..GThread.Abort 
                                                                                                                                                   69 73 20 6E 6F 74 20 73 75 70 70 6F 72 74 65 64   // is not supported
                                                                                                                                                   20 61 6E 64 20 74 68 72 6F 77 73 20 50 6C 61 74   //  and throws Plat
                                                                                                                                                   66 6F 72 6D 4E 6F 74 53 75 70 70 6F 72 74 65 64   // formNotSupported
                                                                                                                                                   45 78 63 65 70 74 69 6F 6E 2E 02 00 54 0E 0C 44   // Exception...T..D
                                                                                                                                                   69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59 53 4C   // iagnosticId.SYSL
                                                                                                                                                   49 42 30 30 30 36 54 0E 09 55 72 6C 46 6F 72 6D   // IB0006T..UrlForm
                                                                                                                                                   61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D   // at"https://aka.m
                                                                                                                                                   73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67   // s/dotnet-warning
                                                                                                                                                   73 2F 7B 30 7D )                                  // s/{0}
    // Method begins at RVA 0x20ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Abort

  .method /*06000026*/ public hidebysig instance void 
          Abort(object stateInfo) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000036:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 47 54 68 72 65 61 64 2E 41 62 6F 72 74 20   // ..GThread.Abort 
                                                                                                                                                   69 73 20 6E 6F 74 20 73 75 70 70 6F 72 74 65 64   // is not supported
                                                                                                                                                   20 61 6E 64 20 74 68 72 6F 77 73 20 50 6C 61 74   //  and throws Plat
                                                                                                                                                   66 6F 72 6D 4E 6F 74 53 75 70 70 6F 72 74 65 64   // formNotSupported
                                                                                                                                                   45 78 63 65 70 74 69 6F 6E 2E 02 00 54 0E 0C 44   // Exception...T..D
                                                                                                                                                   69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59 53 4C   // iagnosticId.SYSL
                                                                                                                                                   49 42 30 30 30 36 54 0E 09 55 72 6C 46 6F 72 6D   // IB0006T..UrlForm
                                                                                                                                                   61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D   // at"https://aka.m
                                                                                                                                                   73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67   // s/dotnet-warning
                                                                                                                                                   73 2F 7B 30 7D )                                  // s/{0}
    // Method begins at RVA 0x20ec
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Abort

  .method /*06000027*/ public hidebysig static 
          class System.LocalDataStoreSlot/*02000002*/ 
          AllocateDataSlot() cil managed
  // SIG: 00 00 12 08
  {
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::AllocateDataSlot

  .method /*06000028*/ public hidebysig static 
          class System.LocalDataStoreSlot/*02000002*/ 
          AllocateNamedDataSlot(string name) cil managed
  // SIG: 00 01 12 08 0E
  {
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::AllocateNamedDataSlot

  .method /*06000029*/ public hidebysig static 
          void  BeginCriticalRegion() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20f4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::BeginCriticalRegion

  .method /*0600002A*/ public hidebysig static 
          void  BeginThreadAffinity() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20f6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::BeginThreadAffinity

  .method /*0600002B*/ public hidebysig instance void 
          DisableComObjectEagerCleanup() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::DisableComObjectEagerCleanup

  .method /*0600002C*/ public hidebysig static 
          void  EndCriticalRegion() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20fa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::EndCriticalRegion

  .method /*0600002D*/ public hidebysig static 
          void  EndThreadAffinity() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x20fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::EndThreadAffinity

  .method /*0600002E*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000013*/::Finalize /*01000013::0A000012*/ 
    // Method begins at RVA 0x2100
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Runtime.ConstrainedExecution.CriticalFinalizerObject/*01000021*/::Finalize() /* 0A00001D */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Thread::Finalize

  .method /*0600002F*/ public hidebysig static 
          void  FreeNamedDataSlot(string name) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x2128
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::FreeNamedDataSlot

  .method /*06000030*/ public hidebysig instance valuetype System.Threading.ApartmentState/*02000003*/ 
          GetApartmentState() cil managed
  // SIG: 20 00 11 0C
  {
    // Method begins at RVA 0x212a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::GetApartmentState

  .method /*06000031*/ public hidebysig instance class System.Threading.CompressedStack/*02000004*/ 
          GetCompressedStack() cil managed
  // SIG: 20 00 12 10
  {
    .custom /*0C000037:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 40 43 6F 64 65 20 41 63 63 65 73 73 20 53   // ..@Code Access S
                                                                                                                                                   65 63 75 72 69 74 79 20 69 73 20 6E 6F 74 20 73   // ecurity is not s
                                                                                                                                                   75 70 70 6F 72 74 65 64 20 6F 72 20 68 6F 6E 6F   // upported or hono
                                                                                                                                                   72 65 64 20 62 79 20 74 68 65 20 72 75 6E 74 69   // red by the runti
                                                                                                                                                   6D 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F 73 74   // me...T..Diagnost
                                                                                                                                                   69 63 49 64 0A 53 59 53 4C 49 42 30 30 30 33 54   // icId.SYSLIB0003T
                                                                                                                                                   0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74 74 70   // ..UrlFormat"http
                                                                                                                                                   73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65   // s://aka.ms/dotne
                                                                                                                                                   74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D )       // t-warnings/{0}
    // Method begins at RVA 0x212d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::GetCompressedStack

  .method /*06000032*/ public hidebysig static 
          int32  GetCurrentProcessorId() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2130
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::GetCurrentProcessorId

  .method /*06000033*/ public hidebysig static 
          object  GetData(class System.LocalDataStoreSlot/*02000002*/ slot) cil managed
  // SIG: 00 01 1C 12 08
  {
    .param [0]/*0800001D*/ 
    .custom /*0C000030:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2133
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::GetData

  .method /*06000034*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.AppDomain/*01000025*/ 
          GetDomain() cil managed
  // SIG: 00 00 12 80 95
  {
    // Method begins at RVA 0x2136
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::GetDomain

  .method /*06000035*/ public hidebysig static 
          int32  GetDomainID() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2139
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::GetDomainID

  .method /*06000036*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x213c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::GetHashCode

  .method /*06000037*/ public hidebysig static 
          class System.LocalDataStoreSlot/*02000002*/ 
          GetNamedDataSlot(string name) cil managed
  // SIG: 00 01 12 08 0E
  {
    // Method begins at RVA 0x213f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::GetNamedDataSlot

  .method /*06000038*/ public hidebysig instance void 
          Interrupt() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2142
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Interrupt

  .method /*06000039*/ public hidebysig instance void 
          Join() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2144
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Join

  .method /*0600003A*/ public hidebysig instance bool 
          Join(int32 millisecondsTimeout) cil managed
  // SIG: 20 01 02 08
  {
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::Join

  .method /*0600003B*/ public hidebysig instance bool 
          Join(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ timeout) cil managed
  // SIG: 20 01 02 11 80 99
  {
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::Join

  .method /*0600003C*/ public hidebysig static 
          void  MemoryBarrier() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x214c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::MemoryBarrier

  .method /*0600003D*/ public hidebysig static 
          void  ResetAbort() cil managed
  // SIG: 00 00 01
  {
    .custom /*0C00003A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 4C 54 68 72 65 61 64 2E 52 65 73 65 74 41   // ..LThread.ResetA
                                                                                                                                                   62 6F 72 74 20 69 73 20 6E 6F 74 20 73 75 70 70   // bort is not supp
                                                                                                                                                   6F 72 74 65 64 20 61 6E 64 20 74 68 72 6F 77 73   // orted and throws
                                                                                                                                                   20 50 6C 61 74 66 6F 72 6D 4E 6F 74 53 75 70 70   //  PlatformNotSupp
                                                                                                                                                   6F 72 74 65 64 45 78 63 65 70 74 69 6F 6E 2E 02   // ortedException..
                                                                                                                                                   00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49 64   // .T..DiagnosticId
                                                                                                                                                   0A 53 59 53 4C 49 42 30 30 30 36 54 0E 09 55 72   // .SYSLIB0006T..Ur
                                                                                                                                                   6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F   // lFormat"https://
                                                                                                                                                   61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61   // aka.ms/dotnet-wa
                                                                                                                                                   72 6E 69 6E 67 73 2F 7B 30 7D )                   // rnings/{0}
    // Method begins at RVA 0x214e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::ResetAbort

  .method /*0600003E*/ public hidebysig instance void 
          Resume() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00003B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A4 54 68 72 65 61 64 2E 52 65 73 75 6D   // ....Thread.Resum
                                                                                                                                                   65 20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65   // e has been depre
                                                                                                                                                   63 61 74 65 64 2E 20 55 73 65 20 6F 74 68 65 72   // cated. Use other
                                                                                                                                                   20 63 6C 61 73 73 65 73 20 69 6E 20 53 79 73 74   //  classes in Syst
                                                                                                                                                   65 6D 2E 54 68 72 65 61 64 69 6E 67 2C 20 73 75   // em.Threading, su
                                                                                                                                                   63 68 20 61 73 20 4D 6F 6E 69 74 6F 72 2C 20 4D   // ch as Monitor, M
                                                                                                                                                   75 74 65 78 2C 20 45 76 65 6E 74 2C 20 61 6E 64   // utex, Event, and
                                                                                                                                                   20 53 65 6D 61 70 68 6F 72 65 2C 20 74 6F 20 73   //  Semaphore, to s
                                                                                                                                                   79 6E 63 68 72 6F 6E 69 7A 65 20 54 68 72 65 61   // ynchronize Threa
                                                                                                                                                   64 73 20 6F 72 20 70 72 6F 74 65 63 74 20 72 65   // ds or protect re
                                                                                                                                                   73 6F 75 72 63 65 73 2E 00 00 )                   // sources...
    // Method begins at RVA 0x2150
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Resume

  .method /*0600003F*/ public hidebysig instance void 
          SetApartmentState(valuetype System.Threading.ApartmentState/*02000003*/ state) cil managed
  // SIG: 20 01 01 11 0C
  {
    .custom /*0C00003C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000027*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2152
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::SetApartmentState

  .method /*06000040*/ public hidebysig instance void 
          SetCompressedStack(class System.Threading.CompressedStack/*02000004*/ stack) cil managed
  // SIG: 20 01 01 12 10
  {
    .custom /*0C00003D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 40 43 6F 64 65 20 41 63 63 65 73 73 20 53   // ..@Code Access S
                                                                                                                                                   65 63 75 72 69 74 79 20 69 73 20 6E 6F 74 20 73   // ecurity is not s
                                                                                                                                                   75 70 70 6F 72 74 65 64 20 6F 72 20 68 6F 6E 6F   // upported or hono
                                                                                                                                                   72 65 64 20 62 79 20 74 68 65 20 72 75 6E 74 69   // red by the runti
                                                                                                                                                   6D 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F 73 74   // me...T..Diagnost
                                                                                                                                                   69 63 49 64 0A 53 59 53 4C 49 42 30 30 30 33 54   // icId.SYSLIB0003T
                                                                                                                                                   0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74 74 70   // ..UrlFormat"http
                                                                                                                                                   73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65   // s://aka.ms/dotne
                                                                                                                                                   74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D )       // t-warnings/{0}
    // Method begins at RVA 0x2154
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::SetCompressedStack

  .method /*06000041*/ public hidebysig static 
          void  SetData(class System.LocalDataStoreSlot/*02000002*/ slot,
                        object data) cil managed
  // SIG: 00 02 01 12 08 1C
  {
    .param [2]/*08000025*/ 
    .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2156
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::SetData

  .method /*06000042*/ public hidebysig static 
          void  Sleep(int32 millisecondsTimeout) cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2158
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Sleep

  .method /*06000043*/ public hidebysig static 
          void  Sleep(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ timeout) cil managed
  // SIG: 00 01 01 11 80 99
  {
    // Method begins at RVA 0x215a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Sleep

  .method /*06000044*/ public hidebysig static 
          void  SpinWait(int32 iterations) cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x215c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::SpinWait

  .method /*06000045*/ public hidebysig instance void 
          Start() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C00003E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000018 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x215e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Start

  .method /*06000046*/ public hidebysig instance void 
          Start(object parameter) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00003F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000040:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000018 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2160
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Start

  .method /*06000047*/ public hidebysig instance void 
          Suspend() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000042:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A5 54 68 72 65 61 64 2E 53 75 73 70 65   // ....Thread.Suspe
                                                                                                                                                   6E 64 20 68 61 73 20 62 65 65 6E 20 64 65 70 72   // nd has been depr
                                                                                                                                                   65 63 61 74 65 64 2E 20 55 73 65 20 6F 74 68 65   // ecated. Use othe
                                                                                                                                                   72 20 63 6C 61 73 73 65 73 20 69 6E 20 53 79 73   // r classes in Sys
                                                                                                                                                   74 65 6D 2E 54 68 72 65 61 64 69 6E 67 2C 20 73   // tem.Threading, s
                                                                                                                                                   75 63 68 20 61 73 20 4D 6F 6E 69 74 6F 72 2C 20   // uch as Monitor, 
                                                                                                                                                   4D 75 74 65 78 2C 20 45 76 65 6E 74 2C 20 61 6E   // Mutex, Event, an
                                                                                                                                                   64 20 53 65 6D 61 70 68 6F 72 65 2C 20 74 6F 20   // d Semaphore, to 
                                                                                                                                                   73 79 6E 63 68 72 6F 6E 69 7A 65 20 54 68 72 65   // synchronize Thre
                                                                                                                                                   61 64 73 20 6F 72 20 70 72 6F 74 65 63 74 20 72   // ads or protect r
                                                                                                                                                   65 73 6F 75 72 63 65 73 2E 00 00 )                // esources...
    // Method begins at RVA 0x2162
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::Suspend

  .method /*06000048*/ public hidebysig instance bool 
          TrySetApartmentState(valuetype System.Threading.ApartmentState/*02000003*/ state) cil managed
  // SIG: 20 01 02 11 0C
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::TrySetApartmentState

  .method /*06000049*/ public hidebysig instance void 
          UnsafeStart() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000043:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000018 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2167
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::UnsafeStart

  .method /*0600004A*/ public hidebysig instance void 
          UnsafeStart(object parameter) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000044:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000045:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000018 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2169
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::UnsafeStart

  .method /*0600004B*/ public hidebysig static 
          uint8  VolatileRead(uint8& address) cil managed
  // SIG: 00 01 05 10 05
  {
    .custom /*0C000046:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000047:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x216b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*0600004C*/ public hidebysig static 
          float64  VolatileRead(float64& address) cil managed
  // SIG: 00 01 0D 10 0D
  {
    .custom /*0C000048:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000049:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x216e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*0600004D*/ public hidebysig static 
          int16  VolatileRead(int16& address) cil managed
  // SIG: 00 01 06 10 06
  {
    .custom /*0C00004A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00004B:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2171
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*0600004E*/ public hidebysig static 
          int32  VolatileRead(int32& address) cil managed
  // SIG: 00 01 08 10 08
  {
    .custom /*0C00004C:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00004D:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2174
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*0600004F*/ public hidebysig static 
          int64  VolatileRead(int64& address) cil managed
  // SIG: 00 01 0A 10 0A
  {
    .custom /*0C00004E:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00004F:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2177
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000050*/ public hidebysig static 
          native int  VolatileRead(native int& address) cil managed
  // SIG: 00 01 18 10 18
  {
    .custom /*0C000050:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000051:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x217a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000051*/ public hidebysig static 
          object  VolatileRead(object& address) cil managed
  // SIG: 00 01 1C 10 1C
  {
    .custom /*0C000052:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000053:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000054:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000032*/ 
    .custom /*0C000038:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000029*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 61 64 64 72 65 73 73 00 00 )             // ...address..
    .param [1]/*08000033*/ 
    .custom /*0C000039:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000029*/::.ctor(string) /* 0A000019 */ = ( 01 00 07 61 64 64 72 65 73 73 00 00 )             // ...address..
    // Method begins at RVA 0x217d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000052*/ public hidebysig static 
          int8  VolatileRead(int8& address) cil managed
  // SIG: 00 01 04 10 04
  {
    .custom /*0C000055:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000056:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000057:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2180
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000053*/ public hidebysig static 
          float32  VolatileRead(float32& address) cil managed
  // SIG: 00 01 0C 10 0C
  {
    .custom /*0C000058:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000059:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2183
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000054*/ public hidebysig static 
          uint16  VolatileRead(uint16& address) cil managed
  // SIG: 00 01 07 10 07
  {
    .custom /*0C00005A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00005B:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C00005C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000055*/ public hidebysig static 
          uint32  VolatileRead(uint32& address) cil managed
  // SIG: 00 01 09 10 09
  {
    .custom /*0C00005D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00005E:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C00005F:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2189
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000056*/ public hidebysig static 
          uint64  VolatileRead(uint64& address) cil managed
  // SIG: 00 01 0B 10 0B
  {
    .custom /*0C000060:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000061:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000062:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x218c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000057*/ public hidebysig static 
          native uint  VolatileRead(native uint& address) cil managed
  // SIG: 00 01 19 10 19
  {
    .custom /*0C000063:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000064:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000065:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::VolatileRead

  .method /*06000058*/ public hidebysig static 
          void  VolatileWrite(uint8& address,
                              uint8 'value') cil managed
  // SIG: 00 02 01 10 05 05
  {
    .custom /*0C000066:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000067:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2192
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*06000059*/ public hidebysig static 
          void  VolatileWrite(float64& address,
                              float64 'value') cil managed
  // SIG: 00 02 01 10 0D 0D
  {
    .custom /*0C000068:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000069:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2194
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*0600005A*/ public hidebysig static 
          void  VolatileWrite(int16& address,
                              int16 'value') cil managed
  // SIG: 00 02 01 10 06 06
  {
    .custom /*0C00006B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00006C:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2196
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*0600005B*/ public hidebysig static 
          void  VolatileWrite(int32& address,
                              int32 'value') cil managed
  // SIG: 00 02 01 10 08 08
  {
    .custom /*0C00006E:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00006F:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2198
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*0600005C*/ public hidebysig static 
          void  VolatileWrite(int64& address,
                              int64 'value') cil managed
  // SIG: 00 02 01 10 0A 0A
  {
    .custom /*0C000070:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000071:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x219a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*0600005D*/ public hidebysig static 
          void  VolatileWrite(native int& address,
                              native int 'value') cil managed
  // SIG: 00 02 01 10 18 18
  {
    .custom /*0C000072:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000073:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x219c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*0600005E*/ public hidebysig static 
          void  VolatileWrite(object& address,
                              object 'value') cil managed
  // SIG: 00 02 01 10 1C 1C
  {
    .custom /*0C000074:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000075:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000076:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000046*/ 
    .custom /*0C000041:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000029*/::.ctor(string) /* 0A000019 */ = ( 01 00 05 76 61 6C 75 65 00 00 )                   // ...value..
    // Method begins at RVA 0x219e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*0600005F*/ public hidebysig static 
          void  VolatileWrite(int8& address,
                              int8 'value') cil managed
  // SIG: 00 02 01 10 04 04
  {
    .custom /*0C000077:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000078:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000079:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*06000060*/ public hidebysig static 
          void  VolatileWrite(float32& address,
                              float32 'value') cil managed
  // SIG: 00 02 01 10 0C 0C
  {
    .custom /*0C00007A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00007B:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x21a2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*06000061*/ public hidebysig static 
          void  VolatileWrite(uint16& address,
                              uint16 'value') cil managed
  // SIG: 00 02 01 10 07 07
  {
    .custom /*0C00007C:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C00007D:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C00007E:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21a4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*06000062*/ public hidebysig static 
          void  VolatileWrite(uint32& address,
                              uint32 'value') cil managed
  // SIG: 00 02 01 10 09 09
  {
    .custom /*0C00007F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000080:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000081:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21a6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*06000063*/ public hidebysig static 
          void  VolatileWrite(uint64& address,
                              uint64 'value') cil managed
  // SIG: 00 02 01 10 0B 0B
  {
    .custom /*0C000082:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000083:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000084:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*06000064*/ public hidebysig static 
          void  VolatileWrite(native uint& address,
                              native uint 'value') cil managed
  // SIG: 00 02 01 10 19 19
  {
    .custom /*0C000085:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 74 54 68 72 65 61 64 2E 56 6F 6C 61 74 69   // ..tThread.Volati
                                                                                                                                                   6C 65 52 65 61 64 20 61 6E 64 20 54 68 72 65 61   // leRead and Threa
                                                                                                                                                   64 2E 56 6F 6C 61 74 69 6C 65 57 72 69 74 65 20   // d.VolatileWrite 
                                                                                                                                                   61 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73   // are obsolete. Us
                                                                                                                                                   65 20 56 6F 6C 61 74 69 6C 65 2E 52 65 61 64 20   // e Volatile.Read 
                                                                                                                                                   6F 72 20 56 6F 6C 61 74 69 6C 65 2E 57 72 69 74   // or Volatile.Writ
                                                                                                                                                   65 20 72 65 73 70 65 63 74 69 76 65 6C 79 20 69   // e respectively i
                                                                                                                                                   6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67   // nstead...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 35 34 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 054T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    .custom /*0C000086:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000087:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Thread::VolatileWrite

  .method /*06000065*/ public hidebysig static 
          bool  Yield() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Thread::Yield

  .property /*17000001*/ instance callconv(8) valuetype System.Threading.ApartmentState/*02000003*/
          ApartmentState()
  {
    .custom /*0C000004:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 7A 54 68 65 20 41 70 61 72 74 6D 65 6E 74   // ..zThe Apartment
                                                                                                                                                   53 74 61 74 65 20 70 72 6F 70 65 72 74 79 20 68   // State property h
                                                                                                                                                   61 73 20 62 65 65 6E 20 64 65 70 72 65 63 61 74   // as been deprecat
                                                                                                                                                   65 64 2E 20 55 73 65 20 47 65 74 41 70 61 72 74   // ed. Use GetApart
                                                                                                                                                   6D 65 6E 74 53 74 61 74 65 2C 20 53 65 74 41 70   // mentState, SetAp
                                                                                                                                                   61 72 74 6D 65 6E 74 53 74 61 74 65 20 6F 72 20   // artmentState or 
                                                                                                                                                   54 72 79 53 65 74 41 70 61 72 74 6D 65 6E 74 53   // TrySetApartmentS
                                                                                                                                                   74 61 74 65 20 69 6E 73 74 65 61 64 2E 00 00 )    // tate instead...
    .get instance valuetype System.Threading.ApartmentState/*02000003*/ System.Threading.Thread/*02000006*/::get_ApartmentState() /* 06000011 */
    .set instance void System.Threading.Thread/*02000006*/::set_ApartmentState(valuetype System.Threading.ApartmentState/*02000003*/) /* 06000012 */
  } // end of property Thread::ApartmentState
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/
          CurrentCulture()
  {
    .get instance class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/ System.Threading.Thread/*02000006*/::get_CurrentCulture() /* 06000013 */
    .set instance void System.Threading.Thread/*02000006*/::set_CurrentCulture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/) /* 06000014 */
  } // end of property Thread::CurrentCulture
  .property /*17000003*/ callconv(8) class [System.Runtime/*23000001*/]System.Security.Principal.IPrincipal/*01000023*/
          CurrentPrincipal()
  {
    .custom /*0C000018:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get class [System.Runtime/*23000001*/]System.Security.Principal.IPrincipal/*01000023*/ System.Threading.Thread/*02000006*/::get_CurrentPrincipal() /* 06000015 */
    .set void System.Threading.Thread/*02000006*/::set_CurrentPrincipal(class [System.Runtime/*23000001*/]System.Security.Principal.IPrincipal/*01000023*/) /* 06000016 */
  } // end of property Thread::CurrentPrincipal
  .property /*17000004*/ callconv(8) class System.Threading.Thread/*02000006*/
          CurrentThread()
  {
    .get class System.Threading.Thread/*02000006*/ System.Threading.Thread/*02000006*/::get_CurrentThread() /* 06000017 */
  } // end of property Thread::CurrentThread
  .property /*17000005*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/
          CurrentUICulture()
  {
    .get instance class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/ System.Threading.Thread/*02000006*/::get_CurrentUICulture() /* 06000018 */
    .set instance void System.Threading.Thread/*02000006*/::set_CurrentUICulture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000022*/) /* 06000019 */
  } // end of property Thread::CurrentUICulture
  .property /*17000006*/ instance callconv(8) class [System.Threading/*23000002*/]System.Threading.ExecutionContext/*01000024*/
          ExecutionContext()
  {
    .custom /*0C00001E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Threading/*23000002*/]System.Threading.ExecutionContext/*01000024*/ System.Threading.Thread/*02000006*/::get_ExecutionContext() /* 0600001A */
  } // end of property Thread::ExecutionContext
  .property /*17000007*/ instance callconv(8) bool
          IsAlive()
  {
    .get instance bool System.Threading.Thread/*02000006*/::get_IsAlive() /* 0600001B */
  } // end of property Thread::IsAlive
  .property /*17000008*/ instance callconv(8) bool
          IsBackground()
  {
    .get instance bool System.Threading.Thread/*02000006*/::get_IsBackground() /* 0600001C */
    .set instance void System.Threading.Thread/*02000006*/::set_IsBackground(bool) /* 0600001D */
  } // end of property Thread::IsBackground
  .property /*17000009*/ instance callconv(8) bool
          IsThreadPoolThread()
  {
    .get instance bool System.Threading.Thread/*02000006*/::get_IsThreadPoolThread() /* 0600001E */
  } // end of property Thread::IsThreadPoolThread
  .property /*1700000A*/ instance callconv(8) int32
          ManagedThreadId()
  {
    .get instance int32 System.Threading.Thread/*02000006*/::get_ManagedThreadId() /* 0600001F */
  } // end of property Thread::ManagedThreadId
  .property /*1700000B*/ instance callconv(8) string
          Name()
  {
    .custom /*0C000029:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Threading.Thread/*02000006*/::get_Name() /* 06000020 */
    .set instance void System.Threading.Thread/*02000006*/::set_Name(string) /* 06000021 */
  } // end of property Thread::Name
  .property /*1700000C*/ instance callconv(8) valuetype System.Threading.ThreadPriority/*0200000B*/
          Priority()
  {
    .get instance valuetype System.Threading.ThreadPriority/*0200000B*/ System.Threading.Thread/*02000006*/::get_Priority() /* 06000022 */
    .set instance void System.Threading.Thread/*02000006*/::set_Priority(valuetype System.Threading.ThreadPriority/*0200000B*/) /* 06000023 */
  } // end of property Thread::Priority
  .property /*1700000D*/ instance callconv(8) valuetype System.Threading.ThreadState/*0200000E*/
          ThreadState()
  {
    .get instance valuetype System.Threading.ThreadState/*0200000E*/ System.Threading.Thread/*02000006*/::get_ThreadState() /* 06000024 */
  } // end of property Thread::ThreadState
} // end of class System.Threading.Thread

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Threading.ThreadAbortException
       extends [System.Runtime/*23000001*/]System.SystemException/*0100002A*/
{
  .custom /*0C000021:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000022:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000066*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21af
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadAbortException::.ctor

  .method /*06000067*/ public hidebysig specialname 
          instance object  get_ExceptionState() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x21b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ThreadAbortException::get_ExceptionState

  .property /*1700000E*/ instance callconv(8) object
          ExceptionState()
  {
    .get instance object System.Threading.ThreadAbortException/*02000007*/::get_ExceptionState() /* 06000067 */
  } // end of property ThreadAbortException::ExceptionState
} // end of class System.Threading.ThreadAbortException

.class /*02000008*/ public auto ansi beforefieldinit System.Threading.ThreadExceptionEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*0100002B*/
{
  .custom /*0C000023:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000068*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*0100002C*/ t) cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x21ba
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*0100002B*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadExceptionEventArgs::.ctor

  .method /*06000069*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*0100002C*/ 
          get_Exception() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ThreadExceptionEventArgs::get_Exception

  .property /*1700000F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*0100002C*/
          Exception()
  {
    .get instance class [System.Runtime/*23000001*/]System.Exception/*0100002C*/ System.Threading.ThreadExceptionEventArgs/*02000008*/::get_Exception() /* 06000069 */
  } // end of property ThreadExceptionEventArgs::Exception
} // end of class System.Threading.ThreadExceptionEventArgs

.class /*02000009*/ public auto ansi sealed System.Threading.ThreadExceptionEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001E*/
{
  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ThreadExceptionEventHandler::.ctor

  .method /*0600006B*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class System.Threading.ThreadExceptionEventArgs/*02000008*/ e) runtime managed
  // SIG: 20 02 01 1C 12 20
  {
    .custom /*0C000088:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method ThreadExceptionEventHandler::Invoke

  .method /*0600006C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ 
          BeginInvoke(object sender,
                      class System.Threading.ThreadExceptionEventArgs/*02000008*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000020*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 7D 1C 12 20 12 80 81 1C
  {
    .param [1]/*08000059*/ 
    .custom /*0C00006A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800005A*/ 
    .custom /*0C00006D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method ThreadExceptionEventHandler::BeginInvoke

  .method /*0600006D*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ result) runtime managed
  // SIG: 20 01 01 12 7D
  {
  } // end of method ThreadExceptionEventHandler::EndInvoke

} // end of class System.Threading.ThreadExceptionEventHandler

.class /*0200000A*/ public auto ansi beforefieldinit System.Threading.ThreadInterruptedException
       extends [System.Runtime/*23000001*/]System.SystemException/*0100002A*/
{
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000028:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600006E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21c5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadInterruptedException::.ctor

  .method /*0600006F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C000089:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00008A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    .custom /*0C00008B:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x21cd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadInterruptedException::.ctor

  .method /*06000070*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21d5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadInterruptedException::.ctor

  .method /*06000071*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100002C*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 B1
  {
    // Method begins at RVA 0x21dd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadInterruptedException::.ctor

} // end of class System.Threading.ThreadInterruptedException

.class /*0200000B*/ public auto ansi sealed System.Threading.ThreadPriority
       extends [System.Runtime/*23000001*/]System.Enum/*01000014*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Threading.ThreadPriority/*0200000B*/ Lowest = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Threading.ThreadPriority/*0200000B*/ BelowNormal = int32(0x00000001)
  .field /*04000008*/ public static literal valuetype System.Threading.ThreadPriority/*0200000B*/ Normal = int32(0x00000002)
  .field /*04000009*/ public static literal valuetype System.Threading.ThreadPriority/*0200000B*/ AboveNormal = int32(0x00000003)
  .field /*0400000A*/ public static literal valuetype System.Threading.ThreadPriority/*0200000B*/ Highest = int32(0x00000004)
} // end of class System.Threading.ThreadPriority

.class /*0200000C*/ public auto ansi sealed System.Threading.ThreadStart
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001E*/
{
  .method /*06000072*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ThreadStart::.ctor

  .method /*06000073*/ public hidebysig newslot virtual 
          instance void  Invoke() runtime managed
  // SIG: 20 00 01
  {
  } // end of method ThreadStart::Invoke

  .method /*06000074*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ 
          BeginInvoke(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000020*/ callback,
                      object 'object') runtime managed
  // SIG: 20 02 12 7D 12 80 81 1C
  {
  } // end of method ThreadStart::BeginInvoke

  .method /*06000075*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001F*/ result) runtime managed
  // SIG: 20 01 01 12 7D
  {
  } // end of method ThreadStart::EndInvoke

} // end of class System.Threading.ThreadStart

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.Threading.ThreadStartException
       extends [System.Runtime/*23000001*/]System.SystemException/*0100002A*/
{
  .method /*06000076*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadStartException::.ctor

} // end of class System.Threading.ThreadStartException

.class /*0200000E*/ public auto ansi sealed System.Threading.ThreadState
       extends [System.Runtime/*23000001*/]System.Enum/*01000014*/
{
  .custom /*0C00002A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100002D*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*0400000B*/ public specialname rtspecialname int32 value__
  .field /*0400000C*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ Running = int32(0x00000000)
  .field /*0400000D*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ StopRequested = int32(0x00000001)
  .field /*0400000E*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ SuspendRequested = int32(0x00000002)
  .field /*0400000F*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ Background = int32(0x00000004)
  .field /*04000010*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ Unstarted = int32(0x00000008)
  .field /*04000011*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ Stopped = int32(0x00000010)
  .field /*04000012*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ WaitSleepJoin = int32(0x00000020)
  .field /*04000013*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ Suspended = int32(0x00000040)
  .field /*04000014*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ AbortRequested = int32(0x00000080)
  .field /*04000015*/ public static literal valuetype System.Threading.ThreadState/*0200000E*/ Aborted = int32(0x00000100)
} // end of class System.Threading.ThreadState

.class /*0200000F*/ public auto ansi beforefieldinit System.Threading.ThreadStateException
       extends [System.Runtime/*23000001*/]System.SystemException/*0100002A*/
{
  .custom /*0C00002B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ed
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadStateException::.ctor

  .method /*06000078*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001B*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001C*/ context) cil managed
  // SIG: 20 02 01 12 6D 11 71
  {
    .custom /*0C00008C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00008D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000018*/::.ctor(string) /* 0A000015 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    .custom /*0C00008E:0A000016*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000019*/) /* 0A000016 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x21f5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadStateException::.ctor

  .method /*06000079*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21fd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadStateException::.ctor

  .method /*0600007A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100002C*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 B1
  {
    // Method begins at RVA 0x2205
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*0100002A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadStateException::.ctor

} // end of class System.Threading.ThreadStateException


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
