
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
// Time-date stamp:            0xb12e381d
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000b200
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000d0f6
// Base of code:                   0x00002000
// Base of data:                   0x0000e000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00012000
// Size of headers:                0x00000200
// Checksum:                       0x000122e7
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000d0a4 [0x0000004f] address [size] of Import Directory:          
// 0x0000e000 [0x00000498] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000bc00 [0x00002910] address [size] of Security Directory:        
// 0x00010000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000d088 [0x0000001c] address [size] of Debug Directory:           
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
//              0x0000b0fc Virtual Size
//              0x00002000 Virtual Address
//              0x0000b200 Size of Raw Data
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
//              0x0000e000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000b400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00010000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0000ba00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000d000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000000f8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000d0e6 Import Name Table
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
// 0x00002c08 [0x0000a400] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000d008 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00007bb8 Size
//              '#~' Name
//    Stream 2:
//              0x00007c24 Offset
//              0x0000152c Size
//              '#Strings' Name
//    Stream 3:
//              0x00009150 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00009154 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00009164 Offset
//              0x0000129c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000040909a05757 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 58640
// PE header size       : 512 (496 used)    ( 0.87%)
// PE additional info   : 11887             (20.27%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.12%)
// CLR meta-data size  : 41984              (71.60%)
// CLR additional info : 128                ( 0.22%)
// CLR method headers  : 1004               ( 1.71%)
// Managed code         : 1995              ( 3.40%)
// Data                 : 2048              ( 3.49%)
// Unaccounted          : -990              (-1.69%)

// Num.of PE sections   : 3
//   .text    - 45568
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 41984
//   Module        -    1 (10 bytes)
//   TypeDef       -   10 (140 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   40 (240 bytes)
//   MethodDef     - 1004 (14056 bytes)   0 abstract, 0 native, 1004 bodies
//   FieldDef      -   38 (228 bytes)     0 constant
//   MemberRef     -   30 (180 bytes)
//   ParamDef      - 1731 (10386 bytes)
//   CustomAttribute-  726 (4356 bytes)
//   InterfaceImpl -   12 (48 bytes)
//   PropertyMap   -    8 (32 bytes)
//   Property      -   74 (444 bytes)
//   MethodSemantic-   91 (546 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    8 (16 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   GenericParam  -  106 (848 bytes)
//   Strings       -  5417 bytes
//   Blobs         -  4764 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   205 bytes

// CLR additional info : 128

// CLR method headers : 1004
//   Num.of method bodies  - 1004
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 1004

