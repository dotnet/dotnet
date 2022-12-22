
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
// Time-date stamp:            0x563c1c32
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000e000
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000fe02
// Base of code:                   0x00002000
// Base of data:                   0x00010000
// Image base:                     0x53100000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0006
// Minor subsystem version:        0x0000
// Size of image:                  0x00014000
// Size of headers:                0x00000200
// Checksum:                       0x00015d77
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000fdb0 [0x0000004f] address [size] of Import Directory:          
// 0x00010000 [0x0000049c] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000ea00 [0x00003f98] address [size] of Security Directory:        
// 0x00012000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00000000 [0x00000000] address [size] of Debug Directory:           
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
//              0x0000de08 Virtual Size
//              0x00002000 Virtual Address
//              0x0000e000 Size of Raw Data
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
//              0x0000049c Virtual Size
//              0x00010000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000e200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00012000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0000e800 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000f000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000e04
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000fdf2 Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 10 53 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x000022e8 [0x0000a278] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x0000c560 [0x00003750] address [size] of Resources Directory:       
// 0x0000fcb0 [0x00000100] address [size] of Strong Name Signature:     
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
//              0x00004b1c Size
//              '#~' Name
//    Stream 2:
//              0x00004b88 Offset
//              0x00003aac Size
//              '#Strings' Name
//    Stream 3:
//              0x00008634 Offset
//              0x00000008 Size
//              '#US' Name
//    Stream 4:
//              0x0000863c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000864c Offset
//              0x00001c2c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000017090ba05f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 76184
// PE header size       : 512 (496 used)    ( 0.67%)
// PE additional info   : 17631             (23.14%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.09%)
// CLR meta-data size  : 41592              (54.59%)
// CLR additional info : 14416              (18.92%)
// CLR method headers  : 658                ( 0.86%)
// Managed code         : 0                 ( 0.00%)
// Data                 : 2048              ( 2.69%)
// Unaccounted          : -745              (-0.98%)

// Num.of PE sections   : 3
//   .text    - 57344
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 41592
//   Module        -    1 (10 bytes)
//   TypeDef       -   52 (728 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -  138 (828 bytes)
//   MethodDef     -  626 (8764 bytes)    12 abstract, 0 native, 614 bodies
//   FieldDef      -   65 (390 bytes)     6 constant
//   MemberRef     -   44 (264 bytes)
//   ParamDef      -  641 (3846 bytes)
//   MethodImpl    -   13 (78 bytes)
//   Constant      -   55 (330 bytes)
//   CustomAttribute-  142 (852 bytes)
//   InterfaceImpl -   23 (92 bytes)
//   PropertyMap   -   28 (112 bytes)
//   Property      -  163 (978 bytes)
//   MethodSemantic-  227 (1362 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -   10 (20 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   10 (200 bytes)
//   ManifestResource-    2 (24 bytes)
//   NestedClass   -    3 (12 bytes)
//   GenericParam  -   20 (160 bytes)
//   GenericParamConstraint-    9 (36 bytes)
//   Strings       - 15020 bytes
//   Blobs         -  7212 bytes
//   UserStrings   -     8 bytes
//   Guids         -    16 bytes
//   Uncategorized -   222 bytes

// CLR additional info : 14416
//   Resources         - 14160

// CLR method headers : 658
//   Num.of method bodies  - 614
//   Num.of fat headers    - 4
//   Num.of tiny headers   - 610

// Managed code : 0
//   Ave method size - 0
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class UdpBinding                     (public) (auto) (ansi) 
// Class UdpBindingCollectionElement    (public) (auto) (ansi) 
// Class UdpBindingElement              (public) (auto) (ansi) 
// Class ByteStreamMessageEncodingElement (public) (auto) (ansi) (sealed) 
// Class UdpTransportElement            (public) (auto) (ansi) (sealed) 
// Class UdpRetransmissionSettingsElement (public) (auto) (ansi) (sealed) 
// Class UdpTransportConfigurationStrings (abstract) (auto) (ansi) (sealed) 
// Class ByteStreamBufferedMessageData  (auto) (ansi) 
// Class ByteStreamMessage              (public) (abstract) (auto) (ansi) (sealed) 
// Class ByteStreamMessageEncoder       (auto) (ansi) 
// Class ByteStreamMessageEncoderFactory (auto) (ansi) 
// Class ByteStreamMessageEncodingBindingElement (public) (auto) (ansi) (sealed) 
// Class ByteStreamMessageUtility       (abstract) (auto) (ansi) (sealed) 
// Class ClientUdpOutputChannel         (auto) (ansi) 
// Class DuplicateMessageDetector       (auto) (ansi) (sealed) 
// Interface IUdpReceiveHandler             (abstract) (auto) (ansi) 
// Class ServerUdpOutputChannel         (auto) (ansi) 
// Class SynchronizedRandom             (auto) (ansi) 
// Class UdpChannelBase`1               (abstract) (auto) (ansi) 
// Class UdpChannelFactory`1            (auto) (ansi) 
// Class UdpChannelListener`3           (abstract) (auto) (ansi) 
// Class UdpDuplexChannelListener       (auto) (ansi) 
// Class UdpReplyChannelListener        (auto) (ansi) 
// Class ServerUdpDuplexChannel         (auto) (ansi) (sealed) 
// Class UdpConstants                   (abstract) (auto) (ansi) (sealed) 
// Class UdpDuplexChannel               (abstract) (auto) (ansi) 
// Class UdpOutputChannel               (abstract) (auto) (ansi) 
// Class NetworkInterfaceMessageProperty (public) (auto) (ansi) 
// Class UdpReplyChannel                (auto) (ansi) (sealed) 
// Class UdpRetransmissionSettings      (public) (auto) (ansi) (sealed) 
// Class UdpRequestContext              (auto) (ansi) 
// Class UdpSocket                      (auto) (ansi) 
// Class UdpSocketReceiveManager        (auto) (ansi) (sealed) 
// Class UdpTransportBindingElement     (public) (auto) (ansi) 
// Class UdpTransportBindingElementPolicy (abstract) (auto) (ansi) (sealed) 
// Class UdpTransportImporter           (public) (auto) (ansi) 
// Class UdpUtility                     (abstract) (auto) (ansi) (sealed) 
// Class XmlByteStreamReader            (abstract) (auto) (ansi) 
// Class XmlByteStreamWriter            (auto) (ansi) (sealed) 
// Class XmlBufferedByteStreamReader    (auto) (ansi) 
// Class XmlStreamedByteStreamReader    (abstract) (auto) (ansi) 
// Class HttpResponseMessageExtensionMethods (public) (abstract) (auto) (ansi) (sealed) 
// Class HttpRequestMessageExtensionMethods (public) (abstract) (auto) (ansi) (sealed) 
// Class MessageExtensionMethods        (public) (abstract) (auto) (ansi) (sealed) 
// Class ByteStreamConfigurationStrings (abstract) (auto) (ansi) (sealed) 
// Class SR                             (auto) (ansi) 
// Class TD                             (auto) (ansi) 
// Class FxTrace                        (abstract) (auto) (ansi) (sealed) 
// Class Defaults                       (abstract) (auto) (ansi) (sealed) (nested assembly) 
// Class UdpSocketReceiveState          (auto) (ansi) (nested assembly) 
// Class ReaderPosition                 (auto) (ansi) (sealed) (nested family) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ System.Xaml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000003*/ System.ServiceModel
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000004*/ System.Runtime.Serialization
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000005*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000006*/ System.Configuration
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly extern /*23000007*/ System.Net.Http
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly extern /*23000008*/ System.ServiceModel.Internals
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly extern /*23000009*/ System.Xml
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*2300000A*/ System.Web.Services
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly /*20000001*/ noplatform System.ServiceModel.Channels
{
  .custom /*0C000002:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
  .custom /*0C000003:0A000009*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000066*/::.ctor(int32) /* 0A000009 */ = ( 01 00 08 00 00 00 00 00 ) 

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000004:0A00000A*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000067*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000067*//DebuggingModes/*01000068*/) /* 0A00000A */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000005:0A00000B*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.GuidAttribute/*01000069*/::.ctor(string) /* 0A00000B */ = ( 01 00 24 43 32 31 39 36 43 42 37 2D 33 31 38 31   // ..$C2196CB7-3181
                                                                                                                                                               2D 34 65 62 65 2D 39 37 42 33 2D 37 33 41 30 37   // -4ebe-97B3-73A07
                                                                                                                                                               35 30 36 36 38 38 30 00 00 )                      // 5066880..
  .custom /*0C000006:0A00000C*/ instance void [System.Xaml/*23000002*/]System.Windows.Markup.XmlnsDefinitionAttribute/*0100006A*/::.ctor(string,
                                                                                                                                         string) /* 0A00000C */ = ( 01 00 39 68 74 74 70 3A 2F 2F 73 63 68 65 6D 61   // ..9http://schema
                                                                                                                                                                    73 2E 6D 69 63 72 6F 73 6F 66 74 2E 63 6F 6D 2F   // s.microsoft.com/
                                                                                                                                                                    6E 65 74 66 78 2F 32 30 30 39 2F 78 61 6D 6C 2F   // netfx/2009/xaml/
                                                                                                                                                                    73 65 72 76 69 63 65 6D 6F 64 65 6C 1C 53 79 73   // servicemodel.Sys
                                                                                                                                                                    74 65 6D 2E 53 65 72 76 69 63 65 4D 6F 64 65 6C   // tem.ServiceModel
                                                                                                                                                                    2E 43 68 61 6E 6E 65 6C 73 00 00 )                // .Channels..
  .custom /*0C000007:0A00000D*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*0100006B*/::.ctor(string) /* 0A00000D */ = ( 01 00 81 6D 4E 65 74 46 78 2E 43 68 65 63 6B 69   // ...mNetFx.Checki
                                                                                                                                                                              6E 2E 43 6F 6D 6D 6F 6E 2E 4E 65 74 46 78 46 72   // n.Common.NetFxFr
                                                                                                                                                                              69 65 6E 64 73 2C 20 50 75 62 6C 69 63 4B 65 79   // iends, PublicKey
                                                                                                                                                                              3D 30 30 32 34 30 30 30 30 30 34 38 30 30 30 30   // =002400000480000
                                                                                                                                                                              30 39 34 30 30 30 30 30 30 30 36 30 32 30 30 30   // 0940000000602000
                                                                                                                                                                              30 30 30 32 34 30 30 30 30 35 32 35 33 34 31 33   // 0002400005253413
                                                                                                                                                                              31 30 30 30 34 30 30 30 30 30 31 30 30 30 31 30   // 1000400000100010
                                                                                                                                                                              30 62 35 66 63 39 30 65 37 30 32 37 66 36 37 38   // 0b5fc90e7027f678
                                                                                                                                                                              37 31 65 37 37 33 61 38 66 64 65 38 39 33 38 63   // 71e773a8fde8938c
                                                                                                                                                                              38 31 64 64 34 30 32 62 61 36 35 62 39 32 30 31   // 81dd402ba65b9201
                                                                                                                                                                              64 36 30 35 39 33 65 39 36 63 34 39 32 36 35 31   // d60593e96c492651
                                                                                                                                                                              65 38 38 39 63 63 31 33 66 31 34 31 35 65 62 62   // e889cc13f1415ebb
                                                                                                                                                                              35 33 66 61 63 31 31 33 31 61 65 30 62 64 33 33   // 53fac1131ae0bd33
                                                                                                                                                                              33 63 35 65 65 36 30 32 31 36 37 32 64 39 37 31   // 3c5ee6021672d971
                                                                                                                                                                              38 65 61 33 31 61 38 61 65 62 64 30 64 61 30 30   // 8ea31a8aebd0da00
                                                                                                                                                                              37 32 66 32 35 64 38 37 64 62 61 36 66 63 39 30   // 72f25d87dba6fc90
                                                                                                                                                                              66 66 64 35 39 38 65 64 34 64 61 33 35 65 34 34   // ffd598ed4da35e44
                                                                                                                                                                              63 33 39 38 63 34 35 34 33 30 37 65 38 65 33 33   // c398c454307e8e33
                                                                                                                                                                              62 38 34 32 36 31 34 33 64 61 65 63 39 66 35 39   // b8426143daec9f59
                                                                                                                                                                              36 38 33 36 66 39 37 63 38 66 37 34 37 35 30 65   // 6836f97c8f74750e
                                                                                                                                                                              35 39 37 35 63 36 34 65 32 31 38 39 66 34 35 64   // 5975c64e2189f45d
                                                                                                                                                                              65 66 34 36 62 32 61 32 62 31 32 34 37 61 64 63   // ef46b2a2b1247adc
                                                                                                                                                                              33 36 35 32 62 66 35 63 33 30 38 30 35 35 64 61   // 3652bf5c308055da
                                                                                                                                                                              39 00 00 )                                        // 9..
  .custom /*0C000008:0A00000D*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*0100006B*/::.ctor(string) /* 0A00000D */ = ( 01 00 81 66 53 79 73 74 65 6D 2E 53 65 72 76 69   // ...fSystem.Servi
                                                                                                                                                                              63 65 4D 6F 64 65 6C 2E 46 72 69 65 6E 64 2C 20   // ceModel.Friend, 
                                                                                                                                                                              50 75 62 6C 69 63 4B 65 79 3D 30 30 32 34 30 30   // PublicKey=002400
                                                                                                                                                                              30 30 30 34 38 30 30 30 30 30 39 34 30 30 30 30   // 0004800000940000
                                                                                                                                                                              30 30 30 36 30 32 30 30 30 30 30 30 32 34 30 30   // 0006020000002400
                                                                                                                                                                              30 30 35 32 35 33 34 31 33 31 30 30 30 34 30 30   // 0052534131000400
                                                                                                                                                                              30 30 30 31 30 30 30 31 30 30 30 37 64 31 66 61   // 000100010007d1fa
                                                                                                                                                                              35 37 63 34 61 65 64 39 66 30 61 33 32 65 38 34   // 57c4aed9f0a32e84
                                                                                                                                                                              61 61 30 66 61 65 66 64 30 64 65 39 65 38 66 64   // aa0faefd0de9e8fd
                                                                                                                                                                              36 61 65 63 38 66 38 37 66 62 30 33 37 36 36 63   // 6aec8f87fb03766c
                                                                                                                                                                              38 33 34 63 39 39 39 32 31 65 62 32 33 62 65 37   // 834c99921eb23be7
                                                                                                                                                                              39 61 64 39 64 35 64 63 63 31 64 64 39 61 64 32   // 9ad9d5dcc1dd9ad2
                                                                                                                                                                              33 36 31 33 32 31 30 32 39 30 30 62 37 32 33 63   // 36132102900b723c
                                                                                                                                                                              66 39 38 30 39 35 37 66 63 34 65 31 37 37 31 30   // f980957fc4e17710
                                                                                                                                                                              38 66 63 36 30 37 37 37 34 66 32 39 65 38 33 32   // 8fc607774f29e832
                                                                                                                                                                              30 65 39 32 65 61 30 35 65 63 65 34 65 38 32 31   // 0e92ea05ece4e821
                                                                                                                                                                              63 30 61 35 65 66 65 38 66 31 36 34 35 63 34 63   // c0a5efe8f1645c4c
                                                                                                                                                                              30 63 39 33 63 31 61 62 39 39 32 38 35 64 36 32   // 0c93c1ab99285d62
                                                                                                                                                                              32 63 61 61 36 35 32 63 31 64 66 61 64 36 33 64   // 2caa652c1dfad63d
                                                                                                                                                                              37 34 35 64 36 66 32 64 65 35 66 31 37 65 35 65   // 745d6f2de5f17e5e
                                                                                                                                                                              61 66 30 66 63 34 39 36 33 64 32 36 31 63 38 61   // af0fc4963d261c8a
                                                                                                                                                                              31 32 34 33 36 35 31 38 32 30 36 64 63 30 39 33   // 12436518206dc093
                                                                                                                                                                              33 34 34 64 35 61 64 32 39 33 00 00 )             // 344d5ad293..
  .custom /*0C000009:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*0100006C*/::.ctor(bool) /* 0A00000E */ = ( 01 00 00 00 00 ) 
  .custom /*0C00000A:0A00000F*/ instance void [mscorlib/*23000001*/]System.CLSCompliantAttribute/*0100006D*/::.ctor(bool) /* 0A00000F */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000B:0A000010*/ instance void [mscorlib/*23000001*/]System.Security.AllowPartiallyTrustedCallersAttribute/*0100006E*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00000C:0A000011*/ instance void [mscorlib/*23000001*/]System.Security.SecurityRulesAttribute/*0100006F*/::.ctor(valuetype [mscorlib/*23000001*/]System.Security.SecurityRuleSet/*01000070*/) /* 0A000011 */ = ( 01 00 02 01 00 54 02 1B 53 6B 69 70 56 65 72 69   // .....T..SkipVeri
                                                                                                                                                                                                                              66 69 63 61 74 69 6F 6E 49 6E 46 75 6C 6C 54 72   // ficationInFullTr
                                                                                                                                                                                                                              75 73 74 01 )                                     // ust.
  .custom /*0C00000D:0A000012*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000071*/::.ctor(string) /* 0A000012 */ = ( 01 00 20 53 79 73 74 65 6D 2E 53 65 72 76 69 63   // .. System.Servic
                                                                                                                                                           65 4D 6F 64 65 6C 2E 43 68 61 6E 6E 65 6C 73 2E   // eModel.Channels.
                                                                                                                                                           64 6C 6C 00 00 )                                  // dll..
  .custom /*0C00000E:0A000013*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*01000072*/::.ctor(string) /* 0A000013 */ = ( 01 00 20 53 79 73 74 65 6D 2E 53 65 72 76 69 63   // .. System.Servic
                                                                                                                                                                 65 4D 6F 64 65 6C 2E 43 68 61 6E 6E 65 6C 73 2E   // eModel.Channels.
                                                                                                                                                                 64 6C 6C 00 00 )                                  // dll..
  .custom /*0C00000F:0A000014*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000073*/::.ctor(string) /* 0A000014 */ = ( 01 00 20 53 79 73 74 65 6D 2E 53 65 72 76 69 63   // .. System.Servic
                                                                                                                                                                  65 4D 6F 64 65 6C 2E 43 68 61 6E 6E 65 6C 73 2E   // eModel.Channels.
                                                                                                                                                                  64 6C 6C 00 00 )                                  // dll..
  .custom /*0C000010:0A000015*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000074*/::.ctor(string) /* 0A000015 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                             70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A000016*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000075*/::.ctor(string) /* 0A000016 */ = ( 01 00 1A 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                             4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 00 00 )    // NET Framework..
  .custom /*0C000012:0A000017*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000076*/::.ctor(string) /* 0A000017 */ = ( 01 00 2F C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ../.. Microsoft 
                                                                                                                                                               43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 20 41 6C   // Corporation.  Al
                                                                                                                                                               6C 20 72 69 67 68 74 73 20 72 65 73 65 72 76 65   // l rights reserve
                                                                                                                                                               64 2E 00 00 )                                     // d...
  .custom /*0C000013:0A000018*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000077*/::.ctor(string) /* 0A000018 */ = ( 01 00 0A 34 2E 36 2E 31 30 35 35 2E 30 00 00 )    // ...4.6.1055.0..
  .custom /*0C000014:0A000019*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000078*/::.ctor(string) /* 0A000019 */ = ( 01 00 0A 34 2E 36 2E 31 30 35 35 2E 30 00 00 )    // ...4.6.1055.0..
  .custom /*0C000015:0A00001A*/ instance void [mscorlib/*23000001*/]System.Resources.SatelliteContractVersionAttribute/*01000079*/::.ctor(string) /* 0A00001A */ = ( 01 00 07 34 2E 30 2E 30 2E 30 00 00 )             // ...4.0.0.0..
  .custom /*0C000016:0A00001B*/ instance void [mscorlib/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*0100007A*/::.ctor(string) /* 0A00001B */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C000017:0A00001C*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDelaySignAttribute/*0100007B*/::.ctor(bool) /* 0A00001C */ = ( 01 00 01 00 00 ) 
  .custom /*0C000018:0A00001D*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyKeyFileAttribute/*0100007C*/::.ctor(string) /* 0A00001D */ = ( 01 00 28 66 3A 5C 64 64 5C 74 6F 6F 6C 73 5C 64   // ..(f:\dd\tools\d
                                                                                                                                                             65 76 64 69 76 5C 33 35 4D 53 53 68 61 72 65 64   // evdiv\35MSShared
                                                                                                                                                             4C 69 62 31 30 32 34 2E 73 6E 6B 00 00 )          // Lib1024.snk..
  .custom /*0C000019:0A00001E*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblySignatureKeyAttribute/*0100007D*/::.ctor(string,
                                                                                                                                       string) /* 0A00001E */ = ( 01 00 82 40 30 30 32 34 30 30 30 30 30 63 38 30   // ...@002400000c80
                                                                                                                                                                  30 30 30 30 31 34 30 31 30 30 30 30 30 36 30 32   // 0000140100000602
                                                                                                                                                                  30 30 30 30 30 30 32 34 30 30 30 30 35 32 35 33   // 0000002400005253
                                                                                                                                                                  34 31 33 31 30 30 30 38 30 30 30 30 30 31 30 30   // 4131000800000100
                                                                                                                                                                  30 31 30 30 38 35 61 61 64 30 62 65 66 30 36 38   // 010085aad0bef068
                                                                                                                                                                  38 64 31 62 39 39 34 61 30 64 37 38 65 31 66 64   // 8d1b994a0d78e1fd
                                                                                                                                                                  32 39 66 63 32 34 61 63 33 34 65 64 33 64 33 61   // 29fc24ac34ed3d3a
                                                                                                                                                                  63 33 66 62 39 62 33 64 30 63 34 38 33 38 36 62   // c3fb9b3d0c48386b
                                                                                                                                                                  61 38 33 34 61 61 38 38 30 30 33 35 30 36 30 61   // a834aa880035060a
                                                                                                                                                                  38 38 34 38 62 32 64 38 61 64 66 35 38 65 36 37   // 8848b2d8adf58e67
                                                                                                                                                                  30 65 64 32 30 39 31 34 62 65 33 36 38 31 61 38   // 0ed20914be3681a8
                                                                                                                                                                  39 31 63 39 63 38 63 30 31 65 65 66 32 61 62 32   // 91c9c8c01eef2ab2
                                                                                                                                                                  32 38 37 32 35 34 37 63 33 39 62 65 30 30 61 66   // 2872547c39be00af
                                                                                                                                                                  30 65 36 63 37 32 34 38 35 64 37 63 66 64 31 61   // 0e6c72485d7cfd1a
                                                                                                                                                                  35 31 64 66 38 39 34 37 64 33 36 63 65 62 61 39   // 51df8947d36ceba9
                                                                                                                                                                  39 38 39 31 30 36 62 35 38 61 62 65 37 39 65 36   // 989106b58abe79e6
                                                                                                                                                                  61 33 65 37 31 61 30 31 65 64 36 62 64 63 38 36   // a3e71a01ed6bdc86
                                                                                                                                                                  37 30 31 32 38 38 33 65 30 62 31 61 34 64 33 35   // 7012883e0b1a4d35
                                                                                                                                                                  62 31 62 35 65 65 65 64 36 64 66 32 31 65 34 30   // b1b5eeed6df21e40
                                                                                                                                                                  31 62 62 30 63 32 32 66 32 32 34 36 63 63 62 36   // 1bb0c22f2246ccb6
                                                                                                                                                                  39 39 37 39 64 63 39 65 36 31 65 65 66 32 36 32   // 9979dc9e61eef262
                                                                                                                                                                  38 33 32 65 64 30 66 32 30 36 34 38 35 33 37 32   // 832ed0f206485372
                                                                                                                                                                  35 61 37 35 34 38 35 66 61 38 61 33 65 66 62 37   // 5a75485fa8a3efb7
                                                                                                                                                                  65 30 32 37 33 31 39 63 38 36 64 65 63 30 33 64   // e027319c86dec03d
                                                                                                                                                                  63 33 62 31 62 63 61 32 62 35 30 38 31 62 61 62   // c3b1bca2b5081bab
                                                                                                                                                                  35 32 61 36 32 37 62 39 39 31 37 34 35 30 64 66   // 52a627b9917450df
                                                                                                                                                                  61 64 35 33 34 37 39 39 65 31 63 37 61 66 35 38   // ad534799e1c7af58
                                                                                                                                                                  36 38 33 62 64 66 61 31 33 35 66 31 35 31 38 66   // 683bdfa135f1518f
                                                                                                                                                                  66 31 65 61 36 30 65 39 30 64 37 62 39 39 33 61   // f1ea60e90d7b993a
                                                                                                                                                                  36 63 38 37 66 64 33 64 64 39 33 34 30 38 65 33   // 6c87fd3dd93408e3
                                                                                                                                                                  35 64 31 32 39 36 66 39 61 37 66 39 61 39 37 63   // 5d1296f9a7f9a97c
                                                                                                                                                                  35 64 62 35 36 63 30 66 33 63 63 32 35 61 64 31   // 5db56c0f3cc25ad1
                                                                                                                                                                  31 65 39 37 37 37 66 39 34 64 31 33 38 62 33 63   // 1e9777f94d138b3c
                                                                                                                                                                  65 61 35 33 62 39 61 38 33 33 31 63 32 65 36 64   // ea53b9a8331c2e6d
                                                                                                                                                                  63 62 38 64 32 65 61 39 34 65 31 38 62 66 31 31   // cb8d2ea94e18bf11
                                                                                                                                                                  36 33 66 66 31 31 32 61 32 32 64 62 64 39 32 64   // 63ff112a22dbd92d
                                                                                                                                                                  34 32 39 61 81 00 38 39 31 33 65 66 38 36 39 36   // 429a..8913ef8696
                                                                                                                                                                  34 36 64 31 34 39 37 31 64 66 32 32 32 63 32 31   // 46d14971df222c21
                                                                                                                                                                  30 30 31 38 61 62 33 39 34 63 66 65 36 33 66 38   // 0018ab394cfe63f8
                                                                                                                                                                  65 62 39 62 34 64 38 39 34 63 30 64 64 61 37 33   // eb9b4d894c0dda73
                                                                                                                                                                  36 38 63 66 62 36 39 64 66 31 35 64 30 34 39 62   // 68cfb69df15d049b
                                                                                                                                                                  33 34 37 66 38 61 38 62 39 32 30 35 63 64 63 63   // 347f8a8b9205cdcc
                                                                                                                                                                  33 64 36 62 64 36 36 39 30 66 30 62 64 32 34 62   // 3d6bd6690f0bd24b
                                                                                                                                                                  33 64 61 33 31 37 39 66 65 62 38 63 30 33 66 34   // 3da3179feb8c03f4
                                                                                                                                                                  31 30 37 30 33 30 32 37 63 37 38 34 34 66 66 36   // 10703027c7844ff6
                                                                                                                                                                  35 34 39 39 37 65 33 38 30 31 35 64 66 63 39 37   // 54997e38015dfc97
                                                                                                                                                                  32 32 32 63 31 35 30 36 31 61 66 32 61 31 64 33   // 222c15061af2a1d3
                                                                                                                                                                  66 65 39 31 37 37 35 62 39 64 64 34 62 38 65 64   // fe91775b9dd4b8ed
                                                                                                                                                                  65 36 34 64 33 64 35 39 38 31 36 32 37 30 61 39   // e64d3d59816270a9
                                                                                                                                                                  35 32 30 65 33 39 33 63 38 65 36 30 36 31 39 62   // 520e393c8e60619b
                                                                                                                                                                  38 30 64 34 30 66 63 31 64 63 34 66 31 32 62 35   // 80d40fc1dc4f12b5
                                                                                                                                                                  61 61 30 63 32 64 66 32 30 64 30 32 65 61 33 32   // aa0c2df20d02ea32
                                                                                                                                                                  39 36 30 38 35 31 00 00 )                         // 960851..
  .custom /*0C00001A:0A00001F*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*0100007E*/::.ctor() /* 0A00001F */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C00001B:0A000020*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*0100007F*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
  .permissionset reqmin
             = {class 'System.Security.Permissions.SecurityPermissionAttribute, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' = {property bool 'Execution' = bool(true)}}
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                B5 FC 90 E7 02 7F 67 87 1E 77 3A 8F DE 89 38 C8   // ......g..w:...8.
                1D D4 02 BA 65 B9 20 1D 60 59 3E 96 C4 92 65 1E   // ....e. .`Y>...e.
                88 9C C1 3F 14 15 EB B5 3F AC 11 31 AE 0B D3 33   // ...?....?..1...3
                C5 EE 60 21 67 2D 97 18 EA 31 A8 AE BD 0D A0 07   // ..`!g-...1......
                2F 25 D8 7D BA 6F C9 0F FD 59 8E D4 DA 35 E4 4C   // /%.}.o...Y...5.L
                39 8C 45 43 07 E8 E3 3B 84 26 14 3D AE C9 F5 96   // 9.EC...;.&.=....
                83 6F 97 C8 F7 47 50 E5 97 5C 64 E2 18 9F 45 DE   // .o...GP..\d...E.
                F4 6B 2A 2B 12 47 AD C3 65 2B F5 C3 08 05 5D A9 ) // .k*+.G..e+....].
  .hash algorithm 0x00008004
  .ver 4:0:0:0
}
.module System.ServiceModel.Channels.dll
// MVID: {6fb12322-96e1-4be6-84ac-084ab85f0a4f}
.imagebase 0x53100000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007FFA6BAE4000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.ServiceModel.UdpBinding
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Binding/*01000001*/
       implements [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IBindingRuntimePreferences/*01000002*/
{
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       0 (0x0)
  } // end of method UdpBinding::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string configurationName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2051
    // Code size       0 (0x0)
  } // end of method UdpBinding::.ctor

  .method /*06000003*/ public hidebysig specialname 
          instance int32  get_DuplicateMessageHistoryLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2052
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_DuplicateMessageHistoryLength

  .method /*06000004*/ public hidebysig specialname 
          instance void  set_DuplicateMessageHistoryLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2053
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_DuplicateMessageHistoryLength

  .method /*06000005*/ public hidebysig specialname 
          instance int64  get_MaxBufferPoolSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2054
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_MaxBufferPoolSize

  .method /*06000006*/ public hidebysig specialname 
          instance void  set_MaxBufferPoolSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2055
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_MaxBufferPoolSize

  .method /*06000007*/ public hidebysig specialname 
          instance int32  get_MaxRetransmitCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2056
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_MaxRetransmitCount

  .method /*06000008*/ public hidebysig specialname 
          instance void  set_MaxRetransmitCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2057
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_MaxRetransmitCount

  .method /*06000009*/ public hidebysig specialname 
          instance int64  get_MaxPendingMessagesTotalSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2058
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_MaxPendingMessagesTotalSize

  .method /*0600000A*/ public hidebysig specialname 
          instance void  set_MaxPendingMessagesTotalSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2059
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_MaxPendingMessagesTotalSize

  .method /*0600000B*/ public hidebysig specialname 
          instance int64  get_MaxReceivedMessageSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x205a
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_MaxReceivedMessageSize

  .method /*0600000C*/ public hidebysig specialname 
          instance void  set_MaxReceivedMessageSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x205b
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_MaxReceivedMessageSize

  .method /*0600000D*/ public hidebysig specialname 
          instance string  get_MulticastInterfaceId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x205c
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_MulticastInterfaceId

  .method /*0600000E*/ public hidebysig specialname 
          instance void  set_MulticastInterfaceId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x205d
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_MulticastInterfaceId

  .method /*0600000F*/ public hidebysig specialname 
          instance class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ 
          get_ReaderQuotas() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x205e
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_ReaderQuotas

  .method /*06000010*/ public hidebysig specialname 
          instance void  set_ReaderQuotas(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x205f
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_ReaderQuotas

  .method /*06000011*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/ 
          get_TextEncoding() cil managed
  // SIG: 20 00 12 11
  {
    // Method begins at RVA 0x2060
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_TextEncoding

  .method /*06000012*/ public hidebysig specialname 
          instance void  set_TextEncoding(class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/ 'value') cil managed
  // SIG: 20 01 01 12 11
  {
    // Method begins at RVA 0x2061
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_TextEncoding

  .method /*06000013*/ public hidebysig specialname 
          instance int32  get_TimeToLive() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2062
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_TimeToLive

  .method /*06000014*/ public hidebysig specialname 
          instance void  set_TimeToLive(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2063
    // Code size       0 (0x0)
  } // end of method UdpBinding::set_TimeToLive

  .method /*06000015*/ public hidebysig specialname virtual 
          instance string  get_Scheme() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2064
    // Code size       0 (0x0)
  } // end of method UdpBinding::get_Scheme

  .method /*06000016*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElementCollection/*01000005*/ 
          CreateBindingElements() cil managed
  // SIG: 20 00 12 15
  {
    // Method begins at RVA 0x2065
    // Code size       0 (0x0)
  } // end of method UdpBinding::CreateBindingElements

  .method /*06000017*/ private hidebysig newslot specialname virtual final 
          instance bool  System.ServiceModel.Channels.IBindingRuntimePreferences.get_ReceiveSynchronously() cil managed
  // SIG: 20 00 02
  {
    .override [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IBindingRuntimePreferences/*01000002*/::get_ReceiveSynchronously /*01000002::0A000001*/ 
    // Method begins at RVA 0x2066
    // Code size       0 (0x0)
  } // end of method UdpBinding::System.ServiceModel.Channels.IBindingRuntimePreferences.get_ReceiveSynchronously

  .method /*06000018*/ public hidebysig instance bool 
          ShouldSerializeReaderQuotas() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000034:0A000021*/ instance void [System/*23000005*/]System.ComponentModel.EditorBrowsableAttribute/*01000080*/::.ctor(valuetype [System/*23000005*/]System.ComponentModel.EditorBrowsableState/*01000081*/) /* 0A000021 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2067
    // Code size       0 (0x0)
  } // end of method UdpBinding::ShouldSerializeReaderQuotas

  .method /*06000019*/ public hidebysig instance bool 
          ShouldSerializeTextEncoding() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000036:0A000021*/ instance void [System/*23000005*/]System.ComponentModel.EditorBrowsableAttribute/*01000080*/::.ctor(valuetype [System/*23000005*/]System.ComponentModel.EditorBrowsableState/*01000081*/) /* 0A000021 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       0 (0x0)
  } // end of method UdpBinding::ShouldSerializeTextEncoding

  .method /*0600001A*/ assembly hidebysig static 
          bool  TryCreate(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElementCollection/*01000005*/ bindingElements,
                          [out] class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Binding/*01000001*/& binding) cil managed
  // SIG: 00 02 02 12 15 10 12 05
  {
    // Method begins at RVA 0x2069
    // Code size       0 (0x0)
  } // end of method UdpBinding::TryCreate

  .property /*17000001*/ instance callconv(8) int32
          DuplicateMessageHistoryLength()
  {
    .custom /*0C000001:0A000022*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int32) /* 0A000022 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance int32 System.ServiceModel.UdpBinding/*02000002*/::get_DuplicateMessageHistoryLength() /* 06000003 */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_DuplicateMessageHistoryLength(int32) /* 06000004 */
  } // end of property UdpBinding::DuplicateMessageHistoryLength
  .property /*17000002*/ instance callconv(8) int64
          MaxBufferPoolSize()
  {
    .custom /*0C00001C:0A000023*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int64) /* 0A000023 */ = ( 01 00 00 00 08 00 00 00 00 00 00 00 ) 
    .get instance int64 System.ServiceModel.UdpBinding/*02000002*/::get_MaxBufferPoolSize() /* 06000005 */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_MaxBufferPoolSize(int64) /* 06000006 */
  } // end of property UdpBinding::MaxBufferPoolSize
  .property /*17000003*/ instance callconv(8) int32
          MaxRetransmitCount()
  {
    .custom /*0C00001D:0A000022*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int32) /* 0A000022 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance int32 System.ServiceModel.UdpBinding/*02000002*/::get_MaxRetransmitCount() /* 06000007 */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_MaxRetransmitCount(int32) /* 06000008 */
  } // end of property UdpBinding::MaxRetransmitCount
  .property /*17000004*/ instance callconv(8) int64
          MaxPendingMessagesTotalSize()
  {
    .custom /*0C00001E:0A000023*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int64) /* 0A000023 */ = ( 01 00 00 00 00 00 00 00 00 00 00 00 ) 
    .get instance int64 System.ServiceModel.UdpBinding/*02000002*/::get_MaxPendingMessagesTotalSize() /* 06000009 */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_MaxPendingMessagesTotalSize(int64) /* 0600000A */
  } // end of property UdpBinding::MaxPendingMessagesTotalSize
  .property /*17000005*/ instance callconv(8) int64
          MaxReceivedMessageSize()
  {
    .custom /*0C00001F:0A000023*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int64) /* 0A000023 */ = ( 01 00 00 00 01 00 00 00 00 00 00 00 ) 
    .get instance int64 System.ServiceModel.UdpBinding/*02000002*/::get_MaxReceivedMessageSize() /* 0600000B */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_MaxReceivedMessageSize(int64) /* 0600000C */
  } // end of property UdpBinding::MaxReceivedMessageSize
  .property /*17000006*/ instance callconv(8) string
          MulticastInterfaceId()
  {
    .custom /*0C000020:0A000024*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(string) /* 0A000024 */ = ( 01 00 FF 00 00 ) 
    .get instance string System.ServiceModel.UdpBinding/*02000002*/::get_MulticastInterfaceId() /* 0600000D */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_MulticastInterfaceId(string) /* 0600000E */
  } // end of property UdpBinding::MulticastInterfaceId
  .property /*17000007*/ instance callconv(8) class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/
          ReaderQuotas()
  {
    .get instance class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ System.ServiceModel.UdpBinding/*02000002*/::get_ReaderQuotas() /* 0600000F */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_ReaderQuotas(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/) /* 06000010 */
  } // end of property UdpBinding::ReaderQuotas
  .property /*17000008*/ instance callconv(8) class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/
          TextEncoding()
  {
    .custom /*0C000021:0A000025*/ instance void [System/*23000005*/]System.ComponentModel.TypeConverterAttribute/*01000083*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000008*/) /* 0A000025 */ = ( 01 00 80 8B 53 79 73 74 65 6D 2E 53 65 72 76 69   // ....System.Servi
                                                                                                                                                                                                            63 65 4D 6F 64 65 6C 2E 43 6F 6E 66 69 67 75 72   // ceModel.Configur
                                                                                                                                                                                                            61 74 69 6F 6E 2E 45 6E 63 6F 64 69 6E 67 43 6F   // ation.EncodingCo
                                                                                                                                                                                                            6E 76 65 72 74 65 72 2C 20 53 79 73 74 65 6D 2E   // nverter, System.
                                                                                                                                                                                                            53 65 72 76 69 63 65 4D 6F 64 65 6C 2C 20 56 65   // ServiceModel, Ve
                                                                                                                                                                                                            72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C 20 43   // rsion=4.0.0.0, C
                                                                                                                                                                                                            75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20   // ulture=neutral, 
                                                                                                                                                                                                            50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62   // PublicKeyToken=b
                                                                                                                                                                                                            37 37 61 35 63 35 36 31 39 33 34 65 30 38 39 00   // 77a5c561934e089.
                                                                                                                                                                                                            00 ) 
    .get instance class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/ System.ServiceModel.UdpBinding/*02000002*/::get_TextEncoding() /* 06000011 */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_TextEncoding(class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/) /* 06000012 */
  } // end of property UdpBinding::TextEncoding
  .property /*17000009*/ instance callconv(8) int32
          TimeToLive()
  {
    .custom /*0C000022:0A000022*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int32) /* 0A000022 */ = ( 01 00 01 00 00 00 00 00 ) 
    .get instance int32 System.ServiceModel.UdpBinding/*02000002*/::get_TimeToLive() /* 06000013 */
    .set instance void System.ServiceModel.UdpBinding/*02000002*/::set_TimeToLive(int32) /* 06000014 */
  } // end of property UdpBinding::TimeToLive
  .property /*1700000A*/ instance callconv(8) string
          Scheme()
  {
    .get instance string System.ServiceModel.UdpBinding/*02000002*/::get_Scheme() /* 06000015 */
  } // end of property UdpBinding::Scheme
  .property /*1700000B*/ instance callconv(8) bool
          System.ServiceModel.Channels.IBindingRuntimePreferences.ReceiveSynchronously()
  {
    .get instance bool System.ServiceModel.UdpBinding/*02000002*/::System.ServiceModel.Channels.IBindingRuntimePreferences.get_ReceiveSynchronously() /* 06000017 */
  } // end of property UdpBinding::System.ServiceModel.Channels.IBindingRuntimePreferences.ReceiveSynchronously
} // end of class System.ServiceModel.UdpBinding

