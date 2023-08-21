// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Microsoft.AspNetCore.Mvc.TagHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting &lt;input&gt; elements with an <c>asp-for</c> attribute.
/// </summary>
[HtmlTargetElement("input", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
public class InputTagHelper : TagHelper
{
    private const string ForAttributeName = "asp-for";
    private const string FormatAttributeName = "asp-format";

    // Mapping from datatype names and data annotation hints to values for the <input/> element's "type" attribute.
    private static readonly Dictionary<string, string> _defaultInputTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "HiddenInput", InputType.Hidden.ToString().ToLowerInvariant() },
            { "Password", InputType.Password.ToString().ToLowerInvariant() },
            { "Text", InputType.Text.ToString().ToLowerInvariant() },
            { "PhoneNumber", "tel" },
            { "Url", "url" },
            { "EmailAddress", "email" },
            { "Date", "date" },
            { "DateTime", "datetime-local" },
            { "DateTime-local", "datetime-local" },
            { nameof(DateTimeOffset), "text" },
            { "Time", "time" },
            { "Week", "week" },
            { "Month", "month" },
            { nameof(Byte), "number" },
            { nameof(SByte), "number" },
            { nameof(Int16), "number" },
            { nameof(UInt16), "number" },
            { nameof(Int32), "number" },
            { nameof(UInt32), "number" },
            { nameof(Int64), "number" },
            { nameof(UInt64), "number" },
            { nameof(Single), InputType.Text.ToString().ToLowerInvariant() },
            { nameof(Double), InputType.Text.ToString().ToLowerInvariant() },
            { nameof(Boolean), InputType.CheckBox.ToString().ToLowerInvariant() },
            { nameof(Decimal), InputType.Text.ToString().ToLowerInvariant() },
            { nameof(String), InputType.Text.ToString().ToLowerInvariant() },
            { nameof(IFormFile), "file" },
            { TemplateRenderer.IEnumerableOfIFormFileName, "file" },
        };

    // Mapping from <input/> element's type to RFC 3339 date and time formats.
    private static readonly Dictionary<string, string> _rfc3339Formats =
        new(StringComparer.Ordinal)
        {
            { "date", "{0:yyyy-MM-dd}" },
            { "datetime", @"{0:yyyy-MM-ddTHH\:mm\:ss.fffK}" },
            { "datetime-local", @"{0:yyyy-MM-ddTHH\:mm\:ss.fff}" },
            { "time", @"{0:HH\:mm\:ss.fff}" },
        };

    /// <summary>
    /// Creates a new <see cref="InputTagHelper"/>.
    /// </summary>
    /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
    public InputTagHelper(IHtmlGenerator generator)
    {
        Generator = generator;
    }

    /// <inheritdoc />
    public override int Order => -1000;

    /// <summary>
    /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="InputTagHelper"/>'s output.
    /// </summary>
    protected IHtmlGenerator Generator { get; }

    /// <summary>
    /// Gets the <see cref="Rendering.ViewContext"/> of the executing view.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression For { get; set; }

    /// <summary>
    /// The format string (see <see href="https://msdn.microsoft.com/en-us/library/txafckwd.aspx"/>) used to format the
    /// <see cref="For"/> result. Sets the generated "value" attribute to that formatted string.
    /// </summary>
    /// <remarks>
    /// Not used if the provided (see <see cref="InputTypeName"/>) or calculated "type" attribute value is
    /// <c>checkbox</c>, <c>password</c>, or <c>radio</c>. That is, <see cref="Format"/> is used when calling
    /// <see cref="IHtmlGenerator.GenerateTextBox"/>.
    /// </remarks>
    [HtmlAttributeName(FormatAttributeName)]
    public string Format { get; set; }

    /// <summary>
    /// The type of the &lt;input&gt; element.
    /// </summary>
    /// <remarks>
    /// Passed through to the generated HTML in all cases. Also used to determine the <see cref="IHtmlGenerator"/>
    /// helper to call and the default <see cref="Format"/> value. A default <see cref="Format"/> is not calculated
    /// if the provided (see <see cref="InputTypeName"/>) or calculated "type" attribute value is <c>checkbox</c>,
    /// <c>hidden</c>, <c>password</c>, or <c>radio</c>.
    /// </remarks>
    [HtmlAttributeName("type")]
    public string InputTypeName { get; set; }

    /// <summary>
    /// The name of the associated form
    /// </summary>
    /// <remarks>
    /// Used to associate a hidden checkbox tag to the respecting form when <see cref="CheckBoxHiddenInputRenderMode"/> is not <see cref="CheckBoxHiddenInputRenderMode.None"/>.
    /// </remarks>
    [HtmlAttributeName("form")]
    public string FormName { get; set; }

    /// <summary>
    /// The name of the &lt;input&gt; element.
    /// </summary>
    /// <remarks>
    /// Passed through to the generated HTML in all cases. Also used to determine whether <see cref="For"/> is
    /// valid with an empty <see cref="ModelExpression.Name"/>.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// The value of the &lt;input&gt; element.
    /// </summary>
    /// <remarks>
    /// Passed through to the generated HTML in all cases. Also used to determine the generated "checked" attribute
    /// if <see cref="InputTypeName"/> is "radio". Must not be <c>null</c> in that case.
    /// </remarks>
    public string Value { get; set; }

    /// <inheritdoc />
    /// <remarks>Does nothing if <see cref="For"/> is <c>null</c>.</remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <see cref="Format"/> is non-<c>null</c> but <see cref="For"/> is <c>null</c>.
    /// </exception>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);

        // Pass through attributes that are also well-known HTML attributes. Must be done prior to any copying
        // from a TagBuilder.
        if (InputTypeName != null)
        {
            output.CopyHtmlAttribute("type", context);
        }

        if (Name != null)
        {
            output.CopyHtmlAttribute(nameof(Name), context);
        }

        if (Value != null)
        {
            output.CopyHtmlAttribute(nameof(Value), context);
        }

        if (FormName != null)
        {
            output.CopyHtmlAttribute("form", context);
        }

        // Note null or empty For.Name is allowed because TemplateInfo.HtmlFieldPrefix may be sufficient.
        // IHtmlGenerator will enforce name requirements.
        var metadata = For.Metadata;
        var modelExplorer = For.ModelExplorer;
        if (metadata == null)
        {
            throw new InvalidOperationException(Resources.FormatTagHelpers_NoProvidedMetadata(
                "<input>",
                ForAttributeName,
                nameof(IModelMetadataProvider),
                For.Name));
        }

        string inputType;
        string inputTypeHint;
        if (string.IsNullOrEmpty(InputTypeName))
        {
            // Note GetInputType never returns null.
            inputType = GetInputType(modelExplorer, out inputTypeHint);
        }
        else
        {
            inputType = InputTypeName.ToLowerInvariant();
            inputTypeHint = null;
        }

        // inputType may be more specific than default the generator chooses below.
        if (!output.Attributes.ContainsName("type"))
        {
            output.Attributes.SetAttribute("type", inputType);
        }

        // Ensure Generator does not throw due to empty "fullName" if user provided a name attribute.
        IDictionary<string, object> htmlAttributes = null;
        if (string.IsNullOrEmpty(For.Name) &&
            string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix) &&
            !string.IsNullOrEmpty(Name))
        {
            htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "name", Name },
                };
        }

        TagBuilder tagBuilder;
        switch (inputType)
        {
            case "hidden":
                tagBuilder = GenerateHidden(modelExplorer, htmlAttributes);
                break;

            case "checkbox":
                tagBuilder = GenerateCheckBox(modelExplorer, output, htmlAttributes);
                break;

            case "password":
                tagBuilder = Generator.GeneratePassword(
                    ViewContext,
                    modelExplorer,
                    For.Name,
                    value: null,
                    htmlAttributes: htmlAttributes);
                break;

            case "radio":
                tagBuilder = GenerateRadio(modelExplorer, htmlAttributes);
                break;

            default:
                tagBuilder = GenerateTextBox(modelExplorer, inputTypeHint, inputType, htmlAttributes);
                break;
        }

        if (tagBuilder != null)
        {
            // This TagBuilder contains the primary <input/> element of interest.
            output.MergeAttributes(tagBuilder);

            if (tagBuilder.Attributes.TryGetValue("name", out var fullName) &&
                ViewContext.FormContext.InvariantField(fullName))
            {
                // If the value attribute used culture-invariant formatting, output a hidden
                // <input/> element so the form submission includes an entry indicating such.
                // This lets the model binding logic decide which CultureInfo to use when parsing form entries.
                GenerateInvariantCultureMetadata(fullName, output.PostElement);
            }

            if (tagBuilder.HasInnerHtml)
            {
                // Since this is not the "checkbox" special-case, no guarantee that output is a self-closing
                // element. A later tag helper targeting this element may change output.TagMode.
                output.Content.AppendHtml(tagBuilder.InnerHtml);
            }
        }
    }

    /// <summary>
    /// Gets an &lt;input&gt; element's "type" attribute value based on the given <paramref name="modelExplorer"/>
    /// or <see cref="InputType"/>.
    /// </summary>
    /// <param name="modelExplorer">The <see cref="ModelExplorer"/> to use.</param>
    /// <param name="inputTypeHint">When this method returns, contains the string, often the name of a
    /// <see cref="ModelMetadata.ModelType"/> base class, used to determine this method's return value.</param>
    /// <returns>An &lt;input&gt; element's "type" attribute value.</returns>
    protected string GetInputType(ModelExplorer modelExplorer, out string inputTypeHint)
    {
        foreach (var hint in GetInputTypeHints(modelExplorer))
        {
            if (_defaultInputTypes.TryGetValue(hint, out var inputType))
            {
                inputTypeHint = hint;
                return inputType;
            }
        }

        inputTypeHint = InputType.Text.ToString().ToLowerInvariant();
        return inputTypeHint;
    }

    private TagBuilder GenerateCheckBox(
        ModelExplorer modelExplorer,
        TagHelperOutput output,
        IDictionary<string, object> htmlAttributes)
    {
        if (modelExplorer.ModelType == typeof(string))
        {
            if (modelExplorer.Model != null)
            {
                if (!bool.TryParse(modelExplorer.Model.ToString(), out _))
                {
                    throw new InvalidOperationException(Resources.FormatInputTagHelper_InvalidStringResult(
                        ForAttributeName,
                        modelExplorer.Model.ToString(),
                        typeof(bool).FullName));
                }
            }
        }
        else if (modelExplorer.ModelType != typeof(bool))
        {
            throw new InvalidOperationException(Resources.FormatInputTagHelper_InvalidExpressionResult(
                   "<input>",
                   ForAttributeName,
                   modelExplorer.ModelType.FullName,
                   typeof(bool).FullName,
                   typeof(string).FullName,
                   "type",
                   "checkbox"));
        }

        if (ViewContext.CheckBoxHiddenInputRenderMode != CheckBoxHiddenInputRenderMode.None)
        {
            // hiddenForCheckboxTag always rendered after the returned element
            var hiddenForCheckboxTag = Generator.GenerateHiddenForCheckbox(ViewContext, modelExplorer, For.Name);
            if (hiddenForCheckboxTag != null)
            {
                var renderingMode =
                    output.TagMode == TagMode.SelfClosing ? TagRenderMode.SelfClosing : TagRenderMode.StartTag;
                hiddenForCheckboxTag.TagRenderMode = renderingMode;
                if (!hiddenForCheckboxTag.Attributes.ContainsKey("name") &&
                    !string.IsNullOrEmpty(Name))
                {
                    // The checkbox and hidden elements should have the same name attribute value. Attributes will
                    // match if both are present because both have a generated value. Reach here in the special case
                    // where user provided a non-empty fallback name.
                    hiddenForCheckboxTag.MergeAttribute("name", Name);
                }

                if (output.Attributes.TryGetAttribute("form", out var formAttribute))
                {
                    // If the original checkbox has a form attribute, the hidden field should respect it and the
                    // attribute should be passed on
                    if (formAttribute.Value is string formAttributeString)
                    {
                        hiddenForCheckboxTag.MergeAttribute("form", formAttributeString);
                    }
                }

                if (ViewContext.CheckBoxHiddenInputRenderMode == CheckBoxHiddenInputRenderMode.EndOfForm && ViewContext.FormContext.CanRenderAtEndOfForm)
                {
                    ViewContext.FormContext.EndOfFormContent.Add(hiddenForCheckboxTag);
                }
                else
                {
                    output.PostElement.AppendHtml(hiddenForCheckboxTag);
                }
            }
        }

        return Generator.GenerateCheckBox(
            ViewContext,
            modelExplorer,
            For.Name,
            isChecked: null,
            htmlAttributes: htmlAttributes);
    }

    private TagBuilder GenerateRadio(ModelExplorer modelExplorer, IDictionary<string, object> htmlAttributes)
    {
        // Note empty string is allowed.
        if (Value == null)
        {
            throw new InvalidOperationException(Resources.FormatInputTagHelper_ValueRequired(
                "<input>",
                nameof(Value).ToLowerInvariant(),
                "type",
                "radio"));
        }

        return Generator.GenerateRadioButton(
            ViewContext,
            modelExplorer,
            For.Name,
            Value,
            isChecked: null,
            htmlAttributes: htmlAttributes);
    }

    private TagBuilder GenerateTextBox(
        ModelExplorer modelExplorer,
        string inputTypeHint,
        string inputType,
        IDictionary<string, object> htmlAttributes)
    {
        var format = Format;
        if (string.IsNullOrEmpty(format))
        {
            if (!modelExplorer.Metadata.HasNonDefaultEditFormat &&
                string.Equals("week", inputType, StringComparison.OrdinalIgnoreCase) &&
                (modelExplorer.Model is DateTime || modelExplorer.Model is DateTimeOffset))
            {
                modelExplorer = modelExplorer.GetExplorerForModel(FormatWeekHelper.GetFormattedWeek(modelExplorer));
            }
            else
            {
                format = GetFormat(modelExplorer, inputTypeHint, inputType);
            }
        }

        if (htmlAttributes == null)
        {
            htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        htmlAttributes["type"] = inputType;
        if (string.Equals(inputType, "file") &&
            string.Equals(
                inputTypeHint,
                TemplateRenderer.IEnumerableOfIFormFileName,
                StringComparison.OrdinalIgnoreCase))
        {
            htmlAttributes["multiple"] = "multiple";
        }

        return Generator.GenerateTextBox(
            ViewContext,
            modelExplorer,
            For.Name,
            modelExplorer.Model,
            format,
            htmlAttributes);
    }

    private static void GenerateInvariantCultureMetadata(string propertyName, TagHelperContent builder)
        => builder
            .AppendHtml("<input name=\"")
            .Append(FormValueHelper.CultureInvariantFieldName)
            .AppendHtml("\" type=\"hidden\" value=\"")
            .Append(propertyName)
            .AppendHtml("\" />");

    // Imitate Generator.GenerateHidden() using Generator.GenerateTextBox(). This adds support for asp-format that
    // is not available in Generator.GenerateHidden().
    private TagBuilder GenerateHidden(ModelExplorer modelExplorer, IDictionary<string, object> htmlAttributes)
    {
        var value = For.Model;
        if (value is byte[] byteArrayValue)
        {
            value = Convert.ToBase64String(byteArrayValue);
        }

        if (htmlAttributes == null)
        {
            htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        // In DefaultHtmlGenerator(), GenerateTextBox() calls GenerateInput() _almost_ identically to how
        // GenerateHidden() does and the main switch inside GenerateInput() handles InputType.Text and
        // InputType.Hidden identically. No behavior differences at all when a type HTML attribute already exists.
        htmlAttributes["type"] = "hidden";

        return Generator.GenerateTextBox(ViewContext, modelExplorer, For.Name, value, Format, htmlAttributes);
    }

    // Get a fall-back format based on the metadata.
    private string GetFormat(ModelExplorer modelExplorer, string inputTypeHint, string inputType)
    {
        string format;
        if (string.Equals("month", inputType, StringComparison.OrdinalIgnoreCase))
        {
            // "month" is a new HTML5 input type that only will be rendered in Rfc3339 mode
            format = "{0:yyyy-MM}";
        }
        else if (string.Equals("decimal", inputTypeHint, StringComparison.OrdinalIgnoreCase) &&
            string.Equals("text", inputType, StringComparison.Ordinal) &&
            string.IsNullOrEmpty(modelExplorer.Metadata.EditFormatString))
        {
            // Decimal data is edited using an <input type="text"/> element, with a reasonable format.
            // EditFormatString has precedence over this fall-back format.
            format = "{0:0.00}";
        }
        else if (ViewContext.Html5DateRenderingMode == Html5DateRenderingMode.Rfc3339 &&
            !modelExplorer.Metadata.HasNonDefaultEditFormat &&
            (typeof(DateTime) == modelExplorer.Metadata.UnderlyingOrModelType ||
             typeof(DateTimeOffset) == modelExplorer.Metadata.UnderlyingOrModelType ||
             typeof(DateOnly) == modelExplorer.Metadata.UnderlyingOrModelType))
        {
            // Rfc3339 mode _may_ override EditFormatString in a limited number of cases. Happens only when
            // EditFormatString has a default format i.e. came from a [DataType] attribute.
            if (string.Equals("text", inputType) &&
                string.Equals(nameof(DateTimeOffset), inputTypeHint, StringComparison.OrdinalIgnoreCase))
            {
                // Auto-select a format that round-trips Offset and sub-Second values in a DateTimeOffset. Not
                // done if user chose the "text" type in .cshtml file or with data annotations i.e. when
                // inputTypeHint==null or "text".
                format = _rfc3339Formats["datetime"];
            }
            else if (_rfc3339Formats.TryGetValue(inputType, out var rfc3339Format))
            {
                format = rfc3339Format;
            }
            else
            {
                // Otherwise use default EditFormatString.
                format = modelExplorer.Metadata.EditFormatString;
            }
        }
        else
        {
            // Otherwise use EditFormatString, if any.
            format = modelExplorer.Metadata.EditFormatString;
        }

        return format;
    }

    // A variant of TemplateRenderer.GetViewNames(). Main change relates to bool? handling.
    private static IEnumerable<string> GetInputTypeHints(ModelExplorer modelExplorer)
    {
        if (!string.IsNullOrEmpty(modelExplorer.Metadata.TemplateHint))
        {
            yield return modelExplorer.Metadata.TemplateHint;
        }

        if (!string.IsNullOrEmpty(modelExplorer.Metadata.DataTypeName))
        {
            yield return modelExplorer.Metadata.DataTypeName;
        }

        // In most cases, we don't want to search for Nullable<T>. We want to search for T, which should handle
        // both T and Nullable<T>. However we special-case bool? to avoid turning an <input/> into a <select/>.
        var fieldType = modelExplorer.ModelType;
        if (typeof(bool?) != fieldType)
        {
            fieldType = modelExplorer.Metadata.UnderlyingOrModelType;
        }

        foreach (var typeName in TemplateRenderer.GetTypeNames(modelExplorer.Metadata, fieldType))
        {
            yield return typeName;
        }
    }
}
