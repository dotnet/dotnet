// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.Arcade.Common
{
    public interface ICommandFactory
    {
        ICommand Create(string executable, IEnumerable<string> args);
        ICommand Create(string executable, params string[] args);
        ICommand Create(string executable, string args);
    }
}
