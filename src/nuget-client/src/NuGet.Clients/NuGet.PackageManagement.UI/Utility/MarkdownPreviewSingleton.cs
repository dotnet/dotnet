// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Markdown.Platform;

namespace NuGet.PackageManagement.UI
{
    public class MarkdownPreviewSingleton
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private static IMarkdownPreview Instance;
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
        public static IMarkdownPreview GetInstance()
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (Instance == null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Instance = new PreviewBuilder().Build();
#pragma warning restore CS0618 // Type or member is obsolete
            }

            return Instance;
        }

        public static void ResetInstance()
        {
            if (Instance != null)
            {
                Instance.Dispose();
                Instance = null;
            }
        }
    }
}
