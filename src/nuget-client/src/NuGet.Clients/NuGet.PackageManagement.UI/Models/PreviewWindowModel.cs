// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace NuGet.PackageManagement.UI
{
    public class PreviewWindowModel
    {
        public IEnumerable<PreviewResult> PreviewResults { get; }

        public string Title { get; private set; }

        public InputGestureCollection CopyGestures;

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var r in PreviewResults)
            {
                sb.AppendLine(r.Name);
                sb.AppendLine("");
                if (r.Deleted.Any())
                {
                    sb.AppendLine(Resources.Label_UninstalledPackages);
                    sb.AppendLine("");
                    foreach (var p in r.Deleted)
                    {
                        sb.AppendLine(p.ToString());
                    }
                    sb.AppendLine("");
                }
                if (r.Updated.Any())
                {
                    sb.AppendLine(Resources.Label_UpdatedPackages);
                    sb.AppendLine("");
                    foreach (var p in r.Updated)
                    {
                        sb.AppendLine(p.ToString());
                    }
                    sb.AppendLine("");
                }
                if (r.Added.Any())
                {
                    sb.AppendLine(Resources.Label_InstalledPackages);
                    sb.AppendLine("");
                    foreach (var p in r.Added)
                    {
                        sb.AppendLine(p.ToString());
                    }
                    sb.AppendLine("");
                }
                if (r.NewSourceMappings?.Count > 0)
                {
                    foreach (var newSourceMapping in r.NewSourceMappings)
                    {
                        sb.AppendLine(string.Format(Resources.Label_CreatingSourceMappings, newSourceMapping.Key));
                        sb.AppendLine("");
                        foreach (string newSourceMappingPackageId in newSourceMapping.Value)
                        {
                            sb.AppendLine(newSourceMappingPackageId);
                        }
                        sb.AppendLine("");
                    }
                }
            }
            return sb.ToString();
        }

        public PreviewWindowModel(IEnumerable<PreviewResult> results)
        {
            PreviewResults = results;
            Title = Resources.WindowTitle_PreviewChanges;
            CopyGestures = ApplicationCommands.Copy.InputGestures;
        }
    }
}
