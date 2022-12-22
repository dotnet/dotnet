// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;
using System.Text;
using System.Windows.Media;
using NuGet.VisualStudio;
using LocalResources = NuGet.PackageManagement.PowerShellCmdlets.Resources;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    internal class NuGetHostUserInterface : PSHostUserInterface, IHostUISupportsMultipleChoiceSelection
    {
        public const ConsoleColor NoColor = (ConsoleColor)(-1);
        private const int VkCodeReturn = 13;
        private const int VkCodeBackspace = 8;
        private static Color[] ConsoleColors;
        private readonly NuGetPSHost _host;
        private readonly object _instanceLock = new object();
        private PSHostRawUserInterface _rawUI;

        public NuGetHostUserInterface(NuGetPSHost host)
        {
            UtilityMethods.ThrowIfArgumentNull(host);
            _host = host;
        }

        private IConsole Console
        {
            get { return _host.ActiveConsole; }
        }

        public override PSHostRawUserInterface RawUI
        {
            get
            {
                if (_rawUI == null)
                {
                    _rawUI = new NuGetRawUserInterface(_host);
                }
                return _rawUI;
            }
        }

        public Collection<int> PromptForChoice(
            string caption, string message, Collection<ChoiceDescription> choices, IEnumerable<int> defaultChoices)
        {
            WriteErrorLine("IHostUISupportsMultipleChoiceSelection.PromptForChoice not implemented.");
            return null;
        }

        private static Type GetFieldType(FieldDescription field)
        {
            Type type = null;
            if (!string.IsNullOrEmpty(field.ParameterAssemblyFullName))
            {
                LanguagePrimitives.TryConvertTo(field.ParameterAssemblyFullName, out type);
            }
            if ((type == null)
                && !string.IsNullOrEmpty(field.ParameterTypeFullName))
            {
                LanguagePrimitives.TryConvertTo(field.ParameterTypeFullName, out type);
            }
            return type;
        }

        public override Dictionary<string, PSObject> Prompt(
            string caption, string message, Collection<FieldDescription> descriptions)
        {
            if (descriptions == null)
            {
                throw new ArgumentNullException(nameof(descriptions));
            }
            if (descriptions.Count == 0)
            {
                // emulate powershell.exe behavior for empty collection.
                throw new ArgumentException(LocalResources.ZeroLengthCollection, nameof(descriptions));
            }

            if (!string.IsNullOrEmpty(caption))
            {
                WriteLine(caption);
            }
            if (!string.IsNullOrEmpty(message))
            {
                WriteLine(message);
            }

            // this stores the field/value pairs - e.g. unbound missing mandatory parameters,
            // or scripted $host.ui.prompt invocation.
            var results = new Dictionary<string, PSObject>(descriptions.Count);
            int index = 0;

            foreach (FieldDescription description in descriptions)
            {
                // if type is not resolvable, throw (as per powershell.exe)
                if (description == null
                    || string.IsNullOrEmpty(description.ParameterAssemblyFullName))
                {
                    throw new ArgumentException("descriptions[" + index + "]");
                }

                bool cancelled;
                object answer;
                string name = description.Name;

                // as per powershell.exe, if input value cannot be coerced to
                // parameter type then default to string.
                Type fieldType = GetFieldType(description) ?? typeof(string);

                // is parameter a collection type?
                if (typeof(IList).IsAssignableFrom(fieldType))
                {
                    // [int[]]$param1, [string[]]$param2
                    cancelled = PromptCollection(name, fieldType, out answer);
                }
                else
                {
                    //[int]$param1, [string]$param2
                    cancelled = PromptScalar(name, fieldType, out answer);
                }

                // user hit ESC?
                if (cancelled)
                {
                    WriteLine(string.Empty);
                    results.Clear();
                    break;
                }
                results.Add(name, PSObject.AsPSObject(answer));
                index++;
            }

            return results;
        }

        // parameter type is a scalar, like [int] or [string]
        private bool PromptScalar(string name, Type fieldType, out object answer)
        {
            bool cancelled;

            // if field a securestring, we prompt with masked input
            if (fieldType.Equals(typeof(SecureString)))
            {
                Write(name + ": ");
                answer = ReadLineAsSecureString();
                cancelled = (answer == null);
            }
            // if field is a credential type, we prompt with the secure dialog
            else if (fieldType.Equals(typeof(PSCredential)))
            {
                answer = PromptForCredential(null, null, null, string.Empty);
                cancelled = (answer == null);
            }
            else
            {
                // everything else is accepted as string, and coerced to the target type
                // if coercion fails, just pass as string.
                bool coercable = true;
                string prompt = name + ": ";
                do
                {
                    if (coercable)
                    {
                        // display field label as a prompt
                        Write(prompt);
                    }
                    else
                    {
                        // last input invalid, display in red
                        Write(prompt, ConsoleColor.DarkRed);
                    }
                    string line = ReadLine();
                    // user hit ESC?
                    cancelled = (line == null);
                    // can powershell turn this string into the field type?
                    coercable = LanguagePrimitives.TryConvertTo(line, fieldType, out answer);
                }
                while (!cancelled
                       && !coercable);
            }
            return cancelled;
        }

        // parameter type is a collection, like [int[]] or [string[]]
        private bool PromptCollection(string name, Type fieldType, out object answer)
        {
            bool cancelled;
            // we default to an object[] array
            Type elementType = typeof(object);

            if (fieldType.IsArray)
            {
                elementType = fieldType.GetElementType();
                // FIXME: zero rank array check?
            }

            // we will hold a list of the user's string input(s)
            var valuesToConvert = new ArrayList();
            bool coercable = true;

            while (true)
            {
                // prompt for collection element, suffixed with the current index
                string prompt = string.Format(
                    CultureInfo.CurrentCulture,
                    "{0}[{1}]: ", name, valuesToConvert.Count);

                if (coercable)
                {
                    Write(prompt);
                }
                else
                {
                    // last input invalid, display prompt in red
                    Write(prompt, ConsoleColor.DarkRed);
                }

                string input = ReadLine();
                // user hit ESC?
                cancelled = (input == null);

                // user hit ENTER on an empty line? we treat this as 
                // terminating the input for the collection prompting.
                bool inputComplete = string.IsNullOrEmpty(input);

                if (cancelled || inputComplete)
                {
                    break;
                }

                // can powershell convert this input to the element type?
                coercable = LanguagePrimitives.TryConvertTo(input, elementType, out answer);
                if (coercable)
                {
                    // yes, so store it
                    valuesToConvert.Add(answer);
                }
            }

            if (!cancelled)
            {
                // now, try to convert the entire collection of user inputs to the field's collection type
                if (!LanguagePrimitives.TryConvertTo(valuesToConvert, elementType, out answer))
                {
                    answer = valuesToConvert;
                }
            }
            else
            {
                answer = null;
            }
            return cancelled;
        }

        public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
        {
            if (!string.IsNullOrEmpty(caption))
            {
                WriteLine(caption);
            }

            if (!string.IsNullOrEmpty(message))
            {
                WriteLine(message);
            }

            int chosen = -1;
            do
            {
                // holds hotkeys, e.g. "[Y] Yes [N] No"
                var accelerators = new string[choices.Count];

                for (int index = 0; index < choices.Count; index++)
                {
                    ChoiceDescription choice = choices[index];
                    string label = choice.Label;
                    int ampIndex = label.IndexOf('&'); // hotkey marker
                    accelerators[index] = string.Empty; // default to empty

                    // accelerator marker found?
                    if (ampIndex != -1
                        && ampIndex < label.Length - 1)
                    {
                        // grab the letter after '&'
                        accelerators[index] = label
                            .Substring(ampIndex + 1, 1)
                            .ToUpper(CultureInfo.CurrentCulture);
                    }

                    Write(string.Format(CultureInfo.CurrentCulture, "[{0}] {1}  ",
                        accelerators[index],
                        // remove the redundant marker from output
                        label.Replace("&", string.Empty)));
                }

                Write(string.Format(CultureInfo.CurrentCulture, LocalResources.PromptForChoiceSuffix, accelerators[defaultChoice]));

                string input = ReadLine().Trim();
                switch (input.Length)
                {
                    case 0:
                        // enter, accept default if provided
                        if (defaultChoice == -1)
                        {
                            continue;
                        }
                        chosen = defaultChoice;
                        break;

                    case 1:
                        if (input[0] == '?')
                        {
                            // show help
                            for (int index = 0; index < choices.Count; index++)
                            {
                                WriteLine(string.Format(
                                    CultureInfo.CurrentCulture, "{0} - {1}.", accelerators[index], choices[index].HelpMessage));
                            }
                        }
                        else
                        {
                            // single letter accelerator, e.g. "Y"
                            chosen = Array.FindIndex(
                                accelerators,
                                accelerator => accelerator.Equals(
                                    input,
                                    StringComparison.OrdinalIgnoreCase));
                        }
                        break;

                    default:
                        // match against entire label, e.g. "Yes"
                        chosen = Array.FindIndex(
                            choices.ToArray(),
                            choice => choice.Label.Equals(
                                input,
                                StringComparison.OrdinalIgnoreCase));
                        break;
                }
            }
            while (chosen == -1);

            return chosen;
        }

        public override PSCredential PromptForCredential(
            string caption, string message, string userName, string targetName,
            PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
        {
            return NativeMethods.CredUIPromptForCredentials(
                caption,
                message,
                userName,
                targetName,
                allowedCredentialTypes,
                options);
        }

        public override PSCredential PromptForCredential(
            string caption, string message, string userName, string targetName)
        {
            return PromptForCredential(
                caption,
                message,
                userName,
                targetName,
                PSCredentialTypes.Default,
                PSCredentialUIOptions.Default);
        }

        public override string ReadLine()
        {
            try
            {
                var builder = new StringBuilder();

                lock (_instanceLock)
                {
                    KeyInfo keyInfo;
                    while ((keyInfo = RawUI.ReadKey()).VirtualKeyCode != VkCodeReturn)
                    {
                        // {enter}
                        if (keyInfo.VirtualKeyCode == VkCodeBackspace)
                        {
                            if (builder.Length > 0)
                            {
                                builder.Remove(builder.Length - 1, 1);
                                NuGetUIThreadHelper.JoinableTaskFactory.Run(() => Console.WriteBackspaceAsync());
                            }
                        }
                        else
                        {
                            builder.Append(keyInfo.Character);
                            // destined for output, so apply culture
                            Write(keyInfo.Character.ToString(CultureInfo.CurrentCulture));
                        }
                    }
                }

                return builder.ToString();
            }
            catch (PipelineStoppedException)
            {
                // ESC was hit
                return null;
            }
            finally
            {
                WriteLine(string.Empty);
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller's responsibility to dispose.")]
        public override SecureString ReadLineAsSecureString()
        {
            var secureString = new SecureString();
            try
            {
                lock (_instanceLock)
                {
                    KeyInfo keyInfo;
                    while ((keyInfo = RawUI.ReadKey()).VirtualKeyCode != VkCodeReturn)
                    {
                        // {enter}
                        if (keyInfo.VirtualKeyCode == VkCodeBackspace)
                        {
                            if (secureString.Length > 0)
                            {
                                secureString.RemoveAt(secureString.Length - 1);
                                NuGetUIThreadHelper.JoinableTaskFactory.Run(() => Console.WriteBackspaceAsync());
                            }
                        }
                        else
                        {
                            // culture is deferred until securestring is decrypted
                            secureString.AppendChar(keyInfo.Character);
                            Write("*");
                        }
                    }
                    secureString.MakeReadOnly();
                }
                return secureString;
            }
            catch (PipelineStoppedException)
            {
                // ESC was hit, clean up secure string
                secureString.Dispose();

                return null;
            }
            finally
            {
                WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// Convert a System.ConsoleColor enum to a Color value, or null if c is not a valid enum.
        /// </summary>
        private static Color? ToColor(ConsoleColor c)
        {
            if (ConsoleColors == null)
            {
                // colors copied from hkcu:\Console color table
                ConsoleColors = new Color[16]
                    {
                        Color.FromRgb(0x00, 0x00, 0x00),
                        Color.FromRgb(0x00, 0x00, 0x80),
                        Color.FromRgb(0x00, 0x80, 0x00),
                        Color.FromRgb(0x00, 0x80, 0x80),
                        Color.FromRgb(0x80, 0x00, 0x00),
                        Color.FromRgb(0x80, 0x00, 0x80),
                        Color.FromRgb(0x80, 0x80, 0x00),
                        Color.FromRgb(0xC0, 0xC0, 0xC0),
                        Color.FromRgb(0x80, 0x80, 0x80),
                        Color.FromRgb(0x00, 0x00, 0xFF),
                        Color.FromRgb(0x00, 0xFF, 0x00),
                        Color.FromRgb(0x00, 0xFF, 0xFF),
                        Color.FromRgb(0xFF, 0x00, 0x00),
                        Color.FromRgb(0xFF, 0x00, 0xFF),
                        Color.FromRgb(0xFF, 0xFF, 0x00),
                        Color.FromRgb(0xFF, 0xFF, 0xFF)
                    };
            }

            var i = (int)c;
            if (i >= 0
                && i < ConsoleColors.Length)
            {
                return ConsoleColors[i];
            }

            return null; // invalid color
        }

        public override void Write(string value)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(() => Console.WriteAsync(value));
        }

        public override void WriteLine(string value)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(() => Console.WriteLineAsync(value));
        }

        private void Write(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor = NoColor)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(() => Console.WriteAsync(value, ToColor(foregroundColor), ToColor(backgroundColor)));
        }

        private void WriteLine(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor = NoColor)
        {
            // If append \n only, text becomes 1 line when copied to notepad.
            Write(value + Environment.NewLine, foregroundColor, backgroundColor);
        }

        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            Write(value, foregroundColor, backgroundColor);
        }

        public override void WriteDebugLine(string message)
        {
            WriteLine(message, ConsoleColor.DarkGray);
        }

        public override void WriteErrorLine(string value)
        {
            WriteLine(value, foregroundColor: ConsoleColor.White, backgroundColor: ConsoleColor.DarkRed);
        }

        public override void WriteProgress(long sourceId, ProgressRecord record)
        {
            string operation = record.CurrentOperation ?? record.StatusDescription;
            if (!string.IsNullOrEmpty(operation))
            {
                NuGetUIThreadHelper.JoinableTaskFactory.Run(() => Console.WriteProgressAsync(operation, record.PercentComplete));
            }
        }

        public override void WriteVerboseLine(string message)
        {
            WriteLine(message, ConsoleColor.DarkGray);
        }

        public override void WriteWarningLine(string message)
        {
            WriteLine(message, foregroundColor: ConsoleColor.Black, backgroundColor: ConsoleColor.Yellow);
        }
    }
}
