// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenerateLicenseList
{

    public class LicenseDataParser
    {
        private string _licenseFile;
        private string _exceptionFile;

        public LicenseDataParser(string licenseFile, string exceptionFile)
        {
            _licenseFile = licenseFile ?? throw new ArgumentNullException(nameof(licenseFile));
            _exceptionFile = exceptionFile ?? throw new ArgumentNullException(nameof(exceptionFile));
        }

        public LicenseDataList ParseLicenses()
        {
            var cursor = ReadJsonFile(_licenseFile);
            var licenseFileVersion = ReadString(cursor, "licenseListVersion");

            var licenseArray = ReadArray(cursor["licenses"] as JArray, (token) => ReadLicense(token));

            return new LicenseDataList(licenseFileVersion, licenseArray);
        }

        private static LicenseData ReadLicense(JToken tokens)
        {
            return new LicenseData(licenseID: ReadString(tokens, "licenseId"),
                                   referenceNumber: ReadInt(tokens, "referenceNumber"),
                                   isOsiApproved: ReadBool(tokens, "isOsiApproved"),
                                   isDeprecatedLicenseId: ReadBool(tokens, "isDeprecatedLicenseId"),
                                   isFsfLibre: ReadBool(tokens, "isFsfLibre", false)
                );
        }

        public ExceptionListData ParseExceptions()
        {
            var cursor = ReadJsonFile(_exceptionFile);
            var licenseFileVersion = ReadString(cursor, "licenseListVersion");

            var exceptionsArray = ReadArray(cursor["exceptions"] as JArray, (token) => ReadException(token));

            return new ExceptionListData(licenseFileVersion, exceptionsArray);
        }

        private static ExceptionData ReadException(JToken tokens)
        {
            return new ExceptionData(licenseID: ReadString(tokens, "licenseExceptionId"),
                                   referenceNumber: ReadInt(tokens, "referenceNumber"),
                                   isDeprecatedLicenseId: ReadBool(tokens, "isDeprecatedLicenseId")
                );
        }

        private static IList<TItem> ReadArray<TItem>(JArray json, Func<JToken, TItem> readItem)
        {
            if (json == null)
            {
                return new List<TItem>();
            }
            var items = new List<TItem>();
            foreach (var child in json)
            {
                var item = readItem(child);
                if (item != null)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        private static string ReadString(JToken json, string property)
        {
            return json[property].Value<string>();
        }

        private static int ReadInt(JToken json, string property)
        {
            return json[property].Value<int>();
        }

        private static bool ReadBool(JToken json, string property)
        {
            return json[property].Value<bool>();
        }

        private static bool ReadBool(JToken json, string property, bool defaultValue = false)
        {
            return json.Value<bool?>(property) ?? defaultValue;
        }

        private static JObject ReadJsonFile(string file)
        {
            if (!new FileInfo(file).Exists)
            {
                throw new ArgumentException($"The file - {file} does not exist");
            }

            using (var stream = File.OpenRead(file))
            {
                using (var textReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(textReader))
                    {
                        while (jsonReader.TokenType != JsonToken.StartObject)
                        {
                            if (!jsonReader.Read())
                            {
                                throw new InvalidDataException();
                            }
                        }
                        return JObject.Load(jsonReader);
                    }
                }
            }
        }
    }

    public class LicenseDataList
    {
        public string LicenseListVersion { get; }

        public IList<LicenseData> LicenseList { get; }

        public LicenseDataList(string licenseListVersion, IList<LicenseData> licenseList)
        {
            LicenseListVersion = licenseListVersion;
            LicenseList = licenseList;
        }
    }

    public class ExceptionListData
    {
        public string LicenseListVersion { get; }

        public IList<ExceptionData> ExceptionList { get; private set; }
        public ExceptionListData(string licenseListVersion, IList<ExceptionData> exceptionList)
        {
            LicenseListVersion = licenseListVersion;
            ExceptionList = exceptionList;
        }
    }

    public class LicenseData
    {
        public LicenseData(string licenseID, int referenceNumber, bool isOsiApproved, bool isDeprecatedLicenseId, bool isFsfLibre)
        {
            LicenseID = licenseID;
            ReferenceNumber = referenceNumber;
            IsOsiApproved = isOsiApproved;
            IsDeprecatedLicenseId = isDeprecatedLicenseId;
            IsFsfLibre = isFsfLibre;
        }

        public string LicenseID { get; }
        public int ReferenceNumber { get; }
        public bool IsOsiApproved { get; }
        public bool IsDeprecatedLicenseId { get; }
        public bool IsFsfLibre { get; }
    }

    public class ExceptionData
    {
        public ExceptionData(string licenseID, int referenceNumber, bool isDeprecatedLicenseId)
        {
            LicenseExceptionID = licenseID;
            ReferenceNumber = referenceNumber;
            IsDeprecatedLicenseId = isDeprecatedLicenseId;
        }

        public string LicenseExceptionID { get; }
        public int ReferenceNumber { get; }
        public bool IsDeprecatedLicenseId { get; }
    }
}
