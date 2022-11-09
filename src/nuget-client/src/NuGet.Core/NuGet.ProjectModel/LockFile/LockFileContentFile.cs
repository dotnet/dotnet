// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using NuGet.Packaging;

namespace NuGet.ProjectModel
{
    public class LockFileContentFile : LockFileItem
    {
        public static readonly string BuildActionProperty = "buildAction";
        public static readonly string CodeLanguageProperty = "codeLanguage";
        public static readonly string PPOutputPathProperty = "ppOutputPath";
        public static readonly string OutputPathProperty = "outputPath";
        public static readonly string CopyToOutputProperty = "copyToOutput";

        public LockFileContentFile(string path) : base(path)
        {
        }

        public string OutputPath
        {
            get
            {
                return GetProperty(OutputPathProperty);
            }
            set
            {
                SetProperty(OutputPathProperty, value);
            }
        }

        public string PPOutputPath
        {
            get
            {
                return GetProperty(PPOutputPathProperty);
            }
            set
            {
                SetProperty(PPOutputPathProperty, value);
            }
        }

        public BuildAction BuildAction
        {
            get
            {
                var value = GetProperty(BuildActionProperty)
                    ?? PackagingConstants.ContentFilesDefaultBuildAction;
                return BuildAction.Parse(value);
            }
            set
            {
                SetProperty(BuildActionProperty, value.Value);
            }
        }

        public string CodeLanguage
        {
            get
            {
                return GetProperty(CodeLanguageProperty);
            }
            set
            {
                SetProperty(CodeLanguageProperty, value);
            }
        }

        public bool CopyToOutput
        {
            get
            {
                return string.Equals(GetProperty(CopyToOutputProperty), bool.TrueString, StringComparison.OrdinalIgnoreCase);
            }
            set
            {
                SetProperty(CopyToOutputProperty, value.ToString(CultureInfo.CurrentCulture));
            }
        }
    }
}
