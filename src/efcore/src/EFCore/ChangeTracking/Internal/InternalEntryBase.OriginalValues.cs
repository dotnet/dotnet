// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public partial class InternalEntryBase
{
    private struct OriginalValues(InternalEntryBase entry)
    {
        private ISnapshot _values = entry.StructuralType.OriginalValuesFactory(entry);

        public object? GetValue(IInternalEntry entry, IPropertyBase property)
            => property.GetOriginalValueIndex() is var index && index == -1
                ? throw new InvalidOperationException(
                    CoreStrings.OriginalValueNotTracked(property.Name, property.DeclaringType.DisplayName()))
                : IsEmpty
                    ? entry[property]
                    : _values[index];

        public T GetValue<T>(IInternalEntry entry, IPropertyBase property, int index)
            => index == -1
                ? throw new InvalidOperationException(
                    CoreStrings.OriginalValueNotTracked(property.Name, property.DeclaringType.DisplayName()))
                : IsEmpty
                    ? entry.GetCurrentValue<T>(property)
                    : _values.GetValue<T>(index);

        public void SetValue(IPropertyBase property, object? value, int index)
        {
            Check.DebugAssert(!IsEmpty, "Original values are empty");

            if (index == -1)
            {
                index = property.GetOriginalValueIndex();

                if (index == -1)
                {
                    throw new InvalidOperationException(
                        CoreStrings.OriginalValueNotTracked(property.Name, property.DeclaringType.DisplayName()));
                }
            }

            if (value == null
                && !property.ClrType.IsNullableType())
            {
                throw new InvalidOperationException(
                    CoreStrings.ValueCannotBeNull(
                        property.Name, property.DeclaringType.DisplayName(), property.ClrType.DisplayName()));
            }

            _values[index] = SnapshotValue(property, value);
        }

        public void RejectChanges(IInternalEntry entry)
        {
            if (IsEmpty)
            {
                return;
            }

            foreach (var property in entry.StructuralType.GetFlattenedProperties())
            {
                var index = property.GetOriginalValueIndex();
                if (index >= 0)
                {
                    entry[property] = SnapshotValue(property, _values[index]);
                }
            }
        }

        public void AcceptChanges(IInternalEntry entry)
        {
            if (IsEmpty)
            {
                return;
            }

            _values = entry.StructuralType.OriginalValuesFactory(entry);
        }

        private static object? SnapshotValue(IPropertyBase propertyBase, object? value)
        {
            if (propertyBase is IProperty property)
            {
                return property.GetValueComparer().Snapshot(value);
            }

            if (propertyBase is IComplexProperty complexProperty && complexProperty.IsCollection && value is IList list)
            {
                return SnapshotFactoryFactory.SnapshotComplexCollection(list, (IRuntimeComplexProperty)complexProperty);
            }

            return value;
        }

        public bool IsEmpty
            => _values == null;
    }
}
