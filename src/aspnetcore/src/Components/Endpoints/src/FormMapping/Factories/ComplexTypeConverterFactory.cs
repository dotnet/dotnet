// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Endpoints.FormMapping.Metadata;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Components.Endpoints.FormMapping;

// This factory is registered last, which means, dictionaries and collections, have already
// been processed by the time we get here.
internal class ComplexTypeConverterFactory(FormDataMapperOptions options) : IFormDataConverterFactory
{
    internal FormDataMetadataFactory MetadataFactory { get; } = new FormDataMetadataFactory(options.Factories);

    [RequiresDynamicCode(FormMappingHelpers.RequiresDynamicCodeMessage)]
    [RequiresUnreferencedCode(FormMappingHelpers.RequiresUnreferencedCodeMessage)]
    public bool CanConvert(Type type, FormDataMapperOptions options)
    {
        if (type.IsGenericTypeDefinition)
        {
            return false;
        }

        var constructors = type.GetConstructors();
        if (constructors.Length > 1 || (constructors.Length == 0 && !type.IsValueType))
        {
            // We can't select the constructor when there are multiple of them.
            return false;
        }

        if (MetadataFactory.HasMetadataFor(type))
        {
            return true;
        }

        // Create the metadata for the type. This walks the graph and creates metadata for all the types
        // in the reference graph, detecting and identifying recursive types.
        MetadataFactory.GetOrCreateMetadataFor(type, options);

        // Check that all properties have a valid converter.
        var propertyHelper = PropertyHelper.GetVisibleProperties(type);
        foreach (var helper in propertyHelper)
        {
            if (!options.CanConvert(helper.Property.PropertyType))
            {
                return false;
            }
        }

        return true;
    }

    // We are going to compile a function that maps all the properties for the type.
    // Beware that the code below is not the actual exact code, just a simplification to understand what is happening at a high level.
    // The general flow is as follows. For a type like Address { Street, City, Country, ZipCode }
    // we will generate a function that looks like:
    // public bool TryRead(ref FormDataReader reader, Type type, FormDataSerializerOptions options, out Address? result, out bool found)
    // {
    //     bool foundProperty;
    //     bool succeeded = true;
    //     string street;
    //     string city;
    //     string country;
    //     string zipCode;
    //     FormDataConveter<string> streetConverter;
    //     FormDataConveter<string> cityConverter;
    //     FormDataConveter<string> countryConverter;
    //     FormDataConveter<string> zipCodeConverter;

    //     var streetConverter = options.ResolveConverter(typeof(string));
    //     reader.PushPrefix("Street");
    //     succeeded &= streetConverter.TryRead(ref reader, typeof(string), options, out street, out foundProperty);
    //     found ||= foundProperty;
    //     reader.PopPrefix("Street");
    //
    //     var cityConverter = options.ResolveConverter(typeof(string));
    //     reader.PushPrefix("City");
    //     succeeded &= ciryConverter.TryRead(ref reader, typeof(string), options, out street, out foundProperty);
    //     found ||= foundProperty;
    //     reader.PopPrefix("City");
    //
    //     var countryConverter = options.ResolveConverter(typeof(string));
    //     reader.PushPrefix("Country");
    //     succeeded &= countryConverter.TryRead(ref reader, typeof(string), options, out street, out foundProperty);
    //     found ||= foundProperty;
    //     reader.PopPrefix("Country");
    //
    //     var zipCodeConverter = options.ResolveConverter(typeof(string));
    //     reader.PushPrefix("ZipCode");
    //     succeeded &= zipCodeConverter.TryRead(ref reader, typeof(string), options, out street, out foundProperty);
    //     found ||= foundProperty;
    //     reader.PopPrefix("ZipCode");
    //
    //     if(found)
    //     {
    //         result = new Address();
    //         result.Street = street;
    //         result.City = city;
    //         result.Country = country;
    //         result.ZipCode = zipCode;
    //     }
    //     else
    //     {
    //         result = null;
    //     }
    //
    //     return succeeded;
    // }
    //
    // The actual blocks above are going to be generated using System.Linq.Expressions.
    // Instead of resolving the property converters every time, we might consider caching the converters in a dictionary and passing an
    // extra parameter to the function with them in it.
    // The final converter is something like
    // internal class CompiledComplexTypeConverter
    //     (ConverterDelegate<FormDataReader, Type, FormDataSerializerOptions, out object, out bool> converterFunc)
    // {
    //     public bool TryRead(ref FormDataReader reader, Type type, FormDataSerializerOptions options, out object? result, out bool found)
    //     {
    //         return converterFunc(ref reader, type, options, out result, out found);
    //     }
    // }
    [RequiresDynamicCode(FormMappingHelpers.RequiresDynamicCodeMessage)]
    [RequiresUnreferencedCode(FormMappingHelpers.RequiresUnreferencedCodeMessage)]
    public FormDataConverter CreateConverter(Type type, FormDataMapperOptions options)
    {
        if (Activator.CreateInstance(typeof(ComplexTypeExpressionConverterFactory<>).MakeGenericType(type), MetadataFactory)
            is not ComplexTypeExpressionConverterFactory factory)
        {
            throw new InvalidOperationException($"Could not create a converter factory for type {type}.");
        }

        return factory.CreateConverter(type, options);
    }
}
