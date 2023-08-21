// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Routing.Constraints;
#if COMPONENTS
using Microsoft.AspNetCore.Components.Routing;
#endif

namespace Microsoft.AspNetCore.Routing;

#if !COMPONENTS
/// <summary>
/// A builder for produding a mapping of keys to see <see cref="IRouteConstraint"/>.
/// </summary>
/// <remarks>
/// <see cref="RouteConstraintBuilder"/> allows iterative building a set of route constraints, and will
/// merge multiple entries for the same key.
/// </remarks>
public class RouteConstraintBuilder
#else
internal class RouteConstraintBuilder
#endif
{
    private readonly IInlineConstraintResolver _inlineConstraintResolver;
    private readonly string _displayName;

    private readonly Dictionary<string, List<IRouteConstraint>> _constraints;
    private readonly HashSet<string> _optionalParameters;
    /// <summary>
    /// Creates a new instance of <see cref="RouteConstraintBuilder"/> instance.
    /// </summary>
    /// <param name="inlineConstraintResolver">The <see cref="IInlineConstraintResolver"/>.</param>
    /// <param name="displayName">The display name (for use in error messages).</param>
    public RouteConstraintBuilder(
        IInlineConstraintResolver inlineConstraintResolver,
        string displayName)
    {
        ArgumentNullException.ThrowIfNull(inlineConstraintResolver);
        ArgumentNullException.ThrowIfNull(displayName);

        _inlineConstraintResolver = inlineConstraintResolver;
        _displayName = displayName;

        _constraints = new Dictionary<string, List<IRouteConstraint>>(StringComparer.OrdinalIgnoreCase);
        _optionalParameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Builds a mapping of constraints.
    /// </summary>
    /// <returns>An <see cref="IDictionary{String, IRouteConstraint}"/> of the constraints.</returns>
    public IDictionary<string, IRouteConstraint> Build()
    {
        var constraints = new Dictionary<string, IRouteConstraint>(StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in _constraints)
        {
            IRouteConstraint constraint;
            if (kvp.Value.Count == 1)
            {
                constraint = kvp.Value[0];
            }
            else
            {
                constraint = new CompositeRouteConstraint(kvp.Value.ToArray());
            }

            if (_optionalParameters.Contains(kvp.Key))
            {
                var optionalConstraint = new OptionalRouteConstraint(constraint);
                constraints.Add(kvp.Key, optionalConstraint);
            }
            else
            {
                constraints.Add(kvp.Key, constraint);
            }
        }

        return constraints;
    }

#if !COMPONENTS
    /// <summary>
    /// Adds a constraint instance for the given key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">
    /// The constraint instance. Must either be a string or an instance of <see cref="IRouteConstraint"/>.
    /// </param>
    /// <remarks>
    /// If the <paramref name="value"/> is a string, it will be converted to a <see cref="RegexRouteConstraint"/>.
    ///
    /// For example, the string <c>Product[0-9]+</c> will be converted to the regular expression
    /// <c>^(Product[0-9]+)</c>. See <see cref="System.Text.RegularExpressions.Regex"/> for more details.
    /// </remarks>
    public void AddConstraint(string key, object value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        var constraint = value as IRouteConstraint;
        if (constraint == null)
        {
            var regexPattern = value as string;
            if (regexPattern == null)
            {
                throw new RouteCreationException(
                    Resources.FormatRouteConstraintBuilder_ValidationMustBeStringOrCustomConstraint(
                        key,
                        value,
                        _displayName,
                        typeof(IRouteConstraint)));
            }

            var constraintsRegEx = "^(" + regexPattern + ")$";
            constraint = new RegexRouteConstraint(constraintsRegEx);
        }

        Add(key, constraint);
    }
#endif

    /// <summary>
    /// Adds a constraint for the given key, resolved by the <see cref="IInlineConstraintResolver"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="constraintText">The text to be resolved by <see cref="IInlineConstraintResolver"/>.</param>
    /// <remarks>
    /// The <see cref="IInlineConstraintResolver"/> can create <see cref="IRouteConstraint"/> instances
    /// based on <paramref name="constraintText"/>. See <see cref="RouteOptions.ConstraintMap"/> to register
    /// custom constraint types.
    /// </remarks>
    public void AddResolvedConstraint(string key, string constraintText)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(constraintText);

        var constraint = _inlineConstraintResolver.ResolveConstraint(constraintText);
        if (constraint == null)
        {
            throw new InvalidOperationException(
                Resources.FormatRouteConstraintBuilder_CouldNotResolveConstraint(
                    key,
                    constraintText,
                    _displayName,
                    _inlineConstraintResolver.GetType().Name));
        }
        else if (constraint == NullRouteConstraint.Instance)
        {
            // A null route constraint can be returned for other parameter policy types
            return;
        }

        Add(key, constraint);
    }

    /// <summary>
    /// Sets the given key as optional.
    /// </summary>
    /// <param name="key">The key.</param>
    public void SetOptional(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        _optionalParameters.Add(key);
    }

    private void Add(string key, IRouteConstraint constraint)
    {
        if (!_constraints.TryGetValue(key, out var list))
        {
            list = new List<IRouteConstraint>();
            _constraints.Add(key, list);
        }

        list.Add(constraint);
    }
}
