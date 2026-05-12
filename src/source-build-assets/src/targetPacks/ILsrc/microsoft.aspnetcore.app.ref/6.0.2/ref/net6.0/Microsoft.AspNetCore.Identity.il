


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
// Number of sections:         0x0004
// Time-date stamp:            0xcf3dc84e
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00006a00
// Size of init.data:              0x00000c00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000a842
// Base of code:                   0x00004000
// Base of data:                   0x00002000
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
// Size of headers:                0x00000400
// Checksum:                       0x0000fe12
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000a7f0 [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x000006b8] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00007a00 [0x00002470] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000a7d4 [0x0000001c] address [size] of Debug Directory:           
// 0x00000000 [0x00000000] address [size] of Architecture Specific:     
// 0x00000000 [0x00000000] address [size] of Global Pointer:            
// 0x00000000 [0x00000000] address [size] of TLS Directory:             
// 0x00000000 [0x00000000] address [size] of Load Config Directory:     
// 0x00000000 [0x00000000] address [size] of Bound Import Directory:    
// 0x00004000 [0x00000008] address [size] of Import Address Table:      
// 0x00000000 [0x00000000] address [size] of Delay Load IAT:            
// 0x00004008 [0x00000048] address [size] of CLR Header:                


// Image sections:
//              .mvid
//              0x00000010 Virtual Size
//              0x00002000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00000400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

//              .text
//              0x00006848 Virtual Size
//              0x00004000 Virtual Address
//              0x00006a00 Size of Raw Data
//              0x00000600 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x000006b8 Virtual Size
//              0x0000c000 Virtual Address
//              0x00000800 Size of Raw Data
//              0x00007000 Pointer to Raw Data
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
//              0x00007800 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000844
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x0000a832 Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 40 00 10 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00004054 [0x00006700] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000a754 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00002b00 Size
//              '#~' Name
//    Stream 2:
//              0x00002b6c Offset
//              0x00001f94 Size
//              '#Strings' Name
//    Stream 3:
//              0x00004b00 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00004b04 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00004b14 Offset
//              0x00001bec Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 40560
// PE header size       : 1024 (536 used)    ( 2.52%)
// PE additional info   : 11247             (27.73%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.18%)
// CLR meta-data size  : 26368              (65.01%)
// CLR additional info : 128                ( 0.32%)
// CLR method headers  : 224                ( 0.55%)
// Managed code         : 448               ( 1.10%)
// Data                 : 3072              ( 7.57%)
// Unaccounted          : -2023             (-4.99%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 27136
//   .rsrc    - 2048
//   .reloc   - 512

// CLR meta-data size  : 26368
//   Module        -    1 (10 bytes)
//   TypeDef       -   61 (854 bytes)     2 interfaces, 0 explicit layout
//   TypeRef       -   91 (546 bytes)
//   MethodDef     -  225 (3150 bytes)    1 abstract, 0 native, 224 bodies
//   FieldDef      -  274 (1644 bytes)    8 constant
//   MemberRef     -   22 (132 bytes)
//   ParamDef      -  224 (1344 bytes)
//   MethodImpl    -   64 (384 bytes)
//   Constant      -    7 (42 bytes)
//   CustomAttribute-  199 (1194 bytes)
//   InterfaceImpl -   39 (156 bytes)
//   PropertyMap   -   12 (48 bytes)
//   Property      -   35 (210 bytes)
//   MethodSemantic-   57 (342 bytes)
//   TypeSpec      -    4 (8 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   13 (260 bytes)
//   NestedClass   -   38 (152 bytes)
//   GenericParam  -   50 (400 bytes)
//   GenericParamConstraint-    1 (4 bytes)
//   Strings       -  8082 bytes
//   Blobs         -  7148 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   216 bytes

// CLR additional info : 128

// CLR method headers : 224
//   Num.of method bodies  - 224
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 224

