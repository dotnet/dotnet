// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.CommandLine.XPlat.ListPackage
{
    internal interface IReportRenderer
    {
        void AddProblem(ProblemType problemType, string text);
        IEnumerable<ReportProblem> GetProblems();
        void Render(ListPackageReportModel reportProject);
    }
}
