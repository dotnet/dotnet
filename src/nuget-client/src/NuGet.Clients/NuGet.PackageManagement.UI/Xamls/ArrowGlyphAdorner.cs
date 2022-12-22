// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Adorner used to indicate direction of list sort
    /// </summary>
    internal class ArrowGlyphAdorner : Adorner
    {
        private readonly GridViewColumnHeader _columnHeader;
        private readonly ListSortDirection _direction;

        public ArrowGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction) : base(columnHeader)
        {
            _columnHeader = columnHeader;
            _direction = direction;
        }

        private Geometry GetDefaultGlyph()
        {
            var x1 = _columnHeader.ActualWidth - 13;
            var x2 = x1 + 7;
            var x3 = x1 + 3.5;
            var y1 = _columnHeader.ActualHeight / 2 - 3;
            var y2 = y1 + 3.5;

            if (_direction == ListSortDirection.Ascending)
            {
                var tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            var pathSegmentCollection = new PathSegmentCollection();
            pathSegmentCollection.Add(new LineSegment(new Point(x3, y2), isStroked: true));
            pathSegmentCollection.Add(new LineSegment(new Point(x2, y1), isStroked: true));

            var pathFigure = new PathFigure(start: new Point(x1, y1), segments: pathSegmentCollection, closed: false);

            var pathGeometry = new PathGeometry() { Figures = { pathFigure } };
            return pathGeometry;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var drawingColor = VSColorTheme.GetThemedColor(EnvironmentColors.BrandedUITextBrushKey);
            var mediaColor = Color.FromRgb(drawingColor.R, drawingColor.G, drawingColor.B);
            drawingContext.DrawGeometry(brush: null, pen: new Pen(new SolidColorBrush(mediaColor), 1.0), geometry: GetDefaultGlyph());
        }
    }
}
