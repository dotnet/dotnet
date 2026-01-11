


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
// Time-date stamp:            0x988ea740
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002200
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003ffe
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
// Checksum:                       0x00009382
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003fa9 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x000004f4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002c00 [0x00002480] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003ea8 [0x00000054] address [size] of Debug Directory:           
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
//              0x00002004 Virtual Size
//              0x00002000 Virtual Address
//              0x00002200 Size of Raw Data
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
//              0x000004f4 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002400 Pointer to Raw Data
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
//              0x00002a00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000000
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003feb Import Name Table
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
// 0x000021bc [0x00001c6c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003e28 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000c74 Size
//              '#~' Name
//    Stream 2:
//              0x00000ce0 Offset
//              0x00000a20 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001700 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001704 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001714 Offset
//              0x00000558 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901a01d57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20608
// PE header size       : 512 (496 used)    ( 2.48%)
// PE additional info   : 10867             (52.73%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.35%)
// CLR meta-data size  : 7276               (35.31%)
// CLR additional info : 128                ( 0.62%)
// CLR method headers  : 66                 ( 0.32%)
// Managed code         : 297               ( 1.44%)
// Data                 : 2048              ( 9.94%)
// Unaccounted          : -658              (-3.19%)

// Num.of PE sections   : 3
//   .text    - 8704
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 7276
//   Module        -    1 (10 bytes)
//   TypeDef       -   13 (182 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   50 (300 bytes)
//   MethodDef     -   66 (924 bytes)     0 abstract, 0 native, 66 bodies
//   FieldDef      -   27 (162 bytes)     0 constant
//   MemberRef     -   25 (150 bytes)
//   ParamDef      -  123 (738 bytes)
//   Constant      -   23 (138 bytes)
//   CustomAttribute-   58 (348 bytes)
//   PropertyMap   -    3 (12 bytes)
//   Property      -    5 (30 bytes)
//   MethodSemantic-    5 (30 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   Strings       -  2592 bytes
//   Blobs         -  1368 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   190 bytes

// CLR additional info : 128

// CLR method headers : 66
//   Num.of method bodies  - 66
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 66

