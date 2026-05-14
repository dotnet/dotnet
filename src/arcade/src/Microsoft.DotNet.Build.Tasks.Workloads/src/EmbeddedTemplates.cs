// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    internal class EmbeddedTemplates
    {
        private static readonly Dictionary<string, string> s_templateResources = new();

        /// <summary>
        /// Extracts the specified filename from the embedded template resource into the destination folder and
        /// returns the full path of the file.
        /// </summary>
        /// <param name="filename">The name of the template file to extract.</param>
        /// <param name="destinationFolder">The directory where the file will be extracted.</param>
        /// <returns>The full path of the extracted file.</returns>
        public static string Extract(string filename, string destinationFolder)
        {
            return Extract(filename, destinationFolder, filename);
        }

        /// <summary>
        /// Extracts the specified filename from the embedded template resource into the destination folder using
        /// the specified filename and return the full path of the file.
        /// </summary>
        /// <param name="filename">The name of the template file to extract.</param>
        /// <param name="destinationFolder">The directory where the file will be extracted.</param>
        /// <returns>The full path of the extracted file.</returns>
        public static string Extract(string filename, string destinationFolder, string destinationFilename)
        {
            if (!s_templateResources.TryGetValue(filename, out string resourceName))
            {
                throw new KeyNotFoundException(string.Format(Strings.TemplateNotFound, filename));
            }

            // Clean out stale files just to be safe.
            string destinationPath = Path.Combine(destinationFolder, destinationFilename);
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }
            else
            {
                Directory.CreateDirectory(destinationFolder);
            }

            using Stream rs = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using FileStream fs = new(destinationPath, FileMode.Create, FileAccess.Write);

            if (rs == null)
            {
                throw new IOException(string.Format(Strings.TemplateResourceNotFound, resourceName));
            }

            rs.CopyTo(fs);

            return destinationPath;
        }

        static EmbeddedTemplates()
        {
            string ns = MethodBase.GetCurrentMethod().DeclaringType.Namespace;

            s_templateResources = new()
            {
                { "DependencyProvider.wxs", $"{ns}.MsiTemplate.DependencyProvider.wxs" },
                { "Directories.wxs", $"{ns}.MsiTemplate.Directories.wxs" },
                { "dotnethome_x64.wxs", $"{ns}.MsiTemplate.dotnethome_x64.wxs" },
                { "ManifestProduct.wxs", $"{ns}.MsiTemplate.ManifestProduct.wxs" },
                { "WorkloadSetProduct.wxs", $"{ns}.MsiTemplate.WorkloadSetProduct.wxs" },
                { "Product.wxs", $"{ns}.MsiTemplate.Product.wxs" },
                { "Registry.wxs", $"{ns}.MsiTemplate.Registry.wxs" },
                { "Variables.wxi", $"{ns}.MsiTemplate.Variables.wxi" },

                { $"msi.swr", $"{ns}.SwixTemplate.msi.swr" },
                { $"msi.swixproj", $"{ns}.SwixTemplate.msi.swixproj" },
                { $"component.swr", $"{ns}.SwixTemplate.component.swr" },
                { $"component.res.swr", $"{ns}.SwixTemplate.component.res.swr" },
                { $"component.swixproj", $"{ns}.SwixTemplate.component.swixproj" },
                { $"manifest.vsmanproj", $"{ns}.SwixTempalte.manifest.vsmanproj" },
                { $"packageGroup.swr", $"{ns}.SwixTemplate.packageGroup.swr" },
                { $"packageGroup.swixproj", $"{ns}.SwixTemplate.packageGroup.swixproj" },

                { "Icon.png", $"{ns}.Misc.Icon.png" },
                { "LICENSE.TXT", $"{ns}.Misc.LICENSE.TXT" },
                { "msi.csproj", $"{ns}.Misc.msi.csproj" }
            };
        }
    }
}
