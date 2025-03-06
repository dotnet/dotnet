// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.SignCheck.Verification
{
    public class SignatureVerificationResult
    {
        private Dictionary<string, string> _detail;
        private List<SignatureVerificationResult> _nestedResults;
        private string _tempPath;
        private List<Timestamp> _timestamps;

        /// <summary>
        /// A dictionary containing detailed information about the verification results. The dictionary keys are used to group related information.
        /// See <see cref="DetailKeys"/> for a list of keys.
        /// </summary>
        public Dictionary<string, string> Detail
        {
            get
            {
                if (_detail == null)
                {
                    _detail = new Dictionary<string, string>();
                }

                return _detail;
            }
        }

        /// <summary>
        /// A string containing the formatted <see cref="Exclusion"/> entry that would allow the file to be excluded from verification.
        /// </summary>
        public string ExclusionEntry
        {
            get;
            set;
        }

        /// <summary>
        /// The filename, including the extension, associated with the result.
        /// </summary>
        public string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// The full path of the file associated with the result.
        /// </summary>
        public string FullPath
        {
            get;
            set;
        }

        /// <summary>
        /// True if the file has a valid AuthentiCode signature.
        /// </summary>
        public bool IsAuthentiCodeSigned
        {
            get;
            set;
        }

        /// <summary>
        /// True if this file was marked as DO-NOT-SIGN. This result can be used with IsSigned to
        /// determine if there is an error.
        /// </summary>
        public bool IsDoNotSign
        {
            get;
            set;
        }

        /// <summary>
        /// True if the file was excluded from verification, false otherwise.
        /// </summary>
        public bool IsExcluded
        {
            get;
            set;
        }

        /// <summary>
        /// True if the file is a portable executable and the header indicates it is a native
        /// code image.
        /// </summary>
        public bool IsNativeImage
        {
            get;
            set;
        }

        /// <summary>
        /// True if the file contains a valid signature, false otherwise.
        /// </summary>
        public bool IsSigned
        {
            get;
            set;
        }

        /// <summary>
        /// Returns true if signature verification was skipped, false otherwise. 
        /// </summary>
        public bool IsSkipped
        {
            get;
            set;
        }

        /// <summary>
        /// True if the file has a valid StrongName signature.
        /// </summary>
        public bool IsStrongNameSigned
        {
            get;
            set;
        }

        public string VirtualPath
        {
            get;
        }

        /// <summary>
        /// A set of results for nested files. For example, if recursive verification is enabled and a file contains embedded files, e.g. an MSI, then
        /// this property will contain the verification results of the embedded files.
        /// </summary>
        public ICollection<SignatureVerificationResult> NestedResults
        {
            get
            {
                if (_nestedResults == null)
                {
                    _nestedResults = new List<SignatureVerificationResult>();
                }

                return _nestedResults;
            }
            set
            {
                _nestedResults = value.ToList();
            }
        }

        /// <summary>
        /// A collection of <see cref="Timestamp"/>s associated with the AuthentiCode signature(s). 
        /// </summary>
        public ICollection<Timestamp> Timestamps
        {
            get
            {
                if (_timestamps == null)
                {
                    _timestamps = new List<Timestamp>();
                }
                return _timestamps;
            }
            set
            {
                _timestamps = value.ToList();
            }
        }


        public string TempPath
        {
            get
            {
                if (String.IsNullOrEmpty(_tempPath))
                {
                    _tempPath = Path.Combine(Path.GetTempPath(), "SignCheck", Path.GetRandomFileName());
                }

                return _tempPath;
            }
        }

        public SignatureVerificationResult(string path, string parent, string virtualPath)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }

            Filename = Path.GetFileName(path);
            FullPath = Path.GetFullPath(path);
            VirtualPath = virtualPath?.Replace('\\', '/');

            AddDetail(DetailKeys.File, Filename);
        }

        /// <summary>
        /// Add detail to the result, classified under the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key under which the detail will be classified.</param>
        /// <param name="detail">A string containing information about the result.</param>
        public void AddDetail(string key, string detail)
        {
            string currentValue;

            if (Detail.TryGetValue(key, out currentValue))
            {
                if (String.IsNullOrEmpty(currentValue))
                {
                    Detail[key] = detail;
                }
                else
                {
                    Detail[key] = String.Join(", ", currentValue, detail);
                }
            }
            else
            {
                Detail[key] = detail;
            }
        }

        /// <summary>
        /// Add formated detail to the result, classified under the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key under which the detail will be classified.</param>
        /// <param name="format">A string containing information about the result that will be formatted based on its format specifiers and <paramref name="values"/>.</param>
        /// <param name="values">The parameters to format.</param>
        public void AddDetail(string key, string format, params object[] values)
        {
            AddDetail(key, String.Format(format, values));
        }

        /// <summary>
        /// </summary>
        /// <param name="detailKeys"></param>
        /// <returns></returns>
        public string ToString(string[] detailKeys)
        {
            var sb = new StringBuilder();

            foreach (var key in detailKeys)
            {
                string value;

                if (Detail.TryGetValue(key, out value))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(String.Format("[{0}] {1}", key, value));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="detailKey"></param>
        /// <returns></returns>
        public string ToString(string detailKey)
        {
            if (Detail.TryGetValue(detailKey, out string value))
            {
                return value;
            }
            return String.Empty;
        }

        /// <summary>
        /// Creates a SignatureVerificationResult for an unsupported file type or file extension.
        /// </summary>
        /// <param name="path">The path to the file that is unsupported</param>
        /// <returns>A SignatureVerificationResult indicating the file is unsupported..</returns>
        public static SignatureVerificationResult UnsupportedFileTypeResult(string path, string parent, string virtualPath)
        {
            var signatureVerificationResult = new SignatureVerificationResult(path, parent, virtualPath)
            {
                IsSkipped = true
            };

            signatureVerificationResult.AddDetail(DetailKeys.File, SignCheckResources.DetailSkippedUnsupportedFileType);

            return signatureVerificationResult;
        }

        /// <summary>
        /// Creates a SignatureVerificationResult for an excluded file type or file extension.
        /// </summary>
        /// <param name="path">The path to the excluded file.</param>
        /// <param name="parent">The parent container of the excluded file</param>
        /// <returns></returns>
        public static SignatureVerificationResult ExcludedFileResult(string path, string parent)
        {
            var signatureVerificationResult = new SignatureVerificationResult(path, parent, null)
            {
                IsExcluded = true
            };

            signatureVerificationResult.AddDetail(DetailKeys.File, SignCheckResources.DetailExcluded);

            return signatureVerificationResult;
        }
    }
}
