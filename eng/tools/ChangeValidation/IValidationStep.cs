// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace ChangeValidation;

internal interface IValidationStep
{
    /// <summary>
    /// Executes the validation step with the provided file names.
    /// </summary>
    /// <param name="fileNames">List of file names to validate.</param>
    /// <returns>boolean determining the success of the validation step</returns>
    Task<bool> Validate(PrInfo prInfo);

    /// <summary>
    /// Gets the display name of the validation step.
    /// </summary>
    string DisplayName { get; }
}