// Managed code : 297
//   Ave method size - 4
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class DirectoryObjectSecurity        (public) (abstract) (auto) (ansi) 
// Class DirectorySecurity              (public) (auto) (ansi) (sealed) 
// Class FileSecurity                   (public) (auto) (ansi) (sealed) 
// Class FileSystemAccessRule           (public) (auto) (ansi) (sealed) 
// Class FileSystemAuditRule            (public) (auto) (ansi) (sealed) 
// Class FileSystemRights               (public) (auto) (ansi) (sealed) 
// Class FileSystemSecurity             (public) (abstract) (auto) (ansi) 
// Class FileSystemAclExtensions        (public) (abstract) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Security.AccessControl
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Security.Principal.Windows
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.IO.FileSystem.AccessControl
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000002:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000003:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000004:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000005:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000006:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000007:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000007*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000008*/::.ctor(bool) /* 0A000007 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000009:0A000008*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .custom /*0C00000A:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 22 53 79 73 74 65 6D 2E 49 4F 2E 46 69 6C   // .."System.IO.Fil
                                                                                                                                                                        65 53 79 73 74 65 6D 2E 41 63 63 65 73 73 43 6F   // eSystem.AccessCo
                                                                                                                                                                        6E 74 72 6F 6C 00 00 )                            // ntrol..
  .custom /*0C00000B:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A00000A */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000C:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A00000A */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A00000A */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A00000A */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 22 53 79 73 74 65 6D 2E 49 4F 2E 46 69 6C   // .."System.IO.Fil
                                                                                                                                                                       65 53 79 73 74 65 6D 2E 41 63 63 65 73 73 43 6F   // eSystem.AccessCo
                                                                                                                                                                       6E 74 72 6F 6C 00 00 )                            // ntrol..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A000011*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 22 53 79 73 74 65 6D 2E 49 4F 2E 46 69 6C   // .."System.IO.Fil
                                                                                                                                                                 65 53 79 73 74 65 6D 2E 41 63 63 65 73 73 43 6F   // eSystem.AccessCo
                                                                                                                                                                 6E 74 72 6F 6C 00 00 )                            // ntrol..
  .custom /*0C000016:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A00000A */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 6:0:0:0
}
.module System.IO.FileSystem.AccessControl.dll
// MVID: {1886ee6b-3dd6-4c90-a8b9-00cca7fa2ada}
.custom /*0C000001:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000017:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000014*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000014*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000031       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000031*/
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000014*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000014*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000014*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public abstract auto ansi beforefieldinit System.Security.AccessControl.DirectoryObjectSecurity
       extends [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectSecurity/*01000017*/
{
  .custom /*0C000022:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000023:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectSecurity/*01000017*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::.ctor

  .method /*06000007*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.CommonSecurityDescriptor/*01000018*/ securityDescriptor) cil managed
  // SIG: 20 01 01 12 61
  {
    // Method begins at RVA 0x20a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectSecurity/*01000017*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::.ctor

  .method /*06000008*/ public hidebysig newslot virtual 
          instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/ 
          AccessRuleFactory(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identityReference,
                            int32 accessMask,
                            bool isInherited,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type',
                            valuetype [System.Runtime/*23000001*/]System.Guid/*0100001E*/ objectType,
                            valuetype [System.Runtime/*23000001*/]System.Guid/*0100001E*/ inheritedObjectType) cil managed
  // SIG: 20 08 12 65 12 69 08 02 11 6D 11 71 11 75 11 79 11 79
  {
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::AccessRuleFactory

  .method /*06000009*/ family hidebysig instance void 
          AddAccessRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x20b0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::AddAccessRule

  .method /*0600000A*/ family hidebysig instance void 
          AddAuditRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x20b2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::AddAuditRule

  .method /*0600000B*/ public hidebysig newslot virtual 
          instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/ 
          AuditRuleFactory(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identityReference,
                           int32 accessMask,
                           bool isInherited,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags',
                           valuetype [System.Runtime/*23000001*/]System.Guid/*0100001E*/ objectType,
                           valuetype [System.Runtime/*23000001*/]System.Guid/*0100001E*/ inheritedObjectType) cil managed
  // SIG: 20 08 12 80 85 12 69 08 02 11 6D 11 71 11 80 89 11 79 11 79
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::AuditRuleFactory

  .method /*0600000C*/ public hidebysig instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuthorizationRuleCollection/*01000023*/ 
          GetAccessRules(bool includeExplicit,
                         bool includeInherited,
                         class [System.Runtime/*23000001*/]System.Type/*01000024*/ targetType) cil managed
  // SIG: 20 03 12 80 8D 02 02 12 80 91
  {
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::GetAccessRules

  .method /*0600000D*/ public hidebysig instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuthorizationRuleCollection/*01000023*/ 
          GetAuditRules(bool includeExplicit,
                        bool includeInherited,
                        class [System.Runtime/*23000001*/]System.Type/*01000024*/ targetType) cil managed
  // SIG: 20 03 12 80 8D 02 02 12 80 91
  {
    // Method begins at RVA 0x20ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::GetAuditRules

  .method /*0600000E*/ family hidebysig virtual 
          instance bool  ModifyAccess(valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlModification/*01000025*/ modification,
                                      class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/ rule,
                                      [out] bool& modified) cil managed
  // SIG: 20 03 02 11 80 95 12 65 10 02
  {
    // Method begins at RVA 0x20bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::ModifyAccess

  .method /*0600000F*/ family hidebysig virtual 
          instance bool  ModifyAudit(valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlModification/*01000025*/ modification,
                                     class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/ rule,
                                     [out] bool& modified) cil managed
  // SIG: 20 03 02 11 80 95 12 80 85 10 02
  {
    // Method begins at RVA 0x20c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::ModifyAudit

  .method /*06000010*/ family hidebysig instance bool 
          RemoveAccessRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 02 12 7D
  {
    // Method begins at RVA 0x20c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::RemoveAccessRule

  .method /*06000011*/ family hidebysig instance void 
          RemoveAccessRuleAll(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x20c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::RemoveAccessRuleAll

  .method /*06000012*/ family hidebysig instance void 
          RemoveAccessRuleSpecific(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x20c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::RemoveAccessRuleSpecific

  .method /*06000013*/ family hidebysig instance bool 
          RemoveAuditRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 02 12 80 81
  {
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::RemoveAuditRule

  .method /*06000014*/ family hidebysig instance void 
          RemoveAuditRuleAll(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x20cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::RemoveAuditRuleAll

  .method /*06000015*/ family hidebysig instance void 
          RemoveAuditRuleSpecific(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x20cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::RemoveAuditRuleSpecific

  .method /*06000016*/ family hidebysig instance void 
          ResetAccessRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x20d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::ResetAccessRule

  .method /*06000017*/ family hidebysig instance void 
          SetAccessRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x20d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::SetAccessRule

  .method /*06000018*/ family hidebysig instance void 
          SetAuditRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x20d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::SetAuditRule

} // end of class System.Security.AccessControl.DirectoryObjectSecurity

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Security.AccessControl.DirectorySecurity
       extends System.Security.AccessControl.FileSystemSecurity/*0200000C*/
{
  .method /*06000019*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000027       */ call       instance void System.Security.AccessControl.FileSystemSecurity/*0200000C*/::.ctor() /* 06000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DirectorySecurity::.ctor

  .method /*0600001A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlSections/*01000026*/ includeSections) cil managed
  // SIG: 20 02 01 0E 11 80 99
  {
    .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20df
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000027       */ call       instance void System.Security.AccessControl.FileSystemSecurity/*0200000C*/::.ctor() /* 06000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DirectorySecurity::.ctor

} // end of class System.Security.AccessControl.DirectorySecurity

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Security.AccessControl.FileSecurity
       extends System.Security.AccessControl.FileSystemSecurity/*0200000C*/
{
  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000027       */ call       instance void System.Security.AccessControl.FileSystemSecurity/*0200000C*/::.ctor() /* 06000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FileSecurity::.ctor

  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlSections/*01000026*/ includeSections) cil managed
  // SIG: 20 02 01 0E 11 80 99
  {
    .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000027       */ call       instance void System.Security.AccessControl.FileSystemSecurity/*0200000C*/::.ctor() /* 06000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FileSecurity::.ctor

} // end of class System.Security.AccessControl.FileSecurity

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Security.AccessControl.FileSystemAccessRule
       extends [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/
{
  .custom /*0C000024:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 03 01 12 69 11 2C 11 75
  {
    // Method begins at RVA 0x20f7
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000017       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                            int32,
                                                                                                                                                                            bool,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/) /* 0A000017 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAccessRule::.ctor

  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 05 01 12 69 11 2C 11 6D 11 71 11 75
  {
    // Method begins at RVA 0x2105
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000017       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                            int32,
                                                                                                                                                                            bool,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/) /* 0A000017 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAccessRule::.ctor

  .method /*0600001F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 03 01 0E 11 2C 11 75
  {
    // Method begins at RVA 0x2113
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000017       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                            int32,
                                                                                                                                                                            bool,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/) /* 0A000017 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAccessRule::.ctor

  .method /*06000020*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 05 01 0E 11 2C 11 6D 11 71 11 75
  {
    // Method begins at RVA 0x2121
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000017       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                            int32,
                                                                                                                                                                            bool,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/) /* 0A000017 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAccessRule::.ctor

  .method /*06000021*/ public hidebysig specialname 
          instance valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ 
          get_FileSystemRights() cil managed
  // SIG: 20 00 11 2C
  {
    // Method begins at RVA 0x212f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAccessRule::get_FileSystemRights

  .property /*17000001*/ instance callconv(8) valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/
          FileSystemRights()
  {
    .get instance valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ System.Security.AccessControl.FileSystemAccessRule/*02000009*/::get_FileSystemRights() /* 06000021 */
  } // end of property FileSystemAccessRule::FileSystemRights
} // end of class System.Security.AccessControl.FileSystemAccessRule

.class /*0200000A*/ public auto ansi sealed beforefieldinit System.Security.AccessControl.FileSystemAuditRule
       extends [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/
{
  .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 03 01 12 69 11 2C 11 80 89
  {
    // Method begins at RVA 0x2132
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000018       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                           int32,
                                                                                                                                                                           bool,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/) /* 0A000018 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAuditRule::.ctor

  .method /*06000023*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 05 01 12 69 11 2C 11 6D 11 71 11 80 89
  {
    // Method begins at RVA 0x2140
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000018       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                           int32,
                                                                                                                                                                           bool,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/) /* 0A000018 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAuditRule::.ctor

  .method /*06000024*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 03 01 0E 11 2C 11 80 89
  {
    // Method begins at RVA 0x214e
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000018       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                           int32,
                                                                                                                                                                           bool,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/) /* 0A000018 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAuditRule::.ctor

  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 05 01 0E 11 2C 11 6D 11 71 11 80 89
  {
    // Method begins at RVA 0x215c
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000018       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                           int32,
                                                                                                                                                                           bool,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/) /* 0A000018 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAuditRule::.ctor

  .method /*06000026*/ public hidebysig specialname 
          instance valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ 
          get_FileSystemRights() cil managed
  // SIG: 20 00 11 2C
  {
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAuditRule::get_FileSystemRights

  .property /*17000002*/ instance callconv(8) valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/
          FileSystemRights()
  {
    .get instance valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ System.Security.AccessControl.FileSystemAuditRule/*0200000A*/::get_FileSystemRights() /* 06000026 */
  } // end of property FileSystemAuditRule::FileSystemRights
} // end of class System.Security.AccessControl.FileSystemAuditRule

.class /*0200000B*/ public auto ansi sealed System.Security.AccessControl.FileSystemRights
       extends [System.Runtime/*23000001*/]System.Enum/*01000028*/
{
  .custom /*0C000028:0A000014*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000027*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ ListDirectory = int32(0x00000001)
  .field /*04000006*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ ReadData = int32(0x00000001)
  .field /*04000007*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ CreateFiles = int32(0x00000002)
  .field /*04000008*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ WriteData = int32(0x00000002)
  .field /*04000009*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ AppendData = int32(0x00000004)
  .field /*0400000A*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ CreateDirectories = int32(0x00000004)
  .field /*0400000B*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ ReadExtendedAttributes = int32(0x00000008)
  .field /*0400000C*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ WriteExtendedAttributes = int32(0x00000010)
  .field /*0400000D*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ ExecuteFile = int32(0x00000020)
  .field /*0400000E*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ Traverse = int32(0x00000020)
  .field /*0400000F*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ DeleteSubdirectoriesAndFiles = int32(0x00000040)
  .field /*04000010*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ ReadAttributes = int32(0x00000080)
  .field /*04000011*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ WriteAttributes = int32(0x00000100)
  .field /*04000012*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ Write = int32(0x00000116)
  .field /*04000013*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ Delete = int32(0x00010000)
  .field /*04000014*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ ReadPermissions = int32(0x00020000)
  .field /*04000015*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ Read = int32(0x00020089)
  .field /*04000016*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ ReadAndExecute = int32(0x000200A9)
  .field /*04000017*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ Modify = int32(0x000301BF)
  .field /*04000018*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ ChangePermissions = int32(0x00040000)
  .field /*04000019*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ TakeOwnership = int32(0x00080000)
  .field /*0400001A*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ Synchronize = int32(0x00100000)
  .field /*0400001B*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ FullControl = int32(0x001F01FF)
} // end of class System.Security.AccessControl.FileSystemRights

.class /*0200000C*/ public abstract auto ansi beforefieldinit System.Security.AccessControl.FileSystemSecurity
       extends [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.NativeObjectSecurity/*01000029*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000027*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x216d
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (0A)000019       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.NativeObjectSecurity/*01000029*/::.ctor(bool,
                                                                                                                                                                                      valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ResourceType/*01000032*/) /* 0A000019 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::.ctor

  .method /*06000028*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ 
          get_AccessRightType() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x2177
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::get_AccessRightType

  .method /*06000029*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ 
          get_AccessRuleType() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x217a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::get_AccessRuleType

  .method /*0600002A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ 
          get_AuditRuleType() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x217d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::get_AuditRuleType

  .method /*0600002B*/ public hidebysig virtual final 
          instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/ 
          AccessRuleFactory(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identityReference,
                            int32 accessMask,
                            bool isInherited,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 06 12 65 12 69 08 02 11 6D 11 71 11 75
  {
    // Method begins at RVA 0x2180
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::AccessRuleFactory

  .method /*0600002C*/ public hidebysig instance void 
          AddAccessRule(class System.Security.AccessControl.FileSystemAccessRule/*02000009*/ rule) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x2183
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::AddAccessRule

  .method /*0600002D*/ public hidebysig instance void 
          AddAuditRule(class System.Security.AccessControl.FileSystemAuditRule/*0200000A*/ rule) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x2185
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::AddAuditRule

  .method /*0600002E*/ public hidebysig virtual final 
          instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/ 
          AuditRuleFactory(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identityReference,
                           int32 accessMask,
                           bool isInherited,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 06 12 80 85 12 69 08 02 11 6D 11 71 11 80 89
  {
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::AuditRuleFactory

  .method /*0600002F*/ public hidebysig instance bool 
          RemoveAccessRule(class System.Security.AccessControl.FileSystemAccessRule/*02000009*/ rule) cil managed
  // SIG: 20 01 02 12 24
  {
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::RemoveAccessRule

  .method /*06000030*/ public hidebysig instance void 
          RemoveAccessRuleAll(class System.Security.AccessControl.FileSystemAccessRule/*02000009*/ rule) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x218d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::RemoveAccessRuleAll

  .method /*06000031*/ public hidebysig instance void 
          RemoveAccessRuleSpecific(class System.Security.AccessControl.FileSystemAccessRule/*02000009*/ rule) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x218f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::RemoveAccessRuleSpecific

  .method /*06000032*/ public hidebysig instance bool 
          RemoveAuditRule(class System.Security.AccessControl.FileSystemAuditRule/*0200000A*/ rule) cil managed
  // SIG: 20 01 02 12 28
  {
    // Method begins at RVA 0x2191
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::RemoveAuditRule

  .method /*06000033*/ public hidebysig instance void 
          RemoveAuditRuleAll(class System.Security.AccessControl.FileSystemAuditRule/*0200000A*/ rule) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x2194
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::RemoveAuditRuleAll

  .method /*06000034*/ public hidebysig instance void 
          RemoveAuditRuleSpecific(class System.Security.AccessControl.FileSystemAuditRule/*0200000A*/ rule) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x2196
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::RemoveAuditRuleSpecific

  .method /*06000035*/ public hidebysig instance void 
          ResetAccessRule(class System.Security.AccessControl.FileSystemAccessRule/*02000009*/ rule) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x2198
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::ResetAccessRule

  .method /*06000036*/ public hidebysig instance void 
          SetAccessRule(class System.Security.AccessControl.FileSystemAccessRule/*02000009*/ rule) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x219a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::SetAccessRule

  .method /*06000037*/ public hidebysig instance void 
          SetAuditRule(class System.Security.AccessControl.FileSystemAuditRule/*0200000A*/ rule) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x219c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::SetAuditRule

  .property /*17000003*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000024*/
          AccessRightType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ System.Security.AccessControl.FileSystemSecurity/*0200000C*/::get_AccessRightType() /* 06000028 */
  } // end of property FileSystemSecurity::AccessRightType
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000024*/
          AccessRuleType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ System.Security.AccessControl.FileSystemSecurity/*0200000C*/::get_AccessRuleType() /* 06000029 */
  } // end of property FileSystemSecurity::AccessRuleType
  .property /*17000005*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000024*/
          AuditRuleType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ System.Security.AccessControl.FileSystemSecurity/*0200000C*/::get_AuditRuleType() /* 0600002A */
  } // end of property FileSystemSecurity::AuditRuleType
} // end of class System.Security.AccessControl.FileSystemSecurity

.class /*0200000D*/ public abstract auto ansi sealed beforefieldinit System.IO.FileSystemAclExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100002A*/
{
  .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000038*/ public hidebysig static 
          void  Create(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ directoryInfo,
                       class System.Security.AccessControl.DirectorySecurity/*02000007*/ directorySecurity) cil managed
  // SIG: 00 02 01 12 80 AD 12 1C
  {
    .custom /*0C000030:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x219e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemAclExtensions::Create

  .method /*06000039*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IO.FileStream/*0100002C*/ 
          Create(class [System.Runtime/*23000001*/]System.IO.FileInfo/*0100002D*/ fileInfo,
                 valuetype [System.Runtime/*23000001*/]System.IO.FileMode/*0100002E*/ mode,
                 valuetype System.Security.AccessControl.FileSystemRights/*0200000B*/ rights,
                 valuetype [System.Runtime/*23000001*/]System.IO.FileShare/*0100002F*/ share,
                 int32 bufferSize,
                 valuetype [System.Runtime/*23000001*/]System.IO.FileOptions/*01000030*/ options,
                 class System.Security.AccessControl.FileSecurity/*02000008*/ fileSecurity) cil managed
  // SIG: 00 07 12 80 B1 12 80 B5 11 80 B9 11 2C 11 80 BD 08 11 80 C1 12 20
  {
    .custom /*0C000031:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::Create

  .method /*0600003A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ 
          CreateDirectory(class System.Security.AccessControl.DirectorySecurity/*02000007*/ directorySecurity,
                          string path) cil managed
  // SIG: 00 02 12 80 AD 12 1C 0E
  {
    .custom /*0C000032:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::CreateDirectory

  .method /*0600003B*/ public hidebysig static 
          class System.Security.AccessControl.DirectorySecurity/*02000007*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ directoryInfo) cil managed
  // SIG: 00 01 12 1C 12 80 AD
  {
    .custom /*0C000033:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*0600003C*/ public hidebysig static 
          class System.Security.AccessControl.DirectorySecurity/*02000007*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ directoryInfo,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlSections/*01000026*/ includeSections) cil managed
  // SIG: 00 02 12 1C 12 80 AD 11 80 99
  {
    .custom /*0C000034:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*0600003D*/ public hidebysig static 
          class System.Security.AccessControl.FileSecurity/*02000008*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileInfo/*0100002D*/ fileInfo) cil managed
  // SIG: 00 01 12 20 12 80 B5
  {
    .custom /*0C000035:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*0600003E*/ public hidebysig static 
          class System.Security.AccessControl.FileSecurity/*02000008*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileInfo/*0100002D*/ fileInfo,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlSections/*01000026*/ includeSections) cil managed
  // SIG: 00 02 12 20 12 80 B5 11 80 99
  {
    .custom /*0C000036:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*0600003F*/ public hidebysig static 
          class System.Security.AccessControl.FileSecurity/*02000008*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileStream/*0100002C*/ fileStream) cil managed
  // SIG: 00 01 12 20 12 80 B1
  {
    .custom /*0C000037:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*06000040*/ public hidebysig static 
          void  SetAccessControl(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ directoryInfo,
                                 class System.Security.AccessControl.DirectorySecurity/*02000007*/ directorySecurity) cil managed
  // SIG: 00 02 01 12 80 AD 12 1C
  {
    .custom /*0C000038:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemAclExtensions::SetAccessControl

  .method /*06000041*/ public hidebysig static 
          void  SetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileInfo/*0100002D*/ fileInfo,
                                 class System.Security.AccessControl.FileSecurity/*02000008*/ fileSecurity) cil managed
  // SIG: 00 02 01 12 80 B5 12 20
  {
    .custom /*0C000039:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemAclExtensions::SetAccessControl

  .method /*06000042*/ public hidebysig static 
          void  SetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileStream/*0100002C*/ fileStream,
                                 class System.Security.AccessControl.FileSecurity/*02000008*/ fileSecurity) cil managed
  // SIG: 00 02 01 12 80 B1 12 20
  {
    .custom /*0C00003A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemAclExtensions::SetAccessControl

} // end of class System.IO.FileSystemAclExtensions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
