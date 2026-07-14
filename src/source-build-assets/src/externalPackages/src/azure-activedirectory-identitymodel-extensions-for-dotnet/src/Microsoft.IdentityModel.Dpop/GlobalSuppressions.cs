// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Returns structured DpopValidationResult instead of throwing", Scope = "member", Target = "~M:Microsoft.IdentityModel.Dpop.DpopProofValidator.ValidateAsync(System.String,System.String,System.Uri,System.String,System.String,Microsoft.IdentityModel.Dpop.DpopValidationOptions,System.Threading.CancellationToken)~System.Threading.Tasks.Task{Microsoft.IdentityModel.Dpop.DpopValidationResult}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Returns structured DpopValidationResult for malformed JWK input", Scope = "member", Target = "~M:Microsoft.IdentityModel.Dpop.DpopProofValidator.ValidateCoreAsync(System.String,System.String,System.Uri,System.String,System.String,Microsoft.IdentityModel.Dpop.DpopValidationOptions,System.Threading.CancellationToken)~System.Threading.Tasks.Task{Microsoft.IdentityModel.Dpop.DpopValidationResult}")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Options pattern requires settable collections", Scope = "member", Target = "~P:Microsoft.IdentityModel.Dpop.DpopValidationOptions.AllowedSigningAlgorithms")]
