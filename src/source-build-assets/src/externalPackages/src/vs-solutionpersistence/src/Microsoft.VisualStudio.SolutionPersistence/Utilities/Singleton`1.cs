// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Utilities;

// Helper struct that can be used in a pattern to
// create a singleton in a lazy thread safe way.
internal readonly struct Singleton<T>
    where T : new()
{
    internal static readonly T Instance;

    static Singleton()
    {
        Instance = new T();
    }
}
