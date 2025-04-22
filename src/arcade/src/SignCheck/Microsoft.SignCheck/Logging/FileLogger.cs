// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Xml;
using Microsoft.SignCheck.Verification;

namespace Microsoft.SignCheck.Logging
{
    public class FileLogger : LoggerBase, ILogger
    {
        internal StreamWriter ErrorWriter
        {
            get;
            set;
        }

        internal StreamWriter MessageWriter
        {
            get;
            set;
        }

        internal XmlWriter ResultsWriter
        {
            get;
            set;
        }

        internal long Lines
        {
            get;
            set;
        }

        public FileLogger(LogVerbosity verbosity, string messageFile, string errorFile, string resultsFile) : base(verbosity)
        {
            if (!String.IsNullOrEmpty(messageFile))
            {
                if (!Directory.Exists(messageFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(messageFile)));
                }
                MessageWriter = File.CreateText(messageFile);
            }

            if (!String.IsNullOrEmpty(errorFile))
            {
                if (!Directory.Exists(errorFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(errorFile)));
                }
                ErrorWriter = File.CreateText(errorFile);
                ErrorWriter.AutoFlush = true;
            }

            if (!String.IsNullOrEmpty(resultsFile))
            {
                if (!Directory.Exists(resultsFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(resultsFile)));
                }
                ResultsWriter = XmlWriter.Create(resultsFile, new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t",
                    OmitXmlDeclaration = true
                });
                ResultsWriter.WriteStartDocument();
                ResultsWriter.WriteStartElement("SignCheckResults");
            }

            Lines = 0;
        }

        public override void Close()
        {
            if (MessageWriter != null)
            {
                MessageWriter.Flush();
                MessageWriter.Close();
            }

            if (ErrorWriter != null)
            {
                ErrorWriter.Flush();
                ErrorWriter.Close();
            }

            if (ResultsWriter != null)
            {
                ResultsWriter.WriteEndElement();
                ResultsWriter.Close();
            }
        }

        public void WriteMessage(string message)
        {
            if (MessageWriter != null)
            {
                MessageWriter.WriteLine(message);
                // periodically flush the log in case it's a large scan 
                Lines++;

                if (Lines % 10 == 0)
                {
                    MessageWriter.Flush();
                }
            }
        }

        public void WriteMessage(LogVerbosity verbosity, string message)
        {
            if (verbosity <= Verbosity)
            {
                WriteMessage(message);
            }
        }

        public void WriteMessage(LogVerbosity verbosity, string message, params object[] values)
        {
            if (verbosity <= Verbosity)
            {
                WriteMessage(String.Format(message, values));
            }
        }

        public void WriteError(string message)
        {
            HasLoggedErrors = true;
            if (ErrorWriter != null)
            {
                ErrorWriter.WriteLine(message);
                ErrorWriter.Flush();
            }
        }

        public void WriteError(LogVerbosity verbosity, string message)
        {
            if (verbosity <= Verbosity)
            {
                WriteError(message);
            }
        }

        public void WriteError(LogVerbosity verbosity, string message, params object[] values)
        {
            if (verbosity <= Verbosity)
            {
                WriteError(String.Format(message, values));
            }
        }

        public void WriteLine()
        {
            if (MessageWriter != null)
            {
                MessageWriter.WriteLine();
                Lines++;
                if (Lines % 10 == 0)
                {
                    MessageWriter.Flush();
                }
            }
        }

        public void WriteStartResult(SignatureVerificationResult result, string outcome)
        {
            if (ResultsWriter != null)
            {
                ResultsWriter.WriteStartElement("File");
                ResultsWriter.WriteAttributeString("Name", result.VirtualPath);
                ResultsWriter.WriteAttributeString("Outcome", outcome);
                foreach (var detail in DetailKeys.ResultKeysVerbose)
                {
                    if (detail == DetailKeys.File)
                    {
                        // Skip the file detail because it's already written as an attribute
                        continue;
                    }
                    string value = result.ToString(detail);
                    if (!String.IsNullOrEmpty(value))
                    {
                        ResultsWriter.WriteAttributeString(detail.ToString(), value);
                    }
                }
            }
        }

        public void WriteEndResult()
        {
            if (ResultsWriter != null)
            {
                ResultsWriter.WriteEndElement();
            }
        }
    }
}
