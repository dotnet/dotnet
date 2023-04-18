// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

[assembly: CLSCompliant(true)]

// Type forwarding utility classes defined earlier in object model to a core utilities assembly.
[assembly: TypeForwardedTo(typeof(EqtTrace))]
[assembly: TypeForwardedTo(typeof(ValidateArg))]
