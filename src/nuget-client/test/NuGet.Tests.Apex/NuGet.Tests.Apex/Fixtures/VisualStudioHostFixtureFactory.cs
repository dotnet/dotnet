// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Tests.Apex
{
    public class VisualStudioHostFixtureFactory : IVisualStudioHostFixtureFactory, IDisposable
    {
        private VisualStudioHostFixture _visualStudioHostFxiture;

        public VisualStudioHostFixture GetVisualStudioHostFixture()
        {
            if (_visualStudioHostFxiture == null)
            {
                _visualStudioHostFxiture = new VisualStudioHostFixture();
            }

            return _visualStudioHostFxiture;
        }

        public virtual void Dispose()
        {
            if (_visualStudioHostFxiture != null)
            {
                _visualStudioHostFxiture.Dispose();
            }
        }
    }
}
