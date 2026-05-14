// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.CodeAnalysis
{
    internal enum DiagnosticIds
    {
        BCL0001, // MemberMustExist

        // AppContext related 
        BCL0010, // AppContextDefaultNotInitializedToTrueDiagnostic
        BCL0011, // AppContextDefaultUsedUnexpectedIfStatement
        BCL0012, // DefaultValueDefinedOutsideIfCondition

        BCL0015, // PinvokeCallCheck

        BCL0020, // ResourceUsageCheck
    }
}
