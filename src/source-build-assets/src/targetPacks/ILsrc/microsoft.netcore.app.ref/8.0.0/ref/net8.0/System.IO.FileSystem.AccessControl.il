


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
// Time-date stamp:            0xc7bcda29
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001e00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003cd6
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
// Checksum:                       0x00007d5a
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003c84 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004f4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002800 [0x00002920] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003c68 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001cdc Virtual Size
//              0x00002000 Virtual Address
//              0x00001e00 Size of Raw Data
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
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002000 Pointer to Raw Data
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
//              0x00002600 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000cd8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003cc6 Import Name Table
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
// 0x00002170 [0x00001a78] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003be8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000ba4 Size
//              '#~' Name
//    Stream 2:
//              0x00000c10 Offset
//              0x000009a4 Size
//              '#Strings' Name
//    Stream 3:
//              0x000015b4 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000015b8 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000015c8 Offset
//              0x000004b0 Size
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

// File size            : 20768
// PE header size       : 512 (496 used)    ( 2.47%)
// PE additional info   : 11995             (57.76%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.35%)
// CLR meta-data size  : 6776               (32.63%)
// CLR additional info : 128                ( 0.62%)
// CLR method headers  : 61                 ( 0.29%)
// Managed code         : 225               ( 1.08%)
// Data                 : 2048              ( 9.86%)
// Unaccounted          : -1049             (-5.05%)

// Num.of PE sections   : 3
//   .text    - 7680
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6776
//   Module        -    1 (10 bytes)
//   TypeDef       -    9 (126 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   49 (294 bytes)
//   MethodDef     -   61 (854 bytes)     0 abstract, 0 native, 61 bodies
//   FieldDef      -   24 (144 bytes)     0 constant
//   MemberRef     -   26 (156 bytes)
//   ParamDef      -  123 (738 bytes)
//   Constant      -   23 (138 bytes)
//   CustomAttribute-   47 (282 bytes)
//   PropertyMap   -    3 (12 bytes)
//   Property      -    5 (30 bytes)
//   MethodSemantic-    5 (30 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   Strings       -  2465 bytes
//   Blobs         -  1200 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   195 bytes

// CLR additional info : 128

// CLR method headers : 61
//   Num.of method bodies  - 61
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 61

