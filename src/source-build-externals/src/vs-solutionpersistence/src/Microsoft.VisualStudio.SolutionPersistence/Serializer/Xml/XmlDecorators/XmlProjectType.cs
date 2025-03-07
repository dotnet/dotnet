// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child of a Solution that represents a project type not implicitly know about.
/// Allows the file to specify a friendly name or associate and extension with a project type guid.
/// </summary>
internal sealed class XmlProjectType(SlnxFile root, XmlElement element) :
    XmlContainer(root, element, Keyword.ProjectType),
    IItemRefDecorator
{
    private ItemConfigurationRulesList configurationRules = new ItemConfigurationRulesList();

    public Keyword ItemRefAttribute => Keyword.TypeId;

    /// <inheritdoc cref="ProjectType.ProjectTypeId"/>
    internal Guid TypeId
    {
        get => this.GetXmlAttributeGuid(Keyword.TypeId);
        set => this.UpdateXmlAttributeGuid(Keyword.TypeId, value);
    }

    /// <inheritdoc cref="ProjectType.Name"/>
    internal string? Name
    {
        get => this.GetXmlAttribute(Keyword.Name);
        set => this.UpdateXmlAttribute(Keyword.Name, value);
    }

    /// <inheritdoc cref="ProjectType.Extension"/>
    internal string? Extension
    {
        get => this.GetXmlAttribute(Keyword.Extension);
        set => this.UpdateXmlAttribute(Keyword.Extension, value);
    }

    /// <inheritdoc cref="ProjectType.BasedOn"/>
    internal string? BasedOn
    {
        get => this.GetXmlAttribute(Keyword.BasedOn);
        set => this.UpdateXmlAttribute(Keyword.BasedOn, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the project type is buildable.
    /// </summary>
    /// <remarks>
    /// Default is <see langword="true"/>.
    /// When <see langword="false"/> automatically sets configuration rules to never build.
    /// </remarks>
    internal bool IsBuildable
    {
        get => this.GetXmlAttributeBool(Keyword.IsBuildable, defaultValue: true);
        set => this.UpdateXmlAttributeBool(Keyword.IsBuildable, value, defaultValue: true);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the project type supports platform configurations.
    /// </summary>
    /// <remarks>
    /// Default is <see langword="true"/>.
    /// When <see langword="false"/> automatically adds configuration rule to remove platform mappings.
    /// This setting is ignored if <see cref="IsBuildable"/> is <see langword="false"/>.
    /// </remarks>
    internal bool SupportsPlatform
    {
        get => this.GetXmlAttributeBool(Keyword.SupportsPlatform, defaultValue: true);
        set => this.UpdateXmlAttributeBool(Keyword.SupportsPlatform, value, defaultValue: true);
    }

    private protected override bool AllowEmptyItemRef => true;

    /// <summary>
    /// Gets or sets although every project type should have a TypeId, there may be multiple project types with the same TypeId.
    /// So use the Name and TypeId to uniquely identify a project type.
    /// </summary>
    private protected override string RawItemRef
    {
        get => GetItemRef(this.Name, this.Extension, this.TypeId);
        set
        {
            if (value.IsNullOrEmpty())
            {
                this.Name = null;
                this.Extension = null;
                this.TypeId = Guid.Empty;
            }
            else if (value.EndsWith('⁂'))
            {
                this.Name = null;
                this.Extension = value.Substring(0, value.Length - 1);
            }
            else
            {
                this.Name = value;
            }
        }
    }

    internal static string GetItemRef(string? name, string? extension, Guid typeId)
    {
        // Return empty string for default project type ItemRef.
        return name is null && extension is null && typeId == Guid.Empty ?
            string.Empty :
            name ?? $"{extension}⁂";
    }

    /// <inheritdoc/>
    internal override XmlDecorator? ChildDecoratorFactory(XmlElement element, Keyword elementName)
    {
        return elementName switch
        {
            Keyword.BuildType => new XmlConfigurationBuildType(this.Root, element),
            Keyword.Platform => new XmlConfigurationPlatform(this.Root, element),
            Keyword.Build => new XmlConfigurationBuild(this.Root, element),
            Keyword.Deploy => new XmlConfigurationDeploy(this.Root, element),
            _ => base.ChildDecoratorFactory(element, elementName),
        };
    }

    /// <inheritdoc/>
    internal override void OnNewChildDecoratorAdded(XmlDecorator childDecorator)
    {
        switch (childDecorator)
        {
            case XmlConfiguration configuration:
                this.configurationRules.Add(configuration);
                break;
        }

        base.OnNewChildDecoratorAdded(childDecorator);
    }

    /// <inheritdoc/>
    internal override XmlDecorator? FindNextDecorator<TDecorator>()
    {
        return this.configurationRules.FindNextDecorator<TDecorator>();
    }

    internal override bool IsValid()
    {
        return base.IsValid();
    }

    internal ProjectType ToModel()
    {
        ConfigurationRule[] rules =
            !this.IsBuildable ? ProjectTypeTable.NoBuildRules :
            !this.SupportsPlatform ? [ProjectTypeTable.NoPlatformsRule, .. this.configurationRules.ToModel()] :
            /*default*/ [.. this.configurationRules.ToModel()];

        return new ProjectType(this.TypeId, rules)
        {
            Name = this.GetTableString(this.Name),
            Extension = this.Extension,
            BasedOn = this.BasedOn,
        };
    }

    // Update the Xml DOM with changes from the model.
    internal bool ApplyModelToXml(ProjectType modelProjectType)
    {
        bool modified = false;
        if (!StringComparer.Ordinal.Equals(this.Name, modelProjectType.Name))
        {
            this.Name = modelProjectType.Name;
            modified = true;
        }

        if (!StringComparer.Ordinal.Equals(this.Extension, modelProjectType.Extension))
        {
            this.Extension = modelProjectType.Extension;
            modified = true;
        }

        if (this.TypeId != modelProjectType.ProjectTypeId)
        {
            this.TypeId = modelProjectType.ProjectTypeId;
            modified = true;
        }

        if (this.BasedOn != modelProjectType.BasedOn)
        {
            this.BasedOn = modelProjectType.BasedOn;
            modified = true;
        }

        ConfigurationRuleFollower rules = new ConfigurationRuleFollower(modelProjectType.ConfigurationRules);
        bool isBuildable = rules.GetIsBuildable() ?? true;
        bool supportsPlatform = rules.GetProjectPlatform() != PlatformNames.Missing;

        if (this.IsBuildable != isBuildable)
        {
            this.IsBuildable = isBuildable;
            modified = true;
        }

        if (this.SupportsPlatform != supportsPlatform)
        {
            this.SupportsPlatform = supportsPlatform;
            modified = true;
        }

        // Determine which rules to serizlize. Remove rules implied by IsBuildable and SupportsPlatform.
        IReadOnlyList<ConfigurationRule>? rulesToApply =
            !isBuildable ? [] :
            !supportsPlatform ? RemovePlatformRules(modelProjectType.ConfigurationRules) :
            modelProjectType.ConfigurationRules;

        modified |= this.configurationRules.ApplyModelToXml(this, rulesToApply);
        return modified;

        // Remove any platform rules from the list.
        static List<ConfigurationRule> RemovePlatformRules(IReadOnlyList<ConfigurationRule> rules) =>
            rules.WhereToList(
                predicate: static (rule, _) => rule.Dimension != BuildDimension.Platform,
                selector: static (rule, _) => rule,
                (object?)null);
    }
}
