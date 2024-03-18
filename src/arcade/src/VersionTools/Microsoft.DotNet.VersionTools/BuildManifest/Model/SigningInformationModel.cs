// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class SigningInformationModel
    {
        public List<FileExtensionSignInfoModel> FileExtensionSignInfo { get; set; } = new List<FileExtensionSignInfoModel>();

        public List<FileSignInfoModel> FileSignInfo { get; set; } = new List<FileSignInfoModel>();

        public List<ItemToSignModel> ItemsToSign { get; set; } = new List<ItemToSignModel>();

        public List<StrongNameSignInfoModel> StrongNameSignInfo { get; set; } = new List<StrongNameSignInfoModel>();

        public List<CertificatesSignInfoModel> CertificatesSignInfo { get; set; } = new List<CertificatesSignInfoModel>();

        public void Add(SigningInformationModel source)
        {
            FileExtensionSignInfo.AddRange(source.FileExtensionSignInfo);
            FileSignInfo.AddRange(source.FileSignInfo);
            ItemsToSign.AddRange(source.ItemsToSign);
            StrongNameSignInfo.AddRange(source.StrongNameSignInfo);
            CertificatesSignInfo.AddRange(source.CertificatesSignInfo);
        }

        // The IsEmpty() check ensures that we dont' return a blank <SigningInformation> XElement
        // when there is no signing information, maintaining parity with the old asset manifest structure
        public XElement ToXml() => IsEmpty() ? null : new XElement(
            "SigningInformation",
            Enumerable.Concat(
                FileExtensionSignInfo
                    .ThrowIfInvalidFileExtensionSignInfo()
                    .OrderBy(fe => fe.Include, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(fe => fe.CertificateName, StringComparer.OrdinalIgnoreCase)
                    .Select(fe => fe.ToXml()),
                FileSignInfo
                    .ThrowIfInvalidFileSignInfo()
                    .OrderBy(f => f.Include, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(f => f.CertificateName, StringComparer.OrdinalIgnoreCase)
                    .Select(f => f.ToXml()))
            .Concat(CertificatesSignInfo
                .ThrowIfInvalidCertificateSignInfo()
                .OrderBy(f => f.Include, StringComparer.OrdinalIgnoreCase)
                .ThenBy(f => f.DualSigningAllowed)
                .Select(f => f.ToXml()))
            .Concat(ItemsToSign
                .OrderBy(i => i.Include, StringComparer.OrdinalIgnoreCase)
                .Select(i => i.ToXml()))
            .Concat(StrongNameSignInfo
                .ThrowIfInvalidStrongNameSignInfo()
                .OrderBy(s => s.Include, StringComparer.OrdinalIgnoreCase)
                .ThenBy(s => s.PublicKeyToken, StringComparer.OrdinalIgnoreCase)
                .Select(s => s.ToXml())));

        public bool IsEmpty() => !FileExtensionSignInfo.Any() && !FileSignInfo.Any()
            && !ItemsToSign.Any() && !StrongNameSignInfo.Any() && !CertificatesSignInfo.Any();

        public static SigningInformationModel Parse(XElement xml) => xml == null ? null : new SigningInformationModel
        {
            FileExtensionSignInfo = xml.Elements("FileExtensionSignInfo").Select(FileExtensionSignInfoModel.Parse)
                .ThrowIfInvalidFileExtensionSignInfo().ToList(),
            FileSignInfo = xml.Elements("FileSignInfo").Select(FileSignInfoModel.Parse)
                .ThrowIfInvalidFileSignInfo().ToList(),
            ItemsToSign = xml.Elements("ItemsToSign").Select(ItemToSignModel.Parse).ToList(),
            StrongNameSignInfo = xml.Elements("StrongNameSignInfo").Select(StrongNameSignInfoModel.Parse)
                .ThrowIfInvalidStrongNameSignInfo().ToList(),
            CertificatesSignInfo = xml.Elements("CertificatesSignInfo").Select(CertificatesSignInfoModel.Parse)
                .ThrowIfInvalidCertificateSignInfo().ToList(),
        };
    }

    public class FileExtensionSignInfoModel
    {
        private static readonly string[] RequiredAttributes =
        {
            nameof(Include),
            nameof(CertificateName)
        };
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public string Include
        {
            get { return Attributes.GetOrDefault(nameof(Include)); }
            set { Attributes[nameof(Include)] = value; }
        }

        public string CertificateName
        {
            get { return Attributes.GetOrDefault(nameof(CertificateName)); }
            set { Attributes[nameof(CertificateName)] = value; }
        }
        public override string ToString() => $"Files \"*{Include}\" are signed with {CertificateName}";

        public XElement ToXml() => new XElement(
            "FileExtensionSignInfo",
            Attributes
                .ThrowIfMissingAttributes(RequiredAttributes)
                .CreateXmlAttributes(RequiredAttributes));

        public static FileExtensionSignInfoModel Parse(XElement xml) => new FileExtensionSignInfoModel
        {
            Attributes = xml
                .CreateAttributeDictionary()
                .ThrowIfMissingAttributes(RequiredAttributes)
        };
    }

    public class FileSignInfoModel
    {
        private static readonly string[] RequiredAttributes =
        {
            nameof(Include),
            nameof(CertificateName)
        };
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public string Include
        {
            get { return Attributes.GetOrDefault(nameof(Include)); }
            set { Attributes[nameof(Include)] = value; }
        }

        public string CertificateName
        {
            get { return Attributes.GetOrDefault(nameof(CertificateName)); }
            set { Attributes[nameof(CertificateName)] = value; }
        }

        public string TargetFramework
        {
            get { return Attributes.GetOrDefault(nameof(TargetFramework)); }
            set { Attributes[nameof(TargetFramework)] = value; }
        }

        public string PublicKeyToken
        {
            get { return Attributes.GetOrDefault(nameof(PublicKeyToken)); }
            set { Attributes[nameof(PublicKeyToken)] = value; }
        }

        public override string ToString() => $"File \"{Include}\" is signed with {CertificateName}";

        public XElement ToXml() => new XElement(
            "FileSignInfo",
            Attributes
                .ThrowIfMissingAttributes(RequiredAttributes)
                .CreateXmlAttributes(RequiredAttributes));

        public static FileSignInfoModel Parse(XElement xml) => new FileSignInfoModel
        {
            Attributes = xml
                .CreateAttributeDictionary()
                .ThrowIfMissingAttributes(RequiredAttributes)
        };
    }

    public class CertificatesSignInfoModel
    {
        private static readonly string[] RequiredAttributes =
        {
            nameof(Include),
            nameof(DualSigningAllowed)
        };
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public string Include
        {
            get { return Attributes.GetOrDefault(nameof(Include)); }
            set { Attributes[nameof(Include)] = value; }
        }

        public bool DualSigningAllowed
        {
            get { return bool.Parse(Attributes.GetOrDefault(nameof(DualSigningAllowed))); }
            set { Attributes[nameof(DualSigningAllowed)] = value.ToString().ToLower(); }
        }
        public override string ToString() => $"Certificate \"{Include}\" has DualSigningAllowed set to {DualSigningAllowed}";

        public XElement ToXml() => new XElement(
            "CertificatesSignInfo",
            Attributes
                .ThrowIfMissingAttributes(RequiredAttributes)
                .CreateXmlAttributes(RequiredAttributes));

        public static CertificatesSignInfoModel Parse(XElement xml) => new CertificatesSignInfoModel
        {
            Attributes = xml
                .CreateAttributeDictionary()
                .ThrowIfMissingAttributes(RequiredAttributes)
        };
    }


    public class ItemToSignModel
    {
        private static readonly string[] RequiredAttributes =
        {
            nameof(Include)
        };
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public string Include
        {
            get { return Attributes.GetOrDefault(nameof(Include)); }
            set { Attributes[nameof(Include)] = value; }
        }
        public override string ToString() => $"Signed: {Include}";

        public XElement ToXml() => new XElement(
            "ItemsToSign",
            Attributes
                .ThrowIfMissingAttributes(RequiredAttributes)
                .CreateXmlAttributes(RequiredAttributes));

        public static ItemToSignModel Parse(XElement xml) => new ItemToSignModel
        {
            Attributes = xml
                .CreateAttributeDictionary()
                .ThrowIfMissingAttributes(RequiredAttributes)
        };
    }

    public class StrongNameSignInfoModel
    {
        private static readonly string[] RequiredAttributes =
        {
            nameof(Include),
            nameof(PublicKeyToken),
            nameof(CertificateName)
        };
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public string Include
        {
            get { return Attributes.GetOrDefault(nameof(Include)); }
            set { Attributes[nameof(Include)] = value; }
        }
        public string PublicKeyToken
        {
            get { return Attributes.GetOrDefault(nameof(PublicKeyToken)); }
            set { Attributes[nameof(PublicKeyToken)] = value; }
        }
        public string CertificateName
        {
            get { return Attributes.GetOrDefault(nameof(CertificateName)); }
            set { Attributes[nameof(CertificateName)] = value; }
        }
        public override string ToString() => $"{Include} strong-name signed with certificate {CertificateName}, public key token: {PublicKeyToken}";

        public XElement ToXml() => new XElement(
            "StrongNameSignInfo",
            Attributes
                .ThrowIfMissingAttributes(RequiredAttributes)
                .CreateXmlAttributes(RequiredAttributes));

        public static StrongNameSignInfoModel Parse(XElement xml) => new StrongNameSignInfoModel
        {
            Attributes = xml
                .CreateAttributeDictionary()
                .ThrowIfMissingAttributes(RequiredAttributes)
        };
    }
}