// Managed code : 1995
//   Ave method size - 1
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class Matrix3x2                      (public) (sequential) (ansi) (sealed) 
// Class Matrix4x4                      (public) (sequential) (ansi) (sealed) 
// Class Plane                          (public) (sequential) (ansi) (sealed) 
// Class Quaternion                     (public) (sequential) (ansi) (sealed) 
// Class Vector                         (public) (abstract) (auto) (ansi) (sealed) 
// Class Vector2                        (public) (sequential) (ansi) (sealed) 
// Class Vector3                        (public) (sequential) (ansi) (sealed) 
// Class Vector4                        (public) (sequential) (ansi) (sealed) 
// Class Vector`1                       (public) (sequential) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.Numerics.Vectors
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000004:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000006:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 17 53 79 73 74 65 6D 2E 4E 75 6D 65 72 69   // ...System.Numeri
                                                                                                                                                                        63 73 2E 56 65 63 74 6F 72 73 00 00 )             // cs.Vectors..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 17 53 79 73 74 65 6D 2E 4E 75 6D 65 72 69   // ...System.Numeri
                                                                                                                                                                       63 73 2E 56 65 63 74 6F 72 73 00 00 )             // cs.Vectors..
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 17 53 79 73 74 65 6D 2E 4E 75 6D 65 72 69   // ...System.Numeri
                                                                                                                                                                 63 73 2E 56 65 63 74 6F 72 73 00 00 )             // cs.Vectors..
  .custom /*0C000017:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 64 6F 74 6E 65   // com/dotnet/dotne
                                                                                                                                                                    74 00 00 )                                        // t..
  .permissionset reqmin
             = {[System.Runtime/*23000001*/]System.Security.Permissions.SecurityPermissionAttribute/*01000013*/ = {property bool 'SkipVerification' = bool(true)}}
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
.module System.Numerics.Vectors.dll
// MVID: {dcdfe3e5-5901-43cd-9bf1-bd6c077f76fd}
.custom /*0C000001:0A000012*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
.custom /*0C000002:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000015*/::.ctor(int32) /* 0A000013 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000016*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F7455F14000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public sequential ansi sealed beforefieldinit System.Numerics.Matrix3x2
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Matrix3x2/*02000002*/>/*1B000001*/
{
  .custom /*0C000019:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .field /*04000001*/ public float32 M11
  .field /*04000002*/ public float32 M12
  .field /*04000003*/ public float32 M21
  .field /*04000004*/ public float32 M22
  .field /*04000005*/ public float32 M31
  .field /*04000006*/ public float32 M32
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 m11,
                               float32 m12,
                               float32 m21,
                               float32 m22,
                               float32 m31,
                               float32 m32) cil managed
  // SIG: 20 06 01 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::.ctor

  .method /*06000002*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          get_Identity() cil managed
  // SIG: 00 00 11 08
  {
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::get_Identity

  .method /*06000003*/ public hidebysig specialname 
          instance bool  get_IsIdentity() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00001A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::get_IsIdentity

  .method /*06000004*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector2/*02000007*/ 
          get_Translation() cil managed
  // SIG: 20 00 11 1C
  {
    .custom /*0C00001C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::get_Translation

  .method /*06000005*/ public hidebysig specialname 
          instance void  set_Translation(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 11 1C
  {
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::set_Translation

  .method /*06000006*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector2/*02000007*/ 
          get_X() cil managed
  // SIG: 20 00 11 1C
  {
    .custom /*0C00001F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::get_X

  .method /*06000007*/ public hidebysig specialname 
          instance void  set_X(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 11 1C
  {
    // Method begins at RVA 0x2062
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::set_X

  .method /*06000008*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector2/*02000007*/ 
          get_Y() cil managed
  // SIG: 20 00 11 1C
  {
    .custom /*0C000022:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::get_Y

  .method /*06000009*/ public hidebysig specialname 
          instance void  set_Y(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 11 1C
  {
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::set_Y

  .method /*0600000A*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector2/*02000007*/ 
          get_Z() cil managed
  // SIG: 20 00 11 1C
  {
    .custom /*0C000026:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::get_Z

  .method /*0600000B*/ public hidebysig specialname 
          instance void  set_Z(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 11 1C
  {
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::set_Z

  .method /*0600000C*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector2/*02000007*/ 
          get_Item(int32 row) cil managed
  // SIG: 20 01 11 1C 08
  {
    .custom /*0C00002A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2071
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::get_Item

  .method /*0600000D*/ public hidebysig specialname 
          instance void  set_Item(int32 row,
                                  valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 20 02 01 08 11 1C
  {
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::set_Item

  .method /*0600000E*/ public hidebysig specialname 
          instance float32  get_Item(int32 row,
                                     int32 column) cil managed
  // SIG: 20 02 0C 08 08
  {
    .custom /*0C00002D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2077
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::get_Item

  .method /*0600000F*/ public hidebysig specialname 
          instance void  set_Item(int32 row,
                                  int32 column,
                                  float32 'value') cil managed
  // SIG: 20 03 01 08 08 0C
  {
    // Method begins at RVA 0x207a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::set_Item

  .method /*06000010*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Add(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
              valuetype System.Numerics.Matrix3x2/*02000002*/ value2) cil managed
  // SIG: 00 02 11 08 11 08 11 08
  {
    // Method begins at RVA 0x207d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Add

  .method /*06000011*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Create(float32 'value') cil managed
  // SIG: 00 01 11 08 0C
  {
    // Method begins at RVA 0x2080
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Create

  .method /*06000012*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Create(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 08 11 1C
  {
    // Method begins at RVA 0x2083
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Create

  .method /*06000013*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Create(valuetype System.Numerics.Vector2/*02000007*/ x,
                 valuetype System.Numerics.Vector2/*02000007*/ y,
                 valuetype System.Numerics.Vector2/*02000007*/ z) cil managed
  // SIG: 00 03 11 08 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2086
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Create

  .method /*06000014*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Create(float32 m11,
                 float32 m12,
                 float32 m21,
                 float32 m22,
                 float32 m31,
                 float32 m32) cil managed
  // SIG: 00 06 11 08 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2089
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Create

  .method /*06000015*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateRotation(float32 radians) cil managed
  // SIG: 00 01 11 08 0C
  {
    // Method begins at RVA 0x208c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateRotation

  .method /*06000016*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateRotation(float32 radians,
                         valuetype System.Numerics.Vector2/*02000007*/ centerPoint) cil managed
  // SIG: 00 02 11 08 0C 11 1C
  {
    // Method begins at RVA 0x208f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateRotation

  .method /*06000017*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateScale(valuetype System.Numerics.Vector2/*02000007*/ scales) cil managed
  // SIG: 00 01 11 08 11 1C
  {
    // Method begins at RVA 0x2092
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateScale

  .method /*06000018*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateScale(valuetype System.Numerics.Vector2/*02000007*/ scales,
                      valuetype System.Numerics.Vector2/*02000007*/ centerPoint) cil managed
  // SIG: 00 02 11 08 11 1C 11 1C
  {
    // Method begins at RVA 0x2095
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateScale

  .method /*06000019*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateScale(float32 scale) cil managed
  // SIG: 00 01 11 08 0C
  {
    // Method begins at RVA 0x2098
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateScale

  .method /*0600001A*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateScale(float32 scale,
                      valuetype System.Numerics.Vector2/*02000007*/ centerPoint) cil managed
  // SIG: 00 02 11 08 0C 11 1C
  {
    // Method begins at RVA 0x209b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateScale

  .method /*0600001B*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateScale(float32 xScale,
                      float32 yScale) cil managed
  // SIG: 00 02 11 08 0C 0C
  {
    // Method begins at RVA 0x209e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateScale

  .method /*0600001C*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateScale(float32 xScale,
                      float32 yScale,
                      valuetype System.Numerics.Vector2/*02000007*/ centerPoint) cil managed
  // SIG: 00 03 11 08 0C 0C 11 1C
  {
    // Method begins at RVA 0x20a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateScale

  .method /*0600001D*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateSkew(float32 radiansX,
                     float32 radiansY) cil managed
  // SIG: 00 02 11 08 0C 0C
  {
    // Method begins at RVA 0x20a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateSkew

  .method /*0600001E*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateSkew(float32 radiansX,
                     float32 radiansY,
                     valuetype System.Numerics.Vector2/*02000007*/ centerPoint) cil managed
  // SIG: 00 03 11 08 0C 0C 11 1C
  {
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateSkew

  .method /*0600001F*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateTranslation(valuetype System.Numerics.Vector2/*02000007*/ position) cil managed
  // SIG: 00 01 11 08 11 1C
  {
    // Method begins at RVA 0x20aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateTranslation

  .method /*06000020*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          CreateTranslation(float32 xPosition,
                            float32 yPosition) cil managed
  // SIG: 00 02 11 08 0C 0C
  {
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::CreateTranslation

  .method /*06000021*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Numerics.Matrix3x2/*02000002*/ other) cil managed
  // SIG: 20 01 02 11 08
  {
    .custom /*0C00003A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Equals

  .method /*06000022*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00003B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00003C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000037*/ 
    .custom /*0C00004C:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001C*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Equals

  .method /*06000023*/ public hidebysig instance float32 
          GetDeterminant() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C00003E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::GetDeterminant

  .method /*06000024*/ public hidebysig instance float32 
          GetElement(int32 row,
                     int32 column) cil managed
  // SIG: 20 02 0C 08 08
  {
    .custom /*0C000040:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::GetElement

  .method /*06000025*/ public hidebysig instance valuetype System.Numerics.Vector2/*02000007*/ 
          GetRow(int32 index) cil managed
  // SIG: 20 01 11 1C 08
  {
    .custom /*0C000042:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::GetRow

  .method /*06000026*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000043:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::GetHashCode

  .method /*06000027*/ public hidebysig static 
          bool  Invert(valuetype System.Numerics.Matrix3x2/*02000002*/ matrix,
                       [out] valuetype System.Numerics.Matrix3x2/*02000002*/& result) cil managed
  // SIG: 00 02 02 11 08 10 11 08
  {
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Invert

  .method /*06000028*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Lerp(valuetype System.Numerics.Matrix3x2/*02000002*/ matrix1,
               valuetype System.Numerics.Matrix3x2/*02000002*/ matrix2,
               float32 amount) cil managed
  // SIG: 00 03 11 08 11 08 11 08 0C
  {
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Lerp

  .method /*06000029*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Multiply(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                   valuetype System.Numerics.Matrix3x2/*02000002*/ value2) cil managed
  // SIG: 00 02 11 08 11 08 11 08
  {
    // Method begins at RVA 0x20c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Multiply

  .method /*0600002A*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Multiply(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                   float32 value2) cil managed
  // SIG: 00 02 11 08 11 08 0C
  {
    // Method begins at RVA 0x20cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Multiply

  .method /*0600002B*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Negate(valuetype System.Numerics.Matrix3x2/*02000002*/ 'value') cil managed
  // SIG: 00 01 11 08 11 08
  {
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Negate

  .method /*0600002C*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          op_Addition(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                      valuetype System.Numerics.Matrix3x2/*02000002*/ value2) cil managed
  // SIG: 00 02 11 08 11 08 11 08
  {
    // Method begins at RVA 0x20d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::op_Addition

  .method /*0600002D*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                            valuetype System.Numerics.Matrix3x2/*02000002*/ value2) cil managed
  // SIG: 00 02 02 11 08 11 08
  {
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::op_Equality

  .method /*0600002E*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                              valuetype System.Numerics.Matrix3x2/*02000002*/ value2) cil managed
  // SIG: 00 02 02 11 08 11 08
  {
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::op_Inequality

  .method /*0600002F*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          op_Multiply(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                      valuetype System.Numerics.Matrix3x2/*02000002*/ value2) cil managed
  // SIG: 00 02 11 08 11 08 11 08
  {
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::op_Multiply

  .method /*06000030*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          op_Multiply(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                      float32 value2) cil managed
  // SIG: 00 02 11 08 11 08 0C
  {
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::op_Multiply

  .method /*06000031*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          op_Subtraction(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                         valuetype System.Numerics.Matrix3x2/*02000002*/ value2) cil managed
  // SIG: 00 02 11 08 11 08 11 08
  {
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::op_Subtraction

  .method /*06000032*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          op_UnaryNegation(valuetype System.Numerics.Matrix3x2/*02000002*/ 'value') cil managed
  // SIG: 00 01 11 08 11 08
  {
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::op_UnaryNegation

  .method /*06000033*/ public hidebysig static 
          valuetype System.Numerics.Matrix3x2/*02000002*/ 
          Subtract(valuetype System.Numerics.Matrix3x2/*02000002*/ value1,
                   valuetype System.Numerics.Matrix3x2/*02000002*/ value2) cil managed
  // SIG: 00 02 11 08 11 08 11 08
  {
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::Subtract

  .method /*06000034*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000048:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000049:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::ToString

  .method /*06000035*/ public hidebysig instance valuetype System.Numerics.Matrix3x2/*02000002*/ 
          WithElement(int32 row,
                      int32 column,
                      float32 'value') cil managed
  // SIG: 20 03 11 08 08 08 0C
  {
    .custom /*0C00004A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::WithElement

  .method /*06000036*/ public hidebysig instance valuetype System.Numerics.Matrix3x2/*02000002*/ 
          WithRow(int32 index,
                  valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 20 02 11 08 08 11 1C
  {
    .custom /*0C00004B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix3x2::WithRow

  .property /*17000001*/ callconv(8) valuetype System.Numerics.Matrix3x2/*02000002*/
          Identity()
  {
    .get valuetype System.Numerics.Matrix3x2/*02000002*/ System.Numerics.Matrix3x2/*02000002*/::get_Identity() /* 06000002 */
  } // end of property Matrix3x2::Identity
  .property /*17000002*/ instance callconv(8) bool
          IsIdentity()
  {
    .get instance bool System.Numerics.Matrix3x2/*02000002*/::get_IsIdentity() /* 06000003 */
  } // end of property Matrix3x2::IsIdentity
  .property /*17000003*/ instance callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          Translation()
  {
    .get instance valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Matrix3x2/*02000002*/::get_Translation() /* 06000004 */
    .set instance void System.Numerics.Matrix3x2/*02000002*/::set_Translation(valuetype System.Numerics.Vector2/*02000007*/) /* 06000005 */
  } // end of property Matrix3x2::Translation
  .property /*17000004*/ instance callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          X()
  {
    .get instance valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Matrix3x2/*02000002*/::get_X() /* 06000006 */
    .set instance void System.Numerics.Matrix3x2/*02000002*/::set_X(valuetype System.Numerics.Vector2/*02000007*/) /* 06000007 */
  } // end of property Matrix3x2::X
  .property /*17000005*/ instance callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          Y()
  {
    .get instance valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Matrix3x2/*02000002*/::get_Y() /* 06000008 */
    .set instance void System.Numerics.Matrix3x2/*02000002*/::set_Y(valuetype System.Numerics.Vector2/*02000007*/) /* 06000009 */
  } // end of property Matrix3x2::Y
  .property /*17000006*/ instance callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          Z()
  {
    .get instance valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Matrix3x2/*02000002*/::get_Z() /* 0600000A */
    .set instance void System.Numerics.Matrix3x2/*02000002*/::set_Z(valuetype System.Numerics.Vector2/*02000007*/) /* 0600000B */
  } // end of property Matrix3x2::Z
  .property /*17000007*/ instance callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          Item(int32)
  {
    .get instance valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Matrix3x2/*02000002*/::get_Item(int32) /* 0600000C */
    .set instance void System.Numerics.Matrix3x2/*02000002*/::set_Item(int32,
                                                                       valuetype System.Numerics.Vector2/*02000007*/) /* 0600000D */
  } // end of property Matrix3x2::Item
  .property /*17000008*/ instance callconv(8) float32
          Item(int32,
               int32)
  {
    .get instance float32 System.Numerics.Matrix3x2/*02000002*/::get_Item(int32,
                                                                          int32) /* 0600000E */
    .set instance void System.Numerics.Matrix3x2/*02000002*/::set_Item(int32,
                                                                       int32,
                                                                       float32) /* 0600000F */
  } // end of property Matrix3x2::Item
} // end of class System.Numerics.Matrix3x2

.class /*02000003*/ public sequential ansi sealed beforefieldinit System.Numerics.Matrix4x4
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Matrix4x4/*02000003*/>/*1B000002*/
{
  .custom /*0C00001B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .field /*04000007*/ public float32 M11
  .field /*04000008*/ public float32 M12
  .field /*04000009*/ public float32 M13
  .field /*0400000A*/ public float32 M14
  .field /*0400000B*/ public float32 M21
  .field /*0400000C*/ public float32 M22
  .field /*0400000D*/ public float32 M23
  .field /*0400000E*/ public float32 M24
  .field /*0400000F*/ public float32 M31
  .field /*04000010*/ public float32 M32
  .field /*04000011*/ public float32 M33
  .field /*04000012*/ public float32 M34
  .field /*04000013*/ public float32 M41
  .field /*04000014*/ public float32 M42
  .field /*04000015*/ public float32 M43
  .field /*04000016*/ public float32 M44
  .method /*06000037*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Numerics.Matrix3x2/*02000002*/ 'value') cil managed
  // SIG: 20 01 01 11 08
  {
    // Method begins at RVA 0x20f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::.ctor

  .method /*06000038*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 m11,
                               float32 m12,
                               float32 m13,
                               float32 m14,
                               float32 m21,
                               float32 m22,
                               float32 m23,
                               float32 m24,
                               float32 m31,
                               float32 m32,
                               float32 m33,
                               float32 m34,
                               float32 m41,
                               float32 m42,
                               float32 m43,
                               float32 m44) cil managed
  // SIG: 20 10 01 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::.ctor

  .method /*06000039*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          get_Identity() cil managed
  // SIG: 00 00 11 0C
  {
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_Identity

  .method /*0600003A*/ public hidebysig specialname 
          instance bool  get_IsIdentity() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00004D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_IsIdentity

  .method /*0600003B*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector3/*02000008*/ 
          get_Translation() cil managed
  // SIG: 20 00 11 20
  {
    .custom /*0C00004E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_Translation

  .method /*0600003C*/ public hidebysig specialname 
          instance void  set_Translation(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 20 01 01 11 20
  {
    // Method begins at RVA 0x2101
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::set_Translation

  .method /*0600003D*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector4/*02000009*/ 
          get_X() cil managed
  // SIG: 20 00 11 24
  {
    .custom /*0C00004F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_X

  .method /*0600003E*/ public hidebysig specialname 
          instance void  set_X(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::set_X

  .method /*0600003F*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector4/*02000009*/ 
          get_Y() cil managed
  // SIG: 20 00 11 24
  {
    .custom /*0C000050:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_Y

  .method /*06000040*/ public hidebysig specialname 
          instance void  set_Y(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::set_Y

  .method /*06000041*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector4/*02000009*/ 
          get_Z() cil managed
  // SIG: 20 00 11 24
  {
    .custom /*0C000051:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_Z

  .method /*06000042*/ public hidebysig specialname 
          instance void  set_Z(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::set_Z

  .method /*06000043*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector4/*02000009*/ 
          get_W() cil managed
  // SIG: 20 00 11 24
  {
    .custom /*0C000053:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_W

  .method /*06000044*/ public hidebysig specialname 
          instance void  set_W(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::set_W

  .method /*06000045*/ public hidebysig specialname 
          instance valuetype System.Numerics.Vector4/*02000009*/ 
          get_Item(int32 row) cil managed
  // SIG: 20 01 11 24 08
  {
    .custom /*0C000055:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_Item

  .method /*06000046*/ public hidebysig specialname 
          instance void  set_Item(int32 row,
                                  valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 20 02 01 08 11 24
  {
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::set_Item

  .method /*06000047*/ public hidebysig specialname 
          instance float32  get_Item(int32 row,
                                     int32 column) cil managed
  // SIG: 20 02 0C 08 08
  {
    .custom /*0C000057:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::get_Item

  .method /*06000048*/ public hidebysig specialname 
          instance void  set_Item(int32 row,
                                  int32 column,
                                  float32 'value') cil managed
  // SIG: 20 03 01 08 08 0C
  {
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::set_Item

  .method /*06000049*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Add(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
              valuetype System.Numerics.Matrix4x4/*02000003*/ value2) cil managed
  // SIG: 00 02 11 0C 11 0C 11 0C
  {
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Add

  .method /*0600004A*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Create(float32 'value') cil managed
  // SIG: 00 01 11 0C 0C
  {
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Create

  .method /*0600004B*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Create(valuetype System.Numerics.Matrix3x2/*02000002*/ 'value') cil managed
  // SIG: 00 01 11 0C 11 08
  {
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Create

  .method /*0600004C*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Create(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 0C 11 24
  {
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Create

  .method /*0600004D*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Create(valuetype System.Numerics.Vector4/*02000009*/ x,
                 valuetype System.Numerics.Vector4/*02000009*/ y,
                 valuetype System.Numerics.Vector4/*02000009*/ z,
                 valuetype System.Numerics.Vector4/*02000009*/ w) cil managed
  // SIG: 00 04 11 0C 11 24 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Create

  .method /*0600004E*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Create(float32 m11,
                 float32 m12,
                 float32 m13,
                 float32 m14,
                 float32 m21,
                 float32 m22,
                 float32 m23,
                 float32 m24,
                 float32 m31,
                 float32 m32,
                 float32 m33,
                 float32 m34,
                 float32 m41,
                 float32 m42,
                 float32 m43,
                 float32 m44) cil managed
  // SIG: 00 10 11 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Create

  .method /*0600004F*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateBillboard(valuetype System.Numerics.Vector3/*02000008*/ objectPosition,
                          valuetype System.Numerics.Vector3/*02000008*/ cameraPosition,
                          valuetype System.Numerics.Vector3/*02000008*/ cameraUpVector,
                          valuetype System.Numerics.Vector3/*02000008*/ cameraForwardVector) cil managed
  // SIG: 00 04 11 0C 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateBillboard

  .method /*06000050*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateBillboardLeftHanded(valuetype System.Numerics.Vector3/*02000008*/ objectPosition,
                                    valuetype System.Numerics.Vector3/*02000008*/ cameraPosition,
                                    valuetype System.Numerics.Vector3/*02000008*/ cameraUpVector,
                                    valuetype System.Numerics.Vector3/*02000008*/ cameraForwardVector) cil managed
  // SIG: 00 04 11 0C 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateBillboardLeftHanded

  .method /*06000051*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateConstrainedBillboard(valuetype System.Numerics.Vector3/*02000008*/ objectPosition,
                                     valuetype System.Numerics.Vector3/*02000008*/ cameraPosition,
                                     valuetype System.Numerics.Vector3/*02000008*/ rotateAxis,
                                     valuetype System.Numerics.Vector3/*02000008*/ cameraForwardVector,
                                     valuetype System.Numerics.Vector3/*02000008*/ objectForwardVector) cil managed
  // SIG: 00 05 11 0C 11 20 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateConstrainedBillboard

  .method /*06000052*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateConstrainedBillboardLeftHanded(valuetype System.Numerics.Vector3/*02000008*/ objectPosition,
                                               valuetype System.Numerics.Vector3/*02000008*/ cameraPosition,
                                               valuetype System.Numerics.Vector3/*02000008*/ rotateAxis,
                                               valuetype System.Numerics.Vector3/*02000008*/ cameraForwardVector,
                                               valuetype System.Numerics.Vector3/*02000008*/ objectForwardVector) cil managed
  // SIG: 00 05 11 0C 11 20 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateConstrainedBillboardLeftHanded

  .method /*06000053*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateFromAxisAngle(valuetype System.Numerics.Vector3/*02000008*/ axis,
                              float32 angle) cil managed
  // SIG: 00 02 11 0C 11 20 0C
  {
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateFromAxisAngle

  .method /*06000054*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateFromQuaternion(valuetype System.Numerics.Quaternion/*02000005*/ quaternion) cil managed
  // SIG: 00 01 11 0C 11 14
  {
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateFromQuaternion

  .method /*06000055*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateFromYawPitchRoll(float32 yaw,
                                 float32 pitch,
                                 float32 roll) cil managed
  // SIG: 00 03 11 0C 0C 0C 0C
  {
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateFromYawPitchRoll

  .method /*06000056*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateLookAt(valuetype System.Numerics.Vector3/*02000008*/ cameraPosition,
                       valuetype System.Numerics.Vector3/*02000008*/ cameraTarget,
                       valuetype System.Numerics.Vector3/*02000008*/ cameraUpVector) cil managed
  // SIG: 00 03 11 0C 11 20 11 20 11 20
  {
    // Method begins at RVA 0x214f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateLookAt

  .method /*06000057*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateLookAtLeftHanded(valuetype System.Numerics.Vector3/*02000008*/ cameraPosition,
                                 valuetype System.Numerics.Vector3/*02000008*/ cameraTarget,
                                 valuetype System.Numerics.Vector3/*02000008*/ cameraUpVector) cil managed
  // SIG: 00 03 11 0C 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2152
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateLookAtLeftHanded

  .method /*06000058*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateLookTo(valuetype System.Numerics.Vector3/*02000008*/ cameraPosition,
                       valuetype System.Numerics.Vector3/*02000008*/ cameraDirection,
                       valuetype System.Numerics.Vector3/*02000008*/ cameraUpVector) cil managed
  // SIG: 00 03 11 0C 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2155
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateLookTo

  .method /*06000059*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateLookToLeftHanded(valuetype System.Numerics.Vector3/*02000008*/ cameraPosition,
                                 valuetype System.Numerics.Vector3/*02000008*/ cameraDirection,
                                 valuetype System.Numerics.Vector3/*02000008*/ cameraUpVector) cil managed
  // SIG: 00 03 11 0C 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateLookToLeftHanded

  .method /*0600005A*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateOrthographic(float32 width,
                             float32 height,
                             float32 zNearPlane,
                             float32 zFarPlane) cil managed
  // SIG: 00 04 11 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateOrthographic

  .method /*0600005B*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateOrthographicLeftHanded(float32 width,
                                       float32 height,
                                       float32 zNearPlane,
                                       float32 zFarPlane) cil managed
  // SIG: 00 04 11 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateOrthographicLeftHanded

  .method /*0600005C*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateOrthographicOffCenter(float32 left,
                                      float32 right,
                                      float32 bottom,
                                      float32 top,
                                      float32 zNearPlane,
                                      float32 zFarPlane) cil managed
  // SIG: 00 06 11 0C 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateOrthographicOffCenter

  .method /*0600005D*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateOrthographicOffCenterLeftHanded(float32 left,
                                                float32 right,
                                                float32 bottom,
                                                float32 top,
                                                float32 zNearPlane,
                                                float32 zFarPlane) cil managed
  // SIG: 00 06 11 0C 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateOrthographicOffCenterLeftHanded

  .method /*0600005E*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreatePerspective(float32 width,
                            float32 height,
                            float32 nearPlaneDistance,
                            float32 farPlaneDistance) cil managed
  // SIG: 00 04 11 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreatePerspective

  .method /*0600005F*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreatePerspectiveLeftHanded(float32 width,
                                      float32 height,
                                      float32 nearPlaneDistance,
                                      float32 farPlaneDistance) cil managed
  // SIG: 00 04 11 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreatePerspectiveLeftHanded

  .method /*06000060*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreatePerspectiveFieldOfView(float32 fieldOfView,
                                       float32 aspectRatio,
                                       float32 nearPlaneDistance,
                                       float32 farPlaneDistance) cil managed
  // SIG: 00 04 11 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreatePerspectiveFieldOfView

  .method /*06000061*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreatePerspectiveFieldOfViewLeftHanded(float32 fieldOfView,
                                                 float32 aspectRatio,
                                                 float32 nearPlaneDistance,
                                                 float32 farPlaneDistance) cil managed
  // SIG: 00 04 11 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreatePerspectiveFieldOfViewLeftHanded

  .method /*06000062*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreatePerspectiveOffCenter(float32 left,
                                     float32 right,
                                     float32 bottom,
                                     float32 top,
                                     float32 nearPlaneDistance,
                                     float32 farPlaneDistance) cil managed
  // SIG: 00 06 11 0C 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreatePerspectiveOffCenter

  .method /*06000063*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreatePerspectiveOffCenterLeftHanded(float32 left,
                                               float32 right,
                                               float32 bottom,
                                               float32 top,
                                               float32 nearPlaneDistance,
                                               float32 farPlaneDistance) cil managed
  // SIG: 00 06 11 0C 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreatePerspectiveOffCenterLeftHanded

  .method /*06000064*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateReflection(valuetype System.Numerics.Plane/*02000004*/ 'value') cil managed
  // SIG: 00 01 11 0C 11 10
  {
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateReflection

  .method /*06000065*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateRotationX(float32 radians) cil managed
  // SIG: 00 01 11 0C 0C
  {
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateRotationX

  .method /*06000066*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateRotationX(float32 radians,
                          valuetype System.Numerics.Vector3/*02000008*/ centerPoint) cil managed
  // SIG: 00 02 11 0C 0C 11 20
  {
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateRotationX

  .method /*06000067*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateRotationY(float32 radians) cil managed
  // SIG: 00 01 11 0C 0C
  {
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateRotationY

  .method /*06000068*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateRotationY(float32 radians,
                          valuetype System.Numerics.Vector3/*02000008*/ centerPoint) cil managed
  // SIG: 00 02 11 0C 0C 11 20
  {
    // Method begins at RVA 0x2185
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateRotationY

  .method /*06000069*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateRotationZ(float32 radians) cil managed
  // SIG: 00 01 11 0C 0C
  {
    // Method begins at RVA 0x2188
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateRotationZ

  .method /*0600006A*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateRotationZ(float32 radians,
                          valuetype System.Numerics.Vector3/*02000008*/ centerPoint) cil managed
  // SIG: 00 02 11 0C 0C 11 20
  {
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateRotationZ

  .method /*0600006B*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateScale(valuetype System.Numerics.Vector3/*02000008*/ scales) cil managed
  // SIG: 00 01 11 0C 11 20
  {
    // Method begins at RVA 0x218e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateScale

  .method /*0600006C*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateScale(valuetype System.Numerics.Vector3/*02000008*/ scales,
                      valuetype System.Numerics.Vector3/*02000008*/ centerPoint) cil managed
  // SIG: 00 02 11 0C 11 20 11 20
  {
    // Method begins at RVA 0x2191
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateScale

  .method /*0600006D*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateScale(float32 scale) cil managed
  // SIG: 00 01 11 0C 0C
  {
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateScale

  .method /*0600006E*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateScale(float32 scale,
                      valuetype System.Numerics.Vector3/*02000008*/ centerPoint) cil managed
  // SIG: 00 02 11 0C 0C 11 20
  {
    // Method begins at RVA 0x2197
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateScale

  .method /*0600006F*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateScale(float32 xScale,
                      float32 yScale,
                      float32 zScale) cil managed
  // SIG: 00 03 11 0C 0C 0C 0C
  {
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateScale

  .method /*06000070*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateScale(float32 xScale,
                      float32 yScale,
                      float32 zScale,
                      valuetype System.Numerics.Vector3/*02000008*/ centerPoint) cil managed
  // SIG: 00 04 11 0C 0C 0C 0C 11 20
  {
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateScale

  .method /*06000071*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateShadow(valuetype System.Numerics.Vector3/*02000008*/ lightDirection,
                       valuetype System.Numerics.Plane/*02000004*/ plane) cil managed
  // SIG: 00 02 11 0C 11 20 11 10
  {
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateShadow

  .method /*06000072*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateTranslation(valuetype System.Numerics.Vector3/*02000008*/ position) cil managed
  // SIG: 00 01 11 0C 11 20
  {
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateTranslation

  .method /*06000073*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateTranslation(float32 xPosition,
                            float32 yPosition,
                            float32 zPosition) cil managed
  // SIG: 00 03 11 0C 0C 0C 0C
  {
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateTranslation

  .method /*06000074*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateViewport(float32 x,
                         float32 y,
                         float32 width,
                         float32 height,
                         float32 minDepth,
                         float32 maxDepth) cil managed
  // SIG: 00 06 11 0C 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateViewport

  .method /*06000075*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateViewportLeftHanded(float32 x,
                                   float32 y,
                                   float32 width,
                                   float32 height,
                                   float32 minDepth,
                                   float32 maxDepth) cil managed
  // SIG: 00 06 11 0C 0C 0C 0C 0C 0C 0C
  {
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateViewportLeftHanded

  .method /*06000076*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          CreateWorld(valuetype System.Numerics.Vector3/*02000008*/ position,
                      valuetype System.Numerics.Vector3/*02000008*/ forward,
                      valuetype System.Numerics.Vector3/*02000008*/ up) cil managed
  // SIG: 00 03 11 0C 11 20 11 20 11 20
  {
    // Method begins at RVA 0x21af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::CreateWorld

  .method /*06000077*/ public hidebysig static 
          bool  Decompose(valuetype System.Numerics.Matrix4x4/*02000003*/ matrix,
                          [out] valuetype System.Numerics.Vector3/*02000008*/& scale,
                          [out] valuetype System.Numerics.Quaternion/*02000005*/& rotation,
                          [out] valuetype System.Numerics.Vector3/*02000008*/& translation) cil managed
  // SIG: 00 04 02 11 0C 10 11 20 10 11 14 10 11 20
  {
    // Method begins at RVA 0x21b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Decompose

  .method /*06000078*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Numerics.Matrix4x4/*02000003*/ other) cil managed
  // SIG: 20 01 02 11 0C
  {
    .custom /*0C000065:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Equals

  .method /*06000079*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000066:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000067:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000115*/ 
    .custom /*0C0000B3:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001C*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Equals

  .method /*0600007A*/ public hidebysig instance float32 
          GetDeterminant() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000068:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::GetDeterminant

  .method /*0600007B*/ public hidebysig instance float32 
          GetElement(int32 row,
                     int32 column) cil managed
  // SIG: 20 02 0C 08 08
  {
    .custom /*0C000069:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::GetElement

  .method /*0600007C*/ public hidebysig instance valuetype System.Numerics.Vector4/*02000009*/ 
          GetRow(int32 index) cil managed
  // SIG: 20 01 11 24 08
  {
    .custom /*0C00006A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::GetRow

  .method /*0600007D*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00006B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::GetHashCode

  .method /*0600007E*/ public hidebysig static 
          bool  Invert(valuetype System.Numerics.Matrix4x4/*02000003*/ matrix,
                       [out] valuetype System.Numerics.Matrix4x4/*02000003*/& result) cil managed
  // SIG: 00 02 02 11 0C 10 11 0C
  {
    // Method begins at RVA 0x21c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Invert

  .method /*0600007F*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Lerp(valuetype System.Numerics.Matrix4x4/*02000003*/ matrix1,
               valuetype System.Numerics.Matrix4x4/*02000003*/ matrix2,
               float32 amount) cil managed
  // SIG: 00 03 11 0C 11 0C 11 0C 0C
  {
    // Method begins at RVA 0x21ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Lerp

  .method /*06000080*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Multiply(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                   valuetype System.Numerics.Matrix4x4/*02000003*/ value2) cil managed
  // SIG: 00 02 11 0C 11 0C 11 0C
  {
    // Method begins at RVA 0x21cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Multiply

  .method /*06000081*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Multiply(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                   float32 value2) cil managed
  // SIG: 00 02 11 0C 11 0C 0C
  {
    // Method begins at RVA 0x21d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Multiply

  .method /*06000082*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Negate(valuetype System.Numerics.Matrix4x4/*02000003*/ 'value') cil managed
  // SIG: 00 01 11 0C 11 0C
  {
    // Method begins at RVA 0x21d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Negate

  .method /*06000083*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          op_Addition(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                      valuetype System.Numerics.Matrix4x4/*02000003*/ value2) cil managed
  // SIG: 00 02 11 0C 11 0C 11 0C
  {
    // Method begins at RVA 0x21d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::op_Addition

  .method /*06000084*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                            valuetype System.Numerics.Matrix4x4/*02000003*/ value2) cil managed
  // SIG: 00 02 02 11 0C 11 0C
  {
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::op_Equality

  .method /*06000085*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                              valuetype System.Numerics.Matrix4x4/*02000003*/ value2) cil managed
  // SIG: 00 02 02 11 0C 11 0C
  {
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::op_Inequality

  .method /*06000086*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          op_Multiply(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                      valuetype System.Numerics.Matrix4x4/*02000003*/ value2) cil managed
  // SIG: 00 02 11 0C 11 0C 11 0C
  {
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::op_Multiply

  .method /*06000087*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          op_Multiply(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                      float32 value2) cil managed
  // SIG: 00 02 11 0C 11 0C 0C
  {
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::op_Multiply

  .method /*06000088*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          op_Subtraction(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                         valuetype System.Numerics.Matrix4x4/*02000003*/ value2) cil managed
  // SIG: 00 02 11 0C 11 0C 11 0C
  {
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::op_Subtraction

  .method /*06000089*/ public hidebysig specialname static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          op_UnaryNegation(valuetype System.Numerics.Matrix4x4/*02000003*/ 'value') cil managed
  // SIG: 00 01 11 0C 11 0C
  {
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::op_UnaryNegation

  .method /*0600008A*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Subtract(valuetype System.Numerics.Matrix4x4/*02000003*/ value1,
                   valuetype System.Numerics.Matrix4x4/*02000003*/ value2) cil managed
  // SIG: 00 02 11 0C 11 0C 11 0C
  {
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Subtract

  .method /*0600008B*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00006C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00006D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::ToString

  .method /*0600008C*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Transform(valuetype System.Numerics.Matrix4x4/*02000003*/ 'value',
                    valuetype System.Numerics.Quaternion/*02000005*/ rotation) cil managed
  // SIG: 00 02 11 0C 11 0C 11 14
  {
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Transform

  .method /*0600008D*/ public hidebysig static 
          valuetype System.Numerics.Matrix4x4/*02000003*/ 
          Transpose(valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 01 11 0C 11 0C
  {
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::Transpose

  .method /*0600008E*/ public hidebysig instance valuetype System.Numerics.Matrix4x4/*02000003*/ 
          WithElement(int32 row,
                      int32 column,
                      float32 'value') cil managed
  // SIG: 20 03 11 0C 08 08 0C
  {
    .custom /*0C00006E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::WithElement

  .method /*0600008F*/ public hidebysig instance valuetype System.Numerics.Matrix4x4/*02000003*/ 
          WithRow(int32 index,
                  valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 20 02 11 0C 08 11 24
  {
    .custom /*0C00006F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Matrix4x4::WithRow

  .property /*17000009*/ callconv(8) valuetype System.Numerics.Matrix4x4/*02000003*/
          Identity()
  {
    .get valuetype System.Numerics.Matrix4x4/*02000003*/ System.Numerics.Matrix4x4/*02000003*/::get_Identity() /* 06000039 */
  } // end of property Matrix4x4::Identity
  .property /*1700000A*/ instance callconv(8) bool
          IsIdentity()
  {
    .get instance bool System.Numerics.Matrix4x4/*02000003*/::get_IsIdentity() /* 0600003A */
  } // end of property Matrix4x4::IsIdentity
  .property /*1700000B*/ instance callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          Translation()
  {
    .get instance valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Matrix4x4/*02000003*/::get_Translation() /* 0600003B */
    .set instance void System.Numerics.Matrix4x4/*02000003*/::set_Translation(valuetype System.Numerics.Vector3/*02000008*/) /* 0600003C */
  } // end of property Matrix4x4::Translation
  .property /*1700000C*/ instance callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          X()
  {
    .get instance valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Matrix4x4/*02000003*/::get_X() /* 0600003D */
    .set instance void System.Numerics.Matrix4x4/*02000003*/::set_X(valuetype System.Numerics.Vector4/*02000009*/) /* 0600003E */
  } // end of property Matrix4x4::X
  .property /*1700000D*/ instance callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          Y()
  {
    .get instance valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Matrix4x4/*02000003*/::get_Y() /* 0600003F */
    .set instance void System.Numerics.Matrix4x4/*02000003*/::set_Y(valuetype System.Numerics.Vector4/*02000009*/) /* 06000040 */
  } // end of property Matrix4x4::Y
  .property /*1700000E*/ instance callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          Z()
  {
    .get instance valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Matrix4x4/*02000003*/::get_Z() /* 06000041 */
    .set instance void System.Numerics.Matrix4x4/*02000003*/::set_Z(valuetype System.Numerics.Vector4/*02000009*/) /* 06000042 */
  } // end of property Matrix4x4::Z
  .property /*1700000F*/ instance callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          W()
  {
    .get instance valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Matrix4x4/*02000003*/::get_W() /* 06000043 */
    .set instance void System.Numerics.Matrix4x4/*02000003*/::set_W(valuetype System.Numerics.Vector4/*02000009*/) /* 06000044 */
  } // end of property Matrix4x4::W
  .property /*17000010*/ instance callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          Item(int32)
  {
    .get instance valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Matrix4x4/*02000003*/::get_Item(int32) /* 06000045 */
    .set instance void System.Numerics.Matrix4x4/*02000003*/::set_Item(int32,
                                                                       valuetype System.Numerics.Vector4/*02000009*/) /* 06000046 */
  } // end of property Matrix4x4::Item
  .property /*17000011*/ instance callconv(8) float32
          Item(int32,
               int32)
  {
    .get instance float32 System.Numerics.Matrix4x4/*02000003*/::get_Item(int32,
                                                                          int32) /* 06000047 */
    .set instance void System.Numerics.Matrix4x4/*02000003*/::set_Item(int32,
                                                                       int32,
                                                                       float32) /* 06000048 */
  } // end of property Matrix4x4::Item
} // end of class System.Numerics.Matrix4x4

.class /*02000004*/ public sequential ansi sealed beforefieldinit System.Numerics.Plane
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Plane/*02000004*/>/*1B000003*/
{
  .field /*04000017*/ public valuetype System.Numerics.Vector3/*02000008*/ Normal
  .field /*04000018*/ public float32 D
  .method /*06000090*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::.ctor

  .method /*06000091*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Numerics.Vector3/*02000008*/ normal,
                               float32 d) cil managed
  // SIG: 20 02 01 11 20 0C
  {
    // Method begins at RVA 0x2200
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::.ctor

  .method /*06000092*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 x,
                               float32 y,
                               float32 z,
                               float32 d) cil managed
  // SIG: 20 04 01 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::.ctor

  .method /*06000093*/ public hidebysig static 
          valuetype System.Numerics.Plane/*02000004*/ 
          Create(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 10 11 24
  {
    // Method begins at RVA 0x2206
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Create

  .method /*06000094*/ public hidebysig static 
          valuetype System.Numerics.Plane/*02000004*/ 
          Create(valuetype System.Numerics.Vector3/*02000008*/ normal,
                 float32 d) cil managed
  // SIG: 00 02 11 10 11 20 0C
  {
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Create

  .method /*06000095*/ public hidebysig static 
          valuetype System.Numerics.Plane/*02000004*/ 
          Create(float32 x,
                 float32 y,
                 float32 z,
                 float32 d) cil managed
  // SIG: 00 04 11 10 0C 0C 0C 0C
  {
    // Method begins at RVA 0x220c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Create

  .method /*06000096*/ public hidebysig static 
          valuetype System.Numerics.Plane/*02000004*/ 
          CreateFromVertices(valuetype System.Numerics.Vector3/*02000008*/ point1,
                             valuetype System.Numerics.Vector3/*02000008*/ point2,
                             valuetype System.Numerics.Vector3/*02000008*/ point3) cil managed
  // SIG: 00 03 11 10 11 20 11 20 11 20
  {
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::CreateFromVertices

  .method /*06000097*/ public hidebysig static 
          float32  Dot(valuetype System.Numerics.Plane/*02000004*/ plane,
                       valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 02 0C 11 10 11 24
  {
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Dot

  .method /*06000098*/ public hidebysig static 
          float32  DotCoordinate(valuetype System.Numerics.Plane/*02000004*/ plane,
                                 valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 02 0C 11 10 11 20
  {
    // Method begins at RVA 0x2215
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::DotCoordinate

  .method /*06000099*/ public hidebysig static 
          float32  DotNormal(valuetype System.Numerics.Plane/*02000004*/ plane,
                             valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 02 0C 11 10 11 20
  {
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::DotNormal

  .method /*0600009A*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Numerics.Plane/*02000004*/ other) cil managed
  // SIG: 20 01 02 11 10
  {
    .custom /*0C000070:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Equals

  .method /*0600009B*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000071:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000072:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000152*/ 
    .custom /*0C0000DB:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001C*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Equals

  .method /*0600009C*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000073:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::GetHashCode

  .method /*0600009D*/ public hidebysig static 
          valuetype System.Numerics.Plane/*02000004*/ 
          Normalize(valuetype System.Numerics.Plane/*02000004*/ 'value') cil managed
  // SIG: 00 01 11 10 11 10
  {
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Normalize

  .method /*0600009E*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Numerics.Plane/*02000004*/ value1,
                            valuetype System.Numerics.Plane/*02000004*/ value2) cil managed
  // SIG: 00 02 02 11 10 11 10
  {
    // Method begins at RVA 0x2227
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::op_Equality

  .method /*0600009F*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Numerics.Plane/*02000004*/ value1,
                              valuetype System.Numerics.Plane/*02000004*/ value2) cil managed
  // SIG: 00 02 02 11 10 11 10
  {
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::op_Inequality

  .method /*060000A0*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000074:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000075:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x222d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::ToString

  .method /*060000A1*/ public hidebysig static 
          valuetype System.Numerics.Plane/*02000004*/ 
          Transform(valuetype System.Numerics.Plane/*02000004*/ plane,
                    valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 02 11 10 11 10 11 0C
  {
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Transform

  .method /*060000A2*/ public hidebysig static 
          valuetype System.Numerics.Plane/*02000004*/ 
          Transform(valuetype System.Numerics.Plane/*02000004*/ plane,
                    valuetype System.Numerics.Quaternion/*02000005*/ rotation) cil managed
  // SIG: 00 02 11 10 11 10 11 14
  {
    // Method begins at RVA 0x2233
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Plane::Transform

} // end of class System.Numerics.Plane

.class /*02000005*/ public sequential ansi sealed beforefieldinit System.Numerics.Quaternion
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Quaternion/*02000005*/>/*1B000004*/
{
  .custom /*0C00001D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .field /*04000019*/ public float32 X
  .field /*0400001A*/ public float32 Y
  .field /*0400001B*/ public float32 Z
  .field /*0400001C*/ public float32 W
  .method /*060000A3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Numerics.Vector3/*02000008*/ vectorPart,
                               float32 scalarPart) cil managed
  // SIG: 20 02 01 11 20 0C
  {
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::.ctor

  .method /*060000A4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 x,
                               float32 y,
                               float32 z,
                               float32 w) cil managed
  // SIG: 20 04 01 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::.ctor

  .method /*060000A5*/ public hidebysig specialname static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          get_Zero() cil managed
  // SIG: 00 00 11 14
  {
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::get_Zero

  .method /*060000A6*/ public hidebysig specialname static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          get_Identity() cil managed
  // SIG: 00 00 11 14
  {
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::get_Identity

  .method /*060000A7*/ public hidebysig specialname 
          instance float32  get_Item(int32 index) cil managed
  // SIG: 20 01 0C 08
  {
    .custom /*0C000076:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::get_Item

  .method /*060000A8*/ public hidebysig specialname 
          instance void  set_Item(int32 index,
                                  float32 'value') cil managed
  // SIG: 20 02 01 08 0C
  {
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::set_Item

  .method /*060000A9*/ public hidebysig specialname 
          instance bool  get_IsIdentity() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000077:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::get_IsIdentity

  .method /*060000AA*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Add(valuetype System.Numerics.Quaternion/*02000005*/ value1,
              valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Add

  .method /*060000AB*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Concatenate(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                      valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Concatenate

  .method /*060000AC*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Conjugate(valuetype System.Numerics.Quaternion/*02000005*/ 'value') cil managed
  // SIG: 00 01 11 14 11 14
  {
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Conjugate

  .method /*060000AD*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Create(valuetype System.Numerics.Vector3/*02000008*/ vectorPart,
                 float32 scalarPart) cil managed
  // SIG: 00 02 11 14 11 20 0C
  {
    // Method begins at RVA 0x2254
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Create

  .method /*060000AE*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Create(float32 x,
                 float32 y,
                 float32 z,
                 float32 w) cil managed
  // SIG: 00 04 11 14 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Create

  .method /*060000AF*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          CreateFromAxisAngle(valuetype System.Numerics.Vector3/*02000008*/ axis,
                              float32 angle) cil managed
  // SIG: 00 02 11 14 11 20 0C
  {
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::CreateFromAxisAngle

  .method /*060000B0*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          CreateFromRotationMatrix(valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 01 11 14 11 0C
  {
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::CreateFromRotationMatrix

  .method /*060000B1*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          CreateFromYawPitchRoll(float32 yaw,
                                 float32 pitch,
                                 float32 roll) cil managed
  // SIG: 00 03 11 14 0C 0C 0C
  {
    // Method begins at RVA 0x2260
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::CreateFromYawPitchRoll

  .method /*060000B2*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Divide(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                 valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Divide

  .method /*060000B3*/ public hidebysig static 
          float32  Dot(valuetype System.Numerics.Quaternion/*02000005*/ quaternion1,
                       valuetype System.Numerics.Quaternion/*02000005*/ quaternion2) cil managed
  // SIG: 00 02 0C 11 14 11 14
  {
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Dot

  .method /*060000B4*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Numerics.Quaternion/*02000005*/ other) cil managed
  // SIG: 20 01 02 11 14
  {
    .custom /*0C000078:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Equals

  .method /*060000B5*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000079:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00007A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800017B*/ 
    .custom /*0C0000FD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001C*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Equals

  .method /*060000B6*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00007B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::GetHashCode

  .method /*060000B7*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Inverse(valuetype System.Numerics.Quaternion/*02000005*/ 'value') cil managed
  // SIG: 00 01 11 14 11 14
  {
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Inverse

  .method /*060000B8*/ public hidebysig instance float32 
          Length() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C00007C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Length

  .method /*060000B9*/ public hidebysig instance float32 
          LengthSquared() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C00007D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::LengthSquared

  .method /*060000BA*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Lerp(valuetype System.Numerics.Quaternion/*02000005*/ quaternion1,
               valuetype System.Numerics.Quaternion/*02000005*/ quaternion2,
               float32 amount) cil managed
  // SIG: 00 03 11 14 11 14 11 14 0C
  {
    // Method begins at RVA 0x227b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Lerp

  .method /*060000BB*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Multiply(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                   valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x227e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Multiply

  .method /*060000BC*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Multiply(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                   float32 value2) cil managed
  // SIG: 00 02 11 14 11 14 0C
  {
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Multiply

  .method /*060000BD*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Negate(valuetype System.Numerics.Quaternion/*02000005*/ 'value') cil managed
  // SIG: 00 01 11 14 11 14
  {
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Negate

  .method /*060000BE*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Normalize(valuetype System.Numerics.Quaternion/*02000005*/ 'value') cil managed
  // SIG: 00 01 11 14 11 14
  {
    // Method begins at RVA 0x2287
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Normalize

  .method /*060000BF*/ public hidebysig specialname static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          op_Addition(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                      valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x228a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::op_Addition

  .method /*060000C0*/ public hidebysig specialname static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          op_Division(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                      valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x228d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::op_Division

  .method /*060000C1*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                            valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 02 11 14 11 14
  {
    // Method begins at RVA 0x2290
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::op_Equality

  .method /*060000C2*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                              valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 02 11 14 11 14
  {
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::op_Inequality

  .method /*060000C3*/ public hidebysig specialname static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          op_Multiply(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                      valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x2296
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::op_Multiply

  .method /*060000C4*/ public hidebysig specialname static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          op_Multiply(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                      float32 value2) cil managed
  // SIG: 00 02 11 14 11 14 0C
  {
    // Method begins at RVA 0x2299
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::op_Multiply

  .method /*060000C5*/ public hidebysig specialname static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          op_Subtraction(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                         valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x229c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::op_Subtraction

  .method /*060000C6*/ public hidebysig specialname static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          op_UnaryNegation(valuetype System.Numerics.Quaternion/*02000005*/ 'value') cil managed
  // SIG: 00 01 11 14 11 14
  {
    // Method begins at RVA 0x229f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::op_UnaryNegation

  .method /*060000C7*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Slerp(valuetype System.Numerics.Quaternion/*02000005*/ quaternion1,
                valuetype System.Numerics.Quaternion/*02000005*/ quaternion2,
                float32 amount) cil managed
  // SIG: 00 03 11 14 11 14 11 14 0C
  {
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Slerp

  .method /*060000C8*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          Subtract(valuetype System.Numerics.Quaternion/*02000005*/ value1,
                   valuetype System.Numerics.Quaternion/*02000005*/ value2) cil managed
  // SIG: 00 02 11 14 11 14 11 14
  {
    // Method begins at RVA 0x22a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::Subtract

  .method /*060000C9*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00007F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Quaternion::ToString

  .property /*17000012*/ callconv(8) valuetype System.Numerics.Quaternion/*02000005*/
          Zero()
  {
    .get valuetype System.Numerics.Quaternion/*02000005*/ System.Numerics.Quaternion/*02000005*/::get_Zero() /* 060000A5 */
  } // end of property Quaternion::Zero
  .property /*17000013*/ callconv(8) valuetype System.Numerics.Quaternion/*02000005*/
          Identity()
  {
    .get valuetype System.Numerics.Quaternion/*02000005*/ System.Numerics.Quaternion/*02000005*/::get_Identity() /* 060000A6 */
  } // end of property Quaternion::Identity
  .property /*17000014*/ instance callconv(8) float32
          Item(int32)
  {
    .get instance float32 System.Numerics.Quaternion/*02000005*/::get_Item(int32) /* 060000A7 */
    .set instance void System.Numerics.Quaternion/*02000005*/::set_Item(int32,
                                                                        float32) /* 060000A8 */
  } // end of property Quaternion::Item
  .property /*17000015*/ instance callconv(8) bool
          IsIdentity()
  {
    .get instance bool System.Numerics.Quaternion/*02000005*/::get_IsIdentity() /* 060000A9 */
  } // end of property Quaternion::IsIdentity
} // end of class System.Numerics.Quaternion

.class /*02000006*/ public abstract auto ansi sealed beforefieldinit System.Numerics.Vector
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000020:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000CA*/ public hidebysig specialname static 
          bool  get_IsHardwareAccelerated() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::get_IsHardwareAccelerated

  .method /*060000CB*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Abs<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C000080:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800019A*/ 
    .custom /*0C000109:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800019B*/ 
    .custom /*0C00010B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Abs

  .method /*060000CC*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Add<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                 valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C000081:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800019C*/ 
    .custom /*0C00010D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800019D*/ 
    .custom /*0C00010F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800019E*/ 
    .custom /*0C000111:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Add

  .method /*060000CD*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          AddSaturate<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                         valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C000082:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800019F*/ 
    .custom /*0C000112:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001A0*/ 
    .custom /*0C000113:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001A1*/ 
    .custom /*0C000114:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AddSaturate

  .method /*060000CE*/ public hidebysig static 
          bool  All<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector',
                       !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C000083:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000005*/ 
      .custom /*0C00001E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001A2*/ 
    .custom /*0C000115:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::All

  .method /*060000CF*/ public hidebysig static 
          bool  AllWhereAllBitsSet<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 02 15 11 28 01 1E 00
  {
    .custom /*0C000084:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001A4*/ 
    .custom /*0C00011A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AllWhereAllBitsSet

  .method /*060000D0*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          AndNot<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                    valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C000085:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001A5*/ 
    .custom /*0C00011D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001A6*/ 
    .custom /*0C000120:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001A7*/ 
    .custom /*0C000123:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AndNot

  .method /*060000D1*/ public hidebysig static 
          bool  Any<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector',
                       !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C000086:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000008*/ 
      .custom /*0C000024:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001A8*/ 
    .custom /*0C000127:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Any

  .method /*060000D2*/ public hidebysig static 
          bool  AnyWhereAllBitsSet<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 02 15 11 28 01 1E 00
  {
    .custom /*0C000087:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001AA*/ 
    .custom /*0C00012C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AnyWhereAllBitsSet

  .method /*060000D3*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!TTo> 
          As<TFrom,TTo>(valuetype System.Numerics.Vector`1/*0200000A*/<!!TFrom> 'vector') cil managed
  // SIG: 10 02 01 15 11 28 01 1E 01 15 11 28 01 1E 00
  {
    .custom /*0C000088:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000089:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*080001AB*/ 
    .custom /*0C00012F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001AC*/ 
    .custom /*0C000133:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::As

  .method /*060000D4*/ public hidebysig static 
          valuetype System.Numerics.Plane/*02000004*/ 
          AsPlane(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 10 11 24
  {
    .custom /*0C00008A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsPlane

  .method /*060000D5*/ public hidebysig static 
          valuetype System.Numerics.Quaternion/*02000005*/ 
          AsQuaternion(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 14 11 24
  {
    .custom /*0C00008B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsQuaternion

  .method /*060000D6*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          AsVector2(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 20
  {
    .custom /*0C00008C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector2

  .method /*060000D7*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          AsVector2(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 24
  {
    .custom /*0C00008D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector2

  .method /*060000D8*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          AsVector3(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 20 11 1C
  {
    .custom /*0C00008E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector3

  .method /*060000D9*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          AsVector3(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 20 11 24
  {
    .custom /*0C00008F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector3

  .method /*060000DA*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          AsVector3Unsafe(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 20 11 1C
  {
    .custom /*0C000090:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector3Unsafe

  .method /*060000DB*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          AsVector4(valuetype System.Numerics.Plane/*02000004*/ 'value') cil managed
  // SIG: 00 01 11 24 11 10
  {
    .custom /*0C000091:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector4

  .method /*060000DC*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          AsVector4(valuetype System.Numerics.Quaternion/*02000005*/ 'value') cil managed
  // SIG: 00 01 11 24 11 14
  {
    .custom /*0C000092:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector4

  .method /*060000DD*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          AsVector4(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 24 11 1C
  {
    .custom /*0C000093:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector4

  .method /*060000DE*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          AsVector4(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 24 11 20
  {
    .custom /*0C000094:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector4

  .method /*060000DF*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          AsVector4Unsafe(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 24 11 1C
  {
    .custom /*0C000095:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector4Unsafe

  .method /*060000E0*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          AsVector4Unsafe(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 24 11 20
  {
    .custom /*0C000096:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVector4Unsafe

  .method /*060000E1*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint8> 
          AsVectorByte<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 05 15 11 28 01 1E 00
  {
    .param type T /*2A00000C*/ 
      .custom /*0C00002B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001BA*/ 
    .custom /*0C00014B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorByte

  .method /*060000E2*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          AsVectorDouble<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 0D 15 11 28 01 1E 00
  {
    .param type T /*2A00000D*/ 
      .custom /*0C00002C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001BB*/ 
    .custom /*0C00014E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorDouble

  .method /*060000E3*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          AsVectorInt16<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 06 15 11 28 01 1E 00
  {
    .param type T /*2A00000E*/ 
      .custom /*0C00002E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001BC*/ 
    .custom /*0C000150:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorInt16

  .method /*060000E4*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          AsVectorInt32<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 08 15 11 28 01 1E 00
  {
    .param type T /*2A00000F*/ 
      .custom /*0C00002F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001BD*/ 
    .custom /*0C000152:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorInt32

  .method /*060000E5*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          AsVectorInt64<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 0A 15 11 28 01 1E 00
  {
    .param type T /*2A000010*/ 
      .custom /*0C000030:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001BE*/ 
    .custom /*0C000154:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorInt64

  .method /*060000E6*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native int> 
          AsVectorNInt<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 18 15 11 28 01 1E 00
  {
    .param type T /*2A000011*/ 
      .custom /*0C000031:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001BF*/ 
    .custom /*0C000155:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x22ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorNInt

  .method /*060000E7*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native uint> 
          AsVectorNUInt<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 19 15 11 28 01 1E 00
  {
    .custom /*0C000097:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000012*/ 
      .custom /*0C000032:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C0*/ 
    .custom /*0C000156:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2302
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorNUInt

  .method /*060000E8*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int8> 
          AsVectorSByte<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 04 15 11 28 01 1E 00
  {
    .custom /*0C000098:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000013*/ 
      .custom /*0C000033:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C1*/ 
    .custom /*0C000158:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2305
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorSByte

  .method /*060000E9*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          AsVectorSingle<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 0C 15 11 28 01 1E 00
  {
    .param type T /*2A000014*/ 
      .custom /*0C000034:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C2*/ 
    .custom /*0C000159:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2308
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorSingle

  .method /*060000EA*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 
          AsVectorUInt16<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 07 15 11 28 01 1E 00
  {
    .custom /*0C000099:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000015*/ 
      .custom /*0C000035:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C3*/ 
    .custom /*0C00015A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x230b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorUInt16

  .method /*060000EB*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          AsVectorUInt32<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 09 15 11 28 01 1E 00
  {
    .custom /*0C00009A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000016*/ 
      .custom /*0C000036:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C4*/ 
    .custom /*0C00015C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x230e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorUInt32

  .method /*060000EC*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 
          AsVectorUInt64<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 0B 15 11 28 01 1E 00
  {
    .custom /*0C00009B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000017*/ 
      .custom /*0C000037:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001C5*/ 
    .custom /*0C00015D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::AsVectorUInt64

  .method /*060000ED*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          BitwiseAnd<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                        valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C00009C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001C6*/ 
    .custom /*0C00015F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001C7*/ 
    .custom /*0C000161:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001C8*/ 
    .custom /*0C000163:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2314
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::BitwiseAnd

  .method /*060000EE*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          BitwiseOr<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                       valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C00009D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001C9*/ 
    .custom /*0C000164:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001CA*/ 
    .custom /*0C000165:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001CB*/ 
    .custom /*0C000167:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2317
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::BitwiseOr

  .method /*060000EF*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Ceiling(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x231a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Ceiling

  .method /*060000F0*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Ceiling(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x231d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Ceiling

  .method /*060000F1*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Clamp<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value',
                   valuetype System.Numerics.Vector`1/*0200000A*/<!!T> min,
                   valuetype System.Numerics.Vector`1/*0200000A*/<!!T> max) cil managed
  // SIG: 10 01 03 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C00009E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001CE*/ 
    .custom /*0C00016A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001CF*/ 
    .custom /*0C00016C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001D0*/ 
    .custom /*0C00016D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*080001D1*/ 
    .custom /*0C00016E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2320
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Clamp

  .method /*060000F2*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          ClampNative<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value',
                         valuetype System.Numerics.Vector`1/*0200000A*/<!!T> min,
                         valuetype System.Numerics.Vector`1/*0200000A*/<!!T> max) cil managed
  // SIG: 10 01 03 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C00009F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001D2*/ 
    .custom /*0C000170:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001D3*/ 
    .custom /*0C000171:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001D4*/ 
    .custom /*0C000173:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*080001D5*/ 
    .custom /*0C000175:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2323
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ClampNative

  .method /*060000F3*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          ConditionalSelect(valuetype System.Numerics.Vector`1/*0200000A*/<int32> condition,
                            valuetype System.Numerics.Vector`1/*0200000A*/<float32> left,
                            valuetype System.Numerics.Vector`1/*0200000A*/<float32> right) cil managed
  // SIG: 00 03 15 11 28 01 0C 15 11 28 01 08 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2326
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConditionalSelect

  .method /*060000F4*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          ConditionalSelect(valuetype System.Numerics.Vector`1/*0200000A*/<int64> condition,
                            valuetype System.Numerics.Vector`1/*0200000A*/<float64> left,
                            valuetype System.Numerics.Vector`1/*0200000A*/<float64> right) cil managed
  // SIG: 00 03 15 11 28 01 0D 15 11 28 01 0A 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2329
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConditionalSelect

  .method /*060000F5*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          ConditionalSelect<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> condition,
                               valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                               valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 03 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000A0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001DC*/ 
    .custom /*0C00017C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001DD*/ 
    .custom /*0C00017D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001DE*/ 
    .custom /*0C00017E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*080001DF*/ 
    .custom /*0C00017F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x232c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConditionalSelect

  .method /*060000F6*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          ConvertToDouble(valuetype System.Numerics.Vector`1/*0200000A*/<int64> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0A
  {
    // Method begins at RVA 0x232f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToDouble

  .method /*060000F7*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          ConvertToDouble(valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0B
  {
    .custom /*0C0000A1:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2332
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToDouble

  .method /*060000F8*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          ConvertToInt32(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'value') cil managed
  // SIG: 00 01 15 11 28 01 08 15 11 28 01 0C
  {
    // Method begins at RVA 0x2335
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToInt32

  .method /*060000F9*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          ConvertToInt32Native(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'value') cil managed
  // SIG: 00 01 15 11 28 01 08 15 11 28 01 0C
  {
    // Method begins at RVA 0x2338
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToInt32Native

  .method /*060000FA*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          ConvertToInt64(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0A 15 11 28 01 0D
  {
    // Method begins at RVA 0x233b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToInt64

  .method /*060000FB*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          ConvertToInt64Native(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0A 15 11 28 01 0D
  {
    // Method begins at RVA 0x233e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToInt64Native

  .method /*060000FC*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          ConvertToSingle(valuetype System.Numerics.Vector`1/*0200000A*/<int32> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 08
  {
    // Method begins at RVA 0x2341
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToSingle

  .method /*060000FD*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          ConvertToSingle(valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 09
  {
    .custom /*0C0000A2:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2344
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToSingle

  .method /*060000FE*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          ConvertToUInt32(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'value') cil managed
  // SIG: 00 01 15 11 28 01 09 15 11 28 01 0C
  {
    .custom /*0C0000A3:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2347
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToUInt32

  .method /*060000FF*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          ConvertToUInt32Native(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'value') cil managed
  // SIG: 00 01 15 11 28 01 09 15 11 28 01 0C
  {
    .custom /*0C0000A4:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x234a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToUInt32Native

  .method /*06000100*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 
          ConvertToUInt64(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0B 15 11 28 01 0D
  {
    .custom /*0C0000A5:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x234d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToUInt64

  .method /*06000101*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 
          ConvertToUInt64Native(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0B 15 11 28 01 0D
  {
    .custom /*0C0000A6:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2350
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ConvertToUInt64Native

  .method /*06000102*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          CopySign<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value',
                      valuetype System.Numerics.Vector`1/*0200000A*/<!!T> sign) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000A7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001EC*/ 
    .custom /*0C000181:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001ED*/ 
    .custom /*0C000182:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080001EE*/ 
    .custom /*0C000183:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2353
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::CopySign

  .method /*06000103*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Cos(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2356
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Cos

  .method /*06000104*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Cos(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2359
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Cos

  .method /*06000105*/ public hidebysig static 
          int32  Count<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector',
                          !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000A8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A00001E*/ 
      .custom /*0C000038:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001F1*/ 
    .custom /*0C000184:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x235c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Count

  .method /*06000106*/ public hidebysig static 
          int32  CountWhereAllBitsSet<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 08 15 11 28 01 1E 00
  {
    .custom /*0C0000A9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080001F3*/ 
    .custom /*0C000185:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x235f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::CountWhereAllBitsSet

  .method /*06000107*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Create<T>(!!T 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000AA:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000020*/ 
      .custom /*0C000039:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001F4*/ 
    .custom /*0C000186:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2362
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Create

  .method /*06000108*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Create<T>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<!!T> values) cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 7D 01 1E 00
  {
    .custom /*0C0000AB:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001F6*/ 
    .custom /*0C000187:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080001F7*/ 
    .custom /*0C000188:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2365
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Create

  .method /*06000109*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          CreateScalar<T>(!!T 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000AC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000022*/ 
      .custom /*0C00003D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001F8*/ 
    .custom /*0C000189:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2368
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::CreateScalar

  .method /*0600010A*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          CreateScalarUnsafe<T>(!!T 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000AD:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000023*/ 
      .custom /*0C00003F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001FA*/ 
    .custom /*0C00018A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x236b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::CreateScalarUnsafe

  .method /*0600010B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          CreateSequence<T>(!!T start,
                            !!T step) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 1E 00 1E 00
  {
    .custom /*0C0000AE:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000024*/ 
      .custom /*0C000041:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001FC*/ 
    .custom /*0C00018D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x236e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::CreateSequence

  .method /*0600010C*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          DegreesToRadians(valuetype System.Numerics.Vector`1/*0200000A*/<float64> degrees) cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::DegreesToRadians

  .method /*0600010D*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          DegreesToRadians(valuetype System.Numerics.Vector`1/*0200000A*/<float32> degrees) cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2374
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::DegreesToRadians

  .method /*0600010E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Divide<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                    valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000AF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000201*/ 
    .custom /*0C000191:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000202*/ 
    .custom /*0C000192:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000203*/ 
    .custom /*0C000193:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Divide

  .method /*0600010F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Divide<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                    !!T right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000B0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000026*/ 
      .custom /*0C000044:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000204*/ 
    .custom /*0C000194:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000205*/ 
    .custom /*0C000196:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Divide

  .method /*06000110*/ public hidebysig static 
          !!T  Dot<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000B1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000027*/ 
      .custom /*0C000045:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000207*/ 
    .custom /*0C000197:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000208*/ 
    .custom /*0C000198:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x237d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Dot

  .method /*06000111*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          Equals(valuetype System.Numerics.Vector`1/*0200000A*/<float64> left,
                 valuetype System.Numerics.Vector`1/*0200000A*/<float64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2380
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Equals

  .method /*06000112*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          Equals(valuetype System.Numerics.Vector`1/*0200000A*/<int32> left,
                 valuetype System.Numerics.Vector`1/*0200000A*/<int32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 08 15 11 28 01 08
  {
    // Method begins at RVA 0x2383
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Equals

  .method /*06000113*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          Equals(valuetype System.Numerics.Vector`1/*0200000A*/<int64> left,
                 valuetype System.Numerics.Vector`1/*0200000A*/<int64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0A 15 11 28 01 0A
  {
    // Method begins at RVA 0x2386
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Equals

  .method /*06000114*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          Equals(valuetype System.Numerics.Vector`1/*0200000A*/<float32> left,
                 valuetype System.Numerics.Vector`1/*0200000A*/<float32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2389
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Equals

  .method /*06000115*/ public hidebysig static 
          bool  EqualsAll<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                             valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000B2:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000211*/ 
    .custom /*0C00019C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000212*/ 
    .custom /*0C00019D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x238c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::EqualsAll

  .method /*06000116*/ public hidebysig static 
          bool  EqualsAny<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                             valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000B4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000213*/ 
    .custom /*0C00019E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000214*/ 
    .custom /*0C0001A0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x238f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::EqualsAny

  .method /*06000117*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Equals<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                    valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000B5:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000215*/ 
    .custom /*0C0001A1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000216*/ 
    .custom /*0C0001A2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000217*/ 
    .custom /*0C0001A3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2392
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Equals

  .method /*06000118*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Exp(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2395
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Exp

  .method /*06000119*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Exp(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2398
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Exp

  .method /*0600011A*/ public hidebysig static 
          uint32  ExtractMostSignificantBits(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 09 11 1C
  {
    .custom /*0C0000B6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B7:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x239b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ExtractMostSignificantBits

  .method /*0600011B*/ public hidebysig static 
          uint32  ExtractMostSignificantBits(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 09 11 20
  {
    .custom /*0C0000B8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B9:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x239e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ExtractMostSignificantBits

  .method /*0600011C*/ public hidebysig static 
          uint32  ExtractMostSignificantBits(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 09 11 24
  {
    .custom /*0C0000BA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BB:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x23a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ExtractMostSignificantBits

  .method /*0600011D*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Floor(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x23a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Floor

  .method /*0600011E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Floor(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x23a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Floor

  .method /*0600011F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          FusedMultiplyAdd(valuetype System.Numerics.Vector`1/*0200000A*/<float64> left,
                           valuetype System.Numerics.Vector`1/*0200000A*/<float64> right,
                           valuetype System.Numerics.Vector`1/*0200000A*/<float64> addend) cil managed
  // SIG: 00 03 15 11 28 01 0D 15 11 28 01 0D 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x23aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::FusedMultiplyAdd

  .method /*06000120*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          FusedMultiplyAdd(valuetype System.Numerics.Vector`1/*0200000A*/<float32> left,
                           valuetype System.Numerics.Vector`1/*0200000A*/<float32> right,
                           valuetype System.Numerics.Vector`1/*0200000A*/<float32> addend) cil managed
  // SIG: 00 03 15 11 28 01 0C 15 11 28 01 0C 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x23ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::FusedMultiplyAdd

  .method /*06000121*/ public hidebysig static 
          !!T  GetElement<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector',
                             int32 index) cil managed
  // SIG: 10 01 02 1E 00 15 11 28 01 1E 00 08
  {
    .custom /*0C0000BC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000BD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002B*/ 
      .custom /*0C000046:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000225*/ 
    .custom /*0C0001A4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GetElement

  .method /*06000122*/ public hidebysig static 
          float32  GetElement(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                              int32 index) cil managed
  // SIG: 00 02 0C 11 1C 08
  {
    .custom /*0C0000BE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GetElement

  .method /*06000123*/ public hidebysig static 
          float32  GetElement(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                              int32 index) cil managed
  // SIG: 00 02 0C 11 20 08
  {
    .custom /*0C0000BF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GetElement

  .method /*06000124*/ public hidebysig static 
          float32  GetElement(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                              int32 index) cil managed
  // SIG: 00 02 0C 11 24 08
  {
    .custom /*0C0000C0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GetElement

  .method /*06000125*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          GreaterThan(valuetype System.Numerics.Vector`1/*0200000A*/<float64> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<float64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x23bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThan

  .method /*06000126*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          GreaterThan(valuetype System.Numerics.Vector`1/*0200000A*/<int32> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<int32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 08 15 11 28 01 08
  {
    // Method begins at RVA 0x23bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThan

  .method /*06000127*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          GreaterThan(valuetype System.Numerics.Vector`1/*0200000A*/<int64> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<int64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0A 15 11 28 01 0A
  {
    // Method begins at RVA 0x23c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThan

  .method /*06000128*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          GreaterThan(valuetype System.Numerics.Vector`1/*0200000A*/<float32> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<float32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x23c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThan

  .method /*06000129*/ public hidebysig static 
          bool  GreaterThanAll<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                  valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000C1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000235*/ 
    .custom /*0C0001A7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000236*/ 
    .custom /*0C0001A8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanAll

  .method /*0600012A*/ public hidebysig static 
          bool  GreaterThanAny<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                  valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000C2:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000237*/ 
    .custom /*0C0001AA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000238*/ 
    .custom /*0C0001AC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanAny

  .method /*0600012B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          GreaterThanOrEqual(valuetype System.Numerics.Vector`1/*0200000A*/<float64> left,
                             valuetype System.Numerics.Vector`1/*0200000A*/<float64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x23ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanOrEqual

  .method /*0600012C*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          GreaterThanOrEqual(valuetype System.Numerics.Vector`1/*0200000A*/<int32> left,
                             valuetype System.Numerics.Vector`1/*0200000A*/<int32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 08 15 11 28 01 08
  {
    // Method begins at RVA 0x23d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanOrEqual

  .method /*0600012D*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          GreaterThanOrEqual(valuetype System.Numerics.Vector`1/*0200000A*/<int64> left,
                             valuetype System.Numerics.Vector`1/*0200000A*/<int64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0A 15 11 28 01 0A
  {
    // Method begins at RVA 0x23d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanOrEqual

  .method /*0600012E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          GreaterThanOrEqual(valuetype System.Numerics.Vector`1/*0200000A*/<float32> left,
                             valuetype System.Numerics.Vector`1/*0200000A*/<float32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x23d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanOrEqual

  .method /*0600012F*/ public hidebysig static 
          bool  GreaterThanOrEqualAll<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                         valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000C3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000241*/ 
    .custom /*0C0001AF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000242*/ 
    .custom /*0C0001B0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanOrEqualAll

  .method /*06000130*/ public hidebysig static 
          bool  GreaterThanOrEqualAny<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                         valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000C4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000243*/ 
    .custom /*0C0001B1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000244*/ 
    .custom /*0C0001B2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanOrEqualAny

  .method /*06000131*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          GreaterThanOrEqual<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000C5:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000245*/ 
    .custom /*0C0001B3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000246*/ 
    .custom /*0C0001B4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000247*/ 
    .custom /*0C0001B5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThanOrEqual

  .method /*06000132*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          GreaterThan<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                         valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000C6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000248*/ 
    .custom /*0C0001B6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000249*/ 
    .custom /*0C0001B7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800024A*/ 
    .custom /*0C0001B8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::GreaterThan

  .method /*06000133*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Hypot(valuetype System.Numerics.Vector`1/*0200000A*/<float64> x,
                valuetype System.Numerics.Vector`1/*0200000A*/<float64> y) cil managed
  // SIG: 00 02 15 11 28 01 0D 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x23e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Hypot

  .method /*06000134*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Hypot(valuetype System.Numerics.Vector`1/*0200000A*/<float32> x,
                valuetype System.Numerics.Vector`1/*0200000A*/<float32> y) cil managed
  // SIG: 00 02 15 11 28 01 0C 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x23e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Hypot

  .method /*06000135*/ public hidebysig static 
          int32  IndexOf<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector',
                            !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000C7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000032*/ 
      .custom /*0C000047:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800024F*/ 
    .custom /*0C0001B9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IndexOf

  .method /*06000136*/ public hidebysig static 
          int32  IndexOfWhereAllBitsSet<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 08 15 11 28 01 1E 00
  {
    .custom /*0C0000C8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000251*/ 
    .custom /*0C0001BA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IndexOfWhereAllBitsSet

  .method /*06000137*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsEvenInteger<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000C9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000252*/ 
    .custom /*0C0001BB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000253*/ 
    .custom /*0C0001BC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsEvenInteger

  .method /*06000138*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsFinite<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000CA:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000254*/ 
    .custom /*0C0001BD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000255*/ 
    .custom /*0C0001BE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsFinite

  .method /*06000139*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsInfinity<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000CB:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000256*/ 
    .custom /*0C0001BF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000257*/ 
    .custom /*0C0001C0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsInfinity

  .method /*0600013A*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsInteger<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000CC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000258*/ 
    .custom /*0C0001C1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000259*/ 
    .custom /*0C0001C2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsInteger

  .method /*0600013B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsNaN<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000CD:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800025A*/ 
    .custom /*0C0001C3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800025B*/ 
    .custom /*0C0001C4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x23fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsNaN

  .method /*0600013C*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsNegative<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000CE:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800025C*/ 
    .custom /*0C0001C5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800025D*/ 
    .custom /*0C0001C6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2401
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsNegative

  .method /*0600013D*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsNegativeInfinity<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000CF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800025E*/ 
    .custom /*0C0001C7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800025F*/ 
    .custom /*0C0001C8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2404
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsNegativeInfinity

  .method /*0600013E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsNormal<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000D0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000260*/ 
    .custom /*0C0001C9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000261*/ 
    .custom /*0C0001CA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2407
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsNormal

  .method /*0600013F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsOddInteger<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000D1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000262*/ 
    .custom /*0C0001CB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000263*/ 
    .custom /*0C0001CC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x240a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsOddInteger

  .method /*06000140*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsPositive<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000D2:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000264*/ 
    .custom /*0C0001CD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000265*/ 
    .custom /*0C0001CE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x240d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsPositive

  .method /*06000141*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsPositiveInfinity<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000D3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000266*/ 
    .custom /*0C0001CF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000267*/ 
    .custom /*0C0001D0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2410
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsPositiveInfinity

  .method /*06000142*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsSubnormal<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000D4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000268*/ 
    .custom /*0C0001D1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000269*/ 
    .custom /*0C0001D2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2413
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsSubnormal

  .method /*06000143*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          IsZero<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000D5:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800026A*/ 
    .custom /*0C0001D3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800026B*/ 
    .custom /*0C0001D4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2416
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::IsZero

  .method /*06000144*/ public hidebysig static 
          int32  LastIndexOf<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector',
                                !!T 'value') cil managed
  // SIG: 10 01 02 08 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000D6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000041*/ 
      .custom /*0C000052:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800026C*/ 
    .custom /*0C0001D5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2419
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LastIndexOf

  .method /*06000145*/ public hidebysig static 
          int32  LastIndexOfWhereAllBitsSet<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 08 15 11 28 01 1E 00
  {
    .custom /*0C0000D7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800026E*/ 
    .custom /*0C0001DA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x241c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LastIndexOfWhereAllBitsSet

  .method /*06000146*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Lerp(valuetype System.Numerics.Vector`1/*0200000A*/<float64> x,
               valuetype System.Numerics.Vector`1/*0200000A*/<float64> y,
               valuetype System.Numerics.Vector`1/*0200000A*/<float64> amount) cil managed
  // SIG: 00 03 15 11 28 01 0D 15 11 28 01 0D 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x241f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Lerp

  .method /*06000147*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Lerp(valuetype System.Numerics.Vector`1/*0200000A*/<float32> x,
               valuetype System.Numerics.Vector`1/*0200000A*/<float32> y,
               valuetype System.Numerics.Vector`1/*0200000A*/<float32> amount) cil managed
  // SIG: 00 03 15 11 28 01 0C 15 11 28 01 0C 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2422
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Lerp

  .method /*06000148*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          LessThan(valuetype System.Numerics.Vector`1/*0200000A*/<float64> left,
                   valuetype System.Numerics.Vector`1/*0200000A*/<float64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2425
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThan

  .method /*06000149*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          LessThan(valuetype System.Numerics.Vector`1/*0200000A*/<int32> left,
                   valuetype System.Numerics.Vector`1/*0200000A*/<int32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 08 15 11 28 01 08
  {
    // Method begins at RVA 0x2428
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThan

  .method /*0600014A*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          LessThan(valuetype System.Numerics.Vector`1/*0200000A*/<int64> left,
                   valuetype System.Numerics.Vector`1/*0200000A*/<int64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0A 15 11 28 01 0A
  {
    // Method begins at RVA 0x242b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThan

  .method /*0600014B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          LessThan(valuetype System.Numerics.Vector`1/*0200000A*/<float32> left,
                   valuetype System.Numerics.Vector`1/*0200000A*/<float32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x242e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThan

  .method /*0600014C*/ public hidebysig static 
          bool  LessThanAll<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                               valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000D8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800027D*/ 
    .custom /*0C0001DD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800027E*/ 
    .custom /*0C0001DE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2431
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanAll

  .method /*0600014D*/ public hidebysig static 
          bool  LessThanAny<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                               valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000D9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800027F*/ 
    .custom /*0C0001DF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000280*/ 
    .custom /*0C0001E0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2434
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanAny

  .method /*0600014E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          LessThanOrEqual(valuetype System.Numerics.Vector`1/*0200000A*/<float64> left,
                          valuetype System.Numerics.Vector`1/*0200000A*/<float64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2437
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanOrEqual

  .method /*0600014F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          LessThanOrEqual(valuetype System.Numerics.Vector`1/*0200000A*/<int32> left,
                          valuetype System.Numerics.Vector`1/*0200000A*/<int32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 08 15 11 28 01 08
  {
    // Method begins at RVA 0x243a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanOrEqual

  .method /*06000150*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          LessThanOrEqual(valuetype System.Numerics.Vector`1/*0200000A*/<int64> left,
                          valuetype System.Numerics.Vector`1/*0200000A*/<int64> right) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0A 15 11 28 01 0A
  {
    // Method begins at RVA 0x243d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanOrEqual

  .method /*06000151*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          LessThanOrEqual(valuetype System.Numerics.Vector`1/*0200000A*/<float32> left,
                          valuetype System.Numerics.Vector`1/*0200000A*/<float32> right) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2440
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanOrEqual

  .method /*06000152*/ public hidebysig static 
          bool  LessThanOrEqualAll<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                      valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000DA:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000289*/ 
    .custom /*0C0001E2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800028A*/ 
    .custom /*0C0001E3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2443
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanOrEqualAll

  .method /*06000153*/ public hidebysig static 
          bool  LessThanOrEqualAny<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                      valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000DC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800028B*/ 
    .custom /*0C0001E4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800028C*/ 
    .custom /*0C0001E5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2446
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanOrEqualAny

  .method /*06000154*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          LessThanOrEqual<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                             valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000DD:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800028D*/ 
    .custom /*0C0001E6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800028E*/ 
    .custom /*0C0001E7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800028F*/ 
    .custom /*0C0001E8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2449
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThanOrEqual

  .method /*06000155*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          LessThan<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000DE:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000290*/ 
    .custom /*0C0001E9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000291*/ 
    .custom /*0C0001EA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000292*/ 
    .custom /*0C0001EB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x244c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LessThan

  .method /*06000156*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Load<T>(!!T* source) cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 0F 1E 00
  {
    .custom /*0C0000DF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E0:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000293*/ 
    .custom /*0C0001EC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000294*/ 
    .custom /*0C0001ED:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x244f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Load

  .method /*06000157*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          LoadAligned<T>(!!T* source) cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 0F 1E 00
  {
    .custom /*0C0000E1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E2:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000295*/ 
    .custom /*0C0001EE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000296*/ 
    .custom /*0C0001EF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2452
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LoadAligned

  .method /*06000158*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          LoadAlignedNonTemporal<T>(!!T* source) cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 0F 1E 00
  {
    .custom /*0C0000E3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E4:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000297*/ 
    .custom /*0C0001F0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000298*/ 
    .custom /*0C0001F1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2455
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LoadAlignedNonTemporal

  .method /*06000159*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          LoadUnsafe<T>([in] !!T& source) cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 10 1E 00
  {
    .custom /*0C0000E5:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A00004C*/ 
      .custom /*0C00005B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000299*/ 
    .custom /*0C0001F2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800029A*/ 
    .custom /*0C0001F5:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000020*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2458
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LoadUnsafe

  .method /*0600015A*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          LoadUnsafe<T>([in] !!T& source,
                        native uint elementOffset) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 10 1E 00 19
  {
    .custom /*0C0000E6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000E7:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A00004D*/ 
      .custom /*0C00005C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800029B*/ 
    .custom /*0C0001F8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800029C*/ 
    .custom /*0C0001FA:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000020*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x245b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::LoadUnsafe

  .method /*0600015B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Log(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x245e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Log

  .method /*0600015C*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Log(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2461
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Log

  .method /*0600015D*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Log2(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2464
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Log2

  .method /*0600015E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Log2(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2467
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Log2

  .method /*0600015F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Max<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                 valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000E8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002A2*/ 
    .custom /*0C0001FB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002A3*/ 
    .custom /*0C0001FC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002A4*/ 
    .custom /*0C0001FE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x246a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Max

  .method /*06000160*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          MaxMagnitude<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000E9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002A5*/ 
    .custom /*0C0001FF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002A6*/ 
    .custom /*0C000200:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002A7*/ 
    .custom /*0C000201:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x246d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MaxMagnitude

  .method /*06000161*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          MaxMagnitudeNumber<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000EA:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002A8*/ 
    .custom /*0C000202:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002A9*/ 
    .custom /*0C000203:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002AA*/ 
    .custom /*0C000204:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2470
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MaxMagnitudeNumber

  .method /*06000162*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          MaxNative<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                       valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000EB:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002AB*/ 
    .custom /*0C000205:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002AC*/ 
    .custom /*0C000207:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002AD*/ 
    .custom /*0C00020A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2473
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MaxNative

  .method /*06000163*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          MaxNumber<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                       valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000EC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002AE*/ 
    .custom /*0C00020B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002AF*/ 
    .custom /*0C00020C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002B0*/ 
    .custom /*0C00020D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2476
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MaxNumber

  .method /*06000164*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Min<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                 valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000ED:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002B1*/ 
    .custom /*0C00020E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002B2*/ 
    .custom /*0C00020F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002B3*/ 
    .custom /*0C000211:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2479
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Min

  .method /*06000165*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          MinMagnitude<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000EE:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002B4*/ 
    .custom /*0C000212:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002B5*/ 
    .custom /*0C000213:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002B6*/ 
    .custom /*0C000214:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x247c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MinMagnitude

  .method /*06000166*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          MinMagnitudeNumber<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                                valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000EF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002B7*/ 
    .custom /*0C000215:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002B8*/ 
    .custom /*0C000216:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002B9*/ 
    .custom /*0C000217:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x247f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MinMagnitudeNumber

  .method /*06000167*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          MinNative<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                       valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000F0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002BA*/ 
    .custom /*0C000218:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002BB*/ 
    .custom /*0C000219:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002BC*/ 
    .custom /*0C00021A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2482
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MinNative

  .method /*06000168*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          MinNumber<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                       valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000F1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002BD*/ 
    .custom /*0C00021B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002BE*/ 
    .custom /*0C00021C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002BF*/ 
    .custom /*0C00021D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2485
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MinNumber

  .method /*06000169*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Multiply<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000F2:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002C0*/ 
    .custom /*0C00021E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002C1*/ 
    .custom /*0C00021F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002C2*/ 
    .custom /*0C000220:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2488
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Multiply

  .method /*0600016A*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Multiply<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                      !!T right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000F3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000059*/ 
      .custom /*0C00005D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002C3*/ 
    .custom /*0C000221:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002C4*/ 
    .custom /*0C000222:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x248b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Multiply

  .method /*0600016B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Multiply<T>(!!T left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000F4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A00005A*/ 
      .custom /*0C00005E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002C6*/ 
    .custom /*0C000223:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080002C8*/ 
    .custom /*0C000224:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x248e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Multiply

  .method /*0600016C*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          MultiplyAddEstimate(valuetype System.Numerics.Vector`1/*0200000A*/<float64> left,
                              valuetype System.Numerics.Vector`1/*0200000A*/<float64> right,
                              valuetype System.Numerics.Vector`1/*0200000A*/<float64> addend) cil managed
  // SIG: 00 03 15 11 28 01 0D 15 11 28 01 0D 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2491
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MultiplyAddEstimate

  .method /*0600016D*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          MultiplyAddEstimate(valuetype System.Numerics.Vector`1/*0200000A*/<float32> left,
                              valuetype System.Numerics.Vector`1/*0200000A*/<float32> right,
                              valuetype System.Numerics.Vector`1/*0200000A*/<float32> addend) cil managed
  // SIG: 00 03 15 11 28 01 0C 15 11 28 01 0C 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x2494
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::MultiplyAddEstimate

  .method /*0600016E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Narrow(valuetype System.Numerics.Vector`1/*0200000A*/<float64> low,
                 valuetype System.Numerics.Vector`1/*0200000A*/<float64> high) cil managed
  // SIG: 00 02 15 11 28 01 0C 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x2497
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Narrow

  .method /*0600016F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int8> 
          Narrow(valuetype System.Numerics.Vector`1/*0200000A*/<int16> low,
                 valuetype System.Numerics.Vector`1/*0200000A*/<int16> high) cil managed
  // SIG: 00 02 15 11 28 01 04 15 11 28 01 06 15 11 28 01 06
  {
    .custom /*0C0000F5:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x249a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Narrow

  .method /*06000170*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          Narrow(valuetype System.Numerics.Vector`1/*0200000A*/<int32> low,
                 valuetype System.Numerics.Vector`1/*0200000A*/<int32> high) cil managed
  // SIG: 00 02 15 11 28 01 06 15 11 28 01 08 15 11 28 01 08
  {
    // Method begins at RVA 0x249d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Narrow

  .method /*06000171*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          Narrow(valuetype System.Numerics.Vector`1/*0200000A*/<int64> low,
                 valuetype System.Numerics.Vector`1/*0200000A*/<int64> high) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 0A 15 11 28 01 0A
  {
    // Method begins at RVA 0x24a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Narrow

  .method /*06000172*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint8> 
          Narrow(valuetype System.Numerics.Vector`1/*0200000A*/<uint16> low,
                 valuetype System.Numerics.Vector`1/*0200000A*/<uint16> high) cil managed
  // SIG: 00 02 15 11 28 01 05 15 11 28 01 07 15 11 28 01 07
  {
    .custom /*0C0000F6:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Narrow

  .method /*06000173*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 
          Narrow(valuetype System.Numerics.Vector`1/*0200000A*/<uint32> low,
                 valuetype System.Numerics.Vector`1/*0200000A*/<uint32> high) cil managed
  // SIG: 00 02 15 11 28 01 07 15 11 28 01 09 15 11 28 01 09
  {
    .custom /*0C0000F7:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Narrow

  .method /*06000174*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          Narrow(valuetype System.Numerics.Vector`1/*0200000A*/<uint64> low,
                 valuetype System.Numerics.Vector`1/*0200000A*/<uint64> high) cil managed
  // SIG: 00 02 15 11 28 01 09 15 11 28 01 0B 15 11 28 01 0B
  {
    .custom /*0C0000F8:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Narrow

  .method /*06000175*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          NarrowWithSaturation(valuetype System.Numerics.Vector`1/*0200000A*/<float64> low,
                               valuetype System.Numerics.Vector`1/*0200000A*/<float64> high) cil managed
  // SIG: 00 02 15 11 28 01 0C 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x24ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::NarrowWithSaturation

  .method /*06000176*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int8> 
          NarrowWithSaturation(valuetype System.Numerics.Vector`1/*0200000A*/<int16> low,
                               valuetype System.Numerics.Vector`1/*0200000A*/<int16> high) cil managed
  // SIG: 00 02 15 11 28 01 04 15 11 28 01 06 15 11 28 01 06
  {
    .custom /*0C0000F9:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::NarrowWithSaturation

  .method /*06000177*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          NarrowWithSaturation(valuetype System.Numerics.Vector`1/*0200000A*/<int32> low,
                               valuetype System.Numerics.Vector`1/*0200000A*/<int32> high) cil managed
  // SIG: 00 02 15 11 28 01 06 15 11 28 01 08 15 11 28 01 08
  {
    // Method begins at RVA 0x24b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::NarrowWithSaturation

  .method /*06000178*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          NarrowWithSaturation(valuetype System.Numerics.Vector`1/*0200000A*/<int64> low,
                               valuetype System.Numerics.Vector`1/*0200000A*/<int64> high) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 0A 15 11 28 01 0A
  {
    // Method begins at RVA 0x24b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::NarrowWithSaturation

  .method /*06000179*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint8> 
          NarrowWithSaturation(valuetype System.Numerics.Vector`1/*0200000A*/<uint16> low,
                               valuetype System.Numerics.Vector`1/*0200000A*/<uint16> high) cil managed
  // SIG: 00 02 15 11 28 01 05 15 11 28 01 07 15 11 28 01 07
  {
    .custom /*0C0000FA:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::NarrowWithSaturation

  .method /*0600017A*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 
          NarrowWithSaturation(valuetype System.Numerics.Vector`1/*0200000A*/<uint32> low,
                               valuetype System.Numerics.Vector`1/*0200000A*/<uint32> high) cil managed
  // SIG: 00 02 15 11 28 01 07 15 11 28 01 09 15 11 28 01 09
  {
    .custom /*0C0000FB:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::NarrowWithSaturation

  .method /*0600017B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          NarrowWithSaturation(valuetype System.Numerics.Vector`1/*0200000A*/<uint64> low,
                               valuetype System.Numerics.Vector`1/*0200000A*/<uint64> high) cil managed
  // SIG: 00 02 15 11 28 01 09 15 11 28 01 0B 15 11 28 01 0B
  {
    .custom /*0C0000FC:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::NarrowWithSaturation

  .method /*0600017C*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Negate<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C0000FE:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002EB*/ 
    .custom /*0C00022B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002EC*/ 
    .custom /*0C00022C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x24c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Negate

  .method /*0600017D*/ public hidebysig static 
          bool  None<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector',
                        !!T 'value') cil managed
  // SIG: 10 01 02 02 15 11 28 01 1E 00 1E 00
  {
    .custom /*0C0000FF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A00005C*/ 
      .custom /*0C00005F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002ED*/ 
    .custom /*0C00022D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x24c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::None

  .method /*0600017E*/ public hidebysig static 
          bool  NoneWhereAllBitsSet<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 02 15 11 28 01 1E 00
  {
    .custom /*0C000100:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002EF*/ 
    .custom /*0C00022E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x24c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::NoneWhereAllBitsSet

  .method /*0600017F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          OnesComplement<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C000101:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080002F0*/ 
    .custom /*0C00022F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080002F1*/ 
    .custom /*0C000230:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x24ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::OnesComplement

  .method /*06000180*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          RadiansToDegrees(valuetype System.Numerics.Vector`1/*0200000A*/<float64> radians) cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x24cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::RadiansToDegrees

  .method /*06000181*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          RadiansToDegrees(valuetype System.Numerics.Vector`1/*0200000A*/<float32> radians) cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x24d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::RadiansToDegrees

  .method /*06000182*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Round(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x24d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Round

  .method /*06000183*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Round(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x24d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Round

  .method /*06000184*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Round(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector',
                valuetype [System.Runtime/*23000001*/]System.MidpointRounding/*01000021*/ mode) cil managed
  // SIG: 00 02 15 11 28 01 0D 15 11 28 01 0D 11 80 85
  {
    // Method begins at RVA 0x24d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Round

  .method /*06000185*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Round(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector',
                valuetype [System.Runtime/*23000001*/]System.MidpointRounding/*01000021*/ mode) cil managed
  // SIG: 00 02 15 11 28 01 0C 15 11 28 01 0C 11 80 85
  {
    // Method begins at RVA 0x24dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Round

  .method /*06000186*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint8> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<uint8> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 05 15 11 28 01 05 08
  {
    // Method begins at RVA 0x24df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*06000187*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<int16> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 06 15 11 28 01 06 08
  {
    // Method begins at RVA 0x24e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*06000188*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<int32> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 08 08
  {
    // Method begins at RVA 0x24e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*06000189*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<int64> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0A 08
  {
    // Method begins at RVA 0x24e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*0600018A*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native int> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<native int> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 18 15 11 28 01 18 08
  {
    // Method begins at RVA 0x24eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*0600018B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native uint> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<native uint> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 19 15 11 28 01 19 08
  {
    .custom /*0C000102:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*0600018C*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int8> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<int8> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 04 15 11 28 01 04 08
  {
    .custom /*0C000103:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*0600018D*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 07 15 11 28 01 07 08
  {
    .custom /*0C000104:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*0600018E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 09 15 11 28 01 09 08
  {
    .custom /*0C000105:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*0600018F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 
          ShiftLeft(valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 'value',
                    int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 0B 15 11 28 01 0B 08
  {
    .custom /*0C000106:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftLeft

  .method /*06000190*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          ShiftRightArithmetic(valuetype System.Numerics.Vector`1/*0200000A*/<int16> 'value',
                               int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 06 15 11 28 01 06 08
  {
    // Method begins at RVA 0x24fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightArithmetic

  .method /*06000191*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          ShiftRightArithmetic(valuetype System.Numerics.Vector`1/*0200000A*/<int32> 'value',
                               int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 08 08
  {
    // Method begins at RVA 0x2500
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightArithmetic

  .method /*06000192*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          ShiftRightArithmetic(valuetype System.Numerics.Vector`1/*0200000A*/<int64> 'value',
                               int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0A 08
  {
    // Method begins at RVA 0x2503
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightArithmetic

  .method /*06000193*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native int> 
          ShiftRightArithmetic(valuetype System.Numerics.Vector`1/*0200000A*/<native int> 'value',
                               int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 18 15 11 28 01 18 08
  {
    // Method begins at RVA 0x2506
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightArithmetic

  .method /*06000194*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int8> 
          ShiftRightArithmetic(valuetype System.Numerics.Vector`1/*0200000A*/<int8> 'value',
                               int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 04 15 11 28 01 04 08
  {
    .custom /*0C000107:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2509
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightArithmetic

  .method /*06000195*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint8> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<uint8> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 05 15 11 28 01 05 08
  {
    // Method begins at RVA 0x250c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*06000196*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<int16> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 06 15 11 28 01 06 08
  {
    // Method begins at RVA 0x250f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*06000197*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<int32> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 08 15 11 28 01 08 08
  {
    // Method begins at RVA 0x2512
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*06000198*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<int64> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 0A 15 11 28 01 0A 08
  {
    // Method begins at RVA 0x2515
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*06000199*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native int> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<native int> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 18 15 11 28 01 18 08
  {
    // Method begins at RVA 0x2518
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*0600019A*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native uint> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<native uint> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 19 15 11 28 01 19 08
  {
    .custom /*0C000108:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x251b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*0600019B*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int8> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<int8> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 04 15 11 28 01 04 08
  {
    .custom /*0C00010A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x251e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*0600019C*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 07 15 11 28 01 07 08
  {
    .custom /*0C00010C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2521
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*0600019D*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 09 15 11 28 01 09 08
  {
    .custom /*0C00010E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2524
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*0600019E*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 
          ShiftRightLogical(valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 'value',
                            int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 0B 15 11 28 01 0B 08
  {
    .custom /*0C000110:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2527
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ShiftRightLogical

  .method /*0600019F*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Sin(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x252a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Sin

  .method /*060001A0*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Sin(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x252d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Sin

  .method /*060001A1*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000023*/<valuetype System.Numerics.Vector`1/*0200000A*/<float64>,valuetype System.Numerics.Vector`1/*0200000A*/<float64>> 
          SinCos(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector') cil managed
  // SIG: 00 01 15 11 80 8D 02 15 11 28 01 0D 15 11 28 01 0D 15 11 28 01 0D
  {
    .param [0]/*0800032E*/ 
    .custom /*0C000238:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000022*/::.ctor(string[]) /* 0A00001C */ = ( 01 00 02 00 00 00 03 53 69 6E 03 43 6F 73 00 00 ) // .......Sin.Cos..
    // Method begins at RVA 0x2530
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::SinCos

  .method /*060001A2*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000023*/<valuetype System.Numerics.Vector`1/*0200000A*/<float32>,valuetype System.Numerics.Vector`1/*0200000A*/<float32>> 
          SinCos(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector') cil managed
  // SIG: 00 01 15 11 80 8D 02 15 11 28 01 0C 15 11 28 01 0C 15 11 28 01 0C
  {
    .param [0]/*08000330*/ 
    .custom /*0C000239:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000022*/::.ctor(string[]) /* 0A00001C */ = ( 01 00 02 00 00 00 03 53 69 6E 03 43 6F 73 00 00 ) // .......Sin.Cos..
    // Method begins at RVA 0x2533
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::SinCos

  .method /*060001A3*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          SquareRoot<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C000116:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000332*/ 
    .custom /*0C00023A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000333*/ 
    .custom /*0C00023B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2536
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::SquareRoot

  .method /*060001A4*/ public hidebysig static 
          void  Store<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> source,
                         !!T* destination) cil managed
  // SIG: 10 01 02 01 15 11 28 01 1E 00 0F 1E 00
  {
    .custom /*0C000117:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000118:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000119:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000334*/ 
    .custom /*0C00023C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000335*/ 
    .custom /*0C00023D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2539
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Store

  .method /*060001A5*/ public hidebysig static 
          void  Store(valuetype System.Numerics.Vector2/*02000007*/ source,
                      float32* destination) cil managed
  // SIG: 00 02 01 11 1C 0F 0C
  {
    .custom /*0C00011B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00011C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x253c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Store

  .method /*060001A6*/ public hidebysig static 
          void  Store(valuetype System.Numerics.Vector3/*02000008*/ source,
                      float32* destination) cil managed
  // SIG: 00 02 01 11 20 0F 0C
  {
    .custom /*0C00011E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00011F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x253f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Store

  .method /*060001A7*/ public hidebysig static 
          void  Store(valuetype System.Numerics.Vector4/*02000009*/ source,
                      float32* destination) cil managed
  // SIG: 00 02 01 11 24 0F 0C
  {
    .custom /*0C000121:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000122:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2542
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Store

  .method /*060001A8*/ public hidebysig static 
          void  StoreAligned<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> source,
                                !!T* destination) cil managed
  // SIG: 10 01 02 01 15 11 28 01 1E 00 0F 1E 00
  {
    .custom /*0C000124:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000125:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000126:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800033C*/ 
    .custom /*0C000243:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800033D*/ 
    .custom /*0C000244:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2545
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreAligned

  .method /*060001A9*/ public hidebysig static 
          void  StoreAligned(valuetype System.Numerics.Vector2/*02000007*/ source,
                             float32* destination) cil managed
  // SIG: 00 02 01 11 1C 0F 0C
  {
    .custom /*0C000128:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000129:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2548
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreAligned

  .method /*060001AA*/ public hidebysig static 
          void  StoreAligned(valuetype System.Numerics.Vector3/*02000008*/ source,
                             float32* destination) cil managed
  // SIG: 00 02 01 11 20 0F 0C
  {
    .custom /*0C00012A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00012B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x254b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreAligned

  .method /*060001AB*/ public hidebysig static 
          void  StoreAligned(valuetype System.Numerics.Vector4/*02000009*/ source,
                             float32* destination) cil managed
  // SIG: 00 02 01 11 24 0F 0C
  {
    .custom /*0C00012D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00012E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x254e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreAligned

  .method /*060001AC*/ public hidebysig static 
          void  StoreAlignedNonTemporal<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> source,
                                           !!T* destination) cil managed
  // SIG: 10 01 02 01 15 11 28 01 1E 00 0F 1E 00
  {
    .custom /*0C000130:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000131:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000132:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000344*/ 
    .custom /*0C000246:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000345*/ 
    .custom /*0C000247:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2551
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreAlignedNonTemporal

  .method /*060001AD*/ public hidebysig static 
          void  StoreAlignedNonTemporal(valuetype System.Numerics.Vector2/*02000007*/ source,
                                        float32* destination) cil managed
  // SIG: 00 02 01 11 1C 0F 0C
  {
    .custom /*0C000134:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000135:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2554
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreAlignedNonTemporal

  .method /*060001AE*/ public hidebysig static 
          void  StoreAlignedNonTemporal(valuetype System.Numerics.Vector3/*02000008*/ source,
                                        float32* destination) cil managed
  // SIG: 00 02 01 11 20 0F 0C
  {
    .custom /*0C000136:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000137:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2557
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreAlignedNonTemporal

  .method /*060001AF*/ public hidebysig static 
          void  StoreAlignedNonTemporal(valuetype System.Numerics.Vector4/*02000009*/ source,
                                        float32* destination) cil managed
  // SIG: 00 02 01 11 24 0F 0C
  {
    .custom /*0C000138:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000139:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x255a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreAlignedNonTemporal

  .method /*060001B0*/ public hidebysig static 
          void  StoreUnsafe<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> source,
                               !!T& destination) cil managed
  // SIG: 10 01 02 01 15 11 28 01 1E 00 10 1E 00
  {
    .custom /*0C00013A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00013B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000063*/ 
      .custom /*0C000060:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800034C*/ 
    .custom /*0C000249:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x255d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreUnsafe

  .method /*060001B1*/ public hidebysig static 
          void  StoreUnsafe(valuetype System.Numerics.Vector2/*02000007*/ source,
                            float32& destination) cil managed
  // SIG: 00 02 01 11 1C 10 0C
  {
    .custom /*0C00013C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2560
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreUnsafe

  .method /*060001B2*/ public hidebysig static 
          void  StoreUnsafe(valuetype System.Numerics.Vector3/*02000008*/ source,
                            float32& destination) cil managed
  // SIG: 00 02 01 11 20 10 0C
  {
    .custom /*0C00013D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2563
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreUnsafe

  .method /*060001B3*/ public hidebysig static 
          void  StoreUnsafe(valuetype System.Numerics.Vector4/*02000009*/ source,
                            float32& destination) cil managed
  // SIG: 00 02 01 11 24 10 0C
  {
    .custom /*0C00013E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2566
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreUnsafe

  .method /*060001B4*/ public hidebysig static 
          void  StoreUnsafe<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> source,
                               !!T& destination,
                               native uint elementOffset) cil managed
  // SIG: 10 01 03 01 15 11 28 01 1E 00 10 1E 00 19
  {
    .custom /*0C00013F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000140:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000141:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000064*/ 
      .custom /*0C000061:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000354*/ 
    .custom /*0C00024D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2569
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreUnsafe

  .method /*060001B5*/ public hidebysig static 
          void  StoreUnsafe(valuetype System.Numerics.Vector2/*02000007*/ source,
                            float32& destination,
                            native uint elementOffset) cil managed
  // SIG: 00 03 01 11 1C 10 0C 19
  {
    .custom /*0C000142:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000143:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x256c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreUnsafe

  .method /*060001B6*/ public hidebysig static 
          void  StoreUnsafe(valuetype System.Numerics.Vector3/*02000008*/ source,
                            float32& destination,
                            native uint elementOffset) cil managed
  // SIG: 00 03 01 11 20 10 0C 19
  {
    .custom /*0C000144:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000145:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x256f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreUnsafe

  .method /*060001B7*/ public hidebysig static 
          void  StoreUnsafe(valuetype System.Numerics.Vector4/*02000009*/ source,
                            float32& destination,
                            native uint elementOffset) cil managed
  // SIG: 00 03 01 11 24 10 0C 19
  {
    .custom /*0C000146:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000147:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2572
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::StoreUnsafe

  .method /*060001B8*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Subtract<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C000148:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000360*/ 
    .custom /*0C00024E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000361*/ 
    .custom /*0C00024F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000362*/ 
    .custom /*0C000250:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2575
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Subtract

  .method /*060001B9*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          SubtractSaturate<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                              valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C000149:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000363*/ 
    .custom /*0C000251:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000364*/ 
    .custom /*0C000252:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000365*/ 
    .custom /*0C000253:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2578
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::SubtractSaturate

  .method /*060001BA*/ public hidebysig static 
          !!T  Sum<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'value') cil managed
  // SIG: 10 01 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C00014A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param type T /*2A000067*/ 
      .custom /*0C000062:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000366*/ 
    .custom /*0C000254:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x257b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Sum

  .method /*060001BB*/ public hidebysig static 
          !!T  ToScalar<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector') cil managed
  // SIG: 10 01 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C00014C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00014D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000068*/ 
      .custom /*0C000063:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000367*/ 
    .custom /*0C000255:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x257e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ToScalar

  .method /*060001BC*/ public hidebysig static 
          float32  ToScalar(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 0C 11 1C
  {
    .custom /*0C00014F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2581
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ToScalar

  .method /*060001BD*/ public hidebysig static 
          float32  ToScalar(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 0C 11 20
  {
    .custom /*0C000151:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2584
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ToScalar

  .method /*060001BE*/ public hidebysig static 
          float32  ToScalar(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 0C 11 24
  {
    .custom /*0C000153:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2587
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::ToScalar

  .method /*060001BF*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          Truncate(valuetype System.Numerics.Vector`1/*0200000A*/<float64> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0D
  {
    // Method begins at RVA 0x258a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Truncate

  .method /*060001C0*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          Truncate(valuetype System.Numerics.Vector`1/*0200000A*/<float32> 'vector') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 0C
  {
    // Method begins at RVA 0x258d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Truncate

  .method /*060001C1*/ public hidebysig static 
          void  Widen(valuetype System.Numerics.Vector`1/*0200000A*/<uint8> source,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<uint16>& low,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<uint16>& high) cil managed
  // SIG: 00 03 01 15 11 28 01 05 10 15 11 28 01 07 10 15 11 28 01 07
  {
    .custom /*0C000157:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2590
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Widen

  .method /*060001C2*/ public hidebysig static 
          void  Widen(valuetype System.Numerics.Vector`1/*0200000A*/<int16> source,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<int32>& low,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<int32>& high) cil managed
  // SIG: 00 03 01 15 11 28 01 06 10 15 11 28 01 08 10 15 11 28 01 08
  {
    // Method begins at RVA 0x2593
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Widen

  .method /*060001C3*/ public hidebysig static 
          void  Widen(valuetype System.Numerics.Vector`1/*0200000A*/<int32> source,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<int64>& low,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<int64>& high) cil managed
  // SIG: 00 03 01 15 11 28 01 08 10 15 11 28 01 0A 10 15 11 28 01 0A
  {
    // Method begins at RVA 0x2596
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Widen

  .method /*060001C4*/ public hidebysig static 
          void  Widen(valuetype System.Numerics.Vector`1/*0200000A*/<int8> source,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<int16>& low,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<int16>& high) cil managed
  // SIG: 00 03 01 15 11 28 01 04 10 15 11 28 01 06 10 15 11 28 01 06
  {
    .custom /*0C00015B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2599
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Widen

  .method /*060001C5*/ public hidebysig static 
          void  Widen(valuetype System.Numerics.Vector`1/*0200000A*/<float32> source,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<float64>& low,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<float64>& high) cil managed
  // SIG: 00 03 01 15 11 28 01 0C 10 15 11 28 01 0D 10 15 11 28 01 0D
  {
    // Method begins at RVA 0x259c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Widen

  .method /*060001C6*/ public hidebysig static 
          void  Widen(valuetype System.Numerics.Vector`1/*0200000A*/<uint16> source,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<uint32>& low,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<uint32>& high) cil managed
  // SIG: 00 03 01 15 11 28 01 07 10 15 11 28 01 09 10 15 11 28 01 09
  {
    .custom /*0C00015E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x259f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Widen

  .method /*060001C7*/ public hidebysig static 
          void  Widen(valuetype System.Numerics.Vector`1/*0200000A*/<uint32> source,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<uint64>& low,
                      [out] valuetype System.Numerics.Vector`1/*0200000A*/<uint64>& high) cil managed
  // SIG: 00 03 01 15 11 28 01 09 10 15 11 28 01 0B 10 15 11 28 01 0B
  {
    .custom /*0C000160:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Widen

  .method /*060001C8*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 
          WidenLower(valuetype System.Numerics.Vector`1/*0200000A*/<uint8> source) cil managed
  // SIG: 00 01 15 11 28 01 07 15 11 28 01 05
  {
    .custom /*0C000162:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenLower

  .method /*060001C9*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          WidenLower(valuetype System.Numerics.Vector`1/*0200000A*/<int16> source) cil managed
  // SIG: 00 01 15 11 28 01 08 15 11 28 01 06
  {
    // Method begins at RVA 0x25a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenLower

  .method /*060001CA*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          WidenLower(valuetype System.Numerics.Vector`1/*0200000A*/<int32> source) cil managed
  // SIG: 00 01 15 11 28 01 0A 15 11 28 01 08
  {
    // Method begins at RVA 0x25ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenLower

  .method /*060001CB*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          WidenLower(valuetype System.Numerics.Vector`1/*0200000A*/<int8> source) cil managed
  // SIG: 00 01 15 11 28 01 06 15 11 28 01 04
  {
    .custom /*0C000166:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenLower

  .method /*060001CC*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          WidenLower(valuetype System.Numerics.Vector`1/*0200000A*/<float32> source) cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0C
  {
    // Method begins at RVA 0x25b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenLower

  .method /*060001CD*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          WidenLower(valuetype System.Numerics.Vector`1/*0200000A*/<uint16> source) cil managed
  // SIG: 00 01 15 11 28 01 09 15 11 28 01 07
  {
    .custom /*0C000168:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenLower

  .method /*060001CE*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 
          WidenLower(valuetype System.Numerics.Vector`1/*0200000A*/<uint32> source) cil managed
  // SIG: 00 01 15 11 28 01 0B 15 11 28 01 09
  {
    .custom /*0C000169:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenLower

  .method /*060001CF*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 
          WidenUpper(valuetype System.Numerics.Vector`1/*0200000A*/<uint8> source) cil managed
  // SIG: 00 01 15 11 28 01 07 15 11 28 01 05
  {
    .custom /*0C00016B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenUpper

  .method /*060001D0*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          WidenUpper(valuetype System.Numerics.Vector`1/*0200000A*/<int16> source) cil managed
  // SIG: 00 01 15 11 28 01 08 15 11 28 01 06
  {
    // Method begins at RVA 0x25bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenUpper

  .method /*060001D1*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          WidenUpper(valuetype System.Numerics.Vector`1/*0200000A*/<int32> source) cil managed
  // SIG: 00 01 15 11 28 01 0A 15 11 28 01 08
  {
    // Method begins at RVA 0x25c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenUpper

  .method /*060001D2*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          WidenUpper(valuetype System.Numerics.Vector`1/*0200000A*/<int8> source) cil managed
  // SIG: 00 01 15 11 28 01 06 15 11 28 01 04
  {
    .custom /*0C00016F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenUpper

  .method /*060001D3*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          WidenUpper(valuetype System.Numerics.Vector`1/*0200000A*/<float32> source) cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 0C
  {
    // Method begins at RVA 0x25c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenUpper

  .method /*060001D4*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          WidenUpper(valuetype System.Numerics.Vector`1/*0200000A*/<uint16> source) cil managed
  // SIG: 00 01 15 11 28 01 09 15 11 28 01 07
  {
    .custom /*0C000172:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenUpper

  .method /*060001D5*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 
          WidenUpper(valuetype System.Numerics.Vector`1/*0200000A*/<uint32> source) cil managed
  // SIG: 00 01 15 11 28 01 0B 15 11 28 01 09
  {
    .custom /*0C000174:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WidenUpper

  .method /*060001D6*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          WithElement<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 'vector',
                         int32 index,
                         !!T 'value') cil managed
  // SIG: 10 01 03 15 11 28 01 1E 00 15 11 28 01 1E 00 08 1E 00
  {
    .custom /*0C000176:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000177:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000069*/ 
      .custom /*0C000064:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000390*/ 
    .custom /*0C00025C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000391*/ 
    .custom /*0C00025D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x25cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WithElement

  .method /*060001D7*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          WithElement(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                      int32 index,
                      float32 'value') cil managed
  // SIG: 00 03 11 1C 11 1C 08 0C
  {
    .custom /*0C000178:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WithElement

  .method /*060001D8*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          WithElement(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                      int32 index,
                      float32 'value') cil managed
  // SIG: 00 03 11 20 11 20 08 0C
  {
    .custom /*0C000179:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WithElement

  .method /*060001D9*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          WithElement(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                      int32 index,
                      float32 'value') cil managed
  // SIG: 00 03 11 24 11 24 08 0C
  {
    .custom /*0C00017A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::WithElement

  .method /*060001DA*/ public hidebysig static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!!T> 
          Xor<T>(valuetype System.Numerics.Vector`1/*0200000A*/<!!T> left,
                 valuetype System.Numerics.Vector`1/*0200000A*/<!!T> right) cil managed
  // SIG: 10 01 02 15 11 28 01 1E 00 15 11 28 01 1E 00 15 11 28 01 1E 00
  {
    .custom /*0C00017B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800039D*/ 
    .custom /*0C00025E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800039E*/ 
    .custom /*0C00025F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800039F*/ 
    .custom /*0C000260:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x25db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector::Xor

  .property /*17000016*/ callconv(8) bool
          IsHardwareAccelerated()
  {
    .get bool System.Numerics.Vector/*02000006*/::get_IsHardwareAccelerated() /* 060000CA */
  } // end of property Vector::IsHardwareAccelerated
} // end of class System.Numerics.Vector

.class /*02000007*/ public sequential ansi sealed beforefieldinit System.Numerics.Vector2
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Vector2/*02000007*/>/*1B000005*/,
                  [System.Runtime/*23000001*/]System.IFormattable/*01000024*/
{
  .custom /*0C000021:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .field /*0400001D*/ public float32 X
  .field /*0400001E*/ public float32 Y
  .method /*060001DB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x25de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::.ctor

  .method /*060001DC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 x,
                               float32 y) cil managed
  // SIG: 20 02 01 0C 0C
  {
    // Method begins at RVA 0x25e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::.ctor

  .method /*060001DD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<float32> values) cil managed
  // SIG: 20 01 01 15 11 7D 01 0C
  {
    // Method begins at RVA 0x25e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::.ctor

  .method /*060001DE*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_AllBitsSet() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_AllBitsSet

  .method /*060001DF*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_E() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_E

  .method /*060001E0*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_Epsilon() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_Epsilon

  .method /*060001E1*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_NaN() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_NaN

  .method /*060001E2*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_NegativeInfinity() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_NegativeInfinity

  .method /*060001E3*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_NegativeZero() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_NegativeZero

  .method /*060001E4*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_One() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_One

  .method /*060001E5*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_Pi() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_Pi

  .method /*060001E6*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_PositiveInfinity() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x25ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_PositiveInfinity

  .method /*060001E7*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_Tau() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2602
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_Tau

  .method /*060001E8*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_UnitX() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2605
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_UnitX

  .method /*060001E9*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_UnitY() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2608
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_UnitY

  .method /*060001EA*/ public hidebysig specialname 
          instance float32  get_Item(int32 index) cil managed
  // SIG: 20 01 0C 08
  {
    .custom /*0C000180:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x260b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_Item

  .method /*060001EB*/ public hidebysig specialname 
          instance void  set_Item(int32 index,
                                  float32 'value') cil managed
  // SIG: 20 02 01 08 0C
  {
    // Method begins at RVA 0x260e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::set_Item

  .method /*060001EC*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          get_Zero() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2611
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::get_Zero

  .method /*060001ED*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Abs(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2614
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Abs

  .method /*060001EE*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Add(valuetype System.Numerics.Vector2/*02000007*/ left,
              valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2617
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Add

  .method /*060001EF*/ public hidebysig static 
          bool  All(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                    float32 'value') cil managed
  // SIG: 00 02 02 11 1C 0C
  {
    // Method begins at RVA 0x261a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::All

  .method /*060001F0*/ public hidebysig static 
          bool  AllWhereAllBitsSet(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 02 11 1C
  {
    // Method begins at RVA 0x261d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::AllWhereAllBitsSet

  .method /*060001F1*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          AndNot(valuetype System.Numerics.Vector2/*02000007*/ left,
                 valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2620
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::AndNot

  .method /*060001F2*/ public hidebysig static 
          bool  Any(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                    float32 'value') cil managed
  // SIG: 00 02 02 11 1C 0C
  {
    // Method begins at RVA 0x2623
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Any

  .method /*060001F3*/ public hidebysig static 
          bool  AnyWhereAllBitsSet(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 02 11 1C
  {
    // Method begins at RVA 0x2626
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::AnyWhereAllBitsSet

  .method /*060001F4*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          BitwiseAnd(valuetype System.Numerics.Vector2/*02000007*/ left,
                     valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2629
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::BitwiseAnd

  .method /*060001F5*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          BitwiseOr(valuetype System.Numerics.Vector2/*02000007*/ left,
                    valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x262c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::BitwiseOr

  .method /*060001F6*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Clamp(valuetype System.Numerics.Vector2/*02000007*/ value1,
                valuetype System.Numerics.Vector2/*02000007*/ min,
                valuetype System.Numerics.Vector2/*02000007*/ max) cil managed
  // SIG: 00 03 11 1C 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x262f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Clamp

  .method /*060001F7*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          ClampNative(valuetype System.Numerics.Vector2/*02000007*/ value1,
                      valuetype System.Numerics.Vector2/*02000007*/ min,
                      valuetype System.Numerics.Vector2/*02000007*/ max) cil managed
  // SIG: 00 03 11 1C 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2632
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::ClampNative

  .method /*060001F8*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          ConditionalSelect(valuetype System.Numerics.Vector2/*02000007*/ condition,
                            valuetype System.Numerics.Vector2/*02000007*/ left,
                            valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 03 11 1C 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2635
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::ConditionalSelect

  .method /*060001F9*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          CopySign(valuetype System.Numerics.Vector2/*02000007*/ 'value',
                   valuetype System.Numerics.Vector2/*02000007*/ sign) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2638
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::CopySign

  .method /*060001FA*/ public hidebysig static 
          int32  Count(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                       float32 'value') cil managed
  // SIG: 00 02 08 11 1C 0C
  {
    // Method begins at RVA 0x263b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Count

  .method /*060001FB*/ public hidebysig static 
          int32  CountWhereAllBitsSet(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 08 11 1C
  {
    // Method begins at RVA 0x263e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::CountWhereAllBitsSet

  .method /*060001FC*/ public hidebysig instance void 
          CopyTo(float32[] 'array') cil managed
  // SIG: 20 01 01 1D 0C
  {
    .custom /*0C00018B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00018C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2641
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector2::CopyTo

  .method /*060001FD*/ public hidebysig instance void 
          CopyTo(float32[] 'array',
                 int32 index) cil managed
  // SIG: 20 02 01 1D 0C 08
  {
    .custom /*0C00018E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00018F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2643
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector2::CopyTo

  .method /*060001FE*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<float32> destination) cil managed
  // SIG: 20 01 01 15 11 80 95 01 0C
  {
    .custom /*0C000190:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2645
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector2::CopyTo

  .method /*060001FF*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Cos(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2647
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Cos

  .method /*06000200*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Create(float32 'value') cil managed
  // SIG: 00 01 11 1C 0C
  {
    // Method begins at RVA 0x264a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Create

  .method /*06000201*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Create(float32 x,
                 float32 y) cil managed
  // SIG: 00 02 11 1C 0C 0C
  {
    // Method begins at RVA 0x264d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Create

  .method /*06000202*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Create(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<float32> values) cil managed
  // SIG: 00 01 11 1C 15 11 7D 01 0C
  {
    // Method begins at RVA 0x2650
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Create

  .method /*06000203*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          CreateScalar(float32 x) cil managed
  // SIG: 00 01 11 1C 0C
  {
    // Method begins at RVA 0x2653
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::CreateScalar

  .method /*06000204*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          CreateScalarUnsafe(float32 x) cil managed
  // SIG: 00 01 11 1C 0C
  {
    // Method begins at RVA 0x2656
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::CreateScalarUnsafe

  .method /*06000205*/ public hidebysig instance bool 
          TryCopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<float32> destination) cil managed
  // SIG: 20 01 02 15 11 80 95 01 0C
  {
    .custom /*0C000195:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2659
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::TryCopyTo

  .method /*06000206*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          DegreesToRadians(valuetype System.Numerics.Vector2/*02000007*/ degrees) cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x265c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::DegreesToRadians

  .method /*06000207*/ public hidebysig static 
          float32  Distance(valuetype System.Numerics.Vector2/*02000007*/ value1,
                            valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 0C 11 1C 11 1C
  {
    // Method begins at RVA 0x265f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Distance

  .method /*06000208*/ public hidebysig static 
          float32  DistanceSquared(valuetype System.Numerics.Vector2/*02000007*/ value1,
                                   valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 0C 11 1C 11 1C
  {
    // Method begins at RVA 0x2662
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::DistanceSquared

  .method /*06000209*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Divide(valuetype System.Numerics.Vector2/*02000007*/ left,
                 valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2665
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Divide

  .method /*0600020A*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Divide(valuetype System.Numerics.Vector2/*02000007*/ left,
                 float32 divisor) cil managed
  // SIG: 00 02 11 1C 11 1C 0C
  {
    // Method begins at RVA 0x2668
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Divide

  .method /*0600020B*/ public hidebysig static 
          float32  Dot(valuetype System.Numerics.Vector2/*02000007*/ value1,
                       valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 0C 11 1C 11 1C
  {
    // Method begins at RVA 0x266b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Dot

  .method /*0600020C*/ public hidebysig static 
          float32  Cross(valuetype System.Numerics.Vector2/*02000007*/ value1,
                         valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 0C 11 1C 11 1C
  {
    // Method begins at RVA 0x266e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Cross

  .method /*0600020D*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Numerics.Vector2/*02000007*/ other) cil managed
  // SIG: 20 01 02 11 1C
  {
    .custom /*0C000199:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2671
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Equals

  .method /*0600020E*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00019A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00019B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080003DE*/ 
    .custom /*0C000275:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001C*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2674
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Equals

  .method /*0600020F*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Equals(valuetype System.Numerics.Vector2/*02000007*/ left,
                 valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2677
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Equals

  .method /*06000210*/ public hidebysig static 
          bool  EqualsAll(valuetype System.Numerics.Vector2/*02000007*/ left,
                          valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x267a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::EqualsAll

  .method /*06000211*/ public hidebysig static 
          bool  EqualsAny(valuetype System.Numerics.Vector2/*02000007*/ left,
                          valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x267d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::EqualsAny

  .method /*06000212*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Exp(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2680
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Exp

  .method /*06000213*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          FusedMultiplyAdd(valuetype System.Numerics.Vector2/*02000007*/ left,
                           valuetype System.Numerics.Vector2/*02000007*/ right,
                           valuetype System.Numerics.Vector2/*02000007*/ addend) cil managed
  // SIG: 00 03 11 1C 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2683
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::FusedMultiplyAdd

  .method /*06000214*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00019F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2686
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::GetHashCode

  .method /*06000215*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          GreaterThan(valuetype System.Numerics.Vector2/*02000007*/ left,
                      valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2689
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::GreaterThan

  .method /*06000216*/ public hidebysig static 
          bool  GreaterThanAll(valuetype System.Numerics.Vector2/*02000007*/ left,
                               valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x268c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::GreaterThanAll

  .method /*06000217*/ public hidebysig static 
          bool  GreaterThanAny(valuetype System.Numerics.Vector2/*02000007*/ left,
                               valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x268f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::GreaterThanAny

  .method /*06000218*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          GreaterThanOrEqual(valuetype System.Numerics.Vector2/*02000007*/ left,
                             valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2692
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::GreaterThanOrEqual

  .method /*06000219*/ public hidebysig static 
          bool  GreaterThanOrEqualAll(valuetype System.Numerics.Vector2/*02000007*/ left,
                                      valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x2695
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::GreaterThanOrEqualAll

  .method /*0600021A*/ public hidebysig static 
          bool  GreaterThanOrEqualAny(valuetype System.Numerics.Vector2/*02000007*/ left,
                                      valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x2698
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::GreaterThanOrEqualAny

  .method /*0600021B*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Hypot(valuetype System.Numerics.Vector2/*02000007*/ x,
                valuetype System.Numerics.Vector2/*02000007*/ y) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x269b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Hypot

  .method /*0600021C*/ public hidebysig static 
          int32  IndexOf(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                         float32 'value') cil managed
  // SIG: 00 02 08 11 1C 0C
  {
    // Method begins at RVA 0x269e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IndexOf

  .method /*0600021D*/ public hidebysig static 
          int32  IndexOfWhereAllBitsSet(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 08 11 1C
  {
    // Method begins at RVA 0x26a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IndexOfWhereAllBitsSet

  .method /*0600021E*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsEvenInteger(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsEvenInteger

  .method /*0600021F*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsFinite(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsFinite

  .method /*06000220*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsInfinity(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsInfinity

  .method /*06000221*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsInteger(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsInteger

  .method /*06000222*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsNaN(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsNaN

  .method /*06000223*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsNegative(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsNegative

  .method /*06000224*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsNegativeInfinity(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsNegativeInfinity

  .method /*06000225*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsNormal(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsNormal

  .method /*06000226*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsOddInteger(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsOddInteger

  .method /*06000227*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsPositive(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsPositive

  .method /*06000228*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsPositiveInfinity(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsPositiveInfinity

  .method /*06000229*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsSubnormal(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsSubnormal

  .method /*0600022A*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          IsZero(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::IsZero

  .method /*0600022B*/ public hidebysig static 
          int32  LastIndexOf(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                             float32 'value') cil managed
  // SIG: 00 02 08 11 1C 0C
  {
    // Method begins at RVA 0x26cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LastIndexOf

  .method /*0600022C*/ public hidebysig static 
          int32  LastIndexOfWhereAllBitsSet(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 08 11 1C
  {
    // Method begins at RVA 0x26ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LastIndexOfWhereAllBitsSet

  .method /*0600022D*/ public hidebysig instance float32 
          Length() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C0001A5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x26d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Length

  .method /*0600022E*/ public hidebysig instance float32 
          LengthSquared() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C0001A6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x26d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LengthSquared

  .method /*0600022F*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Lerp(valuetype System.Numerics.Vector2/*02000007*/ value1,
               valuetype System.Numerics.Vector2/*02000007*/ value2,
               float32 amount) cil managed
  // SIG: 00 03 11 1C 11 1C 11 1C 0C
  {
    // Method begins at RVA 0x26d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Lerp

  .method /*06000230*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Lerp(valuetype System.Numerics.Vector2/*02000007*/ value1,
               valuetype System.Numerics.Vector2/*02000007*/ value2,
               valuetype System.Numerics.Vector2/*02000007*/ amount) cil managed
  // SIG: 00 03 11 1C 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x26da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Lerp

  .method /*06000231*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          LessThan(valuetype System.Numerics.Vector2/*02000007*/ left,
                   valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x26dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LessThan

  .method /*06000232*/ public hidebysig static 
          bool  LessThanAll(valuetype System.Numerics.Vector2/*02000007*/ left,
                            valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x26e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LessThanAll

  .method /*06000233*/ public hidebysig static 
          bool  LessThanAny(valuetype System.Numerics.Vector2/*02000007*/ left,
                            valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x26e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LessThanAny

  .method /*06000234*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          LessThanOrEqual(valuetype System.Numerics.Vector2/*02000007*/ left,
                          valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x26e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LessThanOrEqual

  .method /*06000235*/ public hidebysig static 
          bool  LessThanOrEqualAll(valuetype System.Numerics.Vector2/*02000007*/ left,
                                   valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x26e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LessThanOrEqualAll

  .method /*06000236*/ public hidebysig static 
          bool  LessThanOrEqualAny(valuetype System.Numerics.Vector2/*02000007*/ left,
                                   valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x26ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LessThanOrEqualAny

  .method /*06000237*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Load(float32* source) cil managed
  // SIG: 00 01 11 1C 0F 0C
  {
    .custom /*0C0001A9:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x26ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Load

  .method /*06000238*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          LoadAligned(float32* source) cil managed
  // SIG: 00 01 11 1C 0F 0C
  {
    .custom /*0C0001AB:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x26f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LoadAligned

  .method /*06000239*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          LoadAlignedNonTemporal(float32* source) cil managed
  // SIG: 00 01 11 1C 0F 0C
  {
    .custom /*0C0001AD:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x26f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LoadAlignedNonTemporal

  .method /*0600023A*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          LoadUnsafe([in] float32& source) cil managed
  // SIG: 00 01 11 1C 10 0C
  {
    .param [1]/*0800041F*/ 
    .custom /*0C00027B:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000020*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x26f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LoadUnsafe

  .method /*0600023B*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          LoadUnsafe([in] float32& source,
                     native uint elementOffset) cil managed
  // SIG: 00 02 11 1C 10 0C 19
  {
    .custom /*0C0001AE:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000420*/ 
    .custom /*0C00027C:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000020*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x26fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::LoadUnsafe

  .method /*0600023C*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Log(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x26fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Log

  .method /*0600023D*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Log2(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2701
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Log2

  .method /*0600023E*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Max(valuetype System.Numerics.Vector2/*02000007*/ value1,
              valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2704
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Max

  .method /*0600023F*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MaxMagnitude(valuetype System.Numerics.Vector2/*02000007*/ value1,
                       valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2707
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MaxMagnitude

  .method /*06000240*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MaxMagnitudeNumber(valuetype System.Numerics.Vector2/*02000007*/ value1,
                             valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x270a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MaxMagnitudeNumber

  .method /*06000241*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MaxNative(valuetype System.Numerics.Vector2/*02000007*/ value1,
                    valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x270d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MaxNative

  .method /*06000242*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MaxNumber(valuetype System.Numerics.Vector2/*02000007*/ value1,
                    valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2710
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MaxNumber

  .method /*06000243*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Min(valuetype System.Numerics.Vector2/*02000007*/ value1,
              valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2713
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Min

  .method /*06000244*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MinMagnitude(valuetype System.Numerics.Vector2/*02000007*/ value1,
                       valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2716
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MinMagnitude

  .method /*06000245*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MinMagnitudeNumber(valuetype System.Numerics.Vector2/*02000007*/ value1,
                             valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2719
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MinMagnitudeNumber

  .method /*06000246*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MinNative(valuetype System.Numerics.Vector2/*02000007*/ value1,
                    valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x271c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MinNative

  .method /*06000247*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MinNumber(valuetype System.Numerics.Vector2/*02000007*/ value1,
                    valuetype System.Numerics.Vector2/*02000007*/ value2) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x271f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MinNumber

  .method /*06000248*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Multiply(valuetype System.Numerics.Vector2/*02000007*/ left,
                   valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2722
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Multiply

  .method /*06000249*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Multiply(valuetype System.Numerics.Vector2/*02000007*/ left,
                   float32 right) cil managed
  // SIG: 00 02 11 1C 11 1C 0C
  {
    // Method begins at RVA 0x2725
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Multiply

  .method /*0600024A*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Multiply(float32 left,
                   valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 0C 11 1C
  {
    // Method begins at RVA 0x2728
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Multiply

  .method /*0600024B*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          MultiplyAddEstimate(valuetype System.Numerics.Vector2/*02000007*/ left,
                              valuetype System.Numerics.Vector2/*02000007*/ right,
                              valuetype System.Numerics.Vector2/*02000007*/ addend) cil managed
  // SIG: 00 03 11 1C 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x272b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::MultiplyAddEstimate

  .method /*0600024C*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Negate(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x272e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Negate

  .method /*0600024D*/ public hidebysig static 
          bool  None(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                     float32 'value') cil managed
  // SIG: 00 02 02 11 1C 0C
  {
    // Method begins at RVA 0x2731
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::None

  .method /*0600024E*/ public hidebysig static 
          bool  NoneWhereAllBitsSet(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 02 11 1C
  {
    // Method begins at RVA 0x2734
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::NoneWhereAllBitsSet

  .method /*0600024F*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Normalize(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2737
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Normalize

  .method /*06000250*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          OnesComplement(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x273a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::OnesComplement

  .method /*06000251*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_Addition(valuetype System.Numerics.Vector2/*02000007*/ left,
                      valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x273d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Addition

  .method /*06000252*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_BitwiseAnd(valuetype System.Numerics.Vector2/*02000007*/ left,
                        valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2740
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_BitwiseAnd

  .method /*06000253*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_BitwiseOr(valuetype System.Numerics.Vector2/*02000007*/ left,
                       valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2743
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_BitwiseOr

  .method /*06000254*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_Division(valuetype System.Numerics.Vector2/*02000007*/ left,
                      valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2746
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Division

  .method /*06000255*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_Division(valuetype System.Numerics.Vector2/*02000007*/ value1,
                      float32 value2) cil managed
  // SIG: 00 02 11 1C 11 1C 0C
  {
    // Method begins at RVA 0x2749
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Division

  .method /*06000256*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_ExclusiveOr(valuetype System.Numerics.Vector2/*02000007*/ left,
                         valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x274c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_ExclusiveOr

  .method /*06000257*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Numerics.Vector2/*02000007*/ left,
                            valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x274f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Equality

  .method /*06000258*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_LeftShift(valuetype System.Numerics.Vector2/*02000007*/ 'value',
                       int32 shiftAmount) cil managed
  // SIG: 00 02 11 1C 11 1C 08
  {
    // Method begins at RVA 0x2752
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_LeftShift

  .method /*06000259*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Numerics.Vector2/*02000007*/ left,
                              valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x2755
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Inequality

  .method /*0600025A*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_Multiply(valuetype System.Numerics.Vector2/*02000007*/ left,
                      valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2758
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Multiply

  .method /*0600025B*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_Multiply(valuetype System.Numerics.Vector2/*02000007*/ left,
                      float32 right) cil managed
  // SIG: 00 02 11 1C 11 1C 0C
  {
    // Method begins at RVA 0x275b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Multiply

  .method /*0600025C*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_Multiply(float32 left,
                      valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 0C 11 1C
  {
    // Method begins at RVA 0x275e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Multiply

  .method /*0600025D*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_OnesComplement(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2761
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_OnesComplement

  .method /*0600025E*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_RightShift(valuetype System.Numerics.Vector2/*02000007*/ 'value',
                        int32 shiftAmount) cil managed
  // SIG: 00 02 11 1C 11 1C 08
  {
    // Method begins at RVA 0x2764
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_RightShift

  .method /*0600025F*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_Subtraction(valuetype System.Numerics.Vector2/*02000007*/ left,
                         valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x2767
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_Subtraction

  .method /*06000260*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_UnaryNegation(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x276a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_UnaryNegation

  .method /*06000261*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_UnaryPlus(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x276d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_UnaryPlus

  .method /*06000262*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          op_UnsignedRightShift(valuetype System.Numerics.Vector2/*02000007*/ 'value',
                                int32 shiftAmount) cil managed
  // SIG: 00 02 11 1C 11 1C 08
  {
    // Method begins at RVA 0x2770
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::op_UnsignedRightShift

  .method /*06000263*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          RadiansToDegrees(valuetype System.Numerics.Vector2/*02000007*/ radians) cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2773
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::RadiansToDegrees

  .method /*06000264*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Round(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2776
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Round

  .method /*06000265*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Round(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                valuetype [System.Runtime/*23000001*/]System.MidpointRounding/*01000021*/ mode) cil managed
  // SIG: 00 02 11 1C 11 1C 11 80 85
  {
    // Method begins at RVA 0x2779
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Round

  .method /*06000266*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Reflect(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                  valuetype System.Numerics.Vector2/*02000007*/ normal) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x277c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Reflect

  .method /*06000267*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Shuffle(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                  uint8 xIndex,
                  uint8 yIndex) cil managed
  // SIG: 00 03 11 1C 11 1C 05 05
  {
    // Method begins at RVA 0x277f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Shuffle

  .method /*06000268*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Sin(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2782
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Sin

  .method /*06000269*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000023*/<valuetype System.Numerics.Vector2/*02000007*/,valuetype System.Numerics.Vector2/*02000007*/> 
          SinCos(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 15 11 80 8D 02 11 1C 11 1C 11 1C
  {
    .param [0]/*08000472*/ 
    .custom /*0C00027D:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000022*/::.ctor(string[]) /* 0A00001C */ = ( 01 00 02 00 00 00 03 53 69 6E 03 43 6F 73 00 00 ) // .......Sin.Cos..
    // Method begins at RVA 0x2785
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::SinCos

  .method /*0600026A*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          SquareRoot(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x2788
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::SquareRoot

  .method /*0600026B*/ public hidebysig static 
          float32  Sum(valuetype System.Numerics.Vector2/*02000007*/ 'value') cil managed
  // SIG: 00 01 0C 11 1C
  {
    // Method begins at RVA 0x278b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Sum

  .method /*0600026C*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Subtract(valuetype System.Numerics.Vector2/*02000007*/ left,
                   valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x278e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Subtract

  .method /*0600026D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001D6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2791
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::ToString

  .method /*0600026E*/ public hidebysig instance string 
          ToString(string format) cil managed
  // SIG: 20 01 0E 0E
  {
    .custom /*0C0001D8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000478*/ 
    .custom /*0C00027E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .custom /*0C00027F:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000026*/::.ctor(string) /* 0A00001D */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2794
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::ToString

  .method /*0600026F*/ public hidebysig newslot virtual final 
          instance string  ToString(string format,
                                    class [System.Runtime/*23000001*/]System.IFormatProvider/*01000027*/ formatProvider) cil managed
  // SIG: 20 02 0E 0E 12 80 9D
  {
    .custom /*0C0001DB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0001DC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000479*/ 
    .custom /*0C000280:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800047A*/ 
    .custom /*0C000281:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000026*/::.ctor(string) /* 0A00001D */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2797
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::ToString

  .method /*06000270*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Transform(valuetype System.Numerics.Vector2/*02000007*/ position,
                    valuetype System.Numerics.Matrix3x2/*02000002*/ matrix) cil managed
  // SIG: 00 02 11 1C 11 1C 11 08
  {
    // Method begins at RVA 0x279a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Transform

  .method /*06000271*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Transform(valuetype System.Numerics.Vector2/*02000007*/ position,
                    valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 02 11 1C 11 1C 11 0C
  {
    // Method begins at RVA 0x279d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Transform

  .method /*06000272*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Transform(valuetype System.Numerics.Vector2/*02000007*/ 'value',
                    valuetype System.Numerics.Quaternion/*02000005*/ rotation) cil managed
  // SIG: 00 02 11 1C 11 1C 11 14
  {
    // Method begins at RVA 0x27a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Transform

  .method /*06000273*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          TransformNormal(valuetype System.Numerics.Vector2/*02000007*/ normal,
                          valuetype System.Numerics.Matrix3x2/*02000002*/ matrix) cil managed
  // SIG: 00 02 11 1C 11 1C 11 08
  {
    // Method begins at RVA 0x27a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::TransformNormal

  .method /*06000274*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          TransformNormal(valuetype System.Numerics.Vector2/*02000007*/ normal,
                          valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 02 11 1C 11 1C 11 0C
  {
    // Method begins at RVA 0x27a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::TransformNormal

  .method /*06000275*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Truncate(valuetype System.Numerics.Vector2/*02000007*/ 'vector') cil managed
  // SIG: 00 01 11 1C 11 1C
  {
    // Method begins at RVA 0x27a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Truncate

  .method /*06000276*/ public hidebysig static 
          valuetype System.Numerics.Vector2/*02000007*/ 
          Xor(valuetype System.Numerics.Vector2/*02000007*/ left,
              valuetype System.Numerics.Vector2/*02000007*/ right) cil managed
  // SIG: 00 02 11 1C 11 1C 11 1C
  {
    // Method begins at RVA 0x27ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector2::Xor

  .property /*17000017*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          AllBitsSet()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_AllBitsSet() /* 060001DE */
  } // end of property Vector2::AllBitsSet
  .property /*17000018*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          E()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_E() /* 060001DF */
  } // end of property Vector2::E
  .property /*17000019*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          Epsilon()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_Epsilon() /* 060001E0 */
  } // end of property Vector2::Epsilon
  .property /*1700001A*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          NaN()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_NaN() /* 060001E1 */
  } // end of property Vector2::NaN
  .property /*1700001B*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          NegativeInfinity()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_NegativeInfinity() /* 060001E2 */
  } // end of property Vector2::NegativeInfinity
  .property /*1700001C*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          NegativeZero()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_NegativeZero() /* 060001E3 */
  } // end of property Vector2::NegativeZero
  .property /*1700001D*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          One()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_One() /* 060001E4 */
  } // end of property Vector2::One
  .property /*1700001E*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          Pi()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_Pi() /* 060001E5 */
  } // end of property Vector2::Pi
  .property /*1700001F*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          PositiveInfinity()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_PositiveInfinity() /* 060001E6 */
  } // end of property Vector2::PositiveInfinity
  .property /*17000020*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          Tau()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_Tau() /* 060001E7 */
  } // end of property Vector2::Tau
  .property /*17000021*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          UnitX()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_UnitX() /* 060001E8 */
  } // end of property Vector2::UnitX
  .property /*17000022*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          UnitY()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_UnitY() /* 060001E9 */
  } // end of property Vector2::UnitY
  .property /*17000023*/ instance callconv(8) float32
          Item(int32)
  {
    .get instance float32 System.Numerics.Vector2/*02000007*/::get_Item(int32) /* 060001EA */
    .set instance void System.Numerics.Vector2/*02000007*/::set_Item(int32,
                                                                     float32) /* 060001EB */
  } // end of property Vector2::Item
  .property /*17000024*/ callconv(8) valuetype System.Numerics.Vector2/*02000007*/
          Zero()
  {
    .get valuetype System.Numerics.Vector2/*02000007*/ System.Numerics.Vector2/*02000007*/::get_Zero() /* 060001EC */
  } // end of property Vector2::Zero
} // end of class System.Numerics.Vector2

.class /*02000008*/ public sequential ansi sealed beforefieldinit System.Numerics.Vector3
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Vector3/*02000008*/>/*1B000006*/,
                  [System.Runtime/*23000001*/]System.IFormattable/*01000024*/
{
  .custom /*0C000023:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .field /*0400001F*/ public float32 X
  .field /*04000020*/ public float32 Y
  .field /*04000021*/ public float32 Z
  .method /*06000277*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Numerics.Vector2/*02000007*/ 'value',
                               float32 z) cil managed
  // SIG: 20 02 01 11 1C 0C
  {
    // Method begins at RVA 0x27af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::.ctor

  .method /*06000278*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x27b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::.ctor

  .method /*06000279*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 x,
                               float32 y,
                               float32 z) cil managed
  // SIG: 20 03 01 0C 0C 0C
  {
    // Method begins at RVA 0x27b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::.ctor

  .method /*0600027A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<float32> values) cil managed
  // SIG: 20 01 01 15 11 7D 01 0C
  {
    // Method begins at RVA 0x27b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::.ctor

  .method /*0600027B*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_AllBitsSet() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_AllBitsSet

  .method /*0600027C*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_E() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_E

  .method /*0600027D*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_Epsilon() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_Epsilon

  .method /*0600027E*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_NaN() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_NaN

  .method /*0600027F*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_NegativeInfinity() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_NegativeInfinity

  .method /*06000280*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_NegativeZero() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_NegativeZero

  .method /*06000281*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_One() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_One

  .method /*06000282*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_Pi() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_Pi

  .method /*06000283*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_PositiveInfinity() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_PositiveInfinity

  .method /*06000284*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_Tau() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_Tau

  .method /*06000285*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_UnitX() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_UnitX

  .method /*06000286*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_UnitY() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_UnitY

  .method /*06000287*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_UnitZ() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_UnitZ

  .method /*06000288*/ public hidebysig specialname 
          instance float32  get_Item(int32 index) cil managed
  // SIG: 20 01 0C 08
  {
    .custom /*0C0001E1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x27e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_Item

  .method /*06000289*/ public hidebysig specialname 
          instance void  set_Item(int32 index,
                                  float32 'value') cil managed
  // SIG: 20 02 01 08 0C
  {
    // Method begins at RVA 0x27e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::set_Item

  .method /*0600028A*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          get_Zero() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x27e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::get_Zero

  .method /*0600028B*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Abs(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x27eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Abs

  .method /*0600028C*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Add(valuetype System.Numerics.Vector3/*02000008*/ left,
              valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x27ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Add

  .method /*0600028D*/ public hidebysig static 
          bool  All(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                    float32 'value') cil managed
  // SIG: 00 02 02 11 20 0C
  {
    // Method begins at RVA 0x27f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::All

  .method /*0600028E*/ public hidebysig static 
          bool  AllWhereAllBitsSet(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 02 11 20
  {
    // Method begins at RVA 0x27f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::AllWhereAllBitsSet

  .method /*0600028F*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          AndNot(valuetype System.Numerics.Vector3/*02000008*/ left,
                 valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x27f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::AndNot

  .method /*06000290*/ public hidebysig static 
          bool  Any(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                    float32 'value') cil managed
  // SIG: 00 02 02 11 20 0C
  {
    // Method begins at RVA 0x27fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Any

  .method /*06000291*/ public hidebysig static 
          bool  AnyWhereAllBitsSet(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 02 11 20
  {
    // Method begins at RVA 0x27fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::AnyWhereAllBitsSet

  .method /*06000292*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          BitwiseAnd(valuetype System.Numerics.Vector3/*02000008*/ left,
                     valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2800
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::BitwiseAnd

  .method /*06000293*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          BitwiseOr(valuetype System.Numerics.Vector3/*02000008*/ left,
                    valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2803
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::BitwiseOr

  .method /*06000294*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Clamp(valuetype System.Numerics.Vector3/*02000008*/ value1,
                valuetype System.Numerics.Vector3/*02000008*/ min,
                valuetype System.Numerics.Vector3/*02000008*/ max) cil managed
  // SIG: 00 03 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2806
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Clamp

  .method /*06000295*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          ClampNative(valuetype System.Numerics.Vector3/*02000008*/ value1,
                      valuetype System.Numerics.Vector3/*02000008*/ min,
                      valuetype System.Numerics.Vector3/*02000008*/ max) cil managed
  // SIG: 00 03 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2809
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::ClampNative

  .method /*06000296*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          ConditionalSelect(valuetype System.Numerics.Vector3/*02000008*/ condition,
                            valuetype System.Numerics.Vector3/*02000008*/ left,
                            valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 03 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x280c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::ConditionalSelect

  .method /*06000297*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          CopySign(valuetype System.Numerics.Vector3/*02000008*/ 'value',
                   valuetype System.Numerics.Vector3/*02000008*/ sign) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x280f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::CopySign

  .method /*06000298*/ public hidebysig static 
          int32  Count(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                       float32 'value') cil managed
  // SIG: 00 02 08 11 20 0C
  {
    // Method begins at RVA 0x2812
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Count

  .method /*06000299*/ public hidebysig static 
          int32  CountWhereAllBitsSet(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 08 11 20
  {
    // Method begins at RVA 0x2815
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::CountWhereAllBitsSet

  .method /*0600029A*/ public hidebysig instance void 
          CopyTo(float32[] 'array') cil managed
  // SIG: 20 01 01 1D 0C
  {
    .custom /*0C0001F3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0001F4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2818
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector3::CopyTo

  .method /*0600029B*/ public hidebysig instance void 
          CopyTo(float32[] 'array',
                 int32 index) cil managed
  // SIG: 20 02 01 1D 0C 08
  {
    .custom /*0C0001F6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0001F7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x281a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector3::CopyTo

  .method /*0600029C*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<float32> destination) cil managed
  // SIG: 20 01 01 15 11 80 95 01 0C
  {
    .custom /*0C0001F9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x281c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector3::CopyTo

  .method /*0600029D*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Cos(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x281e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Cos

  .method /*0600029E*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Create(float32 'value') cil managed
  // SIG: 00 01 11 20 0C
  {
    // Method begins at RVA 0x2821
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Create

  .method /*0600029F*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Create(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                 float32 z) cil managed
  // SIG: 00 02 11 20 11 1C 0C
  {
    // Method begins at RVA 0x2824
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Create

  .method /*060002A0*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Create(float32 x,
                 float32 y,
                 float32 z) cil managed
  // SIG: 00 03 11 20 0C 0C 0C
  {
    // Method begins at RVA 0x2827
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Create

  .method /*060002A1*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Create(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<float32> values) cil managed
  // SIG: 00 01 11 20 15 11 7D 01 0C
  {
    // Method begins at RVA 0x282a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Create

  .method /*060002A2*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          CreateScalar(float32 x) cil managed
  // SIG: 00 01 11 20 0C
  {
    // Method begins at RVA 0x282d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::CreateScalar

  .method /*060002A3*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          CreateScalarUnsafe(float32 x) cil managed
  // SIG: 00 01 11 20 0C
  {
    // Method begins at RVA 0x2830
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::CreateScalarUnsafe

  .method /*060002A4*/ public hidebysig instance bool 
          TryCopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<float32> destination) cil managed
  // SIG: 20 01 02 15 11 80 95 01 0C
  {
    .custom /*0C0001FD:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2833
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::TryCopyTo

  .method /*060002A5*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Cross(valuetype System.Numerics.Vector3/*02000008*/ vector1,
                valuetype System.Numerics.Vector3/*02000008*/ vector2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2836
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Cross

  .method /*060002A6*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          DegreesToRadians(valuetype System.Numerics.Vector3/*02000008*/ degrees) cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2839
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::DegreesToRadians

  .method /*060002A7*/ public hidebysig static 
          float32  Distance(valuetype System.Numerics.Vector3/*02000008*/ value1,
                            valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 0C 11 20 11 20
  {
    // Method begins at RVA 0x283c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Distance

  .method /*060002A8*/ public hidebysig static 
          float32  DistanceSquared(valuetype System.Numerics.Vector3/*02000008*/ value1,
                                   valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 0C 11 20 11 20
  {
    // Method begins at RVA 0x283f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::DistanceSquared

  .method /*060002A9*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Divide(valuetype System.Numerics.Vector3/*02000008*/ left,
                 valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2842
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Divide

  .method /*060002AA*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Divide(valuetype System.Numerics.Vector3/*02000008*/ left,
                 float32 divisor) cil managed
  // SIG: 00 02 11 20 11 20 0C
  {
    // Method begins at RVA 0x2845
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Divide

  .method /*060002AB*/ public hidebysig static 
          float32  Dot(valuetype System.Numerics.Vector3/*02000008*/ vector1,
                       valuetype System.Numerics.Vector3/*02000008*/ vector2) cil managed
  // SIG: 00 02 0C 11 20 11 20
  {
    // Method begins at RVA 0x2848
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Dot

  .method /*060002AC*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Numerics.Vector3/*02000008*/ other) cil managed
  // SIG: 20 01 02 11 20
  {
    .custom /*0C000206:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x284b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Equals

  .method /*060002AD*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000208:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000209:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080004CD*/ 
    .custom /*0C000282:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001C*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x284e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Equals

  .method /*060002AE*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Equals(valuetype System.Numerics.Vector3/*02000008*/ left,
                 valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2851
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Equals

  .method /*060002AF*/ public hidebysig static 
          bool  EqualsAll(valuetype System.Numerics.Vector3/*02000008*/ left,
                          valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x2854
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::EqualsAll

  .method /*060002B0*/ public hidebysig static 
          bool  EqualsAny(valuetype System.Numerics.Vector3/*02000008*/ left,
                          valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x2857
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::EqualsAny

  .method /*060002B1*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Exp(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x285a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Exp

  .method /*060002B2*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          FusedMultiplyAdd(valuetype System.Numerics.Vector3/*02000008*/ left,
                           valuetype System.Numerics.Vector3/*02000008*/ right,
                           valuetype System.Numerics.Vector3/*02000008*/ addend) cil managed
  // SIG: 00 03 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x285d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::FusedMultiplyAdd

  .method /*060002B3*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000210:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2860
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::GetHashCode

  .method /*060002B4*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          GreaterThan(valuetype System.Numerics.Vector3/*02000008*/ left,
                      valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2863
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::GreaterThan

  .method /*060002B5*/ public hidebysig static 
          bool  GreaterThanAll(valuetype System.Numerics.Vector3/*02000008*/ left,
                               valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x2866
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::GreaterThanAll

  .method /*060002B6*/ public hidebysig static 
          bool  GreaterThanAny(valuetype System.Numerics.Vector3/*02000008*/ left,
                               valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x2869
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::GreaterThanAny

  .method /*060002B7*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          GreaterThanOrEqual(valuetype System.Numerics.Vector3/*02000008*/ left,
                             valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x286c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::GreaterThanOrEqual

  .method /*060002B8*/ public hidebysig static 
          bool  GreaterThanOrEqualAll(valuetype System.Numerics.Vector3/*02000008*/ left,
                                      valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x286f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::GreaterThanOrEqualAll

  .method /*060002B9*/ public hidebysig static 
          bool  GreaterThanOrEqualAny(valuetype System.Numerics.Vector3/*02000008*/ left,
                                      valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x2872
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::GreaterThanOrEqualAny

  .method /*060002BA*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Hypot(valuetype System.Numerics.Vector3/*02000008*/ x,
                valuetype System.Numerics.Vector3/*02000008*/ y) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2875
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Hypot

  .method /*060002BB*/ public hidebysig static 
          int32  IndexOf(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                         float32 'value') cil managed
  // SIG: 00 02 08 11 20 0C
  {
    // Method begins at RVA 0x2878
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IndexOf

  .method /*060002BC*/ public hidebysig static 
          int32  IndexOfWhereAllBitsSet(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 08 11 20
  {
    // Method begins at RVA 0x287b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IndexOfWhereAllBitsSet

  .method /*060002BD*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsEvenInteger(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x287e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsEvenInteger

  .method /*060002BE*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsFinite(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2881
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsFinite

  .method /*060002BF*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsInfinity(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2884
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsInfinity

  .method /*060002C0*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsInteger(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2887
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsInteger

  .method /*060002C1*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsNaN(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x288a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsNaN

  .method /*060002C2*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsNegative(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x288d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsNegative

  .method /*060002C3*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsNegativeInfinity(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2890
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsNegativeInfinity

  .method /*060002C4*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsNormal(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2893
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsNormal

  .method /*060002C5*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsOddInteger(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2896
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsOddInteger

  .method /*060002C6*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsPositive(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2899
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsPositive

  .method /*060002C7*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsPositiveInfinity(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x289c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsPositiveInfinity

  .method /*060002C8*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsSubnormal(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x289f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsSubnormal

  .method /*060002C9*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          IsZero(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x28a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::IsZero

  .method /*060002CA*/ public hidebysig static 
          int32  LastIndexOf(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                             float32 'value') cil managed
  // SIG: 00 02 08 11 20 0C
  {
    // Method begins at RVA 0x28a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LastIndexOf

  .method /*060002CB*/ public hidebysig static 
          int32  LastIndexOfWhereAllBitsSet(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 08 11 20
  {
    // Method begins at RVA 0x28a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LastIndexOfWhereAllBitsSet

  .method /*060002CC*/ public hidebysig instance float32 
          Length() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000225:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x28ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Length

  .method /*060002CD*/ public hidebysig instance float32 
          LengthSquared() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000226:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x28ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LengthSquared

  .method /*060002CE*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Lerp(valuetype System.Numerics.Vector3/*02000008*/ value1,
               valuetype System.Numerics.Vector3/*02000008*/ value2,
               float32 amount) cil managed
  // SIG: 00 03 11 20 11 20 11 20 0C
  {
    // Method begins at RVA 0x28b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Lerp

  .method /*060002CF*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Lerp(valuetype System.Numerics.Vector3/*02000008*/ value1,
               valuetype System.Numerics.Vector3/*02000008*/ value2,
               valuetype System.Numerics.Vector3/*02000008*/ amount) cil managed
  // SIG: 00 03 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Lerp

  .method /*060002D0*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          LessThan(valuetype System.Numerics.Vector3/*02000008*/ left,
                   valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LessThan

  .method /*060002D1*/ public hidebysig static 
          bool  LessThanAll(valuetype System.Numerics.Vector3/*02000008*/ left,
                            valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x28ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LessThanAll

  .method /*060002D2*/ public hidebysig static 
          bool  LessThanAny(valuetype System.Numerics.Vector3/*02000008*/ left,
                            valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x28bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LessThanAny

  .method /*060002D3*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          LessThanOrEqual(valuetype System.Numerics.Vector3/*02000008*/ left,
                          valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LessThanOrEqual

  .method /*060002D4*/ public hidebysig static 
          bool  LessThanOrEqualAll(valuetype System.Numerics.Vector3/*02000008*/ left,
                                   valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x28c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LessThanOrEqualAll

  .method /*060002D5*/ public hidebysig static 
          bool  LessThanOrEqualAny(valuetype System.Numerics.Vector3/*02000008*/ left,
                                   valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x28c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LessThanOrEqualAny

  .method /*060002D6*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Load(float32* source) cil managed
  // SIG: 00 01 11 20 0F 0C
  {
    .custom /*0C000227:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x28c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Load

  .method /*060002D7*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          LoadAligned(float32* source) cil managed
  // SIG: 00 01 11 20 0F 0C
  {
    .custom /*0C000228:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x28cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LoadAligned

  .method /*060002D8*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          LoadAlignedNonTemporal(float32* source) cil managed
  // SIG: 00 01 11 20 0F 0C
  {
    .custom /*0C000229:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x28cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LoadAlignedNonTemporal

  .method /*060002D9*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          LoadUnsafe([in] float32& source) cil managed
  // SIG: 00 01 11 20 10 0C
  {
    .param [1]/*0800050E*/ 
    .custom /*0C000283:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000020*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x28d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LoadUnsafe

  .method /*060002DA*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          LoadUnsafe([in] float32& source,
                     native uint elementOffset) cil managed
  // SIG: 00 02 11 20 10 0C 19
  {
    .custom /*0C00022A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800050F*/ 
    .custom /*0C000284:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000020*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x28d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::LoadUnsafe

  .method /*060002DB*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Log(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x28d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Log

  .method /*060002DC*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Log2(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x28db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Log2

  .method /*060002DD*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Max(valuetype System.Numerics.Vector3/*02000008*/ value1,
              valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Max

  .method /*060002DE*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MaxMagnitude(valuetype System.Numerics.Vector3/*02000008*/ value1,
                       valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MaxMagnitude

  .method /*060002DF*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MaxMagnitudeNumber(valuetype System.Numerics.Vector3/*02000008*/ value1,
                             valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MaxMagnitudeNumber

  .method /*060002E0*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MaxNative(valuetype System.Numerics.Vector3/*02000008*/ value1,
                    valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MaxNative

  .method /*060002E1*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MaxNumber(valuetype System.Numerics.Vector3/*02000008*/ value1,
                    valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MaxNumber

  .method /*060002E2*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Min(valuetype System.Numerics.Vector3/*02000008*/ value1,
              valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Min

  .method /*060002E3*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MinMagnitude(valuetype System.Numerics.Vector3/*02000008*/ value1,
                       valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MinMagnitude

  .method /*060002E4*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MinMagnitudeNumber(valuetype System.Numerics.Vector3/*02000008*/ value1,
                             valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MinMagnitudeNumber

  .method /*060002E5*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MinNative(valuetype System.Numerics.Vector3/*02000008*/ value1,
                    valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MinNative

  .method /*060002E6*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MinNumber(valuetype System.Numerics.Vector3/*02000008*/ value1,
                    valuetype System.Numerics.Vector3/*02000008*/ value2) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MinNumber

  .method /*060002E7*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Multiply(valuetype System.Numerics.Vector3/*02000008*/ left,
                   valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x28fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Multiply

  .method /*060002E8*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Multiply(valuetype System.Numerics.Vector3/*02000008*/ left,
                   float32 right) cil managed
  // SIG: 00 02 11 20 11 20 0C
  {
    // Method begins at RVA 0x28ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Multiply

  .method /*060002E9*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Multiply(float32 left,
                   valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 0C 11 20
  {
    // Method begins at RVA 0x2902
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Multiply

  .method /*060002EA*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          MultiplyAddEstimate(valuetype System.Numerics.Vector3/*02000008*/ left,
                              valuetype System.Numerics.Vector3/*02000008*/ right,
                              valuetype System.Numerics.Vector3/*02000008*/ addend) cil managed
  // SIG: 00 03 11 20 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2905
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::MultiplyAddEstimate

  .method /*060002EB*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Negate(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2908
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Negate

  .method /*060002EC*/ public hidebysig static 
          bool  None(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                     float32 'value') cil managed
  // SIG: 00 02 02 11 20 0C
  {
    // Method begins at RVA 0x290b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::None

  .method /*060002ED*/ public hidebysig static 
          bool  NoneWhereAllBitsSet(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 02 11 20
  {
    // Method begins at RVA 0x290e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::NoneWhereAllBitsSet

  .method /*060002EE*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Normalize(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2911
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Normalize

  .method /*060002EF*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          OnesComplement(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2914
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::OnesComplement

  .method /*060002F0*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_Addition(valuetype System.Numerics.Vector3/*02000008*/ left,
                      valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2917
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Addition

  .method /*060002F1*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_BitwiseAnd(valuetype System.Numerics.Vector3/*02000008*/ left,
                        valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x291a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_BitwiseAnd

  .method /*060002F2*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_BitwiseOr(valuetype System.Numerics.Vector3/*02000008*/ left,
                       valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x291d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_BitwiseOr

  .method /*060002F3*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_Division(valuetype System.Numerics.Vector3/*02000008*/ left,
                      valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2920
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Division

  .method /*060002F4*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_Division(valuetype System.Numerics.Vector3/*02000008*/ value1,
                      float32 value2) cil managed
  // SIG: 00 02 11 20 11 20 0C
  {
    // Method begins at RVA 0x2923
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Division

  .method /*060002F5*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_ExclusiveOr(valuetype System.Numerics.Vector3/*02000008*/ left,
                         valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2926
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_ExclusiveOr

  .method /*060002F6*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Numerics.Vector3/*02000008*/ left,
                            valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x2929
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Equality

  .method /*060002F7*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_LeftShift(valuetype System.Numerics.Vector3/*02000008*/ 'value',
                       int32 shiftAmount) cil managed
  // SIG: 00 02 11 20 11 20 08
  {
    // Method begins at RVA 0x292c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_LeftShift

  .method /*060002F8*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Numerics.Vector3/*02000008*/ left,
                              valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 02 11 20 11 20
  {
    // Method begins at RVA 0x292f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Inequality

  .method /*060002F9*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_Multiply(valuetype System.Numerics.Vector3/*02000008*/ left,
                      valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2932
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Multiply

  .method /*060002FA*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_Multiply(valuetype System.Numerics.Vector3/*02000008*/ left,
                      float32 right) cil managed
  // SIG: 00 02 11 20 11 20 0C
  {
    // Method begins at RVA 0x2935
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Multiply

  .method /*060002FB*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_Multiply(float32 left,
                      valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 0C 11 20
  {
    // Method begins at RVA 0x2938
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Multiply

  .method /*060002FC*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_OnesComplement(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x293b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_OnesComplement

  .method /*060002FD*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_RightShift(valuetype System.Numerics.Vector3/*02000008*/ 'value',
                        int32 shiftAmount) cil managed
  // SIG: 00 02 11 20 11 20 08
  {
    // Method begins at RVA 0x293e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_RightShift

  .method /*060002FE*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_Subtraction(valuetype System.Numerics.Vector3/*02000008*/ left,
                         valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2941
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_Subtraction

  .method /*060002FF*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_UnaryNegation(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2944
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_UnaryNegation

  .method /*06000300*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_UnaryPlus(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2947
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_UnaryPlus

  .method /*06000301*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          op_UnsignedRightShift(valuetype System.Numerics.Vector3/*02000008*/ 'value',
                                int32 shiftAmount) cil managed
  // SIG: 00 02 11 20 11 20 08
  {
    // Method begins at RVA 0x294a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::op_UnsignedRightShift

  .method /*06000302*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          RadiansToDegrees(valuetype System.Numerics.Vector3/*02000008*/ radians) cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x294d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::RadiansToDegrees

  .method /*06000303*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Reflect(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                  valuetype System.Numerics.Vector3/*02000008*/ normal) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2950
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Reflect

  .method /*06000304*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Round(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2953
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Round

  .method /*06000305*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Round(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                valuetype [System.Runtime/*23000001*/]System.MidpointRounding/*01000021*/ mode) cil managed
  // SIG: 00 02 11 20 11 20 11 80 85
  {
    // Method begins at RVA 0x2956
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Round

  .method /*06000306*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Shuffle(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                  uint8 xIndex,
                  uint8 yIndex,
                  uint8 zIndex) cil managed
  // SIG: 00 04 11 20 11 20 05 05 05
  {
    // Method begins at RVA 0x2959
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Shuffle

  .method /*06000307*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Sin(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x295c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Sin

  .method /*06000308*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000023*/<valuetype System.Numerics.Vector3/*02000008*/,valuetype System.Numerics.Vector3/*02000008*/> 
          SinCos(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 15 11 80 8D 02 11 20 11 20 11 20
  {
    .param [0]/*08000562*/ 
    .custom /*0C000285:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000022*/::.ctor(string[]) /* 0A00001C */ = ( 01 00 02 00 00 00 03 53 69 6E 03 43 6F 73 00 00 ) // .......Sin.Cos..
    // Method begins at RVA 0x295f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::SinCos

  .method /*06000309*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          SquareRoot(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x2962
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::SquareRoot

  .method /*0600030A*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Subtract(valuetype System.Numerics.Vector3/*02000008*/ left,
                   valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2965
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Subtract

  .method /*0600030B*/ public hidebysig static 
          float32  Sum(valuetype System.Numerics.Vector3/*02000008*/ 'value') cil managed
  // SIG: 00 01 0C 11 20
  {
    // Method begins at RVA 0x2968
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Sum

  .method /*0600030C*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000231:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000232:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x296b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::ToString

  .method /*0600030D*/ public hidebysig instance string 
          ToString(string format) cil managed
  // SIG: 20 01 0E 0E
  {
    .custom /*0C000233:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000234:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000568*/ 
    .custom /*0C000286:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .custom /*0C000287:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000026*/::.ctor(string) /* 0A00001D */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x296e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::ToString

  .method /*0600030E*/ public hidebysig newslot virtual final 
          instance string  ToString(string format,
                                    class [System.Runtime/*23000001*/]System.IFormatProvider/*01000027*/ formatProvider) cil managed
  // SIG: 20 02 0E 0E 12 80 9D
  {
    .custom /*0C000235:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000236:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000569*/ 
    .custom /*0C000288:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800056A*/ 
    .custom /*0C000289:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000026*/::.ctor(string) /* 0A00001D */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2971
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::ToString

  .method /*0600030F*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Transform(valuetype System.Numerics.Vector3/*02000008*/ position,
                    valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 02 11 20 11 20 11 0C
  {
    // Method begins at RVA 0x2974
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Transform

  .method /*06000310*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Transform(valuetype System.Numerics.Vector3/*02000008*/ 'value',
                    valuetype System.Numerics.Quaternion/*02000005*/ rotation) cil managed
  // SIG: 00 02 11 20 11 20 11 14
  {
    // Method begins at RVA 0x2977
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Transform

  .method /*06000311*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          TransformNormal(valuetype System.Numerics.Vector3/*02000008*/ normal,
                          valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 02 11 20 11 20 11 0C
  {
    // Method begins at RVA 0x297a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::TransformNormal

  .method /*06000312*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Truncate(valuetype System.Numerics.Vector3/*02000008*/ 'vector') cil managed
  // SIG: 00 01 11 20 11 20
  {
    // Method begins at RVA 0x297d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Truncate

  .method /*06000313*/ public hidebysig static 
          valuetype System.Numerics.Vector3/*02000008*/ 
          Xor(valuetype System.Numerics.Vector3/*02000008*/ left,
              valuetype System.Numerics.Vector3/*02000008*/ right) cil managed
  // SIG: 00 02 11 20 11 20 11 20
  {
    // Method begins at RVA 0x2980
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector3::Xor

  .property /*17000025*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          AllBitsSet()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_AllBitsSet() /* 0600027B */
  } // end of property Vector3::AllBitsSet
  .property /*17000026*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          E()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_E() /* 0600027C */
  } // end of property Vector3::E
  .property /*17000027*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          Epsilon()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_Epsilon() /* 0600027D */
  } // end of property Vector3::Epsilon
  .property /*17000028*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          NaN()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_NaN() /* 0600027E */
  } // end of property Vector3::NaN
  .property /*17000029*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          NegativeInfinity()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_NegativeInfinity() /* 0600027F */
  } // end of property Vector3::NegativeInfinity
  .property /*1700002A*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          NegativeZero()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_NegativeZero() /* 06000280 */
  } // end of property Vector3::NegativeZero
  .property /*1700002B*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          One()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_One() /* 06000281 */
  } // end of property Vector3::One
  .property /*1700002C*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          Pi()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_Pi() /* 06000282 */
  } // end of property Vector3::Pi
  .property /*1700002D*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          PositiveInfinity()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_PositiveInfinity() /* 06000283 */
  } // end of property Vector3::PositiveInfinity
  .property /*1700002E*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          Tau()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_Tau() /* 06000284 */
  } // end of property Vector3::Tau
  .property /*1700002F*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          UnitX()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_UnitX() /* 06000285 */
  } // end of property Vector3::UnitX
  .property /*17000030*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          UnitY()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_UnitY() /* 06000286 */
  } // end of property Vector3::UnitY
  .property /*17000031*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          UnitZ()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_UnitZ() /* 06000287 */
  } // end of property Vector3::UnitZ
  .property /*17000032*/ instance callconv(8) float32
          Item(int32)
  {
    .get instance float32 System.Numerics.Vector3/*02000008*/::get_Item(int32) /* 06000288 */
    .set instance void System.Numerics.Vector3/*02000008*/::set_Item(int32,
                                                                     float32) /* 06000289 */
  } // end of property Vector3::Item
  .property /*17000033*/ callconv(8) valuetype System.Numerics.Vector3/*02000008*/
          Zero()
  {
    .get valuetype System.Numerics.Vector3/*02000008*/ System.Numerics.Vector3/*02000008*/::get_Zero() /* 0600028A */
  } // end of property Vector3::Zero
} // end of class System.Numerics.Vector3

.class /*02000009*/ public sequential ansi sealed beforefieldinit System.Numerics.Vector4
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Vector4/*02000009*/>/*1B000007*/,
                  [System.Runtime/*23000001*/]System.IFormattable/*01000024*/
{
  .custom /*0C000025:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .field /*04000022*/ public float32 X
  .field /*04000023*/ public float32 Y
  .field /*04000024*/ public float32 Z
  .field /*04000025*/ public float32 W
  .method /*06000314*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Numerics.Vector2/*02000007*/ 'value',
                               float32 z,
                               float32 w) cil managed
  // SIG: 20 03 01 11 1C 0C 0C
  {
    // Method begins at RVA 0x2983
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::.ctor

  .method /*06000315*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Numerics.Vector3/*02000008*/ 'value',
                               float32 w) cil managed
  // SIG: 20 02 01 11 20 0C
  {
    // Method begins at RVA 0x2986
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::.ctor

  .method /*06000316*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x2989
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::.ctor

  .method /*06000317*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 x,
                               float32 y,
                               float32 z,
                               float32 w) cil managed
  // SIG: 20 04 01 0C 0C 0C 0C
  {
    // Method begins at RVA 0x298c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::.ctor

  .method /*06000318*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<float32> values) cil managed
  // SIG: 20 01 01 15 11 7D 01 0C
  {
    // Method begins at RVA 0x298f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::.ctor

  .method /*06000319*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_AllBitsSet() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x2992
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_AllBitsSet

  .method /*0600031A*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_E() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x2995
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_E

  .method /*0600031B*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_Epsilon() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x2998
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_Epsilon

  .method /*0600031C*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_NaN() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x299b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_NaN

  .method /*0600031D*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_NegativeInfinity() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x299e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_NegativeInfinity

  .method /*0600031E*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_NegativeZero() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_NegativeZero

  .method /*0600031F*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_One() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_One

  .method /*06000320*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_Pi() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_Pi

  .method /*06000321*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_PositiveInfinity() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_PositiveInfinity

  .method /*06000322*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_Tau() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_Tau

  .method /*06000323*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_UnitW() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_UnitW

  .method /*06000324*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_UnitX() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_UnitX

  .method /*06000325*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_UnitY() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_UnitY

  .method /*06000326*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_UnitZ() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_UnitZ

  .method /*06000327*/ public hidebysig specialname 
          instance float32  get_Item(int32 index) cil managed
  // SIG: 20 01 0C 08
  {
    .custom /*0C000237:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x29bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_Item

  .method /*06000328*/ public hidebysig specialname 
          instance void  set_Item(int32 index,
                                  float32 'value') cil managed
  // SIG: 20 02 01 08 0C
  {
    // Method begins at RVA 0x29bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::set_Item

  .method /*06000329*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          get_Zero() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x29c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::get_Zero

  .method /*0600032A*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Abs(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x29c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Abs

  .method /*0600032B*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Add(valuetype System.Numerics.Vector4/*02000009*/ left,
              valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x29c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Add

  .method /*0600032C*/ public hidebysig static 
          bool  All(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                    float32 'value') cil managed
  // SIG: 00 02 02 11 24 0C
  {
    // Method begins at RVA 0x29cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::All

  .method /*0600032D*/ public hidebysig static 
          bool  AllWhereAllBitsSet(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 02 11 24
  {
    // Method begins at RVA 0x29ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::AllWhereAllBitsSet

  .method /*0600032E*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          AndNot(valuetype System.Numerics.Vector4/*02000009*/ left,
                 valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x29d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::AndNot

  .method /*0600032F*/ public hidebysig static 
          bool  Any(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                    float32 'value') cil managed
  // SIG: 00 02 02 11 24 0C
  {
    // Method begins at RVA 0x29d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Any

  .method /*06000330*/ public hidebysig static 
          bool  AnyWhereAllBitsSet(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 02 11 24
  {
    // Method begins at RVA 0x29d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::AnyWhereAllBitsSet

  .method /*06000331*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          BitwiseAnd(valuetype System.Numerics.Vector4/*02000009*/ left,
                     valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x29da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::BitwiseAnd

  .method /*06000332*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          BitwiseOr(valuetype System.Numerics.Vector4/*02000009*/ left,
                    valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x29dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::BitwiseOr

  .method /*06000333*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Clamp(valuetype System.Numerics.Vector4/*02000009*/ value1,
                valuetype System.Numerics.Vector4/*02000009*/ min,
                valuetype System.Numerics.Vector4/*02000009*/ max) cil managed
  // SIG: 00 03 11 24 11 24 11 24 11 24
  {
    // Method begins at RVA 0x29e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Clamp

  .method /*06000334*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          ClampNative(valuetype System.Numerics.Vector4/*02000009*/ value1,
                      valuetype System.Numerics.Vector4/*02000009*/ min,
                      valuetype System.Numerics.Vector4/*02000009*/ max) cil managed
  // SIG: 00 03 11 24 11 24 11 24 11 24
  {
    // Method begins at RVA 0x29e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::ClampNative

  .method /*06000335*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          ConditionalSelect(valuetype System.Numerics.Vector4/*02000009*/ condition,
                            valuetype System.Numerics.Vector4/*02000009*/ left,
                            valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 03 11 24 11 24 11 24 11 24
  {
    // Method begins at RVA 0x29e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::ConditionalSelect

  .method /*06000336*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          CopySign(valuetype System.Numerics.Vector4/*02000009*/ 'value',
                   valuetype System.Numerics.Vector4/*02000009*/ sign) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x29e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::CopySign

  .method /*06000337*/ public hidebysig static 
          int32  Count(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                       float32 'value') cil managed
  // SIG: 00 02 08 11 24 0C
  {
    // Method begins at RVA 0x29ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Count

  .method /*06000338*/ public hidebysig static 
          int32  CountWhereAllBitsSet(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 08 11 24
  {
    // Method begins at RVA 0x29ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::CountWhereAllBitsSet

  .method /*06000339*/ public hidebysig instance void 
          CopyTo(float32[] 'array') cil managed
  // SIG: 20 01 01 1D 0C
  {
    .custom /*0C00023E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00023F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29f2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector4::CopyTo

  .method /*0600033A*/ public hidebysig instance void 
          CopyTo(float32[] 'array',
                 int32 index) cil managed
  // SIG: 20 02 01 1D 0C 08
  {
    .custom /*0C000240:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000241:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x29f4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector4::CopyTo

  .method /*0600033B*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<float32> destination) cil managed
  // SIG: 20 01 01 15 11 80 95 01 0C
  {
    .custom /*0C000242:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x29f6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector4::CopyTo

  .method /*0600033C*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Cos(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x29f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Cos

  .method /*0600033D*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Create(float32 'value') cil managed
  // SIG: 00 01 11 24 0C
  {
    // Method begins at RVA 0x29fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Create

  .method /*0600033E*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Create(valuetype System.Numerics.Vector2/*02000007*/ 'vector',
                 float32 z,
                 float32 w) cil managed
  // SIG: 00 03 11 24 11 1C 0C 0C
  {
    // Method begins at RVA 0x29fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Create

  .method /*0600033F*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Create(valuetype System.Numerics.Vector3/*02000008*/ 'vector',
                 float32 w) cil managed
  // SIG: 00 02 11 24 11 20 0C
  {
    // Method begins at RVA 0x2a01
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Create

  .method /*06000340*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Create(float32 x,
                 float32 y,
                 float32 z,
                 float32 w) cil managed
  // SIG: 00 04 11 24 0C 0C 0C 0C
  {
    // Method begins at RVA 0x2a04
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Create

  .method /*06000341*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Create(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<float32> values) cil managed
  // SIG: 00 01 11 24 15 11 7D 01 0C
  {
    // Method begins at RVA 0x2a07
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Create

  .method /*06000342*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          CreateScalar(float32 x) cil managed
  // SIG: 00 01 11 24 0C
  {
    // Method begins at RVA 0x2a0a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::CreateScalar

  .method /*06000343*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          CreateScalarUnsafe(float32 x) cil managed
  // SIG: 00 01 11 24 0C
  {
    // Method begins at RVA 0x2a0d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::CreateScalarUnsafe

  .method /*06000344*/ public hidebysig instance bool 
          TryCopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<float32> destination) cil managed
  // SIG: 20 01 02 15 11 80 95 01 0C
  {
    .custom /*0C000245:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2a10
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::TryCopyTo

  .method /*06000345*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          DegreesToRadians(valuetype System.Numerics.Vector4/*02000009*/ degrees) cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a13
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::DegreesToRadians

  .method /*06000346*/ public hidebysig static 
          float32  Distance(valuetype System.Numerics.Vector4/*02000009*/ value1,
                            valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 0C 11 24 11 24
  {
    // Method begins at RVA 0x2a16
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Distance

  .method /*06000347*/ public hidebysig static 
          float32  DistanceSquared(valuetype System.Numerics.Vector4/*02000009*/ value1,
                                   valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 0C 11 24 11 24
  {
    // Method begins at RVA 0x2a19
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::DistanceSquared

  .method /*06000348*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Divide(valuetype System.Numerics.Vector4/*02000009*/ left,
                 valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a1c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Divide

  .method /*06000349*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Divide(valuetype System.Numerics.Vector4/*02000009*/ left,
                 float32 divisor) cil managed
  // SIG: 00 02 11 24 11 24 0C
  {
    // Method begins at RVA 0x2a1f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Divide

  .method /*0600034A*/ public hidebysig static 
          float32  Dot(valuetype System.Numerics.Vector4/*02000009*/ vector1,
                       valuetype System.Numerics.Vector4/*02000009*/ vector2) cil managed
  // SIG: 00 02 0C 11 24 11 24
  {
    // Method begins at RVA 0x2a22
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Dot

  .method /*0600034B*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Cross(valuetype System.Numerics.Vector4/*02000009*/ vector1,
                valuetype System.Numerics.Vector4/*02000009*/ vector2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a25
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Cross

  .method /*0600034C*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Numerics.Vector4/*02000009*/ other) cil managed
  // SIG: 20 01 02 11 24
  {
    .custom /*0C000248:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2a28
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Equals

  .method /*0600034D*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00024A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00024B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080005C1*/ 
    .custom /*0C00028A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001C*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2a2b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Equals

  .method /*0600034E*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Equals(valuetype System.Numerics.Vector4/*02000009*/ left,
                 valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a2e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Equals

  .method /*0600034F*/ public hidebysig static 
          bool  EqualsAll(valuetype System.Numerics.Vector4/*02000009*/ left,
                          valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2a31
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::EqualsAll

  .method /*06000350*/ public hidebysig static 
          bool  EqualsAny(valuetype System.Numerics.Vector4/*02000009*/ left,
                          valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2a34
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::EqualsAny

  .method /*06000351*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Exp(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a37
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Exp

  .method /*06000352*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          FusedMultiplyAdd(valuetype System.Numerics.Vector4/*02000009*/ left,
                           valuetype System.Numerics.Vector4/*02000009*/ right,
                           valuetype System.Numerics.Vector4/*02000009*/ addend) cil managed
  // SIG: 00 03 11 24 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a3a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::FusedMultiplyAdd

  .method /*06000353*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00024C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2a3d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::GetHashCode

  .method /*06000354*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          GreaterThan(valuetype System.Numerics.Vector4/*02000009*/ left,
                      valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a40
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::GreaterThan

  .method /*06000355*/ public hidebysig static 
          bool  GreaterThanAll(valuetype System.Numerics.Vector4/*02000009*/ left,
                               valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2a43
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::GreaterThanAll

  .method /*06000356*/ public hidebysig static 
          bool  GreaterThanAny(valuetype System.Numerics.Vector4/*02000009*/ left,
                               valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2a46
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::GreaterThanAny

  .method /*06000357*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          GreaterThanOrEqual(valuetype System.Numerics.Vector4/*02000009*/ left,
                             valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a49
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::GreaterThanOrEqual

  .method /*06000358*/ public hidebysig static 
          bool  GreaterThanOrEqualAll(valuetype System.Numerics.Vector4/*02000009*/ left,
                                      valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2a4c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::GreaterThanOrEqualAll

  .method /*06000359*/ public hidebysig static 
          bool  GreaterThanOrEqualAny(valuetype System.Numerics.Vector4/*02000009*/ left,
                                      valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2a4f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::GreaterThanOrEqualAny

  .method /*0600035A*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Hypot(valuetype System.Numerics.Vector4/*02000009*/ x,
                valuetype System.Numerics.Vector4/*02000009*/ y) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a52
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Hypot

  .method /*0600035B*/ public hidebysig static 
          int32  IndexOf(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                         float32 'value') cil managed
  // SIG: 00 02 08 11 24 0C
  {
    // Method begins at RVA 0x2a55
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IndexOf

  .method /*0600035C*/ public hidebysig static 
          int32  IndexOfWhereAllBitsSet(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 08 11 24
  {
    // Method begins at RVA 0x2a58
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IndexOfWhereAllBitsSet

  .method /*0600035D*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsEvenInteger(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a5b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsEvenInteger

  .method /*0600035E*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsFinite(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a5e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsFinite

  .method /*0600035F*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsInfinity(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a61
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsInfinity

  .method /*06000360*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsInteger(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a64
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsInteger

  .method /*06000361*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsNaN(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a67
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsNaN

  .method /*06000362*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsNegative(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a6a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsNegative

  .method /*06000363*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsNegativeInfinity(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a6d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsNegativeInfinity

  .method /*06000364*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsNormal(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a70
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsNormal

  .method /*06000365*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsOddInteger(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a73
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsOddInteger

  .method /*06000366*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsPositive(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a76
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsPositive

  .method /*06000367*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsPositiveInfinity(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a79
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsPositiveInfinity

  .method /*06000368*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsSubnormal(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a7c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsSubnormal

  .method /*06000369*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          IsZero(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2a7f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::IsZero

  .method /*0600036A*/ public hidebysig static 
          int32  LastIndexOf(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                             float32 'value') cil managed
  // SIG: 00 02 08 11 24 0C
  {
    // Method begins at RVA 0x2a82
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LastIndexOf

  .method /*0600036B*/ public hidebysig static 
          int32  LastIndexOfWhereAllBitsSet(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 08 11 24
  {
    // Method begins at RVA 0x2a85
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LastIndexOfWhereAllBitsSet

  .method /*0600036C*/ public hidebysig instance float32 
          Length() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000256:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2a88
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Length

  .method /*0600036D*/ public hidebysig instance float32 
          LengthSquared() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000257:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2a8b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LengthSquared

  .method /*0600036E*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Lerp(valuetype System.Numerics.Vector4/*02000009*/ value1,
               valuetype System.Numerics.Vector4/*02000009*/ value2,
               float32 amount) cil managed
  // SIG: 00 03 11 24 11 24 11 24 0C
  {
    // Method begins at RVA 0x2a8e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Lerp

  .method /*0600036F*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Lerp(valuetype System.Numerics.Vector4/*02000009*/ value1,
               valuetype System.Numerics.Vector4/*02000009*/ value2,
               valuetype System.Numerics.Vector4/*02000009*/ amount) cil managed
  // SIG: 00 03 11 24 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a91
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Lerp

  .method /*06000370*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          LessThan(valuetype System.Numerics.Vector4/*02000009*/ left,
                   valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a94
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LessThan

  .method /*06000371*/ public hidebysig static 
          bool  LessThanAll(valuetype System.Numerics.Vector4/*02000009*/ left,
                            valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2a97
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LessThanAll

  .method /*06000372*/ public hidebysig static 
          bool  LessThanAny(valuetype System.Numerics.Vector4/*02000009*/ left,
                            valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2a9a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LessThanAny

  .method /*06000373*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          LessThanOrEqual(valuetype System.Numerics.Vector4/*02000009*/ left,
                          valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2a9d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LessThanOrEqual

  .method /*06000374*/ public hidebysig static 
          bool  LessThanOrEqualAll(valuetype System.Numerics.Vector4/*02000009*/ left,
                                   valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2aa0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LessThanOrEqualAll

  .method /*06000375*/ public hidebysig static 
          bool  LessThanOrEqualAny(valuetype System.Numerics.Vector4/*02000009*/ left,
                                   valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2aa3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LessThanOrEqualAny

  .method /*06000376*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Log(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2aa6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Log

  .method /*06000377*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Log2(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2aa9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Log2

  .method /*06000378*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Load(float32* source) cil managed
  // SIG: 00 01 11 24 0F 0C
  {
    .custom /*0C000258:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2aac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Load

  .method /*06000379*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          LoadAligned(float32* source) cil managed
  // SIG: 00 01 11 24 0F 0C
  {
    .custom /*0C000259:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2aaf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LoadAligned

  .method /*0600037A*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          LoadAlignedNonTemporal(float32* source) cil managed
  // SIG: 00 01 11 24 0F 0C
  {
    .custom /*0C00025A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2ab2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LoadAlignedNonTemporal

  .method /*0600037B*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          LoadUnsafe([in] float32& source) cil managed
  // SIG: 00 01 11 24 10 0C
  {
    .param [1]/*08000604*/ 
    .custom /*0C00028B:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000020*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2ab5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LoadUnsafe

  .method /*0600037C*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          LoadUnsafe([in] float32& source,
                     native uint elementOffset) cil managed
  // SIG: 00 02 11 24 10 0C 19
  {
    .custom /*0C00025B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000605*/ 
    .custom /*0C00028C:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RequiresLocationAttribute/*01000020*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2ab8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::LoadUnsafe

  .method /*0600037D*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Max(valuetype System.Numerics.Vector4/*02000009*/ value1,
              valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2abb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Max

  .method /*0600037E*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MaxMagnitude(valuetype System.Numerics.Vector4/*02000009*/ value1,
                       valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2abe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MaxMagnitude

  .method /*0600037F*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MaxMagnitudeNumber(valuetype System.Numerics.Vector4/*02000009*/ value1,
                             valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2ac1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MaxMagnitudeNumber

  .method /*06000380*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MaxNative(valuetype System.Numerics.Vector4/*02000009*/ value1,
                    valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2ac4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MaxNative

  .method /*06000381*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MaxNumber(valuetype System.Numerics.Vector4/*02000009*/ value1,
                    valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2ac7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MaxNumber

  .method /*06000382*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Min(valuetype System.Numerics.Vector4/*02000009*/ value1,
              valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2aca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Min

  .method /*06000383*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MinMagnitude(valuetype System.Numerics.Vector4/*02000009*/ value1,
                       valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2acd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MinMagnitude

  .method /*06000384*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MinMagnitudeNumber(valuetype System.Numerics.Vector4/*02000009*/ value1,
                             valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2ad0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MinMagnitudeNumber

  .method /*06000385*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MinNative(valuetype System.Numerics.Vector4/*02000009*/ value1,
                    valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2ad3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MinNative

  .method /*06000386*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MinNumber(valuetype System.Numerics.Vector4/*02000009*/ value1,
                    valuetype System.Numerics.Vector4/*02000009*/ value2) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2ad6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MinNumber

  .method /*06000387*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Multiply(valuetype System.Numerics.Vector4/*02000009*/ left,
                   valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2ad9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Multiply

  .method /*06000388*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Multiply(valuetype System.Numerics.Vector4/*02000009*/ left,
                   float32 right) cil managed
  // SIG: 00 02 11 24 11 24 0C
  {
    // Method begins at RVA 0x2adc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Multiply

  .method /*06000389*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Multiply(float32 left,
                   valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 0C 11 24
  {
    // Method begins at RVA 0x2adf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Multiply

  .method /*0600038A*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          MultiplyAddEstimate(valuetype System.Numerics.Vector4/*02000009*/ left,
                              valuetype System.Numerics.Vector4/*02000009*/ right,
                              valuetype System.Numerics.Vector4/*02000009*/ addend) cil managed
  // SIG: 00 03 11 24 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2ae2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::MultiplyAddEstimate

  .method /*0600038B*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Negate(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2ae5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Negate

  .method /*0600038C*/ public hidebysig static 
          bool  None(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                     float32 'value') cil managed
  // SIG: 00 02 02 11 24 0C
  {
    // Method begins at RVA 0x2ae8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::None

  .method /*0600038D*/ public hidebysig static 
          bool  NoneWhereAllBitsSet(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 02 11 24
  {
    // Method begins at RVA 0x2aeb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::NoneWhereAllBitsSet

  .method /*0600038E*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Normalize(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2aee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Normalize

  .method /*0600038F*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          OnesComplement(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2af1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::OnesComplement

  .method /*06000390*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_Addition(valuetype System.Numerics.Vector4/*02000009*/ left,
                      valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2af4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Addition

  .method /*06000391*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_BitwiseAnd(valuetype System.Numerics.Vector4/*02000009*/ left,
                        valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2af7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_BitwiseAnd

  .method /*06000392*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_BitwiseOr(valuetype System.Numerics.Vector4/*02000009*/ left,
                       valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2afa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_BitwiseOr

  .method /*06000393*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_Division(valuetype System.Numerics.Vector4/*02000009*/ left,
                      valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2afd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Division

  .method /*06000394*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_Division(valuetype System.Numerics.Vector4/*02000009*/ value1,
                      float32 value2) cil managed
  // SIG: 00 02 11 24 11 24 0C
  {
    // Method begins at RVA 0x2b00
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Division

  .method /*06000395*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_ExclusiveOr(valuetype System.Numerics.Vector4/*02000009*/ left,
                         valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2b03
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_ExclusiveOr

  .method /*06000396*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Numerics.Vector4/*02000009*/ left,
                            valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2b06
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Equality

  .method /*06000397*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_LeftShift(valuetype System.Numerics.Vector4/*02000009*/ 'value',
                       int32 shiftAmount) cil managed
  // SIG: 00 02 11 24 11 24 08
  {
    // Method begins at RVA 0x2b09
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_LeftShift

  .method /*06000398*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Numerics.Vector4/*02000009*/ left,
                              valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x2b0c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Inequality

  .method /*06000399*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_Multiply(valuetype System.Numerics.Vector4/*02000009*/ left,
                      valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2b0f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Multiply

  .method /*0600039A*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_Multiply(valuetype System.Numerics.Vector4/*02000009*/ left,
                      float32 right) cil managed
  // SIG: 00 02 11 24 11 24 0C
  {
    // Method begins at RVA 0x2b12
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Multiply

  .method /*0600039B*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_Multiply(float32 left,
                      valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 0C 11 24
  {
    // Method begins at RVA 0x2b15
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Multiply

  .method /*0600039C*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_OnesComplement(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2b18
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_OnesComplement

  .method /*0600039D*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_RightShift(valuetype System.Numerics.Vector4/*02000009*/ 'value',
                        int32 shiftAmount) cil managed
  // SIG: 00 02 11 24 11 24 08
  {
    // Method begins at RVA 0x2b1b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_RightShift

  .method /*0600039E*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_Subtraction(valuetype System.Numerics.Vector4/*02000009*/ left,
                         valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2b1e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_Subtraction

  .method /*0600039F*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_UnaryNegation(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2b21
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_UnaryNegation

  .method /*060003A0*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_UnaryPlus(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2b24
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_UnaryPlus

  .method /*060003A1*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          op_UnsignedRightShift(valuetype System.Numerics.Vector4/*02000009*/ 'value',
                                int32 shiftAmount) cil managed
  // SIG: 00 02 11 24 11 24 08
  {
    // Method begins at RVA 0x2b27
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::op_UnsignedRightShift

  .method /*060003A2*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          RadiansToDegrees(valuetype System.Numerics.Vector4/*02000009*/ radians) cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2b2a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::RadiansToDegrees

  .method /*060003A3*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Round(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2b2d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Round

  .method /*060003A4*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Round(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                valuetype [System.Runtime/*23000001*/]System.MidpointRounding/*01000021*/ mode) cil managed
  // SIG: 00 02 11 24 11 24 11 80 85
  {
    // Method begins at RVA 0x2b30
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Round

  .method /*060003A5*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Shuffle(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                  uint8 xIndex,
                  uint8 yIndex,
                  uint8 zIndex,
                  uint8 wIndex) cil managed
  // SIG: 00 05 11 24 11 24 05 05 05 05
  {
    // Method begins at RVA 0x2b33
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Shuffle

  .method /*060003A6*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Sin(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2b36
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Sin

  .method /*060003A7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000023*/<valuetype System.Numerics.Vector4/*02000009*/,valuetype System.Numerics.Vector4/*02000009*/> 
          SinCos(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 15 11 80 8D 02 11 24 11 24 11 24
  {
    .param [0]/*08000655*/ 
    .custom /*0C00028D:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000022*/::.ctor(string[]) /* 0A00001C */ = ( 01 00 02 00 00 00 03 53 69 6E 03 43 6F 73 00 00 ) // .......Sin.Cos..
    // Method begins at RVA 0x2b39
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::SinCos

  .method /*060003A8*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          SquareRoot(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2b3c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::SquareRoot

  .method /*060003A9*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Subtract(valuetype System.Numerics.Vector4/*02000009*/ left,
                   valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2b3f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Subtract

  .method /*060003AA*/ public hidebysig static 
          float32  Sum(valuetype System.Numerics.Vector4/*02000009*/ 'value') cil managed
  // SIG: 00 01 0C 11 24
  {
    // Method begins at RVA 0x2b42
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Sum

  .method /*060003AB*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000261:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000262:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2b45
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::ToString

  .method /*060003AC*/ public hidebysig instance string 
          ToString(string format) cil managed
  // SIG: 20 01 0E 0E
  {
    .custom /*0C000263:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000264:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800065B*/ 
    .custom /*0C00028E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .custom /*0C00028F:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000026*/::.ctor(string) /* 0A00001D */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2b48
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::ToString

  .method /*060003AD*/ public hidebysig newslot virtual final 
          instance string  ToString(string format,
                                    class [System.Runtime/*23000001*/]System.IFormatProvider/*01000027*/ formatProvider) cil managed
  // SIG: 20 02 0E 0E 12 80 9D
  {
    .custom /*0C000265:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000266:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800065C*/ 
    .custom /*0C000290:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800065D*/ 
    .custom /*0C000291:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000026*/::.ctor(string) /* 0A00001D */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2b4b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::ToString

  .method /*060003AE*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Transform(valuetype System.Numerics.Vector2/*02000007*/ position,
                    valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 02 11 24 11 1C 11 0C
  {
    // Method begins at RVA 0x2b4e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Transform

  .method /*060003AF*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Transform(valuetype System.Numerics.Vector2/*02000007*/ 'value',
                    valuetype System.Numerics.Quaternion/*02000005*/ rotation) cil managed
  // SIG: 00 02 11 24 11 1C 11 14
  {
    // Method begins at RVA 0x2b51
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Transform

  .method /*060003B0*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Transform(valuetype System.Numerics.Vector3/*02000008*/ position,
                    valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 02 11 24 11 20 11 0C
  {
    // Method begins at RVA 0x2b54
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Transform

  .method /*060003B1*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Transform(valuetype System.Numerics.Vector3/*02000008*/ 'value',
                    valuetype System.Numerics.Quaternion/*02000005*/ rotation) cil managed
  // SIG: 00 02 11 24 11 20 11 14
  {
    // Method begins at RVA 0x2b57
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Transform

  .method /*060003B2*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Transform(valuetype System.Numerics.Vector4/*02000009*/ 'vector',
                    valuetype System.Numerics.Matrix4x4/*02000003*/ matrix) cil managed
  // SIG: 00 02 11 24 11 24 11 0C
  {
    // Method begins at RVA 0x2b5a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Transform

  .method /*060003B3*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Transform(valuetype System.Numerics.Vector4/*02000009*/ 'value',
                    valuetype System.Numerics.Quaternion/*02000005*/ rotation) cil managed
  // SIG: 00 02 11 24 11 24 11 14
  {
    // Method begins at RVA 0x2b5d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Transform

  .method /*060003B4*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Truncate(valuetype System.Numerics.Vector4/*02000009*/ 'vector') cil managed
  // SIG: 00 01 11 24 11 24
  {
    // Method begins at RVA 0x2b60
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Truncate

  .method /*060003B5*/ public hidebysig static 
          valuetype System.Numerics.Vector4/*02000009*/ 
          Xor(valuetype System.Numerics.Vector4/*02000009*/ left,
              valuetype System.Numerics.Vector4/*02000009*/ right) cil managed
  // SIG: 00 02 11 24 11 24 11 24
  {
    // Method begins at RVA 0x2b63
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector4::Xor

  .property /*17000034*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          AllBitsSet()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_AllBitsSet() /* 06000319 */
  } // end of property Vector4::AllBitsSet
  .property /*17000035*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          E()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_E() /* 0600031A */
  } // end of property Vector4::E
  .property /*17000036*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          Epsilon()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_Epsilon() /* 0600031B */
  } // end of property Vector4::Epsilon
  .property /*17000037*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          NaN()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_NaN() /* 0600031C */
  } // end of property Vector4::NaN
  .property /*17000038*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          NegativeInfinity()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_NegativeInfinity() /* 0600031D */
  } // end of property Vector4::NegativeInfinity
  .property /*17000039*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          NegativeZero()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_NegativeZero() /* 0600031E */
  } // end of property Vector4::NegativeZero
  .property /*1700003A*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          One()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_One() /* 0600031F */
  } // end of property Vector4::One
  .property /*1700003B*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          Pi()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_Pi() /* 06000320 */
  } // end of property Vector4::Pi
  .property /*1700003C*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          PositiveInfinity()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_PositiveInfinity() /* 06000321 */
  } // end of property Vector4::PositiveInfinity
  .property /*1700003D*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          Tau()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_Tau() /* 06000322 */
  } // end of property Vector4::Tau
  .property /*1700003E*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          UnitW()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_UnitW() /* 06000323 */
  } // end of property Vector4::UnitW
  .property /*1700003F*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          UnitX()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_UnitX() /* 06000324 */
  } // end of property Vector4::UnitX
  .property /*17000040*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          UnitY()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_UnitY() /* 06000325 */
  } // end of property Vector4::UnitY
  .property /*17000041*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          UnitZ()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_UnitZ() /* 06000326 */
  } // end of property Vector4::UnitZ
  .property /*17000042*/ instance callconv(8) float32
          Item(int32)
  {
    .get instance float32 System.Numerics.Vector4/*02000009*/::get_Item(int32) /* 06000327 */
    .set instance void System.Numerics.Vector4/*02000009*/::set_Item(int32,
                                                                     float32) /* 06000328 */
  } // end of property Vector4::Item
  .property /*17000043*/ callconv(8) valuetype System.Numerics.Vector4/*02000009*/
          Zero()
  {
    .get valuetype System.Numerics.Vector4/*02000009*/ System.Numerics.Vector4/*02000009*/::get_Zero() /* 06000329 */
  } // end of property Vector4::Zero
} // end of class System.Numerics.Vector4

.class /*0200000A*/ public sequential ansi sealed beforefieldinit System.Numerics.Vector`1<T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Vector`1/*0200000A*/<!T>>/*1B000008*/,
                  [System.Runtime/*23000001*/]System.IFormattable/*01000024*/
{
  .custom /*0C000027:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Numerics.Vector`1/*0200000A*/<!T>>/*1B000008*/
  .custom /*0C000029:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 03 00 00 00 00 00 01 00 00 ) 
  .param type T /*2A000001*/ 
    .custom /*0C000018:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
  .field /*04000026*/ private initonly int32 _dummyPrimitive
  .method /*060003B6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<uint8> values) cil managed
  // SIG: 20 01 01 15 11 7D 01 05
  {
    // Method begins at RVA 0x2b66
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::.ctor

  .method /*060003B7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<!T> values) cil managed
  // SIG: 20 01 01 15 11 7D 01 13 00
  {
    .param [1]/*0800066F*/ 
    .custom /*0C000292:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b69
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::.ctor

  .method /*060003B8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<!T> values) cil managed
  // SIG: 20 01 01 15 11 80 95 01 13 00
  {
    .custom /*0C000267:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.OverloadResolutionPriorityAttribute/*01000028*/::.ctor(int32) /* 0A00001E */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [1]/*08000670*/ 
    .custom /*0C000293:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b6c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::.ctor

  .method /*060003B9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    .custom /*0C000268:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2b6f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::.ctor

  .method /*060003BA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T[] values) cil managed
  // SIG: 20 01 01 1D 13 00
  {
    .custom /*0C000269:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2b72
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::.ctor

  .method /*060003BB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T[] values,
                               int32 index) cil managed
  // SIG: 20 02 01 1D 13 00 08
  {
    .custom /*0C00026A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2b75
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::.ctor

  .method /*060003BC*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          get_AllBitsSet() cil managed
  // SIG: 00 00 15 11 28 01 13 00
  {
    .param [0]/*08000675*/ 
    .custom /*0C000294:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b78
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::get_AllBitsSet

  .method /*060003BD*/ public hidebysig specialname static 
          int32  get_Count() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2b7b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::get_Count

  .method /*060003BE*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          get_Indices() cil managed
  // SIG: 00 00 15 11 28 01 13 00
  {
    .param [0]/*08000676*/ 
    .custom /*0C000295:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b7e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::get_Indices

  .method /*060003BF*/ public hidebysig specialname static 
          bool  get_IsSupported() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2b81
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::get_IsSupported

  .method /*060003C0*/ public hidebysig specialname 
          instance !T  get_Item(int32 index) cil managed
  // SIG: 20 01 13 00 08
  {
    .custom /*0C00026B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2b84
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::get_Item

  .method /*060003C1*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          get_One() cil managed
  // SIG: 00 00 15 11 28 01 13 00
  {
    .param [0]/*08000678*/ 
    .custom /*0C000296:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b87
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::get_One

  .method /*060003C2*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          get_Zero() cil managed
  // SIG: 00 00 15 11 28 01 13 00
  {
    .param [0]/*08000679*/ 
    .custom /*0C000297:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b8a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::get_Zero

  .method /*060003C3*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<uint8> destination) cil managed
  // SIG: 20 01 01 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2b8d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector`1::CopyTo

  .method /*060003C4*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<!T> destination) cil managed
  // SIG: 20 01 01 15 11 80 95 01 13 00
  {
    .param [1]/*0800067B*/ 
    .custom /*0C000298:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b8f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector`1::CopyTo

  .method /*060003C5*/ public hidebysig instance void 
          CopyTo(!T[] destination) cil managed
  // SIG: 20 01 01 1D 13 00
  {
    .custom /*0C00026C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2b91
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector`1::CopyTo

  .method /*060003C6*/ public hidebysig instance void 
          CopyTo(!T[] destination,
                 int32 startIndex) cil managed
  // SIG: 20 02 01 1D 13 00 08
  {
    .custom /*0C00026D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2b93
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Vector`1::CopyTo

  .method /*060003C7*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Numerics.Vector`1/*0200000A*/<!T> other) cil managed
  // SIG: 20 01 02 15 11 28 01 13 00
  {
    .param [1]/*0800067F*/ 
    .custom /*0C000299:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b95
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::Equals

  .method /*060003C8*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00026E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000680*/ 
    .custom /*0C00029A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001C*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2b98
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::Equals

  .method /*060003C9*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2b9b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::GetHashCode

  .method /*060003CA*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_Addition(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*08000681*/ 
    .custom /*0C00029B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000682*/ 
    .custom /*0C00029C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000683*/ 
    .custom /*0C00029D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2b9e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Addition

  .method /*060003CB*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_BitwiseAnd(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                        valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*08000684*/ 
    .custom /*0C00029E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000685*/ 
    .custom /*0C00029F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000686*/ 
    .custom /*0C0002A0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2ba1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_BitwiseAnd

  .method /*060003CC*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_BitwiseOr(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                       valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*08000687*/ 
    .custom /*0C0002A1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000688*/ 
    .custom /*0C0002A2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000689*/ 
    .custom /*0C0002A3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2ba4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_BitwiseOr

  .method /*060003CD*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_Division(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*0800068A*/ 
    .custom /*0C0002A4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800068B*/ 
    .custom /*0C0002A5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800068C*/ 
    .custom /*0C0002A6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2ba7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Division

  .method /*060003CE*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_Division(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                      !T right) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 13 00
  {
    .custom /*0C00026F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800068D*/ 
    .custom /*0C0002A7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800068E*/ 
    .custom /*0C0002A8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2baa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Division

  .method /*060003CF*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                            valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 02 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [1]/*08000690*/ 
    .custom /*0C0002A9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000691*/ 
    .custom /*0C0002AA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Equality

  .method /*060003D0*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_ExclusiveOr(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                         valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*08000692*/ 
    .custom /*0C0002AB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000693*/ 
    .custom /*0C0002AC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000694*/ 
    .custom /*0C0002AD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bb0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_ExclusiveOr

  .method /*060003D1*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint8> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 05 15 11 28 01 13 00
  {
    .param [1]/*08000695*/ 
    .custom /*0C0002AE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bb3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003D2*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float64> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0D 15 11 28 01 13 00
  {
    .param [1]/*08000696*/ 
    .custom /*0C0002AF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bb6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003D3*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int16> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 06 15 11 28 01 13 00
  {
    .param [1]/*08000697*/ 
    .custom /*0C0002B0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bb9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003D4*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int32> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 08 15 11 28 01 13 00
  {
    .param [1]/*08000698*/ 
    .custom /*0C0002B1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bbc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003D5*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int64> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0A 15 11 28 01 13 00
  {
    .param [1]/*08000699*/ 
    .custom /*0C0002B2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bbf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003D6*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native int> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 18 15 11 28 01 13 00
  {
    .param [1]/*0800069A*/ 
    .custom /*0C0002B3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bc2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003D7*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<native uint> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 19 15 11 28 01 13 00
  {
    .custom /*0C000270:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800069B*/ 
    .custom /*0C0002B4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bc5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003D8*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<int8> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 04 15 11 28 01 13 00
  {
    .custom /*0C000271:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800069C*/ 
    .custom /*0C0002B5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bc8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003D9*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<float32> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0C 15 11 28 01 13 00
  {
    .param [1]/*0800069D*/ 
    .custom /*0C0002B6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bcb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003DA*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint16> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 07 15 11 28 01 13 00
  {
    .custom /*0C000272:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800069E*/ 
    .custom /*0C0002B7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003DB*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint32> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 09 15 11 28 01 13 00
  {
    .custom /*0C000273:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800069F*/ 
    .custom /*0C0002B8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bd1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003DC*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<uint64> 
          op_Explicit(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 0B 15 11 28 01 13 00
  {
    .custom /*0C000274:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080006A0*/ 
    .custom /*0C0002B9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bd4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Explicit

  .method /*060003DD*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                              valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 02 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [1]/*080006A1*/ 
    .custom /*0C0002BA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080006A2*/ 
    .custom /*0C0002BB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bd7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Inequality

  .method /*060003DE*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_LeftShift(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value',
                       int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 08
  {
    .param [0]/*080006A3*/ 
    .custom /*0C0002BC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006A4*/ 
    .custom /*0C0002BD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bda
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_LeftShift

  .method /*060003DF*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_Multiply(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*080006A6*/ 
    .custom /*0C0002BE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006A7*/ 
    .custom /*0C0002BF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080006A8*/ 
    .custom /*0C0002C0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bdd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Multiply

  .method /*060003E0*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_Multiply(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value',
                      !T factor) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 13 00
  {
    .custom /*0C000276:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [0]/*080006A9*/ 
    .custom /*0C0002C1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006AA*/ 
    .custom /*0C0002C2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2be0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Multiply

  .method /*060003E1*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_Multiply(!T factor,
                      valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 02 15 11 28 01 13 00 13 00 15 11 28 01 13 00
  {
    .custom /*0C000277:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [0]/*080006AC*/ 
    .custom /*0C0002C3:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080006AE*/ 
    .custom /*0C0002C4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2be3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Multiply

  .method /*060003E2*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_OnesComplement(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*080006AF*/ 
    .custom /*0C0002C5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006B0*/ 
    .custom /*0C0002C6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2be6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_OnesComplement

  .method /*060003E3*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_RightShift(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value',
                        int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 08
  {
    .param [0]/*080006B1*/ 
    .custom /*0C0002C7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006B2*/ 
    .custom /*0C0002C8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2be9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_RightShift

  .method /*060003E4*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_Subtraction(valuetype System.Numerics.Vector`1/*0200000A*/<!T> left,
                         valuetype System.Numerics.Vector`1/*0200000A*/<!T> right) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*080006B4*/ 
    .custom /*0C0002C9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006B5*/ 
    .custom /*0C0002CA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*080006B6*/ 
    .custom /*0C0002CB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_Subtraction

  .method /*060003E5*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_UnaryNegation(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*080006B7*/ 
    .custom /*0C0002CC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006B8*/ 
    .custom /*0C0002CD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_UnaryNegation

  .method /*060003E6*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_UnaryPlus(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value') cil managed
  // SIG: 00 01 15 11 28 01 13 00 15 11 28 01 13 00
  {
    .param [0]/*080006B9*/ 
    .custom /*0C0002CE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006BA*/ 
    .custom /*0C0002CF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bf2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_UnaryPlus

  .method /*060003E7*/ public hidebysig specialname static 
          valuetype System.Numerics.Vector`1/*0200000A*/<!T> 
          op_UnsignedRightShift(valuetype System.Numerics.Vector`1/*0200000A*/<!T> 'value',
                                int32 shiftCount) cil managed
  // SIG: 00 02 15 11 28 01 13 00 15 11 28 01 13 00 08
  {
    .param [0]/*080006BB*/ 
    .custom /*0C0002D0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080006BC*/ 
    .custom /*0C0002D1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2bf5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::op_UnsignedRightShift

  .method /*060003E8*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000278:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2bf8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::ToString

  .method /*060003E9*/ public hidebysig instance string 
          ToString(string format) cil managed
  // SIG: 20 01 0E 0E
  {
    .custom /*0C000279:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080006BE*/ 
    .custom /*0C0002D2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 02 00 00 ) 
    .custom /*0C0002D3:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000026*/::.ctor(string) /* 0A00001D */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2bfb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::ToString

  .method /*060003EA*/ public hidebysig newslot virtual final 
          instance string  ToString(string format,
                                    class [System.Runtime/*23000001*/]System.IFormatProvider/*01000027*/ formatProvider) cil managed
  // SIG: 20 02 0E 0E 12 80 9D
  {
    .custom /*0C00027A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*0100001B*/::.ctor(uint8) /* 0A000017 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080006BF*/ 
    .custom /*0C0002D4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    .param [1]/*080006C0*/ 
    .custom /*0C0002D5:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*01000026*/::.ctor(string) /* 0A00001D */ = ( 01 00 0D 4E 75 6D 65 72 69 63 46 6F 72 6D 61 74   // ...NumericFormat
                                                                                                                                                                                00 00 ) 
    // Method begins at RVA 0x2bfe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::ToString

  .method /*060003EB*/ public hidebysig instance bool 
          TryCopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<uint8> destination) cil managed
  // SIG: 20 01 02 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2c01
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::TryCopyTo

  .method /*060003EC*/ public hidebysig instance bool 
          TryCopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000025*/<!T> destination) cil managed
  // SIG: 20 01 02 15 11 80 95 01 13 00
  {
    .param [1]/*080006C3*/ 
    .custom /*0C0002D6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    // Method begins at RVA 0x2c04
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Vector`1::TryCopyTo

  .property /*17000044*/ callconv(8) valuetype System.Numerics.Vector`1/*0200000A*/<!T>
          AllBitsSet()
  {
    .custom /*0C000054:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get valuetype System.Numerics.Vector`1/*0200000A*/<!T> System.Numerics.Vector`1/*0200000A*/::get_AllBitsSet() /* 060003BC */
  } // end of property Vector`1::AllBitsSet
  .property /*17000045*/ callconv(8) int32
          Count()
  {
    .get int32 System.Numerics.Vector`1/*0200000A*/::get_Count() /* 060003BD */
  } // end of property Vector`1::Count
  .property /*17000046*/ callconv(8) valuetype System.Numerics.Vector`1/*0200000A*/<!T>
          Indices()
  {
    .custom /*0C000056:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get valuetype System.Numerics.Vector`1/*0200000A*/<!T> System.Numerics.Vector`1/*0200000A*/::get_Indices() /* 060003BE */
  } // end of property Vector`1::Indices
  .property /*17000047*/ callconv(8) bool
          IsSupported()
  {
    .get bool System.Numerics.Vector`1/*0200000A*/::get_IsSupported() /* 060003BF */
  } // end of property Vector`1::IsSupported
  .property /*17000048*/ instance callconv(8) !T
          Item(int32)
  {
    .custom /*0C000058:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    .get instance !T System.Numerics.Vector`1/*0200000A*/::get_Item(int32) /* 060003C0 */
  } // end of property Vector`1::Item
  .property /*17000049*/ callconv(8) valuetype System.Numerics.Vector`1/*0200000A*/<!T>
          One()
  {
    .custom /*0C000059:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get valuetype System.Numerics.Vector`1/*0200000A*/<!T> System.Numerics.Vector`1/*0200000A*/::get_One() /* 060003C1 */
  } // end of property Vector`1::One
  .property /*1700004A*/ callconv(8) valuetype System.Numerics.Vector`1/*0200000A*/<!T>
          Zero()
  {
    .custom /*0C00005A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*0100001E*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .get valuetype System.Numerics.Vector`1/*0200000A*/<!T> System.Numerics.Vector`1/*0200000A*/::get_Zero() /* 060003C2 */
  } // end of property Vector`1::Zero
} // end of class System.Numerics.Vector`1


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
