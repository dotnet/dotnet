// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Resources.Container.Utils;

internal class EncodingUtils
{
    /// <summary>
    /// Checks if the string is valid hex.
    /// </summary>
    /// <param name="hexString">string.</param>
    /// <returns>true if valid else false.</returns>
    public static bool IsValidHexString(IEnumerable<char> hexString)
    {
        return hexString.All(currentCharacter => currentCharacter is (>= '0' and <= '9') or (>= 'a' and <= 'f') or (>= 'A' and <= 'F'));
    }
}
