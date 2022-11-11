// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.CommandLine.XPlat.ListPackage
{
    /// <summary>
    /// Report problem text with problem type for a project
    /// </summary>
    internal class ReportProblem
    {
        internal string Project { get; private set; }
        internal string Text { get; private set; }
        internal ProblemType ProblemType { get; }

        private ReportProblem()
        { }

        public ReportProblem(ProblemType problemType, string project, string text)
        {
            ProblemType = problemType;
            Project = project;
            Text = text;
        }
    }
}
