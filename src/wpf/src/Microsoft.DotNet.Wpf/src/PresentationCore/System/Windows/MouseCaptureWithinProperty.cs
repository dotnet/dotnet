﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MS.Internal.KnownBoxes;

namespace System.Windows
{
    /////////////////////////////////////////////////////////////////////////

    internal class MouseCaptureWithinProperty : ReverseInheritProperty
    {
        /////////////////////////////////////////////////////////////////////

        internal MouseCaptureWithinProperty() : base(
            UIElement.IsMouseCaptureWithinPropertyKey,
            CoreFlags.IsMouseCaptureWithinCache,
            CoreFlags.IsMouseCaptureWithinChanged)
        {
        }

        /////////////////////////////////////////////////////////////////////

        internal override void FireNotifications(UIElement uie, ContentElement ce, UIElement3D uie3D, bool oldValue)
        {
            DependencyPropertyChangedEventArgs args = 
                    new DependencyPropertyChangedEventArgs(
                        UIElement.IsMouseCaptureWithinProperty, 
                        BooleanBoxes.Box(oldValue), 
                        BooleanBoxes.Box(!oldValue));
            
            if (uie != null)
            {
                uie.RaiseIsMouseCaptureWithinChanged(args);
            }
            else if (ce != null)
            {
                ce.RaiseIsMouseCaptureWithinChanged(args);
            }
            else
            {
                uie3D?.RaiseIsMouseCaptureWithinChanged(args);
            }
        }
    }
}

