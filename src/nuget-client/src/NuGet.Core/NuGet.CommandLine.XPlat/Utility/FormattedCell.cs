// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.CommandLine.XPlat.Utility
{
    internal class FormattedCell : IEquatable<FormattedCell>
    {
        public string Value { get; set; }
        public ConsoleColor? ForegroundColor { get; set; }

        public FormattedCell() : this(string.Empty) { }

        public FormattedCell(string value, ConsoleColor? foregroundColor = null)
        {
            Value = value ?? string.Empty;
            ForegroundColor = foregroundColor;
        }

        public bool Equals(FormattedCell other) => Value == other?.Value && ForegroundColor == other?.ForegroundColor;

        public override bool Equals(object obj) => Equals(obj as FormattedCell);

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(StringComparer.Ordinal.GetHashCode(Value));
            combiner.AddStruct(ForegroundColor);

            return combiner.CombinedHash;
        }
    }
}
