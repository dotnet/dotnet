// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsTarget.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.NLogTarget
{
    using NLog.Config;
    using NLog.Layouts;

    /// <summary>
    /// NLog Target Context Property that allows capture of context information for all logevents (Ex. Layout=${threadid}).
    /// </summary>
    [NLogConfigurationItem]
    [ThreadAgnostic]
    public class TargetPropertyWithContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetPropertyWithContext" /> class.
        /// </summary>
        public TargetPropertyWithContext() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetPropertyWithContext" /> class.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="layout">The layout of the attribute's value.</param>
        public TargetPropertyWithContext(string name, Layout layout)
        {
            this.Name = name;
            this.Layout = layout;
        }

        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        /// <docgen category='Property Options' order='10' />
        [RequiredParameter]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layout that will be rendered as the attribute's value.
        /// </summary>
        /// <docgen category='Property Options' order='10' />
        [RequiredParameter]
        public Layout Layout { get; set; }
    }
}
