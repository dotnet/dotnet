// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.AsmDiff
{
    public sealed class DiffConfiguration
    {
        public DiffConfiguration()
        {
            Left = AssemblySet.Empty;
            Right = AssemblySet.Empty;
            Options = DiffConfigurationOptions.IncludeAdded |
                      DiffConfigurationOptions.IncludeRemoved |
                      DiffConfigurationOptions.IncludeChanged |
                      DiffConfigurationOptions.IncludeUnchanged |
                      DiffConfigurationOptions.IncludeAddedTypes |
                      DiffConfigurationOptions.IncludeRemovedTypes |
                      DiffConfigurationOptions.HighlightBaseMembers;
        }

        public DiffConfiguration(AssemblySet left, AssemblySet right, DiffConfigurationOptions options)
        {
            Left = left;
            Right = right;
            Options = options;
        }

        public AssemblySet Left { get; private set; }
        
        public AssemblySet Right { get; private set; }

        public bool IsDiff
        {
            get
            {
                var hasLeft = Left != null && !Left.IsEmpty;
                var hasRight = Right != null && !Right.IsEmpty;
                if (!hasLeft && !hasRight)
                    return false;

                return !hasLeft || hasRight;
            }
        }        
        
        public DiffConfigurationOptions Options { get; private set; }
    }
}
