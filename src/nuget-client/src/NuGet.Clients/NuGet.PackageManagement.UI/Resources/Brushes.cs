// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Windows;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Color Brushes for multiple elements of the NuGet Package Manager UI
    ///
    /// Initially, the properties are initialized with SystemColors brushes but, later, proper colors are assigned in
    /// another method.
    /// <seealso cref="Brushes.LoadVsBrushes"/>
    /// </summary>
    public static class Brushes
    {
        public static object ActiveBorderKey { get; private set; } = SystemColors.ActiveBorderBrushKey;

        public static object BackgroundBrushKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object BorderBrush { get; private set; } = SystemColors.InactiveBorderBrushKey;

        public static object ComboBoxBorderKey { get; private set; } = SystemColors.InactiveBorderBrushKey;

        public static object ContentInactiveSelectedBrushKey { get; private set; } = SystemColors.ControlTextBrushKey;

        public static object ContentInactiveSelectedTextBrushKey { get; private set; } = SystemColors.ControlTextBrushKey;

        public static object ContentMouseOverBrushKey { get; private set; } = SystemColors.ControlTextBrushKey;

        public static object ContentMouseOverTextBrushKey { get; private set; } = SystemColors.ControlTextBrushKey;

        public static object ContentSelectedBrushKey { get; private set; } = SystemColors.ActiveCaptionBrushKey;

        public static object ContentSelectedTextBrushKey { get; private set; } = SystemColors.ActiveCaptionTextBrushKey;

        public static object ControlLinkTextHoverKey { get; private set; } = SystemColors.HighlightBrushKey;

        public static object ControlLinkTextKey { get; private set; } = SystemColors.HighlightBrushKey;

        public static object DetailPaneBackground { get; private set; } = SystemColors.WindowBrushKey;

        public static object HeaderBackground { get; private set; } = SystemColors.WindowBrushKey;

        public static object HeaderColorsDefaultBrushKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object HeaderColorsDefaultTextBrushKey { get; private set; } = SystemColors.WindowTextBrushKey;

        public static object HeaderColorsMouseDownBrushKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object HeaderColorsMouseDownTextBrushKey { get; private set; } = SystemColors.WindowTextBrushKey;

        public static object HeaderColorsMouseOverBrushKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object HeaderColorsMouseOverTextBrushKey { get; private set; } = SystemColors.WindowTextBrushKey;

        public static object HeaderColorsSeparatorLineBrushKey { get; private set; } = SystemColors.ActiveBorderBrushKey;

        public static object IndicatorFillBrushKey { get; private set; } = SystemColors.WindowFrameColor;

        public static object InfoBackgroundKey { get; private set; } = SystemColors.InfoBrushKey;

        public static object InfoTextKey { get; private set; } = SystemColors.InfoTextBrushKey;

        public static object LegalMessageBackground { get; private set; } = SystemColors.ControlBrushKey;

        public static object ListPaneBackground { get; private set; } = SystemColors.WindowBrushKey;

        public static object SplitterBackgroundKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object ToolWindowBorderKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object ToolWindowButtonDownBorderKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object ToolWindowButtonDownKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object ToolWindowButtonHoverActiveBorderKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object ToolWindowButtonHoverActiveKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object UIText { get; private set; } = SystemColors.ControlTextBrushKey;

        public static object WindowTextKey { get; private set; } = SystemColors.WindowTextBrushKey;

        public static object CheckBoxBackgroundBrushKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object CheckBoxBackgroundDisabledBrushKey { get; private set; } = SystemColors.InactiveBorderBrushKey;

        public static object CheckBoxBackgroundHoverBrushKey { get; private set; } = SystemColors.HighlightBrushKey;

        public static object CheckBoxBackgroundPressedBrushKey { get; private set; } = SystemColors.GradientActiveCaptionBrushKey;

        public static object CheckBoxBorderBrushKey { get; private set; } = SystemColors.ActiveBorderBrushKey;

        public static object CheckBoxBorderDisabledBrushKey { get; private set; } = SystemColors.InactiveBorderBrushKey;

        public static object CheckBoxGlyphBrushKey { get; private set; } = SystemColors.WindowTextBrushKey;

        public static object CheckBoxGlyphHoverBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object CheckBoxGlyphDisabledBrushKey { get; private set; } = SystemColors.GrayTextBrushKey;

        public static object CheckBoxGlyphPressedBrushKey { get; private set; } = SystemColors.WindowBrushKey;

        public static object CheckBoxTextBrushKey { get; private set; } = SystemColors.ControlTextBrushKey;

        public static object CheckBoxTextDisabledBrushKey { get; private set; } = SystemColors.GrayTextBrushKey;

        public static object CheckBoxTextHoverBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object CheckBoxTextPressedBrushKey { get; private set; } = SystemColors.WindowTextBrushKey;

        public static object CheckBoxBorderHoverBrushKey { get; private set; } = SystemColors.HighlightBrushKey;

        public static object CheckBoxBorderPressedBrushKey { get; private set; } = SystemColors.HighlightBrushKey;

        public static object FocusVisualStyleBrushKey { get; private set; } = SystemColors.ControlTextBrushKey;

        public static object ButtonTextStyleBrushKey { get; private set; } = SystemColors.ControlTextBrushKey;

        public static object ButtonBorderBrushKey { get; private set; } = SystemColors.ActiveBorderBrushKey;

        public static object ButtonDisabledTextStyleBrushKey { get; private set; } = SystemColors.GrayTextBrush;

        public static object ButtonBackgroundStyleBrushKey { get; private set; } = SystemColors.ControlBrushKey;

        public static object ButtonDisabledStyleBrushKey { get; private set; } = SystemColors.InactiveBorderBrushKey;

        public static object ButtonPressedStyleBrushKey { get; private set; } = SystemColors.InactiveBorderBrushKey;

        public static object ButtonPressedBorderStyleBrushKey { get; private set; } = SystemColors.InactiveBorderBrushKey;

        public static object ButtonPressedTextStyleBrushKey { get; private set; } = SystemColors.WindowTextBrushKey;

        public static object ButtonHoverBorderStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object ButtonHoverStyleBrushKey { get; private set; } = SystemColors.HighlightBrushKey;

        public static object ButtonHoverTextStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object ButtonDisabledBorderStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object ButtonDefaultStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object ButtonDefaultBorderStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object ButtonDefaultTextStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object ButtonFocusedTextStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object ButtonBorderFocusedStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object ButtonFocusedStyleBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object GridSplitterFocusBrushKey { get; private set; } = SystemColors.HighlightBrushKey;

        public static object TabSelectedIndicatorBrushKey { get; private set; } = SystemColors.ActiveCaptionTextColor;

        public static object TabSelectedTextBrushKey { get; private set; } = SystemColors.ActiveCaptionTextColorKey;

        public static object TabPopupBrushKey { get; private set; } = SystemColors.HighlightBrushKey;

        public static object TabPopupTextBrushKey { get; private set; } = SystemColors.HighlightTextBrushKey;

        public static object TabTextHoverBrushKey { get; private set; } = SystemColors.HotTrackBrushKey;

        public static object TabTextFocusedBrushKey { get; private set; } = SystemColors.HotTrackBrushKey;

        public static object ListItemBackgroundSelectedColorKey { get; private set; } = SystemColors.HighlightColorKey;

        public static object ListItemTextSelectedColorKey { get; private set; } = SystemColors.HighlightTextColorKey;

        public static void LoadVsBrushes(INuGetExperimentationService nuGetExperimentationService)
        {
            if (nuGetExperimentationService == null)
            {
                throw new ArgumentNullException(nameof(nuGetExperimentationService));
            }

            bool isBgColorFlightEnabled = IsBackgroundColorFlightEnabled(nuGetExperimentationService);

            FocusVisualStyleBrushKey = VsBrushes.ToolWindowTextKey;
            ActiveBorderKey = VsBrushes.ActiveBorderKey;
            BorderBrush = VsBrushes.BrandedUIBorderKey;
            ComboBoxBorderKey = VsBrushes.ComboBoxBorderKey;
            ControlLinkTextHoverKey = VsBrushes.ControlLinkTextHoverKey;
            ControlLinkTextKey = VsBrushes.ControlLinkTextKey;
            DetailPaneBackground = isBgColorFlightEnabled ? CommonDocumentColors.PageBrushKey : VsBrushes.BrandedUIBackgroundKey;
            HeaderBackground = isBgColorFlightEnabled ? CommonDocumentColors.PageBrushKey : VsBrushes.BrandedUIBackgroundKey;
            InfoBackgroundKey = VsBrushes.InfoBackgroundKey;
            InfoTextKey = VsBrushes.InfoTextKey;
            LegalMessageBackground = isBgColorFlightEnabled ? CommonDocumentColors.PageBrushKey : VsBrushes.BrandedUIBackgroundKey;
            ListPaneBackground = isBgColorFlightEnabled ? CommonDocumentColors.PageBrushKey : VsBrushes.BrandedUIBackgroundKey;
            SplitterBackgroundKey = VsBrushes.CommandShelfBackgroundGradientKey;
            ToolWindowBorderKey = VsBrushes.ToolWindowBorderKey;
            ToolWindowButtonDownBorderKey = VsBrushes.ToolWindowButtonDownBorderKey;
            ToolWindowButtonDownKey = VsBrushes.ToolWindowButtonDownKey;
            ToolWindowButtonHoverActiveBorderKey = VsBrushes.ToolWindowButtonHoverActiveBorderKey;
            ToolWindowButtonHoverActiveKey = VsBrushes.ToolWindowButtonHoverActiveKey;
            UIText = isBgColorFlightEnabled ? CommonDocumentColors.PageTextBrushKey : VsBrushes.BrandedUITextKey;
            WindowTextKey = VsBrushes.WindowTextKey;

            HeaderColorsDefaultBrushKey = HeaderColors.DefaultBrushKey;
            HeaderColorsDefaultTextBrushKey = HeaderColors.DefaultTextBrushKey;
            HeaderColorsMouseDownBrushKey = HeaderColors.MouseDownBrushKey;
            HeaderColorsMouseDownTextBrushKey = HeaderColors.MouseDownTextBrushKey;
            HeaderColorsMouseOverBrushKey = HeaderColors.MouseOverBrushKey;
            HeaderColorsMouseOverTextBrushKey = HeaderColors.MouseOverTextBrushKey;
            HeaderColorsSeparatorLineBrushKey = HeaderColors.SeparatorLineBrushKey;

            IndicatorFillBrushKey = ProgressBarColors.IndicatorFillBrushKey;

            ButtonTextStyleBrushKey = CommonControlsColors.ButtonTextBrushKey;
            ButtonBorderBrushKey = CommonControlsColors.ButtonBorderBrushKey;
            ButtonBackgroundStyleBrushKey = CommonControlsColors.ButtonBrushKey;
            ButtonDisabledTextStyleBrushKey = CommonControlsColors.ButtonDisabledTextBrushKey;
            ButtonDisabledStyleBrushKey = CommonControlsColors.ButtonDisabledBrushKey;
            ButtonDisabledBorderStyleBrushKey = CommonControlsColors.ButtonBorderDisabledBrushKey;
            ButtonDefaultStyleBrushKey = CommonControlsColors.ButtonDefaultBrushKey;
            ButtonDefaultBorderStyleBrushKey = CommonControlsColors.ButtonBorderDefaultBrushKey;
            ButtonDefaultTextStyleBrushKey = CommonControlsColors.ButtonDefaultTextBrushKey;
            ButtonPressedStyleBrushKey = CommonControlsColors.ButtonPressedBrushKey;
            ButtonPressedBorderStyleBrushKey = CommonControlsColors.ButtonBorderPressedBrushKey;
            ButtonPressedTextStyleBrushKey = CommonControlsColors.ButtonPressedTextBrushKey;
            ButtonHoverBorderStyleBrushKey = CommonControlsColors.ButtonBorderHoverBrushKey;
            ButtonHoverStyleBrushKey = CommonControlsColors.ButtonHoverBrushKey;
            ButtonHoverTextStyleBrushKey = CommonControlsColors.ButtonHoverTextBrushKey;
            CheckBoxBackgroundBrushKey = CommonControlsColors.CheckBoxBackgroundBrushKey;
            CheckBoxBackgroundDisabledBrushKey = CommonControlsColors.CheckBoxBackgroundDisabledBrushKey;
            CheckBoxBackgroundHoverBrushKey = CommonControlsColors.CheckBoxBackgroundHoverBrushKey;
            CheckBoxBackgroundPressedBrushKey = CommonControlsColors.CheckBoxBackgroundPressedBrushKey;
            CheckBoxBorderBrushKey = CommonControlsColors.CheckBoxBorderBrushKey;
            CheckBoxBorderDisabledBrushKey = CommonControlsColors.CheckBoxBorderDisabledBrushKey;
            CheckBoxGlyphBrushKey = CommonControlsColors.CheckBoxGlyphBrushKey;
            CheckBoxGlyphHoverBrushKey = CommonControlsColors.CheckBoxGlyphHoverBrushKey;
            CheckBoxGlyphDisabledBrushKey = CommonControlsColors.CheckBoxGlyphDisabledBrushKey;
            CheckBoxGlyphPressedBrushKey = CommonControlsColors.CheckBoxGlyphPressedBrushKey;
            CheckBoxTextBrushKey = CommonControlsColors.CheckBoxTextBrushKey;
            CheckBoxTextDisabledBrushKey = CommonControlsColors.CheckBoxTextDisabledBrushKey;
            CheckBoxTextHoverBrushKey = CommonControlsColors.CheckBoxTextHoverBrushKey;
            CheckBoxTextPressedBrushKey = CommonControlsColors.CheckBoxTextPressedBrushKey;
            CheckBoxBorderHoverBrushKey = CommonControlsColors.CheckBoxBorderHoverBrushKey;
            CheckBoxBorderPressedBrushKey = CommonControlsColors.CheckBoxBorderPressedBrushKey;

            BackgroundBrushKey = EnvironmentColors.ToolWindowBackgroundBrushKey;

            // Brushes/Colors for InfiniteScrollList
            ContentMouseOverBrushKey = CommonDocumentColors.ListItemBackgroundHoverBrushKey;
            ContentMouseOverTextBrushKey = CommonDocumentColors.ListItemTextHoverBrushKey;
            ContentInactiveSelectedBrushKey = CommonDocumentColors.ListItemBackgroundUnfocusedBrushKey;
            ContentInactiveSelectedTextBrushKey = CommonDocumentColors.ListItemTextUnfocusedBrushKey;
            ContentSelectedBrushKey = CommonDocumentColors.ListItemBackgroundSelectedBrushKey;
            ContentSelectedTextBrushKey = CommonDocumentColors.ListItemTextSelectedBrushKey;

            // Brushes/Colors for FilterLabel (Top Tabs)
            TabSelectedIndicatorBrushKey = CommonDocumentColors.InnerTabSelectedIndicatorBrushKey; // underline
            TabSelectedTextBrushKey = CommonDocumentColors.InnerTabSelectedTextBrushKey; // text
            TabTextHoverBrushKey = CommonDocumentColors.InnerTabTextHoverBrushKey; //text hover
            TabTextFocusedBrushKey = CommonDocumentColors.InnerTabTextFocusedBrushKey;

            // Mapping color keys directly for use to create brushes using these colors
            ListItemBackgroundSelectedColorKey = CommonDocumentColors.ListItemBackgroundSelectedColorKey;
            ListItemTextSelectedColorKey = CommonDocumentColors.ListItemTextSelectedColorKey;
        }

        private static bool IsBackgroundColorFlightEnabled(INuGetExperimentationService nuGetExperimentationService) =>
            nuGetExperimentationService.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor);
    }
}
