// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.TableControl;
using Microsoft.VisualStudio.Shell.TableManager;
using NuGet.Common;

namespace NuGet.VisualStudio.Common
{
    public class ErrorListTableEntry : ITableEntry
    {
        internal const string ErrorSouce = "NuGet";
        internal const string HelpLink = "https://docs.microsoft.com/nuget/reference/errors-and-warnings/{0}";

        public ILogMessage Message { get; }

        public object Identity => Message.Message;

        public ErrorListTableEntry(ILogMessage message)
        {
            Message = message;
        }

        public ErrorListTableEntry(string message, LogLevel level)
            : this(new LogMessage(level, message))
        {
        }

        public bool CanSetValue(string keyName)
        {
            return false;
        }

        public bool TryGetValue(string keyName, out object content)
        {
            content = null;

            switch (keyName)
            {
                case StandardTableKeyNames.Text:
                    content = Message.Message;
                    return true;
                case StandardTableKeyNames.ErrorSeverity:
                    content = GetErrorCategory(Message.Level);
                    return true;
                case StandardTableKeyNames.Priority:
                    content = "high";
                    return true;
                case StandardTableKeyNames.ErrorSource:
                    content = ErrorSouce;
                    return true;
                case StandardTableKeyNames.HelpKeyword:
                case StandardTableKeyNames.ErrorCode:
                    var result = false;

                    if (Message.Code > NuGetLogCode.Undefined)
                    {
                        result = Message.Code.TryGetName(out var codeString);
                        content = codeString;
                    }

                    return result;
                case StandardTableKeyNames.HelpLink:
                case StandardTableKeyNames.ErrorCodeToolTip:
                    result = false;

                    if (Message.Code > NuGetLogCode.Undefined)
                    {
                        result = Message.Code.TryGetName(out var codeString);
                        content = string.Format(CultureInfo.CurrentCulture, HelpLink, codeString);
                    }

                    return result;
                case StandardTableKeyNames.Line:

                    if (Message is RestoreLogMessage)
                    {
                        content = (Message as RestoreLogMessage).StartLineNumber;
                        return true;
                    }

                    return false;
                case StandardTableKeyNames.Column:

                    if (Message is RestoreLogMessage)
                    {
                        content = (Message as RestoreLogMessage).StartColumnNumber;
                        return true;
                    }

                    return false;
                case StandardTableKeyNames.DocumentName:
                    var documentName = GetProjectFile(Message);

                    if (!string.IsNullOrEmpty(documentName))
                    {
                        content = documentName;
                        return true;
                    }

                    return false;
                case StandardTableColumnDefinitions.ProjectName:
                    var projectName = GetProjectFile(Message);

                    if (!string.IsNullOrEmpty(projectName))
                    {
                        content = Path.GetFileNameWithoutExtension(projectName);
                        return true;
                    }

                    return false;
            }

            return false;
        }

        public bool TrySetValue(string keyName, object content)
        {
            content = null;
            return false;
        }

        private static __VSERRORCATEGORY GetErrorCategory(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    return __VSERRORCATEGORY.EC_ERROR;
                case LogLevel.Warning:
                    return __VSERRORCATEGORY.EC_WARNING;
                default:
                    return __VSERRORCATEGORY.EC_MESSAGE;
            }
        }

        private static string GetProjectFile(ILogMessage logMessage)
        {
            string file = null;
            if (!string.IsNullOrEmpty(logMessage.ProjectPath))
            {
                file = logMessage.ProjectPath;
            }
            else if (logMessage is RestoreLogMessage)
            {
                file = (logMessage as RestoreLogMessage).FilePath;
            }

            return file;
        }
    }
}
