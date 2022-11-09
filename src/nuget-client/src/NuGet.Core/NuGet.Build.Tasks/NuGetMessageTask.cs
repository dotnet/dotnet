// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// A task that logs a message from the localized <see cref="Strings"/> resource.
    /// </summary>
    public sealed class NuGetMessageTask : Task
    {
        [Required]
        public string Name { get; set; }

        public string[] Args { get; set; }

        public string Importance { get; set; } = nameof(MessageImportance.Normal);

        public NuGetMessageTask()
            : base(Strings.ResourceManager)
        {
        }

        public override bool Execute()
        {
            if (!Enum.TryParse(Importance, ignoreCase: true, out MessageImportance messageImportance))
            {
                // MessageImportance defaults to High since its the zero value in the enum, instead default to Normal
                messageImportance = MessageImportance.Normal;
            }

            Log.LogMessageFromResources(messageImportance, Name, Args);

            return true;
        }
    }
}