.class /*02000003*/ public auto ansi beforefieldinit System.ServiceModel.Configuration.UdpBindingCollectionElement
       extends class [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.StandardBindingCollectionElement`2/*01000006*/<class System.ServiceModel.UdpBinding/*02000002*/,class System.ServiceModel.Configuration.UdpBindingElement/*02000004*/>/*1B000001*/
{
  .method /*0600001B*/ assembly hidebysig static 
          class System.ServiceModel.Configuration.UdpBindingCollectionElement/*02000003*/ 
          GetBindingCollectionElement() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x206a
    // Code size       0 (0x0)
  } // end of method UdpBindingCollectionElement::GetBindingCollectionElement

  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206b
    // Code size       0 (0x0)
  } // end of method UdpBindingCollectionElement::.ctor

} // end of class System.ServiceModel.Configuration.UdpBindingCollectionElement

.class /*02000004*/ public auto ansi beforefieldinit System.ServiceModel.Configuration.UdpBindingElement
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.StandardBindingElement/*01000007*/
{
  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x206c
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::.ctor

  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206d
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::.ctor

  .method /*0600001F*/ family hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000008*/ 
          get_BindingElementType() cil managed
  // SIG: 20 00 12 21
  {
    // Method begins at RVA 0x206e
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_BindingElementType

  .method /*06000020*/ public hidebysig specialname 
          instance int32  get_DuplicateMessageHistoryLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x206f
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_DuplicateMessageHistoryLength

  .method /*06000021*/ public hidebysig specialname 
          instance void  set_DuplicateMessageHistoryLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2070
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::set_DuplicateMessageHistoryLength

  .method /*06000022*/ public hidebysig specialname 
          instance int64  get_MaxBufferPoolSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2071
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_MaxBufferPoolSize

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_MaxBufferPoolSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2072
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::set_MaxBufferPoolSize

  .method /*06000024*/ public hidebysig specialname 
          instance int32  get_MaxRetransmitCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2073
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_MaxRetransmitCount

  .method /*06000025*/ public hidebysig specialname 
          instance void  set_MaxRetransmitCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2074
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::set_MaxRetransmitCount

  .method /*06000026*/ public hidebysig specialname 
          instance int64  get_MaxPendingMessagesTotalSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2075
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_MaxPendingMessagesTotalSize

  .method /*06000027*/ public hidebysig specialname 
          instance void  set_MaxPendingMessagesTotalSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2076
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::set_MaxPendingMessagesTotalSize

  .method /*06000028*/ public hidebysig specialname 
          instance int64  get_MaxReceivedMessageSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2077
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_MaxReceivedMessageSize

  .method /*06000029*/ public hidebysig specialname 
          instance void  set_MaxReceivedMessageSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2078
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::set_MaxReceivedMessageSize

  .method /*0600002A*/ public hidebysig specialname 
          instance string  get_MulticastInterfaceId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2079
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_MulticastInterfaceId

  .method /*0600002B*/ public hidebysig specialname 
          instance void  set_MulticastInterfaceId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x207a
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::set_MulticastInterfaceId

  .method /*0600002C*/ public hidebysig specialname 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.XmlDictionaryReaderQuotasElement/*01000009*/ 
          get_ReaderQuotas() cil managed
  // SIG: 20 00 12 25
  {
    // Method begins at RVA 0x207b
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_ReaderQuotas

  .method /*0600002D*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/ 
          get_TextEncoding() cil managed
  // SIG: 20 00 12 11
  {
    // Method begins at RVA 0x207c
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_TextEncoding

  .method /*0600002E*/ public hidebysig specialname 
          instance void  set_TextEncoding(class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/ 'value') cil managed
  // SIG: 20 01 01 12 11
  {
    // Method begins at RVA 0x207d
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::set_TextEncoding

  .method /*0600002F*/ public hidebysig specialname 
          instance int32  get_TimeToLive() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x207e
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_TimeToLive

  .method /*06000030*/ public hidebysig specialname 
          instance void  set_TimeToLive(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x207f
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::set_TimeToLive

  .method /*06000031*/ famorassem hidebysig virtual 
          instance void  InitializeFrom(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Binding/*01000001*/ binding) cil managed
  // SIG: 20 01 01 12 05
  {
    // Method begins at RVA 0x2080
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::InitializeFrom

  .method /*06000032*/ family hidebysig virtual 
          instance void  OnApplyConfiguration(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Binding/*01000001*/ binding) cil managed
  // SIG: 20 01 01 12 05
  {
    // Method begins at RVA 0x2081
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::OnApplyConfiguration

  .method /*06000033*/ family hidebysig specialname virtual 
          instance class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 29
  {
    // Method begins at RVA 0x2082
    // Code size       0 (0x0)
  } // end of method UdpBindingElement::get_Properties

  .property /*1700000C*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000008*/
          BindingElementType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000008*/ System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_BindingElementType() /* 0600001F */
  } // end of property UdpBindingElement::BindingElementType
  .property /*1700000D*/ instance callconv(8) int32
          DuplicateMessageHistoryLength()
  {
    .custom /*0C000023:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 1D 64 75 70 6C 69 63 61 74 65 4D 65 73 73   // ...duplicateMess
                                                                                                                                                                                    61 67 65 48 69 73 74 6F 72 79 4C 65 6E 67 74 68   // ageHistoryLength
                                                                                                                                                                                    01 00 54 51 0C 44 65 66 61 75 6C 74 56 61 6C 75   // ..TQ.DefaultValu
                                                                                                                                                                                    65 08 00 00 00 00 )                               // e.....
    .custom /*0C000024:0A000027*/ instance void [System.Configuration/*23000006*/]System.Configuration.IntegerValidatorAttribute/*01000085*/::.ctor() /* 0A000027 */ = ( 01 00 01 00 54 08 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                         00 00 00 ) 
    .get instance int32 System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_DuplicateMessageHistoryLength() /* 06000020 */
    .set instance void System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::set_DuplicateMessageHistoryLength(int32) /* 06000021 */
  } // end of property UdpBindingElement::DuplicateMessageHistoryLength
  .property /*1700000E*/ instance callconv(8) int64
          MaxBufferPoolSize()
  {
    .custom /*0C000025:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 11 6D 61 78 42 75 66 66 65 72 50 6F 6F 6C   // ...maxBufferPool
                                                                                                                                                                                    53 69 7A 65 01 00 54 51 0C 44 65 66 61 75 6C 74   // Size..TQ.Default
                                                                                                                                                                                    56 61 6C 75 65 0A 00 00 08 00 00 00 00 00 )       // Value.........
    .custom /*0C000026:0A000028*/ instance void [System.Configuration/*23000006*/]System.Configuration.LongValidatorAttribute/*01000086*/::.ctor() /* 0A000028 */ = ( 01 00 01 00 54 0A 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                      00 00 00 00 00 00 00 ) 
    .get instance int64 System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_MaxBufferPoolSize() /* 06000022 */
    .set instance void System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::set_MaxBufferPoolSize(int64) /* 06000023 */
  } // end of property UdpBindingElement::MaxBufferPoolSize
  .property /*1700000F*/ instance callconv(8) int32
          MaxRetransmitCount()
  {
    .custom /*0C000027:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 12 6D 61 78 52 65 74 72 61 6E 73 6D 69 74   // ...maxRetransmit
                                                                                                                                                                                    43 6F 75 6E 74 01 00 54 51 0C 44 65 66 61 75 6C   // Count..TQ.Defaul
                                                                                                                                                                                    74 56 61 6C 75 65 08 00 00 00 00 )                // tValue.....
    .custom /*0C000028:0A000027*/ instance void [System.Configuration/*23000006*/]System.Configuration.IntegerValidatorAttribute/*01000085*/::.ctor() /* 0A000027 */ = ( 01 00 01 00 54 08 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                         00 00 00 ) 
    .get instance int32 System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_MaxRetransmitCount() /* 06000024 */
    .set instance void System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::set_MaxRetransmitCount(int32) /* 06000025 */
  } // end of property UdpBindingElement::MaxRetransmitCount
  .property /*17000010*/ instance callconv(8) int64
          MaxPendingMessagesTotalSize()
  {
    .custom /*0C000029:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 1B 6D 61 78 50 65 6E 64 69 6E 67 4D 65 73   // ...maxPendingMes
                                                                                                                                                                                    73 61 67 65 73 54 6F 74 61 6C 53 69 7A 65 01 00   // sagesTotalSize..
                                                                                                                                                                                    54 51 0C 44 65 66 61 75 6C 74 56 61 6C 75 65 0A   // TQ.DefaultValue.
                                                                                                                                                                                    00 00 00 00 00 00 00 00 ) 
    .custom /*0C00002A:0A000028*/ instance void [System.Configuration/*23000006*/]System.Configuration.LongValidatorAttribute/*01000086*/::.ctor() /* 0A000028 */ = ( 01 00 01 00 54 0A 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                      00 00 00 00 00 00 00 ) 
    .get instance int64 System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_MaxPendingMessagesTotalSize() /* 06000026 */
    .set instance void System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::set_MaxPendingMessagesTotalSize(int64) /* 06000027 */
  } // end of property UdpBindingElement::MaxPendingMessagesTotalSize
  .property /*17000011*/ instance callconv(8) int64
          MaxReceivedMessageSize()
  {
    .custom /*0C00002B:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 16 6D 61 78 52 65 63 65 69 76 65 64 4D 65   // ...maxReceivedMe
                                                                                                                                                                                    73 73 61 67 65 53 69 7A 65 01 00 54 51 0C 44 65   // ssageSize..TQ.De
                                                                                                                                                                                    66 61 75 6C 74 56 61 6C 75 65 0A 00 00 01 00 00   // faultValue......
                                                                                                                                                                                    00 00 00 ) 
    .custom /*0C00002C:0A000028*/ instance void [System.Configuration/*23000006*/]System.Configuration.LongValidatorAttribute/*01000086*/::.ctor() /* 0A000028 */ = ( 01 00 01 00 54 0A 08 4D 69 6E 56 61 6C 75 65 01   // ....T..MinValue.
                                                                                                                                                                      00 00 00 00 00 00 00 ) 
    .get instance int64 System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_MaxReceivedMessageSize() /* 06000028 */
    .set instance void System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::set_MaxReceivedMessageSize(int64) /* 06000029 */
  } // end of property UdpBindingElement::MaxReceivedMessageSize
  .property /*17000012*/ instance callconv(8) string
          MulticastInterfaceId()
  {
    .custom /*0C00002D:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 14 6D 75 6C 74 69 63 61 73 74 49 6E 74 65   // ...multicastInte
                                                                                                                                                                                    72 66 61 63 65 49 64 01 00 54 51 0C 44 65 66 61   // rfaceId..TQ.Defa
                                                                                                                                                                                    75 6C 74 56 61 6C 75 65 0E FF )                   // ultValue..
    .custom /*0C00002E:0A000029*/ instance void [System.Configuration/*23000006*/]System.Configuration.StringValidatorAttribute/*01000087*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
    .get instance string System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_MulticastInterfaceId() /* 0600002A */
    .set instance void System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::set_MulticastInterfaceId(string) /* 0600002B */
  } // end of property UdpBindingElement::MulticastInterfaceId
  .property /*17000013*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.XmlDictionaryReaderQuotasElement/*01000009*/
          ReaderQuotas()
  {
    .custom /*0C00002F:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 0C 72 65 61 64 65 72 51 75 6F 74 61 73 00   // ...readerQuotas.
                                                                                                                                                                                    00 ) 
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.XmlDictionaryReaderQuotasElement/*01000009*/ System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_ReaderQuotas() /* 0600002C */
  } // end of property UdpBindingElement::ReaderQuotas
  .property /*17000014*/ instance callconv(8) class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/
          TextEncoding()
  {
    .custom /*0C000030:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 0C 74 65 78 74 45 6E 63 6F 64 69 6E 67 01   // ...textEncoding.
                                                                                                                                                                                    00 54 51 0C 44 65 66 61 75 6C 74 56 61 6C 75 65   // .TQ.DefaultValue
                                                                                                                                                                                    0E 05 75 74 66 2D 38 )                            // ..utf-8
    .custom /*0C000031:0A000025*/ instance void [System/*23000005*/]System.ComponentModel.TypeConverterAttribute/*01000083*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000008*/) /* 0A000025 */ = ( 01 00 80 8B 53 79 73 74 65 6D 2E 53 65 72 76 69   // ....System.Servi
                                                                                                                                                                                                            63 65 4D 6F 64 65 6C 2E 43 6F 6E 66 69 67 75 72   // ceModel.Configur
                                                                                                                                                                                                            61 74 69 6F 6E 2E 45 6E 63 6F 64 69 6E 67 43 6F   // ation.EncodingCo
                                                                                                                                                                                                            6E 76 65 72 74 65 72 2C 20 53 79 73 74 65 6D 2E   // nverter, System.
                                                                                                                                                                                                            53 65 72 76 69 63 65 4D 6F 64 65 6C 2C 20 56 65   // ServiceModel, Ve
                                                                                                                                                                                                            72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C 20 43   // rsion=4.0.0.0, C
                                                                                                                                                                                                            75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20   // ulture=neutral, 
                                                                                                                                                                                                            50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62   // PublicKeyToken=b
                                                                                                                                                                                                            37 37 61 35 63 35 36 31 39 33 34 65 30 38 39 00   // 77a5c561934e089.
                                                                                                                                                                                                            00 ) 
    .get instance class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/ System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_TextEncoding() /* 0600002D */
    .set instance void System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::set_TextEncoding(class [mscorlib/*23000001*/]System.Text.Encoding/*01000004*/) /* 0600002E */
  } // end of property UdpBindingElement::TextEncoding
  .property /*17000015*/ instance callconv(8) int32
          TimeToLive()
  {
    .custom /*0C000032:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 0A 74 69 6D 65 54 6F 4C 69 76 65 01 00 54   // ...timeToLive..T
                                                                                                                                                                                    51 0C 44 65 66 61 75 6C 74 56 61 6C 75 65 08 01   // Q.DefaultValue..
                                                                                                                                                                                    00 00 00 ) 
    .custom /*0C000033:0A000027*/ instance void [System.Configuration/*23000006*/]System.Configuration.IntegerValidatorAttribute/*01000085*/::.ctor() /* 0A000027 */ = ( 01 00 02 00 54 08 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                         00 00 00 54 08 08 4D 61 78 56 61 6C 75 65 FF 00   // ...T..MaxValue..
                                                                                                                                                                         00 00 ) 
    .get instance int32 System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_TimeToLive() /* 0600002F */
    .set instance void System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::set_TimeToLive(int32) /* 06000030 */
  } // end of property UdpBindingElement::TimeToLive
  .property /*17000016*/ instance callconv(8) class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/
          Properties()
  {
    .get instance class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/ System.ServiceModel.Configuration.UdpBindingElement/*02000004*/::get_Properties() /* 06000033 */
  } // end of property UdpBindingElement::Properties
} // end of class System.ServiceModel.Configuration.UdpBindingElement

.class /*02000005*/ public auto ansi sealed beforefieldinit System.ServiceModel.Configuration.ByteStreamMessageEncodingElement
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.BindingElementExtensionElement/*0100000B*/
{
  .method /*06000034*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2083
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingElement::.ctor

  .method /*06000035*/ public hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000008*/ 
          get_BindingElementType() cil managed
  // SIG: 20 00 12 21
  {
    // Method begins at RVA 0x2084
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingElement::get_BindingElementType

  .method /*06000036*/ public hidebysig specialname 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.XmlDictionaryReaderQuotasElement/*01000009*/ 
          get_ReaderQuotas() cil managed
  // SIG: 20 00 12 25
  {
    // Method begins at RVA 0x2085
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingElement::get_ReaderQuotas

  .method /*06000037*/ public hidebysig virtual 
          instance void  ApplyConfiguration(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElement/*0100000C*/ bindingElement) cil managed
  // SIG: 20 01 01 12 31
  {
    // Method begins at RVA 0x2086
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingElement::ApplyConfiguration

  .method /*06000038*/ public hidebysig virtual 
          instance void  CopyFrom(class [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.ServiceModelExtensionElement/*0100000D*/ from) cil managed
  // SIG: 20 01 01 12 35
  {
    // Method begins at RVA 0x2087
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingElement::CopyFrom

  .method /*06000039*/ famorassem hidebysig virtual 
          instance void  InitializeFrom(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElement/*0100000C*/ bindingElement) cil managed
  // SIG: 20 01 01 12 31
  {
    // Method begins at RVA 0x2088
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingElement::InitializeFrom

  .method /*0600003A*/ famorassem hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElement/*0100000C*/ 
          CreateBindingElement() cil managed
  // SIG: 20 00 12 31
  {
    // Method begins at RVA 0x2089
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingElement::CreateBindingElement

  .method /*0600003B*/ family hidebysig specialname virtual 
          instance class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 29
  {
    // Method begins at RVA 0x208a
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingElement::get_Properties

  .property /*17000017*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000008*/
          BindingElementType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000008*/ System.ServiceModel.Configuration.ByteStreamMessageEncodingElement/*02000005*/::get_BindingElementType() /* 06000035 */
  } // end of property ByteStreamMessageEncodingElement::BindingElementType
  .property /*17000018*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.XmlDictionaryReaderQuotasElement/*01000009*/
          ReaderQuotas()
  {
    .custom /*0C000035:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 0C 72 65 61 64 65 72 51 75 6F 74 61 73 00   // ...readerQuotas.
                                                                                                                                                                                    00 ) 
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.XmlDictionaryReaderQuotasElement/*01000009*/ System.ServiceModel.Configuration.ByteStreamMessageEncodingElement/*02000005*/::get_ReaderQuotas() /* 06000036 */
  } // end of property ByteStreamMessageEncodingElement::ReaderQuotas
  .property /*17000019*/ instance callconv(8) class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/
          Properties()
  {
    .get instance class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/ System.ServiceModel.Configuration.ByteStreamMessageEncodingElement/*02000005*/::get_Properties() /* 0600003B */
  } // end of property ByteStreamMessageEncodingElement::Properties
} // end of class System.ServiceModel.Configuration.ByteStreamMessageEncodingElement

.class /*02000006*/ public auto ansi sealed beforefieldinit System.ServiceModel.Configuration.UdpTransportElement
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.TransportElement/*0100000E*/
{
  .method /*0600003C*/ family hidebysig specialname virtual 
          instance class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 29
  {
    // Method begins at RVA 0x208b
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::get_Properties

  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x208c
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::.ctor

  .method /*0600003E*/ public hidebysig virtual 
          instance void  ApplyConfiguration(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElement/*0100000C*/ bindingElement) cil managed
  // SIG: 20 01 01 12 31
  {
    // Method begins at RVA 0x208d
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::ApplyConfiguration

  .method /*0600003F*/ famorassem hidebysig virtual 
          instance void  InitializeFrom(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElement/*0100000C*/ bindingElement) cil managed
  // SIG: 20 01 01 12 31
  {
    // Method begins at RVA 0x208e
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::InitializeFrom

  .method /*06000040*/ public hidebysig specialname virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000008*/ 
          get_BindingElementType() cil managed
  // SIG: 20 00 12 21
  {
    // Method begins at RVA 0x208f
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::get_BindingElementType

  .method /*06000041*/ family hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.TransportBindingElement/*0100000F*/ 
          CreateDefaultBindingElement() cil managed
  // SIG: 20 00 12 3D
  {
    // Method begins at RVA 0x2090
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::CreateDefaultBindingElement

  .method /*06000042*/ public hidebysig specialname 
          instance int32  get_DuplicateMessageHistoryLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2091
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::get_DuplicateMessageHistoryLength

  .method /*06000043*/ public hidebysig specialname 
          instance void  set_DuplicateMessageHistoryLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2092
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::set_DuplicateMessageHistoryLength

  .method /*06000044*/ public hidebysig specialname 
          instance int64  get_MaxPendingMessagesTotalSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2093
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::get_MaxPendingMessagesTotalSize

  .method /*06000045*/ public hidebysig specialname 
          instance void  set_MaxPendingMessagesTotalSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2094
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::set_MaxPendingMessagesTotalSize

  .method /*06000046*/ public hidebysig specialname 
          instance string  get_MulticastInterfaceId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2095
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::get_MulticastInterfaceId

  .method /*06000047*/ public hidebysig specialname 
          instance void  set_MulticastInterfaceId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2096
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::set_MulticastInterfaceId

  .method /*06000048*/ public hidebysig specialname 
          instance int32  get_SocketReceiveBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2097
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::get_SocketReceiveBufferSize

  .method /*06000049*/ public hidebysig specialname 
          instance void  set_SocketReceiveBufferSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2098
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::set_SocketReceiveBufferSize

  .method /*0600004A*/ public hidebysig specialname 
          instance int32  get_TimeToLive() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2099
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::get_TimeToLive

  .method /*0600004B*/ public hidebysig specialname 
          instance void  set_TimeToLive(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x209a
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::set_TimeToLive

  .method /*0600004C*/ public hidebysig specialname 
          instance class System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/ 
          get_RetransmissionSettings() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x209b
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::get_RetransmissionSettings

  .method /*0600004D*/ public hidebysig specialname 
          instance void  set_RetransmissionSettings(class System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1C
  {
    // Method begins at RVA 0x209c
    // Code size       0 (0x0)
  } // end of method UdpTransportElement::set_RetransmissionSettings

  .property /*1700001A*/ instance callconv(8) class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/
          Properties()
  {
    .get instance class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/ System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::get_Properties() /* 0600003C */
  } // end of property UdpTransportElement::Properties
  .property /*1700001B*/ instance callconv(8) class [mscorlib/*23000001*/]System.Type/*01000008*/
          BindingElementType()
  {
    .get instance class [mscorlib/*23000001*/]System.Type/*01000008*/ System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::get_BindingElementType() /* 06000040 */
  } // end of property UdpTransportElement::BindingElementType
  .property /*1700001C*/ instance callconv(8) int32
          DuplicateMessageHistoryLength()
  {
    .custom /*0C000037:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 1D 64 75 70 6C 69 63 61 74 65 4D 65 73 73   // ...duplicateMess
                                                                                                                                                                                    61 67 65 48 69 73 74 6F 72 79 4C 65 6E 67 74 68   // ageHistoryLength
                                                                                                                                                                                    01 00 54 51 0C 44 65 66 61 75 6C 74 56 61 6C 75   // ..TQ.DefaultValu
                                                                                                                                                                                    65 08 00 00 00 00 )                               // e.....
    .custom /*0C000038:0A000027*/ instance void [System.Configuration/*23000006*/]System.Configuration.IntegerValidatorAttribute/*01000085*/::.ctor() /* 0A000027 */ = ( 01 00 01 00 54 08 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                         00 00 00 ) 
    .get instance int32 System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::get_DuplicateMessageHistoryLength() /* 06000042 */
    .set instance void System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::set_DuplicateMessageHistoryLength(int32) /* 06000043 */
  } // end of property UdpTransportElement::DuplicateMessageHistoryLength
  .property /*1700001D*/ instance callconv(8) int64
          MaxPendingMessagesTotalSize()
  {
    .custom /*0C000039:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 1B 6D 61 78 50 65 6E 64 69 6E 67 4D 65 73   // ...maxPendingMes
                                                                                                                                                                                    73 61 67 65 73 54 6F 74 61 6C 53 69 7A 65 01 00   // sagesTotalSize..
                                                                                                                                                                                    54 51 0C 44 65 66 61 75 6C 74 56 61 6C 75 65 0A   // TQ.DefaultValue.
                                                                                                                                                                                    00 00 00 00 00 00 00 00 ) 
    .custom /*0C00003A:0A000028*/ instance void [System.Configuration/*23000006*/]System.Configuration.LongValidatorAttribute/*01000086*/::.ctor() /* 0A000028 */ = ( 01 00 01 00 54 0A 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                      00 00 00 00 00 00 00 ) 
    .get instance int64 System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::get_MaxPendingMessagesTotalSize() /* 06000044 */
    .set instance void System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::set_MaxPendingMessagesTotalSize(int64) /* 06000045 */
  } // end of property UdpTransportElement::MaxPendingMessagesTotalSize
  .property /*1700001E*/ instance callconv(8) string
          MulticastInterfaceId()
  {
    .custom /*0C00003B:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 14 6D 75 6C 74 69 63 61 73 74 49 6E 74 65   // ...multicastInte
                                                                                                                                                                                    72 66 61 63 65 49 64 01 00 54 51 0C 44 65 66 61   // rfaceId..TQ.Defa
                                                                                                                                                                                    75 6C 74 56 61 6C 75 65 0E FF )                   // ultValue..
    .custom /*0C00003C:0A000029*/ instance void [System.Configuration/*23000006*/]System.Configuration.StringValidatorAttribute/*01000087*/::.ctor() /* 0A000029 */ = ( 01 00 00 00 ) 
    .get instance string System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::get_MulticastInterfaceId() /* 06000046 */
    .set instance void System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::set_MulticastInterfaceId(string) /* 06000047 */
  } // end of property UdpTransportElement::MulticastInterfaceId
  .property /*1700001F*/ instance callconv(8) int32
          SocketReceiveBufferSize()
  {
    .custom /*0C00003D:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 17 73 6F 63 6B 65 74 52 65 63 65 69 76 65   // ...socketReceive
                                                                                                                                                                                    42 75 66 66 65 72 53 69 7A 65 01 00 54 51 0C 44   // BufferSize..TQ.D
                                                                                                                                                                                    65 66 61 75 6C 74 56 61 6C 75 65 08 00 00 01 00 ) // efaultValue.....
    .custom /*0C00003E:0A000027*/ instance void [System.Configuration/*23000006*/]System.Configuration.IntegerValidatorAttribute/*01000085*/::.ctor() /* 0A000027 */ = ( 01 00 01 00 54 08 08 4D 69 6E 56 61 6C 75 65 01   // ....T..MinValue.
                                                                                                                                                                         00 00 00 ) 
    .get instance int32 System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::get_SocketReceiveBufferSize() /* 06000048 */
    .set instance void System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::set_SocketReceiveBufferSize(int32) /* 06000049 */
  } // end of property UdpTransportElement::SocketReceiveBufferSize
  .property /*17000020*/ instance callconv(8) int32
          TimeToLive()
  {
    .custom /*0C00003F:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 0A 74 69 6D 65 54 6F 4C 69 76 65 01 00 54   // ...timeToLive..T
                                                                                                                                                                                    51 0C 44 65 66 61 75 6C 74 56 61 6C 75 65 08 01   // Q.DefaultValue..
                                                                                                                                                                                    00 00 00 ) 
    .custom /*0C000040:0A000027*/ instance void [System.Configuration/*23000006*/]System.Configuration.IntegerValidatorAttribute/*01000085*/::.ctor() /* 0A000027 */ = ( 01 00 02 00 54 08 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                         00 00 00 54 08 08 4D 61 78 56 61 6C 75 65 FF 00   // ...T..MaxValue..
                                                                                                                                                                         00 00 ) 
    .get instance int32 System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::get_TimeToLive() /* 0600004A */
    .set instance void System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::set_TimeToLive(int32) /* 0600004B */
  } // end of property UdpTransportElement::TimeToLive
  .property /*17000021*/ instance callconv(8) class System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/
          RetransmissionSettings()
  {
    .custom /*0C000041:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 16 72 65 74 72 61 6E 73 6D 69 73 73 69 6F   // ...retransmissio
                                                                                                                                                                                    6E 53 65 74 74 69 6E 67 73 00 00 )                // nSettings..
    .get instance class System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/ System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::get_RetransmissionSettings() /* 0600004C */
    .set instance void System.ServiceModel.Configuration.UdpTransportElement/*02000006*/::set_RetransmissionSettings(class System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/) /* 0600004D */
  } // end of property UdpTransportElement::RetransmissionSettings
} // end of class System.ServiceModel.Configuration.UdpTransportElement

.class /*02000007*/ public auto ansi sealed beforefieldinit System.ServiceModel.Configuration.UdpRetransmissionSettingsElement
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.ServiceModelConfigurationElement/*01000010*/
{
  .method /*0600004E*/ family hidebysig specialname virtual 
          instance class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 29
  {
    // Method begins at RVA 0x209d
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::get_Properties

  .method /*0600004F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209e
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::.ctor

  .method /*06000050*/ assembly hidebysig 
          instance void  ApplyConfiguration(class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ udpRetransmissionSettings) cil managed
  // SIG: 20 01 01 12 7C
  {
    // Method begins at RVA 0x209f
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::ApplyConfiguration

  .method /*06000051*/ assembly hidebysig 
          instance void  InitializeFrom(class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ udpRetransmissionSettings) cil managed
  // SIG: 20 01 01 12 7C
  {
    // Method begins at RVA 0x20a0
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::InitializeFrom

  .method /*06000052*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_DelayLowerBound() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x20a1
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::get_DelayLowerBound

  .method /*06000053*/ public hidebysig specialname 
          instance void  set_DelayLowerBound(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 'value') cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x20a2
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::set_DelayLowerBound

  .method /*06000054*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_DelayUpperBound() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x20a3
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::get_DelayUpperBound

  .method /*06000055*/ public hidebysig specialname 
          instance void  set_DelayUpperBound(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 'value') cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x20a4
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::set_DelayUpperBound

  .method /*06000056*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_MaxDelayPerRetransmission() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x20a5
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::get_MaxDelayPerRetransmission

  .method /*06000057*/ public hidebysig specialname 
          instance void  set_MaxDelayPerRetransmission(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 'value') cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x20a6
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::set_MaxDelayPerRetransmission

  .method /*06000058*/ public hidebysig specialname 
          instance int32  get_MaxMulticastRetransmitCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20a7
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::get_MaxMulticastRetransmitCount

  .method /*06000059*/ public hidebysig specialname 
          instance void  set_MaxMulticastRetransmitCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20a8
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::set_MaxMulticastRetransmitCount

  .method /*0600005A*/ public hidebysig specialname 
          instance int32  get_MaxUnicastRetransmitCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20a9
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::get_MaxUnicastRetransmitCount

  .method /*0600005B*/ public hidebysig specialname 
          instance void  set_MaxUnicastRetransmitCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20aa
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettingsElement::set_MaxUnicastRetransmitCount

  .property /*17000022*/ instance callconv(8) class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/
          Properties()
  {
    .get instance class [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyCollection/*0100000A*/ System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::get_Properties() /* 0600004E */
  } // end of property UdpRetransmissionSettingsElement::Properties
  .property /*17000023*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          DelayLowerBound()
  {
    .custom /*0C000042:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 0F 64 65 6C 61 79 4C 6F 77 65 72 42 6F 75   // ...delayLowerBou
                                                                                                                                                                                    6E 64 01 00 54 51 0C 44 65 66 61 75 6C 74 56 61   // nd..TQ.DefaultVa
                                                                                                                                                                                    6C 75 65 0E 0C 30 30 3A 30 30 3A 30 30 2E 30 35   // lue..00:00:00.05
                                                                                                                                                                                    30 )                                              // 0
    .custom /*0C000043:0A000025*/ instance void [System/*23000005*/]System.ComponentModel.TypeConverterAttribute/*01000083*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000008*/) /* 0A000025 */ = ( 01 00 80 95 53 79 73 74 65 6D 2E 53 65 72 76 69   // ....System.Servi
                                                                                                                                                                                                            63 65 4D 6F 64 65 6C 2E 43 6F 6E 66 69 67 75 72   // ceModel.Configur
                                                                                                                                                                                                            61 74 69 6F 6E 2E 54 69 6D 65 53 70 61 6E 4F 72   // ation.TimeSpanOr
                                                                                                                                                                                                            49 6E 66 69 6E 69 74 65 43 6F 6E 76 65 72 74 65   // InfiniteConverte
                                                                                                                                                                                                            72 2C 20 53 79 73 74 65 6D 2E 53 65 72 76 69 63   // r, System.Servic
                                                                                                                                                                                                            65 4D 6F 64 65 6C 2C 20 56 65 72 73 69 6F 6E 3D   // eModel, Version=
                                                                                                                                                                                                            34 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65   // 4.0.0.0, Culture
                                                                                                                                                                                                            3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63   // =neutral, Public
                                                                                                                                                                                                            4B 65 79 54 6F 6B 65 6E 3D 62 37 37 61 35 63 35   // KeyToken=b77a5c5
                                                                                                                                                                                                            36 31 39 33 34 65 30 38 39 00 00 )                // 61934e089..
    .custom /*0C000044:0A00002A*/ instance void [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.ServiceModelTimeSpanValidatorAttribute/*01000088*/::.ctor() /* 0A00002A */ = ( 01 00 01 00 54 0E 0E 4D 69 6E 56 61 6C 75 65 53   // ....T..MinValueS
                                                                                                                                                                                                  74 72 69 6E 67 08 30 30 3A 30 30 3A 30 30 )       // tring.00:00:00
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::get_DelayLowerBound() /* 06000052 */
    .set instance void System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::set_DelayLowerBound(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/) /* 06000053 */
  } // end of property UdpRetransmissionSettingsElement::DelayLowerBound
  .property /*17000024*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          DelayUpperBound()
  {
    .custom /*0C000045:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 0F 64 65 6C 61 79 55 70 70 65 72 42 6F 75   // ...delayUpperBou
                                                                                                                                                                                    6E 64 01 00 54 51 0C 44 65 66 61 75 6C 74 56 61   // nd..TQ.DefaultVa
                                                                                                                                                                                    6C 75 65 0E 0C 30 30 3A 30 30 3A 30 30 2E 32 35   // lue..00:00:00.25
                                                                                                                                                                                    30 )                                              // 0
    .custom /*0C000046:0A000025*/ instance void [System/*23000005*/]System.ComponentModel.TypeConverterAttribute/*01000083*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000008*/) /* 0A000025 */ = ( 01 00 80 95 53 79 73 74 65 6D 2E 53 65 72 76 69   // ....System.Servi
                                                                                                                                                                                                            63 65 4D 6F 64 65 6C 2E 43 6F 6E 66 69 67 75 72   // ceModel.Configur
                                                                                                                                                                                                            61 74 69 6F 6E 2E 54 69 6D 65 53 70 61 6E 4F 72   // ation.TimeSpanOr
                                                                                                                                                                                                            49 6E 66 69 6E 69 74 65 43 6F 6E 76 65 72 74 65   // InfiniteConverte
                                                                                                                                                                                                            72 2C 20 53 79 73 74 65 6D 2E 53 65 72 76 69 63   // r, System.Servic
                                                                                                                                                                                                            65 4D 6F 64 65 6C 2C 20 56 65 72 73 69 6F 6E 3D   // eModel, Version=
                                                                                                                                                                                                            34 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65   // 4.0.0.0, Culture
                                                                                                                                                                                                            3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63   // =neutral, Public
                                                                                                                                                                                                            4B 65 79 54 6F 6B 65 6E 3D 62 37 37 61 35 63 35   // KeyToken=b77a5c5
                                                                                                                                                                                                            36 31 39 33 34 65 30 38 39 00 00 )                // 61934e089..
    .custom /*0C000047:0A00002A*/ instance void [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.ServiceModelTimeSpanValidatorAttribute/*01000088*/::.ctor() /* 0A00002A */ = ( 01 00 01 00 54 0E 0E 4D 69 6E 56 61 6C 75 65 53   // ....T..MinValueS
                                                                                                                                                                                                  74 72 69 6E 67 08 30 30 3A 30 30 3A 30 30 )       // tring.00:00:00
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::get_DelayUpperBound() /* 06000054 */
    .set instance void System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::set_DelayUpperBound(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/) /* 06000055 */
  } // end of property UdpRetransmissionSettingsElement::DelayUpperBound
  .property /*17000025*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          MaxDelayPerRetransmission()
  {
    .custom /*0C000048:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 19 6D 61 78 44 65 6C 61 79 50 65 72 52 65   // ...maxDelayPerRe
                                                                                                                                                                                    74 72 61 6E 73 6D 69 73 73 69 6F 6E 01 00 54 51   // transmission..TQ
                                                                                                                                                                                    0C 44 65 66 61 75 6C 74 56 61 6C 75 65 0E 0C 30   // .DefaultValue..0
                                                                                                                                                                                    30 3A 30 30 3A 30 30 2E 35 30 30 )                // 0:00:00.500
    .custom /*0C000049:0A000025*/ instance void [System/*23000005*/]System.ComponentModel.TypeConverterAttribute/*01000083*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000008*/) /* 0A000025 */ = ( 01 00 80 95 53 79 73 74 65 6D 2E 53 65 72 76 69   // ....System.Servi
                                                                                                                                                                                                            63 65 4D 6F 64 65 6C 2E 43 6F 6E 66 69 67 75 72   // ceModel.Configur
                                                                                                                                                                                                            61 74 69 6F 6E 2E 54 69 6D 65 53 70 61 6E 4F 72   // ation.TimeSpanOr
                                                                                                                                                                                                            49 6E 66 69 6E 69 74 65 43 6F 6E 76 65 72 74 65   // InfiniteConverte
                                                                                                                                                                                                            72 2C 20 53 79 73 74 65 6D 2E 53 65 72 76 69 63   // r, System.Servic
                                                                                                                                                                                                            65 4D 6F 64 65 6C 2C 20 56 65 72 73 69 6F 6E 3D   // eModel, Version=
                                                                                                                                                                                                            34 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65   // 4.0.0.0, Culture
                                                                                                                                                                                                            3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63   // =neutral, Public
                                                                                                                                                                                                            4B 65 79 54 6F 6B 65 6E 3D 62 37 37 61 35 63 35   // KeyToken=b77a5c5
                                                                                                                                                                                                            36 31 39 33 34 65 30 38 39 00 00 )                // 61934e089..
    .custom /*0C00004A:0A00002A*/ instance void [System.ServiceModel/*23000003*/]System.ServiceModel.Configuration.ServiceModelTimeSpanValidatorAttribute/*01000088*/::.ctor() /* 0A00002A */ = ( 01 00 01 00 54 0E 0E 4D 69 6E 56 61 6C 75 65 53   // ....T..MinValueS
                                                                                                                                                                                                  74 72 69 6E 67 08 30 30 3A 30 30 3A 30 30 )       // tring.00:00:00
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::get_MaxDelayPerRetransmission() /* 06000056 */
    .set instance void System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::set_MaxDelayPerRetransmission(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/) /* 06000057 */
  } // end of property UdpRetransmissionSettingsElement::MaxDelayPerRetransmission
  .property /*17000026*/ instance callconv(8) int32
          MaxMulticastRetransmitCount()
  {
    .custom /*0C00004B:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 1B 6D 61 78 4D 75 6C 74 69 63 61 73 74 52   // ...maxMulticastR
                                                                                                                                                                                    65 74 72 61 6E 73 6D 69 74 43 6F 75 6E 74 01 00   // etransmitCount..
                                                                                                                                                                                    54 51 0C 44 65 66 61 75 6C 74 56 61 6C 75 65 08   // TQ.DefaultValue.
                                                                                                                                                                                    00 00 00 00 ) 
    .custom /*0C00004C:0A000027*/ instance void [System.Configuration/*23000006*/]System.Configuration.IntegerValidatorAttribute/*01000085*/::.ctor() /* 0A000027 */ = ( 01 00 01 00 54 08 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                         00 00 00 ) 
    .get instance int32 System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::get_MaxMulticastRetransmitCount() /* 06000058 */
    .set instance void System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::set_MaxMulticastRetransmitCount(int32) /* 06000059 */
  } // end of property UdpRetransmissionSettingsElement::MaxMulticastRetransmitCount
  .property /*17000027*/ instance callconv(8) int32
          MaxUnicastRetransmitCount()
  {
    .custom /*0C00004D:0A000026*/ instance void [System.Configuration/*23000006*/]System.Configuration.ConfigurationPropertyAttribute/*01000084*/::.ctor(string) /* 0A000026 */ = ( 01 00 19 6D 61 78 55 6E 69 63 61 73 74 52 65 74   // ...maxUnicastRet
                                                                                                                                                                                    72 61 6E 73 6D 69 74 43 6F 75 6E 74 01 00 54 51   // ransmitCount..TQ
                                                                                                                                                                                    0C 44 65 66 61 75 6C 74 56 61 6C 75 65 08 00 00   // .DefaultValue...
                                                                                                                                                                                    00 00 ) 
    .custom /*0C00004E:0A000027*/ instance void [System.Configuration/*23000006*/]System.Configuration.IntegerValidatorAttribute/*01000085*/::.ctor() /* 0A000027 */ = ( 01 00 01 00 54 08 08 4D 69 6E 56 61 6C 75 65 00   // ....T..MinValue.
                                                                                                                                                                         00 00 00 ) 
    .get instance int32 System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::get_MaxUnicastRetransmitCount() /* 0600005A */
    .set instance void System.ServiceModel.Configuration.UdpRetransmissionSettingsElement/*02000007*/::set_MaxUnicastRetransmitCount(int32) /* 0600005B */
  } // end of property UdpRetransmissionSettingsElement::MaxUnicastRetransmitCount
} // end of class System.ServiceModel.Configuration.UdpRetransmissionSettingsElement

.class /*02000008*/ private abstract auto ansi sealed beforefieldinit System.ServiceModel.Configuration.UdpTransportConfigurationStrings
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .field /*04000001*/ static assembly literal string DuplicateMessageHistoryLength = "duplicateMessageHistoryLength"
  .field /*04000002*/ static assembly literal string MaxPendingMessagesTotalSize = "maxPendingMessagesTotalSize"
  .field /*04000003*/ static assembly literal string MaxReceivedMessageSize = "maxReceivedMessageSize"
  .field /*04000004*/ static assembly literal string MaxBufferPoolSize = "maxBufferPoolSize"
  .field /*04000005*/ static assembly literal string MulticastInterfaceId = "multicastInterfaceId"
  .field /*04000006*/ static assembly literal string SocketReceiveBufferSize = "socketReceiveBufferSize"
  .field /*04000007*/ static assembly literal string TimeToLive = "timeToLive"
  .field /*04000008*/ static assembly literal string RetransmissionSettings = "retransmissionSettings"
  .field /*04000009*/ static assembly literal string DelayLowerBound = "delayLowerBound"
  .field /*0400000A*/ static assembly literal string DelayUpperBound = "delayUpperBound"
  .field /*0400000B*/ static assembly literal string MaxDelayPerRetransmission = "maxDelayPerRetransmission"
  .field /*0400000C*/ static assembly literal string MaxMulticastRetransmitCount = "maxMulticastRetransmitCount"
  .field /*0400000D*/ static assembly literal string MaxUnicastRetransmitCount = "maxUnicastRetransmitCount"
  .field /*0400000E*/ static assembly literal string UdpBindingElementName = "udpBinding"
  .field /*0400000F*/ static assembly literal string MaxRetransmitCount = "maxRetransmitCount"
  .field /*04000010*/ static assembly literal string ReaderQuotas = "readerQuotas"
  .field /*04000011*/ static assembly literal string TextEncoding = "textEncoding"
} // end of class System.ServiceModel.Configuration.UdpTransportConfigurationStrings

.class /*02000009*/ private auto ansi beforefieldinit System.ServiceModel.Channels.ByteStreamBufferedMessageData
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*0600005C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> buffer) cil managed
  // SIG: 20 01 01 15 11 4D 01 05
  {
    // Method begins at RVA 0x20ab
    // Code size       0 (0x0)
  } // end of method ByteStreamBufferedMessageData::.ctor

  .method /*0600005D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> buffer,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager) cil managed
  // SIG: 20 02 01 15 11 4D 01 05 12 51
  {
    // Method begins at RVA 0x20ac
    // Code size       0 (0x0)
  } // end of method ByteStreamBufferedMessageData::.ctor

  .method /*0600005E*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> 
          get_Buffer() cil managed
  // SIG: 20 00 15 11 4D 01 05
  {
    // Method begins at RVA 0x20ad
    // Code size       0 (0x0)
  } // end of method ByteStreamBufferedMessageData::get_Buffer

  .method /*0600005F*/ public hidebysig instance void 
          Open() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ae
    // Code size       0 (0x0)
  } // end of method ByteStreamBufferedMessageData::Open

  .method /*06000060*/ public hidebysig instance void 
          Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20af
    // Code size       0 (0x0)
  } // end of method ByteStreamBufferedMessageData::Close

  .method /*06000061*/ public hidebysig instance class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 
          ToStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x20b0
    // Code size       0 (0x0)
  } // end of method ByteStreamBufferedMessageData::ToStream

  .property /*17000028*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8>
          Buffer()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> System.ServiceModel.Channels.ByteStreamBufferedMessageData/*02000009*/::get_Buffer() /* 0600005E */
  } // end of property ByteStreamBufferedMessageData::Buffer
} // end of class System.ServiceModel.Channels.ByteStreamBufferedMessageData

.class /*0200000A*/ public abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.ByteStreamMessage
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*06000062*/ public hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          CreateMessage(class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 00 01 12 59 12 55
  {
    // Method begins at RVA 0x20b1
    // Code size       0 (0x0)
  } // end of method ByteStreamMessage::CreateMessage

  .method /*06000063*/ public hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          CreateMessage(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> buffer) cil managed
  // SIG: 00 01 12 59 15 11 4D 01 05
  {
    // Method begins at RVA 0x20b2
    // Code size       0 (0x0)
  } // end of method ByteStreamMessage::CreateMessage

  .method /*06000064*/ public hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          CreateMessage(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> buffer,
                        class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager) cil managed
  // SIG: 00 02 12 59 15 11 4D 01 05 12 51
  {
    // Method begins at RVA 0x20b3
    // Code size       0 (0x0)
  } // end of method ByteStreamMessage::CreateMessage

  .method /*06000065*/ assembly hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          CreateMessage(class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                        class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas,
                        bool moveBodyReaderToContent) cil managed
  // SIG: 00 03 12 59 12 55 12 0D 02
  {
    // Method begins at RVA 0x20b4
    // Code size       0 (0x0)
  } // end of method ByteStreamMessage::CreateMessage

  .method /*06000066*/ assembly hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          CreateMessage(class [System.Net.Http/*23000007*/]System.Net.Http.HttpRequestMessage/*01000017*/ httpRequestMessage,
                        class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 00 02 12 59 12 5D 12 0D
  {
    // Method begins at RVA 0x20b5
    // Code size       0 (0x0)
  } // end of method ByteStreamMessage::CreateMessage

  .method /*06000067*/ assembly hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          CreateMessage(class [System.Net.Http/*23000007*/]System.Net.Http.HttpResponseMessage/*01000018*/ httpResponseMessage,
                        class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 00 02 12 59 12 61 12 0D
  {
    // Method begins at RVA 0x20b6
    // Code size       0 (0x0)
  } // end of method ByteStreamMessage::CreateMessage

  .method /*06000068*/ assembly hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          CreateMessage(class System.ServiceModel.Channels.ByteStreamBufferedMessageData/*02000009*/ bufferedMessageData,
                        class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas,
                        bool moveBodyReaderToContent) cil managed
  // SIG: 00 03 12 59 12 24 12 0D 02
  {
    // Method begins at RVA 0x20b7
    // Code size       0 (0x0)
  } // end of method ByteStreamMessage::CreateMessage

  .method /*06000069*/ assembly hidebysig static 
          bool  IsInternalByteStreamMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 00 01 02 12 59
  {
    // Method begins at RVA 0x20b8
    // Code size       0 (0x0)
  } // end of method ByteStreamMessage::IsInternalByteStreamMessage

} // end of class System.ServiceModel.Channels.ByteStreamMessage

.class /*0200000B*/ private auto ansi beforefieldinit System.ServiceModel.Channels.ByteStreamMessageEncoder
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/
       implements [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IStreamedMessageEncoder/*0100001C*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IWebMessageEncoderHelper/*0100001A*/,
                  [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.ITraceSourceStringProvider/*0100001B*/
{
  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20b9
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::.ctor

  .method /*0600006B*/ private hidebysig newslot virtual final 
          instance void  System.ServiceModel.Channels.IWebMessageEncoderHelper.EnableBodyReaderMoveToContent() cil managed
  // SIG: 20 00 01
  {
    .override [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IWebMessageEncoderHelper/*0100001A*/::EnableBodyReaderMoveToContent /*0100001A::0A000002*/ 
    // Method begins at RVA 0x20ba
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::System.ServiceModel.Channels.IWebMessageEncoderHelper.EnableBodyReaderMoveToContent

  .method /*0600006C*/ public hidebysig specialname virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20bb
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::get_ContentType

  .method /*0600006D*/ public hidebysig specialname virtual 
          instance string  get_MediaType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20bc
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::get_MediaType

  .method /*0600006E*/ public hidebysig specialname virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/ 
          get_MessageVersion() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x20bd
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::get_MessageVersion

  .method /*0600006F*/ public hidebysig virtual 
          instance bool  IsContentTypeSupported(string contentType) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x20be
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::IsContentTypeSupported

  .method /*06000070*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          ReadMessage(class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                      int32 maxSizeOfHeaders,
                      string contentType) cil managed
  // SIG: 20 03 12 59 12 55 08 0E
  {
    // Method begins at RVA 0x20bf
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::ReadMessage

  .method /*06000071*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          ReadMessage(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> buffer,
                      class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                      string contentType) cil managed
  // SIG: 20 03 12 59 15 11 4D 01 05 12 51 0E
  {
    // Method begins at RVA 0x20c0
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::ReadMessage

  .method /*06000072*/ public hidebysig virtual 
          instance void  WriteMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                      class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 'stream') cil managed
  // SIG: 20 02 01 12 59 12 55
  {
    // Method begins at RVA 0x20c1
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::WriteMessage

  .method /*06000073*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginWriteMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                            class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                            class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                            object state) cil managed
  // SIG: 20 04 12 79 12 59 12 55 12 7D 1C
  {
    // Method begins at RVA 0x20c2
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::BeginWriteMessage

  .method /*06000074*/ public hidebysig virtual 
          instance void  EndWriteMessage(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x20c3
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::EndWriteMessage

  .method /*06000075*/ public hidebysig virtual 
          instance valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> 
          WriteMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                       int32 maxMessageSize,
                       class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                       int32 messageOffset) cil managed
  // SIG: 20 04 15 11 4D 01 05 12 59 08 12 51 08
  {
    // Method begins at RVA 0x20c4
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::WriteMessage

  .method /*06000076*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20c5
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::ToString

  .method /*06000077*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 
          GetResponseMessageStream(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 20 01 12 55 12 59
  {
    // Method begins at RVA 0x20c6
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::GetResponseMessageStream

  .method /*06000078*/ private hidebysig newslot virtual final 
          instance string  System.Runtime.Diagnostics.ITraceSourceStringProvider.GetSourceString() cil managed
  // SIG: 20 00 0E
  {
    .override [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.ITraceSourceStringProvider/*0100001B*/::GetSourceString /*0100001B::0A000003*/ 
    // Method begins at RVA 0x20c7
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoder::System.Runtime.Diagnostics.ITraceSourceStringProvider.GetSourceString

  .property /*17000029*/ instance callconv(8) string
          ContentType()
  {
    .get instance string System.ServiceModel.Channels.ByteStreamMessageEncoder/*0200000B*/::get_ContentType() /* 0600006C */
  } // end of property ByteStreamMessageEncoder::ContentType
  .property /*1700002A*/ instance callconv(8) string
          MediaType()
  {
    .get instance string System.ServiceModel.Channels.ByteStreamMessageEncoder/*0200000B*/::get_MediaType() /* 0600006D */
  } // end of property ByteStreamMessageEncoder::MediaType
  .property /*1700002B*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/
          MessageVersion()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/ System.ServiceModel.Channels.ByteStreamMessageEncoder/*0200000B*/::get_MessageVersion() /* 0600006E */
  } // end of property ByteStreamMessageEncoder::MessageVersion
} // end of class System.ServiceModel.Channels.ByteStreamMessageEncoder

.class /*0200000C*/ private auto ansi beforefieldinit System.ServiceModel.Channels.ByteStreamMessageEncoderFactory
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoderFactory/*01000020*/
{
  .method /*06000079*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20c8
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoderFactory::.ctor

  .method /*0600007A*/ public hidebysig specialname virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ 
          get_Encoder() cil managed
  // SIG: 20 00 12 65
  {
    // Method begins at RVA 0x20c9
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoderFactory::get_Encoder

  .method /*0600007B*/ public hidebysig specialname virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/ 
          get_MessageVersion() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x20ca
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncoderFactory::get_MessageVersion

  .property /*1700002C*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/
          Encoder()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ System.ServiceModel.Channels.ByteStreamMessageEncoderFactory/*0200000C*/::get_Encoder() /* 0600007A */
  } // end of property ByteStreamMessageEncoderFactory::Encoder
  .property /*1700002D*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/
          MessageVersion()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/ System.ServiceModel.Channels.ByteStreamMessageEncoderFactory/*0200000C*/::get_MessageVersion() /* 0600007B */
  } // end of property ByteStreamMessageEncoderFactory::MessageVersion
} // end of class System.ServiceModel.Channels.ByteStreamMessageEncoderFactory

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.ServiceModel.Channels.ByteStreamMessageEncodingBindingElement
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncodingBindingElement/*01000021*/
{
  .method /*0600007C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20cb
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::.ctor

  .method /*0600007D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quota) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20cc
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::.ctor

  .method /*0600007E*/ public hidebysig specialname virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/ 
          get_MessageVersion() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x20cd
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::get_MessageVersion

  .method /*0600007F*/ public hidebysig specialname virtual 
          instance void  set_MessageVersion(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 12 75
  {
    // Method begins at RVA 0x20ce
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::set_MessageVersion

  .method /*06000080*/ public hidebysig specialname 
          instance class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ 
          get_ReaderQuotas() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x20cf
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::get_ReaderQuotas

  .method /*06000081*/ public hidebysig specialname 
          instance void  set_ReaderQuotas(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ 'value') cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x20d0
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::set_ReaderQuotas

  .method /*06000082*/ public hidebysig virtual 
          instance bool  CanBuildChannelFactory<TChannel>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 02 12 80 89
  {
    // Method begins at RVA 0x20d1
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::CanBuildChannelFactory

  .method /*06000083*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannelFactory`1/*01000023*/<!!TChannel> 
          BuildChannelFactory<TChannel>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 15 12 80 8D 01 1E 00 12 80 89
  {
    // Method begins at RVA 0x20d2
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::BuildChannelFactory

  .method /*06000084*/ public hidebysig virtual 
          instance bool  CanBuildChannelListener<class ([System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannel/*01000024*/) TChannel>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 02 12 80 89
  {
    // Method begins at RVA 0x20d3
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::CanBuildChannelListener

  .method /*06000085*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannelListener`1/*01000025*/<!!TChannel> 
          BuildChannelListener<class ([System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannel/*01000024*/) TChannel>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 15 12 80 95 01 1E 00 12 80 89
  {
    // Method begins at RVA 0x20d4
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::BuildChannelListener

  .method /*06000086*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoderFactory/*01000020*/ 
          CreateMessageEncoderFactory() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x20d5
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::CreateMessageEncoderFactory

  .method /*06000087*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElement/*0100000C*/ 
          Clone() cil managed
  // SIG: 20 00 12 31
  {
    // Method begins at RVA 0x20d6
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::Clone

  .method /*06000088*/ public hidebysig instance bool 
          ShouldSerializeMessageVersion() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000059:0A000021*/ instance void [System/*23000005*/]System.ComponentModel.EditorBrowsableAttribute/*01000080*/::.ctor(valuetype [System/*23000005*/]System.ComponentModel.EditorBrowsableState/*01000081*/) /* 0A000021 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::ShouldSerializeMessageVersion

  .method /*06000089*/ public hidebysig instance bool 
          ShouldSerializeReaderQuotas() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00005A:0A000021*/ instance void [System/*23000005*/]System.ComponentModel.EditorBrowsableAttribute/*01000080*/::.ctor(valuetype [System/*23000005*/]System.ComponentModel.EditorBrowsableState/*01000081*/) /* 0A000021 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x20d8
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageEncodingBindingElement::ShouldSerializeReaderQuotas

  .property /*1700002E*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/
          MessageVersion()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/ System.ServiceModel.Channels.ByteStreamMessageEncodingBindingElement/*0200000D*/::get_MessageVersion() /* 0600007E */
    .set instance void System.ServiceModel.Channels.ByteStreamMessageEncodingBindingElement/*0200000D*/::set_MessageVersion(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageVersion/*0100001D*/) /* 0600007F */
  } // end of property ByteStreamMessageEncodingBindingElement::MessageVersion
  .property /*1700002F*/ instance callconv(8) class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/
          ReaderQuotas()
  {
    .get instance class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ System.ServiceModel.Channels.ByteStreamMessageEncodingBindingElement/*0200000D*/::get_ReaderQuotas() /* 06000080 */
    .set instance void System.ServiceModel.Channels.ByteStreamMessageEncodingBindingElement/*0200000D*/::set_ReaderQuotas(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/) /* 06000081 */
  } // end of property ByteStreamMessageEncodingBindingElement::ReaderQuotas
} // end of class System.ServiceModel.Channels.ByteStreamMessageEncodingBindingElement

.class /*0200000E*/ private abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.ByteStreamMessageUtility
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .field /*04000012*/ public static literal string StreamElementName = "Binary"
  .field /*04000013*/ public static literal string XmlNamespace = "http://www.w3.org/XML/1998/namespace"
  .field /*04000014*/ public static literal string XmlNamespaceNamespace = "http://www.w3.org/2000/xmlns/"
  .field /*04000015*/ static assembly literal string EncoderName = "ByteStreamMessageEncoder"
  .method /*0600008A*/ assembly hidebysig static 
          void  EnsureByteBoundaries(uint8[] buffer,
                                     int32 index,
                                     int32 count,
                                     bool isRead) cil managed
  // SIG: 00 04 01 1D 05 08 08 02
  {
    // Method begins at RVA 0x20d9
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageUtility::EnsureByteBoundaries

  .method /*0600008B*/ assembly hidebysig static 
          class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ 
          EnsureQuotas(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 00 01 12 0D 12 0D
  {
    // Method begins at RVA 0x20da
    // Code size       0 (0x0)
  } // end of method ByteStreamMessageUtility::EnsureQuotas

} // end of class System.ServiceModel.Channels.ByteStreamMessageUtility

.class /*0200000F*/ private auto ansi beforefieldinit System.ServiceModel.Channels.ClientUdpOutputChannel
       extends System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/
{
  .method /*0600008C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ChannelManagerBase/*01000026*/ factory,
                               class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/ remoteEndPoint,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ encoder,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                               class System.ServiceModel.Channels.UdpSocket/*02000021*/[] sendSockets,
                               class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ retransmissionSettings,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ 'to',
                               class [System/*23000005*/]System.Uri/*01000029*/ via,
                               bool isMulticast) cil managed
  // SIG: 20 09 01 12 80 99 12 80 9D 12 65 12 51 1D 12 80 84 12 7C 12 80 A1 12 80 A5 02
  {
    // Method begins at RVA 0x20db
    // Code size       0 (0x0)
  } // end of method ClientUdpOutputChannel::.ctor

  .method /*0600008D*/ public hidebysig specialname 
          instance class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/ 
          get_RemoteEndPoint() cil managed
  // SIG: 20 00 12 80 9D
  {
    .custom /*0C00005B:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20dc
    // Code size       0 (0x0)
  } // end of method ClientUdpOutputChannel::get_RemoteEndPoint

  .method /*0600008E*/ private hidebysig specialname 
          instance void  set_RemoteEndPoint(class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/ 'value') cil managed
  // SIG: 20 01 01 12 80 9D
  {
    .custom /*0C00005C:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       0 (0x0)
  } // end of method ClientUdpOutputChannel::set_RemoteEndPoint

  .method /*0600008F*/ family hidebysig virtual 
          instance class System.ServiceModel.Channels.UdpSocket/*02000021*/[] 
          GetSendSockets(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                         [out] class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/& remoteEndPoint,
                         [out] class [mscorlib/*23000001*/]System.Exception/*0100002A*/& exceptionToBeThrown) cil managed
  // SIG: 20 03 1D 12 80 84 12 59 10 12 80 9D 10 12 80 A9
  {
    // Method begins at RVA 0x20de
    // Code size       0 (0x0)
  } // end of method ClientUdpOutputChannel::GetSendSockets

  .method /*06000090*/ family hidebysig virtual 
          instance void  AddHeadersTo(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x20df
    // Code size       0 (0x0)
  } // end of method ClientUdpOutputChannel::AddHeadersTo

  .property /*17000030*/ instance callconv(8) class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/
          RemoteEndPoint()
  {
    .get instance class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/ System.ServiceModel.Channels.ClientUdpOutputChannel/*0200000F*/::get_RemoteEndPoint() /* 0600008D */
    .set instance void System.ServiceModel.Channels.ClientUdpOutputChannel/*0200000F*/::set_RemoteEndPoint(class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/) /* 0600008E */
  } // end of property ClientUdpOutputChannel::RemoteEndPoint
} // end of class System.ServiceModel.Channels.ClientUdpOutputChannel

.class /*02000010*/ private auto ansi sealed beforefieldinit System.ServiceModel.Channels.DuplicateMessageDetector
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
       implements [mscorlib/*23000001*/]System.IDisposable/*0100002B*/
{
  .method /*06000091*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 maxListLength) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20e0
    // Code size       0 (0x0)
  } // end of method DuplicateMessageDetector::.ctor

  .method /*06000092*/ public hidebysig instance bool 
          IsDuplicate(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> msgBytes,
                      [out] string& hashString) cil managed
  // SIG: 20 02 02 15 11 4D 01 05 10 0E
  {
    // Method begins at RVA 0x20e1
    // Code size       0 (0x0)
  } // end of method DuplicateMessageDetector::IsDuplicate

  .method /*06000093*/ public hidebysig instance void 
          RemoveEntry(string msgHash) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20e2
    // Code size       0 (0x0)
  } // end of method DuplicateMessageDetector::RemoveEntry

  .method /*06000094*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e3
    // Code size       0 (0x0)
  } // end of method DuplicateMessageDetector::Dispose

} // end of class System.ServiceModel.Channels.DuplicateMessageDetector

.class /*02000011*/ interface private abstract auto ansi System.ServiceModel.Channels.IUdpReceiveHandler
{
  .method /*06000095*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_MaxReceivedMessageSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IUdpReceiveHandler::get_MaxReceivedMessageSize

  .method /*06000096*/ public hidebysig newslot abstract virtual 
          instance void  HandleAsyncException(class [mscorlib/*23000001*/]System.Exception/*0100002A*/ exception) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x0
  } // end of method IUdpReceiveHandler::HandleAsyncException

  .method /*06000097*/ public hidebysig newslot abstract virtual 
          instance bool  HandleDataReceived(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> data,
                                            class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ remoteEndpoint,
                                            int32 interfaceIndex,
                                            class [mscorlib/*23000001*/]System.Action/*0100002D*/ onMessageDequeuedCallback) cil managed
  // SIG: 20 04 02 15 11 4D 01 05 12 80 B1 08 12 80 B5
  {
    // Method begins at RVA 0x0
  } // end of method IUdpReceiveHandler::HandleDataReceived

  .property /*17000031*/ instance callconv(8) int32
          MaxReceivedMessageSize()
  {
    .get instance int32 System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/::get_MaxReceivedMessageSize() /* 06000095 */
  } // end of property IUdpReceiveHandler::MaxReceivedMessageSize
} // end of class System.ServiceModel.Channels.IUdpReceiveHandler

.class /*02000012*/ private auto ansi beforefieldinit System.ServiceModel.Channels.ServerUdpOutputChannel
       extends System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/
{
  .method /*06000098*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ChannelManagerBase/*01000026*/ factory,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ encoder,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                               class System.ServiceModel.Channels.UdpSocket/*02000021*/[] sendSockets,
                               class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ retransmissionSettings,
                               class [System/*23000005*/]System.Uri/*01000029*/ via,
                               bool isMulticast) cil managed
  // SIG: 20 07 01 12 80 99 12 65 12 51 1D 12 80 84 12 7C 12 80 A5 02
  {
    // Method begins at RVA 0x20e4
    // Code size       0 (0x0)
  } // end of method ServerUdpOutputChannel::.ctor

  .method /*06000099*/ family hidebysig instance class System.ServiceModel.Channels.UdpSocket/*02000021*/ 
          GetSendSocket(class [System/*23000005*/]System.Net.IPAddress/*0100002E*/ address,
                        class [System/*23000005*/]System.Uri/*01000029*/ destination,
                        [out] class [mscorlib/*23000001*/]System.Exception/*0100002A*/& exceptionToBeThrown) cil managed
  // SIG: 20 03 12 80 84 12 80 B9 12 80 A5 10 12 80 A9
  {
    // Method begins at RVA 0x20e5
    // Code size       0 (0x0)
  } // end of method ServerUdpOutputChannel::GetSendSocket

  .method /*0600009A*/ family hidebysig instance class System.ServiceModel.Channels.UdpSocket/*02000021*/ 
          GetSendSocket(int32 interfaceIndex,
                        [out] class [mscorlib/*23000001*/]System.Exception/*0100002A*/& exceptionToBeThrown) cil managed
  // SIG: 20 02 12 80 84 08 10 12 80 A9
  {
    // Method begins at RVA 0x20e6
    // Code size       0 (0x0)
  } // end of method ServerUdpOutputChannel::GetSendSocket

  .method /*0600009B*/ family hidebysig virtual 
          instance class System.ServiceModel.Channels.UdpSocket/*02000021*/[] 
          GetSendSockets(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                         [out] class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/& remoteEndPoint,
                         [out] class [mscorlib/*23000001*/]System.Exception/*0100002A*/& exceptionToBeThrown) cil managed
  // SIG: 20 03 1D 12 80 84 12 59 10 12 80 9D 10 12 80 A9
  {
    // Method begins at RVA 0x20e7
    // Code size       0 (0x0)
  } // end of method ServerUdpOutputChannel::GetSendSockets

} // end of class System.ServiceModel.Channels.ServerUdpOutputChannel

.class /*02000013*/ private auto ansi beforefieldinit System.ServiceModel.Channels.SynchronizedRandom
       extends [mscorlib/*23000001*/]System.Random/*0100002F*/
{
  .method /*0600009C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e8
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::.ctor

  .method /*0600009D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 seed) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20e9
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::.ctor

  .method /*0600009E*/ family hidebysig specialname 
          instance object  get_ThisLock() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00005D:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ea
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::get_ThisLock

  .method /*0600009F*/ private hidebysig specialname 
          instance void  set_ThisLock(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00005E:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20eb
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::set_ThisLock

  .method /*060000A0*/ public hidebysig virtual 
          instance int32  Next(int32 minValue,
                               int32 maxValue) cil managed
  // SIG: 20 02 08 08 08
  {
    // Method begins at RVA 0x20ec
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::Next

  .method /*060000A1*/ public hidebysig virtual 
          instance int32  Next() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20ed
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::Next

  .method /*060000A2*/ public hidebysig virtual 
          instance int32  Next(int32 maxValue) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x20ee
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::Next

  .method /*060000A3*/ public hidebysig virtual 
          instance void  NextBytes(uint8[] buffer) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x20ef
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::NextBytes

  .method /*060000A4*/ public hidebysig virtual 
          instance float64  NextDouble() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x20f0
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::NextDouble

  .method /*060000A5*/ family hidebysig virtual 
          instance float64  Sample() cil managed
  // SIG: 20 00 0D
  {
    // Method begins at RVA 0x20f1
    // Code size       0 (0x0)
  } // end of method SynchronizedRandom::Sample

  .property /*17000032*/ instance callconv(8) object
          ThisLock()
  {
    .get instance object System.ServiceModel.Channels.SynchronizedRandom/*02000013*/::get_ThisLock() /* 0600009E */
    .set instance void System.ServiceModel.Channels.SynchronizedRandom/*02000013*/::set_ThisLock(object) /* 0600009F */
  } // end of property SynchronizedRandom::ThisLock
} // end of class System.ServiceModel.Channels.SynchronizedRandom

.class /*02000014*/ private abstract auto ansi beforefieldinit System.ServiceModel.Channels.UdpChannelBase`1<class ([mscorlib/*23000001*/]System.IDisposable/*0100002B*/) QueueItemType>
       extends class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.InputQueueChannel`1/*01000030*/<!QueueItemType>/*1B000002*/
       implements System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/
{
  .method /*060000A6*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ChannelManagerBase/*01000026*/ channelManager,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ encoder,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                               class System.ServiceModel.Channels.UdpSocket/*02000021*/[] sockets,
                               class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ retransmissionSettings,
                               int64 maxPendingMessagesTotalSize,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ localAddress,
                               class [System/*23000005*/]System.Uri/*01000029*/ via,
                               bool isMulticast,
                               int32 maxReceivedMessageSize) cil managed
  // SIG: 20 0A 01 12 80 99 12 65 12 51 1D 12 80 84 12 7C 0A 12 80 A1 12 80 A5 02 08
  {
    // Method begins at RVA 0x20f2
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::.ctor

  .method /*060000A7*/ public hidebysig newslot specialname virtual final 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ 
          get_LocalAddress() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C00005F:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f3
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_LocalAddress

  .method /*060000A8*/ private hidebysig specialname 
          instance void  set_LocalAddress(class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .custom /*0C000060:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f4
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_LocalAddress

  .method /*060000A9*/ public hidebysig newslot specialname virtual final 
          instance class [System/*23000005*/]System.Uri/*01000029*/ 
          get_Via() cil managed
  // SIG: 20 00 12 80 A5
  {
    // Method begins at RVA 0x20f5
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_Via

  .method /*060000AA*/ private hidebysig newslot specialname virtual final 
          instance int32  System.ServiceModel.Channels.IUdpReceiveHandler.get_MaxReceivedMessageSize() cil managed
  // SIG: 20 00 08
  {
    .override System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/::get_MaxReceivedMessageSize /*02000011::06000095*/ 
    // Method begins at RVA 0x20f6
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::System.ServiceModel.Channels.IUdpReceiveHandler.get_MaxReceivedMessageSize

  .method /*060000AB*/ family hidebysig newslot specialname abstract virtual 
          instance bool  get_IgnoreSerializationException() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method UdpChannelBase`1::get_IgnoreSerializationException

  .method /*060000AC*/ family hidebysig specialname 
          instance bool  get_OwnsBufferManager() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000061:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f7
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_OwnsBufferManager

  .method /*060000AD*/ family hidebysig specialname 
          instance void  set_OwnsBufferManager(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000062:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_OwnsBufferManager

  .method /*060000AE*/ family hidebysig specialname 
          instance class System.ServiceModel.Channels.DuplicateMessageDetector/*02000010*/ 
          get_DuplicateDetector() cil managed
  // SIG: 20 00 12 40
  {
    .custom /*0C000063:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f9
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_DuplicateDetector

  .method /*060000AF*/ family hidebysig specialname 
          instance void  set_DuplicateDetector(class System.ServiceModel.Channels.DuplicateMessageDetector/*02000010*/ 'value') cil managed
  // SIG: 20 01 01 12 40
  {
    .custom /*0C000064:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fa
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_DuplicateDetector

  .method /*060000B0*/ family hidebysig specialname 
          instance class System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*/ 
          get_ReceiveManager() cil managed
  // SIG: 20 00 12 80 88
  {
    .custom /*0C000065:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_ReceiveManager

  .method /*060000B1*/ family hidebysig specialname 
          instance void  set_ReceiveManager(class System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*/ 'value') cil managed
  // SIG: 20 01 01 12 80 88
  {
    .custom /*0C000066:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_ReceiveManager

  .method /*060000B2*/ family hidebysig specialname 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ 
          get_BufferManager() cil managed
  // SIG: 20 00 12 51
  {
    .custom /*0C000067:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_BufferManager

  .method /*060000B3*/ private hidebysig specialname 
          instance void  set_BufferManager(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ 'value') cil managed
  // SIG: 20 01 01 12 51
  {
    .custom /*0C000068:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_BufferManager

  .method /*060000B4*/ family hidebysig specialname 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ 
          get_Encoder() cil managed
  // SIG: 20 00 12 65
  {
    .custom /*0C000069:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ff
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_Encoder

  .method /*060000B5*/ private hidebysig specialname 
          instance void  set_Encoder(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ 'value') cil managed
  // SIG: 20 01 01 12 65
  {
    .custom /*0C00006A:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_Encoder

  .method /*060000B6*/ family hidebysig specialname 
          instance bool  get_IsMulticast() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00006B:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_IsMulticast

  .method /*060000B7*/ private hidebysig specialname 
          instance void  set_IsMulticast(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00006C:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_IsMulticast

  .method /*060000B8*/ family hidebysig specialname 
          instance class System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/ 
          get_UdpOutputChannel() cil managed
  // SIG: 20 00 12 70
  {
    .custom /*0C00006D:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2103
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_UdpOutputChannel

  .method /*060000B9*/ private hidebysig specialname 
          instance void  set_UdpOutputChannel(class System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/ 'value') cil managed
  // SIG: 20 01 01 12 70
  {
    .custom /*0C00006E:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2104
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_UdpOutputChannel

  .method /*060000BA*/ family hidebysig specialname 
          instance class System.ServiceModel.Channels.UdpSocket/*02000021*/[] 
          get_Sockets() cil managed
  // SIG: 20 00 1D 12 80 84
  {
    .custom /*0C00006F:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::get_Sockets

  .method /*060000BB*/ private hidebysig specialname 
          instance void  set_Sockets(class System.ServiceModel.Channels.UdpSocket/*02000021*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 80 84
  {
    .custom /*0C000070:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2106
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::set_Sockets

  .method /*060000BC*/ public hidebysig virtual 
          instance !!T  GetProperty<class T>() cil managed
  // SIG: 30 01 00 1E 00
  {
    // Method begins at RVA 0x2107
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::GetProperty

  .method /*060000BD*/ private hidebysig newslot virtual final 
          instance bool  System.ServiceModel.Channels.IUdpReceiveHandler.HandleDataReceived(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> data,
                                                                                            class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ remoteEndpoint,
                                                                                            int32 interfaceIndex,
                                                                                            class [mscorlib/*23000001*/]System.Action/*0100002D*/ onMessageDequeuedCallback) cil managed
  // SIG: 20 04 02 15 11 4D 01 05 12 80 B1 08 12 80 B5
  {
    .override System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/::HandleDataReceived /*02000011::06000097*/ 
    // Method begins at RVA 0x2108
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::System.ServiceModel.Channels.IUdpReceiveHandler.HandleDataReceived

  .method /*060000BE*/ private hidebysig newslot virtual final 
          instance void  System.ServiceModel.Channels.IUdpReceiveHandler.HandleAsyncException(class [mscorlib/*23000001*/]System.Exception/*0100002A*/ ex) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .override System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/::HandleAsyncException /*02000011::06000096*/ 
    // Method begins at RVA 0x2109
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::System.ServiceModel.Channels.IUdpReceiveHandler.HandleAsyncException

  .method /*060000BF*/ assembly hidebysig newslot strict virtual 
          instance void  HandleReceiveException(class [mscorlib/*23000001*/]System.Exception/*0100002A*/ ex) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x210a
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::HandleReceiveException

  .method /*060000C0*/ assembly hidebysig 
          instance bool  TransferReceiveManagerOwnership(class System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*/ socketReceiveManager,
                                                         class System.ServiceModel.Channels.DuplicateMessageDetector/*02000010*/ duplicateDetector) cil managed
  // SIG: 20 02 02 12 80 88 12 40
  {
    // Method begins at RVA 0x210b
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::TransferReceiveManagerOwnership

  .method /*060000C1*/ assembly hidebysig 
          instance bool  EnqueueMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                        int32 messageBufferSize,
                                        class [mscorlib/*23000001*/]System.Action/*0100002D*/ messageDequeuedCallback) cil managed
  // SIG: 20 03 02 12 59 08 12 80 B5
  {
    // Method begins at RVA 0x210c
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::EnqueueMessage

  .method /*060000C2*/ assembly hidebysig newslot abstract strict virtual 
          instance void  FinishEnqueueMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                              class [mscorlib/*23000001*/]System.Action/*0100002D*/ dequeuedCallback,
                                              bool canDispatchOnThisThread) cil managed
  // SIG: 20 03 01 12 59 12 80 B5 02
  {
    // Method begins at RVA 0x0
  } // end of method UdpChannelBase`1::FinishEnqueueMessage

  .method /*060000C3*/ family hidebysig newslot virtual 
          instance void  AddHeadersTo(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x210d
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::AddHeadersTo

  .method /*060000C4*/ family hidebysig virtual 
          instance void  OnAbort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210e
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::OnAbort

  .method /*060000C5*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginOpen(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x210f
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::OnBeginOpen

  .method /*060000C6*/ family hidebysig virtual 
          instance void  OnEndOpen(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2110
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::OnEndOpen

  .method /*060000C7*/ family hidebysig virtual 
          instance void  OnOpen(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x2111
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::OnOpen

  .method /*060000C8*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginClose(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                       class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                       object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x2112
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::OnBeginClose

  .method /*060000C9*/ family hidebysig virtual 
          instance void  OnEndClose(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2113
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::OnEndClose

  .method /*060000CA*/ family hidebysig virtual 
          instance void  OnClose(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x2114
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::OnClose

  .method /*060000CB*/ family hidebysig instance void 
          SetOutputChannel(class System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/ udpOutputChannel) cil managed
  // SIG: 20 01 01 12 70
  {
    // Method begins at RVA 0x2115
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::SetOutputChannel

  .method /*060000CC*/ family hidebysig instance void 
          Cleanup(bool aborting,
                  valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 02 01 02 11 45
  {
    // Method begins at RVA 0x2116
    // Code size       0 (0x0)
  } // end of method UdpChannelBase`1::Cleanup

  .property /*17000033*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/
          LocalAddress()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_LocalAddress() /* 060000A7 */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_LocalAddress(class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/) /* 060000A8 */
  } // end of property UdpChannelBase`1::LocalAddress
  .property /*17000034*/ instance callconv(8) class [System/*23000005*/]System.Uri/*01000029*/
          Via()
  {
    .get instance class [System/*23000005*/]System.Uri/*01000029*/ System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_Via() /* 060000A9 */
  } // end of property UdpChannelBase`1::Via
  .property /*17000035*/ instance callconv(8) int32
          System.ServiceModel.Channels.IUdpReceiveHandler.MaxReceivedMessageSize()
  {
    .get instance int32 System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::System.ServiceModel.Channels.IUdpReceiveHandler.get_MaxReceivedMessageSize() /* 060000AA */
  } // end of property UdpChannelBase`1::System.ServiceModel.Channels.IUdpReceiveHandler.MaxReceivedMessageSize
  .property /*17000036*/ instance callconv(8) bool
          IgnoreSerializationException()
  {
    .get instance bool System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_IgnoreSerializationException() /* 060000AB */
  } // end of property UdpChannelBase`1::IgnoreSerializationException
  .property /*17000037*/ instance callconv(8) bool
          OwnsBufferManager()
  {
    .get instance bool System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_OwnsBufferManager() /* 060000AC */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_OwnsBufferManager(bool) /* 060000AD */
  } // end of property UdpChannelBase`1::OwnsBufferManager
  .property /*17000038*/ instance callconv(8) class System.ServiceModel.Channels.DuplicateMessageDetector/*02000010*/
          DuplicateDetector()
  {
    .get instance class System.ServiceModel.Channels.DuplicateMessageDetector/*02000010*/ System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_DuplicateDetector() /* 060000AE */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_DuplicateDetector(class System.ServiceModel.Channels.DuplicateMessageDetector/*02000010*/) /* 060000AF */
  } // end of property UdpChannelBase`1::DuplicateDetector
  .property /*17000039*/ instance callconv(8) class System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*/
          ReceiveManager()
  {
    .get instance class System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*/ System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_ReceiveManager() /* 060000B0 */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_ReceiveManager(class System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*/) /* 060000B1 */
  } // end of property UdpChannelBase`1::ReceiveManager
  .property /*1700003A*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/
          BufferManager()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_BufferManager() /* 060000B2 */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_BufferManager(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/) /* 060000B3 */
  } // end of property UdpChannelBase`1::BufferManager
  .property /*1700003B*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/
          Encoder()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_Encoder() /* 060000B4 */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_Encoder(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/) /* 060000B5 */
  } // end of property UdpChannelBase`1::Encoder
  .property /*1700003C*/ instance callconv(8) bool
          IsMulticast()
  {
    .get instance bool System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_IsMulticast() /* 060000B6 */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_IsMulticast(bool) /* 060000B7 */
  } // end of property UdpChannelBase`1::IsMulticast
  .property /*1700003D*/ instance callconv(8) class System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/
          UdpOutputChannel()
  {
    .get instance class System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/ System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_UdpOutputChannel() /* 060000B8 */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_UdpOutputChannel(class System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/) /* 060000B9 */
  } // end of property UdpChannelBase`1::UdpOutputChannel
  .property /*1700003E*/ instance callconv(8) class System.ServiceModel.Channels.UdpSocket/*02000021*/[]
          Sockets()
  {
    .get instance class System.ServiceModel.Channels.UdpSocket/*02000021*/[] System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::get_Sockets() /* 060000BA */
    .set instance void System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/::set_Sockets(class System.ServiceModel.Channels.UdpSocket/*02000021*/[]) /* 060000BB */
  } // end of property UdpChannelBase`1::Sockets
} // end of class System.ServiceModel.Channels.UdpChannelBase`1

.class /*02000015*/ private auto ansi beforefieldinit System.ServiceModel.Channels.UdpChannelFactory`1<TChannel>
       extends class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ChannelFactoryBase`1/*01000031*/<!TChannel>/*1B000003*/
{
  .method /*060000CD*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/ transportBindingElement,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 20 02 01 12 80 8C 12 80 89
  {
    // Method begins at RVA 0x2117
    // Code size       0 (0x0)
  } // end of method UdpChannelFactory`1::.ctor

  .method /*060000CE*/ public hidebysig virtual 
          instance !!T  GetProperty<class T>() cil managed
  // SIG: 30 01 00 1E 00
  {
    // Method begins at RVA 0x2118
    // Code size       0 (0x0)
  } // end of method UdpChannelFactory`1::GetProperty

  .method /*060000CF*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginOpen(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x2119
    // Code size       0 (0x0)
  } // end of method UdpChannelFactory`1::OnBeginOpen

  .method /*060000D0*/ family hidebysig virtual 
          instance !TChannel  OnCreateChannel(class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ 'to',
                                              class [System/*23000005*/]System.Uri/*01000029*/ via) cil managed
  // SIG: 20 02 13 00 12 80 A1 12 80 A5
  {
    // Method begins at RVA 0x211c
    // Code size       0 (0x0)
  } // end of method UdpChannelFactory`1::OnCreateChannel

  .method /*060000D1*/ family hidebysig virtual 
          instance void  OnEndOpen(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2128
    // Code size       0 (0x0)
  } // end of method UdpChannelFactory`1::OnEndOpen

  .method /*060000D2*/ family hidebysig virtual 
          instance void  OnOpen(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x2129
    // Code size       0 (0x0)
  } // end of method UdpChannelFactory`1::OnOpen

} // end of class System.ServiceModel.Channels.UdpChannelFactory`1

.class /*02000016*/ private abstract auto ansi beforefieldinit System.ServiceModel.Channels.UdpChannelListener`3<class ([System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannel/*01000024*/) ChannelInterfaceType,(class System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/<!QueueItemType>/*1B000005*/, !ChannelInterfaceType/*1B000006*/) TChannel,class ([mscorlib/*23000001*/]System.IDisposable/*0100002B*/) QueueItemType>
       extends class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ChannelListenerBase`1/*01000032*/<!ChannelInterfaceType>/*1B000004*/
       implements System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/
{
  .method /*060000D3*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/ udpTransportBindingElement,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 20 02 01 12 80 8C 12 80 89
  {
    // Method begins at RVA 0x212a
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::.ctor

  .method /*060000D4*/ public hidebysig specialname 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoderFactory/*01000020*/ 
          get_MessageEncoderFactory() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x212b
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::get_MessageEncoderFactory

  .method /*060000D5*/ public hidebysig specialname virtual 
          instance class [System/*23000005*/]System.Uri/*01000029*/ 
          get_Uri() cil managed
  // SIG: 20 00 12 80 A5
  {
    // Method begins at RVA 0x212c
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::get_Uri

  .method /*060000D6*/ family hidebysig specialname virtual 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_DefaultReceiveTimeout() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x212d
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::get_DefaultReceiveTimeout

  .method /*060000D7*/ family hidebysig specialname virtual 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_DefaultSendTimeout() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x212e
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::get_DefaultSendTimeout

  .method /*060000D8*/ assembly hidebysig specialname 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ 
          get_BufferManager() cil managed
  // SIG: 20 00 12 51
  {
    // Method begins at RVA 0x212f
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::get_BufferManager

  .method /*060000D9*/ assembly hidebysig specialname 
          instance class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/ 
          get_UdpTransportBindingElement() cil managed
  // SIG: 20 00 12 80 8C
  {
    // Method begins at RVA 0x2130
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::get_UdpTransportBindingElement

  .method /*060000DA*/ assembly hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000033*/<class System.ServiceModel.Channels.UdpSocket/*02000021*/> 
          get_ListenSockets() cil managed
  // SIG: 20 00 15 12 80 CD 01 12 80 84
  {
    // Method begins at RVA 0x2131
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::get_ListenSockets

  .method /*060000DB*/ assembly hidebysig specialname 
          instance bool  get_IsMulticast() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2132
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::get_IsMulticast

  .method /*060000DC*/ private hidebysig newslot specialname virtual final 
          instance int32  System.ServiceModel.Channels.IUdpReceiveHandler.get_MaxReceivedMessageSize() cil managed
  // SIG: 20 00 08
  {
    .override System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/::get_MaxReceivedMessageSize /*02000011::06000095*/ 
    // Method begins at RVA 0x2133
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::System.ServiceModel.Channels.IUdpReceiveHandler.get_MaxReceivedMessageSize

  .method /*060000DD*/ public hidebysig virtual 
          instance !!T  GetProperty<class T>() cil managed
  // SIG: 30 01 00 1E 00
  {
    // Method begins at RVA 0x2134
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::GetProperty

  .method /*060000DE*/ private hidebysig newslot virtual final 
          instance void  System.ServiceModel.Channels.IUdpReceiveHandler.HandleAsyncException(class [mscorlib/*23000001*/]System.Exception/*0100002A*/ ex) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .override System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/::HandleAsyncException /*02000011::06000096*/ 
    // Method begins at RVA 0x2135
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::System.ServiceModel.Channels.IUdpReceiveHandler.HandleAsyncException

  .method /*060000DF*/ private hidebysig newslot virtual final 
          instance bool  System.ServiceModel.Channels.IUdpReceiveHandler.HandleDataReceived(valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> data,
                                                                                            class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ remoteEndpoint,
                                                                                            int32 interfaceIndex,
                                                                                            class [mscorlib/*23000001*/]System.Action/*0100002D*/ onMessageDequeuedCallback) cil managed
  // SIG: 20 04 02 15 11 4D 01 05 12 80 B1 08 12 80 B5
  {
    .override System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/::HandleDataReceived /*02000011::06000097*/ 
    // Method begins at RVA 0x2136
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::System.ServiceModel.Channels.IUdpReceiveHandler.HandleDataReceived

  .method /*060000E0*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Type/*01000008*/ 
          GetCommunicationObjectType() cil managed
  // SIG: 20 00 12 21
  {
    // Method begins at RVA 0x2137
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::GetCommunicationObjectType

  .method /*060000E1*/ family hidebysig virtual 
          instance void  OnAbort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2138
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnAbort

  .method /*060000E2*/ family hidebysig virtual 
          instance !ChannelInterfaceType 
          OnAcceptChannel(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 13 00 11 45
  {
    // Method begins at RVA 0x2139
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnAcceptChannel

  .method /*060000E3*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginAcceptChannel(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                               class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                               object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x213a
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnBeginAcceptChannel

  .method /*060000E4*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginClose(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                       class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                       object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x213b
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnBeginClose

  .method /*060000E5*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginOpen(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x213c
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnBeginOpen

  .method /*060000E6*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginWaitForChannel(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                                class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                                object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x213d
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnBeginWaitForChannel

  .method /*060000E7*/ family hidebysig virtual 
          instance void  OnClosing() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213e
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnClosing

  .method /*060000E8*/ family hidebysig virtual 
          instance void  OnClose(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x213f
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnClose

  .method /*060000E9*/ family hidebysig virtual 
          instance !ChannelInterfaceType 
          OnEndAcceptChannel(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 13 00 12 79
  {
    // Method begins at RVA 0x2140
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnEndAcceptChannel

  .method /*060000EA*/ family hidebysig virtual 
          instance void  OnEndClose(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2141
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnEndClose

  .method /*060000EB*/ family hidebysig virtual 
          instance void  OnEndOpen(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2142
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnEndOpen

  .method /*060000EC*/ family hidebysig virtual 
          instance bool  OnEndWaitForChannel(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 02 12 79
  {
    // Method begins at RVA 0x2143
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnEndWaitForChannel

  .method /*060000ED*/ family hidebysig virtual 
          instance void  OnOpen(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x2144
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnOpen

  .method /*060000EE*/ family hidebysig virtual 
          instance void  OnOpened() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2145
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnOpened

  .method /*060000EF*/ family hidebysig virtual 
          instance bool  OnWaitForChannel(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 02 11 45
  {
    // Method begins at RVA 0x2146
    // Code size       0 (0x0)
  } // end of method UdpChannelListener`3::OnWaitForChannel

  .method /*060000F0*/ public hidebysig newslot abstract virtual 
          instance !TChannel  CreateChannel() cil managed
  // SIG: 20 00 13 01
  {
    // Method begins at RVA 0x0
  } // end of method UdpChannelListener`3::CreateChannel

  .property /*1700003F*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoderFactory/*01000020*/
          MessageEncoderFactory()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoderFactory/*01000020*/ System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::get_MessageEncoderFactory() /* 060000D4 */
  } // end of property UdpChannelListener`3::MessageEncoderFactory
  .property /*17000040*/ instance callconv(8) class [System/*23000005*/]System.Uri/*01000029*/
          Uri()
  {
    .get instance class [System/*23000005*/]System.Uri/*01000029*/ System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::get_Uri() /* 060000D5 */
  } // end of property UdpChannelListener`3::Uri
  .property /*17000041*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          DefaultReceiveTimeout()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::get_DefaultReceiveTimeout() /* 060000D6 */
  } // end of property UdpChannelListener`3::DefaultReceiveTimeout
  .property /*17000042*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          DefaultSendTimeout()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::get_DefaultSendTimeout() /* 060000D7 */
  } // end of property UdpChannelListener`3::DefaultSendTimeout
  .property /*17000043*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/
          BufferManager()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::get_BufferManager() /* 060000D8 */
  } // end of property UdpChannelListener`3::BufferManager
  .property /*17000044*/ instance callconv(8) class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/
          UdpTransportBindingElement()
  {
    .get instance class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/ System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::get_UdpTransportBindingElement() /* 060000D9 */
  } // end of property UdpChannelListener`3::UdpTransportBindingElement
  .property /*17000045*/ instance callconv(8) class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000033*/<class System.ServiceModel.Channels.UdpSocket/*02000021*/>
          ListenSockets()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000033*/<class System.ServiceModel.Channels.UdpSocket/*02000021*/> System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::get_ListenSockets() /* 060000DA */
  } // end of property UdpChannelListener`3::ListenSockets
  .property /*17000046*/ instance callconv(8) bool
          IsMulticast()
  {
    .get instance bool System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::get_IsMulticast() /* 060000DB */
  } // end of property UdpChannelListener`3::IsMulticast
  .property /*17000047*/ instance callconv(8) int32
          System.ServiceModel.Channels.IUdpReceiveHandler.MaxReceivedMessageSize()
  {
    .get instance int32 System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/::System.ServiceModel.Channels.IUdpReceiveHandler.get_MaxReceivedMessageSize() /* 060000DC */
  } // end of property UdpChannelListener`3::System.ServiceModel.Channels.IUdpReceiveHandler.MaxReceivedMessageSize
} // end of class System.ServiceModel.Channels.UdpChannelListener`3

.class /*02000017*/ private auto ansi beforefieldinit System.ServiceModel.Channels.UdpDuplexChannelListener
       extends class System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/<class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IDuplexChannel/*01000034*/,class System.ServiceModel.Channels.UdpDuplexChannel/*0200001B*/,class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/>/*1B000007*/
{
  .method /*060000F1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/ udpTransportBindingElement,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 20 02 01 12 80 8C 12 80 89
  {
    // Method begins at RVA 0x2147
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannelListener::.ctor

  .method /*060000F2*/ public hidebysig virtual 
          instance class System.ServiceModel.Channels.UdpDuplexChannel/*0200001B*/ 
          CreateChannel() cil managed
  // SIG: 20 00 12 6C
  {
    // Method begins at RVA 0x2148
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannelListener::CreateChannel

} // end of class System.ServiceModel.Channels.UdpDuplexChannelListener

.class /*02000018*/ private auto ansi beforefieldinit System.ServiceModel.Channels.UdpReplyChannelListener
       extends class System.ServiceModel.Channels.UdpChannelListener`3/*02000016*/<class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IReplyChannel/*01000035*/,class System.ServiceModel.Channels.UdpReplyChannel/*0200001E*/,class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.RequestContext/*01000036*/>/*1B000008*/
{
  .method /*060000F3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/ udpTransportBindingElement,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 20 02 01 12 80 8C 12 80 89
  {
    // Method begins at RVA 0x2149
    // Code size       0 (0x0)
  } // end of method UdpReplyChannelListener::.ctor

  .method /*060000F4*/ public hidebysig virtual 
          instance class System.ServiceModel.Channels.UdpReplyChannel/*0200001E*/ 
          CreateChannel() cil managed
  // SIG: 20 00 12 78
  {
    // Method begins at RVA 0x214a
    // Code size       0 (0x0)
  } // end of method UdpReplyChannelListener::CreateChannel

} // end of class System.ServiceModel.Channels.UdpReplyChannelListener

.class /*02000019*/ private auto ansi sealed beforefieldinit System.ServiceModel.Channels.ServerUdpDuplexChannel
       extends System.ServiceModel.Channels.UdpDuplexChannel/*0200001B*/
{
  .method /*060000F5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpDuplexChannelListener/*02000017*/ listener,
                               class System.ServiceModel.Channels.UdpSocket/*02000021*/[] sockets,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ localAddress,
                               class [System/*23000005*/]System.Uri/*01000029*/ via,
                               bool isMulticast) cil managed
  // SIG: 20 05 01 12 5C 1D 12 80 84 12 80 A1 12 80 A5 02
  {
    // Method begins at RVA 0x214c
    // Code size       0 (0x0)
  } // end of method ServerUdpDuplexChannel::.ctor

  .method /*060000F6*/ family hidebysig specialname virtual 
          instance bool  get_IgnoreSerializationException() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2158
    // Code size       0 (0x0)
  } // end of method ServerUdpDuplexChannel::get_IgnoreSerializationException

  .method /*060000F7*/ assembly hidebysig strict virtual 
          instance void  HandleReceiveException(class [mscorlib/*23000001*/]System.Exception/*0100002A*/ ex) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x2159
    // Code size       0 (0x0)
  } // end of method ServerUdpDuplexChannel::HandleReceiveException

  .property /*17000048*/ instance callconv(8) bool
          IgnoreSerializationException()
  {
    .get instance bool System.ServiceModel.Channels.ServerUdpDuplexChannel/*02000019*/::get_IgnoreSerializationException() /* 060000F6 */
  } // end of property ServerUdpDuplexChannel::IgnoreSerializationException
} // end of class System.ServiceModel.Channels.ServerUdpDuplexChannel

.class /*0200001A*/ private abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.UdpConstants
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .class /*02000032*/ abstract auto ansi sealed nested assembly beforefieldinit Defaults
         extends [mscorlib/*23000001*/]System.Object/*01000012*/
  {
    .field /*04000026*/ public static initonly valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ ReceiveTimeout
    .field /*04000027*/ public static initonly valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ SendTimeout
    .field /*04000028*/ public static literal string EncodingString = "utf-8"
    .field /*04000029*/ public static literal string DelayLowerBound = "00:00:00.050"
    .field /*0400002A*/ public static literal string DelayUpperBound = "00:00:00.250"
    .field /*0400002B*/ public static literal int32 DuplicateMessageHistoryLength = int32(0x00000000)
    .field /*0400002C*/ public static literal int32 DuplicateMessageHistoryLengthWithRetransmission = int32(0x00001000)
    .field /*0400002D*/ public static literal int32 InterfaceIndex = int32(0xFFFFFFFF)
    .field /*0400002E*/ public static literal string MaxDelayPerRetransmission = "00:00:00.500"
    .field /*0400002F*/ public static literal int32 MaxRetransmitCount = int32(0x00000000)
    .field /*04000030*/ public static literal int32 MaxUnicastRetransmitCount = int32(0x00000000)
    .field /*04000031*/ public static literal int32 MaxMulticastRetransmitCount = int32(0x00000000)
    .field /*04000032*/ public static literal int64 DefaultMaxPendingMessagesTotalSize = int64(0x0)
    .field /*04000033*/ public static initonly int64 MaxPendingMessagesTotalSize
    .field /*04000034*/ public static literal int64 MaxReceivedMessageSize = int64(0x10000)
    .field /*04000035*/ public static literal string MulticastInterfaceId = nullref
    .field /*04000036*/ public static literal int32 SocketReceiveBufferSize = int32(0x00010000)
    .field /*04000037*/ public static literal int32 TimeToLive = int32(0x00000001)
    .field /*04000038*/ public static class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoderFactory/*01000020*/ MessageEncoderFactory
    .field /*04000039*/ public static initonly valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ DelayLowerBoundTimeSpan
    .field /*0400003A*/ public static initonly valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ DelayUpperBoundTimeSpan
    .field /*0400003B*/ public static initonly valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ MaxDelayPerRetransmissionTimeSpan
    .method /*0600026B*/ private hidebysig specialname rtspecialname static 
            void  .cctor() cil managed
    // SIG: 00 00 01
    {
      // Method begins at RVA 0x22e0
      // Code size       0 (0x0)
    } // end of method Defaults::.cctor

  } // end of class Defaults

  .field /*04000016*/ public static literal int32 MaxMessageSizeOverIPv4 = int32(0x0000FFE3)
  .field /*04000017*/ public static literal int32 MaxTimeToLive = int32(0x000000FF)
  .field /*04000018*/ public static literal int64 MinPendingMessagesTotalSize = int64(0x0)
  .field /*04000019*/ public static literal int32 MinReceiveBufferSize = int32(0x00000001)
  .field /*0400001A*/ public static literal int32 MinTimeToLive = int32(0x00000000)
  .field /*0400001B*/ public static literal int32 PendingReceiveCountPerProcessor = int32(0x00000002)
  .field /*0400001C*/ public static literal string Scheme = "soap.udp"
  .field /*0400001D*/ public static literal string TimeSpanZero = "00:00:00"
  .field /*0400001E*/ public static literal string WsdlSoapUdpTransportUri = "http://schemas.microsoft.com/soap/udp"
  .field /*0400001F*/ public static literal string WsdlSoapUdpTransportNamespace = "http://schemas.microsoft.com/ws/06/2010/policy/soa"
  + "p/udp"
  .field /*04000020*/ public static literal string WsdlSoapUdpTransportPrefix = "sud"
  .field /*04000021*/ public static literal string RetransmissionEnabled = "RetransmissionEnabled"
} // end of class System.ServiceModel.Channels.UdpConstants

.class /*0200001B*/ private abstract auto ansi beforefieldinit System.ServiceModel.Channels.UdpDuplexChannel
       extends class System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/<class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/>/*1B000009*/
       implements [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IDuplexChannel/*01000034*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IInputChannel/*01000037*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannel/*01000024*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.ICommunicationObject/*01000038*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IOutputChannel/*01000039*/
{
  .method /*060000F8*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ChannelManagerBase/*01000026*/ channelMananger,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ encoder,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                               class System.ServiceModel.Channels.UdpSocket/*02000021*/[] sendSockets,
                               class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ retransmissionSettings,
                               int64 maxPendingMessagesTotalSize,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ localAddress,
                               class [System/*23000005*/]System.Uri/*01000029*/ via,
                               bool isMulticast,
                               int32 maxReceivedMessageSize) cil managed
  // SIG: 20 0A 01 12 80 99 12 65 12 51 1D 12 80 84 12 7C 0A 12 80 A1 12 80 A5 02 08
  {
    // Method begins at RVA 0x215c
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::.ctor

  .method /*060000F9*/ public hidebysig newslot specialname virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ 
          get_RemoteAddress() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x2168
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::get_RemoteAddress

  .method /*060000FA*/ public hidebysig virtual 
          instance !!T  GetProperty<class T>() cil managed
  // SIG: 30 01 00 1E 00
  {
    // Method begins at RVA 0x2169
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::GetProperty

  .method /*060000FB*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginSend(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                    class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                    object state) cil managed
  // SIG: 20 03 12 79 12 59 12 7D 1C
  {
    // Method begins at RVA 0x216a
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::BeginSend

  .method /*060000FC*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginSend(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                    valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                    class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                    object state) cil managed
  // SIG: 20 04 12 79 12 59 11 45 12 7D 1C
  {
    // Method begins at RVA 0x216b
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::BeginSend

  .method /*060000FD*/ public hidebysig newslot virtual final 
          instance void  EndSend(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x216c
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::EndSend

  .method /*060000FE*/ public hidebysig newslot virtual final 
          instance void  Send(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x216d
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::Send

  .method /*060000FF*/ public hidebysig newslot virtual final 
          instance void  Send(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                              valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 02 01 12 59 11 45
  {
    // Method begins at RVA 0x216e
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::Send

  .method /*06000100*/ public hidebysig newslot virtual final 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          Receive() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x216f
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::Receive

  .method /*06000101*/ public hidebysig newslot virtual final 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          Receive(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 12 59 11 45
  {
    // Method begins at RVA 0x2170
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::Receive

  .method /*06000102*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginReceive(class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                       object state) cil managed
  // SIG: 20 02 12 79 12 7D 1C
  {
    // Method begins at RVA 0x2171
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::BeginReceive

  .method /*06000103*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginReceive(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                       class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                       object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x2172
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::BeginReceive

  .method /*06000104*/ public hidebysig newslot virtual final 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          EndReceive(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 12 59 12 79
  {
    // Method begins at RVA 0x2173
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::EndReceive

  .method /*06000105*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                                    [out] class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/& message) cil managed
  // SIG: 20 02 02 11 45 10 12 59
  {
    // Method begins at RVA 0x2174
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::TryReceive

  .method /*06000106*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginTryReceive(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                          class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                          object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x2175
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::BeginTryReceive

  .method /*06000107*/ public hidebysig newslot virtual final 
          instance bool  EndTryReceive(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result,
                                       [out] class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/& message) cil managed
  // SIG: 20 02 02 12 79 10 12 59
  {
    // Method begins at RVA 0x2176
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::EndTryReceive

  .method /*06000108*/ public hidebysig newslot virtual final 
          instance bool  WaitForMessage(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 02 11 45
  {
    // Method begins at RVA 0x2177
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::WaitForMessage

  .method /*06000109*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginWaitForMessage(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                              class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                              object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x2178
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::BeginWaitForMessage

  .method /*0600010A*/ public hidebysig newslot virtual final 
          instance bool  EndWaitForMessage(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 02 12 79
  {
    // Method begins at RVA 0x2179
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::EndWaitForMessage

  .method /*0600010B*/ assembly hidebysig strict virtual 
          instance void  FinishEnqueueMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                              class [mscorlib/*23000001*/]System.Action/*0100002D*/ dequeuedCallback,
                                              bool canDispatchOnThisThread) cil managed
  // SIG: 20 03 01 12 59 12 80 B5 02
  {
    // Method begins at RVA 0x217a
    // Code size       0 (0x0)
  } // end of method UdpDuplexChannel::FinishEnqueueMessage

  .property /*17000049*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/
          RemoteAddress()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ System.ServiceModel.Channels.UdpDuplexChannel/*0200001B*/::get_RemoteAddress() /* 060000F9 */
  } // end of property UdpDuplexChannel::RemoteAddress
} // end of class System.ServiceModel.Channels.UdpDuplexChannel

.class /*0200001C*/ private abstract auto ansi beforefieldinit System.ServiceModel.Channels.UdpOutputChannel
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.OutputChannel/*0100003A*/
       implements [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IOutputChannel/*01000039*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannel/*01000024*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.ICommunicationObject/*01000038*/
{
  .method /*0600010C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ChannelManagerBase/*01000026*/ factory,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ encoder,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                               class System.ServiceModel.Channels.UdpSocket/*02000021*/[] sendSockets,
                               class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ retransmissionSettings,
                               class [System/*23000005*/]System.Uri/*01000029*/ via,
                               bool isMulticast) cil managed
  // SIG: 20 07 01 12 80 99 12 65 12 51 1D 12 80 84 12 7C 12 80 A5 02
  {
    // Method begins at RVA 0x217b
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::.ctor

  .method /*0600010D*/ public hidebysig specialname virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ 
          get_RemoteAddress() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x217c
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::get_RemoteAddress

  .method /*0600010E*/ public hidebysig specialname virtual 
          instance class [System/*23000005*/]System.Uri/*01000029*/ 
          get_Via() cil managed
  // SIG: 20 00 12 80 A5
  {
    // Method begins at RVA 0x217d
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::get_Via

  .method /*0600010F*/ assembly hidebysig specialname 
          instance bool  get_IsMulticast() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000071:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x217e
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::get_IsMulticast

  .method /*06000110*/ private hidebysig specialname 
          instance void  set_IsMulticast(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000072:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x217f
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::set_IsMulticast

  .method /*06000111*/ assembly hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_InternalSendTimeout() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x2180
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::get_InternalSendTimeout

  .method /*06000112*/ family hidebysig specialname 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ 
          get_BufferManager() cil managed
  // SIG: 20 00 12 51
  {
    .custom /*0C000073:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2181
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::get_BufferManager

  .method /*06000113*/ private hidebysig specialname 
          instance void  set_BufferManager(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ 'value') cil managed
  // SIG: 20 01 01 12 51
  {
    .custom /*0C000074:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::set_BufferManager

  .method /*06000114*/ family hidebysig specialname 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ 
          get_Encoder() cil managed
  // SIG: 20 00 12 65
  {
    .custom /*0C000075:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2183
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::get_Encoder

  .method /*06000115*/ private hidebysig specialname 
          instance void  set_Encoder(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ 'value') cil managed
  // SIG: 20 01 01 12 65
  {
    .custom /*0C000076:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2184
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::set_Encoder

  .method /*06000116*/ family hidebysig specialname 
          instance class System.ServiceModel.Channels.UdpSocket/*02000021*/[] 
          get_SendSockets() cil managed
  // SIG: 20 00 1D 12 80 84
  {
    .custom /*0C000077:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2185
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::get_SendSockets

  .method /*06000117*/ private hidebysig specialname 
          instance void  set_SendSockets(class System.ServiceModel.Channels.UdpSocket/*02000021*/[] 'value') cil managed
  // SIG: 20 01 01 1D 12 80 84
  {
    .custom /*0C000078:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2186
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::set_SendSockets

  .method /*06000118*/ public hidebysig virtual 
          instance !!T  GetProperty<class T>() cil managed
  // SIG: 30 01 00 1E 00
  {
    // Method begins at RVA 0x2187
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::GetProperty

  .method /*06000119*/ assembly hidebysig 
          instance void  CancelRetransmission(class [System.Runtime.Serialization/*23000004*/]System.Xml.UniqueId/*0100003B*/ messageId) cil managed
  // SIG: 20 01 01 12 80 ED
  {
    // Method begins at RVA 0x2188
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::CancelRetransmission

  .method /*0600011A*/ family hidebysig static 
          void  LogMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/& message,
                           valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> messageData) cil managed
  // SIG: 00 02 01 10 12 59 15 11 4D 01 05
  {
    // Method begins at RVA 0x2189
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::LogMessage

  .method /*0600011B*/ family hidebysig virtual 
          instance void  AddHeadersTo(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x218a
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::AddHeadersTo

  .method /*0600011C*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginSend(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                      valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object state) cil managed
  // SIG: 20 04 12 79 12 59 11 45 12 7D 1C
  {
    // Method begins at RVA 0x218b
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnBeginSend

  .method /*0600011D*/ family hidebysig virtual 
          instance void  OnEndSend(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x218c
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnEndSend

  .method /*0600011E*/ family hidebysig virtual 
          instance void  OnSend(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 02 01 12 59 11 45
  {
    // Method begins at RVA 0x218d
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnSend

  .method /*0600011F*/ family hidebysig newslot abstract virtual 
          instance class System.ServiceModel.Channels.UdpSocket/*02000021*/[] 
          GetSendSockets(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                         [out] class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/& remoteEndPoint,
                         [out] class [mscorlib/*23000001*/]System.Exception/*0100002A*/& exceptionToBeThrown) cil managed
  // SIG: 20 03 1D 12 80 84 12 59 10 12 80 9D 10 12 80 A9
  {
    // Method begins at RVA 0x0
  } // end of method UdpOutputChannel::GetSendSockets

  .method /*06000120*/ family hidebysig virtual 
          instance void  OnAbort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x218e
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnAbort

  .method /*06000121*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginClose(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                       class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                       object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x218f
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnBeginClose

  .method /*06000122*/ family hidebysig virtual 
          instance void  OnClose(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x2190
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnClose

  .method /*06000123*/ family hidebysig virtual 
          instance void  OnEndClose(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2191
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnEndClose

  .method /*06000124*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginOpen(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x2192
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnBeginOpen

  .method /*06000125*/ family hidebysig virtual 
          instance void  OnEndOpen(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2193
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnEndOpen

  .method /*06000126*/ family hidebysig virtual 
          instance void  OnOpen(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x2194
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::OnOpen

  .method /*06000127*/ family hidebysig instance valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> 
          EncodeMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 20 01 15 11 4D 01 05 12 59
  {
    // Method begins at RVA 0x2195
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::EncodeMessage

  .method /*06000128*/ family hidebysig instance class [mscorlib/*23000001*/]System.ObjectDisposedException/*0100003C*/ 
          CreateObjectDisposedException() cil managed
  // SIG: 20 00 12 80 F1
  {
    // Method begins at RVA 0x2196
    // Code size       0 (0x0)
  } // end of method UdpOutputChannel::CreateObjectDisposedException

  .property /*1700004A*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/
          RemoteAddress()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::get_RemoteAddress() /* 0600010D */
  } // end of property UdpOutputChannel::RemoteAddress
  .property /*1700004B*/ instance callconv(8) class [System/*23000005*/]System.Uri/*01000029*/
          Via()
  {
    .get instance class [System/*23000005*/]System.Uri/*01000029*/ System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::get_Via() /* 0600010E */
  } // end of property UdpOutputChannel::Via
  .property /*1700004C*/ instance callconv(8) bool
          IsMulticast()
  {
    .get instance bool System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::get_IsMulticast() /* 0600010F */
    .set instance void System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::set_IsMulticast(bool) /* 06000110 */
  } // end of property UdpOutputChannel::IsMulticast
  .property /*1700004D*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          InternalSendTimeout()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::get_InternalSendTimeout() /* 06000111 */
  } // end of property UdpOutputChannel::InternalSendTimeout
  .property /*1700004E*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/
          BufferManager()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::get_BufferManager() /* 06000112 */
    .set instance void System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::set_BufferManager(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/) /* 06000113 */
  } // end of property UdpOutputChannel::BufferManager
  .property /*1700004F*/ instance callconv(8) class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/
          Encoder()
  {
    .get instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::get_Encoder() /* 06000114 */
    .set instance void System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::set_Encoder(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/) /* 06000115 */
  } // end of property UdpOutputChannel::Encoder
  .property /*17000050*/ instance callconv(8) class System.ServiceModel.Channels.UdpSocket/*02000021*/[]
          SendSockets()
  {
    .get instance class System.ServiceModel.Channels.UdpSocket/*02000021*/[] System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::get_SendSockets() /* 06000116 */
    .set instance void System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/::set_SendSockets(class System.ServiceModel.Channels.UdpSocket/*02000021*/[]) /* 06000117 */
  } // end of property UdpOutputChannel::SendSockets
} // end of class System.ServiceModel.Channels.UdpOutputChannel

.class /*0200001D*/ public auto ansi beforefieldinit System.ServiceModel.Channels.NetworkInterfaceMessageProperty
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*06000129*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 interfaceIndex) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2197
    // Code size       0 (0x0)
  } // end of method NetworkInterfaceMessageProperty::.ctor

  .method /*0600012A*/ public hidebysig specialname static 
          string  get_Name() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2198
    // Code size       0 (0x0)
  } // end of method NetworkInterfaceMessageProperty::get_Name

  .method /*0600012B*/ public hidebysig specialname 
          instance int32  get_InterfaceIndex() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000079:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2199
    // Code size       0 (0x0)
  } // end of method NetworkInterfaceMessageProperty::get_InterfaceIndex

  .method /*0600012C*/ private hidebysig specialname 
          instance void  set_InterfaceIndex(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C00007A:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x219a
    // Code size       0 (0x0)
  } // end of method NetworkInterfaceMessageProperty::set_InterfaceIndex

  .method /*0600012D*/ public hidebysig static 
          bool  TryGet(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                       [out] class System.ServiceModel.Channels.NetworkInterfaceMessageProperty/*0200001D*/& 'property') cil managed
  // SIG: 00 02 02 12 59 10 12 74
  {
    // Method begins at RVA 0x219b
    // Code size       0 (0x0)
  } // end of method NetworkInterfaceMessageProperty::TryGet

  .method /*0600012E*/ public hidebysig static 
          bool  TryGet(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageProperties/*0100003D*/ properties,
                       [out] class System.ServiceModel.Channels.NetworkInterfaceMessageProperty/*0200001D*/& 'property') cil managed
  // SIG: 00 02 02 12 80 F5 10 12 74
  {
    // Method begins at RVA 0x219c
    // Code size       0 (0x0)
  } // end of method NetworkInterfaceMessageProperty::TryGet

  .method /*0600012F*/ public hidebysig instance void 
          AddTo(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 20 01 01 12 59
  {
    // Method begins at RVA 0x219d
    // Code size       0 (0x0)
  } // end of method NetworkInterfaceMessageProperty::AddTo

  .method /*06000130*/ public hidebysig instance void 
          AddTo(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageProperties/*0100003D*/ properties) cil managed
  // SIG: 20 01 01 12 80 F5
  {
    // Method begins at RVA 0x219e
    // Code size       0 (0x0)
  } // end of method NetworkInterfaceMessageProperty::AddTo

  .property /*17000051*/ callconv(8) string
          Name()
  {
    .get string System.ServiceModel.Channels.NetworkInterfaceMessageProperty/*0200001D*/::get_Name() /* 0600012A */
  } // end of property NetworkInterfaceMessageProperty::Name
  .property /*17000052*/ instance callconv(8) int32
          InterfaceIndex()
  {
    .get instance int32 System.ServiceModel.Channels.NetworkInterfaceMessageProperty/*0200001D*/::get_InterfaceIndex() /* 0600012B */
    .set instance void System.ServiceModel.Channels.NetworkInterfaceMessageProperty/*0200001D*/::set_InterfaceIndex(int32) /* 0600012C */
  } // end of property NetworkInterfaceMessageProperty::InterfaceIndex
} // end of class System.ServiceModel.Channels.NetworkInterfaceMessageProperty

.class /*0200001E*/ private auto ansi sealed beforefieldinit System.ServiceModel.Channels.UdpReplyChannel
       extends class System.ServiceModel.Channels.UdpChannelBase`1/*02000014*/<class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.RequestContext/*01000036*/>/*1B00000A*/
       implements [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IReplyChannel/*01000035*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannel/*01000024*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.ICommunicationObject/*01000038*/
{
  .method /*06000131*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpReplyChannelListener/*02000018*/ listener,
                               class System.ServiceModel.Channels.UdpSocket/*02000021*/[] sockets,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.EndpointAddress/*01000028*/ localAddress,
                               class [System/*23000005*/]System.Uri/*01000029*/ via,
                               bool isMulticast) cil managed
  // SIG: 20 05 01 12 60 1D 12 80 84 12 80 A1 12 80 A5 02
  {
    // Method begins at RVA 0x21a0
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::.ctor

  .method /*06000132*/ family hidebysig specialname virtual 
          instance bool  get_IgnoreSerializationException() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21ac
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::get_IgnoreSerializationException

  .method /*06000133*/ public hidebysig virtual 
          instance !!T  GetProperty<class T>() cil managed
  // SIG: 30 01 00 1E 00
  {
    // Method begins at RVA 0x21ad
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::GetProperty

  .method /*06000134*/ public hidebysig newslot virtual final 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.RequestContext/*01000036*/ 
          ReceiveRequest() cil managed
  // SIG: 20 00 12 80 D9
  {
    // Method begins at RVA 0x21ae
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::ReceiveRequest

  .method /*06000135*/ public hidebysig newslot virtual final 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.RequestContext/*01000036*/ 
          ReceiveRequest(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 12 80 D9 11 45
  {
    // Method begins at RVA 0x21af
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::ReceiveRequest

  .method /*06000136*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginReceiveRequest(class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                              object state) cil managed
  // SIG: 20 02 12 79 12 7D 1C
  {
    // Method begins at RVA 0x21b0
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::BeginReceiveRequest

  .method /*06000137*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginReceiveRequest(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                              class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                              object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x21b1
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::BeginReceiveRequest

  .method /*06000138*/ public hidebysig newslot virtual final 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.RequestContext/*01000036*/ 
          EndReceiveRequest(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 12 80 D9 12 79
  {
    // Method begins at RVA 0x21b2
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::EndReceiveRequest

  .method /*06000139*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveRequest(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                                           [out] class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.RequestContext/*01000036*/& context) cil managed
  // SIG: 20 02 02 11 45 10 12 80 D9
  {
    // Method begins at RVA 0x21b3
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::TryReceiveRequest

  .method /*0600013A*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginTryReceiveRequest(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                                 class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                                 object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x21b4
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::BeginTryReceiveRequest

  .method /*0600013B*/ public hidebysig newslot virtual final 
          instance bool  EndTryReceiveRequest(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result,
                                              [out] class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.RequestContext/*01000036*/& context) cil managed
  // SIG: 20 02 02 12 79 10 12 80 D9
  {
    // Method begins at RVA 0x21b5
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::EndTryReceiveRequest

  .method /*0600013C*/ public hidebysig newslot virtual final 
          instance bool  WaitForRequest(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 02 11 45
  {
    // Method begins at RVA 0x21b6
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::WaitForRequest

  .method /*0600013D*/ public hidebysig newslot virtual final 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginWaitForRequest(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                              class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                              object state) cil managed
  // SIG: 20 03 12 79 11 45 12 7D 1C
  {
    // Method begins at RVA 0x21b7
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::BeginWaitForRequest

  .method /*0600013E*/ public hidebysig newslot virtual final 
          instance bool  EndWaitForRequest(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 02 12 79
  {
    // Method begins at RVA 0x21b8
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::EndWaitForRequest

  .method /*0600013F*/ assembly hidebysig strict virtual 
          instance void  FinishEnqueueMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                              class [mscorlib/*23000001*/]System.Action/*0100002D*/ dequeuedCallback,
                                              bool canDispatchOnThisThread) cil managed
  // SIG: 20 03 01 12 59 12 80 B5 02
  {
    // Method begins at RVA 0x21b9
    // Code size       0 (0x0)
  } // end of method UdpReplyChannel::FinishEnqueueMessage

  .property /*17000053*/ instance callconv(8) bool
          IgnoreSerializationException()
  {
    .get instance bool System.ServiceModel.Channels.UdpReplyChannel/*0200001E*/::get_IgnoreSerializationException() /* 06000132 */
  } // end of property UdpReplyChannel::IgnoreSerializationException
} // end of class System.ServiceModel.Channels.UdpReplyChannel

.class /*0200001F*/ public auto ansi sealed beforefieldinit System.ServiceModel.Channels.UdpRetransmissionSettings
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*06000140*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ba
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::.ctor

  .method /*06000141*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 maxUnicastRetransmitCount,
                               int32 maxMulticastRetransmitCount) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x21bb
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::.ctor

  .method /*06000142*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 maxUnicastRetransmitCount,
                               int32 maxMulticastRetransmitCount,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ delayLowerBound,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ delayUpperBound,
                               valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ maxDelayPerRetransmission) cil managed
  // SIG: 20 05 01 08 08 11 45 11 45 11 45
  {
    // Method begins at RVA 0x21bc
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::.ctor

  .method /*06000143*/ public hidebysig specialname 
          instance int32  get_MaxUnicastRetransmitCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21bd
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::get_MaxUnicastRetransmitCount

  .method /*06000144*/ public hidebysig specialname 
          instance void  set_MaxUnicastRetransmitCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21be
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::set_MaxUnicastRetransmitCount

  .method /*06000145*/ public hidebysig specialname 
          instance int32  get_MaxMulticastRetransmitCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21bf
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::get_MaxMulticastRetransmitCount

  .method /*06000146*/ public hidebysig specialname 
          instance void  set_MaxMulticastRetransmitCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21c0
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::set_MaxMulticastRetransmitCount

  .method /*06000147*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_DelayLowerBound() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x21c1
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::get_DelayLowerBound

  .method /*06000148*/ public hidebysig specialname 
          instance void  set_DelayLowerBound(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 'value') cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x21c2
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::set_DelayLowerBound

  .method /*06000149*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_DelayUpperBound() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x21c3
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::get_DelayUpperBound

  .method /*0600014A*/ public hidebysig specialname 
          instance void  set_DelayUpperBound(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 'value') cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x21c4
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::set_DelayUpperBound

  .method /*0600014B*/ public hidebysig specialname 
          instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 
          get_MaxDelayPerRetransmission() cil managed
  // SIG: 20 00 11 45
  {
    // Method begins at RVA 0x21c5
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::get_MaxDelayPerRetransmission

  .method /*0600014C*/ public hidebysig specialname 
          instance void  set_MaxDelayPerRetransmission(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ 'value') cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x21c6
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::set_MaxDelayPerRetransmission

  .method /*0600014D*/ public hidebysig instance bool 
          ShouldSerializeDelayLowerBound() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21c7
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::ShouldSerializeDelayLowerBound

  .method /*0600014E*/ public hidebysig instance bool 
          ShouldSerializeDelayUpperBound() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21c8
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::ShouldSerializeDelayUpperBound

  .method /*0600014F*/ public hidebysig instance bool 
          ShouldSerializeMaxDelayPerRetransmission() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21c9
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::ShouldSerializeMaxDelayPerRetransmission

  .method /*06000150*/ assembly hidebysig 
          instance int32  GetDelayLowerBound() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21ca
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::GetDelayLowerBound

  .method /*06000151*/ assembly hidebysig 
          instance int32  GetDelayUpperBound() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21cb
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::GetDelayUpperBound

  .method /*06000152*/ assembly hidebysig 
          instance int32  GetMaxDelayPerRetransmission() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21cc
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::GetMaxDelayPerRetransmission

  .method /*06000153*/ assembly hidebysig specialname 
          instance bool  get_Enabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21cd
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::get_Enabled

  .method /*06000154*/ assembly hidebysig 
          instance void  ValidateSettings() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ce
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::ValidateSettings

  .method /*06000155*/ assembly hidebysig 
          instance class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ 
          Clone() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x21cf
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::Clone

  .method /*06000156*/ assembly hidebysig 
          instance bool  IsMatch(class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ udpRetransmissionSettings) cil managed
  // SIG: 20 01 02 12 7C
  {
    // Method begins at RVA 0x21d0
    // Code size       0 (0x0)
  } // end of method UdpRetransmissionSettings::IsMatch

  .property /*17000054*/ instance callconv(8) int32
          MaxUnicastRetransmitCount()
  {
    .custom /*0C000052:0A000022*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int32) /* 0A000022 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance int32 System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::get_MaxUnicastRetransmitCount() /* 06000143 */
    .set instance void System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::set_MaxUnicastRetransmitCount(int32) /* 06000144 */
  } // end of property UdpRetransmissionSettings::MaxUnicastRetransmitCount
  .property /*17000055*/ instance callconv(8) int32
          MaxMulticastRetransmitCount()
  {
    .custom /*0C000053:0A000022*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int32) /* 0A000022 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance int32 System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::get_MaxMulticastRetransmitCount() /* 06000145 */
    .set instance void System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::set_MaxMulticastRetransmitCount(int32) /* 06000146 */
  } // end of property UdpRetransmissionSettings::MaxMulticastRetransmitCount
  .property /*17000056*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          DelayLowerBound()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::get_DelayLowerBound() /* 06000147 */
    .set instance void System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::set_DelayLowerBound(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/) /* 06000148 */
  } // end of property UdpRetransmissionSettings::DelayLowerBound
  .property /*17000057*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          DelayUpperBound()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::get_DelayUpperBound() /* 06000149 */
    .set instance void System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::set_DelayUpperBound(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/) /* 0600014A */
  } // end of property UdpRetransmissionSettings::DelayUpperBound
  .property /*17000058*/ instance callconv(8) valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/
          MaxDelayPerRetransmission()
  {
    .get instance valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::get_MaxDelayPerRetransmission() /* 0600014B */
    .set instance void System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::set_MaxDelayPerRetransmission(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/) /* 0600014C */
  } // end of property UdpRetransmissionSettings::MaxDelayPerRetransmission
  .property /*17000059*/ instance callconv(8) bool
          Enabled()
  {
    .get instance bool System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/::get_Enabled() /* 06000153 */
  } // end of property UdpRetransmissionSettings::Enabled
} // end of class System.ServiceModel.Channels.UdpRetransmissionSettings

.class /*02000020*/ private auto ansi beforefieldinit System.ServiceModel.Channels.UdpRequestContext
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.RequestContextBase/*0100003E*/
{
  .method /*06000157*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpOutputChannel/*0200001C*/ outputChannel,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ requestMessage) cil managed
  // SIG: 20 02 01 12 70 12 59
  {
    // Method begins at RVA 0x21d1
    // Code size       0 (0x0)
  } // end of method UdpRequestContext::.ctor

  .method /*06000158*/ family hidebysig virtual 
          instance void  OnAbort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21d2
    // Code size       0 (0x0)
  } // end of method UdpRequestContext::OnAbort

  .method /*06000159*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          OnBeginReply(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                       valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout,
                       class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                       object state) cil managed
  // SIG: 20 04 12 79 12 59 11 45 12 7D 1C
  {
    // Method begins at RVA 0x21d3
    // Code size       0 (0x0)
  } // end of method UdpRequestContext::OnBeginReply

  .method /*0600015A*/ family hidebysig virtual 
          instance void  OnClose(valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 01 01 11 45
  {
    // Method begins at RVA 0x21d4
    // Code size       0 (0x0)
  } // end of method UdpRequestContext::OnClose

  .method /*0600015B*/ family hidebysig virtual 
          instance void  OnEndReply(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x21d5
    // Code size       0 (0x0)
  } // end of method UdpRequestContext::OnEndReply

  .method /*0600015C*/ family hidebysig virtual 
          instance void  OnReply(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                 valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000011*/ timeout) cil managed
  // SIG: 20 02 01 12 59 11 45
  {
    // Method begins at RVA 0x21d6
    // Code size       0 (0x0)
  } // end of method UdpRequestContext::OnReply

} // end of class System.ServiceModel.Channels.UdpRequestContext

.class /*02000021*/ private auto ansi beforefieldinit System.ServiceModel.Channels.UdpSocket
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*0600015D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System/*23000005*/]System.Net.Sockets.Socket/*0100003F*/ socket,
                               int32 interfaceIndex) cil managed
  // SIG: 20 02 01 12 80 FD 08
  {
    // Method begins at RVA 0x21d7
    // Code size       0 (0x0)
  } // end of method UdpSocket::.ctor

  .method /*0600015E*/ public hidebysig specialname 
          instance valuetype [System/*23000005*/]System.Net.Sockets.AddressFamily/*01000040*/ 
          get_AddressFamily() cil managed
  // SIG: 20 00 11 81 01
  {
    // Method begins at RVA 0x21d8
    // Code size       0 (0x0)
  } // end of method UdpSocket::get_AddressFamily

  .method /*0600015F*/ public hidebysig specialname 
          instance int32  get_PendingReceiveCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21d9
    // Code size       0 (0x0)
  } // end of method UdpSocket::get_PendingReceiveCount

  .method /*06000160*/ assembly hidebysig specialname 
          instance int32  get_InterfaceIndex() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00007B:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21da
    // Code size       0 (0x0)
  } // end of method UdpSocket::get_InterfaceIndex

  .method /*06000161*/ private hidebysig specialname 
          instance void  set_InterfaceIndex(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C00007C:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21db
    // Code size       0 (0x0)
  } // end of method UdpSocket::set_InterfaceIndex

  .method /*06000162*/ assembly hidebysig specialname 
          instance bool  get_IsDisposed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21dc
    // Code size       0 (0x0)
  } // end of method UdpSocket::get_IsDisposed

  .method /*06000163*/ assembly hidebysig specialname 
          instance object  get_ThisLock() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00007D:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21dd
    // Code size       0 (0x0)
  } // end of method UdpSocket::get_ThisLock

  .method /*06000164*/ private hidebysig specialname 
          instance void  set_ThisLock(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00007E:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21de
    // Code size       0 (0x0)
  } // end of method UdpSocket::set_ThisLock

  .method /*06000165*/ public hidebysig instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginReceiveFrom(uint8[] buffer,
                           int32 offset,
                           int32 size,
                           class [System/*23000005*/]System.Net.EndPoint/*0100002C*/& remoteEndPoint,
                           class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                           object state) cil managed
  // SIG: 20 06 12 79 1D 05 08 08 10 12 80 B1 12 7D 1C
  {
    // Method begins at RVA 0x21df
    // Code size       0 (0x0)
  } // end of method UdpSocket::BeginReceiveFrom

  .method /*06000166*/ public hidebysig instance void 
          Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e0
    // Code size       0 (0x0)
  } // end of method UdpSocket::Close

  .method /*06000167*/ public hidebysig instance valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> 
          EndReceiveFrom(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result,
                         class [System/*23000005*/]System.Net.EndPoint/*0100002C*/& remoteEndPoint) cil managed
  // SIG: 20 02 15 11 4D 01 05 12 79 10 12 80 B1
  {
    // Method begins at RVA 0x21e1
    // Code size       0 (0x0)
  } // end of method UdpSocket::EndReceiveFrom

  .method /*06000168*/ assembly hidebysig 
          instance class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ 
          CreateIPAnyEndPoint() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x21e2
    // Code size       0 (0x0)
  } // end of method UdpSocket::CreateIPAnyEndPoint

  .method /*06000169*/ public hidebysig instance void 
          Open() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e3
    // Code size       0 (0x0)
  } // end of method UdpSocket::Open

  .method /*0600016A*/ public hidebysig instance int32 
          SendTo(uint8[] buffer,
                 int32 offset,
                 int32 size,
                 class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ remoteEndPoint) cil managed
  // SIG: 20 04 08 1D 05 08 08 12 80 B1
  {
    // Method begins at RVA 0x21e4
    // Code size       0 (0x0)
  } // end of method UdpSocket::SendTo

  .method /*0600016B*/ public hidebysig instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginSendTo(uint8[] buffer,
                      int32 offset,
                      int32 size,
                      class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ remoteEndPoint,
                      class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object state) cil managed
  // SIG: 20 06 12 79 1D 05 08 08 12 80 B1 12 7D 1C
  {
    // Method begins at RVA 0x21e5
    // Code size       0 (0x0)
  } // end of method UdpSocket::BeginSendTo

  .method /*0600016C*/ public hidebysig instance int32 
          EndSendTo(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 08 12 79
  {
    // Method begins at RVA 0x21e6
    // Code size       0 (0x0)
  } // end of method UdpSocket::EndSendTo

  .property /*1700005A*/ instance callconv(8) valuetype [System/*23000005*/]System.Net.Sockets.AddressFamily/*01000040*/
          AddressFamily()
  {
    .get instance valuetype [System/*23000005*/]System.Net.Sockets.AddressFamily/*01000040*/ System.ServiceModel.Channels.UdpSocket/*02000021*/::get_AddressFamily() /* 0600015E */
  } // end of property UdpSocket::AddressFamily
  .property /*1700005B*/ instance callconv(8) int32
          PendingReceiveCount()
  {
    .get instance int32 System.ServiceModel.Channels.UdpSocket/*02000021*/::get_PendingReceiveCount() /* 0600015F */
  } // end of property UdpSocket::PendingReceiveCount
  .property /*1700005C*/ instance callconv(8) int32
          InterfaceIndex()
  {
    .get instance int32 System.ServiceModel.Channels.UdpSocket/*02000021*/::get_InterfaceIndex() /* 06000160 */
    .set instance void System.ServiceModel.Channels.UdpSocket/*02000021*/::set_InterfaceIndex(int32) /* 06000161 */
  } // end of property UdpSocket::InterfaceIndex
  .property /*1700005D*/ instance callconv(8) bool
          IsDisposed()
  {
    .get instance bool System.ServiceModel.Channels.UdpSocket/*02000021*/::get_IsDisposed() /* 06000162 */
  } // end of property UdpSocket::IsDisposed
  .property /*1700005E*/ instance callconv(8) object
          ThisLock()
  {
    .get instance object System.ServiceModel.Channels.UdpSocket/*02000021*/::get_ThisLock() /* 06000163 */
    .set instance void System.ServiceModel.Channels.UdpSocket/*02000021*/::set_ThisLock(object) /* 06000164 */
  } // end of property UdpSocket::ThisLock
} // end of class System.ServiceModel.Channels.UdpSocket

.class /*02000022*/ private auto ansi sealed beforefieldinit System.ServiceModel.Channels.UdpSocketReceiveManager
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .class /*02000033*/ auto ansi nested assembly beforefieldinit UdpSocketReceiveState
         extends [mscorlib/*23000001*/]System.Object/*01000012*/
  {
    .method /*0600026C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class System.ServiceModel.Channels.UdpSocket/*02000021*/ socket,
                                 uint8[] receiveBuffer) cil managed
    // SIG: 20 02 01 12 80 84 1D 05
    {
      // Method begins at RVA 0x22e1
      // Code size       0 (0x0)
    } // end of method UdpSocketReceiveState::.ctor

    .method /*0600026D*/ public hidebysig specialname 
            instance class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ 
            get_RemoteEndPoint() cil managed
    // SIG: 20 00 12 80 B1
    {
      .custom /*0C000089:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x22e2
      // Code size       0 (0x0)
    } // end of method UdpSocketReceiveState::get_RemoteEndPoint

    .method /*0600026E*/ public hidebysig specialname 
            instance void  set_RemoteEndPoint(class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ 'value') cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C00008A:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x22e3
      // Code size       0 (0x0)
    } // end of method UdpSocketReceiveState::set_RemoteEndPoint

    .method /*0600026F*/ assembly hidebysig specialname 
            instance class System.ServiceModel.Channels.UdpSocket/*02000021*/ 
            get_Socket() cil managed
    // SIG: 20 00 12 80 84
    {
      .custom /*0C00008B:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x22e4
      // Code size       0 (0x0)
    } // end of method UdpSocketReceiveState::get_Socket

    .method /*06000270*/ private hidebysig specialname 
            instance void  set_Socket(class System.ServiceModel.Channels.UdpSocket/*02000021*/ 'value') cil managed
    // SIG: 20 01 01 12 80 84
    {
      .custom /*0C00008C:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x22e5
      // Code size       0 (0x0)
    } // end of method UdpSocketReceiveState::set_Socket

    .method /*06000271*/ assembly hidebysig specialname 
            instance uint8[]  get_ReceiveBuffer() cil managed
    // SIG: 20 00 1D 05
    {
      .custom /*0C00008D:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x22e6
      // Code size       0 (0x0)
    } // end of method UdpSocketReceiveState::get_ReceiveBuffer

    .method /*06000272*/ private hidebysig specialname 
            instance void  set_ReceiveBuffer(uint8[] 'value') cil managed
    // SIG: 20 01 01 1D 05
    {
      .custom /*0C00008E:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x22e7
      // Code size       0 (0x0)
    } // end of method UdpSocketReceiveState::set_ReceiveBuffer

    .property /*170000A1*/ instance callconv(8) class [System/*23000005*/]System.Net.EndPoint/*0100002C*/
            RemoteEndPoint()
    {
      .get instance class [System/*23000005*/]System.Net.EndPoint/*0100002C*/ System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*//UdpSocketReceiveState/*02000033*/::get_RemoteEndPoint() /* 0600026D */
      .set instance void System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*//UdpSocketReceiveState/*02000033*/::set_RemoteEndPoint(class [System/*23000005*/]System.Net.EndPoint/*0100002C*/) /* 0600026E */
    } // end of property UdpSocketReceiveState::RemoteEndPoint
    .property /*170000A2*/ instance callconv(8) class System.ServiceModel.Channels.UdpSocket/*02000021*/
            Socket()
    {
      .get instance class System.ServiceModel.Channels.UdpSocket/*02000021*/ System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*//UdpSocketReceiveState/*02000033*/::get_Socket() /* 0600026F */
      .set instance void System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*//UdpSocketReceiveState/*02000033*/::set_Socket(class System.ServiceModel.Channels.UdpSocket/*02000021*/) /* 06000270 */
    } // end of property UdpSocketReceiveState::Socket
    .property /*170000A3*/ instance callconv(8) uint8[]
            ReceiveBuffer()
    {
      .get instance uint8[] System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*//UdpSocketReceiveState/*02000033*/::get_ReceiveBuffer() /* 06000271 */
      .set instance void System.ServiceModel.Channels.UdpSocketReceiveManager/*02000022*//UdpSocketReceiveState/*02000033*/::set_ReceiveBuffer(uint8[]) /* 06000272 */
    } // end of property UdpSocketReceiveState::ReceiveBuffer
  } // end of class UdpSocketReceiveState

  .method /*0600016D*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpSocket/*02000021*/[] receiveSockets,
                               int32 maxPendingReceivesPerSocket,
                               class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                               class System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/ receiveHandler) cil managed
  // SIG: 20 04 01 1D 12 80 84 08 12 51 12 44
  {
    // Method begins at RVA 0x21e7
    // Code size       0 (0x0)
  } // end of method UdpSocketReceiveManager::.ctor

  .method /*0600016E*/ public hidebysig instance void 
          SetReceiveHandler(class System.ServiceModel.Channels.IUdpReceiveHandler/*02000011*/ 'handler') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x21e8
    // Code size       0 (0x0)
  } // end of method UdpSocketReceiveManager::SetReceiveHandler

  .method /*0600016F*/ public hidebysig instance void 
          Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e9
    // Code size       0 (0x0)
  } // end of method UdpSocketReceiveManager::Close

  .method /*06000170*/ public hidebysig instance void 
          Open() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ea
    // Code size       0 (0x0)
  } // end of method UdpSocketReceiveManager::Open

} // end of class System.ServiceModel.Channels.UdpSocketReceiveManager

.class /*02000023*/ public auto ansi beforefieldinit System.ServiceModel.Channels.UdpTransportBindingElement
       extends [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.TransportBindingElement/*0100000F*/
       implements [System.ServiceModel/*23000003*/]System.ServiceModel.Description.IPolicyExportExtension/*01000045*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ITransportPolicyImport/*01000048*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Description.IWsdlExportExtension/*01000041*/
{
  .method /*06000171*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21eb
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::.ctor

  .method /*06000172*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/ other) cil managed
  // SIG: 20 01 01 12 80 8C
  {
    // Method begins at RVA 0x21ec
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::.ctor

  .method /*06000173*/ public hidebysig specialname 
          instance int32  get_DuplicateMessageHistoryLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21ed
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::get_DuplicateMessageHistoryLength

  .method /*06000174*/ public hidebysig specialname 
          instance void  set_DuplicateMessageHistoryLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21ee
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::set_DuplicateMessageHistoryLength

  .method /*06000175*/ public hidebysig specialname 
          instance int64  get_MaxPendingMessagesTotalSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21ef
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::get_MaxPendingMessagesTotalSize

  .method /*06000176*/ public hidebysig specialname 
          instance void  set_MaxPendingMessagesTotalSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x21f0
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::set_MaxPendingMessagesTotalSize

  .method /*06000177*/ public hidebysig specialname 
          instance string  get_MulticastInterfaceId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007F:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21f1
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::get_MulticastInterfaceId

  .method /*06000178*/ public hidebysig specialname 
          instance void  set_MulticastInterfaceId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000080:0A00002B*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000089*/::.ctor() /* 0A00002B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21f2
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::set_MulticastInterfaceId

  .method /*06000179*/ public hidebysig specialname 
          instance class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ 
          get_RetransmissionSettings() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x21f3
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::get_RetransmissionSettings

  .method /*0600017A*/ public hidebysig specialname 
          instance void  set_RetransmissionSettings(class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ 'value') cil managed
  // SIG: 20 01 01 12 7C
  {
    // Method begins at RVA 0x21f4
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::set_RetransmissionSettings

  .method /*0600017B*/ public hidebysig specialname virtual 
          instance string  get_Scheme() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f5
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::get_Scheme

  .method /*0600017C*/ public hidebysig specialname 
          instance int32  get_SocketReceiveBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21f6
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::get_SocketReceiveBufferSize

  .method /*0600017D*/ public hidebysig specialname 
          instance void  set_SocketReceiveBufferSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21f7
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::set_SocketReceiveBufferSize

  .method /*0600017E*/ public hidebysig specialname 
          instance int32  get_TimeToLive() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21f8
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::get_TimeToLive

  .method /*0600017F*/ public hidebysig specialname 
          instance void  set_TimeToLive(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21f9
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::set_TimeToLive

  .method /*06000180*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannelFactory`1/*01000023*/<!!TChannel> 
          BuildChannelFactory<TChannel>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 15 12 80 8D 01 1E 00 12 80 89
  {
    // Method begins at RVA 0x21fa
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::BuildChannelFactory

  .method /*06000181*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannelListener`1/*01000025*/<!!TChannel> 
          BuildChannelListener<class ([System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannel/*01000024*/) TChannel>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 15 12 80 95 01 1E 00 12 80 89
  {
    // Method begins at RVA 0x21fb
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::BuildChannelListener

  .method /*06000182*/ public hidebysig virtual 
          instance bool  CanBuildChannelFactory<TChannel>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 02 12 80 89
  {
    // Method begins at RVA 0x21fc
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::CanBuildChannelFactory

  .method /*06000183*/ public hidebysig virtual 
          instance bool  CanBuildChannelListener<class ([System.ServiceModel/*23000003*/]System.ServiceModel.Channels.IChannel/*01000024*/) TChannel>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 02 12 80 89
  {
    // Method begins at RVA 0x21fd
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::CanBuildChannelListener

  .method /*06000184*/ public hidebysig virtual 
          instance !!T  GetProperty<class T>(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 30 01 01 1E 00 12 80 89
  {
    // Method begins at RVA 0x21fe
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::GetProperty

  .method /*06000185*/ public hidebysig virtual 
          instance class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElement/*0100000C*/ 
          Clone() cil managed
  // SIG: 20 00 12 31
  {
    // Method begins at RVA 0x21ff
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::Clone

  .method /*06000186*/ public hidebysig instance bool 
          ShouldSerializeRetransmissionSettings() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000081:0A000021*/ instance void [System/*23000005*/]System.ComponentModel.EditorBrowsableAttribute/*01000080*/::.ctor(valuetype [System/*23000005*/]System.ComponentModel.EditorBrowsableState/*01000081*/) /* 0A000021 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2200
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::ShouldSerializeRetransmissionSettings

  .method /*06000187*/ private hidebysig newslot virtual final 
          instance void  System.ServiceModel.Description.IWsdlExportExtension.ExportContract(class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.WsdlExporter/*01000042*/ exporter,
                                                                                             class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.WsdlContractConversionContext/*01000043*/ context) cil managed
  // SIG: 20 02 01 12 81 09 12 81 0D
  {
    .override [System.ServiceModel/*23000003*/]System.ServiceModel.Description.IWsdlExportExtension/*01000041*/::ExportContract /*01000041::0A000004*/ 
    // Method begins at RVA 0x2201
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::System.ServiceModel.Description.IWsdlExportExtension.ExportContract

  .method /*06000188*/ private hidebysig newslot virtual final 
          instance void  System.ServiceModel.Description.IWsdlExportExtension.ExportEndpoint(class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.WsdlExporter/*01000042*/ exporter,
                                                                                             class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.WsdlEndpointConversionContext/*01000044*/ context) cil managed
  // SIG: 20 02 01 12 81 09 12 81 11
  {
    .override [System.ServiceModel/*23000003*/]System.ServiceModel.Description.IWsdlExportExtension/*01000041*/::ExportEndpoint /*01000041::0A000005*/ 
    // Method begins at RVA 0x2202
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::System.ServiceModel.Description.IWsdlExportExtension.ExportEndpoint

  .method /*06000189*/ private hidebysig newslot virtual final 
          instance void  System.ServiceModel.Description.IPolicyExportExtension.ExportPolicy(class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.MetadataExporter/*01000046*/ exporter,
                                                                                             class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.PolicyConversionContext/*01000047*/ context) cil managed
  // SIG: 20 02 01 12 81 19 12 81 1D
  {
    .override [System.ServiceModel/*23000003*/]System.ServiceModel.Description.IPolicyExportExtension/*01000045*/::ExportPolicy /*01000045::0A000006*/ 
    // Method begins at RVA 0x2203
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::System.ServiceModel.Description.IPolicyExportExtension.ExportPolicy

  .method /*0600018A*/ private hidebysig newslot virtual final 
          instance void  System.ServiceModel.Channels.ITransportPolicyImport.ImportPolicy(class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.MetadataImporter/*01000049*/ importer,
                                                                                          class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.PolicyConversionContext/*01000047*/ policyContext) cil managed
  // SIG: 20 02 01 12 81 25 12 81 1D
  {
    .override [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.ITransportPolicyImport/*01000048*/::ImportPolicy /*01000048::0A000007*/ 
    // Method begins at RVA 0x2204
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::System.ServiceModel.Channels.ITransportPolicyImport.ImportPolicy

  .method /*0600018B*/ assembly hidebysig strict virtual 
          instance bool  IsMatch(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingElement/*0100000C*/ b) cil managed
  // SIG: 20 01 02 12 31
  {
    // Method begins at RVA 0x2205
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElement::IsMatch

  .property /*1700005F*/ instance callconv(8) int32
          DuplicateMessageHistoryLength()
  {
    .custom /*0C000054:0A000022*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int32) /* 0A000022 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance int32 System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::get_DuplicateMessageHistoryLength() /* 06000173 */
    .set instance void System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::set_DuplicateMessageHistoryLength(int32) /* 06000174 */
  } // end of property UdpTransportBindingElement::DuplicateMessageHistoryLength
  .property /*17000060*/ instance callconv(8) int64
          MaxPendingMessagesTotalSize()
  {
    .custom /*0C000055:0A000023*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int64) /* 0A000023 */ = ( 01 00 00 00 00 00 00 00 00 00 00 00 ) 
    .get instance int64 System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::get_MaxPendingMessagesTotalSize() /* 06000175 */
    .set instance void System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::set_MaxPendingMessagesTotalSize(int64) /* 06000176 */
  } // end of property UdpTransportBindingElement::MaxPendingMessagesTotalSize
  .property /*17000061*/ instance callconv(8) string
          MulticastInterfaceId()
  {
    .custom /*0C000056:0A000024*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(string) /* 0A000024 */ = ( 01 00 FF 00 00 ) 
    .get instance string System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::get_MulticastInterfaceId() /* 06000177 */
    .set instance void System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::set_MulticastInterfaceId(string) /* 06000178 */
  } // end of property UdpTransportBindingElement::MulticastInterfaceId
  .property /*17000062*/ instance callconv(8) class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/
          RetransmissionSettings()
  {
    .get instance class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/ System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::get_RetransmissionSettings() /* 06000179 */
    .set instance void System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::set_RetransmissionSettings(class System.ServiceModel.Channels.UdpRetransmissionSettings/*0200001F*/) /* 0600017A */
  } // end of property UdpTransportBindingElement::RetransmissionSettings
  .property /*17000063*/ instance callconv(8) string
          Scheme()
  {
    .get instance string System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::get_Scheme() /* 0600017B */
  } // end of property UdpTransportBindingElement::Scheme
  .property /*17000064*/ instance callconv(8) int32
          SocketReceiveBufferSize()
  {
    .custom /*0C000057:0A000022*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int32) /* 0A000022 */ = ( 01 00 00 00 01 00 00 00 ) 
    .get instance int32 System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::get_SocketReceiveBufferSize() /* 0600017C */
    .set instance void System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::set_SocketReceiveBufferSize(int32) /* 0600017D */
  } // end of property UdpTransportBindingElement::SocketReceiveBufferSize
  .property /*17000065*/ instance callconv(8) int32
          TimeToLive()
  {
    .custom /*0C000058:0A000022*/ instance void [System/*23000005*/]System.ComponentModel.DefaultValueAttribute/*01000082*/::.ctor(int32) /* 0A000022 */ = ( 01 00 01 00 00 00 00 00 ) 
    .get instance int32 System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::get_TimeToLive() /* 0600017E */
    .set instance void System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/::set_TimeToLive(int32) /* 0600017F */
  } // end of property UdpTransportBindingElement::TimeToLive
} // end of class System.ServiceModel.Channels.UdpTransportBindingElement

.class /*02000024*/ private abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.UdpTransportBindingElementPolicy
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*0600018C*/ assembly hidebysig static 
          void  ExportRetransmissionEnabledAssertion(class System.ServiceModel.Channels.UdpTransportBindingElement/*02000023*/ bindingElement,
                                                     class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.PolicyAssertionCollection/*0100004A*/ assertions) cil managed
  // SIG: 00 02 01 12 80 8C 12 81 29
  {
    // Method begins at RVA 0x2206
    // Code size       0 (0x0)
  } // end of method UdpTransportBindingElementPolicy::ExportRetransmissionEnabledAssertion

} // end of class System.ServiceModel.Channels.UdpTransportBindingElementPolicy

.class /*02000025*/ public auto ansi beforefieldinit System.ServiceModel.Channels.UdpTransportImporter
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
       implements [System.ServiceModel/*23000003*/]System.ServiceModel.Description.IPolicyImportExtension/*0100004B*/,
                  [System.ServiceModel/*23000003*/]System.ServiceModel.Description.IWsdlImportExtension/*0100004C*/
{
  .method /*0600018D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2207
    // Code size       0 (0x0)
  } // end of method UdpTransportImporter::.ctor

  .method /*0600018E*/ public hidebysig newslot virtual final 
          instance void  BeforeImport(class [System.Web.Services/*2300000A*/]System.Web.Services.Description.ServiceDescriptionCollection/*0100004D*/ wsdlDocuments,
                                      class [System.Xml/*23000009*/]System.Xml.Schema.XmlSchemaSet/*0100004E*/ xmlSchemas,
                                      class [mscorlib/*23000001*/]System.Collections.Generic.ICollection`1/*0100004F*/<class [System.Xml/*23000009*/]System.Xml.XmlElement/*01000050*/> policy) cil managed
  // SIG: 20 03 01 12 81 35 12 81 39 15 12 81 3D 01 12 81 41
  {
    // Method begins at RVA 0x2208
    // Code size       0 (0x0)
  } // end of method UdpTransportImporter::BeforeImport

  .method /*0600018F*/ public hidebysig newslot virtual final 
          instance void  ImportPolicy(class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.MetadataImporter/*01000049*/ importer,
                                      class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.PolicyConversionContext/*01000047*/ context) cil managed
  // SIG: 20 02 01 12 81 25 12 81 1D
  {
    // Method begins at RVA 0x2209
    // Code size       0 (0x0)
  } // end of method UdpTransportImporter::ImportPolicy

  .method /*06000190*/ public hidebysig newslot virtual final 
          instance void  ImportContract(class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.WsdlImporter/*01000051*/ importer,
                                        class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.WsdlContractConversionContext/*01000043*/ context) cil managed
  // SIG: 20 02 01 12 81 45 12 81 0D
  {
    // Method begins at RVA 0x220a
    // Code size       0 (0x0)
  } // end of method UdpTransportImporter::ImportContract

  .method /*06000191*/ public hidebysig newslot virtual final 
          instance void  ImportEndpoint(class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.WsdlImporter/*01000051*/ importer,
                                        class [System.ServiceModel/*23000003*/]System.ServiceModel.Description.WsdlEndpointConversionContext/*01000044*/ context) cil managed
  // SIG: 20 02 01 12 81 45 12 81 11
  {
    // Method begins at RVA 0x220b
    // Code size       0 (0x0)
  } // end of method UdpTransportImporter::ImportEndpoint

} // end of class System.ServiceModel.Channels.UdpTransportImporter

.class /*02000026*/ private abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.UdpUtility
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*06000192*/ public hidebysig static 
          class [System/*23000005*/]System.Uri/*01000029*/ 
          AppendRelativePath(class [System/*23000005*/]System.Uri/*01000029*/ basepath,
                             string relativePath) cil managed
  // SIG: 00 02 12 80 A5 12 80 A5 0E
  {
    // Method begins at RVA 0x220c
    // Code size       0 (0x0)
  } // end of method UdpUtility::AppendRelativePath

  .method /*06000193*/ public hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoderFactory/*01000020*/ 
          GetEncoder(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BindingContext/*01000022*/ context) cil managed
  // SIG: 00 01 12 80 81 12 80 89
  {
    // Method begins at RVA 0x220d
    // Code size       0 (0x0)
  } // end of method UdpUtility::GetEncoder

  .method /*06000194*/ assembly hidebysig static 
          bool  CanIgnoreServerException(class [mscorlib/*23000001*/]System.Exception/*0100002A*/ ex) cil managed
  // SIG: 00 01 02 12 80 A9
  {
    // Method begins at RVA 0x220e
    // Code size       0 (0x0)
  } // end of method UdpUtility::CanIgnoreServerException

  .method /*06000195*/ public hidebysig static 
          void  CheckSocketSupport([out] bool& ipV4Supported,
                                   [out] bool& ipV6Supported) cil managed
  // SIG: 00 02 01 10 02 10 02
  {
    // Method begins at RVA 0x220f
    // Code size       0 (0x0)
  } // end of method UdpUtility::CheckSocketSupport

  .method /*06000196*/ public hidebysig static 
          bool  TryGetLoopbackInterfaceIndex(class [System/*23000005*/]System.Net.NetworkInformation.NetworkInterface/*01000052*/ adapter,
                                             bool ipv4,
                                             [out] int32& interfaceIndex) cil managed
  // SIG: 00 03 02 12 81 49 02 10 08
  {
    // Method begins at RVA 0x2210
    // Code size       0 (0x0)
  } // end of method UdpUtility::TryGetLoopbackInterfaceIndex

  .method /*06000197*/ public hidebysig static 
          class System.ServiceModel.Channels.UdpSocket/*02000021*/ 
          CreateUnicastListenSocket(class [System/*23000005*/]System.Net.IPAddress/*0100002E*/ ipAddress,
                                    int32& port,
                                    int32 receiveBufferSize,
                                    int32 timeToLive) cil managed
  // SIG: 00 04 12 80 84 12 80 B9 10 08 08 08
  {
    // Method begins at RVA 0x2211
    // Code size       0 (0x0)
  } // end of method UdpUtility::CreateUnicastListenSocket

  .method /*06000198*/ public hidebysig static 
          class System.ServiceModel.Channels.UdpSocket/*02000021*/ 
          CreateListenSocket(class [System/*23000005*/]System.Net.IPAddress/*0100002E*/ ipAddress,
                             int32& port,
                             int32 receiveBufferSize,
                             int32 timeToLive,
                             int32 interfaceIndex,
                             bool allowMulticastLoopback,
                             bool isLoopbackAdapter) cil managed
  // SIG: 00 07 12 80 84 12 80 B9 10 08 08 08 08 02 02
  {
    // Method begins at RVA 0x2212
    // Code size       0 (0x0)
  } // end of method UdpUtility::CreateListenSocket

  .method /*06000199*/ public hidebysig static 
          int32  CreateListenSocketsOnUniquePort(class [System/*23000005*/]System.Net.IPAddress/*0100002E*/ ipv4Address,
                                                 class [System/*23000005*/]System.Net.IPAddress/*0100002E*/ ipv6Address,
                                                 int32 receiveBufferSize,
                                                 int32 timeToLive,
                                                 [out] class System.ServiceModel.Channels.UdpSocket/*02000021*/& ipv4Socket,
                                                 [out] class System.ServiceModel.Channels.UdpSocket/*02000021*/& ipv6Socket) cil managed
  // SIG: 00 06 08 12 80 B9 12 80 B9 08 08 10 12 80 84 10 12 80 84
  {
    // Method begins at RVA 0x2213
    // Code size       0 (0x0)
  } // end of method UdpUtility::CreateListenSocketsOnUniquePort

  .method /*0600019A*/ public hidebysig static 
          void  ValidateDuplicateDetectionAndRetransmittionSupport(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoderFactory/*01000020*/ messageEncoderFactory,
                                                                   bool retransmissionEnabled,
                                                                   bool duplicateDetectionEnabled) cil managed
  // SIG: 00 03 01 12 80 81 02 02
  {
    // Method begins at RVA 0x2214
    // Code size       0 (0x0)
  } // end of method UdpUtility::ValidateDuplicateDetectionAndRetransmittionSupport

  .method /*0600019B*/ public hidebysig static 
          void  ThrowIfNoSocketSupport() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2215
    // Code size       0 (0x0)
  } // end of method UdpUtility::ThrowIfNoSocketSupport

  .method /*0600019C*/ public hidebysig static 
          void  ThrowOnUnsupportedHostNameType(class [System/*23000005*/]System.Uri/*01000029*/ uri) cil managed
  // SIG: 00 01 01 12 80 A5
  {
    // Method begins at RVA 0x2216
    // Code size       0 (0x0)
  } // end of method UdpUtility::ThrowOnUnsupportedHostNameType

  .method /*0600019D*/ public hidebysig static 
          class [System/*23000005*/]System.Net.NetworkInformation.NetworkInterface/*01000052*/[] 
          GetMulticastInterfaces(string multicastInterfaceIdentifier) cil managed
  // SIG: 00 01 1D 12 81 49 0E
  {
    // Method begins at RVA 0x2217
    // Code size       0 (0x0)
  } // end of method UdpUtility::GetMulticastInterfaces

  .method /*0600019E*/ public hidebysig static 
          bool  IsMulticastAddress(class [System/*23000005*/]System.Net.IPAddress/*0100002E*/ address) cil managed
  // SIG: 00 01 02 12 80 B9
  {
    // Method begins at RVA 0x2218
    // Code size       0 (0x0)
  } // end of method UdpUtility::IsMulticastAddress

  .method /*0600019F*/ public hidebysig static 
          bool  IsSuitableForMulticast(class [System/*23000005*/]System.Net.NetworkInformation.NetworkInterface/*01000052*/ networkInterface) cil managed
  // SIG: 00 01 02 12 81 49
  {
    // Method begins at RVA 0x2219
    // Code size       0 (0x0)
  } // end of method UdpUtility::IsSuitableForMulticast

  .method /*060001A0*/ public hidebysig static 
          bool  IsSupportedHostNameType(valuetype [System/*23000005*/]System.UriHostNameType/*01000053*/ hostNameType) cil managed
  // SIG: 00 01 02 11 81 4D
  {
    // Method begins at RVA 0x221a
    // Code size       0 (0x0)
  } // end of method UdpUtility::IsSupportedHostNameType

  .method /*060001A1*/ public hidebysig static 
          void  ValidateBufferBounds(uint8[] buffer,
                                     int32 offset,
                                     int32 size) cil managed
  // SIG: 00 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x221b
    // Code size       0 (0x0)
  } // end of method UdpUtility::ValidateBufferBounds

  .method /*060001A2*/ public hidebysig static 
          void  ValidateBufferBounds(int32 bufferSize,
                                     int32 offset,
                                     int32 size) cil managed
  // SIG: 00 03 01 08 08 08
  {
    // Method begins at RVA 0x221c
    // Code size       0 (0x0)
  } // end of method UdpUtility::ValidateBufferBounds

  .method /*060001A3*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Exception/*0100002A*/ 
          WrapAsyncException(class [mscorlib/*23000001*/]System.Exception/*0100002A*/ ex) cil managed
  // SIG: 00 01 12 80 A9 12 80 A9
  {
    // Method begins at RVA 0x221d
    // Code size       0 (0x0)
  } // end of method UdpUtility::WrapAsyncException

  .method /*060001A4*/ public hidebysig static 
          bool  TryGetSocketError(class [mscorlib/*23000001*/]System.Exception/*0100002A*/ ex,
                                  [out] valuetype [System/*23000005*/]System.Net.Sockets.SocketError/*01000054*/& 'error') cil managed
  // SIG: 00 02 02 12 80 A9 10 11 81 51
  {
    // Method begins at RVA 0x221e
    // Code size       0 (0x0)
  } // end of method UdpUtility::TryGetSocketError

  .method /*060001A5*/ public hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          DecodeMessage(class System.ServiceModel.Channels.DuplicateMessageDetector/*02000010*/ duplicateDetector,
                        class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.MessageEncoder/*01000019*/ encoder,
                        class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.BufferManager/*01000014*/ bufferManager,
                        valuetype [mscorlib/*23000001*/]System.ArraySegment`1/*01000013*/<uint8> data,
                        class [System/*23000005*/]System.Net.IPEndPoint/*01000027*/ remoteEndPoint,
                        int32 interfaceIndex,
                        bool ignoreSerializationException,
                        [out] string& messageHash) cil managed
  // SIG: 00 08 12 59 12 40 12 65 12 51 15 11 4D 01 05 12 80 9D 08 02 10 0E
  {
    // Method begins at RVA 0x221f
    // Code size       0 (0x0)
  } // end of method UdpUtility::DecodeMessage

  .method /*060001A6*/ public hidebysig static 
          int32  ComputeMessageBufferSize(int32 maxReceivedMessageSize) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x2220
    // Code size       0 (0x0)
  } // end of method UdpUtility::ComputeMessageBufferSize

} // end of class System.ServiceModel.Channels.UdpUtility

.class /*02000027*/ private abstract auto ansi beforefieldinit System.ServiceModel.Channels.XmlByteStreamReader
       extends [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReader/*01000055*/
{
  .class /*02000034*/ auto ansi sealed nested family ReaderPosition
         extends [mscorlib/*23000001*/]System.Enum/*01000059*/
  {
    .field /*0400003C*/ public specialname rtspecialname int32 value__
    .field /*0400003D*/ public static literal valuetype System.ServiceModel.Channels.XmlByteStreamReader/*02000027*//ReaderPosition/*02000034*/ None = int32(0x00000000)
    .field /*0400003E*/ public static literal valuetype System.ServiceModel.Channels.XmlByteStreamReader/*02000027*//ReaderPosition/*02000034*/ StartElement = int32(0x00000001)
    .field /*0400003F*/ public static literal valuetype System.ServiceModel.Channels.XmlByteStreamReader/*02000027*//ReaderPosition/*02000034*/ Content = int32(0x00000002)
    .field /*04000040*/ public static literal valuetype System.ServiceModel.Channels.XmlByteStreamReader/*02000027*//ReaderPosition/*02000034*/ EndElement = int32(0x00000003)
    .field /*04000041*/ public static literal valuetype System.ServiceModel.Channels.XmlByteStreamReader/*02000027*//ReaderPosition/*02000034*/ EOF = int32(0x00000004)
  } // end of class ReaderPosition

  .field /*04000022*/ family valuetype System.ServiceModel.Channels.XmlByteStreamReader/*02000027*//ReaderPosition/*02000034*/ position
  .field /*04000023*/ family class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas
  .method /*060001A7*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x2221
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::.ctor

  .method /*060001A8*/ public hidebysig specialname virtual 
          instance int32  get_AttributeCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2222
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_AttributeCount

  .method /*060001A9*/ public hidebysig specialname virtual 
          instance string  get_BaseURI() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2223
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_BaseURI

  .method /*060001AA*/ public hidebysig specialname virtual 
          instance bool  get_CanCanonicalize() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2224
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_CanCanonicalize

  .method /*060001AB*/ public hidebysig specialname virtual 
          instance bool  get_CanReadBinaryContent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2225
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_CanReadBinaryContent

  .method /*060001AC*/ public hidebysig specialname virtual 
          instance bool  get_CanReadValueChunk() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2226
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_CanReadValueChunk

  .method /*060001AD*/ public hidebysig specialname virtual 
          instance bool  get_CanResolveEntity() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2227
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_CanResolveEntity

  .method /*060001AE*/ public hidebysig specialname virtual 
          instance int32  get_Depth() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2228
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_Depth

  .method /*060001AF*/ public hidebysig specialname virtual 
          instance bool  get_EOF() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2229
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_EOF

  .method /*060001B0*/ public hidebysig specialname virtual 
          instance bool  get_HasAttributes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x222a
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_HasAttributes

  .method /*060001B1*/ public hidebysig specialname virtual 
          instance bool  get_HasValue() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x222b
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_HasValue

  .method /*060001B2*/ public hidebysig specialname virtual 
          instance bool  get_IsDefault() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x222c
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_IsDefault

  .method /*060001B3*/ public hidebysig specialname virtual 
          instance bool  get_IsEmptyElement() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x222d
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_IsEmptyElement

  .method /*060001B4*/ public hidebysig specialname virtual 
          instance string  get_LocalName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x222e
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_LocalName

  .method /*060001B5*/ public hidebysig virtual 
          instance void  MoveToStartElement() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x222f
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::MoveToStartElement

  .method /*060001B6*/ public hidebysig specialname virtual 
          instance class [System.Xml/*23000009*/]System.Xml.XmlNameTable/*01000056*/ 
          get_NameTable() cil managed
  // SIG: 20 00 12 81 59
  {
    // Method begins at RVA 0x2230
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_NameTable

  .method /*060001B7*/ public hidebysig specialname virtual 
          instance string  get_NamespaceURI() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2231
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_NamespaceURI

  .method /*060001B8*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000009*/]System.Xml.XmlNodeType/*01000057*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 81 5D
  {
    // Method begins at RVA 0x2232
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_NodeType

  .method /*060001B9*/ public hidebysig specialname virtual 
          instance string  get_Prefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2233
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_Prefix

  .method /*060001BA*/ public hidebysig specialname virtual 
          instance class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ 
          get_Quotas() cil managed
  // SIG: 20 00 12 0D
  {
    // Method begins at RVA 0x2234
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_Quotas

  .method /*060001BB*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000009*/]System.Xml.ReadState/*01000058*/ 
          get_ReadState() cil managed
  // SIG: 20 00 11 81 61
  {
    // Method begins at RVA 0x2235
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_ReadState

  .method /*060001BC*/ public hidebysig specialname virtual 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2236
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_Value

  .method /*060001BD*/ public hidebysig virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2237
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::Close

  .method /*060001BE*/ family hidebysig specialname 
          instance bool  get_IsClosed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2238
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::get_IsClosed

  .method /*060001BF*/ family hidebysig newslot virtual 
          instance void  OnClose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2239
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::OnClose

  .method /*060001C0*/ public hidebysig virtual 
          instance string  GetAttribute(int32 i) cil managed
  // SIG: 20 01 0E 08
  {
    // Method begins at RVA 0x223a
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::GetAttribute

  .method /*060001C1*/ public hidebysig virtual 
          instance string  GetAttribute(string name,
                                        string namespaceURI) cil managed
  // SIG: 20 02 0E 0E 0E
  {
    // Method begins at RVA 0x223b
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::GetAttribute

  .method /*060001C2*/ public hidebysig virtual 
          instance string  GetAttribute(string name) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x223c
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::GetAttribute

  .method /*060001C3*/ public hidebysig virtual 
          instance string  LookupNamespace(string prefix) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x223d
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::LookupNamespace

  .method /*060001C4*/ public hidebysig virtual 
          instance bool  MoveToAttribute(string name,
                                         string ns) cil managed
  // SIG: 20 02 02 0E 0E
  {
    // Method begins at RVA 0x223e
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::MoveToAttribute

  .method /*060001C5*/ public hidebysig virtual 
          instance bool  MoveToAttribute(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x223f
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::MoveToAttribute

  .method /*060001C6*/ public hidebysig virtual 
          instance bool  MoveToElement() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2240
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::MoveToElement

  .method /*060001C7*/ public hidebysig virtual 
          instance bool  MoveToFirstAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2241
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::MoveToFirstAttribute

  .method /*060001C8*/ public hidebysig virtual 
          instance bool  MoveToNextAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2242
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::MoveToNextAttribute

  .method /*060001C9*/ public hidebysig virtual 
          instance bool  Read() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2243
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::Read

  .method /*060001CA*/ public hidebysig virtual 
          instance bool  ReadAttributeValue() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2244
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::ReadAttributeValue

  .method /*060001CB*/ public hidebysig abstract virtual 
          instance int32  ReadContentAsBase64(uint8[] buffer,
                                              int32 index,
                                              int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x0
  } // end of method XmlByteStreamReader::ReadContentAsBase64

  .method /*060001CC*/ public hidebysig virtual 
          instance int32  ReadContentAsBinHex(uint8[] buffer,
                                              int32 index,
                                              int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x2245
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::ReadContentAsBinHex

  .method /*060001CD*/ public hidebysig virtual 
          instance void  ResolveEntity() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2246
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::ResolveEntity

  .method /*060001CE*/ public hidebysig instance uint8[] 
          ToByteArray() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2247
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::ToByteArray

  .method /*060001CF*/ family hidebysig newslot abstract virtual 
          instance uint8[]  OnToByteArray() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method XmlByteStreamReader::OnToByteArray

  .method /*060001D0*/ public hidebysig instance class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 
          ToStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2248
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::ToStream

  .method /*060001D1*/ family hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 
          OnToStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x0
  } // end of method XmlByteStreamReader::OnToStream

  .method /*060001D2*/ family hidebysig instance void 
          EnsureInContent() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2249
    // Code size       0 (0x0)
  } // end of method XmlByteStreamReader::EnsureInContent

  .property /*17000066*/ instance callconv(8) int32
          AttributeCount()
  {
    .get instance int32 System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_AttributeCount() /* 060001A8 */
  } // end of property XmlByteStreamReader::AttributeCount
  .property /*17000067*/ instance callconv(8) string
          BaseURI()
  {
    .get instance string System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_BaseURI() /* 060001A9 */
  } // end of property XmlByteStreamReader::BaseURI
  .property /*17000068*/ instance callconv(8) bool
          CanCanonicalize()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_CanCanonicalize() /* 060001AA */
  } // end of property XmlByteStreamReader::CanCanonicalize
  .property /*17000069*/ instance callconv(8) bool
          CanReadBinaryContent()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_CanReadBinaryContent() /* 060001AB */
  } // end of property XmlByteStreamReader::CanReadBinaryContent
  .property /*1700006A*/ instance callconv(8) bool
          CanReadValueChunk()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_CanReadValueChunk() /* 060001AC */
  } // end of property XmlByteStreamReader::CanReadValueChunk
  .property /*1700006B*/ instance callconv(8) bool
          CanResolveEntity()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_CanResolveEntity() /* 060001AD */
  } // end of property XmlByteStreamReader::CanResolveEntity
  .property /*1700006C*/ instance callconv(8) int32
          Depth()
  {
    .get instance int32 System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_Depth() /* 060001AE */
  } // end of property XmlByteStreamReader::Depth
  .property /*1700006D*/ instance callconv(8) bool
          EOF()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_EOF() /* 060001AF */
  } // end of property XmlByteStreamReader::EOF
  .property /*1700006E*/ instance callconv(8) bool
          HasAttributes()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_HasAttributes() /* 060001B0 */
  } // end of property XmlByteStreamReader::HasAttributes
  .property /*1700006F*/ instance callconv(8) bool
          HasValue()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_HasValue() /* 060001B1 */
  } // end of property XmlByteStreamReader::HasValue
  .property /*17000070*/ instance callconv(8) bool
          IsDefault()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_IsDefault() /* 060001B2 */
  } // end of property XmlByteStreamReader::IsDefault
  .property /*17000071*/ instance callconv(8) bool
          IsEmptyElement()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_IsEmptyElement() /* 060001B3 */
  } // end of property XmlByteStreamReader::IsEmptyElement
  .property /*17000072*/ instance callconv(8) string
          LocalName()
  {
    .get instance string System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_LocalName() /* 060001B4 */
  } // end of property XmlByteStreamReader::LocalName
  .property /*17000073*/ instance callconv(8) class [System.Xml/*23000009*/]System.Xml.XmlNameTable/*01000056*/
          NameTable()
  {
    .get instance class [System.Xml/*23000009*/]System.Xml.XmlNameTable/*01000056*/ System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_NameTable() /* 060001B6 */
  } // end of property XmlByteStreamReader::NameTable
  .property /*17000074*/ instance callconv(8) string
          NamespaceURI()
  {
    .get instance string System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_NamespaceURI() /* 060001B7 */
  } // end of property XmlByteStreamReader::NamespaceURI
  .property /*17000075*/ instance callconv(8) valuetype [System.Xml/*23000009*/]System.Xml.XmlNodeType/*01000057*/
          NodeType()
  {
    .get instance valuetype [System.Xml/*23000009*/]System.Xml.XmlNodeType/*01000057*/ System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_NodeType() /* 060001B8 */
  } // end of property XmlByteStreamReader::NodeType
  .property /*17000076*/ instance callconv(8) string
          Prefix()
  {
    .get instance string System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_Prefix() /* 060001B9 */
  } // end of property XmlByteStreamReader::Prefix
  .property /*17000077*/ instance callconv(8) class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/
          Quotas()
  {
    .get instance class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_Quotas() /* 060001BA */
  } // end of property XmlByteStreamReader::Quotas
  .property /*17000078*/ instance callconv(8) valuetype [System.Xml/*23000009*/]System.Xml.ReadState/*01000058*/
          ReadState()
  {
    .get instance valuetype [System.Xml/*23000009*/]System.Xml.ReadState/*01000058*/ System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_ReadState() /* 060001BB */
  } // end of property XmlByteStreamReader::ReadState
  .property /*17000079*/ instance callconv(8) string
          Value()
  {
    .get instance string System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_Value() /* 060001BC */
  } // end of property XmlByteStreamReader::Value
  .property /*1700007A*/ instance callconv(8) bool
          IsClosed()
  {
    .get instance bool System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/::get_IsClosed() /* 060001BE */
  } // end of property XmlByteStreamReader::IsClosed
} // end of class System.ServiceModel.Channels.XmlByteStreamReader

.class /*02000028*/ private auto ansi sealed beforefieldinit System.ServiceModel.Channels.XmlByteStreamWriter
       extends [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryWriter/*0100005A*/
{
  .method /*060001D3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                               bool ownsStream) cil managed
  // SIG: 20 02 01 12 55 02
  {
    // Method begins at RVA 0x224a
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::.ctor

  .method /*060001D4*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml/*23000009*/]System.Xml.WriteState/*0100005B*/ 
          get_WriteState() cil managed
  // SIG: 20 00 11 81 6D
  {
    // Method begins at RVA 0x224b
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::get_WriteState

  .method /*060001D5*/ public hidebysig specialname virtual 
          instance class [System.Xml/*23000009*/]System.Xml.XmlWriterSettings/*0100005C*/ 
          get_Settings() cil managed
  // SIG: 20 00 12 81 71
  {
    // Method begins at RVA 0x224c
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::get_Settings

  .method /*060001D6*/ public hidebysig virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x224d
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::Close

  .method /*060001D7*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x224e
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::Flush

  .method /*060001D8*/ public hidebysig virtual 
          instance string  LookupPrefix(string ns) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x224f
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::LookupPrefix

  .method /*060001D9*/ public hidebysig virtual 
          instance void  WriteBase64(uint8[] buffer,
                                     int32 index,
                                     int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2250
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteBase64

  .method /*060001DA*/ public hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.Threading.Tasks.Task/*0100005D*/ 
          WriteBase64Async(uint8[] buffer,
                           int32 index,
                           int32 count) cil managed
  // SIG: 20 03 12 81 75 1D 05 08 08
  {
    // Method begins at RVA 0x2251
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteBase64Async

  .method /*060001DB*/ assembly hidebysig 
          instance class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginWriteBase64(uint8[] buffer,
                           int32 index,
                           int32 count,
                           class [mscorlib/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                           object state) cil managed
  // SIG: 20 05 12 79 1D 05 08 08 12 7D 1C
  {
    // Method begins at RVA 0x2252
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::BeginWriteBase64

  .method /*060001DC*/ assembly hidebysig 
          instance void  EndWriteBase64(class [mscorlib/*23000001*/]System.IAsyncResult/*0100001E*/ result) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2253
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::EndWriteBase64

  .method /*060001DD*/ public hidebysig virtual 
          instance void  WriteCData(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2254
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteCData

  .method /*060001DE*/ public hidebysig virtual 
          instance void  WriteCharEntity(char ch) cil managed
  // SIG: 20 01 01 03
  {
    // Method begins at RVA 0x2255
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteCharEntity

  .method /*060001DF*/ public hidebysig virtual 
          instance void  WriteChars(char[] buffer,
                                    int32 index,
                                    int32 count) cil managed
  // SIG: 20 03 01 1D 03 08 08
  {
    // Method begins at RVA 0x2256
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteChars

  .method /*060001E0*/ public hidebysig virtual 
          instance void  WriteComment(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2257
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteComment

  .method /*060001E1*/ public hidebysig virtual 
          instance void  WriteDocType(string name,
                                      string pubid,
                                      string sysid,
                                      string subset) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    // Method begins at RVA 0x2258
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteDocType

  .method /*060001E2*/ public hidebysig virtual 
          instance void  WriteEndAttribute() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2259
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteEndAttribute

  .method /*060001E3*/ public hidebysig virtual 
          instance void  WriteEndDocument() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225a
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteEndDocument

  .method /*060001E4*/ public hidebysig virtual 
          instance void  WriteEndElement() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225b
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteEndElement

  .method /*060001E5*/ public hidebysig virtual 
          instance void  WriteEntityRef(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x225c
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteEntityRef

  .method /*060001E6*/ public hidebysig virtual 
          instance void  WriteFullEndElement() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225d
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteFullEndElement

  .method /*060001E7*/ public hidebysig virtual 
          instance void  WriteProcessingInstruction(string name,
                                                    string text) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x225e
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteProcessingInstruction

  .method /*060001E8*/ public hidebysig virtual 
          instance void  WriteRaw(string data) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x225f
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteRaw

  .method /*060001E9*/ public hidebysig virtual 
          instance void  WriteRaw(char[] buffer,
                                  int32 index,
                                  int32 count) cil managed
  // SIG: 20 03 01 1D 03 08 08
  {
    // Method begins at RVA 0x2260
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteRaw

  .method /*060001EA*/ public hidebysig virtual 
          instance void  WriteStartAttribute(string prefix,
                                             string localName,
                                             string ns) cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    // Method begins at RVA 0x2261
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteStartAttribute

  .method /*060001EB*/ public hidebysig virtual 
          instance void  WriteStartDocument(bool standalone) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2262
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteStartDocument

  .method /*060001EC*/ public hidebysig virtual 
          instance void  WriteStartDocument() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2263
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteStartDocument

  .method /*060001ED*/ public hidebysig virtual 
          instance void  WriteStartElement(string prefix,
                                           string localName,
                                           string ns) cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    // Method begins at RVA 0x2264
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteStartElement

  .method /*060001EE*/ public hidebysig virtual 
          instance void  WriteString(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2265
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteString

  .method /*060001EF*/ public hidebysig virtual 
          instance void  WriteSurrogateCharEntity(char lowChar,
                                                  char highChar) cil managed
  // SIG: 20 02 01 03 03
  {
    // Method begins at RVA 0x2266
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteSurrogateCharEntity

  .method /*060001F0*/ public hidebysig virtual 
          instance void  WriteWhitespace(string ws) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2267
    // Code size       0 (0x0)
  } // end of method XmlByteStreamWriter::WriteWhitespace

  .property /*1700007B*/ instance callconv(8) valuetype [System.Xml/*23000009*/]System.Xml.WriteState/*0100005B*/
          WriteState()
  {
    .get instance valuetype [System.Xml/*23000009*/]System.Xml.WriteState/*0100005B*/ System.ServiceModel.Channels.XmlByteStreamWriter/*02000028*/::get_WriteState() /* 060001D4 */
  } // end of property XmlByteStreamWriter::WriteState
  .property /*1700007C*/ instance callconv(8) class [System.Xml/*23000009*/]System.Xml.XmlWriterSettings/*0100005C*/
          Settings()
  {
    .get instance class [System.Xml/*23000009*/]System.Xml.XmlWriterSettings/*0100005C*/ System.ServiceModel.Channels.XmlByteStreamWriter/*02000028*/::get_Settings() /* 060001D5 */
  } // end of property XmlByteStreamWriter::Settings
} // end of class System.ServiceModel.Channels.XmlByteStreamWriter

.class /*02000029*/ private auto ansi beforefieldinit System.ServiceModel.Channels.XmlBufferedByteStreamReader
       extends System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/
{
  .method /*060001F1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ServiceModel.Channels.ByteStreamBufferedMessageData/*02000009*/ bufferedMessageData,
                               class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 20 02 01 12 24 12 0D
  {
    // Method begins at RVA 0x2268
    // Code size       0 (0x0)
  } // end of method XmlBufferedByteStreamReader::.ctor

  .method /*060001F2*/ family hidebysig virtual 
          instance void  OnClose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2269
    // Code size       0 (0x0)
  } // end of method XmlBufferedByteStreamReader::OnClose

  .method /*060001F3*/ public hidebysig virtual 
          instance int32  ReadContentAsBase64(uint8[] buffer,
                                              int32 index,
                                              int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x226a
    // Code size       0 (0x0)
  } // end of method XmlBufferedByteStreamReader::ReadContentAsBase64

  .method /*060001F4*/ family hidebysig virtual 
          instance uint8[]  OnToByteArray() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x226b
    // Code size       0 (0x0)
  } // end of method XmlBufferedByteStreamReader::OnToByteArray

  .method /*060001F5*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 
          OnToStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x226c
    // Code size       0 (0x0)
  } // end of method XmlBufferedByteStreamReader::OnToStream

  .method /*060001F6*/ public hidebysig virtual 
          instance bool  TryGetBase64ContentLength([out] int32& length) cil managed
  // SIG: 20 01 02 10 08
  {
    // Method begins at RVA 0x226d
    // Code size       0 (0x0)
  } // end of method XmlBufferedByteStreamReader::TryGetBase64ContentLength

  .method /*060001F7*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x226e
    // Code size       0 (0x0)
  } // end of method XmlBufferedByteStreamReader::.cctor

} // end of class System.ServiceModel.Channels.XmlBufferedByteStreamReader

.class /*0200002A*/ private abstract auto ansi beforefieldinit System.ServiceModel.Channels.XmlStreamedByteStreamReader
       extends System.ServiceModel.Channels.XmlByteStreamReader/*02000027*/
{
  .method /*060001F8*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x226f
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::.ctor

  .method /*060001F9*/ public hidebysig static 
          class System.ServiceModel.Channels.XmlStreamedByteStreamReader/*0200002A*/ 
          Create(class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 'stream',
                 class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 00 02 12 80 A8 12 55 12 0D
  {
    // Method begins at RVA 0x2270
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::Create

  .method /*060001FA*/ public hidebysig static 
          class System.ServiceModel.Channels.XmlStreamedByteStreamReader/*0200002A*/ 
          Create(class [System.Net.Http/*23000007*/]System.Net.Http.HttpRequestMessage/*01000017*/ httpRequestMessage,
                 class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 00 02 12 80 A8 12 5D 12 0D
  {
    // Method begins at RVA 0x2271
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::Create

  .method /*060001FB*/ public hidebysig static 
          class System.ServiceModel.Channels.XmlStreamedByteStreamReader/*0200002A*/ 
          Create(class [System.Net.Http/*23000007*/]System.Net.Http.HttpResponseMessage/*01000018*/ httpResponseMessage,
                 class [System.Runtime.Serialization/*23000004*/]System.Xml.XmlDictionaryReaderQuotas/*01000003*/ quotas) cil managed
  // SIG: 00 02 12 80 A8 12 61 12 0D
  {
    // Method begins at RVA 0x2272
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::Create

  .method /*060001FC*/ family hidebysig virtual 
          instance void  OnClose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2273
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::OnClose

  .method /*060001FD*/ public hidebysig virtual 
          instance int32  ReadContentAsBase64(uint8[] buffer,
                                              int32 index,
                                              int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x2274
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::ReadContentAsBase64

  .method /*060001FE*/ family hidebysig virtual 
          instance uint8[]  OnToByteArray() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2275
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::OnToByteArray

  .method /*060001FF*/ family hidebysig virtual 
          instance class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 
          OnToStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2276
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::OnToStream

  .method /*06000200*/ family hidebysig newslot abstract virtual 
          instance class [mscorlib/*23000001*/]System.IO.Stream/*01000015*/ 
          GetStream() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x0
  } // end of method XmlStreamedByteStreamReader::GetStream

  .method /*06000201*/ family hidebysig newslot abstract virtual 
          instance void  ReleaseStream() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method XmlStreamedByteStreamReader::ReleaseStream

  .method /*06000202*/ public hidebysig virtual 
          instance bool  TryGetBase64ContentLength([out] int32& length) cil managed
  // SIG: 20 01 02 10 08
  {
    // Method begins at RVA 0x2277
    // Code size       0 (0x0)
  } // end of method XmlStreamedByteStreamReader::TryGetBase64ContentLength

} // end of class System.ServiceModel.Channels.XmlStreamedByteStreamReader

.class /*0200002B*/ public abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.HttpResponseMessageExtensionMethods
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .custom /*0C00004F:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
  .method /*06000203*/ public hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          ToMessage(class [System.Net.Http/*23000007*/]System.Net.Http.HttpResponseMessage/*01000018*/ httpResponseMessage) cil managed
  // SIG: 00 01 12 59 12 61
  {
    .custom /*0C000082:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2278
    // Code size       0 (0x0)
  } // end of method HttpResponseMessageExtensionMethods::ToMessage

} // end of class System.ServiceModel.Channels.HttpResponseMessageExtensionMethods

.class /*0200002C*/ public abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.HttpRequestMessageExtensionMethods
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .custom /*0C000050:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
  .method /*06000204*/ public hidebysig static 
          class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ 
          ToMessage(class [System.Net.Http/*23000007*/]System.Net.Http.HttpRequestMessage/*01000017*/ httpRequestMessage) cil managed
  // SIG: 00 01 12 59 12 5D
  {
    .custom /*0C000083:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2279
    // Code size       0 (0x0)
  } // end of method HttpRequestMessageExtensionMethods::ToMessage

} // end of class System.ServiceModel.Channels.HttpRequestMessageExtensionMethods

.class /*0200002D*/ public abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.MessageExtensionMethods
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .custom /*0C000051:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
  .method /*06000205*/ public hidebysig static 
          class [System.Net.Http/*23000007*/]System.Net.Http.HttpRequestMessage/*01000017*/ 
          ToHttpRequestMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 00 01 12 5D 12 59
  {
    .custom /*0C000084:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x227a
    // Code size       0 (0x0)
  } // end of method MessageExtensionMethods::ToHttpRequestMessage

  .method /*06000206*/ public hidebysig static 
          class [System.Net.Http/*23000007*/]System.Net.Http.HttpResponseMessage/*01000018*/ 
          ToHttpResponseMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message) cil managed
  // SIG: 00 01 12 61 12 59
  {
    .custom /*0C000085:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x227b
    // Code size       0 (0x0)
  } // end of method MessageExtensionMethods::ToHttpResponseMessage

  .method /*06000207*/ assembly hidebysig static 
          void  ConfigureAsHttpMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                       class [System.Net.Http/*23000007*/]System.Net.Http.HttpRequestMessage/*01000017*/ httpRequestMessage) cil managed
  // SIG: 00 02 01 12 59 12 5D
  {
    .custom /*0C000086:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x227c
    // Code size       0 (0x0)
  } // end of method MessageExtensionMethods::ConfigureAsHttpMessage

  .method /*06000208*/ assembly hidebysig static 
          void  ConfigureAsHttpMessage(class [System.ServiceModel/*23000003*/]System.ServiceModel.Channels.Message/*01000016*/ message,
                                       class [System.Net.Http/*23000007*/]System.Net.Http.HttpResponseMessage/*01000018*/ httpResponseMessage) cil managed
  // SIG: 00 02 01 12 59 12 61
  {
    .custom /*0C000087:0A000008*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000065*/::.ctor() /* 0A000008 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x227d
    // Code size       0 (0x0)
  } // end of method MessageExtensionMethods::ConfigureAsHttpMessage

  .method /*06000209*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x227e
    // Code size       0 (0x0)
  } // end of method MessageExtensionMethods::.cctor

} // end of class System.ServiceModel.Channels.MessageExtensionMethods

.class /*0200002E*/ private abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.ByteStreamConfigurationStrings
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .field /*04000024*/ static assembly literal string ReaderQuotas = "readerQuotas"
  .field /*04000025*/ static assembly literal string BindingElementType = "bindingElementType"
} // end of class System.ServiceModel.Channels.ByteStreamConfigurationStrings

.class /*0200002F*/ private auto ansi beforefieldinit System.ServiceModel.Channels.SR
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*0600020A*/ private hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x227f
    // Code size       0 (0x0)
  } // end of method SR::.ctor

  .method /*0600020B*/ assembly hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Resources.ResourceManager/*0100005E*/ 
          get_ResourceManager() cil managed
  // SIG: 00 00 12 81 79
  {
    // Method begins at RVA 0x2280
    // Code size       0 (0x0)
  } // end of method SR::get_ResourceManager

  .method /*0600020C*/ assembly hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 81 7D
  {
    // Method begins at RVA 0x2281
    // Code size       0 (0x0)
  } // end of method SR::get_Culture

  .method /*0600020D*/ assembly hidebysig specialname static 
          void  set_Culture(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/ 'value') cil managed
  // SIG: 00 01 01 12 81 7D
  {
    // Method begins at RVA 0x2282
    // Code size       0 (0x0)
  } // end of method SR::set_Culture

  .method /*0600020E*/ assembly hidebysig specialname static 
          string  get_ArgumentNotInSetOfValidValues() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2283
    // Code size       0 (0x0)
  } // end of method SR::get_ArgumentNotInSetOfValidValues

  .method /*0600020F*/ assembly hidebysig specialname static 
          string  get_ByteStreamWriteStartElementAlreadyCalled() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2284
    // Code size       0 (0x0)
  } // end of method SR::get_ByteStreamWriteStartElementAlreadyCalled

  .method /*06000210*/ assembly hidebysig specialname static 
          string  get_MessageBodyIsStream() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2285
    // Code size       0 (0x0)
  } // end of method SR::get_MessageBodyIsStream

  .method /*06000211*/ assembly hidebysig specialname static 
          string  get_XmlReaderClosed() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2286
    // Code size       0 (0x0)
  } // end of method SR::get_XmlReaderClosed

  .method /*06000212*/ assembly hidebysig specialname static 
          string  get_XmlUnexpectedEndElement() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2287
    // Code size       0 (0x0)
  } // end of method SR::get_XmlUnexpectedEndElement

  .method /*06000213*/ assembly hidebysig specialname static 
          string  get_XmlWriterClosed() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2288
    // Code size       0 (0x0)
  } // end of method SR::get_XmlWriterClosed

  .method /*06000214*/ assembly hidebysig specialname static 
          string  get_AsynchronousExceptionOccurred() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2289
    // Code size       0 (0x0)
  } // end of method SR::get_AsynchronousExceptionOccurred

  .method /*06000215*/ assembly hidebysig specialname static 
          string  get_ExplicitListenUriModeRequiresPort() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x228a
    // Code size       0 (0x0)
  } // end of method SR::get_ExplicitListenUriModeRequiresPort

  .method /*06000216*/ assembly hidebysig specialname static 
          string  get_GetByteArrayFromStreamContentNotAllowed() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x228b
    // Code size       0 (0x0)
  } // end of method SR::get_GetByteArrayFromStreamContentNotAllowed

  .method /*06000217*/ assembly hidebysig specialname static 
          string  get_GetByteArrayLengthFailed() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x228c
    // Code size       0 (0x0)
  } // end of method SR::get_GetByteArrayLengthFailed

  .method /*06000218*/ assembly hidebysig specialname static 
          string  get_IcmpDestinationUnreachable() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x228d
    // Code size       0 (0x0)
  } // end of method SR::get_IcmpDestinationUnreachable

  .method /*06000219*/ assembly hidebysig specialname static 
          string  get_IPv4OrIPv6Required() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x228e
    // Code size       0 (0x0)
  } // end of method SR::get_IPv4OrIPv6Required

  .method /*0600021A*/ assembly hidebysig specialname static 
          string  get_ObjectNotOpen() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x228f
    // Code size       0 (0x0)
  } // end of method SR::get_ObjectNotOpen

  .method /*0600021B*/ assembly hidebysig specialname static 
          string  get_ToOrViaRequired() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2290
    // Code size       0 (0x0)
  } // end of method SR::get_ToOrViaRequired

  .method /*0600021C*/ assembly hidebysig specialname static 
          string  get_UdpReceiveException() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2291
    // Code size       0 (0x0)
  } // end of method SR::get_UdpReceiveException

  .method /*0600021D*/ assembly hidebysig specialname static 
          string  get_UdpSendException() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2292
    // Code size       0 (0x0)
  } // end of method SR::get_UdpSendException

  .method /*0600021E*/ assembly hidebysig specialname static 
          string  get_UniquePortNotAvailable() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2293
    // Code size       0 (0x0)
  } // end of method SR::get_UniquePortNotAvailable

  .method /*0600021F*/ assembly hidebysig specialname static 
          string  get_ValueMustBePositive() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2294
    // Code size       0 (0x0)
  } // end of method SR::get_ValueMustBePositive

  .method /*06000220*/ assembly hidebysig specialname static 
          string  get_TimeoutOutOfRange0() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x2295
    // Code size       0 (0x0)
  } // end of method SR::get_TimeoutOutOfRange0

  .method /*06000221*/ assembly hidebysig static 
          string  SizeExceedsRemainingBufferSpace(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x2296
    // Code size       0 (0x0)
  } // end of method SR::SizeExceedsRemainingBufferSpace

  .method /*06000222*/ assembly hidebysig static 
          string  ArgumentOutOfMinRange(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x2297
    // Code size       0 (0x0)
  } // end of method SR::ArgumentOutOfMinRange

  .method /*06000223*/ assembly hidebysig static 
          string  ByteStreamReaderNotInByteStream(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x2298
    // Code size       0 (0x0)
  } // end of method SR::ByteStreamReaderNotInByteStream

  .method /*06000224*/ assembly hidebysig static 
          string  MaxReceivedMessageSizeExceeded(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x2299
    // Code size       0 (0x0)
  } // end of method SR::MaxReceivedMessageSizeExceeded

  .method /*06000225*/ assembly hidebysig static 
          string  MaxSentMessageSizeExceeded(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x229a
    // Code size       0 (0x0)
  } // end of method SR::MaxSentMessageSizeExceeded

  .method /*06000226*/ assembly hidebysig static 
          string  ObjectDisposed(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x229b
    // Code size       0 (0x0)
  } // end of method SR::ObjectDisposed

  .method /*06000227*/ assembly hidebysig static 
          string  XmlStartElementNameExpected(object param0,
                                              object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x229c
    // Code size       0 (0x0)
  } // end of method SR::XmlStartElementNameExpected

  .method /*06000228*/ assembly hidebysig static 
          string  XmlWriterMustBeInElement(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x229d
    // Code size       0 (0x0)
  } // end of method SR::XmlWriterMustBeInElement

  .method /*06000229*/ assembly hidebysig static 
          string  ByteStreamMessageEncoderMessageVersionNotSupported(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x229e
    // Code size       0 (0x0)
  } // end of method SR::ByteStreamMessageEncoderMessageVersionNotSupported

  .method /*0600022A*/ assembly hidebysig static 
          string  OffsetExceedsBufferSize(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x229f
    // Code size       0 (0x0)
  } // end of method SR::OffsetExceedsBufferSize

  .method /*0600022B*/ assembly hidebysig static 
          string  ArgumentOutOfMinMaxRange(object param0,
                                           object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22a0
    // Code size       0 (0x0)
  } // end of method SR::ArgumentOutOfMinMaxRange

  .method /*0600022C*/ assembly hidebysig static 
          string  ByteStreamMessageGetTypeNotSupported(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22a1
    // Code size       0 (0x0)
  } // end of method SR::ByteStreamMessageGetTypeNotSupported

  .method /*0600022D*/ assembly hidebysig static 
          string  ChannelTypeNotSupported(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22a2
    // Code size       0 (0x0)
  } // end of method SR::ChannelTypeNotSupported

  .method /*0600022E*/ assembly hidebysig static 
          string  DefaultPortRequiredForListenUriModeUnique(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22a3
    // Code size       0 (0x0)
  } // end of method SR::DefaultPortRequiredForListenUriModeUnique

  .method /*0600022F*/ assembly hidebysig static 
          string  DnsResolveFailed(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22a4
    // Code size       0 (0x0)
  } // end of method SR::DnsResolveFailed

  .method /*06000230*/ assembly hidebysig static 
          string  IcmpTimeExpired(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22a5
    // Code size       0 (0x0)
  } // end of method SR::IcmpTimeExpired

  .method /*06000231*/ assembly hidebysig static 
          string  PortNumberInvalid(object param0,
                                    object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22a6
    // Code size       0 (0x0)
  } // end of method SR::PortNumberInvalid

  .method /*06000232*/ assembly hidebysig static 
          string  PortNumberRequiredOnVia(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22a7
    // Code size       0 (0x0)
  } // end of method SR::PortNumberRequiredOnVia

  .method /*06000233*/ assembly hidebysig static 
          string  Property1LessThanOrEqualToProperty2(object param0,
                                                      object param1,
                                                      object param2,
                                                      object param3) cil managed
  // SIG: 00 04 0E 1C 1C 1C 1C
  {
    // Method begins at RVA 0x22a8
    // Code size       0 (0x0)
  } // end of method SR::Property1LessThanOrEqualToProperty2

  .method /*06000234*/ assembly hidebysig static 
          string  RecycledMessageIdDuringRetransmission(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22a9
    // Code size       0 (0x0)
  } // end of method SR::RecycledMessageIdDuringRetransmission

  .method /*06000235*/ assembly hidebysig static 
          string  RelativeUriNotAllowed(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22aa
    // Code size       0 (0x0)
  } // end of method SR::RelativeUriNotAllowed

  .method /*06000236*/ assembly hidebysig static 
          string  RemoteAddressUnreachableDueToIPVersionMismatch(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22ab
    // Code size       0 (0x0)
  } // end of method SR::RemoteAddressUnreachableDueToIPVersionMismatch

  .method /*06000237*/ assembly hidebysig static 
          string  RetransmissionRequiresAddressingOnMessage(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22ac
    // Code size       0 (0x0)
  } // end of method SR::RetransmissionRequiresAddressingOnMessage

  .method /*06000238*/ assembly hidebysig static 
          string  SendTimedOut(object param0,
                               object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22ad
    // Code size       0 (0x0)
  } // end of method SR::SendTimedOut

  .method /*06000239*/ assembly hidebysig static 
          string  SocketAddressAccessDenied(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22ae
    // Code size       0 (0x0)
  } // end of method SR::SocketAddressAccessDenied

  .method /*0600023A*/ assembly hidebysig static 
          string  SocketAddressInUse(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22af
    // Code size       0 (0x0)
  } // end of method SR::SocketAddressInUse

  .method /*0600023B*/ assembly hidebysig static 
          string  TimeoutOnOperation(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22b0
    // Code size       0 (0x0)
  } // end of method SR::TimeoutOnOperation

  .method /*0600023C*/ assembly hidebysig static 
          string  ToAddressIsNotValid(object param0,
                                      object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22b1
    // Code size       0 (0x0)
  } // end of method SR::ToAddressIsNotValid

  .method /*0600023D*/ assembly hidebysig static 
          string  UdpAdapterSpecifiedNotConnected(object param0,
                                                  object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22b2
    // Code size       0 (0x0)
  } // end of method SR::UdpAdapterSpecifiedNotConnected

  .method /*0600023E*/ assembly hidebysig static 
          string  UdpAdapterSpecifiedNotSuitableForMulticast(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22b3
    // Code size       0 (0x0)
  } // end of method SR::UdpAdapterSpecifiedNotSuitableForMulticast

  .method /*0600023F*/ assembly hidebysig static 
          string  UdpFailedToFindMulticastAdapter(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22b4
    // Code size       0 (0x0)
  } // end of method SR::UdpFailedToFindMulticastAdapter

  .method /*06000240*/ assembly hidebysig static 
          string  UdpInterfaceIndexMatchNotFound(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22b5
    // Code size       0 (0x0)
  } // end of method SR::UdpInterfaceIndexMatchNotFound

  .method /*06000241*/ assembly hidebysig static 
          string  UdpMaxMessageSendSizeExceeded(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22b6
    // Code size       0 (0x0)
  } // end of method SR::UdpMaxMessageSendSizeExceeded

  .method /*06000242*/ assembly hidebysig static 
          string  NetworkInterfaceMessagePropertyMissing(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22b7
    // Code size       0 (0x0)
  } // end of method SR::NetworkInterfaceMessagePropertyMissing

  .method /*06000243*/ assembly hidebysig static 
          string  UdpSendFailedInterfaceIndexMatchNotFound(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22b8
    // Code size       0 (0x0)
  } // end of method SR::UdpSendFailedInterfaceIndexMatchNotFound

  .method /*06000244*/ assembly hidebysig static 
          string  UnsupportedUriHostNameType(object param0,
                                             object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22b9
    // Code size       0 (0x0)
  } // end of method SR::UnsupportedUriHostNameType

  .method /*06000245*/ assembly hidebysig static 
          string  UriSchemeNotSupported(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22ba
    // Code size       0 (0x0)
  } // end of method SR::UriSchemeNotSupported

  .method /*06000246*/ assembly hidebysig static 
          string  ValueMustBeNonNegative(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22bb
    // Code size       0 (0x0)
  } // end of method SR::ValueMustBeNonNegative

  .method /*06000247*/ assembly hidebysig static 
          string  ViaUriIsNotValid(object param0,
                                   object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22bc
    // Code size       0 (0x0)
  } // end of method SR::ViaUriIsNotValid

  .method /*06000248*/ assembly hidebysig static 
          string  TransportRequiresAddressingOnEncoderForRetransmission(object param0,
                                                                        object param1,
                                                                        object param2) cil managed
  // SIG: 00 03 0E 1C 1C 1C
  {
    // Method begins at RVA 0x22bd
    // Code size       0 (0x0)
  } // end of method SR::TransportRequiresAddressingOnEncoderForRetransmission

  .method /*06000249*/ assembly hidebysig static 
          string  SizeMessagePropertyMissing(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22be
    // Code size       0 (0x0)
  } // end of method SR::SizeMessagePropertyMissing

  .method /*0600024A*/ assembly hidebysig static 
          string  UriHostNameTypeNotSupportedByOS(object param0,
                                                  object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22bf
    // Code size       0 (0x0)
  } // end of method SR::UriHostNameTypeNotSupportedByOS

  .method /*0600024B*/ assembly hidebysig static 
          string  ReceiveRequestTimedOut(object param0,
                                         object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22c0
    // Code size       0 (0x0)
  } // end of method SR::ReceiveRequestTimedOut

  .method /*0600024C*/ assembly hidebysig static 
          string  ReceiveRequestTimedOutNoLocalAddress(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22c1
    // Code size       0 (0x0)
  } // end of method SR::ReceiveRequestTimedOutNoLocalAddress

  .method /*0600024D*/ assembly hidebysig static 
          string  ArgumentPropertyShouldNotBeNullError(object param0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x22c2
    // Code size       0 (0x0)
  } // end of method SR::ArgumentPropertyShouldNotBeNullError

  .method /*0600024E*/ assembly hidebysig static 
          string  MissingHttpMessageProperty(object param0,
                                             object param1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x22c3
    // Code size       0 (0x0)
  } // end of method SR::MissingHttpMessageProperty

  .method /*0600024F*/ assembly hidebysig static 
          string  TransportRequiresAddressingOnEncoderForDuplicateDetection(object param0,
                                                                            object param1,
                                                                            object param2) cil managed
  // SIG: 00 03 0E 1C 1C 1C
  {
    // Method begins at RVA 0x22c4
    // Code size       0 (0x0)
  } // end of method SR::TransportRequiresAddressingOnEncoderForDuplicateDetection

  .property /*1700007D*/ callconv(8) class [mscorlib/*23000001*/]System.Resources.ResourceManager/*0100005E*/
          ResourceManager()
  {
    .get class [mscorlib/*23000001*/]System.Resources.ResourceManager/*0100005E*/ System.ServiceModel.Channels.SR/*0200002F*/::get_ResourceManager() /* 0600020B */
  } // end of property SR::ResourceManager
  .property /*1700007E*/ callconv(8) class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/
          Culture()
  {
    .get class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/ System.ServiceModel.Channels.SR/*0200002F*/::get_Culture() /* 0600020C */
    .set void System.ServiceModel.Channels.SR/*0200002F*/::set_Culture(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/) /* 0600020D */
  } // end of property SR::Culture
  .property /*1700007F*/ callconv(8) string
          ArgumentNotInSetOfValidValues()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_ArgumentNotInSetOfValidValues() /* 0600020E */
  } // end of property SR::ArgumentNotInSetOfValidValues
  .property /*17000080*/ callconv(8) string
          ByteStreamWriteStartElementAlreadyCalled()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_ByteStreamWriteStartElementAlreadyCalled() /* 0600020F */
  } // end of property SR::ByteStreamWriteStartElementAlreadyCalled
  .property /*17000081*/ callconv(8) string
          MessageBodyIsStream()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_MessageBodyIsStream() /* 06000210 */
  } // end of property SR::MessageBodyIsStream
  .property /*17000082*/ callconv(8) string
          XmlReaderClosed()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_XmlReaderClosed() /* 06000211 */
  } // end of property SR::XmlReaderClosed
  .property /*17000083*/ callconv(8) string
          XmlUnexpectedEndElement()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_XmlUnexpectedEndElement() /* 06000212 */
  } // end of property SR::XmlUnexpectedEndElement
  .property /*17000084*/ callconv(8) string
          XmlWriterClosed()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_XmlWriterClosed() /* 06000213 */
  } // end of property SR::XmlWriterClosed
  .property /*17000085*/ callconv(8) string
          AsynchronousExceptionOccurred()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_AsynchronousExceptionOccurred() /* 06000214 */
  } // end of property SR::AsynchronousExceptionOccurred
  .property /*17000086*/ callconv(8) string
          ExplicitListenUriModeRequiresPort()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_ExplicitListenUriModeRequiresPort() /* 06000215 */
  } // end of property SR::ExplicitListenUriModeRequiresPort
  .property /*17000087*/ callconv(8) string
          GetByteArrayFromStreamContentNotAllowed()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_GetByteArrayFromStreamContentNotAllowed() /* 06000216 */
  } // end of property SR::GetByteArrayFromStreamContentNotAllowed
  .property /*17000088*/ callconv(8) string
          GetByteArrayLengthFailed()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_GetByteArrayLengthFailed() /* 06000217 */
  } // end of property SR::GetByteArrayLengthFailed
  .property /*17000089*/ callconv(8) string
          IcmpDestinationUnreachable()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_IcmpDestinationUnreachable() /* 06000218 */
  } // end of property SR::IcmpDestinationUnreachable
  .property /*1700008A*/ callconv(8) string
          IPv4OrIPv6Required()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_IPv4OrIPv6Required() /* 06000219 */
  } // end of property SR::IPv4OrIPv6Required
  .property /*1700008B*/ callconv(8) string
          ObjectNotOpen()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_ObjectNotOpen() /* 0600021A */
  } // end of property SR::ObjectNotOpen
  .property /*1700008C*/ callconv(8) string
          ToOrViaRequired()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_ToOrViaRequired() /* 0600021B */
  } // end of property SR::ToOrViaRequired
  .property /*1700008D*/ callconv(8) string
          UdpReceiveException()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_UdpReceiveException() /* 0600021C */
  } // end of property SR::UdpReceiveException
  .property /*1700008E*/ callconv(8) string
          UdpSendException()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_UdpSendException() /* 0600021D */
  } // end of property SR::UdpSendException
  .property /*1700008F*/ callconv(8) string
          UniquePortNotAvailable()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_UniquePortNotAvailable() /* 0600021E */
  } // end of property SR::UniquePortNotAvailable
  .property /*17000090*/ callconv(8) string
          ValueMustBePositive()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_ValueMustBePositive() /* 0600021F */
  } // end of property SR::ValueMustBePositive
  .property /*17000091*/ callconv(8) string
          TimeoutOutOfRange0()
  {
    .get string System.ServiceModel.Channels.SR/*0200002F*/::get_TimeoutOutOfRange0() /* 06000220 */
  } // end of property SR::TimeoutOutOfRange0
} // end of class System.ServiceModel.Channels.SR

.class /*02000030*/ private auto ansi beforefieldinit System.ServiceModel.Channels.TD
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*06000250*/ private hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22c5
    // Code size       0 (0x0)
  } // end of method TD::.ctor

  .method /*06000251*/ assembly hidebysig specialname static 
          class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 81 7D
  {
    // Method begins at RVA 0x22c6
    // Code size       0 (0x0)
  } // end of method TD::get_Culture

  .method /*06000252*/ assembly hidebysig specialname static 
          void  set_Culture(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/ 'value') cil managed
  // SIG: 00 01 01 12 81 7D
  {
    // Method begins at RVA 0x22c7
    // Code size       0 (0x0)
  } // end of method TD::set_Culture

  .method /*06000253*/ assembly hidebysig static 
          bool  MaxPendingMessagesTotalSizeReachedIsEnabled() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22c8
    // Code size       0 (0x0)
  } // end of method TD::MaxPendingMessagesTotalSizeReachedIsEnabled

  .method /*06000254*/ assembly hidebysig static 
          void  MaxPendingMessagesTotalSizeReached(class [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EventTraceActivity/*01000060*/ eventTraceActivity,
                                                   string messageID,
                                                   int64 maxPendingMessagesTotalSize,
                                                   string bindingElementTypeName) cil managed
  // SIG: 00 04 01 12 81 81 0E 0A 0E
  {
    // Method begins at RVA 0x22c9
    // Code size       0 (0x0)
  } // end of method TD::MaxPendingMessagesTotalSizeReached

  .method /*06000255*/ assembly hidebysig static 
          bool  ByteStreamMessageDecodingStartIsEnabled() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22ca
    // Code size       0 (0x0)
  } // end of method TD::ByteStreamMessageDecodingStartIsEnabled

  .method /*06000256*/ assembly hidebysig static 
          void  ByteStreamMessageDecodingStart() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x22cb
    // Code size       0 (0x0)
  } // end of method TD::ByteStreamMessageDecodingStart

  .method /*06000257*/ assembly hidebysig static 
          bool  ByteStreamMessageEncodingStartIsEnabled() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22cc
    // Code size       0 (0x0)
  } // end of method TD::ByteStreamMessageEncodingStartIsEnabled

  .method /*06000258*/ assembly hidebysig static 
          void  ByteStreamMessageEncodingStart(class [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EventTraceActivity/*01000060*/ eventTraceActivity) cil managed
  // SIG: 00 01 01 12 81 81
  {
    // Method begins at RVA 0x22cd
    // Code size       0 (0x0)
  } // end of method TD::ByteStreamMessageEncodingStart

  .method /*06000259*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x22ce
    // Code size       0 (0x0)
  } // end of method TD::.cctor

  .property /*17000092*/ callconv(8) class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/
          Culture()
  {
    .get class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/ System.ServiceModel.Channels.TD/*02000030*/::get_Culture() /* 06000251 */
    .set void System.ServiceModel.Channels.TD/*02000030*/::set_Culture(class [mscorlib/*23000001*/]System.Globalization.CultureInfo/*0100005F*/) /* 06000252 */
  } // end of property TD::Culture
} // end of class System.ServiceModel.Channels.TD

.class /*02000031*/ private abstract auto ansi sealed beforefieldinit System.ServiceModel.Channels.FxTrace
       extends [mscorlib/*23000001*/]System.Object/*01000012*/
{
  .method /*0600025A*/ public hidebysig static 
          void  UpdateEventDefinitions(valuetype [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EventDescriptor/*01000061*/[] eventDescriptors,
                                       uint16[] end2EndEvents) cil managed
  // SIG: 00 02 01 1D 11 81 85 1D 07
  {
    .custom /*0C000088:0A00002C*/ instance void [mscorlib/*23000001*/]System.Security.SecuritySafeCriticalAttribute/*0100008A*/::.ctor() /* 0A00002C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22cf
    // Code size       0 (0x0)
  } // end of method FxTrace::UpdateEventDefinitions

  .method /*0600025B*/ public hidebysig static 
          bool  IsEventEnabled(int32 index) cil managed
  // SIG: 00 01 02 08
  {
    // Method begins at RVA 0x22d0
    // Code size       0 (0x0)
  } // end of method FxTrace::IsEventEnabled

  .method /*0600025C*/ public hidebysig specialname static 
          bool  get_ShouldTraceCritical() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d1
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceCritical

  .method /*0600025D*/ public hidebysig specialname static 
          bool  get_TracingEnabled() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d2
    // Code size       0 (0x0)
  } // end of method FxTrace::get_TracingEnabled

  .method /*0600025E*/ public hidebysig specialname static 
          bool  get_ShouldTraceError() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d3
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceError

  .method /*0600025F*/ public hidebysig specialname static 
          bool  get_ShouldTraceInformation() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d4
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceInformation

  .method /*06000260*/ public hidebysig specialname static 
          bool  get_ShouldTraceVerbose() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d5
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceVerbose

  .method /*06000261*/ public hidebysig specialname static 
          bool  get_ShouldTraceWarning() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d6
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceWarning

  .method /*06000262*/ public hidebysig specialname static 
          bool  get_ShouldTraceCriticalToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d7
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceCriticalToTraceSource

  .method /*06000263*/ public hidebysig specialname static 
          bool  get_ShouldTraceErrorToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d8
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceErrorToTraceSource

  .method /*06000264*/ public hidebysig specialname static 
          bool  get_ShouldTraceInformationToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22d9
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceInformationToTraceSource

  .method /*06000265*/ public hidebysig specialname static 
          bool  get_ShouldTraceVerboseToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22da
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceVerboseToTraceSource

  .method /*06000266*/ public hidebysig specialname static 
          bool  get_ShouldTraceWarningToTraceSource() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22db
    // Code size       0 (0x0)
  } // end of method FxTrace::get_ShouldTraceWarningToTraceSource

  .method /*06000267*/ public hidebysig specialname static 
          class [System.ServiceModel.Internals/*23000008*/]System.Runtime.ExceptionTrace/*01000062*/ 
          get_Exception() cil managed
  // SIG: 00 00 12 81 89
  {
    // Method begins at RVA 0x22dc
    // Code size       0 (0x0)
  } // end of method FxTrace::get_Exception

  .method /*06000268*/ public hidebysig specialname static 
          class [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EtwDiagnosticTrace/*01000063*/ 
          get_Trace() cil managed
  // SIG: 00 00 12 81 8D
  {
    // Method begins at RVA 0x22dd
    // Code size       0 (0x0)
  } // end of method FxTrace::get_Trace

  .method /*06000269*/ public hidebysig specialname static 
          class [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EventLogger/*01000064*/ 
          get_EventLog() cil managed
  // SIG: 00 00 12 81 91
  {
    // Method begins at RVA 0x22de
    // Code size       0 (0x0)
  } // end of method FxTrace::get_EventLog

  .method /*0600026A*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x22df
    // Code size       0 (0x0)
  } // end of method FxTrace::.cctor

  .property /*17000093*/ callconv(8) bool
          ShouldTraceCritical()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceCritical() /* 0600025C */
  } // end of property FxTrace::ShouldTraceCritical
  .property /*17000094*/ callconv(8) bool
          TracingEnabled()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_TracingEnabled() /* 0600025D */
  } // end of property FxTrace::TracingEnabled
  .property /*17000095*/ callconv(8) bool
          ShouldTraceError()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceError() /* 0600025E */
  } // end of property FxTrace::ShouldTraceError
  .property /*17000096*/ callconv(8) bool
          ShouldTraceInformation()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceInformation() /* 0600025F */
  } // end of property FxTrace::ShouldTraceInformation
  .property /*17000097*/ callconv(8) bool
          ShouldTraceVerbose()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceVerbose() /* 06000260 */
  } // end of property FxTrace::ShouldTraceVerbose
  .property /*17000098*/ callconv(8) bool
          ShouldTraceWarning()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceWarning() /* 06000261 */
  } // end of property FxTrace::ShouldTraceWarning
  .property /*17000099*/ callconv(8) bool
          ShouldTraceCriticalToTraceSource()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceCriticalToTraceSource() /* 06000262 */
  } // end of property FxTrace::ShouldTraceCriticalToTraceSource
  .property /*1700009A*/ callconv(8) bool
          ShouldTraceErrorToTraceSource()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceErrorToTraceSource() /* 06000263 */
  } // end of property FxTrace::ShouldTraceErrorToTraceSource
  .property /*1700009B*/ callconv(8) bool
          ShouldTraceInformationToTraceSource()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceInformationToTraceSource() /* 06000264 */
  } // end of property FxTrace::ShouldTraceInformationToTraceSource
  .property /*1700009C*/ callconv(8) bool
          ShouldTraceVerboseToTraceSource()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceVerboseToTraceSource() /* 06000265 */
  } // end of property FxTrace::ShouldTraceVerboseToTraceSource
  .property /*1700009D*/ callconv(8) bool
          ShouldTraceWarningToTraceSource()
  {
    .get bool System.ServiceModel.Channels.FxTrace/*02000031*/::get_ShouldTraceWarningToTraceSource() /* 06000266 */
  } // end of property FxTrace::ShouldTraceWarningToTraceSource
  .property /*1700009E*/ callconv(8) class [System.ServiceModel.Internals/*23000008*/]System.Runtime.ExceptionTrace/*01000062*/
          Exception()
  {
    .get class [System.ServiceModel.Internals/*23000008*/]System.Runtime.ExceptionTrace/*01000062*/ System.ServiceModel.Channels.FxTrace/*02000031*/::get_Exception() /* 06000267 */
  } // end of property FxTrace::Exception
  .property /*1700009F*/ callconv(8) class [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EtwDiagnosticTrace/*01000063*/
          Trace()
  {
    .get class [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EtwDiagnosticTrace/*01000063*/ System.ServiceModel.Channels.FxTrace/*02000031*/::get_Trace() /* 06000268 */
  } // end of property FxTrace::Trace
  .property /*170000A0*/ callconv(8) class [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EventLogger/*01000064*/
          EventLog()
  {
    .get class [System.ServiceModel.Internals/*23000008*/]System.Runtime.Diagnostics.EventLogger/*01000064*/ System.ServiceModel.Channels.FxTrace/*02000031*/::get_EventLog() /* 06000269 */
  } // end of property FxTrace::EventLog
} // end of class System.ServiceModel.Channels.FxTrace


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
