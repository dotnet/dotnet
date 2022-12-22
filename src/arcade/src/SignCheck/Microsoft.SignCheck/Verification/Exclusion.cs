// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Microsoft.SignCheck.Verification
{
    /// <summary>
    /// Represents an exclusion describing
    /// </summary>
    public class Exclusion
    {
        private string[] _exclusionParts;
        private const int FilePatternsIndex = 0;
        private const int ParentFilesIndex = 1;
        private const int CommentIndex = 2;

        /// <summary>
        /// Creates a new <see cref="Exclusion"/>.
        /// </summary>
        /// <param name="exclusion">A string representation of a file exclusion. An exclusion contains a number of fields, separated by
        /// a ';'. The entry is formated as FILE_PATTERNS;PARENT_FILES;COMMENT. Additional fields are ignored and fields may be left
        /// empty, e.g. ";B.txt" indicates an exclusion with no file patterns and one parent file.
        ///
        /// The FILE_PATTERNS and PARENT_FILES fields may contain multiple values separated by a '|'.
        ///
        /// For example: "A.txt|C:\Dir1\B.txt;C.zip;" indicates an exclusion with two file patterns ("A.txt" and "C:\Dir1\B.txt") and one
        /// parent file ("C.zip").
        /// </param>
        public Exclusion(string exclusion)
        {
            if (!String.IsNullOrEmpty(exclusion))
            {
                _exclusionParts = exclusion.Split(';');
            }
        }

        public string Comment
        {
            get
            {
                return GetExclusionPart(CommentIndex, defaultValue: SignCheckResources.NA);
            }
        }

        /// <summary>
        /// Returns an array of file patterns or null if there are no entries. Each file pattern is separated by '|'.
        /// </summary>
        public string[] FilePatterns
        {
            get
            {
                return GetExclusionPart(FilePatternsIndex).Split('|');
            }
        }

        /// <summary>
        /// Returns an array of parent files or null if there are no entries. Each parent file is separated by '|'.
        /// </summary>
        public string[] ParentFiles
        {
            get
            {
                return GetExclusionPart(ParentFilesIndex).Split('|');
            }
        }

        public override string ToString()
        {
            return String.Format("FilePattern: {0} | Parent: {1} | Comment: {2}", FilePatterns, ParentFiles, Comment);
        }

        private string GetExclusionPart(int index)
        {
            return GetExclusionPart(index, defaultValue: String.Empty);
        }

        private string GetExclusionPart(int index, string defaultValue)
        {
            if ((_exclusionParts != null) && (index >= 0) && (index < _exclusionParts.Length))
            {
                return _exclusionParts[index];
            }

            return defaultValue;
        }
    }
}
