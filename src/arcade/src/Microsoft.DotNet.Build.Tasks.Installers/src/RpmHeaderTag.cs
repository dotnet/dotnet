// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    public enum RpmHeaderTag
    {
        Immutable = 63,
        I18nTable = 100,
        PackageName = 1000,
        PackageVersion = 1001,
        PackageRelease = 1002,
        Summary = 1004,
        Description = 1005,
        BuildTime = 1006,
        BuildHost = 1007,
        InstalledSize = 1009,
        Vendor = 1011,
        License = 1014,
        Packager = 1015,
        Group = 1016,
        Url = 1020,
        OperatingSystem = 1021,
        Architecture = 1022,
        Prein = 1023,
        Postin = 1024,
        Preun = 1025,
        Postun = 1026,
        FileSizes = 1028,
        FileModes = 1030,
        DeviceFileIds = 1033,
        FileModificationTimestamp = 1034,
        FileDigests = 1035,
        FileLinkTos = 1036,
        FileFlags = 1037,
        FileUserName = 1039,
        FileGroupName = 1040,
        SourceRpm = 1044,
        FileVerifyFlags = 1045,
        ProvideName = 1047,
        RequireFlags = 1048,
        RequireName = 1049,
        RequireVersion = 1050,
        ConflictFlags = 1053,
        ConflictName = 1054,
        ConflictVersion = 1055,
        RpmVersion = 1064,
        ChangelogTimestamp = 1080,
        ChangelogName = 1081,
        ChangelogText = 1082,
        Preinprog = 1085,
        Postinprog = 1086,
        Preunprog = 1087,
        Postunprog = 1088,
        FileDevices = 1095,
        FileInode = 1096,
        FileLang = 1097,
        Prefixes = 1098,
        ProvideFlags = 1112,
        ProvideVersion = 1113,
        DirectoryNameIndices = 1116,
        BaseNames = 1117,
        DirectoryNames = 1118,
        OptFlags = 1122,
        PayloadFormat = 1124,
        PayloadCompressor = 1125,
        PayloadCompressorLevel = 1126,
        Platform = 1132,
        FileColors = 1140,
        FileClass = 1141,
        FileClassDictionary = 1142,
        FileDigestAlgorithm = 5011,
        Encoding = 5062,
        CompressedPayloadDigest = 5092,
        PayloadDigestAlgorithm = 5093,
        UncompressedPayloadDigest = 5097,
    }
}
