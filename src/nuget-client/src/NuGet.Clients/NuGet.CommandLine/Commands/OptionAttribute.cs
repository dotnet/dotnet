using System;
using NuGet.CommandLine;

namespace NuGet
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class OptionAttribute : Attribute
    {
        private string _description;

        public bool IsHidden { get; set; }
        public string AltName { get; set; }
        public string DescriptionResourceName { get; private set; }

        public string Description
        {
            get
            {
                if (ResourceType != null && !String.IsNullOrEmpty(DescriptionResourceName))
                {
                    return ResourceHelper.GetLocalizedString(ResourceType, DescriptionResourceName);
                }
                return _description;

            }
            private set
            {
                _description = value;
            }
        }

        public Type ResourceType { get; private set; }

        public OptionAttribute(string description)
        {
            Description = description;
            IsHidden = false;
        }

        public OptionAttribute(Type resourceType, string descriptionResourceName)
        {
            ResourceType = resourceType;
            DescriptionResourceName = descriptionResourceName;
            IsHidden = false;
        }
        public OptionAttribute(string description, bool isHidden)
            : this(description)
        {
            IsHidden = isHidden;
        }

        public OptionAttribute(Type resourceType, string descriptionResourceName, bool isHidden)
            : this(resourceType, descriptionResourceName)
        {
            IsHidden = isHidden;
        }
    }
}