// Managed code : 225
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
  .ver 8:0:0:0
}
.assembly extern /*23000002*/ System.Security.AccessControl
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000003*/ System.Security.Principal.Windows
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly /*20000001*/ noplatform System.IO.FileSystem.AccessControl
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000003:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000004:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 38 2E 30 01 00 54 0E 14   // ersion=v8.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 38 2E 30 )          // Name..NET 8.0
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 22 53 79 73 74 65 6D 2E 49 4F 2E 46 69 6C   // .."System.IO.Fil
                                                                                                                                                                        65 53 79 73 74 65 6D 2E 41 63 63 65 73 73 43 6F   // eSystem.AccessCo
                                                                                                                                                                        6E 74 72 6F 6C 00 00 )                            // ntrol..
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .custom /*0C00000E:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 22 53 79 73 74 65 6D 2E 49 4F 2E 46 69 6C   // .."System.IO.Fil
                                                                                                                                                                       65 53 79 73 74 65 6D 2E 41 63 63 65 73 73 43 6F   // eSystem.AccessCo
                                                                                                                                                                       6E 74 72 6F 6C 00 00 )                            // ntrol..
  .custom /*0C000011:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 0C 38 2E 30 2E 32 33 2E 35 33 31 30 33 00   // ...8.0.23.53103.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 2E 38 2E 30 2E 30 2B 35 35 33 35 65 33 31   // ...8.0.0+5535e31
                                                                                                                                                                                61 37 31 32 33 34 33 61 36 33 66 35 64 37 64 37   // a712343a63f5d7d7
                                                                                                                                                                                39 36 63 64 38 37 34 65 35 36 33 65 35 61 63 31   // 96cd874e563e5ac1
                                                                                                                                                                                34 00 00 )                                        // 4..
  .custom /*0C000013:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000011*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 22 53 79 73 74 65 6D 2E 49 4F 2E 46 69 6C   // .."System.IO.Fil
                                                                                                                                                                 65 53 79 73 74 65 6D 2E 41 63 63 65 73 73 43 6F   // eSystem.AccessCo
                                                                                                                                                                 6E 74 72 6F 6C 00 00 )                            // ntrol..
  .custom /*0C000015:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 8:0:0:0
}
.module System.IO.FileSystem.AccessControl.dll
// MVID: {9e143589-7195-4d43-9c03-e773b0f2073b}
.custom /*0C000001:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000013*/::.ctor(int32) /* 0A000012 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000014*/::.ctor(bool) /* 0A000013 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi beforefieldinit System.Security.AccessControl.DirectoryObjectSecurity
       extends [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectSecurity/*01000017*/
{
  .custom /*0C000016:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000017:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectSecurity/*01000017*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::.ctor

  .method /*06000002*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.CommonSecurityDescriptor/*01000018*/ securityDescriptor) cil managed
  // SIG: 20 01 01 12 61
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectSecurity/*01000017*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::.ctor

  .method /*06000003*/ public hidebysig newslot virtual 
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
    // Method begins at RVA 0x2060
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::AccessRuleFactory

  .method /*06000004*/ family hidebysig instance void 
          AddAccessRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x2063
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::AddAccessRule

  .method /*06000005*/ family hidebysig instance void 
          AddAuditRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x2065
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::AddAuditRule

  .method /*06000006*/ public hidebysig newslot virtual 
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
    // Method begins at RVA 0x2067
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::AuditRuleFactory

  .method /*06000007*/ public hidebysig instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuthorizationRuleCollection/*01000023*/ 
          GetAccessRules(bool includeExplicit,
                         bool includeInherited,
                         class [System.Runtime/*23000001*/]System.Type/*01000024*/ targetType) cil managed
  // SIG: 20 03 12 80 8D 02 02 12 80 91
  {
    // Method begins at RVA 0x206a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::GetAccessRules

  .method /*06000008*/ public hidebysig instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuthorizationRuleCollection/*01000023*/ 
          GetAuditRules(bool includeExplicit,
                        bool includeInherited,
                        class [System.Runtime/*23000001*/]System.Type/*01000024*/ targetType) cil managed
  // SIG: 20 03 12 80 8D 02 02 12 80 91
  {
    // Method begins at RVA 0x206d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::GetAuditRules

  .method /*06000009*/ family hidebysig virtual 
          instance bool  ModifyAccess(valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlModification/*01000025*/ modification,
                                      class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/ rule,
                                      [out] bool& modified) cil managed
  // SIG: 20 03 02 11 80 95 12 65 10 02
  {
    // Method begins at RVA 0x2070
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::ModifyAccess

  .method /*0600000A*/ family hidebysig virtual 
          instance bool  ModifyAudit(valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlModification/*01000025*/ modification,
                                     class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/ rule,
                                     [out] bool& modified) cil managed
  // SIG: 20 03 02 11 80 95 12 80 85 10 02
  {
    // Method begins at RVA 0x2073
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::ModifyAudit

  .method /*0600000B*/ family hidebysig instance bool 
          RemoveAccessRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 02 12 7D
  {
    // Method begins at RVA 0x2076
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::RemoveAccessRule

  .method /*0600000C*/ family hidebysig instance void 
          RemoveAccessRuleAll(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x2079
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::RemoveAccessRuleAll

  .method /*0600000D*/ family hidebysig instance void 
          RemoveAccessRuleSpecific(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x207b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::RemoveAccessRuleSpecific

  .method /*0600000E*/ family hidebysig instance bool 
          RemoveAuditRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 02 12 80 81
  {
    // Method begins at RVA 0x207d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DirectoryObjectSecurity::RemoveAuditRule

  .method /*0600000F*/ family hidebysig instance void 
          RemoveAuditRuleAll(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x2080
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::RemoveAuditRuleAll

  .method /*06000010*/ family hidebysig instance void 
          RemoveAuditRuleSpecific(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x2082
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::RemoveAuditRuleSpecific

  .method /*06000011*/ family hidebysig instance void 
          ResetAccessRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x2084
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::ResetAccessRule

  .method /*06000012*/ family hidebysig instance void 
          SetAccessRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAccessRule/*0100001F*/ rule) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x2086
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::SetAccessRule

  .method /*06000013*/ family hidebysig instance void 
          SetAuditRule(class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ObjectAuditRule/*01000020*/ rule) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x2088
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DirectoryObjectSecurity::SetAuditRule

} // end of class System.Security.AccessControl.DirectoryObjectSecurity

.class /*02000003*/ public auto ansi sealed beforefieldinit System.Security.AccessControl.DirectorySecurity
       extends System.Security.AccessControl.FileSystemSecurity/*02000008*/
{
  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x208a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000022       */ call       instance void System.Security.AccessControl.FileSystemSecurity/*02000008*/::.ctor() /* 06000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DirectorySecurity::.ctor

  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlSections/*01000026*/ includeSections) cil managed
  // SIG: 20 02 01 0E 11 80 99
  {
    .custom /*0C000022:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2092
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000022       */ call       instance void System.Security.AccessControl.FileSystemSecurity/*02000008*/::.ctor() /* 06000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DirectorySecurity::.ctor

} // end of class System.Security.AccessControl.DirectorySecurity

.class /*02000004*/ public auto ansi sealed beforefieldinit System.Security.AccessControl.FileSecurity
       extends System.Security.AccessControl.FileSystemSecurity/*02000008*/
{
  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000022       */ call       instance void System.Security.AccessControl.FileSystemSecurity/*02000008*/::.ctor() /* 06000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FileSecurity::.ctor

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlSections/*01000026*/ includeSections) cil managed
  // SIG: 20 02 01 0E 11 80 99
  {
    .custom /*0C000023:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20a2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000022       */ call       instance void System.Security.AccessControl.FileSystemSecurity/*02000008*/::.ctor() /* 06000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FileSecurity::.ctor

} // end of class System.Security.AccessControl.FileSecurity

.class /*02000005*/ public auto ansi sealed beforefieldinit System.Security.AccessControl.FileSystemAccessRule
       extends [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/
{
  .custom /*0C000018:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000019:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 03 01 12 69 11 1C 11 75
  {
    // Method begins at RVA 0x20aa
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000018       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                            int32,
                                                                                                                                                                            bool,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/) /* 0A000018 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAccessRule::.ctor

  .method /*06000019*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 05 01 12 69 11 1C 11 6D 11 71 11 75
  {
    // Method begins at RVA 0x20b8
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000018       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                            int32,
                                                                                                                                                                            bool,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/) /* 0A000018 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAccessRule::.ctor

  .method /*0600001A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 03 01 0E 11 1C 11 75
  {
    // Method begins at RVA 0x20c6
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000018       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                            int32,
                                                                                                                                                                            bool,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/) /* 0A000018 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAccessRule::.ctor

  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 05 01 0E 11 1C 11 6D 11 71 11 75
  {
    // Method begins at RVA 0x20d4
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000018       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                            int32,
                                                                                                                                                                            bool,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/) /* 0A000018 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAccessRule::.ctor

  .method /*0600001C*/ public hidebysig specialname 
          instance valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ 
          get_FileSystemRights() cil managed
  // SIG: 20 00 11 1C
  {
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAccessRule::get_FileSystemRights

  .property /*17000001*/ instance callconv(8) valuetype System.Security.AccessControl.FileSystemRights/*02000007*/
          FileSystemRights()
  {
    .get instance valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ System.Security.AccessControl.FileSystemAccessRule/*02000005*/::get_FileSystemRights() /* 0600001C */
  } // end of property FileSystemAccessRule::FileSystemRights
} // end of class System.Security.AccessControl.FileSystemAccessRule

.class /*02000006*/ public auto ansi sealed beforefieldinit System.Security.AccessControl.FileSystemAuditRule
       extends [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/
{
  .custom /*0C00001A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 03 01 12 69 11 1C 11 80 89
  {
    // Method begins at RVA 0x20e5
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000019       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                           int32,
                                                                                                                                                                           bool,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/) /* 0A000019 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAuditRule::.ctor

  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 05 01 12 69 11 1C 11 6D 11 71 11 80 89
  {
    // Method begins at RVA 0x20f3
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000019       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                           int32,
                                                                                                                                                                           bool,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/) /* 0A000019 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAuditRule::.ctor

  .method /*0600001F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 03 01 0E 11 1C 11 80 89
  {
    // Method begins at RVA 0x2101
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000019       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                           int32,
                                                                                                                                                                           bool,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/) /* 0A000019 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAuditRule::.ctor

  .method /*06000020*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identity,
                               valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ fileSystemRights,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                               valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 05 01 0E 11 1C 11 6D 11 71 11 80 89
  {
    // Method begins at RVA 0x210f
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 16   |                  */ ldc.i4.0
    IL_0004:  /* 16   |                  */ ldc.i4.0
    IL_0005:  /* 16   |                  */ ldc.i4.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 28   | (0A)000019       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/::.ctor(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/,
                                                                                                                                                                           int32,
                                                                                                                                                                           bool,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/,
                                                                                                                                                                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/) /* 0A000019 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileSystemAuditRule::.ctor

  .method /*06000021*/ public hidebysig specialname 
          instance valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ 
          get_FileSystemRights() cil managed
  // SIG: 20 00 11 1C
  {
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAuditRule::get_FileSystemRights

  .property /*17000002*/ instance callconv(8) valuetype System.Security.AccessControl.FileSystemRights/*02000007*/
          FileSystemRights()
  {
    .get instance valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ System.Security.AccessControl.FileSystemAuditRule/*02000006*/::get_FileSystemRights() /* 06000021 */
  } // end of property FileSystemAuditRule::FileSystemRights
} // end of class System.Security.AccessControl.FileSystemAuditRule

.class /*02000007*/ public auto ansi sealed System.Security.AccessControl.FileSystemRights
       extends [System.Runtime/*23000001*/]System.Enum/*01000028*/
{
  .custom /*0C00001C:0A000016*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000027*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ ListDirectory = int32(0x00000001)
  .field /*04000003*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ ReadData = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ CreateFiles = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ WriteData = int32(0x00000002)
  .field /*04000006*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ AppendData = int32(0x00000004)
  .field /*04000007*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ CreateDirectories = int32(0x00000004)
  .field /*04000008*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ ReadExtendedAttributes = int32(0x00000008)
  .field /*04000009*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ WriteExtendedAttributes = int32(0x00000010)
  .field /*0400000A*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ ExecuteFile = int32(0x00000020)
  .field /*0400000B*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ Traverse = int32(0x00000020)
  .field /*0400000C*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ DeleteSubdirectoriesAndFiles = int32(0x00000040)
  .field /*0400000D*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ ReadAttributes = int32(0x00000080)
  .field /*0400000E*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ WriteAttributes = int32(0x00000100)
  .field /*0400000F*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ Write = int32(0x00000116)
  .field /*04000010*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ Delete = int32(0x00010000)
  .field /*04000011*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ ReadPermissions = int32(0x00020000)
  .field /*04000012*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ Read = int32(0x00020089)
  .field /*04000013*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ ReadAndExecute = int32(0x000200A9)
  .field /*04000014*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ Modify = int32(0x000301BF)
  .field /*04000015*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ ChangePermissions = int32(0x00040000)
  .field /*04000016*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ TakeOwnership = int32(0x00080000)
  .field /*04000017*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ Synchronize = int32(0x00100000)
  .field /*04000018*/ public static literal valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ FullControl = int32(0x001F01FF)
} // end of class System.Security.AccessControl.FileSystemRights

.class /*02000008*/ public abstract auto ansi beforefieldinit System.Security.AccessControl.FileSystemSecurity
       extends [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.NativeObjectSecurity/*01000029*/
{
  .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .method /*06000022*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2120
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (0A)00001A       */ call       instance void [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.NativeObjectSecurity/*01000029*/::.ctor(bool,
                                                                                                                                                                                      valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.ResourceType/*01000031*/) /* 0A00001A */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::.ctor

  .method /*06000023*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ 
          get_AccessRightType() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x212a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::get_AccessRightType

  .method /*06000024*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ 
          get_AccessRuleType() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x212d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::get_AccessRuleType

  .method /*06000025*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ 
          get_AuditRuleType() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x2130
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::get_AuditRuleType

  .method /*06000026*/ public hidebysig virtual final 
          instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessRule/*01000019*/ 
          AccessRuleFactory(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identityReference,
                            int32 accessMask,
                            bool isInherited,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                            valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlType/*0100001D*/ 'type') cil managed
  // SIG: 20 06 12 65 12 69 08 02 11 6D 11 71 11 75
  {
    // Method begins at RVA 0x2133
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::AccessRuleFactory

  .method /*06000027*/ public hidebysig instance void 
          AddAccessRule(class System.Security.AccessControl.FileSystemAccessRule/*02000005*/ rule) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x2136
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::AddAccessRule

  .method /*06000028*/ public hidebysig instance void 
          AddAuditRule(class System.Security.AccessControl.FileSystemAuditRule/*02000006*/ rule) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x2138
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::AddAuditRule

  .method /*06000029*/ public hidebysig virtual final 
          instance class [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditRule/*01000021*/ 
          AuditRuleFactory(class [System.Security.Principal.Windows/*23000003*/]System.Security.Principal.IdentityReference/*0100001A*/ identityReference,
                           int32 accessMask,
                           bool isInherited,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.InheritanceFlags/*0100001B*/ inheritanceFlags,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.PropagationFlags/*0100001C*/ propagationFlags,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AuditFlags/*01000022*/ 'flags') cil managed
  // SIG: 20 06 12 80 85 12 69 08 02 11 6D 11 71 11 80 89
  {
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::AuditRuleFactory

  .method /*0600002A*/ public hidebysig instance bool 
          RemoveAccessRule(class System.Security.AccessControl.FileSystemAccessRule/*02000005*/ rule) cil managed
  // SIG: 20 01 02 12 14
  {
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::RemoveAccessRule

  .method /*0600002B*/ public hidebysig instance void 
          RemoveAccessRuleAll(class System.Security.AccessControl.FileSystemAccessRule/*02000005*/ rule) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x2140
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::RemoveAccessRuleAll

  .method /*0600002C*/ public hidebysig instance void 
          RemoveAccessRuleSpecific(class System.Security.AccessControl.FileSystemAccessRule/*02000005*/ rule) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x2142
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::RemoveAccessRuleSpecific

  .method /*0600002D*/ public hidebysig instance bool 
          RemoveAuditRule(class System.Security.AccessControl.FileSystemAuditRule/*02000006*/ rule) cil managed
  // SIG: 20 01 02 12 18
  {
    // Method begins at RVA 0x2144
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemSecurity::RemoveAuditRule

  .method /*0600002E*/ public hidebysig instance void 
          RemoveAuditRuleAll(class System.Security.AccessControl.FileSystemAuditRule/*02000006*/ rule) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x2147
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::RemoveAuditRuleAll

  .method /*0600002F*/ public hidebysig instance void 
          RemoveAuditRuleSpecific(class System.Security.AccessControl.FileSystemAuditRule/*02000006*/ rule) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x2149
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::RemoveAuditRuleSpecific

  .method /*06000030*/ public hidebysig instance void 
          ResetAccessRule(class System.Security.AccessControl.FileSystemAccessRule/*02000005*/ rule) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x214b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::ResetAccessRule

  .method /*06000031*/ public hidebysig instance void 
          SetAccessRule(class System.Security.AccessControl.FileSystemAccessRule/*02000005*/ rule) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x214d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::SetAccessRule

  .method /*06000032*/ public hidebysig instance void 
          SetAuditRule(class System.Security.AccessControl.FileSystemAuditRule/*02000006*/ rule) cil managed
  // SIG: 20 01 01 12 18
  {
    // Method begins at RVA 0x214f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemSecurity::SetAuditRule

  .property /*17000003*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000024*/
          AccessRightType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ System.Security.AccessControl.FileSystemSecurity/*02000008*/::get_AccessRightType() /* 06000023 */
  } // end of property FileSystemSecurity::AccessRightType
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000024*/
          AccessRuleType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ System.Security.AccessControl.FileSystemSecurity/*02000008*/::get_AccessRuleType() /* 06000024 */
  } // end of property FileSystemSecurity::AccessRuleType
  .property /*17000005*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000024*/
          AuditRuleType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000024*/ System.Security.AccessControl.FileSystemSecurity/*02000008*/::get_AuditRuleType() /* 06000025 */
  } // end of property FileSystemSecurity::AuditRuleType
} // end of class System.Security.AccessControl.FileSystemSecurity

.class /*02000009*/ public abstract auto ansi sealed beforefieldinit System.IO.FileSystemAclExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100002A*/
{
  .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000020:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000021:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000033*/ public hidebysig static 
          void  Create(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ directoryInfo,
                       class System.Security.AccessControl.DirectorySecurity/*02000003*/ directorySecurity) cil managed
  // SIG: 00 02 01 12 80 AD 12 0C
  {
    .custom /*0C000024:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2151
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemAclExtensions::Create

  .method /*06000034*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IO.FileStream/*0100002C*/ 
          Create(class [System.Runtime/*23000001*/]System.IO.FileInfo/*0100002D*/ fileInfo,
                 valuetype [System.Runtime/*23000001*/]System.IO.FileMode/*0100002E*/ mode,
                 valuetype System.Security.AccessControl.FileSystemRights/*02000007*/ rights,
                 valuetype [System.Runtime/*23000001*/]System.IO.FileShare/*0100002F*/ share,
                 int32 bufferSize,
                 valuetype [System.Runtime/*23000001*/]System.IO.FileOptions/*01000030*/ options,
                 class System.Security.AccessControl.FileSecurity/*02000004*/ fileSecurity) cil managed
  // SIG: 00 07 12 80 B1 12 80 B5 11 80 B9 11 1C 11 80 BD 08 11 80 C1 12 10
  {
    .custom /*0C000025:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [7]/*0800006C*/ 
    .custom /*0C00002F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2153
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::Create

  .method /*06000035*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ 
          CreateDirectory(class System.Security.AccessControl.DirectorySecurity/*02000003*/ directorySecurity,
                          string path) cil managed
  // SIG: 00 02 12 80 AD 12 0C 0E
  {
    .custom /*0C000026:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2156
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::CreateDirectory

  .method /*06000036*/ public hidebysig static 
          class System.Security.AccessControl.DirectorySecurity/*02000003*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ directoryInfo) cil managed
  // SIG: 00 01 12 0C 12 80 AD
  {
    .custom /*0C000027:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2159
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*06000037*/ public hidebysig static 
          class System.Security.AccessControl.DirectorySecurity/*02000003*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ directoryInfo,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlSections/*01000026*/ includeSections) cil managed
  // SIG: 00 02 12 0C 12 80 AD 11 80 99
  {
    .custom /*0C000028:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*06000038*/ public hidebysig static 
          class System.Security.AccessControl.FileSecurity/*02000004*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileInfo/*0100002D*/ fileInfo) cil managed
  // SIG: 00 01 12 10 12 80 B5
  {
    .custom /*0C000029:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*06000039*/ public hidebysig static 
          class System.Security.AccessControl.FileSecurity/*02000004*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileInfo/*0100002D*/ fileInfo,
                           valuetype [System.Security.AccessControl/*23000002*/]System.Security.AccessControl.AccessControlSections/*01000026*/ includeSections) cil managed
  // SIG: 00 02 12 10 12 80 B5 11 80 99
  {
    .custom /*0C00002A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2162
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*0600003A*/ public hidebysig static 
          class System.Security.AccessControl.FileSecurity/*02000004*/ 
          GetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileStream/*0100002C*/ fileStream) cil managed
  // SIG: 00 01 12 10 12 80 B1
  {
    .custom /*0C00002B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2165
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileSystemAclExtensions::GetAccessControl

  .method /*0600003B*/ public hidebysig static 
          void  SetAccessControl(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100002B*/ directoryInfo,
                                 class System.Security.AccessControl.DirectorySecurity/*02000003*/ directorySecurity) cil managed
  // SIG: 00 02 01 12 80 AD 12 0C
  {
    .custom /*0C00002C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2168
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemAclExtensions::SetAccessControl

  .method /*0600003C*/ public hidebysig static 
          void  SetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileInfo/*0100002D*/ fileInfo,
                                 class System.Security.AccessControl.FileSecurity/*02000004*/ fileSecurity) cil managed
  // SIG: 00 02 01 12 80 B5 12 10
  {
    .custom /*0C00002D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x216a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemAclExtensions::SetAccessControl

  .method /*0600003D*/ public hidebysig static 
          void  SetAccessControl(class [System.Runtime/*23000001*/]System.IO.FileStream/*0100002C*/ fileStream,
                                 class System.Security.AccessControl.FileSecurity/*02000004*/ fileSecurity) cil managed
  // SIG: 00 02 01 12 80 B1 12 10
  {
    .custom /*0C00002E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x216c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileSystemAclExtensions::SetAccessControl

} // end of class System.IO.FileSystemAclExtensions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
