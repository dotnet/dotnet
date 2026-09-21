// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

internal static class TestSessionInfoFactory
{
    public static TestSessionInfo Create(Guid id)
    {
        try
        {
            return CreateWithCurrentObjectModel(id);
        }
        catch (MissingMethodException)
        {
            return CreateWithCompatibleObjectModel(id);
        }
        catch (MethodAccessException)
        {
            return CreateWithCompatibleObjectModel(id);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static TestSessionInfo CreateWithCurrentObjectModel(Guid id)
    {
        return new TestSessionInfo(id);
    }

    private static TestSessionInfo CreateWithCompatibleObjectModel(Guid id)
    {
        var testSessionInfo = new TestSessionInfo();
        typeof(TestSessionInfo).GetProperty(nameof(TestSessionInfo.Id), BindingFlags.Public | BindingFlags.Instance)!
            .SetValue(testSessionInfo, id);

        return testSessionInfo;
    }
}