// Managed code : 448
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class LoggingExtensions              (abstract) (auto) (ansi) (sealed) 
// Class IdentityServiceCollectionExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class AspNetRoleManager`1            (public) (auto) (ansi) 
// Class AspNetUserManager`1            (public) (auto) (ansi) 
// Class DataProtectionTokenProviderOptions (public) (auto) (ansi) 
// Class DataProtectorTokenProvider`1   (public) (auto) (ansi) 
// Class StreamExtensions               (abstract) (auto) (ansi) (sealed) 
// Class EventIds                       (abstract) (auto) (ansi) (sealed) 
// Class ExternalLoginInfo              (public) (auto) (ansi) 
// Class IdentityBuilderExtensions      (public) (abstract) (auto) (ansi) (sealed) 
// Class IdentityConstants              (public) (auto) (ansi) 
// Class IdentityCookiesBuilder         (public) (auto) (ansi) 
// Class IdentityCookieAuthenticationBuilderExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Interface ISecurityStampValidator        (public) (abstract) (auto) (ansi) 
// Interface ITwoFactorSecurityStampValidator (public) (abstract) (auto) (ansi) 
// Class SecurityStampRefreshingPrincipalContext (public) (auto) (ansi) 
// Class SecurityStampValidator`1       (public) (auto) (ansi) 
// Class SecurityStampValidator         (public) (abstract) (auto) (ansi) (sealed) 
// Class SecurityStampValidatorOptions  (public) (auto) (ansi) 
// Class SignInManager`1                (public) (auto) (ansi) 
// Class TwoFactorSecurityStampValidator`1 (public) (auto) (ansi) 
// Class Resources                      (abstract) (auto) (ansi) (sealed) 
// Class <>c__1`2                       (auto) (ansi) (sealed) (nested private) 
// Class <GenerateAsync>d__14           (auto) (ansi) (sealed) (nested private) 
// Class <ValidateAsync>d__15           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass3_0`1         (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <SecurityStampVerified>d__14   (auto) (ansi) (sealed) (nested private) 
// Class <ValidateAsync>d__16           (auto) (ansi) (sealed) (nested private) 
// Class TwoFactorAuthenticationInfo    (auto) (ansi) (nested assembly) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass52_0          (auto) (ansi) (sealed) (nested private) 
// Class <CanSignInAsync>d__28          (auto) (ansi) (sealed) (nested private) 
// Class <CheckPasswordSignInAsync>d__40 (auto) (ansi) (sealed) (nested private) 
// Class <CreateUserPrincipalAsync>d__26 (auto) (ansi) (sealed) (nested private) 
// Class <DoTwoFactorSignInAsync>d__45  (auto) (ansi) (sealed) (nested private) 
// Class <ExternalLoginSignInAsync>d__50 (auto) (ansi) (sealed) (nested private) 
// Class <GetExternalAuthenticationSchemesAsync>d__51 (auto) (ansi) (sealed) (nested private) 
// Class <GetExternalLoginInfoAsync>d__52 (auto) (ansi) (sealed) (nested private) 
// Class <GetTwoFactorAuthenticationUserAsync>d__48 (auto) (ansi) (sealed) (nested private) 
// Class <IsLockedOut>d__60             (auto) (ansi) (sealed) (nested private) 
// Class <IsTfaEnabled>d__57            (auto) (ansi) (sealed) (nested private) 
// Class <IsTwoFactorClientRememberedAsync>d__41 (auto) (ansi) (sealed) (nested private) 
// Class <PasswordSignInAsync>d__38     (auto) (ansi) (sealed) (nested private) 
// Class <PasswordSignInAsync>d__39     (auto) (ansi) (sealed) (nested private) 
// Class <PreSignInCheck>d__62          (auto) (ansi) (sealed) (nested private) 
// Class <RefreshSignInAsync>d__29      (auto) (ansi) (sealed) (nested private) 
// Class <RememberTwoFactorClientAsync>d__42 (auto) (ansi) (sealed) (nested private) 
// Class <RetrieveTwoFactorInfoAsync>d__59 (auto) (ansi) (sealed) (nested private) 
// Class <SignInOrTwoFactorAsync>d__58  (auto) (ansi) (sealed) (nested private) 
// Class <SignInWithClaimsAsync>d__33   (auto) (ansi) (sealed) (nested private) 
// Class <SignOutAsync>d__34            (auto) (ansi) (sealed) (nested private) 
// Class <StoreRememberClient>d__56     (auto) (ansi) (sealed) (nested private) 
// Class <TwoFactorAuthenticatorSignInAsync>d__46 (auto) (ansi) (sealed) (nested private) 
// Class <TwoFactorRecoveryCodeSignInAsync>d__44 (auto) (ansi) (sealed) (nested private) 
// Class <TwoFactorSignInAsync>d__47    (auto) (ansi) (sealed) (nested private) 
// Class <UpdateExternalAuthenticationTokensAsync>d__53 (auto) (ansi) (sealed) (nested private) 
// Class <ValidateSecurityStampAsync>d__35 (auto) (ansi) (sealed) (nested private) 
// Class <ValidateSecurityStampAsync>d__37 (auto) (ansi) (sealed) (nested private) 
// Class <ValidateTwoFactorSecurityStampAsync>d__36 (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.Extensions.Identity.Core
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.AspNetCore.Authentication.Cookies
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ Microsoft.AspNetCore.Authentication.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ Microsoft.AspNetCore.Http.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000A*/ Microsoft.AspNetCore.DataProtection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000B*/ System.Security.Claims
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000C*/ Microsoft.AspNetCore.Authentication
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000D*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Identity
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

  .custom /*0C000006:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000007:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 7D 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...}Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74   // pNetCore.Identit
                                                                                                                                                                                    79 2E 53 70 65 63 69 66 69 63 61 74 69 6F 6E 2E   // y.Specification.
                                                                                                                                                                                    54 65 73 74 73 2C 20 50 75 62 6C 69 63 4B 65 79   // Tests, PublicKey
                                                                                                                                                                                    3D 30 30 32 34 30 30 30 30 30 34 38 30 30 30 30   // =002400000480000
                                                                                                                                                                                    30 39 34 30 30 30 30 30 30 30 36 30 32 30 30 30   // 0940000000602000
                                                                                                                                                                                    30 30 30 32 34 30 30 30 30 35 32 35 33 34 31 33   // 0002400005253413
                                                                                                                                                                                    31 30 30 30 34 30 30 30 30 30 31 30 30 30 31 30   // 1000400000100010
                                                                                                                                                                                    30 66 33 33 61 32 39 30 34 34 66 61 39 64 37 34   // 0f33a29044fa9d74
                                                                                                                                                                                    30 63 39 62 33 32 31 33 61 39 33 65 35 37 63 38   // 0c9b3213a93e57c8
                                                                                                                                                                                    34 62 34 37 32 63 38 34 65 30 62 38 61 30 65 31   // 4b472c84e0b8a0e1
                                                                                                                                                                                    61 65 34 38 65 36 37 61 39 66 38 66 36 64 65 39   // ae48e67a9f8f6de9
                                                                                                                                                                                    64 35 66 37 66 33 64 35 32 61 63 32 33 65 34 38   // d5f7f3d52ac23e48
                                                                                                                                                                                    61 63 35 31 38 30 31 66 31 64 63 39 35 30 61 62   // ac51801f1dc950ab
                                                                                                                                                                                    65 39 30 31 64 61 33 34 64 32 61 39 65 33 62 61   // e901da34d2a9e3ba
                                                                                                                                                                                    61 64 62 31 34 31 61 31 37 63 37 37 65 66 33 63   // adb141a17c77ef3c
                                                                                                                                                                                    35 36 35 64 64 35 65 65 35 30 35 34 62 39 31 63   // 565dd5ee5054b91c
                                                                                                                                                                                    66 36 33 62 62 33 63 36 61 62 38 33 66 37 32 61   // f63bb3c6ab83f72a
                                                                                                                                                                                    62 33 61 61 66 65 39 33 64 30 66 63 33 63 32 33   // b3aafe93d0fc3c23
                                                                                                                                                                                    34 38 62 37 36 34 66 61 66 62 30 62 31 63 30 37   // 48b764fafb0b1c07
                                                                                                                                                                                    33 33 64 65 35 31 34 35 39 61 65 61 62 34 36 35   // 33de51459aeab465
                                                                                                                                                                                    38 30 33 38 34 62 66 39 64 37 34 63 34 65 32 38   // 80384bf9d74c4e28
                                                                                                                                                                                    31 36 34 62 37 63 64 65 32 34 37 66 38 39 31 62   // 164b7cde247f891b
                                                                                                                                                                                    61 30 37 38 39 31 63 39 64 38 37 32 61 64 32 62   // a07891c9d872ad2b
                                                                                                                                                                                    62 00 00 )                                        // b..
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 6E 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...nMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74   // pNetCore.Identit
                                                                                                                                                                                    79 2E 54 65 73 74 2C 20 50 75 62 6C 69 63 4B 65   // y.Test, PublicKe
                                                                                                                                                                                    79 3D 30 30 32 34 30 30 30 30 30 34 38 30 30 30   // y=00240000048000
                                                                                                                                                                                    30 30 39 34 30 30 30 30 30 30 30 36 30 32 30 30   // 0094000000060200
                                                                                                                                                                                    30 30 30 30 32 34 30 30 30 30 35 32 35 33 34 31   // 0000240000525341
                                                                                                                                                                                    33 31 30 30 30 34 30 30 30 30 30 31 30 30 30 31   // 3100040000010001
                                                                                                                                                                                    30 30 66 33 33 61 32 39 30 34 34 66 61 39 64 37   // 00f33a29044fa9d7
                                                                                                                                                                                    34 30 63 39 62 33 32 31 33 61 39 33 65 35 37 63   // 40c9b3213a93e57c
                                                                                                                                                                                    38 34 62 34 37 32 63 38 34 65 30 62 38 61 30 65   // 84b472c84e0b8a0e
                                                                                                                                                                                    31 61 65 34 38 65 36 37 61 39 66 38 66 36 64 65   // 1ae48e67a9f8f6de
                                                                                                                                                                                    39 64 35 66 37 66 33 64 35 32 61 63 32 33 65 34   // 9d5f7f3d52ac23e4
                                                                                                                                                                                    38 61 63 35 31 38 30 31 66 31 64 63 39 35 30 61   // 8ac51801f1dc950a
                                                                                                                                                                                    62 65 39 30 31 64 61 33 34 64 32 61 39 65 33 62   // be901da34d2a9e3b
                                                                                                                                                                                    61 61 64 62 31 34 31 61 31 37 63 37 37 65 66 33   // aadb141a17c77ef3
                                                                                                                                                                                    63 35 36 35 64 64 35 65 65 35 30 35 34 62 39 31   // c565dd5ee5054b91
                                                                                                                                                                                    63 66 36 33 62 62 33 63 36 61 62 38 33 66 37 32   // cf63bb3c6ab83f72
                                                                                                                                                                                    61 62 33 61 61 66 65 39 33 64 30 66 63 33 63 32   // ab3aafe93d0fc3c2
                                                                                                                                                                                    33 34 38 62 37 36 34 66 61 66 62 30 62 31 63 30   // 348b764fafb0b1c0
                                                                                                                                                                                    37 33 33 64 65 35 31 34 35 39 61 65 61 62 34 36   // 733de51459aeab46
                                                                                                                                                                                    35 38 30 33 38 34 62 66 39 64 37 34 63 34 65 32   // 580384bf9d74c4e2
                                                                                                                                                                                    38 31 36 34 62 37 63 64 65 32 34 37 66 38 39 31   // 8164b7cde247f891
                                                                                                                                                                                    62 61 30 37 38 39 31 63 39 64 38 37 32 61 64 32   // ba07891c9d872ad2
                                                                                                                                                                                    62 62 00 00 )                                     // bb..
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 82 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ....Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74   // pNetCore.Identit
                                                                                                                                                                                    79 2E 45 6E 74 69 74 79 46 72 61 6D 65 77 6F 72   // y.EntityFramewor
                                                                                                                                                                                    6B 43 6F 72 65 2E 54 65 73 74 2C 20 50 75 62 6C   // kCore.Test, Publ
                                                                                                                                                                                    69 63 4B 65 79 3D 30 30 32 34 30 30 30 30 30 34   // icKey=0024000004
                                                                                                                                                                                    38 30 30 30 30 30 39 34 30 30 30 30 30 30 30 36   // 8000009400000006
                                                                                                                                                                                    30 32 30 30 30 30 30 30 32 34 30 30 30 30 35 32   // 0200000024000052
                                                                                                                                                                                    35 33 34 31 33 31 30 30 30 34 30 30 30 30 30 31   // 5341310004000001
                                                                                                                                                                                    30 30 30 31 30 30 66 33 33 61 32 39 30 34 34 66   // 000100f33a29044f
                                                                                                                                                                                    61 39 64 37 34 30 63 39 62 33 32 31 33 61 39 33   // a9d740c9b3213a93
                                                                                                                                                                                    65 35 37 63 38 34 62 34 37 32 63 38 34 65 30 62   // e57c84b472c84e0b
                                                                                                                                                                                    38 61 30 65 31 61 65 34 38 65 36 37 61 39 66 38   // 8a0e1ae48e67a9f8
                                                                                                                                                                                    66 36 64 65 39 64 35 66 37 66 33 64 35 32 61 63   // f6de9d5f7f3d52ac
                                                                                                                                                                                    32 33 65 34 38 61 63 35 31 38 30 31 66 31 64 63   // 23e48ac51801f1dc
                                                                                                                                                                                    39 35 30 61 62 65 39 30 31 64 61 33 34 64 32 61   // 950abe901da34d2a
                                                                                                                                                                                    39 65 33 62 61 61 64 62 31 34 31 61 31 37 63 37   // 9e3baadb141a17c7
                                                                                                                                                                                    37 65 66 33 63 35 36 35 64 64 35 65 65 35 30 35   // 7ef3c565dd5ee505
                                                                                                                                                                                    34 62 39 31 63 66 36 33 62 62 33 63 36 61 62 38   // 4b91cf63bb3c6ab8
                                                                                                                                                                                    33 66 37 32 61 62 33 61 61 66 65 39 33 64 30 66   // 3f72ab3aafe93d0f
                                                                                                                                                                                    63 33 63 32 33 34 38 62 37 36 34 66 61 66 62 30   // c3c2348b764fafb0
                                                                                                                                                                                    62 31 63 30 37 33 33 64 65 35 31 34 35 39 61 65   // b1c0733de51459ae
                                                                                                                                                                                    61 62 34 36 35 38 30 33 38 34 62 66 39 64 37 34   // ab46580384bf9d74
                                                                                                                                                                                    63 34 65 32 38 31 36 34 62 37 63 64 65 32 34 37   // c4e28164b7cde247
                                                                                                                                                                                    66 38 39 31 62 61 30 37 38 39 31 63 39 64 38 37   // f891ba07891c9d87
                                                                                                                                                                                    32 61 64 32 62 62 00 00 )                         // 2ad2bb..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 8B 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ....Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74   // pNetCore.Identit
                                                                                                                                                                                    79 2E 45 6E 74 69 74 79 46 72 61 6D 65 77 6F 72   // y.EntityFramewor
                                                                                                                                                                                    6B 43 6F 72 65 2E 49 6E 4D 65 6D 6F 72 79 2E 54   // kCore.InMemory.T
                                                                                                                                                                                    65 73 74 2C 20 50 75 62 6C 69 63 4B 65 79 3D 30   // est, PublicKey=0
                                                                                                                                                                                    30 32 34 30 30 30 30 30 34 38 30 30 30 30 30 39   // 0240000048000009
                                                                                                                                                                                    34 30 30 30 30 30 30 30 36 30 32 30 30 30 30 30   // 4000000060200000
                                                                                                                                                                                    30 32 34 30 30 30 30 35 32 35 33 34 31 33 31 30   // 0240000525341310
                                                                                                                                                                                    30 30 34 30 30 30 30 30 31 30 30 30 31 30 30 66   // 004000001000100f
                                                                                                                                                                                    33 33 61 32 39 30 34 34 66 61 39 64 37 34 30 63   // 33a29044fa9d740c
                                                                                                                                                                                    39 62 33 32 31 33 61 39 33 65 35 37 63 38 34 62   // 9b3213a93e57c84b
                                                                                                                                                                                    34 37 32 63 38 34 65 30 62 38 61 30 65 31 61 65   // 472c84e0b8a0e1ae
                                                                                                                                                                                    34 38 65 36 37 61 39 66 38 66 36 64 65 39 64 35   // 48e67a9f8f6de9d5
                                                                                                                                                                                    66 37 66 33 64 35 32 61 63 32 33 65 34 38 61 63   // f7f3d52ac23e48ac
                                                                                                                                                                                    35 31 38 30 31 66 31 64 63 39 35 30 61 62 65 39   // 51801f1dc950abe9
                                                                                                                                                                                    30 31 64 61 33 34 64 32 61 39 65 33 62 61 61 64   // 01da34d2a9e3baad
                                                                                                                                                                                    62 31 34 31 61 31 37 63 37 37 65 66 33 63 35 36   // b141a17c77ef3c56
                                                                                                                                                                                    35 64 64 35 65 65 35 30 35 34 62 39 31 63 66 36   // 5dd5ee5054b91cf6
                                                                                                                                                                                    33 62 62 33 63 36 61 62 38 33 66 37 32 61 62 33   // 3bb3c6ab83f72ab3
                                                                                                                                                                                    61 61 66 65 39 33 64 30 66 63 33 63 32 33 34 38   // aafe93d0fc3c2348
                                                                                                                                                                                    62 37 36 34 66 61 66 62 30 62 31 63 30 37 33 33   // b764fafb0b1c0733
                                                                                                                                                                                    64 65 35 31 34 35 39 61 65 61 62 34 36 35 38 30   // de51459aeab46580
                                                                                                                                                                                    33 38 34 62 66 39 64 37 34 63 34 65 32 38 31 36   // 384bf9d74c4e2816
                                                                                                                                                                                    34 62 37 63 64 65 32 34 37 66 38 39 31 62 61 30   // 4b7cde247f891ba0
                                                                                                                                                                                    37 38 39 31 63 39 64 38 37 32 61 64 32 62 62 00   // 7891c9d872ad2bb.
                                                                                                                                                                                    00 ) 
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 77 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...wMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74   // pNetCore.Identit
                                                                                                                                                                                    79 2E 49 6E 4D 65 6D 6F 72 79 2E 54 65 73 74 2C   // y.InMemory.Test,
                                                                                                                                                                                    20 50 75 62 6C 69 63 4B 65 79 3D 30 30 32 34 30   //  PublicKey=00240
                                                                                                                                                                                    30 30 30 30 34 38 30 30 30 30 30 39 34 30 30 30   // 0000480000094000
                                                                                                                                                                                    30 30 30 30 36 30 32 30 30 30 30 30 30 32 34 30   // 0000602000000240
                                                                                                                                                                                    30 30 30 35 32 35 33 34 31 33 31 30 30 30 34 30   // 0005253413100040
                                                                                                                                                                                    30 30 30 30 31 30 30 30 31 30 30 66 33 33 61 32   // 00001000100f33a2
                                                                                                                                                                                    39 30 34 34 66 61 39 64 37 34 30 63 39 62 33 32   // 9044fa9d740c9b32
                                                                                                                                                                                    31 33 61 39 33 65 35 37 63 38 34 62 34 37 32 63   // 13a93e57c84b472c
                                                                                                                                                                                    38 34 65 30 62 38 61 30 65 31 61 65 34 38 65 36   // 84e0b8a0e1ae48e6
                                                                                                                                                                                    37 61 39 66 38 66 36 64 65 39 64 35 66 37 66 33   // 7a9f8f6de9d5f7f3
                                                                                                                                                                                    64 35 32 61 63 32 33 65 34 38 61 63 35 31 38 30   // d52ac23e48ac5180
                                                                                                                                                                                    31 66 31 64 63 39 35 30 61 62 65 39 30 31 64 61   // 1f1dc950abe901da
                                                                                                                                                                                    33 34 64 32 61 39 65 33 62 61 61 64 62 31 34 31   // 34d2a9e3baadb141
                                                                                                                                                                                    61 31 37 63 37 37 65 66 33 63 35 36 35 64 64 35   // a17c77ef3c565dd5
                                                                                                                                                                                    65 65 35 30 35 34 62 39 31 63 66 36 33 62 62 33   // ee5054b91cf63bb3
                                                                                                                                                                                    63 36 61 62 38 33 66 37 32 61 62 33 61 61 66 65   // c6ab83f72ab3aafe
                                                                                                                                                                                    39 33 64 30 66 63 33 63 32 33 34 38 62 37 36 34   // 93d0fc3c2348b764
                                                                                                                                                                                    66 61 66 62 30 62 31 63 30 37 33 33 64 65 35 31   // fafb0b1c0733de51
                                                                                                                                                                                    34 35 39 61 65 61 62 34 36 35 38 30 33 38 34 62   // 459aeab46580384b
                                                                                                                                                                                    66 39 64 37 34 63 34 65 32 38 31 36 34 62 37 63   // f9d74c4e28164b7c
                                                                                                                                                                                    64 65 32 34 37 66 38 39 31 62 61 30 37 38 39 31   // de247f891ba07891
                                                                                                                                                                                    63 39 64 38 37 32 61 64 32 62 62 00 00 )          // c9d872ad2bb..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000F:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000010:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000012:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000013:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 81 0F 41 53 50 2E 4E 45 54 20 43 6F 72 65   // ....ASP.NET Core
                                                                                                                                                                       20 49 64 65 6E 74 69 74 79 20 69 73 20 74 68 65   //  Identity is the
                                                                                                                                                                       20 6D 65 6D 62 65 72 73 68 69 70 20 73 79 73 74   //  membership syst
                                                                                                                                                                       65 6D 20 66 6F 72 20 62 75 69 6C 64 69 6E 67 20   // em for building 
                                                                                                                                                                       41 53 50 2E 4E 45 54 20 43 6F 72 65 20 77 65 62   // ASP.NET Core web
                                                                                                                                                                       20 61 70 70 6C 69 63 61 74 69 6F 6E 73 2C 20 69   //  applications, i
                                                                                                                                                                       6E 63 6C 75 64 69 6E 67 20 6D 65 6D 62 65 72 73   // ncluding members
                                                                                                                                                                       68 69 70 2C 20 6C 6F 67 69 6E 2C 20 61 6E 64 20   // hip, login, and 
                                                                                                                                                                       75 73 65 72 20 64 61 74 61 2E 20 41 53 50 2E 4E   // user data. ASP.N
                                                                                                                                                                       45 54 20 43 6F 72 65 20 49 64 65 6E 74 69 74 79   // ET Core Identity
                                                                                                                                                                       20 61 6C 6C 6F 77 73 20 79 6F 75 20 74 6F 20 61   //  allows you to a
                                                                                                                                                                       64 64 20 6C 6F 67 69 6E 20 66 65 61 74 75 72 65   // dd login feature
                                                                                                                                                                       73 20 74 6F 20 79 6F 75 72 20 61 70 70 6C 69 63   // s to your applic
                                                                                                                                                                       61 74 69 6F 6E 20 61 6E 64 20 6D 61 6B 65 73 20   // ation and makes 
                                                                                                                                                                       69 74 20 65 61 73 79 20 74 6F 20 63 75 73 74 6F   // it easy to custo
                                                                                                                                                                       6D 69 7A 65 20 64 61 74 61 20 61 62 6F 75 74 20   // mize data about 
                                                                                                                                                                       74 68 65 20 6C 6F 67 67 65 64 20 69 6E 20 75 73   // the logged in us
                                                                                                                                                                       65 72 2E 00 00 )                                  // er...
  .custom /*0C000014:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000015:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000016:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 1D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ...Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                 00 00 ) 
  .custom /*0C000018:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000019:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                F3 3A 29 04 4F A9 D7 40 C9 B3 21 3A 93 E5 7C 84   // .:).O..@..!:..|.
                B4 72 C8 4E 0B 8A 0E 1A E4 8E 67 A9 F8 F6 DE 9D   // .r.N......g.....
                5F 7F 3D 52 AC 23 E4 8A C5 18 01 F1 DC 95 0A BE   // _.=R.#..........
                90 1D A3 4D 2A 9E 3B AA DB 14 1A 17 C7 7E F3 C5   // ...M*.;......~..
                65 DD 5E E5 05 4B 91 CF 63 BB 3C 6A B8 3F 72 AB   // e.^..K..c.<j.?r.
                3A AF E9 3D 0F C3 C2 34 8B 76 4F AF B0 B1 C0 73   // :..=...4.vO....s
                3D E5 14 59 AE AB 46 58 03 84 BF 9D 74 C4 E2 81   // =..Y..FX....t...
                64 B7 CD E2 47 F8 91 BA 07 89 1C 9D 87 2A D2 BB ) // d...G........*..
  .hash algorithm 0x00008004
  .ver 6:0:0:0
}
.module Microsoft.AspNetCore.Identity.dll
// MVID: {3994b317-340d-497b-82d4-d997cf0fe5d5}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private abstract auto ansi sealed Microsoft.Extensions.Logging.LoggingExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C00001B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig static 
          void  InvalidExpirationTime(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ logger) cil managed
  // SIG: 00 01 01 12 51
  {
    .custom /*0C000001:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::InvalidExpirationTime

  .method /*06000002*/ public hidebysig static 
          void  UserIdsNotEquals(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ logger) cil managed
  // SIG: 00 01 01 12 51
  {
    .custom /*0C00001A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::UserIdsNotEquals

  .method /*06000003*/ public hidebysig static 
          void  PurposeNotEquals(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ logger,
                                 string actualPurpose,
                                 string expectedPurpose) cil managed
  // SIG: 00 03 01 12 51 0E 0E
  {
    .custom /*0C00001C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::PurposeNotEquals

  .method /*06000004*/ public hidebysig static 
          void  UnexpectedEndOfInput(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ logger) cil managed
  // SIG: 00 01 01 12 51
  {
    .custom /*0C00001E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::UnexpectedEndOfInput

  .method /*06000005*/ public hidebysig static 
          void  SecurityStampNotEquals(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ logger) cil managed
  // SIG: 00 01 01 12 51
  {
    .custom /*0C00001F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::SecurityStampNotEquals

  .method /*06000006*/ public hidebysig static 
          void  SecurityStampIsNotEmpty(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ logger) cil managed
  // SIG: 00 01 01 12 51
  {
    .custom /*0C000020:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::SecurityStampIsNotEmpty

  .method /*06000007*/ public hidebysig static 
          void  UnhandledException(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ logger) cil managed
  // SIG: 00 01 01 12 51
  {
    .custom /*0C000021:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::UnhandledException

} // end of class Microsoft.Extensions.Logging.LoggingExtensions

.class /*02000003*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.DependencyInjection.IdentityServiceCollectionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C00001D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000018*/ auto ansi serializable sealed nested private beforefieldinit '<>c__1`2'<class TUser,class TRole>
         extends [System.Runtime/*23000001*/]System.Object/*01000013*/
  {
    .custom /*0C00002F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400000E*/ public static initonly class Microsoft.Extensions.DependencyInjection.IdentityServiceCollectionExtensions/*02000003*//'<>c__1`2'/*02000018*/<!TUser,!TRole> '<>9'
    .field /*0400000F*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationOptions/*0100001B*/> '<>9__1_0'
    .field /*04000010*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> '<>9__1_1'
    .field /*04000011*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> '<>9__1_2'
    .field /*04000012*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> '<>9__1_3'
    .field /*04000013*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> '<>9__1_4'
    .method /*0600008A*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__1`2'::.ctor

    .method /*0600008B*/ assembly hidebysig 
            instance void  '<AddIdentity>b__1_0'(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationOptions/*0100001B*/ options) cil managed
    // SIG: 20 01 01 12 6D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__1`2'::'<AddIdentity>b__1_0'

    .method /*0600008C*/ assembly hidebysig 
            instance void  '<AddIdentity>b__1_1'(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/ o) cil managed
    // SIG: 20 01 01 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__1`2'::'<AddIdentity>b__1_1'

    .method /*0600008D*/ assembly hidebysig 
            instance void  '<AddIdentity>b__1_2'(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/ o) cil managed
    // SIG: 20 01 01 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__1`2'::'<AddIdentity>b__1_2'

    .method /*0600008E*/ assembly hidebysig 
            instance void  '<AddIdentity>b__1_3'(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/ o) cil managed
    // SIG: 20 01 01 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__1`2'::'<AddIdentity>b__1_3'

    .method /*0600008F*/ assembly hidebysig 
            instance void  '<AddIdentity>b__1_4'(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/ o) cil managed
    // SIG: 20 01 01 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__1`2'::'<AddIdentity>b__1_4'

  } // end of class '<>c__1`2'

  .method /*06000008*/ public hidebysig static 
          class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityBuilder/*01000015*/ 
          AddIdentity<class TUser,class TRole>(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000016*/ services) cil managed
  // SIG: 10 02 01 12 55 12 59
  {
    .custom /*0C000022:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityServiceCollectionExtensions::AddIdentity

  .method /*06000009*/ public hidebysig static 
          class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityBuilder/*01000015*/ 
          AddIdentity<class TUser,class TRole>(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000016*/ services,
                                               class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityOptions/*01000018*/> setupAction) cil managed
  // SIG: 10 02 02 12 55 12 59 15 12 5D 01 12 61
  {
    .custom /*0C000024:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityServiceCollectionExtensions::AddIdentity

  .method /*0600000A*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000016*/ 
          ConfigureApplicationCookie(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000016*/ services,
                                     class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> configure) cil managed
  // SIG: 00 02 12 59 12 59 15 12 5D 01 12 65
  {
    .custom /*0C000025:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityServiceCollectionExtensions::ConfigureApplicationCookie

  .method /*0600000B*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000016*/ 
          ConfigureExternalCookie(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000016*/ services,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> configure) cil managed
  // SIG: 00 02 12 59 12 59 15 12 5D 01 12 65
  {
    .custom /*0C000026:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityServiceCollectionExtensions::ConfigureExternalCookie

} // end of class Microsoft.Extensions.DependencyInjection.IdentityServiceCollectionExtensions

.class /*02000004*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.AspNetRoleManager`1<class TRole>
       extends class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.RoleManager`1/*0100001C*/<!TRole>/*1B000001*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001D*/
{
  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IRoleStore`1/*0100001E*/<!TRole> store,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IRoleValidator`1/*01000020*/<!TRole>> roleValidators,
                               class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.ILookupNormalizer/*01000021*/ keyNormalizer,
                               class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityErrorDescriber/*01000022*/ errors,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger`1/*01000023*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.RoleManager`1/*0100001C*/<!TRole>> logger,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000007*/]Microsoft.AspNetCore.Http.IHttpContextAccessor/*01000024*/ contextAccessor) cil managed
  // SIG: 20 06 01 15 12 79 01 13 00 15 12 7D 01 15 12 80 81 01 13 00 12 80 85 12 80 89 15 12 80 8D 01 15
  //      12 71 01 13 00 12 80 91
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AspNetRoleManager`1::.ctor

  .method /*0600000D*/ family hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ 
          get_CancellationToken() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AspNetRoleManager`1::get_CancellationToken

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/
          CancellationToken()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ Microsoft.AspNetCore.Identity.AspNetRoleManager`1/*02000004*/::get_CancellationToken() /* 0600000D */
  } // end of property AspNetRoleManager`1::CancellationToken
} // end of class Microsoft.AspNetCore.Identity.AspNetRoleManager`1

.class /*02000005*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.AspNetUserManager`1<class TUser>
       extends class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser>/*1B000002*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001D*/
{
  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserStore`1/*01000027*/<!TUser> store,
                               class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*01000028*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityOptions/*01000018*/> optionsAccessor,
                               class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IPasswordHasher`1/*01000029*/<!TUser> passwordHasher,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserValidator`1/*0100002A*/<!TUser>> userValidators,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IPasswordValidator`1/*0100002B*/<!TUser>> passwordValidators,
                               class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.ILookupNormalizer/*01000021*/ keyNormalizer,
                               class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityErrorDescriber/*01000022*/ errors,
                               class [System.ComponentModel/*23000009*/]System.IServiceProvider/*0100002C*/ services,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger`1/*01000023*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser>> logger) cil managed
  // SIG: 20 09 01 15 12 80 9D 01 13 00 15 12 80 A1 01 12 61 15 12 80 A5 01 13 00 15 12 7D 01 15 12 80 A9
  //      01 13 00 15 12 7D 01 15 12 80 AD 01 13 00 12 80 85 12 80 89 12 80 B1 15 12 80 8D 01 15 12 80 99
  //      01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AspNetUserManager`1::.ctor

  .method /*0600000F*/ family hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ 
          get_CancellationToken() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AspNetUserManager`1::get_CancellationToken

  .property /*17000002*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/
          CancellationToken()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ Microsoft.AspNetCore.Identity.AspNetUserManager`1/*02000005*/::get_CancellationToken() /* 0600000F */
  } // end of property AspNetUserManager`1::CancellationToken
} // end of class Microsoft.AspNetCore.Identity.AspNetUserManager`1

.class /*02000006*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*06000010*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000029:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionTokenProviderOptions::get_Name

  .method /*06000011*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionTokenProviderOptions::set_Name

  .method /*06000012*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ 
          get_TokenLifespan() cil managed
  // SIG: 20 00 11 80 B5
  {
    .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionTokenProviderOptions::get_TokenLifespan

  .method /*06000013*/ public hidebysig specialname 
          instance void  set_TokenLifespan(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 B5
  {
    .custom /*0C00002C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionTokenProviderOptions::set_TokenLifespan

  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionTokenProviderOptions::.ctor

  .property /*17000003*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions/*02000006*/::get_Name() /* 06000010 */
    .set instance void Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions/*02000006*/::set_Name(string) /* 06000011 */
  } // end of property DataProtectionTokenProviderOptions::Name
  .property /*17000004*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/
          TokenLifespan()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions/*02000006*/::get_TokenLifespan() /* 06000012 */
    .set instance void Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions/*02000006*/::set_TokenLifespan(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/) /* 06000013 */
  } // end of property DataProtectionTokenProviderOptions::TokenLifespan
} // end of class Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions

.class /*02000007*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1<class TUser>
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserTwoFactorTokenProvider`1/*0100002E*/<!TUser>/*1B000003*/
{
  .class /*02000019*/ auto ansi sealed nested private beforefieldinit '<GenerateAsync>d__14'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000014*/ public int32 '<>1__state'
    .field /*04000015*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<string> '<>t__builder'
    .field /*04000016*/ public !TUser user
    .field /*04000017*/ public class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> manager
    .field /*04000018*/ public string purpose
    .field /*04000019*/ public class Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/<!TUser> '<>4__this'
    .field /*0400001A*/ private class [System.Runtime/*23000001*/]System.IO.MemoryStream/*01000037*/ '<ms>5__2'
    .field /*0400001B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<string> '<>u__1'
    .field /*0400001C*/ private class [System.Runtime/*23000001*/]System.IO.BinaryWriter/*01000039*/ '<writer>5__3'
    .method /*06000090*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GenerateAsync>d__14'::MoveNext

    .method /*06000091*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000A4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GenerateAsync>d__14'::SetStateMachine

  } // end of class '<GenerateAsync>d__14'

  .class /*0200001A*/ auto ansi sealed nested private beforefieldinit '<ValidateAsync>d__15'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000033:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001D*/ public int32 '<>1__state'
    .field /*0400001E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<bool> '<>t__builder'
    .field /*0400001F*/ public class Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/<!TUser> '<>4__this'
    .field /*04000020*/ public string token
    .field /*04000021*/ public class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> manager
    .field /*04000022*/ public !TUser user
    .field /*04000023*/ public string purpose
    .field /*04000024*/ private class [System.Runtime/*23000001*/]System.IO.BinaryReader/*0100003B*/ '<reader>5__2'
    .field /*04000025*/ private string '<userId>5__3'
    .field /*04000026*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<string> '<>u__1'
    .field /*04000027*/ private string '<>7__wrap3'
    .method /*06000092*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateAsync>d__15'::MoveNext

    .method /*06000093*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000A5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateAsync>d__15'::SetStateMachine

  } // end of class '<ValidateAsync>d__15'

  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.DataProtection.Abstractions/*2300000A*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100002F*/ dataProtectionProvider,
                               class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*01000028*/<class Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions/*02000006*/> options,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger`1/*01000023*/<class Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/<!TUser>> logger) cil managed
  // SIG: 20 03 01 12 80 BD 15 12 80 A1 01 12 18 15 12 80 8D 01 15 12 1C 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectorTokenProvider`1::.ctor

  .method /*06000016*/ family hidebysig specialname 
          instance class Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions/*02000006*/ 
          get_Options() cil managed
  // SIG: 20 00 12 18
  {
    .custom /*0C00002D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectorTokenProvider`1::get_Options

  .method /*06000017*/ family hidebysig specialname 
          instance class [Microsoft.AspNetCore.DataProtection.Abstractions/*2300000A*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*01000030*/ 
          get_Protector() cil managed
  // SIG: 20 00 12 80 C1
  {
    .custom /*0C00002E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectorTokenProvider`1::get_Protector

  .method /*06000018*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectorTokenProvider`1::get_Name

  .method /*06000019*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger`1/*01000023*/<class Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/<!TUser>> 
          get_Logger() cil managed
  // SIG: 20 00 15 12 80 8D 01 15 12 1C 01 13 00
  {
    .custom /*0C000030:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectorTokenProvider`1::get_Logger

  .method /*0600001A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<string> 
          GenerateAsync(string purpose,
                        class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> manager,
                        !TUser user) cil managed
  // SIG: 20 03 15 12 80 CD 01 0E 0E 15 12 80 99 01 13 00 13 00
  {
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..OMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 44 61 74 61 50 72 6F 74 65 63 74 6F 72 54 6F   // .DataProtectorTo
                                                                                                                                                                                                                                        6B 65 6E 50 72 6F 76 69 64 65 72 60 31 2B 3C 47   // kenProvider`1+<G
                                                                                                                                                                                                                                        65 6E 65 72 61 74 65 41 73 79 6E 63 3E 64 5F 5F   // enerateAsync>d__
                                                                                                                                                                                                                                        31 34 00 00 )                                     // 14..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectorTokenProvider`1::GenerateAsync

  .method /*0600001B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<bool> 
          ValidateAsync(string purpose,
                        string token,
                        class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> manager,
                        !TUser user) cil managed
  // SIG: 20 04 15 12 80 CD 01 02 0E 0E 15 12 80 99 01 13 00 13 00
  {
    .custom /*0C000034:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..OMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 44 61 74 61 50 72 6F 74 65 63 74 6F 72 54 6F   // .DataProtectorTo
                                                                                                                                                                                                                                        6B 65 6E 50 72 6F 76 69 64 65 72 60 31 2B 3C 56   // kenProvider`1+<V
                                                                                                                                                                                                                                        61 6C 69 64 61 74 65 41 73 79 6E 63 3E 64 5F 5F   // alidateAsync>d__
                                                                                                                                                                                                                                        31 35 00 00 )                                     // 15..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectorTokenProvider`1::ValidateAsync

  .method /*0600001C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<bool> 
          CanGenerateTwoFactorTokenAsync(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> manager,
                                         !TUser user) cil managed
  // SIG: 20 02 15 12 80 CD 01 02 15 12 80 99 01 13 00 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectorTokenProvider`1::CanGenerateTwoFactorTokenAsync

  .property /*17000005*/ instance callconv(8) class Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions/*02000006*/
          Options()
  {
    .get instance class Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions/*02000006*/ Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/::get_Options() /* 06000016 */
  } // end of property DataProtectorTokenProvider`1::Options
  .property /*17000006*/ instance callconv(8) class [Microsoft.AspNetCore.DataProtection.Abstractions/*2300000A*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*01000030*/
          Protector()
  {
    .get instance class [Microsoft.AspNetCore.DataProtection.Abstractions/*2300000A*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*01000030*/ Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/::get_Protector() /* 06000017 */
  } // end of property DataProtectorTokenProvider`1::Protector
  .property /*17000007*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/::get_Name() /* 06000018 */
  } // end of property DataProtectorTokenProvider`1::Name
  .property /*17000008*/ instance callconv(8) class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger`1/*01000023*/<class Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/<!TUser>>
          Logger()
  {
    .get instance class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger`1/*01000023*/<class Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/<!TUser>> Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1/*02000007*/::get_Logger() /* 06000019 */
  } // end of property DataProtectorTokenProvider`1::Logger
} // end of class Microsoft.AspNetCore.Identity.DataProtectorTokenProvider`1

.class /*02000008*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Identity.StreamExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C000023:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ static assembly initonly class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003C*/ DefaultEncoding
  .method /*0600001D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IO.BinaryReader/*0100003B*/ 
          CreateReader(class [System.Runtime/*23000001*/]System.IO.Stream/*0100003D*/ 'stream') cil managed
  // SIG: 00 01 12 80 ED 12 80 F5
  {
    .custom /*0C000037:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamExtensions::CreateReader

  .method /*0600001E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IO.BinaryWriter/*01000039*/ 
          CreateWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*0100003D*/ 'stream') cil managed
  // SIG: 00 01 12 80 E5 12 80 F5
  {
    .custom /*0C000039:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamExtensions::CreateWriter

  .method /*0600001F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100003E*/ 
          ReadDateTimeOffset(class [System.Runtime/*23000001*/]System.IO.BinaryReader/*0100003B*/ reader) cil managed
  // SIG: 00 01 11 80 F9 12 80 ED
  {
    .custom /*0C00003B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamExtensions::ReadDateTimeOffset

  .method /*06000020*/ public hidebysig static 
          void  Write(class [System.Runtime/*23000001*/]System.IO.BinaryWriter/*01000039*/ writer,
                      valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100003E*/ 'value') cil managed
  // SIG: 00 02 01 12 80 E5 11 80 F9
  {
    .custom /*0C00003C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamExtensions::Write

} // end of class Microsoft.AspNetCore.Identity.StreamExtensions

.class /*02000009*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Identity.EventIds
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .field /*04000002*/ public static valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.EventId/*0100003F*/ UserCannotSignInWithoutConfirmedEmail
  .field /*04000003*/ public static valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.EventId/*0100003F*/ SecurityStampValidationFailed
  .field /*04000004*/ public static valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.EventId/*0100003F*/ SecurityStampValidationFailedId4
  .field /*04000005*/ public static valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.EventId/*0100003F*/ UserCannotSignInWithoutConfirmedPhoneNumber
  .field /*04000006*/ public static valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.EventId/*0100003F*/ InvalidPassword
  .field /*04000007*/ public static valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.EventId/*0100003F*/ UserLockedOut
  .field /*04000008*/ public static valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.EventId/*0100003F*/ UserCannotSignInWithoutConfirmedAccount
  .field /*04000009*/ public static valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.EventId/*0100003F*/ TwoFactorSecurityStampValidationFailed
} // end of class Microsoft.AspNetCore.Identity.EventIds

.class /*0200000A*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.ExternalLoginInfo
       extends [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserLoginInfo/*01000040*/
{
  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ principal,
                               string loginProvider,
                               string providerKey,
                               string displayName) cil managed
  // SIG: 20 04 01 12 81 05 0E 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExternalLoginInfo::.ctor

  .method /*06000022*/ public hidebysig specialname 
          instance class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ 
          get_Principal() cil managed
  // SIG: 20 00 12 81 05
  {
    .custom /*0C00003F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExternalLoginInfo::get_Principal

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_Principal(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ 'value') cil managed
  // SIG: 20 01 01 12 81 05
  {
    .custom /*0C000041:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExternalLoginInfo::set_Principal

  .method /*06000024*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationToken/*01000042*/> 
          get_AuthenticationTokens() cil managed
  // SIG: 20 00 15 12 7D 01 12 81 09
  {
    .custom /*0C000043:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExternalLoginInfo::get_AuthenticationTokens

  .method /*06000025*/ public hidebysig specialname 
          instance void  set_AuthenticationTokens(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationToken/*01000042*/> 'value') cil managed
  // SIG: 20 01 01 15 12 7D 01 12 81 09
  {
    .custom /*0C000045:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExternalLoginInfo::set_AuthenticationTokens

  .method /*06000026*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/ 
          get_AuthenticationProperties() cil managed
  // SIG: 20 00 12 81 0D
  {
    .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExternalLoginInfo::get_AuthenticationProperties

  .method /*06000027*/ public hidebysig specialname 
          instance void  set_AuthenticationProperties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/ 'value') cil managed
  // SIG: 20 01 01 12 81 0D
  {
    .custom /*0C000049:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExternalLoginInfo::set_AuthenticationProperties

  .property /*17000009*/ instance callconv(8) class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/
          Principal()
  {
    .get instance class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/::get_Principal() /* 06000022 */
    .set instance void Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/::set_Principal(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/) /* 06000023 */
  } // end of property ExternalLoginInfo::Principal
  .property /*1700000A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationToken/*01000042*/>
          AuthenticationTokens()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationToken/*01000042*/> Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/::get_AuthenticationTokens() /* 06000024 */
    .set instance void Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/::set_AuthenticationTokens(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationToken/*01000042*/>) /* 06000025 */
  } // end of property ExternalLoginInfo::AuthenticationTokens
  .property /*1700000B*/ instance callconv(8) class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/
          AuthenticationProperties()
  {
    .get instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/ Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/::get_AuthenticationProperties() /* 06000026 */
    .set instance void Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/::set_AuthenticationProperties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/) /* 06000027 */
  } // end of property ExternalLoginInfo::AuthenticationProperties
} // end of class Microsoft.AspNetCore.Identity.ExternalLoginInfo

.class /*0200000B*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Identity.IdentityBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C000027:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200001B*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass3_0`1'<class TSignInManager>
         extends [System.Runtime/*23000001*/]System.Object/*01000013*/
  {
    .custom /*0C000035:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000028*/ public class [System.Runtime/*23000001*/]System.Type/*01000031*/ managerType
    .method /*06000094*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0`1'::.ctor

    .method /*06000095*/ assembly hidebysig 
            instance object  '<AddSignInManager>b__0'(class [System.ComponentModel/*23000009*/]System.IServiceProvider/*0100002C*/ services) cil managed
    // SIG: 20 01 1C 12 80 B1
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0`1'::'<AddSignInManager>b__0'

  } // end of class '<>c__DisplayClass3_0`1'

  .method /*06000028*/ public hidebysig static 
          class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityBuilder/*01000015*/ 
          AddDefaultTokenProviders(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityBuilder/*01000015*/ builder) cil managed
  // SIG: 00 01 12 55 12 55
  {
    .custom /*0C00004B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityBuilderExtensions::AddDefaultTokenProviders

  .method /*06000029*/ public hidebysig static 
          class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityBuilder/*01000015*/ 
          AddSignInManager(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityBuilder/*01000015*/ builder) cil managed
  // SIG: 00 01 12 55 12 55
  {
    .custom /*0C00004D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityBuilderExtensions::AddSignInManager

  .method /*0600002A*/ public hidebysig static 
          class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityBuilder/*01000015*/ 
          AddSignInManager<class TSignInManager>(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityBuilder/*01000015*/ builder) cil managed
  // SIG: 10 01 01 12 55 12 55
  {
    .custom /*0C00004F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityBuilderExtensions::AddSignInManager

} // end of class Microsoft.AspNetCore.Identity.IdentityBuilderExtensions

.class /*0200000C*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.IdentityConstants
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .field /*0400000A*/ public static initonly string ApplicationScheme
  .field /*0400000B*/ public static initonly string ExternalScheme
  .field /*0400000C*/ public static initonly string TwoFactorRememberMeScheme
  .field /*0400000D*/ public static initonly string TwoFactorUserIdScheme
  .method /*0600002B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityConstants::.ctor

} // end of class Microsoft.AspNetCore.Identity.IdentityConstants

.class /*0200000D*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.IdentityCookiesBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*0600002C*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 
          get_ApplicationCookie() cil managed
  // SIG: 20 00 15 12 81 11 01 12 65
  {
    .custom /*0C000052:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::get_ApplicationCookie

  .method /*0600002D*/ public hidebysig specialname 
          instance void  set_ApplicationCookie(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 11 01 12 65
  {
    .custom /*0C000054:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::set_ApplicationCookie

  .method /*0600002E*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 
          get_ExternalCookie() cil managed
  // SIG: 20 00 15 12 81 11 01 12 65
  {
    .custom /*0C000056:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::get_ExternalCookie

  .method /*0600002F*/ public hidebysig specialname 
          instance void  set_ExternalCookie(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 11 01 12 65
  {
    .custom /*0C000058:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::set_ExternalCookie

  .method /*06000030*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 
          get_TwoFactorRememberMeCookie() cil managed
  // SIG: 20 00 15 12 81 11 01 12 65
  {
    .custom /*0C00005A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::get_TwoFactorRememberMeCookie

  .method /*06000031*/ public hidebysig specialname 
          instance void  set_TwoFactorRememberMeCookie(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 11 01 12 65
  {
    .custom /*0C00005C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::set_TwoFactorRememberMeCookie

  .method /*06000032*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 
          get_TwoFactorUserIdCookie() cil managed
  // SIG: 20 00 15 12 81 11 01 12 65
  {
    .custom /*0C00005E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::get_TwoFactorUserIdCookie

  .method /*06000033*/ public hidebysig specialname 
          instance void  set_TwoFactorUserIdCookie(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 11 01 12 65
  {
    .custom /*0C000060:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::set_TwoFactorUserIdCookie

  .method /*06000034*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookiesBuilder::.ctor

  .property /*1700000C*/ instance callconv(8) class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/>
          ApplicationCookie()
  {
    .get instance class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/::get_ApplicationCookie() /* 0600002C */
    .set instance void Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/::set_ApplicationCookie(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/>) /* 0600002D */
  } // end of property IdentityCookiesBuilder::ApplicationCookie
  .property /*1700000D*/ instance callconv(8) class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/>
          ExternalCookie()
  {
    .get instance class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/::get_ExternalCookie() /* 0600002E */
    .set instance void Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/::set_ExternalCookie(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/>) /* 0600002F */
  } // end of property IdentityCookiesBuilder::ExternalCookie
  .property /*1700000E*/ instance callconv(8) class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/>
          TwoFactorRememberMeCookie()
  {
    .get instance class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/::get_TwoFactorRememberMeCookie() /* 06000030 */
    .set instance void Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/::set_TwoFactorRememberMeCookie(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/>) /* 06000031 */
  } // end of property IdentityCookiesBuilder::TwoFactorRememberMeCookie
  .property /*1700000F*/ instance callconv(8) class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/>
          TwoFactorUserIdCookie()
  {
    .get instance class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/::get_TwoFactorUserIdCookie() /* 06000032 */
    .set instance void Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/::set_TwoFactorUserIdCookie(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/>) /* 06000033 */
  } // end of property IdentityCookiesBuilder::TwoFactorUserIdCookie
} // end of class Microsoft.AspNetCore.Identity.IdentityCookiesBuilder

.class /*0200000E*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Identity.IdentityCookieAuthenticationBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C000028:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200001C*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000013*/
  {
    .custom /*0C000036:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000029*/ public static initonly class Microsoft.AspNetCore.Identity.IdentityCookieAuthenticationBuilderExtensions/*0200000E*//'<>c'/*0200001C*/ '<>9'
    .field /*0400002A*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/> '<>9__0_0'
    .field /*0400002B*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> '<>9__2_0'
    .field /*0400002C*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> '<>9__3_0'
    .field /*0400002D*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> '<>9__4_0'
    .field /*0400002E*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> '<>9__5_0'
    .method /*06000096*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000097*/ assembly hidebysig 
            instance void  '<AddIdentityCookies>b__0_0'(class Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/ o) cil managed
    // SIG: 20 01 01 12 34
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AddIdentityCookies>b__0_0'

    .method /*06000098*/ assembly hidebysig 
            instance void  '<AddApplicationCookie>b__2_0'(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/ o) cil managed
    // SIG: 20 01 01 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AddApplicationCookie>b__2_0'

    .method /*06000099*/ assembly hidebysig 
            instance void  '<AddExternalCookie>b__3_0'(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/ o) cil managed
    // SIG: 20 01 01 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AddExternalCookie>b__3_0'

    .method /*0600009A*/ assembly hidebysig 
            instance void  '<AddTwoFactorRememberMeCookie>b__4_0'(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/ o) cil managed
    // SIG: 20 01 01 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AddTwoFactorRememberMeCookie>b__4_0'

    .method /*0600009B*/ assembly hidebysig 
            instance void  '<AddTwoFactorUserIdCookie>b__5_0'(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/ o) cil managed
    // SIG: 20 01 01 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AddTwoFactorUserIdCookie>b__5_0'

  } // end of class '<>c'

  .method /*06000035*/ public hidebysig static 
          class Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/ 
          AddIdentityCookies(class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.AuthenticationBuilder/*01000045*/ builder) cil managed
  // SIG: 00 01 12 34 12 81 15
  {
    .custom /*0C000063:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookieAuthenticationBuilderExtensions::AddIdentityCookies

  .method /*06000036*/ public hidebysig static 
          class Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/ 
          AddIdentityCookies(class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.AuthenticationBuilder/*01000045*/ builder,
                             class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<class Microsoft.AspNetCore.Identity.IdentityCookiesBuilder/*0200000D*/> configureCookies) cil managed
  // SIG: 00 02 12 34 12 81 15 15 12 5D 01 12 34
  {
    .custom /*0C000065:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookieAuthenticationBuilderExtensions::AddIdentityCookies

  .method /*06000037*/ public hidebysig static 
          class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 
          AddApplicationCookie(class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.AuthenticationBuilder/*01000045*/ builder) cil managed
  // SIG: 00 01 15 12 81 11 01 12 65 12 81 15
  {
    .custom /*0C000067:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookieAuthenticationBuilderExtensions::AddApplicationCookie

  .method /*06000038*/ public hidebysig static 
          class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 
          AddExternalCookie(class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.AuthenticationBuilder/*01000045*/ builder) cil managed
  // SIG: 00 01 15 12 81 11 01 12 65 12 81 15
  {
    .custom /*0C000069:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookieAuthenticationBuilderExtensions::AddExternalCookie

  .method /*06000039*/ public hidebysig static 
          class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 
          AddTwoFactorRememberMeCookie(class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.AuthenticationBuilder/*01000045*/ builder) cil managed
  // SIG: 00 01 15 12 81 11 01 12 65 12 81 15
  {
    .custom /*0C00006B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookieAuthenticationBuilderExtensions::AddTwoFactorRememberMeCookie

  .method /*0600003A*/ public hidebysig static 
          class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000044*/<class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions/*01000019*/> 
          AddTwoFactorUserIdCookie(class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.AuthenticationBuilder/*01000045*/ builder) cil managed
  // SIG: 00 01 15 12 81 11 01 12 65 12 81 15
  {
    .custom /*0C00006D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IdentityCookieAuthenticationBuilderExtensions::AddTwoFactorUserIdCookie

} // end of class Microsoft.AspNetCore.Identity.IdentityCookieAuthenticationBuilderExtensions

.class /*0200000F*/ interface public abstract auto ansi Microsoft.AspNetCore.Identity.ISecurityStampValidator
{
  .method /*0600003B*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          ValidateAsync(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieValidatePrincipalContext/*01000047*/ context) cil managed
  // SIG: 20 01 12 81 19 12 81 1D
  {
    // Method begins at RVA 0x0
  } // end of method ISecurityStampValidator::ValidateAsync

} // end of class Microsoft.AspNetCore.Identity.ISecurityStampValidator

.class /*02000010*/ interface public abstract auto ansi Microsoft.AspNetCore.Identity.ITwoFactorSecurityStampValidator
       implements Microsoft.AspNetCore.Identity.ISecurityStampValidator/*0200000F*/
{
} // end of class Microsoft.AspNetCore.Identity.ITwoFactorSecurityStampValidator

.class /*02000011*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*0600003C*/ public hidebysig specialname 
          instance class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ 
          get_CurrentPrincipal() cil managed
  // SIG: 20 00 12 81 05
  {
    .custom /*0C000070:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampRefreshingPrincipalContext::get_CurrentPrincipal

  .method /*0600003D*/ public hidebysig specialname 
          instance void  set_CurrentPrincipal(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ 'value') cil managed
  // SIG: 20 01 01 12 81 05
  {
    .custom /*0C000072:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampRefreshingPrincipalContext::set_CurrentPrincipal

  .method /*0600003E*/ public hidebysig specialname 
          instance class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ 
          get_NewPrincipal() cil managed
  // SIG: 20 00 12 81 05
  {
    .custom /*0C000074:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampRefreshingPrincipalContext::get_NewPrincipal

  .method /*0600003F*/ public hidebysig specialname 
          instance void  set_NewPrincipal(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ 'value') cil managed
  // SIG: 20 01 01 12 81 05
  {
    .custom /*0C000075:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampRefreshingPrincipalContext::set_NewPrincipal

  .method /*06000040*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampRefreshingPrincipalContext::.ctor

  .property /*17000010*/ instance callconv(8) class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/
          CurrentPrincipal()
  {
    .get instance class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/::get_CurrentPrincipal() /* 0600003C */
    .set instance void Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/::set_CurrentPrincipal(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/) /* 0600003D */
  } // end of property SecurityStampRefreshingPrincipalContext::CurrentPrincipal
  .property /*17000011*/ instance callconv(8) class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/
          NewPrincipal()
  {
    .get instance class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/::get_NewPrincipal() /* 0600003E */
    .set instance void Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/::set_NewPrincipal(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/) /* 0600003F */
  } // end of property SecurityStampRefreshingPrincipalContext::NewPrincipal
} // end of class Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext

.class /*02000012*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.SecurityStampValidator`1<class TUser>
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements Microsoft.AspNetCore.Identity.ISecurityStampValidator/*0200000F*/
{
  .class /*0200001D*/ auto ansi sealed nested private beforefieldinit '<SecurityStampVerified>d__14'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400002F*/ public int32 '<>1__state'
    .field /*04000030*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100004A*/ '<>t__builder'
    .field /*04000031*/ public class Microsoft.AspNetCore.Identity.SecurityStampValidator`1/*02000012*/<!TUser> '<>4__this'
    .field /*04000032*/ public !TUser user
    .field /*04000033*/ public class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieValidatePrincipalContext/*01000047*/ context
    .field /*04000034*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/> '<>u__1'
    .field /*04000035*/ private class Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/ '<replaceContext>5__2'
    .field /*04000036*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__2'
    .method /*0600009C*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SecurityStampVerified>d__14'::MoveNext

    .method /*0600009D*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000A6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SecurityStampVerified>d__14'::SetStateMachine

  } // end of class '<SecurityStampVerified>d__14'

  .class /*0200001E*/ auto ansi sealed nested private beforefieldinit '<ValidateAsync>d__16'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00003A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000037*/ public int32 '<>1__state'
    .field /*04000038*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100004A*/ '<>t__builder'
    .field /*04000039*/ public class Microsoft.AspNetCore.Identity.SecurityStampValidator`1/*02000012*/<!TUser> '<>4__this'
    .field /*0400003A*/ public class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieValidatePrincipalContext/*01000047*/ context
    .field /*0400003B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__1'
    .field /*0400003C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__2'
    .method /*0600009E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateAsync>d__16'::MoveNext

    .method /*0600009F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000A7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateAsync>d__16'::SetStateMachine

  } // end of class '<ValidateAsync>d__16'

  .method /*06000041*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*01000028*/<class Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/> options,
                               class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> signInManager,
                               class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.ISystemClock/*01000048*/ clock,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000049*/ logger) cil managed
  // SIG: 20 04 01 15 12 80 A1 01 12 50 15 12 54 01 13 00 12 81 21 12 81 25
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::.ctor

  .method /*06000042*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> 
          get_SignInManager() cil managed
  // SIG: 20 00 15 12 54 01 13 00
  {
    .custom /*0C000076:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::get_SignInManager

  .method /*06000043*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/ 
          get_Options() cil managed
  // SIG: 20 00 12 50
  {
    .custom /*0C000077:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::get_Options

  .method /*06000044*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.ISystemClock/*01000048*/ 
          get_Clock() cil managed
  // SIG: 20 00 12 81 21
  {
    .custom /*0C000078:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::get_Clock

  .method /*06000045*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ 
          get_Logger() cil managed
  // SIG: 20 00 12 51
  {
    .custom /*0C000079:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::get_Logger

  .method /*06000046*/ public hidebysig specialname 
          instance void  set_Logger(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ 'value') cil managed
  // SIG: 20 01 01 12 51
  {
    .custom /*0C00007A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::set_Logger

  .method /*06000047*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          SecurityStampVerified(!TUser user,
                                class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieValidatePrincipalContext/*01000047*/ context) cil managed
  // SIG: 20 02 12 81 19 13 00 12 81 1D
  {
    .custom /*0C00007B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 53 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..SMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 65 63 75 72 69 74 79 53 74 61 6D 70 56 61   // .SecurityStampVa
                                                                                                                                                                                                                                        6C 69 64 61 74 6F 72 60 31 2B 3C 53 65 63 75 72   // lidator`1+<Secur
                                                                                                                                                                                                                                        69 74 79 53 74 61 6D 70 56 65 72 69 66 69 65 64   // ityStampVerified
                                                                                                                                                                                                                                        3E 64 5F 5F 31 34 00 00 )                         // >d__14..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::SecurityStampVerified

  .method /*06000048*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<!TUser> 
          VerifySecurityStamp(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ principal) cil managed
  // SIG: 20 01 15 12 80 CD 01 13 00 12 81 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::VerifySecurityStamp

  .method /*06000049*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          ValidateAsync(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieValidatePrincipalContext/*01000047*/ context) cil managed
  // SIG: 20 01 12 81 19 12 81 1D
  {
    .custom /*0C00007C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..KMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 65 63 75 72 69 74 79 53 74 61 6D 70 56 61   // .SecurityStampVa
                                                                                                                                                                                                                                        6C 69 64 61 74 6F 72 60 31 2B 3C 56 61 6C 69 64   // lidator`1+<Valid
                                                                                                                                                                                                                                        61 74 65 41 73 79 6E 63 3E 64 5F 5F 31 36 00 00 ) // ateAsync>d__16..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator`1::ValidateAsync

  .property /*17000012*/ instance callconv(8) class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser>
          SignInManager()
  {
    .get instance class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> Microsoft.AspNetCore.Identity.SecurityStampValidator`1/*02000012*/::get_SignInManager() /* 06000042 */
  } // end of property SecurityStampValidator`1::SignInManager
  .property /*17000013*/ instance callconv(8) class Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/
          Options()
  {
    .get instance class Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/ Microsoft.AspNetCore.Identity.SecurityStampValidator`1/*02000012*/::get_Options() /* 06000043 */
  } // end of property SecurityStampValidator`1::Options
  .property /*17000014*/ instance callconv(8) class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.ISystemClock/*01000048*/
          Clock()
  {
    .get instance class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.ISystemClock/*01000048*/ Microsoft.AspNetCore.Identity.SecurityStampValidator`1/*02000012*/::get_Clock() /* 06000044 */
  } // end of property SecurityStampValidator`1::Clock
  .property /*17000015*/ instance callconv(8) class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/
          Logger()
  {
    .get instance class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ Microsoft.AspNetCore.Identity.SecurityStampValidator`1/*02000012*/::get_Logger() /* 06000045 */
    .set instance void Microsoft.AspNetCore.Identity.SecurityStampValidator`1/*02000012*/::set_Logger(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/) /* 06000046 */
  } // end of property SecurityStampValidator`1::Logger
} // end of class Microsoft.AspNetCore.Identity.SecurityStampValidator`1

.class /*02000013*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Identity.SecurityStampValidator
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*0600004A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          ValidatePrincipalAsync(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieValidatePrincipalContext/*01000047*/ context) cil managed
  // SIG: 00 01 12 81 19 12 81 1D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator::ValidatePrincipalAsync

  .method /*0600004B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          ValidateAsync<(Microsoft.AspNetCore.Identity.ISecurityStampValidator/*0200000F*/) TValidator>(class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieValidatePrincipalContext/*01000047*/ context) cil managed
  // SIG: 10 01 01 12 81 19 12 81 1D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidator::ValidateAsync

} // end of class Microsoft.AspNetCore.Identity.SecurityStampValidator

.class /*02000014*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*0600004C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ 
          get_ValidationInterval() cil managed
  // SIG: 20 00 11 80 B5
  {
    .custom /*0C00007D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidatorOptions::get_ValidationInterval

  .method /*0600004D*/ public hidebysig specialname 
          instance void  set_ValidationInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 B5
  {
    .custom /*0C00007E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidatorOptions::set_ValidationInterval

  .method /*0600004E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100004C*/<class Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/> 
          get_OnRefreshingPrincipal() cil managed
  // SIG: 20 00 15 12 81 31 02 12 44 12 81 19
  {
    .custom /*0C00007F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidatorOptions::get_OnRefreshingPrincipal

  .method /*0600004F*/ public hidebysig specialname 
          instance void  set_OnRefreshingPrincipal(class [System.Runtime/*23000001*/]System.Func`2/*0100004C*/<class Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 31 02 12 44 12 81 19
  {
    .custom /*0C000080:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidatorOptions::set_OnRefreshingPrincipal

  .method /*06000050*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityStampValidatorOptions::.ctor

  .property /*17000016*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/
          ValidationInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/::get_ValidationInterval() /* 0600004C */
    .set instance void Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/::set_ValidationInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/) /* 0600004D */
  } // end of property SecurityStampValidatorOptions::ValidationInterval
  .property /*17000017*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100004C*/<class Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/>
          OnRefreshingPrincipal()
  {
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100004C*/<class Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/> Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/::get_OnRefreshingPrincipal() /* 0600004E */
    .set instance void Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/::set_OnRefreshingPrincipal(class [System.Runtime/*23000001*/]System.Func`2/*0100004C*/<class Microsoft.AspNetCore.Identity.SecurityStampRefreshingPrincipalContext/*02000011*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/>) /* 0600004F */
  } // end of property SecurityStampValidatorOptions::OnRefreshingPrincipal
} // end of class Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions

.class /*02000015*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.SignInManager`1<class TUser>
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .class /*0200001F*/ auto ansi nested assembly beforefieldinit TwoFactorAuthenticationInfo<class TUser>
         extends [System.Runtime/*23000001*/]System.Object/*01000013*/
  {
    .method /*060000A0*/ public hidebysig specialname 
            instance string  get_UserId() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C0000A8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TwoFactorAuthenticationInfo::get_UserId

    .method /*060000A1*/ public hidebysig specialname 
            instance void  set_UserId(string 'value') cil managed
    // SIG: 20 01 01 0E
    {
      .custom /*0C0000A9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TwoFactorAuthenticationInfo::set_UserId

    .method /*060000A2*/ public hidebysig specialname 
            instance string  get_LoginProvider() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C0000AA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TwoFactorAuthenticationInfo::get_LoginProvider

    .method /*060000A3*/ public hidebysig specialname 
            instance void  set_LoginProvider(string 'value') cil managed
    // SIG: 20 01 01 0E
    {
      .custom /*0C0000AB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TwoFactorAuthenticationInfo::set_LoginProvider

    .method /*060000A4*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TwoFactorAuthenticationInfo::.ctor

    .property /*17000022*/ instance callconv(8) string
            UserId()
    {
      .get instance string Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/::get_UserId() /* 060000A0 */
      .set instance void Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/::set_UserId(string) /* 060000A1 */
    } // end of property TwoFactorAuthenticationInfo::UserId
    .property /*17000023*/ instance callconv(8) string
            LoginProvider()
    {
      .get instance string Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/::get_LoginProvider() /* 060000A2 */
      .set instance void Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/::set_LoginProvider(string) /* 060000A3 */
    } // end of property TwoFactorAuthenticationInfo::LoginProvider
  } // end of class TwoFactorAuthenticationInfo

  .class /*02000020*/ auto ansi serializable sealed nested private beforefieldinit '<>c'<class TUser>
         extends [System.Runtime/*23000001*/]System.Object/*01000013*/
  {
    .custom /*0C00003D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400003D*/ public static initonly class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//'<>c'/*02000020*/<!TUser> '<>9'
    .field /*0400003E*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100004C*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsIdentity/*01000055*/,bool> '<>9__27_0'
    .field /*0400003F*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100004C*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationScheme/*01000053*/,bool> '<>9__51_0'
    .method /*060000A5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*060000A6*/ assembly hidebysig 
            instance bool  '<IsSignedIn>b__27_0'(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsIdentity/*01000055*/ i) cil managed
    // SIG: 20 01 02 12 81 55
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsSignedIn>b__27_0'

    .method /*060000A7*/ assembly hidebysig 
            instance bool  '<GetExternalAuthenticationSchemesAsync>b__51_0'(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationScheme/*01000053*/ s) cil managed
    // SIG: 20 01 02 12 81 4D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetExternalAuthenticationSchemesAsync>b__51_0'

  } // end of class '<>c'

  .class /*02000021*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass52_0'<class TUser>
         extends [System.Runtime/*23000001*/]System.Object/*01000013*/
  {
    .custom /*0C00003E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000040*/ public string provider
    .method /*060000A8*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass52_0'::.ctor

    .method /*060000A9*/ assembly hidebysig 
            instance bool  '<GetExternalLoginInfoAsync>b__0'(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationScheme/*01000053*/ p) cil managed
    // SIG: 20 01 02 12 81 4D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass52_0'::'<GetExternalLoginInfoAsync>b__0'

  } // end of class '<>c__DisplayClass52_0'

  .class /*02000022*/ auto ansi sealed nested private beforefieldinit '<CanSignInAsync>d__28'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000040:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000041*/ public int32 '<>1__state'
    .field /*04000042*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<bool> '<>t__builder'
    .field /*04000043*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000044*/ public !TUser user
    .field /*04000045*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__1'
    .method /*060000AA*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CanSignInAsync>d__28'::MoveNext

    .method /*060000AB*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000AC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CanSignInAsync>d__28'::SetStateMachine

  } // end of class '<CanSignInAsync>d__28'

  .class /*02000023*/ auto ansi sealed nested private beforefieldinit '<CheckPasswordSignInAsync>d__40'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000042:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000046*/ public int32 '<>1__state'
    .field /*04000047*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*04000048*/ public !TUser user
    .field /*04000049*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*0400004A*/ public string password
    .field /*0400004B*/ public bool lockoutOnFailure
    .field /*0400004C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>u__1'
    .field /*0400004D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__2'
    .field /*0400004E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__3'
    .field /*0400004F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityResult/*01000054*/> '<>u__4'
    .method /*060000AC*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CheckPasswordSignInAsync>d__40'::MoveNext

    .method /*060000AD*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000AD:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CheckPasswordSignInAsync>d__40'::SetStateMachine

  } // end of class '<CheckPasswordSignInAsync>d__40'

  .class /*02000024*/ auto ansi sealed nested private beforefieldinit '<CreateUserPrincipalAsync>d__26'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000044:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000050*/ public int32 '<>1__state'
    .field /*04000051*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/> '<>t__builder'
    .field /*04000052*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000053*/ public !TUser user
    .field /*04000054*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/> '<>u__1'
    .method /*060000AE*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CreateUserPrincipalAsync>d__26'::MoveNext

    .method /*060000AF*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000AE:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CreateUserPrincipalAsync>d__26'::SetStateMachine

  } // end of class '<CreateUserPrincipalAsync>d__26'

  .class /*02000025*/ auto ansi sealed nested private beforefieldinit '<DoTwoFactorSignInAsync>d__45'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000046:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000055*/ public int32 '<>1__state'
    .field /*04000056*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100004A*/ '<>t__builder'
    .field /*04000057*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000058*/ public !TUser user
    .field /*04000059*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser> twoFactorInfo
    .field /*0400005A*/ public bool rememberClient
    .field /*0400005B*/ public bool isPersistent
    .field /*0400005C*/ private class [System.Collections/*2300000D*/]System.Collections.Generic.List`1/*01000056*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*01000051*/> '<claims>5__2'
    .field /*0400005D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__1'
    .method /*060000B0*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DoTwoFactorSignInAsync>d__45'::MoveNext

    .method /*060000B1*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000AF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DoTwoFactorSignInAsync>d__45'::SetStateMachine

  } // end of class '<DoTwoFactorSignInAsync>d__45'

  .class /*02000026*/ auto ansi sealed nested private beforefieldinit '<ExternalLoginSignInAsync>d__50'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000048:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400005E*/ public int32 '<>1__state'
    .field /*0400005F*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*04000060*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000061*/ public string loginProvider
    .field /*04000062*/ public string providerKey
    .field /*04000063*/ public bool isPersistent
    .field /*04000064*/ public bool bypassTwoFactor
    .field /*04000065*/ private !TUser '<user>5__2'
    .field /*04000066*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__1'
    .field /*04000067*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>u__2'
    .method /*060000B2*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExternalLoginSignInAsync>d__50'::MoveNext

    .method /*060000B3*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExternalLoginSignInAsync>d__50'::SetStateMachine

  } // end of class '<ExternalLoginSignInAsync>d__50'

  .class /*02000027*/ auto ansi sealed nested private beforefieldinit '<GetExternalAuthenticationSchemesAsync>d__51'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00004A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000068*/ public int32 '<>1__state'
    .field /*04000069*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationScheme/*01000053*/>> '<>t__builder'
    .field /*0400006A*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*0400006B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationScheme/*01000053*/>> '<>u__1'
    .method /*060000B4*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetExternalAuthenticationSchemesAsync>d__51'::MoveNext

    .method /*060000B5*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetExternalAuthenticationSchemesAsync>d__51'::SetStateMachine

  } // end of class '<GetExternalAuthenticationSchemesAsync>d__51'

  .class /*02000028*/ auto ansi sealed nested private beforefieldinit '<GetExternalLoginInfoAsync>d__52'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00004C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400006C*/ public int32 '<>1__state'
    .field /*0400006D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/> '<>t__builder'
    .field /*0400006E*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*0400006F*/ public string expectedXsrf
    .field /*04000070*/ private class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//'<>c__DisplayClass52_0'/*02000021*/<!TUser> '<>8__1'
    .field /*04000071*/ private class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticateResult/*01000057*/ '<auth>5__2'
    .field /*04000072*/ private string '<providerKey>5__3'
    .field /*04000073*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticateResult/*01000057*/> '<>u__1'
    .field /*04000074*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationScheme/*01000053*/>> '<>u__2'
    .method /*060000B6*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetExternalLoginInfoAsync>d__52'::MoveNext

    .method /*060000B7*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetExternalLoginInfoAsync>d__52'::SetStateMachine

  } // end of class '<GetExternalLoginInfoAsync>d__52'

  .class /*02000029*/ auto ansi sealed nested private beforefieldinit '<GetTwoFactorAuthenticationUserAsync>d__48'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00004E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000075*/ public int32 '<>1__state'
    .field /*04000076*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<!TUser> '<>t__builder'
    .field /*04000077*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000078*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser>> '<>u__1'
    .field /*04000079*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__2'
    .method /*060000B8*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetTwoFactorAuthenticationUserAsync>d__48'::MoveNext

    .method /*060000B9*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetTwoFactorAuthenticationUserAsync>d__48'::SetStateMachine

  } // end of class '<GetTwoFactorAuthenticationUserAsync>d__48'

  .class /*0200002A*/ auto ansi sealed nested private beforefieldinit '<IsLockedOut>d__60'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000050:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400007A*/ public int32 '<>1__state'
    .field /*0400007B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<bool> '<>t__builder'
    .field /*0400007C*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*0400007D*/ public !TUser user
    .field /*0400007E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__1'
    .method /*060000BA*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsLockedOut>d__60'::MoveNext

    .method /*060000BB*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsLockedOut>d__60'::SetStateMachine

  } // end of class '<IsLockedOut>d__60'

  .class /*0200002B*/ auto ansi sealed nested private beforefieldinit '<IsTfaEnabled>d__57'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000051:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400007F*/ public int32 '<>1__state'
    .field /*04000080*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<bool> '<>t__builder'
    .field /*04000081*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000082*/ public !TUser user
    .field /*04000083*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__1'
    .field /*04000084*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000058*/<string>> '<>u__2'
    .method /*060000BC*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsTfaEnabled>d__57'::MoveNext

    .method /*060000BD*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsTfaEnabled>d__57'::SetStateMachine

  } // end of class '<IsTfaEnabled>d__57'

  .class /*0200002C*/ auto ansi sealed nested private beforefieldinit '<IsTwoFactorClientRememberedAsync>d__41'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000053:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000085*/ public int32 '<>1__state'
    .field /*04000086*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<bool> '<>t__builder'
    .field /*04000087*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000088*/ public !TUser user
    .field /*04000089*/ private string '<userId>5__2'
    .field /*0400008A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<string> '<>u__1'
    .field /*0400008B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticateResult/*01000057*/> '<>u__2'
    .method /*060000BE*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsTwoFactorClientRememberedAsync>d__41'::MoveNext

    .method /*060000BF*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsTwoFactorClientRememberedAsync>d__41'::SetStateMachine

  } // end of class '<IsTwoFactorClientRememberedAsync>d__41'

  .class /*0200002D*/ auto ansi sealed nested private beforefieldinit '<PasswordSignInAsync>d__38'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000055:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400008C*/ public int32 '<>1__state'
    .field /*0400008D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*0400008E*/ public !TUser user
    .field /*0400008F*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000090*/ public string password
    .field /*04000091*/ public bool lockoutOnFailure
    .field /*04000092*/ public bool isPersistent
    .field /*04000093*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>u__1'
    .method /*060000C0*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<PasswordSignInAsync>d__38'::MoveNext

    .method /*060000C1*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<PasswordSignInAsync>d__38'::SetStateMachine

  } // end of class '<PasswordSignInAsync>d__38'

  .class /*0200002E*/ auto ansi sealed nested private beforefieldinit '<PasswordSignInAsync>d__39'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000057:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000094*/ public int32 '<>1__state'
    .field /*04000095*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*04000096*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000097*/ public string userName
    .field /*04000098*/ public string password
    .field /*04000099*/ public bool isPersistent
    .field /*0400009A*/ public bool lockoutOnFailure
    .field /*0400009B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__1'
    .field /*0400009C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>u__2'
    .method /*060000C2*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<PasswordSignInAsync>d__39'::MoveNext

    .method /*060000C3*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B8:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<PasswordSignInAsync>d__39'::SetStateMachine

  } // end of class '<PasswordSignInAsync>d__39'

  .class /*0200002F*/ auto ansi sealed nested private beforefieldinit '<PreSignInCheck>d__62'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000059:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400009D*/ public int32 '<>1__state'
    .field /*0400009E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*0400009F*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000A0*/ public !TUser user
    .field /*040000A1*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__1'
    .field /*040000A2*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>u__2'
    .method /*060000C4*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<PreSignInCheck>d__62'::MoveNext

    .method /*060000C5*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000B9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<PreSignInCheck>d__62'::SetStateMachine

  } // end of class '<PreSignInCheck>d__62'

  .class /*02000030*/ auto ansi sealed nested private beforefieldinit '<RefreshSignInAsync>d__29'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00005B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000A3*/ public int32 '<>1__state'
    .field /*040000A4*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100004A*/ '<>t__builder'
    .field /*040000A5*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000A6*/ public !TUser user
    .field /*040000A7*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticateResult/*01000057*/> '<>u__1'
    .field /*040000A8*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__2'
    .method /*060000C6*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RefreshSignInAsync>d__29'::MoveNext

    .method /*060000C7*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000BA:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RefreshSignInAsync>d__29'::SetStateMachine

  } // end of class '<RefreshSignInAsync>d__29'

  .class /*02000031*/ auto ansi sealed nested private beforefieldinit '<RememberTwoFactorClientAsync>d__42'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00005D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000A9*/ public int32 '<>1__state'
    .field /*040000AA*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100004A*/ '<>t__builder'
    .field /*040000AB*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000AC*/ public !TUser user
    .field /*040000AD*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/> '<>u__1'
    .field /*040000AE*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__2'
    .method /*060000C8*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RememberTwoFactorClientAsync>d__42'::MoveNext

    .method /*060000C9*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000BB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RememberTwoFactorClientAsync>d__42'::SetStateMachine

  } // end of class '<RememberTwoFactorClientAsync>d__42'

  .class /*02000032*/ auto ansi sealed nested private beforefieldinit '<RetrieveTwoFactorInfoAsync>d__59'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00005F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000AF*/ public int32 '<>1__state'
    .field /*040000B0*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser>> '<>t__builder'
    .field /*040000B1*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000B2*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticateResult/*01000057*/> '<>u__1'
    .method /*060000CA*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RetrieveTwoFactorInfoAsync>d__59'::MoveNext

    .method /*060000CB*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000BC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RetrieveTwoFactorInfoAsync>d__59'::SetStateMachine

  } // end of class '<RetrieveTwoFactorInfoAsync>d__59'

  .class /*02000033*/ auto ansi sealed nested private beforefieldinit '<SignInOrTwoFactorAsync>d__58'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000061:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000B3*/ public int32 '<>1__state'
    .field /*040000B4*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*040000B5*/ public bool bypassTwoFactor
    .field /*040000B6*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000B7*/ public !TUser user
    .field /*040000B8*/ public string loginProvider
    .field /*040000B9*/ public bool isPersistent
    .field /*040000BA*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__1'
    .field /*040000BB*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<string> '<>u__2'
    .field /*040000BC*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__3'
    .method /*060000CC*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SignInOrTwoFactorAsync>d__58'::MoveNext

    .method /*060000CD*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000BD:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SignInOrTwoFactorAsync>d__58'::SetStateMachine

  } // end of class '<SignInOrTwoFactorAsync>d__58'

  .class /*02000034*/ auto ansi sealed nested private beforefieldinit '<SignInWithClaimsAsync>d__33'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000062:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000BD*/ public int32 '<>1__state'
    .field /*040000BE*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100004A*/ '<>t__builder'
    .field /*040000BF*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000C0*/ public !TUser user
    .field /*040000C1*/ public class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*01000051*/> additionalClaims
    .field /*040000C2*/ public class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/ authenticationProperties
    .field /*040000C3*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/> '<>u__1'
    .field /*040000C4*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__2'
    .method /*060000CE*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SignInWithClaimsAsync>d__33'::MoveNext

    .method /*060000CF*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000BE:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SignInWithClaimsAsync>d__33'::SetStateMachine

  } // end of class '<SignInWithClaimsAsync>d__33'

  .class /*02000035*/ auto ansi sealed nested private beforefieldinit '<SignOutAsync>d__34'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000064:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000C5*/ public int32 '<>1__state'
    .field /*040000C6*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100004A*/ '<>t__builder'
    .field /*040000C7*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000C8*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__1'
    .method /*060000D0*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SignOutAsync>d__34'::MoveNext

    .method /*060000D1*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000BF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SignOutAsync>d__34'::SetStateMachine

  } // end of class '<SignOutAsync>d__34'

  .class /*02000036*/ auto ansi sealed nested private beforefieldinit '<StoreRememberClient>d__56'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000066:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000C9*/ public int32 '<>1__state'
    .field /*040000CA*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/> '<>t__builder'
    .field /*040000CB*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000CC*/ public !TUser user
    .field /*040000CD*/ private class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsIdentity/*01000055*/ '<rememberBrowserIdentity>5__2'
    .field /*040000CE*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<string> '<>u__1'
    .method /*060000D2*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StoreRememberClient>d__56'::MoveNext

    .method /*060000D3*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000C0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StoreRememberClient>d__56'::SetStateMachine

  } // end of class '<StoreRememberClient>d__56'

  .class /*02000037*/ auto ansi sealed nested private beforefieldinit '<TwoFactorAuthenticatorSignInAsync>d__46'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000068:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000CF*/ public int32 '<>1__state'
    .field /*040000D0*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*040000D1*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000D2*/ public string code
    .field /*040000D3*/ public bool isPersistent
    .field /*040000D4*/ public bool rememberClient
    .field /*040000D5*/ private class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser> '<twoFactorInfo>5__2'
    .field /*040000D6*/ private !TUser '<user>5__3'
    .field /*040000D7*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser>> '<>u__1'
    .field /*040000D8*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__2'
    .field /*040000D9*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>u__3'
    .field /*040000DA*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__4'
    .field /*040000DB*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__5'
    .field /*040000DC*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityResult/*01000054*/> '<>u__6'
    .method /*060000D4*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<TwoFactorAuthenticatorSignInAsync>d__46'::MoveNext

    .method /*060000D5*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000C1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<TwoFactorAuthenticatorSignInAsync>d__46'::SetStateMachine

  } // end of class '<TwoFactorAuthenticatorSignInAsync>d__46'

  .class /*02000038*/ auto ansi sealed nested private beforefieldinit '<TwoFactorRecoveryCodeSignInAsync>d__44'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00006A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000DD*/ public int32 '<>1__state'
    .field /*040000DE*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*040000DF*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000E0*/ public string recoveryCode
    .field /*040000E1*/ private class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser> '<twoFactorInfo>5__2'
    .field /*040000E2*/ private !TUser '<user>5__3'
    .field /*040000E3*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser>> '<>u__1'
    .field /*040000E4*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__2'
    .field /*040000E5*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityResult/*01000054*/> '<>u__3'
    .field /*040000E6*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__4'
    .method /*060000D6*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<TwoFactorRecoveryCodeSignInAsync>d__44'::MoveNext

    .method /*060000D7*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000C2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<TwoFactorRecoveryCodeSignInAsync>d__44'::SetStateMachine

  } // end of class '<TwoFactorRecoveryCodeSignInAsync>d__44'

  .class /*02000039*/ auto ansi sealed nested private beforefieldinit '<TwoFactorSignInAsync>d__47'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00006C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000E7*/ public int32 '<>1__state'
    .field /*040000E8*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>t__builder'
    .field /*040000E9*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000EA*/ public string provider
    .field /*040000EB*/ public string code
    .field /*040000EC*/ public bool isPersistent
    .field /*040000ED*/ public bool rememberClient
    .field /*040000EE*/ private class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser> '<twoFactorInfo>5__2'
    .field /*040000EF*/ private !TUser '<user>5__3'
    .field /*040000F0*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*//TwoFactorAuthenticationInfo/*0200001F*/<!TUser>> '<>u__1'
    .field /*040000F1*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__2'
    .field /*040000F2*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> '<>u__3'
    .field /*040000F3*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__4'
    .field /*040000F4*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004B*/ '<>u__5'
    .field /*040000F5*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityResult/*01000054*/> '<>u__6'
    .method /*060000D8*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<TwoFactorSignInAsync>d__47'::MoveNext

    .method /*060000D9*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000C3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<TwoFactorSignInAsync>d__47'::SetStateMachine

  } // end of class '<TwoFactorSignInAsync>d__47'

  .class /*0200003A*/ auto ansi sealed nested private beforefieldinit '<UpdateExternalAuthenticationTokensAsync>d__53'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00006E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000F6*/ public int32 '<>1__state'
    .field /*040000F7*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityResult/*01000054*/> '<>t__builder'
    .field /*040000F8*/ public class Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/ externalLogin
    .field /*040000F9*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*040000FA*/ private !TUser '<user>5__2'
    .field /*040000FB*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__1'
    .field /*040000FC*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000059*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationToken/*01000042*/> '<>7__wrap2'
    .field /*040000FD*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityResult/*01000054*/> '<>u__2'
    .method /*060000DA*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<UpdateExternalAuthenticationTokensAsync>d__53'::MoveNext

    .method /*060000DB*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000C4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<UpdateExternalAuthenticationTokensAsync>d__53'::SetStateMachine

  } // end of class '<UpdateExternalAuthenticationTokensAsync>d__53'

  .class /*0200003B*/ auto ansi sealed nested private beforefieldinit '<ValidateSecurityStampAsync>d__35'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C00006F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000FE*/ public int32 '<>1__state'
    .field /*040000FF*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<!TUser> '<>t__builder'
    .field /*04000100*/ public class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ principal
    .field /*04000101*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000102*/ private !TUser '<user>5__2'
    .field /*04000103*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__1'
    .field /*04000104*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__2'
    .method /*060000DC*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateSecurityStampAsync>d__35'::MoveNext

    .method /*060000DD*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000C5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateSecurityStampAsync>d__35'::SetStateMachine

  } // end of class '<ValidateSecurityStampAsync>d__35'

  .class /*0200003C*/ auto ansi sealed nested private beforefieldinit '<ValidateSecurityStampAsync>d__37'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000071:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000105*/ public int32 '<>1__state'
    .field /*04000106*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<bool> '<>t__builder'
    .field /*04000107*/ public !TUser user
    .field /*04000108*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000109*/ public string securityStamp
    .field /*0400010A*/ private string '<>7__wrap1'
    .field /*0400010B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<string> '<>u__1'
    .method /*060000DE*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateSecurityStampAsync>d__37'::MoveNext

    .method /*060000DF*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000C6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateSecurityStampAsync>d__37'::SetStateMachine

  } // end of class '<ValidateSecurityStampAsync>d__37'

  .class /*0200003D*/ auto ansi sealed nested private beforefieldinit '<ValidateTwoFactorSecurityStampAsync>d__36'<class TUser>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000034*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
  {
    .custom /*0C000073:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400010C*/ public int32 '<>1__state'
    .field /*0400010D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<!TUser> '<>t__builder'
    .field /*0400010E*/ public class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ principal
    .field /*0400010F*/ public class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> '<>4__this'
    .field /*04000110*/ private !TUser '<user>5__2'
    .field /*04000111*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<!TUser> '<>u__1'
    .field /*04000112*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000038*/<bool> '<>u__2'
    .method /*060000E0*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A000014*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateTwoFactorSecurityStampAsync>d__36'::MoveNext

    .method /*060000E1*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 D5
    {
      .custom /*0C0000C7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100003A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A000015*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateTwoFactorSecurityStampAsync>d__36'::SetStateMachine

  } // end of class '<ValidateTwoFactorSecurityStampAsync>d__36'

  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> userManager,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000007*/]Microsoft.AspNetCore.Http.IHttpContextAccessor/*01000024*/ contextAccessor,
                               class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory`1/*0100004D*/<!TUser> claimsFactory,
                               class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*01000028*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityOptions/*01000018*/> optionsAccessor,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger`1/*01000023*/<class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser>> logger,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider/*0100004E*/ schemes,
                               class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserConfirmation`1/*0100004F*/<!TUser> confirmation) cil managed
  // SIG: 20 07 01 15 12 80 99 01 13 00 12 80 91 15 12 81 35 01 13 00 15 12 80 A1 01 12 61 15 12 80 8D 01
  //      15 12 54 01 13 00 12 81 39 15 12 81 3D 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::.ctor

  .method /*06000052*/ public hidebysig newslot specialname virtual 
          instance class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ 
          get_Logger() cil managed
  // SIG: 20 00 12 51
  {
    .custom /*0C000081:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::get_Logger

  .method /*06000053*/ public hidebysig newslot specialname virtual 
          instance void  set_Logger(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ 'value') cil managed
  // SIG: 20 01 01 12 51
  {
    .custom /*0C000082:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::set_Logger

  .method /*06000054*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> 
          get_UserManager() cil managed
  // SIG: 20 00 15 12 80 99 01 13 00
  {
    .custom /*0C000083:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::get_UserManager

  .method /*06000055*/ public hidebysig specialname 
          instance void  set_UserManager(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> 'value') cil managed
  // SIG: 20 01 01 15 12 80 99 01 13 00
  {
    .custom /*0C000084:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::set_UserManager

  .method /*06000056*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory`1/*0100004D*/<!TUser> 
          get_ClaimsFactory() cil managed
  // SIG: 20 00 15 12 81 35 01 13 00
  {
    .custom /*0C000085:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::get_ClaimsFactory

  .method /*06000057*/ public hidebysig specialname 
          instance void  set_ClaimsFactory(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory`1/*0100004D*/<!TUser> 'value') cil managed
  // SIG: 20 01 01 15 12 81 35 01 13 00
  {
    .custom /*0C000086:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::set_ClaimsFactory

  .method /*06000058*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityOptions/*01000018*/ 
          get_Options() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000087:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::get_Options

  .method /*06000059*/ public hidebysig specialname 
          instance void  set_Options(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityOptions/*01000018*/ 'value') cil managed
  // SIG: 20 01 01 12 61
  {
    .custom /*0C000088:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::set_Options

  .method /*0600005A*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000007*/]Microsoft.AspNetCore.Http.HttpContext/*01000050*/ 
          get_Context() cil managed
  // SIG: 20 00 12 81 41
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::get_Context

  .method /*0600005B*/ public hidebysig specialname 
          instance void  set_Context(class [Microsoft.AspNetCore.Http.Abstractions/*23000007*/]Microsoft.AspNetCore.Http.HttpContext/*01000050*/ 'value') cil managed
  // SIG: 20 01 01 12 81 41
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::set_Context

  .method /*0600005C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/> 
          CreateUserPrincipalAsync(!TUser user) cil managed
  // SIG: 20 01 15 12 80 CD 01 12 81 05 13 00
  {
    .custom /*0C000089:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..MMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 43 72 65 61 74 65 55 73 65 72 50 72 69 6E   // +<CreateUserPrin
                                                                                                                                                                                                                                        63 69 70 61 6C 41 73 79 6E 63 3E 64 5F 5F 32 36   // cipalAsync>d__26
                                                                                                                                                                                                                                        00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::CreateUserPrincipalAsync

  .method /*0600005D*/ public hidebysig newslot virtual 
          instance bool  IsSignedIn(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ principal) cil managed
  // SIG: 20 01 02 12 81 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::IsSignedIn

  .method /*0600005E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<bool> 
          CanSignInAsync(!TUser user) cil managed
  // SIG: 20 01 15 12 80 CD 01 02 13 00
  {
    .custom /*0C00008A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 43 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..CMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 43 61 6E 53 69 67 6E 49 6E 41 73 79 6E 63   // +<CanSignInAsync
                                                                                                                                                                                                                                        3E 64 5F 5F 32 38 00 00 )                         // >d__28..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::CanSignInAsync

  .method /*0600005F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          RefreshSignInAsync(!TUser user) cil managed
  // SIG: 20 01 12 81 19 13 00
  {
    .custom /*0C00008B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 47 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..GMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 52 65 66 72 65 73 68 53 69 67 6E 49 6E 41   // +<RefreshSignInA
                                                                                                                                                                                                                                        73 79 6E 63 3E 64 5F 5F 32 39 00 00 )             // sync>d__29..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::RefreshSignInAsync

  .method /*06000060*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          SignInAsync(!TUser user,
                      bool isPersistent,
                      [opt] string authenticationMethod) cil managed
  // SIG: 20 03 12 81 19 13 00 02 0E
  {
    .param [3]/*0800006A*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::SignInAsync

  .method /*06000061*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          SignInAsync(!TUser user,
                      class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/ authenticationProperties,
                      [opt] string authenticationMethod) cil managed
  // SIG: 20 03 12 81 19 13 00 12 81 0D 0E
  {
    .param [3]/*0800006D*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::SignInAsync

  .method /*06000062*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          SignInWithClaimsAsync(!TUser user,
                                bool isPersistent,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*01000051*/> additionalClaims) cil managed
  // SIG: 20 03 12 81 19 13 00 02 15 12 7D 01 12 81 45
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::SignInWithClaimsAsync

  .method /*06000063*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          SignInWithClaimsAsync(!TUser user,
                                class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/ authenticationProperties,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*01000051*/> additionalClaims) cil managed
  // SIG: 20 03 12 81 19 13 00 12 81 0D 15 12 7D 01 12 81 45
  {
    .custom /*0C00008C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..JMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 53 69 67 6E 49 6E 57 69 74 68 43 6C 61 69   // +<SignInWithClai
                                                                                                                                                                                                                                        6D 73 41 73 79 6E 63 3E 64 5F 5F 33 33 00 00 )    // msAsync>d__33..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::SignInWithClaimsAsync

  .method /*06000064*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          SignOutAsync() cil managed
  // SIG: 20 00 12 81 19
  {
    .custom /*0C00008D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 41 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..AMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 53 69 67 6E 4F 75 74 41 73 79 6E 63 3E 64   // +<SignOutAsync>d
                                                                                                                                                                                                                                        5F 5F 33 34 00 00 )                               // __34..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::SignOutAsync

  .method /*06000065*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<!TUser> 
          ValidateSecurityStampAsync(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ principal) cil managed
  // SIG: 20 01 15 12 80 CD 01 13 00 12 81 05
  {
    .custom /*0C00008E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..OMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 56 61 6C 69 64 61 74 65 53 65 63 75 72 69   // +<ValidateSecuri
                                                                                                                                                                                                                                        74 79 53 74 61 6D 70 41 73 79 6E 63 3E 64 5F 5F   // tyStampAsync>d__
                                                                                                                                                                                                                                        33 35 00 00 )                                     // 35..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::ValidateSecurityStampAsync

  .method /*06000066*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<!TUser> 
          ValidateTwoFactorSecurityStampAsync(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ principal) cil managed
  // SIG: 20 01 15 12 80 CD 01 13 00 12 81 05
  {
    .custom /*0C00008F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 58 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..XMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 56 61 6C 69 64 61 74 65 54 77 6F 46 61 63   // +<ValidateTwoFac
                                                                                                                                                                                                                                        74 6F 72 53 65 63 75 72 69 74 79 53 74 61 6D 70   // torSecurityStamp
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 33 36 00 00 )          // Async>d__36..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::ValidateTwoFactorSecurityStampAsync

  .method /*06000067*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<bool> 
          ValidateSecurityStampAsync(!TUser user,
                                     string securityStamp) cil managed
  // SIG: 20 02 15 12 80 CD 01 02 13 00 0E
  {
    .custom /*0C000090:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..OMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 56 61 6C 69 64 61 74 65 53 65 63 75 72 69   // +<ValidateSecuri
                                                                                                                                                                                                                                        74 79 53 74 61 6D 70 41 73 79 6E 63 3E 64 5F 5F   // tyStampAsync>d__
                                                                                                                                                                                                                                        33 37 00 00 )                                     // 37..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::ValidateSecurityStampAsync

  .method /*06000068*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          PasswordSignInAsync(!TUser user,
                              string password,
                              bool isPersistent,
                              bool lockoutOnFailure) cil managed
  // SIG: 20 04 15 12 80 CD 01 12 81 49 13 00 0E 02 02
  {
    .custom /*0C000091:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 48 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..HMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 50 61 73 73 77 6F 72 64 53 69 67 6E 49 6E   // +<PasswordSignIn
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 33 38 00 00 )          // Async>d__38..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::PasswordSignInAsync

  .method /*06000069*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          PasswordSignInAsync(string userName,
                              string password,
                              bool isPersistent,
                              bool lockoutOnFailure) cil managed
  // SIG: 20 04 15 12 80 CD 01 12 81 49 0E 0E 02 02
  {
    .custom /*0C000092:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 48 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..HMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 50 61 73 73 77 6F 72 64 53 69 67 6E 49 6E   // +<PasswordSignIn
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 33 39 00 00 )          // Async>d__39..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::PasswordSignInAsync

  .method /*0600006A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          CheckPasswordSignInAsync(!TUser user,
                                   string password,
                                   bool lockoutOnFailure) cil managed
  // SIG: 20 03 15 12 80 CD 01 12 81 49 13 00 0E 02
  {
    .custom /*0C000093:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..MMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 43 68 65 63 6B 50 61 73 73 77 6F 72 64 53   // +<CheckPasswordS
                                                                                                                                                                                                                                        69 67 6E 49 6E 41 73 79 6E 63 3E 64 5F 5F 34 30   // ignInAsync>d__40
                                                                                                                                                                                                                                        00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::CheckPasswordSignInAsync

  .method /*0600006B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<bool> 
          IsTwoFactorClientRememberedAsync(!TUser user) cil managed
  // SIG: 20 01 15 12 80 CD 01 02 13 00
  {
    .custom /*0C000094:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 55 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..UMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 49 73 54 77 6F 46 61 63 74 6F 72 43 6C 69   // +<IsTwoFactorCli
                                                                                                                                                                                                                                        65 6E 74 52 65 6D 65 6D 62 65 72 65 64 41 73 79   // entRememberedAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 34 31 00 00 )                   // nc>d__41..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::IsTwoFactorClientRememberedAsync

  .method /*0600006C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          RememberTwoFactorClientAsync(!TUser user) cil managed
  // SIG: 20 01 12 81 19 13 00
  {
    .custom /*0C000095:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 51 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..QMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 52 65 6D 65 6D 62 65 72 54 77 6F 46 61 63   // +<RememberTwoFac
                                                                                                                                                                                                                                        74 6F 72 43 6C 69 65 6E 74 41 73 79 6E 63 3E 64   // torClientAsync>d
                                                                                                                                                                                                                                        5F 5F 34 32 00 00 )                               // __42..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::RememberTwoFactorClientAsync

  .method /*0600006D*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          ForgetTwoFactorClientAsync() cil managed
  // SIG: 20 00 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::ForgetTwoFactorClientAsync

  .method /*0600006E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          TwoFactorRecoveryCodeSignInAsync(string recoveryCode) cil managed
  // SIG: 20 01 15 12 80 CD 01 12 81 49 0E
  {
    .custom /*0C000096:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 55 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..UMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 54 77 6F 46 61 63 74 6F 72 52 65 63 6F 76   // +<TwoFactorRecov
                                                                                                                                                                                                                                        65 72 79 43 6F 64 65 53 69 67 6E 49 6E 41 73 79   // eryCodeSignInAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 34 34 00 00 )                   // nc>d__44..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::TwoFactorRecoveryCodeSignInAsync

  .method /*0600006F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          TwoFactorAuthenticatorSignInAsync(string code,
                                            bool isPersistent,
                                            bool rememberClient) cil managed
  // SIG: 20 03 15 12 80 CD 01 12 81 49 0E 02 02
  {
    .custom /*0C000097:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 56 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..VMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 54 77 6F 46 61 63 74 6F 72 41 75 74 68 65   // +<TwoFactorAuthe
                                                                                                                                                                                                                                        6E 74 69 63 61 74 6F 72 53 69 67 6E 49 6E 41 73   // nticatorSignInAs
                                                                                                                                                                                                                                        79 6E 63 3E 64 5F 5F 34 36 00 00 )                // ync>d__46..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::TwoFactorAuthenticatorSignInAsync

  .method /*06000070*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          TwoFactorSignInAsync(string provider,
                               string code,
                               bool isPersistent,
                               bool rememberClient) cil managed
  // SIG: 20 04 15 12 80 CD 01 12 81 49 0E 0E 02 02
  {
    .custom /*0C000098:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 49 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..IMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 54 77 6F 46 61 63 74 6F 72 53 69 67 6E 49   // +<TwoFactorSignI
                                                                                                                                                                                                                                        6E 41 73 79 6E 63 3E 64 5F 5F 34 37 00 00 )       // nAsync>d__47..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::TwoFactorSignInAsync

  .method /*06000071*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<!TUser> 
          GetTwoFactorAuthenticationUserAsync() cil managed
  // SIG: 20 00 15 12 80 CD 01 13 00
  {
    .custom /*0C000099:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 58 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..XMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 47 65 74 54 77 6F 46 61 63 74 6F 72 41 75   // +<GetTwoFactorAu
                                                                                                                                                                                                                                        74 68 65 6E 74 69 63 61 74 69 6F 6E 55 73 65 72   // thenticationUser
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 34 38 00 00 )          // Async>d__48..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::GetTwoFactorAuthenticationUserAsync

  .method /*06000072*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          ExternalLoginSignInAsync(string loginProvider,
                                   string providerKey,
                                   bool isPersistent) cil managed
  // SIG: 20 03 15 12 80 CD 01 12 81 49 0E 0E 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::ExternalLoginSignInAsync

  .method /*06000073*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          ExternalLoginSignInAsync(string loginProvider,
                                   string providerKey,
                                   bool isPersistent,
                                   bool bypassTwoFactor) cil managed
  // SIG: 20 04 15 12 80 CD 01 12 81 49 0E 0E 02 02
  {
    .custom /*0C00009A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..MMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 45 78 74 65 72 6E 61 6C 4C 6F 67 69 6E 53   // +<ExternalLoginS
                                                                                                                                                                                                                                        69 67 6E 49 6E 41 73 79 6E 63 3E 64 5F 5F 35 30   // ignInAsync>d__50
                                                                                                                                                                                                                                        00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::ExternalLoginSignInAsync

  .method /*06000074*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationScheme/*01000053*/>> 
          GetExternalAuthenticationSchemesAsync() cil managed
  // SIG: 20 00 15 12 80 CD 01 15 12 7D 01 12 81 4D
  {
    .custom /*0C00009B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 5A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..ZMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 47 65 74 45 78 74 65 72 6E 61 6C 41 75 74   // +<GetExternalAut
                                                                                                                                                                                                                                        68 65 6E 74 69 63 61 74 69 6F 6E 53 63 68 65 6D   // henticationSchem
                                                                                                                                                                                                                                        65 73 41 73 79 6E 63 3E 64 5F 5F 35 31 00 00 )    // esAsync>d__51..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::GetExternalAuthenticationSchemesAsync

  .method /*06000075*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/> 
          GetExternalLoginInfoAsync([opt] string expectedXsrf) cil managed
  // SIG: 20 01 15 12 80 CD 01 12 28 0E
  {
    .custom /*0C00009C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 47 65 74 45 78 74 65 72 6E 61 6C 4C 6F 67   // +<GetExternalLog
                                                                                                                                                                                                                                        69 6E 49 6E 66 6F 41 73 79 6E 63 3E 64 5F 5F 35   // inInfoAsync>d__5
                                                                                                                                                                                                                                        32 00 00 )                                        // 2..
    .param [1]/*08000094*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::GetExternalLoginInfoAsync

  .method /*06000076*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityResult/*01000054*/> 
          UpdateExternalAuthenticationTokensAsync(class Microsoft.AspNetCore.Identity.ExternalLoginInfo/*0200000A*/ externalLogin) cil managed
  // SIG: 20 01 15 12 80 CD 01 12 81 51 12 28
  {
    .custom /*0C00009D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 5C 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..\Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 55 70 64 61 74 65 45 78 74 65 72 6E 61 6C   // +<UpdateExternal
                                                                                                                                                                                                                                        41 75 74 68 65 6E 74 69 63 61 74 69 6F 6E 54 6F   // AuthenticationTo
                                                                                                                                                                                                                                        6B 65 6E 73 41 73 79 6E 63 3E 64 5F 5F 35 33 00   // kensAsync>d__53.
                                                                                                                                                                                                                                        00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::UpdateExternalAuthenticationTokensAsync

  .method /*06000077*/ public hidebysig newslot virtual 
          instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000043*/ 
          ConfigureExternalAuthenticationProperties(string provider,
                                                    string redirectUrl,
                                                    [opt] string userId) cil managed
  // SIG: 20 03 12 81 0D 0E 0E 0E
  {
    .param [3]/*08000098*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::ConfigureExternalAuthenticationProperties

  .method /*06000078*/ assembly hidebysig 
          instance class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ 
          StoreTwoFactorInfo(string userId,
                             string loginProvider) cil managed
  // SIG: 20 02 12 81 05 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::StoreTwoFactorInfo

  .method /*06000079*/ assembly hidebysig 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/> 
          StoreRememberClient(!TUser user) cil managed
  // SIG: 20 01 15 12 80 CD 01 12 81 05 13 00
  {
    .custom /*0C00009E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 48 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..HMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 53 74 6F 72 65 52 65 6D 65 6D 62 65 72 43   // +<StoreRememberC
                                                                                                                                                                                                                                        6C 69 65 6E 74 3E 64 5F 5F 35 36 00 00 )          // lient>d__56..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::StoreRememberClient

  .method /*0600007A*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          SignInOrTwoFactorAsync(!TUser user,
                                 bool isPersistent,
                                 [opt] string loginProvider,
                                 [opt] bool bypassTwoFactor) cil managed
  // SIG: 20 04 15 12 80 CD 01 12 81 49 13 00 02 0E 02
  {
    .custom /*0C00009F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 4B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..KMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 53 69 67 6E 49 6E 4F 72 54 77 6F 46 61 63   // +<SignInOrTwoFac
                                                                                                                                                                                                                                        74 6F 72 41 73 79 6E 63 3E 64 5F 5F 35 38 00 00 ) // torAsync>d__58..
    .param [3]/*0800009E*/  = nullref
    .param [4]/*0800009F*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::SignInOrTwoFactorAsync

  .method /*0600007B*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<bool> 
          IsLockedOut(!TUser user) cil managed
  // SIG: 20 01 15 12 80 CD 01 02 13 00
  {
    .custom /*0C0000A0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 40 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..@Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 49 73 4C 6F 63 6B 65 64 4F 75 74 3E 64 5F   // +<IsLockedOut>d_
                                                                                                                                                                                                                                        5F 36 30 00 00 )                                  // _60..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::IsLockedOut

  .method /*0600007C*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          LockedOut(!TUser user) cil managed
  // SIG: 20 01 15 12 80 CD 01 12 81 49 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::LockedOut

  .method /*0600007D*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.SignInResult/*01000052*/> 
          PreSignInCheck(!TUser user) cil managed
  // SIG: 20 01 15 12 80 CD 01 12 81 49 13 00
  {
    .custom /*0C0000A1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A000013 */ = ( 01 00 43 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..CMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 64 65 6E 74 69 74 79   // NetCore.Identity
                                                                                                                                                                                                                                        2E 53 69 67 6E 49 6E 4D 61 6E 61 67 65 72 60 31   // .SignInManager`1
                                                                                                                                                                                                                                        2B 3C 50 72 65 53 69 67 6E 49 6E 43 68 65 63 6B   // +<PreSignInCheck
                                                                                                                                                                                                                                        3E 64 5F 5F 36 32 00 00 )                         // >d__62..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::PreSignInCheck

  .method /*0600007E*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          ResetLockout(!TUser user) cil managed
  // SIG: 20 01 12 81 19 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInManager`1::ResetLockout

  .property /*17000018*/ instance callconv(8) class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/
          Logger()
  {
    .get instance class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/ Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::get_Logger() /* 06000052 */
    .set instance void Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::set_Logger(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000014*/) /* 06000053 */
  } // end of property SignInManager`1::Logger
  .property /*17000019*/ instance callconv(8) class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser>
          UserManager()
  {
    .get instance class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser> Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::get_UserManager() /* 06000054 */
    .set instance void Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::set_UserManager(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.UserManager`1/*01000026*/<!TUser>) /* 06000055 */
  } // end of property SignInManager`1::UserManager
  .property /*1700001A*/ instance callconv(8) class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory`1/*0100004D*/<!TUser>
          ClaimsFactory()
  {
    .get instance class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory`1/*0100004D*/<!TUser> Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::get_ClaimsFactory() /* 06000056 */
    .set instance void Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::set_ClaimsFactory(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory`1/*0100004D*/<!TUser>) /* 06000057 */
  } // end of property SignInManager`1::ClaimsFactory
  .property /*1700001B*/ instance callconv(8) class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityOptions/*01000018*/
          Options()
  {
    .get instance class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityOptions/*01000018*/ Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::get_Options() /* 06000058 */
    .set instance void Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::set_Options(class [Microsoft.Extensions.Identity.Core/*23000003*/]Microsoft.AspNetCore.Identity.IdentityOptions/*01000018*/) /* 06000059 */
  } // end of property SignInManager`1::Options
  .property /*1700001C*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000007*/]Microsoft.AspNetCore.Http.HttpContext/*01000050*/
          Context()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000007*/]Microsoft.AspNetCore.Http.HttpContext/*01000050*/ Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::get_Context() /* 0600005A */
    .set instance void Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/::set_Context(class [Microsoft.AspNetCore.Http.Abstractions/*23000007*/]Microsoft.AspNetCore.Http.HttpContext/*01000050*/) /* 0600005B */
  } // end of property SignInManager`1::Context
} // end of class Microsoft.AspNetCore.Identity.SignInManager`1

.class /*02000016*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Identity.TwoFactorSecurityStampValidator`1<class TUser>
       extends class Microsoft.AspNetCore.Identity.SecurityStampValidator`1/*02000012*/<!TUser>/*1B000004*/
       implements Microsoft.AspNetCore.Identity.ITwoFactorSecurityStampValidator/*02000010*/,
                  Microsoft.AspNetCore.Identity.ISecurityStampValidator/*0200000F*/
{
  .method /*0600007F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*01000028*/<class Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions/*02000014*/> options,
                               class Microsoft.AspNetCore.Identity.SignInManager`1/*02000015*/<!TUser> signInManager,
                               class [Microsoft.AspNetCore.Authentication/*2300000C*/]Microsoft.AspNetCore.Authentication.ISystemClock/*01000048*/ clock,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000049*/ logger) cil managed
  // SIG: 20 04 01 15 12 80 A1 01 12 50 15 12 54 01 13 00 12 81 21 12 81 25
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TwoFactorSecurityStampValidator`1::.ctor

  .method /*06000080*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<!TUser> 
          VerifySecurityStamp(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000041*/ principal) cil managed
  // SIG: 20 01 15 12 80 CD 01 13 00 12 81 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TwoFactorSecurityStampValidator`1::VerifySecurityStamp

  .method /*06000081*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000046*/ 
          SecurityStampVerified(!TUser user,
                                class [Microsoft.AspNetCore.Authentication.Cookies/*23000005*/]Microsoft.AspNetCore.Authentication.Cookies.CookieValidatePrincipalContext/*01000047*/ context) cil managed
  // SIG: 20 02 12 81 19 13 00 12 81 1D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TwoFactorSecurityStampValidator`1::SecurityStampVerified

} // end of class Microsoft.AspNetCore.Identity.TwoFactorSecurityStampValidator`1

.class /*02000017*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Identity.Resources
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*06000082*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100005A*/ 
          get_ResourceManager() cil managed
  // SIG: 00 00 12 81 69
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_ResourceManager

  .method /*06000083*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100005B*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 81 6D
  {
    .custom /*0C0000A2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Culture

  .method /*06000084*/ assembly hidebysig specialname static 
          void  set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100005B*/ 'value') cil managed
  // SIG: 00 01 01 12 81 6D
  {
    .custom /*0C0000A3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::set_Culture

  .method /*06000085*/ assembly hidebysig static 
          string  GetResourceString(string resourceKey,
                                    [opt] string defaultValue) cil managed aggressiveinlining
  // SIG: 00 02 0E 0E 0E
  {
    .param [2]/*080000AD*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::GetResourceString

  .method /*06000086*/ assembly hidebysig specialname static 
          string  get_InvalidManagerType() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_InvalidManagerType

  .method /*06000087*/ assembly hidebysig static 
          string  FormatInvalidManagerType(object p0,
                                           object p1,
                                           object p2) cil managed
  // SIG: 00 03 0E 1C 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatInvalidManagerType

  .method /*06000088*/ assembly hidebysig specialname static 
          string  get_InvalidPasswordHasherCompatibilityMode() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_InvalidPasswordHasherCompatibilityMode

  .method /*06000089*/ assembly hidebysig specialname static 
          string  get_InvalidPasswordHasherIterationCount() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_InvalidPasswordHasherIterationCount

  .property /*1700001D*/ callconv(8) class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100005A*/
          ResourceManager()
  {
    .get class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100005A*/ Microsoft.AspNetCore.Identity.Resources/*02000017*/::get_ResourceManager() /* 06000082 */
  } // end of property Resources::ResourceManager
  .property /*1700001E*/ callconv(8) class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100005B*/
          Culture()
  {
    .get class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100005B*/ Microsoft.AspNetCore.Identity.Resources/*02000017*/::get_Culture() /* 06000083 */
    .set void Microsoft.AspNetCore.Identity.Resources/*02000017*/::set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100005B*/) /* 06000084 */
  } // end of property Resources::Culture
  .property /*1700001F*/ callconv(8) string
          InvalidManagerType()
  {
    .get string Microsoft.AspNetCore.Identity.Resources/*02000017*/::get_InvalidManagerType() /* 06000086 */
  } // end of property Resources::InvalidManagerType
  .property /*17000020*/ callconv(8) string
          InvalidPasswordHasherCompatibilityMode()
  {
    .get string Microsoft.AspNetCore.Identity.Resources/*02000017*/::get_InvalidPasswordHasherCompatibilityMode() /* 06000088 */
  } // end of property Resources::InvalidPasswordHasherCompatibilityMode
  .property /*17000021*/ callconv(8) string
          InvalidPasswordHasherIterationCount()
  {
    .get string Microsoft.AspNetCore.Identity.Resources/*02000017*/::get_InvalidPasswordHasherIterationCount() /* 06000089 */
  } // end of property Resources::InvalidPasswordHasherIterationCount
} // end of class Microsoft.AspNetCore.Identity.Resources


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
