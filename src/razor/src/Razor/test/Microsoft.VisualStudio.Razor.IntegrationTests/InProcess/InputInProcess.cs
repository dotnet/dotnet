// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Windows.Forms;

namespace Microsoft.VisualStudio.Extensibility.Testing;

[TestService]
internal partial class InputInProcess
{
    internal void Send(string keys)
    {
        SendKeys.Send(keys);
    }
}

