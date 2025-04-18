// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
// Description: Encapsulates IOptionDescription interface exposed 
//              by MsSpellCheckLib.RCW and exposes a safe type
//              that is expressed in terms of .NET types.
//

using System.Runtime.InteropServices;
using IOptionDescription = System.Windows.Documents.MsSpellCheckLib.RCW.IOptionDescription;

namespace System.Windows.Documents.MsSpellCheckLib
{
    internal partial class SpellChecker
    {
        /// <summary>
        /// Represents WPF's internal encapsulation of IOptionDescription
        /// </summary>
        internal class OptionDescription
        {
            /// <summary>
            /// Fields corresponding to the ones present in IOptionDescription
            /// </summary>
            #region IOptionDescription Fields

            internal string Id { get; private set; }
            internal string Heading { get; private set; }
            internal string Description { get; private set; }

            private List<string> _labels;
            internal IReadOnlyCollection<string> Labels
            {
                get
                {
                    return _labels.AsReadOnly();
                }
            }

            #endregion // IOptionDescription Fields

            /// <summary>
            /// Private constructor to prevent direct instantiation 
            /// </summary>
            private OptionDescription(string id, string heading, string description, List<string> labels = null)
            {
                Id = id;
                Heading = heading;
                Description = description;

                _labels = labels ?? new List<string>();
            }


            /// <summary>
            /// Creates an instance of OptionDescription from a handle to IOptionDescription
            /// </summary>
            internal static OptionDescription Create(IOptionDescription optionDescription, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
            {
                ArgumentNullException.ThrowIfNull(optionDescription);

                var od = new OptionDescription(optionDescription.Id, optionDescription.Heading, optionDescription.Description);

                try
                {
                    od._labels = optionDescription.Labels.ToList();
                }
                catch (COMException) when (shouldSuppressCOMExceptions)
                {
                    // do nothing here - the exception filter does it all.
                }
                finally
                {
                    if (shouldReleaseCOMObject)
                    {
                        Marshal.ReleaseComObject(optionDescription);
                    }
                }

                return od;
            }
        }
    }
}
