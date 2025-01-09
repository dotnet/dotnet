// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Helper to serialize all of the different types of configuration rules.
/// This is used to share logic for ProjectTypes and Projects.
/// </summary>
internal struct ItemConfigurationRulesList
{
    private ItemRefList<XmlConfigurationBuildType> buildTypeRules = new(ignoreCase: true);
    private ItemRefList<XmlConfigurationPlatform> platformRules = new(ignoreCase: true);
    private ItemRefList<XmlConfigurationBuild> buildRules = new(ignoreCase: true);
    private ItemRefList<XmlConfigurationDeploy> deployRules = new(ignoreCase: true);

    public ItemConfigurationRulesList()
    {
    }

    internal readonly void Add(XmlConfiguration configuration)
    {
        switch (configuration)
        {
            case XmlConfigurationBuildType buildType:
                this.buildTypeRules.Add(buildType);
                break;
            case XmlConfigurationPlatform platform:
                this.platformRules.Add(platform);
                break;
            case XmlConfigurationBuild build:
                this.buildRules.Add(build);
                break;
            case XmlConfigurationDeploy deploy:
                this.deployRules.Add(deploy);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    internal readonly XmlDecorator? FindNextDecorator<TDecorator>()
    {
        return typeof(TDecorator).Name switch
        {
            nameof(XmlConfigurationBuildType) or nameof(XmlConfiguration) => this.platformRules.FirstOrDefault() ?? this.FindNextDecorator<XmlConfigurationPlatform>(),
            nameof(XmlConfigurationPlatform) => this.buildRules.FirstOrDefault() ?? this.FindNextDecorator<XmlConfigurationBuild>(),
            nameof(XmlConfigurationBuild) => this.deployRules.FirstOrDefault(),
            nameof(XmlConfigurationDeploy) => null,
            _ => null,
        };
    }

    internal readonly XmlDecorator? FirstOrDefault()
    {
        return this.buildTypeRules.FirstOrDefault() ?? this.platformRules.FirstOrDefault() ?? this.buildRules.FirstOrDefault() ?? (XmlDecorator?)this.deployRules.FirstOrDefault();
    }

    internal bool ApplyModelToXml(XmlContainer xmlContainer, IReadOnlyList<ConfigurationRule>? configurationRules)
    {
        bool modified = false;

        configurationRules ??= [];
        modified |= ApplyModelToXml(xmlContainer, configurationRules, BuildDimension.BuildType, Keyword.BuildType, ref this.buildTypeRules);
        modified |= ApplyModelToXml(xmlContainer, configurationRules, BuildDimension.Platform, Keyword.Platform, ref this.platformRules);
        modified |= ApplyModelToXml(xmlContainer, configurationRules, BuildDimension.Build, Keyword.Build, ref this.buildRules);
        modified |= ApplyModelToXml(xmlContainer, configurationRules, BuildDimension.Deploy, Keyword.Deploy, ref this.deployRules);
        return modified;

        static bool ApplyModelToXml<T>(XmlContainer xmlContainer, IReadOnlyList<ConfigurationRule> configurationRules, BuildDimension dimension, Keyword dimensionElementName, ref ItemRefList<T> configurations)
            where T : XmlConfiguration
        {
            List<(string ItemRef, ConfigurationRule Item)> dimensionRules = configurationRules.WhereToList(
                static (x, dimension) => x.Dimension == dimension,
                static (x, _) => (ItemRef: x.GetSolutionConfiguration(), Item: x),
                dimension);

            return xmlContainer.ApplyModelItemsToXml(
                modelItems: dimensionRules,
                decoratorItems: ref configurations,
                decoratorElementName: dimensionElementName,
                applyModelToXml: static (newConfiguration, modelConfiguration) => newConfiguration.ApplyModelToXml(modelConfiguration));
        }
    }

    internal readonly List<ConfigurationRule> ToModel()
    {
        List<ConfigurationRule> rules = new List<ConfigurationRule>(
            this.buildTypeRules.ItemsCount +
            this.platformRules.ItemsCount +
            this.buildRules.ItemsCount +
            this.deployRules.ItemsCount);

        foreach (XmlConfiguration configuration in this.buildTypeRules.GetItems())
        {
            AddRule(rules, configuration);
        }

        foreach (XmlConfiguration configuration in this.platformRules.GetItems())
        {
            AddRule(rules, configuration);
        }

        foreach (XmlConfiguration configuration in this.buildRules.GetItems())
        {
            AddRule(rules, configuration);
        }

        foreach (XmlConfiguration configuration in this.deployRules.GetItems())
        {
            AddRule(rules, configuration);
        }

        return rules;

        static void AddRule(List<ConfigurationRule> rules, XmlConfiguration configuration)
        {
            ConfigurationRule? rule = configuration.ToModel();
            if (rule is not null)
            {
                rules.Add(rule.Value);
            }
        }
    }
}
