using System;
using System.Collections.Generic;

namespace NuGet.CommandLine.Test.Caching
{
    public class CachingValidation
    {
        private static readonly Dictionary<CachingValidationType, Messages> validationMessages = new Dictionary<CachingValidationType, Messages>
        {
            {
                CachingValidationType.CommandSucceeded,
                new Messages
                {
                    True = "The command succeeded.",
                    False = "The command failed."
                }
            },
            {
                CachingValidationType.PackageInstalled,
                new Messages
                {
                    True = "The package was installed.",
                    False = "The package was not installed."
                }
            },
            {
                CachingValidationType.PackageInGlobalPackagesFolder,
                new Messages
                {
                    True = "The package was added to the global packages folder.",
                    False = "The package was not added to the global packages folder."
                }
            },
            {
                CachingValidationType.PackageInHttpCache,
                new Messages
                {
                    True = "The package was written to the HTTP cache.",
                    False = "The package was not written to the HTTP cache."
                }
            },
            {
                CachingValidationType.PackageFromHttpCacheUsed,
                new Messages
                {
                    True = "The package in the HTTP cache was used.",
                    False = "The package in the HTTP cache was not used."
                }
            },
            {
                CachingValidationType.PackageFromSourceUsed,
                new Messages
                {
                    True = "The package from the source was used.",
                    False = "The package from the source was not used."
                }
            },
            {
                CachingValidationType.PackageFromSourceNotUsed,
                new Messages
                {
                    True = "The package from the source was not used.",
                    False = "The package from the source was used."
                }
            },
            {
                CachingValidationType.PackageFromGlobalPackagesFolderUsed,
                new Messages
                {
                    True = "The package from the global packages folder was used.",
                    False = "The package from the global packages folder was not used."
                }
            },
            {
                CachingValidationType.DirectDownloadFilesDoNotExist,
                new Messages
                {
                    True = "The direct download files were cleaned up.",
                    False = "The direct download files were not cleaned up."
                }
            },
            {
                CachingValidationType.RestoreNoOp,
                new Messages
                {
                    True = "NoOp Restore",
                    False = "Restore did not no-op."
                }
            }
        };

        public CachingValidation(CachingValidationType type, bool isTrue)
        {
            IsTrue = isTrue;
            Type = type;

            Messages messages;
            if (!validationMessages.TryGetValue(type, out messages))
            {
                throw new ArgumentException($"The caching validation type '{type}' does not have messages configured.", nameof(type));
            }

            Message = isTrue ? messages.True : messages.False;
        }

        public bool IsTrue { get; }
        public CachingValidationType Type { get; }
        public string Message { get; }

        private class Messages
        {
            public string True { get; set; }
            public string False { get; set; }
        }
    }
}
