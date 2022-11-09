// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.Cci.Writers.Syntax
{
    internal class StyleHelper
    {
        private string _color;
        private string _bgColor;

        public IDisposable SetColor(string color)
        {
            if (_color != null)
                return new DisposeAction(() => { });

            _color = color;
            return new DisposeAction(() => _color = null);
        }

        public IDisposable SetBgColor(string bgColor)
        {
            if (_bgColor != null)
                return new DisposeAction(() => { });

            _bgColor = bgColor;
            return new DisposeAction(() => _bgColor = null);
        }

        public bool HasStyle { get { return _color != null || _bgColor != null; } }

        public string Color { get { return _color; } }

        public string BgColor { get { return _bgColor; } }
    }
}
